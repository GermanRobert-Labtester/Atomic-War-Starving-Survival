# Plan 194 — Emergency Alert & Warning System

## Goal

Create a comprehensive emergency alert and warning system that detects threats (radiation storms, raider attacks, shelter breaches, disease outbreaks, system failures) and automatically warns survivors with appropriate urgency levels, giving players time to prepare and respond. Currently individual systems detect their own emergencies (radiation storms, shelter fires, disease outbreaks) but there is no unified alert system — no alert hierarchy, no centralized warning broadcast, no alert prioritization, no evacuation protocols, no alert history. Players learn about emergencies reactively rather than proactively. This plan transforms emergency response from reactive chaos to proactive management.

## Why

**Repository evidence:** Grep for `EmergencyBroadcast`, `AlertSystem`, `EmergencyAlert`, `WarningSystem`, `ShelterAlert`, `EvacuationAlert` in Core returns ZERO matches. Individual systems detect emergencies: `WeatherSystem` detects radiation storms, `ShelterFireHazardSystem` detects fires, `DiseaseSystem` detects outbreaks, `SumpFloodingSystem` detects flooding, `RadiationSystem` detects high radiation — but each handles its own notifications independently. No unified alert system, no alert hierarchy, no centralized warning broadcast, no alert prioritization, no evacuation protocols.

**What is missing:** No unified emergency alert system. No alert hierarchy (info/warning/critical/emergency). No centralized warning broadcast. No alert prioritization (which emergency is most urgent). No evacuation protocols. No alert history/log. No alert acknowledgment. No alert escalation. Players learn about emergencies when they're already happening, not before.

**Why existing plans don't solve it:** Plan 158 (disaster response) adds emergency response protocols but not early warning. Plan 186 (shelter maintenance) adds component failure warnings but not unified alerts. Plan 135 (weather cascade) adds weather warnings but not general emergency alerts. No plan addresses emergency alerts as a unified system.

**Player value:** Creates proactive gameplay (prepare before crisis), adds strategic depth (prioritize responses), generates emergent stories (racing against the clock), and makes emergencies feel manageable rather than overwhelming.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/World/WeatherSystem.cs` — weather emergencies
- `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` — fire emergencies
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` — disease outbreaks
- `Assets/Ashfall.Core/SumpFloodingSystem.cs` — flooding emergencies
- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — radiation emergencies
- NEW: `Assets/Ashfall.Core/Emergency/EmergencyAlertSystem.cs`
- NEW: `src/UI/EmergencyAlertPanel.cs`

## Main Task 1 — Foundation / System Contract

