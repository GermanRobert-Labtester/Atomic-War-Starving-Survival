using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// Radio module definition: provides radio operation parameters including power
    /// consumption, EMP vulnerability, and antenna characteristics. Wired to RadioTunerSystem
    /// at runtime to enable intel extraction from frequencies.
    /// </summary>
    [CreateAssetMenu(fileName = "RadioModule", menuName = "ASHFALL/Shelter/Radio Module")]
    public class RadioModuleSO : ShelterModule
    {
        [Header("Radio Parameters")]
        public float BroadcastRange = 50f;
        public float EventSignalBonus = 0.1f;

        [Header("Power & Durability")]
        [Tooltip("Fuel/electricity consumed per hour of operation")]
        public float PowerConsumptionPerHour = 0.5f;
        [Tooltip("Initial fuel capacity")]
        public float FuelCapacity = 50f;
        [Tooltip("EMP damage susceptibility (0..1). Higher = more vulnerable")]
        [Range(0f, 1f)]
        public float EmpSusceptibility = 0.3f;
        [Tooltip("Maximum EMP damage before destroyed (100 = destroyed)")]
        public float MaxEmpDamage = 100f;

        [Header("Tuning")]
        [Tooltip("Base tuning time in hours (at 100% signal)")]
        public float BaseTuningHours = 2f;
    }
}
