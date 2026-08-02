using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "RadioModule", menuName = "ASHFALL/Shelter/Radio Module")]
    public class RadioModuleSO : ShelterModule
    {
        [Header("Radio Parameters")]
        public float BroadcastRange = 50f;
        public float EventSignalBonus = 0.1f;
    }
}
