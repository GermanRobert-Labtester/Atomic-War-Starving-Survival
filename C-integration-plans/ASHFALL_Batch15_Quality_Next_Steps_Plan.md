# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 15)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 225–240

## Outcome

Batch 15 should deepen the medical, agricultural, optical, communications, survey, and generational layers through small deterministic vertical slices. The supplied feature descriptions are goals, not evidence that the named systems already exist. Each slice must first identify or create one Core authority, then expose it through a typed Godot host session and a persisted save contract.

Use existing authorities wherever they are present:

- **SilentFoundrySystem** and **CraftingSystem** own industrial jobs and production settlement.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** own existing health, trauma, disease, and basic needs behavior.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and the meal/effect pipeline own food production and consumption.
- **MarketSystem** is the active economy authority.
- **CrossingArbitrationSystem**, **NarrativeEncounterSystem**, and **TacticalCombatSystem** own faction encounters and conflict resolution.
- **LocationLayoutSystem**, **ExpeditionSystem**, and **WarlordDoctrineSystem** own locations, travel, and hostile doctrine.
- **FactionRadioEngine**, RadioHostSession, **ResearchSystem**, and **JournalSystem** own radio, discoveries, and authored records.
- **GenerationalSuccessionEngine**, existing census state, and **SaveChecksum** own lineage and long-lived records.

The roadmap names District8Accords, PowerGridPanel, ShelterHazardLoop, and several clinical or geophysical capabilities that are not confirmed as single active authorities. Do not create duplicate systems merely to satisfy a feature name. Establish typed contracts at the entry gates below.

## Batch entry gates

1. **Medical status gate:** define canonical state for pain, wound contamination, joint trauma, mobility, chronic fatigue, dental disease, surgical risk, and training outcomes. NeedsSystem alone is not sufficient for all of these.
2. **Industrial process gate:** carry forward the Batch 10/12 process contract for alloy composition, furnace energy, temperature, yield, scrap, machine wear, interruption, and exact material settlement.
3. **Power and safety gate:** resolve the shared power/grid contract used by sensors, electrolysis, therapeutic heating, optical tooling, radio transmitters, and automatic dampers. PowerGridPanel is a UI target, not proof of an existing authority.
4. **Sensor and survey gate:** define one deterministic observation model for potential gradient, PIR, TEM, optical, and radio sensors. It must handle range, terrain, weather, calibration, noise, confidence, device condition, and false or stale results.
5. **Agriculture and nutrition gate:** define crop-cycle, contamination, storage, recipe, nutrition, deficiency, and bounded survivor-effect contracts. Do not write direct global buffs from panels.
6. **Prisoner and covert-operations gate:** if the preceding batch has not delivered prisoner lifecycle or infiltration state, defer the related handoff or implement the missing Core contract before UI.
7. **Genealogy and archive gate:** preserve parentage, partnership, name, trait provenance, death, migration, and generation events instead of reconstructing a family tree from current survivor snapshots.

## Delivery order

1. **226, 233, 236:** stabilize non-ferrous process and precision manufacturing primitives.
2. **229, 231:** add food processing and vertical crop cycles on the agriculture contract.
3. **227, 238:** establish the clinical procedure and medical-training state.
4. **225, 228, 235:** use the facility, power, weather, and sensor contracts for therapy and warnings.
5. **232, 237:** add perimeter traps and geophysical surveys with uncertainty.
6. **234, 239:** connect navigation artifacts and long-range radio data links.
7. **230:** add covert faction encounters only after encounter and cover identity persistence is ready.
8. **240:** finish with an event-backed dynasty pedigree and archive view.

## Step integration matrix

### [225] Geothermal Sulfur Mud Baths — ThermalSpringsPanel

**Core integration:** Add a thermal-therapy facility state over medical procedures, SurvivorNeedsState, shelter location/facility condition, water and heat inputs, and the health-effect pipeline. District8Accords may provide an unlock or diplomatic source, but it must not become an unowned gameplay dependency.

**Smallest vertical slice:** Assign one eligible survivor to one heated therapy session, consume measured water/heat capacity, resolve temperature and mineral-safety checks, and apply a bounded recovery effect.

**Acceptance gate:** Facility temperature, capacity, water quality, exposure duration, medical contraindications, fatigue, and maintenance are persisted. Joint trauma and chronic fatigue need named statuses with duration and stacking rules. A bath may reduce a fatigue or pain contribution; it does not permanently clear every fatigue penalty without a configured treatment outcome and follow-up.

