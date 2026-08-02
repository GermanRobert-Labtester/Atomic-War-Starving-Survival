using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "DeconStationModule", menuName = "ASHFALL/Shelter/Decon Station Module")]
    public class DeconStationModuleSO : ShelterModule
    {
        [Header("Decontamination Station Parameters")]
        public float DeconRatePerHour = 20f;
    }
}
