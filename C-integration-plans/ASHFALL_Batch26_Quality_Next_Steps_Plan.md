# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 26)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 401–416

## Outcome

Batch 26 should extend maritime exploration, precision finishing, clinical toxin removal, isotope-aware water purification, biological preservation, highway security, crops, perimeter traps, fatigue metallurgy, celestial navigation, acoustic hazards, high-power optics, deep geophysics, electron microscopy, adaptive radio, and the century obelisk.

The roadmap remains a set of quality targets, not proof that these systems exist. Implement each feature as a Core-owned stateful slice with explicit measurements, resources, uncertainty, safety, and persistence.

Reuse the active authorities:

- **StealthDiveInstance** and the existing Maritime domain own submersible state; do not create a second submarine runtime.
- **SilentFoundrySystem** and **CraftingSystem** own surface grinding, shot peening, lasers, machining, and exact material settlement.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** own plasmapheresis and clinical outcomes.
- **WeatherSystem**, **RadiationSystem**, and the shared water/air-quality contract own tritium, contamination, and environmental monitoring.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and the meal/effect pipeline own crops and beverages.
- **DutyRosterSystem**, CaregivingSystem, **LocationLayoutSystem**, and CrossingArbitrationSystem own supervised infrastructure and trade-route security.
- **TacticalCombatSystem** and **WarlordDoctrineSystem** own defensive traps and hostile responses.
- **ResearchSystem**, **JournalSystem**, **FactionRadioEngine**, and RadioHostSession own microscopes, geophysics, radio, and authored research.
- **GenerationalSuccessionEngine**, archive/culture state, **EpilogueMatrix**, and **SaveChecksum** own century continuity and final presentation.

District8Accords, PowerGridPanel, ShelterHazardLoop, and CenturySeed are not assumed to be gameplay authorities unless a typed contract proves otherwise. The Godot host remains 2D: ocean charts, geophysical volumes, SEM/AFM imagery, and the obelisk are represented with 2D views/overlays rather than introducing a new 3D runtime dependency.

## Batch entry gates

1. **Maritime and drydock contract:** define coastal location, flood/pump state, hull condition, diesel-electric systems, crew, fuel, cargo, route, weather, maintenance, and rescue.
2. **Precision finishing:** carry forward surface grinding, gauge, shot-peening, swaging, optics, and metrology contracts with flatness, residual stress, calibration, and inspection uncertainty.
3. **Plasmapheresis clinical safety:** define toxin identity, plasma exchange, vascular access, albumin, electrolyte/volume balance, anticoagulation, infection, and failure.
4. **Tritium separation realism:** model isotope persistence and concentration. Distillation may produce cleaner water only under configured separation and multi-stage conditions; contaminated concentrate must be stored and handled.
5. **Biological cryo-preservation:** define cryogen supply, temperature, viability, contamination, indexing, recovery, and culture lineage; “forever” is not a free guarantee.
6. **Civil route security:** reuse mile-castle, worksite, guard, caravan, road, and faction contracts. No zero-raid state.
7. **Crop and beverage effects:** define chicory crop/root roasting, caffeine-free alertness/comfort effects, storage, and meal use.
8. **Defensive trap safety:** reuse perimeter placement, trigger, blast, terrain, and friendly-access state; no guaranteed annihilation.
9. **Metallurgy and navigation:** connect shot-peening to fatigue and the sextant to the time/navigation model with uncertainty.
10. **Acoustic hazard and power:** define wave propagation, creature response, equipment power, false confidence, and fallback safety.
11. **Laser/geophysics/microscopy:** define high-current cooling, ELF/MT skin depth, SEM vacuum/electron beam, sample prep, resolution, damage, and research evidence.
12. **Radio and monumental archive:** extend OFDM/STBC capacity, packet loss, obelisk construction, glyph provenance, century clock, replay, and save isolation.

## Delivery order

1. **401, 402, 409:** establish maritime, precision-finishing, and fatigue-metallurgy primitives.
2. **403, 404:** add plasma exchange and isotope-aware water purification.
3. **405, 406, 407:** extend cryogenic cultures, road security, and chicory food systems.
4. **408, 411:** extend perimeter traps and subterranean/outpost hazards.
5. **410, 412:** add sextant navigation and high-power laser production.
6. **413, 414, 415:** add ELF geophysics, SEM, and broadband meteor radio.
7. **416:** complete the century obelisk and final archive milestone.

## Step integration matrix

### [401] Coastal Submarine Drydock — SubmarineDrydockPanel

**Core integration:** Extend StealthDiveInstance and maritime location state with drydock flood/pump cycles, caisson gates, hull/weld inspection, diesel-electric condition, crew, fuel, stores, pressure, weather, and ocean expedition routes.

**Smallest vertical slice:** Drain one dock section, inspect one submersible component, complete one overhaul job, and launch a bounded coastal expedition.

