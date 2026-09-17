---
PLAN_ID: E1-22
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 22
STATUS: READY_FOR_EXECUTION_WHEN_WASTE_HEALTH_AND_INFRASTRUCTURE_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 201 — Shelter Sanitation & Waste Management System"
SEQUENCE_FILENAME: "E1_planintegration[22].md"
PREVIOUS_FILENAME: "E1_planintegration[21].md"
NEXT_FILENAMES:
  - "E1_planintegration[23].md"
  - "E1_planintegration[24].md"
CATEGORY: LINK+SHELTER+SANITATION+WASTE+HYGIENE+ENVIRONMENT
PRIMARY_INTENT: "Create persistent shelter waste streams, sanitation service capacity, processing/containment logistics, hygiene exposure context, and sanitation-risk read models while preserving Disease, Needs, Ventilation, Water, Maintenance, Power, Kitchen, Medical, Greenhouse, Inventory, Construction, and contamination authorities as the owners of their own state and consequences."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
AUTHORITATIVE_GLOBAL_SANITATION_SCORE_FORBIDDEN: true
SECOND_DISEASE_ENGINE_FORBIDDEN: true
SECOND_WATER_TREATMENT_SYSTEM_FORBIDDEN: true
SECOND_AIR_QUALITY_SYSTEM_FORBIDDEN: true
SECOND_MAINTENANCE_SYSTEM_FORBIDDEN: true
SECOND_INVENTORY_LEDGER_FORBIDDEN: true
SECOND_NEEDS_SYSTEM_FORBIDDEN: true
RNG_FOR_BASE_WASTE_PRODUCTION_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: VERY_HIGH
BALANCE_RISK: HIGH
MICROMANAGEMENT_RISK: VERY_HIGH
BIOHAZARD_MODEL_RISK: VERY_HIGH
---

# E1 Plan Integration [22] — Shelter Sanitation, Waste Streams, Sewage, Hygiene, Processing, Containment, and Environmental Health

> **Sequence rule:** this file is `E1_planintegration[22].md`.
> The next files are `E1_planintegration[23].md`, `E1_planintegration[24].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 201 into an implementation-grade shelter sanitation and waste-management programme.

The source plan identifies a real systems gap: the shelter already models air, water infiltration, disease,
food spoilage, shelter environment, and many activities that logically create waste, but there is no
persistent infrastructure/logistics layer that answers:

- what waste streams exist;
- where they are generated;
- where they are stored;
- which facilities can process or contain them;
- whether collection capacity is keeping pace;
- whether sewage is backing up;
- whether hazardous waste is isolated;
- whether washing/toilet access is sufficient;
- whether a sanitation failure has created a real environmental exposure;
- which canonical health/environment systems should react.

The wrong implementation is one large `SanitationSystem` with:

- a universal 0–100 sanitation score;
- a second disease-risk score;
- a second water-recycling efficiency;
- copied facility condition;
- copied survivor morale impact;
- direct air/water contamination writes;
- random “outbreak” events disconnected from actual exposure.

E1-22 uses a stricter architecture:

**sanitation owns waste material flow, sanitation-service availability, source/containment state, and
sanitation-specific exposure events; canonical systems own disease, morale/needs, air quality, water quality,
infrastructure condition, power, inventory, agriculture, and survivor health.**

A sewage backup can create a biological contamination/exposure event. DiseaseSystem determines whether and
how disease propagates. A trash incinerator can create smoke/particulates. Ventilation owns the resulting air
quality. A broken recycler can reduce greywater-processing capability. E1-18/WaterTreatment owns water
availability and quality. A dirty survivor can have a hygiene/readiness context, but Needs/Health owns actual
physiological/morale effects.

## 1. Source Intent Preserved

Plan 201 asks for:

- human, kitchen, medical, chemical, general, and hazardous waste;
- sanitation facilities;
- sewage and trash accumulation;
- per-survivor hygiene;
- disease vectors;
- water recycling;
- infrastructure degradation/repair;
- power use;
- compost/fertilizer;
- UI;
- events and quests;
- deterministic behavior;
- save/load;
- old-save compatibility;
- headless validation.

E1-22 preserves those gameplay goals while correcting authority, physics, health, and player-burden risks.

## 2. Core Architecture Thesis

```text
Canonical activity / population events
      |
      +--> survivor presence/use
      +--> kitchen output/spoilage
      +--> medical treatment
      +--> workshop/industrial jobs
      +--> cleaning/decontamination
      +--> item disposal
      |
      v
Sanitation waste-stream ledger
      |
      +--> stream identity/category
      +--> quantity
      +--> source/location
      +--> contamination/hazard tags
      +--> containment state
      +--> collection/processing destination
      |
      v
Sanitation facilities / service graph
      |
      +--> latrine/sewage handling
      +--> waste collection
      +--> composting
      +--> processing
      +--> incineration
      +--> hazardous containment
      +--> greywater routing
      |
      v
Canonical consequence handoffs
      |
      +--> Disease / Health
      +--> Ventilation / air quality
      +--> E1-18 Water / WaterTreatment
      +--> Needs / morale / washing access
      +--> Greenhouse / fertilizer
      +--> Radiation/Contamination
      +--> Inventory / salvage
