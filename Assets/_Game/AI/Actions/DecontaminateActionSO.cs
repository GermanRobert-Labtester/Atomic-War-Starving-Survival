using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Decontaminate", menuName = "ASHFALL/AI/Decontaminate Action")]
    public class DecontaminateActionSO : SurvivorAction
    {
        public DecontaminateActionSO()
        {
            id = "action_decontaminate";
            displayName = "Decontaminate";
            description = "Wash off radioactive dust at a decontamination station.";
            basePriority = 0.25f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            bool deconStationOperational = context.Shelter != null && context.Shelter.GetModule("decon_station") != null;
            if (!deconStationOperational) return 0f;

            float dose = context.Survivor.RadiationDose;
            if (dose <= 0f) return 0f;

            return Mathf.Clamp01(dose / 80f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor != null)
            {
                context.Survivor.RadiationDose = Mathf.Max(0f, context.Survivor.RadiationDose - 15f);
            }
        }
    }
}
