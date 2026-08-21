using System;
using System.Collections.Generic;

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
    }

    [Serializable]
    public sealed class ScheduleDefinition
    {
        public string schedule_id = string.Empty;
        public string display_name = string.Empty;
        public float dayStartHour = 6f;
        public float dayEndHour = 22f;
        public float curfewStartHour = 22f;
        public float curfewEndHour = 6f;
        public float fatigueRecoveryModifier = 1f;
        public float lightingDemandDay = 0.5f;
        public float lightingDemandNight = 0.8f;
        public float lightingDemandCurfew = 0.3f;
        public bool allowEmergencyOverride = true;
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

        public event Action<SchedulePhase> OnPhaseChanged;
        public event Action OnScheduleChanged;

        public ShelterScheduleSystem(PowerGridSystem powerGrid, ILog log = null)
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

        public ShelterScheduleState CaptureState() => _state;
        public void RestoreState(ShelterScheduleState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnScheduleChanged?.Invoke();
        }
    }
}
