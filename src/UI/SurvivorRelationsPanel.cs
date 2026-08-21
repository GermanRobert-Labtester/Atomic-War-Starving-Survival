using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class SurvivorRelationsPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _mediateBtn = null!;

        private SurvivorRelationsHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(SurvivorRelationsHostSession session)
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

            _shell = new AshfallDashboardShell("Survivor Relations // Social Dynamics", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("conflicts", "Active Conflicts", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("relationships", "Bonds Formed", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("mediations", "Mediations", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _mediateBtn = new Button { Text = "Mediate Active Conflict", CustomMinimumSize = new Vector2(180, 36) };
            _mediateBtn.Pressed += () =>
            {
                if (_host != null && _host.System.State.activeConflicts.Count > 0)
                {
                    var c = _host.System.State.activeConflicts[0];
                    _host.Mediate(c.conflictId, "Leader", MediationStyle.Apology);
                }
            };
            buttonRow.AddChild(_mediateBtn);

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
            int conflictCount = s.activeConflicts.FindAll(c => !c.isResolved).Count;
            _statusRail.Set("conflicts", conflictCount.ToString(), conflictCount > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("relationships", s.relationships.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("mediations", s.mediationHistory.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = $"Active Interpersonal Conflicts: {conflictCount}\n";
                foreach (var c in s.activeConflicts)
                {
                    if (!c.isResolved)
                        text += $"  • {c.dwellerA} vs {c.dwellerB} (Cause: {c.cause}) — Day Started: {c.dayStarted}\n";
                }
                text += $"\nLast Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }
    }
}
