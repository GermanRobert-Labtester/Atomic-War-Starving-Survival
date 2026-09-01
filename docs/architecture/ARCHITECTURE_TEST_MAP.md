# ASHFALL — Evidence-Derived Architecture & Verification Graph

**Last Verified:** 2026-08-30<br>
**Total Subsystems Mapped:** 65/65 (100.0%)<br>
**Verified End-to-End Coverage:** 63/65 (96.9% across all 6 vertical layers)<br>
**Status Breakdown:** Implemented: 65/65 | Constructed: 63/65 | Ticked: 65/65 | Persisted: 65/65 | Routed: 65/65 | Tested: 65/65<br>
**Single Source of Truth:** `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` & `Assets/Ashfall.Core/HostCliRegistry.cs`

> **GENERATED FILE — do not edit by hand.**
> Derived mechanically from real C# type definitions, catalog JSON files, host wiring, and test fixtures.
> Generated via: `bash scripts/ci/generate-architecture-map.sh`
> CI Completeness Gate: `bash scripts/ci/generate-architecture-map.sh --check`

---

## 1. Six-Tier Architectural Layering Flow & Discrete Verification Taxonomy

Every subsystem in ASHFALL is verified against six distinct, non-fungible lifecycle layers:

```
┌────────────────────────────────────────────────────────────────────────┐
│ 1. CORE DOMAIN LOGIC [Implemented]                                     │
│    Engine-agnostic C# systems under Assets/Ashfall.Core/ (0 engine refs)│
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ reads definition schemas
┌───────────────────────────────────▼────────────────────────────────────┐
│ 2. DATA CATALOG AUTHORITY [Data]                                       │
│    snake_case JSON schemas under Assets/StreamingAssets/Data/          │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ constructed & orchestrated by
┌───────────────────────────────────▼────────────────────────────────────┐
│ 3. GODOT HOST SESSION [Constructed & Ticked]                           │
│    Session lifecycle in src/Host/ with Setup* wiring & sim tick cadence │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ snapshots / restores via
┌───────────────────────────────────▼────────────────────────────────────┐
│ 4. PERSISTENCE SAVE STORE [Persisted]                                  │
│    Checksummed SaveStore<T> via SaveStoreHub, atomic writes & SaveAll  │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ presents live state to user
┌───────────────────────────────────▼────────────────────────────────────┐
│ 5. GODOT UI PANEL [Player-Routed]                                      │
│    Responsive Control under src/UI/ routed in OpenPlayerPanel/HUD      │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ protected & regression-gated by
┌───────────────────────────────────▼────────────────────────────────────┐
│ 6. CI SELF-TEST & XUNIT SUITE [Tested]                                 │
│    CLI verbs in HostCliRegistry.cs & test fixtures in Ashfall.Core.Tests│
└────────────────────────────────────────────────────────────────────────┘
```

---

## 2. Complete Architecture Subsystem & Evidence-Derived Graph Matrix

