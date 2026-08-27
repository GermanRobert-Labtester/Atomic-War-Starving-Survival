// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    public class InventoryTransactionTests
    {
        private static ItemDefinition Def(string id, ItemType type = ItemType.Material, int stackMax = 20, float weight = 1f)
        {
            return new ItemDefinition
            {
                id = id,
                displayName = id,
                type = type,
                stackMax = stackMax,
                weight = weight
            };
        }

        [Fact]
        public void Quote_CalculatesWeightsAndAggregatesCorrectly()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var scrap = Def("scrap_metal", weight: 2f);
            var brass = Def("brass_fittings", weight: 1.5f);
            var fuel = Def("fuel", weight: 5f);

            inv.Add(scrap, 10);
            inv.Add(brass, 5);

            var bill = new InventoryBill()
                .AddCost("scrap_metal", 2)
                .AddCost("scrap_metal", 3) // Duplicate ID: total 5
                .AddCost("brass_fittings", 2)
                .AddGrant("fuel", 3);

            var quote = inv.Quote(bill, id => id == "fuel" ? fuel : null);

            Assert.True(quote.CanExecute);
            Assert.Equal(5, quote.AggregatedCosts["scrap_metal"]);
            Assert.Equal(2, quote.AggregatedCosts["brass_fittings"]);
            Assert.Equal(3, quote.AggregatedGrants["fuel"]);

            // Total cost weight = 5 * 2.0 + 2 * 1.5 = 13.0
            Assert.Equal(13f, quote.TotalCostWeight);
            // Total grant weight = 3 * 5.0 = 15.0
            Assert.Equal(15f, quote.TotalGrantWeight);
            Assert.Equal(2f, quote.NetWeightChange);
        }

        [Fact]
        public void Validate_AggregatesDuplicateItemIds_BeforeCheckingAvailability()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var scrap = Def("scrap_metal");
            inv.Add(scrap, 4); // Player has 4 scrap

            // Bill with duplicate entries for scrap_metal: 3 + 2 = 5 needed
            var bill = new InventoryBill()
                .AddCost("scrap_metal", 3)
                .AddCost("scrap_metal", 2);

            var validation = inv.ValidateTransaction(bill);

            Assert.False(validation.IsValid);
            Assert.Equal(InventoryTransactionStatus.InsufficientQuantity, validation.Status);
            Assert.Equal("scrap_metal", validation.FailedItemId);
            Assert.Equal(5, validation.RequiredAmount);
            Assert.Equal(4, validation.AvailableAmount);

            // Adding 1 more scrap satisfies the aggregated requirement of 5
            inv.Add(scrap, 1);
            var validResult = inv.ValidateTransaction(bill);
            Assert.True(validResult.IsValid);
        }

        [Fact]
        public void Validate_AccountsForFreedSlots_WhenCalculatingGrantCapacity()
        {
            var inv = new InventoryContainer { Capacity = 2, MaxWeight = 100f };
            var itemA = Def("item_a");
            var itemB = Def("item_b");
            var itemC = Def("item_c");

            inv.Add(itemA, 1);
            inv.Add(itemB, 1); // 2 of 2 slots occupied

            // Bill consumes all of itemA and grants itemC
            var bill = new InventoryBill()
                .AddCost(itemA, 1)
                .AddGrant(itemC, 1);

            var validation = inv.ValidateTransaction(bill);
            Assert.True(validation.IsValid, "Freed slot from itemA should allow itemC to fit within capacity 2");

            // But if bill grants two new distinct items while only freeing one slot, it should fail capacity
            var itemD = Def("item_d");
            var overflowBill = new InventoryBill()
                .AddCost(itemA, 1)
                .AddGrant(itemC, 1)
                .AddGrant(itemD, 1);

            var overflowValidation = inv.ValidateTransaction(overflowBill);
            Assert.False(overflowValidation.IsValid);
            Assert.Equal(InventoryTransactionStatus.ExceedsCapacity, overflowValidation.Status);
        }

        [Fact]
        public void Validate_FailsWhenGrantExceedsMaxWeight()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 10f };
            var lightItem = Def("light", weight: 1f);
            var heavyItem = Def("heavy", weight: 12f);

            inv.Add(lightItem, 2); // Current weight: 2f, remaining capacity: 8f

            var bill = new InventoryBill().AddGrant(heavyItem, 1);
            var validation = inv.ValidateTransaction(bill);

            Assert.False(validation.IsValid);
            Assert.Equal(InventoryTransactionStatus.ExceedsWeight, validation.Status);
        }

        [Fact]
        public void Execute_LateItemFailure_LeavesInventoryUntouched()
        {
            var inv = new InventoryContainer { Capacity = 10 };
            var scrap = Def("scrap_metal");
            var brass = Def("brass_fittings");

            inv.Add(scrap, 10);
            // 0 brass in inventory

            int inventoryChangedCount = 0;
            int itemsRemovedCount = 0;
            inv.OnInventoryChanged += () => inventoryChangedCount++;
            inv.OnItemRemoved += (_, _) => itemsRemovedCount++;

            // Bill requires 4 scrap and 2 brass
            var bill = new InventoryBill()
                .AddCost("scrap_metal", 4)
                .AddCost("brass_fittings", 2);

            bool executed = inv.TryExecuteTransaction(bill);

            Assert.False(executed);
            Assert.Equal(10, inv.CountById("scrap_metal"));
            Assert.Equal(0, inv.CountById("brass_fittings"));
            Assert.Equal(0, inventoryChangedCount);
            Assert.Equal(0, itemsRemovedCount);
        }

        [Fact]
        public void Execute_CapacityFailure_LeavesInventoryUntouched()
        {
            var inv = new InventoryContainer { Capacity = 1 };
            var itemA = Def("item_a");
            var itemB = Def("item_b");

            inv.Add(itemA, 1); // Capacity 1/1 full

            var bill = new InventoryBill()
                .AddCost(itemA, 1) // frees 1 slot
                .AddGrant(itemB, 1) // uses 1 slot
                .AddGrant("item_c", 1); // requires 2nd slot -> exceeds capacity

            bool executed = inv.TryExecuteTransaction(bill);

            Assert.False(executed);
            Assert.Equal(1, inv.CountById("item_a"));
            Assert.Equal(0, inv.CountById("item_b"));
        }

        [Fact]
        public void Execute_CallbackFailure_RollsBackStateAndReThrows()
        {
            var inv = new InventoryContainer { Capacity = 10 };
            var scrap = Def("scrap_metal");
            inv.Add(scrap, 10);

            int changedEvents = 0;
            inv.OnInventoryChanged += () => changedEvents++;

            var bill = new InventoryBill().AddCost("scrap_metal", 4);

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                inv.TryExecuteTransaction(bill, onCommitted: () =>
                {
                    throw new InvalidOperationException("External domain action failed!");
                });
            });

            Assert.Equal("External domain action failed!", ex.Message);
            // Inventory must be fully restored to 10 scrap
            Assert.Equal(10, inv.CountById("scrap_metal"));
            Assert.Equal(0, changedEvents);
        }

        [Fact]
        public void Execute_ExplicitCancel_RollsBackStagedChanges()
        {
            var inv = new InventoryContainer { Capacity = 10 };
            var scrap = Def("scrap_metal");
            inv.Add(scrap, 10);

            var bill = new InventoryBill().AddCost("scrap_metal", 4);

            using (var tx = inv.BeginTransaction(bill))
            {
                Assert.True(tx.Validation.IsValid);
                tx.Cancel();
                Assert.True(tx.IsCancelled);
                Assert.False(tx.TryCommit());
            }

            Assert.Equal(10, inv.CountById("scrap_metal"));
        }

        [Fact]
        public void Execute_DisposeUncommitted_RollsBackAutomatically()
        {
            var inv = new InventoryContainer { Capacity = 10 };
            var scrap = Def("scrap_metal");
            inv.Add(scrap, 10);

            var bill = new InventoryBill().AddCost("scrap_metal", 5);

            using (var tx = inv.BeginTransaction(bill))
            {
                // Disposed without commit
            }

            Assert.Equal(10, inv.CountById("scrap_metal"));
        }

        [Fact]
        public void Events_FiredExactlyOncePerCommittedTransaction()
        {
            var inv = new InventoryContainer { Capacity = 10 };
            var scrap = Def("scrap_metal");
            var brass = Def("brass_fittings");
            var fuel = Def("fuel");

            inv.Add(scrap, 10);
            inv.Add(brass, 5);

            int changedEventCount = 0;
            var removedEvents = new List<(string id, int qty)>();
            var addedEvents = new List<(string id, int qty)>();

            inv.OnInventoryChanged += () => changedEventCount++;
            inv.OnItemRemoved += (def, qty) => removedEvents.Add((def.id, qty));
            inv.OnItemAdded += (def, qty) => addedEvents.Add((def.id, qty));

            var bill = new InventoryBill()
                .AddCost("scrap_metal", 3)
                .AddCost("brass_fittings", 2)
                .AddGrant(fuel, 4);

            bool success = inv.TryExecuteTransaction(bill);

            Assert.True(success);
            Assert.Equal(1, changedEventCount); // Exactly ONCE

            Assert.Equal(2, removedEvents.Count);
            Assert.Contains(removedEvents, r => r.id == "scrap_metal" && r.qty == 3);
            Assert.Contains(removedEvents, r => r.id == "brass_fittings" && r.qty == 2);

            Assert.Single(addedEvents);
            Assert.Equal("fuel", addedEvents[0].id);
            Assert.Equal(4, addedEvents[0].qty);
        }

        [Fact]
        public void DeepCoast_MultiResourceBill_AtomicConsumption()
        {
            var dc = new District8DeepCoastSystem(12);
            dc.SurveyPerimeter(1);
            dc.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 2, new SeededRng(12));

            // Perimeter bill on StabilizeRepair: 3 scrap_metal + 1 brass_fittings
            var inv = new InventoryContainer { Capacity = 10 };
            inv.Add(Def("scrap_metal"), 3);
            // Missing brass_fittings

            // Attempt clear with missing 2nd resource
            bool cleared = dc.TryClearPerimeter(3, bill => inv.TryConsumeBill(bill));
            Assert.False(cleared);
            Assert.False(dc.State.perimeterCleared);
            // 0 scrap consumed
            Assert.Equal(3, inv.CountById("scrap_metal"));

            // Supply remaining resource
            inv.Add(Def("brass_fittings"), 1);
            bool clearedSuccess = dc.TryClearPerimeter(3, bill => inv.TryConsumeBill(bill));
            Assert.True(clearedSuccess);
            Assert.True(dc.State.perimeterCleared);
            Assert.Equal(0, inv.CountById("scrap_metal"));
            Assert.Equal(0, inv.CountById("brass_fittings"));
        }

        [Fact]
        public void HoldfastTradeInventory_TryConsumeBill_IsAtomic()
        {
            var tradeInv = new HoldfastTradeInventory();
            tradeInv.AddItem("scrap_metal", 5);
            tradeInv.AddItem("clean_water", 2);

            var bill = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                { "scrap_metal", 4 },
                { "clean_water", 2 },
                { "fuel", 1 } // Missing fuel
            };

            bool consumed = tradeInv.TryConsumeBill(bill);
            Assert.False(consumed);
            // Entire inventory state preserved
            Assert.Equal(5, tradeInv.Items["scrap_metal"]);
            Assert.Equal(2, tradeInv.Items["clean_water"]);
            Assert.False(tradeInv.Items.ContainsKey("fuel"));

            // Satisfy fuel
            tradeInv.AddItem("fuel", 1);
            bool consumedSuccess = tradeInv.TryConsumeBill(bill);
            Assert.True(consumedSuccess);
            Assert.Equal(1, tradeInv.Items["scrap_metal"]);
            Assert.False(tradeInv.Items.ContainsKey("clean_water"));
            Assert.False(tradeInv.Items.ContainsKey("fuel"));
        }
    }
}
