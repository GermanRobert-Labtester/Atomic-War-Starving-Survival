# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 22)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 337–352

## Outcome

Batch 22 should extend the late-game chemistry, industrial rolling, clinical life support, seismic/radon observation, river engineering, agriculture, perimeter defense, five-axis machining, navigation, pest control, advanced microscopy, deep geophysics, adaptive radio, and century carillon contracts.

It is not acceptable to implement these as disconnected UI simulations. The active Core authorities remain:

- **SilentFoundrySystem** and **CraftingSystem** for metals, wire rod, machine jobs, material conservation, and equipment condition.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** for ECMO, clinical observation, and bounded survivor effects.
- **WeatherSystem**, **RadiationSystem**, and the shared environmental sensor contract for radon, deposition, and hazardous conditions.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and the meal/effect pipeline for crops, tonics, and preservation.
- **DutyRosterSystem**, CaregivingSystem, and civil-worksite state for supervised labor and safety.
- **LocationLayoutSystem**, **TacticalCombatSystem**, and **WarlordDoctrineSystem** for perimeter layout, assaults, and hazards.
- **ResearchSystem**, **JournalSystem**, **FactionRadioEngine**, and RadioHostSession for microscopy, geophysics, radio, and authored knowledge.
- **GenerationalSuccessionEngine**, archive/culture state, SoundManager, EpilogueMatrix, and **SaveChecksum** for century continuity and the carillon milestone.

PowerGridPanel, ShelterHazardLoop, District8Accords, and CenturySeed are not assumed to be gameplay authorities. If they are only presentation names, route their behavior through typed Core contracts.

## Batch entry gates

1. **Pyrotechnic and hazardous chemistry:** reuse Batch 21 signal-flare handling with barium compounds, nitric acid, storage, ignition, contamination, and non-combat signal semantics.
2. **Wire-rod and downstream process:** connect continuous rolling to galvannealing, wire drawing, springs, fasteners, welding wire, heat treatment, coil handling, and exact mass settlement.
3. **ECMO life-support contract:** define ARDS/chemical injury, cannulation, pump/oxygenator, anticoagulation, blood loss, infection, circuit failure, recovery, and death. Do not add a medical device as a success button.
4. **Radon and seismic observation:** define radon sampling, background, fissure identity, trend analysis, earthquake/fault event uncertainty, and evacuation recommendations. No confirmed earthquake-prediction authority exists by default.
5. **Solera and diplomatic state:** reuse aged-vinegar lineage, gifting, faction negotiation, treaty evidence, and MarketSystem relations.
6. **Flood and agricultural infrastructure:** extend culvert/road drainage into river levees, riprap, flood stages, farming access, and maintenance.
7. **Perimeter and acoustic hazard reuse:** extend the established wire, trap, sensor, and pest-disruptor models; avoid separate trigger or frequency systems.
8. **Complex machining and optics:** carry forward tracer lathe, VTL, interferometer, etalon, and microscopy contracts with measured capacity, calibration, and uncertainty.
9. **Deep geophysics and radio:** extend AMT/RMT/ERT/TEM/SP and meteor-burst protocols with depth, inversion, multipath, burst, SNR, coding, and delivery confidence.
10. **Cultural clockwork:** define bell/carillon construction, pitch, schedule, recordings, authorship, maintenance, and replayable anniversary state; do not tie a fixed real-world day directly to victory without the simulation calendar.

## Delivery order

1. **337, 338:** establish barium signaling and high-tonnage wire-rod production.
2. **339, 340:** add emergency life support and radon/fissure observations.
3. **341, 342, 343:** extend solera diplomacy, river protection, and fast crop/meal cycles.
4. **344, 347:** extend perimeter belts and subterranean pest/hazard acoustics.
5. **345, 348, 350:** add five-axis machining, confocal imaging, and atomic-force metrology.
6. **346, 349:** extend solar time and deep-crust surveys.
7. **351:** finish adaptive meteor-burst radio.
8. **352:** complete the century carillon and final cultural milestone.

