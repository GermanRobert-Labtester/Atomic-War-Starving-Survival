// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class RailLogisticsPlan73Tests
    {
        private static string GetAuthoritativeCatalogPath()
        {
            return Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "rail_logistics_catalog.json");
        }

        private static RailwayNetworkCatalog CreateTestNetworkCatalog()
        {
            return new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "node_terminal", display_name = "Central Terminal" },
                    new RailNodeDef { node_id = "node_quarry", display_name = "Limestone Quarry" },
                    new RailNodeDef { node_id = "node_pass", display_name = "High Ridge Pass" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "seg_term_quarry",
                        display_name = "Flat Quarry Line",
                        start_node_id = "node_terminal",
                        end_node_id = "node_quarry",
                        distance_km = 10f,
                        base_integrity = 0.8f,
                        max_train_mass = 200f
                    },
                    new TrackSegmentDef
                    {
                        segment_id = "seg_term_pass",
                        display_name = "Steep Mountain Pass",
                        start_node_id = "node_terminal",
                        end_node_id = "node_pass",
                        distance_km = 10f,
                        base_integrity = 0.8f,
                        max_train_mass = 200f
                    }
                },
                cars = new List<TrainCarDef>
                {
                    new TrainCarDef { car_type_id = "car_locomotive_diesel", empty_mass = 50f }
                }
            };
        }

        [Fact]
        public void AuthoritativeCatalog_LoadsAndValidates()
        {
            string path = GetAuthoritativeCatalogPath();
            if (!File.Exists(path))
            {
                // Fallback for different working dirs
                path = Path.Combine("Assets", "StreamingAssets", "Data", "rail_logistics_catalog.json");
            }
            Assert.True(File.Exists(path), $"Authoritative catalog must exist at {path}");

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var edges = RailLogisticsCatalogLoader.Load(Path.GetDirectoryName(path)!, io, json);

            Assert.NotNull(edges);
            Assert.True(edges.Count >= 5, "Catalog must define at least 5 rail logistics edges");

            foreach (var edge in edges)
            {
                Assert.False(string.IsNullOrWhiteSpace(edge.rail_edge_id), "Edge ID cannot be empty");
                Assert.StartsWith("rail_edge_", edge.rail_edge_id);
                Assert.False(string.IsNullOrWhiteSpace(edge.from_node), "from_node cannot be empty");
                Assert.False(string.IsNullOrWhiteSpace(edge.to_node), "to_node cannot be empty");
                Assert.True(edge.derailment_risk_bp >= 0, "derailment_risk_bp cannot be negative");
            }
        }

        [Fact]
        public void RegisterLogisticsCatalog_EnablesBidirectionalLookup()
        {
            var sys = new RailwaySystem(new SeededRng(7301));
            var edges = new List<RailLogisticsEdgeDef>
            {
                new RailLogisticsEdgeDef
                {
                    rail_edge_id = "rail_edge_alpha_beta",
                    from_node = "alpha",
                    to_node = "beta",
                    grade = 0.025f,
                    derailment_risk_bp = 150
                }
            };

            sys.RegisterLogisticsCatalog(edges);

            var forward = sys.GetLogisticsEdge("alpha", "beta");
            var reverse = sys.GetLogisticsEdge("beta", "alpha");

            Assert.NotNull(forward);
            Assert.NotNull(reverse);
            Assert.Same(forward, reverse);
            Assert.Equal(0.025f, forward!.grade);
            Assert.Equal(150, forward.derailment_risk_bp);
        }

        [Fact]
        public void GradeSpeedModifier_AttenuatesTravelProgressOnSteepIncline()
        {
            var sysFlat = new RailwaySystem(new SeededRng(7302));
            sysFlat.RegisterCatalog(CreateTestNetworkCatalog());
            var flatEdges = new List<RailLogisticsEdgeDef>
            {
                new RailLogisticsEdgeDef { rail_edge_id = "edge_flat", from_node = "node_terminal", to_node = "node_quarry", grade = 0.0f }
            };
            sysFlat.RegisterLogisticsCatalog(flatEdges);

            var sysSteep = new RailwaySystem(new SeededRng(7302));
            sysSteep.RegisterCatalog(CreateTestNetworkCatalog());
            var steepEdges = new List<RailLogisticsEdgeDef>
            {
                new RailLogisticsEdgeDef { rail_edge_id = "edge_steep", from_node = "node_terminal", to_node = "node_pass", grade = 0.05f }
            };
            sysSteep.RegisterLogisticsCatalog(steepEdges);

            var trainFlat = sysFlat.CreateStarterTrain("train_flat", "Flat Runner", "node_terminal");
            var trainSteep = sysSteep.CreateStarterTrain("train_steep", "Steep Climber", "node_terminal");

            sysFlat.DispatchTrain("train_flat", "seg_term_quarry");
            sysSteep.DispatchTrain("train_steep", "seg_term_pass");

            sysFlat.TickTravel("train_flat", 0.4f);
            sysSteep.TickTravel("train_steep", 0.4f);

            // Grade of 0.05 yields modifier: 1.0 - (0.05 * 5) = 0.75
            // Expected progress: flat = 0.4, steep = 0.4 * 0.75 = 0.3
            Assert.True(trainFlat.segmentProgress > trainSteep.segmentProgress,
                $"Flat progress ({trainFlat.segmentProgress}) must exceed steep progress ({trainSteep.segmentProgress})");
            Assert.Equal(0.4f, trainFlat.segmentProgress, 3);
            Assert.Equal(0.3f, trainSteep.segmentProgress, 3);
        }

        [Fact]
        public void DerailmentRisk_IncreasesWithHigherRiskEdgeOnDegradedTrack()
        {
            // Seed chosen such that NextDouble falls between low-risk and high-risk derail chance
            // Low risk bp=50 -> riskFactor=0.20 -> chance = (0.65 - 0.45) * 0.20 = 0.04
            // High risk bp=800 -> riskFactor=0.80 -> chance = (0.65 - 0.45) * 0.80 = 0.16
            int derailCountLow = 0;
            int derailCountHigh = 0;

            for (int seed = 100; seed < 200; seed++)
            {
                // Low risk system
                var sysLow = new RailwaySystem(new SeededRng(seed));
                sysLow.RegisterCatalog(CreateTestNetworkCatalog());
                sysLow.RegisterLogisticsCatalog(new[]
                {
                    new RailLogisticsEdgeDef { rail_edge_id = "edge_low", from_node = "node_terminal", to_node = "node_quarry", derailment_risk_bp = 50 }
                });
                var segLow = sysLow.EnsureSegmentState("seg_term_quarry");
                segLow.integrity = 0.45f;
                var tLow = sysLow.CreateStarterTrain("t_low", "Low", "node_terminal");
                sysLow.DispatchTrain("t_low", "seg_term_quarry");
                sysLow.TickTravel("t_low", 0.1f);
                if (tLow.status == TrainDispatchStatus.Derailment) derailCountLow++;

                // High risk system
                var sysHigh = new RailwaySystem(new SeededRng(seed));
                sysHigh.RegisterCatalog(CreateTestNetworkCatalog());
                sysHigh.RegisterLogisticsCatalog(new[]
                {
                    new RailLogisticsEdgeDef { rail_edge_id = "edge_high", from_node = "node_terminal", to_node = "node_quarry", derailment_risk_bp = 800 }
                });
                var segHigh = sysHigh.EnsureSegmentState("seg_term_quarry");
                segHigh.integrity = 0.45f;
                var tHigh = sysHigh.CreateStarterTrain("t_high", "High", "node_terminal");
                sysHigh.DispatchTrain("t_high", "seg_term_quarry");
                sysHigh.TickTravel("t_high", 0.1f);
                if (tHigh.status == TrainDispatchStatus.Derailment) derailCountHigh++;
            }

            Assert.True(derailCountHigh > derailCountLow,
                $"High-risk edge should cause more derailments ({derailCountHigh}) than low-risk ({derailCountLow})");
        }

        [Fact]
        public void ClearTrackObstacle_ConsumesScrapMetal_AndClearsSabotage()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("scrap_metal", 10);

            var sys = new RailwaySystem(new SeededRng(7303), inv);
            sys.RegisterCatalog(CreateTestNetworkCatalog());

            var seg = sys.EnsureSegmentState("seg_term_quarry");
            seg.isSabotaged = true;

            var res = sys.ClearTrackObstacle("seg_term_quarry");
            Assert.True(res.IsSuccess, "Clearing obstacle should succeed with scrap metal");
            Assert.False(seg.isSabotaged, "Sabotage flag should be cleared");
            Assert.Equal(5, inv.CountById("scrap_metal")); // 10 - 5 = 5

            // Second clearing should be blocked because not sabotaged
            var res2 = sys.ClearTrackObstacle("seg_term_quarry");
            Assert.False(res2.IsSuccess);
            Assert.Equal("no_obstacle", res2.FailureCode);
        }

        [Fact]
        public void ClearTrackObstacle_BlockedWhenInsufficientMaterials()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("scrap_metal", 2); // needs 5

            var sys = new RailwaySystem(new SeededRng(7304), inv);
            sys.RegisterCatalog(CreateTestNetworkCatalog());

            var seg = sys.EnsureSegmentState("seg_term_quarry");
            seg.isSabotaged = true;

            var res = sys.ClearTrackObstacle("seg_term_quarry");
            Assert.False(res.IsSuccess);
            Assert.Equal("insufficient_clearing_materials", res.FailureCode);
            Assert.True(seg.isSabotaged);
            Assert.Equal(2, inv.CountById("scrap_metal"));
        }

        [Fact]
        public void State_CapturesAndRestoresCorrectly()
        {
            var sys = new RailwaySystem(new SeededRng(7305));
            sys.RegisterCatalog(CreateTestNetworkCatalog());

            var seg = sys.EnsureSegmentState("seg_term_quarry");
            seg.integrity = 0.62f;
            seg.bridgeIntact = false;
            seg.isSabotaged = true;

            var train = sys.CreateStarterTrain("train_saved", "Test Train", "node_terminal");
            train.currentFuel = 180f;

            var state = sys.State;
            Assert.NotNull(state);
            Assert.Single(state.trains);
            Assert.Equal(0.62f, state.segments["seg_term_quarry"].integrity);

            // Restore in new system
            var restoredSys = new RailwaySystem(new SeededRng(7306));
            restoredSys.RegisterCatalog(CreateTestNetworkCatalog());
            restoredSys.RestoreState(state);

            Assert.Equal(180f, restoredSys.GetTrain("train_saved")?.currentFuel);
            var restoredSeg = restoredSys.EnsureSegmentState("seg_term_quarry");
            Assert.Equal(0.62f, restoredSeg.integrity);
            Assert.False(restoredSeg.bridgeIntact);
            Assert.True(restoredSeg.isSabotaged);
        }
    }
}
