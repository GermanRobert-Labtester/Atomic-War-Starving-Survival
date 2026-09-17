# ASHFALL: Batch 28 Integration Plan

**Steps:** 433–448<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 28 is a content batch, not Expansion 28. Preserve `ExpansionSuite` at
its canonical 12 entries. Build on the existing maritime, foundry, medical,
weather, radiation, research, radio, greenhouse, tactical, and history
authorities through typed Core contracts.

Current authority mapping:

- `StealthDiveInstance`/maritime state for underwater gliders, hydrophones,
  piston cores, and ocean observations;
- `SilentFoundrySystem` and `ResearchSystem` for TIG, CNC, optics, and X-ray
  diffraction work orders;
- `MedicalHostSession`, `CombatTraumaSystem`, `RespiratoryDegenerationSystem`,
  `DiseaseSystem`, and `NeedsSystem` for clinical outcomes; no parallel
  `MedicalSystem` is introduced;
- `BrineWaterSystem`, `GreenhouseSystem`, `RecipeCatalog`, `MarketSystem`,
  `LocationLayoutSystem`, `TacticalCombatSystem`, `RadioHostSession`,
  `JournalSystem`, and the shared `PowerGridSystem`/hazard adapters;
- `GenerationalSuccessionEngine`, `EpilogueMatrixRuntime`, and the structured
  memorial/history ledger through typed facades.

The roadmap’s absolute claims become condition-based outcomes. Ocean profiles
can be incomplete, welds can fail inspection, cisterns need maintenance,
defenses can be bypassed, XRD/TEM results need provenance, and QKD cannot be
treated as an impossible-to-intercept game flag without modeling the actual
channel and endpoint.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch28State` and `Batch28SaveStore` at
`user://saves/slot-N/batch28_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and strict
rejection of malformed current envelopes. `SaveLoadHostSession` discovers the
store through its `_save.json` convention and includes it in the slot manifest.

New mutable state belongs in this store by default. If maritime profiles,
greenhouse effects, radio channels, clinical samples, or memorial state moves
into an existing authority, perform a deliberate schema migration and remove
duplicate batch fields. Godot panels never serialize simulation state.

### 2.2 Shared contracts

Verify these prerequisites before feature slices:

- unit-safe inventory for depth, salinity, pressure, water volume, salt,
  welding current/gas, crop mass, fuel, optics, radiation, and packets;
- `PowerGridSystem` support for gliders, welding, ultrasound, hydrophones,
  X-ray, barriers, microscopy, and radio loads;
- weather/radiation events for ocean conditions, surface contamination,
  rainfall, storm state, and geomagnetic/metero-radio windows;
- maritime track/sonar/observation contracts with bearing, range, noise,
  confidence, and provenance;
- medical evidence/treatment ports for echo, infection, oncology, and recovery;
- material, microscopy, diffraction, geophysical, and research unlock records;
- campaign-history, memorial, and Epilogue events with stable source IDs.

Extend `CatalogIntegrityValidator` for new machines, recipes, disease states,
locations, targets, samples, radio relays, memorial identities, and endgame
events. All new JSON uses `snake_case` IDs and `schema_version`.

## 3. Ordered vertical slices

1. **Foundation:** Batch 28 save, units, maritime observations, power,
   evidence, and `--batch28-selftest` scaffolding.
2. **Ocean and industrial:** 433–438, 441, and 442 through maritime, water,
   foundry, research, and expedition contracts.
3. **Clinical and agricultural:** 435, 437, and 439 through medical,
   greenhouse, recipe, needs, and research effects.
4. **Perimeter and tactical:** 440 and 443 through location, tactical, power,
   and hazard systems.
5. **Laboratory science:** 444 and 446 after sample provenance and medical
   evidence are stable.
6. **Radio and memorial endgame:** 447 and 448 after channel security and
   Century Seed/history integration are complete.

## 4. Step-by-step integration contracts

### [433] Autonomous Underwater Gliders — `UnderwaterGliderSystem`

- Bridge `StealthDiveInstance` and `LocationLayoutSystem` with glider hull,
  buoyancy piston, dive/ascend profile, battery, pressure, salinity-
  temperature-depth logger, radio/recall, sample custody, and radiation
  observation state.
