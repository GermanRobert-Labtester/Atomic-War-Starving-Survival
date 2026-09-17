# ASHFALL: 2D Atomic-War Survival
## Quality Implementation Plan — Batch 4 (Steps 49–64)

Generated: 2026-08-19<br>
Source: ASHFALL Quality Next Steps Roadmap, Batch 4<br>
Status: Review-ready implementation plan<br>
Host: Godot 4.7+ with .NET 8 C#<br>
Core target: .NET Standard 2.1

## Executive outcome

Batch 4 should be delivered as a sequence of dependency-first vertical slices. The roadmap contains sixteen good product ideas, but several of its proposed implementations would create duplicate authorities, non-deterministic state, or UI-only mechanics if implemented literally.

The central correction is to move Step 64, Multi-Slot Save Manager, to the first engineering phase. A slot-aware aggregate save contract is needed before adding shielding upgrades, debt, generational education, electronic warfare, memorial state, or campaign history. The visible feature number remains 64, but its infrastructure work becomes the release gate for the rest of the batch.

The recommended order is:

1. Audit and save foundation: Step 64.
2. Shelter infrastructure: Steps 49, 51, 58, and 60.
3. Production and equipment: Steps 50, 55, and 56.
4. Economy, faction, and survivor progression: Steps 52, 54, 53, 59, 61, 62, and 63.
5. Electronic warfare and full-batch integration: Step 57 plus the cross-system regression pass.

This plan assumes the existing Core systems and JSON catalogs are the authority. It does not treat similarly named Unity files as active architecture. Existing user changes in the repository must remain untouched while these slices are implemented.

## Repository-grounded baseline

### Active boundaries

- Core gameplay truth belongs in Assets/Ashfall.Core/ and must remain free of Unity, Godot, GodotSharp, UnityEditor, and JsonUtility references.
- Godot presentation, input, scene composition, audio, particles, and host wiring belong in src/ and Godot-native scenes and assets.
- Data authority belongs in Assets/StreamingAssets/Data/. New definitions require stable snake_case IDs and schema_version.
- Assets/_Game/ is deprecated Unity migration material. It is read-only reference material unless a future request explicitly authorizes Unity work.
- State that affects simulation, outcomes, or progression must have CaptureState and RestoreState coverage.
- Godot verification is the only verification path: dotnet and godot --headless. No Unity command is part of this plan.

### Existing authorities that Batch 4 must extend

| Batch area | Existing authority | Planning consequence |
|---|---|---|
| Radiation sheltering | Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs and Radiation systems | Extend the existing attenuation authority; do not create a second wall or radiation model. |
| Foundry | Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs | Use its deterministic production, labor, incident, maintenance, and inventory-port patterns. |
| Crafting and chemistry | Assets/Ashfall.Core/Crafting/CraftingSystem.cs and Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs | Separate production use from consumption and dependency. |
| Debt | Assets/Ashfall.Core/LedgerDebtSystem.cs | Preserve its intentionally simple contract semantics; add caravan credit as a bounded extension. |
| Factions and Muster | Assets/Ashfall.Core/Muster/MusterSystem.cs, CurrentsCatalog.cs, and existing host/UI sessions | Extend the existing faction and access authorities; do not invent a second council or standing system. |
| Combat | Assets/Ashfall.Core/Combat/BallisticsSystem.cs, WeaponConditionSystem.cs, CombatCatalog.cs | Attach modifications to per-instance weapons and feed the existing ballistic resolver. |
| Agriculture | Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs and greenhouse data | Extend the plot/resource model or add a narrow fungal subsystem that shares its resource contracts. |
| Radio | Assets/Ashfall.Core/Radio/FactionRadioEngine.cs and src/Host/RadioHostSession.cs | Electronic warfare must become a saved Core effect that the radio and warlord systems consume. |
| Mortality and rites | FinalWishSystem, VigilStateMachine, MedicalHostSession, and memorial/epitaph data | Add ceremony state around existing death and final-wish events, without replacing death resolution. |
| Generations | Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs and src/Host/ExpansionHostSession.cs | Add education to the existing chapter and age clock; do not add a second child or age simulation. |
| Saves | SaveChecksum, SaveWireContract, five checksummed stores, and many separate src/Host save stores | Build a profile/slot envelope and migrate existing stores behind it. |

### Known gaps that affect scope

- MaterialShieldingSystem currently stores ceiling material in a dictionary, applies an immediate assignment, and has no material transaction or construction job. Its state capture must be made ordering-stable when touched.
- LedgerDebtSystem intentionally models a simple principal, one rate, term, named forfeit, and settlement path. It is not an amortizing credit score system.
- MusterSystem currently resolves approaches and escalation records; it is not yet a full deterministic council vote.
- WeaponConditionSystem and BallisticsSystem already own condition, jamming, degradation, accuracy, damage, range, cover, and critical-hit calculations. A new armory system must compose with them.
- GreenhouseSystem already owns plot growth, water, contamination, blight, harvesting, and deterministic growth rolls. The current batch must not revive retired or ghost item IDs.
- FactionRadioEngine has dictionary iteration and a HashCode-based fallback that are unsafe foundations for new deterministic effects. Step 57 must repair the touched paths.
- GenerationalSuccessionEngine uses a compressed chapter clock of 365 days and handles aging and retirement. Education must be expressed in that clock.
- src/UI/SaveLoadPanel.cs is currently a placeholder with fake five-slot data and button printouts. There is no proven aggregate SaveLoadHostSession.
- Existing host stores persist separate fixed user:// files. They must be routed through slot-specific roots before the UI can claim profile switching.

## Non-negotiable quality rules

### 1. One authority per mechanic

Each feature gets one Core state owner. Godot panels read projections and issue commands. A panel must not subtract inventory, grant a perk, change faction standing, or complete a quest directly.

### 2. Determinism is a contract

New random outcomes use ISeededRng with explicit stream or roll labels. Do not add System.Random, Guid.NewGuid, HashCode.Combine, wall-clock time, unordered dictionary iteration, or filesystem enumeration to simulation logic. Save arrays in canonical order and sort all derived collections before hashing or serialization.

### 3. Resources move through transactions

Every production, repair, trade, debt, construction, medical, or ritual action has a validated cost and an atomic success/failure result. A failed action cannot consume inputs. A successful action cannot mint outputs outside the authority that owns the recipe or event.

### 4. State is saveable before it is visible

Before a panel exposes an action, its Core state must have a DTO, validation rules, round-trip tests, migration behavior, and an event contract. UI-only placeholders are acceptable during discovery but are not feature completion.

### 5. Save compatibility is explicit

New save DTOs use plain CLR fields, stable names, schema_version, explicit version migration, and checksum coverage. Avoid dictionaries and polymorphic fields in wire DTOs unless their ordering and serialization shape are explicitly normalized.

### 6. Fictional, restrained, human tone

New catalogs use fictional places and factions, avoid real-world countries, wars, and people, and keep violence and suffering grounded. Surveillance, debt collection, mental illness, child education, cremation, and survivor death require consent and consequence framing rather than spectacle.

### 7. Every vertical slice has an attack surface

Each implementation must include:

- a Core behavior test;
- a deterministic replay test;
- a save round-trip and migration test;
- an invalid-input or failed-transaction test;
- a Godot headless wiring probe where the host is involved;
- a different-tool review of the diff when two or more coupled variables are introduced.

