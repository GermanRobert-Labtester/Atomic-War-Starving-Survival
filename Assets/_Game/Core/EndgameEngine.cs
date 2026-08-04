using System;
using System.Collections.Generic;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Event data raised when campaign reaches a victory or defeat endgame condition (Prompt #41).
    /// </summary>
    public struct CampaignEndedEvent
    {
        public CampaignResult Result;
        public EndgameConditionKind ConditionKind;
        public bool IsVictory;
        public int DaysSurvived;
    }

    /// <summary>
    /// Campaign victory and defeat condition evaluation engine (Prompt #41).
    /// Evaluates survivor states, bunker module health, radio extraction, and self-sufficiency.
    /// </summary>
    public class EndgameEngine
    {
        public CampaignResult Result { get; private set; }

        public event Action<CampaignEndedEvent> OnCampaignEnded;

        public EndgameEngine(GameModeKind mode = GameModeKind.Story, int targetDurationDays = 120)
        {
            Result = new CampaignResult
            {
                Mode = mode,
                TargetDurationDays = CampaignResult.GetDefaultDurationForMode(mode, targetDurationDays),
                StartCalendarDate = new DateTime(2026, 8, 25)
            };
        }

        /// <summary>
        /// Evaluate all victory and defeat conditions for the current game day state.
        /// Returns true if a terminal endgame condition was triggered.
        /// </summary>
        public bool Evaluate(
            int currentDay,
            IReadOnlyList<Survivor> survivors,
            Shelter.Shelter shelter,
            bool isExtractionUnlocked,
            bool isHydroponicsOperational,
            int totalDeathsRecorded)
        {
            if (Result.IsTerminal) return true;

            Result.DaysSurvived = Math.Max(1, currentDay);

            // 1. Defeat: All survivors deceased
            if (survivors != null && survivors.Count > 0 && AllDead(survivors))
            {
                return TriggerEndgame(
                    EndgameConditionKind.AllSurvivorsDeceased,
                    isVictory: false,
                    summary: "All survivors perished in the bunker.");
            }

            // 2. Defeat: Bunker structural collapse (air filter & radiation shielding both at 0% health)
            if (shelter != null && IsBunkerStructurallyCollapsed(shelter))
            {
                return TriggerEndgame(
                    EndgameConditionKind.BunkerStructuralCollapse,
                    isVictory: false,
                    summary: "Bunker air filtration and radiation shielding completely failed.");
            }

            // 3. Victory: Rescue extraction success (Day >= 60 with active military channel / extraction unlocked).
            // Skipped if LifeboatPartialExtraction already terminal (mutual exclusion, Prompt #20).
            if (isExtractionUnlocked && currentDay >= 60)
            {
                return TriggerEndgame(
                    EndgameConditionKind.RescueExtractionSuccess,
                    isVictory: true,
                    summary: "Military extraction completed successfully after broadcast contact.");
            }

            // 4. Victory: Long-term self-sufficiency (Day >= 100 with functioning hydroponics and 0 deaths)
            if (currentDay >= 100 && isHydroponicsOperational && totalDeathsRecorded == 0)
            {
                return TriggerEndgame(
                    EndgameConditionKind.LongTermSelfSufficiency,
                    isVictory: true,
                    summary: "Bunker achieved 100 days of self-sufficient survival with zero losses.");
            }

            return false;
        }

        /// <summary>
        /// Prompt #20 — resolve the Lifeboat Transmission: one extracted, rest condemned.
        /// Victory is true but the summary is bittersweet. Blocks full RescueExtractionSuccess
        /// for the same run via <see cref="CampaignResult.IsTerminal"/>.
        /// </summary>
        public bool ApplyLifeboatPartialExtraction(
            int day,
            string extractedName,
            int leftBehindCount)
        {
            if (Result.IsTerminal) return false;
            Result.DaysSurvived = Math.Max(1, day);
            string who = string.IsNullOrEmpty(extractedName) ? "One of us" : extractedName;
            string summary = leftBehindCount <= 0
                ? $"{who} took the only seat. The channel went quiet."
                : $"{who} took the only seat. {leftBehindCount} stayed under concrete.";
            return TriggerEndgame(
                EndgameConditionKind.LifeboatPartialExtraction,
                isVictory: true,
                summary: summary);
        }

        private bool TriggerEndgame(EndgameConditionKind condition, bool isVictory, string summary)
        {
            Result.IsVictory = isVictory;
            Result.IsDefeat = !isVictory;
            Result.ConditionKind = condition;
            Result.OutcomeSummary = summary;

            var evt = new CampaignEndedEvent
            {
                Result = Result,
                ConditionKind = condition,
                IsVictory = isVictory,
                DaysSurvived = Result.DaysSurvived
            };

            OnCampaignEnded?.Invoke(evt);
            EventBus.Raise(evt);
            return true;
        }

        public static bool AllDead(IReadOnlyList<Survivor> survivors)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive) return false;
            }
            return true;
        }

        public static bool IsBunkerStructurallyCollapsed(Shelter.Shelter shelter)
        {
            if (shelter == null) return false;
            var filter = shelter.GetModule("air_filtration");
            var shielding = shelter.GetModule("radiation_shielding");

            if (filter == null || shielding == null) return false;
            return filter.FilterHealth <= 0f && shielding.FilterHealth <= 0f;
        }
    }
}
