using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class AirlockSecurityPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _admitBtn = null!;
        private Button _quarantineBtn = null!;
        private Button _turnAwayBtn = null!;
        private Button _cycleDoorBtn = null!;

        private AirlockSecurityHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(AirlockSecurityHostSession session)
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

            _shell = new AshfallDashboardShell("Airlock Security // Sentry & Biometrics", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("door", "Blast Door", "SECURE", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("sentry", "Posted Sentry", "NONE", AshfallMetricCard.Criticality.Warn, minWidth: 120);
            _statusRail.AddCard("visitor", "Visitor Status", "CLEAR", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("admissions", "Total Admitted", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = AshfallUiHelpers.MakeActionBar(separation: 10);

            _admitBtn = AshfallUiHelpers.MakeButton("Admit Visitor", () => _host?.ResolveIncident(VisitorDecision.Admit));
            _admitBtn.CustomMinimumSize = new Vector2(140, 36);
            buttonRow.AddChild(_admitBtn);

            _quarantineBtn = AshfallUiHelpers.MakeButton("Quarantine (3 Days)", () => _host?.ResolveIncident(VisitorDecision.Quarantine));
            _quarantineBtn.CustomMinimumSize = new Vector2(140, 36);
            buttonRow.AddChild(_quarantineBtn);

            _turnAwayBtn = AshfallUiHelpers.MakeButton("Turn Away", () => _host?.ResolveIncident(VisitorDecision.TurnAway));
            _turnAwayBtn.CustomMinimumSize = new Vector2(140, 36);
            buttonRow.AddChild(_turnAwayBtn);

            _cycleDoorBtn = AshfallUiHelpers.MakeButton("Cycle Door", () => _host?.CycleDoor(AirlockDoorState.Cycling));
            _cycleDoorBtn.CustomMinimumSize = new Vector2(120, 36);
            buttonRow.AddChild(_cycleDoorBtn);

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
            if (_host == null || _statusRail == null)
            {
                if (_detailText != null)
                {
                    _detailText.Text = "Airlock security session is not bound. Sentry post & biometric scanners are offline.";
                }
                if (_admitBtn != null) _admitBtn.Disabled = true;
                if (_quarantineBtn != null) _quarantineBtn.Disabled = true;
                if (_turnAwayBtn != null) _turnAwayBtn.Disabled = true;
                if (_cycleDoorBtn != null) _cycleDoorBtn.Disabled = true;
                return;
            }

            var s = _host.System.State;
            _statusRail.Set("door", s.doorState.ToString().ToUpperInvariant(), s.doorState == AirlockDoorState.Breached ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("sentry", string.IsNullOrEmpty(s.sentryId) ? "NONE" : s.sentryId, string.IsNullOrEmpty(s.sentryId) ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("visitor", s.hasActiveIncident ? $"{s.visitorType} ({s.visitorId})" : "NO VISITORS", s.hasActiveIncident ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("admissions", s.totalAdmissions.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text = $"Blast Door Integrity: {s.blastDoorIntegrity:F0}% | Alertness: {s.alertness:F0}%\n" +
                                   $"Pending Decision: {s.pendingDecision} | Total Turnaways: {s.totalTurnaways}\n" +
                                   $"Last Event: {_host.LastEvent}";
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
