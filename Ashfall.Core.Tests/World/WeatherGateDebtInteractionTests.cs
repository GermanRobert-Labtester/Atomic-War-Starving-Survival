// SPDX-License-Identifier: MIT
using System.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    [Trait("Category", "fast")]
    public sealed class WeatherGateDebtInteractionTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "src", "Main.DebtCredit.cs")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static LedgerDebtSystem NewLedgerWithSignedDebt(string creditor, int term = 5)
        {
            var ledger = new LedgerDebtSystem();
            Assert.True(ledger.PresentContract("debtor", 10f, term, 0.1f, "grain", creditor));
            Assert.True(ledger.PresentContract("debtor", 10f, term, 0.1f, "grain", creditor));
            Assert.True(ledger.SignContract("debtor", 1));
            return ledger;
        }

        [Fact]
        public void BlockedRoute_PausesTerm_UntilGraceCap_ThenResumes_AndRoundTrips()
        {
            var ledger = NewLedgerWithSignedDebt("faction_railway_guild");
            ledger.WeatherDelayGateProvider = _ => "gate_x";
            for (int day = 2; day <= 4; day++) ledger.TickDaily(day);
            var c = ledger.GetContract("debtor")!;
            Assert.Equal(5, c.daysRemaining);
            Assert.Equal(LedgerDebtSystem.MaxWeatherGraceDays, c.weatherDelayDaysUsed);
            Assert.Equal("gate_x", c.lastWeatherDelayGateId);

            ledger.TickDaily(5);
            Assert.Equal(4, ledger.GetContract("debtor")!.daysRemaining);

            var restored = new LedgerDebtSystem();
            restored.RestoreState(ledger.CaptureState());
            var r = restored.GetContract("debtor")!;
            Assert.Equal(3, r.weatherDelayDaysUsed);
            Assert.Equal("gate_x", r.lastWeatherDelayGateId);

            var json = new SystemTextJsonSerializer();
            var viaJson = json.Deserialize<LedgerDebtSystemState>(json.Serialize(ledger.CaptureState()))!;
            Assert.Equal(3, viaJson.contracts[0].weatherDelayDaysUsed);
            Assert.Equal("gate_x", viaJson.contracts[0].lastWeatherDelayGateId);
        }

        [Fact]
        public void EachPausedDay_RaisesEvent_WithGateAndRunningGraceCount()
        {
            var ledger = NewLedgerWithSignedDebt("faction_railway_guild");
            ledger.WeatherDelayGateProvider = _ => "gate_x";
            var seen = new System.Collections.Generic.List<(string Gate, int Used)>();
            ledger.OnWeatherDelayApplied += (c, gate) => seen.Add((gate, c.weatherDelayDaysUsed));
            for (int day = 2; day <= 6; day++) ledger.TickDaily(day);
            Assert.Equal(new[] { ("gate_x", 1), ("gate_x", 2), ("gate_x", 3) }, seen);
        }

        [Fact]
        public void PauseIsSurfaced_ToJournalAndLedgerLine_AndRadioNoteReachesIntercept()
        {
            string root = Path.GetFullPath(Path.Combine(DataDir(), "..", "..", ".."));
            string debt = File.ReadAllText(Path.Combine(root, "src", "Main.DebtCredit.cs"));
            string expansion = File.ReadAllText(Path.Combine(root, "src", "Host", "ExpansionHostSession.cs"));
            string radioHost = File.ReadAllText(Path.Combine(root, "src", "Host", "RadioHostSession.cs"));
            string radioPanel = File.ReadAllText(Path.Combine(root, "src", "UI", "RadioPanel.cs"));
            Assert.Contains("OnWeatherDelayApplied +=", debt);
            Assert.Contains("debt_weather_delay_", debt);
            Assert.Contains("weather-paused", expansion);
            Assert.Contains("broadcast.ListeningNote", radioHost);
            Assert.Contains("Listening note: ", radioPanel);
        }

        [Fact]
        public void NoProviderOrOpenRoute_KeepsNormalCountdown()
        {
            var ledger = NewLedgerWithSignedDebt("faction_railway_guild");
            ledger.TickDaily(2);
            Assert.Equal(4, ledger.GetContract("debtor")!.daysRemaining);
            ledger.WeatherDelayGateProvider = _ => null;
            ledger.TickDaily(3);
            var c = ledger.GetContract("debtor")!;
            Assert.Equal(3, c.daysRemaining);
            Assert.Equal(0, c.weatherDelayDaysUsed);
        }

        [Theory]
        [InlineData("faction_railway_guild", WeatherKind.FalloutStorm, "gate_exposed_highway_fallout")]
        [InlineData("faction_hydro_barons", WeatherKind.Blizzard, "gate_lake_edge_blizzard")]
        [InlineData("faction_supply_corps", WeatherKind.Blizzard, "gate_highland_supply_blizzard")]
        [InlineData("faction_railway_guild", WeatherKind.Clear, null)]
        [InlineData("faction_scavengers", WeatherKind.FalloutStorm, null)]
        public void RealGateData_IsRouteSpecific(string creditor, WeatherKind weather, string? expectedGate)
        {
            var catalog = WeatherGateCatalogLoader.LoadFromDirectory(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var debt = new DebtContract { debtorId = "d", creditorId = creditor };
            string? gate = DebtRouteAccessResolver.FindBlockingGateId(
                debt, catalog.GetAll(), new RouteGateContextResolver(), weather, _ => false);
            Assert.Equal(expectedGate, gate);
        }
    }
}
