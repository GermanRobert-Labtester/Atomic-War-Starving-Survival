// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Power Grid panel (item 13) — breaker controls, live reserve/load/fuel
    /// displays, and per-room priority pickers. Thin Godot-side UI: every
    /// mutation goes through <see cref="PowerGridHostSession"/>.
    /// </summary>
    public partial class PowerGridPanel : Control
    {
        public event Action<string>? OnRoomToggled;
        public event Action<string, PowerGridRoomPriority>? OnPriorityChanged;
        public event Action<string>? OnBreakerResetRequested;
        public event Action<float>? OnFuelAdded;
        public event Action? OnBatteryBankInstallRequested;
        public event Action? OnGeneratorServiceRequested;
        public event Action<string>? OnEmergencyPresetRequested;
        public event Action? OnClose;

        private PowerGridHostSession? _session;
        private Label _genLabel = null!;
        private Label _drawLabel = null!;
        private Label _batteryLabel = null!;
        private Label _fuelLabel = null!;
        private Label _deficitLabel = null!;
        private Label _brownoutLabel = null!;
        private VBoxContainer _roomList = null!;

        private static readonly PowerGridRoomPriority[] s_priorities = (PowerGridRoomPriority[])Enum.GetValues(typeof(PowerGridRoomPriority));

        public bool IsBound => _session != null;

        public void Bind(PowerGridHostSession session)
        {
            if (_session != null)
                _session.OnStateChanged -= RefreshView;
            _session = session;
            if (_session != null)
                _session.OnStateChanged += RefreshView;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_session == null || _genLabel == null || _drawLabel == null || _batteryLabel == null || _fuelLabel == null || _deficitLabel == null || _brownoutLabel == null || _roomList == null) return;
            var snap = _session.LastSnapshot;
            var tick = _session.LastTickSummary;
            float cond = _session.System.GeneratorCondition;
            _genLabel.Text = $"GEN {snap.GenerationWatts:0} W (CONDITION {cond:0}%)";
            _genLabel.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(cond < PowerGridSystem.GeneratorDegradationThreshold
                    ? DesignTheme.Critical : DesignTheme.Pale));
            _drawLabel.Text = $"DRAW {snap.TotalDrawWatts:0} W (net {snap.NetWatts:+0;-0;0})";
            float pct = snap.BatteryCapacityWh > 0
                ? (snap.BatteryReserveWh / snap.BatteryCapacityWh) * 100f : 0f;
            int banks = _session.System.InstalledBatteryBankCount;
            int maxBanks = PowerGridSystem.MaxInstalledBatteryBanks;
            _batteryLabel.Text = $"BATTERY {snap.BatteryReserveWh:0}/{snap.BatteryCapacityWh:0} Wh ({pct:0}%) — BANKS {banks}/{maxBanks}";
            _fuelLabel.Text = $"FUEL {snap.FuelUnits:0} units";

            // C2[6] 23B: canonical demand/supply/deficit/runtime read model — the
            // panel renders Core numbers, never its own arithmetic.
            float runtime = _session.System.EstimatedRuntimeHours;
            string runtimeText = float.IsPositiveInfinity(runtime) ? "INF" : $"{runtime:0.0} h";
            _deficitLabel.Text = $"SUPPLY {_session.System.AvailableSupplyWatts:0} W "
                + $"// DEFICIT {_session.System.DeficitWatts:0} W "
                + $"// BATTERY ETA {runtimeText} "
                + $"// FUEL ~{_session.System.EstimatedFuelRunwayDays:0.0} d";
            _deficitLabel.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(_session.System.DeficitWatts > 0f ? DesignTheme.Warning : DesignTheme.Pale));
            // B5–B8 Phase 9: honest status line — brownout vs critical
            // life-support deficit are different states (§8.6), and served/shed
            // numbers come from the actual tick allocation, not a guess.
            if (tick != null && tick.HasCriticalDeficit)
            {
                _brownoutLabel.Text = $"CRITICAL DEFICIT // LIFE SUPPORT UNSERVED ({tick.UnservedWatts:0} W shed)";
                _brownoutLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
            }
            else if (snap.IsBrownout)
            {
                _brownoutLabel.Text = tick != null
                    ? $"BROWNOUT // {tick.ShedRoomIds.Count} LOAD(S) SHED ({tick.UnservedWatts:0} W)"
                    : "BROWNOUT // LOAD SHED ACTIVE";
                _brownoutLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
            }
            else
            {
                _brownoutLabel.Text = "STABLE";
                _brownoutLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            }

            AshfallUiHelpers.EmptyChildren(_roomList);
            // B5–B8 Phase 9: allocation-aware per-room truth. Served ≠ the old
            // global powered read — during a brownout critical loads show
            // SERVED while optional loads show SHED, matching the tick's
            // deterministic allocation (Phase 2).
            var tickRooms = _session.LastTickSummary;
            foreach (var r in _session.System.Rooms)
            {
                bool served = tickRooms != null
                    ? tickRooms.ServedRoomIds.Contains(r.RoomId)
                    : _session.System.IsRoomServed(r.RoomId);
                bool legacyPowered = _session.System.IsRoomPowered(r.RoomId);
                bool tripped = _session.System.IsRoomTripped(r.RoomId);
                var pri = _session.System.EffectivePriority(r.RoomId);
                _roomList.AddChild(MakeRoomRow(r, served, pri, legacyPowered, tripped));
            }
        }

        private Control MakeRoomRow(PowerGridRoom r, bool served, PowerGridRoomPriority pri, bool legacyPowered, bool tripped)
        {
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var nameLbl = AshfallUiHelpers.MakeMono(r.DisplayName);
            nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            // Shed-but-breaker-closed loads get a distinct warn tone; fully
            // offline rooms (open breaker/tripped) stay muted.
            var nameColor = served ? DesignTheme.Pale
                : legacyPowered ? DesignTheme.Warning
                : DesignTheme.Muted;
            nameLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(nameColor));
            row.AddChild(nameLbl);

            var drawLbl = AshfallUiHelpers.MakeMono($"{r.DrawWatts:0} W{(legacyPowered ? "" : served ? " · SERVED" : " · SHED")}");
            drawLbl.CustomMinimumSize = new Vector2(110, 0);
            row.AddChild(drawLbl);

            var priRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingXs);
            for (int i = 0; i < s_priorities.Length; i++)
            {
                PowerGridRoomPriority p = s_priorities[i];
                var btn = AshfallUiHelpers.MakeButton(p.ToString().ToUpperInvariant(),
                    () => OnPriorityChanged?.Invoke(r.RoomId, p));
                btn.CustomMinimumSize = new Vector2(56, 24);
                if (p == pri) btn.Disabled = true;
                priRow.AddChild(btn);
            }
            row.AddChild(priRow);

            // C2[6] 23B: a tripped circuit needs an explicit (costly) reset; the
            // breaker toggle alone would not clear the trip.
            if (tripped)
            {
                var resetBtn = AshfallUiHelpers.MakeButton("RESET", () => OnBreakerResetRequested?.Invoke(r.RoomId));
                resetBtn.CustomMinimumSize = new Vector2(60, 24);
                resetBtn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
                row.AddChild(resetBtn);
            }
            else
            {
                var state = AshfallUiHelpers.MakeButton(
                    legacyPowered ? "ON" : "OFF",
                    () => OnRoomToggled?.Invoke(r.RoomId));
                state.CustomMinimumSize = new Vector2(60, 24);
                state.AddThemeColorOverride("font_color",
                    AshfallUiHelpers.ToColor(legacyPowered ? DesignTheme.Pale : DesignTheme.Warm));
                row.AddChild(state);
            }
            return row;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(760, 620);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            margins.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("POWER GRID", DesignTheme.FontSizeH2);
            vbox.AddChild(title);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var stats = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            _genLabel = AshfallUiHelpers.MakeMono("GEN 0 W");
            _drawLabel = AshfallUiHelpers.MakeMono("DRAW 0 W (net 0)");
            _batteryLabel = AshfallUiHelpers.MakeMono("BATTERY 0/0 Wh (0%)");
            _fuelLabel = AshfallUiHelpers.MakeMono("FUEL 0 units");
            _deficitLabel = AshfallUiHelpers.MakeMono("SUPPLY 0 W // DEFICIT 0 W // BATTERY ETA INF // FUEL ~0.0 d");
            _brownoutLabel = AshfallUiHelpers.MakeMono("STABLE");
            stats.AddChild(_genLabel);
            stats.AddChild(_drawLabel);
            stats.AddChild(_batteryLabel);
            stats.AddChild(_fuelLabel);
            stats.AddChild(_deficitLabel);
            stats.AddChild(_brownoutLabel);
            vbox.AddChild(stats);

            // B5–B8 Phase 2: battery-bank install (canonical item consumed by
            // the host route; the panel only raises the request).
            var bankBtn = AshfallUiHelpers.MakeButton("INSTALL BATTERY BANK",
                () => OnBatteryBankInstallRequested?.Invoke());
            bankBtn.CustomMinimumSize = new Vector2(220, 26);
            vbox.AddChild(bankBtn);

            // Alpha feature — load-shed drill: rehearse the emergency shedding
            // order through the canonical power grid (no separate drill state).
            var drillBtn = AshfallUiHelpers.MakeButton("RUN LOAD-SHED DRILL", () =>
            {
                if (_session == null) return;
                _session.RunLoadShedDrill();
                RefreshView();
            });
            drillBtn.CustomMinimumSize = new Vector2(220, 26);
            drillBtn.TooltipText = "Drop standard rooms to low priority and watch what survives. Rehearsal, not an emergency.";
            vbox.AddChild(drillBtn);

            // B5–B8 Phase 5: generator service (canonical machine_oil consumed
            // by the host route; the panel only raises the request).
            var serviceBtn = AshfallUiHelpers.MakeButton("SERVICE GENERATOR",
                () => OnGeneratorServiceRequested?.Invoke());
            serviceBtn.CustomMinimumSize = new Vector2(220, 26);
            vbox.AddChild(serviceBtn);

            // B5–B8 expansion (§27): emergency priority presets — the brownout
            // shortcut; the host applies the Core policy and journals it.
            var presetRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var shedBtn = AshfallUiHelpers.MakeButton("EMERGENCY: PRESERVE LIFE SUPPORT",
                () => OnEmergencyPresetRequested?.Invoke("shed"));
            shedBtn.CustomMinimumSize = new Vector2(280, 26);
            presetRow.AddChild(shedBtn);
            var defaultsBtn = AshfallUiHelpers.MakeButton("RESTORE DEFAULTS",
                () => OnEmergencyPresetRequested?.Invoke("defaults"));
            defaultsBtn.CustomMinimumSize = new Vector2(180, 26);
            presetRow.AddChild(defaultsBtn);
            vbox.AddChild(presetRow);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(720, 360),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            _roomList = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
            scroll.AddChild(_roomList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var fuelRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var fuelLbl = AshfallUiHelpers.MakeMono("Add fuel:");
            fuelRow.AddChild(fuelLbl);
            foreach (var amt in new[] { 10f, 25f, 50f })
            {
                var btn = AshfallUiHelpers.MakeButton($"+{amt:0}",
                    () => OnFuelAdded?.Invoke(amt));
                btn.CustomMinimumSize = new Vector2(60, 24);
                fuelRow.AddChild(btn);
            }
            vbox.AddChild(fuelRow);

            var closeRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var closeBtn = AshfallUiHelpers.MakeButton("CLOSE [Esc]",
                () => { Visible = false; OnClose?.Invoke(); });
            closeBtn.CustomMinimumSize = new Vector2(120, 28);
            closeRow.AddChild(closeBtn);
            vbox.AddChild(closeRow);
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }

        public void Unbind()
        {
            if (_session != null)
            {
                _session.OnStateChanged -= RefreshView;
                _session = null;
            }
            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
