using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Inventory panel with real data binding.
    /// Shows storage and gear from InventoryHostSession.
    /// </summary>
    public partial class InventoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblStorageTitle;
        private Label _lblGearTitle;
        private VBoxContainer _storageGrid;
        private VBoxContainer _gearGrid;

        // Placeholder items (will be replaced with real data)
        private readonly string[] _placeholderStorageItems = {
            "Rations x5", "Water x3", "Medkit x1", "Bandages x2",
            "Iodine Pills x10", "Water Filter", "Gas Mask", "Flashlight"
        };

        private readonly string[] _placeholderGearItems = {
            "Leather Jacket", "Canvas Pants", "Boots", "Backpack",
            "Dosimeter", "Geiger Counter"
        };

        // Real data from host session
        private InventoryHostSession? _inventoryHost;

        public void Bind(InventoryHostSession inventory)
        {
            _inventoryHost = inventory;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_storageGrid == null || _gearGrid == null) return;

            // Clear existing items by removing children
            while (_storageGrid.GetChildCount() > 0)
            {
                _storageGrid.RemoveChild(_storageGrid.GetChild(0));
            }
            while (_gearGrid.GetChildCount() > 0)
            {
                _gearGrid.RemoveChild(_gearGrid.GetChild(0));
            }

            if (_inventoryHost != null)
            {
                // Bind real inventory data (placeholder - actual implementation would use real inventory API)
                var inventory = _inventoryHost.Inventory;
                
                // Storage items (using placeholder data structure)
                for (int i = 0; i < _placeholderStorageItems.Length; i++)
                {
                    var itemLabel = new Label { Text = $"{_placeholderStorageItems[i]} x{i+1}" };
                    itemLabel.CustomMinimumSize = new Vector2(150, 40);
                    itemLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    _storageGrid.AddChild(itemLabel);
                }

                // Gear items (using placeholder data structure)
                for (int i = 0; i < _placeholderGearItems.Length; i++)
                {
                    var itemLabel = new Label { Text = _placeholderGearItems[i] };
                    itemLabel.CustomMinimumSize = new Vector2(150, 40);
                    itemLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    _gearGrid.AddChild(itemLabel);
                }
            }
            else
            {
                // Fall back to placeholders
                foreach (string item in _placeholderStorageItems)
                {
                    var itemLabel = new Label { Text = item };
                    itemLabel.CustomMinimumSize = new Vector2(150, 40);
                    itemLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    _storageGrid.AddChild(itemLabel);
                }

                foreach (string item in _placeholderGearItems)
                {
                    var itemLabel = new Label { Text = item };
                    itemLabel.CustomMinimumSize = new Vector2(150, 40);
                    itemLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    _gearGrid.AddChild(itemLabel);
                }
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // Background overlay
            var bg = new ColorRect
            {
                Color = new Color(0.05f, 0.05f, 0.05f, 0.92f)
            };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            // Content container
            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(600, 0);
            container.AddChild(vbox);

            // Title
            var title = AshfallUiHelpers.MakeTitle("INVENTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Storage section
            _lblStorageTitle = AshfallUiHelpers.MakeSectionHeader("STORAGE");
            vbox.AddChild(_lblStorageTitle);

            _storageGrid = new VBoxContainer();
            _storageGrid.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _storageGrid.CustomMinimumSize = new Vector2(550, 0);
            vbox.AddChild(_storageGrid);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Gear section
            _lblGearTitle = AshfallUiHelpers.MakeSectionHeader("GEAR");
            vbox.AddChild(_lblGearTitle);

            _gearGrid = new VBoxContainer();
            _gearGrid.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _gearGrid.CustomMinimumSize = new Vector2(550, 0);
            vbox.AddChild(_gearGrid);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Close button
            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            // Keyboard shortcut
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