## Delivery phases and gates

### Phase 0 — audit and save foundation

Implement the infrastructure portion of Step 64 first.

Tasks:

- Inventory every current host save store and identify its state section, path, version, checksum, and restore side effects.
- Define SaveProfileId, SaveSlotId, CampaignMode, SaveManifest, SaveSectionEnvelope, and AggregateSaveEnvelope as Core wire types.
- Add a slot-aware IFileIO adapter and an atomic write strategy that can be tested against a temporary directory.
- Refactor stores to accept a slot root rather than hard-coding user:// paths.
- Add aggregate validation that parses and verifies every section before mutating live simulation state.
- Replace the fake SaveLoadPanel slot list with a host projection over real manifest data.
- Add legacy import for existing single-campaign files; do not delete or overwrite them during migration.

Gate:

- A fresh slot, second slot, reload, checksum mutation, malformed section, and legacy import all have passing Core tests.
- A simulated power loss leaves either the prior valid envelope or the complete new envelope.
- Iron-man policy cannot be changed by editing a panel field or deleting a manifest entry.

### Phase 1 — shelter infrastructure

Implement Steps 49, 51, 58, and 60 as a shared infrastructure family.

Shared contracts:

- Power consumers expose requested load, priority, enabled state, and failure consequence.
- Shelter hazards expose an incident ID, start day, severity, affected systems, repair work, and resolution event.
- Material and waste flows use explicit inventories and contamination flags.
- Weather produces immutable conditions; hazard systems interpret them without mutating WeatherSystem state.

Gate:

- A saved simulation can reproduce the same storm, repair, digester, renewable generation, and shielding outcomes after reload.

### Phase 2 — production and equipment

Implement Steps 50, 55, and 56.

Gate:

- Inputs, outputs, condition, quality, contamination, and dependency effects are visible in both Core tests and Godot panels.

### Phase 3 — economy, institutions, and survivor progression

Implement Steps 52, 54, 53, 59, 61, 62, and 63 in that order.

Gate:

- Faction IDs, survivor IDs, location IDs, quest IDs, education tracks, rumors, camera evidence, and memorial records all resolve through canonical catalogs and survive save/load.

### Phase 4 — electronic warfare and full integration

Implement Step 57 after the radio, power, faction, and save contracts are stable. Then run the full Batch 4 integration matrix.

Gate:

- Jamming, counter-ciphers, decoys, power costs, warlord effects, journal records, and save recovery replay identically from the same seed.

## Detailed vertical-slice plan

## [64] Multi-Slot Save Manager, Iron-Man Mode, and Campaign Chronicle

Although numbered last in the product roadmap, this is the first engineering slice because every other Batch 4 feature adds persistent state.

### Authority and scope

Extend SaveChecksum, SaveWireContract, the existing checksummed save stores, and the Main save orchestration. Treat src/UI/SaveLoadPanel.cs as a view to replace, not as a source of save behavior.

The new Core save contract should include:

- SaveProfileId and SaveSlotId as validated, filesystem-safe, non-random identifiers;
- CampaignMode with normal and iron-man values;
- manifest version, game/build compatibility, current day, seed, and last successful save metadata;
- a fixed, named list of section envelopes;
- per-section schema version and checksum;
- aggregate checksum over canonical section names and canonical serialized section payloads;
- optional derived chronicle summary fields;
- a migration marker for imported pre-slot saves.

Use lists of explicit section records rather than unordered dictionaries in the wire format. If an internal map is useful, normalize it into a sorted list before capture.

### Core work

- Define an ICampaignSaveSection contract for capture, validation, restore preparation, and schema migration.
- Define a SaveSlotService that creates, lists, validates, loads, and deletes slots through injected IFileIO.
- Make section writes atomic: write a temporary envelope, flush or close it, validate the resulting bytes, then replace the active envelope while retaining a recoverable prior version.
- Add corruption quarantine with a reason code and preserve the last known valid save.
- Validate all sections, IDs, versions, and checksums before applying any state to live systems.
- Route every current host save store through the selected slot root. The store itself remains responsible for its section state.
- Add an explicit legacy import path for fixed user:// files. Import must be idempotent and must never silently merge two campaigns.
- Add CampaignMode to authoritative campaign state. Iron-man must reject manual slot restore after a terminal loss according to a Core policy, not a UI checkbox.
- Compute chronicle totals from authoritative state at save or load time. Do not create a second mutable counter for days survived, deaths, discoveries, treaties, or production.
- Keep screenshot thumbnails outside the simulation checksum. The host may store a thumbnail path or binary attachment with size limits and a missing-thumbnail fallback.

### Godot work

- Add a SaveLoadHostSession that owns the slot service, exposes immutable slot cards, and reports validation errors.
- Replace the placeholder five-slot list with real manifest records, timestamps from save metadata, current day, mode, checksum status, and campaign summary.
- Use a confirmation flow for delete, iron-man activation, legacy import, and load-over-current-state.
- Generate a bunker-diorama thumbnail through the Godot host only. In headless mode, use a deterministic placeholder or no thumbnail.
- Disable load controls for a terminal iron-man campaign while keeping the chronicle export available.
- Ensure the panel is keyboard/controller navigable and does not expose a fake successful action before the host returns success.

### Data, tests, and acceptance

Add a save manifest schema only if it is not already covered by the current save wire catalog. Document all section names and migration versions. Add:

- fresh slot creation and listing;
- two slots with divergent day and inventory state;
- save, switch, load, and verify isolation;
- clean aggregate round-trip;
- one mutated section causing section and aggregate checksum failure;
- missing checksum rejection for new-format envelopes;
- interrupted replacement recovery;
- legacy single-file import;
- future-version rejection and prior-version migration;
- iron-man terminal-state enforcement;
- thumbnail absence, oversized thumbnail, and headless fallback;
- chronicle derivation from the same state after reload.

Done when a new profile can create at least two independent campaigns, each can be saved and loaded through its own root, all sections validate before restore, and a corrupt active envelope cannot replace the last valid campaign.

Main attack surface: a UI that appears to switch slots while fixed user:// stores continue writing global state.

## [49] Lead-Brick Radiation Bulkhead Armor and Structural Upgrades

### Authority and scope

Extend MaterialShieldingSystem and connect it to SilentFoundrySystem, inventory, room construction, and RadiationSystem. Do not add a second attenuation formula or make the panel directly assign a material.

The existing shielding authority currently stores a ceiling material per room, exposes attenuation, and calculates a weakest-ceiling fallback. It lacks a material transaction, build job, local wall surfaces, and structural integrity. The first slice should add only the smallest model needed for room upgrades:

- a validated room shielding target;
- material and quantity requirements;
- construction progress and worker assignment;
- completed shielding material and attenuation;
- optional damage or maintenance state if environmental disasters need it.

### Core work

