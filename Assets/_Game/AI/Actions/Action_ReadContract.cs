using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Companion Utility AI Action: Read Contract.
    /// Dessa Vane companion bias: reviews Underwrite terms and verifies pledges.
    /// </summary>
    [CreateAssetMenu(fileName = "NewReadContractAction", menuName = "ASHFALL/AI Actions/Nobody's Charter/Read Contract")]
    public class Action_ReadContract : SurvivorAction
    {
        [Header("Read Contract")]
        [Tooltip("Base utility score for reading contract terms.")]
        [Range(0f, 1f)]
        public float baseScore = 0.38f;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.Survivor.Needs != null && context.Survivor.Needs.Fatigue > 80f) return 0f;

            float score = baseScore;
            if (context.Survivor.ScienceSkill > 0.3f)
                score += context.Survivor.ScienceSkill * 0.2f;

            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;

            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 3f);
            else if (context.Survivor.Needs != null)
                context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 3f, 0f, 100f);
        }
    }
}
