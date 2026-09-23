# PLAN-ORPHAN-SEAL-01 — Appendix AA: Time-Coupling Census

**Generated:** 2026-09-21. Which orphans expose day/hour stepping methods, so a
seal package knows whether it must attach to the campaign's day loop (Plan 33)
and with what cadence. A system with `TickDay` needs a day owner; one with only
one-shot methods may not.
**Rule:** cadence comes from the method semantics, not from a new timer; the
package cites the existing day owner when wiring.

| Authority | Day-step methods | Hour-step methods | Other step/update methods |
|---|---|---|---|

**37 orphans expose day-step methods · 2 hour-step · 14 other step/update methods.**

| `AccessibilitySettingsSystem` | — | — | — |
| `AudioAccessibilityCoordinator` | — | — | — |
| `CassettePlaybackSystem` | — | — | — |
| `BestiarySystem` | — | — | — |
| `ChemicalPlumeDispersionEngine` | — | — | `AdvancePlumeDispersion` |
| `CommitmentSystem` | `CapturePreDaySnapshot`, `RestorePreDaySnapshot`, `TickDay` | — | — |
| `InternalCommunicationSystem` | `TickDay` | — | — |
| `CommunicationsSystem` | — | — | — |
| `ContentOrphanCertificationEngine` | — | — | — |
| `CookingSystem` | — | — | — |
| `CultureCreationSystem` | — | — | — |
| `ShelterFestivalEngine` | `ProcessDailyTick` | — | — |
| `ShelterMuseumSystem` | `TickDay` | — | — |
| `PerimeterEarlyWarningEngine` | — | — | `ProcessScanSweep` |
| `DifficultySettingsSystem` | — | — | — |
| `FactionDiplomacySystem` | `TickDay` | — | — |
| `BlackMarketContrabandEngine` | — | — | — |
| `BlackMarketHeatAttentionEngine` | `ProcessDailyTick` | — | — |
| `ChitPurityAssayEngine` | — | — | — |
| `LoanSharkEnforcerEngine` | `ProcessDailyTick` | — | — |
| `MigrationConsequenceEngine` | — | — | — |
| `RestockAllocationEngine` | — | — | — |
| `SeasonalHumanMigrationEngine` | `TickDay` | — | — |
| `SurvivorBarterSystem` | `TickDay` | — | — |
| `TradeRouteMonopolyEngine` | `ProcessDailyRecovery` | — | — |
| `TradeRouteRiskBindingEngine` | — | — | — |
| `ApprenticeshipCurriculumEngine` | — | — | `AdvanceLiteracySession` |
| `SurvivorEducationSystem` | `ConductDailySession` | — | `UpdateAge` |
| `EmergencyAlertSystem` | — | `TickHour` | — |
| `InformantNetworkTradecraftEngine` | — | — | — |
| `SeasonalCelebrationSystem` | `CheckHolidayForDay` | — | — |
| `SubterraneanSubsidenceEngine` | `CalculateDailyIntegrityDecayPermille` | — | — |
| `AerialReconWindowEngine` | — | — | — |
| `ColonySystem` | `TickDay` | — | — |
| `TerritoryControlSystem` | `TickDay` | — | — |
| `OilseedPressingEngine` | — | — | — |
| `SoilReclamationProfileEngine` | — | — | — |
| `SecondGenerationMilestoneEngine` | — | — | — |
| `ShelterGovernanceEngine` | — | — | `Tick` |
| `ClothingWarmthSystem` | — | — | — |
| `FoodTypeSystem` | `TickDay` | — | — |
| `CampaignLegacySystem` | — | — | — |
| `MaritimeExplorationSystem` | — | — | — |
| `ClinicalWardTriageEngine` | — | — | — |
| `DependencyTaperWithdrawalEngine` | `AdvanceTaperDay` | — | `ComputeRecommendedStepDown` |
| `PalliativeCareDignityEngine` | `AdvanceDailyCare` | — | — |
| `ProstheticConditionWearEngine` | `EvaluateDailyWear` | — | — |
| `RehabilitationProgressionEngine` | `AdvanceDaily` | — | — |
| `SurgicalGraftRejectionEngine` | `ProcessDailyTick` | — | — |
| `ModSupportSystem` | — | — | — |
| `LetterDeliverySystem` | — | — | — |
| `NpcMemorySystem` | `TickDailyDecay` | — | — |
| `SurvivorLetterDeliverySystem` | — | — | — |
| `SleepAcousticRestEngine` | — | — | — |
| `CommonTableRationingEngine` | — | — | — |
| `PrecisionGlassworksOpticsEngine` | — | — | `AdvanceAnnealingStage` |
| `ConfessionSecretSystem` | — | — | — |
| `PublicBroadsheetPressEngine` | — | — | — |
| `PsychologicalProfileSystem` | — | — | — |
| `RadioPropagationEngine` | — | — | `UpdateMonotonicClarity` |
| `SessionDurabilityManager` | `RecordDayAdvance` | — | `RegisterOrUpdateSlot` |
| `OutpostSettlementSystem` | `TickDay` | — | — |
| `ChemicalReagentSynthesisEngine` | — | — | `EvaluateReactionStep` |
| `CupolaFoundryEngine` | `TickDay` | — | — |
| `DisasterResponseSystem` | — | — | `TickDisaster` |
| `EmergencyMusterReadinessEngine` | — | — | — |
| `KilnFiringEngine` | — | — | `AdvanceFiringStage` |
| `MechanicalPowerDrivelineEngine` | — | — | — |
| `PowerLoadSheddingEngine` | — | — | — |
| `ShelterExpansionSystem` | — | — | — |
| `ShelterIdentitySystem` | — | — | — |
| `ShelterMaintenanceSystem` | `TickDay` | — | — |
| `TrophySystem` | — | — | — |
| `SpiritualRitualCalendarEngine` | — | — | — |
| `AgingSystem` | `TickDay` | — | — |
| `AntenatalMaternalHealthEngine` | `AdvancePregnancyDay` | — | — |
| `BackstorySystem` | — | — | — |
| `HobbySystem` | — | — | — |
| `RecruitmentSystem` | `TickDay` | — | — |
| `SurvivorAgingProgressionEngine` | — | — | — |
| `SurvivorAutonomySystem` | `EvaluateDailyAutonomy` | — | `AdvanceGoal` |
| `SurvivorRoleSystem` | — | — | — |
| `SurvivorRoutineSystem` | `EvaluateDailySatisfaction` | `GetActivityAtHour` | — |
| `PlayableMetricsAggregationEngine` | — | — | — |
| `GarmentLayeringThermalEngine` | `AdvanceDailyGarmentWear` | — | — |
| `VisitorIntegrationSystem` | `TickDay` | — | — |
| `SurvivorVoiceSystem` | — | — | — |
| `VoiceLineDispatchCoordinator` | — | — | — |
| `VoiceLineSelectionEngine` | — | — | — |
| `WaterQualityProfileEngine` | — | — | — |
| `WaterSourceSystem` | `TickDay` | — | — |
| `NuclearWinterProgressionSystem` | `GetPhaseForDay`, `GetSeasonForDay`, `AdvanceDay` | — | — |
| `WeatherCascadeSystem` | `TickDay` | — | — |
| `CascadeTargetSystem` | `TickDay` | — | — |
| `ModalTravelDispatchEngine` | — | — | — |
| `NightWatchPatrolReadinessEngine` | — | — | `AdvanceWatchFatigue` |
| `StormForecastReadinessEngine` | — | — | — |
| `WeatherForecastReliabilityEngine` | — | — | — |
| `WildlifeHarvestQuotaEngine` | — | — | — |
