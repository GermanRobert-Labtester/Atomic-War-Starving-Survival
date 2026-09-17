# ASHFALL: Batch 27 Integration Plan

**Steps:** 417–432<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 27 is a content batch, not Expansion 27. Preserve `ExpansionSuite` at
its canonical 12 entries. Implement the batch as deterministic Core systems,
typed host sessions, validated data, and Godot-only presentation.

Use existing authorities:

- `StealthDiveInstance`/maritime host state for torpedo and seabed operations;
- `SilentFoundrySystem` and `ResearchSystem` for production, tooling, and
  machine quality;
- `MedicalHostSession`, `RespiratoryDegenerationSystem`,
  `CombatTraumaSystem`, and `NeedsSystem` for clinical state; there is no
  general `MedicalSystem` authority to duplicate;
- `WeatherSystem`, `RadiationSystem`, `GreenhouseSystem`, `RecipeCatalog`,
  `LocationLayoutSystem`, `TacticalCombatSystem`, `WarlordDoctrineSystem`,
  `ExpeditionSystem`, `RadioHostSession`, and `JournalSystem` for their live
  domains;
- `GenerationalSuccessionEngine` and `EpilogueMatrixRuntime` through typed
  facades, with the structured campaign/memorial history ledger.

The roadmap’s absolute claims become condition-based outcomes. A torpedo must
resolve through maritime combat; ECCO2R must respect cannulation and blood-gas
state; acoustic and spring-gun defenses can fail; and DSSS/QKD-style later
systems must expose loss and interception conditions rather than promise
perfect security.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch27State` and `Batch27SaveStore` at
`user://saves/slot-N/batch27_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and strict
rejection of malformed current envelopes. `SaveLoadHostSession` discovers
`_save.json` files and records their SHA-256 in the slot manifest.

New mutable state belongs in this store by default. If torpedoes, medical
treatments, radiation water, radio, or Century Seed state is later moved into
an existing authoritative store, migrate that store and remove the duplicate
batch writer. UI nodes never own inventories, combat, health, or history.

### 2.2 Shared contracts

Verify or extend the shared contracts from earlier batches:

- unit-safe inventory for torpedo mass, wire length, pressure, machining
  tolerances, blood/gas values, water dose, food, construction stone, laser
  power, and radio payloads;
- `PowerGridSystem` loads, storage, transient peaks, faults, and priority for
  sonar, ECCO2R, acoustic, laser, TEM, and radio equipment;
- deterministic evidence records carrying units, calibration, sample
  provenance, confidence, uncertainty, and source event;
- medical effect ports through the actual Medical/Trauma/Needs authority;
- location, maritime target, obstacle, line-of-sight, geodesy, and tactical
  encounter references;
- radio packet/acknowledgement/loss contracts and campaign-history events.

Extend `CatalogIntegrityValidator` for new weapons, materials, diseases,
medical procedures, samples, locations, sensors, relays, effects, hero IDs,
and endgame events. New data uses `snake_case` IDs and `schema_version`.

## 3. Ordered vertical slices

1. **Foundation:** Batch 27 save, units, power, maritime target contracts,
   medical evidence, and `--batch27-selftest` scaffolding.
2. **Maritime and industrial:** 417, 418, 425, 428, and 429 through maritime,
   foundry, research, and geophysical adapters.
3. **Clinical, water, food, and culture:** 419–421, 420, and 423 through
   medical, radiation, recipe, greenhouse, needs, and time authorities.
4. **Civil works and perimeter:** 422, 424, 426, and 427 through duty,
   location, tactical, power, and hazard systems.
5. **Laboratory and communications:** 430 and 431 after evidence and radio
   contracts are stable.
6. **Pantheon endgame:** 432 after the history, memorial, Century Seed, and
   Epilogue projections are authoritative.

## 4. Step-by-step integration contracts

### [417] Wire-Guided Torpedoes — `TorpedoBaySystem`

- Bridge `StealthDiveInstance`/maritime state with foundry work orders for
  533 mm tube service, pressure doors, electric propulsion, wire spool,
  acoustic seeker, battery, arming, target track, and launch safety.
