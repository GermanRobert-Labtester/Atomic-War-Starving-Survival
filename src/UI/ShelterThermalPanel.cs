using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ShelterThermalPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _toggleBoilerBtn = null!;

        private ShelterThermalHostSession? _host;

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

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _toggleBoilerBtn = new Button { Text = "Toggle Boiler On/Off", CustomMinimumSize = new Vector2(180, 36) };
            _toggleBoilerBtn.Pressed += () =>
            {
                if (_host != null)
                {
                    bool active = !_host.System.State.boilerActive;
                    _host.SetBoilerActive(active);
                }
            };
            buttonRow.AddChild(_toggleBoilerBtn);

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
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
