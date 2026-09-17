# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 7)

**Generated:** 2026-08-19<br>
**Status:** Repository-grounded implementation plan; ready for adversarial review<br>
**Batch:** 7 — Steps 97–112<br>
**Host engine:** Godot 4.7+ (.NET 8 C#)<br>
**Core target:** .NET Standard 2.1<br>
**Data authority:** Assets/StreamingAssets/Data/<br>
**Active architecture:** Godot host + engine-agnostic Assets/Ashfall.Core/<br>

---

## 1. Executive outcome

Batch 7 has a strong late-game identity: it turns repaired infrastructure, medical knowledge, communications, and social memory into systems that make the shelter feel capable without making it safe. The original sixteen ideas are useful, but they are not sixteen independent UI tickets. Several depend on Batch 6 foundations that are not yet authoritative in the repository, while others currently exist only as narrative catalogs, placeholder panels, or isolated widgets.

This plan therefore treats Batch 7 as a dependency-first set of vertical slices. Each slice must establish one authoritative Core state owner, one data contract, one Godot adapter, one save envelope, and one test seam before its presentation layer is considered complete.

The quality target is:

    canonical data
        -> deterministic Core state
        -> host session / event wiring
        -> Godot panel and input
        -> checked-summed save
        -> Core tests + headless integration smoke

The plan also corrects several overly precise or physically misleading acceptance claims in the source roadmap:

- A sealed rail tunnel may reduce radiation exposure; it cannot promise zero exposure without a route shielding definition and an actual exposure calculation.
- Blood transfusion compatibility depends on the product and ABO/Rh rules; an O-negative red-cell donor is not a universal answer for every blood product.
- Cloud seeding can alter precipitation and local fallout deposition probabilistically; it cannot neutralize radioactivity or guarantee a ten-mile diversion.
- Surface washing can remove loose contamination from a can; it cannot remove radionuclides incorporated into the food or make every item safe by assertion.
- Severe radiation exposure should remain predominantly harmful. A rare authored acquired trait may be an outcome, but “radiation creates beneficial adaptive genetics” must not become a reliable optimization strategy.
- A hobby, newspaper, trophy, or automaton should create a bounded state change with a cost, owner, and save representation rather than a permanent magic aura.

### Batch 7 success criteria

Batch 7 is complete only when:

1. the sixteen features have explicit dependency status (ready, blocked, or deferred),
2. no feature introduces a second authority for routes, power, inventory, weather, survivor health, factions, or campaign achievements,
3. all new state is deterministic and round-trips through versioned, checksummed saves,
4. the Godot UI is a projection of Core state and does not resolve gameplay locally,
5. failed operations have visible, authored consequences rather than silent success,
6. the canonical Core and Godot verification commands pass, and
7. adversarial review has closed contradictions in units, balance, persistence, and migration.

---

## 2. Scope, non-goals, and architecture guardrails

### In scope

- Steps 97–112 from the supplied Batch 7 roadmap.
- Core state contracts and integration seams needed to make those steps real.
- Godot panels, host sessions, event wiring, and headless smoke paths.
- New JSON definitions only where existing authority does not already cover the concept.
- Save migration, deterministic RNG, catalog validation, accessibility, and failure-state UX.
- Dependency gates for missing Batch 6 systems such as shared power storage, paper production, and the cross-campaign Hall of Fame/profile layer.

### Explicitly out of scope

- Any new Unity gameplay, Unity scene, prefab, ScriptableObject, UnityEngine dependency, or Unity verification command.
- A second map, route, weather, inventory, power, health, faction, or achievement authority.
- Replacing existing Core systems with feature-local approximations.
- A 3D simulation. The “3D trophy cabinet” remains a 2D Godot presentation of a 3D-looking or illustrated display.
- Arbitrary item IDs, faction IDs, survivor names, reward quantities, or locations invented in code.
- Guaranteed numerical outcomes that are not supported by the active simulation and data.
- A full multiplayer, networking, or cloud-save layer.

### Non-negotiable project rules

- Godot is authoritative; Unity is deprecated legacy architecture.
- Assets/Ashfall.Core/ remains engine-agnostic. No Godot.*, UnityEngine.*, JsonUtility, System.Random, or Guid.NewGuid() in new Core code.
- New gameplay logic belongs in Core. src/ handles input, presentation, host wiring, and platform adapters.
- JSON in Assets/StreamingAssets/Data/ is the single data authority. New definitions use schema_version and snake_case IDs.
- Stateful systems implement CaptureState() and RestoreState() with versioned DTOs.
- Cross-host-compatible serialization uses IJsonSerializer; checksums use the existing save contract.
- Every public state change raises an event or is surfaced through the host’s existing event bridge.
- Dictionary and set serialization is sorted by stable ordinal keys before hashing or writing.
- The project verification path is dotnet plus godot --headless. No Unity commands are part of this plan.

---

## 3. Repository-grounded preflight audit

This plan was written against the current repository seams rather than assuming that the Batch 7 prose already describes implemented systems.

### Existing systems that should be extended

| Batch 7 concern | Existing authority to extend | Current boundary | Plan consequence |
|---|---|---|---|
| Rail transit | LocationLayoutSystem, ExpeditionSystem, Crossing/Vouch systems | No active rail cargo or route-state authority found | Add logistics state only after route access and cargo reservation contracts are defined. |
| Glass and tubes | SilentFoundrySystem, CraftingSystem, ResearchSystem, glass/optics narrative catalogs | Catalogs describe lore and process; they do not currently produce functional glass or repair a transmitter | Add production recipes and device condition through existing crafting/foundry paths. |
| Weather intervention | WeatherSystem, WeatherKind, Year of Ash hazard loops | Weather state and modifiers exist; no cloud-seeding intervention authority found | Add a bounded intervention layer that requests a weather adjustment; do not fork weather rolls. |
| Medical transfusion | MedicalSystem, DiseaseSystem, NeedsSystem, CombatTraumaSystem, CaregivingSystem | No active hemorrhage, ABO/Rh, donor, or cold-chain model found | Add blood products and hemorrhage state with MedicalSystem as the health gate. |
| Aquaponics | GreenhouseSystem, NeedsSystem, greenhouse catalog | Grow plots and contamination exist; no fish tank or nutrient-loop state found | Add a Greenhouse-owned aquaponics extension and convert fish to canonical food through recipes. |
| Bounty contracts | WarlordDoctrineSystem, NarrativeEncounterSystem, ExpeditionSystem, TacticalCombatSystem | Territory danger and combat exist; no contract board or bounty lifecycle found | Extend faction/expedition contracts, never duplicate warlord AI. |
| Radiation decontamination | RadiationSystem, DoseLedgerSystem, inventory, airlock UI | Survivor dose and contamination concepts exist; no standalone item/food wash process found | Add item surface-contamination state and a treatment process with measurable residuals. |
| Geocaches | LocationLayoutSystem, locations catalogs, ProceduralScavengeSystem, ExpeditionSystem, heirloom data | Lore and locations exist; no clue-chain/cache runtime found | Add cache leads that resolve through the existing map and expedition authority. |
| Gazette | JournalSystem, economy/trade, radio session, paper/printing catalogs | Narrative assets exist; no active issue/distribution state found | Add edition state and integrate audience/trade effects through existing systems. |
| Automata | DutyRosterSystem, CraftingSystem, ResearchSystem | No labor-machine state found | Add constrained task-assist devices with power, wear, and supervision costs. |

### Missing or prerequisite authorities identified in the audit

The following were not found as authoritative active Core systems for Batch 7:

- rail lines, handcars, cargo transfers, or underground transit;
- blood loss, ABO/Rh compatibility, donation, blood storage, or transfusion;
- cloud-seeding/weather intervention;
- a shared shelter PowerGrid/PowerStorage authority and lead-acid cell state;
- aquaponics or fish-farm simulation;
- bounty contract lifecycle and board state;
- item-level surface decontamination;
- escape-tunnel excavation and evacuation;
- currency definition, assay uncertainty, or counterfeit resolution;
- radiation-adaptation/acquired-trait authority;
- active paper-mill/printing-press issue and distribution state;
- heirloom geocache clue chains;
- automaton labor contracts;
- cross-campaign achievement/profile persistence.

The current src/UI/AchievementsPanel.cs is placeholder UI with hard-coded stats and achievement strings and a placeholder Bind(object) method. It must not be presented as evidence of an existing achievement authority.

There is a geothermal/thermal slice in the Year of Ash area, including YearOfAshDeepFreezeSystem and GeothermalHeatingWidget. That is useful evidence for integration, but it is not by itself a shared electrical generation, storage, or breaker model. Step 104 must establish or wait for that authority rather than writing battery logic into the panel.

### Relevant data evidence

Existing data may provide starting points, but it does not automatically become runtime behavior:

- deep_lore_locations.json includes a regional blood-bank location and blood-related item references.
- black_flotilla_items.json and the inventory host demo include blood_bag and spoiled_blood_bag, but not typed blood units or compatibility rules.
- disease_catalog.json includes a blood-vector disease, which must remain integrated with sterilization and isolation logic.
- GreenhouseExpansionCatalog contains a Hydro Barons aquaponics location ID, but no fish-cycle state was found.
- locations.json and Year of Ash location data contain flooded subway, geothermal, seed-vault, freezer, salt, rail, and industrial location material.
- Glass, optics, typographic, assay, paper, seed, rail, and wildlife narrative JSON files provide authored records and possible catalog inputs. They must not be treated as executable system definitions without a loader and a Core consumer.
- dweller_heirlooms_master.json and survivor letter data can seed cache leads, but a lead must reference an existing canonical object or be rejected by validation.
- questline_master.json and trade_screen_scenarios.json contain blood-related narrative/trade references, but those references are not a transfusion system.

### Audit rule for implementation agents

Before writing each feature, re-run a targeted repository search and inspect the existing public APIs. If the repository has gained a new authority since this plan was generated, extend it. Do not add the proposed class merely because its name appears in this document.

---

## 4. Dependency map and implementation order

### High-level dependency graph

    Baseline contracts and ID audit
            |
            +--> Cargo / route access ------------------> 97 Rail logistics
            |
            +--> Power generation + storage + cold chain -> 98 Blood bank
            |       |                                      102 Aquaponics
            |       |                                      104 Battery service
            |       |                                      105 Decon bath
            |       |                                      111 Automata
            |
            +--> Foundry / crafting / research ----------> 99 Glassworks
            |                                              111 Automata
            |
            +--> Weather exposure authority --------------> 100 Cloud seeding
            |
            +--> Caregiving / morale / rooms -------------> 101 Hobbies
            |
            +--> Warlord / expedition / combat ------------> 103 Bounties
            |
            +--> Shelter hazard / shielding --------------> 106 Escape tunnel
            |
            +--> Economy / currency -----------------------> 107 Assay
            |
            +--> Radiation / traits / medical --------------> 108 Acquired traits
            |
            +--> Paper / journal / economy / radio --------> 109 Gazette
            |
            +--> Location / heirloom / scavenge ------------> 110 Geocaches
            |
            +--> Campaign event ledger / profile storage ---> 112 Trophy cabinet

### Proposed dependency-first work packages

| Package | Contents | Gate |
|---|---|---|
| W0 — Baseline and contract audit | Verify current APIs, catalog IDs, units, save envelope conventions, and Batch 6 prerequisites | No feature branch begins until the baseline commands are recorded. |
| W1 — Shared resource and event seams | Stable quantity/weight/energy conventions, cargo reservation, cold-chain hooks, event identity, deterministic ordering helpers | Must not become a speculative “universal” framework; add only seams required by a vertical slice. |
| W2 — Transport and shelter infrastructure | 97 rail, 106 escape tunnel | Requires route authority, shelter hazard ownership, and material reservations. |
| W3 — Medical and food safety | 98 blood bank, 102 aquaponics, 105 food decontamination | Requires health/inventory ownership and explicit contamination semantics. |
| W4 — Industry and electrical support | 99 glassworks, 104 battery service | Requires foundry/crafting integration and a shared power/storage authority. |
| W5 — Weather, faction, and exploration | 100 cloud seeding, 103 bounties, 110 geocaches | Requires deterministic weather requests, contract outcomes, and map resolution. |
| W6 — Shelter life and identity | 101 hobbies, 108 acquired traits, 111 automata | Requires bounded morale/trait/task effects and no direct host mutation. |
| W7 — Information and campaign meta | 107 assay, 109 Gazette, 112 achievements/trophy profile | Requires economy currency authority, paper/ink inputs, and separate cross-run storage. |

### Recommended order inside the packages

1. W0 audit and test baseline.
2. W1 only the shared contracts needed by the first slice.
3. Step 97 if Batch 6 bridge/route access is ready; otherwise mark it blocked with a concrete gate.
4. Step 98, because it exercises medical state, inventory, cold chain, and save rules.
5. Step 99, because it exercises foundry/crafting/research and functional equipment repair.
6. Step 100, because weather intervention can affect multiple downstream systems and must be isolated early.
7. Step 101 and Step 102 as separate vertical slices.
8. Step 103 after combat and expedition outcome events are stable.
9. Step 104 only after a shared power-storage model exists; this is a prerequisite, not a panel-only task.
10. Step 105 after item contamination and treatment semantics are explicit.
11. Step 106 after shelter hazard and evacuation state are available.
12. Step 107 after currency and trade settlement are authoritative.
13. Step 108 after radiation outcome events and trait effects are testable.
14. Step 109 after paper, ink, journal, and regional distribution are real.
15. Step 110 after clue resolution and location-node creation are stable.
16. Step 111 after duty-roster task ownership and power consumption are stable.
17. Step 112 after campaign event aggregation and profile storage are stable.

If a prerequisite is absent, the ticket stays in blocked or deferred status. A placeholder panel is not a valid substitute for the prerequisite.

---

## 5. Shared implementation contract

### 5.1 Core state shape

Every new system should have:

- a stable SystemId;
- a serializable state DTO with schema_version at the envelope or contract level;
- explicit IDs for all referenced locations, survivors, factions, items, recipes, routes, and devices;
- an event or result object for successful and failed commands;
- CaptureState() and RestoreState();
- a migration path for the first version and a rejection path for future versions;
- stable ordinal ordering for lists derived from dictionaries;
- a test that restores a mutated state and compares every gameplay field.

Do not store a live Node, Control, AudioStream, Texture2D, GodotObject, or Unity type in Core state.

### 5.2 Resource and unit discipline

The batch contains several claims that mix mass, volume, power, and time. Before implementation, each domain must record its unit in the field name or DTO documentation:

- quantity for discrete item units;
- mass_kg for cargo;
- volume_l for water, blood, fuel, and chemical fluids;
- power_kw for instantaneous generation/load;
- energy_kwh for stored or accumulated electrical energy;
- temperature_c for thermal conditions;
- dose using the project’s existing radiation unit and conversion rules;
- hours or days rather than an unqualified duration.

If the repository already exposes a canonical unit or conversion, use it. Do not create parallel conversion constants in each system.

### 5.3 RNG and event identity

Use the host-provided ISeededRng for outcomes. A roll must be reproducible from a stable seed, system ID, entity ID, day/tick, and roll index. Never use object hash codes, dictionary iteration order, wall-clock time, or a generated GUID for outcome identity.

Each externally visible event needs an idempotent identity such as:

    <system_id>:<entity_id>:<day_or_tick>:<sequence>

The exact format must follow existing project conventions. The important properties are stable ordering, save persistence, and no duplicate reward when a host replays or restores an event.

### 5.4 Save and migration

Use the existing save envelope and checksum path. The five previously identified save stores with checksum contracts must remain strict about missing checksums. New stores must not reintroduce the old “missing checksum means legacy” ambiguity.

For each new state:

1. serialize a clean default state;
2. mutate every important field;
3. capture and restore;
4. verify checksum changes when state changes;
5. verify null/empty checksums are rejected for new envelopes;
6. test a previous-version migration fixture;
7. test an unsupported future version fails clearly.

### 5.5 Host and UI discipline

The Godot panel should:

- bind to a typed host session or view model, not object or placeholder arrays;
- issue commands to Core through the host;
- subscribe to state/result events;
- render disabled, blocked, failed, and completed states;
- be usable with keyboard focus and readable labels;
- avoid starting its own simulation timer when ISimClock already exists;
- not deduct inventory, health, power, or reputation directly.

For UI ideation, google-stitch may propose a layout, but the proposal remains subordinate to the existing Godot theme, responsive rules, accessibility, and authoritative Core state.

### 5.6 Composition-root discipline

Do not make src/Main.cs a larger god object. Add one domain setup/save/flush triad in the appropriate partial host file, or extract a focused host coordinator when the domain already has one. Every setup path must have a matching save path and headless smoke registration.

---

## 6. Detailed implementation plans

## [97] Subterranean Rail Logistics & Diesel Handcar Transport

### Quality intent

Turn repaired underground routes into a controlled cargo-transfer operation. The feature should solve a logistics problem without bypassing route access, cargo weight, fuel, crew assignment, derailment risk, tunnel hazards, or inventory transactions.

### Current seam and dependency

Extend LocationLayoutSystem, ExpeditionSystem, Crossing/Vouch access, and existing inventory/warehouse transactions. The audit found no authoritative rail route, handcar, cargo reservation, or underground radiation model. If the Batch 6 bridge/route work does not expose reusable route nodes, this step begins with a route contract and is not allowed to invent a second world graph.

### Core work

Add a focused logistics domain, for example Assets/Ashfall.Core/Logistics/, only after checking for an existing cargo abstraction. The minimum state should represent:

- RailRouteDefinition: canonical route ID, ordered location node IDs, distance, access requirements, tunnel shielding segments, speed, capacity, and hazard profile;
- RailcarDefinition: car ID, cargo capacity in kg, fuel type, base fuel cost, condition, operator requirement, and repair recipe ID;
- RailTransitOrder: order ID, route ID, origin, destination, cargo reservation IDs, assigned operator, phase, elapsed hours, and outcome;
- RailTransitState: unlocked routes, railcar condition/fuel, active orders, completed transfers, and pending hazards.

The route resolver must query canonical location/access state. Cargo must be reserved before departure, removed only at the agreed transfer boundary, and returned or marked lost according to a deterministic failure policy. The railcar must consume fuel and condition over time. The exposure result must use the route’s shielding segments and the existing radiation calculation; “0 radiation exposure” is a possible authored route outcome only if the route definition and shelter rules produce it.

The system must reject:

- unknown route or location IDs;
- cargo over capacity;
- unowned or already-reserved inventory;
- missing operator or access vouch;
- insufficient fuel or condition;
- duplicate order IDs after restore.

### Godot host and UI

Create src/Host/RailLogisticsHostSession.cs and src/UI/RailcarTransitPanel.cs only if no existing logistics host can own them. The panel should show a route diagram, access/repair status, cargo manifest with mass totals, fuel/condition, expected duration, exposure estimate with uncertainty, and a confirmation step. During transit, display progress and hazard outcomes; do not allow the UI to teleport cargo.

If a route repair scene is needed, use a Godot .tscn under the Godot asset tree. No Unity scene or prefab is permitted.

### Data

Prefer extending existing location/route data. If a new file is necessary, create a schema-versioned file such as rail_routes.json with IDs that resolve through CatalogIntegrityValidator. Do not duplicate the location coordinates already owned by the location authority.

### Persistence and determinism

Capture active orders, railcar wear, fuel, reservations, route unlocks, and hazard roll indices. Sort active order IDs before serializing. Persist enough progress to resume at the same hour without rerolling a derailment. Use a stable transit event identity for cargo settlement.

### Tests and acceptance

Add Core tests for:

- route access and vouch rejection;
- capacity and mass conservation;
- fuel and condition consumption;
- deterministic hazard/exposure result;
- failure refund or loss policy;
- save round-trip during transit;
- duplicate completion idempotence.

Acceptance is achieved when an unlocked, adequately repaired route can move a valid cargo manifest to its destination, the transfer appears exactly once in both inventories, the operator and fuel costs are recorded, and a headless restore resumes the same transit outcome. The 500 kg / 2 hour / zero-exposure example is a data fixture, not a hard-coded guarantee.

### Dependencies and risks

Blocked until route access and cargo reservation are authoritative. Main risks are a second map graph, silent inventory duplication, and a “safe underground” label that bypasses radiation math.

---

## [98] Clinical Blood Bank Storage & Emergency Transfusions

### Quality intent

Make blood a perishable clinical resource with real compatibility, donor recovery, storage limits, disease screening, and hemorrhage stabilization. It must remain a medical system, not a UI health button.

### Current seam and dependency

Use MedicalSystem, CombatTraumaSystem, DiseaseSystem, NeedsSystem, CaregivingSystem, and the existing inventory definitions. Existing blood-bag IDs and a blood-vector disease are useful data evidence, but they do not establish ABO/Rh, blood products, or blood loss.

### Core work

Create a typed blood domain under Assets/Ashfall.Core/Medical/ or extend the existing medical state owner. The model should include:

- BloodType: ABO group and Rh factor as explicit enums or canonical strings;
- BloodProductKind: packed red cells, plasma, platelets, or only the products the medical rules support;
- BloodUnitState: unit ID, product, type, volume, collection day, expiry day, storage condition, pathogen-screen status, source survivor, and quarantine state;
- BloodBankState: storage slots, power/cold-chain status, queued donations, discarded units, and transfusion history;
- HemorrhageState: patient ID, active blood loss, severity, source trauma, stabilization status, and elapsed time;
- TransfusionResult: accepted product, compatibility rationale, volume delivered, adverse reaction, health/bleeding result, and consumed supplies.

Compatibility must be encoded and tested per product. Do not use “O-negative is universal” as a universal shortcut. A product may be incompatible, expired, contaminated, overheated, or not clinically indicated. A donor pays a bounded fatigue/health cost through the existing needs/medical pathways and cannot donate while disallowed by disease, anemia, or cooldown rules.

MedicalSystem remains the gate for treatment and health changes. If the trauma model lacks blood loss, add an explicit trauma/medical extension or an adapter result rather than subtracting health directly from the panel.

### Godot host and UI

Implement src/Host/BloodBankHostSession.cs or fold the commands into the existing MedicalHostSession. Build src/UI/BloodBankPanel.cs with:

- refrigerator capacity and power state;
- typed bags with expiry and screening labels;
- donor eligibility and recovery timer;
- patient bleeding severity and urgency;
- a compatibility matrix with a plain-language reason for disabled choices;
- transfusion confirmation and reaction outcome;
- keyboard navigation and color-independent type labels.

The UI may request Donate, Quarantine, Discard, Crossmatch, and Transfuse; Core decides whether each command succeeds.

### Data

Extend canonical medical/item data for blood products, storage requirements, procedure costs, disease-screen rules, and expiry. Migrate generic blood_bag instances explicitly rather than silently treating an untyped bag as a fully compatible unit.

### Persistence and determinism

Persist unit IDs, type/product, collection/expiry days, storage temperature status, crossmatch decisions, donor cooldowns, hemorrhage progress, and transfusion history. Repeated UI clicks or restored events must not consume a unit twice. If a storage failure is rolled, use the existing seeded RNG and persist the roll index.

### Tests and acceptance

Add tests for:

- ABO/Rh red-cell compatibility;
- product-specific compatibility;
- invalid, expired, quarantined, or overheated units;
- donor cooldown and recovery effects;
- disease-screen rejection;
- blood loss stabilization and adverse reaction;
- save round-trip and idempotent transfusion;
- power loss causing cold-chain deterioration without inventing free units.

Acceptance is achieved when a valid donor produces a typed, screened unit, a compatible patient can be crossmatched and transfused, the patient’s bleeding state is stabilized through MedicalSystem, all costs and recovery effects are recorded, and an incompatible or unsafe choice explains its failure. The “O-negative saves a dying patient” scenario must be a valid red-cell fixture, not a universal rule embedded in the UI.

### Dependencies and risks

Needs an explicit hemorrhage state and a cold-chain/power contract. Main risks are health mutation in the host, generic blood bags bypassing disease rules, and balancing blood donation as a free renewable resource.

---

## [99] Foundry Glassblowing & Triode Vacuum Tube Fabrication

### Quality intent

Connect the existing foundry, crafting, research, and radio equipment into a production loop where glass quality and vacuum-tube reliability matter. Narrative glass catalogs should enrich the work, not masquerade as a production system.

### Current seam and dependency

Use SilentFoundrySystem, CraftingSystem, ResearchSystem, the glass/optics narrative catalogs, inventory, and the existing radio host/device state. The audit found no active glassworks or triode fabrication authority.

### Core work

Extend crafting/foundry through data-driven recipes and add a focused quality state only where current crafting cannot express it. A GlassworksState may track:

- active batch and workstation;
- glass composition and material inputs;
- heat/anneal stages;
- product quality, cracks, inclusions, and salvage;
- vacuum-pump cycle and leak status;
- operator skill/tool condition;
- completed component IDs and failure history.

Triode production should be staged: glass envelope, filament/electrodes, seal, evacuation, test, and grading. The radio transmitter repair should consume a valid component and use an existing device condition/repair authority. Do not make a repaired transmitter work merely because a narrative catalog entry exists. Research unlocks recipes; it does not itself create physical stock.

All failure probabilities must be deterministic and bounded. A failed batch should return authored salvage where appropriate and record why it failed.

### Godot host and UI

Create src/UI/GlassblowingPanel.cs and a host adapter around foundry/crafting. The UI should show material quantities, temperature stage, anneal timer, pump/vacuum status, operator assignment, tool wear, quality forecast, and result grading. A timing interaction may affect input quality, but the authoritative result must be resolved in Core so headless and UI play agree.

### Data

Add or extend recipes with canonical IDs for silica, glassware, triodes, vacuum-pump supplies, and radio repair. Link narrative log IDs as optional flavor references, not as required runtime outcomes. Add research nodes only after checking the existing ResearchSystem catalog and ensuring CaptureState sorts dictionary-derived IDs.

### Persistence and determinism

Save active furnace/crafting job, heat and anneal progress, vacuum progress, operator, tool condition, roll index, and output reservation. Restoring mid-cycle must not rerun the same failure roll. Sort output and research ID collections before capture/hash.

### Tests and acceptance

Test:

- recipe prerequisites and research gates;
- material conservation and salvage;
- heat/anneal/vacuum state progression;
- deterministic quality grading;
- tool wear and operator modifiers;
- transmitter repair requiring a valid component and power/device condition;
- save round-trip at each major process stage.

Acceptance is achieved when a player can produce a valid triode through the complete process, receive a graded component, install it through the real radio-device repair path, and observe transmitter range/condition change only when the device and power state permit it.

### Dependencies and risks

Requires active device condition/power semantics. Main risks are a second crafting queue, direct transmitter mutation from UI, and “three tubes repairs everything” without quality or installation state.

---

## [100] Wasteland Cloud Seeding & Fallout Rainout Intervention

### Quality intent

Give the player a costly, uncertain weather intervention that rewards forecasting and preparation. Rename the fantasy of “fallout neutralization” into a physically and narratively safer rainout/intervention model: the player can influence where precipitation deposits material, not destroy radioactivity.

### Current seam and dependency

Extend WeatherSystem, WeatherKind, Year of Ash weather/hazard loops, forecast UI, and the existing radiation/exposure calculation. WeatherSystem currently owns deterministic weather transitions and directly derives seeded rolls from its state. Do not add a parallel weather timer or roll schedule.

### Core work

Add a WeatherInterventionSystem only as a request/effect layer, for example:

- CloudSeedingDefinition: reagent ID, launcher requirements, eligible weather kinds, base influence, cost, and safety rules;
- StormCellForecast: source/target region, wind direction, moisture, intensity, arrival window, uncertainty, and forecast ID;
- WeatherInterventionState: active requests, consumed shells, cooldown, deterministic outcome index, changed regional deposition modifiers, and after-action records;
- WeatherInterventionResult: accepted/denied, precipitation shift, local exposure modifier, affected locations, uncertainty, and collateral hazard.

The system should ask the weather authority to apply a bounded, time-limited modifier or scripted transition. The weather authority remains the only owner of the current state. A failed intervention may do nothing, produce earlier rainout, worsen a local acid-rain window, or consume the reagent depending on authored rules. The result must use forecast accuracy, wind, moisture, storm intensity, launcher calibration, and seeded uncertainty.

No code may claim to neutralize radionuclides. Soil and surface contamination are updated through the existing radiation/hazard model after deposition.

### Godot host and UI

Build src/UI/CloudSeedingPanel.cs against a typed host session. Show the forecast window, wind uncertainty, affected sectors, reagent and launcher readiness, probability bands rather than false precision, and consequences for crops, routes, and shelter exposure. The panel must distinguish forecast from outcome and preserve a log entry for the intervention.

### Data

Add canonical intervention definitions and reagent IDs only if absent. Link weather kinds and location sectors through existing IDs. Do not place outcome probabilities in UI code.

### Persistence and determinism

Persist the intervention request, forecast identity, consumed materials, cooldown, effect duration, outcome roll index, and affected regions. Restoring during a storm must produce the same result. If WeatherSystem is touched, add tests around its direct SeededRng strategy and ensure no second roll occurs during intervention.

### Tests and acceptance

Test:

- eligibility and cost rejection;
- forecast uncertainty and deterministic outcome;
- bounded regional deposition change;
- no radioactivity deletion;
- interaction with crops, travel, and shelter hazard loops;
- save/resume across intervention expiry;
- repeated command idempotence.

Acceptance is achieved when a prepared player can launch a valid intervention, see a deterministic but uncertain rainout result, observe the resulting exposure/deposition changes in the real hazard system, and pay a meaningful resource and cooldown cost. “Spares the bunker” is an authored favorable result, not a guaranteed ten-mile rule.

### Dependencies and risks

Requires a clear regional weather/exposure interface. Main risks are duplicate weather authority, deterministic weather drift, and presenting a weather action as a certainty or as radioactivity removal.

---

## [101] Survivor Crafts, Handcrafted Hobbies & Stress Relief

### Quality intent

Give survivors bounded, human-scale ways to recover agency and connection during off-shift time. The system should make a carved chess set meaningful without turning one item into a permanent colony-wide stat exploit.

### Current seam and dependency

Extend CaregivingSystem, SurvivorNeedsState, traits, room assignment, CraftingSystem, and the existing item/object catalog. No dedicated hobby state was found.

### Core work

Create a LeisureCraftSystem or carefully scoped extension with:

- activity definitions: labor time, materials, workstation/room, skill, social tags, and output;
- active sessions: survivor, activity, start day/tick, progress, interruption, and quality;
- bounded effects: stress reduction, morale recovery, relationship interaction, or temporary room ambience;
- output objects: item/artifact ID, creator, quality, placement room, and durability or display state;
- capacity and conflict rules so the same survivor, room, and materials cannot be assigned twice.

Use the existing needs/morale mutation path. Do not write directly to the survivor health/morale fields from the host. Any room beauty or social effect should be a documented, capped modifier with decay or replacement rules.

### Godot host and UI

Implement src/UI/HobbyCraftPanel.cs with activity cards, requirements, expected duration, stress/morale forecast, participant slots, interruption reasons, and a finished-work gallery. The common-room display should be a real placement action, not an automatic global buff. The panel should render author, quality, and current effect duration.

### Data

Define activities and outputs in a schema-versioned catalog. Reuse existing wood, leather, paper, instrument, and floral item IDs when available. Validate all skill and item references.

### Persistence and determinism

Save active sessions, progress, assigned participants, consumed materials, output IDs, room placement, and effect expiry. Quality outcomes use ISeededRng and stable activity/session identity. A restored session must not consume materials or apply morale twice.

### Tests and acceptance

Test:

- activity eligibility and material reservation;
- progress and interruption;
- deterministic quality and output;
- bounded stress/morale effect;
- social/room capacity;
- save round-trip and duplicate completion;
- accessibility-facing view model data for labels and disabled reasons.

Acceptance is achieved when an off-shift survivor can complete a valid craft, receive a real authored output, recover a bounded amount of stress through the existing needs path, and place the item in a room where its documented effect is visible and time-bounded.

### Dependencies and risks

Requires a canonical morale mutation/event path. Main risks are permanent stacking buffs, a second crafting queue, and a UI-only gallery with no saved object.

---

## [102] Subterranean Aquaponics & Closed-Loop Fish Hatchery

### Quality intent

Build a living food-production loop that links fish biomass, water quality, oxygen, feed, biofiltration, greenhouse beds, power, and kitchen recipes. Fish must be an input to the food system, not a direct cure button.

### Current seam and dependency

Extend GreenhouseSystem, NeedsSystem, water/power authorities, RecipeCatalog, and the existing greenhouse expansion/location data. The Hydro Barons aquaponics location ID is not itself a functioning tank.

### Core work

Add an aquaponics extension in the Greenhouse domain with state for:

- tank IDs, volume, water temperature, oxygenation, and contamination;
- fish species, life stage, biomass/count, health, feed reserve, and mortality;
- biofilter maturity, nitrogen conversion, substrate condition, and maintenance;
- linked grow-bed IDs and nutrient contribution;
- power load, pump state, water input/output, and outage behavior;
- harvest batches with quantity, mass, quality, and food-safety status.

The system should expose daily/hourly updates through the existing simulation clock. Use deterministic growth/mortality calculations and saved progression indices. Greenhouse plots remain owned by GreenhouseSystem; aquaponics contributes a defined nutrient/water effect through an interface or event rather than rewriting plot internals.

Fish harvest must create canonical food ingredients that flow into kitchen recipes. Scurvy prevention depends on the resulting meal’s nutrition and the NeedsSystem, not on a fish-harvest flag.

### Godot host and UI

Build src/UI/AquaponicsPanel.cs showing tank status, oxygen, biofilter, fish health, feed, water/power consumption, bed links, harvest readiness, and failure warnings. Include an outage mode and a do not harvest or quarantine state when contamination is unsafe.

### Data

Add species, feed, tank, harvest, disease, and recipe definitions only where missing. Link location_hydro_barons_aquaponics through the existing catalog and ensure IDs pass integrity validation.

### Persistence and determinism

Save every tank and linked-bed state, fish cohort state, feed, oxygen/pump condition, contamination, harvest batch, and roll index. Sort tank/cohort IDs before serialization. A restore at the same day must not double-grow fish or duplicate harvest.

### Tests and acceptance

Test:

- water/power/feed consumption;
- oxygen and biofilter thresholds;
- contamination/quarantine;
- fish growth, mortality, and harvest;
- greenhouse nutrient link without plot duplication;
- food recipe integration and nutrition;
- outage and save-round-trip behavior.

Acceptance is achieved when a commissioned tank can sustain a valid cohort, route waste-derived nutrients to linked beds, produce a food-safe harvest under maintained conditions, and expose meaningful losses when water, power, feed, or biofiltration are neglected. No test should assert that fish alone cures scurvy.

### Dependencies and risks

Requires water and power accounting. Main risks are a second greenhouse tick, free food generation, and an opaque biological simulation with no failure feedback.

---

## [103] Wasteland Bounty Hunting Board & Outlaw Contracts

### Quality intent

Add high-stakes, optional expedition contracts that make faction intelligence and combat preparation matter. A bounty is a contract with an issuer, target evidence, route risk, expiry, and settlement—not a generic combat button.

### Current seam and dependency

Extend WarlordDoctrineSystem, NarrativeEncounterSystem, ExpeditionSystem, TacticalCombatSystem, faction standings, and inventory rewards. Warlord territory already contributes travel danger; the bounty system must consume that modifier rather than fork faction AI.

### Core work

Create a BountyContractSystem under the faction/expedition domain with:

- contract ID, issuer faction, target ID, target type, legal/status tags, and canonical location;
- evidence quality, last-known sighting, route uncertainty, and intel expiry;
- required squad/gear thresholds and optional stealth/combat approaches;
- accept/abandon/expire/complete/fail states;
- target encounter identity and combat linkage;
- reward bundle, standing change, collateral consequence, and settlement flag.

Targets and rewards must come from data. Do not hard-code “Iron Jaw,” a fixed 100-round reward, or an arbitrary boss unless those are canonical definitions. The target may escape, be absent, be replaced by a lesser encounter, or produce evidence that the bounty was false. The combat result is resolved through TacticalCombatSystem and returned to the expedition.

### Godot host and UI

Add src/UI/BountyBoardPanel.cs and a host session or extension to the existing faction/expedition session. Show wanted record, evidence confidence, route hazards, deadline, legal/ethical consequence, squad readiness, and reward terms. The panel must support accepting and declining without treating decline as a failure.

### Data

Create a schema-versioned bounty/contract catalog or extend existing narrative/faction data. All target, location, faction, item, and reward IDs must resolve. Add outcome text for success, failure, abandonment, and false lead.

### Persistence and determinism

Save accepted contracts, generated target identity, sighting data, expiry, squad assignment, combat link, and settlement state. Contract reward settlement must be idempotent. Sort contract IDs in save/hash output.

### Tests and acceptance

Test:

- issuer/standing requirements;
- target and location resolution;
- expiry and abandonment;
- route danger integration;
- combat success/failure mapping;
- reward and reputation settlement exactly once;
- save/resume before and after combat.

Acceptance is achieved when a valid contract can be accepted, dispatched through the real expedition system, resolved through tactical combat or an authored non-combat outcome, and settled with the correct data-defined rewards and standing effects.

### Dependencies and risks

Requires stable combat completion events and contract settlement. Main risks are duplicating warlord doctrine, reward inflation, and presenting violence as the only meaningful faction interaction.

---

## [104] Lead-Acid Battery Chemistry & Cell Reconditioning

### Quality intent

Make stored electricity a maintained resource. Battery service should improve capacity or reliability by consuming time, parts, electrolyte, tools, and safety margin; it must not be hidden in PowerGridPanel.cs or ChemicalDependencySystem.

### Current seam and dependency

This step is blocked until a shared electrical distribution/storage authority exists. A geothermal heating/thermal slice is present, but the audit did not find a canonical PowerGrid/battery cell model. ChemicalDependencySystem models survivor substance dependency and must not become the owner of industrial battery chemistry.

### Core work

First define or locate the shared power contract. It should distinguish:

- instantaneous generation and load in kW;
- stored energy in kWh;
- battery bank capacity, charge, health, and reserve;
- room/device breaker state;
- generator, renewable, geothermal, and battery source contributions;
- outage and brownout consequences.

Then add a BatteryMaintenanceSystem or power-storage extension with:

- bank/cell IDs and topology;
- charge, capacity, internal resistance, temperature, sulfation, and condition;
- electrolyte availability and required tools/protective equipment;
- maintenance stages, skill, time, hazard, and failure result;
- recovered capacity and remaining defects.

The system should model acid exposure as a hazard/event and use existing medical/needs consequences if a worker is injured. Do not give the player 25 kWh merely for pressing a button. Capacity recovery must be bounded by cell state and recipe data.

### Godot host and UI

Extend the real power host with src/UI/BatteryMaintenancePanel.cs. Show bank reserve, load, cell diagnostics, electrolyte/tool costs, worker safety, expected recovery range, and live outage implications. The existing power grid panel may present the result but must not own the state.

### Data

Add canonical battery banks, cell maintenance recipes, acid/electrolyte items, tool requirements, and hazard definitions. Reuse existing item IDs where possible and validate every reference.

### Persistence and determinism

Save cell condition, charge, capacity, temperature, active service stage, worker, material reservations, hazard roll index, and last maintenance result. Energy conservation must hold across save/load. Stable-sort cell IDs before capture.

### Tests and acceptance

Test:

- power generation/load/storage conservation;
- service eligibility and material reservation;
- capacity recovery bound;
- acid hazard and failure consequences;
- brownout behavior;
- deterministic maintenance result;
- save round-trip and no duplicate capacity gain.

Acceptance is achieved when servicing a damaged bank changes its real capacity/condition within data-defined bounds, changes future outage behavior, consumes actual supplies and labor, and appears in all consumers of the shared power authority.

### Dependencies and risks

Hard-blocked by the power-storage authority. Main risks are a UI-only battery minigame, double-counted energy, and using survivor chemical-dependency state for industrial chemistry.

---

## [105] Ultrasonic Radiochemical Decontamination of Canned Food

### Quality intent

Let players make informed decisions about contaminated salvage. The system must distinguish loose surface contamination from intrinsic contamination, processing risk, water/chemical cost, and a measurable post-treatment test.

### Current seam and dependency

Extend ProceduralScavengeSystem, RadiationSystem, inventory, water/chemical processing, and medical/food safety. Existing canned_food and spoiled_canned_food references are not sufficient to represent dose or surface contamination.

### Core work

Add an item-level contamination contract, either in inventory or a focused ItemDecontaminationSystem, with:

- contamination class: loose surface, container breach, intrinsic/embedded, or unknown;
- initial contamination amount and decay/transfer behavior;
- item integrity and food-safety state;
- treatment recipe, bath condition, water/acid/energy cost, and throughput;
- residual contamination measurement and test confidence;
- disposal or quarantine result when treatment is unsafe.

The treatment can remove or reduce loose contamination according to the existing radiation semantics. It cannot remove intrinsic contamination or repair a breached can. If the project’s existing dose unit distinguishes external surface dose from ingested dose, use that distinction. If not, define it before implementing the UI.

The workflow should be: inspect, quarantine, process, drain/rinse, test, approve/discard. Processing a can should not automatically make it safe; the result is based on item state, bath quality, operator/tools, and deterministic uncertainty.

### Godot host and UI

Build src/UI/DeconKitchenPanel.cs with batch slots, contamination class, item integrity, water/chemical/energy cost, processing stage, residual measurement, and food-safety decision. Display uncertainty plainly and never use a green icon to imply “all radiation removed.”

### Data

Define treatment recipes, bath capacity, safe thresholds, contaminated item variants, and disposal outputs in canonical JSON. Reuse water, acid, dosimeter, and protective-equipment IDs from the active catalogs.

### Persistence and determinism

Save batch membership, item condition, contamination, bath state, process progress, test result, and outcome identity. Ensure a restored batch cannot be processed twice or duplicate clean rations.

### Tests and acceptance

Test:

- surface versus intrinsic contamination;
- breached-container rejection;
- water/chemical/energy consumption;
- residual dose and test threshold;
- operator/tool modifiers;
- deterministic outcome and save round-trip;
- food inventory and disease-risk integration.

Acceptance is achieved when a contaminated but intact item can be processed and tested through the real inventory/radiation path, producing a measured safe or unsafe result with correct costs. The ten-can example is a fixture whose result depends on authored contamination and process conditions, not a universal 100% rule.

### Dependencies and risks

Requires explicit item contamination semantics. Main risks are deleting radiation by washing, bypassing foodborne disease, and silently changing an item ID without preserving its state.

---

## [106] Covert Smuggling Tunnel & Emergency Escape Hatch

### Quality intent

Create a costly, imperfect contingency route. An escape tunnel should increase options during a siege while introducing construction risk, concealment maintenance, capacity limits, supply decisions, and possible discovery.

### Current seam and dependency

Extend MaterialShieldingSystem, ShelterHazardLoop, shelter infrastructure, survivor roster, inventory, and faction/raid outcomes. No active tunnel excavation or evacuation authority was found.

### Core work

Create a shelter infrastructure state with:

- tunnel ID, start/end location IDs, length, excavation phase, structural integrity, shoring, ventilation, and flood state;
- concealment rating, discovery risk, exit condition, lock/obstruction state, and inspection schedule;
- evacuation capacity, time, survivor selection, cargo limits, and destination hazards;
- construction labor/material reservations and cave-in outcomes;
- emergency activation policy and post-evacuation shelter state.

Material shielding should contribute to tunnel exposure where appropriate, but the tunnel system owns neither general radiation nor all shelter hazards. Evacuation must be a real choice: leave some inventory, expose survivors to weather/radiation, risk casualties, or remain behind. “Saves dweller lives and vital seed supplies” is a possible outcome only when capacity, supplies, route, and destination conditions support it.

### Godot host and UI

Add excavation and evacuation views to src/UI/ShelterOperationsPanel.cs or a focused EscapeTunnelPanel.cs. Show progress, structural risk, concealment, exit readiness, capacity, estimated route exposure, and survivor/cargo selection. A confirmation modal must summarize irreversible consequences.

### Data

Add tunnel project definitions, construction recipes, exit locations, hazard tables, and evacuation policies. Link all exits to existing location IDs and route/access rules.

### Persistence and determinism

Save construction progress, materials, integrity, concealment decay, active hazards, evacuation sequence, selected survivors/cargo, and settlement result. Event IDs must prevent repeated evacuation rewards or duplicate roster entries.

### Tests and acceptance

Test:

- construction progress and cave-in/shoring;
- concealment/discovery changes;
- capacity and cargo limits;
- exposure and destination hazard calculation;
- partial evacuation and casualty outcomes;
- save/resume during excavation and evacuation;
- no duplicate survivors or inventory.

Acceptance is achieved when the player can complete a valid tunnel project, inspect its real readiness, and use it in an emergency with stateful capacity and consequences. A tunnel that guarantees escape without route hazards is not complete.

### Dependencies and risks

Requires shelter hazard and evacuation state. Main risks are a free fail-safe, game-over bypass, and a second shelter map/structural authority.

---

## [107] Precious Metal Assay & Counterfeit Coin Detection

### Quality intent

Make currency quality and merchant trust legible without giving the player magical omniscience. Assay is a measurement process with tools, skill, sample cost, uncertainty, and a trade consequence.

### Current seam and dependency

Extend DynamicEconomySystem, trade settlement, GoodsCatalog, SilentFoundrySystem, and mint/currency authority from the prior batches. If no canonical currency definition exists, Step 107 starts with that prerequisite rather than placing coin truth in AssayModal.cs.

### Core work

Add an economy assay domain with:

- currency/coin definition, denomination, declared metal, weight, and purity range;
- specimen/lot identity and ownership;
- assay method, tool condition, sample/destruction cost, tester skill, and measurement error;
- observed weight/density/acid response and confidence interval;
- authenticity state: accepted, suspect, counterfeit, or unresolved;
- trade decision and relationship/heat consequences.

The assay should not reveal the hidden truth for free. A skilled tester with a maintained scale and touchstone narrows uncertainty; destructive tests may consume a sample. Counterfeit rejection must affect an actual transaction, not award a generic resource just because a button was pressed. Merchant intent, trust, and regional norms remain data-driven.

### Godot host and UI

Build src/UI/AssayModal.cs with balance readings, density calculation, acid/touchstone result, uncertainty, sample cost, and accept/reject/hold choices. Use text and icon cues in addition to color.

### Data

Add canonical currency, purity, counterfeit, tool, and assay method definitions. Reuse GoodsCatalog and dynamic price rules. Do not create a second value system for coins.

### Persistence and determinism

Save specimen identity, observed measurements, assay attempts, tool condition, accepted/rejected settlement, and any reputation result. Deterministic measurement noise must use ISeededRng and a stable specimen/attempt identity.

### Tests and acceptance

Test:

- authentic and counterfeit samples;
- measurement uncertainty and tester/tool modifiers;
- destructive versus non-destructive assays;
- trade settlement and trust effect;
- repeated assay idempotence;
- save round-trip and stable result.

Acceptance is achieved when a player can inspect a real incoming lot, spend an appropriate sample/tool/time cost, receive a confidence-aware result, and make a transaction decision whose economy and relationship consequences are recorded.

### Dependencies and risks

Requires currency authority. Main risks are a binary lie detector, arbitrary market manipulation, and a panel that disagrees with trade settlement.

---

## [108] Survivor Radiation Exposure Outcomes & Bounded Acquired Traits

### Quality intent

Represent rare, authored post-exposure outcomes without presenting radiation as a reliable path to power or as a deterministic inherited evolution system. The default outcome of severe exposure remains illness, disability, chronic damage, or death; a rare acquired trait is one possible narrative branch with trade-offs.

### Current seam and dependency

Extend RadiationSystem, DoseLedgerSystem, SurvivorNeedsState, medical diagnosis, and TraitDefinition. The audit found no mutation/adaptation authority. Radiation must raise an exposure/outcome event; it must not directly hand out a trait from the UI.

### Core work

Prefer a neutral name such as RadiationOutcomeSystem or AcquiredTraitSystem. Its state should contain:

- survivor ID, exposure episode ID, diagnostic day, and eligibility evidence;
- outcome category and authored trait ID;
- probability/uncertainty policy from data;
- adverse effects, treatment implications, and incompatibilities;
- player/clinician acknowledgement if the UI presents an irreversible choice;
- event history and permanence flag.

Outcomes must be authored and bounded. Do not model germline inheritance or claim that a mutation will benefit descendants unless a separate, carefully reviewed generational mechanic exists. Avoid cosmetic portrait changes unless assets and accessibility behavior are explicitly available. Every beneficial modifier needs a cost or limitation, and the feature must not reward intentionally exposing survivors to radiation more efficiently than protecting them.

The system should integrate with the existing radiation thresholds and medical pipeline. It should be possible for an episode to end in no acquired trait, chronic illness, or death. The survivor’s health and dose remain owned by the existing systems.

### Godot host and UI

Build src/UI/RadiationOutcomePanel.cs or a survivor-dossier section. Use restrained clinical language, show evidence and uncertainty, list risks and treatment options, and avoid a celebratory “DNA upgrade” presentation. Do not force a cosmetic portrait mutation to communicate the result.

### Data

Create a reviewed, schema-versioned acquired-outcome catalog. Every trait and affliction ID must resolve. Include contraindications, adverse effects, and rarity bounds. Add a data rule preventing an outcome from having only positive effects.

### Persistence and determinism

Save exposure episode identity, diagnostic state, outcome roll, selected trait/affliction, acknowledgement, and all modifiers. The same episode must not reroll after restore or repeated diagnosis.

### Tests and acceptance

Test:

- exposure event eligibility;
- deterministic roll and no duplicate trait;
- default harmful/no-trait outcomes;
- trait/adverse-effect compatibility;
- interaction with dose, health, and treatment;
- save round-trip;
- no inheritance unless explicitly data-enabled and separately tested.

Acceptance is achieved when a severe exposure episode resolves once through the medical/radiation pipeline, records a clinically legible outcome, applies bounded effects through the trait/needs path, and leaves intentional exposure strategically dangerous rather than optimal.

### Dependencies and risks

Requires reviewed trait and medical outcome semantics. Main risks are glorifying radiation, direct stat mutation, permanent positive stacking, and unreviewed biological claims.

---

## [109] Movable-Type Printing Press & “The Ashfall Gazette”

### Quality intent

Turn scarce paper, ink, labor, and information into regional influence. A Gazette issue should be a produced, authored object with content selection, production quality, distribution, audience, and measurable but bounded consequences.

### Current seam and dependency

Extend JournalSystem, DynamicEconomySystem, radio/broadcast session, CraftingSystem, and the Batch 6 paper/ink authority if it exists. Existing paper/printing narrative catalogs are not an active press. If paper production is not ready, Step 109 is blocked or ships with a smaller explicit paper prerequisite.

### Core work

Create a PrintPublicationSystem with:

- issue ID, issue number, season/day, editor/worker, and production phase;
- selected journal/codex/trade/news entries with rights/content flags;
- paper, ink, type, press condition, labor, and power/material costs;
- proofing and print quality, damaged copies, and final circulation count;
- distribution destinations, route availability, audience size, and listen/read relay;
- reputation, trust, trade traffic, faction reaction, and rumor consequences;
- immutable issued copy record for the journal/campaign chronicle.

Article selection should use existing Journal entries and catalog IDs. A newspaper cannot invent a fact that has not been authored or observed by the simulation. Broadcast integration may read an issue through the existing radio path; it must not duplicate the journal.

### Godot host and UI

Build src/UI/GazettePrintingPanel.cs with headline/article slots, resource costs, proof preview, worker assignment, press condition, distribution map, and predicted audience/reputation ranges. Include a failed-print and retraction path. The panel should support viewing past issues through the journal.

### Data

Add issue templates, print materials, distribution policies, and canonical article tags. Use existing item and location/faction IDs. Do not hard-code Issue #1 effects or a +30% caravan multiplier.

### Persistence and determinism

Save active press job, selected entries, reserved materials, issue number, proof result, distribution result, and immutable issue metadata. Issue creation and distribution must be idempotent. Sort article IDs and destination IDs before hashing.

### Tests and acceptance

Test:

- content eligibility and resource reservation;
- paper/ink/press condition costs;
- proof quality and damaged copies;
- distribution route and audience calculation;
- journal/radio/economy integration;
- bounded influence and faction reaction;
- save round-trip and issue uniqueness.

Acceptance is achieved when the player can compose and print an issue from real journal/content entries, distribute it along valid routes, see the issue archived, and observe data-defined regional effects with costs and potential backlash.

### Dependencies and risks

Requires paper/ink production and regional distribution authority. Main risks are a text editor that never creates an object, a global reputation multiplier, and duplicated narrative truth.

---

## [110] Heirloom Geocaching & Buried Pre-War Survival Caches

### Quality intent

Make heirlooms, letters, and cartography clues resolve into optional expeditions without hard-coding one sentimental treasure path. A cache is a clue chain, a location node, a hazard, a search state, and a reward.

### Current seam and dependency

Extend LocationLayoutSystem, ProceduralScavengeSystem, ExpeditionSystem, heirloom/letter data, and map node discovery. Do not build a second map. Existing locations and narrative files must be resolved through the active location authority.

### Core work

Create a CacheLeadSystem or add the lifecycle to the existing scavenge/expedition authority. State should include:

- lead ID and source item/letter ID;
- clue chain step, decipher state, and required skill/tool;
- destination location/node ID, confidence, and map reveal status;
- search phase, excavation progress, noise/radiation/weather risk;
- cache condition, lock/container state, and resolved reward bundle;
- failure, abandonment, and red-herring outcomes.

The system must not consume an heirloom merely to reveal a marker unless the data says so. Riddles should resolve to canonical location IDs; unresolved or contradictory clues should remain unresolved and explain why. The actual search should use existing scavenge hazards and inventory capacity.

### Godot host and UI

Build src/UI/GeocacheExpeditionModal.cs with clue transcription, map confidence, tool/skill requirements, route risk, excavation progress, and reward uncertainty. A metal-detector signal or audio cue may be presentation; the Core owns whether the cache is present.

### Data

Extend heirloom/letter/locations data with explicit lead IDs, clue references, canonical destination IDs, and reward tables. Validate no dangling references or duplicate cache resolution.

### Persistence and determinism

Save each clue step, revealed node, search progress, tool wear, active expedition linkage, and resolved cache identity. A restored or revisited location must not create a second cache reward.

### Tests and acceptance

Test:

- valid and invalid clue chains;
- skill/tool gating;
- map reveal through the existing location authority;
- route/weather/radiation/scavenge integration;
- partial search and abandonment;
- reward uniqueness and save round-trip.

Acceptance is achieved when a canonical heirloom or letter can produce a valid lead, reveal or confirm a real map node, support a risky search expedition, and settle a unique cache result without hard-coded family names or rewards.

### Dependencies and risks

Requires stable location/map reveal and scavenge state. Main risks are a parallel map graph, spoiler-like certainty, and duplicate high-tier loot.

---

## [111] Clockwork Automa & Mechanical Labor Assist Devices

### Quality intent

Automate repetitive work at a cost. An automaton should free a human shift only for eligible tasks while consuming materials, condition, maintenance time, power or winding labor, and supervision.

### Current seam and dependency

Extend ResearchSystem, CraftingSystem, and DutyRosterSystem. No active automaton labor authority was found. Do not add a free worker count to the duty roster.

### Core work

Create a constrained LaborAutomationSystem with:

- automaton blueprint/type, build quality, task whitelist, capacity, and required station;
- assigned task, cycle duration, output rate, energy/winding cost, and human supervision requirement;
- wear, jam, repair, spare-parts, and failure state;
- room/route constraints and conflict with survivor assignments;
- labor-equivalent accounting so the simulation can explain saved hours without creating impossible output;
- decommissioning and salvage behavior.

The duty roster remains the owner of assignment availability. An automaton may satisfy part of a task requirement through a contract or modifier, but it cannot silently replace a specialist or bypass safety, morale, or hazard requirements. Each production system should consume the actual output through its own authority.

### Godot host and UI

Build src/UI/AutomataWorkshopPanel.cs with assembly recipe, task whitelist, duty conflict, winding/power, wear, maintenance queue, and current labor contribution. Show a failure alert that points to a real repair action. Avoid presenting the automaton as a character or as disposable free labor; the tone should remain mechanical and limited.

### Data

Add automaton definitions, research gates, tasks, parts, maintenance recipes, and failure outcomes. Link task IDs to the duty roster/workstation catalog.

### Persistence and determinism

Save machine identity, assembly quality, assigned task, progress, wear, maintenance status, consumed materials, and roll index. Restore must not run a completed work cycle twice. Sort automaton IDs and task records before save/hash.

### Tests and acceptance

Test:

- research/crafting gates;
- task eligibility and duty conflicts;
- output and labor accounting;
- power/winding cost;
- wear/jam/repair;
- no specialist bypass;
- save round-trip and idempotent completed cycle.

Acceptance is achieved when an assembled automaton performs an eligible repetitive task, produces the same canonical output path as a human assignment, consumes upkeep, can jam or require maintenance, and frees only the labor explicitly supported by the task definition.

### Dependencies and risks

Requires duty-roster task contracts and, for powered devices, shared power storage. Main risks are infinite labor, duplicate production, and an automation layer that bypasses every survival trade-off.

---

## [112] Interactive Trophy Cabinet & Cross-Campaign Achievement Vault

### Quality intent

Replace the placeholder achievement panel with a durable, separate campaign-profile ledger that records earned achievements without leaking active campaign state between saves. The cabinet is a presentation of the profile, not the profile authority.

### Current seam and dependency

src/UI/AchievementsPanel.cs currently contains placeholder stats, placeholder achievements, placeholder milestones, and Bind(object). AchievementDetailPanel.cs and the Main wiring are presentation seams only. No active cross-campaign achievement/profile authority was found. This step depends on the Batch 6 Hall of Fame concept if that work is accepted, but it must define a proper profile contract rather than relying on SaveChecksum alone.

### Core work

Create a profile/meta domain, for example Assets/Ashfall.Core/Meta/, with:

- AchievementDefinition: stable ID, display metadata, trigger/event predicates, scope, rarity, and localization key;
- CampaignStatistics: days, survivors, expeditions, production, rescues, deaths, discoveries, and other explicitly defined counters;
- CampaignEventLedger: idempotent event identities from the active campaign;
- AchievementProfileState: earned IDs, first-earned timestamp/day if allowed by project rules, display order, statistics aggregates, trophy placements, schema version, and migration metadata;
- CampaignCompletionRecord: immutable end-state summary, checksum, and reference to the profile updates;
- TrophyCabinetState: artifact IDs, display slots, inspection state, and lore references.

The profile must be stored separately from the campaign save through IFileIO/Godot user storage, not PlayerPrefs and not a global mutable singleton. A campaign event can update the profile only through an idempotent command. Loading a campaign must not retroactively award every achievement unless the event ledger proves the trigger. Corrupt profile data must be quarantined or rejected with recovery guidance.

The system should support a fresh profile, one or more completed campaign records, and migration from an earlier profile version. Decide explicitly whether achievements are account-wide across named save profiles; do not infer it from UI behavior.

### Godot host and UI

Replace placeholder binding with a typed AchievementsHostSession or meta-profile view model. Update src/UI/AchievementsPanel.cs, AchievementDetailPanel.cs, and add src/UI/TrophyCabinetPanel.cs if the cabinet is separate. The UI should show:

- earned/locked status and progress;
- campaign source and date/day where appropriate;
- global versus current-campaign statistics;
- trophy placement and inspection card;
- profile checksum/health status;
- recovery/export/reset actions subject to explicit user confirmation.

Use the existing Godot theme. If an illustrated cabinet is needed, create a 2D scene with layered art/animation rather than introducing a 3D engine dependency.

### Data

Create an authoritative achievement catalog with stable snake_case IDs and event predicates. Do not use display strings as IDs. Add trophy art/lore references only after asset paths and fallback behavior are validated.

### Persistence and determinism

Profile updates must be checksummed, versioned, idempotent, and isolated from campaign saves. Tests must cover a profile update after campaign completion, repeated completion events, corrupted checksum, future version, and two independent campaign saves contributing to one profile without overwriting each other.

### Tests and acceptance

Test:

- event predicate and scope evaluation;
- idempotent award;
- campaign/profile separation;
- profile save/load/checksum;
- corruption and future-version rejection;
- migration from placeholder/early profile format if one exists;
- two completed campaigns aggregating statistics correctly;
- UI view model with no placeholder data;
- trophy placement and stable ordering.

Acceptance is achieved when a completed campaign writes a verified profile record, awards only the achievements proven by its event ledger, displays the corresponding trophy in later runs, and leaves active campaign state isolated. The panel must render actual profile data and contain no placeholder achievement arrays.

### Dependencies and risks

Requires a campaign event ledger and user-file adapter. Main risks are cross-run state corruption, achievement duplication, placeholder UI surviving behind a new cabinet, and use of unsandboxed global storage.

---

## 7. Cross-cutting save, determinism, and integration work

The individual steps above are not complete if their local tests pass while the aggregate host loses state. Every implementation ticket must include the following checks.

### Save topology

Add each system to the appropriate aggregate save envelope and host setup/save/flush triad. Verify that:

- construction order is deterministic;
- restore order respects dependencies;
- event emission during restore is suppressed or classified according to existing save/load conventions;
- all active state is captured, including in-progress work and reservations;
- a save created before the feature can still load with a documented default state;
- a future save version fails clearly.

### Stable ordering audit

The current repository contains state capture paths that iterate dictionaries. When touching ResearchSystem, CombatTraumaSystem, GreenhouseSystem, Warlord state, or any shared aggregate, sort keys before writing lists or computing checksums. Tests should compare two logically identical states built through different insertion orders.

### Host event audit

For each new event, identify:

- the Core publisher;
- the host subscriber;
- the save/analytics consumer, if any;
- the UI consumer;
- the idempotence key;
- the behavior during restore.

Do not use a UI polling loop as the only bridge. Do not introduce a third event bus. Follow the existing host event style until the planned bus consolidation is implemented.

### Data audit

For each new JSON file or definition:

1. add schema_version;
2. use snake_case keys and IDs;
3. resolve every cross-reference through the catalog validator;
4. add a duplicate-ID test;
5. add an availability/day range test where relevant;
6. keep narrative flavor separate from executable parameters;
7. add a migration/default rule if an old item or location ID changes.

### UI audit

Every panel must support:

- no data / no power / no access / busy / invalid selection / failed operation / completed operation;
- keyboard navigation and focus order;
- labels that do not depend on color alone;
- readable numbers with units;
- confirmation for irreversible loss or exposure;
- a journal/log entry for consequential choices;
- responsive behavior at the project’s 1920×1080 target and smaller test viewports.

### Performance audit

Do not add sixteen independent _Process loops. Register simulation work with the existing clock/tick path. Use event-driven redraws, bounded lists, and virtualized or paged journal/board views where needed. Any new batch processing must have a maximum work budget per tick and a headless test for worst-case active entities.

---

## 8. Verification plan

### Required baseline and per-deliverable commands

Run the canonical commands from the repository root after each accepted system, not only at the end:

    dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet build Ashfall.csproj
    godot --headless --path . -- --data-integrity-selftest
    godot --headless --path . -- --bridge-selftest

If a feature adds a dedicated headless probe, run it explicitly and include its output in the handoff. Examples include a rail transfer probe, blood compatibility matrix, aquaponics cycle, weather intervention, profile checksum, or trophy aggregation smoke.

### Verification matrix

| Area | Required evidence |
|---|---|
| Core compile | dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj passes without Unity/Godot references in Core. |
| Core behavior | New unit tests cover success, failure, boundary, deterministic rerun, and save restore. |
| Godot host | dotnet build Ashfall.csproj passes with zero errors and no new warnings. |
| Catalogs | Data integrity self-test reports zero errors and all new IDs resolve. |
| Bridge | Bridge self-test passes; no new shim added without a classified gap. |
| Save | New envelope round-trip, checksum mutation, null checksum rejection, past migration, future rejection. |
| Determinism | Same seed/state produces identical result and serialized state regardless of insertion order. |
| UI | Panel smoke confirms binding to real host data, disabled reasons, keyboard path, and no placeholders. |
| Integration | At least one scenario crosses the feature boundary into inventory, needs, radiation, weather, power, economy, or meta profile as applicable. |
| Migration | Existing pre-feature save loads with a documented empty/default state and no data loss in unrelated systems. |

### Failure handling

If a required command fails, report the exact command, first meaningful error, affected files, and whether the failure predates the ticket. Do not hide a failing baseline behind a feature-specific pass. Do not claim a panel is complete when the Core authority is blocked.

---

## 9. Adversarial review packet for GLM 5.2 and Qwen 3.7 Plus

The two external reviews should attack the plan from different directions. They should receive this plan plus the implementation diff/spec for the specific ticket, not the implementer’s private reasoning.

### Review A — architecture, determinism, and save attack

Use this brief with GLM 5.2:

    You are an adversarial ASHFALL architecture reviewer. The active engine is Godot 4.7+ C#; Assets/Ashfall.Core is engine-agnostic truth; Assets/StreamingAssets/Data is JSON authority; Unity is legacy read-only. Review the supplied Batch 7 plan and the proposed diff for one step.

    Find every violation or likely regression involving:
    1. duplicate authorities for inventory, routes, weather, power, survivor health, factions, or achievements;
    2. Godot/Unity coupling in Core or new JsonUtility/System.Random/Guid usage;
    3. missing CaptureState/RestoreState, schema migration, checksum, or idempotence;
    4. nondeterministic dictionary/list ordering, time-based or object-hash rolls, or rerolls after restore;
    5. host gameplay logic, UI-side mutation, or Main.cs god-object growth;
    6. unresolved/dangling canonical IDs and data authority drift;
    7. incorrect save restore ordering and event emission during load;
    8. hidden resource duplication, unit mismatches, and impossible conservation.

    Return findings in severity order (blocker/high/medium/low), with file/path evidence, a concrete repair, and a missing test for each finding. Do not invent repository APIs. If a premise is not proven by the supplied evidence, mark it as an assumption and recommend a gate.

### Review B — gameplay, UX, and simulation attack

Use this brief with Qwen 3.7 Plus:

    You are an adversarial ASHFALL survival-simulation and UX reviewer. Review one Batch 7 vertical slice against the supplied roadmap and architecture rules.

    Attack:
    1. whether the loop creates meaningful scarcity and choices rather than a free bonus;
    2. whether failure states are legible, reversible where appropriate, and narratively restrained;
    3. whether the acceptance criteria are physically/simulation-wise honest and measurable;
    4. whether edge cases cover outage, contamination, disease, weather, faction response, death, cancellation, and partial completion;
    5. whether the UI communicates units, uncertainty, risk, consequences, and accessibility;
    6. whether rewards inflate existing economy, combat, morale, or progression;
    7. whether the tone remains cold, exhausted, human, and non-glorifying;
    8. whether the feature can be tested headlessly without relying on a visual minigame.

    Return: (a) blockers, (b) balance or UX concerns, (c) exact acceptance-criteria changes, (d) edge-case tests, and (e) one smaller vertical slice that proves the architecture before expansion. Do not propose a new authority unless you can explain why an existing one cannot own the state.

### Review merge rules

- A blocker must be resolved in the implementation plan or the ticket is marked blocked.
- A high-severity finding requires a code/test change or a written rationale accepted by the project owner.
- Conflicting suggestions are resolved by repository authority, not by whichever review has the more attractive UI idea.
- No reviewer may authorize Unity work, a new JSON authority, or a host-side gameplay shortcut.
- Preserve the plan’s corrected acceptance criteria unless repository evidence supports a stronger guarantee.

---

## 10. Risk register and mitigations

| Risk | Affected steps | Mitigation |
|---|---|---|
| Batch 6 prerequisite not actually implemented | 97, 104, 109, 112 | Add explicit gates; do not fake completion with a panel. |
| Duplicate route/map authority | 97, 103, 110 | Extend LocationLayoutSystem/location authority and reuse expedition routes. |
| Health mutated outside medical/needs owner | 98, 108 | Return typed medical outcomes; let MedicalSystem/NeedsSystem apply effects. |
| Power counted twice or not at all | 98, 102, 104, 105, 111 | Establish one power/storage contract with kW/kWh units. |
| Radioactivity treated as removable or beneficial | 100, 105, 108 | Separate deposition, surface contamination, intrinsic contamination, and clinical outcomes. |
| Deterministic save drift | All, especially 97–100, 102–104, 110–112 | Stable IDs/order, persisted roll indices, checksum round-trips, insertion-order tests. |
| Economy/reward inflation | 103, 107, 109, 111 | Data-driven bounded rewards, upkeep, opportunity costs, and settlement tests. |
| Placeholder UI reaches release | 109, 112, all new panels | Typed host bindings, no placeholder arrays, headless view-model smoke. |
| Main.cs composition drift | All | Domain-specific setup/save/flush partials and a setup/save parity checklist. |
| Cross-campaign corruption | 112 | Separate versioned profile envelope, checksums, idempotent event ledger, quarantine path. |
| Minigame-only implementation | 98–100, 104–107 | Core resolves outcomes; UI timing/audio is optional presentation. |
| Scope explosion | All | One system per ticket; ship a narrow vertical slice before catalog breadth. |

---

## 11. Definition of ready and definition of done

### Definition of ready

A Batch 7 ticket may enter implementation only when:

- its authoritative Core owner is named;
- all referenced IDs and data files are identified;
- dependencies are marked ready or blocked;
- units, resource costs, and outcome boundaries are written;
- save fields and migration behavior are listed;
- the UI command/result boundary is described;
- at least one success, failure, boundary, and restore test is planned;
- the adversarial review has no unresolved blocker.

### Definition of done

A ticket is done only when:

- Core behavior is implemented in the correct layer;
- host wiring and typed UI binding are complete;
- data integrity passes;
- state captures/restores with checksum and migration coverage;
- deterministic replay and insertion-order tests pass;
- the canonical five verification commands pass;
- one integration scenario proves the feature affects the real game loop;
- no placeholder data, hard-coded reward, or hidden duplicate authority remains;
- documentation names known limitations and the next safe extension.

### Release slicing recommendation

Do not wait for every fantasy surface in a feature before playtesting. Ship the smallest honest loop first:

- 97: one repaired route, one railcar, one cargo transfer;
- 98: one typed red-cell product, one donor path, one hemorrhage state;
- 99: one glass component and one repairable radio device;
- 100: one forecast window and one bounded rainout result;
- 101: two hobby activities and one display room;
- 102: one tank, one fish cohort, one linked bed, one harvest recipe;
- 103: one data-defined contract with success/failure outcomes;
- 104: one bank with one service procedure after power authority exists;
- 105: one intact contaminated food item and one unsafe case;
- 106: one tunnel project and one partial evacuation scenario;
- 107: one currency and two assay methods;
- 108: one rare acquired outcome plus harmful/no-outcome branches;
- 109: one issue, one distribution node, one archived copy;
- 110: one canonical clue chain and one cache;
- 111: one automaton and one eligible repetitive task;
- 112: one completed campaign event and one trophy record.

Expand catalog breadth, art, audio, and narrative only after the narrow loop has passed Core, save, host, and headless integration checks.

---

## 12. Final handoff checklist

Before handing Batch 7 to an implementation agent, include:

- this plan;
- the exact Batch 7 step being implemented;
- the current git diff limited to that step;
- the relevant Core/host/UI/data files;
- the prerequisite status and any blocked gate;
- the two adversarial review reports;
- the five verification outputs;
- the exact next prompt for the following vertical slice.

The recommended first implementation prompt is:

    Implement ASHFALL Batch 7 Step 98 as the smallest repository-grounded vertical slice: typed red-cell blood products, donor eligibility, one hemorrhage state, crossmatch/transfusion through MedicalSystem, Godot host/UI wiring, JSON authority, checksummed save round-trip, deterministic/idempotent behavior, and Core/headless tests. First inspect the current repository and REPO_REVIEW_REPORT.md. Do not add Unity code, JsonUtility, System.Random, Guid.NewGuid, a second health/inventory authority, or placeholder UI data. If the required cold-chain or hemorrhage seam is missing, implement the smallest Core contract needed or mark the slice blocked with evidence. Run the canonical dotnet and godot verification commands and report PASS/FAIL for each.

If Step 98 is blocked by missing medical/cold-chain contracts, start with Step 99 only if foundry/crafting/device seams are proven ready; otherwise implement the prerequisite contract as its own reviewable task. Keep each accepted deliverable small, reversible, and independently verifiable.

---

**Plan status:** ready for targeted attack by GLM 5.2 and Qwen 3.7 Plus.<br>
**Implementation status:** not started by this planning artifact.<br>
**Repository source changes:** none required to export this plan.
