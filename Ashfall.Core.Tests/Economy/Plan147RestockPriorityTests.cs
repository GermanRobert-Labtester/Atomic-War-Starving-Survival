// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan147RestockPriorityTests
    {
        private static (ShelterBarterSystem barter, InventoryContainer inventory) CreateFixture()
        {
            var rng = new SeededRng(42);
            var inventory = new InventoryContainer();
            var barter = new ShelterBarterSystem(rng, inventory);
            return (barter, inventory);
        }

        private static MerchantCaravanDef CreateTestCaravan()
        {
            return new MerchantCaravanDef
            {
                caravan_id = "test_caravan_priority",
                name = "Test Priority Caravan",
                schedule_period_days = 10,
                stay_duration_days = 3,
                stock = new List<CaravanStockItem>
                {
                    new CaravanStockItem { item_id = "item_fuel", quantity = 10, price_multiplier_bp = 10000 },
                    new CaravanStockItem { item_id = "item_canned_food", quantity = 20, price_multiplier_bp = 12000 },
                    new CaravanStockItem { item_id = "item_clean_water", quantity = 30, price_multiplier_bp = 9000 },
                    new CaravanStockItem { item_id = "cloth", quantity = 15, price_multiplier_bp = 10000 }
                }
            };
        }

        [Fact]
        public void ComputePriorityScore_IsPureAndDeterministic()
        {
            var item = new CaravanStockItem { item_id = "item_fuel", price_multiplier_bp = 11500 };
            var def = CreateTestCaravan();

            int score1 = ShelterBarterSystem.ComputeItemPriorityScore(item, def);
            int score2 = ShelterBarterSystem.ComputeItemPriorityScore(item, def);

            Assert.Equal(11500, score1);
            Assert.Equal(score1, score2);
        }

        [Fact]
        public void ComputePriorityScore_WithCustomScorer_AppliesModifier()
        {
            var item = new CaravanStockItem { item_id = "item_fuel", price_multiplier_bp = 10000 };
            var def = CreateTestCaravan();

            int score = ShelterBarterSystem.ComputeItemPriorityScore(item, def, id => id == "item_fuel" ? 2500 : 0);
            Assert.Equal(12500, score);
        }

        [Fact]
        public void GetPrioritizedStock_OrdersByPriorityDescending_AndTieBreaksByItemIdOrdinal()
        {
            var (barter, _) = CreateFixture();
            var def = CreateTestCaravan();

            var prioritized = barter.GetPrioritizedStock(def);

            // Expect:
            // 1. item_canned_food (12000 bp)
            // 2. cloth (10000 bp, ordinal 'cloth' < 'item_fuel')
            // 3. item_fuel (10000 bp)
            // 4. item_clean_water (9000 bp)
            Assert.Equal(4, prioritized.Count);
            Assert.Equal("item_canned_food", prioritized[0].item_id);
            Assert.Equal("cloth", prioritized[1].item_id);
            Assert.Equal("item_fuel", prioritized[2].item_id);
            Assert.Equal("item_clean_water", prioritized[3].item_id);
        }

        [Fact]
        public void RestockCaravan_PreservesAllQuantities_OptionCInvariant()
        {
            var (barter, _) = CreateFixture();
            var def = CreateTestCaravan();
            barter.RegisterCaravan(def);

            // Day 0: Caravan arrives
            barter.TickDay(0);

            var state = barter.State.caravans[def.caravan_id];
            Assert.True(state.isAtAirlock);
            Assert.Equal(10, state.remainingStock["item_fuel"]);
            Assert.Equal(20, state.remainingStock["item_canned_food"]);
            Assert.Equal(30, state.remainingStock["item_clean_water"]);
            Assert.Equal(15, state.remainingStock["cloth"]);
        }

        [Fact]
        public void ReopeningSameDay_DoesNotRerollOrRecomputePinnedStock()
        {
            var (barter, _) = CreateFixture();
            var def = CreateTestCaravan();
            barter.RegisterCaravan(def);

            barter.TickDay(0);
            var state = barter.State.caravans[def.caravan_id];

            // Player purchases 5 canned food
            state.remainingStock["item_canned_food"] -= 5;
            Assert.Equal(15, state.remainingStock["item_canned_food"]);

            // Inspecting prioritized stock or checking schedule again on day 0 does not restore/reroll
            var prioritized = barter.GetPrioritizedStock(def);
            Assert.NotNull(prioritized);

            Assert.Equal(15, state.remainingStock["item_canned_food"]);
        }

        [Fact]
        public void NextArrival_ReevaluatesPriorityWithUpdatedScorer()
        {
            var (barter, _) = CreateFixture();
            var def = CreateTestCaravan();
            barter.RegisterCaravan(def);

            // Arrival 1: default scoring
            barter.TickDay(0);
            Assert.Equal("item_canned_food", barter.GetPrioritizedStock(def)[0].item_id);

            // Scarcity shift: water scarcity spikes (+5000 bp)
            barter.PriorityScorer = id => id == "item_clean_water" ? 5000 : 0;

            // Day 3: caravan departs
            barter.TickDay(3);
            Assert.False(barter.State.caravans[def.caravan_id].isAtAirlock);

            // Day 10: caravan returns (period 10)
            barter.TickDay(10);
            Assert.True(barter.State.caravans[def.caravan_id].isAtAirlock);

            // With water at 9000 + 5000 = 14000 bp, water is now first!
            var newPrioritized = barter.GetPrioritizedStock(def);
            Assert.Equal("item_clean_water", newPrioritized[0].item_id);
            Assert.Equal(30, barter.State.caravans[def.caravan_id].remainingStock["item_clean_water"]);
        }
    }
}
