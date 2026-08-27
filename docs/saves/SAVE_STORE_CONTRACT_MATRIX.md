# ASHFALL — Save-Store Contract Matrix & Completeness Authority

**Last Verified:** 2026-08-27<br>
**Total Save Stores:** 62 classes<br>
**Total Static Persistence Methods:** 68 methods<br>
**Checksum-Protected Stores:** 62/62 (100.0%)<br>
**Slot-Root Isolated Stores:** 62/62 (100.0%)<br>
**Tested Stores:** 62/62 (100.0%)

> **GENERATED FILE — do not edit by hand.**
> Source of truth: All save store classes under `src/` and `Assets/Ashfall.Core/`.
> Generated via: `bash scripts/ci/generate-save-store-matrix.sh`
> CI Completeness Gate: `bash scripts/ci/generate-save-store-matrix.sh --check`

---

## 1. Architectural Save-Store Contract Invariants

1. **Invariant 3 (Save Envelope Integrity):** Every save store must wrap payload state in a `{ State, Checksum }` envelope stamped by `SaveChecksum`, delegate to a Core save codec (`*Codec.Encode / Decode`), or delegate to the generic `SaveStore<T>` service (`SaveStoreHub.Checksummed / .FromCodec`). Bare unchecksummed stores are strictly rejected.
2. **Slot-Root Isolation:** All save paths must resolve through `SaveSlotRoot.ResolveSlotFile(...)`, `SaveSlotRoot.ResolveSlotPath(...)`, or the `SaveStoreHub` factory (which routes through `SaveSlotRoot.ResolveBaseDirectory` per operation) so headless self-tests, slots, and profiles execute in isolated environments without mutating default user data.
3. **Declarative Section Alignment:** Every registered `SectionName` must correspond directly to an entry in `SaveSectionRegistry.cs` (`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`).

---

## 2. Save-Store Inventory & Contract Matrix

