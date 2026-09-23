# ASHFALL — Expansion 47 Design Bible
# THE BRIGADE
### Wave 8 · Fire Prevention, Inspections, Brigade Service, Drills, Equipment, and the Review of Burns

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Shelter` (`ShelterFireHazardSystem`), `Ashfall.Core` (`VentilationSystem`), `Ashfall.Core.Survivors` (`NeedsSystem`), `Ashfall.Core.Shelter` (`Inventory` seams)
**Proposed host owner:** `FireBrigadeHostSession` (extends `ShelterFireHostSession` + `ShelterFireSaveStore`)
**Existing save sections:** `shelter_fire` (`shelter_fire_save.json`, `ShelterFireSaveState.Incidents`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no fire-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has a working fire incident simulation. `ShelterFireHazardSystem`
defines `FireZoneState` (`zoneId`, `displayName`, `fireLevel`, `smokeLevel`,
`coLevel`, `heatLevel`, `damperOpen`, `isEvacuated`, `adjacentZoneIds`) and
`FireIncidentState` (`incidentId`, `sourceZoneId`, `ignitionDay`, `ticksElapsed`,
`alarmRaised`, `isSuppressed`, `isResolved`, `resolution`, `zones`,
`brigadeWorkers`, `extinguisherChargesUsed`, `structuralDamage`). Its constants
are explicit: `FireSpreadRate` 0.05 per tick, `FireDecayRate` 0.02,
`SmokePerFireUnit` 0.3, `CoPerFireUnit` 0.1, `HeatPerFireUnit` 0.4,
`SmokeDecayRate` 0.05, `CoDecayRate` 0.01, `HeatDecayRate` 0.1,
`BrigadeSuppressionPerWorker` 0.08, `ExtinguisherSuppression` 0.25,
`ExtinguisherMaxCharges` 4, `DamperSmokeReduction` 0.5,
`StructuralDamagePerTick` 0.02, `CriticalSmokeLevel` 0.6, `CriticalCoLevel` 0.4,
`LethalCoLevel` 0.8. The API is real: `Ignite(incidentId, sourceZoneId, day,
zones)`, `RaiseAlarm`, `AssignBrigade`, `SetDamper`, `DeployExtinguisher`,
`EvacuateZone`, `Tick(incidentId, rng)`, `GetIncident`, `IsResolved`,
`GetMaxCoLevel`, `GetMaxSmokeLevel`, plus events `OnAlarmRaised`,
`OnFireIgnited`, `OnSmokeZoneChanged`, `OnDamperChanged`, `OnBrigadeDispatched`,
`OnEquipmentDamaged`, `OnSurvivorExposed`, and `OnIncidentSuppressed`. The host
session, the save store, and the `FireIncidentPanel` already exist.

What does not exist: any authored fire content at all. There is no fire zone
layout, no risk catalog, no inspection program, no brigade roster beyond a
worker list, no drill content, no fire equipment items (a search of `items.json`
for extinguisher items returns zero), no cause catalog, and no post-incident
review. The live ignition path in `src/Main.Plans162_165.cs` calls `Ignite` with
a **room id and an empty zone list**, so a shelter fire can begin but has no
authored propagation graph, no fire load, and no prevention story.

**The Brigade** gives the shelter a fire service: inspections before the fire,
teams and equipment during it, drills that rehearse both, and a review after
that changes practice. It extends the live system and never duplicates it.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `ShelterFireHazardSystem` | Fire, smoke, CO, heat, suppression | Extends it with content and commands |
| `VentilationSystem` | Airflow and filtration | Sends smoke/CO into it; never rewrites it |
| `NeedsSystem` | Needs and morale | Uses `Modify` for fatigue and composure only |
| `ShelterNoiseSystem` (Wave 6) | Alarms as noise sources | Registers drill/bell noise through it |
| `AlarmSystem` (Wave 3) | Shelter alarm states | Reuses on urgent calls; never replaces |
| `MedicalWardSystem` (Wave 6) | Burns and smoke injury | Routes every casualty to it |
| `PowerGridSystem` (Wave 2) | Power and isolation | Reports electrical ignition; never cuts power itself |
| `DutyRoster` (Exp 02) | Shifts | Brigade assignments ride the roster; no second rota |
| `ShelterSecurity` (Plan 138) | Lockdowns and breaches | Reads zone locks; never owns them |
| `ExcavationHazards` | Subterranean hazards | Reads adjacent-zone data only |
| `SanitationSystem` (Wave 3) | Waste and spills | Reads fuel/rags as fire load sources |
| `StandingRecord` (Exp 03) | Records | Files inspection and review records |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Thirty meters of corridor, six rooms, one kitchen, one generator hall, and
ninety-four people who all know where the extinguishers are — except that
nobody has counted the extinguishers, nobody has walked the corridor at night
looking for heat, and nobody has ever drilled the evacuation with the north
stair blocked.

**The Brigade** is the expansion about the fire the shelter keeps having to
prevent: inspections, risks, permits, teams, equipment, drills, and reviews. It
turns a working fire sim into a practice of prevention, and it treats fire as
what it is: a quiet neighbor that only gets loud once.

### 1.2 The five loops it adds

```
  Inspect ──► Rate ──► Fix ──► Permit ──► Record
     │          │        │        │         │
     ▼          ▼        ▼        ▼         ▼
   Rounds,   Fire     Repairs,  Hot-work  Register
   hazards   loads    clears    rules     updates
                                        │
                                        ▼
                        Respond ──► Review ──► Retrain
```

### 1.3 What the player manages

1. **Zones.** The authored layout: adjacency, dampers, doors, exits.
2. **Fire loads.** What burns in each room and how fast.
3. **Ignition risks.** Heat, dust, fuel, wiring, cooking, lamps, friction.
4. **Inspections.** Rounds, findings, time to fix, re-checks.
5. **Permits.** Hot work, storage, occupancy, isolation.
6. **Brigade.** Teams, roles, equipment, call order, fatigue.
7. **Equipment.** Extinguishers, blankets, sand, hose, hoods, keys.
8. **Drills.** Evacuation, damper closing, roll call, blocked exits.
9. **Response.** Alarm, dispatch, isolate, suppress, evacuate, recover.
10. **Review.** Cause, lessons, changes, and grief when someone is hurt.

### 1.4 What it is not

- Not a second fire simulation; the live system stays the single authority.
- Not a spectacle; fires are short, ugly, and expensive.
- Not an arson or sabotage reward loop.
- Not a second alarm, needs, or medical system.
- Not a punishment framing; prevention failures are process failures.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | Fire, smoke, CO, suppression | `LIVE` |
| `Assets/Ashfall.Core/VentilationSystem.cs` | Smoke and CO sink | `LIVE` |
| `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | Fatigue, composure | `LIVE` |
| `src/Host/ShelterFireHostSession.cs` | Day tick, incident id, events | `LIVE` |
| `src/Host/ShelterFireSaveStore.cs` | `shelter_fire` persistence | `LIVE` |
| `src/UI/FireIncidentPanel.cs` | Alarm, brigade, extinguisher commands | `LIVE` |
| `src/Main.Plans162_165.cs` | Live ignition path | `LIVE` |

### 2.2 Live data (counted)

| Surface | Size | Notes |
|---|---|---|
| Fire zone layout | absent | confirmed none |
| `items.json` extinguisher items | 0 matches | confirmed none |
| Fire risk, inspection, drill, cause catalogs | absent | confirmed none |
| Rooms and zone ids | live room registry | reused, not duplicated |

### 2.3 Confirmed gaps

- **GAP-47-1 — No authored zone layout or adjacency.**
- **GAP-47-2 — Live ignition passes an empty zone list.**
- **GAP-47-3 — No fire load or ignition risk catalog.**
- **GAP-47-4 — No inspection or re-check content.**
- **GAP-47-5 — No permit or hot-work rules.**
- **GAP-47-6 — No brigade roles, call order, or training content.**
- **GAP-47-7 — No fire equipment items.**
- **GAP-47-8 — No drill or evacuation content.**
- **GAP-47-9 — No cause catalog or post-incident review.**
- **GAP-47-10 — No prevention experience before the first incident.**

### 2.4 Non-duplication statement

This expansion will **not** add a second fire, smoke, alarm, needs, medical,
ventilation, power, or roster system. It extends `ShelterFireHazardSystem` with
content and prevention commands, routes injury to `MedicalWardSystem`, fatigue
to `NeedsSystem.Modify`, noise to `ShelterNoiseSystem`, alarms to `AlarmSystem`,
and records to `StandingRecord`. All new state is additive inside
`ShelterFireSaveState`. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Fire is prevented in daylight.** The dramatic moment is the
inspection that found the dry rag bin, not the alarm.

