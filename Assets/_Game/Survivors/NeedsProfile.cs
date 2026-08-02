using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Designer-tunable rates and thresholds for NeedsSystem. One asset is enough
    /// for the whole game; per-archetype overrides can layer on top later.
    /// </summary>
    [CreateAssetMenu(fileName = "NewNeedsProfile", menuName = "ASHFALL/Data/Needs Profile")]
    public class NeedsProfile : ScriptableObject
    {
        [Header("Per-Hour Rates")]
        public float hungerPerHour = 2f;
        public float thirstPerHour = 3f;
        public float fatiguePerHour = 1.5f;
        public float warmthLossPerHourInCold = 4f;
        public float warmthRestorePerHourNearHeat = 6f;

        [Header("Critical Thresholds")]
        public float hungerCritical = 100f;
        public float thirstCritical = 100f;
        public float warmthCritical = 10f;

        [Header("Health Loss Per Hour While Critical")]
        public float healthLossFromHunger = 3f;
        public float healthLossFromThirst = 4f;
        public float healthLossFromCold = 2f;

        [Header("Morale")]
        public float moraleLossPerHourWhileCritical = 1f;
    }
}
