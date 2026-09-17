---
PLAN_ID: E1-17
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 17
STATUS: READY_FOR_EXECUTION_WHEN_SHELTER_COMPONENT_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 186 — Shelter Maintenance & Degradation System"
SEQUENCE_FILENAME: "E1_planintegration[17].md"
PREVIOUS_FILENAME: "E1_planintegration[16].md"
NEXT_FILENAMES:
  - "E1_planintegration[18].md"
  - "E1_planintegration[19].md"
CATEGORY: LINK+SHELTER+MAINTENANCE+DEGRADATION+INFRASTRUCTURE
PRIMARY_INTENT: "Make shelter infrastructure age, wear, fail, and require maintenance through persistent component condition and real work/resource transactions, while keeping Thermal, Power, Water, Ventilation, Radiation, Inventory, Duty, Construction, and Disaster systems authoritative for their own state and consequences."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_POWER_GRID_FORBIDDEN: true
SECOND_WATER_SYSTEM_FORBIDDEN: true
SECOND_VENTILATION_SYSTEM_FORBIDDEN: true
SECOND_RADIATION_SYSTEM_FORBIDDEN: true
SECOND_STRUCTURAL_SIMULATION_FORBIDDEN: true
AUTHORITATIVE_OVERALL_SHELTER_CONDITION_FORBIDDEN: true
RNG_FOR_BASE_WEAR_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: HIGH
MICROMANAGEMENT_RISK: VERY_HIGH
CASCADE_RISK: VERY_HIGH
---

# E1 Plan Integration [17] — Shelter Maintenance, Component Wear, Inspection, Repair, Replacement, Failure Handoffs, and Infrastructure Aging

> **Sequence rule:** this file is `E1_planintegration[17].md`.
> The next files are `E1_planintegration[18].md`, `E1_planintegration[19].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 186 into an implementation-grade shelter-maintenance programme.

The source plan identifies a meaningful systemic gap: ASHFALL already has shelter thermal behavior, power,
water treatment, ventilation, radiation protection, armor/structural concepts, weather cascades, and acute
disaster plans, yet the shelter itself can remain mechanically static between crises. Narrative references to
filters wearing out, walls cracking, and equipment aging are not enough if nothing consumes parts, labor, time,
or maintenance attention.

The right implementation is **not** a monolithic `ShelterMaintenanceSystem` that owns a parallel copy of
power health, air quality, water quality, radiation leakage, wall stability, generator output, and shelter
integrity.

The maintenance layer should own only the persistent physical condition of maintainable assets that do not
already have a canonical condition owner, plus maintenance work orchestration and inspection knowledge where
those facts are not owned elsewhere. Downstream systems consume component capability/condition and resolve
their own consequences.

The core rule is:

**maintenance owns wear of the component; the component's domain system owns what degraded capability means.**

A dirty filter can reduce ventilation/radiological filtration capability. `VentilationSystem` and the
radiation/contamination authority decide the resulting air/exposure state. A worn generator may reduce
available generation or trip offline. `PowerGridSystem` still owns current power balance. A damaged wall can
reduce thermal resistance or protection. `ShelterThermalSystem`, structural/armor authority, and
RadiationSystem resolve the consequences.

## 1. Source Intent Preserved

Plan 186 asks for:

- shelter-specific component degradation;
- air-filter condition;
- wall integrity;
- generator wear;
- water-recycler maintenance;
- blast-door wear;
- ventilation wear;
- structural degradation;
- radiation-shielding degradation;
- inspection;
- cleaning;
- repair;
- replacement;
- spare parts;
- emergency repair;
- failure events;
- cascade failures;
- maintenance scheduling;
- data-driven components;
- UI and warnings;
- save/load;
- old-save compatibility;
- deterministic behavior;
- cross-system integration;
- headless selftest.

E1-17 preserves those goals while correcting ownership, determinism, cascade, and micromanagement risks.

## 2. Core Architecture Thesis

```text
Shelter component instance
    |
    +--> stable component ID
    +--> component definition ID
    +--> canonical condition/wear
    +--> maintenance knowledge/inspection state
    +--> active maintenance-operation refs
    |
    v
