# Core Port Contracts and Host Wiring Manifest

> **Plan 36 / C2[13] Authority:** Machine-declared integration seams, host wiring expectations, and CI gate.

## Summary Metrics

- **Total integration seams:** 291
- **Host-required (`HOST_REQUIRED`):** 186 (all verified called from `src/`)
- **Optional host ports (`OPTIONAL_HOST`):** 7
- **Live via Core (`LIVE_VIA_CORE`):** 54
- **Test/Diagnostic only (`TEST_ONLY`):** 44
- **Pure library utilities (`PURE_LIBRARY`):** 0
- **Deferred / Exemptions (`DEFERRED`):** 0 (shrink-only ratchet with dated owner)
- **Unbound production-required seams:** 0

## Taxonomy & Classification Rules

| Classification | Requirement | Verification Invariant |
|---|---|---|
| `HOST_REQUIRED` | Production-required | Must have >= 1 caller in `src/`. Missing caller fails fast CI gate. |
| `OPTIONAL_HOST` | Host capability | Null-safe / optional adapter in host. |
| `LIVE_VIA_CORE` | Domain call chain | Invoked internally within `Assets/Ashfall.Core`. |
| `TEST_ONLY` | Test harness | Verified in `Ashfall.Core.Tests/` or diagnostic runner. |
| `PURE_LIBRARY` | Engine-free math/utility | Pure functional or data transformation logic. |
| `DEFERRED` | Formal debt ratchet | Explicit expiry date and activation condition. Caller in `src/` triggers upgrade failure. |

## Active Seam Directory

