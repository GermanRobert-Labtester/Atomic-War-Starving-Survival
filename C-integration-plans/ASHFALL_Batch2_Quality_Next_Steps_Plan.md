# ASHFALL Batch 2 — Quality Implementation Plan

## Summary

Deliver roadmap items 17–32 as dependency-ordered Godot/Core vertical slices building on Batch 1. Existing Core systems and UI scaffolds are reused and upgraded from demo/fixture paths into campaign-safe flows; missing authorities are added only where no authoritative system exists.

Batch 2 remains Godot-only. Core owns simulation, state, deterministic outcomes, and save DTOs. Godot hosts input, presentation, audio, and wiring. Unity remains read-only.

The Batch 1 contracts are prerequisites: `CampaignDayCoordinator`, the modal gate, atomic inventory commands, checksummed saves, canonical ID validation, and the shared event/notification bridge.

## Shared foundation before feature slices

### 1. Establish one action and transaction boundary

New player commands should expose a typed result containing success, stable failure code, player-facing message key, resource/state deltas, and an event identifier. Existing string-returning host methods remain compatibility wrappers while live UI moves to typed results.

Every action must validate before mutating. Inventory consumption, equipment reservation, research progress, resource production, and reward delivery must commit atomically. Repeated commands, reloads, and modal reopenings must not duplicate rewards or consume inputs twice.

### 2. Make catalogs authoritative and data-driven

- Move `ResearchSystem.RegisterDefaults()` knowledge definitions into a versioned research catalog; preserve the existing IDs and prerequisite graph.
- Make `relic_recipes.json` the single workshop relic authority. Wrap or migrate its current array format to a versioned envelope and update its loader/tests.
- Extend existing `recipes.json`, `chemical_dependency_items.json`, `dive_sites.json`, `narrative/vinyl_record_archive.json`, `narrative/wasteland_wildlife_bestiary.json`, `foundry_accords.json`, and orbital telemetry catalogs rather than creating duplicate definitions.
- Add new versioned catalogs only where no authority exists: water treatment, vehicles, airlock rules, social conflicts, excavation sites, and weather-station configuration.
- Register all new references with `CatalogIntegrityValidator`; use canonical snake_case IDs and exact faction/room/item lookups.
- Preserve all current working-tree JSON edits and merge new fields additively.

### 3. Extend the campaign-day order

`CampaignDayCoordinator` becomes the only owner of daily progression. Its explicit order is:

1. Weather forecast/current conditions, orbital warnings/impacts, brine source, and power.
2. Ventilation, foundry, water treatment, greenhouse, excavation, trapping, and other production jobs.
3. Needs, medical, disease, ration conflict, caregiving, social dynamics, and generational time.
4. Research, expedition/vehicle travel, maritime operations, caravans, treaties, and faction effects.
5. Death/memorial processing, audio context refresh, journal notifications, and the daily briefing.

No subsystem may call another subsystem’s daily tick. Real-time maritime interactions use explicit elapsed seconds and are suspended by the same modal/interaction gate.

### 4. Persistence and compatibility

Every new or extended stateful system implements `CaptureState/RestoreState`, defensive deep-copy behavior, deterministic ordering, and checksum-protected persistence. Older saves load with empty/default sections; future versions fail clearly; malformed new envelopes are not treated as legacy saves.

Use `IJsonSerializer`, `IFileIO`, `ISeededRng`, and the existing save codecs. Do not introduce `JsonUtility`, `System.Random`, `Guid.NewGuid()`, Unity references, or gameplay logic in `Assets/_Game/`.

## Dependency-ordered feature delivery

### Phase 1 — Resource, research, health, and shelter infrastructure

#### [17] Workshop and relic reverse-engineering bench

Create a Core `WorkshopReverseEngineeringSystem` that composes `ResearchSystem`, `CraftingSystem`, shared inventory, survivor skills, and the migrated relic catalog.

- State: selected relic, researcher, required tools, reserved components, work phase, progress hours, dismantle yields, repair result, and unlock event.
- Support examine, dismantle, repair, and research actions with cancellation/refund rules defined in data.
- Use the existing `ResearchSystem` for prerequisite/progress/completion authority; completing a relic never edits recipes directly from UI code.
- Replace hard-coded research defaults and fixture-backed detail views with live catalog data in `ResearchAtlasPanel`, `ResearchDetailPanel`, `CraftingPanel`, and `CraftingDetailPanel`.
- Add `WorkshopHostSession` and route actions through the campaign inventory/research state.
- Acceptance: a damaged relic, valid tools, eligible survivor, and required scrap advance one reproducible job and unlock the expected canonical recipe after completion.

