# PLAN-ORPHAN-SEAL-01 — Appendix R: Bound Catalog Shapes

**Generated:** 2026-09-21. For orphans with catalog matches (Appendix M), the
**shape** of each matched catalog: top-level keys (objects) or element keys
(arrays), so a loader can be written without opening files one by one.
**Use:** a package whose loader must read one of these files starts from this
shape; `array[N]` rows show the record count and the representative element
keys.

### `AccessibilitySettingsSystem`

- `accessibility_profiles.json` — object keys: `schema_version`, `default_profile_id`, `profiles`
- `audio_accessibility_cues.json` — object keys: `schema_version`, `cues`, `mix_presets`

### `AudioAccessibilityCoordinator`

- `accessibility_profiles.json` — object keys: `schema_version`, `default_profile_id`, `profiles`
- `audio_accessibility_cues.json` — object keys: `schema_version`, `cues`, `mix_presets`
- `audio_cues.json` — object keys: `schema_version`, `cues`

### `CassettePlaybackSystem`

- `cassette_sets.json` — object keys: `schema_version`, `items`

### `CommitmentSystem`

- `commitments.json` — object keys: `schema_version`, `commitments`

### `InternalCommunicationSystem`

- `communication_templates.json` — object keys: `schema_version`, `templates`
- `communications_networks.json` — object keys: `schema_version`, `collection_id`, `antenna_types`, `default_networks`, `intercept_templates`
- `nvis_communications_catalog.json` — object keys: `schema_version`, `channels`

### `CommunicationsSystem`

- `communications_networks.json` — object keys: `schema_version`, `collection_id`, `antenna_types`, `default_networks`, `intercept_templates`
- `nvis_communications_catalog.json` — object keys: `schema_version`, `channels`

### `CookingSystem`

- `recipes_cooking.json` — object keys: `schema_version`, `recipes`

### `CultureCreationSystem`

- `agriculture_items.json` — object keys: `schema_version`, `items`
- `muster_faction_culture.json` — object keys: `schema_version`, `entries`
- `recreation.json` — object keys: `schema_version`, `hobbies`

### `ShelterFestivalEngine`

- `shelter_audio_cues.json` — object keys: `schema_version`, `cues`, `mix_profiles`
- `shelter_celebrations.json` — object keys: `schema_version`, `holidays`, `anniversary_types`, `scales`
- `shelter_components.json` — object keys: `schema_version`, `components`

### `ShelterMuseumSystem`

- `museum_collection_templates.json` — object keys: `schema_version`, `artifact_templates`, `exhibition_themes`
- `shelter_audio_cues.json` — object keys: `schema_version`, `cues`, `mix_profiles`
- `shelter_celebrations.json` — object keys: `schema_version`, `holidays`, `anniversary_types`, `scales`

### `DifficultySettingsSystem`

- `difficulty_presets.json` — object keys: `schema_version`, `presets`, `default_preset_id`

### `FactionDiplomacySystem`

- `crossing_factions.json` — object keys: `schema_version`, `actions`
- `faction_combat_thresholds.json` — object keys: `schema_version`, `thresholds`, `faction_rules`
- `faction_intelligence.json` — object keys: `schema_version`, `operations`, `dead_drop_templates`

### `BlackMarketContrabandEngine`

- `black_market_inventory.json` — object keys: `schema_version`, `collection_id`, `description`, `syndicates`, `entries`

### `MigrationConsequenceEngine`

- `discovery_consequences.json` — object keys: `schema_version`, `consequence_types`
- `foundry_treaty_consequences.json` — object keys: `schema_version`, `collection_id`, `policies`

### `SurvivorBarterSystem`

- `antigravity_survivor_fields.json` — object keys: `schema_version`, `items`
- `barter_rules.json` — object keys: `schema_version`, `rules`
- `deep_lore_survivor_fields.json` — object keys: `schema_version`, `items`

### `TradeRouteMonopolyEngine`

- `caravan_trade_routes.json` — object keys: `schema_version`, `routes`

### `ApprenticeshipCurriculumEngine`

- `apprenticeship_catalog.json` — object keys: `schema_version`, `mentorships`
- `education_curriculum.json` — object keys: `schema_version`, `collection_id`, `stages`, `subjects`

