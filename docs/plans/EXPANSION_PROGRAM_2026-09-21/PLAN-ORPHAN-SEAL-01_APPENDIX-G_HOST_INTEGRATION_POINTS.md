# PLAN-ORPHAN-SEAL-01 — Appendix G: Host Integration Point Map

**Generated:** 2026-09-21. For each of the 99 host-unreachable authorities, the
**most plausible existing host attachment points**: a `Main.*` partial with a
matching domain word, registry save sections whose key/file matches, and any
existing CLI flag with the domain word. These are *candidates*: a package
re-verifies the real seam per Plan 1 §3 and Appendix C patterns — the map
exists to make that verification fast and to show where no candidate exists.
**Reading a row:** `—` in a column means the audit found no name match; that is
a finding, not a failure. A row with no host partial and no save section is
likely Core-only or needs a new surface decision (EP-01).

| Authority | Candidate `src/Main.*` partials | Registry sections | CLI flags |
|---|---|---|---|
| `AccessibilitySettingsSystem` | — | — | `--accessibility-selftest`, `--ui-access-selftest`, `--ui-accessibility-selftest` |
| `AudioAccessibilityCoordinator` | `Main.Audio.cs` | — | `--audio-selftest`, `--audio-test` |
| `CassettePlaybackSystem` | `Main.Audio.cs` | — | `--audio-selftest`, `--audio-test` |
| `BestiarySystem` | — | — | — |
| `ChemicalPlumeDispersionEngine` | `Main.ChemicalSynthesis.cs` | `combat`, `chemical_dependency`, `sound_ranging` | `--combat-breaching-selftest`, `--combat-selftest` |
| `CommitmentSystem` | — | — | — |
| `InternalCommunicationSystem` | — | `time_capsules`, `nvis_communications` | `--communique-board-selftest`, `--faction-communique-board-selftest` |
| `CommunicationsSystem` | — | `nvis_communications` | `--communique-board-selftest`, `--faction-communique-board-selftest` |
| `ContentOrphanCertificationEngine` | — | — | `--content-utilization`, `--content-utilization-selftest` |
| `CookingSystem` | — | — | — |
| `CultureCreationSystem` | — | `agriculture` | `--agriculture-selftest` |
| `ShelterFestivalEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `agriculture` | `--agriculture-selftest` |
| `ShelterMuseumSystem` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `agriculture` | `--agriculture-selftest` |
| `PerimeterEarlyWarningEngine` | `Main.SkyDefense.cs` | `sky_defense_battery`, `perimeter_defense`, `settlement_defenses` | `--defense-selftest`, `--sky-defense-selftest` |
| `DifficultySettingsSystem` | `Main.Difficulty.cs` | — | `--difficulty-selftest` |
| `FactionDiplomacySystem` | `Main.FactionBranch.cs` | — | — |
| `BlackMarketContrabandEngine` | `Main.BlackMarket.cs`, `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `BlackMarketHeatAttentionEngine` | `Main.BlackMarket.cs`, `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `ChitPurityAssayEngine` | `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `LoanSharkEnforcerEngine` | `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `MigrationConsequenceEngine` | `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `RestockAllocationEngine` | `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `SeasonalHumanMigrationEngine` | `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `SurvivorBarterSystem` | `Main.Economy.cs`, `Main.SurvivorDeathLegacy.cs`, `Main.SurvivorFate.cs` | `survivors`, `economy`, `black_market` | `--economy-selftest`, `--economy-uitest` |
| `TradeRouteMonopolyEngine` | `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `TradeRouteRiskBindingEngine` | `Main.Economy.cs`, `Main.UiTests.Economy.cs` | `economy`, `black_market`, `mercenary_bounties` | `--economy-selftest`, `--economy-uitest` |
| `ApprenticeshipCurriculumEngine` | — | `apprenticeship` | — |
| `SurvivorEducationSystem` | `Main.SurvivorDeathLegacy.cs`, `Main.SurvivorFate.cs`, `Main.SurvivorFitness.cs` | `survivors`, `survivor_relations`, `survivor_social` | — |
| `EmergencyAlertSystem` | — | — | — |
| `InformantNetworkTradecraftEngine` | — | `espionage`, `faction_espionage` | — |
| `SeasonalCelebrationSystem` | — | `host_event`, `moral_choice` | — |
| `SubterraneanSubsidenceEngine` | `Main.Subterranean.cs` | `excavation`, `subterranean`, `excavation_hazards` | — |
| `AerialReconWindowEngine` | `Main.Expeditions.cs`, `Main.UiTests.Expeditions.cs` | `expedition`, `recon_telemetry`, `runflat_tire` | `--expedition-encounter-bridge-selftest`, `--expedition-panel-lifecycle`, `--expedition-panel-uitest` |
| `ColonySystem` | `Main.Expeditions.cs`, `Main.UiTests.Expeditions.cs` | `expedition`, `recon_telemetry`, `runflat_tire` | `--expedition-encounter-bridge-selftest`, `--expedition-panel-lifecycle`, `--expedition-panel-uitest` |
| `TerritoryControlSystem` | — | `regional_treaty`, `counter_intelligence`, `espionage` | `--faction-communique-board-selftest`, `--faction-ecology-selftest` |
| `OilseedPressingEngine` | — | `fungi_cultivation`, `bio_fermentation`, `hydroponic_biomes` | — |
| `SoilReclamationProfileEngine` | — | `fungi_cultivation`, `bio_fermentation`, `hydroponic_biomes` | — |
| `SecondGenerationMilestoneEngine` | — | — | `--ui-snapshot-regenerate` |
| `ShelterGovernanceEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | — | — |
| `ClothingWarmthSystem` | `Main.Inventory.cs`, `Main.UiTests.Inventory.cs` | `inventory`, `collectible_discovery`, `unique_claims` | `--inventory-save-selftest`, `--inventory-selftest`, `--inventory-uitest` |
| `FoodTypeSystem` | — | `kitchen_nutrition` | — |
| `CampaignLegacySystem` | `Main.Campaign.cs`, `Main.CampaignOwners.cs`, `Main.CampaignServices.cs` | `campaign_day`, `propaganda_campaigns`, `death_legacy` | `--death-legacy-selftest` |
| `MaritimeExplorationSystem` | `Main.Maritime.cs` | `maritime` | `--maritime-selftest` |
| `ClinicalWardTriageEngine` | `Main.Medical.cs`, `Main.MedicalTriage.cs` | `medical`, `medical_pipeline`, `medical_ward` | `--medical-selftest`, `--medical-ward-save-selftest` |
| `DependencyTaperWithdrawalEngine` | `Main.Medical.cs`, `Main.MedicalTriage.cs` | `medical`, `medical_pipeline`, `medical_ward` | `--medical-selftest`, `--medical-ward-save-selftest` |
| `PalliativeCareDignityEngine` | `Main.Medical.cs`, `Main.MedicalTriage.cs` | `medical`, `medical_pipeline`, `medical_ward` | `--medical-selftest`, `--medical-ward-save-selftest` |
| `ProstheticConditionWearEngine` | `Main.Medical.cs`, `Main.MedicalTriage.cs` | `medical`, `medical_pipeline`, `medical_ward` | `--medical-selftest`, `--medical-ward-save-selftest` |
| `RehabilitationProgressionEngine` | `Main.Medical.cs`, `Main.MedicalTriage.cs` | `medical`, `medical_pipeline`, `medical_ward` | `--medical-selftest`, `--medical-ward-save-selftest` |
| `SurgicalGraftRejectionEngine` | `Main.Medical.cs`, `Main.MedicalTriage.cs` | `medical`, `medical_pipeline`, `medical_ward` | `--medical-selftest`, `--medical-ward-save-selftest` |
| `ModSupportSystem` | — | — | `--mods-selftest` |
| `LetterDeliverySystem` | `Main.Narrative.cs`, `Main.NarrativeQuestlines.cs`, `Main.SleepNarrative.cs` | `narrative`, `echoes`, `procedural_narrative` | `--narrative-continuity-selftest`, `--narrative-selftest` |
| `NpcMemorySystem` | `Main.Narrative.cs`, `Main.NarrativeQuestlines.cs`, `Main.SleepNarrative.cs` | `narrative`, `echoes`, `procedural_narrative` | `--narrative-continuity-selftest`, `--narrative-selftest` |
| `SurvivorLetterDeliverySystem` | `Main.Narrative.cs`, `Main.NarrativeQuestlines.cs`, `Main.SleepNarrative.cs` | `survivors`, `narrative`, `echoes` | `--narrative-continuity-selftest`, `--narrative-selftest` |
| `SleepAcousticRestEngine` | — | — | — |
| `CommonTableRationingEngine` | — | `kitchen_nutrition`, `grain_processing` | — |
| `PrecisionGlassworksOpticsEngine` | — | `precision_optics`, `precision_metrology` | — |
| `ConfessionSecretSystem` | — | — | — |
| `PublicBroadsheetPressEngine` | — | — | — |
| `PsychologicalProfileSystem` | — | `mental_health_crisis`, `psychological_sanatorium`, `psychological_arcs` | `--psychology-selftest` |
| `RadioPropagationEngine` | `Main.RadioProgramProduction.cs` | `radio`, `psyops`, `radio_program_production` | `--radio-catalog-selftest`, `--radio-selftest` |
| `SessionDurabilityManager` | `Main.SaveOrchestrator.cs` | — | `--chemical-dependency-save-selftest`, `--duty-roster-save-selftest`, `--expansion-hub-save-selftest` |
| `OutpostSettlementSystem` | — | — | — |
| `ChemicalReagentSynthesisEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ChemicalSynthesis.cs`, `Main.ExpandedShelterSystems.cs` | `sanitation`, `chemical_dependency`, `excavation` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `CupolaFoundryEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `DisasterResponseSystem` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `EmergencyMusterReadinessEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `KilnFiringEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `MechanicalPowerDrivelineEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `PowerLoadSheddingEngine` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `ShelterExpansionSystem` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `ShelterIdentitySystem` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `ShelterMaintenanceSystem` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `TrophySystem` | `Main.AdvancedShelterSystems.cs`, `Main.ExpandedShelterSystems.cs`, `Main.ShelterAtmosphere.cs` | `sanitation`, `excavation`, `shelter_thermal` | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest` |
| `SpiritualRitualCalendarEngine` | `Main.Spiritual.cs` | `spiritual_meaning` | — |
| `AgingSystem` | `Main.SurvivorSocial.cs`, `Main.Survivors.cs`, `Main.UiTests.Survivors.cs` | `survivors`, `hidden_agenda`, `death_legacy` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `AntenatalMaternalHealthEngine` | `Main.SurvivorSocial.cs`, `Main.Survivors.cs`, `Main.UiTests.Survivors.cs` | `survivors`, `hidden_agenda`, `death_legacy` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `BackstorySystem` | `Main.SurvivorSocial.cs`, `Main.Survivors.cs`, `Main.UiTests.Survivors.cs` | `survivors`, `hidden_agenda`, `death_legacy` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `HobbySystem` | `Main.SurvivorSocial.cs`, `Main.Survivors.cs`, `Main.UiTests.Survivors.cs` | `survivors`, `hidden_agenda`, `death_legacy` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `RecruitmentSystem` | `Main.SurvivorSocial.cs`, `Main.Survivors.cs`, `Main.UiTests.Survivors.cs` | `survivors`, `hidden_agenda`, `death_legacy` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `SurvivorAgingProgressionEngine` | `Main.SurvivorDeathLegacy.cs`, `Main.SurvivorFate.cs`, `Main.SurvivorFitness.cs` | `survivors`, `survivor_relations`, `hidden_agenda` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `SurvivorAutonomySystem` | `Main.SurvivorDeathLegacy.cs`, `Main.SurvivorFate.cs`, `Main.SurvivorFitness.cs` | `survivors`, `survivor_relations`, `hidden_agenda` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `SurvivorRoleSystem` | `Main.SurvivorDeathLegacy.cs`, `Main.SurvivorFate.cs`, `Main.SurvivorFitness.cs` | `survivors`, `survivor_relations`, `hidden_agenda` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `SurvivorRoutineSystem` | `Main.SurvivorDeathLegacy.cs`, `Main.SurvivorFate.cs`, `Main.SurvivorFitness.cs` | `survivors`, `survivor_relations`, `hidden_agenda` | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `PlayableMetricsAggregationEngine` | — | `recon_telemetry` | `--recon-telemetry-selftest`, `--recon-telemetry-uitest` |
| `GarmentLayeringThermalEngine` | — | — | — |
| `VisitorIntegrationSystem` | — | — | — |
| `SurvivorVoiceSystem` | `Main.SurvivorDeathLegacy.cs`, `Main.SurvivorFate.cs`, `Main.SurvivorFitness.cs` | `survivors`, `survivor_relations`, `survivor_social` | — |
| `VoiceLineDispatchCoordinator` | — | — | — |
| `VoiceLineSelectionEngine` | — | — | — |
| `WaterQualityProfileEngine` | `Main.WaterCondenser.cs` | `water_condenser`, `water_treatment` | — |
| `WaterSourceSystem` | `Main.WaterCondenser.cs` | `water_condenser`, `water_treatment` | — |
| `NuclearWinterProgressionSystem` | — | `weather_hardening` | `--journal-weather-panel-selftest`, `--weather-save-selftest` |
| `WeatherCascadeSystem` | — | `weather_hardening` | `--journal-weather-panel-selftest`, `--weather-save-selftest` |
| `CascadeTargetSystem` | `Main.Cascade.cs` | `weather_hardening` | `--journal-weather-panel-selftest`, `--weather-save-selftest` |
| `ModalTravelDispatchEngine` | `Main.EvolvingWorld.cs`, `Main.World.cs`, `Main.WorldPlaytest.cs` | `world`, `wasteland_rumors`, `subterranean` | `--dynamic-world-selftest`, `--evolving-world-selftest`, `--world-exploration-selftest` |
| `NightWatchPatrolReadinessEngine` | `Main.EvolvingWorld.cs`, `Main.World.cs`, `Main.WorldPlaytest.cs` | `world`, `wasteland_rumors`, `subterranean` | `--dynamic-world-selftest`, `--evolving-world-selftest`, `--world-exploration-selftest` |
| `StormForecastReadinessEngine` | `Main.EvolvingWorld.cs`, `Main.World.cs`, `Main.WorldPlaytest.cs` | `world`, `wasteland_rumors`, `subterranean` | `--dynamic-world-selftest`, `--evolving-world-selftest`, `--world-exploration-selftest` |
| `WeatherForecastReliabilityEngine` | `Main.EvolvingWorld.cs`, `Main.World.cs`, `Main.WorldPlaytest.cs` | `world`, `wasteland_rumors`, `subterranean` | `--dynamic-world-selftest`, `--evolving-world-selftest`, `--world-exploration-selftest` |
| `WildlifeHarvestQuotaEngine` | `Main.EvolvingWorld.cs`, `Main.World.cs`, `Main.WorldPlaytest.cs` | `world`, `wildlife_trapping`, `wasteland_rumors` | `--dynamic-world-selftest`, `--evolving-world-selftest`, `--world-exploration-selftest` |

**Player-surface route ids present:** 203 string ids in `src/Main.PlayerSurfaces.cs`; the route column was omitted per-system because route names do not track authority names — each package picks the route at bind time (Appendix C, panel route pattern).