#### [19] Water filtration, distillation, and toxic decontamination

Create Core `WaterTreatmentSystem` rather than expanding `BrineWaterSystem` with unrelated tank logic. `BrineWaterSystem` remains the source/membrane/treaty adapter; the new system owns bunker water inventories and treatment jobs.

- Track raw, brackish, irradiated, and clean water quantities; charcoal/filter integrity; distillation fuel; treatment mode; contamination profile; and active jobs.
- Consume clean water first during the daily ration pass. If clean water is exhausted, consume raw/contaminated water according to ration policy; if no water exists, apply thirst normally. Never create free water.
- Emit disease/heavy-metal/radiation exposure events into existing `DiseaseSystem`, `NeedsSystem`, and medical/dose pipelines.
- Apply `District8Accords`/regional treaty quotas through existing treaty definitions, not hard-coded water bonuses.
- Build `WaterTreatmentPanel` with tanks, filter wear, fuel burn, queue/progress, and contamination warnings.
- Acceptance: treatment obeys mass balance, depleted clean water produces the documented raw-water exposure path, and save/load preserves tanks and jobs.

#### [30] Ventilation, smoke scrubbing, and radon hazards

Extend the existing `StartingLevelSystem` air state—the current owner of filter integrity, air quality, and radon display—instead of creating a second air-quality meter.

- Add source emissions for Silent Foundry, generator, cooking, and other catalog-defined industrial jobs.
- Track smoke/soot, carbon monoxide, duct/valve state, exhaust-filter saturation, and shelter-room exposure.
- Use power allocation and room assignments from Batch 1 to determine which ventilation branches operate.
- Feed harmful exposure into survivor health/medical status and retain `YearOfAshRadonSystem` as the authoritative radon phase system.
- Add duct/airflow visualization to `ShelterOperationsPanel` and service/toggle commands through a host adapter.
- Acceptance: running a smoke-producing system without an operational exhaust path raises a warning and survivor exposure; servicing the correct filter clears the hazard without resetting unrelated radon state.

#### [22] Chemical synthesis and pharmaceutical compounding

Create Core `PharmaLabSystem` as a domain layer over `CraftingSystem`, `RecipeCatalog`, `ChemicalDependencySystem`, and shared inventory.

- Define reagent sets, station requirements, heating/distillation phases, purity bands, output quantities, and dependency/addiction risk in versioned data.
- Reserve inputs at job start, release/refund them on valid cancellation, and deliver outputs only once on completion.
- Use `ISeededRng` for any purity or contamination roll; expose the deterministic result in the action result and save state.
- Apply dependency risk through `ChemicalDependencySystem`; do not duplicate withdrawal or crafting penalties.
- Build `PharmaLabPanel` with reagent slots, heat state, purity, risk, and completion status.
- Acceptance: a valid pine-resin/alcohol/sulfur recipe—or its canonical catalog equivalent—produces the authored medical item count and records dependency risk exactly once.

#### [31] Meteorological forecast station

Add a small `WeatherStationSystem` for installation/calibration/unlock state and use `WeatherSystem.PeekForecast` as the sole future-weather authority.

- Forecast horizon and accuracy are catalog-defined; the station must not reveal unavailable future states.
- Produce deterministic one-to-three-day entries with confidence and route-safety flags.
- Feed expedition launch validation, map warnings, greenhouse lockdown, and recall prompts through host adapters.
- Extend `WeatherForecastPanel`/`GameHudOverlay` with barometer, trajectory, confidence, and actionable departure status.
- Acceptance: the same campaign seed and station state produce the same forecast, and a forecast can block or warn an expedition without changing the actual weather roll.

#### [32] Dynamic multi-bus audio and soundscape layering

Extend the existing host-only `AudioManager`; Core must emit conditions, never touch `AudioServer`.

