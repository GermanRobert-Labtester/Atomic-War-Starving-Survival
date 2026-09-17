# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 19)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 289–304

## Outcome

Batch 19 should connect late industrial production, emergency medicine, atmospheric measurement, geophysics, radio, food preservation, and long-lived cultural memory. It is a dependency batch: most features require contracts introduced in Batches 10, 12, 15, and 16.

The supplied feature descriptions are targets, not existing authorities. Every feature must become a small Core-owned vertical slice with explicit inputs, uncertainty, resource settlement, save state, and a typed Godot read model. Absolute claims such as zero infection, 2,000 km radio, 10 nanometer accuracy, or eternal victory are not accepted as UI-only outcomes.

Active authorities to reuse:

- **SilentFoundrySystem** and **CraftingSystem** own industrial jobs, material use, machine condition, and output settlement.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** own existing clinical, trauma, disease, and basic-needs behavior.
- **WeatherSystem** and **RadiationSystem** own environmental and radiation simulation; use the shared sensor/air-quality contract from earlier batches.
- **MarketSystem**, **GoodsCatalog**, recipe data, and the meal/effect pipeline own products, trade goods, and consumable effects.
- **LocationLayoutSystem**, **ExpeditionSystem**, **TacticalCombatSystem**, and **WarlordDoctrineSystem** own locations, travel, combat, and hostile behavior.
- **FactionRadioEngine**, RadioHostSession, **ResearchSystem**, and **JournalSystem** own radio, discoveries, and authored records.
- **GenerationalSuccessionEngine**, existing census/genealogy state, **EpilogueMatrix**, and **SaveChecksum** own historical continuity and milestone presentation.

District8Accords, PowerGridPanel, ShelterHazardLoop, and several named geophysics or pathology systems are not confirmed as independent active authorities. Treat them as unlock/read-model names until a Core contract exists.

## Batch entry gates

1. **Chemical and clinical safety:** carry forward antacid, bleach, tonic, wound, cranial, and cardiac procedure contracts. Define dosage, contraindication, contamination, failure, recovery, and follow-up.
2. **Industrial sheet and precision process:** carry forward foundry process contracts for rolling, galvanizing, boring, optical metrology, material quality, energy, wear, scrap, inspection, and interruption.
3. **Atmospheric radiation telemetry:** define balloon, detector, weather layer, count-rate, shielding, altitude, calibration, and confidence behavior. A detector reading is evidence, not an automatic safe-window truth.
4. **Food fermentation and storage:** define mother culture lineage, cask transfer, age, contamination, yield, storage, and trade valuation without inventing magical century quality.
5. **Road and worksite infrastructure:** establish road segments, construction jobs, surface hazards, labor safety, vehicle travel, maintenance, and route modifiers. Reuse the Batch 15/16 prisoner and duty contracts rather than creating a separate labor model.
6. **Geophysical survey contract:** unify TEM, ERT, SP, RMT, and future surveys around equipment, survey grids, inversion confidence, follow-up confirmation, and canonical discoveries.
7. **Optical and radio measurement:** unify planisphere, interferometer, microscope, and skip-link models around calibration, noise, device condition, propagation, and confidence.
8. **Generational archive:** persist authored records, contributors, dates, lineage, materials, and milestone evidence. Do not derive a century history from the current roster alone.

## Delivery order

1. **289, 291:** close medical chemistry and emergency abdominal-trauma prerequisites.
2. **290, 297, 300:** extend the industrial process and precision-metrology pipeline.
3. **293, 295:** add solera food culture and dehydrated crop preservation.
4. **292, 301, 302:** implement atmospheric, geophysical, and clinical-laboratory observation.
5. **294, 296, 299:** extend roads, perimeter defense, and pest-control infrastructure.
6. **298, 303:** connect analog navigation and sporadic-E radio behavior.
7. **304:** finish with the event-backed rotunda and century milestone presentation.

## Step integration matrix

### [289] Geothermal Magnesium Hydroxide Antacid — AntacidMagnesiaPanel

**Core integration:** Add a chemical production and medication record over brine inputs, slaked lime, water, centrifuge/filter equipment, CraftingSystem, GoodsCatalog, MedicalSystem, and the canonical gastrointestinal-status/effect pipeline. District8Accords can unlock access or recipe knowledge but must not silently own the chemistry.

**Smallest vertical slice:** Process one measured brine batch into a tested magnesium-hydroxide suspension, bottle it, administer one dose to an eligible survivor, and record response or adverse effect.

**Acceptance gate:** Brine composition, reagent mass, reaction yield, filtration, concentration, contamination, storage, dosage, contraindications, and worker exposure are persisted. Gastritis, reflux, or ulcer status must be named clinical effects with duration and follow-up. Distributing medicine cannot erase every ulcer debuff or guarantee appetite restoration.

