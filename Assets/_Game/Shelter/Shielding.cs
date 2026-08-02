using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Legacy wrapper for radiation shielding level of the shelter.
    /// Delegates to shelter modules and maintains serializable compatibility.
    /// </summary>
    [System.Serializable]
    public class Shielding
    {
        private Shelter _shelter;
        private int _fallbackLevel = 1;

        public int Level
        {
            get
            {
                var mod = _shelter?.GetModule("radiation_shielding");
                return mod != null ? mod.Level : _fallbackLevel;
            }
            set
            {
                _fallbackLevel = value;
                var mod = _shelter?.GetModule("radiation_shielding");
                if (mod != null) mod.Level = value;
            }
        }

        public float AttenuationFactor => Mathf.Clamp01(Level * 0.15f);

        public void BindShelter(Shelter shelter)
        {
            _shelter = shelter;
        }

        public void Upgrade()
        {
            Level++;
        }
    }
}
