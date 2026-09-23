# PLAN-ORPHAN-SEAL-01 — Appendix B: Wave Package Specifications

**Generated:** 2026-09-21 from the reachability audit (HEAD `5be1a30a`).
**Purpose:** turn each wave from Appendix A into an executable package list.
Each row is a candidate builder package: claim it individually, name the exact
files in `WORKTREE_OWNERSHIP.md`, and verify with the listed command.

**Package naming:** `ORPHAN-SEAL-W<N>-<domain>-<nnn>`.
**Claim shape:** one package = one or two dossiers + their host/save/route seam.
**Global DoD:** main plan §4 (authority, host owner, path, persistence,
outcome, focused tests, reachability re-run).

## Wave summary

| Wave | Dossiers | Systems | Proof command |
|---|---:|---:|---|
| W2 | 11 | `ChemicalReagentSynthesisEngine`, `CupolaFoundryEngine`, `DisasterResponseSystem`, `EmergencyMusterReadinessEngine`, `KilnFiringEngine`, `MechanicalPowerDrivelineEngine` … | reachability re-run + focused suites |
| W3 | 10 | `BlackMarketContrabandEngine`, `BlackMarketHeatAttentionEngine`, `ChitPurityAssayEngine`, `LoanSharkEnforcerEngine`, `MigrationConsequenceEngine`, `RestockAllocationEngine` … | reachability re-run + focused suites |
| W4 | 16 | `AgingSystem`, `AntenatalMaternalHealthEngine`, `BackstorySystem`, `HobbySystem`, `InternalCommunicationSystem`, `LetterDeliverySystem` … | reachability re-run + focused suites |
| W5 | 11 | `ClinicalWardTriageEngine`, `CommonTableRationingEngine`, `DependencyTaperWithdrawalEngine`, `FoodTypeSystem`, `PalliativeCareDignityEngine`, `ProstheticConditionWearEngine` … | reachability re-run + focused suites |
| W6 | 11 | `CascadeTargetSystem`, `EmergencyAlertSystem`, `ModalTravelDispatchEngine`, `NightWatchPatrolReadinessEngine`, `NuclearWinterProgressionSystem`, `PerimeterEarlyWarningEngine` … | reachability re-run + focused suites |
| W7 | 14 | `AudioAccessibilityCoordinator`, `CampaignLegacySystem`, `CassettePlaybackSystem`, `CommunicationsSystem`, `CultureCreationSystem`, `FactionDiplomacySystem` … | reachability re-run + focused suites |
| W8 | 26 | `AccessibilitySettingsSystem`, `AerialReconWindowEngine`, `ApprenticeshipCurriculumEngine`, `BestiarySystem`, `ChemicalPlumeDispersionEngine`, `ClothingWarmthSystem` … | reachability re-run + focused suites |


## W2 — Shelter industry & infrastructure (11 dossiers)

