// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Disease;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace Ashfall.Core.UI
{
    public sealed class CrisisPresentationCoordinator
    {
        private CrisisPresentationSnapshot _currentSnapshot = new CrisisPresentationSnapshot();
        public CrisisPresentationSnapshot CurrentSnapshot => _currentSnapshot;

        public event Action<CrisisPresentationSnapshot>? OnCrisisChanged;

        private PowerGridSystem? _power;
        private DiseaseSystem? _disease;
        private WeatherSystem? _weather;
        private StartingLevelSystem? _startingLevel;
        private RadiationSystem? _radiation;
        private SurvivorFateSystem? _fate;
        // Plan 194 — producers previously unwired to the aggregator.
        private ShelterFireHazardSystem? _fire;
        private SumpFloodingSystem? _sump;

        private CrisisPresentationSnapshot? _customCrisis;

        public void Bind(
            PowerGridSystem? power,
            DiseaseSystem? disease,
            WeatherSystem? weather,
            StartingLevelSystem? startingLevel = null,
            RadiationSystem? radiation = null,
            SurvivorFateSystem? fate = null,
            ShelterFireHazardSystem? fire = null,
            SumpFloodingSystem? sump = null)
        {
            _power = power;
            _disease = disease;
            _weather = weather;
            _startingLevel = startingLevel;
            _radiation = radiation;
            _fate = fate;
            _fire = fire;
            _sump = sump;
        }

        public void TriggerCustomCrisis(CrisisPresentationSnapshot snapshot)
        {
            _customCrisis = snapshot ?? throw new ArgumentNullException(nameof(snapshot));
            _customCrisis.IsActive = true;
            ApplySnapshot(_customCrisis);
        }

        public void ClearCrisis()
        {
            _customCrisis = null;
            var newSnapshot = new CrisisPresentationSnapshot { IsActive = false, Severity = CrisisSeverity.None };
            ApplySnapshot(newSnapshot);
        }

        public void AcknowledgeCurrentCrisis()
        {
            if (_currentSnapshot.IsActive)
            {
                // Acknowledging doesn't silence underlying simulation, but records acknowledgment in log
                _currentSnapshot.Log.Add(new CrisisLogEntryView
                {
                    Timestamp = "LOG",
                    Message = "Crisis acknowledged by shelter commander.",
                    IsError = false
                });
                OnCrisisChanged?.Invoke(_currentSnapshot);
            }
        }

        public void EvaluateCrisisState()
        {
            if (_customCrisis != null && _customCrisis.IsActive)
            {
                ApplySnapshot(_customCrisis);
                return;
            }

            var newSnapshot = new CrisisPresentationSnapshot();

            bool powerFailing = _power != null && (_power.IsBrownout || _power.GenerationWatts <= 0f);
            bool filterHazard = _startingLevel != null && _startingLevel.State.airHazardWarning;
            int activeOutbreaks = _disease != null ? _disease.State.diseases.Count(d => d.outbreak_active) : 0;
            bool diseaseCritical = activeOutbreaks > 0;
            bool isSevereStorm = _weather != null && (_weather.Current == WeatherKind.FalloutStorm || _weather.Current == WeatherKind.EMPStorm || _weather.Current == WeatherKind.Blizzard);

            // Plan 194 — previously unwired producers. Priorities are appended
            // after the existing four so current behaviour and tests are unchanged.
            bool isBurning = _fire != null && _fire.Incidents.Values.Any(i => i != null && !i.isResolved && !i.isSuppressed);
            bool isFlooding = _sump != null && _sump.State.nodes.Any(n => n != null && n.isFlooded);
            int acuteRadiationCases = _radiation != null ? _radiation.Registered.Count(s => s != null && s.HasAcuteRadiationSickness) : 0;
            bool recentDeathRecorded = _fate != null && _fate.PendingDayEventCount > 0;

            // Establish priority
            if (powerFailing)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_power_failure";
                newSnapshot.Kind = "Power";
                newSnapshot.Severity = CrisisSeverity.Critical;
                newSnapshot.HeaderText = "CRITICAL POWER FAILURE";
                newSnapshot.CauseText = "Generator output insufficient for active grid demand.";
                newSnapshot.EffectText = "Life support systems degrading. Critical shelter loads at risk.";
                newSnapshot.AudioStateId = "crisis_critical_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Generation",
                    ValueText = _power != null ? $"{_power.GenerationWatts:F0} W" : "0 W",
                    Trend = "↓",
                    IsFailing = true
                });
                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Grid Demand",
                    ValueText = _power != null ? $"{_power.TotalDrawWatts:F0} W" : "0 W",
                    Trend = "↑",
                    IsFailing = true
                });
                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Fuel Reserve",
                    ValueText = _power != null ? $"{_power.FuelUnits:F1}" : "0",
                    Trend = "↓",
                    IsFailing = _power != null && _power.FuelUnits <= 1f
                });

                newSnapshot.Affected.Add(new CrisisAffectedEntityView
                {
                    Name = "Ventilation Scrubbers",
                    Role = "Life Support",
                    Status = "OFFLINE",
                    IsCritical = true,
                    NavigationTarget = "power_grid"
                });
                newSnapshot.Affected.Add(new CrisisAffectedEntityView
                {
                    Name = "Medical Cold Storage",
                    Role = "Infirmary",
                    Status = "DEGRADED",
                    IsCritical = true,
                    NavigationTarget = "medical"
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "shed_load",
                    Label = "Shed Non-Critical Load",
                    CostText = "0 Fuel",
                    ExpectedEffect = "Restores breaker stability",
                    Shortcut = "1",
                    IsEnabled = true
                });
                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });

                newSnapshot.Log.Add(new CrisisLogEntryView
                {
                    Timestamp = "T-0",
                    Message = "Electrical bus overload detected. Main breaker tripped.",
                    IsError = true
                });
            }
            else if (filterHazard)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_filter_hazard";
                newSnapshot.Kind = "Ventilation";
                newSnapshot.Severity = CrisisSeverity.Severe;
                newSnapshot.HeaderText = "AIR SCRUBBER / FILTER HAZARD";
                newSnapshot.CauseText = "Filter particulate saturation has reached critical operating threshold.";
                newSnapshot.EffectText = "Airflow restricted. Airborne fallout particles entering shelter living quarters.";
                newSnapshot.AudioStateId = "crisis_severe_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Air Quality",
                    ValueText = "HAZARDOUS",
                    Trend = "↓",
                    IsFailing = true
                });
                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Intake Flow",
                    ValueText = "< 25%",
                    Trend = "↓",
                    IsFailing = true
                });

                newSnapshot.Affected.Add(new CrisisAffectedEntityView
                {
                    Name = "Air Filtration Array",
                    Role = "Atmosphere",
                    Status = "CLOGGED",
                    IsCritical = true,
                    NavigationTarget = "shelter"
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "service_filter",
                    Label = "Service Filter Stack",
                    CostText = "1 Filter Cartridge",
                    ExpectedEffect = "Clears intake restriction",
                    Shortcut = "1",
                    IsEnabled = true
                });
                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });

                newSnapshot.Log.Add(new CrisisLogEntryView
                {
                    Timestamp = "T-0",
                    Message = "Air filter saturation exceeds tolerance. Respiratory warning active.",
                    IsError = true
                });
            }
            else if (diseaseCritical)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_disease_outbreak";
                newSnapshot.Kind = "Disease";
                newSnapshot.Severity = CrisisSeverity.Severe;
                newSnapshot.HeaderText = "CONTAGION OUTBREAK";
                newSnapshot.CauseText = "Active pathogen detected in general shelter population.";
                newSnapshot.EffectText = "Infected survivors symptomatic. High transmission risk across work shifts.";
                newSnapshot.AudioStateId = "crisis_severe_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Active Outbreaks",
                    ValueText = activeOutbreaks.ToString(),
                    Trend = "↑",
                    IsFailing = true
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "quarantine",
                    Label = "Enforce Quarantine",
                    ExpectedEffect = "Halts shift contagion",
                    Shortcut = "1",
                    IsEnabled = true
                });
                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });

                newSnapshot.Log.Add(new CrisisLogEntryView
                {
                    Timestamp = "T-0",
                    Message = "Pathogen transmission alert logged by medical coordinator.",
                    IsError = true
                });
            }
            else if (isSevereStorm)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_severe_storm";
                newSnapshot.Kind = "Weather";
                newSnapshot.Severity = CrisisSeverity.Elevated;
                newSnapshot.HeaderText = "EXTERIOR WEATHER HAZARD";
                newSnapshot.CauseText = $"Severe storm event underway ({_weather?.Current}).";
                newSnapshot.EffectText = "Expedition routes closed. Surface intake subject to heavy particulate abrasion.";
                newSnapshot.AudioStateId = "crisis_advisory_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Exterior Storm",
                    ValueText = _weather?.Current.ToString() ?? "Unknown",
                    Trend = "→",
                    IsFailing = false
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });
            }
            else if (isBurning)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_shelter_fire";
                newSnapshot.Kind = "Fire";
                newSnapshot.Severity = CrisisSeverity.Critical;
                newSnapshot.HeaderText = "SHELTER FIRE";
                newSnapshot.CauseText = "An unresolved fire is burning in a shelter zone.";
                newSnapshot.EffectText = "Smoke and heat spreading. Suppress the fire before it reaches adjacent zones.";
                newSnapshot.AudioStateId = "crisis_critical_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Active Fires",
                    ValueText = _fire != null
                        ? _fire.Incidents.Values.Count(i => i != null && !i.isResolved && !i.isSuppressed).ToString()
                        : "0",
                    Trend = "↑",
                    IsFailing = true
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "suppress_fire",
                    Label = "Open Fire Response",
                    ExpectedEffect = "Suppression controls",
                    Shortcut = "1",
                    IsEnabled = true
                });
                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });

                newSnapshot.Log.Add(new CrisisLogEntryView
                {
                    Timestamp = "T-0",
                    Message = "Fire hazard authority reports an unresolved incident.",
                    IsError = true
                });
            }
            else if (isFlooding)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_sump_flooding";
                newSnapshot.Kind = "Flood";
                newSnapshot.Severity = CrisisSeverity.Severe;
                newSnapshot.HeaderText = "SUMP FLOODING";
                newSnapshot.CauseText = "Groundwater has breached a sump node.";
                newSnapshot.EffectText = "Equipment in the flooded node may disable until drained.";
                newSnapshot.AudioStateId = "crisis_severe_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Flooded Nodes",
                    ValueText = _sump != null ? _sump.State.nodes.Count(n => n != null && n.isFlooded).ToString() : "0",
                    Trend = "↑",
                    IsFailing = true
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });
            }
            else if (acuteRadiationCases > 0)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_acute_radiation";
                newSnapshot.Kind = "Radiation";
                newSnapshot.Severity = CrisisSeverity.Critical;
                newSnapshot.HeaderText = "ACUTE RADIATION SICKNESS";
                newSnapshot.CauseText = "One or more survivors are showing acute radiation sickness.";
                newSnapshot.EffectText = "Medical treatment and dose reduction are required.";
                newSnapshot.AudioStateId = "crisis_critical_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Acute Cases",
                    ValueText = acuteRadiationCases.ToString(),
                    Trend = "→",
                    IsFailing = true
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });
            }
            else if (recentDeathRecorded)
            {
                newSnapshot.IsActive = true;
                newSnapshot.CrisisId = "crisis_survivor_death";
                newSnapshot.Kind = "Memorial";
                newSnapshot.Severity = CrisisSeverity.Elevated;
                newSnapshot.HeaderText = "A NAME IS ADDED TO THE WALL";
                newSnapshot.CauseText = "A survivor's fate has been recorded.";
                newSnapshot.EffectText = "The memorial and journal record the loss.";
                newSnapshot.AudioStateId = "crisis_advisory_stinger";

                newSnapshot.Metrics.Add(new CrisisMetricView
                {
                    Label = "Unacknowledged Records",
                    ValueText = _fate != null ? _fate.PendingDayEventCount.ToString() : "0",
                    Trend = "→",
                    IsFailing = false
                });

                newSnapshot.Actions.Add(new CrisisActionView
                {
                    ActionId = "ack",
                    Label = "Acknowledge",
                    Shortcut = "Space",
                    IsEnabled = true
                });
            }
            else
            {
                newSnapshot.IsActive = false;
                newSnapshot.Severity = CrisisSeverity.None;
            }

            ApplySnapshot(newSnapshot);
        }

        private void ApplySnapshot(CrisisPresentationSnapshot newSnapshot)
        {
            if (ShouldNotify(_currentSnapshot, newSnapshot))
            {
                _currentSnapshot = newSnapshot;
                OnCrisisChanged?.Invoke(_currentSnapshot);
            }
        }

        private bool ShouldNotify(CrisisPresentationSnapshot oldState, CrisisPresentationSnapshot newState)
        {
            if (oldState.IsActive != newState.IsActive) return true;
            if (oldState.Severity != newState.Severity) return true;
            if (oldState.HeaderText != newState.HeaderText) return true;
            if (oldState.CrisisId != newState.CrisisId) return true;
            return false;
        }
    }
}
