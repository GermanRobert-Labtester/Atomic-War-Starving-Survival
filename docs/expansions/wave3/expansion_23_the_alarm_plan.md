# ASHFALL — Expansion 23 Design Bible
# THE ALARM
### Wave 3 · Fire, Emergency Response, Rescue, Evacuation, Cascade, and Readiness

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Shelter` (ShelterFireHazard, Cascade), `Ashfall.Core.Disease` (Triage), `Ashfall.Core.Radio` (Rescue Dispatch), `Ashfall.Core.Medical`
**Proposed host owner:** `AlarmHostSession` (extends `ShelterFireHostSession` + cascade and triage surfaces)
**Existing save sections:** `shelter_fire`, `cascade`, `medical_triage`, `defenses`
**Existing CLI verbs:** `--shelter-fire-selftest`, `--cascade-selftest`, `--triage-selftest`, `--rescue-dispatch-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models the worst days with unusual care. `ShelterFireHazardSystem`
owns fire zones with fire, smoke, carbon monoxide, and heat levels; damper state;
evacuation flags; adjacency; an incident lifecycle (`alarmRaised`, `isSuppressed`,
`isResolved`); a work brigade; extinguisher charges; and structural damage.
`CascadeCoordinator` reads typed facts from every other subsystem — power deficit,
pumping unserved, sump rising, heating unserved, filtration unserved, fallout storm,
cold storage unpowered, darkness, outbreak, fire, hatch unsealed — and reports active
cascade rules with authored warning windows, effect tags, and **off-ramps**. It owns
none of those facts and writes none of them back. `DiseaseTriage` derives clinical
stages (Incubating, Ill, Terminal, OutcomePending). `DistressRescueMissionManager`
handles off-site rescue. `EmergencyResponseHud`, `FireIncidentPanel`, and
`SonicRuptureDrillPanel` already exist.

But the content is thin: `cascade_rules.json` is 1.7 KB, `defenses.json` 1.9 KB,
`perimeter_defenses.json` 5.8 KB, `shelter_shielding.json` 580 bytes. The systems
model emergency; the world barely does.

**The Alarm** turns that machinery into the shelter's emergency culture: fire
response, evacuation and accountability, in-shelter rescue, incident command, drills
that actually measure readiness, mutual aid between settlements, and the cascade
that turns one failure into five.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Every shelter has a first bad night. A lamp falls in the stores. A heater line
cracks. A battery vents. The alarm sounds, and everyone discovers at the same moment
that they do not actually know where they are supposed to go.

**The Alarm** is the expansion about preparation and its absence. It adds an
emergency culture: roles before the alarm, drills that measure real readiness,
evacuation routes with accountability, rescue teams that go back in, incident command
that makes decisions under smoke, and mutual-aid pacts that decide whether a neighbor
comes when the siren carries. It also expands the cascade system from a handful of
rules into a real failure web, where losing power loses pumping, rising water loses
the sump, and a hatch left unsealed loses everything at once.

The expansion's hard rule is inherited from the live systems: **a cascade is a
warning with an off-ramp, not a scripted punishment.** The player always has a
chance to intervene, and the game always tells them what that chance is.

### 1.2 The five loops it adds

```
   Hazard ──► Alarm ──► Response ──► Rescue/Evac ──► Recovery ──► Readiness
      │         │          │            │               │            │
      ▼         ▼          ▼            ▼               ▼            ▼
   Fire,     muster,   command,     search,         damage,      drills,
   smoke,    headcount teams,       extraction,     restoration, lessons,
   cascade   accounting roles       refuge          rebuild      training
      │                                                              │
      ▼                                                              │
   Cascade web ──► typed warnings ──► off-ramps ◄────────────────────┘
```

### 1.3 What the player manages

1. **Readiness.** Roles, equipment, routes, and drills, all before the alarm. The
   expansion measures readiness honestly and shows the gaps.
2. **The alarm.** Detection, muster, headcount, and accountability. Knowing who is
   missing is a system, not a feeling.
3. **Incident command.** Who decides, with what authority, under what conditions.
   Command can be delegated, and it can fail.
4. **Fire and smoke.** The live fire zones, dampers, CO, heat, and structural damage;
   suppression is a limited resource with a real cost.
5. **Rescue.** In-shelter search and extraction: teams, risk, time, and the terrible
   arithmetic of going back in.
6. **Evacuation and refuge.** Routes, muster points, refuge chambers, and the
   difference between evacuating a room and abandoning a shelter.
7. **Cascade.** Authored failure webs with warning windows and off-ramps, so the
   player can always act.
8. **Mutual aid.** Regional emergency pacts: who comes, who is obligated, and who
   arrives late.

### 1.4 What it is not

- Not a second fire, cascade, or triage system. All route through the live owners.
- Not a combat system. Rescue is a hazard operation, not a firefight.
- Not a punishment generator. Every cascade has an off-ramp and a warning.
- Not a scripted disaster. Incidents emerge from authored conditions and live state.
- Not a second save authority.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | Fire zones, smoke, CO, heat, dampers, evacuation, incident lifecycle, brigade, damage | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CascadeCoordinator.cs` | Typed cascade facts, active rules, effect tags, off-ramps | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CascadeRuleCatalog.cs` | Authored cascade rules | `LIVE` |
| `Assets/Ashfall.Core/Disease/DiseaseTriage.cs` | Clinical stages and triage urgency | `LIVE` |
| `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs` | Off-site rescue dispatch | `LIVE` |
| `Assets/Ashfall.Core/Radio/RescueDispatchPreflight.cs` | Rescue preflight | `LIVE` |
| `src/Host/ShelterFireHostSession.cs`, `ShelterFireSaveStore.cs` | Host and persistence | `LIVE` |
| `src/UI/EmergencyResponseHud.cs`, `FireIncidentPanel.cs`, `SonicRuptureDrillPanel.cs` | UI | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `cascade_rules.json` | 1.7 KB | a few failure rules with off-ramps |
| `incidents.json` | 12.9 KB | random incident events |
| `defenses.json` | 1.9 KB | shelter defenses |
| `perimeter_defenses.json` | 5.8 KB | perimeter structures |
| `shelter_shielding.json` | 580 B | shielding constants |
| `excavation_hazard_mitigation.json` | 4.9 KB | hazard responses |
| `desperation_events.json` | 1.5 KB | desperation incidents |

### 2.3 Confirmed gaps

- **GAP-23-1 — Cascades are a sample.** The live rule set supports a failure web; only
  a handful of rules are authored.
- **GAP-23-2 — No evacuation content.** Routes, muster points, headcounts, and
  accountability are absent despite the fire system tracking evacuation flags.
- **GAP-23-3 — No in-shelter rescue.** Search, extraction, teams, and risk are absent;
  only off-site rescue dispatch exists.
- **GAP-23-4 — No incident command.** No roles, no authority, no handover, no
  decision log.
- **GAP-23-5 — Drills are a panel, not a system.** `SonicRuptureDrillPanel` exists; no
  drill profiles, readiness scoring, or lesson application.
- **GAP-23-6 — No suppression logistics.** Extinguisher charges exist; no equipment
  catalog, maintenance, or resupply.
- **GAP-23-7 — No refuge chambers.** No authored safe rooms or their limits.
- **GAP-23-8 — No mutual aid.** No regional emergency pacts, obligations, or arrival
  models.
- **GAP-23-9 — No emergency locations or NPCs.**

### 2.4 Non-duplication statement

This expansion will **not** add a second fire, cascade, triage, or rescue-dispatch
system. It extends `ShelterFireHazardSystem`, `CascadeCoordinator`,
`CascadeRuleCatalog`, `DiseaseTriage`, and the live fire host. It routes all medical
consequences through `MedicalPipelineCoordinator`, all morale through
`NeedsSystem`/`MoraleContagionSystem`, and all structural repair through the live
shelter/damage owner. Cascade rules remain authored data with off-ramps; the
coordinator still owns none of the facts it reads.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Preparation is the content.** The expansion's best moments happen before
the fire: a drill that finds a blocked route, a roster that reveals no one is
assigned to the pumps, a headcount that proves a whole wing was never walked.

**Pillar 2 — Warnings are sacred.** Every cascade has a warning window and an
off-ramp. The game never ambushes the player without telegraphing the mechanism.

**Pillar 3 — Rescue is a choice with a price.** Going back in is heroic and costly.
The expansion never makes it free, and never punishes it as foolish.

