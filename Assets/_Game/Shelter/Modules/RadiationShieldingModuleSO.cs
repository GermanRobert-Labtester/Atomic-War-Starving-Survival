using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "RadiationShieldingModule", menuName = "ASHFALL/Shelter/Radiation Shielding Module")]
    public class RadiationShieldingModuleSO : ShelterModule
    {
        [Header("Shielding Parameters")]
        public float RadReductionPercentPerLevel = 0.15f;

        public float GetAttenuationFraction(int level)
        {
            if (level <= 0) return 0f;
            return Mathf.Clamp01(level * RadReductionPercentPerLevel);
        }
    }
}
