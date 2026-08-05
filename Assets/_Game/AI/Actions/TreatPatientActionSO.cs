using UnityEngine;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Utility AI: treat a wounded/ill survivor. Score rises with untreated
    /// affliction pressure. High Medical skill completes care faster (via MedicalSystem).
    /// </summary>
    [CreateAssetMenu(fileName = "Action_TreatPatient", menuName = "ASHFALL/AI/Treat Patient Action")]
    public class TreatPatientActionSO : SurvivorAction
    {
        public TreatPatientActionSO()
        {
            id = "action_treat_patient";
            displayName = "Treat Patient";
            description = "Dress wounds, set bones, or run a treatment recipe on a sick survivor.";
            basePriority = 0.35f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            if (context.MedicalSystem == null) return 0f;
            if (context.GetSurvivors == null) return 0f;

            var medic = context.Survivor;
            // Medics don't treat themselves via this action when alone and healthy
            var patients = context.GetSurvivors();
            if (patients == null || patients.Count == 0) return 0f;

            float best = 0f;
            for (int i = 0; i < patients.Count; i++)
            {
                var patient = patients[i];
                if (patient == null || !patient.IsAlive) continue;
                if (patient.Id == medic.Id && patients.Count > 1) continue;

                var active = context.MedicalSystem.GetActive(patient);
                if (active == null || active.Count == 0) continue;

                float pressure = 0f;
                for (int a = 0; a < active.Count; a++)
                {
                    var aff = active[a];
                    if (aff.IsTreating) continue;
                    var def = context.MedicalSystem.GetAfflictionDef(aff.AfflictionId);
                    if (def == null) continue;
                    float urgency = def.healthDrainPerHour * context.MedicalSystem.EffectiveLethality(patient, def);
                    if (!aff.ProgressionHalted && def.progressionHours > 0f)
                    {
                        // Closer to progression = higher urgency
                        float t = 1f - Mathf.Clamp01(aff.HoursUntilProgression / Mathf.Max(1f, def.progressionHours));
                        urgency *= 1f + t;
                    }
                    pressure += urgency;
                }

                // Slight bias: skilled medics step up more often
                pressure *= 0.5f + 0.5f * Mathf.Clamp01(medic.MedicalSkill);
                if (pressure > best) best = pressure;
            }

            return Mathf.Clamp01(best / 10f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.MedicalSystem == null || context.GetSurvivors == null)
                return;

            var medic = context.Survivor;
            var patients = context.GetSurvivors();
            Survivor patient = null;
            ActiveAffliction worst = null;
            float worstScore = -1f;

            for (int i = 0; i < patients.Count; i++)
            {
                var p = patients[i];
                if (p == null || !p.IsAlive) continue;
                var active = context.MedicalSystem.GetActive(p);
                for (int a = 0; a < active.Count; a++)
                {
                    var aff = active[a];
                    if (aff.IsTreating) continue;
                    var def = context.MedicalSystem.GetAfflictionDef(aff.AfflictionId);
                    if (def == null) continue;
                    float score = def.healthDrainPerHour;
                    if (!aff.ProgressionHalted) score += 5f;
                    if (score > worstScore)
                    {
                        worstScore = score;
                        worst = aff;
                        patient = p;
                    }
                }
            }

            if (patient == null || worst == null) return;

            // Prompt #202 — mark medical action for uninterruptible triage.
            context.MedicalPerks?.BeginMedicalAction(medic);
            try
            {
            // Prefer emergency halt item if available
            var defHalt = context.MedicalSystem.GetAfflictionDef(worst.AfflictionId);
            if (defHalt != null && !string.IsNullOrEmpty(defHalt.emergencyHaltItemId)
                && !worst.ProgressionHalted)
            {
                if (context.MedicalSystem.TryEmergencyHalt(
                        patient, defHalt.emergencyHaltItemId, medic: medic))
                    return;
            }

            // Full treatment recipe if one is registered for this affliction
            TryStartAnyTreatment(context, medic, patient, worst.AfflictionId);

            // Mental-break cure via the medical bed: only fires for
            // breaks that explicitly require it. Closes the loop on
            // ViolentParanoia and similar hospital-grade breaks.
            if (context.MentalBreak != null && context.Shelter != null
                && context.MedicalSystem != null
                && patient.HasMentalBreak
                && context.MedicalSystem.CanCureMentalBreak(patient, context.MentalBreak, context.Shelter))
            {
                context.MedicalSystem.TryCureMentalBreak(patient, context.MentalBreak, context.Shelter);
            }
            }
            finally
            {
                context.MedicalPerks?.EndMedicalAction(medic);
            }
        }

        private static void TryStartAnyTreatment(
            AIContext context, Survivor medic, Survivor patient, string afflictionId)
        {
            // Common recipe ids used by bootstrap / tests
            string[] candidates =
            {
                "treat_gunshot_full",
                "treat_gunshot_bandage",
                "treat_" + afflictionId,
                "treat_" + afflictionId + "_full"
            };
            for (int i = 0; i < candidates.Length; i++)
            {
                var recipe = context.MedicalSystem.GetTreatment(candidates[i]);
                if (recipe == null || recipe.targetAfflictionId != afflictionId) continue;
                if (context.MedicalSystem.TryStartTreatment(medic, patient, recipe))
                    return;
            }
        }
    }
}
