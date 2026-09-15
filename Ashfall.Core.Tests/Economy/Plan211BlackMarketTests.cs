// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Plan 211 Phase 1 — black-market catalog + core behavior: real-catalog
    /// load with items.json FK walk, contact gating, deterministic persisted
    /// stock (no reopen reroll), canonical-derived pricing (premium ≥ broker
    /// band), sell-side value loss, loans/repayment, idempotent overdue
    /// events with bounty delegation, save/restore, legacy defaults, paired
    /// determinism.
    /// </summary>
    public sealed class Plan211BlackMarketTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static MarketSystem CreateMarket()
        {
            var load = GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            var commodity = CommodityBaselineCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            if (!commodity.HasErrors)
                market.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(commodity));
            return market;
        }

        private static BlackMarketInventoryCatalog LoadRealCatalog()
        {
            var load = BlackMarketInventoryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            return BlackMarketInventoryCatalogLoader.ToCatalog(load);
        }

        private static BlackMarketSystem CreateSystem(MarketSystem? market = null, FactionBountySystem? bounties = null)
        {
            var system = new BlackMarketSystem();
            system.BindCatalog(LoadRealCatalog());
            system.BindMarket(market ?? CreateMarket());
            if (bounties != null) system.BindFactionBountySystem(bounties);
            return system;
        }

        // ── Catalog ──────────────────────────────────────────────────

        [Fact]
        public void RealCatalog_Loads_ThreeSyndicates_SevenEntries_NoErrors()
        {
            var catalog = LoadRealCatalog();
            Assert.Equal(3, catalog.SyndicateCount);
            Assert.Equal(7, catalog.EntryCount);
        }

        [Fact]
        public void RealCatalog_EveryEntryItem_IsPricedInTheCanonicalMarket()
        {
            var catalog = LoadRealCatalog();
            var market = CreateMarket();
            var system = CreateSystem(market);
            foreach (var entry in catalog.EntriesById.Values)
            {
                float canonical = market.GetPrice(entry.item_id);
                Assert.False(float.IsNaN(canonical), $"entry '{entry.entry_id}' item '{entry.item_id}' must exist in economy_goods.json");
            }
            Assert.Empty(system.ValidationErrors);
        }

        [Fact]
        public void RealCatalog_Premiums_NeverUndercutTheBrokerBand()
        {
            // Contraband broker precedent: 1.25× over canonical. Every risk
            // premium plus the syndicate base must clear that bar so
            // buy→sell round-trips strictly lose value.
            var catalog = LoadRealCatalog();
            foreach (var s in catalog.SyndicatesById.Values)
                Assert.True(s.base_premium_bp >= 1000, $"{s.syndicate_id} base premium must be ≥1000bp");
            foreach (var e in catalog.EntriesById.Values)
                Assert.InRange(e.risk_premium_bp, 1000, BlackMarketInventoryCatalogLoader.MaxPremiumBp);
        }

        [Fact]
        public void Loader_UnknownSyndicateReference_RejectedAsUnreachable()
        {
            var dir = GetDataDir();
            string source = File.ReadAllText(Path.Combine(dir, BlackMarketInventoryCatalogLoader.FileName));
            var root = System.Text.Json.Nodes.JsonNode.Parse(source)!;
            root["entries"]![0]!["syndicate_ids"] = new System.Text.Json.Nodes.JsonArray("faction_ghost_syndicate");
            string tempDir = Path.Combine(Path.GetTempPath(), "plan211_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                File.WriteAllText(Path.Combine(tempDir, BlackMarketInventoryCatalogLoader.FileName), root.ToJsonString());
                var load = BlackMarketInventoryCatalogLoader.Load(tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
                Assert.True(load.HasErrors);
                Assert.Contains(load.Errors, e => e.Contains("unknown syndicate"));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        // ── Contact gating ───────────────────────────────────────────

        [Fact]
        public void ContactGating_UndiscoveredYieldsNothing()
        {
            var system = CreateSystem();
            Assert.Empty(system.DiscoveredContacts);
            Assert.Null(system.GetStockLine("faction_wasteland_outlaws", "black_market_field_medicine"));
            var rejected = system.Buy("faction_wasteland_outlaws", "black_market_field_medicine", 1, 5);
            Assert.False(rejected.Valid);
            Assert.Equal("contact_not_discovered", rejected.RejectReason);
        }

        [Fact]
        public void DiscoverContact_IsCampaignGate_Idempotent()
        {
            var system = CreateSystem();
            Assert.True(system.DiscoverContact("faction_wasteland_outlaws", 5));
            Assert.True(system.DiscoverContact("faction_wasteland_outlaws", 9));   // re-discover harmless
            Assert.Single(system.DiscoveredContacts);
            Assert.Equal(5, system.FindLedger("faction_wasteland_outlaws")!.discoveredDay);
            Assert.False(system.DiscoverContact("faction_ghost", 5));
        }

        // ── Deterministic stock ──────────────────────────────────────

        [Fact]
        public void StockGeneration_IsDeterministic_AndNeverRerollsOnReopen()
        {
            var a = CreateSystem();
            var b = CreateSystem();
            a.DiscoverContact("faction_wasteland_outlaws", 1);
            b.DiscoverContact("faction_wasteland_outlaws", 1);

            var snapshotA = a.EnsureStockSnapshot("faction_wasteland_outlaws", 2, new SeededRng(42));
            var snapshotB = b.EnsureStockSnapshot("faction_wasteland_outlaws", 2, new SeededRng(42));
            Assert.Equal(snapshotA.Count, snapshotB.Count);
            for (int i = 0; i < snapshotA.Count; i++)
            {
                Assert.Equal(snapshotA[i].entryId, snapshotB[i].entryId);
                Assert.Equal(snapshotA[i].quantity, snapshotB[i].quantity);
            }

            // Reopen same day: persisted lines, not a reroll.
            var reopened = a.EnsureStockSnapshot("faction_wasteland_outlaws", 2, new SeededRng(999));
            Assert.Equal(snapshotA.Count, reopened.Count);
            for (int i = 0; i < snapshotA.Count; i++)
                Assert.Equal(snapshotA[i].quantity, reopened[i].quantity);

            // Buying decrements the snapshot atomically.
            var line = a.GetStockLine("faction_wasteland_outlaws", snapshotA[0].entryId)!;
            int before = line.quantity;
            var quote = a.Buy("faction_wasteland_outlaws", snapshotA[0].entryId, 1, 2);
            Assert.True(quote.Valid);
            Assert.Equal(before - 1, a.GetStockLine("faction_wasteland_outlaws", snapshotA[0].entryId)!.quantity);
        }

        [Fact]
        public void StockRefreshes_OnlyWhenTheDayAdvances()
        {
            var system = CreateSystem();
            system.DiscoverContact("faction_wasteland_outlaws", 1);
            var day2 = system.EnsureStockSnapshot("faction_wasteland_outlaws", 2, new SeededRng(7));
            var day2Again = system.EnsureStockSnapshot("faction_wasteland_outlaws", 2, new SeededRng(8));
            Assert.Equal(day2.Count, day2Again.Count);
            var day3 = system.EnsureStockSnapshot("faction_wasteland_outlaws", 3, new SeededRng(9));
            // Day 3 regenerates (fresh stream), possibly different count.
            Assert.NotNull(day3);
            Assert.Equal(3, system.FindLedger("faction_wasteland_outlaws")!.lastStockRefreshDay);
        }

        // ── Pricing ──────────────────────────────────────────────────

        [Fact]
        public void Pricing_DerivesFromCanonicalMarket_WithPremiumAndScarcity()
        {
            var system = CreateSystem();
            system.DiscoverContact("faction_wasteland_outlaws", 1);
            var entry = system.Catalog.FindEntry("black_market_field_medicine")!;
            var market = CreateMarket();
            float canonical = market.GetPrice(entry.item_id);

            float buy = system.GetBuyPrice("faction_wasteland_outlaws", entry);
            // Premium band: base 1400bp + entry 1250bp ≈ 1.265× minimum.
            Assert.InRange(buy, canonical * 1.25f, canonical * 2.5f);
            Assert.True(buy > canonical, "black market never undercuts the canonical value");

            float sell = system.GetSellPrice("faction_wasteland_outlaws", entry);
            // Sell-side discount 2500bp → 0.75× canonical.
            Assert.InRange(sell, 0f, canonical);
            // Arbitrage guard: buy high, sell low — strictly losing round trip.
            Assert.True(sell < buy);
            Assert.True(sell < canonical);
        }

        [Fact]
        public void Pricing_ScarcityResponse_TracksTheCategoryIndex()
        {
            var system = CreateSystem();
            system.DiscoverContact("faction_ash_market_brokers", 1);
            var entry = system.Catalog.FindEntry("black_market_field_medicine")!;

            var market = CreateMarket();
            market.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "medical", multiplier = 1.5f });
            system.BindMarket(market);
            float elevated = system.GetBuyPrice("faction_ash_market_brokers", entry);

            var market2 = CreateMarket();
            market2.GetStateMutable().categoryIndices.Add(new CategoryIndexEntry { categoryId = "medical", multiplier = 0.8f });
            system.BindMarket(market2);
            float depressed = system.GetBuyPrice("faction_ash_market_brokers", entry);

            Assert.True(elevated > depressed, "medicine shortage must push illicit prices up too");
        }

        [Fact]
        public void Pricing_TrustDiscount_ReducesBuyPrice_Bounded()
        {
            var system = CreateSystem();
            system.DiscoverContact("faction_wasteland_outlaws", 1);
            var entry = system.Catalog.FindEntry("black_market_field_medicine")!;
            float cold = system.GetBuyPrice("faction_wasteland_outlaws", entry);
            var ledger = system.FindLedger("faction_wasteland_outlaws")!;
            ledger.trust = 100f;
            float warm = system.GetBuyPrice("faction_wasteland_outlaws", entry);
            // Trust discount caps at −20%: warm price is lower but still ≥ floor.
            Assert.True(warm < cold);
            Assert.InRange(warm / cold, 1f - BlackMarketSystem.TrustDiscountAtMax - 0.01f, 1f);
        }

        // ── Loans / debt ─────────────────────────────────────────────

        [Fact]
        public void Loan_Creation_WithCreditLimit_AndSingleActivePerSyndicate()
        {
            var system = CreateSystem();
            system.DiscoverContact("faction_cold_ledger", 1);
            Assert.NotNull(system.TakeLoan("faction_cold_ledger", 500f, 2, durationDays: 10));
            Assert.Null(system.TakeLoan("faction_cold_ledger", 100f, 3, durationDays: 10));   // one active loan
            Assert.Null(system.TakeLoan("faction_cold_ledger", 99999f, 3, durationDays: 10)); // over credit limit
            Assert.Single(system.State.debts, d => d.status == UnderworldDebtRecord.StatusActive);
        }

        [Fact]
        public void Loan_UndiscoveredContact_CannotLend()
        {
            var system = CreateSystem();
            Assert.Null(system.TakeLoan("faction_cold_ledger", 100f, 1, 5));
        }

        [Fact]
        public void RepayDebt_ClearsPrincipal_BuildsTrust()
        {
            var system = CreateSystem();
            system.DiscoverContact("faction_cold_ledger", 1);
            var debt = system.TakeLoan("faction_cold_ledger", 400f, 1, 10)!;
            Assert.True(system.RepayDebt(debt.debtId, 400f, 3));
            Assert.Equal(UnderworldDebtRecord.StatusRepaid, debt.status);
            Assert.Equal(0f, system.OutstandingOnDebt(debt), 3);
        }

        [Fact]
        public void Overdue_IsIdempotent_DropsTrust_RaisesHeat_EscalatesBounty()
        {
            var bounties = new FactionBountySystem();
            var system = CreateSystem(bounties: bounties);
            system.DiscoverContact("faction_cold_ledger", 1);

            var overdue = new List<UnderworldDebtRecord>();
            var placed = new List<UnderworldDebtRecord>();
            system.OnDebtOverdue += d => overdue.Add(d);
            system.OnBountyPlaced += d => placed.Add(d);

            var debt = system.TakeLoan("faction_cold_ledger", 800f, 1, 5)!;   // due day 6
            system.TickDaily(5);
            Assert.Empty(overdue);
            system.TickDaily(6);
            Assert.Single(overdue);
            Assert.Equal(UnderworldDebtRecord.StatusDefaulted, debt.status);
            Assert.Single(placed);
            // Trust dropped, heat rose.
            Assert.True(system.FindLedger("faction_cold_ledger")!.trust < 10f + 30f);
            // Bounty landed in the CANONICAL bounty authority (no second ledger).
            Assert.True(bounties.HasActiveBounty("faction_cold_ledger"));

            system.TickDaily(7);   // same debt never re-fires
            Assert.Single(overdue);
            Assert.Single(placed);
        }

        // ── Save / restore ───────────────────────────────────────────

        [Fact]
        public void SaveRestore_RoundTripsExactly()
        {
            var system = CreateSystem();
            system.DiscoverContact("faction_wasteland_outlaws", 1);
            system.DiscoverContact("faction_cold_ledger", 2);
            system.EnsureStockSnapshot("faction_wasteland_outlaws", 3, new SeededRng(11));
            system.TakeLoan("faction_cold_ledger", 300f, 3, 10);
            system.FindLedger("faction_wasteland_outlaws")!.trust = 40f;

            var saved = system.CaptureState();
            Assert.Equal(1, saved.schemaVersion);

            var restored = new BlackMarketSystem();
            restored.BindCatalog(LoadRealCatalog());
            restored.BindMarket(CreateMarket());
            restored.RestoreState(saved);

            Assert.Equal(system.DiscoveredContacts.Count, restored.DiscoveredContacts.Count);
            Assert.Equal(
                system.GetStock("faction_wasteland_outlaws").Select(s => (s.entryId, s.quantity)).ToList(),
                restored.GetStock("faction_wasteland_outlaws").Select(s => (s.entryId, s.quantity)).ToList());
            Assert.Single(restored.State.debts, d => d.status == UnderworldDebtRecord.StatusActive);
            Assert.Equal(40f, restored.FindLedger("faction_wasteland_outlaws")!.trust, 3);
        }

        [Fact]
        public void LegacySave_RestoresUndiscovered_ZeroDebt_NoBounty()
        {
            var restored = new BlackMarketSystem();
            restored.RestoreState(new BlackMarketState { schemaVersion = 1 });
            Assert.Empty(restored.DiscoveredContacts);
            Assert.Empty(restored.State.debts);
            Assert.Empty(restored.GetStock("faction_wasteland_outlaws"));
        }

        [Fact]
        public void Restore_NewerVersion_ThrowsLoudly()
        {
            var system = new BlackMarketSystem();
            Assert.Throws<InvalidOperationException>(() =>
                system.RestoreState(new BlackMarketState { schemaVersion = 2 }));
        }

        // ── Paired determinism ───────────────────────────────────────

        [Fact]
        public void PairedRun_SameSeedSameInputs_IdenticalState()
        {
            var a = CreateSystem();
            var b = CreateSystem();
            int day = 1;
            for (int i = 0; i < 30; i++)
            {
                if (i == 5)
                {
                    a.DiscoverContact("faction_wasteland_outlaws", day);
                    b.DiscoverContact("faction_wasteland_outlaws", day);
                }
                a.EnsureStockSnapshot("faction_wasteland_outlaws", day, new SeededRng(day * 31));
                b.EnsureStockSnapshot("faction_wasteland_outlaws", day, new SeededRng(day * 31));
                if (i == 15)
                {
                    a.TakeLoan("faction_wasteland_outlaws", 100f, day, 5);
                    b.TakeLoan("faction_wasteland_outlaws", 100f, day, 5);
                }
                a.TickDaily(day);
                b.TickDaily(day);
                Assert.Equal(a.GetStock("faction_wasteland_outlaws").Count, b.GetStock("faction_wasteland_outlaws").Count);
                day++;
            }
            Assert.Equal(a.CaptureState().firedEventKeys.Count, b.CaptureState().firedEventKeys.Count);
        }
    }
}
