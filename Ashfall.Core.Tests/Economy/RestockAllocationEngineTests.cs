// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class RestockAllocationEngineTests
    {
        [Fact]
        public void ExactAllocation_WorkedExample()
        {
            var foodItems = new List<RestockItemCandidate>
            {
                new RestockItemCandidate("item_canned_food", currentStock: 10, targetPar: 20, authoredRestockOrder: 1)
            };
            var medItems = new List<RestockItemCandidate>
            {
                new RestockItemCandidate("item_bandage", currentStock: 2, targetPar: 10, authoredRestockOrder: 1)
            };

            // Food: weight 3, scarcity floor 5. Total stock 10 >= 5 -> eff = 3 * 1 = 3.
            var foodCategory = new RestockCategory("Food", weight: 3, scarcityFloor: 5, authoredOrder: 0, foodItems);
            // Med: weight 1, scarcity floor 10. Total stock 2 < 10 -> eff = 1 * 2 = 2.
            var medCategory = new RestockCategory("Medical", weight: 1, scarcityFloor: 10, authoredOrder: 1, medItems);

            // Total eff = 3 + 2 = 5. Capacity = 10.
            // Food base = floor(10 * 3 / 5) = 6. Remainder = 0.
            // Med base = floor(10 * 2 / 5) = 4. Remainder = 0.
            var result = RestockAllocationEngine.Allocate(10, new[] { foodCategory, medCategory });

            Assert.Equal(10, result.TotalCapacityRequested);
            Assert.Equal(10, result.TotalAllocated);
            Assert.Equal(6, result.CategoryAllocations["Food"]);
            Assert.Equal(4, result.CategoryAllocations["Medical"]);
            Assert.Equal(6, result.ItemAllocations["item_canned_food"]);
            Assert.Equal(4, result.ItemAllocations["item_bandage"]);
        }

        [Fact]
        public void ScarcityFloor_TriggersMultiplier()
        {
            var items = new List<RestockItemCandidate>
            {
                new RestockItemCandidate("fuel", currentStock: 4, targetPar: 20)
            };

            // With scarcity floor 5 (stock 4 < 5), multiplier is 2.
            var catScarse = new RestockCategory("Fuel", weight: 2, scarcityFloor: 5, authoredOrder: 0, items);
            // With scarcity floor 4 (stock 4 not < 4), multiplier is 1.
            var catAdequate = new RestockCategory("Fuel", weight: 2, scarcityFloor: 4, authoredOrder: 0, items);

            var otherItems = new List<RestockItemCandidate>
            {
                new RestockItemCandidate("water", currentStock: 10, targetPar: 20)
            };
            var otherCat = new RestockCategory("Water", weight: 2, scarcityFloor: 5, authoredOrder: 1, otherItems);

            // Scarse fuel: eff Fuel = 4, eff Water = 2. Total eff = 6. Cap = 6. Fuel = 4, Water = 2.
            var resScarse = RestockAllocationEngine.Allocate(6, new[] { catScarse, otherCat });
            Assert.Equal(4, resScarse.CategoryAllocations["Fuel"]);
            Assert.Equal(2, resScarse.CategoryAllocations["Water"]);

            // Adequate fuel: eff Fuel = 2, eff Water = 2. Total eff = 4. Cap = 6. Each gets 3.
            var resAdequate = RestockAllocationEngine.Allocate(6, new[] { catAdequate, otherCat });
            Assert.Equal(3, resAdequate.CategoryAllocations["Fuel"]);
            Assert.Equal(3, resAdequate.CategoryAllocations["Water"]);
        }

        [Fact]
        public void LargestRemainder_DistributesSurplusAndHandlesEqualRemainders()
        {
            var catA = new RestockCategory("CatA", weight: 1, scarcityFloor: 0, authoredOrder: 0, new[] { new RestockItemCandidate("item_a", 0, 10) });
            var catB = new RestockCategory("CatB", weight: 1, scarcityFloor: 0, authoredOrder: 1, new[] { new RestockItemCandidate("item_b", 0, 10) });
            var catC = new RestockCategory("CatC", weight: 1, scarcityFloor: 0, authoredOrder: 2, new[] { new RestockItemCandidate("item_c", 0, 10) });

            // 3 categories with equal weight 1. Cap = 4. Total eff = 3.
            // Base: 4 * 1 / 3 = 1 each. Total base = 3. Unallocated = 1.
            // Remainders for all 3 are 1.
            // Tie-break by authoredOrder -> CatA gets the extra unit!
            var result = RestockAllocationEngine.Allocate(4, new[] { catA, catB, catC });

            Assert.Equal(4, result.TotalAllocated);
            Assert.Equal(2, result.CategoryAllocations["CatA"]);
            Assert.Equal(1, result.CategoryAllocations["CatB"]);
            Assert.Equal(1, result.CategoryAllocations["CatC"]);
        }

        [Fact]
        public void WithinCategory_SortsByStockToParRatio_AndTieBreaks()
        {
            var itemA = new RestockItemCandidate("item_a", currentStock: 5, targetPar: 10, authoredRestockOrder: 2); // ratio 0.5
            var itemB = new RestockItemCandidate("item_b", currentStock: 1, targetPar: 10, authoredRestockOrder: 0); // ratio 0.1
            var itemC = new RestockItemCandidate("item_c", currentStock: 2, targetPar: 20, authoredRestockOrder: 1); // ratio 0.1

            var cat = new RestockCategory("General", weight: 1, scarcityFloor: 0, authoredOrder: 0, new[] { itemA, itemB, itemC });

            var result = RestockAllocationEngine.Allocate(5, new[] { cat });

            // Sorted items:
            // 1. item_b (ratio 0.1, authored order 0)
            // 2. item_c (ratio 0.1, authored order 1)
            // 3. item_a (ratio 0.5, authored order 2)
            Assert.Equal(3, result.SortedItemIds.Count);
            Assert.Equal("item_b", result.SortedItemIds[0]);
            Assert.Equal("item_c", result.SortedItemIds[1]);
            Assert.Equal("item_a", result.SortedItemIds[2]);

            // Deficits:
            // item_b: need 9 (10 - 1). Takes min(5, 9) = 5.
            // item_c: need 18, takes 0.
            // item_a: need 5, takes 0.
            Assert.Equal(5, result.ItemAllocations["item_b"]);
            Assert.Equal(0, result.ItemAllocations["item_c"]);
            Assert.Equal(0, result.ItemAllocations["item_a"]);
        }

        [Fact]
        public void DeterminismAndEdgeCases()
        {
            var emptyRes1 = RestockAllocationEngine.Allocate(0, Array.Empty<RestockCategory>());
            Assert.Equal(0, emptyRes1.TotalAllocated);

            var emptyRes2 = RestockAllocationEngine.Allocate(-5, Array.Empty<RestockCategory>());
            Assert.Equal(0, emptyRes2.TotalAllocated);

            var cat = new RestockCategory("Cat", 1, 0, 0, new[] { new RestockItemCandidate("x", 0, 5) });
            var run1 = RestockAllocationEngine.Allocate(3, new[] { cat });
            var run2 = RestockAllocationEngine.Allocate(3, new[] { cat });

            Assert.Equal(run1.TotalAllocated, run2.TotalAllocated);
            Assert.Equal(run1.CategoryAllocations["Cat"], run2.CategoryAllocations["Cat"]);
            Assert.Equal(run1.ItemAllocations["x"], run2.ItemAllocations["x"]);
        }
    }
}