## Step integration matrix

### [337] Barium Emerald Signal Pyrotechnics — BariumFlarePanel

**Core integration:** Extend the Batch 21 non-combat pyrotechnic state over geothermal/brine inputs, SilentFoundrySystem, CraftingSystem, flare/star-shell inventory, faction recognition, convoy/encounter events, and weather visibility.

**Smallest vertical slice:** Produce one valid barium signal flare, fire it from a defined location, and create a color-coded recognition event for one known friendly route or convoy.

**Acceptance gate:** Reagent purity, acid handling, payload, casing, ignition, storage, burn duration, cloud/fallout, observer range, station identity, and false-recognition risk are saved. Friendly recognition can improve encounter readiness; it cannot automatically call artillery or guarantee an allied encirclement.

### [338] Morgan Wire-Rod Mill — WireRodMillPanel

**Core integration:** Extend SilentFoundrySystem with multi-pass billet reduction, laying-head coiling, cooling, coil condition, heat identity, wire drawing, fastener, spring, and welding-wire consumers.

**Smallest vertical slice:** Roll one valid billet heat into a measured wire-rod coil, cool and inspect it, then consume part of it in one downstream recipe.

**Acceptance gate:** Heat mass, billet chemistry, pass schedule, roll wear, temperature, speed, cooling, scale, coil geometry, scrap, and inspection are authoritative. A five-ton heat and 2,000 meters of 5.5 mm rod are data-defined outputs requiring the correct billet mass and yield.

### [339] VV-ECMO Pulmonary Cannulation — EcmoCannulationModal

**Core integration:** Add a high-risk life-support procedure over MedicalSystem, CombatTraumaSystem, DiseaseSystem, SurvivorNeedsState, respiratory injury, clinical inventory, pump/oxygenator condition, staff skill, anticoagulation, and the death/recovery pipeline.

**Smallest vertical slice:** Triage one eligible severe respiratory case, cannulate one venous circuit, run oxygenation for a bounded interval, and resolve gas exchange, circuit condition, complications, or deterioration.

**Acceptance gate:** ARDS severity, cannula placement, flow, oxygen/gas supply, membrane condition, anticoagulation, bleeding, infection, clotting, staff, circuit alarms, and lung recovery are persisted. ECMO can bridge a recoverable case; it cannot guarantee survival from 100% pulmonary failure or replace a respiratory diagnosis.

### [340] Radon-222 Ionization Chambers — RadonIonChamberPanel

**Core integration:** Add continuous radon observation over WeatherSystem, RadiationSystem, LocationLayoutSystem fissure records, gas sampling pumps, ion chambers, ResearchSystem, and hazard/evacuation recommendations.

**Smallest vertical slice:** Sample one fissure, estimate concentration and trend with uncertainty, and create a lower-shaft inspection or evacuation recommendation.

**Acceptance gate:** Fissure flow, radon concentration, chamber background, pump rate, humidity, calibration, ventilation, occupancy, trend window, and sensor failure are saved. A spike changes risk assessment; it does not predict an earthquake with certainty or promise 24 hours of warning.

### [341] Sovereign Solera Decanter — CenturyDecanterPanel

**Core integration:** Extend the Batch 21/19 solera and diplomatic-gift state over RecipeCatalog/data, GoodsCatalog, JournalSystem, GenerationalSuccessionEngine, MarketSystem, faction relations, certificates, seals, and treaty outcomes.

**Smallest vertical slice:** Draw a valid reserve portion, bottle and certify it, present it to one faction, and resolve a negotiation record.

**Acceptance gate:** Cask lineage, age, transfer history, evaporation, acidity, bottle/seal/certificate, faction attitude, demands, concessions, and treaty terms are persisted. The decanter can unlock a summit or improve trust; it cannot force a permanent non-aggression pact without a valid faction-resolution outcome.

