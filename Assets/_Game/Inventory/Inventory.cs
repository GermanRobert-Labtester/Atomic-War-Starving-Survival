using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Runtime item container holding stacks keyed by ItemDefinition. Save/load
    /// safe. Raises events on add/remove so the UI and quests can react.
    /// </summary>
    [Serializable]
    public class Inventory
    {
        public int Capacity = 20;

        [SerializeField]
        private List<InventorySlot> _slots = new List<InventorySlot>();

        public List<InventorySlot> Slots => _slots;

        public event Action<ItemDefinition, int> OnItemAdded;
        public event Action<ItemDefinition, int> OnItemRemoved;

        /// <summary>Total quantity held of an item across all stacks.</summary>
        public int Count(ItemDefinition item)
        {
            if (item == null || _slots == null) return 0;
            int total = 0;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    total += _slots[i].Amount;
                }
            }
            return total;
        }

        /// <summary>Add a quantity of an item; false if capacity is exceeded.</summary>
        public bool Add(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;

            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    int maxStack = item.stackSize > 0 ? item.stackSize : 99;
                    int space = maxStack - _slots[i].Amount;
                    if (space > 0)
                    {
                        int toAdd = Mathf.Min(space, amount);
                        _slots[i].Amount += toAdd;
                        amount -= toAdd;
                        OnItemAdded?.Invoke(item, toAdd);
                        if (amount <= 0) return true;
                    }
                }
            }

            while (amount > 0)
            {
                if (Capacity > 0 && _slots.Count >= Capacity)
                {
                    return false;
                }
                int maxStack = item.stackSize > 0 ? item.stackSize : 99;
                int toAdd = Mathf.Min(maxStack, amount);
                _slots.Add(new InventorySlot { Item = item, Amount = toAdd });
                amount -= toAdd;
                OnItemAdded?.Invoke(item, toAdd);
            }
            return true;
        }

        /// <summary>Remove a quantity of an item; false if insufficient stock.</summary>
        public bool Remove(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;
            if (Count(item) < amount) return false;

            int remaining = amount;
            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    if (_slots[i].Amount <= remaining)
                    {
                        remaining -= _slots[i].Amount;
                        _slots.RemoveAt(i);
                    }
                    else
                    {
                        _slots[i].Amount -= remaining;
                        remaining = 0;
                    }

                    if (remaining <= 0) break;
                }
            }

            OnItemRemoved?.Invoke(item, amount);
            return true;
        }
    }

    /// <summary>A single stack within an inventory.</summary>
    [Serializable]
    public class InventorySlot
    {
        public ItemDefinition Item;
        public int Amount;
    }
}
