# C2 — Flagship Integration Plan [33]: Acute Disasters, Emergency Protocols, Shelter Resilience, and Crisis-Recovery Continuity

> **Deliverable:** `C2_planintegration[33].md`
> **Source scope:** Plan 158 — *Disaster & Emergency Response System*
> **Primary objective:** add a deterministic acute-disaster layer for earthquakes, floods, fires, radiation leaks, structural failures, disease outbreaks, power failures, and air contamination; let the player prepare and activate emergency protocols; and route every physical, medical, environmental, inventory, power, survivor-fate, morale, quest, and recovery consequence through the canonical systems that already own those facts.
> **Required execution order:** **158A Foundation/System Contract → 158B Disasters, Protocols, Supplies, Response & Recovery → 158C Cross-System Integration, Save/CI, Balance, and Closure**
> **Hard dependencies:** canonical shelter structure/room state; `ShelterThermalSystem`; `VentilationSystem`; `SumpFloodingSystem`; power-grid/life-support systems; radiation/decontamination; disease/contamination; `MedicalPipelineCoordinator`; `SurvivorFateSystem`; inventory/resource transactions; duty/assignment; Plan 31 semantic events; Plan 36 port-contract discipline; Plan 39 save durability; Plan 55 retention; Plan 135 weather cascade for weather-triggered disaster eligibility; Plan 138 shelter-boundary systems where evacuation/security intersects.
> **Scope discipline:** no second shelter-condition ledger, no second power/flood/radiation/disease/medical simulation inside `DisasterResponseSystem`, no casualties stored as independent truth before canonical survivor-fate processing, no arbitrary mutable “resilience rating” that can drift from construction/maintenance/training, no emergency supplies duplicated outside inventory unless they represent location/reservation metadata only, no one-off disaster that bypasses deterministic event IDs and idempotency, no catastrophe that becomes unrecoverable purely because a protocol port is missing, and no high-frequency disaster spam that turns the campaign into permanent crisis mode.

---

# 0. Executive Intent

ASHFALL already models slow degradation:

- temperature drift,
- ventilation and air quality,
- water infiltration,
- power load,
- equipment/shelter condition,
- radiation,
- disease,
- medical treatment.

What it does not yet model is **acute discontinuity**:

```text
stable shelter
→ warning
→ sudden crisis
→ active response
→ containment
→ recovery
→ remembered consequence
```

The new system should not become a second copy of every shelter mechanic.

Its job is orchestration.

The target architecture is:

```text
canonical world/shelter conditions
        │
        ├─ weather
        ├─ maintenance
        ├─ power
        ├─ contamination
        ├─ disease
        ├─ construction quality
        └─ sabotage / external causes
        │
        ▼
  DisasterResponseSystem
        │
        ├─ eligibility
        ├─ warning phase
        ├─ protocol state
        ├─ response windows
        ├─ escalation scheduling
        ├─ idempotency
        └─ recovery coordination
        │
        ▼
 typed disaster effect intents
        │
        ├────────► shelter structure
        ├────────► thermal
        ├────────► ventilation
        ├────────► flooding
        ├────────► power
        ├────────► radiation
        ├────────► disease
        ├────────► medical
        ├────────► inventory
        └────────► survivor fate
        │
        ▼
 journal / briefing / quests / epilogue / resilience learning
```

The strongest product-level outcome is:

> **A major shelter emergency feels dramatic because it compresses time and forces immediate prioritization, but it remains fair because warning, preparation, protocol readiness, real supplies, trained personnel, shelter condition, and response choices materially change the outcome.**

---

# 1. Source Diagnosis

The source establishes:

- existing shelter systems are gradual, not acute,
- there is no emergency-response coordinator,
- eight disaster families are proposed:
  - earthquake,
  - flood,
  - fire,
  - radiation leak,
  - structural failure,
  - disease outbreak,
  - power failure,
  - air contamination,
- emergency protocols are proposed:
  - evacuation,
  - lockdown,
  - quarantine,
  - fire suppression,
  - medical emergency,
  - shelter in place,
- emergency resources/personnel are part of response quality,
- disasters require warning, escalation, containment, recovery, and aftermath,
- resilience should improve through preparation/maintenance/training,
- 15 disaster templates are required,
- old saves, deterministic triggering, headless processing, UI, quests, and CI are required,
- risk is explicitly HIGH and source recommends low frequency, early warning, prevention, and recoverability.

The central architectural correction is:

```text
DisasterResponseSystem owns crisis lifecycle
```

while:

```text
room damage belongs to shelter structure
temperature belongs to thermal
air quality belongs to ventilation
water belongs to flooding
power belongs to power grid
radiation belongs to radiation
infection belongs to disease
injuries belong to medical/health
death belongs to SurvivorFate
supplies belong to inventory
```

---

# 2. Program-Level Success Criteria

C2[33] closes only when all of the following are true.

1. Eight disaster types exist as typed, data-authored templates.
2. Disaster eligibility is deterministic and state-backed.
3. Every disaster has a warning/telegraph policy or explicitly documents why warning is absent.
4. Every disaster has active, contained/resolved, and recovery semantics.
5. Emergency protocols require real personnel/resources where specified.
6. Protocol readiness is derived from actual assignments, supplies, training, facilities, and power.
7. Disaster consequences route through canonical owners.
8. No disaster applies the same effect twice after save/load.
9. Recovery clears temporary crisis modifiers while preserving real damage.
10. The player can recover from every supported disaster class if the underlying campaign remains otherwise viable.
11. “Resilience” is derived or decomposed into real contributors rather than stored as a drifting master stat.
12. Disaster frequency is bounded.
13. Emergency supplies are backed by actual inventory transactions.
14. Supply expiry is modeled only where canonical item/perishability systems support it; otherwise use explicit expiration metadata with one owner.
15. Casualties are finalized through `SurvivorFateSystem`.
16. Disease outbreaks use the canonical disease system.
17. Radiation leaks use canonical contamination/radiation systems.
18. Power failures use canonical power-state transitions.
19. Fire/flood/air contamination propagate through real environmental systems.
20. Old saves initialize safely.
21. Headless CI proves trigger → warning → response → containment → recovery.
22. 15 templates validate and become runtime-observed.
23. Constant-disaster stress tests remain bounded and do not duplicate effects.
24. Peaceful/no-disaster scenarios remain fully valid.
25. UI shows exact protocol/resource state used by runtime.

---

# 3. Architectural Invariants

## 3.1 One disaster coordinator

`DisasterResponseSystem` owns:

- disaster instance lifecycle,
- warning and response windows,
- protocol activation,
- escalation scheduling,
- idempotency,
- recovery coordination.

It does not own downstream physical truths.

## 3.2 Trigger cause is explicit

Every disaster has:

```text
cause_id
```

Examples:

- geological,
- weather/flooding,
- electrical fault,
- sabotage,
- poor construction,
- disease import,
- overload,
- external contamination.

No “random disaster” without a recorded deterministic cause.

## 3.3 Consequences are effect intents

The system emits:

```text
apply_structure_damage
inject_flood_input
apply_smoke_load
disable_power_segment
apply_radiation_source
seed_outbreak
apply_medical_injury
consume_supply
```

Owning systems decide final state.

