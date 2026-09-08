using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
#pragma warning disable CS8618

using Ashfall.Core.Shelter;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ShelterScheduleState
    {
        public string systemId = ShelterScheduleSystem.SystemId;
        public SchedulePhase currentPhase = SchedulePhase.Day;
        public bool curfewActive;
        public bool emergencyOverride;
        public float fatigueRecoveryModifier = 1f;
        public float lightingDemand = 0.5f;
        public List<SleepAssignment> assignments = new List<SleepAssignment>();
        public int lastTransitionDay = -1;
        public string activeScheduleId = "default";
    }

    [Serializable]
    public sealed class ScheduleDefinition
    {
        [JsonPropertyName("schedule_id")]
        public string schedule_id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string display_name { get; set; } = string.Empty;

        [JsonPropertyName("day_start_hour")]
        public float dayStartHour { get; set; } = 6f;

        [JsonPropertyName("day_end_hour")]
        public float dayEndHour { get; set; } = 22f;

        [JsonPropertyName("curfew_start_hour")]
        public float curfewStartHour { get; set; } = 22f;

        [JsonPropertyName("curfew_end_hour")]
        public float curfewEndHour { get; set; } = 6f;

        [JsonPropertyName("fatigue_recovery_modifier")]
        public float fatigueRecoveryModifier { get; set; } = 1f;

        [JsonPropertyName("lighting_demand_day")]
        public float lightingDemandDay { get; set; } = 0.5f;

        [JsonPropertyName("lighting_demand_night")]
        public float lightingDemandNight { get; set; } = 0.8f;

        [JsonPropertyName("lighting_demand_curfew")]
        public float lightingDemandCurfew { get; set; } = 0.3f;

        [JsonPropertyName("allow_emergency_override")]
        public bool allowEmergencyOverride { get; set; } = true;

        [JsonPropertyName("shift_pattern")]
        public string shiftPattern { get; set; } = "single_shift";

        [JsonPropertyName("trigger_condition")]
        public string triggerCondition { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SleepAssignment
    {
        public string survivorId = string.Empty;
        public string bedId = string.Empty;
        public bool isAssigned;
        public bool isCompliant;
        public float restQuality = 1f;
    }

    public enum SchedulePhase { Day, Night, Curfew, Emergency }

    public sealed class ShelterScheduleSystem
    {
        public const string SystemId = "shelter_schedule";
        private ShelterScheduleState _state = new ShelterScheduleState();
        private readonly Dictionary<string, ScheduleDefinition> _catalog = new Dictionary<string, ScheduleDefinition>(StringComparer.Ordinal);
        private readonly ILog _log;
        private readonly PowerGridSystem _powerGrid;
        private string _activeScheduleId = "default";

        public ShelterScheduleState State => _state;
        public SchedulePhase CurrentPhase => _state.currentPhase;
        public bool IsCurfewActive => _state.curfewActive && !_state.emergencyOverride;
        public bool IsEmergencyOverride => _state.emergencyOverride;
        public float FatigueRecoveryModifier => _state.fatigueRecoveryModifier;
        public float LightingDemand => _state.lightingDemand;
        public string ActiveScheduleId => _activeScheduleId;

        public IReadOnlyCollection<ScheduleDefinition> GetAllSchedules() => _catalog.Values;

        public ScheduleDefinition? GetSchedule(string scheduleId) =>
            _catalog.TryGetValue(scheduleId, out var def) ? def : null;

        public bool TryActivateScheduleByTrigger(string triggerCondition)
        {
            if (string.IsNullOrEmpty(triggerCondition)) return false;
            foreach (var kvp in _catalog)
            {
                if (string.Equals(kvp.Value.triggerCondition, triggerCondition, StringComparison.OrdinalIgnoreCase))
                {
                    var res = SetSchedule(kvp.Key);
                    return res.IsSuccess;
                }
            }
            return false;
        }

        public event Action<SchedulePhase> OnPhaseChanged;
        public event Action OnScheduleChanged;

        public ShelterScheduleSystem(PowerGridSystem powerGrid, ILog? log = null)
        {
            _powerGrid = powerGrid ?? throw new ArgumentNullException(nameof(powerGrid));
            _log = log ?? NullLog.Instance;
            _catalog["default"] = new ScheduleDefinition
            {
                schedule_id = "default",
                display_name = "Default Schedule",
                allowEmergencyOverride = true,
                fatigueRecoveryModifier = 1f,
                lightingDemandDay = 0.5f,
                lightingDemandNight = 0.8f,
                lightingDemandCurfew = 0.3f
            };
        }

        public void LoadCatalog(List<ScheduleDefinition> definitions)
        {
            if (definitions == null) return;
            _catalog.Clear();
            foreach (var def in definitions)
                if (!string.IsNullOrEmpty(def.schedule_id))
                    _catalog[def.schedule_id] = def;
        }

        public ActionResult SetSchedule(string scheduleId)
        {
            if (!_catalog.TryGetValue(scheduleId, out var def))
                return ActionResult.Failed("unknown_schedule", "schedule.unknown");

            _activeScheduleId = scheduleId;
            _state.activeScheduleId = scheduleId;
            _log.Info($"[Schedule] switched to {def.display_name}");
            OnScheduleChanged?.Invoke();
            return ActionResult.Success("schedule.set");
        }

        public ActionResult SetCurfew(bool active)
        {
            _state.curfewActive = active;
            UpdatePhase();
            OnScheduleChanged?.Invoke();
            return ActionResult.Success("schedule.curfew_set",
                new Dictionary<string, double> { { "curfew", active ? 1 : 0 } });
        }

        public ActionResult SetEmergencyOverride(bool active)
        {
            if (!_catalog.TryGetValue(_activeScheduleId, out var def))
                return ActionResult.Failed("no_schedule", "schedule.no_schedule");

            if (active && !def.allowEmergencyOverride)
                return ActionResult.Blocked("not_allowed", "schedule.emergency_not_allowed");

            _state.emergencyOverride = active;
            UpdatePhase();
            OnScheduleChanged?.Invoke();
            return ActionResult.Success("schedule.emergency_set",
                new Dictionary<string, double> { { "emergency", active ? 1 : 0 } });
        }

        public ActionResult AssignBed(string survivorId, string bedId)
        {
            var existing = _state.assignments.Find(a => a.survivorId == survivorId);
            if (existing != null)
            {
                existing.bedId = bedId;
                existing.isAssigned = true;
            }
            else
            {
                _state.assignments.Add(new SleepAssignment
                {
                    survivorId = survivorId, bedId = bedId, isAssigned = true
                });
            }
            OnScheduleChanged?.Invoke();
            return ActionResult.Success("schedule.bed_assigned");
        }

        public ActionResult UnassignBed(string survivorId)
        {
            var existing = _state.assignments.Find(a => a.survivorId == survivorId);
            if (existing != null)
            {
                existing.isAssigned = false;
                existing.bedId = string.Empty;
            }
            OnScheduleChanged?.Invoke();
            return ActionResult.Success("schedule.bed_unassigned");
        }

        public void TickDay(int day)
        {
            if (_state.lastTransitionDay != day)
            {
                _state.lastTransitionDay = day;
                UpdatePhase();
            }

            // Check compliance
            foreach (var assignment in _state.assignments)
            {
                if (!assignment.isAssigned) continue;
                assignment.isCompliant = _state.curfewActive;
                // Rest quality modifier
                assignment.restQuality = _state.emergencyOverride ? 0.5f : (_state.curfewActive ? 1.2f : 1f);
            }

            // Fatigue recovery modifier
            if (_catalog.TryGetValue(_activeScheduleId, out var def))
            {
                // Bug-07: the schedule's modifier applies across all phases;
                // emergency override is the only thing that overrides it.
                _state.fatigueRecoveryModifier = _state.emergencyOverride ? 0.5f : def.fatigueRecoveryModifier;
                _state.lightingDemand = _state.emergencyOverride ? def.lightingDemandCurfew * 0.5f :
                    (_state.curfewActive ? def.lightingDemandCurfew : def.lightingDemandDay);

                // Bug-15: brownout halves the lighting demand *after* the
                // base setting is assigned. Previously this multiplicative
                // step ran first and was then unconditionally overwritten by
                // the assignment above, so a brownout had no effect on the
                // published lightingDemand value.
                if (_powerGrid.IsBrownout)
                {
                    _state.lightingDemand *= 0.5f;
                }
            }
        }

        public ScheduleDefinition? GetActiveSchedule()
        {
            _catalog.TryGetValue(_activeScheduleId, out var def);
            return def;
        }

        public bool IsSleepEligible(string survivorId)
        {
            var assignment = _state.assignments.Find(a => a.survivorId == survivorId);
            return assignment != null && assignment.isAssigned;
        }

        private void UpdatePhase()
        {
            SchedulePhase newPhase;
            if (_state.emergencyOverride)
                newPhase = SchedulePhase.Emergency;
            else if (_state.curfewActive)
                newPhase = SchedulePhase.Curfew;
            else
                newPhase = SchedulePhase.Day;

            if (newPhase != _state.currentPhase)
            {
                _state.currentPhase = newPhase;
                OnPhaseChanged?.Invoke(newPhase);
            }
        }

        public ShelterScheduleState CaptureState()
        {
            _state.activeScheduleId = _activeScheduleId;
            return CloneState(_state);
        }

        public void RestoreState(ShelterScheduleState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            _activeScheduleId = string.IsNullOrEmpty(_state.activeScheduleId) ? "default" : _state.activeScheduleId;
        }

        private static ShelterScheduleState CloneState(ShelterScheduleState src)
        {
            if (src == null) return new ShelterScheduleState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ShelterScheduleState>(json) ?? new ShelterScheduleState();
        }
    }
}