**Pillar 4 — Command is a person.** Someone owns the decision, and that person can be
wrong, tired, or absent. Command can be delegated and can fail.

**Pillar 5 — Accountability is care.** A headcount is how a shelter proves it did
not leave anyone behind. It is also how it discovers that it did.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| The alarm | A bell, a shout, work stopping | Cinematic score |
| Smoke | Low, moving, and hard to see through | Dramatic fog |
| A rescue | Rope, a name, a time limit | Action heroics |
| A headcount | A list, a pause, a second call | Tension music |
| A drill | Clumsy, slow, and useful | Competence porn |
| A cascade | A warning and a decision | Sudden punishment |

### 3.3 Content limits

- Fire and smoke are depicted practically and without gore.
- Rescue deaths are grave, rare, and consequential; never a spectacle.
- No real-world fire service, disaster agency, or incident-command doctrine is named
  or copied.
- Drills are honest about failure; no survivor is shamed for doing their best.
- Children are evacuated first and are never used as stakes bait.

---

## 4. THE ALARM WORLD

### 4.1 Interior rooms

- **`room_alarm_office`** — command post, logs, and the board.
- **`room_fire_brigade`** — turnout gear, extinguishers, and hose.
- **`room_muster_hall`** — the primary assembly point.
- **`room_refuge_chamber`** — sealed refuge with air and water for a limited stay.
- **`room_drill_yard`** — training ground for drills and rescue practice.
- **`room_smoke_control`** — dampers, fans, and smoke routing.
- **`room_medical_triage`** — casualty collection and sorting.
- **`room_equipment_store`** — gear, charges, ropes, and spares.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_watch_tower` | The Watch Tower | 5 | Early detection and lookout |
| `loc_collapsed_wing` | The Fallen Wing | 6 | Collapse rescue and salvage |
| `loc_muster_field` | The Field | 3 | External assembly point |
| `loc_drill_grounds` | The Grounds | 4 | Regional drill site |
| `loc_mutual_depot` | The Aid Depot | 5 | Shared emergency stores |
| `loc_firebreak_line` | The Firebreak | 5 | Wildfire defense line |
| `loc_ruined_firehouse` | The Firehouse | 6 | Pre-war fire station salvage |
| `loc_water_point` | The Draft Point | 4 | Suppression water source |
| `loc_old_control` | The Control Room | 6 | Pre-war emergency systems |
| `loc_sirens` | The Sirens | 5 | Regional alarm network |

All locations require valid item references and scanner registration.

### 4.3 The alarm graph

The shelter is a graph of zones with adjacency, dampers, routes, and muster points.
The live fire system already tracks adjacency and evacuation flags. The expansion
authors the graph: which zones connect, which routes are safe, which muster points
are reachable, and which rooms can become refuges.

---

## 5. MAIN STORYLINE — "THE FIRST BAD NIGHT"

### 5.1 Central conflict

A lamp falls in the stores at night. The fire is small and entirely survivable — for
a shelter that knows what to do. This one does not. Half the brigade is asleep, the
muster point is on the far side of the smoke, the nearest extinguisher is empty, and
nobody has the corridor that leads to the trapped quartermaster.

The fire is put out. The reckoning is that it did not have to be close. The shelter's
new fire chief, **Halda Voss**, proposes a readiness program: roles, drills, routes,
headcounts, and equipment. The administrator, **Marek Dowd**, supports it until the
first drill reveals how badly the shelter is prepared, and how much of the budget
would be needed to fix it.

Then a cascade begins: a pump fails, the sump rises, the fire brigade is committed to
suppression, the cold storage warms, and the shelter discovers that every failure is
connected to the next. And a neighbor's siren on the ridge asks for aid under a pact
nobody has ever invoked.

The expansion's question: **how much does a shelter spend on the night that may never
come?**

### 5.2 Theme (unspoken)

**Readiness is boring until it is the only thing that matters.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_chief_halda_voss` | Halda Voss | Fire chief | Builds readiness; impatient with excuses |
| `npc_administrator_marek_dowd` | Marek Dowd | Administrator | Pays for readiness; wants proof |
| `npc_brigade_lead_koval` | Koval | Brigade lead | First through the door; haunted by a near miss |
| `npc_smoke_tech_lin` | Lin Orr | Smoke control tech | Dampers, fans, and evacuation routes |
| `npc_rescue_sergeant_cray` | Cray | Rescue sergeant | Search and extraction; decides when to pull back |
| `npc_triage_nurse_asa` | Asa Vell | Triage nurse | Casualty sorting and comfort |
| `npc_neighbor_aid_sable` | Sable | Neighbor envoy | Mutual-aid obligation and refusal |
| `npc_child_muster_pim` | Pim | Child of the muster | Accountability's human meaning |

### 5.4 Story beats (15)

1. **The Night Lamp.** A small fire; a shelter that does not know what to do.
2. **The Count.** The headcount takes forty minutes and misses two people.
3. **The Program.** Halda proposes roles, routes, drills, and equipment.
4. **The Budget.** Marek demands proof before paying for a night that may not come.
5. **The First Drill.** The drill fails publicly; the shelter's confidence drops.
6. **The Route.** The muster point moves; the smoke route is walked.
7. **The Pump.** A pump fails; the sump begins to rise.
8. **The Cascade.** Warnings appear; the shelter chooses off-ramps.
9. **The Brigade.** Suppression is committed; a second hazard is left uncovered.
10. **The Refuge.** The chamber is tested with real people for a real hour.
11. **The Rescue.** Someone is trapped in the flooded wing.
12. **The Siren.** A neighbor invokes the mutual-aid pact.
13. **The Aid.** The shelter sends help, sends less, or refuses.
14. **The Second Fire.** A worse incident tests everything the shelter learned.
15. **The Watch.** Final disposition of readiness, command, and the pact.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Readiness spend | full / partial / minimal | safety vs. resources |
| Command model | chief / rotating / committee | speed vs. legitimacy |
| Rescue depth | push / limited / pull back | life vs. lives |
| Evacuation | full / partial / shelter-in-place | safety vs. exposure |
| Refuge | build / retrofit / none | cost vs. last resort |
| Aid pact | fulfill / partial / refuse | obligation vs. self |
| Cascade off-ramp | power / pumping / sacrifice a load | triage of systems |
| Final | permanent brigade / volunteers / none | legacy |

### 5.6 Endings (5 + fade)

1. **The Ready House** — the program holds; the next fire is handled calmly.
2. **The Close Call** — readiness is partial; the shelter survives on luck and pays for it.
3. **The Empty Muster** — a real fire finds the gaps; the shelter loses people it could have saved.
4. **The Pact Kept** — mutual aid becomes a regional institution.
5. **The Quiet Brigade** — the shelter chooses a small permanent force and tolerates the cost.
6. **Fade** — the alarm goes back into the drawer; the next night is someone else's problem.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_alarm_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_alarm_night_lamp`, `quest_alarm_the_count`, `quest_alarm_the_program`,
`quest_alarm_the_budget`, `quest_alarm_first_drill`, `quest_alarm_the_route`,
`quest_alarm_the_pump`, `quest_alarm_the_cascade`, `quest_alarm_the_brigade`,
`quest_alarm_the_refuge`, `quest_alarm_the_rescue`, `quest_alarm_the_siren`,
`quest_alarm_the_aid`, `quest_alarm_second_fire`, `quest_alarm_the_watch`.

### 6.2 Side quests (30)

**Readiness (5)**
- `quest_alarm_roles` — assign emergency roles
- `quest_alarm_equipment` — inventory and service gear
- `quest_alarm_routes` — walk and mark routes
- `quest_alarm_headcount` — build an accountability method
- `quest_alarm_readiness_report` — publish honest readiness

**Fire and smoke (6)**
- `quest_alarm_extinguisher_line` — charge and stage extinguishers
- `quest_alarm_damper_drill` — practice smoke control
- `quest_alarm_hose_test` — test and repair hose
- `quest_alarm_wildfire_line` — build the firebreak
- `quest_alarm_vent_plan` — plan smoke venting
- `quest_alarm_structural_check` — inspect for fire load

**Rescue (5)**
- `quest_alarm_search_team` — form and train a team
- `quest_alarm_collapse_rescue` — a collapse operation
- `quest_alarm_rope_work` — rig and practice vertical rescue
- `quest_alarm_trapped` — a trapped survivor with a time limit
- `quest_alarm_pull_back` — decide to abandon a rescue