- Add a ShieldingUpgradeDefinition catalog record with a canonical room or blueprint ID, material ID, quantity, work cost, minimum foundry capability, and attenuation contribution.
- Add a ShieldingUpgradeJob state with job ID, room ID, target tier, inputs reserved or consumed, progress, assigned workers, and cancellation behavior.
- Make construction consume lead, scrap steel, firebricks, or graphite concrete only through the authoritative inventory/foundry ports.
- Keep the existing WallMaterial mapping compatible. Migrate old material values into explicit tier records.
- Define attenuation using a documented physical gameplay formula. Preserve a non-zero residual path unless the game explicitly has a sealed experimental tier; never report absolute 0% penetration from an ordinary lead upgrade.
- Apply room-local shielding when a room is known; use weakest-ceiling fallback only for systems without a room context. Record which path was used for diagnostics.
- Ensure the calculation composes with exterior radiation, room air filtration, occupant exposure, and protective gear rather than replacing them.
- Capture state using sorted room/job records. Do not hash dictionary iteration order.

### Godot work

- Add room upgrade cards to ShelterOperationsPanel or the existing shelter operations scene after confirming the actual active panel path.
- Show current tier, target tier, materials available, work progress, completion day, and expected attenuation delta.
- Explain residual exposure and interacting protection layers in a tooltip; avoid a misleading 0% promise.
- Disable the upgrade command when the room, material, foundry capability, or inventory transaction is invalid.
- Display worker assignment and repair state as projections from Core events.

### Tests and acceptance

- deterministic attenuation table for each supported material and tier;
- insufficient material leaves inventory and job unchanged;
- cancellation returns reserved inputs according to the documented policy;
- room-specific exposure differs from an unrelated room;
- roomless legacy exposure uses the weakest fallback deterministically;
- sorted capture produces the same checksum regardless of insertion order;
- foundry output can feed a completed upgrade;
- save/reload preserves in-progress construction and completed shielding;
- an extreme fallout storm lowers exposure but does not make the entire colony invulnerable.

Done when a player can produce the required material, assign a valid construction job, advance time, and see a saved room-local radiation result that matches the Core calculation.

Main attack surface: promising 0% radiation penetration or allowing the UI to grant a material tier without consuming resources.

## [51] Environmental Disasters: Seismic Tremors and Acid Smog Inversions

### Authority and scope

Add a bounded EnvironmentalCrisisSystem in Core that interprets WeatherSystem and Year of Ash conditions and coordinates shelter hazards. Weather remains the source of environmental conditions; the crisis system owns incident lifecycle and damage application.

Initial incidents:

- seismic tremor with a typed infrastructure target such as generator pipe, water pipe, or tunnel support;
- smog inversion that reduces ventilation intake quality and raises air toxicity;
- acid hail that degrades exposed sensors, solar surfaces, or surface equipment.

Do not implement per-frame damage or random UI alarms. Incidents operate on simulation ticks and deterministic scheduled effects.

### Core work

- Define CrisisDefinition and ActiveCrisisState with canonical IDs, start/end day, severity, target, tick schedule, damage already applied, repair work, and resolution.
- Add deterministic eligibility and roll streams keyed by day, weather state, shelter state, and crisis ID.
- Add typed impact events consumed by power, water, air, sensor, and renewable systems.
- Add RepairTask state with required skill, materials, work, urgency, and failure consequence.
- Apply each scheduled consequence at most once using a recorded tick index or event key.
- Add shelter lockdown or evacuation flags only where an existing system can consume them; avoid a second global alert authority.
- Include active incidents and repair queues in save state.

### Godot work

- Add a HUD alarm overlay with incident title, severity, affected subsystem, time to next consequence, and available repair actions.
- Dispatch engineers through the existing duty/work assignment path.
- Show unresolved hazards in shelter and power panels without duplicating state.
- Provide restrained failure feedback: a cracked pipe, rising pressure, blocked intake, or damaged sensor rather than a generic explosion.

### Tests and acceptance

- same seed and same weather produce the same incident schedule;
- one crisis tick cannot apply duplicate damage after duplicate host calls;
- invalid repair lacks resources and changes nothing;
- completing repair stops future damage and emits one resolution event;
- smog changes air/ventilation state but does not mutate the weather catalog;
- crisis state survives save/load at each phase;
- simultaneous crisis ordering is explicitly defined and tested;
- a headless Godot probe can surface and resolve an incident.

Done when a tremor can damage a real subsystem, a repair task can prevent the next consequence, and the result is deterministic and saved.

Main attack surface: a decorative alarm overlay with no authoritative damage, or repeated damage caused by replaying a tick.

## [58] Bunker Sanitation, Waste Biogas Digesters, and Compost

### Authority and scope

Add a Core SanitationSystem that owns waste streams, processing capacity, contamination, hygiene consequences, digester output, and compost eligibility. It must integrate with NeedsSystem, DiseaseSystem, ShelterHazardLoop, GreenhouseSystem, MedicalSystem, Kitchen production, and PowerGrid through ports or events.

The older Unity WasteSystem and CompostSystem are references only. Do not port them into Assets/_Game/ or let them become a second authority.

### Core work

- Define waste categories such as organic, medical, contaminated, and inert using canonical data IDs.
- Produce waste from actual food, medical, hygiene, and manufacturing actions through explicit events or ledger inputs.
- Add sanitation jobs with worker, equipment, capacity, cycle duration, and failure state.
- Add digester state with feedstock, pressure, gas output, leakage risk, and maintenance.
- Add compost state with maturity, contamination flag, nutrient value, and eligible greenhouse destinations.
- Connect hygiene and disease risk to unresolved waste through a bounded tick effect. Do not cause disease every frame.
- Make biogas an inventory or power-fuel output with a conversion rate defined in data. It cannot appear without feedstock and processing time.
- Prevent contaminated waste or rad-tainted material from silently becoming safe fertilizer or clean fuel.
- Save queues, tanks, contamination, and maintenance state in sorted records.

### Godot work

- Add sanitation meters, waste backlog, digester pressure, gas reserve, compost maturity, and assigned duty.
- Show warnings for medical waste, overpressure, contamination, and disease risk.
- Make power and sanitation controls use the same breaker and consumption projection as the power grid.
- Show which outputs are usable, quarantined, or awaiting testing.

### Tests and acceptance

- food and medical actions create the expected waste category;
- digester output is proportional to valid feedstock and available capacity;
- overpressure creates a typed hazard once, not repeated free damage;
- contaminated input cannot produce clean compost without a defined decontamination step;
- sanitation workers and power loss alter processing progress;
- disease risk changes only at documented tick boundaries;
- save/reload preserves tank levels, queues, and contamination;
- 10 units of biogas is achievable only with the required mass/time inputs.

Done when the bunker can route waste through a visible, resource-bounded sanitation cycle and sanitation status changes real disease and fuel outcomes.

Main attack surface: a free biogas button and a compost output that bypasses contamination.

## [60] Surface Solar Arrays and Wind Turbine Power Generation

### Authority and scope

Introduce renewable generation through the active power-grid architecture. First audit the current PowerGridSystem or equivalent host wiring. The legacy Unity WindTurbineSystem is not an implementation target.

Use a RenewablePowerSystem or a PowerGrid extension only if no Core authority currently owns generators and loads. Renewable assets should be real power producers with weather-dependent output, condition, maintenance, and breaker behavior.

### Core work

