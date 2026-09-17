# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 16)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 241–256

## Outcome

Batch 16 should build on the process, medical, sensor, radio, agriculture, prisoner, optical, and survey contracts established by earlier batches. It should prioritize safety-critical chemistry, high-pressure manufacturing, clinical emergency procedures, atmospheric monitoring, perimeter defense, and long-lived demographic evidence.

The supplied “done when” statements are not pre-existing guarantees. A result such as 20 liters of bleach, zero infestation, perfect neurosurgical recovery, 20x optics, or a 150-person six-generation colony is accepted only when the simulation has explicit inputs, limits, uncertainty, state transitions, and tests.

Active authorities to reuse:

- **SilentFoundrySystem** and **CraftingSystem** for industrial jobs and exact resource settlement.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** for clinical outcomes and survivor effects.
- **WeatherSystem**, **RadiationSystem**, and the shared sensor/air-quality contract for environmental telemetry.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and the meal/effect pipeline for crops and tonics.
- **DutyRosterSystem**, CaregivingSystem, and the Batch 15 prisoner lifecycle for parole work.
- **LocationLayoutSystem**, **ExpeditionSystem**, **WarlordDoctrineSystem**, and TacticalCombatSystem for perimeter and expedition hazards.
- **FactionRadioEngine**, RadioHostSession, ResearchSystem, and LocationLayoutSystem for ionospheric and long-range communications.
- **GenerationalSuccessionEngine**, existing census state, JournalSystem, and SaveChecksum for historical demographic records.

There is no confirmed single PowerGridPanel authority, atmospheric intake authority, optical assembly authority, vehicle drivetrain authority, or challenge-independent demographic archive. Use typed Core contracts and host adapters; do not use a UI name as a gameplay owner.

## Batch entry gates

1. **Chemical safety gate:** define brine purity, cell chemistry, gas separation, caustic handling, ventilation, scrubbers, water-treatment recipes, and exposure hazards before implementing bleach.
2. **High-pressure process gate:** carry forward alloy, pipe, furnace, steam, and mechanical-condition contracts. Pressure-bearing output requires pressure rating and inspection, not merely an item ID.
3. **Emergency clinical gate:** define canonical states for intracranial pressure, hematoma, trauma severity, anesthesia, procedure risk, infection, death, recovery, and follow-up. UI cannot directly rescue or kill a survivor.
4. **Air-quality gate:** define particulate density, radiation-bearing dust, filtration/intake state, damper control, sensor confidence, and safe/unsafe thresholds.
5. **Agriculture and disease gate:** define nutrition and disease-risk effects, contamination, livestock/food spoilage, treatment evidence, and bounded effect duration. Avoid unsupported “immunity” guarantees.
6. **Perimeter-defense gate:** reuse Batch 15 trap/sensor placement and detection contracts for razor wire, flares, and pest sensors. Do not create separate grid, trigger, or detection semantics.
7. **Optics and vehicle gate:** extend the Batch 15 precision optics and gear contracts; create a drivetrain/vehicle authority only if no existing expedition transport owner can represent it.
8. **Radio propagation gate:** define ionospheric observations, frequency selection, weather/noise, station geometry, and link confidence. Avoid treating a selected frequency as guaranteed clear communication.
9. **Demographic evidence gate:** define event-sourced birth, death, cause, surname, household, migration, age, and population snapshots before building the census charts.

## Delivery order

1. **241, 242:** establish chemical and pressure-bearing industrial infrastructure.
2. **243, 244:** add emergency clinical and air-quality safety behavior.
3. **245, 247:** add food, disease-risk, and crop effects.
4. **246, 248:** extend the Batch 15 governance and perimeter-defense contracts.
5. **249, 252:** extend precision gearing and optical assembly.
6. **250, 253:** add navigation timing and hydrogeophysical survey artifacts.
7. **251, 254:** add pest-control and medical training records.
8. **255:** connect ionospheric measurement to the existing radio model.
9. **256:** finish with a historical census read model and charting UI.

## Step integration matrix

### [241] Geothermal Brine Electrolysis and Chlorine Bleach — ChlorineBleachPanel

**Core integration:** Add an electrochemical production state over brine/water data, CraftingSystem, the shared power contract, MedicalSystem, DiseaseSystem, air handling, storage, and hazardous-material rules. District8Accords may unlock access or recipe knowledge, but it must not silently own chemical simulation.

**Smallest vertical slice:** Load a measured brine batch, run one cell cycle, separate chlorine/caustic outputs through a validated scrubber path, and produce one catalogued disinfectant batch.

