using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Duty Roster Detail panel.
    /// Shows detailed duty assignments, shift schedules, and worker performance.
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

        // Placeholder duty roster data
        private readonly string[] _placeholderAssignments = {
            "Elena — Commander (Main Hallway) — Full shift active",
            "Marcus — Medic (Medical Bay) — 8-hour shift, on duty",
            "Yuki — Scout (Perimeter Watch) — Night shift, rotating",
            "David — Engineer (Workshop) — Day shift, crafting items",
            "Sofia — Trader (Supply Route) — Day shift, negotiating"
        };

        private readonly string[] _placeholderShifts = {
            "Morning Shift (06:00-14:00) — 4 survivors assigned",
            "Afternoon Shift (14:00-22:00) — 3 survivors assigned",
            "Night Shift (22:00-06:00) — 2 survivors assigned",
            "Rest Period — 6 survivors recovering",
            "Emergency Rotation — 1 survivor on standby"
        };

        private readonly string[] _placeholderPerformance = {
            "Elena — Leadership: 95/100 — Decisive, effective",
            "Marcus — Medical: 88/100 — Skilled, reliable",
            "Yuki — Scouting: 92/100 — Alert, efficient",
            "David — Engineering: 85/100 — Creative, productive",
            "Sofia — Trading: 78/100 — Negotiating, fair"
        };

        // Real data from host session
        // private DutyRosterHostSession? _rosterHost;

        public void Bind(object roster) // placeholder for DutyRosterHostSession
        {
            // _rosterHost = (DutyRosterHostSession)roster;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_assignmentsList == null || _shiftsList == null || _performanceList == null) return;

            // Clear existing lists
            while (_assignmentsList.GetChildCount() > 0)
                _assignmentsList.RemoveChild(_assignmentsList.GetChild(0));
            while (_shiftsList.GetChildCount() > 0)
                _shiftsList.RemoveChild(_shiftsList.GetChild(0));
            while (_performanceList.GetChildCount() > 0)
                _performanceList.RemoveChild(_performanceList.GetChild(0));

            // Display placeholder assignments
            foreach (string assignment in _placeholderAssignments)
            {
                var label = new Label { Text = assignment };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _assignmentsList.AddChild(label);
            }

            // Display placeholder shifts
            foreach (string shift in _placeholderShifts)
            {
                var label = new Label { Text = shift };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _shiftsList.AddChild(label);
            }

            // Display placeholder performance
            foreach (string performance in _placeholderPerformance)
            {
                var label = new Label { Text = performance };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _performanceList.AddChild(label);
            }
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
