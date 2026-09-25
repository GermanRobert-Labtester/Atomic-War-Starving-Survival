// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core.World;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Expansion 36 player surface. It projects canonical watch facts and
    /// forwards only real commands to the existing perimeter, roster, security,
    /// and journal owners. No readiness arithmetic is performed here.
    /// </summary>
    public partial class NightWatchPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _content = null!;
        private Label _summary = null!;
        private Label _details = null!;
        private Label _feedback = null!;
        private OptionButton _postSelect = null!;
        private OptionButton _survivorSelect = null!;
        private OptionButton _routeSelect = null!;
        private OptionButton _drillSelect = null!;
        private CheckButton _drillPassed = null!;
        private Button _assignButton = null!;
        private Button _repairButton = null!;
        private Button _postButton = null!;
        private Button _walkButton = null!;
        private Button _debriefButton = null!;
        private Button _drillButton = null!;
        private Button _sealButton = null!;
        private Button _openButton = null!;
        private Button _alarmButton = null!;
        private NightWatchHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(NightWatchHostSession host)
        {
            Unbind();
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _host.StateChanged += RefreshView;
            _host.PresentationRefreshRequested += RefreshView;
            RebuildSelectors();
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host.PresentationRefreshRequested -= RefreshView;
            }
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("The Watch // Patrol Readiness", minWidth: 1080, minHeight: 680);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("readiness", "Watch Readiness", "—", AshfallMetricCard.Criticality.Normal, minWidth: 170);
            _statusRail.AddCard("coverage", "Patrol Coverage", "—", AshfallMetricCard.Criticality.Normal, minWidth: 170);
            _statusRail.AddCard("gate", "Gate Protocol", "—", AshfallMetricCard.Criticality.Normal, minWidth: 170);
            _statusRail.AddCard("sensors", "Acoustic Array", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);

            _content = new VBoxContainer();
            _content.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _content.SizeFlagsVertical = SizeFlags.ExpandFill;

            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("WATCH PICTURE"));
            _summary = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _content.AddChild(_summary);
            _feedback = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _feedback.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _content.AddChild(_feedback);

            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("WATCH POSTS AND SHIFTS"));
            var postRow = new HBoxContainer();
            _postSelect = MakeSelector(360, "Select post");
            _survivorSelect = MakeSelector(300, "Select survivor");
            _assignButton = AshfallUiHelpers.MakeButton("ASSIGN 4H SHIFT", AssignSelectedShift);
            _repairButton = AshfallUiHelpers.MakeButton("REPAIR POST", RepairSelectedPost);
            _postButton = AshfallUiHelpers.MakeButton("TOGGLE POST", ToggleSelectedPost);
            postRow.AddChild(_postSelect);
            postRow.AddChild(_survivorSelect);
            postRow.AddChild(_assignButton);
            postRow.AddChild(_repairButton);
            postRow.AddChild(_postButton);
            _content.AddChild(postRow);

            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("PATROLS AND DRILLS"));
            var routeRow = new HBoxContainer();
            _routeSelect = MakeSelector(360, "Select route");
            _drillSelect = MakeSelector(360, "Select drill");
            _drillPassed = new CheckButton { Text = "DRILL PASSED", ButtonPressed = true };
            _walkButton = AshfallUiHelpers.MakeButton("WALK ROUTE", WalkSelectedRoute);
            _debriefButton = AshfallUiHelpers.MakeButton("RECORD DEBRIEF", RecordSelectedDebrief);
            _drillButton = AshfallUiHelpers.MakeButton("RUN DRILL", RunSelectedDrill);
            routeRow.AddChild(_routeSelect);
            routeRow.AddChild(_walkButton);
            routeRow.AddChild(_debriefButton);
            routeRow.AddChild(_drillSelect);
            routeRow.AddChild(_drillPassed);
            routeRow.AddChild(_drillButton);
            _content.AddChild(routeRow);

            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("GATE PROTOCOL"));
            var gateRow = new HBoxContainer();
            _sealButton = AshfallUiHelpers.MakeButton("SEAL GATE", SealGate);
            _openButton = AshfallUiHelpers.MakeButton("LIFT LOCKDOWN", OpenGate);
            _alarmButton = AshfallUiHelpers.MakeButton("TOGGLE GATE ALARM", ToggleGateAlarm);
            gateRow.AddChild(_sealButton);
            gateRow.AddChild(_openButton);
            gateRow.AddChild(_alarmButton);
            _content.AddChild(gateRow);

            _content.AddChild(AshfallUiHelpers.MakeSeparator());
            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("LOG AND DEBRIEF RECORD"));
            _details = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _content.AddChild(_details);

            var note = AshfallUiHelpers.MakeBody(
                "The watch reports bearings, confidence, and required verification. It does not declare an enemy. " +
                "A contact is handed to the existing acoustic, perimeter, combat, medical, or shelter-security owner only after verification.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _content.AddChild(note);

            var contentScroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            contentScroll.AddChild(_content);
            _shell.SetContent(contentScroll);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });
            RefreshView();
        }

        public override void _ExitTree() => Unbind();

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;
            RebuildSelectors();
            var snapshots = _host.EvaluateAllSectors();
            var gate = snapshots.FirstOrDefault(x => x.SectorId == "gate");
            if (gate.SectorId == null && snapshots.Count > 0) gate = snapshots[0];
            int sensors = gate.SoundSensorCount;
            int confidence = gate.AcousticConfidencePermille;
            _statusRail.Set("readiness", $"{gate.Band} {gate.OverallReadinessPermille}‰",
                gate.OverallReadinessPermille >= 700 ? AshfallMetricCard.Criticality.Normal :
                gate.OverallReadinessPermille >= 450 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("coverage", $"{gate.PatrolCoverage.CoverageGrade} · {gate.PatrolCoverage.UncoveredGapHours}h gaps",
                gate.PatrolCoverage.MeetsReadinessThreshold ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("gate", gate.GateReadiness.IsGateReady ? "READY" : gate.GateReadiness.PrimaryBottleneck,
                gate.GateReadiness.IsGateReady ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("sensors", sensors == 0 ? "offline" : $"{sensors} nodes · {confidence}‰ contact",
                confidence > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            var lines = new StringBuilder();
            lines.AppendLine($"Day {_host.CurrentDay} · live watch projection from canonical owners");
            foreach (var snapshot in snapshots)
            {
                lines.AppendLine($"{snapshot.SectorId.ToUpperInvariant()}: {snapshot.Band}, {snapshot.OverallReadinessPermille}‰ overall; " +
                    $"{snapshot.PatrolCoverage.CoverageGrade} coverage, {snapshot.PatrolCoverage.DetectionProbabilityPermille}‰ detection; " +
                    $"gate {snapshot.GateReadiness.ReadinessPermille}‰");
            }
            lines.AppendLine();
            lines.AppendLine("POSTS");
            foreach (var post in _host.Posts)
            {
                var state = _host.Perimeter.FindWatchPost(post.post_id);
                lines.AppendLine($"  {(state?.active == true ? "ON" : "OFF")} {post.display_name} — {state?.condition_permille ?? 0}‰ condition; " +
                    $"{_host.Roster.GetWatchShiftCount(post.post_id, _host.CurrentDay)} shift(s)");
            }
            lines.AppendLine();
            lines.AppendLine("ROUTES / DRILLS");
            foreach (var route in _host.Routes)
            {
                var state = _host.Perimeter.FindWatchRoute(route.route_id);
                lines.AppendLine($"  {route.display_name}: {state?.rounds_completed ?? 0} round(s)" +
                    (state?.debrief_pending == true ? " · DEBRIEF PENDING" : string.Empty));
            }
            foreach (var drill in _host.Drills)
            {
                var state = _host.Perimeter.FindWatchDrill(drill.drill_id);
                lines.AppendLine($"  {drill.display_name}: {state?.passes ?? 0} pass / {state?.failures ?? 0} fail; readiness {state?.readiness_permille ?? 0}‰");
            }
            var activeRules = _host.GateRules.Where(x => x.active_by_default).ToList();
            lines.AppendLine();
            lines.AppendLine($"GATE PROTOCOL: {activeRules.Count} active rules · {string.Join(", ", activeRules.Take(3).Select(x => x.display_name))}");
            lines.AppendLine($"ALARM CHAIN: {string.Join(" → ", _host.AlarmProtocols.OrderBy(x => x.trigger_confidence_permille).Select(x => x.display_name))}");
            lines.AppendLine($"DETECTION: {_host.DetectionProfiles.Count} authored profiles; contacts remain owned by Sound Ranging.");
            _summary.Text = lines.ToString().TrimEnd();

            var record = new StringBuilder();
            var incidents = _host.GetNarrativeIncidents();
            if (incidents.Count == 0) record.AppendLine("No authored watch log entries are loaded.");
            else
            {
                foreach (var incident in incidents.OrderByDescending(x => x.recorded_day).Take(6))
                    record.AppendLine($"Day {incident.recorded_day} · {incident.title} — {incident.log_entry}");
            }
            _details.Text = record.ToString().TrimEnd();
            _feedback.Text = _host.LastEvent;

            var postId = SelectedId(_postSelect);
            var routeId = SelectedId(_routeSelect);
            _assignButton.Disabled = string.IsNullOrWhiteSpace(postId) || _survivorSelect.ItemCount == 0;
            _repairButton.Disabled = string.IsNullOrWhiteSpace(postId) || _host.Perimeter.FindWatchPost(postId)?.condition_permille >= 1000;
            _postButton.Disabled = string.IsNullOrWhiteSpace(postId);
            _walkButton.Disabled = string.IsNullOrWhiteSpace(routeId);
            _debriefButton.Disabled = string.IsNullOrWhiteSpace(routeId) || _host.Perimeter.FindWatchRoute(routeId)?.debrief_pending != true;
            _drillButton.Disabled = _drillSelect.ItemCount == 0;
            _sealButton.Disabled = _host.Security.IsInLockdown;
            _openButton.Disabled = !_host.Security.IsInLockdown;
            _alarmButton.Disabled = _host.Catalog == null;
        }

        private OptionButton MakeSelector(int width, string placeholder)
        {
            var selector = new OptionButton { CustomMinimumSize = new Vector2(width, 34) };
            selector.AddItem(placeholder);
            selector.SetItemMetadata(0, string.Empty);
            selector.ItemSelected += _ => RefreshView();
            return selector;
        }

        private void RebuildSelectors()
        {
            if (_host == null || _postSelect == null) return;
            RebuildOptions(_postSelect, _host.Posts.Select(x => x.post_id), id => _host.Catalog?.Post(id)?.display_name ?? id);
            RebuildOptions(_survivorSelect, _host.GetEligibleSurvivorIds(), id => id);
            RebuildOptions(_routeSelect, _host.Routes.Select(x => x.route_id), id => _host.Catalog?.Route(id)?.display_name ?? id);
            RebuildOptions(_drillSelect, _host.Drills.Select(x => x.drill_id), id => _host.Catalog?.Drill(id)?.display_name ?? id);
        }

        private static void RebuildOptions(OptionButton selector, IEnumerable<string> ids, Func<string, string> label)
        {
            string previous = SelectedId(selector);
            selector.Clear();
            selector.AddItem("Select");
            selector.SetItemMetadata(0, string.Empty);
            foreach (var id in ids)
            {
                selector.AddItem(label(id) + " (" + id + ")");
                selector.SetItemMetadata(selector.ItemCount - 1, id);
            }
            if (!string.IsNullOrWhiteSpace(previous))
            {
                for (int i = 0; i < selector.ItemCount; i++)
                {
                    if (selector.GetItemMetadata(i).AsString() == previous) { selector.Select(i); break; }
                }
            }
        }

        private static string SelectedId(OptionButton selector) =>
            selector == null || selector.Selected < 0 ? string.Empty : selector.GetItemMetadata(selector.Selected).AsString();

        private void AssignSelectedShift()
        {
            if (_host == null) return;
            string post = SelectedId(_postSelect);
            string survivor = SelectedId(_survivorSelect);
            if (string.IsNullOrWhiteSpace(post) || string.IsNullOrWhiteSpace(survivor)) return;
            ShowResult(_host.AssignWatchShift(post, survivor, 18, 4));
        }

        private void RepairSelectedPost()
        {
            if (_host == null) return;
            string post = SelectedId(_postSelect);
            if (!string.IsNullOrWhiteSpace(post)) ShowResult(_host.RepairPost(post, _host.CurrentDay));
        }

        private void ToggleSelectedPost()
        {
            if (_host == null) return;
            string post = SelectedId(_postSelect);
            var state = _host.Perimeter.FindWatchPost(post);
            if (!string.IsNullOrWhiteSpace(post) && state != null) ShowResult(_host.SetPostActive(post, !state.active));
        }

        private void WalkSelectedRoute()
        {
            if (_host == null) return;
            string route = SelectedId(_routeSelect);
            if (!string.IsNullOrWhiteSpace(route)) ShowResult(_host.WalkRoute(route, _host.CurrentDay, debriefed: false));
        }

        private void RecordSelectedDebrief()
        {
            if (_host == null) return;
            string route = SelectedId(_routeSelect);
            if (!string.IsNullOrWhiteSpace(route)) ShowResult(_host.RecordDebrief(route, _host.CurrentDay));
        }

        private void RunSelectedDrill()
        {
            if (_host == null) return;
            string drill = SelectedId(_drillSelect);
            if (!string.IsNullOrWhiteSpace(drill))
                ShowResult(_host.RunDrill(drill, _host.CurrentDay, passed: _drillPassed.ButtonPressed));
        }

        private void SealGate()
        {
            if (_host != null) ShowResult(_host.SealGate(_host.CurrentDay));
        }

        private void OpenGate()
        {
            if (_host != null) ShowResult(_host.OpenGate(_host.CurrentDay));
        }

        private void ToggleGateAlarm()
        {
            if (_host != null) ShowResult(_host.ToggleSectorAlarm("gate"));
        }

        private void ShowResult(Ashfall.Core.ActionResult result)
        {
            _feedback.Text = result.IsSuccess
                ? _host?.LastEvent ?? result.MessageKey
                : $"{result.FailureCode}: {result.MessageKey}";
            RefreshView();
        }
    }
}
