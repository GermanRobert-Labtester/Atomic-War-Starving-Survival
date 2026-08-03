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
            float baseScore = Mathf.Clamp01(avgNeed / 100f + 0.15f);
            // High map uncertainty (stale/dark fog-of-war) damps scavenger eagerness:
            // survivors prefer known ground when instruments have failed.
            float uncertaintyDamp = Mathf.Lerp(1f, 0.55f, Mathf.Clamp01(context.MapUncertainty));
            // Belief/risk-perception bias: trait, perceived rad risk, and instrument trust
            // further gate willingness to scavenge on top of the raw uncertainty damp (see BeliefSystem).
            float beliefMultiplier = context.BeliefSystem != null
                ? context.BeliefSystem.ComputeScavengeMultiplier(context.Survivor, context.MapUncertainty)
                : 1f;
            return Mathf.Clamp01(baseScore * uncertaintyDamp * beliefMultiplier);
        }
    }
}
