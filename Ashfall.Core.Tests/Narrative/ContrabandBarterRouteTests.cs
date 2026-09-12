// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// PLAN 147 follow-up — the barter acquisition route: activated contraband
    /// records listed as ShelterBarterSystem stock (the contraband broker),
    /// priced in canonical trade-value units with a scarcity premium, high-tier
    /// stock gated on real campaign state (restock-day gates), and
    /// reroll-resistance pinned end-to-end.
    /// </summary>
    public sealed class ContrabandBarterRouteTests : CatalogTestBase
    {
        private static BunkerContrabandCatalog LoadCatalog()
            => BunkerContrabandCatalog.LoadFromFile(
                Path.Combine(DataDirectory, "narrative", "bunker_contraband_barter.json"));

        private static ItemCatalog LoadItemCatalog()
            => ItemCatalogLoader.LoadCatalog(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

        private static (ShelterBarterSystem barter, InventoryContainer inventory) CreateBarter(
            InventoryContainer? inventory = null,
            ItemCatalog? itemCatalog = null,
            bool withBroker = true,
            int seed = 147)
        {
            inventory ??= new InventoryContainer();
            itemCatalog ??= LoadItemCatalog();

            var barter = new ShelterBarterSystem(
                new SeededRng(seed),
                inventory,
                thermalSystem: null,
                log: null,
                itemLookup: id => itemCatalog.Get(id));

            if (withBroker)
            {
                var broker = ContrabandBrokerCaravan.Build(
                    LoadCatalog(), ContrabandStashSystem.DefaultActivations());
                barter.RegisterCaravan(broker);
            }
            return (barter, inventory);
        }

        private static Dictionary<string, int> OfferWorth(InventoryContainer inventory, float targetValue)
        {
            // Canned food = 15 base value; produce enough to cover the target.
            int count = (int)System.Math.Ceiling(targetValue / 15f) + 1;
            Assert.True(inventory.TryProduce("item_canned_food", count + 2));
            return new Dictionary<string, int> { { "item_canned_food", count } };
        }

        // ── Broker definition ────────────────────────────────────────────

        [Fact]
        public void Broker_BuildsFromActivations_Deterministically()
        {
            var def1 = ContrabandBrokerCaravan.Build(LoadCatalog(), ContrabandStashSystem.DefaultActivations());
            var def2 = ContrabandBrokerCaravan.Build(LoadCatalog(), ContrabandStashSystem.DefaultActivations());

            Assert.Equal(4, def1.stock.Count);
            Assert.Equal(ContrabandBrokerCaravan.CaravanId, def1.caravan_id);
            Assert.True(def1.use_canonical_item_values);

            var ids1 = def1.stock.Select(s => s.item_id).ToList();
            var ids2 = def2.stock.Select(s => s.item_id).ToList();
            Assert.Equal(ids1, ids2); // deterministic ordinal order

            // Stock lines mirror the activation grants and day gates exactly.
            foreach (var activation in ContrabandStashSystem.DefaultActivations())
            {
                var line = def1.stock.Single(s => s.item_id == activation.canonicalItemId);
                Assert.Equal(activation.grantQuantity, line.quantity);
                Assert.Equal(activation.minDay, line.available_from_day);
                Assert.Equal(ContrabandBrokerCaravan.ScarcityPremiumBp, line.price_multiplier_bp);
            }
        }

        [Fact]
        public void Broker_Prices_AreCanonicalTradeValueTimesPremium()
        {
            var (barter, _) = CreateBarter();
            var broker = barter.Catalog[ContrabandBrokerCaravan.CaravanId];
            var itemCatalog = LoadItemCatalog();

            foreach (var line in broker.stock)
            {
                var def = itemCatalog.Get(line.item_id);
                Assert.NotNull(def);
                float expected = def!.tradeValue * (line.price_multiplier_bp / 10000f);
                float actual = barter.CalculateCaravanStockCost(
                    broker, new Dictionary<string, int> { { line.item_id, 1 } });
                Assert.Equal(expected, actual, 2);
            }

            // Spot-check the premium: morphine costs strictly more than its
            // trade value (60 × 1.25 = 75) — no buy-at-discount arbitrage.
            Assert.Equal(75f, barter.CalculateCaravanStockCost(
                broker, new Dictionary<string, int> { { "morphine", 1 } }), 2);
        }

        // ── Day gating: high tier only from real campaign state ──────────

        [Fact]
        public void Broker_HighTierStock_HiddenBeforeGateDay()
        {
            var (barter, _) = CreateBarter();

            // First arrival window: days 10-11 (period 10, stay 2). Both gates
            // (20, 25) still ahead → wheat seeds and morphine must not stock.
            barter.TickDay(10);
            var state = barter.State.caravans[ContrabandBrokerCaravan.CaravanId];
            Assert.True(state.isAtAirlock);
            Assert.True(state.remainingStock["item_playing_cards"] > 0, "tier-1 stock is ungated");
            Assert.True(state.remainingStock["sugar"] > 0, "tier-2 stock is ungated");
            Assert.Equal(0, state.remainingStock["item_seed_wheat"]);
            Assert.Equal(0, state.remainingStock["morphine"]);
        }

        [Fact]
        public void Broker_HighTierPurchase_BlockedBeforeGateDay()
        {
            var (barter, inventory) = CreateBarter();
            barter.TickDay(10); // broker at the airlock, gates not yet passed

            var offer = OfferWorth(inventory, 100f);
            var result = barter.ExecuteTrade(
                ContrabandBrokerCaravan.CaravanId,
                offer,
                new Dictionary<string, int> { { "morphine", 1 } });

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("insufficient_caravan_stock", result.FailureCode);
            Assert.Equal(0, inventory.CountById("morphine"));
        }

        [Fact]
        public void Broker_HighTierStock_AppearsAtGateDay()
        {
            var (barter, _) = CreateBarter();

            // Tick through the gate (25) to the next arrival window (day 30).
            for (int day = 0; day <= 30; day++) barter.TickDay(day);
            var state = barter.State.caravans[ContrabandBrokerCaravan.CaravanId];
            Assert.True(state.isAtAirlock);
            Assert.Equal(4, state.remainingStock["morphine"]);
            Assert.Equal(1, state.remainingStock["item_seed_wheat"]);
        }

        // ── Real trade through the canonical pipeline ─────────────────────

        [Fact]
        public void Broker_Purchase_GrantsCanonicalItemsAtomically()
        {
            var (barter, inventory) = CreateBarter();
            barter.TickDay(30); // broker present, all gates passed

            int slotsBefore = inventory.Slots.Count(s => s.Amount > 0);
            var offer = OfferWorth(inventory, 80f); // covers 1 morphine at 75
            var result = barter.ExecuteTrade(
                ContrabandBrokerCaravan.CaravanId,
                offer,
                new Dictionary<string, int> { { "morphine", 1 } });

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(1, inventory.CountById("morphine"));
            // OfferWorth produced count+2 rations and offered count → 2 remain.
            Assert.Equal(2, inventory.CountById("item_canned_food"));
            Assert.True(inventory.Slots.Count(s => s.Amount > 0) >= slotsBefore);
        }

        [Fact]
        public void Broker_NoBuySellArbitrage_RoundTripLosesValue()
        {
            var (barter, inventory) = CreateBarter();
            var broker = barter.Catalog[ContrabandBrokerCaravan.CaravanId];
            barter.TickDay(30);

            foreach (var line in broker.stock)
            {
                // Buying 1 unit must cost strictly more than selling it back —
                // the scarcity premium guarantees the round trip loses value.
                float buyCost = barter.CalculateCaravanStockCost(
                    broker, new Dictionary<string, int> { { line.item_id, 1 } });
                float sellValue = barter.CalculatePlayerOfferValue(
                    broker, new Dictionary<string, int> { { line.item_id, 1 } });
                Assert.True(buyCost > sellValue,
                    $"{line.item_id}: buy {buyCost} must exceed sell {sellValue} (no free conversion)");
            }

            // End-to-end: buy 1 morphine, sell it straight back for broker
            // stock (the broker only pays out what it carries) → the round
            // trip returns far less value than the buy consumed.
            var offer = OfferWorth(inventory, 80f);
            var buy = barter.ExecuteTrade(
                ContrabandBrokerCaravan.CaravanId, offer,
                new Dictionary<string, int> { { "morphine", 1 } });
            Assert.Equal(ActionResult.StatusKind.Success, buy.Status);
            Assert.Equal(1, inventory.CountById("morphine"));

            var sellBack = barter.ExecuteTrade(
                ContrabandBrokerCaravan.CaravanId,
                new Dictionary<string, int> { { "morphine", 1 } },
                new Dictionary<string, int> { { "sugar", 1 } });
            Assert.Equal(ActionResult.StatusKind.Success, sellBack.Status);
            // Sell-back returns less value than the buy consumed — no arbitrage.
            Assert.Equal(0, inventory.CountById("morphine"));
            Assert.Equal(1, inventory.CountById("sugar"));
        }

        // ── Reroll resistance ─────────────────────────────────────────────

        [Fact]
        public void Broker_StockPinnedDuringStay_NoRerollByReopen()
        {
            var (barter, inventory) = CreateBarter();
            barter.TickDay(30); // arrival — stock pinned

            var offer = OfferWorth(inventory, 300f);
            var r1 = barter.ExecuteTrade(
                ContrabandBrokerCaravan.CaravanId, offer,
                new Dictionary<string, int> { { "morphine", 1 } });
            Assert.Equal(ActionResult.StatusKind.Success, r1.Status);
            Assert.Equal(3, barter.State.caravans[ContrabandBrokerCaravan.CaravanId]
                .remainingStock["morphine"]);

            // Re-reading state (a screen "reopen" is a pure read) cannot
            // regenerate stock: only the next arrival restocks.
            for (int i = 0; i < 3; i++)
            {
                var state = barter.State.caravans[ContrabandBrokerCaravan.CaravanId];
                Assert.Equal(3, state.remainingStock["morphine"]);
            }

            // Buying the remaining 3 empties the line until the next arrival.
            var r2 = barter.ExecuteTrade(
                ContrabandBrokerCaravan.CaravanId, OfferWorth(inventory, 300f),
                new Dictionary<string, int> { { "morphine", 3 } });
            Assert.Equal(ActionResult.StatusKind.Success, r2.Status);
            Assert.Equal(0, barter.State.caravans[ContrabandBrokerCaravan.CaravanId]
                .remainingStock["morphine"]);
        }

        [Fact]
        public void Broker_SaveRoundTrip_PreservesStockAndGates()
        {
            var (barter, inventory) = CreateBarter();
            barter.TickDay(11); // mid-stay (arrived day 10, departs day 12); gates not passed

            var captured = barter.CaptureState();

            var reloaded = new ShelterBarterSystem(
                new SeededRng(999), new InventoryContainer(), null, null,
                id => LoadItemCatalog().Get(id));
            // Registration (code/definitions) precedes state restore — the host order.
            reloaded.RegisterCaravan(ContrabandBrokerCaravan.Build(
                LoadCatalog(), ContrabandStashSystem.DefaultActivations()));
            reloaded.RestoreState(captured);

            var state = reloaded.State.caravans[ContrabandBrokerCaravan.CaravanId];
            Assert.True(state.isAtAirlock);
            // Restore must not reroll gated stock into availability.
            Assert.Equal(0, state.remainingStock["morphine"]);
            Assert.True(state.remainingStock["item_playing_cards"] > 0);

            // And a purchase attempt reflects the restored (pinned) stock.
            var inv2 = new InventoryContainer();
            inv2.TryProduce("item_canned_food", 30);
            var result = reloaded.ExecuteTrade(
                ContrabandBrokerCaravan.CaravanId,
                new Dictionary<string, int> { { "item_canned_food", 20 } },
                new Dictionary<string, int> { { "morphine", 1 } });
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
        }

        [Fact]
        public void Broker_Deterministic_IdenticalSequencesProduceIdenticalStock()
        {
            var a = CreateBarter(seed: 55);
            var b = CreateBarter(seed: 55);

            for (int day = 0; day <= 40; day++)
            {
                a.barter.TickDay(day);
                b.barter.TickDay(day);
                var sa = a.barter.State.caravans[ContrabandBrokerCaravan.CaravanId];
                var sb = b.barter.State.caravans[ContrabandBrokerCaravan.CaravanId];
                Assert.Equal(sa.isAtAirlock, sb.isAtAirlock);
                Assert.Equal(sa.remainingStock, sb.remainingStock);
            }
        }

        // ── Existing economy unchanged ────────────────────────────────────

        [Fact]
        public void LegacyCaravans_Pricing_UnchangedByBrokerPath()
        {
            var (barter, _) = CreateBarter();
            var salvagers = barter.Catalog["caravan_scrap_salvagers"];

            // Table values: blowtorch 45 × 1.0; scrap metal 2 × 0.9.
            Assert.Equal(45f, barter.CalculateCaravanStockCost(
                salvagers, new Dictionary<string, int> { { "item_blowtorch", 1 } }), 2);
            Assert.Equal(1.8f, barter.CalculateCaravanStockCost(
                salvagers, new Dictionary<string, int> { { "item_scrap_metal", 1 } }), 2);

            // The default caravans never use canonical-item pricing.
            foreach (var def in barter.Catalog.Values)
            {
                if (def.caravan_id == ContrabandBrokerCaravan.CaravanId) continue;
                Assert.False(def.use_canonical_item_values);
            }
        }
    }
}
