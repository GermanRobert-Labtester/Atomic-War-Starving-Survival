namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Coarse activity state driving need decay rates and AI availability.
    /// </summary>
    public enum SurvivorState
    {
        Idle,
        Working,
        Resting,
        Sick,
        Incapacitated,
        Dead
    }

    /// <summary>
    /// Runtime model for a single survivor: identity, activity state, and current
    /// need values. Save/load safe (primitives only); behaviour lives in
    /// NeedsSystem, RadiationSystem, and the Utility AI.
    /// </summary>
    [System.Serializable]
    public class Survivor
    {
        public string Id;
        public string DisplayName;

        // Plain field, not an auto-property: JsonUtility does not serialize
        // properties, so a { get; set; } here would silently fail to save/load.
        public SurvivorState State = SurvivorState.Idle;

        public bool IsAlive => State != SurvivorState.Dead;

        public Needs Needs { get; } = new Needs();

        // Owned and written by AtomicWar._Game.Radiation.RadiationSystem.
        // RadiationDose is the current, clamped 0..100 reading; LifetimeRadiationExposure
        // is unclamped and only ever grows, driving Chronic Illness.
        public float RadiationDose;
        public float LifetimeRadiationExposure;

        public bool HasAcuteRadiationSickness;
        public bool HasChronicIllness;
        public bool HasFullSuitEquipped;

        // Temporary rad resistance (e.g. from iodine pills): timed, owned and written
        // by AtomicWar._Game.Radiation.RadiationSystem.
        public bool HasRadResistance;
        public float RadResistanceHoursRemaining;

        // -------------------------------------------------------------------
        // Photoperiod / light state — owned and written by PhotoperiodSystem.
        // -------------------------------------------------------------------

        /// <summary>
        /// Recent-light index (0..100). Accumulates during effective daylight,
        /// drains in darkness.  When it falls to LightProfile.listlessThreshold
        /// the survivor becomes Listless.
        /// </summary>
        public float LightExposure = 100f;

        /// <summary>
        /// Hidden status: true when LightExposure has been below the threshold long
        /// enough to trigger seasonal-affective / cabin-fever effects.  Does NOT
        /// appear as a visible need bar; manifests as morale drain + AI score penalty.
        /// </summary>
        public bool IsListless;

        /// <summary>
        /// Vitamin D proxy (0..100, hidden). Accumulates slowly in useful light;
        /// decays in prolonged darkness.  When low it silently penalises health and
        /// morale. Offset by consuming vitaminD-tagged food items (fish, eggs, etc.).
        /// </summary>
        public float VitaminDProxy = 100f;

        /// <summary>Whether the given status is currently active on this survivor.</summary>
        public bool HasStatus(SurvivorStatus status)
        {
            switch (status)
            {
                case SurvivorStatus.AcuteRadiationSickness: return HasAcuteRadiationSickness;
                case SurvivorStatus.ChronicIllness: return HasChronicIllness;
                case SurvivorStatus.RadResistance: return HasRadResistance;
                case SurvivorStatus.Listless: return IsListless;
                default: return false;
            }
        }
    }
}
