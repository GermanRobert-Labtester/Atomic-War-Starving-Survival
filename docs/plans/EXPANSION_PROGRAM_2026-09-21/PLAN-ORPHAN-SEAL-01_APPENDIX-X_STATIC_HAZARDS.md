# PLAN-ORPHAN-SEAL-01 — Appendix X: Static-State & Singleton Census

**Generated:** 2026-09-21. Static **fields** and singleton-named members per
orphan, hazard-flagged. The host-session pattern (Appendix C) expects state to
belong to a session object; mutable statics and shared caches are the most
common source of cross-save leakage when a system is wired.
**Flags:** `S` singleton-named member · `M` mutable static field (non-readonly,
non-const) · `C` readonly static collection field (cache-shaped).
Methods are excluded: a static method returning a collection is not state.
**Use:** a package treats flagged members as pre-work — convert to session
state or prove the static is a constant lookup.

| Authority | Static fields | Flags |
|---|---|---|

**1 of 99 orphans carry at least one flagged static.**

| `AccessibilitySettingsSystem` | — | — |
| `AudioAccessibilityCoordinator` | — | — |
| `CassettePlaybackSystem` | — | — |
| `BestiarySystem` | — | — |
| `ChemicalPlumeDispersionEngine` | — | — |
| `CommitmentSystem` | — | — |
| `InternalCommunicationSystem` | — | — |
| `CommunicationsSystem` | — | — |
| `ContentOrphanCertificationEngine` | — | — |
| `CookingSystem` | — | — |
| `CultureCreationSystem` | — | — |
| `ShelterFestivalEngine` | — | — |
| `ShelterMuseumSystem` | — | — |
| `PerimeterEarlyWarningEngine` | — | — |
| `DifficultySettingsSystem` | — | — |
| `FactionDiplomacySystem` | — | — |
| `BlackMarketContrabandEngine` | — | — |
| `BlackMarketHeatAttentionEngine` | `PossibleLocations` | — |
| `ChitPurityAssayEngine` | — | — |
| `LoanSharkEnforcerEngine` | — | — |
| `MigrationConsequenceEngine` | — | — |
| `RestockAllocationEngine` | — | — |
| `SeasonalHumanMigrationEngine` | — | — |
| `SurvivorBarterSystem` | — | — |
| `TradeRouteMonopolyEngine` | — | — |
| `TradeRouteRiskBindingEngine` | — | — |
| `ApprenticeshipCurriculumEngine` | — | — |
| `SurvivorEducationSystem` | — | — |
| `EmergencyAlertSystem` | — | — |
| `InformantNetworkTradecraftEngine` | — | — |
| `SeasonalCelebrationSystem` | — | — |
| `SubterraneanSubsidenceEngine` | — | — |
| `AerialReconWindowEngine` | — | — |
| `ColonySystem` | — | — |
| `TerritoryControlSystem` | — | — |
| `OilseedPressingEngine` | — | — |
| `SoilReclamationProfileEngine` | — | — |
| `SecondGenerationMilestoneEngine` | — | — |
| `ShelterGovernanceEngine` | — | — |
| `ClothingWarmthSystem` | — | — |
| `FoodTypeSystem` | — | — |
| `CampaignLegacySystem` | — | — |
| `MaritimeExplorationSystem` | — | — |
| `ClinicalWardTriageEngine` | — | — |
| `DependencyTaperWithdrawalEngine` | — | — |
| `PalliativeCareDignityEngine` | — | — |
| `ProstheticConditionWearEngine` | — | — |
| `RehabilitationProgressionEngine` | — | — |
| `SurgicalGraftRejectionEngine` | — | — |
| `ModSupportSystem` | `ModIdRegex` | — |
| `LetterDeliverySystem` | — | — |
| `NpcMemorySystem` | — | — |
| `SurvivorLetterDeliverySystem` | — | — |
| `SleepAcousticRestEngine` | — | — |
| `CommonTableRationingEngine` | — | — |
| `PrecisionGlassworksOpticsEngine` | — | — |
| `ConfessionSecretSystem` | — | — |
| `PublicBroadsheetPressEngine` | — | — |
| `PsychologicalProfileSystem` | — | — |
| `RadioPropagationEngine` | — | — |
| `SessionDurabilityManager` | — | — |
| `OutpostSettlementSystem` | — | — |
| `ChemicalReagentSynthesisEngine` | — | — |
| `CupolaFoundryEngine` | — | — |
| `DisasterResponseSystem` | — | — |
| `EmergencyMusterReadinessEngine` | — | — |
| `KilnFiringEngine` | — | — |
| `MechanicalPowerDrivelineEngine` | — | — |
| `PowerLoadSheddingEngine` | — | — |
| `ShelterExpansionSystem` | — | — |
| `ShelterIdentitySystem` | — | — |
| `ShelterMaintenanceSystem` | — | — |
| `TrophySystem` | — | — |
| `SpiritualRitualCalendarEngine` | `CanonicalHolyDays` | — |
| `AgingSystem` | — | — |
| `AntenatalMaternalHealthEngine` | — | — |
| `BackstorySystem` | — | — |
| `HobbySystem` | — | — |
| `RecruitmentSystem` | — | — |
| `SurvivorAgingProgressionEngine` | — | — |
| `SurvivorAutonomySystem` | — | — |
| `SurvivorRoleSystem` | — | — |
| `SurvivorRoutineSystem` | — | — |
| `PlayableMetricsAggregationEngine` | — | — |
| `GarmentLayeringThermalEngine` | — | — |
| `VisitorIntegrationSystem` | — | — |
| `SurvivorVoiceSystem` | — | — |
| `VoiceLineDispatchCoordinator` | — | — |
| `VoiceLineSelectionEngine` | `None` | M:None |
| `WaterQualityProfileEngine` | — | — |
| `WaterSourceSystem` | — | — |
| `NuclearWinterProgressionSystem` | — | — |
| `WeatherCascadeSystem` | — | — |
| `CascadeTargetSystem` | — | — |
| `ModalTravelDispatchEngine` | — | — |
| `NightWatchPatrolReadinessEngine` | — | — |
| `StormForecastReadinessEngine` | — | — |
| `WeatherForecastReliabilityEngine` | — | — |
| `WildlifeHarvestQuotaEngine` | — | — |
