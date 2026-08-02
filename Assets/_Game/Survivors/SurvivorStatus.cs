namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Status effects a survivor can carry. Distinct from the coarse SurvivorState:
    /// several statuses can be active at once and don't by themselves block activity.
    /// </summary>
    public enum SurvivorStatus
    {
        AcuteRadiationSickness,
        ChronicIllness,
        RadResistance
    }
}
