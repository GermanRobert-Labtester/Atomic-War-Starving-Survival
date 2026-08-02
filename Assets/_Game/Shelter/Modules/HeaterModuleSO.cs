using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "HeaterModule", menuName = "ASHFALL/Shelter/Heater Module")]
    public class HeaterModuleSO : ShelterModule
    {
        [Header("Heater Parameters")]
        public float HeatOutputPerLevel = 5f;
        public float FuelConsumptionRatePerHour = 1f;
        public float HeatRadius = 10f;
        public ItemDefinition FuelItem;
    }
}
