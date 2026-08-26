using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Inventory Detail panel.
    /// Shows detailed item information, item properties, and item actions.
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

        // Placeholder item data
        private readonly string[] _placeholderItemInfo = {
            "Item: Gas Mask (Basic)",
            "Type: Protective Equipment",
            "Rarity: Common",
            "Condition: 85% (Worn)",
            "Weight: 2.5 kg",
            "Value: 15 units"
        };

        private readonly string[] _placeholderItemStats = {
            "Protection: 40% radiation reduction",
            "Durability: 75/100 uses remaining",
            "Compatibility: Standard gas mask filter",
            "Special: Can be repaired with cloth",
            "Effects: Reduces radiation exposure"
        };

        private readonly string[] _placeholderItemActions = {
            "Equip — Wear the gas mask",
            "Unequip — Remove from inventory",
            "Use — Apply to current situation",
            "Repair — Fix worn condition (requires cloth)",
            "Discard — Remove from inventory permanently",
            "Trade — Offer to another survivor or faction"
        };

        // Real data from host session
        // private InventoryHostSession? _inventoryHost;
        // private string _selectedItemId;

        public void Bind(object inventory, string itemId) // placeholder for InventoryHostSession
        {
            // _inventoryHost = (InventoryHostSession)inventory;
            // _selectedItemId = itemId;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_itemInfo == null || _itemStats == null || _itemActions == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_itemInfo);
            AshfallUiHelpers.EmptyChildren(_itemStats);
            AshfallUiHelpers.EmptyChildren(_itemActions);

            // Display placeholder item info
            foreach (string info in _placeholderItemInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _itemInfo.AddChild(label);
            }

            // Display placeholder item stats
            foreach (string stats in _placeholderItemStats)
            {
                var label = new Label { Text = stats };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _itemStats.AddChild(label);
            }

            // Display placeholder item actions
            foreach (string action in _placeholderItemActions)
            {
                var label = new Label { Text = action };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _itemActions.AddChild(label);
            }
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

            // Item info section
            _lblItemInfoTitle = AshfallUiHelpers.MakeSectionHeader("ITEM INFORMATION");
            vbox.AddChild(_lblItemInfoTitle);

            _itemInfo = new VBoxContainer();
            _itemInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _itemInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_itemInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Item stats section
            _lblItemStatsTitle = AshfallUiHelpers.MakeSectionHeader("ITEM STATISTICS");
            vbox.AddChild(_lblItemStatsTitle);

            _itemStats = new VBoxContainer();
            _itemStats.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _itemStats.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_itemStats);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Item actions section
            _lblItemActionsTitle = AshfallUiHelpers.MakeSectionHeader("ITEM ACTIONS");
            vbox.AddChild(_lblItemActionsTitle);

            _itemActions = new VBoxContainer();
            _itemActions.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _itemActions.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_itemActions);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
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