## 3.4 Resilience is a projection

Use:

```text
construction quality
maintenance state
fortification/structural reinforcement
system redundancy
emergency preparedness
training
supply readiness
```

No free-floating `resilience += 5` unless it is a derived historical descriptor.

## 3.5 Protocol readiness is state-backed

A protocol is “ready” only if required:

- people,
- skills,
- equipment,
- supply,
- facilities,
- power/access

exist.

## 3.6 Disaster severity is bounded

No uncapped repeated severity multiplication.

## 3.7 Recovery is explicit

Temporary disaster states have a defined exit.

## 3.8 Acute disaster is not a modal UI mechanic

It progresses headlessly.

## 3.9 All RNG is seeded

Use `ISeededRng`.

## 3.10 Player failure remains legible

If a response fails, the reason is attributable:

- too late,
- insufficient supplies,
- wrong personnel,
- damaged systems,
- poor shelter condition,
- severe event.

---

# 4. Dependency Graph

```text
weather / geology / maintenance / power / disease / sabotage
                          │
                          ▼
                 DisasterResponseSystem
                          │
            ┌─────────────┼───────────────┐
            ▼             ▼               ▼
         warning       protocols       escalation
            │             │               │
            └─────────────┼───────────────┘
                          ▼
                  effect applications
                          │
        ┌─────────────────┼─────────────────────┐
        ▼                 ▼                     ▼
   structure/rooms   environment/life support  survivors
        │                 │                     │
        ├─ thermal        ├─ ventilation        ├─ health
        ├─ flooding       ├─ radiation          ├─ medical
        ├─ power          └─ disease            └─ fate
        │
        ▼
      recovery
        │
        ▼
 journal / quests / completion record / preparedness learning
```

---

# 5. Baseline Capture

Before implementation, record:

- shelter room/state authority,
- structural condition APIs,
- thermal inputs,
- ventilation air-quality APIs,
- sump/flood inputs,
- power-grid failure/load-shedding APIs,
- radiation/decontamination APIs,
- disease outbreak/transmission APIs,
- medical injury/treatment APIs,
- `SurvivorFateSystem` death/casualty path,
- inventory transaction APIs,
- duty/personnel assignment APIs,
- current alert/briefing/panel routes,
- existing emergency-like data/events.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture baseline shelter fixtures:

```text
healthy shelter
poorly maintained shelter
high preparedness shelter
low preparedness shelter
```

---

# 6. Workstream 158A — Foundation / System Contract

## Goal

Create one deterministic acute-crisis lifecycle with data-authored triggers, warnings, protocols, effect intents, idempotency, and recovery orchestration.

---

# 7. 158A Phase A — Create `DisasterResponseSystem`

Path:

```text
Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs
```

Responsibilities:

- evaluate disaster eligibility,
- instantiate disasters,
- manage warning/active/contained/recovery stages,
- evaluate protocol readiness,
- schedule effect waves,
- prevent duplicate application,
- coordinate recovery completion,
- expose read models,
- capture/restore crisis-specific state.

---

# 8. 158A Phase B — Definition vs Runtime Instance

Create:

```text
DisasterTemplateDefinition
DisasterInstance
DisasterEffectDefinition
DisasterEffectApplication
EmergencyProtocolDefinition
EmergencyProtocolInstance
EmergencyResponseAssignment
DisasterRecoveryState
```

Static template data never needs duplication per save beyond IDs.

---

# 9. 158A Phase C — Disaster Type Vocabulary

Typed enum/stable IDs:

```text
Earthquake
Flood
Fire
RadiationLeak
StructuralFailure
DiseaseOutbreak
PowerFailure
AirContamination
```

Do not use free-text switches.

---

# 10. 158A Phase D — Disaster Severity

Typed bands:

```text
Minor
Moderate
Severe
Catastrophic
```

Severity thresholds/multipliers live in data/config.

---

# 11. 158A Phase E — Disaster Lifecycle

Recommended phases:

```text
Eligible
Warning
Preparing
Active
Contained
Recovering
Resolved
FailedClosed
```

Not every template needs every optional subphase.

---

# 12. 158A Phase F — Disaster Instance Fields

Persist:

```text
instance_id
template_id
cause_id
severity
affected_room_ids
started_day/time
warning_deadline
active_phase
next_effect_wave
selected_protocol_ids
response_assignments
effect_application_keys
containment_progress
recovery_state
resolved_day
```

Do not persist canonical room damage/casualty state here.

---

# 13. 158A Phase G — Do Not Persist Casualty List as Authority

The source proposes `casualties`.

Prefer:

```text
casualty outcome references / event IDs
```

after `SurvivorFateSystem` has processed them.

The disaster system may record historical references, not own death truth.

---

# 14. 158A Phase H — `disaster_templates.json`

Create:

```text
Assets/StreamingAssets/Data/disaster_templates.json
```

Fields:

```text
id
disaster_type
cause_types
eligibility
severity rules
warning policy
affected-room selector
effect waves
protocol recommendations
mitigation tags
recovery policy
cooldown
localization keys
quest hooks
```

---

# 15. 158A Phase I — Trigger Evaluation

Inputs can include:

- weather event/state,
- structural condition,
- maintenance debt,
- power overload,
- contamination state,
- disease state,
- visitor infection,
- sabotage/world events,
- campaign day,
- cooldowns.

All state-backed.

---

# 16. 158A Phase J — Earthquake Trigger

Source says random modified by geology/weather.

Clarify:

- use authored geological stability/region hazard if available,
- weather may affect secondary failure, not necessarily earthquake incidence unless fiction/data supports it.

Do not invent scientifically dubious coupling merely because source says “weather.”

If no geology system exists:

```text
use region hazard template
```

as content authority.

---

# 17. 158A Phase K — Flood Trigger

Use:

```text
heavy precipitation/weather
+ sump/drainage weakness
+ shelter location/level vulnerability
```

Avoid duplicate water simulation.

---

# 18. 158A Phase L — Fire Trigger

Candidate causes:

- electrical fault,
- cooking accident,
- workshop accident,
- arson,
- lightning/external ignition.

Each cause links to real state where possible.

---

# 19. 158A Phase M — Radiation Leak Trigger

Causes:

- equipment damage,
- containment failure,
- sabotage,
- external breach.

Do not create radiation source unless radiation system can represent it.

---

# 20. 158A Phase N — Structural Failure Trigger

Inputs:

- condition,
- over-expansion,
- poor construction,
- prior damage,
- seismic stress.

---

# 21. 158A Phase O — Disease Outbreak Trigger

Must originate from:

- infected survivor/visitor,
- sanitation failure,
- known pathogen mutation/event,
- contamination exposure.

No disease from pure random disaster roll disconnected from DiseaseSystem.

---

# 22. 158A Phase P — Power Failure Trigger

Use actual:

- overload,
- equipment fault,
- sabotage,
- external grid/source loss.

PowerGrid remains authority.

---

# 23. 158A Phase Q — Air Contamination Trigger

Use:

- filter failure,
- smoke,
- external toxic contamination,
- chemical spill,
- ventilation failure.

Ventilation owns air quality.

---

# 24. 158A Phase R — Dedicated RNG Stream

Use:

```text
disaster_response
```

