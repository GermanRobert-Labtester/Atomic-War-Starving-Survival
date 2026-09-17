# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 34)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 529–544

## Outcome

Batch 34 should connect demographic evidence, legal institutions, artillery, botanical medicine, archery, autonomous reconnaissance, land rights, caravans, kitchen ecology, orchestral culture, biochar, currency, timekeeping, hydrology, wired communications, and hydrographic archives.

Reuse the active authorities:

- **GenerationalSuccessionEngine**, census state, NeedsSystem, DutyRosterSystem, MedicalSystem, and the governance contract own population and actuarial records.
- **SilentFoundrySystem**, **CraftingSystem**, **TacticalCombatSystem**, and **WarlordDoctrineSystem** own cannon, bows, ironwork, and combat outcomes.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, Disease/Medical effects, and soil state own plants, perfumery, companion crops, and biochar.
- **CrossingArbitrationSystem**, MarketSystem, ConstitutionAssemblyPanel/governance, and SaveChecksum own courts, property, currency, mutual agreements, and ledgers.
- **ExpeditionSystem**, **LocationLayoutSystem**, **ResearchSystem**, **WeatherSystem**, and the drone/survey contract own reconnaissance and land mapping.
- **StealthDiveInstance**, the surface-vessel state, RadioHostSession, JournalSystem, and hydrographic records own maritime communication and atlases.
- SoundManager, cultural-event state, and JournalSystem own orchestra and public culture.
- The shared power/water/thermal contract owns Trombe, mills, fountains, and utility effects.

No PowerGridPanel, District8Accords, or IEventBus reference should become an untyped global shortcut. Continue using typed Core state and the existing host save pipeline.

## Batch entry gates

1. **Census/privacy gate:** define identity, age, household, occupation, health, dose, consent, missing data, aggregation, actuarial uncertainty, and history.
2. **Cannon safety gate:** define bronze composition, casting, bore, proof testing, powder/roundshot, recoil, crew, misfire, target damage, and maintenance.
3. **Aromatherapy gate:** distinguish scent/comfort effects from clinical treatment; define dose, contraindications, exposure, and evidence.
4. **Legal-code gate:** extend the Living Constitution with published law versions, jurisdiction, due process, conflicts, appeal, court records, and immutable audit history.
5. **Archery/drone gate:** define training, equipment condition, projectile supply, drone power, flight, image coverage, communication, and uncertainty.
6. **Cadastre/land gate:** define parcel geometry, ownership/use rights, commons, minerals, boundaries, survey error, transfers, and disputes.
7. **Caravan gate:** reuse expedition, market, faction, guard, cargo, route, weather, and hostile-doctrine contracts.
8. **Polyculture/biochar gate:** define companion interactions, soil nutrients, pyrolysis mass/energy, carbon, runoff, yield, and long-term degradation.
9. **Currency/time/water/telephone gate:** define mint assay, monetary backing, clock drift, aquifer draw, flow pressure, wired circuits, outages, and maintenance.
10. **Hydrographic archive gate:** define chart provenance, sounding uncertainty, route hazards, source vessel, publication, and update/versioning.

## Delivery order

1. **529, 532:** establish census and legal institutions.
2. **530, 533:** add cannon and archery production/training.
3. **531, 537, 539:** extend botanical products, gardens, and soil amendment.
4. **534, 535, 536:** add drones, cadastre, and trade caravans.
5. **538, 541, 543:** add orchestra, public clock, and telephone exchange.
6. **542:** connect aquifer and fountain infrastructure.
7. **544:** finish the hydrographic atlas.

## Step integration matrix

### [529] Population Census and Actuarial Tables — PopulationCensusPanel

**Core integration:** Extend GenerationalSuccessionEngine and existing census state over NeedsSystem, DutyRosterSystem, MedicalSystem, survivor identity/households, radiation/disease data, privacy, aggregation, and resource projection.

**Smallest vertical slice:** Run one enumeration interval, snapshot one household/occupation/health cohort, calculate an actuarial estimate with confidence, and publish a versioned report.

**Acceptance gate:** Consent/access, age, sex/gender fields as defined by project data, occupation, health, dose, family links, missingness, migration, mortality causes, cohort size, and projection assumptions are persisted. Projections inform planning; they cannot predict individual death or become hidden resource commitments.

