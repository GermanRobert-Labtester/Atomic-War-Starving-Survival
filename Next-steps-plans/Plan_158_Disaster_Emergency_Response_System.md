# Plan 158 — Disaster & Emergency Response System

## Goal

Create a disaster and emergency response system where major crises — earthquakes, floods, fires, radiation leaks, structural failures, disease outbreaks — threaten the shelter and require coordinated emergency response. Currently shelter systems handle gradual degradation (thermal, ventilation, flooding) but there are no acute disaster events that require immediate player action, emergency protocols, or crisis management. This plan adds dramatic crisis moments that test shelter resilience and player decision-making.

## Why

**Repository evidence:** `ShelterThermalSystem.cs` (469 lines) handles gradual temperature changes. `VentilationSystem.cs` (269 lines) manages air quality. `SumpFloodingSystem.cs` (298 lines) tracks water infiltration. But all are gradual processes — no sudden disasters, no emergency events, no crisis response. Plan 135 (weather cascade) makes weather affect shelter but through gradual modifiers, not acute disasters. Plan 138 (shelter defense) handles raids but not natural/technical disasters.

**What is missing:** The shelter never faces sudden crisis. No earthquake shakes the bunker. No fire breaks out. No radiation leak forces evacuation. No structural collapse traps survivors. There are no emergency protocols, no crisis management, no dramatic "the shelter is in danger" moments.

**Why existing plans don't solve it:** Plan 29 (shelter as character) covers wear and degradation but not acute disasters. Plan 135 (weather cascade) adds weather effects but they're gradual modifiers. Plan 138 (shelter defense) handles raids but not natural/technical disasters. Plan 156 (shelter expansion) adds construction but not disaster vulnerability. No plan addresses emergency response or disaster management.

