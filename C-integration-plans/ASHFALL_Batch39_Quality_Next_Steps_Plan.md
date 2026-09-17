# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 39)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 609–624

## Outcome

Batch 39 expands civic information, visual art, maritime craft, food resilience, metallurgy, rehabilitation, debate, field ecology, instrumentation, textiles, security, and cultural synthesis.

Reuse existing authorities:

- **JournalSystem**, governance state, **CrossingArbitrationSystem**, and the event/save contracts own journalism, debate, heritage, and public accountability.
- **SilentFoundrySystem**, **CraftingSystem**, **GreenhouseSystem**, recipe data, **GoodsCatalog**, and textile/material contracts own rope, steel, baking, leather, textiles, instruments, and charcoal.
- **MedicalSystem**, **DiseaseSystem**, **SurvivorNeedsState**, and the clinical effect pipeline own hydrotherapy, nutrition, hygiene, and rehabilitation.
- **StealthDiveInstance**, the surface-vessel extension, **LocationLayoutSystem**, and **ExpeditionSystem** own maritime/rope use, land enclosures, surveys, and route safety.
- **WeatherSystem**, the shared power/thermal/water contract, and sensor contracts own instruments, passive heating, and fermentation/environment conditions.
- **GenerationalSuccessionEngine**, SoundManager, cultural events, and **EpilogueMatrix** own the heritage index and long-lived cultural records.

PowerGridPanel, District8Accords, ShelterHazardLoop, and CenturySeed remain adapters or unlock names unless typed Core authorities are confirmed.

## Batch entry gates

1. **Free-press gate:** define source consent, editorial provenance, evidence, correction, defamation/privacy, publication, circulation, and governance response.
2. **Material/craft gate:** define beeswax, rope fiber, wire core, grain, steel, hides, charcoal, dyes, instruments, textiles, and exact conversion.
3. **Clinical hydrotherapy gate:** define water quality, temperature, contraindications, therapy plan, injury status, fatigue, and bounded recovery.
4. **Food/fermentation gate:** define cultures, contamination, pH, storage, dosage, disease-risk effects, alcohol, and meal integration.
5. **Security gate:** reuse perimeter sensors, night watch, tactical response, false alarms, maintenance, and uncertainty.
6. **Environmental instrument gate:** define calibration, range, drift, station coverage, weather, telemetry, and confidence.
7. **Child/culture gate:** define age-appropriate participation, consent, heritage ownership, authorship, accessibility, and generational archive.
8. **Thermal/utility gate:** resolve the shared power/water/heat contract before passive or spa panels.
9. **Heritage index gate:** compute from provenance-backed achievements, not hard-coded score components or a single victory switch.

## Delivery order

1. **609, 615:** establish journalism and deliberative culture.
2. **610, 620:** establish wax-art and textile-production records after the civic content contract.
3. **611, 612, 613:** build rope, food, and steel production dependencies.
4. **614, 616, 617:** add therapy, field infrastructure, and calibrated instruments.
5. **618, 619, 621:** add fermentation, security, and chemical by-products.
6. **622, 623:** add child science and linen.
7. **624:** finish the heritage index after source systems emit records.

## Step integration matrix

### [609] Newspaper and Investigative Journalism — NewspaperJournalismPanel

**Core integration:** Add an editorial/publication state over JournalSystem, CrossingArbitrationSystem, DutyRosterSystem, governance, source interviews, resource ledgers, evidence, corrections, and circulation.

**Smallest vertical slice:** Assign one story, collect one consented source, verify one ledger/event record, publish one issue, and route one finding to a governance review.

**Acceptance gate:** Source consent/privacy, evidence, editorial role, contradiction, correction, print resources, circulation, readership, retaliation/safety, and governance response are saved. A report can expose a real misallocation only if the cited records support it; no automatic 20% waste saving.

### [610] Encaustic Wax Painting — EncausticPaintingPanel