| # | Section Key | Domain | Core System | Data Catalog | Host Session | Save Store | UI Panel | CLI Self-Test / Unit Tests | Status |
|---|---|---|---|---|---|---|---|---|:---:|
| 1 | `host_event` | Campaign & Lore | `MoralChoiceSystem` | `events.json` | `HostEventAdapter` | `MoralChoiceSaveStore`, `HostEventSaveStore` | `EventDetailPanel` | `--moral-choice-selftest`, `HostEventSaveSealTests` | ✅ 6/6 |
| 2 | `journal` | Campaign & Lore | `JournalSystem` | `world_history.json` | `JournalHostSession` | `JournalSaveStore` | `JournalPanel`, `JournalBookUI` | `--journal-save-selftest`, `JournalSystemTests` | ✅ 6/6 |
| 3 | `memorial` | Campaign & Lore | `MemorialSystem` | — *(Procedural)* | `MemorialSystem` | `MemorialSaveStore` | `GameDashboardPanel` | `--player-panels-uitest`, `MemorialSystemTests` | ✅ 6/6 |
| 4 | `narrative` | Campaign & Lore | `NarrativeEncounterSystem` | `narrative_encounters.json` | `NarrativeHostSession` | `NarrativeSaveStore` | `EventsLogPanel`, `FactionsNarrativePanel` | `--narrative-selftest`, `NarrativeEncounterSystemTests` | ✅ 6/6 |
| 5 | `phase0` | Campaign & Lore | `RespiratoryDegenerationSystem` | — *(Procedural)* | `Phase0HostSession` | `Phase0SaveStore` | `Phase0Panel` | `--phase0-selftest`, `--phase0-uitest`, `Phase0EffectsBridgeTests` | ✅ 6/6 |
| 6 | `survivor_fate` | Campaign & Lore | `SurvivorFateSystem` | — *(Procedural)* | *None (GAP)* | `SurvivorFateSaveStore` | `GameDashboardPanel` | `--playable-shell-selftest`, `SurvivorFateSystemTests` | ❌ GAP |
| 7 | `onboarding` | Campaign & Onboarding | `OnboardingJourney` | — *(Procedural)* | *None (GAP)* | `OnboardingSaveStore` | `OnboardingHintPanel` | `--onboarding-journey-selftest`, `OnboardingJourneyTests` | ❌ GAP |
| 8 | `archive_desk` | Campaign & Progression | `ArchiveDeskSystem` | `archive_inks.json` | `ArchiveDeskHostSession` | `ArchiveDeskSaveStore` | `ArchiveDeskPanel` | `--shelter-operations-selftest`, `ArchiveDeskSystemTests` | ✅ 6/6 |
| 9 | `campaign_day` | Campaign & Progression | `CampaignDayCoordinator` | — *(Procedural)* | `CampaignDayCoordinator` | `CampaignDaySaveStore` | `GameDashboardPanel` | `--day1-selftest`, `--day1-to-day2-selftest`, `CampaignDayCoordinatorTests` | ✅ 6/6 |
| 10 | `daily_briefing` | Campaign & Progression | `DailyBriefingReportBuilder`, `DailyBriefingState` | — *(Procedural)* | `DailyBriefingState` | `DailyBriefingSaveStore` | `DailyBriefingModal` | `--day1-selftest`, `DailyBriefingReportBuilderTests` | ✅ 6/6 |
| 11 | `library_study` | Campaign & Progression | `LibraryStudySystem` | `library_manuals.json` | `LibraryStudyHostSession` | `LibraryStudySaveStore` | `LibraryStudyPanel` | `--shelter-operations-selftest`, `LibraryStudySystemTests` | ✅ 6/6 |
| 12 | `research` | Campaign & Progression | `ResearchSystem`, `ResearchKnowledgeCatalogLoader` | `research_knowledge.json` | `ResearchHostSession` | `ResearchSaveStore` | `ResearchPanel`, `ResearchAtlasPanel` | `--research-catalog-selftest`, `ResearchCatalogParityTests`, `ResearchSaveIntegrationTests` | ✅ 6/6 |
| 67 | `field_guide` | World & Expeditions | `FieldGuideCatalog` | `field_guide_entries.json` *(or procedural)* | `Main.FieldGuide` (world day owner) | `FieldGuideSaveStore` | *(journal/briefing projections)* | `--evolving-world-selftest` | ✅ 6/6 |
| 12 | `caravan` | Economy & Trade | `TravelingCaravanSystem` | `trade_texts.json` | `TravelingCaravanHostSession` | `CaravanSaveStore` | `TravelingCaravanPanel` | `--caravan-selftest`, `TradeCaravanCatalogTests` | ✅ 6/6 |
| 13 | `economy` | Economy & Trade | `MarketSystem` | `economy_goods.json` | `EconomyHostSession` | `EconomySaveStore` | `EconomyMarketPanel`, `EconomyDetailPanel` | `--economy-selftest`, `--economy-uitest`, `DynamicEconomyCharacterizationTests` | ✅ 6/6 |
| 14 | `regional_treaty` | Economy & Trade | `RegionalTreatySystem` | `faction_lore.json` | `RegionalTreatyHostSession` | `RegionalTreatySaveStore` | `RegionalTreatyPanel` | `--shelter-operations-selftest`, `RegionalTreatySaveChecksumTests` | ✅ 6/6 |
| 15 | `expansion_hub` | Expansion Framework | `ExpansionMasterSession` | — *(Procedural)* | `ExpansionHostSession` | `ExpansionHubSaveStore` | `ExpansionsHubPanel` | `--expansions-selftest`, `--expansion-hub-save-selftest`, `ExpansionHubSaveTests` | ✅ 6/6 |
| 16 | `expansion_quest` | Expansion Framework | `ExpansionQuestSystem`, `ExpansionMasterSession` | `crossing_quests.json` | `ExpansionQuestHostSession` | `ExpansionQuestSaveStore` | `CrossingQuestPanel` | `--expansions-selftest`, `VersionReportContractTests` | ✅ 6/6 |
| 17 | `holdfast` | Expansions (Exp 01) | `HoldfastQuestSystem`, `HoldfastSession` | `holdfast_quests.json`, `holdfast_items.json` | `HoldfastRuntimeSession` | `HoldfastSaveStore` | `HoldfastTerminalPanel`, `GameDashboardPanel` | `--holdfast-save-selftest`, `--holdfast-selftest`, `HoldfastSaveTests` | ✅ 6/6 |
| 18 | `holdfast_trade` | Expansions (Exp 01) | `HoldfastTradeSession` | `items.json` | `HoldfastRuntimeSession` | `HoldfastTradeSaveStore` | `TradeScreenGodotPanel`, `HoldfastTerminalPanel` | `--holdfast-trade-save-selftest`, `HoldfastTradeSessionTests` | ✅ 6/6 |
| 19 | `duty_roster` | Expansions (Exp 02) | `DutyRosterSystem` | `duty_roster_quests.json`, `survivors.json` | `DutyRosterHostSession` | `DutyRosterSaveStore` | `DutyRosterPanel`, `DutyRosterDetailPanel` | `--duty-roster-selftest`, `--duty-roster-save-selftest`, `DutyRosterSaveTests` | ✅ 6/6 |
| 20 | `phantom_memory` | Expansions (Exp 03) | `PhantomMemoryEngine` | `phantom_triggers.json` | `PhantomMemoryHostSession` | `PhantomMemorySaveStore` | `StandingRecordPanel`, `PhantomMemoryPanel` | `--standing-record-selftest`, `PhantomMemoryEngineTests` | ✅ 6/6 |
| 21 | `thirdonary` | Expansions (Exp 04) | `ThirdonaryQuestSystem` | `thirdonary_quests.json` | `ThirdonaryHostSession` | `ThirdonarySaveStore` | `CrossingQuestPanel` | `--crossing-selftest`, `--arbitration-selftest`, `ThirdonaryQuestSystemTests`, `CrossingArbitrationSystemTests` | ✅ 6/6 |
| 22 | `year_of_ash` | Expansions (Exp 05) | `YearOfAshDeepFreezeSystem`, `YearOfAshRadonSystem` | `year_of_ash_events.json` | `YearOfAshHostSession` | `YearOfAshSaveStore` | `DoorEncounterModal` | `--year-of-ash-save-selftest`, `YearOfAshQuestProbe` | ✅ 6/6 |
| 23 | `muster` | Expansions (Exp 06) | `MusterSystem` | `muster_witnesses.json` | `MusterHostSession` | `MusterSaveStore` | `MusterPanel` | `--muster-selftest`, `--muster-uitest`, `MusterSystemTests` | ✅ 6/6 |
| 24 | `dose_ledger` | Expansions (Exp 07) | `DoseLedgerSystem`, `RadiationSystem` | `dose_items.json` | `DoseLedgerHostSession` | `DoseLedgerSaveStore` | `RadiationHistoryPanel`, `RadiationDetailPanel` | `--dose-ledger-selftest`, `--dose-uitest`, `NeedsRadiationSaveRoundTripTests` | ✅ 6/6 |
| 25 | `verdict` | Expansions (Exp 08) | `ReckoningSystem`, `MachineLogSystem` | `verdict_data.json` | `VerdictHostSession` | `VerdictSaveStore` | `VerdictPanel`, `VerdictDashboardPanel` | `--verdict-selftest`, `--verdict-uitest`, `VerdictChainTests` | ✅ 6/6 |
| 26 | `maritime` | Expansions (Exp 09) | `MaritimeDiveSystem` | `dive_sites.json` | `MaritimeHostSession` | `MaritimeSaveStore` | `MaritimePanel` | `--black-flotilla-selftest`, `BlackFlotillaTests` | ✅ 6/6 |
| 27 | `silent_foundry` | Expansions (Exp 10) | `SilentFoundrySystem` | `foundry_items.json` | `SilentFoundryHostSession` | `SilentFoundrySaveStore` | `SilentFoundryPanel` | `--silent-foundry-selftest`, `--silent-foundry-uitest`, `SilentFoundryConsequenceTests` | ✅ 6/6 |
| 28 | `weight_of_choices` | Factions & Diplomacy | `FactionBranchCoordinator`, `MilitaryBranchSystem`, `RebelBranchSystem`, `IndependentBranchSystem`, `PrpfStandingSystem` | `military_faction_branch.json`, `rebel_faction_branch.json`, `independent_faction_branch.json` | `FactionBranchHostSession` | `WeightOfChoicesSaveStore` | `FactionsPanel`, `QuestsPanel` | `--expansions-selftest`, `FactionBranchCoordinatorTests`, `MilitaryBranchSystemTests`, `RebelBranchSystemTests`, `IndependentBranchSystemTests`, `PrpfStandingSystemTests`, `WeightOfChoicesSaveTests` | ✅ 6/6 |
| 29 | `moral_choice` | Narrative & Decisions | `MoralChoiceSystem`, `MoralChoiceState` | `moral_choice_quests.json` | `MoralChoiceSystem` | `MoralChoiceSaveStore` | `GameDashboardPanel` | `--moral-choice-selftest`, `MoralChoiceSystemTests` | ✅ 6/6 |
| 30 | `airlock_security` | Shelter & Infrastructure | `AirlockSecuritySystem` | — *(Procedural)* | `AirlockSecurityHostSession` | `AirlockSecuritySaveStore` | `AirlockSecurityPanel` | `--shelter-operations-selftest`, `AirlockSecuritySystemTests` | ✅ 6/6 |
| 31 | `decontamination` | Shelter & Infrastructure | `DecontaminationSystem` | — *(Procedural)* | `DecontaminationHostSession` | `DecontaminationSaveStore` | `DecontaminationPanel` | `--shelter-operations-selftest`, `DecontaminationSystemTests` | ✅ 6/6 |
| 32 | `excavation` | Shelter & Infrastructure | `ExcavationSystem` | — *(Procedural)* | `ExcavationHostSession` | `ExcavationSaveStore` | `ExcavationPanel` | `--shelter-operations-selftest`, `ExcavationSystemTests` | ✅ 6/6 |
| 33 | `greenhouse` | Shelter & Infrastructure | `GreenhouseSystem` | `greenhouse_items.json` | `GreenhouseHostSession` | `GreenhouseSaveStore` | `GreenhousePanel` | `--greenhouse-selftest`, `GreenhouseSystemTests` | ✅ 6/6 |
| 34 | `power_grid` | Shelter & Infrastructure | `PowerGridSystem` | `power_grid.json` | `PowerGridHostSession` | `PowerGridSaveStore` | `PowerGridPanel` | `--player-panels-uitest`, `PowerGridSystemTests` | ✅ 6/6 |
| 35 | `shelter_assignment` | Shelter & Infrastructure | `ShelterAssignmentSystem` | — *(Procedural)* | `ShelterAssignmentHostSession` | `ShelterAssignmentSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `ShelterAssignmentSystemTests` | ✅ 6/6 |
| 36 | `shelter_decor` | Shelter & Infrastructure | `ShelterDecorSystem` | `items.json` | `ShelterDecorHostSession` | `ShelterDecorSaveStore` | `ShelterDecorPanel` | `--shelter-decor-selftest`, `Plan12CDecorTests` | ✅ 6/6 |
| 37 | `shelter_schedule` | Shelter & Infrastructure | `ShelterScheduleSystem` | `shelter_schedules.json` | `ShelterScheduleHostSession` | `ShelterScheduleSaveStore` | `ShelterSchedulePanel` | `--shelter-operations-selftest`, `ShelterScheduleIntegrationTests` | ✅ 6/6 |
| 37 | `shelter_thermal` | Shelter & Infrastructure | `ShelterThermalSystem` | — *(Procedural)* | `ShelterThermalHostSession` | `ShelterThermalSaveStore` | `ShelterThermalPanel` | `--shelter-operations-selftest`, `ShelterThermalSaveChecksumTests` | ✅ 6/6 |
| 38 | `starting_level` | Shelter & Infrastructure | `StartingLevelSystem` | — *(Procedural)* | `StartingLevelHostSession` | `StartingLevelSaveStore` | `OpeningProtocolModal` | `--playable-shell-selftest`, `StartingLevelSystemTests` | ✅ 6/6 |
| 39 | `sump_flooding` | Shelter & Infrastructure | `SumpFloodingSystem` | — *(Procedural)* | `SumpFloodingHostSession` | `SumpFloodingSaveStore` | `SumpFloodingPanel` | `--shelter-operations-selftest`, `SumpFloodingSaveChecksumTests` | ✅ 6/6 |
| 40 | `survivor_social` | Shelter & Infrastructure | `SurvivorSocialCoordinator`, `LeadershipSystem`, `IdeologicalFrictionSystem`, `RationConflictSystem`, `TraumaBondSystem`, `SkillAtrophySystem` | — *(Procedural)* | `SurvivorSocialCoordinator` | `SurvivorSocialSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `SurvivorSocialCoordinatorTests` | ✅ 6/6 |
| 41 | `vinyl_morale` | Shelter & Infrastructure | `VinylMoraleSystem` | — *(Procedural)* | `VinylMoraleHostSession` | `VinylMoraleSaveStore` | `VinylMoralePanel` | `--shelter-operations-selftest`, `VinylMoraleSaveChecksumTests` | ✅ 6/6 |
| 42 | `water_treatment` | Shelter & Infrastructure | `WaterTreatmentSystem` | — *(Procedural)* | `WaterTreatmentHostSession` | `WaterTreatmentSaveStore` | `WaterTreatmentPanel` | `--shelter-operations-selftest`, `WaterTreatmentSystemTests` | ✅ 6/6 |
| 43 | `crafting` | Shelter & Logistics | `CraftingSystem` | `recipes.json` | `CraftingHostSession` | `CraftingSaveStore` | `CraftingPanel` | `--shelter-operations-selftest`, `CraftingSystemTests` | ✅ 6/6 |
| 44 | `equipment_condition` | Shelter & Logistics | `EquipmentConditionSystem` | — *(Procedural)* | `EquipmentConditionHostSession` | `EquipmentConditionSaveStore` | `EquipmentConditionPanel` | `--shelter-operations-selftest`, `EquipmentConditionSystemTests` | ✅ 6/6 |
| 45 | `inventory` | Shelter & Logistics | `Inventory` | `items.json` | `InventoryHostSession` | `InventorySaveStore` | `InventoryPanel`, `InventoryDetailPanel` | `--inventory-save-selftest`, `--inventory-uitest`, `InventorySystemTests` | ✅ 6/6 |
| 46 | `kitchen_nutrition` | Shelter & Logistics | `KitchenNutritionSystem` | — *(Procedural)* | `KitchenNutritionHostSession` | `KitchenNutritionSaveStore` | `KitchenNutritionPanel` | `--shelter-operations-selftest`, `KitchenNutritionSystemTests` | ✅ 6/6 |
| 47 | `radio` | Shelter & Logistics | `FactionRadioEngine` | `radio.json` | `RadioHostSession` | `RadioSaveStore` | `RadioPanel`, `FactionRadioHudPanel` | `--radio-selftest`, `RadioSaveCodecTests` | ✅ 6/6 |
| 48 | `apprenticeship` | Survival & Biology | `ApprenticeshipSystem` | — *(Procedural)* | `ApprenticeshipHostSession` | `ApprenticeshipSaveStore` | `ApprenticeshipPanel` | `--shelter-operations-selftest`, `ApprenticeshipSystemTests` | ✅ 6/6 |
| 49 | `autopsy` | Survival & Biology | `AutopsySystem` | `autopsy_procedures.json` | `AutopsyHostSession` | `AutopsySaveStore` | `AutopsyReportPanel` | `--shelter-operations-selftest`, `AutopsySystemTests` | ✅ 6/6 |
| 50 | `caregiving` | Survival & Biology | `CaregivingSystem` | — *(Procedural)* | `CaregivingHostSession` | `CaregivingSaveStore` | `CaregivingPanel` | `--shelter-operations-selftest`, `CaregivingSystemTests` | ✅ 6/6 |
| 51 | `chemical_dependency` | Survival & Biology | `ChemicalDependencySystem` | `chemical_dependency_items.json` | `MentalHealthCrisisHostSession`, `ChemicalDependencyHostSession` | `ChemicalDependencySaveStore` | `ChemicalDependencyPanel` | `--chemical-dependency-save-selftest`, `ChemicalDependencySaveSealTests` | ✅ 6/6 |
| 52 | `contractor_roster` | Survival & Biology | `ContractorRosterSystem` | — *(Procedural)* | `ContractorRosterHostSession` | `ContractorRosterSaveStore` | `ContractorRosterPanel` | `--shelter-operations-selftest`, `ContractorRosterSystemTests` | ✅ 6/6 |
| 53 | `disease` | Survival & Biology | `DiseaseSystem` | `disease_catalog.json` | `DiseaseHostSession` | `DiseaseSaveStore` | `AfflictionsPanel` | `--disease-selftest`, `DiseaseSystemTests` | ✅ 6/6 |
| 54 | `medical` | Survival & Biology | `MedicalWardSystem`, `SickListSystem` | `medical_texts.json` | `MedicalHostSession` | `MedicalSaveStore` | `MedicalPanel`, `AfflictionsPanel` | `--medical-selftest`, `DwellerMedicalCatalogTests` | ✅ 6/6 |
| 55 | `medical_ward` | Survival & Biology | `MedicalWardSystem` | — *(Procedural)* | `MedicalWardHostSession` | `MedicalWardSaveStore` | `MedicalWardPanel` | `--medical-ward-save-selftest`, `MedicalWardSystemTests` | ✅ 6/6 |
| 56 | `mental_health_crisis` | Survival & Biology | `MentalHealthCrisisSystem` | — *(Procedural)* | `MentalHealthCrisisHostSession` | `MentalHealthCrisisSaveStore` | `MentalHealthCrisisPanel` | `--shelter-operations-selftest`, `MentalHealthCrisisSystemTests` | ✅ 6/6 |
| 57 | `survivor_relations` | Survival & Biology | `SurvivorRelationsSystem` | — *(Procedural)* | `SurvivorRelationsHostSession` | `SurvivorRelationsSaveStore` | `SurvivorRelationsPanel` | `--shelter-operations-selftest`, `SurvivorRelationsSaveChecksumTests` | ✅ 6/6 |
| 58 | `survivors` | Survival & Biology | `NeedsSystem`, `SurvivorRosterSystem` | `survivors.json` | `SurvivorsHostSession` | `SurvivorsSaveStore` | `SurvivorsPanel`, `SurvivorDetailPanel`, `StatusPanel` | `--survivors-selftest`, `--survivors-uitest`, `--player-panels-uitest`, `NeedsSystemTests` | ✅ 6/6 |
| 59 | `combat` | Tactical Combat | `TacticalCombatSystem`, `CombatTraumaSystem` | `combat_catalog.json` | `CombatHostSession` | `CombatSaveStore` | `CombatPanel`, `CombatDetailPanel`, `CombatHistoryPanel` | `--combat-selftest`, `CombatBallisticsTests` | ✅ 6/6 |
| 60 | `encounter_choice` | World & Expeditions | `EncounterChoiceResolver` | `door_encounters.json` | `EncounterChoiceState` | `EncounterChoiceSaveStore` | `DoorEncounterModal` | `--moral-choice-selftest`, `EncounterChoiceResolverTests` | ✅ 6/6 |
| 61 | `expedition` | World & Expeditions | `ExpeditionSystem`, `ExpeditionEncounterBridge` | `locations.json` | `ExpeditionHostSession` | `ExpeditionSaveStore` | `ExpeditionPanel` | `--expedition-selftest`, `--expedition-panel-uitest`, `ExpeditionCampSystemTests` | ✅ 6/6 |
| 62 | `wasteland_map` | World & Expeditions | `WastelandMapSystem` | `wasteland_map_v1.json` | `WorldHostSession` | `WastelandMapSaveStore` | `MapPanel` | `--world-selftest`, `WastelandMapPersistenceTests` | ✅ 6/6 |
| 63 | `waystation` | World & Expeditions | `WaystationSystem` | `locations.json` | `WaystationHostSession` | `WaystationSaveStore` | `WaystationNetworkPanel` | `--shelter-operations-selftest`, `WaystationSystemTests` | ✅ 6/6 |
| 64 | `wildlife_trapping` | World & Expeditions | `WildlifeTrappingSystem` | — *(Procedural)* | `WildlifeTrappingHostSession` | `WildlifeTrappingSaveStore` | `WildlifeTrappingPanel` | `--shelter-operations-selftest`, `WildlifeTrappingSystemTests` | ✅ 6/6 |
| 65 | `world` | World & Expeditions | `WastelandMapSystem`, `WeatherSystem` | `locations.json` | `WorldHostSession` | `WorldSaveStore` | `MapPanel`, `WeatherPanel` | `--world-selftest`, `WorldSaveablesTests` | ✅ 6/6 |
| 66 | `medical_pipeline` | Survival & Biology | `MedicalPipelineCoordinator`, `DiagnosisKnowledgeStore`, `MedicalReservationLedger`, `MedicalProcedureSchedule` | — *(Projection DTOs; no JSON catalog)* | `MedicalHostSession` | `MedicalPipelineSaveStore` | `MedicalPanel` | `MedicalPipelineTests`, `MedicalPipelineArchitectureGateTests` | ✅ 6/6 |
| 67 | `ecological_infestation` | World & Expeditions | `EcologicalInfestationSystem` | `ecological_infestations.json`, `disease_catalog.json`, `items.json`, `locations.json` | `Main.EcologicalInfestations` (world_evolution day owner) | `EcologicalInfestationSaveStore` | *(journal/briefing projections; no panel — Phase 6)* | `--evolving-world-selftest`, `EcologicalInfestationSystemTests` | ✅ 6/6 |
| 68 | `shelter_workshop` | Shelter Operations | `ShelterWorkshopSystem` | `workshop_recipes.json` | `Main.ShelterWorkshop` | `ShelterWorkshopSaveStore` | `WorkshopPanel` | `ShelterWorkshopTests`, `Plans46_49_CrossSystemIntegrationTests` | ✅ 6/6 |
| 69 | `radio_station` | Radio & Communications | `ShelterRadioStationSystem` | `radio_intercepts.json` | `Main.RadioStation` | `RadioStationSaveStore` | `RadioStationPanel` | `ShelterRadioStationTests`, `Plans46_49_CrossSystemIntegrationTests` | ✅ 6/6 |
| 70 | `shelter_social_dynamics` | Shelter Operations | `ShelterSocialDynamicsSystem` | `shelter_social_events.json` | `Main.ShelterSocialDynamics` | `ShelterSocialSaveStore` | `SurvivorRelationsPanel` | `ShelterSocialDynamicsTests`, `Plans46_49_CrossSystemIntegrationTests` | ✅ 6/6 |
| 71 | `excavation_hazards` | Shelter Operations | `ExcavationHazardSystem` | `excavation_hazard_mitigation.json` | `Main.ExcavationHazards` | `ExcavationHazardSaveStore` | `ExcavationPanel` | `ExcavationHazardSystemTests`, `Plans46_49_CrossSystemIntegrationTests` | ✅ 6/6 |

