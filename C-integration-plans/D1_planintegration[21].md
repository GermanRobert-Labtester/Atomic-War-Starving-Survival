# D1 Flagship Integration Plan [21]
## Plan 209 — Shelter Security & Access Control System

> **Canonical filename:** `D1_planintegration[21].md`
>
> **Previous:** `D1_planintegration[20].md`
>
> **Next:** `D1_planintegration[22].md`
>
> **Purpose:** Add one authoritative internal shelter-security layer where rooms and controlled areas have access
> policies, survivors and visitors have scoped authorization, powered locks and emergency seals enforce those
> policies, unauthorized activity creates auditable security events, alarms coordinate responders, and lockdowns
> operate as explicit emergency states—without turning security into a second movement engine, a second survivor
> authority, or a bureaucratic click-tax.
>
> **Primary source:** Plan 209 — Shelter Security & Access Control System.
>
> **Core repository problem:** `AirlockSecuritySystem.cs` already covers external visitor decisions and
> `PowerGridSystem.cs` already models room power, but there is no internal access-control authority. All shelter
> occupants effectively have universal access, rooms cannot express restricted policy, doors do not enforce
> clearance, unauthorized entry is not represented as a persistent security event, and no internal alarm or
> lockdown state coordinates shelter response.
>
> **Implementation posture:** policy-driven, room/topology aware, deterministic, power-aware, event-sourced for
> significant incidents, integrated with scheduler/duty/leadership/fire/conflict systems, migration-safe, and
> biased toward sensible defaults and role-based automation so the feature adds strategic protection rather than
> paperwork.
>
> **Critical guardrail:** `ShelterSecuritySystem` decides whether an access request is authorized under shelter
> policy. It must not become the canonical movement/pathfinding system. Movement systems ask for an authorization
> decision before crossing controlled boundaries; the security system never teleports survivors, simulates every
> footstep, or owns room occupancy.
---

## 1. Source Problem Statement

The source identifies a missing internal-security domain:

- no `ShelterSecurity`, `AccessControl`, `RoomAccess`, `SecurityClearance`, `DoorLock`, `RoomPermission`,
  `RestrictedArea`, or `SecurityBreach` system exists;
- the only `ShelterSecurity` match is a float field in `FactionEventResults.cs`, not a system;
- `AirlockSecuritySystem.cs` handles external admission decisions only;
- `PowerGridSystem.cs` manages room power but not access authorization;
- no room-level clearance policy exists;
- no internal door-lock authority exists;
- no unauthorized-access log exists;
- no breach/alarm lifecycle exists;
- no lockdown protocol exists.

The target architecture is:

```text
room / door / role / emergency state
                 ↓
         ShelterSecuritySystem
      ┌──────────┼─────────────┐
      ↓          ↓             ↓
 zone policy  clearances   lock/alarm state
      └──────────┼─────────────┘
                 ↓
          Access Decision API
                 ↓
       canonical movement/action
                 ↓
     significant security events
      ├─ denied access
      ├─ forced entry
      ├─ alarm
      ├─ breach
      └─ lockdown
                 ↓
       response / duty / conflict
```

The system is therefore an authorization and incident-coordination layer, not a replacement for shelter topology,
movement, leadership, conflict, or defense.
---

## 2. Flagship Success Criteria

Implementation is complete only when all of the following are true:

1. `ShelterSecuritySystem.cs` exists with schema-versioned capture/restore.
2. Security zones reference stable room/area IDs from the canonical shelter topology.
3. Door security references real door/boundary IDs where the topology supports doors.
4. Security levels are data-backed and ordered explicitly.
5. Clearance levels are data-backed and ordered explicitly.
6. Role-based and survivor-specific authorization can coexist without duplicating survivor identity.
7. Clearance grants have stable IDs, provenance, grant/revoke dates, and optional expiry.
8. Expired clearance is rejected deterministically.
9. Access checks are pure/deterministic for the same policy, actor, zone, door, and emergency state.
10. Access decisions return structured reasons, not just true/false.
11. Movement/action systems remain authoritative over actual traversal/interaction.
12. Door locks depend on real power where electronic locking is used.
13. Mechanical/fail-safe/fail-secure behavior is explicit per lock profile.
14. Loss of power cannot silently trap the whole shelter unless the configured lock type intentionally fails secure
    and emergency egress rules permit it.
15. Emergency egress is never accidentally blocked by a generic lockdown.
16. Fire/emergency systems can override normal security policy through typed emergency-access rules.
17. Medical responders can reach patients under explicit emergency override policy.
18. Leadership/all-access status is derived from real role/authority, not hardcoded survivor names.
19. DutyRoster can identify security responders without security system creating a second duty scheduler.
20. Airlock admission and internal access share identity/authorization context without merging external and internal
    state machines.
21. Repeated denied access does not automatically become a hostile breach unless intent/forced-action thresholds are
    met.
