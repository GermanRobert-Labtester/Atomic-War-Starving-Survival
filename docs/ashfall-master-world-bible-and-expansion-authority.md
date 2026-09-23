# ASHFALL: MASTER WORLD BIBLE & EXPANSION AUTHORITY

**Prepared for:** Codex 6 Luna (ChatGPT, working from Codex) — master expansion planner and drafter.
**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` (ASHFALL: Atomic War – Starving Survival)
**Document version:** 1.0.0 — compiled 2026-09-23
**Purpose:** A single master reference that lets an LLM planner expand the game from every side at once — prose of all types and kinds, mechanics, economy, balance, save compatibility, UI, tooling, testing, performance, documentation, and functionality planning — without inventing duplicate systems, contradicting canon, or drifting from repository truth.

---

## PART 0 — HOW TO USE THIS DOCUMENT (LLM BEHAVIOR CONTRACT)

### 0.1 What this document is

This is a master world bible and expansion scaffold. It follows the layered world-bible method: project constitution, canon, world model, dynamic state, writing bible, and production schemas are kept as separate layers, and every statement carries a fact status. It is designed so that a planning model does not need to re-derive the repository's structure before drafting expansion plans.

It is deliberately large. When you (the planner) draft an expansion plan, do not quote this entire document back. Instead, build a small context packet for the task (see 0.6) and cite the sections you used.

### 0.2 Authority order

When drafting any plan, prose, or proposal, resolve conflicts in this order:

1. Live repository source and data (`Assets/Ashfall.Core/`, `Assets/StreamingAssets/Data/`, `src/`).
2. `AGENTS.md` — compact non-negotiable agent rules.
3. This master bible (canon, world model, inventories).
4. `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` and `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md`.
5. `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, `TEST_POLICY.md`.
6. Individual plan closeouts and docs under `docs/`.
7. Brainstorming material and parking-lot ideas (never authoritative).

A casual brainstorming paragraph never overrides an established fact. If this document and live source disagree, live source wins and this document must be corrected.

### 0.3 Fact statuses

Label every substantive statement in plans and drafts:

- **CANON** — confirmed, must be preserved.
- **CANON-CONDITIONAL** — true only under a branch, ending, or world state.
- **VERIFIED** — demonstrated by direct repository evidence (named files, systems, catalogs).
- **HIGH CONFIDENCE** — strong static evidence; runtime confirmation would improve certainty.
- **INFERENCE** — strongly implied but not explicitly confirmed.
- **PROPOSAL** — suggested new content awaiting approval.
- **DRAFT** — creative text that is not yet authoritative.
- **NON-CANON** — experiment, alternative, or discarded concept.
- **UNKNOWN** — a deliberate gap requiring a decision.

### 0.4 Core LLM rules for this project

- Do not invent missing canon silently. Mark gaps as UNKNOWN and propose a decision.
- Do not contradict the architecture invariants (Part 2) or hard world rules (Part 3).
- Do not create a parallel system, registry, ledger, save store, or manager when an owner already exists. Find the owner in Part 5 and extend it.
- Do not create new item, location, faction, or quest IDs without checking the existing ID inventories first.
- Cite the exact entities (files, systems, catalog IDs) used by every proposal.
- Prefer expanding existing content over creating unrelated content.
- Every plan must name its verification commands and acceptance criteria.
- A compile-green or prose-complete result is not proof of integration. Integration means Core authority, host owner, route/event path, persistence, and observable outcome all agree.

### 0.5 The central rule for expansion planning

Do not "write more lore." Modify or extend a specific part of a structured world model while preserving explicit constraints. Every plan must state:

1. Which existing system or catalog it extends.
2. Which seams it uses (host session, save store, event bus, catalog loader).
3. What changes for the player, and when.
4. What must not change.
5. How the result is verified.

### 0.6 Context packet template (use for every drafting request)

```text
# Context Packet: [task]
## Task
[One sentence outcome.]
## Relevant canon
[3-8 facts, each labeled CANON / VERIFIED / INFERENCE.]
## Current world / system state
[What is true now in the relevant systems.]
## Relevant entities
[Exact IDs: locations, items, factions, survivors, quests, systems, files.]
## Required output
[Prose field specs, plan format, schema, count, length.]
## Forbidden
[No new factions, no engine references in Core, no wall-clock randomness, etc.]
## Output sections
1. Final content / plan
2. Facts used
3. New facts introduced
4. Continuity risks
5. Verification steps
```

### 0.7 Operating procedure per content unit

1. Summarize the relevant canon from this bible.
2. List connected entities (from the inventories in Parts 5 and 14).
3. Identify gaps and contradictions.
4. Propose several connected additions.
5. Select one.
6. Draft the structured specification first (IDs, references, states).
7. Review states, dependencies, consequences.
8. Approve the structure.
9. Draft the prose fields.
10. Run the continuity audit (Part 13).
11. Record changes (Part 13.4 change log protocol).
12. Convert into an integration plan (Part 10 template).

---

## PART 1 — PROJECT CONSTITUTION

### 1.1 Project identity

