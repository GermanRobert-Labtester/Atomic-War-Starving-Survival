using UnityEngine;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Utility AI: survivor pedals the bicycle generator when the grid is short
    /// on fuel/generation — keeps critical loads alive without diesel.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_PedalGenerator", menuName = "ASHFALL/AI/Pedal Generator Action")]
    public class PedalGeneratorActionSO : SurvivorAction
    {
        public string BicycleSourceId = "bicycle_generator";

        public PedalGeneratorActionSO()
        {
            id = "action_pedal_generator";
            displayName = "Pedal Generator";
            description = "Manually generate electricity when fuel runs out.";
            basePriority = 0.35f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            var grid = context.PowerNetwork;
            if (grid == null) return 0f;

            // Too exhausted to pedal productively.
            if (context.Survivor.Needs != null && context.Survivor.Needs.Fatigue >= 85f)
                return 0f;

            var bike = grid.GetSource(BicycleSourceId) ?? grid.FindAvailableBicycle();
            if (bike == null || !bike.IsEnabled) return 0f;

            // Someone else already on the pedals (and it isn't us).
            if (bike.HasPedaler && bike.PedalingSurvivorId != context.Survivor.Id)
                return 0f;

            // Already pedaling — keep going while the grid still needs us.
            bool alreadyPedaling = bike.HasPedaler && bike.PedalingSurvivorId == context.Survivor.Id;

            float dieselFuel = grid.GetDieselFuelTotal();
            bool fuelScarce = dieselFuel < 5f;
            bool blackoutRisk = grid.IsBlackout || grid.IsLoadShedding || grid.RequestedDraw > grid.TotalGeneration + 0.01f;
            bool genTight = grid.TotalGeneration < grid.RequestedDraw || grid.TotalGeneration < 30f;

            if (!fuelScarce && !blackoutRisk && !genTight && !alreadyPedaling)
                return 0.02f;

            float urgency = 0f;
            if (grid.IsBlackout) urgency += 0.55f;
            else if (grid.IsLoadShedding) urgency += 0.4f;
            if (fuelScarce) urgency += 0.25f;
            if (genTight) urgency += 0.15f;
            if (alreadyPedaling) urgency += 0.1f;

            // Prefer fresher riders.
            float fatigue = context.Survivor.Needs != null ? context.Survivor.Needs.Fatigue : 50f;
            float staminaFactor = 1f - Mathf.Clamp01(fatigue / 100f);
            return Mathf.Clamp01(urgency * (0.5f + 0.5f * staminaFactor));
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.PowerNetwork == null) return;
            var grid = context.PowerNetwork;
            var bike = grid.GetSource(BicycleSourceId) ?? grid.FindAvailableBicycle();
            if (bike == null) return;

            // Claim the pedals; Tick will drain fatigue and produce watts.
            if (!bike.HasPedaler || bike.PedalingSurvivorId == context.Survivor.Id)
            {
                grid.AssignPedaler(bike.SourceId, context.Survivor.Id);
            }
        }
    }
}
