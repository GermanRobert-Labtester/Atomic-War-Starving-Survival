using System;
using Godot;
using Ashfall.Core.World;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class FalloutPlumePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _btnEmergencySeal = null!;

        private FalloutSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(FalloutSystem system)
        {
            _system = system;
            RefreshView();
        }

        public void Unbind()
        {
            _system = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("ATMOSPHERIC HAZARD // FALLOUT TRACKING & DISPERSAL", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("plumes", "Active Plumes", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("water_table", "Groundwater Status", "Clean", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("seal", "Blast Hatch", "OPEN", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 16);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No active radioactive clouds detected in the local sector.";
            _contentStack.AddChild(_detailText);

            _btnEmergencySeal = new Button { Text = "ENGAGE EMERGENCY 48H AIRLOCK SEAL" };
            _btnEmergencySeal.CustomMinimumSize = new Vector2(400, 40);
            _btnEmergencySeal.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            _btnEmergencySeal.Pressed += () => {
                if (_system != null && !_system.IsShelterSealed)
                {
                    _system.SealShelter(48);
                    RefreshView();
                }
            };
            _contentStack.AddChild(_btnEmergencySeal);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE (ESC)", () => OnClose?.Invoke());

            Visible = false;
            RefreshView();
        }

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

        public void RefreshView()
        {
            if (_statusRail == null || _detailText == null) return;
            if (_system == null)
            {
                _detailText.Text = "Atmospheric sensors offline.";
                _btnEmergencySeal.Disabled = true;
                return;
            }

            _statusRail.Set("plumes", _system.ActiveClouds.Count.ToString(), _system.ActiveClouds.Count > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            bool waterTainted = _system.State.taintedWaterSources.Count > 0;
            _statusRail.Set("water_table", waterTainted ? "CONTAMINATED" : "SAFE", waterTainted ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            _statusRail.Set("seal", _system.IsShelterSealed ? $"SEALED ({_system.State.sealDurationHoursRemaining:F0}h)" : "OPEN", _system.IsShelterSealed ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            _btnEmergencySeal.Disabled = _system.IsShelterSealed;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== RADAR DISPERSAL GRID ===");
            if (_system.ActiveClouds.Count == 0)
            {
                sb.AppendLine("  Sector Clear.");
            }
            else
            {
                foreach (var cloud in _system.ActiveClouds)
                {
                    sb.AppendLine($"- [PATTERN: {cloud.patternId}] RADIUS: {cloud.radius:F1}km | TOXICITY: {cloud.toxicity:F1} | DISPERSAL: {cloud.baseDispersalRate:F2}");
                }
            }
            _detailText.Text = sb.ToString();
        }
    }
}
