using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_UseAntiRad", menuName = "ASHFALL/AI/Use AntiRad Action")]
    public class UseAntiRadActionSO : SurvivorAction
    {
        public string AntiRadItemId = "anti_rad";

        public UseAntiRadActionSO()
        {
            id = "action_use_antirad";
            displayName = "Use Anti-Rad";
            description = "Administer anti-rad medication when radiation dose is high.";
            basePriority = 0.5f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            float radDose = context.Survivor.RadiationDose;
            if (radDose < 60f)
            {
                return Mathf.Clamp01(radDose / 120f);
            }

            bool hasAntiRad = false;
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && slot.Item.id == AntiRadItemId && slot.Amount > 0)
                    {
                        hasAntiRad = true;
                        break;
                    }
                }
            }

            if (!hasAntiRad) return 0f;

            // Score spikes when radiation >= 60 and anti-rad is in inventory
            return 0.98f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor != null)
            {
                context.Survivor.RadiationDose = Mathf.Max(0f, context.Survivor.RadiationDose - 30f);
            }
        }
    }
}