### [342] River Riprap and Levees — RiverRiprapPanel

**Core integration:** Extend the Batch 21 drainage/road infrastructure contract over DutyRosterSystem, CaregivingSystem, LocationLayoutSystem, river/flood state, quarry inventory, worksite safety, farming access, and maintenance.

**Smallest vertical slice:** Place riprap on one bank segment, raise or reinforce one levee, simulate a defined river crest, and update breach probability and farmland access.

**Acceptance gate:** Stone mass, bank geometry, slope, flow, crest height, soil, erosion, rainfall, debris, worker safety, inspection, and maintenance are authoritative. The levee reduces breach probability; “zero flood breach” is not a valid universal state.

### [343] Vertical Turnip Greens and Broths — TurnipTowerPanel

**Core integration:** Extend GreenhouseSystem with a rapid turnip crop, leaf/root harvest allocation, broth recipes, storage, heat/fuel, GoodsCatalog, and NeedsSystem meal effects.

**Smallest vertical slice:** Grow one tower cycle, harvest greens and roots, cook one broth batch, and consume it in one worker meal.

**Acceptance gate:** Growth duration, water, nutrients, light, temperature, crop loss, root/leaf mass, cooking fuel, spoilage, storage, and meal participation are saved. Warmth or energy effects are named, bounded meal effects; 30 kilograms and universal worker energization require actual crop and dining state.

### [344] Triple Concertina Perimeter Belts — TripleConcertinaPanel

**Core integration:** Extend the Batch 21/20 wire and tactical obstacle state over LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, steel pickets, artillery/vehicle interaction, blast displacement, maintenance, and friendly access.

**Smallest vertical slice:** Install one triple-belt ridge segment, simulate one blast or vehicle approach, and resolve delay, displacement, breach, or countermeasure state.

**Acceptance gate:** Belt geometry, anchoring, wire length, terrain, blast pressure, vehicle mass, cutting tools, maintenance, alternate routes, and defender exposure are persisted. A belt can slow or redirect an assault; it cannot be impenetrable or guarantee a kill zone outcome.

### [345] Five-Axis Universal Toolroom Mill — UniversalMillPanel

**Core integration:** Extend SilentFoundrySystem and ResearchSystem with five-axis setup, rotary-table orientation, dividing-head indexing, complex impeller/volute/screw profiles, metrology, machine condition, and turbine/pump consumers.

**Smallest vertical slice:** Mount one valid blank, execute one indexed multi-axis job, inspect profile and balance, and install the component into one compatible pump or turbine.

**Acceptance gate:** Blank geometry, axes travel, table angle, cutter reach, feed, spindle speed, vibration, coolant, power, tolerance, balancing, scrap, and installation compatibility are authoritative. A 25% efficiency change is measured through the consumer model; it cannot be a direct generator stat write.

### [346] Equatorial Sundial and Gnomon — EquatorialSundialModal

**Core integration:** Add a calibrated timekeeping artifact over JournalSystem, ExpeditionSystem, CraftingSystem, SimClock, latitude, equation-of-time data, weather/visibility, and the Batch 10/21 navigation devices.

**Smallest vertical slice:** Build and calibrate one sundial at a fictional site, obtain one valid solar observation, and correct one local mechanical clock with an uncertainty interval.

**Acceptance gate:** Latitude, gnomon alignment, date, solar visibility, equation-of-time table, shadow measurement, observer skill, and clock drift are persisted. A sundial aligns local solar time; it cannot guarantee absolute chronological accuracy or synchronize every underground clock without a modeled transfer.

### [347] Cave-Centipede Acoustic Disruptors — CentipedeDisruptorPanel

**Core integration:** Extend the Batch 21 acoustic-hazard state over LocationLayoutSystem, ShelterHazardLoop if confirmed, mining/ore-hopper locations, power, transducer condition, creature behavior, and miner hazard events.

