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

            // #249 Fierce Mother: cancel Sleep when a child need is critical.
            if (context.PersonalQuests != null && context.GetSurvivors != null
                && context.PersonalQuests.ShouldCancelEatOrSleepForChild(
                    context.Survivor, context.GetSurvivors()))
                return 0f;

            // #250 Workaholic: ignore rest/sleep until fatigue is near collapse.
            if (context.PersonalQuests != null
                && context.PersonalQuests.ShouldIgnoreRestAction(context.Survivor))
                return 0f;

            // #257 Tactician: refuses floor sleep — only beds.
            if (context.PersonalQuests != null
                && context.PersonalQuests.WillRefuseFloorSleep(context.Survivor))
            {
                var bedCheck = ResolveConditions(context);
                if (!bedCheck.HasBed)
                    return 0f;
            }

            return Mathf.Clamp01(context.Survivor.Needs.Fatigue / 100f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return;

            if (context.PersonalQuests != null && context.GetSurvivors != null
                && context.PersonalQuests.ShouldCancelEatOrSleepForChild(
                    context.Survivor, context.GetSurvivors()))
                return;

            var conditions = ResolveConditions(context);

            // #257 Tactician forced onto the floor: massive fatigue penalty, no rest.
            if (context.PersonalQuests != null
                && context.PersonalQuests.RequiresBedModuleToSleep(context.Survivor)
                && !conditions.HasBed)
            {
                float pen = context.PersonalQuests.GetFloorSleepFatiguePenaltyPerHour(
                    context.Survivor, hasBedModule: false);
                context.Survivor.Needs.Fatigue = Mathf.Min(
                    100f, context.Survivor.Needs.Fatigue + pen);
                context.Survivor.State = SurvivorState.Resting;
                return;
            }

            var result = SleepQualitySystem.Evaluate(conditions);
            // #250 Workaholic: sleep restores fatigue at half speed.
            float restoreMult = context.PersonalQuests != null
                ? context.PersonalQuests.GetSleepFatigueRestoreMultiplier(context.Survivor)
                : 1f;
            if (!Mathf.Approximately(restoreMult, 1f))
                result.FatigueRestored *= restoreMult;

            // #261 Night Terrors: roommates of a Child Soldier get disrupted sleep.
            if (context.PersonalQuests != null && context.GetSurvivors != null)
            {
                var all = context.GetSurvivors();
                if (all != null)
                {
                    for (int i = 0; i < all.Count; i++)
                    {
                        var source = all[i];
                        if (source == null || !source.IsAlive) continue;
                        float disrupt = context.PersonalQuests.GetNightTerrorSleepDisruption(
                            source, context.Survivor);
                        if (disrupt > 0f)
                            result.FatigueRestored = Mathf.Max(0f, result.FatigueRestored - disrupt);
                    }
                }
            }

            ApplySleepResult(context.Survivor, result);
            // #268 Restless: fatigue never recovers past 80% cap.
            if (context.PersonalQuests != null)
            {
                float cap = context.PersonalQuests.GetMaxFatigueCap(context.Survivor);
                if (context.Survivor.Needs.Fatigue > cap)
                    context.Survivor.Needs.Fatigue = cap;
            }
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

            // #307 Hermit/Uncivilized: refuses crowded rooms, sleeps in the airlock instead.
            if (context.PersonalQuests != null && context.GetSurvivors != null)
            {
                var list = context.GetSurvivors();
                if (list != null)
                {
                    int pop = 0;
                    for (int i = 0; i < list.Count; i++)
                        if (list[i] != null && list[i].IsAlive) pop++;
                    string preferred = context.PersonalQuests.GetPreferredSleepRoomWhenCrowded(
                        context.Survivor, pop);
                    if (!string.IsNullOrEmpty(preferred))
                        sleepRoom = preferred;
                }
            }

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