**Pillar 2 — Every space has a heat budget.** Fuel, air, and ignition are
countable things, and the shelter counts them.

**Pillar 3 — A drill is a promise kept.** Rehearsal is how a thin crew of
volunteers beats a fire they cannot outrun.

**Pillar 4 — Smoke is the true hazard.** CO and smoke kill quietly; the plan
treats them with the respect the live system's constants already encode.

**Pillar 5 — No blame, only cause.** Reviews find conditions, not culprits.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Prevention | Rounds, clipboards, small fixes | Omniscience |
| Fire | Short, loud, costly, fast | Spectacle or glory |
| Brigade | Volunteers with day jobs | Hero mythology |
| Drills | Honest mistakes and retraining | Humiliation |
| Injury | Routed to the ward, taken seriously | Graphic detail |
| Cause | Conditions and chains | Culprits and vendettas |
| Equipment | Counted, checked, hung properly | Gadget worship |
| Review | Changes to practice | Paralysis |

### 3.3 Content limits

- No arson-for-profit, sabotage-reward, or insurance-fraud content.
- No children in firefighting roles; apprentices are 16+ and supervised, and
  never enter a burning zone.
- No graphic burns; medical outcome is stated, not shown.
- No contempt for the careless; the review is blameless by design.
- No second simulation, no new save section.

---

## 4. THE BRIGADE WORLD

### 4.1 Interior rooms

- **`room_fire_hall`** — the shed with the gear, the roster, and the bell.
- **`room_inspection_desk`** — clipboards, maps, findings boards.
- **`room_equipment_store`** — extinguishers, blankets, sand, hose.
- **`room_drill_yard`** — the training run with a blank door and a dummy.
- **`room_alarm_panel_fire`** — the fire bell and the zone board.
- **`room_review_room`** — causes pinned on a wall, no names.
- **`room_hot_work_shop`** — permitted cutting and welding with screens.
- **`room_watch_walk`** — the night round that smells the corridor.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_burn_yard` | The Burn Yard | 3 | Practice fires and drills |
| `loc_generator_hall` | The Generator Hall | 4 | Electrical ignition risk |
| `loc_kitchen_gallery` | The Kitchen Gallery | 3 | Cooking fires, grease |
| `loc_storage_aisle` | The Storage Aisle | 3 | Fire load and stacking |
| `loc_north_stair` | The North Stair | 4 | Blocked-exit scenario |
| `loc_water_tank_fire` | The Fire Cistern | 2 | Water source for hose lines |
| `loc_workshops_row` | The Workshops Row | 3 | Hot work and screens |
| `loc_laundry_flue` | The Laundry Flue | 3 | Lint and dryer fires |
| `loc_bunker_gate_fire` | The Gatehouse | 2 | External fire spread |
| `loc_memorial_fire` | The Fire Memorial | 2 | Names and causes recorded |

All locations require valid item references and scanner registration.

### 4.3 The daily loop

Night walk checks heat and smells; day shift inspects one zone; findings are
fixed or scheduled; permits gate hot work; drills happen monthly and after
every review. The expansion's clock is the shift, not the alarm.

---

## 5. MAIN STORYLINE — "WHAT WE KEEP FROM BURNING"

### 5.1 Central conflict

**Orlen Drave** has been the shelter's unofficial fire warden for years and has
been losing sleep over the storage aisle. **Oona Brill** arrives from a
neighboring trade stop with dated inspection habits and a devastating question:
how many extinguishers does this shelter actually have? **Ivar Foss** counts for
two days and reports: eleven, four of them empty, none of them ever tested.
**Nedda Nunn** wants a brigade that can actually enter a room. **Pold Greave**
wants causes catalogued so that reviews do not become arguments. **Pimmett
Alard** wants the stairs walked with a child on his back.

Then the laundry flue catches during the night shift and the shelter discovers
that its smoke travels exactly the way the ventilation routes it, and its
brigade has never entered a hot room together. Afterwards, the review finds
conditions — lint, a missing screen, a damper nobody was assigned to close —
and the shelter chooses whether to build a service or a story.

The expansion's question: **what does prevention look like when nothing has
burned yet?**

### 5.2 Theme (unspoken)

**A fire service is mostly not a fire service.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_brigade_chief_orlen_drave` | Orlen Drave | Chief | Standards, decisions |
| `npc_inspector_oona_brill` | Oona Brill | Inspector | Rounds and findings |
| `npc_equipment_ivar_foss` | Ivar Foss | Equipment | Counts, tests, stores |
| `npc_rescue_nedda_nunn` | Nedda Nunn | Rescue lead | Entries and rescue |
| `npc_investigator_pold_greave` | Pold Greave | Investigator | Causes and reviews |
| `npc_stair_warden_pimmett_alard` | Pimmett Alard | Evacuation | Routes and roll call |
| `npc_apprentice_rill_ondine` | Rill Ondine | Apprentice | Night walks |
| `npc_store_effie_onsey` | Effie Onsey | Store keeper | Gear and charges |

### 5.4 Story beats (15)

1. **The Count.** Extinguishers are counted and found wanting.
2. **The Walk.** The first night round finds three heat smells.
3. **The Layout.** Zones, adjacency, and dampers are drawn.
4. **The Load.** The storage aisle is measured and stacked again.
5. **The First Drill.** Evacuation with the north stair blocked.
6. **The Permit.** Hot work gets rules after a near miss.
7. **The Flue.** The laundry fire starts and the brigade runs.
8. **The Review.** Cause, conditions, and no names.
9. **The Cistern.** Water supply becomes a real plan.
10. **The Brigade Forms.** Roles, call order, and refusal to enter.
11. **The Smoke Test.** CO spreads through two zones and is tracked.
12. **The Second Drill.** The stair is blocked again; the time improves.
13. **The Record.** Every finding, fix, and drill enters the register.
14. **The Successor.** Rill walks a night round alone and finds a bad joint.
15. **What We Keep From Burning.** The service becomes ordinary.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Standards | strict / moderate / informal | risk vs. freedom |
| Inspections | daily / weekly / monthly | labor vs. coverage |
| Brigade entry | trained only / all volunteers / nobody | rescue vs. safety |
| Equipment | full / minimal / improvised | cost vs. readiness |
| Drills | rigorous / regular / occasional | realism vs. morale |
| Reviews | public / brigade / private | learning vs. privacy |
| Hot work | permit only / notice / open | control vs. friction |
| Final | service as institution / volunteers / habit | identity |

### 5.6 Endings (5 + fade)

1. **The Service** — the brigade is a real institution with standards,
   equipment, and drills, and the shelter knows it can answer.
2. **The Watchful House** — prevention becomes habit: rounds, counts, permit
   discipline, and almost no fires.
3. **The Cost of Entry** — a rescue costs the shelter someone; the review turns
   the loss into rules about entry that save the next person.
4. **The Rebuilt Haul** — the storage aisle and flue are rebuilt to standard
   and the fire load is a number the shelter maintains.
5. **The Quiet House** — fires stop happening and the brigade spends its time
   teaching children where the exits are.
