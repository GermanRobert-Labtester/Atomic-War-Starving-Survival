# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 20)

**Generated:** 2026-08-19<br>
**Status:** Integration plan — repository-grounded and architecture-aligned<br>
**Host Engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core Target:** .NET Standard 2.1<br>
**Scope:** Steps 305–320

## Outcome

Batch 20 is a late-game infrastructure and archival batch. It should turn chemistry, corrosion protection, emergency cardiac care, radioactive deposition monitoring, food preservation, supervised forestry, perimeter defense, heavy machining, optics, geophysics, radio propagation, and historical archiving into durable Core-owned systems.

Build on prior contracts instead of creating parallel authorities:

- **SilentFoundrySystem** and **CraftingSystem** own metallurgy, machining, material quality, condition, and job settlement.
- **MaterialShieldingSystem**, **RadiationSystem**, and **WeatherSystem** own shielding and environmental hazards.
- **MedicalSystem**, **CombatTraumaSystem**, **DiseaseSystem**, and **SurvivorNeedsState** own clinical states and survivor effects.
- **GreenhouseSystem**, recipe data, **GoodsCatalog**, and the meal/effect pipeline own crops, food, and consumables.
- **DutyRosterSystem**, CaregivingSystem, and the Batch 15/19 worksite contracts own supervised labor and safety.
- **LocationLayoutSystem**, **TacticalCombatSystem**, and **WarlordDoctrineSystem** own perimeter layout and combat resolution.
- **ResearchSystem**, **JournalSystem**, **FactionRadioEngine**, and RadioHostSession own scientific observations, authored records, and communications.
- **GenerationalSuccessionEngine**, existing census state, archive contracts, and **SaveChecksum** own century continuity.

PowerGridPanel, District8Accords, and CenturySeed are feature or narrative names unless a typed active contract is confirmed. Do not use them as hidden gameplay authorities. The project remains Godot 2D: archive halls, rotundas, optical benches, and sensor views are represented with 2D scenes/read models rather than a new 3D runtime dependency.

## Batch entry gates

1. **Boron and shielding safety:** define neutron spectrum, material composition, thickness, geometry, dose contribution, maintenance, and worker exposure. No shielding item may claim protection without a radiation transport approximation.
2. **Galvanizing and corrosion:** carry forward sheet, wire, pipe, acid/flux, coating, inspection, environmental exposure, and maintenance contracts. “Immune for 50 years” is not a default corrosion rule.
3. **Emergency cardiac procedure:** extend the clinical trauma model with tamponade, pressure, blood loss, ultrasound evidence, needle risk, deterioration, and follow-up.
4. **Dry deposition and air quality:** unify sticky collectors with the Batch 19 balloon/nephelometer/radiation telemetry contract and explicit decontamination actions.
5. **Food and disease effects:** reuse fermented-tonic and crop contracts; define dose, adherence, exposure, contraindication, and bounded disease-risk changes.
6. **Forestry and supervised work:** extend worksite safety, route, guard/care, tool condition, radiation exposure, and resource regeneration. No infinite timber or UI-only labor.
7. **Perimeter hazard reuse:** extend wire/trip/flare/sensor contracts. Fragmentation traps require safety, arming, blast, friendly access, and countermeasure states.
8. **Heavy machining and optics:** carry forward boring, gear, interferometer, prism, etalon, measurement uncertainty, and consumer-device contracts.
9. **Geophysics and radio:** reuse ERT/RMT/SP/TEM survey semantics and ionosonde/skip-link propagation. All discoveries and messages require confidence and confirmation.
10. **Archive continuity:** define document identity, authorship, preservation media, vault conditions, retrieval, checksum, duplicate handling, and milestone criteria before the eternal hall UI.

## Delivery order

1. **305, 306:** establish shielding and corrosion-resistant industrial infrastructure.
2. **307, 308:** add emergency cardiac and dry-fallout safety telemetry.
3. **309, 311:** extend food fermentation, crop processing, and bounded wellness effects.
4. **310, 312:** extend safe supervised forestry and perimeter defenses.
5. **313, 316:** implement heavy rotary machining and advanced optical filtering.
6. **314, 318:** connect education, microscopy, and mineral identification to research progression.
7. **315:** extend agricultural pest protection on the shared acoustic model.
8. **317:** add vector RMT with survey uncertainty.
9. **319:** connect ionosonde observations to adaptive radio tuning.
10. **320:** finish with an event-backed nitrogen archive and century record browser.

