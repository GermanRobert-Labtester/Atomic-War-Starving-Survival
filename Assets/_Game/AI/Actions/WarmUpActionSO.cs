using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_WarmUp", menuName = "ASHFALL/AI/WarmUp Action")]
    public class WarmUpActionSO : SurvivorAction
    {
        public string FuelItemId = "fuel";

        public WarmUpActionSO()
        {
            id = "action_warm_up";
            displayName = "Warm Up";
            description = "Activate or stoke the shelter heater when freezing.";
            basePriority = 0.3f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            float warmth = context.Survivor.Needs.Warmth;

            if (warmth >= 30f)
            {
                return Mathf.Clamp01((100f - warmth) / 200f); // Low priority if warmth >= 30
            }

            // Warmth < 30: requires heater and fuel available
            bool heaterExists = context.Shelter != null && context.Shelter.GetModule("heater") != null;
            if (!heaterExists) return 0f;

            var heaterModule = context.Shelter.GetModule("heater");
            bool hasFuel = heaterModule.Fuel > 0f;
            if (!hasFuel && context.Inventory != null)
            {
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && slot.Item.id == FuelItemId && slot.Amount > 0)
                    {
                        hasFuel = true;
                        break;
                    }
                }
            }

            if (!hasFuel) return 0f;

            // Score spikes when warmth < 30, fuel available, and heater exists
            return 0.95f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor != null)
            {
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(context.Survivor, NeedKind.Warmth, 40f);
                else
                    context.Survivor.Needs.Warmth = Mathf.Min(100f, context.Survivor.Needs.Warmth + 40f);
            }
        }
    }
}
