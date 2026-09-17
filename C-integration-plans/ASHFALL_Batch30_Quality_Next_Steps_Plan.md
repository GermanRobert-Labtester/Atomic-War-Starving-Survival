# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 30)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 465–480

## Outcome

Batch 30 expands education, siege engineering, marine ecology, animation, nuclear-research governance, roads, textiles, inertial navigation, forest restoration, underwater acoustics, photogrammetry, computation, carbon chemistry, literature, and water-powered food production.

These are cross-system features. They must extend existing Core authorities instead of creating gameplay inside panels:

- **GenerationalSuccessionEngine**, DutyRosterSystem, JournalSystem, ResearchSystem, and the progression contract own teaching, knowledge transfer, and records.
- **TacticalCombatSystem**, **SilentFoundrySystem**, and **WarlordDoctrineSystem** own siege equipment and tactical outcomes.
- **StealthDiveInstance**, **LocationLayoutSystem**, ResearchSystem, and the surface-vessel/marine contracts own coastal ecology, submarine navigation, and whale research.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and CraftingSystem own nettles, mycorrhiza, textiles, grain, and bread.
- **WeatherSystem**, the shared power/utility contract, and chemistry/process state own thorium feasibility and DAC/methanation.
- **JournalSystem**, SoundManager, and cultural-event state own films, music, poems, and heritage archives.
- **SaveChecksum** and the generational/archive contract own century milestones.

PowerGridPanel, CenturySeed, and other roadmap names should remain adapters/unlocks unless a typed authority is verified. 3D photogrammetry and point clouds must be represented through bounded data and 2D Godot views unless a separate runtime decision is approved.

## Batch entry gates

1. **Education and child-safety gate:** define pupil identity/age, consent/guardianship, teacher duty, curriculum, attendance, assessment, fatigue, progression, and cross-generation knowledge transfer.
2. **Siege gate:** define projectile mass/energy, traction, counterweight, range, accuracy, ammunition, structure damage, defender exposure, and repair. No guaranteed wall breach.
3. **Marine ecology gate:** define reef substrate, water chemistry, temperature, coral survival, biodiversity, fisheries, and survey uncertainty.
4. **Animation/archive gate:** define authored film assets, script/storyboard identity, production time, audio, screening, audience, provenance, and storage.
5. **Nuclear research gate:** keep thorium as feasibility/research until a separately approved safe power implementation exists. Model assays and knowledge unlocks, not an operational reactor by implication.
6. **Civil road gate:** reuse road, drainage, maintenance, worksite, and route modifier contracts; no elimination of every weather penalty.
7. **Multi-purpose crop/textile gate:** model nettle toxicity/processing, rennet, fiber quality, cheese, textile inputs, and exact mass settlement.
8. **Inertial-navigation gate:** reuse Sagnac/gyro and maritime state with bias, drift, calibration, and position-error growth.
9. **Ecology and forest gate:** define fungal inoculum, host compatibility, soil, moisture, survival, plot growth, and biodiversity; no guaranteed threefold restoration.
10. **Radio/heritage gate:** define hydrophone identification, photogrammetric provenance/compression, difference-engine accuracy, DAC energy balance, poem authorship, and water-wheel capacity.

## Delivery order

1. **465, 466:** establish education and siege process contracts.
2. **467, 468, 475:** add marine ecology, film, and acoustic research.
3. **469, 478:** add bounded scientific feasibility and carbon chemistry.
4. **470, 471, 474:** extend roads, crops, food, and textile production.
5. **472, 473:** add inertial navigation and forest restoration.
6. **476, 477:** add heritage survey and mechanical computation.
7. **479, 480:** complete literature and water-powered staple food systems.

## Step integration matrix

### [465] Sanctuary School and Literacy — SanctuarySchoolPanel

**Core integration:** Add an education state over GenerationalSuccessionEngine, DutyRosterSystem, NeedsSystem, survivor/child identity, guardianship, teacher skills, JournalSystem, ResearchSystem, and knowledge progression.

**Smallest vertical slice:** Enroll one eligible child cohort, assign a teacher and lesson, record attendance/assessment, and unlock one knowledge milestone.

**Acceptance gate:** Age/eligibility, consent, safety, teacher workload, classroom capacity, lesson prerequisites, attendance, fatigue, assessment, retention, and transfer to later generations are persisted. A class of ten and +20% research speed require actual pupils and a named research modifier; no direct global stat write.