1. Create `EmergencyAlertSystem.cs` in `Assets/Ashfall.Core/Emergency/`
2. Define `EmergencyAlert` DTO: `alertId`, `alertType` (radiation_storm/raider_attack/shelter_breach/disease_outbreak/fire/flooding/power_failure/air_failure/water_contamination/structural_collapse), `severity` (info/warning/critical/emergency), `source` (system that detected it), `detectedDay`, `detectedHour`, `estimatedImpact` (description), `affectedAreas` (list of shelter zones/locations), `responseTime` (hours until impact), `requiredActions` (list of recommended actions), `isAcknowledged` bool, `isResolved` bool
3. Define `AlertPriority` DTO: `priorityId`, `alertType`, `basePriority` (1-10, 10=highest), `escalationRate` (priority increase per hour unresolved), `maxPriority` (cap), `autoEscalate` bool
4. Define `EvacuationProtocol` DTO: `protocolId`, `protocolName`, `triggerCondition` (alert type + severity), `evacuationRoute` (list of safe zones), `assemblyPoint`, `requiredSupplies` (list), `assignedSurvivors` (list), `status` (standby/active/completed/cancelled)
5. Define `AlertResponse` DTO: `responseId`, `alertId`, `responseType` (shelter_in_place/evacuate/contain/repair/medical_emergency/fire_suppression/radiation_shielding), `assignedSurvivorIds` (list), `requiredItems` (list), `responseTime` (hours), `successChance` (0-100), `status` (planned/in_progress/completed/failed)
6. Define `EmergencyAlertState` DTO: list of active alerts, list of alert priorities, list of evacuation protocols, list of alert responses, alert history (last 50), alert settings (auto-acknowledge threshold, sound alerts bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define alert severity levels:
   - **Info**: minor issue, no immediate danger (filter needs replacement, low supplies)
   - **Warning**: potential problem, prepare response (storm approaching, minor leak)
   - **Critical**: imminent danger, immediate action required (fire spreading, breach detected)
   - **Emergency**: life-threatening, evacuate/shelter immediately (radiation storm, structural collapse)
9. Define alert types (12+ types):
   - **Radiation Storm**: weather-based, requires shelter/shielding
   - **Raider Attack**: faction-based, requires defense/evacuation
   - **Shelter Breach**: structural, requires repair/evacuation
   - **Disease Outbreak**: medical, requires quarantine/treatment
   - **Fire**: shelter hazard, requires suppression/evacuation
   - **Flooding**: water hazard, requires pumping/evacuation
   - **Power Failure**: system failure, requires repair/generator
   - **Air Failure**: ventilation failure, requires repair/masks
   - **Water Contamination**: water hazard, requires treatment/alternative source
   - **Structural Collapse**: critical structural, requires evacuation/rescue
   - **Gas Leak**: environmental, requires evacuation/repair
   - **Medical Emergency**: individual health, requires treatment/evacuation
10. Define alert detection:
    - Each system reports potential emergencies to alert system
    - Alert system evaluates severity based on conditions
    - Alert system prioritizes based on urgency + impact
    - Alert system broadcasts to player
    - Alert system tracks acknowledgment + response
11. Define alert broadcast mechanics:
    - Visual alert: on-screen notification with severity color
    - Audio alert: distinct sound per alert type
    - Text alert: detailed description + recommended actions
    - Map alert: affected areas highlighted
    - Survivor alert: survivors react (panic, prepare, evacuate)
12. Define alert acknowledgment:
    - Player must acknowledge critical/emergency alerts
    - Acknowledgment stops alert escalation
    - Unacknowledged alerts auto-escalate
    - Acknowledgment logged
13. Define alert response:
    - Player assigns survivors to respond
    - Response requires items/resources
    - Response has success chance
    - Response time tracked
    - Response outcome logged
14. Define evacuation protocols:
    - Pre-defined evacuation routes per emergency type
    - Assembly points designated
    - Required supplies listed
    - Survivors assigned to protocol
    - Protocol activated on emergency alert
    - Protocol completion tracked
15. Add deterministic seeding: alert detection uses `ISeededRng`
16. Wire into `GameBootstrap`: `SetupEmergencyAlerts`, `TickEmergencyAlerts`, `SaveEmergencyAlerts`

## Main Task 2 — Implementation / Detection / Broadcast / Response / Evacuation

1. Implement alert detection:
   - Subscribe to emergency events from all systems
   - Evaluate alert severity
   - Create alert DTO
   - Assign priority
   - Broadcast alert
   - Detection logged
2. Implement alert prioritization:
   - Calculate priority based on severity + urgency + impact
   - Sort active alerts by priority
   - Display highest priority first
   - Auto-escalate unacknowledged alerts
   - Priority displayed in UI
3. Implement alert broadcast:
   - Visual notification (color-coded by severity)
   - Audio alert (distinct sound per type)
   - Text description (what, where, when, what to do)
   - Map highlight (affected areas)
   - Survivor reaction (panic, prepare, evacuate)
4. Implement alert acknowledgment:
   - Player clicks alert to acknowledge
   - Acknowledgment stops escalation
   - Acknowledgment logged
   - Unacknowledged alerts escalate
   - Acknowledgment required for critical/emergency
5. Implement alert response:
   - Player assigns survivors to respond
   - Response requires items/resources
   - Response has success chance
   - Response time tracked
   - Response outcome determined
   - Response logged
6. Implement evacuation protocols:
   - Pre-defined routes per emergency type
   - Assembly points designated
   - Survivors assigned to protocol
   - Protocol activated on alert
   - Evacuation progress tracked
   - Protocol completion logged
7. Implement alert history:
   - Last 50 alerts stored
   - Alert details preserved
   - Response outcomes recorded
   - History viewable in UI
   - History searchable by type/date
8. Implement alert settings:
   - Auto-acknowledge threshold (auto-ack info/warning)
   - Sound alerts toggle
   - Alert volume control
   - Alert display duration
   - Alert priority filter
9. Implement alert integration:
   - WeatherSystem: radiation storm alerts
   - ShelterFireHazardSystem: fire alerts
   - DiseaseSystem: outbreak alerts
   - SumpFloodingSystem: flooding alerts
   - RadiationSystem: high radiation alerts
   - ShelterMaintenanceSystem (Plan 186): component failure alerts
   - All systems report to central alert system
10. Create alert events:
    - "The Warning" — emergency detected
    - "The Alert" — alert broadcast
    - "The Acknowledgment" — alert acknowledged
    - "The Response" — response initiated
    - "The Evacuation" — evacuation protocol activated
    - "The Resolution" — emergency resolved
    - "The Escalation" — alert escalated
    - "The Crisis" — multiple simultaneous emergencies
11. Add alert quest hooks:
    - "The Prepared" — acknowledge 10 alerts within response time
    - "The Responder" — successfully respond to 20 emergencies
    - "The Evacuator" — complete 5 evacuation protocols
    - "The Manager" — handle 3 simultaneous emergencies
    - "The Protector" — prevent 10 emergencies through preparation
    - "The Leader" — no survivor casualties in 50 emergencies
    - "The Vigilant" — maintain alert system for 100 days
12. Implement emergency alert UI:
    - Alert panel: active alerts with priority
    - Alert detail: description, affected areas, recommended actions
    - Response panel: assign survivors, allocate resources
    - Evacuation panel: activate protocols, track progress
    - Alert history: past alerts with outcomes
    - Alert settings: customize alert behavior
13. Add alert journal: automatic log of emergency events
14. Implement alert tutorial: first alert explains system
15. Add alert tooltips: hover over alert shows details, response options

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `WeatherSystem`: radiation storm alerts
2. Connect to `ShelterFireHazardSystem`: fire alerts
3. Integrate with `DiseaseSystem`: outbreak alerts
4. Connect to `SumpFloodingSystem`: flooding alerts
5. Wire into `RadiationSystem`: high radiation alerts
6. Connect to `ShelterMaintenanceSystem` (Plan 186): component failure alerts
7. Implement old-save compatibility: existing saves get empty alert state
8. Add deterministic seeding: alert detection uses `ISeededRng`
9. Create exploit prevention: alerts are detection-based, can't be gamed
10. Add tests: alert detection, prioritization, broadcast, acknowledgment, response, evacuation, save round-trip
11. Verify all alert types work correctly
12. Test edge cases: no alerts (peaceful period), many alerts (crisis management)
13. Verify headless behavior: alerts process correctly without UI
14. Add data-integrity-selftest: alerts validate against emergency catalogs
15. Create `--emergency-alert-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --emergency-alert-selftest
```

## Risk

**LOW** — Emergency alerts are straightforward with clear inputs (detection events) and outputs (alerts, responses). Risk of alert fatigue (too many alerts). Mitigation: prioritize effectively, allow filtering, auto-acknowledge minor alerts, and ensure critical alerts feel urgent.

## Definition of Done

- `EmergencyAlertSystem.cs` exists with full `CaptureState/RestoreState`
- 12+ alert types (radiation storm, raider attack, shelter breach, disease outbreak, fire, flooding, power/air/water failure, structural collapse, gas leak, medical emergency)
- 4 severity levels (info, warning, critical, emergency)
- Alert prioritization system
- Alert broadcast (visual, audio, text, map, survivor reaction)
- Alert acknowledgment mechanic
- Alert response system (assign survivors, allocate resources)
- Evacuation protocols (routes, assembly points, supplies)
- Alert history (last 50 alerts)
- Alert settings (auto-ack, sound, volume, duration, filter)
- Alert events and quest hooks
- Save/load round-trip tested
- Deterministic alert detection verified
- Old saves load with empty alert state
- Alert definitions in data authority
- UI alert panel, response panel, evacuation panel, history, settings
- Cross-system integration (weather, fire, disease, flooding, radiation, shelter maintenance)

## Follow-On Opportunities

- Alert specialization (survivors with emergency response training)
- Alert legacy (famous emergency responses remembered)
- Alert quests (specific emergency response goals)
- Alert events (false alarms, cascading failures)
- Alert trading (share emergency intel with other settlements)