Stable choice inputs:

```text
campaign seed
day/time bucket
template id
cause id
target room/region
```

Candidate lists sorted.

---

# 25. 158A Phase S — Disaster Frequency Budget

Source risk proposes:

```text
~1–2 disasters/year
```

Treat as initial major-disaster budget.

Also distinguish:

```text
major disasters
minor incidents
```

Avoid generating eight “major” disasters at equal cadence.

---

# 26. 158A Phase T — Global Disaster Cooldown

Persist:

- last major disaster day,
- rolling disaster burden,
- per-template cooldown.

---

# 27. 158A Phase U — Concurrent Disaster Policy

Initial:

```text
one major active disaster
```

Secondary cascades may occur as effect consequences.

Do not spawn independent full disasters unless authored and budget permits.

---

# 28. 158A Phase V — Warning Policy

Each template declares:

```text
none
seconds/minutes
hours
days
```

in game-time semantics.

Warnings can depend on sensors/intel/maintenance.

---

# 29. 158A Phase W — Warning Quality

Derived from:

- sensors,
- trained personnel,
- maintenance inspections,
- weather station,
- power availability,
- known structural monitoring.

Better preparedness can increase warning lead time/accuracy.

---

# 30. 158A Phase X — Surprise Policy

Some crises may have low/no warning:

- sudden earthquake,
- electrical fire,
- collapse.

But player preparation must still matter through:

- resilience,
- protocols,
- supplies,
- training.

---

# 31. 158A Phase Y — Emergency Protocol Definitions

Create data/typed protocol definitions:

```text
Evacuation
Lockdown
Quarantine
FireSuppression
MedicalEmergency
ShelterInPlace
PowerEmergency
RadiationContainment
FloodResponse
AirSafety
StructuralRescue
```

Source six protocols can remain core; specialized protocols may be subtypes/actions.

---

# 32. 158A Phase Z — Protocol Readiness

A protocol reads:

- trained personnel,
- assigned personnel,
- supplies,
- facilities,
- access,
- power,
- route safety.

Ready state is derived.

---

# 33. 158A Phase AA — Protocol Activation

Player activates:

```text
protocol instance
```

with assigned personnel/resources.

Activation commits a real plan.

---

# 34. 158A Phase AB — Personnel Assignment

Use canonical duty/assignment.

No disaster-only survivor availability list.

---

# 35. 158A Phase AC — Personnel Skill

Response quality may use:

- medical,
- repair,
- engineering,
- leadership,
- survival,
- combat/security

through canonical skill system.

---

# 36. 158A Phase AD — Resource Reservation

Emergency supplies are actual inventory.

Protocol can reserve quantities to avoid double-spend.

Use inventory reservation/transaction contract if available.

---

# 37. 158A Phase AE — Emergency Supply Model

Avoid duplicate quantity truth.

A supply DTO may represent:

```text
item/category
storage location
reserved quantity
expiry metadata
emergency designation
```

Actual count remains inventory.

---

# 38. 158A Phase AF — Supply Expiry

If perishability/expiry exists:

- use it.

If not:

- only implement expiry for items whose content explicitly supports it,
- or defer generalized expiry.

Do not invent a second perishability system.

---

# 39. 158A Phase AG — Protocol Success Assessment

Response effectiveness:

```text
personnel competence
+ supply completeness
+ warning/preparation
+ facility state
+ access
+ disaster severity
+ resilience contributors
```

Prefer deterministic formula.

If uncertainty remains, use seeded RNG once per response phase.

---

# 40. 158A Phase AH — Shelter Resilience Projection

Create:

```text
ShelterResilienceSnapshot
```

Contributors:

- structural condition,
- construction quality,
- redundancy,
- maintenance,
- emergency training,
- supply readiness,
- backup systems.

No independent saved scalar as truth.

---

# 41. 158A Phase AI — Resilience Learning

Source says “learn from disaster.”

Translate to real consequences:

- unlocked preparedness research,
- improved protocol training,
- permanent structural upgrades,
- documented procedure.

Not:

```text
resilience += 10
```

unless explicitly derived.

---

# 42. 158A Phase AJ — Effect Application IDs

Key:

```text
disaster_instance
effect_wave
effect_definition
target
```

Prevents duplicate damage after reload.

---

# 43. 158A Phase AK — Recovery State

Persist coordination state only:

- unresolved repairs,
- services awaiting restoration,
- trapped-survivor rescue IDs,
- active cleanup tasks,
- protocol closure.

Actual damage/health/contamination remains canonical.

---

# 44. 158A Phase AL — Old Save Compatibility

Missing section:

```text
valid
→ no active disasters
→ cooldown initialized safely
```

Do not trigger a disaster immediately on first post-migration tick solely because no history exists.

---

# 45. 158A Phase AM — Legacy Grace Window

For old saves:

```text
short major-disaster grace period
```

may be appropriate to prevent migration ambush.

Data/config, tested.

---

# 46. 158A Phase AN — Semantic Events

Candidate kinds:

```text
disaster_warning
emergency_protocol_activated
disaster_effect_applied
disaster_contained
disaster_recovery_started
disaster_resolved
survivor_trapped
survivor_rescued
emergency_supply_shortage
```

Use Plan 31 governance.

---

# 47. 158A Phase AO — Port Contract

Required downstream sinks:

```text
shelter structure
thermal
ventilation
flooding
power
radiation/decon
disease
medical
survivor fate
inventory
duty/personnel
quest/journal/completion
```

Missing required sink fails validation.

---

# 48. 158A Phase AP — Diagnostics

Expose:

```text
DISASTER_TEMPLATES
ACTIVE_DISASTERS
ACTIVE_PROTOCOLS
DISASTER_EFFECTS_APPLIED
DISASTER_REQUIRED_PORTS_MISSING
MAJOR_DISASTER_COOLDOWN
```

---

# 49. 158A Tests

- deterministic trigger,
- frequency budget,
- warning,
- protocol readiness,
- real inventory reservation,
- derived resilience,
- idempotency,
- old save,
- legacy grace,
- missing-port failure,
- recovery state.

---

# 50. 158A Definition of Done

- [ ] DisasterResponseSystem,
- [ ] definition/instance split,
- [ ] 8 disaster types,
- [ ] 4 severity bands,
- [ ] lifecycle,
- [ ] data authority,
- [ ] deterministic trigger model,
- [ ] frequency/cooldowns,
- [ ] warning policy,
- [ ] protocol definitions,
- [ ] real personnel/resources,
- [ ] inventory-backed emergency supplies,
- [ ] resilience projection,
- [ ] effect idempotency,
- [ ] recovery state,
- [ ] old-save/grace policy,
- [ ] semantic events,
- [ ] required ports,
- [ ] diagnostics.

---

# 51. Workstream 158B — Disasters, Protocols, Supplies, Response & Recovery

## Goal

Implement distinct mechanics for all eight disaster families and 15 templates while preserving canonical state ownership and giving the player meaningful preparation/response/recovery decisions.

---

# 52. 158B Phase A — 15-Template Content Budget

Start with exactly:

```text
15 disaster templates
```

Suggested distribution:

- 2 earthquake,
- 2 flood,
- 2 fire,
- 2 radiation leak,
- 2 structural failure,
- 2 disease outbreak,
- 1 power failure,
- 2 air contamination,

