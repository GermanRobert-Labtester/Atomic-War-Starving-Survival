using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that drives a returning scavenger to enter the bunker
    /// through the airlock properly: decontaminate first, then open the
    /// inner door (Prompt #128 — AirlockSystem). Scores when the airlock
    /// is built AND a scavenger is currently inside.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDeconAndEnterAction", menuName = "ASHFALL/AI Actions/Decon and Enter")]
    public class DeconAndEnterActionSO : SurvivorAction
    {
        public DeconAndEnterActionSO()
        {
            id = "action_decon_and_enter";
            displayName = "Decontaminate and Enter";
            description = "Scavenger decontaminates in the airlock and enters the bunker without spreading rads.";
            basePriority = 0.7f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;
            // High priority — every minute in a contaminated airlock adds dose.
            return 0.75f;
        }

        private static bool MeetsPrerequisites(AIContext context)
        {
            if (!HasLivingSurvivor(context) || context.AirlockSystem == null) return false;
            var airlock = context.AirlockSystem;
            if (!airlock.Exists || !airlock.ScavengerInAirlock) return false;
            // Need decon work or a sealed inner door to justify the action.
            return airlock.Contamination > 0f || airlock.InnerDoorSealed;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.AirlockSystem == null) return;
            context.AirlockSystem.DeconAndEnter(context.Survivor);
        }
    }
}
