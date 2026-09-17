# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 21)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 321–336

## Outcome

Batch 21 should extend the Batch 19/20 chemistry, industrial process, medical emergency, environmental monitoring, geophysics, optical navigation, radio, perimeter, and archive contracts. It is not a set of isolated panels. Each feature must have one Core owner, deterministic inputs and outputs, persisted state, typed host commands, and explicit uncertainty or failure behavior.

Reuse the active authorities:

- **SilentFoundrySystem** and **CraftingSystem** own metallurgy, pyrotechnic production, machining, resource reservation, machine condition, and output settlement.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** own clinical state and bounded survivor effects.
- **WeatherSystem**, **RadiationSystem**, and the shared sensor/hydrology contract own environmental and radiological observations.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and the meal/effect pipeline own crops, food, and consumables.
- **LocationLayoutSystem**, **ExpeditionSystem**, **TacticalCombatSystem**, and **WarlordDoctrineSystem** own route, perimeter, tactical, and hostile behavior.
- **FactionRadioEngine**, RadioHostSession, **ResearchSystem**, and **JournalSystem** own communication, discovery, and authored records.
- **GenerationalSuccessionEngine**, existing census/archive state, SoundManager, EpilogueMatrix, and SaveChecksum own long-lived culture and milestone presentation.

District8Accords, PowerGridPanel, ShelterHazardLoop, and CenturySeed remain unlock or UI names unless a typed active contract is confirmed. Do not create parallel authorities to satisfy those names.

## Batch entry gates

1. **Pyrotechnic and chemical safety:** define strontium compounds, flare payload, casing, ignition, storage, misfire, visibility, worker exposure, and disposal. Signal flares must be separate from destructive ammunition.
2. **Wire metallurgy:** carry forward hot-dip galvanizing, wire rod, spring, cable, and industrial process contracts with coating thickness, diffusion, tensile rating, fatigue, and weldability.
3. **Emergency cardiac contract:** extend pericardiocentesis and trauma state with arrest cause, time-to-intervention, open-chest risk, defibrillation, pulse return, neurological outcome, and follow-up.
4. **Radiological water contract:** unify tritium sampling with well identity, sample chain of custody, detection limit, uncertainty, activity, dose contribution, and water-treatment decisions.
5. **Civil infrastructure contract:** reuse road, culvert, drainage, construction, worksite safety, and route-maintenance state from Batches 15, 19, and 20.
6. **Crop and topical medicine contract:** define okra growth, mucilage extraction, burn-wound effects, contamination, dosage, and clinical evidence.
7. **Perimeter and pest contract:** reuse the wire, trap, acoustic, and sensor model; no “impenetrable” or “zero infestation” flags.
8. **Optical navigation contract:** extend planisphere, sundial, interferometer, and Sagnac measurement with calibration, drift, weather visibility, and confidence.
9. **Deep geophysics contract:** unify AMT with the earlier TEM, ERT, SP, and RMT survey state.
10. **Radio propagation contract:** extend sporadic-E and meteor-burst links with diversity combining, delay, bit errors, burst windows, queueing, and stale data.
11. **Century sound milestone:** define bell construction, acoustics, scheduling, contributors, recordings, and replayable epilogue state without making sound a direct global victory trigger.

## Delivery order

1. **321, 322, 329:** stabilize pyrotechnic, wire-metallurgy, and tracer-machining primitives.
2. **323, 324:** complete emergency resuscitation and radiological-water monitoring.
3. **325, 326, 327:** extend solera diplomacy, drainage infrastructure, and medicinal crops.
4. **328, 331:** reuse the perimeter and acoustic-pest contracts.
5. **330, 332:** extend analog navigation and optical inertial measurement.
6. **333, 334:** add deep geophysics and laboratory imaging.
7. **335:** extend meteor-burst communication.
8. **336:** complete the century bell and cultural milestone presentation.

## Step integration matrix

### [321] Strontium Distress Flares — StrontiumFlarePanel

**Core integration:** Add a non-combat pyrotechnic production and signal-event state over brine/reagent inputs, SilentFoundrySystem, CraftingSystem, expedition distress, LocationLayoutSystem, weather/visibility, and extraction/response events.

**Smallest vertical slice:** Process one valid strontium flare batch, equip it to one expedition, fire one flare at a defined location, and create a time-limited signal/illumination event.

**Acceptance gate:** Reagent purity, casing, payload, ignition, storage, burn duration, cloud/fallout visibility, wind, altitude, signal color, flare inventory, and responder detection are persisted. A one-kilometer radius is a configured illumination result; it cannot guarantee that an extraction team sees or reaches the squad.

### [322] Galvannealed Spring-Steel Wire — GalvannealWirePanel

