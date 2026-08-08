using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// "Desperate Greenhouse" grow-light module.  Burns fuel to generate artificial
    /// light for crops and morale, competing for the same fuel supply as the heater.
    ///
    /// When operational and fuelled:
    ///   • Adds <see cref="LightEquivalentFraction"/> to each survivor's effective
    ///     light fraction (via PhotoperiodSystem / NeedsSystem.SetPhotoPeriodSystem).
    ///   • Provides a direct morale injection each hour via Shelter.GrowLightMoraleBoost.
    ///   • Contributes a per-hour crop yield bonus (reserved for the food system).
    ///
    /// The dilemma: fuel is finite; running the grow-light competes with the heater.
    /// In deep winter the player must choose between staying warm and staying sane.
    /// </summary>
    [CreateAssetMenu(fileName = "GrowLightModule", menuName = "ASHFALL/Shelter/Grow-Light Module")]
    public class GrowLightModuleSO : ShelterModule
    {
        [Header("Grow-Light Parameters")]
        [Tooltip("Fuel units consumed per in-game hour while the grow-light is on.")]
        public float FuelConsumptionRatePerHour = 1.5f;

        [Tooltip("Fraction of a full daylight cycle this lamp provides (0..1). " +
                 "Stacked with natural light in PhotoperiodSystem; capped at 1.")]
        public float LightEquivalentFraction = 0.5f;

        [Tooltip("Direct morale gain per in-game hour for all sheltered survivors while the grow-light is active.")]
        public float MoraleBoostPerHour = 0.3f;

        [Tooltip("Bonus food-production units per in-game hour (reserved for future crop / greenhouse system).")]
        public float CropYieldBonusPerHour = 0.1f;
    }
}
