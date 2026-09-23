# PLAN-ORPHAN-SEAL-01 — Appendix M: Catalog Binding Verification

**Generated:** 2026-09-21. Appendix A listed candidate catalogs by name match.
This appendix **verifies** the match: the file exists, its record count (where
JSON-shaped as a list or with a records/entries array), and whether any loader
in Core references that file name. `exists` without a record count means the
file is not a simple record array (an object/scene shape) and the package must
read its actual shape.
**Use:** a seal package with no verified catalog is data-blocked; a package
with a verified catalog and a loader reference has its data path proven.

### `AccessibilitySettingsSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `accessibility_profiles.json` | yes | object | 0 file(s) |
| `audio_accessibility_cues.json` | yes | object | 0 file(s) |

### `AudioAccessibilityCoordinator`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `accessibility_profiles.json` | yes | object | 0 file(s) |
| `audio_accessibility_cues.json` | yes | object | 0 file(s) |
| `audio_cues.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `audio_logs_expansion_05.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `shelter_audio_cues.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |

### `CassettePlaybackSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `cassette_sets.json` | yes | 12 | 3 file(s): `ContentUtilizationScanner.cs` |

### `BestiarySystem`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ChemicalPlumeDispersionEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `CommitmentSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `commitments.json` | yes | object | 2 file(s): `CatalogIntegrityValidator.cs` |

### `InternalCommunicationSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `communication_templates.json` | yes | object | 0 file(s) |
| `communications_networks.json` | yes | object | 0 file(s) |
| `nvis_communications_catalog.json` | yes | object | 1 file(s): `NvisCommunicationsSystem.cs` |

### `CommunicationsSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `communications_networks.json` | yes | object | 0 file(s) |
| `nvis_communications_catalog.json` | yes | object | 1 file(s): `NvisCommunicationsSystem.cs` |

### `ContentOrphanCertificationEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `CookingSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `recipes_cooking.json` | yes | object | 1 file(s): `CookingSystem.cs` |

### `CultureCreationSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `agriculture_items.json` | yes | 2 | 2 file(s): `ItemCatalogLoader.cs` |
| `muster_faction_culture.json` | yes | 25 | 1 file(s): `FactionCultureCatalog.cs` |
| `recreation.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |

### `ShelterFestivalEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `shelter_audio_cues.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `shelter_celebrations.json` | yes | object | 0 file(s) |
| `shelter_components.json` | yes | object | 0 file(s) |
| `shelter_construction.json` | yes | object | 0 file(s) |
| `shelter_governance_blocs.json` | yes | object | 0 file(s) |

### `ShelterMuseumSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `museum_collection_templates.json` | yes | object | 0 file(s) |
| `shelter_audio_cues.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `shelter_celebrations.json` | yes | object | 0 file(s) |
| `shelter_components.json` | yes | object | 0 file(s) |
| `shelter_construction.json` | yes | object | 0 file(s) |

### `PerimeterEarlyWarningEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `DifficultySettingsSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `difficulty_presets.json` | yes | object | 1 file(s): `DifficultyPresetCatalog.cs` |

### `FactionDiplomacySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `crossing_factions.json` | yes | object | 3 file(s): `CrossingCatalog.cs` |
| `faction_combat_thresholds.json` | yes | object | 0 file(s) |
| `faction_intelligence.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `faction_lore.json` | yes | 47 | 10 file(s): `CrossingHeadlessDemo.cs` |
| `faction_radio_corpus.json` | yes | object | 2 file(s): `RadioBroadcastCatalog.cs` |

### `BlackMarketContrabandEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `black_market_inventory.json` | yes | 7 | 1 file(s): `BlackMarketInventoryCatalog.cs` |

### `BlackMarketHeatAttentionEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ChitPurityAssayEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `LoanSharkEnforcerEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `MigrationConsequenceEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `discovery_consequences.json` | yes | object | 0 file(s) |
| `foundry_treaty_consequences.json` | yes | object | 5 file(s): `SilentFoundryConsequencePolicy.cs` |

