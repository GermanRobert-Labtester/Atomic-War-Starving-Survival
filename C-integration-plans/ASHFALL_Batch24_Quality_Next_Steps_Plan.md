# ASHFALL: Batch 24 Integration Plan

**Steps:** 369–384<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 24 is a content batch, not Expansion 24. Preserve the canonical
12-entry `ExpansionSuite`. Build on the typed Core systems and validated data
contracts established by prior batches.

Current authorities to reuse:

- `BrineWaterSystem` and treaty IDs in `foundry_accords.json` for geothermal
  brine; there is no active `District8Accords` class.
- `SilentFoundrySystem`, the Batch 17 rolling/tooling boundary, and
  `ResearchSystem` for industrial work orders and machine quality.
- `MedicalHostSession`, `RespiratoryDegenerationSystem`,
  `CombatTraumaSystem`, and `NeedsSystem`; there is no general `MedicalSystem`
  to duplicate.
- `WeatherSystem`, `RadiationSystem`, `GreenhouseSystem`, `MarketSystem`,
  `RadioHostSession`, `LocationLayoutSystem`, `TacticalCombatSystem`,
  `JournalSystem`, and the shared power/hazard adapters.
- `GenerationalSuccessionEngine` and `EpilogueMatrixRuntime` through typed
  facades, plus the structured campaign/memorial history ledger.

The roadmap’s absolute claims are made conditional. Cesium mud reduces
blowout risk but does not make drilling risk-free; a panacea cannot grant
unbounded immortality or disease immunity; acoustic and tactical systems do
not guarantee zero breaches; and geophysics discovers targets with
uncertainty rather than promising infinite power.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch24State` and `Batch24SaveStore` at
`user://saves/slot-N/batch24_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and strict
rejection of malformed current envelopes. `SaveLoadHostSession` discovers the
store through its `_save.json` convention and includes it in the slot
manifest.

New mutable Batch 24 state belongs here by default. If the Pilger mill,
meteor-radio, agriculture, medical, or monument systems extend an existing
authority, migrate that authority deliberately and delete duplicate batch
fields. UI nodes never write save or simulation state.

### 2.2 Shared contracts

Verify these prerequisites before feature slices:

- unit-safe inventory for drilling mud density, pipe/barrel dimensions, blood
  and dialysate, rainfall activity, food/medicine, aggregate, optics, and
  packet payloads;
- high-load `PowerGridSystem` with compressor, cryogenic, acoustic, vacuum,
  radio, and monument loads;
- weather/radiation events for rainfall, ash, lightning, noble gases,
  geomagnetic conditions, and exposure;
- geology contracts for boreholes, drilling pressure, MT/RMT/SIP targets,
  geophysical uncertainty, and excavation safety;
- medical diagnosis/treatment/effect ports for renal failure, infection,
  toxicity, and recovery;
- greenhouse nutrient, soil, vitamin, and topical-reagent contracts;
- radio packet, channel matrix, meteor-window, acknowledgement, and loss
  contracts;
- Century Seed, Epilogue, memorial, and campaign-history event contracts.

Extend `CatalogIntegrityValidator` for new materials, recipes, diseases,
locations, samples, signals, historic identities, endgame events, units, and
range/uniqueness constraints. All new JSON uses `snake_case` IDs and
`schema_version`.

## 3. Ordered vertical slices

1. **Foundation:** Batch 24 save, units, power/weather/radiation/geology
   contracts, and `--batch24-selftest` scaffolding.
2. **Deep drilling and industrial:** 369, 370, 377, and 380 through brine,
   geophysics, foundry, and optical-tool authorities.
3. **Clinical and agricultural:** 371, 373, 374, and 375 through medical,
   recipe, greenhouse, needs, and Century Seed time ports.
4. **Perimeter and tactical:** 376 and 379 through location, combat, power,
   weather, and hazard adapters.
5. **Geophysics and laboratory:** 372, 378, 381, and 382 after sampling,
   survey, spectrometry, and evidence contracts are stable.
6. **Communications and endgame:** 383 and 384 after meteor radio and
   Epilogue/history integration are complete.

## 4. Step-by-step integration contracts

### [369] Cesium Formate Drilling Mud — `CesiumFormateDrillingSystem`

