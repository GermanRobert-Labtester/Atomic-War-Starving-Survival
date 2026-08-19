using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Shelter Detail panel.
    /// Shows detailed shelter status, structural integrity, resource management, and shelter upgrades.
    /// </summary>
    public partial class ShelterDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblShelterInfoTitle;
        private VBoxContainer _shelterInfo;
        private Label _lblStructureTitle;
        private VBoxContainer _structureData;
        private Label _lblResourcesTitle;
        private VBoxContainer _shelterResources;
        private Label _lblUpgradesTitle;
        private VBoxContainer _upgradeList;

        private readonly string[] _placeholderShelterInfo = {
            "Shelter Type: Underground Bunker",
            "Capacity: 5/20 survivors",
            "Age: 15 years (Post-exchange)",
            "Condition: Good (92% integrity)",
            "Radiation Shielding: 65% reduction",
            "Air Filtration: 78% efficiency"
        };

        private readonly string[] _placeholderStructure = {
            "Main Hallway: Intact (100%)",
            "East Wing: Minor damage (85%)",
            "West Wing: Intact (100%)",
            "North Tunnel: Sealed (blocked)",
            "South Tunnel: Active (ventilation)",
            "Blast Doors: Operational (85%)"
        };

        private readonly string[] _placeholderResources = {
            "Water Storage: 500 liters (7 days)",
            "Food Storage: 200 units (15 days)",
            "Fuel Reserve: 50 liters (10 days)",
            "Medicine Stock: 30 units (5 days)",
            "Ammunition: 100 rounds (limited)",
            "Tools & Materials: Moderate stock"
        };

        private readonly string[] _placeholderUpgrades = {
            "Additional Shielding: 40% complete",
            "Air Filtration Upgrade: 25% complete",
            "East Wing Repair: 60% complete",
            "Solar Power Integration: 15% complete",
            "Water Collection System: 80% complete",
            "Next Priority: Shielding completion"
        };

        public void Bind(object shelter)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_shelterInfo == null || _structureData == null || _shelterResources == null || _upgradeList == null) return;

            AshfallUiHelpers.EmptyChildren(_shelterInfo);
            AshfallUiHelpers.EmptyChildren(_structureData);
            AshfallUiHelpers.EmptyChildren(_shelterResources);
            AshfallUiHelpers.EmptyChildren(_upgradeList);

            foreach (string info in _placeholderShelterInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _shelterInfo.AddChild(label);
            }

            foreach (string structure in _placeholderStructure)
            {
                var label = new Label { Text = structure };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _structureData.AddChild(label);
            }

            foreach (string resource in _placeholderResources)
            {
                var label = new Label { Text = resource };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _shelterResources.AddChild(label);
            }

            foreach (string upgrade in _placeholderUpgrades)
            {
                var label = new Label { Text = upgrade };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _upgradeList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("SHELTER DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblShelterInfoTitle = AshfallUiHelpers.MakeSectionHeader("SHELTER INFORMATION");
            vbox.AddChild(_lblShelterInfoTitle);

            _shelterInfo = new VBoxContainer();
            _shelterInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _shelterInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_shelterInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblStructureTitle = AshfallUiHelpers.MakeSectionHeader("STRUCTURAL INTEGRITY");
            vbox.AddChild(_lblStructureTitle);

            _structureData = new VBoxContainer();
            _structureData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _structureData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_structureData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblResourcesTitle = AshfallUiHelpers.MakeSectionHeader("SHELTER RESOURCES");
            vbox.AddChild(_lblResourcesTitle);

            _shelterResources = new VBoxContainer();
            _shelterResources.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _shelterResources.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_shelterResources);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblUpgradesTitle = AshfallUiHelpers.MakeSectionHeader("UPGRADE PROGRESS");
            vbox.AddChild(_lblUpgradesTitle);

            _upgradeList = new VBoxContainer();
            _upgradeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _upgradeList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_upgradeList);

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
