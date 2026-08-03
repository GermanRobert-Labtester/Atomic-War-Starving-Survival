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
        Fatalist
    }
}
