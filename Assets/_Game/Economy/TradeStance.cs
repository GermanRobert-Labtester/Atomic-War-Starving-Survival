namespace AtomicWar._Game.Economy
{
    /// <summary>How a faction currently treats the player given TrustLevel.</summary>
    public enum TradeStance
    {
        /// <summary>Trust at/below raid threshold — hatch assault risk.</summary>
        HostileRaid,
        /// <summary>Will not fair-trade; may rob offered goods.</summary>
        Rob,
        /// <summary>Refuses to open a stall (between rob and trade floors).</summary>
        Refuse,
        /// <summary>Normal barter available.</summary>
        Trade,
        /// <summary>Barter + willing to share intel tips.</summary>
        ShareIntel
    }
}
