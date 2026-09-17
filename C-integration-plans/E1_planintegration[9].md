---
PLAN_ID: E1-9
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 9
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 156 — Shelter Expansion & Physical Renovation"
SEQUENCE_FILENAME: "E1_planintegration[9].md"
PREVIOUS_FILENAME: "E1_planintegration[8].md"
NEXT_FILENAMES:
  - "E1_planintegration[10].md"
  - "E1_planintegration[11].md"
CATEGORY: LINK+SHELTER_TOPOLOGY+CONSTRUCTION+INFRASTRUCTURE+PRESENTATION
PRIMARY_INTENT: "Make shelter growth spatial, persistent, visible, and strategically meaningful by adding construction/topology rails while preserving existing room, duty, thermal, power, ventilation, storage, defense, inventory, and survivor authorities."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_SHELTER_MODEL_FORBIDDEN: true
SECOND_THERMAL_GRID_FORBIDDEN: true
SECOND_POWER_GRID_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: HIGH
SCOPE_RISK: VERY_HIGH
---

# E1 Plan Integration [9] — Shelter Expansion, Construction, Renovation, Topology, Infrastructure, and Visible Physical Growth

> **Sequence rule:** this file is `E1_planintegration[9].md`.
> The next files are `E1_planintegration[10].md`, `E1_planintegration[11].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 156 into an implementation-grade shelter-growth programme.

The source plan identifies a high-value progression gap: the shelter has rooms, assignments, thermal behavior,
power infrastructure, room identity/wear, and defensive concepts, but the physical shelter does not visibly
grow. A player cannot dig a new chamber, reopen a sealed section, renovate an unusable room, create a new
wing, or watch the holdfast become structurally different because of collective effort.

The solution must not create a second shelter simulation. The expansion layer should own **spatial topology,
construction-project state, blueprint/configuration references, and room-activation transitions**. Existing
authorities must continue to own room schedule/assignment, heat, power, ventilation, water, inventory,
storage, defense, survivor needs, skill, condition/maintenance, and production.

The foundational rule is:

**construction changes what rooms and connections exist; canonical systems decide how those spaces behave.**

A new room should enter the same room registry/topology used by every downstream system. A renovation should
modify canonical room condition/upgrades through their owner. A new power connection should be registered
with the existing power grid. A ventilation duct should be an edge/capability in the existing ventilation
authority. Shelter expansion should therefore be an integration event, not a parallel set of copied room
stats.

## 1. Source Intent Preserved

Plan 156 asks for:

- new-room construction;
- renovation;
- spatial expansion outward/downward;
- room upgrades;
- blueprints;
- resource/labor/time costs;
- interruptions and resume;
- structural stability/collapse risk;
- power/water/ventilation/defense integration;
- construction skill and tools;
- construction events and quests;
- shelter map UI;
- old-save compatibility;
- deterministic outcomes;
- data-driven blueprints/upgrades;
- headless selftest;
- visible shelter evolution.

E1-9 preserves those ambitions, but sequences them so topology and authority integration are proven before
large blueprint catalogs, structural disasters, or dozens of upgrades are added.

## 2. Core Architecture Thesis

```text
Canonical shelter topology
    |
    +--> existing rooms
    +--> room connections
    +--> blocked/sealed potential spaces
    |
    v
Construction proposal
    |
    +--> blueprint
    +--> target/topology location
    +--> materials
    +--> tools/workstation
    +--> labor/duty
    +--> duration
    |
    v
Canonical construction/job scheduler
    |
    v
Completion transaction
    |
    +--> create/activate room node
    +--> create connection edge
    +--> register room with thermal authority
    +--> register power/water/ventilation interfaces
    +--> register schedule/capacity/defense/storage capability
    |
    v
