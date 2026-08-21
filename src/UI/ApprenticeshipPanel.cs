using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ApprenticeshipPanel : Control
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

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _startPairBtn = new Button { Text = "Assign Metallurgy Apprentice", CustomMinimumSize = new Vector2(220, 36) };
            _startPairBtn.Pressed += () => _host?.StartPair("Master_Blacksmith", "Teen_Dweller_01", "skill_foundry_casting", 100f);
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
            if (_host == null || _statusRail == null) return;

            var s = _host.System.State;
            int active = s.activePairs.FindAll(p => !p.isComplete && !p.isCancelled).Count;
            _statusRail.Set("active_pairs", active.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("graduates", s.completedSkillIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = $"Active Apprenticeships ({active} pairs):\n";
                foreach (var p in s.activePairs)
                {
                    text += $"  • [{p.targetSkillId}] Apprentice: {p.apprenticeId} under Mentor: {p.mentorId} — Progress: {p.progressXp:F0}/{p.targetXp:F0} XP\n";
                }
                text += $"\nLast Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }
    }
}
