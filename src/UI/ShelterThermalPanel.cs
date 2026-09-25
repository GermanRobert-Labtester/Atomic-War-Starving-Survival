// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ShelterThermalPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _toggleBoilerBtn = null!;

        private ShelterThermalHostSession? _host;
        private Ashfall.Core.Inventory.Inventory? _inventory;
        private Func<StormWatch.Snapshot>? _stormWatch;
        private readonly System.Collections.Generic.List<(Button Button, string RoomId)> _zoneButtons = new();

        /// <summary>Canonical inventory for retrofit costs (owned by Main).</summary>
        public void BindInventory(Ashfall.Core.Inventory.Inventory? inventory)
        {
            _inventory = inventory;
        }

        /// <summary>Storm-window read model (owned by the weather authority; read-only).</summary>
        public void BindStormWatch(Func<StormWatch.Snapshot>? stormWatch)
        {
            _stormWatch = stormWatch;
        }

        public bool IsBound => _host != null;

        public void Bind(ShelterThermalHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
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

            _shell = new AshfallDashboardShell("Shelter Thermal // Central Heating", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("boiler_status", "Boiler Status", "OFF", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("boiler_temp", "Boiler Temp", "20°C", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("fuel", "Boiler Fuel", "100 kg", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("frostbite_risk", "Frostbite Risk", "0 zones", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("storm_window", "Storm Window", "—", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Alpha feature — heat zoning: each room's radiator valve is the
            // zoning control. Cycling sends heat where it is needed without
            // touching the boiler or inventing a second heat authority.
            var zoningRow = new HBoxContainer();
            zoningRow.AddThemeConstantOverride("separation", 6);
            foreach (var room in _host.System.State.rooms)
            {
                if (room == null) continue;
                var zoneBtn = new Button { Text = $"{room.displayName} · {room.radiatorValveOpen * 100f:F0}%" };
                zoneBtn.CustomMinimumSize = new Vector2(150, 32);
                zoneBtn.TooltipText = "Cycle this room's radiator valve: 100% → 75% → 50% → 25% → 0%.";
                string roomId = room.roomId;
                zoneBtn.Pressed += () =>
                {
                    float next = NextValveStep(ValveFor(roomId));
                    _host.SetRadiatorValve(roomId, next);
                    RefreshView();
                };
                _zoneButtons.Add((zoneBtn, roomId));
                zoningRow.AddChild(zoneBtn);
            }
            _contentStack.AddChild(zoningRow);

            _detailText = AshfallUiHelpers.MakeBody("", autowrap: true);
            _contentStack.AddChild(_detailText);

            var buttonRow = AshfallUiHelpers.MakeActionBar(separation: 10);

            _toggleBoilerBtn = AshfallUiHelpers.MakeButton("Toggle Boiler On/Off", () =>
            {
                if (_host != null)
                {
                    bool active = !_host.System.State.boilerActive;
                    _host.SetBoilerActive(active);
                }
            });
            _toggleBoilerBtn.CustomMinimumSize = new Vector2(180, 36);
            buttonRow.AddChild(_toggleBoilerBtn);

            // Alpha feature F3 — storm sealing: fit the authored storm-sealing
            // insulation on every room, paying the authored cost from the
            // canonical inventory. The thermal system + its save store own the
            // effect; this button only issues the command.
            var stormSealBtn = AshfallUiHelpers.MakeButton("Storm Seal All Rooms", () =>
            {
                if (_host == null) return;
                var storm = _stormWatch?.Invoke() ?? default;
                _host.RetrofitStormSealing(_inventory, storm.StormActive);
                _statusRail?.Set("frostbite_risk", _host.LastEvent);
            });
            stormSealBtn.CustomMinimumSize = new Vector2(200, 36);
            stormSealBtn.TooltipText = "Battens, tar felt and taped seams — fit before a storm, not during one.";
            buttonRow.AddChild(stormSealBtn);

            _contentStack.AddChild(buttonRow);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var s = _host.System.State;
            RefreshZoneButtons();
            _statusRail.Set("boiler_status", s.boilerActive ? "ACTIVE" : "OFF", s.boilerActive ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("boiler_temp", $"{s.boilerCurrentTempC:F1}°C", s.boilerCurrentTempC < 10f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("fuel", $"{s.boilerFuelLevel:F0} kg", s.boilerFuelLevel < 20f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            int frostbiteZones = 0;
            foreach (var r in s.rooms) if (r.currentTempC < 5f) frostbiteZones++;
            _statusRail.Set("frostbite_risk", frostbiteZones == 0 ? "0 zones" : $"{frostbiteZones} zones <5°C", frostbiteZones > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            // Alpha feature G3 — the sealing window: storms are the reason to
            // seal, and the reason you cannot. Read-only weather authority.
            var storm = _stormWatch?.Invoke();
            if (storm.HasValue)
            {
                var sw = storm.Value;
                if (sw.StormActive)
                    _statusRail.Set("storm_window", $"ACTIVE · {sw.Kind}", AshfallMetricCard.Criticality.Critical);
                else if (sw.DaysUntilStorm == 0)
                    _statusRail.Set("storm_window", "IMMINENT · today", AshfallMetricCard.Criticality.Warn);
                else if (sw.DaysUntilStorm < int.MaxValue)
                    _statusRail.Set("storm_window", $"in {sw.DaysUntilStorm} day(s) · {sw.Kind}", AshfallMetricCard.Criticality.Caution);
                else
                    _statusRail.Set("storm_window", $"clear · {sw.Kind}", AshfallMetricCard.Criticality.Normal);
            }

            if (_detailText != null)
            {
                string text = $"Central Heating Boiler: {(s.boilerActive ? "ONLINE" : "STANDBY")} | Output: {s.totalHeatOutputKw:F1} kW\n" +
                               $"Thermal Rooms ({s.rooms.Count} zones) | Radiator Pipes ({s.pipes.Count} lines)\n" +
                               $"Incidents Recorded: {s.incidentLog.Count}\n";
                foreach (var r in s.rooms)
                {
                    string flag = r.currentTempC < 5f ? " ⚠ FROSTBITE RISK" : r.isFrozen ? " ❄ FROZEN" : "";
                    text += $"  • {r.displayName} ({r.roomId}): {r.currentTempC:F1}°C · valve {r.radiatorValveOpen * 100f:F0}%{flag}\n";
                }
                text += $"Last Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }

        private float ValveFor(string roomId)
        {
            if (_host != null)
            {
                foreach (var room in _host.System.State.rooms)
                {
                    if (room != null && room.roomId == roomId) return room.radiatorValveOpen;
                }
            }
            return 1f;
        }

        private void RefreshZoneButtons()
        {
            if (_host == null) return;
            foreach (var (button, roomId) in _zoneButtons)
            {
                if (button == null || !GodotObject.IsInstanceValid(button)) continue;
                string name = roomId;
                foreach (var room in _host.System.State.rooms)
                {
                    if (room != null && room.roomId == roomId) { name = room.displayName; break; }
                }
                button.Text = $"{name} · {ValveFor(roomId) * 100f:F0}%";
            }
        }

        private static float NextValveStep(float current)
        {
            if (current > 0.875f) return 0.75f;
            if (current > 0.625f) return 0.5f;
            if (current > 0.375f) return 0.25f;
            if (current > 0.125f) return 0f;
            return 1f;
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
