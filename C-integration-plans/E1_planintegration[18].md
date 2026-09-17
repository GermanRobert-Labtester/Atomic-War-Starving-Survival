---
PLAN_ID: E1-18
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 18
STATUS: READY_FOR_EXECUTION_WHEN_WATER_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 189 — Water Source Management & Contamination Network"
SEQUENCE_FILENAME: "E1_planintegration[18].md"
PREVIOUS_FILENAME: "E1_planintegration[17].md"
NEXT_FILENAMES:
  - "E1_planintegration[19].md"
  - "E1_planintegration[20].md"
CATEGORY: LINK+WATER+SOURCES+CONTAMINATION+INFRASTRUCTURE+PRESENTATION
PRIMARY_INTENT: "Create a canonical source/network layer for discoverable water origins, hydraulic connectivity, source availability, sampling knowledge, and source-selection intent while preserving WaterTreatment, LocationEvolution, Disease, Weather, Inventory, Power, Greenhouse, Expedition, and E1-17 Maintenance as the owners of their respective state and consequences."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_WATER_TREATMENT_SYSTEM_FORBIDDEN: true
SECOND_LOCATION_CONTAMINATION_AUTHORITY_FORBIDDEN: true
SECOND_DISEASE_ENGINE_FORBIDDEN: true
SECOND_INVENTORY_WATER_LEDGER_FORBIDDEN: true
SECOND_MAINTENANCE_SYSTEM_FORBIDDEN: true
SECOND_POWER_SYSTEM_FORBIDDEN: true
RNG_FOR_BASE_CONTAMINATION_PROPAGATION_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: HIGH
MICROMANAGEMENT_RISK: HIGH
DATA_MODEL_RISK: VERY_HIGH
---

# E1 Plan Integration [18] — Water Sources, Contamination Networks, Source Discovery, Testing, Hydraulic Routing, and Strategic Supply

> **Sequence rule:** this file is `E1_planintegration[18].md`.
> The next files are `E1_planintegration[19].md`, `E1_planintegration[20].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 189 into an implementation-grade water-source and contamination-network programme.

The source plan identifies a genuine integration gap. `WaterTreatmentSystem` apparently owns purification but
accepts one aggregate incoming contamination value. `LocationEvolutionSystem` tracks location contamination,
`HydroGeologyCatalog` contains static well data, and other systems maintain local contamination state for
sump/coastal contexts. What is missing is a persistent world-facing layer that can answer:

- What water sources exist?
- Which have been discovered?
- Which are physically/hydraulically connected to the shelter?
- What source is currently supplying raw water?
- What is the actual source throughput/availability?
- What contamination state is associated with each source?
- What contamination is known to the player versus merely present in world truth?
- How can contamination move through explicit source connections?
- Which source should feed `WaterTreatmentSystem`?
- What source-side infrastructure is required to draw, pump, isolate, or connect water?
- What happens when a source becomes unavailable, polluted, disconnected, or dry?

That layer should not become a second complete water simulation.

E1-18 therefore adopts this rule:

**WaterSourceNetwork owns source identity, source-local water state that has no other owner, connectivity,
sampling knowledge, and routing intent; canonical systems own treatment, disease, weather, location
contamination, infrastructure wear, power, storage, survivor consumption, and irrigation consequences.**

The plan also makes a major modeling correction: a single 0–1 “contamination” scalar is not sufficient if the
game distinguishes radiological, biological, and chemical hazards. Different contaminants respond
differently to treatment. Boiling may reduce biological risk but does not remove radioactive contamination
from dissolved material; filtration may help certain particulates but not every chemical contaminant. The
source layer therefore needs typed contamination dimensions or a canonical contaminant profile compatible
with `WaterTreatmentSystem`.

## 1. Source Intent Preserved

Plan 189 asks for:

- multiple water sources;
- source discovery;
- per-source contamination;
- source flow;
- source connections;
- contamination propagation;
- pipes, pumps, storage tanks, filtration, test kits;
- source maintenance;
- water testing;
- source switching;
- multiple-source blending;
- supply/demand visibility;
- disease consequences;
- weather/location contamination integration;
- expedition discovery;
- greenhouse integration;
- save/load;
- deterministic behavior;
- old-save compatibility;
- events, quests, UI, and selftests.

E1-18 preserves those strategic goals while narrowing ownership and correcting physical/gameplay abstractions.

## 2. Core Architecture Thesis

```text
World / Location authorities
      |
      +--> discovered locations
      +--> environmental contamination
      +--> weather / runoff
      +--> hydrogeology/static source definitions
      |
      v
