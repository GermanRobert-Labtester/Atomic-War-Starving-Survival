using System;
using Xunit;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for CraftingSystem atomicity, queue progression, deterministic completion,
    /// and RespiratoryDegenerationSystem treatment and save round-trip.
    /// </summary>
    public class CraftingAfflictionLoopTests
    {
        // ═══════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════

        private static ItemDefinition MakeItem(string id, ItemType type = ItemType.Material, int stackMax = 50)
            => new ItemDefinition { id = id, displayName = id, type = type, stackMax = stackMax, weight = 0.1f };

        private static Recipe MakeRecipe(string id, ItemDefinition result, int resultAmt,
            float hours, params (ItemDefinition item, int amt)[] ingredients)
        {
            var r = new Recipe
            {
                id = id,
                recipeName = id,
                result = result,
                resultAmount = resultAmt,
                craftingTimeHours = hours,
                requiredStationId = string.Empty
            };
            foreach (var (item, amt) in ingredients)
                r.ingredients.Add(new Ingredient { item = item, amount = amt });
            return r;
        }

        // ═══════════════════════════════════════════════════════════
        // CraftingSystem — Atomicity
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void StartCraft_ConsumesIngredientsAtomically()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 1f, (cloth, 2));

            var inv = new InventoryContainer();
            inv.Add(cloth, 4);

            var sys = new CraftingSystem(inv);
            sys.SetRecipeLookup(id => recipe.id == id ? recipe : null);

            bool ok = sys.StartCraft(recipe);

            Assert.True(ok);
            Assert.Equal(1, sys.ActiveCraftCount);
            Assert.Equal(2, inv.Count(cloth)); // 4 − 2 consumed
        }

        [Fact]
        public void StartCraft_WhenInsufficientIngredients_ConsumesNothing()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 1f, (cloth, 5));

            var inv = new InventoryContainer();
            inv.Add(cloth, 2); // need 5, only have 2

            var sys = new CraftingSystem(inv);

            bool ok = sys.StartCraft(recipe);

            Assert.False(ok);
            Assert.Equal(0, sys.ActiveCraftCount);
            Assert.Equal(2, inv.Count(cloth)); // unchanged
        }

        [Fact]
        public void StartCraft_WhenNullRecipe_ReturnsFalse()
        {
            var inv = new InventoryContainer();
            var sys = new CraftingSystem(inv);

            bool ok = sys.StartCraft(null);

            Assert.False(ok);
            Assert.Equal(0, sys.ActiveCraftCount);
        }

        // ═══════════════════════════════════════════════════════════
        // CraftingSystem — Queue progression & completion
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void Tick_CompletesQueuedCraft_ExactlyOnce()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 2f, (cloth, 1));

            var inv = new InventoryContainer();
            inv.Add(cloth, 3);

            var sys = new CraftingSystem(inv);
            sys.SetRecipeLookup(id => recipe.id == id ? recipe : null);

            int completionCount = 0;
            sys.OnCraftCompleted += _ => completionCount++;

            sys.StartCraft(recipe);
            Assert.Equal(1, sys.ActiveCraftCount);

            sys.Tick(1.5f); // not yet complete
            Assert.Equal(1, sys.ActiveCraftCount);
            Assert.Equal(0, completionCount);

            sys.Tick(1f); // completes
            Assert.Equal(0, sys.ActiveCraftCount);
            Assert.Equal(1, completionCount); // exactly once
            Assert.Equal(1, inv.Count(bandage));
        }

        [Fact]
        public void Tick_DoesNotCompleteBeforeDuration()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 4f, (cloth, 1));

            var inv = new InventoryContainer();
            inv.Add(cloth, 1);

            var sys = new CraftingSystem(inv);
            sys.StartCraft(recipe);
            sys.Tick(3.99f);

            Assert.Equal(1, sys.ActiveCraftCount);
            Assert.Equal(0, inv.Count(bandage));
        }

        [Fact]
        public void MultipleRecipes_QueueBothAndCompleteInOrder()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical, stackMax: 20);
            var r1 = MakeRecipe("recipe_a", bandage, 1, 2f, (cloth, 1));
            var r2 = MakeRecipe("recipe_b", bandage, 1, 3f, (cloth, 1));

            var inv = new InventoryContainer();
            inv.Add(cloth, 4);

            var sys = new CraftingSystem(inv);
            sys.StartCraft(r1);
            sys.StartCraft(r2);
            Assert.Equal(2, sys.ActiveCraftCount);

            sys.Tick(2f); // r1 done
            Assert.Equal(1, sys.ActiveCraftCount);

            sys.Tick(3f); // r2 done
            Assert.Equal(0, sys.ActiveCraftCount);
            Assert.Equal(2, inv.Count(bandage));
        }

        // ═══════════════════════════════════════════════════════════
        // CraftingSystem — Save / Restore round-trip
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void RestoreState_ReconstitutesQueueWithHoursRemaining()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 6f, (cloth, 1));

            var inv1 = new InventoryContainer();
            inv1.Add(cloth, 1);

            var sys1 = new CraftingSystem(inv1);
            sys1.SetRecipeLookup(id => recipe.id == id ? recipe : null);
            sys1.StartCraft(recipe);
            sys1.Tick(2f); // 4h remaining
            var save = sys1.CaptureState();

            var inv2 = new InventoryContainer();
            var sys2 = new CraftingSystem(inv2);
            sys2.SetRecipeLookup(id => recipe.id == id ? recipe : null);
            sys2.RestoreState(save);

            Assert.Equal(1, sys2.ActiveCraftCount);
            Assert.True(Math.Abs(sys2.ActiveCrafts[0].HoursRemaining - 4f) < 0.01f);
        }

        [Fact]
        public void RestoreState_ThenTickToCompletion_DoesNotDuplicate()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 4f, (cloth, 1));

            var inv1 = new InventoryContainer();
            inv1.Add(cloth, 1);

            var sys1 = new CraftingSystem(inv1);
            sys1.SetRecipeLookup(id => recipe.id == id ? recipe : null);
            sys1.StartCraft(recipe);
            sys1.Tick(2f);
            var save = sys1.CaptureState();

            var inv2 = new InventoryContainer();
            var sys2 = new CraftingSystem(inv2);
            sys2.SetRecipeLookup(id => recipe.id == id ? recipe : null);
            sys2.RestoreState(save);

            int completions = 0;
            sys2.OnCraftCompleted += _ => completions++;
            sys2.Tick(3f); // 2h remaining → completes

            Assert.Equal(0, sys2.ActiveCraftCount);
            Assert.Equal(1, completions); // not 2
            Assert.Equal(1, inv2.Count(bandage));
        }

        [Fact]
        public void RestoreState_WithNullSave_ClearsQueue()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 2f, (cloth, 1));

            var inv = new InventoryContainer();
            inv.Add(cloth, 1);

            var sys = new CraftingSystem(inv);
            sys.SetRecipeLookup(id => recipe.id == id ? recipe : null);
            sys.StartCraft(recipe);
            Assert.Equal(1, sys.ActiveCraftCount);

            sys.RestoreState(null);
            Assert.Equal(0, sys.ActiveCraftCount);
        }

        // ═══════════════════════════════════════════════════════════
        // CraftingSystem — Station blocking
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void CraftingSystem_Station_BlocksCraftWhenInoperational()
        {
            var cloth = MakeItem("cloth");
            var bandage = MakeItem("bandage_out", ItemType.Medical);
            var recipe = MakeRecipe("recipe_bandage", bandage, 1, 1f, (cloth, 1));
            recipe.requiredStationId = "workbench";

            var inv = new InventoryContainer();
            inv.Add(cloth, 5);

            var sys = new CraftingSystem(inv);
            // No station registered → CanCraft should fail
            Assert.False(sys.CanCraft(recipe));

            // Register an operational station
            sys.AddStation(new CraftingStation { id = "workbench", condition = 100f });
            Assert.True(sys.CanCraft(recipe));

            // Degrade station to zero
            sys.GetStation("workbench").Degrade(100f);
            Assert.False(sys.CanCraft(recipe));
        }

        // ═══════════════════════════════════════════════════════════
        // RespiratoryDegenerationSystem — Treatment
        // ═══════════════════════════════════════════════════════════

        private static RespiratoryDegenerationSystem MakeRespiratoryInAshZone()
            => new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 100f,
                IsInFalloutStorm = () => false,
                IsInAshZone = () => true
            };

        [Fact]
        public void ApplyInhaler_ReducesDegradationAndGivesRelief()
        {
            var sys = MakeRespiratoryInAshZone();
            sys.TickHours("sv1", 24f);
            float degBefore = sys.RespiratoryDegradation("sv1");
            Assert.True(degBefore > 0f, "ash zone should accumulate degradation");

            bool result = sys.ApplyInhaler("sv1");

            Assert.True(result);
            Assert.True(sys.RespiratoryDegradation("sv1") < degBefore);
            Assert.Equal(RespiratoryDegenerationSystem.InhalerReliefDurationHours,
                sys.InhalerReliefHours("sv1"));
        }

        [Fact]
        public void ApplyInhaler_OnHealthySurvivor_ReturnsFalse()
        {
            var sys = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 100f,
                IsInFalloutStorm = () => false,
                IsInAshZone = () => false
            };
            sys.GetOrCreate("sv1"); // ensure record exists but degradation=0

            bool result = sys.ApplyInhaler("sv1");

            Assert.False(result);
            Assert.Equal(0f, sys.InhalerReliefHours("sv1"));
        }

        [Fact]
        public void ApplyHerbalTea_ReducesMildDegradation()
        {
            var sys = MakeRespiratoryInAshZone();
            sys.TickHours("sv1", 10f);
            float degBefore = sys.RespiratoryDegradation("sv1");

            bool result = sys.ApplyHerbalTea("sv1");

            Assert.True(result);
            Assert.True(sys.RespiratoryDegradation("sv1") < degBefore);
        }

        [Fact]
        public void ApplyHerbalTea_OnHealthySurvivor_ReturnsFalse()
        {
            var sys = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 100f,
                IsInFalloutStorm = () => false,
                IsInAshZone = () => false
            };
            sys.GetOrCreate("sv1");

            bool result = sys.ApplyHerbalTea("sv1");

            Assert.False(result);
        }

        [Fact]
        public void SevereCoughThreshold_ReducesStaminaMultiplier()
        {
            var sys = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 100f,
                IsInFalloutStorm = () => false,
                IsInAshZone = () => false
            };
            var state = sys.GetOrCreate("sv1");

            // Below threshold — no penalty
            state.respiratoryDegradation = RespiratoryDegenerationSystem.SevereCoughThreshold - 1f;
            Assert.Equal(1f, sys.GetStaminaMultiplier("sv1"));

            // At threshold — penalty applies
            state.respiratoryDegradation = RespiratoryDegenerationSystem.SevereCoughThreshold;
            float mult = sys.GetStaminaMultiplier("sv1");
            Assert.True(mult < 1f);
            Assert.True(Math.Abs(mult - (1f - RespiratoryDegenerationSystem.SevereCoughStaminaPenalty)) < 0.001f);
        }

        [Fact]
        public void InhalerRelief_SuppressesStaminaPenalty()
        {
            var sys = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 100f,
                IsInFalloutStorm = () => false,
                IsInAshZone = () => true
            };
            // Push to severe cough range
            var state = sys.GetOrCreate("sv1");
            state.respiratoryDegradation = RespiratoryDegenerationSystem.SevereCoughThreshold + 5f;
            state.inhalerReliefHours = 0f;
            float mult_noRelief = sys.GetStaminaMultiplier("sv1");

            state.inhalerReliefHours = 4f;
            float mult_withRelief = sys.GetStaminaMultiplier("sv1");

            Assert.True(mult_noRelief < 1f, "should have stamina penalty without relief");
            Assert.Equal(1f, mult_withRelief); // inhaler suppresses symptom
        }

        // ═══════════════════════════════════════════════════════════
        // RespiratoryDegenerationSystem — Save round-trip
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void CaptureRestoreRoundTrip_PreservesAllRespiratoryFields()
        {
            var sys1 = MakeRespiratoryInAshZone();
            sys1.TickHours("sv1", 200f);
            sys1.ApplyInhaler("sv1");

            float origDeg    = sys1.RespiratoryDegradation("sv1");
            float origRelief = sys1.InhalerReliefHours("sv1");
            bool  origPerm   = sys1.HasPermanentLungDamage("sv1");

            var save = sys1.CaptureState();

            var sys2 = new RespiratoryDegenerationSystem();
            sys2.RestoreState(save);

            Assert.True(Math.Abs(sys2.RespiratoryDegradation("sv1") - origDeg) < 0.001f);
            Assert.True(Math.Abs(sys2.InhalerReliefHours("sv1") - origRelief) < 0.001f);
            Assert.Equal(origPerm, sys2.HasPermanentLungDamage("sv1"));
        }

        [Fact]
        public void RestoreRespiratoryState_WithNullSave_ClearsSurvivors()
        {
            var sys = MakeRespiratoryInAshZone();
            sys.TickHours("sv1", 48f);
            Assert.True(sys.RespiratoryDegradation("sv1") > 0f);

            sys.RestoreState(null);

            Assert.Equal(0f, sys.RespiratoryDegradation("sv1"));
            Assert.Empty(sys.Survivors);
        }

        [Fact]
        public void OnStateChanged_FiresAfterInhaler()
        {
            var sys = MakeRespiratoryInAshZone();
            sys.TickHours("sv1", 24f);
            int changeCount = 0;
            sys.OnStateChanged += () => changeCount++;

            sys.ApplyInhaler("sv1");

            Assert.True(changeCount > 0);
        }

        [Fact]
        public void GetStaminaMultiplier_RestoredSurvivor_MatchesOriginal()
        {
            var sys1 = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 100f, IsInFalloutStorm = () => false, IsInAshZone = () => false
            };
            var state = sys1.GetOrCreate("sv1");
            state.respiratoryDegradation = RespiratoryDegenerationSystem.SevereCoughThreshold + 10f;

            float multBefore = sys1.GetStaminaMultiplier("sv1");
            var save = sys1.CaptureState();

            var sys2 = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 100f, IsInFalloutStorm = () => false, IsInAshZone = () => false
            };
            sys2.RestoreState(save);

            Assert.True(Math.Abs(sys2.GetStaminaMultiplier("sv1") - multBefore) < 0.001f);
        }
    }
}