**Acceptance gate:** Brine purity, current, voltage, cell efficiency, membrane condition, gas separation, ventilation, exposure, caustic concentration, storage, and water-treatment dosage are persisted. The UI cannot claim 20 liters of medical bleach without valid inputs and a safe process. Chlorine release must have a modeled failure path and must not be a cosmetic effect.

### [242] Dual-Axis Centrifugal Pipe Casting — CentrifugalSpinPanel

**Core integration:** Extend SilentFoundrySystem with dual-axis casting and pressure-rated pipe definitions. Connect mold geometry, alloy composition, spin speed, cooling, machining, inspection, ShelterOperationsPanel infrastructure, and the shared pressure/utility contract.

**Smallest vertical slice:** Cast one flanged pipe from valid metal stock, inspect it, assign a pressure rating, and install it in one compatible utility segment.

**Acceptance gate:** Rotation, pour temperature, cooling, porosity, wall thickness, flange geometry, material mass, machine wear, and inspection failure are authoritative. A failed pipe cannot be installed as high-pressure infrastructure. “Five heavy pipes” is an output target based on stock and job capacity, not an unconditional result.

### [243] Emergency Skull Trepanation — TrepanationModal

**Core integration:** Add a high-risk cranial procedure state over MedicalSystem, CombatTraumaSystem, DiseaseSystem, SurvivorNeedsState, clinical inventory, clinician skill, and the canonical death/recovery pipeline.

**Smallest vertical slice:** Triage one eligible head-trauma case, reserve drill/sterile/anesthesia supplies, resolve decompression risk, and advance the survivor through immediate recovery or deterioration.

**Acceptance gate:** Intracranial pressure, hematoma size/location, trauma severity, time-to-treatment, clinician skill, sterility, anesthesia, equipment condition, complications, and follow-up are persisted. “Saving a comatose scout” is one possible outcome, not a guaranteed modal result. The UI must not write health or consciousness directly.

### [244] Laser Nephelometer and Dust Sizing — NephelometerPanel

**Core integration:** Add particulate telemetry over WeatherSystem, RadiationSystem, air filtration/intake state, surface sensor condition, and the shared alert/automation contract.

**Smallest vertical slice:** Sample one ash/fallout condition, estimate PM bands and radioactive-dust contribution with confidence, and issue a validated intake-damper recommendation.

**Acceptance gate:** Particle concentration, calibration, laser condition, humidity, weather, sample interval, radiation correlation, filtration capacity, damper travel time, and sensor uncertainty are modeled. Automatic shutoff requires threshold configuration, power, actuator condition, and an event audit. Detection does not guarantee that all toxic dust is excluded.

### [245] Cider Vinegar Fire Tonic — FireTonicPanel

**Core integration:** Extend RecipeCatalog/data, GoodsCatalog, CraftingSystem, DiseaseSystem, NeedsSystem, and meal/medical effect handling. Reuse the cider fermentation contract from Batch 15.

**Smallest vertical slice:** Ferment one qualified vinegar mother, combine one recipe batch, distribute it to one survivor cohort, and record a bounded disease-risk or nutrition effect.

**Acceptance gate:** Fermentation condition, ingredient quality, dosage, contamination, storage, contraindications, adherence, disease exposure, and effect duration are persisted. “Cuts winter illness by 60%” is a measured balance target for a defined exposure cohort, not an absolute immunity buff. No direct colony-wide disease mutation from the panel.

### [246] Prisoner Parole Labor and Agricultural Work Gangs — ParoleGangPanel

**Core integration:** Extend the Batch 15 prisoner lifecycle through DutyRosterSystem, CaregivingSystem, SurvivorNeedsState, faction relations, surface work sites, guards, and crop/scrap production.

**Smallest vertical slice:** Move one eligible prisoner to a supervised parole assignment, consume guard and supply capacity, complete one work cycle, and advance the review record.

**Acceptance gate:** Eligibility, risk level, guard ratio, route/weather exposure, food/medical needs, work safety, productivity, compliance, incident history, credits, and citizenship review are saved. A 100-kilogram harvest and three citizenship outcomes require actual worksite capacity and review evidence; no automatic conversion is permitted.

### [247] Vertical Bell Pepper Columns — PepperTowerPanel

**Core integration:** Extend GreenhouseSystem, crop/recipe catalogs, GoodsCatalog, NeedsSystem nutrition effects, water/nutrient/light demand, and spice processing.

