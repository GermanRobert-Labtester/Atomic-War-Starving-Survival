using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "Action_Craft", menuName = "ASHFALL/AI/Craft Action")]
    public class CraftActionSO : SurvivorAction
    {
        public CraftActionSO()
        {
            id = "action_craft";
            displayName = "Craft";
            description = "Craft tools or supplies at a workbench.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;

            // Cannot craft (child, severe injury, etc.)
            if (context.Survivor.CannotCraft) return 0f;

            bool workbenchPresent = context.Shelter != null && context.Shelter.GetModule("workbench") != null;
            if (!workbenchPresent) return 0f;

            float fatigue = context.Survivor.Needs.Fatigue;
            // #250 Workaholic / #250 Pillar: fatigue less likely to block craft.
            if (context.PersonalQuests != null
                && context.PersonalQuests.IgnoresFatigueActionSpeedPenalty(context.Survivor))
                fatigue = 0f;
            else if (context.PersonalQuests != null
                     && context.PersonalQuests.HasWorkaholic(context.Survivor))
                fatigue *= context.PersonalQuests.GetCraftRepairFatigueDrainMultiplier(context.Survivor);

            float avgNeed = (context.Survivor.Needs.Hunger + context.Survivor.Needs.Thirst + fatigue) / 3f;
            if (avgNeed > 60f) return 0f;

            float score = Mathf.Clamp01((60f - avgNeed) / 100f + 0.1f);
            // #253 Cold Calculus: thrives when the bunker is nearly empty.
            if (context.PersonalQuests != null && context.GetSurvivors != null)
            {
                int pop = 0;
                var list = context.GetSurvivors();
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                        if (list[i] != null && list[i].IsAlive) pop++;
                }
                score = Mathf.Clamp01(score
                    * context.PersonalQuests.GetUtilityExecutionSpeedMultiplier(context.Survivor, pop));
            }
            return score;
        }

        /// <summary>
        /// Prompt #213 — Taskmaster Pacing Aura work-rate for craft ticks.
        /// #253 Cold Calculus multiplies when shelter population is under 3.
        /// Callers with duration-based craft should divide hours by this
        /// (higher mult = faster = fewer hours).
        /// </summary>
        public static float GetCraftSpeedMultiplier(AIContext context)
        {
            float m = 1f;
            if (context?.SocialPerks != null && context.Survivor != null
                && context.GetSurvivors != null)
            {
                m *= context.SocialPerks.GetPacingAuraMultiplier(
                    context.Survivor,
                    context.GetSurvivors(),
                    context.AreRoomsAdjacent);
            }
            if (context?.PersonalQuests != null && context.Survivor != null
                && context.GetSurvivors != null)
            {
                int pop = 0;
                var list = context.GetSurvivors();
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                        if (list[i] != null && list[i].IsAlive) pop++;
                }
                m *= context.PersonalQuests.GetUtilityExecutionSpeedMultiplier(
                    context.Survivor, pop);

                // #312 Apprentice Clumsy: chance to break a tool mid-craft, slowing the work.
                if (context.PersonalQuests.RollClumsyToolBreak(context.Survivor, context.Random))
                    m *= 0.5f;
            }
            return m;
        }
    }
}
