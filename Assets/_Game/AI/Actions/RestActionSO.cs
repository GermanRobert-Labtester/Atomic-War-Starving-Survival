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
            // Low urgency baseline fallback
            return 0.1f;
        }
    }
}