- Use maritime target and combat resolution for range, water conditions,
  countermeasures, wire breakage, seeker confidence, and hit/damage outcome.
  The torpedo cannot directly delete a flagship from a panel command.
- `TorpedoBayPanel` controls tube valves, wire guidance, seeker, and target
  map. Acceptance resolves the authored 5 km pirate-flagship encounter as a
  destroyed/disabled outcome only when launch, guidance, and combat checks pass
  and records the coastal-security event.

### [418] Swiss Jig Borer — `JigBorerSystem`

- Extend `SilentFoundrySystem`/`ResearchSystem` precision tooling with
  vibration isolation, optical scale, boring head, spindle runout, thermal
  drift, hole position, surface finish, inspection, and operator skill.
- Register gear plates, chronometer components, and gyroscope housings with
  tolerance/compatibility data. A 0.5 µm result is an inspected quality tier,
  not a universal machine guarantee.
- `JigBorerPanel` renders coordinate reading, runout, boring, and inspection.
  Acceptance produces the authored in-tolerance aluminum housing and unlocks
  the aircraft gyroscopic-autopilot assembly path.

### [419] ECCO2R Respiratory Support — `Ecco2rSystem`

- Add low-flow circuit, cannulation, sweep gas, hollow-fiber cartridge,
  blood-flow `500 ml/min`, arterial blood gas, pH, CO2 clearance, pressure,
  anticoagulation, infection, and weaning state.
- Use `MedicalHostSession`, `RespiratoryDegenerationSystem`,
  `CombatTraumaSystem`, and `NeedsSystem` through one clinical effect port.
  Cannulation, blood pressure, equipment, and contraindications must be
  validated before treatment.
- `Ecco2rCannulationModal` submits cannulation and therapy commands.
  Acceptance moves the authored pH case from 7.05 toward the configured 7.40
  target and saves the firefighter only when blood-gas and recovery checks
  pass; complications remain persistent.

### [420] Cherenkov Water Detector — `CherenkovWaterDetectorSystem`

- Add tank sampling, optical path, photomultiplier/scintillation counts,
  background calibration, beta-energy response, strontium-90 signature,
  sample volume, detection limit, contamination, and diversion valves.
- Integrate with `RadiationSystem` and the water-storage/safety authority.
  Cherenkov counts are an instrument observation; they do not directly rewrite
  the colony’s radiation dose ledger.
- `CherenkovDetectorPanel` renders pulse counts, blue-light intensity,
  contamination, and valve state. Acceptance detects the authored trace
  strontium sample and isolates the raw tank from drinking supply.

### [421] Century Mead Solera — `MeadFlaskSystem`

- Use `RecipeCatalog`, `NeedsSystem`, `CenturySeed` time progression, and
  expedition equipment for cask lineage, age, honey/cider input, evaporation,
  tasting quality, flask inventory, allocation, and commander eligibility.
- The Golden Mead Flask applies a sourced, bounded squad morale modifier with
  duration/stacking policy; it is not an unrestricted vitality or combat aura.
- `MeadFlaskPanel` manages cask, filling, and veteran allocation. Acceptance
  equips a valid squad leader and applies the configured +30 squad morale
  effect during eligible high-stress missions.

### [422] Romanesque Aqueduct Arches — `AqueductArchesSystem`

- Extend Batch 23 aqueduct state with ravine geometry, centering falsework,
  stone/keystone inventory, arch thrust, elevation, curing, leakage, route
  continuity, and water-flow capacity.
- Use `DutyRosterSystem`, `LocationLayoutSystem`, `CaregivingSystem`, and the
  existing water route. “Forever” becomes a maintained structure with repair,
  flood, earthquake, and sediment states.
- `AqueductArchesPanel` controls span construction, keystones, and flow.
  Acceptance completes the authored 100 m Great Ravine span and reconnects the
  mountain supply when structural and hydraulic checks pass.

### [423] Wasabi Towers — `WasabiTowerSystem`

- Extend `GreenhouseSystem` with chilled gravel flow at the authored 12°C,
  rhizome growth, water quality, harvest, grating, potency, contamination, and
  meal integration.
