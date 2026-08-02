using UnityEngine;

namespace AtomicWar.Data
{
    public enum ItemCategory
    {
        Food,
        Medical,
        Material,
        Weapon,
        Fuel,
        Filter
    }

    [CreateAssetMenu(fileName = "NewItem", menuName = "AtomicWar/Data/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Identity")]
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;
        public Sprite Icon;

        [Header("Properties")]
        public ItemCategory Category;
        public bool IsStackable = true;
        public int MaxStackSize = 20;
        public float Weight = 0.5f;

        [Header("Consumable Stats")]
        public float HungerRestored;
        public float FatigueRestored;
        public float HealthRestored;
        public float MoraleRestored;
    }
}
