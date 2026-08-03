using UnityEngine;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Rest in shelter. Fatigue recovery is scaled by environmental SleepQuality
    /// (temperature, atmosphere, diesel noise, bed comfort) — Prompt #32.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_Sleep", menuName = "ASHFALL/AI/Sleep Action")]
    public class SleepActionSO : SurvivorAction
    {
        public SleepActionSO()
        {
            id = "action_sleep";
            displayName = "Sleep";
            description = "Rest in shelter. Quality depends on warmth, air, noise, and a bed.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            return Mathf.Clamp01(context.Survivor.Needs.Fatigue / 100f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return;

            var conditions = ResolveConditions(context);
            var result = SleepQualitySystem.Evaluate(conditions);
            ApplySleepResult(context.Survivor, result);
        }

        /// <summary>
        /// Build sleep conditions from AI context (temp, power noise, bed claim).
        /// Exposed for tests that drive Execute through a real context.
        /// </summary>
        public static SleepConditions ResolveConditions(AIContext context)
        {
            if (context == null)
            {
                return new SleepConditions
                {
                    IndoorTemperatureC = SleepQualitySystem.IdealTempMinC,
                    AirQuality = 100f,
                    HasBed = false,
                    ComfortLevel = 0f
                };
            }

            // Prefer pre-built conditions when tests inject them
            if (context.SleepConditionsOverride.HasValue)
            {
                return context.SleepConditionsOverride.Value;
            }

            float temp = context.IndoorTemperatureC;
            string sleepRoom = string.IsNullOrEmpty(context.SleepRoomId)
                ? SleepQualitySystem.DefaultSleepRoomId
                : context.SleepRoomId;

            return SleepQualitySystem.BuildConditions(
                context.Shelter,
                context.PowerNetwork,
                temp,
                sleepRoom,
                context.AreRoomsAdjacent);
        }

        public static void ApplySleepResult(Survivor survivor, SleepResult result)
        {
            if (survivor == null || !survivor.IsAlive) return;

            survivor.Needs.Fatigue = Mathf.Max(0f, survivor.Needs.Fatigue - result.FatigueRestored);
            survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + result.MoraleDelta, 0f, 100f);
            if (Mathf.Abs(result.HealthDelta) > 0.001f)
            {
                survivor.Needs.Health = Mathf.Clamp(survivor.Needs.Health + result.HealthDelta, 0f, 100f);
            }

            survivor.State = SurvivorState.Resting;
        }
    }
}