**Core integration:** Extend NeedsSystem, CraftingSystem, JournalSystem, beeswax/pigment inventory, heat safety, artist authorship, gallery placement, and cultural artifact records.

**Smallest vertical slice:** Prepare one pigment wax batch, paint and fuse one panel, archive its artist/title/date, and exhibit it in a valid venue.

**Acceptance gate:** Wax/pigment mass, temperature, panel condition, fusing, defects, ventilation, artist time, storage, and audience response are authoritative. Twenty panels become a milestone only when twenty valid artifacts exist; durability is a configured preservation rating.

### [611] Ropewalk and Rigging Guild — RopewalkGuildPanel

**Core integration:** Extend SilentFoundrySystem/CraftingSystem, DutyRosterSystem, StealthDiveInstance/surface vessels, cranes, construction, fiber inventory, wire core, rope geometry, and condition.

**Smallest vertical slice:** Spin one hemp/sisal batch, lay a specified rope diameter, inspect breaking load, and consume it in one rigging or lifting job.

**Acceptance gate:** Fiber length, moisture, twist, strand count, wire core, diameter, mass, strength, splice, wear, storage, and load are persisted. One thousand meters of rigging requires enough fiber/wire and line capacity; it cannot be infinite.

### [612] Cooperative Grain Mills and Communal Ovens — CommunalBakeryPanel

**Core integration:** Extend RecipeCatalog/data, NeedsSystem, DutyRosterSystem, GreenhouseSystem inputs, neighborhood facilities, grain storage, water, fuel, yeast, ovens, and distribution.

**Smallest vertical slice:** Establish one neighborhood mill/oven, assign a roster, grind/bake one batch, and distribute it to that quarter.

**Acceptance gate:** Grain mass, milling loss, flour, culture, oven heat, labor, maintenance, storage, ration allocation, and outage are saved. Five independent bakeries require five valid facilities and supply routes; resilience is measured after a simulated failure.

### [613] Fire-Clay Wootz and Damascus Blades — WootzDamascusPanel

**Core integration:** Extend SilentFoundrySystem, CraftingSystem, TacticalCombatSystem, heat treatment, crucible, carbon/alloy input, blade geometry, inspection, and tool/weapon condition.

**Smallest vertical slice:** Charge one crucible, produce one ingot, forge and heat-treat one blade, and measure hardness/toughness/edge retention.

**Acceptance gate:** Iron/carbon mass, crucible condition, temperature, soak, cooling, carbide pattern, forging skill, defects, hardness, toughness, and maintenance are authoritative. “Five times” edge retention is a measured item property, not a mythic global weapon bonus.

### [614] Swimming Pool and Hydrotherapy Spa — HydrotherapySpaPanel

**Core integration:** Add a therapy-facility state over MedicalSystem, NeedsSystem, shared power/water/heat, pool chemistry, therapist duty, injury status, exercise plan, hygiene, and recovery effects.

**Smallest vertical slice:** Treat one eligible survivor through one aquatic session, consume water/heat/capacity, and update a named rehabilitation status.

**Acceptance gate:** Temperature, minerals, sanitation, contraindications, injury, exercise, fatigue, therapist skill, session duration, and maintenance are saved. Recovery can improve under a defined protocol; no universal 40% reduction or weekly bonus without cohort/effect rules.

### [615] Debating Society — DebatingSocietyPanel

**Core integration:** Extend governance/Constitution state, CrossingArbitrationSystem, NeedsSystem, DutyRosterSystem, evidence records, motions, speakers, adjudication, and decision-quality metrics.

**Smallest vertical slice:** Register a motion, assign speakers, run timed arguments/rebuttals, score evidence and reasoning, and publish a recommendation to a governance decision.

**Acceptance gate:** Motion scope, speaker consent, evidence, timing, judge criteria, bias/conflict, audience, fatigue, and decision outcome are persisted. Debate quality may improve a decision confidence or civic skill; it cannot create a generation of superior leaders from one championship.