## Step integration matrix

### [305] Boron Neutron-Absorber Shielding — BoronShieldingPanel

**Core integration:** Add a boron-material production and shielding-installation state over brine/mud inputs, SilentFoundrySystem, CraftingSystem, MaterialShieldingSystem, RadiationSystem, storage, and the facility construction contract. District8Accords may provide recipe access but must not own attenuation logic.

**Smallest vertical slice:** Process one boron-bearing feedstock into a measured tile batch, install it on one defined generator-vault surface, and calculate the resulting thermal-neutron attenuation contribution.

**Acceptance gate:** Boron fraction, carbide composition, tile density, thickness, geometry, cracks, mounting, neutron energy band, source intensity, distance, and worker exposure are persisted. The system must distinguish thermal-neutron attenuation from gamma or beta shielding. A 99.9% reduction is a configured material/geometry result, not a universal “radiation zero” switch.

### [306] Continuous Hot-Dip Galvanizing — HotDipGalvanizingPanel

**Core integration:** Extend SilentFoundrySystem and CraftingSystem with pickling, fluxing, zinc-bath coating, wire/bolt/fence inputs, coating thickness, inspection, corrosion exposure, and ShelterOperationsPanel installation.

**Smallest vertical slice:** Clean one steel batch, dip and withdraw it through a temperature-controlled bath, inspect coating thickness, and install one galvanized segment in a perimeter or structure.

**Acceptance gate:** Steel surface condition, acid/flux consumption, zinc mass, bath temperature, line speed, coating adhesion, bath contamination, worker exposure, and weather/radiation corrosion are modeled. Galvanizing reduces corrosion rate; it cannot make steel completely immune for fifty years without a configured durability model and maintenance.

### [307] Cardiac Pericardiocentesis — PericardiocentesisModal

**Core integration:** Add a high-risk cardiac procedure state over MedicalSystem, CombatTraumaSystem, SurvivorNeedsState, clinical inventory, ultrasound/device condition, clinician skill, blood-loss/shock state, and the canonical death/recovery pipeline.

**Smallest vertical slice:** Triage one suspected tamponade, obtain an imperfect ultrasound finding, reserve needle/anesthesia supplies, resolve a guided aspiration attempt, and advance cardiac status.

**Acceptance gate:** Hemopericardium volume, pressure, injury site, pulse pressure, time-to-treatment, ultrasound confidence, needle angle/depth, clinician skill, sterility, aspiration rate, arrhythmia, recurrent bleeding, and follow-up are persisted. The 150 ml value is a scenario input/result, not a universal amount or guaranteed immediate recovery. UI cannot write cardiac output directly.

### [308] Radioactive Dry-Deposition Collectors — StickyCollectorPanel

**Core integration:** Extend WeatherSystem/RadiationSystem telemetry over surface mast equipment, sticky-film consumables, beta-scintillation or detector condition, sample handling, contamination zones, boot/shoe-cover protocols, and shelter hazard alerts.

**Smallest vertical slice:** Run one collector cycle during a wind event, measure a deposition flux with uncertainty, and create a validated worker decontamination recommendation.

**Acceptance gate:** Collection area, exposure duration, wind, particle size, detector calibration, beta response, rain absence, surface accumulation, sample contamination, and action thresholds are saved. A high reading alerts and changes procedures; it cannot automatically clean boots or define all surface areas as unsafe.

### [309] Wasteland Fire Cider — FireCiderKegPanel

**Core integration:** Extend RecipeCatalog/data, GoodsCatalog, CraftingSystem, DiseaseSystem, NeedsSystem, storage, and the Batch 15/19 vinegar-mother contract.

**Smallest vertical slice:** Ferment one barrel from valid vinegar and herb inputs, bottle one measured batch, distribute a dose to one survivor cohort, and record adherence and a bounded respiratory-risk effect.

**Acceptance gate:** Ingredient mass, acidity, fermentation, contamination, dosage, contraindications, storage, adherence, exposure, and disease state are persisted. Fire cider may modify risk or comfort through the existing effect pipeline; it cannot make dwellers immune, erase all coughing, or grant a global +10 stamina effect without explicit modeled support.

