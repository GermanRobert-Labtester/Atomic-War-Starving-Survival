# Plan 135 — Narrative Schema Family Census

**Total Narrative Catalogs Analyzed:** 279
**Schema Version Uniformity:** 100% (all 279 catalogs carry schema_version: 1)
**Total Authoritative Definitions:** 3592 records

## Macro Cluster Breakdown

| Cluster Name | Catalogs | Total Records | Primary ID Field | Primary Text Field | Key Structural Characteristics |
|---|:---:|:---:|---|---|---|
| **Industrial & Technical Process Logs** | 144 | 1129 | `carbon_filter_vessel_id, id` | `prose` | Array key: `items` |
| **Audits, Casebooks & Inspection Reports** | 21 | 437 | `case_id, evidence_presented` | `charge_summary, clerk_margin_notes` | Array key: `cases` |
| **Domain Catalog (items)** | 17 | 414 | `hidden_stash_location, id` | `prose` | Array key: `items` |
| **Transcripts, Documents & Confessions** | 18 | 363 | `doc_id` | `transcript` | Array key: `documents` |
| **Craft & Workshop Production Logs** | 24 | 180 | `id, saw_tool_id` | `log_text` | Array key: `items` |
| **Domain Catalog (entries)** | 4 | 131 | `author_id, id` | `text` | Array key: `entries` |
| **Domain Catalog (broadcasts)** | 4 | 86 | `broadcast_id` | `transcript` | Array key: `broadcasts` |
| **Domain Catalog (postings)** | 2 | 76 | `posting_id` | `—` | Array key: `postings` |
| **Domain Catalog (recipes)** | 2 | 40 | `recipe_id` | `—` | Array key: `recipes` |
| **Domain Catalog (journal_entries)** | 1 | 40 | `id` | `bodyText` | Array key: `journal_entries` |
| **Domain Catalog (memorials)** | 1 | 40 | `id` | `text` | Array key: `memorials` |
| **Faction Directives & Treaties** | 2 | 36 | `directive_id` | `archaeological_notes, transcript` | Array key: `directives` |
| **Trade Ledgers & Caravan Routes** | 2 | 33 | `route_id` | `radiation_notes` | Array key: `routes` |
| **Domain Catalog (relics)** | 1 | 32 | `relic_id` | `curator_note` | Array key: `relics` |
| **Domain Catalog (briefs)** | 2 | 31 | `brief_id` | `—` | Array key: `briefs` |
| **Domain Catalog (dispatches)** | 1 | 30 | `dispatch_id` | `transcript` | Array key: `dispatches` |
| **Domain Catalog (heirlooms)** | 1 | 30 | `heirloom_id, owner_id` | `item_loss_event_text, sensory_memory_text` | Array key: `heirlooms` |
| **Domain Catalog (records)** | 1 | 30 | `record_id` | `dweller_resonance_notes, needle_audio_texture` | Array key: `records` |
| **Domain Catalog (expeditions)** | 1 | 30 | `expedition_id` | `ambient_prose, threat_description` | Array key: `expeditions` |
| **Domain Catalog (letters)** | 1 | 25 | `letter_id` | `galina_dead_letter_note, letter_text` | Array key: `letters` |
| **Domain Catalog (blueprints)** | 1 | 24 | `room_id` | `chief_engineer_note` | Array key: `blueprints` |
| **Domain Catalog (strata)** | 1 | 24 | `strata_id` | `geologist_field_notes` | Array key: `strata` |
| **Domain Catalog (manuals)** | 1 | 24 | `manual_id` | `dmitri_engineering_notes, schematic_summary` | Array key: `manuals` |
| **Domain Catalog (species)** | 1 | 24 | `species_id` | `botanist_field_notes` | Array key: `species` |
| **Domain Catalog (creatures)** | 1 | 24 | `creature_id` | `harlan_scout_notes` | Array key: `creatures` |
| **Engineering Glitches & Failures** | 1 | 20 | `glitch_id` | `anomaly_description, dmitri_shift_note` | Array key: `glitches` |
| **Domain Catalog (transactions)** | 1 | 20 | `tx_id` | `notes` | Array key: `transactions` |
| **Domain Catalog (radio_broadcasts)** | 1 | 20 | `rundown_id` | `—` | Array key: `radio_broadcasts` |
| **Domain Catalog (settlements)** | 1 | 20 | `settlement_id` | `—` | Array key: `settlements` |
| **Domain Catalog (documents)** | 1 | 18 | `id` | `text` | Array key: `documents` |
| **Domain Catalog (pamphlets)** | 1 | 16 | `faction_id, ideological_alignment` | `—` | Array key: `pamphlets` |
| **Domain Catalog (songs)** | 1 | 16 | `lore_id` | `performance_context` | Array key: `songs` |
| **Domain Catalog (mods)** | 1 | 15 | `mod_id` | `—` | Array key: `mods` |
| **Domain Catalog (guides)** | 1 | 15 | `guide_id` | `—` | Array key: `guides` |
| **Domain Catalog (interviews)** | 1 | 15 | `interview_id` | `therapist_note` | Array key: `interviews` |
| **Domain Catalog (surveys)** | 1 | 15 | `survey_id` | `notes` | Array key: `surveys` |
| **Domain Catalog (transmissions)** | 1 | 12 | `transmission_id` | `transcript` | Array key: `transmissions` |
| **Domain Catalog (templates)** | 2 | 10 | `expansion_id, template_id` | `body_template` | Array key: `templates` |
| **Domain Catalog (anomalies)** | 1 | 8 | `anomaly_id, incident_date` | `bureaucratic_summary` | Array key: `anomalies` |
| **Civic & Court Verdicts** | 1 | 8 | `evidence, verdict_id` | `—` | Array key: `verdicts` |
| **Domain Catalog (backstories)** | 1 | 6 | `backstory_id, survivor_id` | `prose, summary` | Array key: `backstories` |
| **Domain Catalog (priority_order)** | 1 | 6 | `—` | `notes` | Array key: `priority_order` |
| **Domain Catalog (rituals)** | 1 | 5 | `ritual_id` | `sociological_summary` | Array key: `rituals` |
| **Domain Catalog (contacts)** | 1 | 5 | `contact_id` | `external_party_description, meeting_summary` | Array key: `contacts` |
| **Domain Catalog (eulogies)** | 1 | 4 | `archetype_id` | `—` | Array key: `eulogies` |
| **Domain Catalog (transcripts)** | 1 | 3 | `transcript_id` | `analyst_note, transcript` | Array key: `transcripts` |
| **Domain Catalog (missions)** | 1 | 1 | `mission_id` | `summary` | Array key: `missions` |
| **Domain Catalog (sessions)** | 1 | 1 | `session_id` | `summary` | Array key: `sessions` |