### [616] Dry-Stone Field Enclosures — DrystoneWallPanel

**Core integration:** Add a field-boundary state over LocationLayoutSystem, DutyRosterSystem, GreenhouseSystem/livestock, stone inventory, wall geometry, wind, gates, and maintenance.

**Smallest vertical slice:** Build one wall segment, install a gate, validate livestock containment, and measure one wind-shelter effect.

**Acceptance gate:** Stone mass, foundation, coursing, through-stones, cope, slope, gate, animal behavior, weather, collapse, repair, and access are saved. Ten kilometers and 30% wind reduction require actual field coverage and weather evidence; dry stone is not maintenance-free.

### [617] Glass Scientific Instrument Factory — ThermometerFactoryPanel

**Core integration:** Extend SilentFoundrySystem, CraftingSystem, WeatherSystem, calibration references, glass capillaries, mercury/alcohol handling, barometer/hygrometer definitions, and distribution.

**Smallest vertical slice:** Make one thermometer/barometer, calibrate it against a reference, assign uncertainty, and install it at one station.

**Acceptance gate:** Bore, fill, seal, material, reference bath/pressure, scale, drift, hazardous mercury, station location, telemetry, and maintenance are persisted. One hundred instruments require actual production and calibration records; “accurate everywhere” is not automatic.

### [618] Wild Fermentation Beverage Bar — WildFermentBeveragePanel

**Core integration:** Extend RecipeCatalog, NeedsSystem, GreenhouseSystem, GoodsCatalog, fermentation culture, grain/tea/honey/vegetable inputs, pH, contamination, alcohol, storage, and disease-risk effects.

**Smallest vertical slice:** Maintain one culture, ferment one beverage, assay it, serve a measured dose, and record an individual/cohort response.

**Acceptance gate:** Culture identity, substrate, pH, temperature, time, alcohol, contamination, dose, contraindications, storage, and adherence are saved. Probiotic effects are bounded and evidence-based; no colony-wide infection reduction without disease/exposure modeling.

### [619] Night Watch Patrol — NightWatchPatrolPanel

**Core integration:** Extend DutyRosterSystem, TacticalCombatSystem, LocationLayoutSystem, PIR/acoustic sensors, checkpoints, incident reports, lighting, fatigue, and response dispatch.

**Smallest vertical slice:** Schedule one patrol circuit, complete checkpoints, detect or miss one event, and produce an incident/response record.

**Acceptance gate:** Coverage, route, staffing, fatigue, visibility, lighting, sensor inputs, false alarms, response time, incident evidence, and maintenance are persisted. Three intercepted attempts can demonstrate a scenario but cannot prove complete perimeter security.

### [620] Mechanical Knitting Machine — CircularKnittingPanel

**Core integration:** Extend SilentFoundrySystem/CraftingSystem, DutyRosterSystem, NeedsSystem, yarn inventory, machine condition, garment patterns, warmth, fit, and distribution.

**Smallest vertical slice:** Load one yarn batch, knit one garment, inspect fit/thermal quality, and assign it to one survivor.

**Acceptance gate:** Yarn mass, gauge, row count, tension, breakage, machine wear, size, fit, warmth, repair, and inventory are authoritative. Clothing every colony member requires actual yarn, machine throughput, tailoring, and population counts.

### [621] Charcoal and Wood-Distillation Kilns — CharcoalBurningPanel

**Core integration:** Add a biomass thermal-processing state over District8Accords if it supplies permits, SilentFoundrySystem, NeedsSystem, wood/forest state, kiln, oxygen control, charcoal, vinegar, tar, methanol, storage, and hazardous handling.

**Smallest vertical slice:** Run one wood batch through a configured kiln/retort, measure charcoal and condensate outputs, and route each to a valid consumer.

**Acceptance gate:** Wood moisture/mass, temperature, residence time, venting, charcoal yield/quality, condensate composition, methanol toxicity, emissions, fire risk, and storage are saved. Imported solvent replacement requires compatibility tests; renewable wood is not infinite.

