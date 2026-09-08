# Plan 214 — Visitor Integration & Housing System

## Goal

Create a visitor integration and housing system where admitted visitors (refugees, traders, defectors, guests) are processed, assigned temporary housing, integrated into shelter life, and either transition to permanent residents or depart after their stay. Currently `AirlockSecuritySystem.cs` (227 lines) handles visitor arrival decisions (Admit/Inspect/Quarantine/TurnAway/Defend) — but once admitted, visitors simply appear with no processing, no housing assignment, no integration period, no departure tracking. Visitors are either instantly recruited or ignored. This plan adds visitor management as a shelter operations layer.

## Why

**Repository evidence:** `AirlockSecuritySystem.cs` (227 lines) handles airlock decisions. Plan 138 (Shelter Defense & Visitors) mentions "Refugee integration: assign housing, work duties, monitor behavior" but doesn't implement it. No visitor integration system exists. Once a visitor is admitted, there is no processing pipeline, no temporary housing, no integration period, no departure tracking.

**What is missing:** No visitor processing pipeline. No temporary housing assignment. No integration period. No departure tracking. No visitor status (guest/refugee/candidate/departed). No visitor resource consumption. No visitor monitoring. Visitors are admitted and then... nothing.

**Why existing plans don't solve it:** Plan 138 (shelter defense) mentions visitor integration as a feature but doesn't implement. Plan 204 (Recruitment) covers recruiting new survivors but not visitor processing. Plan 206 (Death & Inheritance) doesn't address visitors. No plan addresses visitor management as a system.

**Player value:** Creates operational depth (manage visitors), adds realism (visitors need housing/food/processing), generates emergent stories (visitor integration, departure decisions), and makes the shelter feel like a real place with guests.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/AirlockSecuritySystem.cs` — visitor arrival (227 lines, entry point)
- `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` — survivor registry
- `Assets/Ashfall.Core/Shelter/ShelterScheduleSystem.cs` — room assignments
- NEW: `Assets/Ashfall.Core/Visitors/VisitorIntegrationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/visitor_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `VisitorIntegrationSystem.cs` in `Assets/Ashfall.Core/Visitors/`
2. Define `Visitor` DTO: `visitorId`, `visitorName`, `visitorType` (refugee/trader/defector/guest/envoy/deserter/exile), `arrivalDay`, `admittedBy` (survivor_id who authorized entry), `status` (processing/integrated/temporary_resident/departing/departed/recruited), `assignedHousing` (room_id or null), `integrationProgress` (0-100), `departureDay` (-1 if no planned departure), `resourceConsumption` (resources per day), `monitoringLevel` (none/low/medium/high), `notes` (additional details)
3. Define `VisitorHousing` DTO: `housingId`, `visitorId`, `roomId` (assigned room), `housingType` (temporary_bunk/shared_quarter/private_room/guest_suite), `assignedDay`, `expectedDuration` (days, -1 if indefinite), `condition` (0-100, housing quality), `isTemporary` bool
4. Define `IntegrationTask` DTO: `taskId`, `visitorId`, `taskType` (orientation/housing_assignment/work_assignment/medical_check/security_clearance/cultural_orientation/language_assistance), `assignedTo` (survivor_id who handles task), `assignedDay`, `dueDay`, `completedDay` (-1 if incomplete), `status` (pending/in_progress/completed/skipped)
5. Define `VisitorDeparture` DTO: `departureId`, `visitorId`, `departureType` (voluntary/invited/forced/deported/escaped/recruited), `departureDay`, `reason`, `itemsTaken` (list of item_ids), `partingGift` (items given by shelter), `finalStatus` (good_standing/neutral/bad_standing/hostile)
6. Define `VisitorMonitoring` DTO: `monitoringId`, `visitorId`, `monitoringLevel` (none/low/medium/high), `reason` (why monitored), `assignedBy` (survivor_id), `assignedDay`, `findings` (list of suspicious activities), `escalationLevel` (0-100)
7. Define `VisitorIntegrationState` DTO: list of active visitors, list of visitor housing assignments, list of integration tasks, list of departures, list of monitoring records, integration settings (auto-assign housing bool, integration period days, monitoring enabled bool)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define visitor types (6+ types):
   - **Refugee**: fleeing danger, seeking shelter, may become permanent resident
   - **Trader**: here for trade, temporary stay, will depart
   - **Defector**: leaving faction, seeking asylum, may become resident
   - **Guest**: visiting from allied faction, temporary, will depart
   - **Envoy**: diplomatic representative, temporary, specific mission
   - **Deserter**: abandoned faction/post, seeking refuge, may become resident
10. Define visitor processing:
    - Visitor admitted → processing status
    - Processing tasks: orientation, housing, work assignment, medical check
    - Processing takes time (1-3 days)
    - After processing: integrated or temporary resident
    - Processing logged
11. Define housing assignment:
    - Visitors assigned temporary housing
    - Housing types: temporary bunk, shared quarter, private room, guest suite
    - Housing has condition (quality)
    - Housing can be upgraded
    - Housing logged
12. Define integration period:
    - New visitors have integration period (7-30 days)
    - Integration progress increases over time
    - Integration tasks accelerate progress
    - Full integration: visitor can become permanent resident
    - Integration logged