or another balanced distribution after repository audit.

Do not create 15 nearly identical severity variants.

---

# 53. 158B Phase B — Earthquake: Warning

Possible telegraph:

- tremor sensor,
- sound,
- structural monitoring,
- regional hazard alert.

Warning may be short.

---

# 54. 158B Phase C — Earthquake: Effect Waves

Wave examples:

```text
initial shake
aftershock 1
aftershock 2
```

Each applies structural stress through shelter authority.

---

# 55. 158B Phase D — Earthquake: Survivor Injury

Injury risk from:

- room collapse,
- falling debris,
- location.

Medical/health systems own injuries.

---

# 56. 158B Phase E — Earthquake: Response

Protocols:

- shelter in place,
- evacuate unsafe rooms,
- structural assessment,
- rescue trapped.

---

# 57. 158B Phase F — Earthquake: Recovery

- repair,
- reopen rooms,
- treat injuries,
- reinforce.

No instant condition reset.

---

# 58. 158B Phase G — Flood: Warning

Use:

- weather forecast,
- rising sump level,
- drainage alarm.

Hours of lead time when supported.

---

# 59. 158B Phase H — Flood: Electrical Hazard

Flooded/elevated water may require:

```text
cut power to affected segment
```

Power system owns state.

---

# 60. 158B Phase I — Flood: Contamination

Water contamination routes into:

- disease/contamination,
- water purification.

No duplicate “dirty flood” stat.

---

# 61. 158B Phase J — Flood: Evacuation

Move survivors out of affected rooms via canonical location/room assignment.

---

# 62. 158B Phase K — Flood: Response

- pumps,
- barriers/sandbags,
- evacuation,
- water purification.

Pumps use actual power/capacity.

---

# 63. 158B Phase L — Flood: Recovery

- drain/pump remaining water,
- repair,
- decontaminate,
- restore power.

---

# 64. 158B Phase M — Fire: Ignition

Choose room based on cause/template.

Do not spawn in invalid/nonexistent room.

---

# 65. 158B Phase N — Fire: Spread

Adjacency comes from canonical room/layout graph.

If no adjacency graph exists:

- add explicit room adjacency source as prerequisite,
- do not fake spread by random room ID.

---

# 66. 158B Phase O — Fire: Smoke

Ventilation receives smoke/air-quality load.

---

# 67. 158B Phase P — Fire: Oxygen Risk

Use ventilation/air system if oxygen is modeled.

If not:

- use smoke/toxic-air exposure rather than inventing a second oxygen meter.

---

# 68. 158B Phase Q — Fire: Suppression

Consumes real:

- extinguisher/suppression items,
- water,
- personnel time.

---

# 69. 158B Phase R — Fire: Recovery

- burn treatment,
- room repair,
- item loss accounting,
- ventilation cleanup.

---

# 70. 158B Phase S — Radiation Leak: Detection

Use:

- Geiger/radiation sensor,
- room contamination readings.

---

# 71. 158B Phase T — Radiation Leak: Source

There must be an actual source:

- damaged equipment,
- containment breach,
- external leak.

Radiation system owns contamination field/source.

---

# 72. 158B Phase U — Radiation Leak: Response

- seal room,
- evacuate,
- respirators/PPE,
- decontaminate,
- repair source.

---

# 73. 158B Phase V — Radiation Leak: Recovery

Do not clear contamination until Decontamination/Radiation systems say it is reduced.

---

# 74. 158B Phase W — Structural Failure: Warning

Clues:

- cracks,
- sounds,
- stress readings,
- prior damage.

---

# 75. 158B Phase X — Structural Failure: Collapse

Canonical structure applies:

- room closure,
- damage,
- debris/trapped state if supported.

---

# 76. 158B Phase Y — Trapped Survivors

Represent via canonical survivor location/status.

Disaster system tracks rescue objective reference.

No duplicate “trapped survivor list” as sole truth.

---

# 77. 158B Phase Z — Structural Rescue

Requires:

- personnel,
- tools,
- time,
- structural safety.

Failure risk bounded/seeded.

---

# 78. 158B Phase AA — Disease Outbreak: Detection

Disease system detects:

- symptomatic cases,
- test result,
- contact spread.

Disaster coordinator promotes it to acute emergency once threshold met.

---

# 79. 158B Phase AB — Disease Outbreak: Quarantine

Use canonical quarantine/isolation if available.

Otherwise define one host-level quarantine port rather than disease-specific shadow roster.

---

# 80. 158B Phase AC — Disease Outbreak: Medical Response

- treatment,
- sanitation,
- vaccination if actual item/system exists.

Do not invent vaccines for pathogens lacking them.

---

# 81. 158B Phase AD — Disease Outbreak: Recovery

Outbreak ends when disease system reports controlled conditions.

---

# 82. 158B Phase AE — Power Failure: Trigger

PowerGrid/system reports outage/failure.

Disaster system coordinates emergency response.

---

# 83. 158B Phase AF — Backup Power

Use actual backup/battery/generator system.

No disaster-only backup timer.

---

# 84. 158B Phase AG — Load Shedding

Use Plan 23 canonical priority tiers.

Emergency protocol can request load-shedding policy.

---

# 85. 158B Phase AH — Life-Support Cascade

Power loss impacts:

- ventilation,
- heating,
- medical,
- pumping

through their existing dependencies.

Do not separately subtract health in disaster code merely because power failed.

---

# 86. 158B Phase AI — Power Recovery

Repair/restore through power authority.

---

# 87. 158B Phase AJ — Air Contamination: Detection

Ventilation/air sensors report:

- toxic load,
- smoke,
- external contamination.

---

# 88. 158B Phase AK — Air Contamination: PPE

Respirator/protection uses actual equipment.

---

# 89. 158B Phase AL — Air Contamination: Sealing

Room/ventilation isolation uses existing dampers/doors/airflow if modeled.

Otherwise implement minimal explicit isolation control in ventilation authority.

---

# 90. 158B Phase AM — Air Contamination: Recovery

Ventilation/decon clears air.

Medical treats exposed survivors.

---

# 91. 158B Phase AN — Emergency Protocol Chaining

Source proposes:

```text
evacuation → quarantine → treatment
```

Implement explicit dependency graph.

Avoid arbitrary recursion.

---

# 92. 158B Phase AO — Protocol Preconditions

Each step declares:

- required state,
- personnel,
- supply,
- facility,
- preceding protocol.

---

# 93. 158B Phase AP — Protocol Customization

Player may select personnel.

System should:

- show suitability,
- warn about critical-duty conflicts,
- not auto-reassign silently unless explicit.

---

# 94. 158B Phase AQ — Protocol Automation

Optional:

```text
preconfigured emergency plan
```

may assign defaults when crisis hits.

Still confirm or execute according to user settings/design.

No hidden full automation that removes decisions unless intended.

---

# 95. 158B Phase AR — Emergency Supply Categories

Source categories:

```text
medical
fire_suppression
radiation_kit
emergency_food
water_purification
```

Map to actual item tags/categories.

---

# 96. 158B Phase AS — Supply Location

If room/storage location matters:

- track reservation/location metadata.

