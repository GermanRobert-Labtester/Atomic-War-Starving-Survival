// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests
{
    public class SumpNodeLookupBenchmarkTests
    {
        private readonly ITestOutputHelper _output;

        public SumpNodeLookupBenchmarkTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static SumpFloodingSystem CreateSystem()
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            var state = new PowerGridState
            {
                GenerationWatts = 800,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("sump_a", "Lower Level", 100f)
            };
            var power = new PowerGridSystem(state, rooms, new SeededRng(42));
            var df = new YearOfAshDeepFreezeSystem();
            return new SumpFloodingSystem(new SeededRng(42), weather, power, df);
        }

        [Fact]
        public void Benchmark_LinearVsDictionaryNodeLookup()
        {
            const int nodeCount = 100;
            const int iterations = 50000;

            var sys = CreateSystem();
            var nodeIds = new List<string>(nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                string id = $"sump_node_{i:D4}";
                nodeIds.Add(id);
                sys.AddNode(id, $"Sump Basin {i}", 200f);
            }

            // Warmup
            long warmupSum = 0;
            for (int it = 0; it < 200; it++)
            {
                for (int i = 0; i < nodeIds.Count; i++)
                {
                    var n = sys.GetNode(nodeIds[i]);
                    if (n != null) warmupSum++;
                }
            }
            Assert.True(warmupSum > 0);

            // Baseline: Linear search via List.Find
            var swBaseline = Stopwatch.StartNew();
            long baselineMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % nodeCount;
                string queryId = nodeIds[idx];
                var n = sys.State.nodes.Find(node => node.nodeId == queryId);
                if (n != null) baselineMatches++;
            }
            swBaseline.Stop();
            long baselineMs = swBaseline.ElapsedMilliseconds;

            // Optimized: Direct sys.GetNode(queryId) leveraging internal O(1) dictionary index
            var swOptimized = Stopwatch.StartNew();
            long optimizedMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % nodeCount;
                string queryId = nodeIds[idx];
                var n = sys.GetNode(queryId);
                if (n != null)
                {
                    optimizedMatches++;
                }
            }
            swOptimized.Stop();
            long optimizedMs = swOptimized.ElapsedMilliseconds;

            // Correctness check: identical results
            Assert.Equal(baselineMatches, optimizedMatches);

            double speedup = baselineMs > 0 ? (double)baselineMs / Math.Max(1, optimizedMs) : 1.0;
            _output.WriteLine($"[Benchmark Results: Node Lookup] Iterations: {iterations}, Nodes: {nodeCount}");
            _output.WriteLine($"Baseline (Linear List.Find): {baselineMs} ms");
            _output.WriteLine($"Optimized (SumpFloodingSystem.GetNode): {optimizedMs} ms");
            _output.WriteLine($"Speedup factor: {speedup:F2}x faster");

            Assert.True(optimizedMs <= baselineMs, $"Optimized ({optimizedMs}ms) was not faster than baseline ({baselineMs}ms)");
        }

        [Fact]
        public void Benchmark_SystemOperations_IsNodeAvailable()
        {
            const int nodeCount = 100;
            const int iterations = 50000;

            var sys = CreateSystem();
            var nodeIds = new List<string>(nodeCount);
            for (int i = 0; i < nodeCount; i++)
            {
                string id = $"sump_node_{i:D4}";
                nodeIds.Add(id);
                sys.AddNode(id, $"Sump Basin {i}", 200f);
            }

            var sw = Stopwatch.StartNew();
            long availableCount = 0;
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % nodeCount;
                if (sys.IsNodeAvailable(nodeIds[idx]))
                {
                    availableCount++;
                }
            }
            sw.Stop();

            _output.WriteLine($"[Benchmark Results: IsNodeAvailable] Iterations: {iterations}, Elapsed: {sw.ElapsedMilliseconds} ms");
            Assert.Equal(iterations, availableCount);
        }

        [Fact]
        public void GetNode_Correctness_EdgeCasesAndLifecycle()
        {
            var sys = CreateSystem();
            Assert.Null(sys.GetNode(null!));
            Assert.Null(sys.GetNode(string.Empty));
            Assert.Null(sys.GetNode("non_existent"));

            sys.AddNode("node_1", "Basin 1", 150f);
            sys.AddNode("node_2", "Basin 2", 250f);

            var n1 = sys.GetNode("node_1");
            Assert.NotNull(n1);
            Assert.Equal("node_1", n1!.nodeId);
            Assert.Equal("Basin 1", n1.displayName);
            Assert.Equal(150f, n1.maxWaterLevelCm);

            var n2 = sys.GetNode("node_2");
            Assert.NotNull(n2);
            Assert.Equal("node_2", n2!.nodeId);

            // Capture & restore preserves lookup
            var captured = sys.CaptureState();
            var sys2 = CreateSystem();
            sys2.RestoreState(captured);

            var restoredNode = sys2.GetNode("node_1");
            Assert.NotNull(restoredNode);
            Assert.Equal("node_1", restoredNode!.nodeId);
        }
    }
}
