# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 35)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 545–560

## Outcome

Batch 35 expands childhood culture, disciplined self-defence, archaeology,
upper-atmosphere observation, agricultural storage, fine art, cartography,
closed-loop production, cryptography, waste-to-energy, and the centennial
celebration. These features extend existing authorities rather than putting
simulation in Godot panels:

- `JournalSystem`, `NeedsSystem`, `GenerationalSuccessionEngine`, and
  `DutyRosterSystem` own education, culture, participation, and history.
- `ExpeditionSystem`, `ResearchSystem`, `WeatherSystem`, `RadioHostSession`,
  and `LocationLayoutSystem` own surveys, observations, discoveries, and maps.
- `GreenhouseSystem`, `RecipeCatalog`, `GoodsCatalog`, `CraftingSystem`,
  `SilentFoundrySystem`, and the shared water/power authorities own production.
- `CrossingArbitrationSystem`, `WarlordDoctrineSystem`, and the event bus own
  authenticated diplomacy and security outcomes.
- `EpilogueMatrixRuntime` and the generational history contract own the
  centennial milestone; no panel writes an ending flag directly.

`PowerGridPanel`, `District8Accords`, and `CenturySeed` remain conceptual
roadmap names. Use typed power transactions, validated agreement IDs, and the
actual generational engine. No new duplicate authority is permitted.

## Batch entry gates

1. Add `Batch35State` and `batch35_save.json`; register it through the normal
   checksummed save envelope and manifest hash path.
2. Persist stable IDs, provenance, calibration, environmental conditions,
   authored content, resource transactions, and completion evidence. Stateful
   Core systems implement `CaptureState`/`RestoreState` and raise state-change
   events.
3. Treat “perfect”, “zero spoilage”, “unbreakable”, “all waste”, and “highest
   ever” as measured or configured thresholds. They are not unconditional
   global modifiers.
4. Use `ISeededRng` for weather, archaeology finds, defects, ecological
   sampling, and event outcomes. Do not use `System.Random` or `Guid.NewGuid()`.
5. Add snake_case catalog entries under `Assets/StreamingAssets/Data/` with
   `schema_version`; validate ranges, references, uniqueness, and authored
   content before enabling UI actions.

## Delivery order

1. **545–548:** Establish education, training, archaeology, and upper-air
   observation contracts.
2. **549–553:** Add timber storage, grain preservation, art/cartography, and
   sensor-controlled greenhouse production.
3. **554–557:** Add deep-sea science, textile/cidery production, and geohazard
   mapping.
4. **558–559:** Add authenticated cipher workflows and organic-waste energy.
5. **560:** Assemble the centennial history, event, and epilogue proof only
   after all prerequisite records are replay-safe.

## Shared implementation contract

`Batch35State` owns feature registration, cross-feature references, schema
version, and migrations; feature systems own their simulation state. Each step
gets a Core command/state boundary, a thin `src/UI/` presenter, catalog data,
deterministic replay tests, state round-trip tests, invalid-input tests, and a
user-facing acceptance fixture. Panels may dispatch commands and render read
models but cannot mutate inventory, power, weather, or history directly.

## Step integration matrix

### [545] Children’s Illustrated Picture Book Press & Storytelling Library

**Core integration:** Add `PictureBookPressSystem` over `JournalSystem`,
`NeedsSystem`, `GenerationalSuccessionEngine`, child identity/age, education,
author/illustrator duties, print materials, and the archive document store.

**Smallest vertical slice:** Author one age-appropriate manuscript, create one
illustration, print and bind one book, assign it to a child cohort, and record a
lesson/reading milestone.

**Acceptance gate:** Ten books count only when each has a valid manuscript,
illustrations, provenance, binding, reading session, and age-appropriate
assessment. Full literacy by age eight is a configured cohort result requiring
attendance, fatigue, instruction quality, and prior learning; the press cannot
write a literacy stat directly.

**UI:** `PictureBookPressPanel.cs` shows manuscript, illustration, type setting,
binding, child cohort, reading sessions, and literacy evidence.

### [546] Martial Arts & Self-Defence Training Dojo

**Core integration:** Add `MartialArtsTrainingSystem` over `DutyRosterSystem`,
`TacticalCombatSystem`, `NeedsSystem`, survivor skills, injury/medical state,
and training venue capacity.

**Smallest vertical slice:** Enrol one scout, run a safe sparring session,
record fatigue/injury checks, award a belt assessment, and apply one validated
close-quarters skill modifier.

**Acceptance gate:** Thirty graduates require attendance, competency tests,
recovery time, and safe equipment. The -50% fatality target is a combat-cohort
comparison with explicit exposure and sample size; training reduces risk but
does not guarantee survival or eliminate injury.