- Define renewable asset records with asset ID, type, rated output, installation location, condition, maintenance, enabled state, and weather response.
- Calculate output at simulation ticks from deterministic weather conditions, surface exposure, condition, dust or ash coverage, and safety lock state.
- Connect output to the existing power balance. Generated power cannot bypass total load, batteries, or brownout priority.
- Add maintenance tasks for ash removal, blade inspection, brake release, and electrical repair.
- Add failure and storm lockout events with explicit repair consequences.
- Capture asset state in sorted records and migrate older generator-only power state without changing existing fuel semantics.

### Godot work

- Add solar and wind telemetry to PowerGridPanel: current output, rated output, weather factor, condition, maintenance, and lockout.
- Provide a maintenance dispatch command through the existing duty system.
- Show renewable contribution beside diesel output and battery reserve.
- Make power allocation updates visible when a turbine starts, stops, or is damaged.

### Tests and acceptance

- clear weather, ash storm, and high-wind output values are deterministic;
- condition and dust reduce output without deleting fuel directly;
- safety lockout prevents generation until a valid condition is restored;
- total generated and consumed power reconcile with the existing grid;
- saving during a storm and reloading preserves output and maintenance state;
- installing two valid turbines produces the configured output only when weather and condition permit;
- removing a renewable asset requires an explicit authorized operation and returns only documented materials.

Done when renewables reduce generator demand through the real power balance and can fail or require maintenance under surface conditions.

Main attack surface: granting a flat 15 kW or 50% diesel reduction independent of weather, condition, load, and power routing.

## [50] Interactive Moonshine Still, Alcohol Brewing, and Bio-Solvents

### Authority and scope

Extend CraftingSystem and RecipeCatalog with distillation-specific batch state. Use ChemicalDependencySystem only when a survivor consumes an alcoholic beverage or otherwise enters a dependency-relevant use. Antiseptic, industrial solvent, and fuel-additive outputs must be distinct item IDs and must not trigger alcoholism.

### Core work

- Define mash, fermentation, distillation, proof, purity, yield, and contamination fields.
- Use recipe data for grain or potato inputs, water, heat, vessel capacity, cycle duration, and output.
- Model still condition and operator skill as bounded modifiers to purity, yield, incident chance, or cycle time.
- Separate beverage alcohol, medical alcohol, and industrial ethanol in GoodsCatalog and recipe outputs.
- Route heat/fuel through the active power or fuel consumer path.
- Add an incident outcome for contamination, pressure failure, or spoiled mash with deterministic resolution and documented resource loss.
- On actual beverage consumption, call ChemicalDependencySystem and apply the existing morale or dependency events. Do not apply dependency to clinic allocation or fuel.
- Add saveable batch state, output lot identity, purity, and contamination.

### Godot work

- Build DistilleryPanel around vats, heat, proof, purity, cycle progress, operator, and risk.
- Show an explicit use destination for each resulting item: trade, clinic, fuel, or morale consumption.
- Block impossible recipes and show missing resource reasons.
- Present safety failures as operational consequences rather than a random mini-game unrelated to Core.

### Tests and acceptance

- valid inputs produce a deterministic batch and expected item lots;
- insufficient water, grain, vessel, power, or time leaves inputs unchanged;
- purity affects only documented output or use effects;
- medical alcohol does not create dependency;
- beverage consumption does create the existing dependency event;
- contaminated batch cannot be silently relabeled as sterile antiseptic;
- save/reload resumes a fermentation or distillation batch;
- output quantity is never granted without the corresponding consumed inputs.

Done when a player can ferment and distill a batch, see its quality and destination, and use the output through existing medical, trade, fuel, or survivor-consumption paths.

Main attack surface: one generic alcohol item that grants clinic sterilization, fuel, trade value, and morale without usage-specific accounting.

## [55] Bunker Armory, Custom Weapon Modifications, and Handloaded Ammo

### Authority and scope

Create a bounded WeaponCustomizationSystem that operates on WeaponInstanceState and feeds BallisticsSystem and WeaponConditionSystem. Do not copy condition, jam, or damage formulas into an armory UI.

Before adding data, inspect combat_catalog.json and current static IDs. Attachment IDs, ammo IDs, and modifiers must resolve through canonical catalog entries.

### Core work

- Define attachment definitions with slot, compatible weapon tags, resource cost, mass, condition interaction, and ballistic modifiers.
- Define per-instance attachment records or a canonical ordered attachment ID list. Static weapon definitions remain immutable.
- Define ammo lot state for handloaded ammunition: ammo ID, quantity, quality, contamination or proof flag, and deterministic lot identity that does not use Guid.NewGuid.
- Pass the final weapon and ammo modifiers into BallisticContext fields already consumed by BallisticsSystem.
- Let WeaponConditionSystem determine degradation and jam risk. Attachment quality may alter inputs to its existing formulas, not replace them.
- Use CraftingSystem or a dedicated armory transaction for brass, powder, lead, primers, and tools.
- Add compatibility validation, attachment removal costs, reload failures, and condition loss as explicit outcomes.
- Capture attachments and ammo lots in stable wire fields and migrate unmodified legacy weapons as empty attachment lists.

### Godot work

- Build ArmoryBenchPanel with a weapon instance card, attachment slots, compatibility reasons, expected ballistic changes, condition effects, and reload recipe.
- Display the same final accuracy, damage, range, critical, jam, and ammo values used by the resolver.
- Preview changes before consuming resources; commit only through a Core command.
- Show handload quality and risk without promising a fixed 35% benefit unless the data actually defines it.

### Tests and acceptance

- incompatible attachment is rejected without consuming parts;
- valid attachment changes only the selected weapon instance;
- two identical weapons remain independent after one is modified;
- BallisticContext receives the expected modifiers;
- condition and jam behavior remains owned by WeaponConditionSystem;
- handloaded ammo consumes brass and powder and creates the correct lot;
- save/reload preserves attachment order, ammo lots, and condition;
- same seed and same weapon state produce the same jam and ballistic outcome;
- catalog validation rejects unknown attachment or ammo IDs.

Done when a modified weapon can be saved, used in combat, and resolved by the same deterministic combat pipeline as an unmodified weapon.

Main attack surface: changing a static weapon catalog entry and accidentally upgrading every rifle in the campaign.

## [56] Underground Mycology Farm and Bio-Luminescent Mushrooms

### Authority and scope

Extend GreenhouseSystem if its plot model can express fungal substrate and humidity without making crop code ambiguous. Otherwise add a narrow MycologySystem with explicit bridges to water, power, sanitation, medical crafting, and lighting. Do not revive retired ghost IDs such as previously demoted glowing-mushroom records.

Food mushrooms, luminescent fungi, and medical mold must be distinct strains and outputs. A luminescent strain may reduce a defined lighting load only when installed in a valid corridor fixture; it does not create global free light. Medical mold is a reagent, not an automatic antibiotic or immunity grant.

### Core work

- Define fungal strain records with substrate, humidity range, temperature, contamination tolerance, growth days, output, and use category.
- Define bed state with substrate quality, moisture, humidity, contamination, inoculation day, growth stage, and installed-light link.
- Consume valid water, substrate, heat, or power through existing resource ports.
- Reuse GreenhouseSystem contamination and blight semantics where appropriate, or document the separate boundary.
- Produce food, light medium, or medical reagent as separate catalog items with separate validation.
- Add contamination and mold-spread events with bounded tick behavior.
- Capture bed state in sorted order and migrate empty beds safely.

