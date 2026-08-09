using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Chart Map — Cartography table work (Prompt #67). The survivor spends time
    /// at the cartography table processing IntelNodes into charted map entries.
    /// Scores when there are uncharted nodes with available intel.
    /// </summary>
    [CreateAssetMenu(fileName = "NewChartMapAction", menuName = "ASHFALL/AI Actions/Chart Map")]
    public class ChartMapActionSO : SurvivorAction
    {
        [Header("Chart Map")]
        [Tooltip("Base utility score when uncharted nodes exist.")]
        [Range(0f, 1f)]
        public float baseScore = 0.45f;

        [Tooltip("Score bonus when cartography table has supplies.")]
        [Range(0f, 0.5f)]
        public float suppliesBonus = 0.2f;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.CartographySystem == null) return 0f;
            if (!context.CartographySystem.HasModule) return 0f;

            // Science-skilled survivors are more drawn to cartography work.
            float scienceSkill = context.Survivor.ScienceSkill;
            if (scienceSkill < 0.2f) return 0f;

            string uncharted = context.GetUnchartedNodeId?.Invoke();
            if (string.IsNullOrEmpty(uncharted)) return 0f;

            float score = baseScore;
            score += scienceSkill * 0.3f;
            if (context.CartographySystem.HasSupplies)
                score += suppliesBonus;

            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.CartographySystem == null) return;

            string nodeId = context.GetUnchartedNodeId?.Invoke();
            if (string.IsNullOrEmpty(nodeId)) return;

            bool charted = context.CartographySystem.ChartNode(
                nodeId, context.Survivor.ScienceSkill);
            if (charted)
            {
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 5f);
                else
                    context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 5f, 0f, 100f);
            }
        }
    }
}