---

## 3. Subsystem Deep Evidence Graph & Source Paths

Detailed file paths and symbols proving zero conceptual placeholders:

### 1. `host_event` — Host event ledger & moral decisions (Campaign & Lore)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupEventAdapter()` | **Cadence:** `On-Demand (Moral Dilemma)`
- **UI Routes:** `event_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Host Session: [`src/Host/HostEventAdapter.cs`](../../src/Host/HostEventAdapter.cs)
  - Save Store: [`src/Host/HostEventSaveStore.cs`](../../src/Host/HostEventSaveStore.cs)
  - Save Store: [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs)
  - UI Panel: [`src/UI/EventDetailPanel.cs`](../../src/UI/EventDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BareSaveStoreSealTests.cs`](../../Ashfall.Core.Tests/BareSaveStoreSealTests.cs)

### 2. `journal` — Player journal, logs, and codex entries (Campaign & Lore)
- **Owner Domain:** `journal`
- **Setup Method:** `Main.SetupJournal()` | **Cadence:** `On-Demand (Log/Event)`
- **UI Routes:** `journal`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Journal/JournalSystem.cs`](../../Assets/Ashfall.Core/Journal/JournalSystem.cs)
  - Host Session: [`src/Host/JournalHostSession.cs`](../../src/Host/JournalHostSession.cs)
  - Save Store: [`src/Journal/JournalSaveStore.cs`](../../src/Journal/JournalSaveStore.cs)
  - UI Panel: [`src/Journal/JournalBookUI.cs`](../../src/Journal/JournalBookUI.cs)
  - UI Panel: [`src/UI/JournalPanel.cs`](../../src/UI/JournalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/JournalSystemTests.cs`](../../Ashfall.Core.Tests/JournalSystemTests.cs)

### 3. `memorial` — Fallen survivors memorial wall (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupMemorial()` | **Cadence:** `On-Demand (Survivor Fallen Eulogy)`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Save Store: [`src/Host/MemorialSaveStore.cs`](../../src/Host/MemorialSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`](../../Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs)

### 4. `narrative` — Branching story arcs and narrative flags (Campaign & Lore)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupNarrative()` | **Cadence:** `On-Demand (Dialog Choice)`
- **UI Routes:** `journal`, `event_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs)
  - Host Session: [`src/Host/NarrativeHostSession.cs`](../../src/Host/NarrativeHostSession.cs)
  - Save Store: [`src/Host/NarrativeSaveStore.cs`](../../src/Host/NarrativeSaveStore.cs)
  - UI Panel: [`src/UI/EventsLogPanel.cs`](../../src/UI/EventsLogPanel.cs)
  - UI Panel: [`src/UI/FactionsNarrativePanel.cs`](../../src/UI/FactionsNarrativePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`](../../Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs)

### 5. `phase0` — Pre-war timeline and bunker startup (Campaign & Lore)
- **Owner Domain:** `phase0`
- **Setup Method:** `Main.SetupPhase0()` | **Cadence:** `On-Demand (Pre-War Flashback)`
- **UI Routes:** `phase0`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs`](../../Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs)
  - Host Session: [`src/Host/Phase0HostSession.cs`](../../src/Host/Phase0HostSession.cs)
  - Save Store: [`src/Host/Phase0SaveStore.cs`](../../src/Host/Phase0SaveStore.cs)
  - UI Panel: [`src/UI/Phase0Panel.cs`](../../src/UI/Phase0Panel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`](../../Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs)

### 6. `survivor_fate` — Unified survivor-death ledger: one immutable fate record per deceased survivor (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupSurvivorFate()` | **Cadence:** `Daily Survivor-Death Cascade`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs)
  - Save Store: [`src/Host/SurvivorFateSaveStore.cs`](../../src/Host/SurvivorFateSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SurvivorFateSystemTests.cs`](../../Ashfall.Core.Tests/SurvivorFateSystemTests.cs)

### 7. `onboarding` — First-hour onboarding journey progress, dismissed hints, assistance level, completion (Campaign & Onboarding)
- **Owner Domain:** `onboarding`
- **Setup Method:** `Main.SetupOnboarding()` | **Cadence:** `On-Demand (Player Sigil Recording)`
- **UI Routes:** `help`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`](../../Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs)
  - Save Store: [`src/Host/OnboardingSaveStore.cs`](../../src/Host/OnboardingSaveStore.cs)
  - UI Panel: [`src/UI/OnboardingHintPanel.cs`](../../src/UI/OnboardingHintPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/OnboardingJourneyTests.cs`](../../Ashfall.Core.Tests/OnboardingJourneyTests.cs)

### 8. `archive_desk` — Document archiving, ink, and scribing (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupArchiveDesk()` | **Cadence:** `Daily Scribing & Folio Archival`
- **UI Routes:** `archive_desk`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ArchiveDeskSystem.cs`](../../Assets/Ashfall.Core/ArchiveDeskSystem.cs)
  - Host Session: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - Save Store: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - UI Panel: [`src/UI/ArchiveDeskPanel.cs`](../../src/UI/ArchiveDeskPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`](../../Ashfall.Core.Tests/ArchiveDeskSystemTests.cs)