### Godot work

- Build MycologyPanel with beds, humidity, moisture, substrate, strain, growth, contamination, and harvest destination.
- Show the power reduction from installed luminescent fixtures as a real load delta.
- Show medical mold as a reagent awaiting a valid recipe or research unlock.
- Provide quarantine and cleaning commands through sanitation or greenhouse operations.

### Tests and acceptance

- invalid strain/substrate combinations are rejected;
- food harvest enters inventory with the configured nutrition category;
- luminescent harvest changes only linked lighting loads;
- medical mold cannot directly heal a survivor or grant immunity;
- contamination propagates only through documented connected beds or events;
- water and power consumption match the bed state;
- save/reload preserves growth and contamination;
- canonical catalog validation rejects ghost IDs.

Done when a lower tunnel can produce a configured fungal output while using actual shelter resources and respecting food, lighting, and medical boundaries.

Main attack surface: treating one mushroom item as food, electricity, and medicine simultaneously.

## [52] Barter Debt Promissory Notes, Collateral, and Collection Consequences

### Authority and scope

Preserve LedgerDebtSystem semantics for existing debt contracts. Add a TradeCreditSystem or a versioned ledger extension for caravan promissory notes, depending on the audit of current EconomyHostSession and CaravanBarterLedgerPanel.

The existing system intentionally uses a principal, one rate, term, named forfeit, reads, signing, payment, renegotiation, and standing checks. Do not silently add compounding, amortization, or a second credit score to that authority.

### Core work

- Define trade-credit contracts with lender faction or caravan ID, borrower campaign ID, principal in canonical settlement units, term, rate, collateral reference, delivery schedule, and default consequence.
- Prefer integer or fixed-point settlement units. If legacy float fields must remain, normalize and round at the contract boundary.
- Make collateral real: reserve an item, production claim, route privilege, or explicit room asset through a reversible pledge record. A display-only collateral field is not acceptable.
- Define payment, partial payment, renegotiation, default, and collection event transitions. Avoid hidden daily compound interest.
- Use existing faction standing and narrative encounter pathways for collection. A default should surface a deterministic event or threat, not spawn a raid directly from the panel.
- Connect loan outputs to GoodsCatalog and MarketSystem transactions. No loan can mint goods outside the accepted settlement.
- Save active and closed contracts, collateral, lender identity, and event keys.

### Godot work

- Extend CaravanBarterLedgerPanel with credit terms, total owed, due day, collateral, risk, and consequences.
- Show the exact transaction result before signing.
- Require a confirmation step for collateral pledge and default-risk acceptance.
- Display collection events in the journal or threat channel from Core events.

### Tests and acceptance

- loan signing consumes or records only the configured transaction;
- a collateral pledge blocks conflicting use and is released on successful settlement;
- exact payment clears the contract and improves the configured standing;
- partial payment follows the documented schedule without hidden compounding;
- default creates one collection event with the correct lender and consequence;
- renegotiation respects the existing term and standing rules;
- save/reload preserves debt and collateral isolation between slots;
- malformed faction or goods IDs are rejected;
- a failed loan leaves inventory, standing, and contract state unchanged.

Done when a player can take a bounded emergency loan, use the goods, repay it, or face a saved and visible collection consequence.

Main attack surface: a debt UI that displays risk while the player receives free resources and collateral has no gameplay effect.

## [54] Muster Faction Council and Regional Assembly Chamber

### Authority and scope

Extend MusterSystem, CurrentsCatalog, existing faction standing, treaty/access state, and MusterHostSession. The current MusterSystem resolves approaches and escalation records; it should gain a deterministic vote layer rather than being replaced.

### Core work

- Define council session state: session ID, agenda ID, eligible currents, delegate strength, coalition commitments, lobbying costs, ballot state, resolution ID, and outcome.
- Resolve faction references through CurrentsCatalog and canonical faction IDs. Do not add aliases that silently map inconsistent IDs; add explicit migration data where necessary.
- Define deterministic delegate ordering and tie-breaks.
- Make lobbying an actual economy or favor transaction with a cost and a recorded action.
- Model coalition commitments as state that can be honored or broken and feed standing consequences.
- Make Open Riverways Charter a canonical resolution ID that changes the existing maritime/trade access authority.
- Connect charter outcomes to routes or Currents access records rather than only displaying a badge.
- Capture active session, closed votes, commitments, and resolution effects.

### Godot work

- Build MusterPanel around agenda, eligible factions, voting power, current coalition, costs, ballot state, and projected outcomes.
- Show why a delegate is eligible and which authority supplied its standing.
- Let the player submit a ballot or lobby request only once per valid phase.
- Display the actual access unlock after resolution.

### Tests and acceptance

- same session state and seed produce identical delegate ordering and tie result;
- invalid faction cannot vote;
- lobbying consumes the configured resource exactly once;
- a coalition changes outcome only when its commitment is present;
- repeated ballot submission is rejected or idempotent;
- Open Riverways Charter changes a real route/access query;
- failed proposal does not unlock maritime trading;
- save/reload preserves an in-progress council session;
- catalog loader errors are surfaced instead of swallowed when the new definitions are invalid.

Done when a player can influence a council through costly, deterministic actions and a passed charter changes a real downstream capability.

Main attack surface: a dramatic council screen whose vote result never changes faction standing, access, trade, or saved state.

## [53] Unique Survivor Backstory Loyalty Quests and Relic Heirlooms

### Authority and scope

Create a bounded PersonalQuestRuntime in Core that consumes canonical survivor, quest, location, expedition, journal, final-wish, and heirloom records. Do not copy or extend the large legacy Assets/_Game/Survivors/PersonalQuestSystem.cs implementation.

The current data must support the survivor and quest IDs used by the feature. Named examples in the roadmap are illustrative only; do not invent unsupported named characters.

### Core work

- Define personal quest definition records with survivor ID, prerequisites, objective types, location/item references, timeout, failure consequence, reward, and journal text keys.
- Define runtime state with quest ID, survivor ID, step index, progress facts, objective evidence, status, and reward state.
- Use event subscriptions or the existing event bus contract for expedition returns, item discoveries, deaths, locations, memorial acts, and journal discoveries.
- Validate all objectives against canonical catalogs and make evidence idempotent.
- Implement heirloom transfer as an inventory transaction or memorial bequest, not a direct stat grant.
- Model trauma/perk outcomes through existing survivor, SomaticFlashback, or trait authority. Completing a quest may reduce a defined trauma state or unlock a defined perk, but cannot magically erase unrelated conditions.
- Add a content validation report for unreachable objectives, unknown IDs, duplicate quest IDs, and rewards that cannot be granted.
- Save active/completed/failed quests and objective evidence.

### Godot work

- Add personal quest tabs to JournalPanel only after the host session exposes real quest projections.
- Add map markers sourced from objective locations, with hidden, rumored, discovered, and complete states.
- Show the survivor relationship, risk, time pressure, and reward source.
- Make reward and heirloom transfer feedback explicit and journalized.

### Tests and acceptance