**Evacuation and refuge (5)**
- `quest_alarm_muster_drill` — run a full muster
- `quest_alarm_secondary_route` — establish a second route
- `quest_alarm_refuge_air` — provision refuge air and water
- `quest_alarm_evac_families` — family reunification
- `quest_alarm_abandon_room` — practice abandoning a wing

**Cascade (5)**
- `quest_alarm_cascade_map` — map the failure web
- `quest_alarm_off_ramp` — choose which system to sacrifice
- `quest_alarm_warning_window` — shorten a warning by preparation
- `quest_alarm_pump_priority` — settle pump vs. other loads
- `quest_alarm_dark_watch` — operate through a blackout

**Mutual aid (4)**
- `quest_alarm_pact_terms` — negotiate an aid pact
- `quest_alarm_aid_depot` — stock shared emergency stores
- `quest_alarm_aid_run` — deliver aid under risk
- `quest_alarm_aid_refused` — refuse aid and live with it

### 6.3 Repeatable quests (8)

`quest_alarm_repeat_drill`, `quest_alarm_repeat_inspect`,
`quest_alarm_repeat_gear`, `quest_alarm_repeat_watch`,
`quest_alarm_repeat_muster`, `quest_alarm_repeat_patrol`,
`quest_alarm_repeat_triage`, `quest_alarm_repeat_pact_check`.

### 6.4 Dynamic hooks

`ShelterFireHazardSystem` emits incident events; `CascadeCoordinator` reports active
stages with off-ramps; `DiseaseTriage` emits casualty states. The generator attaches
authored follow-ups without a new event bus.

### 6.5 Constraints

- Every cascade rule keeps a warning window and an off-ramp.
- No fire may be scripted to kill without an evacuation path and warning.
- No rescue may succeed without risk and time cost.
- No medical consequence may bypass `MedicalPipelineCoordinator`.
- No structural repair may bypass the live shelter/damage owner.
- No mutual aid may bypass `FactionStanceEngine`.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `ReadinessSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** roles, training, equipment availability, and a derived readiness score per
hazard type. **Consumes:** `DutyRoster`, `SkillProgressionSystem`, `Inventory`,
`ShelterFireHazardSystem`. **Data:** `emergency_roles.json`, `readiness_profiles.json`.
**Rules:** readiness is a derived measure, never a hidden stat; it degrades with
staff turnover, gear use, and neglected drills; the score is surfaced honestly.

### 7.2 `EvacuationSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** routes, muster points, headcounts, accountability, and evacuation orders.
**Consumes:** `ShelterFireHazardSystem` zones and adjacency, `NeedsSystem`,
`MoraleContagionSystem`. **Data:** `evacuation_routes.json`, `muster_points.json`.
**Rules:** routes have capacity and condition; smoke closes routes; a headcount is
only as good as its method; missing persons are tracked and searchable.

### 7.3 `IncidentCommandSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** command roles, authority, handover, decision logging, and command failure.
**Consumes:** `ReadinessSystem`, `EvacuationSystem`, `ShelterFireHazardSystem`.
**Data:** `emergency_roles.json`.
**Rules:** command is a person; a tired or absent commander degrades response;
handover is explicit; the decision log is a real record the epilogue can read.

### 7.4 `RescueSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** search sectors, rescue teams, extraction, time limits, and pull-back
decisions. **Consumes:** `ShelterFireHazardSystem`, `EvacuationSystem`,
`MedicalPipelineCoordinator`, `NeedsSystem`. **Data:** `rescue_teams.json`.
**Rules:** rescue costs time, air, and risk; a team can be lost; pull-back is a
legitimate and burdensome choice; casualties route to the medical pipeline.

### 7.5 `SuppressionSystem` (extend `ShelterFireHazardSystem`)

**Owns:** extinguisher charges, hose, water draft, foam, and suppression logistics.
**Consumes:** live fire zones, `Inventory`, `PowerGridSystem`, `WaterTreatmentSystem`.
**Data:** `suppression_equipment.json`.
**Rules:** suppression consumes real charges and water; running out is a real state;
resupply takes time.

### 7.6 `DrillSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** drill profiles, execution, measured outcomes, lessons, and retraining.
**Consumes:** `ReadinessSystem`, `EvacuationSystem`, `RescueSystem`. **Data:**
`drill_profiles.json`.
**Rules:** drills measure actual performance using the same systems as real events;
lessons change readiness; a drill can reveal a fatal gap, and that is the point.

### 7.7 `MutualAidSystem` (new, `Ashfall.Core.Factions`)

**Owns:** aid pacts, obligations, arrival models, and refusal. **Consumes:**
`FactionStanceEngine`, `DiplomaticTreaties`, `DistressRescueMissionManager`,
`ExpeditionSystem`. **Data:** `mutual_aid_pacts.json`.
**Rules:** aid is a real expedition with real risk; a pact kept builds standing and
a pact refused costs it; no aid resolves as a cutscene.

### 7.8 Systems explicitly not added

- No second fire, cascade, triage, or rescue-dispatch system.
- No scripting that bypasses the live hazard model.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `cascade_rules.json` (extend)

Existing schema preserved (`id`, `display_name`, `requires_all`, `warning_hours`,
`effect_tags`, `off_ramps`). New rules across power, water, sump, cold storage,
filtration, heating, hatch, outbreak, fire, and darkness combinations.

### 8.2 `evacuation_routes.json` (new)

```json
{
  "schema_version": 1,
  "routes": [
    {
      "route_id": "route_muster_north",
      "display_name": "North Corridor Run",
      "from_zone_id": "zone_stores",
      "to_muster_id": "muster_hall",
      "capacity": 40,
      "width_class": "narrow",
      "condition": "good",
      "hazard_sensitivity": ["smoke", "heat"],
      "alternate_route_id": "route_muster_service",
      "tags": ["primary", "smoke_exposed"]
    }
  ]
}
```

### 8.3 `muster_points.json` (new)

Muster rows: id, capacity, adjacency, refuge capability, and headcount method.

### 8.4 `emergency_roles.json` (new)

Role rows: name, required skill, authority scope, gear, and succession order.

### 8.5 `rescue_teams.json` (new)

Team rows: size, skills, gear, risk tolerance, time limits, and pull-back rules.

### 8.6 `suppression_equipment.json` (new)

Equipment rows: type, capacity, water need, maintenance, and resupply recipe.

### 8.7 `drill_profiles.json` (new)

Drill rows: hazard type, scenario, participants, measured metrics, lessons, and
retraining effect.

### 8.8 `readiness_profiles.json` (new)

Readiness rows: hazard, inputs, weights, thresholds, and degradation rates.

### 8.9 `mutual_aid_pacts.json` (new)

Pact rows: partner, obligation, response time, capacity, cost, and breach effect.

### 8.10 `incidents.json` (extend)

New incident rows across fire, smoke, collapse, flood, power, and cascade triggers,
each with authored conditions.

### 8.11 Items

New items appended to `items.json`: `item_extinguisher_charge`, `item_hose_length`,
`item_breathing_mask`, `item_rescue_rope`, `item_pry_bar`, `item_smoke_blanket`,
`item_muster_board`, `item_alarm_bell`, `item_refuge_filter`, `item_triage_tag`,
`item_drill_log`, `item_aid_crate`, `item_foam_canister`, `item_signal_whistle`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`src/Host/ShelterFireSaveStore.cs` captures fire incidents. Cascade state is
reported, not stored, by the coordinator (it reads facts and reports stages). New
sub-objects are additive in the fire envelope and the shelter envelope. No new save
section.

### 9.2 State to persist

- Readiness scores and training history.
- Routes, muster points, and their conditions.
- Command role assignments and handover.
- Rescue operations and outcomes.
- Suppression equipment and charges.
- Drill history and lessons.
- Mutual-aid pacts and obligations.
- Accountability records.

### 9.3 Determinism

- Fire spread, smoke, and rescue risk use the host-forked `ISeededRng` through the
  live fire system.
