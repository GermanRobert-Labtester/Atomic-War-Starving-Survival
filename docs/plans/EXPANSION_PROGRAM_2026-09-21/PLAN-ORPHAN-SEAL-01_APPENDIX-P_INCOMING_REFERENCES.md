# PLAN-ORPHAN-SEAL-01 — Appendix P: Unreachable-Set Inbound Census

**Generated:** 2026-09-21 by re-running the reachability closure and classifying
**who references each orphan**: reachable files (the active game), unreachable
non-authority files (the dormant blob), or tests only.
**By construction a reachable file cannot reference an orphan by type name** —
if it did, the closure would pull the orphan in. This appendix verifies that
property (any positive case is an audit bug to fix, not a wiring lead) and then
answers the useful question: *is an orphan attached to the dormant blob, to
tests only, or to nothing at all?*
**Findings:** 99 orphan files · 2 referenced by at least
one dormant-blob file · 97 referenced by tests but nothing else ·
0 referenced by nothing anywhere (true islands).

### `AccessibilitySettingsSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `AudioAccessibilityCoordinator`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CassettePlaybackSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `BestiarySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ChemicalPlumeDispersionEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CommitmentSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `InternalCommunicationSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CommunicationsSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ContentOrphanCertificationEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CookingSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CultureCreationSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ShelterFestivalEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ShelterMuseumSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `PerimeterEarlyWarningEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `DifficultySettingsSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `FactionDiplomacySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `BlackMarketContrabandEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `BlackMarketHeatAttentionEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ChitPurityAssayEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `LoanSharkEnforcerEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `MigrationConsequenceEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `RestockAllocationEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SeasonalHumanMigrationEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorBarterSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `TradeRouteMonopolyEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `TradeRouteRiskBindingEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ApprenticeshipCurriculumEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorEducationSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `EmergencyAlertSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `InformantNetworkTradecraftEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SeasonalCelebrationSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SubterraneanSubsidenceEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `AerialReconWindowEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ColonySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `TerritoryControlSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `OilseedPressingEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SoilReclamationProfileEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SecondGenerationMilestoneEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ShelterGovernanceEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ClothingWarmthSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `FoodTypeSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CampaignLegacySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `MaritimeExplorationSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ClinicalWardTriageEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `DependencyTaperWithdrawalEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `PalliativeCareDignityEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ProstheticConditionWearEngine`

Dormant-blob references (the orphan is attached to archived work, not to the game):

| File |
|---|
| `Medical/SurvivorBodyPresentationSlate.cs` |

### `RehabilitationProgressionEngine`

Dormant-blob references (the orphan is attached to archived work, not to the game):

| File |
|---|
| `Medical/RehabilitationSlateProjection.cs` |

### `SurgicalGraftRejectionEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ModSupportSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `LetterDeliverySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `NpcMemorySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorLetterDeliverySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SleepAcousticRestEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CommonTableRationingEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `PrecisionGlassworksOpticsEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ConfessionSecretSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `PublicBroadsheetPressEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `PsychologicalProfileSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `RadioPropagationEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SessionDurabilityManager`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `OutpostSettlementSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ChemicalReagentSynthesisEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CupolaFoundryEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `DisasterResponseSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `EmergencyMusterReadinessEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `KilnFiringEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `MechanicalPowerDrivelineEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `PowerLoadSheddingEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ShelterExpansionSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ShelterIdentitySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ShelterMaintenanceSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `TrophySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SpiritualRitualCalendarEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `AgingSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `AntenatalMaternalHealthEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `BackstorySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `HobbySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `RecruitmentSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorAgingProgressionEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorAutonomySystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorRoleSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorRoutineSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `PlayableMetricsAggregationEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `GarmentLayeringThermalEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `VisitorIntegrationSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `SurvivorVoiceSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `VoiceLineDispatchCoordinator`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `VoiceLineSelectionEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `WaterQualityProfileEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `WaterSourceSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `NuclearWinterProgressionSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `WeatherCascadeSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `CascadeTargetSystem`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `ModalTravelDispatchEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `NightWatchPatrolReadinessEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `StormForecastReadinessEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `WeatherForecastReliabilityEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

### `WildlifeHarvestQuotaEngine`

No reference from any Core file outside the orphan set. Referenced only by tests, or by nothing.

