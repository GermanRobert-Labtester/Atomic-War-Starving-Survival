using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Task F20: Weather gate interaction with seasonal events tests.
    /// Verifies compound hazard scaling, highest-only compound rule,
    /// cross-system severity merge precedence, and global 2.0x capping.
    /// </summary>
    public class WeatherGateSeasonalInteractionTests
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
        public void LowlandMarsh_BioFog_FilterClogActive_AppliesCompoundSeverity()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_lowland_marsh_fog", out var gate));
            Assert.NotNull(gate);

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_07_the_aluminium_whale_salvage_run",
                CurrentWeather = WeatherKind.BioFog,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_ash_filter_clog" })
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
            Assert.Contains("seasonal_compound_event_season_ash_filter_clog", result.AppliedContextReasons);
        }

        [Fact]
        public void MountainPass_Blizzard_ColdSnapActive_AppliesCompoundSeverity()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));
            Assert.NotNull(gate);

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_cold_snap" })
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
            Assert.Contains("seasonal_compound_event_season_cold_snap", result.AppliedContextReasons);
        }

        [Fact]
        public void LowlandMarsh_BioFog_NoSeasonalEvent_HasBaselineSeverity()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_lowland_marsh_fog", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_07_the_aluminium_whale_salvage_run",
                CurrentWeather = WeatherKind.BioFog,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string>())
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.Equal(1.0f, result.ConsequenceSeverityMultiplier);
            Assert.DoesNotContain(result.AppliedContextReasons, r => r.StartsWith("seasonal_"));
        }

        [Fact]
        public void MountainPass_ClearWeather_ColdSnapActive_RemainsOpen_WithNoCompoundSeverity()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Clear,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_cold_snap" })
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // Weather authority: Clear weather is never blocked, compound severity only modifies blocked consequences
            Assert.False(result.IsBlocked);
            Assert.Equal(1.0f, result.ConsequenceSeverityMultiplier);
        }

        [Fact]
        public void MultipleMatchingSeasonalEvents_PicksHighestMultiplierOnly()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            // Both cold_snap (1.5x) and freeze_pipe_burst (1.25x) are active
            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string>
                {
                    "event_season_cold_snap",
                    "event_season_freeze_pipe_burst"
                })
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // Highest wins: 1.5x (not 1.875x by multiplication, not 2.75x by addition)
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
            Assert.Contains("seasonal_compound_event_season_cold_snap", result.AppliedContextReasons);
        }

        [Fact]
        public void CrossSystemSeverityMerge_WarAndSeasonalDoNotMultiply()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            // War modifier is 1.5x, Seasonal modifier is 1.5x
            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 80,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true), // war -> 1.5x
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_cold_snap" }) // seasonal -> 1.5x
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // Per Rule 2.3: max(1.0, war 1.5, seasonal 1.5) = 1.5x, NOT 2.25x or 3.0x
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
        }

        [Fact]
        public void CrossSystemSeverityMerge_HigherTerritoryModifierWins()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            // Unclaimed territory is 1.5x; test gate with 1.8x territory modifier
            var customGate = new WeatherGate
            {
                Id = "gate_custom",
                TargetId = "route_custom",
                BlockedWeather = new List<string> { "Blizzard" },
                TerritoryModifier = new TerritoryModifierDefinition
                {
                    unclaimed = new TerritoryStateModifierDefinition { severity_multiplier = 1.8f }
                },
                CompoundEventModifier = new Dictionary<string, float>
                {
                    ["event_season_cold_snap"] = 1.4f
                }
            };

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_custom",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Unclaimed, ""),
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_cold_snap" })
            };

            var result = WeatherGateContextEvaluator.Evaluate(customGate, context);

            // Territory 1.8x is higher than Seasonal 1.4x -> 1.8x wins
            Assert.Equal(1.8f, result.ConsequenceSeverityMultiplier);
        }

        [Fact]
        public void ExcessiveCompoundMultiplier_IsCappedAtGlobalMax()
        {
            var gate = new WeatherGate
            {
                Id = "gate_excessive",
                TargetId = "route_excessive",
                BlockedWeather = new List<string> { "Blizzard" },
                CompoundEventModifier = new Dictionary<string, float>
                {
                    ["event_season_cold_snap"] = 3.2f // Exceeds cap
                }
            };

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_excessive",
                CurrentWeather = WeatherKind.Blizzard,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_cold_snap" })
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate, context);

            Assert.Equal(WeatherGateContextEvaluator.MaxSeverityCap, result.ConsequenceSeverityMultiplier);
        }
    }
}