- Cascade evaluation is a pure function of facts and authored rules.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no readiness, no routes, no command, no drills, and no pacts.
Existing fire and cascade state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for readiness and risk.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `EmergencyResponseHud` (extend) | Active incident, zones, casualties, routes | `AlarmHostSession` |
| `FireIncidentPanel` (extend) | Fire zones, smoke, suppression, brigade | same |
| `ReadinessPanel` (new) | Roles, gear, routes, scores | same |
| `EvacuationPanel` (new) | Routes, muster, headcount, missing | same |
| `CommandPanel` (new) | Command role, authority, handover, log | same |
| `RescuePanel` (new) | Teams, sectors, time, pull-back | same |
| `DrillPanel` (new) | Profiles, execution, lessons | same |
| `CascadePanel` (new) | Active rules, warnings, off-ramps | same |
| `MutualAidPanel` (new) | Pacts, obligations, aid runs | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Cascade warnings and off-ramps are always visible; no surprise failures.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Missing-person states are explicit and never silently dropped.
- Rescue risk and time limits are stated before committing.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: alarm bell, damper closing, extinguisher
hiss, smoke alarm, muster call, rope strain, rescue whistle. No cue is required;
text carries meaning. The alarm has no dramatic fanfare.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ShelterFireHazardSystem` | Zones, suppression, brigade extended |
| `CascadeCoordinator` | Authored rules and off-ramps extended |
| `CascadeRuleCatalog` | Rule data extended |
| `DiseaseTriage` | Casualty sorting consumed |
| `DistressRescueMissionManager` | Mutual aid expeditions |
| `DutyRoster` | Emergency roles |
| `SkillProgressionSystem` | Training and readiness |
| `NeedsSystem` / `MoraleContagionSystem` | Fear, muster, and recovery |
| `GuiltInsomniaSystem` | Rescue and pull-back guilt |
| `MedicalPipelineCoordinator` | Casualty care |
| `PowerGridSystem` | Dampers, fans, alarms, pumps |
| `WaterTreatmentSystem` | Suppression water and refuge supply |
| `FactionStanceEngine` | Aid pacts |
| `ExpeditionSystem` | Aid runs |
| `MemorialSystem` | Fire and rescue deaths |
| `Inventory` | Gear, charges, supplies |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `ShelterFireHazardSystem`,
`CascadeCoordinator`, `CascadeRuleCatalog`, `DiseaseTriage`, rescue dispatch, save
stores, and panels. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend cascade rules and incidents; author routes,
muster points, roles, rescue teams, suppression equipment, drill profiles, readiness
profiles, aid pacts. Register validators and scanner.

**Phase 2 — Pure Core.** `ReadinessSystem`, `EvacuationSystem`,
`IncidentCommandSystem`, `RescueSystem`, `SuppressionSystem`, `DrillSystem`,
`MutualAidSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `AlarmHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 90/180-day soak including fires, cascades, and drills.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Cascade rules | 20 |
| Evacuation routes | 20 |
| Muster points | 10 |
| Emergency roles | 12 |
| Rescue teams | 10 |
| Suppression equipment | 15 |
| Drill profiles | 15 |
| Readiness profiles | 12 |
| Mutual-aid pacts | 8 |
| Incidents | 40 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second fire/cascade system | Critical | Extend live owners |
| Cascade without off-ramp | Critical | Authored warning + off-ramp |
| Rescue trivialized | High | Time, risk, loss, pull-back |
| Readiness becomes a hidden stat | High | Derived and surfaced |
| Drills feel like homework | Medium | Real measured outcomes |
| Mutual aid cutscene | Medium | Real expedition risk |
| Determinism break | Low | Host-forked RNG |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `cascade_rules.json` | +20 | 5,000 |
| `evacuation_routes.json` | 20 | 3,500 |
| `muster_points.json` | 10 | 2,000 |
| `emergency_roles.json` | 12 | 2,500 |
| `rescue_teams.json` | 10 | 2,500 |
| `suppression_equipment.json` | 15 | 3,000 |
| `drill_profiles.json` | 15 | 4,000 |
| `readiness_profiles.json` | 12 | 2,500 |
| `mutual_aid_pacts.json` | 8 | 2,000 |
| `incidents.json` | +40 | 6,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~63,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R23-1 | Second fire/cascade system | Low | Critical | Extend owners |
| R23-2 | Cascade ambush | Med | Critical | Warning + off-ramp |
| R23-3 | Rescue trivial | Med | High | Risk, time, loss |
| R23-4 | Hidden readiness stat | Med | High | Derived and surfaced |
| R23-5 | Drill fatigue | Med | Med | Real outcomes |
| R23-6 | Aid cutscene | Med | Med | Expedition risk |
| R23-7 | Determinism | Low | High | Host-forked RNG |
| R23-8 | Content overrun | Med | Med | Budget §13 |
| R23-9 | Headcount opaque | Med | Med | Explicit missing list |
| R23-10 | Command system unclear | Med | Med | Explicit authority |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can a rescue team die?** Recommended: yes, rarely, through the existing death
   and memorial path.
2. **Is readiness per-hazard or global?** Recommended: per-hazard, so fire readiness
   differs from flood readiness.
3. **Can drills be skipped without cost?** Recommended: no; readiness degrades.
4. **Does mutual aid ever arrive late?** Recommended: yes, with a real reason.
5. **Can command be refused?** Recommended: yes; delegation is a choice and a risk.

---

## 17. APPENDIX D — CASCADE RULE TABLE (20 RULES)

| # | Rule | Requires all | Warning h | Effect tags | Off-ramps |
|---|---|---|---|---|---|
| 1 | Brownout pump flood | power deficit, pumping unserved, sump rising | 24 | sump risk, contamination | restore power, manual pump, shed load |
| 2 | Cold storage warm | power deficit, cold storage unpowered | 12 | spoilage | restore power, shed load |
| 3 | Filtration fallout dose | power deficit, filtration unserved, fallout | 18 | rad ingress | restore power, seal, mask |
| 4 | Heating unserved freeze | power deficit, heating unserved, cold | 24 | exposure | restore power, burn fuel, seal |
| 5 | Darkness panic | power deficit, darkness | 6 | morale, safety | restore, lamps, curfew |
| 6 | Hatch unsealed weather | hatch unsealed, storm | 8 | contamination, cold | seal, shore, evacuate |
| 7 | Outbreak without ward | outbreak, ward unpowered | 12 | spread | restore, isolate, triage |
| 8 | Fire without pressure | fire, water unserved | 3 | spread | restore, draft, foam |
| 9 | Smoke without dampers | fire, smoke control off | 2 | smoke spread | restore, manual damper |
| 10 | Sump without pump | sump rising, pump failed | 12 | flooding | manual pump, seal |
| 11 | Well contaminated | flooding, well nearby | 24 | water exposure | cap, test, alternate |
| 12 | Cold chain break | power deficit, clinic unpowered | 12 | medicine spoilage | restore, prioritize |
| 13 | Deep air loss | power deficit, ventilation unserved | 8 | asphyxia risk | restore, evacuate |
| 14 | Communication silence | power deficit, radio unpowered | 6 | coordination loss | restore, runner, signal |
| 15 | Frozen pipes | heating unserved, cold | 12 | water loss | drain, heat, repair |
| 16 | Structural ice load | cold, storm, roof degraded | 24 | collapse | shore, clear, evacuate |
| 17 | Fuel vapor fire | fuel leak, ignition source | 4 | fire | ventilate, isolate, suppress |
| 18 | Battery vent | battery overcharge, poor vent | 6 | toxic gas | vent, reduce charge |
| 19 | Sewage backflow | drain blocked, sump high | 12 | hygiene collapse | rod, pump, lime |
| 20 | Muster failure | fire active, route blocked | 1 | casualties | alternate route, rescue |

Every rule has a warning and at least two off-ramps. The player can always act; the
game never simply decides that the shelter loses.

---

## 18. APPENDIX E — EVACUATION ROUTE TABLE (20 ROUTES)

| # | Route | From | To | Capacity | Width | Sensitivity | Alternate |
|---|---|---|---|---|---|---|---|
| 1 | North Corridor | stores | muster hall | 40 | narrow | smoke, heat | service run |
| 2 | Service Run | stores | muster hall | 25 | narrow | smoke | north corridor |
| 3 | South Gallery | living | muster hall | 60 | wide | smoke | yard stair |
| 4 | Yard Stair | living | field | 30 | narrow | weather | south gallery |
| 5 | Clinic Hall | clinic | muster hall | 20 | med | smoke | service run |
| 6 | Deep Stair | deep | muster hall | 35 | narrow | smoke, flood | ventilation path |
| 7 | Ventilation Path | deep | works | 15 | tight | heat | deep stair |
| 8 | Works Run | works | field | 25 | narrow | smoke | yard stair |
| 9 | Greenhouse Walk | greenhouse | muster hall | 30 | med | smoke | south gallery |
| 10 | Foundry Run | foundry | field | 20 | wide | heat, smoke | works run |
| 11 | Fuel Yard Path | depot | field | 15 | narrow | fire | works run |
| 12 | Clean Ward Hall | clean ward | clinic | 10 | med | smoke | clinic hall |
| 13 | Quarantine Run | quarantine | yard | 12 | med | smoke | south gallery |
| 14 | Cell Block Run | holding | muster hall | 10 | med | smoke | clinic hall |
| 15 | Radio Corridor | radio | muster hall | 12 | narrow | smoke | north corridor |
| 16 | Archive Path | archive | muster hall | 8 | narrow | smoke | radio corridor |
| 17 | Roof Access | roof | yard stair | 6 | tight | heat | none |
| 18 | Refuge Entry | muster hall | refuge | 40 | wide | none | none |
| 19 | Field Path | field | drill grounds | 100 | open | weather | none |
| 20 | Emergency Cut | living | works run | 20 | narrow | smoke, heat | yard stair |

Routes have real capacity. A muster of 120 people cannot use a 40-person corridor all
at once, and smoke closes routes the moment the fire system says so.

---

## 19. APPENDIX F — MUSTER AND REFUGE TABLE

| # | Point | Type | Capacity | Refuge hours | Headcount method |
|---|---|---|---|---|---|
| 1 | Muster Hall | internal | 80 | 2 | roll call |
| 2 | Field | external | 200 | open | roll call |
| 3 | Refuge Chamber | sealed | 30 | 12 | board check |
| 4 | Works Bay | internal | 40 | 1 | roll call |
| 5 | Clinic Lobby | internal | 20 | 2 | tag count |
| 6 | Greenhouse | internal | 30 | 1 | roll call |
| 7 | Deep Landing | internal | 25 | 2 | board check |
| 8 | Drill Grounds | external | 150 | open | roll call |
| 9 | Tower Base | external | 40 | open | board check |
| 10 | Secondary Field | external | 120 | open | roll call |

A refuge is not a fix; it is a bounded promise of twelve hours of air and water. The
muster method matters: roll call takes time, a board check is faster and misses
people who did not check in.

---

## 20. APPENDIX G — EMERGENCY ROLE TABLE (12 ROLES)

| # | Role | Skill | Authority | Gear | Successor |
|---|---|---|---|---|---|
| 1 | Incident commander | leadership | full | board, radio | deputy |
| 2 | Fire chief | firefighting | suppression | turnout | brigade lead |
| 3 | Smoke control | ventilation | dampers | mask | engineer |
| 4 | Rescue sergeant | rescue | search | rope, pry | team senior |
| 5 | Triage nurse | medicine | casualty sorting | tags | physician |
| 6 | Muster warden | organization | evacuation | board | deputy |
| 7 | Route guide | navigation | route use | lamp | second guide |
| 8 | Pump operator | mechanics | water | tools | shift mate |
| 9 | Brigade member | firefighting | suppression | turnout | any trained |
| 10 | Refuge keeper | logistics | refuge | supplies | deputy |
| 11 | Radio runner | communication | messaging | radio | any runner |
| 12 | Accountability clerk | records | headcount | list | warden |

Roles have authority and a successor. The expansion's deepest readiness problem is
not gear; it is that two people know how the dampers work and one of them is asleep.

---

## 21. APPENDIX H — RESCUE TEAM TABLE (10 TEAMS)

| # | Team | Size | Skills | Gear | Time limit | Pull-back rule |
|---|---|---|---|---|---|---|
| 1 | First Entry | 4 | fire, rescue | turnout, rope | 20 min | smoke > 0.6 |
| 2 | Search North | 3 | rescue, navigation | lamps, pry | 25 min | heat > 0.7 |
| 3 | Search South | 3 | rescue, medicine | tags, rope | 25 min | structural risk |
| 4 | Collapse Crew | 5 | digging, shoring | beams, jacks | 60 min | secondary collapse |
| 5 | Deep Team | 4 | diving, dark | ropes, air | 30 min | air < 25% |
| 6 | Rope Team | 3 | climbing | rope, harness | 40 min | anchor failure |
| 7 | Smoke Team | 4 | ventilation | masks, fans | 15 min | CO > 0.5 |
| 8 | Medical Extrication | 3 | medicine, rescue | stretcher | 30 min | casualty critical |
| 9 | Animal Rescue | 2 | handling | nets, feed | 20 min | bite risk |
| 10 | Last Look | 2 | records | list, lamp | 10 min | route closing |

The pull-back rule is authored and visible before the team enters. The player may
override it, and the override is recorded in the decision log.

---

## 22. APPENDIX I — SUPPRESSION EQUIPMENT TABLE (15 ITEMS)

| # | Equipment | Capacity | Water need | Maintenance | Resupply |
|---|---|---|---|---|---|
| 1 | Extinguisher A | 1 charge | 0 | 30 days | recharge |
| 2 | Extinguisher B | 2 charges | 0 | 20 days | recharge |
| 3 | Hose 20 m | continuous | 60 L/min | after use | splice |
| 4 | Hose 50 m | continuous | 60 L/min | after use | splice |
| 5 | Foam Canister | 5 min | 20 L/min | monthly | chemistry |
| 6 | Sand Bucket | 1 use | 0 | refill | sand |
| 7 | Fire Blanket | 3 uses | 0 | wash | cloth |
| 8 | Smoke Ejector | continuous | 0 | weekly | power |
| 9 | Thermal Curtain | 5 uses | 0 | inspect | cloth |
| 10 | Draft Pump | continuous | source | weekly | service |
| 11 | Foam Nozzle | with foam | 20 L/min | after use | clean |
| 12 | Breathing Set | 30 min | 0 | daily | air |
| 13 | Infrared Lamp | continuous | 0 | monthly | bulb |
| 14 | Cutting Axe | 20 uses | 0 | sharpen | forge |
| 15 | Water Tank Cart | 200 L | 200 L | weekly | fill |

Suppression is a finite resource. A brigade that empties its charges on a small fire
has nothing for the real one, and the expansion makes that arithmetic visible.

---

## 23. APPENDIX J — DRILL PROFILE TABLE (15 DRILLS)

| # | Drill | Hazard | Participants | Metrics | Lesson |
|---|---|---|---|---|---|
| 1 | Muster Call | evacuation | all | time, count | route capacity |
| 2 | Night Muster | evacuation | all | time, count | lighting |
| 3 | Smoke Walk | fire | brigade | route time, air | dampers |
| 4 | Extinguisher Line | fire | all adults | charge use | staging |
| 5 | Damper Drill | fire | smoke team | seconds | control point |
| 6 | Rescue Search | rescue | teams | rooms/min | marking |
| 7 | Collapse Rig | collapse | crew | shoring time | cribbing |
| 8 | Rope Descent | rescue | rope team | descent time | anchors |
| 9 | Triage Sort | medical | medical | sort time | tagging |
| 10 | Refuge Entry | shelter | all | minutes | capacity |
| 11 | Blackout Watch | power | operators | manual start | sequencing |
| 12 | Pump Failure | water | operators | time to pump | alternate |
| 13 | Headcount Board | accountability | clerks | accuracy | check-in |
| 14 | Aid Run | mutual aid | expedition | departure time | readiness |
| 15 | Full Evacuation | all | all | total time | every gap |

A drill uses the same systems as a real incident. That is what makes the score
meaningful: the shelter is not told it is ready; it demonstrates whether it is.

---

## 24. APPENDIX K — READINESS PROFILE TABLE (12 HAZARDS)

| # | Hazard | Inputs | Threshold | Degradation |
|---|---|---|---|---|
| 1 | Fire | brigade, gear, dampers | 0.70 | per use |
| 2 | Smoke | masks, fans, dampers | 0.65 | per use |
| 3 | Collapse | beams, jacks, crew | 0.60 | per use |
| 4 | Flood | pumps, seals, routes | 0.70 | per storm |
| 5 | Blackout | lamps, manual start | 0.75 | per outage |
| 6 | Cold | heat, fuel, seals | 0.70 | per cold snap |
| 7 | Outbreak | ward, masks, triage | 0.65 | per outbreak |
| 8 | Contamination | wash, suits, routes | 0.60 | per event |
| 9 | Rescue | teams, rope, time | 0.55 | per rescue |
| 10 | Evacuation | routes, muster, board | 0.75 | per drill gap |
| 11 | Mutual aid | depot, transport, pact | 0.50 | per run |
| 12 | Command | roles, deputies, log | 0.65 | per turnover |

Readiness is derived from real inputs and degrades from real use. It is never a
hidden number; it is shown with its weakest link.

---

## 25. APPENDIX L — MUTUAL AID PACT TABLE (8 PACTS)

| # | Partner | Obligation | Response | Capacity | Cost | Breach |
|---|---|---|---|---|---|---|
| 1 | Fog Ridge Camp | fire | same day | 6 people | food, gear | standing − |
| 2 | Spring Village | medical | 2 days | 2 medics | medicine | standing − |
| 3 | River Flotilla | evacuation | 1 day | boat lift | fuel | standing − |
| 4 | Foundry Enclave | structural | 3 days | crew, beams | steel | standing − |
| 5 | Deep Bunker | shelter | 1 day | 20 beds | reciprocal | review |
| 6 | Market Town | supplies | 2 days | gear | payment | standing − |
| 7 | Garrison | protection | 1 day | armed crew | obedience | crisis |
| 8 | The Registry | records | 5 days | clerks | access | review |

Mutual aid is a real expedition with real risk. A pact kept builds standing and a
pact refused costs it; neither resolves as a cutscene.

---

## 26. APPENDIX M — INCIDENT TABLE (40 INCIDENTS)

| # | Incident | Class | Severity | Conditions |
|---|---|---|---|---|
| 1 | Lamp Fall | fire | low | stores, clutter |
| 2 | Heater Line | fire | med | cold season |
| 3 | Battery Vent | gas | med | overcharge |
| 4 | Fuel Vapor | fire | high | depot |
| 5 | Kitchen Grease | fire | med | cooking |
| 6 | Foundry Spark | fire | high | metalwork |
| 7 | Chimney Soot | fire | med | heating |
| 8 | Wiring Short | fire | med | age |
| 9 | Laundry Dryer | fire | low | lint |
| 10 | Candle Tip | fire | low | blackout |
| 11 | Deep Cable Melt | fire | high | load |
| 12 | Oxygen Line | fire | high | clinic |
| 13 | Smoke Backdraft | smoke | high | dampers |
| 14 | Damper Jam | smoke | med | neglect |
| 15 | CO Build-Up | gas | med | vent |
| 16 | Stove Backflow | gas | med | cold |
| 17 | Gas Pocket Ingress | gas | high | deep |
| 18 | Sump Overflow | flood | med | rain |
| 19 | Pipe Burst | flood | med | freeze |
| 20 | Drain Backflow | flood | med | block |
| 21 | Reservoir Leak | flood | high | age |
| 22 | Roof Ponding | flood | med | storm |
| 23 | Ceiling Collapse | collapse | high | water |
| 24 | Beam Crack | collapse | high | load |
| 25 | Wall Bulge | collapse | med | soil |
| 26 | Floor Sink | collapse | high | drainage |
| 27 | Shoring Fail | collapse | high | dig |
| 28 | Stack Collapse | collapse | med | storage |
| 29 | Pump Failure | mechanical | med | wear |
| 30 | Hoist Failure | mechanical | high | load |
| 31 | Breaker Cascade | power | high | overload |
| 32 | Transformer Fire | power | high | fault |
| 33 | Alarm Failure | system | high | battery |
| 34 | Radio Silence | system | med | power |
| 35 | Muster Failure | human | critical | route |
| 36 | Headcount Loss | human | high | board |
| 37 | Rescue Entrapment | human | critical | collapse |
| 38 | Panic Surge | human | med | smoke |
| 39 | Door Jam | human | high | crowd |
| 40 | Aid Run Lost | human | high | road |

Incidents are authored conditions plus live state, never a random punishment. Each
one is designed so that preparation changes the outcome.

---

## 27. APPENDIX N — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_alarm_night_lamp` | 3 | Small fire; response and aftermath |
| `quest_alarm_the_count` | 4 | Headcount; find the missing; discover the gap |
| `quest_alarm_the_program` | 4 | Draft roles, routes, drills, gear |
| `quest_alarm_the_budget` | 4 | Argue cost; prove need; fund or trim |
| `quest_alarm_first_drill` | 5 | Run the drill; measure; publish failure |
| `quest_alarm_the_route` | 4 | Walk routes; move muster; mark hazards |
| `quest_alarm_the_pump` | 4 | Pump fails; sump rises; respond |
| `quest_alarm_the_cascade` | 5 | Read warnings; choose off-ramps |
| `quest_alarm_the_brigade` | 4 | Commit suppression; uncover a second hazard |
| `quest_alarm_the_refuge` | 4 | Provision and test the chamber |
| `quest_alarm_the_rescue` | 6 | Trapped survivor; teams, time, pull-back |
| `quest_alarm_the_siren` | 3 | Neighbor invokes the pact |
| `quest_alarm_the_aid` | 5 | Send help, send less, or refuse |
| `quest_alarm_second_fire` | 6 | Worse incident; apply every lesson |
| `quest_alarm_the_watch` | 3 | Final disposition; epilogue |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Halda Voss** — fire chief. Builds readiness with a builder's patience and a
survivor's memory of a fire she did not stop. Not a perfectionist; a person who has
watched a shelter learn the hard way and refuses to watch it again.

