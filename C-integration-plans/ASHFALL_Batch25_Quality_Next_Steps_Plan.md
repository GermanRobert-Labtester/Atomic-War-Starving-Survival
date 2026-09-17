# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 25)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 385–400

## Outcome

Batch 25 should connect environmental remediation, heavy armor production, clinical hematology, intake filtration, fermentation, civil fortification, crops, geophysics, precision optics, radio, and century-scale cultural construction.

The roadmap descriptions are goals, not pre-existing system guarantees. Each step must become a Core-owned vertical slice with explicit resources, measurements, failure modes, deterministic outcomes, save/load state, and a typed Godot host contract.

Reuse the active authorities:

- **SilentFoundrySystem** and **CraftingSystem** own rolling, forging, laser/optical components, machining, material reservation, condition, and output settlement.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** own clinical status and bounded survivor effects.
- **WeatherSystem**, **RadiationSystem**, and the shared air-quality/hydrology contract own environmental and radiological measurements.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and the meal/effect pipeline own crop, fermentation, nutrition, and storage behavior.
- **DutyRosterSystem**, CaregivingSystem, **LocationLayoutSystem**, and the established supervised-work contracts own civil works and safe labor.
- **TacticalCombatSystem** and **WarlordDoctrineSystem** own defenses, traps, attacks, and uncertainty.
- **ResearchSystem**, **JournalSystem**, **FactionRadioEngine**, and RadioHostSession own scientific observations, authored knowledge, and communications.
- **GenerationalSuccessionEngine**, existing archive/census state, **EpilogueMatrix**, and **SaveChecksum** own long-lived history and finale presentation.

District8Accords, PowerGridPanel, ShelterHazardLoop, and CenturySeed remain names for unlocks or read models unless an active typed Core authority is confirmed.

## Batch entry gates

1. **Bioremediation and radiological accounting:** define contaminant isotope, soil compartment, fungal uptake, biomass handling, worker exposure, transport, measurement uncertainty, and residual contamination. Do not label soil safe because a visual bar changed.
2. **Armor and heavy rolling:** carry forward 4-high/continuous mill process contracts with alloy composition, thickness, flatness, heat treatment, ballistic rating, machine limits, scrap, and inspection.
3. **Hematopoietic clinical contract:** define marrow failure, donor matching, collection, conditioning, infusion, engraftment, infection, rejection, transfusion, and follow-up.
4. **Air intake and filtration:** unify cyclone, HEPA, dust, radiation, pressure drop, hopper maintenance, damper, and shelter air-quality state.
5. **Food and alcohol:** define honey/water/yeast inputs, ABV, contamination, storage, age, trade, dietary effects, and safe serving.
6. **Fortification and worksite safety:** reuse prisoner/parole and civil-construction contracts. No coercive labor, automatic loyalty, or “never breach” state.
7. **Crop and nutrition state:** represent sunchoke mass, inulin/prebiotic claims, storage, meals, and bounded health effects.
8. **Geophysics and optical precision:** extend the shared survey and metrology models from Batches 19–22 with 3D/voxel read models rendered as 2D Godot overlays.
9. **Microscopy and medical research:** separate observation from diagnosis, treatment, and research unlocks.
10. **Radio and monumental history:** extend STBC, archive provenance, contributor records, construction criteria, sound/cinematic replay, and century-clock gating.

## Delivery order

1. **385, 386, 393:** establish remediation, armor rolling, and precision swaging dependencies.
2. **387, 388:** add clinical marrow rescue and air-intake protection.
3. **389, 391:** add mead and sunchoke food systems.
4. **390, 392, 395:** extend civil fortification, anti-vehicle hazards, and shaft safety.
5. **394, 396, 398:** extend astronomy, laser, and retinal imaging.
6. **397:** add controlled-source 3D geophysics.
7. **399:** extend meteor-burst radio diversity.
8. **400:** complete the century colonnade and victory presentation.

## Step integration matrix

### [385] Radiotrophic Mycoremediation — MycoremediationPanel

**Core integration:** Add a remediation-bed state over GreenhouseSystem or a dedicated environmental remediation authority, RadiationSystem, soil compartments, biomass inventory, outdoor locations, worker exposure, and land-use state. District8Accords may unlock inoculum or research.

**Smallest vertical slice:** Inoculate one measured soil bed, advance one biomass cycle, assay soil and harvested biomass, and mark one bounded land-use change.

**Acceptance gate:** Soil mass, isotope inventory, uptake/immobilization, biomass yield, sampling uncertainty, contamination spread, harvest disposal, worker dose, moisture, temperature, and recontamination are persisted. A 90% decline is a configured assay result; it does not make a 20-hectare field safe or remove all isotope risk without repeated measurements.

