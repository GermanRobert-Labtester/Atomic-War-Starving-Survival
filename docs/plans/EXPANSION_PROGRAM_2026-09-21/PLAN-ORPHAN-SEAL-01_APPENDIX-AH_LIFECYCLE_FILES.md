# PLAN-ORPHAN-SEAL-01 — Appendix AH: Lifecycle & File-Name Assignment

**Generated:** 2026-09-21. For each stateful orphan (Appendix D/AC), the closest
existing **lifecycle group** (from the registry's 202 metadata rows:
campaign, caravans, combat, communication, crafting, dose_ledger, duty_roster, economy, encounters, endgame, equipment, events, expansion_hub, expansion_quest, expedition, expeditions, factions, farming, foundry, greenhouse, holdfast, hunting, industry, infrastructure, inventory, journal, knowledge, maintenance, maritime, medical, memorial, morale, muster, narrative, nutrition, onboarding, personnel, phase0, power_grid, psychology, quests, radiation, radio, schedule, shelter, social, spiritual, starting_level, survival, survivors, thermal, thirdonary, verdict, world, year_of_ash) and a proposed section file name following the
`*_save.json` convention.
**Use:** a package does not invent a group or a filename; it adopts the row
below or records why it deviates. A proposed filename already in the registry is
a collision to resolve before the save work starts.

**0 proposed filenames collide with existing registry files.**

| Authority | Proposed key | Suggested lifecycle group | Proposed file | File collision |
|---|---|---|---|---|


