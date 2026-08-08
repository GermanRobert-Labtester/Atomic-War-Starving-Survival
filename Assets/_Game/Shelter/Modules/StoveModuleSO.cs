using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "StoveModule", menuName = "ASHFALL/Shelter/Stove Module")]
    public class StoveModuleSO : ShelterModule
    {
        [Header("Stove Parameters")]
        public float CookingEfficiency = 1f;
    }
}
