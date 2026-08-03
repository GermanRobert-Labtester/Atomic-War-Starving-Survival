namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Top-level campaign phase. PreWar is a pre-Day-1 placeholder (the campaign
    /// always starts in CivilWar); Flashpoint is the single day the atomic exchange
    /// happens; NuclearWinter is every day after. Ordinal order matters: gating
    /// checks (e.g. LootEntry.phaseRequirement) compare via &gt;=.
    /// </summary>
    public enum WorldPhase
    {
        PreWar,
        CivilWar,
        Flashpoint,
        NuclearWinter
    }
}
