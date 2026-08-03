using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// Runs a 3-tier conversion queue on <see cref="WaterStorage"/>: irradiated
    /// -&gt; dirty -&gt; clean, one tier-step per <see cref="ConversionHoursPerUnit"/>
    /// hours while powered (needs Power, see PowerNetwork). Each converted unit
    /// consumes <see cref="FilterDegradationPerUnitConverted"/> off the shared
    /// ShelterModuleInstance.FilterHealth (charcoal/filter durability); ticking
    /// halts once the filter is depleted until replaced.
    /// </summary>
    [CreateAssetMenu(fileName = "WaterPurifierModule", menuName = "ASHFALL/Shelter/Water Purifier Module")]
    public class WaterPurifierModuleSO : ShelterModule
    {
        [Header("Water Purifier Parameters")]
        public float ConversionHoursPerUnit = 2f;
        [Tooltip("Filter/charcoal durability consumed per unit of water converted one tier.")]
        public float FilterDegradationPerUnitConverted = 5f;
    }
}
