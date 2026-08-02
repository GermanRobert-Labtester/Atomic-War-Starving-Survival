using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Guard", menuName = "ASHFALL/AI/Guard Action")]
    public class GuardActionSO : SurvivorAction
    {
        public GuardActionSO()
        {
            id = "action_guard";
            displayName = "Guard";
            description = "Keep watch over the shelter perimeter.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            // Rises when basic needs (hunger, thirst, fatigue) are low
            float averageNeed = (context.Survivor.Needs.Hunger + context.Survivor.Needs.Thirst + context.Survivor.Needs.Fatigue) / 3f;
            if (averageNeed > 50f) return 0.05f;

            return Mathf.Clamp01((50f - averageNeed) / 100f + 0.15f);
        }
    }
}