### [530] Bronze Muzzle-Loading Siege Cannon — BronzeCannonPanel

**Core integration:** Extend SilentFoundrySystem, TacticalCombatSystem, WarlordDoctrineSystem, powder/roundshot catalogs, barrel-boring/metrology, proof testing, crew, recoil, target fortifications, and repair.

**Smallest vertical slice:** Cast one barrel, bore and inspect it, conduct a safe proof test, and resolve one catalogued firing against a test wall.

**Acceptance gate:** Alloy, casting defects, bore, wall thickness, powder charge, projectile mass, pressure, recoil, crew, elevation, wind, proof result, misfire, and maintenance are authoritative. A 500-meter shot and wall damage are configured test results; the cannon cannot be certified without proof state.

### [531] Botanical Perfumery and Essential Oils — PerfumeryStudioPanel

**Core integration:** Extend GreenhouseSystem, CraftingSystem, GoodsCatalog, MedicalSystem, NeedsSystem, distillation equipment, plant materials, storage, and scent/comfort effects.

**Smallest vertical slice:** Distill one plant batch, record yield/quality, blend one formula, and offer one consented aromatherapy session.

**Acceptance gate:** Plant mass, steam/water, temperature, distillation time, oil yield, contamination, concentration, exposure, allergies, contraindications, consent, and response are saved. Anxiety/insomnia effects are bounded comfort or treatment-adjunct outcomes; a universal 35% reduction requires evidence and cannot be a direct global stat write.

### [532] Legal Code and Judicial Court — JudicialCourtPanel

**Core integration:** Extend ConstitutionAssemblyPanel/governance, CrossingArbitrationSystem, FinalWishSystem where inheritance/family law applies, IEventBus/state-change events, case records, jurist duty, and immutable law versions.

**Smallest vertical slice:** Publish one legal article, open one property dispute, assign eligible jurists, issue a reasoned verdict, and record appeal/closure.

**Acceptance gate:** Jurisdiction, parties, evidence, rights, counsel/representation, conflicts, quorum, deliberation, verdict, remedy, appeal, law version, and audit history are persisted. The first case establishes process only; no stability bonus can bypass due process or create arbitrary punishment.

### [533] Traditional Archery and Fletcher Workshop — FletchersBowshopPanel

**Core integration:** Extend TacticalCombatSystem, DutyRosterSystem, CraftingSystem, equipment condition, training progression, range, bows, strings, arrows, heads, stealth, and expedition load.

**Smallest vertical slice:** Craft one bow/arrow batch, train one eligible scout, run one range test, and apply measured accuracy/noise/ammunition properties.

**Acceptance gate:** Stave, draw weight, tiller, string, arrow mass, head type, weather, range, training, fatigue, condition, shot count, and resupply are authoritative. Silent ranged capability is a configured tactical option; it cannot dominate night patrols or provide infinite ammunition.

### [534] Autonomous Solar Drone Swarm — DroneSwarmsPanel

**Core integration:** Add a drone-swarm state over ExpeditionSystem, LocationLayoutSystem, ResearchSystem, WeatherSystem, solar/power, flight telemetry, camera/coverage, communications, recovery, and map tiles.

**Smallest vertical slice:** Launch a small configured swarm, assign a survey polygon, collect overlapping image tiles, and return a coverage/confidence map.

**Acceptance gate:** Drone count, mass, power, launch/recovery, wind/thermal conditions, compass/altimeter, camera, overlap, battery, link loss, crash, weather, and mapping uncertainty are saved. Fifty drones and 500 km² require actual flight capacity; hidden enemy positions cannot be revealed without detection evidence.

### [535] Cooperative Land Cadastre — CadastreRegistryPanel

**Core integration:** Add a parcel/rights state over CrossingArbitrationSystem, LocationLayoutSystem, DutyRosterSystem, survey data, Constitution/legal code, ownership/use rights, commons, transfers, mineral claims, and dispute records.

**Smallest vertical slice:** Survey one parcel, register measured boundaries and rights, issue one title/use record, and resolve one boundary review.

**Acceptance gate:** Coordinates, survey error, markers, owner/user, tenure, commons, liens, mineral/water rights, consent, version, transfer, challenge, and adjudication are persisted. A cadastre clarifies claims; it cannot eliminate all land disputes or authorize ownership without legal review.

