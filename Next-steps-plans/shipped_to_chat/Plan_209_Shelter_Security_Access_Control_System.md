# Plan 209 — Shelter Security & Access Control System

## Goal

Create a shelter security and access control system where rooms and areas have security levels, survivors have clearance ratings, restricted areas require authorization, and security breaches are tracked and responded to. Currently `AirlockSecuritySystem.cs` (227 lines) handles visitor arrivals at the airlock, and `PowerGridSystem.cs` (489 lines) manages room power — but there is no internal security system, no room-level access control, no security clearance for survivors, no restricted areas, no security breaches, no internal surveillance. All survivors can access all rooms at all times. This plan adds internal security as a shelter management layer.

## Why

**Repository evidence:** Grep for `ShelterSecurity`, `AccessControl`, `RoomAccess`, `SecurityClearance`, `DoorLock`, `RoomPermission`, `RestrictedArea`, `SecurityBreach` in Core returns only 1 match: `FactionEventResults.cs:15` has a `ShelterSecurity` float field (not a system). `AirlockSecuritySystem.cs` (227 lines) handles external visitor decisions (Admit/Inspect/Quarantine/TurnAway/Defend) but not internal room access. No internal security system exists.

**What is missing:** No room-level access control. No security clearance for survivors. No restricted areas. No door locks. No security breaches. No internal surveillance. No authorization system. All survivors can access all rooms freely. The shelter has no internal security.

**Why existing plans don't solve it:** Plan 138 (shelter defense) covers external defense against raids. Plan 205 (noise discipline) covers acoustic stealth. Plan 186 (shelter maintenance) covers physical degradation. No plan addresses internal security/access control.

