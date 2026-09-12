# ASHFALL Flagship Full Integration Plan
## Plans 138–141 — Low-Background Radiation Metrology, InSAR Ground-Deformation Intelligence, Advanced Seamless Tubing Manufacture & Run-Flat Vehicle Mobility

> **Status:** AUTHORIZED ROADMAP — not yet claimed. Implementation requires a
> batch in `INTEGRATION_PLANS.md` per governance. Wave A reconnaissance
> corrections recorded in `PLANS_138_141_WAVE_A_RECONNAISSANCE.md`.
>
> **Premise corrections (2026-09-12 reconnaissance):** the plan text below
> names some authorities that do not exist under those exact names. Where
> this document says `FoundryProductionSystem`, the live owner is the
> `SilentFoundrySystem` family (`Assets/Ashfall.Core/Foundry/`);
> `DeepExcavationSystem` is `ExcavationSystem` /
> `Excavation/ExcavationHazardSystem`; `ExpeditionVehicleLogistics` is
> `ExpeditionVehicleSystem` + `Expeditions/VehicleGarageSystem`;
> `TerrainTopologyCatalog` has no standalone file (topology lives inside
> `WastelandMapSystem`); `RefractoryCeramicsEngine` and
> `RubberVulcanizationEngine` have no live equivalent (Ceramics exists only
> as `Narrative/CeramicsKilnCatalog`). Implementation must extend the real
> owners named in the reconnaissance report.

**Project:** ASHFALL
**Plan class:** Major flagship full-integration roadmap
**Scope:** Plans 138, 139, 140, and 141
**Primary domains:** Radiation metrology, advanced metallurgy, terrain intelligence, excavation risk, industrial manufacturing, fluid-system reliability, expedition vehicle mobility, persistence, Godot presentation, content utilization, CI
**Priority:** P1/P2 late-game science and industrial resilience expansion
**Recommended implementation order:** **Shared reconnaissance → Plan 138 measurement authority → Plan 139 terrain intelligence → Plan 140 advanced manufacturing → Plan 141 vehicle mobility → unified 30–60 day campaign continuity pass**

---

# 0. Flagship Mission

Turn four high-end technical concepts into connected late-game systems that feel sophisticated without fragmenting ASHFALL's existing architecture.

The integrated player journey should become:

1. **Low-background metrology**
   - Rare low-background shielding materials improve the sensitivity of existing radiation analysis.
   - The player can discriminate very low contamination levels in food, water, salvage, or samples.
   - Shield quality, contamination, detector condition, calibration, and operator skill matter.
   - The system improves detection confidence; it does not magically guarantee perfectly contaminant-free food.

2. **Ground-deformation intelligence**
   - Repeat-pass remote sensing produces deformation-risk maps over known sectors.
   - The system detects subsidence, fault creep, and structural instability trends.
   - These observations improve travel routing, excavation planning, and seismic preparedness.
   - The system provides probability/risk intelligence rather than impossible guaranteed earthquake prediction.

3. **Advanced seamless tubing manufacture**
   - The shelter gains a high-tier industrial process for strong, high-integrity tubing and pressure-service components.
   - Production depends on suitable billets, dies/tooling, machine condition, energy, cooling, quality control, and operator skill.
   - Outputs feed existing geothermal, cryogenic, hydraulic, and high-pressure utility systems through generic component-quality interfaces.
   - The process remains an abstract in-game production chain rather than a real-world heavy press operating guide.

4. **Run-flat expedition mobility**
   - Wheeled vehicles can receive durable mobility upgrades that greatly reduce puncture failures.
   - The upgrade trades puncture resilience for increased mass, heat, rolling resistance, maintenance, and handling penalties.
   - Bullet or severe impact damage is survivable in some cases but never grants invulnerability or guaranteed full-speed escape.
   - The system extends the canonical vehicle-condition and expedition authorities.

The plans must reinforce one another:

- improved radiation metrology can certify industrial salvage, food, water, and rare materials;
- deformation maps can warn of unstable tunnels, sinkholes, and damaged transport corridors;
- advanced tubing can improve the reliability ceiling of geothermal, cryogenic, hydraulic, and deep-excavation systems;
- run-flat mobility can improve expedition survivability while increasing fuel and thermal burden;
- all four systems must preserve deterministic outcomes across save/load and long-campaign replay.

---

# 1. Global Architecture Guardrails

## 1.1 One authority per concern

Before creating any named system, inspect the repository for existing ownership.

Do not create duplicate authorities for:

- radiation dose/contamination;
- sample analysis;
- item provenance/quality;
- metallurgy/foundry production;
- terrain topology;
- excavation instability;
- seismic risk;
- vehicle condition;
- tire/wheel damage;
- expedition movement;
- crafting;
- inventory;
- save/load;
- deterministic RNG.

