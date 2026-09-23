# ASHFALL — Expansion 42 Design Bible
# THE CORE
### Wave 7 · Reactor Operations, Cooling, Shielding, Criticality, Spent Fuel, and Health Physics

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Shelter` (NuclearCoreLifecycleSystem, NuclearCoreCatalog, MaterialShieldingSystem, ShelterShieldingModel, PowerGridSystem interface), `Ashfall.Core.Medical` (dose reading only)
**Proposed host owner:** `ReactorHostSession` (extends core, cooling, shielding, and dose-survey surfaces)
**Existing save sections:** `NuclearCoreLifecycleSave`, shielding state, dose ledger (read-only)
**Existing CLI verbs:** `--nuclear-core-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has reactor cores as objects. `NuclearCoreLifecycleSystem` exposes
`InstalledCores`, `SpentCoreStorage`, `LastTickDay`, `OnCoreInstalled`,
`OnReactorScrammed`, `OnHeatStateChanged`, `OnRadiationLeak`,
`GetCore(instanceId)`, `TryInstallCore(instanceId, profileId, roomId)` (default
room `reactor_vault`), `SetOutputSetting(instanceId, setting)`,
`GetTotalGenerationWatts()`, `TryRepairShielding(instanceId)` (consuming an
inventory bill), `TryEmergencyScram(instanceId)` (consuming a scram canister),
and `TickDay(currentDay, coolantOverride)`. It declares `PowerSourceId =
"nuclear_core"`. `ReactorCoreState` carries `outputSetting` (Shutdown, Low,
Normal, High), `coolantState` (Sufficient, Restricted, Depleted), `heatState`
(Nominal, Elevated, Critical, Runaway), `shieldingIntegrity` (0–100),
`embrittlementWear` (0–100), `isScrammed`, `isInstalled`, `roomId`, and
`lastTickDay`. `NuclearCoreCatalog` defines `NuclearCoreDefinition` with
`powerClass`, `baseElectricalOutput`, `thermalClass`, `radiationClass`,
`coolingDemand`, `shieldingRequirement`, `wearRate`, `decayClass`, and
`emergencyShutdownItemId`. The save holds `installedCores` and
`spentCoreStorage`.

What does not exist: reactor procedures, shifts, cooling loops, shielding
surveys, criticality rules, spent-fuel management, dosimetry rounds, drills,
and any content beyond three profiles in a **3,314-byte** catalog.

**The Core** turns the reactor into a lived institution: the machine the shelter
depends on, the room it is afraid of, the people who understand it, and the
discipline that keeps it quiet. It extends the live core, shielding, and power
owners and routes every radiological consequence to the live dose ledger.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 07 The Dose / dose ledger | Radiation history and bands | Reads it, never writes dose |
| 21 The Grid | Power distribution and loads | Provides generation at the source; grid at the bus |
| 22 The Clean Flow | Water and sanitation | Draws cooling water through its rules |
| 38 The Ward | Medical care for injuries | Routes exposure cases to it |
| 23 The Alarm | Emergency and evacuation | Uses its drills and muster |
| 19 The Bitter Air | Hazard exposure and decon | Uses its decon routes |
| 40 The Wheel | Pumps and machinery | Orders pump repair, never defines it |
| 44 The Outpost (Wave 7) | Remote settlements | Schedules remote power, never owns it |
| 43 The Question (Wave 7) | Research and archives | Decrypts old core documentation |
| 35 The Habit | Dependency care | Routes stress without judgment |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter has a core in a vault and nobody who has ever run one.

**The Core** is the expansion about operating something that can hurt everyone
in the building: output planning, cooling, shielding, criticality discipline,
spent fuel, dosimetry, and the drills that turn a dangerous machine into an
ordinary job. It gives the reactor a control room, a rota, a survey map, a
storage cask, and a safety culture.

### 1.2 The five loops it adds

```
  Plan ──► Run ──► Cool ──► Survey ──► Store
    │        │        │         │         │
    ▼        ▼        ▼         ▼         ▼
  Output,  Settings, Loops,   Shielding, Spent fuel,
  load     shifts    water    readings   casks
                                        │
                                        ▼
                                  Drill ──► Scram ──► Restart ──► Review
```

### 1.3 What the player manages

1. **Output.** Settings, demand, and reserve, planned honestly.
2. **Shifts.** Operators, supervisors, and fatigue rules.
3. **Cooling.** Loops, pumps, heat exchangers, and water quality.
4. **Shielding.** Integrity, surveys, repairs, and exclusion zones.
5. **Criticality.** Procedures, geometry, and the rules that never bend.
6. **Spent fuel.** Decay heat, storage, casks, and final disposition.
7. **Dosimetry.** Film, badges, surveys, and dose tracking.
8. **Drills.** Scram, evacuation, muster, and restart.
9. **Records.** Logs, procedure changes, and honest reporting.
10. **Culture.** The habit of asking whether the machine is telling the truth.

### 1.4 What it is not

- Not a meltdown spectacle. Failure is procedural, traceable, and survivable;
  the drama is in the discipline.
- Not a weapons program. No weapons material, no enrichment, no bomb content.
- Not a second power grid or dose system. It extends the live owners and routes
  radiological facts to the dose ledger.
- Not a haunted-machine story. The reactor is a machine with rules.
- Not a masochistic hazard simulator; safeguards are effective when followed.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs` | Core state, output, scram, leaks | `LIVE` |
| `Assets/Ashfall.Core/Shelter/NuclearCoreCatalog.cs` | Core profiles | `LIVE` |
| `Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs` | Shielding materials | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterShieldingModel.cs` | Shelter shielding model | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | Grid interface | `LIVE` |
| `Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs` | Fluid delivery | `LIVE` |
| `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` | Injury care | `LIVE` |
| `Assets/Ashfall.Core/DoseLedgerSystem.cs` | Dose history (read-only here) | `LIVE` |
| `Assets/Ashfall.Core/Needs/NeedsSystem.cs` | Health and condition | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `nuclear_core_profiles.json` | **3,314 B** | three profiles |
| Cooling, shielding, spent fuel, survey, drill, procedure data | absent | confirmed none |
| `material_shielding` data | small | material classes only |

### 2.3 Confirmed gaps

- **GAP-42-1 — No reactor procedures or shifts.**
- **GAP-42-2 — No output planning or demand content.**
- **GAP-42-3 — No cooling loop content.**
- **GAP-42-4 — No shielding survey or exclusion content.**
- **GAP-42-5 — No criticality rules or geometry discipline.**
- **GAP-42-6 — No spent fuel storage or cask content.**
- **GAP-42-7 — No dosimetry rounds.**
- **GAP-42-8 — No scram, evacuation, or restart drill content.**
- **GAP-42-9 — No reactor logs or review culture.**
- **GAP-42-10 — Core catalog has three profiles and no narrative.**

### 2.4 Non-duplication statement

This expansion will **not** add a second core, power, dose, medical, water, or
emergency system. It extends `NuclearCoreLifecycleSystem` with operations
content, `MaterialShieldingSystem` and `ShelterShieldingModel` with survey and
repair content, the power grid at its interface, and the alarm and dose owners
at their seams. It adds state only as additive sub-objects of the existing core
and shielding stores. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The machine is honest; operators can be wrong.** Instrumentation
and procedure exist to catch the human, not to flatter them.

**Pillar 2 — Cooling is the job.** Heat is the hazard that never stops being
there.

**Pillar 3 — Time and distance are the shield.** Every rule is a shape of one
of those two.

**Pillar 4 — Dose is recorded, never argued.** The ledger decides; the operator
respects it.

**Pillar 5 — A drill is a promise kept.** The day the reactor screams is too
late to learn the muster route.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Control room | Quiet procedure and readouts | Countdown spectacle |
| Heat | Gauges, loops, patience | Glowing menace |
| Shielding | Surveys and boundaries | Mutation horror |
| Scram | Drill and review | Explosion |
| Spent fuel | Casks, records, waiting | Secret dumping |
| Exposure | Clinical and caring | Radiation as curse |
| Operators | Trained professionals | Cults or cowards |
| The vault | A room with rules | A monster's lair |

### 3.3 Content limits

