# ASHFALL — ENGINEERING CODE INDEX (cheap-context reference)

> [!IMPORTANT]
> **CURRENT PROJECT AUTHORITY**
> 1. **Master Directives:** [`AGENTS.md`](../AGENTS.md) is the authoritative architectural and workflow rulebook. All AI agents and contributors must follow its non-negotiable rules.
> 2. **Core Domain Logic (Truth):** [`Assets/Ashfall.Core/`](../Assets/Ashfall.Core) is the single source of truth for simulation logic — 100% engine-agnostic C# with zero engine references.
> 3. **Data Authority:** [`Assets/StreamingAssets/Data/`](../Assets/StreamingAssets/Data) (JSON catalogs) is the absolute authority for definitions, balancing, quests, and economy.
> 4. **Authoritative Host & Runtime:** **Godot 4.7+ (.NET / C#)** (`src/`, `scenes/Main.tscn`, `project.godot`). The legacy Unity host (`Assets/_Game/`) and shim (`src/Bridge/`) have been fully retired and deleted.
> 5. **Verification Pipeline:** `dotnet test` (all unit tests passing, 0 failures) + `godot --headless` only.

> Load this file first. It is a distilled, dense map of the whole codebase so a
> fresh agent/session working on ASHFALL does not need to re-scan ~250K LOC.
> It records **where things live**, **what the key APIs are**, **the data
> schemas**, **the invariants**, and **how to verify**. For migration numbers,
> scenario/lore/expansion creative content, use the docs linked inline.

Path: `home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`

---

## 1. Project Architecture & Authority

| Path | Role |
|---|---|
| `Assets/Ashfall.Core/` | **Domain Logic (Truth).** Engine-agnostic plain C# domain systems, state DTOs, codecs, and validators. |
| `src/` + `scenes/` | **Godot Host (Active).** Thin presentation nodes, UI panels, input handling, and CLI dispatch. `scenes/Main.tscn` boots `src/Main.cs`. |
| `Assets/StreamingAssets/Data/*.json` | **Single Authority for Data.** 129 JSON catalogs loaded via Core serializers. |
| `Ashfall.Core.Tests/` | **xUnit Test Suite.** 3,244 unit, integration, determinism, and contract tests under net9.0. |
| `Assets/_Game/` & `src/Bridge/` | **REMOVED / RETIRED.** Unity host and shim migration is complete; legacy files deleted. |

`Ashfall.csproj` (Godot build, `Godot.NET.Sdk/4.7.1`) compiles `src/**` and `Assets/Ashfall.Core/**`.
Verification uses `dotnet build Ashfall.csproj` and `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.

**Migration to Godot is complete.** All active simulation logic lives in `Assets/Ashfall.Core/` (engine-agnostic C#), presented through the Godot 4.7+ host (`src/`).

**Engine policy (hard rules):**
- NEVER run/invoke Unity (batchmode/editor/playmode) unless the user explicitly asks.
- Verify with `dotnet test`, `dotnet build Ashfall.csproj`, `godot --headless`.
- `JsonUtility` is BANNED from core; serialize via `IJsonSerializer` port.
- Same seed ⇒ same sim: invariant culture, ordinal-sorted collections, `ISeededRng`.
- Versioned saves with checksum envelopes required on all save stores.
- One system per task; small reviewable changes.
- snake_case ids always; never invent an id not in the JSON master lists.

---

## 2. Verification (the gates)

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # core unit tests, 3,244 pass, net9.0
dotnet build Ashfall.csproj                                 # Godot host compile (0 errors target)
godot --headless --path "<root>" --quit-after 2             # boots, prints banner
# Per-subsystem selftest gates (exit != 0 = FAIL):
godot --headless --path . -- --expansions-selftest          # full 7-expansion suite
godot --headless --path . -- --data-integrity-selftest      # cross-ref every id across 55 JSON catalogs
godot --headless --path . -- --ice-road-selftest            # Holdfast S0 (21 checks)
godot --headless --path . -- --holdfast-save-selftest       # Holdfast S1
godot --headless --path . -- --brine-selftest               # Holdfast S2 salt&steam (21)
godot --headless --path . -- --cluster-selftest             # Holdfast S3 order 12-C (19)
godot --headless --path . -- --endings-selftest             # Holdfast S4 (11)
godot --headless --path . -- --year-of-ash-save-selftest    # Days 180-360 save roundtrip (19)
godot --headless --path . -- --duty-roster-save-selftest
godot --headless --path . -- --expansion-hub-save-selftest
godot --headless --path . -- --dose-ledger-selftest
godot --headless --path . -- --journal-selftest / --journal-uitest
godot --headless --path . -- --bridge-selftest              # shim failure policy
godot --headless --path . -- --caravan-selftest
```

Full flag list + exit codes in `src/Host/HostCli.cs` (the CLI dispatcher).

---

## 3. Core infrastructure (in `Assets/Ashfall.Core/`)

### Ports (`Ports.cs`) — engine-agnostic seam
- `IJsonSerializer` `Serialize<T>`/`Deserialize<T>` (JsonUtility banned).
- `IFileIO` `DirectoryExists/FileExists/ReadAllText/WriteAllText/Combine`.
- `ILog` `Info/Warn/Error`.
- `IClock` `Day/AdvanceDays/SetDay` (never `DateTime.Now`.
- `ISeededRng` `Seed/Next/NextFloat/NextDouble`.

### Defaults (`HostDefaults.cs`)
`SystemTextJsonSerializer`, `FileSystemIO`, `SimClock`, `SeededRng`, `ConsoleLog`
(+ `NullLog`). `CatalogLocator.TryFindDataDirectory` resolves the JSON dir;
`CatalogLocator.UseInvariantCulture()` mandatory before any save/parse.

### Core systems (`Assets/Ashfall.Core/` top-level)
- `CensusClaimSystem`, `VouchAccessSystem`, `VoluntaryRegisterSystem`, `SickListSystem`
- `PhantomMemoryEngine` (Antigravity #41 — scavenged-item memory)
- `CatalogIntegrityValidator.Validate(dataDir, IFileIO)` → report with `Errors/Warnings/Summary/ExitCode/IsClean`
- `SaveChecksum`, `WeatherKind`
- **Holdfast (4-storey):** `Holdfast{Session,Save,SaveFrozen,QuestSystem,Endings,Catalog}`, `IceRoadSystem`, `BrineWaterSystem`, `LedgerDebtSystem`, `WaystationSystem`, `Crossing{Session,Catalog,ArbitrationSystem}`
- **Expansions:** `DutyRoster/{DutyRosterSystem,Catalog,MoraleMarkSystem,ShelterEncounterSystem}`, `Muster/{MusterSystem,QuestApproach}`, `StandingRecord/{LocationLayout,LocationMemory,SiteEncounter,StandingRecordCatalog}`, `Journal/{JournalSystem,Entry,Voice,KnowledgeBase,RiskBiasTrait}`, `Greenhouse/{GreenhouseSystem,ExpansionCatalog}`, `TravelingCaravanSystem`
- **Year of Ash (Days 180–360):** `YearOfAsh/{TimelineSystem,DeepFreezeSystem,RadonSystem,FactionWarSystem,QuestlineSystem,DoorEncounterSystem,YearOfAshSave}`
- **Wasteland Map & World Navigation:**
  - **Data Authority:** `Assets/StreamingAssets/Data/wasteland_map.json` and `world_regions.json`
  - **Catalog Loader:** [`Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs`](../Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs) — Loads 99+ wasteland nodes, normalized coordinates `(X, Y)`, sectors, hazard profiles, and route edge connectivity.
  - **Core System (Truth):** [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../Assets/Ashfall.Core/World/WastelandMapSystem.cs) — Manages 4-state marker progression (`Locked`, `Available`, `Discovered`, `Complete`), traversal cost calculation, route unlocks, and `WastelandMapState` snapshot DTO.
  - **Godot Host Session:** [`src/Host/WorldHostSession.cs`](../src/Host/WorldHostSession.cs) — Lifecycle adapter managing map instance, sector hazard tracking, and coordinate mapping.
  - **Save Store:** [`src/Host/WastelandMapSaveStore.cs`](../src/Host/WastelandMapSaveStore.cs) — Persists to `user://saves/save_wasteland_map.json` using versioned codec and SHA-256 state checksum envelope (registered as `"wasteland_map"` in `AllSaveSections`).
  - **View Layer & UI:**
    - [`src/World/WastelandMapView.cs`](../src/World/WastelandMapView.cs) & [`src/World/MapLocationMarkerView.cs`](../src/World/MapLocationMarkerView.cs) — Interactive 2D viewport map surface.
    - [`src/UI/MapAtlasPanel.cs`](../src/UI/MapAtlasPanel.cs) — Fullscreen interactive modal atlas panel with discipline/sector filters, marker state legend, route inspector, and location details.
    - [`src/UI/MapPanel.cs`](../src/UI/MapPanel.cs) — Shelter HUD overview panel with sector indicators and marker state legend.
  - **Verification:** `dotnet test --filter WastelandMap` + `godot --headless --path . -- --world-selftest`.
- Every system: raises events on state change, and has `CaptureState()`/`RestoreState()` returning a `[Serializable]` primitives-only snapshot. Effortless save/load + deep-copy snapshot isolation (never alias the live state into CaptureState).

### Unity-side core infra (`Assets/_Game/Core/`) — the wider dependency
- `GameState.cs` — `enum GamePhase {MainMenu,Running,Paused,GameOver}`; `Day,IsPaused,AccessibilitySafeMode`; `OnPhaseChanged`.
- `TimeSystem.cs` (+`TimeSystemSave`) — `_hourAccumulator`; day=1 base; `CurrentHour`, `TotalElapsedHours`, `SetTimeScale`, `Tick`, `TickHours`, `SetTime`, `SetElapsedHours`, `CaptureState/RestoreState`.
- `EventBus.cs` — static `Subscribe<T>/Unsubscribe<T>/Raise<T>/Clear/SubscriberCount`. Generic (no per-type boilerplate); snapshot-based invocation.
- `SystemRegistry.cs` — `Register<T>/Get/TryGet/RegisterPerSubstep/RegisterDaily/RegisterEventDriven/RegisterSaveOnly/DayGated/TickAll`; the system lifecycle hub.
- `SaveSystem.cs` (+`SaveSystem.*` partials, ~966 lines) — `Register(ISaveable)`, `RegisterSaveable<T>`, `Delete(slotId)`, `SetWorldFlag/GetWorldFlag`; JSON slot persistence.
- `ISaveable.cs` — `interface {object CaptureState(); void RestoreState(object);}` with `[Serializable]` public-fields rule.
- `WorldPhaseSystem.cs` — day-gated world phase config.
- `SystemRegistry.cs` note: `DayGated(key, tick)` is the standard "run once per day" helper.

---

## 4. Domain model (Unity `Assets/_Game/`, the source being migrated)

### Survivors (`_Game/Survivors/Survivor.cs` 1140 lines + siblings)
- `Survivor : Ashfall.Core.Journal.ISurvivorAuthor`; `SurvivorState`, `RadiationSicknessPhase`, `MoralBranchDirection`, `ChemicalDependencyKind`, `PhilosophicalStance` enums.
- Rich lists: `Traits`, `Traumas`, `DisabilityIds`, `AtrophiedSkills`, `ConsumptionHistory`, `GuiltRecord`, `ChemicalDependency`, `TraumaBondRecord`, `HiddenFoodHoard`, `SleepwalkIncident`, `ManifestoLaw`, `CampaignMilestone`, `HiddenItemIds`, `KeepsakeItemIds`.
- **Needs.cs** — `enum NeedKind {Hunger,Thirst,Fatigue,Warmth,Morale,Health,Hygiene}`; `Needs` floats each clamped 0..100 (Warmth=100, Morale=75, Health=100, Hygiene=100 defaults) + `Was*Critical` edge flags + `ClampAll()`.
- `NeedsSystem.Tick(survivor, gameHours)`; `Modify(survivor, need, delta)`, `ForceDeath`, `SetHealth/AdjustHealth`, `NotifyNeedsRestored`. Raises events on critical transitions (edge-detect).

### Radiation (`_Game/Radiation/`)
- `RadiationSystem.cs` — dose accumulation, iodine/anti-rad, chronic illness.
  - `ComputeGearProtection(worn)` = Σ `piece.EffectiveProtection()`.
  - `ComputeEffectiveAmbient(zone, shelterShielding)` = `max(0, zone − max(0,shield))`.
  - `ComputeExposurePerHour(zone, gear, shelter)` = `max(0, zone − gear − shelter)`.
  - `ComputeContaminationAmbient(contams)` = Σ ambient contribution (brought-inside items).
  - `Expose(survivor, radsPerHour, hours)`, `GetDosimeter(id)`, `AdministerIodine`, `AdministerAntiRad(radsRemoved)`, `SetDose`, `AdjustDose`, `SeedLifetimeExposure`, `ExaminePrognosis`.
- `Dosimeter.cs` — `SurvivorId`, `Record(exposure, gameHours)`, `Reset()`.
- `Contamination.cs` — `RadsPerHour, DecayPerHour, IsActive`, `Decay(gameHours)`, `AmbientContribution()`.
- `WornGear` — `RadProtection, Max/CurrentDurability, DegradeRate`, `EffectiveProtection()` (× durability fraction), `Degrade(hours)`.
- `ProtectiveGear` (SO), `ExposureContext`, `DeviceState` (battery/calibration/broken), `RadZoneProfile` (SO), `GeigerCounter`, `InstrumentDevice`, `AfflictionPipeline`, `PrognosisPipeline`, `RadiationPhaseProgression`.

### Environment & Weather (`_Game/Environment/`)
- `WeatherSystem` + `WeatherState` + ~30 `Weather_*.cs` (fallout storm, deep freeze, EMP, acid snow, blood rain, glass storm, solar flare, ozone hole, silent spring…).
- `FalloutMap` (spatial dose), `FalloutForecastSystem`, `TemperatureSystem` (nuclear-winter cold → Warmth), `MapGenerator`, `GeneratedMap`, `MapNode`, `MapTile`, `RiverNodeSystem`, `DangerRing`, `RadiationKnowledgeMap`, `AshDriftBurial`, `AshAccumulationSystem`, `OzoneScourge`, `PhotoperiodSystem`, `WildlifeMigration`, `AcousticEcology`.

### Shelter (`_Game/Shelter/`)
- `Shelter` aggregate — `AddModule/RemoveModule/GetModule/GetRoom/GetRoomIds/NotifyModuleUpgraded`.
- `Shielding` — `Level`, `AttenuationFactor = Clamp01(Level*0.15f)`, `Upgrade()`.
- `AirFiltration` — `FilterHealth`, `FiltrationEfficiency = Clamp01(FilterHealth/100)`, `Tick`, `ReplaceFilter`.
- `PowerNetwork` (765 lines) — consumers/sources, priority, pedaler bikes, diesel fuel, CO monitoring, `CreateDefault(diesel=40)`, `Rebalance(weather)`, `ApplyToShelter`, `CaptureState/RestoreState`.
- `ThermalGridSystem` — per-room `ThermalRoomState` (°C), pipe burst/freeze events, heaters, `Tick(gameHours, needs, survivors)`, `RepairBurstPipes/ThawPipes`.
- ~70 `Modules/ShelterModule_*.cs` (turret, vault door, decon shower, dialysis, greenhouse grow light, treadmill gen, drill…); each `ShelterModuleSO` + `ShelterModuleInstance`.
- `HatchDefenseSystem` (+ raid/siege/repel/breach partials), `StructuralIntegritySystem`, `WaterStorage`, `Vermin`, `Waste`, `NoiseSystem`, `Tunneling`, `Excavation`.

### AI (Utility AI, no LLM at runtime) (`_Game/AI/`)
- `UtilityAI` — should-evaluate gating; `SelectAction(context, candidates)`.
- `AIContext` — holds survivor/shelter/inventory/rng + a richly exposed world snapshot.
- `SurvivorAction` (abstract SO) — `responseCurve` (AnimationCurve), `abstract EvaluateRaw(context)`, virtual `Execute`.
- `ActionScorer` — `Score(action, context)`; classifier helpers: `IsComfortOrTalkAction`, `IsWeaponOrCombatAction`, `IsGunAction`, `IsOrderAction`, `IsPhysicalLaborAction`, `IsSurfaceSkyAction`.
- ~90 `Action_*.cs`/`*ActionSO.cs` (scavenge, treat patient, mercy, harvest organs, tattle, sterilize, chems, demolition, propaganda…).
- `HallucinationSystem`, `SurvivorAction.cs`, `ActionScorer.cs`.

### Inventory (`_Game/Inventory/`)
- `ItemType`, `ItemDefinition`, `Inventory` (stacked container), `EquipSlot/EquipSlots`, `FieldGearLoadoutSystem`, `OverflowCrateSystem`, `ProceduralItemInstance`, `ScrapValue`, `Item_TradeValues`.
- ~90 `Items/Item_*.cs` — dosimeter, geiger counter, iodine, potassium iodide, prussian blue, anti-rad, gas mask, hazmat suit, water tabs, vitamins, keycards, C4, landmine, EMP grenade, exosuit, night vision, teddy bear, guitar, photo album, foreign book, etc. `Item_WorldCatalog(+Expanded/Loot)`.

### Factions (`_Game/Factions/`)
- ~70 `NPC_*.cs` state classes (Charwith named NPCs: Ansel Duth, Dessa Vane, Edor Vale, Hadi Morrow, Ivo Fenn, Kess Adler, Len Quill, Mattis Cray, Nila Brant, Osran Kell, Perrin Ashby, Tamsin Rook, Wyn Sabler, Yara Holm, Tally, Undertow…) + `Visitor_*`, `Fauna_*`.
- Faction systems: `FactionIntelligenceSystem`, `PeaceTreatySystem`, `ScavengerRefugeSystem`, `GarrisonConscriptionSystem`, `AshSignCultSystem`, `BureaucraticFrictionSystem`.

### Events/Narrative (`_Game/Events/`, `_Game/Data/`)
- `EventRunner` (+ Selection/Apply/Tick/Factories/Holdfast/Journal partials), `GameEvent`, `ScheduledEvent`, `EventContext`, `FactionLockoutEngine`, `SuspicionTracker`, `IntelReliability`, `MoralChronicleEntry`, `RadioDarkPuzzleSystem`, `BunkerMicroNarrativeSystem`.
- `Data/` holds ScriptableObject catalog classes + JSON catalog loaders (`CharactersCatalogLoader`, `Crossing*Loader`, `Holdfast*Loader`, `DynamicQuestline*`, `GreenhouseItems*`, `SurvivorCatalogSO`…). Unity importer lives in `_Game/Editor/` (menu `Tools/ASHFALL/...`).

### Crafting / Quests / Endgame / Simulation
- `Crafting/` — `CraftingSystem`, `Recipe`, `CraftingStation`, `WorkbenchSystem`, `NewRecipesCatalog`.
- `Quests/` — `DynamicQuestlineSystem`, `QuestRegistry`, `QuestRuntime`, and named `Quest_*`.
- `Endgame/` — `Victory_*.cs` ×19 (The True Ending, The Cure, MAD, The Broadcast, Underground City, Unifier, Migration…) + `Project_*.cs` ×6 (BioReactor, DeepWell, Elevator, Minecart, RadioArray, SurfaceDome).
- `Simulation/` — `SimulationSystems` (+Core/Medical/Ops partials), `CalorieAccountingSystem`, `WaterRationingMutinySystem`.

### The wiring behemoth: `Core/GameBootstrap.*`
~70 partials (`GameBootstrap.cs` + `.InitFoundation`, `.InitializeSystems`, `.Expansions3to4Wiring`, `.Holdfast`, `.DutyRoster`, `.Hatch`, `.Weather`, `.MapHazards`, `.Missions.*`, `.TickSystems.*`, `.UiActions.*`, `.RadiationExposure`, `.ShelterLayout`, `.StandingRecord`, `.NobodyCharter`, `.VictoryPaths`, `.InternalHorror`…). This is Unity-monobehaviour wiring — NOT to be ported; each subsystem host in Godot replaces a slice.

---

## 5. Godot host (`src/` + `scenes/`)

- **Entry:** `scenes/Main.tscn` → `src/Main.cs` (`Main : Control`).
- `Main.cs` (1602 lines): `_Ready()` → resolve data dir, parse CLI, build UI/menus, setup all host sessions, restore saves; `_Process` pumps the Unity shim (`BridgeRuntime.Tick`), throttled diagnostics + **dirty-flag save coalescing** (one file write per burst); `_UnhandledKeyInput` J toggles journal book, Esc closes; `_Notification(WMCloseRequest)` flushes all saves then `BridgeRuntime.Shutdown()`.
- **Sessions/SaveStores** (thin pattern): each subsystem gets `*HostSession` (`Create(dataDir)`, `.StateChanged`, `*Line()` display strings, `CaptureSave()`) + `*SaveStore` (`TrySave(save[,path])`, `TryLoad([path])`, codec + checksum). Existing: `WorldHostSession` + `WastelandMapSaveStore`, `Holdfast{SaveStore,BriefingView}`, `CoreDemoSession`, `DutyRoster`, `ExpansionHostSession`+`ExpansionHubSaveStore`, `PhantomMemory`, `DoseLedger`, `YearOfAsh`, `Journal`, `Combat`, `Medical`, `Survivors`, `Economy`, `SilentFoundry`. Located `src/Host/` + `src/Journal/` + `src/YearOfAsh/`.
- **UI widgets & modal atlas panels:** `Journal/JournalBookUI.cs` (544 lines), `UI/MapAtlasPanel.cs`, `UI/MapPanel.cs`, `World/WastelandMapView.cs`, `World/MapLocationMarkerView.cs`, `UI/ResearchAtlasPanel.cs`, `UI/EventsLogPanel.cs`, `UI/CombatHistoryPanel.cs`, `YearOfAsh/DoorEncounterModal`, `QuestlineModal`, `FactionWarMapWidget`, `GeothermalHeatingWidget`, `RadonVentilationWidget`, `RadioBroadcastTerminal`, `HoldfastBriefingView`.
- **Bridge Removal:** The legacy `UnityEngine.*` bridge shim (`src/Bridge/`) has been completely removed. `--bridge-selftest` verifies absence and exits 0.
- `src/CSharpVerificationTest.cs`.

---

## 6. Data authority — `Assets/StreamingAssets/Data/` (JSON, ~55 catalogs)

Read by both engines. **Single source of truth — never fork.**

| Catalog | Count | Key fields |
|---|---|---|
| `items.json` | 477 | `id, displayName, description, type, stackMax, weight, radProtection, durability, contamination, hungerRestore, thirstRestore, healthEffect, radCleanse, moraleEffect, isEquipable, equipSlot, tradeValue, empShielded` |
| `survivors.json` | 102 | `id, displayName, profession, bio, baseHealth` |
| `locations.json` | 99 | `id, displayName, description, dangerLevel, travelHours, baseRadsPerHour` |
| `events.json` | 77 | `id, title, bodyText, weight, minDay` |
| `characters.json` | 32 | `id, display_name, profession, bio, faction, region, first_day, location_id, wants[], offers[], will_not[], signature_quote` |
| `holdfast_{factions,items,locations,quests}.json` | — | Holdfast saga content. Quest: `id, display_name, type, briefing, prereq_quest_id, min_day, knowledge_key, target_location_id, stages[]` |
| `crossing_*`, `duty_roster_*`, `standing_record_*`, `greenhouse_items`, `year_of_ash_*`, `currents`, `echoes`, `world_history`, `faction_lore`, `recipes`, `radio`, `phantom_triggers`, `final_wishes`, `door_encounters` (96KB) | — | expansion content |

Unity data flow (legacy): JSON → `Tools/ASHFALL/Import All Data` editor importer → ScriptableObject → `Generate Catalogs` → catalog asset → systems.

---

## 7. Tests

- `Ashfall.Core.Tests/` (33 `.cs`, xUnit) — **the non-Unity suite**. Covers all ported core systems + `CatalogIntegrityValidatorTests`. Run: `dotnet test`.
- `Assets/Tests/EditMode/` + `PlayMode/` — Unity-only (banned to run).

---

## 8. Active work / state (as of last check)

- **Branch:** `cursor/phase11-expansion-ui-integration` (feature branch, continue it).
- **In-flight (uncommitted):** Godot host sessions/save stores (DoseLedger, DutyRoster, Expansion, PhantomMemory, YearOfAsh), modified core systems + tests from audit Loop work, `tools/audit_loops.sh`. Commit after accepted deliverables.
- **Expansion 06 "the Muster" (Days 180–360)** is the active content target per `docs/MUSTER_INTEGRATION_PREP.md`: add Coastal Hydro-Barons (15th current) to `currents.json` (+ `the_coast` region + badge), `NPC_HydroBarons.cs` + 5 other `NPC_*.cs`, 6 new locations, 8 quests, new items/events.
- **Audit loop history:** `docs/DEBUG_LOOPS_LOG.md` (Loop 0: 884→0 data errors; Loop 1: determinism + save/load snapshot-isolation + ling).

### Recent save-envelope versions (cross-host, checksummed)
Saves are versioned envelopes with `saveVersion`, a `Checksum`, and vN→vN+1 chain migrations validated against frozen old shapes. Current: `HoldfastSave` (v3), `YearOfAshSave` (v2), `DutyRosterSave`, `ExpansionHubSave`, `DoseLedgerSave`. Every hosted save store round-trips + rejects tampered files. **Checksum** lives in core (`SaveChecksum`) / codec.

---

## 9. Golden rules recap (from AGENTS.md / CLAUDE.md)
1. Godot is the ACTIVE engine; never run Unity unless asked.
2. One source of truth: logic → `Ashfall.Core`, data → StreamingAssets JSON.
3. Verify before claiming done: `dotnet test` + `dotnet build` + `godot --headless` gates. Report PASS/FAIL.
4. Save/load safe + events per public system; primitives-only state.
5. snake_case ids; no invented ids; no magic/fantasy/real-world events/glorified violence.
6. Tone: cold, exhausted, human, restrained — show, don't preach.
7. AI assets → `generated_AIassets/` (game root).
8. Small reviewable changes; ≥2 new coupled variables ⇒ have a different tool review the code (diff + spec only, never the implementer's narrative).

---

## 10. Quick navigation cheat-sheet

| You need... | Go to |
|---|---|
| Add a subsystem host | `src/Host/` (copy `ExpansionHostSession`+`SaveStore` pattern) |
| Port logic to core | move `Assets/_Game/X.cs` → `Assets/Ashfall.Core/`, remove Unity usings, add ports |
| Add a selftest gate | `src/Host/HostCli.cs` (+ enum, parse, method, `Main.cs` switch) |
| Change JSON catalog data | `Assets/StreamingAssets/Data/` (then `--data-integrity-selftest`) |
| Fix a save/load bug | look at `CaptureState`/`RestoreState` + save envelope version + checksum |
| Determinism | ordinal-sort collections before emit; invariant culture; seeded RNG |
| Understand a subsystem's API | `bit schema`/`bit show` for Bit components; else read the `*.cs` directly |
| Expansion creative docs | `docs/expansions/`, `docs/lore/` |
---

## 11. DEBUGGING & PROBLEM-SOLVING RUNBOOK

> The game's debugging intelligence lives in the docs below; this section distills it so a fresh
> agent can diagnose/fix without re-deriving the lessons. **All audits processed here operate on
> the Godot-side gates (`dotnet` + `godot --headless`) — Unity is never run.**

### 11.1 The standing rule (most important)
> A green build across `_Game` proves the shim's **surface** is complete, NOT its **behaviour**.
> Until a system is instantiated and exercised, treat its port status as **unproven**.
> (Final, reaffirmed line from `docs/ASHFALL_DEEP_CODE_AUDIT_2_2026-08-14.md`.)

This is the failure mode that makes "everything compiles" a trap. If you port something and it
"just works" with no system instantiated, you have NOT proven anything — you proved it type-checks.

### 11.2 The bridge failure policy (how the shim lies loudly)
`src/Bridge/BridgeGap.cs` classifies every unimplemented `UnityEngine` shim member into 3 buckets:
- **Semantic** — silence would make game logic wrong ⇒ `throw NotImplementedException` (default).
- **Cosmetic** — audio/visual only, absence expected headless ⇒ log once, continue.
- **Genuinely inert** — doing nothing IS correct headless (Input.GetKey, isEditor, Destroy) ⇒ no-op.

- `BridgeGap.ThrowOnSemanticGap` (default `true`). Set `false` → newly-wired systems sweep and
  collect every gap via `BridgeGap.Reported` in ONE run instead of crash-per-gap. This is the
  intended tool for "which holes does this path hit".
- Semantic member string format: `join 8: '{member}' is not implemented. {consequence} Implement it in src/Bridge/...`.
- Gate: `--bridge-selftest` (currently 41 checks) asserts semantic members throw, cosmetic stay
  quiet, inert keep answering, reported-once-per-member. Prevents silent regression to plausible defaults.

### 11.3 The dominant bug class: cross-host serialization / determinism
Audit Loop 1 + audit#2 C2. When anything behaves differently between hosts, suspect these first:

| Symptom | Root cause | Fix pattern |
|---|---|---|
| Save loads one host, rejected as corrupt in other | **Cross-host checksum mismatch** (see 11.4) | `SaveChecksum` hashes *state* (reflection, ordinal field-name order, invariant self-delimiting), NOT serialized text |
| Simulation differs between hosts for same seed | Dictionary/HashSet **iteration order** in serialization path | **Ordinal-sort** collections before emit |
| Numbers parse/format differently | culture-sensitive `Parse`/`ToString` | `UseInvariantCulture()` first; never `DateTime.Now` |
| `null` vs `""` / `null` vs `[]` after load | JsonUtility vs System.Text.Json parse differences | checksum normalizes null-string ≡ ""-string, null-collection ≡ empty |
| Value deserializes into a case you have no enum for | **Enum ordinal drift across forked copies** (e.g. RiskBiasTrait had 8 Unity / 6 Godot members) | single source in Core, append-only enums, cross-match test |

**Enum-order gotcha (audit#2 C3):** a same-name-different-namespace duplicate type compiles fine
(same-assembly, namespace picks local copy) and only surfaces as behavioural divergence — ordinal
persisted values map to wrong/unhandled cases. `MercyKillActionSO` branched on `Sociopath`/`Empath`
which the Godot copy lacked. **Watch for silent duplicates; dedupe in Core.**

### 11.4 The save checksum design (C2 fix — read before touching saves)
Old: SHA256 over serialized JSON text → broke cross-host (indent width, key order, null handling).
New (`Assets/Ashfall.Core/SaveChecksum.cs`): computes over the **object state** —
reflection walk over public instance fields, **ordinal name order**, values written in a
self-delimiting invariant-culture form. Independent of serializer.
- Save: `snapshot.Checksum = SaveChecksum.Compute(snapshot)` then serialize ONCE.
- Load: verify state-based first, fall back to `VerifyLegacyTextChecksum` so old saves still open.
- 18 xUnit tests (`SaveChecksumTests.cs`): CrossHostRoundTripAgrees, culture independence,
  field-swap detection, delimiter forgery, reference-cycle guard.
- How to verify a real *load* path: the hosted `*SaveStore.TryLoad` round-trips through the temp
  path in each `--*-save-selftest`; a tampered file (flip a field in raw text) must be **rejected**.

### 11.5 Save/load correctness rules (from loops + AUDIT-004/005)
- `CaptureState()` must return a **fresh deep copy** — never alias the live state (was a real bug:
  PhantomMemoryEngine + DoseLedgerSystem returned live state → edits corrupted the running sim).
- Capture-to-save and restore-to-state must be **fully transactional** OR fail-fast:
  `SaveSystem.DefaultFailFastRestoreForEnvironment()`; partial restore leaves hybrid world state.
- **Never** empty-catch a corrupt save silently — log path + exception type (was silent `(false,null,null)`).
- Restore scopes that broke before save-scumming: every system a session ticks must be in the
  envelope or it silently resets on reload (YearOfAsh v1 famously dropped deep-freeze thermal,
  radon scrubber wear, alpha dose, and questline progress; v2 added them).

### 11.6 Determinism checklist (before claiming "same seed same sim")
- Ordinal-sort all collections (Dictionary keys, HashSet, parents/room-ids) at emission.
- Invariant culture for all numeric parse/format in save paths (Loop 4: 0 violations).
- Seeded RNG via `ISeededRng`/`SeededRng` sharing; bridge `Random.InitState` exists (H2 fix) for
  the 4 `UnityEngine.Random` sites; prefer Core RNG.
- No `DateTime.Now`; use `IClock` (banned in `Ports.cs`).
- Reference-cycle guard in checksum (defensive).

### 11.7 The host lifecycle pump (H1) — why Unity behaviours "do nothing"
The bridge's `MonoBehaviour` originally had no lifecycle. `BridgeRuntime.Tick` now dispatches by
**reflection** (cached per type): `Awake→OnEnable→Start→Update→coroutines→LateUpdate`, and
`OnDisable/OnDestroy` on teardown + opt-in `FixedTick`. Non-virtual magic methods (`private void
Update()`) are why reflection is required. `CoroutineRunner` drives iterators honoring `null`,
`WaitForSeconds` (scaled), `WaitForEndOfFrame`, nested coroutines. Hook exceptions log once per
(type, hook) and continue (Unity-style), not 60×/sec frozen stack traces.
- `Main._Process` pumps it; `_Notification(WMCloseRequest)` calls `Shutdown` (runs OnDisable/OnDestroy).
- Bug symptom to know: a MonoBehaviour whose `Awake`/`Update` "never fires" ⇒ bridge pump not ticking.

### 11.8 The `Time` audit (M1/M2) — fake vs real clock
`Time.deltaTime` was hardcoded `0.016666f` (a constant, 9 uses) and `timeScale` set but never read.
Now `Time.AdvanceFrame` uses the real frame delta × `timeScale`; `unscaledDeltaTime` is raw.
Pause(0)/slow-mo verified by `--bridge-selftest`. Any accumulator that behaved like it ran at a fixed
60fps regardless of real time traced back to these.

### 11.9 Known hollow/duplicate surface (audit#2, still-relevant watch-list)
- **Duplicated types now unified in Core** (8/9 done): `RiskBiasTrait`, `KnowledgeBase(+Keys/Save)`,
  `JournalEntry`, `JournalVoice`, `JournalSystem` (+thin Unity subclass), `ISurvivorAuthor`,
  `HoldfastLocationEntry`, `IceRoadSystem` (delegating adapter, 0 dup logic).
- **Still deliberately forked: `JournalCodex`** — Unity renders live survivor status vocabulary,
  Godot emits literal `"survivor"`; unifying needs the survivor-status domain ported to Core. Do NOT
  auto-merge (would silently downgrade Unity text).
- **Unclassified bridge members (deliberately deferred, none on runtime path):** `UnityEditorBridge`
  (25, editor-only), `UnityEngineGUI` (25, legacy IMGUI never pumped), `UnityEngineUIElements` (14).
- **Hollow risk model:** ~13.5% of 820 bridge members were hollow (79 empty bodies + 32 `=>default`).
  Fixed for runtime files via BridgeGap. Any new hollow member you add is a latent silent bug.

### 11.10 Open/known issues worth knowing (audit#2 priorities + ISSUE_REGISTER)
- **NoWarn mask in `Ashfall.csproj`** suppresses CS8618/8602/8603/8604 + 8 more — nullability problems
  are *masked, not resolved* (~171 warnings even with mask). Tread carefully reading nullable code.
- **`Camera.main => null`** unconditionally — any `Camera.main.X` is a guaranteed NRE if run.
- **`M3` CS0649 ×304** — JSON DTO fields never assigned in code (deserializer fills them). This is
  BY DESIGN — but it is the exact signature a catalog-loading regression hides behind; guard with a
  non-empty catalog-load test.
- **DUAL TICK RISK:** `SystemRegistry` + `SystemWiring.WireDaily` + explicit `GameBootstrap.TickSystems`
  coexist → risk of double-registration. Assert no double tick during port.
- **Static `EventBus`** lives across domain reloads; clear on bootstrap/new game.
- **Perf:** Utility AI scales survivors×actions (no mask/spatial split); JsonUtility double-serializes
  for checksum (now state-based in core); day-tick fans out 30+ systems unbounded.
- **Balance:** last 100-campaign sweep (30 sim-days, artificial rad/thirst drain) → mean days survived
  2.53, all 300 deaths from radiation_overdose (200) + dehydration (100). Artificial harness, not final tuning.

### 11.11 The diagnostic audit loop (how to sweep for bugs, `tools/audit_loops.sh`)
Runs `N` loops; **identification only, no fixes**. Per loop:
1. Build → unique `error CS`.
2. Core tests → last Pass/Fail line.
3. `NotImplementedException` outside `BridgeGap.cs`.
4. Empty `catch {}` blocks.
5. `CaptureState` without a matching `RestoreState` in the same file.
6. Hard-coded id literals in `src/` not present in any `StreamingAssets/Data/*.json` nor Core
   (with a benign-id allowlist: `font_*`, `margin_*`, `trait_*`, host flags, etc.).
7. `obj/`/`bin/`/`.godot/` leaks in git.
8. Selftest sweep: expansions, year-of-ash, duty-roster, expansion-hub, journal, holdfast, bridge.
Output appended to `/tmp/ashfall_audit_master.txt`; closing summary counts unique compile errors
and test-fail lines. Audit loop history + findings in `docs/DEBUG_LOOPS_LOG.md` (Loops 0–6, 3
consecutive zero-finding loops = convergence).

### 11.12 The audit doc lineage (read order for a deep dive)
| When you need | Read |
|---|---|
| Current status of everything | `docs/ASHFALL_DEEP_CODE_AUDIT_2_2026-08-14.md` (post-bridge, supersedes #1) |
| The fix history / what was remediated | `docs/ASHFALL_DEEP_CODE_AUDIT_2026-08-14.md` (first pass, includes original findings) |
| The closed-loop sweep ledger | `docs/DEBUG_LOOPS_LOG.md` |
| The old master issue register (severity × frequency × impact) | `docs/deprecated_audits/ISSUE_REGISTER.md` (marked RESOLVED/CLOSED) |
| Reproducible sweep scripts | `tools/audit_loops.sh` |
| CI/Unity secrets (info only, do NOT run Unity) | `docs/CI.md` |

### 11.13 Where the bug-hunting batteries live
- `audit/` — 180MB of historical log/XML dumps (gitignored; not code. `rg` skips it via `.ignore`).
- `tools/` — `audit_loops.sh`, `merge_item_text.py`, `generate_holdfast_json.py`, `item_text_batches/`, `audit_loops.sh`.
- `.ignore` — rg ignore file: skips `Library/ Temp/ Builds/ Logs/ UserSettings/ .godot/ bin/ obj/
  generated_AIassets/ audit/ Figma-UI/ _quarantine_legacy/ deprecated_audits/ uam/ .blob .dll`
  so code-only sweeps are tight. **Respect it**: code greps should only see source.

### 11.14 Do-not-touch / leave-behind list
- `webGL` build in `build.yml` (premature; Input/file-IO assumptions break).
- `com.unity.modules.physicscore2d` — MUST stay (required by physics2d + ProjectSettings); pin editor
  to 6000.5.5f1, never open on 6000.3.
- Compiling `_Game` is NOT porting — always wire an end-to-end system to prove behaviour.
- No fancy AI in runtime NPC decisions — Utility AI only (never an LLM).
- Cross-tool QA: any system with ≥2 new coupled variables must be implemented by one tool and
  reviewed/tested by a DIFFERENT tool, reviewer sees diff+spec only.

---

## 12. CURRENT VERIFIED STATE

Verification checklist for the canonical Godot pipeline:

| Check | Result | Details |
|---|---|---|
| `dotnet build Ashfall.csproj` | **PASS** | 0 errors, 0 warnings |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** | All unit tests pass cleanly (0 failed, net9.0) |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 129 catalogs verified, 0 errors |
| `godot --headless --path . -- --player-panels-uitest` | **PASS** | All player UI panels bound & verified |
| `godot --headless --path . -- --expansions-selftest` | **PASS** | Expansions 01–10 green |
| `bash scripts/ci/triad-drift-gate.sh` | **PASS** | Setup/Save/AllSaveSections parity verified |
| `bash scripts/ci/generate-cli-catalog.sh --check` | **PASS** | CLI catalog in sync with live `--host-help` |
