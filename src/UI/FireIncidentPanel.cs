using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    using DesignTheme = Ashfall.Core.UI.Theme;
    /// <summary>
    /// ASHFALL — Fire Incident panel.
    /// Thin presentation layer for shelter fire management.
    /// Shows fire/smoke/CO levels, damper state, brigade, extinguishers,
    /// and incident resolution.
    /// All gameplay logic delegates to ShelterFireHazardSystem.
    /// </summary>
    public partial class FireIncidentPanel : Control
    {
        public event Action? OnClose;

        private ShelterFireHostSession? _hostSession;
        private ShelterFireHazardSystem? _fireSystem;
        private string _incidentId = string.Empty;

        private void OnFireStateChanged(Dictionary<string, FireIncidentState> _) => RefreshView();

        public ShelterFireHostSession? HostSession => _hostSession;
        public ShelterFireHazardSystem? FireSystem => _fireSystem;
        public string CurrentIncidentId => _incidentId;
        public ISeededRng? Rng { get; set; }
        public Func<List<string>>? RosterWorkerProvider { get; set; }

        private Label _headerLabel = null!;
        private Label _statusLabel = null!;
        private Label _alarmLabel = null!;
        private Label _brigadeLabel = null!;
        private Label _extinguisherLabel = null!;
        private Label _damageLabel = null!;
        private VBoxContainer _zonesContainer = null!;
        private Label _feedbackLabel = null!;
        private Button _alarmButton = null!;
        private Button _brigadeButton = null!;
        private Button _extinguisherButton = null!;
        private Button _closeButton = null!;

        public bool IsBound => _fireSystem != null;

        public void Bind(ShelterFireHostSession session, string? incidentId = null)
        {
            _hostSession = session;
            BindSystem(session.System, incidentId);
        }

        public void Bind(ShelterFireHazardSystem fireSystem, string? incidentId = null)
        {
            _hostSession = null;
            BindSystem(fireSystem, incidentId);
        }

        private void BindSystem(ShelterFireHazardSystem fireSystem, string? incidentId)
        {
            if (_fireSystem != null)
            {
                _fireSystem.OnStateChanged -= OnFireStateChanged;
            }

            _fireSystem = fireSystem;
            _incidentId = ResolveActiveIncidentId(fireSystem, incidentId);

            if (_fireSystem != null)
            {
                _fireSystem.OnStateChanged += OnFireStateChanged;
                RefreshView();
            }
        }

        /// <summary>Detach the panel before its campaign authority is replaced.</summary>
        public void Unbind()
        {
            if (_fireSystem != null)
                _fireSystem.OnStateChanged -= OnFireStateChanged;
            _fireSystem = null;
            _hostSession = null;
            _incidentId = string.Empty;
            Rng = null;
            RosterWorkerProvider = null;
        }

        public void SelectIncident(string incidentId)
        {
            _incidentId = incidentId ?? string.Empty;
            RefreshView();
        }

        private static string ResolveActiveIncidentId(ShelterFireHazardSystem? fireSystem, string? incidentId)
        {
            if (fireSystem == null) return string.Empty;
            if (!string.IsNullOrEmpty(incidentId) && fireSystem.Incidents.ContainsKey(incidentId))
                return incidentId;

            foreach (var kvp in fireSystem.Incidents)
            {
                if (!kvp.Value.isResolved)
                    return kvp.Key;
            }

            foreach (var kvp in fireSystem.Incidents)
            {
                return kvp.Key;
            }

            return string.Empty;
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public override void _Ready()
        {
            if (_statusLabel == null)
                BuildUI();
        }

        private void BuildUI()
        {
            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("FIRE INCIDENT", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var metricsContainer = new VBoxContainer();
            metricsContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            root.AddChild(metricsContainer);

            _statusLabel = AshfallUiHelpers.MakeBody("Status: —");
            metricsContainer.AddChild(_statusLabel);

            _alarmLabel = AshfallUiHelpers.MakeBody("Alarm: —");
            metricsContainer.AddChild(_alarmLabel);

            _brigadeLabel = AshfallUiHelpers.MakeBody("Brigade: —");
            metricsContainer.AddChild(_brigadeLabel);

            _extinguisherLabel = AshfallUiHelpers.MakeBody("Extinguishers: —");
            metricsContainer.AddChild(_extinguisherLabel);

            _damageLabel = AshfallUiHelpers.MakeBody("Structural Damage: —");
            metricsContainer.AddChild(_damageLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _zonesContainer = new VBoxContainer();
            root.AddChild(_zonesContainer);

            _feedbackLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_feedbackLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = AshfallUiHelpers.MakeActionBar();
            root.AddChild(buttonRow);

            _alarmButton = AshfallUiHelpers.MakeButton("Raise Alarm", OnRaiseAlarm);
            buttonRow.AddChild(_alarmButton);

            _brigadeButton = AshfallUiHelpers.MakeButton("Dispatch Brigade", OnDispatchBrigade);
            buttonRow.AddChild(_brigadeButton);

            _extinguisherButton = AshfallUiHelpers.MakeButton("Deploy Extinguisher", OnDeployExtinguisher);
            buttonRow.AddChild(_extinguisherButton);

            _closeButton = AshfallUiHelpers.MakeButton("Close", () => OnClose?.Invoke());
            buttonRow.AddChild(_closeButton);
        }

        private void OnRaiseAlarm()
        {
            if (_fireSystem == null) return;
            bool ok = _fireSystem.RaiseAlarm(_incidentId);
            _feedbackLabel.Text = ok ? "Alarm raised!" : "Alarm already raised.";
            RefreshView();
        }

        private void OnDispatchBrigade()
        {
            if (_fireSystem == null) return;
            var workers = RosterWorkerProvider?.Invoke();
            if (workers == null || workers.Count == 0)
            {
                _feedbackLabel.Text = "Cannot dispatch brigade: no eligible survivors available.";
                RefreshView();
                return;
            }
            bool ok = _fireSystem.AssignBrigade(_incidentId, workers);
            _feedbackLabel.Text = ok ? "Brigade dispatched." : "Cannot dispatch brigade.";
            RefreshView();
        }

        private void OnDeployExtinguisher()
        {
            if (_fireSystem == null) return;
            var incident = _fireSystem.GetIncident(_incidentId);
            if (incident == null) return;
            // Deploy to hottest zone
            FireZoneState? hottest = null;
            foreach (var z in incident.zones)
                if (hottest == null || z.fireLevel > hottest.fireLevel) hottest = z;
            if (hottest != null)
            {
                bool ok = _fireSystem.DeployExtinguisher(_incidentId, hottest.zoneId);
                _feedbackLabel.Text = ok ? $"Extinguisher deployed to {hottest.displayName}." : "No charges remaining.";
            }
            RefreshView();
        }

        private void OnTick()
        {
            if (_fireSystem == null) return;
            var rng = Rng ?? new CoreSeededRng(StableHash.Of(_incidentId));
            _fireSystem.Tick(_incidentId, rng);
            _feedbackLabel.Text = "Tick advanced.";
            RefreshView();
        }

        private void RefreshView()
        {
            if (_statusLabel == null)
                BuildUI();

            if (_fireSystem == null)
            {
                if (_statusLabel != null) _statusLabel.Text = "Status: Fire suppression system offline (no session bound)";
                if (_alarmButton != null) _alarmButton.Disabled = true;
                if (_brigadeButton != null) _brigadeButton.Disabled = true;
                if (_extinguisherButton != null) _extinguisherButton.Disabled = true;
                return;
            }

            if (string.IsNullOrEmpty(_incidentId) || _fireSystem.GetIncident(_incidentId) == null)
            {
                _incidentId = ResolveActiveIncidentId(_fireSystem, null);
            }

            var incident = string.IsNullOrEmpty(_incidentId) ? null : _fireSystem.GetIncident(_incidentId);
            if (incident == null)
            {
                if (_statusLabel != null) _statusLabel.Text = "Status: No active fire incidents in shelter";
                if (_zonesContainer != null)
                {
                    AshfallUiHelpers.EmptyChildren(_zonesContainer);
                    _zonesContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("All shelter sectors clear of thermal hazards"));
                }
                if (_alarmButton != null) _alarmButton.Disabled = true;
                if (_brigadeButton != null) _brigadeButton.Disabled = true;
                if (_extinguisherButton != null) _extinguisherButton.Disabled = true;
                return;
            }

            if (_statusLabel != null)
            {
                _statusLabel.Text = incident.isResolved
                    ? $"Status: RESOLVED ({incident.resolution})"
                    : $"Status: Active (tick {incident.ticksElapsed})";
            }
            if (_alarmLabel != null) _alarmLabel.Text = $"Alarm: {(incident.alarmRaised ? "RAISED" : "Not raised")}";
            if (_brigadeLabel != null) _brigadeLabel.Text = $"Brigade: {incident.brigadeWorkers.Count} workers";
            if (_extinguisherLabel != null) _extinguisherLabel.Text = $"Extinguishers: {incident.extinguisherChargesUsed}/{ShelterFireHazardSystem.ExtinguisherMaxCharges} used";
            if (_damageLabel != null) _damageLabel.Text = $"Structural Damage: {incident.structuralDamage:P0}";

            // Clear and rebuild zone display
            foreach (var child in _zonesContainer.GetChildren())
                child.QueueFree();

            foreach (var z in incident.zones)
            {
                var row = new HBoxContainer();
                _zonesContainer.AddChild(row);

                string status = z.fireLevel > 0.5f ? "[FIRE]" : z.fireLevel > 0 ? "[smoldering]" : "[clear]";
                string smoke = z.smokeLevel > ShelterFireHazardSystem.CriticalSmokeLevel ? "[SMOKE]" : "";
                string co = z.coLevel > ShelterFireHazardSystem.CriticalCoLevel ? "[CO]" : "";
                string evac = z.isEvacuated ? "[EVAC]" : "";

                var label = AshfallUiHelpers.MakeBody(
                    $"  {z.displayName}: fire={z.fireLevel:F2} smoke={z.smokeLevel:F2} CO={z.coLevel:F2} " +
                    $"damper={(z.damperOpen ? "open" : "closed")} {status}{smoke}{co}{evac}");
                row.AddChild(label);
            }

            _alarmButton.Disabled = incident.alarmRaised || incident.isResolved;
            // Re-enable after empty/offline paths disabled the control; stay
            // disabled once a brigade is already assigned or the incident ended.
            _brigadeButton.Disabled = incident.isResolved || incident.brigadeWorkers.Count > 0;
            _extinguisherButton.Disabled = incident.isResolved || incident.extinguisherChargesUsed >= ShelterFireHazardSystem.ExtinguisherMaxCharges;
        }

        public void RaiseAlarmForTest() => OnRaiseAlarm();
        public void DispatchBrigadeForTest() => OnDispatchBrigade();
        public void DeployExtinguisherForTest() => OnDeployExtinguisher();
        public void AdvanceTickForTest() => OnTick();

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