If inventory is not spatial:

- do not fabricate false room-level supply location.

---

# 97. 158B Phase AT — Supply Rotation

Only implement expiration/rotation if item perishability supports it.

Alternative:

- periodic preparedness inspection that checks minimum stock.

---

# 98. 158B Phase AU — Emergency Stockpile Policy

Allow player to define reserve targets.

Inventory authority remains item owner.

A reserve is:

```text
policy/threshold
```

not hidden duplicated quantity.

---

# 99. 158B Phase AV — Preparedness Drill

Source follow-on mentions training.

Within current scope, implement minimal protocol-readiness training if supported:

- survivor emergency qualification,
- protocol familiarization.

Do not create a giant separate training tree.

---

# 100. 158B Phase AW — Disaster Events

Source examples:

```text
The Quake
The Flood
The Fire
The Leak
The Collapse
The Plague
The Blackout
The Poison
```

Use event templates/localization.

---

# 101. 158B Phase AX — Quest Hooks

Source:

```text
The Rescue
The Containment
The Evacuation
The Recovery
The Prevention
The Hero
The Sacrifice
```

Use canonical quest runtime.

---

# 102. 158B Phase AY — “The Sacrifice” Guardrail

A survivor death/sacrifice must:

- be player choice/autonomous state-backed decision,
- route through SurvivorFate,
- never be silently forced by flavor text.

---

# 103. 158B Phase AZ — Emergency Panel

Show:

```text
disaster
severity
warning time
affected rooms
active effects
recommended protocols
protocol readiness
assigned personnel
supply gaps
containment/recovery status
```

No hidden RNG.

---

# 104. 158B Phase BA — Response Queue

For multi-step emergencies:

- show next required action,
- progress,
- blockers.

Avoid 20 simultaneous buttons.

---

# 105. 158B Phase BB — Critical Alert

Major disaster warning uses:

- alert,
- briefing,
- audio/visual parity.

No audio-only emergency.

---

# 106. 158B Phase BC — Disaster Journal

Record significant phases:

- warning,
- response chosen,
- containment,
- casualties,
- recovery,
- major lessons.

Do not log every effect tick.

---

# 107. 158B Phase BD — Tutorial

First minor/moderate disaster explains:

- warning,
- protocol,
- personnel,
- supplies,
- recovery.

Do not introduce tutorial with catastrophic no-win event.

---

# 108. 158B Phase BE — 15-Template Coverage Matrix

Generate:

| Template | Type | Cause | Severity | Warning | Protocols | Main Effects | Recovery | Runtime observed |
|---|---|---|---|---|---|---|---|---:|

---

# 109. 158B Phase BF — Content Utilization

Run seeded long scenarios.

Report:

```text
templates eligible
warnings generated
protocols activated
contained
escalated
recovered
never observed
```

---

# 110. 158B Phase BG — Dead Template Policy

Never-observed template:

- fix trigger,
- mark rare,
- remove,
- or exempt with reason.

---

# 111. 158B Definition of Done

- [ ] 15 disaster templates,
- [ ] earthquake,
- [ ] flood,
- [ ] fire,
- [ ] radiation leak,
- [ ] structural failure,
- [ ] disease outbreak,
- [ ] power failure,
- [ ] air contamination,
- [ ] protocol chaining,
- [ ] real personnel requirements,
- [ ] inventory-backed emergency supplies,
- [ ] stockpile policy,
- [ ] warning/preparation,
- [ ] recovery mechanics,
- [ ] disaster events,
- [ ] quest hooks,
- [ ] emergency panel,
- [ ] journal,
- [ ] tutorial,
- [ ] utilization report.

---

# 112. Workstream 158C — Cross-System Integration, Save/CI, Balance, and Closure

## Goal

Prove every acute disaster acts through existing shelter/medical/environment/fate authorities, remains deterministic and idempotent, recovers correctly, and produces rare high-value crisis moments rather than unfair or repetitive punishment.

---

# 113. 158C Phase A — Shelter Structure Integration

Earthquake/fire/collapse effect intents must update the canonical room/structure authority.

Assert:

```text
room condition change visible to all consumers
```

---

# 114. 158C Phase B — Thermal Integration

Fire may increase local heat.
Flood may reduce heating/thermal stability.
Power failure may remove heat source.

ThermalSystem computes final temperature.

---

# 115. 158C Phase C — Ventilation Integration

Fire smoke and air contamination feed ventilation.

Power failure affects ventilation through power dependency.

---

# 116. 158C Phase D — SumpFlooding Integration

Flood disaster injects water pressure/level through SumpFloodingSystem.

Do not maintain a second flood meter.

---

# 117. 158C Phase E — Medical Integration

Disaster injuries:

```text
burn
trauma
crush injury
smoke exposure
radiation exposure
poisoning
infection
```

must map to existing medical/affliction vocabulary where possible.

If an injury type is missing:

- add canonical medical definition,
- not disaster-only health subtraction.

---

# 118. 158C Phase F — Survivor Fate Integration

Death/casualty finalization uses `SurvivorFateSystem`.

Disaster history stores cause references.

---

# 119. 158C Phase G — Radiation Integration

Radiation leak creates actual radiation/contamination state.

Decontamination/treatment removes through canonical systems.

---

# 120. 158C Phase H — Disease Integration

Outbreak lifecycle reads canonical disease state.

Quarantine/treatment are real interventions.

---

# 121. 158C Phase I — Power Integration

Power failure/disaster response uses:

- canonical power authority,
- backup,
- load shedding,
- repair.

---

# 122. 158C Phase J — Inventory Integration

Emergency response consumes actual items.

Use transactional APIs.

---

# 123. 158C Phase K — Duty/Personnel Integration

Assigned responders are unavailable for conflicting duties according to real scheduler.

---

# 124. 158C Phase L — Location/Room Integration

Evacuation physically changes survivor room/location assignments.

No “evacuated = true” without location consequence.

---

# 125. 158C Phase M — Quest Integration

Disaster quests:

- appear once,
- preserve accepted state,
- expire/resolve correctly,
- do not duplicate on reload.

---

# 126. 158C Phase N — Morale/Relations Integration

Disaster aftermath may affect morale through:

- casualties,
- successful rescue,
- failed response,
- heroic action.

Use canonical morale/relations/voice systems.

---

# 127. 158C Phase O — Completion/Epilogue Integration

Landmark events:

- major disaster survived,
- catastrophic shelter loss,
- famous rescue,
- failed evacuation,
- rebuilt shelter.

Write through canonical completion record.

---

# 128. 158C Phase P — Save/Load Lifecycle Matrix

Test save/load at:

```text
warning
preparation
active wave
mid-response
contained
recovery
resolved
```

Persist exact phase.

---

# 129. 158C Phase Q — No Double Damage

Reload mid-disaster cannot reapply:

- collapse,
- fire damage,
- radiation source,
- casualty,
- supply consumption.

---

# 130. 158C Phase R — Protocol Idempotency

Reload cannot duplicate:

- personnel assignment,
- resource reservation,
- completion reward.

---

# 131. 158C Phase S — Old Save Compatibility

Old save:

```text
no disaster state
```

loads cleanly.

Grace policy prevents immediate unfair major disaster.

---