**Core integration:** Extend SilentFoundrySystem with inline galvannealing over the Batch 19/20 wire and galvanizing contracts. Connect induction heating, zinc-iron diffusion, coating inspection, tensile/fatigue properties, spring/cable recipes, and vehicle or machinery consumers.

**Smallest vertical slice:** Run one wire coil through the line, measure coating and tensile quality, then draw/form one spring or cable component.

**Acceptance gate:** Wire gauge, line speed, temperature, diffusion, zinc mass, surface condition, cooling, weldability, fatigue, machine wear, and scrap are authoritative. Spring installation checks load, travel, fatigue, and vehicle compatibility. The feature cannot promise a suspension spring merely because a panel job completed.

### [323] Open-Chest Cardiac Resuscitation — CardiacResuscitationModal

**Core integration:** Extend the Batch 20 cardiac procedure state over MedicalSystem, CombatTraumaSystem, SurvivorNeedsState, clinical inventory, anesthesia, surgical staffing, defibrillator condition, and the death/recovery pipeline.

**Smallest vertical slice:** Triage one arrest case, check eligibility and time window, reserve thoracotomy/sterile/defibrillation supplies, resolve manual compression and shock, and record pulse/neurological outcome.

**Acceptance gate:** Arrest cause, no-pulse duration, injury, temperature, shock energy, compression quality, surgical skill, sterility, arrhythmia, recurrent arrest, neurological damage, and post-procedure care are saved. Open-chest resuscitation is a rare high-risk outcome; it cannot restart a survivor from any arbitrary dead state or guarantee survival.

### [324] Liquid Scintillation Tritium Counter — TritiumCounterPanel

**Core integration:** Add a radiological groundwater assay state over WeatherSystem/RadiationSystem, well and aquifer records, sample inventory, scintillation equipment, ResearchSystem, water treatment, and survivor exposure.

**Smallest vertical slice:** Collect one chain-of-custody sample, count it with a configured detection limit, report activity with uncertainty, and classify one water batch for use, treatment, or quarantine.

**Acceptance gate:** Sample volume, fluor cocktail, background, counting time, detector efficiency, quench, activity Bq/L, minimum detectable activity, well depth, dilution, uncertainty, and calibration are persisted. “Zero contamination” means below the modeled detection limit, not mathematically zero; drinking safety requires the configured dose and policy threshold.

### [325] Solera Balsamic Decanter and Wax Seal — WaxSealBottlingPanel

**Core integration:** Extend the Batch 19/20 solera state over RecipeCatalog/data, GoodsCatalog, JournalSystem, GenerationalSuccessionEngine, MarketSystem, faction relations, bottle/seal inventory, and diplomatic encounter outcomes.

**Smallest vertical slice:** Draw one valid aged batch, bottle and seal it with provenance, present it through one negotiation, and record the faction response.

**Acceptance gate:** Cask age/lineage, acidity, transfer history, bottle condition, wax, certificate, gift value, faction attitude, negotiation terms, and treaty conditions are saved. A gift can improve trust or unlock a negotiation path; an immediate peace treaty must require a defined encounter resolution and cannot be forced by prestige meters.

### [326] Highway Culverts and Drainage Sluices — CulvertDrainagePanel

**Core integration:** Extend the civil road segment state over DutyRosterSystem, CaregivingSystem, LocationLayoutSystem, WeatherSystem, aggregate/concrete inventory, flood flow, slope, inspection, and route maintenance.

**Smallest vertical slice:** Install one culvert and riprap apron on a validated road segment, simulate one storm runoff event, and update washout risk and route availability.

**Acceptance gate:** Pipe diameter, slope, inlet/outlet, capacity, debris, soil, riprap mass, rainfall intensity, radioactive sediment, maintenance, and road condition are authoritative. A culvert reduces washout probability; it cannot guarantee a road survives a 100-year flood unless the modeled event and capacity support it.

### [327] Vertical Okra and Mucilage Burn Salve — OkraTowerPanel

**Core integration:** Extend GreenhouseSystem with okra crop cycles and connect extraction to CraftingSystem, GoodsCatalog, MedicalSystem, burn-wound status, contamination, and topical-treatment effects.

**Smallest vertical slice:** Grow one tower crop, harvest pods, extract a measured mucilage batch, formulate one salve, and administer it to one eligible burn wound.

**Acceptance gate:** Crop water/light/nutrient demand, harvest mass, mucilage yield, extraction loss, contamination, salve stability, wound depth, sterility, dosage, and healing effect are persisted. A +40% acceleration is a data-defined clinical modifier requiring evidence and cannot apply to all burns or replace surgical care.

### [328] Double-Apron Anti-Cutter Fence — DoubleApronWirePanel

