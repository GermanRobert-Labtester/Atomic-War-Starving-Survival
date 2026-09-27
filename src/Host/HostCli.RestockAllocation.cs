// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RestockAllocationSelfTest
// Core Authority     : Ashfall.Core.Economy.RestockAllocationEngine (F13-C / XP-04)
// Purpose            : deterministic integer restock allocation over categories
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliRestockAllocation
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Restock Capacity Allocation Self-Test (F13-C) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var food = new RestockCategory("Food", weight: 3, scarcityFloor: 5, authoredOrder: 0,
                    new[] { new RestockItemCandidate("item_canned_food", currentStock: 10, targetPar: 20, authoredRestockOrder: 1) });
                var medical = new RestockCategory("Medical", weight: 1, scarcityFloor: 10, authoredOrder: 1,
                    new[] { new RestockItemCandidate("item_bandage", currentStock: 2, targetPar: 10, authoredRestockOrder: 1) });

                var exact = RestockAllocationEngine.Allocate(10, new[] { food, medical });
                if (exact.TotalCapacityRequested == 10 && exact.TotalAllocated == 10
                    && exact.CategoryAllocations["Food"] == 6 && exact.CategoryAllocations["Medical"] == 4
                    && exact.ItemAllocations["item_canned_food"] == 6 && exact.ItemAllocations["item_bandage"] == 4)
                {
                    Console.WriteLine("[PASS] Check 1: weighted capacity splits exactly 6/4 and reaches the items.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: exact = {exact.CategoryAllocations["Food"]}/{exact.CategoryAllocations["Medical"]}."); }

                var fuelItems = new[] { new RestockItemCandidate("fuel", currentStock: 4, targetPar: 20) };
                var waterItems = new[] { new RestockItemCandidate("water", currentStock: 10, targetPar: 20) };
                var scarse = RestockAllocationEngine.Allocate(6, new[]
                {
                    new RestockCategory("Fuel", weight: 2, scarcityFloor: 5, authoredOrder: 0, fuelItems),
                    new RestockCategory("Water", weight: 2, scarcityFloor: 5, authoredOrder: 1, waterItems)
                });
                if (scarse.CategoryAllocations["Fuel"] == 4 && scarse.CategoryAllocations["Water"] == 2)
                {
                    Console.WriteLine("[PASS] Check 2: a category below its scarcity floor receives the doubled weight.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: scarcity = {scarse.CategoryAllocations["Fuel"]}/{scarse.CategoryAllocations["Water"]}."); }

                var catA = new RestockCategory("CatA", 1, 0, 0, new[] { new RestockItemCandidate("item_a", 0, 10) });
                var catB = new RestockCategory("CatB", 1, 0, 1, new[] { new RestockItemCandidate("item_b", 0, 10) });
                var catC = new RestockCategory("CatC", 1, 0, 2, new[] { new RestockItemCandidate("item_c", 0, 10) });
                var remainder = RestockAllocationEngine.Allocate(4, new[] { catA, catB, catC });
                if (remainder.TotalAllocated == 4 && remainder.CategoryAllocations["CatA"] == 2
                    && remainder.CategoryAllocations["CatB"] == 1 && remainder.CategoryAllocations["CatC"] == 1)
                {
                    Console.WriteLine("[PASS] Check 3: the largest-remainder unit goes to the lowest authored order.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: remainder = {remainder.CategoryAllocations["CatA"]}/{remainder.CategoryAllocations["CatB"]}/{remainder.CategoryAllocations["CatC"]}."); }

                var mix = new RestockCategory("General", 1, 0, 0, new[]
                {
                    new RestockItemCandidate("item_a", 5, 10, authoredRestockOrder: 2),
                    new RestockItemCandidate("item_b", 1, 10, authoredRestockOrder: 0),
                    new RestockItemCandidate("item_c", 2, 20, authoredRestockOrder: 1)
                });
                var within = RestockAllocationEngine.Allocate(5, new[] { mix });
                if (within.SortedItemIds.Count == 3 && within.SortedItemIds[0] == "item_b"
                    && within.SortedItemIds[1] == "item_c" && within.SortedItemIds[2] == "item_a"
                    && within.ItemAllocations["item_b"] == 5 && within.ItemAllocations["item_c"] == 0)
                {
                    Console.WriteLine("[PASS] Check 4: items sort by stock/par ratio with authored-order tie-break.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: within = {string.Join(",", within.SortedItemIds)}."); }

                var run1 = RestockAllocationEngine.Allocate(7, new[] { food, medical });
                var run2 = RestockAllocationEngine.Allocate(7, new[] { food, medical });
                if (run1.TotalAllocated == run2.TotalAllocated
                    && run1.CategoryAllocations.SequenceEqual(run2.CategoryAllocations)
                    && run1.ItemAllocations.SequenceEqual(run2.ItemAllocations))
                {
                    Console.WriteLine("[PASS] Check 5: identical inputs allocate identically (deterministic).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 5: determinism violated."); }

                var zero = RestockAllocationEngine.Allocate(0, Array.Empty<RestockCategory>());
                var negative = RestockAllocationEngine.Allocate(-5, Array.Empty<RestockCategory>());
                if (zero.TotalAllocated == 0 && negative.TotalAllocated == 0)
                {
                    Console.WriteLine("[PASS] Check 6: zero/negative capacity allocates nothing.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: zero {zero.TotalAllocated} / negative {negative.TotalAllocated}."); }

                var nullCategories = RestockAllocationEngine.Allocate(5, null!);
                if (nullCategories.TotalAllocated == 0 && nullCategories.TotalCapacityRequested == 5)
                {
                    Console.WriteLine("[PASS] Check 7: a null category list is refused without allocating.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: null = {nullCategories.TotalAllocated}."); }

                var zeroWeight = RestockAllocationEngine.Allocate(5, new[]
                {
                    new RestockCategory("Dead", weight: 0, scarcityFloor: 0, authoredOrder: 0,
                        new[] { new RestockItemCandidate("x", currentStock: 0, targetPar: 5) })
                });
                if (zeroWeight.TotalAllocated == 0 && zeroWeight.CategoryAllocations["Dead"] == 0 && zeroWeight.ItemAllocations["x"] == 0)
                {
                    Console.WriteLine("[PASS] Check 8: an all-zero-weight allocation falls through with nothing allocated.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: zero-weight = {zeroWeight.TotalAllocated}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Restock allocation: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
