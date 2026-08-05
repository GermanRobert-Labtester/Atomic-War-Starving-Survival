using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that initiates chelation therapy (Prompt #170). A 5-day
    /// coma with constant IV; extremely costly in water + food. Only scores
    /// when the survivor has very high LifetimeRadiationExposure AND the
    /// bunker has the resources to sustain the IV.
    /// </summary>
    [CreateAssetMenu(fileName = "NewBeginChelationAction", menuName = "ASHFALL/AI Actions/Begin Chelation")]
    public class BeginChelationActionSO : SurvivorAction
    {
        public const float ChelationThresholdRad = 400f; // mSv

        public BeginChelationActionSO()
        {
            id = "action_begin_chelation";
            displayName = "Begin Chelation Therapy";
            description = "Commit to 5 days of coma + IV chelation. -500 LifetimeRads. High water/food cost.";
            basePriority = 0.05f; // rare, intentional, last-resort
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;

            // Score proportional to how badly the survivor needs it.
            float over = context.Survivor.LifetimeRadiationExposure - ChelationThresholdRad;
            float urgency = Mathf.Clamp01(over / 200f);
            return 0.4f + 0.4f * urgency;
        }

        private static bool MeetsPrerequisites(AIContext context)
        {
            if (!HasLivingSurvivor(context) || context.ChelationSystem == null) return false;
            bool alreadyChelating = context.ChelationSystem.IsUndergoingChelation(context.Survivor.Id);
            bool radHigh = context.Survivor.LifetimeRadiationExposure >= ChelationThresholdRad;
            bool waterOk = context.WaterStorage != null && context.WaterStorage.CleanWater >= 18f;
            bool foodOk = context.Inventory != null && context.Inventory.CountById("canned_food") >= 12;
            return !alreadyChelating && radHigh && waterOk && foodOk;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.ChelationSystem == null) return;
            context.ChelationSystem.BeginChelation(context.Survivor.Id);
        }
    }
}