**Smallest vertical slice:** Plant one column, advance one ripening cycle, harvest peppers, split fresh and dried outputs, and use one output in a meal.

**Acceptance gate:** Column capacity, nutrient solution, water, light/power, temperature, pest/contamination, ripeness, yield, drying loss, and storage are authoritative. Morale effects come through the existing meal/effect path and have named duration/stacking rules. No fixed 15-kilogram harvest without a valid crop state.

### [248] Concertina Razor Wire and Flare Lines — ConcertinaWirePanel

**Core integration:** Extend the Batch 15 perimeter-trap and sensor model over LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, sentry assignments, flares, ammunition/consumables, and battlefield visibility.

**Smallest vertical slice:** Place one wire segment with one trip flare, detect a crossing, resolve illumination/entanglement, and create a tactical response window.

**Acceptance gate:** Terrain, placement time, material length, trip sensitivity, flare duration, visibility, weather, maintenance, friendly access, injury risk, and enemy countermeasures are modeled. The effect exposes a threat and changes tactical state; it cannot guarantee that snipers hit or that an entire war band is stopped.

### [249] Helical Spiral Bevel Gears and Differentials — HelicalGearPanel

**Core integration:** Extend SilentFoundrySystem, CraftingSystem, ResearchSystem, gear hobbing, vehicle/drivetrain state, and ExpeditionSystem transport if that is the active consumer.

**Smallest vertical slice:** Machine one differential from valid blanks, inspect backlash and ratio, install it into one compatible vehicle, and run one terrain travel test.

**Acceptance gate:** Gear material, cutter geometry, heat treatment, tolerance, backlash, lubrication, torque rating, condition, and installation compatibility are persisted. A vehicle speed modifier is derived from terrain, load, engine, tires, and differential condition. The +40% figure is a tuned result, not a direct permanent bonus.

### [250] Copperplate Moon-Dial — MoonDialModal

**Core integration:** Add a navigation artifact over JournalSystem, ExpeditionSystem, CraftingSystem, SimClock, weather/visibility, and the route-planning contract.

**Smallest vertical slice:** Produce one calibrated moon-dial for a defined latitude/season band, plan one night departure, and apply a bounded concealment or timing modifier.

**Acceptance gate:** Calibration date, location, moon phase, cloud/fallout visibility, route, scout skill, timekeeping drift, and equipment condition matter. A moon-dial can improve timing confidence; it cannot guarantee a 30% stealth increase or perfect synchronization across all locations.

### [251] Ultrasonic Insect Pest Barrier — InsectAcousticPanel

**Core integration:** Add a pest-control state over GreenhouseSystem, pantry/grain inventory, DiseaseSystem or spoilage authority, shared power, acoustic coverage, and maintenance.

**Smallest vertical slice:** Install one transducer sector, configure a frequency band, run one storage cycle, and record pest pressure, grain loss, and power use.

**Acceptance gate:** Pest species, frequency response, room geometry, seals, acoustic attenuation, device condition, power, baseline infestation, and non-target effects are modeled. A full-year zero-infestation result is a configured test case, not a universal promise; conventional inspection and sanitation remain available.

### [252] Achromatic Doublet Lens Cementing — LensCementingPanel

**Core integration:** Extend Batch 15 optical manufacturing and assembly over SilentFoundrySystem, ResearchSystem, glass/resin inventory, optical alignment, device condition, and spotter-scope catalog entries.

**Smallest vertical slice:** Select two compatible lens elements, heat and apply measured cement, center and cure the doublet, then inspect chromatic and alignment quality.

**Acceptance gate:** Glass pairing, resin condition, temperature, bubble contamination, centering, cure time, and inspection determine output quality. A 20x telescope is produced only from a valid optical specification and does not guarantee visibility through fallout, weather, or terrain.

### [253] Self-Potential Groundwater Profiling — SelfPotentialPanel

**Core integration:** Add a passive geophysical survey state over LocationLayoutSystem, ResearchSystem, ExpeditionSystem equipment, electrode placement, survey grid, and resource discovery catalogs.

**Smallest vertical slice:** Place two or more valid electrodes, collect one traverse profile, estimate a water-bearing anomaly with confidence, and mark a follow-up target.

**Acceptance gate:** Electrode calibration, ground contact, natural voltage, soil chemistry, weather, spacing, noise, operator skill, and survey duration are persisted. A target is probabilistic evidence and requires drilling or confirmation before becoming a usable water source.

### [254] Anatomical Wax Moulage Training — WaxMoulageModal