### [226] Tilting Crucible Furnace — TiltingCruciblePanel

**Core integration:** Extend SilentFoundrySystem with non-ferrous alloy recipes and a tilting-pour job. Connect alloy composition, crucible condition, burner/energy demand, mold compatibility, investment tooling, output quality, scrap, and ResearchSystem unlocks.

**Smallest vertical slice:** Reserve valid copper/tin/zinc or other catalogued inputs, run one pour into one valid mold, and produce an alloy item with measured composition and quality.

**Acceptance gate:** Input mass reconciles with output and slag; temperature, pour rate, tilt control, crucible wear, contamination, and mold condition affect quality. “50 kg without a flaw” is a balance target for a defined recipe and operator skill, not a guaranteed result. Save/load resumes or safely cancels the job.

### [227] Wound Debridement — WoundDebridementModal

**Core integration:** Add a wound-procedure state over MedicalSystem, CombatTraumaSystem, DiseaseSystem, SurvivorNeedsState, clinical inventory, and the canonical affliction/effect pipeline.

**Smallest vertical slice:** Diagnose one contaminated laceration, reserve sterile saline/tools/analgesia, remove necrotic tissue, apply closure/traction, and advance a bounded recovery timeline.

**Acceptance gate:** Wound location, contamination, tissue damage, bleeding, clinician skill, sterile supply, anesthesia, infection risk, procedure quality, and follow-up are persisted. Zero infection is only accepted when the modeled inputs and outcome roll support it; the UI cannot force a cure or five-day recovery.

### [228] Atmospheric Potential Gradient Sensors — PotentialGradientPanel

**Core integration:** Add an atmospheric electrostatic observation state over WeatherSystem, surface mast/device condition, the shared power and capacitor contract, and shelter intake or line-isolation commands.

**Smallest vertical slice:** Sample one configured sensor during one storm state, produce a voltage-gradient reading with confidence, and optionally issue a validated pre-charge/isolation recommendation.

**Acceptance gate:** Sensor height, calibration, weather phase, local field noise, power, capacitor capacity, and sampling interval matter. Automatic action must be an explicit command with safety limits and an event log. A 10 kV/m reading and five-minute warning are data-defined scenarios, not universal guarantees.

### [229] Oak Cider Press and Pomace Vinegar — CiderPressPanel

**Core integration:** Extend RecipeCatalog/data, CraftingSystem, GoodsCatalog, storage, fermentation, and the meal/medical effect pipeline. Reuse the food-culture patterns established by prior bakery and tonic work.

**Smallest vertical slice:** Press one apple batch into cider, split the mass between beverage and pomace, begin one vinegar fermentation, and persist both outputs.

**Acceptance gate:** Raw fruit mass, juice yield, solids, water, press condition, fermentation time, contamination, spoilage, and storage are reconciled. Medicinal vinegar is a catalogued product with bounded uses; no fixed 60-liter/20-kilogram yield appears without valid inputs.

### [230] Frontier Smuggling Sting — SmugglingStingModal

**Core integration:** Add a covert-operation encounter state over CrossingArbitrationSystem, NarrativeEncounterSystem, TacticalCombatSystem, faction relations, expedition roster, and the existing deterministic RNG/event pipeline.

**Smallest vertical slice:** Select an eligible operative, establish a cover identity and informant relationship, resolve one sting encounter, and settle evidence, contraband, injuries, and faction consequences.

**Acceptance gate:** Cover quality, informant trust, route risk, detection, exit plan, combat escalation, confiscation capacity, and prisoner disposition are modeled and saved. Contraband is a catalogued inventory result. The system must not guarantee twenty rifles or a named ringleader without a generated encounter payload.

### [231] Vertical Pea Trellises — PeaTrellisPanel

**Core integration:** Extend GreenhouseSystem with a climbing crop definition, trellis capacity, nitrogen/soil or hydroponic requirements, harvest batches, recipe inputs, and the typed nutrition/deficiency effect contract.

**Smallest vertical slice:** Plant one trellis, advance one growth cycle, resolve pollination/contamination/water/light demand, harvest one pea batch, and consume it in a meal.

