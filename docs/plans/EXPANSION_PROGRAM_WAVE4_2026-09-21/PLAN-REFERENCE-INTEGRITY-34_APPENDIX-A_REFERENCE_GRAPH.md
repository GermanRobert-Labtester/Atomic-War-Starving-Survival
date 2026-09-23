# PLAN-REFERENCE-INTEGRITY-34 — Appendix A: Cross-Catalog Reference Graph

**Generated:** 2026-09-21 from `Assets/StreamingAssets/Data/**`
(703 JSON files). Family = the first segment of snake_case id
tokens; the **owner** is the catalog containing the most distinct ids of that
family; a **reference** is another catalog that contains that exact id token.
Families with < 5 owned ids or < 2 referencing files are omitted.
**Use:** RF-34A/34B — the reference graph. Every edge family should have a
validator rule (dangling = FAIL); `UNVALIDATED` edges are the ones this table
newly exposes.

## Reference families (sorted by total references)

| Family | Owner catalog | Distinct ids | Referencing catalogs | Total refs |
|---|---|---:|---|---:|
| `item_` | `items.json` | 332 | 112 files, 702 refs | 702 |
| `loc_` | `locations.json` | 117 | 59 files, 607 refs | 607 |
| `faction_` | `faction_lore.json` | 43 | 60 files, 361 refs | 361 |
| `quest_` | `questline_master.json` | 300 | 14 files, 203 refs | 203 |
| `scrap_` | `items.json` | 6 | 84 files, 139 refs | 139 |
| `knowledge_` | `research_knowledge.json` | 62 | 10 files, 121 refs | 121 |
| `npc_` | `characters.json` | 84 | 9 files, 110 refs | 110 |
| `location_` | `asset_registry.json` | 37 | 26 files, 88 refs | 88 |
| `narrative_` | `events.json` | 68 | 2 files, 67 refs | 67 |
| `table_` | `scavenging_tables.json` | 51 | 4 files, 67 refs | 67 |
| `cassette_` | `cassette_sets.json` | 38 | 2 files, 62 refs | 62 |
| `species_` | `wildlife_ecosystem.json` | 14 | 7 files, 38 refs | 38 |
| `weapon_` | `combat_catalog.json` | 20 | 9 files, 36 refs | 36 |
| `skill_` | `skills.json` | 145 | 11 files, 34 refs | 34 |
| `the_` | `narrative/survivor_profiles_expansion.json` | 133 | 15 files, 33 refs | 33 |
| `medical_` | `medical_texts.json` | 85 | 27 files, 27 refs | 27 |
| `trap_` | `wildlife_trapping_catalog.json` | 14 | 8 files, 26 refs | 26 |
| `council_` | `narrative/council_meeting_minutes.json` | 32 | 9 files, 23 refs | 23 |
| `bone_` | `narrative_discovery_manifest.json` | 22 | 3 files, 22 refs | 22 |
| `window_` | `weather_seasons.json` | 10 | 5 files, 21 refs | 21 |
| `military_` | `items.json` | 5 | 12 files, 19 refs | 19 |
| `disease_` | `disease_catalog.json` | 20 | 4 files, 16 refs | 16 |
| `friction_` | `events.json` | 30 | 2 files, 16 refs | 16 |
| `maint_` | `narrative/bunker_maintenance_logs_batch_3.json` | 25 | 5 files, 16 refs | 16 |
| `patrol_` | `narrative/patrol_debriefs.json` | 37 | 7 files, 15 refs | 15 |
| `treaty_` | `foundry_accords.json` | 13 | 5 files, 15 refs | 15 |
| `doc_` | `narrative/documents_batch_3.json` | 29 | 15 files, 15 refs | 15 |
| `dwr_` | `narrative/dweller_medical_casebook.json` | 37 | 4 files, 15 refs | 15 |
| `stage_` | `standing_record_quests.json` | 97 | 6 files, 14 refs | 14 |
| `therapy_` | `narrative/therapist_session_notes.json` | 20 | 3 files, 14 refs | 14 |
| `diplomatic_` | `narrative/council_meeting_minutes.json` | 6 | 7 files, 14 refs | 14 |
| `template_` | `medical_record_templates.json` | 8 | 12 files, 12 refs | 12 |
| `settlement_` | `settlements.json` | 12 | 2 files, 12 refs | 12 |
| `freq_` | `radio_distress_signals.json` | 25 | 3 files, 12 refs | 12 |
| `letter_` | `narrative/letters_expansion.json` | 10 | 3 files, 11 refs | 11 |
| `day_` | `narrative/weather_almanac_expansion.json` | 24 | 4 files, 10 refs | 10 |
| `first_` | `narrative/council_meeting_minutes.json` | 11 | 6 files, 9 refs | 9 |
| `rail_` | `rail_logistics_catalog.json` | 6 | 2 files, 8 refs | 8 |
| `profile_` | `relationship_decay_profiles.json` | 7 | 8 files, 8 refs | 8 |
| `station_` | `radio_stations.json` | 7 | 6 files, 8 refs | 8 |
| `sava_` | `narrative_encounters_npc_arcs.json` | 10 | 2 files, 8 refs | 8 |
| `ilze_` | `narrative_encounters_npc_arcs.json` | 8 | 2 files, 8 refs | 8 |
| `marek_` | `narrative_encounters_npc_arcs.json` | 8 | 2 files, 8 refs | 8 |
| `mara_` | `narrative_encounters_npc_arcs.json` | 8 | 2 files, 7 refs | 7 |
| `anete_` | `narrative_encounters_npc_arcs.json` | 10 | 2 files, 7 refs | 7 |
| `rika_` | `narrative_encounters_npc_arcs.json` | 8 | 2 files, 7 refs | 7 |
| `liva_` | `narrative_encounters_npc_arcs.json` | 8 | 2 files, 7 refs | 7 |
| `survivor_` | `year_of_ash_survivors.json` | 36 | 3 files, 6 refs | 6 |
| `trait_` | `survivors.json` | 134 | 2 files, 6 refs | 6 |
| `armor_` | `item_description_texts.json` | 5 | 2 files, 6 refs | 6 |
| `recipe_` | `pharma_recipes.json` | 28 | 6 files, 6 refs | 6 |
| `history_` | `muster_witnesses.json` | 19 | 2 files, 6 refs | 6 |
| `morale_` | `feedback_messages.json` | 7 | 5 files, 5 refs | 5 |
| `lore_` | `world_history.json` | 47 | 2 files, 5 refs | 5 |
| `zone_` | `shelter_security_zones.json` | 10 | 3 files, 4 refs | 4 |
| `greenhouse_` | `narrative/greenhouse_cultivation_logs.json` | 16 | 4 files, 4 refs | 4 |
| `lina_` | `quests_npc_arcs.json` | 5 | 2 files, 4 refs | 4 |
| `second_` | `narrative/council_meeting_minutes.json` | 6 | 3 files, 4 refs | 4 |
| `radio_` | `radio.json` | 58 | 3 files, 3 refs | 3 |
| `grain_` | `narrative/grain_silo_weevil_audits.json` | 6 | 3 files, 3 refs | 3 |
| `steam_` | `narrative/steam_trap_water_hammer_logs.json` | 5 | 3 files, 3 refs | 3 |
| `flour_` | `narrative/trade_ledgers_expansion.json` | 8 | 3 files, 3 refs | 3 |
| `ammo_` | `asset_registry.json` | 35 | 2 files, 2 refs | 2 |
| `hazard_` | `toxic_chemical_catalog.json` | 17 | 2 files, 2 refs | 2 |
| `machine_` | `shelter_machine_identities.json` | 9 | 2 files, 2 refs | 2 |
| `route_` | `narrative/expedition_route_waypoint_notes_batch_2.json` | 16 | 2 files, 2 refs | 2 |
| `site_` | `dive_sites.json` | 12 | 2 files, 2 refs | 2 |
| `radiation_` | `feedback_messages.json` | 8 | 2 files, 2 refs | 2 |
| `ending_` | `independent_faction_branch.json` | 20 | 2 files, 2 refs | 2 |
| `caravan_` | `caravans.json` | 5 | 2 files, 2 refs | 2 |
| `strata_` | `geothermal_drilling_depths.json` | 6 | 2 files, 2 refs | 2 |
| `node_` | `power_subgrid_nodes.json` | 13 | 2 files, 2 refs | 2 |
| `anomaly_` | `anomalies.json` | 11 | 2 files, 2 refs | 2 |
| `pistol_` | `asset_registry.json` | 7 | 2 files, 2 refs | 2 |

