namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Prompt #320: Physical condition modifier for map locations.
    /// Applied before NPC spawning during location procedural generation.
    /// </summary>
    public enum LocationStateModifier
    {
        Pristine,
        Looted,
        HalfBurned,
        Exploded,
        Abandoned
    }
}
