// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using System.Linq;
using Godot;
using Ashfall.Core.Combat;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public abstract partial class Plans74To77PanelBase : Control
    {
        protected static Label Label(string text) => AshfallUiHelpers.MakeMono(text);
        protected static LineEdit Field(string placeholder, string value = "")
        {
            var field = new LineEdit { PlaceholderText = placeholder, Text = value };
            field.CustomMinimumSize = new Vector2(180, 28);
            return field;
        }

        protected static Button Action(string text, Action pressed)
            => AshfallUiHelpers.MakeButton(text, pressed);

        protected static void AddRow(VBoxContainer root, string caption, Control control)
        {
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            row.AddChild(Label(caption));
            row.AddChild(control);
            root.AddChild(row);
        }

        protected void CloseOnEscape()
        {
            if (Input.IsActionPressed(AshfallInputActions.Close) || Input.IsActionPressed(AshfallInputActions.UiCancel))
                Visible = false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }
    }

    public partial class GeothermalOrcPanel : Plans74To77PanelBase, IBindablePanel
    {
        public event Action<string, string>? OnActionRequested;
        public event Action? OnClose;

        private GeothermalOrcHostSession? _host;
        private Label _status = null!;
        private Label _telemetry = null!;
        private LineEdit _loopId = null!;
        private LineEdit _stratumId = null!;
        private LineEdit _flow = null!;

        public bool IsBound => _host != null;

        public void Bind(GeothermalOrcHostSession session)
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = session;
            _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            var shell = new AshfallDashboardShell("B74 // GEOTHERMAL ORC LOOP", 920, 600);
            AddChild(shell);
            shell.AttachHeaderCloseButton("CLOSE", () => { Visible = false; OnClose?.Invoke(); });
            var root = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            shell.SetContent(root);

            _status = Label("ORC STATUS // NO LOOP SELECTED");
            _telemetry = Label("OUTPUT 0 kW // WASTE HEAT 0 kW // FOULING 0%");
            root.AddChild(_status);
            root.AddChild(_telemetry);
            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _loopId = Field("loop id", "loop_bay_01");
            _stratumId = Field("stratum id", "borehole_stratum_steam_reservoir");
            _flow = Field("flow L/min", "200");
            AddRow(root, "LOOP", _loopId);
            AddRow(root, "STRATUM", _stratumId);
            root.AddChild(Action("ADD LOOP", () => OnActionRequested?.Invoke(
                "add_loop", $"{_loopId.Text}|{_stratumId.Text}")));
            AddRow(root, "FLOW", _flow);
            root.AddChild(Action("SET FLOW", () => OnActionRequested?.Invoke(
                "set_flow", $"{_loopId.Text}|{_flow.Text}")));

            var controls = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            controls.AddChild(Action("COMMISSION", () => OnActionRequested?.Invoke("commission", _loopId.Text)));
            controls.AddChild(Action("DESCALE", () => OnActionRequested?.Invoke("descale", _loopId.Text)));
            controls.AddChild(Action("REPAIR", () => OnActionRequested?.Invoke("repair", _loopId.Text)));
            root.AddChild(controls);
            Visible = false;
        }

        public void RefreshView()
        {
            if (_host == null || _status == null) return;
            var snapshot = _host.Snapshot;
            _status.Text = snapshot.Active
                ? $"ORC STATUS // {snapshot.LoopId} ACTIVE // LEAK {snapshot.LeakState.ToString().ToUpperInvariant()}"
                : "ORC STATUS // NO ACTIVE LOOP";
            _telemetry.Text =
                $"OUTPUT {snapshot.ElectricalOutputKw:0} kW // WASTE HEAT {snapshot.WasteHeatKw:0} kW // " +
                $"FOULING {snapshot.FoulingPct:0}% // RESERVE {snapshot.ThermalReservePct:0}% // " +
                $"TURBINE {snapshot.TurbineConditionPct:0}%";
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }
    }

    public partial class BallisticsWorkbenchPanel : Plans74To77PanelBase, IBindablePanel
    {
        public event Action<string, string>? OnActionRequested;
        public event Action? OnClose;

        private BallisticsWorkbenchHostSession? _host;
        private Label _status = null!;
        private Label _profiles = null!;
        private LineEdit _weaponId = null!;
        private LineEdit _profileId = null!;
        private LineEdit _opticItemId = null!;

        public bool IsBound => _host != null;

        public void Bind(BallisticsWorkbenchHostSession session)
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = session;
            _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            var shell = new AshfallDashboardShell("B75 // BALLISTICS WORKBENCH", 920, 600);
            AddChild(shell);
            shell.AttachHeaderCloseButton("CLOSE", () => { Visible = false; OnClose?.Invoke(); });
            var root = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            shell.SetContent(root);
            _status = Label("WORKBENCH // NO WEAPON PROFILE SELECTED");
            _profiles = Label("PROFILES // 0");
            root.AddChild(_status);
            root.AddChild(_profiles);
            root.AddChild(AshfallUiHelpers.MakeSeparator());
            _weaponId = Field("canonical weapon instance id");
            _profileId = Field("profile id or weapon tag", "rifle");
            _opticItemId = Field("completed optic item id");
            AddRow(root, "WEAPON", _weaponId);
            AddRow(root, "PROFILE", _profileId);
            AddRow(root, "OPTIC ITEM", _opticItemId);
            root.AddChild(Action("REGISTER PROFILE", () => OnActionRequested?.Invoke(
                "ensure", $"{_weaponId.Text}|{_profileId.Text}")));
            var controls = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            controls.AddChild(Action("INSPECT", () => OnActionRequested?.Invoke("inspect", _weaponId.Text)));
            controls.AddChild(Action("CALIBRATE", () => OnActionRequested?.Invoke("calibrate", _weaponId.Text)));
            controls.AddChild(Action("REFURBISH", () => OnActionRequested?.Invoke("refurbish", _weaponId.Text)));
            controls.AddChild(Action("ATTACH OPTIC", () => OnActionRequested?.Invoke(
                "attach_optic", $"{_weaponId.Text}|{_profileId.Text}|{_opticItemId.Text}")));
            root.AddChild(controls);
            root.AddChild(Label("FIRING wear is recorded by TacticalCombatSystem; this bench owns calibration, service, and optic mount."));
            Visible = false;
        }

        public void RefreshView()
        {
            if (_host == null || _status == null) return;
            _profiles.Text = $"PROFILES // {_host.System.Profiles.Count} CALIBRATED TOKENS";
            if (_host.System.Profiles.TryGetValue(_weaponId?.Text ?? string.Empty, out var profile))
            {
                _status.Text =
                    $"WEAPON {profile.WeaponInstanceId} // HEADSPACE {profile.HeadspaceState.ToString().ToUpperInvariant()} // " +
                    $"ROUNDS {profile.TotalRounds} // CALIBRATION {profile.CalibrationQuality:0.00} // " +
                    $"DISPERSION {profile.DispersionMoa:0.00} MOA";
            }
            else
            {
                _status.Text = "WORKBENCH // ENTER A WEAPON INSTANCE ID";
            }
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }
    }

    public partial class AeroponicsPanel : Plans74To77PanelBase, IBindablePanel
    {
        public event Action<string, string>? OnActionRequested;
        public event Action? OnClose;

        private AeroponicsHostSession? _host;
        private Label _status = null!;
        private LineEdit _chamberId = null!;
        private LineEdit _cropId = null!;
        private LineEdit _profileId = null!;

        public bool IsBound => _host != null;

        public void Bind(AeroponicsHostSession session)
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = session;
            _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            var shell = new AshfallDashboardShell("B76 // AEROPONIC CHAMBER", 920, 600);
            AddChild(shell);
            shell.AttachHeaderCloseButton("CLOSE", () => { Visible = false; OnClose?.Invoke(); });
            var root = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            shell.SetContent(root);
            _status = Label("CHAMBER // NO CYCLE SELECTED");
            root.AddChild(_status);
            root.AddChild(AshfallUiHelpers.MakeSeparator());
            _chamberId = Field("chamber id", "aero_chamber_01");
            _cropId = Field("crop cycle id", "cycle_day_01");
            _profileId = Field("nutrient profile", "crop_aeroponic_food_leaf");
            AddRow(root, "CHAMBER", _chamberId);
            AddRow(root, "CROP CYCLE", _cropId);
            AddRow(root, "PROFILE", _profileId);
            var controls = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            controls.AddChild(Action("ADD CHAMBER", () => OnActionRequested?.Invoke("add_chamber", _chamberId.Text)));
            controls.AddChild(Action("PLANT", () => OnActionRequested?.Invoke(
                "plant", $"{_chamberId.Text}|{_cropId.Text}|{_profileId.Text}")));
            controls.AddChild(Action("ADD WATER", () => OnActionRequested?.Invoke("water", _chamberId.Text)));
            controls.AddChild(Action("HARVEST", () => OnActionRequested?.Invoke("harvest", _chamberId.Text)));
            root.AddChild(controls);
            root.AddChild(Label("Chemistry and nozzle service remain explicit maintenance actions in the same chamber authority."));
            Visible = false;
        }

        public void RefreshView()
        {
            if (_host == null || _status == null) return;
            var chamber = _host.System.State.Chambers.FirstOrDefault(
                item => item.ChamberId == (_chamberId?.Text ?? string.Empty));
            if (chamber == null)
            {
                _status.Text = "CHAMBER // ENTER OR ADD A CHAMBER ID";
                return;
            }
            _status.Text =
                $"CHAMBER {chamber.ChamberId} // CYCLE {chamber.CropCycleId:-} // " +
                $"GROWTH {chamber.GrowthPct:0}% // EC {chamber.ReservoirEcMsCm:0.00} // " +
                $"pH {chamber.ReservoirPh:0.00} // NOZZLES {chamber.NozzleConditionPct:0}% // " +
                $"DISEASE {chamber.DiseaseStage.ToString().ToUpperInvariant()}";
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }
    }

    public partial class PneumaticDispatchPanel : Plans74To77PanelBase, IBindablePanel
    {
        public event Action<string, string>? OnActionRequested;
        public event Action? OnClose;

        private PneumaticDispatchHostSession? _host;
        private Label _status = null!;
        private LineEdit _source = null!;
        private LineEdit _destination = null!;
        private LineEdit _item = null!;
        private LineEdit _amount = null!;
        private LineEdit _capsule = null!;
        private LineEdit _link = null!;

        public bool IsBound => _host != null;

        public void Bind(PneumaticDispatchHostSession session)
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = session;
            _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null) _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            var shell = new AshfallDashboardShell("B77 // PNEUMATIC DISPATCH", 920, 650);
            AddChild(shell);
            shell.AttachHeaderCloseButton("CLOSE", () => { Visible = false; OnClose?.Invoke(); });
            var root = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            shell.SetContent(root);
            _status = Label("NETWORK // OFFLINE");
            root.AddChild(_status);
            root.AddChild(AshfallUiHelpers.MakeSeparator());
            _source = Field("source station", "room_station_clinic");
            _destination = Field("destination station", "room_station_armory");
            _item = Field("canonical item id", "item_pneumatic_capsule_50mm");
            _amount = Field("amount", "1");
            _capsule = Field("capsule id for jam clear");
            _link = Field("link id for maintenance", "pneumatic_link_clinic_armory");
            AddRow(root, "FROM", _source);
            AddRow(root, "TO", _destination);
            AddRow(root, "CARGO", _item);
            AddRow(root, "COUNT", _amount);
            var dispatch = Action("DISPATCH NORMAL", () => OnActionRequested?.Invoke(
                "dispatch", $"{_source.Text}|{_destination.Text}|{_item.Text}|{_amount.Text}"));
            root.AddChild(dispatch);
            AddRow(root, "CAPSULE", _capsule);
            AddRow(root, "LINK", _link);
            var maintenance = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            maintenance.AddChild(Action("CLEAR JAM", () => OnActionRequested?.Invoke("clear_jam", _capsule.Text)));
            maintenance.AddChild(Action("MAINTAIN LINK", () => OnActionRequested?.Invoke("maintain", _link.Text)));
            maintenance.AddChild(Action("TOGGLE BLACKOUT", () => OnActionRequested?.Invoke("blackout", "")));
            root.AddChild(maintenance);
            root.AddChild(Label("Voice pipes remain available during blackout; cargo does not."));
            Visible = false;
        }

        public void RefreshView()
        {
            if (_host == null || _status == null) return;
            var snapshot = _host.System.Snapshot();
            _status.Text =
                $"NETWORK // {(snapshot.Blackout ? "BLACKOUT" : "PRESSURIZED")} // " +
                $"PRESSURE {snapshot.PressureDifferentialKpa:0.0} kPa // SEALS {snapshot.SealEfficiency:0.00} // " +
                $"QUEUE {snapshot.QueueCount} // IN TRANSIT {snapshot.InTransitCount} // " +
                $"VOICE {(snapshot.VoicePipesAvailable ? "AVAILABLE" : "DOWN")}";
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }
    }
}