## Detailed Cluster Profiles

### Industrial & Technical Process Logs (144 catalogs, 1129 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `childrens_folklore_expansion.json` | 31 | `id` | `prose` | folk_theme, origin_sector, tradition_type |
| `bunker_children_folklore.json` | 19 | `id` | `prose` | folk_theme, origin_sector, tradition_type |
| `wasteland_grave_epitaphs_batch_2.json` | 12 | `deceased_identity`, `id` | `prose` | cause_of_death, grave_site, marker_material |
| `numbers_station_ciphers.json` | 11 | `id` | `prose` | chime_interval_seconds, modulation_mode, station_nickname |
| `bunker_children_folklore_batch_2.json` | 10 | `id` | `prose` | folk_theme, origin_sector, tradition_type |
| `bunker_wiretap_transcripts_batch_2.json` | 10 | `id`, `speaker_identities` | `prose` | audio_clarity_score, intercept_channel, target_faction |
| `ammo_hoist_jam_reports.json` | 8 | `id`, `turret_emplacement_id` | `prose` | caliber_designation, hoist_mechanism_type, jam_classification |
| `ammonia_chiller_leak_logs.json` | 8 | `chiller_unit_id`, `id` | `prose` | leak_rate_ppm_ambient, refrigerant_charge_kg, system_failure_mode |
| `apiculture_red_light_audits.json` | 8 | `chamber_zone_id`, `id` | `prose` | brood_chamber_temperature_celsius, colony_population_count, illumination_wavelength_nm |
| `aramid_fiber_rot_reports.json` | 8 | `aramid_yarn_type`, `armor_item_id`, `id`, `residual_tensile_strength_pct` | `prose` | failure_phenomenon |
| *(and 134 more files)* | ... | ... | ... | ... |

