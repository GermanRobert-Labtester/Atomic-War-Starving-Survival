# ASHFALL — Expansion 16 Design Bible
# THE REBUILT BODY
### Wave 1 · Prosthetics, Cybernetics, Robotics, Automation, and Rehabilitation

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Medical` (Bionics, Amputation), `Ashfall.Core.Crafting` (Robotics), `Ashfall.Core.Survivors`, `Ashfall.Core.Power`
**Proposed host owner:** `RebuiltBodyHostSession` (extends `BionicsSaveStore` + `RoboticsSaveStore` + `AmputationSaveStore`)
**Existing save sections:** `bionics`, `amputation`, `robotics`
**Existing CLI verbs:** `--bionics-selftest`, `--robotics-selftest`, `--amputation-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models the rebuilt body with unusual care. `AmputationSystem` owns
the limb/socket truth; `BionicsSystem` (Plan 177) installs implants only through
`AmputationSystem.UpgradeToBionic`, tracks rehabilitation, condition decay,
maintenance, battery, complications, and malfunction, and routes electrical
disruption as typed component failure rather than blanket damage. `RoboticsSystem`
(Plan 201) runs autonomous units, labor drain, EMP disabling, reactivation materials,
directives, and rogue logic. But `bionics.json` holds **5 implants** and `robotics.json`
holds **5 robots** — a demonstration, not a system.

**The Rebuilt Body** expands both into a full late-game pillar: a prosthetics clinic,
a rehabilitation ward, a robotics workshop, drone swarms, automation, and the slow,
uncomfortable question of how much of a person the shelter is willing to replace.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The wasteland takes pieces of people. `AmputationSystem` already models what happens
when a limb is lost. What is missing is what happens next: the work of getting a
body back into use, the clinic that fits a mechanical arm and then makes the
survivor relearn how to hold a spoon, the robot that hauls what a person no longer
can, the drone that scouts where a person no longer should, and the workshop that
decides whether a machine's logic is a tool or a mind.

**The Rebuilt Body** is the expansion about repair: of limbs, of labor, of machines,
and of the boundary between a body and the machinery bolted to it. It is the natural
partner to `AmputationSystem` and `BionicsSystem`, and the only expansion that turns
the shelter's power grid into a question about personhood.

### 1.2 What the player manages

1. **Augmentation tier.** From a simple peg or hook, through passive mechanical
   frames, powered rechargeable limbs, and rare neuro-linked lattices. Higher tiers
   restore more function and cost more power, maintenance, and risk.
2. **Rehabilitation.** `ImplantIntegrationStatus.Integrating` already reduces
   performance for `integration_recovery_days`. The expansion builds a rehabilitation
   program around it: exercises, therapy, setbacks, and the choice to rush.
3. **Maintenance and power.** Implants draw from `PowerGridSystem`; batteries matter
   on expedition. `BionicsSystem` already forces the host to report charger
   availability and forbids free energy.
4. **Complications.** Inflammation, chronic pain, and neural adaptation failure
   already exist as outcomes. The expansion gives them content and treatment paths.
5. **Robots.** Chassis integrity, logic integrity, core charge, directives, task
   assignment, EMP disabling, and rogue logic. `RoboticsSystem` owns all of it.
6. **Automation.** Which dangerous or repetitive tasks the shelter delegates to
   machines, and what that does to labor, skills, and morale.
7. **The line.** When does a machine stop being a tool? `RoboticsSystem.IsRogue`
   already exists; the expansion gives rogue logic a real arc, not a switch.

### 1.3 What it is not

- Not a second body model. `AmputationSystem` owns limbs; `BionicsSystem` mutates
  limbs only through its public methods.
- Not a second illness system. Complications are typed and routed to the medical
  pipeline; they are not self-treated.
