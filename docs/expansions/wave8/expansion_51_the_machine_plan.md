# ASHFALL — Expansion 51 Design Bible
# THE MACHINE
### Wave 8 · Reactivation, Directives, Machine Duty, Docks, Maintenance, Malfunction Protocol, Names, and Retirement

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Crafting` (`RoboticsSystem`), `Ashfall.Core` (`PowerGridSystem` read seam), `Ashfall.Core.Shelter` (`ShelterWorkshopSystem` repair seam), `Ashfall.Core.Inventory`
**Proposed host owner:** `MachineYardHostSession` (extends `RoboticsSaveStore` + `RoboticsWorkshopPanel`)
**Existing save sections:** `robotics` (`robotics_save.json`, `RoboticsSaveState`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no robotics-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has a working robotics system. `RoboticsSystem` defines
`RobotDefinition` (`Id`, `DisplayName`, `Role` default "utility",
`ArmorRating` 0.2f, `MaxChassisIntegrity` 1000, `MaxCoreChargeWh` 5000,
`LaborDrainW` 250, `ChargingRateW` 500, `EmpDisableHours` 24,
`ReactivationMaterials`, `CompatibleTasks`, `Description`),
`RobotUnitState` (`UnitId`, `DefinitionId`, `ChassisIntegrity`,
`LogicIntegrity`, `CoreChargeWh`, `AssignedDirective` default "directive_idle",
`AssignedTask`, `IsEmpDisabled`, `EmpDisableHoursRemaining`, `IsRogue`), and
`RoboticsSaveState` (`SystemId` = "robotics_system", `Units`,
`TotalUnitsReactivated`, `TotalRogueEventsTriggered`). The API is live:
`LoadCatalog(json)`, `ReactivateRobot(definitionId, programmerSkill01, out
error)` (unit ids are numbered `robot_{role}_{n:000}`, starting at half
charge), `ProgramDirective(unitId, directiveId, programmerSkill01, out error)`
(including the condition that can set `IsRogue`), `ApplyEmpShock(disableHours)`,
`TickLabor(hours, isDockedToGrid, gridPowerAvailableWatts)` (EMP countdown,
charging when docked with grid headroom, drain while working, idle at zero
charge, and a rogue roll when `LogicIntegrity` falls below 250),
`RepairRobot(unitId, integrityRestored)`, `CaptureState`, and `RestoreState`,
with events `OnRobotReactivated`, `OnDirectiveChanged`,
`OnRobotEmpDisabled`, `OnRogueEventTriggered`, and `OnStateChanged`. The
`robotics` save section, `RoboticsSaveStore`, and `RoboticsWorkshopPanel`
(directive select, PROGRAM DIRECTIVE, REPAIR CHASSIS, RAISE) already exist.

What does not exist: content and practice. `robotics.json` contains exactly
**five definitions** — a security sentry, a heavy loader, a utility maintenance
drone, a field scout, and a medical assistant bot — each with three compatible
tasks and reactivation materials of `scrap_electronic`, `scrap_metal`, and
`fuel_cell`. There is exactly one directive id in code, `directive_idle`.
There are no directive catalogs, no task standards, no docks, no maintenance
schedules, no malfunction protocols, no safety envelopes, no unit histories,
and no retirement path.

**The Machine** turns five dormant chassis and one idle directive into a
machine department: safe reactivation, authored directives, honest duty rotas,
docks that share the grid, maintenance that prevents the rogue roll, a
malfunction protocol that is calm instead of frightening, and a retirement
practice that gives a machine's parts back to the shelter with its card. It
extends the live system and never adds an autonomous weapon.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `RoboticsSystem` | Units, directives, charge, logic | Extends with content and practice |
| `PowerGridSystem` (Wave 2) | Generation, allocation | Requests dock load; never owns power |
| `DutyRoster` (Exp 02) | Human shifts | Machine duty coordinates; never replaces |
| `NeedsSystem` | Human needs | Machines have no needs and never gain them |
| `ShelterWorkshopSystem` (Wave 6) | Repair jobs and tools | Repairs chassis and logic through it |
| `PrecisionMetrologySystem` (Wave 6) | Calibration grades | Calibrates sensors through it |
| `KineticStorageSystem` (Wave 6) | Flywheels and surge | Docks respect surge limits |
| 40 The Wheel (Wave 6) | Mechanical power | Machines are not mechanical power |
| 36 The Watch (Wave 5) | Perimeter security | The sentry is a sensor, never a shooter |
| `MedicalWardSystem` (Wave 6) | Care | The medical bot assists; never diagnoses alone |
| 06 The Muster | Combat | No combat employment of machines exists |
| `StandingRecord` (Exp 03) | Records | Files service and retirement cards |
| `MemorialSystem` (Wave 3) | The dead | Retirement cards are not memorials |
| 42 The Core (Wave 7) | Reactor and vault | Docks draw from its grid; no reactor access |
| 47 The Brigade (Wave 8) | Fire safety | Machine charging is an electrical fire load |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Five chassis sleep in a workshop bay. One is a cargo loader the shelter could
genuinely use. One is a maintenance drone whose logic is fine. One is a scout
with a good optic. One is a medical assistant with a steady arm. And one is a
security sentry built by people who are all dead, with a line in its file that
says the last word anyone should build a machine around.

**The Machine** is the expansion about waking them carefully: writing
directives that are safe and short, docking them without blacking out the
grid, keeping their hands and minds in good order, handling a malfunction with
a stop protocol instead of a hunt, naming them without pretending they are
people, and retiring them without shame. It is the expansion about what a
shelter owes the tools it inherits.

### 1.2 The five loops it adds

```
  Wake ──► Direct ──► Work ──► Care ──► Retire
    │        │          │        │        │
    ▼        ▼          ▼        ▼        ▼
  Materials, cards,   duty    repairs,  parts,
  skill      orders   rota    docks     cards
                                        │
                                        ▼
                          Stop ──► Secure ──► Review