**Player value:** Creates strategic depth (control who goes where), adds realism (sensitive areas should be restricted), generates emergent stories (security breaches, unauthorized access), and makes shelter management more complete (power + air + water + security).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/AirlockSecuritySystem.cs` — external visitor security (227 lines)
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` — room power (489 lines)
- `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` — survivor registry
- `Assets/Ashfall.Core/DutyRosterSystem.cs` — duty assignments
- NEW: `Assets/Ashfall.Core/Shelter/ShelterSecuritySystem.cs`
- NEW: `Assets/StreamingAssets/Data/security_zones.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterSecuritySystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `SecurityZone` DTO: `zoneId`, `zoneName`, `roomId` (room this zone covers), `securityLevel` (open/restricted/locked/high_security/critical), `accessRequirements` (list of clearance levels needed), `authorizedSurvivors` (list of survivor_ids with access), `doorLockState` (unlocked/locked/sealed), `alarmState` (normal/alert/breach/lockdown), `lastAccessDay`, `accessLog` (list of access events)
3. Define `SecurityClearance` DTO: `clearanceId`, `survivorId`, `clearanceLevel` (none/basic/restricted/high_security/critical/all_access), `grantedBy` (survivor_id or authority), `grantedDay`, `expiresDay` (-1 if permanent), `isActive` bool, `reason` (why granted)
4. Define `AccessEvent` DTO: `eventId`, `eventType` (access_granted/access_denied/door_locked/alarm_triggered/breach_detected/lockdown_initiated/lockdown_lifted), `survivorId`, `zoneId`, `day`, `time` (hour), `outcome` (success/denied/escalated), `notes`
5. Define `SecurityBreach` DTO: `breachId`, `breachType` (unauthorized_access/forced_entry/alarm_triggered/lockdown_violation/sabotage_detected), `zoneId`, `intruderId` (survivor_id), `detectedDay`, `resolvedDay` (-1 if unresolved), `response` (security_dispatched/lockdown/alarms/ignored), `resolution` (apprehended/escaped/false_alarm/resolved)
6. Define `SecurityAlarm` DTO: `alarmId`, `alarmType` (motion_detected/door_forced/unauthorized_access/panic_button/security_breach), `zoneId`, `triggeredDay`, `severity` (low/medium/high/critical), `status` (active/acknowledged/resolved/false_alarm), `responders` (list of survivor_ids)
7. Define `ShelterSecurityState` DTO: list of security zones, list of survivor clearances, list of access events, list of active breaches, list of active alarms, security settings (default security level, alarm response time, lockdown protocol)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define security levels (5 levels):
   - **Open**: anyone can enter (common areas, bunks, mess hall)
   - **Restricted**: basic clearance needed (workshops, storage, greenhouse)
   - **Locked**: high clearance needed (armory, medical, comms)
   - **High Security**: critical clearance only (leadership quarters, vault, reactor)
   - **Critical**: all-access only (emergency shelters, self-destruct, safe room)
10. Define clearance levels (5 levels):
    - **None**: no restricted access (new arrivals, visitors)
    - **Basic**: restricted areas (established survivors)
    - **Restricted**: locked areas (trusted survivors, key personnel)
    - **High Security**: high-security areas (leadership, security chief, medic chief)
    - **All Access**: critical areas (shelter leader only, or emergency override)
11. Define access control mechanics:
    - Each room/zone has security level
    - Each survivor has clearance level
    - Access attempt: check clearance vs. zone security
    - Access granted: survivor enters
    - Access denied: event logged, alarm may trigger
    - Repeated denied access: security breach flagged
12. Define door lock mechanics:
    - Doors can be locked/unlocked/sealed
    - Locked: requires clearance + unlock command
    - Sealed: emergency lockdown, no entry/exit
    - Door state affects access
13. Define alarm mechanics:
    - Alarms triggered by: unauthorized access, forced entry, panic button
    - Alarm severity determines response
    - Alarms alert security personnel
    - Alarms can trigger lockdown
14. Define security breach mechanics:
    - Breach detected when unauthorized access attempted
    - Breach response: security dispatched, lockdown, alarms
    - Breach resolution: apprehend intruder, resolve situation
    - Breach logged
15. Define lockdown mechanics:
    - Lockdown: all doors sealed, no movement
    - Lockdown triggered by: critical breach, external threat, manual override
    - Lockdown affects all zones
    - Lockdown logged
16. Add deterministic seeding: security events use `ISeededRng`
17. Wire into `GameBootstrap`: `SetupShelterSecurity`, `TickShelterSecurity`, `SaveShelterSecurity`

## Main Task 2 — Implementation / Zones / Clearances / Access / Alarms / Breaches / UI

1. Implement security zones:
   - Each room assigned security level
   - Zones have authorized survivor lists
   - Zones have door lock states
   - Zones have alarm states
   - Zone configuration logged
2. Implement security clearances:
   - Each survivor has clearance level
   - Clearances granted by authority
   - Clearances can expire
   - Clearances can be revoked
   - Clearance changes logged
3. Implement access control:
   - Access attempt: check clearance vs. zone
   - Access granted/denied
   - Access events logged
   - Repeated denials: breach flagged
4. Implement door locks:
   - Doors can be locked/unlocked/sealed
   - Lock state affects access
   - Lock changes logged
5. Implement alarms:
   - Alarms triggered by security events
   - Alarm severity determines response
   - Alarms alert security personnel
   - Alarms logged
6. Implement security breaches:
   - Breach detected on unauthorized access
   - Breach response initiated
   - Breach resolution tracked
   - Breach logged
7. Implement lockdown:
   - Lockdown seals all doors
   - Lockdown triggered by critical events
   - Lockdown lifted by authority
   - Lockdown logged
8. Implement security UI:
   - Security panel: zone status, alarm state, breach status
   - Zone detail: security level, authorized survivors, door state
   - Clearance panel: survivor clearances, grant/revoke
   - Access log: recent access events
   - Alarm panel: active alarms, response status
   - Security map: show zones, security levels, alarm states
9. Create security events:
    - "The Breach" — security breach detected
    - "The Alarm" — alarm triggered
    - "The Lockdown" — lockdown initiated
    - "The Access" — access granted/denied
    - "The Clearance" — clearance granted/revoked
    - "The Intruder" — unauthorized access detected
    - "The Response" — security response dispatched
    - "The Resolution" — breach resolved
10. Add security quest hooks:
    - "The Guard" — maintain zero breaches for 100 days
    - "The Warden" — manage 20 security clearances
    - "The Detective" — investigate 5 security breaches
    - "The Locksmith" — install security in 15 rooms
    - "The Watchman" — respond to 10 alarms
    - "The Secure" — achieve maximum security for all zones
    - "The Crisis Manager" — successfully manage 3 lockdowns
11. Implement security tutorial: first access denial explains system
12. Add security tooltips: hover over zone shows security level
13. Create security zone definitions in data file
14. Implement security persistence: zones/clearances/events saved
15. Integrate with `AirlockSecuritySystem`: external + internal security coordinated

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `AirlockSecuritySystem`: coordinate external/internal security
2. Connect to `PowerGridSystem`: door locks require power
3. Integrate with `DutyRosterSystem`: security duty assignments
4. Connect to `LeadershipSystem`: leader has all-access clearance
5. Wire into `InterpersonalConflictSystem` (Plan 202): unauthorized access can trigger conflicts
6. Connect to `ShelterFireHazardSystem`: fire triggers alarms/lockdown
7. Implement old-save compatibility: existing saves get all zones open, all survivors basic clearance
8. Add deterministic seeding: security events use `ISeededRng`
9. Create exploit prevention: security is state-based, can't be gamed
10. Add tests: zones, clearances, access control, alarms, breaches, lockdown, save round-trip
11. Verify all security levels work correctly
12. Test edge cases: no security (current behavior), maximum security (complete lockdown)
13. Verify headless behavior: security processes correctly without UI
14. Add data-integrity-selftest: security validates against room/survivor catalogs
15. Create `--shelter-security-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-security-selftest
```

## Risk

**LOW** — Security is straightforward with clear inputs (zones, clearances) and outputs (access events, breaches). Risk of security feeling like bureaucratic overhead. Mitigation: make breaches consequential, show clear cause-effect, and ensure security feels like protection not restriction.

## Definition of Done

- `ShelterSecuritySystem.cs` exists with full `CaptureState/RestoreState`
- 5 security levels (open, restricted, locked, high security, critical)
- 5 clearance levels (none, basic, restricted, high security, all access)
- Access control mechanics (clearance check, grant/deny, logging)
- Door lock mechanics (locked, sealed, emergency)
- Alarm system (triggered by security events, severity-based response)
- Security breach detection and response
- Lockdown mechanics (seal all doors, emergency protocol)
- Security events and quest hooks
- Save/load round-trip tested
- Deterministic security events verified
- Old saves load with all zones open, basic clearance
- Security zone definitions in data authority
- UI security panel, zone detail, clearance panel, access log, alarm panel, security map
- Cross-system integration (airlock security, power grid, duty roster, leadership, conflicts, fire hazard)

## Follow-On Opportunities

- Security specialization (survivors become expert security officers)
- Security legacy (famous breaches remembered)
- Security quests (specific security goals)
- Security events (massive security failure, perfect security record)
- Security trading (trade security technology with other settlements)