### 9. `campaign_day` — Master campaign day counter & ticks (Campaign & Progression)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCampaignDay()` | **Cadence:** `Master Sim Clock / Dawn Advance`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Host Session: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Save Store: [`src/Host/CampaignDaySaveStore.cs`](../../src/Host/CampaignDaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`](../../Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs)

### 10. `daily_briefing` — Daily dawn briefing notes & status (Campaign & Progression)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupDailyBriefingModal()` | **Cadence:** `Daily Dawn Briefing Aggregation`
- **UI Routes:** `briefing`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs)
  - Core System: [`Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs)
  - Host Session: [`Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs)
  - Save Store: [`src/Host/DailyBriefingSaveStore.cs`](../../src/Host/DailyBriefingSaveStore.cs)
  - UI Panel: [`src/UI/DailyBriefingModal.cs`](../../src/UI/DailyBriefingModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/DailyBriefingReportBuilderTests.cs`](../../Ashfall.Core.Tests/Campaign/DailyBriefingReportBuilderTests.cs)

### 11. `library_study` — Research library books and blueprints (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupLibraryStudy()` | **Cadence:** `Daily Codex Research Ticks`
- **UI Routes:** `library_study`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/LibraryStudySystem.cs`](../../Assets/Ashfall.Core/LibraryStudySystem.cs)
  - Host Session: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - Save Store: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - UI Panel: [`src/UI/LibraryStudyPanel.cs`](../../src/UI/LibraryStudyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/LibraryStudySystemTests.cs`](../../Ashfall.Core.Tests/LibraryStudySystemTests.cs)

### 12. `research` — Research knowledge progress: unlocked, active, and completed nodes (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** *None (lazy)* — `Main.EnsureSharedResearch()` constructs, restores, and loads the catalog on first research consumer
- **UI Routes:** `research`, `research_atlas`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Research/ResearchSystem.cs`](../../Assets/Ashfall.Core/Research/ResearchSystem.cs)
  - Catalog Loader: [`Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs`](../../Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs)
  - Host Session: [`src/Host/ResearchHostSession.cs`](../../src/Host/ResearchHostSession.cs)
  - Save Store: [`src/Host/ResearchSaveStore.cs`](../../src/Host/ResearchSaveStore.cs)
  - UI Panel: [`src/UI/ResearchPanel.cs`](../../src/UI/ResearchPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ResearchCatalogParityTests.cs`](../../Ashfall.Core.Tests/ResearchCatalogParityTests.cs)

### 12. `caravan` — Trade caravans, routes, and arrivals (Economy & Trade)
- **Owner Domain:** `caravans`
- **Setup Method:** `Main.SetupCaravans()` | **Cadence:** `Daily Route Travel`
- **UI Routes:** `traveling_caravan`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/TravelingCaravanSystem.cs`](../../Assets/Ashfall.Core/TravelingCaravanSystem.cs)
  - Host Session: [`src/Host/TravelingCaravanHostSession.cs`](../../src/Host/TravelingCaravanHostSession.cs)
  - Save Store: [`src/Host/CaravanSaveStore.cs`](../../src/Host/CaravanSaveStore.cs)
  - UI Panel: [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TradeCaravanCatalogTests.cs`](../../Ashfall.Core.Tests/TradeCaravanCatalogTests.cs)

### 13. `economy` — Dynamic economy rates and market orders (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupEconomy()` | **Cadence:** `Daily Market Rate Tick`
- **UI Routes:** `trade`, `economy_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/MarketSystem.cs`](../../Assets/Ashfall.Core/Economy/MarketSystem.cs)
  - Host Session: [`src/Host/EconomyHostSession.cs`](../../src/Host/EconomyHostSession.cs)
  - Save Store: [`src/Host/EconomySaveStore.cs`](../../src/Host/EconomySaveStore.cs)
  - UI Panel: [`src/Economy/EconomyMarketPanel.cs`](../../src/Economy/EconomyMarketPanel.cs)
  - UI Panel: [`src/UI/EconomyDetailPanel.cs`](../../src/UI/EconomyDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DynamicEconomyCharacterizationTests.cs`](../../Ashfall.Core.Tests/DynamicEconomyCharacterizationTests.cs)

### 14. `regional_treaty` — Faction treaties and non-aggression pacts (Economy & Trade)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupRegionalTreaty()` | **Cadence:** `Daily Non-Aggression Decay`
- **UI Routes:** `regional_treaty`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/RegionalTreatySystem.cs`](../../Assets/Ashfall.Core/RegionalTreatySystem.cs)
  - Host Session: [`src/Host/RegionalTreatyHostSession.cs`](../../src/Host/RegionalTreatyHostSession.cs)
  - Save Store: [`src/Host/RegionalTreatySaveStore.cs`](../../src/Host/RegionalTreatySaveStore.cs)
  - UI Panel: [`src/UI/RegionalTreatyPanel.cs`](../../src/UI/RegionalTreatyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 15. `expansion_hub` — Expansion hub discovery state (Expansion Framework)
- **Owner Domain:** `expansion_hub`
- **Setup Method:** `Main.SetupExpansions()` | **Cadence:** `Daily Hub Tick`
- **UI Routes:** `expansions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExpansionMasterSession.cs`](../../Assets/Ashfall.Core/ExpansionMasterSession.cs)
  - Host Session: [`src/Host/ExpansionHostSession.cs`](../../src/Host/ExpansionHostSession.cs)
  - Save Store: [`src/Host/ExpansionHubSaveStore.cs`](../../src/Host/ExpansionHubSaveStore.cs)
  - UI Panel: [`src/UI/ExpansionsHubPanel.cs`](../../src/UI/ExpansionsHubPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpansionHubSaveTests.cs`](../../Ashfall.Core.Tests/ExpansionHubSaveTests.cs)

### 16. `expansion_quest` — Expansion questline progression (Expansion Framework)
- **Owner Domain:** `expansion_quest`
- **Setup Method:** `Main.SetupExpansionQuests()` | **Cadence:** `On-Demand (Stage Milestone)`
- **UI Routes:** `crossing_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExpansionMasterSession.cs`](../../Assets/Ashfall.Core/ExpansionMasterSession.cs)
  - Core System: [`Assets/Ashfall.Core/ExpansionQuestSystem.cs`](../../Assets/Ashfall.Core/ExpansionQuestSystem.cs)
  - Host Session: [`src/Host/ExpansionQuestHostSession.cs`](../../src/Host/ExpansionQuestHostSession.cs)
  - Save Store: [`src/Host/ExpansionQuestSaveStore.cs`](../../src/Host/ExpansionQuestSaveStore.cs)
  - UI Panel: [`src/UI/CrossingQuestPanel.cs`](../../src/UI/CrossingQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/VersionReportContractTests.cs`](../../Ashfall.Core.Tests/VersionReportContractTests.cs)

### 17. `holdfast` — Holdfast S1 bunker state (Expansions (Exp 01))
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupHoldfastRuntime()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `holdfast`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HoldfastQuestSystem.cs`](../../Assets/Ashfall.Core/HoldfastQuestSystem.cs)
  - Core System: [`Assets/Ashfall.Core/HoldfastSession.cs`](../../Assets/Ashfall.Core/HoldfastSession.cs)
  - Host Session: [`src/Host/HoldfastRuntimeSession.cs`](../../src/Host/HoldfastRuntimeSession.cs)
  - Save Store: [`src/Host/HoldfastSaveStore.cs`](../../src/Host/HoldfastSaveStore.cs)
  - UI Panel: [`src/Host/HoldfastTerminalPanel.cs`](../../src/Host/HoldfastTerminalPanel.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HoldfastSaveTests.cs`](../../Ashfall.Core.Tests/HoldfastSaveTests.cs)

### 18. `holdfast_trade` — Holdfast trade session state (Expansions (Exp 01))
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupHoldfastRuntime()` | **Cadence:** `On-Demand (Barter)`
- **UI Routes:** `trade`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HoldfastTradeSession.cs`](../../Assets/Ashfall.Core/HoldfastTradeSession.cs)
  - Host Session: [`src/Host/HoldfastRuntimeSession.cs`](../../src/Host/HoldfastRuntimeSession.cs)
  - Save Store: [`src/Host/HoldfastTradeSaveStore.cs`](../../src/Host/HoldfastTradeSaveStore.cs)
  - UI Panel: [`src/Economy/TradeScreenGodotPanel.cs`](../../src/Economy/TradeScreenGodotPanel.cs)
  - UI Panel: [`src/Host/HoldfastTerminalPanel.cs`](../../src/Host/HoldfastTerminalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HoldfastTradeSessionTests.cs`](../../Ashfall.Core.Tests/HoldfastTradeSessionTests.cs)

### 19. `duty_roster` — Duty roster shifts and assignments (Expansions (Exp 02))
- **Owner Domain:** `duty_roster`
- **Setup Method:** `Main.SetupDutyRoster()` | **Cadence:** `Daily Shift Tick`
- **UI Routes:** `duty_roster`, `duty_roster_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`](../../Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs)
  - Host Session: [`src/Host/DutyRosterHostSession.cs`](../../src/Host/DutyRosterHostSession.cs)
  - Save Store: [`src/Host/DutyRosterSaveStore.cs`](../../src/Host/DutyRosterSaveStore.cs)
  - UI Panel: [`src/UI/DutyRosterDetailPanel.cs`](../../src/UI/DutyRosterDetailPanel.cs)
  - UI Panel: [`src/UI/DutyRosterPanel.cs`](../../src/UI/DutyRosterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DutyRosterSaveTests.cs`](../../Ashfall.Core.Tests/DutyRosterSaveTests.cs)

### 20. `phantom_memory` — Phantom memory lineages and echoes (Expansions (Exp 03))
- **Owner Domain:** `phase0`
- **Setup Method:** `Main.SetupPhantom()` | **Cadence:** `On-Demand (Scavenge Echo)`
- **UI Routes:** `standing_record`, `phantom_memory`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/PhantomMemoryEngine.cs`](../../Assets/Ashfall.Core/PhantomMemoryEngine.cs)
  - Host Session: [`src/Host/PhantomMemoryHostSession.cs`](../../src/Host/PhantomMemoryHostSession.cs)
  - Save Store: [`src/Host/PhantomMemorySaveStore.cs`](../../src/Host/PhantomMemorySaveStore.cs)
  - UI Panel: [`src/UI/PhantomMemoryPanel.cs`](../../src/UI/PhantomMemoryPanel.cs)
  - UI Panel: [`src/UI/StandingRecordPanel.cs`](../../src/UI/StandingRecordPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PhantomMemoryEngineTests.cs`](../../Ashfall.Core.Tests/PhantomMemoryEngineTests.cs)

### 21. `thirdonary` — Thirdonary covenant & dispute states (Expansions (Exp 04))
- **Owner Domain:** `thirdonary`
- **Setup Method:** `Main.SetupThirdonary()` | **Cadence:** `On-Demand (Arbitration)`
- **UI Routes:** `crossing_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Thirdonary/ThirdonaryQuestSystem.cs`](../../Assets/Ashfall.Core/Thirdonary/ThirdonaryQuestSystem.cs)
  - Host Session: [`src/Host/ThirdonaryHostSession.cs`](../../src/Host/ThirdonaryHostSession.cs)
  - Save Store: [`src/Host/ThirdonarySaveStore.cs`](../../src/Host/ThirdonarySaveStore.cs)
  - UI Panel: [`src/UI/CrossingQuestPanel.cs`](../../src/UI/CrossingQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`](../../Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs`](../../Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs)

### 22. `year_of_ash` — The Year of Ash harsh winter state (Expansions (Exp 05))
- **Owner Domain:** `year_of_ash`
- **Setup Method:** `Main.SetupYearOfAsh()` | **Cadence:** `Daily Deep-Freeze Tick`
- **UI Routes:** `door_encounter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs`](../../Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs)
  - Core System: [`Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs`](../../Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs)
  - Host Session: [`src/YearOfAsh/YearOfAshHostSession.cs`](../../src/YearOfAsh/YearOfAshHostSession.cs)
  - Save Store: [`src/YearOfAsh/YearOfAshSaveStore.cs`](../../src/YearOfAsh/YearOfAshSaveStore.cs)
  - UI Panel: [`src/YearOfAsh/DoorEncounterModal.cs`](../../src/YearOfAsh/DoorEncounterModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs`](../../Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs)

### 23. `muster` — The Muster military rally & conflict state (Expansions (Exp 06))
- **Owner Domain:** `muster`
- **Setup Method:** `Main.SetupMuster()` | **Cadence:** `On-Demand (Rally Stance)`
- **UI Routes:** `muster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Muster/MusterSystem.cs`](../../Assets/Ashfall.Core/Muster/MusterSystem.cs)
  - Host Session: [`src/Host/MusterHostSession.cs`](../../src/Host/MusterHostSession.cs)
  - Save Store: [`src/Host/MusterSaveStore.cs`](../../src/Host/MusterSaveStore.cs)
  - UI Panel: [`src/UI/MusterPanel.cs`](../../src/UI/MusterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MusterSystemTests.cs`](../../Ashfall.Core.Tests/MusterSystemTests.cs)

### 24. `dose_ledger` — Survivor radiation dose ledger & cohorts (Expansions (Exp 07))
- **Owner Domain:** `dose_ledger`
- **Setup Method:** `Main.SetupDoseLedger()` | **Cadence:** `On-Demand (Dose Log)`
- **UI Routes:** `radiation_history`, `radiation_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DoseLedgerSystem.cs`](../../Assets/Ashfall.Core/DoseLedgerSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`](../../Assets/Ashfall.Core/Radiation/RadiationSystem.cs)
  - Host Session: [`src/Host/DoseLedgerHostSession.cs`](../../src/Host/DoseLedgerHostSession.cs)
  - Save Store: [`src/Host/DoseLedgerSaveStore.cs`](../../src/Host/DoseLedgerSaveStore.cs)
  - UI Panel: [`src/UI/RadiationDetailPanel.cs`](../../src/UI/RadiationDetailPanel.cs)
  - UI Panel: [`src/UI/RadiationHistoryPanel.cs`](../../src/UI/RadiationHistoryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs`](../../Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs)

### 25. `verdict` — The Verdict investigation and tribunal state (Expansions (Exp 08))
- **Owner Domain:** `verdict`
- **Setup Method:** `Main.SetupVerdict()` | **Cadence:** `Daily Machine Log Tick`
- **UI Routes:** `verdict`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`](../../Assets/Ashfall.Core/Verdict/MachineLogSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`](../../Assets/Ashfall.Core/Verdict/ReckoningSystem.cs)
  - Host Session: [`src/Host/VerdictHostSession.cs`](../../src/Host/VerdictHostSession.cs)
  - Save Store: [`src/Host/VerdictSaveStore.cs`](../../src/Host/VerdictSaveStore.cs)
  - UI Panel: [`src/UI/VerdictDashboardPanel.cs`](../../src/UI/VerdictDashboardPanel.cs)
  - UI Panel: [`src/VerdictPanel.cs`](../../src/VerdictPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/VerdictChainTests.cs`](../../Ashfall.Core.Tests/VerdictChainTests.cs)

### 26. `maritime` — The Black Flotilla dives and naval wrecks (Expansions (Exp 09))
- **Owner Domain:** `maritime`
- **Setup Method:** `Main.SetupMaritime()` | **Cadence:** `On-Demand (Dive Sortie)`
- **UI Routes:** `maritime`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs)
  - Host Session: [`src/Host/MaritimeHostSession.cs`](../../src/Host/MaritimeHostSession.cs)
  - Save Store: [`src/Host/MaritimeSaveStore.cs`](../../src/Host/MaritimeSaveStore.cs)
  - UI Panel: [`src/UI/MaritimePanel.cs`](../../src/UI/MaritimePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BlackFlotillaTests.cs`](../../Ashfall.Core.Tests/BlackFlotillaTests.cs)

### 27. `silent_foundry` — Automated foundry machinery & smelters (Expansions (Exp 10))
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupSilentFoundry()` | **Cadence:** `Daily Smelter Cycle`
- **UI Routes:** `silent_foundry`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`](../../Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs)
  - Host Session: [`src/Foundry/SilentFoundryHostSession.cs`](../../src/Foundry/SilentFoundryHostSession.cs)
  - Save Store: [`src/Host/SilentFoundrySaveStore.cs`](../../src/Host/SilentFoundrySaveStore.cs)
  - UI Panel: [`src/UI/SilentFoundryPanel.cs`](../../src/UI/SilentFoundryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`](../../Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs)

### 28. `weight_of_choices` — Weight of choices faction branch progression and PoNR commitments (Factions & Diplomacy)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupFactionBranch()` | **Cadence:** `On-Demand (Branch Decisions)`
- **UI Routes:** `factions`, `quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`](../../Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs`](../../Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs`](../../Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs`](../../Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`](../../Assets/Ashfall.Core/Factions/RebelBranchSystem.cs)
  - Host Session: [`src/Host/FactionBranchHostSession.cs`](../../src/Host/FactionBranchHostSession.cs)
  - Save Store: [`src/Host/WeightOfChoicesSaveStore.cs`](../../src/Host/WeightOfChoicesSaveStore.cs)
  - UI Panel: [`src/UI/FactionsPanel.cs`](../../src/UI/FactionsPanel.cs)
  - UI Panel: [`src/UI/QuestsPanel.cs`](../../src/UI/QuestsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs`](../../Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/IndependentBranchSystemTests.cs`](../../Ashfall.Core.Tests/IndependentBranchSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MilitaryBranchSystemTests.cs`](../../Ashfall.Core.Tests/MilitaryBranchSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PrpfStandingSystemTests.cs`](../../Ashfall.Core.Tests/PrpfStandingSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/RebelBranchSystemTests.cs`](../../Ashfall.Core.Tests/RebelBranchSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WeightOfChoicesSaveTests.cs`](../../Ashfall.Core.Tests/WeightOfChoicesSaveTests.cs)

### 29. `moral_choice` — Moral choice ledger and community trust (Narrative & Decisions)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupMoralChoice()` | **Cadence:** `On-Demand (Branch Choice)`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs)
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Save Store: [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MoralChoiceSystemTests.cs`](../../Ashfall.Core.Tests/MoralChoiceSystemTests.cs)

### 30. `airlock_security` — Airlock decontamination and security (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupAirlockSecurity()` | **Cadence:** `Daily Decon Interlock`
- **UI Routes:** `airlock_security`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AirlockSecuritySystem.cs`](../../Assets/Ashfall.Core/AirlockSecuritySystem.cs)
  - Host Session: [`src/Host/AirlockSecurityHostSession.cs`](../../src/Host/AirlockSecurityHostSession.cs)
  - Save Store: [`src/Host/AirlockSecuritySaveStore.cs`](../../src/Host/AirlockSecuritySaveStore.cs)
  - UI Panel: [`src/UI/AirlockSecurityPanel.cs`](../../src/UI/AirlockSecurityPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AirlockSecuritySystemTests.cs`](../../Ashfall.Core.Tests/AirlockSecuritySystemTests.cs)

### 31. `decontamination` — Rad-scrubbing showers and chambers (Shelter & Infrastructure)
- **Owner Domain:** `radiation`
- **Setup Method:** `Main.SetupDecontamination()` | **Cadence:** `Daily Rad Scrub Shower Cycle`
- **UI Routes:** `decontamination`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DecontaminationSystem.cs`](../../Assets/Ashfall.Core/DecontaminationSystem.cs)
  - Host Session: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - Save Store: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - UI Panel: [`src/UI/DecontaminationPanel.cs`](../../src/UI/DecontaminationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DecontaminationSystemTests.cs`](../../Ashfall.Core.Tests/DecontaminationSystemTests.cs)

