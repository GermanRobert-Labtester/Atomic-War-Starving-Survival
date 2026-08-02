using UnityEngine;

namespace AtomicWar.Data
{
    public enum ItemType
    {
        Food,
        Water,
        Medical,
        Weapon,
        Tool,
        Material,
        Fuel,
        Trade,
        Comfort,
        Quest
    }

    [CreateAssetMenu(fileName = "NewItemDefinition", menuName = "Game/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        public string id;
        public string itemName;
        public ItemType itemType;
        public int stackSize = 1;
        public float weight;
        public int hungerRestored;
        public int healthEffect;
        public int moraleEffect;
        [TextArea(2, 5)] public string description;
    }
}