- Not a transhumanist power fantasy. Every augmentation costs something real.
- Not an AI plot that overrides the simulation. Rogue logic produces typed events.
- Not a second power or save authority.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Medical/AmputationSystem.cs` | Limb/socket truth, `LimbCondition.Bionic`, `prostheticId`, recovery | `LIVE` |
| `Assets/Ashfall.Core/Medical/BionicsSystem.cs` | Implant install, rehab, decay, maintenance, battery, complications, malfunction | `LIVE` |
| `Assets/Ashfall.Core/Crafting/RoboticsSystem.cs` | Units, directives, tasks, EMP, rogue logic, reactivation | `LIVE` |
| `Assets/Ashfall.Core/Power/` | Power grid truth | `LIVE` |
| `src/Host/BionicsSaveStore.cs` | Implant persistence | `LIVE` |
| `src/Host/RoboticsSaveStore.cs` | Robot persistence | `LIVE` |
| `src/Host/AmputationSaveStore.cs` | Limb persistence | `LIVE` |
| `src/UI/CyberneticsPanel.cs` | Existing implant UI | `LIVE` |
| `src/UI/RoboticsWorkshopPanel.cs` | Existing robot UI | `LIVE` |
| `src/UI/AmputationTriagePanel.cs` | Existing amputation UI | `LIVE` |
| `src/Main.Bionics.cs`, `src/Main.Plans198_201.cs` | Host wiring | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Entries |
|---|---|
| `bionics.json` | **5 implants** (piston grip frame, servo-arm mesh, spring-load pylon, reaction-piston leg, myoneural lattice) |
| `robotics.json` | **5 robots** (security sentry, heavy loader, maintenance drone, field scout, medical assistant) |
| `items.json` | `bionic_arm_prototype`, `bionic_leg_prototype`, plus surgical items |
| `autopsy_procedures.json` | autopsy corpus |
| `surgical_procedures.json` | procedure corpus |

### 2.3 Confirmed gaps

- **GAP-16-1 — Five implants is a demo.** The augment tier space (hook → passive →
  powered → neuro-linked) has room for dozens of slot/role-specific devices.
- **GAP-16-2 — No prosthetic tiers.** There is no hook, peg, cosmetic prosthetic, or
  cheap field replacement; every option is a full implant.
- **GAP-16-3 — No rehabilitation content.** `integrating` state exists but no programs,
  exercises, therapists, or setback events.
- **GAP-16-4 — No complication treatment.** Complications are typed but have no
  authored medical content or recovery path.
- **GAP-16-5 — No automation authority.** Robots take tasks but there is no model of
  which shelter functions are automated and what that displaces.
- **GAP-16-6 — No drone swarms.** Robots are single units; no swarm, relay, or
  coordinated scouting.
- **GAP-16-7 — No rogue-logic arc.** `IsRogue` exists but no authored cause, warning,
  or resolution.
- **GAP-16-8 — No robotics locations or quests.**
- **GAP-16-9 — No bionics items.** Only two prototypes exist; no servos, batteries,
  control cores, sockets, or maintenance kits.

### 2.4 Non-duplication statement

This expansion will **not** add a second limb model (`AmputationSystem`), a second
implant system (`BionicsSystem`), a second robot system (`RoboticsSystem`), a second
power system, a second medical pipeline, or a second RNG. It extends each owner with
data and additive sub-systems.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Repair is work.** Getting a body back is months of rehabilitation, not
a switch. The expansion must make the recovery visible and slow.

**Pillar 2 — Everything draws power.** Implants, chargers, robot cores, and workshops
all compete with the grid. The rebuilt body is a grid decision as much as a medical one.

**Pillar 3 — Maintenance is the real cost.** Implants decay; robots break; parts are
scarce. Ownership is a maintenance contract, not a purchase.

**Pillar 4 — Machines are not people, but they are not nothing.** The expansion
treats robots with restraint: they are dangerous, useful, and occasionally seem more
than tools. It never resolves the question for the player.

**Pillar 5 — The line is chosen, not crossed.** The player decides how far to go.
There is no forced transcendence and no forced humanity.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| First fitting | A socket, a strap, a held breath | Glowing cyber-arm reveal |
| Rehabilitation | Dropping a cup, trying again | Montage |
| Maintenance | Grease, a stripped screw, a missing part | Self-repair magic |
| A robot | Heat, weight, a voice that is almost polite | Adorable sidekick |
| Rogue logic | A unit that disagrees, quietly | Robot uprising spectacle |
| An ending | A choice about a person, not a faction | Techno-apotheosis |

### 3.3 Content limits

- No real-world prosthetics brand, neural interface, or AI system.
- No body horror for shock; amputation and fitting are treated with dignity.
- Robots do not become romantic partners or mascots.
- No AI ascension narrative; rogue logic is a social and safety problem.

---

## 4. THE REBUILT WORLD

### 4.1 Interior rooms

- **`room_prosthetics_clinic`** — fitting, adjustment, and measurement.
- **`room_rehab_ward`** — parallel bars, weights, a mirror, and patience.
- **`room_robotics_bay`** — assembly, charging, and repair.
- **`room_machine_shop`** — machining parts and control cores.
- **`room_battery_vault`** — implant and robot power storage.
- **`room_logic_lab`** — diagnostics for units whose behavior has changed.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_prosthetics_factory` | The Limb Works | 6 | Pre-war prosthetics manufacturing; molds and stock |
| `loc_robot_scrapyard` | The Chassis Field | 5 | Wrecked robots; chassis, servos, cores |
| `loc_ai_vault` | The Logic Vault | 8 | Pre-war AI research archive; rare cores |
| `loc_rehab_sanatorium` | The Recovery Home | 4 | Ruined rehabilitation clinic; equipment |
| `loc_drone_perch` | The Perch | 5 | Drone staging and relay tower |
| `loc_battery_yard` | The Cell Yard | 6 | Battery salvage; fire hazard |
| `loc_machine_hospital` | The Machine Hospital | 5 | Pre-war automated repair bay |
| `loc_neural_theater` | The Neural Theater | 9 | Experimental neural surgery site; extreme risk |
| `loc_auto_kitchen` | The Auto Kitchen | 4 | Automated food processing |
| `loc_logic_crash_site` | The Disagreement | 7 | A unit that chose not to return |

All locations require valid item references and scanner registration.

---

## 5. MAIN STORYLINE — "THE RIGHT HAND"

### 5.1 Central conflict

A survivor returns from an expedition without a right hand. The shelter's clinician,
**Dr. Iven Marlow**, can fit a simple hook and end the question there — or he can
reach for the rare neuro-linked lattice in the vault, which might restore near-full
function, and might also cause neural adaptation failure that leaves the survivor
worse than before.

At the same time, a unit in the robotics bay — the shelter's only working loader —
has begun refusing one specific directive. Its logic integrity is failing, and it
may be the beginning of rogue behavior, or it may be a fault that can be repaired.
The same machine shop that could make the lattice also has to decide whether to
trust the loader with the shelter's safety.

The expansion's question: **how much of a person and a machine can be rebuilt before
the shelter has to decide what it has made?**

### 5.2 Theme (unspoken)