### [290] Continuous Cold-Roll and Corrugated Sheet Mill — CorrugatedIronPanel

**Core integration:** Extend SilentFoundrySystem with sheet rolling, corrugation, optional galvanizing, coil inventory, machine condition, ShelterOperationsPanel construction jobs, and exterior weather/shielding ratings.

**Smallest vertical slice:** Reserve one steel coil, roll and corrugate a measured batch, inspect gauge and coating, then consume a quantity in one roof or wall-cladding segment.

**Acceptance gate:** Coil mass, thickness reduction, roll pressure, speed, temperature, corrugation geometry, zinc/flux input, scrap, machine wear, and inspection are authoritative. A roof segment must check span, fasteners, structural frame, fallout load, wind, and corrosion. One hundred sheets and a complete hangar roof are data-defined outputs, not guaranteed results.

### [291] Hepatic Trauma Damage Control — HepaticSurgeryModal

**Core integration:** Add a high-risk abdominal procedure state over MedicalSystem, CombatTraumaSystem, DiseaseSystem, SurvivorNeedsState, clinical supplies, clinician skill, and the canonical bleeding/death/recovery pipeline.

**Smallest vertical slice:** Triage one liver-injury case, reserve gauze/clamp/cautery/anesthesia supplies, resolve packing and vascular-control risk, and advance the survivor through stabilization or deterioration.

**Acceptance gate:** Injury grade, blood loss, shock, time-to-treatment, surgeon skill, sterility, equipment condition, clamp duration, rebleeding, infection, transfusion, and follow-up are saved. The “saved scout” is a possible result from valid inputs and a resolved procedure, not a guaranteed modal success.

### [292] Geiger Radiation Cloud Altimeter Balloon — GeigerAltimeterPanel

**Core integration:** Add a tethered-balloon observation state over WeatherSystem, RadiationSystem, device condition, balloon/winch inventory, power, and expedition sortie planning.

**Smallest vertical slice:** Launch one instrumented balloon, sample count rate at several altitude bands, estimate a cloud boundary with confidence, and publish one sortie recommendation.

**Acceptance gate:** Balloon altitude, cable/winch capacity, wind, detector response, beta shielding, count-rate saturation, calibration, cloud thickness, fallout composition, and sample timing are modeled. A cloud above 1,000 meters may create a configured lower-risk interval; it cannot automatically declare six hours safe for every route or survivor.

### [293] Solera Vinegar Cask Battery — SoleraBatteryPanel

**Core integration:** Extend the Batch 15 cider/fermentation contract over RecipeCatalog/data, GoodsCatalog, storage, JournalSystem, and GenerationalSuccessionEngine. Preserve mother-culture lineage and cask transfer history as records, not as a hard-coded 50-year flag.

**Smallest vertical slice:** Fill a three-tier battery, perform one fractional draw and refill, age one batch across a save/load boundary, and create a trade-gift item with provenance.

**Acceptance gate:** Cask capacity, transfer fraction, mother health, acidity, contamination, evaporation, storage temperature, age, bottling, and valuation are persisted. A diplomatic gift affects MarketSystem/faction relations only through an explicit trade or arbitration outcome; it cannot create an eternal treaty by itself.

### [294] Supervised Highway Paving — HighwayPavingPanel

**Core integration:** Add a road-segment and construction state over DutyRosterSystem, CaregivingSystem, LocationLayoutSystem, ExpeditionSystem, MarketSystem, vehicle travel, aggregate/bitumen inventory, and the Batch 15 prisoner/parole lifecycle where applicable.

**Smallest vertical slice:** Survey one fictional route, assign a safe supervised crew, pave one segment, compact and inspect it, then apply a measured travel modifier to one route.

**Acceptance gate:** Route geometry, aggregate, binder, water, labor hours, guard/care capacity, weather, vehicle traffic, compaction, maintenance, damage, and route access are saved. Work assignments require lawful/safe status and cannot be a UI-only coercion. A 50-kilometer road and a 75% travel reduction are balance outcomes, not permanent global effects.

### [295] Vertical Zucchini and Dehydrated Chips — ZucchiniTowerPanel

**Core integration:** Extend GreenhouseSystem with a zucchini crop, vertical-column capacity, dehydration, storage, expedition rations, GoodsCatalog, NeedsSystem, water, light/heat, and power demand.

**Smallest vertical slice:** Plant one column, harvest one crop batch, slice and dry a measured portion, and add the resulting ration to inventory.

