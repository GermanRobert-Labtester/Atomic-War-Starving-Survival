# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 42)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 657–672

## Outcome

Batch 42 expands seasonal culture, interior arts, cooperative housing, cartography education, brewing, child development, refugee support, prosthetics, lace, epidemiology, preserves, animal communications, woodland management, calligraphy, passive cooling, and the final documentary archive.

Reuse:

- **GenerationalSuccessionEngine**, NeedsSystem, RecipeCatalog/data, JournalSystem, and cultural-event state for festivals, children, toys, documents, and documentary records.
- **CraftingSystem**, **SilentFoundrySystem**, GreenhouseSystem, GoodsCatalog, and textile/food contracts for wallpaper, ale, toys, lace, preserves, and coppice products.
- **DutyRosterSystem**, **LocationLayoutSystem**, and the civil-worksite contract for housing, schools, sanitation, cooling, and woodland.
- **CrossingArbitrationSystem**, MedicalSystem, DiseaseSystem, and refugee/clinical intake contracts for resettlement and outbreak response.
- **ExpeditionSystem**, RadioHostSession, LocationLayoutSystem, and animal/communication state for pigeons.
- **ResearchSystem**, **EpilogueMatrix**, SoundManager, archive state, and SaveChecksum for cartography and the documentary ending.

PowerGridPanel, ShelterHazardLoop, District8Accords, and CenturySeed remain adapter/unlock names unless typed authorities are established.

## Batch entry gates

1. **Seasonal festival gate:** define calendar, weather, consent, fire safety, food, participation, accessibility, and repeatable cultural effects.
2. **Interior/craft gate:** define wallpaper block/pigment, materials, room effects, toys, lace, calligraphy, and provenance.
3. **Housing gate:** define land rights, materials, labor, inspection, utilities, occupancy, safety, and ownership.
4. **Cartography education gate:** define curriculum, survey data, map standards, assessment, versioning, and transfer.
5. **Brewing gate:** define hops, yeast bank, alpha acids, mash/lautering, ABV, contamination, storage, and trade.
6. **Child development gate:** define age, guardianship, play safety, developmental observations, and privacy.
7. **Refugee/prosthetic gate:** define informed intake, capacity, screening, skills, consent, fitting, rehabilitation, and non-discrimination.
8. **Epidemiology gate:** extend DiseaseSystem with case definitions, contacts, isolation, testing, reporting delay, privacy, and interventions.
9. **Ecology/animal gate:** define pigeons, coppice, pollination/crops, regeneration, disease, and carrying capacity.
10. **Passive utility gate:** unify earth-pipe cooling with thermal/air quality, maintenance, and climate load.
11. **Documentary gate:** source-only archive assembly, consent/redaction, narration, date coverage, replay, and ending criteria.

## Delivery order

1. **657, 658, 659:** establish festival, interior arts, and cooperative housing.
2. **660, 661, 662:** add cartography, brewing, and child play.
3. **663, 664:** add refugee intake and prosthetic care.
4. **665, 666, 667:** extend lace, epidemiology, and food preservation.
5. **668, 669, 670:** add pigeons, coppice, and calligraphy.
6. **671, 672:** complete passive cooling and the documentary finale.

## Step integration matrix

### [657] Midwinter Solstice Festival — MidwinterFestivalPanel

**Core integration:** Add a calendar-bound cultural event over NeedsSystem, RecipeCatalog/data, GenerationalSuccessionEngine, venue/fire/weather, food, lantern/bonfire resources, SoundManager, and social effects.

**Smallest vertical slice:** Schedule one solstice event, build a safe fire/lantern route, serve one menu, and record participation/effect.

**Acceptance gate:** Calendar, weather, fuel, fire safety, food, accessibility, attendance, fatigue, alcohol, and emergency response are persisted. A festival can create a large bounded cohesion event; “every member” and deepest belonging require actual participation.

### [658] Block-Printed Wallpaper — WallpaperPrintStudioPanel