## Top families with leading referrers

- `item_` owned by `items.json` (332 ids): top referrers `scavenging_tables.json` (73), `recipes.json` (69), `collectibles.json` (26), `research_knowledge.json` (26), `narrative/documents_batch_3.json` (25)
- `loc_` owned by `locations.json` (117 ids): top referrers `expeditions.json` (46), `characters.json` (41), `world_evolution_seeds.json` (41), `faction_territory.json` (31), `codex_entries.json` (29)
- `faction_` owned by `faction_lore.json` (43 ids): top referrers `muster_faction_culture.json` (24), `faction_territory.json` (19), `year_of_ash_quests.json` (19), `currents.json` (16), `characters.json` (15)
- `quest_` owned by `questline_master.json` (300 ids): top referrers `survivors.json` (63), `duty_roster_quests.json` (28), `quests_expansion_05.json` (26), `thirdonary_quests.json` (19), `year_of_ash_quests.json` (11)
- `scrap_` owned by `items.json` (6 ids): top referrers `recipes.json` (5), `dive_sites.json` (3), `excavation_hazard_mitigation.json` (3), `item_degradation.json` (3), `naval_vessels.json` (3)
- `knowledge_` owned by `research_knowledge.json` (62 ids): top referrers `library_manuals.json` (24), `recipes.json` (24), `research_unlocks.json` (23), `relic_recipes.json` (15), `prewar_archives.json` (12)
- `npc_` owned by `characters.json` (84 ids): top referrers `narrative_encounters_npc_arcs.json` (24), `npc_arcs.json` (24), `wasteland_settlement_npcs.json` (18), `settlements.json` (18), `repeatable_quests.json` (6)
- `location_` owned by `asset_registry.json` (37 ids): top referrers `locations.json` (36), `narrative_discovery_manifest.json` (6), `narrative/journal_entries_batch_1.json` (5), `world_history.json` (4), `narrative/expedition_field_reports.json` (4)
- `narrative_` owned by `events.json` (68 ids): top referrers `trade_specialties.json` (53), `relic_recipes.json` (14)
- `table_` owned by `scavenging_tables.json` (51 ids): top referrers `expeditions.json` (46), `library_manuals.json` (12), `subterranean_zones.json` (6), `anomalies.json` (3)
- `cassette_` owned by `cassette_sets.json` (38 ids): top referrers `items.json` (38), `scavenging_tables.json` (24)
- `species_` owned by `wildlife_ecosystem.json` (14 ids): top referrers `world_evolution_seeds.json` (13), `wildlife_trapping_catalog.json` (7), `field_guide.json` (6), `companion_animals.json` (6), `trophies.json` (4)
- `weapon_` owned by `combat_catalog.json` (20 ids): top referrers `items.json` (15), `item_description_texts.json` (6), `recipes.json` (4), `trade_texts.json` (4), `settlements.json` (2)
- `skill_` owned by `skills.json` (145 ids): top referrers `shelter_rooms.json` (11), `workshop_recipes.json` (5), `duty_roles.json` (5), `final_wishes.json` (3), `radio_intercepts.json` (2)
- `the_` owned by `narrative/survivor_profiles_expansion.json` (133 ids): top referrers `narrative/letters_expansion.json` (15), `narrative/field_reports_expansion.json` (3), `narrative/radio_transcripts_batch_3.json` (3), `currents.json` (1), `environmental_atmosphere_expansion.json` (1)
- `medical_` owned by `medical_texts.json` (85 ids): top referrers `dive_sites.json` (1), `ledger_debt_templates.json` (1), `repeatable_quests.json` (1), `robotics.json` (1), `surgical_procedures.json` (1)
- `trap_` owned by `wildlife_trapping_catalog.json` (14 ids): top referrers `items.json` (10), `events.json` (3), `recipes.json` (3), `economy_goods.json` (3), `expeditions.json` (2)
- `council_` owned by `narrative/council_meeting_minutes.json` (32 ids): top referrers `narrative/patrol_debriefs.json` (9), `narrative/diplomatic_contact_records_batch_1.json` (6), `narrative/load_shed_schedule_001.json` (2), `narrative/chemist_lab_notes_batch_1.json` (1), `narrative/courier_mission_logs_batch_2.json` (1)
- `bone_` owned by `narrative_discovery_manifest.json` (22 ids): top referrers `narrative/bone_degreasing_prep_logs.json` (8), `narrative/needle_awl_hook_assays.json` (7), `narrative/scraping_polishing_reports.json` (7)
- `window_` owned by `weather_seasons.json` (10 ids): top referrers `seasonal_events.json` (6), `travel_encounters.json` (6), `ecological_infestations.json` (4), `wildlife_trapping_catalog.json` (3), `wildlife_ecosystem.json` (2)
- `military_` owned by `items.json` (5 ids): top referrers `expeditions.json` (3), `scavenging_tables.json` (3), `damaged_map_zones.json` (2), `food_preservation.json` (2), `radio_distress_signals.json` (2)
- `disease_` owned by `disease_catalog.json` (20 ids): top referrers `microfluidic_diagnostic_catalog.json` (6), `pathogens.json` (4), `wildlife_trapping_catalog.json` (4), `ecological_infestations.json` (2)
- `friction_` owned by `events.json` (30 ids): top referrers `bunker_graffiti_postings.json` (11), `spiritual_rituals.json` (5)
- `maint_` owned by `narrative/bunker_maintenance_logs_batch_3.json` (25 ids): top referrers `narrative/council_meeting_minutes.json` (9), `narrative/load_shed_schedule_001.json` (3), `narrative/patrol_debriefs.json` (2), `narrative/diplomatic_contact_records_batch_1.json` (1), `narrative/expedition_field_reports_batch_2.json` (1)
- `patrol_` owned by `narrative/patrol_debriefs.json` (37 ids): top referrers `narrative/council_meeting_minutes.json` (7), `narrative/diplomatic_contact_records_batch_1.json` (3), `narrative/chemist_lab_notes_batch_1.json` (1), `narrative/expedition_field_reports_batch_2.json` (1), `narrative/expedition_planning_briefs_batch_1.json` (1)
- `treaty_` owned by `foundry_accords.json` (13 ids): top referrers `foundry_treaty_consequences.json` (8), `narrative/regional_treaty_protocols.json` (3), `foundry_production.json` (2), `diplomatic_treaties.json` (1), `regional_treaties.json` (1)
- `doc_` owned by `narrative/documents_batch_3.json` (29 ids): top referrers `narrative/bunker_shift_schedules_and_notices.json` (1), `narrative/bureaucratic_documents_expansion.json` (1), `narrative/culinary_ration_batch_2.json` (1), `narrative/documents_batch_1.json` (1), `narrative/documents_batch_2.json` (1)
- `dwr_` owned by `narrative/dweller_medical_casebook.json` (37 ids): top referrers `narrative/wire_confessions.json` (8), `narrative/night_watch_logbook.json` (5), `documents/vel_triage_log_names.json` (1), `narrative/medical_documents_expansion.json` (1)
- `stage_` owned by `standing_record_quests.json` (97 ids): top referrers `holdfast_quests.json` (4), `crossing_quests.json` (4), `duty_roster_quests.json` (3), `quests_faction_branching.json` (1), `quests_moral_branching_expansion.json` (1)
- `therapy_` owned by `narrative/therapist_session_notes.json` (20 ids): top referrers `narrative/conflict_mediation_records.json` (11), `narrative/education_session_records.json` (2), `narrative/patrol_debriefs.json` (1)
- `diplomatic_` owned by `narrative/council_meeting_minutes.json` (6 ids): top referrers `narrative/diplomatic_contact_records_batch_1.json` (5), `narrative/patrol_debriefs.json` (4), `narrative/courier_mission_logs_batch_2.json` (1), `narrative/expedition_field_reports_batch_2.json` (1), `narrative/expedition_planning_briefs_batch_1.json` (1)
- `template_` owned by `medical_record_templates.json` (8 ids): top referrers `bounty_board.json` (1), `npc_memory_dialogue.json` (1), `hidden_agendas.json` (1), `death_legacy_templates.json` (1), `dynamic_quest_templates.json` (1)
- `settlement_` owned by `settlements.json` (12 ids): top referrers `repeatable_quests.json` (6), `wasteland_settlement_npcs.json` (6)
- `freq_` owned by `radio_distress_signals.json` (25 ids): top referrers `questline_master.json` (5), `radio_distress_signals_expansion.json` (5), `faction_radio_corpus.json` (2)
- `letter_` owned by `narrative/letters_expansion.json` (10 ids): top referrers `narrative_discovery_manifest.json` (8), `narrative/unsent_letters_batch_2.json` (2), `narrative/survivor_letters_lost_kin.json` (1)
- `day_` owned by `narrative/weather_almanac_expansion.json` (24 ids): top referrers `narrative/shelter_notices_expansion.json` (4), `narrative/trade_ledgers_expansion.json` (4), `narrative/quest_narrative_documents.json` (1), `narrative/undertaker_burial_records.json` (1)
- `first_` owned by `narrative/council_meeting_minutes.json` (11 ids): top referrers `narrative/patrol_debriefs.json` (3), `narrative/radio_transcripts_batch_3.json` (2), `narrative/diplomatic_contact_records_batch_1.json` (1), `narrative/ration_records_expansion.json` (1), `narrative/therapist_session_notes_batch_2.json` (1)
- `rail_` owned by `rail_logistics_catalog.json` (6 ids): top referrers `rail_network.json` (5), `railway_interlock_catalog.json` (3)
- `profile_` owned by `relationship_decay_profiles.json` (7 ids): top referrers `item_degradation.json` (1), `ballistics_workbench_catalog.json` (1), `electrostatic_filtration_catalog.json` (1), `infiltrator_profiles.json` (1), `fog_harvesting_catalog.json` (1)
- `station_` owned by `radio_stations.json` (7 ids): top referrers `radio_programs.json` (3), `direction_finding_catalog.json` (1), `heliograph.json` (1), `pneumatic_network_catalog.json` (1), `narrative/radio_scripts_expansion.json` (1)