### `RestockAllocationEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SeasonalHumanMigrationEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SurvivorBarterSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `antigravity_survivor_fields.json` | yes | 11 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `barter_rules.json` | yes | object | 0 file(s) |
| `deep_lore_survivor_fields.json` | yes | 4 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `expansion_survivor_fields.json` | yes | 73 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `starting_survivor_cohorts.json` | yes | object | 2 file(s): `StartingCohortCatalog.cs` |

### `TradeRouteMonopolyEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `caravan_trade_routes.json` | yes | object | 3 file(s): `InfrastructureHeadlessDemo.cs` |

### `TradeRouteRiskBindingEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ApprenticeshipCurriculumEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `apprenticeship_catalog.json` | yes | object | 2 file(s): `ApprenticeshipSystem.cs` |
| `education_curriculum.json` | yes | object | 0 file(s) |

### `SurvivorEducationSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `antigravity_survivor_fields.json` | yes | 11 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `deep_lore_survivor_fields.json` | yes | 4 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `education_curriculum.json` | yes | object | 0 file(s) |
| `expansion_survivor_fields.json` | yes | 73 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `starting_survivor_cohorts.json` | yes | object | 2 file(s): `StartingCohortCatalog.cs` |

### `EmergencyAlertSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `emergency_alerts.json` | yes | object | 0 file(s) |

### `InformantNetworkTradecraftEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SeasonalCelebrationSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `seasonal_events.json` | yes | object | 1 file(s): `SeasonalEventCatalog.cs` |
| `shelter_celebrations.json` | yes | object | 0 file(s) |

### `SubterraneanSubsidenceEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `subterranean_zones.json` | yes | object | 2 file(s): `ContentUtilizationScanner.cs` |

### `AerialReconWindowEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ColonySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `colony_blueprints.json` | yes | object | 1 file(s): `ModDataContract.cs` |

### `TerritoryControlSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `faction_territory.json` | yes | object | 2 file(s): `FactionTerritoryCatalog.cs` |

### `OilseedPressingEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SoilReclamationProfileEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SecondGenerationMilestoneEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ShelterGovernanceEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `shelter_audio_cues.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `shelter_celebrations.json` | yes | object | 0 file(s) |
| `shelter_components.json` | yes | object | 0 file(s) |
| `shelter_construction.json` | yes | object | 0 file(s) |
| `shelter_governance_blocs.json` | yes | object | 0 file(s) |

### `ClothingWarmthSystem`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `FoodTypeSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `food_preservation.json` | yes | object | 2 file(s): `FoodPreservationCatalog.cs` |
| `food_types.json` | yes | object | 0 file(s) |

### `CampaignLegacySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `campaign_epilogues.json` | yes | object | 2 file(s): `ContentUtilizationScanner.cs` |
| `death_legacy_templates.json` | yes | object | 0 file(s) |
| `legacy_traits.json` | yes | object | 1 file(s): `CampaignLegacySystem.cs` |
| `propaganda_campaigns.json` | yes | object | 3 file(s): `PsyOpsCatalog.cs` |

### `MaritimeExplorationSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `gpr_exploration_catalog.json` | yes | object | 2 file(s): `GroundPenetratingRadarCatalog.cs` |
| `maritime_zones.json` | yes | object | 0 file(s) |

### `ClinicalWardTriageEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `DependencyTaperWithdrawalEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `PalliativeCareDignityEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ProstheticConditionWearEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `RehabilitationProgressionEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `narrative_progression.json` | yes | 15 | 2 file(s): `NarrativeProgressionCatalogLoader.cs` |

### `SurgicalGraftRejectionEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ModSupportSystem`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `LetterDeliverySystem`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `NpcMemorySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `memory_decay_rates.json` | yes | object | 0 file(s) |
| `npc_memory_dialogue.json` | yes | object | 1 file(s): `NpcMemorySystem.cs` |
| `standing_record_memory.json` | yes | 52 | 2 file(s): `LocationMemorySystem.cs` |

