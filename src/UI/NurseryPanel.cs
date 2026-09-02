// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Nursery & Schoolhouse (Plan 178)
// Displays child development phases, education XP, trauma load, and guardian/teacher assignments.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class NurseryPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private GenerationalSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(GenerationalSystem system)
        {
            _system = system;
            RefreshView();
        }

        public void Unbind()
        {
            _system = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Nursery // Childhood & Schoolhouse", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("children", "Active Children", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("graduates", "Adulthood Reached", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("school_xp", "Avg Education XP", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No active children registered in the shelter nursery or schoolhouse.";
            _contentStack.AddChild(_detailText);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());

            // Overlay panels start hidden; PanelRegistry drives visibility.
            Visible = false;

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public void RefreshView()
        {
            if (_statusRail == null || _detailText == null) return;
            if (_system == null)
            {
                _detailText.Text = "Generational system offline.";
                return;
            }

            var state = _system.State;
            _statusRail.Set("children", state.children.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("graduates", state.totalAdulthoodTransitions.ToString(), AshfallMetricCard.Criticality.Normal);

            if (state.children.Count == 0)
            {
                _detailText.Text = "No children under care. Construct cribs and school primers to support generational rearing.";
                return;
            }

            float totalXp = 0f;
            var summary = new System.Text.StringBuilder();
            summary.AppendLine("CHILD DEVELOPMENT & ROSTER:");
            summary.AppendLine("──────────────────────────────────────────────────");
            foreach (var c in state.children)
            {
                totalXp += c.educationXp;
                string guardian = string.IsNullOrEmpty(c.assignedGuardianId) ? "None" : c.assignedGuardianId;
                string teacher = string.IsNullOrEmpty(c.assignedTeacherId) ? "None" : $"{c.assignedTeacherId} ({c.educationFocusId})";
                summary.AppendLine($"• {c.survivorId} | Phase: {c.developmentPhase} | Progress: {c.developmentProgress:F1}% | Edu: {c.educationXp:F0} XP | Trauma: {c.traumaLoad:F1} | Guardian: {guardian} | Teacher: {teacher}");
            }

            float avgXp = state.children.Count > 0 ? totalXp / state.children.Count : 0f;
            _statusRail.Set("school_xp", $"{avgXp:F1}", AshfallMetricCard.Criticality.Normal);
            _detailText.Text = summary.ToString();
        }
    }
}
