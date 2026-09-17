# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 10)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 145–160

## Outcome

Batch 10 should be delivered as dependency-first vertical slices. Each slice must extend the active Core/Godot authorities, persist its state, expose typed host read models, and prove one small playable behavior before the larger feature promise is accepted.

The roadmap language is aspirational. Claims such as 1,000 km flight, 200-mile radar, zero radiation hazard, or a 500-year archive become data-defined outcomes only after the simulation contains the required limits, resources, failure modes, and migration coverage.

Current authority substitutions:

- **MarketSystem** is the active economy authority; do not introduce a parallel DynamicEconomySystem.
- **FactionRadioEngine** and the existing radio host/save contracts are the active communications authorities; do not add a duplicate RadioInterceptionSystem.
- **GenerationalSuccessionEngine** and the existing generational/expansion save contracts are the active lineage authorities; do not create a parallel CenturySeed runtime.
- **SilentFoundrySystem**, **CraftingSystem**, **WeaponConditionSystem**, **WeatherSystem**, **GreenhouseSystem**, **NeedsSystem**, **DutyRosterSystem**, **JournalSystem**, **ResearchSystem**, **LocationLayoutSystem**, and **ExpeditionSystem** remain the relevant existing owners.
- There is no confirmed shared power-grid, clinical oral-health, shelter-transit, prisoner, outpost, radar, archive-format, or challenge-profile authority. Those seams require a typed contract before UI work begins.

## Batch entry gates

Complete these audits before implementing feature panels:

1. **Batch 8/9 dependency audit:** resolve the shared power/storage contract, radio relay/codebook state, heavy cargo semantics, and any rail or infrastructure prerequisites that Batch 10 consumes.
2. **Industrial process contract:** define input/output recipes, machine condition, energy/steam demand, job duration, cancellation, and failure settlement for heavy forging and wire drawing.
3. **Clinical and condition contract:** identify the canonical owner for pain, dental disease, nutrition deficiency, mobility, and procedure outcomes. NeedsSystem alone does not currently model all of these.
4. **Sensor and communications contract:** define deterministic range, line of sight, weather attenuation, antenna direction, noise, jamming, signal confidence, and device degradation.
5. **Governance and profile contract:** define prisoner lifecycle and rehabilitation safeguards, hazardous-waste containment, and a profile-scoped challenge run that cannot corrupt campaign saves.
6. **Archive and culture contract:** define authored documents, scripts, recordings, microfilm media, knowledge unlocks, signatures, and generational persistence through JournalSystem/research state.

## Delivery order

The recommended order minimizes rework:

1. **145, 146, 156:** establish heavy cargo and industrial process primitives.
2. **149, 151:** add durable food-culture and nutrition effects.
3. **147, 150, 154:** add clinical, governance, and cultural state owners.
4. **148, 152, 155, 159:** build the shared sensor/radio/time model and its consumers.
5. **153, 157:** add energy storage and hazardous-material containment on the shared infrastructure contracts.
6. **158:** connect archival capture to the already-tested journal/research contracts.
7. **160:** finish with isolated challenge profiles and local score records.

## Step integration matrix

### [145] Hydrogen Cargo Airship — AirshipHangarPanel

**Core integration:** Add an airship state/route contract over LocationLayoutSystem, ExpeditionSystem, MarketSystem, cargo inventory, weather, crew assignments, fuel, and the heavy-asset contract from Batch 8/9. Keep lift and payload arithmetic in Core; Godot only selects routes and displays telemetry.

**Smallest vertical slice:** Construct one airship hull, assign a crew, load a bounded cargo manifest, and complete one route with a deterministic weather outcome.

**Acceptance gate:** Route distance, payload mass, envelope condition, fuel, crew skills, weather risk, landing capacity, and cargo settlement are all validated and saved. A failed or interrupted flight cannot duplicate cargo. A distant settlement is a data-defined location; do not hard-code a real-world destination or guarantee a 1,000 km route.

### [146] Steam Drop-Hammer — DropHammerPanel