WaterSourceNetwork
      |
      +--> source instance identity
      +--> discovery/availability
      +--> source-local contaminant profile
      +--> source-local natural yield / current draw capacity
      +--> source-to-source hydraulic edges
      +--> source-to-shelter connection topology
      +--> sampling/test knowledge
      +--> source-selection / routing intent
      |
      v
Raw-water intake contract
      |
      +--> flow/capacity
      +--> contaminant profile
      +--> provenance/source blend
      |
      v
WaterTreatmentSystem
      |
      v
Canonical treated-water storage / needs / greenhouse
```

Infrastructure condition belongs to E1-17 Maintenance or existing subsystem owners. Pumps consume PowerGrid
power. Storage belongs to Inventory/Water storage. Treatment belongs to `WaterTreatmentSystem`.

## 3. Architectural Corrections to the Source Plan

### 3.1 Replace one contamination scalar with typed contamination

Do not treat radiological, biological, and chemical contamination as interchangeable.

Recommended conceptual profile:

```text
WaterContaminantProfile
  radiological
  biological
  chemical
  particulate/turbidity (optional)
  salinity/mineral load (optional if gameplay uses it)
```

Exact fields must align with current treatment/disease/radiation systems.

### 3.2 Filtration belongs at the treatment/infrastructure boundary

A source may have source-side screening or natural filtration properties, but an installed filtration unit
that actively purifies drawn water should generally be owned by `WaterTreatmentSystem` or a typed water
infrastructure capability. The source system should not subtract contamination independently and then let
WaterTreatment purify it again.

### 3.3 Storage is not a source fact

A storage tank is infrastructure/container state. Water volume belongs to the canonical water/inventory
storage authority, not a `WaterSourceState.totalAvailableWater` field.

### 3.4 Demand is not source-owned

Survivor water demand belongs to Needs/rationing/consumption. Greenhouse demand belongs to agriculture.
WaterSourceNetwork may expose raw supply capacity. A UI may compose supply versus demand as a read model.

### 3.5 Source switching is hydraulic routing, not a boolean

`isActive` is insufficient once multiple sources, valves, pumps, blending, treatment throughput, and outages
exist. Use explicit intake routes/selection state.

### 3.6 Base contamination propagation should be deterministic

Hydraulic/advection/mixing propagation is best treated deterministically from connection, flow, runoff, and
source values. RNG is unnecessary for ordinary propagation. Stochastic contamination events may exist as
authored world events using their own deterministic keyed RNG.

### 3.7 “Connected sources share contamination” is too broad

A pipe does not automatically contaminate both sources in both directions. Direction, pressure, valves,
backflow prevention, flow, and source isolation matter. Surface runoff and groundwater transport also differ.
Connections must have directional/flow semantics.

### 3.8 Old saves should not fabricate a new well blindly

If the existing shelter intake already has a known canonical source, migrate it. Otherwise introduce a
synthetic legacy intake only as a compatibility shim with documented provenance; do not silently invent a
fully fledged geological well.

## 4. Non-Negotiable Rules

- `WaterTreatmentSystem` remains the sole water-treatment/purification authority.
- `LocationEvolutionSystem` remains owner of location contamination.
- `DiseaseSystem` remains owner of disease outcomes.
- Radiation/contamination authorities remain owners of exposure/dose.
- `WeatherSystem` remains owner of realized weather.
- Expedition/world discovery remains owner of location/source discovery events where applicable.
- Greenhouse/Agriculture owns irrigation demand and crop consequences.
- Needs/rationing owns survivor water demand/consumption.
- Inventory/water-storage authority owns stored water volume.
- PowerGrid owns pump energy availability.
- E1-17 Maintenance owns pipe/pump/tank/filter condition where it is maintainable infrastructure.
- E1-9 Construction/upgrade rails own infrastructure installation when available.
- WaterSourceNetwork owns source identity, source-local availability/yield, source-local contaminant state if
  not already owned elsewhere, hydraulic source connection topology, sample/test knowledge, and intake routing.
- No single universal `currentContamination` if multiple contaminant classes matter downstream.
- No `totalAvailableWater` authoritative field if it is derived from source capacity + treatment + storage.
- No `waterDemand` authoritative field in the source save section.
- No source-side filtering and WaterTreatment filtering of the same stage.
- No direct disease application from source contamination.
- No direct survivor dehydration from insufficient source flow.
- No direct greenhouse yield mutation from the source system.
- No pump flow if PowerGrid says the pump is unavailable.
- No infrastructure condition duplicated from E1-17.
- Base propagation uses deterministic fixed-point or carefully bounded deterministic arithmetic.
- Contaminant mass/level propagation must obey conservation/explicit source-sink rules.
- Backflow and pipe cross-contamination require explicit topology/policy.
- Sampling/test results are knowledge state, not world truth.
- Player UI must distinguish measured contamination from unknown/estimated contamination.
- A source can exist but remain undiscovered.
- A discovered source can be unavailable/unconnected.
- A connected source can be isolated by valve/routing.
- Multiple-source blending must calculate the intake profile from actual flow contributions.
- Source depletion/drying must have a real yield/recharge model or remain authored-event driven.
- Old saves preserve existing treated-water behavior before the player engages new source management.
- Save/load cannot duplicate discovered sources, connections, routing changes, tests, or source water.
- Time skip applies contamination/flow updates exactly once.
- Source count/network complexity must remain bounded.
- The first release should ship 3–4 distinct source types, not all six, unless each adds a real strategic choice.
- Maintenance/testing should not become repetitive busywork.

## 5. Acceptance Slices

### Slice A — Canonical source identity and one intake
Represent the existing shelter's raw-water origin explicitly and feed its contaminant profile into
WaterTreatment without changing current output behavior.

### Slice B — Second discoverable source and source switching
Expedition finds a second source; player builds/connects/selects it.

### Slice C — Typed testing and contamination change
Player samples a source, sees knowledge separate from truth, and weather/location state modifies the source.

### Slice D — Network connection/propagation
One directional runoff/groundwater/pipe interaction with conservation and isolation.

### Slice E — Multi-source blending and infrastructure
Only after single-source routing is proven.

Do not begin with six source types, full aquifer simulation, five infrastructure classes, and all quests.


---

## E1-18A — Premise verification and water-authority audit

**Goal:** Verify every existing water, contamination, location, storage, demand, irrigation, power, maintenance, discovery, and save authority before introducing source-network state.

### Required substeps

1. Inspect `WaterTreatmentSystem`, its `incomingContaminationLevel`, treatment stages, output storage, throughput, contaminant semantics, save contract, and callers.
2. Inspect `LocationEvolutionSystem` contamination fields and mutation events.
3. Inspect `HydroGeologyCatalog` and `WellContaminationEntry` semantics.
4. Inspect `SumpFloodingSystem`, `District8DeepCoastSystem`, Radiation/Contamination, Disease, Weather, Expedition, Greenhouse/Agriculture, Needs, water rationing/storage, PowerGrid, E1-17 maintenance, E1-9 construction, and save registry.
5. Search for water containers, cisterns, tanks, pumps, pipes, wells, rain collectors, rivers, springs, municipal infrastructure, test kits, contamination types, boiling, filtration, purification, irrigation, and water trade.
6. Determine whether water quantity is a dedicated resource or inventory item/abstract meter.
7. Determine whether treatment distinguishes biological/chemical/radiological contamination.
8. Determine whether source discovery is already represented in expedition/world-location data.
9. Determine whether any water source already has stable identity.
10. Map every proposed source DTO field to one canonical owner.
11. Create `docs/systems/WATER_SOURCE_AUTHORITY_MAP.md`.
12. Create intake duplicate-search evidence linking Plans 23, 135, 158, E1-9, E1-17, climate, disease, agriculture, and relevant expedition plans.
13. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18B — Water-source ownership ADR

**Goal:** Define the exact boundary between source/network truth and WaterTreatment/Location/Storage/Maintenance state.

### Required substeps

1. Write an ADR comparing a new `WaterSourceSystem`, extension of WaterTreatment intake model, and a generic world-resource-source graph.
2. Define source-owned facts: source ID, definition/type, world location, discovery/availability, natural/source-local yield model, source-local contaminant profile if no other owner exists, source hydraulic edges, source-to-intake routes, routing selection, test/sample history/knowledge references.
3. Explicitly exclude treated-water volume, survivor demand, greenhouse demand, disease state, power state, infrastructure condition, treatment-stage efficiency, and current location contamination.
4. Define adapters from location/weather to source state and source intake to WaterTreatment.
5. Define E1-17 infrastructure-maintenance boundary.
6. Define E1-9 installation boundary.
7. Define rollback/feature flags.
8. Require second-tool architecture review.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18C — Stable source identity contract

**Goal:** Give every water source a persistent identity tied to the world rather than UI labels.

### Required substeps

1. Define stable `sourceId`, source definition/type ID, canonical location ID, origin/provenance, discovery state, availability state, and optional hydrogeological region/aquifer ID.
2. Separate authored source definition from runtime source instance.
3. Do not use display name as identity.
4. Define behavior for a source that dries, is diverted, destroyed, contaminated, sealed, or rediscovered.
5. Preserve source identity through temporary unavailability.
6. Reuse existing well/hydrogeology IDs where possible.
7. Add uniqueness/reference tests.
8. Define generated-source ID policy only if procedural discovery exists.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18D — Source type schema and strategic differentiation

**Goal:** Author only source types that have real distinct behavior in the current world model.

### Required substeps

1. Define source type metadata: yield policy, recharge policy, weather sensitivity, contamination susceptibility, connection compatibility, draw method, required infrastructure capability, and localization/presentation tags.
2. Do not assume spring = always clean or rain = always low contamination.
3. Do not hard-code municipal = unknown as a permanent property; testing reveals it.
4. Choose initial source types from well, river/surface water, spring/groundwater, rain collector, municipal only if each has a real consumer.
5. Treat underground spring as a subtype/profile if it does not need a separate code path.
6. Validate policy references.
7. Keep source types data-driven.
8. Start with 3–4 types.
9. Add authoring documentation.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18E — Typed contaminant-profile ADR

**Goal:** Replace the one-dimensional contamination scalar where necessary so treatment and disease consequences remain coherent.

### Required substeps

1. Audit treatment/disease/radiation contaminant semantics.
2. Define the minimal typed profile required by existing consumers.
3. Potential dimensions: biological load, chemical load/toxin class, radiological contamination/activity proxy, particulate/turbidity.
4. Do not add dimensions that no system can consume.
5. Define fixed-point ranges/units.
6. Define mixing semantics per dimension.
7. Define source/sink and natural-decay semantics only where justified.
8. Define unknown/untested state separately from true profile.
9. Provide compatibility adapter to legacy `incomingContaminationLevel` until WaterTreatment can consume typed input.
10. Require second-tool review because this changes a central contract.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18F — Legacy WaterTreatment intake adapter

**Goal:** Introduce source identity without changing current water-treatment behavior in the first slice.

### Required substeps

1. Identify the current raw-water input call path.
2. Create a typed `RawWaterIntake` or equivalent carrying source provenance, requested/available flow, and contaminant profile.
3. Adapt the profile to the current single contamination scalar conservatively if WaterTreatment has not yet been expanded.
4. Preserve current treatment output under the migrated legacy source.
5. Do not move treatment rules into WaterSourceNetwork.
6. Add baseline parity tests.
7. Add a feature flag to enable source-based intake.
8. Use this as the architectural bridge.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18G — Canonical source-local contaminant state

**Goal:** Persist only contamination facts truly belonging to the source rather than copying location contamination.

### Required substeps

1. Define source contaminant state as source water composition at the abstraction boundary.
2. Use location contamination as an input, not a copied authoritative duplicate.
3. Define how source type/hydrogeology buffers/transfers location contaminants.
4. Define state update cadence.
5. Use fixed-point bounded values.
6. Track provenance/source terms if debugging requires it.
7. Do not store derived player-known result here.
8. Add round-trip tests.
9. Add no-double-input tests for LocationEvolution.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18H — Deterministic contamination update model

**Goal:** Update source contaminant state from explicit environmental inputs without routine RNG.

### Required substeps

1. Define update equation per contaminant dimension.
2. Inputs may include source baseline/geology, upstream/source connections, runoff, recharge, location contamination, withdrawals, dilution, explicit pollution events, and natural attenuation only if modeled.
3. Do not use `ISeededRng` for ordinary daily propagation.
4. Use deterministic fixed-point or stable arithmetic.
5. Define clamping.
6. Define source/sink terms explicitly.
7. Add stepped-versus-bulk equivalence tests.
8. Reserve keyed RNG only for authored stochastic contamination events.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18I — Contamination conservation and mass-balance policy

**Goal:** Prevent network propagation from creating or deleting contamination arbitrarily.

### Required substeps

1. Decide whether the game tracks contaminant concentration only or concentration plus water volume/mass proxy.
2. For connections with meaningful flow, compute receiving mixture from transfer volume and source concentration.
3. Subtract/export source mass only if source volume model requires it.
4. Define infinite/large-reservoir abstraction explicitly where conservation is intentionally simplified.
5. Do not average connected source contaminations blindly.
6. Define dilution from clean recharge.
7. Add conservation/property tests.
8. Document abstraction limits.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18J — Source yield and flow-capacity model

**Goal:** Represent how much raw water can be drawn without duplicating treated storage or demand.

### Required substeps

1. Define natural yield/available draw capacity in liters/day or canonical unit.
2. Separate instantaneous/periodic source yield from stored water.
3. Define recharge for finite sources only where useful.
4. Define pump/infrastructure capacity separately.
5. Define treatment intake capacity separately.
6. Effective raw intake is bounded by source yield, route, pump, power, and treatment capacity.
7. Do not store `total available water` as a redundant global source value.
8. Add tests for high-yield river, low-yield well/spring, rain-dependent collector, pump-limited intake, and treatment-limited intake.
9. Keep demand external.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18K — Rain-collector integration

**Goal:** If rain harvesting is supported, derive collection from WeatherSystem and collection infrastructure.

### Required substeps

1. Use realized precipitation, collection area, efficiency, and storage capacity.
2. Do not invent rain in WaterSourceNetwork.
3. Define first-flush/particulate contamination only if treatment can consume it.
4. Storage volume goes to canonical water storage.
5. Collector hardware condition belongs to E1-17 if maintainable.
6. Add tests for no rain, light/heavy rain, full tank, contaminated fallout rain if canonical weather/radiation supports it.
7. Feature-gate if rainfall quantity is not represented.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18L — Groundwater/well model

**Goal:** Represent well sources through hydrogeology data and explicit contamination coupling.

### Required substeps

1. Reuse `HydroGeologyCatalog` well entries.
2. Define aquifer/source baseline, recharge/yield, depth/profile tags, surface-runoff susceptibility, and location contamination coupling.
3. Do not make every rain event instantly contaminate deep wells.
4. Define delayed/attenuated infiltration policy if gameplay uses it.
5. Use pump/power capability for draw if required.
6. Add tests for shallow versus deep well profiles, contamination event, pump outage, and recovery.
7. Keep aquifer simulation bounded.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18M — Surface-water/river model

**Goal:** Represent directional surface water with upstream/downstream contamination flow.

### Required substeps

1. Use world/topology direction or authored upstream/downstream edges.
2. Do not create symmetric contamination sharing.
3. Use realized runoff/weather and upstream sources/location contamination.
4. Define high yield but higher variable contamination risk.
5. Do not simulate full hydrodynamics.
6. Add tests for clean upstream, polluted upstream, rain runoff, downstream mixing, source isolation, and save/load.
7. Use one directional graph.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18N — Spring source model

**Goal:** Represent groundwater discharge as a source profile rather than universally clean water.

### Required substeps

1. Define yield/recharge profile.
2. Define geological filtration/buffering if supported.
3. Allow contamination from connected aquifer/location according to policy.
4. Do not guarantee zero biological/chemical/radiological risk.
5. Require testing unless known at scenario start.
6. Add tests for clean baseline, changed aquifer contamination, low-flow period, and save/load.
7. Use same groundwater framework where possible.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18O — Municipal/pre-war source boundary

**Goal:** Treat municipal water as infrastructure-backed intake with uncertain availability/quality, not a magic source type.

### Required substeps

1. Audit pre-war pipe/network/location content.
2. Define source endpoint and connection requirements.
3. Availability may depend on pressure/pump/valve infrastructure.
4. Quality derives from actual source/network contamination and stagnation policies if modeled.
5. Do not invent a city-wide water simulation.
6. Add tests for disconnected, connected, stagnant/unknown, tested, and contaminated states.
7. Feature-gate if no content supports it.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18P — Hydraulic connection graph

**Goal:** Represent source-to-source and source-to-shelter connections with direction/flow semantics.

### Required substeps

1. Define connection ID, from/to endpoint, connection class, direction policy, maximum flow/capacity, enabled/isolated state, controlling infrastructure refs, and contamination transport policy.
2. Connection classes may include groundwater transfer, surface runoff, pipe, channel, collection line.
3. Do not use one undirected `sourceA/sourceB` rule for every connection.
4. Define valve/isolation semantics for pipe edges.
5. Use stable IDs.
6. Validate endpoint and cycle rules where relevant.
7. Add graph-integrity tests.
8. Keep graph small/authored.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18Q — Groundwater connection propagation

**Goal:** Model slow inter-source contamination only where sources actually share a hydrogeological connection.

### Required substeps

1. Use authored aquifer/groundwater edges.
2. Define directional/bidirectional exchange carefully.
3. Use slow transfer coefficients/delay where useful.
4. Do not automatically connect nearby wells.
5. Do not derive precise groundwater flow from map distance alone.
6. Add tests for connected/unconnected wells, delayed transfer, dilution, and isolation where physically possible.
7. Document abstraction.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18R — Surface runoff propagation

**Goal:** Use weather-triggered directional runoff rather than generic daily contamination sharing.

### Required substeps

1. Use runoff edges tied to terrain/location data if available.
2. Activate/scale transfer from realized precipitation/snowmelt only where modeled.
3. Use upstream contaminant source terms.
4. Do not back-propagate runoff uphill.
5. Add tests for dry day, heavy rain, polluted upstream location, clean catchment, and save/load.
6. Keep event/interval update deterministic.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18S — Pipe/backflow contamination policy

**Goal:** Prevent pipe connections from automatically contaminating clean sources.

### Required substeps

1. Define normal directed flow from selected source toward shelter/treatment.
2. Contamination travels with flow.
3. Reverse contamination requires backflow/cross-connection failure or explicit hydraulic condition.
4. Use valve/backflow-preventer capability if infrastructure supports it.
5. E1-17 condition may affect leak/backflow capability.
6. Do not instantly equalize connected concentrations.
7. Add tests for clean pipe, contaminated source, closed valve, backflow failure, two-source manifold, and repair.
8. Keep pressure model abstract unless necessary.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18T — Source discovery contract

**Goal:** Discover water sources through canonical world/exploration/knowledge rails.

### Required substeps

1. Known-at-start sources are scenario data.
2. ExpeditionSystem may reveal authored source IDs at locations.
3. Construction/excavation may reveal a source through E1-9 event if content supports it.
4. Information/rumor systems may reveal uncertain source existence without marking it fully verified.
5. Do not allow WaterSourceNetwork to discover world locations independently.
6. Track source discovery/verification state.
7. Use stable discovery IDs.
8. Add tests for known, rumored, discovered, verified, duplicate discovery, and removed location.
9. Preserve information asymmetry.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18U — Source knowledge and uncertainty

**Goal:** Separate actual source state from what the player knows.

### Required substeps

1. Define source visibility: unknown, rumored, discovered, sampled, monitored.
2. Actual contaminant profile remains world truth.
3. Player-known contamination comes from testing/monitoring/observable events.
4. Flow/yield knowledge may also be estimated until measured if design benefits.
5. Do not show exact untested contamination.
6. Use information/knowledge authority if one already exists.
7. Add tests for hidden source, discovered but untested, stale sample, monitored source, and changed contamination after test.
8. Keep UI honest.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18V — Water sampling action

**Goal:** Make testing a real sample/diagnostic action through canonical items/labor/time.

### Required substeps

1. Define sample action target source.
2. Validate source access.
3. Use a test-kit item only if canonical inventory contains one.
4. Use Duty/job/action scheduler if testing takes meaningful time.
5. Consume kit/reagent according to item semantics.
6. Create stable test operation ID.
7. Sample actual contaminant profile at a defined moment.
8. Do not mutate source contamination.
9. Add tests for inaccessible source, no kit, interrupted test, duplicate completion, and save/load.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18W — Water test-result knowledge contract

**Goal:** Store observed results separately from actual source truth.

### Required substeps

1. Define test ID, source ID, sample day, analysis/completion day, tested-by survivor, method/kit ID, reported contaminant dimensions/bands, uncertainty/accuracy, and source operation ID.
2. Do not store one `testAccuracy` scalar if different contaminant dimensions have different detection limits unless simple abstraction is intentional.
3. Do not overwrite old results; retain bounded recent significant results.
4. Define stale-result UI.
5. Add localization/read-model support.
6. Add save/load/dedupe tests.
7. Bound history.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18X — Testing accuracy and false-certainty policy

**Goal:** Make imperfect testing deterministic and fair if accuracy uncertainty is retained.

### Required substeps

1. Prefer banded detection limits/confidence over noisy random values.
2. Low-quality kits may detect only severe contamination or certain contaminant classes.
3. Do not produce arbitrary random numbers around true contamination on every test.
4. If stochastic assay error is desired, use keyed deterministic sample/test RNG and persist result.
5. Never reroll the same sample after reload.
6. Define unknown/not-detected versus confirmed-safe carefully.
7. Add tests for detection threshold, unsupported contaminant class, high-quality kit, and save/load.
8. Document abstraction.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18Y — Continuous monitoring capability

**Goal:** Allow upgraded sensors/monitoring to reduce manual testing burden.

### Required substeps

1. Audit existing sensors/weather/radiation monitoring systems.
2. Define water-monitor capability as infrastructure/knowledge adapter, not a new treatment system.
3. Monitoring reports source profile according to sensor supported dimensions.
4. Power may be required through PowerGrid.
5. Condition belongs to E1-17.
6. Monitoring updates player knowledge automatically at defined intervals.
7. Add tests for powered, unpowered, failed sensor, changed contamination, and save/load.
8. Use this to avoid repetitive test-kit chores.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18Z — Source-to-shelter intake routing

**Goal:** Represent selected intake path explicitly rather than one `isActive` boolean.

### Required substeps

1. Define intake route ID, source endpoint(s), path/connection edges, requested draw, priority, enabled state, and treatment intake target.
2. Validate all path edges.
3. Validate pump/valve/infrastructure capability.
4. Validate source availability.
5. Validate treatment capacity.
6. Do not store current treated output in route state.
7. Use stable route operation IDs.
8. Add route validation tests.
9. Support one source first.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AA — Source switching transaction

**Goal:** Change raw-water source through a validated hydraulic operation.

### Required substeps

1. Validate destination source is discovered/connected/available.
2. Validate route capacity.
3. Validate pump/power.
4. Validate treatment compatibility.
5. Define whether switch is instant valve change or scheduled plumbing reconfiguration based on existing infrastructure.
6. If work is required, use canonical job scheduler.
7. Commit selected route exactly once.
8. Do not move/duplicate stored water.
9. Add tests for valid switch, unavailable source, broken pump, closed valve, work-required switch, and save/load.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AB — Multi-source blending gate

**Goal:** Add blending only after single-source intake and typed contamination are accepted.

### Required substeps

1. Define concurrent route flow contributions.
2. Compute blended contaminant profile from actual flow-weighted inputs.
3. Do not average source values without flow.
4. Respect treatment intake capacity.
5. Respect pump/route limits.
6. Define whether operators can set priority/ratio or the system derives it.
7. Add conservation tests.
8. Add tests for clean+dirty blend, unequal flow, one source failure, and save/load.
9. Feature-gate in first release.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AC — WaterTreatment typed-input integration

**Goal:** Move raw source water into WaterTreatment without duplicating purification logic.

### Required substeps

1. Define WaterTreatment input as raw-water quantity/flow + contaminant profile + provenance.
2. WaterTreatment owns removal efficiency/process stages/output quality.
3. Do not let source infrastructure filtration and treatment both remove the same contaminant unless they are distinct stages.
4. Define treatment capacity/backpressure behavior.
5. Add tests for biological, chemical, radiological profiles if supported.
6. Add legacy scalar adapter tests.
7. Remove the single global incoming contamination source once migration is complete.
8. Keep treatment save ownership unchanged where possible.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AD — Treated-water storage boundary

**Goal:** Keep stored water volume/quality in the canonical storage/inventory/water authority.

### Required substeps

1. Identify how treated water is stored today.
2. SourceNetwork only supplies raw intake.
3. Storage tanks may be water-storage containers, not source DTOs.
4. E1-17 owns tank condition if maintainable.
5. WaterTreatment writes treated output through canonical storage transaction.
6. Do not compute global source `totalAvailableWater` from storage.
7. Add tests for full storage, empty storage, contaminated raw intake, and treatment throughput.
8. Document storage owner.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AE — Survivor demand and rationing boundary

**Goal:** Expose supply capacity to Needs without owning survivor water demand.

### Required substeps

1. Needs/rationing computes consumption requirement.
2. Water storage/treatment determines available potable water.
3. UI composes supply/demand forecast.
4. Source shortage may reduce raw intake capacity but does not directly dehydrate survivors.
5. Rationing remains canonical.
6. Add tests for low raw supply, adequate stored reserve, empty reserve, and restored source.
7. Do not store demand in source state.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AF — Disease consequence integration

**Goal:** Route unsafe water consumption through Disease/Health using treated-water quality, not source contamination directly.

### Required substeps

1. Define potable-water quality output from WaterTreatment/storage.
2. DiseaseSystem evaluates biological/chemical illness risk where appropriate.
3. Radiation authority evaluates radiological exposure/dose.
4. Do not infect survivors merely because a source is contaminated if treatment/storage made it safe.
5. Do not assume boiling fixes chemical/radiological contamination.
6. Add tests for contaminated source + effective treatment, failed treatment, biological-only boiling if supported, and radiological contamination.
7. Keep health ownership external.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AG — Greenhouse/agriculture irrigation integration

**Goal:** Supply irrigation through canonical water allocation rather than modifying crop yield from source state.

### Required substeps

1. Greenhouse/Agriculture requests water quantity/quality through resource allocation.
2. Potable quality requirements may differ from irrigation quality if system supports it.
3. Do not directly reduce yield from source contamination in WaterSourceNetwork.
4. Agriculture owns crop consequences.
5. Source/treatment/storage capacity constrains available irrigation water.
6. Add tests for sufficient irrigation, shortage, unsuitable quality, treated water, and competing survivor demand.
7. Feature-gate if irrigation is not modeled.

### Water-network invariants

- WaterSourceNetwork owns raw source/network facts, not treatment or downstream consequences.
- WaterTreatment remains the sole purification authority.
- Source world truth and player test knowledge are separate.
- Infrastructure condition stays with E1-17/existing component owners.
- Power, storage, demand, disease, radiation, and greenhouse state remain canonical elsewhere.
- Base contamination propagation is deterministic and directional.
- Mixing/transfer obeys an explicit conservation or documented reservoir abstraction.
- Save/load/time skip cannot duplicate source, water, tests, routing, or propagation.

### Negative tests

- Source state stores treated-water volume or survivor water demand.
- A filter removes contamination in SourceNetwork and again in WaterTreatment.
- Connected pipe sources instantly equalize contamination bidirectionally.
- Boiling removes radiological/chemical contamination generically.
- A source test changes actual contamination.
- Pump continues delivering when PowerGrid says it is unavailable.
- Pipe condition is duplicated outside E1-17.
- Reload/source switching creates water or repeats contamination propagation.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] WaterTreatment parity/adapter tests pass.
- [ ] Contaminant mixing/conservation tests pass.
- [ ] Knowledge/testing separation passes.
- [ ] Routing/power/maintenance integration passes.
- [ ] Save/migration/time-skip tests pass.
- [ ] Performance/economy/micromanagement evidence is captured.

---

## E1-18AH — Pump and power integration

**Goal:** Make extraction/transfer pumps depend on PowerGrid without duplicating power state.

### Required substeps

1. Define pump capability/required power draw in infrastructure definition.
2. PowerGrid owns whether pump can run.
3. WaterSourceNetwork uses available pump throughput.
4. E1-

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
