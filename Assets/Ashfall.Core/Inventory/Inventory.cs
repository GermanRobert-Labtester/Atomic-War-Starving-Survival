using System;
using System.Collections.Generic;
#pragma warning disable CS8618

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

    /// <summary>Authoritative worn-gear record consumed by radiation exposure.</summary>
    public class WornGear
    {
        public EquippedItem? SourceEquipped;
        public IEquipmentConditionSink? ConditionSink;
        public ItemDefinition? SourceItem;
        public float RadProtection;
        public float MaxDurability;
        public float CurrentDurability;
        public float DegradeRate;
        public Action<float>? OnDegraded;

        public float DurabilityFraction()
        {
            return MaxDurability > 0f ? Math.Clamp(CurrentDurability / MaxDurability, 0f, 1f) : 0f;
        }

        public float EffectiveProtection()
        {
            return Math.Max(0f, RadProtection) * DurabilityFraction();
        }

        public void Degrade(float gameHours)
        {
            if (gameHours <= 0f) return;
            float loss = DegradeRate * gameHours;
            if (loss <= 0f) return;
            CurrentDurability = Math.Max(0f, CurrentDurability - loss);
            if (ConditionSink != null && SourceEquipped != null)
            {
                ConditionSink.RecordWear(SourceEquipped, loss, "radiation");
            }
            else if (SourceEquipped != null)
            {
                SourceEquipped.CurrentDurability = Math.Max(0f, SourceEquipped.CurrentDurability - loss);
            }
            OnDegraded?.Invoke(loss);
        }

        public static WornGear FromInventory(EquippedGearData src)
        {
            return new WornGear
            {
                RadProtection = src.RadProtection,
                MaxDurability = src.MaxDurability,
                CurrentDurability = src.CurrentDurability,
                DegradeRate = src.DegradeRate
            };
        }

        public static WornGear? FromInventory(WornGear? src)
        {
            if (src == null) return null;
            return new WornGear
            {
                SourceEquipped = src.SourceEquipped,
                ConditionSink = src.ConditionSink,
                SourceItem = src.SourceItem,
                RadProtection = src.RadProtection,
                MaxDurability = src.MaxDurability,
                CurrentDurability = src.CurrentDurability,
                DegradeRate = src.DegradeRate,
                OnDegraded = src.OnDegraded
            };
        }
    }

    public class Inventory : IPlayerInventoryPort, IEquipmentConditionSink
    {
        public const float ContaminationDosePerUnit = 50f;

        public int Capacity = 20;
        public float MaxWeight = 100f;

        private List<InventorySlot> _slots = new List<InventorySlot>();
        private List<EquippedItem> _equipped = new List<EquippedItem>();

        public IReadOnlyList<InventorySlot> Slots => _slots;
        public IReadOnlyList<EquippedItem> Equipped => _equipped;
        public IReadOnlyList<InventorySlot> GetSlots() => _slots;

        public event Action<ItemDefinition, int> OnItemAdded;
        public event Action<ItemDefinition, int> OnItemRemoved;
        public event Action OnInventoryChanged;

        public bool HasSufficient(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return true;
            string canonical = ItemAliases.ToCanonical(itemId);
            return CountById(canonical) >= count || CountById(itemId) >= count;
        }

        public bool TryConsume(string itemId, int count, Action? onCommitted = null)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return true;
            string canonical = ItemAliases.ToCanonical(itemId);
            if (CountById(canonical) < count && CountById(itemId) < count)
                return false;

            string targetId = CountById(canonical) >= count ? canonical : itemId;
            Remove(targetId, count);
            onCommitted?.Invoke();
            return true;
        }

        public bool TryProduce(string itemId, int count, ItemDefinition? def = null)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return true;
            string canonical = ItemAliases.ToCanonical(itemId);
            var itemDef = def ?? new ItemDefinition
            {
                id = canonical,
                displayName = canonical,
                stackMax = 99,
                weight = 1f
            };
            return Add(itemDef, count);
        }

        public bool Remove(string itemId, int amount) => RemoveById(itemId, amount);

        public int Count(ItemDefinition item)
        {
            if (item == null || _slots == null) return 0;
            return CountById(item.id);
        }

        public int CountById(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return 0;
            string canonical = ItemAliases.ToCanonical(itemId);
            int total = 0;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null)
                {
                    string slotId = _slots[i].Item.id;
                    if (slotId == itemId || slotId == canonical || ItemAliases.ToCanonical(slotId) == canonical)
                        total += _slots[i].Amount;
                }
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

        public InventorySlot? FindSlot(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return null;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == itemId)
                    return _slots[i];
            }
            return null;
        }

        public InventorySlot? FindBestWorkingDevice(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _slots == null) return null;
            InventorySlot best = null!;
            DeviceState? bestDevice = null;
            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                if (slot == null || slot.Item == null || slot.Item.id != itemId) continue;
                if (slot.Device == null) continue;
                if (!InstrumentDevice.CanMeasure(slot.Device)) continue;
                if (bestDevice == null
                    || slot.Device.Battery > bestDevice.Battery
                    || (MathfCompat.Approximately(slot.Device.Battery, bestDevice.Battery)
                        && slot.Device.Calibration > bestDevice.Calibration))
                {
                    best = slot;
                    bestDevice = slot.Device;
                }
            }
            return best;
        }

        public bool HasWorkingGeiger()
        {
            return FindBestWorkingDevice("geiger_counter") != null
                || FindBestWorkingDevice("item_geiger_m3") != null;
        }

        public DeviceState? GetBestGeigerState()
        {
            var working = FindBestWorkingDevice("geiger_counter")
                ?? FindBestWorkingDevice("item_geiger_m3");
            if (working != null) return working.Device;
            var any = FindSlot("geiger_counter") ?? FindSlot("item_geiger_m3");
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

        /// <summary>
        /// Add items by ID only, creating a minimal ItemDefinition.
        /// Useful for systems like workshop reverse-engineering that
        /// operate on item IDs without needing full definition metadata.
        /// </summary>
        public bool AddById(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;
            var def = new ItemDefinition { id = itemId, stackMax = 99 };
            bool result = Add(def, amount);
            return result;
        }

        /// <summary>Check if items can be added by ID.</summary>
        public bool CanAddById(string itemId, int amount)
        {
            if (string.IsNullOrEmpty(itemId) || amount <= 0) return false;
            var def = new ItemDefinition { id = itemId, stackMax = 99 };
            return CanAdd(def, amount);
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
            ItemDefinition? removedDef = null;
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

        // ── Atomic Inventory Transaction Boundary ──────────────────────

        /// <summary>
        /// Validates a multi-item transaction (costs and grants) against current inventory state.
        /// Aggregates duplicate item IDs, verifies availability, and checks capacity and weight limits.
        /// </summary>
        public InventoryTransactionValidationResult ValidateTransaction(
            InventoryBill bill,
            Func<string, ItemDefinition?>? lookup = null)
        {
            if (bill == null || bill.IsEmpty)
                return InventoryTransactionValidationResult.Success();

            var aggregatedCosts = bill.GetAggregatedCosts();
            var aggregatedGrants = bill.GetAggregatedGrants();

            // 1. Verify cost availability with aggregated totals
            foreach (var kv in aggregatedCosts)
            {
                string itemId = kv.Key;
                int required = kv.Value;
                if (required <= 0) continue;

                int available = CountById(itemId);
                if (available < required)
                {
                    return InventoryTransactionValidationResult.Insufficient(itemId, required, available);
                }
            }

            // 2. Simulate slot and weight changes after cost removal
            float simulatedWeight = GetCurrentWeight();
            var simulatedSlots = new List<(string id, int amount, int stackMax)>(_slots.Count);

            for (int i = 0; i < _slots.Count; i++)
            {
                var s = _slots[i];
                if (s?.Item != null && s.Amount > 0)
                {
                    int sMax = s.Item.stackMax > 0 ? s.Item.stackMax : 99;
                    simulatedSlots.Add((s.Item.id, s.Amount, sMax));
                }
            }

            // Deduct costs from simulated slots
            foreach (var kv in aggregatedCosts)
            {
                string itemId = kv.Key;
                int remainingToDeduct = kv.Value;

                for (int i = simulatedSlots.Count - 1; i >= 0 && remainingToDeduct > 0; i--)
                {
                    var entry = simulatedSlots[i];
                    if (entry.id == itemId)
                    {
                        var def = ResolveDefinition(itemId, bill, lookup);
                        float itemWeight = def.weight;

                        if (entry.amount <= remainingToDeduct)
                        {
                            simulatedWeight -= itemWeight * entry.amount;
                            remainingToDeduct -= entry.amount;
                            simulatedSlots.RemoveAt(i);
                        }
                        else
                        {
                            simulatedWeight -= itemWeight * remainingToDeduct;
                            simulatedSlots[i] = (entry.id, entry.amount - remainingToDeduct, entry.stackMax);
                            remainingToDeduct = 0;
                        }
                    }
                }
            }

            // 3. Simulate adding grants to check slot capacity and weight limit
            foreach (var kv in aggregatedGrants)
            {
                string itemId = kv.Key;
                int remainingToAdd = kv.Value;
                if (remainingToAdd <= 0) continue;

                var def = ResolveDefinition(itemId, bill, lookup);
                int stackMax = def.stackMax > 0 ? def.stackMax : 99;
                float itemWeight = def.weight;

                simulatedWeight += itemWeight * remainingToAdd;

                // Fill existing simulated slots with available space
                for (int i = 0; i < simulatedSlots.Count && remainingToAdd > 0; i++)
                {
                    var entry = simulatedSlots[i];
                    if (entry.id == itemId && entry.amount < entry.stackMax)
                    {
                        int space = entry.stackMax - entry.amount;
                        int placed = Math.Min(space, remainingToAdd);
                        simulatedSlots[i] = (entry.id, entry.amount + placed, entry.stackMax);
                        remainingToAdd -= placed;
                    }
                }

                // Allocate new slots as needed
                while (remainingToAdd > 0)
                {
                    int toPlace = Math.Min(stackMax, remainingToAdd);
                    simulatedSlots.Add((itemId, toPlace, stackMax));
                    remainingToAdd -= toPlace;
                }
            }

            if (Capacity > 0 && simulatedSlots.Count > Capacity)
            {
                return InventoryTransactionValidationResult.CapacityExceeded(simulatedSlots.Count, Capacity);
            }

            if (MaxWeight > 0f && simulatedWeight > MaxWeight)
            {
                return InventoryTransactionValidationResult.WeightExceeded(GetCurrentWeight(), simulatedWeight - GetCurrentWeight(), MaxWeight);
            }

            return InventoryTransactionValidationResult.Success();
        }

        private ItemDefinition ResolveDefinition(string itemId, InventoryBill? bill, Func<string, ItemDefinition?>? lookup)
        {
            if (lookup != null)
            {
                var def = lookup(itemId);
                if (def != null) return def;
            }

            var existingSlot = FindSlot(itemId);
            if (existingSlot?.Item != null) return existingSlot.Item;

            if (bill != null)
            {
                for (int i = 0; i < bill.Grants.Count; i++)
                {
                    if (bill.Grants[i].ItemId == itemId && bill.Grants[i].Definition != null)
                        return bill.Grants[i].Definition!;
                }
                for (int i = 0; i < bill.Costs.Count; i++)
                {
                    if (bill.Costs[i].ItemId == itemId && bill.Costs[i].Definition != null)
                        return bill.Costs[i].Definition!;
                }
            }

            return new ItemDefinition { id = itemId, stackMax = 99 };
        }

        /// <summary>
        /// Generates a comprehensive quote for a proposed bill, detailing weights, aggregated costs/grants, and validation.
        /// </summary>
        public InventoryTransactionQuote Quote(
            InventoryBill bill,
            Func<string, ItemDefinition?>? lookup = null)
        {
            if (bill == null) bill = new InventoryBill();

            var aggregatedCosts = bill.GetAggregatedCosts();
            var aggregatedGrants = bill.GetAggregatedGrants();

            float totalCostWeight = 0f;
            foreach (var kv in aggregatedCosts)
            {
                var def = ResolveDefinition(kv.Key, bill, lookup);
                float unitWeight = def != null ? def.weight : 0f;
                totalCostWeight += unitWeight * kv.Value;
            }

            float totalGrantWeight = 0f;
            foreach (var kv in aggregatedGrants)
            {
                var def = ResolveDefinition(kv.Key, bill, lookup);
                float unitWeight = def != null ? def.weight : 0f;
                totalGrantWeight += unitWeight * kv.Value;
            }

            var validation = ValidateTransaction(bill, lookup);
            return new InventoryTransactionQuote(
                bill, aggregatedCosts, aggregatedGrants, totalCostWeight, totalGrantWeight, validation);
        }

        public InventoryTransactionQuote QuoteTransaction(
            InventoryBill bill,
            Func<string, ItemDefinition?>? lookup = null)
        {
            return Quote(bill, lookup);
        }

        /// <summary>
        /// Begins an active atomic transaction with snapshot isolation and rollback guarantees.
        /// </summary>
        public InventoryTransaction BeginTransaction(
            InventoryBill bill,
            Func<string, ItemDefinition?>? lookup = null)
        {
            if (bill == null) bill = new InventoryBill();
            var validation = ValidateTransaction(bill, lookup);
            var snapshot = new InventorySnapshot(Capacity, MaxWeight, _slots, _equipped);
            return new InventoryTransaction(this, bill, validation, snapshot);
        }

        /// <summary>
        /// Atomically executes a transaction (consumes costs and adds grants).
        /// If validation fails or the post-commit callback throws an exception, rolls back completely.
        /// Fires OnInventoryChanged exactly once upon successful completion.
        /// </summary>
        public bool TryExecuteTransaction(
            InventoryBill bill,
            Action? onCommitted = null,
            Func<string, ItemDefinition?>? lookup = null)
        {
            using var tx = BeginTransaction(bill, lookup);
            if (!tx.Validation.IsValid) return false;
            return tx.TryCommit(onCommitted);
        }

        /// <summary>
        /// Atomically consumes a multi-item cost bill. Returns false without modifying inventory if any item is insufficient.
        /// Fires OnInventoryChanged exactly once on success.
        /// </summary>
        public bool TryConsumeBill(IReadOnlyDictionary<string, int> costs, Action? onCommitted = null)
        {
            return TryExecuteTransaction(InventoryBill.FromCosts(costs), onCommitted);
        }

        /// <summary>
        /// Atomically consumes a multi-item cost collection.
        /// </summary>
        public bool TryConsumeBill(IEnumerable<KeyValuePair<string, int>> costs, Action? onCommitted = null)
        {
            return TryExecuteTransaction(InventoryBill.FromCosts(costs), onCommitted);
        }

        /// <summary>
        /// Atomically consumes a list of item IDs (1 of each).
        /// </summary>
        public bool TryConsumeBill(IEnumerable<string> itemIds, Action? onCommitted = null)
        {
            return TryExecuteTransaction(InventoryBill.FromCosts(itemIds), onCommitted);
        }

        internal void ApplyTransactionMutations(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null)
        {
            if (bill == null || bill.IsEmpty) return;

            var aggregatedCosts = bill.GetAggregatedCosts();
            var aggregatedGrants = bill.GetAggregatedGrants();

            // 1. Remove costs (silently during atomic batch)
            foreach (var kv in aggregatedCosts)
            {
                string itemId = kv.Key;
                int remaining = kv.Value;

                for (int i = _slots.Count - 1; i >= 0 && remaining > 0; i--)
                {
                    if (_slots[i] == null || _slots[i].Item == null) continue;
                    if (_slots[i].Item.id != itemId) continue;

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
            }

            // 2. Add grants (silently during atomic batch)
            foreach (var kv in aggregatedGrants)
            {
                string itemId = kv.Key;
                int amount = kv.Value;
                if (amount <= 0) continue;

                var def = ResolveDefinition(itemId, bill, lookup);
                int stackMax = def.stackMax > 0 ? def.stackMax : 99;

                for (int i = 0; i < _slots.Count && amount > 0; i++)
                {
                    if (_slots[i] != null && _slots[i].Item != null && _slots[i].Item.id == itemId)
                    {
                        int space = stackMax - _slots[i].Amount;
                        if (space > 0)
                        {
                            int toAdd = Math.Min(space, amount);
                            _slots[i].Amount += toAdd;
                            amount -= toAdd;
                        }
                    }
                }

                while (amount > 0)
                {
                    int toAdd = Math.Min(stackMax, amount);
                    _slots.Add(new InventorySlot
                    {
                        Item = def,
                        Amount = toAdd,
                        Device = def.type == ItemType.Device ? DeviceState.CreateDefault() : null,
                        CurrentDurability = def.durability > 0f ? def.durability : -1f
                    });
                    amount -= toAdd;
                }
            }
        }

        internal void NotifyTransactionCommitted(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null)
        {
            if (bill == null || bill.IsEmpty) return;

            var aggregatedCosts = bill.GetAggregatedCosts();
            var aggregatedGrants = bill.GetAggregatedGrants();

            if (OnItemRemoved != null)
            {
                foreach (var kv in aggregatedCosts)
                {
                    var def = ResolveDefinition(kv.Key, bill, lookup);
                    OnItemRemoved.Invoke(def, kv.Value);
                }
            }

            if (OnItemAdded != null)
            {
                foreach (var kv in aggregatedGrants)
                {
                    var def = ResolveDefinition(kv.Key, bill, lookup);
                    OnItemAdded.Invoke(def, kv.Value);
                }
            }

            OnInventoryChanged?.Invoke();
        }

        internal void RestoreSnapshot(InventorySnapshot snapshot)
        {
            if (snapshot == null) return;
            _slots.Clear();
            _equipped.Clear();
            Capacity = snapshot.Capacity;
            MaxWeight = snapshot.MaxWeight;

            for (int i = 0; i < snapshot.Slots.Count; i++)
            {
                var s = snapshot.Slots[i];
                if (s == null) continue;
                _slots.Add(new InventorySlot
                {
                    Item = s.Item,
                    Amount = s.Amount,
                    CurrentDurability = s.CurrentDurability,
                    Device = s.Device != null ? new DeviceState
                    {
                        Battery = s.Device.Battery,
                        Calibration = s.Device.Calibration,
                        Broken = s.Device.Broken,
                        LastCalibratedDay = s.Device.LastCalibratedDay
                    } : null
                });
            }

            for (int i = 0; i < snapshot.Equipped.Count; i++)
            {
                var e = snapshot.Equipped[i];
                if (e == null) continue;
                _equipped.Add(new EquippedItem
                {
                    Item = e.Item,
                    CurrentDurability = e.CurrentDurability
                });
            }
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

        public ItemDefinition? Unequip(EquipSlot slot)
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

        public EquippedItem? GetEquipped(EquipSlot slot)
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

        /// <summary>
        /// Assembles a read projection of equipped protective gear for exposure and simulation calculations.
        /// Wear and degradation are written back to canonical EquippedItem instances through the
        /// IEquipmentConditionSink interface.
        /// </summary>
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
                    SourceEquipped = equipped,
                    ConditionSink = this,
                    SourceItem = equipped.Item,
                    RadProtection = equipped.Item.radProtection,
                    MaxDurability = equipped.Item.durability,
                    CurrentDurability = equipped.CurrentDurability,
                    DegradeRate = Math.Clamp(equipped.Item.GetEffectiveDegradeRate(), 0f, 100f)
                });
            }
        }

        public void RecordWear(EquippedItem item, float wearDelta, string cause = "radiation")
        {
            if (item == null || wearDelta <= 0f) return;
            item.CurrentDurability = Math.Max(0f, item.CurrentDurability - wearDelta);
            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Degrades all equipped protective items once by the specified hours and optional multiplier.
        /// </summary>
        public void DegradeEquippedGear(float gameHours, float multiplier = 1f)
        {
            if (gameHours <= 0f || multiplier <= 0f) return;
            bool changed = false;
            for (int i = 0; i < _equipped.Count; i++)
            {
                var equipped = _equipped[i];
                if (equipped?.Item == null) continue;
                float rate = equipped.Item.GetEffectiveDegradeRate();
                if (rate <= 0f) continue;
                float loss = rate * gameHours * multiplier;
                if (loss > 0f)
                {
                    equipped.CurrentDurability = Math.Max(0f, equipped.CurrentDurability - loss);
                    changed = true;
                }
            }
            if (changed)
                OnInventoryChanged?.Invoke();
        }

        /// <summary>Consume one unit, applying effects via optional needs/radiation callbacks.</summary>
        public bool Consume(
            ItemDefinition item,
            Func<ItemType, float, bool>? applyNeed = null,
            Action<float>? applyRadCleanse = null,
            Action? applyIodine = null,
            Action<float>? applyContamination = null,
            float therapeuticScale = 1f)
        {
            if (item == null) return false;
            if (Count(item) < 1) return false;
            if (!Remove(item, 1)) return false;

            float scale = MathfCompat.Clamp01(therapeuticScale);

            try
            {
                if (applyNeed != null)
                {
                    if (item.hungerRestore != 0f && !applyNeed(ItemType.Food, -item.hungerRestore))
                    {
                        Add(item, 1);
                        return false;
                    }
                    if (item.thirstRestore != 0f && !applyNeed(ItemType.Water, -item.thirstRestore))
                    {
                        Add(item, 1);
                        return false;
                    }
                    if (item.healthEffect != 0f && !applyNeed(ItemType.Medical, item.healthEffect * scale))
                    {
                        Add(item, 1);
                        return false;
                    }
                    if (item.moraleEffect != 0f && !applyNeed(ItemType.Comfort, item.moraleEffect))
                    {
                        Add(item, 1);
                        return false;
                    }
                }

                if (item.radCleanse > 0f && scale > 0f)
                    applyRadCleanse?.Invoke(item.radCleanse * scale);
                if (item.type == ItemType.Iodine)
                    applyIodine?.Invoke();
                if (item.contamination > 0f)
                    applyContamination?.Invoke(item.contamination * ContaminationDosePerUnit);
                return true;
            }
            catch
            {
                Add(item, 1);
                throw;
            }
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

        public void RestoreState(InventorySaveState state, Func<string, ItemDefinition?> lookup)
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
                        DeviceState? device = null;
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
        public DeviceState? Device;
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
