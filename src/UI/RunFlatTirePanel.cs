// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 141 Phase 3 — run-flat wheel shop. Presentation only: shows fitted
    /// profile, integrity, heat, balance, safe-speed advisory, and the fuel
    /// penalty. Never claims invulnerability.
    /// </summary>
    public partial class RunFlatTirePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private static readonly string[] VehicleTags = { "road_truck", "expedition_rover", "armored_car" };
        private static readonly string[] HazardClasses = { "glass", "wire", "scrap", "rubble", "spike", "severe" };

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _wheelText = null!;
        private LineEdit _vehicleInput = null!;
        private OptionButton _tagSelector = null!;
        private OptionButton _profileSelector = null!;
        private OptionButton _hazardSelector = null!;
        private SpinBox _speedInput = null!;
        private SpinBox _loadInput = null!;
        private Button _installBtn = null!;
        private Button _hazardBtn = null!;
        private Button _heatBtn = null!;
        private Button _serviceBtn = null!;
        private string _selectedProfileId = string.Empty;

        private RunFlatTireHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(RunFlatTireHostSession session)
        {
            _host = session;
            if (_host != null) _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Vehicle Shop // Run-Flat Wheel Set", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("profile", "Profile", "NONE", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("integrity", "Integrity", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("heat", "Heat", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("speed", "Safe Speed", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("fuel", "Fuel Penalty", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
            _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("FITMENT"));

            var vehicleRow = new HBoxContainer();
            vehicleRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            vehicleRow.AddChild(AshfallUiHelpers.MakeBody("Vehicle id:"));
            _vehicleInput = new LineEdit { Text = "rover_1", CustomMinimumSize = new Vector2(220, 36) };
            vehicleRow.AddChild(_vehicleInput);
            vehicleRow.AddChild(AshfallUiHelpers.MakeBody("Tag:"));
            _tagSelector = new OptionButton { CustomMinimumSize = new Vector2(200, 36) };
            foreach (var tag in VehicleTags) _tagSelector.AddItem(tag);
            vehicleRow.AddChild(_tagSelector);
            _contentStack.AddChild(vehicleRow);

            var profileRow = new HBoxContainer();
            profileRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            profileRow.AddChild(AshfallUiHelpers.MakeBody("Profile:"));
            _profileSelector = new OptionButton { CustomMinimumSize = new Vector2(460, 36) };
            _profileSelector.ItemSelected += idx =>
            {
                var profiles = SortedProfiles();
                if ((int)idx >= 0 && (int)idx < profiles.Count) _selectedProfileId = profiles[(int)idx].id;
            };
            profileRow.AddChild(_profileSelector);
            _installBtn = new Button { Text = "Fit Run-Flat", CustomMinimumSize = new Vector2(160, 36) };
            _installBtn.Pressed += () => _host?.Install(VehicleId(), CurrentTag(), _selectedProfileId, 0.5);
            profileRow.AddChild(_installBtn);
            _contentStack.AddChild(profileRow);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ROAD HAZARD / TRAVEL"));

            var hazardRow = new HBoxContainer();
            hazardRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            hazardRow.AddChild(AshfallUiHelpers.MakeBody("Hazard:"));
            _hazardSelector = new OptionButton { CustomMinimumSize = new Vector2(150, 36) };
            foreach (var h in HazardClasses) _hazardSelector.AddItem(h);
            hazardRow.AddChild(_hazardSelector);
            hazardRow.AddChild(AshfallUiHelpers.MakeBody("Speed:"));
            _speedInput = new SpinBox { MinValue = 0, MaxValue = 140, Step = 5, Value = 50, CustomMinimumSize = new Vector2(90, 36) };
            hazardRow.AddChild(_speedInput);
            _hazardBtn = new Button { Text = "Strike Hazard", CustomMinimumSize = new Vector2(150, 36) };
            _hazardBtn.Pressed += () => _host?.ApplyHazard(VehicleId(), CurrentHazard(), (int)_speedInput.Value);
            hazardRow.AddChild(_hazardBtn);
            _contentStack.AddChild(hazardRow);

            var heatRow = new HBoxContainer();
            heatRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            heatRow.AddChild(AshfallUiHelpers.MakeBody("Load:"));
            _loadInput = new SpinBox { MinValue = 0, MaxValue = 200, Step = 10, Value = 50, CustomMinimumSize = new Vector2(90, 36) };
            heatRow.AddChild(_loadInput);
            _heatBtn = new Button { Text = "Travel Leg (heat tick)", CustomMinimumSize = new Vector2(200, 36) };
            _heatBtn.Pressed += () => _host?.TickHeat(VehicleId(), (int)_speedInput.Value, (int)_loadInput.Value);
            heatRow.AddChild(_heatBtn);
            _serviceBtn = new Button { Text = "Service Wheel", CustomMinimumSize = new Vector2(150, 36) };
            _serviceBtn.Pressed += () => _host?.Repair(VehicleId(), 0.5);
            heatRow.AddChild(_serviceBtn);
            _contentStack.AddChild(heatRow);

            var note = AshfallUiHelpers.MakeBody(
                "Run-flat resilience is real but never absolute. Severe impacts can still bend a rim or destroy a weakened wheel, and the extra mass and drag cost fuel and build heat.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _wheelText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_wheelText);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        private string VehicleId() => string.IsNullOrWhiteSpace(_vehicleInput?.Text) ? "rover_1" : _vehicleInput!.Text.Trim();
        private string CurrentTag() => VehicleTags[Math.Max(0, _tagSelector?.Selected ?? 0)];
        private string CurrentHazard() => HazardClasses[Math.Max(0, _hazardSelector?.Selected ?? 0)];

        private List<RunFlatProfileDef> SortedProfiles()
        {
            var list = new List<RunFlatProfileDef>();
            if (_host != null)
            {
                foreach (var p in _host.System.Catalog.All) list.Add(p);
                list.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            }
            return list;
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var wheel = _host.System.FindWheelSet(VehicleId());
            var profile = wheel == null ? null : _host.System.Catalog.Get(wheel.ProfileId);

            _statusRail.Set("profile", profile?.id ?? "NONE",
                profile == null ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("integrity", wheel == null ? "—" : $"{wheel.IntegrityBp}%",
                wheel != null && wheel.IntegrityBp < 40 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("heat", wheel == null ? "—" : $"{wheel.HeatC}C",
                wheel != null && wheel.HeatC >= RunFlatTireEngine.OverheatThresholdC - 20
                    ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("speed", wheel == null ? "—" : $"{_host.System.GetSafeSpeedKph(VehicleId())} kph",
                AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("fuel", wheel == null ? "—" : $"+{_host.System.GetFuelPenaltyPct(VehicleId())}%",
                AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text =
                    $"Fitted wheel sets: {_host.System.State.WheelSets.Count} | Hazard events: {_host.System.State.TotalHazards}\n" +
                    $"Ambient: {_host.AmbientTempC()}C | Last event: {_host.LastEvent}";
            }

            SyncSelectors();

            if (_wheelText != null)
            {
                if (_host.System.State.WheelSets.Count == 0)
                {
                    _wheelText.Text = "No run-flat wheel sets fitted.";
                }
                else
                {
                    var lines = new List<string>();
                    foreach (var w in _host.System.State.WheelSets)
                    {
                        lines.Add(
                            $"{w.VehicleId}: {w.ProfileId} | integrity {w.IntegrityBp}% | heat {w.HeatC}C | " +
                            $"balance {w.ImbalanceBp}% | rim/bead {w.RimBeadBp}% | wear {w.WearBp}% | punctures {w.PunctureCount} | " +
                            $"safe {_host.System.GetSafeSpeedKph(w.VehicleId)} kph | fuel +{_host.System.GetFuelPenaltyPct(w.VehicleId)}%");
                    }
                    _wheelText.Text = string.Join("\n", lines);
                }
            }
        }

        private void SyncSelectors()
        {
            if (_host == null || _profileSelector == null) return;
            var profiles = SortedProfiles();
            _profileSelector.Clear();
            int selected = 0;
            for (int i = 0; i < profiles.Count; i++)
            {
                _profileSelector.AddItem($"{profiles[i].display_name} [{string.Join("/", profiles[i].compatible_vehicle_tags)}]", i);
                if (profiles[i].id == _selectedProfileId || (string.IsNullOrEmpty(_selectedProfileId) && i == 0))
                {
                    selected = i;
                    _selectedProfileId = profiles[i].id;
                }
            }
            _profileSelector.Selected = selected;
            if (_installBtn != null) _installBtn.Disabled = profiles.Count == 0;
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
