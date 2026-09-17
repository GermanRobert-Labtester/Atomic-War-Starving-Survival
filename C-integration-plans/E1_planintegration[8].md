---
PLAN_ID: E1-8
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 8
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 152 — Vehicle Customization & Mobile Base"
SEQUENCE_FILENAME: "E1_planintegration[8].md"
PREVIOUS_FILENAME: "E1_planintegration[7].md"
NEXT_FILENAMES:
  - "E1_planintegration[9].md"
  - "E1_planintegration[10].md"
CATEGORY: LINK+VEHICLES+EXPEDITIONS+MOBILE_CAMP+PRESENTATION
PRIMARY_INTENT: "Turn vehicles into persistent configurable expedition assets and bounded mobile camps while preserving canonical ownership of fuel, cargo, survivor needs, combat, repair, research, and expedition state."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_SHELTER_AUTHORITY_FORBIDDEN: true
SECOND_INVENTORY_FORBIDDEN: true
SECOND_COMBAT_AUTHORITY_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: HIGH
BALANCE_RISK: VERY_HIGH
SCOPE_RISK: VERY_HIGH
---

# E1 Plan Integration [8] — Vehicle Customization, Expedition Support, Mobile Camps, Convoys, and Persistent Vehicle Assets

> **Sequence rule:** this file is `E1_planintegration[8].md`.
> The next files are `E1_planintegration[9].md`, `E1_planintegration[10].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 152 into an implementation-grade vehicle programme.

The source plan correctly identifies that vehicles already have functional expedition logistics—speed,
capacity, breakdown risk, and fuel—but little persistent strategic identity. The proposed expansion adds
modules, condition, mobile sleeping/storage, camp support, combat capabilities, research unlocks, events,
quests, customization UI, and long-term persistence.

The main architectural danger is that a “mobile base” can easily become a second shelter simulator with its
own inventory, beds, power, medicine, cooking, survivors, repairs, combat, research, and save tree. E1-8
explicitly forbids that. A vehicle is a persistent asset participating in existing systems. A camp is a
temporary expedition state projected around a vehicle and current expedition party. Cargo remains inventory.
Fuel remains fuel/resource state. Survivor fatigue remains Needs state. Medical treatment remains health
authority. Mounted weapons remain combat capabilities interpreted by TacticalCombat. Repair remains
maintenance/workshop authority. Research remains Research. Vehicle modules provide capabilities and modifiers;
they do not own downstream facts.

The programme is divided into narrow vertical slices:

1. persistent vehicle identity and module slots;
2. transactional install/remove/replace;
3. capability projection into expedition travel;
4. bounded mobile-camp support;
5. condition/maintenance integration;
6. optional combat capability handoff;
7. content/UI/events only after the core asset loop is proven.

## 1. Source Plan Intent Preserved

Plan 152 asks for:

- persistent `VehicleInstance` state;
- module categories: armor, cargo, living, weapon, utility;
- installation costs/time and prerequisites;
- mobile shelter functionality;
- condition/fuel persistence;
- expedition base camps;
- combat effects;
- research integration;
- event and quest hooks;
- persistent custom names/modules;
- old-save compatibility;
- deterministic outcomes;
- module catalog validation;
- customization UI;
- 20 starter modules;
- CI selftest.

E1-8 keeps those ambitions but routes each effect through an existing authority.

## 2. Core Architecture Thesis

```text
Vehicle profile
    |
    v
Persistent VehicleInstance identity
    |
    +--> installed module references
    +--> condition/maintenance reference
    +--> fuel-store reference
    +--> cargo-container reference
    +--> custom name / ownership metadata
    |
    v
Capability projection
    |
    +--> travel speed / range / breakdown modifiers
    +--> cargo capacity
    +--> camp rest capacity
    +--> radio / recovery / power capability
    +--> combat armor/weapon capability
    |
    v
Canonical systems resolve effects
    |
    +--> ExpeditionVehicleSystem
    +--> Inventory
    +--> Needs
    +--> ExpeditionSystem
    +--> TacticalCombat
    +--> Repair/Maintenance
    +--> Research
    +--> Shelter/Power
