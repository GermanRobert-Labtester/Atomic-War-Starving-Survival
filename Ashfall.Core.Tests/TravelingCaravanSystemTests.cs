// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TravelingCaravanSystemTests
    {
        [Fact]
        public void SpawnCaravan_AddsCaravanToStateAndFiresEvent()
        {
            var system = new TravelingCaravanSystem();
            bool eventFired = false;
            string arrivedNode = null;
            system.OnCaravanArrivedAtNode += (c, node) =>
            {
                eventFired = true;
                arrivedNode = node;
            };

            var route = new List<string> { "node_alpha", "node_beta" };
            system.SpawnCaravan("caravan_01", "Merchant Convoy", "faction_traders", route);

            Assert.Equal(1, system.CaravanCount);
            Assert.True(eventFired);
            Assert.Equal("node_alpha", arrivedNode);
            var caravan = system.GetCaravanAtNode("node_alpha");
            Assert.NotNull(caravan);
            Assert.Equal("caravan_01", caravan.caravanId);
        }

        [Fact]
        public void Constructor_CapturedState_DoesNotAliasInput()
        {
            var input = new TravelingCaravanState
            {
                activeCaravans = new List<CaravanEntry>(),
                completedTradesCount = 0
            };
            var system = new TravelingCaravanSystem(input);

            input.completedTradesCount = 42;
            input.activeCaravans.Add(new CaravanEntry { caravanId = "injected" });

            Assert.Equal(0, system.State.completedTradesCount);
            Assert.Equal(0, system.CaravanCount);
        }

        [Fact]
        public void DailyTick_MalformedRouteAndNullEntry_FailClosed()
        {
            var input = new TravelingCaravanState
            {
                activeCaravans = new List<CaravanEntry>
                {
                    null,
                    new CaravanEntry
                    {
                        caravanId = "empty-route",
                        routeNodeIds = new List<string>(),
                        stayDurationDays = 1
                    }
                }
            };
            var system = new TravelingCaravanSystem(input);

            system.DailyTick();

            Assert.Equal(1, system.CaravanCount);
        }

        [Fact]
        public void SpawnCaravan_DuplicateIdOrBlankRoute_DoesNotCreateSecondAuthority()
        {
            var system = new TravelingCaravanSystem();
            system.SpawnCaravan("c1", "Trader", "f1", new List<string> { "node_a" });
            system.SpawnCaravan("c1", "Duplicate", "f1", new List<string> { "node_b" });
            system.SpawnCaravan("c2", "Blank", "f1", new List<string> { " " });

            Assert.Equal(1, system.CaravanCount);
            Assert.NotNull(system.GetCaravanAtNode("node_a"));
        }

        [Fact]
        public void DailyTick_AdvancesWaypointsAndLoopsRoute()
        {
            var system = new TravelingCaravanSystem();
            var route = new List<string> { "node_a", "node_b" };
            system.SpawnCaravan("c1", "Caravan 1", "f1", route);

            // stayDurationDays is 2. Day 0 -> Day 1 stays at node_a.
            system.DailyTick();
            Assert.NotNull(system.GetCaravanAtNode("node_a"));

            // Day 1 -> Day 2 moves to node_b.
            system.DailyTick();
            Assert.Null(system.GetCaravanAtNode("node_a"));
            Assert.NotNull(system.GetCaravanAtNode("node_b"));

            // Advance through node_b stay duration.
            system.DailyTick();
            system.DailyTick();

            // Loops back to node_a.
            Assert.NotNull(system.GetCaravanAtNode("node_a"));
        }

        [Fact]
        public void TryBuyItem_DeductsStockAndRations_FailsWhenInsufficient()
        {
            var system = new TravelingCaravanSystem();
            var route = new List<string> { "node_market" };
            system.SpawnCaravan("c1", "Trader", "f1", route);

            int playerRations = 20;
            bool success = system.TryBuyItem("c1", "item_canned_food", 2, ref playerRations);
            Assert.True(success);
            Assert.Equal(16, playerRations); // 2 * 2 rations = 4 deducted
            Assert.Equal(1, system.State.completedTradesCount);

            // Attempt to buy more stock than available (starter canned food has 6 remaining).
            int insufficientRations = 100;
            bool failStock = system.TryBuyItem("c1", "item_canned_food", 10, ref insufficientRations);
            Assert.False(failStock);
            Assert.Equal(100, insufficientRations);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TryBuyItem_NonPositiveAmount_DoesNotMutate(int amount)
        {
            var system = new TravelingCaravanSystem();
            system.SpawnCaravan("c1", "Trader", "f1", new List<string> { "node_market" });
            var caravan = system.GetCaravanAtNode("node_market")!;
            int stockBefore = caravan.inventory.Find(i => i.itemId == "item_canned_food")!.quantity;
            int rations = 10;

            bool success = system.TryBuyItem("c1", "item_canned_food", amount, ref rations);

            Assert.False(success);
            Assert.Equal(10, rations);
            Assert.Equal(stockBefore, caravan.inventory.Find(i => i.itemId == "item_canned_food")!.quantity);
            Assert.Equal(0, system.State.completedTradesCount);
        }

        [Fact]
        public void TryBuyItem_OverflowingPrice_FailsClosed()
        {
            var system = new TravelingCaravanSystem();
            system.SpawnCaravan("c1", "Trader", "f1", new List<string> { "node_market" });
            var caravan = system.GetCaravanAtNode("node_market")!;
            var stock = caravan.inventory.Find(i => i.itemId == "item_canned_food")!;
            stock.priceRations = int.MaxValue;
            stock.quantity = 3;
            int rations = 5;

            bool success = system.TryBuyItem("c1", stock.itemId, 2, ref rations);

            Assert.False(success);
            Assert.Equal(5, rations);
            Assert.Equal(3, stock.quantity);
            Assert.Equal(0, system.State.completedTradesCount);
        }

        [Fact]
        public void CaptureState_ReturnsDeepCopySnapshot()
        {
            var system = new TravelingCaravanSystem();
            var route = new List<string> { "node_x" };
            system.SpawnCaravan("c1", "Trader", "f1", route);

            var snapshot = system.CaptureState();
            Assert.Single(snapshot.activeCaravans);

            // Mutate live system
            system.DailyTick();
            int rations = 10;
            system.TryBuyItem("c1", "item_clean_water", 1, ref rations);

            Assert.Equal(1, system.State.completedTradesCount);
            Assert.Equal(0, snapshot.completedTradesCount); // Snapshot remains unchanged
        }

        [Fact]
        public void RestoreState_RestoresCaravansAndTradeCount()
        {
            var system = new TravelingCaravanSystem();
            var route = new List<string> { "node_x" };
            system.SpawnCaravan("c1", "Trader", "f1", route);
            int rations = 10;
            system.TryBuyItem("c1", "item_clean_water", 1, ref rations);

            var snapshot = system.CaptureState();

            var newSystem = new TravelingCaravanSystem();
            newSystem.RestoreState(snapshot);

            Assert.Equal(1, newSystem.CaravanCount);
            Assert.Equal(1, newSystem.State.completedTradesCount);
            Assert.NotNull(newSystem.GetCaravanAtNode("node_x"));
        }

        [Fact]
        public void RestoreState_DoesNotAliasEnvelopeCollections()
        {
            var route = new List<string> { "node_x" };
            var src = new TravelingCaravanSystem();
            src.SpawnCaravan("c1", "Trader", "f1", route);
            var snapshot = src.CaptureState();

            var restored = new TravelingCaravanSystem();
            restored.RestoreState(snapshot);

            // Mutating the envelope after restore must not touch live state.
            snapshot.activeCaravans.Clear();
            snapshot.activeCaravans.Add(null);
            snapshot.completedTradesCount = 99;

            Assert.Equal(1, restored.CaravanCount);
            Assert.Equal(0, restored.State.completedTradesCount);
            Assert.NotNull(restored.GetCaravanAtNode("node_x"));
        }
    }
}
