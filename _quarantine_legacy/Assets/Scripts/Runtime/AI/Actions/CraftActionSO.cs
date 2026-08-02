using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI.Actions
{
    [CreateAssetMenu(fileName = "CraftAction", menuName = "AtomicWar/AI/Actions/Craft")]
    public class CraftActionSO : UtilityActionSO
    {
        public override float EvaluateScore(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;

            // Crafting preferred during day phase when not exhausted
            if (context.GameStateSystem != null && context.GameStateSystem.CurrentPhase == DayCyclePhase.Day)
            {
                if (survivor.Fatigue > 80f || survivor.Hunger > 80f) return 10f;

                float score = 50f * WeightMultiplier;
                if (survivor.Data != null)
                {
                    score *= survivor.Data.CraftingSpeedMultiplier;
                }
                return score;
            }

            return 0f;
        }

        public override void Execute(SurvivorModel survivor, UtilityAIContext context)
        {
            survivor.CurrentState = SurvivorState.Crafting;
            Debug.Log($"[UtilityAI] {survivor.Data?.CharacterName} executed CRAFT action.");
        }
    }
}