- **Title:** ASHFALL: Atomic War – Starving Survival.
- **One-sentence premise:** A 2D post-nuclear survival-management game in which the player runs a shelter community of survivors through scarcity, radiation, weather, faction politics, and a slow multi-year reckoning.
- **Genre:** Survival management / colony sim with strong narrative simulation layers.
- **Engine:** Godot 4.7.1 .NET (C#). Compatibility renderer, `canvas_items` stretch, fixed 1920×1080 desktop viewport.
- **Host:** `Ashfall.csproj` (net8.0) compiles `src/**/*.cs` and `Assets/Ashfall.Core/**/*.cs` into the Godot assembly `AtomicWar`.
- **Core:** engine-agnostic `Ashfall.Core` (`netstandard2.1` compatibility), zero engine references.
- **Tests:** xUnit under `Ashfall.Core.Tests/` (net9.0); 10,000+ cases in the full suite at recent handoffs (11,098/11,098 green at the 2026-09-13 handoff; 11,697/11,697 at the D1 seal).
- **Data authority:** `Assets/StreamingAssets/Data/` — the sole authored JSON data authority.

### 1.2 Player role and fantasy

The player is the shelter's de facto steward: not a chosen hero, but an administrator of scarcity. The fantasy is competence under exhaustion — reading a weather forecast, a dosimeter, and a pantry ledger, then making a triage decision that someone will pay for. The game rewards foresight and punishes drift, but failure produces consequences and adaptation rather than a simple reset.

### 1.3 Core gameplay loop (CANON)

1. OBSERVE — weather forecast, dosimeters, vitals, radio signals, pantry stocks.
2. INTERPRET — weigh crisis urgency: freezing versus starvation versus raider toll.
3. PRIORITIZE — allocate power, water, shift rosters, medical beds.
4. COMMIT — dispatch an expedition, tap the foundry, enforce strict rationing.
5. PAY COST — burn diesel, consume filters, accumulate survivor fatigue and guilt.
6. RECEIVE CONSEQUENCE — immediate (loot, casualty) and delayed (sickness, strike, raid).
7. ADAPT — rebalance bunks, mediate disputes, treat acute radiation syndrome.
8. Return to 1.

### 1.4 Narrative pillars (CANON)

- Scarcity is the antagonist. Every system charges a cost.
- Choices change relationships, access, and the epilogue matrix.
- The world acts autonomously: faction wars, weather, and wildlife proceed without the player.
- Failure produces consequences, not resets.
- Bureaucracy is texture: ledgers, manifests, permits, verdicts, and schedules are the voice of the world.

### 1.5 Tone (CANON)

Cold, exhausted, human, restrained, material, and bureaucratic. Utilitarian, slightly analog, devoid of gloss. A world of scarcity. Fictional: no real countries, wars, people, copied art, copied text, or copied UI layouts.

### 1.6 Prohibited directions (CANON)

- No engine references inside Core; Godot is authoritative, Unity is retired.
- No unexplained ancient superweapons or prophecy-driven storylines.
- No new faction, location, or system that is not connected to existing conflicts and seams.
- No character reversal without psychological preparation (the psychological arc systems exist; use them).
- No quest that exists only to deliver exposition.
- No wall-clock time, `System.Random`, `Guid.NewGuid()`, or hash-iteration-order dependence in simulation logic.
- No duplicate mutable gameplay authority in panels, hosts, caches, or parallel systems.

### 1.7 The four architectural tiers (VERIFIED)

1. **Tier 1 — Data Authority:** `Assets/StreamingAssets/Data/*.json` — snake_case catalogs, every file carries `schema_version`.
2. **Tier 2 — Core Simulation:** `Assets/Ashfall.Core/` — plain C# systems with `CaptureState()` / `RestoreState()`.
3. **Tier 3 — Host Presentation:** `src/Host/` (host sessions and checksummed save stores) and `src/UI/` (panels).
4. **Tier 4 — Verification:** `Ashfall.Core.Tests/` (xUnit) plus the headless CLI selftest surface in `src/Host/HostCli.cs`.

---

## PART 2 — ARCHITECTURE INVARIANTS (NON-NEGOTIABLE)

Every expansion plan must be checked against all six invariants. A plan that violates one is invalid regardless of its creative merit.

### Invariant 1 — Zero engine coupling in Core
`Assets/Ashfall.Core/` has zero Godot/Unity references. New gameplay mechanics are pure C# classes operating on primitives, domain DTOs, and ports. If a design needs a Vector2 or a node, the design is wrong for Core; the host adapter owns presentation.

### Invariant 2 — Ports and adapters
Shared ports live in `Ports.cs` / `Ports/` (`IJsonSerializer`, `IFileIO`, `ISeededRng`, `ILog`, `IWallClock`, `IWeatherSeverityProvider`). Host requirements are abstracted behind these. Adapters (`GodotFileIO`, `GodotLog`) live in `src/Host/`.

### Invariant 3 — Cross-host save compatibility
State is serialized through DTOs via `SystemTextJsonSerializer` into checksummed save envelopes with explicit version migration. Malformed current envelopes are rejected. Writes go through the shared atomic writer with per-store SHA-256 records (`SaveLoadHostSession`, `SaveChecksum`, `SaveEnvelopeDetection`).

### Invariant 4 — Strict determinism
All pseudo-randomness uses `ISeededRng` (seeded sub-streams; `StableHash`-derived sub-streams for subsystems such as signal authenticity). No `System.Random`, no `Guid.NewGuid()`, no `DateTime.UtcNow` in simulation (a CvdDiamondPanel determinism violation was sealed by replacing `DateTime.UtcNow` batch ids with an engine-minted `NextBatchId()`). Seeding from wall-clock or hash iteration order is forbidden.

### Invariant 5 — Zero gameplay logic in presentation
Panels expose existing commands and truthful current state. Gameplay decisions never live in `src/UI/`. Core events expose facts; host adapters apply presentation and persistence effects.

### Invariant 6 — JSON data authority
`Assets/StreamingAssets/Data/` is the sole source for authored content. Presence in JSON is not gameplay reachability — catalogs are validated by `CatalogIntegrityValidator` and the data-integrity selftest, and utilization is checked by `--content-utilization-selftest`.

### The Setup–Save–Flush triad
`src/Main.cs` (plus its partials `Main.Lifecycle.cs`, `Main.SaveOrchestrator.cs`, and domain partials such as `Main.Narrative.cs`, `Main.Expeditions.cs`, `Main.Economy.cs`, `Main.Medical.cs`, `Main.YearOfAsh.cs`) manages Setup/Save/Flush method families. Any new stateful system must implement the matching triad (`SetupXxx`, `SaveXxx`, `FlushXxxIfDirty`) and register through the current save-section owner. The triad drift gate (`scripts/ci/triad-drift-gate.sh`, `MainTriadDriftGateTests`) enforces parity. Subsystem manifest bootstrap (`Orchestration/SubsystemManifest.cs`, `RegisterSetupAction`, `ExecuteSetup`) is the sanctioned path for new subsystem registration.

### Namespace and ID conventions (CANON)

- Gameplay systems: `Ashfall.Core.<Domain>` (e.g., `Ashfall.Core.Inventory`, `Ashfall.Core.Economy`).
- Godot host: `AtomicWar.GodotApp.*` (`UI` panels, `Host` sessions, `Journal`, `World`, `YearOfAsh` widgets).
- IDs: `snake_case` everywhere — `item_clean_water`, `recipe_iodine`, `loc_broadcast_bunker_echo`, `skill_signal_ear`.
- State changes flow through the event bus (`IEventBus`); state is captured/restored through `CaptureState`/`RestoreState`.

### Multi-agent work discipline (CANON for planners)

The repository runs a foreman/builder/sweep/integrator workflow. Before proposing any plan that touches code:

- Read `INTEGRATION_PLANS.md` (current batch and acceptance) and `WORKTREE_OWNERSHIP.md` (path claims). Claimed paths are read-only to everyone except their owner.
- `TEST_POLICY.md` governs test selection: focused runs below 100 cases via `scripts/run_test.sh`; full-suite runs require an explicit reason.
- `KNOWN_DEBT.md` tracks accepted, blocked, quarantined, and retired work. Never re-open retired debt without new evidence.
- Decision-blocked items (examples at the 2026-09-19 audit: semantic-kind re-grouping D11, quarantine drain D21, XP-04 economy legs F13, XP-06 body-integrity schema F14, EN-01..EN-08 proposals, Plan 49, C3 HOLDs 174/175/192/199, string freeze D22) require the named signature before any drafting.

---

## PART 3 — CANON: THE WORLD OF ASHFALL

### 3.1 Setting (CANON)

An atomic war has ended the old world. The player's community survives in and around a shelter complex in a contaminated wasteland region, through a nuclear-winter climate cycle (Year of Ash), seasonal weather, radioactive fallout patterns, ecological mutation, and the slow collapse or rebuilding of human institutions. The long arc runs through a Machine Reckoning at Day 360 and an epilogue matrix evaluated out to Day 3650 (ten-year generational horizon).

The war itself is fictional and unnamed. Locations, factions, and survivors carry evocative but invented names (District 8 Deep Coast, Denial Cut, Verity Motel, the Silent Foundry, the Iron Cenotaph, the Cobalt Synod). No real-world nations, leaders, or conflicts appear.

### 3.2 Time structure (CANON, from the temporal atlas)

- **Playable calendar:** 360 sim days primary arc (Year of Ash tick window 180–360); faction war-chain timeline maps playable day 180 to authored day 480 via a 300-day offset (`FactionWarChainRunner.ToAuthoredDay`).
- **Real-time/frame:** UI rendering, audio events, radio frequency SNR dial.
- **Hourly:** work shift ticks, pharma distillation phases, dive oxygen drain, foundry pour windows.
- **Daily tick:** weather check, needs decay, sickness progression, ration consumption, save flush.
- **Multi-day (3–14):** expedition transit, crop growth, warlord tribute cycles (7 days), debt interest, ARS latent-to-manifest phases.
- **Seasonal (30–90):** nuclear winter to thaw transitions, second winter roster locks, canal freeze.
- **Endgame:** Day 360 Machine Reckoning; epilogue matrix (32-permutation whole-saga resolution); generational succession out to Day 3650.

### 3.3 Hard world rules (narrative invariants)

- No character may know an event before they experience it or learn of it. Information travels through the modeled channels: radio intercepts, journals, couriers, rumors, witnesses (`InformationFlow/`, `CAMPAIGN_INFORMATION_FLOW.md`).
- A destroyed or degraded location cannot appear intact unless the scene is explicitly historical or restored through the location-evolution systems (`LocationEvolutionSystem`, `LandmarkDegradationSystem`).
- A faction cannot react to a player action before information about it reaches that faction (`FactionStanceEngine`, faction radio, communiqués).
- Dead survivors appear only in memories, recordings, memorial rites, and epilogues (`Memorial/`, `Spiritual/`, `SurvivorFate`).
- Genuine distress signals are never hostile (hard invariant of `SignalAuthenticityEvaluator`); trap-class signals exist but are authored as such (e.g., the hijacked evacuation band).
- The main ending cannot be invalidated by optional side content; expansion content must specify its position relative to each relevant ending.

### 3.4 Major canon domains (each with an owning system — see Part 5)

- **Radiation and the body:** dose ledger, ARS pathology, dosimeters, radon migration, fallout patterns, dose quests.
- **Scarcity economics:** rationing, commodity baselines, regional prices, market shocks, debt ledgers, tribute, black market, caravans.
- **Faction struggle:** warlords with doctrines, faction war chains, regional treaties, embargoes, espionage, propaganda, musters, verdicts.
- **Industry from scraps:** foundry, CVD diamond synthesis, SOFC power, cryogenics, chlor-alkali, Fischer-Tropsch, powder metallurgy, precision optics — an "authentic low-tech chemistry" identity that the narrative corpus mirrors (assay logs, titration reports, kiln records).
- **The human interior:** psychological trauma, therapies, guilt sources, mental health crises, confessions, belief movements, spiritual rituals, funerary rites.
- **The autonomous world:** wildlife migration, ecological infestations, weather hardening, world evolution events and seeds, orbital harrow telemetry, the sky-defense layer.
- **Bureaucratic afterlife:** wasteland laws, registries, censuses, standing records, verdict data, treaties, permits, load-shed schedules, wiretap transcripts, court verdicts.

### 3.5 Terminology and naming (CANON)

- Naming style: bureaucratic-poetic compounds ("Denial Cut", "Load Shed Schedule 001", "Iron Synod Canons", "Year of Ash"). Invent names freely in this register; never use real-world names.
- ID prefixes observed in data: `item_`, `loc_`, `recipe_`, `skill_`, `quest_` (via catalogs), `loc_*` location stubs, faction ids in `factions.json`/`crossing_factions.json`/`holdfast_factions.json`.
- New IDs must not collide with existing catalog IDs; run the data-integrity selftest after authoring.

### 3.6 Timeline event record format

When drafting historical content, use this shape (adapted to a JSON catalog entry):

```text
id: EVENT_[SNAKE_CASE]
date: "[fictional calendar position]"
status: CANON
summary: [one sentence]
public_account: [what the wasteland believes]
hidden_account: [what evidence reveals]
participants: [faction/survivor/location IDs]
locations: [loc_* IDs]
consequences: [state changes that persist]
clues: [item/IDs, documents, dialogue that reveal it]
player_relevance: [quest unlocks, epilogue flags]
```

---

## PART 4 — WORLD MODEL: REGIONS, FACTIONS, ECONOMY, ECOLOGY

### 4.1 Geography and movement (VERIFIED systems)

The wasteland map (`wasteland_map_v1.json`, `WastelandMapSystem`, `WastelandMapCatalogLoader` with the `AllMapNodes_ExistInLocationsCatalog` orphan gate) is a node graph. Expeditions and caravans traverse it with distance, fog-of-war (Unknown nodes), and route gates (`WeatherRouteGateCatalog` — weather-gated crossings with `force_stamina_cost` force-passage; ice-road and deep-coast blocks are absolute). 261+ locations exist across `locations.json`, `locations_expansion3.json`, `crossing_locations.json`, `dose_locations.json`, `holdfast_locations.json`, and destination-bound micro-locations. Expeditions are dispatched against a 53-destination catalog merged to a 263-id dispatchable surface.

### 4.2 Factions (VERIFIED inventory)

Faction identity spans: `factions.json` (core), `faction_lore.json`, `faction_territory.json`, `faction_intelligence.json`, branch catalogs (`independent_faction_branch.json`, `military_faction_branch.json`, `rebel_faction_branch.json`), the faction-war family (`faction_war_communiques/dialogue/events/journal/radio/location_overrides.json`), warlord doctrines, holdfast factions, crossing factions, muster faction culture and actions, foundry accords, regional treaties, trade embargoes. Faction behavior runs through `FactionStanceEngine`, `WarlordDoctrineSystem`, `FactionWarSystem`/`FactionWarChainRunner`, `Diplomacy/`, `EspionageHostSession`, `PsyOpsHostSession`, and `CounterIntelligenceHostSession`.

Faction record fields when drafting a new faction or faction content: public goal, operational goal, hidden goal, long-term goal; methods (willing/unwilling); resources; weaknesses; members; relationships (value to other factions); controlled and contested locations; quest roles (gives/obstructs); dialogue rules (public language, private language, forbidden claims).

### 4.3 Economy (VERIFIED systems)

`Economy/` (market, price factors, shocks), `commodity_baselines.json`, `regional_prices.json`, `hardcore_economy_tuning.json`, `EconomyMarketRumorRules` (deterministic rumor bands, `MarketRumor` kind 6), `BlackMarketHostSession`, `merchant_caravans.json` + restock day-gating, `caravan_trade_routes.json`, `LedgerDebtSystem` with consequence dispatchers and bounty records, `SilentFoundrySystem` (foundry production and accords), bounty board, trade screen scenarios and tell lines. Balance documentation lives in `docs/balance/` and `ECONOMY_PRICE_FACTOR_MATRIX.md`; the expedition destination economy was audited by a seeded 200-run simulation harness (Plan 76.2) — the pattern to follow for new economy content.

### 4.4 Ecology and wildlife (VERIFIED)

`WildlifeMigrationSystem`, `WildlifeTrappingSystem` (+ catalogs/events), `wildlife_ecosystem.json`, seasonal calendar, trapping, bestiary (`wasteland_wildlife_bestiary.json`), underground flora and fungi, ecological infestations, crop strains and genome degradation, mutated botanical logs, contagion events and pathogens (zoonotic bridge: trapping to disease with campfire-cooking sanitization — already an owned seam).

### 4.5 Weather and the Year of Ash (VERIFIED)

`WeatherSystem`/`WeatherKind`, `weather_effects.json`, `weather_seasons.json`, `weather_route_gates.json`, `weather_hardening_upgrades.json`, nuclear winter storm windows (`year_of_ash_storm_windows.json`), the Year of Ash event/item/location/quest/survivor/radio catalogs, and `YearOfAsh/` Core systems. Weather drives radiation (fallout), routes (gates), power (hardening), and morale.

### 4.6 Static canon versus dynamic state (planner rule)

Catalog JSON is static canon. Per-campaign dynamic state lives in Core systems and their save sections. Never propose rewriting catalog facts to represent campaign state. When drafting content that can be in several states, author a `state_variants` block (description focus per state: blocked / accessible / contested / evacuated / collapsed) and let systems select.

---


## PART 5 — SYSTEM AND CONTENT INVENTORY (THE DUPLICATION FIREWALL)

This is the inventory a planner consults before proposing anything. If a system or catalog already covers the concern, extend it; do not create a parallel one. (VERIFIED from repository listings; counts drift as integration waves land — re-verify against `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when precision matters.)

### 5.1 Core domain directories under `Assets/Ashfall.Core/`

AdvancedMachinery, Archaeology, Audio, Balance, Campaign, Catalogs, Clock, Codex, Cognition, CohortSystem/CohortTuning, Collectibles, Combat, Commitments, Communication, Content, Cognition, Crossing (arbitration, catalog, session), Culture, Defense, Difficulty, Diplomacy, Disease, DutyRoster, Ecology, Economy, Encounters, Endgame, Events, Excavation, Expeditions, Exploration, Factions, Farming, Feedback, Flags, Foundry, Governance, Greenhouse, InformationFlow, Institutions, Inventory, Journal, Journeys, Legacy, Lifecycle, Localization, Maritime, Medical, Memorial, Mods, MoralChoice, Muster, Narrative, NarrativeConsequence, Needs, NpcArcs, Onboarding, Orchestration, Performance, PharmaLabSystem, Phantoms, Ports, Production, Propaganda, Quests, Radiation, Radio, Random, Recreation, Research, Sanatorium, Save, Settings, Shelter, StandingRecord, StartingLevel, Spiritual, Subterranean, Survivors, Thirdonary, Treaties, UI, Underground, UtilityAI, Verdict, Waystation, World, YearOfAsh.

### 5.2 Notable single-file Core systems (selection; the full tree is larger)

AirlockSecuritySystem, ApprenticeshipSystem, ArchiveDeskSystem, AtmosphereTextSystem, AtmosphericCondenserSystem, AudioConditionSystem, AutopsySystem, BrineWaterSystem, CensusClaimSystem, CohortSystem, ContractorRosterSystem, CrossingArbitrationSystem, CryogenicAirSeparationSystem, DecontaminationSystem, DeepWellSystem, District8DeepCoastSystem, DiseaseSystem, DoseLedgerSystem, EquipmentConditionSystem, ExcavationSystem, ExpansionQuestSystem, ExpansionMasterSession, ExpeditionVehicleSystem, FactionEmbargoLedger, GenerationalLineageExtension, GrainProcessingSystem, HeliographSystem, HoldfastQuestSystem, HoldfastSession, IceRoadSystem, KitchenNutritionSystem, LandmarkDegradationSystem, LedgerDebtSystem, LibraryStudySystem, LocationEvolutionSystem, MentalHealthCrisisSystem, MusterSystem, OrbitalHarrowTelemetrySystem, PhantomMemoryEngine, RegionalTreatySystem, ShelterScheduleSystem, ShelterThermalSystem, SickListSystem, SumpFloodingSystem, SurvivorRelationsSystem, TravelingCaravanSystem, UniqueItemClaimRegistry, VentilationSystem, VinylMoraleSystem, VoluntaryRegisterSystem, VouchAccessSystem, WaterTreatmentSystem, WeatherStationSystem, WildlifeMigrationSystem, WildlifeSeasonalCalendar, WildlifeTrappingSystem, WorkshopReverseEngineeringSystem.

### 5.3 Save architecture (VERIFIED)

- Checksummed envelopes: `SaveChecksum`, `SaveEnvelopeDetection`, `SaveWireContract`, `SaveLoadHostSession`, atomic writer.
- Codec versions at v1.1.0 (pin examples): holdfast v5, year_of_ash v5, dose_ledger v2, expansion_hub v6, expansion_quest v1, weight_of_choices v2, user_settings v2.
- 62 save-store classes documented in `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` (generated; regenerate, never hand-edit).
- Slot-root isolation via `SaveSlotRoot`; store hub `SaveStoreHub`.
- Save support window pinned by `SaveSupportWindowTests` (15 tests, historical fixture corpus).

### 5.4 Authored data catalogs (`Assets/StreamingAssets/Data/`)

The following is the near-complete catalog inventory (one JSON file per domain; every file carries `schema_version`):

acoustic_triangulation_catalog, aeroponics_nutrient_catalog, agriculture_items, aircraft_parts, alloys_and_ores, amphibious_draisine_catalog, anomalies, anomalous_expedition_encounters, antigravity_survivor_fields, apprenticeship_catalog, aquaponics_system_catalog, archive_inks, armored_crawler_modules, atmospheric_sounding_catalog, audio_cues, audio_logs_expansion_05, autopsy_procedures, ballistic_shield_catalog, ballistics_workbench_catalog, belief_movements, bio_fermentation_catalog, bionics, black_flotilla_items, black_market_inventory, bounty_board, breaching_equipment_catalog, bunker_graffiti_postings, camouflage_gear, campaign_epilogues, captive_interrogations, caravan_trade_routes, caravans, carbon_composite_catalog, cargo_airdrop_catalog, cascade_rules, cassette_sets, cellulosic_ethanol_catalog, ceremonies, characters, chemical_dependency_items, chemical_syntheses, chemical_weapons, chlor_alkali_synthesis_catalog, climbing_winch_catalog, codex_entries, cohort_tuning, collectibles, combat_catalog, commitments, commodity_baselines, comms_targets, companion_animals, confession_secrets, contagion_events, crop_strains, crossing_encounters, crossing_factions, crossing_items, crossing_locations, crossing_quests, cryo_cultivars, cryogenic_air_separation, cultural_archive_tomes, cupola_foundry_catalog, currents, cvd_diamond_catalog, damaged_map_zones, decontamination_protocol_catalog, deep_lore_locations, deep_lore_survivor_fields, defenses, desperation_events, development_traits, difficulty_presets, diplomatic_treaties, direction_finding_catalog, disease_catalog, documents (dir), door_encounters, dose_items, dose_locations, dose_quests, dose_registers, duty_roles, duty_roster_locations, duty_roster_marks, duty_roster_quests, duty_roster_seasons, dynamic_questlines, ebpvd_coating_catalog, echoes, ecological_infestations, economy_goods, electrostatic_filtration_catalog, endings, environmental_atmosphere_expansion, environmental_texts_expansion_05, epilogue_chronicle, espionage_missions, events, excavation_hazard_mitigation, excavation_sites, expansion_item_tags, expansion_survivor_fields, expeditions, faction_intelligence, faction_lore, faction_radio_corpus, faction_territory, faction_war_communiques, faction_war_dialogue, faction_war_events, faction_war_journal, faction_war_location_overrides, faction_war_radio, fallout_patterns, feedback_messages, field_guide, final_wishes, fischer_tropsch_catalog, fluid_infrastructure, fog_harvesting_catalog, food_preservation, foundry_accords, foundry_faction, foundry_items, foundry_production, foundry_treaty_consequences, geodetic_survey_catalog, geothermal_drilling_depths, geothermal_strata_catalog, glassworks_recipes, gpr_exploration_catalog, grain_processing, greenhouse_items, guilt_sources, hardcore_economy_tuning, heliograph, holdfast_factions, holdfast_flavor, holdfast_items, holdfast_locations, holdfast_npcs, holdfast_quests, incidents, independent_faction_branch, infiltrator_profiles, insar_geodesy_catalog, interrogation_tactics, item_degradation, item_description_texts, items, journal_entries_expansion_05, journal_voice_prose, kinetic_flywheel_catalog, labor_camps, ledger_debt_templates, library_manuals, locations, locations_expansion3, lore_archives, low_background_lead_catalog, lyophilization_catalog, medical_texts, memorial_rites, memorials_expansion_05, mental_arcs, merchant_caravans, metallurgy_recipes, micro_locations, microfluidic_diagnostic_catalog, military_faction_branch, mine_flail_catalog, mineral_acid_synthesis_catalog, mutations, narcotics, narrative (dir — see 5.6), narrative_arc_events, narrative_discovery_manifest, narrative_encounters, narrative_encounters_expansion, narrative_encounters_npc_arcs, narrative_progression, narrative_questlines, naval_vessels, npc_arcs, nuclear_core_profiles, nutrition_profiles, nvis_communications_catalog, orbital_harrow_events, pathogens, personal_quests, phantom_heirlooms, phantom_triggers, pharma_recipes, piezometer_network_catalog, plastic_pyrolysis_catalog, pneumatic_network_catalog, policies, political_policies, powder_metallurgy_catalog, power_grid, power_subgrid_nodes, precision_broaching_catalog, precision_optics_catalog, perimeter_defenses, prewar_archives, propaganda_campaigns, psychological_therapies, psychological_trauma, quest_templates, questline_master, quests_bureaucratic_morality, quests_expansion_05, quests_expansion_06, quests_faction_branching, quests_massive_expansion_200, quests_moral_branching_expansion, quests_npc_arcs, radar_ecm_catalog, radio, radio_distress_signals, radio_distress_signals_expansion, radio_intercepts, radio_programs, radio_stations, rail_grinding_catalog, rail_logistics_catalog, rail_network, railway_interlock_catalog, rebel_faction_branch, recipes, recon_telemetry_probes, recreation, regional_prices, regional_treaties, relic_recipes, repeatable_quests, rerailing_equipment_catalog, research_knowledge, robotics, runflat_tire_catalog, sanitation_facilities, scavenging_tables, seasonal_events, seismic_fault_catalog, settlements, shelter_audio_cues, shelter_insulation_catalog, shelter_machine_identities, shelter_room_identities, shelter_rooms, shelter_schedules, shelter_shielding, shelter_social_events, skills, sky_defense_ordnance, sky_layer_armor_catalog, sofc_power_catalog, solar_concentrator_catalog, sound_ranging_catalog, spiritual_rituals, standing_record_factions, standing_record_layouts, standing_record_memory, standing_record_quests, starting_supplies, starting_survivor_cohorts, starting_survivors, subterranean_zones, sump_drainage_catalog, surgical_procedures, survivors, tablet_manufacturing_catalog, tech_salvage, thermal_gear, thirdonary_quests, toxic_chemical_catalog, trade_embargoes, trade_screen_scenarios, trade_specialties, trade_tell_lines, trade_texts, travel_encounters, trophies, underground_flora, uv_corona_detector_catalog, vehicle_armor_grades, vehicle_modifications, vehicles, verdict_data, verdict_items, verdict_locations, verdict_npcs, verdict_questlines, verdict_radio, wall_carving_templates, warlord_doctrines, wasteland_grave_epitaphs, wasteland_laws, wasteland_map_v1, wasteland_religions, wasteland_settlement_npcs, waystations, weather_effects, weather_hardening_upgrades, weather_route_gates, weather_seasons, whitelists (dir), wildlife_ecosystem, wildlife_trapping_catalog, workshop_recipes, world_evolution_events, world_evolution_seeds, world_history, year_of_ash_events, year_of_ash_items, year_of_ash_locations, year_of_ash_questlines, year_of_ash_quests, year_of_ash_radio, year_of_ash_storm_windows, year_of_ash_survivors.

### 5.5 Host surface (`src/Host/`, selection by family)

- **Sessions:** Agriculture, AirlockSecurity, AmphibiousDraisine, Apprenticeship, ArchiveDesk, Autopsy, BallisticShield, BioFermentation, BlackMarket, CargoAirdrop, Caregiving, ChemicalDependency, ChemicalRecon, ChemicalSynthesis, ChlorAlkali, Codex, Combat, ContractorRoster, Crafting, CryogenicAirSeparation, CvdDiamond, DailySurvival, Decontamination, DeepCoast, DeepWell, Defense, DoseLedger, DutyRoster, Echo, EbPvdCoating, Economy, Endgame, EquipmentCondition, Espionage, Events, Excavation, ExpansionHub, ExpansionQuest, Expedition, FactionBranch, FluidLogistics, FoodPreservation, GeodeticSurvey, GeothermalAquifer, GrainProcessing, Greenhouse, Heliograph, HoldfastRuntime, HydraulicExtrusion, InSarMapping, Inventory, Journal, KitchenNutrition, LibraryStudy, LowBackgroundMetrology, Maritime, MedicalWard, MentalHealthCrisis, MicrofluidicDiagnostic, MineClearingFlail, MoralChoice, MoraleContagion, Muster, Narrative, NarrativeQuestline, Onboarding, OralLore, PersonalQuest, PhantomMemory, Piezometer, Plans130To133, Plans74To77, PlasticPyrolysis, PrecisionOptics, ProceduralNarrative, PsychologyArc, PsyOps, Radio, RadioProgramProduction, ReconTelemetry, RegionalTreaty, Research, RailGrinding, Recreation, Sanitation, ShelterAssignment, ShelterAtmosphere, ShelterDecor, ShelterFire, ShelterSchedule, ShelterThermal, SilentFoundry, SkyDefense, SofcPower, SolarConcentrator, SoundRanging, Spiritual, StandingRecord, StartingLevel, Subterranean, Survivors, SurvivorRelations, Thirdonary, TravelingCaravan, UtilityAi, VehicleGarage, Ventilation, Verdict, VinylMorale, Waystation, WeatherHardening, WildlifeEcosystem, WildlifeTrapping, World.
- **CLI selftest families (`HostCli.*`):** SelfTests, SelfTestManifest, PanelTests, AdvancedIndustrialRecon, Cartography, Collectibles, Difficulty, DynamicWorld, EvolvingWorld, ExpansionDepth, ExpeditionPlaytest, ExportParity, FactionCommunique, Mods, MoralChoice, NpcArc, Onboarding, StartingSupplies, VehicleGarage, WastelandInhabitants, WorldExploration, WorldPlaytest, SkyDefense, Plans122to125, Plans139_141, Plans162_165, PlansB86_B89, Summary.

### 5.6 The narrative corpus (`Assets/StreamingAssets/Data/narrative/`)

A deep prose corpus exists — approximately 230 catalogs of in-world documents. Families and representative files:

- **Bunker institutional:** bunker_blueprints_codex, bunker_bureaucratic_anomalies, bunker_children_folklore (+batch 2), bunker_contraband_barter, bunker_court_verdicts (codex, batch 2), bunker_graffiti_postings, bunker_herbalism_pharmacology, bunker_maintenance_glitches (batches 2, 3), bunker_rituals_and_cults, bunker_shift_schedules_and_notices, bunker_trade_ledger_batch_2, bunker_wiretap_transcripts (batches 1, 2).
- **Technical craft records (the "authentic low-tech chemistry" voice):** activated_carbon_adsorption_records, annealing_lehr_birefringence_records, beeswax_rendering_dipping_assays, biochar_cation_exchange_reports, bisque_firing_records, borosilicate_sight_glass_thermal_shock, brain_tanning_hide_reports, brewers_yeast_krausen_audits, bullet_alloy_assay_reports, calcium_hypochlorite_titration_reports, candle_dip_mould_assays, carbide_tool_wear_audits, charcoal_mound_pyrolysis_logs, chrome_alum_tanning_assays, cold_process_soap_curing_reports, cryo_germplasm_viability_audits, deadbeat_escapement_wear_logs, deckle_mould_watermark_audits, drop_spindle_fibre_drafting_logs, forge_charcoal_ash_assays, fulling_trough_nap_assays, green_sand_bentonite_assays, hollander_beater_pulping_logs, invar_pendulum_thermal_expansion, iron_gall_ink_acidity_reports, kiln_draw_trial_assays, langstroth_hive_foundation_logs, lime_kiln_calcination_logs, mainspring_fatigue_rupture_audits, mill_dampener_tempering_assays, mudbrick_weathering_assays, oak_bark_tanning_pit_logs, pattern_maker_shrinkage_records, pozzolan_mortar_formulations, rag_pulp_beater_records, retort_wood_vinegar_audits, rope_break_load_assays, slow_sand_schmutzdecke_logs, sourdough_mother_acidity_logs, square_set_shoring_audits, three_strand_rope_closing_logs, tallow_saponification_kettle_audits, tub_sizing_gelatin_assays, water_clock_orifice_silt_records, wick_braiding_priming_reports, wire_rope_stranding_assays, and dozens more.
- **People and psyche:** dweller_dependency_backstories, dweller_heirlooms_master, dweller_medical_casebook, dweller_psychological_journals, therapist_session_notes (batches 1–3), unsent_letters_batch_2, survivor_letters_lost_kin, survivor_profiles_expansion, confession_secrets, guilt sources (catalog), new_arrival_intake_interviews.
- **Faction and radio:** faction_directives_and_notices, faction_field_documents, faction_texts_expansion, radio_broadcast_rundowns, radio_scriptbook, radio_scripts_expansion, radio_transcripts (batches 2, 3), radio_mysteries_expansion, numbers_station_ciphers, ghost_transmissions, dead_hand_directives, cobalt_arming_directives, cobalt_liturgies (batch 2), iron_synod_canons, geophone_hymnals.
- **World and nature:** crater_lake_limnology_records, cave_aquatic_biota_logs, blind_cave_molerat_studies, armored_cockroach_hive_logs, carrion_vulture_sighting_logs, mutated_botanical_logs, permafrost_methane_eruption_logs, stalactite_mineral_assay_reports, silo_mosquito_vector_records, grain_silo_weevil_audits, root_cellar_humidity_rot_reports.
- **Field operations:** courier_dispatches_master, courier_mission_logs (+batch 2), expedition_briefs_expansion, expedition_field_reports (+batch 2), expedition_planning_briefs_batch_1, expedition_route_waypoint_notes_batch_2, patrol_debriefs, scavenger_expedition_route_notes, wasteland_expeditions_master, security_incident_reports_batch_2, pipeline_sabotage_records, vault_seal_breach_logs, load_shed_schedule_001.
- **Memory and memorial:** eulogy_corpus_batch_1, memorial_expansion, undertaker_burial_records, wasteland_grave_epitaphs (+batch 2), relic_provenance_dossiers, personal_effects_inventory_batch_2, found_objects_expansion, heirloom_seed_viability_reports, phantom_heirlooms/triggers.
- **Deep lore:** deep_lore_texts, prewar_archives, lore_archives, lost_tech_manuals, salt_mine_inscriptions, wall_carving_templates, bunker children folklore, wasteland_religions, wasteland_settlement_gazetteer, world_history_expansion, weather_almanac_expansion.

### 5.7 UI panels (`src/UI/`, selection)

Achievements, Afflictions, AirlockSecurity, AmphibiousDraisine, AmputationTriage, AnaerobicBiogasDigester, AnomalyWatch, Apprenticeship, AquiferTreatyConcession, ArchaeologyExcavation, ArchiveDesk, Beliefs, Bestiary, BioFermentation, BlackMarket, BlackProjectsArchive, BoreholeSeismograph, BrineExtraction, CaravanBarterLedger, Caregiving, CargoAirdrop, CenturySeed, CeremonyFestival, ChemWarfareDefense, ChemicalDependency, ChemicalLab, ChemicalRecon, Chronicle, ClandestineInsurgency, Combat (panel, HUD overlay, detail, history), CommsArrayTransceiver, ContractorRoster, Crafting, CrossingQuest, CrossingSafeConductVouch, CryogenicPermafrostCore, CvdDiamond, Cybernetics, DailyBriefing, DeconAirlock, Decontamination, DeepCoast, DefenseGrid, DesperationCrisis, DoseGeography, DoseLedger, DutyRoster (+detail), DynamicQuestline, EbPvdCoating, EconomyDetail, ElectrostaticScrubber, EmergencyResponseHud, Epilogue, EquipmentCondition, EventDetail, EventsLog, Excavation, ExpansionsHub, Expedition (camp, radar), and the shell components (AshfallDashboardShell, Sidebar, StatusRail, DataGrid, MetricCard, FocusNavigator, FocusPolicy, UiHelpers, ConfirmationModal, AnalogConditionGauge, BackdropArt). Design language is pinned by `DESIGN.md` (ASHFALL Tactical UI): charcoal surfaces, washed CRT green-white `#c7dcd0`, hazard orange `#ff6b35`, BarlowCondensed caps for headers, ShareTechMono for body, 1px hard borders, zero border-radius, segmented progress bars, fixed 1920×1080.

### 5.8 Docs map (where to look before planning)

- `AGENTS.md`, `AI_AGENT_WORKFLOW.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`.
- `docs/CURRENT_AUTHORITY.md` (navigation), `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` (what exists), `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` (how it connects), `docs/ASHFALL_MASTER_IMPLEMENTATION_PLAN.md`, `docs/ASHFALL_CODE_INDEX.md`.
- Domain authority maps: `CAPTIVE_SYSTEM_AUTHORITY_MAP`, `EPILOGUE_METRIC_AUTHORITY_MAP`, `FACTION_ESPIONAGE_AUTHORITY_MAP`, `FOOD_PRESERVATION_AUTHORITY_MAP`, `PREWAR_ARCHIVE_AUTHORITY_MAP`, `SHELTER_ACOUSTIC_AUTHORITY_MAP`, `SURVIVOR_MENTAL_HEALTH_AUTHORITY_MAP`, `VEHICLE_GARAGE_AUTHORITY_MAP`, `CODEX_CONTRACT`, `L10N_CONTRACT`, `MORAL_CHOICE_SYSTEM`, `CAMPAIGN_INFORMATION_FLOW`, `CAMPAIGN_PROVENANCE_CONTRACT`, `CONTENT_AUTHORITY_AND_MIGRATION_STATUS`.
- Generated (never hand-edit): `docs/cli/HOST_CLI_COMMAND_CATALOG.md`, `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`, `KEYBOARD.md`, changelog generated region, `docs/INDEX.md` plan paths, `docs/ui/SNAPSHOT_COVERAGE.md`.

---


## PART 6 — GAMEPLAY-NARRATIVE SYSTEMS (HOW CONTENT BECOMES PLAY)

### 6.1 Quest architecture (VERIFIED owners)

- **Questline master:** `questline_master.json` + `QuestlineMasterCatalog` — the spine for major questlines.
- **Moral choice system:** `moral_choice_chains/factions_reactions/flags/gossip/quests*.json`, `MoralChoice/` Core, `MORAL_CHOICE_SYSTEM.md`. Choices produce flags, faction reactions, gossip, and `weight_of_choices` epilogue weight.
- **Dynamic questlines:** `dynamic_questlines.json`, `DynamicQuestSaveStore`.
- **Personal and NPC arcs:** `personal_quests.json`, `npc_arcs.json`, `quests_npc_arcs.json`, `NpcArcs/`, `narrative_encounters_npc_arcs.json`.
- **Repeatable quests, quest templates, branching faction quests** (`quests_faction_branching.json`), bureaucratic morality quests (`quests_bureaucratic_morality.json`), massive expansion corpus (`quests_massive_expansion_200.json`).
- **Domain questlines:** dose quests, year-of-ash questlines, holdfast quests, crossing quests, thirdonary quests, verdict questlines, expansion quests (`ExpansionQuestSystem`, `expansion_hub` v6 codec).

Quest record contract when drafting: `id`, `type`, `status`, `title`, `priority`, `connections` (characters/locations/factions/items/arcs — by ID only, never re-biography), `purpose` (narrative/gameplay/thematic), `availability` (requires/excludes/discovery_methods), `stages` (id, title, objectives, location_requirements), `branches` (choice, effects on trust/state), `outcomes` (success/failure/abandonment), `prose_fields` (see Part 9).

### 6.2 Expedition rules (VERIFIED)

Dispatch consumes party roster (scouts/guards/medics), stance, supplies (ammo, water, filters, fuel), map distance, and fog state. Systems: `ExpeditionHostSession`, `ExpeditionSystem`/`Expeditions/`, `StartExpedition` bridges, `ExpeditionVehicleSystem`, dispatch preflight (`RescueDispatchPreflight`), weather gates with force-passage, per-destination scavenging tables (49 tables across 53 destinations; renewable trade-stock for living settlements, one-time caches for supply caches), micro-location encounters, anomalous expedition encounters, travel encounters, and the 30-day expedition playtest report (`docs/expeditions/EXPEDITION_30_DAY_PLAYTEST_REPORT.md`). Consequences flow into dose ledger, ARS progression, trauma bonds, faction standing, and the epilogue matrix.

### 6.3 Encounter and event surface

`narrative_encounters*.json`, `Encounters/`, door encounters, travel encounters, seasonal events, desperation events, incidents, echoes, anomalies, world evolution events (with seeds for determinism), cascade rules (`cascade_rules.json` — cross-system consequence propagation), `Events/` Core + `EventsHostSession`. Consequence delivery is routed through `NarrativeConsequence/`, journal entries, radio, and `FactionStanceEngine`.

### 6.4 Radio and information (VERIFIED)

`radio.json`, stations, programs, intercepts, faction radio corpus, distress signals (+ expansion) with the Rescue Signal Runtime: `DistressRescueMissionManager`, `DistressDestinationResolver`, `SignalAuthenticityEvaluator` (skill-driven detection with dedicated RNG sub-stream, genuine-never-hostile invariant, persisted first result), ignore consequences (deadline, sender death, faction standing loss, ambush — closed vocabulary, exactly-once guards), sender survival (`SenderDeathDay = firstHeardDay + SenderSurvivalDays`; arrival day governs live-rescue vs remains). Radio drives market rumors, weather, warlord warnings, sound-ranging observations, and journal records.

### 6.5 Survivor interiority (VERIFIED)

`Survivors/` Core: needs, health, skills, afflictions, development traits, mental arcs (`mental_arcs.json`, `PsychologyArcHostSession`), psychological trauma and therapies, guilt sources, guilt insomnia, somatic flashbacks, mental health crises, morale contagion, survivor relations and social state, caregiving, chemical dependency, companion animals, belief movements, spiritual rituals, memorial rites, final wishes, personal belongings (`ClaimPersonalBelonging` — note: a review found it had no callers; verify before extending), memory decay (`MemoryDecaySystem`), phantom memory engine (heirlooms and triggers), generational lineage and cohort system, apprenticeships.

### 6.6 Endings and the epilogue matrix (VERIFIED)

`endings.json`, `campaign_epilogues.json`, `epilogue_chronicle.json`, `Endgame/`, `ReckoningSystem`, `VerdictEndingEvaluator`, `EpilogueMatrixRuntime`, muster epilogues, holdfast endings, Crossing endings (prose pinned to house-voice wording). The saga resolves through a 32-permutation epilogue matrix; the Machine Reckoning at Day 360 consumes enrolled forensic evidence, machine-log reckonings, and census data. Every expansion that touches canon-relevant state must state which epilogue permutations it affects.

### 6.7 Reward, failure, and recovery grammar (planner rule)

- Rewards: items (existing IDs), standing, knowledge/research, flags, world-state changes, epilogue evidence. Avoid inventing new currencies.
- Failure produces consequences with recovery paths: injury chains feed the medical pipeline; standing loss can be rebuilt through treaty, tribute, or service; abandoned quests can reopen after new discoveries.
- Never propose a fail state without a recovery path or an explicit epilogue consequence.

---

## PART 7 — DECISION HORIZONS AND PACING (WHERE NEW PRESSURE BELONGS)

| Horizon | Information the player receives | Resources manipulated | Core uncertainty | Executing systems |
|---|---|---|---|---|
| Minute-to-minute | Vitals gauges, room temperature, crafting queue, sound cues | Active survivor assignment, station focus, radio dial | Exact item yield, audio hazard triggers | Needs, Crafting, RadioTuner |
| Shift-based | Roster slots, sickness bands, intake filter status | Shift labor, filtration jobs, greenhouse watering | Work accident rolls, flashback triggers | DutyRoster, PowerGrid, WaterTreatment |
| Daily | Briefing modal, weather transition, distress intercepts, food stocks | Ration tier, dosimeter clearance, medical procedures | Night raids, incubation manifestation | DailySurvival, Disease, RationConflict |
| Expedition (multi-day) | Map danger tier, route distance, forecast, vehicle fuel | Party, stance, supplies | Encounter threat, loot rolls, breakdowns | Expeditions, WastelandMap, Combat, Radiation |
| Strategic (weekly/monthly) | Tribute demands, price shocks, debt statements | Tribute payments, loans, treaty quotas | Doctrine shifts, caravan schedules | WarlordDoctrine, LedgerDebt, Market, Foundry |
| Endgame/saga | Reckoning evidence, census, epilogue chronicle | Treaty ratification, evidence submissions, ledger burning | Tribunal verdict, lineage viability | Reckoning, Verdict, EpilogueMatrix, Muster |

### Known pacing gaps (HIGH CONFIDENCE, from the atlas — prime expansion targets)

1. **The mid-winter slump (Days 90–180):** once water and power stabilize, pressure can settle into routine. Candidate expansions: mid-game faction conscription levies, crop blight epidemics, deep-strata seismic cave-ins.
2. **Delayed narrative callbacks:** early moral choices in `DoorEncounterSystem` lack multi-month delayed callbacks. Seam: store choice flags in `IFlagLedger` and spawn vengeful or grateful returns ~100 days later.
3. **Low-connectivity islands** (VinylMorale, WildlifeTrapping, SkyLayerArmor, Cohort/Lineage) are partially bridged (vinyl-to-radio, zoonosis-to-disease, armor-to-weather, youth-to-apprenticeship); verify current bridging before proposing more, then extend to factions, expeditions, and epilogues.
4. **Foreman-flagged open decisions:** black-market funds/goods legs (needs canonical funds authority), merchant restock priority (needs signed ordering design), FactionWar per-strike emitter extension, flooded-route topology tags (needs authored map edges).

---

## PART 8 — WRITING BIBLE (PROSE OF ALL TYPES AND KINDS)

### 8.1 House voice (CANON)

The house voice is restrained, concrete, exhausted, human, and bureaucratic. It trusts objects over adjectives. The narrative corpus demonstrates it: a rope is measured by its break-load assay; grief is recorded in an undertaker's ledger; belief appears as a liturgy or a shift notice. Prose should read as if the wasteland keeps records, and the records are the poetry.

Qualities:

- Concrete rather than abstract. Names, quantities, materials, procedures.
- Sensory but restrained — one or two sensory anchors per passage, never a perfume catalog.
- Emotionally observant without stating the emotion. The character tightens a strap; the narrator does not say she is afraid.
- Avoids explaining every implication. Trust the player to assemble meaning.
- Uses location details to reveal history (a chamber's wear tells its use).
- Bureaucratic registers are diegetic: manifests, audits, titration reports, load-shed schedules, court verdicts, epitaphs, rundowns, hymnals, ciphers.

### 8.2 Sentence and dialogue style

- Short sentences during danger and decision. Longer sentences during reflection and aftermath.
- Sparse semicolons. No ornamental description where the player needs clear information.
- Every speaker wants something in the scene. Dialogue carries subtext.
- Characters rarely state complete motivations directly; they may be wrong, biased, or incompletely informed.
- NPCs never explain facts they would assume the listener already knows.
- Bureaucratic speakers use procedural, legal, reassuring public language; private language is transactional, impatient, or threatening.
- Each named character needs a distinct voice signature (sentence length, vocabulary domain, avoidance subjects, repeated patterns). Define these in the entity record's `voice` block.

### 8.3 Tone restrictions (banned patterns)

- Generic "ancient evil" or empty darkness imagery.
- Prophecy language and chosen-one framing.
- Modern internet slang or anachronistic modern idiom outside satirical radio bits.
- Identical voices for all characters.
- Real countries, wars, people, brands, or events.
- Grief porn: melodramatic death scenes. The corpus models restraint (a burial record, an unsent letter).
- Exposition dumps disguised as dialogue or codex entries.

### 8.4 Document genres already established (extend these, do not invent parallels)

Manifest, audit, assay report, titration record, log, journal entry, diary, letter (sent/unsent), intake interview, therapy note, casebook, court verdict, wiretap transcript, communiqué, directive, liturgy, hymnal, canon (religious), epitaph, eulogy, burial record, provenance dossier, rundown, scriptbook, cipher, dispatch, debrief, field report, waypoint note, planning brief, schedule notice, graffiti, carving, folklore (children's and adult), song, almanac entry, gazetteer entry, bestiary entry, genealogy, treaty protocol, permit, load-shed schedule, maintenance glitch report, risk-of-failure wishlist.

### 8.5 Example patterns (house demonstrations)

Model sentences (study the corpus for the full pattern):

- Technical: "The batch held its break load within tolerance until the third wetting; the ledger records the shortfall, not the cause." (Assay-register: fact, tolerance, implied hand.)
- Personal: "She signed the ration ledger with his mark because her hands had not stopped shaking since the gate." (Action carries emotion; institution frames it.)
- Bureaucratic: "Per Schedule 001, the corridor will be dark from the nineteenth hour. Complaints may be entered in the register and will be read in spring." (Deadpan institutional voice.)
- Radio: "This is the frequency that still answers. If you can hear this, you are not finished." (Sparse, human, signal-as-hope.)

Anti-examples (do not produce):

- "An ancient evil stirred beneath the cursed earth as the survivors huddled in fear." (Banned imagery, abstraction, emotion named.)
- "Hey guys, the shelter's looking pretty rough today, ngl." (Register violation.)
- "Mara explained that she had lost her brother during the archive fire and that she now distrusted all officials." (Motivation stated outright; NPC over-explains.)

---


## PART 9 — PROSE FIELD SPECIFICATIONS

When drafting prose, never "write a description." Fill a named prose field with a contract. The canonical field set (extend with new fields only when a loader consumes them):

| Field id | Purpose | Trigger | Length | Must include | Must not include |
|---|---|---|---|---|---|
| `map_label` | Cartographic identity | Map render | 2-4 words | Distinctive proper noun | Sentences |
| `map_description` | Map hover/legend text | Map focus | 15-30 words | One concrete fact | Quest spoilers |
| `arrival_description` | First impression | First arrival | 80-140 words | One landmark, one sensory detail, one danger indication | Undiscovered information, hidden reveals |
| `revisit_description` | Changed-state impression | Return visits | 60-110 words | Sign of prior visit, sign of elapsed time | Repeating first-arrival imagery |
| `changed_state_description` | State-variant text | Location/quest state change | 50-100 words | The state's cause made visible | Contradicting prior state |
| `quest_hook` | Why the player cares | Quest offer | 40-80 words | Stakes for a person, not a concept | Mechanics language |
| `journal_entry` | Player's own record | Event/quest beat | 60-120 words | First person, house restraint | Omnipotent narration |
| `objective_text` | Task instruction | Objective update | 1 line | Action + target location/entity | Flavor padding |
| `codex_entry` | World knowledge | Discovery | 80-150 words | Public account framing | Hidden account before evidence |
| `environmental_clue` | Embedded history | Inspection | 25-60 words | Wear, tool marks, residue | Explicit exposition |
| `item_inspection_text` | Object interiority | Item examine | 30-70 words | Material, provenance hint | Lore dumps |
| `rumor` | Distorted information | Gossip/radio | 30-60 words | A kernel of truth plus distortion | Verified facts |
| `success_text` / `failure_text` / `abandonment_text` | Outcome framing | Resolution | 40-90 words | Consequence, next-pressure hint | Moralizing |
| `relationship_reaction` | NPC response | Trust stage change | 30-70 words | Stage-appropriate behavior | Out-of-stage intimacy |
| `world_state_notification` | System feedback | State change | 1-2 lines | What changed, plainly | Alarm spam |
| `epitaph` / `eulogy` | Memorial | Death/memorial | Epitaph 6-20 words; eulogy 80-160 | Life compressed to record | Sentimentality |
| `radio_rundown` / `transcript` | Broadcast texture | Radio play | Rundown 3-8 lines; transcript 60-150 | Station identity, signal framing | Modern idiom |
| `expansion_callback` | Long-delay return | Author-specified day/window | 50-100 words | Recognition anchor from prior content | New-entity clutter |

Field contract format for new prose fields:

```text
prose_field:
  id: [SNAKE_CASE]
  purpose: [one sentence]
  audience: player
  trigger: [when it displays]
  length: [range]
  viewpoint: [player perception / document voice / narrator]
  must_include: [...]
  must_not_include: [...]
  tone: [register]
  spoiler_level: [low/medium/high]
  localization_priority: [high/medium/low]   # see L10N_CONTRACT.md; string freeze D22 pending
```

Retrieval tags to attach to every drafted content unit:

```text
tags: [region:..., theme:..., quest:..., location:loc_*, character:..., faction:...,
       state:..., prose:arrival|revisit|journal|codex|..., priority:..., spoiler:...,
       production_status:draft|approved]
```

### Corpus-matching rule

Before finalizing any prose batch, list which existing `narrative/*.json` catalogs the batch extends and which it could duplicate. Two catalogs describing the same craft or institution must differ in voice or evidence, or they must be merged.

---

## PART 10 — THE EXPANSION PLAN FORMAT (HOW PLANS ARE DRAFTED HERE)

The repository has run 185+ numbered plans in waves, with per-plan closeouts, authority maps, and second-tool reviews. New plans must follow this discipline. Master format:

```text
# Plan [ID] — [Specific outcome]
## Premise and current evidence
[What exists now: exact files, systems, catalogs, counts. Verified before planning.]
## Non-goals
[What this plan explicitly does not do.]
## Finding / opportunity
[The demonstrated gap or extension, labeled VERIFIED / HIGH CONFIDENCE / PROPOSAL.]
## Proposed change
[Smallest coherent solution. Data-only, host-wiring, or Core change — say which tier.]
## Integration steps
[3-8 ordered steps, naming the seam at each step: catalog file, loader, Core system,
 host session, save store/codec version, UI panel, event routing.]
## Files likely affected
[Only verified or strongly supported paths.]
## Determinism and save impact
[RNG sub-stream, capture/restore, codec version bump + migration, checksum behavior.]
## Verification
[Exact commands: focused xUnit file(s), godot headless selftest flags, replay/determinism
 proof, balance simulation harness where relevant.]
## Risk
[Low/Medium/High + one sentence.]
## Dependencies / conflicts
[Other plans, ownership claims, decision-blocked items.]
## Done when
[Concrete acceptance criteria.]
## Closeout
[Results, evidence, deviations, follow-ups — recorded as docs/plans/PLAN[N]_CLOSEOUT.md.]
```

Plan-drafting rules:

- One plan = one bounded outcome riding existing seams. No plan may create a parallel authority.
- Data-first: most expansion is authored JSON through existing catalogs and loaders, requiring zero new C#. Say so when true.
- If new state is needed: new save section only via the current save-section owner; bump the codec version; write the migration; add round-trip tests.
- If a new panel is needed: it exposes an existing command and truthful state; snapshot coverage and a11y gates apply.
- Verification must be runnable headless or via focused xUnit. "It should work" is not acceptance.
- Register the plan in `INTEGRATION_PLANS.md` and claim exact paths in `WORKTREE_OWNERSHIP.md` before implementation.

### Plan-series pattern (for large expansion campaigns)

Wave structure used successfully in this repository: a flagship plan (Tasks 1-N across Core/host/data/UI/tests), complementary thin-seam plans (deferred UI wiring, additive ports), debt-closure batch, then a second-tool review (independent audit of the battery, determinism, and closeouts). For multi-million-character planning campaigns, draft: (1) a wave charter naming the domain and non-goals; (2) flagship plan; (3) 2-6 satellite plans; (4) verification matrix; (5) closeout templates. Never draft more than five concurrent plans that touch shared seams.

---

## PART 11 — EXPANSION LANES BY SUBJECT (THE ALL-SIDES MATRIX)

The planner should rotate across lanes rather than deepening one. Each lane lists current owners, high-value openings (labeled by confidence), and typical plan shape.

### Lane A — Narrative and prose (all types and kinds)
Owners: Part 5.6 corpus, Part 8/9 contracts, `Narrative/`, `ProceduralNarrativeHostSession`, environmental text and atmosphere systems, journal voice prose.
Openings: delayed moral-choice callbacks (HIGH CONFIDENCE); mid-winter slump story pressure (HIGH CONFIDENCE); destination-bound micro-location expansion beyond the current three; oral lore and radio program expansion; epilogue-chronicle depth for under-served epilogue permutations; phantom memory triggers tied to surviving cohorts. Plan shape: data-first JSON into existing catalogs + integrity + utilization selftests.

### Lane B — Mechanics and systems functionality
Owners: Part 5.1/5.2 inventory.
Openings: bind remaining decision-blocked seams once signatures exist (black-market funds legs, merchant restock priority, FactionWar per-strike emitters, flooded-route topology tags); extend island systems into faction/epilogue space (VinylMorale into cultural standing; SkyLayerArmor into warlord siege math). Plan shape: small Core extension + host wiring + save codec bump.

### Lane C — Economy and balance
Owners: `Economy/`, commodity baselines, regional prices, hardcore tuning, scavenging tables, Plan 76.2 harness.
Openings: re-run the deterministic balance simulation after any loot/economy authoring; audit dominated goods and runaway loops mathematically, not aesthetically; extend quantity-band trims where E[value] outliers persist; time-to-kill and income-versus-expenditure audits for new content. Plan shape: data + balance simulation report in `docs/balance/`.

### Lane D — Save, state, and compatibility
Owners: Save architecture (Part 5.3), `SaveSupportWindowTests`, migration codecs.
Openings: any plan that adds state must define v(n)→v(n+1) behavior for every affected codec; test old-save→new-build, missing field, unknown field, corrupted entry, mid-event save. Plan shape: codec bump + migration + round-trip and fixture tests. Never propose destructive schema changes without migration analysis.

### Lane E — UI, UX, and accessibility
Owners: `src/UI/` panels, `DESIGN.md`, `ACCESSIBILITY.md`, input contract (`AshfallInputActions`, 22-action map, focus navigator, rebinding).
Openings: panels that render stale or missing data for newer systems (verify via `--ui-layout-selftest`, `player_panels_uitest`, snapshot coverage); a11y gating for any new words-not-color-only information; controller parity for new panels. Plan shape: host/UI wiring only, no gameplay authority in panels.

### Lane F — Performance
Owners: `Performance/`, `PerformanceSelfTest`, CI performance gate.
Openings: profile before rewrite; candidates flagged in the atlas (repeated tree searches, per-frame allocations, signal traffic) require measurement first. Plan shape: measurement harness + before/after numbers in `docs/perf/`.

### Lane G — Testing and verification
Owners: `TEST_POLICY.md`, xUnit tree, HostCli selftest battery (57 CI gates at v1.1.0: 53 fast + 3 full + 1 performance).
Openings: tests only for uncovered confirmed defects, new public contracts, save/load, determinism, lifecycle, mutation, state transitions, cross-system consequences. Aggregate homogeneous catalog checks with per-row failure output; use TEST-AGGREGATION metadata. Plan shape: focused test files, run alone first via `scripts/run_test.sh`.

### Lane H — Tooling and developer experience
Owners: `scripts/ci/`, `tools/`, generators (changelog, CLI catalog, save-store matrix, version gate), asset gate.
Openings: validators for newly authored content kinds; deterministic seed tooling; content-utilization reporting extensions. Constraint: a tool must solve a recurring demonstrated workflow problem and be smaller than the workflow it replaces.

### Lane I — Documentation and conventions
Owners: `docs/` map (Part 5.8), generated docs, closeout discipline.
Openings: authority maps for domains that gained systems since their last map; the docs index drift gate; L10N wave roadmap continuation (respect string freeze D22 while pending). Documentation must describe reality; never document an architecture that does not exist.

### Lane J — Onboarding and player experience
Owners: `Onboarding/`, onboarding selftest, manual playthrough checklists, difficulty presets and full binding (CF-XP01).
Openings: difficulty preset scalar consumers; onboarding flow for waves of newly added systems; daily briefing surface for new content.

### Lane rotation discipline

For "expand from all sides": pick at most one plan per lane per wave; sequence so that data-first lanes (A, C, J) precede wiring lanes (B, D, E) in the same domain; reserve Lane G additions for plans that introduced new contracts. State the lane in every plan header.

---


## PART 12 — PRODUCTION SCHEMAS AND CONTENT AUTHORING CONTRACT

### 12.1 Catalog authoring rules (CANON)

- One JSON file per domain; `snake_case` ids; root `schema_version` on every file.
- Cross-references must resolve (items, locations, factions, survivors, quests, skills); the `CatalogIntegrityValidator` checks cross-reference, range, and uniqueness; the data-integrity selftest must exit 0 with 0 findings.
- Loot references must point at real item ids (historical defect class: `bandages`→`bandage`, `food_rations`→`dried_rations`, `copper_wire`→`copper_wire_10m_of_10m` — regressions guarded by `Plan76DestinationLootReferenceTests`).
- Map nodes must exist in the locations catalog (guarded by `AllMapNodes_ExistInLocationsCatalog`).
- New catalog kinds require: loader, integrity rules, host session consumption, and a utilization path (presence in JSON is not reachability).
- Zero new item ids when existing ids suffice (repository norm: reuse and tag with `expansion_item_tags.json`).

### 12.2 Entity record templates

Character/survivor record:

```text
id: [snake_case]
type: character
status: [CANON/DRAFT/PROPOSAL]
name, aliases, role
summary
identity: age, origin, occupation, public_reputation, private_reality
psychology: core_desire, immediate_goal, fear, wound, contradiction,
  moral_boundary, false_belief, true_need
voice: sentence_length, vocabulary, humor, avoids, repeated_patterns
knowledge: knows / suspects / does_not_know / cannot_know_yet
relationships: [{target, type, initial_trust}]
arcs: [{id, start_state, pressure, possible_resolution}]
gameplay: quest_roles, location_roles, expedition_relevance
prose_requirements: must_show / must_avoid
```

Relationship record:

```text
id, source, target, status
dimensions: trust, respect, fear, dependence, resentment, affection
stages: [{id, range, behavior}]
change_events: [{event, effects}]
```

Location record:

```text
id: loc_*
name, region, location_types
identity / short_description
narrative_purpose / gameplay_purpose
spatial: neighbors, travel_cost, entry_routes, hazards
history: founded_by, former_purpose, historical_events,
  public_misconception, hidden_truth
availability: map_visibility gates, expedition_selection
  (guaranteed_when / optional_when), revisitable
capabilities: supports / does_not_support
quest_connections (by id)
state_variants: blocked / accessible / contested / evacuated / collapsed
prose_fields: see Part 9
```

### 12.3 Save authoring contract (CANON)

- Every new persisted payload: DTO, codec, schema_version, migration from prior version, checksum participation, atomic write through the shared writer, slot-root isolation.
- Old-save→new-build must load; unknown fields tolerated; missing fields defaulted explicitly; corrupted envelopes rejected, not silently repaired.
- Mid-event and mid-combat saves (where supported) must round-trip exactly-once effects (the Rescue Signal runtime models this: persisted first-result anti-reroll, exactly-once consequence guards, duplicate arrival guards).

### 12.4 Determinism authoring contract (CANON)

- All randomness via `ISeededRng`; dedicated sub-streams per subsystem (pattern: `StableHash`-derived stream names).
- No wall-clock, no GUID, no unordered-hash iteration in simulation.
- New simulations should ship a two-pass byte-identical determinism proof (pattern: Plan 76.2 harness).

---

## PART 13 — QUALITY: VALIDATION, CONTINUITY, AND CHANGE CONTROL

### 13.1 Two-level validation of generated content

Structural (machine): valid ids; required fields; allowed types; resolvable references; valid state values; no duplicate ids; schema_version present. Command surface: `--data-integrity-selftest`, `--content-utilization-selftest`, focused xUnit.

Narrative/continuity (planner-audited): the character knows only what they could know at that point; the location exists and is in the right state; the item is owned by the right owner; the timeline is possible; the quest is completable; the branch has a recovery path; the dead do not speak in the present; the faction has the resources it uses; the expansion is compatible with every affected ending; the prose reveals only scene-appropriate information.

Output format for validation reports in plans:

```text
validation:
  structural: { passed, errors[] }
  continuity: { passed, warnings[] }
  gameplay:   { passed, warnings[] }
  prose:      { passed, warnings[] }
```

### 13.2 Continuity checklist (run before submitting any plan or prose batch)

1. All entity ids referenced exist in catalogs.
2. No duplicate catalogs or overlapping institutions without differentiation.
3. Information-flow legality (who can know what, through which channel).
4. Timeline legality (no anachrony without an explicit historical framing).
5. State legality (location/faction/survivor states consistent with the campaign window).
6. Voice consistency (per-entity voice signatures; house register; banned patterns absent).
7. Save/determinism impact assessed and stated.
8. Verification commands named and runnable.
9. Epilogue compatibility stated (which permutations are touched).
10. No new parallel authority; the owner system named for every state change.

### 13.3 What not to put in one plan

Avoid plans that bundle: the full history, all characters, all quests, all dialogue, all items, all branches, implementation notes, and brainstorming. One bounded outcome per plan; cite entities by id; park loose ideas in a `docs/plans/` parking-lot note labeled NON-CANON.

### 13.4 Change log protocol

Every accepted plan updates the repository's changelog (`CHANGELOG.md`, generated region via `generate_changelog.py --check`) and, for canon-affecting changes, a change record:

```text
## [Plan ID / Version] — [date]
### Added   [new canon/content]
### Changed [what was altered; old fact -> new fact]
### Deprecated [superseded explanations]
### Continuity consequences [what older content must stop claiming]
### Requires review [files/quests needing re-audit]
```

Consult the change log when revising older content; do not draft against deprecated facts.

---

## PART 14 — APPENDICES

### 14.1 Verification command surface (VERIFIED)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # full tier only with reason
dotnet build Ashfall.csproj
bash scripts/run_test.sh <test-file-or-focused-directory>    # focused xUnit, 180s cap
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --ui-layout-selftest
godot --headless --path . -- --expedition-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --plans-122-125-selftest
./scripts/ci/godot-asset-gate.sh
bash scripts/ci/verify-fast.sh        # mirrors all fast CI gates
bash scripts/ci/triad-drift-gate.sh   # Setup/Save/Flush parity
```

Full gate inventory at v1.1.0: 57 gates (53 fast + 3 full + 1 performance). Godot runtime sessions for testing: 15 FPS unless the user requests otherwise.

### 14.2 Release and version discipline

Three-source version agreement (`project.godot`, `Directory.Build.props`, `export_presets.cfg`) enforced by `version-gate.py`; semver bump classification via `ReleaseVersion.cs`; `VERSIONING.md`, `PROCESS.md`, `TEMPLATE.md` govern releases; save support window pinned per release. v1.1.0 tagged 2026-09-19.

### 14.3 Current queue snapshot (2026-09-19 audit; re-verify before use)

Unblocked and available: `CF-P1-DISTRESS-CONTENT-SEAL`, `CF-P5-RESTOCK-RECONCILE`, `CF-P6-VEHICLE-ARMOR-GRADES`, `CF-P28-ONE-BOOTSTRAP-PATH`, `CF-XP01-DIFFICULTY-FULL-BINDING`, `E1/Plan 53` census governance, `C2[15]/Plan 37` input/focus/controller parity, `C2[21]/Plan 48` release craft. Decision-blocked (never start without the named signature): D11, D21, F13, F14, EN-01..EN-08, Plan 49, C3 HOLDs 174/175/192/199, D22 string freeze. Partial-wave placeholders tracked in `docs/plans/PARTIAL_REMAINING_PLACEHOLDER_2026-09-19.md` (Plans 185, 162, 216, 202, 163, 210 now have bounded logs).

### 14.4 Planner's first-session checklist

1. Read `AGENTS.md`, then this document, then `INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md`.
2. Pick one lane from Part 11; state it.
3. Verify premises in live source before drafting (counts and APIs drift; this document is a snapshot dated 2026-09-23).
4. Draft the plan in Part 10 format with fact statuses on every claim.
5. Attach prose field contracts (Part 9) for any prose deliverable.
6. Run the continuity checklist (13.2); include the validation block (13.1).
7. Deliver: plan, facts used, new facts introduced, continuity risks, verification steps.

### 14.5 Known uncertainty in this document

- Catalog, system, and test counts drift with each wave; treat all counts as approximate and re-verify against the generated registries.
- Codec versions listed are the v1.1.0 snapshot; consult `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` for current values.
- The `ClaimPersonalBelonging` no-caller finding comes from a Copilot review of PR #62; runtime verification is outstanding.
- Some content counts in the atlas (318 Core files, 296 catalogs, 2,194 tests) predate later waves (which report 11,098 and 11,697 test totals and 192 mapped subsystems); the larger, newer figures are the more reliable ones.

*End of master bible. Authority remains with live repository source; correct this document when source disagrees.*

---

## PART 15 — WORKED EXAMPLES (MODEL OUTPUTS FOR THE PLANNER)

### 15.1 Example mini-plan (Lane A, data-first)

```text
# Plan 220 — Delayed Moral-Choice Callbacks (Lane A: Narrative)
## Premise and current evidence
VERIFIED: `moral_choice_flags.json` defines flag ids; `IFlagLedger` persists choice
flags; `DoorEncounterSystem` emits choice events; `MoralChoiceSaveStore` (weight_of_choices
v2 codec) persists weights. VERIFIED: no consumer re-reads door-choice flags after ~10 days
(verify by grepping flag consumers before implementation).
## Non-goals
No new factions, no new items, no new save codec, no UI redesign.
## Finding
Early moral choices produce flags with no long-horizon consequence. HIGH CONFIDENCE
opportunity flagged by the expansion atlas: delayed callbacks (~100 days).
## Proposed change
Data-only: author 12 callback entries in a new `moral_choice_delayed_callbacks.json`
(catalog kind: id, source_flag, delay_days_min/max, requires_state, excludes_state,
callback_kind [visitor|letter|radio|journal|rumor], target_entity, prose fields from
Part 9). Loader + integrity rules in the moral-choice loader family; dispatch through
the existing events/daily-tick seam.
## Integration steps
1. Author catalog (12 entries; reuse existing survivor/location ids only).
2. Extend the moral-choice catalog loader + integrity rules (uniqueness, flag existence,
   target existence, range checks).
3. Wire daily-tick dispatch in the moral-choice host session; RNG sub-stream
   "delayed_callback" from ISeededRng.
4. Route outcomes: journal entry, radio strip item, or relationship delta — all through
   existing owners.
5. Tests: loader gate (aggregate, per-row failures), determinism replay (two-pass),
   one cross-system consequence test.
## Files likely affected
Assets/StreamingAssets/Data/moral_choice_delayed_callbacks.json (new),
moral-choice loader/integrity files (extend), src/Host/MoralChoiceSaveStore.cs adjacent
session (extend dispatch only), focused test file (new).
## Determinism and save impact
No new save state: callbacks derive from already-persisted flags. Dispatch uses a named
RNG sub-stream; restore must not replay (dispatch-once guard keyed on flag id + day —
persisted via the existing weight_of_choices section without codec bump if the store
already carries per-flag records; otherwise bump to v3 with migration).
## Verification
--data-integrity-selftest (0 findings); --content-utilization-selftest (new catalog
consumed); focused xUnit new file; 7day smoke.
## Risk
Low — data-first, additive, no parallel authority.
## Dependencies
None decision-blocked. Coordinate ownership of the moral-choice loader family.
## Done when
A save that resolves a door choice produces the callback in the authored window,
exactly once, on both fresh and restored campaigns; all gates green.
```

### 15.2 Example prose field fill (arrival_description contract)

```text
prose_field: arrival_description
entity: loc_denial_cut (exists in expeditions/destinations — verify id)
contract: 80-140 words; one landmark; one sensory detail; one danger indication;
  no endgame spoilers; restrained bureaucratic register.
draft:
"The road stops where the cut begins. Someone has painted a meter line down the
rockface, and the paint holds better than the road did. Below, the ravine keeps its
own weather: a wet ash smell rises from it, and the sound of the wind arrives a
half-second after the wind itself. The toll post at the far lip is dark at this hour,
but the chain across the path is new. Whoever maintains the chain is owed, and
collects."
validation:
  structural: passed (id resolvable; length 96 words; fields present)
  continuity: passed (no faction named before contact; toll behavior matches
    warlord-layer encounter data)
  prose: warning — imagery overlaps nothing in current corpus (checked arrival texts
    for 5 neighboring destinations)
```

### 15.3 Example voice-signature dialogue pair (same scene, two registers)

```text
speaker: bunker_quartermaster  | register: institutional
"Line six. Coats are logged, not given. If the register says you owe two, you owe
two, and spring does not change arithmetic."

speaker: traveling_caravan_lead | register: caravan vernacular
"I don't read your book, friend. I read faces. Yours says three coats and a bad
night. I can fix one of those."
```

Both want something (compliance; a trade). Neither explains themselves. Register
difference carries characterization without narration.

---

## PART 16 — SUBSYSTEM DEEP REFERENCE (PLANNER'S OPERATIONAL MAPS)

### 16.1 Shelter operations

Rooms and identities (`shelter_rooms`, `shelter_room_identities`, `shelter_machine_identities`), thermal (`ShelterThermalSystem`, insulation catalog, shielding), schedules (`ShelterScheduleSystem`, `shelter_schedules` with emergency override), social events, decor, barter, fire, noise, espionage, prisoners, workshop, archive and atmosphere (`ShelterAtmosphereSystem`), sanitation (`SanitationSystem` with power-grid room feed via `RoomPowerProvider`), ventilation, sump drainage and flooding, airlock security with visitor types, decontamination protocols. Plan-touch rule: shelter state persists through the holdfast/shelter save family; room-level effects route through the power grid's `IsRoomPowered` seam.

### 16.2 Medical pipeline

Disease catalog and outbreaks, pathogens and strains, dose ledger and registers, ARS progression, autopsy procedures, surgical procedures and wards (staffing preflight via duty roster `ward` role, `skill_paramedic`, hazard class medical), pharmaceutical lab and recipes, microfluidic diagnostics, medical texts, therapies and trauma, sanatorium, sick lists, chemical dependency, mental health crises. Journey documented in `docs/medical/MEDICAL_PIPELINE_JOURNEY.md` and the 30-day capacity report.

### 16.3 Water, food, and agriculture

Water treatment, condensers (atmospheric, fog harvesting), deep wells, brine processing, waterborne exposure rules, dive sites and the deep coast; nutrition profiles, kitchen system, food preservation, grain processing; greenhouse, hydroponic and aeroponic crops, aquaponics, crop strains, cryo cultivars, farming, apiculture (hive logs), cellar rot and silo weevils. The food-preservation authority map governs who may add preservation content.

### 16.4 Power and industry

Power grid and subgrid nodes, SOFC (with real inventory fuel consumption since the Plan 125 seal), solar concentrators, kinetic storage, geothermal, diesel and generators; foundry (cupola, accords, treaty consequences), CVD diamond synthesis, EB/PVD coatings, precision optics and broaching, powder metallurgy, plastic pyrolysis, Fischer-Tropsch, chlor-alkali, mineral acid synthesis, bio fermentation, cellulosic ethanol, cryogenic air separation, low-background lead and metrology; workshop reverse engineering, tech salvage, robotics, armored crawlers, amphibious draisines, rail logistics and interlocks, runflat tires, hydraulic extrusion, ballistic shields, breaching equipment, mine flails. Content pattern: every industrial process has a technical catalog plus narrative assay/log corpus (Part 5.6); new processes should ship both.

### 16.5 Exploration, travel, and defense

Expeditions and destinations, vehicles and modifications and armor grades, wasteland map and damaged map zones, waystations, caravans and trade routes, maritime (vessels, currents, deep coast, ice roads), subterranean zones, excavation and hazards, archaeology, GPR/InSAR/geodetic survey, cartography, weather route gates; perimeter defenses, defense grid, sky defense ordnance and armor layers, chemical warfare defense, orbital harrow telemetry, sound ranging and acoustic triangulation, radar/ECM, direction finding, heliograph, NVIS communications. Traversal reachability questions belong to the map owner's authored edges.

### 16.6 Factions, law, and the verdict endgame

Warlord doctrines and tribute, faction war chains with journal/radio/communiqué projection, crossing arbitration, holdfast trade and factions, muster camps and witnesses, labor camps, bounty board, ledger debt with seals, regional treaties and embargoes, diplomatic summits, espionage and counter-intelligence, psyops and propaganda, infiltration, captive interrogation, wasteland laws and registries, voluntary register, census claims, standing records, verdict data and questlines, the Reckoning, epilogue chronicle and matrix. Political content must route through `FactionStanceEngine` for standing effects.

### 16.7 Progression and meta

Skills, research and knowledge, collectibles and trophies, achievements, XP expansion wave (difficulty authority, W1 in flight), difficulty presets, development traits, cohort tuning, apprenticeships, library study, cultural archive tomes, codex entries and field guide, bestiary, mod runtime, localization (L10N contract, wave-2 roadmap), settings and input rebinding. Completion history stamps difficulty preset id (schema v2).

---

## PART 17 — GLOSSARY OF HOUSE TERMS

- **Ashfall** — the game's internal project name and the ash-weather condition.
- **Capture/Restore** — the Core persistence pattern (`CaptureState()`/`RestoreState()`).
- **Codec** — a versioned save serializer for one store (e.g., holdfast v5).
- **Catalog** — one authored JSON domain file under `Assets/StreamingAssets/Data/`.
- **Destination** — an expedition dispatch target (53 authored).
- **Dose ledger** — cumulative radiation accounting per survivor.
- **Foreman / builder / sweep / integrator** — the multi-agent roles governing worktree ownership.
- **Holdfast** — the settlement-defense subsystem family and its save family.
- **Host session** — a `src/Host/` class binding a Core system to the Godot lifecycle.
- **The Machine / Reckoning** — the Day 360 tribunal endgame.
- **Seam** — an existing extension point (loader, event, port, save section, panel).
- **Selftest** — a headless CLI verification battery (`HostCli`).
- **Triad** — Setup/Save/Flush parity for stateful systems.
- **UtilityAI** — the scored-decision AI framework for NPC/survivor choices.
- **Year of Ash** — the nuclear-winter campaign era and its subsystem family.
- **Wave** — a coordinated batch of integration plans with one flagship.

---

*Compiled from repository evidence gathered 2026-09-23 from `GermanRobert-Labtester/Atomic-War-Starving-Survival` (README, AGENTS.md, TEST_POLICY.md, CHANGELOG.md, SESSION_HANDOFF.md, KNOWN_DEBT.md, docs atlas and authority maps, and the live `Assets/Ashfall.Core/`, `Assets/StreamingAssets/Data/`, `src/Host/`, and `src/UI/` trees), structured according to the layered world-bible method (constitution, canon, world model, dynamic state, writing bible, production schemas, quality gates).*

---

## PART 18 — COMPLETE NARRATIVE CORPUS INDEX (`Assets/StreamingAssets/Data/narrative/`)

Full file listing for duplication checks when authoring prose batches. Extend an existing file's family before creating a parallel catalog.

activated_carbon_adsorption_records, ammo_hoist_jam_reports, ammonia_chiller_leak_logs, annealing_lehr_birefringence_records, antler_horn_sawing_records, apiculture_red_light_audits, aramid_fiber_rot_reports, architect_vault_audits, armored_cockroach_hive_logs, armored_locomotive_manifests, artesian_well_contamination_logs, awl_saddle_stitch_journals, bark_tanning_vat_logs, beeswax_clarification_records, beeswax_rendering_dipping_assays, biochar_cation_exchange_reports, bisque_firing_records, blast_gate_mechanical_audits, blind_cave_molerat_studies, boiler_feedwater_deaerator_audits, bolting_silk_mesh_reports, bone_degreasing_prep_logs, borosilicate_sight_glass_thermal_shock, brain_tanning_hide_reports, brewers_yeast_krausen_audits, brine_pickling_barrel_spoilage, bullet_alloy_assay_reports, bunker_blueprints_codex, bunker_bureaucratic_anomalies, bunker_children_folklore, bunker_children_folklore_batch_2, bunker_contraband_barter, bunker_court_verdicts_batch_2, bunker_court_verdicts_codex, bunker_graffiti_postings, bunker_herbalism_pharmacology, bunker_maintenance_glitches, bunker_maintenance_logs_batch_2, bunker_maintenance_logs_batch_3, bunker_rituals_and_cults, bunker_shift_schedules_and_notices, bunker_trade_ledger_batch_2, bunker_wiretap_transcripts, bunker_wiretap_transcripts_batch_2, bureaucratic_document_runtime_map, bureaucratic_documents_expansion, burr_millstone_dressing_logs, calcium_hypochlorite_titration_reports, candle_dip_mould_assays, canyon_mudflow_hazard_reports, carbide_tool_wear_audits, carrion_vulture_sighting_logs, cave_aquatic_biota_logs, celluloid_film_decomposition_records, charcoal_mound_pyrolysis_logs, chef_recipe_development, chemist_lab_notes_batch_1, childrens_artwork_batch_2, childrens_folklore_expansion, chrome_alum_tanning_assays, clay_wedging_forming_logs, cobalt_arming_directives, cobalt_liturgies, cobalt_liturgies_batch_2, cold_process_soap_curing_reports, conflict_mediation_records, council_meeting_minutes, courier_dispatches_master, courier_mission_logs, courier_mission_logs_batch_2, crater_lake_limnology_records, crop_experiment_logs, crop_genome_degradation_reports, crucible_clay_pot_slag_logs, cryo_germplasm_viability_audits, cryo_seed_ampoule_logs, cryopod_failure_logs, culinary_ration_batch_2, culinary_ration_codex, cupola_melting_ratio_audits, cupola_slag_leaching_records, currents_pamphlets, currying_burnishing_assays, dead_hand_directives, deadbeat_escapement_wear_logs, deckle_mould_watermark_audits, deep_lore_texts, diplomatic_contact_records_batch_1, documents_batch_1, documents_batch_2, documents_batch_3, drone_carrier_blackboxes, drop_spindle_fibre_drafting_logs, dweller_dependency_backstories, dweller_heirlooms_master, dweller_medical_casebook, dweller_psychological_journals, education_session_records, emp_atmospheric_sniffer_logs, engineering_logs_expansion, engineering_mod_notes, equipment_failure_logs, eulogy_corpus_batch_1, expedition_briefs_expansion, expedition_field_reports, expedition_field_reports_batch_2, expedition_planning_briefs_batch_1, expedition_route_waypoint_notes_batch_2, faction_directives_and_notices, faction_field_documents, faction_texts_expansion, fallout_sensory_loss_records, fermentation_crock_airlock_assays, fibre_heckling_prep_logs, field_reports_expansion, forge_charcoal_ash_assays, found_objects_expansion, fulling_trough_nap_assays, gear_quenching_fault_logs, geological_strata_logs, geophone_hymnals, geothermal_borehole_logs, geothermal_steam_vent_diagnostics, geothermal_steam_well_logs, ghost_transmissions, graffiti_expansion, grain_silo_weevil_audits, green_sand_bentonite_assays, greenhouse_cultivation_logs, ground_glass_joint_greasing_audits, heirloom_seed_viability_reports, hemp_fiber_hackling_logs, hollander_beater_pulping_logs, honey_extractor_balance_reports, hydrophone_acoustic_logs, improvised_repair_guides_batch_2, inkle_loom_warp_tally_sheets, intake_filter_clogging_logs, invar_pendulum_thermal_expansion, iron_gall_ink_acidity_reports, iron_synod_canons, journal_entries_batch_1, journal_entries_batch_2, journal_entries_batch_3, journals_expansion, jrnl_templates_cycle_c, jrnl_templates_cycle_d, kiln_draw_trial_assays, langstroth_hive_foundation_logs, lead_crystal_scintillator_aging_logs, lead_wall_degradation_logs, leather_harness_conditioning_audits, letters_expansion, liebig_condenser_fracture_logs, lime_kiln_calcination_logs, liquid_nitrogen_compressor_failures, load_shed_schedule_001, lost_tech_manuals, mainspring_fatigue_rupture_audits, manila_hawser_breakage_reports, medical_documents_expansion, memorials_expansion, mill_dampener_tempering_assays, mortise_tenon_failure_reports, mudbrick_weathering_assays, munitions_leaching_records, mutated_botanical_logs, needle_awl_hook_assays, neoprene_gasket_degradation_logs, new_arrival_intake_interviews, night_watch_expansion, night_watch_logbook, numbers_station_ciphers, oak_bark_tanning_pit_logs, operating_theater_surgical_logs, optical_coating_rad_browning_reports, oral_lore_batch_2, oral_lore_codex, orbital_kinetic_telemetry, ozone_contact_tower_audits, patrol_debriefs, pattern_maker_shrinkage_records, periscope_prism_delamination_logs, permafrost_methane_eruption_logs, personal_effects_inventory_batch_2, pipeline_sabotage_records, plan17_discoverable_documents, pneumatic_carrier_capsule_logs, pneumatic_cylinder_leather_assays, pneumatic_tube_diverter_audits, pot_furnace_glass_melts, power_grid_management_logs, pozzolan_mortar_formulations, quest_narrative_documents, rad_pathology_autopsy_records, radiation_survey_readings_batch_2, radio_broadcast_rundowns, radio_mysteries_expansion, radio_scriptbook, radio_scripts_expansion, radio_transcripts_batch_2, radio_transcripts_batch_3, rag_pulp_beater_records, ragdoll_germination_assays, ration_fraud_records, ration_records_expansion, rawhide_bating_failure_reports, refractory_firebrick_spalling_logs, regional_treaty_protocols, relic_provenance_dossiers, retort_wood_vinegar_audits, root_cellar_humidity_rot_reports, rootes_blower_vacuum_reports, rope_break_load_assays, rope_transmission_splicing_audits, salt_mine_inscriptions, scavenger_expedition_route_notes, scraping_polishing_reports, screw_press_felt_reports, security_incident_reports_batch_2, seismic_array_fault_alarms, shelter_notices_expansion, shelter_songs_expansion, silage_lactic_pit_reports, silica_gel_seed_desiccation_audits, silo_mosquito_vector_records, slip_glaze_formulation_notes, slow_sand_schmutzdecke_logs, smoked_meat_creosote_assays, sonar_array_fault_logs, sourdough_mother_acidity_logs, square_set_shoring_audits, stalactite_mineral_assay_reports, steam_trap_water_hammer_logs, stencil_propaganda_smear_logs, strand_twisting_lay_reports, substation_transformer_fires, subterranean_zones (see catalog), sump_drainage_silt_reports, supply_audit_records, supply_audit_records_batch_2, surface_dragline_ruins, surface_radiation_topo_sheets, surgeons_casebook_batch_2, survivor_letters_lost_kin, survivor_profiles_expansion, sweet_water_glycerin_assays, tallow_rendering_vat_logs, tallow_saponification_kettle_audits, therapist_session_notes, therapist_session_notes_batch_2, therapist_session_notes_batch_3, three_strand_rope_closing_logs, timber_creosote_treatment_logs, timber_dry_rot_fruiting_records, tire_retreading_compound_logs, trade_ledgers_expansion, treadle_loom_heddle_reports, tub_sizing_gelatin_assays, turbine_blade_erosion_reports, typographic_lead_wear_logs, underground_fungi_flora, undertaker_burial_records, unsent_letters_batch_2, vault_seal_breach_logs, vinyl_record_archive, wasteland_expeditions_master, wasteland_grave_epitaphs, wasteland_grave_epitaphs_batch_2, wasteland_settlement_gazetteer, wasteland_trade_caravan_routes, wasteland_wildlife_bestiary, water_clock_orifice_silt_records, water_quality_test_reports_batch_2, weather_almanac_expansion, wick_braiding_priming_reports, wildlife_field_encounter_logs, wire_confessions, wire_rope_stranding_assays, wood_ash_lye_hydrometer_logs, world_history_expansion.

---

## PART 19 — PROMPT HEADER FOR POINTING THE PLANNER AT THIS DOCUMENT

When initiating a planning session with the master writer, prepend a header of this shape:

```text
You are the master expansion planner for ASHFALL (repo: GermanRobert-Labtester/
Atomic-War-Starving-Survival). The MASTER WORLD BIBLE & EXPANSION AUTHORITY (the
document you were given) is your primary context. Rules:
1. Verify premises against live source before drafting; the bible is a snapshot.
2. Never create a parallel system, catalog, ledger, save store, or manager; extend
   the named owner.
3. Label every claim VERIFIED / HIGH CONFIDENCE / PROPOSAL / UNKNOWN.
4. Draft plans in the Part 10 format; prose batches under Part 9 field contracts;
   house voice per Part 8.
5. One lane from Part 11 per plan; state the lane; do not exceed five concurrent
   plans on shared seams.
6. Every plan names its verification commands and acceptance criteria.
7. Respect decision-blocked items and worktree ownership.
Session objective: [state the wave charter, domain, or lane rotation here].
```

Recommended wave charter template for multi-plan campaigns:

```text
# Wave Charter — [Domain]
Domain and lane rotation: [e.g., A (narrative) + C (economy) + E (UI wiring)]
Flagship: [one bounded outcome]
Satellites: [2-6 thin-seam plans]
Verification matrix: [gates and focused tests per plan]
Non-goals: [explicit exclusions]
Decision asks: [items requiring the foreman/user signature]
```

*End of document.*
---

# VOLUME II — PRODUCTION DEEP REFERENCE (THE "BORING STUFF" THE PLANNER ALSO OWNS)

Expansion is not only prose and mechanics. A plan that adds content without knowing the gates, the generated documents, the asset rules, the localization contract, or the accessibility floors will fail integration even if it reads beautifully. Volume II gives the planner working knowledge of the entire production surface. Where a claim is a snapshot, it is dated; verify against the generated authority before use.

---

## PART 20 — CI AND VERIFICATION ARCHITECTURE (VERIFIED)

### 20.1 Canonical CI pipeline (`.github/workflows/ci.yml`)

Executed on every push and PR, in order:

1. Trailing whitespace gate — `scripts/ci/no-whitespace-churn.sh`.
2. JSON syntax and schema policy gate — `scripts/ci/json-schema-policy-gate.sh` (invalid JSON, bare array roots, missing/invalid `schema_version` all fast-fail).
3. Build Core and tests — `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.
4. Full xUnit regression gate — `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (CI/full tier).
5. Build Godot host — `dotnet build Ashfall.csproj` (0 errors).
6. Data integrity gate — `--data-integrity-selftest` (snapshot: 129 catalogs, 4,794 authored IDs, 0 errors; counts drift — the data-integrity battery reports 325/325 in later handoffs).
7. Bridge removal gate — `--bridge-selftest` (Unity shim removal notice).
8. Asset registry gate — `--asset-registry-selftest` (catalog ids resolve to real textures under `assets/`).
9. Player panels UI test — `--player-panels-uitest` (binds and renders Survivors, Medical, Weather, Radio, Shelter, Status, Tutorial, Afflictions, Radiation panels).
10. Save store and failure-path suite — `--save-load-ui-failure-selftest` (missing/corrupt/tampered saves show recoverable errors and preserve the live session), `--holdfast-save-selftest` (round-trip + tamper rejection), `--inventory-save-selftest`, `--journal-save-selftest`.
11. Deterministic campaign smoke — `--playable-shell-selftest` (multi-day loop, bunker upgrades, greenhouse planting, save/continue), `--day1-selftest` (Day 1 onboarding, needs decay, triage, fortification, radio protocols).
12. Expansions completeness — `--expansions-selftest` (expansions 01-10 plus the Verdict chain).
13. Triad drift gate — `scripts/ci/triad-drift-gate.sh` (Setup/Save/AllSaveSections parity against declarative `SaveSectionRegistry.cs`).
14. CLI catalog drift gate — `generate-cli-catalog.sh --check` (`docs/cli/HOST_CLI_COMMAND_CATALOG.md` matches live `--host-help`).
15. Save-store contract matrix gate — `generate-save-store-matrix.sh --check` (all save store classes keep checksum envelopes and slot-root isolation).
16. Compile warning baseline gate — `warning-baseline-gate.sh` (0 unexpected warnings across all targets).
17. Master docs index drift gate — `generate-docs-index.py --check` (`docs/INDEX.md` stays in sync with the docs corpus).

Later releases add: `input_map_contract` (fast), `version_gate` (fast, `version-gate.py`), `changelog_drift` (`generate_changelog.py --check`), `save_support_window` (full), `l10n` gates (`extract_l10n_inventory.py`, `l10n_drift_gate.py`), architecture-map sync, performance gate. v1.1.0 total: 57 gates (53 fast + 3 full + 1 performance).

### 20.2 Local runner discipline

- `bash scripts/ci/verify-fast.sh` runs the ordered fast tier locally, fail-fast.
- All local headless Godot verification goes through `bash scripts/ci/run-godot-bounded.sh ...`, which injects fixed/max 15 FPS and hard-stops each Godot process at 180 seconds.
- Local xUnit is targeted: `./scripts/run_test.sh <path>` with the 180-second limit; the complete xUnit gate is an explicit CI/full-tier operation (see TEST_POLICY.md).
- Clean cold build recipe: remove `.godot/mono/temp`, `Ashfall.Core/bin|obj`, `Ashfall.Core.Tests/bin|obj` before rebuilding when diagnosing flaky builds.
- Gate taxonomy: Tier 1 blocking CI gates, Tier 2 domain quality gates, Tier 3 report-only diagnostic tools (`docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md`). A plan must state which tier each of its checks belongs to.

### 20.3 What this means for drafting plans

Every plan's Verification section must name: (a) which focused xUnit files run locally and alone first; (b) which headless selftest flags it affects; (c) whether any drift gate's generated output will change (CLI catalog, save-store matrix, docs index, changelog region, KEYBOARD.md) — if so, the plan must regenerate via the owning generator, never hand-edit; (d) whether the JSON schema policy gate's rules need new entries (e.g., a new catalog file must carry `schema_version` and no bare array root).

---

## PART 21 — ACCESSIBILITY, THEME, AND UI STANDARDS (VERIFIED)

Authority: `docs/ACCESSIBILITY.md` (Plan 80 / Task B21, wired end-to-end by Plan 37); enforcement test `Ashfall.Core.Tests/UI/AccessibilitySourceAuditTests.cs`; theme tokens in `Assets/Ashfall.Core/UI/Theme.cs` verified by `ThemeSemanticTokensTests.cs`.

### 21.1 Focus policy (`AshfallFocusPolicy`, `AshfallFocusNavigator`)

1. Deterministic initial focus (`OpenWithFocus`): overlays/modals acquire focus at the primary action; the opening control is remembered (`_ashfall_focus_opener`).
2. Modal focus trap (`TrapFocus`): Tab/Shift+Tab are constrained within the active overlay; focus never leaks to background dashboard controls.
3. Visible focus rings: 2px amber border (`Theme.Hot` `#F4C875`) with translucent fill and sharp corners; contrast vs `#090B0C` background exceeds 12:1.
4. Focus restoration (`RestoreFocus`): closing via Escape, `ashfall_close`, or the close button returns focus to the initiating control; dashboard rail context is preserved.

### 21.2 Keyboard navigation model (no mouse required)

| Verb | Scope | Default | Function |
|---|---|---|---|
| `ashfall_close` / `ui_cancel` | Global/overlay | Escape | Dismiss overlay, close top modal, restore focus |
| `ui_focus_next` / `ui_focus_prev` | Active panel | Tab / Shift+Tab | Cycle interactive controls |
| `ui_up` / `ui_down` | Grids/lists | Arrows | Move selection |
| `ui_accept` / `ashfall_confirm` | Focused control | Enter / Space | Activate / commit |

The full 22-action input contract lives in `AshfallInputActions.cs` with joypad bindings (B, A, RB, Dpad, Y, Back) in `project.godot`; `KeyBindingApplicator` handles safe-mode boot, conflict detection, reset; rebinding UI sits in `SettingsPanel`; `KEYBOARD.md` is the generated key map (never hand-edit); the `input_map_contract` CI gate and 6 xUnit tests pin it. Controller analog-stick navigation runs on `AshfallFocusNavigator` with repeat cadence.

### 21.3 Color tokens and WCAG (mechanically verified)

| Token | Hex | Contrast vs Ink `#090B0C` | Standard | Usage |
|---|---|---|---|---|
| Pale | `#C7DCD0` | 13.8:1 | AAA | Primary body text, readouts |
| Hot | `#F4C875` | 12.5:1 | AAA | Highlights, focus rings, urgent telemetry |
| Warm | `#D3AA62` | 9.2:1 | AAA | Secondary highlights, section headers |
| Muted | `#938F84` | 6.2:1 | AA | Secondary text, timestamps |
| Critical | `#FF4D4D` | 6.1:1 | AA | Danger, acute radiation, zero HP |
| Dim | `#7E827A` | 5.1:1 | AA | Disabled, tertiary metadata |
| Warning | `#FF6B35` | 6.4:1 | AA | Scarcity, radiation caution, mechanical alerts |
| Success | `#7CD3A2` | 11.5:1 | AAA | Success states, healed conditions |

Planner rule: new UI states must use these tokens only; never introduce a color whose ratio is unverified, and never encode information in color alone — pair with words (the a11y gate enforces words-not-color-only).

### 21.4 Typographic floors

Enforced minimums in `Theme.cs`, audited by `AccessibilitySourceAuditTests.cs`: H1 22px title scale, H2 section headers above that floor, body text at the ShareTechMono base (14px per `DESIGN.md`), label caps 12px with 2px letter spacing. No micro-text. All headers BarlowCondensed ALL CAPS.

### 21.5 Honesty clause

`ACCESSIBILITY.md` records an explicit statement of assistive-technology limitations — screen-reader support is not claimed beyond what exists. New plans touching UI must not claim accessibility properties the runtime does not deliver; extend the statement if the surface changes.

---

## PART 22 — LOCALIZATION (L10N) CONTRACT (VERIFIED)

Authority: `docs/L10N_CONTRACT.md`; runtime owner `Ashfall.Core.Localization.LocalizationService` (stable-key lookup, English fallback); Godot adapter `src/Localization/AshfallLocalization.cs` (loads the deterministic CSV catalog, mirrors locale changes to `TranslationServer`).

- Wave 1 scope: `ResearchPanel`, `OnboardingHintPanel`, onboarding status-bar projection. Other panels remain in the inventory and roadmap (`L10N_WAVE2_ROADMAP.md`).
- Source format: UTF-8 CSV `key,en,de,source`. Keys are lowercase dot-separated semantic names. English is the fallback authority; German is the Wave-1 secondary-locale skeleton; `pseudo` is development-only.
- Missing keys return explicit fallback text or a visible key marker plus a developer diagnostic. Positional placeholders use invariant culture; CI checks placeholder-set parity for every translated row.
- Locale persists in `user://settings.json`; legacy settings default English; invalid locales recover to English.
- Gates: `extract_l10n_inventory.py` (deterministic inventory) and `l10n_drift_gate.py` (pilot references, duplicate keys, German completeness, placeholder parity, direct literal assignments in pilot files).
- String freeze D22 is decision-blocked: while pending, no plan may mass-extract strings without the named signature.

Planner rule for prose: authored JSON prose is data, not UI strings — it does not enter the CSV catalog. Only panel chrome, labels, and hints do. State in every plan whether any pilot-panel literals change.


---

## PART 23 — CODEX AND KNOWLEDGE PROJECTION CONTRACTS (VERIFIED)

### 23.1 Codex read-model (`docs/CODEX_CONTRACT.md`, Plan 74)

The Codex is a pure functional projection of settlement knowledge — it has no persistence of its own:

- Categories: Ecology, Technology, WastelandLore, SurvivalOperations, Factions.
- States: Locked, Studying, Known.
- Projection carries: `EntryId`, `Category`, `Title`, `Subtitle`, `Body`, `State`, `DayLearned`, `InformationConfidence`, `Provenance` (campaign provenance records), `RelatedLocationIds`, `Tags`.
- Built by `CodexProjectionBuilder.Build(fieldGuide, researchState, researchCatalog, journalSystem, currentDay)`.

Invariants: (1) zero persistence — no `CodexState`, no `CodexSaveStore`; (2) determinism — identical system states produce bitwise identical order and content; never iterate `Dictionary`/`HashSet` without ordinal sorting; (3) no redaction leakage — `Studying` masks mechanics and lore secrets behind placeholder text; `Locked` text is inaccessible.

Planner rule: codex content derives from `codex_entries.json`, `field_guide.json`, research knowledge, and journal state. New codex content must state which source feeds the entry and how it becomes `Known` (a discovery path that exists).

### 23.2 Campaign provenance contract

`docs/CAMPAIGN_PROVENANCE_CONTRACT.md` and `CampaignProvenanceRecord` govern where a piece of knowledge came from in this campaign (who learned it, from what source, on which day). When drafting knowledge-revealing content, specify provenance: radio intercept, document, witness, autopsy, survey. Unprovenanced knowledge is an anti-pattern.

### 23.3 Information flow

`docs/CAMPAIGN_INFORMATION_FLOW.md` and `InformationFlow/` Core define how facts travel: radio, journals, couriers, rumors, witnesses, spies. Content that reveals a fact to the player must route through one of the modeled channels. Rumors distort (kernel + noise); numbers stations and ghost transmissions are noise-dominant; surveys and assay records are evidence-grade.

---

## PART 24 — ASSETS, IMPORTS, AND THE ART PIPELINE (VERIFIED)

- `assets/` holds Godot-native art, audio, fonts, sprites, and UI resources. Images and fonts are Git LFS; runtime audio is plain binary. Every importable file must have its `.import` sidecar — the pre-commit hook enforces this.
- `src/Host/AssetRegistry.cs` is the centralized texture resolution authority with canonical fallbacks (`placeholder_survivor.png`, `icon_placeholder.png`) and procedural generation; policy in `docs/visual/FALLBACK_VISUAL_ASSETS.md`.
- The asset gate (`scripts/ci/godot-asset-gate.sh`) runs the selftest battery; `asset-orphan-sweep.sh` finds orphans; `AssetCoverageScanner`/`AssetCoverageReport` track catalog-to-texture coverage (`--asset-registry-selftest` verifies resolution).
- `tools/` contains the asset pipeline and dev utilities (`ui-preview`, manifest tools, generation scripts). `docs/ASSET_MIGRATION_LEDGER.md` records the migration from the legacy tree.
- The legacy Unity tree (`Assets/_Game/`, `Assets/UI/...`, `Assets/Resources/`, `Assets/Samples/`, `.meta`/`.asmdef` sidecars, `src/Bridge/`) is deprecated migration surface: never add code or assets there; it is in scope for removal.

Planner rule: new authored content that implies visuals (items, locations, factions) should check whether an existing texture id covers it before requesting new art; new art must enter through the registry with fallbacks. Icons are resolved by catalog id — reuse the placeholder system rather than blocking on art.

---

## PART 25 — SAVE SYSTEM DEEP REFERENCE (VERIFIED)

### 25.1 Envelope architecture

- `SaveWireContract` — the wire format contract.
- `SaveChecksum` / `SaveEnvelopeDetection` — SHA-256 checksummed envelopes; malformed current envelopes are rejected, never silently repaired.
- `SaveLoadHostSession` — the orchestration session; `SaveSlotRoot` isolates slots; `SaveStoreHub` routes stores; shared atomic writer serializes writes.
- Per-store codecs: `SaveStoreChecksumSelfTest`, `SaveStoreHub`, and the generated `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` (62 store classes at last generation) are the completeness authority.
- `SaveSectionRegistry.cs` (declarative) + triad drift gate enforce Setup/Save/AllSaveSections parity.
- `SaveSupportWindowTests` pins six codec schema versions and validates a historical fixture corpus (15 tests, full tier). Pinned versions at v1.1.0: holdfast v5, year_of_ash v5, dose_ledger v2, expansion_hub v6, expansion_quest v1, weight_of_choices v2, user_settings v2 (v1 loads with empty key bindings).

### 25.2 Failure-path contract

Missing, corrupt, and tampered saves must surface recoverable error messages and preserve the live session (`--save-load-ui-failure-selftest`). Tamper rejection is tested for holdfast (`--holdfast-save-selftest`) and checksummed for inventory and journal.

### 25.3 Adding persistence: the planner's checklist

1. Which existing save section owns this state? If none: new store requires a codec, a version, a migration, `SaveSectionRegistry` entry, matrix regeneration, and slot-root isolation.
2. Is the payload a DTO through `SystemTextJsonSerializer`? No engine types.
3. Exactly-once effects (rescue dispatch, consequences, arrivals) need persisted first-result or duplicate-guard fields.
4. Restore must not replay: event-derived side effects on restore are defects (the market-rumor restore-never-replays rule is the model).
5. Determinism: persisted RNG-sensitive decisions must persist the decision, not the seed intent.
6. Tests: round-trip, old-version migration, missing/unknown/corrupt-field tolerance, and a checksum tamper case.

---

## PART 26 — DETERMINISM, RNG, AND SIMULATION CONTRACTS (VERIFIED)

- `ISeededRng` is the only sanctioned PRNG surface; `StableHash` derives named sub-streams (e.g., the signal-authenticity stream that never touches session RNG). `Random/` Core hosts the framework.
- Banned in simulation: `System.Random`, `Guid.NewGuid()`, `DateTime.UtcNow` (see the CvdDiamondPanel batch-id fix: engine-minted `NextBatchId()`), unseeded `GetHashCode`, unordered `Dictionary`/`HashSet` iteration in projections (the codex builder sorts ordinally).
- Wall-clock concerns route through `IWallClock` / `IJsonSerializer`-adjacent ports; the host binds them.
- Determinism proof pattern: seeded harness, two passes, byte-identical outputs (Plan 76.2: 200 runs × 53 destinations). Any new simulation-bearing content should ship or extend such a harness.
- Restore discipline: restoring must reconstruct state without re-rolling; first results are persisted (anti-reroll rule from the rescue-signal authenticity evaluator).
- `SevenDayDeterministicSmokeTest` and the deterministic campaign smoke gates pin the campaign spine.

---

## PART 27 — EVENT BUS, JOURNAL, AND CONSEQUENCE ROUTING (VERIFIED)

- `IEventBus` carries state-change facts from Core systems; host adapters (`HostEventAdapter`) apply presentation and persistence effects. Panels never apply gameplay effects.
- `JournalSystem.TryAddRawEntry` is the canonical archival record sink; journal voice prose (`journal_voice_prose.json`, `jrnl_templates_cycle_c/d.json`) styles player-facing journal text; journal entries persist with ordering guarantees (`--journal-save-selftest`).
- Consequence routing pattern (from sealed debt work): territorial clashes feed `RadioHostSession.InterceptWarlordWarning`, journal records, and `SoundRangingHostSession.RecordHostileFire`; war-chain stage events journal and broadcast; expedition arrivals record location visits for visit triggers.
- `NarrativeConsequence/` maps narrative outcomes onto system state; `cascade_rules.json` governs cross-system cascade propagation; `StandingRecord` and `FactionStanceEngine` are the standing-authority sinks.
- Planner rule: a consequence that the player cannot observe through some channel (panel, journal, radio, codex, epilogue) is not integrated. Name the observation channel for every consequence in a plan.

---

## PART 28 — UTILITY AI, DUTY, AND SURVIVOR AUTONOMY (VERIFIED surface)

- `UtilityAI/` is the scored-decision framework; `utility_actions.json` is its action catalog; `UtilityAiHostSession` binds it.
- `DutyRoster/` with `duty_roles.json` (roles carry skills and hazard classes, e.g., `ward` requiring `skill_paramedic`, hazard `medical`), location/marks/quests/seasons catalogs; `DutyRosterSystem` drives shift labor; `StaffingPreflight` gates medical procedures on ward assignment.
- Skills (`skills.json`), development traits, cohort tuning and survivor cohorts govern capability; apprenticeships train the young without quota (cohort-apprenticeship seam).
- `MusterSystem` with muster camps, witnesses, faction actions, and epilogues governs settlement-scale mobilization events.
- Planner rule: when drafting NPC/survivor behavior, express new choices as scored actions in the existing framework with named inputs (needs, skills, relations, fears), not as scripted one-offs. Weights and tuning belong in data (Lane C territory) with balance documentation.


---

## PART 29 — DOCUMENTATION GOVERNANCE AND THE PLAN ARCHIVE (VERIFIED)

### 29.1 The documentation corpus

`docs/INDEX.md` is the master index (snapshot 2026-09-19: 2,545 indexed documents — 2,498 CURRENT, 45 HISTORICAL, 2 GENERATED) and is itself drift-gated (`generate-docs-index.py --check`). Root agent rulebooks (`CLAUDE.md`, `CODEX.md`, `GEMINI.md`, `QWEN.md`, `GOOSE.md`, `CRUSH.md`, `MIMOCODE.md`, `ANTIGRAVITY.md`, `VIBE.md`, `.clinerules`, `.cursorrules`, `.windsurfrules` — byte-near-identical mirrors of the agent rules) are synchronized by convention; the pre-foreman originals are archived under `docs/archive/agent-rules/2026-09-12-pre-foreman/` and are historical, not active.

Canonical-location rule: when a filename exists in several places (root, `docs/`, `deprecated_audits/`), use the canonical location listed in `docs/INDEX.md` (e.g., `AGENTS.md` at root; `Next-steps-plans/Plan_13X_*.md` in `Next-steps-plans/`, not `shipped_to_chat/`).

### 29.2 The plan archive (titles worth knowing)

The repository keeps plan drafts in several corpora: `C-integration-plans/`, `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, and per-domain `docs/plans/`. The `Next-steps-plans` series (Plans 131-141, visible in the index) names durable expansion themes the planner should treat as prior art to verify rather than re-propose blind:

- Plan 131 — Wasteland Information & Rumor Network.
- Plan 132 — Survivor Hidden Agendas & Betrayal Arcs.
- Plan 133 — Expedition Discovery & Persistent World Consequences.
- Plan 134 — Dynamic Faction Territory & Supply Lines.
- Plan 135 — Weather Deep Gameplay Cascade.
- Plan 136 — Wildlife Trapping & Food Pipeline (Cooking).
- Plan 137 — Needs Performance Cascade.
- Plan 138 — Shelter Defense & Visitor/Refugee System.
- Plan 139 — Combat & Faction Standing Bridge.
- Plan 140 — Generational Legacy & Campaign Inheritance.
- Plan 141 — Research Downstream Unlocks.

Before drafting in these areas, check the plan's implementation status against closeouts and `INTEGRATION_PLANS.md`: several of these themes are partially or fully shipped (e.g., trapping-to-cooking and weather cascades exist; graph travel, war-chain clock, and consequence reach were sealed under C2[10]-C2[13]).

### 29.3 Root-level coordination artifacts (planner-facing)

`INTEGRATION_PLANS.md` (current batch ledger), `WORKTREE_OWNERSHIP.md` (path claims), `KNOWN_DEBT.md` (debt register), `SESSION_HANDOFF.md` (latest handoff), `AI_AGENT_WORKFLOW.md` (roles), `TEST_POLICY.md`, `CHANGELOG.md` (generated region), `DESIGN.md` (UI identity), `A1_BRIEFING_DEFERRED.md` / `A1_COORDINATION_RECORD.md` / `C1_COMPLETION.md` / `WAVE9_PART1_CLOSEOUT.md` (wave records), `sources.md` (source provenance ledger), `POTENTIALCLUTTER.md` (clutter audit candidate list), `DESIGN_b64.txt` (encoded design payload). The planner should not edit these directly; they belong to the foreman/integrator, but must read them for claims and status.

### 29.4 Docs a new plan must not hand-edit

Generated or authority-owned: `docs/INDEX.md`, `docs/cli/HOST_CLI_COMMAND_CATALOG.md`, `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`, `KEYBOARD.md`, the changelog generated region, snapshot coverage docs, `docs/player_surface_manifest.json`, `docs/scenes.ownership.manifest.json`. Run the owning generator with `--check`/regenerate instead.

---

## PART 30 — PER-DOMAIN AUTHORING FIELD GUIDES (DATA SHAPES AND CONTENT GRAMMARS)

These guides define what a complete, integration-ready content unit looks like per domain. They describe required content fields and system expectations; exact JSON schemas are in the catalogs and loaders — inspect the target catalog file for the literal shape before authoring. A content unit missing any "always" field is incomplete for planning purposes.

### 30.1 Items and equipment

Files: `items.json`, `item_description_texts.json`, `item_degradation.json`, `expansion_item_tags.json`, domain item catalogs (agriculture, alloys, camouflage, chemical dependency, dose, greenhouse, thermal gear, vehicles, robotics, bionics, narcotics, tech salvage...). Always: `snake_case` id unique across all catalogs; display name; description (house voice, `item_inspection_text` register); weight/size where inventory-relevant; category tags; degradation profile if physical; crafting relation or loot/table provenance (where it comes from); consumption/equipment effect through the owning system. Reuse before creating: check `expansion_item_tags.json` for tag-based reuse, and the recipes/loot tables for existing equivalents. New item ids are a last resort (repository norm: zero new item ids in scavenging migration).

### 30.2 Locations and destinations

Files: `locations.json` (+ expansions), `micro_locations.json`, `crossing_locations.json`, `dose_locations.json`, `holdfast_locations.json`, `deep_lore_locations.json`, `damaged_map_zones.json`, `excavation_sites.json`, `dive_sites.json`, `subterranean_zones.json`, wasteland map. Always: `loc_*` id present in the locations catalog (map-orphan gate); region; location types; short description; hazards; travel/entry data where traversal-relevant; quest connections by id; state variants; prose fields per Part 9. Destinations additionally: danger tier, route distance, scavenging table binding (all 53 destinations are bound — a new destination must bind a table from `scavenging_tables.json`), encounter hooks, weather gate assignments where crossing-dependent.

### 30.3 Expeditions and encounters

Files: `expeditions.json`, `narrative_encounters*.json`, `travel_encounters.json`, `door_encounters.json`, `anomalous_expedition_encounters.json`, `scavenging_tables.json`, `micro_locations.json`, vehicles and modifications. Always for an encounter: trigger context (location/party/state), closed-choice set with consequences routed to owners (inventory, dose, standing, relations, flags), prose fields, and a determinism note (which RNG sub-stream rolls it). Encounter consequences should use the closed vocabulary pattern of the rescue system (e.g., `sender_death`, `faction_standing_loss`, `faction_ambush`) rather than ad-hoc strings.

### 30.4 Quests and moral choices

See Part 6.1 for the record contract. Additional always-fields: stage objectives reference real interactables/locations; branches carry effects on named dimensions (trust, flags, standing) and epilogue weight where applicable; abandonment behavior defined; journal/quest-log prose fields included. Moral chains must declare flag ids used and their downstream consumers (gossip, faction reactions, delayed callbacks if authored).

### 30.5 Radio

Files: `radio.json`, `radio_stations.json`, `radio_programs.json`, `radio_intercepts.json`, `radio_distress_signals*.json`, faction radio corpus and war radio, `year_of_ash_radio.json`, verdict radio, radio broadcast rundowns and transcripts (narrative). Always for a station/program: frequency and identity framing; for a distress signal: authenticity classification (genuine/trap), deadline/death-window math if genuine (first-heard + survival days), ignore consequences from the closed vocabulary, rescue mission linkage through `DistressRescueMissionManager` and the four existing catalogs; for a rumor band: deterministic start/expiry from the canonical market (`MarketRumor` kind 6 model).

### 30.6 Factions, warlords, treaties

Files per Part 4.2. Always: public/operational/hidden goals; willing/unwilling methods; resources; controlled/contested locations (ids must exist); relationship values to other factions; quest roles; dialogue rules; standing effects through `FactionStanceEngine`. Warlord content adds tribute cycles (7-day), doctrine selection, and enforcement encounters. Treaty content adds ratification prerequisites, quota effects, and consequence feeds (`RegionalTreatyFeed`).

### 30.7 Diseases, medicine, and the body

Files: `disease_catalog.json`, `pathogens.json`, `pathogen strains`, `contagion_events.json`, `autopsy_procedures.json`, `surgical_procedures.json`, `medical_texts.json`, `pharma_recipes.json`, `psychological_trauma.json`, `psychological_therapies.json`, dose registers. Always: incubation and progression windows; transmission vector (waterborne, zoonotic, contact) matching the exposure rules systems; treatment path through an existing medical pipeline step; staffing preflight if ward-based (`skill_paramedic`); prose in the casebook/therapy-note register. Dose content adds the ledger accounting (what dose source, what register).

### 30.8 Weather and seasons

Files: `weather_effects.json`, `weather_seasons.json`, `weather_route_gates.json`, `weather_hardening_upgrades.json`, storm windows, almanac expansion. Always: seasonal window; effect routing (needs, power, routes, radiation, morale) through existing owners; gate assignments with `force_stamina_cost` semantics where crossing-relevant; hardening counter-play where severe. The weather deep cascade (Plan 135 theme) is partially implemented — verify current owners before extending.

### 30.9 Research, skills, and knowledge

Files: `research_knowledge.json`, `library_manuals.json`, `skills.json`, `development_traits.json`, codex entries, field guide. Always: research prerequisites form a DAG (no cycles); unlocks target existing systems/items; library study consumption path exists; codex projection picks it up (category, state gating, provenance). Downstream unlock audit (Plan 141 theme) is prior art — check status.

### 30.10 Production and industry processes

Files: the technical catalogs (foundry production, cupola, CVD diamond, EB/PVD, powder metallurgy, Fischer-Tropsch, chlor-alkali, mineral acids, bio fermentation, cellulosic ethanol, cryogenic air separation, plastic pyrolysis, glassworks, metallurgy, workshop/pharma/relic recipes, metrology). Always for a new process: inputs and outputs as existing item ids; power/fuel consumption through the grid or SOFC seam; skill/role requirements; determinism of batch outcomes (seeded); condition/wear hooks where machinery degrades; UI surface through the owning panel; and — house pattern — a matching narrative assay/log corpus family. Economy balance runs through the Plan 76.2 harness pattern when the process mints tradeable value.

### 30.11 Collectibles, trophies, achievements, memorials

Files: `collectibles.json`, `trophies.json`, `phantom_heirlooms.json`, `phantom_triggers.json`, `memorial_rites.json`, `wasteland_grave_epitaphs*.json`, `eulogy` corpus, `final_wishes.json`. Always: discovery path (where/how found — presence is not reachability); effect dispatcher linkage for collectibles; memorial content tied to death/fate systems (`SurvivorFate`); epitaphs in the corpus register (6-20 words, restrained).

### 30.12 World evolution and ecology events

Files: `world_evolution_events.json`, `world_evolution_seeds.json`, `ecological_infestations.json`, wildlife ecosystem and migration, seasonal calendar, crop genome degradation. Always: seeding through `world_evolution_seeds` (deterministic); affected locations by id; recovery or terminal state defined; narrative projection (gazetteer, journal, radio) named.


---

## PART 31 — BALANCE METHODOLOGY (HOW NUMBERS ARE ARGUED HERE)

Balance claims require mathematics, not adjectives. The repository's established method (Plan 76.2 as the model):

1. Build or extend a seeded simulation harness over real runtime math (not approximations).
2. Run N repetitions across the content surface (e.g., 200 runs × 53 destinations).
3. Prove determinism: two passes byte-identical.
4. Compute the decision metrics: expected value, time-to-kill, income vs. expenditure, survival-day sustainability, dominance ratios (best-to-next ratio; the Denial Cut 3.5x dominance case was reviewed and accepted with documented reasons — warlord-layer encounters and narrative hooks as differentiators).
5. Flag outliers by ratio and identity loss, not by magnitude. Fix by the smallest intervention (quantity-band trim reduced a destination E[value] 216.9 → 114.4 while retaining best-ammo identity).
6. Record decisions: accepted dominance must be documented with its differentiators (the "decision record" pattern).
7. Publish the report under `docs/balance/` with the harness command.

Balance authoring vocabulary: quantity bands, renewable trade-stock (living settlements), one-time caches (supply caches), regional price factors (`ECONOMY_PRICE_FACTOR_MATRIX.md`), hardcore tuning overrides, difficulty preset scalars (the XP wave binds preset authority), cohort tuning. Time-based audits: 7-day tribute cycles, 30-day capacity and maintenance reports (medical, shelter, expedition playtests exist as formats to extend).

Prohibited balance arguments: "this seems too generous/harsh"; "players won't like X"; unrunged comparisons. State assumptions where design targets are unknown, and separate mathematical issues from design preferences.

---

## PART 32 — PERFORMANCE METHODOLOGY

- The performance gate is one of the full-tier CI checks; `PerformanceSelfTest` and `docs/perf/` hold reports. `docs/GATING_VS_DIAGNOSTIC_CHECKS.md` distinguishes blocking gates from diagnostics.
- Profile before rewrite. Candidate hotspots named by the atlas (per-frame tree searches, allocations, signal traffic, polling vs event-driven) are "potential hotspots requiring profiling" until measured.
- Measurements to use: frame time, CPU time, physics time, draw calls, object counts, allocations, signal frequency, navigation update cost, memory, loading time.
- Do not trade correctness or readability for microscopic gains; do not propose speculative scalability.
- The 15 FPS bounded Godot runner means selftests measure logic cost, not frame cost — frame-cost claims need the performance tier.

---

## PART 33 — MODS, SETTINGS, AND RUNTIME CONFIGURATION (VERIFIED surface)

- `Mods/` Core and `src/Host/ModRuntime.cs` plus `--mods` CLI family (`HostCli.Mods.cs`): the mod runtime exists; content mods must respect catalog authority and integrity validation. Plans adding moddable surface must define: what is moddable, what is validated, and what is rejected.
- Settings: `user_settings` codec v2 (key bindings field, sanitization codec, v1 loads with empty map); locale in `user://settings.json`; rebinding UI in `SettingsPanel` with conflict detection and safe-mode boot.
- Difficulty presets: `difficulty_presets.json` with scalar consumers being fully bound (CF-XP01); completion history stamps `difficultyPresetId` (schema v2).
- Export/release: `export_presets.cfg` version agreement gate; `docs/RELEASE_EXPORT.md`; hotfix path per `PROCESS.md`.

---

## PART 34 — HOST CLI SELFTEST SURFACE (THE DIAGNOSTIC LEXICON)

The headless CLI (`src/Host/HostCli.cs` + partials, cataloged in `docs/cli/HOST_CLI_COMMAND_CATALOG.md`, regenerated from `--host-help`) is the primary runtime diagnostic surface. Known verb families (snapshot; `--host-help` is truth): data-integrity, asset-registry, playable-shell, ui-layout, expedition, content-utilization, expansions, day1, bridge, holdfast-save, inventory-save, journal-save, save-load-ui-failure, 7day smoke, plans-122-125, plans-139-141, plans-162-165, plans-B86-B89, moral-choice, npc-arc, onboarding, starting-supplies, collectibles, difficulty, dynamic-world, evolving-world, expansion-depth, expedition-playtest, export-parity, faction-communique, sky-defense, vehicle-garage, wasteland-inhabitants, world-exploration, world-playtest, mods, summary, host-help.

Planner rules: (1) every undocumented flag is debt — the CLI help contract requires all flags documented in `PrintHelp` (sealed once for 13 flags; keep it that way); (2) a new selftest must register in the selftest manifest (`HostCli.SelfTestManifest.cs`) and in the CLI catalog regeneration; (3) selftests are bounded (180s, 15 FPS) — keep new ones bounded and deterministic.

---

## PART 35 — THE WRITING SUBJECT CATALOG (WHAT CODEX MAY WRITE ABOUT, ALL TYPES)

A non-exhaustive but broad menu of writable subjects, each mapped to its owner surface, so the planner can rotate prose work without drifting into unowned territory. For each: house register + owner files.

1. Item inspection texts — register: material/provenance — `item_description_texts.json`.
2. Map labels and descriptions — cartographic — location catalogs + map.
3. Arrival/revisit/changed-state descriptions — restrained perceptual — locations + state variants.
4. Journal entries — first-person house restraint — `journal_voice_prose.json`, cycle templates.
5. Codex entries — institutional-public-account — `codex_entries.json`, field guide.
6. Environmental texts and atmosphere — wear-as-history — `environmental_texts_expansion_05.json`, `AtmosphereTextSystem`, `environmental_atmosphere_expansion.json`.
7. Radio: rundowns, transcripts, scripts, intercepts, mysteries, numbers-station ciphers, ghost transmissions — signal-framing — radio family.
8. Distress signals and rescue missions — desperate-practical — distress catalogs.
9. Rumors and gossip — distorted-kernel — `moral_choice_gossip.json`, rumor networks.
10. Quest hooks, objectives, outcomes — stakes-for-persons — quest catalogs.
11. Moral choice framing and consequences — ambiguous-bureaucratic — moral choice family.
12. NPC dialogue and arcs — voice-signed — `characters.json`, `npc_arcs.json`, dialogue catalogs.
13. Survivor backstories, dependency histories, psychological journals — casebook register — dweller/therapist corpora.
14. Letters (sent/unsent), survivor letters to lost kin — intimate-institutional — letters corpora.
15. Bureaucratic documents: permits, schedules, notices, verdicts, wiretaps, ledgers — deadpan-institutional — documents batches, bunker corpora.
16. Maintenance and engineering logs, failure reports — procedure-as-poetry — engineering corpora.
17. Craft assay records (tanning, firing, brewing, rope, soap, ink, glass...) — tolerance-and-hand — narrative technical families.
18. Medical casebooks, autopsy records, therapy notes, surgical logs — clinical-restrained — medical corpora.
19. Funeralia: epitaphs, eulogies, burial records, memorial rites, final wishes — compressed-life — memorial corpora.
20. Religious and cult material: liturgies, hymnals, canons, doctrines, folk rituals — sincerely-strange — cobalt/iron synod families, `wasteland_religions.json`, `spiritual_rituals.json`.
21. Children's folklore and artwork — innocently-eerie — folklore corpora.
22. Faction directives, communiqués, war journals, propaganda — voice-per-faction — faction war family.
23. Treaties, accords, protocols, consequences — high-formal — treaty catalogs.
24. Trade: ledgers, tell lines, screen scenarios, caravan routes — transactional-vernacular — trade family.
25. Gazetteer and bestiary entries — surveyor's voice — gazetteer/bestiary.
26. Almanac and weather folklore — almanac deadpan — almanac corpora.
27. Epitaph-adjacent epilogue chronicle prose — saga-condensing — `epilogue_chronicle.json`, epilogue matrix.
28. Epilogue permutations — consequence-surveying — endings/campaign epilogues.
29. Onboarding hints and briefings — plain-teaching — onboarding catalogs, daily briefing modal.
30. UI system feedback and error strings — terse-truthful — feedback messages, save failure surfaces.
31. Achievement/trophy flavor lines — compressed-recognition — trophies/achievements.
32. Oral lore and songs — spoken-memory — oral lore codex, shelter songs.
33. Graffiti, wall carvings, found objects, personal effects — fragmentary — graffiti/carving/found-object corpora.
34. Deep lore: prewar archives, lost tech manuals, salt mine inscriptions — ruined-knowledge — deep lore family.
35. Confession and interrogation material — pressured-intimate — `confession_secrets.json`, captive interrogations, interrogation tactics.
36. Desperation events and crisis vignettes — tightened-strapped — desperation corpus.
37. Philosophy of the machine: dead-hand directives, orbital telemetry, reckoning records — cold-sublime — dead hand/orbital corpora.
38. History and world chronicle — public-vs-hidden account — `world_history.json` (+expansion).
39. World-state notifications and cascade reports — plain-consequence — feedback/cascade surfaces.
40. Localization-eligible panel prose — terse-standardized — L10N pilot files.

Rotation rule: a prose wave should draw from at least six distinct registers and declare which corpora it extends; a wave that produces only one document family is under-using the house.

---

## PART 36 — ANTI-PATTERN AND KNOWN DEFECT CLASSES (LEARN FROM SEALED DEBT)

Historical defect classes the planner must not reintroduce:

1. **Silent persistence gaps** — a system runs but nothing saves it (rescue missions had zero persistence callers once; the seal added radio save V4 + V3 migration + fingerprint). Every stateful plan asks: who captures, who restores, which codec.
2. **Unbound HOST_REQUIRED effects** — declared host effects with no binding (262 seams audited to 0 deferred in the Plan 36 port-sweep seal). New content with host effects must name the binder.
3. **Unreachable authored content** — data with no caller (ClaimPersonalBelonging found with no call sites; panels exist with unreachable projections). Always name the consumer.
4. **Invalid cross-references** — loot refs to nonexistent item ids (bandages/food_rations/copper_wire class). Integrity selftest + reference regression tests.
5. **Map orphans** — map nodes missing from the locations catalog (10 stubs authored; loader gate added).
6. **Determinism leaks** — DateTime/Guid in batch ids (CvdDiamondPanel). The batch-id pattern is now engine-minted.
7. **Restore replay** — restore re-triggering events (market-rumor restore rule is the model).
8. **Ghost test metadata** — quarantine entries referencing absent files (51 ghost Compile Remove entries reconciled; manifest gate now requires real source files).
9. **Clock contract drift** — authored minDays outside the playable window (war-chain offset 300 fix; do not re-author JSON minDays without the clock contract).
10. **Placeholder consumers** — `FuelConsumer = units => true` style stubs left in production (SOFC seal). Search for placeholder seams when planning adjacent work.
11. **Color-only information** — UI signaling without words (a11y gate).
12. **Generated-doc hand edits** — always run the owning generator.
13. **Duplicate state ownership** — local counters/caches shadowing catalog or system truth (rule 5 of AGENTS.md).
14. **Prose pin drift** — endings prose retargeted to data house-voice wording; when data wording changes, prose that quotes it must be re-pinned.

---

## PART 37 — VOLUME II PLANNER SELF-AUDIT (RUN BEFORE SUBMITTING ANY PLAN)

1. Did I verify the premise in live source/data this session (not from this snapshot)?
2. Did I name the owning system, catalog, host session, and save section for every state change?
3. Did I check `WORKTREE_OWNERSHIP.md` claims and decision-blocked items?
4. Does my data carry `schema_version`, closed-vocabulary effects, resolvable references, and a consumer?
5. Did I state determinism impact (RNG sub-stream, persisted decisions, restore behavior)?
6. Did I name gate-tier verification (fast/full/diagnostic) and the exact commands?
7. Did I check whether generated documents will drift (CLI catalog, save matrix, docs index, changelog, KEYBOARD) and plan regeneration?
8. Is my UI change a11y-compliant (tokens, focus, words-not-color), input-map registered, and L10N-aware?
9. Is my balance claim backed by the harness math with a decision record?
10. Is my prose within register, corpus-differentiated, and contracted to a Part 9 field?
11. Is the plan smaller than necessary rather than larger — one seam, one outcome?
12. Have I labeled every claim VERIFIED / HIGH CONFIDENCE / PROPOSAL / UNKNOWN?

---

*End of Volume II. Volumes I and II together form the master reference. Authority remains with live repository source; when source and this document disagree, source wins and this document must be corrected.*

---

# VOLUME III — FULL SURFACE INVENTORIES AND DRAFTING TOOLKITS

---

## PART 38 — COMPLETE UI PANEL INVENTORY (248 files, VERIFIED snapshot)

The full `src/UI/` listing. When a plan touches a subsystem, this inventory tells the planner which panels already exist. Panels follow the expose-a-command contract (Part 2, Invariant 5); scaffolding components at the end.

AchievementsPanel, AfflictionsPanel, AirlockSecurityPanel, AmphibiousDraisinePanel, AmputationTriagePanel, AnaerobicBiogasDigesterPanel, AnalogConditionGauge, AnomalyWatchPanel, ApprenticeshipPanel, AquiferTreatyConcessionPanel, ArchaeologyExcavationPanel, ArchiveDeskPanel, AshfallDashboardShell, AshfallDataGrid, AshfallFocusNavigator, AshfallFocusPolicy, AshfallMetricCard, AshfallSidebar, AshfallStatusRail, AshfallUiHelpers, AutopsyReportPanel, AviationUI, BackdropArt, BasalRadonMigrationPanel, BeliefsPanel, BestiaryPanel, BioFermentationPanel, BlackMarketPanel, BlackMarketSnapshotFixture, BlackProjectsArchivePanel, BoreholeSeismographPanel, BrineExtractionPanel, CaravanBarterLedgerPanel, CaregivingPanel, CargoAirdropPanel, CenturySeedPanel, CeremonyFestivalPanel, ChemUI, ChemWarfareDefensePanel, ChemicalDependencyPanel, ChemicalLabPanel, ChemicalReconPanel, ChroniclePanel, ClandestineInsurgencyPanel, CombatDetailPanel, CombatHistoryPanel, CombatHudOverlay, CombatPanel, CommsArrayTransceiverPanel, ConfirmationModal, ContractorRosterPanel, CraftingPanel, CrossingQuestPanel, CrossingSafeConductVouchPanel, CryogenicPermafrostCorePanel, CvdDiamondPanel, CyberneticsPanel, DailyBriefingModal, DailyBriefingModalContent, DeconAirlockPanel, DecontaminationPanel, DeepCoastPanel, DefenseGridPanel, DesperationCrisisPanel, DoseGeographyPanel, DoseLedgerPanel, DutyRosterDetailPanel, DutyRosterPanel, DynamicQuestlinePanel, EbPvdCoatingPanel, EconomyDetailPanel, EconomyMarketSnapshotFixture, ElectrostaticScrubberPanel, EmergencyResponseHud, EpiloguePanel, EquipmentConditionPanel, EventDetailPanel, EventsLogPanel, ExcavationPanel, ExpansionsHubPanel, ExpeditionCampPanel, ExpeditionPanel, ExpeditionRadarPanel, FactionCommuniqueBoardPanel, FactionDetailPanel, FactionMatrixPanel, FactionsNarrativePanel, FactionsPanel, FalloutPlumePanel, FarmingPanel, FeedbackMessages, FeedbackPanel, FireIncidentPanel, FungalProteinFermenterPanel, FungiCultivationBedPanel, GameDashboardPanel, GameHudOverlay, GameOverPanel, GeigerCalibrationPanel, GeodeticSurveyPanel, GeothermalAquiferPanel, GeothermalSteamTurbinePanel, GreenhousePanel, HeavyLogisticsAirlockPanel, HeavyMarineDieselGeneratorPanel, HydraulicExtrusionPanel, IBindablePanel, IModalPanel, InSarMappingPanel, InductionCupolaFurnacePanel, InventoryDetailPanel, InventoryPanel, IronCenotaphMemorialPanel, IsotopeSeparatorPanel, JournalDetailPanel, JournalPanel, JusticeTribunalPanel, KennelPanel, KineticStoragePanel, KitchenNutritionPanel, KitchenNutritionPanelContent, LaborUI, LibraryStudyPanel, LongWalkExpeditionPanel, LowBackgroundLeadPanel, MagneticDrumArchivePanel, MainMenuBuilder, MainMenuPanel, MapAtlasPanel, MapDetailPanel, MapPanel, MaritimeAtlasPanel, MaritimePanel, MechanicalProstheticsLathePanel, MedicalPanel, MedicalWardPanel, MentalHealthCrisisPanel, MercenaryBountyBoardPanel, MicrofluidicDiagnosticPanel, MineFlailPanel, ModalManager, MoralChoiceModal, MusterAtlasPanel, MusterPanel, MutationTreePanel, NarrativeArcModal, NurseryPanel, OnboardingHintPanel, OpeningProtocolModal, OpeningProtocolModalContent, PanelSceneLoader, PhantomMemoryPanel, PharmaLabPanel, PharmaLabPanelContent, Phase0Panel, Plans130To133Panel, Plans198To201Display, Plans74To77Panels, Plans94To97Panel, PlasmaArcSmeltingPanel, PlasticPyrolysisPanel, PoliticsUI, PowerGridPanel, PrisonerPanel, PsychologyArcPanel, QuestDetailPanel, QuestsAtlasPanel, QuestsPanel, RadiationDetailPanel, RadiationHistoryPanel, RadioIntelligencePanel, RadioPanel, RailGrindingPanel, RailwayTerminalPanel, ReconTelemetryPanel, RegionalTreatyPanel, ResearchAtlasPanel, ResearchPanel, RoboticsWorkshopPanel, RunFlatTirePanel, SafeCrackModal, SafeCrackModalContent, SanitationPanel, SaveLoadPanel, SceneBinder, SceneBindingHeadlessProbe, SettingsPanel, ShelterAtmospherePanel, ShelterBarterPanel, ShelterDecorPanel, ShelterDecorSnapshotFixture, ShelterHudPanel, ShelterPanel, ShelterSchedulePanel, ShelterSocialPanel, ShelterThermalPanel, SilentFoundryPanel, SiliconIngotSlicingPanel, SkillMatrixPanel, SkyDefenseBatteryPanel, SlurryDewateringSumpPanel, SnapshotHarness, SnapshotOrchestrator, SolidOxideFuelCellPanel, SonicRuptureDrillPanel, SoundRangingPanel, StandingRecordAtlasPanel, StandingRecordPanel, StartingCohortSetupPanel, StatusPanel, StealthReadoutPanel, SubterraneanCartographyPanel, SubterraneanDebtLedgerPanel, SubterraneanOperationsPanel, SumpFloodingPanel, SurfaceShrapnelAegisPanel, SurvivalDetailPanel, SurvivalWorkstationPanel, SurvivorDetailPanel, SurvivorDowntimePanel, SurvivorRelationsPanel, SurvivorsPanel, ThreePanePanelScaffold, TraumaBondingCohortPanel, TravelingCaravanPanel, TriangulationPanel, TroposphericRadioRelayPanel, TutorialPanel, UiBackgroundCarousel, UiNodeDiagnostics, UltrasonicDecontaminationAirlockPanel, UndergroundPrintingPressPanel, VaultDoorBreachingPanel, VehicleGaragePanel, VerdictDashboardPanel, VinylMoralePanel, WarDogKennelPanel, WaterTreatmentPanel, WaterTreatmentPanelContent, WaystationNetworkPanel, WeatherDetailPanel, WeatherForecastPanel, WeatherHistoryPanel, WeatherPanel, WeatherSondePanel, WildlifeTrappingPanel, WinterFreezePanel, WorkshopPanel.

Planner rules: (1) if a subsystem already has a panel, extend it — do not propose a second; (2) UI snapshot coverage (`SnapshotHarness`, `SnapshotOrchestrator`, snapshot fixtures like `BlackMarketSnapshotFixture`, `ShelterDecorSnapshotFixture`, `EconomyMarketSnapshotFixture`) generates golden targets at 1280×800 (`docs/ui/SNAPSHOT_COVERAGE.md`, 29 targets at last audit) — new panels need snapshot participation; (3) panel bind lifecycle is gated (`PanelBindLifecycleSelfTest`, `IBindablePanel`, `SceneBinder`, `SceneBindingHeadlessProbe`); (4) modals go through `ModalManager` over the Core `ModalStackController` and the `IModalPanel` contract; (5) three-pane scaffolding exists (`ThreePanePanelScaffold`) — reuse it rather than inventing layouts.

---

## PART 39 — HOST SESSION AND SAVE STORE PAIRING REFERENCE (VERIFIED listing)

The `src/Host/` tree pairs most host sessions with a save store of the same family. The planner's persistence question ("who captures?") is usually answered by this pairing. Representative pairs (full listing in the directory and the generated save-store matrix):

Agriculture + AgricultureSaveStore; AirlockSecurity + AirlockSecuritySaveStore; AmphibiousDraisine + AmphibiousDraisineSaveStore; Apprenticeship + ApprenticeshipSaveStore; AquaponicsSaveStore; ArchaeologySaveStore; Autopsy + AutopsySaveStore; BallisticShield + BallisticShieldSaveStore; BioFermentation + BioFermentationSaveStore; BionicsSaveStore; BlackMarket + BlackMarketSaveStore; CargoAirdrop + CargoAirdropSaveStore; CaravanSaveStore + CaravanTradeSaveStore; ChemicalDependency + ChemicalDependencySaveStore; ChemicalRecon + ChemicalReconSaveStore; ChemicalSynthesis + ChemicalSynthesisSaveStore; ChlorAlkali + ChlorAlkaliSaveStore; Combat + CombatSaveStore; Crafting + CraftingSaveStore; CvdDiamond + CvdDiamondSaveStore; DeepWell + DeepWellSaveStore; Defense + DefenseSaveStore; Disease + DiseaseSaveStore; DoseLedger + DoseLedgerSaveStore; DutyRoster + DutyRosterSaveStore; DynamicQuest + DynamicQuestSaveStore; EbPvdCoating + EbPvdSaveStore; Echo + EchoSaveStore; Economy + EconomySaveStore; Endgame + EndgameSaveStore; Events + HostEventSaveStore; Excavation + ExcavationSaveStore; ExpansionHub + ExpansionHubSaveStore; ExpansionQuest + ExpansionQuestSaveStore; FalloutSaveStore; GeodeticSurvey + GeodeticSurveySaveStore; GeothermalAquifer + GeothermalAquiferSaveStore; HydraulicExtrusion + HydraulicExtrusionSaveStore; InSarMapping + InSarMappingSaveStore; Inventory + InventorySaveStore; Journal (journal save via JournalHostSession); KineticStorage + KineticStorageSaveStore; Maritime + MaritimeSaveStore; Medical + MedicalSaveStore; MedicalWard + MedicalWardSaveStore; Memorial + MemorialSaveStore; MicrofluidicDiagnostic + MicrofluidicDiagnosticSaveStore; MineClearingFlail + MineClearingFlailSaveStore; MoralChoice + MoralChoiceSaveStore; MoraleContagion + MoraleContagionSaveStore; Muster + MusterSaveStore; PersonalQuest + PersonalQuestSaveStore; PhantomMemory + PhantomMemorySaveStore; Piezometer + PiezometerSaveStore; PlasticPyrolysis + PlasticPyrolysisSaveStore; PowerGrid + PowerGridSaveStore; PrecisionOptics + PrecisionOpticsSaveStore; Radio + RadioSaveStore + RadioStationSaveStore + RadioProgramProductionSaveStore; RailGrinding + RailGrindingSaveStore; ReconTelemetry + ReconTelemetrySaveStore; RegionalTreaty + RegionalTreatySaveStore; Research + ResearchSaveStore; RunFlatTire + RunFlatTireSaveStore; Sanitation + SanitationSaveStore; ShelterSchedule + ShelterScheduleSaveStore; ShelterAtmosphere + ShelterAtmosphereSaveStore; ShelterDecor + ShelterDecorSaveStore; ShelterFire + ShelterFireSaveStore; ShelterThermal + ShelterThermalSaveStore; SofcPower + SofcPowerSaveStore; SolarConcentrator + SolarConcentratorSaveStore; SoundRanging + SoundRangingSaveStore; Spiritual + SpiritualSaveStore; Subterranean + SubterraneanSaveStore; Survivors + SurvivorsSaveStore; SurvivorRelations + SurvivorRelationsSaveStore; Thirdonary + ThirdonarySaveStore; Waystation + WaystationSaveStore; WeatherHardening + WeatherHardeningSaveStore; WildlifeEcosystem + WildlifeEcosystemSaveStore; WildlifeTrapping + WildlifeTrappingSaveStore; World + WorldSaveStore; CampaignDay + CampaignDaySaveStore.

Selftest-bearing hosts (their own battery files): InventorySaveSelfTest, JournalSaveSelfTest, WeatherSaveSelfTest, MedicalWardSaveSelfTest, ChemicalDependencySaveSelfTest, HoldfastTradeSaveStoreSelfTest, SaveStoreChecksumSelfTest, SaveLoadUiFailureSelfTest, PanelBindLifecycleSelfTest, SceneBindingSelfTest, LoaderWiringSelfTest, NarrativeContinuitySelfTest, ContentUtilizationSelfTest, CompletionHistorySelfTest, PortContractSelfTest, PerformanceSelfTest, SevenDayDeterministicSmokeTest, ShelterAtmosphereSelfTest, ShelterDecorSelfTest, ContrabandStashSelfTest, RadioCatalogSelfTest, AssetCoverageReport.

Planner rule: if the subsystem you extend has a save store, your new state probably belongs in that store's next codec version, not in a new store. If it has a selftest file, extend that file's focused run.


---

## PART 40 — NARRATIVE SYSTEM MECHANICS (HOW STORY STATE ACTUALLY WORKS)

### 40.1 The narrative stack

- `NarrativeHostSession` binds narrative catalogs; `NarrativeQuestlineHostSession` + `NarrativeQuestlineSaveStore` persist questline progress; `ProceduralNarrativeHostSession` + save store handle generated narrative; `EchoHostSession` persists echoes; `PhantomMemoryHostSession` persists phantom memory state.
- Progression: `narrative_progression.json`, arc events (`narrative_arc_events.json`), discovery manifest (`narrative_discovery_manifest.json` — what content is discoverable and how), encounters (multiple corpora, including NPC-arcs and expansion files).
- Continuity is gated: `NarrativeContinuitySelfTest`.
- Consequence: `NarrativeConsequence/` + `NarrativeArcConsequenceAdapter` map narrative outcomes onto system state.

### 40.2 Design patterns the house uses (use these, not new ones)

1. **Closed-vocabulary consequences.** Consequence kinds are enumerated strings, validated by tests (rescue ignore-consequences; exactly-once guards). Never free-text consequence kinds.
2. **First-result persistence (anti-reroll).** Any evaluation the player could re-roll by reloading persists its first result (signal authenticity verdict).
3. **Deadline math from anchored days.** Deadlines are `anchoredDay + authoredWindow` (signal deadline from first-heard; sender death from first-heard + survival days). Use anchored-day arithmetic, never "N days from now" evaluated lazily.
4. **State gates on authored prerequisites.** Availability = requires/excludes/discovery_methods; guaranteed vs optional (location selection). The map-fog (Unknown nodes) gates visibility.
5. **Layered consequence vocabularies.** Immediate effects (inventory, stamina), mid effects (standing, flags), long effects (epilogue weight, chronicle), and echo effects (delayed callbacks). A good choice event declares which layers it touches.
6. **Provenance-tagged knowledge.** Who learned what, from which channel, on which day; feeds the codex projection with confidence.
7. **Voice-signed speakers.** Every recurring speaker has a voice block; dialogue generators must respect avoids/repeated_patterns.
8. **Public account vs hidden account.** Events carry both a wasteland belief and an evidenced truth; clues bridge them. This is the house's central epistemology — replicate it in new lore.

### 40.3 Quest pattern library (approved shapes)

- Investigation chain: clue → hypothesis stage → confrontation choice → evidence-based resolution → codex provenance update. (Model: questline master entries.)
- Moral dilemma: closed choice set; each branch effects trust/standing/flags; delayed callback seed; gossip propagation; epilogue weight.
- Deadline rescue: authentic signal; skill-driven evaluation; forced-passage risk; live-rescue vs remains branch on arrival day; exactly-once salvage. (Model: rescue runtime.)
- Debt spiral: contract terms; compounding cycles; consequence dispatcher at thresholds; bounty at default; ledger-burning epilogue escape. (Model: `LedgerDebtSystem`.)
- Census/registry drama: claim → verification → bureaucratic verdict → standing effects. (Model: `CensusClaimSystem`, verdict family.)
- Generational inheritance: survivor death → heirloom/phantom trigger → cohort memory decay → successor arc. (Model: lineage extension, phantom memory, memory decay.)
- Territorial escalation: clash event → communiqué → warlord doctrine response → tribute/treaty pressure → siege math. (Model: faction war chain.)

Each new quest should instantiate one of these shapes or propose a new one explicitly in the plan's finding section — with the reason the existing shapes do not fit.

---

## PART 41 — VOCABULARY, NAMING, AND IDIOM BANKS

### 41.1 Institutional vocabulary (safe to use)

Register, ledger, manifest, audit, assay, titration, calibration, tolerance, batch, intake, throughput, ration tier, load shed, blackout, curfew, permit, safe conduct, vouch, quota, tariff, embargo, tribute, toll, levy, muster, roster, shift, watch, standing, decree, verdict, tribunal, reckoning, census, registry, provenance, dossier, communiqué, directive, briefing, debrief, dispatch, waypoint, survey, sounding, triangulation, dosimetry, decontamination, quarantine, triage, ward, sanatorium, boiler, firebrick, crucible, cupola, slag, refractory, annealing, quench, temper, sinter, retort, condenser, zeolite, desiccant, schmutzdecke, bentonite, pozzolan, cation exchange, lye, tallow, rendered, cured, pickled, smoked, creosote, kiln, bisque, glaze, slip, deckle, watermark, nap, fulling, hackling, heckling, retting, bating, currying, burnishing, escarpment, deadbeat, mainspring, hairspring, verge, foliot.

### 41.2 Wasteland idiom (house coinages — extend in this register)

"The glass counts you" (dosimeter folklore); "spring arithmetic" (debts outlive winter); "the register remembers" (records outlive their clerks); "ash-proof" (claimed, never proven); "second winter people" (those hardened by the Year of Ash); "walk the meter line" (pay a toll); "grey hunger" (radiation appetite); "signal-fast" (rarely, and only with luck). New idioms must be coined sparingly, attached to a speaker or document, and never explained.

### 41.3 Banned vocabulary

Modern slang; internet idiom; corporate-speak ("synergy", "optimize your workflow" in-fiction); prophecy registers ("chosen", "the prophecy foretold"); generic dark-fantasy stock ("ancient evil", "the shadow rises"); real-world product, nation, or leader names; modern units where the house uses its own (use meters/liters sparingly and diegetically — gauges, not exposition).

### 41.4 Naming generator rules

Faction names: concrete-noun + governing-body noun (Iron Synod, Cobalt Directive, Ashen Court pattern → keep inventing fresh pairs). Location names: feature + institution (Verity Motel, Denial Cut, Muster Camp). Items: material + form + qualifier (`copper_wire_10m_of_10` style — terse, inventory-clerk tone). Survivor names: invented Latinate/Slavic/Germanic-flavored names without real famous-name collisions. Radio stations: frequency + persona ("The frequency that still answers"). Document titles: number + noun ("Load Shed Schedule 001", "Directive 14-B").

---

## PART 42 — DRAFTING TOOLKITS: TEMPLATES FOR EVERY OUTPUT TYPE

### 42.1 Catalog batch template (prose/data authoring)

```text
# Batch: [corpus]_[batch_N]
## Extends
[catalog files touched; why this family]
## New entries: [N]
## Register(s) used
[from Part 35's list]
## Reference integrity
[all ids referenced exist: list the checks you ran]
## Voice differentiation
[how these entries differ from sibling corpora]
## Validation
[structural / continuity / prose results block per Part 13.1]
## Gate impact
[data-integrity, content-utilization, any drift]
```

### 42.2 Feature specification template (functionality planning)

```text
# Feature: [name]
## Player-visible outcome
[What the player sees/does, in one paragraph.]
## Owning systems
[Core system(s), host session(s), save store(s), panel(s), catalog(s).]
## Inputs
[What feeds it: catalogs, player actions, events, tick.]
## Outputs and consequences
[State changes; observation channels; epilogue impact.]
## Determinism
[RNG stream, capture/restore, anti-reroll notes.]
## Failure and recovery
[What failure looks like; the recovery path.]
## Non-goals
## Verification
## Open questions
[labeled UNKNOWN, with who decides]
```

### 42.3 Balance change record template

```text
# Balance Record: [content id / system]
## Metric
[E[value], TTK, income/expenditure, sustainability, dominance ratio]
## Method
[harness, runs, seed, two-pass determinism proof]
## Before / After
## Rationale
[mathematical, not aesthetic; identity retained/changed]
## Decision
[applied trim / accepted dominance with differentiators / deferred]
## Decision owner
[foreman signature where required]
```

### 42.4 Save migration record template

```text
# Migration: [codec] v[n] -> v[n+1]
## New fields
[name, type, default for old payloads]
## Removed/renamed fields
[behavior on old payloads]
## Checksum behavior
[folded into hash? NonSerialized?]
## Migration test evidence
[fixture ids, round-trip results]
## Support window
[which historical versions still load]
```

### 42.5 UI wiring plan template

```text
# UI Wiring: [panel]
## Command exposed
[existing Core command/host method — name it]
## State rendered
[truthful current state; owner system]
## Focus/input contract
[initial focus, trap, restore, ashfall_* actions used]
## A11y
[tokens used, words-not-color, contrast note]
## Snapshots
[golden target participation]
## L10N
[pilot literals affected or not]
```

### 42.6 Documentation plan template

```text
# Doc: [title]
## Audience and job
[who reads this, what decision it enables]
## Authority checked against
[files/systems verified this session]
## Scope
## Known unknowns
[labeled, not papered over]
## Index/registration
[docs/INDEX.md regeneration plan]
```


---

## PART 43 — WORKED EXPANSION SEEDS (IDEA BANK FOR FUTURE PLAN WAVES)

Each seed states the lane, the owner seams, the shape, and the first verification question. All are PROPOSAL status — verify premises before promoting any seed into a plan. Use these to rotate subjects; do not execute more than a few per wave.

1. **Lane A — Assay culture deepening:** extend the craft-assay corpus into under-covered trades (compass making, lock making, match manufacturing, tallow-dipped fuse testing) with matching `narrative/` files and codex hooks. First question: does a trade already have a corpus family?
2. **Lane A — Numbers-station narrative arc:** a cipher corpus gains a slow-burn arc solved through the radio intelligence panel over a season. Owners: radio catalogs, radio intelligence panel, codex. First question: which closed vocabulary carries the payoff?
3. **Lane A — Bunker children generation growing up:** the folklore children become cohort adults; their childhood folklore becomes their beliefs and political positions. Owners: cohorts, belief movements, psychology arcs. First question: does the cohort system track origin-tagged members?
4. **Lane A/B — Undertaker's registry as quest source:** burial records contain discrepancies that open investigation quests. Owners: memorial systems, quests. First question: are burial records queryable at runtime or corpus-only?
5. **Lane B — Hydrophone coast mystery:** the deep-coast hydrophone logs seed a maritime encounter chain. Owners: maritime, deep coast, dive sites. First question: does the encounter loader consume hydrophone state?
6. **Lane B — Geiger calibration minigame depth:** calibration procedures as deterministic skill encounters with wear consequences. Owners: radiation systems, GeigerCalibrationPanel. First question: is the calibration flow stateful or a one-shot check?
7. **Lane C — Regional price shock cascade:** authored market shocks propagate through rumor bands and caravan restock. Owners: economy, market rumor rules, caravans. First question: which authority owns the funds legs (F13 decision-blocked — check)?
8. **Lane C — Food pipeline sustainability audit:** run the 200-run harness pattern over hunting/trapping/farming income vs. cohort consumption. Owners: trapping, farming, kitchen nutrition, balance harness. First question: what is the current survival-day sustainability curve?
9. **Lane D — Collectible discovery persistence sweep:** confirm every collectible has a persisted discovery state and a round-trip test. Owners: collectibles, CollectibleDiscoverySaveStore, effect dispatcher.
10. **Lane E — Panel truth audit:** for the newest systems, verify each panel renders current truthful state (the unreachable-projection defect class). Owners: panel bind lifecycle selftest, content utilization.
11. **Lane E — WinterFreeze/Season surface:** if any seasonal panel under-reports storm windows, surface them with words-not-color warnings. Owners: weather panels, year_of_ash storm windows.
12. **Lane G — Cross-system consequence tests:** for each cascade rule in `cascade_rules.json`, one test proving the observable channel fires. Owners: cascade rules, journal/radio/codex sinks.
13. **Lane H — Content lint for new catalogs:** a small validator enforcing Part 30 always-fields per domain, reported per-row. Owners: integrity pipeline, JSON schema policy gate.
14. **Lane H — Determinism lint:** static scan banning `DateTime.UtcNow`/`Guid.NewGuid()`/`System.Random` in `Assets/Ashfall.Core/` simulation files. Owners: CI scripts.
15. **Lane I — Save-store matrix human summary:** the generated matrix plus a curated narrative of the save families for onboarding agents. Owners: docs, generated matrix.
16. **Lane J — Difficulty preset playtest matrices:** per-preset 30-day playtest reports following the existing medical/shelter/expedition report formats. Owners: difficulty presets, playtest harnesses.
17. **Lane A — Wiretap transcripts as standing-record evidence:** transcripts feed tribunal/verdict standing inputs. Owners: standing record, verdict, bunker wiretap corpus. First question: does verdict consume document evidence generically (prewar archive pattern)?
18. **Lane B — Waystation network expansion:** living waystations with micro-economies on caravan routes. Owners: waystations, caravans, map edges (flooded-route decision pending — check).
19. **Lane A — Verdict radio theater:** the Machine's tribunal broadcasts as a radio program series with procedural case content. Owners: verdict radio, radio program production, procedural narrative. First question: which generator is deterministic and seeded?
20. **Lane B/C — Silent Foundry treaty economics:** foundry accords gain quota consequences that feed the balance harness. Owners: foundry accords/treaty consequences, economy.

Rotation reminder: a healthy wave charter draws from at least four lanes across its plans.

---

## PART 44 — THE PLANNER'S SESSION PLAYBOOK (OPERATIONAL, STEP BY STEP)

### 44.1 Cold start (first session with this document)

1. Read Parts 0-3 (behavior contract, constitution, invariants, canon) fully.
2. Skim Parts 5-6 (inventories, systems) to calibrate what exists; do not memorize — return per task.
3. Read Part 10 (plan format) and Part 11 (lanes) fully.
4. Read the current `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md` in the repository.
5. Draft the wave charter (Part 19 template).

### 44.2 Per task

1. Build a context packet (Part 0.6) naming the lane, the owner systems, and the forbidden list.
2. Verify premises in source (grep the repository; open the catalogs; never trust counts from this document for final claims).
3. Draft the specification (Part 10 or Part 42 template as fits).
4. Fill prose under Part 9 contracts in Part 8 voice.
5. Run the Part 13.2 continuity checklist and the Part 37 self-audit.
6. Submit with: plan, facts used, new facts introduced, continuity risks, verification steps.

### 44.3 Escalation and stopping rules

- Stop and ask when: authority is missing (Part 2 workflow rule 10), a decision-blocked item is touched, ownership conflicts exist, or a premise cannot be verified.
- Do not improvise architecture decisions, restore retired behavior, or create parallel authorities.
- Report blockers with the exact evidence; propose options when genuinely unresolvable.

### 44.4 Length discipline for large drafting requests

When asked for very large outputs (the 1-2 million character plan regime): structure the output as a wave charter plus plans plus appendices, not as one continuous document. Each plan remains bounded; appendices carry the reference bulk (catalogs of drafted content, prose batches, matrices). State the word budget per section in the charter. Never pad a plan to reach a length; expand the number of bounded plans and appendices instead. A plan padded past its content is a defect, not a deliverable.

---

## PART 45 — QUICK-REFERENCE CARD (PIN THIS)

- Engine: Godot 4.7.1 .NET. Core engine-free (`Ashfall.Core`, netstandard2.1). Host `src/` (net8.0). Tests xUnit (net9.0).
- Data authority: `Assets/StreamingAssets/Data/*.json`, snake_case, `schema_version`, integrity + utilization gates.
- Determinism: `ISeededRng` only; named sub-streams; persist decisions; restore never replays.
- Saves: checksummed envelopes; codec + migration + matrix; triad parity gate.
- Ownership: check `INTEGRATION_PLANS.md` + `WORKTREE_OWNERSHIP.md`; decision-blocked items need signatures.
- Verification: focused xUnit (`scripts/run_test.sh`), headless selftests via `run-godot-bounded.sh` (15 FPS, 180s), `verify-fast.sh`, 57-gate CI.
- Prose: house voice (Part 8), field contracts (Part 9), 40-subject rotation (Part 35), registers differentiated.
- Balance: harness math, decision records (Part 31).
- UI: tokens, focus policy, words-not-color, snapshots, input map, L10N pilots (Part 21-22).
- Anti-patterns: the 14 sealed defect classes (Part 36).
- Plan format: Part 10. Templates: Part 42. Seeds: Part 43.
- Fact labels: VERIFIED / HIGH CONFIDENCE / PROPOSAL / UNKNOWN — always.

---

*End of Volume III and the master reference (Volumes I-III). Compiled 2026-09-23 from live repository evidence (README, AGENTS.md, CI.md, ACCESSIBILITY.md, L10N_CONTRACT.md, CODEX_CONTRACT.md, TEST_POLICY.md, CHANGELOG.md, SESSION_HANDOFF.md, KNOWN_DEBT.md, docs/INDEX.md and the atlas/authority maps, plus the full directory trees of Assets/Ashfall.Core/, Assets/StreamingAssets/Data/ (including narrative/), src/Host/, and src/UI/). Authority remains with live repository source; when source and this document disagree, source wins and this document must be corrected.*

---

## PART 46 — SUBJECT QUESTION BANK (PROMPTS THE PLANNER CAN ANSWER, BY DOMAIN)

A bank of concrete planning questions per domain. Each is a legitimate session objective; answer with the plan/feature templates and verification named. This bank exists so expansion never runs out of subjects and always knows its owner.

### Shelter and daily life
- What does a new shelter room type need (rooms catalog, identities, thermal, power feed, schedule, save)? 
- What happens when sanitation loses power for N days (degradation ladder, UI warnings, journal)?
- How does decor affect morale, and what decor families are unrepresented?
- What crisis can originate in the kitchen, and who observes it (nutrition system → sick list → journal)?
- What new shift hazards exist for night watch, and how do duty marks record them?

### Health, trauma, and the body
- Which diseases have no treatment path, and what pharma recipe or procedure closes each?
- What does a multi-week ARS recovery arc look like across dose ledger, ward, therapy, and journal?
- Which psychological traumas lack a therapy counterpart?
- What dependency content (narcotics, chemical dependency) can originate from authored events rather than items?
- What does caregiving failure look like, and which relations does it strain?

### Expedition and the surface
- Which map regions have low destination density (a coverage question answerable from the map + destination catalog)?
- What vehicle modification families exist without armor-grade or condition hooks?
- What happens to an expedition caught by a storm window mid-route (gate math, forced passage, camp behavior)?
- Which micro-locations lack narrative encounters?
- What does a failed-but-survived expedition leave behind (world state, rumors, standing)?

### Economy, trade, and scarcity
- Which goods have no regional price entry or which regions lack coverage?
- What does a caravan carry when a settlement's specialty is X (trade specialties, tell lines)?
- Which debts in the template catalog lack consequence dispatchers at their thresholds?
- What black-market contraband categories exist without stash or bounty interplay?
- What is the 30-day water budget of a mid-game cohort under each difficulty preset?

### Factions, politics, and the endgame
- Which warlord doctrines have no authored enforcement encounter?
- Which treaty protocols lack consequence feeds?
- What muster witness testimony exists for which faction actions?
- What evidence classes does the Reckoning consume, and which authored content can enroll them?
- Which epilogue permutations are thin in chronicle prose?

### World, ecology, and time
- Which wildlife species lack seasonal calendar entries?
- What world-evolution events exist without seeds (unreachable content class)?
- What infestations have no treatment path through the medical or agriculture systems?
- What does the second winter lock actually prevent, and is every prevented action surfaced?
- Which year-of-ash events lack journal/radio projection (observation-channel audit)?

### Industry and craft
- Which technical catalogs have recipes with no power/fuel consumer?
- Which foundry products lack trade demand (economy bridge)?
- What machine wear lacks a maintenance log corpus family (Lane A companion)?
- Which workshop reverse-engineering targets exist without tech-salvage entries?

### Information and knowledge
- Which radio programs have no production pipeline (radio program production save)?
- Which codex categories are under-populated relative to catalog content?
- Which library manuals have no study consumption?
- What rumors circulate with no underlying event (kernel-less noise — allowed for noise-dominant stations only)?
- Which field guide entries never trigger?

### Player experience
- Which settings lack persistence (settings store audit)?
- What onboarding steps assume content that changed (day1 selftest drift)?
- Which tutorial hint sequences end without a success confirmation?
- What feedback messages exist for every failure path in the save UI (failure-path coverage)?

Each answered question becomes a context packet (Part 0.6) and then a plan (Part 10) in the correct lane.

---

## PART 47 — CONTENT COMPLETENESS MATRICES (HOW TO THINK ABOUT COVERAGE)

Coverage is not "more content"; it is closed loops. Use these matrices when choosing subjects.

### 47.1 The loop-closure matrix

For each domain, every authored unit should close a loop: SOURCE (where it comes from) → STATE (who tracks it) → OBSERVATION (how the player sees it) → CONSEQUENCE (what it changes) → MEMORY (how it is remembered: journal, codex, standing, epilogue). A unit missing an element is incomplete; a wave should improve closure, not merely count.

Examples of strong closure: a distress signal (radio source → mission manager state → panel strip → rescue/ignore consequences → journal + epilogue). Weak: a corpus-only document with no discovery path (no SOURCE).

### 47.2 The register-rotation matrix

Prose waves should rotate: institutional (deadpan) / technical (assay) / intimate (letters, therapy) / voice-per-faction (communiqués) / folkloric (children, songs, liturgies) / media (radio) / cartographic (gazetteer, almanac). Track which registers the last three batches drew from; do not repeat a register in consecutive waves without need.

### 47.3 The severity ladder (for consequence design)

Level 0: flavor (no state change). Level 1: local state (one survivor, one room). Level 2: system state (power grid, standing tier). Level 3: cross-system cascade (cascade rules). Level 4: world state (map, territory, evolution). Level 5: saga state (epilogue weight, reckoning evidence). A wave should author consequences at multiple levels and say which.

### 47.4 The time-spread matrix

New pressure should spread across the temporal atlas (Part 7 / Vol. I): some same-day, some weekly, some seasonal, some saga. A wave of only same-day content flattens pacing; the mid-winter slump (Days 90-180) and the pre-reckoning run-in are the current documented gaps.

---

## PART 48 — FINAL INTEGRITY STATEMENT FOR THE PLANNER

This document is a map, not the territory. Its inventories are snapshots dated 2026-09-23; the repository integrates continuously and counts drift daily. The planner who treats this document as a substitute for reading source will eventually violate its own rules. The correct usage pattern is: use this document to know what to ask and where to look; use the repository to know what is true; use the templates to draft; use the gates to prove.

When this document is found wrong, correct it through the same closeout discipline the repository uses: a change record (Part 13.4), the affected parts named, and the evidence cited. The master bible that cannot be corrected is itself an anti-pattern.

*End of the question bank, matrices, and final statement. Volume III closes here.*