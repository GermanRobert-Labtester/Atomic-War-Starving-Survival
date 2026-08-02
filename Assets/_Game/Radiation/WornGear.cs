using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Runtime instance of a piece of protective gear currently worn by a survivor,
    /// stored as raw stats (protection, durability, degrade rate) so it can be built
    /// from either a ProtectiveGear asset or an equipped ItemDefinition without the
    /// Radiation assembly depending on Inventory. Protection scales with the durability
    /// fraction, so a suit at zero durability protects nothing. Save/load safe.
    /// </summary>
    [System.Serializable]
    public class WornGear
    {
        public float RadProtection;
        public float MaxDurability;
        public float CurrentDurability;
        public float DegradeRate;

        private ProtectiveGear _gear;
        public ProtectiveGear Gear
        {
            get => _gear;
            set
            {
                _gear = value;
                if (value != null)
                {
                    RadProtection = value.radProtection;
                    MaxDurability = value.durability;
                    DegradeRate = value.degradeRate;
                }
            }
        }

        /// <summary>Remaining durability as a 0..1 fraction of max durability.</summary>
        public float DurabilityFraction()
        {
            return MaxDurability > 0f ? Mathf.Clamp01(CurrentDurability / MaxDurability) : 0f;
        }

        /// <summary>Effective protection right now: radProtection scaled by durability fraction.</summary>
        public float EffectiveProtection()
        {
            return Mathf.Max(0f, RadProtection) * DurabilityFraction();
        }

        /// <summary>Wear the gear down by its degrade rate over elapsed game hours (floored at 0).</summary>
        public void Degrade(float gameHours)
        {
            if (gameHours <= 0f)
            {
                return;
            }
            CurrentDurability = Mathf.Max(0f, CurrentDurability - DegradeRate * gameHours);
        }
    }
}
