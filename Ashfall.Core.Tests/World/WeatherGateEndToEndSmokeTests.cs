using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Task F8 — End-to-End Weather Gate Lifecycle Smoke Test.
    /// Proves authoritative catalog load, mountain pass blizzard block,
    /// clear open, frozen lake positive gate allow/close, biofog gas mask override,
    /// underpass black rain block, save/restore continuity, and weather transition.
    /// </summary>
    public sealed class WeatherGateEndToEndSmokeTests
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly IJsonSerializer _serializer;

        public WeatherGateEndToEndSmokeTests()
        {
            _dataDir = WeatherGateAuditSimulator.FindDataDir();
            _fileIO = new FileSystemIO();
            _serializer = new SystemTextJsonSerializer();
        }

        [Fact]
        public void FullWeatherGateLifecycle_EndToEnd_ProvesAllBehaviors()
        {
            // 1. Authoritative production catalog load
            Assert.False(string.IsNullOrEmpty(_dataDir), "StreamingAssets/Data directory must exist");
            var catalog = WeatherGateCatalogLoader.LoadFromDirectory(_dataDir, _fileIO, _serializer);

            Assert.True(catalog.IsValid, $"Expected valid catalog, but had errors: {string.Join("; ", catalog.Errors)}");
            Assert.Empty(catalog.Errors);
            Assert.Equal(18, catalog.Count);
            Assert.Equal(15, System.Linq.Enumerable.Count(catalog.GetAll(), g => g.GateType == "route"));
            Assert.Equal(3, System.Linq.Enumerable.Count(catalog.GetAll(), g => g.GateType == "destination"));

            var evaluator = new WeatherGateEvaluator(catalog);

            // 2. Mountain pass — Blizzard blocks with zero override available
            var mountainBlizzard = evaluator.EvaluateLive("gate_mountain_pass_blizzard", WeatherKind.Blizzard);
            Assert.True(mountainBlizzard.IsBlocked);
            Assert.False(mountainBlizzard.IsOpen);
            Assert.False(mountainBlizzard.OverrideAvailable);
            Assert.Contains("impassable", mountainBlizzard.Reason);

            // 3. Mountain pass — Clear is passable
            var mountainClear = evaluator.EvaluateLive("gate_mountain_pass_blizzard", WeatherKind.Clear);
            Assert.False(mountainClear.IsBlocked);
            Assert.True(mountainClear.IsOpen);

            // 4. Frozen lake — Positive weather gate: Blizzard permits, Clear blocks
            var lakeBlizzard = evaluator.EvaluateLive("gate_frozen_lake_crossing", WeatherKind.Blizzard);
            Assert.False(lakeBlizzard.IsBlocked);
            Assert.True(lakeBlizzard.IsOpen);
            Assert.True(lakeBlizzard.IsPositiveGate);

            var lakeClear = evaluator.EvaluateLive("gate_frozen_lake_crossing", WeatherKind.Clear);
            Assert.True(lakeClear.IsBlocked);
            Assert.False(lakeClear.IsOpen);
            Assert.True(lakeClear.IsPositiveGate);

            // 5. Lowland marsh — BioFog with gas mask override allows passage; without gas mask is blocked
            var marshWithMask = evaluator.EvaluateLive("gate_lowland_marsh_fog", WeatherKind.BioFog, new[] { "gas_mask" });
            Assert.False(marshWithMask.IsBlocked);
            Assert.True(marshWithMask.IsOpen);
            Assert.True(marshWithMask.OverrideAvailable);
            Assert.Contains("override_used:gas_mask", marshWithMask.Reason);

            var marshWithoutMask = evaluator.EvaluateLive("gate_lowland_marsh_fog", WeatherKind.BioFog, new[] { "item_rope" });
            Assert.True(marshWithoutMask.IsBlocked);
            Assert.False(marshWithoutMask.IsOpen);
            Assert.True(marshWithoutMask.OverrideAvailable);

            // 6. Underpass — Black Rain blocks with no override
            var underpassBlackRain = evaluator.EvaluateLive("gate_underpass_black_rain", WeatherKind.BlackRain);
            Assert.True(underpassBlackRain.IsBlocked);
            Assert.False(underpassBlackRain.IsOpen);
            Assert.False(underpassBlackRain.OverrideAvailable);

            // 7. Save / Restore continuity and determinism
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "test_season" }, 777);
            weather.ForceWeather(WeatherKind.Blizzard);

            var stateBefore = weather.CaptureState();
            string json = _serializer.Serialize(stateBefore);

            var weatherRestored = new WeatherSystem();
            weatherRestored.BindProfile(new SeasonProfileDef { id = "test_season" }, 777);
            var restoredState = _serializer.Deserialize<WorldWeatherState>(json);
            Assert.NotNull(restoredState);
            weatherRestored.RestoreState(restoredState);

            Assert.Equal(weather.Current, weatherRestored.Current);
            Assert.Equal(WeatherKind.Blizzard, weatherRestored.Current);
            Assert.Equal(stateBefore.rollCount, weatherRestored.CaptureState().rollCount);

            // Re-evaluate mountain pass after restore
            var evalRestored = evaluator.EvaluateLive("gate_mountain_pass_blizzard", weatherRestored.Current);
            Assert.Equal(mountainBlizzard.IsOpen, evalRestored.IsOpen);
            Assert.Equal(mountainBlizzard.IsBlocked, evalRestored.IsBlocked);
            Assert.Equal(mountainBlizzard.Reason, evalRestored.Reason);

            // 8. Weather transition — changing weather dynamically flips gate state
            weatherRestored.ForceWeather(WeatherKind.Clear);
            var evalTransitioned = evaluator.EvaluateLive("gate_mountain_pass_blizzard", weatherRestored.Current);
            Assert.False(evalTransitioned.IsBlocked);
            Assert.True(evalTransitioned.IsOpen);
        }
    }
}
