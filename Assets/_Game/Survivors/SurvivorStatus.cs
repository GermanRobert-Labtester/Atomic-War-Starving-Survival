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
        Listless,
        /// <summary>
        /// Manifest-stage outcome of the delayed prognosis pipeline (distinct from the
        /// instant AcuteRadiationSickness above): granted when Latent's onset timer
        /// elapses and health crashes. See AtomicWar._Game.Radiation.PrognosisPipeline.
        /// </summary>
        AcuteRadiationSyndrome,
        /// <summary>
        /// Rises when a survivor's perceived radiation risk AND current instrument
        /// uncertainty are both high; causes refusal-to-scavenge, hoarding, and sleep
        /// loss. Cleared by talk/comfort event effects lowering RadiationAnxiety, or
        /// by perceived risk or uncertainty dropping. See BeliefSystem.
        /// </summary>
        RadiationAnxiety,
        /// <summary>
        /// The opposite failure mode of RadiationAnxiety: the survivor stops caring
        /// about radiation danger and takes lethal risks. Builds when perceived risk
        /// stays low for a trait prone to numbness (Reckless/Denialist/Fatalist).
        /// Can be shocked back down by a near-death experience. See BeliefSystem.
        /// </summary>
        Numb
    }
}