6. **Fade** — a wall of inspection cards, each initialed and dated, and one
   empty hook where an extinguisher is being refilled.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_brigade_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_brigade_count`, `quest_brigade_walk`, `quest_brigade_layout`,
`quest_brigade_load`, `quest_brigade_first_drill`, `quest_brigade_permit`,
`quest_brigade_flue`, `quest_brigade_review`, `quest_brigade_cistern`,
`quest_brigade_form`, `quest_brigade_smoke_test`, `quest_brigade_second_drill`,
`quest_brigade_record`, `quest_brigade_successor`, `quest_brigade_what_we_keep`.

### 6.2 Side quests (30)

**Equipment (5)**
- `quest_brigade_extinguishers` — count and test
- `quest_brigade_blankets` — place fire blankets
- `quest_brigade_sand` — fill sand buckets
- `quest_brigade_hose` — lay and dry a hose
- `quest_brigade_hoods` — smoke hoods and their limits

**Inspection (5)**
- `quest_brigade_kitchen` — grease and gaskets
- `quest_brigade_wiring` — heat checks on runs
- `quest_brigade_rags` — oily rag discipline
- `quest_brigade_stack` — storage height and aisles
- `quest_brigade_lamps` — lamp and candle placement

**Brigade (5)**
- `quest_brigade_roles` — assignments and call order
- `quest_brigade_fitness` — air and stamina
- `quest_brigade_damper_drill` — damper closing under time
- `quest_brigade_casualty` — drag and carry practice
- `quest_brigade_pairing` — never enter alone

**Prevention (5)**
- `quest_brigade_permit_desk` — hot work permits
- `quest_brigade_screens` — sparks and screens
- `quest_brigade_isolate` — power isolation before cutting
- `quest_brigade_rewire` — replace a bad run
- `quest_brigade_flue_clean` — flue sweeping rota

**Aftermath (5)**
- `quest_brigade_cause` — find the origin
- `quest_brigade_smoke_damage` — clean smoke residue
- `quest_brigade_water` — pump out and dry
- `quest_brigade_memorial` — record the loss
- `quest_brigade_change` — change one practice

**Evacuation (5)**
- `quest_brigade_routes` — mark every route
- `quest_brigade_roll_call` — count at the muster point
- `quest_brigade_blocked` — blocked-exit variants
- `quest_brigade_children` — children's drill and adults leading
- `quest_brigade_aids` — help for those who need it

### 6.3 Repeatable quests (8)

`quest_brigade_repeat_round`, `quest_brigade_repeat_extinguisher`,
`quest_brigade_repeat_cleaning`, `quest_brigade_repeat_drill`,
`quest_brigade_repeat_permit`, `quest_brigade_repeat_review`,
`quest_brigade_repeat_store`, `quest_brigade_repeat_readiness`.

### 6.4 Dynamic hooks

Live events (`OnFireIgnited`, `OnAlarmRaised`, `OnSurvivorExposed`,
`OnIncidentSuppressed`, `OnEquipmentDamaged`, ventilation state changes, grid
faults, sanitation spills, weather dry spells) attach authored follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- Fire simulation stays with `ShelterFireHazardSystem`.
- Smoke and CO stay with `VentilationSystem`.
- Injury stays with the medical owners; death stays with the memorial owners.
- Fatigue and composure use `NeedsSystem.Modify`.
- Drill noise registers through `ShelterNoiseSystem`.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `FireZoneLayoutSystem` (extend `ShelterFireHazardSystem`)

**Owns:** authored zone graphs, adjacency, dampers, doors, exits, and the
starting zone set passed to `Ignite`. **Consumes:** the live room registry,
`VentilationSystem` ducts, `ShelterSecurity` locks. **Data:** `fire_zones.json`.
**Rules:** the layout is data, not code; the live ignition path resolves zones
from the layout so fires propagate on authored maps; a missing layout is a
validation error.

### 7.2 `FireLoadSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** fire load per room, fuel classes, stacking limits, and clear-aisle
state. **Consumes:** `Inventory` contents, `SanitationSystem` waste, workshop
materials. **Data:** `fire_risks.json`. **Rules:** fire load is a number that
raises spread and intensity; storage discipline lowers it; nothing here mutates
inventory.

### 7.3 `InspectionSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** inspection rounds, findings, severity, deadline, fix, and re-check.
**Consumes:** fire load, risks, live equipment counts, `DutyRoster` shifts.
**Data:** `fire_inspections.json`. **Rules:** every finding has an owner and a
deadline; overdue findings raise risk; re-checks close findings.

### 7.4 `PermitSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** hot-work permits, storage permits, occupancy limits, and isolation
preconditions. **Consumes:** inspections, grid isolation state, workshop
schedule. **Data:** `fire_permits.json`. **Rules:** cutting, welding, and
grinding require a permit; a permit requires a screen, an extinguisher, and an
isolation check; unsupervised hot work raises ignition risk.

### 7.5 `BrigadeSystem` (extend `ShelterFireHazardSystem`)

**Owns:** brigade membership, roles, call order, entry pairs, fitness, and
fatigue. **Consumes:** `DutyRoster`, `NeedsSystem`, live incident workers.
**Data:** `brigade_roles.json`, `fire_drills.json`. **Rules:** `AssignBrigade`
receives the roster from this system; entry requires a pair and a hood; tired
crews suppress less, which the live constants already model per worker.

### 7.6 `FireEquipmentSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** equipment items, locations, charges, tests, and stores. **Consumes:**
`Inventory` (existing item authority), `ShelterWorkshopSystem` for repairs.
**Data:** `fire_equipment_catalog.json`. **Rules:** `DeployExtinguisher`
consumes a charged unit; empty units are bayonets without a blade; quarterly
tests mark readiness; `ExtinguisherMaxCharges` remains the live limit.

### 7.7 `FireDrillSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** drill types, scenarios, timings, roll call, and lessons. **Consumes:**
zone layout, `ShelterNoiseSystem`, `NeedsSystem` for composure. **Data:**
`fire_drills.json`, `evacuation_routes.json`. **Rules:** drills are scheduled
and timed; blocked-exit variants are authored; roll call is counted at a muster
point; a failed drill produces training, never punishment.

### 7.8 `IncidentReviewSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** causes, chains of conditions, lessons, and practice changes. **Data:**
`fire_causes.json`, `incident_reviews.json`. Records through `StandingRecord`.
**Rules:** reviews name conditions, not people; every review must change at
least one practice or explicitly record why none changes; injuries route
through the medical and memorial owners.

### 7.9 Systems explicitly not added

- No second fire, smoke, ventilation, alarm, or medical simulation.
- No arson reward loop.
- No hero-fantasy brigade mechanics.
- No second roster; brigade duty rides `DutyRoster`.
- No new power, needs, or inventory authority.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `fire_zones.json` (new)

```json
{
  "schema_version": 1,
  "zones": [
    {
      "zone_id": "fire_zone_corridor_bunker",
      "display_name": "Bunker Corridor",
      "room_tags": ["corridor"],
      "adjacent_zone_ids": ["fire_zone_kitchen", "fire_zone_stair_north"],
      "damper_id": "damper_corridor",
      "has_exit": true,
      "fire_load_base": 12,
      "tags": ["circulation", "primary"]
    }
  ]
}
```

### 8.2 `fire_risks.json` (new)

Risks: zone, kind, base rate, fuel class, mitigations, inspection cadence.

### 8.3 `fire_inspections.json` (new)

Findings: area, finding, severity, deadline days, fix action, re-check state.

### 8.4 `fire_permits.json` (new)

Permits: kind, zone, preconditions, duration, screen, extinguisher, isolation.

### 8.5 `brigade_roles.json` (new)

Roles: name, duties, entry allowed, fitness need, call order position.

### 8.6 `fire_equipment_catalog.json` (new)

Equipment: item id, kind, charges, test interval, placement zones, note.

### 8.7 `fire_drills.json` (new)

Drills: scenario, blocked exits, target minutes, roll call, lessons.

### 8.8 `fire_causes.json` (new)

Causes: kind, conditions chain, likelihood modifiers, review questions.

### 8.9 `evacuation_routes.json` (new)

Routes: route id, zones covered, muster point, alternatives, aids.

### 8.10 `incident_reviews.json` (new)

Reviews: incident, cause, conditions, lesson, practice change, record link.

### 8.11 Items

