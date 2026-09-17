# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 40)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 625–640

## Outcome

Batch 40 adds beverage culture, percussion, heritage art, distributed weather telemetry, subterranean adventure, sanitation, satire, mechanical computation, horticulture, basketry, astronomy, animal breeding, communications, and the complete knowledge archive.

Reuse:

- **RecipeCatalog/data**, GoodsCatalog, GreenhouseSystem, NeedsSystem, and CraftingSystem for beverages, crops, baskets, soapstone, and food.
- **JournalSystem**, SoundManager, cultural-event state, governance, and ResearchSystem for art, satire, astronomy, computation, and encyclopedic records.
- **WeatherSystem**, RadioHostSession, LocationLayoutSystem, and sensor contracts for wind telemetry.
- **ExpeditionSystem**, StealthDiveInstance, LocationLayoutSystem, and surface-vessel state for caves, horses, pigeons, pirogues, and hydrographic work.
- **MedicalSystem**, DiseaseSystem, DutyRosterSystem, and the sanitation contract for laundry.
- **MarketSystem**, CrossingArbitrationSystem, and the governance/ledger contract for beverage trade and public works.
- **GenerationalSuccessionEngine** and **EpilogueMatrix** for cultural milestones.

## Batch entry gates

1. Beverage/tasting gate: quality descriptors, provenance, alcohol safety, storage, price, and cultural effects.
2. Music/art gate: instrument materials, audio assets, performer participation, archive rights, and bounded event effects.
3. Weather telemetry gate: station calibration, wind vectors, gusts, radio latency, missing data, and forecast confidence.
4. Cave/wilderness gate: route hazards, equipment, exposure, supervision, rescue, and discovery uncertainty.
5. Sanitation gate: laundry water/heat/soap, disease transmission, household allocation, and effect evidence.
6. Computing gate: mechanical state transitions, tape/program identity, correctness, step limits, and research unlocks.
7. Ecology/garden gate: crop interactions, willow regeneration, horse/beekeeping biology, and land carrying capacity.
8. Communications gate: pigeon welfare/route, wired/electronic fallback, outage, and message provenance.
9. Power/thermal/water gate: soapstone, VAWT, district heating, and fountains use measured utility state.
10. Encyclopedia gate: source records, cross-references, versioning, missing data, authorship, and finale criteria.

## Delivery order

1. **625, 626, 627:** beverage, music, and heritage art foundations.
2. **628, 629, 630:** wind, caves, and sanitation.
3. **631, 632:** satire and mechanical computation.
4. **633, 634, 636:** horticulture and craft materials.
5. **635, 637, 638, 639:** astronomy, horses, bees, and river transport.
6. **640:** complete the encyclopedic synthesis.

## Step integration matrix

### [625] Sommelier Guild — SommelierGuildPanel

**Core integration:** Extend RecipeCatalog/data, NeedsSystem, DutyRosterSystem, GoodsCatalog, MarketSystem, tasting records, vintage provenance, alcohol safety, and cultural events.

**Smallest vertical slice:** Train one taster, evaluate one beverage under a structured rubric, archive a vintage record, and update a trade/cultural read model.

**Acceptance gate:** Beverage identity, batch, age, aroma/flavor descriptors, serving, bias, taster skill, storage, price evidence, and alcohol risk are saved. A threefold value change requires market transactions or a defined price rule; it cannot be a prestige-meter write.

### [626] Drum Circle and Percussion Ensemble — DrumCirclePanel

**Core integration:** Extend NeedsSystem, CraftingSystem, DutyRosterSystem, SoundManager, cultural-event state, instruments, rhythm programs, venue, and audience.

**Smallest vertical slice:** Craft/tune one drum, schedule a session, include consenting participants, play one pattern, and record attendance/effect.

**Acceptance gate:** Shell/skin materials, tuning, rhythm complexity, performer fatigue, volume, venue, accessibility, audience, and audio are persisted. One hundred participants and the strongest bonding event require actual participation and comparison with prior events.

### [627] Cave Painting Revival — CavePaintingPanel

**Core integration:** Add a heritage/art study over JournalSystem, NeedsSystem, ResearchSystem, cave location, pigment, conservation, source-community/heritage consent, and site access.

**Smallest vertical slice:** Document one existing art site without damaging it, create one new painting in an approved chamber, and archive both provenance records.

**Acceptance gate:** Site condition, age/uncertainty, pigment, artist, conservation, access, lighting, contamination, and cultural permissions are saved. The Unbroken Thread milestone requires valid historic and new-art records; do not assert a real-world age without in-game evidence.

### [628] Wind Vane and Anemometer Network — WindTelemetryPanel

**Core integration:** Extend WeatherSystem, RadioHostSession, LocationLayoutSystem, instrument calibration, wind-vector observations, station maintenance, and forecast/energy consumers.

**Smallest vertical slice:** Install three stations, collect wind speed/direction/gust samples, transmit them, and update a confidence-rated local wind map.

