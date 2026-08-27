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

        [Fact] public void StartMaintenance_LaterPartMissing_DoesNotConsumeEarlierPart()
        {
            // CR3-03 regression: StartMaintenance previously consumed the
            // first required part before checking whether later parts were
            // available. The fix pre-checks ALL required parts first;
            // inventory is mutated only when every required part resolves.
            var e = Create(out var inv, out _);
            e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            inv.AddById("part_cleaner", 5);
            // part_grease intentionally absent.
            int cleanerBefore = inv.CountById("part_cleaner");
            var r = e.StartMaintenance("tool_1", "station_1", MaintenanceType.Clean,
                new System.Collections.Generic.List<string> { "part_cleaner", "part_grease" });
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("missing_part", r.FailureCode);
            // Atomicity: cleaner inventory must be unchanged.
            Assert.Equal(cleanerBefore, inv.CountById("part_cleaner"));
            // No job was created.
            Assert.Empty(e.State.pendingJobs);
        }

        [Fact]
        public void StartMaintenance_DuplicatePartsRequired_AggregatesAndRefusesWhenShort()
        {
            var e = Create(out var inv, out _);
            e.RegisterItem("tool_1", "wrench", "survivor_1", EquipmentFamily.Tool);
            // Player only has 1 part_cleaner
            inv.AddById("part_cleaner", 1);

            // Job requires 2 part_cleaner (passed as list with duplicate IDs)
            var r = e.StartMaintenance("tool_1", "station_1", MaintenanceType.Repair,
                new System.Collections.Generic.List<string> { "part_cleaner", "part_cleaner" });

            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("missing_part", r.FailureCode);
            // 0 consumed
            Assert.Equal(1, inv.CountById("part_cleaner"));
            Assert.Empty(e.State.pendingJobs);
        }

        private static EquipmentConditionSystem Create(out Inventory.Inventory inv, out CraftingSystem crafting)
        {
            inv = new Inventory.Inventory();
            crafting = new CraftingSystem(inv);
            return new EquipmentConditionSystem(new SeededRng(42), inv, crafting);
        }
    }
}
