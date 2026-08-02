using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.AI.Actions
{
    [CreateAssetMenu(fileName = "PanicAction", menuName = "AtomicWar/AI/Actions/Panic")]
    public class PanicActionSO : UtilityActionSO
    {
        public override float EvaluateScore(SurvivorModel survivor, UtilityAIContext context)
        {
            if (survivor == null || !survivor.IsAlive) return 0f;

            // Panic overrides all other actions if Morale drops to 0 (mentally broken)
            if (survivor.Morale <= 0f)
            {
                return 1000f; // Dominant emergency utility score
            }

            return 0f;
        }

        public override void Execute(SurvivorModel survivor, UtilityAIContext context)
        {
            survivor.CurrentState = SurvivorState.Idle;
            Debug.LogWarning($"[UtilityAI] {survivor.Data?.CharacterName} is PANICKING due to mental breakdown!");
        }
    }
}
