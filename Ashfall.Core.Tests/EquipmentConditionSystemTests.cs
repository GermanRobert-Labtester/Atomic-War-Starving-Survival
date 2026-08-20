using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class EquipmentConditionSystemTests
    {
        [Fact] public void RegisterItem_CreatesInstance()
        {
            var e = Create(out _, out _);
            var r = e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(e.State.items);
        }

        [Fact] public void UseItem_DecreasesCondition()
        {
            var e = Create(out _, out _);
            e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            e.UseItem("tool_1", 10f);
            Assert.Equal(90f, e.State.items[0].condition);
        }

        [Fact] public void UseItem_Unusable_WhenZero()
        {
            var e = Create(out _, out _);
            e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            e.UseItem("tool_1", 100f);
            Assert.False(e.IsUsable("tool_1"));
        }

        [Fact] public void StartMaintenance_ReservesParts()
        {
            var e = Create(out var inv, out _);
            e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            inv.AddById("spare_part", 2);
            var r = e.StartMaintenance("tool_1", "bench_1", MaintenanceType.Sharpen, new System.Collections.Generic.List<string> { "spare_part" });
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(e.State.pendingJobs);
        }

        [Fact] public void TickDay_CompletesMaintenance()
        {
            var e = Create(out var inv, out _);
            e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            e.UseItem("tool_1", 50f);
            inv.AddById("spare_part", 1);
            e.StartMaintenance("tool_1", "bench_1", MaintenanceType.Repair, new System.Collections.Generic.List<string> { "spare_part" });
            e.TickDay(1);
            Assert.True(e.State.pendingJobs[0].isComplete);
            Assert.True(e.State.items[0].condition > 50f);
        }

        [Fact] public void GetSlipRisk_HighWhenDamaged()
        {
            var e = Create(out _, out _);
            e.RegisterItem("tool_1", "scalpel", "survivor_1", EquipmentFamily.Medical);
            e.UseItem("tool_1", 80f);
            Assert.True(e.GetSlipRisk("tool_1") > 0);
        }

        [Fact] public void CaptureRestoreState_PreservesItems()
        {
            var e = Create(out _, out _);
            e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            e.UseItem("tool_1", 30f);
            var state = e.CaptureState();
            Assert.Single(state.items);

            var e2 = Create(out _, out _);
            e2.RestoreState(state);
            Assert.Single(e2.State.items);
            Assert.Equal(70f, e2.State.items[0].condition);
        }

        private static EquipmentConditionSystem Create(out Inventory.Inventory inv, out CraftingSystem crafting)
        {
            inv = new Inventory.Inventory();
            crafting = new CraftingSystem(inv);
            return new EquipmentConditionSystem(new SeededRng(42), inv, crafting);
        }
    }
}
