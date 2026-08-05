using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that drives a survivor to dump waste/spoiled food into the
    /// compost bin (Prompt #167). Scores when the bin is installed and
    /// CompostProgress is low; the action adds 1 unit of waste per call.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCompostWasteAction", menuName = "ASHFALL/AI Actions/Compost Waste")]
    public class CompostWasteActionSO : SurvivorAction
    {
        public CompostWasteActionSO()
        {
            id = "action_compost_waste";
            displayName = "Compost Waste";
            description = "Dump waste/spoiled food into the compost bin. Generates fertilizer over time.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            if (!context.Survivor.IsAlive) return 0f;
            if (context.CompostSystem == null) return 0f;

            // Don't over-fill the bin. Cap at half the per-survivor daily
            // contribution; excess waste has nowhere to go.
            if (context.CompostSystem.CompostProgress > 4f) return 0f;

            // Sanitation / morale workers should focus here when morale is OK
            // but the bin is under-fed. Mid-priority, not urgent.
            float morale = context.Survivor.Needs.Morale / 100f;
            return Mathf.Clamp01(0.2f + 0.1f * morale);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.CompostSystem == null) return;
            // 1 unit per call. The system converts at WasteToFertilizerRatio.
            context.CompostSystem.AddWaste(1f);
        }
    }
}
