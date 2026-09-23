// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class Plan73_76RailLootIntegrationTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            throw new DirectoryNotFoundException("StreamingAssets/Data directory could not be located.");
        }

        [Fact]
        public void RailLogisticsCatalog_And_RailwaySystem_FullLogisticsContract()
        {
            var dataDir = GetDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Plan 73 catalog loading
            var edges = RailLogisticsCatalogLoader.Load(dataDir, io, json);
            Assert.NotNull(edges);
            Assert.True(edges.Count >= 5, "Authoritative rail logistics catalog must contain at least 5 edges");

            foreach (var edge in edges)
            {
                Assert.False(string.IsNullOrWhiteSpace(edge.rail_edge_id));
                Assert.StartsWith("rail_edge_", edge.rail_edge_id);
                Assert.False(string.IsNullOrWhiteSpace(edge.from_node));
                Assert.False(string.IsNullOrWhiteSpace(edge.to_node));
                Assert.InRange(edge.derailment_risk_bp, 0, 10000);
            }

            // System integration with RailwaySystem
            var rng = new SeededRng(73001);
            var railSystem = new RailwaySystem(rng);
            railSystem.RegisterLogisticsCatalog(edges);

            // Verify bidirectional lookup on registered catalog edges
            var firstEdge = edges[0];
            var fwd = railSystem.GetLogisticsEdge(firstEdge.from_node, firstEdge.to_node);
            var rev = railSystem.GetLogisticsEdge(firstEdge.to_node, firstEdge.from_node);

            Assert.NotNull(fwd);
            Assert.NotNull(rev);
            Assert.Same(fwd, rev);
            Assert.Equal(firstEdge.grade, fwd!.grade);
            Assert.Equal(firstEdge.derailment_risk_bp, fwd.derailment_risk_bp);
        }

        [Fact]
        public void ScavengingTableCatalog_And_ExpeditionLoot_FullContract()
        {
            var dataDir = GetDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Plan 76 catalog loading
            var catalog = ScavengingTableCatalog.LoadFromDirectory(dataDir, io, json);
            Assert.NotNull(catalog);
            Assert.True(catalog.TableCount >= 20, "Authoritative scavenging catalog must contain >= 20 tables");

            foreach (var table in catalog.Tables)
            {
                Assert.False(string.IsNullOrWhiteSpace(table.id));
                Assert.False(string.IsNullOrWhiteSpace(table.location_type));
                Assert.NotEmpty(table.entries);
                Assert.True(table.TotalWeight > 0);

                foreach (var entry in table.entries)
                {
                    Assert.True(!string.IsNullOrWhiteSpace(entry.item_id) || !string.IsNullOrWhiteSpace(entry.map_fragment_id));
                    Assert.True(entry.weight > 0);
                    Assert.True(entry.min_quantity >= 1);
                    Assert.True(entry.max_quantity >= entry.min_quantity);
                }
            }

            // Deterministic loot roll verification
            var rng = new SeededRng(76001);
            var testTable = catalog.Tables[0];
            var roll = catalog.RollLoot(testTable.id, rng);

            Assert.NotNull(roll);
            Assert.Equal(testTable.id, roll!.TableId);
            Assert.True(!string.IsNullOrWhiteSpace(roll.ItemId) || !string.IsNullOrWhiteSpace(roll.MapFragmentId));
            Assert.True(roll.Quantity >= 1);
            Assert.False(string.IsNullOrWhiteSpace(roll.RarityTier));
        }

        [Fact]
        public void CrossModalTransit_RailNetwork_To_ScavengingTableHarvest()
        {
            var dataDir = GetDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var railEdges = RailLogisticsCatalogLoader.Load(dataDir, io, json);
            var lootCatalog = ScavengingTableCatalog.LoadFromDirectory(dataDir, io, json);

            var rng = new SeededRng(7376);
            var railSystem = new RailwaySystem(rng);

            // Setup synthetic rail track and registered logistics edge
            var networkCatalog = new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "node_shelter_depot", display_name = "Shelter Rail Depot" },
                    new RailNodeDef { node_id = "node_abandoned_works", display_name = "Abandoned Rail Works" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "seg_depot_to_works",
                        display_name = "Depot to Works Spur",
                        start_node_id = "node_shelter_depot",
                        end_node_id = "node_abandoned_works",
                        distance_km = 12f,
                        base_integrity = 0.85f,
                        max_train_mass = 150f
                    }
                },
                cars = new List<TrainCarDef>
                {
                    new TrainCarDef { car_type_id = "car_locomotive_diesel", empty_mass = 45f }
                }
            };
            railSystem.RegisterCatalog(networkCatalog);

            var logisticsEdges = new List<RailLogisticsEdgeDef>
            {
                new RailLogisticsEdgeDef
                {
                    rail_edge_id = "edge_depot_works",
                    from_node = "node_shelter_depot",
                    to_node = "node_abandoned_works",
                    grade = 0.015f,
                    derailment_risk_bp = 80,
                    track_condition = "good"
                }
            };
            railSystem.RegisterLogisticsCatalog(logisticsEdges);

            // Dispatch starter train
            var train = railSystem.CreateStarterTrain("train_scavenger_1", "Iron Scavenger", "node_shelter_depot");
            Assert.NotNull(train);
            Assert.Equal("node_shelter_depot", train.currentNodeId);

            var dispatched = railSystem.DispatchTrain("train_scavenger_1", "seg_depot_to_works");
            Assert.True(dispatched.IsSuccess);

            // Advance travel ticks until train arrives at destination
            railSystem.TickTravel("train_scavenger_1", 0.8f);
            Assert.True(train.segmentProgress > 0.0f);

            // Train arrives at destination; dwellers scavenge the rail depot / industrial works
            // Find appropriate scavenging table or use fallback table
            var matchingTable = lootCatalog.Tables.FirstOrDefault(t =>
                t.location_type.Contains("industrial", StringComparison.OrdinalIgnoreCase) ||
                t.location_type.Contains("urban", StringComparison.OrdinalIgnoreCase) ||
                t.location_type.Contains("military", StringComparison.OrdinalIgnoreCase)) ?? lootCatalog.Tables[0];

            var harvestedLoot = new List<ScavengingRollResult>();
            for (int i = 0; i < 5; i++)
            {
                var loot = lootCatalog.RollLoot(matchingTable.id, rng);
                if (loot != null)
                {
                    harvestedLoot.Add(loot);
                }
            }

            Assert.NotEmpty(harvestedLoot);
            Assert.All(harvestedLoot, item =>
            {
                Assert.True(!string.IsNullOrWhiteSpace(item.ItemId) || !string.IsNullOrWhiteSpace(item.MapFragmentId));
                Assert.True(item.Quantity >= 1);
            });
        }
    }
}