- Use `RecipeCatalog` and `NeedsSystem`; wasabi may provide a culinary,
  sensory, or configured congestion-relief effect, but it is not an instant
  universal cure or immunity boost.
- `WasabiTowerPanel` controls chillers, gravel flow, harvest, and meal use.
  Acceptance applies the authored `clear_breathing` effect to eligible diners
  after a valid meal and clears only the configured sinus state.

### [424] Spring-Gun Tripwires — `SpringGunTrapSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for 12-gauge device, buckshot, monofilament, sear,
  arming authority, blast fan, friend/foe identification, misfire, and
  maintenance.
- Automatic lethal traps require safe placement, a valid target rule, and
  combat resolution. They cannot directly eliminate a scout from UI code.
- `SpringGunTrapPanel` controls placement, arming, and incident reports.
  Acceptance resolves the authored infiltrator as a valid trap casualty only
  when the wire, target, and tactical rules pass.

### [425] High-Pressure Gundrilling — `GundrillingSystem`

- Extend `SilentFoundrySystem`/`ResearchSystem` with 100-bar coolant, single-lip
  carbide bit, feed, chip evacuation, 3 m depth, straightness laser,
  temperature, chatter, bore finish, and tool wear.
- Register artillery barrel and oil-passage products with length, diameter,
  straightness, and pressure compatibility. “Perfect” and “mirror-smooth” are
  measured quality tiers.
- `GundrillingPanel` renders pump pressure, feed, laser, and bore inspection.
  Acceptance produces the authored two-meter barrel with in-tolerance bore
  quality and accepts it into the heavy-artillery recipe.

### [426] Precision Theodolite Network — `TheodoliteSurveySystem`

- Track engraved circles, verniers, telescope/bubble, baseline length,
  horizontal/vertical angles, atmospheric refraction, closure error, station
  control, and geodetic confidence.
- Use `JournalSystem`, `ExpeditionSystem`, `CraftingSystem`, and
  `LocationLayoutSystem`. Survey output is a confidence-bearing network, not a
  mathematically perfect continent map.
- `TheodoliteModal` controls station setup, observations, and triangulation.
  Acceptance completes the authored 10-point network and reveals the validated
  hidden pass when closure/error thresholds pass.

### [427] Blind-Leech Disruptors — `LeechDisruptorSystem`

- Add drainage/sump sectors, acoustic frequency `20–45 kHz`, transducer power,
  water coupling, pump impeller coverage, infestation response, habituation,
  maintenance, and clog/breach events.
- Connect to `PowerGridSystem`, water infrastructure, and the host hazard
  adapter. “Zero infestation” becomes a measured clean state for the authored
  sumps, not permanent pest immunity.
- `LeechDisruptorPanel` controls frequency and coverage. Acceptance clears the
  authored sump-pit infestation while retaining ongoing power and inspection
  requirements.

### [428] 500-Watt CO2 Cutting Laser — `Co2LaserSystem`

- Add CO2/N2/He gas mixture, 50 kV discharge, optical cavity, ZnSe focus,
  cooling, interlocks, beam alignment, kerf, material thickness, and power/
  gas consumption.
- Connect to `PowerGridSystem`, `SilentFoundrySystem`, and `ResearchSystem`.
  The 500 W and 25 mm values are authored capability fixtures; laser safety,
  gas depletion, and thermal faults remain active.
- `Co2LaserPanel` controls gas, discharge, focus, and cut recipe. Acceptance
  cuts the authored heavy armor plate and reduces vehicle-fabrication time from
  days to hours through measured work-order throughput.

### [429] Marine CSEM — `MarineCsemSystem`

- Bridge `StealthDiveInstance` and `LocationLayoutSystem` with dipole tow,
  seabed receiver nodes, depth/position, source current, electric-field
  response, resistivity inversion, noise, and target confidence.
- Methane hydrate/oil targets must be data-authored, environmentally gated,
  and economically assessed. “Infinite energy” is not a valid direct outcome.
- `CsemMarinePanel` renders tow line, receivers, curves, cross-sections, and
  offshore candidates. Acceptance identifies the authored gas reservoir and
  creates a coastal energy prospect requiring engineering follow-up.

### [430] 200 kV TEM — `TemMicroscopeSystem`

