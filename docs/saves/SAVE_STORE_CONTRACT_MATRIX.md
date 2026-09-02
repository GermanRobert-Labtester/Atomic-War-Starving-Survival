# ASHFALL — Save-Store Contract Matrix & Completeness Authority

**Last Verified:** 2026-09-02<br>
**Total Save Stores:** 96 classes<br>
**Total Static Persistence Methods:** 102 methods<br>
**Checksum-Protected Stores:** 96/96 (100.0%)<br>
**Slot-Root Isolated Stores:** 96/96 (100.0%)<br>
**Tested Stores:** 67/96 (69.8%)

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
| 1 | `AirlockSecuritySaveStore` | [`src/Host/AirlockSecuritySaveStore.cs`](../../src/Host/AirlockSecuritySaveStore.cs) | `airlock_security` | `airlock_security_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `EndToEndPlayerJourneyTests.cs` *(+1 more)* |
| 2 | `AmputationSaveStore` | [`src/Host/AmputationSaveStore.cs`](../../src/Host/AmputationSaveStore.cs) | `amputation` | `amputation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 3 | `ApprenticeshipSaveStore` | [`src/Host/ApprenticeshipSaveStore.cs`](../../src/Host/ApprenticeshipSaveStore.cs) | `apprenticeship` | `apprenticeship_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `EndToEndPlayerJourneyTests.cs` *(+1 more)* |
| 4 | `ArchaeologySaveStore` | [`src/Host/ArchaeologySaveStore.cs`](../../src/Host/ArchaeologySaveStore.cs) | `archaeology` | `archaeology_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 5 | `ArchiveDeskSaveStore` | [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs) | `archive_desk` | `archive_desk_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 6 | `AutopsySaveStore` | [`src/Host/AutopsySaveStore.cs`](../../src/Host/AutopsySaveStore.cs) | `autopsy` | `autopsy_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 7 | `AviationSaveStore` | [`src/Host/AviationSaveStore.cs`](../../src/Host/AviationSaveStore.cs) | `aviation` | `aviation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 8 | `CampaignDaySaveStore` | [`src/Host/CampaignDaySaveStore.cs`](../../src/Host/CampaignDaySaveStore.cs) | `campaign_day` | `campaign_day_save.json` | `TryLoad()` | ✅ | ✅ | `CampaignCalendarTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+1 more)* |
| 9 | `CaravanSaveStore` | [`src/Host/CaravanSaveStore.cs`](../../src/Host/CaravanSaveStore.cs) | `caravan` | `caravan_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 10 | `CaregivingSaveStore` | [`src/Host/CaregivingSaveStore.cs`](../../src/Host/CaregivingSaveStore.cs) | `caregiving` | `caregiving_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 11 | `CeremonySaveStore` | [`src/Host/CeremonySaveStore.cs`](../../src/Host/CeremonySaveStore.cs) | `ceremony` | `ceremony_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 12 | `ChemWarfareSaveStore` | [`src/Host/ChemWarfareSaveStore.cs`](../../src/Host/ChemWarfareSaveStore.cs) | `chem_warfare` | `chem_warfare_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 13 | `ChemicalDependencySaveStore` | [`src/Host/ChemicalDependencySaveStore.cs`](../../src/Host/ChemicalDependencySaveStore.cs) | `chemical_dependency` | `chemical_dependency_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+3 more)* |
| 14 | `CombatSaveStore` | [`src/Host/CombatSaveStore.cs`](../../src/Host/CombatSaveStore.cs) | `combat` | `combat_save.json` | `TryLoad()` | ✅ | ✅ | `CampaignConsequenceLedgerTests.cs`, `CombatSystemTests.cs` *(+11 more)* |
| 15 | `CommsArraySaveStore` | [`src/Host/CommsArraySaveStore.cs`](../../src/Host/CommsArraySaveStore.cs) | `comms_array` | `comms_array_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 16 | `ContractorRosterSaveStore` | [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs) | `contractor_roster` | `contractor_roster_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 17 | `CraftingSaveStore` | [`src/Host/CraftingSaveStore.cs`](../../src/Host/CraftingSaveStore.cs) | `crafting` | `crafting_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `EndToEndPlayerJourneyTests.cs` *(+6 more)* |
| 18 | `DailyBriefingSaveStore` | [`src/Host/DailyBriefingSaveStore.cs`](../../src/Host/DailyBriefingSaveStore.cs) | `daily_briefing` | `daily_briefing_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 19 | `DecontaminationSaveStore` | [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs) | `decontamination` | `decontamination_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 20 | `DesperationSaveStore` | [`src/Host/DesperationSaveStore.cs`](../../src/Host/DesperationSaveStore.cs) | `desperation` | `desperation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 21 | `DiseaseSaveStore` | [`src/Host/DiseaseSaveStore.cs`](../../src/Host/DiseaseSaveStore.cs) | `disease` | `disease_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `DiseaseSystemTests.cs` |
| 22 | `DoseLedgerSaveStore` | [`src/Host/DoseLedgerSaveStore.cs`](../../src/Host/DoseLedgerSaveStore.cs) | `dose_ledger` | `dose_ledger_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `VersionReportContractTests.cs` |
| 23 | `DutyRosterSaveStore` | [`src/Host/DutyRosterSaveStore.cs`](../../src/Host/DutyRosterSaveStore.cs) | `duty_roster` | `duty_roster_save.json` | `TryLoad()` | ✅ | ✅ | `CampaignCalendarTests.cs`, `CampaignDayCoordinatorSourceGateTests.cs` *(+3 more)* |
| 24 | `EcologicalInfestationSaveStore` | [`src/Host/EcologicalInfestationSaveStore.cs`](../../src/Host/EcologicalInfestationSaveStore.cs) | `ecological_infestation` | `ecological_infestation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 25 | `EconomySaveStore` | [`src/Host/EconomySaveStore.cs`](../../src/Host/EconomySaveStore.cs) | `economy` | `economy_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `EconomyProbeTests.cs` |
| 26 | `EncounterChoiceSaveStore` | [`src/Host/EncounterChoiceSaveStore.cs`](../../src/Host/EncounterChoiceSaveStore.cs) | `encounter_choice` | `encounter_choice_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 27 | `EquipmentConditionSaveStore` | [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs) | `equipment_condition` | `equipment_condition_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 28 | `ExcavationHazardSaveStore` | [`src/Host/ExcavationHazardSaveStore.cs`](../../src/Host/ExcavationHazardSaveStore.cs) | `excavation_hazards` | `excavation_hazards_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 29 | `ExcavationSaveStore` | [`src/Host/ExcavationSaveStore.cs`](../../src/Host/ExcavationSaveStore.cs) | `excavation` | `excavation_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 30 | `ExpansionHubSaveStore` | [`src/Host/ExpansionHubSaveStore.cs`](../../src/Host/ExpansionHubSaveStore.cs) | `expansion_hub` | `expansion_hub_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `VersionReportContractTests.cs` |
| 31 | `ExpansionQuestSaveStore` | [`src/Host/ExpansionQuestSaveStore.cs`](../../src/Host/ExpansionQuestSaveStore.cs) | `expansion_quest` | `expansion_quest_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `VersionReportContractTests.cs` |
| 32 | `ExpeditionSaveStore` | [`src/Host/ExpeditionSaveStore.cs`](../../src/Host/ExpeditionSaveStore.cs) | `expedition` | `expedition_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+3 more)* |
| 33 | `FalloutSaveStore` | [`src/Host/FalloutSaveStore.cs`](../../src/Host/FalloutSaveStore.cs) | `fallout` | `fallout_save.json` | `TryLoad()` | ✅ | ✅ | `Plan27BodyMindTests.cs` |
| 34 | `FieldGuideSaveStore` | [`src/Host/FieldGuideSaveStore.cs`](../../src/Host/FieldGuideSaveStore.cs) | `field_guide` | `field_guide_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 35 | `ForcedLaborSaveStore` | [`src/Host/ForcedLaborSaveStore.cs`](../../src/Host/ForcedLaborSaveStore.cs) | `forced_labor` | `forced_labor_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 36 | `FungiSaveStore` | [`src/Host/FungiSaveStore.cs`](../../src/Host/FungiSaveStore.cs) | `fungi_cultivation` | `fungi_cultivation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 37 | `GenerationalSaveStore` | [`src/Host/GenerationalSaveStore.cs`](../../src/Host/GenerationalSaveStore.cs) | `child_development` | `child_development_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 38 | `GreenhouseSaveStore` | [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs) | `greenhouse` | `greenhouse_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `ScavengingTableCatalogTests.cs` *(+1 more)* |
| 39 | `HoldfastSaveStore` | [`src/Host/HoldfastSaveStore.cs`](../../src/Host/HoldfastSaveStore.cs) | `holdfast_s1` | `holdfast_s1_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PersistentFilenameRegistryGateTests.cs` |
| 40 | `HoldfastTradeSaveStore` | [`src/Host/HoldfastTradeSaveStore.cs`](../../src/Host/HoldfastTradeSaveStore.cs) | `holdfast_trade` | `holdfast_trade_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PersistentFilenameRegistryGateTests.cs` |
| 41 | `HostEventSaveStore` | [`src/Host/HostEventSaveStore.cs`](../../src/Host/HostEventSaveStore.cs) | `host_event` | `host_event_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 42 | `InventorySaveStore` | [`src/Host/InventorySaveStore.cs`](../../src/Host/InventorySaveStore.cs) | `inventory` | `inventory_save.json` | `TryLoad()` | ✅ | ✅ | `ActiveSaveSlotPersistenceTests.cs`, `CampaignEnvelopeBuilderTests.cs` *(+7 more)* |
| 43 | `JournalSaveStore` | [`src/Journal/JournalSaveStore.cs`](../../src/Journal/JournalSaveStore.cs) | `journal` | `journal_save.json` | `Load()`, `Save()` | ✅ | ✅ | `CampaignEnvelopeBuilderTests.cs`, `CampaignEnvelopeFuzzTests.cs` *(+7 more)* |
| 44 | `JusticeSaveStore` | [`src/Host/JusticeSaveStore.cs`](../../src/Host/JusticeSaveStore.cs) | `wasteland_justice` | `wasteland_justice_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 45 | `KitchenNutritionSaveStore` | [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs) | `kitchen_nutrition` | `kitchen_nutrition_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 46 | `LibraryStudySaveStore` | [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs) | `library_study` | `library_study_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 47 | `MaritimeSaveStore` | [`src/Host/MaritimeSaveStore.cs`](../../src/Host/MaritimeSaveStore.cs) | `maritime` | `maritime_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 48 | `MedicalPipelineSaveStore` | [`src/Host/MedicalPipelineSaveStore.cs`](../../src/Host/MedicalPipelineSaveStore.cs) | `medical_pipeline` | `medical_pipeline_save.json` | `TryLoad()` | ✅ | ✅ | `MedicalPipelineArchitectureGateTests.cs` |
| 49 | `MedicalSaveStore` | [`src/Host/MedicalSaveStore.cs`](../../src/Host/MedicalSaveStore.cs) | `medical` | `medical_save.json` | `TryLoad()` | ✅ | ✅ | `AudioConditionSystemTests.cs`, `CampaignConsequenceLedgerTests.cs` *(+15 more)* |
| 50 | `MedicalWardSaveStore` | [`src/Host/MedicalWardSaveStore.cs`](../../src/Host/MedicalWardSaveStore.cs) | `medical_ward` | `medical_ward_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `EndToEndPlayerJourneyTests.cs` *(+1 more)* |
| 51 | `MemorialSaveStore` | [`src/Host/MemorialSaveStore.cs`](../../src/Host/MemorialSaveStore.cs) | `memorial` | `memorial_save.json` | `TryLoad()` | ✅ | ✅ | `CampaignCalendarTests.cs`, `CampaignDayCoordinatorSourceGateTests.cs` *(+5 more)* |
| 52 | `MentalHealthCrisisSaveStore` | [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs) | `mental_health_crisis` | `mental_health_crisis_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 53 | `MercenarySaveStore` | [`src/Host/MercenarySaveStore.cs`](../../src/Host/MercenarySaveStore.cs) | `mercenary_bounties` | `mercenary_bounties_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 54 | `MoralChoiceSaveStore` | [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs) | `moral_choice` | `moral_choice_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 55 | `MusterSaveStore` | [`src/Host/MusterSaveStore.cs`](../../src/Host/MusterSaveStore.cs) | `muster` | `muster_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 56 | `MutationSaveStore` | [`src/Host/MutationSaveStore.cs`](../../src/Host/MutationSaveStore.cs) | `mutation_tree` | `mutation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 57 | `NarcoticsSaveStore` | [`src/Host/NarcoticsSaveStore.cs`](../../src/Host/NarcoticsSaveStore.cs) | `narcotics` | `narcotics_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 58 | `NarrativeSaveStore` | [`src/Host/NarrativeSaveStore.cs`](../../src/Host/NarrativeSaveStore.cs) | `narrative` | `narrative_save.json` | `TryLoad()` | ✅ | ✅ | `AbyssalAnomaliesCatalogTests.cs`, `ApicultureBeeCatalogTests.cs` *(+71 more)* |
| 59 | `OnboardingSaveStore` | [`src/Host/OnboardingSaveStore.cs`](../../src/Host/OnboardingSaveStore.cs) | `onboarding` | `onboarding_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 60 | `PhantomMemorySaveStore` | [`src/Host/PhantomMemorySaveStore.cs`](../../src/Host/PhantomMemorySaveStore.cs) | `phantom_memory` | `phantom_memory_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 61 | `Phase0SaveStore` | [`src/Host/Phase0SaveStore.cs`](../../src/Host/Phase0SaveStore.cs) | `phase0` | `phase0_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 62 | `PoliticsSaveStore` | [`src/Host/PoliticsSaveStore.cs`](../../src/Host/PoliticsSaveStore.cs) | `settlement_politics` | `settlement_politics_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 63 | `PowerGridSaveStore` | [`src/Host/PowerGridSaveStore.cs`](../../src/Host/PowerGridSaveStore.cs) | `power_grid` | `power_grid_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 64 | `PrisonerSaveStore` | [`src/Host/PrisonerSaveStore.cs`](../../src/Host/PrisonerSaveStore.cs) | `prisoner_management` | `prisoner_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 65 | `RadioSaveStore` | [`src/Host/RadioSaveStore.cs`](../../src/Host/RadioSaveStore.cs) | `radio` | `radio_save.json` | `TryLoad()` | ✅ | ✅ | `AudioConditionSystemTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+2 more)* |
| 66 | `RadioStationSaveStore` | [`src/Host/RadioStationSaveStore.cs`](../../src/Host/RadioStationSaveStore.cs) | `radio_station` | `radio_station_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 67 | `RailwaySaveStore` | [`src/Host/RailwaySaveStore.cs`](../../src/Host/RailwaySaveStore.cs) | `railway` | `railway_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 68 | `RecreationSaveStore` | [`src/Host/RecreationSaveStore.cs`](../../src/Host/RecreationSaveStore.cs) | `recreation` | `recreation_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 69 | `RegionalTreatySaveStore` | [`src/Host/RegionalTreatySaveStore.cs`](../../src/Host/RegionalTreatySaveStore.cs) | `regional_treaty` | `regional_treaty_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 70 | `ResearchSaveStore` | [`src/Host/ResearchSaveStore.cs`](../../src/Host/ResearchSaveStore.cs) | `research` | `research_save.json` | `TryLoad()` | ✅ | ✅ | `PanelRouteGateTests.cs` |
| 71 | `RoboticsSaveStore` | [`src/Host/RoboticsSaveStore.cs`](../../src/Host/RoboticsSaveStore.cs) | `robotics` | `robotics_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 72 | `ShelterAssignmentSaveStore` | [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs) | `shelter_assignment` | `shelter_assignment_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 73 | `ShelterDecorSaveStore` | [`src/Host/ShelterDecorSaveStore.cs`](../../src/Host/ShelterDecorSaveStore.cs) | `shelter_decor` | `shelter_decor_save.json` | `TryLoad()` | ✅ | ✅ | `PanelRouteGateTests.cs`, `Plan12CDecorTests.cs` |
| 74 | `ShelterScheduleSaveStore` | [`src/Host/ShelterScheduleSaveStore.cs`](../../src/Host/ShelterScheduleSaveStore.cs) | `shelter_schedule` | `shelter_schedule_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 75 | `ShelterSocialSaveStore` | [`src/Host/ShelterSocialSaveStore.cs`](../../src/Host/ShelterSocialSaveStore.cs) | `shelter_social_dynamics` | `shelter_social_dynamics_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 76 | `ShelterThermalSaveStore` | [`src/Host/ShelterThermalSaveStore.cs`](../../src/Host/ShelterThermalSaveStore.cs) | `shelter_thermal` | `shelter_thermal_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 77 | `ShelterWorkshopSaveStore` | [`src/Host/ShelterWorkshopSaveStore.cs`](../../src/Host/ShelterWorkshopSaveStore.cs) | `shelter_workshop` | `shelter_workshop_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 78 | `SilentFoundrySaveStore` | [`src/Host/SilentFoundrySaveStore.cs`](../../src/Host/SilentFoundrySaveStore.cs) | `silent_foundry` | `silent_foundry_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 79 | `StartingLevelSaveStore` | [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs) | `starting_level` | `starting_level_save.json` | `SaveExists()`, `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 80 | `StealthSaveStore` | [`src/Host/StealthSaveStore.cs`](../../src/Host/StealthSaveStore.cs) | `expedition_stealth` | `stealth_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 81 | `SumpFloodingSaveStore` | [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs) | `sump_flooding` | `sump_flooding_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `ExpandedShelterSavePersistenceTests.cs` *(+1 more)* |
| 82 | `SurvivorFateSaveStore` | [`src/Host/SurvivorFateSaveStore.cs`](../../src/Host/SurvivorFateSaveStore.cs) | `survivor_fate` | `survivor_fate_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 83 | `SurvivorRelationsSaveStore` | [`src/Host/SurvivorRelationsSaveStore.cs`](../../src/Host/SurvivorRelationsSaveStore.cs) | `survivor_relations` | `survivor_relations_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 84 | `SurvivorSocialSaveStore` | [`src/Host/SurvivorSocialSaveStore.cs`](../../src/Host/SurvivorSocialSaveStore.cs) | `survivor_social` | `survivor_social_save.json` | `TryLoad()` | ✅ | ✅ | — |
| 85 | `SurvivorsSaveStore` | [`src/Host/SurvivorsSaveStore.cs`](../../src/Host/SurvivorsSaveStore.cs) | `survivors` | `survivors_save.json` | `TryLoad()` | ✅ | ✅ | `ActiveSaveSlotPersistenceTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+8 more)* |
| 86 | `ThirdonarySaveStore` | [`src/Host/ThirdonarySaveStore.cs`](../../src/Host/ThirdonarySaveStore.cs) | `thirdonary` | `thirdonary_quest_save.json` | `Save()`, `TryLoad()` | ✅ | ✅ | `CampaignEnvelopeBuilderTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` |
| 87 | `VerdictSaveStore` | [`src/Host/VerdictSaveStore.cs`](../../src/Host/VerdictSaveStore.cs) | `verdict` | `verdict_save.json` | `TryLoad()` | ✅ | ✅ | `CampaignConsequenceLedgerTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+1 more)* |
| 88 | `VinylMoraleSaveStore` | [`src/Host/VinylMoraleSaveStore.cs`](../../src/Host/VinylMoraleSaveStore.cs) | `vinyl_morale` | `vinyl_morale_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` |
| 89 | `WastelandMapSaveStore` | [`src/Host/WastelandMapSaveStore.cs`](../../src/Host/WastelandMapSaveStore.cs) | `wasteland_map` | `wasteland_map_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `SaveSectionRegistryTests.cs` |
| 90 | `WaterTreatmentSaveStore` | [`src/Host/WaterTreatmentSaveStore.cs`](../../src/Host/WaterTreatmentSaveStore.cs) | `water_treatment` | `water_treatment_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `EndToEndPlayerJourneyTests.cs` *(+1 more)* |
| 91 | `WaystationSaveStore` | [`src/Host/WaystationSaveStore.cs`](../../src/Host/WaystationSaveStore.cs) | `waystation` | `waystation_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `SaveChecksumTests.cs` |
| 92 | `WeatherSaveStore` | [`src/Host/WeatherSaveStore.cs`](../../src/Host/WeatherSaveStore.cs) | `weather` | `weather_save.json` | `TryLoad()` | ✅ | ✅ | `BareSaveStoreSealTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+2 more)* |
| 93 | `WeightOfChoicesSaveStore` | [`src/Host/WeightOfChoicesSaveStore.cs`](../../src/Host/WeightOfChoicesSaveStore.cs) | `weight_of_choices` | `weight_of_choices_save.json` | `TryLoad()` | ✅ | ✅ | `VersionReportContractTests.cs` |
| 94 | `WildlifeTrappingSaveStore` | [`src/Host/WildlifeTrappingSaveStore.cs`](../../src/Host/WildlifeTrappingSaveStore.cs) | `wildlife_trapping` | `wildlife_trapping_save.json` | `TryLoad()` | ✅ | ✅ | `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`, `PanelRouteGateTests.cs` *(+1 more)* |
| 95 | `WorldSaveStore` | [`src/Host/WorldSaveStore.cs`](../../src/Host/WorldSaveStore.cs) | `world` | `world_save.json` | `TryLoad()`, `TryLoadEnvelope()` | ✅ | ✅ | `ActiveSaveSlotPersistenceTests.cs`, `CampaignEnvelopeBuilderTests.cs` *(+4 more)* |
| 96 | `YearOfAshSaveStore` | [`src/YearOfAsh/YearOfAshSaveStore.cs`](../../src/YearOfAsh/YearOfAshSaveStore.cs) | `year_of_ash` | `year_of_ash_save.json` | `TryLoad()` | ✅ | ✅ | `CampaignCalendarTests.cs`, `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` *(+1 more)* |
