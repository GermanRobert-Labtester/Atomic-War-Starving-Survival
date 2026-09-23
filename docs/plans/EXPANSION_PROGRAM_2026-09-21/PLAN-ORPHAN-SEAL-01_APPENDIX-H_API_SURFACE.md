# PLAN-ORPHAN-SEAL-01 — Appendix H: API Surface & Complexity Census

**Generated:** 2026-09-21. Per orphan file: line count, public method count,
public property count, constructor count, static member count, and declared
base type or interfaces. Used to size seal packages (a 2,000-line engine with
no ctor is not a small package) and to spot static state that fights the host
session pattern.

| Authority | Lines | Public methods | Public props | Ctors | Static members | Base/interfaces |
|---|---:|---:|---:|---:|---:|---|
| `AccessibilitySettingsSystem` | 261 | 15 | 32 | 2 | 0 | — |
| `AudioAccessibilityCoordinator` | 294 | 5 | 29 | 1 | 0 | — |
| `CassettePlaybackSystem` | 237 | 12 | 0 | 1 | 0 | — |
| `BestiarySystem` | 285 | 13 | 18 | 2 | 1 | — |
| `ChemicalPlumeDispersionEngine` | 286 | 4 | 18 | 0 | 4 | — |
| `CommitmentSystem` | 314 | 10 | 0 | 1 | 0 | IDayAdvanceOwner, IPreDaySnapshotRestore |
| `InternalCommunicationSystem` | 509 | 17 | 36 | 1 | 0 | — |
| `CommunicationsSystem` | 561 | 21 | 44 | 1 | 0 | — |
| `ContentOrphanCertificationEngine` | 136 | 1 | 11 | 0 | 2 | — |
| `CookingSystem` | 395 | 12 | 39 | 1 | 2 | — |
| `CultureCreationSystem` | 287 | 8 | 27 | 1 | 0 | — |
| `ShelterFestivalEngine` | 323 | 7 | 25 | 0 | 0 | — |
| `ShelterMuseumSystem` | 539 | 18 | 44 | 1 | 0 | — |
| `PerimeterEarlyWarningEngine` | 280 | 7 | 20 | 0 | 0 | — |
| `DifficultySettingsSystem` | 215 | 7 | 5 | 2 | 0 | — |
| `FactionDiplomacySystem` | 451 | 17 | 56 | 1 | 2 | — |
| `BlackMarketContrabandEngine` | 267 | 6 | 12 | 0 | 8 | — |
| `BlackMarketHeatAttentionEngine` | 289 | 10 | 22 | 0 | 1 | — |
| `ChitPurityAssayEngine` | 228 | 2 | 11 | 0 | 5 | — |
| `LoanSharkEnforcerEngine` | 442 | 10 | 36 | 1 | 0 | — |
| `MigrationConsequenceEngine` | 144 | 7 | 2 | 1 | 0 | — |
| `RestockAllocationEngine` | 245 | 1 | 12 | 0 | 2 | — |
| `SeasonalHumanMigrationEngine` | 162 | 4 | 14 | 1 | 0 | — |
| `SurvivorBarterSystem` | 666 | 15 | 49 | 1 | 0 | — |
| `TradeRouteMonopolyEngine` | 206 | 10 | 10 | 0 | 2 | — |
| `TradeRouteRiskBindingEngine` | 130 | 1 | 7 | 0 | 2 | — |
| `ApprenticeshipCurriculumEngine` | 219 | 4 | 8 | 0 | 4 | — |
| `SurvivorEducationSystem` | 579 | 14 | 44 | 1 | 0 | — |
| `EmergencyAlertSystem` | 355 | 14 | 30 | 2 | 0 | — |
| `InformantNetworkTradecraftEngine` | 222 | 4 | 18 | 0 | 4 | — |
| `SeasonalCelebrationSystem` | 365 | 7 | 35 | 1 | 0 | — |
| `SubterraneanSubsidenceEngine` | 299 | 5 | 17 | 0 | 6 | — |
| `AerialReconWindowEngine` | 200 | 1 | 10 | 0 | 2 | — |
| `ColonySystem` | 517 | 12 | 47 | 1 | 0 | — |
| `TerritoryControlSystem` | 451 | 13 | 34 | 1 | 3 | — |
| `OilseedPressingEngine` | 246 | 4 | 14 | 0 | 5 | — |
| `SoilReclamationProfileEngine` | 216 | 1 | 9 | 0 | 2 | — |
| `SecondGenerationMilestoneEngine` | 229 | 2 | 6 | 0 | 4 | — |
| `ShelterGovernanceEngine` | 669 | 13 | 0 | 1 | 0 | — |
| `ClothingWarmthSystem` | 390 | 10 | 0 | 1 | 0 | — |
| `FoodTypeSystem` | 267 | 14 | 20 | 2 | 0 | — |
| `CampaignLegacySystem` | 305 | 8 | 31 | 1 | 3 | — |
| `MaritimeExplorationSystem` | 743 | 13 | 57 | 1 | 1 | — |
| `ClinicalWardTriageEngine` | 307 | 5 | 18 | 0 | 5 | — |
| `DependencyTaperWithdrawalEngine` | 268 | 5 | 12 | 0 | 5 | — |
| `PalliativeCareDignityEngine` | 254 | 4 | 18 | 0 | 4 | — |
| `ProstheticConditionWearEngine` | 157 | 1 | 6 | 0 | 2 | — |
| `RehabilitationProgressionEngine` | 112 | 3 | 0 | 0 | 4 | — |
| `SurgicalGraftRejectionEngine` | 284 | 9 | 26 | 0 | 1 | — |
| `ModSupportSystem` | 506 | 9 | 30 | 1 | 1 | — |
| `LetterDeliverySystem` | 200 | 8 | 0 | 1 | 0 | — |
| `NpcMemorySystem` | 463 | 13 | 22 | 0 | 0 | — |
| `SurvivorLetterDeliverySystem` | 285 | 12 | 4 | 1 | 1 | — |
| `SleepAcousticRestEngine` | 230 | 5 | 14 | 0 | 5 | — |
| `CommonTableRationingEngine` | 298 | 2 | 10 | 0 | 3 | — |
| `PrecisionGlassworksOpticsEngine` | 302 | 5 | 13 | 0 | 5 | — |
| `ConfessionSecretSystem` | 288 | 10 | 0 | 1 | 0 | — |
| `PublicBroadsheetPressEngine` | 272 | 4 | 12 | 0 | 4 | — |
| `PsychologicalProfileSystem` | 355 | 12 | 41 | 1 | 0 | — |
| `RadioPropagationEngine` | 233 | 6 | 15 | 0 | 7 | — |
| `SessionDurabilityManager` | 365 | 11 | 26 | 1 | 0 | — |
| `OutpostSettlementSystem` | 318 | 12 | 18 | 1 | 1 | — |
| `ChemicalReagentSynthesisEngine` | 276 | 5 | 17 | 0 | 5 | — |
| `CupolaFoundryEngine` | 445 | 8 | 0 | 1 | 0 | — |
| `DisasterResponseSystem` | 457 | 14 | 30 | 1 | 0 | — |
| `EmergencyMusterReadinessEngine` | 192 | 1 | 7 | 0 | 2 | — |
| `KilnFiringEngine` | 241 | 4 | 11 | 0 | 4 | — |
| `MechanicalPowerDrivelineEngine` | 278 | 5 | 18 | 0 | 5 | — |
| `PowerLoadSheddingEngine` | 212 | 1 | 12 | 0 | 2 | — |
| `ShelterExpansionSystem` | 665 | 17 | 51 | 1 | 0 | — |
| `ShelterIdentitySystem` | 401 | 15 | 0 | 1 | 0 | — |
| `ShelterMaintenanceSystem` | 306 | 11 | 27 | 2 | 0 | — |
| `TrophySystem` | 411 | 12 | 20 | 1 | 0 | — |
| `SpiritualRitualCalendarEngine` | 182 | 3 | 11 | 0 | 5 | — |
| `AgingSystem` | 304 | 11 | 30 | 2 | 0 | — |
| `AntenatalMaternalHealthEngine` | 321 | 5 | 17 | 0 | 5 | — |
| `BackstorySystem` | 364 | 14 | 47 | 2 | 0 | — |
| `HobbySystem` | 344 | 9 | 24 | 1 | 1 | — |
| `RecruitmentSystem` | 367 | 12 | 56 | 1 | 0 | — |
| `SurvivorAgingProgressionEngine` | 203 | 5 | 10 | 0 | 6 | — |
| `SurvivorAutonomySystem` | 667 | 10 | 38 | 1 | 7 | — |
| `SurvivorRoleSystem` | 321 | 12 | 21 | 2 | 0 | — |
| `SurvivorRoutineSystem` | 527 | 15 | 48 | 2 | 0 | — |
| `PlayableMetricsAggregationEngine` | 186 | 1 | 15 | 0 | 2 | — |
| `GarmentLayeringThermalEngine` | 230 | 4 | 10 | 0 | 4 | — |
| `VisitorIntegrationSystem` | 611 | 18 | 44 | 1 | 0 | — |
| `SurvivorVoiceSystem` | 316 | 6 | 30 | 1 | 0 | — |
| `VoiceLineDispatchCoordinator` | 253 | 8 | 19 | 0 | 2 | — |
| `VoiceLineSelectionEngine` | 185 | 2 | 23 | 0 | 3 | — |
| `WaterQualityProfileEngine` | 203 | 3 | 9 | 0 | 4 | — |
| `WaterSourceSystem` | 434 | 16 | 38 | 2 | 0 | — |
| `NuclearWinterProgressionSystem` | 472 | 10 | 40 | 1 | 0 | — |
| `WeatherCascadeSystem` | 78 | 5 | 2 | 1 | 0 | — |
| `CascadeTargetSystem` | 438 | 6 | 24 | 0 | 6 | — |
| `ModalTravelDispatchEngine` | 200 | 2 | 9 | 0 | 4 | — |
| `NightWatchPatrolReadinessEngine` | 279 | 5 | 8 | 0 | 6 | — |
| `StormForecastReadinessEngine` | 276 | 3 | 9 | 0 | 4 | — |
| `WeatherForecastReliabilityEngine` | 146 | 1 | 5 | 0 | 3 | — |
| `WildlifeHarvestQuotaEngine` | 268 | 3 | 7 | 0 | 4 | — |