**Marek Dowd** — administrator. Supports readiness until it costs, then demands
proof, then pays. The expansion's realistic budget actor; not a villain, just the
person who has to say no before saying yes.

**Koval** — brigade lead. First through the door and still going, which is both
heroic and a problem. His arc is about training a successor before he becomes the
weak link.

**Lin Orr** — smoke control tech. Owns the dampers and the routes. Quietly furious
about how long it took the shelter to walk its own corridors in the dark.

**Cray** — rescue sergeant. Decides when to pull back, and carries every name he
could not reach. The expansion's hardest role and its most honest one.

**Asa Vell** — triage nurse. Sorts casualties with a clinician's calm and a
colleague's grief. Represents the medical system's role in a fire.

**Sable** — neighbor envoy. Brings the pact, states the obligation, and accepts
refusal without drama. The expansion's test of whether the shelter is part of a
region or only above it.

**Pim** — child of the muster. Holds the board and calls names. The headcount's
human meaning: a list is only real when someone reads it aloud.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Watch Tower** — a lookout with a bell, a lamp, and a log.
- **The Fallen Wing** — a collapsed corridor; cribbing, dust, and a name.
- **The Field** — an open muster ground; cold at night, easy to count.
- **The Grounds** — a regional drill site; chalk marks and old water lines.
- **The Aid Depot** — shared stores with a ledger nobody enjoys signing.
- **The Firebreak** — a cut line around the shelter; the wildfire side.
- **The Firehouse** — pre-war gear, rusted but instructive.
- **The Draft Point** — water for suppression; a hose run and a pump.
- **The Control Room** — old alarm panels and a manual that explains them.
- **The Sirens** — a network node; the sound that means someone else needs help.

