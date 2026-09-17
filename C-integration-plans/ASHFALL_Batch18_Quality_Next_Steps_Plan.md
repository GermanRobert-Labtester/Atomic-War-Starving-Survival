# ASHFALL: Batch 18 Integration Plan

**Steps:** 273–288<br>
**Generated:** 2026-08-19<br>
**Status:** Execution-ready, architecture-aligned<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1

## 1. Integration decision

Batch 18 is a content batch, not Expansion 18. Preserve the canonical
12-entry `ExpansionSuite`. Build on the shared power, inventory, nutrition,
radio, geophysics, medical, tactical, and campaign-history contracts from
earlier batches.

The current repository has real `BrineWaterSystem`, `SilentFoundrySystem`,
`GreenhouseSystem`, `MarketSystem`, `DiseaseSystem`, `WeatherSystem`,
`RadioHostSession`, `ResearchSystem`, `JournalSystem`, `DutyRosterSystem`,
`CrossingArbitrationSystem`, `TacticalCombatSystem`, and `NeedsSystem`
authorities. It does not have active `DynamicEconomySystem`,
`PowerGridPanel`/`ShelterHazardLoop` Core authorities, or a general
`GenerationalSuccessionSystem`; the current Century Seed symbol is
`GenerationalSuccessionEngine`. New work must use typed adapters and avoid
parallel writers.

Absolute roadmap results are converted into authored, condition-based
outcomes. Fire-retardant paint reduces risk but does not make a shelter
fireproof, obstacles can be bypassed, communications can be lost, and
permanent ancestor buffs require explicit provenance, stacking, and save rules.

## 2. Shared foundation and persistence

### 2.1 Batch save

Add `Batch18State` and `Batch18SaveStore` at
`user://saves/slot-N/batch18_save.json`. Use a checksummed versioned envelope,
`AtomicFileWriter`, explicit migrations, deep-copy capture/restore, and strict
rejection of malformed current envelopes. `SaveLoadHostSession` will discover
the store and include its SHA-256 in the slot manifest.

New mutable state belongs in the Batch 18 store by default. If the cold-draw
mill, acetator, meteor radio, or memorial systems extend an existing authority,
perform a deliberate schema migration and remove duplicate batch fields. UI
nodes never serialize production, treatment, combat, or history state.

### 2.2 Shared contracts

Verify these prerequisites before implementing the feature slices:

- unit-safe inventory for fuel, aggregate, pipe length, crops, samples,
  chemical concentration, signal packets, and durable machine tooling;
- high-load `PowerGridSystem` with transient peaks, storage, priority, surge,
  sensor, and compressor loads;
- weather/radiation events for rainfall, lightning, ash, radon, and geomagnetic
  conditions;
- geology contracts for boreholes, survey lines, geophones, resistivity,
  velocity, void confidence, and excavation safety;
- treatment/effect contracts for abdominal trauma, disease diagnosis, targeted
  treatment, and recovery through `NeedsSystem`/medical authorities;
- crop/soil/nutrient contracts through `GreenhouseSystem` and `NeedsSystem`;
- `CampaignHistoryLedger`, memorial ledger, and typed Century Seed facade for
  founder/hero/chronicle events.

Extend `CatalogIntegrityValidator` for new items, machines, diseases, recipes,
locations, signals, effects, historic identities, and range/uniqueness rules.
New JSON uses `snake_case` IDs and `schema_version`.

## 3. Ordered vertical slices

1. **Foundation:** Batch 18 save, units, power/weather/radiation events,
   geology and history contracts, and `--batch18-selftest` scaffolding.
2. **Fuel and industrial systems:** 273, 274, 281, and 284 through brine,
   rolling, foundry, machine-tool, and research authorities.
3. **Medical, food, and trade:** 275–279 through treatment, recipe,
   greenhouse, duty, market, and disease ports.
4. **Perimeter and tactical field:** 280, 282, and 283 through location,
   combat, astronomy, power, and hazard adapters.
5. **Geophysics and archival science:** 285 and 286 after Batch 13/17 survey,
   spectrometry, and pathology evidence paths are stable.
6. **Communications:** 287 as an extension of the Batch 14 meteor-burst radio
   contract.
7. **Hero crypt:** 288 after memorial and Century Seed history authorities are
   complete.

## 4. Step-by-step integration contracts

### [273] Bromine Extraction and Fire-Retardant Paint — `BrominePaintSystem`