Condition capability projection
    |
    +--> filtration efficiency/capacity input
    +--> generator availability/derating input
    +--> recycler efficiency/capacity input
    +--> door sealing/security input
    +--> insulation/protection input
    +--> ventilation capacity input
    |
    v
Canonical domain systems
    |
    +--> VentilationSystem
    +--> PowerGridSystem
    +--> WaterTreatmentSystem
    +--> ShelterThermalSystem
    +--> Radiation/Contamination
    +--> ShelterDefense / structural authority
    +--> DisasterResponse
```

Maintenance determines whether an asset is healthy, worn, degraded, failed, temporarily patched, or replaced.
It does **not** own the resulting current air quality, power deficit, clean-water amount, survivor dose, room
temperature, or defense outcome.

## 3. Architectural Corrections to the Source Plan

### 3.1 Base wear should be deterministic

The source plan asks for `ISeededRng` for degradation. Ordinary wear does not need randomness. Runtime hours,
particulate load, contamination load, cycles, temperature stress, age, and explicit disaster damage can
produce deterministic wear.

RNG may be used only for an explicitly authored stochastic failure/accident policy where deterministic keyed
variation improves gameplay. The baseline system should be zero-RNG.

### 3.2 No authoritative “overall shelter condition”

A single shelter-wide condition number would collapse unrelated engineering systems and become a second
authority. A UI may compute a derived maintenance-health summary, but the simulation should not store or use
one generic global condition unless an existing shelter authority already owns such a metric.

### 3.3 Failures should hand off capability loss

A failed filter does not directly “damage survivors.” It reduces filtration capability; ventilation,
radiation, air-quality, and health systems produce consequences. A failed generator does not directly set
every system off; PowerGrid owns the blackout. This prevents duplicate cascades.

### 3.4 Daily blanket degradation is not required

Wear should be driven by relevant usage/environment units. Daily settlement may aggregate accumulated runtime
or exposure, but generator wear should reflect runtime/load; blast-door wear should reflect cycles and events;
filters should reflect particulate/contaminant throughput; structural elements should reflect damage,
moisture/thermal stress, and aging policy.

### 3.5 Inspection should not invent false precision

If exact condition is hidden until inspection, the system needs an explicit knowledge model. A low-skilled
inspection should not silently mutate true condition. It can reveal a band, confidence interval, or
diagnostic certainty. If the game does not benefit from hidden condition, simplify: always show condition and
make inspection a preventive-maintenance action rather than forced UI opacity.

## 4. Non-Negotiable Rules

- One stable component ID per maintainable shelter asset.
- One canonical condition owner per component.
- Existing subsystem-owned condition must be reused, not shadowed.
- Maintenance may own condition only where no more specific system already does.
- PowerGrid owns current generation, consumption, circuit state, brownouts, and blackouts.
- WaterTreatment owns water treatment/quality/throughput.
- Ventilation owns airflow/air quality/ventilation state.
- Radiation/contamination owns environmental and survivor exposure.
- ShelterThermal owns room/shelter thermal state.
- Defense/structural authority owns combat damage/structural safety consequences where present.
- DisasterResponse owns acute disaster orchestration.
- E1-9 construction/renovation owns creation/upgrading of shelter spaces and infrastructure installation.
- Inventory owns spare parts and repair materials.
- Duty/job scheduler owns worker assignment and elapsed work.
- Skills own worker competence/XP.
- Tools/equipment authority owns tool condition.
- Maintenance operations are transactional and idempotent.
- Base degradation is deterministic.
- No condition is reduced twice by the same source event.
- No failure consequence is applied twice by maintenance and downstream system.
- Maintenance cannot create/delete inventory outside canonical transactions.
- Replacement cannot clone components or parts.
- Temporary repair must be distinguishable from permanent restoration.
- Emergency repair cannot become a cheaper permanent repair exploit.
- Old saves start from a documented baseline without retroactive decades/months of hidden wear.
- Save/reload cannot replay failures, repairs, or material consumption.
- Time skip applies wear exactly once.
- Maintenance history is bounded.
- Alerts are threshold/milestone driven, not daily spam.
- The first release should use a small representative component set before 12+ components.
- Maintenance burden must be tuned as strategic prioritization, not routine clicking.

## 5. Acceptance Slices

### Slice A — Component registry and deterministic condition
Represent 4–6 core maintainable assets with stable identity and condition.

### Slice B — Preventive maintenance
Inspect/clean/service through real labor, parts, tools, and time.

### Slice C — Failure handoff
One filter, one power component, and one water/ventilation component fail into their canonical systems.

### Slice D — Repair/replacement
Transactional repair, temporary patch, and replacement with save/load safety.

### Slice E — Cascades and broad content
Only after cross-system failure handoffs are proven not to double-apply effects.

Do not author 12–20 components before the first vertical slice passes.


---

## E1-17A — Premise verification and maintenance-authority audit

**Goal:** Verify every shelter subsystem's existing condition, failure, repair, upgrade, damage, save, and component identity rails before adding maintenance state.

### Required substeps

1. Inspect all `Assets/Ashfall.Core/Shelter/` systems, `ShelterThermalSystem`, `SkyLayerArmorSystem`, `PowerGridSystem`, `WaterTreatmentSystem`, `VentilationSystem`, radiation/contamination, disaster response, weather cascades, E1-9 shelter expansion/renovation, duty/job scheduler, inventory, crafting/workshop, skill progression, tool condition, UI, and save registry.
2. Search for existing durability, condition, health, integrity, wear, service, maintenance, repair, fault, outage, breaker, filter, pump, generator, door, seal, structural, and replacement state.
3. Determine whether generators, filters, pumps, doors, walls, or ventilation equipment already have canonical runtime identities.
4. Determine whether any subsystem already persists component condition.
5. Determine whether failure states already exist in PowerGrid, WaterTreatment, Ventilation, or structural systems.
6. Determine whether repair jobs already exist as a generic work type.
7. Map each source-plan proposed component and consequence to one canonical owner.
8. Identify narrative-only mentions that have no runtime object.
9. Create `docs/systems/SHELTER_MAINTENANCE_AUTHORITY_MAP.md`.
10. Create intake duplicate-search evidence linking Plans 23, 29, 135, 158, E1-9/Plan 156, E1-11 climate, and any repair/workshop plans.
11. Set `PREMISE_VERIFIED_AT` to current HEAD and stop if a component already has a different canonical condition authority.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17B — Maintenance ownership ADR

**Goal:** Define whether condition belongs to a generic shelter-maintenance component registry or remains distributed among subsystem assets.

### Required substeps

1. Write an ADR comparing centralized condition ownership, subsystem-owned condition with a maintenance coordinator, and hybrid typed component registry.
2. Prefer subsystem-owned condition where a real asset already exists; use maintenance-owned condition only for assets lacking an owner.
3. Define one `IMaintainableAsset`/capability contract or equivalent if architecture benefits.
4. Define stable component identity separate from component definition.
5. Define maintenance operation ownership.
6. Explicitly exclude current power, water quality, airflow, radiation level, room temperature, survivor effects, structural combat outcome, and inventory.
7. Define feature flags per component family.
8. Define rollback.
9. Require second-tool architecture review.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17C — Stable maintainable-asset identity

**Goal:** Give every maintained component an unambiguous persistent identity without duplicating subsystem asset identity.

### Required substeps

1. Reuse existing subsystem asset ID when present.
2. For authored shelter components without identity, create deterministic stable IDs derived from shelter layout/component slot definitions.
3. Define component definition/profile ID separately.
4. Define lifecycle states such as Installed, Serviceable, Degraded, Failed, TemporarilyPatched, Removed, Replaced only where state is not derivable from condition and operation status.
5. Do not create duplicate IDs for E1-9 installed upgrades.
6. Handle old component removal/replacement with stable provenance.
7. Add uniqueness/reference tests.
8. Document ID conventions.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17D — Component definition catalog schema

**Goal:** Create data-driven maintenance definitions that describe wear and service requirements without embedding downstream simulation effects.

### Required substeps

1. Define definition ID, component family/type, localization keys, maintenance owner/consumer, wear-policy ID, warning thresholds, failure threshold/policy, service actions, required capability/tool/skill tags, replacement item/blueprint reference, and compatibility/location tags.
2. Do not define arbitrary `effects: [power_off, radiation_damage]` lists.
3. Reference typed downstream capability adapters.
4. Do not store current condition in catalog data.
5. Validate item/skill/tool/subsystem references.
6. Validate thresholds.
7. Validate each enabled component has exactly one condition owner.
8. Version catalog semantics.
9. Start with 4–6 components.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17E — Component-instance condition contract

**Goal:** Define one persistent condition representation per asset with explicit units and no duplicate subsystem state.

### Required substeps

1. Decide condition scale: integer basis points, 0–100 integer, or subsystem-specific engineering state.
2. Prefer integer/fixed-point representation for deterministic persistence.
3. Define condition maximum/minimum.
4. Define accumulated wear counters only if required by the wear policy.
5. Define service/inspection metadata separately.
6. Do not store derived warning color/state if it can be computed.
7. Do not store downstream output values.
8. Define condition mutation API with reason/provenance ID.
9. Add boundary/round-trip tests.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17F — Zero-RNG deterministic wear model

**Goal:** Make ordinary degradation a deterministic function of relevant accumulated usage and environment.

### Required substeps

1. Define `wear_delta = policy(runtime, load, cycles, environment, age, damage inputs)` using fixed-point arithmetic.
2. Do not call `ISeededRng` for routine daily wear.
3. Use source-specific accumulated inputs.
4. Define rounding and carry/remainder handling so low rates do not vanish.
5. Clamp condition.
6. Make repeated evaluation over the same interval idempotent.
7. Add stepped-versus-bulk equivalence tests.
8. Document units.
9. Reserve RNG only for explicitly accepted stochastic failures.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17G — Wear-source provenance

**Goal:** Prevent weather, use, and disaster damage from being applied twice.

### Required substeps

1. Define wear source IDs/classes: runtime, load, particulate, contamination throughput, open-close cycle, thermal stress, moisture, explicit disaster damage, combat damage, aging baseline.
2. Every submitted wear delta includes stable provenance or interval key.
3. Do not let maintenance both observe a weather cascade result and independently re-derive the same acute damage.
4. Differentiate gradual weather stress from acute disaster damage.
5. Add dedupe tests.
6. Add same-tick multi-source ordering tests.
7. Keep source ownership documented.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17H — Usage-meter integration

**Goal:** Use actual subsystem usage rather than blanket daily rates where possible.

### Required substeps

1. Power assets consume runtime/load telemetry from PowerGrid.
2. Water-treatment assets consume processed-volume/contaminant-load telemetry from WaterTreatment.
3. Ventilation/filter assets consume runtime/airflow/particulate-load telemetry from Ventilation/Weather.
4. Blast doors consume operation-cycle and acute impact events.
5. Structural assets consume explicit damage/stress summaries rather than fake daily use.
6. Define periodic aggregation if raw telemetry is too granular.
7. Add usage-meter tests.
8. Do not tick unused equipment as if operating at full load.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17I — Environmental wear integration

**Goal:** Apply gradual environment stress from canonical weather/climate state without creating a parallel weather model.

### Required substeps

1. Consume realized weather/environment state from WeatherSystem.
2. Consume E1-11 climate only indirectly through realized conditions or explicit long-term stress inputs.
3. Define particulate/dust, moisture, freeze-thaw, heat, wind/storm, corrosion, and contamination inputs only where real systems expose them.
4. Do not invent radiation-induced material erosion universally; define it per material/component if gameplay chooses the abstraction.
5. Separate chronic environmental wear from acute disaster damage.
6. Add tests for mild, storm, dusty, wet, freeze-thaw, and no-input cases.
7. Keep coefficients data-driven.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17J — Air-filter wear vertical slice

**Goal:** Implement one filter component end-to-end through real ventilation/radiation/air-quality consumers.

### Required substeps

1. Identify the actual filter concept in Ventilation/Radiation systems.
2. Define filter condition owner.
3. Use airflow/particulate/contamination throughput as wear inputs.
4. Define service actions: inspect, clean if physically meaningful, cartridge replacement.
5. Project filtration capability from condition.
6. Ventilation/Radiation authority resolves resulting air/exposure state.
7. Do not directly apply survivor dose from maintenance.
8. Add failure/recovery/save tests.
9. Use this as first life-support vertical slice.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17K — Filter-stage composition policy

**Goal:** Add pre/main/HEPA stages only if the current ventilation architecture can represent staged filtration meaningfully.

### Required substeps

1. Do not create three bars solely for UI complexity.
2. Define each stage's actual function/capability.
3. Define bypass/series behavior.
4. Define how one failed stage changes effective filtration through Ventilation.
5. Define independent replacement items only if inventory/content exists.
6. Add tests for one degraded stage, one failed stage, all healthy, and replacement.
7. Defer staged filtering if one canonical filter abstraction is sufficient.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17L — Power-generator maintenance integration

**Goal:** Model generator wear without duplicating PowerGrid availability/output truth.

### Required substeps

1. Identify generator runtime/load assets and IDs.
2. Use runtime/load/start-stop cycles as wear inputs.
3. Project derating/availability/failure state into PowerGrid through a typed adapter.
4. PowerGrid owns actual generation and blackout/brownout.
5. Define maintenance shutdown semantics.
6. Define service/repair parts.
7. Add tests for worn derating if supported, failed generator, grid fallback, repair, replacement, save/load.
8. Do not directly set shelter power state.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17M — Water-recycler maintenance integration

**Goal:** Model recycler/filter/pump wear through WaterTreatment.

### Required substeps

1. Identify actual treatment stages/assets.
2. Use processed volume and contamination load as wear inputs.
3. Project throughput/efficiency/failure capability.
4. WaterTreatment owns water quality and clean-water production.
5. Define membrane/filter/pump service actions only where actual item/content rails exist.
6. Add tests for degraded throughput, failure, bypass if supported, repair, replacement, save/load.
7. Do not directly create contamination or dehydration consequences.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17N — Ventilation-unit maintenance integration

**Goal:** Model fan/bearing/duct/filter condition without duplicating air-flow state.

### Required substeps

1. Identify ventilation units/duct nodes.
2. Use runtime and particulate load.
3. Project available airflow/capacity/seal condition.
4. VentilationSystem owns actual airflow and air-quality outcome.
5. Define fan-service/duct-repair actions.
6. Handle power-off separately from mechanical failure.
7. Add tests for unpowered healthy fan versus powered failed fan.
8. Do not conflate power outage with equipment degradation.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17O — Blast-door and seal maintenance

**Goal:** Model door mechanism/seal wear as capabilities consumed by security/thermal/radiation systems.

### Required substeps

1. Identify blast-door/entrance authority.
2. Use open-close cycles, impact events, contamination/dust, and age policy as inputs.
3. Separate mechanical actuator condition from seal condition if gameplay warrants it.
4. Project seal/security/operability capability.
5. Defense/thermal/radiation systems resolve consequences.
6. Define emergency manual operation if supported.
7. Add tests for worn seal, jammed actuator, power loss, manual operation, repair, replacement.
8. Do not create generic security-breach state locally.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17P — Wall/ceiling/floor condition boundary

**Goal:** Represent structural envelope wear only where a structural/armor topology can consume it.

### Required substeps

1. Audit SkyLayerArmor, shelter room topology, defense, disaster, and E1-9 structural support.
2. Do not create six arbitrary wall bars if no location-based consequence can use them.
3. Reuse structural section IDs where they exist.
4. Separate thermal insulation condition from load-bearing structural integrity if necessary.
5. Acute combat/disaster damage belongs to their authorities and may submit condition damage.
6. Project thermal/protection/seal capability to consumers.
7. Add tests for section damage, repair, breach handoff, and save/load.
8. Keep full structural engineering simulation out of scope.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17Q — Radiation-shielding maintenance boundary

**Goal:** Avoid physically dubious generic 'radiation shielding erosion' while supporting explicit damaged-barrier mechanics.

### Required substeps

1. Audit shielding/armor representation.
2. Ordinary passive lead/concrete shielding should not automatically lose radiological effectiveness simply from being exposed to radiation unless the game intentionally abstracts material damage.
3. Model degradation from cracking, spalling, moisture/corrosion, impact, seal failure, or removed material rather than dose alone where appropriate.
4. Project protection capability to RadiationSystem.
5. Do not own survivor dose.
6. Document gameplay abstraction.
7. Add tests for intact, physically damaged, repaired shielding.
8. Remove daily radiation-only erosion if unsupported.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17R — Structural-support/beam maintenance

**Goal:** Integrate load-bearing component condition only if E1-9 structural rules can consume it.

### Required substeps

1. Reuse support-beam/structural-node identities.
2. Use age, moisture, overload, disaster/impact, and construction-quality inputs.
3. Define inspection/repair/support replacement.
4. Structural authority decides unsafe/collapse state.
5. Maintenance does not collapse rooms directly.
6. Add tests for degraded support, warning, structural authority response, repair, save/load.
7. Feature-gate if structural rail is not ready.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17S — Preventive inspection contract

**Goal:** Define inspection as knowledge/diagnostic work, not an arbitrary condition bonus.

### Required substeps

1. Decide whether component true condition is always known or partially hidden.
2. If always known, inspection can reveal fault details/prognosis and count as preventive maintenance only if supported.
3. If hidden, store inspection knowledge state separately from true condition.
4. Define skill effect on precision/confidence, not true condition.
5. Define duration/tool requirements.
6. Use duty/job scheduler.
7. Do not let repeated inspection restore condition.
8. Add tests for exact/estimated/unknown condition if enabled.
9. Keep UI fair enough to avoid blind failure.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17T — Maintenance action taxonomy

**Goal:** Define inspect, clean/service, repair, temporary patch, and replace as distinct transactions.

### Required substeps

1. Inspect changes knowledge only.
2. Clean/service reduces accumulated fouling/wear only for components where that operation is meaningful.
3. Repair restores bounded condition and may require parts.
4. Temporary patch restores limited capability/condition with expiry or lower max if supported.
5. Replace installs a new component instance/condition state according to asset identity policy.
6. Do not use one generic `conditionRestored` field for every action without a policy consumer.
7. Define action eligibility and completion policy in data.
8. Add action-type tests.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17U — Maintenance job scheduling

**Goal:** Schedule work through canonical Duty/job infrastructure.

### Required substeps

1. Validate component availability and maintenance lock.
2. Validate worker eligibility/skill.
3. Validate tools.
4. Validate parts/materials.
5. Validate safe shutdown/isolation if required.
6. Reserve resources and target.
7. Create stable maintenance operation ID.
8. Advance through game-time scheduler.
9. Resolve once.
10. Release locks/resources on cancel/failure according to policy.
11. Add concurrency/save/reload tests.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17V — Skill and workmanship integration

**Goal:** Use canonical skills to affect duration, diagnostics, repair quality, or failure risk without owning XP.

### Required substeps

1. Audit engineering/mechanics/electrical/medical/construction skills.
2. Map action requirements to real skill IDs.
3. SkillProgression owns XP.
4. Prefer skill affecting time/quality bounds rather than a flat random success roll.
5. Remove universal `successChance` if deterministic workmanship is sufficient.
6. If stochastic repair quality is retained, use keyed deterministic RNG and explicit policy.
7. Add skill boundary tests.
8. Prevent skill from exceeding condition caps.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17W — Tool/equipment integration

**Goal:** Use canonical tools and tool wear rather than treating tools as free boolean requirements.

### Required substeps

1. Audit wrench/toolkit/welder/filter tools/diagnostic equipment.
2. Reference tool capability IDs.
3. Reserve unique tools if job scheduler supports it.
4. Apply tool wear through equipment authority.
5. Handle tool failure mid-job.
6. Do not duplicate tool condition.
7. Add tests for missing/broken/shared tool.
8. Defer specialized tool content until basic maintenance works.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17X — Spare-parts inventory authority

**Goal:** Consume real parts through canonical inventory/crafting/salvage systems.

### Required substeps

1. Audit existing spare-part items before creating new ones.
2. Prefer generic component classes where item proliferation would add little gameplay.
3. Define filter cartridges, seals, bearings, coils, membranes, patches, replacement units only when distinct logistics choices matter.
4. Reserve/consume parts transactionally.
5. Crafting owns production recipes.
6. Salvage owns acquisition.
7. Do not create maintenance-local part counts.
8. Add item-reference/inventory conservation tests.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17Y — Repair transaction

**Goal:** Restore component condition/capability through a stable idempotent completion transaction.

### Required substeps

1. Validate target condition and active operation.
2. Consume committed materials exactly once.
3. Apply repair policy through component condition owner.
4. Clamp restoration.
5. Record repair provenance.
6. Notify downstream adapter of condition/capability change.
7. Emit significant repair event only after commit.
8. Do not replay on load.
9. Add interruption/retry/double-completion tests.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17Z — Replacement transaction

**Goal:** Replace failed/worn equipment without duplicating component identities or inventory.

### Required substeps

1. Define whether component instance ID remains stable while installed unit serial/definition changes, or whether a new asset ID is created and slot identity remains stable.
2. Use E1-9 infrastructure slot identity where available.
3. Reserve replacement item.
4. Require downtime/labor/tools.
5. Consume item once.
6. Archive old component history as bounded provenance.
7. Initialize new condition deterministically.
8. Re-register downstream capability once.
9. Add save/reload and duplicate-replacement tests.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AA — Temporary and emergency repair policy

**Goal:** Provide crisis recovery without making emergency repair the optimal permanent maintenance strategy.

### Required substeps

1. Define patchable component families.
2. Use faster labor/material recipe.
3. Cap restored condition or capability.
4. Apply reduced maximum condition, decay multiplier, expiry, or mandatory follow-up only if the model can represent it clearly.
5. Do not use random hidden failure every hour as punishment.
6. Prevent repeated patch stacking.
7. Add tests for one patch, repeated patch attempt, permanent repair after patch, and save/load.
8. Measure emergency-repair value/cost.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AB — Maintenance shutdown and service isolation

**Goal:** Represent the fact that some equipment must be taken offline to service it.

### Required substeps

1. Define required operational state for each action.
2. Coordinate shutdown through the owning subsystem.
3. Power generator service requests PowerGrid asset offline state.
4. Water/ventilation service requests their operational authority.
5. Do not directly set global outputs to zero.
6. Define bypass/redundancy behavior.
7. Add tests for refusal when shutdown unsafe, redundant unit, service completion, abort, and restore.
8. Expose downtime forecast in UI.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AC — Failure threshold and state transition

**Goal:** Make component failure an idempotent capability transition, not a bundle of direct cross-system mutations.

### Required substeps

1. Define warning threshold(s) and failure threshold per component policy.
2. On threshold crossing, mark/notify the canonical component condition owner.
3. Publish `MaintainableAssetDegraded` / `MaintainableAssetFailed` with stable event ID.
4. Downstream system consumes condition/capability.
5. Do not apply radiation/blackout/dehydration directly.
6. Prevent repeated failure event while already failed.
7. Define recovery transition on repair.
8. Add threshold-crossing tests.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AD — Failure consequence adapters

**Goal:** Connect degraded components to domain systems with typed, one-way capability adapters.

### Required substeps

1. Filter -> Ventilation/Radiation filtration capability.
2. Generator -> PowerGrid generation/availability capability.
3. Recycler -> WaterTreatment throughput/treatment capability.
4. Vent unit -> Ventilation airflow capability.
5. Door/seal -> Defense/Thermal/Radiation seal/operability capability.
6. Wall/structural -> Thermal/Defense/Structural protection capability.
7. Each adapter has one owner and explicit input/output.
8. Add negative tests proving maintenance never writes downstream current-state values.
9. Support adapter feature flags.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AE — Cascade ownership and causality

**Goal:** Allow real cascades to emerge from canonical systems instead of hard-coded maintenance chains.

### Required substeps

1. Example: failed generator reduces PowerGrid generation.
2. PowerGrid may cause ventilation power loss.
3. Ventilation then reduces airflow.
4. Air-quality/health authority may affect survivors.
5. Maintenance records only the initiating component failure and any separate component wear/damage events.
6. Do not implement `generator failed => ventilation failed => survivors damaged` as one maintenance action.
7. Use provenance/correlation IDs for diagnostics.
8. Add integration test showing cascade emerges exactly once.
9. Document causality trace.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AF — Acute disaster integration

**Goal:** Separate gradual degradation from earthquake/flood/storm/attack damage.

### Required substeps

1. DisasterResponse/Defense computes acute damage.
2. Submit component condition damage through canonical maintenance/asset API with source event ID.
3. Do not also derive the same damage from daily weather wear.
4. Allow pre-existing low condition to worsen disaster vulnerability only through an explicit consumer policy.
5. Add tests for healthy versus worn component in same disaster if supported.
6. Add duplicate-source guard.
7. Keep disaster event authority external.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AG — Weather and climate integration

**Goal:** Use weather/climate as wear inputs without duplicating the weather cascade.

### Required substeps

1. Consume realized storm/dust/moisture/freeze-thaw metrics.
2. E1-11 long-horizon climate changes frequency/intensity of those conditions indirectly.
3. Define accumulated stress counters if needed.
4. Do not create separate storm probabilities.
5. Do not directly trigger weather damage from maintenance.
6. Add long-run mild versus severe climate wear tests.
7. Keep coefficients bounded.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AH — Thermal integration

**Goal:** Let envelope/component condition affect thermal performance through ShelterThermal.

### Required substeps

1. Map wall/door/insulation condition to thermal resistance/leakage capability.
2. ShelterThermal owns actual heat loss and room temperature.
3. Do not store thermal-loss values as maintenance truth unless derived.
4. Define condition-response curve.
5. Add tests for healthy/damaged/repaired envelope.
6. Ensure E1-9 insulation upgrades compose correctly.
7. Prevent double modifier application.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure handoffs and cascades resolve once.
- [ ] Player-burden/economy targets pass.
- [ ] Performance and observability evidence is captured.

---

## E1-17AI — Radiation ingress integration

**Goal:** Route physical barrier/filter degradation to the radiation authority.

### Required substeps

1. Identify actual shelter-shielding/exposure model.
2. Map filter/seal/wall capability into ingress calculation.
3. Radiation authority owns environmental/survivor dose.
4. Do not emit direct survivor radiation damage from maintenance.
5. Handle repaired capability immediately according to radiation model.
6. Add tests for filter failure, wall/seal degradation, repaired barrier, and existing ambient radiation.
7. Keep contamination versus dose distinct.

### Maintenance invariants

- Each component has one condition owner.
- Maintenance owns condition/work orchestration only where a more specific subsystem does not already own it.
- Power, water, ventilation, thermal, radiation, structural, inventory, duty, and disaster state remain canonical elsewhere.
- Base wear is deterministic and provenance-tracked.
- Repairs/replacements consume real resources exactly once.
- Failures alter capabilities; downstream systems own consequences.
- Old saves begin from a fair prospective maintenance baseline.
- Maintenance frequency and UI burden are explicitly bounded.

### Negative tests

- Maintenance stores a copied current power/air/water/radiation state.
- The same storm/disaster damages a component twice.
- A generator failure directly disables ventilation from maintenance code.
- A filter failure directly applies survivor radiation dose.
- Reload duplicates repair material consumption.
- Emergency patch can be stacked into a cheaper permanent replacement.
- Old saves instantly fail components based only on campaign day.
- A component with an existing subsystem condition gets a second maintenance condition field.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Deterministic wear/time-skip tests pass.
- [ ] Save/migration/idempotency tests pass.
- [ ] Material/tool/labor conservation passes.
- [ ] Failure han

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