**Core integration:** Extend the Batch 19/20 perimeter-defence state over LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, wire/picket inventory, sapper tools, sentry response, terrain, and countermeasures.

**Smallest vertical slice:** Build one double-apron segment, resolve one sapper attempt with tool and terrain conditions, and create a breach/delay report.

**Acceptance gate:** Wire length, picket spacing, entanglement, clipper condition, defender exposure, weather, maintenance, alternate routes, cutting time, and breach probability are saved. The fence delays or exposes a breach attempt; it cannot become an impenetrable or automatic neutralization state.

### [329] Hydraulic Tracer Copying Lathe — HydraulicTracerLathePanel

**Core integration:** Extend SilentFoundrySystem and ResearchSystem with template identity, stylus hydraulics, follower accuracy, carriage control, shell/rotor profiles, material stock, tool wear, and inspection.

**Smallest vertical slice:** Mount one approved template, copy one curved component, measure profile deviation, and add the result to a compatible assembly queue.

**Acceptance gate:** Template condition, stylus pressure, hydraulic stability, feed rate, material, tool geometry, cooling, machine wear, operator setup, dimensional deviation, and scrap are authoritative. A fivefold speed multiplier and fifty casings per shift are tuning outcomes requiring machine capacity and valid blanks.

### [330] Brass Nocturnal Dial — NocturnalDialModal

**Core integration:** Add a calibrated night-time navigation artifact over JournalSystem, ExpeditionSystem, CraftingSystem, SimClock, latitude/season, star visibility, and the existing planisphere/sundial contract.

**Smallest vertical slice:** Calibrate one dial for a fictional latitude band, align it to the catalogued pole-star reference under valid visibility, and produce a local-time estimate for one expedition.

**Acceptance gate:** Date, latitude, horizon, season, cloud/fallout, star visibility, dial calibration, observer skill, and clock drift matter. A nocturnal dial can provide an uncertainty interval; it cannot promise an exact 02:15 reading or perfectly synchronize an ambush under obscured skies.

### [331] Termite Acoustic Disruptors — TermiteDisruptorPanel

**Core integration:** Extend the Batch 16/19 pest and acoustic-preservation contract over wooden foundations, library media, GreenhouseSystem where relevant, power, transducer condition, timber integrity, document condition, and inspection.

**Smallest vertical slice:** Install one emitter sector, run one frequency profile, simulate one inspection interval, and record termite pressure, timber/document damage, power, and maintenance.

**Acceptance gate:** Species, baseline infestation, wood moisture, room geometry, frequency response, attenuation, device condition, power, non-target effects, and inspection evidence are modeled. Zero damage is a scenario outcome only; sanitation, barriers, and manual inspection remain necessary.

### [332] Sagnac Ring Interferometer and Fiber Gyro — SagnacGyroPanel

**Core integration:** Extend the Batch 15/19 optical metrology state over ResearchSystem, SilentFoundrySystem, fiber spool, laser source, phase detectors, calibration, ExpeditionSystem, and vehicle/airship navigation consumers.

**Smallest vertical slice:** Wind one valid fiber ring, calibrate counter-propagating phase response, measure drift over time, and apply the result to one expedition navigation leg.

**Acceptance gate:** Fiber length, loop area, laser stability, temperature, vibration, polarization, phase noise, calibration, drift, power, and device condition are saved. A 0.01 degree/hour rating is a measured configuration result. The gyro improves heading confidence; it does not create 100% precision through every storm or remove route/weather uncertainty.

### [333] Audio-Frequency Magnetotelluric Survey — AmtGeophysicsPanel

**Core integration:** Extend the shared geophysical survey contract over LocationLayoutSystem, ResearchSystem, ExpeditionSystem equipment, induction coils, telluric dipoles, frequency bands, natural-noise sampling, inversion, and geothermal discovery records.

**Smallest vertical slice:** Deploy one AMT station, collect valid frequency bands, calculate a resistivity/phase profile with confidence, and mark one follow-up geothermal target.

**Acceptance gate:** Station geometry, grounding, frequency, thunderstorm noise, cultural interference, sensor condition, recording duration, inversion assumptions, depth resolution, and target confidence are persisted. An 800-meter reservoir is a candidate interpretation requiring confirmation; it cannot authorize a power plant from one survey.

### [334] Epifluorescence Biological Microscopy — EpifluorescenceModal

**Core integration:** Add a fluorescence observation state over ResearchSystem, MedicalSystem, DiseaseSystem, DutyRosterSystem, microscope/lamp/filter condition, sample handling, pathogen catalog, and treatment decision support.

**Smallest vertical slice:** Prepare one sample, select an excitation/emission filter set, observe one catalogued fluorescent morphology, and generate a confidence-rated clinical or environmental follow-up.

