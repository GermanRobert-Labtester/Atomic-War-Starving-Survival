// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Shelter
{
    public enum SecurityLevel
    {
        Open = 0,
        Restricted = 1,
        Locked = 2,
        HighSecurity = 3,
        Critical = 4
    }

    public enum ClearanceLevel
    {
        None = 0,
        Basic = 1,
        Restricted = 2,
        HighSecurity = 3,
        AllAccess = 4
    }

    public enum DoorLockState
    {
        Unlocked = 0,
        Locked = 1,
        Sealed = 2
    }

    public enum SecurityAlarmState
    {
        Normal = 0,
        Alert = 1,
        Breach = 2,
        Lockdown = 3
    }

    [Serializable]
    public sealed class SecurityZone
    {
        public string ZoneId { get; set; } = string.Empty;
        public string ZoneName { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public SecurityLevel Level { get; set; } = SecurityLevel.Open;
        public DoorLockState LockState { get; set; } = DoorLockState.Unlocked;
        public SecurityAlarmState AlarmState { get; set; } = SecurityAlarmState.Normal;
        public List<string> ExplicitAuthorizedSurvivors { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class SurvivorClearance
    {
        public string SurvivorId { get; set; } = string.Empty;
        public ClearanceLevel Level { get; set; } = ClearanceLevel.None;
        public int GrantedDay { get; set; } = 1;
        public string GrantedBy { get; set; } = "leader";
        public string Reason { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SecurityBreach
    {
        public string BreachId { get; set; } = string.Empty;
        public string ZoneId { get; set; } = string.Empty;
        public string IntruderId { get; set; } = string.Empty;
        public int DetectedDay { get; set; } = 1;
        public string BreachType { get; set; } = "unauthorized_entry";
        public bool IsResolved { get; set; } = false;
        public string ResolutionNotes { get; set; } = string.Empty;
    }

    public sealed class AccessAttemptResult
    {
        public bool IsGranted { get; set; }
        public string OutcomeMessage { get; set; } = string.Empty;
        public bool TriggeredAlarm { get; set; }
    }

    [Serializable]
    public sealed class ShelterSecurityState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public bool ShelterInLockdown { get; set; } = false;
        public List<SecurityZone> Zones { get; set; } = new List<SecurityZone>();
        public List<SurvivorClearance> Clearances { get; set; } = new List<SurvivorClearance>();
        public List<SecurityBreach> Breaches { get; set; } = new List<SecurityBreach>();
    }

    /// <summary>
    /// Plan 209 — Shelter Security & Access Control System.
    /// Manages internal zone security levels, survivor clearance tiers,
    /// door lock states (unlocked/locked/sealed), lockdown protocol, and security breaches.
    /// </summary>
    public sealed class ShelterSecuritySystem
    {
        private readonly ShelterSecurityState _state;

        public event Action<string, string, string>? OnAccessDenied;
        public event Action<SecurityBreach>? OnSecurityBreachDetected;
        public event Action<bool>? OnLockdownToggled;

        public bool IsInLockdown => _state.ShelterInLockdown;
        public int ZoneCount => _state.Zones.Count;
        public int ActiveBreachCount => _state.Breaches.Count(b => !b.IsResolved);

        public ShelterSecuritySystem(ShelterSecurityState? state = null)
        {
            _state = state ?? new ShelterSecurityState();
        }

        public SecurityZone ConfigureZone(
            string zoneId,
            string roomId,
            string zoneName,
            SecurityLevel level,
            DoorLockState lockState = DoorLockState.Unlocked)
        {
            if (string.IsNullOrWhiteSpace(zoneId)) throw new ArgumentNullException(nameof(zoneId));

            var zone = _state.Zones.FirstOrDefault(z => string.Equals(z.ZoneId, zoneId, StringComparison.OrdinalIgnoreCase));
            if (zone == null)
            {
                zone = new SecurityZone
                {
                    ZoneId = zoneId.Trim(),
                    RoomId = roomId ?? string.Empty,
                    ZoneName = string.IsNullOrWhiteSpace(zoneName) ? zoneId : zoneName.Trim(),
                    Level = level,
                    LockState = lockState,
                    AlarmState = SecurityAlarmState.Normal
                };
                _state.Zones.Add(zone);
            }
            else
            {
                zone.RoomId = roomId ?? zone.RoomId;
                zone.ZoneName = string.IsNullOrWhiteSpace(zoneName) ? zone.ZoneName : zoneName.Trim();
                zone.Level = level;
                zone.LockState = lockState;
            }

            return zone;
        }

        public SurvivorClearance GrantClearance(
            string survivorId,
            ClearanceLevel level,
            int day = 1,
            string grantedBy = "leader",
            string reason = "Duty assignment")
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));

            var cl = _state.Clearances.FirstOrDefault(c => string.Equals(c.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            if (cl == null)
            {
                cl = new SurvivorClearance
                {
                    SurvivorId = survivorId.Trim(),
                    Level = level,
                    GrantedDay = day,
                    GrantedBy = grantedBy ?? "leader",
                    Reason = reason ?? string.Empty
                };
                _state.Clearances.Add(cl);
            }
            else
            {
                cl.Level = level;
                cl.GrantedDay = day;
                cl.GrantedBy = grantedBy ?? cl.GrantedBy;
                cl.Reason = reason ?? cl.Reason;
            }

            return cl;
        }

        public ClearanceLevel GetClearance(string survivorId)
        {
            var cl = _state.Clearances.FirstOrDefault(c => string.Equals(c.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            return cl?.Level ?? ClearanceLevel.None;
        }

        public bool SetDoorLockState(string zoneId, DoorLockState lockState)
        {
            var zone = _state.Zones.FirstOrDefault(z => string.Equals(z.ZoneId, zoneId, StringComparison.OrdinalIgnoreCase));
            if (zone == null) return false;

            zone.LockState = lockState;
            return true;
        }

        public void SetShelterLockdown(bool active, int currentDay)
        {
            _state.ShelterInLockdown = active;
            foreach (var zone in _state.Zones)
            {
                if (active)
                {
                    zone.LockState = DoorLockState.Sealed;
                    zone.AlarmState = SecurityAlarmState.Lockdown;
                }
                else
                {
                    zone.AlarmState = SecurityAlarmState.Normal;
                    if (zone.LockState == DoorLockState.Sealed)
                    {
                        zone.LockState = (zone.Level == SecurityLevel.Open) ? DoorLockState.Unlocked : DoorLockState.Locked;
                    }
                }
            }

            OnLockdownToggled?.Invoke(active);
        }

        public AccessAttemptResult RequestAccess(string survivorId, string zoneId, int currentDay)
        {
            var zone = _state.Zones.FirstOrDefault(z => string.Equals(z.ZoneId, zoneId, StringComparison.OrdinalIgnoreCase));
            if (zone == null)
            {
                return new AccessAttemptResult { IsGranted = false, OutcomeMessage = "Zone not found.", TriggeredAlarm = false };
            }

            var survivorClearance = GetClearance(survivorId);

            // 1. Shelter Lockdown check
            if (_state.ShelterInLockdown && survivorClearance != ClearanceLevel.AllAccess)
            {
                OnAccessDenied?.Invoke(survivorId, zoneId, "Shelter is under emergency lockdown.");
                return new AccessAttemptResult { IsGranted = false, OutcomeMessage = "Shelter lockdown in effect.", TriggeredAlarm = false };
            }

            // 2. Door sealed check
            if (zone.LockState == DoorLockState.Sealed && survivorClearance != ClearanceLevel.AllAccess)
            {
                OnAccessDenied?.Invoke(survivorId, zoneId, "Door is sealed shut.");
                return new AccessAttemptResult { IsGranted = false, OutcomeMessage = "Door sealed.", TriggeredAlarm = false };
            }

            // 3. Explicit whitelist bypass
            if (zone.ExplicitAuthorizedSurvivors.Any(s => string.Equals(s, survivorId, StringComparison.OrdinalIgnoreCase)))
            {
                return new AccessAttemptResult { IsGranted = true, OutcomeMessage = "Access granted via explicit authorization.", TriggeredAlarm = false };
            }

            // 4. Clearance vs Security level comparison
            bool hasRequiredClearance = survivorClearance switch
            {
                ClearanceLevel.AllAccess => true,
                ClearanceLevel.HighSecurity => zone.Level <= SecurityLevel.HighSecurity,
                ClearanceLevel.Restricted => zone.Level <= SecurityLevel.Locked,
                ClearanceLevel.Basic => zone.Level <= SecurityLevel.Restricted,
                ClearanceLevel.None => zone.Level == SecurityLevel.Open,
                _ => false
            };

            if (hasRequiredClearance)
            {
                return new AccessAttemptResult { IsGranted = true, OutcomeMessage = "Access granted.", TriggeredAlarm = false };
            }

            // Access denied
            bool triggerBreach = zone.Level >= SecurityLevel.Locked;
            if (triggerBreach)
            {
                zone.AlarmState = SecurityAlarmState.Breach;
                var breach = new SecurityBreach
                {
                    BreachId = $"br_{_state.NextSequence++}",
                    ZoneId = zoneId,
                    IntruderId = survivorId,
                    DetectedDay = currentDay,
                    BreachType = "unauthorized_entry_attempt",
                    IsResolved = false
                };
                _state.Breaches.Add(breach);
                OnSecurityBreachDetected?.Invoke(breach);
            }

            OnAccessDenied?.Invoke(survivorId, zoneId, $"Insufficient clearance ({survivorClearance} vs {zone.Level}).");
            return new AccessAttemptResult
            {
                IsGranted = false,
                OutcomeMessage = $"Access denied: requires {zone.Level} clearance.",
                TriggeredAlarm = triggerBreach
            };
        }

        public bool ResolveBreach(string breachId, string resolutionNotes = "Resolved by security team")
        {
            var breach = _state.Breaches.FirstOrDefault(b => string.Equals(b.BreachId, breachId, StringComparison.OrdinalIgnoreCase));
            if (breach == null) return false;

            breach.IsResolved = true;
            breach.ResolutionNotes = resolutionNotes;

            // Reset zone alarm if no other active breaches exist for this zone
            var zone = _state.Zones.FirstOrDefault(z => string.Equals(z.ZoneId, breach.ZoneId, StringComparison.OrdinalIgnoreCase));
            if (zone != null && !_state.Breaches.Any(b => !b.IsResolved && string.Equals(b.ZoneId, zone.ZoneId, StringComparison.OrdinalIgnoreCase)))
            {
                zone.AlarmState = SecurityAlarmState.Normal;
            }

            return true;
        }

        public IReadOnlyList<SecurityBreach> GetActiveBreaches()
        {
            return _state.Breaches.Where(b => !b.IsResolved).ToList();
        }

        public ShelterSecurityState CaptureState()
        {
            var state = new ShelterSecurityState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                ShelterInLockdown = _state.ShelterInLockdown,
                Zones = new List<SecurityZone>(_state.Zones.Count),
                Clearances = new List<SurvivorClearance>(_state.Clearances.Count),
                Breaches = new List<SecurityBreach>(_state.Breaches.Count)
            };

            foreach (var z in _state.Zones)
            {
                state.Zones.Add(new SecurityZone
                {
                    ZoneId = z.ZoneId,
                    ZoneName = z.ZoneName,
                    RoomId = z.RoomId,
                    Level = z.Level,
                    LockState = z.LockState,
                    AlarmState = z.AlarmState,
                    ExplicitAuthorizedSurvivors = new List<string>(z.ExplicitAuthorizedSurvivors)
                });
            }

            foreach (var c in _state.Clearances)
            {
                state.Clearances.Add(new SurvivorClearance
                {
                    SurvivorId = c.SurvivorId,
                    Level = c.Level,
                    GrantedDay = c.GrantedDay,
                    GrantedBy = c.GrantedBy,
                    Reason = c.Reason
                });
            }

            foreach (var b in _state.Breaches)
            {
                state.Breaches.Add(new SecurityBreach
                {
                    BreachId = b.BreachId,
                    ZoneId = b.ZoneId,
                    IntruderId = b.IntruderId,
                    DetectedDay = b.DetectedDay,
                    BreachType = b.BreachType,
                    IsResolved = b.IsResolved,
                    ResolutionNotes = b.ResolutionNotes
                });
            }

            return state;
        }

        public void RestoreState(ShelterSecurityState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.ShelterInLockdown = state.ShelterInLockdown;
            _state.Zones.Clear();
            _state.Clearances.Clear();
            _state.Breaches.Clear();

            if (state.Zones != null)
            {
                foreach (var z in state.Zones)
                {
                    _state.Zones.Add(new SecurityZone
                    {
                        ZoneId = z.ZoneId,
                        ZoneName = z.ZoneName,
                        RoomId = z.RoomId,
                        Level = z.Level,
                        LockState = z.LockState,
                        AlarmState = z.AlarmState,
                        ExplicitAuthorizedSurvivors = new List<string>(z.ExplicitAuthorizedSurvivors ?? Enumerable.Empty<string>())
                    });
                }
            }

            if (state.Clearances != null)
            {
                foreach (var c in state.Clearances)
                {
                    _state.Clearances.Add(new SurvivorClearance
                    {
                        SurvivorId = c.SurvivorId,
                        Level = c.Level,
                        GrantedDay = c.GrantedDay,
                        GrantedBy = c.GrantedBy,
                        Reason = c.Reason
                    });
                }
            }

            if (state.Breaches != null)
            {
                foreach (var b in state.Breaches)
                {
                    _state.Breaches.Add(new SecurityBreach
                    {
                        BreachId = b.BreachId,
                        ZoneId = b.ZoneId,
                        IntruderId = b.IntruderId,
                        DetectedDay = b.DetectedDay,
                        BreachType = b.BreachType,
                        IsResolved = b.IsResolved,
                        ResolutionNotes = b.ResolutionNotes
                    });
                }
            }
        }
    }
}
