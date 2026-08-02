using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI.Actions
{
    [CreateAssetMenu(fileName = "RestAction", menuName = "AtomicWar/AI/Actions/Rest")]
    public class RestActionSO : UtilityActionSO
    {
        public override float EvaluateScore(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;

            // Mild rest during day to recover small amount of fatigue/morale
            if (survivor.Fatigue > 40f || survivor.Morale < 60f)
            {
                return 35f * WeightMultiplier;
            }

            return 15f;
        }

        public override void Execute(SurvivorModel survivor, UtilityAIContext context)
        {
            survivor.CurrentState = SurvivorState.Idle;
            context.LegacyVitalsSystem.ModifyFatigue(survivor, -15f);
            context.LegacyVitalsSystem.ModifyMorale(survivor, 5f);
            Debug.Log($"[UtilityAI] {survivor.Data?.CharacterName} executed REST action.");
        }
    }
}
