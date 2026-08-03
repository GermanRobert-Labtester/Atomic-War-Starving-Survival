using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// Quiet comfort corner: multiplies Utility AI score for mental-break
    /// comfort care when enabled in the shelter.
    /// </summary>
    [CreateAssetMenu(fileName = "ComfortStationModule", menuName = "ASHFALL/Shelter/Comfort Station")]
    public class ComfortStationModuleSO : ShelterModule
    {
        /// <summary>snake_case id — matches MedicalSystem.ComfortStationModuleId.</summary>
        public const string DefaultModuleId = "comfort_station";

        [Header("Comfort Care")]
        [Tooltip("Multiplier applied to MentalBreakComfortActionSO score when this station is enabled.")]
        [Range(1f, 4f)]
        public float comfortCureScoreMultiplier = 1.5f;

        /// <summary>Alias used by some tests / data loaders (maps to ModuleId).</summary>
        public string id
        {
            get => ModuleId;
            set => ModuleId = value;
        }

        public static ComfortStationModuleSO CreateDefault(float multiplier = 1.5f)
        {
            var so = CreateInstance<ComfortStationModuleSO>();
            so.ModuleId = DefaultModuleId;
            so.DisplayName = "Comfort Station";
            so.Description = "A quiet corner with blankets and a lamp. Softens the worst nights.";
            so.comfortCureScoreMultiplier = Mathf.Max(1f, multiplier);
            so.MaxLevel = 2;
            return so;
        }
    }
}
