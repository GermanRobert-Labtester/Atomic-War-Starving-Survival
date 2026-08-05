using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_RepairFilter", menuName = "ASHFALL/AI/Repair Filter Action")]
    public class RepairFilterActionSO : SurvivorAction
    {
        public string AirFilterItemId = "air_filter";

        public RepairFilterActionSO()
        {
            id = "action_repair_filter";
            displayName = "Repair Air Filter";
            description = "Replace or repair degrading air filtration system.";
            basePriority = 0.35f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Shelter == null) return 0f;

            var airModule = context.Shelter.GetModule("air_filtration");
            if (airModule == null) return 0f;

            float filterHealth = airModule.FilterHealth;
            if (filterHealth >= 90f) return 0f;

            bool hasFilterItem = false;
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && slot.Item.id == AirFilterItemId && slot.Amount > 0)
                    {
                        hasFilterItem = true;
                        break;
                    }
                }
            }

            if (!hasFilterItem) return 0f;

            // Score rises as filterHealth falls
            return Mathf.Clamp01((100f - filterHealth) / 100f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Shelter == null) return;
            var airModule = context.Shelter.GetModule("air_filtration");
            if (airModule == null) return;

            airModule.ReplaceFilter();

            // Prompt #197 — HVAC Tech: count filter ops during FalloutStorms.
            if (context.IsFalloutStorm && context.ShelterPerks != null && context.Survivor != null)
            {
                context.ShelterPerks.RecordStormAirFilterOp(
                    context.Survivor, context.CurrentDay);
            }
        }
    }
}
