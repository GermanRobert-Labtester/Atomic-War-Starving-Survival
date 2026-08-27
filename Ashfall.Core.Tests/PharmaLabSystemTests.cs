using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class PharmaLabSystemTests
    {
        private static PharmaLabSystem Create(out Inventory.Inventory inv, out ISeededRng rng)
        {
            inv = new Inventory.Inventory();
            rng = new SeededRng(42);
            return new PharmaLabSystem(inv, rng);
        }

        private static PharmaRecipe MakeRecipe(string id = "test_recipe", string output = "medicine_basic",
            int outputAmount = 3, float risk = 0.1f)
        {
            return new PharmaRecipe
            {
                recipe_id = id, display_name = "Test Recipe",
                input_ids = new System.Collections.Generic.List<string> { "chemicals", "herbs" },
                input_amounts = new System.Collections.Generic.List<int> { 2, 1 },
                output_item_id = output, output_amount = outputAmount,
                base_hours = 2f, dependency_risk = risk
            };
        }

        [Fact] public void StartBatch_WithoutInputs_Blocks()
        {
            var pharma = Create(out _, out _);
            pharma.RegisterRecipe(MakeRecipe());
            var r = pharma.StartBatch("test_recipe", "chemist_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact]
        public void StartBatch_LateInputMissing_DoesNotConsumeEarlierInputs()
        {
            var pharma = Create(out var inv, out _);
            inv.AddById("chemicals", 5); // has chemicals, but 0 herbs
            pharma.RegisterRecipe(MakeRecipe()); // requires chemicals: 2, herbs: 1
            var r = pharma.StartBatch("test_recipe", "chemist_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.False(pharma.IsProcessing);
            // 0 chemicals consumed
            Assert.Equal(5, inv.CountById("chemicals"));
        }

        [Fact] public void StartBatch_WithInputs_StartsProcessing()
        {
            var pharma = Create(out var inv, out _);
            inv.AddById("chemicals", 5); inv.AddById("herbs", 3);
            pharma.RegisterRecipe(MakeRecipe());
            var r = pharma.StartBatch("test_recipe", "chemist_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(pharma.IsProcessing);
        }

        [Fact] public void CompleteBatch_DeliversOutput()
        {
            var pharma = Create(out var inv, out _);
            inv.AddById("chemicals", 5); inv.AddById("herbs", 3);
            pharma.RegisterRecipe(MakeRecipe(output: "medicine_basic", outputAmount: 3));
            pharma.StartBatch("test_recipe", "chemist_1");
            pharma.TickProgress(10f);
            Assert.Equal(3, inv.CountById("medicine_basic"));
            Assert.False(pharma.IsProcessing);
        }

        [Fact] public void CancelBatch_RefundsInputs()
        {
            var pharma = Create(out var inv, out _);
            inv.AddById("chemicals", 5); inv.AddById("herbs", 3);
            pharma.RegisterRecipe(MakeRecipe());
            pharma.StartBatch("test_recipe", "chemist_1");
            Assert.Equal(3, inv.CountById("chemicals")); // 5-2
            pharma.CancelBatch();
            Assert.Equal(5, inv.CountById("chemicals")); // refunded
        }

        [Fact] public void CaptureRestoreState_PreservesCompleted()
        {
            var pharma = Create(out var inv, out _);
            inv.AddById("chemicals", 5); inv.AddById("herbs", 3);
            pharma.RegisterRecipe(MakeRecipe());
            pharma.StartBatch("test_recipe", "chemist_1");
            pharma.TickProgress(10f);

            var state = pharma.CaptureState();
            Assert.Single(state.completedRecipeIds);

            var pharma2 = Create(out _, out _);
            pharma2.RegisterRecipe(MakeRecipe());
            pharma2.RestoreState(state);
            Assert.Single(pharma2.State.completedRecipeIds);
        }
    }
}