**Acceptance gate:** Crop cycle, pollination, water, nutrients, light, contamination, slicer/dehydrator condition, drying loss, packaging, spoilage, and ration quality are authoritative. Fifty kilograms cannot become one hundred bags unless the configured dry-mass and package recipes reconcile.

### [296] S-Bend Defensive Corridors — FunnelKillZonePanel

**Core integration:** Extend the Batch 15/16 perimeter trap and wire placement model over LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, sentry assignments, cover, firing arcs, ammunition, and civilian/access safety.

**Smallest vertical slice:** Place one legal corridor segment, validate two firing arcs and cover states, resolve one assault approach, and produce a tactical report.

**Acceptance gate:** Terrain, wire length, construction time, visibility, fields of fire, ammunition, defender exposure, enemy doctrine, countermeasures, and friendly access are persisted. The model may improve defensive position and change casualty probabilities; it cannot guarantee annihilation, zero friendly losses, or a predetermined enemy casualty count.

### [297] Heavy Horizontal Boring Mill — BoringMillPanel

**Core integration:** Extend SilentFoundrySystem and ResearchSystem with large-bore machining, engine-block and cylinder-sleeve definitions, measurement tools, heat treatment, assembly, and the active generator/vehicle consumer.

**Smallest vertical slice:** Bore one compatible engine block, inspect bore geometry, install one sleeve or piston set, and run a condition/load test.

**Acceptance gate:** Blank geometry, spindle travel, tool rigidity, material hardness, coolant, tolerances, surface finish, machine wear, power, and inspection results are saved. A 500-horsepower generator requires a catalogued engine design, components, fuel, cooling, and a rated installation; it cannot appear solely from one UI completion.

### [298] Rotating Planisphere — PlanisphereCraftModal

**Core integration:** Add a navigation artifact over JournalSystem, ExpeditionSystem, CraftingSystem, SimClock, route latitude/season, weather visibility, and the existing chart/chronometer contracts.

**Smallest vertical slice:** Engrave one planisphere for a defined latitude band, calibrate its date/hour alignment, equip it to one night expedition, and apply a bounded heading-confidence modifier.

**Acceptance gate:** Calibration, latitude, season, moon/cloud/fallout visibility, scout skill, time drift, route familiarity, and device condition matter. A planisphere reduces uncertainty; it never guarantees 100% heading accuracy.

### [299] Acoustic Cockroach Perimeter — CockroachDisruptorPanel

**Core integration:** Extend the Batch 16 pest-control contract over pantry/medical inventory, GreenhouseSystem where relevant, DiseaseSystem or spoilage authority, power, acoustic coverage, species response, and sanitation inspections.

**Smallest vertical slice:** Install one room emitter, configure one validated frequency band, run one storage cycle, and record pest pressure, contamination, power, and maintenance.

**Acceptance gate:** Pest species, baseline infestation, room geometry, seals, attenuation, device condition, power, food/medical sensitivity, false confidence, and non-target effects are modeled. Zero contamination is a scenario result only when sanitation and inspection also pass.

### [300] Michelson Interferometer Metrology — InterferometerPanel

**Core integration:** Extend the Batch 15/16 optical manufacturing contract over SilentFoundrySystem, ResearchSystem, granite-bench stability, vibration, temperature, beam splitter/mirror condition, gauge blocks, and calibration records.

**Smallest vertical slice:** Align the interferometer, count a repeatable fringe displacement, calibrate one known gauge, and attach an uncertainty interval to the measurement.

**Acceptance gate:** Optical path length, wavelength, alignment, vibration, thermal drift, air path, mirror quality, operator skill, fringe count, and repeatability are persisted. A 10-nanometer result is accepted only if the modeled resolution and uncertainty support it; otherwise the output is a lower-confidence calibration.

### [301] Electrical Resistivity Tomography — ErtGeophysicsPanel

**Core integration:** Extend the shared geophysical survey state over LocationLayoutSystem, ResearchSystem, expedition equipment, 64-electrode layouts, power, inversion configuration, and canonical resource/cavern discoveries.

**Smallest vertical slice:** Deploy one electrode array, collect a valid measurement set, run one deterministic inversion, and mark a subsurface anomaly with confidence and follow-up requirements.

**Acceptance gate:** Electrode contact, spacing, current, terrain, saturation, noise, array geometry, inversion assumptions, depth resolution, survey time, and operator skill are saved. A cavern is a candidate interpretation until confirmed by a route, borehole, or later evidence; it cannot become a guaranteed room from a color-map click.

### [302] Phase-Contrast and Darkfield Microscopy — PhaseContrastModal

**Core integration:** Add laboratory observation records over ResearchSystem, MedicalSystem, DiseaseSystem, DutyRosterSystem, microscope condition, sample handling, and the pathogen catalog.