# 132. 158C Phase T — Disaster Farming Prevention

Player cannot:

- reload for a different disaster,
- reroll room target,
- reroll aftershock,
- reroll response outcome,
- duplicate recovery rewards.

---

# 133. 158C Phase U — Peaceful Shelter Edge Case

Run years with no eligible disasters.

Expected:

- system stable,
- no forced event,
- no stale protocol state.

---

# 134. 158C Phase V — Constant Disaster Stress Case

Force/high-frequency fixture.

Goal:

- no duplicate effects,
- no memory leak,
- cooldown/attention budget enforce,
- recovery queues remain coherent.

Not a normal balance mode.

---

# 135. 158C Phase W — No Supplies Edge Case

Protocols can still be attempted when sensible.

Response effectiveness falls.

Do not hard-lock every emergency if improvisation is plausible.

---

# 136. 158C Phase X — No Trained Personnel Edge Case

Likewise:

- reduced effectiveness,
- longer response,
- greater risk.

Maturation/education/training plans can improve future readiness.

---

# 137. 158C Phase Y — All Systems Damaged Edge Case

A disaster affecting already-degraded shelter should be severe but recoverable if campaign resources/people remain.

Avoid “no valid response” dead-end unless game-over contract explicitly applies.

---

# 138. 158C Phase Z — Evacuation Edge Case

If full shelter evacuation is not yet supported:

- support room-level/local evacuation,
- gate full-shelter evacuation quest behind world/shelter-relocation prerequisite.

Do not fake a full relocation.

---

# 139. 158C Phase AA — Fire Spread Graph Integrity

If room adjacency exists:

- validate no invalid room references,
- stable spread order.

If absent:

- feature remains blocked until canonical adjacency exists.

---

# 140. 158C Phase AB — Disease Quarantine Integrity

Quarantine state survives load.
No duplicated isolation.

---

# 141. 158C Phase AC — Supply Expiry Integrity

If expiry exists:

- expired supplies cannot satisfy protocol,
- rotation replaces via real inventory.

---

# 142. 158C Phase AD — Recovery Completeness

A disaster is not “resolved” until:

- immediate hazard ended,
- temporary modifiers cleared,
- unresolved persistent damage handed off,
- response assignments released,
- reserved supplies reconciled.

---

# 143. 158C Phase AE — Recovery Does Not Auto-Heal Damage

Resolved crisis ≠ repaired shelter.

Persistent:

- room damage,
- survivor injuries,
- contamination,
- missing items

remain until their systems resolve them.

---

# 144. 158C Phase AF — Resilience Learning Integrity

Preparedness improvements arise from:

- real upgrades,
- training,
- maintenance,
- stored supplies,
- procedures.

No invisible permanent stat inflation.

---

# 145. 158C Phase AG — `--disaster-response-selftest`

Required scenarios:

1. deterministic earthquake,
2. warned flood with pump response,
3. fire with suppression,
4. radiation leak with containment,
5. structural collapse/rescue,
6. disease outbreak/quarantine,
7. power failure/load shedding,
8. air contamination/ventilation repair,
9. old save,
10. save/load mid-disaster,
11. no-disaster case,
12. constant-disaster stress case,
13. no-supplies case,
14. no-trained-personnel case,
15. full recovery closure.

---

# 146. 158C Phase AH — Data Integrity

Validate:

- 15 template IDs,
- disaster types,
- room selectors,
- protocol refs,
- item/category refs,
- skill refs,
- quest refs,
- localization,
- cooldowns,
- severity bounds,
- recovery policies.

---

# 147. 158C Phase AI — Deliberate Failure Proof

Break:

- missing room target,
- missing protocol,
- duplicate effect application key,
- invalid supply category,
- missing required port.

Assert CI/selftest fails.

---

# 148. 158C Phase AJ — Long-Run Disaster Soak

Run long seeded horizon.

Measure:

```text
major disasters/year
warnings
protocol activations
contained vs escalated
casualties
resource cost
repair burden
recovery duration
```

---

# 149. 158C Phase AK — Frequency Guard

Source target:

```text
~1–2 major disasters/year
```

Measure actual distribution.

Avoid clustered back-to-back major events unless scenario explicitly causes them.

---

# 150. 158C Phase AL — Prepared vs Unprepared Profiles

Run:

```text
prepared
average
neglectful
```

Compare:

- damage,
- casualties,
- duration,
- supply use,
- recovery time.

Preparedness should materially help.

---

# 151. 158C Phase AM — Protocol Value Test

Each major protocol must have at least one scenario where it improves outcome.

No decorative protocol.

---

# 152. 158C Phase AN — Resilience Contributor Test

Improve one contributor at a time:

- structure,
- redundancy,
- supplies,
- training,
- maintenance.

Verify expected disaster mitigation changes.

---

# 153. 158C Phase AO — Fairness Test

For each disaster:

```text
what could the player know?
what could they prepare?
what response could they choose?
what consequence follows failure?
```

No opaque arbitrary punishment.

---

# 154. 158C Phase AP — Attention Budget

One major crisis should consolidate alerts.

Use:

- single critical alert,
- emergency panel,
- briefing updates.

Do not spawn one modal per subsystem effect.

---

# 155. 158C Phase AQ — Audio/Visual Parity

Alarms may reinforce crisis.

But every critical state must have:

- text,
- icon/status,
- accessible explanation.

---

# 156. 158C Phase AR — Accessibility

Emergency panel:

- keyboard navigation,
- controller if supported,
- text severity,
- no color-only warnings,
- explicit timers,
- scalable text.

---

# 157. 158C Phase AS — UI Runtime Parity

Displayed:

- warning duration,
- supply requirement,
- personnel readiness,
- containment progress

must match runtime values.

---

# 158. 158C Phase AT — Headless Lifecycle

No panel required for:

- warning,
- effect waves,
- protocol timers,
- containment,
- recovery.

---

# 159. 158C Phase AU — Retention

Plan 55 policy:

Keep:

- recent disaster details,
- landmark disasters,
- casualties/rescues,
- major resilience upgrades.

Roll up:

- minor incident ticks,
- routine protocol steps.

---

# 160. 158C Phase AV — Epilogue / Legacy

Possible landmark summaries:

- survived the quake,
- rebuilt after the flood,
- contained the leak,
- plague survivor,
- blacked-out winter,
- famous rescue.

Use canonical completion/epilogue authority.

---

# 161. 158C Phase AW — Human Playtest

Evaluate:

```text
Was the warning understandable?
Could preparation matter?
Did protocol choices feel distinct?
Was failure attributable?
Did recovery feel achievable?
Did the crisis overstay its welcome?
```

Human review for drama/fairness.

---

# 162. 158C Phase AX — Documentation

Create:

```text
docs/systems/DISASTER_RESPONSE.md
```

Include:

- authority boundaries,
- trigger model,
- lifecycle,
- protocols,
- supplies,
- resilience projection,
- idempotency,
- recovery,
- save behavior,
- adding templates.

---

# 163. 158C Definition of Done

