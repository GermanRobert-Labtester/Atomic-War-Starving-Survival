using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Core inventory bridge for the bunker receiving crate. The crate's actual
    /// contents live in an ordinary saveable <see cref="Inventory"/>; this
    /// system owns only safe one-item transfers and an immutable UI snapshot.
    /// </summary>
    public sealed class OverflowCrateSystem : IDisposable
    {
        private readonly Inventory _overflowStash;
        private readonly Inventory _fieldBag;
        private bool _suppressInventoryEvents;

        /// <summary>Raised whenever crate contents or field-bag capacity changes.</summary>
        public event Action OnChanged;
        /// <summary>Raised after every player transfer attempt, successful or held.</summary>
        public event Action<OverflowCrateTransferResult> OnTransferResolved;

        public OverflowCrateSystem(Inventory overflowStash, Inventory fieldBag)
        {
            _overflowStash = overflowStash ?? throw new ArgumentNullException(nameof(overflowStash));
            _fieldBag = fieldBag ?? throw new ArgumentNullException(nameof(fieldBag));
            _overflowStash.OnInventoryChanged += HandleInventoryChanged;
            _fieldBag.OnInventoryChanged += HandleInventoryChanged;
        }

        /// <summary>
        /// Builds a display-only snapshot. Each line represents one item id held
        /// in the crate and includes whether one unit can safely enter the bag.
        /// </summary>
        public OverflowCrateSnapshot GetSnapshot()
        {
            var snapshot = new OverflowCrateSnapshot
            {
                FieldBagStackCount = _fieldBag.Slots != null ? _fieldBag.Slots.Count : 0,
                FieldBagCapacity = _fieldBag.Capacity,
                FieldBagWeight = _fieldBag.GetCurrentWeight(),
                FieldBagMaxWeight = _fieldBag.MaxWeight
            };

            var slots = _overflowStash.Slots;
            if (slots == null) return snapshot;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot?.Item == null || slot.Amount <= 0 || string.IsNullOrEmpty(slot.Item.id)) continue;

                var entry = FindEntry(snapshot.Items, slot.Item.id);
                if (entry == null)
                {
                    entry = new OverflowCrateItem
                    {
                        ItemId = slot.Item.id,
                        DisplayName = string.IsNullOrEmpty(slot.Item.displayName) ? slot.Item.id : slot.Item.displayName,
                        UnitWeight = slot.Item.weight
                    };
                    snapshot.Items.Add(entry);
                }
                entry.Amount += slot.Amount;
            }

            for (int i = 0; i < snapshot.Items.Count; i++)
            {
                var entry = snapshot.Items[i];
                entry.TransferBlockReason = GetTransferBlockReason(entry.ItemId);
                entry.CanTransfer = string.IsNullOrEmpty(entry.TransferBlockReason);
            }
            return snapshot;
        }

        /// <summary>
        /// Moves exactly one selected item into the field bag. <see cref="Inventory.Transfer"/>
        /// preflights capacity and rolls back if needed, so neither side can lose stock.
        /// </summary>
        public bool TryTransferOne(string itemId, out OverflowCrateTransferResult result)
        {
            var item = FindItem(itemId);
            string reason = GetTransferBlockReason(itemId);
            if (!string.IsNullOrEmpty(reason))
            {
                result = OverflowCrateTransferResult.Held(itemId, item != null ? DisplayName(item) : itemId, reason);
                OnTransferResolved?.Invoke(result);
                return false;
            }

            _suppressInventoryEvents = true;
            bool transferred;
            try
            {
                transferred = _overflowStash.Transfer(item, 1, _fieldBag);
            }
            finally
            {
                _suppressInventoryEvents = false;
            }

            if (!transferred)
            {
                result = OverflowCrateTransferResult.Held(itemId, DisplayName(item),
                    "Field bag could not accept the item.");
                OnTransferResolved?.Invoke(result);
                return false;
            }

            result = OverflowCrateTransferResult.Moved(itemId, DisplayName(item));
            OnChanged?.Invoke();
            OnTransferResolved?.Invoke(result);
            return true;
        }

        public void Dispose()
        {
            _overflowStash.OnInventoryChanged -= HandleInventoryChanged;
            _fieldBag.OnInventoryChanged -= HandleInventoryChanged;
        }

        private void HandleInventoryChanged()
        {
            if (!_suppressInventoryEvents)
                OnChanged?.Invoke();
        }

        private string GetTransferBlockReason(string itemId)
        {
            var item = FindItem(itemId);
            if (item == null) return "Item is no longer in the crate.";
            if (_overflowStash.Count(item) < 1) return "Item is no longer in the crate.";
            if (_fieldBag.CanAdd(item, 1)) return null;

            if (_fieldBag.MaxWeight > 0f
                && _fieldBag.GetCurrentWeight() + item.weight > _fieldBag.MaxWeight)
                return "Field bag would be too heavy.";
            return "Field bag has no safe room for one item.";
        }

        private ItemDefinition FindItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            var slot = _overflowStash.FindSlot(itemId);
            return slot != null ? slot.Item : null;
        }

        private static OverflowCrateItem FindEntry(List<OverflowCrateItem> entries, string itemId)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i] != null && entries[i].ItemId == itemId)
                    return entries[i];
            }
            return null;
        }

        private static string DisplayName(ItemDefinition item)
        {
            return item == null || string.IsNullOrEmpty(item.displayName) ? item?.id : item.displayName;
        }
    }

    /// <summary>Display-only inventory state for the bunker overflow-crate UI.</summary>
    [Serializable]
    public sealed class OverflowCrateSnapshot
    {
        public List<OverflowCrateItem> Items = new List<OverflowCrateItem>();
        public int FieldBagStackCount;
        public int FieldBagCapacity;
        public float FieldBagWeight;
        public float FieldBagMaxWeight;
    }

    /// <summary>One aggregated item line in an <see cref="OverflowCrateSnapshot"/>.</summary>
    [Serializable]
    public sealed class OverflowCrateItem
    {
        public string ItemId;
        public string DisplayName;
        public int Amount;
        public float UnitWeight;
        public bool CanTransfer;
        public string TransferBlockReason;
    }

    /// <summary>Outcome of a one-item transfer from the bunker crate to the field bag.</summary>
    [Serializable]
    public sealed class OverflowCrateTransferResult
    {
        public string ItemId;
        public string DisplayName;
        public bool Succeeded;
        public string Reason;

        public static OverflowCrateTransferResult Moved(string itemId, string displayName)
        {
            return new OverflowCrateTransferResult
            {
                ItemId = itemId,
                DisplayName = displayName,
                Succeeded = true
            };
        }

        public static OverflowCrateTransferResult Held(string itemId, string displayName, string reason)
        {
            return new OverflowCrateTransferResult
            {
                ItemId = itemId,
                DisplayName = displayName,
                Succeeded = false,
                Reason = reason
            };
        }
    }
}
