using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_PurifyWater", menuName = "ASHFALL/AI/Purify Water Action")]
    public class PurifyWaterActionSO : SurvivorAction
    {
        public string IrradiatedWaterItemId = "irradiated_water";

        public PurifyWaterActionSO()
        {
            id = "action_purify_water";
            displayName = "Purify Water";
            description = "Convert irradiated water into clean drinking water.";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            bool purifierOperational = context?.Shelter != null && context.Shelter.GetModule("water_purifier") != null;
            if (!purifierOperational) return 0f;

            bool hasIrradiatedWater = false;
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && slot.Item.id == IrradiatedWaterItemId && slot.Amount > 0)
                    {
                        hasIrradiatedWater = true;
                        break;
                    }
                }
            }

            if (!hasIrradiatedWater) return 0f;

            return 0.6f;
        }
    }
}