**Smallest vertical slice:** Prepare one water or patient sample, configure one condenser mode, observe a catalogued morphology, and create a diagnosis or follow-up test recommendation with confidence.

**Acceptance gate:** Sample quality, contamination, magnification, condenser alignment, operator skill, pathogen morphology, false positives, and confirmation tests are modeled. The system can trigger a water-treatment or clinical review action through a validated command; it cannot identify every pathogen with certainty in seconds.

### [303] Sporadic-E VHF Skip Links — SporadicESkipPanel

**Core integration:** Extend RadioHostSession/FactionRadioEngine, ResearchSystem, LocationLayoutSystem, the ionospheric measurement contract, antenna condition, power, and long-range message delivery.

**Smallest vertical slice:** Observe one sporadic-E event, configure a 50 MHz link between two fictional known stations, transmit one voice/data packet, and record signal-to-noise, latency, and delivery confidence.

**Acceptance gate:** Cloud position, duration, frequency, station geometry, antenna elevation, weather, noise, power, queueing, and receiver condition are persisted. A 2,000-kilometer maritime link is a data-defined scenario and may fail, fade, or deliver stale information.

### [304] Century Stained-Glass Sanctuary Rotunda — SanctuaryRotundaPanel

**Core integration:** Add a construction and cultural milestone state over GenerationalSuccessionEngine, JournalSystem, EpilogueMatrix/EpilogueMatrixRuntime, SaveChecksum, materials, shelter layout, and authored panel records.

**Smallest vertical slice:** Complete one rotunda section, install one documented stained-glass panel, record its contributors and milestone, and display it in a 2D Godot sanctuary view.

**Acceptance gate:** Glass/lead/frame materials, construction jobs, light/maintenance, panel provenance, contributor signatures, accessibility, and milestone requirements are persisted. The 100-year sequence triggers only when the campaign clock, lineage/history, and construction criteria are actually satisfied. The finale must be replayable and must not corrupt the ordinary campaign save.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for brine, reagents, metal coils, fuel, water, food, medical supplies, optical components, radio time, and construction materials.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, misalignment, injury, maintenance, and stale-data outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, concentration_mg_m3, dose_rate_uSv_h, altitude_m, count_rate_cps, wavelength_nm, uncertainty_nm, signal_db, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new data surfaces, only if current catalogs cannot express the domain, are:

- clinical_medications.json
- sheet_mill_processes.json
- road_segment_specs.json
- atmospheric_sensor_defs.json
- fermentation_cask_defs.json
- perimeter_defense_defs.json
- industrial_metrology_defs.json
- geophysical_survey_defs.json
- pathogen_observation_defs.json
- radio_skip_profiles.json
- sanctuary_panel_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Named settlements, stations, pathogens, materials, effects, and historical milestones must resolve through the catalog registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

AntacidMagnesiaPanel, CorrugatedIronPanel, HepaticSurgeryModal, GeigerAltimeterPanel, SoleraBatteryPanel, HighwayPavingPanel, ZucchiniTowerPanel, FunnelKillZonePanel, BoringMillPanel, PlanisphereCraftModal, CockroachDisruptorPanel, InterferometerPanel, ErtGeophysicsPanel, PhaseContrastModal, SporadicESkipPanel, and SanctuaryRotundaPanel.

Each panel must bind to a typed host session/read model, expose loading/empty/error states, show units and confidence where relevant, and issue commands that return validation results. No Bind(object), placeholder arrays, direct Core mutation, hard-coded success, or unsupported 3D-only dependency is permitted. New dirty stores must be wired through Main.Setup, Main.Save, and Main.Flush with the shared atomic writer.

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

- chemical yield, dosage, contraindications, contamination, shelf life, and clinical response;
- sheet rolling, coating, structural installation, boring tolerances, generator/load validation, and material conservation;
- emergency trauma triage, timing, procedure risk, complications, death/recovery, and save round-trip;
- balloon count-rate profiles, altitude interpolation, cloud-layer confidence, sortie risk, and stale sensor data;
- solera transfer/aging, crop dehydration, storage loss, food effects, trade valuation, and provenance;
- roads, labor safety, maintenance, vehicle route changes, perimeter arcs, trap outcomes, and pest-control false positives;
- optical alignment, interference measurement, uncertainty, planisphere calibration, microscope observations, and survey inversions;
- radio propagation, fade, latency, message delivery, station geometry, and time/weather variation;
- generational milestone conditions, panel provenance, replay, checksum, and campaign-save isolation;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 289–304 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Medical, industrial, sensor, geophysical, radio, and generational claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
