namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// A survivor's characteristic bias in how they interpret radiation risk.
    /// Same world state, different felt danger — this is what makes two survivors
    /// in the same bunker act differently. See BeliefSystem.
    /// </summary>
    public enum RiskBiasTrait
    {
        Paranoid,
        Cautious,
        Realist,
        Reckless,
        Denialist,
        Fatalist,
        /// <summary>Gains/loses morale based on the bunker's average morale.
        /// A fragile barometer for the group.</summary>
        Empath,
        /// <summary>Suffers zero morale loss when another survivor dies.
        /// Terrifies others but makes a perfect cold-blooded scavenger.</summary>
        Sociopath
    }
}