**Core integration:** Extend SilentFoundrySystem with a heavy-forging job type and connect it to shelter construction, machine condition, material inventory, steam/energy demand, and CraftingSystem output catalogs.

**Smallest vertical slice:** Reserve one ingot batch, run one drop-hammer job, produce one heavy plate, and settle wear, energy, heat, and scrap.

**Acceptance gate:** The job is deterministic, cancellable, condition-aware, and cannot output material without valid inputs. Tonnes, pressure, temperature, and duration are data-defined. Tier-5 weapons or armor are unlocks, not automatic consequences of installing the machine.

### [147] Dental Surgery — DentalSurgeryModal

**Core integration:** Introduce a typed oral-health/procedure state that composes with MedicalSystem, CombatTraumaSystem, SurvivorNeedsState, inventory, and the canonical affliction/effect pipeline. Do not mutate health directly from the modal.

**Smallest vertical slice:** Diagnose one infected molar, reserve a procedure kit and analgesic, resolve extraction, and apply a bounded pain/eating/work recovery effect.

**Acceptance gate:** Procedure eligibility, clinician skill, anesthesia/analgesic supply, infection risk, recovery time, and failure outcomes are persisted. “Full eating capacity” is a named effect with a duration and stacking rule, not an unbounded UI flag.

### [148] Parabolic Radar Dish — RadarDishPanel

**Core integration:** Add a radar observation state over WeatherSystem, FactionRadioEngine, LocationLayoutSystem, device condition, and the shared power contract. Radar observations must use deterministic world contacts and confidence rather than inventing entities in the UI.

**Smallest vertical slice:** Complete one sweep and detect one canonical weather or radio-linked contact with azimuth, range band, confidence, and timestamp.

**Acceptance gate:** Range is reduced by weather, terrain/line of sight, dish condition, power, and operator calibration. Contacts can be false, stale, or lost. No automatic 200-mile detection or aircraft discovery is accepted without a catalogued contact and modeled propagation limits.

### [149] Sourdough Mother and Brick Bakery — BakeryOvenPanel

**Core integration:** Extend RecipeCatalog, CraftingSystem, GoodsCatalog, NeedsSystem, greenhouse inputs, and generational/cultural state with a persistent mother-culture record and bakery jobs.

**Smallest vertical slice:** Feed one culture, bake one batch from valid grain/water/fuel, add a pantry item, and record a bounded dining morale effect.

**Acceptance gate:** Culture age, contamination, hydration, oven temperature, fuel, batch yield, spoilage, and recipe inputs survive save/load. The morale bonus applies only through the existing meal/effect path; it is not a global direct stat write.

### [150] Prisoner Brig and Rehabilitation — PrisonBrigPanel

**Core integration:** Create a prisoner lifecycle contract connected to DutyRosterSystem, CaregivingSystem, survivor needs, faction relations, shelter security, and the narrative/event pipeline. Do not reuse survivor citizenship fields as an implicit prisoner system.

**Smallest vertical slice:** Register one captured person, assign supervised non-combat work, record compliance and care, then resolve one review/parole/citizenship outcome.

**Acceptance gate:** Capture provenance, security capacity, guard duty, food/medical needs, consent/safety rules, escape risk, faction consequences, and rehabilitation evidence are modeled and persisted. There is no instant loyalty result and no UI-only labor assignment.

### [151] Microgreen Sprouter — SprouterPanel

**Core integration:** Extend GreenhouseSystem, CraftingSystem, GoodsCatalog, and a typed nutrition/deficiency effect contract consumed by NeedsSystem.

**Smallest vertical slice:** Start one tray with seed, water, light/heat requirements, resolve a four-day cycle, and add a fresh-greens batch to meals.

**Acceptance gate:** Crop yield, contamination, water, light or power demand, temperature, and harvest loss are authoritative. Vitamin-C/scurvy effects need a named status owner and tests; do not claim that one tray cures every survivor or consumes zero power unless the configuration explicitly says so.

