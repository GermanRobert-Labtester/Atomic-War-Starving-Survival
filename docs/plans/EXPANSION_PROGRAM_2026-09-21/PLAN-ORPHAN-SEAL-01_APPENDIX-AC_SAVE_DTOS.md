# PLAN-ORPHAN-SEAL-01 — Appendix AC: Save-State Signature Census

**Generated:** 2026-09-21. For the 56 orphans implementing
`CaptureState()`/`RestoreState()` (Appendix Z), the **actual signature types**:
what `CaptureState` returns and what `RestoreState` accepts. (An earlier draft
assumed these DTOs were named `*Save`; only one is — the real names vary, which
is itself the finding: there is no naming convention to rely on.)
**Use:** a package wiring one of these orphans starts from the existing
signature pair; where the type is defined outside the orphan's file, the
package locates the owner. A signature type defined nowhere in Core is a build
gap to report.

**56 orphans carry a capture/restore signature · 0 carry a type defined nowhere in Core (build-gap candidates).**

| Authority | CaptureState returns | RestoreState takes | Defined in same file | Elsewhere in Core |
|---|---|---|---|---|


| `AccessibilitySettingsSystem` | `AccessibilitySettingsState` | `AccessibilitySettingsState` | `AccessibilitySettingsState`, `AccessibilitySettingsState` | — |
| `CassettePlaybackSystem` | `CassettePlaybackState` | `CassettePlaybackState` | `CassettePlaybackState`, `CassettePlaybackState` | — |
| `BestiarySystem` | `BestiaryState` | `BestiaryState` | `BestiaryState`, `BestiaryState` | — |
| `CommitmentSystem` | `CommitmentSaveState` | `CommitmentSaveState` | `CommitmentSaveState`, `CommitmentSaveState` | — |
| `InternalCommunicationSystem` | `InternalCommunicationState` | `InternalCommunicationState` | `InternalCommunicationState`, `InternalCommunicationState` | — |
| `CommunicationsSystem` | `CommunicationsState` | `CommunicationsState` | `CommunicationsState`, `CommunicationsState` | — |
| `CookingSystem` | `CookingState` | `CookingState` | `CookingState`, `CookingState` | — |
| `CultureCreationSystem` | `CultureCreationState` | `CultureCreationState` | `CultureCreationState`, `CultureCreationState` | — |
| `ShelterFestivalEngine` | `FestivalPlanSaveState` | `ShelterFestivalSaveState` | `FestivalPlanSaveState`, `ShelterFestivalSaveState` | — |
| `ShelterMuseumSystem` | `ShelterMuseumState` | `ShelterMuseumState` | `ShelterMuseumState`, `ShelterMuseumState` | — |
| `PerimeterEarlyWarningEngine` | `RadarContactSaveState` | `PerimeterEarlyWarningSaveState` | `RadarContactSaveState`, `PerimeterEarlyWarningSaveState` | — |
| `DifficultySettingsSystem` | `DifficultySettingsState` | `DifficultySettingsState` | `DifficultySettingsState`, `DifficultySettingsState` | — |
| `FactionDiplomacySystem` | `FactionDiplomacyState` | `FactionDiplomacyState` | `FactionDiplomacyState`, `FactionDiplomacyState` | — |
| `BlackMarketHeatAttentionEngine` | `SyndicateHeatRecordSaveState` | `BlackMarketHeatAttentionSaveState` | `SyndicateHeatRecordSaveState`, `BlackMarketHeatAttentionSaveState` | — |
| `LoanSharkEnforcerEngine` | `LoanDebtRecordSaveState` | `LoanSharkEnforcerEngineSaveState` | `LoanDebtRecordSaveState`, `LoanSharkEnforcerEngineSaveState` | — |
| `MigrationConsequenceEngine` | `MigrationConsequenceSaveState` | `MigrationConsequenceSaveState` | `MigrationConsequenceSaveState`, `MigrationConsequenceSaveState` | — |
| `SeasonalHumanMigrationEngine` | `SeasonalMigrationSaveState` | `SeasonalMigrationSaveState` | `SeasonalMigrationSaveState`, `SeasonalMigrationSaveState` | — |
| `SurvivorBarterSystem` | `SurvivorBarterSaveState` | `SurvivorBarterSaveState` | `SurvivorBarterSaveState`, `SurvivorBarterSaveState` | — |
| `TradeRouteMonopolyEngine` | `TradeRouteMonopolySaveState` | `TradeRouteMonopolySaveState` | `TradeRouteMonopolySaveState`, `TradeRouteMonopolySaveState` | — |
| `SurvivorEducationSystem` | `EducationSystemState` | `EducationSystemState` | `EducationSystemState`, `EducationSystemState` | — |
| `EmergencyAlertSystem` | `EmergencyAlertState` | `EmergencyAlertState` | `EmergencyAlertState`, `EmergencyAlertState` | — |
| `SeasonalCelebrationSystem` | `CelebrationSaveState` | `CelebrationSaveState` | `CelebrationSaveState`, `CelebrationSaveState` | — |
| `ColonySystem` | `ColonyState` | `ColonyState` | `ColonyState`, `ColonyState` | — |
| `ShelterGovernanceEngine` | `ShelterGovernanceSaveState` | `ShelterGovernanceSaveState` | `ShelterGovernanceSaveState`, `ShelterGovernanceSaveState` | — |
| `ClothingWarmthSystem` | `ClothingWarmthSaveState` | `ClothingWarmthSaveState` | `ClothingWarmthSaveState`, `ClothingWarmthSaveState` | — |
| `FoodTypeSystem` | `FoodTypeSystemState` | `FoodTypeSystemState` | `FoodTypeSystemState`, `FoodTypeSystemState` | — |
| `CampaignLegacySystem` | `CampaignLegacyState` | `CampaignLegacyState` | `CampaignLegacyState`, `CampaignLegacyState` | — |
| `MaritimeExplorationSystem` | `MaritimeExplorationState` | `MaritimeExplorationState` | `MaritimeExplorationState`, `MaritimeExplorationState` | — |
| `SurgicalGraftRejectionEngine` | `SurgicalGraftRecordSaveState` | `SurgicalGraftRejectionEngineSaveState` | `SurgicalGraftRecordSaveState`, `SurgicalGraftRejectionEngineSaveState` | — |
| `ModSupportSystem` | `ModSaveState` | `ModSaveState` | `ModSaveState`, `ModSaveState` | — |
| `LetterDeliverySystem` | `LetterDeliverySystemState` | `LetterDeliverySystemState` | `LetterDeliverySystemState`, `LetterDeliverySystemState` | — |
| `NpcMemorySystem` | `NpcMemorySaveState` | `NpcMemorySaveState` | `NpcMemorySaveState`, `NpcMemorySaveState` | — |
| `SurvivorLetterDeliverySystem` | `SurvivorLetterDeliverySaveState` | `SurvivorLetterDeliverySaveState` | `SurvivorLetterDeliverySaveState`, `SurvivorLetterDeliverySaveState` | — |
| `ConfessionSecretSystem` | `ConfessionSecretState` | `ConfessionSecretState` | `ConfessionSecretState`, `ConfessionSecretState` | — |
| `PsychologicalProfileSystem` | `PsychologyState` | `PsychologyState` | `PsychologyState`, `PsychologyState` | — |
| `SessionDurabilityManager` | `SessionDurabilityState` | `SessionDurabilityState` | `SessionDurabilityState`, `SessionDurabilityState` | — |
| `CupolaFoundryEngine` | `CupolaFoundrySave` | `CupolaFoundrySave` | `CupolaFoundrySave`, `CupolaFoundrySave` | — |
| `DisasterResponseSystem` | `DisasterResponseState` | `DisasterResponseState` | `DisasterResponseState`, `DisasterResponseState` | — |
| `ShelterExpansionSystem` | `ShelterExpansionState` | `ShelterExpansionState` | `ShelterExpansionState`, `ShelterExpansionState` | — |
| `ShelterIdentitySystem` | `ShelterIdentityState` | `ShelterIdentityState` | `ShelterIdentityState`, `ShelterIdentityState` | — |
| `ShelterMaintenanceSystem` | `ShelterMaintenanceState` | `ShelterMaintenanceState` | `ShelterMaintenanceState`, `ShelterMaintenanceState` | — |
| `TrophySystem` | `TrophySaveState` | `TrophySaveState` | `TrophySaveState`, `TrophySaveState` | — |
| `AgingSystem` | `AgingState` | `AgingState` | `AgingState`, `AgingState` | — |
| `BackstorySystem` | `BackstoryState` | `BackstoryState` | `BackstoryState`, `BackstoryState` | — |
| `HobbySystem` | `HobbySystemState` | `HobbySystemState` | `HobbySystemState`, `HobbySystemState` | — |
| `RecruitmentSystem` | `RecruitmentState` | `RecruitmentState` | `RecruitmentState`, `RecruitmentState` | — |
| `SurvivorAutonomySystem` | `SurvivorAutonomySaveState` | `SurvivorAutonomySaveState` | `SurvivorAutonomySaveState`, `SurvivorAutonomySaveState` | — |
| `SurvivorRoleSystem` | `SurvivorRoleState` | `SurvivorRoleState` | `SurvivorRoleState`, `SurvivorRoleState` | — |
| `SurvivorRoutineSystem` | `SurvivorRoutineState` | `SurvivorRoutineState` | `SurvivorRoutineState`, `SurvivorRoutineState` | — |
| `VisitorIntegrationSystem` | `VisitorIntegrationSaveState` | `VisitorIntegrationSaveState` | `VisitorIntegrationSaveState`, `VisitorIntegrationSaveState` | — |
| `SurvivorVoiceSystem` | `SurvivorVoiceState` | `SurvivorVoiceState` | `SurvivorVoiceState`, `SurvivorVoiceState` | — |
| `VoiceLineDispatchCoordinator` | `VoiceDispatchCoordinatorSaveState` | `VoiceDispatchCoordinatorSaveState` | `VoiceDispatchCoordinatorSaveState`, `VoiceDispatchCoordinatorSaveState` | — |
| `WaterSourceSystem` | `WaterSourceSystemState` | `WaterSourceSystemState` | `WaterSourceSystemState`, `WaterSourceSystemState` | — |
| `NuclearWinterProgressionSystem` | `NuclearWinterSaveState` | `NuclearWinterSaveState` | `NuclearWinterSaveState`, `NuclearWinterSaveState` | — |
| `WeatherCascadeSystem` | `WeatherCascadeState` | `WeatherCascadeState` | — | `WeatherCascadeState`, `WeatherCascadeState` |
| `CascadeTargetSystem` | `WeatherCascadeState` | `WeatherCascadeState` | `WeatherCascadeState`, `WeatherCascadeState` | — |
