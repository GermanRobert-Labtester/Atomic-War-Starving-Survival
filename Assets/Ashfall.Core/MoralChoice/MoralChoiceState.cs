using System;
using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Save DTO for the moral choice system ("The Weight of Survival",
    /// docs/MORAL_CHOICE_SYSTEM.md). Journal resolutions, seeded outcome
    /// rolls, and gossip propagation schedules all live here so a save
    /// replays identically — there is no second file format.
    /// </summary>
    [Serializable]
    public sealed class MoralChoiceState
    {
        public string systemId = MoralChoiceSystem.SystemId;
        public int schemaVersion = 1;
        public int moralScore;
        public int empathyPoints;
        public List<MoralChoiceResolution> resolutions = new List<MoralChoiceResolution>();
        public int lastReconciledDay = -1;

        /// <summary>Ordinal of MoralPathBand at the last reconcile; -1 = never reconciled.</summary>
        public int bandAtLastReconcile = -1;

        /// <summary>One-time threshold/legend events already fired, by id.</summary>
        public List<string> firedThresholdEvents = new List<string>();
    }

    /// <summary>
    /// One resolved moral quest: the journal line, the ledger entry, and the
    /// seeded rolls drawn at resolution time. Immutable after creation.
    /// </summary>
    [Serializable]
    public sealed class MoralChoiceResolution
    {
        public string questId = string.Empty;
        public string locationId = string.Empty;
        public int resolvedDay = -1;
        public int choiceIndex = -1;

        /// <summary>Raw design delta of the chosen option (pre-clamp); the journal arrow shows its sign.</summary>
        public int moralDelta;

        public int empathyDelta;

        /// <summary>up | down | flat — never the number, only the direction.</summary>
        public string impactMark = "flat";

        /// <summary>0-99, rolled once at resolution and stored for deterministic outcome branches.</summary>
        public int outcomeRoll = -1;

        /// <summary>Gossip leaves the witnessing circle on this day (resolvedDay + 1..3).</summary>
        public int propagatesOnDay = -1;

        public string epitaph = string.Empty;
    }
}