### [466] Clockwork Siege Trebuchet — TrebuchetSiegePanel

**Core integration:** Extend TacticalCombatSystem, SilentFoundrySystem, WarlordDoctrineSystem, projectile inventory, construction, ballistic calculation, target fortification, crew, and maintenance.

**Smallest vertical slice:** Build one trebuchet, calibrate counterweight/sling, launch one catalogued projectile at one target, and resolve accuracy/damage with an after-action report.

**Acceptance gate:** Counterweight, projectile mass, sling length, release timing, wind, range, elevation, crew, frame condition, target material, impact, reload, and repair are authoritative. A 100-kilogram projectile and 400-meter range are configured capacity values; a fortress breach requires a tactical damage sequence.

### [467] Coral Reef Bioremediation — CoralReefSeedingPanel

**Core integration:** Add a marine habitat state over LocationLayoutSystem, StealthDiveInstance, surface-vessel/underwater expedition, water chemistry, temperature, radiation, reef substrate, coral fragments, fisheries, and ResearchSystem.

**Smallest vertical slice:** Place one reef module, graft one compatible coral fragment, monitor one growth interval, and record survival/biodiversity indicators.

**Acceptance gate:** Depth, temperature, pH, salinity, light, sediment, radiation, fragment health, substrate, predation, and survey uncertainty are saved. One hectare and abundant fish require sustained coverage and ecological evidence; the UI cannot declare a thriving reef after placement.

### [468] Hand-Drawn Animation Studio — AnimationStudioPanel

**Core integration:** Add an authored production state over JournalSystem, DutyRosterSystem, NeedsSystem, SoundManager, script/storyboard records, cel/film materials, screening venue, and archive media.

**Smallest vertical slice:** Write or select one script, assign artists/actors, produce a short sequence, screen it, and archive the film with provenance.

**Acceptance gate:** Script, frames, exposure sheet, materials, camera, audio, artist time, defects, storage, audience, and screening conditions are persisted. Morale is a bounded event effect; the first film cannot automatically become the highest morale event without comparing recorded outcomes.

### [469] Thorium Fuel-Cycle Feasibility — ThoriumFeasibilityPanel

**Core integration:** Add a research-only study state over ResearchSystem, material assays, neutron/energy model inputs, safety review, power research catalog, and the shared energy authority. Do not implement an operational breeder reactor in this step.

**Smallest vertical slice:** Assay a thorium sample, run a versioned feasibility calculation, record breeding-ratio assumptions/uncertainty, and unlock or reject one research node.

**Acceptance gate:** Ore composition, neutron assumptions, conversion model, losses, shielding, waste, proliferation/safety review, energy demand, and research provenance are saved. Completion may unlock a future research tree; it cannot create U-233 fuel, energy independence, or an unapproved reactor.

### [470] Roman Road and Drainage Network — RomanRoadPanel

**Core integration:** Extend the Batch 21/26 civil road state over DutyRosterSystem, LocationLayoutSystem, CrossingArbitrationSystem, aggregate/stone, culverts, drainage, weather, maintenance, caravan/expedition routes, and vehicle condition.

**Smallest vertical slice:** Build one layered road segment with drainage, inspect it, simulate one weather event, and apply a measured route modifier.

**Acceptance gate:** Foundation, subbase, paving stone, slope, culvert, labor safety, weather, traffic, erosion, repairs, and route access are persisted. A +40% travel effect is segment- and weather-specific; the road cannot erase every movement penalty or grant infinite all-weather access.

### [471] Vertical Nettle, Rennet, and Fiber — NettleTowerPanel

**Core integration:** Extend GreenhouseSystem, GoodsCatalog, RecipeCatalog/data, CraftingSystem, textile, cheese, and NeedsSystem. Include blanching/toxin handling and separate food/fiber outputs.

**Smallest vertical slice:** Grow one tower, harvest a measured batch, process leaves into rennet and stalks into fiber, then use each output in one valid recipe.

**Acceptance gate:** Crop growth, water/nutrients, harvest mass, blanching, rennet activity, curd yield, fiber length/strength, processing loss, contamination, textile quality, and storage are authoritative. Both cheese and textile outputs require reconciled mass; the crop cannot produce two full products from the same material.

