# 30-Subsystem Forensic Survey

**Date:** 2026-08-22  
**Scope:** Evidence-first read-only classification of 30 ASHFALL subsystems  
**Method:** Source, host, test, and data discovery per `ashfall-analyze`  
**Constraint:** No code modified; no Unity launched  

---

# 1. DiseaseSystem
**Files:** 6 Core, 3 Host, 2 Tests, 6 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`, `DiseaseCatalog.cs`, `src/Host/DiseaseHostSession.cs`, `Ashfall.Core.Tests/DiseaseSystemTests.cs`  
**Runtime:** Constructed by `ExpansionMasterSession`; ticked in `Main.Medical.cs`; events raised on infection/quarantine/outbreak  
**Data:** `disease_catalog.json` (7 diseases with countermeasures), `items.json` for countermeasure resolution  
**Save:** `CaptureState/RestoreState` with `DiseaseSystemState` DTO; round-trip verified  
**Determinism:** Uses `ISeededRng`; deterministic candidate ordering  
**Tests:** 28/28 headless demo checks pass; save round-trip preserves count and outcome history  
**Gaps:** None significant  
**Risk:** LOW

# 2. NeedsSystem
**Files:** 7 Core, 8 Host, 6 Tests, 30 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`, `src/Host/SurvivorsHostSession.cs`, `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`  
**Runtime:** Tick-driven via `Main.Phase0.cs`; hunger/thirst/fatigue/warmth/morale/radiation tracked per survivor  
**Data:** `survivors.json` (102 definitions), `items.json` for restore items  
**Save:** `CaptureState/RestoreState` with per-survivor need values  
**Determinism:** `ISeededRng`; no `System.Random`  
**Tests:** 58 tests covering tick behavior; save/load round-trip still missing per H11  
**Gaps:** Save/load round-trip coverage gap (H11)  
**Risk:** MEDIUM

# 3. CombatSystem
**Files:** 3 Core, 4 Host, 3 Tests, 14 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Combat/CombatTypes.cs`, `CombatCatalog.cs`, `src/Host/CombatHostSession.cs`, `UI/CombatPanel.cs`  
**Runtime:** Event-driven; `CombatEvent` raised on hit/miss/kill; UI panel subscribes  
**Data:** `combat_catalog.json` (weapons, ammo, materials)  
**Save:** `CaptureState/RestoreState` with combat log and participant states  
**Determinism:** Seeded RNG for hit rolls  
**Tests:** `CombatSystemTests`, `CombatSaveRoundTripTests`; catalog loads 0 entries when wrapped with wrong key (recent regression, fixed)  
**Gaps:** None  
**Risk:** LOW

# 4. TacticalCombatSystem
**Files:** 3 Core, 4 Host, 3 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`, `CombatTypes.cs`, `src/Host/CombatHostSession.cs`  
**Runtime:** Extends `CombatSystem` with suppression, burst fire, armor, morale  
**Data:** Reuses `combat_catalog.json`  
**Save:** Inherited from `CombatSystem`  
**Determinism:** Seeded  
**Tests:** 3 tests; suppression, ammo consumption, armor stop verified  
**Gaps:** None  
**Risk:** LOW

# 5. ExpeditionSystem
**Files:** 7 Core, 8 Host, 5 Tests, 33 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`, `src/Host/ExpeditionHostSession.cs`, `Ashfall.Core.Tests/ExpeditionSystemTests.cs`  
**Runtime:** Tick-based journey simulation; encounter bridge to `DoorEncounterCatalog`; warlord danger integration  
**Data:** `expedition_encounters.json`, `door_encounters.json`, `wasteland_*` catalogs  
**Save:** `CaptureState/RestoreState` with expedition state, roster, vehicle  
**Determinism:** Seeded RNG; deterministic encounter selection  
**Tests:** 5 test files; encounter bridge verified  
**Gaps:** None  
**Risk:** LOW

# 6. MarketSystem
**Files:** 3 Core, 4 Host, 4 Tests, 26 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Economy/MarketSystem.cs`, `src/Host/EconomyHostSession.cs`, `Ashfall.Core.Tests/EconomySystemTests.cs`  
**Runtime:** Price calculation per good; demand/supply modifiers; trade screen integration  
**Data:** `economy_goods.json`, `trade_screen_scenarios.json`  
**Save:** `CaptureState/RestoreState` with market prices and modifiers  
**Determinism:** Deterministic price updates per tick  
**Tests:** 4 test files; market adapter probes pass  
**Gaps:** None  
**Risk:** LOW