New items appended to `items.json`: `item_fire_extinguisher`,
`item_fire_blanket`, `item_sand_bucket`, `item_fire_hose`, `item_smoke_hood`,
`item_damper_key`, `item_screen_stand`, `item_hot_work_permit`,
`item_inspection_clipboard`, `item_fire_load_chart`, `item_alarm_bell_fire`,
`item_cause_jar`, `item_drill_dummy`, `item_evac_sign`, `item_refill_kit`,
`item_review_board`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ShelterFireSaveState` remains the live save owner. New sub-objects (layout
reference, loads, inspections, permits, brigade roster, equipment state, drills,
reviews) are additive inside it. No new save section.

### 9.2 State to persist

- Authored layout id and any shelter-specific zone edits.
- Fire loads and stacking state per room.
- Inspection findings, deadlines, and re-checks.
- Permit history and open permits.
- Brigade roster, roles, and fitness flags.
- Equipment placements, charges, and last test day.
- Drill results and roll-call times.
- Review records and practice changes.

### 9.3 Determinism

- Ignition, spread, smoke, CO, heat, and suppression remain in the live
  system's seeded `Tick`.
- Inspection outcomes derive from state, not rolls; where a roll is needed it
  uses the existing live path.
- Drill timings derive from route length, crowd size, and door state.
- Review text is projected from recorded facts.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with incidents intact and no inspection, permit, drill, or
equipment state; a shelter that already had a fire keeps its incident history.
The first inspection round after load backfills equipment counts from the
authored catalog as "untested".

### 9.5 Checksum

Invariant-culture floats; integer day, count, and charge fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `FireIncidentPanel` (extend) | Live incident commands | `FireBrigadeHostSession` |
| `FirePreventionPanel` (new) | Layout, loads, risks | same |
| `InspectionPanel` (new) | Findings and deadlines | same |
| `PermitPanel` (new) | Hot work and storage | same |
| `BrigadePanel` (new) | Roster, roles, readiness | same |
| `DrillPanel` (new) | Scenarios and results | same |
| `FireReviewPanel` (new) | Causes and changes | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Alarm states are shown as text as well as color; no color-only signal.
- The incident panel keeps the existing alarm/brigade/extinguisher buttons and
  adds no new authority.
- Drill timers can be paused by the drill officer in-fiction, never by hiding
  information.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a bell, a damper handle, a charge
hiss, water on hot metal, a roll call, a clipboard hook. No cue is required;
text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ShelterFireHazardSystem` | Zones, brigade, suppression, review |
| `VentilationSystem` | Smoke and CO routing |
| `NeedsSystem` | Fatigue and composure |
| `AlarmSystem` (Wave 3) | Urgent calls |
| `ShelterNoiseSystem` (Wave 6) | Drill and bell noise |
| `MedicalWardSystem` (Wave 6) | Burns and smoke injury |
| `DoseLedgerHostSession` (Exp 07) | Nothing; smoke is not radiation |
| `PowerGridSystem` (Wave 2) | Isolation checks |
| `DutyRoster` (Exp 02) | Brigade shifts and call order |
| `ShelterSecurity` (Plan 138) | Locks and zone access |
| `SanitationSystem` (Wave 3) | Grease, lint, and waste fire load |
| `ShelterWorkshopSystem` (Wave 6) | Extinguisher and kit repair |
| `StandingRecord` (Exp 03) | Inspections, drills, reviews |
| `MemorialSystem` (Wave 3) | Loss records |
| `EpilogueChronicleBuilder` | Fire history in the record |
| `FieldGuide` (Plan 20A/28) | Hazard reading entries |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm the fire system, host, store, panel,
ventilation sink, needs, roster, and record owners. Record file:line; change
nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner; make the empty-zone ignition path a validation error.