### [472] Inertial Navigation Dead Reckoning — InertialNavModal

**Core integration:** Extend StealthDiveInstance, ExpeditionSystem, ResearchSystem, Sagnac/gyro state, accelerometers, SimClock, route maps, and navigation error consumers.

**Smallest vertical slice:** Calibrate one inertial unit, run one submerged route interval, integrate motion with bias/drift, and produce a position estimate with error ellipse.

**Acceptance gate:** Initial position, gyro bias, accelerometer bias, drift, scale error, vibration, temperature, calibration, elapsed time, maneuvers, currents, and correction opportunities are saved. A two-week patrol can remain within five kilometers only for a defined device/route profile; dead reckoning error must grow and remain visible.

### [473] Mycorrhizal Forest Inoculation — MycorrhizalNetworkPanel

**Core integration:** Add a restoration-plot state over GreenhouseSystem, LocationLayoutSystem, environmental soil/weather, sapling inventory, fungal inoculum, biodiversity, contamination, and ResearchSystem.

**Smallest vertical slice:** Inoculate one sapling plot, advance a growth interval, assay colonization and survival, and update canopy/restoration progress.

**Acceptance gate:** Host species, inoculum compatibility, soil, moisture, temperature, radiation, pests, survival, network spread, canopy growth, and sampling are persisted. A threefold acceleration or five-year closure is a balance result requiring measured plot state, not an automatic ecological multiplier.

### [474] Steam Jacquard Textile Loom — JacquardLoomPanel

**Core integration:** Extend SilentFoundrySystem, DutyRosterSystem, ResearchSystem, CraftingSystem, textile inventory, steam/power, punch-card pattern data, loom condition, and cloth consumers.

**Smallest vertical slice:** Punch one pattern, set up warp/weft, run one powered batch, inspect the cloth, and add it to a valid clothing/blanket recipe.

**Acceptance gate:** Yarn mass, fiber type, pattern complexity, warp tension, steam, speed, breakage, loom wear, cloth length, width, warmth, and storage are authoritative. Fifty meters and complete clothing require actual fiber stock, sizing, and tailoring capacity.

### [475] Whale Bioacoustic Catalog — WhaleAcousticPanel

**Core integration:** Add a marine observation state over StealthDiveInstance, LocationLayoutSystem, ResearchSystem, hydrophone, vessel position, sound propagation, species catalog, and population trend records.

**Smallest vertical slice:** Record one clean vocalization, classify it with confidence, associate it with a route/season, and publish one research entry.

**Acceptance gate:** Hydrophone condition, depth, salinity/temperature, background noise, vessel movement, call variation, species ambiguity, sample chain, and population inference are saved. Ten species require ten supported observations; a catalog does not prove population recovery without repeated surveys.

### [476] Photogrammetric Ruin Survey — PhotogrammetryPanel

**Core integration:** Add a survey/archive state over ExpeditionSystem, JournalSystem, ResearchSystem, camera calibration, image sets, feature points, uncertainty, storage, fictional ruin locations, and a bounded 2D point-cloud/plan-view renderer.

**Smallest vertical slice:** Capture a small stereo image set, calibrate baseline, triangulate a limited set of points, and archive a heritage record with provenance.

**Acceptance gate:** Baseline, lens calibration, overlap, lighting, motion, feature confidence, point density, scale, missing surfaces, storage, and source images are persisted. Five complete cities require actual expeditions and data volume. Do not introduce an unbounded 3D runtime solely for the panel.

### [477] Mechanical Difference Engine — DifferenceEnginePanel

**Core integration:** Add a mechanical-computation state over ResearchSystem, SilentFoundrySystem, JournalSystem, gear/column inventory, calibration, function tables, print media, and downstream navigation/artillery consumers.

**Smallest vertical slice:** Assemble a small validated column set, compute one polynomial/table interval, print results, and compare against reference values with error bounds.

**Acceptance gate:** Column count, gear tolerance, carry transfer, crank speed, friction, calibration, overflow, function approximation, paper, and operator time are authoritative. Ten-decimal tables require a verified numerical error model; the system cannot claim arbitrary mathematical accuracy from a display.

### [478] Direct Air Capture and Methanation — DacMethanationPanel

**Core integration:** Add a carbon-process state over WeatherSystem, shared power/utility authority, sorbent inventory, regeneration heat, hydrogen, Sabatier reactor, catalyst condition, methane storage, leakage, and emissions.