---

## 30. APPENDIX Q — FIRE AND SMOKE MODEL

The live system tracks four per-zone values. The expansion gives them meaning:

| Value | Meaning | Player action |
|---|---|---|
| fireLevel | active burning | suppress, starve, isolate |
| smokeLevel | particulate | dampers, vent, evacuate |
| coLevel | carbon monoxide | vent, mask, evacuate |
| heatLevel | temperature | cool, curtain, retreat |

Adjacency spreads fire and smoke along authored zones. Dampers either confine or
route. A suppression team that is fighting fire cannot simultaneously manage smoke,
which is why the expansion forces the choice between saving the room and saving the
route.

---

## 31. APPENDIX R — HEADCOUNT AND ACCOUNTABILITY MODEL

| Method | Speed | Accuracy | Requirement |
|---|---|---|---|
| Roll call | slow | high | clerk, list |
| Board check-in | fast | medium | board, discipline |
| Buddy pairs | fast | high | pairing culture |
| Room sweep | slow | high | teams |
| Tag board | medium | high | tags |
| None | instant | none | — |

Accuracy is not a feeling. The expansion measures it and surfaces missing persons as
a concrete list of names. The most memorable scene in the expansion is not the fire;
it is the pause after the second call.

---

## 32. APPENDIX S — WORKED 180-DAY EMERGENCY SCENARIO

**Days 1–15.** The night lamp fire. The headcount takes 40 minutes, misses two
people who had gone to the greenhouse, and the shelter decides to build a program.

**Days 16–40.** Roles are assigned, routes are walked, extinguishers are staged. The
first drill measures a 22-minute muster and a blocked service run. The failure is
published, not buried.

**Days 41–60.** A pump fails; the sump rises. Cascade warnings appear with 12–24 hour
windows. The shelter restores power, runs a manual pump, and sacrifices the foundry
load. Cold storage holds.