- Ocean profiles are finite route samples with weather, power, drift, and
  sensor uncertainty. A complete 3D map requires sufficient transects and
  valid interpolation confidence.
- `UnderwaterGliderPanel` controls profile, depth, and recovery. Acceptance
  produces the authored oceanographic radiation profile and marks safe
  fishing grounds only when coverage and contamination thresholds pass.

### [434] Orbital TIG Tube Welder — `OrbitalTigWelderSystem`

- Extend `SilentFoundrySystem` with orbital head rotation, pulsed current,
  argon shielding, tungsten condition, filler, heat input, oscillation,
  penetration, distortion, and X-ray inspection.
- Register pressure tubes/heat-exchanger bundles with weld procedure,
  material, pressure class, and inspection criteria. “Perfect” and “zero
  porosity” are passed certificates, not automatic outputs.
- `OrbitalTigWelderPanel` renders head rotation, waveform, argon, and weld
  certificate. Acceptance produces a zero-porosity fixture only when the
  authored X-ray inspection and procedure checks pass.

### [435] Transthoracic Echocardiography — `EchocardiographySystem`

- Add probe view, parasternal planes, Doppler velocity, wall-motion segments,
  image quality, operator skill, radiation-cardiomyopathy markers, and
  intervention recommendation state.
- Use `MedicalHostSession`, `CombatTraumaSystem`, `NeedsSystem`, and
  `ResearchSystem` through an evidence port. A score triggers prevention or
  follow-up; it does not directly cure cardiac damage.
- `EchoCardiographyModal` controls probe plane, Doppler, scoring, and plan.
  Acceptance detects the authored irradiated-worker abnormality and creates a
  validated preventive cardiac-treatment action.

### [436] Passive Hydrophone Array — `HydrophoneArraySystem`

- Bridge `StealthDiveInstance`/maritime state with 10 km line topology,
  hydrophone channels, clock sync, noise floor, bearing/goniometer,
  propagation, acoustic signature database, and contact confidence.
- Whale, submarine, and explosion signatures are data-authored sources. An
  800 km contact depends on sound conditions, sensor coverage, and confidence;
  it is not a guaranteed omniscient detector.
- `HydrophoneArrayPanel` renders spectral waterfalls, bearing, classifier, and
  maritime alerts. Acceptance detects the authored propeller signature and
  creates a naval-watch alert through the maritime threat system.

### [437] Coastal Salt Marsh — `SeaSaltMarshSystem`

- Use `RecipeCatalog`, `NeedsSystem`, coastal location/weather, and treaty/
  market ports for tidal flooding, salicornia/sea-lavender growth, salinity,
  evaporation, hand-raking, flake quality, contamination, and stock.
- “Premium trade” is a market valuation and faction demand outcome, not a
  hard-coded luxury price. Salt remains measured inventory.
- `SeaSaltMarshPanel` controls tidal beds, refractometer, and harvest.
  Acceptance produces 50 kg of valid flake salt and the authored six-month
  seasoning stock under storage/evaporation conditions.

### [438] Underground Cistern Barrel Vault — `UndergroundCisternSystem`

- Use `DutyRosterSystem`, `LocationLayoutSystem`, water quality, and structural
  contracts for brick rings, lime rendering, capacity, inflow, filtration,
  contamination, leaks, access, and inspection.
- Five million liters and three years are authored capacity/survival fixtures;
  the cistern has finite quality, evaporation, maintenance, and drawdown.
- `UndergroundCisternPanel` manages vault layers, waterproofing, fill, and
  survival projection. Acceptance stores the configured reserve and computes
  three years of independence only under the authored consumption/quality
  assumptions.

### [439] Saffron Towers — `SaffronTowerSystem`

- Extend `GreenhouseSystem` with crocus dormancy/cold-break, tower water and
  nutrient, flowering, stigma harvest, safranal potency, contamination, and
  luxury/medical item output.
- Use `RecipeCatalog`, `NeedsSystem`, and `MarketSystem`. Saffron can support
  configured culinary or mood effects; it is not automatically a potent
  antidepressant or unlimited currency.
