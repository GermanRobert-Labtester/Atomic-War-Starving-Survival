using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_UseAntiRad", menuName = "ASHFALL/AI/Use AntiRad Action")]
    public class UseAntiRadActionSO : SurvivorAction
    {
        public const float BaseRadReduction = 30f;

        public string AntiRadItemId = "anti_rad";

        /// <summary>Host: peek next-dose effectiveness (0..1) before recording use.</summary>
        public Func<Survivor, string, float> GetChemEffectiveness;

        /// <summary>Host: peek next-dose duration hours before recording use.</summary>
        public Func<Survivor, string, float> GetChemDurationHours;

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
            if (context?.Survivor == null) return;

            // Consume one anti-rad unit when present (host tracks chem side-effects).
            bool consumed = false;
            if (context.Inventory != null && context.Inventory.Slots != null)
            {
                for (int i = 0; i < context.Inventory.Slots.Count; i++)
                {
                    var slot = context.Inventory.Slots[i];
                    if (slot != null && slot.Item != null
                        && slot.Item.id == AntiRadItemId && slot.Amount > 0)
                    {
                        if (context.Inventory.Remove(slot.Item, 1))
                        {
                            consumed = true;
                            break;
                        }
                    }
                }
            }

            // Only apply therapeutic effect when a dose was actually taken.
            // (Score already requires stock when rad is high; free doses are not allowed.)
            if (!(consumed || context.Inventory == null)) return;

            float effectiveness = GetChemEffectiveness != null
                ? Mathf.Clamp01(GetChemEffectiveness(context.Survivor, AntiRadItemId))
                : 1f;
            float durationHours = GetChemDurationHours != null
                ? Mathf.Max(0f, GetChemDurationHours(context.Survivor, AntiRadItemId))
                : 24f;

            // Prompt #833 — tolerance shrinks cleanse; 6+ uses → no therapeutic benefit.
            // DEEP3-INV-005 — prefer the route through RadiationSystem.AdministerAntiRad
            // so the per-run GetRadAwayEfficiencyMultiplier applies. Fall back to a
            // direct RadiationDose write when no system is provided (e.g. host-injected
            // AI action running in a test that does not bind a radiation host) so the
            // pre-fix behaviour is preserved for those callers.
            if (effectiveness > 0f)
            {
                if (context.RadiationSystem != null)
                {
                    context.RadiationSystem.AdministerAntiRad(
                        context.Survivor, BaseRadReduction * effectiveness);
                }
                else
                {
                    float cleanse = BaseRadReduction * effectiveness;
                    context.Survivor.RadiationDose =
                        Mathf.Max(0f, context.Survivor.RadiationDose - cleanse);
                }
            }

            // Duration grants temporary rad resistance (half-strength window from iodine path).
            if (effectiveness > 0f && durationHours > 0f)
            {
                float resistHours = durationHours * effectiveness;
                context.Survivor.RadResistanceHoursRemaining =
                    Mathf.Max(context.Survivor.RadResistanceHoursRemaining, resistHours);
                context.Survivor.HasRadResistance = true;
            }
        }
    }
}