```

### 1.3 What the player manages

1. **Reactivation.** Which chassis wake, with what skill and materials.
2. **Directives.** Safe, short instructions with clear limits.
3. **Tasks.** What each machine does, where, and with whom.
4. **Duty.** A rota that does not take work away from people.
5. **Docks.** Charging, grid load, and priority.
6. **Care.** Chassis, logic, sensors, lubrication, and calibration.
7. **Malfunction.** Stop, secure, recover, review.
8. **Names.** What the shelter calls its machines and why.
9. **Safety envelopes.** Where machines may go and what they may never do.
10. **Retirement.** Parts, cards, and the end of a service.

### 1.4 What it is not

- Not robots with rights, feelings, or romance; they are tools people name.
- Not autonomous weapons; the sentry is a sensor platform, never a shooter.
- Not a combat system; no machine fights anything.
- Not a second power, roster, workshop, or needs system.
- Not a labor replacement story; dis-placed work is retrained, not discarded.
- Not a torture or cruelty surface; machines are never punished.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Crafting/RoboticsSystem.cs` | Units, directives, care | `LIVE` |
| `src/Host/RoboticsSaveStore.cs` | `robotics` persistence | `LIVE` |
| `src/UI/RoboticsWorkshopPanel.cs` | Directive and repair commands | `LIVE` |
| `Assets/Ashfall.Core/PowerGridSystem.cs` | Grid state and headroom | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs` | Repair jobs | `LIVE` |
| `Assets/Ashfall.Core/DutyRoster` | Human shifts | `LIVE` |
| `Assets/Ashfall.Core/PrecisionMetrologySystem.cs` | Calibration grades | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `robotics.json` | 4,431 B | **5 definitions**, 3 tasks each |
| Directive catalog | absent | only `directive_idle` in code |
| Dock, maintenance, protocol catalogs | absent | confirmed none |
| Unit names and histories | absent | confirmed none |
| Reactivation materials | live | `scrap_electronic`, `scrap_metal`, `fuel_cell` |

### 2.3 Confirmed gaps

- **GAP-51-1 — Five definitions and one directive.**
- **GAP-51-2 — No reactivation skill or decision content.**
- **GAP-51-3 — No directive authoring or validation catalog.**
- **GAP-51-4 — No task standards or duty rota content.**
- **GAP-51-5 — No docks or charging schedules.**
- **GAP-51-6 — No maintenance or calibration content.**
- **GAP-51-7 — No malfunction protocol content.**
- **GAP-51-8 — No safety envelopes.**
- **GAP-51-9 — No unit names or service histories.**
- **GAP-51-10 — No retirement or parts path.**

### 2.4 Non-duplication statement

This expansion will **not** add a second power, roster, workshop, needs,
medical, combat, or records system. It extends `RoboticsSystem` with
catalogs and practice, requests dock load from `PowerGridSystem`, routes
repairs through the workshop and metrology owners, coordinates with
`DutyRoster`, and files cards with `StandingRecord`. Machines gain no needs,
no rights narrative, and no combat role. All new state is additive inside
`RoboticsSaveState`. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — A directive is a promise about failure.** Every instruction says
what to do when it cannot be followed.

**Pillar 2 — Machines are tools with names.** People name things they work
with, and the shelter does not confuse affection with personhood.

**Pillar 3 — Maintenance prevents fear.** The rogue roll only exists below 250
logic integrity; care is the real safety system.

**Pillar 4 — Work belongs to people first.** Machines take the heavy and dull
loads; they never take a person's place without a plan for that person.

**Pillar 5 — Retirement is ordinary.** A chassis that has served gets its
parts used and its card kept, and nobody pretends it died.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Reactivation | Careful, documented, skill-gated | Awakening a myth |
| Directives | Short, bounded, reviewable | Ominous incantations |
| Work | Heavy, dull, honest labor | Slave fantasy |
| Malfunction | Stop, secure, recover, review | Attack of the machines |
| Names | Affectionate, practical | Romance or worship |
| Docks | Load schedules and honest math | Free unlimited power |
| Retirement | Parts and a card | Execution or funeral |
| Displacement | Retraining and rota talks | "The machines took it" |

### 3.3 Content limits

- No autonomous lethal authority anywhere; the sentry cannot harm a person.
- No machine-versus-human violence, hunting, or horror content.
- No torture, punishment, or humiliation of machines.
- No personhood, romance, or "they dream" content.
- No EMP weapon glorification; EMP is a storm/anomaly hazard, never a toy.
- No machine labor that replaces a person without a rostered plan.
- No new save section.

---

## 4. THE MACHINE WORLD

### 4.1 Interior rooms

- **`room_machine_bay`** — five bays, one occupied by a dust sheet.
- **`room_directive_room`** — cards, review board, and a lamp.
- **`room_dock_row`** — charging plates, cables, and the load board.
- **`room_machine_parts`** — servos, logic boards, optics, and lubricant.
- **`room_machine_logs`** — service cards, names, and years.
- **`room_workshop_machine`** — chassis stands and repair tools.
- **`room_machine_school`** — a training chassis and a chalk directive.
- **`room_retirement_bench`** — the place where parts go back to the store.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_loader_yard` | The Loader Yard | 3 | Cargo and lift work |
| `loc_drone_roofs` | The Roof Runs | 3 | Maintenance drone routes |
| `loc_scout_ridge` | The Scout Ridge | 3 | Sensor sweeps |
| `loc_dock_shed` | The Dock Shed | 2 | Charging in the cold |
| `loc_sentry_post` | The Sentry Post | 4 | Sensor-only perimeter |
| `loc_scrap_grave` | The Scrap Grave | 4 | Where chassis are found |
| `loc_retire_yard` | The Retire Yard | 3 | Parts return |
| `loc_calibration_bench` | The Calibration Bench | 2 | Sensor truth tests |
| `loc_storm_shelter_bots` | The Storm Bay | 3 | EMP season storage |
| `loc_name_wall` | The Name Wall | 2 | Unit names and years |

All locations require valid item or map-node references and scanner registration.

### 4.3 The daily rhythm

Charge cycles at night, work by day, maintenance on the rest day, reviews after
any malfunction, and a name ceremony when a unit is reactivated. The
expansion's clock is the charge cycle.

---

## 5. MAIN STORYLINE — "WHAT THE MACHINES CARRY"

### 5.1 Central conflict

**Gwen Sark** has maintained the machine bay for years without waking anything,
because a sleeping chassis is a set of problems she can carry in one hand.
**Kester Garr** wants the loader awake, because the shelter's backs are
breaking in the loading yard. **Tia Mow** wants the maintenance drone awake,
because the roofs need someone who likes heights. **Ulric Jarl** wants the
directive system proven safe before anything walks, and he has read the file
on the sentry, which describes a machine built to make decisions about people,
and he wants that chassis left under its sheet. **Wray Nesto** wants docks that
do not brown out the corridors. **Jerro Hemi** wants a duty rota that does not
tell a warehouse worker that her job is now a machine's.

Then the loader wakes and does in one week what the yard did in three, and
everyone has to sit with how good that feels and what it means. Then the drone
malfunctions on a roof — a logic fault, not a rampage, a machine walking in
slow circles and not answering — and the shelter discovers that its preparedness
was a protocol on paper. Then the storm comes and the EMP season disables the
fleet for a day, and the yard goes back to backs for an afternoon and nobody
dies, which is exactly the point.

The expansion's question: **what do you owe a tool that has done its work?**

### 5.2 Theme (unspoken)

**Care is the only real safety system.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_robotics_gwen_sark` | Gwen Sark | Lead | Policy and reactivation |
| `npc_directives_kester_garr` | Kester Garr | Programmer | Directive authoring |
| `npc_chassis_tia_mow` | Tia Mow | Chassis repair | Hands, logic, sensors |
| `npc_docks_wray_nesto` | Wray Nesto | Docks | Charge and grid load |
| `npc_duty_jerro_hemi` | Jerro Hemi | Duty foreman | Rotas and displacement |
| `npc_safety_ulric_jarl` | Ulric Jarl | Logic safety | Envelopes and reviews |
| `npc_parts_hessa_quorra` | Hessa Quorra | Parts | Store and salvage |
| `npc_apprentice_rist_yew` | Rist Yew | Apprentice | Training and logs |

### 5.4 Story beats (15)

1. **The Bay.** Five chassis are counted and dusted.
2. **The Loader.** The first unit wakes and lifts.
3. **The Lesson.** A directive fails safe and the rule is learned.
4. **The Rota.** Work is counted and shared honestly.
5. **The Dock Row.** Charging becomes a schedule.
6. **The Care.** Chassis and logic are serviced before failure.
7. **The Circles.** The drone malfunctions on a roof.
8. **The Stop.** The protocol works and the review begins.
9. **The Roofs.** The drone returns to work with a new envelope.
10. **The Names.** Units get names and cards.
11. **The Storm.** EMP disables the fleet for a day.
12. **The Sheet.** The sentry question is decided.
13. **The Retirement.** A chassis is retired to parts with its card.
14. **The Apprentice.** Rist wakes a unit alone under supervision.
15. **What the Machines Carry.** The bay becomes ordinary.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Fleet | wake most / wake few / wake none | ambition |
| Directives | strict envelopes / permissive / case-by-case | safety vs. speed |
| Docks | scheduled / opportunistic / manual | grid care |
| Duty | machine-first / shared / people-first | labor ethics |
| Names | named / numbered / mixed | affection |
| Sentry | dormant / sensor / dismantled | inherited weapons |
| Malfunction | recover always / suspend / dismantle | trust |
| Final | crew / conservative fleet / parts | identity |

### 5.6 Endings (5 + fade)

1. **The Crew** — machines work the heavy and dull loads, maintenance is
   routine, and the yard's backs are intact.
2. **The Well-Kept** — the fleet stays small and perfectly maintained, and
   nothing ever rolls below 250.
3. **The Wheel and the Hand** — machine work is shared with people by rota,
   and the warehouse crew retrains for the work that needs judgment.
4. **The Quiet Sheet** — the sentry is dismantled, its parts return to the
   store, and its card records what it was built to do and what the shelter
   chose instead.
5. **The Parts That Went On** — every retired chassis lives on in tools,
   pumps, and prosthetics, its name on the card beside the part.
6. **Fade** — a machine bay at night, a charge lamp breathing green, a name
   plate polished by a hand that is not the machine's, and a loader sleeping
   where a person once broke their back.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_machine_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_machine_bay`, `quest_machine_loader`, `quest_machine_lesson`,
`quest_machine_rota`, `quest_machine_dock_row`, `quest_machine_care`,
`quest_machine_circles`, `quest_machine_stop`, `quest_machine_roofs`,
`quest_machine_names`, `quest_machine_storm`, `quest_machine_sheet`,
`quest_machine_retire`, `quest_machine_apprentice`,
`quest_machine_what_they_carry`.

### 6.2 Side quests (30)

**Reactivation (5)**
- `quest_machine_count` — chassis counted
- `quest_machine_materials` — parts gathered
- `quest_machine_skill` — skill proven
- `quest_machine_wake` — unit woken
- `quest_machine_first_task` — first task supervised