| # | System | File | Tests | Package | Verify |
|---|---|---|---:|---|---|
| 01 | `ChemicalReagentSynthesisEngine` | `Shelter/ChemicalReagentSynthesisEngine.cs` | 1 | `ORPHAN-SEAL-W2-chemicalreagentsynthesis-001` | reachability + focused suite |
| 02 | `CupolaFoundryEngine` | `Shelter/CupolaFoundryEngine.cs` | 1 | `ORPHAN-SEAL-W2-cupolafoundry-002` | reachability + focused suite |
| 03 | `DisasterResponseSystem` | `Shelter/DisasterResponseSystem.cs` | 1 | `ORPHAN-SEAL-W2-disasterresponse-003` | reachability + focused suite |
| 04 | `EmergencyMusterReadinessEngine` | `Shelter/EmergencyMusterReadinessEngine.cs` | 1 | `ORPHAN-SEAL-W2-emergencymusterreadiness-004` | reachability + focused suite |
| 05 | `KilnFiringEngine` | `Shelter/KilnFiringEngine.cs` | 1 | `ORPHAN-SEAL-W2-kilnfiring-005` | reachability + focused suite |
| 06 | `MechanicalPowerDrivelineEngine` | `Shelter/MechanicalPowerDrivelineEngine.cs` | 1 | `ORPHAN-SEAL-W2-mechanicalpowerdriveline-006` | reachability + focused suite |
| 07 | `PowerLoadSheddingEngine` | `Shelter/PowerLoadSheddingEngine.cs` | 1 | `ORPHAN-SEAL-W2-powerloadshedding-007` | reachability + focused suite |
| 08 | `ShelterExpansionSystem` | `Shelter/ShelterExpansionSystem.cs` | 1 | `ORPHAN-SEAL-W2-shelterexpansion-008` | reachability + focused suite |
| 09 | `ShelterIdentitySystem` | `Shelter/ShelterIdentitySystem.cs` | 1 | `ORPHAN-SEAL-W2-shelteridentity-009` | reachability + focused suite |
| 10 | `ShelterMaintenanceSystem` | `Shelter/ShelterMaintenanceSystem.cs` | 1 | `ORPHAN-SEAL-W2-sheltermaintenance-010` | reachability + focused suite |
| 11 | `TrophySystem` | `Shelter/TrophySystem.cs` | 1 | `ORPHAN-SEAL-W2-trophy-011` | reachability + focused suite |

**Wave exit criteria**
- every listed system is host-reachable or registered `CORE_ONLY` with a reason;
- one player-visible outcome per system (journal, panel row, CLI probe);
- save path named (existing section preferred; new section only with integrator sign-off);
- focused suites green and the reachability gate reports the wave's systems as reachable;
- handoff uses the standard block from `AI_AGENT_WORKFLOW.md`.

## W3 — Economy depth (10 dossiers)

| # | System | File | Tests | Package | Verify |
|---|---|---|---:|---|---|
| 01 | `BlackMarketContrabandEngine` | `Economy/BlackMarketContrabandEngine.cs` | 1 | `ORPHAN-SEAL-W3-blackmarketcontraband-001` | reachability + focused suite |
| 02 | `BlackMarketHeatAttentionEngine` | `Economy/BlackMarketHeatAttentionEngine.cs` | 1 | `ORPHAN-SEAL-W3-blackmarketheatattention-002` | reachability + focused suite |
| 03 | `ChitPurityAssayEngine` | `Economy/ChitPurityAssayEngine.cs` | 1 | `ORPHAN-SEAL-W3-chitpurityassay-003` | reachability + focused suite |
| 04 | `LoanSharkEnforcerEngine` | `Economy/LoanSharkEnforcerEngine.cs` | 1 | `ORPHAN-SEAL-W3-loansharkenforcer-004` | reachability + focused suite |
| 05 | `MigrationConsequenceEngine` | `Economy/MigrationConsequenceEngine.cs` | 1 | `ORPHAN-SEAL-W3-migrationconsequence-005` | reachability + focused suite |
| 06 | `RestockAllocationEngine` | `Economy/RestockAllocationEngine.cs` | 1 | `ORPHAN-SEAL-W3-restockallocation-006` | reachability + focused suite |
| 07 | `SeasonalHumanMigrationEngine` | `Economy/SeasonalHumanMigrationEngine.cs` | 1 | `ORPHAN-SEAL-W3-seasonalhumanmigration-007` | reachability + focused suite |
| 08 | `SurvivorBarterSystem` | `Economy/SurvivorBarterSystem.cs` | 1 | `ORPHAN-SEAL-W3-survivorbarter-008` | reachability + focused suite |
| 09 | `TradeRouteMonopolyEngine` | `Economy/TradeRouteMonopolyEngine.cs` | 1 | `ORPHAN-SEAL-W3-traderoutemonopoly-009` | reachability + focused suite |
| 10 | `TradeRouteRiskBindingEngine` | `Economy/TradeRouteRiskBindingEngine.cs` | 1 | `ORPHAN-SEAL-W3-traderouteriskbinding-010` | reachability + focused suite |

