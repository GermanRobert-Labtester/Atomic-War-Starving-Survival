using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Utility AI: feed water/medicine to a comatose bunkmate who cannot
    /// self-care. Score rises with neglect clock and drops if water is gone.
    /// Internal Horror — The Bedridden.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_Caregive", menuName = "ASHFALL/AI/Caregive Action")]
    public class CaregiveActionSO : SurvivorAction
    {
        public CaregiveActionSO()
        {
            id = "action_caregive";
            displayName = "Caregive";
            description = "Moisten cracked lips. Tip clean water. Keep the body alive another few hours.";
            basePriority = 0.45f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            if (context.MedicalSystem == null || context.GetSurvivors == null) return 0f;

            var caregiver = context.Survivor;
            if (!caregiver.IsAlive || caregiver.State == SurvivorState.Incapacitated)
                return 0f;

            // No water → cannot care
            if (context.Inventory != null)
            {
                bool hasWater = false;
                var slots = context.Inventory.Slots;
                if (slots != null)
                {
                    for (int i = 0; i < slots.Count; i++)
                    {
                        var s = slots[i];
                        if (s?.Item == null) continue;
                        if (s.Item.type == ItemType.Water
                            || s.Item.id == "clean_water"
                            || s.Item.id == "water")
                        {
                            hasWater = true;
                            break;
                        }
                    }
                }
                if (!hasWater) return 0f;
            }

            var patients = context.GetSurvivors();
            if (patients == null || patients.Count == 0) return 0f;

            float best = 0f;
            for (int i = 0; i < patients.Count; i++)
            {
                var p = patients[i];
                if (p == null || !p.IsAlive || p.Id == caregiver.Id) continue;
                if (!context.MedicalSystem.IsComatose(p)) continue;

                var active = context.MedicalSystem.GetActive(p);
                float sinceCare = 0f;
                for (int a = 0; a < active.Count; a++)
                {
                    if (active[a].AfflictionId == AfflictionSO.Ids.Coma)
                    {
                        sinceCare = active[a].HoursSinceLastCare;
                        break;
                    }
                }

                // Urgency ramps as care interval approaches / is exceeded
                float urgency = Mathf.Clamp01(
                    sinceCare / Mathf.Max(0.5f, MedicalSystem.ComaCareIntervalHours));
                // Empathic / medical survivors step up sooner
                urgency *= 0.6f + 0.4f * Mathf.Clamp01(caregiver.MedicalSkill);
                if (urgency > best) best = urgency;
            }

            return best;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.MedicalSystem == null
                || context.GetSurvivors == null)
                return;

            var caregiver = context.Survivor;
            var patients = context.GetSurvivors();
            Survivor target = null;
            float worst = -1f;

            for (int i = 0; i < patients.Count; i++)
            {
                var p = patients[i];
                if (p == null || !p.IsAlive || p.Id == caregiver.Id) continue;
                if (!context.MedicalSystem.IsComatose(p)) continue;

                var active = context.MedicalSystem.GetActive(p);
                float sinceCare = 0f;
                for (int a = 0; a < active.Count; a++)
                {
                    if (active[a].AfflictionId == AfflictionSO.Ids.Coma)
                    {
                        sinceCare = active[a].HoursSinceLastCare;
                        break;
                    }
                }
                if (sinceCare > worst)
                {
                    worst = sinceCare;
                    target = p;
                }
            }

            if (target == null) return;
            context.MedicalSystem.TryCaregive(caregiver, target);
        }
    }
}
