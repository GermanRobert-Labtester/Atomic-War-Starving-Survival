using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_RefuelHeater", menuName = "ASHFALL/AI/Refuel Heater Action")]
    public class RefuelHeaterActionSO : SurvivorAction
    {
        public string FuelItemId = "fuel";

        public RefuelHeaterActionSO()
        {
            id = "action_refuel_heater";
            displayName = "Refuel Heater";
            description = "Add fuel to the shelter heater.";
            basePriority = 0.25f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Shelter == null) return 0f;

            var heaterModule = context.Shelter.GetModule("heater");
            if (heaterModule == null) return 0f;

            bool hasFuelItem = false;
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null && slot.Item.id == FuelItemId && slot.Amount > 0)
                    {
                        hasFuelItem = true;
                        break;
                    }
                }
            }

            if (!hasFuelItem) return 0f;

            float fuel = heaterModule.Fuel;
            if (fuel >= 10f) return 0.05f;

            return Mathf.Clamp01((10f - fuel) / 10f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Shelter == null) return;
            var heaterModule = context.Shelter.GetModule("heater");
            if (heaterModule == null) return;

            // Prompt #200 — Thermodynamics: fuel burns 20% longer when this survivor loads it.
            float burnMult = context.ShelterPerks != null
                ? context.ShelterPerks.GetFuelBurnMultiplier(context.Survivor)
                : 1f;
            heaterModule.AddFuel(5f, burnMult);
        }
    }
}
