using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// ScriptableObject definition of a single item type: identity, stacking,
    /// weight, and on-consume effects. Authored as a .asset or imported from
    /// StreamingAssets/Data/items.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItemDefinition", menuName = "ASHFALL/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea(2, 5)] public string description;

        [Header("Classification")]
        public ItemType itemType;
        public int stackSize = 1;
        public float weight;

        [Header("Effects (applied on consume)")]
        public float hungerRestored;
        public float thirstRestored;
        public float healthRestored;
        public float moraleRestored;
        public float radiationRemoved;
    }
}
