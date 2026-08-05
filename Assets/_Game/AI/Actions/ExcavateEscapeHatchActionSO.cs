using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that progresses the secondary escape hatch excavation
    /// (Prompt #126 — EscapeHatchSystem). 120 hours of work; once complete,
    /// the survivor can trigger evacuation as an alternate endgame. Scores
    /// when the hatch is not yet built AND the survivor has the time.
    /// </summary>
    [CreateAssetMenu(fileName = "NewExcavateEscapeHatchAction", menuName = "ASHFALL/AI Actions/Excavate Escape Hatch")]
    public class ExcavateEscapeHatchActionSO : SurvivorAction
    {
        public ExcavateEscapeHatchActionSO()
        {
            id = "action_excavate_escape_hatch";
            displayName = "Excavate Escape Hatch";
            description = "Spend 120 hours digging the secondary escape hatch (alternate endgame).";
            basePriority = 0.1f; // very long, very rare
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;
            // Mid-game (Day 30+): the alternate endgame becomes relevant.
            if (context.CurrentDay < 30) return 0.1f;
            // Score rises with how "stuck" the survivor feels (low morale, no
            // upgrades left to do, no expeditions available).
            float morale = context.Survivor.Needs.Morale / 100f;
            float stuck = 1f - morale;
            return Mathf.Clamp01(0.15f + 0.3f * stuck);
        }

        private static bool MeetsPrerequisites(AIContext context)
            => CanCraft(context)
               && context.EscapeHatchSystem != null
               && !context.EscapeHatchSystem.IsBuilt;

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.EscapeHatchSystem == null) return;
            // 2 hours per call (AI ticks at substep rate).
            context.EscapeHatchSystem.Excavate(2f);
        }
    }
}
