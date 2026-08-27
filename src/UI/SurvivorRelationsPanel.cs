using System;
using System.Text;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class SurvivorRelationsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _mediateBtn = null!;

        private SurvivorRelationsHostSession? _host;
        private SurvivorSocialReadModel? _socialReadModel;

        public bool IsBound => _host != null;

        /// <summary>Attach the survivor-social read model so the panel can display leadership, bonds, friction, and atrophy alongside relations.</summary>
        public void SetSocialReadModel(SurvivorSocialReadModel? rm)
        {
            _socialReadModel = rm;
            RefreshView();
        }

        public void Bind(SurvivorRelationsHostSession session)
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

            _detailText = AshfallUiHelpers.MakeBody("", autowrap: true);
            _contentStack.AddChild(_detailText);

            var buttonRow = AshfallUiHelpers.MakeActionBar(separation: 10);

            _mediateBtn = AshfallUiHelpers.MakeButton("Mediate Active Conflict", () =>
            {
                if (_host != null && _host.System.State.activeConflicts.Count > 0)
                {
                    var c = _host.System.State.activeConflicts[0];
                    _host.Mediate(c.conflictId, "Leader", MediationStyle.Apology);
                }
            });
            _mediateBtn.CustomMinimumSize = new Vector2(180, 36);
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
                var sb = new StringBuilder();
                sb.Append($"Active Interpersonal Conflicts: {conflictCount}\n");
                foreach (var c in s.activeConflicts)
                {
                    if (!c.isResolved)
                        sb.Append($"  • {c.dwellerA} vs {c.dwellerB} (Cause: {c.cause}) — Day Started: {c.dayStarted}\n");
                }

                if (_socialReadModel != null)
                {
                    sb.Append("\n── Social Dynamics ──\n");
                    if (!string.IsNullOrEmpty(_socialReadModel.leaderId))
                        sb.Append($"Leader: {_socialReadModel.leaderId} (stress {_socialReadModel.leaderStress:F0})\n");
                    foreach (var e in _socialReadModel.entries)
                    {
                        sb.Append($"  • {e.survivorId}");
                        if (!string.IsNullOrEmpty(e.belief))
                            sb.Append($" [{e.belief}]");
                        if (e.bondCount > 0)
                            sb.Append($" bonds:{e.bondCount} (strongest: {e.strongestBondPartnerId} {e.strongestBondStrength:F2})");
                        if (e.resentmentLevel > 0f)
                            sb.Append($" resentment→{e.resentmentTargetId} ({e.resentmentLevel:F2})");
                        if (e.atrophiedSkills.Count > 0)
                            sb.Append($" atrophied:[{string.Join(", ", e.atrophiedSkills)}]");
                        sb.Append('\n');
                    }
                }

                sb.Append($"\nLast Event: {_host.LastEvent}");
                _detailText.Text = sb.ToString();
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