22. Benign pathfinding attempts do not flood the security log.
23. One physical unauthorized attempt creates at most one access-denied incident.
24. Forced entry has an explicit action/event path.
25. Breaches have lifecycle states and stable IDs.
26. Alarms have lifecycle states and stable IDs.
27. Lockdowns have explicit scope, reason, initiator, start/end, override policy, and stable ID.
28. Lockdown does not mean "seal every door with no exit" by default.
29. Safe rooms/critical areas can have stronger rules without making ordinary rooms tedious.
30. Old saves default to permissive/current-behavior access and do not suddenly lock residents out of their shelter.
31. Old-save migration does not fabricate historical breaches or clearances.
32. Security UI reads projection DTOs only.
33. Routine access is not logged forever; significant access history is bounded.
34. Headless simulations behave identically to UI-driven runs.
35. `--shelter-security-selftest` validates policies, clearances, door power, alarms, breaches, lockdowns,
    emergency override, migration, idempotency, and save round-trip.
---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/shelter_security/SHELTER_SECURITY_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/AirlockSecuritySystem.cs`
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs`
- `Assets/Ashfall.Core/DutyRosterSystem.cs`
- shelter room/topology graph
- door/boundary/opening state
- movement/pathfinding/navigation APIs
- leadership/authority system
- role/job system
- shelter fire/emergency systems
- alarm/notification systems
- InterpersonalConflictSystem from Plan 202 if implemented
- sabotage/espionage systems from Plan 153 if implemented
- visitor/faction identity representation
- inventory/armory/medical/comms/vault room definitions
- safe-room/emergency-shelter content
- save schema/migrations
- deterministic RNG
- UI shelter map / room detail
- journal/archive/quest/achievement consumers

Build an authority matrix:

| Concern | Canonical owner | Security-system role |
|---|---|---|
| survivor identity | SurvivorCatalog | resolve actor |
| role/leadership | Leadership/roles | derive authorization |
| room topology | Shelter | map zone/boundary |
| movement | navigation/scheduler | request authorization |
| door physical state | shelter/door system | request lock state / read state |
| door power | PowerGrid | read powered capability |
| duty assignment | DutyRoster | identify responders |
| fire evacuation | FireHazard | emergency override context |
| external visitor admission | AirlockSecurity | pass admitted identity/context |
| interpersonal conflict | Conflict | consume/emit incident context |
| sabotage | espionage/sabotage | consume breach facts |
| raid defense | ShelterDefense | external threat authority |

Do not begin by adding survivor movement loops inside `ShelterSecuritySystem`.
---

## 4. Scope Boundary

### In scope

- security-zone policies;
- clearance definitions/grants;
- role-based access;
- survivor-specific exceptions;
- visitor/internal-access context;
- door lock/security state;
- powered lock behavior;
- emergency egress/override;
- alarms;
- breaches;
- lockdown;
- responder dispatch requests;
- significant access audit;
- UI/map;
- persistence/migration;
- deterministic tests.

### Explicitly out of scope

- full stealth AI;
- prisoner simulation;
- guard combat AI;
- interrogation;
- police/legal system;
- surveillance-camera rendering;
- facial recognition;
- biometric simulation;
- global faction espionage replacement;
- replacing movement/pathfinding;
- replacing external airlock admission;
- replacing shelter defense;
- automatic punishment system;
- punitive bureaucracy for every room entry.

The initial system should secure sensitive areas while keeping normal shelter life frictionless.

