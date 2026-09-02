using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class EquipmentConditionDegradationTests
    {
        private static EquipmentConditionSystem CreateSystem(out Inventory.Inventory inv, out CraftingSystem crafting)
        {
            inv = new Inventory.Inventory();
            crafting = new CraftingSystem(inv);
            var rng = new SeededRng(1337);
            return new EquipmentConditionSystem(rng, inv, crafting);
        }

        [Fact]
        public void ApplyWear_DecreasesConditionDeterministically()
        {
            var sys = CreateSystem(out _, out _);
            sys.RegisterItem("gun_01", "weapon_service_rifle", "survivor_01", EquipmentFamily.Weapon, 100f);

            var wearEvt = new WearEvent { intensity = 2.0f, environmentModifier = 1.0f };
            var res = sys.ApplyWear("gun_01", wearEvt);

            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            var item = sys.State.items.Find(i => i.instanceId == "gun_01");
            Assert.NotNull(item);
            Assert.True(item.condition < 100f);
            Assert.Equal(100f, item.originalMaxCondition);
        }

        [Fact]
        public void UnusedItem_ConditionRemainsIntact()
        {
            var sys = CreateSystem(out _, out _);
            sys.RegisterItem("tool_01", "iron_pipe", "survivor_01", EquipmentFamily.Tool, 80f);

            var item = sys.State.items.Find(i => i.instanceId == "tool_01");
            Assert.NotNull(item);
            Assert.Equal(80f, item.condition);
            Assert.Equal(80f, item.maxCondition);
            Assert.False(item.isBroken);
        }

        [Fact]
        public void ApplyCorrosion_IncreasesRustAndDegradesCondition()
        {
            var sys = CreateSystem(out _, out _);
            sys.RegisterItem("knife_01", "iron_pipe", "survivor_01", EquipmentFamily.Weapon, 100f);

            sys.ApplyCorrosion("knife_01", 20f, "acid_rain");
            var item = sys.State.items.Find(i => i.instanceId == "knife_01");
            Assert.NotNull(item);
            Assert.True(item.rustLevel > 0f);
            Assert.True(item.condition < 100f);
        }

        [Fact]
        public void JuryRig_RestoresPartialCondition_WithPermanentMaxLoss()
        {
            var sys = CreateSystem(out var inv, out _);
            sys.RegisterItem("rifle_01", "weapon_service_rifle", "survivor_01", EquipmentFamily.Weapon, 100f);
            sys.UseItem("rifle_01", 70f); // condition down to 30

            inv.AddById("scrap_metal", 2);
            var res = sys.JuryRig("rifle_01", new List<string> { "scrap_metal" });

            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            var item = sys.State.items.Find(i => i.instanceId == "rifle_01");
            Assert.NotNull(item);
            Assert.True(item.temporaryPatch);
            Assert.True(item.condition > 30f);
            Assert.True(item.maxCondition < 100f); // max durability loss
            Assert.Equal(100f, item.originalMaxCondition);
        }

        [Fact]
        public void RepairItem_RestoresFullCondition_ClampedByDecayedMax()
        {
            var sys = CreateSystem(out var inv, out _);
            sys.RegisterItem("coat_01", "item_heavy_wool_coat", "survivor_01", EquipmentFamily.Clothing, 100f);
            sys.UseItem("coat_01", 60f);

            inv.AddById("scrap_wood", 2);
            var res = sys.RepairItem("coat_01", MaintenanceType.Repair, new List<string> { "scrap_wood" }, 1.0f);

            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            var item = sys.State.items.Find(i => i.instanceId == "coat_01");
            Assert.NotNull(item);
            Assert.Equal(item.maxCondition, item.condition);
            Assert.True(item.maxCondition <= item.originalMaxCondition);
            Assert.False(item.temporaryPatch);
        }

        [Fact]
        public void ClearJam_ResetsJammedState()
        {
            var sys = CreateSystem(out _, out _);
            sys.RegisterItem("smg_01", "weapon_smg", "survivor_01", EquipmentFamily.Weapon, 100f);
            var item = sys.State.items.Find(i => i.instanceId == "smg_01");
            Assert.NotNull(item);
            item.isJammed = true;

            var res = sys.ClearJam("smg_01");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.False(item.isJammed);
        }

        [Fact]
        public void SaveAndRestore_PreservesAllDegradationFields()
        {
            var sys = CreateSystem(out _, out _);
            sys.RegisterItem("axe_01", "iron_pipe", "survivor_01", EquipmentFamily.Tool, 100f);
            var item = sys.State.items.Find(i => i.instanceId == "axe_01");
            Assert.NotNull(item);
            item.rustLevel = 35f;
            item.maxCondition = 85f;
            item.condition = 60f;
            item.temporaryPatch = true;
            item.originalMaxCondition = 100f;

            var saved = sys.CaptureState();
            var restoredSys = CreateSystem(out _, out _);
            restoredSys.RestoreState(saved);

            var restoredItem = restoredSys.State.items.Find(i => i.instanceId == "axe_01");
            Assert.NotNull(restoredItem);
            Assert.Equal(35f, restoredItem.rustLevel);
            Assert.Equal(85f, restoredItem.maxCondition);
            Assert.Equal(60f, restoredItem.condition);
            Assert.True(restoredItem.temporaryPatch);
            Assert.Equal(100f, restoredItem.originalMaxCondition);
        }
    }
}