- Track accelerator voltage `200 kV`, vacuum, electromagnetic lenses,
  condenser/alignment, specimen thickness, diffraction, radiation shielding,
  image resolution, dose, and instrument faults.
- Use `ResearchSystem`, `MedicalHostSession`, and `DutyRosterSystem` through an
  evidence port. A TEM image is a sample-backed material result; it cannot
  directly “perfect” reactor steel without a validated metallurgy step.
- `TemMicroscopeModal` controls vacuum, voltage, lenses, and lattice image.
  Acceptance identifies the authored radiation-hardened alloy dislocations and
  unlocks the reactor-vessel steel improvement recipe.

### [431] Trellis-Coded DSSS Meteor Radio — `DsssMeteorSystem`

- Extend the meteor-radio boundary with 1,024-chip PN code, trellis coding,
  matched filter, correlation delay, 30 dB anti-jam margin, packet secrecy,
  meteor window, interception, loss, and acknowledgement.
- Use `RadioHostSession`, `ResearchSystem`, and `LocationLayoutSystem`.
  DSSS reduces detectability/interference under configured conditions; it is
  not unjammable, unobservable, or guaranteed 100% successful.
- `DsssMeteorPanel` renders code, correlation, margin, and covert logs.
  Acceptance sends the authored encrypted strike order through the specified
  jamming field and verifies receipt after packet/error checks.

### [432] Grand Pantheon of Heroes — `PantheonOfHeroesSystem`

- Use the typed Century Seed facade, `EpilogueMatrixRuntime`, memorial ledger,
  and campaign history. Track survivor identity provenance, eligibility,
  tablets, dome/oculus construction, sun angle, dedication, and cinematic
  state.
- `PantheonOfHeroesPanel` renders the dome, sunbeam, name tablets, filters, and
  endgame readiness. It must not synthesize missing survivor names or replace
  the saved epilogue state.
- Acceptance dedicates the authored 100-year roster, records the memorial
  event, and triggers the final victory cinematic through the Epilogue
  authority; reopening the pantheon must not re-trigger or duplicate history.

## 5. Data, host, and UI deliverables

Add validated data for torpedoes/targets, jig-borer tolerances, ECCO2R cases,
water isotope thresholds, mead/vitality effects, arch geometry, wasabi
conditions, spring-gun sectors, gundrilling recipes, geodetic stations,
leech species, laser cuts, marine CSEM targets, TEM samples, DSSS relays,
pantheon identities, and endgame events. Narrative files are flavor/evidence
only unless promoted through catalog validation.

Create these presentation views with typed read models and command results:

`TorpedoBayPanel`, `JigBorerPanel`, `Ecco2rCannulationModal`,
`CherenkovDetectorPanel`, `MeadFlaskPanel`, `AqueductArchesPanel`,
`WasabiTowerPanel`, `SpringGunTrapPanel`, `GundrillingPanel`,
`TheodoliteModal`, `LeechDisruptorPanel`, `Co2LaserPanel`, `CsemMarinePanel`,
`TemMicroscopeModal`, `DsssMeteorPanel`, and `PantheonOfHeroesPanel`.

Every panel needs loading/empty/error states, accessible controls, save/load
rehydration, deterministic state-change refresh, and no hard-coded inventory,
health, dose, combat, radio, history, or endgame state.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, maritime target/torpedo loss, machining
tolerances, ECCO2R contraindications, water contamination, food effects,
construction stability, trap misfires, laser safety, CSEM uncertainty, TEM
evidence, DSSS packet loss/interception, pantheon deduplication, and all 16
acceptance fixtures.

History tests must confirm maritime combat, medical treatment, scientific
observations, construction, communications, memorial, and final-campaign
events publish exactly once with stable causal/source IDs. Catalog tests must
reject unknown torpedoes, samples, diseases, procedures, targets, relays,
heroes, and unsafe permanent victory modifiers.

Add host self-tests for `--batch27-selftest` and `--batch27-ui-selftest`, then
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

Batch 27 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive active maritime, clinical,
industrial, geophysical, radio, and endgame boundaries, and every claimed
outcome is traceable to validated Core state.