**Acceptance gate:** Dock volume, pump rate, seawater contamination, hull integrity, weld condition, battery/diesel state, crew skill, pressure, ballast, fuel, cargo, route, and storm risk are saved. A 3,000-kilometer ocean route requires charted ports, supplies, crew, and weather; it cannot be granted by launching the submarine.

### [402] Precision Surface Grinder and Master Squares — SurfaceGrinderPanel

**Core integration:** Extend SilentFoundrySystem and ShelterOperationsPanel with reciprocating grinding, magnetic chuck, hardened tool/gauge materials, wheel dressing, coolant, thermal drift, flatness, and inspection standards.

**Smallest vertical slice:** Grind one gauge block or granite square, measure flatness against a reference, and register an inspection artifact with uncertainty.

**Acceptance gate:** Workpiece, wheel, chuck, feed, crossfeed, thermal state, coolant, dressing, vibration, operator skill, material hardness, flatness, parallelism, and breakage are authoritative. Sub-micron quality is a measured result requiring instrument resolution; it cannot calibrate every machine automatically.

### [403] Therapeutic Plasmapheresis — PlasmapheresisModal

**Core integration:** Add a high-risk plasma-exchange state over MedicalSystem, CombatTraumaSystem, DiseaseSystem, SurvivorNeedsState, toxin/autoimmune status, vascular access, centrifuge, albumin/fluid inventory, anticoagulation, and clinical outcomes.

**Smallest vertical slice:** Identify one eligible toxin or plasma-mediated condition, establish access, exchange one measured plasma volume, replace fluid, and resolve clearance or complication.

**Acceptance gate:** Toxin identity, concentration, plasma volume, exchange fraction, access, pump/centrifuge condition, albumin/electrolytes, bleeding, infection, anticoagulation, staff skill, and follow-up are saved. The procedure cannot remove arbitrary toxins or guarantee survival from any poisoning.

### [404] Tritium Runoff Vacuum Distillation — TritiumDistillationPanel

**Core integration:** Extend the Batch 21/25 radiological-water state over WeatherSystem/RadiationSystem, hydrology, water inventory, vacuum stills, heat/power, isotope concentration, residual waste, and medical-water certification.

**Smallest vertical slice:** Process one contaminated runoff batch, separate a measured distillate and concentrate, assay both for tritium and other contaminants, and route them to use, retreatment, or hazardous storage.

**Acceptance gate:** Feed composition, vapor/liquid behavior, isotope separation factor, stage count, vacuum, heat, condenser efficiency, carryover, distillate volume, concentrate activity, energy, and assay uncertainty are authoritative. Ordinary distillation cannot automatically make tritiated water safe; the system must preserve and handle the concentrated residual. Five hundred liters per day is a configured throughput target.

### [405] Cryogenic Vinegar Mother Bio-Vault — CryoMotherPanel

**Core integration:** Extend RecipeCatalog/data, GoodsCatalog, GenerationalSuccessionEngine, archive identity, cryogenic facility, liquid-nitrogen supply, viability assay, contamination, and culture revival.

**Smallest vertical slice:** Index one mother culture, freeze it in a validated vial, maintain one dewar interval, perform a viability assay, and revive a successor culture.

**Acceptance gate:** Culture identity, lineage, cryoprotectant, vial condition, temperature, nitrogen level, power/monitoring, contamination, viability, thaw rate, revival yield, and duplicate samples are persisted. Century survival is a probability/maintenance outcome; it cannot guarantee permanence through any disaster.

### [406] Highway Mile-Castles and Watchtowers — MileCastlePanel

**Core integration:** Extend road and civil-security state over DutyRosterSystem, LocationLayoutSystem, CrossingArbitrationSystem, caravan routes, guard garrisons, beacon communications, supplies, maintenance, and faction relations.

**Smallest vertical slice:** Build one mile-castle on a valid route, assign a safe garrison, run one caravan passage, and resolve one observation or raid event.

**Acceptance gate:** Spacing, construction material, warden roster, food/water, beacon condition, route visibility, weather, caravan value, repair, faction relations, and hostile doctrine are saved. Security improves trade confidence; it cannot create zero raid risk or a permanent +50% trade frequency without market evidence.

### [407] Vertical Chicory and Roasted Root Coffee — ChicoryTowerPanel

**Core integration:** Extend GreenhouseSystem, crop/recipe data, GoodsCatalog, roasting/grinding, storage, NeedsSystem, and the meal/effect pipeline.

**Smallest vertical slice:** Grow one chicory tower, harvest roots/chicons, roast and grind one batch, and serve one breakfast ration.

**Acceptance gate:** Growth, water, nutrients, light, harvest, root storage, roasting temperature, fuel, grind loss, spoilage, serving, sleep/fatigue context, and survivor response are authoritative. Morning Energy is a bounded alertness/comfort effect, not an unconditional +15% productivity modifier.