If an existing authority owns the concern, extend or adapt it.

## 1.2 Core owns simulation

Core owns:

- detector background/noise state;
- assay confidence;
- material contamination/quality state;
- deformation-map processing;
- subsidence/fault-risk projection;
- production batch quality;
- tube defect state;
- run-flat wheel condition;
- vehicle movement penalties/benefits.

Host/Godot may:

- issue commands;
- bind snapshots;
- display spectra/maps/gauges;
- animate machines/vehicles;
- show warnings and confidence.

Godot must not:

- decide radiation cleanliness;
- infer earthquake occurrence;
- calculate tubing strength;
- decide puncture immunity;
- mutate vehicle condition.

## 1.3 Catalog-driven content

New authored catalogs:

- `low_background_lead_catalog.json`
- `insar_geodesy_catalog.json`
- `hydraulic_extrusion_catalog.json`
- `runflat_tire_catalog.json`

Each must:

- include `schema_version`;
- use canonical IDs;
- validate all cross-references;
- reject duplicates;
- use stable ordering;
- define bounded numeric ranges;
- participate in data-integrity/content-utilization tests.

## 1.4 Gameplay abstraction boundary

Plans 138 and 140 involve hazardous heavy-industry operations and high-energy equipment.

Plan 141 involves industrial polymer/vehicle service processes.

Implementation should model:

- input classes;
- machine profiles;
- quality;
- energy use;
- wear;
- defects;
- maintenance;
- operator skill;
- hazard states.

Do not encode real-world operational instructions, material recipes, pressure/temperature procedures, equipment setup sequences, or construction guidance that could function as a real process manual.

## 1.5 Scientific realism corrections

The source concepts should be adjusted where necessary:

### Low-background lead

Ancient lead is useful because much of its original Pb-210 activity has decayed over many half-lives, not because it literally reaches zero activity.

Use:
- `residual_activity`
- `background_class`
- `assay_quality`

rather than "zero radioactivity."

### Food/water screening

A detector cannot guarantee "100% contaminant-free" food or water.

Use:
- detection limit;
- confidence;
- assay result;
- below-detection-threshold classification.

### InSAR

Repeat-pass interferometry can reveal deformation trends.

It cannot reliably predict a specific earthquake 48 hours in advance.

Use:
- deformation risk;
- accelerating creep;
- anomaly warning;
- uncertainty.

### Seamless tubing

High-quality tubing can reduce failure probability.

It does not make fluid systems 100% reliable.

### Run-flat tires

Run-flat/solid-fill systems can strongly reduce puncture failure.

They do not make a wheel immune to all spikes, bullets, overheating, bead/rim failure, or structural destruction.

These corrections should be reflected in mechanics, UI wording, tests, and documentation.

---

# 2. Shared Repository Reconnaissance Gate

Before implementation, produce an authority map.

## Plan 138

Inspect:

- detector systems;
- contamination analysis;
- water/food testing;
- radiation shielding;
- item provenance;
- laboratory rooms.

## Plan 139

Inspect:

- map topology;
- terrain sectors;
- excavation geometry;
- geological fault state;
- seismic system;
- airborne/platform systems;
- survey/intelligence state;
- weather/cloud occlusion.

## Plan 140

Inspect:

- Foundry production;
- melting/refining;
- industrial workstations;
- item quality;
- fixture condition;
- geothermal;
- cryogenic air separation;
- hydraulic systems;
- water/cooling.

## Plan 141

Inspect:

- vehicle condition;
- wheel/tire state;
- expedition hazards;
- fuel use;
- rolling resistance if present;
- heat/weather;
- road surface;
- combat damage;
- maintenance.

Produce:

| Concern | Current Authority | Required Change |
|---|---|---|
| Radiation truth | RadiationSystem | reuse |
| Sample assay | detector/lab authority | extend |
| Material provenance | item/loot provenance | extend if needed |
| Terrain geometry | WastelandMap | reuse |
| Deformation intelligence | survey/intelligence | add projection |
| Seismic truth | seismic/geology | consume, not replace |
| Industrial production | foundry | extend |
| Tubing quality | item quality/industrial QA | add profile |
| Utility reliability | each utility owner | consume quality modifier |
| Vehicle condition | vehicle authority | extend |
| Tire/wheel state | vehicle component state | add component |
| Expedition movement | expedition | consume vehicle state |
| Save | current aggregate stores | extend, not mirror |

---

# 3. Shared Simulation Ordering

Unless existing orchestration supersedes this:

1. Advance world weather and geology.
2. Advance excavation/seismic truth.
3. Process scheduled survey/remote-sensing observations.
4. Update deformation intelligence products.
5. Advance radiation/environment contamination.
6. Advance sample assay/calibration state.
7. Advance industrial production batches.
8. Apply machine/tool wear and QA results.
9. Advance vehicle component heat/wear.
10. Advance expeditions using current vehicle state.
11. Resolve road hazards/combat damage.
12. Commit inventory/production outputs.
13. Emit presentation snapshots/events.
14. Persist through canonical save boundary.

No system should recursively tick another authority.

---

# 4. Plan 138 — Subterranean Low-Background Lead Smelting & Radiation Metrology
## Ultra-Low Background Assay Capability

## Mission

Give the shelter a high-tier radiation metrology capability where rare low-background shielding, detector quality, calibration, and sample handling lower the practical detection threshold of contamination analysis.

The gameplay payoff is **better information and earlier detection**, not absolute immunity from contamination.

## 4.1 Radiation architecture review

Inspect detector systems, contamination analysis, water/food testing, radiation shielding, item provenance, laboratory rooms. If an assay system already exists, extend it. Do not create a second radiation truth model.

## 4.2 `low_background_lead_catalog.json`

Create `schema_version: 1`. Suggested fields: `material_profile_id`, `source_tag`, `background_activity_class`, `residual_activity`, `shielding_factor`, `cross_contamination_sensitivity`, `processing_loss`, `detector_compatibility_tags`, `required_quality_control`, `tags`. Avoid storing real-world operating procedures or exact industrial furnace parameters.

## 4.3 Material provenance

Low-background shielding depends on provenance: pre-industrial salvage; submerged historical ballast; certified shield stock; ordinary modern scrap. Use item metadata or quality/provenance. Do not create dozens of duplicate lead item IDs if provenance can be represented safely through item instances.

## 4.4 Residual activity model

`effective_detector_background = detector_native_background + shield_material_background + environmental_background - shielding_reduction`, clamped non-negative. Do not claim activity equals zero.

## 4.5 Material aging/decay

Campaign duration is too short for meaningful Pb-210 decay; use provenance-authored residual activity rather than day-by-day decay as a gameplay loop. Unit tests may verify generic decay math as a utility.

## 4.6 Shield manufacturing

Use the existing metallurgy/foundry authority. Inputs: low-background feedstock, mold/brick production capacity, clean handling/QA, energy, labor. Output: shield bricks/modules with provenance/quality. Keep heavy-industry process details abstract.

## 4.7 Cross-contamination

If incompatible scrap enters a batch: background class worsens; batch may become ordinary shielding; material is not deleted. Transactional batch state — no catastrophic batch disappearance.

## 4.8 Detector cave/installations

Detector installation combines shield modules, probe/detector, electronics, calibration source if existing, lab room. It modifies background count rate, minimum detectable activity, confidence/time for assay. It does not reduce environmental radiation.

## 4.9 Assay model

Inputs: sample contamination truth; detector sensitivity; background; assay duration; sample prep quality; operator skill. Outputs: detected/not detected; confidence; estimated band; detection limit. No perfect exact contamination values unless the detector model supports them.

## 4.10 Water treatment integration

WaterTreatmentSystem may request assay. States: safe under current threshold; contamination detected; indeterminate; resample required. The detector does not purify water.

## 4.11 Kitchen/food integration

KitchenNutritionSystem may use assay results to tag ingredients as screened, block contaminated use, reduce uncertainty. Never guarantee food is contamination-free.

## 4.12 Skill integration

Verify canonical skill IDs (radiological analysis, salvage identification, laboratory QA). Skill may improve provenance identification, assay confidence, sample prep quality, cross-contamination risk. No flawless guaranteed outcome unless a capped late-game perk explicitly defines it.

## 4.13 Items/recipes

Verify/create canonical items: low-background lead ingot/stock; shield brick/module; scintillation probe/detector component. Use generic detector compatibility tags.

## 4.14 Persistence

Persist shield installation, material batch provenance/activity class, detector calibration state, bounded assay history if gameplay-visible. Do not persist enormous raw spectrum histories unless needed.

## 4.15 UI

`LowBackgroundLeadPanel.cs` or equivalent: detector baseline, shield quality, detection threshold, assay confidence, calibration state, sample result, spectrum visualization. Spectral display is presentation; Core owns analysis.

## 4.16 Tests

Required: provenance classification; ordinary vs low-background shielding; background reduction; cross-contamination downgrade; assay detection threshold; indeterminate case; operator skill modifier; water assay handoff; food assay handoff; no purification side effect; save/load; old-save baseline; deterministic assay replay.

## 4.17 Definition of Done — Plan 138

