# PLAN-ORPHAN-SEAL-01 — Appendix AK: Unreachable Blob Inventory

**Generated:** 2026-09-21 (closure re-run). The 99-authority list counts only
suffix-named types. The **blob** — unreachable files without an authority
suffix — is larger: **147 files**, and they compile into the game
(Appendix AL). Most are loaders, DTOs, catalogs, and partial classes that
support authority systems.
**Use:** a seal package that wires its authority will often need a blob file
(loader, DTO). This appendix is the lookup table; it also identifies blob
families with no authority owner — candidates for retirement review, not
automatic deletion.

**Blob by directory:**

| Directory | Files |
|---|---:|
| `Narrative` | 48 |
| `World` | 10 |
| `Survivors` | 8 |
| `Ashfall.Core` | 7 |
| `Shelter` | 6 |
| `Expeditions` | 5 |
| `Economy` | 4 |
| `Medical` | 4 |
| `Radio` | 4 |
| `Endgame` | 4 |
| `Factions` | 4 |
| `Radiation` | 3 |
| `UI` | 3 |
| `NarrativeConsequence` | 3 |
| `Research` | 2 |
| `Audio` | 2 |
| `PlayerCommand` | 2 |
| `Collectibles` | 2 |
| `Orchestration` | 2 |
| `Crafting` | 1 |
| `Inventory` | 1 |
| `Encounters` | 1 |
| `Maritime` | 1 |
| `Crossing` | 1 |
| `Foundry` | 1 |
| `Warlords` | 1 |
| `Quests` | 1 |
| `Campaign` | 1 |
| `Journeys` | 1 |
| `Localization` | 1 |

**Representative blob files (by directory):**

- **Audio**: `CassetteSetCatalogLoader.cs`, `ScarcityAudioStateMachine.cs`
- **Balance**: `ResourceMassBalanceSimulator.cs`
- **Campaign**: `SliceScenario.cs`
- **Ashfall.Core**: `CatalogIntegrityCheckers.cs`, `DebtBountyRecord.cs`, `InfrastructureHeadlessDemo.cs`, `IsExternalInit.cs`, `RegionalTreatyFeed.cs`, `SaveWireContract.cs`
- **Collectibles**: `CollectibleMapProjector.cs`, `CollectibleTutorialTracker.cs`
- **Commitments**: `CommitmentReadModel.cs`
- **Crafting**: `CraftContext.cs`
- **Crossing**: `CrossingThirdonaryIntegration.cs`
- **Difficulty**: `DifficultyConsequenceWeave.cs`
- **Economy**: `CaravanAtomicTrader.cs`, `TradeRouteContract.cs`, `TradeScreenScenarios.cs`, `UndergroundEconomyPressure.cs`
- **Encounters**: `OrphanKnockWhitelist.cs`
- **Endgame**: `CrossRunProfileStore.cs`, `EndgameHeadlessDemo.cs`, `EpilogueChronicleCatalog.cs`, `UnifiedEndingResolver.cs`
- **Expeditions**: `ExpeditionLootReferenceResolver.cs`, `ExpeditionLootValidator.cs`, `RadarEcmCatalog.cs`, `ReconTelemetryHeadlessDemo.cs`, `VerticalAscentCatalog.cs`
- **Factions**: `IndependentBranchSave.cs`, `MilitaryBranchSave.cs`, `PrpfSave.cs`, `RebelBranchSave.cs`
- **Foundry**: `FoundryActionSurface.cs`
- **Governance**: `StandingGateRegistry.cs`
- **Hygiene**: `RepositoryClassificationPolicy.cs`
- **Inventory**: `InventoryProvenance.cs`
- **Journeys**: `JourneyExecutionContext.cs`
- **Launch**: `StoreCapabilityManifest.cs`
- **Localization**: `StringFreezePolicy.cs`
- **Maritime**: `BlackFlotillaStanding.cs`
- **Medical**: `AfflictionQuestWorkBridge.cs`, `RehabilitationSlateProjection.cs`, `SurvivorBodyPresentationSlate.cs`, `SurvivorBodyState.cs`
- **Narrative**: `ApicultureBeeCatalog.cs`, `CandleMakingWaxCatalog.cs`, `CeramicsKilnCatalog.cs`, `CharcoalPyrolysisCatalog.cs`, `CourierDispatchCatalog.cs`, `CrucibleFoundryCatalog.cs`
- **NarrativeConsequence**: `NarrativeConsequenceGraph.cs`, `NarrativeSimulator.cs`, `NarrativeValidator.cs`
- **Orchestration**: `BootstrapLifecycleGate.cs`, `LedgerTruthIntegrityGate.cs`
- **Performance**: `PerfTestMarker.cs`
- **Phantoms**: `ConfessionSecretCatalog.cs`
- **PlayerCommand**: `CampaignActionLog.cs`, `CommandContext.cs`
- **Ports**: `PortContract.cs`
- **Presentation**: `HoldfastPresentationSlate.cs`
- **Production**: `IOutputSink.cs`
- **Quests**: `PersonalQuestHeadlessDemo.cs`
- **Radiation**: `Dosimeter.cs`, `RadiationEconomyBridge.cs`, `RadiationSocialBridge.cs`
- **Radio**: `AcousticDirectionFindingCatalog.cs`, `PatrolRadioHooks.cs`, `RescuedArcProjection.cs`, `SignalTrustAvailability.cs`
- **Records**: `RetentionPolicy.cs`
- **Research**: `KnowledgeAcquisitionSource.cs`, `ResearchUnlockBridge.cs`
- **Shelter**: `CellulosicBiofuelCatalog.cs`, `CupolaFoundryCatalog.cs`, `FogHarvestingCatalog.cs`, `GeothermalAquiferHeadlessDemo.cs`, `PrecisionBroachingCatalog.cs`, `WallCarvingCatalog.cs`
- **Survivors**: `GenealogyBridge.cs`, `GuiltSourceCatalog.cs`, `IdeologicalFrictionEvents.cs`, `MemorialComponentAdapter.cs`, `MemorialComponentParity.cs`, `MemorialComponentStore.cs`
- **Telemetry**: `PlaySessionRecorder.cs`
- **UI**: `CollectiblePresentationModel.cs`, `PlayerSurfaceContract.cs`, `PlayerSurfaceManifest.cs`
- **Warlords**: `WarlordResponseActions.cs`
- **World**: `DebtRouteAccessResolver.cs`, `FactionTerritoryCatalog.cs`, `MapRouteHazardEvaluator.cs`, `RouteAvailabilityKind.cs`, `RouteAvailabilityPresentation.cs`, `RouteRegionTopology.cs`
