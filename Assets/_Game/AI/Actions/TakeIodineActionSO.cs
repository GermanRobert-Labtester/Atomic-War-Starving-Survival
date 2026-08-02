using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_TakeIodine", menuName = "ASHFALL/AI/Take Iodine Action")]
    public class TakeIodineActionSO : SurvivorAction
    {
        public string IodineItemId = "iodine_pills";

        public TakeIodineActionSO()
        {
            id = "action_take_iodine";
            displayName = "Take Iodine";
            description = "Administer iodine pills when ambient radiation is rising.";
            basePriority = 0.4f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context == null || !context.IsRadiationRising) return 0f;

            bool hasIodine = false;
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && slot.Item.id == IodineItemId && slot.Amount > 0)
                    {
                        hasIodine = true;
                        break;
                    }
                }
            }

            if (!hasIodine) return 0f;

            // Score spikes when radiation rising AND iodine in inventory
            return 0.92f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Inventory != null)
            {
                // Consume iodine pill
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && slot.Item.id == IodineItemId && slot.Amount > 0)
                    {
                        context.Inventory.Remove(slot.Item, 1);
                        break;
                    }
                }
            }
        }
    }
}