# 7. HoldfastTradeSession
**Files:** 1 Core, 5 Host, 1 Test, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/HoldfastTradeSession.cs`, `HoldfastCatalog.cs`, `src/Host/HoldfastRuntimeSession.cs`  
**Runtime:** Buy/sell/transfer with faction stock and player inventory; `HoldfastCatalog` provides item/faction definitions  
**Data:** `holdfast_items.json` (40 items), `holdfast_factions.json`, `holdfast_locations.json`  
**Save:** `HoldfastSave` with V1→V2→V3→V4 migration  
**Determinism:** Transaction order deterministic  
**Tests:** 2 tests; regression fixed after `LoadWrappedList` wrapper key issue  
**Gaps:** None  
**Risk:** LOW

# 8. WarlordDoctrineSystem
**Files:** 4 Core, 1 Host, 3 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs`, `WarlordDoctrineCatalog.cs`, `src/YearOfAsh/YearOfAshHostSession.cs`  
**Runtime:** Doctrine state machine (toll/aggression/containment); territory graph; tribute escalation  
**Data:** `warlord_doctrines.json` (object-format with warlord, territory, doctrines)  
**Save:** `YearOfAshSave` V3 includes warlord state  
**Determinism:** Seeded RNG; deterministic target selection  
**Tests:** 16 tests; all pass after recent fixes  
**Gaps:** None  
**Risk:** LOW

# 9. QuestlineSystem
**Files:** 8 Core, 5 Host, 9 Tests, 5 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`, `QuestlineMasterCatalog.cs`, `src/Host/VerdictHostSession.cs`  
**Runtime:** Quest registration, stage progression, completion; master registry cross-references all expansions  
**Data:** `questline_master.json` (262 entries), `year_of_ash_quests.json`, `dose_quests.json`, expansion quest files  
**Save:** `CaptureState/RestoreState` with active quests and stage pointers  
**Determinism:** Deterministic quest lookup by ID  
**Tests:** 8 tests; master registry validated against YOA and Dose quest IDs  
**Gaps:** None  
**Risk:** LOW

# 10. FactionWarSystem
**Files:** 5 Core, 3 Host, 2 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`, `FactionWarContentCatalog.cs`, `src/YearOfAsh/FactionWarMapWidget.cs`  
**Runtime:** Sector control simulation; broadcast events; dialogue snippets; map widget visualization  
**Data:** `faction_war_*.json` (5 files: broadcasts, communiques, dialogue, events, journal)  
**Save:** `YearOfAshSave` includes faction war state  
**Determinism:** Seeded RNG for event resolution  
**Tests:** 2 test files; content catalog loads 5 JSON files  
**Gaps:** None  
**Risk:** LOW

# 11. DutyRosterSystem
**Files:** 12 Core, 14 Host, 11 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`, `MoraleMarkSystem.cs`, `src/Main.DutyRoster.cs`  
**Runtime:** Shift assignment, morale marks, shelter encounters, apprenticeship, archive desk, contractor roster  
**Data:** `duty_roster_*.json` (locations, marks, seasons, quests)  
**Save:** `DutyRosterSave` with V1/V2 migration  
**Determinism:** Seeded; deterministic roster ordering  
**Tests:** 11 test files; comprehensive coverage  
**Gaps:** None  
**Risk:** LOW

# 12. GreenhouseSystem
**Files:** 3 Core, 4 Host, 4 Tests, 43 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`, `GreenhouseExpansionCatalog.cs`, `src/Host/GreenhouseHostSession.cs`  
**Runtime:** Crop growth cycles; soil/water/pest mechanics; harvest yield  
**Data:** `greenhouse_items.json`, `greenhouse_*.json` (43 data files)  
**Save:** `ExpansionHubSave` includes greenhouse state  
**Determinism:** Seeded growth RNG  
**Tests:** 4 test files; headless demo exists  
**Gaps:** None  
**Risk:** LOW