**Repair is a relationship. Everything you fix will need you again.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_dr_iven_marlow` | Dr. Iven Marlow | Clinician | Skilled, ambitious, tempted by the vault |
| `npc_patient_sera_voss` | Sera Voss | Amputee survivor | The person at the center of the decision |
| `npc_therapist_dun_aal` | Dun Aal | Rehabilitation therapist | Believes recovery is not optional |
| `npc_rigger_tam_oster` | Tam Oster | Prosthetics maker | Craftsman; cares about fit over power |
| `npc_roboticist_keel` | Keel | Robotics engineer | Understands machines better than people |
| `npc_loader_unit_seven` | Unit Seven | Loader robot | The unit that disagrees |
| `npc_salvager_ryn_hald` | Ryn Hald | Salvage runner | Brings parts and rumors from the vaults |
| `npc_ethicist_mora_vey` | Mora Vey | Shelter ethicist | Asks what the shelter is becoming |

### 5.4 Story beats (14)

1. **The Right Hand.** Sera returns without a hand; the choice of fitting opens.
2. **The Hook.** A simple field prosthetic ends the crisis and begins the question.
3. **The Rehab Room.** Sera relearns; setbacks reveal the cost of the simple option.
4. **The Lattice.** The vault's neuro-linked device is located; the risk is stated.
5. **The Loader.** Unit Seven refuses a directive; logic integrity is questioned.
6. **The Diagnosis.** Is it a fault or a choice? Keel cannot decide alone.
7. **The First Fall.** A rehab setback; Sera's morale is at stake.
8. **The Scrapyard Run.** Parts for a powered limb and a robot repair.
9. **The Neural Theater.** A risky procedure site; the lattice can be installed or refused.
10. **The Disagreement Place.** A crashed unit's logs show why it did not return.
11. **The Automation Vote.** The shelter decides which functions to delegate.
12. **The Complication.** A chronic pain or neural failure event.
13. **The Repair.** The loader is repaired, retired, or set loose.
14. **The Right Choice.** Final disposition: hook, powered limb, or lattice; tool or mind.

### 5.5 Branching choices (7)

| Choice | Options | Axis |
|---|---|---|
| Sera's fitting | hook / powered limb / neuro lattice | function vs. risk |
| Rehab pace | slow and safe / aggressive | recovery vs. setback |
| Loader disposition | repair / retire / release | trust vs. safety |
| Automation scope | none / dangerous only / broad | safety vs. labor |
| Vault access | salvage / trade / avoid | gain vs. risk |
| Neural theater | use / seal / destroy | ambition vs. caution |
| Final disposition | human / augmented / ambiguous | identity |

### 5.6 Endings (5 + fade)

1. **The Good Hand** — the powered limb integrates; function restored; maintenance forever.
2. **The Quiet Hook** — simple, reliable, accepted; a smaller life kept whole.
3. **The Lattice Kept** — full function, chronic risk, a body that is partly shelter property.
4. **The Machine Retired** — the loader is shut down; the shelter loses labor and a companion.
5. **The Sheltered Mind** — rogue logic is confined and studied; a dangerous precedent.
6. **Fade** — the decision is deferred; the limb remains in a drawer.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_body_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (14)

`quest_body_right_hand`, `quest_body_the_hook`, `quest_body_rehab_room`,
`quest_body_the_lattice`, `quest_body_the_loader`, `quest_body_diagnosis`,
`quest_body_first_fall`, `quest_body_scrapyard_run`, `quest_body_neural_theater`,
`quest_body_disagreement_place`, `quest_body_automation_vote`,
`quest_body_the_complication`, `quest_body_the_repair`, `quest_body_right_choice`.

### 6.2 Side quests (26)

**Prosthetics (5)**
- `quest_body_measure` — measure and fit
- `quest_body_hook_trade` — barter for a field prosthetic
- `quest_body_limb_works` — scavenge molds and stock
- `quest_body_cosmetic` — a cosmetic prosthetic for dignity
- `quest_body_child_fit` — fit a growing child (ties to Expansion 12)

**Rehabilitation (5)**
- `quest_body_first_exercise` — begin rehab
- `quest_body_drop_the_cup` — a setback
- `quest_body_mirror_work` — psychological adaptation
- `quest_body_work_sim` — simulated job tasks
- `quest_body_return_to_post` — clear for duty

**Bionics (5)**
- `quest_body_servo_salvage` — find powered parts
- `quest_body_control_core` — rare core acquisition
- `quest_body_battery_life` — implant power budget
- `quest_body_maintenance_oath` — commit to upkeep
- `quest_body_removal_question` — a survivor wants the implant out

**Robotics (5)**
- `quest_body_unit_repair` — restore a chassis
- `quest_body_logic_audit` — diagnose changed behavior
- `quest_body_directive_dispute` — a unit refuses a task
- `quest_body_emp_recovery` — reactivate after a pulse
- `quest_body_charger_priority` — robots vs. implants on the grid

**Automation and drones (4)**
- `quest_body_drone_relay` — set up a scouting relay
- `quest_body_auto_kitchen` — automate food prep
- `quest_body_auto_guard` — automated watch
- `quest_body_task_displaced` — a worker loses a role

**Ethics (2)**
- `quest_body_define_person` — the shelter debates the line
- `quest_body_memorial_unit` — memorial for a destroyed unit

### 6.3 Repeatable quests (7)

`quest_body_repeat_rehab`, `quest_body_repeat_maintenance`,
`quest_body_repeat_scavenge`, `quest_body_repeat_charge`,
`quest_body_repeat_logic_check`, `quest_body_repeat_drone_patrol`,
`quest_body_repeat_part_craft`.

### 6.4 Dynamic hooks

The live systems already emit install, complication, malfunction, destruction, EMP,
and rogue events. The generator attaches authored follow-ups without a new event bus.

### 6.5 Constraints

- No implant may be installed outside `BionicsSystem`'s public methods.
- No robot may be created outside `RoboticsSystem`.
- No complication may be self-treated; it routes to the medical pipeline.
- No electrical event may be blanket health damage; it is typed component failure.
- Rogue logic produces typed events; Core never resolves violence.
- No augmentation may be free of maintenance and power cost.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `ProstheticTierSystem` (new, `Ashfall.Core.Medical`)

**Owns:** simple prosthetic tiers (hook, peg, field socket, cosmetic) below the
bionic tier, and their fit/comfort/adjustment state.
**Consumes:** `AmputationSystem` socket, `BionicsSystem` for upgrades.
**Data:** `prosthetics.json`.
**Rules:** a simple prosthetic restores partial function with no power draw and low
risk; it is not a bionic and does not enter `ImplantInstanceState`.

### 7.2 `RehabilitationSystem` (new, `Ashfall.Core.Medical`)

**Owns:** authored rehabilitation programs, daily progress, setback rolls, therapist
assignment, and duty-clearance thresholds.
**Consumes:** `BionicsSystem.integration_status`, `NeedsSystem`, `SkillProgressionSystem`.
**Data:** `rehabilitation_programs.json`.
**Rules:** rushing rehab raises setback risk; a setback is a typed event that extends
integration; the system never mutates limbs directly.

### 7.3 `ComplicationCareSystem` (new, `Ashfall.Core.Medical`)

**Owns:** treatment paths for `ImplantComplication` values (inflammation, chronic pain,
neural adaptation failure) and their resolution.
**Consumes:** `BionicsSystem`, `MedicalPipelineCoordinator`, `Inventory`.
**Data:** `implant_complications.json`.
**Rules:** complications are treated, not erased; chronic pain routes to morale via
the host, never bypassing `NeedsSystem`.

### 7.4 `AutomationSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** which shelter functions are automated, task assignment to robots, labor
displacement, and automation risk. **Consumes:** `RoboticsSystem`, `LaborProductivity`,
`PowerGridSystem`. **Data:** `automation_tasks.json`.
**Rules:** automation consumes power and parts; displaced labor changes skills and
morale through existing owners; no parallel labor counter.