**Directives (5)**
- `quest_machine_directive_write` — directive written
- `quest_machine_directive_test` — fail-safe tested
- `quest_machine_directive_review` — review passed
- `quest_machine_directive_simple` — short forms
- `quest_machine_directive_limit` — envelope added

**Duty (5)**
- `quest_machine_task_board` — task board kept
- `quest_machine_shift` — machine shift run
- `quest_machine_override` — human override drilled
- `quest_machine_retrain` — worker retrained
- `quest_machine_pair` — machine and person paired

**Docks (5)**
- `quest_machine_dock_build` — dock built
- `quest_machine_dock_load` — load scheduled
- `quest_machine_dock_night` — night charging
- `quest_machine_dock_repair` — cable repaired
- `quest_machine_dock_share` — share with surge

**Care (5)**
- `quest_machine_chassis` — chassis repaired
- `quest_machine_logic` — logic serviced
- `quest_machine_sensor` — sensors calibrated
- `quest_machine_lube` — joints lubricated
- `quest_machine_card` — service card current

**Aftermath (5)**
- `quest_machine_protocol` — stop protocol drilled
- `quest_machine_recover` — unit recovered
- `quest_machine_review` — malfunction reviewed
- `quest_machine_nameplate` — plate fitted
- `quest_machine_retire_parts` — parts returned

### 6.3 Repeatable quests (8)

`quest_machine_repeat_charge`, `quest_machine_repeat_care`,
`quest_machine_repeat_card`, `quest_machine_repeat_duty`,
`quest_machine_repeat_dock_check`, `quest_machine_repeat_review`,
`quest_machine_repeat_training`, `quest_machine_repeat_parts`.

### 6.4 Dynamic hooks

Live events (`OnRobotReactivated`, `OnDirectiveChanged`, `OnRobotEmpDisabled`,
`OnRogueEventTriggered`, `OnStateChanged`, storm/EMP events, grid state
changes, workshop repairs, deaths and retirements) attach authored follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- Unit state, charge, logic, and directives stay with `RoboticsSystem`.
- Power stays with `PowerGridSystem`; docks request, never allocate.
- Repairs stay with the workshop and metrology owners.
- Duty coordinates with `DutyRoster`; no second rota.
- Machines gain no needs, no combat role, and no lethal authority.
- Parts use existing items; no new economy.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `ReactivationSystem` (extend `RoboticsSystem`)

**Owns:** reactivation decisions, materials, skill checks, bay assignments,
and first-task supervision. **Consumes:** inventory materials, workshops,
training bay. **Data:** `robotics.json` (additive definitions where needed),
`robotics_tasks.json`. **Rules:** waking a unit consumes real materials and
requires a proven programmer; a unit's first tasks are supervised; a chassis
may be left sleeping without penalty.

### 7.2 `DirectiveSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** authored directives, validation, envelopes, review, and retirement.
**Consumes:** programmer skill, review board, safety owner. **Data:**
`robotics_directives.json`, `robotics_safety_envelopes.json`. **Rules:** every
directive has a scope, a stop condition, and a fail-safe; directives that could
cause harm are structurally invalid, not merely discouraged; review happens
before a unit's first unsupervised shift.

### 7.3 `MachineDutySystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** task board, machine shifts, human pairing, override drills, and
displacement plans. **Consumes:** `DutyRoster`, task standards, workshop
throughput. **Data:** `robotics_tasks.json`. **Rules:** machines take heavy,
dull, and hazardous loads; human work is not silently removed; a displaced
worker has a retraining plan before a task changes hands; overrides are
drilled, not assumed.

### 7.4 `DockSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** docks, cables, charge schedules, load requests, and priority.
**Consumes:** `PowerGridSystem` headroom, kinetic surge limits, night rota.
**Data:** `robotics_docks.json`. **Rules:** charging respects the live
`ChargingRateW` and the grid's available watts; the dock never browns out the
corridors; a dock left unserviced becomes a fire load and routes through the
fire owner.

### 7.5 `MachineCareSystem` (extend `RoboticsSystem`)

**Owns:** maintenance schedules, chassis and logic service, sensor
calibration, lubrication, and service cards. **Consumes:** workshop, metrology,
parts, lubricant. **Data:** `robotics_maintenance.json`. **Rules:** service
before thresholds, never after; logic service keeps units above the rogue roll;
a card records every intervention with day and hand.

### 7.6 `RogueResponseSystem` (extend `RoboticsSystem`)

**Owns:** the malfunction protocol: stop, secure, recover, review, and return
to service or retirement. **Consumes:** unit state, envelopes, workshop.
**Data:** `robotics_rogue_protocols.json`. **Rules:** a malfunctioning unit is
not an enemy; the protocol has no hunt; nobody is punished for a logic fault;
reviews name conditions and maintenance gaps.

### 7.7 `SafetyEnvelopeSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** where each unit may work, proximity rules, human presence, and
never-actions. **Consumes:** directives, locations, review. **Data:**
`robotics_safety_envelopes.json`. **Rules:** envelopes are specific; hazardous
areas require a paired human; no unit ever holds a weapon system state; the
sentry platform's envelope explicitly excludes any harm to people.

### 7.8 `MachineHistorySystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** unit names, name ceremonies, service histories, quirks, and
retirement cards. **Data:** `machine_logs.json`, `machine_history.json`.
Records through `StandingRecord`. **Rules:** names are practical and chosen by
the people who work with the unit; a retirement card names the unit, its years,
its work, and where its parts went; nobody pretends a machine died.

### 7.9 Systems explicitly not added

- No second power, roster, workshop, needs, or medical system.
- No autonomous weapons or combat employment.
- No machine personhood, romance, or horror content.
- No torture, punishment, or humiliation of machines.
- No EMP weapon content.
- No new currency.
- No new RNG stream beyond the live rogue roll.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `robotics.json` (extend, additive definitions)

```json
{
  "schema_version": 1,
  "robots": [
    {
      "id": "robot_utility_maintenance_drone",
      "display_name": "Utility Maintenance Drone",
      "role": "maintenance",
      "armor_rating": 0.15,
      "max_chassis_integrity": 900,
      "max_core_charge_wh": 4200,
      "labor_drain_w": 220,
      "charging_rate_w": 600,
      "emp_disable_hours": 18,
      "reactivation_materials": [
        { "item_id": "scrap_electronic", "quantity": 6 },
        { "item_id": "scrap_metal", "quantity": 8 },
        { "item_id": "fuel_cell", "quantity": 1 }
      ],
      "compatible_tasks": ["roof_check", "vent_clean", "gutter"],
      "description": "A quiet four-rotor chassis with a patient arm and a habit of hovering three centimeters too low."
    }
  ]
}
```

### 8.2 `robotics_directives.json` (new)

Directives: id, display name, scope, stop condition, fail-safe, envelope.

### 8.3 `robotics_tasks.json` (new)

Tasks: id, standard, load class, hazards, human pairing, displacement note.

### 8.4 `robotics_docks.json` (new)

Docks: id, bays served, draw watts, schedule, cable state, fire note.

### 8.5 `robotics_maintenance.json` (new)

Maintenance: unit class, interval days, work, parts, threshold, card field.

### 8.6 `robotics_rogue_protocols.json` (new)

Protocols: stage, action, roles, equipment, review question.

### 8.7 `robotics_safety_envelopes.json` (new)

Envelopes: unit, locations, proximity, paired human, never-actions.

### 8.8 `machine_logs.json` (new)

Logs: unit, day, event, actor, note, card reference.

### 8.9 `machine_history.json` (new)

History: unit, name, first day, service years, work list, retirement, parts.

### 8.10 `robotics_parts.json` (new)

Parts: item id, use, source, stock, salvage yield, note.

### 8.11 Items

New items appended to `items.json`: `item_servo_kit`, `item_bearing_set`,
`item_logic_board`, `item_optic_lens`, `item_machine_lubricant`,
`item_charging_cable`, `item_dock_plate`, `item_tow_cradle`,
`item_diagnostic_meter`, `item_directive_cards`, `item_unit_nameplate`,
`item_service_log`, `item_static_strap`, `item_spare_tracks`,
`item_sensor_kit`, `item_retire_card`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`RoboticsSaveState` remains the live save owner. New sub-objects (directives
authored, duty board, docks, maintenance cards, envelopes, logs, histories,
retirements) are additive inside it. No new save section.