- quest prerequisites gate activation;
- the same objective evidence cannot advance a quest twice;
- a valid expedition result advances only the intended survivor quest;
- an invalid location or reward ID prevents content loading;
- heirloom transfer changes inventory through a validated transaction;
- completed quest reward is granted once;
- failed or deceased-survivor behavior follows data-defined policy;
- save/reload preserves progress and does not duplicate rewards;
- no new logic is added to the legacy Unity quest tree.

Done when a supported survivor can receive, pursue, complete, and save a personal quest whose reward changes a real downstream system.

Main attack surface: copying the 4,936-line legacy quest authority or granting named-character perks without canonical data and evidence.

## [59] Bunker Schoolroom and Second-Generation Child Education

### Authority and scope

Extend GenerationalSuccessionEngine and DutyRosterSystem with an EducationSystem or education state component. The existing chapter clock, ages, retirements, mentorship, and host session remain authoritative.

The roadmap's five-year curriculum must be translated into the compressed 365-day chapter model. The plan must document whether five in-game years equals five chapters or another explicit unit; it must not imply real-time child simulation without a calendar rule.

### Core work

- Define curriculum records with track ID, duration in chapters or days, required teacher skill, room requirement, capacity, lesson outputs, and failure/absence behavior.
- Define student state with survivor/child ID, track, progress, attendance, teacher, age or life-stage eligibility, and graduation status.
- Reuse existing mentorship and skill/perk authority for outputs.
- Keep one age and chapter clock. Education listens to chapter advancement rather than adding an independent age tick.
- Define attendance effects from illness, work, danger, and shelter crises.
- Make graduation a state transition with validated trait/perk application, not an immediate UI action.
- Save student state and migration data for existing dwellers without education records.

### Godot work

- Build SchoolroomPanel with eligible students, teachers, curriculum, capacity, attendance, progress, and projected result.
- Show the compressed calendar explicitly.
- Prevent assigning ineligible or over-capacity students.
- Present graduation as a logged milestone that the lineage tree can read.

### Tests and acceptance

- only eligible students can enroll;
- teacher and classroom requirements are enforced;
- progress advances on the documented calendar;
- illness or absence affects progress deterministically;
- graduation applies a defined skill/perk exactly once;
- aging and education remain consistent after save/load;
- no second child roster or age clock is created;
- five-year curriculum completion cannot occur in one day unless data explicitly defines that duration.

Done when a student can enroll, progress across the existing generational clock, graduate, and carry a saved education result into succession.

Main attack surface: an instant trait-granting classroom that bypasses age, time, teacher skill, and lineage state.

## [61] Internal Security Surveillance Cameras and Monitor Console

### Authority and scope

Add a SecurityMonitoringSystem that observes only configured camera coverage and produces evidence with confidence. It must integrate with airlock/security, ration conflict, CaregivingSystem, SurvivorNeedsState, DutyRosterSystem, inventory, power, and privacy rules.

Surveillance must not become omniscient. The system needs camera condition, power, coverage, blind spots, staffing or review delay, and evidence quality.

### Core work

- Define camera records with ID, room, coverage tags, power load, condition, storage capacity, and privacy classification.
- Define monitored event records with source camera, event type, day/tick, evidence confidence, reviewed state, and subject IDs when known.
- Generate evidence from actual inventory changes, room presence, ration events, sabotage, or duty events only when the camera covers the relevant location.
- Add blind-spot and signal-loss behavior for power brownouts, damage, smoke, and storage overflow.
- Connect reviewed evidence to narrative/conflict events; security cannot directly punish or alter morale.
- Add audit and consent policy for living quarters, infirmary, and child areas. Sensitive rooms may require a doctrine or explicit policy.
- Save cameras, evidence, storage, and review state.

### Godot work

- Build a CRT monitor panel with camera selector, signal state, timestamps, coverage, evidence confidence, and review controls.
- Show a suspicious activity record as an allegation requiring counseling, inspection, or an existing disciplinary event.
- Allow camera installation, repair, power priority, and removal through real transactions.
- Never present a generated event as certain when evidence confidence is low.

### Tests and acceptance

- camera cannot see outside its coverage;
- power loss, damage, smoke, and storage limits affect evidence;
- a pantry theft event can be detected only with a functioning pantry camera;
- duplicate event review does not duplicate a counseling or disciplinary consequence;
- low-confidence evidence offers uncertainty in the UI and event path;
- privacy policy blocks or records prohibited coverage;
- save/reload preserves reviewed evidence;
- monitoring load participates in the power grid.

Done when a starving survivor's pantry theft can be detected through a valid camera path and resolved through an existing social or disciplinary event without omniscient knowledge.

Main attack surface: using cameras as a global cheat that reads every inventory and survivor action regardless of coverage or power.

## [62] Wasteland Nomad Rumors, Cartography Leads, and Hidden Caches

### Authority and scope

Add a RumorLeadSystem that connects GoodsCatalog, MarketSystem, caravan sessions, LocationLayoutSystem, WastelandCartographyCatalog, ExpeditionSystem, and JournalSystem. Rumors are leads with provenance and confidence, not guaranteed loot spawners.

### Core work

- Define rumor records with rumor ID, source caravan/faction, price, location or region reference, confidence, expiry, discovery state, and reward generation policy.
- Validate the referenced location or create a data-defined secret location in the canonical location catalog. Do not create ad hoc map nodes from UI text.
- Make purchase/barter a MarketSystem transaction and record source, price, and day.
- Project hidden, approximate, discovered, false, expired, and resolved marker states to the map.
- Resolve the cache through the existing expedition/scavenge outcome pipeline, with deterministic reward rolls and risk.
- Support false or stale rumors as legitimate outcomes; do not promise high-tier rewards for every purchase.
- Save purchased leads, evidence, map reveal, and resolved status.

### Godot work

- Add rumor items to CaravanBarterLedgerPanel with source, cost, confidence, and expiry.
- Show uncertain map rings or approximate sectors before discovery.
- Allow expedition planning from a revealed or approximate lead through MapAtlasPanel.
- Journal the lead and its outcome.

### Tests and acceptance

- buying a lead consumes the negotiated goods exactly once;
- duplicate purchase is prevented unless data allows multiple copies;
- unknown locations fail catalog validation;
- confidence affects marker precision or outcome policy, not arbitrary UI color only;
- a false or expired rumor resolves without granting the hidden cache;
- valid cache rewards flow through expedition/scavenge inventory;
- save/reload preserves lead and map state;
- same seed produces the same cache outcome after the same lead purchase.

Done when a trader can sell a traceable, uncertain lead that becomes a real map/expedition objective and resolves through the existing loot pipeline.

Main attack surface: guaranteed high-tier loot from a UI purchase or a map node that bypasses canonical location and expedition state.

## [63] Funeral Rites, Mourning Vigils, and Crematorium Recovery

### Authority and scope

Extend the existing FinalWishSystem, VigilStateMachine, death/memorial event path, grave epitaph catalog, MedicalHostSession, and sanitation output. Do not create a second death system or distribute multiple overlapping morale buffs for the same death.

### Core work

