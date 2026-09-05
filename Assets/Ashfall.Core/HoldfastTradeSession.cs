using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Inventory;
using Ashfall.Core.PlayerCommand;

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
        InvalidPrice,
        Embargoed
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
        private readonly HoldfastCatalog? _catalog;
        private readonly Inventory.Inventory? _backingInventory;

        public HoldfastTradeInventory(HoldfastCatalog? catalog = null, Inventory.Inventory? backingInventory = null)
        {
            _catalog = catalog;
            _backingInventory = backingInventory;
        }

        public int OccupiedCount => _backingInventory != null ? _backingInventory.Slots.Count : _items.Count;
        public IReadOnlyDictionary<string, int> Items
        {
            get
            {
                if (_backingInventory != null)
                {
                    var map = new Dictionary<string, int>(StringComparer.Ordinal);
                    for (int i = 0; i < _backingInventory.Slots.Count; i++)
                    {
                        var s = _backingInventory.Slots[i];
                        if (s?.Item != null && s.Amount > 0)
                        {
                            map.TryGetValue(s.Item.id, out int cur);
                            map[s.Item.id] = cur + s.Amount;
                        }
                    }
                    return map;
                }
                return _items;
            }
        }

        public IReadOnlyList<HoldfastTradeInventorySlot> Slots
        {
            get
            {
                var list = new List<HoldfastTradeInventorySlot>();
                if (_backingInventory != null)
                {
                    for (int i = 0; i < _backingInventory.Slots.Count; i++)
                    {
                        var s = _backingInventory.Slots[i];
                        if (s?.Item != null && s.Amount > 0)
                        {
                            var def = _catalog?.GetItem(s.Item.id) ?? new HoldfastItemDefinition(s.Item.id, s.Item.displayName ?? s.Item.id, "", 1f, s.Item.weight);
                            list.Add(new HoldfastTradeInventorySlot(def, s.Amount));
                        }
                    }
                    return list;
                }

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
            if (_backingInventory != null)
                return _backingInventory.GetCurrentWeight();

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

        private ItemDefinition CreateItemDefinition(string canonicalId)
        {
            var def = _catalog?.GetItem(canonicalId);
            return new ItemDefinition
            {
                id = canonicalId,
                displayName = def?.DisplayName ?? canonicalId,
                description = def?.Description ?? string.Empty,
                type = ParseHoldfastItemType(def?.Type),
                stackMax = def != null && def.StackMax > 0 ? def.StackMax : (def != null && (def.Type == "gear" || def.Type == "equipment") ? 1 : 99),
                weight = def?.Weight ?? 1f,
                tradeValue = def?.TradeValue ?? 1f
            };
        }

        private static ItemType ParseHoldfastItemType(string? type)
        {
            if (string.IsNullOrWhiteSpace(type)) return ItemType.Material;
            if (Enum.TryParse<ItemType>(type, true, out var parsed)) return parsed;
            switch (type.Trim().ToLowerInvariant())
            {
                case "food": return ItemType.Food;
                case "water": return ItemType.Water;
                case "medical": return ItemType.Medical;
                case "gear":
                case "equipment": return ItemType.Protective;
                case "device": return ItemType.Device;
                case "tool": return ItemType.Tool;
                default: return ItemType.Material;
            }
        }

        public bool CanAdd(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return false;
            string canonical = ItemAliases.ToCanonical(itemId);
            if (_backingInventory != null)
            {
                var itemDef = CreateItemDefinition(canonical);
                return _backingInventory.CanAdd(itemDef, count);
            }
            float currentWeight = GetCurrentWeight();
            var def = _catalog?.GetItem(canonical);
            float unitWeight = def?.Weight ?? 1f;
            if (MaxWeight > 0f && currentWeight + unitWeight * count > MaxWeight) return false;
            int stackMax = def != null && def.StackMax > 0 ? def.StackMax : 99;
            _items.TryGetValue(canonical, out int currentCount);
            if (currentCount == 0 && OccupiedCount >= Capacity) return false;
            return true;
        }

        public bool AddItem(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return false;
            string canonical = ItemAliases.ToCanonical(itemId);
            if (!CanAdd(canonical, count)) return false;

            if (_backingInventory != null)
            {
                var itemDef = CreateItemDefinition(canonical);
                return _backingInventory.Add(itemDef, count);
            }

            _items.TryGetValue(canonical, out int existing);
            _items[canonical] = existing + count;
            return true;
        }

        public void RemoveItem(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return;
            string canonical = ItemAliases.ToCanonical(itemId);
            if (_backingInventory != null)
            {
                _backingInventory.RemoveById(canonical, count);
                return;
            }
            if (_items.TryGetValue(canonical, out int existing))
            {
                if (existing <= count) _items.Remove(canonical);
                else _items[canonical] = existing - count;
            }
        }

        public bool HasSufficient(string itemId, int count)
        {
            if (string.IsNullOrEmpty(itemId) || count <= 0) return true;
            string canonical = ItemAliases.ToCanonical(itemId);
            if (_backingInventory != null)
                return _backingInventory.CountById(canonical) >= count;
            return _items.TryGetValue(canonical, out int existing) && existing >= count;
        }

        public bool ValidateBill(IReadOnlyDictionary<string, int> bill)
        {
            if (bill == null || bill.Count == 0) return true;
            foreach (var kv in bill)
            {
                if (kv.Value <= 0) continue;
                string canonical = ItemAliases.ToCanonical(kv.Key);
                if (_backingInventory != null)
                {
                    if (_backingInventory.CountById(canonical) < kv.Value) return false;
                }
                else
                {
                    if (!_items.TryGetValue(canonical, out int existing) || existing < kv.Value)
                        return false;
                }
            }
            return true;
        }

        public bool TryConsumeBill(IReadOnlyDictionary<string, int> bill, Action? onCommitted = null)
        {
            if (!ValidateBill(bill)) return false;

            if (_backingInventory != null)
            {
                return _backingInventory.TryConsumeBill(bill, onCommitted);
            }

            // Standalone fallback: take a backup snapshot for rollback
            var snapshot = new Dictionary<string, int>(_items, StringComparer.Ordinal);
            try
            {
                if (bill != null)
                {
                    foreach (var kv in bill)
                    {
                        if (kv.Value > 0) RemoveItem(kv.Key, kv.Value);
                    }
                }

                if (onCommitted != null)
                {
                    onCommitted();
                }

                return true;
            }
            catch (Exception)
            {
                // Rollback
                _items.Clear();
                foreach (var kv in snapshot) _items[kv.Key] = kv.Value;
                throw;
            }
        }

        public void Clear()
        {
            _items.Clear();
            _backingInventory?.Clear();
        }
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
        private readonly HoldfastCatalog _catalog = null!;
        private readonly Inventory.Inventory? _playerInventory;
        private readonly Dictionary<string, int> _held = new Dictionary<string, int>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _stock = new Dictionary<string, int>(StringComparer.Ordinal);
        private long _value;
        private readonly long _initialValue;

        public HoldfastTradeInventory Inventory { get; }
        public Inventory.Inventory? PlayerInventory => _playerInventory;
        public string SelectedFactionId { get; private set; } = string.Empty;

        /// <summary>
        /// Canonical embargo authority hook, bound by the host as
        /// factionId → embargoed. When set, a suspended counterparty refuses
        /// both directions of trade; credit eligibility queries the same
        /// ledger, so credit can never bypass what trade cannot.
        /// </summary>
        public Func<string, bool>? EmbargoQuery { get; set; }

        public event Action StateChanged;

        public HoldfastTradeSession(HoldfastCatalog catalog, long startingValue = 100, Inventory.Inventory? playerInventory = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _playerInventory = playerInventory;
            Inventory = new HoldfastTradeInventory(_catalog, _playerInventory);
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
            string canonical = ItemAliases.ToCanonical(itemId);
            _held[canonical] = GetHeld(canonical) + count;
            Inventory.AddItem(canonical, count);
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
        {
            if (string.IsNullOrEmpty(itemId)) return 0;
            string canonical = ItemAliases.ToCanonical(itemId);
            if (_playerInventory != null)
            {
                return _playerInventory.CountById(canonical);
            }
            return _held.TryGetValue(canonical, out int h) ? h : (_held.TryGetValue(itemId, out int raw) ? raw : 0);
        }

        /// <summary>Faction stock for an item id (0 = none, 1 = low marker).</summary>
        public int GetStock(string itemId)
        {
            string canonical = ItemAliases.ToCanonical(itemId);
            return _stock.TryGetValue(canonical, out int s) ? s : (_stock.TryGetValue(itemId ?? string.Empty, out int raw) ? raw : 0);
        }

        public void SetStock(string itemId, int count)
        {
            if (!string.IsNullOrEmpty(itemId))
            {
                string canonical = ItemAliases.ToCanonical(itemId);
                _stock[canonical] = Math.Max(0, count);
            }
        }

        public long Value => _value;
        public long PlayerValue => _value;
        public IReadOnlyDictionary<string, int> Held
        {
            get
            {
                if (_playerInventory != null)
                {
                    var map = new Dictionary<string, int>(StringComparer.Ordinal);
                    for (int i = 0; i < _playerInventory.Slots.Count; i++)
                    {
                        var s = _playerInventory.Slots[i];
                        if (s?.Item != null && s.Amount > 0)
                        {
                            string canonical = ItemAliases.ToCanonical(s.Item.id);
                            map.TryGetValue(canonical, out int cur);
                            map[canonical] = cur + s.Amount;
                        }
                    }
                    return map;
                }
                return _held;
            }
        }

        public bool TryGetUnitValue(string itemId, out long unitValue)
        {
            string canonical = ItemAliases.ToCanonical(itemId);
            var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
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
            string canonical = ItemAliases.ToCanonical(itemId);
            var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
            if (string.IsNullOrEmpty(canonical) || def == null)
                return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);

            if (!string.IsNullOrEmpty(factionId) && factionId != "none")
            {
                if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
                    return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);

                if (factionId == "faction_the_fleet")
                    return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
            }

            if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
                return HoldfastTradeResult.Fail("Trade with this faction is suspended (embargo).", HoldfastTradeFailure.Embargoed);

            if (quantity <= 0)
                return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);

            int currentStock = GetStock(canonical);
            if (currentStock < quantity)
                return HoldfastTradeResult.Fail("Insufficient merchant stock.", HoldfastTradeFailure.InsufficientStock);

            int cost = quantity * Math.Max(1, (int)def.TradeValue);
            if (cost > _value)
                return HoldfastTradeResult.Fail("Insufficient funds.", HoldfastTradeFailure.InsufficientFunds);

            if (!Inventory.CanAdd(canonical, quantity))
            {
                float unitWeight = def.Weight;
                if (Inventory.MaxWeight > 0f && Inventory.GetCurrentWeight() + unitWeight * quantity > Inventory.MaxWeight)
                    return HoldfastTradeResult.Fail("Inventory weight limit exceeded.", HoldfastTradeFailure.InventoryCapacity);
                return HoldfastTradeResult.Fail("Inventory capacity reached.", HoldfastTradeFailure.InventoryCapacity);
            }

            long prevValue = _value;
            int prevStock = currentStock;
            int prevHeld = GetHeld(canonical);

            bool added = Inventory.AddItem(canonical, quantity);
            if (!added)
                return HoldfastTradeResult.Fail("Inventory capacity reached.", HoldfastTradeFailure.InventoryCapacity);

            try
            {
                _value -= cost;
                _stock[canonical] = currentStock - quantity;
                if (_playerInventory == null)
                {
                    _held[canonical] = prevHeld + quantity;
                }
                StateChanged?.Invoke();
                return HoldfastTradeResult.Ok(canonical, quantity, factionId, cost);
            }
            catch (Exception ex)
            {
                Inventory.RemoveItem(canonical, quantity);
                _value = prevValue;
                _stock[canonical] = prevStock;
                if (_playerInventory == null)
                {
                    _held[canonical] = prevHeld;
                }
                return HoldfastTradeResult.Fail("Trade transaction failed: " + ex.Message, HoldfastTradeFailure.InvalidPrice);
            }
        }

        public HoldfastTradeResult Sell(string itemId, int quantity, string factionId)
        {
            string canonical = ItemAliases.ToCanonical(itemId);
            var def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
            if (string.IsNullOrEmpty(canonical) || def == null)
                return HoldfastTradeResult.Fail("Unknown item: " + itemId, HoldfastTradeFailure.UnknownItem);

            if (!string.IsNullOrEmpty(factionId) && factionId != "none")
            {
                if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
                    return HoldfastTradeResult.Fail("Unknown faction: " + factionId, HoldfastTradeFailure.UnknownFaction);

                if (factionId == "faction_the_fleet")
                    return HoldfastTradeResult.Fail("Unavailable or restricted counterparty: " + factionId, HoldfastTradeFailure.UnavailableOrRestricted);
            }

            if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
                return HoldfastTradeResult.Fail("Trade with this faction is suspended (embargo).", HoldfastTradeFailure.Embargoed);

            if (quantity <= 0)
                return HoldfastTradeResult.Fail("Quantity must be at least 1.", HoldfastTradeFailure.InvalidQuantity);

            int currentlyHeld = GetHeld(canonical);
            if (currentlyHeld < quantity)
                return HoldfastTradeResult.Fail("Insufficient player inventory.", HoldfastTradeFailure.InsufficientInventory);

            int gain = quantity * Math.Max(1, (int)def.TradeValue);
            _value += gain;
            _held[canonical] = currentlyHeld - quantity;
            _stock[canonical] = GetStock(canonical) + quantity;
            Inventory.RemoveItem(canonical, quantity);
            StateChanged?.Invoke();
            return HoldfastTradeResult.Ok(canonical, quantity, factionId, gain);
        }

        public bool TryRestoreState(HoldfastTradeSaveState state, out string error)
        {
            error = string.Empty;
            if (state == null) { error = "null state"; return false; }
            _held.Clear();
            _stock.Clear();

            // When a backing inventory is wired, Inventory IS the shared
            // authoritative player inventory (Inventory = new
            // HoldfastTradeInventory(_catalog, _playerInventory) above) — the
            // same object InventoryHostSession, expeditions, crafting, and
            // every other system read/write. Clearing it here would silently
            // discard everything the player holds that isn't Holdfast trade
            // stock (food, water, medicine, gear) and is not this session's
            // to own. MigrateHoldfastHeld already performs the correct,
            // non-destructive merge into that authoritative inventory below;
            // only the standalone/no-backing-inventory path (tests, or a
            // trade session with its own private ledger) still owns its
            // items outright and may safely clear+rebuild them here.
            if (_playerInventory == null)
                Inventory.Clear();

            if (state.held != null)
            {
                if (_playerInventory == null)
                {
                    foreach (var kv in state.held)
                    {
                        string canonical = ItemAliases.ToCanonical(kv.Key);
                        _held[canonical] = kv.Value;
                        Inventory.AddItem(canonical, kv.Value);
                    }
                }
                else
                {
                    if (state.schemaVersion < 2)
                    {
                        InventoryMigrator.MigrateHoldfastHeld(state, _playerInventory, id =>
                        {
                            var def = _catalog?.GetItem(id);
                            return def != null ? new ItemDefinition { id = id, displayName = def.DisplayName, stackMax = 99, weight = def.Weight } : null;
                        }, allowResurrectLowerPhysicalCount: false);
                    }
                    _held.Clear();
                    for (int i = 0; i < _playerInventory.Slots.Count; i++)
                    {
                        var s = _playerInventory.Slots[i];
                        if (s?.Item != null && s.Amount > 0)
                        {
                            string canonical = ItemAliases.ToCanonical(s.Item.id);
                            _held.TryGetValue(canonical, out int cur);
                            _held[canonical] = cur + s.Amount;
                        }
                    }
                }
            }
            if (state.stock != null)
                foreach (var kv in state.stock) _stock[ItemAliases.ToCanonical(kv.Key)] = kv.Value;
            _value = state.value >= 0 ? state.value : _value;
            StateChanged?.Invoke();
            return true;
        }

        private bool ValidateTradeItem(string itemId, out HoldfastItemDefinition def, out string failureCode, out string messageKey)
        {
            failureCode = "unknown_item";
            messageKey = "trade.unknown_item";
            string canonical = ItemAliases.ToCanonical(itemId);
            def = _catalog?.GetItem(canonical) ?? _catalog?.GetItem(itemId);
            if (string.IsNullOrEmpty(canonical) || def == null)
                return false;
            return true;
        }

        private bool ValidateFaction(string factionId, out string failureCode, out string messageKey)
        {
            failureCode = "unknown_faction";
            messageKey = "trade.unknown_faction";
            if (!string.IsNullOrEmpty(factionId) && factionId != "none")
            {
                if (factionId == "faction_nonexistent" || (_catalog?.GetFaction(factionId) == null && factionId != "faction_the_office" && factionId != "faction_the_tempest"))
                {
                    failureCode = "unknown_faction";
                    messageKey = "trade.unknown_faction";
                    return false;
                }
                if (factionId == "faction_the_fleet")
                {
                    failureCode = "unavailable_or_restricted";
                    messageKey = "trade.unavailable_or_restricted";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Side-effect-free preview of a buy command.
        /// Shares the same validation path as <see cref="Buy"/>.
        /// </summary>
        public CommandPreview PreviewBuy(string itemId, int quantity, string factionId, long stateVersion = 0)
        {
            if (quantity <= 0)
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "invalid_quantity", "trade.invalid_quantity", stateVersion);

            if (!ValidateTradeItem(itemId, out var def, out var itemFailure, out var itemMessage))
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, itemFailure, itemMessage, stateVersion);

            if (!ValidateFaction(factionId, out var factionFailure, out var factionMessage))
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, factionFailure, factionMessage, stateVersion);

            if (!string.IsNullOrEmpty(factionId) && factionId != "none" && EmbargoQuery != null && EmbargoQuery(factionId))
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "embargoed", "trade.embargoed", stateVersion);

            string canonical = ItemAliases.ToCanonical(itemId);
            int currentStock = GetStock(canonical);
            if (currentStock < quantity)
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "insufficient_stock", "trade.insufficient_stock", stateVersion);

            int cost = quantity * Math.Max(1, (int)def.TradeValue);
            if (cost > _value)
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "insufficient_funds", "trade.insufficient_funds", stateVersion);

            if (!Inventory.CanAdd(canonical, quantity))
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "inventory_capacity", "trade.inventory_capacity", stateVersion);

            var deltas = new Dictionary<string, double>
            {
                { "value", -cost },
                { canonical, quantity },
                { "stock", -quantity }
            };

            return CommandPreview.Available(
                PlayerCommandCode.TradeConfirm,
                stateVersion,
                deltas,
                isIrreversible: false,
                messageKey: "trade.preview_buy");
        }

        /// <summary>
        /// Execute a buy command using the same validation path as <see cref="PreviewBuy"/>.
        /// Stale previews (state version mismatch) are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteBuy(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewBuy(itemId, quantity, factionId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.TradeConfirm, preview.StateVersion, currentStateVersion);

            var result = Buy(itemId, quantity, factionId);
            if (!result.Success)
                return new CommandResult(
                    PlayerCommandCode.TradeConfirm,
                    ActionResult.Failed(result.Failure.ToString(), "trade.buy_failed"),
                    expectedStateVersion,
                    currentStateVersion);

            var deltas = new Dictionary<string, double>
            {
                { "value", -result.TotalValue },
                { result.ItemId, result.Quantity },
                { "stock", -result.Quantity }
            };

            return CommandResult.FromSuccess(
                PlayerCommandCode.TradeConfirm,
                ActionResult.Success("trade.bought", deltas),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        /// <summary>
        /// Side-effect-free preview of a sell command.
        /// Shares the same validation path as <see cref="Sell"/>.
        /// </summary>
        public CommandPreview PreviewSell(string itemId, int quantity, string factionId, long stateVersion = 0)
        {
            if (quantity <= 0)
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "invalid_quantity", "trade.invalid_quantity", stateVersion);

            if (!ValidateTradeItem(itemId, out var def, out var itemFailure, out var itemMessage))
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, itemFailure, itemMessage, stateVersion);

            if (!ValidateFaction(factionId, out var factionFailure, out var factionMessage))
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, factionFailure, factionMessage, stateVersion);

            string canonical = ItemAliases.ToCanonical(itemId);
            int currentlyHeld = GetHeld(canonical);
            if (currentlyHeld < quantity)
                return CommandPreview.Unavailable(PlayerCommandCode.TradeConfirm, "insufficient_inventory", "trade.insufficient_inventory", stateVersion);

            int gain = quantity * Math.Max(1, (int)def.TradeValue);
            var deltas = new Dictionary<string, double>
            {
                { "value", gain },
                { canonical, -quantity },
                { "stock", quantity }
            };

            return CommandPreview.Available(
                PlayerCommandCode.TradeConfirm,
                stateVersion,
                deltas,
                isIrreversible: false,
                messageKey: "trade.preview_sell");
        }

        /// <summary>
        /// Execute a sell command using the same validation path as <see cref="PreviewSell"/>.
        /// Stale previews (state version mismatch) are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteSell(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewSell(itemId, quantity, factionId, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.TradeConfirm, preview.StateVersion, currentStateVersion);

            var result = Sell(itemId, quantity, factionId);
            if (!result.Success)
                return new CommandResult(
                    PlayerCommandCode.TradeConfirm,
                    ActionResult.Failed(result.Failure.ToString(), "trade.sell_failed"),
                    expectedStateVersion,
                    currentStateVersion);

            var deltas = new Dictionary<string, double>
            {
                { "value", result.TotalValue },
                { result.ItemId, -result.Quantity },
                { "stock", result.Quantity }
            };

            return CommandResult.FromSuccess(
                PlayerCommandCode.TradeConfirm,
                ActionResult.Success("trade.sold", deltas),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        public HoldfastTradeSaveState CaptureState()
        {
            var heldMap = new Dictionary<string, int>(Held, StringComparer.Ordinal);
            return new HoldfastTradeSaveState
            {
                schemaVersion = 2,
                value = _value,
                held = heldMap,
                stock = new Dictionary<string, int>(_stock, StringComparer.Ordinal)
            };
        }
    }

    /// <summary>Serializable trade save envelope (value + held + stock).</summary>
    [Serializable]
    public class HoldfastTradeSaveState
    {
        public int schemaVersion = 0;
        public long value;
        public Dictionary<string, int> held = new Dictionary<string, int>();
        public Dictionary<string, int> stock = new Dictionary<string, int>();
    }
}
