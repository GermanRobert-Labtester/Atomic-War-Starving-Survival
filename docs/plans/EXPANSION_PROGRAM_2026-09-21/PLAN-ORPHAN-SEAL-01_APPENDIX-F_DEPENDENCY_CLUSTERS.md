# PLAN-ORPHAN-SEAL-01 — Appendix F: Orphan Dependency Clusters & Seal Order

**Generated:** 2026-09-21. Intra-orphan reference graph over the 99
host-unreachable authorities (5 edges).
**Findings:** 94 connected clusters; 89 authorities are
isolated (no orphan-to-orphan references) and can be sealed in any order.
**Rule:** seal a cluster bottom-up along its edges — a system referenced by
another orphan is wired first (its API is the dependency); a referenced-only
authority is never wired before its consumers exist.
**Note:** an edge is a compile-time reference, not proof of a runtime
relationship; each package re-verifies the direction at the seam (EP-01).

## Clusters (largest first)

### Cluster 01 — 2 authorities

```
MigrationConsequenceEngine
SeasonalHumanMigrationEngine
```

Suggested seal order (referenced-first):

01. `MigrationConsequenceEngine` — leaf (safe first)
02. `SeasonalHumanMigrationEngine` — referenced by 1 orphan(s)

### Cluster 02 — 2 authorities

```
CommonTableRationingEngine
OilseedPressingEngine
```

Suggested seal order (referenced-first):

01. `OilseedPressingEngine` — leaf (safe first)
02. `CommonTableRationingEngine` — referenced by 1 orphan(s)

### Cluster 03 — 2 authorities

```
CupolaFoundryEngine
KilnFiringEngine
```

Suggested seal order (referenced-first):

01. `KilnFiringEngine` — leaf (safe first)
02. `CupolaFoundryEngine` — referenced by 1 orphan(s)

### Cluster 04 — 2 authorities

```
AgingSystem
SurvivorAgingProgressionEngine
```

Suggested seal order (referenced-first):

01. `AgingSystem` — leaf (safe first)
02. `SurvivorAgingProgressionEngine` — referenced by 1 orphan(s)

### Cluster 05 — 2 authorities

```
VoiceLineDispatchCoordinator
VoiceLineSelectionEngine
```

Suggested seal order (referenced-first):

01. `VoiceLineDispatchCoordinator` — leaf (safe first)
02. `VoiceLineSelectionEngine` — referenced by 1 orphan(s)

## Isolated authorities (order-free)

`AccessibilitySettingsSystem`, `AudioAccessibilityCoordinator`, `CassettePlaybackSystem`, `BestiarySystem`, `ChemicalPlumeDispersionEngine`, `CommitmentSystem`, `InternalCommunicationSystem`, `CommunicationsSystem`, `ContentOrphanCertificationEngine`, `CookingSystem`, `CultureCreationSystem`, `ShelterFestivalEngine`, `ShelterMuseumSystem`, `PerimeterEarlyWarningEngine`, `DifficultySettingsSystem`, `FactionDiplomacySystem`, `BlackMarketContrabandEngine`, `BlackMarketHeatAttentionEngine`, `ChitPurityAssayEngine`, `LoanSharkEnforcerEngine`, `RestockAllocationEngine`, `SurvivorBarterSystem`, `TradeRouteMonopolyEngine`, `TradeRouteRiskBindingEngine`, `ApprenticeshipCurriculumEngine`, `SurvivorEducationSystem`, `EmergencyAlertSystem`, `InformantNetworkTradecraftEngine`, `SeasonalCelebrationSystem`, `SubterraneanSubsidenceEngine`, `AerialReconWindowEngine`, `ColonySystem`, `TerritoryControlSystem`, `SoilReclamationProfileEngine`, `SecondGenerationMilestoneEngine`, `ShelterGovernanceEngine`, `ClothingWarmthSystem`, `FoodTypeSystem`, `CampaignLegacySystem`, `MaritimeExplorationSystem`, `ClinicalWardTriageEngine`, `DependencyTaperWithdrawalEngine`, `PalliativeCareDignityEngine`, `ProstheticConditionWearEngine`, `RehabilitationProgressionEngine`, `SurgicalGraftRejectionEngine`, `ModSupportSystem`, `LetterDeliverySystem`, `NpcMemorySystem`, `SurvivorLetterDeliverySystem`, `SleepAcousticRestEngine`, `PrecisionGlassworksOpticsEngine`, `ConfessionSecretSystem`, `PublicBroadsheetPressEngine`, `PsychologicalProfileSystem`, `RadioPropagationEngine`, `SessionDurabilityManager`, `OutpostSettlementSystem`, `ChemicalReagentSynthesisEngine`, `DisasterResponseSystem`, `EmergencyMusterReadinessEngine`, `MechanicalPowerDrivelineEngine`, `PowerLoadSheddingEngine`, `ShelterExpansionSystem`, `ShelterIdentitySystem`, `ShelterMaintenanceSystem`, `TrophySystem`, `SpiritualRitualCalendarEngine`, `AntenatalMaternalHealthEngine`, `BackstorySystem`, `HobbySystem`, `RecruitmentSystem`, `SurvivorAutonomySystem`, `SurvivorRoleSystem`, `SurvivorRoutineSystem`, `PlayableMetricsAggregationEngine`, `GarmentLayeringThermalEngine`, `VisitorIntegrationSystem`, `SurvivorVoiceSystem`, `WaterQualityProfileEngine`, `WaterSourceSystem`, `NuclearWinterProgressionSystem`, `WeatherCascadeSystem`, `CascadeTargetSystem`, `ModalTravelDispatchEngine`, `NightWatchPatrolReadinessEngine`, `StormForecastReadinessEngine`, `WeatherForecastReliabilityEngine`, `WildlifeHarvestQuotaEngine`
