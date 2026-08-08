using AtomicWar._Game.Shelter;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "AirFiltrationModule", menuName = "ASHFALL/Shelter/Air Filtration Module")]
    public class AirFiltrationModuleSO : ShelterModule
    {
        [Header("Filtration Parameters")]
        public float DegradationRatePerHour = 2f;
        public float LowHealthThreshold = 25f;
        public float RadLeakPerTickWhenDepleted = 5f;
        public float MoraleLossPerHourWhenDepleted = 1f;
        public ItemDefinition AirFilterItem;
    }
}