**Smallest vertical slice:** Capture one measured CO2 batch, regenerate sorbent, combine with valid hydrogen, produce and assay one methane batch, and store it safely.

**Acceptance gate:** Airflow, CO2 concentration, sorbent capacity, regeneration energy, hydrogen mass, catalyst conversion, water, pressure, methane purity, leakage, heat, and storage are saved. “Zero mining” does not mean zero energy or inputs. 100 cubic meters/day requires plant capacity and measured feedstock.

### [479] Great Survivor Epic — EpicPoemManuscriptPanel

**Core integration:** Add an authored long-form work over JournalSystem, GenerationalSuccessionEngine, NeedsSystem, SoundManager, writing duty, vellum/ink/gold materials, contributors, readings, and archive state.

**Smallest vertical slice:** Commission one section, record author/editor/illustrator provenance, complete a reading, and archive the manuscript.

**Acceptance gate:** Authorship, consent, themes, length, materials, writing time, contributor fatigue, revisions, preservation, audience, and reading event are persisted. The manuscript can create a bounded cultural effect and future knowledge record; it cannot grant a permanent morale trait to every dweller without an explicit effect rule.

### [480] Hydro-Powered Flour Mill and Rye Bakery — StoneMillBakeryPanel

**Core integration:** Extend RecipeCatalog/data, NeedsSystem, LocationLayoutSystem, water-flow/power contract, mill condition, grain inventory, flour extraction, sourdough culture, oven, storage, and ration distribution.

**Smallest vertical slice:** Install one water wheel and mill, grind a measured grain batch, bake one sourdough batch, and distribute it through the meal/ration system.

**Acceptance gate:** Flow, head, wheel efficiency, millstone gap, grain mass, extraction, bran/byproduct, culture, oven fuel, loaf size, spoilage, labor, and distribution are authoritative. Five hundred daily loaves require population, grain, flow, mill, bakery, and storage capacity; water power is not maintenance-free.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for educational materials, stone, projectiles, marine equipment, camera/film, crops, fiber, grain, hydrogen, sorbent, catalysts, fuel, water, power, manuscript media, and archive records.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, drift, miscalibration, equipment damage, maintenance, stale-data, and safety outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, flow_m3_s, concentration_ppm, concentration_bq_l, depth_m, error_radius_km, point_count, resolution_nm, bit_rate_bps, signal_db, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- education_course_defs.json
- siege_engine_defs.json
- marine_habitat_defs.json
- animation_production_defs.json
- nuclear_feasibility_defs.json
- road_layer_defs.json
- nettle_process_defs.json
- inertial_nav_defs.json
- forest_restoration_defs.json
- textile_loom_defs.json
- marine_bioacoustic_defs.json
- photogrammetry_profiles.json
- mechanical_compute_defs.json
- carbon_process_defs.json
- epic_manuscript_defs.json
- water_mill_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Lessons, laws, habitats, machines, recipes, vessels, discoveries, authored works, and effects must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

SanctuarySchoolPanel, TrebuchetSiegePanel, CoralReefSeedingPanel, AnimationStudioPanel, ThoriumFeasibilityPanel, RomanRoadPanel, NettleTowerPanel, InertialNavModal, MycorrhizalNetworkPanel, JacquardLoomPanel, WhaleAcousticPanel, PhotogrammetryPanel, DifferenceEnginePanel, DacMethanationPanel, EpicPoemManuscriptPanel, and StoneMillBakeryPanel.

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

- pupil safety/attendance/progression, siege physics/accuracy/damage, reef substrate/water conditions/coral survival, and film production/archive provenance;
- thorium feasibility assumptions/safety review, road layers/drainage/weather/maintenance, nettle mass/rennet/fiber reconciliation, and inertial drift/error growth;
- fungal inoculation/host compatibility/canopy growth, Jacquard loom setup/yarn/cloth, hydrophone classification, and acoustic population inference;
- photogrammetry calibration/triangulation/scale/uncertainty/storage, difference-engine carry/error/reference tables, and DAC energy/methane mass balance;
- poem authorship/materials/reading effects, water-wheel flow/mill extraction/loaf production/spoilage, and all consumer effects;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 465–480 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Education, ecology, engineering, clinical, industrial, radio, literary, and food-system claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
