using System;
#pragma warning disable CS8618

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Player-relationship standing toward any faction that is not the
    /// player's own committed faction (i.e. not tracked via a branch
    /// system's own BranchRecord). Deliberately the same shape as
    /// FactionWarSystem.FactionStandingRecord (-100..+100, clamp-on-write,
    /// hostile/allied derived at +/-50) so the numeric contract feels
    /// identical to every other player-vs-faction relationship in the game.
    /// Used for PRPF (no branch system owns it) and, by the Independent
    /// branch slice, for the player's standing toward Military and Rebel
    /// when the player never committed to either — a value neither
    /// MilitaryBranchSystem nor RebelBranchSystem tracks for themselves,
    /// since they only care about a player who DID commit.
    /// </summary>
    [Serializable]
    public class PlayerFactionStandingRecord
    {
        public string factionId = string.Empty;
        public int standing = 0; // -100 (kill-on-sight) to +100 (allied)
        public bool isHostile = false;
        public bool isAllied = false;
    }

    [Serializable]
    public class PrpfSystemState
    {
        public string systemId = PrpfStandingSystem.SystemId;
        public int schemaVersion = 1;

        public PlayerFactionStandingRecord standing = new PlayerFactionStandingRecord { factionId = PrpfIds.FactionId };

        /// <summary>PRPF's OWN internal alignment — distinct from the player's
        /// MoralChoiceSystem score and from standing.standing (the player's
        /// relationship TO PRPF). PRPF is a positive-leaning faction by
        /// design; player choices while allied can shift it further, same
        /// -200..+200 axis as Military/Rebel's FactionAlignmentRecord for
        /// player-facing scale consistency.</summary>
        public FactionAlignmentRecord alignment = new FactionAlignmentRecord
        {
            factionId = PrpfIds.FactionId,
            alignment = 120
        };

        public bool joined = false;
        public bool opposed = false;

        /// <summary>Last campaign day that applied daily influence (−1 = never).
        /// Idempotent TickDay guard so day-advance retries do not double-drift.</summary>
        public int lastTickedDay = -1;
    }
}
