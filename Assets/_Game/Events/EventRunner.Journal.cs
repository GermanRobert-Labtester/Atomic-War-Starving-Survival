using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    public partial class EventRunner
    {
        public int ObserveDiscoveries(JournalSystem journal, EventContext context)
        {
            if (journal == null || context == null) return 0;
            int added = 0;

            // high_co2 — Atmosphere system (#20) foul air / diesel CO
            float air = context.Shelter != null ? context.Shelter.AirQuality : 100f;
            bool foulAir = air <= SleepQualitySystem.HighCo2AirQualityThreshold
                || context.CarbonMonoxidePpm >= SleepQualitySystem.HighCo2PpmThreshold;
            if (foulAir && TryRecordDiscovery(journal, KnowledgeKeys.HighCo2, context) != null)
                added++;

            // has_seen_radiation — first meaningful dose
            var doseAuthor = PickAuthor(context);
            if (doseAuthor != null && doseAuthor.RadiationDose >= 5f
                && TryRecordDiscovery(journal, KnowledgeKeys.HasSeenRadiation, context, doseAuthor) != null)
                added++;

            // has_experienced_storm
            if (context.IsFalloutStorm
                && TryRecordDiscovery(journal, KnowledgeKeys.HasExperiencedStorm, context) != null)
                added++;

            // filter_failing — air filtration degrading but not yet full high_co2
            if (IsFilterFailing(context)
                && TryRecordDiscovery(journal, KnowledgeKeys.FilterFailing, context) != null)
                added++;

            // freezing_shelter
            if (context.IndoorTemperatureC <= SleepQualitySystem.FreezingTempC + 0.001f
                && TryRecordDiscovery(journal, KnowledgeKeys.FreezingShelter, context) != null)
                added++;

            return added;
        }
        private static bool IsFilterFailing(EventContext context)
        {
            if (context?.Shelter == null) return false;
            var airMod = context.Shelter.GetModule("air_filtration");
            if (airMod == null || !airMod.IsOperational) return false;
            if (airMod.FilterHealth <= 0f || airMod.FilterHealth > 40f) return false;
            return airMod.FilterHealth > SleepQualitySystem.HighCo2AirQualityThreshold;
        }
        public JournalEntry TryRecordDiscovery(
            JournalSystem journal,
            string knowledgeKey,
            EventContext context,
            Survivor authorOverride = null)
        {
            if (journal == null || context == null || string.IsNullOrEmpty(knowledgeKey))
                return null;
            var author = authorOverride ?? PickAuthor(context);
            return journal.TryDiscover(
                knowledgeKey,
                author,
                context.CurrentDay,
                context.CurrentHour);
        }
        public static Survivor PickAuthor(EventContext context)
        {
            if (context == null) return null;
            if (context.PrimarySurvivor != null && context.PrimarySurvivor.IsAlive)
                return context.PrimarySurvivor;
            if (context.AllSurvivors == null) return null;
            for (int i = 0; i < context.AllSurvivors.Count; i++)
            {
                var s = context.AllSurvivors[i];
                if (s != null && s.IsAlive) return s;
            }
            return context.PrimarySurvivor;
        }
    }
}
