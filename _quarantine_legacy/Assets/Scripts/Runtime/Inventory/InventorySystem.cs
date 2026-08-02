using System.Collections.Generic;
using System.Linq;
using AtomicWar.Core.Events;
using AtomicWar.Data;
using UnityEngine;

namespace AtomicWar.Runtime.Inventory
{
    public class InventorySlot
    {
        public ItemData Item { get; set; }
        public int Amount { get; set; }

        public InventorySlot(ItemData item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    public struct InventoryUpdatedEvent
    {
        public ItemData Item;
        public int NewTotalAmount;
    }

    /// <summary>
    /// Pure C# system managing stockpile and survivor inventory containers.
    /// </summary>
    public class InventorySystem
    {
        private readonly List<InventorySlot> _slots = new List<InventorySlot>();
        public IReadOnlyList<InventorySlot> Slots => _slots;

        public bool AddItem(ItemData item, int amount)
        {
            if (item == null || amount <= 0) return false;

            var existingSlot = _slots.FirstOrDefault(s => s.Item.Id == item.Id && s.Amount < item.MaxStackSize);
            if (existingSlot != null)
            {
                int addAmount = Mathf.Min(amount, item.MaxStackSize - existingSlot.Amount);
                existingSlot.Amount += addAmount;
                int remaining = amount - addAmount;

                EventBus.Raise(new InventoryUpdatedEvent
                {
                    Item = item,
                    NewTotalAmount = GetItemCount(item.Id)
                });

                if (remaining > 0)
                {
                    return AddItem(item, remaining);
                }
                return true;
            }

            _slots.Add(new InventorySlot(item, amount));
            EventBus.Raise(new InventoryUpdatedEvent
            {
                Item = item,
                NewTotalAmount = GetItemCount(item.Id)
            });

            return true;
        }

        public bool RemoveItem(ItemData item, int amount)
        {
            if (!HasItemAmount(item.Id, amount)) return false;

            int toRemove = amount;
            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                if (_slots[i].Item.Id == item.Id)
                {
                    if (_slots[i].Amount <= toRemove)
                    {
                        toRemove -= _slots[i].Amount;
                        _slots.RemoveAt(i);
                    }
                    else
                    {
                        _slots[i].Amount -= toRemove;
                        toRemove = 0;
                    }

                    if (toRemove <= 0) break;
                }
            }

            EventBus.Raise(new InventoryUpdatedEvent
            {
                Item = item,
                NewTotalAmount = GetItemCount(item.Id)
            });

            return true;
        }

        public bool HasItemAmount(string itemId, int requiredAmount)
        {
            return GetItemCount(itemId) >= requiredAmount;
        }

        public int GetItemCount(string itemId)
        {
            return _slots.Where(s => s.Item.Id == itemId).Sum(s => s.Amount);
        }

        public float TotalWeight => _slots.Sum(s => s.Item.Weight * s.Amount);
    }
}