- No weapons material, enrichment, or bomb content of any kind.
- No meltdown as entertainment; the worst outcome is a controlled emergency
  with traceable causes and a full review.
- No radiation-as-mutation horror; exposure consequences route to factual
  medical and dose systems.
- No real reactor designs, vendors, or accident names; all cores are fictional
  archetypes.
- No disposal of spent fuel into the environment as a solvable shortcut.
- No shaming of operators; fatigue and error are met with rotation and review.

---

## 4. THE CORE WORLD

### 4.1 Interior rooms

- **`room_reactor_vault`** — the shielding cell and the core itself.
- **`room_control_desk`** — the readouts, the log, and two chairs.
- **`room_cooling_room`** — pumps, headers, and heat exchangers.
- **`room_shield_store`** — lead, borated panels, and survey gear.
- **`room_dose_office`** — badges, records, and the health physicist.
- **`room_spent_pool`** — a dark room with a cask and a rule.
- **`room_reactor_ward`** — the small rest room for the long shift.
- **`room_safety_board`** — procedure cards and the drill calendar.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_vault_door` | The Vault Door | 4 | Controlled entry |
| `loc_cooling_intake` | The Cooling Intake | 3 | Water source |
| `loc_heat_outfall` | The Warm Outfall | 2 | Heat rejection |
| `loc_shielding_quarry` | The Shielding Quarry | 3 | Lead, stone, sand |
| `loc_dose_shack` | The Dose Shack | 2 | Badge checkpoint |
| `loc_muster_pad` | The Muster Pad | 3 | Evacuation assembly |
| `loc_cask_yard` | The Cask Yard | 4 | Spent fuel parking |
| `loc_far_boundary` | The Far Boundary | 3 | Exclusion line |
| `loc_core_grave` | The Deep Vault | 4 | Final disposition |
| `loc_signal_mast` | The Reactor Mast | 2 | Drill signal |

All locations require valid item references and scanner registration.

### 4.3 The watch rhythm

Two operators on shift; hourly rounds; a survey every week; a drill every
month; a review after every event. The expansion's clock is the core day.

---

## 5. MAIN STORYLINE — "THE THING THAT STAYS WARM"

### 5.1 Central conflict

A pebble-bed core has been running in the vault for years, cooling on water
that arrives by chance and shielded by walls nobody has ever measured.
**Reiska Morn** understands the machine and is the only person who does.
**Anil Kesh** the health physicist wants surveys and dosimetry before anyone
else walks in. **Maura Lin** the cooling technician wants a loop with a real
pump and a clean intake. **Edda Harn** the control operator wants shifts long
enough to be useful and short enough to be safe. **Tol** the shielding mason
wants the walls measured and the boundaries painted. **Sib** the waste warden
wants a proper place for the spent core.

Then the coolant state changes during a cold snap, and the shelter learns
exactly how much of its power depends on one warm room and three people who
have been awake too long.

The expansion's question: **can the shelter be worthy of the machine it
inherited?**

### 5.2 Theme (unspoken)

**Power is a debt paid in attention.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_reactor_engineer_reiska_morn` | Reiska Morn | Reactor engineer | Operations and procedures |
| `npc_health_physicist_anil_kesh` | Anil Kesh | Health physicist | Surveys, badges, dose routing |
| `npc_cooling_tech_maura_lin` | Maura Lin | Cooling technician | Loops and water |
| `npc_control_operator_edda_harn` | Edda Harn | Control operator | Shifts and readouts |
| `npc_shielding_mason_tol` | Tol | Shielding mason | Walls and boundaries |
| `npc_waste_warden_sib` | Sib | Waste warden | Spent fuel and casks |
| `npc_physicist_yun_aral` | Yun Aral | Physicist | Theory and documentation |
| `npc_apprentice_reactor_beck` | Beck | Apprentice | Rounds, checks, learning |

### 5.4 Story beats (15)

1. **The Warm Room.** The vault is found to be warmer than the logs say.
2. **The Badge.** Anil issues the first dosimeters and is argued with.
3. **The Rounds.** Hourly rounds are written and walked.
4. **The Loop.** A real cooling loop replaces chance.
5. **The Wall.** Shielding is measured and found wanting.
6. **The Boundary.** Exclusion lines are painted and posted.
7. **The Shift.** Edda's rota balances coverage against fatigue.
8. **The Snap.** A cold snap restricts the coolant and tests the procedures.
9. **The Scram.** A drill becomes a real scram and the shelter discovers its
   restart procedure is a guess.
10. **The Spent Core.** Sib argues for a cask before the first core is spent.
11. **The Survey.** A contamination check finds a trace nobody expected.
12. **The Log.** Honest reporting becomes the culture, not the exception.
13. **The Yard.** The cask yard is built and the first core is moved safely.
14. **The Review.** The scram is reviewed blamelessly and a procedure changes.
15. **The Thing That Stays Warm.** The shelter decides what the core means.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Output policy | conserve / steady / industrial | life vs. growth |
| Shifts | two-person / three / long | coverage vs. health |
| Shielding | patch / rebuild / distance | cost vs. certainty |
| Spent fuel | pool / cask / deep vault | waiting vs. finality |
| Surveys | weekly / monthly / event | rigor vs. labor |
| Drills | monthly / quarterly / surprise | readiness vs. fear |
| Records | open / restricted / sealed | trust vs. security |
| Final | core as institution / machine / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Quiet Core** — procedures, surveys, and drills make the reactor an
   ordinary department.
2. **The Long Watch** — the core runs at a human pace, and the shelter plans its
   life around a machine it respects.
3. **The Cold Season** — a hard winter tests cooling and rota, and the shelter
   passes because it prepared.
4. **The Deep Vault** — the spent core is placed with ceremony and records, and
   the shelter stops pretending the problem will go away.
5. **The Honest Log** — the culture survives a serious event because nobody hid
   anything.
6. **Fade** — a warm room, a green light, and two operators making tea.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_core_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_core_warm_room`, `quest_core_badge`, `quest_core_rounds`, `quest_core_loop`,
`quest_core_wall`, `quest_core_boundary`, `quest_core_shift`, `quest_core_snap`,
`quest_core_scram`, `quest_core_spent`, `quest_core_survey`, `quest_core_log`,
`quest_core_yard`, `quest_core_review`, `quest_core_warm_thing`.

### 6.2 Side quests (30)

**Operations (5)**
- `quest_core_output_plan` — write an output plan
- `quest_core_demand` — read real demand
- `quest_core_console` — improve the console
- `quest_core_handover` — write shift handover
- `quest_core_reserve` — keep a reserve

**Cooling (5)**
- `quest_core_pump` — repair the pump
- `quest_core_intake` — clean the intake
- `quest_core_exchanger` — service the exchanger
- `quest_core_water` — test cooling water
- `quest_core_winter` — winter cooling plan

**Shielding (5)**
- `quest_core_survey_wall` — survey the wall
- `quest_core_repair_wall` — repair the wall
- `quest_core_panels` — fit borated panels
- `quest_core_distance` — use distance
- `quest_core_lead_store` — control the shield store

**Criticality (5)**
- `quest_core_geometry` — geometry rules
- `quest_core_exclusion` — exclusion zone
- `quest_core_procedure` — procedure cards
- `quest_core_training` — train the operators
- `quest_core_audit` — criticality audit

**Spent fuel (5)**
- `quest_core_cask_build` — build the cask
- `quest_core_pool` — build the pool
- `quest_core_transfer` — move a spent core
- `quest_core_record_fuel` — record the fuel
- `quest_core_grave_site` — choose the deep site

**Dose and drills (5)**
- `quest_core_badges` — issue dosimeters
- `quest_core_survey_round` — weekly survey
- `quest_core_drill_scram` — scram drill
- `quest_core_drill_evac` — evacuation drill
- `quest_core_health_check` — operator health review

### 6.3 Repeatable quests (8)

`quest_core_repeat_round`, `quest_core_repeat_survey`, `quest_core_repeat_badge`,
`quest_core_repeat_cooling`, `quest_core_repeat_log`, `quest_core_repeat_drill`,
`quest_core_repeat_check`, `quest_core_repeat_restock`.

### 6.4 Dynamic hooks

Live events (coolant state changes, heat state changes, leaks, scram, weather
cold snaps, ward admissions, power brownouts, dose alerts) attach authored
follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Core state, output, scram, and leak events stay with
  `NuclearCoreLifecycleSystem`.
- Shielding stays with the shielding owners; dose stays with the ledger.
- Power distribution stays with the grid.
- No weapons, enrichment, or bomb content.
- Exposure routes to medical and dose owners only.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `CoreOperationsSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** procedures, shift rota, output plans, console state, handovers, and
reactor logs. **Consumes:** `NuclearCoreLifecycleSystem`,
`ShelterPowerGridCatalog`, `PowerGridSystem`, `DutyRoster`, `NeedsSystem`.
**Data:** `core_procedures.json`, `core_shifts.json`, `core_output_plans.json`.
**Rules:** orders come from written procedures; every setting change is logged
with a reason; shifts respect fatigue rules; the system never duplicates the
core's own state machine.