**UI:** `MartialArtsDojoPanel.cs` displays rosters, belt progress, sparring
outcomes, fatigue/injury, instructor duty, and combat-readiness modifiers.

### [547] Archaeological Ruin Excavation Field School & Stratigraphy

**Core integration:** Add `ArchaeologyFieldSchoolSystem` over
`ExpeditionSystem`, `JournalSystem`, `ResearchSystem`, location/site entities,
stratigraphic layers, artefact provenance, conservation, and research review.

**Smallest vertical slice:** Register one ruin, excavate one grid cell, record
layer order and context, conserve one artefact, and submit one evidence-backed
knowledge candidate.

**Acceptance gate:** Excavation consumes time, tools, labour, and site integrity;
context is never regenerated after removal. A university laboratory find
unlocks five research nodes only after manuals/formulas pass provenance,
condition, interpretation, and catalog validation.

**UI:** `ArchaeologyFieldSchoolPanel.cs` presents trench grid, layers, finds,
conservation, context records, and research-review status.

### [548] Colony Radiosonde Weather Balloon Network & Upper Atmosphere

**Core integration:** Add `RadiosondeNetworkSystem` over `WeatherSystem`,
`RadioHostSession`, balloon/gas inventory, sensor calibration, launch duty,
upper-atmosphere profiles, radiation exposure, and forecast assimilation.

**Smallest vertical slice:** Launch one sonde, collect a profile to its safe
ceiling, transmit packets, validate calibration, and improve one forecast
interval with a confidence score.

**Acceptance gate:** Twice-daily launches and a five-day forecast horizon require
gas, packages, radio link, recovery/telemetry quality, and sufficient station
continuity. Forecast accuracy is a confidence-weighted result; missing sondes,
wind drift, sensor failure, and radio loss remain possible.

**UI:** `RadiosondeNetworkPanel.cs` shows inflation, launch schedule, sensor
calibration, altitude profile, telemetry loss, and forecast uncertainty.

### [549] Traditional Timber-Frame Barn Raising & Post-and-Beam Joinery

**Core integration:** Add `BarnRaisingSystem` over `DutyRosterSystem`,
`LocationLayoutSystem`, `NeedsSystem`, timber inventory, joinery quality,
construction safety, event attendance, and agricultural storage.

**Smallest vertical slice:** Cut and inspect one mortise-and-tenon set, assign a
raising crew, erect one frame, and commission its storage capacity.

**Acceptance gate:** Three barns require timber mass, joinery checks, weather,
crew fatigue, structural inspection, and site capacity. A single-day raising is
an event-duration target, not a guarantee; morale and storage modifiers are
granted only after safe commissioning.

**UI:** `BarnRaisingPanel.cs` handles joint plans, materials, volunteer duty,
raising progress, safety, inspection, and storage capacity.

### [550] Cooperative Grain Elevator Silo & Moisture-Controlled Storage

**Core integration:** Add `GrainElevatorSystem` over `GreenhouseSystem`,
`NeedsSystem`, `LocationLayoutSystem`, grain goods, conveyor throughput,
moisture/temperature sensors, aeration power, pest control, and storage loss.

**Smallest vertical slice:** Load one grain lot, monitor moisture and heat,
run aeration, resolve a storage interval, and retrieve a measured ration.

**Acceptance gate:** The 1,000-tonne/18-month target is capacity data requiring
validated bin volume, dry grain, aeration, pest control, and power. “Zero
spoilage” is a certificate threshold with sampling and loss tolerance; mould,
insects, sensor faults, and power outages can reduce the lot.

**UI:** `GrainElevatorPanel.cs` presents conveyor state, bin fill, humidity,
temperature cables, aeration, pest alerts, and stock quality.

### [551] Natural Charcoal Drawing & Fine Arts Studio

**Core integration:** Add `FineArtsStudioSystem` over `NeedsSystem`,
`DutyRosterSystem`, `JournalSystem`, `CraftingSystem`, pigment/canvas goods,
artist identity, exhibition, and cultural-event records.

**Smallest vertical slice:** Create one work, record artist/material provenance,
curate it in the gallery, and apply a bounded audience morale response.

**Acceptance gate:** Fifty original works require unique authorship, materials,
completion, conservation, and display records. “Surrounded by Art” is a single
ceremony-backed cultural trait with a defined scope and cap, not a repeatable
gallery exploit.

**UI:** `FineArtsStudioPanel.cs` covers easels, materials, work metadata,
gallery placement, audience, and cultural effect.

### [552] Handmade Mosaic Cartographic World Map & Geography Institute