- Add dedicated buses for generator, ventilation, radio, medical, surface weather, and ambient churn while preserving current Master/Music/Ambience/SFX/UI/Voice/Alerts settings.
- Add runtime audio context inputs for shelter depth, power load, ventilation state, weather severity, radio lock, medical activity, and common-room playback.
- Configure low-pass, reverb, distortion, volume, and pitch modulation with bounded curves and safe defaults.
- Route state changes through `AudioEventBridge`; do not have individual UI panels manipulate buses directly.
- Extend `AudioCueCatalog` with stable cue IDs and existing-resource fallbacks; do not add unlicensed real-world recordings.
- Add audio self-tests for bus creation, effect setup, mute/headless behavior, missing resources, and settings preservation.
- Acceptance: moving between shelter contexts and changing environmental conditions changes the intended layers without leaking players or overriding user volume settings.

### Phase 2 — Maritime, mobility, orbital, excavation, and hunting loops

#### [18] Maritime stealth dive console and sonar interaction

Promote the existing `StealthDiveInstance`, `ProceduralScavengeSystem`, and `PsychologicalContaminationSystem` from demo methods to production commands through `MaritimeHostSession`/`DeepCoastHostSession`.

- Add explicit start, crank, timed tick, room entry, search-progress, winch/retrieve, abort, and surface actions.
- Store room search progress and looted state; prevent a second reward from a searched room.
- Keep air depletion and noise state authoritative in Core. Noise ≥80 marks the dive compromised and triggers a deterministic hazard resolution at the next authored hazard point.
- Resolve loot through the existing procedural scavenge system and apply contamination through the psychological system.
- Replace fixture action rows in `MaritimeAtlasPanel` with real controls, air telemetry, noise/sonar display, room state, and winch results.
- Acceptance: an operator can sustain air, move through rooms, retrieve one deterministic loot result, and surface or fail without frame-rate-dependent simulation.

#### [23] Expedition vehicles and armored sled maintenance

Create Core `ExpeditionVehicleSystem` and a versioned vehicle catalog.

- Track owned vehicle condition, fuel, attachments, cargo capacity, speed multiplier, terrain compatibility, and breakdown state.
- Snapshot the selected vehicle into an expedition at launch; apply fuel consumption, wear, travel-time modifiers, and breakdown checks in `ExpeditionSystem` through a travel modifier interface.
- Define the example armored-sled values in data, not code; values such as triple capacity, 40% shorter travel, and five diesel are catalog tuning, not universal rules.
- On breakdown, return a typed outcome that preserves carried cargo and applies the authored delay/repair path.
- Build `VehicleBayPanel` for repair, outfitting, fuel, and expedition assignment.
- Acceptance: vehicle modifiers affect a real expedition, fuel is consumed once, breakdowns are deterministic, and an active vehicle survives save/load.

#### [24] Orbital Harrow telemetry and impact defense

Extend the existing `OrbitalHarrowSystem` and `SkyLayerArmorSystem`; do not create a second orbital scheduler.

- Add authored telemetry reveal timing, impact forecast state, target grid, brace requirements, and player reinforcement action.
- Preserve the existing day-based schedule and impact history while adding a warning window before `TickDay` applies the impact.
- `Brace` must consume or reserve canonical structural materials and modify the existing armor/mitigation path; no UI-only damage reduction.
- Add `OrbitalHarrowPanel` with orbit/trajectory display, target grid, countdown, telemetry text, braces, roof condition, and impact history.
- Acceptance: telemetry becomes available at the authored lead time, a valid brace mitigates the correct impact, an unbraced impact uses the existing armor calculation, and repeated clicks do not duplicate materials or mitigation.

#### [27] Subterranean excavation and rubble clearing

Create Core `ExcavationSystem` tied to existing material shielding, room blueprint IDs, inventory, work assignments, and the Batch 1 shelter diorama.

- Track buried sector nodes, crew, tools, shoring/explosive requirements, progress, structural risk, cave-in outcome, and discovered caches.
- Use deterministic risk resolution from authored site conditions and crew capabilities.
- Unlock room construction slots through the existing blueprint/room authority, not by directly editing UI lists.
- Add excavation overlay controls to `ShelterOperationsPanel` with progress, risk, crew, materials, and discovery results.
- Acceptance: two eligible workers advance a valid site over three campaign days, risk remains visible, and completion unlocks exactly the authored room slots.

#### [28] Wildlife hunting, trapping, and toxin removal

Create Core `WildlifeTrappingSystem` using the existing wildlife bestiary and procedural RNG conventions.

