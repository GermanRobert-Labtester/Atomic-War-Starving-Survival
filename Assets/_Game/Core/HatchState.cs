namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Physical state of the bunker hatch (Prompt #48 — weather-driven entrapment).
    /// Extreme weather does not only change numbers; it seals the only exit.
    /// </summary>
    public enum HatchState
    {
        /// <summary>Hatch operable. Expeditions may leave.</summary>
        Clear = 0,

        /// <summary>Blizzard snowed the hatch shut. DigOut required from inside.</summary>
        Buried = 1,

        /// <summary>Fallout storm ash/ice seal. Same hard lock as Buried.</summary>
        Frozen = 2
    }
}