**Core integration:** Extend CraftingSystem, NeedsSystem, LocationLayoutSystem, paper/cloth, blocks, mineral pigments, binders, room finish, installation, and maintenance.

**Smallest vertical slice:** Carve one block, print one repeat panel, install it in one room, and update its comfort/beauty read model.

**Acceptance gate:** Block geometry, registration, ink, substrate, drying, coverage, defects, room condition, lighting, maintenance, and occupant response are saved. Maximum living quality requires coverage and measured effect, not a wallpaper completion flag.

### [659] Cooperative Self-Build Housing — SelfBuildHousingPanel

**Core integration:** Add a housing-project state over DutyRosterSystem, LocationLayoutSystem, NeedsSystem, land rights, materials, family/group consent, builder supervision, inspections, utilities, occupancy, and maintenance.

**Smallest vertical slice:** Form one cooperative group, approve a site/design, complete the foundation/wall/utility milestones, inspect it, and assign occupancy.

**Acceptance gate:** Land rights, design, material mass, labor, safety, weather, inspection, utility capacity, accessibility, ownership/equity, and maintenance are persisted. Homeowner pride is a bounded social effect; a family cannot receive a permanent trait before a safe completed home exists.

### [660] Cartography School — CartographyAcademyPanel

**Core integration:** Extend ResearchSystem, DutyRosterSystem, LocationLayoutSystem, education, survey/atlas data, map projections, symbols, reproduction, assessment, and cartographer progression.

**Smallest vertical slice:** Enroll one student, teach one projection/symbol module, assess one map, and publish a corrected tile.

**Acceptance gate:** Curriculum, teacher, source data, projection, scale, symbols, error, assessment, fatigue, and map version are saved. Graduates improve compilation under a named skill modifier; speed and accuracy cannot simply double without measured progression.

### [661] Craft Ale Brewery — CraftAleBreweryPanel

**Core integration:** Extend RecipeCatalog/data, GreenhouseSystem, NeedsSystem, GoodsCatalog, yeast-bank state, hops, alpha-acid assay, mash/lauter/whirlpool, fermentation, storage, and MarketSystem.

**Smallest vertical slice:** Grow/harvest one hop batch, brew one ale with a preserved yeast strain, assay it, and record one variety.

**Acceptance gate:** Hop mass/alpha acid, grain, water, yeast identity, mash, boil, fermentation, ABV, contamination, vessel, storage, taste, and trade are persisted. Six varieties require six valid recipes and batches, not labels.

### [662] Wooden Toy Workshop — WoodenToyWorkshopPanel

**Core integration:** Extend GenerationalSuccessionEngine, NeedsSystem, CraftingSystem, child development, safety, materials, toy identity, and play sessions.

**Smallest vertical slice:** Craft one age-appropriate toy, safety-inspect it, assign it to a consenting child, and record a play/development observation.

**Acceptance gate:** Age, guardianship, material toxicity/splinters, design, skill, play time, supervision, accessibility, loss, and developmental evidence are saved. Child happiness and development are observed/aggregated, not set to maximum for every child.

### [663] Refugee Intake and Resettlement — RefugeeResettlementPanel

**Core integration:** Add an intake/resettlement state over CrossingArbitrationSystem, NeedsSystem, MedicalSystem, DutyRosterSystem, shelter capacity, skills, family identity, consent, privacy, education, and faction relations.

**Smallest vertical slice:** Receive one group, conduct informed intake/medical screening, allocate temporary shelter/food, match one skill/work role, and record integration support.

**Acceptance gate:** Identity uncertainty, consent, family links, health, trauma, disability, capacity, food, shelter, work choice, language, security, privacy, and resettlement outcome are persisted. Fifty successful integrations require actual capacity and support; no population growth or specialist skills are granted by count alone.

### [664] Prosthetic Limb Workshop — ProstheticLimbPanel

**Core integration:** Add a prosthetic fitting/rehabilitation state over MedicalSystem, SilentFoundrySystem, DutyRosterSystem, injury/amputation status, socket casting, materials, body-powered/myoelectric components, therapy, and work roles.