### 32. `excavation` — Shelter expansion rubble clearing (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupExcavation()` | **Cadence:** `Daily Rubble Shoring Work`
- **UI Routes:** `excavation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExcavationSystem.cs`](../../Assets/Ashfall.Core/ExcavationSystem.cs)
  - Host Session: [`src/Host/ExcavationHostSession.cs`](../../src/Host/ExcavationHostSession.cs)
  - Save Store: [`src/Host/ExcavationSaveStore.cs`](../../src/Host/ExcavationSaveStore.cs)
  - UI Panel: [`src/UI/ExcavationPanel.cs`](../../src/UI/ExcavationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExcavationSystemTests.cs`](../../Ashfall.Core.Tests/ExcavationSystemTests.cs)

### 33. `greenhouse` — Hydroponic crops and food production (Shelter & Infrastructure)
- **Owner Domain:** `greenhouse`
- **Setup Method:** `Main.SetupGreenhouse()` | **Cadence:** `Daily Hydroponic Growth`
- **UI Routes:** `greenhouse`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`](../../Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs)
  - Host Session: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - Save Store: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - UI Panel: [`src/UI/GreenhousePanel.cs`](../../src/UI/GreenhousePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GreenhouseSystemTests.cs`](../../Ashfall.Core.Tests/GreenhouseSystemTests.cs)

### 34. `power_grid` — Shelter generator & power allocations (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupPowerGrid()` | **Cadence:** `Daily Fuel Consumption & Wattage`
- **UI Routes:** `power_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`](../../Assets/Ashfall.Core/Shelter/PowerGridSystem.cs)
  - Host Session: [`src/Host/PowerGridHostSession.cs`](../../src/Host/PowerGridHostSession.cs)
  - Save Store: [`src/Host/PowerGridSaveStore.cs`](../../src/Host/PowerGridSaveStore.cs)
  - UI Panel: [`src/UI/PowerGridPanel.cs`](../../src/UI/PowerGridPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs)

### 35. `shelter_assignment` — Room assignments and living quarters (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAssignment()` | **Cadence:** `On-Demand (Bunk Reassignment)`
- **UI Routes:** `shelter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs)
  - Host Session: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - Save Store: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - UI Panel: [`src/UI/ShelterPanel.cs`](../../src/UI/ShelterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs)

### 36. `shelter_schedule` — Shift rotations and curfews (Shelter & Infrastructure)
- **Owner Domain:** `schedule`
- **Setup Method:** `Main.SetupShelterSchedule()` | **Cadence:** `Daily Curfew Rotation`
- **UI Routes:** `shelter_schedule`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterScheduleSystem.cs`](../../Assets/Ashfall.Core/ShelterScheduleSystem.cs)
  - Host Session: [`src/Host/ShelterScheduleHostSession.cs`](../../src/Host/ShelterScheduleHostSession.cs)
  - Save Store: [`src/Host/ShelterScheduleSaveStore.cs`](../../src/Host/ShelterScheduleSaveStore.cs)
  - UI Panel: [`src/UI/ShelterSchedulePanel.cs`](../../src/UI/ShelterSchedulePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs`](../../Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs)

### 37. `shelter_thermal` — Heating, insulation, and frost protection (Shelter & Infrastructure)
- **Owner Domain:** `thermal`
- **Setup Method:** `Main.SetupShelterThermal()` | **Cadence:** `Daily HVAC Frost Dissipation`
- **UI Routes:** `shelter_thermal`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterThermalSystem.cs`](../../Assets/Ashfall.Core/ShelterThermalSystem.cs)
  - Host Session: [`src/Host/ShelterThermalHostSession.cs`](../../src/Host/ShelterThermalHostSession.cs)
  - Save Store: [`src/Host/ShelterThermalSaveStore.cs`](../../src/Host/ShelterThermalSaveStore.cs)
  - UI Panel: [`src/UI/ShelterThermalPanel.cs`](../../src/UI/ShelterThermalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 38. `starting_level` — Bunker initial configuration & tier (Shelter & Infrastructure)
- **Owner Domain:** `starting_level`
- **Setup Method:** `Main.SetupStartingLevel()` | **Cadence:** `On-Demand (Opening Protocol)`
- **UI Routes:** `protocol`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`](../../Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs)
  - Host Session: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - Save Store: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - UI Panel: [`src/UI/OpeningProtocolModal.cs`](../../src/UI/OpeningProtocolModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/StartingLevelSystemTests.cs`](../../Ashfall.Core.Tests/StartingLevelSystemTests.cs)

### 39. `sump_flooding` — Bunker sump pump drainage & flood risk (Shelter & Infrastructure)
- **Owner Domain:** `maintenance`
- **Setup Method:** `Main.SetupSumpFlooding()` | **Cadence:** `Daily Drainage Pump Work`
- **UI Routes:** `sump_flooding`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SumpFloodingSystem.cs`](../../Assets/Ashfall.Core/SumpFloodingSystem.cs)
  - Host Session: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - Save Store: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - UI Panel: [`src/UI/SumpFloodingPanel.cs`](../../src/UI/SumpFloodingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs`](../../Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs)

### 40. `survivor_social` — Leadership, friction, ration conflict, trauma bonds, skill atrophy (Shelter & Infrastructure)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupSurvivorSocial()` | **Cadence:** `Daily Shelter Social Dynamics`
- **UI Routes:** `shelter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs`](../../Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`](../../Assets/Ashfall.Core/Survivors/LeadershipSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/RationConflictSystem.cs`](../../Assets/Ashfall.Core/Survivors/RationConflictSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`](../../Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs`](../../Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs)
  - Save Store: [`src/Host/SurvivorSocialSaveStore.cs`](../../src/Host/SurvivorSocialSaveStore.cs)
  - UI Panel: [`src/UI/ShelterPanel.cs`](../../src/UI/ShelterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs`](../../Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs)

### 41. `vinyl_morale` — Gramophone records and music morale (Shelter & Infrastructure)
- **Owner Domain:** `morale`
- **Setup Method:** `Main.SetupVinylMorale()` | **Cadence:** `Daily Turntable Morale Broadcast`
- **UI Routes:** `vinyl_morale`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/VinylMoraleSystem.cs`](../../Assets/Ashfall.Core/VinylMoraleSystem.cs)
  - Host Session: [`src/Host/VinylMoraleHostSession.cs`](../../src/Host/VinylMoraleHostSession.cs)
  - Save Store: [`src/Host/VinylMoraleSaveStore.cs`](../../src/Host/VinylMoraleSaveStore.cs)
  - UI Panel: [`src/UI/VinylMoralePanel.cs`](../../src/UI/VinylMoralePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 42. `water_treatment` — Water filtration and purification (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaterTreatment()` | **Cadence:** `Daily Filtration Cycle`
- **UI Routes:** `water_treatment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaterTreatmentSystem.cs`](../../Assets/Ashfall.Core/WaterTreatmentSystem.cs)
  - Host Session: [`src/Host/WaterTreatmentHostSession.cs`](../../src/Host/WaterTreatmentHostSession.cs)
  - Save Store: [`src/Host/WaterTreatmentSaveStore.cs`](../../src/Host/WaterTreatmentSaveStore.cs)
  - UI Panel: [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaterTreatmentSystemTests.cs`](../../Ashfall.Core.Tests/WaterTreatmentSystemTests.cs)

### 43. `crafting` — Known recipes and workbench queues (Shelter & Logistics)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupCrafting()` | **Cadence:** `Daily Workbench Queue`
- **UI Routes:** `crafting`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/CraftingSystem.cs`](../../Assets/Ashfall.Core/Crafting/CraftingSystem.cs)
  - Host Session: [`src/Host/CraftingHostSession.cs`](../../src/Host/CraftingHostSession.cs)
  - Save Store: [`src/Host/CraftingSaveStore.cs`](../../src/Host/CraftingSaveStore.cs)
  - UI Panel: [`src/UI/CraftingPanel.cs`](../../src/UI/CraftingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CraftingSystemTests.cs`](../../Ashfall.Core.Tests/CraftingSystemTests.cs)

### 44. `equipment_condition` — Tool and weapon wear/repair (Shelter & Logistics)
- **Owner Domain:** `equipment`
- **Setup Method:** `Main.SetupEquipmentCondition()` | **Cadence:** `Daily Gear Wear & Maintenance`
- **UI Routes:** `equipment_condition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/EquipmentConditionSystem.cs`](../../Assets/Ashfall.Core/EquipmentConditionSystem.cs)
  - Host Session: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - Save Store: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - UI Panel: [`src/UI/EquipmentConditionPanel.cs`](../../src/UI/EquipmentConditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EquipmentConditionSystemTests.cs`](../../Ashfall.Core.Tests/EquipmentConditionSystemTests.cs)

### 45. `inventory` — Shelter warehouse & items storage (Shelter & Logistics)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupInventory()` | **Cadence:** `On-Demand (Item Use)`
- **UI Routes:** `inventory`, `inventory_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Inventory/Inventory.cs`](../../Assets/Ashfall.Core/Inventory/Inventory.cs)
  - Host Session: [`src/Host/InventoryHostSession.cs`](../../src/Host/InventoryHostSession.cs)
  - Save Store: [`src/Host/InventorySaveStore.cs`](../../src/Host/InventorySaveStore.cs)
  - UI Panel: [`src/UI/InventoryDetailPanel.cs`](../../src/UI/InventoryDetailPanel.cs)
  - UI Panel: [`src/UI/InventoryPanel.cs`](../../src/UI/InventoryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/InventorySystemTests.cs`](../../Ashfall.Core.Tests/InventorySystemTests.cs)

### 46. `kitchen_nutrition` — Rationing recipes and caloric balance (Shelter & Logistics)
- **Owner Domain:** `nutrition`
- **Setup Method:** `Main.SetupKitchenNutrition()` | **Cadence:** `Daily Rationing Meal Prep`
- **UI Routes:** `kitchen_nutrition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/KitchenNutritionSystem.cs`](../../Assets/Ashfall.Core/KitchenNutritionSystem.cs)
  - Host Session: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - Save Store: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - UI Panel: [`src/UI/KitchenNutritionPanel.cs`](../../src/UI/KitchenNutritionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/KitchenNutritionSystemTests.cs`](../../Ashfall.Core.Tests/KitchenNutritionSystemTests.cs)

### 47. `radio` — Radio frequencies, logs, and distress signals (Shelter & Logistics)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadio()` | **Cadence:** `On-Demand (Frequency Scan)`
- **UI Routes:** `radio`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`](../../Assets/Ashfall.Core/Radio/FactionRadioEngine.cs)
  - Host Session: [`src/Host/RadioHostSession.cs`](../../src/Host/RadioHostSession.cs)
  - Save Store: [`src/Host/RadioSaveStore.cs`](../../src/Host/RadioSaveStore.cs)
  - UI Panel: [`src/Radio/FactionRadioHudPanel.cs`](../../src/Radio/FactionRadioHudPanel.cs)
  - UI Panel: [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/RadioSaveCodecTests.cs`](../../Ashfall.Core.Tests/RadioSaveCodecTests.cs)

### 48. `apprenticeship` — Mentorship pairings and skill growth (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupApprenticeship()` | **Cadence:** `Daily Mentorship XP Transfer`
- **UI Routes:** `apprenticeship`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ApprenticeshipSystem.cs`](../../Assets/Ashfall.Core/ApprenticeshipSystem.cs)
  - Host Session: [`src/Host/ApprenticeshipHostSession.cs`](../../src/Host/ApprenticeshipHostSession.cs)
  - Save Store: [`src/Host/ApprenticeshipSaveStore.cs`](../../src/Host/ApprenticeshipSaveStore.cs)
  - UI Panel: [`src/UI/ApprenticeshipPanel.cs`](../../src/UI/ApprenticeshipPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`](../../Ashfall.Core.Tests/ApprenticeshipSystemTests.cs)

