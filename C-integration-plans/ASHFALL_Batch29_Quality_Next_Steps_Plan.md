# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 29)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 449–464

## Outcome

Batch 29 expands ASHFALL into mental health, governance, maritime trade, photographic memory, radiological waste, biodiversity, precision science, radio, and century archives. These features need new social and evidentiary state owners in addition to the existing survival and production systems.

Reuse active authorities:

- **GreenhouseSystem**, crop data, **GoodsCatalog**, and the meal/effect pipeline own apiary, crops, fertilizers, and food.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** own clinical status; a dedicated psychological-care contract is required for PTSD and trauma recovery.
- **StealthDiveInstance** owns existing submarine behavior; surface sailing requires a maritime vessel extension, not a second disconnected ocean runtime.
- **SilentFoundrySystem** and **CraftingSystem** own glass, metal, textiles, chemistry equipment, and production settlement.
- **RadiationSystem**, **MaterialShieldingSystem**, and the waste-repository contract own radiological material and containment.
- **DutyRosterSystem**, CaregivingSystem, **CrossingArbitrationSystem**, and the governance contract own work, care, diplomacy, and democratic decisions.
- **LocationLayoutSystem**, **TacticalCombatSystem**, and **WarlordDoctrineSystem** own harbor defenses and hostile encounters.
- **ResearchSystem**, **JournalSystem**, RadioHostSession, and **FactionRadioEngine** own measurement records, authored works, and communications.
- **GenerationalSuccessionEngine**, archive state, **EpilogueMatrix**, SoundManager, and **SaveChecksum** own long-lived culture and endings.

District8Accords, PowerGridPanel, FinalWishSystem, and CenturySeed should only be used as unlock or adapter names until their active Core contracts are confirmed.

## Batch entry gates

1. **Apiary and topical-medicine gate:** define queen lineage, hive health, royal-jelly yield, contamination, formulation, dosage, wound status, and clinical evidence.
2. **Psychological-care gate:** create canonical trauma/PTSD, survivor guilt, avoidance, sleep, therapy, consent, privacy, therapist skill, and relapse state. NeedsSystem alone is insufficient.
3. **Surface maritime gate:** extend StealthDiveInstance with surface vessels, wind, sail plan, cargo, crew, route, port, weather, and rescue.
4. **Photographic/archive gate:** define wet-plate chemicals, exposure, plate condition, portraits, identity, provenance, storage, and retrieval.
5. **Waste vitrification gate:** reuse repository state with isotope inventory, glass composition, melter, leach testing, residual waste, transport, and monitored storage.
6. **Governance gate:** define representatives, electorate, eligibility, quorum, ballot, amendment, rights, judicial review, law version, and save migration.
7. **Ecology and seed gate:** define seed identity/viability, soil/fertilizer mass balance, biodiversity, marine habitat, and ecological uncertainty.
8. **Scientific measurement gate:** unify gamma spectroscopy, SAED, optical and geophysical records with calibration, uncertainty, sample chain of custody, and research unlock provenance.
9. **Energy and harbor gate:** establish shared power generation/consumption and maritime security contracts before PV or mine-necklace UI.
10. **Century archive gate:** define authorship, signatures, constitution, photographs, DNA/seed records, time-capsule media, vault conditions, and ending criteria.

## Delivery order

1. **449, 453:** establish apiary medicine and waste-vitrification safety.
2. **450, 456:** create psychological-care and democratic governance authorities.
3. **451, 452:** extend surface maritime trade and photographic archives.
4. **455, 457, 460:** extend crops, seed biodiversity, and fertilizer inputs.
5. **454, 458, 459:** add cultural spaces, mechanical music, and radiological analysis.
6. **461, 462:** add harbor security and energy-generation contracts.
7. **463:** connect materials research to turbine/superalloy quality.
8. **464:** finish with the sealed historical archive and ending record.

## Step integration matrix

### [449] Apiary Queen Rearing and Royal Jelly — RoyalJellyPanel

**Core integration:** Add apiary colony/queen-lineage state over GreenhouseSystem, GoodsCatalog, CraftingSystem, MedicalSystem, wound status, storage, and the research/recipe registry.

**Smallest vertical slice:** Rear one valid queen cell, harvest a measured royal-jelly batch, formulate a topical product, and apply it to one eligible wound.

**Acceptance gate:** Colony health, queen genetics, graft success, cell count, harvest mass, contamination, formulation, shelf life, dosage, wound depth, sterility, and clinical response are saved. A healing effect is a bounded evidence-backed modifier; +60% acceleration and pharmaceutical equivalence are not defaults.

