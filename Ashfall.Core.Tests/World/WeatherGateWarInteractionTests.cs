using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Task F17: Weather gate interaction with faction war state tests.
    /// Verifies wartime route pressure contextualization, passability authority,
    /// encounter weight adjustments, and determinism.
    /// </summary>
    public class WeatherGateWarInteractionTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static WeatherGateCatalog LoadCatalog()
        {
            var catalog = WeatherGateFile.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog);
            return catalog!;
        }

        [Fact]
        public void Route12_Blizzard_WarHostile_AppliesWartimeEncounterAndSeverity()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));
            Assert.NotNull(gate);

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 80,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true),
                Territory = new TerritorySnapshot(TerritoryControlState.Controlled, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // Weather authority: blocked by blizzard
            Assert.True(result.IsBlocked);
            Assert.Contains("blizzard", result.BlockedReason, StringComparison.OrdinalIgnoreCase);

            // Wartime modifier: 1.5x severity, encounter tag, weight 1.75x
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
            Assert.Equal("warlord_checkpoint", result.FactionEncounterTag);
            Assert.Equal(1.75f, result.FactionEncounterWeightMultiplier);
            Assert.Contains("war_hostile_tension_80", result.AppliedContextReasons);
        }

        [Fact]
        public void Route12_Blizzard_Peacetime_HasNoWarModifier()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: false,
                    ActiveWarTension: 80,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true),
                Territory = new TerritorySnapshot(TerritoryControlState.Contested, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.Equal(string.Empty, result.FactionEncounterTag);
            Assert.Equal(1.0f, result.FactionEncounterWeightMultiplier);
            Assert.DoesNotContain(result.AppliedContextReasons, r => r.StartsWith("war_"));
        }

        [Fact]
        public void Route12_Clear_WarHostile_RemainsWeatherOpen_WithElevatedEncounterWeight()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Clear,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 75,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true),
                Territory = new TerritorySnapshot(TerritoryControlState.Controlled, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // Passability authority: route is NOT blocked because weather is Clear
            Assert.False(result.IsBlocked);
            Assert.Empty(result.BlockedReason);

            // War encounter pressure is active on the corridor
            Assert.Equal("warlord_checkpoint", result.FactionEncounterTag);
            Assert.Equal(1.75f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void Route12_Clear_Peacetime_IsOpen_WithDefaultEncounterWeight()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Clear,
                War = new FactionWarSnapshot(
                    IsAtWar: false,
                    ActiveWarTension: 0,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: false),
                Territory = new TerritorySnapshot(TerritoryControlState.Controlled, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.False(result.IsBlocked);
            Assert.Equal(string.Empty, result.FactionEncounterTag);
            Assert.Equal(1.0f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void WarModifier_TensionBelowThreshold_DoesNotApply()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            // Min tension on gate_mountain_pass_blizzard is 50; test with 49
            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 49,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true)
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.Equal(string.Empty, result.FactionEncounterTag);
            Assert.Equal(1.0f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void WarModifier_TensionAtThreshold_Applies()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            // Min tension on gate_mountain_pass_blizzard is 50; test with exactly 50
            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 50,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true)
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.Equal("warlord_checkpoint", result.FactionEncounterTag);
            Assert.Equal(1.75f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void WarModifier_NonHostileDominantFaction_HostileOnly_DoesNotApply()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 90,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: false) // Not hostile!
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.Equal(string.Empty, result.FactionEncounterTag);
            Assert.Equal(1.0f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void GateWithoutWarModifier_PreservesLegacyBehavior()
        {
            var plainGate = new WeatherGate
            {
                Id = "gate_plain",
                TargetId = "route_plain",
                BlockedWeather = new List<string> { "Blizzard" }
            };

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_plain",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 100,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true)
            };

            var result = WeatherGateContextEvaluator.Evaluate(plainGate, context);

            Assert.True(result.IsBlocked);
            Assert.Equal(1.0f, result.ConsequenceSeverityMultiplier);
            Assert.Equal(string.Empty, result.FactionEncounterTag);
            Assert.Equal(1.0f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void WarModifier_IsDeterministic_AcrossIdenticalCalls()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 80,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true)
            };

            var res1 = WeatherGateContextEvaluator.Evaluate(gate!, context);
            var res2 = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.Equal(res1.EvaluationTrace, res2.EvaluationTrace);
            Assert.Equal(res1.ConsequenceSeverityMultiplier, res2.ConsequenceSeverityMultiplier);
            Assert.Equal(res1.FactionEncounterWeightMultiplier, res2.FactionEncounterWeightMultiplier);
        }
    }
}
