using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Self-contained day counter for the Rebel branch system's own clock.
    /// Mirrors MilitaryBranchTimelineState: independent of the global IClock
    /// and of Year of Ash's 180-360 day timeline. Note this is a SEPARATE
    /// counter from MilitaryBranchTimelineState — a player can only ever be
    /// on one of Military/Rebel in a given playthrough, but the two systems
    /// do not share a clock instance, so each stays self-contained and
    /// trivially composable if a future host session wires both.
    /// </summary>
    [Serializable]
    public class RebelBranchTimelineState
    {
        public int currentDay = 0;
    }

    /// <summary>
    /// Which base Rebel branch (if any) the player has committed to, and
    /// whether its point-of-no-return has fired. Mirrors
    /// MilitaryBranchRecord exactly.
    /// </summary>
    [Serializable]
    public class RebelBranchRecord
    {
        public string branchId = string.Empty;
        public bool committed = false;
        public bool ponrLocked = false;
        public int ponrLockedDay = -1;
        public string resolvedEndingId = string.Empty;
    }

    [Serializable]
    public class RebelBranchSystemState
    {
        public string systemId = RebelBranchSystem.SystemId;
        public int schemaVersion = 1;

        public RebelBranchTimelineState timeline = new RebelBranchTimelineState();
        public RebelBranchRecord branch = new RebelBranchRecord();

        /// <summary>Rebel's own internal alignment, distinct from the player's
        /// MoralChoiceSystem score and from FactionWarSystem.standing (player
        /// relationship, not faction-internal morality). Rebels start
        /// evil-leaning per design, same as Military, and are equally
        /// swayable by the player.</summary>
        public FactionAlignmentRecord rebelAlignment = new FactionAlignmentRecord
        {
            factionId = RebelBranchIds.FactionId,
            alignment = -80
        };

        /// <summary>Flags set by branch/PoNR events. Distinct from the runtime IFlagLedger
        /// (which is not persisted) — this list is the save-durable record.</summary>
        public List<string> setFlags = new List<string>();
    }
}