**Player value:** Creates dramatic tension (the shelter is in danger!), tests preparation (emergency supplies, protocols), generates emergent stories (how did we survive that?), and makes shelter management feel consequential (disasters punish unpreparedness).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Shelter/` — shelter systems
- `Assets/Ashfall.Core/ShelterThermalSystem.cs` — thermal management
- `Assets/Ashfall.Core/VentilationSystem.cs` — air quality
- `Assets/Ashfall.Core/SumpFloodingSystem.cs` — water management
- `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` — medical response
- NEW: `Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs`
- NEW: `Assets/StreamingAssets/Data/disaster_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `DisasterResponseSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `DisasterEvent` DTO: `eventId`, `disasterType` (earthquake/flood/fire/radiation_leak/structural_failure/disease_outbreak/power_failure/air_contamination), `severity` (minor/moderate/severe/catastrophic), `affectedRooms` (list), `startedDay`, `durationDays`, `status` (active/contained/resolved), `casualties` (list of survivor IDs)
3. Define `EmergencyProtocol` DTO: `protocolId`, `protocolType` (evacuation/lockdown/quarantine/fire_suppression/medical_emergency/shelter_in_place), `triggerCondition`, `requiredResources`, `assignedPersonnel`, `status` (ready/active/complete)
4. Define `EmergencySupply` DTO: `supplyId`, `supplyType` (medical/fire_suppression/radiation_kit/emergency_food/water_purification), `quantity`, `location`, `expiryDay`
5. Define `DisasterResponseState` DTO: list of active disasters, list of emergency protocols, list of emergency supplies, disaster history, shelter resilience rating
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define disaster types with distinct mechanics:
   - **Earthquake**: structural damage, room collapse, survivor injury, aftershocks
   - **Flood**: water damage, electrical hazard, contamination, evacuation required
   - **Fire**: room destruction, smoke damage, oxygen depletion, spread risk
   - **Radiation leak**: contamination spread, health effects, evacuation/containment
   - **Structural failure**: room collapse, trapped survivors, rescue required
   - **Disease outbreak**: infection spread, quarantine required, medical response
   - **Power failure**: systems offline, backup power required, cascade failures
   - **Air contamination**: toxic air, respirator required, ventilation failure
8. Define disaster trigger rules:
   - Earthquakes: random, modified by geological stability, weather
   - Floods: heavy rain + poor drainage + low shelter level
   - Fires: electrical fault, cooking accident, arson, lightning
   - Radiation leaks: equipment failure, damage, sabotage
   - Structural failure: over-expansion, poor construction, damage
   - Disease outbreaks: infected visitor, poor sanitation, mutation
   - Power failure: equipment failure, overload, sabotage
   - Air contamination: filter failure, external contamination, chemical spill
9. Define emergency response mechanics:
   - Player activates emergency protocol
   - Protocol requires resources and personnel
   - Response reduces disaster severity/duration
   - Failed response: disaster worsens, casualties increase
   - Successful response: disaster contained, minimal damage
10. Define shelter resilience:
    - Resilience based on construction quality, maintenance, upgrades
    - High resilience: disasters less severe, response more effective
    - Low resilience: disasters more severe, response less effective
    - Resilience improved through construction, maintenance, training
11. Add deterministic seeding: disaster triggers use `ISeededRng`
12. Wire into `GameBootstrap`: `SetupDisasterResponse`, `TickDisasters`, `SaveDisasterResponse`
13. Create `DisasterTemplateCatalogLoader` for disaster definitions
14. Implement disaster warning system: early warning provides preparation time
15. Create UI hook: emergency panel showing active disasters, protocols, supplies

## Main Task 2 — Implementation / Disasters / Protocols / Response / Recovery

1. Implement earthquake disaster:
   - Tremor warning (seconds to minutes)
   - Structural damage to rooms (condition reduced)
   - Survivor injury (health reduced, possible death)
   - Aftershocks (additional damage waves)
   - Response: shelter in place, structural assessment, rescue trapped
   - Recovery: repair damage, treat injured, restore systems
2. Implement flood disaster:
   - Water level rising (warning: hours)
   - Electrical hazard (power must be cut)
   - Contamination risk (waterborne disease)
   - Evacuation required (move to higher levels)
   - Response: sandbags, pumps, evacuation, water purification
   - Recovery: pump water, repair damage, decontaminate
3. Implement fire disaster:
   - Fire starts in room (smoke detected)
   - Fire spreads to adjacent rooms (if not contained)
   - Oxygen depletion (asphyxiation risk)
   - Structural damage (room may collapse)
   - Response: fire suppression, evacuation, oxygen supply
   - Recovery: repair damage, replace lost items, treat burns
4. Implement radiation leak disaster:
   - Radiation spike detected (Geiger alarm)
   - Contamination spreads (rooms affected)
   - Health effects (radiation sickness)
   - Evacuation/containment required
   - Response: seal area, respirators, decontamination
   - Recovery: repair source, decontaminate, treat exposed
5. Implement structural failure disaster:
   - Structural stress detected (warning: cracks, sounds)
   - Room collapse (instant damage)
   - Survivors trapped (rescue required)
   - Adjacent rooms at risk
   - Response: evacuate area, rescue trapped, shore up structure
   - Recovery: rebuild room, rescue survivors, reinforce structure
6. Implement disease outbreak disaster:
   - Disease detected (symptoms appear)
   - Infection spreads (contact transmission)
   - Quarantine required (isolate infected)
   - Medical response needed (treatment, vaccines)
   - Response: quarantine, treatment, sanitation, vaccination
   - Recovery: treat infected, sanitize shelter, prevent recurrence
7. Implement power failure disaster:
   - Power lost (systems offline)
   - Backup power activates (limited duration)
   - Cascade failures (systems depending on power fail)
   - Life support at risk (ventilation, heating, medical)
   - Response: restore power, load shedding, backup management
   - Recovery: repair power systems, restore services, prevent recurrence
8. Implement air contamination disaster:
   - Air quality alert (contamination detected)
   - Toxic air (respirator required)
   - Ventilation failure (air not circulating)
   - Health effects (poisoning, asphyxiation)
   - Response: seal area, respirators, ventilation repair
   - Recovery: decontaminate air, repair ventilation, treat exposed
9. Create emergency protocol system:
   - Pre-defined protocols for each disaster type
   - Protocols require resources and personnel
   - Protocols can be customized (assign specific survivors)
   - Protocols have success/failure outcomes
   - Protocols can be chained (evacuation → quarantine → treatment)
10. Implement emergency supply system:
    - Emergency supplies stored in shelter
    - Supplies consumed during disaster response
    - Supplies expire over time (must be rotated)
    - Supply types: medical, fire suppression, radiation kits, food, water
    - Supply shortage reduces response effectiveness
11. Create disaster events:
    - "The Quake" — earthquake strikes shelter
    - "The Flood" — water inundates lower levels
    - "The Fire" — fire breaks out in workshop
    - "The Leak" — radiation leak detected
    - "The Collapse" — room ceiling collapses
    - "The Plague" — disease spreads through shelter
    - "The Blackout" — power fails completely
    - "The Poison" — air becomes toxic
12. Add disaster quest hooks:
    - "The Rescue" — survivors trapped, mount rescue operation
    - "The Containment" — contain radiation leak before it spreads
    - "The Evacuation" — evacuate shelter to safe location
    - "The Recovery" — rebuild after major disaster
    - "The Prevention" — implement measures to prevent future disasters
    - "The Hero" — survivor performs heroic act during disaster
    - "The Sacrifice" — survivor sacrifices self to save others
13. Add UI: emergency panel showing active disasters, response status
14. Create disaster journal: automatic log of disasters and responses
15. Implement disaster tutorial: first disaster explains system
16. Create 15 disaster templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into shelter systems: disasters affect rooms, systems, survivors
2. Connect to `ShelterThermalSystem`: fire/flood affect temperature
3. Integrate with `VentilationSystem`: air contamination, fire smoke
4. Connect to `SumpFloodingSystem`: flood disaster integration
5. Wire into `MedicalPipelineCoordinator`: disaster injuries treated
6. Connect to `SurvivorFateSystem`: disaster casualties processed
7. Implement old-save compatibility: existing saves get empty disaster state
8. Add deterministic seeding: disaster triggers use `ISeededRng`
9. Create exploit prevention: disasters have cooldowns, can't be farmed
10. Add tests: disaster triggering, response mechanics, recovery, save round-trip
11. Verify catalog integrity: all disaster/room/survivor IDs resolve
12. Test edge cases: no disasters (peaceful shelter), constant disasters (apocalypse)
13. Verify headless behavior: disasters process correctly without UI
14. Add data-integrity-selftest: disaster templates validate against room/survivor catalogs
15. Create `--disaster-response-selftest` verb for CI validation

## State / System Interaction Model

```text
Disaster triggers (random or caused)
├─ Warning phase (seconds to hours)
│  ├─ Early warning detected
│  ├─ Player activates emergency protocol
│  ├─ Resources/personnel assigned
│  └─ Shelter prepares (evacuation, lockdown, etc.)
├─ Disaster active
│  ├─ Damage occurs (rooms, systems, survivors)
│  ├─ Player responds (containment, rescue, treatment)
│  ├─ Response reduces severity/duration
│  └─ Failed response: disaster worsens
├─ Disaster contained
│  ├─ Immediate threat ended
│  ├─ Damage assessed
│  ├─ Casualties counted
│  └─ Recovery begins
├─ Recovery phase
│  ├─ Repair damage (rooms, systems)
│  ├─ Treat injured (medical response)
│  ├─ Restore services (power, water, air)
│  └─ Learn from disaster (improve resilience)
└─ Aftermath
   ├─ Shelter resilience modified
   ├─ Survivor morale affected
   ├─ Resources consumed
   └─ Story remembered (journal, epilogue)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --disaster-response-selftest
```

## Risk

**HIGH** — Disaster complexity can overwhelm players if too many disaster types and response options exist. Risk of disasters feeling unfair or unavoidable. Mitigation: provide early warnings, make prevention possible through preparation, keep disaster frequency low (1-2 per year), and ensure recovery is always possible (no unwinnable states).

## Definition of Done

- `DisasterResponseSystem.cs` exists with full `CaptureState/RestoreState`
- 8 disaster types implemented (earthquake, flood, fire, radiation leak, structural failure, disease outbreak, power failure, air contamination)
- Emergency protocol system functional
- Emergency supply management working
- Disaster warning and response mechanics
- Disaster recovery and resilience tracking
- Disaster events and quest hooks
- Save/load round-trip tested
- Deterministic disaster triggers verified
- Old saves load without error
- 15 disaster templates in data authority
- UI panel shows emergency status
- Cross-system integration (shelter systems, medical, survivor fate, thermal, ventilation, flooding)

## Follow-On Opportunities

- Disaster prevention research (unlock better construction, early warning)
- Disaster training (survivors become emergency responders)
- Disaster legacy (famous disasters remembered in shelter history)
- Disaster quests (prevent specific disasters, respond to crises)
- Disaster simulation (train for disasters without real danger)
