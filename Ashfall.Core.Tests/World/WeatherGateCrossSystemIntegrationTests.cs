using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Acceptance Scenarios A through H and full cross-system integration tests.
    /// Proves F17 (War), F18 (Territory), F19 (Debt), and F20 (Seasonal) operate together
    /// as a unified, deterministic, cross-system strategic layer.
    /// </summary>
    public class WeatherGateCrossSystemIntegrationTests
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
        public void ScenarioA_Route12_Blizzard_WarActive_Hostile_ContestedTerritory()
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
                    IsDominantFactionHostile: true),
                Territory = new TerritorySnapshot(TerritoryControlState.Contested, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // 1. Passability: blocked by blizzard
            Assert.True(result.IsBlocked);
            // 2. Consequence: 1.5x severity
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
            // 3. Encounters: warlord_checkpoint with 1.75x weight
            Assert.Equal("warlord_checkpoint", result.FactionEncounterTag);
            Assert.Equal(1.75f, result.FactionEncounterWeightMultiplier);
            // 4. Territory: contested -> no shelter
            Assert.False(result.ShelterAvailable);
        }

        [Fact]
        public void ScenarioB_Route12_Clear_WarActive_Hostile()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Clear,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 80,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true),
                Territory = new TerritorySnapshot(TerritoryControlState.Contested, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            // Passability authority holds: Clear weather is NOT blocked
            Assert.False(result.IsBlocked);
            Assert.Empty(result.BlockedReason);

            // War encounter pressure is active on the open corridor
            Assert.Equal("warlord_checkpoint", result.FactionEncounterTag);
            Assert.Equal(1.75f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void ScenarioC_Route12_Blizzard_Peacetime_TerritoryControlled()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: false,
                    ActiveWarTension: 0,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: false),
                Territory = new TerritorySnapshot(TerritoryControlState.Controlled, "warlords_sector_4")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            // Shelter is available
            Assert.True(result.ShelterAvailable);
            // No wartime encounters
            Assert.Empty(result.FactionEncounterTag);
            Assert.Equal(1.0f, result.FactionEncounterWeightMultiplier);
        }

        [Fact]
        public void ScenarioD_Route12_Blizzard_Peacetime_TerritoryUnclaimed()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: false,
                    ActiveWarTension: 0,
                    DominantFactionId: "",
                    IsDominantFactionHostile: false),
                Territory = new TerritorySnapshot(TerritoryControlState.Unclaimed, "")
            };

            var result = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.True(result.IsBlocked);
            Assert.False(result.ShelterAvailable);
            Assert.Equal(1.5f, result.ConsequenceSeverityMultiplier);
        }

        [Fact]
        public void ScenarioE_RailwayDebt_Route08Blocked_PausesCountdown()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_seasonal_ice_road", out var gate));

            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_08");

            var gateResult = WeatherGateContextEvaluator.Evaluate(gate!, new WeatherGateEvaluationContext
            {
                TargetId = gate!.TargetId,
                CurrentWeather = WeatherKind.Clear // Blocked (seasonal ice road requires Blizzard)
            });

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("p1", 100f, 10, 0.1f, "f_parts", "faction_railway_guild");
            debtSys.PresentContract("p1", 100f, 10, 0.1f, "f_parts", "faction_railway_guild");
            debtSys.SignContract("p1", 1);
            var contract = debtSys.GetContract("p1");

            // Tick day with debt access check
            debtSys.TickDaily(2, c => DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, gateResult, routeCtx));

            Assert.Equal(10, contract!.daysRemaining);
            Assert.Equal(1, contract.weatherDelayDaysUsed);
        }

        [Fact]
        public void ScenarioF_WeatherClears_DebtResumesNormally()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_seasonal_ice_road", out var gate));

            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_08");

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("p1", 100f, 8, 0.1f, "f_parts", "faction_railway_guild");
            debtSys.PresentContract("p1", 100f, 8, 0.1f, "f_parts", "faction_railway_guild");
            debtSys.SignContract("p1", 1);
            var contract = debtSys.GetContract("p1");

            // Day 2: Clear weather (blocked)
            var blockedResult = WeatherGateContextEvaluator.Evaluate(gate!, new WeatherGateEvaluationContext
            {
                TargetId = gate!.TargetId,
                CurrentWeather = WeatherKind.Clear
            });
            debtSys.TickDaily(2, c => DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, blockedResult, routeCtx));
            Assert.Equal(8, contract!.daysRemaining);

            // Day 3: Blizzard weather (seasonal ice road is open!)
            var openResult = WeatherGateContextEvaluator.Evaluate(gate!, new WeatherGateEvaluationContext
            {
                TargetId = gate.TargetId,
                CurrentWeather = WeatherKind.Blizzard
            });
            debtSys.TickDaily(3, c => DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, openResult, routeCtx));

            Assert.Equal(7, contract.daysRemaining); // Resumed!
        }

        [Fact]
        public void ScenarioG_ContinuousBlockage_GraceCapEnforced()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_seasonal_ice_road", out var gate));

            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_08");

            var blockedResult = WeatherGateContextEvaluator.Evaluate(gate!, new WeatherGateEvaluationContext
            {
                TargetId = gate!.TargetId,
                CurrentWeather = WeatherKind.Clear
            });

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("p1", 100f, 6, 0.1f, "f_parts", "faction_railway_guild");
            debtSys.PresentContract("p1", 100f, 6, 0.1f, "f_parts", "faction_railway_guild");
            debtSys.SignContract("p1", 1);
            var contract = debtSys.GetContract("p1");

            bool IsDelayed(DebtContract c) =>
                DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, blockedResult, routeCtx);

            // Days 2..4: Grace budget 1, 2, 3
            debtSys.TickDaily(2, IsDelayed);
            debtSys.TickDaily(3, IsDelayed);
            debtSys.TickDaily(4, IsDelayed);
            Assert.Equal(6, contract!.daysRemaining);
            Assert.Equal(3, contract.weatherDelayDaysUsed);

            // Day 5: Grace budget exhausted (3 == MaxWeatherGraceDays) -> decrements
            debtSys.TickDaily(5, IsDelayed);
            Assert.Equal(5, contract.daysRemaining);

            // Day 6: continues decrementing
            debtSys.TickDaily(6, IsDelayed);
            Assert.Equal(4, contract.daysRemaining);
        }

        [Fact]
        public void ScenarioH_Route07_BioFog_FilterClog_WithAndWithoutGasMask()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_lowland_marsh_fog", out var gate));

            // Without gas mask: blocked, compound severity 1.5x, no override
            var ctxWithoutMask = new WeatherGateEvaluationContext
            {
                TargetId = "route_07_the_aluminium_whale_salvage_run",
                CurrentWeather = WeatherKind.BioFog,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_ash_filter_clog" }),
                InventoryItems = new[] { "clean_water" }
            };

            var resWithoutMask = WeatherGateContextEvaluator.Evaluate(gate!, ctxWithoutMask);
            Assert.True(resWithoutMask.IsBlocked);
            Assert.False(resWithoutMask.OverrideAvailable);
            Assert.Equal(1.5f, resWithoutMask.ConsequenceSeverityMultiplier);

            // With gas mask: override is available
            var ctxWithMask = new WeatherGateEvaluationContext
            {
                TargetId = "route_07_the_aluminium_whale_salvage_run",
                CurrentWeather = WeatherKind.BioFog,
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_ash_filter_clog" }),
                InventoryItems = new[] { "gas_mask", "clean_water" }
            };

            var resWithMask = WeatherGateContextEvaluator.Evaluate(gate!, ctxWithMask);
            Assert.True(resWithMask.IsBlocked); // Base weather is still BioFog
            Assert.True(resWithMask.OverrideAvailable); // Player possesses the required equipment
            Assert.Contains("override_available_gas_mask", resWithMask.AppliedContextReasons);
        }

        [Fact]
        public void DeterministicTrace_ReplayIsByteIdentical()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_mountain_pass_blizzard", out var gate));

            var context = new WeatherGateEvaluationContext
            {
                TargetId = "route_12_the_cloud_eyrie_meteorological_ascent",
                CurrentWeather = WeatherKind.Blizzard,
                War = new FactionWarSnapshot(
                    IsAtWar: true,
                    ActiveWarTension: 85,
                    DominantFactionId: "warlords_sector_4",
                    IsDominantFactionHostile: true),
                Territory = new TerritorySnapshot(TerritoryControlState.Contested, "warlords_sector_4"),
                Seasonal = new SeasonalEventSnapshot(new HashSet<string> { "event_season_cold_snap" })
            };

            var res1 = WeatherGateContextEvaluator.Evaluate(gate!, context);
            var res2 = WeatherGateContextEvaluator.Evaluate(gate!, context);

            Assert.Equal(res1.EvaluationTrace, res2.EvaluationTrace);
            Assert.Equal(res1.ConsequenceSeverityMultiplier, res2.ConsequenceSeverityMultiplier);
            Assert.Equal(res1.FactionEncounterWeightMultiplier, res2.FactionEncounterWeightMultiplier);
            Assert.Equal(res1.AppliedContextReasons.Count, res2.AppliedContextReasons.Count);
            for (int i = 0; i < res1.AppliedContextReasons.Count; i++)
            {
                Assert.Equal(res1.AppliedContextReasons[i], res2.AppliedContextReasons[i]);
            }
        }
    }
}
