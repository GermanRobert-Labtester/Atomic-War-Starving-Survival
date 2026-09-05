using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Task F19: Weather gate interaction with ledger debt system tests.
    /// Verifies route-specific creditor matching, weather grace pausing,
    /// anti-exploit cap enforcement, and save/load persistence.
    /// </summary>
    public class WeatherGateDebtInteractionTests
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
        public void RailwayGuildDebt_Route08Blocked_DelayTrue_PausesCountdown()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.TryGet("gate_seasonal_ice_road", out var gate));
            Assert.NotNull(gate);
            Assert.True(gate!.WeatherDelayDebt);

            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_08");

            // Evaluate gate under Clear weather (which blocks seasonal_ice_road because it requires Blizzard)
            var gateContext = new WeatherGateEvaluationContext
            {
                TargetId = gate.TargetId,
                CurrentWeather = WeatherKind.Clear
            };
            var gateResult = WeatherGateContextEvaluator.Evaluate(gate, gateContext);
            Assert.True(gateResult.IsBlocked);
            Assert.True(gateResult.WeatherDelayDebtEligible);

            // Create debt system with a Railway Guild debt contract
            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("debtor_player", 100f, 10, 0.1f, "forfeit_iron", "faction_railway_guild", "debt_railway_guild_parts");
            debtSys.PresentContract("debtor_player", 100f, 10, 0.1f, "forfeit_iron", "faction_railway_guild", "debt_railway_guild_parts");
            debtSys.SignContract("debtor_player", 1);

            var contract = debtSys.GetContract("debtor_player");
            Assert.NotNull(contract);
            Assert.Equal(10, contract!.daysRemaining);
            Assert.Equal(0, contract.weatherDelayDaysUsed);

            // Predicate queries DebtRouteAccessResolver
            bool IsDelayed(DebtContract c) =>
                DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, gateResult, routeCtx);

            // Tick day 2: route is blocked, contract pauses
            debtSys.TickDaily(2, IsDelayed);
            Assert.Equal(10, contract.daysRemaining); // Paused!
            Assert.Equal(1, contract.weatherDelayDaysUsed);
        }

        [Fact]
        public void RailwayGuildDebt_DelayFalse_DoesNotPauseCountdown()
        {
            var gate = new WeatherGate
            {
                Id = "gate_no_delay",
                TargetId = "route_08",
                BlockedWeather = new List<string> { "Blizzard" },
                WeatherDelayDebt = false // Delay NOT allowed
            };

            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_08");

            var gateContext = new WeatherGateEvaluationContext
            {
                TargetId = "route_08",
                CurrentWeather = WeatherKind.Blizzard
            };
            var gateResult = WeatherGateContextEvaluator.Evaluate(gate, gateContext);
            Assert.True(gateResult.IsBlocked);
            Assert.False(gateResult.WeatherDelayDebtEligible);

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("debtor_player", 100f, 10, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.PresentContract("debtor_player", 100f, 10, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.SignContract("debtor_player", 1);

            var contract = debtSys.GetContract("debtor_player");
            Assert.NotNull(contract);

            bool IsDelayed(DebtContract c) =>
                DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, gateResult, routeCtx);

            // Tick day 2: delay false -> timer decrements
            debtSys.TickDaily(2, IsDelayed);
            Assert.Equal(9, contract!.daysRemaining);
            Assert.Equal(0, contract.weatherDelayDaysUsed);
        }

        [Fact]
        public void UnrelatedDebt_DoesNotPause_WhenUnrelatedRouteIsBlocked()
        {
            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_12"); // Mount Karkov route (no creditor)

            var gate = new WeatherGate
            {
                Id = "gate_route_12",
                TargetId = "route_12",
                BlockedWeather = new List<string> { "Blizzard" },
                WeatherDelayDebt = true
            };

            var gateResult = WeatherGateContextEvaluator.Evaluate(gate, new WeatherGateEvaluationContext
            {
                TargetId = "route_12",
                CurrentWeather = WeatherKind.Blizzard
            });

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("debtor_player", 100f, 10, 0.1f, "forfeit_water", "faction_hydro_barons");
            debtSys.PresentContract("debtor_player", 100f, 10, 0.1f, "forfeit_water", "faction_hydro_barons");
            debtSys.SignContract("debtor_player", 1);

            var contract = debtSys.GetContract("debtor_player");
            Assert.NotNull(contract);

            bool IsDelayed(DebtContract c) =>
                DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, gateResult, routeCtx);

            // Tick day 2: route 12 does not lead to hydro barons -> contract decrements
            debtSys.TickDaily(2, IsDelayed);
            Assert.Equal(9, contract!.daysRemaining);
            Assert.Equal(0, contract.weatherDelayDaysUsed);
        }

        [Fact]
        public void AntiExploitGraceCap_PreventsIndefiniteDebtShield()
        {
            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_08");

            var gate = new WeatherGate
            {
                Id = "gate_route_08",
                TargetId = "route_08",
                BlockedWeather = new List<string> { "Blizzard" },
                WeatherDelayDebt = true
            };

            var gateResult = WeatherGateContextEvaluator.Evaluate(gate, new WeatherGateEvaluationContext
            {
                TargetId = "route_08",
                CurrentWeather = WeatherKind.Blizzard
            });

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("debtor_player", 100f, 5, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.PresentContract("debtor_player", 100f, 5, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.SignContract("debtor_player", 1);

            var contract = debtSys.GetContract("debtor_player");
            Assert.NotNull(contract);

            bool IsDelayed(DebtContract c) =>
                DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, gateResult, routeCtx);

            // Day 2 (Grace 1): paused
            debtSys.TickDaily(2, IsDelayed);
            Assert.Equal(5, contract!.daysRemaining);
            Assert.Equal(1, contract.weatherDelayDaysUsed);

            // Day 3 (Grace 2): paused
            debtSys.TickDaily(3, IsDelayed);
            Assert.Equal(5, contract.daysRemaining);
            Assert.Equal(2, contract.weatherDelayDaysUsed);

            // Day 4 (Grace 3): paused (cap reached)
            debtSys.TickDaily(4, IsDelayed);
            Assert.Equal(5, contract.daysRemaining);
            Assert.Equal(3, contract.weatherDelayDaysUsed);

            // Day 5 (Grace exhausted): timer resumes decrementing!
            debtSys.TickDaily(5, IsDelayed);
            Assert.Equal(4, contract.daysRemaining);
            Assert.Equal(3, contract.weatherDelayDaysUsed); // Capped at 3

            // Day 6: continues decrementing
            debtSys.TickDaily(6, IsDelayed);
            Assert.Equal(3, contract.daysRemaining);
        }

        [Fact]
        public void WeatherClears_PausedDebtResumesExactly()
        {
            var resolver = new RouteGateContextResolver();
            var routeCtx = resolver.Resolve("route_08");

            var gate = new WeatherGate
            {
                Id = "gate_route_08",
                TargetId = "route_08",
                BlockedWeather = new List<string> { "Blizzard" },
                WeatherDelayDebt = true
            };

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("debtor_player", 100f, 8, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.PresentContract("debtor_player", 100f, 8, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.SignContract("debtor_player", 1);
            var contract = debtSys.GetContract("debtor_player");
            Assert.NotNull(contract);

            // Day 2: Blizzard (blocked)
            var blockedResult = WeatherGateContextEvaluator.Evaluate(gate, new WeatherGateEvaluationContext
            {
                TargetId = "route_08",
                CurrentWeather = WeatherKind.Blizzard
            });
            debtSys.TickDaily(2, c => DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, blockedResult, routeCtx));
            Assert.Equal(8, contract!.daysRemaining);
            Assert.Equal(1, contract.weatherDelayDaysUsed);

            // Day 3: Weather clears to Clear (open)
            var clearResult = WeatherGateContextEvaluator.Evaluate(gate, new WeatherGateEvaluationContext
            {
                TargetId = "route_08",
                CurrentWeather = WeatherKind.Clear
            });
            debtSys.TickDaily(3, c => DebtRouteAccessResolver.IsDebtRepaymentRouteBlocked(c, clearResult, routeCtx));

            // Resumes from exact 8 -> 7
            Assert.Equal(7, contract.daysRemaining);
            Assert.Equal(1, contract.weatherDelayDaysUsed); // Preserved
        }

        [Fact]
        public void WeatherGraceState_SurvivesSaveLoadRoundtrip()
        {
            var serializer = new SystemTextJsonSerializer();

            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("debtor_player", 100f, 7, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.PresentContract("debtor_player", 100f, 7, 0.1f, "forfeit_iron", "faction_railway_guild");
            debtSys.SignContract("debtor_player", 1);
            var contract = debtSys.GetContract("debtor_player");
            Assert.NotNull(contract);

            contract!.weatherDelayDaysUsed = 2;
            contract.daysRemaining = 6;
            contract.lastWeatherDelayGateId = "gate_route_08";

            // Serialize & deserialize
            string json = serializer.Serialize(debtSys.State);
            var restoredState = serializer.Deserialize<LedgerDebtSystemState>(json);
            Assert.NotNull(restoredState);

            var restoredContract = restoredState!.contracts.Find(c => c.debtorId == "debtor_player");
            Assert.NotNull(restoredContract);
            Assert.Equal(6, restoredContract!.daysRemaining);
            Assert.Equal(2, restoredContract.weatherDelayDaysUsed);
            Assert.Equal("gate_route_08", restoredContract.lastWeatherDelayGateId);
        }
    }
}
