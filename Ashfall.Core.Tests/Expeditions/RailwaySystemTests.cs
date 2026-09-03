// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;
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

    // ── Plan 73 §7.18: rail logistics extensions ──────────────────────

    public class RailLogisticsTests
    {
        private RailwayNetworkCatalog CreateHandcarCatalog()
        {
            return new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "term_a", display_name = "Terminal A", node_type = "Terminal" },
                    new RailNodeDef { node_id = "term_b", display_name = "Terminal B", node_type = "Terminal" },
                    new RailNodeDef { node_id = "junc_x", display_name = "Switchyard X", node_type = "Junction" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "seg_a_x",
                        start_node_id = "term_a",
                        end_node_id = "junc_x",
                        distance_km = 10f,
                        base_integrity = 0.9f,
                        bridge_required = false,
                        max_train_mass = 200f
                    },
                    new TrackSegmentDef
                    {
                        segment_id = "seg_x_b",
                        start_node_id = "junc_x",
                        end_node_id = "term_b",
                        distance_km = 15f,
                        base_integrity = 0.9f,
                        bridge_required = false,
                        max_train_mass = 200f
                    }
                },
                cars = new List<TrainCarDef>
                {
                    new TrainCarDef { car_type_id = "car_locomotive", empty_mass = 50f, vehicle_class = "locomotive" },
                    new TrainCarDef
                    {
                        car_type_id = "car_handcar",
                        empty_mass = 8f,
                        vehicle_class = "handcar",
                        crew_stamina_max = 1.0f,
                        stamina_drain_per_km = 0.04f,
                        stamina_recovery_per_stop = 0.4f
                    }
                }
            };
        }

        private RailwayNetworkCatalog CreateSwitchyardCatalog()
        {
            return new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "node_a", display_name = "Alpha", node_type = "Terminal" },
                    new RailNodeDef { node_id = "node_b", display_name = "Beta", node_type = "Terminal" },
                    new RailNodeDef { node_id = "node_c", display_name = "Gamma", node_type = "Junction" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "seg_a_c",
                        start_node_id = "node_a",
                        end_node_id = "node_c",
                        distance_km = 10f,
                        base_integrity = 0.9f,
                        bridge_required = false,
                        max_train_mass = 200f
                    },
                    new TrackSegmentDef
                    {
                        segment_id = "seg_c_b",
                        start_node_id = "node_c",
                        end_node_id = "node_b",
                        distance_km = 12f,
                        base_integrity = 0.9f,
                        bridge_required = false,
                        max_train_mass = 200f
                    }
                },
                cars = new List<TrainCarDef>
                {
                    new TrainCarDef { car_type_id = "car_loco", empty_mass = 50f, vehicle_class = "locomotive" }
                }
            };
        }

        [Fact]
        public void Handcar_StaminaDrainsPerKm()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateHandcarCatalog());
            var train = sys.CreateStarterTrain("handcar", "Pump Car", "term_a");
            train.cars[0] = new TrainCarInstance { instanceId = "hc1", carTypeId = "car_handcar" };
            train.crewStamina = 1.0f;
            train.maxCrewStamina = 1.0f;

            sys.DispatchTrain("handcar", "seg_a_x");
            sys.TickTravel("handcar", 0.5f);
            Assert.Equal(0.8f, train.crewStamina, 1);
        }

        [Fact]
        public void Handcar_Exhaustion_HaltsProgress()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateHandcarCatalog());
            var train = sys.CreateStarterTrain("handcar", "Pump Car", "term_a");
            train.cars[0] = new TrainCarInstance { instanceId = "hc1", carTypeId = "car_handcar" };
            train.crewStamina = 0.05f;
            train.maxCrewStamina = 1.0f;

            sys.DispatchTrain("handcar", "seg_a_x");
            sys.TickTravel("handcar", 1.0f);
            Assert.True(train.isCrewExhausted);
            Assert.True(train.segmentProgress < 1.0f);
        }

        [Fact]
        public void Handcar_StaminaRecoversAtTerminal()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateHandcarCatalog());
            var train = sys.CreateStarterTrain("handcar", "Pump Car", "term_a");
            train.cars[0] = new TrainCarInstance { instanceId = "hc1", carTypeId = "car_handcar" };
            train.crewStamina = 0.6f;
            train.maxCrewStamina = 1.0f;

            sys.DispatchTrain("handcar", "seg_a_x");
            sys.TickTravel("handcar", 1.0f);
            Assert.Equal(TrainDispatchStatus.Arrived, train.status);
            Assert.Equal(0.6f, train.crewStamina, 1);
            Assert.False(train.isCrewExhausted);
        }

        [Fact]
        public void Locomotive_IgnoresStamina()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateHandcarCatalog());
            var train = sys.CreateStarterTrain("loco", "Diesel", "term_a");
            train.crewStamina = 0.1f;

            sys.DispatchTrain("loco", "seg_a_x");
            sys.TickTravel("loco", 1.0f);
            Assert.Equal(TrainDispatchStatus.Arrived, train.status);
            Assert.False(train.isCrewExhausted);
            Assert.Equal(0.1f, train.crewStamina);
        }

        [Fact]
        public void ExpeditionMode_PlansMultiSegmentRoute()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var train = sys.CreateStarterTrain("exp", "Expedition Loco", "node_a");

            var est = sys.EstimateExpeditionTravel("node_a", "node_b");
            Assert.NotNull(est);
            Assert.Equal(2, est!.path.Count);
            Assert.Equal("seg_a_c", est.path[0]);
            Assert.Equal("seg_c_b", est.path[1]);
        }

        [Fact]
        public void ExpeditionMode_AutoAdvancesAtJunction()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var train = sys.CreateStarterTrain("exp", "Expedition Loco", "node_a");
            train.currentFuel = 500f;

            var dispatch = sys.DispatchExpedition("exp", "node_b");
            Assert.True(dispatch.IsSuccess);
            Assert.Equal(TrainDispatchStatus.EnRoute, train.status);
            Assert.Equal("seg_a_c", train.activeSegmentId);

            sys.TickTravel("exp", 1.0f);
            Assert.Equal("node_c", train.currentNodeId);
            Assert.Equal(TrainDispatchStatus.EnRoute, train.status);
            Assert.Equal("seg_c_b", train.activeSegmentId);
        }

        [Fact]
        public void Switchyard_RejectsDisconnectedSegment()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var train = sys.CreateStarterTrain("exp", "Expedition Loco", "node_a");

            var res = sys.PlanRoute("exp", new List<string> { "seg_c_b" });
            Assert.False(res.IsSuccess);
            Assert.Equal("segment_not_connected", res.FailureCode);
        }

        [Fact]
        public void Derailment_ReducesSegmentIntegrity()
        {
            var sys = new RailwaySystem(new SeededRng(1), new Inventory.Inventory());
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var seg = sys.EnsureSegmentState("seg_a_c");
            seg.integrity = 0.45f;

            var train = sys.CreateStarterTrain("derail", "Rusty", "node_a");
            train.status = TrainDispatchStatus.EnRoute;
            train.activeSegmentId = "seg_a_c";

            sys.TickTravel("derail", 0.2f);
            if (train.status == TrainDispatchStatus.Derailment)
                Assert.Equal(0.30f, seg.integrity, 1);
        }

        [Fact]
        public void Derailment_DamagesCarCondition()
        {
            var sys = new RailwaySystem(new SeededRng(1), new Inventory.Inventory());
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var train = sys.CreateStarterTrain("derail", "Rusty", "node_a");
            train.cars[1].condition = 100f;
            train.status = TrainDispatchStatus.EnRoute;
            train.activeSegmentId = "seg_a_c";

            sys.TickTravel("derail", 0.2f);
            if (train.status == TrainDispatchStatus.Derailment)
                Assert.Equal(70f, train.cars[1].condition, 0);
        }

        [Fact]
        public void Derailment_CanBeClearedAfterConsequences()
        {
            var sys = new RailwaySystem(new SeededRng(1), new Inventory.Inventory());
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var train = sys.CreateStarterTrain("derail", "Rusty", "node_a");
            train.status = TrainDispatchStatus.EnRoute;
            train.activeSegmentId = "seg_a_c";

            sys.TickTravel("derail", 0.2f);
            if (train.status == TrainDispatchStatus.Derailment)
            {
                var clear = sys.ClearDerailment("derail");
                Assert.True(clear.IsSuccess);
                Assert.Equal(TrainDispatchStatus.Idle, train.status);
                Assert.False(train.isCrewExhausted);
            }
        }

        [Fact]
        public void DispatchTrain_FuelConsumesByMassAndDistance()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("train_coal", 50);
            var sys = new RailwaySystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var train = sys.CreateStarterTrain("loco", "Heavy", "node_a");
            train.currentFuel = 0f;

            float before = inv.CountById("train_coal");
            var res = sys.DispatchTrain("loco", "seg_a_c");
            Assert.True(res.IsSuccess);
            Assert.True(inv.CountById("train_coal") < before);
        }

        [Fact]
        public void MassLimit_BlocksTraversal()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            var cat = CreateSwitchyardCatalog();
            cat.segments[0].max_train_mass = 50f;
            sys.RegisterCatalog(cat);
            var train = sys.CreateStarterTrain("heavy", "Big Train", "node_a");
            train.cars.Add(new TrainCarInstance { instanceId = "h1", carTypeId = "car_freight_hopper" });
            train.cars.Add(new TrainCarInstance { instanceId = "h2", carTypeId = "car_freight_hopper" });

            var res = sys.DispatchTrain("heavy", "seg_a_c");
            Assert.False(res.IsSuccess);
            Assert.Equal("cannot_traverse_segment", res.FailureCode);
        }

        [Fact]
        public void BridgeRequired_BlocksWithoutRepair()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            var cat = CreateSwitchyardCatalog();
            cat.segments[0].bridge_required = true;
            sys.RegisterCatalog(cat);
            var seg = sys.EnsureSegmentState("seg_a_c");
            seg.bridgeIntact = false;

            var train = sys.CreateStarterTrain("bridge", "Trestle", "node_a");
            var res = sys.DispatchTrain("bridge", "seg_a_c");
            Assert.False(res.IsSuccess);
            Assert.Equal("cannot_traverse_segment", res.FailureCode);
        }

        [Fact]
        public void Sabotage_BlocksTraversal()
        {
            var sys = new RailwaySystem(new SeededRng(42), new Inventory.Inventory());
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var seg = sys.EnsureSegmentState("seg_a_c");
            seg.isSabotaged = true;

            var train = sys.CreateStarterTrain("sabotaged", "Sabotaged", "node_a");
            var res = sys.DispatchTrain("sabotaged", "seg_a_c");
            Assert.False(res.IsSuccess);
            Assert.Equal("cannot_traverse_segment", res.FailureCode);
        }

        [Fact]
        public void RepairTrack_RestoresIntegrity()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("steel_rail_segment", 2);
            var sys = new RailwaySystem(new SeededRng(42), inv);
            sys.RegisterCatalog(CreateSwitchyardCatalog());
            var seg = sys.EnsureSegmentState("seg_a_c");
            seg.integrity = 0.5f;

            var res = sys.RepairTrack("seg_a_c", 0.3f);
            Assert.True(res.IsSuccess);
            Assert.Equal(0.8f, seg.integrity, 2);
            Assert.Equal(1, inv.CountById("steel_rail_segment"));
        }

        [Fact]
        public void SaveLoad_PreservesRailwayData()
        {
            var sys = new RailwaySystem(new SeededRng(42));
            sys.RegisterCatalog(CreateHandcarCatalog());
            var train = sys.CreateStarterTrain("handcar", "Pump", "term_a");
            train.cars[0] = new TrainCarInstance { instanceId = "hc1", carTypeId = "car_handcar" };
            train.crewStamina = 0.6f;
            train.isCrewExhausted = true;
            train.plannedPath.Add("seg_a_x");

            var state = sys.State;
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            var restored = System.Text.Json.JsonSerializer.Deserialize<RailwayState>(json);
            var sys2 = new RailwaySystem(new SeededRng(42));
            sys2.RestoreState(restored!);

            var t2 = sys2.State.trains.Find(t => t.trainId == "handcar");
            Assert.NotNull(t2);
            Assert.Equal(0.6f, t2!.crewStamina, 1);
            Assert.True(t2.isCrewExhausted);
            Assert.Single(t2.plannedPath);
        }

        [Fact]
        public void ContentUtilization_RailNetworkCatalog_IsMappedToRailwaySystem()
        {
            Assert.True(ContentUtilizationScanner.IsAuthoritativeCatalog("rail_network.json"));
        }

        [Fact]
        public void Derailment_IsDeterministicForSameSeed()
        {
            var sysA = new RailwaySystem(new SeededRng(1), new Inventory.Inventory());
            sysA.RegisterCatalog(CreateSwitchyardCatalog());
            var trainA = sysA.CreateStarterTrain("d1", "Rusty", "node_a");
            trainA.status = TrainDispatchStatus.EnRoute;
            trainA.activeSegmentId = "seg_a_c";
            sysA.TickTravel("d1", 0.2f);

            var sysB = new RailwaySystem(new SeededRng(1), new Inventory.Inventory());
            sysB.RegisterCatalog(CreateSwitchyardCatalog());
            var trainB = sysB.CreateStarterTrain("d1", "Rusty", "node_a");
            trainB.status = TrainDispatchStatus.EnRoute;
            trainB.activeSegmentId = "seg_a_c";
            sysB.TickTravel("d1", 0.2f);

            Assert.Equal(trainA.status, trainB.status);
        }
    }
}