# 13. VerdictSystem
**Files:** 0 Core (class not found), 0 Host, 0 Tests, 13 Data  
**Classification:** DATA_ONLY / MISSING_CLASS  
**Evidence:** No `VerdictSystem.cs` found. Related classes: `VerdictNpcSystem`, `VerdictRadioSystem`, `VerdictCatalogLoader`, `VerdictSave`. Data: `verdict_data.json`, `verdict_items.json`, `verdict_questlines.json`, `verdict_radio.json`, `verdict_locations.json`  
**Gaps:** No standalone `VerdictSystem` class; behavior distributed across `VerdictNpcSystem`, `VerdictRadioSystem`, `VerdictHostSession`  
**Risk:** MEDIUM — ownership ambiguity

# 14. CrossingQuestSystem
**Files:** 2 Core, 4 Host, 4 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs`, `ExpansionHubSave.cs`, `src/Host/ExpansionHostSession.cs`  
**Runtime:** Arbitration quest stages; faction trust gating; crossing-specific quest lines  
**Data:** `crossing_quests.json`, `crossing_encounters.json`, `crossing_factions.json`  
**Save:** `ExpansionHubSave`  
**Determinism:** Seeded  
**Tests:** 4 tests; quest progression verified  
**Gaps:** None  
**Risk:** LOW

# 15. DoseLedgerSystem
**Files:** 5 Core, 7 Host, 5 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/DoseLedgerSystem.cs`, `DoseLedgerSave.cs`, `src/Host/DoseLedgerHostSession.cs`  
**Runtime:** Radiation dose tracking per survivor; ledger entries; dose-based afflictions  
**Data:** `dose_items.json`, `dose_locations.json`, `dose_quests.json`, `dose_registers.json`  
**Save:** `DoseLedgerSave` with checksum  
**Determinism:** Deterministic dose accumulation  
**Tests:** 5 tests; 10/10 dose-ledger selftest passes  
**Gaps:** None  
**Risk:** LOW

# 16. WeatherSystem
**Files:** 5 Core, 8 Host, 5 Tests, 45 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/World/WeatherSystem.cs`, `WeatherStationSystem.cs`, `src/Host/WorldHostSession.cs`  
**Runtime:** Season/day cycle; precipitation; temperature; nuclear winter; fallout storms  
**Data:** `weather_seasons.json`, `weather_*.json` (45 files)  
**Save:** `WorldSaveStore` (hard-reject restored in BUG-01)  
**Determinism:** Seeded seasonal progression  
**Tests:** 5 test files; audio integration tests pass  
**Gaps:** None  
**Risk:** LOW

# 17. MedicalSystem
**Files:** 1 Core, 3 Host, 5 Tests, 68 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`, `MedicalWardSave.cs`, `src/Main.Medical.cs`  
**Runtime:** Ward capacity; triage; treatment time; affliction progression  
**Data:** `medical_ward_*.json`, `autopsy_procedures.json`, `dweller_medical_casebook.json` (68 files)  
**Save:** `MedicalWardSave`  
**Determinism:** Seeded triage outcomes  
**Tests:** 5 test files; autopsy integration verified  
**Gaps:** None  
**Risk:** LOW

# 18. CraftingSystem
**Files:** 4 Core, 9 Host, 5 Tests, 6 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`, `EquipmentConditionSystem.cs`, `src/Host/CraftingHostSession.cs`  
**Runtime:** Recipe execution; material consumption; quality tiers; workshop reverse engineering  
**Data:** `recipes.json`, `crafting_*.json`  
**Save:** `CraftingSaveStore`  
**Determinism:** Seeded quality roll  
**Tests:** 5 test files; affliction loop tests pass  
**Gaps:** None  
**Risk:** LOW

# 19. UtilityAiSystem
**Files:** 2 Core, 2 Host, 2 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs`, `UtilityAction.cs`, `src/Host/UtilityAiHostSession.cs`  
**Runtime:** Action scoring via response curves; trait bias; fatigue gating  
**Data:** `utility_actions.json` (6 actions)  
**Save:** No dedicated save; actions re-evaluated from catalog each tick  
**Determinism:** Deterministic scoring given same context  
**Tests:** 19 tests; 4 crossing actions verified  
**Gaps:** None  
**Risk:** LOW