| `AccessibilitySettingsSystem` | `accessibility_settings` | `journal` | `accessibility_settings_save.json` | — |
| `AudioAccessibilityCoordinator` | `audio_accessibility` | `journal` | `audio_accessibility_save.json` | — |
| `CassettePlaybackSystem` | `cassette_playback` | `journal` | `cassette_playback_save.json` | — |
| `BestiarySystem` | `bestiary` | `journal` | `bestiary_save.json` | — |
| `CommitmentSystem` | `commitment` | `journal` | `commitment_save.json` | — |
| `InternalCommunicationSystem` | `internal_communication` | `communication` | `internal_communication_save.json` | — |
| `CommunicationsSystem` | `communications` | `journal` | `communications_save.json` | — |
| `CookingSystem` | `cooking` | `journal` | `cooking_save.json` | — |
| `CultureCreationSystem` | `culture_creation` | `journal` | `culture_creation_save.json` | — |
| `ShelterFestivalEngine` | `shelter_festival` | `shelter` | `shelter_festival_save.json` | — |
| `ShelterMuseumSystem` | `shelter_museum` | `shelter` | `shelter_museum_save.json` | — |
| `PerimeterEarlyWarningEngine` | `perimeter_early_warning` | `journal` | `perimeter_early_warning_save.json` | — |
| `DifficultySettingsSystem` | `difficulty_settings` | `journal` | `difficulty_settings_save.json` | — |
| `FactionDiplomacySystem` | `faction_diplomacy` | `factions` | `faction_diplomacy_save.json` | — |
| `BlackMarketHeatAttentionEngine` | `black_market_heat_attention` | `economy` | `black_market_heat_attention_save.json` | — |
| `LoanSharkEnforcerEngine` | `loan_shark_enforcer` | `economy` | `loan_shark_enforcer_save.json` | — |
| `MigrationConsequenceEngine` | `migration_consequence` | `economy` | `migration_consequence_save.json` | — |
| `SeasonalHumanMigrationEngine` | `seasonal_human_migration` | `economy` | `seasonal_human_migration_save.json` | — |
| `SurvivorBarterSystem` | `survivor_barter` | `economy` | `survivor_barter_save.json` | — |
| `TradeRouteMonopolyEngine` | `trade_route_monopoly` | `economy` | `trade_route_monopoly_save.json` | — |
| `SurvivorEducationSystem` | `survivor_education` | `survivors` | `survivor_education_save.json` | — |
| `EmergencyAlertSystem` | `emergency_alert` | `journal` | `emergency_alert_save.json` | — |
| `SeasonalCelebrationSystem` | `seasonal_celebration` | `events` | `seasonal_celebration_save.json` | — |
| `ColonySystem` | `colony` | `expeditions` | `colony_save.json` | — |
| `TerritoryControlSystem` | `territory_control` | `factions` | `territory_control_save.json` | — |
| `ShelterGovernanceEngine` | `shelter_governance` | `shelter` | `shelter_governance_save.json` | — |
| `ClothingWarmthSystem` | `clothing_warmth` | `inventory` | `clothing_warmth_save.json` | — |
| `FoodTypeSystem` | `food_type` | `journal` | `food_type_save.json` | — |
| `CampaignLegacySystem` | `campaign_legacy` | `campaign` | `campaign_legacy_save.json` | — |
| `MaritimeExplorationSystem` | `maritime_exploration` | `maritime` | `maritime_exploration_save.json` | — |
| `SurgicalGraftRejectionEngine` | `surgical_graft_rejection` | `medical` | `surgical_graft_rejection_save.json` | — |
| `ModSupportSystem` | `mod_support` | `journal` | `mod_support_save.json` | — |
| `LetterDeliverySystem` | `letter_delivery` | `narrative` | `letter_delivery_save.json` | — |
| `NpcMemorySystem` | `npc_memory` | `narrative` | `npc_memory_save.json` | — |
| `SurvivorLetterDeliverySystem` | `survivor_letter_delivery` | `narrative` | `survivor_letter_delivery_save.json` | — |
| `ConfessionSecretSystem` | `confession_secret` | `journal` | `confession_secret_save.json` | — |
| `PublicBroadsheetPressEngine` | `public_broadsheet_press` | `journal` | `public_broadsheet_press_save.json` | — |
| `PsychologicalProfileSystem` | `psychological_profile` | `psychology` | `psychological_profile_save.json` | — |
| `SessionDurabilityManager` | `session_durability` | `journal` | `session_durability_save.json` | — |
| `CupolaFoundryEngine` | `cupola_foundry` | `shelter` | `cupola_foundry_save.json` | — |
| `DisasterResponseSystem` | `disaster_response` | `shelter` | `disaster_response_save.json` | — |
| `PowerLoadSheddingEngine` | `power_load_shedding` | `shelter` | `power_load_shedding_save.json` | — |
| `ShelterExpansionSystem` | `shelter_expansion` | `shelter` | `shelter_expansion_save.json` | — |
| `ShelterIdentitySystem` | `shelter_identity` | `shelter` | `shelter_identity_save.json` | — |
| `ShelterMaintenanceSystem` | `shelter_maintenance` | `shelter` | `shelter_maintenance_save.json` | — |
| `TrophySystem` | `trophy` | `shelter` | `trophy_save.json` | — |
| `AgingSystem` | `aging` | `survivors` | `aging_save.json` | — |
| `BackstorySystem` | `backstory` | `survivors` | `backstory_save.json` | — |
| `HobbySystem` | `hobby` | `survivors` | `hobby_save.json` | — |
| `RecruitmentSystem` | `recruitment` | `survivors` | `recruitment_save.json` | — |
| `SurvivorAutonomySystem` | `survivor_autonomy` | `survivors` | `survivor_autonomy_save.json` | — |
| `SurvivorRoleSystem` | `survivor_role` | `survivors` | `survivor_role_save.json` | — |
| `SurvivorRoutineSystem` | `survivor_routine` | `survivors` | `survivor_routine_save.json` | — |
| `VisitorIntegrationSystem` | `visitor_integration` | `journal` | `visitor_integration_save.json` | — |
| `SurvivorVoiceSystem` | `survivor_voice` | `survivors` | `survivor_voice_save.json` | — |
| `VoiceLineDispatchCoordinator` | `voice_line_dispatch` | `journal` | `voice_line_dispatch_save.json` | — |
| `WaterSourceSystem` | `water_source` | `journal` | `water_source_save.json` | — |
| `NuclearWinterProgressionSystem` | `nuclear_winter_progression` | `journal` | `nuclear_winter_progression_save.json` | — |
| `WeatherCascadeSystem` | `weather_cascade` | `journal` | `weather_cascade_save.json` | — |
| `CascadeTargetSystem` | `cascade_target` | `journal` | `cascade_target_save.json` | — |