**Acceptance gate:** Station position/elevation, vane/anemometer condition, calibration, gust sampling, radio latency, missing data, terrain, and storm state are persisted. Twenty stations, three turbine sites, and six-hour warnings require coverage and forecast validation.

### [629] Subterranean River Kayaking — CaveKayakingPanel

**Core integration:** Extend ExpeditionSystem, LocationLayoutSystem, NeedsSystem, equipment condition, cave hydrology, route hazards, headlamps/ropes, rescue, and discovery records.

**Smallest vertical slice:** Plan one supervised route, pass equipment checks, navigate one river segment, and record surveyed map/progress or incident.

**Acceptance gate:** Flow, depth, rapids, waterfalls, visibility, temperature, supplies, skill, fatigue, rope/headlamp condition, rescue route, and unknown passages are saved. Ten kilometers and two chambers require actual surveyed segments and cannot bypass hazards.

### [630] Cooperative Laundry and Soap Factory — CoopLaundryPanel

**Core integration:** Add a sanitation service over NeedsSystem, DutyRosterSystem, MedicalSystem, DiseaseSystem, water/heat/power, soap inventory, household allocation, bedding/clothing, and outbreak state.

**Smallest vertical slice:** Process one household batch, consume water/heat/soap, update garment cleanliness, and measure one disease-exposure effect.

**Acceptance gate:** Load, water temperature, soap, agitation, mangle, drying, household priority, contamination, disease transmission, worker safety, and outages are persisted. A 35% reduction requires epidemiological evidence and coverage; weekly service cannot happen without capacity.

### [631] Satirical Broadsheet — SatiricalBroadsheetPanel

**Core integration:** Extend JournalSystem, CrossingArbitrationSystem, NeedsSystem, print/craft resources, editorial/source rules, governance response, consent/privacy, and public circulation.

**Smallest vertical slice:** Publish one satirical issue with an attributed editorial, circulate it, record audience response, and route one policy reflection.

**Acceptance gate:** Authorship, subject consent/privacy, satire/content tags, print run, audience, criticism, retaliation, correction, and policy response are saved. Satire can improve accountability or morale; it cannot guarantee laughter or a specific policy change.

### [632] Mechanical Universal Turing Machine — TuringMachinePanel

**Core integration:** Add a mechanical-computation state over ResearchSystem, SilentFoundrySystem, JournalSystem, tape/program records, symbol alphabet, transition table, step counter, wear, and output archive.

**Smallest vertical slice:** Load a finite prime-sieve tape, execute within a bounded step limit, verify output against a reference, and archive the program/result.

**Acceptance gate:** Tape identity, transition rules, head position, state, carry, step count, friction, jam, input/output, correctness, and research provenance are persisted. One successful program demonstrates the implemented machine profile; it does not automatically unlock every computer-science result.

### [633] Horticultural Society Flower Show — FlowerShowPanel

**Core integration:** Extend GreenhouseSystem, NeedsSystem, DutyRosterSystem, crop quality, exhibits, judges, event attendance, medals, and research/crop-quality progression.

**Smallest vertical slice:** Register one exhibit, score it under a defined rubric, award a medal, and record one grower learning effect.

**Acceptance gate:** Variety, genetics, growing conditions, size/color/quality measurements, judge bias, exhibit condition, attendance, and awards are saved. Fifty exhibitors and improved average yield require actual entries and a tested progression effect.

### [634] Basket and Willow Wicker Guild — BasketWeavingPanel

**Core integration:** Extend CraftingSystem, NeedsSystem, DutyRosterSystem, LocationLayoutSystem, willow coppice, soaking/seasoning, patterns, storage/fish-trap/furniture recipes, and durability.

**Smallest vertical slice:** Harvest and soak willow, weave one container, inspect load capacity, and use it in one storage recipe.

**Acceptance gate:** Willow mass, harvest rotation, moisture, split quality, pattern, labor, breakage, capacity, repair, and storage are authoritative. Five hundred containers require sufficient renewable stock and workshop throughput.

### [635] Astronomy Club and Stargazing — AstronomyClubPanel

**Core integration:** Extend NeedsSystem, JournalSystem, ResearchSystem, observatory/telescope condition, weather/cloud/fallout, observation catalog, sketch/archive, and wonder/comfort effects.

**Smallest vertical slice:** Schedule one public night, observe one valid object, record a sketch/data entry, and apply a bounded event effect.

**Acceptance gate:** Sky visibility, telescope calibration, object position, observer skill, session attendance, lighting, fatigue, and weather are saved. Twelve consecutive nights require twelve completed valid sessions; existential-wellbeing improvement is measured, not a direct global reset.

### [636] Soapstone Vessels — SoapstoneCarvingPanel

**Core integration:** Extend CraftingSystem, NeedsSystem, LocationLayoutSystem, quarry/sample data, vessel recipes, thermal properties, oil lamps, and meal quality.

**Smallest vertical slice:** Quarry one block, carve one pot, test thermal retention/condition, and use it in one meal.

**Acceptance gate:** Block mass, hardness, carving time, wall thickness, cracks, thermal capacity, heat, safety, capacity, and fuel effect are persisted. A vessel can reduce modeled fuel use; no universal 20% reduction without cooking tests.

