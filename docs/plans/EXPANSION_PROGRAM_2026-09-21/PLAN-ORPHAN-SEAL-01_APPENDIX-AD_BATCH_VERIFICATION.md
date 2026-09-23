# PLAN-ORPHAN-SEAL-01 — Appendix AD: Batch Verification Protocol

**Generated:** 2026-09-21. The exact verification loop per batch: the focused
commands from Appendix O, the reachability re-run, and the **expected delta** —
a sealed batch must remove its members from the 99-file orphan list, so the
audit becomes the batch's acceptance witness (99 → 99−N). If the count does not
move, the batch is not integrated regardless of green tests.

**Per-batch loop**

1. Run the members' focused commands (below); all green.
2. Run `python3 docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/reachability-audit.py`
   and expect `host-unreachable authority files: 99−N` for N sealed
   members — the audit itself is the acceptance witness.
3. Run `python3 scripts/ci/generate-docs-index.py --check` if docs changed.
4. Record the audit delta in the claim handoff; a zero delta means unreachable→reachable did not actually happen.

### Batch 01 — expected 99 → 90 when all 9 members seal

| Orphan | Focused command |
|---|---|
| `CommunicationsSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Communications/` |
| `SurvivorVoiceSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `CommitmentSystem` | new focused block required |
| `ShelterIdentitySystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `OutpostSettlementSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Settlements/` |
| `SurvivorRoleSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `ShelterMaintenanceSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `ChemicalReagentSynthesisEngine` | `godot --headless --path . -- --chemical-dependency-save-selftest` |
| `StormForecastReadinessEngine` | new focused block required |

### Batch 02 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `SeasonalCelebrationSystem` | new focused block required |
| `ShelterFestivalEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `ConfessionSecretSystem` | new focused block required |
| `TrophySystem` | new focused block required |
| `PrecisionGlassworksOpticsEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Optics/` |
| `HobbySystem` | new focused block required |
| `CampaignLegacySystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/` |
| `MechanicalPowerDrivelineEngine` | `godot --headless --path . -- --power-grid-catalog-selftest` |
| `DependencyTaperWithdrawalEngine` | `godot --headless --path . -- --chemical-dependency-save-selftest` |
| `RehabilitationProgressionEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/` |

### Batch 03 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `ShelterExpansionSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Expansions/` |
| `PsychologicalProfileSystem` | `godot --headless --path . -- --starting-profile-selftest` |
| `CultureCreationSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/` |
| `WaterSourceSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Water/` |
| `SubterraneanSubsidenceEngine` | new focused block required |
| `BackstorySystem` | new focused block required |
| `AgingSystem` | new focused block required |
| `NightWatchPatrolReadinessEngine` | `godot --headless --path . -- --patrol-encounter-selftest` |
| `WildlifeHarvestQuotaEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrapping/` |
| `TradeRouteRiskBindingEngine` | `godot --headless --path . -- --deep-coast-route-selftest` |

### Batch 04 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `DisasterResponseSystem` | new focused block required |
| `EmergencyAlertSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Emergency/` |
| `BestiarySystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Bestiary/` |
| `LoanSharkEnforcerEngine` | new focused block required |
| `CommonTableRationingEngine` | new focused block required |
| `RecruitmentSystem` | new focused block required |
| `AudioAccessibilityCoordinator` | `bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/` |
| `ClinicalWardTriageEngine` | `godot --headless --path . -- --medical-ward-save-selftest` |
| `BlackMarketContrabandEngine` | `godot --headless --path . -- --black-flotilla-selftest` |
| `WeatherForecastReliabilityEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Weather/` |

### Batch 05 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `ShelterGovernanceEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/` |
| `CookingSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Cooking/` |
| `PublicBroadsheetPressEngine` | new focused block required |
| `CupolaFoundryEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/` |
| `ChemicalPlumeDispersionEngine` | `godot --headless --path . -- --chemical-dependency-save-selftest` |
| `ClothingWarmthSystem` | new focused block required |
| `BlackMarketHeatAttentionEngine` | `godot --headless --path . -- --black-flotilla-selftest` |
| `AntenatalMaternalHealthEngine` | new focused block required |
| `PalliativeCareDignityEngine` | new focused block required |
| `ProstheticConditionWearEngine` | new focused block required |

### Batch 06 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `VisitorIntegrationSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Integration/` |
| `SessionDurabilityManager` | new focused block required |
| `FoodTypeSystem` | new focused block required |
| `NpcMemorySystem` | new focused block required |
| `OilseedPressingEngine` | new focused block required |
| `ContentOrphanCertificationEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Content/` |
| `SurvivorLetterDeliverySystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `MigrationConsequenceEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeConsequence/` |
| `RestockAllocationEngine` | new focused block required |
| `SpiritualRitualCalendarEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Spiritual/` |

### Batch 07 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `SurvivorEducationSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Education/` |
| `CascadeTargetSystem` | new focused block required |
| `AccessibilitySettingsSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/` |
| `ColonySystem` | new focused block required |
| `SleepAcousticRestEngine` | new focused block required |
| `VoiceLineSelectionEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Voice/` |
| `SurgicalGraftRejectionEngine` | new focused block required |
| `SeasonalHumanMigrationEngine` | new focused block required |
| `KilnFiringEngine` | new focused block required |
| `EmergencyMusterReadinessEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Emergency/` |

### Batch 08 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `ShelterMuseumSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/` |
| `TerritoryControlSystem` | new focused block required |
| `VoiceLineDispatchCoordinator` | `bash scripts/run_test.sh Ashfall.Core.Tests/Voice/` |
| `SurvivorRoutineSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `GarmentLayeringThermalEngine` | `godot --headless --path . -- --geothermal-aquifer-selftest` |
| `PlayableMetricsAggregationEngine` | `godot --headless --path . -- --day1-playable-selftest` |
| `PerimeterEarlyWarningEngine` | new focused block required |
| `LetterDeliverySystem` | new focused block required |
| `RadioPropagationEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` |
| `ModalTravelDispatchEngine` | `godot --headless --path . -- --travel-encounter-selftest` |

### Batch 09 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `InternalCommunicationSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Communication/` |
| `FactionDiplomacySystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Diplomacy/` |
| `WeatherCascadeSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Weather/` |
| `SurvivorBarterSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `SecondGenerationMilestoneEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Generations/` |
| `SoilReclamationProfileEngine` | `godot --headless --path . -- --starting-profile-selftest` |
| `CassettePlaybackSystem` | new focused block required |
| `TradeRouteMonopolyEngine` | `godot --headless --path . -- --deep-coast-route-selftest` |
| `ChitPurityAssayEngine` | new focused block required |
| `AerialReconWindowEngine` | `godot --headless --path . -- --advanced-industrial-recon-selftest` |

### Batch 10 — expected 99 → 89 when all 10 members seal

| Orphan | Focused command |
|---|---|
| `ModSupportSystem` | new focused block required |
| `NuclearWinterProgressionSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/` |
| `MaritimeExplorationSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Exploration/` |
| `SurvivorAutonomySystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/` |
| `InformantNetworkTradecraftEngine` | `godot --headless --path . -- --rumor-network-selftest` |
| `ApprenticeshipCurriculumEngine` | new focused block required |
| `DifficultySettingsSystem` | `bash scripts/run_test.sh Ashfall.Core.Tests/Difficulty/` |
| `PowerLoadSheddingEngine` | `godot --headless --path . -- --power-grid-catalog-selftest` |
| `SurvivorAgingProgressionEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/` |
| `WaterQualityProfileEngine` | `bash scripts/run_test.sh Ashfall.Core.Tests/Water/` |

