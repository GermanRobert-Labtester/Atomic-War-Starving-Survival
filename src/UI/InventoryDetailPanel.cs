using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Inventory Detail panel.
    /// Shows item info, stats, and available actions for a specific item —
    /// bound to the live InventoryHostSession for a given item id.
    /// </summary>
    public partial class InventoryDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblItemInfoTitle;
        private VBoxContainer _itemInfo;
        private Label _lblItemStatsTitle;
        private VBoxContainer _itemStats;
        private Label _lblItemActionsTitle;
        private VBoxContainer _itemActions;

        private InventoryHostSession? _inventory;
        private string _itemId = string.Empty;

        public bool IsBound => _inventory != null && !string.IsNullOrEmpty(_itemId);
        public int RenderedRowCount { get; private set; }

        public void Bind(InventoryHostSession? inventory, string itemId)
        {
            _inventory = inventory;
            _itemId = itemId ?? string.Empty;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_itemInfo == null || _itemStats == null || _itemActions == null) return;

            AshfallUiHelpers.EmptyChildren(_itemInfo);
            AshfallUiHelpers.EmptyChildren(_itemStats);
            AshfallUiHelpers.EmptyChildren(_itemActions);

            RenderedRowCount = 0;

            if (_inventory?.Inventory == null || string.IsNullOrEmpty(_itemId))
            {
                _itemInfo.AddChild(MakeDimLine("No item selected."));
                return;
            }

            var slot = _inventory.Inventory.FindSlot(_itemId);
            if (slot == null)
            {
                _itemInfo.AddChild(MakeDimLine($"Item '{_itemId}' not in inventory."));
                return;
            }

            var def = slot.Item;
            int count = _inventory.Inventory.CountById(_itemId);

            // ── Item info ──
            AddRow(_itemInfo, $"Name: {def.displayName}", Ashfall.Core.UI.Theme.Pale);
            AddRow(_itemInfo, $"ID: {def.id}", Ashfall.Core.UI.Theme.Dim);
            AddRow(_itemInfo, $"Type: {def.type}", Ashfall.Core.UI.Theme.Lethe);
            AddRow(_itemInfo, $"In Stock: {count}", count > 0 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 4;

            if (!string.IsNullOrEmpty(def.description))
            {
                AddRow(_itemInfo, def.description, Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount++;
            }

            // ── Stats ──
            if (def.radProtection > 0) { AddRow(_itemStats, $"Rad Protection: {def.radProtection * 100f:0}%", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
            if (def.durability > 0) { AddRow(_itemStats, $"Durability: {def.durability:0}", Ashfall.Core.UI.Theme.Pale); RenderedRowCount++; }
            if (def.hungerRestore > 0) { AddRow(_itemStats, $"Hunger Restore: {def.hungerRestore:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
            if (def.thirstRestore > 0) { AddRow(_itemStats, $"Thirst Restore: {def.thirstRestore:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
            if (def.healthEffect > 0) { AddRow(_itemStats, $"Health Effect: +{def.healthEffect:0}", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
            if (def.radCleanse > 0) { AddRow(_itemStats, $"Rad Cleanse: −{def.radCleanse:0} mSv", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
            if (def.moraleEffect > 0) { AddRow(_itemStats, $"Morale Effect: +{def.moraleEffect:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
            if (def.tradeValue > 0) { AddRow(_itemStats, $"Trade Value: {def.tradeValue:0} (tier {def.tradeTier})", Ashfall.Core.UI.Theme.Pale); RenderedRowCount++; }
            if (def.isEquipable) { AddRow(_itemStats, $"Equipable: {def.equipSlot}", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
            if (RenderedRowCount == 4)
                _itemStats.AddChild(MakeDimLine("No special stats."));

            // ── Actions (contextual) ──
            AddRow(_itemActions, $"Consume: {(def.hungerRestore > 0 || def.thirstRestore > 0 || def.healthEffect > 0 ? "available" : "not consumable")}",
                (def.hungerRestore > 0 || def.thirstRestore > 0 || def.healthEffect > 0) ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
            AddRow(_itemActions, $"Equip: {(def.isEquipable ? "available (" + def.equipSlot + ")" : "not equipable")}",
                def.isEquipable ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount += 2;
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("ITEM DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblItemInfoTitle = AshfallUiHelpers.MakeSectionHeader("ITEM INFO");
            vbox.AddChild(_lblItemInfoTitle);
            _itemInfo = new VBoxContainer();
            _itemInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _itemInfo.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_itemInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblItemStatsTitle = AshfallUiHelpers.MakeSectionHeader("ITEM STATS");
            vbox.AddChild(_lblItemStatsTitle);
            _itemStats = new VBoxContainer();
            _itemStats.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _itemStats.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_itemStats);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblItemActionsTitle = AshfallUiHelpers.MakeSectionHeader("ACTIONS");
            vbox.AddChild(_lblItemActionsTitle);
            _itemActions = new VBoxContainer();
            _itemActions.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _itemActions.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_itemActions);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