- RadiationSystem remains truth authority.
- Low-background materials improve assay sensitivity.
- Material provenance matters.
- Cross-contamination is persistent.
- Detector installation improves detection, not purification.
- Food/water receive confidence-based screening.
- Save/load exact.
- UI is presentation-only.
- Scientific claims avoid "zero radioactivity" and "100% contaminant-free."

---

# 5. Plan 139 — Wasteland InSAR Ground-Deformation Mapping
## Deformation Intelligence, Subsidence & Terrain-Risk Projection

## Mission

Add repeat-pass deformation mapping as an intelligence layer over world terrain and excavation risk. The system detects and visualizes **deformation trends**, not omniscient earthquake prediction.

## 5.1 World/geology architecture review

Preferred architecture: `world/geology truth` → `survey observations` → `InSAR intelligence projection` → UI/player decisions.

## 5.2 `insar_geodesy_catalog.json`

Create `schema_version: 1`. Suggested fields: `sensor_profile_id`, `coverage_width`, `nominal_resolution`, `repeat_pass_interval`, `coherence_decay_rate`, `weather_decorrelation_modifier`, `vegetation/terrain_decorrelation_tags`, `minimum_detectable_deformation`, `processing_skill_modifier`, `tags`. Not a real radar engineering spec.

## 5.3 Survey pass state

Track survey ID, platform/sensor, sector coverage, observation day, observation quality, reference geometry ID, weather quality, processing state. A deformation map requires at least two compatible observations.

## 5.4 Interferometric state

Derived intelligence: coherence, relative line-of-sight displacement, confidence, trend velocity, anomaly classification. Do not persist raw synthetic radar phase histories unless required.

## 5.5 Repeat-pass processing

Stable sector ordering, stable reference, deterministic noise/uncertainty model, explicit missing-data behavior. No UI-side phase calculations.

## 5.6 Subsidence detection

InSAR projection can flag: stable; slow subsidence; accelerating subsidence; abrupt deformation; low-confidence/unknown. Informs excavation reinforcement, route planning, sinkhole risk.

## 5.7 Excavation integration

Accelerating subsidence over underground works raises an engineering warning and exposes risk to the excavation planner. InSAR never directly collapses a tunnel — collapse remains excavation/geology-owned.

## 5.8 Seismic/fault integration

InSAR may observe fault creep to improve fault-risk classification, anomaly confidence, and lead-time awareness. No deterministic "earthquake in 48 hours." If the seismic system has scheduled/seeded fault events, InSAR may improve recognition probability without revealing exact hidden timing.

## 5.9 Travel corridor intelligence

Map overlay classifies surveyed corridors: stable; subsiding; fractured; unknown; low coherence. Route planner incorporates known hazard penalties only when the player has the intelligence. The sensor never changes terrain truth.

## 5.10 Atmospheric decorrelation

Weather (precipitation, ash, atmospheric variability, surface change) reduces coherence, widens uncertainty, or makes a pass unusable. Use WeatherSystem state.

## 5.11 Processing skill

Verify canonical skills (radar analysis, geodesy, remote sensing). Skill improves unwrapping success abstraction, bad-data rejection, confidence, processing time. Never guarantees zero errors.

## 5.12 Sensor/platform items

Verify/create canonical items: airborne radar pod; calibration reflector; terrain model/data disk. Use a generic survey platform if aviation exists. Do not create a new aircraft runtime.

## 5.13 Persistence

Persist survey passes, processed sector deformation summaries, confidence, bounded trend history, calibration state — in world/intelligence save. Do not duplicate geological truth.

## 5.14 UI

`InSarMappingPanel.cs` or equivalent: deformation heatmap, confidence/coherence, trend arrows, subsidence warnings, surveyed vs unknown sectors, line-of-sight profile. Clear uncertainty language.

## 5.15 Tests

Required: compatible pass pairing; incompatible pass rejection; stable terrain result; subsidence detection; accelerating trend; low coherence; weather degradation; skill improvement; excavation-warning handoff; travel-risk projection; no direct geology mutation; save/load; split-run same map; deterministic replay.

## 5.16 Definition of Done — Plan 139

- InSAR is an intelligence layer, not terrain truth.
- Repeat-pass data required.
- Deformation trends deterministic.
- Weather affects confidence.
- Excavation and route planning consume intelligence.
- Seismic warnings express risk, not guaranteed prediction.
- Save/load preserves survey history.
- UI presents uncertainty honestly.

---

# 6. Plan 140 — Advanced Hydraulic Extrusion & Seamless Tubing Manufacture
## High-Integrity Industrial Component Production

## Mission