- Extend brine chemistry and deep-drilling state with cesium precipitation,
  formate concentration, density `2.2 g/cm³`, viscosity, thermal stability,
  solids control, mud returns, borehole pressure, casing, and blowout
  preventer state.
- Use Batch 13/17 geophysics and `LocationLayoutSystem` for the 3,000 m well.
  A superheated steam reservoir must be an authored target with pressure,
  temperature, and well-control prerequisites.
- `CesiumFormatePanel` controls tanks, density, BOP, casing, and telemetry.
  Acceptance reaches 3,000 m and taps the authored reservoir only when the
  pressure window, mud properties, casing, and safety checks pass.

### [370] Pilger Seamless Pipe Mill — `PilgerMillSystem`

- Extend the Batch 17 rolling boundary with periodic Pilger rolls, ring dies,
  tapered mandrel, feed/return stroke, elongation, wall thickness, heat/cold
  schedule, surface defects, residual stress, and barrel/pipe inspection.
- Register casing pipe and artillery-barrel blanks with dimensions, grade,
  pressure/strength, and machining compatibility. “Flawless grain flow” is an
  inspected quality tier, not an automatic result.
- `PilgerMillPanel` renders crank arms, mandrel feed, elongation, and quality.
  Acceptance produces the authored six-meter barrel blank accepted by the
  heavy-artillery assembly path.

### [371] Hemodialysis — `HemodialysisSystem`

- Add patient renal state, crush injury, blood access, dialysate composition,
  hollow-fiber flow, ultrafiltration, potassium/urea/toxin clearance,
  anticoagulation, pressure, infection, and four-hour treatment progress.
- Use `MedicalHostSession`, `CombatTraumaSystem`, `NeedsSystem`, and radiation/
  toxin ports. The procedure must respect access, blood pressure, equipment,
  supplies, and contraindications.
- `DialysisModal` controls access, dialysate, flow, and run. Acceptance clears
  the authored urea/potassium crisis and saves the miner only when the four-hour
  treatment and recovery checks pass; long-term renal state remains recorded.

### [372] Noble Gas Freeze Traps — `NobleGasTrapSystem`

- Extend `WeatherSystem`/`RadiationSystem` sampling with liquid-nitrogen supply,
  charcoal trap temperature, xenon/krypton capture, gamma photopeak, plume
  concentration, wind transport, uncertainty, and source localization.
- Treat Xenon-133/Krypton-85 as catalogued radiological observations. A plume
  suggests a source but cannot pinpoint a silo without wind history,
  triangulation, and confidence thresholds.
- `NobleGasTrapPanel` controls cooling, trap exposure, counting, and map
  estimate. Acceptance detects the authored Xenon-133 plume and marks the
  secret reactor/silo as a probabilistic target requiring confirmation.

### [373] Century Solera Panacea — `CenturyElixirSystem`

- Track cask lineage, age, evaporation, contamination, decanting, vial
  provenance, recipe quality, distribution, and Year-100 gating through the
  existing Century Seed time authority.
- Use `RecipeCatalog` and `NeedsSystem` for a ceremonial/culinary item. Do not
  grant immortality, universal disease immunity, or unbounded max health; use a
  validated `century_vitality` effect with duration/scope/stacking rules.
- `CenturyElixirPanel` renders cask, vials, blessing, and recipient selection.
  Acceptance at Day 36,525 produces the authored panacea and applies the
  configured vitality trait to eligible living survivors through the normal
  needs/effect path.

### [374] Mountain Terrace Farming — `TerraceFarmingSystem`

- Use `DutyRosterSystem`, `CaregivingSystem`, `LocationLayoutSystem`, water,
  soil, and crop contracts for contour survey, retaining-wall tiers, stone
  supply, soil fill, erosion, drainage, labor safety, and acreage.
- Ten tiers and 50 hectares are an authored construction fixture. Crops still
  depend on weather, water, soil quality, pests, and labor; no permanent
  doubling is granted by a completion button.
- `TerraceFarmingPanel` controls survey, wall, fill, drainage, and crop links.
  Acceptance adds 50 hectares and the configured grain-yield modifier after
  the ten tiers pass stability and water checks.

### [375] Horseradish Towers and Topical Wash — `HorseradishSystem`

- Extend `GreenhouseSystem` with root growth, aeroponic water/nutrients,
  glucosinolate/sinigrin concentration, extraction, dilution, contamination,
  topical bottle, and shelf-life state.
