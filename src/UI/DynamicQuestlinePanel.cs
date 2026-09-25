// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Quests;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Wave 8 B2 — Emergency Dynamic Questline board.
    ///
    /// Read-only player surface over the campaign-wide emergency quest runtime.
    /// Quests are opened, advanced, completed, and failed by real shelter and
    /// field events (workshop refurbishment, radio triangulation, excavation
    /// cave-ins); this board only reports the authoritative
    /// <see cref="DynamicQuestlineSystem"/> state. It owns no command and
    /// recomputes nothing.
    /// </summary>
    public partial class DynamicQuestlinePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _activeText = null!;
        private Label _historyText = null!;

        private DynamicQuestlineSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(DynamicQuestlineSystem system)
        {
            Unbind();
            _system = system;
            _system.OnStateChanged += HandleStateChanged;
            RefreshView();
        }

        public void Unbind()
        {
            if (_system != null)
            {
                _system.OnStateChanged -= HandleStateChanged;
                _system = null;
            }
        }

        private void HandleStateChanged() => RefreshView();

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("EMERGENCY QUESTS // DYNAMIC OPERATIONS", minWidth: 1100, minHeight: 680);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active", "Active", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("completed", "Resolved", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("failed", "Failed / Expired", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("incidents", "Incidents Seen", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("day", "Campaign Day", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            var scroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(_contentStack);

            _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE OPERATIONS"));
            _contentStack.AddChild(_activeText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("RESOLVED & FAILED"));
            _contentStack.AddChild(_historyText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            var note = AshfallUiHelpers.MakeBody(
                "Emergency operations are opened by what happens in the shelter and the field — a "
                + "collapsed gallery, a triangulated broadcast, an armory going unserviceable. This "
                + "board reports the active response; it does not start, advance, or cancel one.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _shell.SetContent(scroll);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        public void RefreshView()
        {
            if (_system == null || _statusRail == null) return;

            int active = _system.ActiveQuests.Count;
            _statusRail.Set("active", active.ToString(),
                active > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("completed", _system.CompletedIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("failed", _system.FailedIds.Count.ToString(),
                _system.FailedIds.Count > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("incidents", _system.State.triggeredIncidentIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("day", _system.State.currentDay.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text =
                    $"Active: {active} | Resolved: {_system.CompletedIds.Count} | Failed: {_system.FailedIds.Count} | "
                    + $"Tracked incidents: {_system.State.triggeredIncidentIds.Count}";
            }

            RenderActive();
            RenderHistory();
        }

        private void RenderActive()
        {
            if (_activeText == null || _system == null) return;
            if (_system.ActiveQuests.Count == 0)
            {
                _activeText.Text = "No emergency operations are active. The response board is clear.";
                return;
            }

            var lines = new List<string>();
            foreach (var q in _system.ActiveQuests)
            {
                if (q == null) continue;
                lines.Add(FormatQuest(q));
                lines.Add(string.Empty);
            }
            _activeText.Text = string.Join("\n", lines).TrimEnd();
        }

        private void RenderHistory()
        {
            if (_historyText == null || _system == null) return;
            if (_system.CompletedIds.Count == 0 && _system.FailedIds.Count == 0)
            {
                _historyText.Text = "Nothing has been resolved or lost yet.";
                return;
            }

            var lines = new List<string>();
            if (_system.CompletedIds.Count > 0)
                lines.Add("Resolved: " + string.Join(", ", _system.CompletedIds));
            if (_system.FailedIds.Count > 0)
                lines.Add("Failed / expired: " + string.Join(", ", _system.FailedIds));
            _historyText.Text = string.Join("\n", lines);
        }

        private string FormatQuest(DynamicQuestInstance q)
        {
            string stage = q.CurrentStageIndex >= 0 && q.CurrentStageIndex < q.Stages.Count
                ? q.Stages[q.CurrentStageIndex]
                : "—";
            string deadline = FormatDeadline(q);
            string targets = q.TargetSurvivorIds != null && q.TargetSurvivorIds.Count > 0
                ? $" | people at risk: {q.TargetSurvivorIds.Count}"
                : string.Empty;
            return $"{q.Title}\n"
                + $"  {q.Description}\n"
                + $"  Site: {q.TargetLocationId} | Stage {q.CurrentStageIndex + 1}/{q.Stages.Count} ({stage}) | "
                + $"Progress {q.ProgressCurrent}/{q.ProgressRequired} | Opened day {q.TriggerDay} | Deadline {deadline}{targets}";
        }

        private string FormatDeadline(DynamicQuestInstance q)
        {
            if (!q.DeadlineDay.HasValue) return "none";
            int day = _system?.State.currentDay ?? 0;
            int daysLeft = q.DeadlineDay.Value - day;
            string when = daysLeft > 0 ? $"day {q.DeadlineDay.Value} ({daysLeft}d left)"
                : daysLeft == 0 ? $"day {q.DeadlineDay.Value} (due today)"
                : $"day {q.DeadlineDay.Value} (overdue)";
            return when;
        }
    }
}