### [310] Supervised Forestry Sawmill — SawmillConvictPanel

**Core integration:** Add a forestry worksite/resource state over DutyRosterSystem, CaregivingSystem, LocationLayoutSystem, ExpeditionSystem, prisoner/parole records, radiation exposure, log inventory, sawmill condition, and structural timber outputs.

**Smallest vertical slice:** Survey one safe harvest area, assign a lawful supervised crew, fell and decontaminate one log batch, mill it, and stockpile inspected beams.

**Acceptance gate:** Forest regeneration, tree mass, contamination, radiation dose, route, guard/care capacity, tool fuel, saw condition, labor safety, lumber dimensions, waste, and storage are saved. “Infinite domestic timber” is not accepted; depleted or unsafe stands must remain possible. One hundred beams require enough logs and mill capacity.

### [311] Vertical Eggplant and Smoked Spread — EggplantTowerPanel

**Core integration:** Extend GreenhouseSystem, crop/recipe catalogs, GoodsCatalog, NeedsSystem, cooking fuel, hardwood, storage, and the meal/effect pipeline.

**Smallest vertical slice:** Grow one tower crop, roast a measured harvest, process it into a catalogued spread, and serve one meal.

**Acceptance gate:** Crop cycle, water, nutrients, light, heat, contamination, harvest mass, roasting fuel, processing loss, jar capacity, spoilage, and dining participation are authoritative. Morale and Gourmet Feast are bounded named effects; fifty jars and +20 morale cannot be direct UI assignments.

### [312] Barbed-Wire Apron and Fragmentation Trap — ApronWireTrapPanel

**Core integration:** Extend the Batch 15/19 perimeter-defense contract over LocationLayoutSystem, TacticalCombatSystem, WarlordDoctrineSystem, wire inventory, explosive/fragmentation catalog, arming safety, sentry access, and battlefield visibility.

**Smallest vertical slice:** Place one apron segment and one validated trip device, arm it after a safety check, resolve one crossing, and generate a tactical report with blast/entanglement consequences.

**Acceptance gate:** Slope, wire length, arming state, fuse delay, blast radius, fragmentation, weather, maintenance, friendly access, misfire, countermeasure, and casualty uncertainty are modeled. The feature may deter or damage an assault; it cannot guarantee heavy casualties or an assault break.

### [313] Four-Meter Vertical Turret Lathe — VerticalTurretLathePanel

**Core integration:** Extend SilentFoundrySystem and ResearchSystem with heavy rotary machining, vertical table capacity, tool-turret indexing, ring/runner/armored-swivel definitions, metrology, power, vibration, and installation contracts.

**Smallest vertical slice:** Mount one valid casting, turn one circular component, inspect runout and bore, and install it into one compatible blast-gate or turbine assembly.

**Acceptance gate:** Workpiece mass/diameter, chuck capacity, table speed, tool reach, feed, vibration, material hardness, coolant, power, wear, tolerance, and inspection are saved. A three-meter ring cannot be produced unless the machine, blank, rigging, and installation capacity all exist.

### [314] Brass Armillary Sphere Education — ArmillarySphereModal

**Core integration:** Add an educational artifact/course state over JournalSystem, ExpeditionSystem, DutyRosterSystem, SimClock, the Batch 15 navigation model, materials, and the canonical skill progression authority.

**Smallest vertical slice:** Craft one sphere, deliver one lesson to an eligible student cohort, record the lesson and assessment, and grant one bounded navigation knowledge increment.

**Acceptance gate:** Material quality, teacher availability, lesson prerequisites, attendance, fatigue, comprehension, retention, and skill caps are persisted. Student XP flows through the active progression system. A +25 XP reward is a balance parameter, not a hard-coded guarantee.

### [315] Subterranean Mole-Cricket Disruptors — MoleCricketDisruptorPanel

**Core integration:** Extend the Batch 16/19 acoustic pest contract over GreenhouseSystem, root-zone state, soil/hydroponic beds, power, vibration propagation, transducer condition, and inspection.

**Smallest vertical slice:** Install one bed-sector emitter, run a frequency sweep, measure root damage/pest pressure over one crop cycle, and record energy and maintenance.

