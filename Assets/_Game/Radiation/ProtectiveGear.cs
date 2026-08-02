using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Definition of a piece of protective gear (hazmat suit, mask, gloves).
    /// radProtection is the dose-rate blocked when the gear is intact; protection
    /// falls off linearly with durability, reaching zero when durability is spent.
    /// </summary>
    [CreateAssetMenu(fileName = "NewProtectiveGear", menuName = "ASHFALL/Radiation/Protective Gear")]
    public class ProtectiveGear : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;

        /// <summary>Dose-rate blocked when intact (same abstract units as RadZoneProfile.radLevel).</summary>
        public float radProtection;

        /// <summary>Maximum durability; current durability is tracked per worn instance (WornGear).</summary>
        public float durability = 100f;

        /// <summary>Durability lost per hour the gear is worn in the field.</summary>
        public float degradeRate = 1f;
    }
}
