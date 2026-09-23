# PLAN-ORPHAN-SEAL-01 — Appendix A: Orphan Authority Dossiers (99)

**Generated:** 2026-09-21 from the host-reachability audit (HEAD `5be1a30a`).
**Method:** `tools/reachability-audit.py` (type extraction → Core reference
graph → host roots → BFS closure).
**Scope:** every Core authority file unreachable from the Godot host, with its
types, known tests, candidate catalogs, suggested host seam, and the standard
five-step wiring recipe from the main plan §4.

**How to use this appendix**
1. The wave column matches `PLAN-ORPHAN-SEAL-01` §5; a wave package picks its
   dossiers from here.
2. The suggested seam is a *default*; the package's premise check may correct
   it (if current evidence disproves the assumption, return `STALE_PLAN`).
3. The wiring recipe is the Definition of Done: host path, day owner if
   stateful, save path, one player surface, focused verification + reachability
   re-run.
4. Candidate catalogs are name-matched suggestions, not proof of ownership;
   confirm the loader and consumer before editing.

## Summary table

| # | Authority | Core file | Tests | Catalog? |
|---|---|---|---:|---|
| 01 | `CascadeTargetSystem` | `Weather/WeatherGameplayCascadeEngine.cs` | 1 | yes |
| 02 | `SeasonalHumanMigrationEngine` | `Economy/SeasonalHumanMigrationEngine.cs` | 2 | — |
| 03 | `RestockAllocationEngine` | `Economy/RestockAllocationEngine.cs` | 1 | — |
| 04 | `TradeRouteRiskBindingEngine` | `Economy/TradeRouteRiskBindingEngine.cs` | 1 | — |
| 05 | `BlackMarketContrabandEngine` | `Economy/BlackMarketContrabandEngine.cs` | 2 | yes |
| 06 | `ChitPurityAssayEngine` | `Economy/ChitPurityAssayEngine.cs` | 1 | — |
| 07 | `LoanSharkEnforcerEngine` | `Economy/LoanSharkEnforcerEngine.cs` | 1 | — |
| 08 | `TradeRouteMonopolyEngine` | `Economy/TradeRouteMonopolyEngine.cs` | 1 | yes |
| 09 | `MigrationConsequenceEngine` | `Economy/MigrationConsequenceEngine.cs` | 1 | yes |
| 10 | `BlackMarketHeatAttentionEngine` | `Economy/BlackMarketHeatAttentionEngine.cs` | 1 | — |
| 11 | `SurvivorBarterSystem` | `Economy/SurvivorBarterSystem.cs` | 1 | yes |
| 12 | `ColonySystem` | `Expeditions/ColonySystem.cs` | 2 | yes |
| 13 | `AerialReconWindowEngine` | `Expeditions/AerialReconWindowEngine.cs` | 1 | — |
| 14 | `ClothingWarmthSystem` | `Inventory/ClothingWarmthSystem.cs` | 1 | — |
| 15 | `RehabilitationProgressionEngine` | `Medical/RehabilitationProgressionEngine.cs` | 1 | yes |
| 16 | `ProstheticConditionWearEngine` | `Medical/ProstheticConditionWearEngine.cs` | 1 | — |
| 17 | `SurgicalGraftRejectionEngine` | `Medical/SurgicalGraftRejectionEngine.cs` | 1 | — |
| 18 | `PalliativeCareDignityEngine` | `Medical/PalliativeCareDignityEngine.cs` | 1 | — |
| 19 | `DependencyTaperWithdrawalEngine` | `Medical/DependencyTaperWithdrawalEngine.cs` | 1 | — |
| 20 | `ClinicalWardTriageEngine` | `Medical/ClinicalWardTriageEngine.cs` | 2 | — |
| 21 | `SurvivorLetterDeliverySystem` | `Narrative/SurvivorLetterDeliverySystem.cs` | 1 | yes |
| 22 | `LetterDeliverySystem` | `Narrative/LetterDeliverySystem.cs` | 1 | yes |
| 23 | `NpcMemorySystem` | `Narrative/NpcMemorySystem.cs` | 2 | yes |
| 24 | `RadioPropagationEngine` | `Radio/RadioPropagation.cs` | 1 | yes |
| 25 | `CupolaFoundryEngine` | `Shelter/CupolaFoundryEngine.cs` | 1 | yes |
| 26 | `TrophySystem` | `Shelter/TrophySystem.cs` | 1 | — |
| 27 | `PowerLoadSheddingEngine` | `Shelter/PowerLoadSheddingEngine.cs` | 1 | — |
| 28 | `EmergencyMusterReadinessEngine` | `Shelter/EmergencyMusterReadinessEngine.cs` | 1 | — |
| 29 | `KilnFiringEngine` | `Shelter/KilnFiringEngine.cs` | 1 | yes |
| 30 | `ChemicalReagentSynthesisEngine` | `Shelter/ChemicalReagentSynthesisEngine.cs` | 1 | — |
| 31 | `MechanicalPowerDrivelineEngine` | `Shelter/MechanicalPowerDrivelineEngine.cs` | 1 | — |
| 32 | `ShelterIdentitySystem` | `Shelter/ShelterIdentitySystem.cs` | 1 | yes |
| 33 | `ShelterExpansionSystem` | `Shelter/ShelterExpansionSystem.cs` | 1 | yes |
| 34 | `DisasterResponseSystem` | `Shelter/DisasterResponseSystem.cs` | 1 | yes |
| 35 | `ShelterMaintenanceSystem` | `Shelter/ShelterMaintenanceSystem.cs` | 1 | yes |
| 36 | `HobbySystem` | `Survivors/HobbySystem.cs` | 2 | yes |
| 37 | `AntenatalMaternalHealthEngine` | `Survivors/AntenatalMaternalHealthEngine.cs` | 1 | — |
| 38 | `SurvivorAgingProgressionEngine` | `Survivors/SurvivorAgingProgressionEngine.cs` | 2 | — |
| 39 | `SurvivorAutonomySystem` | `Survivors/SurvivorAutonomySystem.cs` | 1 | yes |
| 40 | `BackstorySystem` | `Survivors/BackstorySystem.cs` | 1 | yes |
| 41 | `AgingSystem` | `Survivors/AgingSystem.cs` | 1 | yes |
| 42 | `SurvivorRoutineSystem` | `Survivors/SurvivorRoutineSystem.cs` | 1 | yes |
| 43 | `SurvivorRoleSystem` | `Survivors/SurvivorRoleSystem.cs` | 1 | yes |
| 44 | `RecruitmentSystem` | `Survivors/RecruitmentSystem.cs` | 1 | yes |
| 45 | `WeatherForecastReliabilityEngine` | `World/WeatherForecastReliabilityEngine.cs` | 1 | — |
| 46 | `ModalTravelDispatchEngine` | `World/ModalTravelDispatchEngine.cs` | 1 | — |
| 47 | `WildlifeHarvestQuotaEngine` | `World/WildlifeHarvestQuotaEngine.cs` | 1 | — |
| 48 | `StormForecastReadinessEngine` | `World/StormForecastReadinessEngine.cs` | 1 | — |
| 49 | `NightWatchPatrolReadinessEngine` | `World/NightWatchPatrolReadinessEngine.cs` | 1 | — |
| 50 | `SeasonalCelebrationSystem` | `Events/SeasonalCelebrationSystem.cs` | 1 | yes |
| 51 | `MaritimeExplorationSystem` | `Maritime/MaritimeExplorationSystem.cs` | 1 | yes |
| 52 | `ChemicalPlumeDispersionEngine` | `Combat/ChemicalPlumeDispersionEngine.cs` | 1 | — |
| 53 | `ContentOrphanCertificationEngine` | `Content/ContentOrphanCertificationEngine.cs` | 2 | — |
| 54 | `AudioAccessibilityCoordinator` | `Audio/AudioAccessibilityCoordinator.cs` | 1 | yes |
| 55 | `CassettePlaybackSystem` | `Audio/CassettePlaybackSystem.cs` | 1 | yes |
| 56 | `SubterraneanSubsidenceEngine` | `Excavation/SubterraneanSubsidenceEngine.cs` | 1 | yes |
| 57 | `TerritoryControlSystem` | `Factions/TerritoryControlSystem.cs` | 2 | yes |
| 58 | `SoilReclamationProfileEngine` | `Farming/SoilReclamationProfileEngine.cs` | 1 | — |
| 59 | `OilseedPressingEngine` | `Farming/OilseedPressingEngine.cs` | 1 | — |
| 60 | `CampaignLegacySystem` | `Legacy/CampaignLegacySystem.cs` | 1 | yes |
| 61 | `ConfessionSecretSystem` | `Phantoms/ConfessionSecretSystem.cs` | 2 | yes |
| 62 | `SessionDurabilityManager` | `Save/SessionDurabilityManager.cs` | 2 | yes |
| 63 | `SpiritualRitualCalendarEngine` | `Spiritual/SpiritualRitualCalendarEngine.cs` | 1 | yes |
| 64 | `CultureCreationSystem` | `Culture/CultureCreationSystem.cs` | 2 | yes |
| 65 | `ShelterFestivalEngine` | `Culture/ShelterFestivalEngine.cs` | 1 | yes |
| 66 | `ShelterMuseumSystem` | `Culture/ShelterMuseumSystem.cs` | 1 | yes |
| 67 | `PerimeterEarlyWarningEngine` | `Defense/PerimeterEarlyWarningEngine.cs` | 1 | — |
| 68 | `FactionDiplomacySystem` | `Diplomacy/FactionDiplomacySystem.cs` | 1 | yes |
| 69 | `ModSupportSystem` | `Mods/ModDataContract.cs` | 1 | yes |
| 70 | `SleepAcousticRestEngine` | `Needs/SleepAcousticRestEngine.cs` | 1 | — |
| 71 | `CommitmentSystem` | `Commitments/CommitmentSystem.cs` | 2 | yes |
| 72 | `ShelterGovernanceEngine` | `Governance/ShelterGovernanceEngine.cs` | 2 | yes |
| 73 | `DifficultySettingsSystem` | `Difficulty/DifficultySettingsSystem.cs` | 1 | yes |
| 74 | `InternalCommunicationSystem` | `Communication/InternalCommunicationSystem.cs` | 1 | yes |
| 75 | `SecondGenerationMilestoneEngine` | `Generations/SecondGenerationMilestoneEngine.cs` | 1 | — |
| 76 | `WaterQualityProfileEngine` | `Water/WaterQualityProfileEngine.cs` | 1 | yes |
| 77 | `WaterSourceSystem` | `Water/WaterSourceSystem.cs` | 2 | yes |
| 78 | `CommonTableRationingEngine` | `Nutrition/CommonTableRationingEngine.cs` | 1 | — |
| 79 | `VoiceLineSelectionEngine` | `Voice/VoiceLineSelectionEngine.cs` | 1 | yes |
| 80 | `VoiceLineDispatchCoordinator` | `Voice/VoiceLineDispatchCoordinator.cs` | 1 | yes |
| 81 | `SurvivorVoiceSystem` | `Voice/SurvivorVoiceSystem.cs` | 2 | yes |
| 82 | `PlayableMetricsAggregationEngine` | `Telemetry/PlayableMetricsAggregationEngine.cs` | 1 | — |
| 83 | `InformantNetworkTradecraftEngine` | `Espionage/InformantNetworkTradecraftEngine.cs` | 1 | — |
| 84 | `GarmentLayeringThermalEngine` | `Textiles/GarmentLayeringThermalEngine.cs` | 1 | — |
| 85 | `ApprenticeshipCurriculumEngine` | `Education/ApprenticeshipCurriculumEngine.cs` | 1 | yes |
| 86 | `SurvivorEducationSystem` | `Education/SurvivorEducationSystem.cs` | 1 | yes |
| 87 | `PrecisionGlassworksOpticsEngine` | `Optics/PrecisionGlassworksOpticsEngine.cs` | 1 | yes |
| 88 | `PublicBroadsheetPressEngine` | `Print/PublicBroadsheetPressEngine.cs` | 1 | — |
| 89 | `OutpostSettlementSystem` | `Settlements/OutpostSettlementSystem.cs` | 1 | yes |
| 90 | `WeatherCascadeSystem` | `Weather/WeatherCascadeSystem.cs` | 1 | yes |
| 91 | `NuclearWinterProgressionSystem` | `Weather/NuclearWinterProgressionSystem.cs` | 1 | yes |
| 92 | `CookingSystem` | `Cooking/CookingSystem.cs` | 1 | yes |
| 93 | `CommunicationsSystem` | `Communications/CommunicationsSystem.cs` | 1 | yes |
| 94 | `PsychologicalProfileSystem` | `Psychology/PsychologicalProfileSystem.cs` | 1 | yes |
| 95 | `AccessibilitySettingsSystem` | `Accessibility/AccessibilitySettingsSystem.cs` | 1 | yes |
| 96 | `BestiarySystem` | `Bestiary/BestiarySystem.cs` | 1 | yes |
| 97 | `EmergencyAlertSystem` | `Emergency/EmergencyAlertSystem.cs` | 1 | yes |
| 98 | `FoodTypeSystem` | `Kitchen/FoodTypeSystem.cs` | 2 | yes |
| 99 | `VisitorIntegrationSystem` | `Visitors/VisitorIntegrationSystem.cs` | 1 | yes |
## Dossiers