```

Facility installation belongs to E1-9/Construction. Facility condition and repair belong to E1-17 Maintenance.
Power comes from PowerGrid.

## 3. Architectural Corrections to the Source Plan

### 3.1 Do not make `sanitationLevel` simulation truth

A global sanitation score can be a derived UI summary, but it should not be the mechanism that directly causes
disease, morale loss, contamination, and vermin.

The simulation should use specific facts:

- uncontained human waste;
- sewage overflow;
- contaminated surfaces;
- unavailable washing;
- exposed medical waste;
- chemical spill;
- spoiled kitchen waste;
- ventilation load from incineration/odor;
- blocked toilets;
- overflowing storage.

### 3.2 Do not create a second per-survivor disease-risk score

`HygieneLevel.diseaseRisk` duplicates DiseaseSystem. Hygiene may own cleanliness/exposure context only if Needs
does not already own it. Disease risk remains derived by DiseaseSystem from canonical exposures.

### 3.3 Do not create a second morale-impact field

`moraleImpact` belongs to Needs/MentalHealth consequence policy, not persisted sanitation state.

### 3.4 Water recycling belongs to water authorities

Greywater generation/routing may originate in sanitation, but purification, potable/non-potable quality,
storage, and output belong to E1-18/WaterTreatment/water-storage authorities.

### 3.5 Facility condition belongs to E1-17

Latrines, pumps, sewage tanks, incinerators, processors, recycler hardware, containment seals, and pipework
should use maintainable-asset condition if E1-17 is available. Sanitation must not maintain its own duplicate
`condition` field.

### 3.6 Waste production should be deterministic

Survivor count, meals cooked, medical actions, workshop jobs, and item disposal produce deterministic waste.
Routine generation does not need `ISeededRng`.

### 3.7 Outbreaks are health consequences, not sanitation random events

Sanitation can create pathogen exposure/vector conditions. DiseaseSystem determines outbreak dynamics.
`SanitationSystem` must not independently roll an outbreak and then also increase Disease transmission.

### 3.8 Compost is not automatically safe fertilizer

Human/medical/chemical/radiological waste must never become greenhouse fertilizer just because a generic
composter exists. Compostable inputs, processing requirements, maturation, and contamination classes must be
explicit.

### 3.9 Incineration is not consequence-free disposal

Incineration may reduce waste volume but can require fuel/power and create emissions/ash. Certain hazardous or
radioactive waste may be inappropriate to burn. Ventilation/contamination authorities own environmental
effects.

### 3.10 Old saves should not receive a fabricated free latrine silently

The safest compatibility rule is to preserve pre-feature behavior with a migration grace/baseline or map
existing shelter toilet/plumbing infrastructure if it exists. Do not conjure a physical facility into E1-9
topology without repository evidence.

## 4. Non-Negotiable Rules

- Sanitation owns waste-stream quantity/state only where no canonical inventory/hazard authority already owns it.
- Waste generation comes from real source events or bounded population/activity formulas.
- DiseaseSystem owns disease transmission, infection, outbreak, and disease risk.
- Needs/MentalHealth owns morale/stress.
- Needs or an approved survivor hygiene component owns hygiene if such a need is accepted.
- Ventilation owns air quality, smoke, soot, CO, odor-related air consequences where modeled.
- E1-18/WaterTreatment owns water treatment/quality and recycled-water output.
- Water storage/Inventory owns stored water volume.
- E1-17 Maintenance owns facility condition, servicing, repair, failure state where maintainable.
- E1-9 Construction owns facility installation/upgrades where available.
- PowerGrid owns electricity/fuel-power availability.
- Inventory owns items, consumables, waste items where represented as items, and material transactions.
- Kitchen owns food production/spoilage events; sanitation consumes waste output evidence.
- MedicalPipeline owns treatment actions and medical-waste production evidence.
- Greenhouse/Agriculture owns crop/fertilizer consequences.
- Radiation/Contamination owns radiological exposure/contamination consequences.
- SumpFlooding owns water infiltration/flood state.
- No authoritative universal `sanitationLevel` drives all consequences.
- No persisted sanitation-local `diseaseRisk`.
- No persisted sanitation-local `moraleImpact`.
- No copied facility condition.
- No copied water-recycling efficiency if WaterTreatment already owns it.
- Human waste, medical waste, chemical waste, and radioactive/hazardous waste require distinct treatment rules.
- General trash is not automatically one homogeneous material.
- Waste cannot disappear without an explicit processing/export/disposal sink.
- Waste cannot duplicate through save/load, transfer, processing, or cancellation.
- Waste processing obeys quantity conservation and explicit outputs/byproducts.
- Composting may create compost only from allowed input streams and processing state.
- Incineration produces residual ash/byproducts according to policy.
- Hazardous containment isolates waste; it does not destroy it.
- Sewage tank capacity is storage/containment capacity, not “processing” unless a real septic/treatment process exists.
- Base waste accumulation uses no RNG.
- Random sanitation disasters, if authored, use stable event IDs/keyed RNG outside routine flow.
- Hygiene cannot decay/restore twice through both Needs and Sanitation.
- Washing consumes canonical water where modeled.
- Lack of washing water does not directly infect survivors.
- Old saves begin prospectively with no retroactive months of waste.
- Time skip applies waste generation/processing once.
- No per-frame full-shelter waste recomputation.
- First release should use 3–4 major waste streams and 3–4 facilities before six/seven breadth.
- Sanitation must create meaningful logistics decisions without constant click-cleaning.

## 5. Acceptance Slices

### Slice A — Human + kitchen waste
Generate two streams from real population/kitchen activity and route them into basic containment/processing.

### Slice B — Sewage failure handoff
Overflow/back-up produces one sanitation exposure event consumed by Disease/Needs/Water/Ventilation as appropriate.

### Slice C — Medical/hazardous segregation
Medical waste requires proper containment and cannot enter compost/general disposal.

### Slice D — Hygiene/washing
Only after ownership with Needs is resolved.

### Slice E — Recycler/incinerator/compost complexity
Only after conservation, facility ownership, and consequence handoffs are proven.

Do not author all six waste streams and seven facilities before Slice A passes.


---

## E1-22A — Premise verification and sanitation-authority audit

**Goal:** Verify waste-generating activities, disease, needs/hygiene, water, ventilation, flooding, maintenance, construction, power, kitchen, medical, greenhouse, contamination, inventory, save, and shelter topology authorities before adding sanitation state.

### Required substeps

1. Inspect `VentilationSystem`, `SumpFloodingSystem`, `DiseaseSystem`, `ShelterThermalSystem`, `NeedsSystem`, `KitchenNutritionSystem`, `MedicalPipelineCoordinator`, Greenhouse/Agriculture, E1-18 water source/treatment/storage, E1-17 maintenance, E1-9 construction/topology, PowerGrid, Inventory, Radiation/Contamination, Duty/jobs, and save registry.
2. Search for toilet, latrine, sewage, septic, drain, greywater, blackwater, garbage, trash, compost, incinerator, medical waste, chemical waste, radioactive waste, hygiene, washing, shower, cleaning, decontamination, sanitation, and disposal.
3. Determine whether survivors already have hygiene/cleanliness/dirty status.
4. Determine whether room/environment systems already track contamination or surface filth.
5. Determine whether water use is metered by activity.
6. Determine whether spoiled-food disposal already removes items through Inventory.
7. Determine whether medical consumables become consumed items with provenance.
8. Determine whether waste can be represented as bulk stream state, inventory items, or a hybrid.
9. Determine whether existing shelter rooms include toilets/plumbing that can map into sanitation capability.
10. Create `docs/systems/SANITATION_WASTE_AUTHORITY_MAP.md`.
11. Create intake duplicate-search evidence linking Plans 29, 135, 156/E1-9, 158, 186/E1-17, 189/E1-18, disease, greenhouse, workshop, and survivor-needs plans.
12. Set `PREMISE_VERIFIED_AT` to current HEAD.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22B — Sanitation ownership ADR

**Goal:** Define sanitation as waste-flow/service orchestration and reject duplicate health/environment authorities.

### Required substeps

1. Write an ADR comparing one `SanitationSystem`, distributed waste-stream components, and a sanitation coordinator over waste/storage/facility authorities.
2. Define sanitation-owned facts: waste stream state, source/location, containment, routing/processing operation refs, sanitation service capacity/availability, sanitation-specific exposure events, and optionally survivor cleanliness if Needs has no owner.
3. Explicitly exclude disease state/risk, morale, air quality, potable-water quality, facility condition, power, crop yield, radiation dose, and generic shelter condition.
4. Define one facility capability contract.
5. Define one processing transaction model.
6. Define rollback/feature flags.
7. Require second-tool architecture review.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22C — Waste-stream taxonomy ADR

**Goal:** Define minimal waste classes by physical handling/health consequences rather than arbitrary labels.

### Required substeps

1. Audit source categories from real game actions.
2. Separate human blackwater/sewage from general solid waste.
3. Separate kitchen organic waste from non-compostable packaging if it creates gameplay value.
4. Separate medical biohazard from chemical and radiological hazardous waste.
5. Decide whether `HazardousMaterial` is a parent tag rather than a sixth overlapping category.
6. Prefer orthogonal hazard tags: biological, chemical, radiological, sharps, flammable, compostable, recyclable.
7. Define first-release streams.
8. Document compatibility/mixing restrictions.
9. Add taxonomy integrity tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22D — Waste quantity and unit contract

**Goal:** Define deterministic units that support conservation across generation, containment, processing, and disposal.

### Required substeps

1. Choose liters/kg/abstract waste units per stream, but do not mix liquid sewage and solid trash under one meaningless unit unless explicitly normalized.
2. Use fixed-point/integer representation.
3. Define source event quantity conversion.
4. Define container/facility capacities in matching units.
5. Define rounding/carry semantics.
6. Define byproduct output units.
7. Add boundary/overflow tests.
8. Document units per stream.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22E — Waste-stream state DTO

**Goal:** Persist waste material without storing derived rates as authoritative state.

### Required substeps

1. Define stream ID/category/hazard profile, amount, source/location/container ref, containment state, created/updated day, routing destination, and provenance aggregates.
2. Do not persist `accumulationRate`, `processingRate`, or `netAccumulation` when they are derived from current operations.
3. Do not persist global sanitation consequence scores.
4. Support bulk aggregated streams where individual bags/items add no value.
5. Reference inventory items for discrete hazardous objects when needed.
6. Add serialization and merge/split tests.
7. Bound stream count through aggregation.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22F — Deterministic waste-generation contract

**Goal:** Generate waste from canonical population and activity evidence with no routine RNG.

### Required substeps

1. Human waste derives from survivor presence/consumption/population-day policy.
2. Kitchen organic waste derives from cooking/spoilage/discard events.
3. Medical waste derives from treatment/consumable events.
4. Industrial/chemical waste derives from real workshop/crafting jobs.
5. General trash derives from item use/disposal policies only where modeled.
6. Radiological waste derives from decontamination/filter/replacement events if such materials exist.
7. Use stable generation IDs by source operation/day.
8. Add dedupe tests.
9. Do not generate waste twice from both daily population formula and source events for the same activity.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22G — Population human-waste model

**Goal:** Create the first continuous sanitation load from shelter occupancy without over-simulating physiology.

### Required substeps

1. Use survivor-days present at shelter.
2. Exclude survivors away on expeditions/outposts according to canonical placement.
3. Choose bounded per-survivor production rate compatible with campaign scale.
4. Optionally vary by food/water consumption only if Needs exposes a stable input.
5. Do not require individual bowel-event simulation.
6. Route waste to available sanitation service/containment.
7. Handle no-latrine capability.
8. Add population/time-skip tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22H — Kitchen waste integration

**Goal:** Generate organic/packaging waste from canonical food handling without duplicating spoilage.

### Required substeps

1. Consume kitchen cooking/processing events.
2. Consume spoiled-food disposal events if Inventory explicitly removes spoiled items.
3. Do not create kitchen waste merely because food exists in inventory.
4. Classify compostable versus non-compostable outputs.
5. Use stable source operation ID.
6. Add cooking/spoilage/disposal tests.
7. Prevent double counting from Kitchen and Inventory listeners.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22I — Medical waste integration

**Goal:** Create biohazard/sharps waste from real medical procedures and used consumables.

### Required substeps

1. Consume completed treatment/procedure events from MedicalPipeline.
2. Map used bandages, dressings, syringes, contaminated PPE, samples, and similar content only when actual items/actions exist.
3. Do not create medical waste from a canceled treatment.
4. Use biological/sharps hazard tags.
5. Route to approved containment/processing.
6. Do not directly increase disease risk.
7. Add treatment/cancel/dedupe tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22J — Chemical/industrial waste integration

**Goal:** Create chemical waste only from recipes/jobs that declare an explicit waste output.

### Required substeps

1. Audit workshop/crafting chemistry processes.
2. Add waste output metadata to canonical recipes where meaningful.
3. Do not assume every industrial job produces toxic waste.
4. Keep chemical identity/class tags if treatment/disposal differs.
5. Route through containment/export/approved processing.
6. Do not dump directly into water/air from generation.
7. Add job/output/conservation tests.
8. Feature-gate if industrial chemistry is not yet implemented.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22K — Radiological/hazardous waste boundary

**Goal:** Keep radioactive materials under Radiation/Contamination authority while sanitation manages physical containment logistics.

### Required substeps

1. Audit contaminated filters, clothing, soil, medical/decon waste, reactor/fuel content if any.
2. Represent physical waste object/stream with radiological hazard reference/value supplied by canonical contamination authority.
3. Sanitation owns container/routing, not dose.
4. Do not incinerate radioactive waste by generic policy.
5. Containment failure emits contamination event to Radiation/Contamination.
6. Add containment/failure/repair tests.
7. Use strict disposal compatibility.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22L — Waste source-location topology

**Goal:** Track where waste originates and where service capacity exists without creating a second shelter map.

### Required substeps

1. Use E1-9 canonical room/node IDs.
2. Associate source streams/facilities with room/node or site.
3. Use canonical connections for collection/plumbing routes where meaningful.
4. Do not store duplicate room coordinates.
5. Define central versus room-local bins/latrines.
6. Handle room removal/renovation.
7. Add topology reference tests.
8. Keep pathing abstract unless Duty/transport requires it.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22M — Sanitation service capability model

**Goal:** Represent access to toilets, washing, collection, processing, containment, and recycling as typed capabilities.

### Required substeps

1. Define capability IDs and canonical facility provider refs.
2. Examples: human_waste_collection, sewage_storage, compost_processing, general_waste_processing, biohazard_containment, hazardous_containment, washing_service, greywater_collection.
3. Do not persist one 0–100 sanitation efficiency scalar.
4. Capacity/throughput comes from real installed/operational facilities.
5. Condition is queried from E1-17.
6. Power is queried from PowerGrid.
7. Add capability-resolution tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22N — Facility identity and E1-9 construction boundary

**Goal:** Install sanitation infrastructure through canonical construction/topology systems.

### Required substeps

1. Reuse E1-9 room/infrastructure slot IDs.
2. Facility definition describes sanitation capabilities, capacity, inputs/outputs, power/fuel needs, and maintenance asset refs.
3. E1-9 owns installation/removal/upgrade.
4. Do not create facilities directly in sanitation save without topology provenance.
5. Handle pre-authored starting facilities.
6. Add install/remove/upgrade reference tests.
7. Keep facility location canonical.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22O — Facility condition and E1-17 boundary

**Goal:** Reuse maintainable-asset condition instead of a sanitation-local condition meter.

### Required substeps

1. Latrine/septic hardware, pumps, processor, incinerator, recycler, containment seals, and pipes reference E1-17 assets where applicable.
2. Sanitation queries availability/throughput capability.
3. E1-17 owns wear, maintenance, repair, failure.
4. Do not store `lastMaintenanceDay` locally.
5. Failure emits capability change; sanitation reacts by rerouting/overflow.
6. Add healthy/degraded/failed/repaired tests.
7. Assert no duplicate condition owner.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22P — Latrine capability vertical slice

**Goal:** Implement basic human-waste service without pretending a latrine destroys waste.

### Required substeps

1. Define collection/containment capacity.
2. Human waste enters latrine/holding stream.
3. Manual emptying or downstream transfer is required if design uses finite capacity.
4. Facility use requires no power unless infrastructure says so.
5. Overflow creates sanitation exposure, not direct disease.
6. Add full/empty/overflow/transfer tests.
7. Use as first sanitation facility.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22Q — Septic tank boundary

**Goal:** Treat septic capacity, settling/treatment, pumping, and overflow according to a bounded game abstraction.

### Required substeps

1. Define whether septic tank is storage only or provides partial biological treatment.
2. Do not call storage `processing` if no actual sink/output exists.
3. Define sludge/effluent outputs if treatment exists.
4. Require pumping/emptying destination.
5. E1-17 owns tank/pump condition.
6. Overflow emits environmental exposure.
7. Add capacity/pumping/failure tests.
8. Document abstraction.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22R — Composting toilet and compost processing

**Goal:** Allow nutrient recovery only from explicitly compatible inputs and completed processing.

### Required substeps

1. Define accepted human/kitchen organic inputs separately.
2. Define pathogen-control/maturation policy as a game abstraction.
3. Reject medical, chemical, radiological, plastics, and incompatible trash.
4. Output compost/fertilizer becomes canonical inventory/agriculture resource only after processing transaction.
5. Greenhouse owns crop effect.
6. Do not grant fertilizer instantly on waste input.
7. Add contamination/compatibility/maturation tests.
8. Use conservative content wording.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22S — General waste processor

**Goal:** Reduce/transform solid waste through real power/time/output transactions.

### Required substeps

1. Define accepted categories.
2. Define throughput and power requirement.
3. Define output: compacted waste, recyclables, ash, salvage, or reduced-volume waste depending on content.
4. Do not simply delete 100% mass without an explicit sink/abstraction.
5. PowerGrid owns power.
6. E1-17 owns condition.
7. Inventory owns salvage outputs.
8. Add processing/conservation/power-loss tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22T — Trash incinerator safety boundary

**Goal:** Model incineration as waste transformation with fuel/power and emissions.

### Required substeps

1. Define allowed combustible waste categories.
2. Reject incompatible chemical/radiological/medical streams unless a specialized policy explicitly supports them.
3. Consume fuel/power through canonical systems.
4. Reduce solid waste into ash/residue.
5. Emit smoke/particulate/CO source event to Ventilation.
6. Do not directly set air quality.
7. Add no-power/no-fuel/wrong-waste/emission/ash tests.
8. Document environmental tradeoff.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22U — Hazardous-waste containment

**Goal:** Provide secure storage/segregation rather than magical disposal.

### Required substeps

1. Define supported hazard classes.
2. Containment capacity is finite.
3. Condition/seal state comes from E1-17.
4. Contained waste remains in canonical sanitation/inventory state.
5. Full containment blocks new transfer.
6. Leak/breach emits typed contamination exposure to relevant authority.
7. Add full/leak/repair/transfer tests.
8. Require later export/decontamination/disposal sink.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22V — Greywater generation boundary

**Goal:** Generate reusable wastewater from washing/cooking only when water consumption is modeled.

### Required substeps

1. Audit E1-18/water-storage activity use.
2. Greywater volume derives from real water-use transactions, not a parallel estimate if canonical usage exists.
3. Do not mix blackwater/sewage into greywater automatically.
4. Assign contaminant profile appropriate to source activity using water authority contract.
5. Sanitation manages collection/routing.
6. Add generation/conservation tests.
7. Feature-gate if washing/cooking water use is not explicit.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22W — Water recycler integration

**Goal:** Route greywater into E1-18/WaterTreatment rather than purifying it inside sanitation.

### Required substeps

1. Recycler facility provides intake capability/route.
2. E1-18/WaterTreatment owns treatment stages, quality, efficiency, and output.
3. E1-17 owns recycler hardware condition.
4. PowerGrid owns electricity.
5. Water storage owns recycled output volume.
6. Sanitation does not store `waterRecyclingEfficiency`.
7. Add healthy/power-loss/failure/unsafe-input tests.
8. Prevent duplicate water creation.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22X — Waste transfer and collection jobs

**Goal:** Move waste between source containers and processing/containment through canonical labor when the logistics decision matters.

### Required substeps

1. Define source/destination/container/quantity.
2. Validate compatible route and facility capacity.
3. Use Duty/job scheduler.
4. Assign worker and protective equipment where required.
5. Reserve waste quantity and destination capacity.
6. Create stable transfer operation ID.
7. Commit move exactly once.
8. Handle interruption/cancel/save-load.
9. Add conservation tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22Y — Cleaning and decontamination jobs

**Goal:** Separate cleaning of spaces from disposal of bulk waste.

### Required substeps

1. Define cleaning target room/surface exposure state.
2. Use water/cleaning-supply transactions if modeled.
3. Use Duty/job scheduler.
4. Cleaning may reduce sanitation contamination/exposure state but does not cure disease.
5. Decontamination may route hazardous residue into a waste stream.
6. Radiological decon uses contamination authority.
7. Add clean/decon/resource/byproduct tests.
8. Prevent cleaning from deleting bulk contained waste.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22Z — Sewage backup state and event

**Goal:** Represent plumbing/containment failure as a concrete sanitation exposure rather than a global meter decrement.

### Required substeps

1. Trigger when inflow exceeds available storage/transfer or a critical blockage/failure occurs.
2. Use affected room/node.
3. Create stable backup event ID.
4. Create biological/odor/moisture exposure terms.
5. Ventilation/Disease/Water/Sump/Needs consume relevant pieces.
6. Do not directly infect survivors.
7. Remain active until source stopped/cleaned according to policy.
8. Add onset/persistence/cleanup tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AA — Waste overflow state and event

**Goal:** Represent overflowing solid waste as a localized exposure and service failure.

### Required substeps

1. Trigger from container/source capacity.
2. Track affected location and waste/hazard class.
3. Allow odor/pest/social consequence policies through canonical systems.
4. Do not roll disease directly.
5. Require collection/cleanup.
6. Use stable overflow ID.
7. Add repeated-overflow/dedupe/cleanup tests.
8. Keep current quantity authoritative.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AB — Biohazard exposure contract

**Goal:** Translate uncontained biological waste into canonical disease exposure without owning infection probability.

### Required substeps

1. Define exposure ID, location, source waste stream, hazard class, duration/intensity band, survivor exposure refs if known, and remediation state.
2. DiseaseSystem consumes exposure according to its own vector model.
3. Do not store `diseaseRisk` in sanitation.
4. Use PPE/health rules only through canonical consumers.
5. Add exposed/unexposed/PPE/cleanup tests.
6. Prevent one waste event from generating duplicate disease exposures.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AC — Chemical spill handoff

**Goal:** Route chemical waste leaks through canonical contamination/health systems.

### Required substeps

1. Create typed chemical exposure/spill event with location and quantity/class.
2. Contamination/Health owns toxicity/exposure consequence.
3. Ventilation may consume volatile-air source if relevant.
4. Water may consume runoff/ingress if spill reaches drainage.
5. Sanitation owns cleanup/containment logistics.
6. Do not hard-code survivor damage.
7. Add spill/containment/cleanup tests.
8. Feature-gate unsupported chemical classes.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AD — Radiological waste breach handoff

**Goal:** Route breached contaminated waste to Radiation/Contamination exactly once.

### Required substeps

1. Use canonical radiological content/provenance.
2. Containment failure produces location contamination source event.
3. Radiation authority owns dose/exposure.
4. Sanitation owns physical waste transfer/cleanup operation.
5. Do not convert radioactive waste into generic hazardous score.
6. Add breach/cleanup/repair tests.
7. Ensure incinerator rejects it by default.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AE — Ventilation and odor/emission integration

**Goal:** Make sanitation affect air only through explicit emission/odor source events.

### Required substeps

1. Incinerator emits smoke/CO/particulate source.
2. Decomposing exposed organic/sewage waste may emit odor/bioaerosol source if Ventilation supports such categories.
3. Ventilation owns room air quality.
4. Do not set air quality or filter saturation directly.
5. Filters may accumulate wear through E1-17/Ventilation, not sanitation.
6. Add emission/ventilation-off/filter tests.
7. Keep odor primarily presentation unless a canonical health effect exists.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AF — Sump/flooding integration

**Goal:** Allow sanitation failures to contaminate infiltrated/flood water without duplicating flood state.

### Required substeps

1. SumpFlooding owns water presence/level.
2. If sewage/chemical waste contacts flooded node, sanitation emits contamination-source event.
3. E1-18/contamination authority owns resulting water quality.
4. Do not increase sump level.
5. Do not create flood from sanitation unless a pipe/sewage release physically adds water through a supported API.
6. Add dry-node/wet-node/backup/cleanup tests.
7. Use provenance to dedupe.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AG — DiseaseSystem integration

**Goal:** Feed explicit vectors/exposures rather than a global sanitation multiplier.

### Required substeps

1. Audit DiseaseSystem vector inputs.
2. Prefer contact/fomite/biohazard/waterborne exposure events.
3. Allow a shelter-wide sanitation context multiplier only if DiseaseSystem already has a canonical environment modifier API and the value is derived from specific exposures.
4. Do not store disease transmission multiplier in sanitation save.
5. Do not roll outbreak events locally.
6. Add tests for exposed waste with no survivors, survivor exposure, clean isolation, and outbreak handled by Disease.
7. Keep vector ownership documented.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AH — Hygiene ownership ADR

**Goal:** Decide whether survivor hygiene belongs in Needs, a survivor component, or sanitation service state.

### Required substeps

1. Audit Needs for hygiene/cleanliness.
2. If hygiene already exists, sanitation only provides washing access/exposure inputs.
3. If absent and gameplay justifies it, add hygiene as a canonical survivor need/component rather than a sanitation-local duplicate.
4. Define cleanliness as state, not disease risk/morale impact.
5. Define decay from activity/environment and restoration from washing.
6. Do not duplicate bathing/water consumption.
7. Require second-tool review before adding per-survivor state.
8. Add ownership tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AI — Hygiene decay and exposure policy

**Goal:** If hygiene is accepted, make it contextual and bounded instead of a constant arbitrary daily tax.

### Required substeps

1. Use survivor presence, work type, combat/expedition return, heat/sweat if canonical, waste exposure, medical work, and washing access where supported.
2. Use a small baseline only if needed.
3. Do not reduce hygiene twice through Needs and Sanitation.
4. Use fixed-point deterministic updates.
5. Define floor/ceiling.
6. Add activity-specific tests.
7. Time skip must be equivalent.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AJ — Washing action and water-cost integration

**Goal:** Restore hygiene through a real activity that consumes canonical water where appropriate.

### Required substeps

1. Define washing facility/capability.
2. Use Duty/free-time/autonomy action.
3. Water authority reserves/consumes water.
4. Sanitation supplies service availability.
5. Needs/hygiene owner applies cleanliness change.
6. Do not mint greywater or consume water twice.
7. Generate greywater from the same transaction if enabled.
8. Add no-water/no-facility/interruption/save tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AK — Hygiene social-effect boundary

**Goal:** Reject automatic social ostracism from a low cleanliness number unless a real social event occurs.

### Required substeps

1. Relations owns relationship consequences.
2. Low hygiene may become dialogue/social-context input.
3. Repeated severe hygiene plus close contact may affect social events if authored.
4. Do not directly subtract affinity from every survivor pair.
5. Do not stigmatize illness/hygiene mechanically without context.
6. Add boundary tests.
7. Keep effects humane and explainable.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AL — Needs/morale consequence boundary

**Goal:** Use significant sanitation conditions as canonical stressor/comfort events rather than a persistent hidden morale multiplier.

### Required substeps

1. Examples: sewage backup, unusable toilets, severe odor/overflow, restored sanitation service.
2. Needs/MentalHealth owns morale/stress.
3. Do not apply a daily +morale for sanitation >80.
4. Do not persist `moraleImpact`.
5. Use stable event IDs.
6. Add dedupe tests.
7. Measure event frequency.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AM — Vermin gate

**Goal:** Do not create a new pest ecology unless a canonical wildlife/pest authority exists.

### Required substeps

1. Audit rats/insects/vermin content.
2. If no pest system exists, treat vermin as authored sanitation event/presentation with bounded consequences routed to Disease/Inventory.
3. Do not add autonomous vermin population simulation inside Sanitation.
4. Use waste exposure eligibility.
5. Feature-gate detailed vermin mechanics.
6. Add simple event tests if enabled.
7. Document deferral.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AN — Processing transaction and conservation

**Goal:** Transform waste through explicit input/output/byproduct accounting.

### Required substeps

1. Define processing operation ID, facility ID, input stream IDs/quantities, output stream/item IDs, byproducts, consumed fuel/power/material refs, start/end state, and result.
2. Reserve input and facility capacity.
3. Commit once.
4. Input quantity equals outputs + explicit destruction/emission abstraction.
5. Do not delete waste on start if cancellation can occur.
6. Add retry/cancel/save-load tests.
7. Add property-based conservation tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AO — Waste mixing and compatibility

**Goal:** Prevent incompatible waste streams from being merged or processed incorrectly.

### Required substeps

1. Define compatibility by stream/hazard tags.
2. General and compostable waste may be separable.
3. Medical/sharps remain segregated.
4. Chemical classes may require separate containment.
5. Radiological waste never merges into ordinary trash by default.
6. Mixed contaminated waste inherits required hazard constraints.
7. Add merge/split/contamination escalation tests.
8. Keep rules data-driven.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AP — Waste export/disposal boundary

**Goal:** Provide a true sink for waste that cannot be processed on-site without deleting it magically.

### Required substeps

1. Audit expedition/trade/world disposal possibilities.
2. Export may move waste to a canonical caravan/inventory container and external disposal location/contract.
3. E1-19 trade route may support waste shipment only if markets/contracts accept it.
4. World dumping creates contamination event if allowed; do not silently erase waste.
5. Do not assume off-map disposal is free.
6. Use real cargo/cost/risk.
7. Add export/loss/illegal-dumping tests.
8. Feature-gate if no external sink exists.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AQ — Greenhouse compost integration

**Goal:** Hand safe mature compost to Agriculture as a canonical fertilizer input.

### Required substeps

1. Composting process creates real fertilizer/compost item/resource.
2. Greenhouse owns application and crop effect.
3. Do not directly increase yield from sanitation.
4. Reject contaminated compost.
5. Track nutrient/quality bands only if Agriculture can consume them.
6. Add safe/unsafe/application tests.
7. Prevent compost duplication.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AR — PowerGrid integration

**Goal:** Make powered processors/recyclers/pumps/incinerators depend on canonical power.

### Required substeps

1. Facility definition declares power demand/capability.
2. PowerGrid owns supply/brownout/load shedding.
3. Sanitation queries operational power.
4. Do not store independent `isActive` if it is derivable from installed+condition+power+operator state.
5. Handle partial throughput if owner system supports it.
6. Add powered/unpowered/brownout tests.
7. Keep fuel ownership external.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AS — Duty and staffing integration

**Goal:** Use canonical jobs for collection, cleaning, pumping, sorting, processing, inspection, and emergency cleanup.

### Required substeps

1. Define action/job factories.
2. Use survivor skills/roles as eligibility inputs.
3. E1-20 Engineer/Technician/Sanitation specialization may influence eligibility if later authored.
4. Do not create a sanitation worker scheduler.
5. Respect critical Duty priorities.
6. Handle PPE/tool requirements through canonical equipment.
7. Add assignment/interruption tests.
8. Keep routine automation possible via Duty policy.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AT — Inspection and diagnostics

**Goal:** Make sanitation inspection reveal service/capacity/exposure problems without duplicating E1-17 condition inspection.

### Required substeps

1. Sanitation inspection can audit fill levels, routing, waste segregation, overflow risk, service bottlenecks, and cleanliness/exposure state.
2. Mechanical condition inspection remains E1-17.
3. Do not let inspection improve sanitation by itself.
4. Use canonical job if time/skill matters.
5. Create stable inspection report ID.
6. Add tests for clean/overloaded/hidden blockage where supported.
7. Keep reports bounded.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AU — Derived sanitation risk summary

**Goal:** Provide a player-facing overall status as a read model only.

### Required substeps

1. Compute status from uncontained waste, overflow, sewage service availability, hazardous containment, washing access, active exposure events, processing backlog, and facility capacity.
2. Use bands such as Good/Strained/Poor/Critical rather than authoritative 0–100 unless UI convention strongly prefers a number.
3. Do not feed the summary back into Disease/Needs/Water/Ventilation.
4. Show contributing reasons.
5. Add consistency tests.
6. Do not persist the derived score.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AV — Waste-flow and capacity forecast

**Goal:** Show where backlog will occur without storing fake accumulation rates.

### Required substeps

1. Derive generation rate from recent/canonical source policies.
2. Derive processing capacity from installed operational facilities.
3. Derive storage headroom.
4. Estimate days-to-overflow.
5. Show bottleneck by waste stream.
6. Do not persist forecast as truth.
7. Account for power/maintenance outages where known.
8. Add forecast consistency tests.
9. Use broad confidence if future usage is uncertain.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AW — Sanitation dashboard UI

**Goal:** Create one operational surface for waste streams, capacity, exposure, facilities, and jobs without duplicating other subsystem dashboards.

### Required substeps

1. Show waste categories/quantities/backlog.
2. Show storage headroom.
3. Show facility capabilities and operational state from owners.
4. Show active overflows/backups/exposures.
5. Show sanitation-risk summary with reasons.
6. Show current jobs.
7. Link mechanical faults to E1-17.
8. Link water recycler to E1-18.
9. Add snapshot tests for normal/strained/overflow/biohazard/power-failure states.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AX — Facility detail UI

**Goal:** Show sanitation-specific throughput and inputs/outputs while querying condition/power from canonical owners.

### Required substeps

1. Show facility type/location.
2. Show accepted waste streams.
3. Show current input/output/backlog.
4. Show capacity/throughput.
5. Show condition from E1-17.
6. Show power from PowerGrid.
7. Show required job/resources.
8. Do not provide local repair button that bypasses E1-17.
9. Add detail snapshot tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AY — Hygiene UI

**Goal:** If hygiene is implemented, present cleanliness/service access without exposing disease probability as a fake certainty.

### Required substeps

1. Show hygiene/cleanliness band.
2. Show last wash/service availability if useful.
3. Show major exposure/reason.
4. Show washing action.
5. Do not display sanitation-local disease-risk percent.
6. Do not shame/stigmatize survivor language.
7. Add snapshot tests.
8. Feature-gate if hygiene owner is rejected.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22AZ — Sanitation map/read model

**Goal:** Visualize known waste sources, collection points, processing, containment, and problem areas over canonical shelter topology.

### Required substeps

1. Use E1-9 room/node layout.
2. Show facilities by reference.
3. Show active overflow/backups.
4. Show routing/flow only if it helps decisions.
5. Do not create independent simulation coordinates.
6. Do not expose hidden contamination beyond knowledge rules.
7. Add snapshot tests.
8. Keep map optional if topology UI cannot support it cleanly.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BA — Alerts and accessibility

**Goal:** Emit semantic warnings for significant sanitation failures without meter spam.

### Required substeps

1. Warn on sewage backup.
2. Warn on hazardous containment breach.
3. Warn on critical storage nearing overflow.
4. Warn when no human-waste service remains.
5. Warn on water-recycler outage only through correlated owner status.
6. Coalesce repeated warnings.
7. Use E1-12 accessible visual equivalents for any critical audio cue.
8. Add dedupe/cooldown tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BB — Journal and event contract

**Goal:** Record major sanitation milestones without logging routine waste generation.

### Required substeps

1. Use canonical journal/chronicle.
2. Record first sanitation facility, major overflow, sewage crisis, hazardous breach, major cleanup, recycler activation, sanitation recovery.
3. Do not log daily trash increments.
4. Do not label Disease outbreak as sanitation-caused unless provenance supports it.
5. Use stable event IDs.
6. Add dedupe/retention tests.
7. Keep journal non-authoritative.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BC — Quest hook contract

**Goal:** Add sanitation objectives only after core waste/resource loops are stable.

### Required substeps

1. Prioritize first sanitation upgrade, recover from backup, safely process medical waste, establish recycling/composting, and maintain no critical overflow for a bounded period.
2. Do not require arbitrary 100-day perfect sanitation if it becomes passive/grindy.
3. Do not require every survivor above a hygiene threshold if hygiene is not a canonical need.
4. QuestSystem owns lifecycle/rewards.
5. Use facility/operation/event IDs as provenance.
6. Add dedupe/invalidation tests.
7. Keep broad quest corpus deferred.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BD — Old-save migration strategy

**Goal:** Introduce sanitation prospectively without retroactive waste debt or fabricated physical infrastructure.

### Required substeps

1. Do not back-calculate waste from campaign day.
2. Do not create a free physical latrine unless an existing room/fixture maps to it.
3. If current shelter behavior implicitly assumes basic sanitation, provide a temporary legacy sanitation service capability or map existing toilet/plumbing content through migration ADR.
4. Preserve all water, disease, needs, power, room, and inventory state.
5. Initialize waste streams empty/prospective unless existing explicit waste items exist.
6. Do not emit first-load overflows.
7. Version migration.
8. Add early/late/expanded-shelter old-save fixtures.
9. Make migration idempotent.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BE — Legacy sanitation compatibility retirement

**Goal:** Remove any migration-only compatibility capability safely once the player has real infrastructure.

### Required substeps

1. Define whether legacy service is permanent parity shim or retires after tutorial/construction milestone.
2. Never strand a save with unavoidable waste because migration capability vanished before build path existed.
3. If replaced, require installed operational facility first.
4. Do not refund imaginary infrastructure.
5. Document behavior.
6. Add transition tests.
7. Keep old-save parity measurable.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BF — Save contract and restoration order

**Goal:** Persist sanitation-owned waste/service/exposure state without copying domain systems.

### Required substeps

1. Persist waste streams/aggregates, containment/routing state, processing operation refs if not owned by generic jobs, sanitation exposure incidents, optional hygiene only if sanitation is chosen as owner, receipts/cursors, and schema version.
2. Do not persist facility condition, current power, disease risk, morale, air quality, potable-water quality, crop effect, or room topology copies.
3. Restore topology/facilities/Inventory before sanitation stream references.
4. Restore E1-17/Power/Water before resolving facility capabilities.
5. Restore survivor/Needs before hygiene access.
6. Do not replay overflow/exposure events.
7. Add corruption/round-trip tests.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BG — Time-skip and batch processing

**Goal:** Advance waste generation, facility throughput, jobs, and capacity boundaries exactly once.

### Required substeps

1. Use CampaignCalendar only.
2. Do not use wall-clock time.
3. Aggregate deterministic survivor-day waste generation.
4. Use real source events inside skipped interval when available.
5. Process facility throughput subject to power/condition/job availability.
6. Detect threshold/overflow transition at correct point or conservatively within interval.
7. Add 1/7/30/100-day skip tests.
8. Compare stepped versus batch-equivalent results.
9. Prevent reload duplicate generation.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BH — Zero-RNG baseline

**Goal:** Remove unnecessary random sanitation events from routine simulation.

### Required substeps

1. Base waste generation is deterministic.
2. Facility throughput is deterministic from capacity/inputs/power/condition.
3. Overflow is deterministic from quantity/capacity.
4. Hygiene change is deterministic.
5. Disease outbreak remains DiseaseSystem-owned.
6. If vermin/spill accident events need randomness, use Event/Disaster authority with keyed RNG and persisted outcome.
7. Add same-input determinism tests.
8. Document zero-RNG baseline.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BI — Waste conservation and exploit audit

**Goal:** Prove waste cannot vanish, multiply, or produce free fertilizer/water through transaction edges.

### Required substeps

1. Test generation once.
2. Test transfer cancel/retry.
3. Test processing cancel/retry.
4. Test compost output.
5. Test incineration residue.
6. Test recycler water conservation.
7. Test export/disposal.
8. Test mixed waste.
9. Test save/reload mid-operation.
10. Assert input/output/bysink conservation.
11. Add property tests.
12. Audit all first-release recipes.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BJ — Disease/contamination deduplication audit

**Goal:** Prove one sanitation failure produces one exposure path and no duplicate health effect.

### Required substeps

1. Test sewage backup -> biohazard exposure -> Disease.
2. Verify Sanitation does not also roll outbreak.
3. Test chemical leak -> contamination authority.
4. Test radiological breach -> Radiation.
5. Test incinerator emissions -> Ventilation.
6. Test sump contact -> water contamination.
7. Use correlation IDs.
8. Assert no duplicate consequence event.
9. Document causality traces.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BK — Sanitation player-burden audit

**Goal:** Keep the system strategic rather than repetitive janitorial clicking.

### Required substeps

1. Measure waste-management actions per shelter-week.
2. Allow routine collection/processing through standing Duty policy where supported.
3. Use facility capacity buffers.
4. Use warnings before overflow.
5. Use monitoring/forecasting.
6. Do not require manual emptying every day.
7. Use infrastructure upgrades to reduce labor burden.
8. Cap alerts.
9. Measure washing/hygiene micromanagement if enabled.
10. Tune rates from campaign duration/population.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BL — Waste economy and facility-balance simulation

**Goal:** Ensure sanitation consumes meaningful labor/resources without dominating shelter management.

### Required substeps

1. Simulate low, medium, and high population.
2. Simulate minimal/basic/advanced infrastructure.
3. Measure waste generated/day, processing capacity, storage headroom, labor-hours, power, fuel, water, maintenance parts, compost output, and emergency incidents.
4. Ensure competent basic sanitation is viable.
5. Ensure advanced infrastructure reduces labor/risk but costs resources/power.
6. Ensure compost/recycling outputs do not exceed input value absurdly.
7. Use actual campaign duration.
8. Document accepted ranges.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BM — Facility/content complexity budget

**Goal:** Ship only facilities with distinct logistics choices.

### Required substeps

1. Start with basic latrine/holding, one sewage or compost path, one general-waste path, and one hazardous containment path.
2. Add water recycler only through E1-18.
3. Add incinerator only if emissions/fuel tradeoff matters.
4. Merge facilities whose only difference is a cosmetic efficiency percentage.
5. Do not force seven facility types to satisfy source count.
6. Cap dashboard complexity.
7. Review after vertical slice.
8. Document deferrals.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BN — Performance and long-campaign sanitation soak

**Goal:** Prove waste generation, routing, processing, hygiene, exposure, and UI remain bounded at max shelter scale.

### Required substeps

1. Run supported maximum-duration headless simulation with expanding population and E1-9 shelter growth.
2. Exercise kitchen/medical waste, sewage, power outages, E1-17 failures, overflow, cleanup, recycling, composting, and one hazardous breach.
3. Measure active stream count, generation events, transfer/jobs, processing operations, exposure events, save size, allocations, and dashboard read-model cost.
4. Aggregate compatible streams.
5. Use event/interval-driven updates.
6. Do not scan every survivor/facility every frame.
7. Record median/p95 sanitation processing cost.
8. Add regression thresholds.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BO — Headless sanitation selftest

**Goal:** Build a deterministic CI scenario proving generation, containment, processing, failure handoff, hygiene boundary, and save/load.

### Required substeps

1. Load shelter with one basic sanitation capability.
2. Generate human waste from survivor-days.
3. Generate kitchen and medical waste from canonical events.
4. Route human waste into containment.
5. Process one compatible stream.
6. Force capacity overflow.
7. Verify one biohazard exposure enters DiseaseSystem without direct infection mutation.
8. Fail one facility through E1-17 and verify sanitation capacity changes.
9. Power-cycle one processor.
10. Process greywater through E1-18 if enabled.
11. Save/reload at overflow/processing boundary.
12. Create `--sanitation-selftest`.
13. Assert sanitation state contains no copied disease risk, morale, air quality, water quality, facility condition, or power state.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BP — Data-integrity selftest

**Goal:** Validate waste categories, hazard tags, facility capabilities, topology refs, processing rules, item refs, and owner boundaries.

### Required substeps

1. Validate unique waste/facility definitions.
2. Validate accepted/rejected stream compatibility.
3. Validate processing input/output conservation declarations.
4. Validate facility capability IDs.
5. Validate room/topology refs.
6. Validate E1-17 asset refs.
7. Validate E1-18/WaterTreatment refs.
8. Validate Inventory items/byproducts.
9. Validate skill/job refs.
10. Validate no facility embeds direct disease/morale/air/water mutations.
11. Fail with actionable diagnostics.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BQ — Documentation and observability

**Goal:** Document waste ownership, facility contracts, processing conservation, exposure handoffs, hygiene ownership, and migration.

### Required substeps

1. Create `docs/systems/SANITATION_WASTE.md`.
2. Create `docs/systems/WASTE_STREAM_CONTRACT.md`.
3. Create `docs/systems/SANITATION_FACILITY_CAPABILITIES.md`.
4. Create `docs/systems/SANITATION_EXPOSURE_HANDOFFS.md`.
5. Create `docs/systems/HYGIENE_OWNERSHIP.md` if hygiene is enabled.
6. Document zero-RNG baseline.
7. Document E1-17/E1-18/E1-9 boundaries.
8. Add debug readout for stream quantities, locations, containment, capacity, processing ops, exposure IDs, facility capabilities, and blockers.
9. Keep debug mutations dev-only.
10. Update plan register/intake.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

## E1-22BR — Release gate and closure

**Goal:** Ship only when sanitation creates real waste logistics and environmental-health consequences without becoming a second disease, water, air, maintenance, or needs system.

### Required substeps

1. Run .NET build/test and game build.
2. Run data-integrity selftest.
3. Run sanitation selftest.
4. Run authority-boundary tests.
5. Run waste-generation/dedupe tests.
6. Run processing/conservation tests.
7. Run sewage/overflow exposure tests.
8. Run Disease/Ventilation/Sump/Water handoff tests.
9. Run E1-17/E1-9/Power facility tests.
10. Run hygiene ownership tests if enabled.
11. Run old-save migration tests.
12. Run time-skip/determinism tests.
13. Run exploit and sanitation-burden tests.
14. Run long-campaign performance/economy soak.
15. Verify 3–4 waste streams and 3–4 facilities before six/seven expansion.
16. Update ADR, authority map, docs, plan register, and handoff.
17. Mark DONE only when the shelter must manage waste materially while every downstream health/environment authority remains singular.

**Gate:** Preserve canonical ownership, deterministic provenance/idempotency, material conservation, save/load safety, and package-specific negative duplicate-authority evidence.

---

# 6. Canonical Sanitation Authority Matrix

| Fact | Canonical owner | E1-22 role |
|---|---|---|
| Waste stream amount/location | E1-22 or Inventory for discrete items | Own/reference |
| Shelter room/topology | E1-9 | Reference |
| Facility installation | E1-9/Construction | Reference |
| Facility condition/repair | E1-17 | Query |
| Power availability | PowerGrid | Query |
| Disease/infection | DiseaseSystem | Exposure handoff only |
| Morale/stress | Needs/MentalHealth | Significant-event handoff |
| Survivor hygiene | Needs/survivor component or E1-22 after ADR | One owner |
| Air quality | Ventilation | Emission source only |
| Flood/sump level | SumpFlooding | Read / contamination contact |
| Water treatment | E1-18/WaterTreatment | Greywater handoff |
| Stored water | Water storage/Inventory | No duplicate |
| Kitchen spoilage/cooking | KitchenNutrition | Waste source evidence |
| Medical treatment | MedicalPipeline | Waste source evidence |
| Chemical/radiological exposure | Contamination/Radiation/Health | Typed handoff |
| Greenhouse crop effect | Agriculture | Compost item input |
| Duty/jobs | DutyRoster/Job system | Work execution |
| Journal | Chronicle | Significant history only |

---

# 7. Suggested Waste Stream Record

```yaml
schema_version: 1
stream_id: "waste_human_shelter_day_084"
category: "HUMAN_BLACKWATER"
hazards:
  - "BIOLOGICAL"

