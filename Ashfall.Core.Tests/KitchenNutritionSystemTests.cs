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
            Assert.True(k.State.activeJobs[0].isComplete);
            Assert.Equal(3, k.State.activeJobs[0].portionsProduced);
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