- `SaffronTowerPanel` controls dormancy, flowering, and hand harvest.
  Acceptance produces 10 g of validated threads and applies authored trade/
  culinary/medical effects after potency and safety checks.

### [440] Obsidian Caltrop Field — `CaltropFieldSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for beach corridors, seeding density, obsidian/iron
  quality, magnesium ignition, fire spread, amphibious movement, detection,
  and casualty resolution.
- A field creates a tactical hazard with spacing, bypass, weather, and
  friendly-access rules. It cannot guarantee a fixed 70% casualty result.
- `CaltropFieldPanel` manages corridor placement and fire-risk state.
  Acceptance resolves the authored amphibious assault with the configured
  casualty/retreat outcome when the combat model passes.

### [441] Five-Axis CNC — `FiveAxisCncSystem`

- Extend `SilentFoundrySystem`/`ResearchSystem` tooling with NC program,
  A/B/C axes, tool probe, titanium billet, tool wear, collision checks,
  balance, surface finish, and inspection.
- Register turbine impellers with geometry, balance, alloy, and turbine
  compatibility. “Flawless” becomes an inspected quality tier with rework and
  scrap paths.
- `FiveAxisCncPanel` renders toolpath, axes, probe, and yield. Acceptance
  produces a balanced authored titanium impeller and accepts it into the
  geothermal-turbine assembly path.

### [442] Marine Chronometer — `MarineChronometerSystem`

- Track escapement, jewels, temperature compensation, gimbal, guilloche,
  rate drift, calibration, maintenance, and voyage time observations.
- Use `JournalSystem`, `ExpeditionSystem`, `CraftingSystem`, and the time/
  navigation contract. A 0.3-second/day rate is an inspected tier; longitude
  error depends on usage, calibration, and voyage conditions.
- `MarineChronometerModal` controls inspection, finishing, and certificate.
  Acceptance equips the chronometer and keeps the authored three-month voyage
  longitude within 200 m when calibration and observation conditions pass.

### [443] Centipede Electrostatic Barriers — `CentipedeBarrierSystem`

- Add utility-corridor sectors, 15 kV energizer, pulse timing, grounding,
  insulation, charge, maintenance, animal response, and incursion events.
- Connect to `PowerGridSystem`, `LocationLayoutSystem`, and the host hazard
  adapter. “Zero invasion” is a monitored result for the authored swarm, not
  an absolute basement safety rating.
- `CentipedeBarrierPanel` controls charge, timing, and sector reports.
  Acceptance intercepts the authored drainage-shaft swarm and preserves the
  barrier’s power/fault state.

### [444] X-Ray Diffractometer — `XrdDiffractometerSystem`

- Track rotating copper anode, X-ray shielding, powder preparation,
  goniometer, 2θ ring positions, detector calibration, phase database,
  radiation exposure, and identification confidence.
- Integrate `SilentFoundrySystem`/`ResearchSystem`; molybdenite identification
  must be sample-backed and unlock only the authored molybdenum/superalloy
  research path.
- `XrdDiffractometerPanel` renders tube, goniometer, rings, and phase result.
  Acceptance identifies the authored grey mineral as high-grade molybdenite
  and creates the validated alloy unlock.

### [445] Seafloor Piston-Core Chronology — `PistonCoreSystem`

- Bridge maritime state with 10 m core depth, piston/corer recovery, sediment
  sections, layer provenance, Cs-137/Pb-210 decay data, calibration, age model,
  uncertainty, and war-event correlation.
- Use `LocationLayoutSystem`, `StealthDiveInstance`, `RadiationSystem`, and
  `JournalSystem`. Radioisotope dating reconstructs a confidence-bounded
  timeline; it cannot manufacture exact history from insufficient layers.
- `PistonCorePanel` controls lowering, sectioning, and dating. Acceptance
  reconstructs the authored nuclear-exchange sequence with an uncertainty band
  and unlocks the corresponding lore records.

### [446] Gold Nanoparticle Theranostics — `GoldNanoparticleSystem`

- Add citrate reduction, temperature, particle-size distribution `5–50 nm`,
  dynamic-light-scattering readout, antibody conjugation, sterility, laser
  photothermal dose, targeting, toxicity, and tumor response.