**Smallest vertical slice:** Install one lower-shaft emitter, configure a safe frequency band, observe one hazard interval, and record creature pressure, energy, maintenance, and miner safety.

**Acceptance gate:** Creature species, baseline activity, tunnel geometry, resonance, attenuation, frequency response, power, worker exposure, false confidence, and fallback defenses are saved. The device can reduce encounters or redirect a creature; it cannot guarantee zero attacks or clear every tunnel.

### [348] Confocal Laser Scanning Microscope — ConfocalMicroscopePanel

**Core integration:** Extend Batch 19/21 optical and laboratory observation states over ResearchSystem, MedicalSystem, DutyRosterSystem, microscope condition, laser/filter inventory, pinhole, galvanometer scan, sample stack, and knowledge unlocks.

**Smallest vertical slice:** Acquire a valid sample, scan one bounded Z-stack, reconstruct a typed observation, and add one research lead with provenance and confidence.

**Acceptance gate:** Laser wavelength, power, pinhole, alignment, scan step, photobleaching, sample preparation, optical resolution, operator skill, data size, and reconstruction uncertainty are persisted. A radiation-resistance mechanism is a possible discovery requiring catalogued evidence and research validation, not an automatic result of one scan.

### [349] Deep Magnetotelluric Sounding — DeepMtGeophysicsPanel

**Core integration:** Extend the shared geophysical survey state over LocationLayoutSystem, ResearchSystem, ExpeditionSystem equipment, broadband coils, telluric pots, low-frequency recording, inversion, crustal layers, and heat-flow discoveries.

**Smallest vertical slice:** Run one long-period station, collect valid low-frequency data, invert a bounded depth profile, and create a geothermal or crustal target with uncertainty.

**Acceptance gate:** Recording duration, sensor drift, station separation, electrode contact, cultural noise, geomagnetic conditions, frequency band, inversion regularization, depth resolution, and target confidence are saved. A six-kilometer intrusion is a candidate interpretation; it does not prove unlimited energy or a ready power plant.

### [350] Atomic-Force Cantilever Microscope — AfmStageModal

**Core integration:** Add a high-resolution surface-metrology observation state over ResearchSystem, MedicalSystem where biological samples are used, DutyRosterSystem, optical laser/photodiode condition, cantilever inventory, piezo scanner, sample preparation, and research unlocks.

**Smallest vertical slice:** Mount one valid sample, calibrate cantilever/photodiode response, scan a bounded surface region, and produce a height map with lateral/vertical uncertainty.

**Acceptance gate:** Tip radius, cantilever stiffness, laser alignment, piezo travel, feedback, vibration, contamination, sample damage, scan speed, and resolution are authoritative. An atomic lattice or DNA image requires a modeled resolution band and valid sample; the UI cannot label every high-zoom map “atomic.”

### [351] Adaptive Meteor-Burst TCM — AdaptiveTcmPanel

**Core integration:** Extend the Batch 21/19 meteor-burst radio state over RadioHostSession, FactionRadioEngine, ResearchSystem, modulation/coding profiles, burst detection, SNR, packet queues, and map/document transfer.

**Smallest vertical slice:** Detect one burst, choose a modulation/coding profile from measured SNR, transmit a packet set, and report data rate, error correction, loss, and retry behavior.

**Acceptance gate:** Constellation, coding rate, SNR, burst duration, oscillator drift, antenna diversity, packet size, queueing, and receiver condition are persisted. A two-second burst can carry only the modeled number of bits; a complete survey map cannot be promised without adequate payload capacity.

### [352] Sanctuary Clockwork Carillon — CarillonClockworkPanel

**Core integration:** Extend the Batch 21 bell state over GenerationalSuccessionEngine, SoundManager, EpilogueMatrix, SaveChecksum, clock/time-standard state, 24-bell tuning, gear train condition, pinned-program records, and anniversary scheduling.

