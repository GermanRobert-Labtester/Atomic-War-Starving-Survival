// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core.Visitors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 214 — Visitor Integration &amp; Temporary Housing surface.
    /// Read-only projection over the visitor stay owner plus bounded actions
    /// (housing, processing, recruitment handoff, departure). Physical
    /// inventory, rations, and permanent roster state stay with their owners.
    /// </summary>
    public partial class VisitorIntegrationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _visitorsContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _tasksContainer = null!;
        private VBoxContainer _departuresContainer = null!;
        private Label _emptyLabel = null!;

        private VisitorIntegrationHostSession? _host;
        private int _admitCursor;

        public bool IsBound => _host != null;

        public void Bind(VisitorIntegrationHostSession session)
        {
            _host = session;
            if (_host != null) _host.StateChanged += RefreshView;
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

            _shell = new AshfallDashboardShell("Visitor Integration // Temporary Residency & Processing", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active", "Active Visitors", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("processing", "Awaiting Requirements", "0", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("departures", "Departures Logged", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("monitored", "Monitored", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // ── Left column: active visitors ────────────────────────────
            _leftColumn = new VBoxContainer();
            _leftColumn.AddThemeConstantOverride("separation", 10);
            _leftColumn.CustomMinimumSize = new Vector2(470, 400);
            _leftColumn.SizeFlagsHorizontal = SizeFlags.Fill;
            _leftColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var visitorsHeader = new Label { Text = "ACTIVE VISITOR STAYS" };
            _leftColumn.AddChild(visitorsHeader);

            var admitButton = new Button { Text = "ADMIT NEXT ARRIVAL (LOCAL)" };
            admitButton.Pressed += OnAdmitPressed;
            _leftColumn.AddChild(admitButton);

            var visitorsScroll = new ScrollContainer();
            visitorsScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            visitorsScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _visitorsContainer = new VBoxContainer();
            _visitorsContainer.AddThemeConstantOverride("separation", 8);
            _visitorsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            visitorsScroll.AddChild(_visitorsContainer);
            _leftColumn.AddChild(visitorsScroll);

            _emptyLabel = new Label { Text = "No visitors are currently housed. Admitted airlock guests appear here." };
            _leftColumn.AddChild(_emptyLabel);

            _splitBody.AddChild(_leftColumn);

            // ── Right column: requirements + departures ─────────────────
            _rightColumn = new VBoxContainer();
            _rightColumn.AddThemeConstantOverride("separation", 10);
            _rightColumn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _rightColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var taskHeader = new Label { Text = "PENDING PROCESSING REQUIREMENTS" };
            _rightColumn.AddChild(taskHeader);
            _tasksContainer = new VBoxContainer();
            _tasksContainer.AddThemeConstantOverride("separation", 6);
            _tasksContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _rightColumn.AddChild(_tasksContainer);

            var departureHeader = new Label { Text = "DEPARTURE & RECRUITMENT RECEIPTS" };
            _rightColumn.AddChild(departureHeader);

            var departureScroll = new ScrollContainer();
            departureScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            departureScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _departuresContainer = new VBoxContainer();
            _departuresContainer.AddThemeConstantOverride("separation", 6);
            _departuresContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            departureScroll.AddChild(_departuresContainer);
            _rightColumn.AddChild(departureScroll);

            _splitBody.AddChild(_rightColumn);

            _contentStack.AddChild(_splitBody);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        private void OnAdmitPressed()
        {
            if (_host == null) return;
            var templates = _host.Templates.ToList();
            if (templates.Count == 0) return;
            var template = templates[_admitCursor % templates.Count];
            _admitCursor++;
            int day = _host.Visitors.Count > 0 ? _host.Visitors.Max(v => v.ArrivalDay) : 1;
            _host.AdmitFromTemplate(template.Id, template.Name, "player", day);
            RefreshView();
        }

        private void OnRecruitPressed(VisitorRecord visitor)
        {
            if (_host == null) return;
            _host.Recruit(visitor.VisitorId, visitor.ArrivalDay + 1);
            RefreshView();
        }

        private void OnDepartPressed(VisitorRecord visitor)
        {
            if (_host == null) return;
            _host.Depart(visitor.VisitorId, DepartureType.Voluntary, "Player-approved departure", visitor.ArrivalDay + 1);
            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || !IsInsideTree()) return;

            var active = _host.GetActiveVisitors();
            int pending = _host.Tasks.Count(t => !t.IsCompleted);
            int monitored = active.Count(v => v.Monitoring != MonitoringLevel.None);

            _statusRail?.Set("active", active.Count.ToString(),
                active.Count > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("processing", pending.ToString(),
                pending > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("departures", _host.Departures.Count.ToString());
            _statusRail?.Set("monitored", monitored.ToString(),
                monitored > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            _emptyLabel.Visible = active.Count == 0;

            foreach (Node child in _visitorsContainer.GetChildren()) child.QueueFree();
            foreach (Node child in _tasksContainer.GetChildren()) child.QueueFree();
            foreach (Node child in _departuresContainer.GetChildren()) child.QueueFree();

            foreach (var visitor in active)
            {
                var card = new PanelContainer();
                var box = new VBoxContainer();
                box.AddThemeConstantOverride("separation", 3);

                box.AddChild(new Label { Text = $"{visitor.Name} — {visitor.Type} ({visitor.VisitorId})" });
                box.AddChild(new Label
                {
                    Text = $"Lifecycle: {visitor.Status} | Housing: {visitor.Housing} @ {(string.IsNullOrEmpty(visitor.AssignedRoomId) ? "unassigned" : visitor.AssignedRoomId)}"
                });
                string departure = visitor.DepartureDay > 0 ? $"planned day {visitor.DepartureDay}" : "open-ended";
                box.AddChild(new Label
                {
                    Text = $"Arrived day {visitor.ArrivalDay} | {departure} | rations {visitor.DailyFoodRate:0.0}/{visitor.DailyWaterRate:0.0}"
                });
                box.AddChild(new Label
                {
                    Text = $"Admitted by {visitor.AdmittedBy} | monitoring {visitor.Monitoring} | administrative readiness {visitor.IntegrationProgress:0}%"
                });

                var actions = new HBoxContainer();
                actions.AddThemeConstantOverride("separation", 6);

                var bunk = new Button { Text = "Berth" };
                bunk.Pressed += () => { _host.AssignHousing(visitor.VisitorId, "visitor_berth_" + visitor.VisitorId, HousingType.TemporaryBunk, visitor.ArrivalDay); RefreshView(); };
                actions.AddChild(bunk);

                var shared = new Button { Text = "Shared Quarter" };
                shared.Pressed += () => { _host.AssignHousing(visitor.VisitorId, "room_shared_bunks_b", HousingType.SharedQuarter, visitor.ArrivalDay); RefreshView(); };
                actions.AddChild(shared);

                var privateRoom = new Button { Text = "Private Room" };
                privateRoom.Pressed += () => { _host.AssignHousing(visitor.VisitorId, "room_visitor_private", HousingType.PrivateRoom, visitor.ArrivalDay); RefreshView(); };
                actions.AddChild(privateRoom);

                var guestSuite = new Button { Text = "Guest Suite" };
                guestSuite.Pressed += () => { _host.AssignHousing(visitor.VisitorId, "room_guest_suite", HousingType.GuestSuite, visitor.ArrivalDay); RefreshView(); };
                actions.AddChild(guestSuite);

                box.AddChild(actions);

                var decisionRow = new HBoxContainer();
                decisionRow.AddThemeConstantOverride("separation", 6);

                if (visitor.Status == VisitorStatus.Integrated)
                {
                    var recruit = new Button { Text = "RECRUIT AS RESIDENT" };
                    var captured = visitor;
                    recruit.Pressed += () => OnRecruitPressed(captured);
                    decisionRow.AddChild(recruit);
                }

                var depart = new Button { Text = "LOG DEPARTURE" };
                var departCaptured = visitor;
                depart.Pressed += () => OnDepartPressed(departCaptured);
                decisionRow.AddChild(depart);

                box.AddChild(decisionRow);

                card.AddChild(box);
                _visitorsContainer.AddChild(card);
            }

            var allPending = _host.Tasks.Where(t => !t.IsCompleted).ToList();
            if (allPending.Count == 0)
            {
                _tasksContainer.AddChild(new Label { Text = "No outstanding processing requirements." });
            }
            else
            {
                foreach (var task in allPending)
                {
                    var row = new HBoxContainer();
                    row.AddThemeConstantOverride("separation", 8);
                    var visitor = _host.Visitors.FirstOrDefault(v => v.VisitorId == task.VisitorId);
                    row.AddChild(new Label { Text = $"{visitor?.Name ?? task.VisitorId}: {task.TaskType} (due day {task.DueDay})" });
                    var done = new Button { Text = "Satisfy" };
                    var capturedTask = task;
                    done.Pressed += () => { _host.CompleteTask(capturedTask.TaskId, capturedTask.DueDay); RefreshView(); };
                    row.AddChild(done);
                    _tasksContainer.AddChild(row);
                }
            }

            if (_host.Departures.Count == 0)
            {
                _departuresContainer.AddChild(new Label { Text = "No departures or recruitment conversions recorded." });
            }
            else
            {
                foreach (var departure in _host.Departures)
                {
                    _departuresContainer.AddChild(new Label
                    {
                        Text = $"{departure.VisitorName} — {departure.DepartureType} on day {departure.DepartureDay}: {departure.Reason} [{departure.FinalStanding}]"
                    });
                }
            }
        }
    }
}
