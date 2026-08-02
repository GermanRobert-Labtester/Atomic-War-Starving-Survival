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
        RadResistance,
        /// <summary>
        /// Seasonal-affective / cabin-fever collapse from prolonged light deprivation.
        /// Hidden from the main needs bars; visible only as a status tag.
        /// Cleared by a sun-lamp session, a vitaminD-rich meal, or returning
        /// LightExposure above LightProfile.listlessThreshold.
        /// </summary>
        Listless
    }
}