### [152] Mountain Observation Pillbox OP-1 — OutpostPanel

**Core integration:** Add an outpost state over LocationLayoutSystem, ExpeditionSystem, WarlordDoctrineSystem, supplies, garrison rotation, and radio/relay contracts.

**Smallest vertical slice:** Establish one two-person post, consume a supply cycle, receive one observation report, and rotate or recall the garrison.

**Acceptance gate:** Construction, travel, staffing, resupply, weather, comms uptime, detection confidence, and hostile interference are saved. The outpost reduces surprise risk; it does not eliminate uncertainty or guarantee continuous surveillance.

### [153] Compressed-Air Storage — PneumaticStoragePanel

**Core integration:** Introduce a storage adapter to the shared power/grid contract and connect it to material shielding, cavern integrity, compressor loads, pneumatic tools, and outage handling.

**Smallest vertical slice:** Charge one storage cavern, discharge it into one valid tool load, and record pressure, usable energy, losses, and seal condition.

**Acceptance gate:** Pressure, volume, temperature, compressor efficiency, leakage, conversion losses, tool demand, and safety limits use explicit units. The system must conserve energy and reject overpressure. “500 PSI for three days” is a balance result, not a hard-coded guarantee.

### [154] Common-Room Theater — TheaterStageModal

**Core integration:** Add a cultural-event/script record connected to CaregivingSystem, JournalSystem, survivor participation, audience needs, and SoundManager playback metadata.

**Smallest vertical slice:** Draft or select one script, cast available survivors, perform one event, record the script, and apply a bounded audience effect.

**Acceptance gate:** The event checks time, venue, participants, props, fatigue, and audience. Scripts and performer credits are saved as journal entries. Morale/stress effects use the existing effect pipeline and are not an unsupported shelter-wide direct bonus.

### [155] Phased Radio Array — PhasedArrayPanel

**Core integration:** Extend FactionRadioEngine/radio save state with antenna geometry, phase configuration, directional gain, noise, jamming, power, and research unlocks. Reuse the relay and codebook contracts from earlier batches.

**Smallest vertical slice:** Tune two antennas to one channel, apply a phase configuration, and improve or worsen one deterministic signal observation.

**Acceptance gate:** Signal-to-noise changes are reproducible from seed and configuration; antenna condition, phase error, terrain, weather, power, and hostile jamming matter. A 95% reduction is a balance value for a defined configuration, not an unconditional promise.

### [156] Wire-Drawing Mill — WireDrawMillPanel

**Core integration:** Extend SilentFoundrySystem, CraftingSystem, inventory, material catalogs, and the infrastructure installation contract. Use wire length, mass, gauge, insulation, conductivity, and condition as typed properties.

**Smallest vertical slice:** Draw one copper batch from valid stock, spool it, and consume a measured length in one real cable-consuming installation.

**Acceptance gate:** Input mass and output mass reconcile, drawplate wear and annealing are modeled, scrap is settled, and the installation rejects insufficient gauge/insulation. No 500-meter output appears without material and machine capacity.

### [157] Deep-Borehole Waste Repository — WasteRepositoryPanel

**Core integration:** Add a hazardous-waste repository state over RadiationSystem, MaterialShieldingSystem, inventory, facility condition, monitoring, and the shared power/industrial contract.

**Smallest vertical slice:** Seal one eligible waste batch in a canister, lower it into one validated borehole, and create a monitoring record.

**Acceptance gate:** Waste identity, activity band, shielding, containment, borehole depth, aquifer exclusion, placement capacity, leakage, retrieval, and inspection are persisted. Repository completion reduces a modeled hazard contribution; it cannot set total radiation hazard to zero unless every source is resolved.

### [158] Microfilm Reader and Archive — MicrofilmReaderPanel

**Core integration:** Add an archive-media state over JournalSystem, ResearchSystem, inventory, generational succession, and document/knowledge catalogs.

**Smallest vertical slice:** Copy one existing authored document to one film roll, read it with a functioning reader, and unlock one research record with provenance.

