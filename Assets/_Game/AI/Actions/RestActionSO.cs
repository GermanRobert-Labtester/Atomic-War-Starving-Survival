using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Rest", menuName = "ASHFALL/AI/Rest Action")]
    public class RestActionSO : SurvivorAction
    {
        public RestActionSO()
        {
            id = "action_rest";
            displayName = "Rest";
            description = "Fallback action when no urgent needs require attention.";
            basePriority = 0.05f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            // #250 Workaholic: ignore Rest until fatigue is near collapse.
            if (context.PersonalQuests != null
                && context.PersonalQuests.ShouldIgnoreRestAction(context.Survivor))
                return 0f;

            // #252 Hardened Daughter: refuses idle Play/Rest comfort.
            if (context.PersonalQuests != null
                && context.PersonalQuests.RefusesPlayOrComfort(context.Survivor))
                return 0f;

            // Low urgency baseline fallback
            return 0.1f;
        }
    }
}
