using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Represents a storage slot within a shelter room. Tracks the item stored,
    /// its quantity, and its adjacency relationships for cross-contamination spread.
    /// Save/load safe.
    /// </summary>
    [Serializable]
    public class StorageSlot
    {
        public string SlotId;
        public ItemDefinition Item;
        public int Amount;
        
        /// <summary>
        /// Actual contamination level of this item instance (0..1).
        /// May differ from ItemDefinition.contamination due to cross-contamination.
        /// </summary>
        public float Contamination;
        
        /// <summary>Adjacent slot indices (for cross-contamination spread).</summary>
        public List<int> AdjacentSlotIndices = new List<int>();
        
        /// <summary>2D position for distance calculations (if using spatial falloff).</summary>
        public Vector2Int Position;

        public StorageSlot() { }

        public StorageSlot(string slotId, Vector2Int position)
        {
            SlotId = slotId;
            Position = position;
            AdjacentSlotIndices = new List<int>();
        }

        /// <summary>Whether this slot contains an item.</summary>
        public bool IsEmpty => Item == null || Amount <= 0;

        /// <summary>Add an item to this slot. Returns true if successful.</summary>
        public bool AddItem(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;
            
            if (IsEmpty)
            {
                Item = item;
                Amount = amount;
                Contamination = item.contamination;
                return true;
            }
            
            if (Item.id != item.id) return false;
            
            Amount += amount;
            // Contamination is the weighted average of existing and new items
            float totalAmount = Amount;
            Contamination = (Contamination * (totalAmount - amount) + item.contamination * amount) / totalAmount;
            return true;
        }

        /// <summary>Remove an amount from this slot. Returns true if successful.</summary>
        public bool RemoveItem(int amount)
        {
            if (IsEmpty || amount <= 0 || amount > Amount) return false;
            
            Amount -= amount;
            if (Amount <= 0)
            {
                Item = null;
                Amount = 0;
                Contamination = 0f;
            }
            return true;
        }
    }
}