### 7.2 `CoolingSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** cooling loops, pumps, heat exchangers, intake condition, and coolant
quality. **Consumes:** `FluidLogisticsSystem`, `WaterTreatmentSystem` (Wave 3),
`MachineToolSystem` (Wave 6), `NuclearCoreLifecycleSystem` (coolant state).
**Data:** `cooling_loops.json`. **Rules:** cooling demand comes from the core
profile; water is real and shared with the shelter; a loop failure raises the
live coolant state honestly.

### 7.3 `ShieldingSystem` (extend `MaterialShieldingSystem` and `ShelterShieldingModel`)

**Owns:** shielding surveys, repairs, exclusion boundaries, and shield stores.
**Consumes:** the live shielding fields, `Inventory`, `BuildWorksSystem`
(Wave 4), `DoseLedgerSystem` (read). **Data:** `shielding_surveys.json`.
**Rules:** shielding integrity decays from heat and time; surveys produce
readings; repairs consume materials and labor; distance and time are always
valid shields.

### 7.4 `CriticalitySafetySystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** geometry rules, exclusion zones, procedure cards, and audits.
**Consumes:** `CoreOperationsSystem`, `Inventory`, records owners.
**Data:** `criticality_rules.json`. **Rules:** criticality rules are absolute
and unchangeable by circumstance; audits check compliance; violations are
recorded and reviewed without blame.

### 7.5 `SpentFuelSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** spent core storage, decay heat, casks, transfer, and final
disposition. **Consumes:** `NuclearCoreLifecycleSystem.SpentCoreStorage`,
`Inventory`, `BuildWorksSystem`, `RailwaySystem` (Wave 3) for heavy moves.
**Data:** `spent_fuel.json`. **Rules:** spent cores stay warm and must be
cooled, stored, and eventually placed; every transfer is recorded; no dumping.

### 7.6 `HealthPhysicsSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** dosimeters, surveys, contamination checks, and ALARA culture.
**Consumes:** `DoseLedgerSystem` (read-only), `NeedsSystem`, medical pipeline,
`DecontaminationSystem` (Wave 2). **Data:** `dosimetry_rounds.json`.
**Rules:** dose is recorded by the ledger; the physicist reads it and advises;
exposure cases route to the ward; nothing here re-derives or re-ranks dose.

### 7.7 `ReactorDrillSystem` (new, thin, extends emergency owners)

**Owns:** scram, evacuation, muster, restart, and post-drill reviews.
**Consumes:** `NuclearCoreLifecycleSystem`, `AlarmSystem` (Wave 3),
`DutyRoster`, `WatchHouseHostSession` (Wave 5). **Data:**
`reactor_drills.json`. **Rules:** every drill has a scenario, a time target,
and a review; surprise drills are announced in principle but not in hour; the
restart procedure is practiced before it is needed.

### 7.8 Systems explicitly not added

- No second core, power, dose, medical, water, or emergency system.
- No weapons, enrichment, or bomb content.
- No meltdown spectacle or radiation horror.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `core_procedures.json` (new)

```json
{
  "schema_version": 1,
  "procedures": [
    {
      "procedure_id": "procedure_startup",
      "display_name": "Startup",
      "steps": ["survey", "cooling check", "rod check", "log"],
      "roles": ["engineer", "operator"],
      "duration_hours": 4,
      "criticality_rule": "geometry_card_startup",
      "tags": ["reactor", "routine"]
    }
  ]
}
```

### 8.2 `core_shifts.json` (new)

Shifts: operator, supervisor, hours, fatigue rule, handover, reserve.

### 8.3 `core_output_plans.json` (new)

Plans: demand band, output setting, reserve, black-start link, review.

### 8.4 `cooling_loops.json` (new)

Loops: pumps, exchangers, intake, flow, temperature, failure mode.

### 8.5 `shielding_surveys.json` (new)

Surveys: location, reading, threshold, action, repair, re-survey.

### 8.6 `criticality_rules.json` (new)

Rules: geometry, mass, moderation, reflection, exclusion, audit.

### 8.7 `spent_fuel.json` (new)

Spent cores: decay heat, cooling need, cask, placement, record, final site.

### 8.8 `dosimetry_rounds.json` (new)

Rounds: badge, station, frequency, threshold, review, referral.

### 8.9 `reactor_drills.json` (new)

Drills: scenario, roles, time target, failure modes, review, restart.

### 8.10 Extensions

Extend `nuclear_core_profiles.json` with additional fictional archetypes and
`nuclear_core_profiles` metadata (cooling class, shielding class, decay class),
and extend shielding material data with survey and repair content.

### 8.11 Items

New items appended to `items.json`: `item_dosimeter_badge`,
`item_survey_meter`, `item_boron_canister`, `item_lead_panel`,
`item_shield_brick`, `item_scram_canister`, `item_fuel_cask`,
`item_cooling_pump_part`, `item_heat_exchanger_tube`, `item_exclusion_sign`,
`item_reactor_log`, `item_procedure_card`, `item_contamination_kit`,
`item_warm_room_lamp`, `item_operator_chair`, `item_cask_seal`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`NuclearCoreLifecycleSave` and the shielding state remain the live save owners.
New sub-objects (procedures, shifts, plans, loops, surveys, criticality, spent
fuel, dosimetry, drills) are additive inside them. No new save section.

### 9.2 State to persist

- Procedure completion and revisions.
- Shift rota and fatigue records.
- Output plans and reserve.
- Cooling loop condition and water state.
- Shielding readings and repairs.
- Criticality audits.
- Spent core inventory and placement.
- Dosimetry rounds and referrals.
- Drills and reviews.

### 9.3 Determinism

- Core output, heat, coolant, wear, and scram outcomes remain in the live
  system.
- Cooling, shielding, and spent-fuel outcomes derive from condition, labor, and
  recorded state.
- Leaks and dose route through the live engine and ledger.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with installed cores, spent storage, and shielding untouched;
no procedures, shifts, surveys, or drills exist until started. The three legacy
core profiles keep working; new profiles and content are additive.

### 9.5 Checksum

Invariant-culture floats; integer day, hour, and count fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `CorePanel` (new) | Readouts, settings, procedures | `ReactorHostSession` |
| `CoolingPanel` (new) | Loops, pumps, water | same |
| `ShieldingPanel` (new) | Surveys, boundaries, repairs | same |
| `SpentFuelPanel` (new) | Casks, storage, moves | same |
| `DoseSurveyPanel` (new) | Badges and rounds | same |
| `DrillPanel` (new) | Scenarios and reviews | same |
| `CoreLogPanel` (new) | Logs and handovers | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Readouts use text and shape, never color alone; alarms have text equivalents.
- Procedures are readable step cards, not checklists that hide state.
- Dose displays come from the ledger and show uncertainty in plain language.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- High-contrast control display for the dark control room.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a pump starting, a meter ticking, a
relay closing, a cask rolling on rails, a drill horn, a logbook closing. No cue
is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `NuclearCoreLifecycleSystem` | Output, heat, coolant, scram, leaks |
| `NuclearCoreCatalog` | Core profiles |
| `MaterialShieldingSystem` | Shielding materials |
| `ShelterShieldingModel` | Shelter shielding |
| `PowerGridSystem` (Wave 2) | Generation at the bus |
| `FluidLogisticsSystem` | Cooling water delivery |
| `WaterTreatmentSystem` (Wave 3) | Water quality |
| `MachineToolSystem` (Wave 6) | Pump and exchanger parts |
| `DoseLedgerSystem` (Exp 07) | Read-only dose history |
| `NeedsSystem` | Health and fatigue |
| `MedicalPipelineCoordinator` | Exposure care |
| `DecontaminationSystem` (Wave 2) | Contamination routes |
| `AlarmSystem` (Wave 3) | Drills and evacuation |
| `WatchHouseHostSession` (Wave 5) | Muster and boundary watch |
| `RailwaySystem` (Wave 3) | Heavy fuel moves |
| `BuildWorksSystem` (Wave 4) | Walls, pools, casks |
| `ArchiveDeskSystem` (Wave 4) | Procedures and records |
| `EpilogueChronicleBuilder` | Core milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm core lifecycle, catalog, shielding,
grid, fluid, dose, and alarm owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the nine catalogs; extend core profiles
and shielding data; append items; register validators and scanner.

