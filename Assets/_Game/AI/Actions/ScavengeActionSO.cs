using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Scavenge", menuName = "ASHFALL/AI/Scavenge Action")]
    public class ScavengeActionSO : SurvivorAction
    {
        public ScavengeActionSO()
        {
            id = "action_scavenge";
            displayName = "Scavenge";
            description = "Explore ruins outside for supplies and materials.";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            // Scavenge score suppressed to 0 during FalloutStorm unless full suit equipped
            if (context.IsFalloutStorm)
            {
                bool fullSuitEquipped = context.Survivor.HasFullSuitEquipped;
                if (!fullSuitEquipped)
                {
                    return 0f;
                }
            }

            float avgNeed = (context.Survivor.Needs.Hunger + context.Survivor.Needs.Thirst) / 2f;
            return Mathf.Clamp01(avgNeed / 100f + 0.15f);
        }
    }
}