**Core integration:** Add a training-record state over MedicalSystem, DutyRosterSystem, JournalSystem, skill progression, wax/pigment materials, and the authored case catalog.

**Smallest vertical slice:** Build or study one moulage case, record the trainee’s completion and assessment, and apply one canonical medical knowledge increment.

**Acceptance gate:** Case complexity, training time, instructor/material quality, trainee fatigue, assessment, retention, and skill prerequisites are saved. XP flows through the canonical progression system; +25 XP and live-operation readiness are not direct UI promises.

### [255] Ionospheric Sounder — IonosondePanel

**Core integration:** Extend the Batch 15 radio propagation model over RadioHostSession, FactionRadioEngine, ResearchSystem, LocationLayoutSystem, antenna condition, frequency sweep, and time/weather state.

**Smallest vertical slice:** Run one sweep, record an ionogram-like observation with critical frequency and confidence, select one candidate channel, and test one transmission.

**Acceptance gate:** Sweep range, transmitter power, ionospheric state, time of day, season, storm/solar-noise state, antenna condition, and station geometry affect the result. A 14.2 MHz selection is a scenario result; the link must still pass signal-to-noise and delivery checks.

### [256] 100-Year Colony Census — ColonyCensusPanel

**Core integration:** Extend GenerationalSuccessionEngine, existing census state, survivor roster/identity records, JournalSystem, SaveChecksum, and a versioned demographic read model.

**Smallest vertical slice:** Record birth, death, cause, age, surname, household/lineage, migration, and population snapshot events for one campaign interval, then render a population pyramid and mortality breakdown.

**Acceptance gate:** Charts derive from event-backed records with explicit missing-data handling, not invented totals. Population, lifespan, mortality, surname, generation, and migration metrics survive save/load and migrations. A 150-person, six-generation result is a campaign outcome and must never be displayed as a hard-coded victory condition.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact resource reservation and settlement for brine, water, electricity, fuel, metals, glass, resin, food, medical supplies, flares, wire, and survey equipment.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, misfire, injury, and maintenance outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, concentration_mg_m3, current_a, voltage_v, signal_db, frequency_mhz, confidence_fraction, condition_fraction, contamination_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new data surfaces, only if needed, are:

- electrochemical_processes.json
- pressure_pipe_specs.json
- clinical_procedures.json
- air_quality_sensor_defs.json
- food_fermentation_defs.json
- crop_columns.json
- perimeter_defense_defs.json
- vehicle_drivetrain_defs.json
- optical_assembly_defs.json
- radio_propagation_profiles.json
- demographic_schema.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Chemical, medical, sensor, and demographic claims must be data-driven. Narrative files may explain a result but cannot create it.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

ChlorineBleachPanel, CentrifugalSpinPanel, TrepanationModal, NephelometerPanel, FireTonicPanel, ParoleGangPanel, PepperTowerPanel, ConcertinaWirePanel, HelicalGearPanel, MoonDialModal, InsectAcousticPanel, LensCementingPanel, SelfPotentialPanel, WaxMoulageModal, IonosondePanel, and ColonyCensusPanel.

Each panel must bind to a typed host session/read model, expose loading/empty/error states, show units and confidence where relevant, and issue commands that return validation results. No Bind(object), placeholder arrays, direct Core mutation, or hard-coded success outcome is permitted. New dirty stores must be wired through Main.Setup, Main.Save, and Main.Flush using the shared atomic writer.

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

- brine purity, electrolysis balance, chlorine containment, caustic storage, dosage, and failure;
- pressure-rated pipes, casting porosity, inspection, material conservation, and utility installation;
- cranial triage, time-to-treatment, procedure risk, complications, death/recovery, and save round-trip;
- particulate sensor calibration, radiation correlation, intake automation, filter capacity, stale readings, and failure;
- tonic dosage, fermentation/contamination, disease exposure, adherence, contraindications, and bounded effects;
- parole safety, guard capacity, surface work, productivity, incidents, review, and citizenship evidence;
- crops, pest pressure, yields, food preservation, nutrient/meal integration, and power use;
- traps, flares, detection confidence, friendly access, tactical visibility, and countermeasures;
- drivetrain and optical manufacturing tolerances, device condition, terrain/weather effects, and consumer modifiers;
- survey uncertainty, follow-up confirmation, radio propagation, ionogram selection, message delivery, and time/weather variation;
- demographic event ordering, missing data, lineage, migration, mortality causes, chart aggregation, migrations, and checksum integrity;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 241–256 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Chemical, clinical, demographic, sensor, and resource claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
