// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Represents an immutable bill entry specifying an item ID or definition and required/granted quantity.
    /// </summary>
    public readonly struct InventoryBillItem : IEquatable<InventoryBillItem>
    {
        public string ItemId { get; }
        public int Amount { get; }
        public ItemDefinition? Definition { get; }

        public InventoryBillItem(string itemId, int amount, ItemDefinition? definition = null)
        {
            if (string.IsNullOrWhiteSpace(itemId) && definition == null)
                throw new ArgumentException("ItemId or Definition must be provided.", nameof(itemId));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be strictly positive (> 0).");

            ItemId = !string.IsNullOrEmpty(itemId) ? itemId : definition!.id;
            Amount = amount;
            Definition = definition;
        }

        public InventoryBillItem(ItemDefinition definition, int amount)
            : this(definition?.id ?? throw new ArgumentNullException(nameof(definition)), amount, definition)
        {
        }

        public bool Equals(InventoryBillItem other) =>
            string.Equals(ItemId, other.ItemId, StringComparison.Ordinal) && Amount == other.Amount;

        public override bool Equals(object? obj) => obj is InventoryBillItem other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(ItemId, Amount);
        public override string ToString() => $"{Amount}x {ItemId}";
    }

    /// <summary>
    /// Represents an atomic multi-item bill specifying items to consume (costs) and items to grant (grants/yields).
    /// Duplicate item IDs are aggregated automatically before checking availability or executing mutations.
    /// </summary>
    public sealed class InventoryBill
    {
        private readonly List<InventoryBillItem> _costs = new();
        private readonly List<InventoryBillItem> _grants = new();

        public IReadOnlyList<InventoryBillItem> Costs => _costs;
        public IReadOnlyList<InventoryBillItem> Grants => _grants;

        public bool IsEmpty => _costs.Count == 0 && _grants.Count == 0;

        public InventoryBill AddCost(string itemId, int amount, ItemDefinition? def = null)
        {
            if (!string.IsNullOrEmpty(itemId) && amount > 0)
                _costs.Add(new InventoryBillItem(itemId, amount, def));
            return this;
        }

        public InventoryBill AddCost(ItemDefinition def, int amount)
        {
            if (def != null && amount > 0)
                _costs.Add(new InventoryBillItem(def, amount));
            return this;
        }

        public InventoryBill AddGrant(string itemId, int amount, ItemDefinition? def = null)
        {
            if (!string.IsNullOrEmpty(itemId) && amount > 0)
                _grants.Add(new InventoryBillItem(itemId, amount, def));
            return this;
        }

        public InventoryBill AddGrant(ItemDefinition def, int amount)
        {
            if (def != null && amount > 0)
                _grants.Add(new InventoryBillItem(def, amount));
            return this;
        }

        /// <summary>
        /// Aggregates all cost items by item ID, summing quantities for duplicate IDs.
        /// </summary>
        public Dictionary<string, int> GetAggregatedCosts()
        {
            var map = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < _costs.Count; i++)
            {
                string id = _costs[i].ItemId;
                map.TryGetValue(id, out int current);
                map[id] = current + _costs[i].Amount;
            }
            return map;
        }

        /// <summary>
        /// Aggregates all grant items by item ID, summing quantities for duplicate IDs.
        /// </summary>
        public Dictionary<string, int> GetAggregatedGrants()
        {
            var map = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < _grants.Count; i++)
            {
                string id = _grants[i].ItemId;
                map.TryGetValue(id, out int current);
                map[id] = current + _grants[i].Amount;
            }
            return map;
        }

        public static InventoryBill FromCosts(IReadOnlyDictionary<string, int> costs)
        {
            var bill = new InventoryBill();
            if (costs != null)
            {
                foreach (var kv in costs)
                {
                    if (kv.Value > 0) bill.AddCost(kv.Key, kv.Value);
                }
            }
            return bill;
        }

        public static InventoryBill FromCosts(IEnumerable<KeyValuePair<string, int>> costs)
        {
            var bill = new InventoryBill();
            if (costs != null)
            {
                foreach (var kv in costs)
                {
                    if (kv.Value > 0) bill.AddCost(kv.Key, kv.Value);
                }
            }
            return bill;
        }

        public static InventoryBill FromCosts(IEnumerable<string> itemIds)
        {
            var bill = new InventoryBill();
            if (itemIds != null)
            {
                foreach (var id in itemIds)
                {
                    if (!string.IsNullOrEmpty(id)) bill.AddCost(id, 1);
                }
            }
            return bill;
        }

        public static InventoryBill FromCostsAndGrants(
            IReadOnlyDictionary<string, int>? costs,
            IReadOnlyDictionary<string, int>? grants)
        {
            var bill = new InventoryBill();
            if (costs != null)
            {
                foreach (var kv in costs)
                    if (kv.Value > 0) bill.AddCost(kv.Key, kv.Value);
            }
            if (grants != null)
            {
                foreach (var kv in grants)
                    if (kv.Value > 0) bill.AddGrant(kv.Key, kv.Value);
            }
            return bill;
        }
    }

    public enum InventoryTransactionStatus
    {
        Success = 0,
        MissingItem = 1,
        InsufficientQuantity = 2,
        ExceedsCapacity = 3,
        ExceedsWeight = 4,
        InvalidArguments = 5,
        Cancelled = 6,
        CallbackFailed = 7
    }

    public sealed class InventoryTransactionValidationResult
    {
        public bool IsValid => Status == InventoryTransactionStatus.Success;
        public InventoryTransactionStatus Status { get; }
        public string FailureReason { get; }
        public string FailedItemId { get; }
        public int RequiredAmount { get; }
        public int AvailableAmount { get; }

        private InventoryTransactionValidationResult(
            InventoryTransactionStatus status,
            string failureReason,
            string failedItemId = "",
            int requiredAmount = 0,
            int availableAmount = 0)
        {
            Status = status;
            FailureReason = failureReason;
            FailedItemId = failedItemId;
            RequiredAmount = requiredAmount;
            AvailableAmount = availableAmount;
        }

        public static InventoryTransactionValidationResult Success() =>
            new(InventoryTransactionStatus.Success, string.Empty);

        public static InventoryTransactionValidationResult Insufficient(string itemId, int required, int available) =>
            new(InventoryTransactionStatus.InsufficientQuantity,
                $"Insufficient items: '{itemId}' requires {required}, but only {available} available.",
                itemId, required, available);

        public static InventoryTransactionValidationResult CapacityExceeded(int requiredSlots, int availableSlots) =>
            new(InventoryTransactionStatus.ExceedsCapacity,
                $"Inventory capacity exceeded: requires {requiredSlots} additional slot(s), only {availableSlots} available.");

        public static InventoryTransactionValidationResult WeightExceeded(float currentWeight, float addedWeight, float maxWeight) =>
            new(InventoryTransactionStatus.ExceedsWeight,
                $"Inventory weight exceeded: current {currentWeight:F1} + added {addedWeight:F1} exceeds max {maxWeight:F1}.");

        public static InventoryTransactionValidationResult Invalid(string reason) =>
            new(InventoryTransactionStatus.InvalidArguments, reason);

        public static InventoryTransactionValidationResult Cancelled() =>
            new(InventoryTransactionStatus.Cancelled, "Transaction was explicitly cancelled.");

        public static InventoryTransactionValidationResult CallbackError(string message) =>
            new(InventoryTransactionStatus.CallbackFailed, message);

        public override string ToString() => IsValid ? "Valid" : $"{Status}: {FailureReason}";
    }

    public sealed class InventoryTransactionQuote
    {
        public InventoryBill Bill { get; }
        public IReadOnlyDictionary<string, int> AggregatedCosts { get; }
        public IReadOnlyDictionary<string, int> AggregatedGrants { get; }
        public float TotalCostWeight { get; }
        public float TotalGrantWeight { get; }
        public float NetWeightChange => TotalGrantWeight - TotalCostWeight;
        public InventoryTransactionValidationResult Validation { get; }
        public bool CanExecute => Validation.IsValid;

        public InventoryTransactionQuote(
            InventoryBill bill,
            IReadOnlyDictionary<string, int> costs,
            IReadOnlyDictionary<string, int> grants,
            float costWeight,
            float grantWeight,
            InventoryTransactionValidationResult validation)
        {
            Bill = bill;
            AggregatedCosts = costs;
            AggregatedGrants = grants;
            TotalCostWeight = costWeight;
            TotalGrantWeight = grantWeight;
            Validation = validation;
        }
    }

    internal sealed class InventorySnapshot
    {
        public int Capacity { get; }
        public float MaxWeight { get; }
        public List<InventorySlot> Slots { get; }
        public List<EquippedItem> Equipped { get; }

        public InventorySnapshot(int capacity, float maxWeight, List<InventorySlot> slots, List<EquippedItem> equipped)
        {
            Capacity = capacity;
            MaxWeight = maxWeight;
            Slots = new List<InventorySlot>(slots.Count);
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (s == null) continue;
                Slots.Add(new InventorySlot
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
            Equipped = new List<EquippedItem>(equipped.Count);
            for (int i = 0; i < equipped.Count; i++)
            {
                var e = equipped[i];
                if (e == null) continue;
                Equipped.Add(new EquippedItem
                {
                    Item = e.Item,
                    CurrentDurability = e.CurrentDurability
                });
            }
        }
    }

    /// <summary>
    /// Active transaction handle providing stage, commit, and rollback guarantees.
    /// Implements IDisposable to ensure uncommitted transactions roll back automatically.
    /// </summary>
    public sealed class InventoryTransaction : IDisposable
    {
        private readonly Inventory _inventory;
        private readonly InventorySnapshot _initialSnapshot;
        private bool _isCommitted;
        private bool _isCancelled;

        public InventoryBill Bill { get; }
        public InventoryTransactionValidationResult Validation { get; }
        public bool IsCommitted => _isCommitted;
        public bool IsCancelled => _isCancelled;
        public bool IsActive => !_isCommitted && !_isCancelled;

        internal InventoryTransaction(
            Inventory inventory,
            InventoryBill bill,
            InventoryTransactionValidationResult validation,
            InventorySnapshot initialSnapshot)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            Bill = bill ?? throw new ArgumentNullException(nameof(bill));
            Validation = validation ?? throw new ArgumentNullException(nameof(validation));
            _initialSnapshot = initialSnapshot ?? throw new ArgumentNullException(nameof(initialSnapshot));
        }

        /// <summary>
        /// Commits the atomic transaction (removes costs and adds grants).
        /// If an optional domain callback is supplied and throws an exception,
        /// the transaction immediately rolls back and re-throws the error.
        /// Fires inventory change events exactly once upon successful completion.
        /// </summary>
        public bool TryCommit(Action? onCommitted = null)
        {
            if (!IsActive || !Validation.IsValid) return false;

            try
            {
                _inventory.ApplyTransactionMutations(Bill);

                if (onCommitted != null)
                {
                    onCommitted();
                }

                _isCommitted = true;
                _inventory.NotifyTransactionCommitted(Bill);
                return true;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Cancel()
        {
            if (_isCommitted) return;
            Rollback();
            _isCancelled = true;
        }

        private void Rollback()
        {
            if (_isCommitted) return;
            _inventory.RestoreSnapshot(_initialSnapshot);
            _isCancelled = true;
        }

        public void Dispose()
        {
            if (!_isCommitted && !_isCancelled)
            {
                Cancel();
            }
        }
    }
}