- Track trap sites, bait, assigned hunter, check schedule, weather/radiation modifiers, catch state, carcass yield, toxic glands, and meat treatment.
- Use catalog-defined species and canonical item IDs; no invented wildlife IDs in UI code.
- Apply hunter skill and exposure consequences through existing survivor/radiation/medical ports.
- Separate catch, butchery, toxin removal, curing, and consumption so unsafe meat cannot silently become clean food.
- Build `HuntingTrapPanel` for perimeter sites, bait, catch tables, butchery checks, and contamination warnings.
- Acceptance: a trap produces a deterministic carcass result, skilled butchery separates edible meat from radioactive glands, and failed handling records the correct exposure.

### Phase 3 — Security, social stability, and diplomacy

#### [20] Airlock sentry post and infiltration defense

Create Core `AirlockSecuritySystem` and a thin `AirlockSecurityHostSession`.

- Track blast-door integrity, airlock state, sentry shift assignment, alertness, visitor identity/type, quarantine status, and active incident.
- Reuse `DutyRosterSystem` for staffing and `NarrativeEncounterSystem` for visitor choices.
- Admit, inspect/quarantine, and defend actions must be explicit authored choices. A violent incident emits the Batch 1 `CombatStartRequest` rather than starting UI demo combat.
- Apply door damage, safety, faction standing, contamination, and visitor outcomes in Core.
- Build `AirlockSecurityPanel` with intercom/visitor feed, integrity, alertness, shift slots, and decision modal.
- Acceptance: a visitor incident pauses normal interaction, one decision resolves it, door/safety/faction state updates, and reloading the incident cannot resolve it twice.

#### [21] Survivor social dynamics and conflict arbitration

Create Core `SurvivorRelationsSystem` as the missing umbrella over `RationConflictSystem`, `CaregivingSystem`, `SomaticFlashbackSystem`, `UtilityAiSystem`, and `CrossingArbitrationSystem`.

- Store stable pairwise relationship state: affinity, trust, resentment, grief, bond type, recent causes, and pending conflict.
- Consume existing system events rather than recalculating ration fairness, caregiving bonds, or flashback rules.
- Use `UtilityAiSystem` only to score authored response choices; it must not become a second simulation authority.
- Load conflict and mediation choices from JSON. Support mediation, apology/resource settlement, discipline, and refusal outcomes with explicit morale/standing effects.
- Keep romance or other sensitive relationship content authored and gated; do not generate unbounded relationships from random pairings.
- Add social-tension status cards and a blocking mediation modal; expose relationship detail in the survivor dossier.
- Acceptance: sustained authored stress can produce one dispute, mediation changes the relationship and morale state deterministically, and unresolved conflicts persist through save/load.

#### [29] District 8 treaty and diplomacy desk

Create Core `RegionalTreatySystem` over the existing `RegionalTreatyCatalog`, `foundry_accords.json`, `foundry_treaty_consequences.json`, `VouchAccessSystem`, economy, water, and map/travel adapters.

- Track proposed, ratified, active, violated, suspended, and expired treaty state with dates and compliance metrics.
- Use exact canonical faction IDs; do not use substring matching for treaty effects.
- Implement proposal requirements, ratification costs/conditions, compliance ticks, violation penalties, and termination rules in data.
- Apply Road Iron effects to economy discounts and protected map routes only after ratification; apply water/power/labor effects through their actual systems.
- Build `TreatyDeskPanel` and extend faction narrative UI with article text, quotas, compliance, benefits, and penalties.
- Acceptance: signing the canonical Road Iron Charter updates economy and route availability, a violation removes or suspends those effects, and the treaty state is saved.

### Phase 4 — Long-horizon continuity and morale

#### [25] Generational succession and family lineage

Extend `GenerationalSuccessionEngine` and `CensusClaimSystem`; do not create a parallel age clock.

- Add parent/child or guardian links, lineage records, inherited trait/skill snapshots, retirement status, successor nomination, and inauguration state.
- Preserve the existing 365-day chapter model and synchronize deaths with the roster/memorial pipeline.
- Make births, adoption, and succession explicit authored commands/events; do not spawn unsourced survivors automatically.
- Transfer only canonical traits/skills permitted by data and record the transfer for journal/history.
- Replace fixture lineage content in `CenturySeedPanel` with a live pedigree/tree view, elder retirement, successor, and inheritance details.
- Acceptance: an eligible founder can retire, an eligible apprentice can be inaugurated, inherited traits persist, and the lineage graph round-trips without duplicate records.

