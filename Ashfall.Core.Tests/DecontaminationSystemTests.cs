using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DecontaminationSystemTests
    {
        [Fact] public void Enqueue_CreatesCase()
        {
            var d = Create(out _, out _, out _, out _);
            var r = d.Enqueue("survivor_1", "gear_1", 0.8f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(d.State.queue);
        }

        [Fact] public void Enqueue_Duplicate_Blocks()
        {
            var d = Create(out _, out _, out _, out _);
            d.Enqueue("survivor_1", "gear_1", 0.8f);
            var r = d.Enqueue("survivor_1", "gear_1", 0.8f);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void ProcessQueue_NoWater_Blocks()
        {
            var d = Create(out var inv, out _, out _, out _);
            d.Enqueue("survivor_1", "gear_1", 0.5f);
            // Ensure no water
            while (inv.RemoveById("water_clean", 1)) { }
            var r = d.ProcessQueue();
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void ProcessQueue_WithResources_StartsProcessing()
        {
            var d = Create(out var inv, out _, out _, out _);
            inv.AddById("water_clean", 5);
            inv.AddById("soap", 5);
            d.Enqueue("survivor_1", "gear_1", 0.5f);
            var r = d.ProcessQueue();
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(d.HasActiveCase);
        }

        [Fact] public void CompleteCycle_ReducesContamination()
        {
            var d = Create(out var inv, out _, out _, out _);
            inv.AddById("water_clean", 5);
            inv.AddById("soap", 5);
            d.Enqueue("survivor_1", "gear_1", 0.9f);
            d.ProcessQueue();
            var before = d.State.activeCase;
            var r = d.CompleteCycle(safeRelease: true);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(DeconStatus.Complete, before.status);
            Assert.True(before.surfaceContamination < 0.9f);
        }

        [Fact] public void CompleteCycle_Bypass_IncreasesShelterContamination()
        {
            // Bug-11 update: in the pre-fix symmetric-transfer design a
            // bypassed case *did* increase shelter contamination (+0.1f
            // while surface dropped -0.1f). The fix changed
            // BypassShelterDelta to 0f so a bypass no longer pours surface
            // dust into shelter air. `shelterContaminated` still flips true
            // (the event is real - the case bypassed a designed airlock
            // step), but the level itself stays put.
            var d = Create(out var inv, out _, out _, out _);
            inv.AddById("water_clean", 5);
            inv.AddById("soap", 5);
            d.Enqueue("survivor_1", "gear_1", 0.5f);
            d.ProcessQueue();
            d.CompleteCycle(safeRelease: false);
            Assert.True(d.State.shelterContaminated);
            // Level did NOT move above the baseline because
            // BypassShelterDelta == 0f (no transfer).
            Assert.True(d.State.shelterContaminationLevel == 0f,
                "bypass should not add shelter contamination (post-fix)");
        }

        [Fact] public void CompleteCycle_WithoutActive_Blocks()
        {
            var d = Create(out _, out _, out _, out _);
            var r = d.CompleteCycle(true);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void Enqueue_SurvivorAlreadyOnQueue_Blocks()
        {
            // CR3-06 regression: caseId is day-scoped (`decon_{day}_{survivorId}`),
            // so the caseId predicate alone lets a survivor re-enqueue every new
            // day forever. The fix adds a survivor+status lock that matches
            // MentalHealthCrisisSystem's pattern. Enqueue once on day 1, enqueue
            // the same survivor on day 2 → must Block, not Success.
            var d = Create(out _, out _, out _, out _);
            d.TickDay(1);
            var first = d.Enqueue("survivor_1", "gear_a", 0.5f);
            Assert.Equal(ActionResult.StatusKind.Success, first.Status);
            d.TickDay(2);
            var second = d.Enqueue("survivor_1", "gear_a", 0.5f);
            Assert.Equal(ActionResult.StatusKind.Blocked, second.Status);
            Assert.Equal("survivor_busy", second.FailureCode);
            Assert.Single(d.State.queue);
        }

        [Fact] public void TickDay_ReducesShelterContamination()
        {
            var d = Create(out _, out _, out _, out _);
            d.State.shelterContaminated = true;
            d.State.shelterContaminationLevel = 0.5f;
            for (int i = 0; i < 100; i++) d.TickDay(i + 1);
            Assert.True(d.State.shelterContaminationLevel < 0.5f);
        }

        [Fact] public void CaptureRestoreState_PreservesQueue()
        {
            var d = Create(out _, out _, out _, out _);
            d.Enqueue("survivor_1", "gear_1", 0.5f);
            var state = d.CaptureState();
            Assert.Single(state.queue);

            var d2 = Create(out _, out _, out _, out _);
            d2.RestoreState(state);
            Assert.Single(d2.State.queue);
        }

        // Bug-11 regression: bypassed decon must not transfer to shelter air
        // contamination. Change: BypassShelterDelta = 0f (was +0.1f — the
        // audit's symmetric +0.1/−0.1 transfer).
        [Fact] public void Bug11_Bypass_NetShelterContamination_IsNotIncreased()
        {
            var d = Create(out var inv, out _, out _, out _);
            inv.AddById("water_clean", 5);
            inv.AddById("soap", 5);
            d.State.shelterContaminationLevel = 0.4f;
            d.Enqueue("survivor_bypass", "gear_bypass", 0.9f);
            d.ProcessQueue();
            float before = d.State.shelterContaminationLevel;
            var r = d.CompleteCycle(false);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            float after = d.State.shelterContaminationLevel;
            Assert.True(after <= before,
                $"bypass increased shelter contam: {before:F2} → {after:F2}");
        }

        private static DecontaminationSystem Create(out Inventory.Inventory inv, out RadiationSystem rad, out AirlockSecuritySystem airlock, out StartingLevelSystem sl)
        {
            inv = new Inventory.Inventory();
            rad = new RadiationSystem(seed: 42);
            airlock = new AirlockSecuritySystem(new SeededRng(42));
            sl = new StartingLevelSystem();
            return new DecontaminationSystem(new SeededRng(42), rad, inv, airlock, sl);
        }
    }
}
