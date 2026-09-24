#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Settlements;
using Xunit;

namespace Ashfall.Core.Tests.Settlements
{
    public sealed class Plan58OutpostSettlementIntegrationTests
    {
        private static string ResolveCatalogPath(string filename)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", filename);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", filename));
            }
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", filename);
            }
            return path;
        }

        [Fact]
        public void OutpostsCatalog_LoadsAndParsesCanonicalOutposts()
        {
            string path = ResolveCatalogPath("outposts.json");
            Assert.True(File.Exists(path), $"Catalog not found at {path}");

            string json = File.ReadAllText(path);
            var system = OutpostSettlementSystem.FromJson(json);

            var definitions = system.GetAllDefinitions();
            Assert.Equal(4, definitions.Count);

            var northWatch = system.GetDefinition("outpost_north_watch");
            Assert.NotNull(northWatch);
            Assert.Equal("node_north_ridge", northWatch!.GraphNodeId);
            Assert.Equal(4, northWatch.MaxGarrisonBunks);
            Assert.Equal(80, northWatch.DefenseRating);
            Assert.True(northWatch.BuildCost.ContainsKey("scrap_metal"));
        }

        [Fact]
        public void EstablishOutpost_ConsumesResources_AndFiresSeam()
        {
            string path = ResolveCatalogPath("outposts.json");
            var system = OutpostSettlementSystem.FromJson(File.ReadAllText(path));

            string? establishedId = null;
            string? establishedNode = null;
            system.OnOutpostEstablishedSeam = (id, node) =>
            {
                establishedId = id;
                establishedNode = node;
            };

            var inventory = new Dictionary<string, int>
            {
                { "scrap_metal", 100 },
                { "scrap_wood", 50 },
                { "dried_rations", 50 }
            };

            bool costConsumer(string item, int qty)
            {
                if (inventory.TryGetValue(item, out int current) && current >= qty)
                {
                    inventory[item] -= qty;
                    return true;
                }
                return false;
            }

            bool success = system.EstablishOutpost("outpost_north_watch", costConsumer);
            Assert.True(success);
            Assert.Equal("outpost_north_watch", establishedId);
            Assert.Equal("node_north_ridge", establishedNode);
            Assert.Equal(50, inventory["scrap_metal"]); // 100 - 50

            var inst = system.GetInstance("outpost_north_watch");
            Assert.NotNull(inst);
            Assert.True(inst!.IsEstablished);
            Assert.Equal(1000, inst.ConditionPermille);

            // Re-establishing already established outpost fails
            Assert.False(system.EstablishOutpost("outpost_north_watch", costConsumer));
        }

        [Fact]
        public void AssignGarrison_EnforcesBunkCapacity_AndFitnessGating()
        {
            string path = ResolveCatalogPath("outposts.json");
            var system = OutpostSettlementSystem.FromJson(File.ReadAllText(path));
            system.EstablishOutpost("outpost_north_watch");

            string? assignedOutpost = null;
            string? assignedSurvivor = null;
            system.OnGarrisonAssignedSeam = (outpostId, survivorId) =>
            {
                assignedOutpost = outpostId;
                assignedSurvivor = survivorId;
            };

            // Fitness check: "unfit_survivor" is rejected
            bool fitnessCheck(string id) => id != "unfit_survivor";

            Assert.False(system.AssignGarrison("outpost_north_watch", "unfit_survivor", fitnessCheck));
            Assert.True(system.AssignGarrison("outpost_north_watch", "survivor_1", fitnessCheck));
            Assert.Equal("outpost_north_watch", assignedOutpost);
            Assert.Equal("survivor_1", assignedSurvivor);

            // Add up to capacity (max 4)
            Assert.True(system.AssignGarrison("outpost_north_watch", "survivor_2", fitnessCheck));
            Assert.True(system.AssignGarrison("outpost_north_watch", "survivor_3", fitnessCheck));
            Assert.True(system.AssignGarrison("outpost_north_watch", "survivor_4", fitnessCheck));

            // Exceeding capacity fails
            Assert.False(system.AssignGarrison("outpost_north_watch", "survivor_5", fitnessCheck));

            // Relieve survivor
            string? relievedSurvivor = null;
            system.OnGarrisonRelievedSeam = (outpostId, survivorId) => relievedSurvivor = survivorId;
            Assert.True(system.RelieveGarrison("outpost_north_watch", "survivor_1"));
            Assert.Equal("survivor_1", relievedSurvivor);

            // Now survivor_5 can be assigned
            Assert.True(system.AssignGarrison("outpost_north_watch", "survivor_5", fitnessCheck));
        }

        [Fact]
        public void SupplyAndTickDay_HandlesStarvationAndRecovery()
        {
            string path = ResolveCatalogPath("outposts.json");
            var system = OutpostSettlementSystem.FromJson(File.ReadAllText(path));
            system.EstablishOutpost("outpost_relay_tower"); // max 3 bunks
            system.AssignGarrison("outpost_relay_tower", "survivor_a");
            system.AssignGarrison("outpost_relay_tower", "survivor_b");

            string? starvingOutpost = null;
            system.OnOutpostStarvingSeam = id => starvingOutpost = id;

            // Day 1 with 0 rations -> starves
            system.TickDay();
            var inst = system.GetInstance("outpost_relay_tower");
            Assert.NotNull(inst);
            Assert.True(inst!.IsStarving);
            Assert.Equal(1, inst.DaysSinceSupply);
            Assert.Equal(950, inst.ConditionPermille);
            Assert.Equal("outpost_relay_tower", starvingOutpost);

            // Resupply with 10 rations
            int suppliedRations = 0;
            system.OnOutpostSuppliedSeam = (id, r) => suppliedRations = r;
            Assert.True(system.SupplyOutpost("outpost_relay_tower", 10));
            Assert.Equal(10, suppliedRations);
            Assert.False(inst.IsStarving);

            // Day 2 -> consumes 2 rations from reserve (demand is 2 for 2 survivors)
            system.TickDay();
            Assert.False(inst.IsStarving);
            Assert.Equal(8, inst.RationReserve);
            Assert.Equal(0, inst.DaysSinceSupply);
            Assert.Equal(955, inst.ConditionPermille); // slight recovery
        }

        [Fact]
        public void SimulateRisk_DeterministicallyResolvesOverrun()
        {
            string path = ResolveCatalogPath("outposts.json");
            var system = OutpostSettlementSystem.FromJson(File.ReadAllText(path));
            system.EstablishOutpost("outpost_quarry_camp"); // defense 50

            string? overrunOutpost = null;
            system.OnOutpostOverrunSeam = id => overrunOutpost = id;

            // Low danger does not overrun
            var rng = new SeededRng(42);
            bool overrunLow = system.SimulateRisk("outpost_quarry_camp", 30, rng);
            Assert.False(overrunLow);

            // Extremely high danger with low roll triggers overrun
            var hostileRng = new SeededRng(1);
            bool overrunHigh = system.SimulateRisk("outpost_quarry_camp", 300, hostileRng);
            Assert.True(overrunHigh);
            Assert.Equal("outpost_quarry_camp", overrunOutpost);

            var inst = system.GetInstance("outpost_quarry_camp");
            Assert.NotNull(inst);
            Assert.True(inst!.IsOverrun);

            // Overrun outpost cannot be garrisoned
            Assert.False(system.AssignGarrison("outpost_quarry_camp", "survivor_x"));

            // Abandon clears state
            Assert.True(system.AbandonOutpost("outpost_quarry_camp"));
            Assert.False(inst.IsEstablished);
        }
    }
}