**Smallest vertical slice:** Tune and install one subset of bells, program one short melody, schedule it on the simulated calendar, and save/replay the performance record.

**Acceptance gate:** Bell pitch, hammer force, gear ratio, tempo, governor, wear, maintenance, sound assets, time zone/calendar, and anniversary criteria are authoritative. Day 36,525 is valid only if the campaign calendar reaches year 100. The carillon may trigger a final presentation when all criteria pass; it cannot create victory solely from a UI button or a fixed date outside the simulation.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for brine, acids, metal billets, wire, fuel, medical supplies, food, stone, explosives, wood, nitrogen, optical parts, radio airtime, and sound/cultural materials.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, misalignment, misfire, clotting, injury, maintenance, stale-data, and treaty outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, dose_rate_uSv_h, activity_bq, concentration_bq_m3, concentration_bq_l, frequency_mhz, frequency_hz, wavelength_nm, depth_m, signal_db, bit_rate_bps, latency_ms, uncertainty_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if required, are:

- signal_pyrotechnic_defs.json
- wire_rod_processes.json
- ecmo_procedure_defs.json
- radon_sensor_defs.json
- river_engineering_defs.json
- rapid_crop_defs.json
- perimeter_belt_defs.json
- five_axis_processes.json
- acoustic_hazard_defs.json
- confocal_imaging_defs.json
- geophysical_mt_profiles.json
- afm_scan_profiles.json
- meteor_modulation_profiles.json
- carillon_program_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Clinical procedures, sensors, discoveries, materials, stations, recipes, effects, and cultural milestones must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

BariumFlarePanel, WireRodMillPanel, EcmoCannulationModal, RadonIonChamberPanel, CenturyDecanterPanel, RiverRiprapPanel, TurnipTowerPanel, TripleConcertinaPanel, UniversalMillPanel, EquatorialSundialModal, CentipedeDisruptorPanel, ConfocalMicroscopePanel, DeepMtGeophysicsPanel, AfmStageModal, AdaptiveTcmPanel, and CarillonClockworkPanel.

Each panel must bind to a typed host session/read model, expose loading/empty/error states, show units and confidence where relevant, and issue commands that return validation results. No Bind(object), placeholder arrays, direct Core mutation, hard-coded success, or unsupported 3D-only dependency is permitted. New dirty stores must be wired through Main.Setup, Main.Save, and Main.Flush using the shared atomic writer.

## Verification plan

Run focused tests after each vertical slice, then the full Godot-only acceptance set:

~~~
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --ui-layout-selftest
godot --headless --path . -- --save-slots-selftest
./scripts/ci/godot-asset-gate.sh
./scripts/ci/no-legacy-residue.sh
~~~

Add focused coverage for:

- pyrotechnic chemistry, signal recognition, storage, misfire, wire-rod mass/yield, downstream spring/fastener use, and machine wear;
- ECMO eligibility, cannulation, flow/oxygenation, circuit failure, anticoagulation, infection, recovery, death, and save round-trip;
- radon sampling/background/trend, fissure identity, evacuation recommendation, stale readings, and false earthquake-warning handling;
- solera provenance, treaty negotiation, levee/riprap capacity, flood stages, crop/root/leaf yields, broth effects, spoilage, and maintenance;
- perimeter belt geometry, blast/cutter interaction, acoustic hazard response, false confidence, worker safety, and tactical uncertainty;
- five-axis setup, geometry, balance, optical confocal resolution, AFM scan uncertainty, sample provenance, and research unlock requirements;
- sundial calibration, time drift, geophysical inversion, depth uncertainty, station noise, and follow-up confirmation;
- adaptive modulation, SNR, coding, meteor burst payload, packet error, latency, retries, and radio save state;
- carillon tuning, gear condition, program scheduling, audio playback, anniversary criteria, replay, and campaign-save isolation;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 337–352 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Chemical, clinical, industrial, agricultural, defense, optical, geophysical, radio, and cultural claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
