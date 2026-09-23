// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Item candidate for partial-capacity restock allocation (XP-04 / UNBLOCK-02 F13-C).
    /// </summary>
    public sealed class RestockItemCandidate
    {
        public string ItemId { get; }
        public int CurrentStock { get; }
        public int TargetPar { get; }
        public int AuthoredRestockOrder { get; }

        public RestockItemCandidate(string itemId, int currentStock, int targetPar, int authoredRestockOrder = 0)
        {
            ItemId = itemId ?? string.Empty;
            CurrentStock = Math.Max(0, currentStock);
            TargetPar = Math.Max(1, targetPar);
            AuthoredRestockOrder = authoredRestockOrder;
        }

        public int Deficit => Math.Max(0, TargetPar - CurrentStock);
    }

    /// <summary>
    /// Category grouping for restock allocation with weight, scarcity floor, and candidate items.
    /// </summary>
    public sealed class RestockCategory
    {
        public string Name { get; }
        public int Weight { get; }
        public int ScarcityFloor { get; }
        public int AuthoredOrder { get; }
        public IReadOnlyList<RestockItemCandidate> Items { get; }

        public RestockCategory(
            string name,
            int weight,
            int scarcityFloor,
            int authoredOrder,
            IEnumerable<RestockItemCandidate>? items)
        {
            Name = name ?? string.Empty;
            Weight = Math.Max(0, weight);
            ScarcityFloor = Math.Max(0, scarcityFloor);
            AuthoredOrder = authoredOrder;
            Items = new List<RestockItemCandidate>(items ?? Array.Empty<RestockItemCandidate>()).AsReadOnly();
        }

        public int TotalStock => Items.Sum(i => i.CurrentStock);
        public int TotalDeficit => Items.Sum(i => i.Deficit);
    }

    /// <summary>
    /// Result of deterministic partial-capacity restock allocation.
    /// </summary>
    public sealed class RestockAllocationResult
    {
        public int TotalCapacityRequested { get; }
        public int TotalAllocated { get; }
        public IReadOnlyDictionary<string, int> CategoryAllocations { get; }
        public IReadOnlyDictionary<string, int> ItemAllocations { get; }
        public IReadOnlyList<string> SortedItemIds { get; }

        public RestockAllocationResult(
            int requested,
            int totalAllocated,
            IReadOnlyDictionary<string, int> categoryAllocations,
            IReadOnlyDictionary<string, int> itemAllocations,
            IReadOnlyList<string> sortedItemIds)
        {
            TotalCapacityRequested = requested;
            TotalAllocated = totalAllocated;
            CategoryAllocations = categoryAllocations;
            ItemAllocations = itemAllocations;
            SortedItemIds = sortedItemIds;
        }
    }

    /// <summary>
    /// XP-04 / UNBLOCK-02 F13-C: Deterministic integer restock allocation engine.
    /// Allocates partial capacity across categories using effective weights (boosted when below scarcity floor)
    /// and largest-remainder proportional rounding, then sorts items within each category by stock/target_par
    /// ascending, tie-breaking on authored restock order, then item id ordinal.
    /// </summary>
    public static class RestockAllocationEngine
    {
        public static RestockAllocationResult Allocate(
            int capacity,
            IReadOnlyList<RestockCategory> categories)
        {
            if (capacity <= 0 || categories == null || categories.Count == 0)
            {
                return new RestockAllocationResult(
                    Math.Max(0, capacity),
                    0,
                    new Dictionary<string, int>(StringComparer.Ordinal),
                    new Dictionary<string, int>(StringComparer.Ordinal),
                    Array.Empty<string>());
            }

            // 1. Effective weight = weight * (stock < scarcity_floor ? 2 : 1)
            var effWeights = new int[categories.Count];
            long totalEff = 0;
            for (int i = 0; i < categories.Count; i++)
            {
                var cat = categories[i];
                int mult = cat.TotalStock < cat.ScarcityFloor ? 2 : 1;
                int eff = cat.Weight * mult;
                effWeights[i] = eff;
                totalEff += eff;
            }

            var categoryAllocations = new Dictionary<string, int>(StringComparer.Ordinal);
            var itemAllocations = new Dictionary<string, int>(StringComparer.Ordinal);
            var allSortedItemIds = new List<string>();

            if (totalEff == 0)
            {
                foreach (var cat in categories)
                {
                    categoryAllocations[cat.Name] = 0;
                    foreach (var item in cat.Items)
                    {
                        itemAllocations[item.ItemId] = 0;
                    }
                }
                return new RestockAllocationResult(capacity, 0, categoryAllocations, itemAllocations, Array.Empty<string>());
            }

            // 2. Allocate floor(capacity * eff / total_eff) to each category
            var baseAllocations = new int[categories.Count];
            var remainders = new long[categories.Count];
            int allocatedSoFar = 0;

            for (int i = 0; i < categories.Count; i++)
            {
                long product = (long)capacity * effWeights[i];
                baseAllocations[i] = (int)(product / totalEff);
                remainders[i] = product % totalEff;
                allocatedSoFar += baseAllocations[i];
            }

            // 3. Distribute remaining units by largest fractional remainder
            // Ties broken by authored category order, then category name ordinal
            int unallocated = capacity - allocatedSoFar;
            var categoryIndices = Enumerable.Range(0, categories.Count).ToList();
            categoryIndices.Sort((a, b) =>
            {
                int remCmp = remainders[b].CompareTo(remainders[a]); // largest first
                if (remCmp != 0) return remCmp;
                int orderCmp = categories[a].AuthoredOrder.CompareTo(categories[b].AuthoredOrder);
                if (orderCmp != 0) return orderCmp;
                return string.Compare(categories[a].Name, categories[b].Name, StringComparison.Ordinal);
            });

            for (int i = 0; i < unallocated && i < categoryIndices.Count; i++)
            {
                baseAllocations[categoryIndices[i]]++;
            }

            for (int i = 0; i < categories.Count; i++)
            {
                categoryAllocations[categories[i].Name] = baseAllocations[i];
            }

            int totalAllocated = 0;

            // 4. Within each category:
            // Sort items by stock / target_par ascending (rational compare),
            // tie-break authored restock_order, then item id ordinal.
            for (int i = 0; i < categories.Count; i++)
            {
                var cat = categories[i];
                int catCap = baseAllocations[i];
                var items = cat.Items.ToList();

                items.Sort((a, b) =>
                {
                    // Rational compare: a.CurrentStock / a.TargetPar vs b.CurrentStock / b.TargetPar
                    long crossA = (long)a.CurrentStock * b.TargetPar;
                    long crossB = (long)b.CurrentStock * a.TargetPar;
                    int ratioCmp = crossA.CompareTo(crossB);
                    if (ratioCmp != 0) return ratioCmp;

                    int orderCmp = a.AuthoredRestockOrder.CompareTo(b.AuthoredRestockOrder);
                    if (orderCmp != 0) return orderCmp;

                    return string.Compare(a.ItemId, b.ItemId, StringComparison.Ordinal);
                });

                foreach (var item in items)
                {
                    allSortedItemIds.Add(item.ItemId);
                }

                // Allocate capacity to items in sorted order
                int catRemaining = catCap;

                // Pass 1: Fill deficits up to target par
                foreach (var item in items)
                {
                    if (catRemaining <= 0)
                    {
                        itemAllocations[item.ItemId] = 0;
                        continue;
                    }

                    int need = item.Deficit;
                    int take = Math.Min(catRemaining, need);
                    itemAllocations[item.ItemId] = take;
                    catRemaining -= take;
                    totalAllocated += take;
                }

                // Pass 2: If surplus remains, distribute round-robin in sorted priority order
                if (catRemaining > 0 && items.Count > 0)
                {
                    while (catRemaining > 0)
                    {
                        foreach (var item in items)
                        {
                            if (catRemaining <= 0) break;
                            itemAllocations[item.ItemId]++;
                            catRemaining--;
                            totalAllocated++;
                        }
                    }
                }
            }

            return new RestockAllocationResult(
                capacity,
                totalAllocated,
                categoryAllocations,
                itemAllocations,
                allSortedItemIds.AsReadOnly());
        }
    }
}