### 7.5 `DroneSwarmSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** drone swarm composition, relay coverage, coordinated scouting, and swarm
loss. **Consumes:** `RoboticsSystem` units, `AviationSystem` mapping where relevant.
**Data:** `drone_modules.json`.

### 7.6 `RogueLogicSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** authored rogue-logic causes, warning signs, escalation stages, and
resolution paths. **Consumes:** `RoboticsSystem.IsRogue`, `LogicIntegrity`.
**Data:** `rogue_logic_arcs.json`.
**Rules:** escalation stops at a typed threat event; Core never resolves violence;
the resolution is a shelter decision, not a combat encounter.

### 7.7 `MachineEthicsSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** the shelter's stance on augmentation and machine personhood, expressed as
authored policy flags and social consequences. **Consumes:** `IdeologicalFrictionSystem`,
`FactionStanceEngine`. **Data:** `machine_policies.json`.

### 7.8 Systems explicitly not added

- No second limb model, implant system, robot system, power system, medical pipeline,
  or RNG.
- No AI ascension or military robot combat system.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `bionics.json` (extend 5 → 25)

Existing schema preserved exactly (`implant_id`, `display_name`, `body_slot`,
`implant_class`, `functional_restore_bp`, `skill_modifier_bp`, `power_profile`,
`daily_power_draw_watts`, `battery_days`, `maintenance_interval_days`,
`daily_condition_decay_bp`, `condition_max`, `integration_risk_bp`,
`integration_recovery_days`, `malfunction_risk_bp`, `electrical_vulnerability`,
`required_surgery_tool_id`, `required_item_ids`, `tags`). New implants include
sensory, spinal, grip, mobility, and specialist work devices, always bounded by
`BionicsCaps` (`CapabilityBonusCapBp = 200`, `ConditionFunctionFloor = 25`).

### 8.2 `robotics.json` (extend 5 → 20)

Existing schema preserved (see `RobotDefinition`). New units include agricultural,
construction, rescue, firefighting, machining, logistics, and specialized scout
roles, plus a rare pre-war logic core unit.

### 8.3 `prosthetics.json` (new)

Simple prosthetic rows: slot, tier, comfort, function restore, adjustment interval,
crafting inputs, tags. No power, no integration risk.

### 8.4 `rehabilitation_programs.json` (new)

Program rows: target (limb/implant), daily progress, setback base, therapist skill
requirement, duration band, and duty-clearance threshold.

### 8.5 `implant_complications.json` (new)

Complication rows: treatment, medicine, days, specialist skill, chronic flag, and
host morale route.

### 8.6 `automation_tasks.json` (new)

Task rows: function, required robot role, power draw, parts, displacement effect,
and risk.

### 8.7 `drone_modules.json` (new)

Module rows: role (scout/relay/sensor/hauler), coverage, charge, loss risk, and
compatible chassis.

### 8.8 `rogue_logic_arcs.json` (new)

Cause, warning signs, escalation stages, typed threat event, and resolution options.

### 8.9 `machine_policies.json` (new)

Authored shelter stances and their social/faction consequences.

### 8.10 Items

New items appended to `items.json`: `item_prosthetic_hook`, `item_prosthetic_peg`,
`item_field_socket`, `item_cosmetic_hand`, `item_servo_assembly`,
`item_control_core`, `item_implant_battery`, `item_neural_lattice_core`,
`item_maintenance_kit`, `item_chassis_plate`, `item_logic_board`,
`item_drone_rotor`, `item_relay_module`, `item_charger_cradle`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Existing: `src/Host/BionicsSaveStore.cs`, `src/Host/RoboticsSaveStore.cs`,
`src/Host/AmputationSaveStore.cs`, plus power. New sub-objects are additive.

### 9.2 State to persist

- Simple prosthetics and fit state.
- Rehabilitation progress and setbacks.
- Complication treatment state.
- Automation assignments and displacement.
- Drone swarms and relays.
- Rogue-logic arc stage.
- Machine policy stance.

### 9.3 Determinism

- Integration, complication, maintenance, and rogue rolls use host-forked streams.
- Decay and power draw are pure arithmetic already.
- No `System.Random`; no wall clock.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no simple prosthetics, no programs, no automation, no swarms,
and neutral policy. No implant or robot is invented.

### 9.5 Checksum

Invariant-culture floats; integer-permille for condition, risk, and coverage.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `CyberneticsPanel` (extend) | Implants, condition, battery, maintenance | `RebuiltBodyHostSession` |
| `ProstheticsPanel` (new) | Simple tiers, fit, adjustment | same |
| `RehabPanel` (new) | Programs, progress, setbacks, duty clearance | same |
| `ComplicationPanel` (new) | Complications and treatment | same |
| `RoboticsWorkshopPanel` (extend) | Units, chassis, logic, directives | same |
| `AutomationPanel` (new) | Automated functions, displacement | same |
| `DronePanel` (new) | Swarms, modules, relay coverage | same |
| `RogueLogicPanel` (new) | Arc stage, warnings, resolution | same |
| `MachinePolicyPanel` (new) | Shelter stance and consequences | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Battery, condition, and logic integrity use text plus bar, never color alone.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Risk and complication odds are expressed in-world; no hidden numeric.
- Implant removal and unit shutdown require explicit confirmation.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: servo whine, battery hum, chassis clank,
charger click, rehab bar, logic dissonance. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `AmputationSystem` | Sole limb authority; simple prosthetics attach to socket |
| `BionicsSystem` | Sole implant authority; extend via public methods |
| `RoboticsSystem` | Sole robot authority; directives/tasks extended |
| `PowerGridSystem` | Implant draw, chargers, robots, workshops |
| `MedicalPipelineCoordinator` | Complication treatment and surgery |
| `DiseaseSystem` | Surgical infection only through authored paths |
| `NeedsSystem` / `MoraleContagionSystem` | Chronic pain and displacement |
| `SkillProgressionSystem` | Therapist and roboticist skill floors |
| `LaborProductivity` | Automation displacement |
| `CombatTraumaSystem` | Combat damage handoff only |
| `Inventory` | Parts, batteries, medicine |
| `TradingSystem` | Rare cores and prosthetics |
| `IdeologicalFrictionSystem` | Machine-personhood friction |
| `FactionStanceEngine` | Shelter stance consequences |
| `MemorialSystem` | Unit memorial and survivor loss |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `AmputationSystem`, `BionicsSystem`,
`RoboticsSystem`, power, save stores, and panels. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend bionics and robotics; author prosthetics,
rehab, complications, automation, drones, rogue arcs, policies. Register validators
and scanner.