amount_ml: 184000
location_id: "room_sanitation_block"

containment:
  container_ref: "latrine_holding_01"
  state: "CONTAINED"

destination_policy_id: "human_waste_basic"
last_updated_day: 84
```

No disease-risk or morale field appears here.

---

# 8. Waste Taxonomy Recommendation

Prefer category + hazard tags.

Examples:

```text
HUMAN_BLACKWATER
  hazards: biological

KITCHEN_ORGANIC
  hazards: biological, compostable

MEDICAL_BIOHAZARD
  hazards: biological, sharps(optional)

CHEMICAL_PROCESS
  hazards: chemical

GENERAL_SOLID
  hazards: combustible/recyclable by subtype

CONTAMINATED_SOLID
  hazards: radiological and/or chemical/biological
```

This avoids a vague overlapping `HazardousMaterial` bucket.

---

# 9. Processing Conservation Contract

For one processing operation:

```text
input mass/volume
=
retained output
+ useful recovered output
+ residue
+ emitted/discharged sink
```

Every sink must be explicit.

Examples:

- composting -> mature compost + residual loss;
- incineration -> ash + emissions;
- compaction -> compacted trash;
- recycling -> recovered material + residue;
- external disposal -> transferred cargo leaves shelter.

---

# 10. Sanitation Risk Read Model

Derived factors may include:

- uncontained human waste;
- sewage overflow;
- exposed medical waste;
- hazardous containment breach;
- days to storage overflow;
- unavailable toilet service;
- unavailable washing;
- processing backlog;
- active contamination events.

Possible UI bands:

- Stable
- Strained
- Poor
- Critical

This summary must not be read by DiseaseSystem as a hidden master multiplier.

---

# 11. Human Waste Pipeline

```text
survivor-days at shelter
  |
  v