| # | Save Store Class | Source File | Section Key | Target JSON File | Methods | Checksum | Slot Root | Test Fixtures |
|---|---|---|---|---|---|:---:|:---:|---|
| 1 | `AirlockSecuritySaveStore` | [`src/Host/AirlockSecuritySaveStore.cs`](../../src/Host/AirlockSecuritySaveStore.cs) | `airlock_security` | `airlock_security_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 2 | `ApprenticeshipSaveStore` | [`src/Host/ApprenticeshipSaveStore.cs`](../../src/Host/ApprenticeshipSaveStore.cs) | `apprenticeship` | `apprenticeship_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 3 | `ArchiveDeskSaveStore` | [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs) | `archive_desk` | `archive_desk_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 4 | `AutopsySaveStore` | [`src/Host/AutopsySaveStore.cs`](../../src/Host/AutopsySaveStore.cs) | `autopsy` | `autopsy_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 5 | `CampaignDaySaveStore` | [`src/Host/CampaignDaySaveStore.cs`](../../src/Host/CampaignDaySaveStore.cs) | `campaign_day` | `campaign_day_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 6 | `CaravanSaveStore` | [`src/Host/CaravanSaveStore.cs`](../../src/Host/CaravanSaveStore.cs) | `caravan` | `caravan_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 7 | `CaregivingSaveStore` | [`src/Host/CaregivingSaveStore.cs`](../../src/Host/CaregivingSaveStore.cs) | `caregiving` | `caregiving_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 8 | `ChemicalDependencySaveStore` | [`src/Host/ChemicalDependencySaveStore.cs`](../../src/Host/ChemicalDependencySaveStore.cs) | `chemical_dependency` | `chemical_dependency_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+1 more)* |
| 9 | `CombatSaveStore` | [`src/Host/CombatSaveStore.cs`](../../src/Host/CombatSaveStore.cs) | `combat` | `combat_save.json` | `TryLoad()` | ✅ | ✅ | `CombatSystemTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+4 more)* |
| 10 | `ContractorRosterSaveStore` | [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs) | `contractor_roster` | `contractor_roster_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 11 | `CraftingSaveStore` | [`src/Host/CraftingSaveStore.cs`](../../src/Host/CraftingSaveStore.cs) | `crafting` | `crafting_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` *(+1 more)* |
| 12 | `DailyBriefingSaveStore` | [`src/Host/DailyBriefingSaveStore.cs`](../../src/Host/DailyBriefingSaveStore.cs) | `daily_briefing` | `daily_briefing_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 13 | `DecontaminationSaveStore` | [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs) | `decontamination` | `decontamination_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 14 | `DiseaseSaveStore` | [`src/Host/DiseaseSaveStore.cs`](../../src/Host/DiseaseSaveStore.cs) | `disease` | `disease_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `DiseaseSystemTests.cs` |
| 15 | `DoseLedgerSaveStore` | [`src/Host/DoseLedgerSaveStore.cs`](../../src/Host/DoseLedgerSaveStore.cs) | `dose_ledger` | `dose_ledger_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `VersionReportContractTests.cs` |
| 16 | `DutyRosterSaveStore` | [`src/Host/DutyRosterSaveStore.cs`](../../src/Host/DutyRosterSaveStore.cs) | `duty_roster` | `duty_roster_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 17 | `EconomySaveStore` | [`src/Host/EconomySaveStore.cs`](../../src/Host/EconomySaveStore.cs) | `economy` | `economy_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `EconomyProbeTests.cs` |
| 18 | `EncounterChoiceSaveStore` | [`src/Host/EncounterChoiceSaveStore.cs`](../../src/Host/EncounterChoiceSaveStore.cs) | `encounter_choice` | `encounter_choice_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 19 | `EquipmentConditionSaveStore` | [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs) | `equipment_condition` | `equipment_condition_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 20 | `ExcavationSaveStore` | [`src/Host/ExcavationSaveStore.cs`](../../src/Host/ExcavationSaveStore.cs) | `excavation` | `excavation_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 21 | `ExpansionHubSaveStore` | [`src/Host/ExpansionHubSaveStore.cs`](../../src/Host/ExpansionHubSaveStore.cs) | `expansion_hub` | `expansion_hub_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `VersionReportContractTests.cs` |
| 22 | `ExpansionQuestSaveStore` | [`src/Host/ExpansionQuestSaveStore.cs`](../../src/Host/ExpansionQuestSaveStore.cs) | `expansion_quest` | `expansion_quest_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `VersionReportContractTests.cs` |
| 23 | `ExpeditionSaveStore` | [`src/Host/ExpeditionSaveStore.cs`](../../src/Host/ExpeditionSaveStore.cs) | `expedition` | `expedition_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+2 more)* |
| 24 | `GreenhouseSaveStore` | [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs) | `greenhouse` | `greenhouse_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 25 | `HoldfastSaveStore` | [`src/Host/HoldfastSaveStore.cs`](../../src/Host/HoldfastSaveStore.cs) | `holdfast_s1` | `holdfast_s1_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PersistentFilenameRegistryGateTests.cs` |
| 26 | `HoldfastTradeSaveStore` | [`src/Host/HoldfastTradeSaveStore.cs`](../../src/Host/HoldfastTradeSaveStore.cs) | `holdfast_trade` | `holdfast_trade_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PersistentFilenameRegistryGateTests.cs` |
| 27 | `HostEventSaveStore` | [`src/Host/HostEventSaveStore.cs`](../../src/Host/HostEventSaveStore.cs) | `host_event` | `host_event_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 28 | `InventorySaveStore` | [`src/Host/InventorySaveStore.cs`](../../src/Host/InventorySaveStore.cs) | `inventory` | `inventory_save.json` | `TryLoad()` | ✅ | ✅ | `CampaignEnvelopeBuilderTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+3 more)* |
| 29 | `JournalSaveStore` | [`src/Journal/JournalSaveStore.cs`](../../src/Journal/JournalSaveStore.cs) | `journal` | `journal_save.json` | `Load()`, `Save()` | ✅ | ✅ | `CampaignEnvelopeBuilderTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+3 more)* |
| 30 | `KitchenNutritionSaveStore` | [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs) | `kitchen_nutrition` | `kitchen_nutrition_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 31 | `LibraryStudySaveStore` | [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs) | `library_study` | `library_study_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 32 | `MaritimeSaveStore` | [`src/Host/MaritimeSaveStore.cs`](../../src/Host/MaritimeSaveStore.cs) | `maritime` | `maritime_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 33 | `MedicalSaveStore` | [`src/Host/MedicalSaveStore.cs`](../../src/Host/MedicalSaveStore.cs) | `medical` | `medical_save.json` | `TryLoad()` | ✅ | ✅ | `AudioConditionSystemTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+7 more)* |
| 34 | `MedicalWardSaveStore` | [`src/Host/MedicalWardSaveStore.cs`](../../src/Host/MedicalWardSaveStore.cs) | `medical_ward` | `medical_ward_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 35 | `MemorialSaveStore` | [`src/Host/MemorialSaveStore.cs`](../../src/Host/MemorialSaveStore.cs) | `memorial` | `memorial_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 36 | `MentalHealthCrisisSaveStore` | [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs) | `mental_health_crisis` | `mental_health_crisis_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 37 | `MoralChoiceSaveStore` | [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs) | `moral_choice` | `moral_choice_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 38 | `MusterSaveStore` | [`src/Host/MusterSaveStore.cs`](../../src/Host/MusterSaveStore.cs) | `muster` | `muster_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 39 | `NarrativeSaveStore` | [`src/Host/NarrativeSaveStore.cs`](../../src/Host/NarrativeSaveStore.cs) | `narrative` | `narrative_save.json` | `TryLoad()` | ✅ | ✅ | `AbyssalAnomaliesCatalogTests.cs`, `ApicultureBeeCatalogTests.cs` *(+69 more)* |
| 40 | `PhantomMemorySaveStore` | [`src/Host/PhantomMemorySaveStore.cs`](../../src/Host/PhantomMemorySaveStore.cs) | `phantom_memory` | `phantom_memory_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 41 | `Phase0SaveStore` | [`src/Host/Phase0SaveStore.cs`](../../src/Host/Phase0SaveStore.cs) | `phase0` | `phase0_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 42 | `PowerGridSaveStore` | [`src/Host/PowerGridSaveStore.cs`](../../src/Host/PowerGridSaveStore.cs) | `power_grid` | `power_grid_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 43 | `RadioSaveStore` | [`src/Host/RadioSaveStore.cs`](../../src/Host/RadioSaveStore.cs) | `radio` | `radio_save.json` | `TryLoad()` | ✅ | ✅ | `AudioConditionSystemTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+1 more)* |
| 44 | `RegionalTreatySaveStore` | [`src/Host/RegionalTreatySaveStore.cs`](../../src/Host/RegionalTreatySaveStore.cs) | `regional_treaty` | `regional_treaty_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 45 | `ShelterAssignmentSaveStore` | [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs) | `shelter_assignment` | `shelter_assignment_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 46 | `ShelterScheduleSaveStore` | [`src/Host/ShelterScheduleSaveStore.cs`](../../src/Host/ShelterScheduleSaveStore.cs) | `shelter_schedule` | `shelter_schedule_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 47 | `ShelterThermalSaveStore` | [`src/Host/ShelterThermalSaveStore.cs`](../../src/Host/ShelterThermalSaveStore.cs) | `shelter_thermal` | `shelter_thermal_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 48 | `SilentFoundrySaveStore` | [`src/Host/SilentFoundrySaveStore.cs`](../../src/Host/SilentFoundrySaveStore.cs) | `silent_foundry` | `silent_foundry_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 49 | `StartingLevelSaveStore` | [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs) | `starting_level` | `starting_level_save.json` | `SaveExists()`, `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 50 | `SumpFloodingSaveStore` | [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs) | `sump_flooding` | `sump_flooding_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `ExpandedShelterSavePersistenceTests.cs` *(+1 more)* |
| 51 | `SurvivorRelationsSaveStore` | [`src/Host/SurvivorRelationsSaveStore.cs`](../../src/Host/SurvivorRelationsSaveStore.cs) | `survivor_relations` | `survivor_relations_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 52 | `SurvivorsSaveStore` | [`src/Host/SurvivorsSaveStore.cs`](../../src/Host/SurvivorsSaveStore.cs) | `survivors` | `survivors_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` *(+4 more)* |
| 53 | `ThirdonarySaveStore` | [`src/Host/ThirdonarySaveStore.cs`](../../src/Host/ThirdonarySaveStore.cs) | `thirdonary` | `thirdonary_quest_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | `CampaignEnvelopeBuilderTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 54 | `VerdictSaveStore` | [`src/Host/VerdictSaveStore.cs`](../../src/Host/VerdictSaveStore.cs) | `verdict` | `verdict_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 55 | `VinylMoraleSaveStore` | [`src/Host/VinylMoraleSaveStore.cs`](../../src/Host/VinylMoraleSaveStore.cs) | `vinyl_morale` | `vinyl_morale_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 56 | `WastelandMapSaveStore` | [`src/Host/WastelandMapSaveStore.cs`](../../src/Host/WastelandMapSaveStore.cs) | `wasteland_map` | `wasteland_map_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `SaveSectionRegistryTests.cs` |
| 57 | `WaterTreatmentSaveStore` | [`src/Host/WaterTreatmentSaveStore.cs`](../../src/Host/WaterTreatmentSaveStore.cs) | `water_treatment` | `water_treatment_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 58 | `WaystationSaveStore` | [`src/Host/WaystationSaveStore.cs`](../../src/Host/WaystationSaveStore.cs) | `waystation` | `waystation_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `SaveChecksumTests.cs` |
| 59 | `WeatherSaveStore` | [`src/Host/WeatherSaveStore.cs`](../../src/Host/WeatherSaveStore.cs) | `weather` | `weather_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+2 more)* |
| 60 | `WildlifeTrappingSaveStore` | [`src/Host/WildlifeTrappingSaveStore.cs`](../../src/Host/WildlifeTrappingSaveStore.cs) | `wildlife_trapping` | `wildlife_trapping_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 61 | `WorldSaveStore` | [`src/Host/WorldSaveStore.cs`](../../src/Host/WorldSaveStore.cs) | `world` | `world_save.json` | `TryLoad()`, `TryLoadEnvelope()` | ✅ | ✅ | `CampaignEnvelopeBuilderTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+2 more)* |
| 62 | `YearOfAshSaveStore` | [`src/YearOfAsh/YearOfAshSaveStore.cs`](../../src/YearOfAsh/YearOfAshSaveStore.cs) | `year_of_ash` | `year_of_ash_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `VersionReportContractTests.cs` |