**Core integration:** Add `MosaicCartographySystem` over
`LocationLayoutSystem`, `ExpeditionSystem`, `JournalSystem`, surveyed geometry,
territory/discovery records, tessera materials, and research modifiers.

**Smallest vertical slice:** Place one map tile from a validated survey, mark an
allied settlement, update it with a new discovery, and recalculate coverage.

**Acceptance gate:** Full-map completion requires all eligible discovered
territories, confidence thresholds, material coverage, and versioned updates.
The Geography Institute and +30% planning bonus apply only to complete,
non-stale cartographic evidence and do not reveal unexplored territory.

**UI:** `MosaicWorldMapPanel.cs` provides tile placement, territory brushes,
survey confidence, update markers, and institute progress.

### [553] Hydroponics Automated Nutrient Dosing Robot & Sensor Array

**Core integration:** Add `AutomatedNutrientDosingSystem` over
`GreenhouseSystem`, sensor calibration, nutrient A/B goods, water/power,
peristaltic pumps, crop beds, and failure alarms.

**Smallest vertical slice:** Calibrate EC/pH/oxygen/temperature sensors, run a
closed-loop dosing cycle on one bed, and resolve one crop growth interval.

**Acceptance gate:** The +35% yield target applies to eligible monitored beds
with valid calibration, nutrient stock, pump health, and power. “No deficiency
failures” means the configured threshold during the tested interval; sensor
drift, dosing error, contamination, and empty tanks remain modeled.

**UI:** `AutoNutrientDosingPanel.cs` displays sensor confidence, dosing schedule,
tank levels, alarms, bed coverage, and actual yield delta.

### [554] Deep-Sea Submersible ROV & Hydrothermal Vent Exploration

**Core integration:** Add `DeepSeaRovSystem` over `StealthDiveInstance`,
`ResearchSystem`, `LocationLayoutSystem`, tether/power, depth/pressure,
manipulator state, sample custody, and maritime hazards.

**Smallest vertical slice:** Deploy one ROV, maintain tether and telemetry,
collect one chimney sample, return it, and log a controlled extremophile assay.

**Acceptance gate:** Ultra-rich minerals and novel organisms unlock only when
depth, sample location, condition, custody, and assay evidence pass thresholds.
Tether breakage, pressure damage, current, battery, and submarine exposure can
abort the dive; discovery is not guaranteed by opening the panel.

**UI:** `DeepSeaROVPanel.cs` shows thrusters, tether, depth/pressure, manipulator,
sample baskets, telemetry, and discovery evidence.

### [555] Traditional Wax-Resist Batik Textile Dyeing Studio

**Core integration:** Add `BatikDyeingSystem` over `NeedsSystem`,
`CraftingSystem`, `GreenhouseSystem` wax/dye inputs, textile goods, labour skill,
and trade-quality evaluation.

**Smallest vertical slice:** Prepare cloth, draw one wax motif, complete two dye
baths, remove resist, and grade the resulting textile.

**Acceptance gate:** Trade value depends on cloth mass, wax/dye consumption,
layer registration, colourfastness, defects, and artisan skill. “Master Dyer” is
awarded through a quality portfolio and review, not one lucky item.

**UI:** `BatikDyeingStudioPanel.cs` handles wax temperature, motif, dye cycles,
colour layers, defects, and trade grade.

### [556] Cooperative Orchard Cidery & Wild Apple Fermentation

**Core integration:** Add `CiderySystem` over `RecipeCatalog`, `NeedsSystem`,
`GreenhouseSystem`, orchard harvest, press throughput, barrel inventory,
fermentation, alcohol safety, storage, and trade goods.

**Smallest vertical slice:** Harvest fruit, press one batch, track gravity and
fermentation, rack a barrel, and register a safe cider lot.

**Acceptance gate:** One hundred barrels require orchard yield, juice volume,
barrels, yeast, time, sanitation, and storage. ABV, spoilage, pressure, and
worker safety are recorded; a celebration/trade value cannot be created from
unfermented or contaminated stock.

**UI:** `CideryOrchardPanel.cs` shows harvest, press, barrel schedule, gravity,
ABV, sanitation, stock, and trade value.

### [557] Geological Geomorphology Survey & Landslide Hazard Mapping

**Core integration:** Add `LandslideHazardSystem` over `WeatherSystem`,
`LocationLayoutSystem`, `ShelterHazardLoop`, geotechnical sensors, slope/soil
profiles, pore pressure, infrastructure footprints, and relocation work orders.

**Smallest vertical slice:** Survey one slope, install crack/pore-pressure
monitoring, calculate a confidence-bounded risk class, and relocate one exposed
asset or commission stabilization.

**Acceptance gate:** The grain-elevator slope is classified unstable only from
measured geometry, saturation, cracks, and threshold data. Stabilization reduces
risk after construction/maintenance validation; it does not promise that all
future landslides are prevented.