Add an advanced industrial manufacturing chain for high-integrity seamless tubing and pressure-service components that can improve downstream system reliability without creating impossible 100% failure immunity.

## 6.1 Industrial authority review

Prefer extending the live foundry authority (`SilentFoundrySystem` family). Do not create an isolated inventory/manufacturing economy.

## 6.2 `hydraulic_extrusion_catalog.json`

Create `schema_version: 1`. Suggested fields: `product_profile_id`, `billet_material_tag`, `machine_class`, `die_profile_id`, `energy_cost`, `cooling_requirement`, `tool_wear`, `base_quality`, `defect_risk_bp`, `operator_skill_tags`, `result_item_id`, `tags`. Keep exact industrial force/temperature recipes out of gameplay data.

## 6.3 Machine state

Track machine ID, tooling profile, die condition, liner/tool condition, alignment quality, maintenance state, active batch. No real machine setup parameters exposed.

## 6.4 Batch input

Inputs: refined billet; tooling/die; energy; coolant/water; labor; optional finishing. All from canonical inventory/resources.

## 6.5 Production phases

Abstract phases: billet conditioning; forming; piercing/hollowing; sizing; finishing; QA. No real step-by-step process instructions.

## 6.6 Defect model

Defects: eccentric wall; surface flaw; dimensional out-of-spec; contamination; tooling mark. Risk inputs: billet quality; die/tool condition; machine condition; operator skill; power stability. Deterministic RNG for probabilistic outcomes.

## 6.7 Quality grade

Output quality class: rejected/scrap; utility grade; high-pressure grade; premium grade. Downstream systems consume quality class. No magic "perfect tube."

## 6.8 Finishing

Generic finishing step: extra time, energy, tooling, improved dimensional quality, work-hardening/condition tradeoff. No operational procedure.

## 6.9 Geothermal integration

Geothermal system may query tubing quality for lower leak probability, higher service pressure limit within authored bounds, lower maintenance rate. Never 100% reliability.

## 6.10 Cryogenic integration

High-integrity tubing may lower leak probability, improve maintenance interval, improve process stability. Bounded.

## 6.11 Hydraulic/excavation integration

High-quality tubing can improve hydraulic shoring, excavation equipment, fluid lines via generic component compatibility tags. No Plan 140 checks in every consumer.

## 6.12 Material compatibility

Verify canonical materials (titanium alloy, stainless steel, high-grade steel). If the resource economy cannot support true titanium production, keep the late-game item rare or salvage-dependent.

## 6.13 Tooling items

Verify/create abstract items: extrusion die; generic high-temp forming consumable; mandrel/tooling module. No real material formulations.

## 6.14 Machine wear

Production submits wear to the fixture/equipment condition authority. Tool/die wear affects defect risk, dimensions, downtime. No parallel wear model.

## 6.15 Persistence

Persist active batch, tooling condition if system-owned, machine operating hours if gameplay-relevant, produced but unclaimed output if canonical production does so. Inventory owns stock after claim.

## 6.16 UI

`HydraulicExtrusionPanel.cs` or equivalent: material, product profile, machine condition, die/tool condition, batch phase, quality forecast band, defect warning, output class. Not a real heavy-press operating interface.

## 6.17 Tests

Required: valid material; invalid material; energy/water consumption; tooling wear; defect probability; skill modifier; out-of-spec result; high-quality result; rejected batch; generic downstream compatibility; geothermal reliability modifier; cryogenic reliability modifier; no 100% reliability; save/load; old-save baseline; deterministic replay.

## 6.18 Definition of Done — Plan 140

- Foundry/industrial production remains canonical.
- Advanced tubing is a real late-game component.
- Production consumes real materials/energy/cooling.
- Tooling and machine condition matter.
- Quality/defects deterministic.
- Downstream utilities consume generic quality tags.
- Reliability improves but never becomes absolute.
- Save/load exact.
- UI presentation-only.
- No actionable heavy-industrial operating procedure.

---

# 7. Plan 141 — Expedition Run-Flat Tire & Wheel Mobility Upgrade
## Puncture Resilience, Heat, Mass & Vehicle Reliability

## Mission

Add a persistent tire/wheel upgrade that trades puncture resilience for heavier rotating mass, greater rolling resistance, heat accumulation, maintenance burden, and handling penalties. Improves survivability without invulnerable wheels.

## 7.1 Vehicle architecture review

Preferred architecture: `VehicleState` → component slots → wheel/tire component condition/profile. No second vehicle damage system.

## 7.2 `runflat_tire_catalog.json`