human waste generation
  |
  v
toilet/latrine collection capability
  |
  v
holding / septic / compost route
  |
  +--> capacity available
  +--> capacity exhausted -> backup/overflow
```

No individual toilet event simulation is required.

---

# 12. Kitchen Waste Pipeline

```text
cooking/spoilage/disposal event
  |
  v
organic + packaging waste outputs
  |
  +--> compostable path
  +--> general solid path
```

Do not generate waste from inventory existence alone.

---

# 13. Medical Waste Pipeline

```text
completed treatment
  |
  v
used consumables / contaminated material
  |
  v
medical biohazard stream
  |
  +--> secure containment
  +--> approved treatment/export
```

No direct disease roll.

---

# 14. Greywater Pipeline

```text
canonical washing/cooking water use
  |
  v
greywater generation
  |
  v
collection route
  |
  v
E1-18 / WaterTreatment
  |
  v
canonical water storage
```

Do not route blackwater into greywater unless a dedicated sewage-treatment system exists.

---

# 15. Sewage Backup Causality

Correct:

```text
storage/pipe service exceeded or failed
-> sewage backup exposure exists in room
-> Disease receives biological exposure
-> Ventilation may receive odor/bioaerosol source
-> Water/Contamination may receive contact event
-> Needs may receive significant sanitation-stressor event
```

Incorrect:

```text
sanitationLevel -= 50
diseaseRisk += 40
morale -= 20
waterContamination += 30
```

---

# 16. Facility Ownership

A sanitation facility has several dimensions owned by different systems:

```text
installed location -> E1-9
mechanical condition -> E1-17
power -> PowerGrid
waste throughput -> E1-22
water treatment -> E1-18/WaterTreatment
air emissions -> Ventilation
```

Do not collapse these into one DTO with copied state.

---

# 17. Compost Safety

Compost inputs require explicit compatibility.

Default reject:

- medical biohazard;
- sharps;
- industrial chemical waste;
- battery acid;
- radiological waste;
- contaminated filters;
- mixed unknown hazardous waste.

Human-waste composting, if enabled, should be an explicit game abstraction with maturation/treatment and
clear segregation.

---

# 18. Incineration Safety

Incineration is appropriate only for allowed combustible waste classes.

Potential outputs:

- ash;
- smoke;
- particulate;
- CO;
- contaminated ash if input contained hazards.

Do not treat incineration as universal hazardous-waste destruction.

---

# 19. Hygiene Ownership Decision Tree

```text
Does NeedsSystem already own hygiene?
  |
  +--> yes -> sanitation supplies washing access/exposure context only
  |
  +--> no
        |
        v