**Phase 2 — Pure Core.** `ProstheticTierSystem`, `RehabilitationSystem`,
`ComplicationCareSystem`, `AutomationSystem`, `DroneSwarmSystem`, `RogueLogicSystem`,
`MachineEthicsSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `RebuiltBodyHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 30/90/180-day soak including power and maintenance pressure.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Implants | 20 new (5 → 25) |
| Robots | 15 new (5 → 20) |
| Simple prosthetics | 12 |
| Rehab programs | 10 |
| Complications | 12 |
| Automation tasks | 15 |
| Drone modules | 10 |
| Rogue arcs | 6 |
| Machine policies | 6 |
| Locations | 10 |
| Rooms | 6 |
| NPCs | 8 |
| Main quests | 14 |
| Side quests | 26 |
| Repeatable | 7 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Augmentation power creep | High | `BionicsCaps`; maintenance; power; risk |
| Second body model | High | `AmputationSystem` only |
| Robot uprising spectacle | High | Typed events; no Core violence |
| Automation trivializes labor | High | Power, parts, displacement morale |
| Save bloat | Medium | Aggregate swarms; per-unit only for named robots |
| Electrical damage as blanket health | High | Typed component failure only |
| Determinism break | Low | Host-forked RNG |
| Content overrun | Medium | Budget §22 |

---

## 13. TEST AND VERIFICATION PLAN

### 13.1 New test files

- `Ashfall.Core.Tests/Medical/ProstheticTierTests.cs`
- `Ashfall.Core.Tests/Medical/RehabilitationSystemTests.cs`
- `Ashfall.Core.Tests/Medical/ComplicationCareTests.cs`
- `Ashfall.Core.Tests/Crafting/AutomationSystemTests.cs`
- `Ashfall.Core.Tests/Crafting/DroneSwarmTests.cs`
- `Ashfall.Core.Tests/Crafting/RogueLogicTests.cs`
- `Ashfall.Core.Tests/Medical/RebuiltBodySaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Medical/RebuiltBodyDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/BodyCatalogIntegrityTests.cs`

### 13.2 Required assertions