**Acceptance gate:** Pest species, soil geometry, frequency response, attenuation, bed structure, crop sensitivity, power, baseline infestation, false positives, and maintenance are authoritative. One hundred percent protection is a configured test outcome only and must not suppress other pest causes or bypass inspection.

### [316] Fabry-Pérot Laser Etalon — FabryPerotPanel

**Core integration:** Extend Batch 15/19 optical manufacturing and interferometer contracts over ResearchSystem, SilentFoundrySystem, optical flats, piezo spacing, laser source, finesse, linewidth, rangefinder device condition, and TacticalCombatSystem consumers.

**Smallest vertical slice:** Align two optical flats, measure one transmission peak/linewidth, assemble a rangefinder module, and use it for one distance estimate.

**Acceptance gate:** Mirror reflectivity, spacing, parallelism, vibration, temperature, laser condition, calibration, fringe/finesse measurement, and range uncertainty are persisted. A rangefinder can improve distance estimation; a +35% sniper accuracy effect requires a named combat modifier and may be reduced by weather, target motion, cover, and operator skill.

### [317] Vector RMT Geophysics — VectorRmtPanel

**Core integration:** Extend the shared geophysical survey state over LocationLayoutSystem, ResearchSystem, ExpeditionSystem equipment, orthogonal magnetic/electric sensors, field orientation, inversion, depth resolution, and geothermal resource catalogs.

**Smallest vertical slice:** Deploy one vector array, collect a valid frequency band, calculate apparent resistivity/phase with confidence, and mark one follow-up geothermal target.

**Acceptance gate:** Sensor orientation, grounding, frequency, ambient noise, station geometry, weather, instrument condition, inversion assumptions, anisotropy, and depth uncertainty are saved. A 300-meter fracture is a probabilistic interpretation requiring confirmation before becoming a well location.

### [318] Polarizing Petrographic Microscope — PolarizingMicroscopeModal

**Core integration:** Add a mineral-analysis record over ResearchSystem, LocationLayoutSystem, DutyRosterSystem, sample inventory, microscope condition, mineral catalog, and discovery/unlock rules.

**Smallest vertical slice:** Prepare one thin section, observe it under crossed polarizers, identify one catalogued mineral class with confidence, and create a research or prospecting lead.

**Acceptance gate:** Section quality, orientation, stage angle, polarizer condition, birefringence data, operator skill, sample contamination, mineral ambiguity, and reference catalog are authoritative. Monazite or rare-earth discovery requires confirmatory evidence and cannot be inferred from a single UI color chart. Geological age estimates need a separate dating model.

### [319] F2-Layer Auto-Tracker — F2AutoTrackerPanel

**Core integration:** Extend RadioHostSession/FactionRadioEngine, ionosonde observations, ResearchSystem, LocationLayoutSystem, adaptive tuning, transmitter/receiver condition, power, and link delivery.

**Smallest vertical slice:** Receive a critical-frequency observation, choose a candidate channel, retune the transmitter, and maintain one link through a bounded time interval with measured quality.

**Acceptance gate:** foF2 uncertainty, frequency range, antenna bandwidth, coil movement, tuning latency, power, noise, day/night changes, weather, station geometry, and failure/recovery are persisted. The tracker improves channel selection; it cannot guarantee 100% uptime or uninterrupted contact.

### [320] Eternal Hall of Records and Nitrogen Vaults — EternalArchivesPanel

**Core integration:** Add an archive facility and preservation state over GenerationalSuccessionEngine, JournalSystem, document/treaty/blueprint identity, SaveChecksum, shelter construction, nitrogen utilities, media condition, and the EpilogueMatrix milestone contract.

**Smallest vertical slice:** Select one signed record set, verify provenance and checksum, place it in one nitrogen vault, and retrieve it through a 2D archive browser.

**Acceptance gate:** Document identity, author/signatories, date, duplicate handling, media condition, vault temperature, nitrogen pressure/purity, seal integrity, retrieval, inspection, and archive capacity are saved. A preservation perk is a configured read/retention benefit; it cannot promise absolute permanence. The 100-year ending is triggered only by actual campaign time and historical criteria, and must be replayable without mutating the campaign incorrectly.

## Shared Core requirements

Every new stateful system or extension must provide:

