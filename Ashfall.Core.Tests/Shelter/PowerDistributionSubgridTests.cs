// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class PowerDistributionSubgridTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            candidate = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void CatalogLoader_LoadsAllTwelveNodes()
        {
            string dataDir = GetDataDir();
            var nodes = PowerSubgridCatalogLoader.Load(dataDir);
            Assert.NotNull(nodes);
            Assert.Equal(12, nodes.Count);

            foreach (var n in nodes)
            {
                Assert.False(string.IsNullOrEmpty(n.node_id));
                Assert.False(string.IsNullOrEmpty(n.target_room_id));
                Assert.True(n.max_capacity_watts > 0);
                Assert.True(n.surge_limit_watts >= n.max_capacity_watts);
                Assert.True(n.fuse_rating_amps > 0);
            }
        }

        [Fact]
        public void LoadDistribution_AppliesRoomLoad_AndTracksDeliveredPower()
        {
            var def = new PowerSubgridNodeDefinition
            {
                node_id = "node_clinic",
                target_room_id = "room_clinic",
                max_capacity_watts = 3000,
                surge_limit_watts = 4000
            };

            var inv = new Inventory.Inventory();
            var system = new PowerDistributionSubgridSystem(new[] { def }, inv, new SeededRng(1));

            Assert.True(system.IsRoomPowered("room_clinic"));

            system.ApplyRoomLoad("room_clinic", 1500f);
            var node = system.FindNodeForRoom("room_clinic");
            Assert.NotNull(node);
            Assert.Equal(1500f, node.current_load_watts);
            Assert.False(node.is_fuse_blown);
        }

        [Fact]
        public void ThermalModel_OverloadExceedingNinetyPercentGeneratesHeatAndDegradesOil()
        {
            var def = new PowerSubgridNodeDefinition
            {
                node_id = "node_foundry",
                target_room_id = "room_foundry",
                max_capacity_watts = 2000,
                surge_limit_watts = 4000,
                cooling_efficiency = 0.5f
            };

            var inv = new Inventory.Inventory();
            var system = new PowerDistributionSubgridSystem(new[] { def }, inv, new SeededRng(2));

            var node = system.FindNode("node_foundry");
            Assert.NotNull(node);
            float initialOil = node.transformer_oil_condition;

            // Apply 98% load (1960W > 1800W 90% threshold)
            for (int i = 0; i < 5; i++)
            {
                system.ApplyRoomLoad("room_foundry", 1960f);
            }

            Assert.True(node.temperature_celsius > PowerDistributionSubgridSystem.AmbientTemperatureCelsius);
            Assert.True(node.transformer_oil_condition <= initialOil);
        }

        [Fact]
        public void CapacitorBank_BuffersSurgeBelowLimit()
        {
            var def = new PowerSubgridNodeDefinition
            {
                node_id = "node_workshop",
                target_room_id = "room_workshop",
                max_capacity_watts = 2000,
                surge_limit_watts = 3000
            };

            var inv = new Inventory.Inventory();
            var system = new PowerDistributionSubgridSystem(new[] { def }, inv, new SeededRng(3));

            float initCapacitor = system.CapacitorBankChargeWatts;

            // Apply 2500W surge (exceeds 2000W max but within 3000W surge limit)
            system.ApplyRoomLoad("room_workshop", 2500f);

            var node = system.FindNode("node_workshop");
            Assert.NotNull(node);
            Assert.False(node.is_fuse_blown);
            Assert.True(system.CapacitorBankChargeWatts < initCapacitor);
        }

        [Fact]
        public void SurgeExceedingLimit_BlowsFuse_AndDisablesPower()
        {
            var def = new PowerSubgridNodeDefinition
            {
                node_id = "node_workshop",
                target_room_id = "room_workshop",
                max_capacity_watts = 2000,
                surge_limit_watts = 2500
            };

            var inv = new Inventory.Inventory();
            var system = new PowerDistributionSubgridSystem(new[] { def }, inv, new SeededRng(4));

            bool fuseBlownEvent = false;
            system.OnNodeFuseBlown += _ => fuseBlownEvent = true;

            // Apply 3000W (exceeds 2500W surge limit)
            system.ApplyRoomLoad("room_workshop", 3000f);

            var node = system.FindNode("node_workshop");
            Assert.NotNull(node);
            Assert.True(node.is_fuse_blown);
            Assert.True(fuseBlownEvent);
            Assert.False(system.IsRoomPowered("room_workshop"));
        }

        [Fact]
        public void FuseReplacement_RequiresCopperFuseAndSkill()
        {
            var def = new PowerSubgridNodeDefinition
            {
                node_id = "node_water_pump",
                target_room_id = "room_water_pump",
                max_capacity_watts = 2000,
                surge_limit_watts = 2200
            };

            var inv = new Inventory.Inventory();
            var system = new PowerDistributionSubgridSystem(new[] { def }, inv, new SeededRng(5));

            system.ApplyRoomLoad("room_water_pump", 3000f);
            Assert.False(system.IsRoomPowered("room_water_pump"));

            // Blocked without skill
            var resSkill = system.ReplaceFuse("node_water_pump", hasRepairSkill: false);
            Assert.False(resSkill.IsSuccess);
            Assert.Equal("lacks_repair_skill", resSkill.FailureCode);

            // Blocked without fuse item
            var resItem = system.ReplaceFuse("node_water_pump", hasRepairSkill: true);
            Assert.False(resItem.IsSuccess);
            Assert.Equal("missing_fuse", resItem.FailureCode);

            // Add copper fuse
            inv.TryProduce("copper_fuse", 1);
            var resOk = system.ReplaceFuse("node_water_pump", hasRepairSkill: true);
            Assert.True(resOk.IsSuccess);
            Assert.True(system.IsRoomPowered("room_water_pump"));
            Assert.Equal(0, inv.CountById("copper_fuse"));
        }

        [Fact]
        public void TransformerMaintenance_RestoresOilConditionAndTemperature()
        {
            var def = new PowerSubgridNodeDefinition
            {
                node_id = "node_filtration",
                target_room_id = "room_filtration",
                max_capacity_watts = 2000,
                surge_limit_watts = 3000
            };

            var inv = new Inventory.Inventory();
            var system = new PowerDistributionSubgridSystem(new[] { def }, inv, new SeededRng(6));

            var node = system.FindNode("node_filtration");
            Assert.NotNull(node);
            node.transformer_oil_condition = 30.0f;
            node.temperature_celsius = 75.0f;

            // Missing items
            var resBlocked = system.PerformTransformerMaintenance("node_filtration");
            Assert.False(resBlocked.IsSuccess);

            inv.TryProduce("machine_oil", 1);
            inv.TryProduce("electrical_wire", 2);

            var resOk = system.PerformTransformerMaintenance("node_filtration");
            Assert.True(resOk.IsSuccess);
            Assert.Equal(100.0f, node.transformer_oil_condition);
            Assert.Equal(PowerDistributionSubgridSystem.AmbientTemperatureCelsius, node.temperature_celsius);
            Assert.Equal(0, inv.CountById("machine_oil"));
            Assert.Equal(0, inv.CountById("electrical_wire"));
        }

        [Fact]
        public void Persistence_SubgridStateSurvivesSaveLoad()
        {
            var def = new PowerSubgridNodeDefinition
            {
                node_id = "node_save_test",
                target_room_id = "room_clinic",
                max_capacity_watts = 2000,
                surge_limit_watts = 3000
            };

            var inv = new Inventory.Inventory();
            var systemA = new PowerDistributionSubgridSystem(new[] { def }, inv, new SeededRng(7));

            var nodeA = systemA.FindNode("node_save_test");
            Assert.NotNull(nodeA);
            nodeA.is_breaker_closed = false;
            nodeA.temperature_celsius = 45.0f;
            nodeA.transformer_oil_condition = 65.0f;

            var save = systemA.CaptureState();
            Assert.NotNull(save);

            var systemB = new PowerDistributionSubgridSystem(new[] { def }, new Inventory.Inventory(), new SeededRng(8));
            systemB.RestoreState(save);

            var nodeB = systemB.FindNode("node_save_test");
            Assert.NotNull(nodeB);
            Assert.False(nodeB.is_breaker_closed);
            Assert.Equal(45.0f, nodeB.temperature_celsius);
            Assert.Equal(65.0f, nodeB.transformer_oil_condition);
        }
    }
}