- [ ] shelter structure integration,
- [ ] thermal integration,
- [ ] ventilation integration,
- [ ] sump flooding integration,
- [ ] medical integration,
- [ ] survivor fate integration,
- [ ] radiation integration,
- [ ] disease integration,
- [ ] power integration,
- [ ] inventory integration,
- [ ] duty/personnel integration,
- [ ] room evacuation integration,
- [ ] quest/morale/epilogue integration,
- [ ] save/load lifecycle matrix,
- [ ] no double damage,
- [ ] protocol idempotency,
- [ ] old-save grace,
- [ ] anti-farming,
- [ ] no-disaster edge case,
- [ ] constant-disaster stress,
- [ ] no-supply/no-personnel cases,
- [ ] evacuation gating,
- [ ] recovery completeness,
- [ ] selftest,
- [ ] failure proof,
- [ ] long-run soak,
- [ ] frequency guard,
- [ ] prepared/unprepared comparison,
- [ ] protocol value test,
- [ ] fairness test,
- [ ] accessibility,
- [ ] UI/runtime parity,
- [ ] retention,
- [ ] epilogue,
- [ ] playtest,
- [ ] docs.

---

# 164. Integrated Disaster Pipeline

```text
canonical cause state
    │
    ▼
disaster eligibility
    │
    ▼
warning
    │
    ├─ assign personnel
    ├─ reserve supplies
    ├─ activate protocol
    └─ evacuate/prepare
    │
    ▼
active disaster
    │
    ├─ effect wave 1
    ├─ player response
    ├─ effect wave 2
    └─ escalation / containment
    │
    ▼
contained
    │
    ▼
recovery coordination
    │
    ├─ repairs
    ├─ treatment
    ├─ cleanup
    ├─ service restoration
    └─ rescue
    │
    ▼
resolved
    │
    ├─ history
    ├─ journal
    ├─ quests
    └─ preparedness improvements
```

---

# 165. Disaster Authority Contract

DisasterResponseSystem owns:

```text
crisis identity
phase
warning
protocol state
effect-wave schedule
idempotency
recovery coordination
```

Nothing else.

---

# 166. Shelter State Contract

Room condition/damage stays canonical in shelter systems.

---

# 167. Thermal Contract

Fire/flood/power alter thermal inputs.

ThermalSystem owns final temperature.

---

# 168. Ventilation Contract

Smoke/toxic air/vent failure feed VentilationSystem.

No disaster-owned air-quality number.

---

# 169. Flood Contract

Flood effects use SumpFloodingSystem and canonical water state.

---

# 170. Radiation Contract

Radiation leak uses Radiation/Contamination systems.

No disaster-only dose.

---

# 171. Disease Contract

Disease outbreak uses DiseaseSystem.

No disaster-only infection count.

---

# 172. Power Contract

Power failures and backup use PowerGrid.

No disaster-only power boolean.

---

# 173. Medical Contract

Injuries become canonical afflictions/treatment needs.

---

# 174. Fate Contract

Casualties become deaths only through SurvivorFateSystem.

---

# 175. Inventory Contract

Emergency supplies are actual items.

Protocol reservations do not duplicate quantity.

---

# 176. Personnel Contract

Emergency responders are real survivors with actual assignments and fitness.

---

# 177. Protocol Contract

A protocol is:

```text
trigger
preconditions
resources
personnel
steps
success criteria
fallback
```

---

# 178. Resilience Contract

Resilience is a read-model projection of real preparedness.

---

# 179. Warning Contract

Every disaster template explicitly declares:

```text
warning source
lead time
confidence
```

or “none.”

---

# 180. Idempotency Contract

Effect key:

```text
disaster
wave
effect
target
```

applies once.

---

# 181. Recovery Contract

Temporary crisis modifiers have explicit recovery.

Persistent consequences remain with canonical owners.

---

# 182. Save Contract

Persist:

- active disaster instances,
- phase/timing,
- protocol state,
- effect idempotency,
- recovery coordination,
- cooldowns.

Do not persist duplicates of downstream state.

---

# 183. Old-Save Contract

Missing disaster state is valid.

No immediate post-migration catastrophe.

---

# 184. Frequency Contract

Major disasters are rare.

Source baseline:

```text
1–2/year
```

subject to scenario/world state.

---

# 185. Fairness Contract

A fair disaster has one or more of:

- warning,
- prevention,
- preparedness,
- mitigation,
- response,
- recovery.

No disaster should be “random HP tax” disguised as drama.

---

# 186. Content Acceptance Contract

Templates progress through:

```text
AUTHORED
→ LOADS
→ ELIGIBLE
→ WARNING_PRODUCED
→ EFFECT_PRODUCED
→ RESPONSE_CONSUMED
→ RECOVERY_COMPLETED
→ PLAYER_VISIBLE
```

---

# 187. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| disaster coordinator duplicates every shelter subsystem | High | Critical | strict port/effect-intent architecture |
| major disasters fire too often | High | High | global budget + per-template cooldown |
| disaster feels unfair | High | High | warning/prep/recovery contracts |
| save/load repeats damage | Medium | Critical | effect idempotency keys |
| “resilience” becomes opaque magic stat | Medium | High | projection-only snapshot |
| emergency supplies duplicate inventory | Medium | High | reservation/category metadata only |
| casualty list drifts from survivor state | Medium | High | SurvivorFate owns death |
| full evacuation unsupported | Medium | High | room evacuation + explicit prerequisite |
| fire spread lacks room adjacency | Medium | High | canonical graph prerequisite |
| disease disaster duplicates disease system | Medium | High | threshold/orchestration only |
| constant-disaster stress causes runaway queues | Medium | High | one-major policy + attention budget |
| recovery clears real damage accidentally | Medium | Critical | persistent-vs-temporary separation |

---

# 188. Commit Strategy

## 158A — Foundation

### C2[33].1 — baseline + disaster-response ADR

### C2[33].2 — disaster/protocol/effect DTOs

### C2[33].3 — disaster_templates.json schema

### C2[33].4 — deterministic trigger/cause/frequency model

### C2[33].5 — warning/preparation lifecycle

### C2[33].6 — protocol/personnel/supply reservation

### C2[33].7 — resilience projection

### C2[33].8 — effect-idempotency/recovery/save

### C2[33].9 — ports/events/diagnostics

### Gate: 158A complete

---

## 158B — Disaster Mechanics / Content

### C2[33].10 — earthquake templates

### C2[33].11 — flood templates

### C2[33].12 — fire templates

### C2[33].13 — radiation-leak templates

### C2[33].14 — structural-failure templates

### C2[33].15 — disease-outbreak templates

### C2[33].16 — power/air-contamination templates

### C2[33].17 — protocol chaining / emergency stockpile

### C2[33].18 — quests/events/UI/journal/tutorial

### C2[33].19 — content-utilization report

### Gate: 158B complete

---

## 158C — Closure

### C2[33].20 — shelter/thermal/ventilation/flood integration

### C2[33].21 — radiation/disease/power integration

### C2[33].22 — medical/fate/inventory/personnel integration

### C2[33].23 — save-load/idempotency matrix

### C2[33].24 — edge-case/recovery-completeness suite

### C2[33].25 — selftest + deliberate failure proof

### C2[33].26 — long-run disaster soak

### C2[33].27 — prepared/unprepared balance profiles

### C2[33].28 — fairness/attention/accessibility tests

### C2[33].29 — playtest/docs/release closure

