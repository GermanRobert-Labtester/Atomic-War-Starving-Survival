using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Companion Utility AI Action: Run Vouch.
    /// Mattis Cray companion bias: checks viaduct gate waypoints and carries approach messages.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRunVouchAction", menuName = "ASHFALL/AI Actions/Nobody's Charter/Run Vouch")]
    public class Action_RunVouch : SurvivorAction
    {
        [Header("Run Vouch")]
        [Tooltip("Base utility score for running gate vouches.")]
        [Range(0f, 1f)]
        public float baseScore = 0.42f;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.Survivor.Needs != null && context.Survivor.Needs.Fatigue > 75f) return 0f;

            float score = baseScore;
            if (context.Survivor.CraftingSkill > 0.3f)
                score += context.Survivor.CraftingSkill * 0.2f;

            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;

            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 6f);
            else if (context.Survivor.Needs != null)
                context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 6f, 0f, 100f);
        }
    }
}
