using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI.Actions
{
    [CreateAssetMenu(fileName = "GuardAction", menuName = "AtomicWar/AI/Actions/Guard")]
    public class GuardActionSO : UtilityActionSO
    {
        public override float EvaluateScore(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;

            // Guarding is primarily valuable at night
            if (context.GameStateSystem != null &&
                (context.GameStateSystem.CurrentPhase == DayCyclePhase.Night ||
                 context.GameStateSystem.CurrentPhase == DayCyclePhase.Evening))
            {
                float baseScore = 60f * WeightMultiplier;
                // Survivors with high combat efficiency score higher for guarding
                if (survivor.Data != null)
                {
                    baseScore *= survivor.Data.CombatEfficiency;
                }
                return baseScore;
            }

            return 10f; // Low score during day
        }

        public override void Execute(SurvivorModel survivor, UtilityAIContext context)
        {
            survivor.CurrentState = SurvivorState.Guard;
            Debug.Log($"[UtilityAI] {survivor.Data?.CharacterName} executed GUARD action.");
        }
    }
}