### [450] Psychological Trauma Counselling — PsychCounsellingModal

**Core integration:** Create a PsychologicalCareSystem or equivalent canonical status owner over MedicalSystem, NeedsSystem, survivor identity, CombatTraumaSystem, FinalWishSystem where relevant, therapist duty, private room, consent, and narrative records.

**Smallest vertical slice:** Assess one consenting survivor, assign one therapist, complete one CBT or narrative-exposure session, and update symptoms, coping, sleep, and follow-up.

**Acceptance gate:** Trauma source, symptom severity, consent, privacy, therapist skill, session time, fatigue, therapeutic approach, progress, relapse, and crisis escalation are persisted. Therapy can reduce symptoms or restore readiness gradually; it cannot erase PTSD instantly or force participation.

### [451] Three-Masted Sailing Barque — SailingBarquePanel

**Core integration:** Extend StealthDiveInstance’s maritime domain or create one shared SurfaceVesselState over LocationLayoutSystem, CrossingArbitrationSystem, MarketSystem, crew, sail plan, wind, cargo, ports, weather, and ocean expeditions.

**Smallest vertical slice:** Build one barque, assign crew, load a bounded manifest, sail one fictional route under wind conditions, and settle cargo and arrival state.

**Acceptance gate:** Hull, mast/sail condition, rigging, crew, wind angle, current, provisions, cargo mass, draft, route, storm, port access, damage, and rescue are saved. Zero fuel does not mean zero maintenance. A 200-ton voyage requires hull capacity, port, cargo, and route data.

### [452] Wet-Plate Collodion Portrait Studio — CollodionPhotoStudio

**Core integration:** Add a photographic artifact state over JournalSystem, DutyRosterSystem, CraftingSystem, chemical inventory, survivor identity, archive media, darkroom condition, and portrait provenance.

**Smallest vertical slice:** Prepare one glass plate, expose/develop/fix one portrait, assign subject and photographer, and archive the result.

**Acceptance gate:** Plate chemistry, exposure, light, camera condition, subject consent, development timing, fixer, defects, storage, identity, date, and duplicate handling are persisted. Founder portraits require actual historical subjects and records; the gallery cannot be completed by placeholder images or hard-coded names.

### [453] Radioactive Waste Vitrification — VitrificationPanel

**Core integration:** Extend the existing waste-repository state over RadiationSystem, MaterialShieldingSystem, hazardous inventory, borosilicate frit, joule-heated melter, molds, leach testing, transport, and monitored storage.

**Smallest vertical slice:** Select one eligible waste batch, melt and cast one glass log, assay its activity/leach behavior, and place it in a valid repository.

**Acceptance gate:** Isotope inventory, glass composition, temperature, redox/volatile handling, melter condition, log geometry, cooling, leach rate, residual waste, shielding, transport, and monitoring are saved. Ten-thousand-year retention is a configured durability model, not a guarantee; surface risk falls only for the contained batch.

### [454] Limestone Cave Chapel and Frescoes — CaveChapelPanel

**Core integration:** Add a cultural/facility state over DutyRosterSystem, LocationLayoutSystem, CaregivingSystem, JournalSystem, materials, construction safety, accessibility, and survivor participation.

**Smallest vertical slice:** Excavate one chamber, create one safe alcove, paint one provenance-recorded fresco, and schedule one voluntary gathering.

**Acceptance gate:** Excavation, ventilation, lighting, stone stability, labor safety, pigment/material stock, contributors, accessibility, attendance, and care effects are persisted. A chapel may provide bounded comfort or community participation; no permanent Spiritual Peace trait is applied to every dweller by a construction flag.

### [455] Vertical Ginseng and Root Tonics — GinsengTowerPanel

**Core integration:** Extend GreenhouseSystem with multi-year ginseng growth and connect extraction to GoodsCatalog, CraftingSystem, NeedsSystem, MedicalSystem, stress status, dose, storage, and research evidence.

**Smallest vertical slice:** Plant one tower, advance a configured growth interval, harvest a root batch, formulate a tonic, and distribute one dose to an eligible survivor.

**Acceptance gate:** Growth time, water, nutrients, light, root mass, ginsenoside proxy, extraction, contamination, dose, contraindications, adherence, and stress response are saved. An adaptogenic effect is not a permanent radiation/starvation/cold immunity trait without explicit clinical modeling.

### [456] Living Constitution and Democratic Assembly — ConstitutionAssemblyPanel