```

The vehicle customization layer owns configuration and capability declarations. It does not replace the
systems that consume them.

## 3. Non-Negotiable Rules

- `ExpeditionVehicleSystem` remains the canonical travel/logistics authority unless the premise audit finds a
  successor.
- Vehicle fuel uses the canonical fuel/resource representation.
- Vehicle cargo is canonical inventory/container state.
- Survivor rest/fatigue remains owned by `NeedsSystem`.
- Medical treatment remains owned by health/medical systems.
- Cooking in a vehicle uses the same cooking/food pipeline as E1-4 if that rail exists.
- Vehicle power generation/consumption uses canonical power/energy contracts.
- Vehicle repair and maintenance use canonical repair/workshop/job systems where possible.
- Vehicle combat is resolved by TacticalCombat or the canonical combat authority.
- Module installation is transactional and time/resource constrained.
- Modules may add capabilities and bounded modifiers; they cannot directly mutate unrelated state.
- Module slots/capacity are data-driven and validated.
- Installed modules survive save/load exactly once.
- Removing/replacing a module cannot duplicate resources.
- A “mobile base” is a temporary expedition camp context, not a second campaign/shelter save root.
- No vehicle may own survivor copies.
- No vehicle may own a second item database.
- No vehicle may own a second quest, research, combat, or needs subsystem.
- Destruction/loss must reconcile cargo, occupants, modules, and expedition state explicitly.
- Old saves with profile-only vehicles migrate deterministically.
- Vehicle inheritance across campaigns is governed by E1-5 and disabled by default unless an explicit
  continuation decision permits it.
- Mounted weapons, rams, flame weapons, or other combat modules are content/capabilities; safety/balance rules
  remain in combat authority.
- The first playable slice must ship with a small module set before 20-module breadth.

## 4. Acceptance Slices

### Slice A — Persistent vehicle identity
Stable instances, names, condition reference, cargo/fuel references, module slots.

### Slice B — Customization
Install/remove/replace a small set of modules with real resource/time costs.

### Slice C — Expedition support
Cargo, recovery, radio, sleeping/rest support, field repair or resupply.

### Slice D — Mobile camp
One temporary expedition camp anchored to one vehicle and expedition party.

### Slice E — Combat and convoy depth
Only after TacticalCombat and multi-vehicle ownership semantics are verified.

### Slice F — Content scale
Expand to 20 modules, quests/events, famous-vehicle history, and advanced support roles.

Do not start with all five categories simultaneously.


---

## E1-8A — Premise verification and vehicle-authority audit

**Goal:** Verify every existing vehicle, expedition, inventory, fuel, repair, combat, research, camp, and save rail before introducing customization state.

### Required substeps

1. Inspect `ExpeditionVehicleSystem`, `ExpeditionSystem`, vehicle profiles/data, inventory/container APIs, fuel consumption, breakdown logic, repair/maintenance systems, TacticalCombat, camp/dive systems, research, power, shelter defense, survivor needs, cooking/medical rails, caravan/convoy systems, and save sections.
2. Verify whether vehicle instances already exist or the current system only references profile IDs.
3. Verify whether fuel and condition are already persisted and where.
4. Verify whether cargo capacity maps to a real inventory container or only a numeric expedition capacity.
5. Verify whether expedition camps already provide sleeping/rest/resupply semantics that vehicle modules should extend.
6. Verify whether vehicles already participate in combat indirectly.
7. Search for module/equipment/upgrade frameworks that could be reused.
8. Search for generic installation/production/workshop jobs.
9. Verify research unlock ownership and catalog conventions.
10. Create `docs/systems/VEHICLE_AUTHORITY_MAP.md`.
11. Create intake duplicate-search evidence covering Plan 60, Plan 10, Plan 133, convoy/caravan rails, and any live mobile-camp code.
12. Set `PREMISE_VERIFIED_AT` to current HEAD and stop if a broader equipment/module system already owns customization.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8B — Vehicle customization ADR and ownership boundary

**Goal:** Define the smallest durable customization authority and prevent a second shelter/expedition simulator.

### Required substeps

1. Write an ADR comparing extension of `ExpeditionVehicleSystem`, a dedicated `VehicleCustomizationSystem`, and a generic equipment/module host if one exists.
2. Define vehicle-owned facts: stable vehicle instance ID, profile reference, custom name, installed module references, slot occupancy, module installation state if persistent, and perhaps condition if current vehicle authority already owns it.
3. Explicitly exclude cargo contents, survivor state, quest state, research state, combat state, shelter state, and external power-grid state.
4. Define mobile-camp state as expedition-owned temporary context referencing a vehicle.
5. Define module capability projection API.
6. Define which modifiers may be cached and which must be recomputed.
7. Define rollback and feature flags for living/combat modules independently.
8. Require second-tool ADR review before implementation.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8C — Persistent VehicleInstance identity and migration

**Goal:** Create stable vehicle instances from existing profile-based vehicles without duplicating current state.

### Required substeps

1. Define stable vehicle instance ID, profile ID, custom name, owner/availability context, installed modules, lifecycle status, and references to canonical fuel/cargo/condition stores.
2. Do not copy cargo contents into the instance DTO if inventory owns them.
3. Do not copy live expedition party into vehicle state.
4. Define lifecycle states such as Available, Assigned, InExpedition, Disabled, UnderRepair, Lost, Destroyed, Retired if each changes behavior.
5. Define deterministic default IDs for migrated legacy vehicles where possible.
6. Define old-save migration from profile-only state.
7. Ensure migration is idempotent.
8. Add tests for zero vehicles, one stock vehicle, multiple same-profile vehicles, named vehicle, assigned vehicle, and missing profile.
9. Define how removed vehicle profiles are handled.
10. Version the save schema from first release.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8D — Vehicle module catalog schema

**Goal:** Define data-driven module content with capabilities, slot requirements, prerequisites, cost, installation time, mass, and compatibility.

### Required substeps

1. Define module ID, category, localization key, compatible vehicle tags/profiles, slot type/count, mass/weight, cost bill, installation duration, required workstation/skill/research, granted capabilities, stat modifiers, maintenance burden, power/fuel demand where relevant, and removal policy.
2. Represent downstream effects as typed capabilities/modifiers rather than arbitrary script callbacks.
3. Validate every resource/item/research/workstation/capability reference.
4. Define incompatibility/exclusion groups.
5. Define prerequisite relationships and detect cycles.
6. Define module condition/damage semantics only if individual module durability is justified.
7. Start with 6-8 modules across utility/cargo/living/armor before authoring 20.
8. Add schema versioning and loader only if generic catalog infrastructure cannot be reused.
9. Add data-integrity tests for invalid references, impossible slots, circular prerequisites, negative costs, invalid modifier ranges, and duplicate IDs.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8E — Slot, mass, and compatibility model

**Goal:** Prevent every vehicle from becoming a maxed all-purpose platform.

### Required substeps

1. Define slot capacity by vehicle profile or chassis tags.
2. Prefer typed slot/capability constraints where useful: external, internal, roof, utility, weapon hardpoint, living volume.
3. Define mass/weight budget if vehicle speed/fuel systems can consume it.
4. Ensure module combinations cannot exceed cargo/weight/chassis constraints.
5. Define mutually exclusive combinations such as large trailer vs certain mobility modules if design requires.
6. Apply compatibility validation before installation starts.
7. Expose blocking reasons in the read model.
8. Add tests for valid install, full slots, wrong slot, mass overflow, incompatible pair, and profile mismatch.
9. Keep first slice simple enough that slot UI remains understandable.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8F — Transactional module installation

**Goal:** Install modules through canonical resource/workshop/time rails without instant stat mutation.

### Required substeps

1. Identify canonical production/workshop job API.
2. Validate ownership/availability of the target vehicle.
3. Validate resources, research, workstation, mechanic/worker eligibility, module compatibility, and vehicle status.
4. Reserve/consume resources transactionally.
5. Create stable installation operation ID.
6. Mark vehicle unavailable or partially available according to repair/workshop rules.
7. Advance time through canonical scheduler.
8. Install module only on authoritative completion.
9. Make completion idempotent across reload/retry.
10. Add tests for successful installation, missing resource, unavailable research, occupied vehicle, interrupted job, save/reload, and duplicate completion.
11. Do not let UI append module IDs directly.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8G — Module removal and replacement

**Goal:** Support reconfiguration without resource duplication or impossible mid-expedition changes.

### Required substeps

1. Define removal eligibility: shelter/workshop only by default unless field-removal is explicitly supported.
2. Define removal time/cost and salvage return through canonical inventory.
3. Do not guarantee full refund.
4. Define damage/wear impact on salvage.
5. Create stable removal/replacement operation IDs.
6. Validate slot state and vehicle availability.
7. Prevent simultaneous install/remove on same slot.
8. Add tests for remove, replace, insufficient destination capacity for salvaged parts, interrupted removal, and save/reload.
9. Ensure no module effect remains after authoritative removal completion.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8H — Capability projection service

**Goal:** Expose installed module effects to existing vehicle consumers without duplicating their calculations.

### Required substeps

1. Define a read-only projection from base profile + installed modules + condition into typed capabilities/modifiers.
2. Examples: cargo capacity modifier, travel speed modifier, fuel consumption modifier, breakdown modifier, rest capacity, radio capability, recovery capability, combat armor, combat weapon descriptors, power generation, refrigeration capability.
3. Keep canonical final calculations in the consuming system where possible.
4. Do not store duplicate 'finalSpeed' or 'finalCargo' unless cache invalidation is robust.
5. Invalidate/recompute when module set or condition changes.
6. Add deterministic projection tests.
7. Add conflicting modifier resolution policy.
8. Expose debug breakdown showing base + module + condition components.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8I — Cargo module integration

**Goal:** Increase carrying capability through canonical inventory/container ownership.

### Required substeps

1. Verify how cargo capacity is represented today.
2. Bind vehicle instance to a canonical cargo container or capacity context.
3. Apply cargo module capacity through inventory/container capability API.
4. Do not store a separate module-local cargo list.
5. Define over-capacity behavior when a module is damaged/removed.
6. Define trailer/roof-rack effects through capacity/mass/speed modifiers.
7. Define cargo accessibility at mobile camp.
8. Add tests for capacity increase, module removal with excess cargo, save/load, multiple vehicles, and inventory conservation.
9. Measure expedition carrying-value tradeoffs.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8J — Fuel and range integration

**Goal:** Make module weight/power demands affect canonical fuel/range calculations.

### Required substeps

1. Use existing fuel store and consumption authority.
2. Apply mass/aerodynamic/power module modifiers as inputs to fuel consumption.
3. Do not create a second 0-100 fuel percentage if a real unit-based fuel system exists.
4. Define stationary camp fuel use separately from travel fuel use.
5. Define refueling through canonical inventory/resource transactions.
6. Add tests for stock consumption, heavy module consumption, efficient module, stationary power use, empty fuel, and save/load.
7. Expose predicted range through authoritative calculator.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8K — Vehicle condition and maintenance

**Goal:** Integrate condition/degradation with existing breakdown/repair systems rather than duplicating health bars.

### Required substeps

1. Verify whether `ExpeditionVehicleSystem` already tracks breakdown/condition.
2. Define one vehicle condition authority.
3. Map travel distance, rough terrain, combat damage, overload, and breakdown events to condition changes through canonical policy.
4. Define module effectiveness degradation only if supported.
5. Use workshop/repair jobs for restoration.
6. Define spare-parts/resource costs.
7. Define disabled versus destroyed thresholds.
8. Add tests for degradation, repair, breakdown, disabled vehicle, save/load, and no double-damage.
9. Do not create customization-local condition state if vehicle logistics already owns it.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8L — Living module and rest-capability contract

**Goal:** Provide field rest through canonical camp/needs systems without creating a second shelter simulation.

### Required substeps

1. Define living modules as capabilities such as enclosed_sleep_slots, insulated_rest, weather_protection, privacy, water_access, or cooking_access.
2. Vehicle module state does not store survivor fatigue.
3. Expedition/camp authority assigns occupants to rest slots.
4. `NeedsSystem` resolves fatigue/warmth/rest effects.
5. Define capacity and sharing rules.
6. Define environmental/weather modifiers through existing expedition/weather systems.
7. Define condition/power/fuel prerequisites if relevant.
8. Add tests for rest with valid capability, overcapacity, damaged module, cold/weather exposure, unavailable occupant, and save/load mid-camp.
9. Do not duplicate shelter bed/bunk authority unnecessarily; reuse generic sleeping-capacity semantics if available.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8M — Mobile-camp state model

**Goal:** Anchor an expedition camp to a vehicle while keeping camp lifecycle owned by ExpeditionSystem.

### Required substeps

1. Audit existing camp events and lifecycle.
2. Represent vehicle camp as expedition camp context referencing vehicle ID, camp location/node, occupants, active duration, and enabled vehicle capabilities.
3. Do not create a second permanent site save tree.
4. Define establish/pack-up/abandon/lose transitions.
5. Require reachable/valid expedition location.
6. Define whether vehicle stays stationary while party explores.
7. Define how return-to-base/resupply uses existing expedition routing.
8. Add save/load tests for established camp, party away from camp, pack-up, and vehicle unavailable.
9. Keep camp history bounded.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8N — Camp resupply and return-point integration

**Goal:** Make the vehicle useful as a return/resupply point through canonical inventory and expedition routes.

### Required substeps

1. Expose vehicle cargo container to expedition resupply commands.
2. Do not teleport supplies between party and vehicle when distance/path rules say otherwise.
3. Define return-to-camp path and time.
4. Define medical/rest/food access from installed capabilities.
5. Define equipment swap if inventory rules permit it.
6. Ensure resources consumed at camp are removed canonically.
7. Add tests for resupply, insufficient cargo, distant party, blocked return, item transfer, and save/load.
8. Measure how mobile camp changes expedition endurance.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8O — Field cooking and water support

**Goal:** Reuse E1-4 food/cooking rails for kitchenette/water modules.

### Required substeps

1. Only enable kitchenette if cooking system/capability contract exists.
2. Expose heat/cook capability from vehicle module.
3. Use canonical cooking recipes, fuel, ingredients, and job state.
4. Do not create vehicle-specific recipes unless content requires them.
5. Water tank capacity uses canonical water/resource container.
6. Define refill through canonical resource transfer.
7. Add tests for cook-capable vehicle, no fuel, no water, damaged kitchenette, and inventory conservation.
8. Feature-flag this integration if food/cooking rails are not ready.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8P — Field medical support

**Goal:** Treat medical-bay modules as capability gates for existing medical treatment.

### Required substeps

1. Audit injury/treatment/medical station requirements.
2. Expose field_treatment or stabilization capability.
3. Use canonical medicine inventory and treatment actions.
4. Do not heal survivors directly from vehicle module state.
5. Define treatment limits compared with full shelter clinic.
6. Define power/equipment prerequisites where appropriate.
7. Add tests for eligible treatment, missing supplies, insufficient capability, damaged bay, and save/load.
8. Keep advanced surgery out of scope unless medical rails support it.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8Q — Radio and communication module

**Goal:** Integrate vehicle radio with existing radio/intel/information-flow systems.

### Required substeps

1. Audit radio tuner, signal coverage, weather intelligence, distress, and E1-3 information-flow rails.
2. Expose mobile radio capability and antenna/range modifiers.
3. Use canonical tuning/interception/communication rules.
4. Do not create a separate vehicle communications network.
5. Define power/fuel requirements.
6. Allow calling for help or shelter contact only through existing comms APIs.
7. Add tests for in-range, out-of-range, unpowered, damaged antenna, and save/load.
8. Allow information-flow claims to use vehicle radio as a channel only if E1-3 is active.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8R — Recovery, winch, crane, and utility capabilities

**Goal:** Make utility modules unlock bounded expedition interactions rather than generic stat bonuses.

### Required substeps

1. Define typed capabilities such as recover_vehicle, move_obstacle, heavy_load, tow, salvage_heavy.
2. Map each capability to an existing expedition/salvage/travel interaction.
3. Do not let the module directly complete quests or grant loot.
4. Define condition/fuel/time/worker requirements.
5. Create failure behavior through canonical expedition resolution.
6. Add tests for eligible obstacle, no capability, insufficient condition, towing, heavy cargo, and save/load.
7. Use utility modules as a high-value first-slice category.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8S — Refrigeration and preservation capability

**Goal:** Extend canonical food spoilage rather than creating a vehicle-specific perishability system.

### Required substeps

1. Only enable if canonical spoilage/perishability rails exist.
2. Expose refrigeration capability with temperature/power requirement if modeled.
3. Apply storage-condition modifier inside perishability authority.
4. Do not reset item age or freshness merely when moved into vehicle.
5. Define failure on power loss or damaged refrigerator.
6. Add tests for normal preservation, power loss, spoiled cargo, transfer between containers, and save/load.
7. Keep refrigeration as an advanced module if food systems are not yet accepted.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8T — Armor and defensive capability handoff

**Goal:** Expose vehicle protection to TacticalCombat without implementing damage resolution in customization code.

### Required substeps

1. Define armor descriptors: protection class, coverage, mass, condition interaction, occupant protection, weak points if combat model supports them.
2. Combat authority interprets armor.
3. Do not store separate combat HP in customization if vehicle condition is canonical elsewhere.
4. Define bulletproof glass as a capability/coverage modifier, not a hard-coded -50% damage in module code.
5. Define escape/smoke capability through combat actions.
6. Add tests proving combat receives the capability and owns resulting damage.
7. Keep armor penalties to speed/fuel as vehicle-logistics modifiers.
8. Balance with module mass and slots.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8U — Mounted weapon capability handoff

**Goal:** Add weapon hardpoints only through TacticalCombat's weapon/action model.

### Required substeps

1. Audit whether TacticalCombat supports vehicle actors/weapons.
2. Define mounted weapon descriptors referencing canonical weapon/ammo IDs.
3. Use canonical ammunition inventory.
4. Define crew/operator requirement if needed.
5. Define firing arcs/range/cooldown through combat authority.
6. Do not implement combat rolls in customization.
7. Add tests for weapon mounted, no ammo, no operator, destroyed vehicle, and save/load.
8. Keep flamethrower/area weapons out of first slice unless combat content already supports them.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8V — Ramming and vehicle-combat movement

**Goal:** Only implement vehicle-specific combat maneuvers if TacticalCombat can model them without a parallel resolver.

### Required substeps

1. Define ram as a combat action requiring speed/mass/position capability.
2. Combat authority resolves hit/damage/self-damage.
3. Vehicle condition authority receives resulting damage.
4. Define flee/escape modifiers through combat movement rules.
5. Do not calculate bespoke combat outside TacticalCombat.
6. Add tests for valid ram, blocked ram, self-damage, immobilized vehicle, and escape.
7. Defer if current combat model lacks vehicle actor support.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8W — Vehicle destruction, loss, occupants, and cargo reconciliation

**Goal:** Handle catastrophic failure explicitly across inventory, expedition, survivors, and save state.

### Required substeps

1. Define Destroyed/Lost/Abandoned semantics.
2. Resolve occupants through canonical combat/health/expedition systems.
3. Resolve cargo as destroyed, dropped, salvageable, or inaccessible according to existing item/world rules.
4. Resolve installed modules as destroyed/salvageable references.
5. Remove or disable the vehicle from future assignments.
6. Record loss event/journal/legacy provenance.
7. Prevent destroyed vehicle from reappearing after reload.
8. Add tests for empty vehicle destruction, occupied destruction, cargo loss, salvage, save/reload, and duplicate loss event.
9. Do not silently delete survivors or inventory.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8X — Research and blueprint unlock integration

**Goal:** Use canonical research/knowledge systems to gate advanced modules.

### Required substeps

1. Define module prerequisite references to real research IDs.
2. Research authority owns unlock state.
3. Customization reads eligibility only.
4. Do not duplicate an `availableModules` list in save state if it is derivable from research/catalogs.
5. Define salvage-found blueprints through canonical knowledge/unlock authority if supported.
6. Add tests for locked module, unlock, removed research ID, save/load, and old-save migration.
7. Allow first-slice modules to be ungated if research rail is not ready.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8Y — Vehicle ownership, assignment, and availability

**Goal:** Ensure vehicles have one owner/availability model across shelter, expedition, convoy, repair, and combat.

### Required substeps

1. Define whether ownership means shelter asset, faction asset, expedition-assigned asset, or survivor-associated asset.
2. Prefer campaign/shelter ownership plus temporary assignment.
3. Prevent the same vehicle from joining two expeditions.
4. Prevent customization while vehicle is away unless field modification is explicitly supported.
5. Prevent dispatch while under installation/repair.
6. Define transfer/sale only through canonical asset/economy authority.
7. Add tests for double assignment, repair conflict, installation conflict, lost vehicle, and return.
8. Do not use `ownerId` ambiguously for both legal ownership and current driver.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8Z — Convoy composition and multi-vehicle expedition handoff

**Goal:** Add convoy semantics only if expedition architecture supports more than one vehicle without rewriting travel.

### Required substeps

1. Audit current expedition party/vehicle cardinality.
2. Define convoy as expedition-owned list of assigned vehicle IDs if supported.
3. Aggregate speed/range/cargo using canonical convoy policy.
4. Define weakest-link and breakdown behavior.
5. Define inter-vehicle towing/recovery capability.
6. Define cargo distribution through actual containers.
7. Add tests for two vehicles, different speeds, one breakdown, one destroyed, towing, and save/load.
8. Feature-flag convoy support if current expedition contract is single-vehicle.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AA — Shelter-defense participation

**Goal:** Let parked vehicles contribute capabilities to shelter defense only through defense authority.

### Required substeps

1. Audit shelter-defense system and whether parked assets can register defenses.
2. Expose armor/weapon/mobile-cover capabilities from available parked vehicles.
3. Defense authority decides deployment, effectiveness, damage, and ammunition consumption.
4. Do not create shelter-defense modifiers directly in customization state.
5. Prevent vehicle simultaneously being on expedition and defending shelter.
6. Add tests for parked armed vehicle, absent vehicle, no ammo, destroyed vehicle, and save/load.
7. Defer if shelter-defense rail is not ready.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AB — Customization UI and authoritative read model

**Goal:** Create a vehicle panel that displays real configuration, capability, cost, availability, and blocking reasons.

### Required substeps

1. Audit current expedition/vehicle/inventory/workshop panels.
2. Prefer extending an existing vehicle/expedition asset surface.
3. Show base profile, custom name, condition, fuel, cargo capacity, installed modules, slot map, compatible modules, cost, installation time, research requirements, and derived capability preview.
4. Every Install/Remove/Replace action calls an authoritative command.
5. UI never edits module lists directly.
6. Show exact block reasons from authority.
7. Show tradeoffs such as mass/speed/fuel/capacity.
8. Add snapshot tests for stock vehicle, full slots, incompatible module, locked research, active installation, damaged vehicle, and mobile-camp capability.
9. Add keyboard navigation and confirmation for destructive replacements.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AC — Mobile-camp UI and expedition presentation

**Goal:** Expose camp capabilities through expedition UI rather than a separate shelter interface.

### Required substeps

1. Show assigned vehicle, camp status, occupants, rest slots, cargo/resupply, radio, medical/cooking capability, fuel/condition, and return path.
2. Use existing expedition map/encounter/camp navigation.
3. Do not present full shelter-management tabs for mobile camp.
4. Show unavailable actions with canonical reasons.
5. Add actions for Establish Camp, Pack Up, Rest, Resupply, Use Utility, Return to Vehicle only where authorities support them.
6. Add snapshot/integration tests for established camp, damaged module, no fuel, party away, and pack-up.
7. Keep presentation compact enough for expedition flow.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AD — Vehicle event and quest hooks

**Goal:** Add authored content after the persistent asset loop works.

### Required substeps

1. Define event hooks from vehicle state transitions: installation complete, breakdown, recovery, camp established, convoy attacked, vehicle lost, named vehicle milestone.
2. Use canonical event/quest system.
3. Prioritize source concepts such as Breakdown, Rescue, Salvage, Convoy, Extended Expedition.
4. Treat Race and Dream Machine as optional flavor after baseline.
5. Quest authority owns lifecycle/rewards.
6. Prevent duplicate triggers after save/load.
7. Add provenance IDs.
8. Add tests for event eligibility, dedupe, vehicle lost mid-quest, module removed, and completion.
9. Do not require 20 modules to make quests functional.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AE — Vehicle history, naming, fame, and legacy boundary

**Goal:** Make persistent vehicles memorable without converting reputation into hidden power or cross-campaign inheritance automatically.

### Required substeps

1. Allow custom names under vehicle identity.
2. Record major milestones: expeditions survived, rescues, breakdowns, losses, notable battles, long-distance travel.
3. Use bounded history/journal records.
4. Do not grant reputation bonuses by default.
5. If the legacy system records a famous vehicle, store narrative history/tag only.
6. Passing a vehicle across campaigns is disabled by default under E1-5 mechanical inheritance rules.
7. Within one campaign, vehicles naturally persist between expeditions.
8. Add tests for naming, history dedupe, loss, and legacy-record export.
9. Keep vehicle fame presentation-only unless separately approved.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AF — Old-save migration

**Goal:** Upgrade existing vehicle state to persistent instances without inventing duplicate vehicles, modules, fuel, or cargo.

### Required substeps

1. Identify exact old save shapes.
2. Create deterministic instance IDs from stable campaign/profile/index context when no instance ID exists.
3. Preserve existing fuel/condition/cargo semantics from canonical owners.
4. Install no modules by default.
5. Do not create free module inventory.
6. Handle multiple same-profile vehicles carefully.
7. Add migration version and idempotency tests.
8. Add fixtures for no vehicle, one vehicle, multiple vehicles, vehicle currently assigned to expedition, breakdown state, and missing profile.
9. Ensure old saves remain playable without opening customization UI.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AG — Save contract and state restoration

**Goal:** Persist customization state without duplicating expedition/inventory/combat state.

### Required substeps

1. Store instance identity and installed module references in the canonical vehicle section.
2. Persist active installation/removal operation state only if not already in generic production save.
3. Persist custom names and lifecycle availability flags as appropriate.
4. Do not persist copied cargo contents, fuel totals, survivor occupants, combat HP, research unlocks, or needs.
5. Rebuild capability projections on restore.
6. Validate module compatibility on load and surface degraded state if catalogs changed.
7. Add checksum/version tests.
8. Add round-trip tests for stock, customized, under-installation, mobile-camp-assigned, damaged, lost, and destroyed vehicles.
9. Ensure restore cannot duplicate module effects.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AH — Exploit and conservation audit

**Goal:** Prevent customization from generating resources, duplicating modules, bypassing travel costs, or cloning vehicles.

### Required substeps

1. Test install retry/double click.
2. Test save/reload mid-install.
3. Test removal/refund conservation.
4. Test replace operation under capacity pressure.
5. Test cargo over-capacity after module removal.
6. Test fuel consumption with mass modifiers.
7. Test module effects applied exactly once.
8. Test vehicle double assignment.
9. Test destroyed vehicle persistence.
10. Test salvage quest duplicate rewards.
11. Test camp resupply conservation.
12. Test research unlock cannot be bypassed by UI.
13. Add property/fuzz tests over module combinations if infrastructure supports it.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AI — Balance and dominance audit

**Goal:** Ensure modules create specialization and tradeoffs rather than one obvious best build.

### Required substeps

1. Measure speed, range, cargo, rest capacity, protection, utility, fuel cost, repair burden, and installation cost for representative builds.
2. Compare stock vehicle to cargo, expedition-support, armored, utility, and combat builds.
3. Enforce slot/mass/opportunity tradeoffs.
4. Check whether one module is mandatory for all missions.
5. Check whether living modules trivialize expedition attrition.
6. Check whether armor trivializes combat.
7. Check whether cargo modules trivialize scarcity.
8. Check whether solar/refrigeration removes fuel/spoilage pressure too cheaply.
9. Start with 6-8 modules, tune, then expand.
10. Record balance metrics and design decisions.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AJ — Performance and long-run vehicle soak

**Goal:** Prove persistent modules, camps, and vehicle histories remain cheap over a full campaign.

### Required substeps

1. Run 180-day headless simulation with multiple vehicles, repeated expeditions, installs/removals, breakdowns, repairs, camps, and cargo transfers.
2. Measure vehicle tick time, expedition travel cost, capability projection cost, save size, allocations, module-history growth, and UI read-model build time.
3. Ensure catalogs load once.
4. Ensure no per-frame full module catalog scans.
5. Bound vehicle event/history records.
6. Test 10+ vehicle instances and max module slots.
7. Record median/p95 vehicle-system cost.
8. Add practical regression thresholds.
9. Block convoy/module content expansion if performance becomes superlinear.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates salvage/resources.
- Vehicle cargo exists in both customization and inventory state.
- A mobile camp persists survivor need values independently.
- Combat damage is resolved twice by customization and TacticalCombat.
- One vehicle can be assigned to two expeditions.
- Reload recreates a destroyed vehicle or duplicates capability modifiers.
- A 20-module content set bypasses slot/mass tradeoffs.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Conservation/idempotency tests pass.
- [ ] Save/migration tests pass.
- [ ] Authority map remains correct.
- [ ] Headless scenario passes.
- [ ] Balance tradeoffs are measured.
- [ ] Performance/retention remains bounded.

---

## E1-8AK — Headless selftest and data-integrity validation

**Goal:** Create a deterministic end-to-end vehicle scenario for CI.

### Required substeps

1. Create a stock vehicle instance.
2. Install one cargo module through a real installation transaction.
3. Install one living/utility module.
4. Verify projected speed/cargo/fuel capability.
5. Assign vehicle to expedition.
6. Establish vehicle-anchored camp.
7. Rest one survivor through canonical needs.
8. Transfer cargo through canonical inventory.
9. Trigger one deterministic breakdown and repair path.
10. Save/reload mid-expedition and verify no duplicated modules/cargo/effects.
11. Validate module catalog references.
12. Create `--vehicle-customization-selftest` if project conventions support it.
13. Assert no second shelter/inventory/combat state exists in customization DTOs.

### Vehicle-integration invariants

- Customization owns module configuration; canonical systems own downstream state.
- Cargo, fuel, needs, combat, research, and shelter state are never duplicated.
- Install/remove/replace operations are transactional and idempotent.
- Mobile camp is expedition state, not a second campaign save root.
- All module effects are typed capabilities/modifiers with explicit consumers.
- Save/reload cannot apply a module effect twice.
- High-risk combat/convoy/mobile-base depth remains feature-gated.
- Every persistent collection has a retention rule.

### Negative tests

- A module is installed twice from one completion event.
- Removing a module duplicates s

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
