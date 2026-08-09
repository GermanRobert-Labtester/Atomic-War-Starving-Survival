using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// A roof water trap. Collects rainfall/fallout runoff into the bunker's
    /// <see cref="WaterStorage"/> while open (<see cref="ShelterModuleInstance.IsEnabled"/>
    /// true) and the weather is Rain or FalloutStorm. Which tier it fills
    /// (clean/dirty/irradiated) depends on the current weather and campaign
    /// day; see <see cref="AtomicWar._Game.Core.WaterEconomySystem"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "CatchmentSurfaceModule", menuName = "ASHFALL/Shelter/Catchment Surface Module")]
    public class CatchmentSurfaceModuleSO : ShelterModule
    {
        [Header("Catchment Parameters")]
        [Tooltip("Volume collected per game-hour while the trap is open during Rain or FalloutStorm.")]
        public float CollectionRatePerHour = 5f;
    }
}
