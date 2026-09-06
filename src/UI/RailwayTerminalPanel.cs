using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Transit Corridor // Armored Railway Terminal & Logistics Dashboard (Plan 73).
    /// Pure presentation: renders RailwaySystem state, track segment telemetry,
    /// and issues dispatch / repair / obstacle-clearing commands.
    /// </summary>
    public partial class RailwayTerminalPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private AshfallDataGrid? _grid;
        private Label? _detailTitle;
        private Label? _detailEndpoints;
        private Label? _detailDistance;
        private Label? _detailIntegrity;
        private Label? _detailLogistics;
        private Label? _feedbackLabel;

        private Button? _repairButton;
        private Button? _bridgeButton;
        private Button? _clearObstacleButton;
        private Button? _dispatchButton;
        private Button? _clearDerailButton;

        private RailwaySystem? _system;
        private readonly List<TrackSegmentDef> _segmentList = new();
        private int _selectedSegmentIndex = -1;

        public bool IsBound => _system != null;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
            RefreshView();
        }

        public void Bind(RailwaySystem system)
        {
            Unbind();
            _system = system;
            if (_system != null)
            {
                _system.OnTrainDispatched += HandleTrainEvent;
                _system.OnTrainArrived += HandleTrainEvent;
                _system.OnDerailment += HandleTrainEvent;
                _system.OnTrainAmbushed += HandleTrainEvent;
                _system.OnTrackRepaired += HandleTrackRepaired;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_system != null)
            {
                _system.OnTrainDispatched -= HandleTrainEvent;
                _system.OnTrainArrived -= HandleTrainEvent;
                _system.OnDerailment -= HandleTrainEvent;
                _system.OnTrainAmbushed -= HandleTrainEvent;
                _system.OnTrackRepaired -= HandleTrackRepaired;
            }
            _system = null;
        }

        private void HandleTrainEvent(string trainId, string locOrSeg) => CallDeferred(nameof(RefreshView));
        private void HandleTrackRepaired(string segId, float integrity) => CallDeferred(nameof(RefreshView));

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

        private void BuildInterface()
        {
            _shell = new AshfallDashboardShell("TRANSIT CORRIDOR // RAILWAY NETWORK & DISPATCH", minWidth: 1060, minHeight: 680);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("train_id", "TRAIN UNIT", "—", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("status", "STATUS", "Idle", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("location", "LOCATION", "Depot", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("fuel", "FUEL LEVEL", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("crew", "CREW STAMINA", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);

            var contentSplit = new HSplitContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };

            var cols = new[]
            {
                new AshfallDataGrid.Column { Header = "SEGMENT ID", MinWidth = 160 },
                new AshfallDataGrid.Column { Header = "CONNECTS", MinWidth = 180 },
                new AshfallDataGrid.Column { Header = "DIST (KM)", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
                new AshfallDataGrid.Column { Header = "INTEGRITY", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "BRIDGE", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "OBSTACLE", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Center },
                new AshfallDataGrid.Column { Header = "TRAVERSABLE", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Center }
            };

            _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 640, minHeight: 400)
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _grid.OnRowSelected += idx =>
            {
                _selectedSegmentIndex = idx;
                RefreshDetailView();
            };
            contentSplit.AddChild(_grid);

            var detailPanel = new PanelContainer
            {
                CustomMinimumSize = new Vector2(380, 400),
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            detailPanel.AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());

            var detailMargin = new MarginContainer();
            detailMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingSm);
            detailMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingSm);
            detailMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingSm);
            detailMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingSm);
            detailPanel.AddChild(detailMargin);

            var detailBox = new VBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            detailBox.AddThemeConstantOverride("separation", 10);
            detailMargin.AddChild(detailBox);

            var detailHeader = new Label { Text = "SEGMENT LOGISTICS & CONTROL" };
            detailHeader.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowSemiBold);
            detailHeader.AddThemeFontSizeOverride("font_size", 16);
            detailHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            detailBox.AddChild(detailHeader);

            _detailTitle = new Label { Text = "No segment selected" };
            _detailTitle.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowSemiBold);
            _detailTitle.AddThemeFontSizeOverride("font_size", 15);
            _detailTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            detailBox.AddChild(_detailTitle);

            _detailEndpoints = new Label { Text = "Endpoints: —" };
            _detailEndpoints.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowRegular);
            _detailEndpoints.AddThemeFontSizeOverride("font_size", 13);
            _detailEndpoints.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            detailBox.AddChild(_detailEndpoints);

            _detailDistance = new Label { Text = "Distance: —" };
            _detailDistance.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowRegular);
            _detailDistance.AddThemeFontSizeOverride("font_size", 13);
            _detailDistance.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            detailBox.AddChild(_detailDistance);

            _detailIntegrity = new Label { Text = "Integrity: —" };
            _detailIntegrity.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowRegular);
            _detailIntegrity.AddThemeFontSizeOverride("font_size", 13);
            _detailIntegrity.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            detailBox.AddChild(_detailIntegrity);

            _detailLogistics = new Label { Text = "Logistics: —" };
            _detailLogistics.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowRegular);
            _detailLogistics.AddThemeFontSizeOverride("font_size", 13);
            _detailLogistics.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            detailBox.AddChild(_detailLogistics);

            detailBox.AddChild(new HSeparator());

            var actionsHeader = new Label { Text = "COMMANDS" };
            actionsHeader.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowSemiBold);
            actionsHeader.AddThemeFontSizeOverride("font_size", 14);
            actionsHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            detailBox.AddChild(actionsHeader);

            _repairButton = new Button { Text = "[REPAIR TRACK (+25%)]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _repairButton.Pressed += OnRepairTrackPressed;
            detailBox.AddChild(_repairButton);

            _bridgeButton = new Button { Text = "[REBUILD BRIDGE]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _bridgeButton.Pressed += OnRebuildBridgePressed;
            detailBox.AddChild(_bridgeButton);

            _clearObstacleButton = new Button { Text = "[CLEAR TRACK OBSTACLE]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _clearObstacleButton.Pressed += OnClearObstaclePressed;
            detailBox.AddChild(_clearObstacleButton);

            _dispatchButton = new Button { Text = "[DISPATCH TRAIN ON SEGMENT]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _dispatchButton.Pressed += OnDispatchTrainPressed;
            detailBox.AddChild(_dispatchButton);

            _clearDerailButton = new Button { Text = "[CLEAR DERAILMENT & RE-RAIL]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _clearDerailButton.Pressed += OnClearDerailmentPressed;
            detailBox.AddChild(_clearDerailButton);

            detailBox.AddChild(new HSeparator());

            _feedbackLabel = new Label
            {
                Text = "Terminal standby.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _feedbackLabel.AddThemeFontOverride("font", AshfallUiHelpers.FontBarlowRegular);
            _feedbackLabel.AddThemeFontSizeOverride("font_size", 12);
            _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            detailBox.AddChild(_feedbackLabel);

            contentSplit.AddChild(detailPanel);
            _shell.SetContent(contentSplit);

            _shell.AttachHeaderCloseButton("[X] CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });
        }

        public void RefreshView()
        {
            if (_system == null || !IsInsideTree()) return;

            var trains = _system.State.trains;
            var activeTrain = trains.FirstOrDefault();

            if (activeTrain != null)
            {
                _statusRail?.Set("train_id", activeTrain.displayName ?? activeTrain.trainId, AshfallMetricCard.Criticality.Normal);
                var statusCrit = activeTrain.status switch
                {
                    TrainDispatchStatus.Derailment => AshfallMetricCard.Criticality.Critical,
                    TrainDispatchStatus.RobberyAmbush => AshfallMetricCard.Criticality.Warn,
                    TrainDispatchStatus.EnRoute => AshfallMetricCard.Criticality.Normal,
                    _ => AshfallMetricCard.Criticality.Normal
                };
                _statusRail?.Set("status", activeTrain.status.ToString(), statusCrit);

                string locName = activeTrain.currentNodeId;
                if (_system.Nodes.TryGetValue(activeTrain.currentNodeId, out var nDef))
                    locName = nDef.display_name;
                _statusRail?.Set("location", locName, AshfallMetricCard.Criticality.Normal);

                _statusRail?.Set("fuel", $"{activeTrain.currentFuel:F0} / {activeTrain.maxFuel:F0} L",
                    activeTrain.currentFuel < 50f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

                _statusRail?.Set("crew", $"{activeTrain.crewStamina * 100f:F0}%",
                    activeTrain.isCrewExhausted ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            }
            else
            {
                _statusRail?.Set("train_id", "No Trains", AshfallMetricCard.Criticality.Caution);
                _statusRail?.Set("status", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail?.Set("location", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail?.Set("fuel", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail?.Set("crew", "—", AshfallMetricCard.Criticality.Normal);
            }

            _segmentList.Clear();
            _segmentList.AddRange(_system.SegmentDefs.Values);

            var rows = new List<AshfallDataGrid.Row>();
            foreach (var segDef in _segmentList)
            {
                var segState = _system.EnsureSegmentState(segDef.segment_id);
                var row = new AshfallDataGrid.Row();
                row.Cells.Add(new AshfallDataGrid.Cell(segDef.display_name ?? segDef.segment_id));
                row.Cells.Add(new AshfallDataGrid.Cell($"{segDef.start_node_id} ➔ {segDef.end_node_id}"));
                row.Cells.Add(new AshfallDataGrid.Cell($"{segDef.distance_km:F0}"));

                var integState = segState.integrity >= 0.70f ? AshfallDataGrid.CellState.Positive
                    : segState.integrity >= 0.40f ? AshfallDataGrid.CellState.Caution
                    : AshfallDataGrid.CellState.Critical;
                row.Cells.Add(new AshfallDataGrid.Cell($"{segState.integrity * 100f:F0}%", integState));

                var bridgeState = (!segDef.bridge_required || segState.bridgeIntact)
                    ? (segDef.bridge_required ? new AshfallDataGrid.Cell("INTACT", AshfallDataGrid.CellState.Positive) : new AshfallDataGrid.Cell("N/A", AshfallDataGrid.CellState.Muted))
                    : new AshfallDataGrid.Cell("COLLAPSED", AshfallDataGrid.CellState.Critical);
                row.Cells.Add(bridgeState);

                var obsCell = segState.isSabotaged
                    ? new AshfallDataGrid.Cell("BLOCKED", AshfallDataGrid.CellState.Warning)
                    : new AshfallDataGrid.Cell("CLEAR", AshfallDataGrid.CellState.Positive);
                row.Cells.Add(obsCell);

                bool traversable = activeTrain != null && _system.CanTraverseSegment(activeTrain, segDef.segment_id);
                var travCell = traversable
                    ? new AshfallDataGrid.Cell("PASSABLE", AshfallDataGrid.CellState.Positive)
                    : new AshfallDataGrid.Cell("BLOCKED", AshfallDataGrid.CellState.Critical);
                row.Cells.Add(travCell);

                rows.Add(row);
            }

            _grid?.SetRows(rows);

            if (_selectedSegmentIndex < 0 && _segmentList.Count > 0)
                _selectedSegmentIndex = 0;

            RefreshDetailView();
        }

        private void RefreshDetailView()
        {
            if (_system == null) return;

            var activeTrain = _system.State.trains.FirstOrDefault();
            if (_selectedSegmentIndex < 0 || _selectedSegmentIndex >= _segmentList.Count)
            {
                if (_detailTitle != null) _detailTitle.Text = "No segment selected";
                if (_repairButton != null) _repairButton.Disabled = true;
                if (_bridgeButton != null) _bridgeButton.Disabled = true;
                if (_clearObstacleButton != null) _clearObstacleButton.Disabled = true;
                if (_dispatchButton != null) _dispatchButton.Disabled = true;
                if (_clearDerailButton != null) _clearDerailButton.Disabled = activeTrain?.status != TrainDispatchStatus.Derailment;
                return;
            }

            var segDef = _segmentList[_selectedSegmentIndex];
            var segState = _system.EnsureSegmentState(segDef.segment_id);
            var edge = _system.GetLogisticsEdge(segDef.start_node_id, segDef.end_node_id);

            if (_detailTitle != null)
                _detailTitle.Text = $"{segDef.display_name} ({segDef.segment_id})";

            if (_detailEndpoints != null)
                _detailEndpoints.Text = $"Corridor: {segDef.start_node_id} ➔ {segDef.end_node_id}";

            if (_detailDistance != null)
                _detailDistance.Text = $"Distance: {segDef.distance_km:F1} km | Max Mass: {segDef.max_train_mass:F0} t";

            if (_detailIntegrity != null)
            {
                string bridgeTxt = segDef.bridge_required ? (segState.bridgeIntact ? "Bridge intact" : "Bridge collapsed") : "No bridge";
                string sabTxt = segState.isSabotaged ? "Sabotage detected" : "No obstacles";
                _detailIntegrity.Text = $"Integrity: {segState.integrity * 100f:F0}% | {bridgeTxt} | {sabTxt}";
            }

            if (_detailLogistics != null)
            {
                if (edge != null)
                {
                    _detailLogistics.Text = $"Grade: {edge.grade * 100f:F1}% | Derail Risk: {edge.derailment_risk_bp} bp | Gauge: {edge.gauge_tag}";
                }
                else
                {
                    _detailLogistics.Text = "Standard gauge | Default grade";
                }
            }

            if (_repairButton != null)
                _repairButton.Disabled = segState.integrity >= 1.0f;

            if (_bridgeButton != null)
                _bridgeButton.Disabled = !segDef.bridge_required || segState.bridgeIntact;

            if (_clearObstacleButton != null)
                _clearObstacleButton.Disabled = !segState.isSabotaged;

            if (_dispatchButton != null)
            {
                bool canDispatch = activeTrain != null
                    && (activeTrain.status == TrainDispatchStatus.Idle || activeTrain.status == TrainDispatchStatus.Arrived)
                    && (segDef.start_node_id == activeTrain.currentNodeId || segDef.end_node_id == activeTrain.currentNodeId)
                    && _system.CanTraverseSegment(activeTrain, segDef.segment_id);
                _dispatchButton.Disabled = !canDispatch;
            }

            if (_clearDerailButton != null)
            {
                _clearDerailButton.Disabled = activeTrain?.status != TrainDispatchStatus.Derailment;
            }
        }

        private void OnRepairTrackPressed()
        {
            if (_system == null || _selectedSegmentIndex < 0 || _selectedSegmentIndex >= _segmentList.Count) return;
            var segDef = _segmentList[_selectedSegmentIndex];
            var res = _system.RepairTrack(segDef.segment_id, 0.25f);
            SetFeedback(res.IsSuccess ? $"Track repaired (+25%): {segDef.segment_id}" : $"Repair blocked: {res.FailureCode}");
            RefreshView();
        }

        private void OnRebuildBridgePressed()
        {
            if (_system == null || _selectedSegmentIndex < 0 || _selectedSegmentIndex >= _segmentList.Count) return;
            var segDef = _segmentList[_selectedSegmentIndex];
            var res = _system.RepairBridge(segDef.segment_id);
            SetFeedback(res.IsSuccess ? $"Bridge reconstructed on {segDef.segment_id}" : $"Bridge repair blocked: {res.FailureCode}");
            RefreshView();
        }

        private void OnClearObstaclePressed()
        {
            if (_system == null || _selectedSegmentIndex < 0 || _selectedSegmentIndex >= _segmentList.Count) return;
            var segDef = _segmentList[_selectedSegmentIndex];
            var res = _system.ClearTrackObstacle(segDef.segment_id);
            SetFeedback(res.IsSuccess ? $"Obstacle cleared on {segDef.segment_id}" : $"Clearing blocked: {res.FailureCode}");
            RefreshView();
        }

        private void OnDispatchTrainPressed()
        {
            if (_system == null || _selectedSegmentIndex < 0 || _selectedSegmentIndex >= _segmentList.Count) return;
            var activeTrain = _system.State.trains.FirstOrDefault();
            if (activeTrain == null) return;
            var segDef = _segmentList[_selectedSegmentIndex];
            var res = _system.DispatchTrain(activeTrain.trainId, segDef.segment_id);
            SetFeedback(res.IsSuccess ? $"Train {activeTrain.trainId} dispatched on {segDef.segment_id}" : $"Dispatch blocked: {res.FailureCode}");
            RefreshView();
        }

        private void OnClearDerailmentPressed()
        {
            if (_system == null) return;
            var activeTrain = _system.State.trains.FirstOrDefault();
            if (activeTrain == null) return;
            var res = _system.ClearDerailment(activeTrain.trainId);
            SetFeedback(res.IsSuccess ? $"Derailment cleared for {activeTrain.trainId}. Train re-railed." : $"Clear blocked: {res.FailureCode}");
            RefreshView();
        }

        private void SetFeedback(string text)
        {
            if (_feedbackLabel != null)
            {
                _feedbackLabel.Text = text;
                _feedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            }
        }
    }
}
