using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Eat", menuName = "ASHFALL/AI/Eat Action")]
    public class EatActionSO : SurvivorAction
    {
        public string FoodItemId = "canned_food";

        /// <summary>Probability of botulism when eating ContaminatedFood (Internal Horror).</summary>
        public const float ContaminatedBotulismChance = 0.35f;

        public EatActionSO()
        {
            id = "action_eat";
            displayName = "Eat";
            description = "Consume food to reduce hunger.";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            // Comatose survivors cannot self-feed
            if (context.MedicalSystem != null && context.MedicalSystem.IsComatose(context.Survivor))
                return 0f;

            float hunger = context.Survivor.Needs.Hunger;

            // If inventory check is required, check if food exists
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                bool hasFood = false;
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null
                        && (slot.Item.id == FoodItemId
                            || slot.Item.type == ItemType.Food
                            || slot.Item.type == ItemType.ContaminatedFood
                            || slot.Item.hungerRestore > 0f))
                    {
                        hasFood = true;
                        break;
                    }
                }
                if (!hasFood) return 0f;
            }

            return Mathf.Clamp01(hunger / 100f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            var survivor = context.Survivor;
            if (context.MedicalSystem != null && context.MedicalSystem.IsComatose(survivor))
                return;

            ItemDefinition food = FindFood(context);
            if (food != null && context.Inventory != null)
            {
                if (!context.Inventory.Consume(food, survivor, context.RadiationSystem, null))
                    return;

                // Apply hunger via needs if available; Consume already applied hungerRestore
                // when NeedsSystem is passed — pass null above and apply here for parity.
                float restore = food.hungerRestore > 0f ? food.hungerRestore : 40f;
                survivor.Needs.Hunger = Mathf.Max(0f, survivor.Needs.Hunger - restore);
                if (food.moraleEffect != 0f)
                    survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + food.moraleEffect, 0f, 100f);
                if (food.healthEffect != 0f)
                    survivor.Needs.Health = Mathf.Clamp(survivor.Needs.Health + food.healthEffect, 0f, 100f);

                // ContaminatedFood / spoiled meat → Phase-1 gastric illness roll
                // Prompt #190 — Iron Stomach multiplies chance by 0.10
                if (context.MedicalSystem != null
                    && (food.type == ItemType.ContaminatedFood
                        || string.Equals(food.id, "spoiled_meat", System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(food.id, SurvivalPerkSystem.SpoiledMeatId,
                            System.StringComparison.OrdinalIgnoreCase)))
                {
                    float chance = ContaminatedBotulismChance;
                    if (context.SurvivalPerks != null)
                        chance = context.SurvivalPerks.ScaleIllnessChance(survivor, chance);
                    double roll = context.Random != null
                        ? context.Random.NextDouble()
                        : UnityEngine.Random.value;
                    if (roll < chance)
                        context.MedicalSystem.Inflict(survivor, AfflictionSO.Ids.Botulism);
                }
                return;
            }

            // Fallback when no inventory food (tests / empty stock): just reduce hunger
            survivor.Needs.Hunger = Mathf.Max(0f, survivor.Needs.Hunger - 40f);
        }

        private ItemDefinition FindFood(AIContext context)
        {
            if (context.Inventory?.Slots == null) return null;
            ItemDefinition preferred = null;
            ItemDefinition any = null;
            for (int i = 0; i < context.Inventory.Slots.Count; i++)
            {
                var slot = context.Inventory.Slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                var item = slot.Item;
                if (item.id == FoodItemId) preferred = item;
                if (item.type == ItemType.Food || item.type == ItemType.ContaminatedFood
                    || item.hungerRestore > 0f)
                {
                    // Prefer safe food over contaminated when both exist
                    if (item.type == ItemType.Food && any == null)
                        any = item;
                    else if (any == null)
                        any = item;
                }
            }
            return preferred ?? any;
        }
    }
}