### [386] Four-High Armor Plate Reversing Mill — ArmorPlateMillPanel

**Core integration:** Extend SilentFoundrySystem with heavy 4-high reversing rolling over the Batch 19/20 sheet and wire process contracts. Connect steel chemistry, roll force, backup-roll capacity, hot/cold state, leveling, heat treatment, plate inspection, MaterialShieldingSystem, and vehicle/door consumers.

**Smallest vertical slice:** Reserve one compatible slab, reverse-roll one plate, inspect thickness/flatness/quality, and add it to one valid armor or blast-door assembly.

**Acceptance gate:** Slab mass, alloy, thickness reduction, roll force, temperature, pass schedule, flatness, quench/temper, defects, machine vibration, and scrap are authoritative. Ballistic protection is a catalogued rating against defined threats. “Impervious” and direct-artillery protection are not default outputs.

### [387] Bone-Marrow Stem-Cell Transplant — StemCellTransplantModal

**Core integration:** Add a high-risk hematology procedure state over MedicalSystem, CombatTraumaSystem where injury coexists, SurvivorNeedsState, RadiationSystem dose history, DiseaseSystem infection, donor/recipient identity, blood products, and the canonical clinical outcome pipeline.

**Smallest vertical slice:** Diagnose one marrow-failure case, check donor compatibility, collect and process a marrow product, infuse it, and advance engraftment or complication state.

**Acceptance gate:** Dose, marrow cellularity, donor match, collection yield, conditioning, sterility, transfusion, infection, graft failure/rejection, engraftment time, blood counts, and follow-up are saved. Successful engraftment is a probabilistic clinical outcome; it cannot guarantee a cure for all acute radiation injury.

### [388] Cyclone Fallout Pre-Separator — CyclonePreSeparatorPanel

**Core integration:** Add a pre-separator state over WeatherSystem, RadiationSystem, shelter air intake, pressure/airflow, HEPA filter condition, hopper inventory, power, and damper controls.

**Smallest vertical slice:** Run one intake cycle during an ash event, remove a measured coarse-particle fraction, update hopper/filter condition, and issue a maintenance or intake recommendation.

**Acceptance gate:** Airflow, pressure drop, particle-size distribution, cyclone geometry, fan power, hopper capacity, moisture, detector calibration, filter loading, bypass leakage, and fallout activity are persisted. A 95% coarse removal rate is band-specific; the cyclone cannot make HEPA filters completely clean or remove fine aerosols without modeled efficiency.

### [389] Century Honey Meadery — CenturyMeaderyPanel

**Core integration:** Extend RecipeCatalog/data, GoodsCatalog, CraftingSystem, NeedsSystem, storage, MarketSystem, and GenerationalSuccessionEngine with apiary harvests, fermentation, cask age, and beverage provenance.

**Smallest vertical slice:** Collect one honey batch, ferment it with valid water/yeast, measure ABV, store it in one cask, and serve or trade one batch.

**Acceptance gate:** Honey mass, water, yeast, nutrients, fermentation temperature, ABV, contamination, barrel condition, age, serving, alcohol effects, and trade value are authoritative. A morale effect is bounded and cohort-specific; ten barrels and +25 morale require actual inputs and dining participation.

### [390] Ashlar Gatehouse Fortification — GatehouseFortressPanel

**Core integration:** Add a civil-fortification state over DutyRosterSystem, CaregivingSystem, LocationLayoutSystem, supervised work/parole, stone/iron inventory, portcullis condition, TacticalCombatSystem, and WarlordDoctrineSystem.

**Smallest vertical slice:** Survey one surface entrance, construct one gatehouse segment, install a working portcullis, and resolve one breach attempt against its structural/tactical state.

**Acceptance gate:** Stone mass, block quality, mortar, labor safety, guard/care capacity, winch condition, firing arcs, access control, weather, repair, and attack doctrine are saved. A gatehouse reduces breach probability and changes tactical access; it cannot prevent every assault or create murder-hole damage without a defined combat interaction.

### [391] Vertical Sunchoke Towers — SunchokeTowerPanel

**Core integration:** Extend GreenhouseSystem with sunchoke crop, aeroponic column, tuber storage, inulin/prebiotic nutrition data, recipe inputs, GoodsCatalog, and NeedsSystem effects.

**Smallest vertical slice:** Grow one tower cycle, harvest and store a measured tuber batch, cook one dish, and apply a bounded meal/nutrition effect.

**Acceptance gate:** Water, nutrients, light, temperature, column capacity, tuber mass, harvest loss, storage humidity, spoilage, cooking, and survivor dietary state are authoritative. Digestive effects require a named status/effect with duration and contraindications; 40 kilograms cannot appear without crop capacity.