### [622] Children’s Science Discovery Trail — ScienceDiscoveryTrailPanel

**Core integration:** Extend GenerationalSuccessionEngine, NeedsSystem, LocationLayoutSystem, education, outdoor safety, specimen stations, weather shelter, and ResearchSystem literacy records.

**Smallest vertical slice:** Build one safe trail segment, assign an age-appropriate lesson, complete observations, and record assessment/knowledge gain.

**Acceptance gate:** Child eligibility/consent, route safety, weather, supervision, stations, specimen provenance, accessibility, fatigue, and assessment are persisted. Science literacy gains flow through education progression, not a direct future-research multiplier.

### [623] Flax Retting and Linen Mill — LinenWeavingMillPanel

**Core integration:** Extend GreenhouseSystem, CraftingSystem, SilentFoundrySystem, NeedsSystem, flax crop, retting water, scutching/hackling, linen quality, bandage/clothing recipes, and water contamination.

**Smallest vertical slice:** Harvest one flax batch, ret it, separate fiber, weave one bolt, and consume it in one valid clothing or medical recipe.

**Acceptance gate:** Stalk mass, retting time/water, temperature, fiber length, waste, contamination, yarn, weave, sterilization, and bolt length are saved. Two hundred meters and 5,000 sterile bandages require real crop and processing capacity.

### [624] Cultural Heritage Index — CulturalHeritageIndexPanel

**Core integration:** Add a provenance-based aggregation over GenerationalSuccessionEngine, JournalSystem, ResearchSystem, NeedsSystem, EpilogueMatrix, and all prior cultural artifact/event records.

**Smallest vertical slice:** Aggregate three domain records with evidence links, publish one versioned index, and unlock one milestone when criteria pass.

**Acceptance gate:** Source artifact/event IDs, domain weights, duplicates, missing data, quality, participation, time window, version, and milestone criteria are persisted. The index must explain its score and cannot trigger a grand victory from arbitrary maximum bars.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for wax, pigments, fiber, grain, yeast, metals, hides, water, food, leather, dyes, instruments, fuel, glass, medicine, and archive media.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, miscalibration, injury, false alarm, maintenance, stale-data, and governance outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, flow_m3_s, pH, redox_mv, acoustic_level_db, tensile_mpa, confidence_fraction, condition_fraction, vote_count, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- journalism_rules.json
- encaustic_process_defs.json
- rope_rigging_defs.json
- communal_bakery_defs.json
- wootz_process_defs.json
- hydrotherapy_defs.json
- debate_rules.json
- dry_stone_defs.json
- instrument_calibration_defs.json
- wild_fermentation_defs.json
- patrol_route_defs.json
- knitting_machine_defs.json
- charcoal_process_defs.json
- science_trail_defs.json
- linen_process_defs.json
- cultural_index_rules.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

NewspaperJournalismPanel, EncausticPaintingPanel, RopewalkGuildPanel, CommunalBakeryPanel, WootzDamascusPanel, HydrotherapySpaPanel, DebatingSocietyPanel, DrystoneWallPanel, ThermometerFactoryPanel, WildFermentBeveragePanel, NightWatchPatrolPanel, CircularKnittingPanel, CharcoalBurningPanel, ScienceDiscoveryTrailPanel, LinenWeavingMillPanel, and CulturalHeritageIndexPanel.

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

Add focused coverage for editorial provenance/corrections, production mass balance, therapy eligibility/outcomes, patrol false alarms, instrument calibration/drift, culture consent, linen sterility, and heritage-index source explainability, plus clean/tampered/missing-checksum/legacy/future-version/atomic-write saves.

## Definition of done

Each Step 609–624 must be implemented, explicitly deferred with a dependency, or rejected as unsupported. Implemented steps require Core ownership, typed Godot UI, schema-valid data, deterministic and checksummed persistence, dirty-store flushing, tunable claims, and the complete Godot-only verification pipeline.