- CaptureState/RestoreState with versioned DTOs and migrations for supported past versions.
- Future-version rejection, checksum coverage, atomic save integration, and explicit pre-checksum compatibility only where the existing save contract requires it.
- Deterministic seeded outcomes with stable ordinal ordering; no System.Random, Guid.NewGuid(), wall-clock randomness, or unordered collection dependence.
- State-change events for typed host sessions and save orchestration.
- Exact reservation and settlement for brine, boron, zinc, steel, logs, food, fuel, nitrogen, medical supplies, explosives, wire, optical components, and survey equipment.
- Explicit interruption, cancellation, failure, spoilage, contamination, leakage, misalignment, misfire, injury, maintenance, and stale-data outcomes.
- Canonical snake_case IDs and ordinal validation through the data-integrity rules.

Use explicit units: mass_kg, distance_km, duration_hours, power_kw, energy_kwh, pressure_kpa, temperature_c, volume_m3, dose_rate_uSv_h, activity_bq, concentration_mg_m3, altitude_m, frequency_mhz, wavelength_nm, linewidth_hz, uncertainty_nm, signal_db, confidence_fraction, condition_fraction, and population_count.

## Data authority and content work

Extend existing catalogs where possible. Candidate new authority files, only if existing catalogs cannot express the domain, are:

- shielding_material_defs.json
- galvanizing_processes.json
- cardiac_procedure_defs.json
- deposition_sensor_defs.json
- forestry_worksite_defs.json
- perimeter_defense_defs.json
- heavy_machining_defs.json
- optical_filter_defs.json
- geophysical_rmt_defs.json
- mineral_observation_defs.json
- archive_vault_defs.json

Every new or edited authority file requires schema_version, snake_case properties and IDs, duplicate-ID checks, reference validation, range validation, and loader tests. Materials, facilities, procedures, hazards, sensors, resources, research unlocks, historical records, and effects must resolve through the canonical registry.

## Godot host and UI work

Build these panels only after their typed host contracts exist:

BoronShieldingPanel, HotDipGalvanizingPanel, PericardiocentesisModal, StickyCollectorPanel, FireCiderKegPanel, SawmillConvictPanel, EggplantTowerPanel, ApronWireTrapPanel, VerticalTurretLathePanel, ArmillarySphereModal, MoleCricketDisruptorPanel, FabryPerotPanel, VectorRmtPanel, PolarizingMicroscopeModal, F2AutoTrackerPanel, and EternalArchivesPanel.

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

- boron composition, neutron attenuation bands, shielding geometry, corrosion/coating, pipe/metal process quality, and material conservation;
- cardiac triage, ultrasound confidence, aspiration risk, tamponade state, deterioration, death/recovery, and save round-trip;
- deposition collection area, count/activity conversion, sensor calibration, decontamination actions, stale data, and weather variation;
- fermented tonic dose/adherence, crop yields, processing loss, spoilage, meal integration, pest pressure, and bounded wellness effects;
- forestry regeneration, contamination, safe work, guard/care capacity, sawmill output, wire/explosive placement, blast/entanglement, and countermeasures;
- heavy rotary machining capacity, tolerance, vibration, installation compatibility, optical finesse, range uncertainty, and combat consumer modifiers;
- educational completion, skill progression, microscope ambiguity, mineral confirmation, geophysical inversion, depth uncertainty, and follow-up discoveries;
- ionosonde observations, adaptive tuning, frequency drift, fade, link delivery, latency, and uptime measurement;
- archive provenance, checksum, nitrogen condition, media degradation, retrieval, milestone conditions, replay, and campaign-save isolation;
- clean, tampered, missing-checksum, legacy, future-version, interruption, and atomic-write saves.

## Definition of done

Each Step 305–320 must be documented as implemented, explicitly deferred with a dependency, or rejected as unsupported by the current simulation model. An implemented step requires:

1. Core owns the behavior and state.
2. Godot exposes a typed, non-placeholder workflow.
3. Data is schema-versioned, canonical, and reference-valid.
4. Save/load, migration, checksum, atomicity, determinism, and dirty-store flushing are covered.
5. Chemical, medical, industrial, sensor, geophysical, radio, agricultural, and archival claims are backed by tunable data and failure modes.
6. The full Godot-only verification pipeline passes without Unity dependencies.
