using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Companion Utility AI Action: Canvas Support.
    /// Perrin Ashby companion bias: builds consensus around draft clauses.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCanvasSupportAction", menuName = "ASHFALL/AI Actions/Nobody's Charter/Canvas Support")]
    public class Action_CanvasSupport : SurvivorAction
    {
        [Header("Canvas Support")]
        [Tooltip("Base utility score for petition canvassing.")]
        [Range(0f, 1f)]
        public float baseScore = 0.35f;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.Survivor.Needs != null && context.Survivor.Needs.Fatigue > 80f) return 0f;

            float score = baseScore;
            // Higher score if morale is healthy
            if (context.Survivor.Needs != null && context.Survivor.Needs.Morale > 60f)
                score += 0.15f;

            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;

            if (context.NeedsSystem != null)
            {
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 5f);
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Morale, 2f);
            }
            else if (context.Survivor.Needs != null)
            {
                context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 5f, 0f, 100f);
                context.Survivor.Needs.Morale = Mathf.Clamp(context.Survivor.Needs.Morale + 2f, 0f, 100f);
            }
        }
    }
}