### Audits, Casebooks & Inspection Reports (21 catalogs, 437 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `dweller_medical_casebook.json` | 40 | `case_id`, `patient_id` | `doctor_margin_note` | attending_physician, category, dose_estimate_msv |
| `field_reports_expansion.json` | 39 | `location_id`, `report_id` | `approach_summary` | day_completed, duration_days, expedition_leader |
| `medical_documents_expansion.json` | 36 | `case_id`, `patient_id` | `doctor_margin_note` | attending_physician, category, doc_type |
| `night_watch_expansion.json` | 35 | `log_id`, `sentry_id` | `log_entry`, `sentry_id`, `sentry_name` | post_location, recorded_day, tactical_action |
| `supply_audit_records_batch_2.json` | 34 | `audit_id` | `notes` | auditor, category, day |
| `engineering_logs_expansion.json` | 32 | `log_id` | `entry` | follow_up, parts_used, recorded_day |
| `bunker_maintenance_logs_batch_3.json` | 25 | `log_id` | `—` | action_taken, author, day |
| `bunker_court_verdicts_codex.json` | 24 | `case_id`, `evidence_presented`, `presiding_magistrate` | `charge_summary`, `clerk_margin_notes` | defendant_name, disciplinary_penalty, docket_number |
| `crop_experiment_logs.json` | 20 | `log_id` | `—` | action, author, crop |
| `power_grid_management_logs.json` | 20 | `log_id` | `notes` | author, day, fuel_consumed_litres |
| *(and 11 more files)* | ... | ... | ... | ... |

### Domain Catalog (items) (17 catalogs, 414 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `found_objects_expansion.json` | 40 | `id` | `description` | name, tier |
| `survivor_profiles_expansion.json` | 40 | `id`, `location_id` | `—` | bio, display_name, faction |
| `world_history_expansion.json` | 39 | `discovery_location_id` | `body` | discovery_trigger, era, knowledge_key |
| `patrol_debriefs.json` | 36 | `debrief_id` | `—` | cross_refs, day, distance_km |
| `council_meeting_minutes.json` | 32 | `minutes_id` | `notes` | action_items, agenda, attendees |
| `undertaker_burial_records.json` | 25 | `burial_id`, `deceased_id` | `ledger_entry`, `notes` | age_estimate, attendees, burial_location |
| `bunker_contraband_barter.json` | 20 | `hidden_stash_location`, `id` | `prose` | category, contraband_tier, market_price_scrip |
| `chef_recipe_development.json` | 20 | `recipe_id` | `—` | author, calories_estimate, day |
| `childrens_artwork_batch_2.json` | 20 | `id` | `description` | artist, day, medium |
| `conflict_mediation_records.json` | 20 | `mediation_id` | `summary` | cross_refs, day, dispute_type |
| *(and 7 more files)* | ... | ... | ... | ... |

### Transcripts, Documents & Confessions (18 catalogs, 363 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `ration_records_expansion.json` | 36 | `doc_id` | `transcript` | doc_type, location, material |
| `documents_batch_3.json` | 33 | `doc_id`, `item_id_source` | `physical_description`, `transcript` | lore_flags, material, origin |
| `shelter_songs_expansion.json` | 31 | `doc_id` | `transcript` | doc_type, location, material |
| `weather_almanac_expansion.json` | 30 | `doc_id` | `transcript` | doc_type, location, material |
| `wire_confessions.json` | 30 | `author_id`, `confession_id` | `transcript` | acoustic_environment, author_name, device_type |
| `faction_texts_expansion.json` | 29 | `doc_id` | `transcript` | doc_type, location, material |
| `bureaucratic_documents_expansion.json` | 27 | `doc_id` | `transcript` | doc_type, location, material |
| `shelter_notices_expansion.json` | 26 | `doc_id` | `transcript` | doc_type, location, material |
| `letters_expansion.json` | 25 | `letter_id` | `—` | content, cross_refs, day |
| `trade_ledgers_expansion.json` | 23 | `doc_id` | `transcript` | doc_type, location, material |
| *(and 8 more files)* | ... | ... | ... | ... |

