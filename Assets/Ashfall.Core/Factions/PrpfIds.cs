using System;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Canonical ids for the hidden third-power faction, Peace Reputation
    /// Protection Forces (PRPF). Standing, alignment, join, and oppose are
    /// live via <see cref="PrpfStandingSystem"/> (wired through
    /// <c>FactionBranchCoordinator</c>). Autonomous power-growth, the
    /// chance-based hidden-recruitment encounter roll, and concealed HQ
    /// location remain follow-on work on top of this id set.
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