- Connect through `RecipeCatalog`, `NeedsSystem`, `MedicalHostSession`, and
  `DiseaseSystem`. The wash is a configured topical aid, not a universal
  broad-spectrum antibiotic or substitute for validated treatment.
- `HorseradishTowerPanel` manages growth, grinding, titration, and bottles.
  Acceptance applies the configured +50% recovery modifier to the authored
  wound case only when concentration, sterility, and clinical suitability pass.

### [376] Razor-Wire Magnesium Trip Flares — `TripFlareWireSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for fence joints, flare brackets, sear tension,
  trip detection, illumination radius, burn/fire hazard, friendly movement,
  and panic/assault resolution.
- The flare reveals and disrupts a threat; it does not directly force a route,
  casualty count, or universal panic. Fire spread, ammunition, and maintenance
  remain simulation inputs.
- `TripFlareWirePanel` controls placement, arming, and sector reports.
  Acceptance illuminates the authored canyon assault and creates the configured
  route/retreat result through tactical combat.

### [377] Heavy Hydraulic Pull Broach — `HeavyBroachSystem`

- Extend `SilentFoundrySystem`/`ResearchSystem` tooling with 20-ton hydraulic
  load, two-meter broach bar, tooth sequence, stroke, lubrication, go/no-go
  gauges, tool wear, workholding, and part stress.
- Register internal involute spline hubs and recovery-truck winch components
  with fit, torque, and safety compatibility.
- `HeavyBroachPanel` renders tonnage, stroke, gauges, and yields. Acceptance
  produces an in-tolerance drive hub and completes the heavy recovery tow-truck
  winch assembly through the existing crafting/vehicle path.

### [378] Astrometric Glass Plates — `AstrometricPlateSystem`

- Track glass plate preparation, star-field exposure, comparator calibration,
  X/Y micrometer measurements, parallax, refraction, time, plate provenance,
  and geodetic confidence.
- Use `JournalSystem`, `ExpeditionSystem`, and `CraftingSystem`. Stellar
  parallax is an astronomical observation; geographical latitude requires an
  authored reference/observation calculation and cannot be inferred from a
  single plate without context.
- `AstrometricPlateModal` controls alignment and measurement. Acceptance adds
  the authored latitude/map refinement after plate quality, reference stars,
  and calculation confidence pass.

### [379] Armored Beetle Disruptors — `BeetleDisruptorSystem`

- Add foundation sectors, transducer frequency `12–40 Hz`, power, bedrock
  coupling, footing vibration, resonance damping, animal response,
  habituation, and undermining events.
- Connect to `PowerGridSystem`, `LocationLayoutSystem`, and the hazard adapter.
  “Zero breach” is a monitored threshold for the authored event, not universal
  foundation immunity.
- `BeetleDisruptorPanel` controls sectors and frequency. Acceptance drives the
  authored beetles away from Sub-Level 3 while keeping structural integrity,
  power, and alternate-breach risks active.

### [380] Vacuum Anti-Reflective Coater — `VacuumCoaterSystem`

- Track bell-jar vacuum, pump condition, base pressure `10^-6 torr`, MgF2
  evaporation, substrate cleanliness, quartz thickness, layer uniformity,
  adhesion, lens coverage, and rework.
- Integrate `SilentFoundrySystem`/`ResearchSystem` optics and equipment
  records. A 99% transmission result is an inspected coating tier with angle,
  wavelength, and surface limitations.
- `VacuumCoaterPanel` renders pressure, boat, thickness, and lens yields.
  Acceptance gives the authored sniper scope the configured +30% night
  visibility/critical-hit modifier only under compatible low-light conditions.

### [381] Mantle Magnetotellurics — `MantleMtSystem`

- Add months-long ultra-low-frequency station logs at `0.0001 Hz`, electric/
  magnetic tensor, calibration, cultural noise, remote reference, inversion,
  depth resolution, conductivity model, and plume confidence.
- Use `LocationLayoutSystem` and `ResearchSystem`; a mantle anomaly is a
  geophysical discovery, not an infinite energy source. Power generation still
  requires a drilled, engineered, permitted geothermal facility.
- `MantleMtGeophysicsPanel` renders log duration, tensor, sections, plate
  boundaries, and targets. Acceptance locates the authored mantle plume and
  unlocks a future geothermal prospect with uncertainty and construction cost.