### `SurvivorLetterDeliverySystem`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SleepAcousticRestEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `CommonTableRationingEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `PrecisionGlassworksOpticsEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `precision_optics_catalog.json` | yes | object | 1 file(s): `PrecisionOpticsEngine.cs` |

### `ConfessionSecretSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `confession_secrets.json` | yes | 38 | 1 file(s): `ContentUtilizationScanner.cs` |

### `PublicBroadsheetPressEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `PsychologicalProfileSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `accessibility_profiles.json` | yes | object | 0 file(s) |
| `atmosphere_profiles.json` | yes | object | 0 file(s) |
| `infiltrator_profiles.json` | yes | object | 2 file(s): `ContentUtilizationScanner.cs` |
| `nuclear_core_profiles.json` | yes | object | 1 file(s): `NuclearCoreCatalog.cs` |
| `nutrition_profiles.json` | yes | object | 2 file(s): `ContentUtilizationScanner.cs` |

### `RadioPropagationEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `faction_radio_corpus.json` | yes | object | 2 file(s): `RadioBroadcastCatalog.cs` |
| `faction_war_radio.json` | yes | object | 4 file(s): `RadioBroadcastCatalog.cs` |
| `radio.json` | yes | object | 9 file(s): `RadioBroadcastCatalog.cs` |
| `radio_distress_signals.json` | yes | object | 4 file(s): `CatalogIntegrityValidator.cs` |
| `radio_distress_signals_expansion.json` | yes | object | 2 file(s): `CatalogIntegrityValidator.cs` |

### `SessionDurabilityManager`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `OutpostSettlementSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `outposts.json` | yes | object | 1 file(s): `OutpostSettlementSystem.cs` |
| `settlements.json` | yes | object | 1 file(s): `SettlementCatalog.cs` |
| `wasteland_settlement_npcs.json` | yes | object | 1 file(s): `SettlementCatalog.cs` |

### `ChemicalReagentSynthesisEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `CupolaFoundryEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `cupola_foundry_catalog.json` | yes | object | 1 file(s): `CupolaFoundryCatalog.cs` |
| `foundry_accords.json` | yes | object | 6 file(s): `ExpansionMasterSession.cs` |
| `foundry_faction.json` | yes | object | 4 file(s): `FactionIconCatalog.cs` |
| `foundry_items.json` | yes | 30 | 3 file(s): `ItemCatalogLoader.cs` |
| `foundry_production.json` | yes | object | 4 file(s): `SilentFoundryCatalog.cs` |

### `DisasterResponseSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `disaster_templates.json` | yes | object | 0 file(s) |

### `EmergencyMusterReadinessEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `KilnFiringEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `MechanicalPowerDrivelineEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `PowerLoadSheddingEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `ShelterExpansionSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `audio_logs_expansion_05.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `environmental_atmosphere_expansion.json` | yes | object | 2 file(s): `AtmosphereCatalogLoader.cs` |
| `environmental_texts_expansion_05.json` | yes | object | 3 file(s): `CatalogIntegrityRules.cs` |
| `expansion_item_tags.json` | yes | object | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `expansion_survivor_fields.json` | yes | 73 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |

### `ShelterIdentitySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `shelter_audio_cues.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `shelter_celebrations.json` | yes | object | 0 file(s) |
| `shelter_components.json` | yes | object | 0 file(s) |
| `shelter_construction.json` | yes | object | 0 file(s) |
| `shelter_governance_blocs.json` | yes | object | 0 file(s) |

### `ShelterMaintenanceSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `shelter_audio_cues.json` | yes | object | 1 file(s): `ContentUtilizationScanner.cs` |
| `shelter_celebrations.json` | yes | object | 0 file(s) |
| `shelter_components.json` | yes | object | 0 file(s) |
| `shelter_construction.json` | yes | object | 0 file(s) |
| `shelter_governance_blocs.json` | yes | object | 0 file(s) |

### `TrophySystem`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SpiritualRitualCalendarEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `spiritual_rituals.json` | yes | object | 2 file(s): `ContentUtilizationScanner.cs` |

### `AgingSystem`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `AntenatalMaternalHealthEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `BackstorySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `backstory_templates.json` | yes | object | 0 file(s) |

### `HobbySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `hobby_definitions.json` | yes | object | 1 file(s): `ModDataContract.cs` |

### `RecruitmentSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `recruitment_templates.json` | yes | object | 0 file(s) |

### `SurvivorAgingProgressionEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `SurvivorAutonomySystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `antigravity_survivor_fields.json` | yes | 11 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `autonomy_actions.json` | yes | object | 0 file(s) |
| `deep_lore_survivor_fields.json` | yes | 4 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `expansion_survivor_fields.json` | yes | 73 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `starting_survivor_cohorts.json` | yes | object | 2 file(s): `StartingCohortCatalog.cs` |

### `SurvivorRoleSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `antigravity_survivor_fields.json` | yes | 11 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `deep_lore_survivor_fields.json` | yes | 4 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `duty_roles.json` | yes | object | 2 file(s): `CatalogIntegrityValidator.cs` |
| `expansion_survivor_fields.json` | yes | 73 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `starting_survivor_cohorts.json` | yes | object | 2 file(s): `StartingCohortCatalog.cs` |

### `SurvivorRoutineSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `antigravity_survivor_fields.json` | yes | 11 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `deep_lore_survivor_fields.json` | yes | 4 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `exercise_routines.json` | yes | object | 0 file(s) |
| `expansion_survivor_fields.json` | yes | 73 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `routine_templates.json` | yes | object | 0 file(s) |

### `PlayableMetricsAggregationEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `GarmentLayeringThermalEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `VisitorIntegrationSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `visitor_templates.json` | yes | object | 0 file(s) |

### `SurvivorVoiceSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `antigravity_survivor_fields.json` | yes | 11 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `deep_lore_survivor_fields.json` | yes | 4 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `expansion_survivor_fields.json` | yes | 73 | 2 file(s): `ExpansionEnrichmentCatalog.cs` |
| `journal_voice_prose.json` | yes | object | 3 file(s): `JournalVoiceProseCatalog.cs` |
| `starting_survivor_cohorts.json` | yes | object | 2 file(s): `StartingCohortCatalog.cs` |

### `VoiceLineDispatchCoordinator`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `survivor_voice_lines.json` | yes | object | 0 file(s) |

### `VoiceLineSelectionEngine`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `survivor_voice_lines.json` | yes | object | 0 file(s) |

### `WaterQualityProfileEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `WaterSourceSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `guilt_sources.json` | yes | 40 | 2 file(s): `GuiltSourceCatalog.cs` |
| `noise_sources.json` | yes | object | 0 file(s) |
| `water_sources.json` | yes | object | 0 file(s) |

### `NuclearWinterProgressionSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `nuclear_winter_phases.json` | yes | object | 1 file(s): `ModDataContract.cs` |

### `WeatherCascadeSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `cascade_rules.json` | yes | object | 1 file(s): `CascadeRuleCatalog.cs` |
| `weather_effects.json` | yes | object | 3 file(s): `CatalogIntegrityValidator.cs` |
| `weather_gameplay_effects.json` | yes | object | 1 file(s): `WeatherGameplayCascadeEngine.cs` |
| `weather_hardening_upgrades.json` | yes | object | 2 file(s): `WeatherHardeningCatalogLoader.cs` |
| `weather_route_gates.json` | yes | object | 6 file(s): `CatalogIntegrityValidator.cs` |

### `CascadeTargetSystem`

| Catalog | Exists | Records | Loader reference |
|---|:---:|---:|---|
| `cascade_rules.json` | yes | object | 1 file(s): `CascadeRuleCatalog.cs` |
| `comms_targets.json` | yes | object | 2 file(s): `CommsArraySystem.cs` |

### `ModalTravelDispatchEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `NightWatchPatrolReadinessEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `StormForecastReadinessEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `WeatherForecastReliabilityEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

### `WildlifeHarvestQuotaEngine`

No catalog matched by name. This does not mean none is needed: the package checks the loader path before accepting a data-free seal.