**Days 61–90.** The refuge chamber is provisioned and tested for six hours. A rope
team is trained. A rescue operation retrieves a survivor from the flooded wing; one
rescuer is injured and treated.

**Days 91–120.** A neighbor invokes the mutual-aid pact. The shelter sends a team;
their road is worse than expected; the team arrives late and still helps. Standing
rises. The aid depot is restocked from the neighbor's stores.

**Days 121–150.** Smoke control is drilled at night. The damper jam is found and
fixed before the second fire. The headcount method changes to buddy pairs.

**Days 151–180.** The second fire: a chimney soot incident that spreads to the
works. Every lesson is exercised. The shelter is faster, calmer, and still not
perfect. The epilogue records the readiness score and the names on the board.

---

## 33. APPENDIX T — VIGNETTE (TONE SAMPLE)

> The bell is a length of pipe struck with a wrench, and it is loud enough. Halda
> stands at the head of the corridor with the board under her arm and calls the
> names in order, and the answer comes back from the muster hall, and then from the
> clinic, and then there is a pause.
>
> She calls the next name twice. Pim, at the board, looks up and does not say
> anything, because there is nothing to say yet, and the pause is the whole of the
> night.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Alarm failure | late response | manual watch, spares |
| Route blocked | trapped people | alternate route, rescue |
| Muster failure | missing persons | sweep, search teams |
| Suppression empty | fire spreads | withdraw, contain, rebuild |
| Damper jam | smoke spread | manual control, evacuate |
| Rescue loss | casualty | medical, memorial, review |
| Cascade unchecked | multiple failures | off-ramp, restore, triage |
| Refuge breach | trapped occupants | rescue, seal, relocate |
| Aid refused | standing loss | compensate, renegotiate |
| Drill skipped | readiness decay | retrain, re-drill |

No failure is a game over. Every failure has a recovery path, and every recovery
costs time, gear, or trust. The deepest failure is a shelter that stops drilling
because the last drill was embarrassing.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] `ShelterFireHazardSystem` remains the fire authority.
- [ ] `CascadeCoordinator` remains the cascade authority.
- [ ] Every cascade rule has a warning and at least two off-ramps.
- [ ] No fire is scripted to kill without route and warning.
- [ ] Rescue has time, risk, and loss.
- [ ] Readiness is derived, never a hidden stat.
- [ ] Headcounts produce concrete missing-person lists.
- [ ] Mutual aid is a real expedition.
- [ ] Casualties route through the medical pipeline.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 36. APPENDIX W — GLOSSARY

- **Readiness** — a derived per-hazard preparedness score from real inputs.
- **Muster** — an assembly point with capacity and a headcount method.
- **Route** — an authored evacuation path with capacity and sensitivity.
- **Refuge** — a sealed room with bounded air and water.
- **Cascade rule** — an authored failure combination with warning and off-ramps.
- **Off-ramp** — an action that breaks a cascade.
- **Pull-back** — the authored decision to abandon a rescue.
- **Pact** — a mutual-aid obligation between settlements.
- **Brigade** — the trained suppression roster.
- **Decision log** — the durable record of command choices.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ShelterFireHazardSystem` | zones | fire state | health |
| `CascadeCoordinator` | facts | reported stages | facts |
| `CascadeRuleCatalog` | rules | — | — |
| `DiseaseTriage` | infection | triage state | disease |
| `DistressRescueMissionManager` | dispatch | missions | — |
| `DutyRoster` | roles | — | — |
| `SkillProgressionSystem` | skills | training | — |
| `NeedsSystem` | fear | morale | — |
| `MoraleContagionSystem` | muster | contagion | — |
| `GuiltInsomniaSystem` | pull-back | guilt | — |
| `MedicalPipelineCoordinator` | casualties | treatment | — |
| `PowerGridSystem` | dampers | — | — |
| `WaterTreatmentSystem` | suppression | water | — |
| `FactionStanceEngine` | pacts | standing | — |
| `ExpeditionSystem` | aid runs | expeditions | — |
| `MemorialSystem` | deaths | memorials | — |
| `Inventory` | gear | transfers | — |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`evacuation_routes.json`** — `route_id`, `display_name`, `from_zone_id`,
`to_muster_id`, `capacity`, `width_class`, `condition`, `hazard_sensitivity[]`,
`alternate_route_id`, `tags`.

**`muster_points.json`** — `muster_id`, `display_name`, `point_type`, `capacity`,
`refuge_hours`, `headcount_method`, `adjacency[]`, `tags`.

**`emergency_roles.json`** — `role_id`, `display_name`, `required_skill`,
`authority_scope`, `gear[]`, `successor_role_id`, `tags`.

**`rescue_teams.json`** — `team_id`, `display_name`, `size`, `skills[]`, `gear[]`,
`time_limit_min`, `pull_back_rule`, `risk_tolerance`, `tags`.

**`suppression_equipment.json`** — `equipment_id`, `display_name`, `capacity`,
`water_need_lpm`, `maintenance_days`, `resupply_recipe[]`, `tags`.

**`drill_profiles.json`** — `drill_id`, `display_name`, `hazard`, `participants`,
`metrics[]`, `lesson_effect`, `retraining_days`, `tags`.

**`readiness_profiles.json`** — `profile_id`, `hazard`, `inputs[]`, `weights[]`,
`threshold`, `degradation_per_event`, `tags`.

**`mutual_aid_pacts.json`** — `pact_id`, `partner_faction_id`, `obligation`,
`response_days`, `capacity`, `cost[]`, `breach_effect`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid zone/route/faction references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Muster time | evacuation quality | EvacuationSystem |
| Headcount accuracy | accountability | EvacuationSystem |
| Readiness per hazard | preparedness | ReadinessSystem |
| Drill frequency | training | DrillSystem |
| Cascade warnings seen | fairness | CascadeCoordinator |
| Off-ramps used | agency | CascadeCoordinator |
| Rescue time | risk | RescueSystem |
| Pull-backs | ethics pressure | RescueSystem |
| Aid runs fulfilled | regional standing | MutualAidSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether the alarm is tense or merely noisy.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Every cascade rule has a warning and off-ramps.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows fires, cascades, drills, and recovery paths.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel fire, cascade, triage, or rescue system exists.

---

## 41. APPENDIX AB — CAMPAIGN ARC TIMELINE

| Phase | Days | Theme | Decision |
|---|---|---|---|
| First fire | 1–20 | shock | build a program or not |
| Setup | 21–50 | preparation | roles, routes, gear |
| First drill | 51–80 | honesty | fix or hide gaps |
| Pump failure | 81–110 | cascade | off-ramp choice |
| Rescue | 111–140 | risk | push or pull back |
| Mutual aid | 141–170 | region | fulfill or refuse |
| Second fire | 171–220 | proof | apply the lessons |
| Watch | 221–260 | legacy | permanent or voluntary |

Each phase changes the shelter's relationship to readiness, risk, and each other.

---

## 42. APPENDIX AC — OPEN QUESTIONS FOR REVIEW

1. Should a rescue team be permanently lost, or only injured?
2. Should readiness decay without drills, or only with gear use?
3. Should the refuge be able to fail structurally?
4. Should mutual aid be refusable without a permanent penalty?
5. Should the incident commander be player-named or survivor-assigned?
6. Should a drill be skippable in an emergency week?
7. Should cascade off-ramps have costs that are always visible in advance?
8. Should the second fire be authored, or emergent from live conditions?

None of these may be decided unilaterally; each changes the expansion's balance and
emotional contract.

---

## 44. APPENDIX AD — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_alarm_roles` | 4 | List needs, assign people, name successors |
| `quest_alarm_equipment` | 4 | Inventory, service, stage, log |
| `quest_alarm_routes` | 4 | Walk, mark, time, publish |
| `quest_alarm_headcount` | 4 | Choose a method, build the board |
| `quest_alarm_readiness_report` | 3 | Measure, admit gaps, publish |
| `quest_alarm_extinguisher_line` | 4 | Charge, stage, mark, verify |
| `quest_alarm_damper_drill` | 3 | Locate, operate, time |
| `quest_alarm_hose_test` | 4 | Lay, pressurize, repair, store |
| `quest_alarm_wildfire_line` | 5 | Cut, wet, patrol, hold |
| `quest_alarm_vent_plan` | 4 | Map smoke paths, set dampers |
| `quest_alarm_structural_check` | 4 | Inspect fire load, clear, report |
| `quest_alarm_search_team` | 4 | Recruit, kit, train, certify |
| `quest_alarm_collapse_rescue` | 6 | Locate, shore, dig, extract |
| `quest_alarm_rope_work` | 4 | Anchor, rig, descend, ascend |
| `quest_alarm_trapped` | 5 | Locate, reach, stabilize, carry |
| `quest_alarm_pull_back` | 3 | Judge risk, order, live with it |
| `quest_alarm_muster_drill` | 3 | Call, count, time, learn |
| `quest_alarm_secondary_route` | 4 | Survey, clear, mark, test |
| `quest_alarm_refuge_air` | 4 | Filters, water, seats, seal test |
| `quest_alarm_evac_families` | 4 | Reunify, account, reassure |
| `quest_alarm_abandon_room` | 4 | Order, seal, salvage, accept |
| `quest_alarm_cascade_map` | 4 | Trace links, confirm, publish |
| `quest_alarm_off_ramp` | 4 | Weigh systems, choose, execute |
| `quest_alarm_warning_window` | 3 | Pre-stage a response |
| `quest_alarm_pump_priority` | 4 | Argue loads, set order, log |
| `quest_alarm_dark_watch` | 4 | Operate through blackout |
| `quest_alarm_pact_terms` | 5 | Draft, negotiate, sign |
| `quest_alarm_aid_depot` | 4 | Stock, inventory, train |
| `quest_alarm_aid_run` | 5 | Load, travel, deliver, return |
| `quest_alarm_aid_refused` | 3 | Refuse, explain, absorb |

