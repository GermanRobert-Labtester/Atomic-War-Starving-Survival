# PLAN-ORPHAN-SEAL-01 — Appendix D: Orphan State & Save Ownership Map

**Generated:** 2026-09-21. For each of the 99 host-unreachable authorities:
does it define capture/restore-shaped methods, does it know the save-section
registry, and how many instance fields would need state registration if it is
wired stateful?
**Findings:** 58 of 99 have any capture/restore method or a
`SaveSectionRegistry` reference; **41 are stateless or
state-blind** and must not invent a save section merely to be reachable.

**Use:** O1 (state census). A system that owns campaign-visible state gets a
save row in its package before wiring; a stateless evaluator gets none. A
`—` in both columns means the package must first decide whether the system is
stateful at all (EP-01 decision), not add persistence defensively.

## Systems with capture/restore-shaped methods or save-registry knowledge

| Authority | File | Capture/restore methods | Registry refs | Fields |
|---|---|---|---:|---:|
| `AccessibilitySettingsSystem` | `Accessibility/AccessibilitySettingsSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `AudioAccessibilityCoordinator` | `Audio/AudioAccessibilityCoordinator.cs` | `LoadCatalog` | 0 | 2 |
| `CassettePlaybackSystem` | `Audio/CassettePlaybackSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 3 |
| `BestiarySystem` | `Bestiary/BestiarySystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `CommitmentSystem` | `Commitments/CommitmentSystem.cs` | `CapturePreDaySnapshot`, `RestorePreDaySnapshot`, `RestoreState` | 0 | 0 |
| `InternalCommunicationSystem` | `Communication/InternalCommunicationSystem.cs` | `LoadCatalog`, `LoadCatalog`, `RestoreState` | 0 | 1 |
| `CommunicationsSystem` | `Communications/CommunicationsSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 0 |
| `CookingSystem` | `Cooking/CookingSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 2 |
| `CultureCreationSystem` | `Culture/CultureCreationSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `ShelterFestivalEngine` | `Culture/ShelterFestivalEngine.cs` | `RestoreState` | 0 | 0 |
| `ShelterMuseumSystem` | `Culture/ShelterMuseumSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `PerimeterEarlyWarningEngine` | `Defense/PerimeterEarlyWarningEngine.cs` | `RestoreState` | 0 | 0 |
| `DifficultySettingsSystem` | `Difficulty/DifficultySettingsSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 2 |
| `FactionDiplomacySystem` | `Diplomacy/FactionDiplomacySystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `BlackMarketHeatAttentionEngine` | `Economy/BlackMarketHeatAttentionEngine.cs` | `RestoreState` | 0 | 0 |
| `LoanSharkEnforcerEngine` | `Economy/LoanSharkEnforcerEngine.cs` | `RestoreState` | 0 | 0 |
| `MigrationConsequenceEngine` | `Economy/MigrationConsequenceEngine.cs` | `RestoreState` | 0 | 1 |
| `SeasonalHumanMigrationEngine` | `Economy/SeasonalHumanMigrationEngine.cs` | `RestoreState` | 0 | 1 |
| `SurvivorBarterSystem` | `Economy/SurvivorBarterSystem.cs` | `LoadCatalog`, `LoadCatalog`, `RestoreState` | 0 | 1 |
| `TradeRouteMonopolyEngine` | `Economy/TradeRouteMonopolyEngine.cs` | `RestoreState` | 0 | 0 |
| `SurvivorEducationSystem` | `Education/SurvivorEducationSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 0 |
| `EmergencyAlertSystem` | `Emergency/EmergencyAlertSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `SeasonalCelebrationSystem` | `Events/SeasonalCelebrationSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `ColonySystem` | `Expeditions/ColonySystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `TerritoryControlSystem` | `Factions/TerritoryControlSystem.cs` | `RestoreSupplyLine` | 0 | 0 |
| `ShelterGovernanceEngine` | `Governance/ShelterGovernanceEngine.cs` | `LoadCatalog`, `RestoreState` | 0 | 8 |
| `ClothingWarmthSystem` | `Inventory/ClothingWarmthSystem.cs` | `RestoreState` | 0 | 2 |
| `FoodTypeSystem` | `Kitchen/FoodTypeSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `CampaignLegacySystem` | `Legacy/CampaignLegacySystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `MaritimeExplorationSystem` | `Maritime/MaritimeExplorationSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `SurgicalGraftRejectionEngine` | `Medical/SurgicalGraftRejectionEngine.cs` | `RestoreState` | 0 | 0 |
| `ModSupportSystem` | `Mods/ModDataContract.cs` | `LoadSpecification`, `RestoreState` | 0 | 0 |
| `LetterDeliverySystem` | `Narrative/LetterDeliverySystem.cs` | `RestoreState` | 0 | 1 |
| `NpcMemorySystem` | `Narrative/NpcMemorySystem.cs` | `LoadDialogueCatalog`, `RestoreState` | 0 | 0 |
| `SurvivorLetterDeliverySystem` | `Narrative/SurvivorLetterDeliverySystem.cs` | `RestoreState` | 0 | 2 |
| `ConfessionSecretSystem` | `Phantoms/ConfessionSecretSystem.cs` | `RestoreState` | 0 | 3 |
| `PsychologicalProfileSystem` | `Psychology/PsychologicalProfileSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `SessionDurabilityManager` | `Save/SessionDurabilityManager.cs` | `RestoreState` | 0 | 1 |
| `CupolaFoundryEngine` | `Shelter/CupolaFoundryEngine.cs` | `RestoreState` | 0 | 18 |
| `DisasterResponseSystem` | `Shelter/DisasterResponseSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 0 |
| `ShelterExpansionSystem` | `Shelter/ShelterExpansionSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 0 |
| `ShelterIdentitySystem` | `Shelter/ShelterIdentitySystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 8 |
| `ShelterMaintenanceSystem` | `Shelter/ShelterMaintenanceSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `TrophySystem` | `Shelter/TrophySystem.cs` | `LoadCatalogFromJson`, `RestoreState` | 0 | 0 |
| `AgingSystem` | `Survivors/AgingSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `BackstorySystem` | `Survivors/BackstorySystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `HobbySystem` | `Survivors/HobbySystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `RecruitmentSystem` | `Survivors/RecruitmentSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `SurvivorAutonomySystem` | `Survivors/SurvivorAutonomySystem.cs` | `LoadCatalog`, `LoadCatalog`, `RestoreState` | 0 | 4 |
| `SurvivorRoleSystem` | `Survivors/SurvivorRoleSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `SurvivorRoutineSystem` | `Survivors/SurvivorRoutineSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `VisitorIntegrationSystem` | `Visitors/VisitorIntegrationSystem.cs` | `LoadCatalog`, `LoadCatalog`, `RestoreState` | 0 | 1 |
| `SurvivorVoiceSystem` | `Voice/SurvivorVoiceSystem.cs` | `LoadCatalog`, `LoadCatalog`, `RestoreState` | 0 | 1 |
| `VoiceLineDispatchCoordinator` | `Voice/VoiceLineDispatchCoordinator.cs` | `RestoreState` | 0 | 0 |
| `WaterSourceSystem` | `Water/WaterSourceSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `NuclearWinterProgressionSystem` | `Weather/NuclearWinterProgressionSystem.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |
| `WeatherCascadeSystem` | `Weather/WeatherCascadeSystem.cs` | `RestoreState` | 0 | 1 |
| `CascadeTargetSystem` | `Weather/WeatherGameplayCascadeEngine.cs` | `LoadCatalog`, `RestoreState` | 0 | 1 |

## Stateless or state-blind systems (no save path found)

| Authority | File | Fields |
|---|---|---:|
| `ChemicalPlumeDispersionEngine` | `Combat/ChemicalPlumeDispersionEngine.cs` | 0 |
| `ContentOrphanCertificationEngine` | `Content/ContentOrphanCertificationEngine.cs` | 0 |
| `BlackMarketContrabandEngine` | `Economy/BlackMarketContrabandEngine.cs` | 0 |
| `ChitPurityAssayEngine` | `Economy/ChitPurityAssayEngine.cs` | 0 |
| `RestockAllocationEngine` | `Economy/RestockAllocationEngine.cs` | 0 |
| `TradeRouteRiskBindingEngine` | `Economy/TradeRouteRiskBindingEngine.cs` | 0 |
| `ApprenticeshipCurriculumEngine` | `Education/ApprenticeshipCurriculumEngine.cs` | 0 |
| `InformantNetworkTradecraftEngine` | `Espionage/InformantNetworkTradecraftEngine.cs` | 0 |
| `SubterraneanSubsidenceEngine` | `Excavation/SubterraneanSubsidenceEngine.cs` | 0 |
| `AerialReconWindowEngine` | `Expeditions/AerialReconWindowEngine.cs` | 0 |
| `OilseedPressingEngine` | `Farming/OilseedPressingEngine.cs` | 0 |
| `SoilReclamationProfileEngine` | `Farming/SoilReclamationProfileEngine.cs` | 0 |
| `SecondGenerationMilestoneEngine` | `Generations/SecondGenerationMilestoneEngine.cs` | 0 |
| `ClinicalWardTriageEngine` | `Medical/ClinicalWardTriageEngine.cs` | 0 |
| `DependencyTaperWithdrawalEngine` | `Medical/DependencyTaperWithdrawalEngine.cs` | 0 |
| `PalliativeCareDignityEngine` | `Medical/PalliativeCareDignityEngine.cs` | 0 |
| `ProstheticConditionWearEngine` | `Medical/ProstheticConditionWearEngine.cs` | 0 |
| `RehabilitationProgressionEngine` | `Medical/RehabilitationProgressionEngine.cs` | 0 |
| `SleepAcousticRestEngine` | `Needs/SleepAcousticRestEngine.cs` | 0 |
| `CommonTableRationingEngine` | `Nutrition/CommonTableRationingEngine.cs` | 0 |
| `PrecisionGlassworksOpticsEngine` | `Optics/PrecisionGlassworksOpticsEngine.cs` | 0 |
| `PublicBroadsheetPressEngine` | `Print/PublicBroadsheetPressEngine.cs` | 0 |
| `RadioPropagationEngine` | `Radio/RadioPropagation.cs` | 0 |
| `OutpostSettlementSystem` | `Settlements/OutpostSettlementSystem.cs` | 0 |
| `ChemicalReagentSynthesisEngine` | `Shelter/ChemicalReagentSynthesisEngine.cs` | 0 |
| `EmergencyMusterReadinessEngine` | `Shelter/EmergencyMusterReadinessEngine.cs` | 0 |
| `KilnFiringEngine` | `Shelter/KilnFiringEngine.cs` | 0 |
| `MechanicalPowerDrivelineEngine` | `Shelter/MechanicalPowerDrivelineEngine.cs` | 0 |
| `PowerLoadSheddingEngine` | `Shelter/PowerLoadSheddingEngine.cs` | 0 |
| `SpiritualRitualCalendarEngine` | `Spiritual/SpiritualRitualCalendarEngine.cs` | 0 |
| `AntenatalMaternalHealthEngine` | `Survivors/AntenatalMaternalHealthEngine.cs` | 0 |
| `SurvivorAgingProgressionEngine` | `Survivors/SurvivorAgingProgressionEngine.cs` | 0 |
| `PlayableMetricsAggregationEngine` | `Telemetry/PlayableMetricsAggregationEngine.cs` | 0 |
| `GarmentLayeringThermalEngine` | `Textiles/GarmentLayeringThermalEngine.cs` | 0 |
| `VoiceLineSelectionEngine` | `Voice/VoiceLineSelectionEngine.cs` | 0 |
| `WaterQualityProfileEngine` | `Water/WaterQualityProfileEngine.cs` | 0 |
| `ModalTravelDispatchEngine` | `World/ModalTravelDispatchEngine.cs` | 0 |
| `NightWatchPatrolReadinessEngine` | `World/NightWatchPatrolReadinessEngine.cs` | 0 |
| `StormForecastReadinessEngine` | `World/StormForecastReadinessEngine.cs` | 0 |
| `WeatherForecastReliabilityEngine` | `World/WeatherForecastReliabilityEngine.cs` | 0 |
| `WildlifeHarvestQuotaEngine` | `World/WildlifeHarvestQuotaEngine.cs` | 0 |