**Phase 2 — Pure Core.** `FireZoneLayoutSystem`, `FireLoadSystem`,
`InspectionSystem`, `PermitSystem`, `BrigadeSystem`, `FireEquipmentSystem`,
`FireDrillSystem`, `IncidentReviewSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `FireBrigadeHostSession`, focused selftest coverage,
fresh journey from the count to the review.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-incident soak: prevention lowers fires; drills
lower evacuation time; neglected inspection raises severity.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Fire zones | 24 |
| Fire risks | 24 |
| Inspection findings | 40 |
| Permits | 12 |
| Brigade roles | 10 |
| Equipment entries | 16 |
| Drills | 12 |
| Causes | 20 |
| Evacuation routes | 8 |
| Reviews | 16 |
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
| Second fire sim | Critical | Extend live owner only |
| Empty-zone ignition persists | High | Validation gate in Phase 1 |
| Brigade heroics | High | Entry rules and costs |
| Injury spectacle | High | Medical owner routing |
| Roster duplication | Medium | Rides `DutyRoster` |
| Alarm duplication | Medium | `AlarmSystem` calls only |
| Busywork inspections | Medium | Findings and deadlines |
| Determinism break | Low | Live seeded Tick only |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `fire_zones.json` | 24 | 4,500 |
| `fire_risks.json` | 24 | 4,500 |
| `fire_inspections.json` | 40 | 6,000 |
| `fire_permits.json` | 12 | 2,500 |
| `brigade_roles.json` | 10 | 2,500 |
| `fire_equipment_catalog.json` | 16 | 2,500 |
| `fire_drills.json` | 12 | 3,000 |
| `fire_causes.json` | 20 | 4,000 |
| `evacuation_routes.json` | 8 | 2,000 |
| `incident_reviews.json` | 16 | 4,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~66,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R47-1 | Second fire sim | Low | Critical | Live owner |
| R47-2 | Empty zones | High | High | Phase 1 gate |
| R47-3 | Hero framing | Med | High | Entry rules |
| R47-4 | Injury spectacle | Med | High | Ward routing |
| R47-5 | Roster duplication | Med | Medium | DutyRoster |
| R47-6 | Alarm duplication | Med | Medium | AlarmSystem |
| R47-7 | Inspection busywork | Med | Medium | Deadlines |
| R47-8 | Determinism | Low | High | Seeded Tick |
| R47-9 | Content overrun | Med | Medium | Budget §13 |
| R47-10 | Tone drift to thrills | Med | High | Pillars §3 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Should fire zones be authored data or derived from rooms?** Recommended:
   authored data with room-tag mapping, so the live ignition path can resolve a
   real graph without code changes.
2. **Can untrained residents enter a fire?** Recommended: no; entry requires a
   pair, a hood, and a role, and refusal is an honored decision.
3. **How much should incidents cost?** Recommended: structural damage and smoke
   residue with real repair and cleaning labor, but no permanent loss of a
   whole shelter section.
4. **Are reviews public?** Recommended: causes public, personal details
   private.
5. **Do drills ever fail people?** Recommended: drills produce lessons and
   retraining only; readiness is unit-level, never a personal score.

---

## 17. APPENDIX D — FIRE ZONE TABLE (24 ZONES)

| # | Zone | Kind | Damper | Exit | Base load | Adjacent |
|---|---|---|---|---|---|---|
| 1 | Bunker corridor | circulation | yes | yes | 12 | kitchen, stair |
| 2 | Kitchen gallery | cooking | yes | yes | 30 | corridor, store |
| 3 | Dry store | storage | yes | no | 45 | kitchen, aisle |
| 4 | Storage aisle | storage | yes | yes | 60 | dry store, works |
| 5 | Generator hall | machinery | yes | yes | 25 | works, corridor |
| 6 | Works row | workshop | yes | yes | 35 | hall, aisle |
| 7 | Laundry | service | yes | yes | 40 | corridor, tank |
| 8 | Dormitory A | quarters | yes | yes | 20 | corridor, stair |
| 9 | Dormitory B | quarters | yes | no | 22 | corridor |
| 10 | Mess hall | common | yes | yes | 18 | kitchen, corridor |
| 11 | Clinic | medical | yes | yes | 15 | corridor |
| 12 | Library | knowledge | yes | no | 38 | corridor |
| 13 | North stair | circulation | no | yes | 8 | dorms, gate |
| 14 | South stair | circulation | no | yes | 8 | mess, gate |
| 15 | Gatehouse | entry | yes | yes | 10 | stair, yard |
| 16 | Power subgrid | machinery | yes | no | 28 | hall |
| 17 | Water plant | service | yes | yes | 12 | laundry, tank |
| 18 | Fire cistern room | water | no | yes | 5 | tank |
| 19 | Fuel bay | storage | yes | no | 55 | works |
| 20 | Paint store | storage | yes | no | 48 | works |
| 21 | Archive vault | knowledge | yes | no | 20 | library |
| 22 | Ventilation plant | machinery | yes | no | 18 | corridor |
| 23 | Drill yard | training | no | yes | 6 | gate |
| 24 | Muster yard | assembly | no | yes | 4 | gate |

Twenty-four zones, each with a damper state, an exit flag, a base fire load, and
authored adjacency. The rail of dampers is what makes smoke a manageable
problem instead of a corridor-wide event, and the two long stair rows exist to
make the blocked-exit drill meaningful rather than decorative.

---

## 18. APPENDIX E — FIRE RISK TABLE (24 RISKS)

| # | Risk | Zone kind | Factor | Mitigation | Cadence |
|---|---|---|---|---|---|
| 1 | Lint in flue | laundry | high | sweep, screen | weekly |
| 2 | Grease build | kitchen | high | clean, gasket | daily |
| 3 | Oily rags | works | high | sealed bin | daily |
| 4 | Stacked fuel | store | high | aisle, limits | weekly |
| 5 | Bad wiring | machinery | high | heat check | monthly |
| 6 | Loose joint | machinery | med | torque, inspect | monthly |
| 7 | Lamp heat | quarters | med | shade, distance | weekly |
| 8 | Candle use | quarters | med | stand, away | daily |
| 9 | Hot work | works | high | permit | event |
| 10 | Welding sparks | works | high | screens | event |
| 11 | Grinder dust | works | med | wet, clear | event |
| 12 | Fuel vapor | fuel bay | high | vent, seal | daily |
| 13 | Paint solvent | paint store | high | lids, cool | weekly |
| 14 | Dust layer | aisle | med | sweep | weekly |
| 15 | Paper stacks | library | high | spacing | monthly |
| 16 | Duvet near lamp | dorms | med | spacing | weekly |
| 17 | Overloaded socket | dorm | high | limits | monthly |
| 18 | Friction drive | machinery | med | lube, align | monthly |
| 19 | Battery heat | subgrid | high | vent, check | weekly |
| 20 | Chimney creosote | flue | high | sweep, burn hot | weekly |
| 21 | Ash bin hot | kitchen | med | metal, away | daily |
| 22 | Chemical mix | paint | high | segregation | weekly |
| 23 | Cassette heater | archive | med | space, check | weekly |
| 24 | Dry yard grass | drill yard | med | cut, water | seasonal |

Twenty-four risks, each with a mitigation and a cadence. The table is the
inspection system's whole justification: every risk is a small chore, none of
them is dramatic, and skipping four of them in the same week is how an incident
chain begins.

---

## 19. APPENDIX F — INSPECTION FINDINGS TABLE (40 FINDINGS)

| # | Area | Finding | Severity | Deadline | Fix |
|---|---|---|---|---|---|
| 1 | Laundry | flue lint over fill line | 3 | 2 days | sweep |
| 2 | Laundry | screen missing screw | 2 | 3 days | fit screw |
| 3 | Kitchen | grease tray full | 2 | 1 day | empty |
| 4 | Kitchen | hood filter torn | 2 | 3 days | replace |
| 5 | Works | rag bin lid warped | 3 | 2 days | refit |
| 6 | Works | no screen on bench | 3 | 1 day | fit |
| 7 | Store | aisle under 60 cm | 3 | 2 days | restack |
| 8 | Store | paint above paper | 3 | 1 day | move |
| 9 | Aisle | dust layer thick | 2 | 3 days | sweep |
| 10 | Generator | run heat above mark | 3 | 2 days | clean, torque |
| 11 | Generator | cable sheath worn | 3 | 1 day | wrap, log |
| 12 | Power bay | vent blocked | 3 | 1 day | clear |
| 13 | Fuel bay | lid gasket cracked | 3 | 2 days | replace |
| 14 | Paint | solvent lid loose | 2 | 1 day | close |
| 15 | Dorm A | lamp on cloth | 3 | immediate | move |
| 16 | Dorm A | socket overloaded | 3 | 1 day | split |
| 17 | Dorm B | candle stand absent | 2 | 2 days | make stand |
| 18 | Mess | urn cable frayed | 3 | 1 day | replace |
| 19 | Clinic | oxygen trolley wheel | 1 | 5 days | oil |
| 20 | Library | paper against heater | 3 | 1 day | move |
| 21 | Library | shelf spacing too tight | 2 | 4 days | space |
| 22 | Archive | humidity card high | 2 | 4 days | vent, absorb |
| 23 | Corridor | exit sign missing | 3 | 2 days | hang |
| 24 | Corridor | damper handle stiff | 2 | 3 days | free, lube |
| 25 | North stair | crate on landing | 3 | immediate | clear |
| 26 | South stair | door latch bent | 2 | 2 days | fix |
| 27 | Gatehouse | ash bin against wall | 2 | 1 day | move |
| 28 | Water plant | pump room rags | 2 | 2 days | bin |
| 29 | Vent plant | belt dust thick | 2 | 3 days | clean |
| 30 | Vent plant | filter date past | 2 | 3 days | change |
| 31 | Drill yard | dry grass by wall | 1 | 6 days | cut |
| 32 | Fuel bay | earth clamp corroded | 2 | 4 days | clean |
| 33 | Works | extinguisher missing | 3 | immediate | replace |
| 34 | Kitchen | extinguisher empty | 3 | immediate | refill |
| 35 | Store | sand bucket wet | 2 | 2 days | dry, fill |
| 36 | Laundry | extinguisher buried | 2 | 1 day | clear |
| 37 | Corridor | two found on one hook | 2 | 2 days | split |
| 38 | Archive | no smoke hood | 2 | 4 days | place |
| 39 | Clinic | blanket used as teatowel | 1 | 3 days | replace |
| 40 | Everywhere | test tags undated | 2 | 5 days | date all |

Forty findings, each small enough to fix and each capable of starting a chain
if it is not. The distribution is deliberate: most findings are severity two,
because a fire service is mostly a list of ordinary chores, and the few
immediate findings are the ones where someone acted like the building was a
building instead of a shelter.

---

## 20. APPENDIX G — HOT WORK PERMIT TABLE

| # | Permit | Zone | Preconditions | Duration | Notes |
|---|---|---|---|---|---|
| 1 | Weld bracket | works | screen, extinguisher, clear | 2 h | paired |
| 2 | Cut pipe | water plant | drain, screen, isolate | 3 h | wet floor |
| 3 | Grind frame | works | curtain, mask | 1 h | spark line |
| 4 | Solder wire | subgrid | isolate, mat | 1 h | no fuel |
| 5 | Braze tool | works | screen, cool | 2 h | storage clear |
| 6 | Heat shrink | laundry | mask, vent | 1 h | away from lint |
| 7 | Cut plate | aisle | clear floor, screen | 3 h | fire watch |
| 8 | Weld rail | gate | isolate, screen | 4 h | outdoor |
| 9 | Thaw pipe | water plant | gentle heat only | 3 h | no flame |
| 10 | Repair stove | kitchen | isolate gas | 3 h | hood off |
| 11 | Cut glass | works | felt, screen | 1 h | shared |
| 12 | Finish weld | gate | recheck, sweep | 2 h | watch hour |

Twelve permits, each with a screen, an extinguisher, and a clearance. The last
row exists because the hour after the flame goes out is when a fire service
earns its name, and the permit is not closed until the watch hour is finished.

---

## 21. APPENDIX H — BRIGADE ROLE TABLE

| # | Role | Duties | Entry | Fitness | Call order |
|---|---|---|---|---|---|
| 1 | Chief | decide, account | no | yes | all |
| 2 | Second | take over | yes | yes | 2 |
| 3 | Pump | water and pressure | no | yes | 1 |
| 4 | Entry pair A | enter, search | yes | yes | 2 |
| 5 | Entry pair B | enter, search | yes | yes | 4 |
| 6 | Damper warden | close and hold | no | yes | 1 |
| 7 | Stair warden | routes, roll call | no | yes | 1 |
| 8 | First aid | carry, treat | no | yes | 3 |
| 9 | Runner | messages, tools | no | no | 2 |
| 10 | Store | charges, kit | no | no | event |

Ten roles with three rules encoded in the columns: entry requires training and
a pair, the pump and dampers are called before anyone enters, and the chief
never enters because the chief is the person who has to account for everyone
afterwards.

---

## 22. APPENDIX I — FIRE EQUIPMENT TABLE

| # | Equipment | Charges | Test | Placement | Notes |
|---|---|---|---|---|---|
| 1 | Dry extinguisher | 1 | quarterly | corridors | 4 empty |
| 2 | Foam extinguisher | 1 | quarterly | fuel bay | 1 only |
| 3 | Fire blanket | reuse | yearly | kitchen | mounted |
| 4 | Sand bucket | refill | monthly | stores | dry |
| 5 | Fire hose | n/a | yearly | cistern | two lengths |
| 6 | Smoke hood | 1 | yearly | each floor | 6 total |
| 7 | Damper key | n/a | weekly | each damper | tethered |
| 8 | Screen stand | n/a | yearly | works | two |
| 9 | Alarm bell | n/a | monthly | station | manual |
| 10 | Evac sign | n/a | yearly | all routes | reflective |
| 11 | Line rope | n/a | yearly | entries | waist |
| 12 | Hand lamp | battery | monthly | each floor | safe type |
| 13 | Entry coat | n/a | yearly | hook wall | two |
| 14 | Gloves | n/a | yearly | hook wall | pairs |
| 15 | Refill kit | n/a | yearly | store | for dry units |
| 16 | Drill dummy | n/a | yearly | yard | weighted |

Sixteen equipment entries, and the readiness column is the real content: four
empty extinguishers, one obsolete foam unit, six hoods for ninety-four people,
and two entry coats. The shelter is not poorly served by its gear; it is poorly
served by not knowing which gear works, which is exactly what a count fixes.

---

## 23. APPENDIX J — DRILL TABLE (12 DRILLS)

| # | Drill | Scenario | Blocked | Target | Lesson |
|---|---|---|---|---|---|
| 1 | First walkthrough | quiet evacuation | none | 6 min | familiarity |
| 2 | Roll call | muster count | none | 4 min | accounting |
| 3 | North stair | one exit closed | north | 8 min | alternatives |
| 4 | Smoke crawl | low visibility | one | 7 min | posture |
| 5 | Damper run | close all dampers | none | 90 s | isolation |
| 6 | Kitchen fire | pan on stove | none | 3 min | lid, not water |
| 7 | Flue fire | chimney | none | 5 min | access, check |
| 8 | Entry pair | dummy drag | none | 4 min | pairs |
| 9 | Night shift | skeleton crew | one | 9 min | staffing |
| 10 | Children | youngest first | none | 6 min | leading |
| 11 | Aids | one on carry board | side | 8 min | planning |
| 12 | Combined | fire plus flood alarm | two | 12 min | stress |

Twelve drills, from the first walkthrough to the combined night drill that no
shelter ever looks good doing. The blocked-exit column is where the expansion
keeps its drama: a drill that goes well in an empty building is a walk, and a
drill with a closed stair is a rehearsal for the only night that matters.

---

## 24. APPENDIX K — CAUSE TABLE (20 CAUSES)

| # | Cause | Chain | Modifiers | Review question |
|---|---|---|---|---|
| 1 | Lint ignition | lint, spark, no screen | +flue, +laundry | screen fitted? |
| 2 | Grease fire | fat, heat, splash | +kitchen | hood cleaned? |
| 3 | Rag pile | oil, air, heat | +works | bin sealed? |
| 4 | Electrical fault | wear, load, heat | +aged run | test date? |
| 5 | Loose joint | vibration, heat, arc | +machinery | torqued? |
| 6 | Lamp cloth | heat, drape, time | +dorm | spacing? |
| 7 | Candle tip | flame, draft, cloth | +quarters | stand? |
| 8 | Welding spark | spark, dust, distance | +permit | screen? |
| 9 | Grinder dust | spark, dust cloud | +works | wetted? |
| 10 | Fuel vapor | vapor, spark, space | +fuel bay | vent? |
| 11 | Solvent vapor | fumes, pilot flame | +paint | lids? |
| 12 | Dust layer | dust, heat, air | +aisle | sweep? |
| 13 | Paper chain | stack, heat, draught | +library | spacing? |
| 14 | Battery heat | cell, charge, vent | +subgrid | vent? |
| 15 | Friction belt | belt, pulley, dust | +plant | lube? |
| 16 | Chimney creosote | soot, heat, deposit | +flue | sweep? |
| 17 | Hot ash | ash, bin, liner | +kitchen | metal? |
| 18 | Chemical mix | oxidizer, fuel, jar | +paint | segregation? |
| 19 | Heater proximity | heater, paper, hours | +archive | space? |
| 20 | Yard grass | dry grass, spark, wind | +summer | cut? |

Twenty causes with twenty chains of conditions, and the review column is always
a question rather than an accusation. The whole table is designed so that a
review can pin conditions on a wall, find the one broken link, and fix a
practice instead of a person.

---

## 25. APPENDIX L — EVACUATION ROUTE TABLE

| # | Route | Covers | Muster | Alternative | Aids |
|---|---|---|---|---|---|
| 1 | North primary | dorms, works | yard | south stair | one carry |
| 2 | South primary | mess, kitchen | yard | north stair | one carry |
| 3 | Central east | library, clinic | east wall | gallery | two carry |
| 4 | Central west | archive, plant | west wall | gate | board |
| 5 | Service | laundry, water | drill yard | gate | board |
| 6 | Gate route | gatehouse, stair | muster yard | drill yard | none |
| 7 | Deep route | vault, subgrid | gate | central | board |
| 8 | Night route | all occupied | yard | any stair | marked |

Eight routes with alternatives and aids, because an evacuation plan that has
never been walked with a carry board is a drawing, not a plan. The night route
is the one the drill system tests last and grades hardest.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_brigade_count` | 3 | Extinguishers counted |
| `quest_brigade_walk` | 3 | Night round walked |
| `quest_brigade_layout` | 4 | Zones drawn |
| `quest_brigade_load` | 4 | Loads measured |
| `quest_brigade_first_drill` | 3 | Evacuation timed |
| `quest_brigade_permit` | 3 | Permit desk opened |
| `quest_brigade_flue` | 5 | Fire survived |
| `quest_brigade_review` | 4 | Cause and changes |
| `quest_brigade_cistern` | 3 | Water plan proven |
| `quest_brigade_form` | 4 | Brigade recognized |
| `quest_brigade_smoke_test` | 4 | CO tracked |
| `quest_brigade_second_drill` | 3 | Time improved |
| `quest_brigade_record` | 3 | Register filed |
| `quest_brigade_successor` | 4 | Apprentice alone |
| `quest_brigade_what_we_keep` | 3 | Service decided |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_brigade_extinguishers` | 3 | Count and test |
| `quest_brigade_blankets` | 3 | Blankets placed |
| `quest_brigade_sand` | 3 | Buckets filled |
| `quest_brigade_hose` | 4 | Hose laid, dried |
| `quest_brigade_hoods` | 3 | Hoods placed |
| `quest_brigade_kitchen` | 3 | Grease survey |
| `quest_brigade_wiring` | 4 | Runs heat-checked |
| `quest_brigade_rags` | 3 | Rag discipline |
| `quest_brigade_stack` | 3 | Stack lowered |
| `quest_brigade_lamps` | 3 | Lamps placed |
| `quest_brigade_roles` | 3 | Roles assigned |
| `quest_brigade_fitness` | 3 | Stamina checks |
| `quest_brigade_damper_drill` | 3 | Damper run timed |
| `quest_brigade_casualty` | 4 | Drag practice |
| `quest_brigade_pairing` | 3 | Pair rule kept |
| `quest_brigade_permit_desk` | 3 | Permit log opened |
| `quest_brigade_screens` | 3 | Screens fitted |
| `quest_brigade_isolate` | 3 | Isolation checks |
| `quest_brigade_rewire` | 4 | Bad run replaced |
| `quest_brigade_flue_clean` | 3 | Flue rota set |
| `quest_brigade_cause` | 4 | Origin found |
| `quest_brigade_smoke_damage` | 4 | Residue cleaned |
| `quest_brigade_water` | 3 | Pump and dry |
| `quest_brigade_memorial` | 3 | Loss recorded |
| `quest_brigade_change` | 3 | Practice changed |
| `quest_brigade_routes` | 4 | Routes marked |
| `quest_brigade_roll_call` | 3 | Muster counted |
| `quest_brigade_blocked` | 4 | Exits blocked |
| `quest_brigade_children` | 3 | Children drilled |
| `quest_brigade_aids` | 3 | Carry board used |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Orlen Drave** — chief. Has slept badly for two years about a storage aisle and
would rather be proved wrong about a risk than right about a fire. Believes a
standard is a promise made in daylight.

**Oona Brill** — inspector. Arrives with dated habits from a trade stop and
counts everything twice, not from distrust but because she has seen what a
missing count costs.

**Ivar Foss** — equipment. Knows where all eleven extinguishers are, which four
are empty, and exactly what mood that puts him in. Believes gear should be
boring.

**Nedda Nunn** — rescue lead. Trains entries with a weighted dummy and refuses
to let anyone go in without a pair, including the chief. Believes a rescue that
kills the rescuer is a failure with witnesses.

**Pold Greave** — investigator. Reads char patterns like sentences and writes
reviews that name conditions. Believes blame is what people use when they
cannot find a cause.

**Pimmett Alard** — evacuation warden. Walks the stairs with a crate on his
shoulder to prove the landing is not a storage room. Believes the stair is the
most important room in the shelter.

**Rill Ondine** — apprentice. Sixteen, supervised, and proud of the night walk
card. Believes the smell of hot dust is a message.

**Effie Onsey** — store keeper. Dates every test tag and keeps a board of what
is out. Believes a store is a promise about the next hour.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Burn Yard** — a low wall, a dummy, and a water bucket.
- **The Generator Hall** — heat marks and a mirror-polished floor.
- **The Kitchen Gallery** — hood, gasket, and the smell of old grease.
- **The Storage Aisle** — restacked once, measured twice.
- **The North Stair** — one crate from being impassable.
- **The Fire Cistern** — cold water and two lengths of dry hose.
- **The Workshops Row** — screens, sparks, and a permit board.
- **The Laundry Flue** — swept on a rota now.
- **The Gatehouse** — ash bin moved out from the wall.
- **The Fire Memorial** — names and causes, no blame. 

---

## 30. APPENDIX Q — INCIDENT REVIEW PROTOCOL

| Step | Action | Owner |
|---|---|---|
| Secure | make the zone safe | brigade |
| Account | count everyone | chief |
| Preserve | mark origin, do not clean yet | investigator |
| Map | draw the chain of conditions | investigator |
| Ask | questions, not accusations | all |
| Find | the broken link | investigator |
| Change | one practice, or write why not | chief |
| Train | teach the change | rescue lead |
| File | record in the register | clerk |
| Revisit | re-inspect in thirty days | inspector |

The review protocol is the promise that a fire does not have to happen twice to
teach the shelter everything it knows. The sixth step is the one that keeps the
review honest: if nothing changes, the review must say so, in writing, and the
next fire will read that line back.

---

## 32. APPENDIX R — WORKED FIRE YEAR

**Month one.** Oona counts eleven extinguishers, four empty. Ivar tests them
all by weight and tag and writes the number on a board nobody wants to read.
The first night round finds three heat smells: a bad joint on a pump run, a
lamp near a duvet, and lint behind the laundry flue.

**Month two.** The zones are drawn on the wall: twenty-four rooms, dampers,
exits, adjacency. The storage aisle is restacked to sixty centimeters and loses
two crates to the workshop rather than to the fire. Rill initials her first
walk card.

**Month three.** The first drill evacuates ninety-four people in six minutes
with every stair open, then in eight minutes with the north stair blocked by a
crate that should not have been there. Pimmett takes the crate to the chief's
office and leaves it there.

**Month four.** A hot-work permit desk opens. The first week produces two
refusals and one argument, and the argument ends when Ivar points out that the
bench under the work has sawdust on it.

**Month five.** The laundry flue catches at 02:40. The night crew raises the
alarm, the dampers close in eighty-one seconds, the smoke stays in two zones,
and the entry pair knocks the fire down with a dry unit and a wet blanket. The
last flame is out before the second unit is empty.

**Month six.** Pold's review pins a chain on the wall: lint above the line, a
missing screw, a damper nobody was assigned to close. Three changes are written
and taught, and the first is a name on the damper list, because "someone" was
the broken link.

**Month seven.** The cistern plan becomes real: two lengths of hose, a dry
route, and a drill that proves water arrives in under four minutes.

**Month eight.** The brigade is recognized with ten roles and one refusal rule,
which the chief signs first to prove it applies to him as well.

**Month nine.** A smoke test moves carbon monoxide through two zones on
purpose. The readings climb exactly where the model says they will, and the
shelter finally believes the window strips that say where smoke goes.

**Month ten.** The second blocked-stair drill finishes in six minutes forty
seconds, thirty seconds better than the first, and the difference is one door
that now opens outward.

**Month eleven.** Every finding, fix, drill, and review is entered in the
register; the wall of inspection cards is a year long and dated.

**Month twelve.** Rill walks the north round alone and finds a bad joint on the
heater run in the archive. The fix takes an hour. The chief writes one line in
the register: found before it was hot.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Ivar weighs the fourth extinguisher and it is light, and he stands there
> holding a thing that looks like help and weighs like nothing, and he puts it
> on the table with the others and says: this is what we had.

> The alarm goes at twenty to three, and nobody panics because the drill said
> where to walk, and the walking is the whole point, and Pimmett counts heads
> in the yard by lantern light while the smoke finds the dampers and gives up.

> Pold pins the chain of conditions on the review wall with string, six cards,
> and the last card is not a person and that is the hardest lesson of the
> evening, because a name would be easier to punish and a condition is harder
> to forgive.

> Rill's walk card has one line on it, written carefully: smell near archive
> heater, not smoke, checked twice. Orlen reads it twice and files it and thinks
> that the shelter just bought another year.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No count | empty units at worst time | count and test |
| No layout | fire with no map | author zones |
| Late inspection | hidden chain | tighten cadence |
| No drill | slow, lost evacuations | schedule drills |
| No permit | spark near dust | gate hot work |
| Blind entry | rescuer becomes casualty | pair rule enforced |
| Dirty suppression | damaged gear after | clean and re-charge |
| Blamed review | silence and hiding | blameless restatement |
| No record | lesson lost | register re-opened |
| Untested hood | unusable at the door | test days posted |

No failure in the table is catastrophic on its own, which is the point: fire
kills through chains of small omissions, and the recovery column is a list of
small habits that break chains before they finish.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No second fire, smoke, alarm, or ventilation simulation.
- [ ] The live `Ignite` path receives authored zones from data.
- [ ] Injury routes to medical owners; death to memorial owners.
- [ ] Brigade duty rides `DutyRoster`; no second roster.
- [ ] Drill noise registers through `ShelterNoiseSystem`.
- [ ] Reviews name conditions, never people.
- [ ] Children never fight fires; apprentices are 16+ and supervised.
- [ ] No arson, sabotage, or insurance content.
- [ ] Save additions are additive inside `shelter_fire`.
- [ ] Determinism uses the live seeded tick only.

---

## 36. APPENDIX V — GLOSSARY

- **Zone** — an authored room block with adjacency and a damper.
- **Fire load** — how much a zone can feed a fire.
- **Finding** — an inspection result with a severity and deadline.
- **Permit** — written permission for hot work with preconditions.
- **Entry pair** — two trained people who enter together or not at all.
- **Watch hour** — the hour after hot work when the work is not yet finished.
- **Muster** — the counting point after evacuation.
- **Chain** — the conditions that joined to make a fire possible.
- **Change** — the one practice a review must alter or explicitly decline to.
- **Readiness** — the unit-level state of gear, people, and drills.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ShelterFireHazardSystem` | time, zones | incidents | layout data |
| `FireZoneLayoutSystem` | rooms | layout selection | incidents |
| `FireLoadSystem` | inventory | loads | inventory |
| `InspectionSystem` | risks, gear | findings | gear |
| `PermitSystem` | grid state | permits | grid |
| `BrigadeSystem` | roster, needs | assignments | needs |
| `FireEquipmentSystem` | inventory | charges, tests | incidents |
| `FireDrillSystem` | layout | drill results | incidents |
| `IncidentReviewSystem` | incidents | reviews | incidents |
| `VentilationSystem` | nothing | smoke, CO | nothing |
| `NeedsSystem` | nothing | nothing | nothing |
| `AlarmSystem` | alerts | state | nothing |
| `ShelterNoiseSystem` | noise | nothing | nothing |
| `MedicalWardSystem` | casualties | care | nothing |
| `DutyRoster` | shifts | nothing | nothing |
| `PowerGridSystem` | isolation | nothing | nothing |
| `StandingRecord` | records | records | nothing |
| `MemorialSystem` | losses | memory | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`fire_zones.json`** — `zone_id`, `display_name`, `room_tags[]`,
`adjacent_zone_ids[]`, `damper_id`, `has_exit`, `fire_load_base`, `tags[]`.

