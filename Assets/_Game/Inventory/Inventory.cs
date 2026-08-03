using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Runtime item container holding stacks keyed by ItemDefinition, with stack and
    /// weight limits, equipment slots, and item consumption. Save/load safe via
    /// CaptureState/RestoreState (serializes item ids + amounts, not asset refs).
    /// Raises OnItemAdded/OnItemRemoved and the broader OnInventoryChanged on mutation
    /// so the UI, quests, and AI can react.
    /// </summary>
    [Serializable]
    public struct EquippedGearData
    {
        public float RadProtection;
        public float MaxDurability;
        public float CurrentDurability;
        public float DegradeRate;
    }

    [Serializable]
    public class Inventory
    {
        /// <summary>Dose rate applied per unit of item contamination when ingested.</summary>
        public const float ContaminationDosePerUnit = 50f;

        public int Capacity = 20;
        public float MaxWeight = 100f;

        [SerializeField]
        private List<InventorySlot> _slots = new List<InventorySlot>();

        [SerializeField]
        private List<EquippedItem> _equipped = new List<EquippedItem>();

        public List<InventorySlot> Slots => _slots;
        public IReadOnlyList<EquippedItem> Equipped => _equipped;

        public event Action<ItemDefinition, int> OnItemAdded;
        public event Action<ItemDefinition, int> OnItemRemoved;
        /// <summary>Raised after any mutation (add/remove/transfer/equip/unequip/restore).</summary>
        public event Action OnInventoryChanged;

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

        /// <summary>First inventory slot whose item id matches (for device state access).</summary>
        public InventorySlot FindSlot(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return null;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == itemId)
                {
                    return _slots[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Best working Device slot matching the given item id (prefers highest battery,
        /// then highest calibration). Null if none can measure.
        /// </summary>
        public InventorySlot FindBestWorkingDevice(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return null;
            InventorySlot best = null;
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (slot == null || slot.Item == null || slot.Item.id != itemId) continue;
                if (slot.Device == null) slot.Device = DeviceState.CreateDefault();
                if (!InstrumentDevice.CanMeasure(slot.Device)) continue;
                if (best == null
                    || slot.Device.Battery > best.Device.Battery
                    || (Mathf.Approximately(slot.Device.Battery, best.Device.Battery)
                        && slot.Device.Calibration > best.Device.Calibration))
                {
                    best = slot;
                }
            }
            return best;
        }

        /// <summary>True if any owned geiger_counter can currently measure.</summary>
        public bool HasWorkingGeiger()
        {
            return FindBestWorkingDevice("geiger_counter") != null;
        }

        /// <summary>
        /// Best owned geiger device state (working preferred; otherwise any geiger for
        /// "last calibrated" display). Null if none owned.
        /// </summary>
        public DeviceState GetBestGeigerState()
        {
            var working = FindBestWorkingDevice("geiger_counter");
            if (working != null) return working.Device;

            var any = FindSlot("geiger_counter");
            if (any == null) return null;
            if (any.Device == null) any.Device = DeviceState.CreateDefault();
            return any.Device;
        }

        /// <summary>Drift calibration on every Device slot by the given day count.</summary>
        public void DriftAllDevices(float days = 1f)
        {
            if (_slots == null || days <= 0f) return;
            bool changed = false;
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (slot?.Device == null) continue;
                InstrumentDevice.DriftCalibration(slot.Device, days);
                changed = true;
            }
            if (changed) OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Recharge the first matching device by consuming one battery item.
        /// Does not un-break a dead instrument.
        /// </summary>
        public bool RechargeDevice(string deviceItemId, ItemDefinition batteryItem)
        {
            if (batteryItem == null || batteryItem.id != "battery") return false;
            var slot = FindSlot(deviceItemId);
            if (slot?.Device == null) return false;
            if (Count(batteryItem) < 1) return false;
            if (!Remove(batteryItem, 1)) return false;
            InstrumentDevice.Recharge(slot.Device);
            OnInventoryChanged?.Invoke();
            return true;
        }

        /// <summary>Recalibrate the first matching device by consuming one calibration_kit.</summary>
        public bool RecalibrateDevice(string deviceItemId, ItemDefinition kitItem, int currentDay)
        {
            if (kitItem == null || kitItem.id != "calibration_kit") return false;
            var slot = FindSlot(deviceItemId);
            if (slot?.Device == null) return false;
            if (Count(kitItem) < 1) return false;
            if (!Remove(kitItem, 1)) return false;
            InstrumentDevice.Recalibrate(slot.Device, currentDay);
            OnInventoryChanged?.Invoke();
            return true;
        }

        /// <summary>Current carried weight (stored stacks plus equipped items).</summary>
        public float GetCurrentWeight()
        {
            float total = 0f;
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (slot != null && slot.Item != null)
                {
                    total += slot.Item.weight * slot.Amount;
                }
            }
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped != null && equipped.Item != null)
                {
                    total += equipped.Item.weight;
                }
            }
            return total;
        }

        /// <summary>Whether the inventory could accept a quantity (stack + weight + capacity).</summary>
        public bool CanAdd(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;
            if (MaxWeight > 0f && GetCurrentWeight() + item.weight * amount > MaxWeight) return false;

            int stackMax = item.stackMax > 0 ? item.stackMax : 99;
            int remaining = amount;
            for (int i = 0; i < _slots.Count && remaining > 0; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    remaining -= Mathf.Min(stackMax - _slots[i].Amount, remaining);
                }
            }

            int newStacksNeeded = (remaining + stackMax - 1) / stackMax;
            if (Capacity > 0 && _slots.Count + newStacksNeeded > Capacity) return false;
            return true;
        }

        /// <summary>Add a quantity of an item; false if stack, weight, or capacity limits are exceeded.</summary>
        public bool Add(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;
            if (MaxWeight > 0f && GetCurrentWeight() + item.weight * amount > MaxWeight) return false;

            int stackMax = item.stackMax > 0 ? item.stackMax : 99;

            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    int space = stackMax - _slots[i].Amount;
                    if (space > 0)
                    {
                        int toAdd = Mathf.Min(space, amount);
                        _slots[i].Amount += toAdd;
                        amount -= toAdd;
                        OnItemAdded?.Invoke(item, toAdd);
                        if (amount <= 0)
                        {
                            OnInventoryChanged?.Invoke();
                            return true;
                        }
                    }
                }
            }

            while (amount > 0)
            {
                if (Capacity > 0 && _slots.Count >= Capacity)
                {
                    return false;
                }
                int toAdd = Mathf.Min(stackMax, amount);
                _slots.Add(new InventorySlot
                {
                    Item = item,
                    Amount = toAdd,
                    Device = item.type == ItemType.Device ? DeviceState.CreateDefault() : null,
                    CurrentDurability = item.durability > 0f ? item.durability : -1f
                });
                amount -= toAdd;
                OnItemAdded?.Invoke(item, toAdd);
            }

            OnInventoryChanged?.Invoke();
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
            OnInventoryChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Move a quantity of an item to another inventory, respecting the destination's
        /// stack/weight/capacity limits. Rolls back if the destination cannot take it.
        /// </summary>
        public bool Transfer(ItemDefinition item, int amount, Inventory destination)
        {
            if (item == null || amount <= 0 || destination == null || destination == this) return false;
            if (Count(item) < amount) return false;
            if (!destination.CanAdd(item, amount)) return false;

            if (!Remove(item, amount)) return false;
            if (!destination.Add(item, amount))
            {
                Add(item, amount); // rollback
                return false;
            }
            return true;
        }

        // -----------------------------------------------------------------
        // Equipment
        // -----------------------------------------------------------------

        /// <summary>
        /// Equip one of an equipable item: removes it from storage and places it in its
        /// slot, returning any current occupant of that slot to storage.
        /// </summary>
        public bool Equip(ItemDefinition item)
        {
            if (item == null || !item.isEquipable || item.equipSlot == EquipSlot.None) return false;
            if (Count(item) < 1) return false;

            Unequip(item.equipSlot);

            if (!Remove(item, 1)) return false;
            _equipped.Add(new EquippedItem { Item = item, CurrentDurability = item.durability });
            OnInventoryChanged?.Invoke();
            return true;
        }

        /// <summary>Unequip the item in a slot, returning it to storage. Returns the item or null.</summary>
        public ItemDefinition Unequip(EquipSlot slot)
        {
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped != null && equipped.Item != null && equipped.Item.equipSlot == slot)
                {
                    var item = equipped.Item;
                    _equipped.RemoveAt(i);
                    Add(item, 1);
                    OnInventoryChanged?.Invoke();
                    return item;
                }
            }
            return null;
        }

        /// <summary>The equipped item in a slot, or null.</summary>
        public EquippedItem GetEquipped(EquipSlot slot)
        {
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped != null && equipped.Item != null && equipped.Item.equipSlot == slot)
                {
                    return equipped;
                }
            }
            return null;
        }

        /// <summary>Total rad protection from equipped gear, scaled by each piece's durability.</summary>
        public float GetEquippedProtection()
        {
            float total = 0f;
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped == null || equipped.Item == null) continue;
                float fraction = equipped.Item.durability > 0f
                    ? Mathf.Clamp01(equipped.CurrentDurability / equipped.Item.durability)
                    : 0f;
                total += Mathf.Max(0f, equipped.Item.radProtection) * fraction;
            }
            return Mathf.Max(0f, total);
        }

        /// <summary>
        /// Build the worn-gear list (for RadiationSystem exposure) from equipped protective
        /// items. Equipped items don't auto-degrade per tick (DegradeRate 0); durability is a
        /// damage resource reduced by events.
        /// </summary>
        public List<WornGear> BuildWornGear()
        {
            var list = new List<WornGear>();
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped == null || equipped.Item == null || equipped.Item.radProtection <= 0f) continue;
                list.Add(new WornGear
                {
                    RadProtection = equipped.Item.radProtection,
                    MaxDurability = equipped.Item.durability,
                    CurrentDurability = equipped.CurrentDurability,
                    DegradeRate = 0f
                });
            }
            return list;
        }

        // -----------------------------------------------------------------
        // Consumption
        // -----------------------------------------------------------------

        /// <summary>
        /// Consume one of an item, applying its effects: need changes via NeedsSystem,
        /// rad cleanse / iodine resistance / ingested contamination via RadiationSystem.
        /// Either system may be null to skip that category of effects.
        /// </summary>
        public bool Consume(ItemDefinition item, Survivor survivor, RadiationSystem radiation = null, NeedsSystem needs = null)
        {
            if (item == null || survivor == null || !survivor.IsAlive) return false;
            if (Count(item) < 1) return false;
            if (!Remove(item, 1)) return false;

            if (needs != null)
            {
                needs.Modify(survivor, NeedKind.Hunger, item.hungerRestore);
                needs.Modify(survivor, NeedKind.Thirst, item.thirstRestore);
                needs.Modify(survivor, NeedKind.Health, item.healthEffect);
                needs.Modify(survivor, NeedKind.Morale, item.moraleEffect);
            }

            if (radiation != null)
            {
                if (item.radCleanse > 0f)
                {
                    radiation.AdministerAntiRad(survivor, item.radCleanse);
                }
                if (item.type == ItemType.Iodine)
                {
                    radiation.AdministerIodine(survivor);
                }
                if (item.contamination > 0f)
                {
                    radiation.Expose(survivor, item.contamination * ContaminationDosePerUnit, 1f);
                }
            }

            return true;
        }

        // -----------------------------------------------------------------
        // Save / load
        // -----------------------------------------------------------------

        /// <summary>Snapshot the inventory as item ids + amounts (save/load safe, no asset refs).</summary>
        public InventorySaveState CaptureState()
        {
            var state = new InventorySaveState { capacity = Capacity, maxWeight = MaxWeight };
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (slot != null && slot.Item != null && slot.Amount > 0)
                {
                    var slotSave = new SlotSave { itemId = slot.Item.id, amount = slot.Amount };
                    if (slot.Device != null)
                    {
                        slotSave.hasDevice = true;
                        slotSave.battery = slot.Device.Battery;
                        slotSave.calibration = slot.Device.Calibration;
                        slotSave.broken = slot.Device.Broken;
                        slotSave.lastCalibratedDay = slot.Device.LastCalibratedDay;
                    }
                    state.slots.Add(slotSave);
                }
            }
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped != null && equipped.Item != null)
                {
                    state.equipped.Add(new EquippedSave { itemId = equipped.Item.id, durability = equipped.CurrentDurability });
                }
            }
            return state;
        }

        /// <summary>Rebuild the inventory from a snapshot, resolving item ids via a catalog lookup.</summary>
        public void RestoreState(InventorySaveState state, Func<string, ItemDefinition> lookup)
        {
            if (state == null) return;

            _slots.Clear();
            _equipped.Clear();
            Capacity = state.capacity;
            MaxWeight = state.maxWeight;

            if (state.slots != null)
            {
                foreach (var slotSave in state.slots)
                {
                    var item = lookup?.Invoke(slotSave.itemId);
                    if (item != null && slotSave.amount > 0)
                    {
                        DeviceState device = null;
                        if (item.type == ItemType.Device)
                        {
                            if (slotSave.hasDevice)
                            {
                                device = new DeviceState
                                {
                                    Battery = slotSave.battery,
                                    Calibration = slotSave.calibration,
                                    Broken = slotSave.broken,
                                    LastCalibratedDay = slotSave.lastCalibratedDay
                                };
                                device.Normalize();
                            }
                            else
                            {
                                device = DeviceState.CreateDefault();
                            }
                        }
                        _slots.Add(new InventorySlot { Item = item, Amount = slotSave.amount, Device = device });
                    }
                }
            }

            if (state.equipped != null)
            {
                foreach (var equippedSave in state.equipped)
                {
                    var item = lookup?.Invoke(equippedSave.itemId);
                    if (item != null)
                    {
                        _equipped.Add(new EquippedItem { Item = item, CurrentDurability = equippedSave.durability });
                    }
                }
            }

            OnInventoryChanged?.Invoke();
        }
    }

    /// <summary>A single stack within an inventory.</summary>
    [Serializable]
    public class InventorySlot
    {
        public ItemDefinition Item;
        public int Amount;
        /// <summary>Per-instance reliability for Device items (geiger, dosimeter); null otherwise.</summary>
        public DeviceState Device;
        /// <summary>
        /// Remaining durability for non-device gear/tools in storage (0..Item.durability).
        /// -1 means "full / not tracked" (use Item.durability when needed).
        /// </summary>
        public float CurrentDurability = -1f;

        /// <summary>Resolved durability for workbench repair UI.</summary>
        public float GetDurability()
        {
            if (Item == null) return 0f;
            if (Item.type == ItemType.Device && Device != null)
                return Device.Broken ? 0f : Mathf.Max(0f, Device.Calibration * 100f);
            if (Item.durability <= 0f) return 100f;
            if (CurrentDurability < 0f) return Item.durability;
            return CurrentDurability;
        }

        public bool IsBrokenOrDegraded()
        {
            if (Item == null) return false;
            if (Item.type == ItemType.Device && Device != null)
                return Device.Broken || Device.Calibration < InstrumentDevice.ReliableCalibrationThreshold;
            if (Item.durability <= 0f) return false;
            float d = GetDurability();
            return d < Item.durability * 0.99f;
        }
    }

    /// <summary>An item currently worn in an equipment slot, with its remaining durability.</summary>
    [Serializable]
    public class EquippedItem
    {
        public ItemDefinition Item;
        public float CurrentDurability;
    }

    /// <summary>Serializable inventory snapshot (item ids + amounts), safe for JsonUtility.</summary>
    [Serializable]
    public class InventorySaveState
    {
        public int capacity;
        public float maxWeight;
        public List<SlotSave> slots = new List<SlotSave>();
        public List<EquippedSave> equipped = new List<EquippedSave>();
    }

    [Serializable]
    public class SlotSave
    {
        public string itemId;
        public int amount;
        public bool hasDevice;
        public float battery;
        public float calibration;
        public bool broken;
        public int lastCalibratedDay;
    }

    [Serializable]
    public class EquippedSave
    {
        public string itemId;
        public float durability;
    }
}