### [392] Anti-Tank Ditches and Timber-Fall Traps — AntiTankDitchPanel

**Core integration:** Extend LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, terrain modification, concrete/earthwork inventory, vehicle mobility, trigger state, and salvage/expedition outcomes.

**Smallest vertical slice:** Excavate one ditch, install one revetment and trigger, resolve one vehicle approach, and produce a mobility/capture/salvage report.

**Acceptance gate:** Ditch geometry, soil, depth, wall condition, camouflage, vehicle mass, speed, reconnaissance, trigger reliability, rain, maintenance, crew safety, and recovery equipment are saved. A tank may be immobilized or damaged; capture requires a separate tactical and engineering outcome, not a guaranteed report.

### [393] Four-Die Rotary Swaging — RotarySwagingPanel

**Core integration:** Extend SilentFoundrySystem and ResearchSystem with radial forging/swaging, die geometry, feed, workpiece material, barrel/rod/shaft profiles, heat/cold state, inspection, fatigue, and weapon/vehicle consumers.

**Smallest vertical slice:** Swage one valid rod or tube, measure diameter/profile and surface condition, and add the component to one compatible recipe.

**Acceptance gate:** Die alignment, RPM, feed, reduction, temperature, material hardness, residual stress, tool wear, lubrication, profile deviation, cracks, and scrap are authoritative. A barrel or driveshaft receives measured quality and fatigue rating; “zero machining waste,” match grade, and double fatigue life are configured outcomes only.

### [394] Armillary Precession Dial — PrecessionDialModal

**Core integration:** Extend JournalSystem, ExpeditionSystem, CraftingSystem, SimClock, celestial epoch data, planisphere/sundial state, and chart calibration.

**Smallest vertical slice:** Engrave one epoch-corrected dial, calibrate a star chart for one route/date, and apply a bounded long-term navigation correction.

**Acceptance gate:** Epoch, latitude, date, precession model, chart quality, observer skill, sky visibility, time drift, and instrument condition are persisted. The dial compensates for configured celestial drift; it cannot guarantee 100% precision across all future expeditions or poor visibility.

### [395] Trapdoor-Spider Acoustic Disruptors — TrapdoorSpiderDisruptorPanel

**Core integration:** Extend the Batch 21/22 acoustic hazard state over LocationLayoutSystem, lower air-shaft safety, ShelterHazardLoop if confirmed, power, transducer condition, tunnel geometry, and creature encounters.

**Smallest vertical slice:** Install one shaft-sector emitter, run a frequency sweep, detect one burrow/hazard observation, and resolve diversion, collapse risk, or inspection.

**Acceptance gate:** Burrow location, soil, tunnel stability, frequency response, attenuation, worker exposure, power, maintenance, false confidence, and fallback access are saved. A device can reduce encounters or alter a route; it cannot collapse every burrow or guarantee zero ambush risk.

### [396] Helium-Neon Laser Tube — HeNeLaserPanel

**Core integration:** Extend the Batch 21/22 optical manufacturing state over SilentFoundrySystem, ResearchSystem, glass inventory, gas handling, Brewster windows, discharge power, cavity alignment, thermal condition, and interferometer/gyro consumers.

**Smallest vertical slice:** Fabricate one tube, evacuate/backfill it with a measured gas mixture, align the cavity, and produce a calibrated wavelength/power record.

**Acceptance gate:** Tube geometry, glass quality, vacuum, gas ratio/pressure, electrode condition, discharge current, cooling, window angle, mirror alignment, wavelength, mode stability, and safety are authoritative. A laser is usable by downstream optics only when output and alignment meet its catalogued specification.

### [397] Controlled-Source AMT 3D Survey — CsamtGeophysicsPanel

**Core integration:** Extend the shared geophysical survey state over LocationLayoutSystem, ResearchSystem, transmitter/receiver equipment, grounded dipoles, 3D station geometry, power, frequency, inversion, and mineral discovery records.

**Smallest vertical slice:** Configure one fictional survey grid, transmit a bounded source waveform, collect receiver channels, run a deterministic 3D inversion, and mark one anomaly with confidence.

**Acceptance gate:** Electrode contact, source current, frequency, geometry, attenuation, noise, station condition, inversion regularization, voxel resolution, depth uncertainty, survey time, and follow-up confirmation are persisted. A gold-copper body is a candidate resource until drilled or otherwise confirmed; 3D coloring is not proof.

### [398] Retinal Confocal Microvascular Angiography — RetinalAngiographyModal

**Core integration:** Add a non-invasive observation/procedure record over ResearchSystem, MedicalSystem, DutyRosterSystem, radiation dose history, optical device condition, sample/patient identity, vascular status, and treatment recommendation.

