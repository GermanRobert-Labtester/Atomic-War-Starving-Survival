using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Self-contained day counter for the Independent branch system's own
    /// clock. Mirrors MilitaryBranchTimelineState/RebelBranchTimelineState:
    /// independent of the global IClock and of every other expansion's day
    /// range, including Military's and Rebel's own local clocks.
    /// </summary>
    [Serializable]
    public class IndependentBranchTimelineState
    {
        public int currentDay = 0;
    }

    /// <summary>
    /// Which base Independent branch (if any) the player has committed to,
    /// and whether its point-of-no-return has fired. Same shape as
    /// MilitaryBranchRecord/RebelBranchRecord — the branch commitment
    /// lifecycle itself IS a mirror, even though the gating logic around it
    /// (see IndependentBranchSystem.CommitBranch) is not.
    /// </summary>
    [Serializable]
    public class IndependentBranchRecord
    {
        public string branchId = string.Empty;
        public bool committed = false;
        public bool ponrLocked = false;
        public int ponrLockedDay = -1;
        public string resolvedEndingId = string.Empty;
    }

    [Serializable]
    public class IndependentBranchSystemState
    {
        public string systemId = IndependentBranchSystem.SystemId;
        public int schemaVersion = 1;

        public IndependentBranchTimelineState timeline = new IndependentBranchTimelineState();
        public IndependentBranchRecord branch = new IndependentBranchRecord();

        /// <summary>
        /// The player's own standing toward Military and Rebel. Deliberately
        /// owned HERE, not by MilitaryBranchSystem/RebelBranchSystem — those
        /// two systems only model the player once committed to them
        /// (branch/PoNR/alignment), and have no concept of "how Military
        /// feels about a player who never joined." An Independent player's
        /// relationship to both factions is exactly that: standing without
        /// commitment. Same PlayerFactionStandingRecord shape PRPF uses.
        /// </summary>
        public PlayerFactionStandingRecord militaryStanding = new PlayerFactionStandingRecord
        {
            factionId = MilitaryBranchIds.FactionId
        };

        public PlayerFactionStandingRecord rebelStanding = new PlayerFactionStandingRecord
        {
            factionId = RebelBranchIds.FactionId
        };

        /// <summary>Flags set by branch/PoNR events. Distinct from the runtime IFlagLedger
        /// (which is not persisted) — this list is the save-durable record.</summary>
        public List<string> setFlags = new List<string>();
    }
}
