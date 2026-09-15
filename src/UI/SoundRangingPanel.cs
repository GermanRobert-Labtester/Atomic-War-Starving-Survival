// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Combat;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 123 Phase 9 — DEFENSIVE sound-ranging panel. Presents threat intel
    /// with honest uncertainty language (Low Confidence / Probable Sector /
    /// High Confidence Threat Zone). No fire-control coordinates exist to
    /// show — the DTO carries bearing, region radius, and confidence only.
    /// </summary>
    public partial class SoundRangingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _deployBtn = null!;
        private Button _recalibrateBtn = null!;
        private Button _nodeToggleBtn = null!;

        private SoundRangingHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(SoundRangingHostSession session)
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

        private static string ConfidenceLabel(int bp) => bp >= 7000 ? "High Confidence Threat Zone"
            : bp >= 4000 ? "Probable Sector"
            : "Low Confidence";

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;
            var engine = _host.System;
            var state = engine.State;
            var threat = engine.GetActiveThreat();
            int operational = engine.OperationalSensorCount();
            int total = state.Nodes.Count;

            _statusRail.Set("nodes", $"{operational}/{total}",
                operational < 2 ? AshfallMetricCard.Criticality.Critical
                : operational < total ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("calibration",
                state.CalibrationDriftBp >= SoundRangingThreatEngine.CalibrationDriftCapBp
                    ? "Invalid — recalibrate"
                    : $"{Math.Max(0, 100 - state.CalibrationDriftBp / 100):0}%",
                state.CalibrationDriftBp >= SoundRangingThreatEngine.CalibrationDriftCapBp
                    ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("bearing",
                threat != null ? $"{threat.BearingDeg}° ±{threat.BearingErrorDeg:0}°" : "—",
                AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("confidence",
                threat != null ? ConfidenceLabel(threat.ConfidenceBp) : "No threat intel",
                threat != null && threat.ConfidenceBp >= 7000
                    ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            _detailText.Text = threat != null
                ? $"Probable origin: {threat.BearingDeg}° bearing ±{threat.BearingErrorDeg:0}°, region ±{threat.RegionRadiusCells} map cells. "
                + $"{ConfidenceLabel(threat.ConfidenceBp)} as of day {threat.DayLastObserved}. "
                + $"Source class estimate: {threat.SourceClassEstimateId ?? "unknown"}. "
                + "Stale readings decay and expire — old intel is not safe intel.\n"
                + "This is early-warning information for evacuation, readiness, and route planning. It is not a firing solution, and no weapon cueing consumes it."
                : "No live hostile-fire estimate. The array listens passively; observations decay after five days.";

            _deployBtn.Disabled = engine.State.Nodes.Count > 0;
            _recalibrateBtn.Disabled = false;
            _nodeToggleBtn.Disabled = total == 0;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Acoustic Sound-Ranging // Siege Warning", minWidth: 950, minHeight: 620);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("nodes", "Sensor Nodes", "0/0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("calibration", "Calibration", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("bearing", "Bearing Sector", "—", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("confidence", "Intel Status", "No threat", AshfallMetricCard.Criticality.Normal, minWidth: 200);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("THREAT ESTIMATE"));
            _detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_detailText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ARRAY COMMANDS"));
            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", 10);
            _deployBtn = AshfallUiHelpers.MakeButton("Deploy Array", () => { });
            _deployBtn.Pressed += () => _host?.Deploy("sound_array_mk1", partsAvailable: true);
            _recalibrateBtn = AshfallUiHelpers.MakeButton("Recalibrate Array", () => { });
            _recalibrateBtn.Pressed += () => _host?.PerformMaintenance(partsAvailable: true);
            _nodeToggleBtn = AshfallUiHelpers.MakeButton("Damage Report: First Node", () => { });
            _nodeToggleBtn.Pressed += () =>
            {
                var state = _host?.System.State;
                var first = state?.Nodes.Find(n => n.Operational);
                if (first != null) _host?.SetNodeOperational(first.NodeId, operational: false);
            };
            row.AddChild(_deployBtn);
            row.AddChild(_recalibrateBtn);
            row.AddChild(_nodeToggleBtn);
            _contentStack.AddChild(row);

            var note = AshfallUiHelpers.MakeBody(
                "Wind and storms broaden the probable region. Repeated fire from a stationary battery narrows it; a redeployed source resets the picture. Surface nodes can be damaged, severed, or stolen — repairs restore capability.");
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
