using AtomicWar.Data;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI.Actions
{
    [CreateAssetMenu(fileName = "EatAction", menuName = "AtomicWar/AI/Actions/Eat")]
    public class EatActionSO : UtilityActionSO
    {
        public override float EvaluateScore(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;

            // Normalize Hunger (0 to 1)
            float normalizedHunger = Mathf.Clamp01(survivor.Hunger / 100f);

            // Curve scoring
            float rawScore = UtilityCurve.Evaluate(normalizedHunger) * 100f * WeightMultiplier;

            // Check if food exists in inventory; if not, score drops to 0
            bool hasFood = context.InventorySystem != null && context.InventorySystem.Slots.Count > 0;
            if (!hasFood) return 0f;

            return rawScore;
        }

        public override void Execute(SurvivorModel survivor, UtilityAIContext context)
        {
            survivor.CurrentState = SurvivorState.Idle;
            // Reduce hunger if food consumed
            context.LegacyVitalsSystem.ModifyHunger(survivor, -40f);
            Debug.Log($"[UtilityAI] {survivor.Data?.CharacterName} executed EAT action.");
        }
    }
}