**`fire_risks.json`** — `risk_id`, `zone_kind`, `factor`, `fuel_class`,
`mitigation`, `cadence`, `tags[]`.

**`fire_inspections.json`** — `finding_id`, `area`, `finding`, `severity`,
`deadline_days`, `fix_action`, `recheck_state`, `tags[]`.

**`fire_permits.json`** — `permit_id`, `kind`, `zone_id`, `preconditions[]`,
`duration_hours`, `screen`, `extinguisher`, `isolation`, `tags[]`.

**`brigade_roles.json`** — `role_id`, `display_name`, `duties[]`, `entry`,
`fitness`, `call_order`, `tags[]`.

**`fire_equipment_catalog.json`** — `item_id`, `kind`, `charges`, `test_days`,
`placement_zones[]`, `note`, `tags[]`.

**`fire_drills.json`** — `drill_id`, `scenario`, `blocked_exits[]`,
`target_minutes`, `roll_call`, `lesson`, `tags[]`.

**`fire_causes.json`** — `cause_id`, `kind`, `conditions[]`, `modifiers[]`,
`review_question`, `tags[]`.

**`evacuation_routes.json`** — `route_id`, `zone_ids[]`, `muster_point`,
`alternatives[]`, `aids[]`, `tags[]`.

**`incident_reviews.json`** — `review_id`, `incident_id`, `cause_id`,
`conditions[]`, `lesson`, `practice_change`, `record_link`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Rounds completed | inspection discipline | Inspections |
| Findings open | backlog risk | Inspections |
| Findings overdue | risk | Inspections |
| Extinguishers charged | readiness | Equipment |
| Tests current | readiness | Equipment |
| Drills completed | rehearsal | Drills |
| Evacuation time | capability | Drills |
| Damper time | isolation | Drills |
| Incidents per year | prevention outcome | Incidents |
| Reviews with change | learning | Reviews |