### Gate: 158C complete

---

# 189. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --disaster-response-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
disaster-template content-utilization report
old-save fixture load
same-seed disaster replay
warning→response→recovery lifecycle matrix
long-run disaster frequency soak
emergency-panel snapshot/accessibility gate
```

---

# 190. Flagship Definition of Done

## 158A — Foundation

- [ ] DisasterResponseSystem,
- [ ] definition/instance split,
- [ ] eight disaster types,
- [ ] severity bands,
- [ ] deterministic cause/trigger model,
- [ ] warning/preparation phases,
- [ ] emergency protocols,
- [ ] real personnel assignments,
- [ ] inventory-backed supplies,
- [ ] resilience projection,
- [ ] effect idempotency,
- [ ] recovery state,
- [ ] old-save/grace behavior,
- [ ] semantic events,
- [ ] downstream ports,
- [ ] diagnostics.

## 158B — Mechanics / Content

- [ ] earthquake,
- [ ] flood,
- [ ] fire,
- [ ] radiation leak,
- [ ] structural failure,
- [ ] disease outbreak,
- [ ] power failure,
- [ ] air contamination,
- [ ] 15 templates,
- [ ] protocol chaining,
- [ ] emergency reserves,
- [ ] warning mechanics,
- [ ] response mechanics,
- [ ] recovery mechanics,
- [ ] event set,
- [ ] quest hooks,
- [ ] emergency panel,
- [ ] journal,
- [ ] tutorial,
- [ ] utilization.

## 158C — Integration / Closure

- [ ] shelter state,
- [ ] thermal,
- [ ] ventilation,
- [ ] sump flooding,
- [ ] medical,
- [ ] survivor fate,
- [ ] radiation,
- [ ] disease,
- [ ] power,
- [ ] inventory,
- [ ] duty/personnel,
- [ ] room evacuation,
- [ ] quest/morale/epilogue,
- [ ] save/load lifecycle,
- [ ] no duplicate damage,
- [ ] protocol idempotency,
- [ ] anti-farming,
- [ ] peaceful scenario,
- [ ] constant-disaster stress,
- [ ] no-supply/no-personnel scenarios,
- [ ] unsupported full-evacuation safely gated,
- [ ] recovery completeness,
- [ ] selftest,
- [ ] failure proof,
- [ ] long-run soak,
- [ ] frequency guard,
- [ ] prepared/unprepared profiles,
- [ ] protocol value,
- [ ] fairness,
- [ ] accessibility,
- [ ] UI/runtime parity,
- [ ] retention,
- [ ] epilogue,
- [ ] playtest,
- [ ] docs.

## Global

- [ ] no duplicate shelter truth,
- [ ] no duplicate power/flood/radiation/disease truth,
- [ ] no duplicate inventory quantities,
- [ ] no duplicate casualty authority,
- [ ] no free-floating resilience stat,
- [ ] no save/load disaster reroll,
- [ ] no unrecoverable crisis caused by orchestration bugs,
- [ ] no disaster-frequency spam,
- [ ] full verification green.

---

# 191. Closure Report Template

```markdown
## C2[33] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Shelter structure owner:
- Thermal owner:
- Ventilation owner:
- Flood owner:
- Power owner:
- Radiation owner:
- Disease owner:
- Medical owner:
- Fate owner:
- Existing emergency events:

### 158A — Foundation
- Disaster types:
- Templates:
- Trigger causes:
- Major-disaster budget:
- Warning policies:
- Protocols:
- Personnel model:
- Supply reservation:
- Resilience projection:
- Save schema:
- Old-save grace:
- Missing ports:
- Result:

### 158B — Disasters
- Earthquake:
- Flood:
- Fire:
- Radiation leak:
- Structural failure:
- Disease outbreak:
- Power failure:
- Air contamination:
- Protocol chaining:
- Emergency reserves:
- Quests:
- UI:
- Unused templates:
- Result:

### 158C — Integration
- Shelter:
- Thermal:
- Ventilation:
- Flood:
- Radiation:
- Disease:
- Power:
- Medical:
- Fate:
- Inventory:
- Duty/personnel:
- Room evacuation:
- Save-load duplicates:
- Recovery completeness:
- Peaceful case:
- Stress case:
- Prepared profile:
- Neglectful profile:
- Playtest:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Disaster response selftest:
- Port contract:
- Content utilization:
- Old-save fixtures:
- Same-seed replay:
- Long-run soak:
- UI/accessibility:
- Verify fast:

### Final Metrics
- DISASTER_TEMPLATES:
- MAJOR_DISASTERS_PER_YEAR:
- WARNED_DISASTERS:
- PROTOCOL_ACTIVATIONS:
- CONTAINED_DISASTERS:
- ESCALATED_DISASTERS:
- CASUALTIES:
- RESOURCE_COST:
- MEAN_RECOVERY_DAYS:
- DUPLICATE_EFFECT_APPLICATIONS:
- REQUIRED_PORTS_MISSING:
- UNRESOLVED_RECOVERY_ITEMS:

### Remaining Debt
- Disaster content:
- Facilities:
- Training:
- Full evacuation:
- Room adjacency:
- UI:
```

---

# 192. Final Execution Directive

Execute Plan 158 as an **acute crisis-orchestration layer over the existing shelter, environmental, medical, power, inventory, and survivor-fate authorities**.

The critical sequence is:

```text
identify state-backed disaster causes
→ create deterministic disaster instances
→ provide warning/preparation when appropriate
→ activate protocols using real people and supplies
→ apply crisis effects through canonical owners
→ prevent duplicate effects across save/load
→ contain the immediate hazard
→ hand persistent damage/injury/contamination to owning systems
→ coordinate recovery
→ preserve landmark history
→ prove rarity, preparedness value, fairness, and recoverability
```

Do not create a second shelter-damage system.

Do not create a second flood, radiation, disease, or power state.

Do not store emergency-supply quantities outside inventory.

Do not store casualty truth outside survivor fate.

Do not make “resilience” an opaque magic number.

Do not let “resolved disaster” mean “all damage magically repaired.”

The strongest authority rule is:

> **DisasterResponseSystem owns the crisis timeline and response coordination; every physical or survivor consequence remains owned by the system that normally governs that fact.**

The strongest fairness rule is:

> **A major disaster should be dangerous because preparation was insufficient or the event was severe—not because the player had no warning, no protocol, no meaningful response, or no recovery path.**

The strongest recovery rule is:

> **Containment ends the acute emergency; recovery then restores services and repairs consequences through normal gameplay rather than erasing them.**

The flagship acceptance scenario is:

> **Run a seeded severe flood against a poorly maintained lower-level shelter with functioning early warning. Let the player cut power to affected rooms, assign responders, reserve pumps and purification supplies, evacuate occupants, and activate the flood-response protocol. Save/load during the active phase. Water, electrical risk, contamination, injuries, and resource use must route through the real flooding/power/disease/medical/inventory systems and must not apply twice after reload. Once contained, the acute flood state must end while persistent room damage, contamination, injuries, and repair tasks remain until their canonical systems resolve them. Repeat with a prepared shelter and prove that stronger drainage, trained responders, stocked supplies, and maintenance materially reduce loss without changing the deterministic disaster identity.**
