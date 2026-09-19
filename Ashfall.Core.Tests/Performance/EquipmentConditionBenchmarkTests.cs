// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests
{
    public class EquipmentConditionBenchmarkTests
    {
        private readonly ITestOutputHelper _output;

        public EquipmentConditionBenchmarkTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static EquipmentConditionSystem CreateSystem()
        {
            var rng = new SeededRng(42);
            var inv = new Inventory.Inventory();
            var crafting = new CraftingSystem(inv);
            return new EquipmentConditionSystem(rng, inv, crafting);
        }

        [Fact]
        public void Benchmark_LinearVsDictionaryItemLookup()
        {
            const int itemCount = 100;
            const int iterations = 50000;

            var sys = CreateSystem();
            var instanceIds = new List<string>(itemCount);

            for (int i = 0; i < itemCount; i++)
            {
                string id = $"equip_inst_{i:D4}";
                instanceIds.Add(id);
                sys.RegisterItem(id, $"item_tool_{i % 10}", "survivor_1", EquipmentFamily.Tool, 100f);
            }

            // Warmup
            long warmupSum = 0;
            for (int it = 0; it < 200; it++)
            {
                for (int i = 0; i < instanceIds.Count; i++)
                {
                    var item = sys.GetItem(instanceIds[i]);
                    if (item != null) warmupSum++;
                }
            }
            Assert.True(warmupSum > 0);

            // Baseline: Linear search via List.Find
            var swBaseline = Stopwatch.StartNew();
            long baselineMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % itemCount;
                string queryId = instanceIds[idx];
                var item = sys.State.items.Find(i => i.instanceId == queryId);
                if (item != null) baselineMatches++;
            }
            swBaseline.Stop();
            long baselineMs = swBaseline.ElapsedMilliseconds;

            // Optimized: Direct sys.GetItem(queryId) leveraging internal O(1) dictionary index
            var swOptimized = Stopwatch.StartNew();
            long optimizedMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % itemCount;
                string queryId = instanceIds[idx];
                var item = sys.GetItem(queryId);
                if (item != null) optimizedMatches++;
            }
            swOptimized.Stop();
            long optimizedMs = swOptimized.ElapsedMilliseconds;

            // Correctness check: identical results
            Assert.Equal(baselineMatches, optimizedMatches);

            double speedup = baselineMs > 0 ? (double)baselineMs / Math.Max(1, optimizedMs) : 1.0;
            _output.WriteLine($"[Benchmark Results: Equipment Lookup] Iterations: {iterations}, Items: {itemCount}");
            _output.WriteLine($"Baseline (Linear List.Find): {baselineMs} ms");
            _output.WriteLine($"Optimized (EquipmentConditionSystem.GetItem): {optimizedMs} ms");
            _output.WriteLine($"Speedup factor: {speedup:F2}x faster");

            Assert.True(optimizedMs <= baselineMs, $"Optimized ({optimizedMs}ms) was not faster than baseline ({baselineMs}ms)");
        }

        [Fact]
        public void Benchmark_SystemOperations_IsUsableAndReduceDurability()
        {
            const int itemCount = 100;
            const int iterations = 30000;

            var sys = CreateSystem();
            var instanceIds = new List<string>(itemCount);
            for (int i = 0; i < itemCount; i++)
            {
                string id = $"equip_{i:D4}";
                instanceIds.Add(id);
                sys.RegisterItem(id, "item_weapon", "survivor_1", EquipmentFamily.Weapon, 100f);
            }

            var sw = Stopwatch.StartNew();
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % itemCount;
                string id = instanceIds[idx];
                sys.ReduceDurability(id, 0.001f);
                bool usable = sys.IsUsable(id);
                Assert.True(usable);
            }
            sw.Stop();

            _output.WriteLine($"[Benchmark Results: ReduceDurability + IsUsable] Iterations: {iterations}, Elapsed: {sw.ElapsedMilliseconds} ms");
        }

        [Fact]
        public void EquipmentConditionSystem_CorrectnessAndEdgeCases()
        {
            var sys = CreateSystem();

            // Null and empty safety
            Assert.Null(sys.GetItem(null!));
            Assert.Null(sys.GetItem(string.Empty));
            Assert.Null(sys.GetItem("non_existent"));

            // Register and retrieve
            sys.RegisterItem("tool_1", "item_axe", "survivor_1", EquipmentFamily.Tool, 100f);
            var item = sys.GetItem("tool_1");
            Assert.NotNull(item);
            Assert.Equal("tool_1", item!.instanceId);
            Assert.Equal(100f, item.condition);

            // ReduceDurability
            sys.ReduceDurability("tool_1", 25f);
            Assert.Equal(75f, item.condition);
            Assert.False(item.isBroken);

            // Reduce to 0 marks broken
            sys.ReduceDurability("tool_1", 80f);
            Assert.Equal(0f, item.condition);
            Assert.True(item.isBroken);

            // Save and restore
            var savedState = sys.CaptureState();
            var sys2 = CreateSystem();
            sys2.RestoreState(savedState);

            var restoredItem = sys2.GetItem("tool_1");
            Assert.NotNull(restoredItem);
            Assert.Equal(0f, restoredItem!.condition);
            Assert.True(restoredItem.isBroken);
        }
    }
}
