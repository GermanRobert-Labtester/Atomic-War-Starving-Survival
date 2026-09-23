# PLAN-ORPHAN-SEAL-01 — Appendix O: Verification Command Set

**Generated:** 2026-09-21. For each orphan, the **concrete focused commands a
seal package can cite** in its acceptance section: an existing test region (if
any) and an existing headless selftest flag whose name matches the domain
(109 test regions and 218 `--*-selftest` flags currently exist).
**Use:** a package that cannot cite a command runs its own focused test block
per `TEST_POLICY.md`; this table prevents inventing commands that do not exist.
**Note:** a flag match is name-level; the package confirms the flag's current
behavior before citing it (Plan 23's selftest truth).

| Authority | Test region | Headless flag candidate | Suggested focused command |
|---|---|---|---|
| `AccessibilitySettingsSystem` | `Ashfall.Core.Tests/Accessibility/` | `--accessibility-selftest`, `--settings-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/` |
| `AudioAccessibilityCoordinator` | `Ashfall.Core.Tests/Accessibility/` | `--accessibility-selftest`, `--audio-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/` |
| `CassettePlaybackSystem` | — | — | new focused block required |
| `BestiarySystem` | `Ashfall.Core.Tests/Bestiary/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Bestiary/` |
| `ChemicalPlumeDispersionEngine` | — | `--chemical-dependency-save-selftest`, `--chemical-recon-selftest` | `godot --headless --path . -- --chemical-dependency-save-selftest` |
| `CommitmentSystem` | — | — | new focused block required |
| `InternalCommunicationSystem` | `Ashfall.Core.Tests/Communication/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Communication/` |
| `CommunicationsSystem` | `Ashfall.Core.Tests/Communications/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Communications/` |
| `ContentOrphanCertificationEngine` | `Ashfall.Core.Tests/Content/` | `--content-utilization-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Content/` |
| `CookingSystem` | `Ashfall.Core.Tests/Cooking/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Cooking/` |
| `CultureCreationSystem` | `Ashfall.Core.Tests/Culture/` | `--agriculture-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/` |
| `ShelterFestivalEngine` | `Ashfall.Core.Tests/Shelter/` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `ShelterMuseumSystem` | `Ashfall.Core.Tests/Shelter/` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `PerimeterEarlyWarningEngine` | — | — | new focused block required |
| `DifficultySettingsSystem` | `Ashfall.Core.Tests/Difficulty/` | `--difficulty-selftest`, `--settings-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/` |
| `FactionDiplomacySystem` | `Ashfall.Core.Tests/Diplomacy/` | `--faction-communique-board-selftest`, `--faction-ecology-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Diplomacy/` |
| `BlackMarketContrabandEngine` | — | `--black-flotilla-selftest`, `--contraband-selftest` | `godot --headless --path . -- --black-flotilla-selftest` |
| `BlackMarketHeatAttentionEngine` | — | `--black-flotilla-selftest` | `godot --headless --path . -- --black-flotilla-selftest` |
| `ChitPurityAssayEngine` | — | — | new focused block required |
| `LoanSharkEnforcerEngine` | — | — | new focused block required |
| `MigrationConsequenceEngine` | `Ashfall.Core.Tests/NarrativeConsequence/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeConsequence/` |
| `RestockAllocationEngine` | — | — | new focused block required |
| `SeasonalHumanMigrationEngine` | — | — | new focused block required |
| `SurvivorBarterSystem` | `Ashfall.Core.Tests/Survivors/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `TradeRouteMonopolyEngine` | — | `--deep-coast-route-selftest`, `--holdfast-trade-save-selftest` | `godot --headless --path . -- --deep-coast-route-selftest` |
| `TradeRouteRiskBindingEngine` | — | `--deep-coast-route-selftest`, `--holdfast-trade-save-selftest` | `godot --headless --path . -- --deep-coast-route-selftest` |
| `ApprenticeshipCurriculumEngine` | — | — | new focused block required |
| `SurvivorEducationSystem` | `Ashfall.Core.Tests/Education/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Education/` |
| `EmergencyAlertSystem` | `Ashfall.Core.Tests/Emergency/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Emergency/` |
| `InformantNetworkTradecraftEngine` | — | `--rumor-network-selftest` | `godot --headless --path . -- --rumor-network-selftest` |
| `SeasonalCelebrationSystem` | — | — | new focused block required |
| `SubterraneanSubsidenceEngine` | — | — | new focused block required |
| `AerialReconWindowEngine` | — | `--advanced-industrial-recon-selftest`, `--chemical-recon-selftest` | `godot --headless --path . -- --advanced-industrial-recon-selftest` |
| `ColonySystem` | — | — | new focused block required |
| `TerritoryControlSystem` | — | — | new focused block required |
| `OilseedPressingEngine` | — | — | new focused block required |
| `SoilReclamationProfileEngine` | — | `--starting-profile-selftest` | `godot --headless --path . -- --starting-profile-selftest` |
| `SecondGenerationMilestoneEngine` | `Ashfall.Core.Tests/Generations/` | `--day1-to-day2-milestone-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Generations/` |
| `ShelterGovernanceEngine` | `Ashfall.Core.Tests/Governance/` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/` |
| `ClothingWarmthSystem` | — | — | new focused block required |
| `FoodTypeSystem` | — | — | new focused block required |
| `CampaignLegacySystem` | `Ashfall.Core.Tests/Campaign/` | `--campaign-fuzz-selftest`, `--campaign-journey-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/` |
| `MaritimeExplorationSystem` | `Ashfall.Core.Tests/Exploration/` | `--maritime-selftest`, `--world-exploration-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Exploration/` |
| `ClinicalWardTriageEngine` | — | `--medical-ward-save-selftest` | `godot --headless --path . -- --medical-ward-save-selftest` |
| `DependencyTaperWithdrawalEngine` | — | `--chemical-dependency-save-selftest` | `godot --headless --path . -- --chemical-dependency-save-selftest` |
| `PalliativeCareDignityEngine` | — | — | new focused block required |
| `ProstheticConditionWearEngine` | — | — | new focused block required |
| `RehabilitationProgressionEngine` | `Ashfall.Core.Tests/Progression/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/` |
| `SurgicalGraftRejectionEngine` | — | — | new focused block required |
| `ModSupportSystem` | — | — | new focused block required |
| `LetterDeliverySystem` | — | — | new focused block required |
| `NpcMemorySystem` | — | — | new focused block required |
| `SurvivorLetterDeliverySystem` | `Ashfall.Core.Tests/Survivors/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `SleepAcousticRestEngine` | — | — | new focused block required |
| `CommonTableRationingEngine` | — | — | new focused block required |
| `PrecisionGlassworksOpticsEngine` | `Ashfall.Core.Tests/Optics/` | `--precision-metrology-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Optics/` |
| `ConfessionSecretSystem` | — | — | new focused block required |
| `PublicBroadsheetPressEngine` | — | — | new focused block required |
| `PsychologicalProfileSystem` | — | `--starting-profile-selftest` | `godot --headless --path . -- --starting-profile-selftest` |
| `RadioPropagationEngine` | `Ashfall.Core.Tests/Radio/` | `--radio-catalog-selftest`, `--radio-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` |
| `SessionDurabilityManager` | — | — | new focused block required |
| `OutpostSettlementSystem` | `Ashfall.Core.Tests/Settlements/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Settlements/` |
| `ChemicalReagentSynthesisEngine` | — | `--chemical-dependency-save-selftest`, `--chemical-recon-selftest` | `godot --headless --path . -- --chemical-dependency-save-selftest` |
| `CupolaFoundryEngine` | `Ashfall.Core.Tests/Foundry/` | `--silent-foundry-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/` |
| `DisasterResponseSystem` | — | — | new focused block required |
| `EmergencyMusterReadinessEngine` | `Ashfall.Core.Tests/Emergency/` | `--muster-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Emergency/` |
| `KilnFiringEngine` | — | — | new focused block required |
| `MechanicalPowerDrivelineEngine` | — | `--power-grid-catalog-selftest`, `--sofc-power-selftest` | `godot --headless --path . -- --power-grid-catalog-selftest` |
| `PowerLoadSheddingEngine` | — | `--power-grid-catalog-selftest`, `--save-load-failure-selftest` | `godot --headless --path . -- --power-grid-catalog-selftest` |
| `ShelterExpansionSystem` | `Ashfall.Core.Tests/Expansions/` | `--all-expansions-selftest`, `--disease-expansion-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Expansions/` |
| `ShelterIdentitySystem` | `Ashfall.Core.Tests/Shelter/` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `ShelterMaintenanceSystem` | `Ashfall.Core.Tests/Shelter/` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `TrophySystem` | — | — | new focused block required |
| `SpiritualRitualCalendarEngine` | `Ashfall.Core.Tests/Spiritual/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Spiritual/` |
| `AgingSystem` | — | — | new focused block required |
| `AntenatalMaternalHealthEngine` | — | — | new focused block required |
| `BackstorySystem` | — | — | new focused block required |
| `HobbySystem` | — | — | new focused block required |
| `RecruitmentSystem` | — | — | new focused block required |
| `SurvivorAgingProgressionEngine` | `Ashfall.Core.Tests/Progression/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/` |
| `SurvivorAutonomySystem` | `Ashfall.Core.Tests/Survivors/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `SurvivorRoleSystem` | `Ashfall.Core.Tests/Survivors/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `SurvivorRoutineSystem` | `Ashfall.Core.Tests/Survivors/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `PlayableMetricsAggregationEngine` | — | `--day1-playable-selftest`, `--playable-loop-selftest` | `godot --headless --path . -- --day1-playable-selftest` |
| `GarmentLayeringThermalEngine` | — | `--geothermal-aquifer-selftest` | `godot --headless --path . -- --geothermal-aquifer-selftest` |
| `VisitorIntegrationSystem` | `Ashfall.Core.Tests/Integration/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Integration/` |
| `SurvivorVoiceSystem` | `Ashfall.Core.Tests/Survivors/` | `--survivor-death-selftest`, `--survivors-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `VoiceLineDispatchCoordinator` | `Ashfall.Core.Tests/Voice/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Voice/` |
| `VoiceLineSelectionEngine` | `Ashfall.Core.Tests/Voice/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Voice/` |
| `WaterQualityProfileEngine` | `Ashfall.Core.Tests/Water/` | `--starting-profile-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Water/` |
| `WaterSourceSystem` | `Ashfall.Core.Tests/Water/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Water/` |
| `NuclearWinterProgressionSystem` | `Ashfall.Core.Tests/Progression/` | — | `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/` |
| `WeatherCascadeSystem` | `Ashfall.Core.Tests/Weather/` | `--journal-weather-panel-selftest`, `--weather-save-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Weather/` |
| `CascadeTargetSystem` | — | — | new focused block required |
| `ModalTravelDispatchEngine` | — | `--travel-encounter-selftest`, `--traveling-caravan-selftest` | `godot --headless --path . -- --travel-encounter-selftest` |
| `NightWatchPatrolReadinessEngine` | — | `--patrol-encounter-selftest` | `godot --headless --path . -- --patrol-encounter-selftest` |
| `StormForecastReadinessEngine` | — | — | new focused block required |
| `WeatherForecastReliabilityEngine` | `Ashfall.Core.Tests/Weather/` | `--journal-weather-panel-selftest`, `--weather-save-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/Weather/` |
| `WildlifeHarvestQuotaEngine` | `Ashfall.Core.Tests/WildlifeTrapping/` | `--wildlife-selftest` | `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrapping/` |