Does hygiene create enough gameplay value?
  |
  +--> no -> omit per-survivor hygiene
  |
  +--> yes -> add one canonical survivor hygiene component/need
```

Never create hygiene twice.

---

# 20. Washing Transaction

```text
survivor chooses/is assigned wash action
  |
  v
facility available?
water available?
time available?
  |
  v
canonical water transaction
  |
  v
hygiene owner increases cleanliness
  |
  v
optional greywater output generated from same water transaction
```

No duplicate water consumption or greywater creation.

---

# 21. Disease Boundary

Sanitation produces exposure conditions.

DiseaseSystem decides:

- susceptibility;
- transmission;
- incubation;
- infection;
- outbreak;
- symptoms.

Sanitation never stores a parallel “disease risk 0–100” as canonical state.

---

# 22. Old-Save Migration

Recommended default:

```text
old save
-> preserve shelter topology
-> preserve existing water/disease/needs
-> initialize waste prospectively
-> map any real pre-existing toilet/plumbing fixture if discoverable
-> otherwise provide compatibility sanitation service via migration policy
-> no retroactive waste
-> no first-load overflow
```

Do not invent a free E1-9 facility silently.

---

# 23. Migration Compatibility Capability

If old gameplay implicitly assumed functioning sanitation, a temporary compatibility provider can expose:

```text
basic human-waste service capacity
```

without pretending a newly constructed object exists.

It must be documented and either:

- remain as legacy abstract shelter plumbing; or
- retire only after the player can replace it safely with real infrastructure.

---

# 24. Waste Processing Operations

Every operation should answer:

- source stream;
- amount;
- facility;
- worker/job if required;
- power/fuel;
- accepted hazard tags;
- output streams/items;
- emissions/discharge;
- operation ID;
- start/completion time.

This makes conservation auditable.

---

# 25. Exploit Matrix

| Exploit/failure | Guard |
|---|---|
| Save/reload duplicates daily waste | Day/source generation ID |
| Cancel processing deletes waste | Reservation transaction |
| Process twice creates compost | Operation ID |
| Recycler mints water | E1-18 conservation |
| Incinerator deletes hazardous waste | Compatibility rules |
| Overflow infects twice | Exposure correlation ID |
| Cleanup deletes contained inventory waste | Separate bulk/cleaning state |
| Facility repair occurs in two systems | E1-17 only |
| Old save gets free infrastructure | Migration service shim |
| Washing consumes no water | Water transaction |
| Hygiene duplicated in Needs/Sanitation | Ownership ADR |
| Export deletes cargo | Canonical transfer |

---

# 26. Player-Burden Targets

Track:

- sanitation-related jobs/week;
- emergency cleanup incidents;
- overflow warnings;
- manual facility operations;
- washing actions if hygiene exists;
- percentage of routine sanitation handled by standing Duty policy;
- time spent in sanitation dashboard;
- maintenance overlap with E1-17;
- number of waste streams visible to player.

If the player is clicking “empty bin” repeatedly, redesign toward capacity, automation, and scheduled jobs.

---

# 27. Economy Targets

Sanitation should cost:

- labor;
- water for washing/cleaning;
- power/fuel for advanced processing;
- facility maintenance;
- consumables/PPE where relevant;
- construction investment.

Sanitation may recover:

- compost/fertilizer;
- recyclables/salvage;
- reusable water through E1-18.

Recovered value must remain lower than or proportionate to real inputs.

---

# 28. First Release Scope

Recommended:

1. human waste;
2. kitchen organic/general waste;
3. medical biohazard;
4. basic latrine/holding capability;
5. one compost/general-waste processing route;
6. hazardous containment;
7. one sewage overflow exposure;
8. derived sanitation dashboard.

Add recycler/incinerator/hygiene breadth after this passes.

---

# 29. Headless Selftest

1. load shelter/topology;
2. generate human waste from survivor-days;
3. generate kitchen waste from canonical event;
4. generate medical waste from treatment event;
5. route compatible waste;
6. reject incompatible processing;
7. fill human-waste capacity;
8. create one sewage/overflow exposure;
9. verify Disease receives exposure exactly once;
10. fail facility through E1-17;
11. verify throughput changes;
12. process waste transactionally;
13. save/reload;
14. assert no duplicated downstream state.

---

# 30. Performance Strategy

Avoid:

- per-frame survivor hygiene/waste ticks;
- one waste object per trivial trash event;
- daily scans of all rooms/facilities;
- unbounded sanitation-event history;
- repeated recomputation of full forecasts.

Prefer:

- aggregated bulk streams;
- population-day generation;
- source-event generation for episodic waste;
- due/dirty facility processing;
- event-driven overflow transitions;
- bounded history;
- derived UI summaries on demand.

---

# 31. Rollback Strategy

### Waste generation
Disable new generation while preserving existing streams for migration/debug.

### Hygiene
Disable independently.

### Advanced processors
Disable per facility definition.

### Recycler
Fall back to E1-18 without greywater intake.

### Incinerator
Disable independently.

### UI
Keep read-only basic waste/capacity view.

Never roll back by moving Disease, Water, Air, Power, or facility condition into sanitation state.

---

# 32. Follow-On Opportunities

## Sanitation specialist role
E1-20 specialization if distinctive enough.

## Waste trading/export
E1-19 route contracts.

## Outpost sanitation
E1-10 site capability.

## Disaster sewage crisis
Plan 158 integration.

## Legacy sanitation crises
E1-5 historical memory.

## Advanced wastewater treatment
E1-18/WaterTreatment extension.

---

# 33. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --sanitation-selftest
```