**UI:** `LandslideHazardPanel.cs` presents slope maps, instruments, readings,
risk confidence, exposed assets, and mitigation orders.

### [558] Mechanical Slot-Dial Enigma-Style Polyalphabetic Cipher System

**Core integration:** Add `CipherMachineSystem` over
`CrossingArbitrationSystem`, `RadioHostSession`, `LocationLayoutSystem`, key
ceremony, rotor/plugboard configuration, operator duty, message envelopes,
authentication, and key rotation.

**Smallest vertical slice:** Generate a daily key, encrypt/decrypt one
authenticated message with matching settings, and record a failed-key attempt.

**Acceptance gate:** Sensitive messages use valid key IDs, recipient identity,
operator duty, and rotation policy. “Unreadable to enemies” becomes a configured
security margin with key compromise, operator error, and interception risks;
the feature cannot claim cryptographic unbreakability.

**UI:** `CipherMachinePanel.cs` handles key cards, rotors, plugboard, tapes,
authentication, and key compromise/rotation status.

### [559] Composting Biodigester & Biogas Methane Micro-Grid

**Core integration:** Add `BiogasDigesterSystem` over organic-waste goods,
`NeedsSystem`, the shared power authority, heat, pH/temperature, retention time,
gas storage, flare safety, and CHP maintenance.

**Smallest vertical slice:** Load a measured slurry batch, run digestion, assay
gas composition, store methane, and settle one CHP generation interval.

**Acceptance gate:** Fifty kW is available only with sufficient feedstock,
retention, methane quality, storage pressure, generator condition, and grid
capacity. Waste diversion and power output are separate ledgers; the system
cannot eliminate operating cost or process hazardous material without checks.

**UI:** `BiogasDigesterPanel.cs` displays feed, pH/temperature, retention,
methane quality, pressure, flare, CHP output, and waste diversion.

### [560] Grand Colony Founding Day Centennial Celebration & Pageant

**Core integration:** Add `CentennialPageantSystem` over
`GenerationalSuccessionEngine`, `NeedsSystem`, `JournalSystem`,
`EpilogueMatrixRuntime`, venues, performers, food, safety, and the campaign
history ledger.

**Smallest vertical slice:** Validate the founding date, assemble one program,
assign performers and resources, run one event, and archive attendance and
outcomes.

**Acceptance gate:** The centennial event is available only at the authored
milestone and with adequate safety, food, performers, and venue capacity. The
“Century Pride” trait and victory epilogue are idempotent, checksummed, and
history-backed; the event cannot be replay-farmed or called the highest morale
event without a recorded comparison metric.

**UI:** `CentennialPageantPanel.cs` covers program, floats, music, fireworks
safety, feast, attendance, historical reading, and epilogue state.

## Data, UI, and test deliverables

- Add `batch35_features.json`, child curricula, dojo ranks, archaeology layers,
  radiosonde sensor definitions, timber/storage specs, art/map schemas,
  hydroponic control thresholds, ROV sample types, textile/cidery recipes,
  landslide thresholds, cipher key policies, biodigester process data, and
  centennial event definitions under `Assets/StreamingAssets/Data/`.
- Add thin presenters: `PictureBookPressPanel.cs`, `MartialArtsDojoPanel.cs`,
  `ArchaeologyFieldSchoolPanel.cs`, `RadiosondeNetworkPanel.cs`,
  `BarnRaisingPanel.cs`, `GrainElevatorPanel.cs`, `FineArtsStudioPanel.cs`,
  `MosaicWorldMapPanel.cs`, `AutoNutrientDosingPanel.cs`,
  `DeepSeaROVPanel.cs`, `BatikDyeingStudioPanel.cs`, `CideryOrchardPanel.cs`,
  `LandslideHazardPanel.cs`, `CipherMachinePanel.cs`,
  `BiogasDigesterPanel.cs`, and `CentennialPageantPanel.cs`.
- Test deterministic replay, state round-trip, resource conservation,
  calibration/failure paths, child-safety and training eligibility, provenance,
  cryptographic key mismatch, power settlement, and one-time milestone behavior.
  Add Godot command-wiring, disabled-state, navigation, and layout tests.
- Run the Core build/test, Godot project build, four headless self-tests, and
  `./scripts/ci/godot-asset-gate.sh` required by `AGENTS.md`.

## Batch completion definition

Batch 35 is complete when all 16 contracts are catalog-validated,
deterministically replayable, round-trip safe, and reachable through Godot UI;
upper-air observations, archaeological evidence, map coverage, production
outputs, security records, and the centennial event are journaled and the
required verification suite passes.
