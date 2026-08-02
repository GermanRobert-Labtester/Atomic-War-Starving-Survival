using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Drink", menuName = "ASHFALL/AI/Drink Action")]
    public class DrinkActionSO : SurvivorAction
    {
        public string WaterItemId = "clean_water";

        public DrinkActionSO()
        {
            id = "action_drink";
            displayName = "Drink";
            description = "Consume clean water to reduce thirst.";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            float thirst = context.Survivor.Needs.Thirst;

            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                bool hasWater = false;
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && (slot.Item.id == WaterItemId || slot.Item.thirstRestore > 0f))
                    {
                        hasWater = true;
                        break;
                    }
                }
                if (!hasWater) return 0f;
            }

            return Mathf.Clamp01(thirst / 100f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor != null)
            {
                context.Survivor.Needs.Thirst = Mathf.Max(0f, context.Survivor.Needs.Thirst - 50f);
            }
        }
    }
}
