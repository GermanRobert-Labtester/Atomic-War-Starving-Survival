# ASHFALL — AI Skill Context

> **CRITICAL ARCHITECTURE DIRECTIVE**: Godot is authoritative. Unity is deprecated legacy architecture. Never introduce or restore Unity dependencies. Existing Unity references are migration artifacts to remove, port, or isolate—not architectural guidance.


**Repository dossier generated:** 2026-08-16
**Repository examined:** `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`
**Purpose:** high-density context for future prompt optimization, game-design analysis, implementation planning, debugging, expansion design, content creation, and repository auditing.

This is an evidence-based intelligence dossier, not a README. It describes the implementation that was observable in the repository at the analysis snapshot and flags stale plans, uncertain integrations, and inferred design intent. Repository implementation outranks prose documentation when the two disagree.

## Evidence discipline

- **OBSERVED** — directly supported by source, data, configuration, test files, or a reproducible static count.
- **INFERRED** — a reasonable conclusion from several observed signals, but not stated as canonical implementation fact.
- **UNCERTAIN** — evidence is incomplete, contradictory, or requires runtime verification that was not performed.
- **RECOMMENDED CONTEXT NOTE** — useful guidance for a future prompt, not an established game fact.
- Status values use `IMPLEMENTED`, `PARTIALLY IMPLEMENTED`, `PLANNED`, `STUB`, `BROKEN/SUSPECTED`, `DEPRECATED`, `UNUSED`, or `UNKNOWN`.

The active Godot host and Core source were not launched during this documentation pass. Unity was deliberately not invoked because the repository instructions prohibit it unless the user explicitly requests it. Historical test results and static inspection are therefore not current test proof.

## 1. Executive Context

ASHFALL is an original 2D post-nuclear survival-management and narrative strategy RPG. The player manages a shelter and a group of survivors through hunger, thirst, fatigue, warmth, morale, health, radiation, illness, environmental hazards, scarcity, expeditions, records, faction pressure, and morally costly decisions. The intended experience is restrained and human: survival is a sequence of trade-offs, maintenance tasks, records, damaged relationships, and consequences rather than a power fantasy.

The repository is in a dual-engine strangler migration. The large Unity 6 LTS codebase is legacy, while Godot 4.7+ with C#/.NET is the active migration target. `Assets/Ashfall.Core` is intended to be the engine-agnostic source of simulation truth. `src` is the Godot host. `Assets/_Game` is the Unity-era implementation and still contains much more gameplay, UI, and wiring than the Godot host. JSON under `Assets/StreamingAssets/Data` is the content authority; Unity ScriptableObjects are convenience/import artifacts, not a second design authority.

### Snapshot of scale

| Area | Observed snapshot | Interpretation |
| --- | ---: | --- |
| `Assets/Ashfall.Core` | 234 C# files / ~43,690 lines | Active plain-C# migration surface; many systems and catalogs exist. |
| `src` | 84 C# files / ~19,713 lines | Godot host, UI, save stores, bridge, and integration sessions. |
| `Assets/_Game` | 1,337 C# files / ~233,307 lines | Large Unity legacy implementation and editor/content wiring. |
| `Ashfall.Core.Tests` | 143 top-level C# files / ~1,492 xUnit attributes | Broad static test surface; current full-suite result was not rerun. |
| `Assets/StreamingAssets/Data` | 82 top-level JSON + 196 narrative JSON | Main data/catalog surface; 280 JSON files observed. |
| `generated_AIassets` | 248 files, including ~226 images | Generated/approved visual asset staging and proofs. |
| `Assets/Resources/Art/Items` | 523 image files | Legacy/runtime item-art pool; only a subset maps exactly to current item IDs. |

The repository has substantial implemented depth, but it is asymmetrical: the Unity legacy host has the broadest assembled game, Core contains an expanding set of ports and new systems, and Godot currently presents a developer/integration hub plus several real host slices rather than full visual/gameplay parity.

## 2. Game Identity

### Observable identity

- **Name:** `ASHFALL: Atomic War - Starving Survival`; working title ASHFALL.
- **Genre:** 2D post-nuclear survival-management / strategy RPG with shelter management, exploration, procedural or data-driven encounters, faction pressure, and narrative choice.
- **Player role:** shelter or bunker decision-maker responsible for allocating scarce resources and preserving a survivor community. Exact diegetic title is not consistently fixed.
- **Setting:** a fictional post-exchange region of bunkers, upland settlements, industrial sites, frozen waterways, military remnants, civic archives, and damaged infrastructure.
- **Core pressures:** food, water, fuel, heat, radiation, illness, injuries, morale, shelter degradation, uncertain information, faction demands, and time.
- **Target emotional register:** cold, exhausted, human, restrained, materially specific, and morally uncomfortable.

### Design pillars

- **OBSERVED:** the project instructions explicitly require scarcity, radiation, shelter degradation, deterministic simulation, data-driven content, cross-host save compatibility, thin presentation hosts, Utility AI, and restrained post-disaster tone.
- **INFERRED:** the game’s strongest design pillars are survival through maintenance, administration as narrative, resource decay as pressure, and systems that create stories through constrained choices.
- **INFERRED:** records, ledgers, radio fragments, letters, manifests, census work, and procedural paperwork are not just flavor; they form a recurring narrative language.
- **RECOMMENDED CONTEXT NOTE:** new content should make the player choose what to preserve, who receives scarce capacity, or which truth becomes actionable. Additions that only increase combat power or loot quantity will likely conflict with the project identity.

The project explicitly forbids copying existing games’ names, art, characters, UI, text, or code. It also forbids magic/fantasy, glorified violence, and ungrounded real-country/real-war/real-person framing. Some current content violates or risks violating those rules; see the canon discrepancy section.

## 3. Repository / Technology Overview

### Relevant structure

```text
Assets/Ashfall.Core/       Engine-agnostic C# simulation, ports, DTOs, catalogs, saves
Assets/Ashfall.Core.Tests/ xUnit tests for Core
Assets/_Game/              Unity legacy gameplay, UI, editor, ScriptableObjects, wiring
Assets/StreamingAssets/Data/ JSON authority and expansion/narrative catalogs
src/                       Godot C# host, panels, sessions, save stores, bridge
scenes/                    Godot scenes; Main.tscn is the active entry scene
Assets/Scenes/             Unity legacy scenes
docs/                      Migration, UI, art, CI, expansion, audit, and planning documents
generated_AIassets/        AI-generated/approved asset staging and proofs
scripts/                   Godot/support C# scripts and command helpers
tools/                     Repository/content tooling
ProjectSettings/           Unity project settings and editor version
Packages/                  Unity package manifest and lock data
.github/workflows/         CI/build definitions, mostly Unity-oriented and partly stale
```

### Engine and build configuration

- `project.godot` identifies Godot C# 4.7, compatibility renderer, 1920×1080 viewport, 60 FPS cap, and `scenes/Main.tscn` as the main scene. Fonts include Barlow Condensed and Share Tech Mono.
- `Ashfall.csproj` uses `Godot.NET.Sdk/4.7.1`, targets `net8.0`, enables nullable, and compiles `src/**/*.cs`, `scripts/**/*.cs`, `Assets/Ashfall.Core/**/*.cs`, and `Assets/_Game/**/*.cs` through the bridge compatibility layer.
- `Ashfall.Core/Ashfall.Core.csproj` is a deterministic `netstandard2.1` plain-C# project with C# 9 and `System.Text.Json` 8.0.5.
- `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` targets `net9.0` and uses xUnit plus Microsoft.NET.Test.Sdk.
- `ProjectSettings/ProjectVersion.txt` identifies Unity 6.0.5f1 / editor `6000.5.5f1`.
- `Packages/manifest.json` contains a substantial Unity 2D/URP/Input System/UI Toolkit/test package set. This is legacy-engine configuration, not evidence that Unity is the active execution target.
- The active source-level rule is Godot + `dotnet`; Unity must not be launched by an agent unless explicitly requested.

### Useful commands documented by the repository

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expansion-suite
```

These are expected verification procedures, not results from this pass. CLI self-tests are dispatched through `src/Host/HostCli.cs` and `src/Main.cs`.

## 4. Current Development State

### Overall state

- **Godot host:** `PARTIALLY IMPLEMENTED`. It boots a programmatic developer/integration UI, runs many Core and host self-tests, hosts several expansion surfaces, and persists multiple subsystems. It is not yet a complete visual or gameplay-parity presentation of the legacy game.
- **Engine-agnostic Core:** `PARTIALLY IMPLEMENTED` as a migration. It has serious breadth and many saveable systems, but several newer ports are not demonstrably wired into a complete player loop and some direct randomness/duplicate abstractions remain.
- **Unity codebase:** `IMPLEMENTED` in breadth but `DEPRECATED` as the active migration target. It contains the most assembled gameplay and UI, but violates the desired long-term boundary in many places.
- **Data catalogs:** `IMPLEMENTED` as a large authority surface, with mixed schema conventions, partial validation coverage, and uncertain host consumption for some catalogs.
- **Narrative/expansion layer:** `PARTIALLY IMPLEMENTED`; many expansions have real Core/data/test artifacts, but integration, content completeness, and host visibility vary sharply.

### Strongest areas

1. Data volume and thematic specificity: items, survivors, locations, radio, history, factions, events, and expansion catalogs are substantial.
2. Core save-state discipline in newer systems: explicit DTOs, checksums, sorted captures, migrations, and round-trip tests are common in expansion envelopes.
3. Deterministic design intent: seeded RNG ports, ordered captures, invariant serialization, and self-tests are first-class goals.
4. Expansion scaffolding: the first four expansions and several later expansion systems follow repeatable Core/data/host/save/test patterns.
5. Repository awareness: migration and UI documents identify many risks instead of pretending the Unity/Godot transition is complete.

### Weakest or most uncertain areas

1. Full Godot gameplay loop and visual parity.
2. Cross-host parity for legacy survival, combat, medical, shelter, quest, and UI systems.
3. Single canonical faction/character ID vocabulary.
4. Main legacy save compatibility and complete save ownership.
5. End-to-end integration of the large narrative catalog set.
6. Current CI alignment with the active Godot workflow.
7. Runtime audio and generated-asset coverage.

### Likely development phase

**INFERRED:** the project is in a migration/integration phase rather than a content-greenlight phase. New feature prompts should generally port or integrate an existing mechanic into Core and a thin Godot host before inventing a Godot-only replacement.

## 5. Core Gameplay Loop

The repository documents and implements fragments of the following loop; the exact complete loop differs by host.

### Minute-to-minute

The player inspects survivor condition, shelter capacity, supplies, equipment, tasks, warnings, radio/records, and current hazards; assigns work or treatment; chooses whether to consume, craft, trade, repair, ration, explore, or respond to an event; then advances time. The active Godot UI exposes these as dev/integration panels and testable sessions more than as a finished in-world interface.

### Typical game day

1. Read new journal, radio, roster, census, faction, or shelter information.
2. Allocate survivors to work, treatment, maintenance, food/water processing, research, or expedition preparation.
3. Check weather, fallout, cold, radiation, air/heat, fuel, and shelter degradation.
4. Craft, repair, filter water, grow/harvest, trade, or consume supplies.
5. Resolve travel, scavenging, door, faction, social, or narrative events.
6. Accept a loss, compromise, debt, suspicion increase, or relationship consequence.
7. Save a durable state and advance the simulation.

### Medium- and long-term loops

- **Short term:** survive the next tick/day and prevent cascading needs, exposure, or shelter failures.
- **Medium term:** improve shelter capacity and equipment; discover locations; build relationships; stabilize food, water, fuel, medicine, and information; navigate factions and expansion arcs.
- **Long term:** survive phase-based environmental and faction crises, preserve institutional or human knowledge, determine regional/faction/settlement outcomes, and reach an epilogue/ending state.

### Risk/reward loops

- Leaving shelter yields supplies, information, locations, and narrative but spends time/stamina and exposes people to radiation, weather, injury, contamination, and encounters.
- Pushing an expedition increases potential loot and discovery while raising failure risk, exposure, and depletion.
- Spending scarce medicine, filters, fuel, or clean water protects present survivors but can reduce future resilience.
- Cooperation with a faction can unlock access/trade/quests while creating obligations, standing changes, or enemies.
- Preserving records, evidence, or children may improve future meaning/epilogue outcomes while consuming immediate capacity.

### Failure and success

- **OBSERVED:** `NeedsSystem` can kill survivors at zero health unless death is deferred; radiation has acute/chronic thresholds; expeditions can fail; shelter/environment systems have hazards; expansion systems have explicit endings and epilogues.
- **UNCERTAIN:** one unified Godot game-over and campaign victory flow is not established. `src/Main.cs` has `GameOverPanel` and endgame/epilogue hosts, but the full campaign loop is not visibly assembled.

## 6. Architecture Overview

### Intended layering

```text
JSON catalogs / save envelopes
          ↓
