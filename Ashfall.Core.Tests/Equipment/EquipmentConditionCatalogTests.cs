// SPDX-License-Identifier: MIT
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Equipment
{
    public sealed class EquipmentConditionCatalogTests
    {
        private static EquipmentConditionSystem CreateSystem()
        {
            var inv = new Inventory.Inventory();
            var crafting = new CraftingSystem(inv);
            return new EquipmentConditionSystem(new SeededRng(13), inv, crafting);
        }

        [Fact]
        public void BuiltInProfiles_ExistBeforeCatalogLoad()
        {
            var sys = CreateSystem();
            Assert.True(sys.TryGetProfile("degrade_profile_tool", out var tool));
            Assert.Equal("Tool", tool.item_family);
            Assert.Equal(0.8f, tool.base_wear_per_use, 3);
        }

        [Fact]
        public void LoadProfiles_AuthoredIdOverridesMatchingBuiltIn()
        {
            var sys = CreateSystem();
            sys.LoadProfiles("""
                {
                  "schema_version": 1,
                  "profiles": [
                    {
                      "profile_id": "degrade_profile_tool",
                      "item_family": "Tool",
                      "base_wear_per_use": 3.5,
                      "break_threshold": 9.0
                    }
                  ]
                }
                """);

            Assert.True(sys.TryGetProfile("degrade_profile_tool", out var tool));
            Assert.Equal(3.5f, tool.base_wear_per_use, 3);
            Assert.Equal(9.0f, tool.break_threshold, 3);
            Assert.True(sys.TryGetProfile("Tool", out var byFamily));
            Assert.Equal(3.5f, byFamily.base_wear_per_use, 3);
        }

        [Fact]
        public void LoadProfiles_ShippedCatalog_OverridesFirearmHeatSensitivity()
        {
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dataDir))
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");

            var sys = CreateSystem();
            Assert.True(sys.TryGetProfile("degrade_profile_firearm", out var before));
            Assert.Equal(1f, before.heat_sensitivity, 3);

            string json = File.ReadAllText(Path.Combine(dataDir, "item_degradation.json"));
            sys.LoadProfiles(json);

            Assert.True(sys.TryGetProfile("degrade_profile_firearm", out var after));
            Assert.Equal(1.4f, after.heat_sensitivity, 3);
            Assert.True(sys.TryGetProfile("degrade_profile_melee", out var melee));
            Assert.Equal("Weapon", melee.item_family);
        }

        [Fact]
        public void ApplyWear_UsesAuthoredFamilyWear()
        {
            var sys = CreateSystem();
            sys.LoadProfiles("""
                {
                  "schema_version": 1,
                  "profiles": [
                    {
                      "profile_id": "degrade_profile_tool",
                      "item_family": "Tool",
                      "base_wear_per_use": 5.0
                    }
                  ]
                }
                """);
            Assert.True(sys.RegisterItem("inst_1", "item_greenhouse_trowel", "sv_1", EquipmentFamily.Tool).IsSuccess);
            var result = sys.ApplyWear("inst_1", new WearEvent { intensity = 1f, environmentModifier = 1f });
            Assert.True(result.IsSuccess);
            var item = sys.State.items.Find(i => i.instanceId == "inst_1");
            Assert.NotNull(item);
            Assert.Equal(95f, item!.condition, 3);
        }
    }
}