13. Define departure mechanics:
    - Visitors can depart voluntarily
    - Visitors can be invited to leave
    - Visitors can be forced out (deported)
    - Visitors can escape
    - Visitors can be recruited (become permanent)
    - Departure logged
14. Define monitoring:
    - Suspicious visitors monitored
    - Monitoring levels: none/low/medium/high
    - Monitoring findings logged
    - High monitoring: escalation possible
    - Monitoring logged
15. Add deterministic seeding: visitor events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupVisitorIntegration`, `TickVisitorIntegration`, `SaveVisitorIntegration`

## Main Task 2 — Implementation / Processing / Housing / Integration / Departure / Monitoring / UI

1. Implement visitor processing:
   - Visitor admitted → processing
   - Processing tasks assigned
   - Tasks completed over time
   - After processing: integrated
   - Processing logged
2. Implement housing assignment:
   - Visitors assigned housing
   - Housing type based on visitor status
   - Housing condition tracked
   - Housing logged
3. Implement integration period:
   - Integration progress tracked
   - Tasks accelerate progress
   - Full integration: permanent resident option
   - Integration logged
4. Implement departure:
   - Visitors depart (voluntary/invited/forced/escaped/recruited)
   - Departure logged
   - Items taken/gifts given
   - Final status recorded
5. Implement monitoring:
   - Suspicious visitors monitored
   - Findings logged
   - Escalation possible
   - Monitoring logged
6. Implement visitor UI:
   - Visitor panel: active visitors, status, housing
   - Visitor detail: type, progress, tasks, monitoring
   - Housing panel: visitor housing assignments
   - Integration panel: integration tasks, progress
   - Departure panel: pending departures
   - Monitoring panel: monitored visitors, findings
7. Create visitor events:
    - "The Arrival" — visitor admitted
    - "The Processing" — visitor being processed
    - "The Integration" — visitor integrating
    - "The Departure" — visitor leaving
    - "The Recruitment" — visitor becomes resident
    - "The Deportation" — visitor forced out
    - "The Escape" — visitor escapes
    - "The Monitoring" — suspicious activity detected
8. Add visitor quest hooks:
    - "The Host" — integrate 10 visitors
    - "The Diplomat" — host 5 envoy visitors
    - "The Recruiter" — recruit 5 visitors as residents
    - "The Monitor" — successfully monitor 3 suspicious visitors
    - "The Housing Manager" — house 20 visitors
    - "The Integration Specialist" — fully integrate 15 visitors
    - "The Gatekeeper" — process 30 visitors total
9. Implement visitor tutorial: first visitor arrival explains system
10. Add visitor tooltips: hover over visitor shows details
11. Create visitor templates in data file (15+ visitor types)
12. Implement visitor persistence: visitors/housing/tasks saved
13. Integrate with `AirlockSecuritySystem`: visitor admission triggers processing

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `AirlockSecuritySystem`: visitor admission triggers processing
2. Connect to `SurvivorCatalog`: recruited visitors become survivors
3. Integrate with `ShelterScheduleSystem`: housing assignments
4. Connect to `NeedsSystem`: visitors consume resources
5. Wire into `ShelterSecuritySystem` (Plan 209): monitoring integrates with security
6. Connect to `InternalCommunicationSystem` (Plan 211): visitor notices
7. Implement old-save compatibility: existing saves get no active visitors
8. Add deterministic seeding: visitor events use `ISeededRng`
9. Create exploit prevention: visitors are event-driven, can't be gamed
10. Add tests: visitor processing, housing, integration, departure, monitoring, save round-trip
11. Verify all visitor types work correctly
12. Test edge cases: no visitors (current behavior), many visitors (shelter full)
13. Verify headless behavior: visitor integration processes correctly without UI
14. Add data-integrity-selftest: visitors validate against room/survivor catalogs
15. Create `--visitor-integration-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --visitor-integration-selftest
```

## Risk

**LOW** — Visitor integration is straightforward with clear inputs (admitted visitors) and outputs (processing, housing, departure). Risk of visitor management feeling like bureaucracy. Mitigation: make visitors feel like individuals (backstories, personalities), show clear consequences, and ensure integration feels meaningful not tedious.

## Definition of Done

- `VisitorIntegrationSystem.cs` exists with full `CaptureState/RestoreState`
- 6+ visitor types (refugee, trader, defector, guest, envoy, deserter)
- Visitor processing pipeline (orientation, housing, work, medical, security)
- Housing assignment (temporary bunk, shared quarter, private room, guest suite)
- Integration period (progress tracking, task acceleration)
- Departure mechanics (voluntary, invited, forced, escaped, recruited)
- Monitoring system (suspicious visitors, findings, escalation)
- Visitor events and quest hooks
- Save/load round-trip tested
- Deterministic visitor events verified
- Old saves load with no active visitors
- Visitor templates in data authority (15+ types)
- UI visitor panel, visitor detail, housing panel, integration panel, departure panel, monitoring panel
- Cross-system integration (airlock security, survivor catalog, schedule, needs, security, communication)

## Follow-On Opportunities

- Visitor specialization (survivors become expert hosts/diplomats)
- Visitor legacy (famous visitors remembered)
- Visitor quests (specific visitor goals)
- Visitor events (mass refugee arrival, diplomatic summit)
- Visitor trading (trade visitor services with other settlements)