Telemetry is diagnostic only; it never gates content and never ranks a
brigade member.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 1 makes empty-zone ignition a validation error.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `shelter_fire`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows prevention reducing incidents and drills reducing time.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No arson, spectacle, or hero content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Does a shelter-wide fire ever destroy a whole section, or only damage it?
2. Who decides that a room is too dangerous to enter, and can they overrule
   the chief?
3. Should inspection failures ever have soft consequences beyond risk?
4. Are permits public record, or private until an incident?
5. Can a resident refuse fire duty without stigma?
6. Do drills disturb sleep enough to touch the quiet-hours owners?
7. Is the fire bell a noise source, an alarm, or both?
8. How much time can prevention demand before it becomes the whole game?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 16 The Rebuilt Body | Entry fitness and prosthetics |
| 2 | 18 The Underneath | Duct and tunnel fire routes |
| 2 | 21 The Grid | Isolation and electrical ignition |
| 3 | 22 The Clean Flow | Grease and waste loads |
| 3 | 23 The Alarm | Fire alarm calls |
| 3 | 24 The Long Goodbye | Loss records |
| 4 | 27 The Thread | Blankets, hoods, and cloth fire load |
| 4 | 28 The Lesson | Drills for children |
| 4 | 31 The Kiln | Screens, stands, and firebrick |
| 5 | 34 The Long Road | Dry-season yard risk |
| 5 | 36 The Watch | Night rounds and alarm relay |
| 6 | 38 The Ward | Burn and smoke care |
| 6 | 40 The Wheel | Machine friction and belts |
| 6 | 41 The Quiet | Drill noise and quiet hours |
| 7 | 42 The Core | Reactor vault fire boundaries |
| 7 | 44 The Outpost | Remote fire kits |
| 7 | 46 The Long Change | Dry year grass risk |

