using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Inventory panel (HYBRID, lightweight).
    /// Single-purpose modal surface. Wraps the existing list rendering in the
    /// dashboard shell for Phase 13 sidebar + status rail consistency, but does
    /// NOT clone the Survival Workstation. DataGrid is intentionally NOT
    /// applied here — the data is row-major stack listing, where the existing
    /// icon + count rows already serve readability well.
    /// </summary>
    public partial class InventoryPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _storageGrid = null!;
        private VBoxContainer _gearGrid = null!;
        private Label _weightLabel = null!;

        private InventoryHostSession? _inventoryHost;
        private string _activeFilter = "all"; // all | consumable | material | medical | equipment

        public bool IsBound => _inventoryHost != null;

        public void Bind(InventoryHostSession inventory)
        {
            _inventoryHost = inventory;
            if (_inventoryHost != null)
            {
                _inventoryHost.StateChanged -= RefreshView;
                _inventoryHost.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public void RefreshView()
        {
            RefreshStatusRail();
            RefreshStorageList();
            RefreshGearList();
        }

        private void RefreshStatusRail()
        {
            if (_statusRail == null) return;
            if (_inventoryHost == null)
            {
                _statusRail.Set("stacks",  "0",   AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("weight",  "—",   AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("equip",   "—",   AshfallMetricCard.Criticality.Normal);
                return;
            }
            int stacks = _inventoryHost.Inventory.Slots.Count;
            float cw = _inventoryHost.Inventory.GetCurrentWeight();
            float mw = _inventoryHost.Inventory.MaxWeight;
            _statusRail.Set("stacks", $"{stacks}",
                stacks == 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("weight", $"{cw:0.0}/{mw:0} kg",
                cw >= mw ? AshfallMetricCard.Criticality.Critical
                : cw >= mw * 0.85 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("equip", _inventoryHost.EquipLine());
        }

        private void RefreshStorageList()
        {
            if (_storageGrid == null) return;
            AshfallUiHelpers.EmptyChildren(_storageGrid);
            if (_inventoryHost == null || _inventoryHost.Inventory.Slots.Count == 0)
            {
                _storageGrid.AddChild(AshfallUiHelpers.MakeMetadata("Nothing stored. The shelter shelves are bare."));
                return;
            }
            foreach (var slot in _inventoryHost.Inventory.Slots)
            {
                if (slot?.Item == null) continue;
                if (!FilterPass(slot.Item)) continue;
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var icon = AshfallUiHelpers.MakeItemIcon(slot.Item.id, 26);
                row.AddChild(icon);
                var name = AshfallUiHelpers.MakeSmall(slot.Item.displayName);
                name.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(name);
                var count = AshfallUiHelpers.MakeMono($"×{slot.Amount}");
                count.AddThemeColorOverride("font_color",
                    slot.Amount <= 1 ? AshfallUiHelpers.ToColor(DesignTheme.Warm) : AshfallUiHelpers.ToColor(DesignTheme.Hot));
                row.AddChild(count);
                _storageGrid.AddChild(row);
            }
            if (_storageGrid.GetChildCount() == 0)
                _storageGrid.AddChild(AshfallUiHelpers.MakeMetadata("No items in this filter."));
        }

        private void RefreshGearList()
        {
            if (_gearGrid == null) return;
            AshfallUiHelpers.EmptyChildren(_gearGrid);
            if (_inventoryHost == null)
            {
                _gearGrid.AddChild(AshfallUiHelpers.MakeSmall("EQUIPPED // —"));
                return;
            }
            _gearGrid.AddChild(AshfallUiHelpers.MakeSmall(_inventoryHost.EquipLine()));
        }

        private bool FilterPass(Ashfall.Core.Inventory.ItemDefinition item)
        {
            return _activeFilter switch
            {
                "consumable" => item.type == Ashfall.Core.Inventory.ItemType.Food
                    || item.type == Ashfall.Core.Inventory.ItemType.Water,
                "material" => item.type == Ashfall.Core.Inventory.ItemType.Material,
                "medical" => item.type == Ashfall.Core.Inventory.ItemType.Medical
                    || item.type == Ashfall.Core.Inventory.ItemType.Iodine
                    || item.type == Ashfall.Core.Inventory.ItemType.AntiRad,
                "equipment" => item.type == Ashfall.Core.Inventory.ItemType.Filter,
                _ => true,
            };
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            _shell = new AshfallDashboardShell(
                "SHELTER STORAGE & GEAR",
                1100, 720);

            var hostContainer = new MarginContainer();
            hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.HudEdge);
            hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
            hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.HudEdge);
            hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
            hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            hostContainer.AddChild(_shell);
            AddChild(hostContainer);

            _shell.SetSidebar(new[]
            {
                new AshfallSidebar.Item { Id = "filter_all",       Label = "Filter: All",          Hint = "everything" },
                new AshfallSidebar.Item { Id = "filter_consumable",Label = "Filter: Consumable",   Hint = "food · water" },
                new AshfallSidebar.Item { Id = "filter_material",  Label = "Filter: Material",     Hint = "scrap stacks" },
                new AshfallSidebar.Item { Id = "filter_medical",   Label = "Filter: Medical",      Hint = "treatments" },
                new AshfallSidebar.Item { Id = "filter_equipment", Label = "Filter: Equipment",    Hint = "filters · tools" },
            }, "STORAGE OPS", "filter_all");
            if (_sidebar != null)
                _sidebar.OnSelected += id => SetFilter(id);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("stacks", "STACKS",     "0",   AshfallMetricCard.Criticality.Normal, 100);
            _statusRail.AddCard("weight", "WEIGHT",     "—",   AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("equip",  "EQUIP LINE", "—",   AshfallMetricCard.Criticality.Normal, 320);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

            BuildContent();
            RefreshView();
        }

        private AshfallSidebar? _sidebar;

        private void SetFilter(string id)
        {
            _activeFilter = id switch
            {
                "filter_consumable" => "consumable",
                "filter_material" => "material",
                "filter_medical" => "medical",
                "filter_equipment" => "equipment",
                _ => "all",
            };
            RefreshStorageList();
        }

        private void BuildContent()
        {
            var content = new HBoxContainer();
            content.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            content.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            content.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

            var listCol = new VBoxContainer();
            listCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            listCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            listCol.SizeFlagsStretchRatio = 1.45f;
            listCol.AddChild(AshfallUiHelpers.MakeSectionHeader("EQUIPPED FIELD GEAR"));
            _gearGrid = new VBoxContainer();
            _gearGrid.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _gearGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            listCol.AddChild(_gearGrid);
            listCol.AddChild(AshfallUiHelpers.MakeSeparator());
            listCol.AddChild(AshfallUiHelpers.MakeSectionHeader("STORAGE MANIFEST"));
            _storageGrid = new VBoxContainer();
            _storageGrid.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _storageGrid.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _storageGrid.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            listCol.AddChild(_storageGrid);
            content.AddChild(listCol);

            var detailCol = new VBoxContainer();
            detailCol.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            detailCol.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            detailCol.SizeFlagsStretchRatio = 1.0f;
            var detailPanel = AshfallUiHelpers.MakePanel();
            detailPanel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            detailPanel.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            var dMargin = new MarginContainer();
            dMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingMd);
            dMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingMd);
            dMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingMd);
            dMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
            detailPanel.AddChild(dMargin);
            var dVBox = new VBoxContainer();
            dVBox.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            dMargin.AddChild(dVBox);
            dVBox.AddChild(AshfallUiHelpers.MakeSectionHeader("STORAGE PRINCIPLES"));
            dVBox.AddChild(AshfallUiHelpers.MakeBody("Materials that stretch the shelter budget — iodine pills, water filters, " +
                "rad-away — should be rationed before comfort stocks. Consumables spoil; materials don't."));
            dVBox.AddChild(AshfallUiHelpers.MakeSmall("Weapons and armor live in loadouts, not stack counts; consult the survivor panel."));
            _weightLabel = AshfallUiHelpers.MakeSmall("CAPACITY // —");
            _weightLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
            dVBox.AddChild(_weightLabel);
            detailCol.AddChild(detailPanel);
            content.AddChild(detailCol);

            _shell.SetContent(content);
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