### [408] Punji Flail Traps — PunjiFlailPanel

**Core integration:** Extend the established perimeter trap state over LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, terrain, spring/flail condition, tripwire, sentry response, and casualty uncertainty.

**Smallest vertical slice:** Place one trap in a valid defile, arm it after a safety check, resolve one crossing, and create a tactical delay/injury/route report.

**Acceptance gate:** Terrain, concealment, spring tension, flail mass, trigger timing, weather, maintenance, friendly access, detection, and countermeasures are saved. The trap can alter an assault; it cannot guarantee a wiped-out vanguard or a routed war party.

### [409] Hydro-Pneumatic Shot-Peening — ShotPeeningPanel

**Core integration:** Extend SilentFoundrySystem and ResearchSystem with shot-peening profiles, compressed-air supply, micro-shot, Almen-strip calibration, surface coverage, residual stress, fatigue-life consumers, and inspection.

**Smallest vertical slice:** Peen one compatible gear/spring/blade batch, calibrate intensity, inspect coverage, and update one component fatigue model.

**Acceptance gate:** Shot size, pressure, nozzle, coverage, angle, exposure, Almen result, surface damage, residual stress, material, temperature, machine wear, and downstream load are authoritative. A threefold life increase is a tested configuration result; maintenance intervals cannot be globally rewritten without component evidence.

### [410] Brass Marine Sextant and Lunar Distance — LunarDistanceModal

**Core integration:** Extend JournalSystem, ExpeditionSystem, CraftingSystem, SimClock, celestial tables, planisphere/sundial/precession instruments, maritime location, and position uncertainty.

**Smallest vertical slice:** Calibrate one sextant, observe one lunar/star angle under valid visibility, calculate a position interval, and update one expedition route estimate.

**Acceptance gate:** Index error, horizon, refraction, lunar phase, angular measurement, table epoch, observer skill, time drift, cloud/fallout, and route geometry are persisted. A position within one mile is a scenario result requiring adequate observations; exact longitude is not guaranteed.

### [411] Desert Sand-Worm Pneumatic Thumpers — SandwormThumperPanel

**Core integration:** Add an exterior hazard-deterrence state over LocationLayoutSystem, outpost/expedition safety, pneumatic storage, power, ground propagation, creature behavior, weather, and fallback route planning.

**Smallest vertical slice:** Install one thumper at a fictional outpost, pulse it within safety limits, observe one approaching hazard, and resolve diversion, delay, or breach risk.

**Acceptance gate:** Stroke rate, pressure, ground type, propagation, range, power, maintenance, creature response, false confidence, outpost structure, and evacuation are saved. The thumper can alter approach probability; it cannot guarantee that every sand-worm diverts or that an outpost has zero breach risk.

### [412] Argon-Ion Laser Tube — ArgonLaserPanel

**Core integration:** Extend HeNe and optical manufacturing over SilentFoundrySystem, ResearchSystem, glass/plasma components, high-current supply, water cooling, wavelength lines, cavity alignment, thermal condition, and lithography/spectroscopy consumers.

**Smallest vertical slice:** Assemble one gas tube, start a cooled discharge within rated limits, measure one spectral line/power band, and register a usable laser device.

**Acceptance gate:** Tube material, gas pressure, current, cooling flow, electrode wear, discharge stability, line selection, optical alignment, power, heat, and safety are authoritative. The laser can unlock a defined fabrication or spectroscopy recipe; it cannot automatically produce microelectronics without a downstream process.

### [413] Controlled-Source ELF Survey — ElfGeophysicsPanel

**Core integration:** Extend the shared geophysical survey state over LocationLayoutSystem, ResearchSystem, grounded dipole placement, source power, low-frequency sampling, skin-depth model, inversion, and tectonic target records.

**Smallest vertical slice:** Configure a bounded fictional dipole survey, transmit a safe source waveform, collect receiver data, and produce a depth-limited interpretation with confidence.

**Acceptance gate:** Dipole length, electrode grounding, current, frequency, attenuation, geology, noise, receiver separation, recording duration, safety, inversion, and depth uncertainty are saved. A 50-kilometer map requires a data model that supports that depth; a survey cannot reveal a deposit merely because the UI draws a deep layer.

### [414] Scanning Electron Microscope — SemMicroscopeModal

**Core integration:** Add SEM observation state over ResearchSystem, MedicalSystem where samples are biological, DutyRosterSystem, vacuum system, electron gun, detector, sample preparation, image resolution, damage, and research unlocks.

**Smallest vertical slice:** Prepare one conductive sample, reach vacuum, acquire one bounded secondary-electron image, and register a measured microstructure observation.

