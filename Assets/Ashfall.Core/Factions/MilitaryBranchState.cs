using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Self-contained day counter for the branching system's own clock. Runs
    /// independently of the global IClock and of Year of Ash's 180-360 day
    /// timeline, mirroring YearOfAshTimelineState's pattern: a local
    /// currentDay field advanced only by this system's own AdvanceDay(int).
    /// </summary>
    [Serializable]
    public class MilitaryBranchTimelineState
    {
        public int currentDay = 0;
    }

    /// <summary>
    /// Faction-internal alignment: a value the FACTION itself holds, separate
    /// from the player's own MoralChoiceSystem score and separate from
    /// FactionWarSystem.standing (which is the player's relationship TO a
    /// faction, not the faction's own moral character). Player choices made
    /// while allied/serving nudge this; it uses the same -200..+200 range and
    /// band semantics as MoralChoiceSystem purely for player-facing
    /// consistency, not because the two scores are the same thing.
    /// </summary>
    [Serializable]
    public class FactionAlignmentRecord
    {
        public string factionId = string.Empty;
        public int alignment = -80; // Military starts evil-leaning per design, swayable by the player.
    }

    /// <summary>
    /// Which base branch (if any) the player has committed to, and whether its
    /// point-of-no-return has fired. A branch is "committed" the first time
    /// SetBranch is called; it never silently changes afterward except
    /// through an explicit, narratively-gated transition (not modeled in this
    /// slice — Military-only branches do not yet cross into Rebel/Independent
    /// territory).
    /// </summary>
    [Serializable]
    public class MilitaryBranchRecord
    {
        public string branchId = string.Empty;
        public bool committed = false;
        public bool ponrLocked = false;
        public int ponrLockedDay = -1;
        public string resolvedEndingId = string.Empty;
    }

    [Serializable]
    public class MilitaryBranchSystemState
    {
        public string systemId = MilitaryBranchSystem.SystemId;
        public int schemaVersion = 1;

        public MilitaryBranchTimelineState timeline = new MilitaryBranchTimelineState();
        public MilitaryBranchRecord branch = new MilitaryBranchRecord();
        public FactionAlignmentRecord militaryAlignment = new FactionAlignmentRecord
        {
            factionId = MilitaryBranchIds.FactionId,
            alignment = -80
        };

        /// <summary>Flags set by branch/PoNR events. Distinct from the runtime IFlagLedger
        /// (which is not persisted) — this list is the save-durable record.</summary>
        public List<string> setFlags = new List<string>();
    }
}