Each hook is additive. The Brigade can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Service.** The brigade has standards, gear, and drills, and the shelter
knows the answer is coming before the smell arrives.

**The Watchful House.** Prevention becomes habit and the loudest thing in the
shelter is a kettle, and nobody can remember the last alarm.

**The Cost of Entry.** A rescue costs the shelter someone it loved, and the
review turns that loss into a rule that saves the next person who runs in.

**The Rebuilt Haul.** The aisle and flue are rebuilt to standard and the fire
load becomes a number the shelter keeps under a line it drew itself.

**The Quiet House.** Fires stop happening, and the brigade spends its evenings
teaching children which doors open outward.

**Fade.** A wall of inspection cards, each dated and initialed, and one empty
hook with a tag that says: refilling, back by Tuesday.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Second fire sim | authority break | extend live owner |
| Empty zone lists | unwinnable fires | authored layout |
| Hero brigade | fantasy | entry rules and costs |
| Panic theatre | spectacle | drills and calm |
| Blame reviews | hiding | conditions and questions |
| Gadget worship | object fetish | counts and tests |
| Endless chores | tedium | deadlines and meaning |
| Fire as enemy | tone break | fire as consequence |
| Permanent damage | bleak | repair and rebuild |
| Quiet alarm | accessibility break | text plus sound |

The list exists because fire is the easiest thing in a survival game to turn
into a show. The expansion's rule is that the show is a checklist somebody
finished before anything was ever on fire.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Fire zones | 24 | 4,500 |
| Fire risks | 24 | 4,500 |
| Findings | 40 | 6,000 |
| Permits | 12 | 2,500 |
| Brigade roles | 10 | 2,500 |
| Equipment | 16 | 2,500 |
| Drills | 12 | 3,000 |
| Causes | 20 | 4,000 |
| Routes | 8 | 2,000 |
| Reviews | 16 | 4,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~66,500** |

---

## 46. APPENDIX AF — FIRST YEAR OF THE BRIGADE

| Month | Focus | Milestone |
|---|---|---|
| 1 | Count | gear known |
| 2 | Layout | zones drawn |
| 3 | First drill | evacuation timed |
| 4 | Permits | hot work gated |
| 5 | Flue | first answered fire |
| 6 | Review | changes written |
| 7 | Water | cistern proven |
| 8 | Form | service recognized |
| 9 | Smoke | CO mapped |
| 10 | Drill two | time improved |
| 11 | Record | register filed |
| 12 | Successor | apprentice finds one |

A year is the honest arc for a fire service: twelve months from a count to a
capability, with one real incident in the middle that tests whether the paper
was true.

---

## 47. APPENDIX AG — FIRE SERVICE COVENANT

| Clause | Promise |
|---|---|
| Count | Gear is known by number and test date |
| Draw | Every zone has a map and a damper |
| Inspect | Findings have owners and deadlines |
| Permit | Hot work is gated in writing |
| Pair | Nobody enters alone, including the chief |
| Drill | Rehearsal happens before need |
| Answer | The brigade arrives and isolates first |
| Review | Causes are conditions, never people |
| Change | Every review alters practice or says why not |
| Record | Everything is dated and kept |

The covenant is the expansion's first-class design object. Fire is the one
disaster a shelter can almost entirely prevent, and the covenant is the list of
small promises that turn almost entirely into entirely.

---

## 48. APPENDIX AH — BRIGADE SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Chief | Orlen | Nedda | standards walk |
| Inspector | Oona | Rill | round together |
| Equipment | Ivar | Effie | count together |
| Entry A | Nedda | pair lead | drill together |
| Entry B | second pair | rotating | drill together |
| Damper | assigned | rotating | damper run |
| Stairs | Pimmett | deputy | blocked drill |
| First aid | clinic staff | rotating | carry drill |
| Runner | youngest able | rotating | message drill |
| Store | Effie | Rill | test day |

The succession table is the difference between a capability and a person. Every
role has a named successor before it has a crisis, and every handover happens
by doing the job beside the person who already holds it.

---

## 49. APPENDIX AI — NIGHT ROUND CARD TABLE

| # | Check | Look | Smell | Touch | Note |
|---|---|---|---|---|---|
| 1 | Corridor | exit clear | dust hot | no | card |
| 2 | Kitchen | hood clean | grease | no | card |
| 3 | Laundry | flue line | lint | no | card |
| 4 | Works | rag lid | oil | no | card |
| 5 | Store | aisle width | dust | no | card |
| 6 | Plant | belt dust | rubber | no | card |
| 7 | Subgrid | vent clear | ozone | no | card |
| 8 | Archive | heater space | paper | no | card |
| 9 | Stairs | landing clear | none | door open | card |
| 10 | Yard | grass cut | none | no | card |

Ten checks in a fixed order, walked at the same hour by the same hand each
night. The card is the smallest unit of the whole expansion: a person walking a
building with a lamp, noticing what is different, and writing it down.

---

## 50. CLOSING STATEMENT

ASHFALL already simulates fire with more care than most games simulate combat:
zones, dampers, smoke, carbon monoxide, heat, spreading, brigade suppression,
and structural damage, all on a seeded tick. What it lacks is everything around
the incident: the count of extinguishers, the night walk, the layout, the
permit, the drill, the cause, and the review. The Brigade adds the practice of
prevention around the sim that already exists. It adds a clipboard on a hook
and a wall of dated inspection cards, and it keeps the loudest thing in the
shelter from ever needing to be loud.

> Wave 8 note: this plan is one of five Wave 8 expansion bibles (47–51). Each is
> self-contained; none requires another to ship. The shared Wave 8 index lives
> at `docs/expansions/wave8/WAVE8_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `ShelterFireHazardSystem` (`FireZoneState`,
> `FireIncidentState`, spread/smoke/CO/heat constants, `Ignite`, `RaiseAlarm`,
> `AssignBrigade`, `SetDamper`, `DeployExtinguisher`, `EvacuateZone`, `Tick`,
> `CaptureState`), `ShelterFireHostSession` and `ShelterFireSaveStore` under the
> `shelter_fire` section, `FireIncidentPanel` commands, `VentilationSystem` as
> the smoke and CO sink, `src/Main.Plans162_165.cs` live ignition with an empty
> zone list, and zero extinguisher items in `Assets/StreamingAssets/Data/items.json`.