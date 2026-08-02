using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Eat", menuName = "ASHFALL/AI/Eat Action")]
    public class EatActionSO : SurvivorAction
    {
        public string FoodItemId = "canned_food";

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
            float hunger = context.Survivor.Needs.Hunger;

            // If inventory check is required, check if food exists
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                bool hasFood = false;
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && (slot.Item.id == FoodItemId || slot.Item.hungerRestored > 0f))
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
            if (context?.Survivor != null)
            {
                context.Survivor.Needs.Hunger = Mathf.Max(0f, context.Survivor.Needs.Hunger - 40f);
            }
        }
    }
}
