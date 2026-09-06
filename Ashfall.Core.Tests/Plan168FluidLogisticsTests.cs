using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan168FluidLogisticsTests
    {
        private sealed class FixedRng : ISeededRng
        {
            private readonly Queue<double> _values;
            public FixedRng(params double[] values) => _values = new Queue<double>(values);
            public int Seed => 168;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)NextDouble();
            public double NextDouble() => _values.Count > 0 ? _values.Dequeue() : 0.99;
        }

        private static FluidLogisticsSystem CreateGraph()
        {
            var system = new FluidLogisticsSystem();
            Assert.True(system.AddNode(new FluidNodeState
            {
                nodeId = "source_a",
                nodeType = FluidNodeType.Source,
                capacity = 100f,
                pressure = 1f,
                quality = new FluidQuality()
            }));
            Assert.True(system.AddNode(new FluidNodeState
            {
                nodeId = "source_b",
                nodeType = FluidNodeType.Source,
                capacity = 100f,
                pressure = 1f,
                quality = new FluidQuality { pathogen01 = 1f }
            }));
            Assert.True(system.AddNode(new FluidNodeState
            {
                nodeId = "sink_drinking",
                nodeType = FluidNodeType.Sink,
                demand = 40f,
                priority = 1,
                pressure = 0.5f
            }));
            Assert.True(system.AddEdge(new FluidEdgeState { edgeId = "pipe_a", fromNodeId = "source_a", toNodeId = "sink_drinking", maxFlow = 100f, resistance = 0.1f }));
            Assert.True(system.AddEdge(new FluidEdgeState { edgeId = "pipe_b", fromNodeId = "source_b", toNodeId = "sink_drinking", maxFlow = 100f, resistance = 0.1f }));
            return system;
        }

        [Fact]
        public void DistributionConservesMassAndMixesQualityByVolume()
        {
            var system = CreateGraph();
            Assert.True(system.InjectWater("source_a", 10f, new FluidQuality()));
            Assert.True(system.InjectWater("source_b", 30f, new FluidQuality { pathogen01 = 1f }));

            var report = system.Solve(4);

            Assert.Equal(40f, report.deliveredVolume, 4);
            Assert.Equal(0f, report.endingVolume, 4);
            Assert.Equal(0f, report.massBalanceError, 4);
            Assert.Equal(0.75f, report.qualityBySink["sink_drinking"].pathogen01, 4);
        }

        [Fact]
        public void ClosedValveBlocksFlowWithoutDestroyingStoredWater()
        {
            var system = CreateGraph();
            Assert.True(system.InjectWater("source_a", 10f, new FluidQuality()));
            Assert.True(system.SetValve("pipe_a", 0f));

            var report = system.Solve(2);

            Assert.Equal(0f, report.deliveredVolume, 4);
            Assert.Equal(10f, report.endingVolume, 4);
            Assert.Equal(0f, report.massBalanceError, 4);
        }

        [Fact]
        public void PumpPowerLossStopsPoweredPathAndRecoveryRestoresIt()
        {
            var system = new FluidLogisticsSystem();
            Assert.True(system.AddNode(new FluidNodeState { nodeId = "source", nodeType = FluidNodeType.Source, capacity = 100f, pressure = 0.2f }));
            Assert.True(system.AddNode(new FluidNodeState { nodeId = "pump", nodeType = FluidNodeType.Pump, pumpPowered = false, pumpHeadPressure = 1f }));
            Assert.True(system.AddNode(new FluidNodeState { nodeId = "sink", nodeType = FluidNodeType.Sink, demand = 5f, pressure = 0.5f }));
            Assert.True(system.AddEdge(new FluidEdgeState { edgeId = "in", fromNodeId = "source", toNodeId = "pump", maxFlow = 100f, resistance = 0.05f }));
            Assert.True(system.AddEdge(new FluidEdgeState { edgeId = "out", fromNodeId = "pump", toNodeId = "sink", maxFlow = 100f, resistance = 0.05f }));
            Assert.True(system.InjectWater("source", 5f, new FluidQuality()));

            Assert.Equal(0f, system.Solve(1, powerAvailability01: 0f).deliveredVolume, 4);
            Assert.True(system.SetPumpState("pump", true));
            Assert.Equal(5f, system.Solve(2).deliveredVolume, 4);
        }

        [Fact]
        public void FreezeBurstIsSeededAndRepairRestoresTopology()
        {
            var system = CreateGraph();
            system.BindRng(new FixedRng(0.0, 0.99));
            Assert.True(system.InjectWater("source_a", 10f, new FluidQuality()));
            string? burstEdge = null;
            system.OnPipeBurst += edge => burstEdge = edge;

            var frozen = system.Tick(8, outdoorTemperatureC: -20f);

            Assert.Equal("pipe_a", burstEdge);
            Assert.Equal(0f, frozen.deliveredVolume, 4);
            Assert.True(system.RepairPipe("pipe_a"));
            Assert.Equal(10f, system.Solve(9).deliveredVolume, 4);
        }

        [Fact]
        public void StateRoundTripPreservesVolumeQualityAndPipeCondition()
        {
            var system = CreateGraph();
            Assert.True(system.InjectWater("source_a", 12f, new FluidQuality { chemical01 = 0.2f }));
            Assert.True(system.SetValve("pipe_a", 0.4f));
            var saved = system.CaptureState();

            var restored = new FluidLogisticsSystem();
            restored.RestoreState(saved);

            Assert.Equal(12f, restored.State.nodes[0].volume, 4);
            Assert.Equal(0.2f, restored.State.nodes[0].quality.chemical01, 4);
            Assert.Equal(0.4f, restored.State.edges[0].valveOpen01, 4);
        }

        [Fact]
        public void InfrastructureCatalogLoadsAndValidatesAuthoritativeData()
        {
            string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            string dataDir = System.IO.Path.Combine(root, "Assets", "StreamingAssets", "Data");
            var catalog = FluidInfrastructureCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(2, catalog.Pipes.Count);
            Assert.Equal(2, catalog.Pumps.Count);
            Assert.Equal(2, catalog.Reservoirs.Count);
            Assert.True(FluidInfrastructureCatalogLoader.Validate(catalog, out var error), error);
        }
    }
}
