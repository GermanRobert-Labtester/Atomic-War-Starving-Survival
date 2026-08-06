using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class EpilogueStatsState
    {
        public string systemId = "system_epilogue_stats";
    }

    [Serializable]
    public class EpilogueRecord
    {
        public int mealsCooked;
        public int bulletsFired;
        public int daysSurvived;
        public List<string> survivorsDead = new List<string>();
        public List<string> deathLocations = new List<string>();
        public List<string> journalEntries = new List<string>();
    }

    /// <summary>
    /// Prompt #768: The Empty Bunker (Epilogue).
    /// On game end, generate top-down view of ruined bunker.
    /// Highlight where people died, meals cooked, bullets fired, final journal entries.
    /// </summary>
    public class System_EpilogueStats
    {
        private EpilogueStatsState _state = new EpilogueStatsState();
        private EpilogueRecord _lastRecord;

        public event Action<EpilogueRecord> OnEpilogueGenerated;

        public EpilogueStatsState State => _state;
        public EpilogueRecord LastRecord => _lastRecord;

        /// <summary>
        /// Generate the epilogue record from end-of-game statistics.
        /// </summary>
        public EpilogueRecord GenerateEpilogue(
            int mealsCooked,
            int bulletsFired,
            int daysSurvived,
            List<string> deadSurvivors,
            List<string> deathRoomIds,
            List<string> finalJournalEntries)
        {
            var record = new EpilogueRecord
            {
                mealsCooked = Math.Max(0, mealsCooked),
                bulletsFired = Math.Max(0, bulletsFired),
                daysSurvived = Math.Max(0, daysSurvived),
                survivorsDead = deadSurvivors != null
                    ? new List<string>(deadSurvivors)
                    : new List<string>(),
                deathLocations = deathRoomIds != null
                    ? new List<string>(deathRoomIds)
                    : new List<string>(),
                journalEntries = finalJournalEntries != null
                    ? new List<string>(finalJournalEntries)
                    : new List<string>()
            };

            _lastRecord = record;
            OnEpilogueGenerated?.Invoke(record);
            return record;
        }

        /// <summary>
        /// Build a haunting narrative summary from the epilogue record.
        /// </summary>
        public string GetNarrativeSummary(EpilogueRecord record)
        {
            if (record == null) return "The bunker is empty. No one remembers.";

            int dead = record.survivorsDead != null ? record.survivorsDead.Count : 0;
            int meals = record.mealsCooked;
            int bullets = record.bulletsFired;
            int days = record.daysSurvived;

            string dayWord = days == 1 ? "day" : "days";
            string mealWord = meals == 1 ? "meal" : "meals";
            string bulletWord = bullets == 1 ? "bullet" : "bullets";

            var sb = new System.Text.StringBuilder();
            sb.Append($"They lasted {days} {dayWord}. ");
            sb.Append($"{meals} {mealWord} cooked over charcoal. ");
            sb.Append($"{bullets} {bulletWord} fired into the dark.");

            if (dead > 0)
            {
                sb.Append(" ");
                if (dead == 1)
                {
                    string name = !string.IsNullOrEmpty(record.survivorsDead[0])
                        ? record.survivorsDead[0]
                        : "someone";
                    string room = (record.deathLocations != null && record.deathLocations.Count > 0
                                   && !string.IsNullOrEmpty(record.deathLocations[0]))
                        ? $" in {record.deathLocations[0]}"
                        : "";
                    sb.Append($"{name} died{room}. No one was left to bury them.");
                }
                else
                {
                    sb.Append($"{dead} people died under concrete. ");
                    sb.Append("The rooms remember what the survivors could not.");
                }
            }

            if (record.journalEntries != null && record.journalEntries.Count > 0)
            {
                sb.Append(" The last journal entry read: ");
                sb.Append('"');
                sb.Append(record.journalEntries[record.journalEntries.Count - 1]);
                sb.Append('"');
            }

            sb.Append(" The bunker is empty now. The wind writes the rest.");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the room IDs where survivors died, for top-down highlight rendering.
        /// </summary>
        public List<string> GetTopDownHighlights(EpilogueRecord record)
        {
            if (record == null || record.deathLocations == null)
                return new List<string>();

            return new List<string>(record.deathLocations);
        }
    }
}