- Define FuneralRiteDefinition records for Formal Burial, Common Vigil, and Cremation with requirements, duration, staff, water/fuel/material costs, grief effects, sanitation effects, and memorial output.
- Create MemorialRiteState keyed to the actual deceased survivor and death event. It must be idempotent.
- Consume FinalWish completion or failure state where relevant. A rite can fulfill an existing wish step but cannot fabricate a wish.
- Use VigilStateMachine for the ceremony state if its lifecycle fits; otherwise add an adapter rather than a second ceremony clock.
- Generate epitaph content through wasteland_grave_epitaphs.json and record it in the memorial ledger.
- Treat cremation ash as a typed sanitation or agricultural byproduct with contamination handling. It cannot become fertilizer without the configured safety process.
- Apply one documented grief/morale consequence from the memorial event, with kin and shelter scope defined in data.
- Save rite progress, completion, ledger entry, ash output, and event keys.

### Godot work

- Add a rite selector to MemorialLedgerPanel with requirements, costs, grief effect, time, and sanitation consequences.
- Display cause of death and heirloom disposition from authoritative records.
- Present a restrained memorial sequence and allow the player to defer only when the system permits it.
- Show the resulting epitaph and ledger entry after completion.

### Tests and acceptance

- only a real deceased survivor can enter a rite;
- a rite cannot complete twice;
- costs are consumed atomically;
- final-wish progress can advance only through the documented rite;
- funeral choices produce distinct configured consequences;
- ash is quarantined or processed according to contamination rules;
- one death does not receive duplicate morale effects from both vigil and memorial completion;
- save/reload resumes or completes a rite without duplication;
- epitaph output resolves through canonical data and remains fictional.

Done when a death creates one authoritative memorial path, a selected rite has real cost and consequence, and the record survives campaign reload.

Main attack surface: treating a body as a resource loot container or stacking identical morale bonuses across multiple panels.

## [57] Radio Electronic Warfare, Frequency Jamming, and Counter-Ciphers

### Authority and scope

Add ElectronicWarfareSystem in Core, connected to RadioInterceptionSystem or FactionRadioEngine, WarlordDoctrineSystem, power, research/knowledge, and journal. Existing RadioHostSession remains the presentation bridge.

Fix determinism hazards in touched radio code:

- use canonical faction order instead of raw dictionary enumeration for frequency ties;
- replace the HashCode-based fallback with a stable seed derivation;
- ensure ciphertext, jamming, and decoy results use explicit deterministic rolls.

### Core work

- Define EW module state with module ID, power load, antenna condition, active window, target frequency, mode, detection risk, and cooldown.
- Define modes for jamming, decryption, and decoy broadcast with data-defined requirements and effects.
- Model jamming as a typed reduction or delay to a WarlordDoctrine action, not a direct raid cancellation hidden in UI.
- Model counter-ciphers as research or skill-gated decoding that produces an evidence record and journal entry.
- Model decoy chatter as an input to faction intelligence/route selection only where WarlordDoctrineSystem supports it.
- Add detection, misidentification, and signal failure outcomes with deterministic evidence.
- Consume power and antenna condition through existing systems.
- Save active EW state, decoded records, cooldowns, and applied effect keys.

### Godot work

- Add spectrum analyzer, frequency controls, power draw, target faction, confidence, cipher progress, and detection risk to RadioPanel.
- Show actual current broadcasts and EW effects from Core state.
- Make jamming and decoy actions require a valid frequency, module, power, and cooldown.
- Write decoded intelligence to JournalSystem and show whether it is confirmed or inferred.

### Tests and acceptance

- frequency matching is stable regardless of channel insertion order;
- same seed and state produce identical broadcast, jamming, and cipher outcomes;
- power loss or antenna damage prevents activation;
- a successful jam delays or alters a supported warlord action exactly once;
- failed decryption does not create a false confirmed journal fact;
- decoy effects use the faction doctrine path and have detection consequences;
- save/reload preserves active windows and applied effect keys;
- no new System.Random or HashCode-based simulation fallback appears in touched files.

Done when the player can operate a powered radio module, produce deterministic intelligence or disruption, and see a saved downstream faction effect.

Main attack surface: an atmospheric spectrum screen that says a raid was delayed while WarlordDoctrineSystem remains unchanged.

## [54/52/53/59/61/62/63 integration checkpoint]

Before calling the institution and survivor features complete, run a cross-domain contract pass:

- A Muster resolution must unlock a canonical trade or maritime route that a caravan or expedition can query.
- A trade-credit default must surface through faction, encounter, or threat systems without bypassing the save envelope.
- A personal quest location must resolve on the same map authority used by ordinary expeditions and rumor leads.
- Education must alter the same skill/trait representation used by work, medicine, crafting, and succession.
- Camera evidence must refer to the same survivor and inventory IDs used by ration and social systems.
- Memorial outputs must feed the same journal, epitaph, heirloom, sanitation, and greenhouse item authorities.
- Every cross-domain event must have a stable event key for idempotent restore and replay.

## Data and file-boundary plan

### New or extended Core files

Use the following as likely seams after the Phase 0 audit. Exact file names may change if an existing authority already owns the responsibility.

- SaveProfileService.cs, SaveSlotService.cs, CampaignSaveEnvelope.cs, and SaveSlotTypes.cs near the existing save wire/checksum types.
- ShieldingUpgradeSystem.cs or an extension of Shelter/MaterialShieldingSystem.cs.
- EnvironmentalCrisisSystem.cs.
- SanitationSystem.cs.
- RenewablePowerSystem.cs or a PowerGrid extension.
- DistillerySystem.cs or a CraftingSystem extension.
- WeaponCustomizationSystem.cs.
- MycologySystem.cs or a Greenhouse extension.
- TradeCreditSystem.cs near LedgerDebtSystem and Economy.
- MusterCouncilSystem.cs or a MusterSystem extension.
- PersonalQuestRuntime.cs.
- EducationSystem.cs near Legacy/GenerationalSuccessionEngine.cs.
- SecurityMonitoringSystem.cs.
- RumorLeadSystem.cs.
- MemorialRiteSystem.cs.
- ElectronicWarfareSystem.cs.

Do not create these files automatically if an existing Core authority can be extended without ambiguity. The first implementation commit for each slice should document the chosen ownership.

### Godot host/UI seams

Likely host seams include:

- SaveLoadHostSession.cs and src/UI/SaveLoadPanel.cs.
- Shelter operations and GameHudOverlay for shielding, crises, sanitation, heating, and power.
- PowerGridPanel for renewables and consumer load.
- DistilleryPanel.cs, ArmoryBenchPanel.cs, and MycologyPanel.cs.
- CaravanBarterLedgerPanel.cs and MusterPanel.cs.
- JournalPanel.cs and MapAtlasPanel.cs.
- SchoolroomPanel.cs and survivor/lineage panels.
- AirlockSecurityPanel.cs for cameras and rites where appropriate.
- RadioHostSession.cs and RadioPanel.cs.
- MemorialLedgerPanel.cs.

Before creating a new panel, search for the active scene and host binding. A roadmap path is not proof that the file is active.

### Data changes

For every new catalog:

- use snake_case IDs;
- include schema_version;
- use fictional names and locations;
- define explicit ranges, costs, duration, resource categories, and migration behavior;
- add the ID to the canonical registry or static catalog only after the JSON definition exists;
- add CatalogIntegrityValidator coverage for references;
- avoid reactivating demoted ghost definitions;
- preserve existing user-modified JSON and review diffs before touching shared catalogs.

