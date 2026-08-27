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
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Water Treatment // Filtration & Decon", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("clean", "Clean Potable", "0.0 L", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("raw", "Raw Intake", "0.0 L", AshfallMetricCard.Criticality.Warn, minWidth: 120);
            _statusRail.AddCard("filter_health", "Filter Integrity", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("contamination", "Flood Contam.", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("status", "Active Mode", "IDLE", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _detailText.Text = "Select filtration process or replace sediment filter membranes.";
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _charcoalBtn = new Button { Text = "Run Charcoal Filter (10L)", CustomMinimumSize = new Vector2(160, 36) };
            _charcoalBtn.Pressed += () => _host?.StartFiltration(TreatmentMode.CharcoalFiltration, 10f);
            buttonRow.AddChild(_charcoalBtn);

            _distillBtn = new Button { Text = "Run Distillation (10L)", CustomMinimumSize = new Vector2(160, 36) };
            _distillBtn.Pressed += () => _host?.StartFiltration(TreatmentMode.Distillation, 10f);
            buttonRow.AddChild(_distillBtn);

            _osmosisBtn = new Button { Text = "Run Reverse Osmosis (10L)", CustomMinimumSize = new Vector2(160, 36) };
            _osmosisBtn.Pressed += () => _host?.StartFiltration(TreatmentMode.ReverseOsmosis, 10f);
            buttonRow.AddChild(_osmosisBtn);

            _replaceFilterBtn = new Button { Text = "Replace Filter", CustomMinimumSize = new Vector2(120, 36) };
            _replaceFilterBtn.Pressed += () => _host?.ReplaceFilter();
            buttonRow.AddChild(_replaceFilterBtn);

            _contentStack.AddChild(buttonRow);
            _shell.SetContent(_contentStack);

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