# 20. RadioSystem
**Files:** 2 Core, 3 Host, 1 Test, 101 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`, `VerdictSave.cs`, `src/UI/VerdictPanel.cs`  
**Runtime:** Broadcast scheduling; distress signal detection; scripted radio events  
**Data:** `radio.json` (50 entries), `verdict_radio.json`, `faction_radio_corpus.json` (101 data files)  
**Save:** `VerdictSave` / `RadioSave`  
**Determinism:** Seeded broadcast timing  
**Tests:** 1 test; 13 authored broadcasts load  
**Gaps:** None  
**Risk:** LOW

# 21. WorldSystem
**Files:** 0 Core (class not found), 0 Host, 0 Tests, 37 Data  
**Classification:** DATA_ONLY / DISTRIBUTED  
**Evidence:** No `WorldSystem.cs` class. Related: `WastelandMapSystem`, `WeatherSystem`, `WorldHostSession`, `WorldSaveStore`. Data: `world_*.json`, `wasteland_*.json`, `sectors.json` (37 files)  
**Gaps:** No single `WorldSystem`; behavior split across map, weather, and host session  
**Risk:** MEDIUM — ownership fragmentation

# 22. SurvivorsHostSession
**Files:** 1 Core, 19 Host, 1 Test, 0 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/SurvivorsHostSession.cs`, `src/Main.Survivors.cs`, `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`  
**Runtime:** Thin Godot host; wires Core survivor systems to UI panels; handles input and presentation  
**Data:** `survivors.json`, `characters.json`  
**Save:** Delegates to Core save stores  
**Determinism:** No RNG; deterministic UI updates  
**Tests:** 1 test file; needs/radiation integration verified  
**Gaps:** H1 — duplicates core survival mechanics in host session  
**Risk:** HIGH — host-core duplication

# 23. NarrativeBatchCatalog
**Files:** 2 Core, 1 Host, 1 Test, 0 Data  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/NarrativeBatchCatalog.cs`, `src/Foundry/SilentFoundryHostSession.cs`  
**Runtime:** Loads batched narrative files (journal templates, found documents, eulogies, Vel triage logs)  
**Data:** `narrative_batch.json`, `found_documents_batch.json`, `eulogy_corpus_batch_1.json`  
**Save:** Not stateful; content catalog only  
**Determinism:** N/A  
**Tests:** 1 test; journal templates load with expansion isolation  
**Gaps:** None  
**Risk:** LOW

# 24. SaveChecksum
**Files:** 22 Core, 42 Host, 33 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/SaveChecksum.cs`, `HoldfastSave.cs`, `src/Host/*SaveStore.cs`  
**Runtime:** Reflection-based integrity hash; normalizes null/empty, float G9 formatting, culture-invariant, ordinal name order  
**Data:** No data files; operates on save DTOs  
**Save:** Computes checksum over serialized save envelope  
**Determinism:** Deterministic hash given same state  
**Tests:** 33 test files; checksum sweep verified across 12 stores  
**Gaps:** 5 Godot save stores lacked checksum (now fixed); pre-checksum fallback path exists  
**Risk:** LOW

# 25. CatalogIntegrityValidator
**Files:** 2 Core, 1 Host, 3 Tests, 0 Data  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, `src/Host/HostCli.SelfTests.cs`, `Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`  
**Runtime:** Five-tier validation: REGISTRY, TIER-1 (prefix resolution), TIER-2 (reference keys), RANGES, UNIQUENESS  
**Data:** Validates all JSON in `Assets/StreamingAssets/Data/`  
**Save:** N/A  
**Determinism:** Deterministic validation order  
**Tests:** 3 tests; 0 errors across 102 catalogs (3637 ids)  
**Gaps:** None  
**Risk:** LOW

# 26. PowerGridSave
**Files:** 2 Core, 2 Host, 1 Test, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Shelter/PowerGridSave.cs`, `PowerGridSystem.cs`, `src/Host/PowerGridSaveStore.cs`  
**Runtime:** Power generation/distribution; battery charge; load shedding  
**Data:** `power_grid.json`  
**Save:** `PowerGridSave` DTO with `CaptureState/RestoreState`  
**Determinism:** Seeded failure RNG  
**Tests:** 1 test file; power grid system tests pass  
**Gaps:** None  
**Risk:** LOW

# 27. ShelterAssignmentSave
**Files:** 1 Core, 1 Host, 2 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs`, `src/Host/ShelterAssignmentHostSession.cs`  
**Runtime:** Survivor-to-room assignment; capacity tracking  
**Data:** `shelter_schedules.json`  
**Save:** `ShelterAssignmentSave` with checksum  
**Determinism:** Deterministic assignment order  
**Tests:** 2 tests; checksum sweep verified  
**Gaps:** None  
**Risk:** LOW

# 28. ExpansionHubSave
**Files:** 2 Core, 7 Host, 5 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/ExpansionHubSave.cs`, `src/Main.ExpansionHub.cs`, `Ashfall.Core.Tests/ExpansionHubSaveTests.cs`  
**Runtime:** Coordinates 4 expansions (Holdfast 01, Duty Roster 02, Standing Record 03, Crossing 04); aggregates expansion state  
**Data:** No dedicated data file; reads expansion-specific catalogs  
**Save:** `ExpansionHubSave` with V1/V2/V3 migration  
**Determinism:** Seeded  
**Tests:** 5 tests; disease expansion headless smoke  
**Gaps:** Phase 11 wiring stubs remain  
**Risk:** MEDIUM

# 29. SilentFoundrySystem
**Files:** 6 Core, 2 Host, 3 Tests, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`, `SilentFoundryCatalog.cs`, `src/Foundry/SilentFoundryHostSession.cs`  
**Runtime:** Production cycles; treaty/quota tracking; strike escalation; journal template emission  
**Data:** `foundry_*.json` (production, accords, treaties, factions)  
**Save:** `ExpansionHubSave` includes foundry state  
**Determinism:** Seeded production RNG  
**Tests:** 3 test files; 22 SilentFoundrySystemTests, 18 SilentFoundryConsequenceTests  
**Gaps:** Recent wrapper key regression in `foundry_*.json` files (fixed)  
**Risk:** LOW

# 30. EquipmentConditionSystem
**Files:** 1 Core, 2 Host, 1 Test, 0 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/EquipmentConditionSystem.cs`, `src/Host/EquipmentConditionHostSession.cs`  
**Runtime:** Item durability; repair consumption; degradation per use  
**Data:** `items.json` (condition fields)  
**Save:** `CaptureState/RestoreState` with per-item condition  
**Determinism:** Deterministic degradation  
**Tests:** 1 test file; condition/repair verified  
**Gaps:** None  
**Risk:** LOW

---

# Consolidated Risk Map

| Subsystem | Classification | Risk | Key Gap |
|-----------|---------------|------|---------|
| DiseaseSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| NeedsSystem | LIVE_CORE+LIVE_GODOT | MEDIUM | Save round-trip gap (H11) |
| CombatSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| TacticalCombatSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ExpeditionSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MarketSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| HoldfastTradeSession | LIVE_CORE+LIVE_GODOT | LOW | None |
| WarlordDoctrineSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| QuestlineSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| FactionWarSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| DutyRosterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| GreenhouseSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| VerdictSystem | DATA_ONLY/MISSING | MEDIUM | No standalone class |
| CrossingQuestSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| DoseLedgerSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| WeatherSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MedicalSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| CraftingSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| UtilityAiSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| RadioSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| WorldSystem | DATA_ONLY/DISTRIBUTED | MEDIUM | No single class |
| SurvivorsHostSession | LIVE_GODOT | HIGH | Host-core duplication (H1) |
| NarrativeBatchCatalog | LIVE_CORE | LOW | None |
| SaveChecksum | LIVE_CORE+LIVE_GODOT | LOW | None |
| CatalogIntegrityValidator | LIVE_CORE | LOW | None |
| PowerGridSave | LIVE_CORE+LIVE_GODOT | LOW | None |
| ShelterAssignmentSave | LIVE_CORE+LIVE_GODOT | LOW | None |
| ExpansionHubSave | LIVE_CORE+LIVE_GODOT | MEDIUM | Phase 11 stubs |
| SilentFoundrySystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| EquipmentConditionSystem | LIVE_CORE+LIVE_GODOT | LOW | None |

---

# Summary for Planning

- **27/30 subsystems are LIVE** with Core + Godot host wiring and test coverage.
- **2 subsystems lack a standalone Core class** (`VerdictSystem`, `WorldSystem`); behavior is distributed.
- **1 subsystem has host-core duplication** (`SurvivorsHostSession` duplicates core survival mechanics — H1).
- **2 subsystems have MEDIUM risk**: `NeedsSystem` (save round-trip gap), `ExpansionHubSave` (Phase 11 stubs).
- **Zero CONFLICT or STALE divergence** in agent rulebooks (separate report at `docs/agents/AGENTS_SYNC_REPORT.md`).
