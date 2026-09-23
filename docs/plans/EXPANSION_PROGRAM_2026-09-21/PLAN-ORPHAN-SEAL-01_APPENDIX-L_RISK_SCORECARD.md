# PLAN-ORPHAN-SEAL-01 — Appendix L: Seal Risk Scorecard

**Generated:** 2026-09-21. A transparent triage score per orphan, so seal
packages can be ordered by cost and risk rather than by list position.
**Formula (max 12):** size (0–5; 1 point per 400 lines) + determinism risk
(3 if any banned primitive) + state (1 if capture/restore or registry
knowledge) + attachment need (2 if **no** matching `Main.*` candidate) +
test debt (2 if **no** test file references it).
**Reading:** high scores are expensive or risky seals — start there with
Appendix G/H/K open; low scores are small, attached, tested seals.
0 orphans score ≥8; 84 score ≤3.

| Rank | Authority | Lines | Banned refs | Stateful | Host partial | Tests | Score |
|---:|---|---:|---:|:---:|:---:|---:|---:|
| 1 | `CommunicationsSystem` | 561 | 2 | yes | **no** | 1 | **7** |
| 2 | `SeasonalCelebrationSystem` | 365 | 1 | yes | **no** | 1 | **6** |
| 3 | `DisasterResponseSystem` | 457 | 1 | yes | yes | 1 | **5** |
| 4 | `ShelterExpansionSystem` | 665 | 3 | yes | yes | 1 | **5** |
| 5 | `InternalCommunicationSystem` | 509 | 0 | yes | **no** | 1 | **4** |
| 6 | `ShelterMuseumSystem` | 539 | 0 | yes | **no** | 1 | **4** |
| 7 | `FactionDiplomacySystem` | 451 | 0 | yes | **no** | 1 | **4** |
| 8 | `SurvivorEducationSystem` | 579 | 0 | yes | **no** | 1 | **4** |
| 9 | `TerritoryControlSystem` | 451 | 0 | yes | **no** | 2 | **4** |
| 10 | `ShelterGovernanceEngine` | 669 | 0 | yes | **no** | 2 | **4** |
| 11 | `ModSupportSystem` | 506 | 0 | yes | **no** | 1 | **4** |
| 12 | `SessionDurabilityManager` | 365 | 2 | yes | yes | 2 | **4** |
| 13 | `VisitorIntegrationSystem` | 611 | 0 | yes | **no** | 1 | **4** |
| 14 | `NuclearWinterProgressionSystem` | 472 | 0 | yes | **no** | 1 | **4** |
| 15 | `CascadeTargetSystem` | 438 | 0 | yes | **no** | 1 | **4** |
| 16 | `AccessibilitySettingsSystem` | 261 | 0 | yes | **no** | 1 | **3** |
| 17 | `BestiarySystem` | 285 | 0 | yes | **no** | 1 | **3** |
| 18 | `CommitmentSystem` | 314 | 0 | yes | **no** | 2 | **3** |
| 19 | `CookingSystem` | 395 | 0 | yes | **no** | 1 | **3** |
| 20 | `CultureCreationSystem` | 287 | 0 | yes | **no** | 2 | **3** |
| 21 | `ShelterFestivalEngine` | 323 | 0 | yes | **no** | 1 | **3** |
| 22 | `EmergencyAlertSystem` | 355 | 0 | yes | **no** | 1 | **3** |
| 23 | `FoodTypeSystem` | 267 | 0 | yes | **no** | 2 | **3** |
| 24 | `ConfessionSecretSystem` | 288 | 0 | yes | **no** | 2 | **3** |
| 25 | `PublicBroadsheetPressEngine` | 272 | 0 | yes | **no** | 1 | **3** |
| 26 | `PsychologicalProfileSystem` | 355 | 0 | yes | **no** | 1 | **3** |
| 27 | `SurvivorVoiceSystem` | 316 | 0 | yes | **no** | 2 | **3** |
| 28 | `VoiceLineDispatchCoordinator` | 253 | 0 | yes | **no** | 1 | **3** |
| 29 | `WeatherCascadeSystem` | 78 | 0 | yes | **no** | 1 | **3** |
| 30 | `ChemicalPlumeDispersionEngine` | 286 | 0 | — | **no** | 1 | **2** |
| 31 | `ContentOrphanCertificationEngine` | 136 | 0 | — | **no** | 2 | **2** |
| 32 | `LoanSharkEnforcerEngine` | 442 | 0 | yes | yes | 1 | **2** |
| 33 | `SurvivorBarterSystem` | 666 | 0 | yes | yes | 1 | **2** |
| 34 | `ApprenticeshipCurriculumEngine` | 219 | 0 | — | **no** | 1 | **2** |
| 35 | `InformantNetworkTradecraftEngine` | 222 | 0 | — | **no** | 1 | **2** |
| 36 | `SubterraneanSubsidenceEngine` | 299 | 0 | — | **no** | 1 | **2** |
| 37 | `ColonySystem` | 517 | 0 | yes | yes | 2 | **2** |
| 38 | `OilseedPressingEngine` | 246 | 0 | — | **no** | 1 | **2** |
| 39 | `SoilReclamationProfileEngine` | 216 | 0 | — | **no** | 1 | **2** |
| 40 | `SecondGenerationMilestoneEngine` | 229 | 0 | — | **no** | 1 | **2** |
| 41 | `MaritimeExplorationSystem` | 743 | 0 | yes | yes | 1 | **2** |
| 42 | `NpcMemorySystem` | 463 | 0 | yes | yes | 2 | **2** |
| 43 | `SleepAcousticRestEngine` | 230 | 0 | — | **no** | 1 | **2** |
| 44 | `CommonTableRationingEngine` | 298 | 0 | — | **no** | 1 | **2** |
| 45 | `PrecisionGlassworksOpticsEngine` | 302 | 0 | — | **no** | 1 | **2** |
| 46 | `OutpostSettlementSystem` | 318 | 0 | — | **no** | 1 | **2** |
| 47 | `CupolaFoundryEngine` | 445 | 0 | yes | yes | 1 | **2** |
| 48 | `ShelterIdentitySystem` | 401 | 0 | yes | yes | 1 | **2** |
| 49 | `TrophySystem` | 411 | 0 | yes | yes | 1 | **2** |
| 50 | `SurvivorAutonomySystem` | 667 | 0 | yes | yes | 1 | **2** |
| 51 | `SurvivorRoutineSystem` | 527 | 0 | yes | yes | 1 | **2** |
| 52 | `PlayableMetricsAggregationEngine` | 186 | 0 | — | **no** | 1 | **2** |
| 53 | `GarmentLayeringThermalEngine` | 230 | 0 | — | **no** | 1 | **2** |
| 54 | `VoiceLineSelectionEngine` | 185 | 0 | — | **no** | 1 | **2** |
| 55 | `WaterSourceSystem` | 434 | 0 | yes | yes | 2 | **2** |
| 56 | `AudioAccessibilityCoordinator` | 294 | 0 | yes | yes | 1 | **1** |
| 57 | `CassettePlaybackSystem` | 237 | 0 | yes | yes | 1 | **1** |
| 58 | `PerimeterEarlyWarningEngine` | 280 | 0 | yes | yes | 1 | **1** |
| 59 | `DifficultySettingsSystem` | 215 | 0 | yes | yes | 1 | **1** |
| 60 | `BlackMarketHeatAttentionEngine` | 289 | 0 | yes | yes | 1 | **1** |
| 61 | `MigrationConsequenceEngine` | 144 | 0 | yes | yes | 1 | **1** |
| 62 | `SeasonalHumanMigrationEngine` | 162 | 0 | yes | yes | 2 | **1** |
| 63 | `TradeRouteMonopolyEngine` | 206 | 0 | yes | yes | 1 | **1** |
| 64 | `ClothingWarmthSystem` | 390 | 0 | yes | yes | 1 | **1** |
| 65 | `CampaignLegacySystem` | 305 | 0 | yes | yes | 1 | **1** |
| 66 | `SurgicalGraftRejectionEngine` | 284 | 0 | yes | yes | 1 | **1** |
| 67 | `LetterDeliverySystem` | 200 | 0 | yes | yes | 1 | **1** |
| 68 | `SurvivorLetterDeliverySystem` | 285 | 0 | yes | yes | 1 | **1** |
| 69 | `PowerLoadSheddingEngine` | 212 | 0 | yes | yes | 1 | **1** |
| 70 | `ShelterMaintenanceSystem` | 306 | 0 | yes | yes | 1 | **1** |
| 71 | `AgingSystem` | 304 | 0 | yes | yes | 1 | **1** |
| 72 | `BackstorySystem` | 364 | 0 | yes | yes | 1 | **1** |
| 73 | `HobbySystem` | 344 | 0 | yes | yes | 2 | **1** |
| 74 | `RecruitmentSystem` | 367 | 0 | yes | yes | 1 | **1** |
| 75 | `SurvivorRoleSystem` | 321 | 0 | yes | yes | 1 | **1** |
| 76 | `BlackMarketContrabandEngine` | 267 | 0 | — | yes | 2 | **0** |
| 77 | `ChitPurityAssayEngine` | 228 | 0 | — | yes | 1 | **0** |
| 78 | `RestockAllocationEngine` | 245 | 0 | — | yes | 1 | **0** |
| 79 | `TradeRouteRiskBindingEngine` | 130 | 0 | — | yes | 1 | **0** |
| 80 | `AerialReconWindowEngine` | 200 | 0 | — | yes | 1 | **0** |
| 81 | `ClinicalWardTriageEngine` | 307 | 0 | — | yes | 2 | **0** |
| 82 | `DependencyTaperWithdrawalEngine` | 268 | 0 | — | yes | 1 | **0** |
| 83 | `PalliativeCareDignityEngine` | 254 | 0 | — | yes | 1 | **0** |
| 84 | `ProstheticConditionWearEngine` | 157 | 0 | — | yes | 1 | **0** |
| 85 | `RehabilitationProgressionEngine` | 112 | 0 | — | yes | 1 | **0** |
| 86 | `RadioPropagationEngine` | 233 | 0 | — | yes | 1 | **0** |
| 87 | `ChemicalReagentSynthesisEngine` | 276 | 0 | — | yes | 1 | **0** |
| 88 | `EmergencyMusterReadinessEngine` | 192 | 0 | — | yes | 1 | **0** |
| 89 | `KilnFiringEngine` | 241 | 0 | — | yes | 1 | **0** |
| 90 | `MechanicalPowerDrivelineEngine` | 278 | 0 | — | yes | 1 | **0** |
| 91 | `SpiritualRitualCalendarEngine` | 182 | 0 | — | yes | 1 | **0** |
| 92 | `AntenatalMaternalHealthEngine` | 321 | 0 | — | yes | 1 | **0** |
| 93 | `SurvivorAgingProgressionEngine` | 203 | 0 | — | yes | 2 | **0** |
| 94 | `WaterQualityProfileEngine` | 203 | 0 | — | yes | 1 | **0** |
| 95 | `ModalTravelDispatchEngine` | 200 | 0 | — | yes | 1 | **0** |
| 96 | `NightWatchPatrolReadinessEngine` | 279 | 0 | — | yes | 1 | **0** |
| 97 | `StormForecastReadinessEngine` | 276 | 0 | — | yes | 1 | **0** |
| 98 | `WeatherForecastReliabilityEngine` | 146 | 0 | — | yes | 1 | **0** |
| 99 | `WildlifeHarvestQuotaEngine` | 268 | 0 | — | yes | 1 | **0** |
