// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class WaterTreatmentPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private Control _contentScene = null!;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _charcoalBtn = null!;
        private Button _distillBtn = null!;
        private Button _osmosisBtn = null!;
        private Button _replaceFilterBtn = null!;
        private Label? _waterSourcesStatus;
        private Button? _deepWellAction;
        private Button? _deepWellService;
        private Button? _condenserAction;
        private Button? _condenserMembrane;
        private Button? _piezometerInstall;

        private WaterTreatmentHostSession? _host;
        private WaterSourcesHostSession? _waterSources;

        public bool IsBound => _host != null;
        public bool AreWaterSourcesBound => _waterSources != null;
        public string WaterSourcesStatusText => _waterSourcesStatus?.Text ?? string.Empty;
        public Button? DeepWellActionButton => _deepWellAction;
        public Button? CondenserActionButton => _condenserAction;
        public Button? PiezometerInstallButton => _piezometerInstall;

        public void Bind(WaterTreatmentHostSession session)
        {
            // Phase 1 audit defect fix: previously the previous subscription
            // was never unsubscribed before binding the new one. Calling Bind
            // twice with the same session would cause every StateChanged to
            // fire RefreshView twice; Bind then Unbind left a stale handler
            // attached to a disposed host. The two-line guard below restores
            // the workshop/pharma-style "-= then +=" pattern that the rest of
            // the workstation family already uses.
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
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

        public void BindWaterSources(WaterSourcesHostSession session)
        {
            if (_waterSources != null)
                _waterSources.StateChanged -= RefreshView;
            _waterSources = session ?? throw new ArgumentNullException(nameof(session));
            _waterSources.StateChanged += RefreshView;
            RefreshView();
        }

        public void UnbindWaterSources()
        {
            if (_waterSources == null) return;
            _waterSources.StateChanged -= RefreshView;
            _waterSources = null;
        }



        public override void _Ready()
        {
            // Ticket #125 follow-up: layout chrome owned by
            // res://assets/ui/panels/WaterTreatmentPanel.tscn. The AshfallDashboardShell
            // (shared P0 primitive built in C#, see AshfallDashboardShell.cs) is a
            // runtime widget that hosts the content's typed Script. SceneBinder
            // resolves the typed unique-name nodes that the content script attached.
            //
            // The script attaches the scene as a child of _shell by creating a
            // sub-scene PanelSceneLoader.Load. The shell-driven ContentRef is
            // a Control hosting the ContentStack VBoxContainer from the scene.
            var shell = new AshfallDashboardShell("Water Treatment // Filtration & Decon", minWidth: 1000, minHeight: 650);
            AddChild(shell);

            _shell = shell;
            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("clean", "Clean Potable", "0.0 L", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("raw", "Raw Intake", "0.0 L", AshfallMetricCard.Criticality.Warn, minWidth: 120);
            _statusRail.AddCard("filter_health", "Filter Integrity", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("contamination", "Flood Contam.", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("status", "Active Mode", "IDLE", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            // Load content scene and resolve typed nodes via SceneBinder.
            _contentScene = PanelSceneLoader.Load<WaterTreatmentPanelContent>("res://assets/ui/panels/WaterTreatmentPanel.tscn");

            var binder = new SceneBinder(_contentScene, typeof(WaterTreatmentPanelContent));
            binder.Require<VBoxContainer>("ContentStack");
            binder.Require<Label>("DetailText");
            binder.Require<Button>("CharcoalButton");
            binder.Require<Button>("DistillButton");
            binder.Require<Button>("OsmosisButton");
            binder.Require<Button>("ReplaceFilterButton");

            _contentStack = binder.Get<VBoxContainer>("ContentStack");
            _detailText = binder.Get<Label>("DetailText");
            _charcoalBtn = binder.Get<Button>("CharcoalButton");
            _distillBtn = binder.Get<Button>("DistillButton");
            _osmosisBtn = binder.Get<Button>("OsmosisButton");
            _replaceFilterBtn = binder.Get<Button>("ReplaceFilterButton");

            _charcoalBtn.Pressed += () => _host?.StartFiltration(TreatmentMode.CharcoalFiltration, 10f);
            _distillBtn.Pressed += () => _host?.StartFiltration(TreatmentMode.Distillation, 10f);
            _osmosisBtn.Pressed += () => _host?.StartFiltration(TreatmentMode.ReverseOsmosis, 10f);
            _replaceFilterBtn.Pressed += () => _host?.ReplaceFilter();

            BuildWaterSourcesSection();
            _shell.SetContent(_contentScene);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            RefreshWaterSourcesView();
            if (_host == null || _statusRail == null) return;

            var s = _host.System.State;
            _statusRail.Set("clean", $"{s.cleanWater:F1} L", s.cleanWater < 10f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("raw", $"{s.rawWater:F1} L", s.rawWater > 50f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("filter_health", $"{s.filterIntegrity:F0}%", s.filterIntegrity < 25f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("contamination", $"{s.incomingContaminationLevel*100f:F0}%", s.incomingContaminationLevel > 0.5f ? AshfallMetricCard.Criticality.Critical : s.incomingContaminationLevel > 0.01f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", s.isProcessing ? s.activeMode.ToString().ToUpperInvariant() : "IDLE", s.isProcessing ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string floodWarn = s.incomingContaminationLevel > 0.5f ? " ⚠ FLOOD CONTAMINATION" : s.incomingContaminationLevel > 0.01f ? " (settling)" : "";
                _detailText.Text = $"Clean Water: {s.cleanWater:F1} L | Raw: {s.rawWater:F1} L | Brackish: {s.brackishWater:F1} L | Irradiated: {s.irradiatedWater:F1} L\n" +
                                   $"Filter Integrity: {s.filterIntegrity:F0}% | Charcoal Supply: {s.charcoalSupply:F1} units | Fuel: {s.distillationFuel:F1} units{floodWarn}\n" +
                                   $"Incoming Contamination: {s.incomingContaminationLevel*100f:F0}% | Processed Total: {s.totalWaterProcessed:F1} L | Last Event: {_host.LastEvent}";
            }
        }

        private void BuildWaterSourcesSection()
        {
            var separator = new HSeparator();
            _contentStack.AddChild(separator);

            var title = new Label
            {
                Text = "WATER SOURCES // EXTRACTION & MONITORING",
                FocusMode = Control.FocusModeEnum.None
            };
            _contentStack.AddChild(title);

            _waterSourcesStatus = new Label
            {
                Text = "WATER SOURCES: SESSION UNAVAILABLE",
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                FocusMode = Control.FocusModeEnum.None
            };
            _contentStack.AddChild(_waterSourcesStatus);

            var wellRow = new HBoxContainer();
            _deepWellAction = CreateSourceButton("BUILD DEEP WELL");
            _deepWellAction.Pressed += () =>
            {
                if (_waterSources?.DeepWellState.built == true)
                    _waterSources.TrySetDeepWellEnabled(!_waterSources.DeepWellState.enabled);
                else
                    _waterSources?.TryBuildDeepWell();
                RefreshWaterSourcesView();
            };
            _deepWellService = CreateSourceButton("SERVICE DEEP WELL");
            _deepWellService.Pressed += () =>
            {
                _waterSources?.TryServiceDeepWell();
                RefreshWaterSourcesView();
            };
            wellRow.AddChild(_deepWellAction);
            wellRow.AddChild(_deepWellService);
            _contentStack.AddChild(wellRow);

            var condenserRow = new HBoxContainer();
            _condenserAction = CreateSourceButton("BUILD CONDENSER");
            _condenserAction.Pressed += () =>
            {
                if (_waterSources?.CondenserState.built == true)
                    _waterSources.TrySetCondenserEnabled(!_waterSources.CondenserState.enabled);
                else
                    _waterSources?.TryBuildCondenser();
                RefreshWaterSourcesView();
            };
            _condenserMembrane = CreateSourceButton("REPLACE MEMBRANE");
            _condenserMembrane.Pressed += () =>
            {
                _waterSources?.TryReplaceCondenserMembrane();
                RefreshWaterSourcesView();
            };
            condenserRow.AddChild(_condenserAction);
            condenserRow.AddChild(_condenserMembrane);
            _contentStack.AddChild(condenserRow);

            _piezometerInstall = CreateSourceButton("INSTALL MONITORING NETWORK");
            _piezometerInstall.Pressed += () =>
            {
                _waterSources?.TryConstructPiezometer();
                RefreshWaterSourcesView();
            };
            _contentStack.AddChild(_piezometerInstall);
        }

        private static Button CreateSourceButton(string label) => new()
        {
            Text = label,
            FocusMode = Control.FocusModeEnum.All,
            CustomMinimumSize = new Vector2(240, 36),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };

        private void RefreshWaterSourcesView()
        {
            if (_waterSources == null || _waterSourcesStatus == null) return;
            var well = _waterSources.DeepWellState;
            var condenser = _waterSources.CondenserState;
            var monitor = _waterSources.PiezometerState;

            string wellStatus = well.built
                ? $"DEEP WELL: {(well.enabled ? "ENABLED" : "PAUSED")} | POWER {(_waterSources.DeepWellPowerServed ? "SERVED" : "UNSERVED")} | PUMP {well.condition:F0}% | TOTAL {well.totalYieldLiters} L"
                : $"DEEP WELL: NOT BUILT | CAPABILITY {(_waterSources.HasDeepWellCapability ? "READY" : "LOCKED")} | ACTUATOR {_waterSources.ItemCount(DeepWellSystem.BuildItemId)}/1 | MECHANICAL PARTS {_waterSources.ItemCount("mechanical_parts")}/2";
            string condenserStatus = condenser.built
                ? $"CONDENSER: {(condenser.enabled ? "ENABLED" : "PAUSED")} | POWER {(_waterSources.CondenserPowerServed ? "SERVED" : "UNSERVED")} | MEMBRANE {condenser.membraneIntegrity:F0}% | TOTAL {condenser.totalYieldLiters} L"
                : $"CONDENSER: NOT BUILT | CAPABILITY {(_waterSources.HasCondenserCapability ? "READY" : "LOCKED")} | MEMBRANE {_waterSources.ItemCount(AtmosphericCondenserSystem.MembraneItemId)}/1 | PIPES {_waterSources.ItemCount("metal_pipe")}/2 | SCRAP {_waterSources.ItemCount("scrap_metal")}/4";
            string monitorStatus = monitor.constructed
                ? $"AQUIFER MONITORING: ONLINE | DRAWDOWN {monitor.drawdown_state.ToUpperInvariant()} | RISK {monitor.contamination_risk:P0} | HEALTH {monitor.aquifer_health:F0}%"
                : $"AQUIFER MONITORING: NOT INSTALLED | ZONES {(_waterSources.Piezometer.System.Catalog.strata?.Count ?? 0)} | {(_waterSources.CanConstructPiezometer ? "INSTALL MATERIALS READY" : "INSTALL MATERIALS OR CATALOG REQUIRED")}";

            _waterSourcesStatus.Text = string.Join("\n", wellStatus, condenserStatus, monitorStatus,
                string.IsNullOrEmpty(_waterSources.LastEvent) ? string.Empty : "LAST ACTION: " + _waterSources.LastEvent);

            if (_deepWellAction != null)
            {
                _deepWellAction.Text = well.built
                    ? (well.enabled ? "PAUSE DEEP WELL" : "ENABLE DEEP WELL")
                    : "BUILD DEEP WELL";
                _deepWellAction.Disabled = !well.built && !_waterSources.CanBuildDeepWell;
            }
            if (_deepWellService != null)
                _deepWellService.Disabled = !_waterSources.CanServiceDeepWell;

            if (_condenserAction != null)
            {
                _condenserAction.Text = condenser.built
                    ? (condenser.enabled ? "PAUSE CONDENSER" : "ENABLE CONDENSER")
                    : "BUILD CONDENSER";
                _condenserAction.Disabled = !condenser.built && !_waterSources.CanBuildCondenser;
            }
            if (_condenserMembrane != null)
                _condenserMembrane.Disabled = !_waterSources.CanReplaceCondenserMembrane;
            if (_piezometerInstall != null)
                _piezometerInstall.Disabled = !_waterSources.CanConstructPiezometer;
        }

        public override void _ExitTree()
        {
            Unbind();
            UnbindWaterSources();
            base._ExitTree();
        }
    }
}
