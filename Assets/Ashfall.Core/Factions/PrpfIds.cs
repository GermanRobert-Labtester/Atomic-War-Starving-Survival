using System;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Canonical ids for the hidden third-power faction, Peace Reputation
    /// Protection Forces (PRPF). This file owns only the stub surface this
    /// task builds: a player-relationship standing track and PRPF's own
    /// internal alignment. PRPF's autonomous power-growth curve, the
    /// chance-based hidden-recruitment encounter roll, and its concealed HQ
    /// location are explicitly out of scope here — later tasks build those
    /// on top of this id set and PrpfStandingSystem/PrpfAlignmentRecord.
    /// </summary>
    public static class PrpfIds
    {
        public const string FactionId = "faction_prpf";

        /// <summary>Set once the player has formally joined PRPF. Distinct from
        /// merely having high standing — joining is a deliberate, gated act
        /// (requires the player's own morality band to clear
        /// JoinMinPlayerMoralBand), not an automatic threshold crossing.</summary>
        public const string FlagJoined = "flag_prpf_joined";

        /// <summary>Set once the player has committed to actively opposing PRPF
        /// (the Year 2 confrontation branch), as opposed to simply never
        /// having joined.</summary>
        public const string FlagOpposed = "flag_prpf_opposed";
    }
}