### [536] Transcontinental Trade Caravan Network — TradeCaravanPanel

**Core integration:** Extend ExpeditionSystem, CrossingArbitrationSystem, LocationLayoutSystem, WarlordDoctrineSystem, MarketSystem, caravan save, cargo, guards, route, weather, settlements, recruitment, and trade resolution.

**Smallest vertical slice:** Assemble one caravan, reserve goods/guards/provisions, travel a multi-waypoint route, resolve one encounter, and settle return cargo and losses.

**Acceptance gate:** Wagon capacity, cargo mass/value, animals/fuel, guards, crew, route, weather, tolls, faction relations, ambush, injury, recruitment, market prices, and delivery are saved. Platinum, seeds, and specialists are possible catalogued results, not guaranteed loot.

### [537] Raised-Bed Companion Kitchen Garden — KitchenGardenPanel

**Core integration:** Extend GreenhouseSystem, LocationLayoutSystem, NeedsSystem, crop/recipe data, companion interaction rules, pest/spoilage, water, soil, harvest, and meal composition.

**Smallest vertical slice:** Design one raised bed, plant a valid companion set, advance a succession cycle, harvest multiple outputs, and consume them in a meal.

**Acceptance gate:** Bed area, crop compatibility, planting dates, nutrient/water demand, pests, shade, yield, harvest timing, spoilage, and kitchen use are authoritative. Twenty varieties may be a milestone if catalogued and grown; full nutritional diversity requires actual meal coverage.

### [538] Colony Symphony Orchestra — SymphonyOrchestraPanel

**Core integration:** Extend NeedsSystem, DutyRosterSystem, CraftingSystem, SoundManager, cultural-event state, instrument condition, rehearsal, venue, audience, and authored program/archive.

**Smallest vertical slice:** Craft or repair one instrument, assign players, rehearse a short program, perform it, and record attendance/effect.

**Acceptance gate:** Instrument materials, tuning, player skill, rehearsal time, fatigue, venue, hearing/accessibility, audience, audio asset, and post-event effect are saved. A concert can create a high morale event when measured; it cannot automatically be the highest event in history.

### [539] Biochar Pyrolysis and Soil Carbon — BiocharKilnPanel

**Core integration:** Add a biomass-pyrolysis state over GreenhouseSystem, WeatherSystem, soil, waste biomass, kiln, heat, biochar quality, application, carbon accounting, runoff, and crop yields.

**Smallest vertical slice:** Load one biomass batch, run one controlled pyrolysis, assay char, apply it to one field, and observe one soil/water response.

**Acceptance gate:** Biomass mass/moisture, temperature, residence time, oxygen control, yield, emissions, char stability, application rate, soil texture, rainfall, nutrient interactions, and crop response are persisted. Water retention and yield improvements are field-specific and degrade/require maintenance; no permanent +40%/+20% global effects.

### [540] Copper/Silver/Electrum Coin Mint — CoinMintingPanel

**Core integration:** Extend MarketSystem, cooperative finance, Constitution/legal tender rules, CrossingArbitrationSystem, DutyRosterSystem, metal inventory, die condition, assay, weight, mint authority, and anti-counterfeit records.

**Smallest vertical slice:** Approve a coin standard, assay metal, strike one denomination, record its weight/mark, and settle one transaction.

**Acceptance gate:** Metal purity, mass, denomination, authority, die, minting loss, seigniorage, reserves, counterfeit risk, exchange value, and ledger settlement are saved. Coins reduce transaction friction only through market behavior; they cannot eliminate barter or grant +30% economic output directly.

### [541] Astronomical Clock Tower — AstroClockTowerPanel

**Core integration:** Extend JournalSystem, LocationLayoutSystem, CraftingSystem, SimClock, clockwork gear, lunar/solar/tide data, celestial model, public schedule, and drift correction.

**Smallest vertical slice:** Construct one clock module, calibrate it against an approved time observation, and publish one synchronized schedule/chime.

**Acceptance gate:** Gear ratios, escapement, pendulum/temperature, lunar/solar data, tide data, calibration, drift, maintenance, visibility, and time zone are persisted. A public clock can improve coordination; it cannot synchronize every activity perfectly without receiver/clock state.

