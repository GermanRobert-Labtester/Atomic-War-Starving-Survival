using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core
{
    public enum HoldfastTradeFailure
    {
        None,
        InvalidQuantity,
        InsufficientFunds,
        InsufficientStock,
        InsufficientInventory,
        UnknownItem,
        UnknownFaction,
        UnavailableOrRestricted,
        InventoryCapacity,
        InvalidPrice
    }

    public sealed class HoldfastTradeInventorySlot
    {
        public HoldfastItemDefinition Item { get; }
        public int Amount { get; }

        public HoldfastTradeInventorySlot(HoldfastItemDefinition item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    public sealed class HoldfastTradeInventory
    {
        public int Capacity { get; set; } = 20;
        public float MaxWeight { get; set; } = 100f;
        private readonly Dictionary<string, int> _items = new Dictionary<string, int>(StringComparer.Ordinal);
        private readonly HoldfastCatalog _catalog;

        public HoldfastTradeInventory(HoldfastCatalog catalog = null)
        {
            _catalog = catalog;
        }

        public int OccupiedCount => _items.Count;
        public IReadOnlyDictionary<string, int> Items => _items;

        public IReadOnlyList<HoldfastTradeInventorySlot> Slots
        {
            get
            {
                var list = new List<HoldfastTradeInventorySlot>();
                foreach (var pair in _items)
                {
                    if (pair.Value <= 0) continue;
                    var def = _catalog?.GetItem(pair.Key) ?? new HoldfastItemDefinition(pair.Key, pair.Key, "", 1f, 1f);
                    list.Add(new HoldfastTradeInventorySlot(def, pair.Value));
                }
                return list;
            }
        }

        public float GetCurrentWeight()
        {
            float total = 0f;
            foreach (var pair in _items)
            {
                if (pair.Value <= 0) continue;
                var def = _catalog?.GetItem(pair.Key);
                float unitWeight = def != null ? def.Weight : 1f;
                total += unitWeight * pair.Value;
            }
            return total;
        }

        public void AddItem(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return;
            _items.TryGetValue(itemId, out int existing);
            _items[itemId] = existing + count;
        }

        public void RemoveItem(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return;
            if (_items.TryGetValue(itemId, out int existing))
            {
                if (existing <= count) _items.Remove(itemId);
                else _items[itemId] = existing - count;
            }
        }

        public void Clear() => _items.Clear();
    }

    /// <summary>Holdfast trade-state snapshot carried by the terminal's Buy/Sell surface.</summary>
    public sealed class HoldfastTradeResult
    {
        public bool Success { get; set; }
        public string ItemId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string FactionId { get; set; } = string.Empty;
        public int TotalValue { get; set; }
        public string Message { get; set; } = string.Empty;
        public HoldfastTradeFailure Failure { get; set; } = HoldfastTradeFailure.None;

        public static HoldfastTradeResult Ok(string itemId, int quantity, string factionId, int totalValue)
            => new HoldfastTradeResult { Success = true, ItemId = itemId, Quantity = quantity, FactionId = factionId, TotalValue = totalValue, Message = "Trade completed." };
        public static HoldfastTradeResult Fail(string message, HoldfastTradeFailure failure = HoldfastTradeFailure.None)
            => new HoldfastTradeResult { Success = false, Message = message, Failure = failure };
    }

    /// <summary>Engine-agnostic mutable trade state (inventory depth / faction stock).
    /// Terminal-facing; deterministic; host calls Buy/Sell.</summary>
    public sealed class HoldfastTradeSession
    {
        public const int DefaultInventoryCapacity = 20;
        private readonly HoldfastCatalog _catalog;
        private readonly Dictionary<string, int> _held = new Dictionary<string, int>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _stock = new Dictionary<string, int>(StringComparer.Ordinal);
        private long _value;
        private readonly long _initialValue;

        public HoldfastTradeInventory Inventory { get; }
        public string SelectedFactionId { get; private set; } = string.Empty;

        public event Action StateChanged;

        public HoldfastTradeSession(HoldfastCatalog catalog, long startingValue = 100)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            Inventory = new HoldfastTradeInventory(_catalog);
            _value = startingValue;
            _initialValue = startingValue;
            InitializeDefaultStocks();
        }

        private void InitializeDefaultStocks()
        {
            if (_catalog?.Items != null)
            {
                foreach (var item in _catalog.Items.Items)
                {
                    _stock[item.Id] = 20;
                }
            }
        }

        public bool SelectFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId) || _catalog?.GetFaction(factionId) == null)
            {
                if (factionId == "faction_the_office" || factionId == "faction_the_tempest" || factionId == "faction_the_fleet")
                {
                    SelectedFactionId = factionId;
                    StateChanged?.Invoke();
                    return true;
                }
                return false;
            }
            SelectedFactionId = factionId;
            StateChanged?.Invoke();
            return true;
        }

        public void SeedInventory(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return;
            _held[itemId] = GetHeld(itemId) + count;
            Inventory.AddItem(itemId, count);
            StateChanged?.Invoke();
        }

        public void ResetToDefaults()
        {
            _held.Clear();
            _stock.Clear();
            Inventory.Clear();
            _value = _initialValue;
            InitializeDefaultStocks();
            StateChanged?.Invoke();
        }

        /// <summary>Player-held quantity of an item id (0 when never held).</summary>
        public int GetHeld(string itemId)
            => _held.TryGetValue(itemId ?? string.Empty, out int h) ? h : 0;

        /// <summary>Faction stock for an item id (0 = none, 1 = low marker).</summary>
        public int GetStock(string itemId)
            => _stock.TryGetValue(itemId ?? string.Empty, out int s) ? s : 0;

        public void SetStock(string itemId, int count)
        {
            if (!string.IsNullOrEmpty(itemId))
                _stock[itemId] = Math.Max(0, count);
        }

        public long Value => _value;
        public long PlayerValue => _value;
        public IReadOnlyDictionary<string, int> Held => _held;

        public bool TryGetUnitValue(string itemId, out long unitValue)
        {
            var def = _catalog?.GetItem(itemId);
            if (def != null)
            {
                unitValue = Math.Max(1, (long)def.TradeValue);
                return true;
            }
            unitValue = 0;
            return false;
        }

        public HoldfastTradeResult Buy(string itemId, int quantity, string factionId)
        {
            if (string.IsNullOrEmpty(itemId) || _catalog?.GetItem(itemId) == null)
                return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);

            if (!string.IsNullOrEmpty(factionId) && factionId != "none")
            {
                if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
                    return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);

                if (factionId == "faction_the_fleet")
                    return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
            }

            if (quantity <= 0)
                return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);

            var def = _catalog.GetItem(itemId);
            int currentStock = GetStock(itemId);
            if (currentStock < quantity)
                return HoldfastTradeResult.Fail("Insufficient merchant stock.", HoldfastTradeFailure.InsufficientStock);

            int cost = quantity * Math.Max(1, (int)def.TradeValue);
            if (cost > _value)
                return HoldfastTradeResult.Fail("Insufficient funds.", HoldfastTradeFailure.InsufficientFunds);

            if (GetHeld(itemId) == 0 && Inventory.OccupiedCount >= Inventory.Capacity)
                return HoldfastTradeResult.Fail("Inventory capacity reached.", HoldfastTradeFailure.InventoryCapacity);

            _value -= cost;
            _held[itemId] = GetHeld(itemId) + quantity;
            _stock[itemId] = currentStock - quantity;
            Inventory.AddItem(itemId, quantity);
            StateChanged?.Invoke();
            return HoldfastTradeResult.Ok(itemId, quantity, factionId, cost);
        }

        public HoldfastTradeResult Sell(string itemId, int quantity, string factionId)
        {
            if (string.IsNullOrEmpty(itemId) || _catalog?.GetItem(itemId) == null)
                return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);

            if (!string.IsNullOrEmpty(factionId) && factionId != "none")
            {
                if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
                    return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);

                if (factionId == "faction_the_fleet")
                    return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
            }

            if (quantity <= 0)
                return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);

            int currentlyHeld = GetHeld(itemId);
            if (currentlyHeld < quantity)
                return HoldfastTradeResult.Fail("Insufficient player inventory.", HoldfastTradeFailure.InsufficientInventory);

            var def = _catalog.GetItem(itemId);
            int gain = quantity * Math.Max(1, (int)def.TradeValue);
            _value += gain;
            _held[itemId] = currentlyHeld - quantity;
            _stock[itemId] = GetStock(itemId) + quantity;
            Inventory.RemoveItem(itemId, quantity);
            StateChanged?.Invoke();
            return HoldfastTradeResult.Ok(itemId, quantity, factionId, gain);
        }

        public bool TryRestoreState(HoldfastTradeSaveState state, out string error)
        {
            error = string.Empty;
            if (state == null) { error = "null state"; return false; }
            _held.Clear();
            _stock.Clear();
            Inventory.Clear();
            if (state.held != null)
                foreach (var kv in state.held)
                {
                    _held[kv.Key] = kv.Value;
                    Inventory.AddItem(kv.Key, kv.Value);
                }
            if (state.stock != null)
                foreach (var kv in state.stock) _stock[kv.Key] = kv.Value;
            _value = state.value >= 0 ? state.value : _value;
            StateChanged?.Invoke();
            return true;
        }

        public HoldfastTradeSaveState CaptureState()
            => new HoldfastTradeSaveState { value = _value, held = new Dictionary<string, int>(_held), stock = new Dictionary<string, int>(_stock) };
    }

    /// <summary>Serializable trade save envelope (value + held + stock).</summary>
    [Serializable]
    public class HoldfastTradeSaveState
    {
        public long value;
        public Dictionary<string, int> held = new Dictionary<string, int>();
        public Dictionary<string, int> stock = new Dictionary<string, int>();
    }
}
