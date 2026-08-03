namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// How a power source produces watts on the shelter grid.
    /// </summary>
    public enum PowerSourceKind
    {
        /// <summary>Burns diesel/fuel; high output; emits CO (Prompt #20 hook).</summary>
        Diesel = 0,
        /// <summary>Survivor pedals; drains Fatigue/Hunger; low output.</summary>
        Bicycle = 1,
        /// <summary>Produces only under windy weather (Clear/Overcast excluded).</summary>
        Wind = 2,
        /// <summary>Produces only under clear daylight weather.</summary>
        Solar = 3
    }
}
