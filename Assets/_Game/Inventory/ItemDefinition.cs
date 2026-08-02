using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// ScriptableObject definition of a single item type. Authored as a .asset or
    /// imported from StreamingAssets/Data/items.json. Covers identity, stacking,
    /// weight, protective/equipment stats, contamination, and on-consume effects.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItemDefinition", menuName = "ASHFALL/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea(2, 5)] public string description;
        public Sprite iconRef;

        [Header("Classification")]
        public ItemType type;
        public int stackMax = 1;
        public float weight;

        [Header("Protection / Equipment")]
        /// <summary>Dose-rate blocked while equipped/intact (same units as RadZoneProfile.radLevel).</summary>
        public float radProtection;
        /// <summary>Maximum durability; protection scales with remaining durability.</summary>
        public float durability;
        public bool isEquipable;
        public EquipSlot equipSlot;

        [Header("Contamination")]
        /// <summary>0..1 contamination level; ingesting contaminated items adds dose.</summary>
        [Range(0f, 1f)] public float contamination;

        [Header("Effects (applied on consume)")]
        public float hungerRestore;
        public float thirstRestore;
        public float healthEffect;
        /// <summary>Current radiation dose cleansed on consume (anti-rad).</summary>
        public float radCleanse;
        public float moraleEffect;
    }
}
