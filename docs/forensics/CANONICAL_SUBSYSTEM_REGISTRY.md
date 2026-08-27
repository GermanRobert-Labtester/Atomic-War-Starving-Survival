# Canonical Subsystem Registry

**Date:** 2026-08-26
**Total unique entries:** 578
**Authority:** Reconciled from 6 forensic surveys against live source code in `Assets/Ashfall.Core/` and `src/`

## 1. Summary by Kind

- **gameplay system**: 143
- **ui panel**: 120
- **catalog**: 108
- **host session**: 82
- **save store**: 81
- **save DTO/codec**: 37
- **domain component**: 7

## 2. Summary by Classification

- **LIVE_GODOT**: 285
- **LIVE_CORE + LIVE_GODOT**: 166
- **CORE_INTERNAL**: 119
- **LIVE_CORE**: 8

## 3. Orphan Candidate Reclassification (F0-2)

All 15 candidates previously reported as `PORTED_NOT_WIRED` / orphan systems have been forensically verified:

| Candidate System | Source Path | Host Entrypoint | Runtime Reachability | Disposition | Rationale |
|---|---|---|---|---|---|
| `BallisticsSystem` | `Assets/Ashfall.Core/Combat/BallisticsSystem.cs` | `TacticalCombatSystem` | proven | `CORE_INTERNAL` | Pure ballistic calculation collaborator of TacticalCombat; no standalone host needed |
| `CaregivingSystem` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | `CaregivingHostSession` | proven | `DIRECT_HOSTED` | Full host session + save store + panel wired in `Main.ExpandedShelterSystems.cs` |
| `ExpeditionVehicleSystem` | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | `ExpeditionSystem` | proven | `CORE_INTERNAL` | Vehicle maintenance and upgrade collaborator of Expedition system |
| `IdeologicalFrictionSystem` | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | `DutyRosterSystem` / `HoldfastRuntimeSession` | proven | `CORE_INTERNAL` | Dweller morale and tension collaborator; integrated with Roster |
| `LeadershipSystem` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | `DutyRosterSystem` / `Survivors` | proven | `CORE_INTERNAL` | Command role and initiative modifier collaborator |
| `PhantomMemorySystem` | `Assets/Ashfall.Core/PhantomMemoryEngine.cs` | `PhantomMemoryHostSession` | proven | `DIRECT_HOSTED` | Direct host session + save store + panel wired in `Main.Phase0.cs` and `Main.ShelterBatch3.cs` |
| `RationConflictSystem` | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | `HoldfastRuntimeSession` / `NeedsSystem` | proven | `CORE_INTERNAL` | Scarcity event collaborator of Needs system |
| `MaritimeDiveSystem` | `Assets/Ashfall.Core/MaritimeDiveSystem.cs` | `MaritimeHostSession` | proven | `INDIRECT_HOSTED` | Directly instantiated and managed within `MaritimeHostSession` and `MaritimePanel` |
| `OrbitalHarrowTelemetrySystem` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | `WeatherSystem` / `DeepFreeze` | proven | `CORE_INTERNAL` | Satellite tracking and storm forecasting collaborator |
| `PharmaLabSystem` | `Assets/Ashfall.Core/PharmaLabSystem.cs` | `MedicalWardSystem` / `MedicalSystem` | proven | `CORE_INTERNAL` | Medicine refinement and compounding collaborator of Medical ward |
| `SkillAtrophySystem` | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs` | `SkillProgressionSystem` | proven | `CORE_INTERNAL` | Skill decay calculation collaborator of SkillProgressionSystem |
| `TraumaBondSystem` | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` | `SurvivorRelationsSystem` | proven | `CORE_INTERNAL` | Interpersonal relationship modifier collaborator of SurvivorRelations |
| `WeaponConditionSystem` | `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs` | `TacticalCombatSystem` / `EquipmentCondition` | proven | `CORE_INTERNAL` | Weapon degradation and jam collaborator of Combat systems |
| `WeatherStationSystem` | `Assets/Ashfall.Core/WeatherStationSystem.cs` | `WeatherSystem` / `StartingLevelSystem` | proven | `CORE_INTERNAL` | Sensor calibration collaborator of WeatherSystem |
| `WorkshopReverseEngineeringSystem` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | `CraftingSystem` / `ResearchSystem` | proven | `CORE_INTERNAL` | Blueprint recovery and disassembly collaborator of Crafting |

## 4. Complete Canonical Registry

