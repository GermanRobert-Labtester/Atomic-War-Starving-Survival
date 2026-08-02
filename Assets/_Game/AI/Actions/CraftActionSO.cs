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

            bool workbenchPresent = context.Shelter != null && context.Shelter.GetModule("workbench") != null;
            if (!workbenchPresent) return 0f;

            float avgNeed = (context.Survivor.Needs.Hunger + context.Survivor.Needs.Thirst + context.Survivor.Needs.Fatigue) / 3f;
            if (avgNeed > 60f) return 0f;

            return Mathf.Clamp01((60f - avgNeed) / 100f + 0.1f);
        }
    }
}