| Seam | Owner | Classification | Callers in `src/` | Status | Reason / Activation |
|---|---|---|---:|---|---|
| `AeroponicsSystem.RegisterProfile` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in AeroponicsSystem. |
| `AmputationSystem.RegisterProcedure` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in AmputationSystem. |
| `AnomalyHazardSystem.BindCatalog` | world | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in AnomalyHazardSystem. |
| `ApprenticeshipSystem.RegisterMentorship` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ApprenticeshipSystem. |
| `ApprenticeshipSystem.RegisterWill` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ApprenticeshipSystem. |
| `AquiferPiezometerEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in AquiferPiezometerEngine. |
| `AquiferPiezometerEngine.BindInventory` | shelter | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in AquiferPiezometerEngine. |
| `ArchaeologySystem.RegisterArchive` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in ArchaeologySystem. |
| `AviationSystem.RegisterAircraft` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in AviationSystem. |
| `BallisticsWorkbenchSystem.RegisterDefinition` | combat | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in BallisticsWorkbenchSystem. |
| `BioFermentationEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in BioFermentationEngine. |
| `BionicsSystem.BindInventory` | medical | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in BionicsSystem. |
| `BlackFlotillaStanding.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in BlackFlotillaStanding. |
| `BlackMarketSystem.BindCatalog` | economy | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in BlackMarketSystem. |
| `BlackMarketSystem.BindFactionBountySystem` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in BlackMarketSystem. |
| `BlackMarketSystem.BindMarket` | economy | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in BlackMarketSystem. |
| `BootstrapLifecycleGate.RegisterSubsystem` | orchestration | `TEST_ONLY` | 0 | 🧪 TEST | EN-06 one-bootstrap-path lifecycle verification read model; consumed by BootstrapLifecycleGateTests only, no production caller. |
| `CampaignCalendar.BindProfile` | campaign | `HOST_REQUIRED` | 4 | ✅ BOUND | Integration seam in CampaignCalendar. |
| `CampaignDayCoordinator.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in CampaignDayCoordinator. |
| `CarbonCompositeEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in CarbonCompositeEngine. |
| `CarbonCompositeEngine.BindInventory` | shelter | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in CarbonCompositeEngine. |
| `CargoAirdropSystem.BindCatalog` | world | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in CargoAirdropSystem. |
| `CartographySystem.RegisterRegion` | exploration | `TEST_ONLY` | 0 | 🧪 TEST | Region registration is Core-test exercised; no production host caller yet. |
| `CatalogBootValidator.RegisterCatalog` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in CatalogEntry. |
| `CatalogDiagnostics.RegisterLog` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in CatalogDiagnostics. |
| `CatalogIntegrityDefinitionChecker.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in CatalogIntegrityDefinitionChecker. |
| `CatalogIntegrityDefinitionChecker.RegisterOrReference` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in CatalogIntegrityDefinitionChecker. |
| `ChildDevelopmentSystem.RegisterChild` | survivors | `TEST_ONLY` | 0 | 🧪 TEST | Child registration is exercised in Core tests; no production host caller yet. |
| `CombatCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in CombatCatalog. |
| `CombatPerks.RegisterSurvivor` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in CombatPerks. |
| `CombatTraumaSystem.RegisterSurvivor` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in CombatTraumaSystem. |
| `CommitmentSystem.RegisterCommitment` | commitments | `LIVE_VIA_CORE` | 3 | ✅ BOUND | Integration seam in CommitmentSystem. |
| `CompanionAnimalSystem.BindFoodPort` | ecology | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in CompanionAnimalSystem. |
| `CompanionAnimalSystem.RegisterCompanion` | ecology | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in CompanionAnimalSystem. |
| `CounterIntelligenceSystem.RegisterProfile` | factions | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in CounterIntelligenceSystem. |
| `CraftingSystem.BindCraftResultGate` | crafting | `HOST_REQUIRED` | 1 | ✅ BOUND | CraftingHostSession binds the gate to ItemCatalog.Contains so unknown result ids cannot be crafted. |
| `CraftingSystem.BindResearchGate` | crafting | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in CraftingSystem. |
| `CrisisPresentationCoordinator.Bind` | core-architecture | `HOST_REQUIRED` | 295 | ✅ BOUND | Integration seam in CrisisPresentationCoordinator. |
| `CrossingQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in CrossingQuestSystem. |
| `CrossingQuestSystem.BindConsequenceLedger` | crossing | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in CrossingQuestSystem. |
| `CrossingQuestSystem.BindMoralSystem` | crossing | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in CrossingQuestSystem. |
| `CryoVaultSystem.RegisterSample` | shelter | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in CryoVaultSystem. |
| `CryogenicAirSeparationSystem.ConfigureProducts` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in CryogenicAirSeparationSystem. |
| `CvdDiamondSynthesisEngine.RegisterConsumer` | shelter | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in CvdDiamondSynthesisEngine. |
| `DamagedMapSystem.RegisterFragment` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in DamagedMapSystem. |
| `DamagedMapSystem.RegisteredCount` | core-architecture | `TEST_ONLY` | 1 | ✅ BOUND | Integration seam in DamagedMapSystem. |
| `DesperationSystem.RegisterCorpse` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in DesperationSystem. |
| `DesperationSystem.RegisterEvent` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in DesperationSystem. |
| `DiseaseAfflictionHandler.RegisterAll` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in DiseaseAfflictionHandler. |
| `DiseaseProtocolHandler.RegisterAll` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in DiseaseProtocolHandler. |
| `DiseaseSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in DiseaseSystem. |
| `DiseaseSystem.RegisterStrain` | disease | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in DiseaseSystem. |
| `DiscoveryConsequenceSystem.RegisterDiscovery` | expeditions | `TEST_ONLY` | 0 | 🧪 TEST | Discovery registration is Core-test exercised; no production host caller yet. |
| `DiscoveryConsequenceSystem.RegisterExpeditionDiscovery` | expeditions | `HOST_REQUIRED` | 1 | ✅ BOUND | Main.Expeditions records completed expedition discoveries into consequence state. |
| `DistressFollowUpScheduler.BindToMissionEvents` | radio | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in DistressFollowUpScheduler. |
| `DoorEncounterSystem.RegisterEncounter` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in DoorEncounterSystem. |
| `DosimeterCalibrationSystem.RegisterDevice` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in DosimeterCalibrationSystem. |
| `DutyRosterChartEngine.Bind` | core-architecture | `HOST_REQUIRED` | 295 | ✅ BOUND | Integration seam in DutyRosterChartEngine. |
| `DutyRosterOverflowEngine.Bind` | core-architecture | `HOST_REQUIRED` | 295 | ✅ BOUND | Integration seam in DutyRosterOverflowEngine. |
| `DutyRosterOverflowEngine.RegisterOverflowVisit` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in DutyRosterOverflowEngine. |
| `DutyRosterQuestRuntime.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in DutyRosterQuestRuntime. |
| `DutyRosterSystem.RegisterBlankRowsLivingName` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in DutyRosterSystem. |
| `DutyRosterSystem.RegisterOverflowVisit` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in DutyRosterSystem. |
| `DynamicQuestGenerator.RegisterTemplate` | quests | `TEST_ONLY` | 1 | ✅ BOUND | Template registration is Core-side; mercenary host calls a different RegisterTemplate. |
| `EbPvdCoatingEngine.RegisterCoating` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EbPvdCoatingEngine. |
| `EchoSystem.RegisterRange` | narrative | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in EchoSystem. |
| `EquipmentConditionSystem.RegisterItem` | core-architecture | `HOST_REQUIRED` | 4 | ✅ BOUND | Integration seam in EquipmentConditionSystem. |
| `EquipmentConditionSystem.RegisterProfile` | equipment | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by EquipmentConditionSystem default-profile construction and LoadProfiles catalog ingestion. |
| `EspionageConsequenceRouter.BindConsumers` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageConsequenceRouter. |
| `EspionageSystem.BindAgentAvailability` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EspionageSystem.BindAgentCapability` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EspionageSystem.BindFactionResolver` | factions | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional override hook; EspionageSystem already defaults to FactionStandingIdResolver.ToSystemsId when no host resolver is supplied. |
| `EspionageSystem.BindResearchGate` | factions | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EspionageSystem.BindRng` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EvidenceLedger.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in EvidenceLedger. |
| `ExpansionQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in ExpansionQuestSystem. |
| `ExpeditionDefinitionRegistry.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in ExpeditionDefinitionRegistry. |
| `ExpeditionLootReferenceResolver.RegisterCategory` | expeditions | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional resolver extension; canonical/default category sets are already accepted through the resolver constructor. |
| `ExpeditionLootReferenceResolver.RegisterItem` | core-architecture | `HOST_REQUIRED` | 4 | ✅ BOUND | Integration seam in ExpeditionLootReferenceResolver. |
| `ExpeditionNavalSystem.RegisterVessel` | expeditions | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by ExpeditionNavalSystem default-vessel construction and LoadCatalog ingestion. |
| `FactionRadioEngine.RegisterChannel` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FactionRadioEngine. |
| `FactionStanceEngine.RegisterFaction` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in FactionStanceEngine. |
| `FactionStanceEngine.RegisterFactions` | factions | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional batch convenience API over the production-wired RegisterFaction seam; no separate activation is required. |
| `FalloutSystem.RegisterPattern` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FalloutSystem. |
| `FeedbackMessageCatalog.RegisterTemplate` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FeedbackMessageCatalog. |
| `FinalWishSystem.RegisterWish` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FinalWishSystem. |
| `FischerTropschSynthesisEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in FischerTropschSynthesisEngine. |
| `FischerTropschSynthesisEngine.BindInventory` | shelter | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in FischerTropschSynthesisEngine. |
| `FischerTropschSynthesisEngine.RegisterLubricantConsumer` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FischerTropschSynthesisEngine. |
| `FluidLogisticsSystem.BindRng` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FluidLogisticsSystem. |
| `FluidLogisticsSystem.ConfigureSink` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in FluidLogisticsSystem. |
| `FungiCultivationSystem.RegisterCatalog` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in FungiCultivationSystem. |
| `GenerationalSuccessionEngine.RegisterDweller` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in GenerationalSuccessionEngine. |
| `GenerationalSystem.RegisterTrait` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in GenerationalSystem. |
| `GeothermalAquiferSystem.RegisterStrata` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in GeothermalAquiferSystem. |
| `GeothermalOrcSystem.RegisterStratum` | shelter | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in GeothermalOrcSystem. |
| `GrainProcessingSystem.RegisterRecipe` | core-architecture | `LIVE_VIA_CORE` | 1 | ✅ BOUND | Integration seam in GrainProcessingSystem. |
| `GrainProcessingSystem.RegisterSilo` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in GrainProcessingSystem. |
| `GroundPenetratingRadarEngine.BindCatalog` | world | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in GroundPenetratingRadarEngine. |
| `GroundPenetratingRadarEngine.BindInventory` | world | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in GroundPenetratingRadarEngine. |
| `HeliographSystem.RegisterStation` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in HeliographSystem. |
| `HoldfastFactionsCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in HoldfastFactionsCatalog. |
| `HoldfastItemsCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in HoldfastItemsCatalog. |
| `HoldfastNpcCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in HoldfastNpcCatalog. |
| `HoldfastQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in HoldfastQuestSystem. |
| `HydraulicExtrusionEngine.RegisterMachine` | foundry | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in HydraulicExtrusionEngine. |
| `IceRoadSystem.RegisterHoldfastNode` | holdfast | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Default Cut/holdfast node registration goes through this method. |
| `IdeologicalFrictionSystem.RegisterBelief` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in IdeologicalFrictionSystem. |
| `IndependentBranchCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in IndependentBranchCatalog. |
| `ItemCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in ItemCatalog. |
| `ItemCatalog.RegisterRange` | core-architecture | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in ItemCatalog. |
| `ItemDescriptionCatalog.Register` | inventory | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in ItemDescriptionCatalog. |
| `ItemDescriptionCatalog.RegisterAlias` | inventory | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ItemDescriptionCatalog. |
| `ItemLoreSystem.RegisterItem` | inventory | `TEST_ONLY` | 4 | ✅ BOUND | Item provenance registration is Core-test exercised; no production host caller yet. |
| `JournalSystem.BindAuthoredCorpus` | journal | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in JournalSystem. |
| `JournalVoice.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in JournalVoice. |
| `JusticeSystem.RegisterLaw` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in JusticeSystem. |
| `LandmarkDegradationSystem.RegisterLandmark` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in LandmarkDegradationSystem. |
| `LatentExpertAwakeningSystem.RegisterDefinition` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in LatentExpertAwakeningSystem. |
| `LocalizationService.RegisterString` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in LocalizationService. |
| `LocalizationService.RegisterTranslation` | localization | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in LocalizationService. |
| `LyophilizationSystem.RegisterMedicalProtocol` | medical | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in LyophilizationSystem. |
| `MaritimeDiveSystem.RegisterSite` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in MaritimeDiveSystem. |
| `MarketSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in MarketSystem. |
| `MarketSystem.BindCommodityCatalog` | economy | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in MarketSystem. |
| `MarketSystem.BindEmbargoSystem` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MarketSystem. |
| `MarketSystem.BindRegionalPriceAtlas` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MarketSystem. |
| `MaterialProfileCatalog.BindOutputItem` | foundry | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in MaterialProfileCatalog. |
| `MedicalPipelineCoordinator.RegisterHandler` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MedicalPipelineCoordinator. |
| `MedicalPipelineCoordinator.RegisterProtocol` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in MedicalPipelineCoordinator. |
| `MemoryDecaySystem.RegisterOrUpdate` | cognition | `TEST_ONLY` | 0 | 🧪 TEST | Memory records are registered from Core tests; no production host caller yet. |
| `MercenarySystem.RegisterTemplate` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MercenarySystem. |
| `MicrofluidicDiagnosticEngine.RegisterAssay` | medical | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MicrofluidicDiagnosticEngine. |
| `MilitaryBranchCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in MilitaryBranchCatalog. |
| `MineClearingFlailEngine.RegisterModule` | expeditions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MineClearingFlailEngine. |
| `MoralBranchingSystem.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in MoralBranchingSystem. |
| `MoralBranchingSystem.RegisterMoralChoice` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MoralBranchingSystem. |
| `MoralChoiceSystem.RegisterQuest` | moralchoice | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in MoralChoiceSystem. |
| `MoralChoiceSystem.RegisterQuests` | moralchoice | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MoralChoiceSystem. |
| `MoraleMarkSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in MoraleMarkSystem. |
| `MusterSystem.RegisterQuestline` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MusterSystem. |
| `MutationSystem.RegisterMutation` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in MutationSystem. |
| `NarrativeArcEventSystem.RegisterRange` | narrative | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in NarrativeArcEventSystem. |
| `NarrativeDiscoveryCatalog.RegisterAdapter` | narrative | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional extension hook; NarrativeDiscoveryCatalog installs its canonical default adapters in the constructor. |
| `NarrativeEncounterSystem.RegisterEncounter` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in NarrativeEncounterSystem. |
| `NarrativeEncounterSystem.RegisterRange` | core-architecture | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in NarrativeEncounterSystem. |
| `NeedsSystem.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in NeedsSystem. |
| `NpcArcCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in NpcArcCatalog. |
| `PanelDescriptor.Bind` | core-architecture | `HOST_REQUIRED` | 295 | ✅ BOUND | Integration seam in PanelDescriptor. |
| `PanelRegistry.ConfigureActions` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PanelRegistry. |
| `PanelRegistry.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in PanelRegistry. |
| `PanelRegistryBootstrap.RegisterAll` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in PanelRegistryBootstrap. |
| `PathogenStrainSystem.BindEngineHooks` | disease | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PathogenStrainSystem. |
| `PersonalBelongingsSystem.RegisterBelonging` | survivors | `TEST_ONLY` | 1 | ✅ BOUND | Belonging registration is Core-test exercised; no production host caller yet. |
| `PhantomMemoryEngine.RegisterRule` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in PhantomMemoryEngine. |
| `PhantomMemoryEngine.RegisterRuleDetailed` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PhantomMemoryEngine. |
| `PharmaLabSystem.BindSkillEvaluator` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PharmaLabSystem. |
| `PharmaLabSystem.RegisterRecipe` | core-architecture | `TEST_ONLY` | 1 | ✅ BOUND | Integration seam in PharmaLabSystem. |
| `PharmaceuticalTabletEngine.BindCatalog` | medical | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in PharmaceuticalTabletEngine. |
| `PharmaceuticalTabletEngine.BindInventory` | medical | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in PharmaceuticalTabletEngine. |
| `PlasticPyrolysisSystem.BindCatalog` | shelter | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in PlasticPyrolysisSystem. |
| `PlasticPyrolysisSystem.BindInventory` | shelter | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in PlasticPyrolysisSystem. |
| `PneumaticDispatchSystem.RegisterEndpoint` | shelter | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in PneumaticDispatchSystem. |
| `PowerGridSystem.ConfigureSurge` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PowerGridSystem. |
| `PowerGridSystem.RegisterLoadRoom` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in PowerGridSystem. |
| `PrisonerSystem.RegisterTactic` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PrisonerSystem. |
| `ProceduralItemInstance.ConfigureSequence` | inventory | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ProceduralItemInstance. |
| `QuestRuntimeCoordinator.Register` | quests | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in QuestRuntimeCoordinator. |
| `QuestlineSystem.RegisterQuestline` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in QuestlineSystem. |
| `RadiationPhaseProgression.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in RadiationPhaseProgression. |
| `RadiationSystem.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in RadiationSystem. |
| `RadioBroadcastCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in RadioBroadcastCatalog. |
| `RadioBroadcastCatalog.RegisterAuthoredGapBroadcasts` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in RadioBroadcastCatalog. |
| `RadioDistressSystem.RegisterSignal` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RadioDistressSystem. |
| `RadioStationCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in RadioStationCatalog. |
| `RailGrindingEngine.RegisterHead` | expeditions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RailGrindingEngine. |
| `RailwayInterlockEngine.BindCatalog` | expeditions | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in RailwayInterlockEngine. |
| `RailwayInterlockEngine.BindInventory` | expeditions | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in RailwayInterlockEngine. |
| `RailwaySystem.RegisterCatalog` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in RailwaySystem. |
| `RailwaySystem.RegisterLogisticsCatalog` | expeditions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RailwaySystem. |
| `RationConflictSystem.RegisterSurvivor` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RationConflictSystem. |
| `RebelBranchCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in RebelBranchCatalog. |
| `ReconTelemetrySystem.RegisterPlatform` | expeditions | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ReconTelemetrySystem. |
| `RelationshipDecaySystem.RegisterOrUpdatePair` | survivors | `TEST_ONLY` | 2 | ✅ BOUND | Pair-bond registration is Core-test exercised; no production host caller yet. |
| `ResearchSystem.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in ResearchSystem. |
| `ResourceRationingSystem.BindResourceValidator` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | EconomyHostSession binds the canonical catalog/consumer validator to rationing state. |
| `RouteInfrastructureSystem.RegisterMinefield` | world | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RouteInfrastructureSystem. |
| `RouteInfrastructureSystem.RegisterRailSegment` | world | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RouteInfrastructureSystem. |
| `RumorSystem.RegisterHub` | information-flow | `TEST_ONLY` | 2 | ✅ BOUND | Hub registration is Core-test exercised; no production host caller yet. |
| `SafeCrackingSystem.RegisterSafe` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SafeCrackingSystem. |
| `SaltMineExtractionSystem.RegisterVein` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SaltMineExtractionSystem. |
| `SanitationSystem.BindFacilityCatalog` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SanitationSystem. |
| `SeasonalEventSystem.BindDefinitions` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SeasonalEventSystem. |
| `SeismicDynamicsSystem.RegisterFault` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in SeismicDynamicsSystem. |
| `SessionLifecycleRegistry.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in SessionLifecycleRegistry. |
| `ShelterBarterSystem.RegisterCaravan` | economy | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in ShelterBarterSystem. |
| `ShelterDecorSystem.RegisterItemModifier` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in ShelterDecorSystem. |
| `ShelterNoiseSystem.RegisterRoom` | shelter | `TEST_ONLY` | 0 | 🧪 TEST | Room acoustic registration is Core-internal / test-exercised; no production host caller yet. |
| `ShelterSecuritySystem.ConfigureZone` | shelter | `TEST_ONLY` | 2 | ✅ BOUND | Security-zone configuration is Core-test exercised; no production host caller yet. |
| `ShelterRadioStationSystem.BindSkillProvider` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ShelterRadioStationSystem. |
| `ShelterRadioStationSystem.BindWeatherNoiseProvider` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in ShelterRadioStationSystem. |
| `ShelterSocialDynamicsSystem.BindMediatorSkillProvider` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ShelterSocialDynamicsSystem. |
| `ShelterSocialDynamicsSystem.RegisterSurvivorRoom` | core-architecture | `TEST_ONLY` | 1 | ✅ BOUND | Integration seam in ShelterSocialDynamicsSystem. |
| `ShelterThermalSystem.RegisterExternalBurst` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | WeatherHardeningSystem freeze bursts call this Core-to-Core; host does not invoke it directly. |
| `ShelterThermalSystem.RegisterInsulation` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ShelterThermalSystem. |
| `ShelterThermalSystem.RegisterThermalGear` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by ShelterThermalSystem default thermal-gear registration during construction. |
| `ShelterWorkshopSystem.BindWorkerSkillProvider` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ShelterWorkshopSystem. |
| `SignalTriangulationSystem.RegisterStationBaseline` | radio | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in SignalTriangulationSystem. |
| `SilentFoundrySystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindConsequencePolicy` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindGlassworksCatalog` | foundry | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindInventory` | core-architecture | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindMaterialProfiles` | foundry | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindMetallurgyCatalog` | foundry | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindTreaties` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindVentilation` | foundry | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SkillProgressionSystem.RegisterDefaultSkills` | survivors | `TEST_ONLY` | 0 | 🧪 TEST | Retained zero-op compatibility boundary; production skills load from skills.json. |
| `SkillProgressionSystem.RegisterSkill` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SkillProgressionSystem. |
| `SpiritualMeaningCoordinator.RegisterDeath` | spiritual | `HOST_REQUIRED` | 1 | ✅ BOUND | SurvivorFate.OnSurvivorFate registers mourning arcs; memorial vigil maps to ShelterVigilRiteId. |
| `StartingLevelSystem.BindMaintenance` | startinglevel | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in StartingLevelSystem. |
| `StealthSystem.RegisterCamouflageGear` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in StealthSystem. |
| `StealthSystem.RegisterWeaponNoise` | combat | `HOST_REQUIRED` | 1 | ✅ BOUND | Weapon-noise profiles feed PartyStealthState.accumulatedNoise. |
| `SubsystemManifest.RegisterSetupAction` | orchestration | `HOST_REQUIRED` | 1 | ✅ BOUND | Plan 28C host registers setup delegates for declarative subsystem bootstrap. |
| `SumpFloodingSystem.BindServices` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SumpFloodingSystem. |
| `SurvivorDowntimeSystem.RegisterHobby` | survivors | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by SurvivorDowntimeSystem default-hobby construction and LoadCatalog ingestion. |
| `SurvivorEntityStore.RegisterComponentStore` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in SurvivorEntityStore. |
| `SurvivorRosterSystem.RegisterDefinition` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SurvivorRosterSystem. |
| `SurvivorRosterSystem.RegisterRange` | core-architecture | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in SurvivorRosterSystem. |
| `SurvivorSocialCoordinator.RegisterBelief` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in SurvivorSocialCoordinator. |
| `TacticalCombatSystem.ConfigureBreachingLogistics` | combat | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in TacticalCombatSystem. |
| `ThirdonaryQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in ThirdonaryQuestSystem. |
| `TradeEmbargoSystem.RegisterRule` | economy | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in TradeEmbargoSystem. |
| `TradeSpecialtySystem.RegisterProfessionInfo` | survivors | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in TradeSpecialtySystem. |
| `TradeSpecialtySystem.RegisterProfessionPatterns` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in TradeSpecialtySystem. |
| `TradeTellEngine.RegisterBand` | economy | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by TradeTellEngine.LoadFromJson while materializing the authored trade_tell_lines.json corpus. |
| `TradeTellEngine.RegisterTellPool` | economy | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by TradeTellEngine.LoadFromJson while materializing the authored trade_tell_lines.json corpus. |
| `TrophySystem.RegisterTrophy` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in TrophySystem. |
| `TunnelNetworkSystem.RegisterJunction` | underground | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Junction registration called canonically by WastelandMapSystem.EnsureCanonicalTunnels. |
| `TunnelNetworkSystem.RegisterSegment` | underground | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Segment registration called canonically by WastelandMapSystem.EnsureCanonicalTunnels. |
| `UvCoronaDetectionEngine.BindCatalog` | radio | `HOST_REQUIRED` | 20 | ✅ BOUND | Integration seam in UvCoronaDetectionEngine. |
| `UvCoronaDetectionEngine.BindInventory` | radio | `HOST_REQUIRED` | 10 | ✅ BOUND | Integration seam in UvCoronaDetectionEngine. |
| `VehicleGarageSystem.RegisterRecoveryMission` | expeditions | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in VehicleGarageSystem. |
| `VentilationSystem.BindStageServices` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in VentilationSystem. |
| `VentilationSystem.RegisterSource` | core-architecture | `LIVE_VIA_CORE` | 1 | ✅ BOUND | Integration seam in VentilationSystem. |
| `VerdictAccusationSystem.Bind` | verdict | `HOST_REQUIRED` | 295 | ✅ BOUND | Integration seam in VerdictAccusationSystem. |
| `VerdictNpcSystem.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in VerdictNpcSystem. |
| `WastelandMapSystem.RegisterTrapSiteLocation` | world | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional runtime extension hook; authored trap-site locations are already supplied by WastelandMapCatalogLoader through the system constructor. |
| `WaterTreatmentSystem.RegisterContaminationAdvisory` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in WaterTreatmentSystem. |
| `WaystationNetworkSystem.BindShortagePolicy` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in WaystationNetworkSystem. |
| `WeatherGateCatalog.Register` | core-architecture | `HOST_REQUIRED` | 27 | ✅ BOUND | Integration seam in WeatherGateCatalog. |
| `WeatherHardeningSystem.RegisterUpgrade` | world | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in WeatherHardeningSystem. |
| `WeatherSondeSystem.BindRecoveryInventory` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in WeatherSondeSystem. |
| `WeatherSystem.BindProfile` | core-architecture | `HOST_REQUIRED` | 4 | ✅ BOUND | Integration seam in WeatherSystem. |
| `WeatherSystem.BindWeatherEffects` | world | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in WeatherSystem. |
| `WildlifeMigrationSystem.BindSeasonProfile` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in WildlifeMigrationSystem. |
| `WildlifeMigrationSystem.RegisterPack` | core-architecture | `LIVE_VIA_CORE` | 1 | ✅ BOUND | Integration seam in WildlifeMigrationSystem. |
| `WildlifeTrappingCatalog.RegisterWith` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in WildlifeTrappingCatalog. |
| `WildlifeTrappingSystem.RegisterBait` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in WildlifeTrappingSystem. |
| `WildlifeTrappingSystem.RegisterPreyDefinition` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in WildlifeTrappingSystem. |
| `WildlifeTrappingSystem.RegisterQuarry` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in WildlifeTrappingSystem. |
| `WildlifeTrappingSystem.RegisterTrapDefinition` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in WildlifeTrappingSystem. |
| `WorkshopReverseEngineeringSystem.BindSkillEvaluator` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in WorkshopReverseEngineeringSystem. |
| `WorkshopReverseEngineeringSystem.BindTechSalvageRng` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in WorkshopReverseEngineeringSystem. |
| `WorkshopReverseEngineeringSystem.RegisterRelic` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in WorkshopReverseEngineeringSystem. |
| `ZealotrySystem.RegisterLeader` | survivors | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in ZealotrySystem. |
| `CampaignLegacySystem.RegisterTrait` | legacy | `LIVE_VIA_CORE` | 2 | ✅ BOUND | Catalog-load self-registration; called by CampaignLegacySystem.LoadCatalog. |
| `ClothingWarmthSystem.RegisterProfile` | inventory | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Catalog-load self-registration; called by ClothingWarmthSystem.LoadCatalog. |
| `CookingSystem.RegisterRecipe` | cooking | `LIVE_VIA_CORE` | 1 | ✅ BOUND | Catalog-load self-registration; called by CookingSystem.LoadCatalog. |
| `DreamSystem.RegisterTemplate` | survivors | `LIVE_VIA_CORE` | 1 | ✅ BOUND | Catalog-load self-registration; called by DreamSystem.LoadCatalog. |
| `PropagandaSystem.RegisterTemplate` | propaganda | `LIVE_VIA_CORE` | 1 | ✅ BOUND | Catalog-load self-registration via LoadTemplates (host-bound 2026-09-22). |
| `RepositoryClassificationPolicy.RegisterBudget` | hygiene | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Default budget table registered in the policy constructor. |
| `ResearchUnlockBridge.RegisterUnlock` | research | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Catalog-load self-registration; called by ResearchUnlockBridge.LoadCatalog. |
| `RetentionPolicyCatalog.RegisterPolicy` | records | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Protected obligation table registered in the catalog constructor. |
| `ShelterGovernanceEngine.RegisterBlocDefinition` | governance | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Catalog-load self-registration; called by ShelterGovernanceEngine.LoadCatalog. |
| `ShelterIdentitySystem.RegisterOrigin` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Catalog-load self-registration; called by ShelterIdentitySystem.LoadCatalog. |
| `AgingSystem.RegisterSurvivor` | survivors | `TEST_ONLY` | 1 | ✅ BOUND | Plan 176 aging authority is Core-only (no host owner); seam covered by AgingSystem tests. |
| `AssetManifestCatalog.RegisterEntry` | assets | `TEST_ONLY` | 0 | 🧪 TEST | Plan 50 asset manifest registration exercised by tests; host resolution uses the catalog loader. |
| `MaritimeExplorationSystem.RegisterDiveSite` | maritime | `TEST_ONLY` | 0 | 🧪 TEST | Island system (no host/save/panel); catalog registration covered by tests only. |
| `MaritimeExplorationSystem.RegisterEquipment` | maritime | `TEST_ONLY` | 0 | 🧪 TEST | Island system (no host/save/panel); catalog registration covered by tests only. |
| `ModSupportSystem.RegisterMod` | mods | `TEST_ONLY` | 0 | 🧪 TEST | Plan 165 mod registry is Core-only; seam covered by mod contract tests. |
| `PersonalBelongingsSystem.RegisterFromTemplate` | survivors | `TEST_ONLY` | 0 | 🧪 TEST | Plan 210 island system; template registration covered by tests only. |
| `ResearchUnlockBridge.BindResearchSystem` | research | `TEST_ONLY` | 3 | ✅ BOUND | Host binding seam is not yet wired (bridge is Core-only); covered by tests. |
| `SessionDurabilityManager.RegisterOrUpdateSlot` | save | `TEST_ONLY` | 2 | ✅ BOUND | Plan 39 durability manager is Core-only; slot registration covered by tests. |
| `SurvivorEducationSystem.RegisterLearner` | education | `TEST_ONLY` | 2 | ✅ BOUND | Plan 154 education island; learner registration covered by tests only. |
| `SurvivorVoiceSystem.RegisterLine` | voice | `TEST_ONLY` | 0 | 🧪 TEST | Plan 42 voice catalog loader feeds RegisterLine; no host consumer yet. |
| `WeatherCascadeSystem.BindWeatherSource` | weather | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional source hook; TriggerCascade works without it and no caller exists yet. |
| `WeatherGameplayCascadeEngine.BindValidatedTemplates` | weather | `HOST_REQUIRED` | 1 | ✅ BOUND | Plan 135 / C2[27] validated-template binding: the host loads the authored cascade table through the strict WeatherCascadeCatalogLoader and hands the engine pre-validated rows, so no lenient vocabulary fallback can hide an authored typo inside a live cascade. |
| `RomanceCourtshipCatalog.BindValidatedEvents` | survivors | `HOST_REQUIRED` | 1 | ✅ BOUND | Plan 150 validated-courtship binding: the host loads the authored table through the strict RomanceCourtshipCatalogLoader and hands the catalog pre-validated rows, so no lenient fallback can hide an authored typo inside a live courtship event. |
| `VehicleCustomizationCatalog.BindValidatedModules` | vehicles | `HOST_REQUIRED` | 1 | ✅ BOUND | Plan 152 validated-module binding: the host loads the authored table through the strict VehicleModuleCatalogLoader and hands the catalog pre-validated rows, so no lenient fallback can hide an authored typo inside a live vehicle build. |
| `CookingSystem.BindValidatedRecipes` | cooking | `HOST_REQUIRED` | 1 | ✅ BOUND | Plan 136 validated-recipe binding: the host loads recipes_cooking.json through the strict CookingRecipeCatalogLoader and hands the system pre-validated rows, so an authored typo cannot become a live recipe. |
| `MetaProgressionSystem.BindProfileStore` | endgame | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Plan 175 profile-store late binding. Optional: the constructor already accepts a CrossRunProfileStore and defaults to a new one, so the system is functional without this seam and no caller exists yet. Same shape as WeatherCascadeSystem.BindWeatherSource. |
