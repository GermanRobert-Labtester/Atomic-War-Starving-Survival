// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : ShelterSecurityHostSession
// Core System  : Ashfall.Core.Shelter.ShelterSecuritySystem
// Host Caller  : Main.ShelterSecurity
// Purpose      : Plan 138 — Coordinates shelter security zones, clearances,
//                access control, door locks, lockdown protocols, and breaches.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class ShelterSecurityHostSession
    {
        private readonly ShelterSecuritySystem _system;

        public ShelterSecuritySystem System => _system;

        public event Action? StateChanged;

        public bool IsInLockdown => _system.IsInLockdown;
        public int ZoneCount => _system.ZoneCount;
        public int ActiveBreachCount => _system.ActiveBreachCount;
        public IReadOnlyList<SecurityZone> Zones => _system.Zones;
        public IReadOnlyList<SurvivorClearance> Clearances => _system.Clearances;
        public IReadOnlyList<SecurityBreach> Breaches => _system.Breaches;

        public ShelterSecurityHostSession(ShelterSecuritySystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            // Seed canonical shelter zones if fresh
            if (_system.ZoneCount == 0)
            {
                _system.ConfigureZone("zone_armory", "room_armory", "Armory", SecurityLevel.HighSecurity, DoorLockState.Locked);
                _system.ConfigureZone("zone_vault", "room_vault", "Main Vault", SecurityLevel.Critical, DoorLockState.Locked);
                _system.ConfigureZone("zone_medical", "room_medical_ward", "Medical Ward", SecurityLevel.Restricted, DoorLockState.Unlocked);
                _system.ConfigureZone("zone_command", "room_command", "Command Center", SecurityLevel.HighSecurity, DoorLockState.Locked);
                _system.ConfigureZone("zone_hydro", "room_hydroponics", "Hydroponics Bay", SecurityLevel.Restricted, DoorLockState.Unlocked);
                _system.ConfigureZone("zone_quarters", "room_quarters", "Living Quarters", SecurityLevel.Open, DoorLockState.Unlocked);
            }

            _system.OnAccessDenied += (_, _, _) => StateChanged?.Invoke();
            _system.OnSecurityBreachDetected += _ => StateChanged?.Invoke();
            _system.OnLockdownToggled += _ => StateChanged?.Invoke();
            _system.OnStateChanged += () => StateChanged?.Invoke();
        }

        public SecurityZone ConfigureZone(string zoneId, string roomId, string zoneName, SecurityLevel level, DoorLockState lockState = DoorLockState.Unlocked)
        {
            var zone = _system.ConfigureZone(zoneId, roomId, zoneName, level, lockState);
            StateChanged?.Invoke();
            return zone;
        }

        public SurvivorClearance GrantClearance(string survivorId, ClearanceLevel level, int day = 1, string grantedBy = "leader", string reason = "Duty assignment")
        {
            var cl = _system.GrantClearance(survivorId, level, day, grantedBy, reason);
            StateChanged?.Invoke();
            return cl;
        }

        public ClearanceLevel GetClearance(string survivorId)
        {
            return _system.GetClearance(survivorId);
        }

        public bool SetDoorLockState(string zoneId, DoorLockState lockState)
        {
            bool ok = _system.SetDoorLockState(zoneId, lockState);
            if (ok) StateChanged?.Invoke();
            return ok;
        }

        public void SetShelterLockdown(bool active, int currentDay)
        {
            _system.SetShelterLockdown(active, currentDay);
            StateChanged?.Invoke();
        }

        public AccessAttemptResult RequestAccess(string survivorId, string zoneId, int currentDay)
        {
            var res = _system.RequestAccess(survivorId, zoneId, currentDay);
            StateChanged?.Invoke();
            return res;
        }

        public bool ResolveBreach(string breachId, string resolutionNotes = "Resolved by security team")
        {
            bool ok = _system.ResolveBreach(breachId, resolutionNotes);
            if (ok) StateChanged?.Invoke();
            return ok;
        }

        public IReadOnlyList<SecurityBreach> GetActiveBreaches()
        {
            return _system.GetActiveBreaches();
        }

        public ShelterSecurityState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(ShelterSecurityState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
