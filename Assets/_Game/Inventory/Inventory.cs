using System.Collections.Generic;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Runtime item container holding stacks keyed by ItemDefinition. Save/load
    /// safe. Raises events on add/remove so the UI and quests can react.
    /// </summary>
    [System.Serializable]
    public class Inventory
    {
        public int Capacity;
        public List<InventorySlot> Slots { get; } = new List<InventorySlot>();

        /// <summary>Add a quantity of an item; false if capacity is exceeded.</summary>
        public bool Add(ItemDefinition item, int amount) => throw new System.NotImplementedException();

        /// <summary>Remove a quantity of an item; false if insufficient stock.</summary>
        public bool Remove(ItemDefinition item, int amount) => throw new System.NotImplementedException();

        /// <summary>Total quantity held of an item across all stacks.</summary>
        public int Count(ItemDefinition item) => throw new System.NotImplementedException();
    }

    /// <summary>A single stack within an inventory.</summary>
    [System.Serializable]
    public class InventorySlot
    {
        public ItemDefinition Item;
        public int Amount;
    }
}
