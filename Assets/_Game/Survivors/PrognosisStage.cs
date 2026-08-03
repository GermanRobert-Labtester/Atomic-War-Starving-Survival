namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Delayed radiation-illness pipeline stage. Owned and written by
    /// AtomicWar._Game.Radiation.PrognosisPipeline (invoked from RadiationSystem).
    /// Distinct from the instant acute/chronic checks in RadiationSystem.Expose,
    /// which remain unchanged: this is the "hurts you later" layer on top.
    /// </summary>
    public enum PrognosisStage
    {
        Healthy,
        Prodromal,
        Latent,
        Manifest,
        RecoveryOrDeath
    }
}