### 49. `autopsy` — Post-mortem forensic analysis (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupAutopsy()` | **Cadence:** `Daily Forensic Case Progress`
- **UI Routes:** `autopsy_report`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AutopsySystem.cs`](../../Assets/Ashfall.Core/AutopsySystem.cs)
  - Host Session: [`src/Host/AutopsyHostSession.cs`](../../src/Host/AutopsyHostSession.cs)
  - Save Store: [`src/Host/AutopsySaveStore.cs`](../../src/Host/AutopsySaveStore.cs)
  - UI Panel: [`src/UI/AutopsyReportPanel.cs`](../../src/UI/AutopsyReportPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AutopsySystemTests.cs`](../../Ashfall.Core.Tests/AutopsySystemTests.cs)

### 50. `caregiving` — Childcare, elderly care, and comfort (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupCaregiving()` | **Cadence:** `Daily Nursery/Eldercare Comfort`
- **UI Routes:** `caregiving`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/CaregivingSystem.cs`](../../Assets/Ashfall.Core/Survivors/CaregivingSystem.cs)
  - Host Session: [`src/Host/CaregivingHostSession.cs`](../../src/Host/CaregivingHostSession.cs)
  - Save Store: [`src/Host/CaregivingSaveStore.cs`](../../src/Host/CaregivingSaveStore.cs)
  - UI Panel: [`src/UI/CaregivingPanel.cs`](../../src/UI/CaregivingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CaregivingSystemTests.cs`](../../Ashfall.Core.Tests/CaregivingSystemTests.cs)

### 51. `chemical_dependency` — Substance dependencies and withdrawal (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMentalHealthCrisis()` | **Cadence:** `Daily Tolerance & Withdrawal`
- **UI Routes:** `chemical_dependency`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`](../../Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs)
  - Host Session: [`src/Host/ChemicalDependencyHostSession.cs`](../../src/Host/ChemicalDependencyHostSession.cs)
  - Host Session: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - Save Store: [`src/Host/ChemicalDependencySaveStore.cs`](../../src/Host/ChemicalDependencySaveStore.cs)
  - UI Panel: [`src/UI/ChemicalDependencyPanel.cs`](../../src/UI/ChemicalDependencyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BareSaveStoreSealTests.cs`](../../Ashfall.Core.Tests/BareSaveStoreSealTests.cs)

### 52. `contractor_roster` — Hired mercenaries and specialists (Survival & Biology)
- **Owner Domain:** `personnel`
- **Setup Method:** `Main.SetupContractorRoster()` | **Cadence:** `Daily Mercenary Wage Payroll`
- **UI Routes:** `contractor_roster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ContractorRosterSystem.cs`](../../Assets/Ashfall.Core/ContractorRosterSystem.cs)
  - Host Session: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - Save Store: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - UI Panel: [`src/UI/ContractorRosterPanel.cs`](../../src/UI/ContractorRosterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ContractorRosterSystemTests.cs`](../../Ashfall.Core.Tests/ContractorRosterSystemTests.cs)

### 53. `disease` — Epidemics, contagions, and pathogen spread (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupDisease()` | **Cadence:** `Daily Pathogen Transmission`
- **UI Routes:** `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`](../../Assets/Ashfall.Core/Disease/DiseaseSystem.cs)
  - Host Session: [`src/Disease/DiseaseHostSession.cs`](../../src/Disease/DiseaseHostSession.cs)
  - Save Store: [`src/Host/DiseaseSaveStore.cs`](../../src/Host/DiseaseSaveStore.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiseaseSystemTests.cs`](../../Ashfall.Core.Tests/DiseaseSystemTests.cs)

### 54. `medical` — Triage, illnesses, and treatments (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedical()` | **Cadence:** `Daily Recovery / Affliction`
- **UI Routes:** `medical`, `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/MedicalWardSystem.cs)
  - Core System: [`Assets/Ashfall.Core/SickListSystem.cs`](../../Assets/Ashfall.Core/SickListSystem.cs)
  - Host Session: [`src/Host/MedicalHostSession.cs`](../../src/Host/MedicalHostSession.cs)
  - Save Store: [`src/Host/MedicalSaveStore.cs`](../../src/Host/MedicalSaveStore.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DwellerMedicalCatalogTests.cs`](../../Ashfall.Core.Tests/DwellerMedicalCatalogTests.cs)

### 55. `medical_ward` — Hospital ward beds and inpatients (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedicalWard()` | **Cadence:** `Daily Bed Inpatient Triage`
- **UI Routes:** `medical_ward`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/MedicalWardSystem.cs)
  - Host Session: [`src/Host/MedicalWardHostSession.cs`](../../src/Host/MedicalWardHostSession.cs)
  - Save Store: [`src/Host/MedicalWardSaveStore.cs`](../../src/Host/MedicalWardSaveStore.cs)
  - UI Panel: [`src/UI/MedicalWardPanel.cs`](../../src/UI/MedicalWardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs)

### 56. `mental_health_crisis` — Psychological trauma and psych ward (Survival & Biology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupMentalHealthCrisis()` | **Cadence:** `Daily Psych Ward Calming Ticks`
- **UI Routes:** `mental_health_crisis`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`](../../Assets/Ashfall.Core/MentalHealthCrisisSystem.cs)
  - Host Session: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - Save Store: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - UI Panel: [`src/UI/MentalHealthCrisisPanel.cs`](../../src/UI/MentalHealthCrisisPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs`](../../Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs)

### 57. `survivor_relations` — Survivor affinities, feuds, and bonds (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupSurvivorRelations()` | **Cadence:** `Daily Affinity & Feud Drift`
- **UI Routes:** `survivor_relations`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SurvivorRelationsSystem.cs`](../../Assets/Ashfall.Core/SurvivorRelationsSystem.cs)
  - Host Session: [`src/Host/SurvivorRelationsHostSession.cs`](../../src/Host/SurvivorRelationsHostSession.cs)
  - Save Store: [`src/Host/SurvivorRelationsSaveStore.cs`](../../src/Host/SurvivorRelationsSaveStore.cs)
  - UI Panel: [`src/UI/SurvivorRelationsPanel.cs`](../../src/UI/SurvivorRelationsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 58. `survivors` — Living survivors, needs, and traits (Survival & Biology)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivors()` | **Cadence:** `Daily Needs Decay`
- **UI Routes:** `survivors`, `survivor_detail`, `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`](../../Assets/Ashfall.Core/Survivors/NeedsSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs)
  - Host Session: [`src/Host/SurvivorsHostSession.cs`](../../src/Host/SurvivorsHostSession.cs)
  - Save Store: [`src/Host/SurvivorsSaveStore.cs`](../../src/Host/SurvivorsSaveStore.cs)
  - UI Panel: [`src/UI/StatusPanel.cs`](../../src/UI/StatusPanel.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - UI Panel: [`src/UI/SurvivorsPanel.cs`](../../src/UI/SurvivorsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`](../../Ashfall.Core.Tests/NeedsRadiationSystemTests.cs)

### 59. `combat` — Combat encounters and tactical trauma (Tactical Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupCombat()` | **Cadence:** `On-Demand (Turn-Based)`
- **UI Routes:** `combat`, `combat_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`](../../Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`](../../Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs)
  - Host Session: [`src/Host/CombatHostSession.cs`](../../src/Host/CombatHostSession.cs)
  - Save Store: [`src/Host/CombatSaveStore.cs`](../../src/Host/CombatSaveStore.cs)
  - UI Panel: [`src/UI/CombatDetailPanel.cs`](../../src/UI/CombatDetailPanel.cs)
  - UI Panel: [`src/UI/CombatHistoryPanel.cs`](../../src/UI/CombatHistoryPanel.cs)
  - UI Panel: [`src/UI/CombatPanel.cs`](../../src/UI/CombatPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CombatBallisticsTests.cs`](../../Ashfall.Core.Tests/CombatBallisticsTests.cs)

### 60. `encounter_choice` — Encounter choice history & outcomes (World & Expeditions)
- **Owner Domain:** `encounters`
- **Setup Method:** `Main.SetupEncounterChoice()` | **Cadence:** `On-Demand (Door Event Resolution)`
- **UI Routes:** `door_encounter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Host Session: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Save Store: [`src/Host/EncounterChoiceSaveStore.cs`](../../src/Host/EncounterChoiceSaveStore.cs)
  - UI Panel: [`src/YearOfAsh/DoorEncounterModal.cs`](../../src/YearOfAsh/DoorEncounterModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs`](../../Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs)