### [542] Artesian Fountain Network — ArterialFountainPanel

**Core integration:** Add a hydrology/fountain state over LocationLayoutSystem, well/aquifer records, MaterialShieldingSystem where relevant, shared water/pressure/utility contract, basin construction, contamination, drawdown, and maintenance.

**Smallest vertical slice:** Validate one well, install one fountain line, measure flow/pressure/water quality, and add it to one corridor’s supply network.

**Acceptance gate:** Aquifer recharge, well depth, casing, pressure, flow, drawdown, contamination, seasonal change, basin loss, overflow, access, and maintenance are authoritative. “Zero-energy continuous” applies only where natural head supports it; permanent supply requires recharge and water-quality checks.

### [543] Mechanical Telephone Exchange — TelephoneExchangePanel

**Core integration:** Add a wired-communications state over LocationLayoutSystem, DutyRosterSystem, department endpoints, power/battery where needed, cable condition, switchboard operator, calls, outages, privacy, and crisis events.

**Smallest vertical slice:** Wire two departments, place handsets, establish one ring-down call, and record response latency/outage status.

**Acceptance gate:** Cable route, endpoint, magneto/battery, operator, line condition, switchboard capacity, cross-talk, privacy, maintenance, and call duration are saved. A medical-to-foundry call can reduce response time when both endpoints and operators are available; no instant colony-wide network effect.

### [544] Navigator’s Hydrographic Atlas — HydrographicAtlasPanel

**Core integration:** Extend StealthDiveInstance, surface vessels, ExpeditionSystem, JournalSystem, LocationLayoutSystem, survey/photogrammetry data, depth soundings, coastal hazards, chart versions, and route-planning consumers.

**Smallest vertical slice:** Compile one validated ocean/river survey set into a versioned chart, annotate hazards, publish it, and use it on one maritime expedition.

**Acceptance gate:** Source vessel, survey date, sounding method, depth uncertainty, tide/current, coast, hazard, chart scale, version, correction, route, and user skill are persisted. An atlas improves planning and reduces modeled risk; it cannot reduce casualties to zero through perfect foreknowledge.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for census records, metal/powder, plant material, survey devices, drones, grain, water, biochar feedstock, coin metal, wire/cable, instruments, food, and archive media.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, miscalibration, misfire, default, dispute, outage, maintenance, stale-data, and route outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, flow_m3_s, depth_m, error_radius_km, sound_level_db, balance_units, vote_count, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- census_actuarial_defs.json
- muzzle_cannon_defs.json
- botanical_distillate_defs.json
- legal_code_defs.json
- archery_equipment_defs.json
- drone_survey_defs.json
- cadastre_rules.json
- caravan_route_defs.json
- companion_crop_defs.json
- orchestra_program_defs.json
- biochar_process_defs.json
- currency_rules.json
- astronomical_clock_defs.json
- artesian_water_defs.json
- telephone_exchange_defs.json
- hydrographic_chart_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Legal articles, parcels, coins, routes, crops, instruments, resources, reports, hazards, and effects must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

PopulationCensusPanel, BronzeCannonPanel, PerfumeryStudioPanel, JudicialCourtPanel, FletchersBowshopPanel, DroneSwarmsPanel, CadastreRegistryPanel, TradeCaravanPanel, KitchenGardenPanel, SymphonyOrchestraPanel, BiocharKilnPanel, CoinMintingPanel, AstroClockTowerPanel, ArterialFountainPanel, TelephoneExchangePanel, and HydrographicAtlasPanel.

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

- census privacy/aggregation/missingness/actuarial confidence, cannon casting/proof/pressure/accuracy/damage, botanical yield/dosage/contraindications, and court law/version/appeal;
- archery manufacture/training/condition/resupply, drone flight/coverage/recovery/map confidence, cadastre error/rights/disputes, and caravan cargo/route/market/hostile outcomes;
- companion crops/pest/nutrition, orchestra instruments/rehearsal/audience, biochar mass/energy/soil response, coin assay/ledger/counterfeit, and clock drift/calibration;
- artesian flow/drawdown/water quality, telephone endpoints/outages/latency/privacy, hydrographic depth uncertainty/chart versioning, and all consumer modifiers;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 529–544 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Legal, economic, clinical, ecological, navigation, culture, infrastructure, and agricultural claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