### A.01 `CascadeTargetSystem`
- **File:** `Weather/WeatherGameplayCascadeEngine.cs` · **Types in file:** `CascadeTargetSystem`, `WeatherGameplayCascadeEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `Weather/Plan135WeatherCascadeIntegrationTests.cs`
- **Candidate catalogs:** `comms_targets.json`, `cascade_rules.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.02 `SeasonalHumanMigrationEngine`
- **File:** `Economy/SeasonalHumanMigrationEngine.cs` · **Types in file:** `SeasonalHumanMigrationEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (2):** `Economy/SeasonalHumanMigrationEngineTests.cs`, `Economy/MigrationConsequenceEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.03 `RestockAllocationEngine`
- **File:** `Economy/RestockAllocationEngine.cs` · **Types in file:** `RestockAllocationEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/RestockAllocationEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.04 `TradeRouteRiskBindingEngine`
- **File:** `Economy/TradeRouteRiskBindingEngine.cs` · **Types in file:** `TradeRouteRiskBindingEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/TradeRouteRiskBindingEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.05 `BlackMarketContrabandEngine`
- **File:** `Economy/BlackMarketContrabandEngine.cs` · **Types in file:** `BlackMarketContrabandEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (2):** `Economy/BlackMarketContrabandEngineTests.cs`, `Economy/ContrabandMarketIntegrationTests.cs`
- **Candidate catalogs:** `black_market_inventory.json`
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.06 `ChitPurityAssayEngine`
- **File:** `Economy/ChitPurityAssayEngine.cs` · **Types in file:** `ChitPurityAssayEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/ChitPurityAssayEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.07 `LoanSharkEnforcerEngine`
- **File:** `Economy/LoanSharkEnforcerEngine.cs` · **Types in file:** `LoanSharkEnforcerEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/LoanSharkEnforcerEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.08 `TradeRouteMonopolyEngine`
- **File:** `Economy/TradeRouteMonopolyEngine.cs` · **Types in file:** `TradeRouteMonopolyEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/TradeRouteMonopolyEngineTests.cs`
- **Candidate catalogs:** `caravan_trade_routes.json`, `wasteland_trade_caravan_routes.json`
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.09 `MigrationConsequenceEngine`
- **File:** `Economy/MigrationConsequenceEngine.cs` · **Types in file:** `MigrationConsequenceEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/MigrationConsequenceEngineTests.cs`
- **Candidate catalogs:** `foundry_treaty_consequences.json`, `discovery_consequences.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.10 `BlackMarketHeatAttentionEngine`
- **File:** `Economy/BlackMarketHeatAttentionEngine.cs` · **Types in file:** `BlackMarketHeatAttentionEngine`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/BlackMarketHeatAttentionEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.11 `SurvivorBarterSystem`
- **File:** `Economy/SurvivorBarterSystem.cs` · **Types in file:** `SurvivorBarterSystem`
- **Wave:** Wave 3 — economy depth
- **Known tests (1):** `Economy/Plan213SurvivorBarterIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.12 `ColonySystem`
- **File:** `Expeditions/ColonySystem.cs` · **Types in file:** `ColonySystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (2):** `Expeditions/ColonySystemTests.cs`, `Expeditions/Plan160ColonyIntegrationTests.cs`
- **Candidate catalogs:** `colony_blueprints.json`
- **Suggested host seam:** expedition/logistics host session
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.13 `AerialReconWindowEngine`
- **File:** `Expeditions/AerialReconWindowEngine.cs` · **Types in file:** `AerialReconWindowEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Expeditions/AerialReconWindowEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.14 `ClothingWarmthSystem`
- **File:** `Inventory/ClothingWarmthSystem.cs` · **Types in file:** `ClothingWarmthSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Inventory/ClothingWarmthSystemTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.15 `RehabilitationProgressionEngine`
- **File:** `Medical/RehabilitationProgressionEngine.cs` · **Types in file:** `RehabilitationProgressionEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Medical/RehabilitationProgressionEngineTests.cs`
- **Candidate catalogs:** `narrative_progression.json`
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.16 `ProstheticConditionWearEngine`
- **File:** `Medical/ProstheticConditionWearEngine.cs` · **Types in file:** `ProstheticConditionWearEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Medical/ProstheticConditionWearEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.17 `SurgicalGraftRejectionEngine`
- **File:** `Medical/SurgicalGraftRejectionEngine.cs` · **Types in file:** `SurgicalGraftRejectionEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Medical/SurgicalGraftRejectionEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.18 `PalliativeCareDignityEngine`
- **File:** `Medical/PalliativeCareDignityEngine.cs` · **Types in file:** `PalliativeCareDignityEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Medical/PalliativeCareDignityEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.19 `DependencyTaperWithdrawalEngine`
- **File:** `Medical/DependencyTaperWithdrawalEngine.cs` · **Types in file:** `DependencyTaperWithdrawalEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Medical/DependencyTaperWithdrawalEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.20 `ClinicalWardTriageEngine`
- **File:** `Medical/ClinicalWardTriageEngine.cs` · **Types in file:** `ClinicalWardTriageEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (2):** `Medical/ClinicalWardTriageEngineTests.cs`, `Medical/Plan16_19TriageEpilogueIntegrationTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.21 `SurvivorLetterDeliverySystem`
- **File:** `Narrative/SurvivorLetterDeliverySystem.cs` · **Types in file:** `SurvivorLetterDeliverySystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `NarrativeAndFactionWarIntegrationTests.cs`
- **Candidate catalogs:** `survivor_letters_lost_kin.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.22 `LetterDeliverySystem`
- **File:** `Narrative/LetterDeliverySystem.cs` · **Types in file:** `LetterDeliverySystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `LetterDeliverySystemTests.cs`
- **Candidate catalogs:** `letters_expansion.json`, `survivor_letters_lost_kin.json`, `unsent_letters_batch_2.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.23 `NpcMemorySystem`
- **File:** `Narrative/NpcMemorySystem.cs` · **Types in file:** `NpcMemorySystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (2):** `Narrative/NpcMemorySystemTests.cs`, `Narrative/Plan147_151NpcAnimalIntegrationTests.cs`
- **Candidate catalogs:** `standing_record_memory.json`, `wasteland_settlement_npcs.json`, `narrative_encounters_npc_arcs.json`, `npc_arcs.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.24 `RadioPropagationEngine`
- **File:** `Radio/RadioPropagation.cs` · **Types in file:** `RadioPropagationEngine`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Radio/RadioPropagationTests.cs`
- **Candidate catalogs:** `faction_war_radio.json`, `radio_intercepts.json`, `year_of_ash_radio.json`, `faction_radio_corpus.json`
- **Suggested host seam:** expedition/logistics host session
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.25 `CupolaFoundryEngine`
- **File:** `Shelter/CupolaFoundryEngine.cs` · **Types in file:** `CupolaFoundryEngine`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/CupolaFoundryEngineTests.cs`
- **Candidate catalogs:** `foundry_faction.json`, `foundry_items.json`, `cupola_foundry_catalog.json`, `foundry_accords.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.26 `TrophySystem`
- **File:** `Shelter/TrophySystem.cs` · **Types in file:** `TrophySystem`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/TrophyPipelineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.27 `PowerLoadSheddingEngine`
- **File:** `Shelter/PowerLoadSheddingEngine.cs` · **Types in file:** `PowerLoadSheddingEngine`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/PowerLoadSheddingEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.28 `EmergencyMusterReadinessEngine`
- **File:** `Shelter/EmergencyMusterReadinessEngine.cs` · **Types in file:** `EmergencyMusterReadinessEngine`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/EmergencyMusterReadinessEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.29 `KilnFiringEngine`
- **File:** `Shelter/KilnFiringEngine.cs` · **Types in file:** `KilnFiringEngine`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/KilnFiringEngineTests.cs`
- **Candidate catalogs:** `bisque_firing_records.json`, `kiln_draw_trial_assays.json`, `lime_kiln_calcination_logs.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.30 `ChemicalReagentSynthesisEngine`
- **File:** `Shelter/ChemicalReagentSynthesisEngine.cs` · **Types in file:** `ChemicalReagentSynthesisEngine`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/ChemicalReagentSynthesisEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.31 `MechanicalPowerDrivelineEngine`
- **File:** `Shelter/MechanicalPowerDrivelineEngine.cs` · **Types in file:** `MechanicalPowerDrivelineEngine`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/MechanicalPowerDrivelineEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.32 `ShelterIdentitySystem`
- **File:** `Shelter/ShelterIdentitySystem.cs` · **Types in file:** `ShelterIdentitySystem`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/ShelterIdentitySystemTests.cs`
- **Candidate catalogs:** `shelter_machine_identities.json`, `shelter_room_identities.json`, `shelter_social_events.json`, `shelter_audio_cues.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.33 `ShelterExpansionSystem`
- **File:** `Shelter/ShelterExpansionSystem.cs` · **Types in file:** `ShelterExpansionSystem`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/Plan156ShelterExpansionIntegrationTests.cs`
- **Candidate catalogs:** `audio_logs_expansion_05.json`, `environmental_atmosphere_expansion.json`, `environmental_texts_expansion_05.json`, `journal_entries_expansion_05.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.34 `DisasterResponseSystem`
- **File:** `Shelter/DisasterResponseSystem.cs` · **Types in file:** `DisasterResponseSystem`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/Plan158DisasterResponseIntegrationTests.cs`
- **Candidate catalogs:** `disaster_templates.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.35 `ShelterMaintenanceSystem`
- **File:** `Shelter/ShelterMaintenanceSystem.cs` · **Types in file:** `ShelterMaintenanceSystem`
- **Wave:** Wave 2 — shelter industry
- **Known tests (1):** `Shelter/Plan186ShelterMaintenanceIntegrationTests.cs`
- **Candidate catalogs:** `shelter_machine_identities.json`, `shelter_room_identities.json`, `shelter_social_events.json`, `shelter_audio_cues.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.36 `HobbySystem`
- **File:** `Survivors/HobbySystem.cs` · **Types in file:** `HobbySystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (2):** `Survivors/HobbySystemTests.cs`, `Survivors/Plan161HobbyIntegrationTests.cs`
- **Candidate catalogs:** `hobby_definitions.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.37 `AntenatalMaternalHealthEngine`
- **File:** `Survivors/AntenatalMaternalHealthEngine.cs` · **Types in file:** `AntenatalMaternalHealthEngine`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Survivors/AntenatalMaternalHealthEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.38 `SurvivorAgingProgressionEngine`
- **File:** `Survivors/SurvivorAgingProgressionEngine.cs` · **Types in file:** `SurvivorAgingProgressionEngine`
- **Wave:** Wave 4 — survivor life
- **Known tests (2):** `Survivors/SurvivorAgingProgressionEngineTests.cs`, `Survivors/Plan176AgingSystemIntegrationTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.39 `SurvivorAutonomySystem`
- **File:** `Survivors/SurvivorAutonomySystem.cs` · **Types in file:** `SurvivorAutonomySystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Survivors/Plan144SurvivorAutonomyIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.40 `BackstorySystem`
- **File:** `Survivors/BackstorySystem.cs` · **Types in file:** `BackstorySystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Survivors/Plan174SurvivorBackstoriesIntegrationTests.cs`
- **Candidate catalogs:** `backstory_templates.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.41 `AgingSystem`
- **File:** `Survivors/AgingSystem.cs` · **Types in file:** `AgingSystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Survivors/Plan176AgingSystemIntegrationTests.cs`
- **Candidate catalogs:** `lead_crystal_scintillator_aging_logs.json`
- **Suggested host seam:** medical host session / body-state projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.42 `SurvivorRoutineSystem`
- **File:** `Survivors/SurvivorRoutineSystem.cs` · **Types in file:** `SurvivorRoutineSystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Survivors/Plan188SurvivorRoutineIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.43 `SurvivorRoleSystem`
- **File:** `Survivors/SurvivorRoleSystem.cs` · **Types in file:** `SurvivorRoleSystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Survivors/Plan195SurvivorRoleIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.44 `RecruitmentSystem`
- **File:** `Survivors/RecruitmentSystem.cs` · **Types in file:** `RecruitmentSystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Survivors/Plan204RecruitmentIntegrationTests.cs`
- **Candidate catalogs:** `recruitment_templates.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.45 `WeatherForecastReliabilityEngine`
- **File:** `World/WeatherForecastReliabilityEngine.cs` · **Types in file:** `WeatherForecastReliabilityEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `World/WeatherForecastReliabilityEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.46 `ModalTravelDispatchEngine`
- **File:** `World/ModalTravelDispatchEngine.cs` · **Types in file:** `ModalTravelDispatchEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `World/ModalTravelDispatchEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.47 `WildlifeHarvestQuotaEngine`
- **File:** `World/WildlifeHarvestQuotaEngine.cs` · **Types in file:** `WildlifeHarvestQuotaEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `World/WildlifeHarvestQuotaEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.48 `StormForecastReadinessEngine`
- **File:** `World/StormForecastReadinessEngine.cs` · **Types in file:** `StormForecastReadinessEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `World/StormForecastReadinessEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.49 `NightWatchPatrolReadinessEngine`
- **File:** `World/NightWatchPatrolReadinessEngine.cs` · **Types in file:** `NightWatchPatrolReadinessEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `World/NightWatchPatrolReadinessEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.50 `SeasonalCelebrationSystem`
- **File:** `Events/SeasonalCelebrationSystem.cs` · **Types in file:** `SeasonalCelebrationSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Events/Plan170SeasonalCelebrationsIntegrationTests.cs`
- **Candidate catalogs:** `seasonal_events.json`, `shelter_celebrations.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.51 `MaritimeExplorationSystem`
- **File:** `Maritime/MaritimeExplorationSystem.cs` · **Types in file:** `MaritimeExplorationSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Maritime/Plan207MaritimeExplorationIntegrationTests.cs`
- **Candidate catalogs:** `gpr_exploration_catalog.json`, `maritime_zones.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.52 `ChemicalPlumeDispersionEngine`
- **File:** `Combat/ChemicalPlumeDispersionEngine.cs` · **Types in file:** `ChemicalPlumeDispersionEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Combat/ChemicalPlumeDispersionEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.53 `ContentOrphanCertificationEngine`
- **File:** `Content/ContentOrphanCertificationEngine.cs` · **Types in file:** `ContentOrphanCertificationEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (2):** `Content/ContentOrphanCertificationEngineTests.cs`, `Content/Plan49ContentAtmosphereIntegrationTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.54 `AudioAccessibilityCoordinator`
- **File:** `Audio/AudioAccessibilityCoordinator.cs` · **Types in file:** `AudioAccessibilityCoordinator`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Audio/Plan169AudioAccessibilityIntegrationTests.cs`
- **Candidate catalogs:** `audio_logs_expansion_05.json`, `audio_cues.json`, `shelter_audio_cues.json`, `audio_accessibility_cues.json`
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.55 `CassettePlaybackSystem`
- **File:** `Audio/CassettePlaybackSystem.cs` · **Types in file:** `CassettePlaybackSystem`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Verdict/Plan82_67VerdictCassetteIntegrationTests.cs`
- **Candidate catalogs:** `cassette_sets.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.56 `SubterraneanSubsidenceEngine`
- **File:** `Excavation/SubterraneanSubsidenceEngine.cs` · **Types in file:** `SubterraneanSubsidenceEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `Excavation/SubterraneanSubsidenceEngineTests.cs`
- **Candidate catalogs:** `subterranean_zones.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.57 `TerritoryControlSystem`
- **File:** `Factions/TerritoryControlSystem.cs` · **Types in file:** `TerritoryControlSystem`
- **Wave:** Wave 7 — culture & comms
- **Known tests (2):** `World/Plan43_44SettlementTerritoryIntegrationTests.cs`, `Factions/Plan134TerritoryControlIntegrationTests.cs`
- **Candidate catalogs:** `faction_territory.json`
- **Suggested host seam:** faction day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.58 `SoilReclamationProfileEngine`
- **File:** `Farming/SoilReclamationProfileEngine.cs` · **Types in file:** `SoilReclamationProfileEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Farming/SoilReclamationProfileEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** production/treatment day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.59 `OilseedPressingEngine`
- **File:** `Farming/OilseedPressingEngine.cs` · **Types in file:** `OilseedPressingEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Farming/OilseedPressingTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** production/treatment day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.60 `CampaignLegacySystem`
- **File:** `Legacy/CampaignLegacySystem.cs` · **Types in file:** `CampaignLegacySystem`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Legacy/Plan140GenerationalLegacyIntegrationTests.cs`
- **Candidate catalogs:** `campaign_epilogues.json`, `propaganda_campaigns.json`, `death_legacy_templates.json`, `legacy_traits.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.61 `ConfessionSecretSystem`
- **File:** `Phantoms/ConfessionSecretSystem.cs` · **Types in file:** `ConfessionSecretSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (2):** `ConfessionSecretSystemTests.cs`, `Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs`
- **Candidate catalogs:** `confession_secrets.json`, `wire_confessions.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.62 `SessionDurabilityManager`
- **File:** `Save/SessionDurabilityManager.cs` · **Types in file:** `SessionDurabilityManager`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (2):** `Save/SessionDurabilityManagerTests.cs`, `Launch/Plan57StoreKitTruthIntegrationTests.cs`
- **Candidate catalogs:** `education_session_records.json`, `therapist_session_notes.json`, `therapist_session_notes_batch_2.json`, `therapist_session_notes_batch_3.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.63 `SpiritualRitualCalendarEngine`
- **File:** `Spiritual/SpiritualRitualCalendarEngine.cs` · **Types in file:** `SpiritualRitualCalendarEngine`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Spiritual/SpiritualRitualCalendarEngineTests.cs`
- **Candidate catalogs:** `spiritual_rituals.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.64 `CultureCreationSystem`
- **File:** `Culture/CultureCreationSystem.cs` · **Types in file:** `CultureCreationSystem`
- **Wave:** Wave 7 — culture & comms
- **Known tests (2):** `Survivors/Plan178ArtCultureIntegrationTests.cs`, `Culture/CultureCreationSystemTests.cs`
- **Candidate catalogs:** `recreation.json`, `agriculture_items.json`, `muster_faction_culture.json`, `apiculture_red_light_audits.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.65 `ShelterFestivalEngine`
- **File:** `Culture/ShelterFestivalEngine.cs` · **Types in file:** `ShelterFestivalEngine`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Culture/ShelterFestivalEngineTests.cs`
- **Candidate catalogs:** `shelter_machine_identities.json`, `shelter_room_identities.json`, `shelter_social_events.json`, `shelter_audio_cues.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.66 `ShelterMuseumSystem`
- **File:** `Culture/ShelterMuseumSystem.cs` · **Types in file:** `ShelterMuseumSystem`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Culture/Plan218MuseumIntegrationTests.cs`
- **Candidate catalogs:** `shelter_machine_identities.json`, `shelter_room_identities.json`, `shelter_social_events.json`, `shelter_audio_cues.json`
- **Suggested host seam:** shelter day-owner + room/condition projection
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.67 `PerimeterEarlyWarningEngine`
- **File:** `Defense/PerimeterEarlyWarningEngine.cs` · **Types in file:** `PerimeterEarlyWarningEngine`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `Defense/PerimeterEarlyWarningEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.68 `FactionDiplomacySystem`
- **File:** `Diplomacy/FactionDiplomacySystem.cs` · **Types in file:** `FactionDiplomacySystem`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Diplomacy/Plan197FactionDiplomacyIntegrationTests.cs`
- **Candidate catalogs:** `faction_territory.json`, `faction_war_events.json`, `faction_war_journal.json`, `faction_war_radio.json`
- **Suggested host seam:** faction day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.69 `ModSupportSystem`
- **File:** `Mods/ModDataContract.cs` · **Types in file:** `ModSupportSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Mods/Plan165ModdingIntegrationTests.cs`
- **Candidate catalogs:** `vehicle_modifications.json`, `armored_crawler_modules.json`, `commodity_baselines.json`, `vehicle_modules.json`
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.70 `SleepAcousticRestEngine`
- **File:** `Needs/SleepAcousticRestEngine.cs` · **Types in file:** `SleepAcousticRestEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Needs/SleepAcousticRestEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.71 `CommitmentSystem`
- **File:** `Commitments/CommitmentSystem.cs` · **Types in file:** `CommitmentSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (2):** `Campaign/CommitmentSystemTests.cs`, `Campaign/Plan33_38IntelCalendarIntegrationTests.cs`
- **Candidate catalogs:** `commitments.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.72 `ShelterGovernanceEngine`
- **File:** `Governance/ShelterGovernanceEngine.cs` · **Types in file:** `ShelterGovernanceEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (2):** `Governance/ShelterGovernanceEngineTests.cs`, `Governance/Plan159_190GovernanceProvenanceIntegrationTests.cs`
- **Candidate catalogs:** `shelter_machine_identities.json`, `shelter_room_identities.json`, `shelter_social_events.json`, `shelter_audio_cues.json`
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.73 `DifficultySettingsSystem`
- **File:** `Difficulty/DifficultySettingsSystem.cs` · **Types in file:** `DifficultySettingsSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Difficulty/Plan181DifficultySettingsIntegrationTests.cs`
- **Candidate catalogs:** `difficulty_presets.json`
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.74 `InternalCommunicationSystem`
- **File:** `Communication/InternalCommunicationSystem.cs` · **Types in file:** `InternalCommunicationSystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Communication/Plan211InternalCommunicationIntegrationTests.cs`
- **Candidate catalogs:** `nvis_communications_catalog.json`, `communications_networks.json`, `communication_templates.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.75 `SecondGenerationMilestoneEngine`
- **File:** `Generations/SecondGenerationMilestoneEngine.cs` · **Types in file:** `SecondGenerationMilestoneEngine`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Generations/SecondGenerationMilestoneEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.76 `WaterQualityProfileEngine`
- **File:** `Water/WaterQualityProfileEngine.cs` · **Types in file:** `WaterQualityProfileEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Water/WaterQualityProfileEngineTests.cs`
- **Candidate catalogs:** `water_quality_test_reports_batch_2.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.77 `WaterSourceSystem`
- **File:** `Water/WaterSourceSystem.cs` · **Types in file:** `WaterSourceSystem`
- **Wave:** Wave 5 — medical & table
- **Known tests (2):** `Water/Plan189WaterSourceIntegrationTests.cs`, `Emergency/Plan194EmergencyAlertIntegrationTests.cs`
- **Candidate catalogs:** `guilt_sources.json`, `water_sources.json`, `noise_sources.json`, `boiler_feedwater_deaerator_audits.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.78 `CommonTableRationingEngine`
- **File:** `Nutrition/CommonTableRationingEngine.cs` · **Types in file:** `CommonTableRationingEngine`
- **Wave:** Wave 5 — medical & table
- **Known tests (1):** `Nutrition/CommonTableRationingEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.79 `VoiceLineSelectionEngine`
- **File:** `Voice/VoiceLineSelectionEngine.cs` · **Types in file:** `VoiceLineSelectionEngine`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Voice/VoiceLineSelectionEngineTests.cs`
- **Candidate catalogs:** `survivor_voice_lines.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.80 `VoiceLineDispatchCoordinator`
- **File:** `Voice/VoiceLineDispatchCoordinator.cs` · **Types in file:** `VoiceLineDispatchCoordinator`
- **Wave:** Wave 4 — survivor life
- **Known tests (1):** `Voice/VoiceLineDispatchCoordinatorTests.cs`
- **Candidate catalogs:** `survivor_voice_lines.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.81 `SurvivorVoiceSystem`
- **File:** `Voice/SurvivorVoiceSystem.cs` · **Types in file:** `SurvivorVoiceSystem`
- **Wave:** Wave 4 — survivor life
- **Known tests (2):** `Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Voice/SurvivorVoiceSystemTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.82 `PlayableMetricsAggregationEngine`
- **File:** `Telemetry/PlayableMetricsAggregationEngine.cs` · **Types in file:** `PlayableMetricsAggregationEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Telemetry/PlayableMetricsAggregationEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.83 `InformantNetworkTradecraftEngine`
- **File:** `Espionage/InformantNetworkTradecraftEngine.cs` · **Types in file:** `InformantNetworkTradecraftEngine`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Espionage/InformantNetworkTradecraftEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** economy day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.84 `GarmentLayeringThermalEngine`
- **File:** `Textiles/GarmentLayeringThermalEngine.cs` · **Types in file:** `GarmentLayeringThermalEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Textiles/GarmentLayeringThermalEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.85 `ApprenticeshipCurriculumEngine`
- **File:** `Education/ApprenticeshipCurriculumEngine.cs` · **Types in file:** `ApprenticeshipCurriculumEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Education/ApprenticeshipCurriculumEngineTests.cs`
- **Candidate catalogs:** `apprenticeship_catalog.json`, `education_curriculum.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.86 `SurvivorEducationSystem`
- **File:** `Education/SurvivorEducationSystem.cs` · **Types in file:** `SurvivorEducationSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Education/Plan154EducationIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.87 `PrecisionGlassworksOpticsEngine`
- **File:** `Optics/PrecisionGlassworksOpticsEngine.cs` · **Types in file:** `PrecisionGlassworksOpticsEngine`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Optics/PrecisionGlassworksOpticsEngineTests.cs`
- **Candidate catalogs:** `precision_optics_catalog.json`
- **Suggested host seam:** production/treatment day-owner consumer
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.88 `PublicBroadsheetPressEngine`
- **File:** `Print/PublicBroadsheetPressEngine.cs` · **Types in file:** `PublicBroadsheetPressEngine`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Print/PublicBroadsheetPressEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Suggested host seam:** culture host session + journal/audio bridge
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.89 `OutpostSettlementSystem`
- **File:** `Settlements/OutpostSettlementSystem.cs` · **Types in file:** `OutpostSettlementSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Settlements/Plan58OutpostSettlementIntegrationTests.cs`
- **Candidate catalogs:** `wasteland_settlement_npcs.json`, `settlements.json`, `outposts.json`, `wasteland_settlement_gazetteer.json`
- **Suggested host seam:** expedition/logistics host session
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.90 `WeatherCascadeSystem`
- **File:** `Weather/WeatherCascadeSystem.cs` · **Types in file:** `WeatherCascadeSystem`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `Weather/Plan135WeatherCascadeIntegrationTests.cs`
- **Candidate catalogs:** `weather_hardening_upgrades.json`, `weather_route_gates.json`, `weather_effects.json`, `weather_seasons.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.91 `NuclearWinterProgressionSystem`
- **File:** `Weather/NuclearWinterProgressionSystem.cs` · **Types in file:** `NuclearWinterProgressionSystem`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `Weather/Plan164NuclearWinterIntegrationTests.cs`
- **Candidate catalogs:** `nuclear_winter_phases.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.92 `CookingSystem`
- **File:** `Cooking/CookingSystem.cs` · **Types in file:** `CookingSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Cooking/Plan136WildlifeCookingIntegrationTests.cs`
- **Candidate catalogs:** `recipes_cooking.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.93 `CommunicationsSystem`
- **File:** `Communications/CommunicationsSystem.cs` · **Types in file:** `CommunicationsSystem`
- **Wave:** Wave 7 — culture & comms
- **Known tests (1):** `Communications/Plan157CommunicationsIntegrationTests.cs`
- **Candidate catalogs:** `nvis_communications_catalog.json`, `communications_networks.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.94 `PsychologicalProfileSystem`
- **File:** `Psychology/PsychologicalProfileSystem.cs` · **Types in file:** `PsychologicalProfileSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Survivors/Plan179UnifiedPsychologyIntegrationTests.cs`
- **Candidate catalogs:** `infiltrator_profiles.json`, `psychological_therapies.json`, `psychological_trauma.json`, `nuclear_core_profiles.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.95 `AccessibilitySettingsSystem`
- **File:** `Accessibility/AccessibilitySettingsSystem.cs` · **Types in file:** `AccessibilitySettingsSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`
- **Candidate catalogs:** `audio_accessibility_cues.json`, `accessibility_profiles.json`
- **Suggested host seam:** read-only projection + CLI probe (no day tick)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.96 `BestiarySystem`
- **File:** `Bestiary/BestiarySystem.cs` · **Types in file:** `BestiarySystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Bestiary/Plan187BestiaryIntegrationTests.cs`
- **Candidate catalogs:** `wasteland_wildlife_bestiary.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.97 `EmergencyAlertSystem`
- **File:** `Emergency/EmergencyAlertSystem.cs` · **Types in file:** `EmergencyAlertSystem`
- **Wave:** Wave 6 — world & risk
- **Known tests (1):** `Emergency/Plan194EmergencyAlertIntegrationTests.cs`
- **Candidate catalogs:** `emergency_alerts.json`
- **Suggested host seam:** host session (pattern: src/Host/<Domain>HostSession.cs)
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.98 `FoodTypeSystem`
- **File:** `Kitchen/FoodTypeSystem.cs` · **Types in file:** `FoodTypeSystem`
- **Wave:** Wave 5 — medical & table
- **Known tests (2):** `Kitchen/Plan196FoodTypeIntegrationTests.cs`, `Kitchen/Plan22_40FoodIdentityIntegrationTests.cs`
- **Candidate catalogs:** `food_preservation.json`, `food_types.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.

### A.99 `VisitorIntegrationSystem`
- **File:** `Visitors/VisitorIntegrationSystem.cs` · **Types in file:** `VisitorIntegrationSystem`
- **Wave:** Wave 8 — logistics & meta
- **Known tests (1):** `Visitors/Plan214VisitorIntegrationTests.cs`
- **Candidate catalogs:** `visitor_templates.json`
- **Suggested host seam:** needs/day-owner adapter
- **Wiring recipe:** (1) host session or projection; (2) day owner if it mutates after load; (3) save via existing section or one integrator-approved section; (4) one player surface (panel region, journal line, or CLI probe); (5) focused test + reachability re-run.