**Smallest vertical slice:** Image one eligible retinal region, estimate vessel flow/leakage with confidence, and create a follow-up or treatment review record.

**Acceptance gate:** Scan wavelength/power, alignment, motion, image quality, vessel state, dose, operator skill, artifact, and interpretation uncertainty are saved. Detection is not diagnosis; a vasoprotective treatment requires a catalogued condition, evidence threshold, and clinical decision.

### [399] Meteor-Burst STBC Diversity — StbcMeteorPanel

**Core integration:** Extend RadioHostSession/FactionRadioEngine and the Batch 21/22 meteor-burst state with Alamouti encoding, dual transmitters, synchronization, fading, coding, packet queues, retries, and critical-warning delivery.

**Smallest vertical slice:** Encode one warning packet, transmit it over one burst with two antennas, decode it at a known station, and record delivery confidence, loss, and retry.

**Acceptance gate:** Antenna geometry, power, timing, channel coherence, burst duration, fading, SNR, oscillator drift, coding overhead, packet size, and receiver condition are authoritative. STBC reduces fading risk but cannot guarantee zero packet loss during a one-second trail.

### [400] Golden Colonnade — GoldenColonnadePanel

**Core integration:** Add a monument construction/milestone state over GenerationalSuccessionEngine, JournalSystem, EpilogueMatrix, SaveChecksum, materials, shelter/outdoor layout, lighting/audio, and century criteria.

**Smallest vertical slice:** Construct one colonnade segment, record its material/provenance/contributors, display it in a 2D Godot exterior scene, and unlock one historical milestone entry.

**Acceptance gate:** Marble/gold mass, quarry/transport, structural capacity, weather, maintenance, accessibility, construction jobs, campaign year, lineage/history, and milestone prerequisites are saved. The 100-year ending triggers only after actual criteria pass; the monument cannot manufacture victory or require an unsupported 3D panorama runtime.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for soil, biomass, radioactive material, steel, alloying elements, gas, glass, reagents, food, alcohol, stone, concrete, wood, medical supplies, power, optical components, radio airtime, and monument materials.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, misalignment, misfire, rejection, injury, maintenance, stale-data, and treaty outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, concentration_bq_kg, concentration_bq_m3, concentration_bq_l, dose_rate_uSv_h, activity_bq, thickness_mm, tensile_mpa, fatigue_cycles, wavelength_nm, uncertainty_nm, signal_db, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- remediation_bed_defs.json
- heavy_armor_processes.json
- marrow_transplant_defs.json
- air_intake_separator_defs.json
- beverage_fermentation_defs.json
- civil_gatehouse_defs.json
- root_crop_defs.json
- anti_vehicle_defense_defs.json
- rotary_swaging_profiles.json
- celestial_epoch_defs.json
- acoustic_burrow_defs.json
- gas_laser_defs.json
- controlled_source_geophysics_defs.json
- retinal_observation_defs.json
- meteor_diversity_profiles.json
- monument_milestone_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Isotopes, clinical statuses, crops, materials, sensors, discoveries, instruments, effects, and monuments must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

MycoremediationPanel, ArmorPlateMillPanel, StemCellTransplantModal, CyclonePreSeparatorPanel, CenturyMeaderyPanel, GatehouseFortressPanel, SunchokeTowerPanel, AntiTankDitchPanel, RotarySwagingPanel, PrecessionDialModal, TrapdoorSpiderDisruptorPanel, HeNeLaserPanel, CsamtGeophysicsPanel, RetinalAngiographyModal, StbcMeteorPanel, and GoldenColonnadePanel.

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

- isotope uptake, soil sampling, biomass handling, armor rolling, plate quality, ballistic rating, and material conservation;
- marrow diagnosis, donor match, collection, engraftment, infection, rejection, transfusion, and save round-trip;
- cyclone particle efficiency, pressure drop, hopper/filter loading, fine dust, damper behavior, and stale sensor data;
- fermentation, ABV, cask aging, crop yield/storage, nutrition, meal effects, and alcohol safety;
- gatehouse/ditch geometry, labor safety, vehicle mobility, capture/salvage, shaft hazards, acoustic false positives, and maintenance;
- swaging profile/fatigue, precession correction, laser output/alignment, downstream metrology, and optical uncertainty;
- CSAMT station geometry, inversion, voxel confidence, deep target confirmation, retinal interpretation, treatment gating, and provenance;
- STBC encoding, fading, synchronization, packet capacity, loss, retries, latency, and radio save state;
- colonnade construction, milestone criteria, contributor records, 2D presentation, replay, checksum, and campaign-save isolation;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 385–400 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Remediation, medical, industrial, agricultural, defense, optical, geophysical, radio, and cultural claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