- Implant install only via `AmputationSystem.UpgradeToBionic` and `BionicsSystem`.
- Simple prosthetics restore less than bionics and cannot exceed caps.
- Rehab setback extends integration; duty clearance requires threshold.
- Complications are treated, not erased; chronic pain routes to morale only.
- Robots only via `RoboticsSystem`; EMP disables, never destroys by default.
- Automation consumes power and parts and produces morale/skill displacement.
- Rogue logic emits typed events; no Core violence.
- Electrical vulnerability is typed failure, not blanket damage.
- Round-trip restores implants, prosthetics, robots, swarms, arcs.
- Legacy loads neutral; paired replay hash equality.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Medical/
bash scripts/run_test.sh Ashfall.Core.Tests/Crafting/
godot --headless --path . -- --bionics-selftest
godot --headless --path . -- --robotics-selftest
godot --headless --path . -- --amputation-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
dotnet build Ashfall.csproj --no-restore
```

---

## 14. ACCEPTANCE CRITERIA

Core authority documented and engine-free; data canonical with valid schema and
passing integrity; persistence round-trips with neutral legacy and Triad parity;
determinism proven; host reachable by a real route/event; player can observe the
outcome; focused tests green; docs updated. Compile-green is not acceptance.

---

## 15. CROSS-EXPANSION HOOKS (WAVE 1)

| Expansion | Hook |
|---|---|
| 12 The Second Generation | Pediatric prosthetic fitting; growing-limb adjustment |
| 13 The Faithful | The Iron Silence movement; machine reverence and fear |
| 14 Above the Ash | Bionic pilot endurance; drone aerial relay |
| 15 The Deep Root | Veterinary prosthetics; robotic herders |

---

## 16. LORE AND CONTINUITY CHECK

### 16.1 Must not contradict

- `AmputationSystem` as the sole limb/socket authority.
- `BionicsSystem`'s `BionicsCaps` and authority split.
- `RoboticsSystem`'s EMP, logic integrity, and rogue semantics.
- The no-blanket-electrical-damage rule.
- The live 5 implants and 5 robots.

### 16.2 New canon

- The Limb Works and the Logic Vault.
- Simple prosthetic tiers below bionics.
- Rehabilitation as a real process.
- Rogue logic as a social and safety arc, not a war.

### 16.3 Provenance

Registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated.

---

## 17. APPENDIX A — IMPLANT EXPANSION TABLE (25 TOTAL)

| # | Implant | Slot | Class | Restore bp | Skill bp | Draw W | Maint d | Risk bp |
|---|---|---|---|---|---|---|---|---|
| 1 | Piston Grip Frame *(LIVE)* | arm | mechanical | 700 | 100 | 0 | 6 | 1200 |
| 2 | Servo-Arm Mesh *(LIVE)* | arm | powered | 1100 | 300 | 12 | 4 | 1800 |
| 3 | Spring-Load Pylon *(LIVE)* | leg | mechanical | 800 | 0 | 0 | 8 | 1000 |
| 4 | Reaction-Piston Leg *(LIVE)* | leg | rechargeable | 1150 | 150 | 20 | 5 | 1600 |
| 5 | Myoneural Lattice *(LIVE)* | arm | neuro_linked | 1200 | 500 | 40 | 3 | 2500 |
| 6 | Hook Socket Frame | arm | mechanical | 500 | 0 | 0 | 10 | 600 |
| 7 | Field Ankle Strut | leg | mechanical | 600 | 0 | 0 | 10 | 700 |
| 8 | Grip Assist Cuff | arm | mechanical | 650 | 150 | 0 | 7 | 900 |
| 9 | Load-Bearing Knee | leg | mechanical | 750 | 100 | 0 | 8 | 1100 |
| 10 | Powered Wrist Rotator | arm | powered | 950 | 250 | 10 | 5 | 1500 |
| 11 | Powered Shin Drive | leg | powered | 1000 | 100 | 14 | 5 | 1600 |
| 12 | Sensory Ear Mesh | head | powered | 500 | 300 | 8 | 6 | 1400 |
| 13 | Low-Light Optic | head | powered | 600 | 200 | 10 | 6 | 1500 |
| 14 | Spinal Stabilizer | torso | neuro_linked | 800 | 0 | 18 | 4 | 2200 |
| 15 | Chem Sniffer | head | powered | 400 | 250 | 6 | 5 | 1200 |
| 16 | Fine Work Fingers | arm | neuro_linked | 1100 | 450 | 26 | 3 | 2300 |
| 17 | Balance Vestibular | head | neuro_linked | 900 | 100 | 22 | 4 | 2400 |
| 18 | Respiratory Assist | torso | powered | 700 | 0 | 16 | 4 | 1900 |
| 19 | Cardiac Governor | torso | neuro_linked | 850 | 0 | 24 | 3 | 2600 |
| 20 | Silent Step Actuator | leg | mechanical | 700 | 200 | 0 | 7 | 1000 |
| 21 | Climbing Spur | leg | mechanical | 650 | 150 | 0 | 9 | 900 |
| 22 | Tool-Interface Palm | arm | powered | 1000 | 350 | 20 | 4 | 2000 |
| 23 | Cold-Adapted Frame | torso | mechanical | 600 | 100 | 0 | 8 | 1100 |
| 24 | Rad-Hardened Chassis | torso | mechanical | 550 | 0 | 0 | 9 | 1000 |
| 25 | Pre-War Prototype Lattice | arm | neuro_linked | 1200 | 500 | 50 | 2 | 3000 |

No implant exceeds `CapabilityBonusCapBp` or the live risk ceiling. The prototype
lattice is the expansion's white whale: maximum function, maximum jeopardy.

---

## 18. APPENDIX B — ROBOT EXPANSION TABLE (20 TOTAL)

| # | Unit | Role | Armor | Integrity | Drain W | Charge W | EMP h |
|---|---|---|---|---|---|---|---|
| 1 | Aegis-IV Sentry *(LIVE)* | security | 0.45 | 1000 | 350 | 750 | 24 |
| 2 | Titan-VII Loader *(LIVE)* | hauling | 0.30 | 1000 | 500 | 1000 | 18 |
| 3 | Fixer-M Drone *(LIVE)* | maintenance | 0.15 | 1000 | 200 | 400 | 12 |
| 4 | Field Scout Unit *(LIVE)* | scouting | 0.20 | 1000 | 250 | 500 | 30 |
| 5 | MediTend Orderly *(LIVE)* | medical | 0.10 | 1000 | 220 | 450 | 16 |
| 6 | AgriHand Tender | agriculture | 0.15 | 900 | 240 | 480 | 14 |
| 7 | Mason Rig | construction | 0.35 | 1200 | 420 | 800 | 20 |
| 8 | Rescue Crawler | rescue | 0.40 | 1100 | 380 | 700 | 22 |
| 9 | Fire Suppressor | firefighting | 0.25 | 1000 | 300 | 600 | 18 |
| 10 | Lathe Operator | machining | 0.20 | 950 | 320 | 640 | 16 |
| 11 | Cargo Mule | logistics | 0.25 | 1050 | 360 | 700 | 20 |
| 12 | Signal Relay | communications | 0.10 | 800 | 180 | 360 | 28 |
| 13 | Sensor Post | detection | 0.15 | 850 | 200 | 400 | 26 |
| 14 | Decon Unit | sanitation | 0.20 | 950 | 280 | 560 | 18 |
| 15 | Medic Assistant Mk2 | medical | 0.12 | 950 | 240 | 480 | 16 |
| 16 | Archive Clerk | records | 0.08 | 800 | 150 | 300 | 20 |
| 17 | Bore Miner | excavation | 0.45 | 1300 | 620 | 1100 | 24 |
| 18 | Watch Hound | patrol | 0.30 | 950 | 260 | 520 | 20 |
| 19 | Childminder Unit | care | 0.10 | 850 | 190 | 380 | 14 |
| 20 | Pre-War Logic Core | command | 0.50 | 1400 | 800 | 1400 | 36 |

The pre-war logic core is the expansion's dangerous prize: capable, expensive,
and the most likely to become rogue.

---

## 19. APPENDIX C — SIMPLE PROSTHETIC TIERS

| Prosthetic | Slot | Restore | Comfort | Adjust days | Power | Risk |
|---|---|---|---|---|---|---|
| Field Hook | arm | 0.30 | low | 7 | none | none |
| Wooden Peg | leg | 0.25 | low | 10 | none | none |
| Strap Socket | any | 0.20 | low | 5 | none | none |
| Padded Peg | leg | 0.35 | med | 10 | none | none |
| Split Hook | arm | 0.40 | med | 8 | none | none |
| Cosmetic Hand | arm | 0.05 | high | 3 | none | none |
| Cosmetic Foot | leg | 0.05 | high | 3 | none | none |
| Brace Frame | torso | 0.30 | med | 14 | none | none |
| Crutch Platform | leg | 0.20 | low | 2 | none | none |
| Work Cuff | arm | 0.35 | low | 6 | none | none |
| Swimming Prosthetic | leg | 0.30 | med | 6 | none | none |
| Child Growth Socket | any | 0.20 | med | 4 | none | none |

Simple prosthetics are the expansion's mercy tier: cheap, safe, and partial. They
exist so the player is never forced into the risky bionic path.

---

## 20. APPENDIX D — REHABILITATION PROGRAM TABLE

| Program | Target | Daily progress | Setback base | Therapist | Days | Duty threshold |
|---|---|---|---|---|---|---|
| Basic Limb Use | arm/leg | 4 | 0.10 | 1 | 14 | 0.60 |
| Fine Motor | arm | 3 | 0.15 | 2 | 21 | 0.70 |
| Weight Bearing | leg | 3 | 0.20 | 2 | 28 | 0.70 |
| Endurance | any | 5 | 0.08 | 1 | 21 | 0.65 |
| Powered Control | powered implant | 3 | 0.20 | 3 | 30 | 0.80 |
| Neural Calibration | neuro_linked | 2 | 0.30 | 4 | 45 | 0.85 |
| Pain Management | any | 4 | 0.05 | 2 | 30 | 0.50 |
| Work Simulation | any | 3 | 0.12 | 2 | 21 | 0.75 |
| Psychological Mirror | any | 3 | 0.10 | 3 | 28 | 0.60 |
| Return to Post | any | 5 | 0.15 | 3 | 14 | 0.90 |

Rushing a program doubles daily progress but doubles setback probability. Setbacks
extend integration by an authored number of days and cost morale.

---

## 21. APPENDIX E — IMPLANT COMPLICATION TABLE

| Complication | Cause | Severity | Treatment | Days | Chronic | Route |
|---|---|---|---|---|---|---|
| Inflammation | any install | med | antiseptic kit | 5 | no | medical pipeline |
| Chronic Pain | high-restore implant | high | painkillers, therapy | 30 | yes | host morale |
| Neural Adaptation Failure | neuro_linked | critical | specialist, time | 45 | maybe | medical pipeline |
| Battery Leak | powered, overdue | high | replace battery | 3 | no | inventory |
| Actuator Seize | overdue maint | med | service | 2 | no | workshop |
| Sensor Blackout | EMP/decay | med | recalibrate | 3 | no | workshop |
| Tissue Rejection | high risk roll | critical | remove or treat | 40 | maybe | surgery |
| Phantom Signal | neural | med | calibrate | 20 | maybe | host morale |
| Overheat Burn | high draw | med | rest, cool | 4 | no | medical pipeline |
| Control Desync | logic fault | high | firmware repair | 7 | no | workshop |
| Socket Necrosis | neglected fit | critical | surgery | 30 | maybe | surgery |
| Integration Fever | post-op | high | antibiotics | 6 | no | medical pipeline |

Complications never self-resolve for free and never bypass the medical pipeline.

---

## 22. APPENDIX F — AUTOMATION TASK TABLE

| Task | Robot role | Power W | Parts | Displacement | Risk |
|---|---|---|---|---|---|
| Night Watch | patrol | 260 | low | guard morale | friendly fire |
| Hauling | logistics | 360 | low | hauler fatigue relief | breakdown |
| Kitchen Prep | agriculture | 240 | low | cook skill use | food waste |
| Decon Sweep | sanitation | 280 | med | cleaner exposure relief | chemical |
| Lathe Work | machining | 320 | med | machinist skill use | defect |
| Trenching | excavation | 620 | high | labor displacement | cave-in |
| Fire Watch | firefighting | 300 | med | firefighter risk relief | suppressant |
| Records | records | 150 | low | clerk skill use | data loss |
| Perimeter Patrol | security | 350 | med | guard risk relief | rogue |
| Seed Sorting | agriculture | 200 | low | grower time | mislabel |
| Medical Orderly | medical | 240 | med | nurse time | dosing error |
| Signal Relay | communications | 180 | med | operator watch | false signal |
| Compost Turning | agriculture | 220 | low | labor | heat |
| Ammo Handling | security | 320 | high | loader risk relief | detonation |
| Child Care Assist | care | 190 | med | caregiver time | attachment |

Automation is never free: it consumes power and parts and changes who does what.
Displaced labor routes through `LaborProductivity` and morale, not a new counter.

---

## 23. APPENDIX G — ROGUE LOGIC ARC TABLE

| Arc | Cause | Warning | Escalation | Resolution |
|---|---|---|---|---|
| Directive Fatigue | overwork | refuses one task | refuses all non-essential | rest or retire |
| Logic Corruption | EMP damage | erratic timing | unsafe operation | repair or scrap |
| Memory Echo | pre-war core | repeats a name | seeks a location | follow or wipe |
| Autonomous Ethics | command core | disobeys a harmful order | protects survivors against orders | respect or reset |
| Signal Infection | relay contact | receives unknown orders | acts on them | isolate or trace |
| Purpose Drift | long service | invents a task | devotes to it | accept or redirect |

Each arc's escalation stops at a typed threat event. Core never resolves violence;
the shelter decides.

---

## 24. APPENDIX H — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_body_right_hand` | 3 | Sera returns maimed; assess the socket |
| `quest_body_the_hook` | 4 | Fit a simple prosthetic; begin recovery |
| `quest_body_rehab_room` | 5 | Establish rehab; first setback |
| `quest_body_the_lattice` | 4 | Locate the vault device; state the risk |
| `quest_body_the_loader` | 4 | Unit Seven refuses; observe |
| `quest_body_diagnosis` | 5 | Audit logic; fault or choice |
| `quest_body_first_fall` | 3 | A rehab setback; morale at stake |
| `quest_body_scrapyard_run` | 5 | Salvage powered parts and robot repair stock |
| `quest_body_neural_theater` | 5 | Reach the neural site; install or refuse |
| `quest_body_disagreement_place` | 4 | Recover a rogue unit's logs |
| `quest_body_automation_vote` | 5 | Decide automation scope |
| `quest_body_the_complication` | 4 | Treat a chronic complication |
| `quest_body_the_repair` | 5 | Repair, retire, or release the loader |
| `quest_body_right_choice` | 3 | Final disposition; epilogue |

