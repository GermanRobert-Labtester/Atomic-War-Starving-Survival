using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Safe face/body loadout bridge for the field bag. The worn state remains
    /// owned by <see cref="Inventory"/> so it is saved by the existing inventory
    /// serializer; this system only coordinates protected swaps and builds UI
    /// snapshots.
    /// </summary>
    public sealed class FieldGearLoadoutSystem : IDisposable
    {
        private readonly Inventory _fieldBag;
        private readonly Inventory _overflowStash;
        private bool _suppressInventoryEvents;

        /// <summary>Raised whenever shown gear, bag candidates, or crate state changes.</summary>
        public event Action OnChanged;
        /// <summary>Raised after every equip or stow attempt.</summary>
        public event Action<FieldGearLoadoutResult> OnActionResolved;

        public FieldGearLoadoutSystem(Inventory fieldBag, Inventory overflowStash)
        {
            _fieldBag = fieldBag ?? throw new ArgumentNullException(nameof(fieldBag));
            _overflowStash = overflowStash ?? throw new ArgumentNullException(nameof(overflowStash));
            _fieldBag.OnInventoryChanged += HandleInventoryChanged;
            _overflowStash.OnInventoryChanged += HandleInventoryChanged;
        }

        /// <summary>Build a read-only presentation of face/body equipment and usable bag candidates.</summary>
        public FieldGearLoadoutSnapshot GetSnapshot()
        {
            var snapshot = new FieldGearLoadoutSnapshot
            {
                Face = BuildSlot(EquipSlot.Face),
                Body = BuildSlot(EquipSlot.Body),
                TotalProtection = _fieldBag.GetEquippedProtection(),
                FieldBagStackCount = _fieldBag.Slots != null ? _fieldBag.Slots.Count : 0,
                FieldBagCapacity = _fieldBag.Capacity,
                FieldBagWeight = _fieldBag.GetCurrentWeight(),
                FieldBagMaxWeight = _fieldBag.MaxWeight
            };

            var slots = _fieldBag.Slots;
            if (slots == null) return snapshot;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var item = slot != null ? slot.Item : null;
                if (!SupportsLoadout(item) || slot.Amount <= 0) continue;

                var candidate = FindCandidate(snapshot.Candidates, item.id);
                if (candidate == null)
                {
                    candidate = new FieldGearCandidate
                    {
                        ItemId = item.id,
                        DisplayName = DisplayName(item),
                        Slot = item.equipSlot,
                        Protection = Mathf.Max(0f, item.radProtection),
                        MaxDurability = item.durability
                    };
                    snapshot.Candidates.Add(candidate);
                }
                candidate.Amount += slot.Amount;
            }
            return snapshot;
        }

        /// <summary>
        /// Equip one face/body candidate. If its occupied slot cannot return the
        /// outgoing gear to the full field bag, that gear is sent to the existing
        /// unlimited receiving crate before the candidate is equipped.
        /// </summary>
        public bool TryEquip(string itemId, out FieldGearLoadoutResult result)
        {
            var item = FindCandidateItem(itemId);
            if (item == null)
            {
                result = FieldGearLoadoutResult.Held(itemId, null, EquipSlot.None,
                    "That gear is no longer in the field bag.");
                OnActionResolved?.Invoke(result);
                return false;
            }

            var current = _fieldBag.GetEquipped(item.equipSlot);
            bool sentToOverflow = false;
            bool equipped;
            _suppressInventoryEvents = true;
            try
            {
                if (current != null && !_fieldBag.CanAdd(current.Item, 1))
                {
                    if (!_overflowStash.CanAdd(current.Item, 1)
                        || !_fieldBag.TryUnequipTo(item.equipSlot, _overflowStash))
                    {
                        result = FieldGearLoadoutResult.Held(item.id, DisplayName(item), item.equipSlot,
                            "The field bag is full and the receiving crate cannot hold the outgoing gear.");
                        OnActionResolved?.Invoke(result);
                        return false;
                    }
                    sentToOverflow = true;
                }

                equipped = _fieldBag.Equip(item);
            }
            finally
            {
                _suppressInventoryEvents = false;
            }

            if (!equipped)
            {
                result = FieldGearLoadoutResult.Held(item.id, DisplayName(item), item.equipSlot,
                    "The field bag could not complete that gear change.");
                OnActionResolved?.Invoke(result);
                return false;
            }

            result = FieldGearLoadoutResult.Equipped(item.id, DisplayName(item), item.equipSlot, sentToOverflow);
            OnChanged?.Invoke();
            OnActionResolved?.Invoke(result);
            return true;
        }

        /// <summary>Stow worn face/body gear in the bag, using the receiving crate if the bag is full.</summary>
        public bool TryUnequip(EquipSlot slot, out FieldGearLoadoutResult result)
        {
            if (!IsLoadoutSlot(slot))
            {
                result = FieldGearLoadoutResult.Held(null, null, slot, "Only face or body gear can be managed here.");
                OnActionResolved?.Invoke(result);
                return false;
            }

            var current = _fieldBag.GetEquipped(slot);
            if (current == null || current.Item == null)
            {
                result = FieldGearLoadoutResult.Held(null, null, slot, "That loadout slot is already clear.");
                OnActionResolved?.Invoke(result);
                return false;
            }

            var item = current.Item;
            bool sentToOverflow = false;
            bool stowed;
            _suppressInventoryEvents = true;
            try
            {
                if (_fieldBag.CanAdd(item, 1))
                    stowed = _fieldBag.Unequip(slot) != null;
                else
                {
                    stowed = _overflowStash.CanAdd(item, 1)
                        && _fieldBag.TryUnequipTo(slot, _overflowStash);
                    sentToOverflow = stowed;
                }
            }
            finally
            {
                _suppressInventoryEvents = false;
            }

            if (!stowed)
            {
                result = FieldGearLoadoutResult.Held(item.id, DisplayName(item), slot,
                    "Neither the field bag nor the receiving crate can hold that gear.");
                OnActionResolved?.Invoke(result);
                return false;
            }

            result = FieldGearLoadoutResult.Stowed(item.id, DisplayName(item), slot, sentToOverflow);
            OnChanged?.Invoke();
            OnActionResolved?.Invoke(result);
            return true;
        }

        public void Dispose()
        {
            _fieldBag.OnInventoryChanged -= HandleInventoryChanged;
            _overflowStash.OnInventoryChanged -= HandleInventoryChanged;
        }

        private FieldGearSlotState BuildSlot(EquipSlot slot)
        {
            var equipped = _fieldBag.GetEquipped(slot);
            var item = equipped != null ? equipped.Item : null;
            if (item == null)
                return new FieldGearSlotState { Slot = slot };

            float durabilityPercent = item.durability > 0f
                ? Mathf.Clamp01(equipped.CurrentDurability / item.durability) * 100f
                : -1f;
            return new FieldGearSlotState
            {
                Slot = slot,
                ItemId = item.id,
                DisplayName = DisplayName(item),
                CurrentDurability = equipped.CurrentDurability,
                MaxDurability = item.durability,
                DurabilityPercent = durabilityPercent,
                Protection = durabilityPercent >= 0f
                    ? Mathf.Max(0f, item.radProtection) * durabilityPercent / 100f
                    : 0f
            };
        }

        private ItemDefinition FindCandidateItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            var slot = _fieldBag.FindSlot(itemId);
            return slot != null && slot.Amount > 0 && SupportsLoadout(slot.Item) ? slot.Item : null;
        }

        private void HandleInventoryChanged()
        {
            if (!_suppressInventoryEvents)
                OnChanged?.Invoke();
        }

        private static bool SupportsLoadout(ItemDefinition item)
        {
            return item != null && item.isEquipable && IsLoadoutSlot(item.equipSlot);
        }

        private static bool IsLoadoutSlot(EquipSlot slot)
        {
            return slot == EquipSlot.Face || slot == EquipSlot.Body;
        }

        private static FieldGearCandidate FindCandidate(List<FieldGearCandidate> candidates, string itemId)
        {
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i] != null && candidates[i].ItemId == itemId)
                    return candidates[i];
            }
            return null;
        }

        private static string DisplayName(ItemDefinition item)
        {
            return item == null || string.IsNullOrEmpty(item.displayName) ? item?.id : item.displayName;
        }
    }

    /// <summary>Display-only state used by the field-gear UI.</summary>
    [Serializable]
    public sealed class FieldGearLoadoutSnapshot
    {
        public FieldGearSlotState Face = new FieldGearSlotState { Slot = EquipSlot.Face };
        public FieldGearSlotState Body = new FieldGearSlotState { Slot = EquipSlot.Body };
        public List<FieldGearCandidate> Candidates = new List<FieldGearCandidate>();
        public float TotalProtection;
        public int FieldBagStackCount;
        public int FieldBagCapacity;
        public float FieldBagWeight;
        public float FieldBagMaxWeight;
    }

    [Serializable]
    public sealed class FieldGearSlotState
    {
        public EquipSlot Slot;
        public string ItemId;
        public string DisplayName;
        public float CurrentDurability;
        public float MaxDurability;
        /// <summary>-1 means the gear has no durability meter.</summary>
        public float DurabilityPercent = -1f;
        public float Protection;
        public bool IsEquipped => !string.IsNullOrEmpty(ItemId);
    }

    [Serializable]
    public sealed class FieldGearCandidate
    {
        public string ItemId;
        public string DisplayName;
        public EquipSlot Slot;
        public int Amount;
        public float Protection;
        public float MaxDurability;
    }

    /// <summary>Player-facing outcome of a field-gear operation.</summary>
    [Serializable]
    public sealed class FieldGearLoadoutResult
    {
        public string ItemId;
        public string DisplayName;
        public EquipSlot Slot;
        public bool Succeeded;
        public bool SentToOverflow;
        public string Reason;

        public static FieldGearLoadoutResult Equipped(string itemId, string displayName, EquipSlot slot, bool sentToOverflow)
        {
            return new FieldGearLoadoutResult
            {
                ItemId = itemId,
                DisplayName = displayName,
                Slot = slot,
                Succeeded = true,
                SentToOverflow = sentToOverflow
            };
        }

        public static FieldGearLoadoutResult Stowed(string itemId, string displayName, EquipSlot slot, bool sentToOverflow)
        {
            return Equipped(itemId, displayName, slot, sentToOverflow);
        }

        public static FieldGearLoadoutResult Held(string itemId, string displayName, EquipSlot slot, string reason)
        {
            return new FieldGearLoadoutResult
            {
                ItemId = itemId,
                DisplayName = displayName,
                Slot = slot,
                Succeeded = false,
                Reason = reason
            };
        }
    }
}
