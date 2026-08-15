using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Runtime item container holding stacks keyed by ItemDefinition, with stack and
    /// weight limits, equipment slots, and item consumption. Save/load safe via
    /// CaptureState/RestoreState (serializes item ids + amounts, not refs).
    /// Ported engine-agnostic from Unity's AtomicWar._Game.Inventory.Inventory.
    /// </summary>
    [Serializable]
    public struct EquippedGearData
    {
        public float RadProtection;
        public float MaxDurability;
        public float CurrentDurability;
        public float DegradeRate;
    }

    /// <summary>Ported worn-gear record consumed by radiation exposure.</summary>
    public class WornGear
    {
        public float RadProtection;
        public float MaxDurability;
        public float CurrentDurability;
        public float DegradeRate;
    }

    public class Inventory
    {
        public const float ContaminationDosePerUnit = 50f;

        public int Capacity = 20;
        public float MaxWeight = 100f;

        private List<InventorySlot> _slots = new List<InventorySlot>();
        private List<EquippedItem> _equipped = new List<EquippedItem>();

        public IReadOnlyList<InventorySlot> Slots => _slots;
        public IReadOnlyList<EquippedItem> Equipped => _equipped;

        public event Action<ItemDefinition, int> OnItemAdded;
        public event Action<ItemDefinition, int> OnItemRemoved;
        public event Action OnInventoryChanged;

        public int Count(ItemDefinition item)
        {
            if (item == null || _slots == null) return 0;
            return CountById(item.id);
        }

        public int CountById(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return 0;
            int total = 0;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == itemId)
                    total += _slots[i].Amount;
            }
            return total;
        }

        public int CountByType(ItemType type)
        {
            if (_slots == null) return 0;
            int total = 0;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.type == type)
                    total += _slots[i].Amount;
            }
            return total;
        }

        public int RemoveByType(ItemType type, int amount)
        {
            if (amount <= 0 || _slots == null) return 0;
            int remaining = amount;
            int removed = 0;
            for (int i = _slots.Count - 1; i >= 0 && remaining > 0; i--)
            {
                var slot = _slots[i];
                if (slot == null || slot.Item == null || slot.Item.type != type) continue;
                int take = MathfCompat.Min(slot.Amount, remaining);
                var def = slot.Item;
                if (slot.Amount <= take)
                {
                    remaining -= slot.Amount;
                    removed += slot.Amount;
                    _slots.RemoveAt(i);
                }
                else
                {
                    slot.Amount -= take;
                    remaining -= take;
                    removed += take;
                }
                OnItemRemoved?.Invoke(def, take);
            }
            if (removed > 0)
                OnInventoryChanged?.Invoke();
            return removed;
        }

        public float FoodFillRatio()
        {
            int cap = MathfCompat.Max(1, Capacity);
            return MathfCompat.Clamp01(CountByType(ItemType.Food) / (float)cap);
        }

        public float WaterFillRatio()
        {
            int cap = MathfCompat.Max(1, Capacity);
            int units = CountByType(ItemType.Water) + CountByType(ItemType.IrradiatedWater);
            return MathfCompat.Clamp01(units / (float)cap);
        }

        public float FuelFillRatio()
        {
            int cap = MathfCompat.Max(1, Capacity);
            return MathfCompat.Clamp01(CountByType(ItemType.Fuel) / (float)cap);
        }

        public InventorySlot FindSlot(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return null;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == itemId)
                    return _slots[i];
            }
            return null;
        }

        public InventorySlot FindBestWorkingDevice(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return null;
            InventorySlot best = null;
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (slot == null || slot.Item == null || slot.Item.id != itemId) continue;
                if (slot.Device == null) continue;
                if (!InstrumentDevice.CanMeasure(slot.Device)) continue;
                if (best == null
                    || slot.Device.Battery > best.Device.Battery
                    || (MathfCompat.Approximately(slot.Device.Battery, best.Device.Battery)
                        && slot.Device.Calibration > best.Device.Calibration))
                {
                    best = slot;
                }
            }
            return best;
        }

        public bool HasWorkingGeiger() => FindBestWorkingDevice("geiger_counter") != null;

        public DeviceState GetBestGeigerState()
        {
            var working = FindBestWorkingDevice("geiger_counter");
            if (working != null) return working.Device;
            var any = FindSlot("geiger_counter");
            return any?.Device;
        }

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

        public float GetCurrentWeight()
        {
            float total = 0f;
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (slot != null && slot.Item != null)
                    total += slot.Item.weight * slot.Amount;
            }
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped != null && equipped.Item != null)
                    total += equipped.Item.weight;
            }
            return total;
        }

        public bool CanAdd(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;
            if (MaxWeight > 0f && GetCurrentWeight() + item.weight * amount > MaxWeight) return false;
            int stackMax = item.stackMax > 0 ? item.stackMax : 99;
            int remaining = amount;
            for (int i = 0; i < _slots.Count && remaining > 0; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                    remaining -= MathfCompat.Min(stackMax - _slots[i].Amount, remaining);
            }
            int newStacksNeeded = (remaining + stackMax - 1) / stackMax;
            if (Capacity > 0 && _slots.Count + newStacksNeeded > Capacity) return false;
            return true;
        }

        public bool Add(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;
            if (!CanAdd(item, amount)) return false;
            int stackMax = item.stackMax > 0 ? item.stackMax : 99;

            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    int space = stackMax - _slots[i].Amount;
                    if (space > 0)
                    {
                        int toAdd = MathfCompat.Min(space, amount);
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
                int toAdd = MathfCompat.Min(stackMax, amount);
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

        public bool Remove(ItemDefinition item, int amount)
        {
            if (item == null || amount <= 0) return false;
            if (Count(item) < amount) return false;
            int remaining = amount;
            ItemDefinition removedDef = item;
            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    removedDef = _slots[i].Item;
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
            OnItemRemoved?.Invoke(removedDef, amount);
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool RemoveById(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;
            if (CountById(itemId) < amount) return false;
            int remaining = amount;
            ItemDefinition removedDef = null;
            for (int i = _slots.Count - 1; i >= 0 && remaining > 0; i--)
            {
                if (_slots[i] == null || _slots[i].Item == null) continue;
                if (_slots[i].Item.id != itemId) continue;
                removedDef = _slots[i].Item;
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
            }
            if (removedDef != null)
                OnItemRemoved?.Invoke(removedDef, amount);
            OnInventoryChanged?.Invoke();
            return true;
        }

        public void Clear()
        {
            _slots.Clear();
            _equipped.Clear();
            OnInventoryChanged?.Invoke();
        }

        public bool Transfer(ItemDefinition item, int amount, Inventory destination)
        {
            if (item == null || amount <= 0 || destination == null || destination == this) return false;
            if (Count(item) < amount) return false;
            if (!destination.CanAdd(item, amount)) return false;
            if (!Remove(item, amount)) return false;
            if (!destination.Add(item, amount))
            {
                Add(item, amount);
                return false;
            }
            return true;
        }

        // ── Equipment ──────────────────────────────────────────────────

        public bool Equip(ItemDefinition item)
        {
            if (item == null || !item.isEquipable || item.equipSlot == EquipSlot.None) return false;
            if (Count(item) < 1) return false;
            if (GetEquipped(item.equipSlot) != null && Unequip(item.equipSlot) == null)
                return false;
            if (!Remove(item, 1)) return false;
            _equipped.Add(new EquippedItem { Item = item, CurrentDurability = item.durability });
            OnInventoryChanged?.Invoke();
            return true;
        }

        public ItemDefinition Unequip(EquipSlot slot)
        {
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped != null && equipped.Item != null && equipped.Item.equipSlot == slot)
                {
                    var item = equipped.Item;
                    if (!CanAdd(item, 1)) return null;
                    _equipped.RemoveAt(i);
                    Add(item, 1);
                    OnInventoryChanged?.Invoke();
                    return item;
                }
            }
            return null;
        }

        public bool TryUnequipTo(EquipSlot slot, Inventory destination)
        {
            if (destination == null || ReferenceEquals(destination, this)) return false;
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped == null || equipped.Item == null || equipped.Item.equipSlot != slot)
                    continue;
                var item = equipped.Item;
                if (!destination.CanAdd(item, 1)) return false;
                _equipped.RemoveAt(i);
                if (!destination.Add(item, 1))
                {
                    _equipped.Insert(i, equipped);
                    return false;
                }
                OnInventoryChanged?.Invoke();
                return true;
            }
            return false;
        }

        public EquippedItem GetEquipped(EquipSlot slot)
        {
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped != null && equipped.Item != null && equipped.Item.equipSlot == slot)
                    return equipped;
            }
            return null;
        }

        public float GetEquippedProtection()
        {
            float total = 0f;
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped == null || equipped.Item == null) continue;
                float fraction = equipped.Item.durability > 0f
                    ? MathfCompat.Clamp01(equipped.CurrentDurability / equipped.Item.durability)
                    : 0f;
                total += MathfCompat.Max(0f, equipped.Item.radProtection) * fraction;
            }
            return MathfCompat.Max(0f, total);
        }

        public List<WornGear> BuildWornGear()
        {
            var list = new List<WornGear>();
            FillWornGear(list);
            return list;
        }

        public void FillWornGear(List<WornGear> buffer)
        {
            if (buffer == null) return;
            buffer.Clear();
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped == null || equipped.Item == null || equipped.Item.radProtection <= 0f) continue;
                buffer.Add(new WornGear
                {
                    RadProtection = equipped.Item.radProtection,
                    MaxDurability = equipped.Item.durability,
                    CurrentDurability = equipped.CurrentDurability,
                    DegradeRate = 0f
                });
            }
        }

        /// <summary>Consume one unit, applying effects via optional needs/radiation callbacks.</summary>
        public bool Consume(
            ItemDefinition item,
            Func<ItemType, float, bool> applyNeed = null,
            Action<float> applyRadCleanse = null,
            Action applyIodine = null,
            Action<float> applyContamination = null,
            float therapeuticScale = 1f)
        {
            if (item == null) return false;
            if (Count(item) < 1) return false;
            if (!Remove(item, 1)) return false;

            float scale = MathfCompat.Clamp01(therapeuticScale);

            applyNeed?.Invoke(ItemType.Food, -item.hungerRestore);
            applyNeed?.Invoke(ItemType.Water, -item.thirstRestore);
            applyNeed?.Invoke(ItemType.Medical, item.healthEffect * scale);
            applyNeed?.Invoke(ItemType.Comfort, item.moraleEffect);

            if (item.radCleanse > 0f && scale > 0f)
                applyRadCleanse?.Invoke(item.radCleanse * scale);
            if (item.type == ItemType.Iodine)
                applyIodine?.Invoke();
            if (item.contamination > 0f)
                applyContamination?.Invoke(item.contamination * ContaminationDosePerUnit);
            return true;
        }

        // ── Save / load ────────────────────────────────────────────────

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
                    state.equipped.Add(new EquippedSave { itemId = equipped.Item.id, durability = equipped.CurrentDurability });
            }
            return state;
        }

        public void ResortSlotsByType()
        {
            if (_slots == null || _slots.Count <= 1) return;
            _slots.Sort((a, b) =>
            {
                if (a?.Item == null && b?.Item == null) return 0;
                if (a?.Item == null) return 1;
                if (b?.Item == null) return -1;
                int typeCmp = ((int)a.Item.type).CompareTo((int)b.Item.type);
                if (typeCmp != 0) return typeCmp;
                return string.CompareOrdinal(a.Item.id ?? string.Empty, b.Item.id ?? string.Empty);
            });
            OnInventoryChanged?.Invoke();
        }

        public bool IsSortedByType()
        {
            if (_slots == null || _slots.Count <= 1) return true;
            for (int i = 1; i < _slots.Count; i++)
            {
                var prev = _slots[i - 1];
                var cur = _slots[i];
                if (prev?.Item == null || cur?.Item == null) continue;
                int typeCmp = ((int)prev.Item.type).CompareTo((int)cur.Item.type);
                if (typeCmp > 0) return false;
                if (typeCmp == 0
                    && string.CompareOrdinal(prev.Item.id ?? string.Empty, cur.Item.id ?? string.Empty) > 0)
                    return false;
            }
            return true;
        }

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
                        _equipped.Add(new EquippedItem { Item = item, CurrentDurability = equippedSave.durability });
                }
            }
            OnInventoryChanged?.Invoke();
        }
    }

    [Serializable]
    public class InventorySlot
    {
        public ItemDefinition Item;
        public int Amount;
        public DeviceState Device;
        public float CurrentDurability = -1f;

        public float GetDurability()
        {
            if (Item == null) return 0f;
            if (Item.type == ItemType.Device && Device != null)
                return Device.Broken ? 0f : MathfCompat.Max(0f, Device.Calibration * 100f);
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

    [Serializable]
    public class EquippedItem
    {
        public ItemDefinition Item;
        public float CurrentDurability;
    }

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