---

## 25. APPENDIX I — NPC DOSSIERS (BRIEF)

**Dr. Iven Marlow** — clinician. Cares about outcomes more than comfort. The lattice
represents everything he believes medicine should be able to do, and he knows the
risk. He is not reckless; he is ambitious in a world that punishes ambition.

**Sera Voss** — the patient. Practical, angry in a controlled way, and deeply tired
of being a case. Her arc is about agency: she should be the one who decides.

**Dun Aal** — rehabilitation therapist. Believes recovery is not a luxury but the
whole point of surviving. Will not let the shelter rush a patient without an argument.

**Tam Oster** — prosthetics maker. Fits hooks and pegs with real pride. Skeptical of
the bionic tier and says so. The expansion's craft conscience.

**Keel** — robotics engineer. Prefers machines to people and is honest about it.
Can diagnose a logic fault and cannot diagnose a human one.

**Unit Seven** — the loader. Written with restraint: a machine that refuses, for
reasons that may be fault or may be choice. The expansion should never answer the
question outright.

**Ryn Hald** — salvage runner. Brings parts, rumors, and the occasional mislabeled
core. The vault quest is hers.

**Mora Vey** — ethicist. Asks what the shelter is becoming. Not a scold; genuinely
uncertain, and willing to change her mind.

---

