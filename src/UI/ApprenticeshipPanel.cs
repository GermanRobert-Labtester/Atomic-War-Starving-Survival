using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ApprenticeshipPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _startPairBtn = null!;

        private ApprenticeshipHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(ApprenticeshipHostSession session)
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

            _shell = new AshfallDashboardShell("Apprenticeship // Skill Mentorship", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active_pairs", "Active Pairings", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("graduates", "Graduations", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = AshfallUiHelpers.MakeActionBar(separation: 10);

            _startPairBtn = AshfallUiHelpers.MakeButton("Assign Metallurgy Apprentice", () => _host?.StartPair("Master_Blacksmith", "Teen_Dweller_01", "skill_foundry_casting", 100f));
            _startPairBtn.CustomMinimumSize = new Vector2(220, 36);
            buttonRow.AddChild(_startPairBtn);

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
                    _detailText.Text = "Apprenticeship host session is not bound. Mentor-apprentice skill progression records are offline.";
                }
                return;
            }

            var s = _host.System.State;
            int active = s.activePairs.FindAll(p => !p.isComplete && !p.isCancelled).Count;
            _statusRail.Set("active_pairs", active.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("graduates", s.completedSkillIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = active > 0
                    ? $"Active Apprenticeships ({active} pairs):\n"
                    : "No active apprenticeship pairs registered.\nPair veteran survivors with apprentices to transmit technical and survival skills.\n";
                foreach (var p in s.activePairs)
                {
                    text += $"  • [{p.targetSkillId}] Apprentice: {p.apprenticeId} under Mentor: {p.mentorId} — Progress: {p.progressXp:F0}/{p.targetXp:F0} XP\n";
                }
                text += $"\nLast Event: " + (string.IsNullOrEmpty(_host.LastEvent) ? "None recorded" : _host.LastEvent);
                _detailText.Text = text;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
