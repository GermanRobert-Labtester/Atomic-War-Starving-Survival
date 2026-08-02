using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Panic", menuName = "ASHFALL/AI/Panic Action")]
    public class PanicActionSO : SurvivorAction
    {
        public PanicActionSO()
        {
            id = "action_panic";
            displayName = "Panic";
            description = "Panic behavior triggered by low morale or acute radiation sickness.";
            basePriority = 0.5f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            float morale = context.Survivor.Needs.Morale;
            if (morale <= 15f || context.Survivor.HasAcuteRadiationSickness)
            {
                return 0.99f;
            }

            return 0f;
        }
    }
}