Existing shelter systems consume the changed topology
```

Construction owns the transition. It does not own the final thermal/power/needs behavior.

## 3. Non-Negotiable Rules

- Every room has one stable identity.
- Room existence/topology has one canonical owner.
- `ShelterScheduleSystem` or its successor owns room assignment/scheduling.
- `ShelterThermalSystem` owns room temperature/heat behavior.
- `PowerGridSystem` owns power nodes/loads/connections.
- Ventilation/air authority owns airflow/air-quality effects.
- Water/plumbing authority owns water supply if present.
- Inventory/storage authority owns item quantities and container capacities.
- Defense authority owns combat/fortification effects.
- Condition/maintenance authority owns degradation if such a system exists.
- Construction may request/register upgrades but does not duplicate those systems.
- Construction materials are consumed transactionally through canonical inventory.
- Labor is assigned through duty/job systems.
- Tools/equipment are canonical items/assets and their wear belongs to tool/maintenance authority.
- Project progress is persisted exactly once.
- Save/reload cannot double-consume materials or double-create rooms.
- Cancellation/refund rules must conserve resources.
- Expansion topology must remain deterministic.
- Hidden/sealed spaces cannot be materialized twice.
- Structural failure cannot silently delete survivors/items/rooms.
- Deeper/outward expansion mechanics must be data-driven and bounded.
- A “shelter level/depth” number cannot become a substitute for real topology.
- The first slice uses a small set of buildable spaces and renovations before 20 blueprints/10 upgrades.
- Old saves must map existing rooms into the new topology without losing state.
- Visual shelter layout is a projection of canonical topology, not UI-owned construction state.

## 4. Acceptance Slices

### Slice A — Topology and room identity
Represent existing shelter rooms and connections canonically; migrate old saves safely.

### Slice B — Renovation
Repair/restore one existing room through real materials/labor/time.

### Slice C — New room construction
Add one new room node and one connection edge.

### Slice D — Infrastructure registration
Connect a newly completed room to thermal, power, ventilation, schedule, storage, and other rails.

### Slice E — Spatial expansion
Open a new wing/level through an explicit topology edge and structural-support rules.

### Slice F — Advanced content
Blueprint discovery, structural events, hidden rooms, 20+ blueprints, landmarks, and disasters.

Do not start with arbitrary free-form underground digging.


---

## E1-9A — Premise verification and shelter-authority audit

**Goal:** Verify all existing shelter, room, topology, power, thermal, ventilation, storage, defense, duty, condition, and save authorities before introducing construction state.

### Required substeps

1. Inspect `Assets/Ashfall.Core/Shelter/`, `ShelterScheduleSystem`, `ShelterThermalSystem`, `PowerGridSystem`, ventilation/air systems, water/plumbing systems, shelter defense, room catalogs, room-wear/identity systems, duty roster, inventory/storage, production/workshop jobs, research/blueprint systems, survivor skills, construction-related items/tools, and save registries.
2. Verify whether `shelter_rooms.json` exists at current HEAD and whether rooms are catalog definitions, runtime instances, or both.
3. Verify whether any canonical room registry or shelter graph already exists.
4. Verify whether room IDs are stable across saves.
5. Verify whether room adjacency/connections are represented anywhere.
6. Verify how room temperature, capacity, assignment, power, and ventilation currently identify rooms.
7. Search for sealed rooms, hidden rooms, bunker levels, doors, tunnels, expansion slots, or construction systems.
8. Search for generic production/project/job scheduling that can own construction progress.
9. Search prior plans for Plan 29, 41, 71, 138 and any newer rail implementations that supersede them.
10. Create `docs/systems/SHELTER_EXPANSION_AUTHORITY_MAP.md`.
11. Create intake duplicate-search evidence.
12. Set `PREMISE_VERIFIED_AT` to current HEAD and block implementation if a canonical shelter graph is missing but required by multiple systems.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9B — Shelter topology ADR

**Goal:** Define the canonical spatial model that all existing room systems will consume.

### Required substeps

1. Write an ADR comparing a graph of room nodes/connection edges, fixed grid coordinates, authored slot graph, and hybrid authored-graph + visual coordinates.
2. Prefer stable logical topology over UI pixel/grid coordinates as authority.
3. Define whether topology lives in a new `ShelterTopology` authority or extends an existing room registry.
4. Define room-node identity, room type/profile reference, lifecycle state, connection edges, level/depth metadata, and optional visual placement metadata.
5. Explicitly exclude temperature, power state, air quality, inventory contents, survivor assignment, morale, and defense state from topology.
6. Define potential/blocked expansion nodes separately from active rooms.
7. Define migration from current static room lists.
8. Define topology validation: connectedness, unique IDs, valid edges, no impossible cycles if restricted, no duplicate active slot.
9. Require second-tool ADR review.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9C — Canonical room instance contract

**Goal:** Represent built rooms as stable instances referencing room definitions while preserving downstream authorities.

### Required substeps

1. Define stable room instance ID and room profile/type ID.
2. Define lifecycle status: Existing, Sealed, Planned, UnderConstruction, Active, Disabled, Collapsed, Ruined, Archived only where semantics require it.
3. Define topology position/level and connection references.
4. Define canonical capability tags such as sleeping, medical, storage, workshop, leisure, power-room, ventilation-node, quarantine, or infrastructure if existing systems use them.
5. Do not store copied temperature, radiation, power availability, resident list, inventory contents, or morale modifiers in the expansion DTO.
6. Define room creation/activation registration callbacks into consumers.
7. Define room deactivation/collapse deregistration rules.
8. Add validation for missing profile, invalid connection, duplicate instance ID, unsupported capability.
9. Add save fixtures for existing active room, sealed room, planned slot, collapsed room, and removed profile.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9D — Old shelter migration into canonical topology

**Goal:** Map existing static-room saves into the new spatial model without changing established behavior.

### Required substeps

1. Inventory every room identity that old saves can contain.
2. Create deterministic room instance IDs for legacy static rooms.
3. Create authored default connection topology matching existing gameplay expectations.
4. Preserve room assignments through stable mapping.
5. Preserve thermal/power/ventilation room references.
6. Do not fabricate renovations/upgrades that were not previously stored.
7. Default construction-project state to empty.
8. Default potential expansion slots from catalog/world configuration, not random generation.
9. Make migration idempotent.
10. Add fixtures for minimal old save, fully populated shelter, room references from multiple systems, missing room definition, and unsupported legacy room ID.
11. Run parity tests proving old shelter behavior is unchanged immediately after migration.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9E — Blueprint and construction-definition schema

**Goal:** Define data-driven build/renovation/expansion definitions with explicit topology and system requirements.

### Required substeps

1. Define blueprint ID, project class, source/knowledge requirement, target room profile, allowed target slot/connection types, material bill, labor requirement, duration, required tools/workstation, skill/engineering requirement, structural prerequisites, infrastructure prerequisites, completion actions, and localization keys.
2. Represent downstream room capabilities through room profiles, not arbitrary blueprint-side stat deltas.
3. Define renovation blueprints separately from new-room blueprints where semantics differ.
4. Define expansion/tunnel blueprints as topology operations.
5. Validate every item/tool/research/room/profile/capability reference.
6. Detect blueprint dependency cycles.
7. Define schema versioning.
8. Start with 5-8 blueprints.
9. Add integrity tests for invalid target, negative cost, impossible duration, unknown room, circular prerequisite, duplicate ID, and unsupported completion action.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9F — Construction-project state machine

**Goal:** Persist long-running building work without duplicating generic job/production state.

### Required substeps

1. Determine whether generic project/job scheduling can own project progress.
2. If a dedicated construction state is required, define project ID, blueprint ID, target topology reference, stage, reserved/consumed material references, labor requirement, applied labor, start day, completion transaction ID, blocked reason, and status.
3. Define statuses: Planned, Validating, AwaitingResources, AwaitingLabor, Active, Interrupted, CompletedPendingCommit, Completed, Cancelled, Failed.
4. Use game time rather than wall clock.
5. Define progress as work performed, not merely completionDay prediction.
6. Define deterministic progress ordering.
7. Persist only authoritative project facts.
8. Add state-machine tests.
9. Ensure restore cannot regress a completed project back to Active.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9G — Transactional project planning/start

**Goal:** Start construction only after validating topology, resources, labor, tools, and prerequisites.

### Required substeps

1. Validate blueprint knowledge/unlock.
2. Validate target room/slot/edge is eligible and not already claimed.
3. Validate structural prerequisites.
4. Validate resources and storage ownership.
5. Validate tools/equipment/workstation availability.
6. Validate labor/skill eligibility.
7. Reserve target topology slot immediately so two projects cannot build into the same space.
8. Reserve/consume materials according to canonical production convention.
9. Create stable operation/project ID.
10. Return explicit blocking reasons.
11. Add tests for missing blueprint, invalid target, duplicate slot reservation, no resources, no labor, no tool, and concurrent start.
12. UI may never create project state directly.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9H — Material accounting and conservation

**Goal:** Make shelter growth consume real resources without duplication or hidden daily recharges.

### Required substeps

1. Identify canonical inventory transaction API.
2. Define material timing: upfront reservation, staged delivery, progressive consumption, or milestone consumption using existing project conventions.
3. Ensure every consumed item is accounted for.
4. Define unused-material return on cancellation.
5. Define damaged/wasted materials only as explicit outcomes.
6. Do not hard-code concrete/steel/wood if actual item catalogs use different IDs.
7. Add conservation assertions.
8. Test insufficient storage destination on refund.
9. Test save/reload around reservation and milestone consumption.
10. Measure total material cost per functional capacity added.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9I — Labor, duty, and worker eligibility

**Goal:** Apply survivor labor through canonical duty/job systems.

### Required substeps

1. Represent construction as a duty/job target.
2. Use canonical survivor availability, fitness, injury, fatigue, skill, and schedule rules.
3. Define maximum simultaneous workers per project.
4. Define labor contribution per time unit through job authority.
5. Do not copy worker IDs into project state if assignment authority already stores them; reference/query instead.
6. Handle worker reassignment, absence, sickness, expedition departure, and death.
7. Allow interruption without losing completed work unless an explicit project stage says otherwise.
8. Add tests for valid workers, unfit workers, worker change, no labor, multiple projects competing for labor, and save/load.
9. Measure labor-days per room and opportunity cost.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9J — Tools, machinery, and wear

**Goal:** Use canonical tool/equipment condition rather than creating a construction-only tool inventory.

### Required substeps

1. Audit drills, saws, welding tools, excavators, supports, and workshop equipment actually represented in data.
2. Define required tool capabilities by blueprint.
3. Use canonical tool availability/condition.
4. Apply tool wear through maintenance/equipment authority.
5. Prevent one unique tool from supporting two simultaneous incompatible jobs.
6. Define failure or slowdown if tools break.
7. Add tests for valid tool, missing capability, broken tool, repair, shared tool contention, and save/load.
8. Keep heavy machinery content deferred unless existing item/asset rails support it.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9K — Construction skill and engineering integration

**Goal:** Use existing survivor skill/proficiency frameworks to influence eligibility/throughput rather than adding a parallel skill system.

### Required substeps

1. Audit construction/engineering/mechanics skills or traits.
2. Map blueprint complexity to canonical skill requirements.
3. Use skill authority for XP/progression.
4. Define throughput/quality effects through job policies.
5. Do not let construction code directly grant XP.
6. Define apprentice/mentor support only if training rails exist.
7. Add tests for insufficient skill, qualified worker, mentor session, XP idempotency, and save/load.
8. Allow first slice to use generic labor if no construction skill exists.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9L — Renovation vertical slice

**Goal:** Ship renovation before free-form expansion to prove materials, labor, time, condition, and UI integration.

### Required substeps

1. Select one existing degraded/low-quality room type.
2. Define renovation blueprint.
3. Use canonical room condition/maintenance authority for restoration.
4. Use real materials/labor/tools.
5. Block room functions or reduce availability during renovation according to room authority.
6. On completion, request canonical condition/upgrade change.
7. Do not set temperature/morale/efficiency directly.
8. Add save/load mid-renovation.
9. Add tests for completion, cancellation, room in use, insufficient materials, and duplicate completion.
10. Capture player-facing before/after evidence.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9M — New-room construction vertical slice

**Goal:** Add one real room node and connection edge transactionally.

### Required substeps

1. Choose one authored expansion slot adjacent to an existing room.
2. Select one simple room profile such as storage or living quarters.
3. Reserve slot and connection edge.
4. Complete materials/labor/time.
5. At commit, create room instance and topology edge atomically.
6. Register room with downstream authorities.
7. Do not mark room usable until all mandatory registrations succeed.
8. Define rollback/degraded state if one registration fails.
9. Add tests for successful build, failed registration, duplicate commit, save before commit, save after commit, and removed blueprint.
10. Verify new room appears in schedule/map/thermal/power systems at correct stage.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9N — Room activation transaction and downstream registration

**Goal:** Make completion a cross-system composition operation with explicit success/failure semantics.

### Required substeps

1. Define canonical completion coordinator/host boundary.
2. Register new room identity with schedule/assignment authority.
3. Register thermal node/parameters from room profile.
4. Register power load/connection hooks.
5. Register ventilation/air node.
6. Register water/plumbing node only if present.
7. Register storage/container capability if room profile requires it.
8. Register defense/structural zones if required.
9. Register UI/map projection data.
10. Emit `RoomActivated` only after mandatory registration succeeds.
11. Make transaction idempotent.
12. Add negative test for each missing mandatory authority.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9O — Shelter thermal integration

**Goal:** Ensure new rooms participate in the existing thermal graph instead of storing their own temperature.

### Required substeps

1. Identify how thermal nodes and adjacency are represented.
2. Derive wall/connection/insulation parameters from room and connection profiles.
3. On room activation, create/register thermal node.
4. On connection opening/closing, update thermal graph.
5. Renovation/insulation upgrades call thermal authority.
6. Do not persist duplicate current temperature in expansion state.
7. Add tests for new cold room, heated connection, insulated renovation, sealed door, save/load, and thermal recalculation.
8. Benchmark graph growth with max planned rooms.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9P — Power-grid integration

**Goal:** Connect new rooms and upgrades through the canonical power grid.

### Required substeps

1. Define power-node/load requirements in room/upgrade profiles.
2. Building a room does not automatically energize it unless grid connection exists.
3. Create power connection as separate infrastructure project where useful.
4. Use canonical breaker/circuit/capacity rules.
5. Lighting, medical, workshop, ventilation, elevators, and pumps consume real power through their systems.
6. Do not store `powered` boolean in expansion state.
7. Add tests for unpowered new room, connected room, overloaded circuit, power project completion, save/load, and disconnect.
8. Expose authoritative power block reasons in UI.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9Q — Ventilation and air-quality integration

**Goal:** Make new underground volume affect real ventilation/air systems.

### Required substeps

1. Verify ventilation authority and room-node representation.
2. Register air/ventilation node on activation.
3. Require ducts/fans/filters through infrastructure projects where design requires.
4. Opening a new section may increase air demand through canonical ventilation calculations.
5. Do not encode air quality in room expansion DTO.
6. Add tests for unventilated room, connected duct, fan power loss, quarantine isolation, and save/load.
7. Block room occupancy if canonical air authority says unsafe where appropriate.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9R — Water/plumbing integration

**Goal:** Connect water-dependent rooms through a real plumbing/resource authority if one exists.

### Required substeps

1. Audit water storage/flow/plumbing systems.
2. Define water inlet/outlet capability in room profiles.
3. Create plumbing connection upgrades as topology/infrastructure operations.
4. Do not create a second water balance.
5. Kitchen, clinic, sanitation, and garden rooms consume canonical water.
6. Add tests for dry room, connected room, insufficient water, leak/failure if supported, and save/load.
7. Defer plumbing complexity if no canonical water rail exists.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9S — Storage and inventory integration

**Goal:** Make newly built storage capacity use canonical inventory containers.

### Required substeps

1. Define storage-room profile capability.
2. Create/register container/capacity through inventory authority on activation.
3. Do not store room item lists in expansion state.
4. Define room disable/collapse behavior with occupied inventory.
5. Prevent demolition/renovation that would orphan items.
6. Add tests for capacity creation, room unavailable, full storage, collapse, relocation, and save/load.
7. Ensure inventory conservation.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9T — Living quarters and population capacity

**Goal:** Increase habitable capacity through canonical housing/bunk/census rules.

### Required substeps

1. Identify canonical bunk/room-capacity authority.
2. Living room profile exposes capacity/bunk capability.
3. Do not directly increase survivor cap in expansion state.
4. Use room condition, temperature, ventilation, and power as eligibility inputs where supported.
5. Add tests for completed room, unpowered/unheated unsafe room, overcapacity, reassignment, and save/load.
6. Measure crowding relief and population-capacity value.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9U — Medical/work/leisure room capability integration

**Goal:** Treat room types as capability compositions rather than hard-coded expansion effects.

### Required substeps

1. Medical rooms register medical-treatment capabilities.
2. Work rooms register workshop/lab/production capabilities.
3. Leisure rooms register social/recreation capabilities.
4. Office/administrative rooms register governance/work capabilities only if those systems exist.
5. Do not directly grant morale/research/production bonuses from expansion code.
6. Use canonical systems to calculate effects.
7. Add one targeted integration test per shipped room category.
8. Start with only categories whose consumers already exist.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9V — Infrastructure upgrade contract

**Goal:** Model heating, insulation, lighting, ventilation, security, and other upgrades as typed modifications consumed by their owner systems.

### Required substeps

1. Define upgrade ID, compatible room types, slot/capability requirements, materials/labor/duration, owning consumer, and reversible/removal semantics if applicable.
2. Heating routes to thermal/power systems.
3. Insulation routes to thermal parameters.
4. Lighting routes to power plus room/needs/morale consumer where canonical.
5. Ventilation routes to air authority.
6. Security routes to defense/security authority.
7. Decoration routes to room/social/morale authority if one exists.
8. Prohibit generic `efficiencyBonus` or `moraleBonus` fields in expansion data.
9. Add validation that every upgrade has a registered consumer.
10. Start with 3-4 upgrades before 10.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9W — Structural support and stability model

**Goal:** Introduce structural constraints only at a level supported by real topology and engineering data.

### Required substeps

1. Decide whether structural stability is a new canonical authority, a topology rule, or deferred content.
2. Do not create a global 0-100 shelter stability number if no downstream model can explain it.
3. Prefer local support requirements: span, depth, load, support pillar, excavation type, unstable-zone tag.
4. Represent hazardous expansion slots with authored/geological tags.
5. Require support projects before opening certain edges/nodes.
6. Define deterministic structural risk only if events/physics abstractions need it.
7. Add tests for supported build, unsupported blocked build, depth restriction, support installed, and save/load.
8. Keep full engineering simulation out of scope.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9X — Depth, levels, and vertical transport

**Goal:** Support downward expansion without turning depth into a fake progression stat.

### Required substeps

1. Represent levels as topology metadata.
2. Vertical edges require stairs/ladder/elevator capability according to design.
3. Use travel/time/accessibility authority for movement if room traversal is simulated.
4. Deeper rooms may have different authored excavation/thermal/radiation/water tags.
5. Do not universally claim deeper = safer/stabler unless data supports it.
6. Elevators consume canonical power and maintenance.
7. Add tests for ladder connection, powered elevator, power loss, inaccessible level, and save/load.
8. Cap maximum authored levels in first release.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9Y — Expansion slots, sealed sectors, and hidden spaces

**Goal:** Make spatial discovery authored and deterministic before considering procedural bunker generation.

### Required substeps

1. Define potential expansion nodes/edges in data.
2. States may include Unknown, Sealed, Surveyed, Buildable, Hazardous, Opened.
3. Knowledge/reveal authority owns what the player knows.
4. Construction authority owns whether an opening project exists.
5. Hidden-room discoveries produce canonical room/topology activation only once.
6. Do not roll infinite random rooms.
7. Add tests for sealed slot, discovered slot, hidden room, duplicate discovery, invalid slot, and save/load.
8. Use this rail for 'The Discovery' quest/event.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9Z — Blueprint discovery and knowledge integration

**Goal:** Route blueprint availability through canonical research/exploration/trade knowledge systems.

### Required substeps

1. Audit research/knowledge/unlock systems.
2. Blueprint catalog definitions are static; known/unlocked state belongs to knowledge/research authority.
3. Do not persist a duplicate `availableBlueprints` list in expansion state if derivable.
4. Allow exploration/trade quest rewards to unlock blueprint IDs through canonical APIs.
5. Add tests for locked, unlocked, duplicate unlock, removed blueprint, and old-save default.
6. Let first-slice basic blueprints start known if necessary.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AA — Construction interruption, blocking, and resumption

**Goal:** Make long projects robust to resource/labor/power/tool disruptions.

### Required substeps

1. Define blocked reasons separately from failed projects.
2. Resource shortage pauses future stages rather than consuming nonexistent materials.
3. Worker absence pauses labor.
4. Tool failure pauses relevant stage.
5. Power requirement pauses powered machinery stages.
6. Emergency shelter state may suspend non-essential construction.
7. Resume deterministically when conditions recover.
8. Do not destroy completed labor arbitrarily.
9. Add tests for each block/unblock path and save/load.
10. Only explicit accident/failure outcomes may lose progress.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AB — Cancellation, demolition, and resource reconciliation

**Goal:** Allow project cancellation and eventual room removal without duplication or orphaned state.

### Required substeps

1. Define cancellation by project stage.
2. Return unconsumed reserved materials.
3. Apply explicit salvage/loss rules to consumed materials.
4. Release topology reservation.
5. Do not permit demolition of occupied/critical rooms until residents/items/services are reconciled.
6. Define downstream deregistration order.
7. Add tests for early cancel, late cancel, full room demolition if enabled, occupied room block, storage contents, power/thermal deregistration, and save/load.
8. Defer demolition from first slice if reconciliation is too broad.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AC — Room condition and maintenance boundary

**Goal:** Reuse existing room wear/maintenance rather than coupling expansion to a second condition system.

### Required substeps

1. Audit Plan 29/live room condition implementation.
2. If room condition already exists, construction only sets initial state and requests renovation/repair.
3. If it does not exist, define a separate maintenance rail rather than hiding degradation inside expansion.
4. Do not tick every room condition from construction code unless that authority is explicitly assigned.
5. Renovation may restore condition through canonical command.
6. Add tests for new room condition, degraded room renovation, save/load, and no duplicate degradation.
7. Keep disaster/wear expansion separate from construction progression.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AD — Structural accidents and collapse gate

**Goal:** Add construction hazards only after health, topology, survivor location, and room deactivation rails can resolve them safely.

### Required substeps

1. Define accident eligibility from project stage, local hazard, worker safety, tool condition, and engineering support.
2. Use deterministic RNG only if stochastic accidents are accepted.
3. Route injuries through health authority.
4. Route room damage/collapse through topology/condition authority.
5. Reconcile occupants/items/services before disabling a room.
6. Do not use collapse as cosmetic random punishment.
7. Add tests for no-accident baseline, deterministic accident, worker injury, blocked room, item reconciliation, and save/load.
8. Feature-gate collapse events from the first construction slice.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AE — Construction events and quest hooks

**Goal:** Add narrative around physical growth after the core building transaction is stable.

### Required substeps

1. Create event hooks for blueprint discovery, groundbreaking, completion, hidden space, infrastructure connection, structural problem, and landmark milestone.
2. Quest authority owns quest lifecycle.
3. Prioritize Architect, Engineer, Discovery, Crisis, and one completion/landmark quest.
4. Use project/room IDs as provenance.
5. Prevent duplicate event/quest triggers after reload.
6. Allow quest invalidation if project cancelled or room lost.
7. Add tests for eligibility, dedupe, invalidation, and completion.
8. Keep celebratory morale effects authority-owned.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AF — Shelter-map UI and topology read model

**Goal:** Make physical growth visible through an authoritative map rather than a UI-owned grid.

### Required substeps

1. Audit existing shelter map/room/navigation surfaces.
2. Render room nodes and connections from canonical topology.
3. Use visual coordinates as presentation metadata only.
4. Show active, sealed, planned, under-construction, disabled, and hazardous spaces distinctly.
5. Show room type/capabilities, condition, power/thermal/ventilation status through canonical read models.
6. Show construction progress and blocking reason.
7. Every Build/Renovate/Connect/Cancel action calls authoritative commands.
8. Do not allow drag/drop to mutate topology without validation.
9. Add snapshot tests for legacy shelter, one planned room, active project, multi-level shelter, sealed node, and infrastructure warning.
10. Support keyboard/navigation access.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AG — Construction planning UI and decision support

**Goal:** Show strategic costs/tradeoffs before the player commits a project.

### Required substeps

1. Display material bill, labor-days, tool/skill requirements, duration, infrastructure prerequisites, topology effect, and expected capability.
2. Show what room/services will be unavailable during renovation.
3. Show connection consequences such as new thermal load or power requirement where authoritative forecasts exist.
4. Do not fabricate exact morale/efficiency gains.
5. Show blocked reasons from authorities.
6. Allow project comparison/queue view if generic job UI supports it.
7. Add interaction tests for plan/start/cancel.
8. Keep advanced optimizer/recommendation features out of first slice.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AH — Construction queue and concurrency policy

**Goal:** Bound simultaneous projects and shared-resource contention through existing scheduler/commitment rails.

### Required substeps

1. Define whether multiple projects can run concurrently.
2. Use duty/workshop/tool/resource constraints to determine effective concurrency.
3. Do not create a parallel construction-only calendar if generic job scheduler exists.
4. Reserve unique topology targets and unique tools.
5. Define project priority/pause rules.
6. Add tests for two independent projects, shared tool, shared workers, same target, and emergency suspension.
7. Measure construction burden on shelter operation.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AI — Shelter-growth balance and progression

**Goal:** Make expansion a strategic choice rather than an automatic always-good upgrade path.

### Required substeps

1. Measure material/labor/power/heat/ventilation cost per new room.
2. Measure population/capability pressure relieved.
3. Ensure extra volume creates upkeep/infrastructure demands where canonical systems support them.
4. Ensure storage/living expansion does not trivialize scarcity/crowding too early.
5. Gate advanced rooms through knowledge/skills/resources rather than arbitrary shelter level alone.
6. Compare renovate-existing versus build-new choices.
7. Allow successful campaigns without maximal expansion.
8. Start with authored topology budget.
9. Record dominant blueprint/build-order patterns.
10. Use balance evidence before adding 20 blueprints.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AJ — Max-expansion and topology-scale soak

**Goal:** Prove the shelter graph and downstream systems remain stable at intended maximum size.

### Required substeps

1. Define first-release maximum active rooms/levels/edges.
2. Generate or load max authored shelter.
3. Run thermal, power, ventilation, duty, storage, and map queries.
4. Measure build/activation time, per-tick cost, save size, topology validation cost, and UI read-model cost.
5. Run repeated construction/renovation across 180 days.
6. Verify no orphaned nodes/edges.
7. Verify bounded project/history records.
8. Record median/p95 relevant tick costs.
9. Add regression thresholds.
10. Block expansion-content growth if downstream systems scale poorly.

### Shelter-expansion invariants

- Construction owns projects/topology transitions; canonical systems own room behavior.
- Room identity and topology are unique and stable.
- Thermal, power, ventilation, inventory, duty, and defense state are never duplicated.
- Project resource/labor operations are transactional and idempotent.
- Completion creates/registers a room exactly once.
- Save/reload cannot replay completion or consume materials twice.
- Spatial growth is authored/bounded before any procedural expansion.
- Every downstream registration has a negative test.

### Negative tests

- One project creates the same room twice.
- Two projects reserve the same expansion slot.
- Expansion state stores a copied temperature/power/inventory value that can diverge.
- Cancel/reload produces net-positive materials.
- A room becomes assignable before mandatory downstream registration succeeds.
- A removed/collapsed room leaves orphaned inventory or survivors.
- Old-save migration changes shelter behavior immediately.
- Max expansion produces orphaned topology edges or superlinear tick cost.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Topology/authority-boundary tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Resource/labor conservation passes.
- [ ] Headless vertical slice passes.
- [ ] Balance/performance metrics are captured.
- [ ] Docs/plan metadata are current.

---

## E1-9AK — Save contract and restoration ordering

**Goal:** Persist construction/topology without duplicating downstream state and restore systems in a safe dependency order.

### Required substeps

1. Define topology/room-instance save section and project save section only if existing she

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