**Wave exit criteria**
- every listed system is host-reachable or registered `CORE_ONLY` with a reason;
- one player-visible outcome per system (journal, panel row, CLI probe);
- save path named (existing section preferred; new section only with integrator sign-off);
- focused suites green and the reachability gate reports the wave's systems as reachable;
- handoff uses the standard block from `AI_AGENT_WORKFLOW.md`.

## W4 — Survivor life, voice & narrative (16 dossiers)

| # | System | File | Tests | Package | Verify |
|---|---|---|---:|---|---|
| 01 | `AgingSystem` | `Survivors/AgingSystem.cs` | 1 | `ORPHAN-SEAL-W4-aging-001` | reachability + focused suite |
| 02 | `AntenatalMaternalHealthEngine` | `Survivors/AntenatalMaternalHealthEngine.cs` | 1 | `ORPHAN-SEAL-W4-antenatalmaternalhealth-002` | reachability + focused suite |
| 03 | `BackstorySystem` | `Survivors/BackstorySystem.cs` | 1 | `ORPHAN-SEAL-W4-backstory-003` | reachability + focused suite |
| 04 | `HobbySystem` | `Survivors/HobbySystem.cs` | 1 | `ORPHAN-SEAL-W4-hobby-004` | reachability + focused suite |
| 05 | `InternalCommunicationSystem` | `Communication/InternalCommunicationSystem.cs` | 1 | `ORPHAN-SEAL-W4-internalcommunication-005` | reachability + focused suite |
| 06 | `LetterDeliverySystem` | `Narrative/LetterDeliverySystem.cs` | 1 | `ORPHAN-SEAL-W4-letterdelivery-006` | reachability + focused suite |
| 07 | `NpcMemorySystem` | `Narrative/NpcMemorySystem.cs` | 1 | `ORPHAN-SEAL-W4-npcmemory-007` | reachability + focused suite |
| 08 | `RecruitmentSystem` | `Survivors/RecruitmentSystem.cs` | 1 | `ORPHAN-SEAL-W4-recruitment-008` | reachability + focused suite |
| 09 | `SurvivorAgingProgressionEngine` | `Survivors/SurvivorAgingProgressionEngine.cs` | 1 | `ORPHAN-SEAL-W4-survivoragingprogression-009` | reachability + focused suite |
| 10 | `SurvivorAutonomySystem` | `Survivors/SurvivorAutonomySystem.cs` | 1 | `ORPHAN-SEAL-W4-survivorautonomy-010` | reachability + focused suite |
| 11 | `SurvivorLetterDeliverySystem` | `Narrative/SurvivorLetterDeliverySystem.cs` | 1 | `ORPHAN-SEAL-W4-survivorletterdelivery-011` | reachability + focused suite |
| 12 | `SurvivorRoleSystem` | `Survivors/SurvivorRoleSystem.cs` | 1 | `ORPHAN-SEAL-W4-survivorrole-012` | reachability + focused suite |
| 13 | `SurvivorRoutineSystem` | `Survivors/SurvivorRoutineSystem.cs` | 1 | `ORPHAN-SEAL-W4-survivorroutine-013` | reachability + focused suite |
| 14 | `SurvivorVoiceSystem` | `Voice/SurvivorVoiceSystem.cs` | 1 | `ORPHAN-SEAL-W4-survivorvoice-014` | reachability + focused suite |
| 15 | `VoiceLineDispatchCoordinator` | `Voice/VoiceLineDispatchCoordinator.cs` | 1 | `ORPHAN-SEAL-W4-voicelinedispatch-015` | reachability + focused suite |
| 16 | `VoiceLineSelectionEngine` | `Voice/VoiceLineSelectionEngine.cs` | 1 | `ORPHAN-SEAL-W4-voicelineselection-016` | reachability + focused suite |