### Craft & Workshop Production Logs (24 catalogs, 180 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `antler_horn_sawing_records.json` | 8 | `id`, `saw_tool_id` | `log_text` | blank_shape_cut, material_type |
| `bark_tanning_vat_logs.json` | 8 | `id`, `tanning_vat_id` | `log_text` | bark_species, liquor_strength_baume |
| `beeswax_clarification_records.json` | 8 | `id` | `log_text` | clarification_method, clarity_grade, wax_lot_source |
| `bisque_firing_records.json` | 8 | `id`, `kiln_chamber_id` | `log_text` | firing_duration_hours, peak_temp_celsius |
| `bone_degreasing_prep_logs.json` | 8 | `id` | `log_text` | bone_source_animal, degreasing_method, prep_duration_days |
| `brain_tanning_hide_reports.json` | 8 | `brain_emulsion_batch_id`, `hide_source_animal`, `id` | `log_text` | smoke_cycle_count |
| `clay_wedging_forming_logs.json` | 8 | `id` | `log_text` | clay_bed_source, forming_method, wedging_cycle_count |
| `drop_spindle_fibre_drafting_logs.json` | 8 | `id`, `spindle_unit_id` | `log_text` | draft_ratio_target, fibre_stock_type |
| `fibre_heckling_prep_logs.json` | 8 | `heckling_comb_id`, `id` | `log_text` | fibre_source_plant, retting_days |
| `inkle_loom_warp_tally_sheets.json` | 8 | `id`, `loom_frame_id` | `log_text` | warp_fibre_type, weft_thread_count |
| *(and 14 more files)* | ... | ... | ... | ... |

### Domain Catalog (entries) (4 catalogs, 131 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `journal_entries_batch_3.json` | 88 | `author_id`, `id` | `text` | author_name, day, hour |
| `journal_entries_batch_1.json` | 18 | `author_id`, `id` | `text` | author_name, day, hour |
| `journal_entries_batch_2.json` | 15 | `author_id`, `id` | `text` | author_name, day, hour |
| `oral_lore_batch_2.json` | 10 | `lore_id` | `performance_context` | genre, lyrics, tempo |

### Domain Catalog (broadcasts) (4 catalogs, 86 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `radio_scripts_expansion.json` | 36 | `broadcast_id` | `transcript` | day_trigger, frequency_mhz, station_name |
| `radio_transcripts_batch_3.json` | 27 | `broadcast_id` | `transcript` | day_trigger, frequency_mhz, station_name |
| `radio_scriptbook.json` | 15 | `broadcast_id` | `transcript` | day_trigger, frequency_mhz, station_name |
| `radio_transcripts_batch_2.json` | 8 | `broadcast_id` | `transcript` | day_trigger, frequency_mhz, station_name |

### Domain Catalog (postings) (2 catalogs, 76 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `graffiti_expansion.json` | 40 | `posting_id` | `—` | author_signature, category, content |
| `bunker_graffiti_postings.json` | 36 | `posting_id` | `—` | author_signature, category, content |

### Domain Catalog (recipes) (2 catalogs, 40 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `culinary_ration_codex.json` | 30 | `recipe_id` | `—` | calories_per_portion, canteen_gossip_review, daily_morale_modifier |
| `culinary_ration_batch_2.json` | 10 | `recipe_id` | `—` | calories_approx, canteen_gossip_review, chef_author |

### Domain Catalog (journal_entries) (1 catalogs, 40 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `journals_expansion.json` | 40 | `id` | `bodyText` | author, day, title |

### Domain Catalog (memorials) (1 catalogs, 40 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `memorials_expansion.json` | 40 | `id` | `text` | name, type |

