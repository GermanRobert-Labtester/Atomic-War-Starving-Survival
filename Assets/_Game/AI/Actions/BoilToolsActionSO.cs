using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that drives a survivor to boil the surgical tools at the
    /// stove (Prompt #169 — SterilizationSystem). Scores when tools are
    /// non-sterile AND the shelter has both clean water and fuel. Calling
    /// the action resets ToolsSterile to true.
    /// </summary>
    [CreateAssetMenu(fileName = "NewBoilToolsAction", menuName = "ASHFALL/AI Actions/Boil Tools")]
    public class BoilToolsActionSO : SurvivorAction
    {
        public BoilToolsActionSO()
        {
            id = "action_boil_tools";
            displayName = "Boil Surgical Tools";
            description = "Boil medical tools at the stove to reset ToolsSterile (prevents Sepsis).";
            basePriority = 0.5f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;
            // High priority — without sterile tools, every surgery risks Sepsis.
            return 0.7f;
        }

        private static bool MeetsPrerequisites(AIContext context)
        {
            if (!HasLivingSurvivor(context) || context.SterilizationSystem == null) return false;
            if (context.SterilizationSystem.ToolsSterile || context.Shelter == null) return false;
            var stove = context.Shelter.GetModule("stove");
            bool stoveReady = stove != null && stove.IsOperational && stove.Fuel > 0f;
            bool waterReady = context.WaterStorage != null && context.WaterStorage.CleanWater >= 2f;
            return stoveReady && waterReady;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.SterilizationSystem == null) return;
            // The action passes the consumeWater/consumeFuel delegates bound to
            // the actual Inventory/Shelter systems. Without those bindings the
            // BoilTools call returns false (insufficient resources), which is
            // acceptable: the action will keep scoring until the resources are
            // available. The host (GameBootstrap) supplies the delegates.
            context.SterilizationSystem.BoilTools(
                consumeWater: (id, n) => n, // host override (see below)
                consumeFuel: n => true);
        }
    }
}