#### [26] Common-room vinyl and morale buffer

Create Core `VinylMoraleSystem` for archive ownership, current playback, daily application, and one-time-per-day accounting. Keep actual audio playback in `AudioManager` through a `VinylHostSession`.

- Use the existing vinyl archive as content authority, but add stable audio cue/resource IDs rather than treating prose texture descriptions as playable streams.
- Map records to original/cleared in-game audio assets or abstract needle/static cues. Do not add copyrighted real-world recordings or introduce real people/country references that violate project content rules; sanitize legacy display content before exposing it in the new UI.
- Apply daily morale and flashback suppression through explicit host hooks into the existing morale/Phase-0 flashback pipeline; never directly edit survivor stats from UI.
- Prevent repeated playback from stacking the daily bonus; define interruption, replacement, and common-room occupancy behavior.
- Build `CommonRoomPanel` with record collection, turntable state, needle-drop, playback, broadcast relay, and morale effect history.
- Acceptance: one owned record plays through the audio service, applies its authored daily effect once, reduces the configured flashback probability/penalty, and persists current/last-played state.

## Primary implementation boundaries

Core additions/extensions belong under `Assets/Ashfall.Core/`:

- New authorities: workshop reverse engineering, water treatment, pharma lab, vehicle, airlock security, survivor relations, excavation, wildlife trapping, regional treaties, and weather station.
- Extended authorities: research/catalog loading, maritime dive actions, orbital telemetry/brace state, generational lineage, starting-level air quality, and vinyl morale state.
- New/extended DTOs and tests for every stateful system.

Godot host additions/extensions belong under `src/Host/`, `src/Main*.cs`, and `src/Audio/`:

- Production host sessions replacing demo wrappers.
- Campaign-day wiring, inventory/resource adapters, event bridges, save stores, and audio context routing.
- No simulation formulas in panels or `AudioManager`.

Godot UI additions/extensions belong under `src/UI/` and existing world views:

- `WaterTreatmentPanel`, `AirlockSecurityPanel`, `PharmaLabPanel`, `VehicleBayPanel`, `OrbitalHarrowPanel`, `HuntingTrapPanel`, `TreatyDeskPanel`, `CommonRoomPanel`, and the excavation/ventilation overlays.
- Existing research, crafting, maritime, weather, century-seed, shelter, faction, and survivor panels are upgraded from fixture/action-row displays to live bound views.

## Verification, review, and rollout gates

For every slice:

- Add Core tests for happy path, invalid inputs, boundary values, deterministic repeatability, and save/load.
- Add host tests for atomic inventory/resource transactions, event order, expansion gating, and duplicate-action prevention.
- Add `HostCli` headless gates for each new domain and a combined `--batch2-selftest` aggregator with named subreports.
- Add `SnapshotHarness` coverage for new/changed panels. Fixtures remain valid only for unbound snapshot tests.
- For every new system with two or more coupled state variables, perform an independent diff review using a separate review/testing tool before acceptance.

Run the canonical project gate after each accepted slice:

1. `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
2. `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
3. `dotnet build Ashfall.csproj`
4. `godot --headless --path . -- --data-integrity-selftest`
5. `godot --headless --path . -- --bridge-selftest`

The final Batch 2 acceptance pass must additionally verify:

- Batch 1 daily progression still ticks every system once.
- Old saves load without fabricated inventory, survivors, treaties, vehicles, or relationships.
- Missing audio/resources degrade safely and never affect simulation outcomes.
- Expansion 09/11/12 features remain correctly gated.
- Real-world names, countries, wars, and copyrighted recordings are not introduced into new content or UI.
- Each accepted vertical slice is committed separately; no all-at-once Batch 2 commit.

## Assumptions and defaults

- Batch 2 uses the previously selected dependency-first vertical-slice strategy.
- Existing Core systems are authoritative wherever they already own the relevant state; new systems are orchestration authorities only for genuinely missing domains.
- Existing demo methods remain available for headless regression tests but are not used by live campaign UI.
- Existing modified JSON files are user-owned and must not be overwritten.
- No new external art or commercial audio is required; use Godot-native assets, original/cleared audio, and `AssetRegistry`.
- Content examples in the roadmap are treated as intent, not literal identifiers or licensed source material; all shipped IDs and text must pass ASHFALL data/content validation.