- Use `ResearchSystem`, `MedicalHostSession`, `DiseaseSystem`, and
  `DutyRosterSystem` through an evidence/treatment port. Complete remission is
  an authored clinical result, not a guaranteed effect or generic cure.
- `GoldNanoparticleModal` controls synthesis, conjugation, and treatment.
  Acceptance ablates the authored radiation-induced tumor when size, antibody
  binding, targeting, laser dose, and follow-up checks pass.

### [447] Meteor-Burst QKD Channel — `QkdMeteorSystem`

- Model optical meteor-window alignment, photon source, polarization,
  beam-splitter/coincidence counts, quantum-bit error rate, endpoint
  authentication, key sifting, loss, channel exposure, and abort conditions.
- Use `RadioHostSession`, `ResearchSystem`, and `LocationLayoutSystem`. Treat
  “uncloneable” as a protocol property only after authenticated endpoints and
  an acceptable QBER; an open meteor channel can still be unavailable or
  vulnerable at the endpoints.
- `QkdMeteorPanel` renders source, polarization, coincidence, QBER, and key
  logs. Acceptance distributes the authored key or aborts on eavesdrop/error;
  it must never report mathematical certainty from a UI toggle.

### [448] Sacred Garden of Remembrance — `EternalGardenSystem`

- Use the typed Century Seed facade, memorial ledger, `EpilogueMatrixRuntime`,
  and campaign history for tree species, planting/season, water, memorial
  stone, survivor identity, dedication, and cinematic state.
- `EternalGardenPanel` renders grove seasons, stones, inscriptions, and
  dedication. It must import every fallen survivor from the structured ledger,
  preserve provenance, and never synthesize missing names.
- Acceptance dedicates the authored memorial grove, records one peace event,
  and triggers the final remembrance cinematic through the Epilogue authority;
  reopening must be idempotent and must not duplicate the ending.

## 5. Data, host, and UI deliverables

Add validated data for glider routes/profiles, weld procedures, cardiac cases,
hydrophone signatures, salt marsh yields, cistern geometry, saffron potency,
beach hazards, CNC impellers, chronometer certificates, centipede events, XRD
phases, sediment layers, nanoparticle treatments, QKD endpoints, garden trees,
memorial identities, and endgame events. Narrative files remain flavor/evidence
only unless promoted through catalog validation.

Create these presentation views with typed read models and command results:

`UnderwaterGliderPanel`, `OrbitalTigWelderPanel`, `EchoCardiographyModal`,
`HydrophoneArrayPanel`, `SeaSaltMarshPanel`, `UndergroundCisternPanel`,
`SaffronTowerPanel`, `CaltropFieldPanel`, `FiveAxisCncPanel`,
`MarineChronometerModal`, `CentipedeBarrierPanel`, `XrdDiffractometerPanel`,
`PistonCorePanel`, `GoldNanoparticleModal`, `QkdMeteorPanel`, and
`EternalGardenPanel`.

Every panel needs loading/empty/error states, accessible controls, save/load
rehydration, deterministic state-change refresh, and no hard-coded inventory,
health, radiation, combat, research, radio, history, or endgame state.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, ocean sensor uncertainty, weld inspection,
cardiac evidence, hydrophone noise, salt/water storage loss, crop potency,
combat hazards, CNC balance, chronometer drift, barrier faults, XRD phase
confidence, sediment age uncertainty, nanoparticle toxicity, QKD abort/error
behavior, memorial deduplication, and all 16 acceptance fixtures.

History tests must confirm ocean profiles, construction, treatments, combat,
research, communications, clinical outcomes, and memorial dedication publish
exactly once with stable causal/source IDs. Catalog tests must reject unknown
targets, diseases, treatments, samples, material phases, radio endpoints,
memorial identities, and unsafe permanent modifiers.

Add host self-tests for `--batch28-selftest` and `--batch28-ui-selftest`, then
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

Batch 28 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive active ocean, industrial,
clinical, tactical, radio, and memorial boundaries, and every endgame outcome
is traceable to validated Core history and state.