### 9.2 State to persist

- Units with chassis, logic, charge, directive, and task.
- Directives authored with envelopes and reviews.
- Task board and machine duty assignments.
- Dock state, schedules, and cable condition.
- Maintenance cards and next-service days.
- Malfunction history and reviews.
- Unit names, first days, and service logs.
- Retirement records and part destinations.

### 9.3 Determinism

- Charge, drain, EMP, and rogue logic stay on the live `TickLabor` path with
  the live RNG.
- Reactivation and directive programming use live skill checks.
- Docks draw against real grid headroom.
- Maintenance prevents the rogue roll by keeping logic integrity above 250.
- Reviews derive from recorded facts.
- Paired replay hashes must match; no new RNG streams.

### 9.4 Migration

Legacy saves load with units, charge, and rogue counters intact; no directives,
docks, cards, envelopes, or histories exist until started. A unit already rogue
upon load is recoverable through the live `RepairRobot` path and the new review
flow.

### 9.5 Checksum

Invariant-culture floats; integer watt, day, and integrity fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `RoboticsWorkshopPanel` (extend) | Units and chassis | `MachineYardHostSession` |
| `DirectivePanel` (new) | Authoring and review | same |
| `MachineDutyPanel` (new) | Tasks and pairing | same |
| `DockPanel` (new) | Charge and grid load | same |
| `MachineCarePanel` (new) | Service cards | same |
| `MalfunctionPanel` (new) | Stop and recovery | same |
| `MachineHistoryPanel` (new) | Names and years | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Charge, logic, and next-service days are shown as numbers, never as dread.
- Malfunction screens are calm: steps, roles, and a stop condition are always
  visible, never a countdown of fear.
- No color-only signals; no reflex input.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Directive text is readable at the review board before accepting any shift.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a servo settling, a charge relay
clicking, a cable being coiled, a name plate being tapped twice, a chassis
rolling to the retirement bench. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `RoboticsSystem` | Units, directives, charge, rogue state |
| `PowerGridSystem` (Wave 2) | Dock load requests |
| `KineticStorageSystem` (Wave 6) | Surge limits during charging |
| `DutyRoster` (Exp 02) | Human shifts and pairings |
| `ShelterWorkshopSystem` (Wave 6) | Chassis and parts repair |
| `PrecisionMetrologySystem` (Wave 6) | Sensor calibration grades |
| `MedicalWardSystem` (Wave 6) | Medical assistant bot assistance |
| 36 The Watch (Wave 5) | Sentry sensor feed |
| 42 The Core (Wave 7) | Dock power source |
| 47 The Brigade (Wave 8) | Charging electrical load |
| `Inventory` | Materials and parts |
| `StandingRecord` (Exp 03) | Service and retirement cards |
| `MemorialSystem` (Wave 3) | Retirement is not memorial |
| `FieldGuide` (Plan 20A/28) | Machine safety reading |
| `EpilogueChronicleBuilder` | Bay history lines |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm robotics system, store, panel, grid,
roster, workshop, metrology, and records owners. Record file:line; change
nothing.

**Phase 1 — Data + validators.** Extend `robotics.json`; author the nine new
catalogs; register validators and scanner.