Core loaders + validation + typed DTOs
          ↓
Ashfall.Core systems (plain C#, ports, events, CaptureState/RestoreState)
          ↓
Host sessions and adapters (Godot or Unity)
          ↓
Thin Nodes / MonoBehaviours / panels / input / presentation
          ↓
UI, audio, assets, diagnostics, and player-facing feedback
```

The intended rule is that simulation remains in `Ashfall.Core`, while hosts provide file I/O, logging, time, RNG seed plumbing, serialization, persistence, and presentation. New code should not put gameplay rules in Godot Nodes or Unity MonoBehaviours.

### Core ports and infrastructure

`Assets/Ashfall.Core/Ports.cs` defines `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, and `ISeededRng`. `HostDefaults.cs` supplies BCL file I/O, `System.Text.Json`, a deterministic xorshift64* RNG, invariant-culture behavior, catalog location, and simple logging. `SaveChecksum.cs` calculates a canonical SHA-256 over public instance fields with sorted reflection traversal and depth limits.

Core also has `Clock/ISimClock.cs`/`SimClock`, `Events/IEventBus.cs`/`SimpleEventBus`, and `Flags/IFlagLedger.cs`. These overlap conceptually with legacy `TimeSystem`, a static legacy `EventBus`, and other host-specific registries; migration should reduce rather than multiply these surfaces.

### Godot host

`src/Main.cs` is a large partial `Control` and current composition root. `_Ready()` resolves the data directory, parses `HostCli`, dispatches self-tests, builds the programmatic UI, and sets up journal, Ice Road, Duty Roster, expansions, and Year of Ash. `_Process()` ticks the bridge and refreshes diagnostics; `_Notification()` flushes many dirty stores and shuts down the bridge.

The host currently includes panels/sessions for Holdfast, trade, Duty Roster, Standing Record, Crossing, Arbitration, ledger debt, Greenhouse, Year of Ash, Muster, Dose, Verdict, Inventory, Survivors, Economy, Utility AI, Radio, Journal, and diagnostics. This is a useful migration laboratory and dev hub, but `Main.cs` is a high-coupling monolith and should not become the permanent home for domain rules.

### Unity legacy composition

`Assets/_Game/Core/GameBootstrap.cs` and its partials are a broad composition root with Inspector references and public accessors for nearly every major system: game state, time, weather, temperature, photoperiod, needs, radiation, shelter, air/heat, maintenance, work shifts, tasks, inventory, crafting, Utility AI, events, suspicion, save, scavenging, expeditions, atmosphere, medical, economy, world phase, factions, radio, endgame, and many specialized systems. `GameBootstrap.InitLate.cs` wires a large save graph and still contains host-era randomness and dependencies.

### Unity-to-Godot bridge

`src/Bridge` contains a compatibility shim for Unity APIs used while legacy code is compiled under Godot. `BridgeRuntime` emulates lifecycle calls and coroutines; `BridgeGap` distinguishes semantic gaps that throw from cosmetic gaps that log or quietly no-op. `BridgeSelfTest` explicitly probes unsupported `Instantiate`, `PlayerPrefs.Save`, `Camera.main`, texture, audio, and deterministic shim behavior.

**RECOMMENDED CONTEXT NOTE:** a bridge pass is migration scaffolding, not permission to keep Unity APIs in Core. Treat every bridge reference as a candidate for port/adaptation, and classify the behavior as semantic, cosmetic, or test-only before changing it.

## 7. Major System Dependency Map

The following relationships are more useful for future changes than isolated file lists.

```text
NeedsSystem
├── consumes time, shelter temperature/heat, food/water state, and survivor state
├── changes hunger, thirst, fatigue, warmth, morale, health, and hygiene
├── emits need-changed, critical, and death events
├── affects work efficiency, treatment urgency, expedition readiness, and narrative eligibility
├── persists through CaptureState/RestoreState in Core hosts
└── is also represented by a larger Unity legacy system that must not become a second authority
```

```text
Radiation + Weather + Shelter
├── Weather adds fallout/black-rain dose, cold, visibility, and gear-melt modifiers
├── Shelter shielding and air filtration reduce exposure but degrade or consume maintenance resources
├── Worn gear reduces dose and can lose durability
├── Dose affects acute/chronic status, health, medicine demand, and survivor eligibility
├── Dosimeters, iodine, anti-rad, medical UI, and records expose the state
└── save/checksum state must preserve dose, lifetime exposure, resistance windows, gear, and status
```

```text
Inventory → Crafting → Shelter/Economy
├── ItemDefinition supplies stack, weight, durability, contamination, effects, trade, and equipment data
├── Inventory capacity and equipment constrain crafting, travel, treatment, and work
├── Crafting consumes ingredients, requires stations/condition, advances time, and routes overflow to stash/refund
├── Economy assigns demand/price and barter outcomes to goods and faction stock
├── UI needs item icons, quantities, tooltips, fairness, and failure feedback
└── all state changes require persistence and content-ID validation
```

```text
Time / Weather / World Phase
├── advances needs, shelter degradation, markets, crops, expeditions, quests, and expansion phases
├── changes encounter eligibility and location danger
├── feeds radio, journal, faction escalation, Deep Freeze, Siege, and Great Thaw schedules
├── controls save checkpoints and autosave cadence
└── must remain deterministic across hosts and respect stable ordering
```

```text
Faction standing
├── is represented by faction data, expansion standing/charter systems, and legacy reputation systems
├── affects access, prices, trade inventory, quests, encounter selection, warnings, and possible attacks
├── writes evidence/journal/radio/dialogue and can change epilogues
├── depends on canonical faction IDs, which are currently inconsistent across catalogs
└── therefore any faction feature requires data, narrative, UI, save, and mapping review
```

```text
Narrative / Events / Journal
├── catalogs select by day, location, stance, danger, flags, and faction context
├── choices modify morale, guilt, standing, evidence, flags, resources, and future eligibility
├── journal/radio/records make consequences legible
├── quests and expansions add content through catalog IDs and save state
└── incomplete host wiring can make authored content appear unused even when the data exists
```

```text
Expansion host
├── loads data catalogs
├── constructs Core session/system
├── subscribes to events for UI and dirty-save coalescing
├── ticks at day/hour/encounter boundaries
├── exposes a panel or terminal surface
├── persists a versioned envelope with checksum and migration
└── registers self-tests and data-integrity checks
```

## 8. Gameplay Systems

### 8.1 Needs, survivor condition, and death — `IMPLEMENTED` in Core; `PARTIALLY IMPLEMENTED` in Godot parity

`Assets/Ashfall.Core/Survivors/NeedsSystem.cs` models Hunger, Thirst, Fatigue, Warmth, Morale, Health, and Hygiene. Hunger, thirst, and fatigue worsen upward; warmth, health, and hygiene worsen downward; morale defaults around 50. Defaults include hunger `.8/hour`, thirst `1.2/hour`, fatigue `.4/hour`, cold warmth loss `.5/hour`, heat warmth gain `3/hour`, and critical hunger/thirst around 90. Critical conditions can damage morale/health; health reaching zero raises death unless a deferral hook is active. Events include need change, critical, and death.

The legacy survivor/schedule/work systems add profession, task, skill, relationship, social, work-shift, injury, and psychological behavior. These remain distributed through `Assets/_Game/Survivors`, `Assets/_Game/Medical`, and `Assets/_Game/Shelter` and are not equivalent to the smaller Core needs model. Any “add a need” request must determine whether it belongs in Core, legacy compatibility, or both.

### 8.2 Radiation, contamination, and protective equipment — `IMPLEMENTED` in Core; host integration `PARTIALLY IMPLEMENTED`

`Assets/Ashfall.Core/Radiation/RadiationSystem.cs` tracks per-survivor dose, lifetime exposure, acute/chronic statuses, worn gear, shelter shielding, zone rate, contamination decay, dosimeter use, iodine resistance, and anti-radiation treatment. Constants include an acute threshold around 80, a chronic lifetime threshold around 400, acute health loss around 5/hour, iodine resistance of 6 hours, and a 24-hour iodine window. Gear protection and durability are applied to exposure; weather can add fallout and black-rain dose.

The model also contains a radiotrophic/high-radiation hook inherited from legacy design. It is unusual and must be treated as an explicit mechanic, not assumed realism. Radiation touches health, medical items, shelter, weather, inventory, UI badges, save state, work, expeditions, and narrative eligibility.

### 8.3 Weather, temperature, atmosphere, and environmental hazards — mixed `IMPLEMENTED` / `PARTIALLY IMPLEMENTED`

`Assets/Ashfall.Core/World/WeatherSystem.cs` models clear, fallout, black rain, blizzard, and visibility/temperature/radiation modifiers. Weather checks occur on a multi-hour cadence and can block scavenging without suitable protection. The legacy environment area additionally contains temperature, photoperiod, air, heat, black rain, shifting hotspots, frozen pipes, radon, atmosphere, and shelter maintenance systems.

Direct risk: the Core `WeatherSystem` currently constructs `System.Random(seed + rollCount)` instead of consuming the Core `ISeededRng` port. This contradicts the repository’s deterministic-host rule and should be called out in any weather or parity prompt.

### 8.4 Shelter, air, heat, and maintenance — `IMPLEMENTED` in Unity legacy; Core/Godot status `PARTIALLY IMPLEMENTED`

The Unity shelter area is large (`Assets/_Game/Shelter`) and includes bunker rooms, capacity, shielding, air filtration, heat, power, water storage/economy, work orders, maintenance, damage, structural concerns, waste, vermin, pipes, pantry, greenhouse, and specialized shelter events. Newer Core expansion slices add Greenhouse and other shelter-adjacent systems, but no evidence establishes complete Godot parity for all legacy shelter simulation.

Shelter is an especially coupled domain: its capacity and shielding affect survivor assignment, radiation, air, heat, resource demand, event eligibility, construction/upgrades, UI layout, and save compatibility.

### 8.5 Inventory, equipment, items, and crafting — Core `IMPLEMENTED`; full integration `PARTIALLY IMPLEMENTED`

`Assets/Ashfall.Core/Inventory` provides stack/weight/capacity inventory, all-or-nothing add, remove, transfer rollback, equipment slots, worn gear, devices with battery/calibration, consumption, and ID-based state copying. It emits item-added, item-removed, and inventory-changed events. It is designed to be saveable and host-neutral.

`ItemDefinition` carries IDs, names, description, icon path, type, stack max, weight, radiation protection, durability, equip slot, contamination, hunger/thirst/health/radiation/morale effects, EMP shielding, trade value/tier, scrap, repair data, and related properties. There are conceptual `WornGear` types in both Inventory and Radiation Core locations and another legacy version; new equipment work must resolve ownership before adding fields.

`CraftingSystem` supports recipe queues, station requirements, station condition/wear, ingredient consumption, timed completion, result overflow to stash or ingredient refund, crafter/time gates, a moonshine gate, state capture, and start/completed/overflow events. The data catalog currently has 32 recipes, mostly workbench recipes, with stove, water purifier, distiller, and heater examples.

### 8.6 Economy, markets, trade, and barter — Core `IMPLEMENTED`; legacy integration `PARTIALLY IMPLEMENTED`

`Assets/Ashfall.Core/Economy/MarketSystem.cs` loads `economy_goods.json`, validates snake_case goods IDs, categories, and base prices, tracks demand, clamps demand multipliers to roughly `.25–4`, treats demand above about `1.35` as shortage, and clamps prices to `.25–4×` base. Daily noise is deterministic through a caller-provided RNG. Equal-value barter has explicit remainder handling. Captured state is versioned (`MarketState` v1), sorted, and evented.

The legacy `DynamicEconomySystem` remains a large cross-domain system connected to factions, quests, shelter, survivors, and trade. Migration notes say it delegates market demand to Core and applies Hardcore Economy tuning, but the legacy class is still an integration hotspot. The Godot host now has Economy and Holdfast trade panels; exact campaign-wide trade coverage remains uncertain.

### 8.7 Expeditions, travel, scavenging, and procedural maritime content — mixed

Core `Expeditions` contains a tick machine with stance, speed, stealth, push-luck, capacity/stamina, encounters per leg, and save state. Migration notes identify known semantic deviations from Unity: night-scavenge and bicycle bonuses, stamina-zero failure behavior, an unread flashlight field, and differing save shape. Treat parity as `PARTIALLY IMPLEMENTED`.

Core `Maritime` adds Black Flotilla-adjacent content: a four-room stealth dive (`Deckhouse`, `Companionway`, `HoldApproach`, `DeepHold`), 120-second air, noise and compromised state, compressor crank, procedural scavenge with degradation phases around days 20/50/80, high-radiation/biohazard modifiers, visit depletion, container state, psychological contamination, deep-lore location loading, and variable loot. The data and tests exist, but host/save/content integration is not proven complete.

### 8.8 Combat, injury, illness, and medicine — legacy broad; Core migration `PARTIALLY IMPLEMENTED`

The Unity tree contains combat, combat trauma, injuries, illness, medical treatment, radiation care, mental breaks, emergency care, work hazards, and medicine-linked events. Recent Core ports include or reference Combat Trauma, Respiratory Degeneration, Caregiving, Guilt/Insomnia, and other narrative/medical systems. The current Core model is not a demonstrated replacement for all legacy combat/medical behavior.

Any medicine or injury expansion should inspect health/needs, radiation, medical catalogs, inventory consumption, survivor schedules, work efficiency, UI condition displays, event/quest eligibility, trade/loot, and save state. Do not add a standalone medicine list without integrating those dependencies.

### 8.9 Utility AI — Core `IMPLEMENTED`; Unity fork remains

`Assets/Ashfall.Core/UtilityAI` loads four actions from `utility_actions.json`. Actions have base priority, weight, override, tags, and piecewise-linear curves. The scorer applies trait vetoes, tiny deterministic noise (`.0001`), positive-score filtering, and first-wins ordering. Veto examples include coward/loud labor, god-complex/menial work, pacifist/weapons, blind/guns, ex-con/order, hitman/medical/farming, and germaphobe/medical triage without hazmat. Selection events exist; AI state is not persisted, while host-provided survivor context is external.

The Unity Utility AI implementation is still present. A prompt that changes action scoring should specify whether it is a Core rule, a legacy bridge compatibility change, or a host-only context change, then add deterministic tests and inspect task/needs/work effects.

### 8.10 Survivors, social behavior, relationships, and psychology — legacy rich; Core slices emerging

`Assets/_Game/Survivors` includes survivor models, work shifts, social systems, relationships, personal quests, beliefs, memories, mental breaks, bunker social systems, and a very large `PersonalQuestSystem`. Core ports and narrative systems add trauma bonds, caregiving, ideological friction, guilt/insomnia, and other focused mechanics. There is no single demonstrated cross-host survivor authority yet.

Future survivor content needs canonical IDs, profession/trait data, needs, work/task eligibility, medical state, inventory/equipment, faction/relationship effects, event hooks, UI portrait/condition presentation, and persistence. The base survivor catalog has 102 entries; expansion survivor catalogs add named and archetypal records.

### 8.11 Factions, reputation, standing, and access — broad but identity-fragmented

Faction behavior is distributed across `Assets/_Game/Factions`, economy, events, quests, radio, raids, debt, standing, and expansion systems. Core/host expansions add Standing Record, Nobody’s Charter/Crossing, Arbitration, Ledger Debt, faction war, and faction icon surfaces. Faction standing can logically affect access, trade, prices, quest availability, encounters, raid pressure, evidence, journal entries, and endings, but current ID mapping is inconsistent (see sections 13 and 28).

### 8.12 Quests, encounters, events, and world simulation — data-rich, integration-variable

The legacy tree includes `EventRunner`, encounter factories, events, quests, dynamic questlines, suspicion, parley, raids, flashpoints, and world phase. Core has `NarrativeEncounterSystem`, expansion quest systems, door encounters, faction-war events, and multiple narrative records. The data volume is larger than the visibly assembled Godot campaign loop.

### 8.13 Time, progression, succession, and endings — mixed

Time drives needs, weather, markets, crops, crafting, travel, quest gates, expansion phases, and save checkpoints. Core `Legacy/GenerationalSuccessionEngine` models a 365-day Century Seed with chapters, aging, retirement, mentoring, inherited traits, and save state, but host integration is uncertain. `Endgame/EpilogueMatrixRuntime` evaluates regional fate, demographics, moral standing, and flags into a chronicle with many permutations; it is a strong extension point but not proof of a complete campaign ending flow.

## 9. Data Architecture

### Authority rules

`Assets/StreamingAssets/Data` is the intended source of truth. New content should normally be represented as JSON with an existing schema and loaded through a Core catalog or a deliberately documented host adapter. Unity ScriptableObjects are generated/editor conveniences, not a parallel authority. Never invent IDs outside the relevant master/catalog vocabulary.

### Data conventions

- IDs are intended to be `snake_case`, though some historical catalogs and faction namespaces are inconsistent.
- Base catalogs commonly use camelCase fields such as `displayName`, `minDay`, and `tradeValue`.
- Many expansion/narrative catalogs use snake_case fields such as `schema_version`, `faction_id`, and `min_day`.
- `System.Text.Json` is configured case-insensitively in Core, but case-insensitivity is not the same as a camelCase/snake_case naming policy. New schemas must follow their nearest authoritative example and have a loader test.
- Exactly 35 of the 280 observed JSON files contained a `schema_version` field. Versioning is therefore strong in some save/catalog surfaces and absent in many content files.
- Catalog loaders commonly use typed public-field DTOs. Some legacy loaders use Unity `JsonUtility` wrappers and are not safe as Core authority.
- Several loaders catch parse/load exceptions and return empty data. This can turn malformed content into silent absence; validation and logging need scrutiny before relying on a catalog.

### Main catalog inventory

| Catalog | Observed records / shape | Use |
| --- | ---: | --- |
| `items.json` | 499 array entries | Base item authority. |
| `survivors.json` | 102 | Base survivor records/archetypes. |
| `locations.json` | 105 | Base location graph/content. |
| `recipes.json` | 32 | Crafting recipes. |
| `events.json` | 77 | Base event pool, mostly early-game. |
| `radio.json` | 50 | Radio/intel messages. |
| `echoes.json` | 23 | Echo/record content. |
| `world_history.json` | 79 | Timeline/history records. |
| `characters.json` | 36 | Named character catalog. |
| `faction_lore.json` | 19 | Faction lore/identity records. |
| `narrative_arc_events.json` | 15 | Deep-lore arc events. |
| `narrative_encounters.json` | 3 | Core narrative encounter records. |
| `narrative_questlines.json` | 4 | Narrative questline definitions. |
| `questline_master.json` | 194 ID entries | Registry-like master list; not all entries are fully authored. |
| `dynamic_questlines.json` | 2 full questlines | Dying Signal and Aquifer Contamination. |
| `door_encounters.json` | 68 entries / 153 choices | Late door/visitor/faction choice content. |
| `faction_war_events.json` | 21 chains plus related comms/dialogue/journal/radio/overrides | Late faction conflict. |
| `economy_goods.json` | 12 | Core MarketSystem goods. |
| `utility_actions.json` | 4 | Core Utility AI actions. |

Expansion catalogs include Holdfast items 40/locations 35/quests 10/factions 3; Duty Roster locations 14/marks 43/quests 28; Standing Record layouts 14/memory records 38/quests 10/factions 1; Crossing factions 3/items 11/locations 13/quests 12; Year of Ash items 57/locations 66/events 48/quests 32/radio 50/survivors 36; Greenhouse items 14; Verdict items 15/locations 4/NPCs 6/questline entries 8; Black Flotilla items 24 and two dive-site keys.

### Content extension pattern

For new content, inspect the nearest catalog, its Core record/loader, its host session, its save DTO, its tests, and `DataIntegrity` checks before editing. The safest extension pattern is:

1. Add or extend an existing JSON schema with valid IDs.
2. Add a typed Core record/loader or reuse the existing one.
3. Add validation for required fields, references, and ordering.
4. Add simulation rules and explicit events in Core.
5. Add host wiring/panel only for presentation and input.
6. Add save state, migration/checksum behavior where stateful.
7. Add unit, round-trip, data-integrity, and relevant Godot self-tests.

## 10. Persistence / Save System

### Cross-host design

Newer systems use serializable public-field DTOs with `CaptureState()`/`RestoreState()`, versioned envelopes, checksums, sorted collections, explicit old-version migration, and temp-file/user-path stores in the host. `SaveChecksum.cs` is intended to provide serializer-independent integrity by traversing public instance fields in a canonical order.

### Important save envelopes

- `HoldfastSave` v4 includes Ice Road, Census, Brine, quests, day, and checksum with frozen v1/v2/v3 migrations.
- `ExpansionHubSave` v1 includes Waystation, Standing layouts/memory/site encounters, Crossing vouch, Greenhouse, Arbitration, and Ledger state.
- `YearOfAshSave` v2 includes timeline, door encounters, faction war, deep freeze, radon, quests, and checksum with v1 migration.
- Verdict save v2 includes machine log, census/readout, evidence, NPC state, and reckoning with v1→v2 migration.
- Other `src/Host/*SaveStore.cs` files persist Economy, Inventory, Survivors, World, Narrative, Muster, Dose, Verdict, and related sessions.

### Legacy risk

The legacy `_Game/SaveSystem` is broad and its main path uses Unity serialization/`JsonUtility` patterns. It is not a safe Core authority and is not proven cross-host compatible. A save-affecting change must identify whether the target is a Core envelope, Godot store, Unity legacy save, or a migration bridge.

### Save safety rules

- Add fields to a versioned DTO and define old-state defaults.
- Preserve stable ordering for lists/maps that feed checksums or deterministic outcomes.
- Do not persist host objects, Unity/Godot references, private fields that checksums cannot see, or unstable generated IDs.
- Verify referenced item, survivor, location, faction, quest, and event IDs.
- Add round-trip, old-version migration, checksum, and cross-host shape tests.
- Treat any new state as a UI/dirty-save/event integration task too.

## 11. World and Canon

### Established setting elements

The canonical world is a fictional region after a nuclear exchange. Bunkers, damaged civil-defense infrastructure, water systems, rail/industrial sites, hospitals, farms, archives, military remnants, and isolated communities survive under cold, ash, radiation, and institutional collapse. The world is not meant to be a heroic war simulator; its conflict is experienced through scarcity, paperwork, coercion, damaged trust, and the material consequences of decisions.

### Recurring world concepts

- Tessarat appears in endgame/epilogue and wider world material as a regional anchor.
- Water, filtration, reservoirs, hydro infrastructure, and water rights are central social/economic structures.
- The Garrison, Ash Sign/cult, Rebuilders, Warlords, Black Ops, Forward Roster, Railway, Ordnance, Penal, Salt, Supply, and related groups represent competing forms of continuity, autonomy, faith, force, logistics, and record-keeping.
- The Forty-Five Minute War, EMP Silence, Open Door, nuclear winter, black rain, bunker continuity, Ashfall, deep freeze, faction siege, and great thaw recur in history and expansion content.

### Canon discipline

Not every record in `world_history.json`, faction lore, character catalogs, or expansion documents is proven to be active in the runtime. Treat catalog presence as authored content, not necessarily playable canon. The exact geopolitical identity of the pre-exchange world is intentionally or inconsistently abstracted; do not add real countries or wars.

## 12. Timeline

The following timeline is compressed from `world_history.json`, expansion catalogs, and Core phase systems. Dates and phase boundaries are authoritative only where a runtime system uses them.

| Period | Observed content |
| --- | --- |
| Before exchange | Water Wars, rare-earth crisis, continuity office, bunker boom, last harvest, evacuation and final broadcasts. |
| Hour zero / first days | Forty-Five Minute War, EMP Silence, Open Door, convoy turnback, first night, registrar, hospital triage, first council, codes, militia gathering. |
| Black sky | Infrastructure failure, nuclear winter, ozone collapse, allocation orders, black rain, Vessel, Ash Sign/Cult emergence, Memory Vault, tunnel exodus, Garrison requests, Warlords, archive and score systems. |
| Early Ashfall | Scavenger economy, engineer loss at 12-B, defection, children of dark, Ash Sign readings, grain exchange, provenance disputes, present-day records and claims. |
| Days 90+ | Holdfast and related early expansion surfaces begin around the day-90 migration/session boundary. Exact campaign sequencing is host-dependent. |
| Days 180–239 | Year of Ash Deep Freeze: about −35°C, conduit fracture, frostbite/black blizzard, cold-frame collapse, fuel gelling, rime choke, periscope frosting, battery slush, hydraulic freeze, stasis despair. |
| Days 240–299 | Year of Ash Faction Siege: registration/martial schedules, artillery, Ash Sign pyres, Hydro Baron closure, sabotage, mobilization, warlord ambush, tool embargo, air breach, executions, plus Railway/Arsenal/Penal/Salt/Supply/Ash Militia variants. |
| Days 300–360 | Year of Ash Great Thaw: black mud, inundation, radon fissures, 142.850 MHz carrier, sump overrun, toxic mold, thawing corpses, saline inversion, icebreaker opening, aurora manifest, isotope revelations, subsidence, geothermal flash, first rye, stand-down/abandonment, final dawn. |
| Days 480–605 | Faction War data contains cold-war, open-conflict, offensive, and culmination chains. This is authored future/late content; complete campaign integration is uncertain. |

`events.json` is heavily front-loaded: 67 of 77 observed events fall around days 1–30, 10 fall around days 31–179, and no base entries were observed after day 179. Late content is mainly supplied by expansion/door/faction-war catalogs. This creates a likely early-content density imbalance.

## 13. Factions

### Current faction records

`faction_lore.json` contains 19 records including:

`iron_garrison`, `ash_militia`, `cult_of_ash_sign`, `warlords_sector_4`, `faction_rebuilders`, `faction_black_ops`, `faction_central_garrison`, `faction_ash_sign`, `faction_scavengers`, `faction_hydro_barons`, `faction_unaligned`, `faction_salt_freeholders`, `faction_railway_guild`, `faction_ordnance_foundry`, `faction_penal_battalion`, a second/legacy-looking `faction_ash_militia` namespace, `faction_supply_corps`, `raiders`, and `faction_forward_roster`.

### Faction meaning and mechanics

- **Garrison / Central Garrison / Iron Garrison:** military continuity, registration, defense, coercive order, checkpoints, and possible siege/raid pressure.
- **Ash Militia:** local armed protection/autonomy with overlapping legacy and expansion IDs.
- **Ash Sign / Cult of the Ash Sign:** ritual/ideological interpretation of ash, sacrifice, pyres, belief, and social pressure.
- **Warlords / Raiders:** force, opportunism, ambush, tribute, and territorial insecurity.
- **Rebuilders:** technical/civic repair, infrastructure, and reconstruction choices.
- **Black Ops:** covert continuity, information, sabotage, or hidden institutional agendas.
- **Hydro Barons:** water control, filtration, sluices, prices, access, and technical feudalism.
- **Scavengers / Unaligned:** exchange, salvage, information, and flexible local survival.
- **Salt Freeholders:** salt/resource autonomy and property/territory politics.
- **Railway Guild / Ordnance Foundry / Penal Battalion / Supply Corps:** logistics, industrial capability, labor coercion, and supply chains.
- **Forward Roster:** expansion-specific continuity/roster organization whose exact relation to other martial factions must be mapped rather than assumed.

### Faction status

Factions are `IMPLEMENTED` as lore/data and legacy code, `PARTIALLY IMPLEMENTED` as unified cross-system entities, and `UNCERTAIN` as a single canonical identity layer. Faction-war catalogs contain 21 chains, 18 communiques, 18 dialogue records, 22 journal records, 29 radio records, and 9 location overrides, demonstrating substantial late-game authored content.

### Identity hazard

`characters.json` and older content use names such as `military_remnants`, `upland_militia`, and `cult_of_the_glow`, while `faction_lore.json` uses different IDs. There are duplicate-looking Ash Militia records. The project’s snake_case rule is not enough to resolve semantic aliases. Any faction prompt must first define or discover a canonical mapping table and update all consumers, saves, icons, quests, radio, and UI.

### Expansion surfaces

The most promising faction depth is not adding more faction names. It is connecting standing to water prices, access, survivor assignment, raid/defense pressure, evidence, journal/radio, infrastructure repair, faction war, and epilogue outcomes. Internal faction conflicts and noncombat logistics are underused relative to the number of faction records.

## 14. Characters and NPCs

### Named catalog

`characters.json` contains 36 named records. Significant names include Bram Ostrowski (mapmaker / toll-related material), Sergeant Pell, Doctor Ianov, Wren, Kestrel, Nomi Fisk, Ivor Lasko, the Cartwright Sisters, Edor Vale, Yara Holm, Leva Quist, Cael Ormund, Halden Mire, Cluster Teacher, Osran Kell, Mattis Cray, Wyn Sabler, Dessa Vane, Perrin Ashby, Ivo Fenn, Kess Adler, Ansel Duth, Tamsin Rook, Len Quill, Hadi Morrow, Nila Brant, Maren Holt, Ira Vell, Benno Kade, Quil Esser, Osric Tann, Dara Mewn, Dr Irina Vel, Wyn Omah, Piet Abar, and Saria Voss.

The base `survivors.json` catalog has 102 entries spanning paramedic, mechanical engineer, farmer, surgeon, pharmacist, veterinarian, therapist, undertaker, soldier, police officer, bouncer, hunter, scuba scavenger, sonar operator, and many other professions. These are more useful as mechanically meaningful archetypes than as a purely cosmetic roster.

### Expansion characters

Year of Ash adds named survivors and faction figures including Ottilie Frayne, Anneke Ruhl, Corporal Felix Vane, Sister Martha, Dr Sarah Chen, Tomas Lind, Valeria Koss, Gunner Mikhail, Naomi Strand, Pavel Volkov, Zoya Reid, Captain Alder, Lydia Hart, Dr Erik Dahl, Igor Morozov, Clara Sloan, Vera Sokolov, Marcus Vane, 1st Officer Lindqvist, Colonel Brand, Ansel Duth, Hadi Morrow, Kess Adler, Len Quill, Chief Assayer Markov, Commander Talia, Major Kroll, Hierophant Malachi, Sapper Vance, Elena Vasquez, Gregor the Salt Miner, Marina Drake, Yuri Belov, Nadia Brant, Boris Kogan, and Anton Vane.

### Character implementation status

Named records are `IMPLEMENTED` as data. Their runtime dialogue, relationship graphs, personal quests, portrait/asset mappings, and persistence are `PARTIALLY IMPLEMENTED` or `UNCERTAIN` depending on the character. Do not assume every catalog name is a recruitable NPC. Before adding an arc, locate its quest/event references and host presentation.

### NPC/enemy archetypes

The repository contains survivor professions, faction agents, raiders, garrison personnel, cult members, scavengers, medical staff, traders, workers, animals/mutated fauna references, and environmental threats. Exact combat AI and enemy roster authority remains primarily in Unity legacy code. New NPC prompts should specify whether the output is a data archetype, social actor, combat actor, quest role, or named character.

## 15. Locations

### Base world

`locations.json` contains 105 records. Danger levels range from 2 to 10; radiation rates observed range roughly 6–85/hour; travel time ranges roughly 1–11 hours. Examples include Abandoned Hospital, Rural Gas Station, Suburban House, Government Bunker, Cartographer’s Cache, Geo-Thermal Plant, Arcology Sector 4, Frozen River Barge, Icebreaker Convoy, Silent Observatory, Seed Vault, Ministry of Truth Bunker, Ash Dune Cemetery, Ski Resort, Geothermal Borehole, and Flooded Subway.

Locations are authored as data nodes and travel/content references. A complete active Godot 2D world map/room renderer was not observed; the current Godot UI is a host surface for systems rather than proof of full map presentation.

### Expansion locations

Expansion 3 adds 21 useful civic/industrial locations: Civil Defense Bunker, Water Treatment, Highway Checkpoint, Grain Silo, Substation, Regional Hospital, Suburban District, Train Yard, Comm Array, Ash Woodland, Pharmacy, Missile Silo, Fuel Depot, Metro Tunnel, Agricultural Co-op, Basement Vault, Police Precinct, Botanical Nursery, Bus Depot, Coal Mine, and Toll House.

Year of Ash adds 66 locations, including Garrison Checkpoint, Hydro Baron Sluice, Icebreaker Dock, Rhizome Vault, D9, salt/rail/arsenal sites, and related industrial/faction spaces. Holdfast adds 35; Duty Roster adds 14; Verdict adds 4; Crossing adds 13.

### Location design pattern

A strong location should expose identity, atmosphere, controlling faction, resources, threats, travel/radiation/danger, quest/encounter hooks, and persistence-relevant changes. Underused opportunities include locations with data but little event/UI linkage, especially industrial, water, archive, medical, and transport sites.

## 16. Narrative / Quest Structure

### Narrative layers

1. Base events and radio establish early survival and world facts.
2. Dynamic questlines provide staged multi-step objectives.
3. Door encounters create late shelter/visitor choices.
4. Faction/expansion chains add political, environmental, and institutional pressure.
5. Journal, records, evidence, letters, lore, and epilogue systems provide memory and interpretation.

### Implemented authored structures

- `dynamic_questlines.json` contains two fully authored staged lines: **The Dying Signal** at a communications array and **Aquifer Contamination** at a water-treatment site. Each has discovery, investigation, crisis, and resolution structure.
- `questline_master.json` contains 194 IDs, but many are registry entries rather than fully authored quest definitions. Do not claim 194 complete questlines.
- `narrative_questlines.json` contains four records and `narrative_arc_events.json` contains 15 records.
- `door_encounters.json` contains 68 entries and 153 choices, gated mainly between days 180 and 360.
- `faction_war_events.json` contains late conflict chains with separate comms, dialogue, journal, radio, and location overrides.
- `NarrativeEncounterSystem` uses encounter weights based on stance, danger, location, and seeded RNG, then records day/location/choice/morale/guilt and history.
- `EpilogueMatrixRuntime` evaluates regional fate, demographics, moral standing, and named flags into a chronicle; exact full campaign reachability is uncertain.

### Narrative status

Data and Core records are `IMPLEMENTED`; full authored-to-runtime coverage is `PARTIALLY IMPLEMENTED`. The most important future prompt requirement is to connect choice consequences to visible state, future eligibility, faction/market behavior, survivor relationships, journal/radio feedback, save state, and ending evaluation.

### Unfinished narrative surfaces

- Many catalog records may be present without confirmed host entry points.
- Faction identity aliases undermine consistent dialogue/standing consequences.
- Base events are early-heavy, while late content is expansion-heavy.
- Some named characters have data but uncertain arcs or UI hooks.
- The distinction between planned/deep-lore canon and playable canon is not always explicit.

## 17. Existing Expansions

### Expansion convention

The repository’s recurring expansion pattern is: Core system and DTOs → JSON catalogs and IDs → host session/panel → event/tick integration → versioned save envelope and migration → tests/data-integrity/self-test → UI/assets/radio/journal. Future expansion prompts should follow this structure and state what is intentionally out of scope.

### Current expansion inventory

| Expansion / surface | Observed content | Status |
| --- | --- | --- |
| The Holdfast | Ice Road, Census, Brine, Waystation, quests, membrane/levy/order choices, save v4 | `IMPLEMENTED` Core/host slice; full campaign integration `PARTIAL`. |
| The Duty Roster | Allocation chart, marks, morning rows, visitor/hatch encounters, seasons, save/test surface | `IMPLEMENTED` as a hostable expansion slice. |
| The Standing Record | Layouts, room hierarchy/memory, evidence/access mutation, site encounters | `IMPLEMENTED` slice; unified standing mapping `PARTIAL`. |
| Nobody’s Charter / Crossing | Vouch, weighbridge/charter, arbitration, ledger debt, reputation/access | `IMPLEMENTED` slice; broader faction integration `PARTIAL`. |
| Glass Orchard / Greenhouse | Planting, watering, harvesting, catalog, save | `IMPLEMENTED` slice; full shelter economy integration `PARTIAL`. |
| Year of Ash | Deep Freeze, Faction Siege, Great Thaw; door encounters, radon, geothermal, faction war, quests, radio, locations, items | `IMPLEMENTED` data/Core slices; complete campaign reachability `UNCERTAIN`. |
| Muster | Escalation day, currents, approach selection, deserter coalition, witnesses, epilogue | `IMPLEMENTED` Core/host/test slice. |
| Dose | Dose register, dosimeters, sick list, cohort child, volunteers, sealed readings | `IMPLEMENTED` Core/host surface; broad medical integration `PARTIAL`. |
| Verdict | Machine/census/readout, evidence ledger, NPCs, reckoning, ending chains, save migration | `IMPLEMENTED` slice; overall ending integration `UNCERTAIN`. |
| Black Flotilla | Maritime dive, procedural scavenge, contamination, 24 item records, two dive-site keys | `PARTIALLY IMPLEMENTED`; current code exceeds stale plan, but host/content integration is incomplete or unproven. |
| Century Seed / Generational Succession | 365-day chapters, aging, retirement, mentoring, inherited traits | Core code exists; host wiring `UNCERTAIN`. |
| Endgame / Epilogue Matrix | Regional fate, demographics, moral standing, chronicle, many flags/permutations | Core runtime exists; campaign reachability `UNCERTAIN`. |

`ExpansionMasterSession` currently orchestrates the first four expansion surfaces and loads around the day-90 boundary. Later expansions have separate hosts/self-tests. The repository contains more expansion code than the current Godot main menu necessarily exposes as a seamless campaign.

## 18. Encounters and Events

- Base `events.json` contains 77 records and is weighted toward days 1–30.
- `NarrativeEncounterSystem` provides deterministic weighted selection and resolution history.
- `door_encounters.json` supplies visitor/shelter/faction/medical/radiation moral decisions from days 180–360.
- Faction war has staged chains and parallel communication/journal/radio/location content.
- `OrphanKnockWhitelist` deliberately validates orphan-door events through `whitelists/orphan_knocks.json`, gating a future expansion resolution rather than allowing arbitrary orphan records.
- Legacy `EventRunner`, `EncounterEventFactory`, faction raids, suspicion, parley, flashpoints, and event-specific systems remain much larger than the Core narrative surface.
- Radio catalog has 50 records with frequency, day range, intelligence type, confidence, and message. It is a potential information/reward/foreshadowing system, not merely flavor.

**RECOMMENDED CONTEXT NOTE:** an event addition should specify trigger, eligibility, location, actor/faction, choices, state deltas, future consequences, journal/radio feedback, persistence, and test fixture. A text-only event can become disconnected content.

## 19. Items / Equipment / Resources

### Item taxonomy

The 499 entries in `items.json` classify approximately as:

| Type | Count |
| --- | ---: |
| Material | 137 |
| Weapon | 79 |
| Device | 55 |
| Tool | 43 |
| Food | 34 |
| Medical | 26 |
| Quest | 26 |
| Protective | 21 |
| Comfort | 20 |
| Trade | 13 |
| Filter / Water / Fuel | 7 each |
| Equipment | 4 |
| Ammo / Clothing / Power | 3 each |
| Iodine | 2 |
| IrradiatedWater | 2 |
| AntiRad / Consumable | 1 each |
| Relic | 5 |

Observed nonzero fields include 221 items with durability, 70 equipable items, 51 morale effects, 30 hunger effects, 23 thirst effects, 16 health effects, 9 radiation-protection values, 7 contamination values, 4 EMP-shielded items, and 1 explicit radiation-cleanse item. These counts are useful for identifying taxonomy and balance gaps, not for assuming all items are obtainable or wired to every system.

### Key categories

Food and water support needs; iodine/anti-rad/medical content supports radiation and illness; gas masks, hazmat suits, filters, dosimeters, Geiger counters, and protective equipment support hazards; fuel, power, air filters, repair materials, and tools support shelter; relics, letters, and quest items support narrative; weapons/ammunition support legacy combat; comfort items support morale and social systems.

### Asset mapping risk

There are 523 item-art image files in `Assets/Resources/Art/Items`, but only about 176 exact basename intersections with current item IDs were observed. The rest include legacy/deprecated/variant names. Any new item needs an ID, data record, trade/loot/craft placement, icon path or fallback, UI tooltip, save behavior, and tests—not only an image.

## 20. Economy / Balance

### Observed model

Core MarketSystem uses 12 goods, base prices, demand multipliers, deterministic daily noise, price bounds, shortage threshold, equal-value barter and explicit remainder. Legacy DynamicEconomySystem adds faction/quest/shelter/survivor relationships and Hardcore Economy tuning.

### Resource sources and sinks

- **Sources:** scavenging, expeditions, loot, crafting outputs, farming/greenhouse, trade, faction supply, radio/intel, salvage, and location caches.
- **Sinks:** survivor consumption, radiation/medical treatment, crafting ingredients, repairs/durability, fuel/heat/air, trade, travel, shelter maintenance, and moral/social choices.
- **Opportunity costs:** assigning a skilled survivor to one task removes them from another; expedition time changes needs and event timing; using a filter or anti-rad item now reduces future resilience.

### Balance observations

- Base event supply is front-loaded, while late phase content depends on expansions; this may create progression dead zones or abrupt content transitions.
- The item catalog is large but category counts and exact loot/craft accessibility are uneven.
- Water, medicine, fuel, radiation protection, and shelter maintenance are natural scarcity anchors with many cross-system dependencies.
- Faction prices/access are high-value but require canonical faction mapping.
- Core and legacy expedition failure semantics differ; balance comparisons across hosts are unsafe until parity is resolved.

Do not rebalance from catalog counts alone. Trace production, consumption, prices, loot probabilities, travel time, survivor labor, difficulty, and save/day gates together.

## 21. UI / UX

### Current Godot UI

The Godot UI is generated programmatically through `Control`, `VBoxContainer`, `HSplitContainer`, labels, buttons, panels, and custom helper/theme code. `Main.cs` creates a dev terminal/log-style hub with title, subtitle, main menu, HUD overlay, game-over panel, journal book, diagnostics, expansion panels, inventory, survivors, economy, Utility AI, radio, and several self-test entry points.

The active menu includes Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, Ledger Debt, Greenhouse, Year of Ash, Muster, Dose, Verdict, Inventory, Survivors, Economy, Utility AI, Codex, Diagnostics, and Exit surfaces. J toggles the journal; number tabs select major views; Escape closes overlays. There is at least one duplicate roster button/case artifact in the current host, a small sign of the `Main.cs` monolith’s maintenance burden.

### Legacy Unity UI

`Assets/_Game/UI` contains about 167 C# files / 31,060 lines and extensive UI Toolkit/panel logic. Legacy docs describe HUD widgets, inventory, shelter, character, quests, journal, map, settings, notifications, and other views. The Unity UI is broader but is not the active target.

### UI architecture and risks

The preferred current pattern is `Core state → thin host session → Godot panel → labels/buttons/textures`. `Assets/Ashfall.Core/UI`, `src/UI`, `AshfallUiHelpers`, `FactionIconCatalog`, `FactionIconLoader`, `AssetRegistry`, and theme tokens support this direction. `docs/ui/UI_CORRECTION_REPORT.md` documents dark paper/charcoal/rust/amber tokens, 9-slice panels, faction icon mappings, and typography.

Known risks include direct UI rules in `Main.cs`, incomplete screen flow, current dev-hub presentation standing in for a finished campaign, mismatched/stale counts in UI docs, asset mapping gaps, incomplete tooltip/feedback coverage, and uncertain input/accessibility/localization coverage. Text should not encode important state through color alone.

## 22. Art / Assets

### Visual identity

`docs/ai-art/GAME_VISUAL_DNA.md` describes a near-black UI/presentation over a restrained dry-gouache digital illustration language with charcoal/ink edges, desaturated ash blue, charcoal, rust, mud, and medical cream. Objects should look worn, repaired, practical, and materially specific. It prohibits gore, logos, flags, neon, fantasy, pixel-art, and isometric shortcuts unless a task explicitly establishes a controlled exception.

The current playable presentation is documented as UI Toolkit/panels over an orthographic near-black presentation, not a fully authored 2D world renderer. A 1920×1080 reference is used; inventory thumbnails are commonly 64–128px.

### Asset pipeline

- AI-generated images, video, audio, and 3D assets belong in root `generated_AIassets/`.
- Human-approved/runtime item art belongs under `Assets/Resources/Art/Items/<item_id>.png` or the existing resolver convention.
- Item icon IDs must reuse existing data IDs; never invent filenames disconnected from the catalog.
- Existing prompt rules favor dry gouache, consistent framing, one object, no text, opaque black backgrounds for item icons, and human paintover/approval.
- `generated_AIassets` contains faction emblem variants, title/game-over backgrounds, badges, icons, terminal frame, proofs, and other generated assets.
- `Assets/UI` contains current UI images; `Assets/UI/Icons` includes faction/bio/shock/scarcity assets. `Assets/UI/Textures/Backgrounds` had pre-existing untracked content at the analysis snapshot.

`docs/ai-art/ASSET_TAXONOMY.md` contains useful category/model-routing guidance but has stale counts in places. Desktop prompt libraries referenced by the docs were not part of this repository inspection and should be treated as external/uncertain context.

### Asset risks

The exact item-art intersection is partial; generated/runtime duplication exists; style consistency across legacy and newer assets is uncertain; many data records may have fallback/missing art; and asset references must be tested without Unity. An asset prompt should include destination, ID, dimensions, style anchor, transparency/background rule, import/runtime path, and proof requirements.

## 23. Audio

Audio is `PARTIALLY IMPLEMENTED` and integration is uncertain. On disk, six notable WAV clips were observed: `radio_static_hiss.wav`, `vo_ch11_stockpile.wav`, `vo_ch3_ash_road.wav`, `vo_ch7_milband.wav`, `vo_kind_hatch.wav`, and `vo_kind_parley.wav`, with duplicated radio paths under legacy/assets locations. Legacy hooks include faction radio voice, Geiger audio, asset services, and an editor generator for faction radio voice libraries. Many expected/generated kind clips named in code were not observed on disk.

The bridge intentionally treats some audio methods as cosmetic/quiet gaps. There is no strong evidence of complete Godot mixer/routing/event integration. New audio prompts should specify trigger event, host routing, loop/one-shot behavior, volume/mixer category, fallback when missing, asset destination, and test/smoke procedure.

## 24. Testing / Validation

### Frameworks and test surfaces

- Core tests use xUnit in `Ashfall.Core.Tests`.
- Godot host checks are CLI/headless self-tests routed through `HostCli` and `Main.cs`.
- Unity legacy has EditMode/PlayMode tests and data-validation workflows, but Unity execution is prohibited by the active repository instructions unless explicitly requested.
- Data integrity, catalog validation, save round trips, migration tests, bridge probes, expansion suites, UI probes, and RNG checks are present.

### Static and historical evidence

- About 1,492 xUnit `[Fact]`/`[Theory]` attributes were statically observed in 143 top-level test files.
- Historical migration docs report older Core runs of 408 and later 488 tests, while another UI document claims 1,303 tests and an expected 732 count. These numbers are inconsistent and should not be treated as current truth.
- Archived Unity XML from 2026-08-12 reports 2,287 EditMode cases with 2,285 pass, 1 fail, 1 skip; PlayMode reports 100 with 99 pass and 1 fail. These are historical snapshots.
- Targeted Core XML snapshots from 2026-08-15 include both passing and failing runs for catalog, FinalWish, MoralBranching, and Black Flotilla-related areas. They are not a full-suite result.

### Required verification mindset

For a Core change: run Core tests, data integrity, save round trip, deterministic same-seed tests, and relevant host build/self-test. For a Godot UI/session change: run a Godot build or headless probe, check logs, verify panel state and save reload. For a content change: validate IDs/references/schema and load the affected catalog. Never claim passing current tests from an archived report.

Weakly evidenced areas include full Godot campaign loop, cross-host parity, audio, localization, accessibility, some late expansion integration, and complete data-consumer coverage.

## 25. Build / Platform Constraints

- Active target: Godot 4.7+ .NET/C# on the compatibility renderer.
- Core target: `netstandard2.1`; Godot host: `net8.0`; test project: `net9.0`.
- Legacy Unity target: Unity 6 LTS / 2D URP, editor `6000.5.5f1`, but legacy only.
- Current CI files are Unity-oriented: validation, EditMode/PlayMode, Linux build, Windows/WebGL build. They are stale or only partially aligned with Godot migration instructions.
- No networking architecture was established during inspection; treat networking as `UNKNOWN`/not a current core feature.
- Localization framework was not established; many UI strings are direct literals and data fields are English. Treat localization as `UNKNOWN` or an expansion risk, not a solved system.
- Accessibility guidance exists at the level of avoiding color-only signals and using readable text, but full input, screen-reader, remapping, contrast, and controller support are `UNCERTAIN`.
- Platform-specific file/path behavior matters: Godot `user://`, StreamingAssets discovery, temp save writes, and invariant serialization must remain portable.

## 26. Technical Debt

The following are evidence-backed candidates, not fixes performed by this dossier.

| Risk | Evidence | Confidence |
| --- | --- | --- |
| Unity coupling remains large | `Assets/_Game` is ~233k lines; bridge compiles legacy APIs; many MonoBehaviour/editor dependencies | OBSERVED / HIGH |
| Direct randomness violates policy | ~5 Core files and ~285 legacy files reference `System.Random`, `new Random`, or Unity random; Core weather directly constructs `Random` | OBSERVED / HIGH |
| Unstable IDs | `Guid.NewGuid` appears in Core/legacy/src; `ProceduralItemInstance` uses it for item instances | OBSERVED / HIGH |
| Legacy serialization risk | `JsonUtility` appears in ~50 legacy files; legacy main save is broad and host-specific | OBSERVED / HIGH |
| Oversized composition/managers | `PersonalQuestSystem`, `SaveSystem.Wiring`, `GameBootstrap`, `DynamicEconomySystem`, `MedicalSystem`, `Main.cs` are very large | OBSERVED / HIGH |
| Duplicate abstractions | Core/legacy clocks, Core/legacy event buses, multiple `WornGear` types, duplicate host sessions | OBSERVED / HIGH |
| Silent data failure | Several loaders catch errors and return empty catalogs/lists | OBSERVED / MEDIUM-HIGH |
| Faction identity drift | Multiple faction ID vocabularies and duplicate-looking Ash Militia entries | OBSERVED / HIGH |
| Save compatibility surface | Many new envelopes are versioned, but not all systems are; checksum sees public fields only | OBSERVED / HIGH |
| Godot host monolith risk | `src/Main.cs` contains composition, menu, test dispatch, panel setup, and save flushing | OBSERVED / HIGH |
| Stale documentation/CI | README and top-level CI describe Unity as authoritative; migration/UI docs disagree on counts | OBSERVED / HIGH |
| Asset/audio integration gaps | Partial item-ID image coverage and missing expected audio files | OBSERVED / MEDIUM |

Other code-health concerns from the repository’s audits include hard-coded content, host logic leakage, fragile initialization ordering, PlayerPrefs settings in Unity, bridge semantic gaps, and missing tests around some systems. Do not assert a subsystem is dead solely because it appears in a legacy folder; inspect references and runtime registration first.

## 27. TODO / Incomplete Work

A static marker scan found roughly 2 Core files, 9 Godot-host files, and 44 Unity-legacy files containing TODO/FIXME/HACK/XXX/TEMP/placeholder/stub/unimplemented/deprecated/planned/roadmap-like terms. The meaningful grouped work is:

- **Migration:** move remaining survival, medical, economy, narrative, encounter, UI, and save logic out of Unity-coupled files into Core ports; reduce bridge reliance.
- **Determinism:** replace direct random/Guid generation in gameplay with seeded ports and stable instance IDs.
- **Data:** reconcile schema styles, validate all references, surface parse errors, and determine which of the 280 JSON files are loaded by which host.
- **Save:** complete cross-host authority, version/migration coverage, checksum semantics, and legacy-to-Core transition.
- **UI:** replace dev-hub-only flows with integrated campaign screens, remove duplicate wiring, complete accessibility/input/feedback, and align icons/assets.
- **Narrative:** connect deep-lore, faction-war, door, character, and catalog content to real entry points and consequences.
- **Content:** fill late/base event imbalance, missing item art/audio, sparse or uneven faction/character/location usage, and incomplete Black Flotilla/Century Seed host surfaces.
- **Validation:** align CI with active Godot commands and publish one authoritative current test count/result.

`docs/GODOT_MIGRATION_STATUS.md`, `docs/CI.md`, expansion plans, UI reports, and repository audits contain more planned work, but many statements are now stale. Verify every TODO against current Core/src before converting it into a task.

## 28. Canon vs Implementation Discrepancies

| Documentation or stated intent | Current implementation evidence | Interpretation |
| --- | --- | --- |
| README presents Unity scenes/UI as current | `project.godot`, `src`, Core ports, and AGENTS instructions identify Godot as active | README is stale; use migration docs and source. |
| CI docs mix Unity authority with Godot active commands and old test counts | `.github/workflows` remain Unity-heavy; current code has newer host/self-tests | CI/docs migration incomplete. |
| Migration baseline cites much smaller Core/Godot LOC and older port list | Current Core has 234 files/~43.7k lines, src 84/~19.7k, and newer ports | Baseline is historical, not a current inventory. |
| Black Flotilla plan describes Unity-only/stub content | Current Core has Maritime dive/procedural/contamination code, data, and tests | Plan is stale; current state is partial, not complete. |
| Art docs cite older item/survivor/location counts and zero/low art coverage | Current data has 499 items, 102 survivors, 105 locations and 523 item images | Art docs need reconciliation; exact mapping is still partial. |
| Faction lore appears to be a unified voice set | Current faction IDs and character/expansion namespaces differ and duplicate Ash Militia | Canonical mapping is unresolved. |
| Project rules prohibit real-country/wars/people | `world_history.json` and some catalogs contain direct real-world references such as China, NATO/Russian/Iran-related terms | Content cleanup/fictionalization is required; do not extend the violation. |
| JSON is authority and Core should be host-neutral | Many legacy loaders still use `JsonUtility`; legacy SaveSystem remains Unity-shaped | Migration is incomplete; do not use legacy loader patterns for new Core authority. |
| Determinism is a hard rule | Core weather uses `System.Random`; item instance uses `Guid.NewGuid` | Direct implementation violations remain. |

When a prompt touches a discrepancy, state whether it is a documentation correction, implementation migration, content canon decision, or compatibility bridge. Do not silently choose one.

## 29. Content Gaps

### Gameplay gaps

- Full Godot player loop and world presentation are thinner than the Unity implementation.
- Need/radiation/medical/shelter/work systems have many interactions in legacy code but not one proven Core/Godot authority.
- Late/base event distribution is uneven: base events are early-heavy, while late content is expansion-heavy.
- Expedition and combat/medical semantics differ between Core and Unity.
- Faction standing, economy, raids, quests, and endings are not demonstrably one connected graph.

### Narrative gaps

- Large catalog volume does not guarantee runtime access; some arcs and characters lack confirmed host paths.
- Faction aliases and real-world references weaken canon safety.
- Many locations and records can be expanded through consequences rather than more exposition.
- The transition from survival story to late faction-war/endgame story needs explicit player-facing progression.

### Content gaps

- Uneven faction depth and usage.
- Sparse/uncertain Black Flotilla and Century Seed presentation.
- Item categories with small counts—fuel, filters, power, water, anti-rad, ammo, and clothing—may need depth more than raw quantity.
- Some item IDs lack exact art; expected radio/voice clips are missing.
- Industrial, water, archive, medical, and transit locations are strong but unevenly used.

### Technical/UI/asset gaps

- Incomplete validator/CI alignment, stale reports, duplicate host abstractions, and broad save coupling.
- Dev-oriented UI and programmatic panels need integrated information architecture and campaign flow.
- Audio and asset resolver coverage is partial.
- Localization and accessibility are not proven systems.

## 30. Expansion Opportunities

These are opportunity surfaces, not full designs.

### Small

- Add missing item-art mappings for existing IDs and validate them in `AssetRegistry`.
- Add a focused weather/radiation UI warning with tooltips and save-safe state display.
- Give one underused location a linked event, resource, radio clue, and journal consequence.
- Add a migration/round-trip test to an existing expansion envelope.
- Add a faction alias validator that reports conflicting IDs without choosing a canon silently.
- Add missing audio fallback behavior for one existing radio/event family.

### Medium

- Connect water treatment, Hydro Baron standing, contamination, market price, shelter consumption, and aquifer quests into one Core flow.
- Extend medical care so injury, illness, radiation, caregiving, work efficiency, inventory, and survivor morale share visible consequences.
- Make one late faction chain use access, trade, raids, radio, journal, and ending flags rather than isolated text.
- Port a bounded Unity system such as a specific expedition or medical subsystem with parity tests and a thin Godot panel.
- Turn a data-rich location cluster—rail, hospital, archive, or industrial—into a reusable encounter region.

### Large

- Complete a canonical faction standing/identity layer and propagate it through catalogs, trade, quests, encounters, radio, icons, saves, and epilogues.
- Finish a coherent Year of Ash campaign path from Deep Freeze through Great Thaw with visible resource, faction, weather, radon, and ending consequences.
- Integrate Core survivor state, Utility AI, needs, medical, work, inventory, shelter, and narrative into a playable Godot day loop.
- Complete Black Flotilla as a full expedition/stealth/resource/narrative subsystem with host UI and persistence.

### Transformative

- Deliver the migration’s true source-of-truth architecture: Core simulation and data, shared save shapes, deterministic host adapters, full Godot campaign host, and Unity compatibility only where needed.
- Build a records-and-consequences meta-layer where census, evidence, radio, faction ledgers, survivor memory, resource decisions, and epilogue outcomes form one persistent history.
- Add generational succession only after ordinary survivor, save, and ending flows are stable; otherwise it risks multiplying unresolved identity and persistence problems.

### Depth versus quantity

More quantity is valuable for early/mid/late event coverage, missing item utility, and location usage. More depth is more valuable for medicine, water, shelter maintenance, faction standing, survivor relationships, and consequences. Avoid adding hundreds of disconnected items, names, or lore records.

## 31. Cross-System Expansion Opportunities

The following chains are high-value prompt context.

```text
Weather → fallout/black rain/cold → shelter shielding/air/heat → fuel/filter use
        → radiation/illness → medicine demand → work loss → morale/relationships
        → market prices and faction trade → quest/ending eligibility
```

```text
Water contamination → treatment/filtration capacity → clean-water rationing
        → survivor thirst/health → Hydro Baron standing and prices
        → Aquifer quest → radio/evidence/journal → faction access and ending flags
```

```text
Faction identity/standing → access and trade stock/prices → equipment availability
        → survivor assignment/ideology → encounter and raid selection
        → evidence, radio, journal → faction war → epilogue matrix
```

```text
Injury/illness/radiation → treatment items and caregiver assignment
        → work efficiency and shelter production → resource scarcity
        → morale/guilt/trauma → personal quest/relationship eligibility
        → journal/ending consequences
```

```text
Expedition stance/speed/stealth → stamina/noise/exposure → encounter selection
        → loot/depletion/contamination → inventory/crafting/trade
        → survivor condition → return timing and shelter event eligibility
```

```text
Greenhouse/crops → water/fuel/time/labor → food and morale
        → shelter capacity and seasonal risk → market/trade leverage
        → faction negotiations and long-term settlement outcome
```

```text
Records/census/evidence → access, legitimacy, and narrative truth
        → faction standing/quest branches → survivor memory and journal
        → Verdict/Epilogue Matrix → regional fate
```

```text
Maritime dive → air/noise/stealth → contamination/radiation/psychology
        → equipment wear and medical demand → loot/depletion and trade
        → Black Flotilla faction/lore → late-game access and endings
```

## 32. High-Risk Integration Areas

Before editing any of these, inspect the full dependency chain and tests:

1. `SaveSystem`, `SaveSystem.Wiring`, and any save envelope: broad graph, versioning, checksum, migration, host compatibility.
2. `GameBootstrap`, `Main.cs`, and bridge lifecycle: initialization order, duplicate subscriptions, dirty-save flush, UI ownership.
3. Needs/radiation/weather/shelter: tick order and coupled thresholds can change survival outcomes globally.
4. Inventory/item schema: IDs, stacking, equipment, art paths, crafting, trade, loot, and save shape.
5. Faction identity/standing: aliases affect nearly every authored system.
6. Event/quest/narrative catalogs: eligibility, flags, deterministic selection, journal/radio, and future branches.
7. Time and RNG: same-seed parity, day/hour boundaries, stable ordering, and random stream consumption.
8. Legacy-to-Core ports: semantic deviations are documented for expeditions and likely exist elsewhere.
9. UI panels/assets: a state change may require icon, tooltip, input, feedback, and save refresh changes.
10. CI and validation: stale Unity workflows can give false confidence about the active Godot path.

## 33. Authoritative File Map

### Project rules and migration

- `AGENTS.md` — active engineering, tone, migration, determinism, save, asset, and verification rules.
- `docs/GODOT_MIGRATION_STATUS.md` — migration direction and historical baseline; useful but stale in counts.
- `docs/CI.md` — validation commands and historical results; dual/stale sections require source verification.
- `README.md` — high-level overview, currently Unity-biased and stale.

### Core authority

- `Assets/Ashfall.Core/Ports.cs` — host ports.
- `Assets/Ashfall.Core/HostDefaults.cs` — default serializer, file I/O, clock, RNG, catalog location.
- `Assets/Ashfall.Core/SaveChecksum.cs` — canonical checksum behavior.
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — Core needs.
- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — radiation/dose.
- `Assets/Ashfall.Core/Inventory/` — inventory/equipment/item instances.
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` — crafting queue and station rules.
- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — market and barter.
- `Assets/Ashfall.Core/World/WeatherSystem.cs` — Core weather.
- `Assets/Ashfall.Core/UtilityAI/` — deterministic action scoring/veto rules.
- `Assets/Ashfall.Core/Narrative/` — narrative records, loaders, journal/quest-adjacent systems.
- `Assets/Ashfall.Core/Expeditions/` and `Maritime/` — travel, dive, scavenge, and expedition state.
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` — Century Seed slice.
- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` — endgame/chronicle evaluation.
- `Assets/Ashfall.Core/Encounters/OrphanKnockWhitelist.cs` — explicit orphan-event gate.

### Godot host

- `src/Main.cs` — current composition/UI/self-test host; high coupling.
- `src/Host/HostCli.cs` — headless/self-test dispatch.
- `src/Bridge/` — Unity compatibility/lifecycle shim.
- `src/Host/*Session.cs` — host adapters for expansion and domain slices.
- `src/Host/*SaveStore.cs` — Godot persistence adapters.
- `src/UI/` — reusable Godot UI helpers, overlays, theme and panels.
- `src/Economy/`, `src/Journal/`, `src/Muster/`, `src/YearOfAsh/`, `src/VerdictPanel.cs` — current panel/session areas.

### Data

- `Assets/StreamingAssets/Data/items.json` — base item catalog.
- `survivors.json`, `locations.json`, `recipes.json`, `events.json`, `radio.json`, `characters.json`, `faction_lore.json`, `world_history.json` — base content.
- `dynamic_questlines.json`, `questline_master.json`, `narrative_*.json` — quest/narrative surfaces.
- `door_encounters.json`, `faction_war_events.json`, `year_of_ash_*.json` — late/expansion content.
- `holdfast_*.json`, `duty_roster_*.json`, `standing_*.json`, `crossing_*.json`, `greenhouse_*.json`, `muster_*.json`, `dose_*.json`, `verdict_*.json`, `black_flotilla_*.json` — expansion catalogs.
- `Assets/StreamingAssets/Data/whitelists/orphan_knocks.json` — gated orphan encounter IDs.

### Legacy source and validation

- `Assets/_Game/Core/GameBootstrap*.cs` — Unity composition and initialization.
- `Assets/_Game/Core/SaveSystem*.cs` — legacy save graph.
- `Assets/_Game/Survivors/PersonalQuestSystem.cs` — large survivor/narrative manager.
- `Assets/_Game/Economy/DynamicEconomySystem.cs` — legacy economy integration.
- `Assets/_Game/Medical/MedicalSystem.cs` — legacy medical integration.
- `Assets/_Game/UI/` — Unity UI.
- `Ashfall.Core.Tests/` — Core tests and test result snapshots.
- `scripts/`, `tools/`, `.github/workflows/` — tooling/CI, with Unity/Godot alignment caveats.

## 34. Change-Safety Guidance

### Non-negotiables

- Keep authoritative simulation in `Assets/Ashfall.Core`; do not fork logic in Godot or Unity.
- Use ports for file I/O, logging, time, RNG, persistence, and serialization.
- Use invariant culture, seeded RNG, stable ordering, and deterministic instance IDs.
- Reuse existing snake_case IDs; discover the master list before adding references.
- Raise explicit events for state changes so UI and save coalescing can observe them.
- Make state serializable and migration-safe.
- Put AI-generated assets in `generated_AIassets/` and approved/runtime assets in the existing resolver path.
- Never launch Unity for this repository without an explicit user request.

### Before coding

1. Decide whether the task is creative design, data-only, Core implementation, host integration, UI, migration, debugging, or validation.
2. Read the authoritative Core system, data schema, host session, save store, tests, and relevant UI/assets.
3. Search for duplicate legacy implementations and existing IDs before adding anything.
4. Identify initialization/tick/event/save order and any cross-host semantic deviation.
5. Define exact verification commands and expected invariants.

### After coding

Use the smallest relevant scope: Core tests, data integrity, save round trip/migration, deterministic same-seed comparison, `dotnet build Ashfall.csproj`, and a targeted Godot headless/self-test. Report PASS/FAIL and unverified areas. Do not use an archived Unity result as current proof.

## 35. Task-to-Context Routing Map

| Prompt topic | Read first | Also trace |
| --- | --- | --- |
| Add a need | `NeedsSystem`, survivor DTOs | shelter/temperature, work efficiency, medical, UI, save, tests |
| Add radiation hazard | `RadiationSystem`, `WeatherSystem` | shelter shielding, worn gear, medical/items, dosimeter UI, save, deterministic RNG |
| Expand medicine/injury | Core medical/narrative ports and legacy `MedicalSystem` | needs/health, radiation, inventory, work, morale, traders/loot, quests, UI, persistence |
| Add item/equipment | `items.json`, Core Inventory, asset resolver | recipes, loot, trade, worn gear, UI icon/tooltip, save, data validator |
| Add recipe/crafting | `recipes.json`, `CraftingSystem` | stations, ingredients, durability/wear, overflow/stash, time, inventory, UI, save |
| Add economy/trade | `MarketSystem`, economy goods, trade panel/session | faction standing, inventory, prices/demand, barter/remainder, save, UI |
| Add faction | `faction_lore.json`, existing mappings | standing, quests, encounters, radio, trade, icons, locations, war, epilogue, save |
| Add survivor/NPC | `survivors.json`/`characters.json` and relevant expansion catalog | traits/profession, needs, work, inventory, medical, relationships, portrait, save, quest hooks |
| Add location | `locations.json` plus nearest expansion catalog | travel/radiation/danger, loot, events, faction control, assets/map, journal, save |
| Add quest/event | event/quest/narrative catalog and Core selector | flags, day/location/stance gates, choices/deltas, journal/radio, faction, ending, tests |
| Add weather/environment | Core `World`, legacy environment | needs, shelter heat/air, radiation, travel, gear, economy, UI warnings, RNG |
| Add shelter feature | `_Game/Shelter` plus Core shelter/expansion slice | capacity, power/water/air/heat, maintenance, work, events, UI layout, save |
| Add expansion | `ExpansionMasterSession` or nearest expansion host | Core DTOs, JSON, IDs, session, tick/event, panel, save migration, checksum, tests, assets |
| Modify save | target `*SaveStore`/envelope and `SaveChecksum` | all fields, version migration, old files, ordering, host parity, dirty-save wiring |
| Debug Godot host | `src/Main.cs`, `HostCli`, relevant session/panel | bridge gaps, lifecycle, log output, save flush, data path, self-test |
| Debug legacy behavior | relevant `Assets/_Game` system and `GameBootstrap` | Unity-only boundary, bridge behavior, duplicate Core rule, no Unity execution unless requested |
| Improve UI/UX | current panel/helper/theme and UI spec | Core events/state, input flow, asset mapping, accessibility, tooltip, save refresh |
| Generate art | `GAME_VISUAL_DNA.md`, `ASSET_TAXONOMY.md`, `PROMPT_RULES.md` | existing asset anchors, exact ID/path, generated_AIassets, runtime import/proof |
| Add audio | existing audio files/hooks | event trigger, mixer/routing, fallback, asset naming, Godot host integration |
| Optimize/test | project files, Core tests, CI docs, HostCli | actual active commands, deterministic result, data validation, historical-result caveats |
| Expand lore | `world_history`, faction/character/location catalogs, narrative docs | canon/implementation status, real-world violations, runtime links, journal/radio |

## 36. Context for Future Prompt Optimization

Use this file as a routing and constraint layer, not as a replacement for inspecting the authoritative files in the task’s dependency chain.

1. **Start with task type.** Separate brainstorming, design analysis, implementation, debugging, refactoring, audit, content authoring, asset generation, and verification. Each needs different output and evidence.
2. **Select only relevant context.** For a small item change, do not repeat the entire lore dossier; read item schema, inventory, crafting/trade/loot, asset resolver, UI, save, and tests.
3. **Trust implementation over stale prose.** Treat README, old migration baselines, old expansion plans, and archived test counts as historical unless confirmed against source.
4. **Preserve canon and uncertainty.** Never turn an inferred relationship or an unused catalog record into established lore. Mark assumptions and unresolved IDs.
5. **Prefer integration over replacement.** Mature Core systems, save envelopes, and catalog patterns should be extended unless evidence shows they are obsolete or broken.
6. **Name all downstream effects.** A vague request such as “expand medicine” can touch injury, illness, radiation, item definitions, inventory, crafting, traders, loot, survivor health, work, UI, persistence, quests, encounters, and balance.
7. **Keep the source of truth singular.** A new Godot-only mechanic that duplicates a Unity/Core rule is migration regression unless explicitly approved.
8. **Make verification part of the prompt.** Ask for exact files, expected invariants, Core tests, data validation, save round trips, Godot build/headless checks, and a PASS/FAIL report.
9. **Be model-neutral but output-specific.** A prompt for ChatGPT/Codex, Claude, Gemini, Qwen, or another model should still define scope, evidence to inspect, constraints, file targets, acceptance criteria, and uncertainty handling. Adapt formatting to the target model only after those facts are fixed.
10. **Require small reviewable deliverables.** One system or one bounded content slice per task is safer than a broad rewrite.

### Prompt conversion template

```text
Goal:
Task type: design | implementation | debugging | audit | verification
Authoritative files/data:
Current status and evidence:
Dependencies to inspect:
Canon/design constraints:
Save/determinism/ID implications:
Host boundary:
Files allowed to change:
Acceptance criteria:
Verification commands:
Known uncertainty:
```

## 37. Development Priority Candidates

These are candidate categories for future prompts, not a mandated roadmap.

### Critical correctness

- Replace gameplay `System.Random`/`Guid.NewGuid` with deterministic ports and stable IDs.
- Resolve Core/legacy duplicate clocks, buses, and `WornGear` ownership.
- Make malformed catalogs fail loudly with actionable validation rather than silently becoming empty.
- Establish a canonical faction ID/alias map and test all references.

### Architecture and migration

- Extract bounded systems from `Main.cs`, `GameBootstrap`, `SaveSystem.Wiring`, `PersonalQuestSystem`, `DynamicEconomySystem`, and `MedicalSystem`.
- Port one end-to-end subsystem at a time with parity tests, rather than broad rewrites.
- Reduce bridge semantic gaps and keep bridge code out of new Core logic.

### Testing and persistence

- Publish one current authoritative Core/Godot test count and result.
- Expand save migration/round-trip/cross-host tests for all stateful expansions.
- Add deterministic same-seed tests around weather, expeditions, procedural loot, markets, AI, and event selection.
- Align CI with Godot’s active build/test path while preserving separately scoped legacy checks.

### Gameplay/system depth

- Connect water, radiation, shelter, medicine, work, market, and faction mechanics.
- Complete a bounded Godot daily loop with visible consequences.
- Resolve expedition semantic deviations before using Core and Unity balance data interchangeably.

### Narrative/content

- Convert the strongest data-rich locations/factions/records into interconnected playable chains.
- Fill mid/late event density with meaningful consequences, not filler.
- Clarify named-character arcs and faction relationships.
- Finish or explicitly scope Black Flotilla, Century Seed, and endgame integration.

### UI/assets/audio

- Replace duplicate/monolithic host wiring with reusable state-driven panels.
- Close item-ID art gaps and establish missing-asset fallbacks.
- Make radio/audio event routing real in Godot or label it legacy-only.
- Add localization/accessibility decisions before UI surface area grows.

## 38. Open Questions / Uncertainties

1. Which host currently represents the intended playable campaign loop: the legacy Unity composition, the Godot dev hub, or an intermediate slice?
2. Which Core systems are officially adopted by Godot versus merely compiled/tested?
3. What is the canonical faction ID mapping across base, expansion, character, radio, quest, icon, and save catalogs?
4. Which 280 JSON files are loaded at runtime, which are fixtures, and which are planned/unused?
5. What is the intended canonical save format for the full game, and how will legacy Unity saves migrate?
6. Is the 365-day succession system a current target, an experimental Core port, or future expansion content?
7. Which endgame flags are reachable from the current campaign and which are test-only or authored future state?
8. Is Black Flotilla scheduled for a full host release or only a Core prototype?
9. What are the supported shipping platforms and input/accessibility requirements for Godot?
10. Is localization planned, and if so, should existing English JSON fields be treated as keys or source text?
11. Which faction/voice/audio assets are approved for use, and which are placeholders or proof images?
12. Are direct real-world references intentional alternate-history material or canon violations to remove?
13. Which archived test failures are still reproducible on the current branch?
14. Does `SaveChecksum` need to include private serialized fields, or is the public-field DTO rule absolute?
15. Which older Unity systems should be ported, retired, or left as legacy-only compatibility code?

## 39. Evidence / Confidence Notes

### Method

Evidence came from repository tree/config inspection, source and namespace inspection, JSON shape/count inspection, docs/audits, historical test artifacts, Git history, and targeted searches for serialization, randomness, TODO markers, UI, bridge, save, and asset references. Source and current data were prioritized over README/plans. No Unity process was launched. No game source, scene, configuration, data, or asset was changed for this dossier.

### Confidence levels

- **High:** project rules, engine/config files, directory/module locations, static source counts, JSON counts/shapes, direct calls such as `System.Random`, `Guid.NewGuid`, `JsonUtility`, and explicit Core/host class behavior.
- **Medium:** intended gameplay loop, design pillars, faction role interpretation, expansion integration status inferred from code plus docs, asset/audio coverage.
- **Low/uncertain:** complete playable campaign reachability, unused-vs-planned catalog records, exact cross-host parity, current runtime test pass state, localization/accessibility readiness, and canonical faction alias decisions.

### Snapshot caveat

The worktree was already dirty and continued changing during inspection. At final verification it showed user/concurrent edits in `src/Main.cs`, `src/Economy/TradeScreenGodotPanel.cs`, `src/Host/FactionIconLoader.cs`, and `src/Host/HostCli.cs`, generated Godot `.uid` files under `src/`, plus an untracked `Assets/UI/Textures/Backgrounds/` directory. These pre-existing/concurrent changes were not edited or reverted. Re-check those files before relying on line-level conclusions in a future task.

### Final operating rule

When this dossier conflicts with the repository, inspect the current repository. When the repository itself contains conflicting implementations, choose neither silently: identify the competing authorities, preserve compatibility where required, and ask for an explicit canon/architecture decision only when the task cannot safely proceed without one.