---

## 45. APPENDIX AE — COMMAND MODEL DETAIL

Command is a chain with explicit authority. The expansion authors it:

| Level | Holder | Authority | Fails when |
|---|---|---|---|
| Incident commander | named chief | all decisions | absent or exhausted |
| Sector lead | brigade/rescue | their sector | unclear orders |
| Evacuation warden | clerk/warden | routes and muster | route closes |
| Medical lead | physician | casualty priority | overwhelmed |
| Logistics lead | quartermaster | gear and supplies | stock short |
| Deputy | successor | inherits on handover | unclear handover |

Command is a person, and a person can be tired. The expansion models handover as a
deliberate act: if no one is named, the shelter defaults to whoever shouts first,
which is exactly how shelters lose people. The decision log records every order and
every override, and the epilogue reads from it.

---

## 46. APPENDIX AF — REGIONAL EMERGENCY MAP

| Settlement | Capability | Weakness | Pact value |
|---|---|---|---|
| The shelter | mixed brigade | gear stock | mutual |
| Fog Ridge Camp | rope, height | no water | high |
| Spring Village | medical | no brigade | high |
| River Flotilla | evacuation lift | no fire gear | med |
| Foundry Enclave | shoring, beams | slow | high |
| Deep Bunker | refuge space | isolated | med |
| Market Town | supplies | no crew | med |
| Garrison | armed protection | coercive terms | low |

No settlement can handle a major incident alone, which is what makes the pact
network a real regional institution rather than a favor.

---

## 47. APPENDIX AG — STANDING WATER AND SUPPRESSION MODEL

Fire suppression needs water, and water needs power, and power needs fuel, and fuel
needs a depot that might be on fire. The expansion ties them:

| Suppression action | Water | Power | Time | Effect |
|---|---|---|---|---|
| Extinguisher | 0 | 0 | instant | small fire |
| Hose line | 60 L/min | pump 400 W | minutes | room fire |
| Foam attack | 20 L/min | pump 400 W | minutes | fuel fire |
| Draft fill | 200 L | pump 800 W | 10 min | refill cart |
| Water curtain | 40 L/min | pump 300 W | continuous | contain spread |
| Flood a zone | high | pump 900 W | 30 min | last resort |

A brigade that suppresses a small fire with a hose line has spent water the shelter
will need tomorrow. That trade-off is the expansion's quiet economy of safety.

---

## 48. APPENDIX AH — STRUCTURAL DAMAGE TABLE

| Damage | Cause | Effect | Repair |
|---|---|---|---|
| Charred beam | fire | load loss | replace |
| Spalled wall | heat | weakness | patch |
| Cracked floor | collapse | route loss | shore |
| Warped door | heat | route jam | refit |
| Buckled rail | collapse | access loss | straighten |
| Waterlogged floor | suppression | rot risk | dry |
| Smoke-stained duct | smoke | vent loss | clean |
| Melted cable | fire | power loss | rewire |
| Sagging roof | fire, load | collapse risk | shore |
| Broken glass | heat | vent loss | board |

Repair consumes materials from the foundry and foundry time from the grid, which
links the expansion to the other survival systems rather than isolating it.

---

## 49. APPENDIX AI — POST-INCIDENT REVIEW MODEL

After every real incident and every drill, the shelter can conduct a review:

| Step | Output | Effect |
|---|---|---|
| Timeline | what happened, when | accuracy |
| Headcount | who was where | accountability |
| Decisions | orders and overrides | command quality |
| Gaps | what failed | readiness target |
| Lessons | what changes | training |
| Publication | what is disclosed | trust |

A review can be honest, partial, or closed. An honest review is painful and improves
readiness; a closed review preserves comfort and repeats the failure. That choice is
the expansion's moral core.

---

## 50. APPENDIX AJ — LORE: THE ALARM CULTURE

The pre-war world trained for fires and floods; the shelter inherited the ruins and
none of the habit. The fiction:

- **The Firehouse** was a municipal station; its turnout gear is the shelter's first
  brigade kit, and its manual is the expansion's founding text.
- **The Sirens** were a civil defense network; the shelter can still receive and
  transmit on some of its nodes, which is why mutual aid is even possible.
- **The Drill Grounds** were a schoolyard used for emergency exercises; the shelter
  uses it because it is flat and already has painted lanes.
- **The Watch Tower** is a scavenged fire tower, adopted for early detection.
- **The Aid Depot** is a pre-war emergency cache, shared and resent in equal measure.

No real fire service, civil defense program, or doctrine is copied. The Alarm Culture
is fictional and exists to explain why the wasteland's emergency knowledge is
partially recoverable.

---

## 51. APPENDIX AK — WORKED READINESS NUMBERS

| Hazard | Start | After program | After neglect |
|---|---|---|---|
| Fire | 0.35 | 0.72 | 0.50 |
| Smoke | 0.30 | 0.65 | 0.45 |
| Collapse | 0.20 | 0.58 | 0.38 |
| Flood | 0.40 | 0.70 | 0.52 |
| Blackout | 0.45 | 0.75 | 0.55 |
| Cold | 0.38 | 0.70 | 0.50 |
| Outbreak | 0.42 | 0.66 | 0.48 |
| Contamination | 0.25 | 0.60 | 0.40 |
| Rescue | 0.15 | 0.55 | 0.35 |
| Evacuation | 0.30 | 0.75 | 0.50 |
| Mutual aid | 0.10 | 0.50 | 0.30 |
| Command | 0.35 | 0.65 | 0.45 |

Readiness starts low, improves with deliberate investment, and decays without use.
The numbers are authored so that the player feels the difference between a shelter
that has drilled and one that has only intended to.

---

## 52. APPENDIX AL — CLOSING VIGNETTE

> The second fire is worse than the first. It starts in the works and it has a
> head start, and by the time the bell goes the corridor is already grey. Halda is at
> the board before anyone else, and Pim is calling names, and the answers come back
> faster this time, in order, from every room.
>
> There is a pause at the same name, because there is always a pause, and then a
> voice from the service run says "here," and Pim marks the board and calls the next.
>
> Nobody in the shelter would call the night good. But nobody is missing, either, and
> that is what the last six months bought.

---

## 53. CLOSING STATEMENT

ASHFALL already models the bad night in typed detail: fire zones with smoke and heat,
dampers and evacuation flags, a fire brigade and extinguisher charges, structural
damage, cascade facts and rule warnings with off-ramps, and clinical triage. What it
lacks is a shelter that is ready: roles before the alarm, routes that are walked,
counts that account for everyone, teams that go back in, drills that tell the truth
about readiness, and neighbors who come when the siren carries. The Alarm adds that
culture without adding a second fire or cascade system, and without ever ambushing
the player. It adds a bell, a board, a rope, and the only kind of heroism this game
believes in: preparation.

> Wave 3 note: this plan is one of five Wave 3 expansion bibles (22–26). Each is
> self-contained; none requires another to ship. The shared Wave 3 index lives at
> `docs/expansions/wave3/WAVE3_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible.