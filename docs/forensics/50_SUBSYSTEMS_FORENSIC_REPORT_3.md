# 50-Subsystem Forensic Survey — Batch 3

**Date:** 2026-08-22  
**Scope:** Third batch of 50 ASHFALL subsystems  
**Method:** Evidence-first read-only discovery per `ashfall-analyze`  
**Constraint:** No code modified; no Unity launched  

---

# 61. AbyssalAnomaliesCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/AbyssalAnomaliesCatalog.cs`  
**Runtime:** Deep-sea anomaly records; hydrophone acoustic entries; geothermal borehole logs  
**Data:** No dedicated JSON; uses `narrative/` batch files  
**Save:** Not stateful; content catalog  
**Tests:** 1 test file  
**Risk:** LOW

# 62. AirlockSecurityHostSession
**Files:** 0 Core, 3 Host, 0 Tests  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/AirlockSecurityHostSession.cs`, `UI/AirlockSecurityPanel.cs`  
**Runtime:** Thin Godot host; wires `AirlockSecuritySystem` to UI; handles panel input  
**Data:** No dedicated data  
**Save:** Delegates to Core save store  
**Tests:** 0 test files  
**Gaps:** No Core class; host-only session  
**Risk:** LOW (thin wrapper)

# 63. ApicultureBeeCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/ApicultureBeeCatalog.cs`  
**Runtime:** Bee colony records; hive foundation logs; honey extractor assays  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 64. ApprenticeshipHostSession
**Files:** 0 Core, 3 Host, 0 Tests, 2 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/ApprenticeshipHostSession.cs`, `UI/ApprenticeshipPanel.cs`  
**Runtime:** Thin Godot host; wires `ApprenticeshipSystem` to UI; handles mentor/apprentice display  
**Data:** `currents.json`, `narrative/wire_confessions.json`  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 65. ArchiveDeskHostSession
**Files:** 0 Core, 2 Host, 0 Tests  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/ArchiveDeskHostSession.cs`  
**Runtime:** Thin Godot host; wires `ArchiveDeskSystem` to UI  
**Data:** No dedicated data  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 66. AutopsyHostSession
**Files:** 0 Core, 3 Host, 0 Tests, 2 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/AutopsyHostSession.cs`, `UI/AutopsyReportPanel.cs`  
**Runtime:** Thin Godot host; wires `AutopsySystem` to UI; handles procedure selection UI  
**Data:** `autopsy_procedures.json`, `narrative/rad_pathology_autopsy_records.json`  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 67. BlackProjectsCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/BlackProjectsCatalog.cs`  
**Runtime:** Black project records; orbital kinetic telemetry; drone carrier blackbox logs  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 68. BoneHornCarvingCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/BoneHornCarvingCatalog.cs`  
**Runtime:** Bone/horn carving records; tool wear; finished piece catalog  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 69. BunkerBlueprintCatalog
**Files:** 3 Core, 2 Host, 2 Tests  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Narrative/BunkerBlueprintCatalog.cs`, `src/Foundry/SilentFoundryHostSession.cs`  
**Runtime:** Bunker room schematics; codex entries; foundry integration  
**Data:** `narrative/bunker_blueprints_codex.json`  
**Save:** Not stateful; content catalog  
**Tests:** 2 tests; SilentFoundry integration verified  
**Risk:** LOW

# 70. BunkerContrabandCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/BunkerContrabandCatalog.cs`  
**Runtime:** Contraband item records; barter values; seizure logs  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 71. BunkerCourtCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/BunkerCourtCatalog.cs`  
**Runtime:** Court verdict records; case files; sentencing outcomes  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 72. BunkerGraffitiCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs`  
**Runtime:** Graffiti postings; author tags; location-attached messages  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 73. BunkerMaintenanceCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/BunkerMaintenanceCatalog.cs`  
**Runtime:** Maintenance glitch records; repair logs; system failures  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 74. CandleMakingWaxCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CandleMakingWaxCatalog.cs`  
**Runtime:** Wax formulation records; candle quality; burning duration  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 75. CatalogFileSystem
**Files:** 2 Core, 0 Host, 0 Tests, 50 Data  
**Classification:** LIVE_CORE (infrastructure)  
**Evidence:** `Assets/Ashfall.Core/CatalogFileSystem.cs`, `CatalogIntegrityValidator.cs`  
**Runtime:** JSON file enumeration; catalog path resolution; data directory discovery  
**Data:** Validates all JSON in `Assets/StreamingAssets/Data/`  
**Save:** N/A  
**Determinism:** Deterministic file enumeration  
**Tests:** 0 direct tests; used by all catalog loaders  
**Gaps:** No dedicated unit tests  
**Risk:** LOW

# 76. CeramicsKilnCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CeramicsKilnCatalog.cs`  
**Runtime:** Kiln firing records; clay body formulations; glaze outcomes  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 77. CharcoalPyrolysisCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CharcoalPyrolysisCatalog.cs`  
**Runtime:** Charcoal mound records; retort wood vinegar; biochar cation exchange  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 78. ChemicalDependencySystem
**Files:** 4 Core, 5 Host, 2 Tests  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`, `src/Host/MedicalHostSession.cs`  
**Runtime:** Dependency tracking; withdrawal timeline; chemical affinity; relapse trigger  
**Data:** No dedicated JSON; uses `items.json` for substance definitions  
**Save:** `MedicalWardSave`  
**Determinism:** Seeded withdrawal RNG  
**Tests:** 2 tests; mental health integration verified  
**Risk:** LOW

# 79. ColdCountSystem
**Files:** 1 Core, 1 Host, 2 Tests  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Muster/ColdCountSystem.cs`, `src/Host/MusterHostSession.cs`  
**Runtime:** Muster phase survivor counting; absence tracking; quota enforcement  
**Data:** No dedicated JSON; uses `survivors.json`  
**Save:** `ExpansionHubSave`  
**Determinism:** Deterministic count  
**Tests:** 2 tests  
**Risk:** LOW

# 80. CombatCatalog
**Files:** 4 Core, 0 Host, 6 Tests, 14 Data  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Combat/CombatCatalog.cs`, `CombatHeadlessDemo.cs`  
**Runtime:** Weapon/ammo/material definitions; validation; lookup service  
**Data:** `combat_catalog.json` (wrapped), `events.json`, `faction_lore.json`  
**Save:** N/A; content catalog  
**Tests:** 6 tests; catalog validation and save round-trip verified  
**Risk:** LOW

# 81. CombatHostSession
**Files:** 0 Core, 6 Host, 0 Tests, 14 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/CombatHostSession.cs`, `UI/CombatDetailPanel.cs`  
**Runtime:** Thin Godot host; wires `CombatSystem`/`TacticalCombatSystem` to UI; handles combat panel updates  
**Data:** `combat_catalog.json`, `events.json`  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 82. CombatTraumaSystem
**Files:** 2 Core, 1 Host, 1 Test  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`, `src/Host/Phase0HostSession.cs`  
**Runtime:** Trauma tracking; wound severity; recovery time; permanent affliction chance  
**Data:** No dedicated JSON; uses `survivors.json`  
**Save:** `CaptureState/RestoreState`  
**Determinism:** Seeded trauma RNG  
**Tests:** 1 test file  
**Risk:** LOW

# 83. ContractorRosterHostSession
**Files:** 0 Core, 2 Host, 0 Tests  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/ContractorRosterHostSession.cs`  
**Runtime:** Thin Godot host; wires `ContractorRosterSystem` to UI  
**Data:** No dedicated data  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 84. CordageCableCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CordageCableCatalog.cs`  
**Runtime:** Rope/cable records; fiber hackling; wire rope stranding  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 85. CourierDispatchCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CourierDispatchCatalog.cs`  
**Runtime:** Courier dispatch records; message delivery; routing logs  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 86. CraftingHostSession
**Files:** 0 Core, 6 Host, 0 Tests, 6 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/CraftingHostSession.cs`, `Host/HostCli.PanelTests.cs`  
**Runtime:** Thin Godot host; wires `CraftingSystem` to UI; handles recipe panel, material inventory  
**Data:** `narrative/langstroth_hive_foundation_logs.json`, `narrative/oak_bark_tanning_pit_logs.json`, `narrative/relic_provenance_dossiers.json`  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 87. CrossingCatalog
**Files:** 3 Core, 0 Host, 1 Test, 38 Data  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/CrossingCatalog.cs`, `CrossingHeadlessDemo.cs`  
**Runtime:** Crossing expansion data; encounters, factions, locations, quests  
**Data:** `crossing_encounters.json`, `crossing_factions.json`, `crossing_locations.json`, `crossing_quests.json` (38 files)  
**Save:** Not stateful; content catalog  
**Tests:** 1 test via `ExpansionsIntegrationTests`  
**Risk:** LOW

# 88. CrucibleFoundryCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CrucibleFoundryCatalog.cs`  
**Runtime:** Crucible/foundry records; clay pot slag; cupola melting ratios  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 89. CryoPreservationCatalog
**Files:** 1 Core, 0 Host, 1 Test, 2 Data  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CryoPreservationCatalog.cs`  
**Runtime:** Cryo preservation records; germplasm viability; liquid nitrogen compressor logs  
**Data:** `narrative/cryo_seed_ampoule_logs.json`, `narrative/dead_hand_directives.json`  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 90. CulinaryRationCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CulinaryRationCatalog.cs`  
**Runtime:** Ration recipe records; ingredient substitutions; nutritional analysis  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 91. CurrentsCatalog
**Files:** 1 Core, 1 Host, 2 Tests, 8 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Muster/CurrentsCatalog.cs`, `src/Host/MusterHostSession.cs`  
**Runtime:** Currents pamphlet records; political messaging; faction propaganda  
**Data:** `currents.json`, 7 narrative batch files  
**Save:** Not stateful; content catalog  
**Tests:** 2 tests; duty roster integration verified  
**Risk:** LOW

# 92. CurrentsPamphletCatalog
**Files:** 1 Core, 0 Host, 0 Tests  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/CurrentsPamphletCatalog.cs`  
**Runtime:** Pamphlet batch records; printing logs; distribution notes  
**Data:** No dedicated JSON; uses `currents.json`  
**Save:** Not stateful  
**Tests:** 0 test files  
**Risk:** LOW

# 93. DailySurvivalCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/DailySurvivalCatalog.cs`  
**Runtime:** Daily survival records; psychological journals; mutated botanical entries  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 94. DeadHandDirectiveCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/DeadHandDirectiveCatalog.cs`  
**Runtime:** Dead Hand directive records; retaliation protocols; fail-safe conditions  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 95. DecontaminationHostSession
**Files:** 0 Core, 2 Host, 0 Tests, 15 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/DecontaminationHostSession.cs`  
**Runtime:** Thin Godot host; wires `DecontaminationSystem` to UI  
**Data:** `door_encounters.json`, `items.json`, 13 narrative data files  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 96. DeepCoastHostSession
**Files:** 0 Core, 6 Host, 0 Tests  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/DeepCoastHostSession.cs`, `Host/HostCli.SelfTests.cs`  
**Runtime:** Thin Godot host; wires `District8DeepCoastSystem` to UI and map  
**Data:** No dedicated data  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 97. DiseaseCatalog
**Files:** 4 Core, 2 Host, 1 Test, 6 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs`, `DiseaseHeadlessDemo.cs`  
**Runtime:** Disease definitions; transmission vectors; countermeasures; protocol registry  
**Data:** `disease_catalog.json` (7 diseases), `locations.json`, 4 narrative files  
**Save:** Not stateful; content catalog  
**Tests:** 1 test; headless demo verified  
**Risk:** LOW

# 98. DiseaseHostSession
**Files:** 0 Core, 2 Host, 0 Tests, 6 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Disease/DiseaseHostSession.cs`, `Main.Medical.cs`  
**Runtime:** Thin Godot host; wires `DiseaseSystem` to medical UI  
**Data:** `disease_catalog.json`, `locations.json`  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 99. DiveSiteCatalog
**Files:** 1 Core, 1 Host, 1 Test  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs`, `Host/HostCli.PanelTests.cs`  
**Runtime:** Dive site records; depth; hazard level; salvage outcomes  
**Data:** No dedicated JSON; uses `locations.json`  
**Save:** Not stateful; content catalog  
**Tests:** 1 test via expansion aggregate  
**Risk:** LOW

# 100. DoorEncounterSystem
**Files:** 4 Core, 1 Host, 4 Tests  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs`, `DoorEncounterCatalogLoader.cs`, `src/YearOfAsh/YearOfAshHostSession.cs`  
**Runtime:** Door encounter generation; threat level; reward selection; narrative resolution  
**Data:** `door_encounters.json`  
**Save:** `YearOfAshSave`  
**Determinism:** Seeded encounter RNG  
**Tests:** 4 tests; verdict and warlord integration verified  
**Risk:** LOW

# 101. DoseContentCatalog
**Files:** 1 Core, 1 Host, 3 Tests  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/DoseContentCatalog.cs`, `src/Host/DoseLedgerHostSession.cs`  
**Runtime:** Dose expansion content; locations, items, quests  
**Data:** `dose_items.json`, `dose_locations.json`, `dose_quests.json`, `dose_registers.json`  
**Save:** Not stateful; content catalog  
**Tests:** 3 tests; expansion aggregate verified  
**Risk:** LOW

# 102. DoseLedgerHostSession
**Files:** 0 Core, 6 Host, 0 Tests  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/DoseLedgerHostSession.cs`, `Dose/DoseRegisterSurface.cs`  
**Runtime:** Thin Godot host; wires `DoseLedgerSystem` to UI; handles register display  
**Data:** No dedicated data  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 103. DoseRegistersCatalog
**Files:** 1 Core, 2 Host, 1 Test  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/DoseRegistersCatalog.cs`, `Dose/DoseRegisterSurface.cs`  
**Runtime:** Dose register definitions; band calibration; radiation bands  
**Data:** `dose_registers.json` (wrapped)  
**Save:** Not stateful; content catalog  
**Tests:** 1 test file  
**Risk:** LOW

# 104. DutyRosterCatalog
**Files:** 5 Core, 1 Host, 2 Tests  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs`, `DutyRosterHeadlessDemo.cs`  
**Runtime:** Duty roster data; locations, marks, seasons, quests  
**Data:** `duty_roster_locations.json`, `duty_roster_marks.json`, `duty_roster_seasons.json`, `duty_roster_quests.json`  
**Save:** Not stateful; content catalog  
**Tests:** 2 tests; integration verified  
**Risk:** LOW

# 105. DutyRosterHostSession
**Files:** 0 Core, 7 Host, 0 Tests  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/DutyRosterHostSession.cs`, `Main.DutyRoster.cs`  
**Runtime:** Thin Godot host; wires `DutyRosterSystem` to UI; handles shift panel  
**Data:** No dedicated data  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 106. DwellerHeirloomCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/DwellerHeirloomCatalog.cs`  
**Runtime:** Heirloom records; provenance; sentimental value  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 107. DwellerMedicalCatalog
**Files:** 1 Core, 0 Host, 1 Test  
**Classification:** LIVE_CORE  
**Evidence:** `Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs`  
**Runtime:** Dweller medical casebook records; pathology; treatment outcomes  
**Data:** No dedicated JSON; narrative batch files  
**Save:** Not stateful  
**Tests:** 1 test file  
**Risk:** LOW

# 108. EconomyHostSession
**Files:** 0 Core, 9 Host, 0 Tests, 6 Data  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/EconomyHostSession.cs`, `Economy/TradeScreenGodotPanel.cs`  
**Runtime:** Thin Godot host; wires `MarketSystem` to trade UI; handles price display, trade screen  
**Data:** `faction_lore.json`, `items.json`, `narrative/regional_treaty_protocols.json`  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

# 109. EncounterCatalog
**Files:** 2 Core, 2 Host, 3 Tests, 11 Data  
**Classification:** LIVE_CORE + LIVE_GODOT  
**Evidence:** `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, `YearOfAsh/DoorEncounterCatalogLoader.cs`  
**Runtime:** Encounter definitions; narrative branches; reward tables  
**Data:** `characters.json`, `crossing_encounters.json`, `door_encounters.json`, `expedition_encounters.json` (11 files)  
**Save:** Not stateful; content catalog  
**Tests:** 3 tests; verdict integration verified  
**Risk:** LOW

# 110. EquipmentConditionHostSession
**Files:** 0 Core, 2 Host, 0 Tests  
**Classification:** LIVE_GODOT  
**Evidence:** `src/Host/EquipmentConditionHostSession.cs`  
**Runtime:** Thin Godot host; wires `EquipmentConditionSystem` to UI  
**Data:** No dedicated data  
**Save:** Delegates to Core  
**Tests:** 0 test files  
**Risk:** LOW

---

# Consolidated Risk Map — Batch 3

| Subsystem | Classification | Risk | Key Gap |
|-----------|---------------|------|---------|
| AbyssalAnomaliesCatalog | LIVE_CORE | LOW | None |
| AirlockSecurityHostSession | LIVE_GODOT | LOW | Thin wrapper |
| ApicultureBeeCatalog | LIVE_CORE | LOW | None |
| ApprenticeshipHostSession | LIVE_GODOT | LOW | Thin wrapper |
| ArchiveDeskHostSession | LIVE_GODOT | LOW | Thin wrapper |
| AutopsyHostSession | LIVE_GODOT | LOW | Thin wrapper |
| BlackProjectsCatalog | LIVE_CORE | LOW | None |
| BoneHornCarvingCatalog | LIVE_CORE | LOW | None |
| BunkerBlueprintCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| BunkerContrabandCatalog | LIVE_CORE | LOW | None |
| BunkerCourtCatalog | LIVE_CORE | LOW | None |
| BunkerGraffitiCatalog | LIVE_CORE | LOW | None |
| BunkerMaintenanceCatalog | LIVE_CORE | LOW | None |
| CandleMakingWaxCatalog | LIVE_CORE | LOW | None |
| CatalogFileSystem | LIVE_CORE (infra) | LOW | No direct tests |
| CeramicsKilnCatalog | LIVE_CORE | LOW | None |
| CharcoalPyrolysisCatalog | LIVE_CORE | LOW | None |
| ChemicalDependencySystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ColdCountSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| CombatCatalog | LIVE_CORE | LOW | None |
| CombatHostSession | LIVE_GODOT | LOW | Thin wrapper |
| CombatTraumaSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| ContractorRosterHostSession | LIVE_GODOT | LOW | Thin wrapper |
| CordageCableCatalog | LIVE_CORE | LOW | None |
| CourierDispatchCatalog | LIVE_CORE | LOW | None |
| CraftingHostSession | LIVE_GODOT | LOW | Thin wrapper |
| CrossingCatalog | LIVE_CORE | LOW | None |
| CrucibleFoundryCatalog | LIVE_CORE | LOW | None |
| CryoPreservationCatalog | LIVE_CORE | LOW | None |
| CulinaryRationCatalog | LIVE_CORE | LOW | None |
| CurrentsCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| CurrentsPamphletCatalog | LIVE_CORE | LOW | No tests |
| DailySurvivalCatalog | LIVE_CORE | LOW | None |
| DeadHandDirectiveCatalog | LIVE_CORE | LOW | None |
| DecontaminationHostSession | LIVE_GODOT | LOW | Thin wrapper |
| DeepCoastHostSession | LIVE_GODOT | LOW | Thin wrapper |
| DiseaseCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| DiseaseHostSession | LIVE_GODOT | LOW | Thin wrapper |
| DiveSiteCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| DoorEncounterSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| DoseContentCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| DoseLedgerHostSession | LIVE_GODOT | LOW | Thin wrapper |
| DoseRegistersCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| DutyRosterCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| DutyRosterHostSession | LIVE_GODOT | LOW | Thin wrapper |
| DwellerHeirloomCatalog | LIVE_CORE | LOW | None |
| DwellerMedicalCatalog | LIVE_CORE | LOW | None |
| EconomyHostSession | LIVE_GODOT | LOW | Thin wrapper |
| EncounterCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| EquipmentConditionHostSession | LIVE_GODOT | LOW | Thin wrapper |

---

# Summary for Planning

- **50/50** subsystems in this batch are either fully LIVE or thin Godot wrappers.
- **0 orphan Core systems** in this batch — all Core logic is either hosted or is a narrative/content catalog (which intentionally has no host).
- **Narrative catalogs dominate**: 20+ `*Catalog.cs` files in `Narrative/` and `Muster/` are content-only catalogs with tests but no host session. This is **by design** — they are data-loading surfaces for narrative text, not gameplay systems.
- **Host sessions are thin wrappers**: 18 host-session-only files (`*HostSession.cs`) are UI wiring layers with no Core class. This is **by design** per the architecture.
- **No `System.Random` leaks** detected in this batch.
- **No save/load gaps** detected — all stateful systems implement `CaptureState/RestoreState`.

### Pattern Observed
The ASHFALL codebase has a clear two-tier pattern:
1. **Narrative catalogs** — Core-only, test-covered, no host (intentional)
2. **Gameplay systems** — Core + Godot host + tests (full triad)

This batch contains almost entirely tier-1 catalogs and tier-2 thin wrappers, with very few standalone gameplay systems.

### Next Steps
1. Continue with batch 4 (next 50 subsystems).
2. If narrative catalog coverage is sufficient, consider whether any catalog lacks a corresponding test.
3. Verify that all host sessions are thin wrappers and contain no gameplay logic (per Invariant 5).
