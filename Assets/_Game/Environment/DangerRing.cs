namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Proc-gen danger zoning for wasteland map nodes (Prompt #23).
    /// Rings radiate outward from the shelter.
    /// </summary>
    public enum DangerRing
    {
        /// <summary>Not on a danger ring (the shelter hub).</summary>
        Shelter = -1,
        /// <summary>Close: looted suburbs, low rads.</summary>
        Suburbs = 0,
        /// <summary>Mid: gang-controlled city outskirts, medium rads.</summary>
        CityOutskirts = 1,
        /// <summary>Far: military loot at ground zero, extreme rads.</summary>
        GroundZero = 2
    }
}
