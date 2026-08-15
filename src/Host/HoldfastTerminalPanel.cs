using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Playable Godot Holdfast terminal. It only queries the Core catalog/session;
    /// all inventory, value, stock, and transaction rules stay in Core.
    /// </summary>
    public partial class HoldfastTerminalPanel : PanelContainer
    {
        private HoldfastRuntimeSession _session;
        private HoldfastDispatchLog _dispatch;
        private TabContainer _tabs;
        private ItemList _factionList;
        private ItemList _supplyList;
        private ItemList _inventoryList;
        private OptionButton _tradeItemSelector;
        private SpinBox _tradeQuantity;
        private Label _statusText;
        private Label _factionDetails;
        private Label _supplyDetails;
        private Label _inventorySummary;
        private Label _tradeDetails;
        private Label _feedback;
        private Label _dispatchLog;
        private Button _buyButton;
        private Button _sellButton;
        private Button _newLedgerButton;
        private readonly List<string> _factionIds = new List<string>();
        private readonly List<string> _itemIds = new List<string>();
        private string _selectedFactionId = string.Empty;
        private string _selectedItemId = string.Empty;
        private bool _refreshing;
        private bool _newLedgerArmed;
        private float _newLedgerTimer;

        public event Action Closed;

        public bool IsBound => _session != null;
        public string SelectedFactionId => _selectedFactionId;
        public string SelectedItemId => _selectedItemId;
        public string FeedbackText => _feedback?.Text ?? string.Empty;
        public string FactionDetailsText => _factionDetails?.Text ?? string.Empty;
        public string SupplyDetailsText => _supplyDetails?.Text ?? string.Empty;
        public string InventorySummaryText => _inventorySummary?.Text ?? string.Empty;
        public string TradeDetailsText => _tradeDetails?.Text ?? string.Empty;
        public int PresentedItemCount => _supplyList?.ItemCount ?? 0;
        public int PresentedFactionCount => _factionList?.ItemCount ?? 0;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
        }

        public void BindSession(HoldfastRuntimeSession session)
        {
            if (_session != null)
                _session.StateChanged -= RefreshView;
            _session = session;
            if (_session != null)
            {
                _session.StateChanged += RefreshView;
                _selectedFactionId = _session.Trade.SelectedFactionId;
                _dispatch = new HoldfastDispatchLog(HoldfastFlavorCatalog.Load(session.World.Catalog.Items.Items.Count > 0 ? "Assets/StreamingAssets/Data" : null));
                _dispatch.OnSessionOpened("holdfast");
            }
            RefreshView();
        }

        public void OpenTerminal()
        {
            Visible = true;
            RefreshView();
            _tabs.CurrentTab = 0;
            _newLedgerArmed = false;
            _newLedgerButton.Text = "NEW LEDGER";
            _newLedgerButton.Disabled = false;
        }

        public void CloseTerminal()
        {
            Visible = false;
            Closed?.Invoke();
        }

        public void SelectFaction(string factionId)
        {
            if (_session == null || !_session.Trade.SelectFaction(factionId)) return;
            _selectedFactionId = factionId;
            RefreshView();
        }

        public void SelectItem(string itemId)
        {
            if (_session?.Catalog.GetItem(itemId) == null) return;
            _selectedItemId = itemId;
            SelectItemInControls(itemId);
            RefreshView();
        }

        /// <summary>Test-only: select an item without catalog validation.</summary>
        public void SelectItemRaw(string itemId)
        {
            _selectedItemId = itemId;
        }

        /// <summary>Test-only: select a faction without catalog validation.</summary>
        public void SelectFactionRaw(string factionId)
        {
            _selectedFactionId = factionId;
        }

        public void SetTradeQuantity(int quantity)
        {
            if (_tradeQuantity != null)
                _tradeQuantity.Value = quantity;
            RefreshTradeDetails();
        }

        public HoldfastTradeResult PressBuy()
        {
            if (_session == null)
                return null;
            var result = _session.Trade.Buy(_selectedItemId, (int)_tradeQuantity.Value, _selectedFactionId);
            if (result.Success)
            {
                _session.HasPurchasedThisSession = true;
                _dispatch?.OnPurchase(result.ItemId, result.Quantity, result.TotalValue, _selectedFactionId);
                if (_session.HasPurchasedThisSession && _session.Trade.GetHeld(_selectedItemId) == result.Quantity)
                    _dispatch?.OnFirstPurchase(result.ItemId, result.Quantity, result.TotalValue, _selectedFactionId);
                if (_session.Trade.GetStock(result.ItemId) == 1)
                    _dispatch?.OnStockLow(result.ItemId, 1, _selectedFactionId);
                else if (_session.Trade.GetStock(result.ItemId) == 0)
                    _dispatch?.OnStockEmpty(result.ItemId, _selectedFactionId);
            }
            else
            {
                _dispatch?.OnRejected(result, _selectedFactionId);
            }
            ShowTradeResult(result);
            return result;
        }

        public HoldfastTradeResult PressSell()
        {
            if (_session == null)
                return null;
            var result = _session.Trade.Sell(_selectedItemId, (int)_tradeQuantity.Value, _selectedFactionId);
            if (result.Success)
            {
                _dispatch?.OnSale(result.ItemId, result.Quantity, result.TotalValue, _selectedFactionId);
                if (_session.Trade.GetHeld(result.ItemId) == 0)
                    _dispatch?.OnHoldingEmptied(result.ItemId, _selectedFactionId);
            }
            else
            {
                _dispatch?.OnRejected(result, _selectedFactionId);
            }
            ShowTradeResult(result);
            return result;
        }

        public bool PressSave(string basePathOverride = null, string tradePathOverride = null)
        {
            bool saved = _session != null && _session.TrySave(basePathOverride, tradePathOverride);
            _feedback.Text = _session == null
                ? "Holdfast terminal is not connected."
                : _session.LastPersistenceMessage;
            if (saved)
            {
                string path = basePathOverride ?? HoldfastSaveStore.SavePath;
                _dispatch?.OnSaveCommitted(path);
            }
            RefreshDispatchLog();
            return saved;
        }

        public bool PressReload(string basePathOverride = null, string tradePathOverride = null)
        {
            bool loaded = _session != null && _session.TryReload(basePathOverride, tradePathOverride);
            _feedback.Text = _session == null
                ? "Holdfast terminal is not connected."
                : _session.LastPersistenceMessage;
            if (loaded)
            {
                string path = basePathOverride ?? HoldfastSaveStore.SavePath;
                _dispatch?.OnReloaded(path);
            }
            RefreshView();
            RefreshDispatchLog();
            return loaded;
        }

        public bool PressNewLedger()
        {
            if (_session == null) return false;
            if (!_newLedgerArmed)
            {
                _newLedgerArmed = true;
                _newLedgerButton.Text = "CONFIRM NEW LEDGER?";
                _newLedgerTimer = 3.0f;
                _feedback.Text = "Press again within 3 seconds to archive current ledger and start fresh.";
                return false;
            }

            bool ok = _session.ArchiveAndFreshStart();
            _newLedgerArmed = false;
            _newLedgerButton.Text = "NEW LEDGER";
            _feedback.Text = _session.LastPersistenceMessage;
            if (ok)
            {
                _dispatch?.OnNewLedger();
                RefreshDispatchLog();
            }
            RefreshView();
            return ok;
        }

        public override void _Process(double delta)
        {
            if (_newLedgerArmed)
            {
                _newLedgerTimer -= (float)delta;
                if (_newLedgerTimer <= 0f)
                {
                    _newLedgerArmed = false;
                    _newLedgerButton.Text = "NEW LEDGER";
                }
            }
        }

        public override void _UnhandledKeyInput(InputEvent @event)
        {
            var key = @event as InputEventKey;
            if (key == null || !key.Pressed || key.Echo) return;

            if (key.Keycode == Key.Escape)
            {
                CloseTerminal();
                GetViewport().SetInputAsHandled();
                return;
            }

            if (!Visible || _session == null) return;

            // Keyboard shortcuts when trade tab is active.
            if (_tabs.CurrentTab == 4)
            {
                if (key.Keycode == Key.B)
                {
                    PressBuy();
                    GetViewport().SetInputAsHandled();
                }
                else if (key.Keycode == Key.S && !key.CtrlPressed)
                {
                    PressSell();
                    GetViewport().SetInputAsHandled();
                }
            }
        }

        public void RefreshView()
        {
            if (_session == null || _tabs == null) return;
            _refreshing = true;
            EnsureSelections();
            RefreshFactions();
            RefreshSupplies();
            RefreshInventory();
            RefreshStatus();
            RefreshTradeSelector();
            _refreshing = false;
            RefreshFactionDetails();
            RefreshSupplyDetails();
            RefreshTradeDetails();
            RefreshDispatchLog();
        }

        private void BuildLayout()
        {
            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", 32);
            margin.AddThemeConstantOverride("margin_top", 24);
            margin.AddThemeConstantOverride("margin_right", 32);
            margin.AddThemeConstantOverride("margin_bottom", 24);
            AddChild(margin);

            var root = new VBoxContainer();
            root.AddThemeConstantOverride("separation", 8);
            margin.AddChild(root);

            var header = new HBoxContainer();
            var title = new Label
            {
                Text = "THE HOLDFAST · QUARTERMASTER TERMINAL",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            title.AddThemeFontSizeOverride("font_size", 22);
            title.AddThemeColorOverride("font_color", new Color(0.95f, 0.65f, 0.25f));
            header.AddChild(title);

            _newLedgerButton = new Button { Text = "NEW LEDGER" };
            _newLedgerButton.Pressed += () => PressNewLedger();
            header.AddChild(_newLedgerButton);

            var save = new Button { Text = "SAVE" };
            save.Pressed += () => PressSave();
            header.AddChild(save);
            var reload = new Button { Text = "RELOAD" };
            reload.Pressed += () => PressReload();
            header.AddChild(reload);
            var close = new Button { Text = "CLOSE [Esc]" };
            close.Pressed += CloseTerminal;
            header.AddChild(close);
            root.AddChild(header);

            _feedback = new Label
            {
                Text = "Select a faction and a catalog item. The terminal reports the live store state.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _feedback.AddThemeColorOverride("font_color", new Color(0.95f, 0.95f, 0.85f));
            root.AddChild(_feedback);

            _tabs = new TabContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            root.AddChild(_tabs);
            _tabs.AddChild(BuildStatusPage());
            _tabs.AddChild(BuildFactionPage());
            _tabs.AddChild(BuildSupplyPage());
            _tabs.AddChild(BuildInventoryPage());
            _tabs.AddChild(BuildTradePage());

            var logLabel = new Label { Text = "DISPATCH LOG" };
            logLabel.AddThemeFontSizeOverride("font_size", 12);
            logLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.7f, 0.65f));
            root.AddChild(logLabel);

            _dispatchLog = new Label
            {
                Text = string.Empty,
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                CustomMinimumSize = new Vector2(0, 80)
            };
            _dispatchLog.AddThemeFontSizeOverride("font_size", 11);
            _dispatchLog.AddThemeColorOverride("font_color", new Color(0.75f, 0.85f, 0.78f));
            _dispatchLog.AddThemeColorOverride("font_color_shadow", new Color(0f, 0f, 0f, 0.6f));
            root.AddChild(_dispatchLog);
        }

        private Control BuildStatusPage()
        {
            var page = NewPage("Status");
            _statusText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _statusText.AddThemeFontSizeOverride("font_size", 13);
            _statusText.AddThemeColorOverride("font_color", new Color(0.85f, 0.9f, 0.85f));
            page.AddChild(_statusText);
            page.AddChild(new Label
            {
                Text = "The status page is read-only. Use Factions, Supplies, Inventory, and Trade to operate on live state.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            });
            return page;
        }

        private Control BuildFactionPage()
        {
            var page = NewPage("Factions");
            var split = new HSplitContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _factionList = new ItemList
            {
                CustomMinimumSize = new Vector2(260, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _factionList.ItemSelected += OnFactionSelected;
            split.AddChild(_factionList);
            _factionDetails = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _factionDetails.AddThemeFontSizeOverride("font_size", 13);
            _factionDetails.AddThemeColorOverride("font_color", new Color(0.85f, 0.9f, 0.85f));
            split.AddChild(_factionDetails);
            page.AddChild(split);
            return page;
        }

        private Control BuildSupplyPage()
        {
            var page = NewPage("Supplies");
            var split = new HSplitContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _supplyList = new ItemList
            {
                CustomMinimumSize = new Vector2(420, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _supplyList.ItemSelected += OnSupplySelected;
            split.AddChild(_supplyList);
            _supplyDetails = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _supplyDetails.AddThemeFontSizeOverride("font_size", 13);
            _supplyDetails.AddThemeColorOverride("font_color", new Color(0.85f, 0.9f, 0.85f));
            split.AddChild(_supplyDetails);
            page.AddChild(split);
            return page;
        }

        private Control BuildInventoryPage()
        {
            var page = NewPage("Inventory");
            _inventorySummary = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _inventorySummary.AddThemeFontSizeOverride("font_size", 13);
            _inventorySummary.AddThemeColorOverride("font_color", new Color(0.95f, 0.95f, 0.85f));
            page.AddChild(_inventorySummary);
            _inventoryList = new ItemList { SizeFlagsVertical = SizeFlags.ExpandFill };
            _inventoryList.AddThemeFontSizeOverride("font_size", 13);
            page.AddChild(_inventoryList);
            return page;
        }

        private Control BuildTradePage()
        {
            var page = NewPage("Trade");
            page.AddChild(new Label
            {
                Text = "TRADE · all prices use Holdfast item tradeValue; active factions are valid counterparties.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            });

            _tradeItemSelector = new OptionButton();
            _tradeItemSelector.ItemSelected += OnTradeItemSelected;
            page.AddChild(_tradeItemSelector);

            _tradeDetails = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _tradeDetails.AddThemeFontSizeOverride("font_size", 13);
            _tradeDetails.AddThemeColorOverride("font_color", new Color(0.85f, 0.9f, 0.85f));
            page.AddChild(_tradeDetails);

            var quantityRow = new HBoxContainer();
            quantityRow.AddChild(new Label { Text = "Quantity" });
            _tradeQuantity = new SpinBox
            {
                MinValue = 0,
                MaxValue = 1,
                Step = 1,
                Value = 1,
                AllowLesser = true,
                CustomMinimumSize = new Vector2(140, 0)
            };
            _tradeQuantity.ValueChanged += _ => RefreshTradeDetails();
            quantityRow.AddChild(_tradeQuantity);
            page.AddChild(quantityRow);

            var actions = new HBoxContainer();
            _buyButton = new Button { Text = "BUY SELECTED" };
            _buyButton.Pressed += () => PressBuy();
            actions.AddChild(_buyButton);
            _sellButton = new Button { Text = "SELL SELECTED" };
            _sellButton.Pressed += () => PressSell();
            actions.AddChild(_sellButton);
            page.AddChild(actions);
            return page;
        }

        private static VBoxContainer NewPage(string name)
        {
            return new VBoxContainer
            {
                Name = name,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
        }

        private void EnsureSelections()
        {
            if (_session == null) return;
            if (_session.Catalog.GetFaction(_selectedFactionId) == null)
                _selectedFactionId = _session.Trade.SelectedFactionId;
            if (_session.Catalog.GetItem(_selectedItemId) == null && _session.Catalog.Items.Count > 0)
                _selectedItemId = _session.Catalog.Items.Items[0].Id;
        }

        private void RefreshFactions()
        {
            _factionIds.Clear();
            _factionList.Clear();
            var factions = new List<HoldfastFactionEntry>(_session.Catalog.Factions);
            factions.Sort((left, right) => string.CompareOrdinal(left?.id, right?.id));
            foreach (var faction in factions)
            {
                if (faction == null) continue;
                _factionIds.Add(faction.id);
                _factionList.AddItem(faction.display_name + (faction.is_active ? " [ACTIVE]" : " [DORMANT]"));
            }
            int index = _factionIds.IndexOf(_selectedFactionId);
            if (index >= 0) _factionList.Select(index, true);
        }

        private void RefreshSupplies()
        {
            _itemIds.Clear();
            _supplyList.Clear();
            foreach (var definition in _session.Catalog.Items.Items)
            {
                _itemIds.Add(definition.Id);
                _supplyList.AddItem(
                    definition.DisplayName + " · " + definition.Type +
                    " · stock " + _session.Trade.GetStock(definition.Id));
            }
            int index = _itemIds.IndexOf(_selectedItemId);
            if (index >= 0) _supplyList.Select(index, true);
        }

        private void RefreshInventory()
        {
            var inventory = _session.Trade.Inventory;
            _inventorySummary.Text =
                "Available value: " + _session.Trade.PlayerValue +
                "\nStacks: " + inventory.Slots.Count +
                " · weight " + inventory.GetCurrentWeight().ToString("0.0") + "/" + inventory.MaxWeight.ToString("0") + " kg";
            _inventoryList.Clear();
            foreach (var slot in inventory.Slots)
            {
                if (slot?.Item == null) continue;
                _inventoryList.AddItem(slot.Item.displayName + " ×" + slot.Amount + " [" + slot.Item.id + "]");
            }
            if (_inventoryList.ItemCount == 0)
                _inventoryList.AddItem("Nothing stored. The shelves are bare.");
        }

        private void RefreshStatus()
        {
            _statusText.Text =
                _session.World.StatusLine() + "\n" +
                _session.World.BrineLine() + "\n" +
                _session.World.CensusLine() + "\n" +
                _session.World.CatalogLine() + "\n" +
                "Selected counterparty: " + (_selectedFactionId.Length == 0 ? "none" : _selectedFactionId) + "\n" +
                "Available value: " + _session.Trade.PlayerValue;
        }

        private void RefreshFactionDetails()
        {
            var faction = _session.Catalog.GetFaction(_selectedFactionId);
            if (faction == null)
            {
                _factionDetails.Text = "No faction selected.";
                return;
            }
            _factionDetails.Text =
                faction.display_name + " [" + (faction.is_active ? "ACTIVE" : "DORMANT") + "]\n" +
                "Id: " + faction.id + "\n" +
                "Alignment: " + faction.alignment + "\n" +
                "Region: " + faction.home_region + "\n" +
                "Trust: " + faction.trust.ToString("0.0") + "\n\n" +
                "Wants: " + Join(faction.wants) + "\n" +
                "Offers: " + Join(faction.offers) + "\n\n" +
                faction.signature_quote + "\n\n" +
                "Access: " + faction.access_rule;
        }

        private void RefreshSupplyDetails()
        {
            var definition = _session.Catalog.GetItem(_selectedItemId);
            if (definition == null)
            {
                _supplyDetails.Text = "No item selected.";
                return;
            }
            _supplyDetails.Text =
                definition.DisplayName + "\n" +
                "Id: " + definition.Id + "\n" +
                "Category: " + definition.Type + "\n" +
                "Description: " + definition.Description + "\n\n" +
                "Unit value: " + FormatUnitValue(definition.Id) +
                " · stock: " + _session.Trade.GetStock(definition.Id) +
                " · held: " + _session.Trade.GetHeld(definition.Id) + "\n" +
                "Stack max: " + definition.StackMax +
                " · weight: " + definition.Weight.ToString("0.0") + " kg\n\n" +
                (_dispatch != null ? _dispatch.GetType().Name : "Flavor") + ": " +
                HoldfastFlavorCatalog.NeutralItemMarginalia;
        }

        private void RefreshTradeSelector()
        {
            _tradeItemSelector.Clear();
            foreach (var definition in _session.Catalog.Items.Items)
                _tradeItemSelector.AddItem(definition.DisplayName + " [" + definition.Id + "]");
            int index = _session.Catalog.Items.Items.IndexOfId(_selectedItemId);
            if (index >= 0) _tradeItemSelector.Select(index);
        }

        private void RefreshTradeDetails()
        {
            if (_session == null || _tradeDetails == null) return;
            var definition = _session.Catalog.GetItem(_selectedItemId);
            if (definition == null)
            {
                _tradeDetails.Text = "No item selected.";
                UpdateTradeActions(null, 0, 0);
                return;
            }
            int stock = _session.Trade.GetStock(definition.Id);
            int held = _session.Trade.GetHeld(definition.Id);
            int qty = (int)_tradeQuantity.Value;
            _tradeQuantity.MaxValue = Math.Max(1, Math.Max(stock, held));
            _tradeDetails.Text =
                "Counterparty: " + (_selectedFactionId.Length == 0 ? "none" : _selectedFactionId) + "\n" +
                "Item: " + definition.DisplayName + "\n" +
                "Unit value: " + FormatUnitValue(definition.Id) + "\n" +
                "Merchant stock: " + stock + " · Player holdings: " + held + "\n" +
                "Available value: " + _session.Trade.PlayerValue + "\n" +
                "Quantity limits: buy up to stock; sell up to holdings.";

            UpdateTradeActions(definition, qty, stock);
        }

        private void UpdateTradeActions(HoldfastItemDefinition definition, int quantity, int stock)
        {
            if (_buyButton == null || _sellButton == null || _tradeQuantity == null) return;
            bool canBuy = definition != null
                && quantity > 0
                && stock > 0
                && _session.Trade.TryGetUnitValue(definition.Id, out long uv)
                && _session.Trade.PlayerValue >= uv * quantity;
            bool canSell = definition != null
                && quantity > 0
                && _session.Trade.GetHeld(definition.Id) >= quantity
                && _session.Trade.TryGetUnitValue(definition.Id, out uv)
                && _session.Trade.GetStock(definition.Id) >= 0;

            _buyButton.Disabled = !canBuy;
            _sellButton.Disabled = !canSell;
        }

        private void RefreshDispatchLog()
        {
            if (_dispatchLog == null || _dispatch == null) return;
            var recent = new List<string>(_dispatch.Entries);
            if (recent.Count > 0)
                _dispatchLog.Text = string.Join("\n", recent.GetRange(Math.Max(0, recent.Count - 6), Math.Min(6, recent.Count)));
        }

        private void OnFactionSelected(long index)
        {
            if (_refreshing || index < 0 || index >= _factionIds.Count) return;
            SelectFaction(_factionIds[(int)index]);
        }

        private void OnSupplySelected(long index)
        {
            if (_refreshing || index < 0 || index >= _itemIds.Count) return;
            SelectItem(_itemIds[(int)index]);
        }

        private void OnTradeItemSelected(long index)
        {
            if (_refreshing || index < 0 || index >= _session.Catalog.Items.Count) return;
            SelectItem(_session.Catalog.Items.Items[(int)index].Id);
        }

        private void SelectItemInControls(string itemId)
        {
            int supplyIndex = _itemIds.IndexOf(itemId);
            if (supplyIndex >= 0) _supplyList.Select(supplyIndex, true);
            int tradeIndex = _session.Catalog.Items.Items.IndexOfId(itemId);
            if (tradeIndex >= 0) _tradeItemSelector.Select(tradeIndex);
        }

        private void ShowTradeResult(HoldfastTradeResult result)
        {
            _feedback.Text = result == null
                ? "Holdfast terminal is not connected."
                : result.Message;
            RefreshView();
        }

        private string FormatUnitValue(string itemId)
        {
            return _session.Trade.TryGetUnitValue(itemId, out long value) ? value.ToString() : "invalid";
        }

        private static string Join(string[] values)
        {
            return values == null || values.Length == 0 ? "none recorded" : string.Join(", ", values);
        }
    }

    internal static class HoldfastItemListExtensions
    {
        public static int IndexOfId(this IReadOnlyList<HoldfastItemDefinition> definitions, string id)
        {
            if (definitions == null) return -1;
            for (int i = 0; i < definitions.Count; i++)
                if (definitions[i] != null && definitions[i].Id == id) return i;
            return -1;
        }
    }
}