- Extend `BrineWaterSystem` chemistry with bromide concentration, acidification,
  chlorination, condenser recovery, hazardous bromine handling, binder/pigment
  mixing, paint batch quality, and application coverage.
- Register fire-retardant paint as a surface treatment with burn-rate,
  toxicity, aging, and repair metadata. Connect it to wooden furniture,
  electrical looms, and lockers through a material-surface adapter.
- `BromineExtractionPanel` controls towers, condenser, mixing, and coverage.
  Acceptance reduces the authored living-quarter fire vulnerability to 0% for
  the configured test source while preserving other ignition, wiring, and
  maintenance risks.

### [274] Cold-Drawn Hydraulic Tubes — `ColdDrawTubeSystem`

- Extend Batch 17 rolling stock with tube blank grade, pickling, plug/die
  geometry, 50-ton draw-bench load, carriage speed, lubrication, wall
  thickness, surface finish, residual stress, and breakage.
- Register seamless tubes with pressure rating, diameter, length, and machine
  compatibility. “Flawless” becomes an inspected quality tier with failure
  probability, not a universal product claim.
- `ColdDrawTubePanel` manages pickling, die selection, draw, and inspection.
  Acceptance creates tubes accepted by the blast-gate hydraulic-actuator
  overhaul recipe and records any pressure/fit failures.

### [275] Abdominal Laparotomy and Splenectomy — `SplenectomySystem`

- Add abdominal trauma, hemoperitoneum, splenic rupture, blood pressure,
  vascular ligation, anesthesia, transfusion, infection, postoperative care,
  and recovery state through the existing medical/trauma/needs boundary.
- Do not add a second `MedicalSystem` or set survival directly from the modal;
  the patient’s `SurvivorNeedsState` and medical store remain authoritative.
- `SplenectomyModal` submits exploratory, ligation, resection, and care
  commands. Acceptance stops the authored fatal bleed when time, blood,
  surgeon skill, and equipment pass; complications and long-term effects are
  deterministic and persist across save/load.

### [276] Radioactive Rainwater Auto-Sampler — `RainAutoSamplerSystem`

- Extend `WeatherSystem` precipitation events with carousel index, bottle
  identity, rain-rate trigger, timed fraction, sensor failure, beta/gamma
  activity, wash-down dose, and sample custody.
- Connect to the existing radiation/dosimetry and water-reservoir safety
  boundary. A hot sample creates a diversion/quarantine command; it cannot
  delete contaminated runoff retroactively.
- `RainAutoSamplerPanel` shows moisture trigger, bottle, activity, and alert.
  Acceptance detects the authored radioactive rain and diverts roof drainage
  away from drinking cisterns through the actual water-safety path.

### [277] Barrel-Aged Balsamic Vinegar — `BalsamicBarrelSystem`

- Extend the acetator/pickling boundary with solera barrels, wood species,
  concentration, evaporation, topping, temperature, contamination, age,
  tasting quality, and batch provenance across the `CenturySeed` time scale.
- Use `RecipeCatalog`, `NeedsSystem`, and existing faction/trade/treaty
  authorities. A diplomatic gift changes stance through an arbitration result,
  not a direct trust assignment.
- `BalsamicBarrelPanel` manages racks, bungs, tasting, and gift selection.
  Acceptance presents an authored ten-year cask to the Pacific Enclave and
  raises trust to “Honored Allies” only when quality, age, diplomatic context,
  and negotiation conditions pass.

### [278] Quarry and Crushed Aggregate — `QuarrySystem`

- Model quarry face, drilling/blasting schedule, guard coverage, prisoner duty,
  rock type, crusher throughput, sieve sizes, dust, injury, stockpile, and
  construction reservation.
- Use `DutyRosterSystem`, `CaregivingSystem`, `SurvivorNeedsState`,
  `LocationLayoutSystem`, and the existing hazard/combat adapter. Convict labor
  remains a custody/needs state with safety and rehabilitation consequences,
  not an infinite resource source.
- `QuarryConvictPanel` controls schedules, guards, crusher, and stockpile.
  Acceptance produces the authored 50-ton aggregate stockpile and makes
  Sub-Level 5 construction eligible when labor, safety, and material conditions
  pass.

### [279] Bush Bean Towers and Rhizobium — `BushBeanTowerSystem`

- Extend `GreenhouseSystem` with tower volume, aeroponic misting, inoculant
  viability, Rhizobium colonization, water nitrogen ppm, flower/harvest state,
  contamination, and fresh bean output.
