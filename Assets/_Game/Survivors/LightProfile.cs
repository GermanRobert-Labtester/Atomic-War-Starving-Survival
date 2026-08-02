using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Designer-tunable rates for the photoperiod / light system.
    /// Governs how quickly survivors accumulate or lose LightExposure, when the
    /// "Listless" hidden status activates, and how the vitamin-D proxy drains
    /// health and morale in the dark.
    ///
    /// One asset is sufficient for the whole campaign; per-archetype overrides
    /// can stack on top when survivors with different physiologies are added.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLightProfile", menuName = "ASHFALL/Data/Light Profile")]
    public class LightProfile : ScriptableObject
    {
        [Header("Light Exposure (0..100)")]
        [Tooltip("Points of LightExposure gained per in-game hour of effective daylight (scaled by light fraction).")]
        public float lightExposureGainPerHourDaylight = 8f;

        [Tooltip("Points of LightExposure lost per in-game hour of near-zero effective daylight.")]
        public float lightExposureLossPerHourDark = 3f;

        [Header("Listless Status")]
        [Tooltip("LightExposure at or below this value triggers the Listless status.")]
        public float listlessThreshold = 20f;

        [Tooltip("Morale drained per in-game hour while Listless is active.")]
        public float listlessMoraleDrainPerHour = 0.5f;

        [Tooltip("Immediate LightExposure boost from a sun-lamp session or equivalent.")]
        public float sunLampSessionBoost = 30f;

        [Header("Vitamin D Proxy (0..100, hidden)")]
        [Tooltip("VitaminD gained per in-game hour when light fraction > 0.1.")]
        public float vitaminDGainPerHourNormalLight = 2f;

        [Tooltip("VitaminD lost per in-game hour in near-total darkness.")]
        public float vitaminDDecayPerHour = 0.8f;

        [Tooltip("VitaminD at or below this value activates the hidden health/morale drain.")]
        public float vitaminDLowThreshold = 20f;

        [Tooltip("Hidden health drain per in-game hour at zero VitaminD (scales linearly with depletion).")]
        public float vitaminDHealthPenaltyPerHour = 0.15f;

        [Tooltip("Hidden morale drain per in-game hour at zero VitaminD.")]
        public float vitaminDMoralePenaltyPerHour = 0.20f;

        [Tooltip("Fraction of VitaminD restored by consuming one vitaminD-tagged food item.")]
        public float vitaminDFoodRestoreAmount = 30f;

        [Header("Grow-Light / Sun-Lamp")]
        [Tooltip("Effective light fraction added by a running grow-light module (0..1); " +
                 "stacks with natural daylight fraction (total clamped to 1).")]
        public float growLightEquivalentFraction = 0.5f;

        [Tooltip("Morale gain per in-game hour when the grow-light is active.")]
        public float growLightMoraleBoostPerHour = 0.3f;
    }
}