**Phase 2 — Pure Core.** `CoreOperationsSystem`, `CoolingSystem`,
`ShieldingSystem`, `CriticalitySafetySystem`, `SpentFuelSystem`,
`HealthPhysicsSystem`, `ReactorDrillSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `ReactorHostSession`, selftest coverage, fresh journey
from warm room to quiet core.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: cold snap, scram, spent core, survey
findings, and rota fatigue.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Procedures | 24 |
| Shifts | 8 |
| Output plans | 8 |
| Cooling loops | 10 |
| Surveys | 12 |
| Criticality rules | 10 |
| Spent fuel records | 8 |
| Dosimetry rounds | 8 |
| Drills | 10 |
| Core profiles extension | 6 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Weapons content | Critical | Content limits §3.3 |
| Meltdown spectacle | Critical | Procedural failure only |
| Second dose system | Critical | Ledger read-only |
| Radiation horror tone | High | Clinical presentation |
| Fatigue ignored | High | Rota and drills |
| Spent fuel dumping | High | Recorded placement |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `core_procedures.json` | 24 | 6,000 |
| `core_shifts.json` | 8 | 2,000 |
| `core_output_plans.json` | 8 | 2,000 |
| `cooling_loops.json` | 10 | 3,000 |
| `shielding_surveys.json` | 12 | 3,000 |
| `criticality_rules.json` | 10 | 2,500 |
| `spent_fuel.json` | 8 | 2,500 |
| `dosimetry_rounds.json` | 8 | 2,000 |
| `reactor_drills.json` | 10 | 3,000 |
| Core profile extensions | 6 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~59,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R42-1 | Weapons content | Low | Critical | Hard prohibition |
| R42-2 | Meltdown spectacle | Med | Critical | Procedural outcomes |
| R42-3 | Second dose system | Low | Critical | Read-only ledger |
| R42-4 | Radiation horror | Med | High | Clinical tone |
| R42-5 | Operator fatigue | Med | High | Shift rules |
| R42-6 | Dumped spent fuel | Low | High | Recorded placement |
| R42-7 | Sadistic drills | Med | Med | Review and care |
| R42-8 | Determinism | Low | High | Live paths |
| R42-9 | Content overrun | Med | Med | Budget §13 |
| R42-10 | Grid vs. core overlap | Med | Med | Boundary §0.1 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can the reactor be decommissioned?** Recommended: yes, eventually, with
   spent fuel placed and the vault sealed.
2. **Do leaks ever occur in perfect play?** Recommended: wear and events can
   leak; preparation contains them, never eliminates risk entirely.
3. **How much power should one core deliver?** Recommended: enough to matter to
   the grid and never enough to stop all other generation.
4. **Are the exclusion zones permanent?** Recommended: yes, surveyed and
   re-marked each season.
5. **Can spent cores be moved between outposts?** Recommended: yes, through the
   rail owner, with casks and records.

---

## 17. APPENDIX D — CORE PROCEDURE TABLE (24 PROCEDURES)

| # | Procedure | Roles | Duration | Key steps | Criticality card |
|---|---|---|---|---|---|
| 1 | Startup | engineer, operator | 4h | survey, cooling, rods, log | startup card |
| 2 | Normal run | operator | continuous | read, log, round | none |
| 3 | Output change | engineer, operator | 1h | plan, set, verify, log | none |
| 4 | Shutdown | engineer, operator | 3h | lower, cool, verify | shutdown card |
| 5 | Scram | any | instant | press, confirm, log | scram card |
| 6 | Restart | engineer, two | 6h | survey, cool, rods, test | startup card |
| 7 | Cooling check | tech | 1h | flow, temp, leak | none |
| 8 | Pump swap | tech, mechanic | 2h | isolate, swap, bleed | none |
| 9 | Exchanger clean | tech | 3h | isolate, clean, test | none |
| 10 | Intake clear | any | 1h | stop, rake, restart | none |
| 11 | Shield survey | physicist | 2h | meter, record, mark | none |
| 12 | Shield repair | mason, physicist | 6h | remove, pour, re-survey | none |
| 13 | Exclusion remark | mason | 2h | paint, post, log | none |
| 14 | Cask load | warden, two | 8h | cool, seat, seal | cask card |
| 15 | Cask move | warden, rail | 12h | secure, move, place | transfer card |
| 16 | Pool check | warden | 1h | level, temp, clean | none |
| 17 | Badge round | physicist | 1h | collect, read, record | none |
| 18 | Contamination check | physicist | 2h | survey, mark, decon | none |
| 19 | Fuel inventory | warden | 3h | count, verify, log | inventory card |
| 20 | Drill: scram | all | 1h | signal, scram, muster | scram card |
| 21 | Drill: evacuation | all | 2h | signal, route, count | none |
| 22 | Drill: leak | selected | 2h | signal, isolate, survey | leak card |
| 23 | Handover | both shifts | 1h | read, walk, sign | none |
| 24 | Annual review | all | 4h | read, question, change | none |

Procedures are the reactor's language. Every row has roles, a duration, and a
criticality card, which means the shelter can swap an operator without losing
the machine's history and can never claim it did not know the rule.

---

## 18. APPENDIX E — SHIFT TABLE

| Shift | Hours | Operator | Supervisor | Fatigue rule |
|---|---|---|---|---|
| Day | 06–14 | one | engineer | 8h max |
| Evening | 14–22 | one | senior operator | 8h max |
| Night | 22–06 | one | on call | 6h watch |
| Startup day | 08–16 | two | engineer | no overtime |
| Refuel | 08–18 | three | engineer, physicist | scheduled |
| Drill | announced | all | engineer | paid time |
| Post-scram | 24h | two | engineer | rest first |
| Reserve | on call | one | engineer | phone reachable |

The shift table exists because a reactor watched by tired people is a reactor
watched badly. The post-scram row is deliberate: after an event, the first duty
is rest, and the machine can wait.

---

## 19. APPENDIX F — OUTPUT PLAN TABLE

| Plan | Setting | Reserve | Grid use | Notes |
|---|---|---|---|---|
| Conserve | Low | 30% | essentials | quiet seasons |
| Steady | Normal | 20% | standard | default |
| Industrial | High | 10% | works, foundry | short runs |
| Winter | Normal | 25% | heat, light | cold months |
| Outage | Shutdown | 0 | grid only | maintenance |
| Emergency | Low | 40% | ward, watch | storm |
| Startup | ramp | 20% | none | test |
| Drill | Low | 30% | none | practice |

Output planning is where the reactor meets the shelter's ordinary life. The
reserve column is the plan's honesty: a core that runs at its limit has no room
for the night the hospital needs more light.

---

## 20. APPENDIX G — COOLING LOOP TABLE

| # | Loop | Pumps | Exchanger | Flow | Failure |
|---|---|---|---|---|---|
| 1 | Primary | 1 | core-side | high | heat rise |
| 2 | Secondary | 1 | shell | high | poor reject |
| 3 | Emergency | 1 | closed | med | reserve |
| 4 | Intake line | 1 | screen | varies | clog |
| 5 | Outfall | 1 | none | high | warm river |
| 6 | Cold reserve | 1 | tank | low | freeze |
| 7 | Cask loop | 1 | small | low | patient |
| 8 | Ward tap | shared | none | low | draw |
| 9 | Works tap | shared | none | med | draw |
| 10 | Test rig | none | portable | low | lesson |

The cooling table is the expansion's real subject. Ten rows, each a place water
can stop being useful, and each with a failure mode the operator has already
prepared for. The cask loop is last because spent fuel is patient and the
reactor is not.

---

## 21. APPENDIX H — SHIELDING SURVEY TABLE

| # | Location | Threshold | Action at limit | Repair | Re-survey |
|---|---|---|---|---|---|
| 1 | Vault door | dose rate | restrict entry | add brick | after |
| 2 | Vault wall | dose rate | add panel | pour | after |
| 3 | Vault ceiling | dose rate | add shield | pour | after |
| 4 | Control wall | dose rate | distance | panels | after |
| 5 | Cooling room | swab | decon | wash | after |
| 6 | Corridor | dose rate | mark | brick | after |
| 7 | Cask yard | dose rate | fence | distance | after |
| 8 | Outfall | sample | stop flow | dilute | after |
| 9 | Tool crib | swab | decon | clean | after |
| 10 | Ward boundary | dose rate | move bed | wall | after |
| 11 | Exclusion line | visual | repaint | sign | after |
| 12 | Shield store | inventory | count | replace | monthly |

Surveys make radiation visible in the only way that is healthy for a game: as
numbers, boundaries, and deliberate work. Each row has a threshold and an
action, so a player never has to guess what concerned looks like.

---

## 22. APPENDIX I — CRITICALITY RULE TABLE

| # | Rule | Statement | Audit |
|---|---|---|---|
| 1 | Geometry | never move fuel into a bad shape | card check |
| 2 | Mass | never exceed the posted limit | monthly |
| 3 | Moderation | keep moderators away from fuel | monthly |
| 4 | Reflection | control reflecting walls | quarterly |
| 5 | Exclusion | no one enters a marked cell | daily |
| 6 | Tools | no metal near the fuel face | daily |
| 7 | Cards | every move has a card | per move |
| 8 | Two persons | no fuel move alone | per move |
| 9 | Log | every move recorded | weekly |
| 10 | Stop authority | anyone may stop a move | always |

Criticality rules are written as absolutes because the margin for cleverness is
zero. The stop-authority row is the culture: the newest apprentice can stop the
most senior engineer, and the log will thank them for it.

---

## 23. APPENDIX J — SPENT FUEL TABLE

| # | Record | Decay heat | Cooling | Storage | Disposition |
|---|---|---|---|---|---|
| 1 | First spent core | high | pool | cask yard | deep vault |
| 2 | Second core | high | pool | cask | deep vault |
| 3 | Fragments | low | cask | store | deep vault |
| 4 | Contaminated tools | low | none | sealed box | deep vault |
| 5 | Filter media | low | none | sealed drum | deep vault |
| 6 | Sludge | med | pool | sealed | deep vault |
| 7 | Test samples | low | none | cabinet | archive |
| 8 | Resin | med | tank | sealed | deep vault |

Spent fuel is the expansion's honesty about time. A core is spent and then it
waits for decades, and the table turns that into a management system with
records rather than a story about a mysterious glowing room.

---

## 24. APPENDIX K — DOSIMETRY ROUND TABLE

| # | Round | Frequency | Tool | Threshold | Action |
|---|---|---|---|---|---|
| 1 | Control room badge | daily | film | low | log |
| 2 | Vault entry badge | per entry | film | low | check |
| 3 | Physicist badge | weekly | film | med | review |
| 4 | Repair crew badge | per job | film | med | check |
| 5 | Ward staff badge | weekly | film | low | log |
| 6 | Perimeter swab | weekly | swab | trace | investigate |
| 7 | Outfall sample | weekly | sample | trace | stop flow |
| 8 | Operator health | monthly | review | med | rest, refer |

Dosimetry is read by the physicist and recorded by the ledger. The table's
final row matters most: dose is not just a badge number but a health
conversation, and the shelter treats it as care rather than as a score.

---

## 25. APPENDIX L — DRILL TABLE

| # | Drill | Scenario | Time target | Failure modes |
|---|---|---|---|---|
| 1 | Scram | rod drop | 1 min | slow hand |
| 2 | Evacuation | leak signal | 8 min | blocked route |
| 3 | Muster | assembly | 10 min | missed person |
| 4 | Restricted coolant | pump stop | 20 min | panic |
| 5 | Fire near vault | smoke | 10 min | wrong extinguisher |
| 6 | Leak response | trace found | 30 min | missed spot |
| 7 | Cask move | dry run | 2h | seat, seal |
| 8 | Power loss | grid down | 15 min | reserve start |
| 9 | Medical | exposure | 20 min | slow decon |
| 10 | Full review | tabletop | 1h | shallow review |

Drills are the shelter's promise that the terrible day is rehearsed. Each row
records a time target and the failure modes the review expects, so the drill
produces knowledge rather than merely proving bravery.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_core_warm_room` | 4 | Vault discovered warm |
| `quest_core_badge` | 4 | Dosimeters issued |
| `quest_core_rounds` | 3 | Hourly rounds walked |
| `quest_core_loop` | 5 | Real cooling loop built |
| `quest_core_wall` | 5 | Shielding measured |
| `quest_core_boundary` | 3 | Boundaries painted |
| `quest_core_shift` | 4 | Rota balanced |
| `quest_core_snap` | 5 | Cold snap handled |
| `quest_core_scram` | 5 | Scram and restart |
| `quest_core_spent` | 4 | First cask prepared |
| `quest_core_survey` | 4 | Trace found and handled |
| `quest_core_log` | 3 | Honest log culture |
| `quest_core_yard` | 5 | Cask yard built |
| `quest_core_review` | 4 | Blameless review |
| `quest_core_warm_thing` | 3 | Final disposition |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_core_output_plan` | 4 | Plan written |
| `quest_core_demand` | 3 | Demand read |
| `quest_core_console` | 4 | Console improved |
| `quest_core_handover` | 3 | Handover written |
| `quest_core_reserve` | 3 | Reserve kept |
| `quest_core_pump` | 4 | Pump repaired |
| `quest_core_intake` | 3 | Intake cleared |
| `quest_core_exchanger` | 4 | Exchanger serviced |
| `quest_core_water` | 3 | Water tested |
| `quest_core_winter` | 4 | Winter plan |
| `quest_core_survey_wall` | 4 | Wall surveyed |
| `quest_core_repair_wall` | 5 | Wall repaired |
| `quest_core_panels` | 3 | Panels fitted |
| `quest_core_distance` | 3 | Distance used |
| `quest_core_lead_store` | 3 | Store controlled |
| `quest_core_geometry` | 4 | Geometry rules |
| `quest_core_exclusion` | 3 | Exclusion marked |
| `quest_core_procedure` | 3 | Cards posted |
| `quest_core_training` | 5 | Operators trained |
| `quest_core_audit` | 4 | Audit held |
| `quest_core_cask_build` | 5 | Cask built |
| `quest_core_pool` | 4 | Pool built |
| `quest_core_transfer` | 5 | Core moved |
| `quest_core_record_fuel` | 3 | Fuel recorded |
| `quest_core_grave_site` | 4 | Deep site chosen |
| `quest_core_badges` | 3 | Badges issued |
| `quest_core_survey_round` | 3 | Round walked |
| `quest_core_drill_scram` | 4 | Scram drilled |
| `quest_core_drill_evac` | 4 | Evac drilled |
| `quest_core_health_check` | 4 | Operator reviewed |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Reiska Morn** — reactor engineer. Reads the machine by sound and habit and
has a procedure for every mood it has ever had. Believes the core is safe
because the shelter is careful, not because the core is friendly.

**Anil Kesh** — health physicist. Carries a meter everywhere and argues about
boundaries with the patience of someone who has seen one trace. Wants dose to
be a conversation, not a verdict.

**Maura Lin** — cooling technician. Knows every pump by its vibration. Treats
water as a coworker and the intake as a person who can be offended.

**Edda Harn** — control operator. Watches the readouts with a mug in hand and
the log open. Refuses to stand a shift she would not want her sibling to
stand.

**Tol** — shielding mason. Measures walls with string and geometry and pours
with the calm of a person who knows exactly how thick certainty is.

**Sib** — waste warden. Keeps the cask yard the way a librarian keeps a shelf:
numbered, sealed, and unashamed of its patience.

**Yun Aral** — physicist. Explains decay curves with kitchen analogies and
keeps the old core documentation in a tin.

**Beck** — apprentice. Seventeen, supervised, and assigned to rounds because
rounds teach every gauge in the building by name.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Vault Door** — heavy, warm, and surveyed before every opening.
- **The Cooling Intake** — a screen, a rake, and the shelter's water promise.
- **The Warm Outfall** — a stream that never froze and now never will.
- **The Shielding Quarry** — lead, stone, and sand, measured by the barrow.
- **The Dose Shack** — badges on hooks and a physicist with a ledger.
- **The Muster Pad** — painted lines where everyone is counted.
- **The Cask Yard** — numbered seals and rails that lead somewhere patient.
- **The Far Boundary** — a line the shelter painted for its own future.
- **The Deep Vault** — the last room, prepared long before it is needed.
- **The Reactor Mast** — a signal, a drill, and a horn that everyone knows.

---

## 30. APPENDIX Q — REACTOR EVENT PROTOCOL TABLE

| Event | Signal | First action | Roles | Review |
|---|---|---|---|---|
| Heat rise | meter | reduce output | operator | yes |
| Coolant restriction | gauge | start emergency loop | tech | yes |
| Coolant depletion | alarm | scram | all | yes |
| Leak trace | survey | exclude, decon | physicist | yes |
| Scram | horn | confirm, log | all | yes |
| Fire | alarm | evacuate, isolate | watch | yes |
| Power loss | lights | reserve start | operator | yes |
| Cask damage | inspection | reseal, survey | warden | yes |
| Exposure | badge | ward referral | physicist | yes |
| False alarm | none | stand down, log | all | yes |

The event table is the reactor's emergency grammar. Each row names a signal, a
first action, the roles involved, and a mandatory review, so no one has to
improvise at the worst moment and no event is ever forgotten by the next shift.

---

## 31. APPENDIX R — WORKED 360-DAY REACTOR SCENARIO

**Days 1–20.** The vault measures warm; Reiska admits the logs are guesses;
anil issues the first badges and is argued with.

**Days 21–50.** Hourly rounds and handovers become habit; a cooling loop is
surveyed; the pump's vibration is measured and the first spare part is made in
the wheel shop.

**Days 51–80.** The vault wall is surveyed and found thin in one corner; Tol
pours a panel and the second survey reads clean.

**Days 81–110.** Exclusion lines are painted; the shield store is counted; the
rota is rewritten around eight-hour days and a reserve.

**Days 111–140.** Output planning begins; the grid learns the core's reserve;
a cold snap restricts the intake and the emergency loop carries the load.

**Days 141–170.** The first scram drill runs in four seconds; the restart
procedure is discovered to be a guess and is rewritten by the whole shift.

**Days 171–200.** Sib argues for a cask; the cask is built from foundry
castings and kiln brick; the yard is prepared with rails.

**Days 201–230.** A trace contamination is found on a tool crib swab; the
crib is decontaminated, the route traced to a bad seal, and the seal is
replaced.

**Days 231–260.** The first spent core is moved to the pool and then to a cask;
the move is practiced dry first and every step is recorded.

**Days 261–290.** A real scram during a storm teaches the shelter the value of
the rewritten procedure; the review is blameless and the log is complete.

**Days 291–320.** Winter cooling plan tested; the ward gets a survey boundary;
the operators get a rest room and a kettle.

**Days 321–360.** Year review: twenty-four procedures, ten drills, one
trace, zero mysteries, and a core that hums quietly under a shelter that has
earned it.

---

## 32. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Reiska stands at the vault door and listens before she looks, and the hum is
> the same hum it always is, and she opens the door anyway, because the job is
> not trust, the job is checking.

> Anil writes the reading on the wall chart and circles it twice and says the
> number out loud, and Edda writes it in the log, and the two of them have
> performed the most important ritual in the building with nothing but a meter
> and a pen.

> Tol mixes the pour for the corner wall and works alone and measures with
> string twice, and when the physicists survey it the next morning he is not
> thanked, because nobody thanks a wall, and the wall is the point.

> Beck walks the rounds with a lamp and a clipboard and learns the difference
> between the rumble of the pumps and the rumble of the pumps when one of them
> is unhappy, and the difference is the whole apprenticeship right there.

---

## 33. APPENDIX T — REACTOR SAFETY COVENANT

| Clause | Promise |
|---|---|
| Procedures | Every action has a written card |
| Logs | Every change is recorded with a reason |
| Cool first | Heat is the enemy that never rests |
| Distance | Time and distance are always valid shields |
| Dose | The ledger decides; no one argues with it |
| Stop authority | Anyone may stop a move |
| Two persons | Fuel is never moved alone |
| Records | Spent cores are numbered and patient |
| Drills | The worst day is rehearsed |
| Review | Every event changes a procedure |

The covenant is the expansion's first-class design object. A reactor is a
promise about attention, and this table is the shelter's signature.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Missed round | late finding | double round, review |
| Pump stop | heat rise | emergency loop |
| Intake clog | restricted flow | rake, restart |
| Thin wall | dose reading | panel, survey |
| Bad seal | trace | decon, replace |
| Wrong setting | load miss | plan, reset |
| Scram | outage | restart procedure |
| Fatigued operator | error | rest, rota |
| Cask damage | containment | reseal, survey |
| Hidden error | trust loss | open log, review |

No failure is fatal by design, and no failure is hidden by reward. The reactor's
whole culture is that bad news is delivered early and received calmly.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] No weapons, enrichment, or bomb content exists.
- [ ] No meltdown spectacle or radiation horror exists.
- [ ] `NuclearCoreLifecycleSystem` remains the core authority.
- [ ] The dose ledger is read-only in this plan.
- [ ] The grid keeps power distribution.
- [ ] All cores are fictional archetypes.
- [ ] Spent fuel is never dumped.
- [ ] Operators are professionals and fatigue is managed.
- [ ] Every event has a blameless review.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 36. APPENDIX W — GLOSSARY

- **Core** — the reactor fuel assembly and its state machine.
- **Scram** — the emergency insertion that stops the reaction.
- **Coolant state** — how much cooling the core is actually getting.
- **Heat state** — how close the core is to its thermal limits.
- **Shielding integrity** — how well the walls do their job.
- **Embrittlement** — the slow wear of materials under heat and time.
- **Criticality card** — the unchangeable rules for handling fuel.
- **Spent core** — fuel that can no longer run but still stays warm.
- **Dosimeter** — the badge that tells the truth.
- **ALARA** — as low as reasonably achievable, a habit rather than a slogan.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `NuclearCoreLifecycleSystem` | profiles | core state | procedures |
| `CoreOperationsSystem` | core state | plans, logs | core state |
| `CoolingSystem` | water | loop state | core state |
| `ShieldingSystem` | surveys | integrity, boundaries | dose |
| `CriticalitySafetySystem` | rules | audits | fuel state |
| `SpentFuelSystem` | spent storage | placement | core state |
| `HealthPhysicsSystem` | ledger | surveys, referrals | dose |
| `ReactorDrillSystem` | alarms | drill records | core state |
| `DoseLedgerSystem` | dose facts | ledger | nothing else |
| `PowerGridSystem` | generation | distribution | core state |
| `FluidLogisticsSystem` | water | delivery | cooling state |
| `MedicalPipelineCoordinator` | referrals | care | dose |
| `AlarmSystem` | drills | alarm state | reactor state |
| `ArchiveDeskSystem` | procedures | records | reactor state |
| `EpilogueChronicleBuilder` | milestones | chronicle | core state |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`core_procedures.json`** — `procedure_id`, `display_name`, `steps[]`,
`roles[]`, `duration_hours`, `criticality_rule`, `tags`.

**`core_shifts.json`** — `shift_id`, `hours`, `operator`, `supervisor`,
`fatigue_rule`, `reserve`, `tags`.

**`core_output_plans.json`** — `plan_id`, `display_name`, `setting`, `reserve`,
`grid_use[]`, `notes`, `tags`.

**`cooling_loops.json`** — `loop_id`, `display_name`, `pumps`, `exchanger`,
`flow`, `failure`, `tags`.

**`shielding_surveys.json`** — `survey_id`, `location`, `threshold`,
`action`, `repair`, `re_survey`, `tags`.

**`criticality_rules.json`** — `rule_id`, `display_name`, `statement`,
`audit`, `tags`.

**`spent_fuel.json`** — `record_id`, `display_name`, `decay_heat`, `cooling`,
`storage`, `disposition`, `tags`.

**`dosimetry_rounds.json`** — `round_id`, `frequency`, `tool`, `threshold`,
`action`, `tags`.

**`reactor_drills.json`** — `drill_id`, `scenario`, `time_target`,
`failure_modes[]`, `review`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Output delivered | planning | Core |
| Coolant margin | safety | Cooling |
| Shielding readings | protection | Shielding |
| Scrams per year | events | Core |
| Round completion | discipline | Operations |
| Drill times | readiness | Drills |
| Dose per worker | health | Ledger readout |
| Spent fuel inventory | stewardship | SpentFuel |
| Procedure revisions | learning | Logs |
| Reserve held | honesty | Plans |

Telemetry is diagnostic only; it never gates content and never becomes a score
against an operator.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Core profiles extended beyond the three legacy entries.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Core, shielding, grid, and dose authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows a cold snap, a scram, a spent core, and a survey trace.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No weapons, spectacle, or horror content exists.

---

## 41. APPENDIX AB — OPEN QUESTIONS FOR REVIEW

1. Should the reactor ever be shut down permanently?
2. How much of the shelter's power should depend on one core?
3. Do spent cores decay enough to be re-used, or only stored?
4. Should the exclusion zone ever shrink?
5. Are surprise drills ever acceptable, and at what cost to trust?
6. Who can order a scram besides the operator?
7. Does the core power the outposts, and how is that load planned?
8. Which department owns the deep vault after the core is placed?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AC — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Apprentices and operator families |
| 1 | 14 Above the Ash | Reactor warning and sky tie |
| 1 | 16 The Rebuilt Body | Shielding for prosthetics |
| 2 | 18 The Underneath | Deep vault and geology |
| 2 | 19 The Bitter Air | Decon routes for traces |
| 2 | 21 The Grid | Generation at the bus |
| 3 | 22 The Clean Flow | Cooling water and outfall |
| 3 | 23 The Alarm | Scram signals and evacuation |
| 3 | 25 The Iron Road | Cask moves by rail |
| 4 | 29 The Glass | Meters and survey optics |
| 4 | 30 The Press | Procedures and logs |
| 4 | 31 The Kiln | Shielding brick and casks |
| 5 | 33 The Weather | Cold snap cooling plans |
| 5 | 36 The Watch | Boundary and muster |
| 6 | 38 The Ward | Exposure care |
| 6 | 39 The Reagent | Coolant chemistry |
| 6 | 40 The Wheel | Pumps and exchanger parts |
| 7 | 43 The Question | Old core documentation |
| 7 | 44 The Outpost | Remote power planning |

Each hook is additive. The Core can ship alone, and every other expansion can
ship without it.

---

## 43. APPENDIX AD — ENDING PROSE SKETCHES

**The Quiet Core.** Procedures, surveys, and drills make the reactor an ordinary
department, and the loudest thing in the vault is the kettle in the break room.

**The Long Watch.** The core runs at a human pace, and the shelter plans its
nights and its winters around a machine it has learned to respect.

**The Cold Season.** The worst winter in years tests the intake and the rota,
and the shelter passes because it practiced, and the review records exactly why.

**The Deep Vault.** The first spent core is placed with numbers, seals, and a
short quiet ceremony, and the shelter stops pretending the future will handle
itself.

**The Honest Log.** A serious event happens, the log is complete, and the
shelter discovers that honesty is the safety system that never needs spare
parts.

**Fade.** A warm room, a green light, and two operators making tea, and the
core humming under a shelter that earns its power every day.

---

## 44. APPENDIX AE — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Meltdown cutscene | spectacle | procedural events |
| Glowing monster | horror | meters and boundaries |
| Weapons shortcut | harmful | hard prohibition |
| Miracle restart | dishonest | written procedure |
| Dumped waste | irresponsible | casks and vault |
| Heroic overtime | exploitation | rota and reserve |
| Hidden danger | unfair | surveys and alarms |
| Curse framing | fatalism | engineering and care |
| Useless drills | theater | timed and reviewed |
| Lone genius | unsafe | two persons, logs |

The list exists because reactors are the easiest machines to turn into either a
spectacle or a monster. The expansion's rule is that the drama is in the
discipline, and the discipline keeps everyone alive.

---

## 45. APPENDIX AF — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Procedures | 24 | 6,000 |
| Shifts | 8 | 2,000 |
| Output plans | 8 | 2,000 |
| Cooling loops | 10 | 3,000 |
| Surveys | 12 | 3,000 |
| Criticality rules | 10 | 2,500 |
| Spent fuel records | 8 | 2,500 |
| Dosimetry rounds | 8 | 2,000 |
| Drills | 10 | 3,000 |
| Core extensions | 6 | 2,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~59,000** |

---

## 46. APPENDIX AG — FIRST YEAR OF THE CORE

| Month | Focus | Milestone |
|---|---|---|
| 1 | Warm room | logs audited |
| 2 | Badges | dosimetry begins |
| 3 | Rounds | hourly discipline |
| 4 | Loop | real cooling |
| 5 | Wall | survey and repair |
| 6 | Boundary | exclusion marked |
| 7 | Rota | shifts balanced |
| 8 | Snap | cold tested |
| 9 | Scram | drill becomes procedure |
| 10 | Cask | yard ready |
| 11 | Survey | trace handled |
| 12 | Review | year audited |

A year of the core is a year of attention made routine, and the shelter ends it
with a warm room that is safe to enter and two operators who sleep well.

---

## 47. APPENDIX AH — OPERATOR TRAINING TABLE

| Month | Skill | Check | Supervisor |
|---|---|---|---|
| 1 | badges and rounds | log | physicist |
| 2 | readouts | blank console | operator |
| 3 | cooling basics | pump walk | tech |
| 4 | procedures | card drill | engineer |
| 5 | survey | meter check | physicist |
| 6 | shielding | barrow math | mason |
| 7 | fuel handling | dry run | warden |
| 8 | scram | timed drill | engineer |
| 9 | restart | supervised | engineer |
| 10 | documentation | old manuals | physicist |
| 11 | emergency | full drill | all |
| 12 | independent desk | verified shift | engineer |

Training runs a full year and ends with one verified shift at the desk. The
table is the expansion's answer to the question of how a shelter turns a
careful person into an operator without pretending that a month of reading is
experience.

---

## 48. APPENDIX AI — REACTOR INSTRUMENT TABLE

| Instrument | Measures | Check | Failure response |
|---|---|---|---|
| Output meter | watts | daily | estimate, repair |
| Heat gauge | core temp | daily | reduce output |
| Coolant gauge | flow | daily | loop swap |
| Level gauge | water | daily | refill |
| Survey meter | dose rate | weekly | boundary |
| Swab kit | contamination | weekly | decon |
| Badge reader | dose | weekly | review |
| Alarm panel | states | daily | test |
| Log board | records | daily | handover |
| Reserve clock | uptime | daily | restock |

Instruments are how the machine tells the truth, and the failure column is how
the shelter plans for a meter that lies. The reserve clock is included because
attention has a budget just like water and fuel.

---

## 49. APPENDIX AJ — Vault MAINTENANCE TABLE

| Task | Interval | Who | Consumes | Sign |
|---|---|---|---|---|
| Door seal check | monthly | mason | grease | log |
| Door track clean | monthly | apprentice | rags | board |
| Panel survey | quarterly | physicist | none | log |
| Panel refit | yearly | mason | lead, brick | log |
| Cable check | monthly | operator | tape | log |
| Lamp replace | as needed | operator | lamp | board |
| Floor swab | weekly | physicist | swabs | log |
| Vent filter | quarterly | tech | felt | log |
| Pool clean | monthly | warden | none | log |
| Cask seal | yearly | warden | seal | log |

The vault's maintenance list is short and unglamorous, which is exactly what a
safety system should look like. Every task is signed, and the yearly panel
refit is the largest single job in the reactor's calendar.

---

## 50. APPENDIX AK — EMERGENCY PLANNING ZONE TABLE

| Zone | Distance | Access | Status | Response |
|---|---|---|---|---|
| Inner cell | in contact | two persons | restricted | procedure only |
| Vault ring | same room | controlled | surveyed | badge, time |
| Building | adjacent | evacuated on alarm | marked | muster |
| Shelter | whole | shelter-in-place | posted | windows, vents |
| Near field | outside | cleared on leak | signed | walk, count |
| Far field | boundary | consulted | mapped | sample |

Zones are the geometry of safety. The table is drawn so the player can see the
whole response landscape at once and understand that distance is not fear but
arithmetic.

---

## 51. APPENDIX AL — RECORD RETENTION TABLE

| Record | Keep | Where | Who reads |
|---|---|---|---|
| Shift log | 10 years | archive | operators, review |
| Procedure revision | permanent | archive | all |
| Survey sheet | 10 years | dose office | physicist, review |
| Badge history | lifetime | dose office | physicist, person |
| Spent fuel record | permanent | cask yard | warden, review |
| Drill review | 10 years | archive | all |
| Event review | permanent | archive | all |
| Training record | career | archive | supervisor |
| Instrument calibration | 10 years | archive | all |
| Deep vault register | permanent | vault | keeper |

Records are the reactor's memory across staffing changes. The permanent rows
are the promises the shelter makes to people who are not born yet, and the
deep vault register is the quietest document in the game.

---

## 52. APPENDIX AM — OPERATOR REST TABLE

| Condition | Rest rule | Coverage | Return |
|---|---|---|---|
| Normal shift | 8h sleep window | two operators | next day |
| Overtime | 1.5x pay time | reserve called | review |
| Post-scram | 12h off | reserve | supervised |
| Night watch | 6h max | rotation | next week |
| Drill day | paid, fed | all | none |
| Training | day shift | one | none |
| Injury | ward rules | reserve | medical |
| Stress | counselor | rota | as needed |

Operator rest is a safety system, and the table treats it like one. Post-scram
rest is non-negotiable because the second event is always more dangerous than
the first, and the person who saw the first one is not the right operator for
the second.

---

## 53. APPENDIX AN — REACTOR EMERGENCY SUPPLY TABLE

| Supply | Stored | Location | Check | Use |
|---|---|---|---|---|
| Scram canister | 3 | control desk | monthly | scram |
| Spare pump part | 2 | cooling room | monthly | swap |
| Coolant reserve | 2000 L | tank | weekly | loop |
| Lead panel | 6 | shield store | monthly | repair |
| Survey meter | 2 | dose office | weekly | survey |
| Badges | 40 | dose office | monthly | rounds |
| Decon kit | 4 | vault door | monthly | trace |
| Fire blanket | 3 | vault, desk | monthly | fire |
| Cask seal | 4 | cask yard | monthly | fuel |
| Reserve lamp | 6 | control desk | weekly | power loss |
| Rope and sling | 2 | cask yard | monthly | moves |
| Log blanks | 100 | desk | monthly | records |

Emergency supplies are counted on the same rhythm as the reactor's ordinary
parts, because the day the shelter needs a scram canister is not the day to
discover it was borrowed. The table is also the shopping list for the rest of
Wave 7: casks, seals, and panels all come from the foundry, the kiln, and the
wheel shop.

---

## 54. APPENDIX AO — DOSE REVIEW TABLE

| Reading | Meaning | Action | Reviewer |
|---|---|---|---|
| Below threshold | ordinary work | log | physicist |
| At threshold | attention | rotate, review | physicist |
| Above threshold | concern | remove from duty, ward | physicist, medic |
| Trace contamination | surface | decon, reswab | physicist |
| Instrument drift | uncertainty | recalibrate | physicist |
| Missing badge | unknown | interview, reissue | physicist |
| Repeated high | pattern | redesign work | engineer |
| Emergency dose | event | care first, review second | all |

Dose reviews are conversations, not verdicts. The repeated-high row is the
important one: when the same reading appears twice, the shelter changes the
work rather than blaming the worker, which is how real safety culture behaves.

---

## 55. APPENDIX AP — SEASONAL REACTOR OPERATION TABLE

| Season | Load | Cooling | Staffing | Notes |
|---|---|---|---|---|
| Spring | steady | good | normal | maintenance window |
| Summer | high | stressed | normal | intake heat |
| Autumn | steady | good | normal | refuel prep |
| Winter | high | cold risk | doubled | freeze watch |
| Ash | normal | filter watch | normal | dusty intake |
| Storm | low | reserve | all hands | grid tie |

The reactor has a year like everything else in the shelter. The table is how the
core plans its maintenance in the mild months and its vigilance in the hard
ones, and why the freeze watch in winter is doubled rather than improvised.

---

## 56. APPENDIX AQ — CORE CONTROL DESK TABLE

| Item | Purpose | Check | Backup |
|---|---|---|---|
| Output meter | reads generation | daily | estimate |
| Heat gauge | core temperature | daily | hand check |
| Coolant gauge | flow state | daily | sight glass |
| Level gauge | water level | daily | dip stick |
| Alarm panel | state changes | weekly | manual watch |
| Log board | records | daily | notebook |
| Procedure rack | cards | monthly | spare copies |
| Reserve clock | attention budget | daily | roster |
| Scram handle | emergency stop | weekly | button |
| Kettle | sanity | always | patience |

The control desk is a list of ordinary objects that together make a dangerous
machine governable. The final row is not a joke: the kettle is part of the shift
plan, and the shelter has decided that a calm operator is a safety feature.

---

## 57. APPENDIX AR — NUCLEAR WASTE PATIENCE TABLE

| Material | Heat | Wait | Store | Final |
|---|---|---|---|---|
| Spent core | high | months | pool | deep vault |
| Resin | low | none | drum | deep vault |
| Sludge | medium | weeks | tank | deep vault |
| Tools | none | none | box | deep vault |
| Filters | low | none | drum | deep vault |
| Fuel dust | low | none | sealed | deep vault |

The table's middle column is the expansion's whole attitude to spent fuel: it
does not stop existing because the shelter is done with it. Patience, records,
and a vault that will outlast everyone who built it are the only answers the
shelter has, and it gives them honestly.

---

## 58. CLOSING STATEMENT

ASHFALL already runs cores as state machines: profiles, output settings, coolant
and heat states, shielding integrity, embrittlement, scrams, and leak events.
What it lacks is everything around the machine: procedures, shifts, cooling
loops, surveys, boundaries, criticality discipline, spent fuel, dosimetry, and
drills. The Core adds that world without adding a weapon or a spectacle. It adds
a warm room with rules, a log that never lies, two operators making tea while
the meters tick, and a shelter that is worthy of the machine it inherited.

> Wave 7 note: this plan is one of five Wave 7 expansion bibles (42–46). Each is
> self-contained; none requires another to ship. The shared Wave 7 index lives
> at `docs/expansions/wave7/WAVE7_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `NuclearCoreLifecycleSystem` (`InstalledCores`,
> `SpentCoreStorage`, `OnReactorScrammed`, `OnHeatStateChanged`,
> `OnRadiationLeak`, `TryInstallCore`, `SetOutputSetting`,
> `GetTotalGenerationWatts`, `TryRepairShielding`, `TryEmergencyScram`,
> `TickDay(currentDay, coolantOverride)`, `PowerSourceId = "nuclear_core"`),
> `ReactorCoreState` (`outputSetting`, `coolantState`, `heatState`,
> `shieldingIntegrity`, `embrittlementWear`, `isScrammed`, `roomId`),
> `NuclearCoreDefinition` (`powerClass`, `baseElectricalOutput`,
> `thermalClass`, `radiationClass`, `coolingDemand`, `shieldingRequirement`,
> `wearRate`, `decayClass`, `emergencyShutdownItemId`), and
> `nuclear_core_profiles.json` (3,314 B, three profiles: a strontium RTG, a
> submersible pebble-bed core, and a civilian research TRIGA).