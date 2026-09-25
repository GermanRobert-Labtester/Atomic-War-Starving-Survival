// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plans 190–193: railway terminal workflow.
    /// Presentation-only — track repair, obstacle clearing, servicing and
    /// dispatch commands are emitted via <see cref="OnActionRequested"/> and
    /// resolved by the host through the bound <see cref="RailwaySystem"/>
    /// and the canonical inventory authority.
    /// </summary>
    public partial class RailwayTerminalPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private RailwaySystem? _system;
        private ItemList _segmentList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedSegmentIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(RailwaySystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("THE LINE // RAILWAY TERMINAL", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("segments", "Segments", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
            _statusRail.AddCard("damaged", "Damaged", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
            _statusRail.AddCard("trains", "Rolling Stock", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("derailed", "Derailments", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);

            _segmentList = new ItemList
            {
                CustomMinimumSize = new Vector2(300, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _segmentList.ItemSelected += index => { _selectedSegmentIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_segmentList);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            bodyRow.AddChild(detailScroll);

            _shell.SetContent(bodyRow);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private List<TrackSegmentState> SortedSegments()
        {
            if (_system == null) return new List<TrackSegmentState>();
            return _system.State.segments.Values
                .OrderBy(s => s.segmentId, StringComparer.Ordinal)
                .ToList();
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _segmentList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var segments = SortedSegments();
            _selectedSegmentIndex = Math.Clamp(_selectedSegmentIndex, -1, Math.Max(0, segments.Count - 1));

            int damaged = segments.Count(s => s.integrity < 0.6f || !s.bridgeIntact || s.isSabotaged);
            int derailed = 0;
            foreach (var t in _system.State.trains)
                if (t != null && t.status == TrainDispatchStatus.Derailment) derailed++;

            if (_statusRail != null)
            {
                _statusRail.Set("segments", segments.Count.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("damaged", damaged.ToString(), damaged > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("trains", _system.State.trains.Count.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("derailed", derailed.ToString(), derailed > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            }

            _segmentList.Clear();
            for (int i = 0; i < segments.Count; i++)
            {
                var s = segments[i];
                string label = $"{ItemDisplay.Prettify(s.segmentId)} — {SegmentText(s)}";
                _segmentList.AddItem(label, null, false);
                if (i == _selectedSegmentIndex)
                    _segmentList.Select(i);
            }
            if (segments.Count == 0)
                _segmentList.AddItem("No track segments known", null, false);

            var selected = _selectedSegmentIndex >= 0 && _selectedSegmentIndex < segments.Count
                ? segments[_selectedSegmentIndex] : null;

            // ── Trains overview ──
            if (_system.State.trains.Count > 0)
            {
                _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ROLLING STOCK"));
                foreach (var t in _system.State.trains)
                {
                    if (t == null) continue;
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow(
                        string.IsNullOrEmpty(t.displayName) ? ItemDisplay.Prettify(t.trainId) : t.displayName,
                        $"{t.status} — fuel {t.currentFuel:0}/{t.maxFuel:0} — crew stamina {t.crewStamina * 100f:0}%",
                        t.status == TrainDispatchStatus.Derailment ? AshfallUiHelpers.ColorCritical :
                        t.status == TrainDispatchStatus.EnRoute ? AshfallUiHelpers.ColorInfo : AshfallUiHelpers.ColorText));
                }
            }

            // ── Segment detail ──
            if (selected != null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(AshfallUiHelpers.MakeSectionHeader($"SEGMENT: {ItemDisplay.Prettify(selected.segmentId).ToUpperInvariant()}"));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Track integrity", $"{selected.integrity * 100f:0}%",
                    selected.integrity < 0.4f ? AshfallUiHelpers.ColorCritical :
                    selected.integrity < 0.7f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorSuccess));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Bridge", selected.bridgeIntact ? "intact" : "COLLAPSED",
                    selected.bridgeIntact ? AshfallUiHelpers.ColorSuccess : AshfallUiHelpers.ColorCritical));
                if (selected.isSabotaged)
                    _detail.AddChild(AshfallUiHelpers.MakeWarning("Sabotage reported on this segment — clear it before dispatch."));

                if (!string.IsNullOrEmpty(_feedbackText))
                {
                    _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                    _detail.AddChild(_feedbackIsFailure
                        ? AshfallUiHelpers.MakeWarning(_feedbackText)
                        : AshfallUiHelpers.MakeSuccess(_feedbackText));
                }

                // ── Actions ──
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

                if (selected.isSabotaged)
                {
                    var clearBtn = AshfallUiHelpers.MakeButton("CLEAR SABOTAGE", () =>
                        OnActionRequested?.Invoke("clear_obstacle", selected.segmentId));
                    clearBtn.TooltipText = "Removes whatever someone left on the rails. Costs crew time and tools.";
                    row.AddChild(clearBtn);
                }
                if (!selected.bridgeIntact)
                {
                    var bridgeBtn = AshfallUiHelpers.MakeButton("REBUILD BRIDGE", () =>
                        OnActionRequested?.Invoke("repair_bridge", selected.segmentId));
                    bridgeBtn.TooltipText = "A collapsed bridge blocks the segment completely until rebuilt.";
                    row.AddChild(bridgeBtn);
                }
                if (selected.integrity < 1.0f)
                {
                    var trackBtn = AshfallUiHelpers.MakeButton("REPAIR TRACK", () =>
                        OnActionRequested?.Invoke("repair_track", selected.segmentId));
                    trackBtn.TooltipText = "Restores rail integrity. Worn track risks derailments under dispatch.";
                    row.AddChild(trackBtn);
                }

                foreach (var t in _system.State.trains)
                {
                    if (t == null) continue;
                    if (t.status == TrainDispatchStatus.Derailment)
                    {
                        var rerailBtn = AshfallUiHelpers.MakeButton($"RERAIL {ItemDisplay.Prettify(t.trainId).ToUpperInvariant()}", () =>
                            OnActionRequested?.Invoke("clear_derailment", t.trainId));
                        rerailBtn.TooltipText = "Puts derailed rolling stock back on the rail. Heavy work.";
                        row.AddChild(rerailBtn);
                    }
                    else if (t.status == TrainDispatchStatus.Idle && t.transmissionServiceRequired)
                    {
                        var svcBtn = AshfallUiHelpers.MakeButton($"SERVICE {ItemDisplay.Prettify(t.trainId).ToUpperInvariant()}", () =>
                            OnActionRequested?.Invoke("service", t.trainId));
                        svcBtn.TooltipText = "Transmission service is overdue — dispatch will be refused until it's done.";
                        row.AddChild(svcBtn);
                    }
                }

                if (row.GetChildCount() == 0)
                    row.AddChild(AshfallUiHelpers.MakeMetadata("Segment is sound. Nothing needs the crew today."));
                _detail.AddChild(row);
            }
        }

        private static string SegmentText(TrackSegmentState s)
        {
            if (s.isSabotaged) return "SABOTAGED";
            if (!s.bridgeIntact) return "BRIDGE DOWN";
            if (s.integrity < 0.4f) return $"CRITICAL {s.integrity * 100f:0}%";
            if (s.integrity < 0.7f) return $"worn {s.integrity * 100f:0}%";
            return $"sound {s.integrity * 100f:0}%";
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