**Smallest vertical slice:** Assess one amputee, manufacture and fit one limb, calibrate control, and complete a rehabilitation milestone.

**Acceptance gate:** Residual limb, socket fit, pain, skin, component condition, alignment, control signal, power, therapy, fatigue, adaptation, work compatibility, and maintenance are saved. Twenty people returning to work requires twenty valid fittings and individual outcomes; “full productivity” is not automatic.

### [665] Bobbin and Needle Lace Studio — LacemakingStudioPanel

**Core integration:** Extend CraftingSystem, NeedsSystem, DutyRosterSystem, textile inventory, pattern, bobbins, thread, time, quality, ceremonial garments, and diplomatic gift provenance.

**Smallest vertical slice:** Select one pattern, complete one lace section, inspect it, and add it to a valid garment/gift.

**Acceptance gate:** Thread mass, pattern complexity, tension, worker time, defects, quality, repair, garment fit, and provenance are persisted. Diplomatic value changes through MarketSystem/faction negotiation, not a fixed “most treasured” flag.

### [666] Epidemiology Unit — EpidemiologySurveillancePanel

**Core integration:** Extend MedicalSystem/DiseaseSystem, DutyRosterSystem, ShelterHazardLoop if confirmed, case definitions, testing/observation, contact tracing, isolation, treatment, privacy, and event reporting.

**Smallest vertical slice:** Detect a defined cluster, create cases/contacts, isolate or monitor eligible people, and close one investigation.

**Acceptance gate:** Disease, symptom onset, case definition, test sensitivity, reporting delay, contact network, isolation capacity, compliance, privacy, treatment, and outcome are saved. A seven-day typhoid containment is a scenario target requiring actual detection/intervention; no disease rate is reset directly.

### [667] Cooperative Fruit Preserves — PreserveKitchenPanel

**Core integration:** Extend RecipeCatalog/data, NeedsSystem, GreenhouseSystem, fruit inventory, pectin, sugar, heat, Brix, jar/headspace, seal, storage, spoilage, and ration distribution.

**Smallest vertical slice:** Process one fruit batch, reach a validated gel point, seal jars, inspect vacuum, and add them to a dated stockpile.

**Acceptance gate:** Fruit mass, sugar, pectin, water, heat, Brix, acidity, jar, headspace, seal, contamination, storage, shelf life, and distribution are authoritative. Two thousand jars require the harvested fruit and kitchen capacity.

### [668] Homing Pigeon Messenger Corps — PigeonMessengerCorpsPanel

**Core integration:** Add an animal-message state over RadioHostSession fallback, LocationLayoutSystem, DutyRosterSystem, loft/breeding, training, route, weather, message capsule, welfare, and delivery logs.

**Smallest vertical slice:** Train one pigeon to one known fictional station, dispatch a small message during a radio outage, and record arrival or loss.

**Acceptance gate:** Bird health, pair/lineage, training distance, route, weather, predator risk, capsule, message identity, welfare, arrival time, and loss are saved. Pigeons are not immune to all electronic warfare or weather and cannot guarantee delivery.

### [669] Coppice Woodland Management — CoppiceWoodlandPanel

**Core integration:** Extend LocationLayoutSystem, GreenhouseSystem, forest/coppice, District8Accords if it supplies permits, rotation, stool regrowth, species, biodiversity, charcoal/pole/hurdle outputs, and certification.

**Smallest vertical slice:** Register one coupe, harvest a valid rotation, process poles/charcoal, and schedule regrowth.

**Acceptance gate:** Species, age, rotation, stool health, soil, rainfall, wildlife, harvest mass, products, fire, disease, and regrowth are persisted. “Forever” supply requires sustainable rotation and can fail under drought, pests, or overharvest.

### [670] Calligraphy School — CalligraphySchoolPanel

**Core integration:** Extend JournalSystem, DutyRosterSystem, CraftingSystem, document identity, ink/paper/vellum, scripts, student progression, official versions, and ceremonial records.