**Phase 2 — Pure Core.** `ReactivationSystem`, `DirectiveSystem`,
`MachineDutySystem`, `DockSystem`, `MachineCareSystem`, `RogueResponseSystem`,
`SafetyEnvelopeSystem`, `MachineHistorySystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `MachineYardHostSession`, focused selftest coverage,
fresh journey from the bay to the retirement card.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Year-long soak: maintenance prevents rogue events,
docks respect the grid, duty never displaces a person silently.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Robot definitions (total) | 10 |
| Directives | 30 |
| Tasks | 24 |
| Docks | 8 |
| Maintenance classes | 10 |
| Rogue protocols | 8 |
| Safety envelopes | 10 |
| Logs | 24 |
| Histories | 10 |
| Parts | 16 |
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
| Robot violence | Critical | No lethal authority |
| Personhood drift | High | Tools-with-names framing |
| Power duplication | Critical | Request-only docks |
| Roster duplication | Medium | Coordinate, never replace |
| Displacement insensitivity | High | Retraining plans |
| Rogue horror | High | Calm protocol |
| Determinism break | Low | Live paths only |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `robotics.json` | 5 new | 2,500 |
| `robotics_directives.json` | 30 | 5,000 |
| `robotics_tasks.json` | 24 | 4,000 |
| `robotics_docks.json` | 8 | 2,000 |
| `robotics_maintenance.json` | 10 | 2,500 |
| `robotics_rogue_protocols.json` | 8 | 2,500 |
| `robotics_safety_envelopes.json` | 10 | 3,000 |
| `machine_logs.json` | 24 | 4,000 |
| `machine_history.json` | 10 | 3,000 |
| `robotics_parts.json` | 16 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~62,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R51-1 | Violence | Low | Critical | No lethal path |
| R51-2 | Personhood | Med | High | Framing |
| R51-3 | Power overlap | Low | Critical | Requests |
| R51-4 | Roster overlap | Med | Medium | Coordination |
| R51-5 | Displacement tone | Med | High | Retraining |
| R51-6 | Rogue horror | Med | High | Calm protocol |
| R51-7 | Determinism | Low | High | Live paths |
| R51-8 | Content overrun | Med | Medium | Budget §13 |
| R51-9 | Sentry legacy | Med | High | Sheet decision |
| R51-10 | Care tedium | Med | Medium | Cards and thresholds |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Is the sentry ever useful?** Recommended: as a passive sensor platform
   only, with an envelope that structurally excludes harm to people, and with
   the dismantle option fully valid.
2. **Can a machine replace a worker?** Recommended: only with a named
   retraining plan, recorded before the task changes hands.
3. **Do machines get names?** Recommended: yes, chosen by the crew, practical,
   and never treated as personhood.
4. **What happens to a rogue unit?** Recommended: stop, secure, recover,
   review, and return or retire; never punishment.
5. **Who owns a retired unit's parts?** Recommended: the shelter store, with a
   card that records the destination of each major part.

---

## 17. APPENDIX D — UNIT DEFINITION TABLE (10 UNITS)

| # | Unit | Role | Chassis | Charge Wh | Draw W | EMP h |
|---|---|---|---|---|---|---|
| 1 | Security sentry | security | 1000 | 6000 | 350 | 24 |
| 2 | Heavy loader | hauling | 1000 | 8000 | 500 | 18 |
| 3 | Maintenance drone | maintenance | 900 | 4200 | 220 | 18 |
| 4 | Field scout | scouting | 800 | 5000 | 260 | 20 |
| 5 | Medical assistant | medical | 850 | 4800 | 240 | 16 |
| 6 | Lifting arm | hauling | 950 | 7000 | 460 | 18 |
| 7 | Cold-storage hauler | hauling | 900 | 6500 | 420 | 20 |
| 8 | Crop tender | farming | 750 | 3800 | 200 | 16 |
| 9 | Pump runner | utility | 800 | 4000 | 210 | 17 |
| 10 | Welding assistant | workshop | 900 | 5200 | 300 | 19 |

Ten unit definitions, five inherited and five authored to fill the jobs the
shelter actually has. The choreography in the numbers matters: the loader
carries the most charge and the least downtime, because the loader is the unit
the yard genuinely needs, and the crop tender is small on purpose because a
machine in a garden should be light on the beds.

---

## 18. APPENDIX E — DIRECTIVE TABLE (30 DIRECTIVES)

| # | Directive | Scope | Stop condition | Fail-safe |
|---|---|---|---|---|
| 1 | Idle | none | always | park, low power |
| 2 | Lift goods | yard | load over limit | set down, idle |
| 3 | Stack crates | store | aisle under width | stop, wait |
| 4 | Haul water | plant | tank full | return, idle |
| 5 | Roof check | roofs | wind over limit | land, hold |
| 6 | Gutter clear | roofs | rain | land, hold |
| 7 | Vent clean | shafts | air below limit | withdraw |
| 8 | Scout ridge | ridge | weather closing | return home |
| 9 | Count stock | store | mismatch | stop, report |
| 10 | Assist surgery | ward | human present | hold, wait |
| 11 | Move patient | ward | patient objects | stop, call |
| 12 | Pump watch | plant | pressure alarm | idle, call |
| 13 | Crop water | garden | soil wet | move on |
| 14 | Seed sort | garden | bin full | idle |
| 15 | Weld support | workshop | human within 2 m | stop |
| 16 | Hold panel | workshop | hand nearby | release |
| 17 | Cold haul | cold store | door open | wait |
| 18 | Pallet move | yard | person in path | stop, wait |
| 19 | Path clear | corridors | person in path | stop, wait |
| 20 | Door hold | corridors | person through | close |
| 21 | Perimeter watch | fence | human approach | report only |
| 22 | Sensor sweep | fence | storm | stow, hold |
| 23 | Escort beacon | gate | gate closed | hold |
| 24 | Training loop | bay | trainer stop | idle |
| 25 | Charge seek | dock | dock full | queue |
| 26 | Return home | bay | -- | park |
| 27 | Shutdown | any | order given | park, off |
| 28 | Report fault | any | fault found | stop, flash |
| 29 | Follow guide | any | guide stops | stop |
| 30 | Weather hold | any | severe weather | park |

Thirty directives, every one with a stop condition and a fail-safe, and the two
that matter most are the ones a shelter is tempted to skip: person-in-path stops
for the yard and corridors, and human-presence holds at the ward and the welding
bench. A directive without a stop condition does not pass review, and the system
makes that structural rather than polite.

---

## 19. APPENDIX F — TASK STANDARD TABLE

| # | Task | Load | Hazards | Pairing | Displacement note |
|---|---|---|---|---|---|
| 1 | Lift pallets | heavy | crush | no | yard crew retrained |
| 2 | Stack store | heavy | fall | no | keeper keeps plan |
| 3 | Haul water | medium | slip | no | plant keeps valves |
| 4 | Roof check | light | fall | yes | roofer keeps judgment |
| 5 | Gutter clear | light | fall | yes | roofer keeps judgment |
| 6 | Vent clean | light | air | yes | plant keeps fans |
| 7 | Scout ridge | light | terrain | no | scout keeps route |
| 8 | Stock count | light | none | yes | clerk keeps ledger |
| 9 | Ward assist | light | medical | yes | nurse keeps care |
| 10 | Pump watch | light | pressure | yes | engineer keeps call |
| 11 | Crop water | light | none | no | grower keeps plan |
| 12 | Seed sort | light | dust | no | grower keeps choice |
| 13 | Weld support | medium | sparks | yes | welder keeps weld |
| 14 | Cold haul | medium | frost | no | keeper keeps stock |
| 15 | Path clear | light | none | yes | warden keeps order |
| 16 | Perimeter watch | light | none | yes | watch keeps judgment |
| 17 | Training | light | none | yes | trainer keeps stop |
| 18 | Dock queue | light | electric | no | dock keeper plans |

Eighteen task standards, and the pairing and displacement columns are the
labor ethics written as data: machines may lift, haul, and watch, and every
single task that requires judgment keeps a person, named, holding it. The
retraining column is why the duty system exists — nobody wakes a machine and
walks a worker to the door.

---

## 20. APPENDIX G — DOCK TABLE

| # | Dock | Bays | Draw W | Schedule | Cable | Fire note |
|---|---|---|---|---|---|---|
| 1 | Bay one | heavy | 1200 | night | inspected | clearance |
| 2 | Bay two | utility | 800 | night | inspected | clearance |
| 3 | Bay three | light | 500 | day gap | inspected | clearance |
| 4 | Yard dock | mobile | 1000 | evening | coiled | outdoor |
| 5 | Roof dock | drone | 600 | dusk | sealed | dry |
| 6 | Ward dock | medical | 700 | night | inspected | hospital rule |
| 7 | Training dock | school unit | 500 | supervised | short cable | fire class |
| 8 | Storm dock | any | 400 | emergency | spare | surge guard |

Eight docks, and the schedule column is the shelter's honesty about its power:
heavy charging happens at night when the corridors are quiet, the ward dock
follows a hospital rule about current and damp, and the storm dock is guarded
because the night it is needed is the night the grid is weakest.

---

## 21. APPENDIX H — MAINTENANCE TABLE

| # | Class | Interval | Work | Parts | Threshold |
|---|---|---|---|---|---|
| 1 | Hauling | 14 days | joints, tracks, lube | bearing set | chassis 700 |
| 2 | Utilities | 12 days | arms, filters, lube | servo kit | chassis 650 |
| 3 | Roofing | 10 days | rotors, guards, lube | rotor set | chassis 700 |
| 4 | Scouting | 12 days | optics, legs | optic lens | chassis 650 |
| 5 | Medical | 10 days | arm, optics, clean | sensor kit | chassis 750 |
| 6 | Workshop | 14 days | torque, cable, lube | spare tracks | chassis 700 |
| 7 | Docks | 7 days | cable, plate, earth | charging cable | insulation |
| 8 | Logic (all) | 30 days | board check, logs | logic board | logic 400 |
| 9 | Calibration | 60 days | sensor truth check | gauge kit | drift |
| 10 | Seals | 90 days | gaskets, boots | seal kit | watertight |

Ten maintenance classes with intervals and thresholds, and the logic row is the
true one: service every thirty days keeps logic integrity far above the
250 line where the rogue roll lives. The expansion can therefore say something
both true and calm — malfunctions are a maintenance failure, not a monster.

---

## 22. APPENDIX I — ROGUE PROTOCOL TABLE

| # | Stage | Action | Roles | Equipment | Review question |
|---|---|---|---|---|---|
| 1 | Notice | log first sign | any | card | what changed? |
| 2 | Clear | people move clear | warden | calm call | was anyone near? |
| 3 | Stop | stop instruction | programmer | card | did it fail safe? |
| 4 | Strand | remove charge path | dock keeper | cable | is the dock safe? |
| 5 | Secure | surround, no touch | pair | cradle | is the unit contained? |
| 6 | Diagnose | read logic, cards | tech | meter | when was service? |
| 7 | Recover | repair and reset | tech | parts | what part failed? |
| 8 | Return | supervised task | foreman | envelope | what envelope changed? |

Eight stages, and the fourth is the one no horror story includes: you do not
fight a malfunctioning loader, you pull its cable and wait, and the hardest
stage is the last, where the unit goes back to a reduced envelope with an
escort and the shelter's trust repaired alongside its logic board.

---

## 23. APPENDIX J — SAFETY ENVELOPE TABLE

| # | Unit | Locations | Proximity | Paired | Never |
|---|---|---|---|---|---|
| 1 | Loader | yard, store | 3 m under load | no | over people |
| 2 | Drone | roofs, shafts | none | yes | storms |
| 3 | Scout | ridge, gate | none | no | inside |
| 4 | Medical | ward | arm's reach | yes | needles alone |
| 5 | Sentry | fence only | 10 m | yes | harm people |
| 6 | Pump runner | plant | pipes only | yes | valves alone |
| 7 | Crop tender | garden | none | no | sheds |
| 8 | Welding unit | bench | 2 m | yes | flame alone |
| 9 | Training unit | yard | class zone | yes | live work |
| 10 | Dock unit | dock row | electrical | yes | wet hands |

Ten envelopes, and the sentry's row contains the expansion's hardest line
written as policy: the machine that was built to make decisions about people
is allowed to watch a fence and structurally is not allowed to harm. The
never-column is exhaustive and reviewable, which is how a safety system stops
being a hope.

---

## 24. APPENDIX K — NAME AND HISTORY TABLE

| # | Unit | Name | First day | Years | Retirement |
|---|---|---|---|---|---|
| 1 | Loader | Tote | day 41 | 3 | parts to yard |
| 2 | Drone | Hover | day 63 | 4 | active |
| 3 | Scout | Far | day 88 | 2 | parts to watch |
| 4 | Medical | Hand | day 96 | 5 | active |
| 5 | Sentry | Sheet | never | 0 | dismantled |
| 6 | Lifting arm | Ox | day 140 | 3 | active |
| 7 | Cold hauler | Frost | day 160 | 2 | active |
| 8 | Crop tender | Sprig | day 190 | 2 | active |
| 9 | Pump runner | Well | day 210 | 1 | active |
| 10 | Welding unit | Spark | day 240 | 1 | active |

Ten histories, and the name column is the shelter's affection kept practical:
short names the crew can shout in a yard over a running machine. The sentry's
row says never, zero years, dismantled, and that is a complete and honorable
service record for a machine the shelter chose not to wake.

---

## 25. APPENDIX L — PARTS TABLE

| # | Part | Use | Source | Yield |
|---|---|---|---|---|
| 1 | Servo kit | arms, joints | retired units | 1 per arm |
| 2 | Bearing set | tracks, rotors | retired units | 2 per set |
| 3 | Logic board | control rebuilds | retired units | 1 per unit |
| 4 | Optic lens | sensors | retired units | 1-3 per unit |
| 5 | Machine lubricant | all joints | reagent, salvage | consumable |
| 6 | Charging cable | docks | salvage | rare |
| 7 | Dock plate | new docks | metal, workshop | craft |
| 8 | Tow cradle | recovery | metal, workshop | craft |
| 9 | Diagnostic meter | service | salvage | rare |
| 10 | Directive cards | authoring | press, card | craft |
| 11 | Nameplate | names | metal, paint | craft |
| 12 | Service log | cards | press, paper | craft |
| 13 | Static strap | board work | cloth, wire | craft |
| 14 | Spare tracks | heavy units | scrap, workshop | craft |
| 15 | Sensor kit | calibration | salvage | rare |
| 16 | Retire card | records | press, card | craft |

Sixteen parts with sources and yields, and the rule underneath the table is
that a retired unit is the shelter's richest single source of certain parts.
The retire card is on the list because the record of where the parts went is
part of the retirement, not an afterthought to it.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_machine_bay` | 3 | Chassis counted |
| `quest_machine_loader` | 4 | Loader woken |
| `quest_machine_lesson` | 4 | Fail-safe taught |
| `quest_machine_rota` | 4 | Work shared honestly |
| `quest_machine_dock_row` | 4 | Charging scheduled |
| `quest_machine_care` | 4 | Maintenance routine |
| `quest_machine_circles` | 4 | Malfunction occurs |
| `quest_machine_stop` | 5 | Protocol executed |
| `quest_machine_roofs` | 4 | Reduced envelope |
| `quest_machine_names` | 3 | Units named |
| `quest_machine_storm` | 4 | EMP day survived |
| `quest_machine_sheet` | 5 | Sentry decided |
| `quest_machine_retire` | 4 | Retirement complete |
| `quest_machine_apprentice` | 4 | Supervised wake |
| `quest_machine_what_they_carry` | 3 | Bay ordinary |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_machine_count` | 3 | Chassis counted |
| `quest_machine_materials` | 3 | Parts gathered |
| `quest_machine_skill` | 4 | Skill proven |
| `quest_machine_wake` | 4 | Unit woken |
| `quest_machine_first_task` | 3 | Task supervised |
| `quest_machine_directive_write` | 3 | Directive written |
| `quest_machine_directive_test` | 4 | Fail-safe tested |
| `quest_machine_directive_review` | 3 | Review passed |
| `quest_machine_directive_simple` | 3 | Short forms kept |
| `quest_machine_directive_limit` | 3 | Envelope added |
| `quest_machine_task_board` | 3 | Board kept |
| `quest_machine_shift` | 3 | Shift run |
| `quest_machine_override` | 3 | Override drilled |
| `quest_machine_retrain` | 4 | Worker retrained |
| `quest_machine_pair` | 3 | Pair assigned |
| `quest_machine_dock_build` | 4 | Dock built |
| `quest_machine_dock_load` | 3 | Load scheduled |
| `quest_machine_dock_night` | 3 | Night charging kept |
| `quest_machine_dock_repair` | 3 | Cable repaired |
| `quest_machine_dock_share` | 3 | Surge shared |
| `quest_machine_chassis` | 3 | Chassis repaired |
| `quest_machine_logic` | 4 | Logic serviced |
| `quest_machine_sensor` | 3 | Sensors calibrated |
| `quest_machine_lube` | 3 | Joints lubricated |
| `quest_machine_card` | 3 | Card current |
| `quest_machine_protocol` | 4 | Protocol drilled |
| `quest_machine_recover` | 4 | Unit recovered |
| `quest_machine_review` | 3 | Review written |
| `quest_machine_nameplate` | 3 | Plate fitted |
| `quest_machine_retire_parts` | 4 | Parts returned |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Gwen Sark** — lead. Maintained the bay for years without waking anything,
and has a list of every reason each chassis was left asleep. Believes a
machine is a promise about maintenance.

**Kester Garr** — programmer. Writes directives the length of a shopping list
and reads every stop condition aloud twice. Believes an instruction that
cannot fail safe is an instruction that cannot be given.

**Tia Mow** — chassis repair. Rebuilds hands and shoulders and can hear a bad
bearing through a floor. Believes the work is mostly listening.

**Wray Nesto** — docks. Keeps a load board and refuses to start a charge that
would dim the ward lights. Believes a dock is a promise to the grid.

**Jerro Hemi** — duty foreman. Counts work in hours and people in jobs, and
will not sign a task transfer without a retraining line. Believes dignity is
in the rota.

**Ulric Jarl** — logic safety. Wrote the never-column, including the sentry's
line, and checks the review board before coffee. Believes safety is a list you
keep, not a feeling you have.

**Hessa Quorra** — parts. Labels every servo that comes off a retired unit
with the unit's name so the part carries its history. Believes a store is a
kind of memory.

**Rist Yew** — apprentice. Seventeen, supervised, and proud of the first wake
he did not need correcting. Believes the charge lamp green is the best sound
in the shelter.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Loader Yard** — crates, a chalk line, and a lift that no longer hurts.
- **The Roof Runs** — a drone, a tether rule, and weather that counts.
- **The Scout Ridge** — sensors, distance, and a return-home directive.
- **The Dock Shed** — cables, a load board, and the green lights at night.
- **The Sentry Post** — a fence, a sensor, and a strict never-column.
- **The Scrap Grave** — chassis in the open and the parts that came back.
- **The Retire Yard** — a bench, a card, and parts going home to the store.
- **The Calibration Bench** — gauges, grades, and a sensor that had to be told
  the truth.
- **The Storm Bay** — the fleet stored through EMP season.
- **The Name Wall** — short names, first days, and years of work. 

---

## 30. APPENDIX Q — MACHINE DEPARTMENT CHARTER

| Clause | Promise |
|---|---|
| Wake | A unit is woken by skill, materials, and reason |
| Direct | Every directive fails safe and stops clean |
| Pair | Judgment work keeps a named person |
| Rotate | A displaced worker gets a plan before a machine gets a task |
| Charge | Docks request; the grid decides |
| Service | Maintenance happens before thresholds, not after |
| Calm | Malfunction is stopped and secured, never hunted |
| Name | Units are named by the crews who work with them |
| Envelope | The never-column is specific and reviewable |
| Retire | Every part has a destination and every unit a card |

The charter is the expansion's first-class design object, and its calm clause
is the one that changes the whole genre: in ASHFALL a machine that stops
working correctly is an engineering event with a checklist, not a monster.

---

## 32. APPENDIX R — WORKED MACHINE YEAR

**Month one.** Gwen dusts five chassis and writes the reasons each one slept.
Kester writes four directives and throws three away for having no stop
condition. Rist learns to read a charge table without being told twice.

**Month two.** The loader wakes. It consumes the listed materials, starts at
half charge, and lifts a pallet on its first supervised task in the yard. The
yard crew watches a week of back work disappear in an afternoon, and Jerro
counts the hours and schedules a conversation, because the rota has to answer
for the change.

**Month three.** The first directive fails safe: the loader stops with a load
half up because a person walked under it, and waits. The shelter's rule is
proved on a pallet of seed sacks rather than in a story, and the rule is now
real.

**Month four.** The rota meeting happens. One yard worker retrains to machine
supervision and one moves to the store, and the machine takes the lifting and
nobody takes the judgment. Jerro signs the transfer with the retraining line
filled in.

**Month five.** The dock row goes in: three bays, one mobile, one roof, one
ward, and a load board that keeps heavy charging at night. The corridor lights
stop flickering because Wray refuses to start a charge that would dim them.

**Month six.** Tia services every unit on schedule and proves logic integrity
twice the distance from the rogue line. The maintenance cards go on the wall
beside the charge board, and the bay starts to look like a department.

**Month seven.** The drone walks circles on a roof. Hover does not answer,
does not come down, and does not threaten anyone. The protocol runs exactly as
written: clear, stop, strand, secure, diagnose, and the fault is a servo and a
missed lube. The review names a schedule gap, not a curse.

**Month eight.** Hover returns to a reduced envelope with an escort and a
roof-only directive. It works a third more slowly for a month, and nobody
complains because the alternative is a shelter without a roof checker.

**Month nine.** The name wall goes up: Tote, Hover, Far, Hand, and the rest.
The names are short because they get shouted across a yard, and the crews chose
them without a ceremony that pretended the machines were anything but tools
with a history.

**Month ten.** The storm comes. The EMP season disables the fleet for a day
and a half, and the yard goes back to backs for an afternoon. Nobody is hurt,
nothing is lost, and the shelter learns that a machine department is a
supplement and not a spine.

**Month eleven.** The sentry question is decided. The sheet comes off, the
chassis is inspected, the never-column is read aloud, and the shelter
unbolts it for parts rather than waking a machine built to make decisions
about people. The card records the decision in one sentence.

**Month twelve.** A retired hauler's bearings go into the yard cart, its logic
board goes to the school as a teaching piece, and its nameplate goes on the
name wall. Rist wakes the training unit alone under supervision and does it
right, and Gwen writes one line in the bay log: bay ordinary.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Kester deletes a directive because it says make things safe, and he says: a
> machine cannot hear a feeling, and he rewrites it as stop if a person is
> within two meters, wait, and it works.

> The loader stops with the pallet in the air and waits, and the person who
> walked under it looks up and understands that the machine has a rule, and
> the rule is better than its eyes, and that is the whole success of the day.

> Tia hears the bad bearing through the floor before the meter finds it, and
> she puts the machine on the stand and says: you were due six days ago, and
> the schedule gets fixed, not the machine blamed.

> The nameplate goes on the wall beside a part from the same unit, and Hessa
> says the part carries the name so nobody has to pretend the machine was
> alive, and nobody has to pretend it did not matter either.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Wake without reason | idle chassis | write the reason |
| Bad directive | unit stops wrong | fail-safe review |
| Silent displacement | resentment | retraining plan |
| Dock overload | brownout | load board |
| Missed service | rogue roll | schedule and logic check |
| Panic at malfunction | injury risk | calm drilled protocol |
| Envelope absent | harm near unit | never-column review |
| Sentry woke | inherited weapon | sheet and dismantle |
| No cards | history lost | service cards |
| Parts unlabelled | memory lost | name every part |

Every failure in the table is a paperwork failure before it is a machine
failure, which is the expansion's quiet argument: the risks here are managed
with cards, schedules, and short sentences, not with heroics.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No autonomous lethal authority exists anywhere.
- [ ] The sentry cannot harm people by construction.
- [ ] No combat employment of machines exists.
- [ ] Docks request power; `PowerGridSystem` allocates.
- [ ] Repairs route through the workshop and metrology owners.
- [ ] Duty coordinates with `DutyRoster`; no second rota.
- [ ] Machines gain no needs and no personhood.
- [ ] Malfunction screens are calm, stepwise, and drillable.
- [ ] Retirement records parts destinations.
- [ ] Save additions are additive inside `robotics`.

---

## 36. APPENDIX V — GLOSSARY

- **Reactivation** — waking a dormant unit with skill and materials.
- **Directive** — a short instruction with a scope, a stop, and a fail-safe.
- **Envelope** — where a unit may work and what it may never do.
- **Duty board** — the rotas that pair machines with work and people.
- **Dock** — a charging station that requests power from the grid.
- **Logic integrity** — the machine value whose fall enables malfunction.
- **Rogue roll** — the live malfunction chance below 250 logic integrity.
- **Protocol** — stop, secure, recover, review, return.
- **Name** — the short crew-chosen label on a unit and its parts.
- **Retirement** — parts and a card, and no pretending either way.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `RoboticsSystem` | time, grid | units | power |
| `ReactivationSystem` | inventory | units | inventory |
| `DirectiveSystem` | reviews | directives | units |
| `MachineDutySystem` | roster | duty board | roster |
| `DockSystem` | grid headroom | docks | grid |
| `MachineCareSystem` | workshop | cards | units |
| `RogueResponseSystem` | units | protocol state | nothing |
| `SafetyEnvelopeSystem` | reviews | envelopes | directives |
| `MachineHistorySystem` | logs | histories | memorial |
| `PowerGridSystem` | nothing | nothing | nothing |
| `DutyRoster` | nothing | nothing | nothing |
| `ShelterWorkshopSystem` | jobs | nothing | nothing |
| `PrecisionMetrologySystem` | grades | nothing | nothing |
| `MedicalWardSystem` | assistance | nothing | nothing |
| `Inventory` | items | nothing | nothing |
| `StandingRecord` | records | records | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`robotics.json`** (extend) — existing `robots` rows with the same schema;
new rows must reference real items and real tasks.

**`robotics_directives.json`** — `directive_id`, `display_name`, `scope`,
`stop_condition`, `fail_safe`, `envelope_id`, `tags[]`.

**`robotics_tasks.json`** — `task_id`, `standard`, `load_class`, `hazards[]`,
`paired_human`, `displacement_note`, `tags[]`.

**`robotics_docks.json`** — `dock_id`, `bays[]`, `draw_watts`, `schedule`,
`cable_state`, `fire_note`, `tags[]`.

**`robotics_maintenance.json`** — `class_id`, `interval_days`, `work[]`,
`parts[]`, `threshold`, `card_field`, `tags[]`.

**`robotics_rogue_protocols.json`** — `stage_id`, `action`, `roles[]`,
`equipment[]`, `review_question`, `tags[]`.

**`robotics_safety_envelopes.json`** — `envelope_id`, `unit_role`,
`locations[]`, `proximity`, `paired_human`, `never_actions[]`, `tags[]`.

**`machine_logs.json`** — `log_id`, `unit_id`, `day`, `event`, `actor_id`,
`note`, `card_ref`, `tags[]`.

**`machine_history.json`** — `unit_id`, `name`, `first_day`, `service_years`,
`work[]`, `retirement`, `parts_destinations[]`, `tags[]`.

**`robotics_parts.json`** — `item_id`, `use`, `source`, `stock`, `salvage_yield`,
`note`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid item or task references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Units active | fleet size | Units |
| Wake reasons recorded | discipline | Logs |
| Directives with fail-safes | safety | Directives |
| Service cards current | upkeep | Care |
| Logic integrity minimum | rogue margin | Units |
| Docks healthy | grid care | Docks |
| Override drills run | readiness | Duty |
| Malfunctions reviewed | learning | Protocols |
| Retirements recorded | history | Retire |
| Parts destinations kept | memory | Parts |

Telemetry is diagnostic only; it never gates content and never ranks a unit,
a crew, or a foreman.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `robotics`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows maintenance preventing rogue rolls and docks holding the grid.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No lethal, horror, or personhood content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Can a unit be transferred to another settlement, and does its card go with it?
2. Who signs a wake decision when the lead and the safety officer disagree?
3. Is a retired unit's name ever reused, and is that honoring or erasing?
4. Can a malfunctioning unit refuse a stop instruction, and what then?
5. Do machines ever work at night, and how does that touch quiet hours?
6. Is there a limit on fleet size, and who enforces it?
7. What happens to a unit whose crew all died or left?
8. How much affection can the shelter show a machine before it becomes
   something else?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 16 The Rebuilt Body | Machine parts into prosthetics |
| 2 | 21 The Grid | Dock load and brownout avoidance |
| 2 | 18 The Underneath | Drones in shafts and tunnels |
| 3 | 23 The Alarm | Unit alarms and stop signals |
| 4 | 31 The Kiln | Cast parts and bearings |
| 5 | 34 The Long Road | Hauling convoys and recovery |
| 5 | 36 The Watch | Sentry sensor feed |
| 6 | 38 The Ward | Medical assistant limits |
| 6 | 40 The Wheel | Bearings, belts, and machine tools |
| 6 | 41 The Quiet | Night charging and quiet hours |
| 7 | 42 The Core | Dock power from the grid |
| 7 | 43 The Question | Logic studies and teaching boards |
| 7 | 44 The Outpost | Machines at remote sites |
| 7 | 46 The Long Change | Chassis found in the changing world |
| 8 | 47 The Brigade | Charging as an electrical fire load |
| 8 | 49 The Mirror | Machine-carried messages to stations |
| 8 | 50 The Vault | Logic board as a teaching exhibit |

Each hook is additive. The Machine can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Crew.** Machines work the heavy and dull loads, maintenance is routine,
and the yard's backs are intact at the end of a decade.

**The Well-Kept.** The fleet stays small and perfectly serviced, and nothing
ever rolls below 250 logic integrity, which is the quietest possible victory.

**The Wheel and the Hand.** Machine work is shared with people by rota, and
the warehouse crew retrains for the tasks that need judgment, and nobody is
replaced by a chassis.

**The Quiet Sheet.** The sentry is dismantled, its parts return to the store,
and its card records what it was built to do and what the shelter chose
instead, in one sentence that is read aloud once and never again.

**The Parts That Went On.** Every retired chassis lives on in tools, pumps,
and prosthetics, and each part carries a name so that the shelter's debt to its
machines is never entirely anonymized.

**Fade.** A machine bay at night, a charge lamp breathing green, a nameplate
polished by a hand that is not the machine's, and a loader sleeping where a
person once broke their back.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Killer robots | tone and ethics | never-column |
| Personhood | false depth | tools with names |
| Slave framing | cruelty | duty and retraining |
| Free power | authority break | grid requests |
| Horror rogue | genre break | calm protocol |
| Punish machines | cruelty | repair and review |
| Waking the sentry | inherited weapon | sheet and dismantle |
| No cards | lost history | service cards |
| Displacement silence | injustice | retraining plans |
| EMP as weapon | glorification | storm hazard only |

The list exists because machine stories default to three things ASHFALL will
not do: make the machines people, make them monsters, or make them slaves. The
expansion's rule is that machines are the best tools the shelter owns and the
first things it should learn to maintain.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Unit definitions | 10 | 2,500 |
| Directives | 30 | 5,000 |
| Tasks | 18 | 4,000 |
| Docks | 8 | 2,000 |
| Maintenance | 10 | 2,500 |
| Protocols | 8 | 2,500 |
| Envelopes | 10 | 3,000 |
| Logs | 24 | 4,000 |
| Histories | 10 | 3,000 |
| Parts | 16 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~62,000** |

---

## 46. APPENDIX AF — FIRST YEAR OF THE BAY

| Month | Focus | Milestone |
|---|---|---|
| 1 | Survey | reasons recorded |
| 2 | Loader | first lift |
| 3 | Fail-safe | stop proved |
| 4 | Rota | retraining plan |
| 5 | Docks | charge scheduled |
| 6 | Care | cards current |
| 7 | Malfunction | protocol proven |
| 8 | Return | reduced envelope |
| 9 | Names | wall up |
| 10 | Storm | EMP day survived |
| 11 | Sentry | dismantled |
| 12 | Retire | parts home |

A year from dust to a department, with the malfunction in the middle and the
decision about the last chassis at the end, because the sentry question should
not be answered in month two when the shelter still thinks machines are
wishes.

---

## 47. APPENDIX AG — OVERRIDE DRILL TABLE

| # | Drill | Who | Scenario | Pass |
|---|---|---|---|---|
| 1 | Big red stop | any worker | loader mid-lift | stops under 2 s |
| 2 | Cable pull | dock keeper | unit on charge | power cut clean |
| 3 | Path clear | yard crew | person under load | unit waits |
| 4 | Strand | pair | unit circles | enclosed, calm |
| 5 | Wards hold | nurse | arm near patient | unit waits |
| 6 | Rain land | roof pair | drone in rain | lands safely |
| 7 | Weather park | foreman | storm | all parked |
| 8 | Card drill | apprentice | card check | service found |

Eight drills, and the first one is the drill every crew member can perform
regardless of training: a person can stop any machine with a word or a button,
without asking permission, and the expansion treats that as the single most
important safety feature in the bay.

---

## 48. APPENDIX AH — STORM DAY LOG

| Hour | Event | Action | Result |
|---|---|---|---|
| 05:40 | warning | park order | fleet home |
| 06:10 | EMP front | cables pulled | docks safe |
| 06:30 | first surge | units off | no damage |
| 08:00 | yard opens | hand crews | slow day |
| 11:00 | surge two | grid dips | ward held |
| 14:00 | front passes | inspection | one fault |
| 15:30 | repair | logic board | unit ready |
| 17:00 | charge | staggered | grid stable |
| 18:00 | review | schedule change | next season set |

The storm log is included in full because the EMP day is the expansion's
proof: an afternoon of hand labor, a ward that never dimmed, one fault found
and fixed, and a schedule change before the next season. No machine was hurt,
no person was replaced, and the shelter was not afraid of its own equipment.

---

## 49. APPENDIX AI — RETIREMENT CARD TABLE

| # | Unit | Years | Work | Parts | Card |
|---|---|---|---|---|---|
| 1 | Sheet (sentry) | 0 | none | store shelf | kept |
| 2 | Tote (loader) | 3 | 4,100 lifts | bearings to cart | kept |
| 3 | Far (scout) | 2 | 380 sweeps | optic to watch | kept |
| 4 | Hover (drone) | 4 | 9,000 roof hours | rotors to spares | kept |
| 5 | Hand (medical) | 5 | 2,300 assists | arm to ward | kept |
| 6 | Ox (lift arm) | 3 | 1,900 lifts | servo to yard | kept |
| 7 | Frost (hauler) | 2 | 700 haul days | board to school | kept |
| 8 | Sprig (crop) | 2 | 600 bed days | servo to farm | kept |
| 9 | Well (pump) | 1 | 340 watch days | sensor to plant | kept |
| 10 | Spark (welder) | 1 | 290 bench days | optic to shop | kept |

Ten retirement cards with years, work, and destinations, and the sentry's row
is a complete record of a machine that did nothing because the shelter chose
that, which is the most honest thing a card can say. The parts column is where
the history goes back to work, and the card stays on the wall with the name.

---

## 50. APPENDIX AJ — BAY COVENANT

| Clause | Promise |
|---|---|
| Reason | No unit wakes without a written reason |
| Fail-safe | Every directive stops clean |
| Stop | Any person can stop any machine, no permission needed |
| Pair | Judgment keeps a person's name on it |
| Grid | Docks ask, the grid decides |
| Service | Maintenance before thresholds |
| Calm | Malfunction is stopped and secured, never feared |
| Never | Harm to people is structurally excluded |
| Name | Crews name the machines they work with |
| Card | Every unit and every part keeps its history |

The bay covenant is the expansion's first-class design object, and it is short
enough to hang on the bay wall beside the charge board. Its third clause is the
one the shelter drills most, because the best safety system in a machine bay
is a person who never has to wonder whether they are allowed to say stop.

---

## 51. APPENDIX AK — BAY SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Lead | Gwen | Tia | one full service |
| Programmer | Kester | Rist | one directive review |
| Repair | Tia | apprentice | one stand job |
| Docks | Wray | Rist | one load board |
| Duty | Jerro | foreman deputy | one rota |
| Safety | Ulric | Tia | one never-column read |
| Parts | Hessa | store keeper | one salvage week |
| Apprentice | Rist | next recruit | one supervised wake |

The succession table keeps the department from depending on one excellent
person. The safety row is the one handover that includes reading the entire
never-column aloud, because the successor must inherit the list, not just the
chair.

---

## 52. CLOSING STATEMENT

ASHFALL already models machines with real detail: chassis and logic integrity,
charge in watt-hours, labor draw, EMP downtime, skill-gated reactivation and
directive programming, a rogue state that only becomes possible when logic
integrity falls below 250, and five authored chassis waiting in a bay. What it
lacks is everything that turns a simulator into a department: safe directives
with fail-safes, a duty rota that protects people, docks that share the grid,
maintenance that keeps the rogue roll from ever being reached, a malfunction
protocol that is calm and rehearsed, names and cards, and a retirement bench
where parts go back to work with a record of whose shoulders they once saved.
The Machine adds the practice, keeps the tools as tools, and makes the bay
ordinary — which is the highest thing that can be said about a dangerous room.

> Wave 8 note: this plan is one of five Wave 8 expansion bibles (47–51). Each is
> self-contained; none requires another to ship. The shared Wave 8 index lives
> at `docs/expansions/wave8/WAVE8_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `RoboticsSystem` (`RobotDefinition`, `RobotUnitState`,
> `RoboticsSaveState`, `LoadCatalog`, `ReactivateRobot`,
> `ProgramDirective`, `ApplyEmpShock`, `TickLabor`, `RepairRobot`,
> `CaptureState`, `RestoreState`, events `OnRobotReactivated` /
> `OnDirectiveChanged` / `OnRobotEmpDisabled` / `OnRogueEventTriggered` /
> `OnStateChanged`, the rogue roll gated on `LogicIntegrity < 250`, and the
> only directive id `directive_idle`), `RoboticsSaveStore` under the `robotics`
> section, `RoboticsWorkshopPanel` commands, and `robotics.json` (4,431 B, five
> definitions: security sentry, heavy loader, utility maintenance drone, field
> scout, medical assistant bot; each with three compatible tasks and
> reactivation materials of `scrap_electronic`, `scrap_metal`, and
> `fuel_cell`).