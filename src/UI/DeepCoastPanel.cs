using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — District 8 Deep Coast panel (Exp 01 sibling layer).
    /// Presents the route beyond the Shelf: reopening stage, decision, dock
    /// condition, contamination, fleet/office access, route-node gating,
    /// seasonal availability, the next material bill, and the dock dive
    /// operation handoff. Presentation only — every rule lives in
    /// DeepCoastHostSession / District8DeepCoastSystem.
    /// </summary>
    public partial class DeepCoastPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private DeepCoastHostSession? _deepCoast;
        private CoreDemoSession? _core;
        private VBoxContainer _statusContainer = null!;
        private VBoxContainer _routeContainer = null!;
        private VBoxContainer _actionContainer = null!;
        private Label _statusLabel = null!;

        public bool IsBound => _deepCoast != null || _core != null;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public void Bind(DeepCoastHostSession? deepCoast, CoreDemoSession? core)
        {
            _deepCoast = deepCoast;
            _core = core;
            if (_deepCoast != null)
            {
                _deepCoast.StateChanged += RefreshView;
            }
            if (_core != null)
            {
                _core.IceRoad.OnStateChanged += HandleIceRoadStateChanged;
            }
            RefreshView();
        }

        private void HandleIceRoadStateChanged(Ashfall.Core.IceRoadSystemState _) => RefreshView();

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

        private void BuildLayout()
        {
            var backdrop = new ColorRect
            {
                Color = new Color(0.03f, 0.04f, 0.06f, 0.95f)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_right", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_top", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_bottom", (int)CoreTheme.SpacingLg);
            AddChild(margin);

            var mainVBox = new VBoxContainer();
            mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            margin.AddChild(mainVBox);

            var headerCard = AshfallUiHelpers.MakeCardFrame(
                "DISTRICT 8 DEEP COAST // THE ROUTE BEYOND THE SHELF",
                "The coastal perimeter, the flooded service channel, and the deep berth at the Northern Sound Icebreaker Dock. Sealed until surveyed; worked open by decision, material, and grit."
            );
            mainVBox.AddChild(headerCard);

            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            var contentBox = new VBoxContainer();
            contentBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ROUTE & DOCK STATUS"));
            _statusContainer = new VBoxContainer();
            _statusContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_statusContainer);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ROUTE NODES (TRAVEL FROM FOGHORN 8)"));

            _routeContainer = new VBoxContainer();
            _routeContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_routeContainer);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIONS"));

            _actionContainer = new VBoxContainer();
            _actionContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_actionContainer);

            var bottomBar = new HBoxContainer();
            bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            mainVBox.AddChild(bottomBar);

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
            btnClose.CustomMinimumSize = new Vector2(240, 44);
            bottomBar.AddChild(btnClose);

            _statusLabel = AshfallUiHelpers.MakeMono("The deep coast is sealed. The boom is rusted shut and the ledger is unsigned.");
            _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomBar.AddChild(_statusLabel);
        }

        public void RefreshView()
        {
            if (_statusContainer == null || _routeContainer == null || _actionContainer == null) return;

            ClearContainer(_statusContainer);
            ClearContainer(_routeContainer);
            ClearContainer(_actionContainer);

            if (_deepCoast == null)
            {
                _statusLabel.Text = "Deep coast session unavailable.";
                return;
            }

            var dc = _deepCoast.DeepCoast;
            var state = dc.State;

            // ── Status card ──
            var statusCard = AshfallUiHelpers.MakePanel();
            var statusMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            statusCard.AddChild(statusMargin);
            var sBox = new VBoxContainer();
            sBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            statusMargin.AddChild(sBox);

            string seasonal = _core != null && _core.IceRoad.IsOpen ? "ICE ROAD OPEN — ROUTE SEASONAL GATE DOWN" : "ICE ROAD CLOSED — SHELF & DEEP COAST SEASON-BLOCKED";
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Seasonal Access", seasonal, AshfallUiHelpers.ToColor(_core != null && _core.IceRoad.IsOpen ? CoreTheme.Warm : CoreTheme.Dim)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Reopening Stage", state.stage.ToString().Replace("DeepBerthOperational", "Deep Berth Operational").Replace("DockAccessible", "Dock Accessible").Replace("PerimeterOpen", "Perimeter Open"), AshfallUiHelpers.ToColor(CoreTheme.Warm)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Access Decision", state.accessDecision == 0 ? "Undecided" : state.accessDecision.ToString(), AshfallUiHelpers.ToColor(state.accessDecision == (int)DeepCoastAccessDecision.SalvageImmediate ? CoreTheme.Hot : CoreTheme.Pale)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Dock Structural Integrity", $"{state.structuralIntegrity:F0}%", AshfallUiHelpers.ToColor(state.structuralIntegrity < 60f ? CoreTheme.Critical : CoreTheme.Pale)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Brine / Industrial Contamination", state.contaminationLevel <= 0.001f ? "Clear" : $"{state.contaminationLevel:P0}", AshfallUiHelpers.ToColor(state.contaminationLevel > 0.5f ? CoreTheme.Critical : CoreTheme.Warm)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Fleet", _deepCoast.IsFleetActive ? "STOOD UP — ON THE QUAY" : "waiting on the sound", AshfallUiHelpers.ToColor(_deepCoast.IsFleetActive ? CoreTheme.Hot : CoreTheme.Dim)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Office", dc.IsOfficeAccessLimited ? "access limited — municipal control" : "auditing as usual", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Fleet Levy", dc.IsFleetLevyActive ? "25% of dock salvage" : "none", AshfallUiHelpers.ToColor(dc.IsFleetLevyActive ? CoreTheme.Hot : CoreTheme.Pale)));
            sBox.AddChild(AshfallUiHelpers.MakeDataRow("Dock Operation", dc.IsDockOperationActive ? $"ACTIVE — {dc.ActiveDockOperationDiverId}" : "idle", AshfallUiHelpers.ToColor(dc.IsDockOperationActive ? CoreTheme.Hot : CoreTheme.Dim)));
            _statusContainer.AddChild(statusCard);

            // ── Route nodes ──
            for (int i = 0; i < dc.Route.Count; i++)
            {
                var node = dc.Route[i];
                if (node == null) continue;
                bool unlocked = dc.IsNodeAccessible(node.id);
                string status = unlocked ? "REACHABLE" : (node.id == District8DeepCoastSystem.DockId ? "SEALED" : "LOCKED");
                var color = unlocked ? CoreTheme.Warm : CoreTheme.Dim;
                var row = AshfallUiHelpers.MakeDataRow(
                    $"{node.displayName} [{node.id}]",
                    $"{status} · {node.travelHours:F1}h · danger {node.dangerLevel:F0} · rad +{dc.RadsPerHour(node.id):F0} mSv/h",
                    AshfallUiHelpers.ToColor(color));
                _routeContainer.AddChild(row);
            }

            // ── Actions ──
            _actionContainer.AddChild(AshfallUiHelpers.MakeDataRow("Required Materials (next step)", BillText(dc.NextStepBill()), AshfallUiHelpers.ToColor(dc.NextStepBill().Count > 0 ? CoreTheme.Pale : CoreTheme.Dim)));

            var actions = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
            actions.AddThemeConstantOverride("h_separation", (int)CoreTheme.SpacingSm);
            _actionContainer.AddChild(actions);

            // Survey.
            if (!state.perimeterSurveyed)
            {
                var btnSurvey = AshfallUiHelpers.MakeButton("SURVEY THE BREAKWATER", () =>
                {
                    _statusLabel.Text = _deepCoast.Survey(Day());
                    RefreshView();
                });
                btnSurvey.CustomMinimumSize = new Vector2(220, 36);
                actions.AddChild(btnSurvey);
            }

            // Decision (surveyed, undecided).
            if (state.perimeterSurveyed && state.accessDecision == (int)DeepCoastAccessDecision.None)
            {
                var dBox = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
                dBox.AddThemeConstantOverride("h_separation", (int)CoreTheme.SpacingSm);
                _actionContainer.AddChild(dBox);
                dBox.AddChild(AshfallUiHelpers.MakeButton("DECISION: STABILIZE & REPAIR", () => Decide("stabilize")));
                dBox.AddChild(AshfallUiHelpers.MakeButton("DECISION: SALVAGE IMMEDIATE (RISKY)", () => Decide("salvage")));
                dBox.AddChild(AshfallUiHelpers.MakeButton("DECISION: GRANT THE FLEET (LEVY)", () => Decide("fleet")));
                dBox.AddChild(AshfallUiHelpers.MakeButton("DECISION: MUNICIPAL CONTROL", () => Decide("municipal")));
            }

            // Perimeter clear.
            if (!state.perimeterCleared && state.accessDecision != (int)DeepCoastAccessDecision.None)
            {
                var btn = AshfallUiHelpers.MakeButton("CLEAR THE PERIMETER BOOM", () =>
                {
                    _statusLabel.Text = _deepCoast.ClearPerimeter(Day());
                    RefreshView();
                });
                btn.CustomMinimumSize = new Vector2(240, 36);
                actions.AddChild(btn);
            }

            // Channel clear.
            if (state.perimeterCleared && !state.channelCleared)
            {
                var btn = AshfallUiHelpers.MakeButton("CLEAR THE SERVICE CHANNEL", () =>
                {
                    _statusLabel.Text = _deepCoast.ClearChannel(Day());
                    RefreshView();
                });
                btn.CustomMinimumSize = new Vector2(240, 36);
                actions.AddChild(btn);
            }

            // Berth repair.
            if (state.channelCleared && !state.berthRepaired)
            {
                var btn = AshfallUiHelpers.MakeButton("REPAIR DEEP BERTH 9", () =>
                {
                    _statusLabel.Text = _deepCoast.RepairBerth(Day());
                    RefreshView();
                });
                btn.CustomMinimumSize = new Vector2(220, 36);
                actions.AddChild(btn);
            }

            // Dock dive operation.
            if (dc.CanStartDockOperation)
            {
                var diveBox = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
                diveBox.AddThemeConstantOverride("h_separation", (int)CoreTheme.SpacingSm);
                _actionContainer.AddChild(diveBox);
                var btnStart = AshfallUiHelpers.MakeButton("LAUNCH DOCK DIVE (S. TANAKA / M. OLEJNIK)", () =>
                {
                    _statusLabel.Text = _deepCoast.StartDockDive("suki_tanaka", "marcus_olejnik", Day());
                    RefreshView();
                });
                btnStart.CustomMinimumSize = new Vector2(340, 36);
                diveBox.AddChild(btnStart);
            }
            else if (dc.IsDockOperationActive && _deepCoast.Maritime.Dive.IsActive)
            {
                var diveBox = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
                diveBox.AddThemeConstantOverride("h_separation", (int)CoreTheme.SpacingSm);
                _actionContainer.AddChild(diveBox);
                diveBox.AddChild(AshfallUiHelpers.MakeButton("CRANK COMPRESSOR (+30s)", () => { _statusLabel.Text = _deepCoast.CrankDockDive(); RefreshView(); }));
                diveBox.AddChild(AshfallUiHelpers.MakeButton("ADVANCE ROOM (+10 NOISE)", () => { _statusLabel.Text = _deepCoast.AdvanceDockDive(10); RefreshView(); }));
                diveBox.AddChild(AshfallUiHelpers.MakeButton("SIMULATE DIVE (+30s)", () => { _statusLabel.Text = _deepCoast.TickDockDive(30f); RefreshView(); }));
                diveBox.AddChild(AshfallUiHelpers.MakeButton("SURFACE — RECOVER SALVAGE", () => { _statusLabel.Text = _deepCoast.CompleteDockDive(true, null!, Day()); RefreshView(); }));
                diveBox.AddChild(AshfallUiHelpers.MakeButton("ABORT DIVE", () => { _statusLabel.Text = _deepCoast.CompleteDockDive(false); RefreshView(); }));
            }
        }

        private void Decide(string decisionId)
        {
            var result = _deepCoast!.Decide(decisionId, Day());
            _statusLabel.Text = result;
            RefreshView();
        }

        private int Day() => _core != null ? _core.Clock.Day : (_simDayFallback > 0 ? _simDayFallback : 1);
        private int _simDayFallback = 1;
        public void SetSimDay(int day) => _simDayFallback = day > 0 ? day : 1;

        private static string BillText(Dictionary<string, int> bill)
        {
            if (bill == null || bill.Count == 0) return "none (the Fleet pays, or nothing required)";
            var sb = new System.Text.StringBuilder();
            foreach (var kv in bill)
                sb.Append(kv.Value).Append("× ").Append(kv.Key).Append("   ");
            return sb.ToString().TrimEnd();
        }

        private static void ClearContainer(VBoxContainer container)
        {
            AshfallUiHelpers.EmptyChildren(container);
        }


    public void Unbind()
    {
        if (_deepCoast != null)
            {
                _deepCoast.StateChanged -= RefreshView;
            }
            if (_core != null)
            {
                _core.IceRoad.OnStateChanged -= HandleIceRoadStateChanged;
            }
    }

    public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