Suggested targeted suites:

- `SanitationAuthorityBoundaryTests`
- `WasteTaxonomyTests`
- `WasteQuantityTests`
- `WasteGenerationTests`
- `HumanWasteTests`
- `KitchenWasteIntegrationTests`
- `MedicalWasteIntegrationTests`
- `ChemicalWasteTests`
- `HazardousWasteTests`
- `SanitationFacilityCapabilityTests`
- `SanitationConstructionBoundaryTests`
- `SanitationMaintenanceBoundaryTests`
- `LatrineTests`
- `SepticTests`
- `CompostingTests`
- `WasteProcessorTests`
- `IncineratorTests`
- `HazardousContainmentTests`
- `GreywaterTests`
- `SanitationWaterRecyclerBoundaryTests`
- `WasteTransferTests`
- `SanitationCleaningTests`
- `SewageBackupTests`
- `SanitationDiseaseHandoffTests`
- `SanitationVentilationHandoffTests`
- `SanitationSumpHandoffTests`
- `HygieneOwnershipTests`
- `SanitationMigrationTests`
- `SanitationTimeSkipTests`
- `SanitationConservationTests`
- `SanitationPerformanceTests`

---

# 34. Completion Checklist

- [ ] Premise audit completed at current HEAD.
- [ ] Sanitation ownership ADR accepted.
- [ ] Waste taxonomy uses distinct handling/hazard semantics.
- [ ] Waste units are explicit.
- [ ] Derived rates are not persisted as truth.
- [ ] Routine generation uses no RNG.
- [ ] Human waste uses survivor presence.
- [ ] Kitchen waste comes from canonical events.
- [ ] Medical waste comes from completed medical actions.
- [ ] Hazardous/radiological waste remains segregated.
- [ ] Facility installation remains E1-9-owned.
- [ ] Facility condition remains E1-17-owned.
- [ ] Power remains PowerGrid-owned.
- [ ] Latrine/holding does not magically destroy waste.
- [ ] Composting has safe input rules and delayed output.
- [ ] Incineration has emissions/residue.
- [ ] Hazardous containment stores rather than destroys waste.
- [ ] Greywater routes through E1-18/WaterTreatment.
- [ ] Waste transfers/jobs are transactional.
- [ ] Sewage/overflow creates typed exposures.
- [ ] Disease owns outbreaks.
- [ ] Ventilation owns air quality.
- [ ] Sump owns flood state.
- [ ] Hygiene has exactly one owner or is omitted.
- [ ] Washing uses canonical water.
- [ ] Relations/social effects are contextual, not automatic stigma.
- [ ] No authoritative global sanitation score drives simulation.
- [ ] Waste processing obeys conservation.
- [ ] Greenhouse consumes real compost/fertilizer.
- [ ] Alerts are threshold/event based.
- [ ] Old saves get no retroactive waste or fabricated facility.
- [ ] Time skip is exact.
- [ ] Burden/economy/performance audits pass.
- [ ] 3–4 streams/facilities pass before full breadth.
- [ ] `E1_planintegration[23].md` is the next sequence filename.

---

# 35. Final Directive

Plan 201 should make sanitation a real survival logistics problem, not another master meter.

The player should understand that twelve survivors produce sewage, the kitchen produces organics, the infirmary
produces biohazard waste, the workshop may produce chemicals, and every stream needs somewhere to go. When a
holding tank fills, something must be pumped, transferred, processed, composted, contained, exported, or
cleaned. When a seal fails, the consequences should emerge through the same Disease, Water, Ventilation,
Radiation, Needs, and Maintenance systems already trusted elsewhere.

The architectural standard is:

**sanitation owns waste and service flow; canonical health, environment, infrastructure, and survivor systems
own the consequences.**

If `SanitationSystem` starts storing a universal health/morale score, duplicating facility condition,
purifying water, rolling disease outbreaks, setting air quality, changing crop yield directly, or inventing
retroactive waste on old saves, stop and restore the boundary.


<!-- Deliverable intentionally capped within the requested 50–90k character envelope. -->
