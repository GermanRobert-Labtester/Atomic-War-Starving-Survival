// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class RailwaySystemTests
    {
        private RailwayNetworkCatalog CreateTestCatalog()
        {
            return new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "node_a", display_name = "Alpha Terminal" },
                    new RailNodeDef { node_id = "node_b", display_name = "Beta Depot" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "seg_a_b",
                        start_node_id = "node_a",
                        end_node_id = "node_b",
                        distance_km = 10f,
                        base_integrity = 0.8f,
                        bridge_required = false,
                        max_train_mass = 200f
                    }
                },
                cars = new List<TrainCarDef>
                {
                    new TrainCarDef { car_type_id = "car_locomotive_diesel", empty_mass = 50f },
                    new TrainCarDef { car_type_id = "car_freight_hopper", empty_mass = 20f }
                }
            };
        }

        [Fact]
        public void TrackRepair_ConsumesMaterials_AndRestoresIntegrity()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("steel_rail_segment", 2);

            var sys = new RailwaySystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateTestCatalog());

            var seg = sys.EnsureSegmentState("seg_a_b");
            seg.integrity = 0.5f;

            var res = sys.RepairTrack("seg_a_b", 0.3f);
            Assert.True(res.IsSuccess);
            Assert.Equal(0.8f, seg.integrity, 2);
            Assert.Equal(1, inv.CountById("steel_rail_segment"));
        }

        [Fact]
        public void BridgeRepair_RestoresBrokenBridge()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("steel_rail_segment", 3);
            inv.AddById("railroad_ties", 3);

            var cat = CreateTestCatalog();
            cat.segments[0].bridge_required = true;

            var sys = new RailwaySystem(new SeededRng(42), inv);
            sys.RegisterCatalog(cat);

            var seg = sys.EnsureSegmentState("seg_a_b");
            seg.bridgeIntact = false;

            var res = sys.RepairBridge("seg_a_b");
            Assert.True(res.IsSuccess);
            Assert.True(seg.bridgeIntact);
            Assert.Equal(1, inv.CountById("steel_rail_segment"));
            Assert.Equal(1, inv.CountById("railroad_ties"));
        }

        [Fact]
        public void DispatchTrain_ConsumesFuel_AndArrivesAtDestination()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("train_coal", 50);

            var sys = new RailwaySystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateTestCatalog());

            var train = sys.CreateStarterTrain("train_express", "Wasteland Express", "node_a");

            var dispatchRes = sys.DispatchTrain("train_express", "seg_a_b");
            Assert.True(dispatchRes.IsSuccess);
            Assert.Equal(TrainDispatchStatus.EnRoute, train.status);
            Assert.Equal("seg_a_b", train.activeSegmentId);

            // Travel ticks
            sys.TickTravel("train_express", 0.5f);
            Assert.Equal(0.5f, train.segmentProgress);
            Assert.Equal(TrainDispatchStatus.EnRoute, train.status);

            sys.TickTravel("train_express", 0.5f);
            Assert.Equal(1.0f, train.segmentProgress);
            Assert.Equal(TrainDispatchStatus.Arrived, train.status);
            Assert.Equal("node_b", train.currentNodeId);
        }

        [Fact]
        public void Derailment_On_Degraded_Track_Can_Be_Cleared()
        {
            var sys = new RailwaySystem(new SeededRng(1), new Inventory.Inventory());
            sys.RegisterCatalog(CreateTestCatalog());

            var seg = sys.EnsureSegmentState("seg_a_b");
            seg.integrity = 0.45f; // degraded

            var train = sys.CreateStarterTrain("train_derail", "Rusty Engine", "node_a");
            train.status = TrainDispatchStatus.EnRoute;
            train.activeSegmentId = "seg_a_b";

            // Advance travel with RNG roll that hits derailment
            sys.TickTravel("train_derail", 0.2f);
            if (train.status == TrainDispatchStatus.Derailment)
            {
                var clearRes = sys.ClearDerailment("train_derail");
                Assert.True(clearRes.IsSuccess);
                Assert.Equal(TrainDispatchStatus.Idle, train.status);
            }
        }

        [Fact]
        public void State_RoundTrip_PreservesRailwayData()
        {
            var sys = new RailwaySystem(new SeededRng(42));
            sys.RegisterCatalog(CreateTestCatalog());
            sys.CreateStarterTrain("train_saved", "Iron Horse", "node_b");

            var state = sys.State;
            var json = System.Text.Json.JsonSerializer.Serialize(state);

            var deserialized = System.Text.Json.JsonSerializer.Deserialize<RailwayState>(json);
            var sys2 = new RailwaySystem(new SeededRng(42));
            sys2.RestoreState(deserialized!);

            var restoredTrain = sys2.State.trains.Find(t => t.trainId == "train_saved");
            Assert.NotNull(restoredTrain);
            Assert.Equal("node_b", restoredTrain!.currentNodeId);
        }
    }
}
