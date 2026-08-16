using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Shelter panel.
    /// Shows shelter status, radiation shielding, air filtration, structural integrity, and shelter upgrades.
    /// </summary>
    public partial class ShelterPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblStatusTitle;
        private VBoxContainer _statusList;
        private Label _lblRadiationTitle;
        private VBoxContainer _radiationData;
        private Label _lblStructureTitle;
        private VBoxContainer _structureList;
        private Label _lblUpgradesTitle;
        private VBoxContainer _upgradesList;

        // Placeholder shelter data
        private readonly string[] _placeholderStatus = {
            "Shelter Type: Underground Bunker",
            "Capacity: 5/20 survivors",
            "Air Filtration: 78% efficiency",
            "Radiation Shielding: 65% reduction",
            "Structural Integrity: 92%"
        };

        private readonly string[] _placeholderRadiation = {
            "Interior Radiation: 0.2 mSv/hr (Low)",
            "Exterior Radiation: 1.8 mSv/hr (Elevated)",
            "Shielding Effectiveness: 65% reduction",
            "Last Calibration: Day 10",
            "Next Maintenance: Day 20"
        };

        private readonly string[] _placeholderStructure = {
            "Main Hallway: Intact",
            "East Wing: Minor damage (repairable)",
            "West Wing: Intact",
            "North Tunnel: Sealed (blocked)",
            "South Tunnel: Active (used for ventilation)"
        };

        private readonly string[] _placeholderUpgrades = {
            "Additional Radiation Shielding — 40% complete",
            "Air Filtration Upgrade — 25% complete",
            "East Wing Repair — 60% complete",
            "Solar Power Integration — 15% complete",
            "Water Collection System — 80% complete"
        };

        // Real data from host session
        // private ShelterHostSession? _shelterHost;

        public void Bind(object shelter) // placeholder for ShelterHostSession
        {
            // _shelterHost = (ShelterHostSession)shelter;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_statusList == null || _radiationData == null || _structureList == null || _upgradesList == null) return;

            // Clear existing lists
            while (_statusList.GetChildCount() > 0)
                _statusList.RemoveChild(_statusList.GetChild(0));
            while (_radiationData.GetChildCount() > 0)
                _radiationData.RemoveChild(_radiationData.GetChild(0));
            while (_structureList.GetChildCount() > 0)
                _structureList.RemoveChild(_structureList.GetChild(0));
            while (_upgradesList.GetChildCount() > 0)
                _upgradesList.RemoveChild(_upgradesList.GetChild(0));

            // Display placeholder status
            foreach (string status in _placeholderStatus)
            {
                var label = new Label { Text = status };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _statusList.AddChild(label);
            }

            // Display placeholder radiation data
            foreach (string rad in _placeholderRadiation)
            {
                var label = new Label { Text = rad };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _radiationData.AddChild(label);
            }

            // Display placeholder structure status
            foreach (string structure in _placeholderStructure)
            {
                var label = new Label { Text = structure };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _structureList.AddChild(label);
            }

            // Display placeholder upgrades
            foreach (string upgrade in _placeholderUpgrades)
            {
                var label = new Label { Text = upgrade };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _upgradesList.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("SHELTER STATUS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Status section
            _lblStatusTitle = AshfallUiHelpers.MakeSectionHeader("SHELTER STATUS");
            vbox.AddChild(_lblStatusTitle);

            _statusList = new VBoxContainer();
            _statusList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _statusList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_statusList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Radiation section
            _lblRadiationTitle = AshfallUiHelpers.MakeSectionHeader("RADIATION & SHIELDING");
            vbox.AddChild(_lblRadiationTitle);

            _radiationData = new VBoxContainer();
            _radiationData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _radiationData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_radiationData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Structure section
            _lblStructureTitle = AshfallUiHelpers.MakeSectionHeader("STRUCTURAL INTEGRITY");
            vbox.AddChild(_lblStructureTitle);

            _structureList = new VBoxContainer();
            _structureList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _structureList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_structureList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Upgrades section
            _lblUpgradesTitle = AshfallUiHelpers.MakeSectionHeader("UPGRADES & REPAIRS");
            vbox.AddChild(_lblUpgradesTitle);

            _upgradesList = new VBoxContainer();
            _upgradesList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _upgradesList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_upgradesList);

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
