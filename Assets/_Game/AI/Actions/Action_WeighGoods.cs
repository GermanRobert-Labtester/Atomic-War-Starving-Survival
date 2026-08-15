using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Companion Utility AI Action: Weigh Goods.
    /// Osran Kell companion bias: verifies cargo mass against depot calibration.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeighGoodsAction", menuName = "ASHFALL/AI Actions/Nobody's Charter/Weigh Goods")]
    public class Action_WeighGoods : SurvivorAction
    {
        [Header("Weigh Goods")]
        [Tooltip("Base utility score for trade goods verification.")]
        [Range(0f, 1f)]
        public float baseScore = 0.40f;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.Survivor.Needs != null && context.Survivor.Needs.Fatigue > 85f) return 0f;

            float score = baseScore;
            // Higher score if survivor has crafting / precision skills
            if (context.Survivor.CraftingSkill > 0.4f)
                score += context.Survivor.CraftingSkill * 0.25f;

            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;

            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 4f);
            else if (context.Survivor.Needs != null)
                context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 4f, 0f, 100f);
        }
    }
}