Create `schema_version: 1`. Suggested abstract fields: `runflat_profile_id`, `compatible_vehicle_tags`, `puncture_resistance`, `sidewall_damage_resistance`, `rolling_resistance_modifier`, `unsprung_mass_modifier`, `heat_generation_rate`, `safe_speed_profile`, `terrain_modifier`, `repairability`, `required_item_ids`, `tags`. No real polyurethane ratios, injection instructions, torque specs, or construction procedures.

## 7.3 Wheel state

Track per wheel set or axle group: component ID, run-flat profile, integrity, puncture count, heat, imbalance, rim/bead state, wear. Use current vehicle granularity; simplified axle/wheel subsystem if the project tracks vehicle-wide condition only.

## 7.4 Upgrade installation

Requires compatible vehicle, upgrade kit, workshop/garage, labor/skill, downtime. Transactional item consumption. No instant field installation unless authored.

## 7.5 Puncture resistance

Road hazards (glass, wire, scrap, spikes, rubble) submit tire hazard events. Run-flat reduces probability/severity — not to zero unless a narrow hazard class is explicitly ignored.

## 7.6 Severe damage

Major hazards can still cause carcass damage, rim damage, overheating, chunk loss, wheel detachment, suspension damage. Combat damage may be survivable but reduces integrity. No guaranteed full-speed escape after a hit.

## 7.7 Rolling resistance

May increase fuel consumption, crew fatigue/noise, acceleration penalty. Uses the expedition vehicle performance authority. No free resilience.

## 7.8 Heat buildup

Heat inputs: speed, ambient temperature, load, road surface, rolling resistance, condition. Cooling: lower speed, ambient conditions, stops. Excess heat: accelerated wear, speed limit, degradation. Deterministic temperature model.

## 7.9 Speed policy

Core determines the recommended/safe operating band from the profile. Player may exceed it if game design permits, accepting higher wear/failure risk.

## 7.10 Rim retention

Bead/rim retention as an abstract wheel-system property reducing unseating risk and off-road failure. No real clamping/torque procedures.

## 7.11 Skill integration

Verify canonical mechanic/tire/expedition skills. Skill improves installation quality, wheel balance, diagnosis, maintenance, heat management. Do not invent trait IDs the catalog lacks.

## 7.12 Perimeter-defense interaction

Run-flat tires can reduce puncture severity from certain surface obstacles but the vehicle never ignores perimeter defenses. Heavy wire/obstacles may still cause axle entanglement, suspension damage, speed loss.

## 7.13 Rail/draisine interaction

Apply only to wheeled road vehicles that actually use tires. No pneumatic/run-flat state on steel-wheel rail vehicles unless a specific design supports rubber-tired auxiliary wheels.

## 7.14 Expedition integration

Vehicle movement consumes current tire/wheel state for speed, fuel, hazard breakdown risk, route suitability. No separate run-flat movement loop.

## 7.15 Maintenance

Maintenance may require wheel components, balancing materials, repair kit, workshop. Profile-specific repairability; some damage not field-repairable.

## 7.16 Persistence

Use vehicle/expedition save. Persist run-flat profile, wheel integrity, heat if stateful, puncture/damage state, imbalance, upgrade installation. No duplicate overall vehicle condition.

## 7.17 UI

`RunFlatTirePanel.cs` or equivalent: installed profile, integrity, heat, imbalance, rolling resistance/fuel penalty, puncture incidents, maintenance state, safe-speed advisory. No real installation procedures.

## 7.18 Tests

Required: compatible installation; incompatible vehicle rejection; resource consumption; puncture hazard reduction; non-zero severe hazard failure; bullet/fragment damage survival cases; rim damage; rolling resistance fuel penalty; heat accumulation; hot-weather modifier; cooldown; overloaded vehicle; skill installation quality; maintenance; save/load; old-save baseline; deterministic replay.

## 7.19 Definition of Done — Plan 141

- Run-flat is a vehicle-component upgrade, not a new vehicle runtime.
- Puncture risk is strongly reduced, not universally eliminated.
- Severe damage remains possible.
- Rolling resistance/fuel tradeoff is real.
- Heat is deterministic.
- Skills/maintenance use canonical systems.
- Save/load exact.
- UI presentation-only.
- No real-world chemical/installation procedure is encoded.

---

# 8. Cross-Plan Integration Contracts

- **8.1 Plan 138 → 140:** assay may certify salvage/material batches (provenance, contamination, suitability); never alters metallurgy directly.
- **8.2 Plan 139 → Excavation/Seismic:** intelligence exposes warnings; excavation/seismic remain truth authorities.
- **8.3 Plan 139 → Expedition:** known unstable terrain may raise route cost/risk; only known intelligence affects route planning UI; terrain hazard stays world-owned.
- **8.4 Plan 140 → high-pressure utilities:** downstream systems consume generic tubing quality; no direct state mutation.
- **8.5 Plan 140 → 141:** advanced manufacturing may produce wheel/rim components only if existing systems need them; no forced coupling.
- **8.6 Plan 141 → Expedition:** mobility, fuel use, breakdown risk consume wheel state; expedition owns travel.
- **8.7 Plan 138 → Food/Water:** assay results improve safety decisions; food/water systems remain contamination/nutrition authorities.

