using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Prompt #211 — De-Escalator unique Utility action.
    /// Instantly ends a ViolentParanoia break without meds or isolation.
    /// Only scores when the actor holds perk_de_escalator and a living
    /// survivor is currently in ViolentParanoia.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_TalkDown", menuName = "ASHFALL/AI/Talk Down Action")]
    public class TalkDownActionSO : SurvivorAction
    {
        public TalkDownActionSO()
        {
            id = "action_talk_down";
            displayName = "Talk Down";
            description = "Calm a violently paranoid survivor with words alone. No meds, no isolation.";
            basePriority = 0.55f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.SocialPerks == null || !context.SocialPerks.HasDeEscalator(context.Survivor))
                return 0f;
            if (context.MentalBreak == null || context.GetSurvivors == null) return 0f;

            var all = context.GetSurvivors();
            if (all == null || all.Count == 0) return 0f;

            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (s == null || s == context.Survivor || !s.IsAlive || !s.HasMentalBreak) continue;
                if (SocialPerkSystem.IsViolentParanoia(s.currentMentalBreakId))
                    return 1f; // highest urgency — active violent break
            }
            return 0f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.SocialPerks == null) return;
            if (context.MentalBreak == null || context.GetSurvivors == null) return;

            var all = context.GetSurvivors();
            if (all == null) return;

            Survivor target = null;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (s == null || s == context.Survivor || !s.IsAlive || !s.HasMentalBreak) continue;
                if (SocialPerkSystem.IsViolentParanoia(s.currentMentalBreakId))
                {
                    target = s;
                    break;
                }
            }
            if (target == null) return;

            context.SocialPerks.TryTalkDown(
                context.Survivor,
                target,
                context.MentalBreak,
                context.CurrentDay);
        }
    }
}
