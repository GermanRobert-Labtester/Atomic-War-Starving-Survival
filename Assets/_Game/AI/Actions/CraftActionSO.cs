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

            float avgNeed = (context.Survivor.Needs.Hunger + context.Survivor.Needs.Thirst + context.Survivor.Needs.Fatigue) / 3f;
            if (avgNeed > 60f) return 0f;

            return Mathf.Clamp01((60f - avgNeed) / 100f + 0.1f);
        }

        /// <summary>
        /// Prompt #213 — Taskmaster Pacing Aura work-rate for craft ticks.
        /// Callers with duration-based craft should multiply hours by this.
        /// </summary>
        public static float GetCraftSpeedMultiplier(AIContext context)
        {
            if (context?.SocialPerks == null || context.Survivor == null) return 1f;
            if (context.GetSurvivors == null) return 1f;
            return context.SocialPerks.GetPacingAuraMultiplier(
                context.Survivor,
                context.GetSurvivors(),
                context.AreRoomsAdjacent);
        }
    }
}
