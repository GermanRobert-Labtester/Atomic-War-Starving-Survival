# Core Port Contracts and Host Wiring Manifest

> **Plan 36 / C2[13] Authority:** Machine-declared integration seams, host wiring expectations, and CI gate.

## Summary Metrics

- **Total integration seams:** 248
- **Host-required (`HOST_REQUIRED`):** 176 (all verified called from `src/`)
- **Optional host ports (`OPTIONAL_HOST`):** 5
- **Live via Core (`LIVE_VIA_CORE`):** 41
- **Test/Diagnostic only (`TEST_ONLY`):** 21
- **Pure library utilities (`PURE_LIBRARY`):** 0
- **Deferred / Exemptions (`DEFERRED`):** 4 (shrink-only ratchet with dated owner)
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
| `AnomalyHazardSystem.BindCatalog` | world | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in AnomalyHazardSystem. |
| `ApprenticeshipSystem.RegisterMentorship` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ApprenticeshipSystem. |
| `ApprenticeshipSystem.RegisterWill` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ApprenticeshipSystem. |
| `AquiferPiezometerEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in AquiferPiezometerEngine. |
| `AquiferPiezometerEngine.BindInventory` | shelter | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in AquiferPiezometerEngine. |
| `ArchaeologySystem.RegisterArchive` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in ArchaeologySystem. |
| `AviationSystem.RegisterAircraft` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in AviationSystem. |
| `BallisticsWorkbenchSystem.RegisterDefinition` | combat | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in BallisticsWorkbenchSystem. |
| `BioFermentationEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in BioFermentationEngine. |
| `BionicsSystem.BindInventory` | medical | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in BionicsSystem. |
| `BlackFlotillaStanding.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in BlackFlotillaStanding. |
| `BlackMarketSystem.BindCatalog` | economy | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in BlackMarketSystem. |
| `BlackMarketSystem.BindFactionBountySystem` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in BlackMarketSystem. |
| `BlackMarketSystem.BindMarket` | economy | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in BlackMarketSystem. |
| `CampaignCalendar.BindProfile` | campaign | `HOST_REQUIRED` | 4 | ✅ BOUND | Integration seam in CampaignCalendar. |
| `CampaignDayCoordinator.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in CampaignDayCoordinator. |
| `CarbonCompositeEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in CarbonCompositeEngine. |
| `CarbonCompositeEngine.BindInventory` | shelter | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in CarbonCompositeEngine. |
| `CargoAirdropSystem.BindCatalog` | world | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in CargoAirdropSystem. |
| `CatalogBootValidator.RegisterCatalog` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in CatalogEntry. |
| `CatalogDiagnostics.RegisterLog` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in CatalogDiagnostics. |
| `CatalogIntegrityDefinitionChecker.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in CatalogIntegrityDefinitionChecker. |
| `CatalogIntegrityDefinitionChecker.RegisterOrReference` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in CatalogIntegrityDefinitionChecker. |
| `CombatCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in CombatCatalog. |
| `CombatPerks.RegisterSurvivor` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in CombatPerks. |
| `CombatTraumaSystem.RegisterSurvivor` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in CombatTraumaSystem. |
| `CommitmentSystem.RegisterCommitment` | commitments | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in CommitmentSystem. |
| `CompanionAnimalSystem.BindFoodPort` | ecology | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in CompanionAnimalSystem. |
| `CompanionAnimalSystem.RegisterCompanion` | ecology | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in CompanionAnimalSystem. |
| `CounterIntelligenceSystem.RegisterProfile` | factions | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in CounterIntelligenceSystem. |
| `CraftingSystem.BindResearchGate` | crafting | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in CraftingSystem. |
| `CrisisPresentationCoordinator.Bind` | core-architecture | `HOST_REQUIRED` | 261 | ✅ BOUND | Integration seam in CrisisPresentationCoordinator. |
| `CrossingQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in CrossingQuestSystem. |
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
| `DiseaseSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in DiseaseSystem. |
| `DiseaseSystem.RegisterStrain` | disease | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in DiseaseSystem. |
| `DistressFollowUpScheduler.BindToMissionEvents` | radio | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in DistressFollowUpScheduler. |
| `DoorEncounterSystem.RegisterEncounter` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in DoorEncounterSystem. |
| `DosimeterCalibrationSystem.RegisterDevice` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in DosimeterCalibrationSystem. |
| `DutyRosterChartEngine.Bind` | core-architecture | `HOST_REQUIRED` | 261 | ✅ BOUND | Integration seam in DutyRosterChartEngine. |
| `DutyRosterOverflowEngine.Bind` | core-architecture | `HOST_REQUIRED` | 261 | ✅ BOUND | Integration seam in DutyRosterOverflowEngine. |
| `DutyRosterOverflowEngine.RegisterOverflowVisit` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in DutyRosterOverflowEngine. |
| `DutyRosterQuestRuntime.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in DutyRosterQuestRuntime. |
| `DutyRosterSystem.RegisterBlankRowsLivingName` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in DutyRosterSystem. |
| `DutyRosterSystem.RegisterOverflowVisit` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in DutyRosterSystem. |
| `EbPvdCoatingEngine.RegisterCoating` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EbPvdCoatingEngine. |
| `EchoSystem.RegisterRange` | narrative | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in EchoSystem. |
| `EquipmentConditionSystem.RegisterItem` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in EquipmentConditionSystem. |
| `EquipmentConditionSystem.RegisterProfile` | equipment | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by EquipmentConditionSystem default-profile construction and LoadProfiles catalog ingestion. |
| `EspionageConsequenceRouter.BindConsumers` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageConsequenceRouter. |
| `EspionageSystem.BindAgentAvailability` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EspionageSystem.BindAgentCapability` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EspionageSystem.BindFactionResolver` | factions | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional override hook; EspionageSystem already defaults to FactionStandingIdResolver.ToSystemsId when no host resolver is supplied. |
| `EspionageSystem.BindResearchGate` | factions | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EspionageSystem.BindRng` | factions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in EspionageSystem. |
| `EvidenceLedger.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in EvidenceLedger. |
| `ExpansionQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in ExpansionQuestSystem. |
| `ExpeditionDefinitionRegistry.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in ExpeditionDefinitionRegistry. |
| `ExpeditionLootReferenceResolver.RegisterCategory` | expeditions | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional resolver extension; canonical/default category sets are already accepted through the resolver constructor. |
| `ExpeditionLootReferenceResolver.RegisterItem` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in ExpeditionLootReferenceResolver. |
| `ExpeditionNavalSystem.RegisterVessel` | expeditions | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by ExpeditionNavalSystem default-vessel construction and LoadCatalog ingestion. |
| `FactionRadioEngine.RegisterChannel` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FactionRadioEngine. |
| `FactionStanceEngine.RegisterFaction` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in FactionStanceEngine. |
| `FactionStanceEngine.RegisterFactions` | factions | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional batch convenience API over the production-wired RegisterFaction seam; no separate activation is required. |
| `FalloutSystem.RegisterPattern` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FalloutSystem. |
| `FeedbackMessageCatalog.RegisterTemplate` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FeedbackMessageCatalog. |
| `FinalWishSystem.RegisterWish` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FinalWishSystem. |
| `FischerTropschSynthesisEngine.BindCatalog` | shelter | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in FischerTropschSynthesisEngine. |
| `FischerTropschSynthesisEngine.BindInventory` | shelter | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in FischerTropschSynthesisEngine. |
| `FischerTropschSynthesisEngine.RegisterLubricantConsumer` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FischerTropschSynthesisEngine. |
| `FluidLogisticsSystem.BindRng` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in FluidLogisticsSystem. |
| `FluidLogisticsSystem.ConfigureSink` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in FluidLogisticsSystem. |
| `FungiCultivationSystem.RegisterCatalog` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in FungiCultivationSystem. |
| `GenerationalSuccessionEngine.RegisterDweller` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in GenerationalSuccessionEngine. |
| `GenerationalSystem.RegisterTrait` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in GenerationalSystem. |
| `GeothermalAquiferSystem.RegisterStrata` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in GeothermalAquiferSystem. |
| `GeothermalOrcSystem.RegisterStratum` | shelter | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in GeothermalOrcSystem. |
| `GrainProcessingSystem.RegisterRecipe` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in GrainProcessingSystem. |
| `GrainProcessingSystem.RegisterSilo` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in GrainProcessingSystem. |
| `GroundPenetratingRadarEngine.BindCatalog` | world | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in GroundPenetratingRadarEngine. |
| `GroundPenetratingRadarEngine.BindInventory` | world | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in GroundPenetratingRadarEngine. |
| `HeliographSystem.RegisterStation` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in HeliographSystem. |
| `HoldfastFactionsCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in HoldfastFactionsCatalog. |
| `HoldfastItemsCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in HoldfastItemsCatalog. |
| `HoldfastNpcCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in HoldfastNpcCatalog. |
| `HoldfastQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in HoldfastQuestSystem. |
| `HydraulicExtrusionEngine.RegisterMachine` | foundry | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in HydraulicExtrusionEngine. |
| `IceRoadSystem.RegisterHoldfastNode` | world | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by IceRoadSystem.RegisterDefaultHoldfastNodes; public method remains the validated extension path for additional holdfast nodes. |
| `IdeologicalFrictionSystem.RegisterBelief` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in IdeologicalFrictionSystem. |
| `IndependentBranchCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in IndependentBranchCatalog. |
| `ItemCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in ItemCatalog. |
| `ItemCatalog.RegisterRange` | core-architecture | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in ItemCatalog. |
| `ItemDescriptionCatalog.Register` | inventory | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in ItemDescriptionCatalog. |
| `ItemDescriptionCatalog.RegisterAlias` | inventory | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ItemDescriptionCatalog. |
| `JournalSystem.BindAuthoredCorpus` | journal | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in JournalSystem. |
| `JournalVoice.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in JournalVoice. |
| `JusticeSystem.RegisterLaw` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in JusticeSystem. |
| `LandmarkDegradationSystem.RegisterLandmark` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in LandmarkDegradationSystem. |
| `LatentExpertAwakeningSystem.RegisterDefinition` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in LatentExpertAwakeningSystem. |
| `LocalizationService.RegisterString` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in LocalizationService. |
| `LocalizationService.RegisterTranslation` | localization | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in LocalizationService. |
| `LyophilizationSystem.RegisterMedicalProtocol` | medical | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in LyophilizationSystem. |
| `MaritimeDiveSystem.RegisterSite` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in MaritimeDiveSystem. |
| `MarketSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in MarketSystem. |
| `MarketSystem.BindCommodityCatalog` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MarketSystem. |
| `MarketSystem.BindEmbargoSystem` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MarketSystem. |
| `MarketSystem.BindRegionalPriceAtlas` | economy | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MarketSystem. |
| `MaterialProfileCatalog.BindOutputItem` | foundry | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in MaterialProfileCatalog. |
| `MedicalPipelineCoordinator.RegisterHandler` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MedicalPipelineCoordinator. |
| `MedicalPipelineCoordinator.RegisterProtocol` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in MedicalPipelineCoordinator. |
| `MercenarySystem.RegisterTemplate` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MercenarySystem. |
| `MicrofluidicDiagnosticEngine.RegisterAssay` | medical | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MicrofluidicDiagnosticEngine. |
| `MilitaryBranchCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in MilitaryBranchCatalog. |
| `MineClearingFlailEngine.RegisterModule` | expeditions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MineClearingFlailEngine. |
| `MoralBranchingSystem.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in MoralBranchingSystem. |
| `MoralBranchingSystem.RegisterMoralChoice` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MoralBranchingSystem. |
| `MoralChoiceSystem.RegisterQuest` | moralchoice | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in MoralChoiceSystem. |
| `MoralChoiceSystem.RegisterQuests` | moralchoice | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MoralChoiceSystem. |
| `MoraleMarkSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in MoraleMarkSystem. |
| `MusterSystem.RegisterQuestline` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in MusterSystem. |
| `MutationSystem.RegisterMutation` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in MutationSystem. |
| `NarrativeArcEventSystem.RegisterRange` | narrative | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in NarrativeArcEventSystem. |
| `NarrativeDiscoveryCatalog.RegisterAdapter` | narrative | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional extension hook; NarrativeDiscoveryCatalog installs its canonical default adapters in the constructor. |
| `NarrativeEncounterSystem.RegisterEncounter` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in NarrativeEncounterSystem. |
| `NarrativeEncounterSystem.RegisterRange` | core-architecture | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in NarrativeEncounterSystem. |
| `NeedsSystem.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in NeedsSystem. |
| `NpcArcCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in NpcArcCatalog. |
| `PanelDescriptor.Bind` | core-architecture | `HOST_REQUIRED` | 261 | ✅ BOUND | Integration seam in PanelDescriptor. |
| `PanelRegistry.ConfigureActions` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PanelRegistry. |
| `PanelRegistry.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in PanelRegistry. |
| `PanelRegistryBootstrap.RegisterAll` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in PanelRegistryBootstrap. |
| `PathogenStrainSystem.BindEngineHooks` | disease | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PathogenStrainSystem. |
| `PhantomMemoryEngine.RegisterRule` | core-architecture | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in PhantomMemoryEngine. |
| `PhantomMemoryEngine.RegisterRuleDetailed` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PhantomMemoryEngine. |
| `PharmaLabSystem.BindSkillEvaluator` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PharmaLabSystem. |
| `PharmaLabSystem.RegisterRecipe` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in PharmaLabSystem. |
| `PharmaceuticalTabletEngine.BindCatalog` | medical | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in PharmaceuticalTabletEngine. |
| `PharmaceuticalTabletEngine.BindInventory` | medical | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in PharmaceuticalTabletEngine. |
| `PlasticPyrolysisSystem.BindCatalog` | shelter | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in PlasticPyrolysisSystem. |
| `PlasticPyrolysisSystem.BindInventory` | shelter | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in PlasticPyrolysisSystem. |
| `PneumaticDispatchSystem.RegisterEndpoint` | shelter | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in PneumaticDispatchSystem. |
| `PowerGridSystem.ConfigureSurge` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PowerGridSystem. |
| `PowerGridSystem.RegisterLoadRoom` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in PowerGridSystem. |
| `PrisonerSystem.RegisterTactic` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in PrisonerSystem. |
| `ProceduralItemInstance.ConfigureSequence` | inventory | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ProceduralItemInstance. |
| `QuestRuntimeCoordinator.Register` | quests | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in QuestRuntimeCoordinator. |
| `QuestlineSystem.RegisterQuestline` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in QuestlineSystem. |
| `RadiationPhaseProgression.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in RadiationPhaseProgression. |
| `RadiationSystem.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in RadiationSystem. |
| `RadioBroadcastCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in RadioBroadcastCatalog. |
| `RadioBroadcastCatalog.RegisterAuthoredGapBroadcasts` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in RadioBroadcastCatalog. |
| `RadioDistressSystem.RegisterSignal` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RadioDistressSystem. |
| `RadioStationCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in RadioStationCatalog. |
| `RailGrindingEngine.RegisterHead` | expeditions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RailGrindingEngine. |
| `RailwayInterlockEngine.BindCatalog` | expeditions | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in RailwayInterlockEngine. |
| `RailwayInterlockEngine.BindInventory` | expeditions | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in RailwayInterlockEngine. |
| `RailwaySystem.RegisterCatalog` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in RailwaySystem. |
| `RailwaySystem.RegisterLogisticsCatalog` | expeditions | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RailwaySystem. |
| `RationConflictSystem.RegisterSurvivor` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RationConflictSystem. |
| `RebelBranchCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in RebelBranchCatalog. |
| `ReconTelemetrySystem.RegisterPlatform` | expeditions | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ReconTelemetrySystem. |
| `ResearchSystem.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in ResearchSystem. |
| `RouteInfrastructureSystem.RegisterMinefield` | world | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RouteInfrastructureSystem. |
| `RouteInfrastructureSystem.RegisterRailSegment` | world | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in RouteInfrastructureSystem. |
| `SafeCrackingSystem.RegisterSafe` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SafeCrackingSystem. |
| `SaltMineExtractionSystem.RegisterVein` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SaltMineExtractionSystem. |
| `SanitationSystem.BindFacilityCatalog` | shelter | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SanitationSystem. |
| `SeasonalEventSystem.BindDefinitions` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SeasonalEventSystem. |
| `SeismicDynamicsSystem.RegisterFault` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in SeismicDynamicsSystem. |
| `SessionLifecycleRegistry.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in SessionLifecycleRegistry. |
| `ShelterBarterSystem.RegisterCaravan` | economy | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in ShelterBarterSystem. |
| `ShelterDecorSystem.RegisterItemModifier` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in ShelterDecorSystem. |
| `ShelterRadioStationSystem.BindSkillProvider` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ShelterRadioStationSystem. |
| `ShelterRadioStationSystem.BindWeatherNoiseProvider` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in ShelterRadioStationSystem. |
| `ShelterSocialDynamicsSystem.BindMediatorSkillProvider` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ShelterSocialDynamicsSystem. |
| `ShelterSocialDynamicsSystem.RegisterSurvivorRoom` | core-architecture | `TEST_ONLY` | 1 | ✅ BOUND | Integration seam in ShelterSocialDynamicsSystem. |
| `ShelterThermalSystem.RegisterExternalBurst` | core-architecture | `DEFERRED` | 0 | ⏳ DEFERRED | Planned beta activation or host wiring. |
| `ShelterThermalSystem.RegisterInsulation` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in ShelterThermalSystem. |
| `ShelterThermalSystem.RegisterThermalGear` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by ShelterThermalSystem default thermal-gear registration during construction. |
| `ShelterWorkshopSystem.BindWorkerSkillProvider` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in ShelterWorkshopSystem. |
| `SignalTriangulationSystem.RegisterStationBaseline` | radio | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in SignalTriangulationSystem. |
| `SilentFoundrySystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindConsequencePolicy` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindGlassworksCatalog` | foundry | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindInventory` | core-architecture | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindMaterialProfiles` | foundry | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindMetallurgyCatalog` | foundry | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindTreaties` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SilentFoundrySystem.BindVentilation` | foundry | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SilentFoundrySystem. |
| `SkillProgressionSystem.RegisterDefaultSkills` | core-architecture | `DEFERRED` | 0 | ⏳ DEFERRED | Planned beta activation or cleanup boundary. |
| `SkillProgressionSystem.RegisterSkill` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SkillProgressionSystem. |
| `SpiritualMeaningCoordinator.RegisterDeath` | core-architecture | `DEFERRED` | 0 | ⏳ DEFERRED | Planned beta activation or cleanup boundary. |
| `StartingLevelSystem.BindMaintenance` | startinglevel | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in StartingLevelSystem. |
| `StealthSystem.RegisterCamouflageGear` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in StealthSystem. |
| `StealthSystem.RegisterWeaponNoise` | core-architecture | `DEFERRED` | 0 | ⏳ DEFERRED | Planned beta activation or cleanup boundary. |
| `SumpFloodingSystem.BindServices` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SumpFloodingSystem. |
| `SurvivorDowntimeSystem.RegisterHobby` | survivors | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by SurvivorDowntimeSystem default-hobby construction and LoadCatalog ingestion. |
| `SurvivorEntityStore.RegisterComponentStore` | core-architecture | `TEST_ONLY` | 0 | 🧪 TEST | Integration seam in SurvivorEntityStore. |
| `SurvivorRosterSystem.RegisterDefinition` | core-architecture | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in SurvivorRosterSystem. |
| `SurvivorRosterSystem.RegisterRange` | core-architecture | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in SurvivorRosterSystem. |
| `SurvivorSocialCoordinator.RegisterBelief` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in SurvivorSocialCoordinator. |
| `TacticalCombatSystem.ConfigureBreachingLogistics` | combat | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in TacticalCombatSystem. |
| `ThirdonaryQuestSystem.BindCatalog` | core-architecture | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in ThirdonaryQuestSystem. |
| `TradeEmbargoSystem.RegisterRule` | economy | `HOST_REQUIRED` | 3 | ✅ BOUND | Integration seam in TradeEmbargoSystem. |
| `TradeSpecialtySystem.RegisterProfessionInfo` | survivors | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in TradeSpecialtySystem. |
| `TradeSpecialtySystem.RegisterProfessionPatterns` | core-architecture | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in TradeSpecialtySystem. |
| `TradeTellEngine.RegisterBand` | economy | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by TradeTellEngine.LoadFromJson while materializing the authored trade_tell_lines.json corpus. |
| `TradeTellEngine.RegisterTellPool` | economy | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Invoked internally by TradeTellEngine.LoadFromJson while materializing the authored trade_tell_lines.json corpus. |
| `TrophySystem.RegisterTrophy` | shelter | `LIVE_VIA_CORE` | 0 | 🔹 CORE | Integration seam in TrophySystem. |
| `UvCoronaDetectionEngine.BindCatalog` | radio | `HOST_REQUIRED` | 19 | ✅ BOUND | Integration seam in UvCoronaDetectionEngine. |
| `UvCoronaDetectionEngine.BindInventory` | radio | `HOST_REQUIRED` | 7 | ✅ BOUND | Integration seam in UvCoronaDetectionEngine. |
| `VehicleGarageSystem.RegisterRecoveryMission` | expeditions | `HOST_REQUIRED` | 2 | ✅ BOUND | Integration seam in VehicleGarageSystem. |
| `VentilationSystem.BindStageServices` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in VentilationSystem. |
| `VentilationSystem.RegisterSource` | core-architecture | `LIVE_VIA_CORE` | 1 | ✅ BOUND | Integration seam in VentilationSystem. |
| `VerdictAccusationSystem.Bind` | verdict | `HOST_REQUIRED` | 261 | ✅ BOUND | Integration seam in VerdictAccusationSystem. |
| `VerdictNpcSystem.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in VerdictNpcSystem. |
| `WastelandMapSystem.RegisterTrapSiteLocation` | world | `OPTIONAL_HOST` | 0 | 🧩 OPTIONAL | Optional runtime extension hook; authored trap-site locations are already supplied by WastelandMapCatalogLoader through the system constructor. |
| `WaterTreatmentSystem.RegisterContaminationAdvisory` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in WaterTreatmentSystem. |
| `WaystationNetworkSystem.BindShortagePolicy` | core-architecture | `HOST_REQUIRED` | 1 | ✅ BOUND | Integration seam in WaystationNetworkSystem. |
| `WeatherGateCatalog.Register` | core-architecture | `HOST_REQUIRED` | 24 | ✅ BOUND | Integration seam in WeatherGateCatalog. |
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
