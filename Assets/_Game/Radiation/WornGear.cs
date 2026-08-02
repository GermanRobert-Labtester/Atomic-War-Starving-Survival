using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Runtime instance of a piece of ProtectiveGear currently worn by a survivor:
    /// the gear definition plus its remaining durability. Protection scales with
    /// the durability fraction, so a suit at zero durability protects nothing.
    /// Save/load safe (public fields).
    /// </summary>
    [System.Serializable]
    public class WornGear
    {
        public ProtectiveGear Gear;
        public float CurrentDurability;

        /// <summary>Remaining durability as a 0..1 fraction of the gear's max durability.</summary>
        public float DurabilityFraction()
        {
            if (Gear == null || Gear.durability <= 0f)
            {
                return 0f;
            }
            return Mathf.Clamp01(CurrentDurability / Gear.durability);
        }

        /// <summary>Effective protection right now: radProtection scaled by durability fraction.</summary>
        public float EffectiveProtection()
        {
            return Gear == null ? 0f : Mathf.Max(0f, Gear.radProtection) * DurabilityFraction();
        }

        /// <summary>Wear the gear down by its degradeRate over elapsed game hours (floored at 0).</summary>
        public void Degrade(float gameHours)
        {
            if (Gear == null || gameHours <= 0f)
            {
                return;
            }
            CurrentDurability = Mathf.Max(0f, CurrentDurability - Gear.degradeRate * gameHours);
        }
    }
}
