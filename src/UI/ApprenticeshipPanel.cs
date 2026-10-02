// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

using AtomicWar.GodotApp.Localization;
namespace AtomicWar.GodotApp.UI
{
    public partial class ApprenticeshipPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private VBoxContainer _actingRows = null!;
        private VBoxContainer _vocationalRows = null!;
        private string _vocationalSignature = string.Empty;

        private ApprenticeshipHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(ApprenticeshipHostSession session)
        {
            Unbind();
            _vocationalSignature = string.Empty;
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

            // Actionable acting-designation rows for apprentices whose
            // vocational mentor was lost. Without this the actingEligible
            // pairs were a label-only stub retained forever.
            _actingRows = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _actingRows.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _contentStack.AddChild(_actingRows);

            var vocationalScroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                CustomMinimumSize = new Vector2(0, 180)
            };
            _vocationalRows = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            vocationalScroll.AddChild(_vocationalRows);
            _contentStack.AddChild(vocationalScroll);

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
                    _detailText.Text = AshfallLocalization.Tr("ui.apprenticeship.unbound",
                        "Apprenticeship host session is not bound. Mentor-apprentice skill progression records are offline.");
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
                    if (p.actingEligible) text += "    Eligible for an acting designation; no designation assigned.\n";
                }
                text += $"\nLast Event: " + (string.IsNullOrEmpty(_host.LastEvent) ? "None recorded" : _host.LastEvent);
                _detailText.Text = text;
            }
            RefreshVocationalRows();
            RefreshActingRows();
        }

        /// <summary>
        /// Renders an ASSIGN action for every retained acting-eligible pair
        /// (vocational apprentice whose mentor died). Assigning completes the
        /// pair and removes it from the active ledger.
        /// </summary>
        private void RefreshActingRows()
        {
            if (_actingRows == null || _host == null) return;
            foreach (Node row in _actingRows.GetChildren())
            {
                _actingRows.RemoveChild(row);
                row.QueueFree();
            }

            var acting = _host.System.State.activePairs.Where(p => p.actingEligible).ToList();
            if (acting.Count == 0) return;

            _actingRows.AddChild(AshfallUiHelpers.MakeTitle("ACTING DESIGNATIONS"));
            foreach (var pair in acting)
            {
                string pairId = pair.pairId;
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var label = AshfallUiHelpers.MakeBody(
                    $"{pair.apprenticeId} — {pair.targetSkillId} (mentor {pair.mentorId} lost; assign to qualify the apprentice).");
                label.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(label);
                row.AddChild(AshfallUiHelpers.MakeButton("ASSIGN ACTING DESIGNATION", () =>
                    _host?.AssignActingDesignation(pairId)));
                _actingRows.AddChild(row);
            }
        }

        private void RefreshVocationalRows()
        {
            if (_vocationalRows == null || _host == null) return;
            var candidates = _host.VocationalCandidates();
            var mentors = (_host.MentorsProvider?.Invoke() ?? Array.Empty<string>()).ToArray();
            var definitions = _host.System.Catalog.Values.Where(d => !string.IsNullOrEmpty(d.target_skill_id))
                .OrderBy(d => d.mentorship_id, StringComparer.Ordinal).ToArray();
            string signature = string.Join("|", candidates.Select(c => c.ChildId)) + ";"
                + string.Join("|", mentors) + ";" + string.Join("|", definitions.Select(d => d.mentorship_id));
            if (_vocationalSignature == signature) return;
            _vocationalSignature = signature;
            var focused = GetViewport().GuiGetFocusOwner();
            bool restoreFocus = focused != null && _vocationalRows.IsAncestorOf(focused);
            foreach (Node row in _vocationalRows.GetChildren())
            {
                _vocationalRows.RemoveChild(row);
                row.QueueFree();
            }
            _vocationalRows.AddChild(AshfallUiHelpers.MakeTitle("VOCATIONAL MENTORSHIP"));
            if (candidates.Count == 0)
            {
                _vocationalRows.AddChild(AshfallUiHelpers.MakeBody("No adolescents with a vocational milestone are awaiting a pairing this quarter."));
                if (restoreFocus) RestoreVocationalFocus();
                return;
            }
            foreach (var child in candidates)
            {
                var row = new VBoxContainer();
                row.AddChild(AshfallUiHelpers.MakeBody($"{child.Name} — the apprentice may accept or decline."));
                var mentorChoice = new OptionButton();
                foreach (var mentor in mentors) mentorChoice.AddItem(mentor);
                var disciplineChoice = new OptionButton();
                foreach (var definition in definitions) disciplineChoice.AddItem(definition.name);
                row.AddChild(mentorChoice);
                row.AddChild(disciplineChoice);
                var buttons = new HBoxContainer();
                var accept = AshfallUiHelpers.MakeButton("ACCEPT PAIRING", () =>
                {
                    if (mentorChoice.Selected < 0 || disciplineChoice.Selected < 0) return;
                    _host?.RespondVocationalPair(child.ChildId, mentors[mentorChoice.Selected],
                        definitions[disciplineChoice.Selected].mentorship_id, true);
                });
                accept.Disabled = mentors.Length == 0 || definitions.Length == 0;
                buttons.AddChild(accept);
                buttons.AddChild(AshfallUiHelpers.MakeButton("DECLINE THIS QUARTER", () =>
                    _host?.RespondVocationalPair(child.ChildId, "", "", false)));
                row.AddChild(buttons);
                if (accept.Disabled) row.AddChild(AshfallUiHelpers.MakeBody("No mentor or discipline is currently available. Declining remains available."));
                _vocationalRows.AddChild(row);
            }
            if (restoreFocus) RestoreVocationalFocus();
        }

        private void RestoreVocationalFocus()
        {
            foreach (var node in _vocationalRows.FindChildren("*", "Button", true, false))
                if (node is Button button && !button.Disabled) { button.GrabFocus(); return; }
            foreach (var node in _shell.FindChildren("*", "Button", true, false))
                if (node is Button button && !button.Disabled) { button.GrabFocus(); return; }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