### `SurvivorEducationSystem`

- `antigravity_survivor_fields.json` — object keys: `schema_version`, `items`
- `deep_lore_survivor_fields.json` — object keys: `schema_version`, `items`
- `education_curriculum.json` — object keys: `schema_version`, `collection_id`, `stages`, `subjects`

### `EmergencyAlertSystem`

- `emergency_alerts.json` — object keys: `schema_version`, `alert_types`

### `SeasonalCelebrationSystem`

- `seasonal_events.json` — object keys: `schema_version`, `events`
- `shelter_celebrations.json` — object keys: `schema_version`, `holidays`, `anniversary_types`, `scales`

### `SubterraneanSubsidenceEngine`

- `subterranean_zones.json` — object keys: `schema_version`, `collection_id`, `subterranean_zones`

### `ColonySystem`

- `colony_blueprints.json` — object keys: `schema_version`, `colony_types`, `buildings`

### `TerritoryControlSystem`

- `faction_territory.json` — object keys: `schema_version`, `collection_id`, `territories`, `contested_zones`

### `ShelterGovernanceEngine`

- `shelter_audio_cues.json` — object keys: `schema_version`, `cues`, `mix_profiles`
- `shelter_celebrations.json` — object keys: `schema_version`, `holidays`, `anniversary_types`, `scales`
- `shelter_components.json` — object keys: `schema_version`, `components`

### `FoodTypeSystem`

- `food_preservation.json` — object keys: `schema_version`, `preservation_tiers`, `curing_recipes`, `food_type_by_item_id`
- `food_types.json` — object keys: `schema_version`, `food_types`

### `CampaignLegacySystem`

- `campaign_epilogues.json` — object keys: `schema_version`, `vignettes`
- `death_legacy_templates.json` — object keys: `schema_version`, `templates`
- `legacy_traits.json` — object keys: `schema_version`, `legacy_traits`

### `MaritimeExplorationSystem`

- `gpr_exploration_catalog.json` — object keys: `schema_version`, `equipment`, `modes`, `terrains`, `anomalies`
- `maritime_zones.json` — object keys: `schema_version`, `zones`

### `RehabilitationProgressionEngine`

- `narrative_progression.json` — object keys: `schema_version`, `entries`

### `NpcMemorySystem`

- `memory_decay_rates.json` — object keys: `schema_version`, `domain_rates`, `clarity_thresholds`
- `npc_memory_dialogue.json` — object keys: `schema_version`, `dialogue_templates`
- `standing_record_memory.json` — object keys: `schema_version`, `items`

### `PrecisionGlassworksOpticsEngine`

- `precision_optics_catalog.json` — object keys: `schema_version`, `recipes`

### `ConfessionSecretSystem`

- `confession_secrets.json` — object keys: `schema_version`, `items`

### `PsychologicalProfileSystem`

- `accessibility_profiles.json` — object keys: `schema_version`, `default_profile_id`, `profiles`
- `atmosphere_profiles.json` — object keys: `schema_version`, `profiles`
- `infiltrator_profiles.json` — object keys: `schema_version`, `profiles`

### `RadioPropagationEngine`

- `faction_radio_corpus.json` — object keys: `schema_version`, `$schema`, `version`, `description`, `silence_events`, `factions`, `broadcasts`
- `faction_war_radio.json` — object keys: `schema_version`, `broadcasts`
- `radio.json` — object keys: `schema_version`, `radio_broadcasts`

### `OutpostSettlementSystem`

- `outposts.json` — object keys: `schema_version`, `outposts`
- `settlements.json` — object keys: `schema_version`, `collection_id`, `settlements`
- `wasteland_settlement_npcs.json` — object keys: `schema_version`, `collection_id`, `npcs`

### `CupolaFoundryEngine`

- `cupola_foundry_catalog.json` — object keys: `schema_version`, `charges`, `molds`, `maintenance`
- `foundry_accords.json` — object keys: `schema_version`, `collection_id`, `treaties`
- `foundry_faction.json` — object keys: `schema_version`, `faction_id`, `display_name`, `short_name`, `identity`, `icon_path`, `internal_divisions`, `relationships`, `tags`

