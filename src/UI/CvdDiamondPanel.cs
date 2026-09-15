// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 124 Phase 9 — CVD diamond growth panel. Presentation only: what is
    /// growing, how stable the process is, expected grade, degraded equipment,
    /// and which consumers can use the output. No tuning controls.
    /// </summary>
    public partial class CvdDiamondPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _startBtn = null!;
        private Button _advanceBtn = null!;
        private Button _maintainBtn = null!;

        private CvdDiamondHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(CvdDiamondHostSession session)
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

            _statusRail.Set("mode", state.Mode.ToString(),
                state.Mode == CvdReactorMode.Faulted ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("magnetron", $"{state.MagnetronConditionBp / 100.0:0.0}%",
                state.MagnetronConditionBp < 3000 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("chamber", $"{state.ChamberConditionBp / 100.0:0.0}%",
                state.ChamberConditionBp < 3000 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("progress",
                state.ActiveBatch != null ? $"{state.ActiveBatch.GrowthProgressBp / 100.0:0.0}%" : "—",
                AshfallMetricCard.Criticality.Normal);

            string batchLine = state.ActiveBatch != null
                ? $"Growing: {state.ActiveBatch.ComponentId} — substrate {state.ActiveBatch.SubstrateProfileId}, feed {state.ActiveBatch.FeedProfileId}."
                : "No active batch.";
            _detailText.Text = $"{batchLine}\n"
                + $"Plasma stability: {state.PlasmaStabilityBp / 100.0:0.0}% — instability can void the batch and damage the chamber.\n"
                + $"Output grades are checked against the precision-metrology ladder: master-grade inserts need certification before release.\n"
                + $"Only registered high-wear consumers (deep-excavation cutter, precision lathe) receive the wear benefit.";

            _startBtn.Disabled = state.ActiveBatch != null || state.Mode is CvdReactorMode.Faulted or CvdReactorMode.Offline;
            _advanceBtn.Disabled = state.ActiveBatch == null;
            _maintainBtn.Disabled = false;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Synthetic Diamond Tooling // CVD Growth", minWidth: 900, minHeight: 600);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("mode", "Reactor", "Idle", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("progress", "Batch", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("magnetron", "Magnetron", "—", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("chamber", "Chamber", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("GROWTH BATCH"));
            _detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_detailText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("INDUSTRIAL COMMANDS"));
            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", 10);
            _startBtn = AshfallUiHelpers.MakeButton("Start Growth Batch", () => { });
            _startBtn.Pressed += () => _host?.StartBatch(_host.NextBatchId(), "diamond_insert_industrial", "feed_refined_methane", "substrate_superalloy_billet");
            _advanceBtn = AshfallUiHelpers.MakeButton("Advance Batch", () => { });
            _advanceBtn.Pressed += () => _host?.AdvanceBatch();
            _maintainBtn = AshfallUiHelpers.MakeButton("Maintain Chamber", () => { });
            _maintainBtn.Pressed += () => _host?.PerformMaintenance();
            row.AddChild(_startBtn);
            row.AddChild(_advanceBtn);
            row.AddChild(_maintainBtn);
            _contentStack.AddChild(row);

            var note = AshfallUiHelpers.MakeBody(
                "Higher-purity feedstock and better substrates raise conformity, but defects can still occur. Tool inserts extend service intervals of registered consumers; they never make tools wear-free.");
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