### Faction Directives & Treaties (2 catalogs, 36 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `dead_hand_directives.json` | 20 | `directive_id` | `archaeological_notes`, `transcript` | clearance_level, crypto_checksum, directive_title |
| `regional_treaty_protocols.json` | 16 | `treaty_id` | `—` | demarcated_territory, penalties, power_quota_kw |

### Trade Ledgers & Caravan Routes (2 catalogs, 33 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `wasteland_trade_caravan_routes.json` | 18 | `route_id` | `—` | caravan_master_log, destination_hub, hazard_index |
| `expedition_route_waypoint_notes_batch_2.json` | 15 | `route_id` | `radiation_notes` | author, difficulty, estimated_travel_time_hours |

### Domain Catalog (relics) (1 catalogs, 32 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `relic_provenance_dossiers.json` | 32 | `relic_id` | `curator_note` | discovery_location, gameplay_effect, material |

### Domain Catalog (briefs) (2 catalogs, 31 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `expedition_briefs_expansion.json` | 30 | `brief_id` | `—` | author, cross_refs, day |
| `expedition_planning_briefs_batch_1.json` | 1 | `brief_id` | `—` | author, cross_refs, day |

### Domain Catalog (dispatches) (1 catalogs, 30 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `courier_dispatches_master.json` | 30 | `dispatch_id` | `transcript` | delivery_status, goods_manifest, recipient |

### Domain Catalog (heirlooms) (1 catalogs, 30 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `dweller_heirlooms_master.json` | 30 | `heirloom_id`, `owner_id` | `item_loss_event_text`, `sensory_memory_text` | daily_morale_modifier, item_name, owner_name |

### Domain Catalog (records) (1 catalogs, 30 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `vinyl_record_archive.json` | 30 | `record_id` | `dweller_resonance_notes`, `needle_audio_texture` | broadcast_frequency_mhz, catalog_number, daily_morale_modifier |

### Domain Catalog (expeditions) (1 catalogs, 30 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `wasteland_expeditions_master.json` | 30 | `expedition_id` | `ambient_prose`, `threat_description` | choices, title, zone |

### Domain Catalog (letters) (1 catalogs, 25 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `survivor_letters_lost_kin.json` | 25 | `letter_id` | `galina_dead_letter_note`, `letter_text` | author_dweller, destination_address, dispatch_attempt_date |

### Domain Catalog (blueprints) (1 catalogs, 24 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `bunker_blueprints_codex.json` | 24 | `room_id` | `chief_engineer_note` | acoustic_noise_db, base_power_draw_kw, catastrophic_failure_mode |

### Domain Catalog (strata) (1 catalogs, 24 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `geological_strata_logs.json` | 24 | `strata_id` | `geologist_field_notes` | compressive_strength_mpa, depth_end_meters, depth_start_meters |

### Domain Catalog (manuals) (1 catalogs, 24 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `lost_tech_manuals.json` | 24 | `manual_id` | `dmitri_engineering_notes`, `schematic_summary` | engineering_discipline, manual_code, origin_facility |

### Domain Catalog (species) (1 catalogs, 24 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `underground_fungi_flora.json` | 24 | `species_id` | `botanist_field_notes` | common_name, edible_calories_per_100g, growth_cycle_days |

### Domain Catalog (creatures) (1 catalogs, 24 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `wasteland_wildlife_bestiary.json` | 24 | `creature_id` | `harlan_scout_notes` | acoustic_lure_frequency_hz, butchered_meat_calories, colloquial_name |

### Engineering Glitches & Failures (1 catalogs, 20 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `bunker_maintenance_glitches.json` | 20 | `glitch_id` | `anomaly_description`, `dmitri_shift_note` | affected_subsystem, diagnostic_telemetry, emergency_protocol |

### Domain Catalog (transactions) (1 catalogs, 20 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `bunker_trade_ledger_batch_2.json` | 20 | `tx_id` | `notes` | day, given, giver |

### Domain Catalog (radio_broadcasts) (1 catalogs, 20 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `radio_broadcast_rundowns.json` | 20 | `rundown_id` | `—` | actual_airtime, broadcast_day, host |

### Domain Catalog (settlements) (1 catalogs, 20 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `wasteland_settlement_gazetteer.json` | 20 | `settlement_id` | `—` | colloquial_name, controlling_faction, defense_fortifications |

