// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 122 Phase 9 — SOFC plant panel. Presentation only: answers the
    /// flagship questions (online? why limited? fuel clean? stack healthy?
    /// waste heat? startup progress?). Reads the session; never invents
    /// state and never claims perfect silence.
    /// </summary>
    public partial class SolidOxideFuelCellPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _preheatBtn = null!;
        private Button _shutdownBtn = null!;
        private Button _maintainBtn = null!;

        private SofcPowerHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(SofcPowerHostSession session)
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
            var state = engine.State;
            var result = _host.LastTickResult;

            _statusRail.Set("mode", state.Mode.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("output",
                result != null ? $"{result.DispatchedOutputKw:0.0} kW" : "—",
                AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("health",
                SofcStackHealth.Classify(state.StackHealthBp),
                state.StackHealthBp < 4000 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("waste",
                result != null ? $"{result.WasteHeatKw:0.0} kW" : "—",
                AshfallMetricCard.Criticality.Normal);

            var why = result?.FailureCode != null
                ? $"Limited: {result.FailureCode}."
                : state.Mode switch
                {
                    SofcOperatingMode.Preheating => "Preheating — the stack must reach its operating band before any output.",
                    SofcOperatingMode.Stabilizing => "Stabilizing — output begins next tick.",
                    SofcOperatingMode.CoolingDown => "Cooling down before it can restart.",
                    SofcOperatingMode.Faulted => "Faulted — maintenance is required before restart.",
                    _ => "Standing by."
                };
            _detailText.Text = $"{why}\n"
                + $"Fuel quality (last burn): {(string.IsNullOrEmpty(state.LastFuelQualityClass) ? "—" : state.LastFuelQualityClass)}\n"
                + $"Stack thermal level: {state.ThermalLevel:0.00} · thermal cycles: {state.ThermalCycles}\n"
                + $"Acoustic signature: {result?.AcousticSignatureClass} — very low, not silent: pumps and machinery still emit sound.\n"
                + $"Waste heat is routed to the shelter thermal loop through the canonical coordinator.";

            bool busy = state.Mode is SofcOperatingMode.Preheating or SofcOperatingMode.Online
                or SofcOperatingMode.Derated or SofcOperatingMode.Stabilizing;
            _preheatBtn.Disabled = busy || state.Mode == SofcOperatingMode.Faulted;
            _shutdownBtn.Disabled = !busy;
            _maintainBtn.Disabled = state.Mode != SofcOperatingMode.Faulted && state.Mode != SofcOperatingMode.Online;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Solid-Oxide Fuel Cell // Quiet Baseload", minWidth: 900, minHeight: 600);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("mode", "Mode", "Offline", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("output", "Output", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("health", "Stack Health", "—", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("waste", "Waste Heat", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("PLANT STATUS"));
            _detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_detailText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("COMMISSIONING"));
            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _preheatBtn = AshfallUiHelpers.MakeButton("Start Preheat Cycle", () => { });
            _preheatBtn.Pressed += () => _host?.StartPreheat(fuelAvailable: true);
            _shutdownBtn = AshfallUiHelpers.MakeButton("Controlled Shutdown", () => { });
            _shutdownBtn.Pressed += () => _host?.Shutdown();
            _maintainBtn = AshfallUiHelpers.MakeButton("Perform Maintenance", () => { });
            _maintainBtn.Pressed += () => _host?.PerformMaintenance(partsAvailable: true);
            row.AddChild(_preheatBtn);
            row.AddChild(_shutdownBtn);
            row.AddChild(_maintainBtn);
            _contentStack.AddChild(row);

            var note = AshfallUiHelpers.MakeBody(
                "Dirty fuel scours the stack faster. Rebuilds need real ceramic components from the kiln and foundry lines. A low health reading is a maintenance warning, not a shutdown order.");
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
