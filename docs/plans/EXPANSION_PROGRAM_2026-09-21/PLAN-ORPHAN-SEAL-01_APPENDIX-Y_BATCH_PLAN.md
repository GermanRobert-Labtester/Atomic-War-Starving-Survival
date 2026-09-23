# PLAN-ORPHAN-SEAL-01 — Appendix Y: Seal Batch Planner

**Generated:** 2026-09-21. A heuristic split of the 99 orphans into **10
balanced batches** (snake distribution by risk so each batch carries a mix of
anchors and small members). Each batch lists its dominant test region where one
exists (Appendix S) and a suggested focused command.
**This is a planning aid, not a claim schedule** — a batch may split further if
a member needs a new surface, and the foreman assigns claims.

### Batch 01 — 9 members · risk sum 19 · dominant region `Shelter`

| Orphan | Risk | Own test region |
|---|---:|---|
| `CommunicationsSystem` | 7 | `Communications` |
| `SurvivorVoiceSystem` | 3 | `Content` |
| `CommitmentSystem` | 3 | `Campaign` |
| `ShelterIdentitySystem` | 2 | `Shelter` |
| `OutpostSettlementSystem` | 2 | `Settlements` |
| `SurvivorRoleSystem` | 1 | `Survivors` |
| `ShelterMaintenanceSystem` | 1 | `Shelter` |
| `ChemicalReagentSynthesisEngine` | 0 | `Shelter` |
| `StormForecastReadinessEngine` | 0 | `World` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`

### Batch 02 — 10 members · risk sum 18 · dominant region `Survivors`

| Orphan | Risk | Own test region |
|---|---:|---|
| `SeasonalCelebrationSystem` | 6 | `Events` |
| `ShelterFestivalEngine` | 3 | `Culture` |
| `ConfessionSecretSystem` | 3 | `Survivors` |
| `TrophySystem` | 2 | `Shelter` |
| `PrecisionGlassworksOpticsEngine` | 2 | `Optics` |
| `HobbySystem` | 1 | `Survivors` |
| `CampaignLegacySystem` | 1 | `Legacy` |
| `MechanicalPowerDrivelineEngine` | 0 | `Shelter` |
| `DependencyTaperWithdrawalEngine` | 0 | `Medical` |
| `RehabilitationProgressionEngine` | 0 | `Medical` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`

### Batch 03 — 10 members · risk sum 17 · dominant region `Survivors`

| Orphan | Risk | Own test region |
|---|---:|---|
| `ShelterExpansionSystem` | 5 | `Shelter` |
| `PsychologicalProfileSystem` | 3 | `Survivors` |
| `CultureCreationSystem` | 3 | `Survivors` |
| `WaterSourceSystem` | 2 | `Water` |
| `SubterraneanSubsidenceEngine` | 2 | `Excavation` |
| `BackstorySystem` | 1 | `Survivors` |
| `AgingSystem` | 1 | `Survivors` |
| `NightWatchPatrolReadinessEngine` | 0 | `World` |
| `WildlifeHarvestQuotaEngine` | 0 | `World` |
| `TradeRouteRiskBindingEngine` | 0 | `Economy` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`

### Batch 04 — 10 members · risk sum 17 · dominant region `Economy`

| Orphan | Risk | Own test region |
|---|---:|---|
| `DisasterResponseSystem` | 5 | `Shelter` |
| `EmergencyAlertSystem` | 3 | `Emergency` |
| `BestiarySystem` | 3 | `Bestiary` |
| `LoanSharkEnforcerEngine` | 2 | `Economy` |
| `CommonTableRationingEngine` | 2 | `Nutrition` |
| `RecruitmentSystem` | 1 | `Survivors` |
| `AudioAccessibilityCoordinator` | 1 | `Audio` |
| `ClinicalWardTriageEngine` | 0 | `Medical` |
| `BlackMarketContrabandEngine` | 0 | `Economy` |
| `WeatherForecastReliabilityEngine` | 0 | `World` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`

### Batch 05 — 10 members · risk sum 16 · dominant region `Medical`

