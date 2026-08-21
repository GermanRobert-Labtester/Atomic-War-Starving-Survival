using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ShelterSchedulePanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _curfewBtn = null!;
        private Button _emergencyBtn = null!;

        private ShelterScheduleHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(ShelterScheduleHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Shelter Schedule // Shift Assignment", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("phase", "Current Phase", "DAY", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("curfew", "Curfew", "INACTIVE", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("lighting", "Lighting Load", "50%", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _curfewBtn = new Button { Text = "Toggle Night Curfew", CustomMinimumSize = new Vector2(180, 36) };
            _curfewBtn.Pressed += () =>
            {
                if (_host != null)
                {
                    bool active = !_host.System.State.curfewActive;
                    _host.SetCurfew(active);
                }
            };
            buttonRow.AddChild(_curfewBtn);

            _emergencyBtn = new Button { Text = "Emergency Override", CustomMinimumSize = new Vector2(180, 36) };
            _emergencyBtn.Pressed += () =>
            {
                if (_host != null)
                {
                    bool active = !_host.System.State.emergencyOverride;
                    _host.SetEmergencyOverride(active);
                }
            };
            buttonRow.AddChild(_emergencyBtn);

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
            _statusRail.Set("phase", s.currentPhase.ToString().ToUpperInvariant(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("curfew", s.curfewActive ? "ACTIVE" : "INACTIVE", s.curfewActive ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("lighting", $"{s.lightingDemand:P0}", AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text = $"Shelter Schedule Phase: {s.currentPhase} | Fatigue Recovery Rate: {s.fatigueRecoveryModifier:P0}\n" +
                                   $"Bunk Assignments: {s.assignments.Count} dwellers assigned\n" +
                                   $"Last Event: {_host.LastEvent}";
            }
        }
    }
}
