using System;
using System.Collections.Generic;

namespace Ashfall.Core.Onboarding
{
    /// <summary>
    /// Ordered stages of the first-hour onboarding journey. Each stage declares
    /// real, observable completion signals ("sigils") emitted by the live host
    /// when the player performs genuine systems actions. Nothing here fabricates
    /// resources or bypasses gameplay rules.
    /// </summary>
    public enum OnboardingStage
    {
        /// <summary>Resolve all three Opening Protocol directives.</summary>
        Protocol = 0,

        /// <summary>Inspect at least three bunker rooms.</summary>
        Inspect = 1,

        /// <summary>Set a ration policy and open the stores.</summary>
        Rationing = 2,

        /// <summary>Assign at least one survivor to a duty.</summary>
        Assignment = 3,

        /// <summary>Read the weather (forecast, history, or panel).</summary>
        Weather = 4,

        /// <summary>Equip or consume an item from the real inventory.</summary>
        InventoryUse = 5,

        /// <summary>Commit the first day advance to reach Day 2.</summary>
        DayAdvance = 6
    }

    /// <summary>
    /// Assistance intensity for contextual onboarding hints. Persisted with the
    /// journey so a returning player keeps their chosen level.
    /// </summary>
    public enum OnboardingAssistance
    {
        Minimal = 0,   // Objective text only.
        Standard = 1,  // Objective + contextual hints after inactivity/failure.
        Guided = 2     // Steps are auto-highlighted; hints offer "show me where".
    }

    /// <summary>
    /// Result of recording one observed signal against the journey.
    /// </summary>
    public enum OnboardingSignalResult
    {
        /// <summary>The signal is meaningless to the journey (ignored).</summary>
        Ignored = 0,

        /// <summary>Recorded, but the active stage is not yet satisfied.</summary>
        Progressed = 1,

        /// <summary>Recorded and the active stage became satisfied (journey may advance).</summary>
        Advanced = 2
    }

    /// <summary>
    /// A stable ordered (signal name, count) pair. Used in
    /// <see cref="OnboardingSaveState.sigils"/> so the save checksum's
    /// recursive walk produces a deterministic serial form irrespective of the
    /// underlying dictionary/enumeration semantics.
    /// </summary>
    [Serializable]
    public sealed class OnboardingSigilRecord
    {
        public string key = string.Empty;
        public int count;
    }

    /// <summary>
    /// Serialized onboarding progress. Public fields only (SaveChecksum walks
    /// public fields); snake_case for wire-format parity with the other save stores.
    /// </summary>
    [Serializable]
    public sealed class OnboardingSaveState
    {
        public int schemaVersion = OnboardingJourney.SaveVersion;
        public int day = 1;

        /// <summary>
        /// Ordinal-stable signal list — the checksum recursive walk iterates this
        /// list in documented order rather than relying on a Dictionary's
        /// behavioural enumeration semantics.
        /// </summary>
        public List<OnboardingSigilRecord> sigils = new List<OnboardingSigilRecord>();

        public int currentStage = (int)OnboardingStage.Protocol;
        public List<int> completedStages = new List<int>();
        public bool journeyComplete;
        public int assistance = (int)OnboardingAssistance.Standard;

        /// <summary>Dismissed contextual hint keys (persisted so they stay dismissed).</summary>
        public List<string> dismissedHints = new List<string>();

        /// <summary>One-shot "show me where" guidance already offered per stage.</summary>
        public List<int> stagesGuided = new List<int>();
    }
}