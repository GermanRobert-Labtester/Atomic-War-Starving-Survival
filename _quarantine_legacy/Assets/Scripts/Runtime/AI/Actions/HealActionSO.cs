using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI.Actions
{
    [CreateAssetMenu(fileName = "HealAction", menuName = "AtomicWar/AI/Actions/Heal")]
    public class HealActionSO : UtilityActionSO
    {
        public override float EvaluateScore(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;

            // Score based on Sickness level and missing Health
            float sicknessNormalized = Mathf.Clamp01(survivor.Sickness / 100f);
            float missingHealthNormalized = Mathf.Clamp01((100f - survivor.Health) / 100f);

            float highestUrgency = Mathf.Max(sicknessNormalized, missingHealthNormalized);
            float score = UtilityCurve.Evaluate(highestUrgency) * 100f * WeightMultiplier;

            return score;
        }

        public override void Execute(SurvivorModel survivor, UtilityAIContext context)
        {
            survivor.CurrentState = SurvivorState.Idle;
            context.LegacyVitalsSystem.ModifySickness(survivor, -30f);
            context.LegacyVitalsSystem.ModifyHealth(survivor, 20f);
            Debug.Log($"[UtilityAI] {survivor.Data?.CharacterName} executed HEAL action.");
        }
    }
}