**Core integration:** Add a governance state over DutyRosterSystem, CrossingArbitrationSystem, survivor identity/eligibility, CaregivingSystem, law/rule data, event publication, and the existing event-bus seam. FinalWishSystem should only participate where a rights or succession rule requires it.

**Smallest vertical slice:** Register an electorate, elect representatives, draft one amendment, hold a validated ballot, and record the result and law version.

**Acceptance gate:** Eligibility, electorate, vacancies, quorum, ballot secrecy, abstention, majority threshold, amendment scope, rights, judicial review, dissent, term, and migration are persisted. A 2/3 vote ratifies only a valid amendment under the current constitution; the UI cannot create a First Republic without the required civic state.

### [457] Ancient Seed Library — SeedLibraryPanel

**Core integration:** Add seed-vault archaeology and viability state over GreenhouseSystem, ResearchSystem, CraftingSystem, expedition/location records, seed inventory, taxonomy, and crop catalogs.

**Smallest vertical slice:** Recover one seed lot, assay viability, catalog identity/provenance, grow a small test plot, and unlock one crop definition.

**Acceptance gate:** Vault condition, lot identity, species/variety, contamination, germination sample, genetic drift, researcher skill, test-plot inputs, and catalog references are saved. Five hundred catalogued varieties and fifty revived crops require actual records and successful assays, not count labels.

### [458] Mechanical Music Box Cylinder — MusicBoxCylinderPanel

**Core integration:** Extend JournalSystem, CraftingSystem, SoundManager, NeedsSystem, and cultural-event state with cylinder notes, comb tuning, program identity, performance, wear, and archive provenance.

**Smallest vertical slice:** Engrave one short cylinder, tune it, perform it in one venue, and record audience participation/effect.

**Acceptance gate:** Note layout, cylinder geometry, pin defects, comb tuning, tempo, mechanism condition, audio asset, performer/engraver, fatigue, and audience are persisted. Culture and morale effects are bounded event results; a permanent +20 culture score cannot be a direct UI write.

### [459] Gamma Spectroscopy — GammaSpectroscopyPanel

**Core integration:** Add a sample-analysis state over ResearchSystem, RadiationSystem, detector/cooling condition, calibration sources, sample chain of custody, isotope catalog, and narrative/faction evidence.

**Smallest vertical slice:** Acquire one sample, run a calibrated spectrum, identify one isotope band with confidence, and attach the result to a radiological report.

**Acceptance gate:** Detector resolution, cooling, energy calibration, background, geometry, counting time, shielding, sample activity, peak overlap, uncertainty, and reference database are saved. An isotope fingerprint can support an attribution hypothesis; it cannot prove a faction or reactor design without a separate evidence/encounter rule.

### [460] Guano Phosphate and Superphosphate — GuanoPhosphatePanel

**Core integration:** Extend GreenhouseSystem, MarketSystem, marine/coastal location, boat/expedition logistics, acid processing, GoodsCatalog, soil nutrient state, and crop yield.

**Smallest vertical slice:** Recover one guano load, process it into a measured fertilizer batch, apply it to one field, and resolve one crop cycle.

**Acceptance gate:** Guano mass, contamination, transport, acid, reactor condition, nutrient composition, granulation, soil capacity, runoff, worker exposure, and crop response are authoritative. A yield improvement is seasonal and field-specific; fertilizer cannot permanently double all farms.

### [461] Harbour Boom and Anti-Ship Mine Necklace — HarbourBoomPanel

**Core integration:** Extend LocationLayoutSystem, TacticalCombatSystem, StealthDiveInstance, surface-vessel state, harbor access, mine/chain inventory, friendly identification, safe passage, and maritime encounters.

**Smallest vertical slice:** Install one boom segment and one controlled defensive mine field, register a friendly passage gate, and resolve one vessel approach.

**Acceptance gate:** Mine position, arming state, chain condition, tide/current, harbor geometry, recognition, gate operator, clearance, maintenance, misidentification, and blast effects are persisted. The defense can deny or delay hostile entry; it cannot guarantee that a warship detonates or that the drydock is safe without a tactical outcome.

### [462] CdTe Thin-Film Solar Panels — ThinFilmSolarPanel

**Core integration:** Add a solar-generation state over ResearchSystem, SilentFoundrySystem, glass/deposition equipment, hazardous cadmium handling, shared power/grid contract, weather, panel condition, and maintenance.

**Smallest vertical slice:** Deposit one test panel, measure efficiency and leakage, install it in a valid surface location, and add its output to the power network.