| Orphan | Risk | Own test region |
|---|---:|---|
| `ShelterGovernanceEngine` | 4 | `Governance` |
| `CookingSystem` | 3 | `Cooking` |
| `PublicBroadsheetPressEngine` | 3 | `Print` |
| `CupolaFoundryEngine` | 2 | `Shelter` |
| `ChemicalPlumeDispersionEngine` | 2 | `Combat` |
| `ClothingWarmthSystem` | 1 | `Inventory` |
| `BlackMarketHeatAttentionEngine` | 1 | `Economy` |
| `AntenatalMaternalHealthEngine` | 0 | `Survivors` |
| `PalliativeCareDignityEngine` | 0 | `Medical` |
| `ProstheticConditionWearEngine` | 0 | `Medical` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`

### Batch 06 — 10 members · risk sum 19 · dominant region `Economy`

| Orphan | Risk | Own test region |
|---|---:|---|
| `VisitorIntegrationSystem` | 4 | `Visitors` |
| `SessionDurabilityManager` | 4 | `Save` |
| `FoodTypeSystem` | 3 | `Kitchen` |
| `NpcMemorySystem` | 2 | `Narrative` |
| `OilseedPressingEngine` | 2 | `Farming` |
| `ContentOrphanCertificationEngine` | 2 | `Content` |
| `SurvivorLetterDeliverySystem` | 1 | `Survivors` |
| `MigrationConsequenceEngine` | 1 | `Economy` |
| `RestockAllocationEngine` | 0 | `Economy` |
| `SpiritualRitualCalendarEngine` | 0 | `Spiritual` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`

### Batch 07 — 10 members · risk sum 19 · dominant region `Shelter`

| Orphan | Risk | Own test region |
|---|---:|---|
| `SurvivorEducationSystem` | 4 | `Education` |
| `CascadeTargetSystem` | 4 | `Weather` |
| `AccessibilitySettingsSystem` | 3 | `Accessibility` |
| `ColonySystem` | 2 | `Expeditions` |
| `SleepAcousticRestEngine` | 2 | `Needs` |
| `VoiceLineSelectionEngine` | 2 | `Voice` |
| `SurgicalGraftRejectionEngine` | 1 | `Medical` |
| `SeasonalHumanMigrationEngine` | 1 | `Economy` |
| `KilnFiringEngine` | 0 | `Shelter` |
| `EmergencyMusterReadinessEngine` | 0 | `Shelter` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`

### Batch 08 — 10 members · risk sum 19 · dominant region `World`

| Orphan | Risk | Own test region |
|---|---:|---|
| `ShelterMuseumSystem` | 4 | `Culture` |
| `TerritoryControlSystem` | 4 | `World` |
| `VoiceLineDispatchCoordinator` | 3 | `Voice` |
| `SurvivorRoutineSystem` | 2 | `Survivors` |
| `GarmentLayeringThermalEngine` | 2 | `Textiles` |
| `PlayableMetricsAggregationEngine` | 2 | `Telemetry` |
| `PerimeterEarlyWarningEngine` | 1 | `Defense` |
| `LetterDeliverySystem` | 1 | `(new)` |
| `RadioPropagationEngine` | 0 | `Radio` |
| `ModalTravelDispatchEngine` | 0 | `World` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/`

### Batch 09 — 10 members · risk sum 19 · dominant region `Economy`

| Orphan | Risk | Own test region |
|---|---:|---|
| `InternalCommunicationSystem` | 4 | `Communication` |
| `FactionDiplomacySystem` | 4 | `Diplomacy` |
| `WeatherCascadeSystem` | 3 | `Weather` |
| `SurvivorBarterSystem` | 2 | `Economy` |
| `SecondGenerationMilestoneEngine` | 2 | `Generations` |
| `SoilReclamationProfileEngine` | 2 | `Farming` |
| `CassettePlaybackSystem` | 1 | `Verdict` |
| `TradeRouteMonopolyEngine` | 1 | `Economy` |
| `ChitPurityAssayEngine` | 0 | `Economy` |
| `AerialReconWindowEngine` | 0 | `Expeditions` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`

### Batch 10 — 10 members · risk sum 18 · dominant region `Survivors`

| Orphan | Risk | Own test region |
|---|---:|---|
| `ModSupportSystem` | 4 | `Mods` |
| `NuclearWinterProgressionSystem` | 4 | `Weather` |
| `MaritimeExplorationSystem` | 2 | `Maritime` |
| `SurvivorAutonomySystem` | 2 | `Survivors` |
| `InformantNetworkTradecraftEngine` | 2 | `Espionage` |
| `ApprenticeshipCurriculumEngine` | 2 | `Education` |
| `DifficultySettingsSystem` | 1 | `Difficulty` |
| `PowerLoadSheddingEngine` | 1 | `Shelter` |
| `SurvivorAgingProgressionEngine` | 0 | `Survivors` |
| `WaterQualityProfileEngine` | 0 | `Water` |

Suggested focused command: `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`

