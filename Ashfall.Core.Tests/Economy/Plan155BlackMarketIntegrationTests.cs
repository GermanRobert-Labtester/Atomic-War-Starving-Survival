// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 155 / 211: Black Market & Underground Economy — Integration Tests
// Verifies catalog loading, syndicate discovery, loan issuance, debt repayment,
// default escalation to FactionBountySystem, and state round-trip.
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Plan155BlackMarket
{
    public sealed class Plan155BlackMarketIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        private static BlackMarketInventoryCatalog LoadCatalog()
        {
            string dataDir = Path.GetDirectoryName(ResolveDataPath("black_market_inventory.json"))!;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var result = BlackMarketInventoryCatalogLoader.Load(dataDir, fileIO, serializer);
            return BlackMarketInventoryCatalogLoader.ToCatalog(result);
        }

        [Fact]
        public void AuthoritativeCatalog_LoadsAllAuthoredSyndicatesAndEntries()
        {
            string path = ResolveDataPath("black_market_inventory.json");
            Assert.True(File.Exists(path), $"black_market_inventory.json must exist at {path}");

            string dataDir = Path.GetDirectoryName(path)!;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var result = BlackMarketInventoryCatalogLoader.Load(dataDir, fileIO, serializer);

            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Syndicates.Count >= 3);
            Assert.True(result.Entries.Count >= 7);
            Assert.NotNull(result.Syndicates.FirstOrDefault(s => s.syndicate_id == "faction_wasteland_outlaws"));
            Assert.NotNull(result.Syndicates.FirstOrDefault(s => s.syndicate_id == "faction_ash_market_brokers"));
            Assert.NotNull(result.Syndicates.FirstOrDefault(s => s.syndicate_id == "faction_cold_ledger"));
        }

        [Fact]
        public void DiscoverContact_EnablesSyndicateTransactions()
        {
            var catalog = LoadCatalog();
            var system = new BlackMarketSystem();
            system.BindCatalog(catalog);

            const string syndicate = "faction_wasteland_outlaws";
            Assert.False(system.IsContactDiscovered(syndicate));

            bool discovered = system.DiscoverContact(syndicate, 1);
            Assert.True(discovered);
            Assert.True(system.IsContactDiscovered(syndicate));
        }

        [Fact]
        public void LoansAndRepayment_TrackDebtAndPrincipalCorrectly()
        {
            var catalog = LoadCatalog();
            var system = new BlackMarketSystem();
            system.BindCatalog(catalog);

            const string syndicate = "faction_cold_ledger";
            bool disc = system.DiscoverContact(syndicate, 1);
            Assert.True(disc);

            var debt = system.TakeLoan(syndicate, 500, day: 1, durationDays: 5);
            Assert.NotNull(debt);
            Assert.Equal(500, debt!.principalUnits);
            Assert.Equal(6, debt.dueDay);
            Assert.Equal(UnderworldDebtRecord.StatusActive, debt.status);

            // Partial repayment
            bool repayRes = system.RepayDebt(debt.debtId, 200, day: 2);
            Assert.True(repayRes);
            Assert.Equal(200, debt.repaidUnits);
            Assert.Equal(UnderworldDebtRecord.StatusActive, debt.status);

            // Full repayment
            bool finalRepay = system.RepayDebt(debt.debtId, 300, day: 3);
            Assert.True(finalRepay);
            Assert.Equal(UnderworldDebtRecord.StatusRepaid, debt.status);
        }

        [Fact]
        public void DefaultedDebt_EscalatesToFactionBountySystem()
        {
            var catalog = LoadCatalog();
            var system = new BlackMarketSystem();
            system.BindCatalog(catalog);
            var bounties = new FactionBountySystem();
            system.BindFactionBountySystem(bounties);

            const string syndicate = "faction_wasteland_outlaws";
            system.DiscoverContact(syndicate, 1);

            var debt = system.TakeLoan(syndicate, 400, day: 1, durationDays: 3);
            Assert.NotNull(debt);
            Assert.Equal(4, debt!.dueDay);

            // Advance time past due day (day 5)
            system.TickDaily(5);

            Assert.Equal(UnderworldDebtRecord.StatusDefaulted, debt.status);
            Assert.True(bounties.AllBounties.Count > 0);
            var issuedBounty = bounties.AllBounties.FirstOrDefault(b => b.FactionId == syndicate);
            Assert.NotNull(issuedBounty);
        }

        [Fact]
        public void SaveAndRestore_PreservesDiscoveredContactsAndDebts()
        {
            var catalog = LoadCatalog();
            var system = new BlackMarketSystem();
            system.BindCatalog(catalog);

            const string syndicate = "faction_wasteland_outlaws";
            system.DiscoverContact(syndicate, 1);
            system.TakeLoan(syndicate, 300, 1, 4);

            var state = system.CaptureState();

            var restored = new BlackMarketSystem();
            restored.BindCatalog(catalog);
            restored.RestoreState(state);

            Assert.True(restored.IsContactDiscovered(syndicate));
            Assert.Single(restored.State.debts);
            Assert.Equal(300, restored.State.debts[0].principalUnits);
        }
    }
}
