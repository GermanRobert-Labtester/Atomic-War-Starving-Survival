# PLAN-EVENT-WIRING-21 — Appendix A: Event Wiring Inventory

**Generated:** 2026-09-21, HEAD `5be1a30a`.
**Method:** every `public event` declaration in `Assets/Ashfall.Core/**`
(478 raw, 478 after exclusions) cross-checked for
subscribers (`\bName\s*+=`) across Core+host+tests and producer evidence
(`Name?.Invoke` / `Name(`) in Core+host.
**Verdicts:** `WIRED` (has at least one `+=` subscriber) · `DEAD_CONSUMER`
(produced, never subscribed) · `DEAD_BOTH` (neither).
**Summary:** 394 wired · 84 dead-consumer · 0 dead-both.

Use with PLAN-EVENT-WIRING-21 §3 (EW-21B): every non-`WIRED` row needs a
consumer wired, a journal route, or retirement with its raise site.

## Inventory

| Event | Type | Declaring file | Subs | Subscriber files (≤3) | Verdict |
|---|---|---|---:|---|---|
| `Changed` | Action | `Assets/Ashfall.Core/Economy/TradeScreenSeam.cs` | 1 | `src/Economy/TradeScreenGodotPanel.cs` | WIRED |
| `On12CActivated` | Action | `Assets/Ashfall.Core/CensusClaimSystem.cs` | 2 | `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Assets/Ashfall.Core/Cluster12CHeadlessDemo.cs` | WIRED |
| `OnAccessSoftened` | Action | `Assets/Ashfall.Core/VouchAccessSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnAccidentLogged` | Action | `Assets/Ashfall.Core/IceRoadSystem.cs` | 1 | `src/Host/CoreDemoSession.cs` | WIRED |
| `OnActionCompleted` | Action<ActionResult> | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | 2 | `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs`, `src/Main.World.cs` | WIRED |
| `OnActionExecuted` | Action<WarlordActionResult> | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | 3 | `Ashfall.Core.Tests/WarlordDoctrineTests.cs`, `Ashfall.Core.Tests/Warlords/WarlordPlan63DoctrineTests.cs`, `Assets/Ashfall.Core/Warlords/WarlordHeadlessDemo.cs` | WIRED |
| `OnActionResolved` | Action<FactionActionResolutionRecord> | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` | 2 | `src/Host/MusterHostSession.cs`, `src/Main.Muster.cs` | WIRED |
| `OnActionSelected` | Action<string, string, float> | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` | 3 | `Ashfall.Core.Tests/UtilityAiProbeTests.cs`, `Ashfall.Core.Tests/UtilityAiTests.cs`, `src/Host/UtilityAiHostSession.cs` | WIRED |
| `OnAffinityChanged` | Action<string, string, float> | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | 1 | `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` | WIRED |
| `OnAlarmRaised` | Action<string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 2 | `Ashfall.Core.Tests/ShelterFireHazardSystemTests.cs`, `src/Host/ShelterFireHostSession.cs` | WIRED |
| `OnAlarmTriggered` | Action<string> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 2 | `Ashfall.Core.Tests/SafeCrackingSystemTests.cs`, `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnAntennaCalibrationChanged` | Action<string> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnApproachResolved` | Action<string> | `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnArchiveChanged` | Action | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` | 1 | `src/Host/ArchiveDeskHostSession.cs` | WIRED |
| `OnAssignmentChanged` | Action<string, string> | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | 3 | `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`, `src/Host/Phase0HostSession.cs`, `src/Host/ShelterAssignmentHostSession.cs` | WIRED |
| `OnAtrophyDangerPassed` | Action<string> | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnAttemptMade` | Action<string, SafeAttemptResult> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnAutopsyChanged` | Action | `Assets/Ashfall.Core/AutopsySystem.cs` | 1 | `src/Host/AutopsyHostSession.cs` | WIRED |
| `OnBandReached` | Action<string, int> | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | 1 | `Ashfall.Core.Tests/DoseLedgerSystemTests.cs` | WIRED |
| `OnBaselineCorrected` | Action<string, string> | `Assets/Ashfall.Core/CohortSystem.cs` | 1 | `Ashfall.Core.Tests/CohortSystemTests.cs` | WIRED |
| `OnBatchCompleted` | Action<ActionResult> | `Assets/Ashfall.Core/PharmaLabSystem.cs` | 3 | `src/Host/BioFermentationHostSession.cs`, `src/Host/ChlorAlkaliHostSession.cs`, `src/Host/Plans130To133HostSessions.cs` | WIRED |
| `OnBeaconDark` | Action<string> | `Assets/Ashfall.Core/IceRoadSystem.cs` | 1 | `src/Host/CoreDemoSession.cs` | WIRED |
| `OnBlacklisted` | Action<string, string> | `Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnBlightNarrative` | Action<int> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 3 | `Ashfall.Core.Tests/AgricultureSystemTests.cs`, `src/Host/AgricultureHostSession.cs`, `src/Main.Plans162_165.cs` | WIRED |
| `OnBlightOutbreak` | Action<int> | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | 3 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/GreenhouseHostSession.cs` | WIRED |
| `OnBountyRequested` | Action<string, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 2 | `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs`, `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnBountyRequestedDetailed` | Action<DebtConsequence, string, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 1 | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | WIRED |
| `OnBranchDecided` | Action<MoralBranchState, MoralBranchDirection> | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` | 2 | `Ashfall.Core.Tests/MoralBranchingSystemTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnBribeRefused` | Action<string, string> | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | 2 | `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`, `Assets/Ashfall.Core/CrossingArbitrationHeadlessDemo.cs` | WIRED |
| `OnBrigadeDispatched` | Action<string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnBroadcast` | Action | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnBurdenedCompassionActivated` | Action<MoralBranchState, float> | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` | 1 | `Ashfall.Core.Tests/MoralBranchingSystemTests.cs` | WIRED |
| `OnButcheryCompleted` | Action<string, string, string, bool> | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | 2 | `Ashfall.Core.Tests/WildlifeDiseaseBridgeTests.cs`, `src/Main.ExpandedShelterSystems.cs` | WIRED |
| `OnCalibrationCompleted` | Action<string> | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCalibrationFailed` | Action<string> | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCalibrationOverdue` | Action<string> | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | 1 | `Ashfall.Core.Tests/DosimeterCalibrationSystemTests.cs` | WIRED |
| `OnCalibrationStarted` | Action<string> | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCampDawnResolved` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 1 | `Ashfall.Core.Tests/ExpeditionCampSystemTests.cs` | WIRED |
| `OnCampEncounterResolved` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCampEncounterSurfaced` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 1 | `Ashfall.Core.Tests/ExpeditionCampSystemTests.cs` | WIRED |
| `OnCampEntered` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 2 | `Ashfall.Core.Tests/ExpeditionCampSystemTests.cs`, `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnCampFormed` | Action<int> | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCampNightSegmentResolved` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 1 | `Ashfall.Core.Tests/ExpeditionCampSystemTests.cs` | WIRED |
| `OnCampSuppliesReserved` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCandidateChanged` | Action<TriangulationCandidate> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCaregivingBondDeepened` | Action<string, string, float> | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | 2 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `src/Host/CaregivingHostSession.cs` | WIRED |
| `OnCaregivingDialogueUnlocked` | Action<string, string> | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | 2 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `src/Host/CaregivingHostSession.cs` | WIRED |
| `OnCaregivingEnded` | Action<string, string> | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | 2 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `src/Host/CaregivingHostSession.cs` | WIRED |
| `OnCaregivingStarted` | Action<string, string> | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `src/Host/CaregivingHostSession.cs`, `src/Main.ShelterSocial.cs` | WIRED |
| `OnCarrierHeard` | Action | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCaseCompleted` | Action<AutopsyCase> | `Assets/Ashfall.Core/AutopsySystem.cs` | 3 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/Integration/PrecisionHazardInfrastructureTests.cs`, `Ashfall.Core.Tests/JournalProducerIntegrationTests.cs` | WIRED |
| `OnCaseCompleted` | Action<DeconCase> | `Assets/Ashfall.Core/DecontaminationSystem.cs` | 3 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/Integration/PrecisionHazardInfrastructureTests.cs`, `Ashfall.Core.Tests/JournalProducerIntegrationTests.cs` | WIRED |
| `OnCastFailed` | Action<FoundryFailedCastRecord> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 3 | `Ashfall.Core.Tests/Foundry/MetallurgyB66Tests.cs`, `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`, `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Metallurgy.cs` | WIRED |
| `OnCeilingUpgraded` | Action<string, WallMaterial> | `Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs` | 1 | `Ashfall.Core.Tests/MaterialShieldingSystemTests.cs` | WIRED |
| `OnCensusUpdated` | Action | `Assets/Ashfall.Core/CensusClaimSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnChallengeInitiated` | Action<LeadershipChallengeDTO> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnChallengeResolved` | Action<LeadershipChallengeDTO> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnChapterAdvanced` | Action<int> | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` | 2 | `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `src/Host/ExpansionHostSession.cs` | WIRED |
| `OnChildBooked` | Action<string, string> | `Assets/Ashfall.Core/CohortSystem.cs` | 1 | `Ashfall.Core.Tests/CohortSystemTests.cs` | WIRED |
| `OnChronicFibrosisMarked` | Action<string> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 2 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnChronicIllnessRequested` | Action<string> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 2 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnClaimed` | Action<string> | `Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCoShiftBonusApplied` | Action<string, string, float> | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnCodexUnlocked` | Action<string> | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | 3 | `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/BureaucraticDocumentCatalogTests.cs`, `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs` | WIRED |
| `OnCollateralSeizure` | Action<string, int, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 2 | `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs`, `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnCollateralSeizureDetailed` | Action<DebtConsequence, string, int, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 1 | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | WIRED |
| `OnColonyDied` | Action<string> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 2 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `src/Host/GreenhouseHostSession.cs` | WIRED |
| `OnColonyStressed` | Action<string> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 1 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs` | WIRED |
| `OnColonySwarming` | Action<string> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 2 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `src/Host/GreenhouseHostSession.cs` | WIRED |
| `OnCombatEvent` | Action<CombatState, CombatEvent> | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` | 2 | `src/Audio/AudioEventBridge.cs`, `src/Host/CombatHostSession.cs` | WIRED |
| `OnCombatPenaltyChanged` | Action<string, float> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 3 | `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `src/Host/MedicalHostSession.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnCombatPerkEarned` | Action<string, string> | `Assets/Ashfall.Core/Combat/CombatPerks.cs` | 0 | — | DEAD_CONSUMER |
| `OnCompostCollected` | Action<string, int> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 1 | `src/Host/AgricultureHostSession.cs` | WIRED |
| `OnCompostStarted` | Action<string, int> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 1 | `src/Host/AgricultureHostSession.cs` | WIRED |
| `OnConditionChanged` | Action<EquipmentInstance> | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | 1 | `src/Host/EquipmentConditionHostSession.cs` | WIRED |
| `OnConditionStarted` | Action<ActiveAudioCondition> | `Assets/Ashfall.Core/AudioConditionSystem.cs` | 1 | `src/Audio/AudioConditionHostBridge.cs` | WIRED |
| `OnConditionStopped` | Action<ActiveAudioCondition> | `Assets/Ashfall.Core/AudioConditionSystem.cs` | 1 | `src/Audio/AudioConditionHostBridge.cs` | WIRED |
| `OnConditionsChanged` | Action | `Assets/Ashfall.Core/AudioConditionSystem.cs` | 1 | `src/Audio/AudioConditionHostBridge.cs` | WIRED |
| `OnConflictResolved` | Action<MediationEntry> | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` | 2 | `src/Host/SurvivorRelationsHostSession.cs`, `src/Main.ShelterSocial.cs` | WIRED |
| `OnConflictStarted` | Action<ConflictEntry> | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` | 1 | `src/Host/SurvivorRelationsHostSession.cs` | WIRED |
| `OnConsequenceApplied` | Action<FoundryConsequenceRecord> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 3 | `Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/CombatHostSession.cs` | WIRED |
| `OnConsequenceDispatched` | Action<DebtConsequence, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 3 | `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs`, `Ashfall.Core.Tests/ExpansionHubSaveV5Tests.cs`, `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | WIRED |
| `OnContactMade` | Action | `Assets/Ashfall.Core/Muster/ProvisionedSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnContaminationApplied` | Action<string, string> | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` | 2 | `Ashfall.Core.Tests/BlackFlotillaTests.cs`, `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnContaminationApplied` | Action<string, string> | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | 2 | `Ashfall.Core.Tests/BlackFlotillaTests.cs`, `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnContaminationExpired` | Action<string, string> | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | 1 | `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnContractForgiven` | Action<DebtContract> | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | 1 | `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs` | WIRED |
| `OnContractPaid` | Action<DebtContract> | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | 3 | `Ashfall.Core.Tests/LedgerDebtSystemTests.cs`, `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs`, `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnContractRenegotiated` | Action<DebtContract> | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | 2 | `Ashfall.Core.Tests/LedgerDebtSystemTests.cs`, `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnContractSigned` | Action<DebtContract> | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | 1 | `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnContractorStatusChanged` | Action<Contractor> | `Assets/Ashfall.Core/ContractorRosterSystem.cs` | 1 | `src/Host/ContractorRosterHostSession.cs` | WIRED |
| `OnCraftCompleted` | Action<Recipe, string> | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | 3 | `Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs`, `Ashfall.Core.Tests/CraftingCommandTests.cs`, `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnCraftResultOverflow` | Action<Recipe, string, int> | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | 3 | `Ashfall.Core.Tests/CraftingSystemTests.cs`, `Ashfall.Core.Tests/Production/Plan35ProductionDeliveryTests.cs`, `Ashfall.Core.Tests/Production/Plan35_43ProductionGovernanceIntegrationTests.cs` | WIRED |
| `OnCraftStarted` | Action<Recipe> | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | 3 | `src/Host/CraftingHostSession.cs`, `src/Main.UiPanels.cs`, `src/UI/CraftingPanel.cs` | WIRED |
| `OnCraftingPenaltyChanged` | Action<string, float> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 3 | `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`, `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `Assets/Ashfall.Core/Medical/MedicalHeadlessDemo.cs` | WIRED |
| `OnCrisisResolved` | Action<CrisisCase> | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` | 2 | `src/Host/EconomyHostSession.cs`, `src/Host/MentalHealthCrisisHostSession.cs` | WIRED |
| `OnCropFailed` | Action<int> | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | 3 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/GreenhouseHostSession.cs` | WIRED |
| `OnCropHarvested` | Action<GreenhouseHarvest> | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | 3 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/GreenhouseHostSession.cs` | WIRED |
| `OnCropMatured` | Action<int, string> | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | 3 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `Ashfall.Core.Tests/GreenhouseSystemTests.cs`, `src/Host/ExpansionHostSession.cs` | WIRED |
| `OnCropPlanted` | Action<int, string, int> | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | 3 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/GreenhouseHostSession.cs` | WIRED |
| `OnCulturalBroadcast` | Action<VinylRecordDefinition, int> | `Assets/Ashfall.Core/VinylMoraleSystem.cs` | 3 | `Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs`, `Ashfall.Core.Tests/VinylRadioBridgeTests.cs`, `src/Main.ExpandedShelterSystems.cs` | WIRED |
| `OnDamperChanged` | Action<string, string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 1 | `Ashfall.Core.Tests/ShelterFireHazardSystemTests.cs` | WIRED |
| `OnDayAdvanced` | Action<DayAdvancedEventArgs> | `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs` | 3 | `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`, `Ashfall.Core.Tests/YearOfAshTests.cs`, `src/Host/HostCli.WorldPlaytest.cs` | WIRED |
| `OnDayAdvanced` | Action<int> | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` | 3 | `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`, `Ashfall.Core.Tests/YearOfAshTests.cs`, `src/Host/HostCli.WorldPlaytest.cs` | WIRED |
| `OnDecisionMade` | Action<DeepCoastAccessDecision> | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnDeconChanged` | Action | `Assets/Ashfall.Core/DecontaminationSystem.cs` | 2 | `Ashfall.Core.Tests/Shelter/DeconAirlockSaveTests.cs`, `src/Host/DecontaminationHostSession.cs` | WIRED |
| `OnDecreeEnacted` | Action<string> | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | 1 | `src/Main.YearOfAsh.cs` | WIRED |
| `OnDeficiencyCleared` | Action<string, string> | `Assets/Ashfall.Core/Farming/NutritionDiversitySystem.cs` | 1 | `Ashfall.Core.Tests/AgricultureSystemTests.cs` | WIRED |
| `OnDeficiencyStarted` | Action<string, string> | `Assets/Ashfall.Core/Farming/NutritionDiversitySystem.cs` | 1 | `Ashfall.Core.Tests/AgricultureSystemTests.cs` | WIRED |
| `OnDemandAdjusted` | Action<string, float> | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | 1 | `src/Host/EconomyHostSession.cs` | WIRED |
| `OnDependencyFormed` | Action<string, string> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 3 | `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`, `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `Ashfall.Core.Tests/Medical/Plan22MedicineConsumptionTests.cs` | WIRED |
| `OnDependencyReFormedByStress` | Action<string, string, ChemicalDependencyKind> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 1 | `Ashfall.Core.Tests/Medical/ChemicalDependencyStressRelapseTests.cs` | WIRED |
| `OnDependencyRisk` | Action<float> | `Assets/Ashfall.Core/PharmaLabSystem.cs` | 1 | `src/Main.World.cs` | WIRED |
| `OnDetoxCompleted` | Action<string, string> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 3 | `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`, `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `Ashfall.Core.Tests/MedicalDiagnosisAndCareIntegrationTests.cs` | WIRED |
| `OnDetoxFailed` | Action<string, string> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 3 | `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`, `Assets/Ashfall.Core/Medical/MedicalHeadlessDemo.cs`, `src/Host/ChemicalDependencyHostSession.cs` | WIRED |
| `OnDeviceConditionChanged` | Action<string> | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnDiagnosed` | Action<string, int> | `Assets/Ashfall.Core/SickListSystem.cs` | 1 | `Ashfall.Core.Tests/SickListSystemTests.cs` | WIRED |
| `OnDoctrineChanged` | Action<string, string, string, int> | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | 1 | `src/Main.YearOfAsh.cs` | WIRED |
| `OnDoseChanged` | Action<SurvivorRadState, float> | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | 1 | `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnDoseCorrected` | Action<string, float> | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnDrillFailure` | Action<string> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 2 | `Ashfall.Core.Tests/SaltMineExtractionSystemTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnDutyVacated` | Action<string, string> | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | 2 | `Ashfall.Core.Tests/DutyRoster/Plan24DutyRosterFitnessTests.cs`, `src/Host/DutyRosterHostSession.cs` | WIRED |
| `OnDwellerRetired` | Action<string, int> | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` | 2 | `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `src/Host/ExpansionHostSession.cs` | WIRED |
| `OnEconomyChanged` | Action | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | 2 | `Ashfall.Core.Tests/EconomySystemTests.cs`, `src/Host/EconomyHostSession.cs` | WIRED |
| `OnEmbargoAdded` | Action<FactionEmbargoRecord> | `Assets/Ashfall.Core/FactionEmbargoLedger.cs` | 0 | — | DEAD_CONSUMER |
| `OnEmbargoRequested` | Action<string, int, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 1 | `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnEmbargoRequestedDetailed` | Action<DebtConsequence, string, int, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 1 | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | WIRED |
| `OnEncounterArrived` | Action<DoorEncounterEntry> | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnEncounterEnded` | Action<CombatState> | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` | 1 | `src/Host/CombatHostSession.cs` | WIRED |
| `OnEncounterResolved` | Action<EncounterResolutionRecord> | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | 3 | `Ashfall.Core.Tests/MicroLocationPersistenceWaveTests.cs`, `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`, `src/Host/NarrativeHostSession.cs` | WIRED |
| `OnEncounterResolved` | Action<EncounterResolutionResult> | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` | 3 | `Ashfall.Core.Tests/MicroLocationPersistenceWaveTests.cs`, `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`, `src/Host/NarrativeHostSession.cs` | WIRED |
| `OnEncounterSelected` | Action<EncounterDefinition> | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | 1 | `src/Host/NarrativeHostSession.cs` | WIRED |
| `OnEncounterTriggered` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 3 | `Ashfall.Core.Tests/ExpeditionEncounterBridgeTests.cs`, `Ashfall.Core.Tests/ExpeditionWarlordDangerTests.cs`, `Ashfall.Core.Tests/Expeditions/MicroLocationLifecycleSmokeTests.cs` | WIRED |
| `OnEnrolled` | Action<string> | `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs` | 3 | `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs`, `Ashfall.Core.Tests/VerdictSystemTests.cs`, `src/Host/VerdictHostSession.cs` | WIRED |
| `OnEntryAdded` | Action<JournalEntry> | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | 3 | `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs`, `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs` | WIRED |
| `OnEntryRead` | Action<MachineLogEntry> | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` | 2 | `Assets/Ashfall.Core/Verdict/VerdictEvidenceChain.cs`, `src/Host/VerdictHostSession.cs` | WIRED |
| `OnEnvironmentalCrisisTriggered` | Action<int, string> | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnEquipmentChanged` | Action | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | 1 | `src/Host/EquipmentConditionHostSession.cs` | WIRED |
| `OnEquipmentDamaged` | Action<string, float> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnEulogySpoken` | Action<string, string> | `Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs` | 2 | `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs`, `Ashfall.Core.Tests/JournalProducerIntegrationTests.cs` | WIRED |
| `OnEventRaised` | Action<string, string> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 3 | `Ashfall.Core.Tests/Foundry/MetallurgyB66Tests.cs`, `Ashfall.Core.Tests/Medical/DiseaseProtocolExpiryTests.cs`, `Ashfall.Core.Tests/SilentFoundrySystemTests.cs` | WIRED |
| `OnEventRaised` | Action<string> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 3 | `Ashfall.Core.Tests/Foundry/MetallurgyB66Tests.cs`, `Ashfall.Core.Tests/Medical/DiseaseProtocolExpiryTests.cs`, `Ashfall.Core.Tests/SilentFoundrySystemTests.cs` | WIRED |
| `OnExcavationChanged` | Action | `Assets/Ashfall.Core/ExcavationSystem.cs` | 1 | `src/Host/ExcavationHostSession.cs` | WIRED |
| `OnExpeditionCompleted` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 3 | `Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs`, `Ashfall.Core.Tests/MicroLocationEconomyAuditTests.cs`, `Ashfall.Core.Tests/UI/PanelLifecycleTests.cs` | WIRED |
| `OnExpeditionFailed` | Action<ExpeditionState, string> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 3 | `Ashfall.Core.Tests/ExpeditionSystemTests.cs`, `Ashfall.Core.Tests/MicroLocationEconomyAuditTests.cs`, `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnExpeditionStarted` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 3 | `src/Audio/AudioEventBridge.cs`, `src/Host/ExpeditionHostSession.cs`, `src/Main.Expeditions.cs` | WIRED |
| `OnExpeditionTick` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 1 | `src/Main.Plans152.cs` | WIRED |
| `OnExposureEnded` | Action<SurvivorRadState> | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | 2 | `Ashfall.Core.Tests/Radiation/RadiationExposureTransitionTests.cs`, `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnExposureStarted` | Action<SurvivorRadState> | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | 2 | `Ashfall.Core.Tests/Radiation/RadiationExposureTransitionTests.cs`, `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnExtractionBatchProduced` | Action<string, float> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 1 | `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnFactionStandingChanged` | Action<string, int> | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | 2 | `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs`, `src/YearOfAsh/FactionWarMapWidget.cs` | WIRED |
| `OnFalseAlarmTriggered` | Action<string> | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` | 3 | `Ashfall.Core.Tests/CombatTraumaSystemTests.cs`, `Ashfall.Core.Tests/Defense/PerimeterEarlyWarningEngineTests.cs`, `Ashfall.Core.Tests/Shelter/Plan12_27SocialAutopsyIntegrationTests.cs` | WIRED |
| `OnFinalWishCompleted` | Action<string> | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | 3 | `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs`, `Ashfall.Core.Tests/FinalWishSystemTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnFinalWishFailed` | Action<string> | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | 1 | `Ashfall.Core.Tests/FinalWishSystemTests.cs` | WIRED |
| `OnFinalWishStepCompleted` | Action<string, string> | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | 1 | `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs` | WIRED |
| `OnFireIgnited` | Action<string, string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 2 | `Ashfall.Core.Tests/ElectrostaticFiltrationEngineTests.cs`, `src/Host/ShelterFireHostSession.cs` | WIRED |
| `OnFirstHarvest` | Action<int> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 3 | `Ashfall.Core.Tests/AgricultureSystemTests.cs`, `src/Host/AgricultureHostSession.cs`, `src/Host/HostCli.Plans162_165.cs` | WIRED |
| `OnFlagSet` | Action<string, string> | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 1 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs` | WIRED |
| `OnFlashbackEnded` | Action<string> | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` | 2 | `Ashfall.Core.Tests/SomaticFlashbackSystemTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnFlashbackGrounded` | Action<string, float, float> | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` | 3 | `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`, `Ashfall.Core.Tests/SomaticFlashbackSystemTests.cs`, `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnFlashbackSuppressed` | Action<float> | `Assets/Ashfall.Core/VinylMoraleSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnFlashbackTriggered` | Action<string, float> | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` | 3 | `Ashfall.Core.Tests/SomaticFlashbackSystemTests.cs`, `src/Audio/AudioEventBridge.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnFloodingChanged` | Action | `Assets/Ashfall.Core/SumpFloodingSystem.cs` | 1 | `src/Host/SumpFloodingHostSession.cs` | WIRED |
| `OnForecastConfidenceChanged` | Action<List<SondeForecastEntry>> | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnForecastUpdated` | Action | `Assets/Ashfall.Core/WeatherStationSystem.cs` | 1 | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | WIRED |
| `OnForfeitTriggered` | Action<DebtContract> | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | 3 | `Ashfall.Core.Tests/LedgerDebtSystemTests.cs`, `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs`, `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnFortified` | Action | `Assets/Ashfall.Core/Muster/IronRaidersSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnFreezeAlarmTriggered` | Action<string> | `Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnFrequencyLocked` | Action<string> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnFrictionDetected` | Action<string, string, float> | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | 3 | `Ashfall.Core.Tests/IdeologicalFrictionSystemTests.cs`, `Ashfall.Core.Tests/Plan12BFrictionTests.cs`, `Ashfall.Core.Tests/Shelter/Plan12_27SocialAutopsyIntegrationTests.cs` | WIRED |
| `OnFrostbiteRisk` | Action<string, string> | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | 2 | `Ashfall.Core.Tests/ShelterThermalFrostbiteBridgeTests.cs`, `src/Host/ShelterThermalHostSession.cs` | WIRED |
| `OnGuiltInsomniaCritical` | Action<string> | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | 3 | `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`, `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`, `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs` | WIRED |
| `OnGuiltRecorded` | Action<string, GuiltRecord> | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | 3 | `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`, `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnGuiltResolved` | Action<string> | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | 1 | `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs` | WIRED |
| `OnHarvest` | Action<AgricultureHarvest> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 1 | `src/Host/AgricultureHostSession.cs` | WIRED |
| `OnHazardWarning` | Action<VentilationLogEntry> | `Assets/Ashfall.Core/VentilationSystem.cs` | 2 | `src/Host/AutopsyHostSession.cs`, `src/Host/VentilationHostSession.cs` | WIRED |
| `OnHealthDeltaRequested` | Action<string, float> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 2 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnHeavyMetalExposure` | Action<float> | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | 1 | `src/Host/WaterTreatmentHostSession.cs` | WIRED |
| `OnHidePreserved` | Action<string, string> | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnHiveInstalled` | Action<string> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 2 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `src/Host/GreenhouseHostSession.cs` | WIRED |
| `OnHypervigilanceIncreased` | Action<string, float> | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` | 1 | `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnIceRoadClosed` | Action | `Assets/Ashfall.Core/IceRoadSystem.cs` | 1 | `src/Host/CoreDemoSession.cs` | WIRED |
| `OnIceRoadOpened` | Action | `Assets/Ashfall.Core/IceRoadSystem.cs` | 1 | `src/Host/CoreDemoSession.cs` | WIRED |
| `OnImpactDetailed` | Action<OrbitalImpactReport> | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | 3 | `Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`, `Ashfall.Core.Tests/Shelter/SkyLayerArmorCatalogTests.cs`, `Ashfall.Core.Tests/World/Plan19DynamicWorldTests.cs` | WIRED |
| `OnImpactResolved` | Action<int, float> | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | 2 | `Ashfall.Core.Tests/OrbitalHarrowTelemetrySystemTests.cs`, `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | WIRED |
| `OnImpactWarning` | Action<OrbitalWarningEntry> | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | 2 | `Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs`, `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | WIRED |
| `OnIncident` | Action<FloodIncident> | `Assets/Ashfall.Core/SumpFloodingSystem.cs` | 3 | `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`, `Ashfall.Core.Tests/WaterTreatmentSumpBridgeTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnIncident` | Action<FoundryIncidentRecord> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 3 | `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`, `Ashfall.Core.Tests/WaterTreatmentSumpBridgeTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnIncident` | Action<ThermalIncident> | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | 3 | `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`, `Ashfall.Core.Tests/WaterTreatmentSumpBridgeTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnIncidentResolved` | Action<AirlockIncidentLog> | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` | 2 | `src/Host/AirlockSecurityHostSession.cs`, `src/Host/ShelterFireHostSession.cs` | WIRED |
| `OnIncidentResolved` | Action<string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 2 | `src/Host/AirlockSecurityHostSession.cs`, `src/Host/ShelterFireHostSession.cs` | WIRED |
| `OnIncidentSuppressed` | Action<string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 1 | `src/Host/ShelterFireHostSession.cs` | WIRED |
| `OnInfection` | Action<string, string> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 3 | `Ashfall.Core.Tests/Medical/DiseaseQuarantineCoordinatorTests.cs`, `src/Audio/AudioEventBridge.cs`, `src/Disease/DiseaseHostSession.cs` | WIRED |
| `OnInspectionCompleted` | Action<string> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnInventoryChanged` | Action | `Assets/Ashfall.Core/Inventory/Inventory.cs` | 3 | `Ashfall.Core.Tests/Inventory/InventoryTransactionTests.cs`, `Ashfall.Core.Tests/Inventory/Plan22TagAndRepairTests.cs`, `src/Host/InventoryHostSession.cs` | WIRED |
| `OnItemAdded` | Action<ItemDefinition, int> | `Assets/Ashfall.Core/Inventory/Inventory.cs` | 3 | `Ashfall.Core.Tests/Inventory/InventoryTransactionTests.cs`, `Ashfall.Core.Tests/InventorySystemTests.cs`, `src/Host/HostCli.Collectibles.cs` | WIRED |
| `OnItemDegraded` | Action<string, string> | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` | 1 | `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnItemRemoved` | Action<ItemDefinition, int> | `Assets/Ashfall.Core/Inventory/Inventory.cs` | 1 | `Ashfall.Core.Tests/Inventory/InventoryTransactionTests.cs` | WIRED |
| `OnJobCompleted` | Action<PrepJob> | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | 3 | `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`, `Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`, `src/Audio/ShelterOperationsAudioBridge.cs` | WIRED |
| `OnJobCompleted` | Action<StudyJob> | `Assets/Ashfall.Core/LibraryStudySystem.cs` | 3 | `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`, `Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`, `src/Audio/ShelterOperationsAudioBridge.cs` | WIRED |
| `OnJobCompleted` | Action<TranscriptionJob> | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` | 3 | `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`, `Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`, `src/Audio/ShelterOperationsAudioBridge.cs` | WIRED |
| `OnJournalTriggered` | Action<FoundryJournalTrigger> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 2 | `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnKitchenChanged` | Action | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | 1 | `src/Host/KitchenNutritionHostSession.cs` | WIRED |
| `OnLaborDisputeChanged` | Action<FoundryLaborDispute, int> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnLaborObligation` | Action<string, int, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 0 | — | DEAD_CONSUMER |
| `OnLaborObligationCreated` | Action<DebtLaborObligationRecord> | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | 0 | — | DEAD_CONSUMER |
| `OnLaborObligationDetailed` | Action<DebtConsequence, string, int, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 1 | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | WIRED |
| `OnLaborObligationReleased` | Action<DebtLaborObligationRecord> | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | 0 | — | DEAD_CONSUMER |
| `OnLandmarkCollapsed` | Action<LandmarkStatusRecord> | `Assets/Ashfall.Core/LandmarkDegradationSystem.cs` | 2 | `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs`, `src/Host/HostCli.EvolvingWorld.cs` | WIRED |
| `OnLastSurvivorDied` | Action<SurvivorFateEvent> | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` | 2 | `Ashfall.Core.Tests/SurvivorFateSystemTests.cs`, `src/Main.SurvivorFate.cs` | WIRED |
| `OnLaunchStarted` | Action<string> | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 1 | `Ashfall.Core.Tests/WeatherSondeSystemTests.cs` | WIRED |
| `OnLayoutMutated` | Action<string> | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` | 1 | `Ashfall.Core.Tests/LocationLayoutSystemTests.cs` | WIRED |
| `OnLeaderBreakRisk` | Action<string> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 1 | `Ashfall.Core.Tests/LeadershipSystemTests.cs` | WIRED |
| `OnLeaderDesignated` | Action<string> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 2 | `Ashfall.Core.Tests/LeadershipSystemTests.cs`, `src/Main.Plans182_185.cs` | WIRED |
| `OnLeaderSteppedDown` | Action<string> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 1 | `Ashfall.Core.Tests/LeadershipSystemTests.cs` | WIRED |
| `OnLeaderStressIncreased` | Action<string, float> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnLedgerCalibrated` | Action | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnLedgerTampered` | Action | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | 2 | `Ashfall.Core.Tests/LedgerDebtSystemTests.cs`, `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | WIRED |
| `OnLevyIssued` | Action<LevyOrder> | `Assets/Ashfall.Core/CensusClaimSystem.cs` | 1 | `Assets/Ashfall.Core/CensusHeadlessDemo.cs` | WIRED |
| `OnLevyResolved` | Action<string> | `Assets/Ashfall.Core/CensusClaimSystem.cs` | 2 | `Assets/Ashfall.Core/CensusHeadlessDemo.cs`, `src/Host/CoreDemoSession.cs` | WIRED |
| `OnLibraryChanged` | Action | `Assets/Ashfall.Core/LibraryStudySystem.cs` | 1 | `src/Host/LibraryStudyHostSession.cs` | WIRED |
| `OnLocationMutated` | Action<string> | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnLocationOwnerChanged` | Action<string, string> | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnLocationRecast` | Action<string, string> | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` | 1 | `Ashfall.Core.Tests/StandingRecordSystemTests.cs` | WIRED |
| `OnLocationRevealed` | Action<string> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 3 | `Ashfall.Core.Tests/SignalTriangulationSystemTests.cs`, `src/Host/RadioHostSession.cs`, `src/Main.Narrative.cs` | WIRED |
| `OnLockoutShifted` | Action<int> | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnLogPosted` | Action<MachineLogEntry> | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` | 1 | `src/Host/VerdictHostSession.cs` | WIRED |
| `OnLootAdded` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 2 | `src/Audio/AudioEventBridge.cs`, `src/Host/HostCli.ExpeditionPlaytest.cs` | WIRED |
| `OnLootRolled` | Action<string, string, int> | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` | 1 | `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnLootTransferred` | Action<string> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnLungCapacityReduced` | Action<string, float> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 1 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs` | WIRED |
| `OnMaintenanceCompleted` | Action<MaintenanceJob> | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | 2 | `Ashfall.Core.Tests/Shelter/Plan186ShelterMaintenanceIntegrationTests.cs`, `src/Host/EquipmentConditionHostSession.cs` | WIRED |
| `OnMarkCleared` | Action<string> | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnMarkSet` | Action<string, string> | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnMaturation` | Action<string, int> | `Assets/Ashfall.Core/CohortSystem.cs` | 2 | `Ashfall.Core.Tests/Plan12AGenerationTests.cs`, `Ashfall.Core.Tests/Plan12DCrossSystemContinuityTests.cs` | WIRED |
| `OnMealServed` | Action<MealServingLog> | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | 1 | `src/Host/KitchenNutritionHostSession.cs` | WIRED |
| `OnMedicalProcessingCompleted` | Action<string> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnMentalBreakFromContamination` | Action<string> | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | 1 | `Ashfall.Core.Tests/BlackFlotillaTests.cs` | WIRED |
| `OnMentalHealthChanged` | Action | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` | 1 | `src/Host/MentalHealthCrisisHostSession.cs` | WIRED |
| `OnMineClosed` | Action<string> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnMineOpened` | Action<string> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 1 | `Ashfall.Core.Tests/SaltMineExtractionSystemTests.cs` | WIRED |
| `OnMoralChronicleEntry` | Action<string, string> | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | 1 | `Ashfall.Core.Tests/BlackFlotillaTests.cs` | WIRED |
| `OnMoraleApplied` | Action<float> | `Assets/Ashfall.Core/VinylMoraleSystem.cs` | 3 | `Ashfall.Core.Tests/Collectibles/CollectibleVinylIntegrationTests.cs`, `Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`, `Ashfall.Core.Tests/VinylAcquisitionIntegrationTests.cs` | WIRED |
| `OnMoraleDelta` | Action<string, float, string> | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | 1 | `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` | WIRED |
| `OnMoraleDeltaRequested` | Action<string, float> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 2 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnMoraleDrainRequested` | Action<string, float> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 3 | `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`, `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `Ashfall.Core.Tests/MedicalDiagnosisAndCareIntegrationTests.cs` | WIRED |
| `OnMoraleDrainRequested` | Action<string, float> | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | 3 | `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`, `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `Ashfall.Core.Tests/MedicalDiagnosisAndCareIntegrationTests.cs` | WIRED |
| `OnMutationDetermined` | Action<int, AgriMutationOutcome, string> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 1 | `src/Host/AgricultureHostSession.cs` | WIRED |
| `OnNameErased` | Action<string> | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | 2 | `src/Host/DutyRosterHostSession.cs`, `src/UI/DutyRosterPanel.cs` | WIRED |
| `OnNameRecited` | Action<string, int> | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` | 2 | `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `src/Host/MedicalHostSession.cs` | WIRED |
| `OnNameWritten` | Action<string> | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | 2 | `src/Host/DutyRosterHostSession.cs`, `src/UI/DutyRosterPanel.cs` | WIRED |
| `OnNarrativeMarker` | Action<string> | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnNarrativeRequested` | Action<string, string> | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | 1 | `src/Main.YearOfAsh.cs` | WIRED |
| `OnNoiseGenerated` | Action<string> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnNotificationPing` | Action<JournalEntry> | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | 3 | `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs`, `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs` | WIRED |
| `OnNumbedComfortBlocked` | Action<MoralBranchState> | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` | 1 | `Ashfall.Core.Tests/MoralBranchingSystemTests.cs` | WIRED |
| `OnObservationRecorded` | Action<RadioObservation> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 1 | `Ashfall.Core.Tests/SignalTriangulationSystemTests.cs` | WIRED |
| `OnOfferStatusChanged` | Action<ContractOffer> | `Assets/Ashfall.Core/ContractorRosterSystem.cs` | 1 | `src/Host/ContractorRosterHostSession.cs` | WIRED |
| `OnOutbreakContained` | Action<string, bool> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 2 | `src/Audio/AudioEventBridge.cs`, `src/Disease/DiseaseHostSession.cs` | WIRED |
| `OnOutbreakDeclared` | Action<string> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 3 | `Ashfall.Core.Tests/Flagship11/CrossSystemSmokeTests.cs`, `src/Audio/AudioEventBridge.cs`, `src/Disease/DiseaseHostSession.cs` | WIRED |
| `OnOutcomeResolved` | Action<string, string, bool> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 3 | `Ashfall.Core.Tests/Medical/DiseaseQuarantineCoordinatorTests.cs`, `Ashfall.Core.Tests/Medical/DiseaseTreatmentTests.cs`, `src/Audio/AudioEventBridge.cs` | WIRED |
| `OnOutputContaminated` | Action<string> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnOverlayAccessChanged` | Action<bool> | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnPackMigrated` | Action<WildlifePackRecord> | `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` | 1 | `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs` | WIRED |
| `OnPalliativeAssigned` | Action<string, string> | `Assets/Ashfall.Core/SickListSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnPathogenExposure` | Action<float> | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | 2 | `Ashfall.Core.Tests/WaterTreatmentSumpBridgeTests.cs`, `src/Host/WaterTreatmentHostSession.cs` | WIRED |
| `OnPayloadLanded` | Action | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 1 | `src/Host/WeatherHostSession.cs` | WIRED |
| `OnPermanentMoraleBuffApplied` | Action<float> | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnPestTreated` | Action<int, string> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 1 | `src/Host/AgricultureHostSession.cs` | WIRED |
| `OnPhantomKnock` | Action | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` | 3 | `Ashfall.Core.Tests/MedicalDiagnosisAndCareIntegrationTests.cs`, `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `src/Host/MedicalHostSession.cs` | WIRED |
| `OnPharmaStateChanged` | Action | `Assets/Ashfall.Core/PharmaLabSystem.cs` | 2 | `src/Host/CraftingHostSession.cs`, `src/UI/PharmaLabPanel.cs` | WIRED |
| `OnPhaseChanged` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 3 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `Ashfall.Core.Tests/Shelter/Plan188ScheduleHourConsumerTests.cs`, `Ashfall.Core.Tests/VerdictChainTests.cs` | WIRED |
| `OnPhaseChanged` | Action<ReckoningPhase> | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | 3 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `Ashfall.Core.Tests/Shelter/Plan188ScheduleHourConsumerTests.cs`, `Ashfall.Core.Tests/VerdictChainTests.cs` | WIRED |
| `OnPhaseChanged` | Action<SchedulePhase> | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | 3 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `Ashfall.Core.Tests/Shelter/Plan188ScheduleHourConsumerTests.cs`, `Ashfall.Core.Tests/VerdictChainTests.cs` | WIRED |
| `OnPhaseChanged` | Action<string, RadiationSicknessPhase, RadiationSicknessPhase> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 3 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `Ashfall.Core.Tests/Shelter/Plan188ScheduleHourConsumerTests.cs`, `Ashfall.Core.Tests/VerdictChainTests.cs` | WIRED |
| `OnPhaseTransitioned` | Action<YearOfAshPhase> | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` | 1 | `src/YearOfAsh/FactionWarMapWidget.cs` | WIRED |
| `OnPlaybackChanged` | Action | `Assets/Ashfall.Core/VinylMoraleSystem.cs` | 1 | `src/Host/VinylMoraleHostSession.cs` | WIRED |
| `OnPlotDriedOut` | Action<int> | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | 3 | `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`, `Ashfall.Core.Tests/GreenhouseSystemTests.cs`, `src/Host/ExpansionHostSession.cs` | WIRED |
| `OnPlotInfested` | Action<int, string> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 2 | `src/Host/AgricultureHostSession.cs`, `src/Main.Plans162_165.cs` | WIRED |
| `OnPolicyChanged` | Action<string> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 2 | `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`, `Assets/Ashfall.Core/Governance/ShelterGovernanceEngine.cs` | WIRED |
| `OnPollinationChanged` | Action<string, float> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnProductionCompleted` | Action<FoundryProductionRecord> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 3 | `Ashfall.Core.Tests/Foundry/GlassworksB100Tests.cs`, `Ashfall.Core.Tests/Foundry/MetallurgyB66Tests.cs`, `Ashfall.Core.Tests/SilentFoundrySystemTests.cs` | WIRED |
| `OnProductionTick` | Action<string, float, float> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnProvenanceComplete` | Action | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnPumpFailure` | Action<string> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 1 | `Ashfall.Core.Tests/SaltMineExtractionSystemTests.cs` | WIRED |
| `OnQuarantineEnded` | Action<string, string> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 2 | `src/Audio/AudioEventBridge.cs`, `src/Disease/DiseaseHostSession.cs` | WIRED |
| `OnQuarantineStarted` | Action<string, string> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 3 | `Ashfall.Core.Tests/Flagship11/CrossSystemSmokeTests.cs`, `src/Audio/AudioEventBridge.cs`, `src/Disease/DiseaseHostSession.cs` | WIRED |
| `OnQuestChoiceTaken` | Action<QuestChoiceResult> | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | 3 | `Ashfall.Core.Tests/QuestlineSystemTests.cs`, `src/Host/DoseLedgerHostSession.cs`, `src/Main.YearOfAsh.cs` | WIRED |
| `OnQuestCompleted` | Action<DutyRosterQuestProgress> | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestCompleted` | Action<ExpansionQuestEntry> | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestCompleted` | Action<string> | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestCompleted` | Action<string> | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestFailed` | Action<DutyRosterQuestProgress> | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestFailed` | Action<ExpansionQuestEntry> | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestFailed` | Action<string> | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestStageAdvanced` | Action<DutyRosterQuestProgress> | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` | 0 | — | DEAD_CONSUMER |
| `OnQuestStageChanged` | Action<string, int> | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 2 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `src/UI/QuestsAtlasPanel.cs` | WIRED |
| `OnQuestStageChanged` | Action<string, int> | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` | 2 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `src/UI/QuestsAtlasPanel.cs` | WIRED |
| `OnQuestStarted` | Action<DutyRosterQuestProgress> | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestStarted` | Action<ExpansionQuestEntry> | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestStarted` | Action<string> | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestStarted` | Action<string> | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs` | WIRED |
| `OnQuestlineResolved` | Action<MusterRecord> | `Assets/Ashfall.Core/Muster/MusterSystem.cs` | 3 | `Ashfall.Core.Tests/MusterSystemTests.cs`, `Ashfall.Core.Tests/QuestlineSystemTests.cs`, `src/Host/DoseLedgerHostSession.cs` | WIRED |
| `OnQuestlineResolved` | Action<string, QuestlineStatus> | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | 3 | `Ashfall.Core.Tests/MusterSystemTests.cs`, `Ashfall.Core.Tests/QuestlineSystemTests.cs`, `src/Host/DoseLedgerHostSession.cs` | WIRED |
| `OnQuestlineStarted` | Action<QuestlineDefinition> | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | 3 | `Ashfall.Core.Tests/QuestlineSystemTests.cs`, `src/Host/DoseLedgerHostSession.cs`, `src/Main.YearOfAsh.cs` | WIRED |
| `OnRadiationDoseResetRequested` | Action<string> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 2 | `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnRadiationExposure` | Action<float> | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | 1 | `Ashfall.Core.Tests/WaterTreatmentSystemTests.cs` | WIRED |
| `OnRadonAlarmTriggered` | Action<string> | `Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnRadonLevelChanged` | Action<float> | `Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs` | 1 | `src/YearOfAsh/RadonVentilationWidget.cs` | WIRED |
| `OnRaidExecuted` | Action | `Assets/Ashfall.Core/Muster/IronRaidersSystem.cs` | 1 | `src/Main.Muster.cs` | WIRED |
| `OnRationConfrontation` | Action<string, string> | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnRationsStolen` | Action<string, string> | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnReadingConfidenceChanged` | Action<string> | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnReckoningCall` | Action<int> | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | 1 | `src/Host/VerdictHostSession.cs` | WIRED |
| `OnRegionChanged` | Action | `Assets/Ashfall.Core/Muster/LongWalkSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnRelationsChanged` | Action | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnReleased` | Action<string> | `Assets/Ashfall.Core/SickListSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnRequiresInhaler` | Action<string> | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | 2 | `Ashfall.Core.Tests/RespiratoryDegenerationSystemTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnResentmentBuilt` | Action<string, string, float> | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | 1 | `Ashfall.Core.Tests/RationConflictSystemTests.cs` | WIRED |
| `OnRespiratoryDegradationIncreased` | Action<string, float> | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | 1 | `Ashfall.Core.Tests/RespiratoryDegenerationSystemTests.cs` | WIRED |
| `OnRoomEntered` | Action<string, string> | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` | 3 | `Ashfall.Core.Tests/BlackFlotillaTests.cs`, `Ashfall.Core.Tests/LocationLayoutSystemTests.cs`, `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnRoomUnlocked` | Action<string, string> | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` | 1 | `Ashfall.Core.Tests/LocationLayoutSystemTests.cs` | WIRED |
| `OnRoommateSynergy` | Action<string, string> | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | 2 | `Ashfall.Core.Tests/IdeologicalFrictionSystemTests.cs`, `Ashfall.Core.Tests/Shelter/Plan12_27SocialAutopsyIntegrationTests.cs` | WIRED |
| `OnRosterBurned` | Action | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | 1 | `src/Host/DutyRosterHostSession.cs` | WIRED |
| `OnRosterChanged` | Action | `Assets/Ashfall.Core/ContractorRosterSystem.cs` | 1 | `src/Host/ContractorRosterHostSession.cs` | WIRED |
| `OnRosterUpdated` | Action | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | 2 | `src/Host/DutyRosterHostSession.cs`, `src/UI/DutyRosterPanel.cs` | WIRED |
| `OnRulingMade` | Action<StandingRuling> | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | 1 | `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs` | WIRED |
| `OnRulingOverturned` | Action<StandingRuling> | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | 2 | `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`, `Assets/Ashfall.Core/CrossingArbitrationHeadlessDemo.cs` | WIRED |
| `OnSafeInspected` | Action<string> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSafeJammed` | Action<string> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 1 | `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnSafeOpened` | Action<string> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 1 | `src/Host/MaritimeHostSession.cs` | WIRED |
| `OnSafetyWarning` | Action<string> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSalvageRolled` | Action<string, string, int> | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnScheduleChanged` | Action | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | 1 | `src/Host/ShelterScheduleHostSession.cs` | WIRED |
| `OnSecurityChanged` | Action | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` | 2 | `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs`, `src/Host/AirlockSecurityHostSession.cs` | WIRED |
| `OnSevereCoughStarted` | Action<string> | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | 3 | `Ashfall.Core.Tests/RespiratoryDegenerationSystemTests.cs`, `src/Host/Phase0HostSession.cs`, `src/Main.Medical.cs` | WIRED |
| `OnShelterEncounterResolved` | Action<ShelterEncounterRecord> | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnShelterEncounterStarted` | Action<ShelterEncounterRecord> | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` | 1 | `src/Host/DutyRosterHostSession.cs` | WIRED |
| `OnShelterFalseAlarm` | Action<float> | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` | 1 | `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnShockExpired` | Action<MarketShockState> | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | 3 | `Ashfall.Core.Tests/Economy/Plan212DynamicEconomyTests.cs`, `src/Host/EconomyHostSession.cs`, `src/Main.Economy.cs` | WIRED |
| `OnShockStarted` | Action<MarketShockState> | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | 2 | `src/Host/EconomyHostSession.cs`, `src/Main.Economy.cs` | WIRED |
| `OnSiteEncounterResolved` | Action<SiteEncounterRecord> | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSiteEncounterStarted` | Action<SiteEncounterRecord> | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSkillAtrophied` | Action<string, string> | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs` | 1 | `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` | WIRED |
| `OnSmokeZoneChanged` | Action<string, string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSondeFailed` | Action<string> | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 1 | `src/Host/WeatherHostSession.cs` | WIRED |
| `OnSondeRecovered` | Action<string> | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 1 | `src/Host/WeatherHostSession.cs` | WIRED |
| `OnSpecialtyMastered` | Action<string, string> | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` | 3 | `Ashfall.Core.Tests/Progression/Plan105_106TradeDoseIntegrationTests.cs`, `Ashfall.Core.Tests/Progression/TradeSpecialtySystemTests.cs`, `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs` | WIRED |
| `OnSpecialtyMilestone` | Action<string, string, int> | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` | 3 | `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`, `Ashfall.Core.Tests/Progression/Plan105_106TradeDoseIntegrationTests.cs`, `Ashfall.Core.Tests/Progression/TradeSpecialtySystemTests.cs` | WIRED |
| `OnSpoken` | Action<VerdictNpcEntry> | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | 1 | `src/Host/VerdictHostSession.cs` | WIRED |
| `OnStageAdvanced` | Action<DeepCoastStage> | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` | 3 | `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`, `src/Host/NarrativeQuestlineHostSession.cs`, `src/Host/PersonalQuestHostSession.cs` | WIRED |
| `OnStageNarrativeEmitted` | Action<CrossingStageNarrativeEvent> | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 2 | `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `src/Host/ExpansionHostSession.cs` | WIRED |
| `OnStaminaPenaltyRequested` | Action<string, float> | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | 2 | `Ashfall.Core.Tests/RespiratoryDegenerationSystemTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnStandingCalled` | Action<string> | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | 2 | `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`, `Assets/Ashfall.Core/CrossingArbitrationHeadlessDemo.cs` | WIRED |
| `OnStandingPenalty` | Action<DebtConsequence, string, DebtContract> | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` | 3 | `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs`, `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs`, `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/FactionEmbargoLedger.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<ApicultureState> | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<BrineWaterSystemState> | `Assets/Ashfall.Core/BrineWaterSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<CensusClaimSystemState> | `Assets/Ashfall.Core/CensusClaimSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<CoalitionCampState> | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<CohortSystemState> | `Assets/Ashfall.Core/CohortSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<ColdCountState> | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<CombatState> | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<CrossingArbitrationState> | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<CrossingQuestSystemState> | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<Dictionary<string, FireIncidentState>> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<DiseaseSystemState> | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<DoseLedgerSystemState> | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<DosimeterCalibrationState> | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<DutyRosterQuestState> | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<DutyRosterSystemState> | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<ExpansionQuestSystemState> | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<FactionActionBoardState> | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<HoldfastQuestSystemState> | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<HydroBaronsState> | `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<IceRoadSystemState> | `Assets/Ashfall.Core/IceRoadSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<IronRaidersState> | `Assets/Ashfall.Core/Muster/IronRaidersSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<LedgerDebtSystemState> | `Assets/Ashfall.Core/LedgerDebtSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<LocationLayoutState> | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<LocationMemoryState> | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<LongWalkState> | `Assets/Ashfall.Core/Muster/LongWalkSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<MarketState> | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<MoraleMarkSystemState> | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<MusterState> | `Assets/Ashfall.Core/Muster/MusterSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<NarrativeEncounterState> | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<ProvisionedState> | `Assets/Ashfall.Core/Muster/ProvisionedSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<SafeCrackingState> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<SaltMineState> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<ScavengerGuildState> | `Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<ShelterEncounterSystemState> | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<SickListSystemState> | `Assets/Ashfall.Core/SickListSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<SilentFoundryState> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<SiteEncounterState> | `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<SurvivorRosterState> | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<TriangulationState> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<VoluntaryRegisterSystemState> | `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<VouchAccessSystemState> | `Assets/Ashfall.Core/VouchAccessSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<WaystationSystemState> | `Assets/Ashfall.Core/WaystationSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<WeatherSondeState> | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStateChanged` | Action<WorldWeatherState> | `Assets/Ashfall.Core/World/WeatherSystem.cs` | 3 | `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/CombatTraumaSystemTests.cs` | WIRED |
| `OnStationStateChanged` | Action | `Assets/Ashfall.Core/WeatherStationSystem.cs` | 1 | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | WIRED |
| `OnStatusGained` | Action<SurvivorRadState, SurvivorStatus> | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | 3 | `Ashfall.Core.Tests/AudioEventIntegrationTests.cs`, `src/Audio/AudioEventBridge.cs`, `src/Host/SurvivorsHostSession.cs` | WIRED |
| `OnStatusLost` | Action<SurvivorRadState, SurvivorStatus> | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSteamTrip` | Action | `Assets/Ashfall.Core/BrineWaterSystem.cs` | 3 | `Ashfall.Core.Tests/BrineWaterSystemTests.cs`, `Assets/Ashfall.Core/BrineWaterHeadlessDemo.cs`, `Assets/Ashfall.Core/HoldfastSession.cs` | WIRED |
| `OnStoveDied` | Action | `Assets/Ashfall.Core/WaystationSystem.cs` | 2 | `Ashfall.Core.Tests/WaystationSystemTests.cs`, `src/Host/WaystationHostSession.cs` | WIRED |
| `OnStrainPlanted` | Action<int, string, int> | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | 1 | `src/Host/AgricultureHostSession.cs` | WIRED |
| `OnStrategySet` | Action<string> | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnStressReported` | Action<string, string, float> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 1 | `Ashfall.Core.Tests/Medical/ChemicalDependencyStressRelapseTests.cs` | WIRED |
| `OnStrikeResolved` | Action<FoundryStrikeResolution, int> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSuccessionTriggered` | Action<string, string> | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | 2 | `Ashfall.Core.Tests/Survivors/LeadershipSuccessionTests.cs`, `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs` | WIRED |
| `OnSurfaced` | Action<EncounterSurfaced> | `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs` | 3 | `Ashfall.Core.Tests/ExpeditionEncounterBridgeTests.cs`, `Ashfall.Core.Tests/MicroLocationDeterminismHarness.cs`, `Ashfall.Core.Tests/MicroLocationEconomyAuditTests.cs` | WIRED |
| `OnSurvivorDied` | Action<SurvivorRosterEntry, string> | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` | 3 | `Ashfall.Core.Tests/SurvivorRosterSystemTests.cs`, `Assets/Ashfall.Core/Survivors/SurvivorsHeadlessDemo.cs`, `src/Host/HostCli.PanelTests.cs` | WIRED |
| `OnSurvivorExposed` | Action<string, string> | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnSurvivorFate` | Action<SurvivorFateEvent> | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` | 3 | `src/Audio/AudioEventBridge.cs`, `src/Main.Spiritual.cs`, `src/Main.SurvivorFate.cs` | WIRED |
| `OnSurvivorJoined` | Action<SurvivorRosterEntry> | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` | 2 | `Ashfall.Core.Tests/SurvivorRosterSystemTests.cs`, `Assets/Ashfall.Core/Survivors/SurvivorsHeadlessDemo.cs` | WIRED |
| `OnTabChanged` | Action<int> | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | 3 | `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs`, `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs`, `Ashfall.Core.Tests/JournalSystemTests.cs` | WIRED |
| `OnTapeSpin` | Action | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` | 1 | `Ashfall.Core.Tests/VerdictSystemTests.cs` | WIRED |
| `OnTelemetryChanged` | Action | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | 1 | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | WIRED |
| `OnTelemetryLost` | Action<SondeTelemetrySample> | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnTelemetryReceived` | Action<SondeTelemetrySample> | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnTemperatureChanged` | Action<float> | `Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs` | 1 | `src/YearOfAsh/GeothermalHeatingWidget.cs` | WIRED |
| `OnTerminalLungDamage` | Action<string> | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` | 1 | `Ashfall.Core.Tests/RespiratoryDegenerationSystemTests.cs` | WIRED |
| `OnTerminalPrognosisDeclared` | Action<string, float> | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` | 3 | `Ashfall.Core.Tests/FinalWishSystemTests.cs`, `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnTerminalPrognosisDeclared` | Action<string, string, float> | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` | 3 | `Ashfall.Core.Tests/FinalWishSystemTests.cs`, `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`, `src/Host/Phase0HostSession.cs` | WIRED |
| `OnTerritorialClashOccurred` | Action<string, string> | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` | 1 | `src/Main.YearOfAsh.cs` | WIRED |
| `OnTerritoryChanged` | Action<string, int, int, int> | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | 2 | `Ashfall.Core.Tests/WarlordDoctrineTests.cs`, `Assets/Ashfall.Core/Warlords/WarlordHeadlessDemo.cs` | WIRED |
| `OnThermalChanged` | Action | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | 1 | `src/Host/ShelterThermalHostSession.cs` | WIRED |
| `OnToolDamaged` | Action<string> | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnTraitInherited` | Action<string, string, string> | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` | 2 | `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `src/Host/ExpansionHostSession.cs` | WIRED |
| `OnTrappingChanged` | Action | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | 1 | `src/Host/WildlifeTrappingHostSession.cs` | WIRED |
| `OnTraumaBondDecayed` | Action<string, string> | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` | 1 | `Ashfall.Core.Tests/TraumaBondSystemTests.cs` | WIRED |
| `OnTraumaBondFormed` | Action<string, string, string> | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` | 1 | `Ashfall.Core.Tests/TraumaBondSystemTests.cs` | WIRED |
| `OnTreatmentCompleted` | Action<ActionResult> | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | 2 | `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`, `src/Host/WaterTreatmentHostSession.cs` | WIRED |
| `OnTreatyDeliveryAccepted` | Action<TreatyDeliveryRecord> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 2 | `Ashfall.Core.Tests/SaltMineExtractionSystemTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnTreatyDeliveryMissed` | Action<TreatyDeliveryRecord> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 1 | `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnTreatyQuotaMet` | Action<FoundryTreatyCompliance> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 3 | `Ashfall.Core.Tests/Foundry/FoundryPlan129IntegrationTests.cs`, `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnTreatyQuotaMissed` | Action<FoundryTreatyCompliance> | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` | 2 | `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`, `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnTreatyStatusChanged` | Action<TreatyInstance> | `Assets/Ashfall.Core/RegionalTreatySystem.cs` | 1 | `src/Host/RegionalTreatyHostSession.cs` | WIRED |
| `OnTriangulationCompleted` | Action<string> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnTriangulationFailed` | Action<string> | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` | 0 | — | DEAD_CONSUMER |
| `OnTributeDemanded` | Action<int, string, int> | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | 3 | `Ashfall.Core.Tests/WarlordDoctrineTests.cs`, `Assets/Ashfall.Core/Warlords/WarlordHeadlessDemo.cs`, `src/Main.YearOfAsh.cs` | WIRED |
| `OnTributeSettled` | Action<bool, int> | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` | 2 | `src/UI/FactionsPanel.cs`, `src/YearOfAsh/YearOfAshHostSession.cs` | WIRED |
| `OnUnlocked` | Action | `Assets/Ashfall.Core/WaystationSystem.cs` | 2 | `Ashfall.Core.Tests/WaystationSystemTests.cs`, `src/Host/WaystationHostSession.cs` | WIRED |
| `OnVehicleBreakdown` | Action<ExpeditionState> | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | 3 | `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`, `src/Audio/AudioEventBridge.cs`, `src/Host/ExpeditionHostSession.cs` | WIRED |
| `OnVehicleStateChanged` | Action | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | 1 | `src/Host/ExpeditionHostSession.cs` | WIRED |
| `OnVentilationChanged` | Action | `Assets/Ashfall.Core/VentilationSystem.cs` | 1 | `src/Host/VentilationHostSession.cs` | WIRED |
| `OnVerdictResolved` | Action<string> | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | 1 | `src/Host/VerdictHostSession.cs` | WIRED |
| `OnVigilCompleted` | Action<bool> | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` | 3 | `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `src/Host/MedicalHostSession.cs` | WIRED |
| `OnVigilStarted` | Action<string> | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` | 3 | `Ashfall.Core.Tests/Medical/MedicalHostSessionTests.cs`, `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs`, `src/Host/HostCli.PanelTests.cs` | WIRED |
| `OnVolunteerCompleted` | Action<string, float> | `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs` | 1 | `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs` | WIRED |
| `OnVolunteered` | Action<string, string> | `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs` | 1 | `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs` | WIRED |
| `OnVouchBurned` | Action | `Assets/Ashfall.Core/VouchAccessSystem.cs` | 1 | `Ashfall.Core.Tests/VouchAccessSystemTests.cs` | WIRED |
| `OnVouchGranted` | Action<string> | `Assets/Ashfall.Core/VouchAccessSystem.cs` | 1 | `Ashfall.Core.Tests/VouchAccessSystemTests.cs` | WIRED |
| `OnWaterStateChanged` | Action | `Assets/Ashfall.Core/BrineWaterSystem.cs` | 1 | `src/Host/WaterTreatmentHostSession.cs` | WIRED |
| `OnWaterStateChanged` | Action | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | 1 | `src/Host/WaterTreatmentHostSession.cs` | WIRED |
| `OnWeatherChanged` | Action<WeatherKind> | `Assets/Ashfall.Core/World/WeatherSystem.cs` | 3 | `Ashfall.Core.Tests/AudioEventIntegrationTests.cs`, `Ashfall.Core.Tests/UI/PanelLifecycleTests.cs`, `Ashfall.Core.Tests/WeatherSystemTests.cs` | WIRED |
| `OnWithdrawalStarted` | Action<string, string> | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | 1 | `src/Host/ChemicalDependencyHostSession.cs` | WIRED |
| `OnWorkerExposure` | Action<string, float> | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | 1 | `src/Foundry/SilentFoundryHostSession.cs` | WIRED |
| `OnWorkshopStateChanged` | Action | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | 2 | `src/Host/CraftingHostSession.cs`, `src/UI/WorkshopPanel.cs` | WIRED |
| `StateChanged` | Action | `Assets/Ashfall.Core/HoldfastTradeSession.cs` | 3 | `Ashfall.Core.Tests/DutyRoster/DutyRosterHostSessionTests.cs`, `Ashfall.Core.Tests/Economy/EconomyHostSessionTests.cs`, `Ashfall.Core.Tests/Economy/HoldfastTradeIntegrityTests.cs` | WIRED |

## Work list (non-WIRED events)

- `OnAccessSoftened` (`Assets/Ashfall.Core/VouchAccessSystem.cs`)
- `OnAntennaCalibrationChanged` (`Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs`)
- `OnApproachResolved` (`Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs`)
- `OnAtrophyDangerPassed` (`Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`)
- `OnAttemptMade` (`Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs`)
- `OnBlacklisted` (`Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs`)
- `OnBrigadeDispatched` (`Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`)
- `OnBroadcast` (`Assets/Ashfall.Core/Muster/ColdCountSystem.cs`)
- `OnCalibrationCompleted` (`Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`)
- `OnCalibrationFailed` (`Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`)
- `OnCalibrationStarted` (`Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`)
- `OnCampEncounterResolved` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`)
- `OnCampFormed` (`Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs`)
- `OnCampSuppliesReserved` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`)
- `OnCandidateChanged` (`Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs`)
- `OnCarrierHeard` (`Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`)
- `OnCensusUpdated` (`Assets/Ashfall.Core/CensusClaimSystem.cs`)
- `OnChallengeInitiated` (`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`)
- `OnChallengeResolved` (`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`)
- `OnClaimed` (`Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs`)
- `OnCoShiftBonusApplied` (`Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs`)
- `OnCombatPerkEarned` (`Assets/Ashfall.Core/Combat/CombatPerks.cs`)
- `OnContactMade` (`Assets/Ashfall.Core/Muster/ProvisionedSystem.cs`)
- `OnDecisionMade` (`Assets/Ashfall.Core/District8DeepCoastSystem.cs`)
- `OnDeviceConditionChanged` (`Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`)
- `OnDoseCorrected` (`Assets/Ashfall.Core/DoseLedgerSystem.cs`)
- `OnEmbargoAdded` (`Assets/Ashfall.Core/FactionEmbargoLedger.cs`)
- `OnEncounterArrived` (`Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs`)
- `OnEnvironmentalCrisisTriggered` (`Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs`)
- `OnEquipmentDamaged` (`Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`)
- `OnFlashbackSuppressed` (`Assets/Ashfall.Core/VinylMoraleSystem.cs`)
- `OnForecastConfidenceChanged` (`Assets/Ashfall.Core/World/WeatherSondeSystem.cs`)
- `OnFortified` (`Assets/Ashfall.Core/Muster/IronRaidersSystem.cs`)
- `OnFreezeAlarmTriggered` (`Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs`)
- `OnFrequencyLocked` (`Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs`)
- `OnHidePreserved` (`Assets/Ashfall.Core/WildlifeTrappingSystem.cs`)
- `OnInspectionCompleted` (`Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs`)
- `OnLaborDisputeChanged` (`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`)
- `OnLaborObligation` (`Assets/Ashfall.Core/DebtConsequenceDispatcher.cs`)
- `OnLaborObligationCreated` (`Assets/Ashfall.Core/DebtConsequenceHostBridge.cs`)
- `OnLaborObligationReleased` (`Assets/Ashfall.Core/DebtConsequenceHostBridge.cs`)
- `OnLeaderStressIncreased` (`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`)
- `OnLedgerCalibrated` (`Assets/Ashfall.Core/DoseLedgerSystem.cs`)
- `OnLocationMutated` (`Assets/Ashfall.Core/LocationEvolutionSystem.cs`)
- `OnLocationOwnerChanged` (`Assets/Ashfall.Core/LocationEvolutionSystem.cs`)
- `OnLockoutShifted` (`Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs`)
- `OnLootTransferred` (`Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs`)
- `OnMarkCleared` (`Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs`)
- `OnMarkSet` (`Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs`)
- `OnMedicalProcessingCompleted` (`Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs`)
- `OnMineClosed` (`Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs`)
- `OnNarrativeMarker` (`Assets/Ashfall.Core/District8DeepCoastSystem.cs`)
- `OnNoiseGenerated` (`Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs`)
- `OnOutputContaminated` (`Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs`)
- `OnOverlayAccessChanged` (`Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs`)
- `OnPalliativeAssigned` (`Assets/Ashfall.Core/SickListSystem.cs`)
- `OnPermanentMoraleBuffApplied` (`Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`)
- `OnPollinationChanged` (`Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs`)
- `OnProductionTick` (`Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs`)
- `OnProvenanceComplete` (`Assets/Ashfall.Core/Muster/ColdCountSystem.cs`)
- `OnQuestStageAdvanced` (`Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs`)
- `OnRadonAlarmTriggered` (`Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs`)
- `OnRationConfrontation` (`Assets/Ashfall.Core/Survivors/RationConflictSystem.cs`)
- `OnRationsStolen` (`Assets/Ashfall.Core/Survivors/RationConflictSystem.cs`)
- `OnReadingConfidenceChanged` (`Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`)
- `OnRegionChanged` (`Assets/Ashfall.Core/Muster/LongWalkSystem.cs`)
- `OnRelationsChanged` (`Assets/Ashfall.Core/SurvivorRelationsSystem.cs`)
- `OnReleased` (`Assets/Ashfall.Core/SickListSystem.cs`)
- `OnSafeInspected` (`Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs`)
- `OnSafetyWarning` (`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`)
- `OnSalvageRolled` (`Assets/Ashfall.Core/District8DeepCoastSystem.cs`)
- `OnShelterEncounterResolved` (`Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs`)
- `OnSiteEncounterResolved` (`Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs`)
- `OnSiteEncounterStarted` (`Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs`)
- `OnSmokeZoneChanged` (`Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`)
- `OnStatusLost` (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`)
- `OnStrategySet` (`Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs`)
- `OnStrikeResolved` (`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`)
- `OnSurvivorExposed` (`Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`)
- `OnTelemetryLost` (`Assets/Ashfall.Core/World/WeatherSondeSystem.cs`)
- `OnTelemetryReceived` (`Assets/Ashfall.Core/World/WeatherSondeSystem.cs`)
- `OnToolDamaged` (`Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs`)
- `OnTriangulationCompleted` (`Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs`)
- `OnTriangulationFailed` (`Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs`)