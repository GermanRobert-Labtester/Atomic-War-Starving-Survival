using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Data definition for a plantable hydroponic crop (Prompt #37).
    /// </summary>
    [CreateAssetMenu(fileName = "CropDefinition", menuName = "ASHFALL/Shelter/Crop Definition")]
    public class CropSO : ScriptableObject
    {
        [Header("Crop Identity")]
        public string CropId = "potatoes";
        public string DisplayName = "Potatoes";

        [Header("Growth Parameters")]
        [Tooltip("In-game hours required from planting seed to reach maturity.")]
        public float GrowthHoursRequired = 48f;

        [Tooltip("Clean water consumed per in-game hour of growth.")]
        public float WaterRequiredPerHour = 0.5f;

        [Header("Yield Parameters")]
        [Tooltip("Calories provided upon harvesting mature crop.")]
        public float CalorieYield = 50f;

        [Tooltip("Contamination/radiation added upon consuming or harvesting.")]
        public float ContaminationYield = 0f;

        /// <summary>
        /// Prompt #194 — toxic mutant strain. Mycology holders identify these
        /// before harvest and can discard safely.
        /// </summary>
        public bool IsToxicStrain;

        /// <summary>
        /// Create a runtime default instance for testing / fallback.
        /// </summary>
        public static CropSO CreatePotatoes()
        {
            var crop = CreateInstance<CropSO>();
            crop.CropId = "potatoes";
            crop.DisplayName = "Potatoes";
            crop.GrowthHoursRequired = 48f;
            crop.WaterRequiredPerHour = 0.5f;
            crop.CalorieYield = 50f;
            crop.ContaminationYield = 0f;
            return crop;
        }

        public static CropSO CreateMutatedFungi()
        {
            var crop = CreateInstance<CropSO>();
            crop.CropId = "mutated_fungi";
            crop.DisplayName = "Mutated Fungi";
            crop.GrowthHoursRequired = 12f;
            crop.WaterRequiredPerHour = 0.2f;
            crop.CalorieYield = 15f;
            crop.ContaminationYield = 5f;
            return crop;
        }
    }
}