**Acceptance gate:** Trellis footprint, growth duration, water, nutrients, light/power, yield, spoilage, and harvest loss are authoritative. Well-Nourished is a named effect with a bounded duration and stacking rule, not a direct global stat write. The 20-kilogram result is a data target, not an unconditional outcome.

### [232] Acoustic Tripwires — AcousticTripwirePanel

**Core integration:** Add a perimeter-trap state over LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, sentry assignments, and the shared hazard/event system. Reuse the trap placement model later needed by Batch 16 razor wire.

**Smallest vertical slice:** Place one tripwire in a valid gully cell, arm it with a defined alarm/distraction payload, detect one crossing, and dispatch one sentry response.

**Acceptance gate:** Placement terrain, visibility, line tension, weather degradation, false triggers, maintenance, payload safety, and response time are persisted. “Flashbang” must be a catalogued effect with friendly-fire and supply rules. No automatic enemy confusion or casualty outcome is allowed from a UI button.

### [233] Gear Hobbing and Watch Escapements — GearHobbingPanel

**Core integration:** Extend SilentFoundrySystem, CraftingSystem, ResearchSystem, precision-tool condition, and item definitions for gears, escapements, timers, chronometers, and fuse components.

**Smallest vertical slice:** Machine one bronze gear set from valid stock, inspect tooth profile/tolerance, assemble one timing device, and expose its measured drift/condition to a navigation consumer.

**Acceptance gate:** Blank material, cutter profile, machine calibration, operator skill, tolerance, lubrication, and wear affect the result. A marine chronometer is a typed device with drift and maintenance; a navigation bonus is applied only through an existing expedition modifier contract. No automatic +15% bonus or perfect clock is accepted.

### [234] Copperplate Star Charts — StarChartPrintModal

**Core integration:** Add a chart artifact over JournalSystem, ExpeditionSystem, CraftingSystem, route/weather state, and the archive/catalog contract. Charts should describe fictional in-world sky and terrain data, not real-world military geography.

**Smallest vertical slice:** Survey or unlock one route, print one dated chart, equip it to one expedition, and apply a bounded night-navigation confidence modifier.

**Acceptance gate:** Chart coverage, date/season, cloud/fallout visibility, scout skill, compass/timekeeping condition, route familiarity, and chart wear matter. The chart reduces disorientation probability; it never creates zero risk or guarantees safe travel.

### [235] Passive Infrared Perimeter Sensors — PirThermalPanel

**Core integration:** Add PIR observation state over the shared sensor contract, LocationLayoutSystem or ShelterHazardLoop if that authority is confirmed, power, sentry dispatch, weather/foliage, and tactical encounters.

**Smallest vertical slice:** Install one sensor sector, sample one moving contact, classify it with confidence, and create a warning event or dispatch request.

**Acceptance gate:** Detection distance, body-heat contrast, occlusion, ambient temperature, foliage, sensor angle, battery/solar state, false positives, and maintenance are persisted. “300 meters” is a configured test case. A sensor cannot identify a mutant species or human squad with certainty without a catalogued classification model.

### [236] Optical Prism Grinding and Telescope Collimation — OpticsGrindingPanel

**Core integration:** Create a precision-optics manufacturing contract over SilentFoundrySystem, ResearchSystem, CraftingSystem, glass inventory, abrasives, pitch laps, calibration tools, and optical device condition.

**Smallest vertical slice:** Grind one prism or lens blank, measure curvature/figure/clarity, collimate one instrument, and produce a device with a quality band.

**Acceptance gate:** Glass type, blank mass, grit, tool wear, polish time, curvature, alignment, chromatic performance, and breakage are modeled. A sniper scope or periscope consumes a valid finished optical component and exposes a typed magnification/clarity value. “8x” and “+50% critical range” are balance outcomes, not direct UI guarantees.

### [237] TEM Subsurface Conductivity Survey — TemGeophysicsPanel

**Core integration:** Add a geophysical survey state over LocationLayoutSystem, ResearchSystem, expedition equipment, power, survey grid, and resource discovery catalogs.

**Smallest vertical slice:** Deploy one transmitter loop, pulse a defined waveform, collect one decay curve, and resolve one probabilistic aquifer or ore-body interpretation.

**Acceptance gate:** Loop geometry, current, ground conductivity, depth resolution, noise, weather, operator skill, equipment condition, survey duration, and follow-up drilling are persisted. A survey produces evidence and confidence, not a guaranteed deposit. Discovered resources must be canonical location/catalog records.