**Smallest vertical slice:** Train one calligrapher, copy one constitutional/legal article, verify it against the source, and register the ceremonial edition.

**Acceptance gate:** Script, source, copy accuracy, pen/brush, ink, paper, time, student, reviewer, corrections, version, and document authority are saved. Ceremonial prestige is a read model; legal force remains in the authoritative law record.

### [671] Passive Earth-Pipe Cooling — EarthPipeCoolingPanel

**Core integration:** Add a passive air/thermal state over shared utility, WeatherSystem, LocationLayoutSystem, intake quality, buried pipe geometry, ground temperature, airflow, condensation, mold, and NeedsSystem comfort.

**Smallest vertical slice:** Install one pipe loop, simulate one summer cycle, measure air temperature/pressure drop/condensation, and update one room.

**Acceptance gate:** Depth, length, diameter, soil temperature, outdoor air, airflow, heat exchange, condensation, mold, leakage, maintenance, and room load are persisted. Passive cooling reduces demand under valid conditions; it cannot guarantee 18°C everywhere or zero energy.

### [672] 100-Year Documentary — DocumentaryFilmPanel

**Core integration:** Extend GenerationalSuccessionEngine, JournalSystem, archive photos/art/audio, animation/film state, consent/redaction, narration, amphitheatre, SoundManager, EpilogueMatrix, and SaveChecksum.

**Smallest vertical slice:** Assemble one source-verified chapter, record narration, screen it to a valid audience, and save the documentary version/provenance.

**Acceptance gate:** Source records, contributor consent, redactions, date coverage, image/audio rights, narration, edit version, screening, audience, and milestone criteria are persisted. The final ending requires actual century/history coverage; it cannot trigger from an empty timeline or arbitrary progress bar.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for food, paper, pigments, building materials, beer ingredients, toys, medical components, lace thread, fruit, jars, birds, wood, ink, cooling materials, and archive media.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, relapse, injury, disease, outage, maintenance, stale-data, and integration outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, flow_m3_s, speed_kmh, humidity_fraction, concentration_bq_l, confidence_fraction, condition_fraction, vote_count, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- seasonal_festival_defs.json
- wallpaper_process_defs.json
- housing_project_defs.json
- cartography_course_defs.json
- craft_ale_defs.json
- child_play_defs.json
- refugee_intake_defs.json
- prosthetic_defs.json
- lace_process_defs.json
- epidemiology_rules.json
- preserve_process_defs.json
- pigeon_messenger_defs.json
- coppice_rotation_defs.json
- calligraphy_defs.json
- earth_pipe_defs.json
- documentary_rules.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

MidwinterFestivalPanel, WallpaperPrintStudioPanel, SelfBuildHousingPanel, CartographyAcademyPanel, CraftAleBreweryPanel, WoodenToyWorkshopPanel, RefugeeResettlementPanel, ProstheticLimbPanel, LacemakingStudioPanel, EpidemiologySurveillancePanel, PreserveKitchenPanel, PigeonMessengerCorpsPanel, CoppiceWoodlandPanel, CalligraphySchoolPanel, EarthPipeCoolingPanel, and DocumentaryFilmPanel.

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

Add focused coverage for festival safety/attendance, wallpaper coverage, housing inspection/utilities, cartography assessment, ale chemistry, child safety, refugee consent/capacity, prosthetic fit/rehab, lace quality, epidemiology contacts/isolation, preserves/seals, pigeon welfare/delivery, coppice rotation, calligraphy accuracy, passive cooling/condensation, documentary provenance, and clean/tampered/missing-checksum/legacy/future-version/atomic-write saves.

## Definition of done

Each Step 657–672 must be implemented, explicitly deferred with a dependency, or rejected as unsupported. Implemented steps require Core ownership, typed Godot UI, schema-valid data, deterministic and checksummed persistence, dirty-store flushing, tunable claims, and the complete Godot-only verification pipeline.
