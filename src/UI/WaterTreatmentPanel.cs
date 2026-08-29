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

        private WaterTreatmentHostSession? _host;

        public bool IsBound => _host != null;

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

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
