using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Inventory panel with real data binding.
    /// Shows storage and gear from InventoryHostSession with item icons and tactile 9-slice framing.
    /// </summary>
    public partial class InventoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _storageGrid = null!;
        private VBoxContainer _gearGrid = null!;
        private Label _weightLabel = null!;

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

            while (_storageGrid.GetChildCount() > 0)
                _storageGrid.RemoveChild(_storageGrid.GetChild(0));
            while (_gearGrid.GetChildCount() > 0)
                _gearGrid.RemoveChild(_gearGrid.GetChild(0));

            if (_inventoryHost != null)
            {
                var inventory = _inventoryHost.Inventory;
                _weightLabel.Text = $"CAPACITY // {inventory.Slots.Count} STACKS · {inventory.GetCurrentWeight():0.0}/{inventory.MaxWeight:0} KG";

                if (inventory.Slots.Count == 0)
                {
                    _storageGrid.AddChild(AshfallUiHelpers.MakeMetadata("Nothing stored. The shelter shelves are bare."));
                }
                else
                {
                    for (int i = 0; i < inventory.Slots.Count; i++)
                    {
                        var slot = inventory.Slots[i];
                        if (slot?.Item == null) continue;

                        var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                        var icon = AshfallUiHelpers.MakeItemIcon(slot.Item.id, 26);
                        row.AddChild(icon);

                        var name = AshfallUiHelpers.MakeSmall(slot.Item.displayName);
                        name.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                        row.AddChild(name);

                        var count = AshfallUiHelpers.MakeMono($"×{slot.Amount}");
                        count.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                        row.AddChild(count);

                        _storageGrid.AddChild(row);
                    }
                }

                var gearRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var gearIcon = AshfallUiHelpers.MakeItemIcon("item_lead_plate", 24);
                gearRow.AddChild(gearIcon);
                var gearText = AshfallUiHelpers.MakeSmall(_inventoryHost.EquipLine());
                gearText.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                gearRow.AddChild(gearText);
                _gearGrid.AddChild(gearRow);
            }
            else
            {
                _weightLabel.Text = "CAPACITY // 8 STACKS · 14.5/40.0 KG";
                string[] sampleItems = {
                    "item_resin_adhesive", "item_lead_plate", "item_potassium_iodide",
                    "item_geiger_m3", "item_air_filter_hepa", "item_desal_membrane",
                    "item_brine_salt", "item_dosimeter_pen"
                };
                foreach (string itemId in sampleItems)
                {
                    var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    var icon = AshfallUiHelpers.MakeItemIcon(itemId, 24);
                    row.AddChild(icon);

                    var name = AshfallUiHelpers.MakeSmall(itemId.Replace('_', ' ').ToUpperInvariant());
                    name.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                    row.AddChild(name);

                    var count = AshfallUiHelpers.MakeMono("×01");
                    row.AddChild(count);

                    _storageGrid.AddChild(row);
                }

                _gearGrid.AddChild(AshfallUiHelpers.MakeSmall("EQUIPPED // Hazmat Suit [Worn 72%] · Quartz Dosimeter Pen · Gas Mask M3"));
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(700, 560);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("SHELTER STORAGE & GEAR", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _weightLabel = AshfallUiHelpers.MakeMono("CAPACITY // --");
            _weightLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot));
            vbox.AddChild(_weightLabel);

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(660, 410),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("EQUIPPED FIELD GEAR"));
            _gearGrid = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_gearGrid);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("STORAGE MANIFEST"));
            _storageGrid = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_storageGrid);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
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
