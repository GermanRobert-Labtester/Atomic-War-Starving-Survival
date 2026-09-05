using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Task F18: Weather gate interaction with territory control tests.
    /// Verifies shelter mitigation, territory consequence scaling, live state reflection,
    /// and strict weather passability authority.
    /// </summary>
    public class WeatherGateTerritoryInteractionTests
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
        public void Route12_Blizzard_ControlledTerritory_ExposesShelter()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Controlled, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.True(result.ShelterAvailable);
            Assert.Contains("territory_controlled", result.AppliedContextReasons);
        }

        [Fact]
        public void Route12_Blizzard_ContestedTerritory_HasNoShelter_BaselineSeverity()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Contested, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.False(result.ShelterAvailable);
            Assert.Equal(1.0f, result.ConsequenceSeverityMultiplier);
            Assert.Contains("territory_contested", result.AppliedContextReasons);
        }

        [Fact]
        public void Route12_Blizzard_UnclaimedTerritory_HasNoShelter_ElevatedSeverity()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Unclaimed, "")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.False(result.ShelterAvailable);
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
            Assert.Contains("territory_unclaimed", result.AppliedContextReasons);
        }

        [Theory]
        [InlineData(TerritoryControlState.Controlled)]
        [InlineData(TerritoryControlState.Contested)]
        [InlineData(TerritoryControlState.Unclaimed)]
        public void Route12_ClearWeather_UnderAllTerritoryStates_RemainsPassable(TerritoryControlState state)
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Clear,
                Territory = new TerritorySnapshot(state, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // Weather authority: Clear weather is never blocked by territory state
            Assert.False(result.IsBlocked);
            Assert.Empty(result.BlockedReason);
        }

        [Fact]
        public void TerritoryStateMutation_IsImmediatelyReflectedInNextEvaluation()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            // Tick 1: Controlled territory
            var ctx1 = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Controlled, "warlords_sector_4")
            };
            var res1 = WeatherGateContextEvaluator.Evaluate(gate!, ctx1);
            Assert.True(res1.ShelterAvailable);

            // Tick 2: Territory mutates to Contested
            var ctx2 = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Contested, "warlords_sector_4")
            };
            var res2 = WeatherGateContextEvaluator.Evaluate(gate!, ctx2);
            Assert.False(res2.ShelterAvailable);
            Assert.Equal(1.0f, res2.ConsequenceSeverityMultiplier);

            // Tick 3: Territory mutates to Unclaimed
            var ctx3 = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Unclaimed, "")
            };
            var res3 = WeatherGateContextEvaluator.Evaluate(gate!, ctx3);
            Assert.False(res3.ShelterAvailable);
            Assert.Equal(1.5f, res3.ConsequenceSeverityMultiplier);
        }

        [Fact]
        public void ExcessiveTerritorySeverity_IsCappedAtGlobalMax()
        {
            var gate = new WeatherGate
            {
                Id = "gate_test_extreme",
                TargetId = "route_extreme",
                BlockedWeather = new List<string> { "Blizzard" },
                TerritoryModifier = new TerritoryModifierDefinition
                {
                    unclaimed = new TerritoryStateModifierDefinition
                    {
                        severity_multiplier = 3.5f // Exceeds cap
                    }
                }
            };

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_extreme",
                CurrentWeather = WeatherKind.Blizzard,
                Territory = new TerritorySnapshot(TerritoryControlState.Unclaimed, "")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate, context);

            Assert.Equal(WeatherGateContextEvaluator.MaxSeverityCap, result.ConsequenceSeverityMultiplier);
        }
    }
}