**Wave exit criteria**
- every listed system is host-reachable or registered `CORE_ONLY` with a reason;
- one player-visible outcome per system (journal, panel row, CLI probe);
- save path named (existing section preferred; new section only with integrator sign-off);
- focused suites green and the reachability gate reports the wave's systems as reachable;
- handoff uses the standard block from `AI_AGENT_WORKFLOW.md`.

## W5 — Medical continuum & table (11 dossiers)

| # | System | File | Tests | Package | Verify |
|---|---|---|---:|---|---|
| 01 | `ClinicalWardTriageEngine` | `Medical/ClinicalWardTriageEngine.cs` | 1 | `ORPHAN-SEAL-W5-clinicalwardtriage-001` | reachability + focused suite |
| 02 | `CommonTableRationingEngine` | `Nutrition/CommonTableRationingEngine.cs` | 1 | `ORPHAN-SEAL-W5-commontablerationing-002` | reachability + focused suite |
| 03 | `DependencyTaperWithdrawalEngine` | `Medical/DependencyTaperWithdrawalEngine.cs` | 1 | `ORPHAN-SEAL-W5-dependencytaperwithdrawal-003` | reachability + focused suite |
| 04 | `FoodTypeSystem` | `Kitchen/FoodTypeSystem.cs` | 1 | `ORPHAN-SEAL-W5-foodtype-004` | reachability + focused suite |
| 05 | `PalliativeCareDignityEngine` | `Medical/PalliativeCareDignityEngine.cs` | 1 | `ORPHAN-SEAL-W5-palliativecaredignity-005` | reachability + focused suite |
| 06 | `ProstheticConditionWearEngine` | `Medical/ProstheticConditionWearEngine.cs` | 1 | `ORPHAN-SEAL-W5-prostheticconditionwear-006` | reachability + focused suite |
| 07 | `RehabilitationProgressionEngine` | `Medical/RehabilitationProgressionEngine.cs` | 1 | `ORPHAN-SEAL-W5-rehabilitationprogression-007` | reachability + focused suite |
| 08 | `SleepAcousticRestEngine` | `Needs/SleepAcousticRestEngine.cs` | 1 | `ORPHAN-SEAL-W5-sleepacousticrest-008` | reachability + focused suite |
| 09 | `SurgicalGraftRejectionEngine` | `Medical/SurgicalGraftRejectionEngine.cs` | 1 | `ORPHAN-SEAL-W5-surgicalgraftrejection-009` | reachability + focused suite |
| 10 | `WaterQualityProfileEngine` | `Water/WaterQualityProfileEngine.cs` | 1 | `ORPHAN-SEAL-W5-waterqualityprofile-010` | reachability + focused suite |
| 11 | `WaterSourceSystem` | `Water/WaterSourceSystem.cs` | 1 | `ORPHAN-SEAL-W5-watersource-011` | reachability + focused suite |

**Wave exit criteria**
- every listed system is host-reachable or registered `CORE_ONLY` with a reason;
- one player-visible outcome per system (journal, panel row, CLI probe);
- save path named (existing section preferred; new section only with integrator sign-off);
- focused suites green and the reachability gate reports the wave's systems as reachable;
- handoff uses the standard block from `AI_AGENT_WORKFLOW.md`.

## W6 — World, weather & risk (11 dossiers)

