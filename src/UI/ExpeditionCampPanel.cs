// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.Localization;

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

        /// <summary>Localized presentation string with an English fallback.</summary>
        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

        /// <summary>Localize + format without throwing on a malformed catalog row.</summary>
        private static string TrFmt(string key, string fallback, params object[] args)
        {
            string template = AshfallUiText.Tr(key, fallback);
            try { return string.Format(template, args); }
            catch (FormatException) { return template; }
        }

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

            _headerLabel = AshfallUiHelpers.MakeLabel(
                Tr("ui.expedition.camp.header", "EXPEDITION CAMP"), 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _phaseLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.phase_empty", "Phase: —"));
            root.AddChild(_phaseLabel);

            _temperatureLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.temperature_empty", "Temperature: —"));
            root.AddChild(_temperatureLabel);

            _weatherLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.weather_empty", "Weather: —"));
            root.AddChild(_weatherLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _firewoodLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.firewood_empty", "Firewood: —"));
            root.AddChild(_firewoodLabel);

            _waterLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.water_empty", "Water: —"));
            root.AddChild(_waterLabel);

            _foodLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.food_empty", "Food: —"));
            root.AddChild(_foodLabel);

            _staminaLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.stamina_empty", "Stamina: —"));
            root.AddChild(_staminaLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _nightProgressLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.night_empty", "Night: —"));
            root.AddChild(_nightProgressLabel);

            _sentryLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.sentry_empty", "Sentry: —"));
            root.AddChild(_sentryLabel);

            _coldExposureLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.cold_empty", "Cold Exposure: —"));
            root.AddChild(_coldExposureLabel);

            _encounterLabel = AshfallUiHelpers.MakeBody(Tr("ui.expedition.camp.encounter_none", "Encounter: None"));
            root.AddChild(_encounterLabel);

            _outcomeLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_outcomeLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = new HBoxContainer();
            root.AddChild(buttonRow);

            _tickButton = AshfallUiHelpers.MakeButton(
                Tr("ui.expedition.camp.advance", "Advance Night Segment"), OnTickPressed);
            buttonRow.AddChild(_tickButton);

            _breakCampButton = AshfallUiHelpers.MakeButton(
                Tr("ui.expedition.camp.break_resume", "Break Camp (Resume)"), OnBreakCampResume);
            buttonRow.AddChild(_breakCampButton);

            _retreatButton = AshfallUiHelpers.MakeButton(
                Tr("ui.expedition.camp.break_retreat", "Break Camp (Retreat)"), OnBreakCampRetreat);
            buttonRow.AddChild(_retreatButton);

            _resolveEncounterButton = AshfallUiHelpers.MakeButton(
                Tr("ui.expedition.camp.resolve", "Resolve Encounter"), OnResolveEncounter);
            _resolveEncounterButton.Visible = false;
            buttonRow.AddChild(_resolveEncounterButton);

            _closeButton = AshfallUiHelpers.MakeButton(
                Tr("ui.expedition.camp.close", "Close"), () => OnClose?.Invoke());
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
                _phaseLabel.Text = Tr("ui.expedition.camp.phase_none", "Phase: Not in camp");
                _tickButton.Disabled = true;
                _breakCampButton.Disabled = true;
                _retreatButton.Disabled = true;
                return;
            }

            _phaseLabel.Text = Tr("ui.expedition.camp.phase_night", "Phase: Camp (Night)");
            _temperatureLabel.Text = TrFmt("ui.expedition.camp.temperature",
                "Temperature: {0:F1}°C (ambient {1:F1}°C + heat {2:F1}°C)",
                camp.temperatureC + camp.heatOutput, camp.temperatureC, camp.heatOutput);
            _weatherLabel.Text = TrFmt("ui.expedition.camp.weather", "Weather: {0}", camp.weatherCondition);

            _firewoodLabel.Text = TrFmt("ui.expedition.camp.firewood",
                "Firewood: {0:F1} remaining ({1:F1} consumed)",
                camp.firewoodRemaining, camp.firewoodConsumed);
            _waterLabel.Text = TrFmt("ui.expedition.camp.water",
                "Water: {0:F1} remaining ({1:F1} consumed)",
                camp.waterReserved, camp.waterConsumed);
            _foodLabel.Text = TrFmt("ui.expedition.camp.food",
                "Food: {0:F1} remaining ({1:F1} consumed)",
                camp.foodReserved, camp.foodConsumed);

            // Get stamina from active expedition
            var exp = _expeditionHost.Engine.Active;
            if (exp.TryGetValue(_survivorId, out var expState))
            {
                _staminaLabel.Text = TrFmt("ui.expedition.camp.stamina", "Stamina: {0:F0}%", expState.stamina);
            }

            _nightProgressLabel.Text = TrFmt("ui.expedition.camp.night",
                "Night: {0}/{1} segments", camp.nightSegmentsCompleted, camp.totalNightSegments);

            bool hasSentry = camp.watchShifts.Count > 0;
            _sentryLabel.Text = hasSentry
                ? TrFmt("ui.expedition.camp.sentry_active", "Sentry: Active ({0} shifts)", camp.watchShifts.Count)
                : Tr("ui.expedition.camp.sentry_none", "Sentry: None");

            _coldExposureLabel.Text = camp.coldExposure > 0
                ? TrFmt("ui.expedition.camp.cold", "Cold Exposure: {0:F1} [!]", camp.coldExposure)
                : Tr("ui.expedition.camp.cold_none", "Cold Exposure: None");

            if (camp.encounterTriggered && !camp.encounterResolved)
            {
                _encounterLabel.Text = TrFmt("ui.expedition.camp.encounter_wildlife",
                    "Encounter: Wildlife threat (level {0}) [!]", camp.wildlifeThreatLevel);
                _resolveEncounterButton.Visible = true;
            }
            else if (camp.encounterResolved)
            {
                _encounterLabel.Text = Tr("ui.expedition.camp.encounter_resolved", "Encounter: Resolved");
                _resolveEncounterButton.Visible = false;
            }
            else
            {
                _encounterLabel.Text = Tr("ui.expedition.camp.encounter_none", "Encounter: None");
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