- Apply nitrogen cycling through the greenhouse nutrient ledger and food
  effects through `NeedsSystem`. The +20 nutrient-fertility result is a bounded
  water-system modifier with consumption and contamination behavior.
- `BushBeanTowerPanel` manages inoculation, misting, harvest, and water readout.
  Acceptance produces 30 kg fresh beans and raises the authored water fertility
  value by +20 when the culture is viable.

### [280] Concrete Dragon’s Teeth — `DragonsTeethSystem`

- Use `LocationLayoutSystem`, `TacticalCombatSystem`, and
  `WarlordDoctrineSystem` for concrete/rebar inputs, mold, curing, placement,
  row spacing, vehicle class, hang-up, bypass, damage, and artillery response.
- Obstacles channel and slow armored units; they do not create an impassable
  line or directly destroy every tank. Kill-zone resolution remains tactical
  combat state.
- `DragonsTeethPanel` controls molds and defensive layout. Acceptance channels
  the authored armored column and creates the configured hang-up/defensive
  artillery opportunity when the line is correctly placed and cured.

### [281] Heavy Planer Mill — `PlanerMillSystem`

- Extend `SilentFoundrySystem`/`ResearchSystem` precision tooling with table
  length, reciprocating stroke, crossfeed, cutter wear, lubrication, thermal
  drift, dial-indicator readings, and flatness tolerance.
- Register machine beds, surface plates, and frame rails with dimensions and
  compatible assembly recipes. Sub-millimeter flatness is an inspected target,
  not a guaranteed result.
- `PlanerMillPanel` controls stroke/crossfeed and inspection. Acceptance
  produces an in-tolerance five-meter lathe bed that unlocks the heavy engine
  lathe assembly path.

### [282] Solar Eclipse Prediction — `EclipsePredictionSystem`

- Track engraved plate quality, saros/ephemeris data, local path, duration,
  daylight, cloud/ash visibility, calendar uncertainty, and prediction
  confidence.
- Use `JournalSystem`, `ExpeditionSystem`, and `CraftingSystem`. A tactical
  morale effect is a data-authored encounter modifier with faction/culture
  prerequisites; it does not force an enemy to flee.
- `EclipsePredictionModal` renders cycle dials, path map, and timing. Acceptance
  applies the configured three-minute eclipse morale modifier to the authored
  encounter and resolves the enemy response through `TacticalCombatSystem`.

### [283] Mole-Rat Foundation Disruptor — `MoleRatDisruptorSystem`

- Add foundation sectors, piezoelectric emitter, frequency `5–30 Hz`, power,
  bedrock coupling, resonance damping, structural stress, animal response,
  habituation, and tunnel/breach events.
- Connect to `PowerGridSystem`, `LocationLayoutSystem`, and the host hazard
  adapter. Preventing a breach is a risk reduction, not a zero-breach rating
  with no maintenance or alternative route.
- `MoleRatDisruptorPanel` controls frequency and sectors. Acceptance drives the
  authored mutant mole-rat event away while keeping foundation integrity and
  power consumption visible.

### [284] Diffraction Grating Ruling — `DiffractionGratingSystem`

- Track aluminized glass, diamond stylus, 600 grooves/mm target, feed accuracy,
  interferometer stroke count, groove defects, optical resolution, and yield.
- Integrate with `SilentFoundrySystem`/`ResearchSystem` and the existing prism/
  spectroscopy chain. A grating unlocks a high-resolution instrument only when
  measured resolution passes the authored threshold.
- `DiffractionGratingPanel` controls ruling and interferometer inspection.
  Acceptance produces a valid grating and enables the spectrometer assay that
  detects trace uranium in the authored rock sample.

### [285] RMT Geophysics — `RmtSurveySystem`

- Model distant radio-source selection, electric stakes, magnetic coils,
  frequency, coupling, noise, apparent resistivity, depth resolution, and
  utility/anomaly confidence to 100 m.
- Use `LocationLayoutSystem` and `ResearchSystem`; detected pipelines are
  authored map targets requiring follow-up verification and safe excavation.
- `RmtGeophysicsPanel` renders tuning, voltage, magnetic response,
  pseudosections, and utility candidates. Acceptance detects the authored
  cast-iron water main 15 m underground and adds a validated map coordinate.

### [286] Laboratory Photomicrography — `PhotomicrographySystem`

