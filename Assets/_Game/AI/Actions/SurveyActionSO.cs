using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Stop and take radiation readings with a working instrument.
    /// Costs time and risks the surveyor; updates map knowledge.
    /// Actual survey execution is driven by LocationScavengingSystem.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_Survey", menuName = "ASHFALL/AI/Survey Action")]
    public class SurveyActionSO : SurvivorAction
    {
        public SurveyActionSO()
        {
            id = "action_survey";
            displayName = "Survey";
            description = "Stop and take radiation readings with a working geiger counter.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            if (context.IsFalloutStorm && !context.Survivor.HasFullSuitEquipped) return 0f;

            // Prefer surveying when ambient uncertainty is high and a device is available.
            // Inventory check is soft — execution path validates a working geiger.
            bool hasGeiger = context.Inventory != null && context.Inventory.HasWorkingGeiger();
            if (!hasGeiger) return 0f;
            if (context.OnRequestSurvey == null) return 0f;

            // Mild priority: scavenge still usually wins when hungry; survey when safer.
            float avgNeed = (context.Survivor.Needs.Hunger + context.Survivor.Needs.Thirst) / 2f;
            float needPressure = Mathf.Clamp01(avgNeed / 100f);
            return Mathf.Clamp01(0.35f - needPressure * 0.2f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return;
            context.OnRequestSurvey?.Invoke(context.Survivor);
        }
    }
}