---
## 5. Security Zone Definition vs Runtime State
- Separate catalog policy from run-local mutable state.
- `SecurityZoneDefinition` should contain zone ID, room/area references, baseline security level, allowed role/clearance policies, lock profile, alarm profile, and emergency override profile.
- `SecurityZoneState` should contain temporary overrides, current alert/lockdown state, last significant incident, and system-owned runtime exceptions.
- Do not persist room names or static access requirements into every save.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 6. Zone Granularity
- Prefer one zone per room or meaningful secured area where current shelter topology supports it.
- Do not create multiple overlapping zones without an explicit precedence rule.
- Common areas may remain outside explicit secured-zone state and use the default open policy.
- Critical subareas inside one room require real interaction/boundary support before being modeled separately.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 7. Security Level Taxonomy
- Retain source levels: open, restricted, locked, high_security, critical.
- Order them through data/enum rank rather than string comparison.
- Security level determines baseline authorization rigor, not necessarily physical lock state.
- A high-security zone can be temporarily unlocked while still requiring authorization for entry.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 8. Clearance Taxonomy
- Retain source intent: none, basic, restricted, high_security, all_access.
- Resolve the source mismatch where it calls five levels but earlier names `critical` in the DTO example—use one canonical ordered set.
- Do not create both `critical` clearance and `all_access` unless they have distinct gameplay semantics.
- Document the mapping in data and migration.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 9. Clearance vs Security Level Mapping
- Define an explicit matrix rather than relying on equal string names.
- Example: open→none, restricted→basic, locked→restricted, high_security→high_security, critical→all_access.
- Zone-specific role requirements may be stricter than rank alone.
- UI should explain the unmet condition.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 10. Role-Based Authorization
- Use roles to eliminate clearance micromanagement.
- Examples: medic role may access medical storage, security role may access armory while on duty, engineer role may access reactor/power rooms.
- Role authorization comes from canonical DutyRoster/Leadership/role state.
- Do not grant permanent survivor-specific clearance for every routine job if a role policy can express it.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 11. Survivor-Specific Clearance Grants
- Use explicit grants for exceptions, temporary assignments, trusted personnel, investigations, or special access.
- Fields: clearanceGrantId, survivorId, scope, level, grantedBy, grantedDay, expiresDay, reasonCode, sourceEventId.
- Free-text reason is presentation only.
- Grants are additive subject to revocation/emergency policy.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 12. Scoped Clearance
- A grant may be shelter-wide level, specific zone, specific capability, or temporary emergency access.
- Prefer narrow scope when possible.
- Do not force all exceptional access into a global rank escalation.
- This reduces the need to give everyone all-access.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 13. Clearance Expiry
- Expiry is evaluated from campaign time.
- No per-frame polling; maintain next-expiry index or evaluate on access/periodic checkpoint.
- Expired grants remain historical but inactive.
- Save/load at the expiry boundary is deterministic.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 14. Clearance Revocation
- Revocation records revoker, day, source/reason, and prior grant ID.
- Do not delete historical grant provenance.
- Revoked access stops immediately for future checks.
- Active movement already inside the zone follows exit/escort policy rather than teleportation.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 15. Leadership Authorization
- LeadershipSystem exposes the current shelter leader/authorized authority role.
- Do not hardcode all-access by survivor ID.
- Leadership succession automatically updates role-derived authority.
- Emergency override authority can be a separate capability.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 16. DutyRoster Integration
- Security responder access can derive from active security duty assignment.
- Off-duty security staff can retain only the baseline access intended by policy.
- DutyRoster remains scheduler/assignment authority.
- Security system queries role state; it does not schedule shifts.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 17. Visitor Identity
- AirlockSecuritySystem decides whether a visitor is admitted.
- On admission, provide an internal actor identity/context with visitor/escort restrictions.
- Do not automatically give admitted visitors `basic` survivor clearance.
- Visitor access should default to escort/open zones only.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 18. New Arrival Defaults
- Source suggests None for new arrivals/visitors and Basic for established survivors.
- Make this policy-driven.
- Old residents from migrated saves need permissive compatibility; newly recruited survivors can receive role-based defaults.
- No manual clearance assignment should be required for every routine recruit.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 19. Access Request Contract
- Create a pure API such as `EvaluateAccess(AccessRequest request)`.
- Request includes actor, source room/zone, target room/zone, boundary/door ID, action kind, time, emergency context, escort context, and requested capability.
- Return `AccessDecision` with allowed/denied, reason codes, required credentials, lock action requirement, and alarm/breach consequences.
- Evaluation does not itself move the actor.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 20. Access Action Kinds
- Differentiate enter_zone, exit_zone, open_door, operate_equipment, access_storage, override_lock, escort_entry, and emergency_egress where useful.
- A survivor may be allowed to enter a reactor control room but not operate a critical console without a separate capability.
- Do not overload room entry to secure every interaction.
- Keep v1 action set aligned with existing interactable systems.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 21. Access Decision Reasons
- Use structured codes: open_access, clearance_sufficient, role_authorized, explicit_grant, escorted, emergency_override, expired_clearance, insufficient_clearance, locked_boundary, sealed_lockdown, power_unavailable, revoked, visitor_restricted.
- UI can render localized explanations.
- Logs record reason code, not only free text.
- Tests assert exact reason where important.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 22. Movement Integration
- Navigation/path scheduler calls authorization before crossing a secured boundary.
- Denied access should cause route rejection/replanning, not repeated attempts every simulation tick.
- Movement system owns actual location changes.
- Security system never advances position.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 23. Pathfinding Cost/Reachability
- Secured doors can appear unavailable to unauthorized actors.
- Authorized locked doors may be traversable with an unlock/open action cost.
- Emergency sealed boundaries may block path entirely except egress/override routes.
- Do not make pathfinder trigger security events merely while evaluating hypothetical paths.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 24. No Log on Path Planning
- Only a committed physical access attempt creates an access event.
- Pathfinding queries are side-effect free.
- This prevents thousands of denied-access logs from AI route exploration.
- Separate `CanAccess` query from `AttemptAccess` command.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 25. Access Attempt Command
- `AttemptAccess` receives a committed attempt ID.
- It revalidates current policy/lock/power and returns a final decision.
- One attempt ID can produce at most one logged denial/grant incident according to logging policy.
- Movement proceeds only after success.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 26. Routine Access Logging Policy
- Do not retain every successful entry into mess hall/workshop forever.
- Log high-security/critical grants, denied access, overrides, forced entries, clearance changes, alarms, breaches, lockdowns, and configured sensitive-zone access.
- Routine access can be aggregated in short rolling telemetry if needed.
- This is essential for save size and UI usability.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 27. Access Log Retention
- Keep recent significant events plus permanent major incidents.
- Example: rolling 200–500 significant access events, with breaches/lockdowns retained in separate history.
- Do not persist millions of door crossings.
- Retention limit belongs in config.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 28. Door Identity
- Door lock/security state should key to canonical door/boundary IDs.
- If the shelter currently has room access without explicit door objects, represent a controlled boundary keyed to room-edge ID.
- Do not invent arbitrary door IDs detached from topology.
- Stable IDs are save contracts.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 29. Door Lock Profiles
- Distinguish mechanical lock, electronic fail-safe, electronic fail-secure, sealed blast door, and unlocked/no lock where supported.
- Power dependency belongs to the lock profile.
- Security level does not itself imply a specific lock technology.
- Use construction/upgrades to change lock profile.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 30. Door Lock State
- Runtime state can be unlocked, locked, sealed, damaged/jammed where the door system supports it.
- Sealed is reserved for explicit emergency/containment states.
- Do not interpret `locked` as physically impassable to all—authorized users may unlock.
- State transitions use door/security commands.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 31. Electronic Lock Power
- Read circuit/room power from PowerGridSystem.
- Define fail-safe/fail-secure behavior per lock.
- Power loss is not automatically a security breach.
- UI explains unpowered lock behavior.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 32. Fail-Safe Locks
- On power loss, unlock to preserve egress/access.
- Appropriate for common/medical/emergency routes where safety dominates.
- Security risk may increase, but do not trigger a breach merely from power loss.
- Alarm can report lock unavailable.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 33. Fail-Secure Locks
- On power loss, remain locked mechanically or electrically if plausible in game abstraction.
- Require mechanical override/emergency power for entry.
- Never use on required egress routes without an independent emergency exit path.
- Data-integrity validator can enforce this.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 34. Manual/Mechanical Override
- Authorized personnel may use a key/manual release/tool interaction if equipment/system supports it.
- Override is a significant access event in sensitive zones.
- Forced mechanical entry is distinct from authorized override.
- Do not invent a key-item system unless inventory already supports it.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 35. Door Construction/Upgrade Integration
- Security hardware installation belongs to construction/maintenance systems.
- Security system consumes resulting lock/alarm capabilities.
- Do not create free locks by changing a zone dropdown if physical security is intended to cost resources.
- Open/basic policy can remain software/policy-only where appropriate.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 36. Zone Policy vs Door Hardware
- A zone can be restricted even with an unlocked door; unauthorized entry is policy violation if surveillance/observation can detect it.
- A locked door physically enforces access.
- Keep policy and hardware distinct.
- This allows low-resource shelters to declare sensitive areas before upgrading locks.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 37. Alarm Definition
- Use alarm profiles for unauthorized access, forced entry, panic button, door tamper, breach, and emergency integration.
- Alarm definition specifies severity mapping, notification channel, responder duty, lockdown escalation policy, and persistence.
- Do not create one universal alarm behavior.
- Alarm source event IDs are stable.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 38. Alarm State Machine
- Recommended: active → acknowledged → responding → resolved, with false_alarm as terminal outcome where appropriate.
- Do not conflate alarm status with zone alert level.
- One alarm can link to one or more incidents.
- Repeated sensor/attempt callbacks should update the same alarm if within one incident.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 39. Alarm Severity
- Retain low/medium/high/critical.
- Severity is based on source zone, breach type, actor context, forced-entry evidence, and emergency conditions.
- Do not randomize severity.
- Critical may request lockdown; low may only notify.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 40. Panic Button
- Only model if a panic/emergency interaction exists or is added as a real room control.
- Authorized actor can trigger alarm without being treated as intruder.
- Useful for medical/security/leadership rooms.
- No phantom panic buttons in zones without interactables.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 41. Alarm Power
- Alarms may depend on power/circuit capability.
- If backup power/battery exists, consume canonical power capability.
- Unpowered alarm does not mean access policy disappears; it means detection/notification may degrade.
- Do not create a second battery resource.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 42. Security Sensors/Surveillance
- Source asks for internal surveillance but does not define a camera system.
- V1 can model door/tamper/access-control detection without full cameras.
- If sensor hardware exists later, add coverage capability to zone/door.
- Do not claim omniscient breach detection in unlocked/unmonitored zones.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 43. Observed vs Unobserved Unauthorized Access
- Authorization failure and detection are distinct.
- Locked electronic door denial is inherently observed by the access-control system.
- Walking into an unlocked restricted room may only become a breach if surveillance, guard, witness, or later evidence detects it.
- Do not auto-detect all policy violations if no sensing exists.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 44. Surveillance Coverage Contract
- Expose `ISecurityDetectionCoverage` based on lock/sensor/guard/watch capability.
- Can return guaranteed, probabilistic, or none depending existing systems.
- V1 may make electronic controlled doors guaranteed and unmonitored open boundaries undetected.
- Use deterministic seeded detection only where uncertainty is intentional.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 45. Breach Definition
- Do not mark every denied attempt as a full breach.
- Use thresholds/types: repeated suspicious denied attempts, forced entry, tamper, sabotage, unauthorized actual entry, lockdown violation.
- Benign mistake can remain an access-denied event.
- Security breach is a higher-level incident.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 46. Breach State Machine
- Recommended: detected → active → contained/investigating → resolved, with resolution such as apprehended, escaped, false_alarm, authorized_exception, resolved.
- Keep stable breach ID.
- Link alarms and access events.
- Do not create one breach per repeated attempt in the same incident.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 47. Breach Correlation
- Correlate related denied attempts/forced entry within a bounded time/zone/actor window.
- Use root incident ID where upstream action provides it.
- Repeated pathfinding failures are never correlated because they are not committed attempts.
- Correlation is deterministic.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 48. Forced Entry
- Forced entry must be an explicit action through a door/interactable/combat/sabotage system.
- Security system validates hardware, records attempt, and consumes the confirmed outcome.
- Do not let an unauthorized actor bypass a lock merely by receiving a random breach roll.
- Door damage belongs to door/maintenance/combat systems.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 49. Sabotage Integration
- Plan 153 sabotage/espionage can emit tamper/unauthorized-entry intents.
- Security coverage may detect them.
- Espionage system owns agent intent/sabotage outcome.
- Security system owns access policy, alarm, and breach record.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 50. Conflict Integration
- Plan 202 may react to denied/unauthorized incidents.
- Security system should not create interpersonal hostility directly.
- Emit structured facts: denied, accused, detained if such response exists.
- Conflict/relations systems decide relationship consequences.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 51. Responder Dispatch
- Security alarm requests responders from DutyRoster/response coordinator.
- Do not create a second survivor assignment scheduler.
- Response request specifies zone, severity, required role/capability, urgency.
- Roster returns available responders.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 52. Responder Access
- Active responders receive role/emergency authorization appropriate to the incident.
- Do not manually grant permanent all-access to every guard.
- Responder authorization expires with duty/incident.
- Audit event records emergency override.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 53. Response Timing
- Source includes alarm response time.
- Derive from roster availability, distance/path, alert system, and configured response policy if those systems exist.
- Do not store one arbitrary global seconds-to-response if movement is simulated.
- If only abstract response exists, use deterministic policy delay.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 54. Apprehension Boundary
- Do not implement full detention/combat in this plan unless existing conflict/combat systems expose it.
- Breach resolution can consume confirmed `apprehended`, `escaped`, or `resolved` events from the responsible system.
- Security system tracks lifecycle.
- Follow-on can add detention.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 55. Lockdown Definition
- Lockdown is an explicit shelter security state with scope, reason, initiator, start time, policy profile, overrides, and end state.
- Do not simply set every door to `sealed` without context.
- Support shelter-wide or zone-scoped lockdown if topology permits.
- Persist current lockdown state.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 56. Lockdown Profiles
- Examples: security_lockdown, contamination_containment, fire_egress_mode, external_raid_lockdown, safe_room_lockdown.
- Different profiles have different door rules.
- Fire egress should not behave like intruder containment.
- Use a registered profile catalog.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 57. Security Lockdown
- Sensitive doors lock/seal; common movement may be restricted; responder/leader overrides apply.
- Emergency egress routes remain valid.
- Visitor movement may freeze to escort/safe zone.
- Do not trap civilians in hazardous rooms.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 58. Fire/Evacuation Integration
- Source says fire can trigger alarms/lockdown; refine this because sealing all doors during fire is dangerous.
- FireHazardSystem should activate an evacuation/containment profile that opens/permits safe egress while isolating affected zones where possible.
- Fire authority supplies hazard zones/routes.
- Security enforces emergency access rules.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 59. Medical Emergency Override
- Critical medical responders can cross ordinary restricted zones as policy allows.
- Do not require leader clicks during every emergency.
- Override is scoped/time-limited and audited.
- Medical inventory security can still require explicit emergency-access capability.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 60. External Raid Integration
- ShelterDefense may request a raid lockdown profile.
- Internal security locks armory/critical rooms appropriately while preserving defender movement.
- Defense owns raid state.
- Security does not start raids.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 61. Contamination/Quarantine Integration
- Airlock/medical systems may request quarantine-zone isolation.
- Security controls access boundary according to quarantine profile.
- Medical/contamination systems own who is quarantined and when it is safe to release.
- Do not duplicate disease state.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 62. Manual Lockdown
- Authorized leadership/security role can initiate lockdown.
- UI shows scope, consequences, overrides, and affected movement before confirmation where possible.
- Manual action uses stable command ID.
- Repeated button presses are idempotent.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 63. Lockdown Lift
- Only authorized role/system can lift.
- Emergency owner may have veto if hazard still active.
- Record initiator/day/reason.
- Door states restore from pre-lockdown policy/hardware, not blindly unlock all.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 64. Pre-Lockdown Door Snapshot
- Do not overwrite persistent door policy when lockdown begins.
- Lockdown applies an override layer.
- On lift, remove override and reveal prior state.
- This avoids doors incorrectly staying unlocked/locked afterward.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 65. Emergency Egress Validator
- At data-integrity/build time, validate that critical occupied/common zones have at least one legal egress route under relevant emergency profiles where topology data permits.
- Do not allow all fail-secure doors around dormitory/medical areas without override.
- This is a release gate.
- Runtime can warn if construction creates an unsafe configuration.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 66. Security Zone Catalog
- Create `Assets/StreamingAssets/Data/security_zones.json` for baseline zone policies and lock/alarm profiles.
- Use stable room/zone IDs.
- Dynamic constructed rooms may use default policies plus runtime assignment.
- Do not hardcode every room name in C#.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 67. Dynamic Room Security
- New rooms from shelter expansion need a default security policy based on room type/capability.
- Player can raise/lower policy after construction.
- Physical lock hardware still depends on installed upgrades.
- Default should favor usability.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 68. Room-Type Defaults
- Common areas: open.
- Workshop/storage/greenhouse: often open or restricted depending shelter policy.
- Armory/medical/comms/vault/reactor: stronger defaults where these rooms actually exist.
- Do not enforce the source examples if repository room taxonomy differs.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 69. Default Security Policy
- Keep default all-open behavior available.
- Allow preset: permissive, standard, hardened.
- Presets configure policies, not magical hardware.
- Old saves migrate to permissive/current behavior.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 70. Security Presets
- Presets reduce micromanagement.
- `Permissive`: common/open, only critical authored zones restricted.
- `Standard`: role-based sensitive areas.
- `Hardened`: stronger restrictions and alarms where hardware supports.
- Player can customize per zone.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 71. Policy Inheritance
- Zone policy can inherit shelter default unless overridden.
- Dynamic room additions use current default.
- Store only overrides where practical.
- This keeps state sparse and migration simple.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 72. Authorized Survivors List
- Do not persist a huge duplicated authorized list if authorization can be derived from role/clearance rules.
- Use explicit per-zone exceptions only.
- `authorizedSurvivors` in the source DTO should be interpreted as overrides/whitelist, not the sole authority.
- This prevents O(zones×survivors) state.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 73. Access Requirement Profiles
- Represent requirements as declarative rules: min_clearance, required_role_any, explicit_grant, escort_allowed, emergency_override, visitor_policy.
- No arbitrary executable expressions.
- Validator ensures referenced roles/clearances exist.
- Policy evaluator remains deterministic.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 74. Multiple Requirement Semantics
- Define AND/OR grouping explicitly.
- Example medical pharmacy: min restricted OR active medic role OR emergency medical override.
- Do not ambiguously treat a list of requirements as all required.
- Data schema should express rule groups.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 75. Access to Equipment vs Room
- Entering an armory and withdrawing a weapon are different actions.
- Room access is Plan 209; inventory permissions can consume the same authorization API for sensitive storage.
- Do not automatically grant item withdrawal rights because someone can enter.
- Follow existing storage/inventory action framework.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 76. Critical Controls
- Reactor/self-destruct/emergency controls should use action-level authorization if those systems exist.
- Do not create fictional self-destruct just to populate a critical tier.
- Critical clearance must correspond to real interactions.
- UI hides unused policy concepts.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 77. Access Events
- Use events such as access_granted_sensitive, access_denied, door_locked/unlocked, alarm_triggered, breach_detected, lockdown_started/ended, clearance_granted/revoked.
- Routine open-zone access does not need persistent events.
- Stable IDs and reason codes.
- Events link actor, zone, boundary, and source command.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 78. Time Representation
- Use campaign clock timestamp/day+minute or project-standard simulation time.
- Do not store separate ambiguous `day` and `hour` if one timestamp type already exists.
- Sort with stable event ID tie-break.
- Timezone has no role.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 79. Access Logging Privacy/UX
- In single-player shelter management, logs are operational history.
- Do not expose hidden narrative secrets unless the detecting system actually knows them.
- Unauthorized undetected entry should not appear as a confirmed access log unless later discovered.
- Distinguish attempted, detected, and confirmed.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 80. False Alarms
- False alarm can result from explicit sensor/system uncertainty or investigation outcome.
- Do not randomly manufacture false alarms solely for variety.
- If no sensor uncertainty exists, false_alarm remains reserved.
- Resolution provenance required.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 81. Alarm Acknowledgement
- Player/leader/security role can acknowledge without resolving.
- Acknowledgement suppresses repeated notifications but does not close incident.
- Responder state can advance independently.
- Save/load preserves status.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 82. Alarm Escalation
- Repeated/stronger evidence can escalate an existing alarm severity.
- Use one alarm/incident rather than creating a chain of duplicates.
- Escalation can request a stricter lockdown profile.
- Deterministic thresholds.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 83. Alarm De-Escalation
- Investigation/confirmed resolution lowers/closes alarm.
- Do not silently reset at day boundary.
- False alarm requires explicit outcome.
- History keeps final severity/outcome.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 84. Breach Resolution
- Resolution records responsible system, responders, outcome, and day.
- Do not require every breach to end in apprehension.
- Possible outcomes include escaped, false_alarm, authorized_exception, resolved, contained.
- Unresolved incidents remain active.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 85. Security Settings
- Source lists default security level, alarm response time, lockdown protocol.
- Refine into policy profiles and presets rather than free-floating primitive fields.
- Store player-selected default/preset and per-zone overrides.
- Derived response timing stays outside when scheduler exists.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 86. No Generic Random Security Tick
- Access authorization is deterministic and event-driven.
- `ISeededRng` is only needed for uncertain detection/investigation outcomes where another system cannot determine them.
- Do not roll random breaches because a day passed.
- `TickShelterSecurity` should be replaced by event/expiry/alarm schedule processing unless a daily checkpoint is actually needed.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 87. Deterministic Detection
- If an unmonitored unauthorized entry can be detected probabilistically through guard coverage, seed from campaign + incident + observer/coverage interval.
- Persist result.
- Reload cannot reroll.
- Prefer deterministic guaranteed detection for electronic controlled doors.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 88. Clearance Automation
- On role assignment, derive role authorization automatically.
- Do not issue/revoke explicit grants every shift.
- Temporary grants can be created by high-level commands such as 'grant repair team reactor access until job completes.'
- This is central to avoiding bureaucracy.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 89. Task-Scoped Authorization
- Scheduler/work system can request a temporary access token tied to a specific task/job.
- Example: maintenance worker gets access to comms room for repair job.
- Token expires when job ends/cancels.
- Do not permanently raise clearance for one task.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 90. Escort Authorization
- Visitors/new arrivals can enter restricted zones if escorted by an authorized survivor and zone policy permits.
- Escort relationship comes from movement/task system.
- If escort leaves, policy determines whether visitor must exit.
- Do not simulate complex escort AI inside security.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 91. Emergency Temporary Access
- Emergency systems can issue scoped tokens.
- Tokens have source emergency ID, allowed zones/actions, and expiry.
- Audit them.
- Do not convert them into permanent grants.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 92. Grant Authority
- Define who can grant which access level.
- Leadership/security-chief roles may have grant capability.
- A survivor should not grant a level higher than their authority unless a policy explicitly allows delegation.
- Validator/test escalation.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 93. Delegation
- Optional: leader can delegate clearance-administration capability.
- Use a role/capability flag rather than special-case survivor IDs.
- Delegation itself is a security event.
- Follow-on if LeadershipSystem supports it.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 94. Privilege Escalation Prevention
- An actor cannot self-grant access via the same API unless explicitly authorized.
- Grant command validates grantor authority and target scope.
- Repeated/replayed grant command is idempotent.
- Audit tests for all-access escalation.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 95. Expired Token Race
- Access checks use current simulation time at committed attempt.
- If token expires between path plan and crossing, revalidation denies or requests replanning.
- No use-after-expiry.
- Important for long movement jobs.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 96. Inside-Zone Revocation
- If clearance revoked while survivor is inside, do not teleport.
- Policy may allow exit-only, request escort, or trigger incident if they attempt sensitive actions.
- Exit egress should usually remain possible.
- Movement system handles leaving.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 97. Lockdown Occupancy
- Lockdown should account for occupants already inside zones.
- Do not seal a hazard with survivors trapped unless profile intends containment and safety system accepts it.
- UI lists affected occupants before manual lockdown if possible.
- Emergency systems may override.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 98. Visitors During Lockdown
- AirlockSecurity and internal security coordinate status.
- Visitors can be held in airlock/visitor zone or escorted to safe area based on policy.
- Do not duplicate admission decision.
- Lockdown is internal movement restriction.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 99. New Arrival Quarantine
- Airlock/medical quarantine can use a restricted quarantine zone.
- Visitor/new survivor authorization limited accordingly.
- Medical system decides quarantine duration/status.
- Security enforces boundary.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 100. Armory Integration
- Armory access policy can be role-based and task-based.
- Inventory withdrawal still requires inventory/action permission if implemented.
- During raid, defenders may get emergency armory access.
- Do not force manual clearance changes before every attack.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 101. Medical Storage Integration
- Medic role accesses medical zone/storage.
- Emergency responders may get temporary override.
- Controlled drugs/special items could require action-level authorization later.
- Do not block life-saving care because of a static room lock with no emergency path.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 102. Comms Integration
- Communications room can be restricted to radio/operator/security/leadership roles.
- Plan 157 remains communications authority.
- Security protects access only.
- Do not duplicate network authentication.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 103. Reactor/Power Room Integration
- Engineer/leadership role-based access.
- PowerGrid owns reactor/generator behavior.
- Security can deny unauthorized interaction/entry.
- Emergency repair task can issue scoped access.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 104. Vault/Critical Storage Integration
- Use high-security/critical policies for actual vault content.
- SafeCracking remains safe/lock puzzle authority if applicable.
- Security controls room/boundary access, not safe internals.
- Do not duplicate item ownership.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 105. Safe Room Integration
- Safe room should prioritize emergency entry for intended occupants under relevant profile.
- `critical` should not mean nobody except leader can enter during emergency.
- Use emergency-policy-specific authorization.
- This corrects the simplistic source tier mapping.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 106. Power Outage Edge Case
- Evaluate each lock by fail profile.
- Alarms/sensors may degrade if unpowered.
- Emergency egress remains valid.
- Power restoration returns hardware to policy state without duplicate lock events.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 107. Fire Edge Case
- Fire emergency may unlock egress routes, seal fire compartments, and override ordinary clearance.
- Do not apply security lockdown semantics blindly.
- FireHazardSystem provides hazard context.
- Test evacuation path availability.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 108. Raid Edge Case
- Raid lockdown hardens sensitive areas while defenders retain access.
- Visitor/noncombatant movement can be restricted safely.
- Power failure during raid follows lock profiles.
- Defense system remains authoritative.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 109. Medical Crisis Edge Case
- Patient located in restricted zone.
- Medic emergency token allows entry.
- Treatment system remains authoritative.
- No manual leader intervention required.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 110. Leadership Death/Succession Edge Case
- Role-derived all-access updates after succession.
- Do not leave dead leader as sole all-access owner.
- Explicit grants to dead survivor become inactive through survivor lifecycle validation.
- Emergency fallback authority policy required.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 111. No Leader Edge Case
- If shelter temporarily has no leader, critical emergency override path must still exist.
- Could be security-chief quorum/manual emergency mechanism depending LeadershipSystem.
- Do not permanently lock critical systems.
- Define policy.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 112. All Zones Open Edge Case
- Replicates pre-feature current behavior.
- No denials, no alarms from internal access.
- Security panel can still exist but minimal.
- Old-save baseline regression.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 113. Maximum Security Edge Case
- Every sensitive zone locked/hardened but common life must still function.
- Emergency egress and task tokens prevent deadlock.
- AI scheduler should not endlessly assign impossible jobs.
- Run long simulation.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 114. Zero Power + Maximum Security Edge Case
- Stress fail-safe/fail-secure mix.
- Ensure no unsalvageable shelter lockout.
- Critical manual override/emergency path remains.
- Release gate.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 115. Repeated Denial Edge Case
- One mistaken attempt logs denial.
- Repeated committed attempts may escalate only according to policy/time window.
- Path planner rejections do not count.
- UI must not spam alarms.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 116. Forced Entry Edge Case
- Explicit forced action triggers tamper/door damage outcome, detection if covered, alarm, breach correlation, response request.
- One root event.
- Do not duplicate breach via door + alarm + conflict consumers.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 117. Sabotage Edge Case
- Intruder with valid stolen/compromised credential could pass access check; detection belongs to espionage/intelligence layer if credential misuse is modeled.
- Do not make clearance system omniscient.
- Follow-on credential compromise possible.
- V1 can assume survivor identity is authentic.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 118. Access Log Query
- Index by zone, survivor, event type, day range, severity/incident.
- UI searches structured fields.
- Do not parse rendered note strings.
- Archive old routine events through retention policy.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 119. Security Map
- Overlay room security level, lock state, alarm state, active breach, and lockdown scope.
- Use canonical room map geometry.
- Do not create duplicate topology.
- Provide list/text alternative for accessibility.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 120. Zone Detail UI
- Show baseline level, effective policy, lock hardware/state, power dependency, alarm coverage, role access, explicit grants, active overrides, and recent incidents.
- Allow policy edits through commands.
- Physical hardware changes route to construction/door system.
- UI does not mutate state directly.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 121. Clearance Panel
- Show role-derived access separately from explicit grants.
- Grant/revoke temporary or permanent scoped clearance.
- Display expiry and grantor.
- Prevent granting impossible/higher authority with disabled state/reason.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 122. Access Log UI
- Default to significant incidents only.
- Filter denied, override, high-security access, breach, lockdown, clearance change.
- Do not show every mess-hall entry.
- Pagination/retention.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 123. Alarm Panel
- Show active alarms, source, zone, severity, acknowledgement, responder status, linked breach, and safe actions.
- Allow acknowledge/escalate/lift only according to authority.
- Do not resolve automatically because panel closed.
- Projection-only.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 124. Breach Panel
- Show active incident timeline, actor if known, affected zone/door, related alarms, response, resolution state.
- Unknown intruder remains unknown.
- Do not leak espionage identities not yet discovered.
- Link to conflict/investigation system where implemented.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 125. Lockdown UI
- Show profile, scope, reason, initiator, affected doors/zones/occupants, emergency overrides, and projected egress concerns.
- Manual lockdown confirmation should warn about unsafe affected areas.
- Lift controls respect authority.
- No global 'seal everything' one-click without policy explanation.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 126. Security Alerts
- Actionable only: breach, critical denial/forced entry, unpowered fail-secure critical lock, unsafe lockdown configuration, alarm needing acknowledgement.
- Aggregate repeated denials.
- Use cooldown/hysteresis.
- Routine access creates no notification.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 127. Tutorial
- First meaningful denial in a sensitive zone explains zone level, role/clearance, lock hardware, and how to grant task/temporary access.
- Do not fire tutorial on old-save migration.
- Explain emergency overrides.
- Keep concise.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 128. Tooltips
- Security level tooltip shows who/what normally qualifies.
- Lock tooltip shows fail-safe/fail-secure and power state.
- Clearance tooltip shows scope/expiry.
- Critical information must also be available without hover.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 129. Accessibility
- Security state must not rely on color alone.
- Support keyboard/controller, text scaling, list alternative to map, and clear reason messages.
- Lockdown/alarms should avoid flashing-only indicators.
- Accessible emergency controls are mandatory.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 130. Quest Hooks
- Source hooks: Guard, Warden, Detective, Locksmith, Watchman, Secure, Crisis Manager.
- Refine counts to unique incidents/grants/upgrades rather than repeated toggles.
- `zero breaches for 100 days` should ignore migration era and define what counts as a breach.
- `maximum security for all zones` should exclude open/common zones that are intentionally non-secured.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 131. Achievement Integration
- Plan 149 can observe sustained breach-free periods, first successful containment, complete sensitive-zone hardware, or safe emergency lockdown.
- Do not reward needless clearance churn.
- Use stable incident/upgrade IDs.
- Security system exports facts only.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 132. Archive/Epilogue Integration
- Plan 145/archive may consume famous breach, successful lockdown, internal sabotage detection, or long secure period.
- Only meaningful confirmed incidents.
- Export structural facts.
- No routine access history.

Implementation consequence: treat this concern as a tested authorization/incident contract. The actor identity, role/clearance source, room/boundary ID, power/hardware state, emergency overlay, side-effect boundary, save semantics, and UI projection must resolve consistently before the feature is considered wired.

---
## 133. Noise Discipline Integration
- Plan 205 acoustic alarms/door operations may create noise but ShelterSecurity remains access authority.
- Security alarm can emit a noise-source event to Plan 205.
- Noise consequences remain acoustic.
- Do not merge security alert level with acoustic signature.