### [382] Nomarski DIC Microscopy — `DicMicroscopySystem`

- Track Wollaston prism shear, polarizer angle, illumination, live-cell
  handling, focus, relief contrast, chromosome observation, sample provenance,
  and karyotype confidence.
- Use `ResearchSystem`, `MedicalHostSession`, and `DutyRosterSystem`. Results
  become evidence/research records and must not claim an exact mutation
  mechanism without an authored reference model.
- `DicMicroscopyModal` controls shear, polarization, focus, and analysis.
  Acceptance identifies the authored radiation-resistant-cell mechanism and
  completes the genetic research unlock after confidence checks pass.

### [383] Dual-Circular 2x2 MIMO Meteor Radio — `MimoMeteorSystem`

- Extend the meteor radio boundary with left/right circular polarization,
  2x2 channel matrix, phase matching, singular-value estimate, spatial streams,
  coding, packet loss, meteor window, and relay acknowledgement.
- Use `RadioHostSession`, `ResearchSystem`, and `LocationLayoutSystem`.
  Doubling throughput is an ideal channel result; it is limited by trail,
  antenna, coding, and packet conditions.
- `MimoMeteorPanel` renders polarization, matrix, singular values, and transfer
  logs. Acceptance transfers the authored encyclopedic payload across the
  2,000 km route and verifies it after retransmission/error checks.

### [384] Sanctuary Triumph Mosaic — `TriumphMosaicSystem`

- Track 100,000 tesserae, panel/ceiling geometry, glass/gold inventory,
  placement progress, artist/duty assignment, damage, restoration, narrative
  chapters, and final dedication.
- Use the typed Century Seed facade, `EpilogueMatrixRuntime`,
  `CampaignHistoryLedger`, and `SaveChecksum`. Completion must be a persisted
  campaign event, not a cinematic-only flag.
- `TriumphMosaicPanel` renders the rotunda view, tessera placement, gilding,
  chapter progress, and cinematic readiness. Acceptance places the final
  validated tessera, records the 100-year victory event, and triggers the
  epilogue cinematic through the endgame authority.

## 5. Data, host, and UI deliverables

Add validated data for cesium mud, deep-well targets, Pilger grades, dialysis
cases, noble-gas plume signatures, solera/vitality effects, terrace routes,
horseradish concentrations, flare sectors, broach/tow-truck parts, astrometric
plates, beetle species, coating tolerances, mantle targets, DIC mechanisms,
MIMO relays, mosaic chapters, and endgame events. Existing narrative files are
flavor/evidence only unless promoted through catalog validation.

Create these presentation views with typed read models and command results:

`CesiumFormatePanel`, `PilgerMillPanel`, `DialysisModal`, `NobleGasTrapPanel`,
`CenturyElixirPanel`, `TerraceFarmingPanel`, `HorseradishTowerPanel`,
`TripFlareWirePanel`, `HeavyBroachPanel`, `AstrometricPlateModal`,
`BeetleDisruptorPanel`, `VacuumCoaterPanel`, `MantleMtGeophysicsPanel`,
`DicMicroscopyModal`, `MimoMeteorPanel`, and `TriumphMosaicPanel`.

Every panel needs loading/empty/error states, accessible controls, save/load
rehydration, deterministic state-change refresh, and no hard-coded resource,
health, radiation, combat, research, radio, history, or endgame state.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, density/pressure units, blowout/failure paths,
machine quality, dialysis complications, plume uncertainty, century gating,
soil/food effects, trip-flare combat, broach fit, astrometric error, acoustic
hazards, vacuum/coating metrology, mantle inversion uncertainty, live-cell
evidence, MIMO packet loss, mosaic progress, and all 16 acceptance fixtures.

History tests must confirm drilling, treatment, agriculture, combat, research,
communications, elixir dedication, and final mosaic events publish once with
stable causal/source IDs. Catalog tests must reject unknown muds, targets,
diseases, treatments, samples, signals, heroes, mosaic chapters, and unsafe
immortality or disease-immunity modifiers.

Add host self-tests for `--batch24-selftest` and `--batch24-ui-selftest`, then
run the repository gates:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --ui-layout-selftest
./scripts/ci/godot-asset-gate.sh
```

Batch 24 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive active drilling, treatment,
agriculture, combat, radio, and endgame save boundaries, and all final-campaign
effects remain traceable to validated Core state.
