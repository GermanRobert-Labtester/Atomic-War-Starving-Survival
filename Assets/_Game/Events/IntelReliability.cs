namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Reliability rating for an intel source. The radio airwaves are full of
    /// desperate liars — an "Unverified" broadcast is just what was said, a
    /// "Verified" broadcast has been corroborated (multiple frequencies, a
    /// second-source confirmation, or a survivor actually walked the route),
    /// and a "Trap" is a broadcast engineered to lure a response (a pre-
    /// positioned ambush, a recorded loop, a poisoned cache).
    ///
    /// EventRunner uses the active value on <see cref="EventContext"/> to
    /// gate radio-driven choices: an Unverified broadcast should never
    /// unlock a low-cost "trust the source" path, and a Trap should bias
    /// outcomes toward hazard/ambush encounters.
    /// </summary>
    public enum IntelReliability
    {
        /// <summary>Single source, not yet corroborated. Default for any new
        /// radio broadcast that the player has not yet scrutinized.</summary>
        Unverified = 0,

        /// <summary>At least two independent sources agree, or a survivor has
        /// actually visited the location and confirmed it.</summary>
        Verified = 1,

        /// <summary>The broadcast is engineered to lure a response. The
        /// "scrubber hum" is a recorded loop; the "supply cache" is bait;
        /// the "survivor signal" is a faction ambush. Any expedition
        /// launched on this intel should bias toward hazard/ambush.</summary>
        Trap = 2,
    }
}