---

# 9. Shared Persistence Ownership

| State | Preferred Owner |
|---|---|
| Low-background material provenance | item/material state |
| Shield installation | lab/shelter |
| Detector calibration | radiation/lab |
| Geological truth | world/seismic |
| InSAR observations | intelligence/world |
| Tubing production batch | foundry/production |
| Finished tubing | inventory |
| Machine/tool condition | equipment/fixture |
| Run-flat upgrade | vehicle component |
| Wheel/tire damage | vehicle condition |
| Expedition movement | expedition |

Do not mirror state.

---

# 10. Old-Save Migration Matrix

| Feature | Old Save Baseline |
|---|---|
| Low-background shield | absent unless existing lab upgrade implies it |
| Detector calibration | preserve current detector state; default ordinary baseline |
| Material provenance | existing lead remains ordinary unless explicitly identifiable |
| InSAR observations | none |
| World geology | unchanged |
| Extrusion machine | absent unless existing industrial construction implies it |
| Tubing inventory | none fabricated |
| Existing utility reliability | unchanged |
| Run-flat wheels | standard tire profile |
| Vehicle condition | preserved exactly |

Migration must not turn ordinary lead into rare stock, reveal free map intelligence, fabricate titanium tubing, auto-upgrade vehicles, or erase vehicle damage.

---

# 11. Determinism Contract

- **Plan 138:** same sample truth/detector/shield/calibration/operator/seed → same assay result/confidence.
- **Plan 139:** same world truth/survey passes/weather/processing state → same deformation map.
- **Plan 140:** same inputs/machine/tooling condition/operator/seed → same output quality/defects.
- **Plan 141:** same vehicle/wheel state/route hazards/weather/seed → same tire damage/heat/performance.

---

# 12–13. Unified 30-Day Deterministic Replay & Save/Load Split

Fixed-seed 30-day replay twice with all four systems active must match all authoritative daily captures. Compare 30 continuous days against 14 days → save/load → 16 days with identical commands: same assay confidence, survey intelligence, tubing quality, defect outcome, run-flat heat/damage, expedition arrival day. No rerolls.

---

# 14. Major Campaign Acceptance Scenario

Days 1–5 material discovery and provenance assay; days 6–10 detector upgrade and food/water screening; days 11–15 repeat-pass terrain survey and route risk; days 16–20 tubing manufacture with tooling/skill effects; days 21–24 utility tubing installation; days 25–30 run-flat installation and puncture-heavy expedition with heat burden. Assertions per phase in the flagship brief; every phase forbids absolute guarantees (no zero-activity, no guaranteed-clean, no quake prediction, no 100% reliability, no invulnerability).

---

# 15–18. Failure-State Matrices

Per-plan failure matrices (138: ordinary lead, cross-contamination, uncalibrated detector, below-limit sample, no shield, save/load; 139: single pass, low coherence, weather, insufficient skill, rapid terrain change, save/load; 140: invalid billet, worn tooling, missing power/cooling, poor alignment, failed QA, storage full, save/load; 141: incompatible vehicle, common vs severe hazards, heat, rim failure, save/load) — see flagship brief for the required behaviors; all encoded as typed failures in §20.

---

# 19. UI/UX Acceptance Standard

Every panel communicates **state → confidence/quality → blocker → consequence**. No raw IDs. No absolute "guaranteed safe/100% reliable/immune" language.

---

# 20. Typed Failure Contracts

- **Plan 138:** `MaterialNotSuitable`, `DetectorUnavailable`, `DetectorUncalibrated`, `SampleInvalid`, `AssayBelowDetectionLimit`, `ShieldContaminated`
- **Plan 139:** `InsufficientSurveyPasses`, `PassGeometryIncompatible`, `LowCoherence`, `SectorNotSurveyed`, `ProcessingDataMissing`
- **Plan 140:** `MaterialIncompatible`, `MachineUnavailable`, `ToolingUnavailable`, `PowerUnavailable`, `CoolingUnavailable`, `QualityControlFailed`
- **Plan 141:** `VehicleIncompatible`, `UpgradePartsMissing`, `WorkshopUnavailable`, `WheelOverheated`, `WheelDamaged`, `MaintenanceRequired`

Godot formats these.

---