### `DisasterResponseSystem`

- `disaster_templates.json` — object keys: `schema_version`, `collection_id`, `disaster_templates`, `emergency_protocols`

### `ShelterExpansionSystem`

- `audio_logs_expansion_05.json` — object keys: `schema_version`, `audio_logs`
- `environmental_atmosphere_expansion.json` — object keys: `schema_version`, `collection_id`, `environmental_texts`
- `environmental_texts_expansion_05.json` — object keys: `schema_version`, `environmental_texts`

### `ShelterIdentitySystem`

- `shelter_audio_cues.json` — object keys: `schema_version`, `cues`, `mix_profiles`
- `shelter_celebrations.json` — object keys: `schema_version`, `holidays`, `anniversary_types`, `scales`
- `shelter_components.json` — object keys: `schema_version`, `components`

### `ShelterMaintenanceSystem`

- `shelter_audio_cues.json` — object keys: `schema_version`, `cues`, `mix_profiles`
- `shelter_celebrations.json` — object keys: `schema_version`, `holidays`, `anniversary_types`, `scales`
- `shelter_components.json` — object keys: `schema_version`, `components`

### `SpiritualRitualCalendarEngine`

- `spiritual_rituals.json` — object keys: `schema_version`, `rituals`

### `BackstorySystem`

- `backstory_templates.json` — object keys: `schema_version`, `occupations`, `life_experiences`, `backstory_templates`

### `HobbySystem`

- `hobby_definitions.json` — object keys: `schema_version`, `hobbies`

### `RecruitmentSystem`

- `recruitment_templates.json` — object keys: `schema_version`, `campaign_templates`, `candidate_templates`

### `SurvivorAutonomySystem`

- `antigravity_survivor_fields.json` — object keys: `schema_version`, `items`
- `autonomy_actions.json` — object keys: `schema_version`, `actions`
- `deep_lore_survivor_fields.json` — object keys: `schema_version`, `items`

### `SurvivorRoleSystem`

- `antigravity_survivor_fields.json` — object keys: `schema_version`, `items`
- `deep_lore_survivor_fields.json` — object keys: `schema_version`, `items`
- `duty_roles.json` — object keys: `schema_version`, `collection_id`, `thresholds`, `roles`, `overwork`, `worker_yield`

### `SurvivorRoutineSystem`

- `antigravity_survivor_fields.json` — object keys: `schema_version`, `items`
- `deep_lore_survivor_fields.json` — object keys: `schema_version`, `items`
- `exercise_routines.json` — object keys: `schema_version`, `routines`

### `VisitorIntegrationSystem`

- `visitor_templates.json` — object keys: `schema_version`, `templates`

### `SurvivorVoiceSystem`

- `antigravity_survivor_fields.json` — object keys: `schema_version`, `items`
- `deep_lore_survivor_fields.json` — object keys: `schema_version`, `items`
- `expansion_survivor_fields.json` — object keys: `schema_version`, `items`

### `VoiceLineDispatchCoordinator`

- `survivor_voice_lines.json` — object keys: `schema_version`, `lines`

### `VoiceLineSelectionEngine`

- `survivor_voice_lines.json` — object keys: `schema_version`, `lines`

### `WaterSourceSystem`

- `guilt_sources.json` — object keys: `schema_version`, `items`
- `noise_sources.json` — object keys: `schema_version`, `sources`
- `water_sources.json` — object keys: `schema_version`, `sources`, `connections`

### `NuclearWinterProgressionSystem`

- `nuclear_winter_phases.json` — object keys: `schema_version`, `phases`, `seasons`

### `WeatherCascadeSystem`

- `cascade_rules.json` — object keys: `schema_version`, `minimum_warning_hours`, `rules`
- `weather_effects.json` — object keys: `schema_version`, `weather_effects`
- `weather_gameplay_effects.json` — object keys: `schema_version`, `cascade_templates`

### `CascadeTargetSystem`

- `cascade_rules.json` — object keys: `schema_version`, `minimum_warning_hours`, `rules`
- `comms_targets.json` — object keys: `schema_version`, `targets`