**Acceptance gate:** Glass area, CdTe thickness, TCO, vacuum/deposition conditions, toxicity, encapsulation, efficiency, irradiance, temperature, shading, degradation, inverter/storage, and power demand are saved. Fifty square meters and 7.5 kW require the measured efficiency and sunlight; the grid cannot receive power through a panel UI flag.

### [463] TEM SAED Superalloy Mapping — TedsaedModal

**Core integration:** Extend ResearchSystem, SilentFoundrySystem, DutyRosterSystem, sample preparation, TEM/SAED observation, superalloy heat-treatment recipes, turbine blade state, and strength/condition consumers.

**Smallest vertical slice:** Prepare one alloy sample, acquire a diffraction pattern, index one phase, and produce a heat-treatment recommendation with confidence.

**Acceptance gate:** Sample thickness, beam condition, aperture, diffraction calibration, phase database, precipitate size, coherency estimate, operator skill, sample damage, heat-treatment parameters, and blade inspection are authoritative. A ten-year blade life is a downstream durability result requiring actual thermal/load testing.

### [464] Millennium Time Capsule — TimeCapsulePanel

**Core integration:** Add a sealed archival-vault state over GenerationalSuccessionEngine, JournalSystem, seed/DNA/photo/map/constitution records, SaveChecksum, materials, welding/seal, vault depth, monitoring, and EpilogueMatrix.

**Smallest vertical slice:** Select and verify one archival bundle, seal it in a capsule, bury it in a validated vault, and record a retrieval/provenance entry.

**Acceptance gate:** Item identity, consent/ownership, checksum, media condition, temperature, moisture, weld quality, vault stability, depth, duplicate preservation, and retrieval policy are persisted. A millennium ending is a configured narrative outcome after actual year/milestone criteria; the capsule cannot promise literal preservation for 1,000 years.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for bees, crops, chemicals, glass, radioactive waste, metals, stone, food, fertilizer, mines, power, cadmium, research samples, archive media, and cultural materials.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, relapse, ballot invalidation, misidentification, equipment damage, maintenance, stale-data, and negotiation outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, concentration_bq_kg, concentration_bq_l, activity_bq, dose_rate_uSv_h, abv_fraction, confidence_fraction, condition_fraction, healing_rate_fraction, vote_count, quorum_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- apiary_line_defs.json
- psychological_care_defs.json
- surface_vessel_defs.json
- photographic_process_defs.json
- waste_vitrification_defs.json
- cultural_facility_defs.json
- medicinal_crop_defs.json
- constitution_rules.json
- seed_library_defs.json
- music_cylinder_defs.json
- spectroscopy_profiles.json
- fertilizer_process_defs.json
- harbor_defense_defs.json
- thin_film_pv_defs.json
- saed_analysis_defs.json
- time_capsule_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Laws, rights, elections, clinical effects, isotopes, samples, crops, factions, vessels, records, and milestones must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

RoyalJellyPanel, PsychCounsellingModal, SailingBarquePanel, CollodionPhotoStudio, VitrificationPanel, CaveChapelPanel, GinsengTowerPanel, ConstitutionAssemblyPanel, SeedLibraryPanel, MusicBoxCylinderPanel, GammaSpectroscopyPanel, GuanoPhosphatePanel, HarbourBoomPanel, ThinFilmSolarPanel, TedsaedModal, and TimeCapsulePanel.

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

- queen lineage, royal-jelly yield, formulation, wound status, contamination, dosage, and clinical response;
- trauma assessment, consent/privacy, therapy progress, relapse, sleep, therapist skill, and save round-trip;
- sailing hull/rigging/cargo/wind/weather, port access, photographic plate process, portrait provenance, and archive retrieval;
- waste isotope/glass composition, melter, leach test, residual activity, repository monitoring, and time-capsule seal/chain of custody;
- constitution electorate/quorum/ballot/amendment/law migration, chapel safety/attendance, music-cylinder playback, and bounded culture effects;
- seed lot identity/viability, crop revival, fertilizer mass/yield, marine harbor hazards, friendly passage, mine misidentification, and route consequences;
- PV deposition, cadmium containment, irradiance/output/degradation, SAED indexing, heat-treatment recommendation, and sample provenance;
- gamma calibration/peak overlap/uncertainty, attribution evidence, archive checksum, vault conditions, milestone criteria, and replay;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 449–464 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Governance, psychology, medical, maritime, ecological, industrial, radio, and archival claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