### Domain Catalog (documents) (1 catalogs, 18 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `plan17_discoverable_documents.json` | 18 | `id` | `text` | author, date_era, discovery_location |

### Domain Catalog (pamphlets) (1 catalogs, 16 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `currents_pamphlets.json` | 16 | `faction_id`, `ideological_alignment`, `pamphlet_id` | `—` | doctrine, key_figure, liturgy |

### Domain Catalog (songs) (1 catalogs, 16 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `oral_lore_codex.json` | 16 | `lore_id` | `performance_context` | genre, lyrics, meter |

### Domain Catalog (mods) (1 catalogs, 15 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `engineering_mod_notes.json` | 15 | `mod_id` | `—` | author, day, materials_used |

### Domain Catalog (guides) (1 catalogs, 15 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `improvised_repair_guides_batch_2.json` | 15 | `guide_id` | `—` | author, cause, steps |

### Domain Catalog (interviews) (1 catalogs, 15 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `new_arrival_intake_interviews.json` | 15 | `interview_id` | `therapist_note` | admitted, bunk_assigned, day |

### Domain Catalog (surveys) (1 catalogs, 15 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `radiation_survey_readings_batch_2.json` | 15 | `survey_id` | `notes` | day, location, reading_usv_h |

### Domain Catalog (transmissions) (1 catalogs, 12 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `ghost_transmissions.json` | 12 | `transmission_id` | `transcript` | broadcast_format, cipher_clue, estimated_origin |

### Domain Catalog (templates) (2 catalogs, 10 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `jrnl_templates_cycle_c.json` | 6 | `expansion_id`, `template_id` | `body_template` | author_role, hope_earned, stress_delta |
| `jrnl_templates_cycle_d.json` | 4 | `expansion_id`, `template_id` | `body_template` | author_role, hope_earned, stress_delta |

### Domain Catalog (anomalies) (1 catalogs, 8 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `bunker_bureaucratic_anomalies.json` | 8 | `anomaly_id`, `incident_date` | `bureaucratic_summary` | clinical_resolution, designation, reporting_officer |

### Civic & Court Verdicts (1 catalogs, 8 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `bunker_court_verdicts_batch_2.json` | 8 | `evidence`, `verdict_id` | `—` | case_day, charge, defendant |

### Domain Catalog (backstories) (1 catalogs, 6 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `dweller_dependency_backstories.json` | 6 | `backstory_id`, `survivor_id` | `prose`, `summary` | kind, voice |

### Domain Catalog (priority_order) (1 catalogs, 6 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `load_shed_schedule_001.json` | 6 | `—` | `notes` | load_kw, manual_fallback, priority |

### Domain Catalog (rituals) (1 catalogs, 5 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `bunker_rituals_and_cults.json` | 5 | `ritual_id` | `sociological_summary` | administrative_action, name, origin_sector |

### Domain Catalog (contacts) (1 catalogs, 5 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `diplomatic_contact_records_batch_1.json` | 5 | `contact_id` | `external_party_description`, `meeting_summary` | agreements_reached, bunker_overwatch, bunker_representative |

### Domain Catalog (eulogies) (1 catalogs, 4 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `eulogy_corpus_batch_1.json` | 4 | `archetype_id` | `—` | closing, cooking_line, opening |

### Domain Catalog (transcripts) (1 catalogs, 3 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `radio_mysteries_expansion.json` | 3 | `transcript_id` | `analyst_note`, `transcript`, `transcript_id` | frequency_mhz, origin |

### Domain Catalog (missions) (1 catalogs, 1 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `courier_mission_logs_batch_2.json` | 1 | `mission_id` | `summary` | cargo_inbound, cargo_outbound, courier |

### Domain Catalog (sessions) (1 catalogs, 1 records)

Representative catalogs:

| Catalog File | Records | ID Field | Text / Content Fields | Domain Context Fields |
|---|:---:|---|---|---|
| `therapist_session_notes_batch_3.json` | 1 | `session_id` | `summary` | assessment, cross_refs, day |