# 21–24. Data Integrity, Content Utilization, Test Organization & Pyramid

Catalogs validate cross-references with per-row failure output (catalog, owner ID, field, invalid value). Content-utilization gates must prove runtime consumption (shield quality changes detector background; coherence threshold changes map availability; defect profile changes output distribution; heat/rolling resistance changes expedition behavior). Test files: `LowBackgroundLeadEngineTests.cs`, `LowBackgroundMetrologyIntegrationTests.cs`, `InSarInterferometryEngineTests.cs`, `InSarWorldIntegrationTests.cs`, `HydraulicExtrusionEngineTests.cs`, `AdvancedTubingIntegrationTests.cs`, `RunFlatTireEngineTests.cs`, `RunFlatVehicleIntegrationTests.cs`, `Plans138To141CampaignIntegrationTests.cs` — prefer extending canonical suites. Pyramid: pure unit → Core integration → save/load split → host/UI → CI/system.

---

# 25. Recommended Implementation Waves

- **Wave A — shared reconnaissance:** map authorities, canonical IDs, save ownership, numeric precision, existing abstractions. Exit: no duplicate-authority risk.
- **Wave B — Plan 138.** Exit: low-background metrology playable.
- **Wave C — Plan 139.** Exit: deformation intelligence playable.
- **Wave D — Plan 140.** Exit: advanced tubing production playable.
- **Wave E — Plan 141.** Exit: run-flat mobility playable.
- **Wave F — unified campaign:** 30–60 days across all four systems with save/load split.

---

# 26. CI Gate Matrix

Use current repository commands (`scripts/run_test.sh` for focused xUnit; `dotnet build Ashfall.csproj`; `godot --headless --path . -- --data-integrity-selftest` / `--content-utilization-selftest` / bridge, scene-binding, and campaign selftests where canonical; `python3 scripts/ci/scene-lint.py`; `python3 scripts/ci/run-gates.py --tier fast`). Closure requires zero regressions, zero new integrity findings, deterministic replay green, save compatibility green, UI/scene gates green.

---

# 27. High-Risk Engineering Traps

A: detector becomes purifier. B: ancient lead as literally zero activity. C: InSAR predicts exact earthquakes. D: sensor mutates terrain. E: industrial process becomes a real operating manual. F: 100% utility reliability. G: run-flat invulnerability. H: steel-wheel rail vehicles receive tire upgrades. I: UI performs analysis. J: migration creates free late-game technology.

---

# 28. Data Ownership Summary

Radiation contamination truth → RadiationSystem; detector assay/calibration → radiation/lab; material provenance → item/material state; geological truth → world/seismic; survey observations → intelligence/world; deformation display → projection/UI; industrial batch → foundry/production; finished tubing → inventory; utility reliability → downstream utility; vehicle condition → vehicle authority; wheel component → vehicle component; expedition movement → expedition; Godot spectra/maps/gauges → presentation.

---

# 29. Documentation Deliverables

- `docs/shelter/PLAN_138_LOW_BACKGROUND_LEAD_CLOSEOUT.md`
- `docs/world/PLAN_139_INSAR_INTERFEROMETRY_CLOSEOUT.md`
- `docs/shelter/PLAN_140_HYDRAULIC_EXTRUSION_CLOSEOUT.md`
- `docs/expeditions/PLAN_141_RUNFLAT_TIRE_CLOSEOUT.md`
- catalog schema notes; save/migration matrix; architecture/test map updates if gate-cited files change.

Documentation must capture corrected claims: low-background ≠ zero activity; below detection limit ≠ guaranteed uncontaminated; InSAR anomaly ≠ deterministic quake prediction; advanced tubing ≠ 100% reliability; run-flat ≠ universal puncture/ballistic immunity.

---

# 30–34. Definitions of Done & Final Acceptance

Per-plan checklists (§30–33) and the global standard (§34): one connected late-game campaign flow — rare low-background material → shield manufacture → improved assay → safer decisions; repeat-pass survey → deformation intelligence → excavation/travel warning; advanced production → quality tubing → lower utility failure risk; wheel upgrade → reduced puncture downtime → higher fuel/heat burden → improved but not invulnerable mobility. All interactions use existing state authorities.

---

# 35. Final Closure Standard

**Core owns simulation. Existing authorities own their state. Cross-system effects travel through typed APIs/events. Host projects. Godot presents. Saves preserve. Tests prove.**

A 30–60 day deterministic replay must produce identical detector baseline, assay confidence/results, shield state, survey observations, deformation intelligence, industrial batch progress, tubing quality/defects, tooling condition, run-flat integrity, tire/wheel heat, fuel penalty, and expedition progress for identical seed, state, and player commands — even across save/load splits.