- Track microscope/camera alignment, bellows extension, condenser aperture,
  glass plate stock, exposure, chemical development, specimen provenance,
  grain/pathology classification, and archival plate quality.
- Use `ResearchSystem`, `JournalSystem`, and `DutyRosterSystem`; a photograph
  is an evidence record with a source specimen, not an instant research bonus.
- `PhotomicrographyModal` controls focus, aperture, exposure, development, and
  archive. Acceptance photographs the authored pathogen, completes the
  Pathology Reference Atlas, and applies the configured +20% medical-research
  speed modifier through the research authority.

### [287] Meteor-Burst Multipath Synchronizer — `MeteorMultipathSystem`

- Extend Batch 14 meteor-burst radio with dual VHF yagi orientation, Doppler
  ping, multipath selection, ionization window, packet buffer, encryption,
  relay identity, acknowledgement, loss, and retransmission.
- Integrate `RadioHostSession`, `ResearchSystem`, and `LocationLayoutSystem`.
  A high-speed route is reliable only within authored burst windows and can be
  lost, jammed, or intercepted.
- `MeteorMultipathPanel` controls antennas, burst buffers, and relay logs.
  Acceptance delivers the authored encrypted map update to the 1,200 km allied
  bunker and records acknowledgement/verification.

### [288] Hero Sarcophagus Crypt — `HeroCryptSystem`

- Extend the Batch 14 memorial ledger and the current
  `GenerationalSuccessionEngine` through a typed Century Seed facade. Track
  hero eligibility, death provenance, interment consent, crypt capacity,
  marble/bronze materials, epitaph, genealogy, and dedication.
- `HeroCryptPanel` renders the crypt, selects validated heroes, and edits
  epitaphs through a journal/history command. It must not invent heroic status
  or alter death records.
- Apply `wisdom_of_the_ancestors` as a data-defined future-leader modifier with
  source, generation scope, stacking, and maintenance policy. Acceptance
  inters the authored master commander and grants the configured ancestor
  leadership effect without an uncapped permanent aura.

## 5. Data, host, and UI deliverables

Add validated data for bromine chemistry/paint surfaces, tube grades and
hydraulic recipes, abdominal trauma/disease cases, rain sample thresholds,
vinegar aging/gifts, quarry faces and labor rules, bean/nitrogen cycles,
obstacle lines, planer tolerances, eclipse fixtures, pest species, diffraction
targets, RMT anomalies, specimens/plates, meteor relays, hero identities, and
ancestor modifiers. Existing narrative files are flavor/evidence only unless
promoted through catalog validation.

Create these presentation views with typed read models and command results:

`BromineExtractionPanel`, `ColdDrawTubePanel`, `SplenectomyModal`,
`RainAutoSamplerPanel`, `BalsamicBarrelPanel`, `QuarryConvictPanel`,
`BushBeanTowerPanel`, `DragonsTeethPanel`, `PlanerMillPanel`,
`EclipsePredictionModal`, `MoleRatDisruptorPanel`, `DiffractionGratingPanel`,
`RmtGeophysicsPanel`, `PhotomicrographyModal`, `MeteorMultipathPanel`, and
`HeroCryptPanel`.

Every panel needs loading/empty/error states, accessible controls, save/load
rehydration, deterministic state-change refresh, and no hard-coded resource,
health, combat, research, radio, or memorial state.

## 6. Verification and exit criteria

Core tests must cover deterministic replay, capture/restore, migration,
malformed-envelope rejection, unit conversions, fire-risk reduction, tube
pressure/quality, surgical complications, radiation sampling, decade-scale
aging, prisoner labor safety, greenhouse nitrogen contamination, obstacle and
combat routing, astronomy confidence, acoustic hazards, optical metrology,
geophysical noise, evidence provenance, packet loss, and all 16 acceptance
fixtures.

History tests must confirm trade gifts, quarry work, combat, research plates,
communications, interments, and epitaphs publish exactly once to
`CampaignHistoryLedger` and memorial records. Catalog tests must reject unknown
materials, diseases, samples, signals, locations, heroes, recipes, and unsafe
permanent modifiers.

Add host self-tests for `--batch18-selftest` and `--batch18-ui-selftest`, then
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

Batch 18 is ready when all 16 fixtures pass from clean and migrated saves,
replay identically with the same seed, survive save/load at active production,
medical, combat, radio, geophysical, and memorial boundaries, and all
permanent-looking effects are traceable to validated Core state.
