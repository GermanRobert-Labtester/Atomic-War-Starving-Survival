using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that hauls loot from the airlock to internal storage
    /// (Prompt #173 — InternalHaulingSystem). Scores when the airlock has
    /// dumped loot; the action moves a fraction of the weight into the
    /// survivor's carrying capacity and costs fatigue.
    /// </summary>
    [CreateAssetMenu(fileName = "NewHaulLootAction", menuName = "ASHFALL/AI Actions/Haul Loot")]
    public class HaulLootActionSO : SurvivorAction
    {
        public HaulLootActionSO()
        {
            id = "action_haul_loot";
            displayName = "Haul Loot";
            description = "Move dumped loot from the airlock to internal storage. Fatigue cost.";
            basePriority = 0.4f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;
            // Score proportional to how much loot is waiting.
            float weight = context.HaulingSystem.AirlockDumpedWeight;
            float urgency = Mathf.Clamp01(weight / 50f);
            return 0.3f + 0.4f * urgency;
        }

        private static bool MeetsPrerequisites(AIContext context)
            => CanScavenge(context)
               && context.HaulingSystem != null
               && context.HaulingSystem.AirlockDumpedWeight > 0f
               && context.Survivor.Needs.Fatigue <= 80f;

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.HaulingSystem == null) return;
            // 1 hour of work per call. The system moves 20kg/hour and adds
            // 0.5 fatigue per kg. Sub-stepping is handled by the host loop.
            float moved = context.HaulingSystem.HaulFromAirlock(context.Survivor, 1f);

            // Prompt #212 — Quartermaster: tally items moved via InternalHauling.
            if (moved > 0f && context.SocialPerks != null)
                context.SocialPerks.RecordItemsHauled(context.Survivor, moved, context.CurrentDay);
        }
    }
}
