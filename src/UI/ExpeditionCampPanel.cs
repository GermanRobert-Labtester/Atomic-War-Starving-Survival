using System;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Expedition Camp panel.
    /// Thin presentation layer for overnight camp management.
    /// Shows camp state, supplies, temperature, sentry, and night progress.
    /// All gameplay logic delegates to ExpeditionHostSession → ExpeditionSystem.
    /// </summary>
    public partial class ExpeditionCampPanel : Control
    {
        public event Action? OnClose;
        public event Action? OnCampResolved;

        private ExpeditionHostSession? _expeditionHost;
        private string _survivorId = string.Empty;

        private Label _headerLabel = null!;
        private Label _phaseLabel = null!;
        private Label _temperatureLabel = null!;
        private Label _weatherLabel = null!;
        private Label _firewoodLabel = null!;
        private Label _waterLabel = null!;
        private Label _foodLabel = null!;
        private Label _staminaLabel = null!;
        private Label _nightProgressLabel = null!;
        private Label _sentryLabel = null!;
        private Label _coldExposureLabel = null!;
        private Label _encounterLabel = null!;
        private Label _outcomeLabel = null!;
        private Button _tickButton = null!;
        private Button _breakCampButton = null!;
        private Button _retreatButton = null!;
        private Button _resolveEncounterButton = null!;
        private Button _closeButton = null!;

        public bool IsBound => _expeditionHost != null;

        public void Bind(ExpeditionHostSession expeditionHost, string survivorId)
        {
            if (_expeditionHost != null)
            {
                _expeditionHost.StateChanged -= RefreshView;
            }

            _expeditionHost = expeditionHost;
            _survivorId = survivorId;

            if (_expeditionHost != null)
            {
                _expeditionHost.StateChanged += RefreshView;
                RefreshView();
            }
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public override void _Ready()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("EXPEDITION CAMP", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _phaseLabel = AshfallUiHelpers.MakeBody("Phase: —");
            root.AddChild(_phaseLabel);

            _temperatureLabel = AshfallUiHelpers.MakeBody("Temperature: —");
            root.AddChild(_temperatureLabel);

            _weatherLabel = AshfallUiHelpers.MakeBody("Weather: —");
            root.AddChild(_weatherLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _firewoodLabel = AshfallUiHelpers.MakeBody("Firewood: —");
            root.AddChild(_firewoodLabel);

            _waterLabel = AshfallUiHelpers.MakeBody("Water: —");
            root.AddChild(_waterLabel);

            _foodLabel = AshfallUiHelpers.MakeBody("Food: —");
            root.AddChild(_foodLabel);

            _staminaLabel = AshfallUiHelpers.MakeBody("Stamina: —");
            root.AddChild(_staminaLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _nightProgressLabel = AshfallUiHelpers.MakeBody("Night: —");
            root.AddChild(_nightProgressLabel);

            _sentryLabel = AshfallUiHelpers.MakeBody("Sentry: —");
            root.AddChild(_sentryLabel);

            _coldExposureLabel = AshfallUiHelpers.MakeBody("Cold Exposure: —");
            root.AddChild(_coldExposureLabel);

            _encounterLabel = AshfallUiHelpers.MakeBody("Encounter: None");
            root.AddChild(_encounterLabel);

            _outcomeLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_outcomeLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = new HBoxContainer();
            root.AddChild(buttonRow);

            _tickButton = AshfallUiHelpers.MakeButton("Advance Night Segment", OnTickPressed);
            buttonRow.AddChild(_tickButton);

            _breakCampButton = AshfallUiHelpers.MakeButton("Break Camp (Resume)", OnBreakCampResume);
            buttonRow.AddChild(_breakCampButton);

            _retreatButton = AshfallUiHelpers.MakeButton("Break Camp (Retreat)", OnBreakCampRetreat);
            buttonRow.AddChild(_retreatButton);

            _resolveEncounterButton = AshfallUiHelpers.MakeButton("Resolve Encounter", OnResolveEncounter);
            _resolveEncounterButton.Visible = false;
            buttonRow.AddChild(_resolveEncounterButton);

            _closeButton = AshfallUiHelpers.MakeButton("Close", () => OnClose?.Invoke());
            buttonRow.AddChild(_closeButton);
        }

        private void OnTickPressed()
        {
            if (_expeditionHost == null) return;
            string result = _expeditionHost.CampTick(_survivorId);
            _outcomeLabel.Text = result;
            RefreshView();
        }

        private void OnBreakCampResume()
        {
            if (_expeditionHost == null) return;
            string result = _expeditionHost.BreakCamp(_survivorId, retreat: false);
            _outcomeLabel.Text = result;
            RefreshView();
            OnCampResolved?.Invoke();
        }

        private void OnBreakCampRetreat()
        {
            if (_expeditionHost == null) return;
            string result = _expeditionHost.BreakCamp(_survivorId, retreat: true);
            _outcomeLabel.Text = result;
            RefreshView();
            OnCampResolved?.Invoke();
        }

        private void OnResolveEncounter()
        {
            if (_expeditionHost == null) return;
            string result = _expeditionHost.ResolveCampEncounter(_survivorId, "resolved");
            _outcomeLabel.Text = result;
            RefreshView();
        }

        private void RefreshView()
        {
            if (_expeditionHost == null) return;

            var camp = _expeditionHost.GetCampState(_survivorId);
            if (camp == null)
            {
                _phaseLabel.Text = "Phase: Not in camp";
                _tickButton.Disabled = true;
                _breakCampButton.Disabled = true;
                _retreatButton.Disabled = true;
                return;
            }

            _phaseLabel.Text = "Phase: Camp (Night)";
            _temperatureLabel.Text = $"Temperature: {camp.temperatureC + camp.heatOutput:F1}°C (ambient {camp.temperatureC:F1}°C + heat {camp.heatOutput:F1}°C)";
            _weatherLabel.Text = $"Weather: {camp.weatherCondition}";

            _firewoodLabel.Text = $"Firewood: {camp.firewoodRemaining:F1} remaining ({camp.firewoodConsumed:F1} consumed)";
            _waterLabel.Text = $"Water: {camp.waterReserved:F1} remaining ({camp.waterConsumed:F1} consumed)";
            _foodLabel.Text = $"Food: {camp.foodReserved:F1} remaining ({camp.foodConsumed:F1} consumed)";

            // Get stamina from active expedition
            var exp = _expeditionHost.Engine.Active;
            if (exp.TryGetValue(_survivorId, out var expState))
            {
                _staminaLabel.Text = $"Stamina: {expState.stamina:F0}%";
            }

            _nightProgressLabel.Text = $"Night: {camp.nightSegmentsCompleted}/{camp.totalNightSegments} segments";

            bool hasSentry = camp.watchShifts.Count > 0;
            _sentryLabel.Text = hasSentry
                ? $"Sentry: Active ({camp.watchShifts.Count} shifts)"
                : "Sentry: None";

            _coldExposureLabel.Text = camp.coldExposure > 0
                ? $"Cold Exposure: {camp.coldExposure:F1} [!]"
                : "Cold Exposure: None";

            if (camp.encounterTriggered && !camp.encounterResolved)
            {
                _encounterLabel.Text = $"Encounter: Wildlife threat (level {camp.wildlifeThreatLevel}) [!]";
                _resolveEncounterButton.Visible = true;
            }
            else if (camp.encounterResolved)
            {
                _encounterLabel.Text = "Encounter: Resolved";
                _resolveEncounterButton.Visible = false;
            }
            else
            {
                _encounterLabel.Text = "Encounter: None";
                _resolveEncounterButton.Visible = false;
            }

            bool nightComplete = camp.nightSegmentsCompleted >= camp.totalNightSegments;
            _tickButton.Disabled = nightComplete;
            _breakCampButton.Disabled = !nightComplete;
            _retreatButton.Disabled = !nightComplete;
        }

        public override void _ExitTree()
        {
            if (_expeditionHost != null)
            {
                _expeditionHost.StateChanged -= RefreshView;
            }
        }
    }
}
