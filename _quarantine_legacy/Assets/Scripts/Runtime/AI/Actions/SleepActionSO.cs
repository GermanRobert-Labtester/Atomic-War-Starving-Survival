using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI.Actions
{
    [CreateAssetMenu(fileName = "SleepAction", menuName = "AtomicWar/AI/Actions/Sleep")]
    public class SleepActionSO : UtilityActionSO
    {
        public override float EvaluateScore(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;

            float normalizedFatigue = Mathf.Clamp01(survivor.Fatigue / 100f);
            float score = UtilityCurve.Evaluate(normalizedFatigue) * 100f * WeightMultiplier;

            // Night/Evening increases urgency to sleep if not assigned to guard/scavenge
            if (context.GameStateSystem != null && 
                (context.GameStateSystem.CurrentPhase == DayCyclePhase.Night || context.GameStateSystem.CurrentPhase == DayCyclePhase.Evening))
            {
                score += 25f;
            }

            return score;
        }

        public override void Execute(SurvivorModel survivor, UtilityAIContext context)
        {
            survivor.CurrentState = SurvivorState.Sleeping;
            Debug.Log($"[UtilityAI] {survivor.Data?.CharacterName} executed SLEEP action.");
        }
    }
}