**Acceptance gate:** Source document, media condition, reader condition, storage environment, duplicate handling, and knowledge unlock are checked and saved. “500-year preservation” is a configured retention rating with degradation/inspection behavior, not an absolute claim.

### [159] Radio Time Standard — TimeStandardPanel

**Core integration:** Add a time-standard state over SimClock, RadioHostSession, power, oscillator condition, and expedition navigation consumers.

**Smallest vertical slice:** Calibrate one local clock, broadcast one synchronization pulse, and apply a bounded drift correction to one expedition/navigation consumer.

**Acceptance gate:** Drift, holdover, transmission delay, signal confidence, power, and receiver condition are modeled. The system must not call itself atomic or promise perfect synchronization without an actual precision model. Save/load preserves calibration and last-sync time.

### [160] Challenge Mode and Local Leaderboards — ChallengeModePanel

**Core integration:** Create a versioned, profile-scoped challenge-run record connected to Main, Settings/UserSettings, SaveChecksum, seed configuration, mutators, and the result/achievement profile. Keep challenge metadata separate from ordinary campaign state.

**Smallest vertical slice:** Start one seeded run with one mutator, advance to a terminal result, calculate a deterministic score, and append one tamper-evident local record.

**Acceptance gate:** Mutator versions, seed, ruleset hash, start/end state, score inputs, completion reason, and checksum are stored. Campaign saves remain loadable; changing a mutator after start is rejected; duplicate submissions are stable; leaderboard scope is local-only unless a separate authenticated service is explicitly approved. Do not add network leaderboard behavior to this batch.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact resource reservation and settlement, including cancellation, interruption, failure, spoilage, and loss paths.
- Stable ordinal identifiers normalized to canonical snake_case and validated by the data-integrity rules.

Use explicit units in DTOs and data: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, signal_db, time_drift_ppm, condition_fraction, and contamination_fraction.

## Data authority and content work

Extend existing JSON catalogs where possible. Add new files only when an existing catalog cannot represent the domain. Candidate data surfaces are:

- airship_routes.json
- industrial_processes.json
- clinical_procedures.json
- food_cultures.json
- outposts.json
- power_storage.json
- radio_sensor_defs.json
- archive_formats.json
- challenge_mutators.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and a loader test. No Godot panel may become a hidden data authority.

## Godot host and UI work

Build the named panels only after their host contracts exist:

AirshipHangarPanel, DropHammerPanel, DentalSurgeryModal, RadarDishPanel, BakeryOvenPanel, PrisonBrigPanel, SprouterPanel, OutpostPanel, PneumaticStoragePanel, TheaterStageModal, PhasedArrayPanel, WireDrawMillPanel, WasteRepositoryPanel, MicrofilmReaderPanel, TimeStandardPanel, and ChallengeModePanel.

Each panel must bind to a typed host session/read model, expose empty/loading/error states, and issue commands that return validation results. No Bind(object), placeholder arrays, direct Core mutation, or hard-coded success outcome is permitted. Add each new dirty store to Main.Setup, Main.Save, and Main.Flush only through the shared save orchestration and atomic writer.

## Verification plan

Run the focused Core tests after each vertical slice, then the full acceptance set:

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

Add focused tests for each slice:

- deterministic route, cargo, industrial-job, sensor, radio, and time outcomes;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write save cases;
- resource conservation and duplicate-prevention for airship cargo, wire, energy, food, film, and hazardous waste;
- clinical procedure eligibility and bounded survivor effects;
- prisoner lifecycle, security, care, review, and faction consequences;
- challenge profile isolation, ruleset hashing, score reproducibility, and campaign-save non-corruption;
- typed panel command validation and empty/error-state behavior.

## Definition of done

Batch 10 is complete only when every step has one of these outcomes documented in tests and handoff notes: implemented, explicitly deferred with a reason and dependency, or rejected as unsupported by the current simulation model. For implemented steps:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Hard roadmap claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