| Canonical Name | Kind | Source Path | Core Authority | Host Entrypoint | Runtime Reachable | Save Owner | Test Surface | Classification | Risk |
|---|---|---|---|---|---|---|---|---|---|
| `**Total**` | domain component | `Assets/Ashfall.Core/**Total**.cs` | yes | none | not proven | none | none | LIVE_CORE | LOW |
| `1 (1-30)` | domain component | `Assets/Ashfall.Core/1 (1-30).cs` | yes | none | not proven | none | none | LIVE_CORE | LOW |
| `2 (31-60)` | domain component | `Assets/Ashfall.Core/2 (31-60).cs` | yes | none | not proven | none | none | LIVE_CORE | LOW |
| `3 (61-110)` | domain component | `Assets/Ashfall.Core/3 (61-110).cs` | yes | none | not proven | none | none | LIVE_CORE | LOW |
| `4 (111-160)` | domain component | `Assets/Ashfall.Core/4 (111-160).cs` | yes | none | not proven | none | none | LIVE_CORE | LOW |
| `5 (161-210)` | domain component | `Assets/Ashfall.Core/5 (161-210).cs` | yes | none | not proven | none | none | LIVE_CORE | LOW |
| `6 (211-254)` | domain component | `Assets/Ashfall.Core/6 (211-254).cs` | yes | none | not proven | none | none | LIVE_CORE | LOW |
| `AbyssalAnomaliesCatalog` | catalog | `Assets/Ashfall.Core/Narrative/AbyssalAnomaliesCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: AbyssalAnomaliesCatalogTests.cs) | CORE_INTERNAL | LOW |
| `AchievementDetailPanel` | ui panel | `src/UI/AchievementDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `AchievementsPanel` | ui panel | `src/UI/AchievementsPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ActiveCraftSave` | save DTO/codec | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `AfflictionsPanel` | ui panel | `src/UI/AfflictionsPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `AirlockSecurityHostSave` | save store | `src/Host/AirlockSecuritySaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `AirlockSecurityHostSession` | host session | `src/Host/AirlockSecurityHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `AirlockSecurityPanel` | ui panel | `src/UI/AirlockSecurityPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `AirlockSecuritySaveStore` | save store | `src/Host/AirlockSecuritySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `AirlockSecuritySystem` | gameplay system | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` | yes | direct | proven | AirlockSecuritySaveStore | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ApicultureBeeCatalog` | catalog | `Assets/Ashfall.Core/Narrative/ApicultureBeeCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: ApicultureBeeCatalogTests.cs) | CORE_INTERNAL | LOW |
| `ApicultureSystem` | gameplay system | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: ApicultureAndTriangulationIntegrationTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `ApprenticeshipHostSave` | save store | `src/Host/ApprenticeshipSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `ApprenticeshipHostSession` | host session | `src/Host/ApprenticeshipHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ApprenticeshipPanel` | ui panel | `src/UI/ApprenticeshipPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ApprenticeshipSaveStore` | save store | `src/Host/ApprenticeshipSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ApprenticeshipSystem` | gameplay system | `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | yes | direct | proven | ApprenticeshipSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ArchiveDeskHostSave` | host session | `src/Host/ArchiveDeskHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: NewSaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `ArchiveDeskHostSession` | host session | `src/Host/ArchiveDeskHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ArchiveDeskPanel` | ui panel | `src/UI/ArchiveDeskPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ArchiveDeskSaveStore` | host session | `src/Host/ArchiveDeskHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ArchiveDeskSystem` | gameplay system | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` | yes | direct | proven | ArchiveDeskSaveStore | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `AudioConditionSystem` | gameplay system | `Assets/Ashfall.Core/AudioConditionSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `AudioCueCatalog` | catalog | `src/Audio/AudioCueCatalog.cs` | no (host) | direct (host) | proven | none | CatalogIntegrityValidator (129 catalogs) | LIVE_GODOT | LOW |
| `AutopsyHostSave` | save store | `src/Host/AutopsySaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `AutopsyHostSession` | host session | `src/Host/AutopsyHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `AutopsyReportPanel` | ui panel | `src/UI/AutopsyReportPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `AutopsySaveStore` | save store | `src/Host/AutopsySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `AutopsySystem` | gameplay system | `Assets/Ashfall.Core/AutopsySystem.cs` | yes | direct | proven | AutopsySaveStore | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `BallisticsSystem` | gameplay system | `Assets/Ashfall.Core/Combat/BallisticsSystem.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CombatBallisticsTests.cs) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `BlackProjectsCatalog` | catalog | `Assets/Ashfall.Core/Narrative/BlackProjectsCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: BlackProjectsCatalogTests.cs) | CORE_INTERNAL | LOW |
| `BoneHornCarvingCatalog` | catalog | `Assets/Ashfall.Core/Narrative/BoneHornCarvingCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: BoneHornCarvingCatalogTests.cs) | CORE_INTERNAL | LOW |
| `BrineExtractionPanel` | ui panel | `src/UI/BrineExtractionPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `BrineWaterSystem` | gameplay system | `Assets/Ashfall.Core/BrineWaterSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `BuiltInQuestlineCatalog` | catalog | `Assets/Ashfall.Core/YearOfAsh/BuiltInQuestlineCatalog.cs` | yes | core-internal | proven (Core) | none | CatalogIntegrityValidator (129 catalogs) | CORE_INTERNAL | LOW |
| `BunkerBlueprintCatalog` | catalog | `Assets/Ashfall.Core/Narrative/BunkerBlueprintCatalog.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `BunkerContrabandCatalog` | catalog | `Assets/Ashfall.Core/Narrative/BunkerContrabandCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: BunkerContrabandCatalogTests.cs) | CORE_INTERNAL | LOW |
| `BunkerCourtCatalog` | catalog | `Assets/Ashfall.Core/Narrative/BunkerCourtCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: BunkerCourtCatalogTests.cs) | CORE_INTERNAL | LOW |
| `BunkerGraffitiCatalog` | catalog | `Assets/Ashfall.Core/Narrative/BunkerGraffitiCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: BunkerGraffitiCatalogTests.cs) | CORE_INTERNAL | LOW |
| `BunkerMaintenanceCatalog` | catalog | `Assets/Ashfall.Core/Narrative/BunkerMaintenanceCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: BunkerMaintenanceCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CampaignDaySave` | save DTO/codec | `Assets/Ashfall.Core/Campaign/CampaignDaySave.cs` | yes | indirect | proven | none | none | LIVE_CORE + LIVE_GODOT | LOW |
| `CampaignDaySaveStore` | save store | `src/Host/CampaignDaySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `CandleMakingWaxCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CandleMakingWaxCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CandleMakingWaxCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CaravanBarterLedgerPanel` | ui panel | `src/UI/CaravanBarterLedgerPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CaravanHostSave` | save store | `src/Host/CaravanSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CaravanSaveStore` | save store | `src/Host/CaravanSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `CaregivingHostSave` | save store | `src/Host/CaregivingSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `CaregivingHostSession` | host session | `src/Host/CaregivingHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `CaregivingPanel` | ui panel | `src/UI/CaregivingPanel.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `CaregivingSaveStore` | save store | `src/Host/CaregivingSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `CaregivingSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | yes | direct | proven | CaregivingSaveStore | Core tests (1 suites: CaregivingSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `CatalogFileSystem` | catalog | `Assets/Ashfall.Core/CatalogFileSystem.cs` | yes | indirect | proven | none | Core tests (1 suites: CatalogFileSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `CatalogIntegrityValidator` | catalog | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | yes | indirect | proven | none | Core tests (7 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CensusClaimSystem` | gameplay system | `Assets/Ashfall.Core/CensusClaimSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CenturySeedPanel` | ui panel | `src/UI/CenturySeedPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CeramicsKilnCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CeramicsKilnCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CeramicsKilnCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CharcoalPyrolysisCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CharcoalPyrolysisCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CharcoalPyrolysisCatalogTests.cs) | CORE_INTERNAL | LOW |
| `ChemicalDependencyHostSave` | save store | `src/Host/ChemicalDependencySaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: BareSaveStoreSealTests.cs) | LIVE_GODOT | LOW |
| `ChemicalDependencyHostSession` | host session | `src/Host/ChemicalDependencyHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ChemicalDependencyPanel` | ui panel | `src/UI/ChemicalDependencyPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ChemicalDependencySaveStore` | save store | `src/Host/ChemicalDependencySaveStore.cs` | no (host) | direct (host) | proven | store (self) | Core tests (1 suites: BareSaveStoreSealTests.cs) | LIVE_GODOT | LOW |
| `ChemicalDependencySystem` | gameplay system | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | yes | direct | proven | ChemicalDependencySaveStore | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CoalitionCampSystem` | gameplay system | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: CoalitionCampSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `CohortSystem` | gameplay system | `Assets/Ashfall.Core/CohortSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ColdCountSystem` | gameplay system | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CombatCatalog` | catalog | `Assets/Ashfall.Core/Combat/CombatCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (6 suites) | CORE_INTERNAL | LOW |
| `CombatDetailPanel` | ui panel | `src/UI/CombatDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CombatHistoryPanel` | ui panel | `src/UI/CombatHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CombatHostSave` | save store | `src/Host/CombatSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CombatHostSession` | host session | `src/Host/CombatHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `CombatPanel` | ui panel | `src/UI/CombatPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CombatSaveStore` | save store | `src/Host/CombatSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `CombatSystem` | gameplay system | `Assets/Ashfall.Core/CombatSystem.cs` | yes | direct | proven | CombatSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CombatTraumaSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: CombatTraumaSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `ContaminationEntrySave` | save DTO/codec | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `ContaminationSurvivorSave` | save DTO/codec | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `ContractorRosterHostSave` | host session | `src/Host/ContractorRosterHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: NewSaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `ContractorRosterHostSession` | host session | `src/Host/ContractorRosterHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ContractorRosterPanel` | ui panel | `src/UI/ContractorRosterPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ContractorRosterSaveStore` | host session | `src/Host/ContractorRosterHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ContractorRosterSystem` | gameplay system | `Assets/Ashfall.Core/ContractorRosterSystem.cs` | yes | direct | proven | ContractorRosterSaveStore | Core tests (1 suites: ContractorRosterSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `CordageCableCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CordageCableCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CordageCableCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CourierDispatchCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CourierDispatchCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CourierDispatchCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CraftingDetailPanel` | ui panel | `src/UI/CraftingDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CraftingHistoryPanel` | ui panel | `src/UI/CraftingHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CraftingHostSave` | save store | `src/Host/CraftingSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CraftingHostSession` | host session | `src/Host/CraftingHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `CraftingPanel` | ui panel | `src/UI/CraftingPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CraftingSaveStore` | save store | `src/Host/CraftingSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `CraftingSystem` | gameplay system | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | yes | direct | proven | CraftingSaveStore | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CraftingSystemSave` | save DTO/codec | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | yes | indirect | proven | system (CaptureState) | none | LIVE_CORE + LIVE_GODOT | LOW |
| `CropCatalog` | catalog | `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs` | yes | core-internal | proven (Core) | none | CatalogIntegrityValidator (129 catalogs) | CORE_INTERNAL | LOW |
| `CrossingArbitrationSystem` | gameplay system | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CrossingCatalog` | catalog | `Assets/Ashfall.Core/CrossingCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: ExpansionsIntegrationTests.cs) | CORE_INTERNAL | LOW |
| `CrossingQuestPanel` | ui panel | `src/UI/CrossingQuestPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `CrossingQuestSystem` | gameplay system | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | yes | direct | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CrucibleFoundryCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CrucibleFoundryCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CrucibleFoundryCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CryoPreservationCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CryoPreservationCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CryoPreservationCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CulinaryRationCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CulinaryRationCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CulinaryRationCatalogTests.cs) | CORE_INTERNAL | LOW |
| `CurrentsCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CurrentsCatalog.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `CurrentsPamphletCatalog` | catalog | `Assets/Ashfall.Core/Narrative/CurrentsPamphletCatalog.cs` | yes | core-internal | proven (Core) | none | CatalogIntegrityValidator (129 catalogs) | CORE_INTERNAL | LOW |
| `DailyBriefingSave` | save DTO/codec | `Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: DailyBriefingReportBuilderTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `DailyBriefingSaveStore` | save store | `src/Host/DailyBriefingSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `DailySurvivalCatalog` | catalog | `Assets/Ashfall.Core/Narrative/DailySurvivalCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: DailySurvivalCatalogTests.cs) | CORE_INTERNAL | LOW |
| `DeadHandDirectiveCatalog` | catalog | `Assets/Ashfall.Core/Narrative/DeadHandDirectiveCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: DeadHandDirectiveCatalogTests.cs) | CORE_INTERNAL | LOW |
| `DecontaminationHostSave` | host session | `src/Host/DecontaminationHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: NewSaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `DecontaminationHostSession` | host session | `src/Host/DecontaminationHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `DecontaminationPanel` | ui panel | `src/UI/DecontaminationPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DecontaminationSaveStore` | host session | `src/Host/DecontaminationHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `DecontaminationSystem` | gameplay system | `Assets/Ashfall.Core/DecontaminationSystem.cs` | yes | direct | proven | DecontaminationSaveStore | Core tests (1 suites: DecontaminationSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `DeepCoastHostSession` | host session | `src/Host/DeepCoastHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `DeepCoastPanel` | ui panel | `src/UI/DeepCoastPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DiseaseCatalog` | catalog | `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs` | yes | indirect | proven | none | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DiseaseHostSession` | host session | `src/Disease/DiseaseHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DiseaseSaveStore` | save store | `src/Host/DiseaseSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `DiseaseSystem` | gameplay system | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | yes | direct | proven | DiseaseSaveStore | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `District8DeepCoastSystem` | gameplay system | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: District8DeepCoastTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `DiveSiteCatalog` | catalog | `Assets/Ashfall.Core/Narrative/DiveSiteCatalog.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: ExpansionAggregateCompletenessTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `DoorEncounterSystem` | gameplay system | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DoseContentCatalog` | catalog | `Assets/Ashfall.Core/DoseContentCatalog.cs` | yes | indirect | proven | none | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DoseLedgerHostSession` | host session | `src/Host/DoseLedgerHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DoseLedgerPanel` | ui panel | `src/UI/DoseLedgerPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DoseLedgerSave` | save DTO/codec | `Assets/Ashfall.Core/DoseLedgerSave.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DoseLedgerSaveStore` | save store | `src/Host/DoseLedgerSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `DoseLedgerSystem` | gameplay system | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | yes | direct | proven | DoseLedgerSaveStore | Core tests (6 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DoseRegistersCatalog` | catalog | `Assets/Ashfall.Core/DoseRegistersCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: DoseRegistersCatalogTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `DosimeterCalibrationSystem` | gameplay system | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: DosimeterCalibrationSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `DosimeterSave` | save DTO/codec | `Assets/Ashfall.Core/Radiation/Dosimeter.cs` | yes | core-internal | proven (Core) | none | none | CORE_INTERNAL | LOW |
| `DutyRosterAssignmentEngine` | gameplay system | `Assets/Ashfall.Core/DutyRoster/DutyRosterAssignmentEngine.cs` | yes | core-internal | proven (Core) | none | none | CORE_INTERNAL | LOW |
| `DutyRosterCatalog` | catalog | `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DutyRosterChartEngine` | gameplay system | `Assets/Ashfall.Core/DutyRoster/DutyRosterChartEngine.cs` | yes | core-internal | proven (Core) | none | none | CORE_INTERNAL | LOW |
| `DutyRosterDetailPanel` | ui panel | `src/UI/DutyRosterDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DutyRosterHostSession` | host session | `src/Host/DutyRosterHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DutyRosterOverflowEngine` | gameplay system | `Assets/Ashfall.Core/DutyRoster/DutyRosterOverflowEngine.cs` | yes | core-internal | proven (Core) | none | none | CORE_INTERNAL | LOW |
| `DutyRosterPanel` | ui panel | `src/UI/DutyRosterPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `DutyRosterSave` | save DTO/codec | `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DutyRosterSaveStore` | save store | `src/Host/DutyRosterSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `DutyRosterSystem` | gameplay system | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | yes | direct | proven | DutyRosterSaveStore | Core tests (11 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `DwellerHeirloomCatalog` | catalog | `Assets/Ashfall.Core/Narrative/DwellerHeirloomCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: DwellerHeirloomCatalogTests.cs) | CORE_INTERNAL | LOW |
| `DwellerMedicalCatalog` | catalog | `Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: DwellerMedicalCatalogTests.cs) | CORE_INTERNAL | LOW |
| `EconomyDetailPanel` | ui panel | `src/UI/EconomyDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `EconomyHostSession` | host session | `src/Host/EconomyHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `EconomyMarketPanel` | ui panel | `src/Economy/EconomyMarketPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `EconomyOverlayPanel` | ui panel | `src/UI/EconomyPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `EconomySaveStore` | save store | `src/Host/EconomySaveStore.cs` | no (host) | direct (host) | proven | store (self) | Core tests (1 suites: EconomyProbeTests.cs) | LIVE_GODOT | LOW |
| `EncounterCatalog` | catalog | `Assets/Ashfall.Core/Narrative/EncounterCatalog.cs` | yes | indirect | proven | none | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `EncounterChoiceSaveStore` | save store | `src/Host/EncounterChoiceSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `EpiloguePanel` | ui panel | `src/UI/EpiloguePanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `EquipmentConditionHostSave` | host session | `src/Host/EquipmentConditionHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: NewSaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `EquipmentConditionHostSession` | host session | `src/Host/EquipmentConditionHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `EquipmentConditionPanel` | ui panel | `src/UI/EquipmentConditionPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `EquipmentConditionSaveStore` | host session | `src/Host/EquipmentConditionHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `EquipmentConditionSystem` | gameplay system | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | yes | direct | proven | EquipmentConditionSaveStore | Core tests (1 suites: EquipmentConditionSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `EquippedSave` | save DTO/codec | `Assets/Ashfall.Core/Inventory/Inventory.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `EventDetailPanel` | ui panel | `src/UI/EventDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `EventsHostSession` | host session | `src/Host/EventsHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `EventsLogPanel` | ui panel | `src/UI/EventsLogPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExcavationHostSave` | save store | `src/Host/ExcavationSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExcavationHostSession` | host session | `src/Host/ExcavationHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ExcavationPanel` | ui panel | `src/UI/ExcavationPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExcavationSaveStore` | save store | `src/Host/ExcavationSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ExcavationSystem` | gameplay system | `Assets/Ashfall.Core/ExcavationSystem.cs` | yes | direct | proven | ExcavationSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ExpansionEnrichmentCatalog` | catalog | `Assets/Ashfall.Core/ExpansionEnrichmentCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `ExpansionHostSession` | host session | `src/Host/ExpansionHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: ExpansionHubSaveTests.cs) | LIVE_GODOT | LOW |
| `ExpansionHubSave` | save DTO/codec | `Assets/Ashfall.Core/ExpansionHubSave.cs` | yes | indirect | proven | system (CaptureState) | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ExpansionHubSaveStore` | save store | `src/Host/ExpansionHubSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ExpansionQuestHostSession` | host session | `src/Host/ExpansionQuestHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ExpansionQuestSaveStore` | save store | `src/Host/ExpansionQuestSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ExpansionQuestSystem` | gameplay system | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` | yes | direct | proven | ExpansionQuestSaveStore | Core tests (1 suites: ExpansionQuestSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `ExpansionsHubPanel` | ui panel | `src/UI/ExpansionsHubPanel.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ExpeditionCampPanel` | ui panel | `src/UI/ExpeditionCampPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExpeditionDetailPanel` | ui panel | `src/UI/ExpeditionDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExpeditionHistoryPanel` | ui panel | `src/UI/ExpeditionHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExpeditionHostSave` | save store | `src/Host/ExpeditionSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: SaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `ExpeditionHostSession` | host session | `src/Host/ExpeditionHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: ExpeditionEncounterBridgeTests.cs) | LIVE_GODOT | LOW |
| `ExpeditionPanel` | ui panel | `src/UI/ExpeditionPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExpeditionRadarPanel` | ui panel | `src/UI/ExpeditionRadarPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ExpeditionSaveStore` | save store | `src/Host/ExpeditionSaveStore.cs` | no (host) | direct (host) | proven | store (self) | Core tests (2 suites) | LIVE_GODOT | LOW |
| `ExpeditionSystem` | gameplay system | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | yes | direct | proven | ExpeditionSaveStore | Core tests (6 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ExpeditionVehicleSystem` | gameplay system | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `FactionDetailPanel` | ui panel | `src/UI/FactionDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `FactionHistoryPanel` | ui panel | `src/UI/FactionHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `FactionIconCatalog` | catalog | `Assets/Ashfall.Core/UI/FactionIconCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: FactionIconCatalogTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `FactionMatrixPanel` | ui panel | `src/UI/FactionMatrixPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `FactionRadioEngine` | gameplay system | `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs` | yes | indirect | proven | none | Core tests (1 suites: FactionRadioCorpusTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `FactionRadioHudPanel` | ui panel | `src/Radio/FactionRadioHudPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `FactionStanceEngine` | gameplay system | `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` | yes | indirect | proven | none | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `FactionWarContentCatalog` | catalog | `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (4 suites) | CORE_INTERNAL | LOW |
| `FactionWarSystem` | gameplay system | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `FactionsNarrativePanel` | ui panel | `src/UI/FactionsNarrativePanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `FactionsPanel` | ui panel | `src/UI/FactionsPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `FaunaEntomologyCatalog` | catalog | `Assets/Ashfall.Core/Narrative/FaunaEntomologyCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: FaunaEntomologyCatalogTests.cs) | CORE_INTERNAL | LOW |
| `FermentationYeastCatalog` | catalog | `Assets/Ashfall.Core/Narrative/FermentationYeastCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: FermentationYeastCatalogTests.cs) | CORE_INTERNAL | LOW |
| `FinalWishSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: FinalWishSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `FireIncidentPanel` | ui panel | `src/UI/FireIncidentPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `FringeCultsCatalog` | catalog | `Assets/Ashfall.Core/Narrative/FringeCultsCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: FringeCultsCatalogTests.cs) | CORE_INTERNAL | LOW |
| `GameDashboardPanel` | ui panel | `src/UI/GameDashboardPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `GameOverPanel` | ui panel | `src/UI/GameOverPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `GeigerCalibrationPanel` | ui panel | `src/UI/GeigerCalibrationPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `GenerationalSuccessionEngine` | gameplay system | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `GeologicalStrataCatalog` | catalog | `Assets/Ashfall.Core/Narrative/GeologicalStrataCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: GeologicalStrataCatalogTests.cs) | CORE_INTERNAL | LOW |
| `GhostTransmissionCatalog` | catalog | `Assets/Ashfall.Core/Narrative/GhostTransmissionCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: GhostTransmissionCatalogTests.cs) | CORE_INTERNAL | LOW |
| `GlassblowingDistillationCatalog` | catalog | `Assets/Ashfall.Core/Narrative/GlassblowingDistillationCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: GlassblowingDistillationCatalogTests.cs) | CORE_INTERNAL | LOW |
| `GoodsCatalog` | catalog | `Assets/Ashfall.Core/Economy/GoodsCatalog.cs` | yes | indirect | proven | none | Core tests (7 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `GrainMillingCatalog` | catalog | `Assets/Ashfall.Core/Narrative/GrainMillingCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: GrainMillingCatalogTests.cs) | CORE_INTERNAL | LOW |
| `GreenhouseExpansionCatalog` | catalog | `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: GreenhouseSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `GreenhouseHostSession` | host session | `src/Host/GreenhouseHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `GreenhousePanel` | ui panel | `src/UI/GreenhousePanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `GreenhouseSaveStore` | host session | `src/Host/GreenhouseHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `GreenhouseSystem` | gameplay system | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | yes | direct | proven | GreenhouseSaveStore | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `GuiltInsomniaSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: GuiltInsomniaSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `HoldfastCatalog` | catalog | `Assets/Ashfall.Core/HoldfastCatalog.cs` | yes | indirect | proven | none | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `HoldfastFactionsCatalog` | catalog | `Assets/Ashfall.Core/HoldfastFactionsCatalog.cs` | yes | indirect | proven | none | CatalogIntegrityValidator (129 catalogs) | LIVE_CORE + LIVE_GODOT | LOW |
| `HoldfastFlavorCatalog` | catalog | `src/Host/HoldfastFlavorCatalog.cs` | no (host) | direct (host) | proven | none | CatalogIntegrityValidator (129 catalogs) | LIVE_GODOT | LOW |
| `HoldfastItemsCatalog` | catalog | `Assets/Ashfall.Core/HoldfastItemsCatalog.cs` | yes | core-internal | proven (Core) | none | CatalogIntegrityValidator (129 catalogs) | CORE_INTERNAL | LOW |
| `HoldfastNpcCatalog` | catalog | `Assets/Ashfall.Core/Narrative/HoldfastNpcCatalog.cs` | yes | core-internal | proven (Core) | none | CatalogIntegrityValidator (129 catalogs) | CORE_INTERNAL | LOW |
| `HoldfastQuestSystem` | gameplay system | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `HoldfastSave` | save DTO/codec | `Assets/Ashfall.Core/HoldfastSave.cs` | yes | indirect | proven | system (CaptureState) | Core tests (6 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `HoldfastSaveStore` | save store | `src/Host/HoldfastSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `HoldfastTerminalPanel` | ui panel | `src/Host/HoldfastTerminalPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `HoldfastTradeSaveStore` | save store | `src/Host/HoldfastTradeSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `HoldfastTradeSession` | gameplay system | `Assets/Ashfall.Core/HoldfastTradeSession.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: HoldfastTradeSessionTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `HostEventHostSave` | save store | `src/Host/HostEventSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: BareSaveStoreSealTests.cs) | LIVE_GODOT | LOW |
| `HostEventSaveStore` | save store | `src/Host/HostEventSaveStore.cs` | no (host) | direct (host) | proven | store (self) | Core tests (1 suites: BareSaveStoreSealTests.cs) | LIVE_GODOT | LOW |
| `HydroBaronsSystem` | gameplay system | `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `HydroGeologyCatalog` | catalog | `Assets/Ashfall.Core/Narrative/HydroGeologyCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: HydroGeologyCatalogTests.cs) | CORE_INTERNAL | LOW |
| `IceRoadSystem` | gameplay system | `Assets/Ashfall.Core/IceRoadSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `IdeologicalFrictionSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: IdeologicalFrictionSystemTests.cs) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `IndependentBranchCatalog` | catalog | `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (3 suites) | CORE_INTERNAL | LOW |
| `IndependentBranchSave` | save DTO/codec | `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: IndependentBranchSystemTests.cs) | CORE_INTERNAL | LOW |
| `IndependentBranchSystem` | gameplay system | `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs` | yes | core-internal | proven (Core) | codec | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `IndustrialRuinsCatalog` | catalog | `Assets/Ashfall.Core/Narrative/IndustrialRuinsCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: IndustrialRuinsCatalogTests.cs) | CORE_INTERNAL | LOW |
| `InventoryDetailPanel` | ui panel | `src/UI/InventoryDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `InventoryHostSave` | save store | `src/Host/InventorySaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `InventoryHostSession` | host session | `src/Host/InventoryHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `InventoryPanel` | ui panel | `src/UI/InventoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `InventorySaveStore` | save store | `src/Host/InventorySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `IronRaidersSystem` | gameplay system | `Assets/Ashfall.Core/Muster/IronRaidersSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: MusterCurrentSystemsTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `ItemCatalog` | catalog | `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `JournalDetailPanel` | ui panel | `src/UI/JournalDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `JournalHostSave` | save store | `src/Journal/JournalSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: SaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `JournalHostSession` | host session | `src/Host/JournalHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `JournalPanel` | ui panel | `src/UI/JournalPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `JournalSave` | save DTO/codec | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `JournalSaveStore` | save store | `src/Journal/JournalSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `JournalSystem` | gameplay system | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | yes | direct | proven | JournalSaveStore | Core tests (7 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `JournalWitnessPanel` | ui panel | `src/Muster/JournalWitnessPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `KitchenNutritionHostSave` | host session | `src/Host/KitchenNutritionHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: NewSaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `KitchenNutritionHostSession` | host session | `src/Host/KitchenNutritionHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `KitchenNutritionPanel` | ui panel | `src/UI/KitchenNutritionPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `KitchenNutritionSaveStore` | host session | `src/Host/KitchenNutritionHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `KitchenNutritionSystem` | gameplay system | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | yes | direct | proven | KitchenNutritionSaveStore | Core tests (1 suites: KitchenNutritionSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `KnowledgeBaseSave` | save DTO/codec | `Assets/Ashfall.Core/Journal/KnowledgeBase.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `LandmarkDegradationSystem` | gameplay system | `Assets/Ashfall.Core/LandmarkDegradationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: WorldSaveablesTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `LeadershipSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: LeadershipSystemTests.cs) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `LedgerDebtSystem` | gameplay system | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (6 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `LibraryStudyHostSave` | host session | `src/Host/LibraryStudyHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: NewSaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `LibraryStudyHostSession` | host session | `src/Host/LibraryStudyHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `LibraryStudyPanel` | ui panel | `src/UI/LibraryStudyPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `LibraryStudySaveStore` | host session | `src/Host/LibraryStudyHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `LibraryStudySystem` | gameplay system | `Assets/Ashfall.Core/LibraryStudySystem.cs` | yes | direct | proven | LibraryStudySaveStore | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `LocationEvolutionSystem` | gameplay system | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: WorldSaveablesTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `LocationLayoutParentSave` | save DTO/codec | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `LocationLayoutSystem` | gameplay system | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `LocationMemorySystem` | gameplay system | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `LocationVisitSave` | save DTO/codec | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `LongWalkSystem` | gameplay system | `Assets/Ashfall.Core/Muster/LongWalkSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: MusterCurrentSystemsTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `LostTechManualCatalog` | catalog | `Assets/Ashfall.Core/Narrative/LostTechManualCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: LostTechManualCatalogTests.cs) | CORE_INTERNAL | LOW |
| `MachineLogSystem` | gameplay system | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MainMenuPanel` | ui panel | `src/UI/MainMenuPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MapAtlasPanel` | ui panel | `src/UI/MapAtlasPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MapDetailPanel` | ui panel | `src/UI/MapDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MapPanel` | ui panel | `src/UI/MapPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MaritimeAtlasPanel` | ui panel | `src/UI/MaritimeAtlasPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MaritimeDiveSystem` | gameplay system | `Assets/Ashfall.Core/MaritimeDiveSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `MaritimeHostSave` | host session | `src/Host/MaritimeHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `MaritimeHostSession` | host session | `src/Host/MaritimeHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `MaritimePanel` | ui panel | `src/UI/MaritimePanel.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `MaritimeSaveStore` | save store | `src/Host/MaritimeSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `MaritimeSystem` | gameplay system | `Assets/Ashfall.Core/MaritimeSystem.cs` | yes | direct | proven | MaritimeSaveStore | none | LIVE_CORE + LIVE_GODOT | LOW |
| `MarketSystem` | gameplay system | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (8 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MasonryBrickworksCatalog` | catalog | `Assets/Ashfall.Core/Narrative/MasonryBrickworksCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: MasonryBrickworksCatalogTests.cs) | CORE_INTERNAL | LOW |
| `MaterialShieldingSave` | save DTO/codec | `Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: MaterialShieldingSystemTests.cs) | CORE_INTERNAL | LOW |
| `MaterialShieldingSystem` | gameplay system | `Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: MaterialShieldingSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `MedicalBedSave` | save DTO/codec | `Assets/Ashfall.Core/Medical/MedicalWardSave.cs` | yes | indirect | proven | none | Core tests (1 suites: MedicalWardSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `MedicalDetailPanel` | ui panel | `src/UI/MedicalDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MedicalHistoryPanel` | ui panel | `src/UI/MedicalHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MedicalHostSave` | save store | `src/Host/MedicalSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: SaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `MedicalHostSession` | host session | `src/Host/MedicalHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `MedicalPanel` | ui panel | `src/UI/MedicalPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MedicalPathologyCatalog` | catalog | `Assets/Ashfall.Core/Narrative/MedicalPathologyCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: MedicalPathologyCatalogTests.cs) | CORE_INTERNAL | LOW |
| `MedicalSaveStore` | save store | `src/Host/MedicalSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `MedicalSystem` | gameplay system | `Assets/Ashfall.Core/MedicalSystem.cs` | yes | direct | proven | MedicalSaveStore | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MedicalWardHostSession` | host session | `src/Host/MedicalWardHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `MedicalWardPanel` | ui panel | `src/UI/MedicalWardPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MedicalWardSave` | save DTO/codec | `Assets/Ashfall.Core/Medical/MedicalWardSave.cs` | yes | indirect | proven | none | Core tests (1 suites: MedicalWardSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `MedicalWardSaveStore` | save store | `src/Host/MedicalWardSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `MedicalWardSystem` | gameplay system | `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` | yes | direct | proven | MedicalWardSaveStore | Core tests (6 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MemorialSave` | save DTO/codec | `Assets/Ashfall.Core/Memorial/MemorialSave.cs` | yes | indirect | proven | none | none | LIVE_CORE + LIVE_GODOT | LOW |
| `MemorialSaveStore` | save store | `src/Host/MemorialSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `MemorialSystem` | gameplay system | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | yes | indirect | proven | MemorialSaveStore | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MentalHealthCrisisHostSave` | host session | `src/Host/MentalHealthCrisisHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: NewSaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `MentalHealthCrisisHostSession` | host session | `src/Host/MentalHealthCrisisHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `MentalHealthCrisisPanel` | ui panel | `src/UI/MentalHealthCrisisPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MentalHealthCrisisSaveStore` | host session | `src/Host/MentalHealthCrisisHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `MentalHealthCrisisSystem` | gameplay system | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` | yes | direct | proven | MentalHealthCrisisSaveStore | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MetallurgyToolingCatalog` | catalog | `Assets/Ashfall.Core/Narrative/MetallurgyToolingCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: MetallurgyToolingCatalogTests.cs) | CORE_INTERNAL | LOW |
| `MilitaryArmoryCatalog` | catalog | `Assets/Ashfall.Core/Narrative/MilitaryArmoryCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: MilitaryArmoryCatalogTests.cs) | CORE_INTERNAL | LOW |
| `MilitaryBranchCatalog` | catalog | `Assets/Ashfall.Core/Factions/MilitaryBranchCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (5 suites) | CORE_INTERNAL | LOW |
| `MilitaryBranchSave` | save DTO/codec | `Assets/Ashfall.Core/Factions/MilitaryBranchSave.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: MilitaryBranchSystemTests.cs) | CORE_INTERNAL | LOW |
| `MilitaryBranchSystem` | gameplay system | `Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs` | yes | core-internal | proven (Core) | codec | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `MoralBranchingSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: MoralBranchingSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `MoralChoiceHostSave` | save store | `src/Host/MoralChoiceSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MoralChoiceSaveStore` | save store | `src/Host/MoralChoiceSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `MoralChoiceSystem` | gameplay system | `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` | yes | indirect | proven | MoralChoiceSaveStore | Core tests (8 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MoraleMarkSystem` | gameplay system | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `MusterAtlasPanel` | ui panel | `src/UI/MusterAtlasPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MusterHostSave` | save store | `src/Host/MusterSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MusterHostSession` | host session | `src/Host/MusterHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `MusterPanel` | ui panel | `src/UI/MusterPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `MusterSaveStore` | save store | `src/Host/MusterSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `MusterSystem` | gameplay system | `Assets/Ashfall.Core/Muster/MusterSystem.cs` | yes | direct | proven | MusterSaveStore | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `NarrativeBatchCatalog` | catalog | `Assets/Ashfall.Core/Narrative/NarrativeBatchCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: SilentFoundrySystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `NarrativeEncounterSystem` | gameplay system | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `NarrativeHostSave` | save store | `src/Host/NarrativeSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (2 suites) | LIVE_GODOT | LOW |
| `NarrativeHostSession` | host session | `src/Host/NarrativeHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `NarrativeSaveStore` | save store | `src/Host/NarrativeSaveStore.cs` | no (host) | direct (host) | proven | store (self) | Core tests (2 suites) | LIVE_GODOT | LOW |
| `NeedsSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | yes | indirect | proven | none | Core tests (14 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `NightWatchCatalog` | catalog | `Assets/Ashfall.Core/Narrative/NightWatchCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: NightWatchCatalogTests.cs) | CORE_INTERNAL | LOW |
| `OpticsGlassworksCatalog` | catalog | `Assets/Ashfall.Core/Narrative/OpticsGlassworksCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: OpticsGlassworksCatalogTests.cs) | CORE_INTERNAL | LOW |
| `OralLoreCatalog` | catalog | `Assets/Ashfall.Core/Narrative/OralLoreCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: OralLoreCatalogTests.cs) | CORE_INTERNAL | LOW |
| `OrbitalHarrowTelemetrySystem` | gameplay system | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `PaperMakingCatalog` | catalog | `Assets/Ashfall.Core/Narrative/PaperMakingCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: PaperMakingCatalogTests.cs) | CORE_INTERNAL | LOW |
| `PaperPrintingCatalog` | catalog | `Assets/Ashfall.Core/Narrative/PaperPrintingCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: PaperPrintingCatalogTests.cs) | CORE_INTERNAL | LOW |
| `PhantomMemoryEngine` | gameplay system | `Assets/Ashfall.Core/PhantomMemoryEngine.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: PhantomMemoryEngineTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `PhantomMemoryHostSave` | save store | `src/Host/PhantomMemorySaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `PhantomMemoryHostSession` | host session | `src/Host/PhantomMemoryHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `PhantomMemoryPanel` | ui panel | `src/UI/PhantomMemoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `PhantomMemorySaveStore` | save store | `src/Host/PhantomMemorySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `PhantomMemorySystem` | gameplay system | `Assets/Ashfall.Core/PhantomMemoryEngine.cs` | yes | direct | proven | PhantomMemorySaveStore | Core tests (1 suites: PhantomMemoryEngineTests.cs) | LIVE_CORE + LIVE_GODOT | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `PharmaLabSystem` | gameplay system | `Assets/Ashfall.Core/PharmaLabSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `PharmaRecipeCatalog` | catalog | `Assets/Ashfall.Core/PharmaLabSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: IslandBridgesTests.cs) | CORE_INTERNAL | LOW |
| `Phase0HostSave` | save store | `src/Host/Phase0SaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `Phase0HostSession` | host session | `src/Host/Phase0HostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (1 suites: GapTestCoverageTests.cs) | LIVE_GODOT | LOW |
| `Phase0Panel` | ui panel | `src/UI/Phase0Panel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `Phase0SaveStore` | save store | `src/Host/Phase0SaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `PhaseProgressionSurvivorSave` | save DTO/codec | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `PneumaticTubeDispatchCatalog` | catalog | `Assets/Ashfall.Core/Narrative/PneumaticTubeDispatchCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: PneumaticTubeDispatchCatalogTests.cs) | CORE_INTERNAL | LOW |
| `PolymerTextileCatalog` | catalog | `Assets/Ashfall.Core/Narrative/PolymerTextileCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: PolymerTextileCatalogTests.cs) | CORE_INTERNAL | LOW |
| `PowerGridHostSession` | host session | `src/Host/PowerGridHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `PowerGridPanel` | ui panel | `src/UI/PowerGridPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `PowerGridRoomSave` | save DTO/codec | `Assets/Ashfall.Core/Shelter/PowerGridSave.cs` | yes | indirect | proven | none | Core tests (1 suites: PowerGridSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `PowerGridSave` | save DTO/codec | `Assets/Ashfall.Core/Shelter/PowerGridSave.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `PowerGridSaveStore` | save store | `src/Host/PowerGridSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `PowerGridSystem` | gameplay system | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | yes | direct | proven | PowerGridSaveStore | Core tests (9 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ProceduralEulogyEngine` | gameplay system | `Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `ProceduralScavengeSave` | save DTO/codec | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` | yes | indirect | proven | system (CaptureState) | none | LIVE_CORE + LIVE_GODOT | LOW |
| `ProceduralScavengeSystem` | gameplay system | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ProvisionedSystem` | gameplay system | `Assets/Ashfall.Core/Muster/ProvisionedSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: MusterCurrentSystemsTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `PrpfSave` | save DTO/codec | `Assets/Ashfall.Core/Factions/PrpfSave.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: PrpfStandingSystemTests.cs) | CORE_INTERNAL | LOW |
| `PrpfStandingSystem` | gameplay system | `Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (3 suites) | CORE_INTERNAL | LOW |
| `PsychContaminationSave` | save DTO/codec | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | yes | indirect | proven | system (CaptureState) | none | LIVE_CORE + LIVE_GODOT | LOW |
| `PsychologicalContaminationSystem` | gameplay system | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: BlackFlotillaTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `QuestDetailPanel` | ui panel | `src/UI/QuestDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `QuestlineMasterCatalog` | catalog | `Assets/Ashfall.Core/QuestlineMasterCatalog.cs` | yes | indirect | proven | none | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `QuestlineSystem` | gameplay system | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (9 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `QuestsAtlasPanel` | ui panel | `src/UI/QuestsAtlasPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `QuestsPanel` | ui panel | `src/UI/QuestsPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `RadiationDetailPanel` | ui panel | `src/UI/RadiationDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `RadiationHistoryPanel` | ui panel | `src/UI/RadiationHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `RadiationSystem` | gameplay system | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | yes | indirect | proven | none | Core tests (12 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `RadioDetailPanel` | ui panel | `src/UI/RadioDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `RadioHostSession` | host session | `src/Host/RadioHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `RadioPanel` | ui panel | `src/UI/RadioPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `RadioSaveStore` | save store | `src/Host/RadioSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `RadioScriptbookCatalog` | catalog | `Assets/Ashfall.Core/Narrative/RadioScriptbookCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: RadioScriptbookCatalogTests.cs) | CORE_INTERNAL | LOW |
| `RadioSystem` | gameplay system | `Assets/Ashfall.Core/RadioSystem.cs` | yes | direct | proven | RadioSaveStore | Core tests (1 suites: VerdictRadioSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `RationConflictSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: RationConflictSystemTests.cs) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `RebelBranchCatalog` | catalog | `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (4 suites) | CORE_INTERNAL | LOW |
| `RebelBranchSave` | save DTO/codec | `Assets/Ashfall.Core/Factions/RebelBranchSave.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: RebelBranchSystemTests.cs) | CORE_INTERNAL | LOW |
| `RebelBranchSystem` | gameplay system | `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` | yes | core-internal | proven (Core) | codec | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `ReckoningSystem` | gameplay system | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `RefrigerationFermentationCatalog` | catalog | `Assets/Ashfall.Core/Narrative/RefrigerationFermentationCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: RefrigerationFermentationCatalogTests.cs) | CORE_INTERNAL | LOW |
| `RegionalTreatyCatalog` | catalog | `Assets/Ashfall.Core/Narrative/RegionalTreatyCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `RegionalTreatyHostSave` | save store | `src/Host/RegionalTreatySaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `RegionalTreatyHostSession` | host session | `src/Host/RegionalTreatyHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `RegionalTreatyPanel` | ui panel | `src/UI/RegionalTreatyPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `RegionalTreatySaveStore` | save store | `src/Host/RegionalTreatySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `RegionalTreatySystem` | gameplay system | `Assets/Ashfall.Core/RegionalTreatySystem.cs` | yes | direct | proven | RegionalTreatySaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `RelicCatalog` | catalog | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `RelicProvenanceCatalog` | catalog | `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: RelicProvenanceCatalogTests.cs) | CORE_INTERNAL | LOW |
| `ResearchAtlasPanel` | ui panel | `src/UI/ResearchAtlasPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ResearchDetailPanel` | ui panel | `src/UI/ResearchDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ResearchHostSession` | host session | `src/Host/ResearchHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ResearchPanel` | ui panel | `src/UI/ResearchPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ResearchSave` | host session | `src/Host/ResearchHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | none | LIVE_GODOT | LOW |
| `ResearchSystem` | gameplay system | `Assets/Ashfall.Core/Research/ResearchSystem.cs` | yes | direct | proven | system (CaptureState) | Core tests (8 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `RespiratoryDegenerationSystem` | gameplay system | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `RopeMakingCordageCatalog` | catalog | `Assets/Ashfall.Core/Narrative/RopeMakingCordageCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: RopeMakingCordageCatalogTests.cs) | CORE_INTERNAL | LOW |
| `SafeCrackingSystem` | gameplay system | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: SafeCrackingSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `SaltMineExtractionSystem` | gameplay system | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: SaltMineExtractionSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `SaveChecksum` | save DTO/codec | `Assets/Ashfall.Core/SaveChecksum.cs` | yes | indirect | proven | none | Core tests (48 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SaveLoadHostSession` | host session | `src/Host/SaveLoadHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SaveLoadPanel` | save DTO/codec | `src/UI/SaveLoadPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ScavengerGuildSystem` | gameplay system | `Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: MusterCurrentSystemsTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `SeedBankPreservationCatalog` | catalog | `Assets/Ashfall.Core/Narrative/SeedBankPreservationCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: SeedBankPreservationCatalogTests.cs) | CORE_INTERNAL | LOW |
| `SettingsPanel` | ui panel | `src/UI/SettingsPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterAssignmentHostSession` | host session | `src/Host/ShelterAssignmentHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterAssignmentSave` | save DTO/codec | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ShelterAssignmentSaveStore` | host session | `src/Host/ShelterAssignmentHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ShelterAssignmentSystem` | gameplay system | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | yes | direct | proven | ShelterAssignmentSaveStore | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ShelterDetailPanel` | ui panel | `src/UI/ShelterDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterEncounterSystem` | gameplay system | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ShelterFireHazardSystem` | gameplay system | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: ShelterFireHazardSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `ShelterHistoryPanel` | ui panel | `src/UI/ShelterHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterHudPanel` | ui panel | `src/UI/ShelterHudPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterPanel` | ui panel | `src/UI/ShelterPanel.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterRoomSave` | save DTO/codec | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ShelterScheduleHostSave` | save store | `src/Host/ShelterScheduleSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterScheduleHostSession` | host session | `src/Host/ShelterScheduleHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterSchedulePanel` | ui panel | `src/UI/ShelterSchedulePanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterScheduleSaveStore` | save store | `src/Host/ShelterScheduleSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ShelterScheduleSystem` | gameplay system | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | yes | direct | proven | ShelterScheduleSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `ShelterThermalHostSave` | save store | `src/Host/ShelterThermalSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `ShelterThermalHostSession` | host session | `src/Host/ShelterThermalHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterThermalPanel` | ui panel | `src/UI/ShelterThermalPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `ShelterThermalSaveStore` | save store | `src/Host/ShelterThermalSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `ShelterThermalSystem` | gameplay system | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | yes | direct | proven | ShelterThermalSaveStore | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SickListSystem` | gameplay system | `Assets/Ashfall.Core/SickListSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SignalIntelligenceCatalog` | catalog | `Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: SignalIntelligenceCatalogTests.cs) | CORE_INTERNAL | LOW |
| `SignalTriangulationSystem` | gameplay system | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SilentFoundryCatalog` | catalog | `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs` | yes | indirect | proven | none | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SilentFoundryConsequencePolicyCatalog` | catalog | `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs` | yes | indirect | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SilentFoundryHostSession` | host session | `src/Foundry/SilentFoundryHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SilentFoundryPanel` | ui panel | `src/UI/SilentFoundryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SilentFoundrySaveStore` | save store | `src/Host/SilentFoundrySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `SilentFoundrySystem` | gameplay system | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.TreatyLabor.cs` | yes | direct | proven | SilentFoundrySaveStore | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SiteEncounterSystem` | gameplay system | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SkillAtrophySystem` | gameplay system | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: SkillProgressionSystemTests.cs) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `SkillMatrixPanel` | ui panel | `src/UI/SkillMatrixPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SkillProgressionSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (7 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SkyLayerArmorSystem` | gameplay system | `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (4 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SlotSave` | save DTO/codec | `Assets/Ashfall.Core/Inventory/Inventory.cs` | yes | core-internal | proven (Core) | system (CaptureState) | none | CORE_INTERNAL | LOW |
| `SoapSaponificationCatalog` | catalog | `Assets/Ashfall.Core/Narrative/SoapSaponificationCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: SoapSaponificationCatalogTests.cs) | CORE_INTERNAL | LOW |
| `SomaticFlashbackSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `StandingRecordAtlasPanel` | ui panel | `src/UI/StandingRecordAtlasPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `StandingRecordCatalog` | catalog | `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: StandingRecordSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `StandingRecordEngine` | gameplay system | `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: StandingRecordEngineTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `StandingRecordHostSession` | host session | `src/Host/StandingRecordHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `StandingRecordPanel` | ui panel | `src/UI/StandingRecordPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `StandingRecordSave` | host session | `src/Host/StandingRecordHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | none | LIVE_GODOT | LOW |
| `StartingLevelHostSession` | host session | `src/Host/StartingLevelHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `StartingLevelSaveStore` | host session | `src/Host/StartingLevelHostSession.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `StartingLevelSystem` | gameplay system | `Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs` | yes | direct | proven | StartingLevelSaveStore | Core tests (11 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `StatusPanel` | ui panel | `src/UI/StatusPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SteamTurbinePowerCatalog` | catalog | `Assets/Ashfall.Core/Narrative/SteamTurbinePowerCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: SteamTurbinePowerCatalogTests.cs) | CORE_INTERNAL | LOW |
| `StructuralFortificationCatalog` | catalog | `Assets/Ashfall.Core/Narrative/StructuralFortificationCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: StructuralFortificationCatalogTests.cs) | CORE_INTERNAL | LOW |
| `SumpFloodingHostSave` | host session | `src/Host/SumpFloodingHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Core tests (2 suites) | LIVE_GODOT | LOW |
| `SumpFloodingHostSession` | host session | `src/Host/SumpFloodingHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `SumpFloodingPanel` | ui panel | `src/UI/SumpFloodingPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SumpFloodingSaveStore` | host session | `src/Host/SumpFloodingHostSession.cs` | no (host) | direct (host) | proven | store (self) | Core tests (1 suites: ExpandedShelterSavePersistenceTests.cs) | LIVE_GODOT | LOW |
| `SumpFloodingSystem` | gameplay system | `Assets/Ashfall.Core/SumpFloodingSystem.cs` | yes | direct | proven | SumpFloodingSaveStore | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SurvivalDetailPanel` | ui panel | `src/UI/SurvivalDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SurvivalWorkstationPanel` | ui panel | `src/UI/SurvivalWorkstationPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SurvivorCatalog` | catalog | `Assets/Ashfall.Core/Narrative/SurvivorCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: SurvivorRosterSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `SurvivorDetailPanel` | ui panel | `src/UI/SurvivorDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SurvivorInspectionHostSession` | host session | `Assets/Ashfall.Core/Survivors/SurvivorInspectionHostSession.cs` | yes | direct (host) | proven | none | Core tests (1 suites: SurvivorInspectionHostSessionTests.cs) | LIVE_CORE | LOW |
| `SurvivorLetterCatalog` | catalog | `Assets/Ashfall.Core/Narrative/SurvivorLetterCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: SurvivorLetterCatalogTests.cs) | CORE_INTERNAL | LOW |
| `SurvivorRelationsHostSave` | save store | `src/Host/SurvivorRelationsSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `SurvivorRelationsHostSession` | host session | `src/Host/SurvivorRelationsHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `SurvivorRelationsPanel` | ui panel | `src/UI/SurvivorRelationsPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SurvivorRelationsSaveStore` | save store | `src/Host/SurvivorRelationsSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `SurvivorRelationsSystem` | gameplay system | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` | yes | direct | proven | SurvivorRelationsSaveStore | Core tests (6 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `SurvivorRosterSystem` | catalog | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: SurvivorRosterSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `SurvivorsHostSave` | save store | `src/Host/SurvivorsSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SurvivorsHostSession` | host session | `src/Host/SurvivorsHostSession.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: NeedsRadiationSystemTests.cs) | LIVE_GODOT | MEDIUM — authority projected from Needs/Radiation |
| `SurvivorsPanel` | ui panel | `src/UI/SurvivorsPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `SurvivorsSaveStore` | save store | `src/Host/SurvivorsSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `TacticalCombatSystem` | gameplay system | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `TanningLeatherCatalog` | catalog | `Assets/Ashfall.Core/Narrative/TanningLeatherCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: TanningLeatherCatalogTests.cs) | CORE_INTERNAL | LOW |
| `TanningLeatherworkCatalog` | catalog | `Assets/Ashfall.Core/Narrative/TanningLeatherworkCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: TanningLeatherworkCatalogTests.cs) | CORE_INTERNAL | LOW |
| `TextileSpinningWeavingCatalog` | catalog | `Assets/Ashfall.Core/Narrative/TextileSpinningWeavingCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: TextileSpinningWeavingCatalogTests.cs) | CORE_INTERNAL | LOW |
| `ThirdonaryHostSession` | host session | `src/Host/ThirdonaryHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `ThirdonaryQuestSystem` | gameplay system | `Assets/Ashfall.Core/Thirdonary/ThirdonaryQuestSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: ThirdonaryQuestSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `ThirdonarySaveStore` | save store | `src/Host/ThirdonarySaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `TimberCarpentryCatalog` | catalog | `Assets/Ashfall.Core/Narrative/TimberCarpentryCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: TimberCarpentryCatalogTests.cs) | CORE_INTERNAL | LOW |
| `TimekeepingHorologyCatalog` | catalog | `Assets/Ashfall.Core/Narrative/TimekeepingHorologyCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: TimekeepingHorologyCatalogTests.cs) | CORE_INTERNAL | LOW |
| `TradeCaravanCatalog` | catalog | `Assets/Ashfall.Core/Narrative/TradeCaravanCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: TradeCaravanCatalogTests.cs) | CORE_INTERNAL | LOW |
| `TradeDetailPanel` | ui panel | `src/UI/TradeDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `TradeScreenGodotPanel` | ui panel | `src/Economy/TradeScreenGodotPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `TradeSpecialtySystem` | gameplay system | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: TradeSpecialtySystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `TradeTellEngine` | gameplay system | `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` | yes | core-internal | proven (Core) | none | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `TraumaBondSystem` | gameplay system | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: TraumaBondSystemTests.cs) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `TravelingCaravanHostSession` | host session | `src/Host/TravelingCaravanHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `TravelingCaravanPanel` | ui panel | `src/UI/TravelingCaravanPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `TravelingCaravanSystem` | gameplay system | `Assets/Ashfall.Core/TravelingCaravanSystem.cs` | yes | direct | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `TriangulationPanel` | ui panel | `src/UI/TriangulationPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `TutorialPanel` | ui panel | `src/UI/TutorialPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `UndergroundFungiCatalog` | catalog | `Assets/Ashfall.Core/Narrative/UndergroundFungiCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: UndergroundFungiCatalogTests.cs) | CORE_INTERNAL | LOW |
| `UtilityAiHostSession` | host session | `src/Host/UtilityAiHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `UtilityAiPanel` | ui panel | `src/UtilityAI/UtilityAiPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `UtilityAiSystem` | gameplay system | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` | yes | direct | proven | none | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `VehicleCatalog` | catalog | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW |
| `VentilationSystem` | gameplay system | `Assets/Ashfall.Core/VentilationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (5 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `VerdictDashboardPanel` | ui panel | `src/UI/VerdictDashboardPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `VerdictHostSession` | host session | `src/Host/VerdictHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `VerdictNpcSystem` | gameplay system | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `VerdictPanel` | ui panel | `src/VerdictPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `VerdictRadioSystem` | gameplay system | `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: VerdictRadioSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `VerdictSave` | save DTO/codec | `Assets/Ashfall.Core/Verdict/VerdictSave.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `VerdictSaveStore` | save store | `src/Host/VerdictSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `VerdictSystem` | gameplay system | `Assets/Ashfall.Core/VerdictSystem.cs` | yes | direct | proven | VerdictSaveStore | none | LIVE_CORE + LIVE_GODOT | LOW |
| `VinylMoraleHostSave` | save store | `src/Host/VinylMoraleSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `VinylMoraleHostSession` | host session | `src/Host/VinylMoraleHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `VinylMoralePanel` | ui panel | `src/UI/VinylMoralePanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `VinylMoraleSaveStore` | save store | `src/Host/VinylMoraleSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `VinylMoraleSystem` | gameplay system | `Assets/Ashfall.Core/VinylMoraleSystem.cs` | yes | direct | proven | VinylMoraleSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `VinylRecordCatalog` | catalog | `Assets/Ashfall.Core/Narrative/VinylRecordCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: VinylRecordCatalogTests.cs) | CORE_INTERNAL | LOW |
| `VoluntaryRegisterSystem` | gameplay system | `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `VouchAccessSystem` | gameplay system | `Assets/Ashfall.Core/VouchAccessSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (6 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `WarlordDoctrineCatalog` | catalog | `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: WarlordDoctrineTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `WarlordDoctrineSystem` | gameplay system | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `WastelandBestiaryCatalog` | catalog | `Assets/Ashfall.Core/Narrative/WastelandBestiaryCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: WastelandBestiaryCatalogTests.cs) | CORE_INTERNAL | LOW |
| `WastelandCartographyCatalog` | catalog | `Assets/Ashfall.Core/Narrative/WastelandCartographyCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: WastelandCartographyCatalogTests.cs) | CORE_INTERNAL | LOW |
| `WastelandExpeditionCatalog` | catalog | `Assets/Ashfall.Core/Narrative/WastelandExpeditionCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: WastelandExpeditionCatalogTests.cs) | CORE_INTERNAL | LOW |
| `WastelandGazetteerCatalog` | catalog | `Assets/Ashfall.Core/Narrative/WastelandGazetteerCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: WastelandGazetteerCatalogTests.cs) | CORE_INTERNAL | LOW |
| `WastelandMapSaveStore` | save store | `src/Host/WastelandMapSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `WastelandMapSystem` | gameplay system | `Assets/Ashfall.Core/World/WastelandMapSystem.cs` | yes | indirect | proven | WastelandMapSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `WaterTreatmentHostSave` | save store | `src/Host/WaterTreatmentSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: ExpandedShelterSaveChecksumTests.cs) | LIVE_GODOT | LOW |
| `WaterTreatmentHostSession` | host session | `src/Host/WaterTreatmentHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `WaterTreatmentPanel` | ui panel | `src/UI/WaterTreatmentPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WaterTreatmentPotableCatalog` | catalog | `Assets/Ashfall.Core/Narrative/WaterTreatmentPotableCatalog.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: WaterTreatmentPotableCatalogTests.cs) | CORE_INTERNAL | LOW |
| `WaterTreatmentSaveStore` | save store | `src/Host/WaterTreatmentSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `WaterTreatmentSystem` | gameplay system | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | yes | direct | proven | WaterTreatmentSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `WaystationHostSave` | save store | `src/Host/WaystationSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WaystationHostSession` | host session | `src/Host/WaystationHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `WaystationNetworkPanel` | ui panel | `src/UI/WaystationNetworkPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WaystationSaveStore` | save store | `src/Host/WaystationSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `WaystationSystem` | gameplay system | `Assets/Ashfall.Core/WaystationSystem.cs` | yes | direct | proven | WaystationSaveStore | Core tests (10 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `WeaponConditionSystem` | gameplay system | `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs` | yes | core-internal | proven (Core) | none | Core tests (1 suites: CombatWeaponConditionTests.cs) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `WeatherDetailPanel` | ui panel | `src/UI/WeatherDetailPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WeatherForecastPanel` | ui panel | `src/UI/WeatherForecastPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WeatherHistoryPanel` | ui panel | `src/UI/WeatherHistoryPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WeatherHostSave` | save store | `src/Host/WeatherSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: BareSaveStoreSealTests.cs) | LIVE_GODOT | LOW |
| `WeatherHostSession` | host session | `src/Host/WeatherHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `WeatherPanel` | ui panel | `src/UI/WeatherPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WeatherSaveStore` | save store | `src/Host/WeatherSaveStore.cs` | no (host) | direct (host) | proven | store (self) | Core tests (1 suites: BareSaveStoreSealTests.cs) | LIVE_GODOT | LOW |
| `WeatherSondePanel` | ui panel | `src/UI/WeatherSondePanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WeatherSondeSystem` | gameplay system | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | yes | direct | proven | system (CaptureState) | Core tests (1 suites: WeatherSondeSystemTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `WeatherStationSystem` | gameplay system | `Assets/Ashfall.Core/WeatherStationSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `WeatherSystem` | gameplay system | `Assets/Ashfall.Core/World/WeatherSystem.cs` | yes | direct | proven | WeatherSaveStore | Core tests (7 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `WeightOfChoicesSave` | save DTO/codec | `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (1 suites: WeightOfChoicesSaveTests.cs) | CORE_INTERNAL | LOW |
| `WildlifeMigrationSystem` | gameplay system | `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: WorldSaveablesTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `WildlifeTrappingHostSave` | save store | `src/Host/WildlifeTrappingSaveStore.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WildlifeTrappingHostSession` | host session | `src/Host/WildlifeTrappingHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `WildlifeTrappingPanel` | ui panel | `src/UI/WildlifeTrappingPanel.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `WildlifeTrappingSaveStore` | save store | `src/Host/WildlifeTrappingSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `WildlifeTrappingSystem` | gameplay system | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | yes | direct | proven | WildlifeTrappingSaveStore | Core tests (3 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `WireConfessionCatalog` | catalog | `Assets/Ashfall.Core/Narrative/WireConfessionCatalog.cs` | yes | core-internal | proven (Core) | none | CatalogIntegrityValidator (129 catalogs) | CORE_INTERNAL | LOW |
| `WitnessCatalog` | catalog | `Assets/Ashfall.Core/Narrative/WitnessCatalog.cs` | yes | indirect | proven | none | Core tests (1 suites: MusterContentCatalogTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `WorkshopReverseEngineeringSystem` | gameplay system | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | yes | core-internal | proven (Core) | system (CaptureState) | Core tests (2 suites) | CORE_INTERNAL | LOW — reconciled as CORE_INTERNAL / DIRECT_HOSTED |
| `WorldHostSave` | save store | `src/Host/WorldSaveStore.cs` | no (host) | direct (host) | proven | none | Core tests (1 suites: SaveStoreChecksumSweepTests.cs) | LIVE_GODOT | LOW |
| `WorldHostSession` | host session | `src/Host/WorldHostSession.cs` | no (host) | direct (host) | proven | system (CaptureState) | Godot headless selftests | LIVE_GODOT | LOW |
| `WorldSaveStore` | save store | `src/Host/WorldSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `WorldSystem` | gameplay system | `Assets/Ashfall.Core/WorldSystem.cs` | yes | direct | proven | WorldSaveStore | none | LIVE_CORE + LIVE_GODOT | LOW |
| `YearOfAshDeepFreezeSystem` | gameplay system | `Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (7 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `YearOfAshHostSession` | host session | `src/YearOfAsh/YearOfAshHostSession.cs` | no (host) | direct (host) | proven | none | Godot headless selftests | LIVE_GODOT | LOW |
| `YearOfAshRadonSystem` | gameplay system | `Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (1 suites: YearOfAshTests.cs) | LIVE_CORE + LIVE_GODOT | LOW |
| `YearOfAshSave` | save DTO/codec | `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
| `YearOfAshSaveStore` | save store | `src/YearOfAsh/YearOfAshSaveStore.cs` | no (host) | direct (host) | proven | store (self) | none | LIVE_GODOT | LOW |
| `YearOfAshTimelineSystem` | gameplay system | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` | yes | indirect | proven | system (CaptureState) | Core tests (2 suites) | LIVE_CORE + LIVE_GODOT | LOW |