### [238] Stereoscopic Anatomy Training — StereoAnatomyModal

**Core integration:** Add a medical-training record over MedicalSystem, DutyRosterSystem, JournalSystem, ResearchSystem or the existing skill progression authority, and authored slide/document inventory.

**Smallest vertical slice:** Assign one apprentice to one available anatomy module, consume time and training material, record completion, and apply one validated knowledge/skill increment.

**Acceptance gate:** Apprentice eligibility, instructor or material quality, study time, fatigue, module prerequisites, retention, and skill caps are saved. XP must flow through the canonical progression system. “Vascular Suture” is an unlockable catalogued capability, not a free-form panel flag.

### [239] Tropospheric Scatter Data Links — TropoScatterPanel

**Core integration:** Extend RadioHostSession/FactionRadioEngine, ResearchSystem, LocationLayoutSystem, antenna/device condition, shared power, and the sensor/noise model with line-of-sight-beyond-horizon troposcatter links.

**Smallest vertical slice:** Align two known fictional stations, establish one link at a measured data rate, transmit one weather or warning record, and record delivery/confidence.

**Acceptance gate:** Station geometry, elevation, frequency, weather, antenna alignment, power, noise, bandwidth, queue latency, and outages are modeled. A link can degrade, drop, or deliver stale data. “Instantaneous” sharing is not accepted unless latency is explicitly configured as negligible for the scenario.

### [240] 100-Year Dynasty Pedigree — DynastyPedigreePanel

**Core integration:** Extend GenerationalSuccessionEngine, existing census state, JournalSystem, survivor identity/relationship records, and SaveChecksum with event-backed parentage, partnerships, surnames, migrations, deaths, traits, and founder references.

**Smallest vertical slice:** Record one founder generation and two descendants with a marriage/partnership and inherited trait provenance, then render the graph from a typed read model.

**Acceptance gate:** The graph handles missing parents, remarriage, adoption or guardianship if supported, name changes, migration, deceased members, duplicate identities, and save migration. It must query historical events rather than infer ancestry only from current roster state. Five generations and a 100-year span are campaign data outcomes, not hard-coded labels.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for ingredients, water, fuel, power, tools, medical supplies, film, and survey equipment.
- Explicit interruption, cancellation, failure, spoilage, contamination, injury, and maintenance outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, voltage_kv_per_m, current_a, signal_db, confidence_fraction, condition_fraction, contamination_fraction, and time_drift_ppm.

## Data authority and content work

Extend existing catalogs where possible. Add new authority files only where current data cannot express the feature. Candidate data surfaces are:

- thermal_facilities.json
- industrial_processes.json
- clinical_procedures.json
- food_cultures.json
- perimeter_sensors.json
- geophysical_survey_defs.json
- optical_devices.json
- radio_link_profiles.json
- dynasty_record_rules.json

Every new or edited JSON authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. New named destinations, factions, devices, effects, traits, procedures, and resources must resolve through the catalog registry. Narrative flavor files are not substitutes for authoritative state.

## Godot host and UI work

Build these panels only after their host contracts exist:

ThermalSpringsPanel, TiltingCruciblePanel, WoundDebridementModal, PotentialGradientPanel, CiderPressPanel, SmugglingStingModal, PeaTrellisPanel, AcousticTripwirePanel, GearHobbingPanel, StarChartPrintModal, PirThermalPanel, OpticsGrindingPanel, TemGeophysicsPanel, StereoAnatomyModal, TropoScatterPanel, and DynastyPedigreePanel.

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

- clinical eligibility, contamination, bounded recovery, training prerequisites, and skill unlocks;
- alloy composition, thermal/pour quality, material conservation, machine wear, and interruption;
- crop/fermentation yields, spoilage, nutrition effects, and meal integration;
- covert operation detection, evidence, faction consequences, inventory settlement, and combat escalation;
- trap and sensor false positives, confidence, terrain/weather attenuation, maintenance, and dispatch;
- chart, chronometer, optical, TEM, and radio-link determinism and uncertainty;
- genealogy event ordering, migration, missing records, trait provenance, save migration, and graph rendering;
- clean, tampered, missing-checksum, legacy, future-version, and atomic-write saves.

## Definition of done

Each Step 225–240 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Medical, resource, sensor, and generational claims are backed by tunable data and failure modes.
6. The Godot-only verification pipeline passes without Unity dependencies.