| # | System | File | Tests | Package | Verify |
|---|---|---|---:|---|---|
| 01 | `CascadeTargetSystem` | `Weather/WeatherGameplayCascadeEngine.cs` | 2 | `ORPHAN-SEAL-W6-cascadetarget-001` | reachability + focused suite |
| 02 | `EmergencyAlertSystem` | `Emergency/EmergencyAlertSystem.cs` | 1 | `ORPHAN-SEAL-W6-emergencyalert-002` | reachability + focused suite |
| 03 | `ModalTravelDispatchEngine` | `World/ModalTravelDispatchEngine.cs` | 1 | `ORPHAN-SEAL-W6-modaltraveldispatch-003` | reachability + focused suite |
| 04 | `NightWatchPatrolReadinessEngine` | `World/NightWatchPatrolReadinessEngine.cs` | 1 | `ORPHAN-SEAL-W6-nightwatchpatrolreadiness-004` | reachability + focused suite |
| 05 | `NuclearWinterProgressionSystem` | `Weather/NuclearWinterProgressionSystem.cs` | 1 | `ORPHAN-SEAL-W6-nuclearwinterprogression-005` | reachability + focused suite |
| 06 | `PerimeterEarlyWarningEngine` | `Defense/PerimeterEarlyWarningEngine.cs` | 1 | `ORPHAN-SEAL-W6-perimeterearlywarning-006` | reachability + focused suite |
| 07 | `StormForecastReadinessEngine` | `World/StormForecastReadinessEngine.cs` | 1 | `ORPHAN-SEAL-W6-stormforecastreadiness-007` | reachability + focused suite |
| 08 | `SubterraneanSubsidenceEngine` | `Excavation/SubterraneanSubsidenceEngine.cs` | 1 | `ORPHAN-SEAL-W6-subterraneansubsidence-008` | reachability + focused suite |
| 09 | `WeatherCascadeSystem` | `Weather/WeatherCascadeSystem.cs` | 1 | `ORPHAN-SEAL-W6-weathercascade-009` | reachability + focused suite |
| 10 | `WeatherForecastReliabilityEngine` | `World/WeatherForecastReliabilityEngine.cs` | 1 | `ORPHAN-SEAL-W6-weatherforecastreliability-010` | reachability + focused suite |
| 11 | `WildlifeHarvestQuotaEngine` | `World/WildlifeHarvestQuotaEngine.cs` | 1 | `ORPHAN-SEAL-W6-wildlifeharvestquota-011` | reachability + focused suite |

**Wave exit criteria**
- every listed system is host-reachable or registered `CORE_ONLY` with a reason;
- one player-visible outcome per system (journal, panel row, CLI probe);
- save path named (existing section preferred; new section only with integrator sign-off);
- focused suites green and the reachability gate reports the wave's systems as reachable;
- handoff uses the standard block from `AI_AGENT_WORKFLOW.md`.

## W7 — Culture, comms & politics (14 dossiers)

| # | System | File | Tests | Package | Verify |
|---|---|---|---:|---|---|
| 01 | `AudioAccessibilityCoordinator` | `Audio/AudioAccessibilityCoordinator.cs` | 1 | `ORPHAN-SEAL-W7-audioaccessibility-001` | reachability + focused suite |
| 02 | `CampaignLegacySystem` | `Legacy/CampaignLegacySystem.cs` | 1 | `ORPHAN-SEAL-W7-campaignlegacy-002` | reachability + focused suite |
| 03 | `CassettePlaybackSystem` | `Audio/CassettePlaybackSystem.cs` | 1 | `ORPHAN-SEAL-W7-cassetteplayback-003` | reachability + focused suite |
| 04 | `CommunicationsSystem` | `Communications/CommunicationsSystem.cs` | 1 | `ORPHAN-SEAL-W7-communications-004` | reachability + focused suite |
| 05 | `CultureCreationSystem` | `Culture/CultureCreationSystem.cs` | 1 | `ORPHAN-SEAL-W7-culturecreation-005` | reachability + focused suite |
| 06 | `FactionDiplomacySystem` | `Diplomacy/FactionDiplomacySystem.cs` | 1 | `ORPHAN-SEAL-W7-factiondiplomacy-006` | reachability + focused suite |
| 07 | `InformantNetworkTradecraftEngine` | `Espionage/InformantNetworkTradecraftEngine.cs` | 1 | `ORPHAN-SEAL-W7-informantnetworktradecraft-007` | reachability + focused suite |
| 08 | `PublicBroadsheetPressEngine` | `Print/PublicBroadsheetPressEngine.cs` | 1 | `ORPHAN-SEAL-W7-publicbroadsheetpress-008` | reachability + focused suite |
| 09 | `RadioPropagationEngine` | `Radio/RadioPropagation.cs` | 1 | `ORPHAN-SEAL-W7-radiopropagation-009` | reachability + focused suite |
| 10 | `SecondGenerationMilestoneEngine` | `Generations/SecondGenerationMilestoneEngine.cs` | 1 | `ORPHAN-SEAL-W7-secondgenerationmilestone-010` | reachability + focused suite |
| 11 | `ShelterFestivalEngine` | `Culture/ShelterFestivalEngine.cs` | 1 | `ORPHAN-SEAL-W7-shelterfestival-011` | reachability + focused suite |
| 12 | `ShelterMuseumSystem` | `Culture/ShelterMuseumSystem.cs` | 1 | `ORPHAN-SEAL-W7-sheltermuseum-012` | reachability + focused suite |
| 13 | `SpiritualRitualCalendarEngine` | `Spiritual/SpiritualRitualCalendarEngine.cs` | 1 | `ORPHAN-SEAL-W7-spiritualritualcalendar-013` | reachability + focused suite |
| 14 | `TerritoryControlSystem` | `Factions/TerritoryControlSystem.cs` | 1 | `ORPHAN-SEAL-W7-territorycontrol-014` | reachability + focused suite |

