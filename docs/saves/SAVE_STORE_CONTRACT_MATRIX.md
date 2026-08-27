# ASHFALL — Save-Store Contract Matrix & Completeness Authority

**Last Verified:** 2026-08-27<br>
**Total Save Stores:** 62 classes<br>
**Total Static Persistence Methods:** 68 methods<br>
**Checksum-Protected Stores:** 62/62 (100.0%)<br>
**Slot-Root Isolated Stores:** 62/62 (100.0%)<br>
**Tested Stores:** 22/62 (35.5%)

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
| 1 | `AirlockSecuritySaveStore` | [`src/Host/AirlockSecuritySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/AirlockSecuritySaveStore.cs) | `airlock_security` | `airlock_security_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 2 | `ApprenticeshipSaveStore` | [`src/Host/ApprenticeshipSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ApprenticeshipSaveStore.cs) | `apprenticeship` | `apprenticeship_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 3 | `ArchiveDeskSaveStore` | [`src/Host/ArchiveDeskHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ArchiveDeskHostSession.cs) | `archive_desk` | `archive_desk_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 4 | `AutopsySaveStore` | [`src/Host/AutopsySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/AutopsySaveStore.cs) | `autopsy` | `autopsy_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 5 | `CampaignDaySaveStore` | [`src/Host/CampaignDaySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/CampaignDaySaveStore.cs) | `campaign_day` | `campaign_day_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 6 | `CaravanSaveStore` | [`src/Host/CaravanSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/CaravanSaveStore.cs) | `caravan` | `caravan_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 7 | `CaregivingSaveStore` | [`src/Host/CaregivingSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/CaregivingSaveStore.cs) | `caregiving` | `caregiving_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 8 | `ChemicalDependencySaveStore` | [`src/Host/ChemicalDependencySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ChemicalDependencySaveStore.cs) | `chemical_dependency` | `chemical_dependency_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs` |
| 9 | `CombatSaveStore` | [`src/Host/CombatSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/CombatSaveStore.cs) | `combat` | `combat_save.json` | `TryLoad()` | ✅ | ✅ | `CombatSystemTests.cs`, `EncounterChoiceResolverTests.cs` *(+3 more)* |
| 10 | `ContractorRosterSaveStore` | [`src/Host/ContractorRosterHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ContractorRosterHostSession.cs) | `contractor_roster` | `contractor_roster_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 11 | `CraftingSaveStore` | [`src/Host/CraftingSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/CraftingSaveStore.cs) | `crafting` | `crafting_save.json` | `TryLoad()` | ✅ | ✅ | `SkillProgressionSystemTests.cs` |
| 12 | `DailyBriefingSaveStore` | [`src/Host/DailyBriefingSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/DailyBriefingSaveStore.cs) | `daily_briefing` | `daily_briefing_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 13 | `DecontaminationSaveStore` | [`src/Host/DecontaminationHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/DecontaminationHostSession.cs) | `decontamination` | `decontamination_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 14 | `DiseaseSaveStore` | [`src/Host/DiseaseSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/DiseaseSaveStore.cs) | `disease` | `disease_save.json` | `TryLoad()` | ✅ | ✅ | `DiseaseSystemTests.cs` |
| 15 | `DoseLedgerSaveStore` | [`src/Host/DoseLedgerSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/DoseLedgerSaveStore.cs) | `dose_ledger` | `dose_ledger_save.json` | `TryLoad()` | ✅ | ✅ | `VersionReportContractTests.cs` |
| 16 | `DutyRosterSaveStore` | [`src/Host/DutyRosterSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/DutyRosterSaveStore.cs) | `duty_roster` | `duty_roster_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 17 | `EconomySaveStore` | [`src/Host/EconomySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/EconomySaveStore.cs) | `economy` | `economy_save.json` | `TryLoad()` | ✅ | ✅ | `EconomyProbeTests.cs` |
| 18 | `EncounterChoiceSaveStore` | [`src/Host/EncounterChoiceSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/EncounterChoiceSaveStore.cs) | `encounter_choice` | `encounter_choice_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 19 | `EquipmentConditionSaveStore` | [`src/Host/EquipmentConditionHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/EquipmentConditionHostSession.cs) | `equipment_condition` | `equipment_condition_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 20 | `ExcavationSaveStore` | [`src/Host/ExcavationSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ExcavationSaveStore.cs) | `excavation` | `excavation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 21 | `ExpansionHubSaveStore` | [`src/Host/ExpansionHubSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ExpansionHubSaveStore.cs) | `expansion_hub` | `expansion_hub_save.json` | `TryLoad()` | ✅ | ✅ | `VersionReportContractTests.cs` |
| 22 | `ExpansionQuestSaveStore` | [`src/Host/ExpansionQuestSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ExpansionQuestSaveStore.cs) | `expansion_quest` | `expansion_quest_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | `VersionReportContractTests.cs` |
| 23 | `ExpeditionSaveStore` | [`src/Host/ExpeditionSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ExpeditionSaveStore.cs) | `expedition` | `expedition_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `CrossingQuestSystemTests.cs` |
| 24 | `GreenhouseSaveStore` | [`src/Host/GreenhouseHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/GreenhouseHostSession.cs) | `greenhouse` | `greenhouse_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 25 | `HoldfastSaveStore` | [`src/Host/HoldfastSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/HoldfastSaveStore.cs) | `holdfast_s1` | `holdfast_s1_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 26 | `HoldfastTradeSaveStore` | [`src/Host/HoldfastTradeSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/HoldfastTradeSaveStore.cs) | `holdfast_trade` | `holdfast_trade_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 27 | `HostEventSaveStore` | [`src/Host/HostEventSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/HostEventSaveStore.cs) | `host_event` | `host_event_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs` |
| 28 | `InventorySaveStore` | [`src/Host/InventorySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/InventorySaveStore.cs) | `inventory` | `inventory_save.json` | `TryLoad()` | ✅ | ✅ | `SaveAggregateContractTests.cs`, `SaveLoadFailurePathTests.cs` |
| 29 | `JournalSaveStore` | [`src/Journal/JournalSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Journal/JournalSaveStore.cs) | `journal` | `journal_save.json` | `Load()`, `Save()` | ✅ | ✅ | `VersionReportContractTests.cs` |
| 30 | `KitchenNutritionSaveStore` | [`src/Host/KitchenNutritionHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/KitchenNutritionHostSession.cs) | `kitchen_nutrition` | `kitchen_nutrition_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 31 | `LibraryStudySaveStore` | [`src/Host/LibraryStudyHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/LibraryStudyHostSession.cs) | `library_study` | `library_study_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 32 | `MaritimeSaveStore` | [`src/Host/MaritimeSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/MaritimeSaveStore.cs) | `maritime` | `maritime_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 33 | `MedicalSaveStore` | [`src/Host/MedicalSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/MedicalSaveStore.cs) | `medical` | `medical_save.json` | `TryLoad()` | ✅ | ✅ | `AudioConditionSystemTests.cs`, `DescriptiveTextsTests.cs` *(+5 more)* |
| 34 | `MedicalWardSaveStore` | [`src/Host/MedicalWardSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/MedicalWardSaveStore.cs) | `medical_ward` | `medical_ward_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 35 | `MemorialSaveStore` | [`src/Host/MemorialSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/MemorialSaveStore.cs) | `memorial` | `memorial_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 36 | `MentalHealthCrisisSaveStore` | [`src/Host/MentalHealthCrisisHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/MentalHealthCrisisHostSession.cs) | `mental_health_crisis` | `mental_health_crisis_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 37 | `MoralChoiceSaveStore` | [`src/Host/MoralChoiceSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/MoralChoiceSaveStore.cs) | `host_event` | `moral_choice_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | — |
| 38 | `MusterSaveStore` | [`src/Host/MusterSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/MusterSaveStore.cs) | `muster` | `muster_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 39 | `NarrativeSaveStore` | [`src/Host/NarrativeSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/NarrativeSaveStore.cs) | `narrative` | `narrative_save.json` | `TryLoad()` | ✅ | ✅ | `AbyssalAnomaliesCatalogTests.cs`, `ApicultureBeeCatalogTests.cs` *(+68 more)* |
| 40 | `PhantomMemorySaveStore` | [`src/Host/PhantomMemorySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/PhantomMemorySaveStore.cs) | `phantom_memory` | `phantom_memory_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 41 | `Phase0SaveStore` | [`src/Host/Phase0SaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/Phase0SaveStore.cs) | `phase0` | `phase0_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 42 | `PowerGridSaveStore` | [`src/Host/PowerGridSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/PowerGridSaveStore.cs) | `power_grid` | `power_grid_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 43 | `RadioSaveStore` | [`src/Host/RadioSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/RadioSaveStore.cs) | `radio` | `radio_save.json` | `TryLoad()` | ✅ | ✅ | `AudioConditionSystemTests.cs` |
| 44 | `RegionalTreatySaveStore` | [`src/Host/RegionalTreatySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/RegionalTreatySaveStore.cs) | `regional_treaty` | `regional_treaty_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 45 | `ShelterAssignmentSaveStore` | [`src/Host/ShelterAssignmentHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ShelterAssignmentHostSession.cs) | `shelter_assignment` | `shelter_assignment_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 46 | `ShelterScheduleSaveStore` | [`src/Host/ShelterScheduleSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ShelterScheduleSaveStore.cs) | `shelter_schedule` | `shelter_schedule_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 47 | `ShelterThermalSaveStore` | [`src/Host/ShelterThermalSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ShelterThermalSaveStore.cs) | `shelter_thermal` | `shelter_thermal_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 48 | `SilentFoundrySaveStore` | [`src/Host/SilentFoundrySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/SilentFoundrySaveStore.cs) | `silent_foundry` | `silent_foundry_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 49 | `StartingLevelSaveStore` | [`src/Host/StartingLevelHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/StartingLevelHostSession.cs) | `starting_level` | `starting_level_save.json` | `SaveExists()`, `TryLoad()` | ✅ | ✅ | — |
| 50 | `SumpFloodingSaveStore` | [`src/Host/SumpFloodingHostSession.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/SumpFloodingHostSession.cs) | `sump_flooding` | `sump_flooding_save.json` | `TryLoad()` | ✅ | ✅ | `ExpandedShelterSavePersistenceTests.cs` |
| 51 | `SurvivorRelationsSaveStore` | [`src/Host/SurvivorRelationsSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/SurvivorRelationsSaveStore.cs) | `survivor_relations` | `survivor_relations_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 52 | `SurvivorsSaveStore` | [`src/Host/SurvivorsSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/SurvivorsSaveStore.cs) | `survivors` | `survivors_save.json` | `TryLoad()` | ✅ | ✅ | `ProductionArtManifestTests.cs`, `SaveAggregateContractTests.cs` *(+2 more)* |
| 53 | `ThirdonarySaveStore` | [`src/Host/ThirdonarySaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/ThirdonarySaveStore.cs) | `thirdonary` | `thirdonary_quest_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | — |
| 54 | `VerdictSaveStore` | [`src/Host/VerdictSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/VerdictSaveStore.cs) | `verdict` | `verdict_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 55 | `VinylMoraleSaveStore` | [`src/Host/VinylMoraleSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/VinylMoraleSaveStore.cs) | `vinyl_morale` | `vinyl_morale_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 56 | `WastelandMapSaveStore` | [`src/Host/WastelandMapSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/WastelandMapSaveStore.cs) | `wasteland_map` | `wasteland_map_save.json` | `TryLoad()` | ✅ | ✅ | `SaveSectionRegistryTests.cs` |
| 57 | `WaterTreatmentSaveStore` | [`src/Host/WaterTreatmentSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/WaterTreatmentSaveStore.cs) | `water_treatment` | `water_treatment_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 58 | `WaystationSaveStore` | [`src/Host/WaystationSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/WaystationSaveStore.cs) | `waystation` | `waystation_save.json` | `TryLoad()` | ✅ | ✅ | `SaveChecksumTests.cs` |
| 59 | `WeatherSaveStore` | [`src/Host/WeatherSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/WeatherSaveStore.cs) | `weather` | `weather_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs` |
| 60 | `WildlifeTrappingSaveStore` | [`src/Host/WildlifeTrappingSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/WildlifeTrappingSaveStore.cs) | `wildlife_trapping` | `wildlife_trapping_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 61 | `WorldSaveStore` | [`src/Host/WorldSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/Host/WorldSaveStore.cs) | `world` | `world_save.json` | `TryLoad()`, `TryLoadEnvelope()` | ✅ | ✅ | `SaveAggregateContractTests.cs` |
| 62 | `YearOfAshSaveStore` | [`src/YearOfAsh/YearOfAshSaveStore.cs`](file:////home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/src/YearOfAsh/YearOfAshSaveStore.cs) | `year_of_ash` | `year_of_ash_save.json` | `TryLoad()` | ✅ | ✅ | `VersionReportContractTests.cs` |
