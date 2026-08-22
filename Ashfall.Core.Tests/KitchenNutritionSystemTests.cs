using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class KitchenNutritionSystemTests
    {
        [Fact] public void SetCellar_SetsTemp()
        {
            var k = Create(out _, out _);
            var r = k.SetCellar(true, 8f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(k.State.hasCellar);
            Assert.Equal(8f, k.State.cellarTempC);
        }

        [Fact] public void StartPrepJob_WithoutIngredients_Blocks()
        {
            var k = Create(out var inv, out _);
            var r = k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void StartPrepJob_WithIngredients_StartsJob()
        {
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            inv.AddById("veg", 3);
            var r = k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 }, { "veg", 1 } });
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(k.State.activeJobs);
        }

        [Fact] public void TickDay_CompletesJob()
        {
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1);
            // CR3-05 contract: a completed job is now removed from activeJobs
            // at end-of-tick to prevent unbounded list growth. Assert completion
            // observable-side via pantry and totalMealsPrepared instead of via
            // activeJobs[0] (which no longer exists post-eviction).
            Assert.Empty(k.State.activeJobs);
            Assert.Equal(3, k.State.totalMealsPrepared);
            Assert.Single(k.State.pantry);
            Assert.Equal("stew", k.State.pantry[0].itemId);
            Assert.Equal(3, k.State.pantry[0].portionCount);
        }

        [Fact] public void TickDay_JobCompletes_RemovesJobFromActiveList()
        {
            // CR3-05 regression: previously, completed (or cancelled) jobs accumulated
            // forever in State.activeJobs even though GetActiveJobs filtered them
            // out of the API surface. The list still serialised to every save,
            // bloating memory in long campaigns. The fix evicts terminally-finished
            // jobs from the underlying list at the end of TickDay.
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1);
            // Job is now complete — the underlying list must be empty post-cleanup.
            Assert.Empty(k.State.activeJobs);
        }

        [Fact] public void TickDay_CancelledJob_IsRemovedFromActiveList()
        {
            // CR3-05 regression (variant): cancellation also clears the slot.
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.CancelJob(k.State.activeJobs[0].jobId);
            k.TickDay(1); // cleanup window
            Assert.Empty(k.State.activeJobs);
        }

        [Fact] public void CancelJob_RefundsIngredients()
        {
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            int before = inv.CountById("meat");
            k.CancelJob(k.State.activeJobs[0].jobId);
            Assert.Equal(before + 2, inv.CountById("meat"));
        }

        [Fact] public void StartPrepJob_LaterIngredientInsufficient_DoesNotConsumeEarlierIngredient()
        {
            // CR3-02 regression: StartPrepJob previously consumed the first
            // ingredient before checking whether later ingredients were sufficient.
            // A job requiring {meat:2, veg:1} with meat present but veg absent
            // would silently drain 2 of meat on failed start. The fix pre-checks
            // ALL required counts before consuming any.
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            // veg is intentionally absent.
            int meatBefore = inv.CountById("meat");
            var r = k.StartPrepJob("stew", "cook_1",
                new Dictionary<string, int> { { "meat", 2 }, { "veg", 1 } });
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("insufficient_ingredients", r.FailureCode);
            // Atomicity: meat inventory must be unchanged.
            Assert.Equal(meatBefore, inv.CountById("meat"));
            // No job was created.
            Assert.Empty(k.State.activeJobs);
        }

        [Fact] public void ServeMeal_ConsumesPortion()
        {
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            k.TickDay(1);
            var r = k.ServeMeal("survivor_1", "stew");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(2, k.State.pantry[0].portionCount);
        }

        [Fact] public void ServeMeal_NoPortions_Blocks()
        {
            var k = Create(out _, out _);
            var r = k.ServeMeal("survivor_1", "stew");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void CaptureRestoreState_PreservesJobs()
        {
            var k = Create(out var inv, out _);
            inv.AddById("meat", 5);
            k.StartPrepJob("stew", "cook_1", new Dictionary<string, int> { { "meat", 2 } });
            var state = k.CaptureState();
            Assert.Single(state.activeJobs);

            var k2 = Create(out _, out _);
            k2.RestoreState(state);
            Assert.Single(k2.State.activeJobs);
        }

        private static KitchenNutritionSystem Create(out Inventory.Inventory inv, out NeedsSystem needs)
        {
            inv = new Inventory.Inventory();
            needs = new NeedsSystem();
            return new KitchenNutritionSystem(new SeededRng(42), inv, needs);
        }
    }
}