### 61. `expedition` — Wasteland expedition runs & status (World & Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupExpeditions()` | **Cadence:** `Daily Sortie Travel`
- **UI Routes:** `expeditions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`](../../Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs)
  - Core System: [`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`](../../Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs)
  - Host Session: [`src/Host/ExpeditionHostSession.cs`](../../src/Host/ExpeditionHostSession.cs)
  - Save Store: [`src/Host/ExpeditionSaveStore.cs`](../../src/Host/ExpeditionSaveStore.cs)
  - UI Panel: [`src/UI/ExpeditionPanel.cs`](../../src/UI/ExpeditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpeditionCampSystemTests.cs`](../../Ashfall.Core.Tests/ExpeditionCampSystemTests.cs)

### 62. `wasteland_map` — Wasteland map markers and fog-of-war (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorld()` | **Cadence:** `On-Demand (Fog-of-War Discovery)`
- **UI Routes:** `map`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Host Session: [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
  - Save Store: [`src/Host/WastelandMapSaveStore.cs`](../../src/Host/WastelandMapSaveStore.cs)
  - UI Panel: [`src/UI/MapPanel.cs`](../../src/UI/MapPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`](../../Ashfall.Core.Tests/WastelandMapPersistenceTests.cs)

### 63. `waystation` — Wasteland outpost network & relay hubs (World & Expeditions)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaystation()` | **Cadence:** `Daily Outpost Relay Barter`
- **UI Routes:** `waystation_network`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaystationSystem.cs`](../../Assets/Ashfall.Core/WaystationSystem.cs)
  - Host Session: [`src/Host/WaystationHostSession.cs`](../../src/Host/WaystationHostSession.cs)
  - Save Store: [`src/Host/WaystationSaveStore.cs`](../../src/Host/WaystationSaveStore.cs)
  - UI Panel: [`src/UI/WaystationNetworkPanel.cs`](../../src/UI/WaystationNetworkPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaystationSystemTests.cs`](../../Ashfall.Core.Tests/WaystationSystemTests.cs)

### 64. `wildlife_trapping` — Snares, game catches, and foraging (World & Expeditions)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupWildlifeTrapping()` | **Cadence:** `Daily Snare Yield & Butchery`
- **UI Routes:** `wildlife_trapping`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WildlifeTrappingSystem.cs`](../../Assets/Ashfall.Core/WildlifeTrappingSystem.cs)
  - Host Session: [`src/Host/WildlifeTrappingHostSession.cs`](../../src/Host/WildlifeTrappingHostSession.cs)
  - Save Store: [`src/Host/WildlifeTrappingSaveStore.cs`](../../src/Host/WildlifeTrappingSaveStore.cs)
  - UI Panel: [`src/UI/WildlifeTrappingPanel.cs`](../../src/UI/WildlifeTrappingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs`](../../Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs)

### 65. `world` — World map nodes, sectors, and discovery (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorld()` | **Cadence:** `Daily Weather & Hazard`
- **UI Routes:** `map`, `weather`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Core System: [`Assets/Ashfall.Core/World/WeatherSystem.cs`](../../Assets/Ashfall.Core/World/WeatherSystem.cs)
  - Host Session: [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
  - Save Store: [`src/Host/WorldSaveStore.cs`](../../src/Host/WorldSaveStore.cs)
  - UI Panel: [`src/UI/MapPanel.cs`](../../src/UI/MapPanel.cs)
  - UI Panel: [`src/UI/WeatherPanel.cs`](../../src/UI/WeatherPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WorldSaveablesTests.cs`](../../Ashfall.Core.Tests/WorldSaveablesTests.cs)

---

### 66. `medical_pipeline` — Unified affliction, diagnosis, and treatment pipeline (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.EnsureMedicalPipeline()` | **Cadence:** `Daily Scheduled Procedure Resolution (medical_disease day owner)`
- **UI Routes:** `medical`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`](../../Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs)
  - Core Contracts: [`Assets/Ashfall.Core/Medical/AfflictionId.cs`](../../Assets/Ashfall.Core/Medical/AfflictionId.cs), [`Assets/Ashfall.Core/Medical/AfflictionContracts.cs`](../../Assets/Ashfall.Core/Medical/AfflictionContracts.cs), [`Assets/Ashfall.Core/Medical/DiagnosisKnowledgeStore.cs`](../../Assets/Ashfall.Core/Medical/DiagnosisKnowledgeStore.cs), [`Assets/Ashfall.Core/Medical/MedicalReservationLedger.cs`](../../Assets/Ashfall.Core/Medical/MedicalReservationLedger.cs), [`Assets/Ashfall.Core/Medical/MedicalProcedureSchedule.cs`](../../Assets/Ashfall.Core/Medical/MedicalProcedureSchedule.cs), [`Assets/Ashfall.Core/Medical/PatientRecord.cs`](../../Assets/Ashfall.Core/Medical/PatientRecord.cs)
  - Host Session: [`src/Host/MedicalHostSession.cs`](../../src/Host/MedicalHostSession.cs)
  - Save Store: [`src/Host/MedicalPipelineSaveStore.cs`](../../src/Host/MedicalPipelineSaveStore.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalPipelineTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalPipelineTests.cs)

### 67. `ecological_infestation` — Location & shelter ecological infestations (World & Expeditions)
- **Owner Domain:** `world` (Plan 28)
- **Setup Method:** `Main.SetupEcologicalInfestation()` | **Cadence:** `Daily trigger pass + consequence tick (world_evolution day owner)`
- **UI Routes:** briefing events + journal (dedicated panel = later phase)
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs`](../../Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs)
  - Data Authority: [`Assets/StreamingAssets/Data/ecological_infestations.json`](../../Assets/StreamingAssets/Data/ecological_infestations.json)
  - Host Session: [`src/Main.EcologicalInfestations.cs`](../../src/Main.EcologicalInfestations.cs)
  - Save Store: [`src/Host/EcologicalInfestationSaveStore.cs`](../../src/Host/EcologicalInfestationSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`](../../Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs)

## 4. Lifecycle Status & Reachability Proof Matrix

| Section Key | Implemented | Constructed | Ticked / Cadence | Persisted | Player-Routed | Tested | E2E Status |
|---|:---:|:---:|---|:---:|:---:|:---:|:---:|
| `airlock_security` | ✅ | ✅ | ✅ `Daily Decon Interlock` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `apprenticeship` | ✅ | ✅ | ✅ `Daily Mentorship XP Transfer` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `archive_desk` | ✅ | ✅ | ✅ `Daily Scribing & Folio Archival` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `autopsy` | ✅ | ✅ | ✅ `Daily Forensic Case Progress` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `campaign_day` | ✅ | ✅ | ✅ `Master Sim Clock / Dawn Advance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caravan` | ✅ | ✅ | ✅ `Daily Route Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caregiving` | ✅ | ✅ | ✅ `Daily Nursery/Eldercare Comfort` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chemical_dependency` | ✅ | ✅ | ✅ `Daily Tolerance & Withdrawal` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `combat` | ✅ | ✅ | ⚡ `On-Demand (Turn-Based)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `contractor_roster` | ✅ | ✅ | ✅ `Daily Mercenary Wage Payroll` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `crafting` | ✅ | ✅ | ✅ `Daily Workbench Queue` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `daily_briefing` | ✅ | ✅ | ✅ `Daily Dawn Briefing Aggregation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `decontamination` | ✅ | ✅ | ✅ `Daily Rad Scrub Shower Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `disease` | ✅ | ✅ | ✅ `Daily Pathogen Transmission` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dose_ledger` | ✅ | ✅ | ⚡ `On-Demand (Dose Log)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `duty_roster` | ✅ | ✅ | ✅ `Daily Shift Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `economy` | ✅ | ✅ | ✅ `Daily Market Rate Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `encounter_choice` | ✅ | ✅ | ⚡ `On-Demand (Door Event Resolution)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `equipment_condition` | ✅ | ✅ | ✅ `Daily Gear Wear & Maintenance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `excavation` | ✅ | ✅ | ✅ `Daily Rubble Shoring Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expansion_hub` | ✅ | ✅ | ✅ `Daily Hub Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expansion_quest` | ✅ | ✅ | ⚡ `On-Demand (Stage Milestone)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expedition` | ✅ | ✅ | ✅ `Daily Sortie Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `greenhouse` | ✅ | ✅ | ✅ `Daily Hydroponic Growth` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `holdfast` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `holdfast_trade` | ✅ | ✅ | ⚡ `On-Demand (Barter)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `host_event` | ✅ | ✅ | ⚡ `On-Demand (Moral Dilemma)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `inventory` | ✅ | ✅ | ⚡ `On-Demand (Item Use)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `journal` | ✅ | ✅ | ⚡ `On-Demand (Log/Event)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `kitchen_nutrition` | ✅ | ✅ | ✅ `Daily Rationing Meal Prep` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `library_study` | ✅ | ✅ | ✅ `Daily Codex Research Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `maritime` | ✅ | ✅ | ⚡ `On-Demand (Dive Sortie)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical` | ✅ | ✅ | ✅ `Daily Recovery / Affliction` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_pipeline` | ✅ | ✅ | ✅ `Daily Scheduled Procedure Resolution` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_ward` | ✅ | ✅ | ✅ `Daily Bed Inpatient Triage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `memorial` | ✅ | ✅ | ⚡ `On-Demand (Survivor Fallen Eulogy)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mental_health_crisis` | ✅ | ✅ | ✅ `Daily Psych Ward Calming Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `moral_choice` | ✅ | ✅ | ⚡ `On-Demand (Branch Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `muster` | ✅ | ✅ | ⚡ `On-Demand (Rally Stance)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narrative` | ✅ | ✅ | ⚡ `On-Demand (Dialog Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `onboarding` | ✅ | ❌ | ⚡ `On-Demand (Player Sigil Recording)` | ✅ | ✅ | ✅ | **FAIL (GAP)** |
| `phantom_memory` | ✅ | ✅ | ⚡ `On-Demand (Scavenge Echo)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `phase0` | ✅ | ✅ | ⚡ `On-Demand (Pre-War Flashback)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `power_grid` | ✅ | ✅ | ✅ `Daily Fuel Consumption & Wattage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `radio` | ✅ | ✅ | ⚡ `On-Demand (Frequency Scan)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `regional_treaty` | ✅ | ✅ | ✅ `Daily Non-Aggression Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_assignment` | ✅ | ✅ | ⚡ `On-Demand (Bunk Reassignment)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_schedule` | ✅ | ✅ | ✅ `Daily Curfew Rotation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_thermal` | ✅ | ✅ | ✅ `Daily HVAC Frost Dissipation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `silent_foundry` | ✅ | ✅ | ✅ `Daily Smelter Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `starting_level` | ✅ | ✅ | ⚡ `On-Demand (Opening Protocol)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `sump_flooding` | ✅ | ✅ | ✅ `Daily Drainage Pump Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_fate` | ✅ | ❌ | ✅ `Daily Survivor-Death Cascade` | ✅ | ✅ | ✅ | **FAIL (GAP)** |
| `survivor_relations` | ✅ | ✅ | ✅ `Daily Affinity & Feud Drift` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_social` | ✅ | ✅ | ✅ `Daily Shelter Social Dynamics` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivors` | ✅ | ✅ | ✅ `Daily Needs Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `thirdonary` | ✅ | ✅ | ⚡ `On-Demand (Arbitration)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `verdict` | ✅ | ✅ | ✅ `Daily Machine Log Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `vinyl_morale` | ✅ | ✅ | ✅ `Daily Turntable Morale Broadcast` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_map` | ✅ | ✅ | ⚡ `On-Demand (Fog-of-War Discovery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `water_treatment` | ✅ | ✅ | ✅ `Daily Filtration Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `waystation` | ✅ | ✅ | ✅ `Daily Outpost Relay Barter` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `weight_of_choices` | ✅ | ✅ | ⚡ `On-Demand (Branch Decisions)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wildlife_trapping` | ✅ | ✅ | ✅ `Daily Snare Yield & Butchery` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `world` | ✅ | ✅ | ✅ `Daily Weather & Hazard` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `year_of_ash` | ✅ | ✅ | ✅ `Daily Deep-Freeze Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |

---

## 5. Architectural Verification Invariants

1. **Invariant 1 (Core Engine Agnosticism):** Core systems contain zero references to `Godot`, `UnityEngine`, or engine globals.
2. **Invariant 3 (Save Store Integrity):** Every save store delegates to `SaveStoreHub` / `SaveEnvelopeHelper` or a Core codec and wraps state in a verified checksum envelope.
3. **Invariant 5 (Thin Host Nodes):** UI panels and host sessions handle only presentation, lifecycle, and wiring — never domain calculations.
4. **Invariant 6 (Data Authority):** `Assets/StreamingAssets/Data/` JSON files are the sole authority.
5. **Mechanical Reachability Gate:** Every system in this matrix is verified by headless test runs in `verify-fast.sh` and xUnit suites in `Ashfall.Core.Tests`.
6. **Zero Conceptual Placeholders:** If a layer is absent or procedural, it is documented with explicit status rather than filled with conceptual names.