**Wave exit criteria**
- every listed system is host-reachable or registered `CORE_ONLY` with a reason;
- one player-visible outcome per system (journal, panel row, CLI probe);
- save path named (existing section preferred; new section only with integrator sign-off);
- focused suites green and the reachability gate reports the wave's systems as reachable;
- handoff uses the standard block from `AI_AGENT_WORKFLOW.md`.

## W8 — Logistics, settlement & meta (26 dossiers)

| # | System | File | Tests | Package | Verify |
|---|---|---|---:|---|---|
| 01 | `AccessibilitySettingsSystem` | `Accessibility/AccessibilitySettingsSystem.cs` | 1 | `ORPHAN-SEAL-W8-accessibilitysettings-001` | reachability + focused suite |
| 02 | `AerialReconWindowEngine` | `Expeditions/AerialReconWindowEngine.cs` | 1 | `ORPHAN-SEAL-W8-aerialreconwindow-002` | reachability + focused suite |
| 03 | `ApprenticeshipCurriculumEngine` | `Education/ApprenticeshipCurriculumEngine.cs` | 1 | `ORPHAN-SEAL-W8-apprenticeshipcurriculum-003` | reachability + focused suite |
| 04 | `BestiarySystem` | `Bestiary/BestiarySystem.cs` | 1 | `ORPHAN-SEAL-W8-bestiary-004` | reachability + focused suite |
| 05 | `ChemicalPlumeDispersionEngine` | `Combat/ChemicalPlumeDispersionEngine.cs` | 1 | `ORPHAN-SEAL-W8-chemicalplumedispersion-005` | reachability + focused suite |
| 06 | `ClothingWarmthSystem` | `Inventory/ClothingWarmthSystem.cs` | 1 | `ORPHAN-SEAL-W8-clothingwarmth-006` | reachability + focused suite |
| 07 | `ColonySystem` | `Expeditions/ColonySystem.cs` | 1 | `ORPHAN-SEAL-W8-colony-007` | reachability + focused suite |
| 08 | `CommitmentSystem` | `Commitments/CommitmentSystem.cs` | 1 | `ORPHAN-SEAL-W8-commitment-008` | reachability + focused suite |
| 09 | `ConfessionSecretSystem` | `Phantoms/ConfessionSecretSystem.cs` | 1 | `ORPHAN-SEAL-W8-confessionsecret-009` | reachability + focused suite |
| 10 | `ContentOrphanCertificationEngine` | `Content/ContentOrphanCertificationEngine.cs` | 1 | `ORPHAN-SEAL-W8-contentorphancertification-010` | reachability + focused suite |
| 11 | `CookingSystem` | `Cooking/CookingSystem.cs` | 1 | `ORPHAN-SEAL-W8-cooking-011` | reachability + focused suite |
| 12 | `DifficultySettingsSystem` | `Difficulty/DifficultySettingsSystem.cs` | 1 | `ORPHAN-SEAL-W8-difficultysettings-012` | reachability + focused suite |
| 13 | `GarmentLayeringThermalEngine` | `Textiles/GarmentLayeringThermalEngine.cs` | 1 | `ORPHAN-SEAL-W8-garmentlayeringthermal-013` | reachability + focused suite |
| 14 | `MaritimeExplorationSystem` | `Maritime/MaritimeExplorationSystem.cs` | 1 | `ORPHAN-SEAL-W8-maritimeexploration-014` | reachability + focused suite |
| 15 | `ModSupportSystem` | `Mods/ModDataContract.cs` | 1 | `ORPHAN-SEAL-W8-modsupport-015` | reachability + focused suite |
| 16 | `OilseedPressingEngine` | `Farming/OilseedPressingEngine.cs` | 1 | `ORPHAN-SEAL-W8-oilseedpressing-016` | reachability + focused suite |
| 17 | `OutpostSettlementSystem` | `Settlements/OutpostSettlementSystem.cs` | 1 | `ORPHAN-SEAL-W8-outpostsettlement-017` | reachability + focused suite |
| 18 | `PlayableMetricsAggregationEngine` | `Telemetry/PlayableMetricsAggregationEngine.cs` | 1 | `ORPHAN-SEAL-W8-playablemetricsaggregation-018` | reachability + focused suite |
| 19 | `PrecisionGlassworksOpticsEngine` | `Optics/PrecisionGlassworksOpticsEngine.cs` | 1 | `ORPHAN-SEAL-W8-precisionglassworksoptics-019` | reachability + focused suite |
| 20 | `PsychologicalProfileSystem` | `Psychology/PsychologicalProfileSystem.cs` | 1 | `ORPHAN-SEAL-W8-psychologicalprofile-020` | reachability + focused suite |
| 21 | `SeasonalCelebrationSystem` | `Events/SeasonalCelebrationSystem.cs` | 1 | `ORPHAN-SEAL-W8-seasonalcelebration-021` | reachability + focused suite |
| 22 | `SessionDurabilityManager` | `Save/SessionDurabilityManager.cs` | 1 | `ORPHAN-SEAL-W8-sessiondurabilitymanager-022` | reachability + focused suite |
| 23 | `ShelterGovernanceEngine` | `Governance/ShelterGovernanceEngine.cs` | 1 | `ORPHAN-SEAL-W8-sheltergovernance-023` | reachability + focused suite |
| 24 | `SoilReclamationProfileEngine` | `Farming/SoilReclamationProfileEngine.cs` | 1 | `ORPHAN-SEAL-W8-soilreclamationprofile-024` | reachability + focused suite |
| 25 | `SurvivorEducationSystem` | `Education/SurvivorEducationSystem.cs` | 1 | `ORPHAN-SEAL-W8-survivoreducation-025` | reachability + focused suite |
| 26 | `VisitorIntegrationSystem` | `Visitors/VisitorIntegrationSystem.cs` | 1 | `ORPHAN-SEAL-W8-visitorintegration-026` | reachability + focused suite |

**Wave exit criteria**
- every listed system is host-reachable or registered `CORE_ONLY` with a reason;
- one player-visible outcome per system (journal, panel row, CLI probe);
- save path named (existing section preferred; new section only with integrator sign-off);
- focused suites green and the reachability gate reports the wave's systems as reachable;
- handoff uses the standard block from `AI_AGENT_WORKFLOW.md`.
