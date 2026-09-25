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

        /// <summary>Canonical inventory for retrofit costs (owned by Main).</summary>
        public void BindInventory(Ashfall.Core.Inventory.Inventory? inventory)
        {
            _inventory = inventory;
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

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

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
                _host.RetrofitStormSealing(_inventory);
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
            _statusRail.Set("boiler_status", s.boilerActive ? "ACTIVE" : "OFF", s.boilerActive ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("boiler_temp", $"{s.boilerCurrentTempC:F1}°C", s.boilerCurrentTempC < 10f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("fuel", $"{s.boilerFuelLevel:F0} kg", s.boilerFuelLevel < 20f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            int frostbiteZones = 0;
            foreach (var r in s.rooms) if (r.currentTempC < 5f) frostbiteZones++;
            _statusRail.Set("frostbite_risk", frostbiteZones == 0 ? "0 zones" : $"{frostbiteZones} zones <5°C", frostbiteZones > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = $"Central Heating Boiler: {(s.boilerActive ? "ONLINE" : "STANDBY")} | Output: {s.totalHeatOutputKw:F1} kW\n" +
                               $"Thermal Rooms ({s.rooms.Count} zones) | Radiator Pipes ({s.pipes.Count} lines)\n" +
                               $"Incidents Recorded: {s.incidentLog.Count}\n";
                foreach (var r in s.rooms)
                {
                    string flag = r.currentTempC < 5f ? " ⚠ FROSTBITE RISK" : r.isFrozen ? " ❄ FROZEN" : "";
                    text += $"  • {r.displayName} ({r.roomId}): {r.currentTempC:F1}°C{flag}\n";
                }
                text += $"Last Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