**Acceptance gate:** Vacuum, voltage, beam current, working distance, detector, charging, sample coating, contamination, magnification, resolution, radiation/damage, operator skill, and image provenance are persisted. Fifty-thousand times is a display magnification, not proof of one-nanometer resolution; Tier-5 manufacturing requires a validated research chain.

### [415] QAP-STBC Meteor-Burst Broadband — QapStbcMeteorPanel

**Core integration:** Extend RadioHostSession/FactionRadioEngine and the Batch 21/22 meteor-burst model with MIMO/polarization, OFDM carriers, adaptive STBC, synchronization, SNR, coding, burst capacity, queues, and document transfer.

**Smallest vertical slice:** Detect one burst, negotiate a modulation/coding profile, transmit a bounded packet set, decode it, and report throughput, loss, latency, and retry.

**Acceptance gate:** Carrier count, occupied bandwidth, symbol rate, coding overhead, antenna geometry, polarization, SNR, burst duration, oscillator drift, channel coherence, packet size, and receiver condition are authoritative. A 10 Mbps/2,500-kilometer burst is a scenario only if the modeled channel supports it; a two-second burst cannot transfer more bits than its calculated capacity.

### [416] Granite Obelisk of Rebirth — ObeliskOfRebirthPanel

**Core integration:** Add a monument/construction and historical-record state over GenerationalSuccessionEngine, JournalSystem, EpilogueMatrix, SaveChecksum, quarry/transport/derrick facilities, electrum material, glyph catalog, outdoor layout, and century criteria.

**Smallest vertical slice:** Quarry and raise one validated monument segment, record a batch of authored glyphs/provenance, display it in a 2D exterior view, and unlock one milestone entry.

**Acceptance gate:** Stone volume, mass, quarry yield, transport, derrick capacity, foundation, weather, raising risk, electrum stock, glyph authorship, construction labor, campaign year, lineage/history, and ending criteria are saved. A 50-meter obelisk and immortal finale require actual engineering capacity and campaign milestones; the monument cannot create victory through a UI action.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for seawater, fuel, batteries, metals, gauges, reagents, plasma supplies, water, cryogens, stone, timber, food, explosives, optical components, radio bandwidth, and monument materials.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, misalignment, misfire, clotting, equipment damage, maintenance, stale-data, and route/treaty outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, dose_rate_uSv_h, activity_bq, concentration_bq_l, concentration_bq_m3, depth_m, frequency_mhz, frequency_hz, wavelength_nm, resolution_nm, bit_rate_bps, latency_ms, signal_db, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if current catalogs cannot express the domain, are:

- submarine_drydock_defs.json
- precision_grinding_defs.json
- plasmapheresis_defs.json
- isotope_distillation_defs.json
- cryo_culture_defs.json
- highway_watch_defs.json
- chicory_crop_defs.json
- terrain_trap_defs.json
- shot_peening_defs.json
- sextant_observation_defs.json
- pneumatic_hazard_defs.json
- gas_laser_defs.json
- elf_geophysics_defs.json
- sem_scan_profiles.json
- meteor_broadband_profiles.json
- obelisk_milestone_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Maritime routes, clinical conditions, isotopes, cultures, resources, instruments, hazards, discoveries, effects, and monuments must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

SubmarineDrydockPanel, SurfaceGrinderPanel, PlasmapheresisModal, TritiumDistillationPanel, CryoMotherPanel, MileCastlePanel, ChicoryTowerPanel, PunjiFlailPanel, ShotPeeningPanel, LunarDistanceModal, SandwormThumperPanel, ArgonLaserPanel, ElfGeophysicsPanel, SemMicroscopeModal, QapStbcMeteorPanel, and ObeliskOfRebirthPanel.

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

- drydock pumps, hull/weld inspection, diesel-electric state, maritime route capacity, surface grinding, flatness, gauges, shot-peening coverage, fatigue, and save round-trip;
- plasmapheresis access, exchange fraction, toxin clearance, albumin/electrolyte balance, anticoagulation, infection, and failure;
- tritium isotope separation, distillate/concentrate activity, assay uncertainty, water certification, cryogenic temperature/viability, culture revival, and contamination;
- mile-castle construction, garrison supply, caravan route confidence, trade events, crop/root/roast yields, meal effects, spoilage, and maintenance;
- trap placement, pneumatic propagation, creature response, friendly access, false confidence, and tactical uncertainty;
- sextant observation/refraction/time uncertainty, argon laser output/cooling, ELF skin-depth/inversion, and follow-up confirmation;
- SEM vacuum/resolution/sample damage, image provenance, research unlock requirements, adaptive OFDM/STBC capacity, burst loss, latency, and retries;
- obelisk mass/raising/foundation, electrum/glyph provenance, century criteria, 2D presentation, replay, checksum, and campaign-save isolation;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 401–416 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Maritime, medical, water, biological, civil, agricultural, defense, precision, geophysical, radio, and cultural claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
