// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 125 Phase 9 — amphibious draisine panel. Presentation only: can
    /// this vehicle cross, what is the flotation/cargo margin, route risk,
    /// kit condition, pump status — with text status for colorblind safety.
    /// No real conversion pressures or procedures are shown or needed.
    /// </summary>
    public partial class AmphibiousDraisinePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _installBtn = null!;
        private Button _deployBtn = null!;
        private Button _crossBtn = null!;
        private Button _abortBtn = null!;
        private Button _repairBtn = null!;
        private Button _tickBtn = null!;

        private AmphibiousDraisineHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(AmphibiousDraisineHostSession session)
        {
            _host = session;
            if (_host != null)
                _host.StateChanged += RefreshView;
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

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;
            var engine = _host.System;
            var state = engine.FindVehicle("draisine_1");

            _statusRail.Set("phase", state?.Phase.ToString() ?? "No kit",
                state?.Phase == AmphibiousCrossingPhase.EmergencyRecovery
                    ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("pontoons",
                state != null ? $"{state.PontoonConditionBp / 100.0:0.0}%" : "—",
                state != null && state.PontoonConditionBp < 2500
                    ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("cargo",
                state != null ? $"{state.CargoLoadFraction * 100:0}%" : "—",
                AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("ingress",
                state != null ? $"{state.IngressBp / 100.0:0.0}%" : "—",
                state != null && state.IngressBp >= AmphibiousDraisineEngine.IngressEmergencyBp
                    ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            _detailText.Text = state != null
                ? $"Kit: {state.KitProfileId}. Phase: {state.Phase}. "
                + $"Cargo effect: capacity reduced to the kit's modifier while installed. "
                + $"Flotation margin is computed per crossing attempt against vehicle mass, cargo load, kit mass, and vehicle damage.\n"
                + $"Crossing progress: {state.CrossingProgressBp / 100.0:0.0}%. "
                + $"Route: {(string.IsNullOrEmpty(state.ActiveRouteClassId) ? "—" : state.ActiveRouteClassId)}. "
                + "Strong current, bad weather, or a failing pump can force an emergency recovery. Boats remain the right tool for open water."
                : "No amphibious kit installed on this vehicle. Kits install in a workshop against compatible vehicle classes; every crossing is a loadout tradeoff.";

            bool hasKit = state != null && state.Phase != AmphibiousCrossingPhase.NoKit;
            _installBtn.Disabled = hasKit;
            _deployBtn.Disabled = !hasKit || state!.Phase != AmphibiousCrossingPhase.LandReady;
            _crossBtn.Disabled = !hasKit || state!.Phase != AmphibiousCrossingPhase.WaterReady;
            _abortBtn.Disabled = !hasKit || (state!.Phase != AmphibiousCrossingPhase.Crossing && state.Phase != AmphibiousCrossingPhase.WaterReady);
            _repairBtn.Disabled = !hasKit;
            _tickBtn.Disabled = !hasKit;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Amphibious Draisine // Flooded-Corridor Mobility", minWidth: 950, minHeight: 620);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("phase", "Phase", "No kit", AshfallMetricCard.Criticality.Normal, minWidth: 170);
            _statusRail.AddCard("pontoons", "Pontoons", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("cargo", "Cargo Load", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("ingress", "Hull Ingress", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("CROSSING STATUS"));
            _detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_detailText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("KIT COMMANDS"));
            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _installBtn = AshfallUiHelpers.MakeButton("Install Kit", () => { });
            _installBtn.Pressed += () => _host?.InstallKit("draisine_1", "amphibious_draisine_mk1");
            _deployBtn = AshfallUiHelpers.MakeButton("Deploy Outriggers", () => { });
            _deployBtn.Pressed += () => _host?.Deploy("draisine_1");
            _crossBtn = AshfallUiHelpers.MakeButton("Begin Crossing", () => { });
            _crossBtn.Pressed += () => _host?.BeginCrossing("draisine_1", "route_class_flooded_rail_bed", vehicleMassFraction: 0.3f);
            _abortBtn = AshfallUiHelpers.MakeButton("Abort Crossing", () => { });
            _abortBtn.Pressed += () => _host?.AbortCrossing("draisine_1");
            row.AddChild(_installBtn);
            row.AddChild(_deployBtn);
            row.AddChild(_crossBtn);
            row.AddChild(_abortBtn);
            _contentStack.AddChild(row);
            var row2 = new HBoxContainer();
            row2.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _repairBtn = AshfallUiHelpers.MakeButton("Repair Kit", () => { });
            _repairBtn.Pressed += () => _host?.RepairKit("draisine_1");
            _tickBtn = AshfallUiHelpers.MakeButton("Advance Transition", () => { });
            _tickBtn.Pressed += () => _host?.TickTransitions("draisine_1");
            row2.AddChild(_repairBtn);
            row2.AddChild(_tickBtn);
            _contentStack.AddChild(row2);

            var note = AshfallUiHelpers.MakeBody(
                "Overloading or a damaged chassis shrinks the flotation margin below the safe minimum and the crossing is refused. Current strength, weather, and pontoon condition decide the rest — a skilled navigator lowers risk but never guarantees a crossing.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public override void _ExitTree() => Unbind();
    }
}