Likely data domains include shielding upgrades, crisis definitions, sanitation recipes, renewable assets, distillation recipes, weapon attachments/ammo lots, fungal strains, trade-credit terms, council agendas/resolutions, personal quests, curricula, camera policies, rumor leads, funeral rites, and EW modules.

## Test and verification matrix

### Core behavior tests

Add focused suites rather than one large integration fixture:

- SaveSlotServiceTests and SaveAggregateContractTests;
- MaterialShieldingUpgradeTests;
- EnvironmentalCrisisSystemTests;
- SanitationSystemTests;
- RenewablePowerSystemTests;
- DistillerySystemTests;
- WeaponCustomizationTests;
- MycologySystemTests;
- TradeCreditSystemTests;
- MusterCouncilTests;
- PersonalQuestRuntimeTests;
- EducationSystemTests;
- SecurityMonitoringTests;
- RumorLeadSystemTests;
- MemorialRiteTests;
- ElectronicWarfareSystemTests.

### Required test shapes for every new stateful system

- valid command changes expected state;
- invalid command is side-effect free;
- repeated command is idempotent or explicitly rejected;
- deterministic replay from the same seed;
- capture/restore round-trip;
- old schema migration;
- future schema rejection;
- checksum changes when meaningful state changes;
- catalog reference validation;
- event emitted exactly once for a committed transition.

### Cross-system regression tests

- foundry output to shielding construction to radiation exposure;
- weather crisis to repair task to power or water recovery;
- sanitation waste to biogas to power balance and disease risk;
- renewable output to diesel demand and brownout priority;
- distillery output to clinic use, trade, and survivor beverage consumption;
- weapon attachment to BallisticContext and WeaponConditionSystem;
- fungal harvest to food, lighting, and medical recipes;
- trade credit to goods transaction, standing, collateral, and collection event;
- Muster charter to route/trade access;
- personal quest objective to expedition and heirloom transfer;
- education graduation to skill/trait and generational succession;
- camera evidence to ration conflict resolution;
- rumor lead to map node and expedition resolution;
- death to final wish, funeral, epitaph, memorial, and sanitation;
- EW action to power, radio evidence, WarlordDoctrine, and journal.

### Canonical command verification

Run after implementation, with PASS/FAIL reported for each command:

    dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet build Ashfall.csproj
    godot --headless --path . -- --data-integrity-selftest
    godot --headless --path . -- --bridge-selftest

For affected slices, also run the relevant existing headless probes and add new probes only through the Godot host. Do not substitute Unity compilation or Unity playmode.

### Determinism audit

Before review approval, search changed files for:

- System.Random;
- Guid.NewGuid;
- HashCode.Combine or other runtime-dependent hashing;
- dictionary enumeration used in an outcome;
- filesystem enumeration used in a catalog or save order;
- wall-clock values used in simulation;
- unordered DTO collections that affect SaveChecksum.

The audit must explicitly cover MaterialShieldingSystem, WeatherSystem, FactionRadioEngine, Muster ordering, weapon quality, and save slot manifests.

## Review and repair loop for GLM 5.2 and Qwen 3.7 Plus

Use the other models as adversarial reviewers after each vertical slice. Give them only the diff, the relevant roadmap item, the architecture rules, and the acceptance tests. Ask for findings first; do not ask them to rewrite the whole feature.

### Review packet

1. Identify every new authority and prove that no existing authority was duplicated.
2. Find every UI path that can mutate simulation state without a Core command.
3. Check whether every resource output has a corresponding input transaction.
4. Check whether every coupled variable is captured, restored, migrated, and checksummed.
5. Search for non-deterministic random, ordering, identity, time, and filesystem behavior.
6. Verify that material shielding never promises impossible absolute immunity.
7. Verify that environmental incidents cannot apply duplicate damage on repeated ticks.
8. Verify that sanitation cannot create clean fuel or fertilizer from contaminated inputs for free.
9. Verify that renewables participate in power balancing and cannot bypass fuel or brownout logic.
10. Verify that alcohol dependency is applied only to relevant consumption.
11. Verify that armory changes are per weapon instance and use the existing ballistic resolver.
12. Verify that mycology outputs remain separate food, lighting, and medical resources.
13. Verify that debt collateral and collection consequences are real and saved.
14. Verify that Muster resolutions alter a canonical access or standing authority.
15. Verify that personal quests do not copy or revive the legacy Unity quest system.
16. Verify that school progression uses one age/chapter clock and cannot grant instant adulthood.
17. Verify that cameras have coverage, power, condition, blind spots, confidence, and privacy constraints.
18. Verify that rumor leads can be stale or false and never bypass map/expedition resolution.
19. Verify that funeral rites do not duplicate death, heirloom, or morale effects.
20. Verify that iron-man mode cannot be bypassed through UI, slot deletion, or a stale store path.

Repair loop:

1. Implement the smallest slice and its Core tests.
2. Run the deterministic and save probes.
3. Give the diff and spec to a different tool for review.
4. Classify findings as blocker, correctness, determinism, save compatibility, UX, or polish.
5. Repair blockers and correctness findings before adding the next slice.
6. Re-run the full canonical verification at each phase gate.

## Definition of done for Batch 4

Batch 4 is complete only when all of the following are true:

- Steps 49–64 have a single documented Core authority or an explicit extension point.
- Step 64 supports isolated profile/slot state, aggregate checksums, atomic replacement, corruption handling, legacy import, and enforced iron-man policy.
- Every feature has a Godot panel that is wired to a real host session and Core command.
- Every new stateful system has deterministic behavior, save/load, migration, and validation tests.
- CatalogIntegrityValidator reports zero errors for all new references.
- No new Unity gameplay files, Unity scenes, ScriptableObjects, JsonUtility calls, System.Random calls, or Guid.NewGuid calls were added.
- Existing user edits are preserved and no broad formatting or unrelated data rewrite is included.
- The five canonical verification commands pass.
- The different-tool review finds no unresolved blocker or correctness issue.
- A fresh campaign can exercise the infrastructure family, production family, faction/survivor family, and EW family without a slot reload changing outcomes.

## Recommended next implementation prompt

Run this as the next coding task:

    ASHFALL Batch 4 Phase 0: implement only the save foundation audit and slot-aware aggregate save contract. Do not implement Steps 49–63 yet. Read the project AGENTS.md and REPO_REVIEW_REPORT.md first. Inspect every active src/Host/*SaveStore.cs, Main save orchestration, SaveChecksum, SaveWireContract, and src/UI/SaveLoadPanel.cs. Preserve all existing user-modified files. Create a Core SaveProfile/SaveSlot service with explicit versioned section envelopes, canonical ordering, per-section and aggregate checksums, atomic replacement, corruption quarantine, legacy single-file import, and normal/iron-man policy state. Refactor only the minimum host wiring needed to route stores through a selected slot root. Replace fake SaveLoadPanel data with a real host projection, but keep thumbnails host-only and outside the simulation checksum. Add Core tests for two-slot isolation, clean round-trip, mutated checksum rejection, missing checksum rejection, interrupted replacement recovery, legacy import, future-version rejection, and iron-man terminal enforcement. Run the five canonical dotnet and godot verification commands and report PASS/FAIL for each. Do not run Unity.

This prompt keeps the first coding task reviewable and establishes the persistence boundary that every later Batch 4 feature needs.
