using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that drives a survivor to cook a meal at the stove
    /// (Prompt #189 — CookingSystem). Scores when the shared food stock is
    /// low AND the shelter has both an operational stove and clean water.
    /// The actual cook (recipe cost, perk rolls) is delegated to
    /// CookingSystem via AIContext.OnRequestCookMeal, since the AI assembly
    /// cannot reference Core types.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCookMealAction", menuName = "ASHFALL/AI Actions/Cook Meal")]
    public class CookActionSO : SurvivorAction
    {
        private const string StoveModuleId = "stove";
        private const float CleanWaterPerMeal = 1f; // mirrors CookingSystem.CleanWaterPerMeal
        private const int LowFoodThreshold = 3;

        public CookActionSO()
        {
            id = "action_cook_meal";
            displayName = "Cook a Meal";
            description = "Cook a meal at the stove to replenish the shared food stock.";
            basePriority = 0.3f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;
            int foodUnits = CountFoodUnits(context);
            if (foodUnits >= LowFoodThreshold) return 0f;
            // Scarcer stock -> higher urgency; empty stock scores highest.
            return Mathf.Clamp01(1f - (float)foodUnits / LowFoodThreshold) * 0.6f;
        }

        private static bool MeetsPrerequisites(AIContext context)
        {
            if (!HasLivingSurvivor(context) || context.Shelter == null) return false;
            var stove = context.Shelter.GetModule(StoveModuleId);
            bool stoveReady = stove != null && stove.IsOperational && stove.Fuel > 0f;
            bool waterReady = context.WaterStorage != null && context.WaterStorage.CleanWater >= CleanWaterPerMeal;
            return stoveReady && waterReady;
        }

        private static int CountFoodUnits(AIContext context)
        {
            if (context.Inventory?.Slots == null) return 0;
            int total = 0;
            for (int i = 0; i < context.Inventory.Slots.Count; i++)
            {
                var slot = context.Inventory.Slots[i];
                if (slot?.Item != null && slot.Item.type == ItemType.Food)
                    total += slot.Amount;
            }
            return total;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            context.OnRequestCookMeal?.Invoke(context.Survivor);
        }
    }
}
