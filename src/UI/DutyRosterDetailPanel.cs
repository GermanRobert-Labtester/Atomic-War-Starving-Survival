using System;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Duty Roster Detail panel.
    /// Presents the Core read model only: chart script, roster rows,
    /// assignments, morale marks, encounters, Second Winter, Overflow, and the
    /// Holdfast-linked snapshot. No rules are computed here.
    /// </summary>
    public partial class DutyRosterDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblAssignmentsTitle;
        private VBoxContainer _assignmentsList;
        private Label _lblShiftsTitle;
        private VBoxContainer _shiftsList;
        private Label _lblPerformanceTitle;
        private VBoxContainer _performanceList;

        private DutyRosterHostSession? _rosterHost;
        public bool IsBound => _rosterHost != null;

        // Real data from host session
        // private DutyRosterHostSession? _rosterHost;

        public void Bind(DutyRosterHostSession roster)
        {
            _rosterHost = roster;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_assignmentsList == null || _shiftsList == null || _performanceList == null) return;
            if (_rosterHost == null) return;

            AshfallUiHelpers.EmptyChildren(_assignmentsList);
            AshfallUiHelpers.EmptyChildren(_shiftsList);
            AshfallUiHelpers.EmptyChildren(_performanceList);

            var roster = _rosterHost.Roster;

            // Chart + rows.
            var chart = new Label
            {
                Text = $"CHART: {roster.ChartScript.ToUpperInvariant()} · rows {roster.OccupiedRowCount}/14 · " +
                       $"{(roster.State.wallInspected ? "inspected" : "unseen")} · " +
                       $"overflow {(roster.OverflowAccess ? "OPEN" : "sealed")} · " +
                       $"blank rows {(roster.BlankRowsAccess ? "open" : "WITHDRAWN")}"
            };
            labelify(chart);
            _assignmentsList.AddChild(chart);

            for (int i = 0; i < roster.Rows.Count; i++)
            {
                var r = roster.Rows[i];
                if (r == null) continue;
                var lbl = new Label
                {
                    Text = $"{r.displayName} — {r.status} · script {r.script} · slept d{r.lastSleptDay}"
                };
                labelify(lbl);
                _assignmentsList.AddChild(lbl);
            }

            // Assignments (ordinal role order).
            for (int i = 0; i < DutyRosterIds.AssignmentRoles.Length; i++)
            {
                string role = DutyRosterIds.AssignmentRoles[i];
                string who = roster.GetAssignment(role)!;
                if (string.IsNullOrEmpty(who)) continue;
                var lbl = new Label { Text = $"{role.Replace('_', ' ').ToUpperInvariant()}: {who}" };
                labelify(lbl, warm: true);
                _shiftsList.AddChild(lbl);
            }

            // Morale marks + later prose.
            var marks = _rosterHost.Marks;
            if (marks.Count > 0)
            {
                var m = marks.State.marks;
                for (int i = 0; i < m.Count; i++)
                {
                    var rec = m[i];
                    if (rec == null) continue;
                    var lbl = new Label
                    {
                        Text = $"[{rec.id}] {marks.GetLaterProse(rec.id)}"
                    };
                    labelify(lbl);
                    _performanceList.AddChild(lbl);
                }
            }
            else
            {
                var lbl = new Label { Text = "Marks: none yet. The wall is blank." };
                labelify(lbl);
                _performanceList.AddChild(lbl);
            }

            // Holdfast-linked snapshot + Second Winter + encounters.
            var snap = _rosterHost.SnapshotForHoldfast();
            var snapLabel = new Label
            {
                Text = $"NORTH COPY: {snap.NorthRows.Count} rows · levy {snap.LevyNames.Count} · hadi {(string.IsNullOrEmpty(snap.HadiStatus) ? "—" : snap.HadiStatus)} · " +
                       $"mutation {snap.Mutation} · visitors {_rosterHost.Encounters.ActiveVisitorQueue.Count} · " +
                       $"second winter {(roster.IsSecondWinterActive ? "ACTIVE" : "no")}"
            };
            labelify(snapLabel, warm: true);
            _performanceList.AddChild(snapLabel);
        }

        private static void labelify(Label lbl, bool warm = false)
        {
            lbl.CustomMinimumSize = new Vector2(400, 28);
            lbl.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            if (warm)
                lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("DUTY ROSTER DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Assignments section
            _lblAssignmentsTitle = AshfallUiHelpers.MakeSectionHeader("CURRENT ASSIGNMENTS");
            vbox.AddChild(_lblAssignmentsTitle);

            _assignmentsList = new VBoxContainer();
            _assignmentsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _assignmentsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_assignmentsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Shifts section
            _lblShiftsTitle = AshfallUiHelpers.MakeSectionHeader("SHIFT SCHEDULE");
            vbox.AddChild(_lblShiftsTitle);

            _shiftsList = new VBoxContainer();
            _shiftsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _shiftsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_shiftsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Performance section
            _lblPerformanceTitle = AshfallUiHelpers.MakeSectionHeader("WORKER PERFORMANCE");
            vbox.AddChild(_lblPerformanceTitle);

            _performanceList = new VBoxContainer();
            _performanceList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _performanceList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_performanceList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
