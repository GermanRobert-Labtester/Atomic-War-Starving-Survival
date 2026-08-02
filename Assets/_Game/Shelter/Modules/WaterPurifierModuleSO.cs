using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "WaterPurifierModule", menuName = "ASHFALL/Shelter/Water Purifier Module")]
    public class WaterPurifierModuleSO : ShelterModule
    {
        [Header("Water Purifier Parameters")]
        public float ConversionHoursPerUnit = 2f;
        public ItemDefinition IrradiatedWaterItem;
        public ItemDefinition CleanWaterItem;
    }
}
