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
        public event Action<float>? OnFuelAdded;

        private PowerGridHostSession? _session;
        private Label _genLabel = null!;
        private Label _drawLabel = null!;
        private Label _batteryLabel = null!;
        private Label _fuelLabel = null!;
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
            if (_session == null || _genLabel == null || _drawLabel == null || _batteryLabel == null || _fuelLabel == null || _brownoutLabel == null || _roomList == null) return;
            var snap = _session.LastSnapshot;
            _genLabel.Text = $"GEN {snap.GenerationWatts:0} W";
            _drawLabel.Text = $"DRAW {snap.TotalDrawWatts:0} W (net {snap.NetWatts:+0;-0;0})";
            float pct = snap.BatteryCapacityWh > 0
                ? (snap.BatteryReserveWh / snap.BatteryCapacityWh) * 100f : 0f;
            _batteryLabel.Text = $"BATTERY {snap.BatteryReserveWh:0}/{snap.BatteryCapacityWh:0} Wh ({pct:0}%)";
            _fuelLabel.Text = $"FUEL {snap.FuelUnits:0} units";
            _brownoutLabel.Text = snap.IsBrownout ? "BROWNOUT // LOAD SHED ACTIVE" : "STABLE";
            _brownoutLabel.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(snap.IsBrownout ? DesignTheme.Critical : DesignTheme.Pale));

            AshfallUiHelpers.EmptyChildren(_roomList);
            foreach (var r in _session.System.Rooms)
            {
                bool powered = _session.System.IsRoomPowered(r.RoomId);
                var pri = _session.System.EffectivePriority(r.RoomId);
                _roomList.AddChild(MakeRoomRow(r, powered, pri));
            }
        }

        private Control MakeRoomRow(PowerGridRoom r, bool powered, PowerGridRoomPriority pri)
        {
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var nameLbl = AshfallUiHelpers.MakeMono(r.DisplayName);
            nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            nameLbl.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(powered ? DesignTheme.Pale : DesignTheme.Muted));
            row.AddChild(nameLbl);

            var drawLbl = AshfallUiHelpers.MakeMono($"{r.DrawWatts:0} W");
            drawLbl.CustomMinimumSize = new Vector2(70, 0);
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

            var state = AshfallUiHelpers.MakeButton(
                powered ? "ON" : "OFF",
                () => OnRoomToggled?.Invoke(r.RoomId));
            state.CustomMinimumSize = new Vector2(60, 24);
            state.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(powered ? DesignTheme.Pale : DesignTheme.Warm));
            row.AddChild(state);
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
            _brownoutLabel = AshfallUiHelpers.MakeMono("STABLE");
            stats.AddChild(_genLabel);
            stats.AddChild(_drawLabel);
            stats.AddChild(_batteryLabel);
            stats.AddChild(_fuelLabel);
            stats.AddChild(_brownoutLabel);
            vbox.AddChild(stats);

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
                () => Visible = false);
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
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
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