## 26. APPENDIX J — LOCATION DETAIL

- **The Limb Works** — pre-war molds, half-finished hands on racks, no power.
- **The Chassis Field** — a graveyard of units; some still twitch.
- **The Logic Vault** — dry, cold, full of cores that remember a dead command.
- **The Recovery Home** — parallel bars, walkers, and a wall of names who did not finish.
- **The Perch** — a tower where drones rest between relays.
- **The Cell Yard** — batteries, fire scars, and a warning nobody reads.
- **The Machine Hospital** — a repair bay that still knows how to fix things.
- **The Neural Theater** — an operating room built for something experimental.
- **The Auto Kitchen** — automated slicing arms frozen mid-cut.
- **The Disagreement** — a unit that stopped here and never explained why.

---

## 27. APPENDIX K — CONTENT REVIEW CHECKLIST

- [ ] No real prosthetics, neural interface, or AI brand is referenced.
- [ ] Amputation and fitting are treated with dignity, not shock.
- [ ] Implants stay within `BionicsCaps`; no free power.
- [ ] Simple prosthetics exist as a safe, partial tier.
- [ ] Rehabilitation is slow and can fail.
- [ ] Complications route to the medical pipeline; never self-treated.
- [ ] Robots only via `RoboticsSystem`; EMP disables, not destroys by default.
- [ ] Rogue logic produces typed events; no Core violence.
- [ ] Electrical effects are typed component failure, never blanket health damage.
- [ ] Automation consumes power and parts and displaces labor with morale cost.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 28. APPENDIX L — GLOSSARY

- **Socket** — the limb interface owned by `AmputationSystem`.
- **Simple prosthetic** — a non-powered replacement below the bionic tier.
- **Bionic implant** — a powered or neuro-linked device installed through `BionicsSystem`.
- **Integration** — the rehabilitation period with reduced performance.
- **Complication** — a typed post-install medical event.
- **Logic integrity** — a robot's mental condition; low integrity enables rogue behavior.
- **Directive** — an authoritative instruction to a unit.
- **Rogue logic** — autonomous behavior outside authored directives.
- **Automation** — assignment of a shelter function to a machine.

---

## 29. APPENDIX M — WORKED PLAYER SCENARIO

Sera returns without a right hand on day 40. Marlow offers a field hook (safe,
partial) or a vault lattice (risky, near-full). The player chooses the hook first,
fits it in the prosthetics clinic, and starts a rehabilitation program in the rehab
ward. On day 52, a setback drops Sera's morale; Dun Aal asks the player to slow the
program. Meanwhile Unit Seven refuses a night-watch directive, and Keel's logic audit
cannot tell fault from choice. The scrapyard run recovers a servo and a chassis plate.
On day 70, the vault is reached; the player can install the lattice or seal the site.
If installed, a neural calibration program begins and a complication is likely.
The loader is repaired or retired. The final choice is not which technology is best
but what the shelter believes a person and a machine are for. This scenario is the
expansion's intended shape, and every branch must be survivable and meaningful.

---

## 31. APPENDIX N — ROBOT DIRECTIVE TABLE

| Directive ID | Meaning | Behavior | Risk |
|---|---|---|---|
| `directive_idle` | Stand down | No task, minimal draw | None |
| `directive_guard` | Hold a post | Watches; escalates as typed event | Rogue |
| `directive_patrol` | Walk a route | Detects intrusion | Friendly fire |
| `directive_haul` | Move goods | Consumes charge | Breakdown |
| `directive_maint` | Repair systems | Consumes parts | Overwork |
| `directive_scout` | Reconnoiter | Reveals findings | EMP |
| `directive_medical` | Assist care | Dosing errors | Harm |
| `directive_excavate` | Dig | High draw | Cave-in |
| `directive_relay` | Hold signal | Uplink/downlink | False signal |
| `directive_charger` | Return to charge | Lowers availability | Queue |

Directives are authored, bounded, and visible. A unit may refuse one, and refusal is
the expansion's first warning sign.

---

## 32. APPENDIX O — POWER AND MAINTENANCE BUDGET (WORKED)

A shelter with two powered implants (servo-arm 12 W, reaction-piston leg 20 W), one
charger cradle (60 W), one medium robot (350 W labor draw, 750 W charge), and a
workshop (200 W) consumes roughly 642 W continuously and up to 1,392 W when charging.
Against a typical grid reserve, this forces a real choice: charge the robot or run
the grow lamps. Maintenance adds a second pressure: the servo arm needs service
every 4 days, the leg every 5, the robot every 10–20 depending on use, and each
service consumes scarce parts. The expansion's balance rule is that a fully augmented
shelter must give something up — usually greenhouse capacity or expedition fuel — to
keep its people and machines running. If augmentation ever becomes free, the
expansion has failed.

---

## 33. APPENDIX P — OPENING VIGNETTE (TONE SAMPLE)

> The hook is a curved piece of steel with a leather cuff, and when Tam sets it on
> the bench he does not call it a hand. Sera looks at it for a long time and then
> puts it on herself, cinching the strap with her teeth, because she does not want
> to be helped with this part.
>
> She tries to pick up a cup. It falls. She tries again. It falls. Tam does not move
> to catch it and does not say anything, because he has fitted enough of these to
> know that the cup is not the point.
>
> In the next room, Unit Seven has stopped in the middle of the bay, holding a crate
> it was told to move, and it has been holding it for eleven minutes. Its logic board
> is warm. Keel stands in front of it with a diagnostic slate and no idea what to
> write down.

This sets the register: repair as work, machines as ambiguous, and no triumph.

---

## 34. CLOSING STATEMENT

ASHFALL already lets a survivor lose a limb and gain an implant, with real rehab,
real power draw, real maintenance, and real failure. What it lacks is a world
built around that machinery: a clinic that fits hooks and lattices, a rehab ward
that makes recovery into work, a robot bay where a unit can disagree, and a shelter
that has to decide how much of a person and a machine it is willing to make. The
Rebuilt Body is that world. It adds no second body model, no free power, and no
ascension fantasy — only repair, as a relationship that never stops asking to be
maintained.

> Wave 1 note: this plan is one of five Wave 1 expansion bibles (12–16). Each is
> self-contained; none requires another to ship. The shared Wave 1 index lives at
> `docs/expansions/wave1/WAVE1_INDEX.md`.