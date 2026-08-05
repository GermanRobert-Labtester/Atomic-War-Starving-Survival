using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action that builds the overworld wind turbine (Prompt #171).
    /// One-shot build; once built, the turbine provides free power but
    /// increases hatch visibility (more raids). Scores when the bunker
    /// lacks power generation capacity and the wind is steady.
    /// </summary>
    [CreateAssetMenu(fileName = "NewBuildWindTurbineAction", menuName = "ASHFALL/AI Actions/Build Wind Turbine")]
    public class BuildWindTurbineActionSO : SurvivorAction
    {
        public BuildWindTurbineActionSO()
        {
            id = "action_build_wind_turbine";
            displayName = "Build Wind Turbine";
            description = "Construct the overworld wind turbine. Free power, +hatch visibility (more raids).";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (!MeetsPrerequisites(context)) return 0f;

            float gen = Mathf.Max(0.0001f, context.PowerNetwork.TotalGeneration);
            float draw = context.PowerNetwork.TotalDraw;
            float loadRatio = draw / gen;
            return Mathf.Clamp01(0.5f + 0.3f * (loadRatio - 0.85f) / 0.15f);
        }

        private static bool MeetsPrerequisites(AIContext context)
        {
            if (!CanCraft(context) || context.WindTurbineSystem == null) return false;
            if (context.WindTurbineSystem.IsBuilt || context.PowerNetwork == null) return false;
            // When TotalDraw approaches TotalGeneration, the network is at
            // capacity. We use the ratio > 0.85 as the trigger.
            float gen = Mathf.Max(0.0001f, context.PowerNetwork.TotalGeneration);
            return context.PowerNetwork.TotalDraw >= 0.85f * gen;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.WindTurbineSystem == null) return;
            context.WindTurbineSystem.Build();
        }
    }
}