**Acceptance gate:** Lamp dose/condition, filter cube, exposure, autofluorescence, staining/antibody availability, sample contamination, operator skill, pathogen morphology, false positives, and confirmatory tests are saved. Identification can trigger quarantine or targeted-test recommendations; targeted antiviral therapy requires a valid disease and treatment authority.

### [335] Meteor-Burst Rake Receiver — RakeReceiverPanel

**Core integration:** Extend RadioHostSession/FactionRadioEngine and the Batch 19/20 radio propagation state with dual-diversity antennas, tap-delay paths, meteor-burst windows, correlation, coding, packet queues, and ResearchSystem unlocks.

**Smallest vertical slice:** Capture one multipath burst on two antenna channels, combine valid paths, decode one packet, and report bit-error rate, throughput, latency, and confidence.

**Acceptance gate:** Antenna separation, geometry, burst duration, path delay, SNR, oscillator drift, coding, buffer, packet loss, and receiver condition are authoritative. A 300% throughput gain is a measured configuration result. A textbook transfer must be sized against actual packet capacity and may take longer or fail.

### [336] Five-Ton Bronze Sanctuary Bell — BellTowerPanel

**Core integration:** Extend SilentFoundrySystem with bell-alloy casting and tune/inspection state; connect it to GenerationalSuccessionEngine, SoundManager, EpilogueMatrix, SaveChecksum, shelter/location construction, and cultural milestone records.

**Smallest vertical slice:** Cast or install one bell component, inspect mass/tone/support, schedule one toll, record contributors and sound metadata, and display a 2D bell-tower view with an acoustic radius estimate.

**Acceptance gate:** Copper/tin composition, mold, pour, shrinkage, casting defects, mass, support, clapper, resonance, atmosphere, sound propagation, time, and safety are persisted. A five-ton bell may be heard across a configured area; it cannot claim continent-wide audibility or trigger victory unless the actual century and construction criteria are met.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for brine, reagents, metal, wire, fuel, water, crops, medical supplies, explosives, optical parts, radio time, audio/cultural materials, and construction stock.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, misalignment, misfire, injury, maintenance, stale-data, and negotiation outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, dose_rate_uSv_h, activity_bq, concentration_bq_l, altitude_m, count_rate_cps, frequency_mhz, wavelength_nm, drift_deg_per_hour, uncertainty_fraction, signal_db, confidence_fraction, condition_fraction, and acoustic_level_db.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current data cannot express the feature, are:

- pyrotechnic_signal_defs.json
- wire_metallurgy_defs.json
- emergency_cardiac_defs.json
- radiological_water_assays.json
- civil_drainage_defs.json
- topical_medicine_defs.json
- perimeter_obstacle_defs.json
- tracer_lathe_profiles.json
- navigation_instrument_defs.json
- acoustic_pest_defs.json
- optical_gyro_defs.json
- geophysical_amt_defs.json
- fluorescence_observation_defs.json
- meteor_burst_profiles.json
- sanctuary_sound_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Flares, stations, diseases, materials, instruments, discoveries, effects, and milestones must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

StrontiumFlarePanel, GalvannealWirePanel, CardiacResuscitationModal, TritiumCounterPanel, WaxSealBottlingPanel, CulvertDrainagePanel, OkraTowerPanel, DoubleApronWirePanel, HydraulicTracerLathePanel, NocturnalDialModal, TermiteDisruptorPanel, SagnacGyroPanel, AmtGeophysicsPanel, EpifluorescenceModal, RakeReceiverPanel, and BellTowerPanel.

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

- pyrotechnic composition, illumination, storage, misfire, extraction visibility, and non-combat signal handling;
- galvannealing, wire coating, tensile/fatigue quality, tracer copying, template deviation, throughput, and material conservation;
- cardiac triage, arrest timing, open-chest risk, defibrillation, pulse/neurological outcomes, and save round-trip;
- tritium sample chain, detector limit, count uncertainty, well classification, water-treatment decision, and stale data;
- solera provenance, bottling, diplomacy, road/culvert capacity, floods, maintenance, crop/mucilage yield, burn effects, contamination, and food storage;
- wire obstacles, sapper attempts, acoustic pest controls, false positives, maintenance, friendly access, and tactical uncertainty;
- nocturnal/planisphere calibration, gyro drift, optical alignment, weather visibility, route confidence, and consumer modifiers;
- AMT inversion, depth uncertainty, epifluorescent observation, pathogen confirmation, and research unlock provenance;
- meteor-burst diversity combining, path delay, packet loss, coding, throughput, latency, and radio save state;
- bell casting, support, acoustic propagation, milestone requirements, cultural records, replay, and campaign-save isolation;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 321–336 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Chemical, medical, industrial, agricultural, perimeter, optical, geophysical, radio, and cultural claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