### [637] Horse Breeding and Cavalry Scouts — CavalryScoutPanel

**Core integration:** Add horse/breeding state over ExpeditionSystem, DutyRosterSystem, TacticalCombatSystem, LocationLayoutSystem, animal needs, pedigree, training, route, and mounted reconnaissance.

**Smallest vertical slice:** Register a breeding pair, train one horse/scout, run one route, and report speed, fatigue, feed, and detection.

**Acceptance gate:** Pedigree, temperament, health, feed, terrain, rider skill, weather, route, injury, animal care, and resupply are saved. A 150-kilometer day is a configured profile, not a guarantee; a 200-kilometer warning requires actual route and communications.

### [638] Migratory Beekeeping Cooperative — MigratoryBeekeepingPanel

**Core integration:** Extend GreenhouseSystem, apiary/queen state, CrossingArbitrationSystem, DutyRosterSystem, MarketSystem, hive transport, bloom calendar, contracts, and allied crop outcomes.

**Smallest vertical slice:** Move one hive cohort to one flowering site, complete a pollination contract, return/inspect the hives, and settle payment/yield.

**Acceptance gate:** Hive health, queen, transport, weather, bloom timing, forage, disease, route, contract, crop response, and loss are persisted. Five settlements and increased yields require five valid contracts and evidence.

### [639] Dugout Pirogue — DugoutPiroguePanel

**Core integration:** Extend surface-vessel state over ExpeditionSystem, LocationLayoutSystem, StealthDiveInstance/maritime, tree/log inventory, construction, river route, stability, cargo, and weather.

**Smallest vertical slice:** Select a suitable trunk, hollow/finish one canoe, test stability/load, and navigate one river segment.

**Acceptance gate:** Trunk mass, species, dimensions, carving, fire hollowing, hull condition, stability, load, crew, flow, hazards, portage, and repair are saved. Five hundred kilometers require surveyed route segments and cannot be unlocked by fleet count alone.

### [640] Complete Encyclopaedia of Ashfall — AshfallEncyclopaediaPanel

**Core integration:** Add a compiled-reference state over GenerationalSuccessionEngine, JournalSystem, ResearchSystem, authored records, cross-references, people/privacy, versioning, archive media, EpilogueMatrix, and SaveChecksum.

**Smallest vertical slice:** Compile one volume from verified records, resolve links, record editors/illustrators, publish it, and archive the source snapshot.

**Acceptance gate:** Entry provenance, inclusion permissions, redactions, references, duplicate handling, missing data, version, page/volume, contributors, checksum, and publication milestone are persisted. The ending triggers only when the actual completeness criteria pass; it cannot be a score-bar shortcut.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for beverages, instruments, pigments, rope fiber, grain, steel, hides, soap, textiles, charcoal, sensors, food, animals, boats, archives, and publication materials.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, miscalibration, injury, outage, false alarm, route, contract, and governance outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, flow_m3_s, speed_kmh, depth_m, signal_db, confidence_fraction, condition_fraction, balance_units, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- beverage_tasting_defs.json
- percussion_event_defs.json
- heritage_art_defs.json
- wind_station_defs.json
- cave_route_defs.json
- sanitation_service_defs.json
- satire_publication_defs.json
- mechanical_compute_defs.json
- horticulture_show_defs.json
- wicker_process_defs.json
- astronomy_session_defs.json
- soapstone_process_defs.json
- horse_breeding_defs.json
- pollination_contract_defs.json
- pirogue_defs.json
- encyclopedia_rules.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

SommelierGuildPanel, DrumCirclePanel, CavePaintingPanel, WindTelemetryPanel, CaveKayakingPanel, CoopLaundryPanel, SatiricalBroadsheetPanel, TuringMachinePanel, FlowerShowPanel, BasketWeavingPanel, AstronomyClubPanel, SoapstoneCarvingPanel, CavalryScoutPanel, MigratoryBeekeepingPanel, DugoutPiroguePanel, and AshfallEncyclopaediaPanel.

Each panel must bind to typed host sessions/read models, expose loading/empty/error states, show units and confidence where relevant, and issue validation-returning commands. No Bind(object), placeholder arrays, direct Core mutation, hard-coded success, or unsupported 3D-only dependency is permitted. New dirty stores must be wired through Main.Setup, Main.Save, and Main.Flush.

## Verification plan

Run focused tests after each vertical slice, then:

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

Add focused coverage for beverage quality/market, instruments/audio, heritage consent, wind telemetry, cave safety, sanitation transmission, satire corrections, Turing correctness, garden/animal/boat yields, and encyclopedia provenance, plus clean/tampered/missing-checksum/legacy/future-version/atomic-write saves.

## Definition of done

Each Step 625–640 must be implemented, explicitly deferred with a dependency, or rejected as unsupported. Implemented steps require Core ownership, typed Godot UI, schema-valid data, deterministic and checksummed persistence, dirty-store flushing, tunable claims, and the complete Godot-only verification pipeline.
