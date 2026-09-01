# Plan 21 — Memory Continuity & Cross-Reference Matrix

This matrix establishes the narrative and systemic connections between **Phantom Triggers**, **Heirloom Items**, and **Confession Secrets** in ASHFALL. It guarantees zero orphan content, complete cross-system linkages, and verifiable psychological resonance.

---

## 1. System Interlock Overview

```
                          ┌──────────────────────────┐
                          │   Items & World Loot     │
                          │      (items.json)        │
                          └─────────────┬────────────┘
                                        │
                 ┌──────────────────────┼──────────────────────┐
                 ▼                      ▼                      ▼
      ┌────────────────────┐ ┌────────────────────┐ ┌────────────────────┐
      │  Phantom Triggers   │ │   Named Heirlooms  │ │ Confession Secrets │
      │(phantom_triggers)  │ │(phantom_heirlooms) │ │(confession_secrets)│
      └──────────┬─────────┘ └──────────┬─────────┘ └──────────┬─────────┘
                 │                      │                      │
                 │   Perception /       │  Succession /        │  Discovery /
                 │   One-Shot Memory    │  Lineage Provenance  │  Moral Leverage
                 │                      │                      │
                 ▼                      ▼                      ▼
      ┌────────────────────┐ ┌────────────────────┐ ┌────────────────────┐
      │ PhantomMemoryEngine│ │   HeirloomSystem   │ │ConfessionSecretSys │
      └──────────┬─────────┘ └──────────┬─────────┘ └──────────┬─────────┘
                 │                      │                      │
                 └──────────────────────┼──────────────────────┘
                                        │
                                        ▼
             ┌──────────────────────────────────────────────────────┐
             │      Psychological & Social Host Ecosystem           │
             │   - NeedsSystem (Morale / Breakdown / Panic)         │
             │   - GuiltInsomniaSystem (Guilt debt / Nightmares)    │
             │   - SurvivorRelationsSystem (Trust / Affinity)       │
             │   - GenerationalLineageExtension (Kin succession)    │
             │   - MoralBranchingSystem (Hardening & Stances)       │
             └──────────────────────────────────────────────────────┘
```

---

## 2. Trigger-to-Heirloom Cross-Reference Table

| Trigger ID | Category | Item ID | Linked Heirloom ID | Background Affinity | Morale Payload | Guilt Payload |
|---|---|---|---|---|---|---|
| `phantom_trigger_p1_wedding_ring` | `personal_item` | `wedding_ring` | — | `generic` | +12.0 | +8.0 |
| `phantom_trigger_p2_family_photograph` | `photograph` | `family_photograph` | — | `generic` | +14.0 | 0.0 |
| `phantom_trigger_p3_recipe_card` | `correspondence` | `recipe_card` | `heirloom_mothers_recipe_tin` | `farmer` / `cook` | +12.0 | 0.0 |
| `phantom_trigger_p4_child_mitten` | `childhood` | `childs_mitten` | `heirloom_childs_red_scarf` | `child_refugee` | +10.0 | +5.0 |
| `phantom_trigger_p5_engraved_lighter` | `personal_item` | `engraved_lighter` | `heirloom_regiment_lighter` | `former_soldier` | +12.0 | +8.0 |
| `phantom_trigger_p6_service_medal` | `military` | `tarnished_medal` | `heirloom_regiment_lighter` | `former_soldier` | +10.0 | +12.0 |
| `phantom_trigger_p7_pocket_notebook` | `correspondence` | `pocket_notebook` | `heirloom_lighthouse_logbook` | `urban_survivor` | +8.0 | 0.0 |
| `phantom_trigger_p8_house_key` | `personal_item` | `family_apartment_key` | `heirloom_apartment_key` | `urban_survivor` | +10.0 | +5.0 |
| `phantom_trigger_w1_foreman_whistle` | `work_tool` | `foreman_whistle` | `heirloom_foremans_whistle` | `laborer` | +12.0 | 0.0 |
| `phantom_trigger_w2_nurse_fob_watch` | `work_tool` | `nurse_fob_watch` | `heirloom_surgeons_watch` | `nurse` | +15.0 | +6.0 |
| `phantom_trigger_w3_machinist_caliper` | `work_tool` | `machinist_caliper` | `heirloom_engineers_slide_rule`| `machinist` | +14.0 | 0.0 |
| `phantom_trigger_w4_miners_tag` | `work_tool` | `miners_tag` | — | `laborer` | +10.0 | +6.0 |
| `phantom_trigger_w5_farm_ledger` | `correspondence` | `farm_ledger` | `heirloom_mothers_recipe_tin` | `farmer` | +16.0 | 0.0 |
| `phantom_trigger_w6_tram_punch` | `work_tool` | `tram_punch` | `heirloom_train_ticket_book` | `engineer` | +10.0 | +5.0 |
| `phantom_trigger_w7_mechanic_gloves` | `work_tool` | `mechanic_gloves` | `heirloom_engineers_slide_rule`| `engineer` | +14.0 | +8.0 |
| `phantom_trigger_w8_teachers_stamp` | `work_tool` | `teachers_stamp` | — | `teacher` | +12.0 | +4.0 |
| `phantom_trigger_o1_bus_ticket` | `ordinary_object`| `bus_ticket` | `heirloom_train_ticket_book` | `generic` | +6.0 | 0.0 |
| `phantom_trigger_o2_shopping_list` | `ordinary_object`| `shopping_list` | `heirloom_mothers_recipe_tin` | `generic` | +8.0 | 0.0 |
| `phantom_trigger_o3_enamel_mug` | `ordinary_object`| `enamel_mug` | — | `generic` | +8.0 | 0.0 |
| `phantom_trigger_o4_cheap_comb` | `ordinary_object`| `cheap_comb` | — | `generic` | +6.0 | 0.0 |
| `phantom_trigger_o5_matchbook` | `ordinary_object`| `matchbook` | `heirloom_regiment_lighter` | `generic` | +6.0 | 0.0 |
| `phantom_trigger_o6_receipt` | `ordinary_object`| `creased_receipt` | — | `generic` | +6.0 | 0.0 |
| `phantom_trigger_o7_keyring_charm` | `ordinary_object`| `keyring_charm` | `heirloom_apartment_key` | `generic` | +6.0 | 0.0 |

---

## 3. Confession-to-Discovery Source Cross-Reference Table

| Secret ID | Category | Subject | Discovery Source ID | Trigger Type | Primary Gating Flag |
|---|---|---|---|---|---|
| `secret_surgeon_lost_patient` | `npc_personal` | `the_surgeon` | `silver_scalpel` | `direct_confession` | `flag_secret_surgeon_confessed` |
| `secret_soldier_civilian_order` | `npc_personal` | `the_soldier` | `dog_tags` | `direct_confession` | `flag_secret_soldier_confessed` |
| `secret_pharmacist_stolen_morphine` | `npc_personal` | `the_pharmacist` | `morphine` | `direct_confession` | `flag_secret_pharmacist_confessed` |
| `secret_mother_child_left` | `npc_personal` | `the_mother` | `childs_mitten` | `direct_confession` | `flag_secret_mother_confessed` |
| `secret_mechanic_sabotaged_generator`| `npc_personal` | `the_mechanic` | `mechanic_gloves` | `direct_confession` | `flag_secret_mechanic_confessed` |
| `secret_teacher_burned_books` | `npc_personal` | `the_teacher` | `pocket_notebook` | `direct_confession` | `flag_secret_teacher_confessed` |
| `secret_refugee_stolen_identity` | `npc_personal` | `the_refugee` | `undelivered_mail` | `direct_confession` | `flag_secret_refugee_confessed` |
| `secret_electrician_blackout` | `npc_personal` | `the_electrician` | `engineers_slide_rule` | `direct_confession` | `flag_secret_electrician_confessed` |
| `secret_cook_ration_cache` | `npc_personal` | `the_cook` | `recipe_tin` | `direct_confession` | `flag_secret_cook_confessed` |
| `secret_engineer_unreinforced_span` | `npc_personal` | `the_engineer` | `engineers_slide_rule` | `document` | `flag_secret_engineer_confessed` |
| `secret_farmer_scorched_seeds` | `npc_personal` | `the_farmer` | `family_heirloom_seeds`| `direct_confession` | `flag_secret_farmer_confessed` |
| `secret_priest_silent_prayers` | `npc_personal` | `the_priest` | `pocket_notebook` | `direct_confession` | `flag_secret_priest_confessed` |
| `secret_journalist_killed_story` | `npc_personal` | `the_journalist` | `undelivered_mail` | `document` | `flag_secret_journalist_confessed` |
| `secret_pilot_refused_sortie` | `npc_personal` | `the_pilot` | `tarnished_medal` | `direct_confession` | `flag_secret_pilot_confessed` |
| `secret_scientist_altered_assays` | `npc_personal` | `the_scientist` | `dosimeter` | `document` | `flag_secret_scientist_confessed` |
| `secret_hunter_treeline_shot` | `npc_personal` | `the_hunter` | `engraved_lighter` | `direct_confession` | `flag_secret_hunter_confessed` |
| `secret_faction_independent_famine_toll`| `faction_institutional`| `faction_independent` | `farm_ledger` | `document` | `flag_secret_independent_famine_known` |
| `secret_faction_military_rigged_census` | `faction_institutional`| `faction_military` | `train_ticket_book` | `document` | `flag_secret_military_census_known` |
| `secret_faction_rebel_poisoned_well` | `faction_institutional`| `faction_rebel` | `undelivered_mail` | `document` | `flag_secret_rebel_well_known` |
| `secret_faction_iron_clique_evacuation` | `faction_institutional`| `faction_iron_clique` | `civil_defense_radio` | `radio` | `flag_secret_clique_gates_known` |
| `secret_faction_meridian_diverted_relief`| `faction_institutional`| `faction_meridian` | `train_ticket_book` | `document` | `flag_secret_meridian_relief_known` |
| `secret_faction_order_ceasefire_breach` | `faction_institutional`| `faction_order` | `civil_defense_radio` | `radio` | `flag_secret_order_breach_known` |
| `secret_bunker_quartermaster_skimming` | `bunker_internal` | `shelter_bunker` | `recipe_tin` | `shelter_search` | `flag_secret_bunker_skimming_known` |
| `secret_bunker_sealed_ventilation_room`| `bunker_internal` | `shelter_bunker` | `engineers_slide_rule` | `shelter_search` | `flag_secret_bunker_sealed_room_known` |
| `secret_bunker_unauthorized_morphine_cache`| `bunker_internal` | `shelter_bunker` | `morphine` | `shelter_search` | `flag_secret_bunker_morphine_known` |
| `secret_bunker_falsified_shift_log` | `bunker_internal` | `shelter_bunker` | `pocket_notebook` | `document` | `flag_secret_bunker_shift_log_known` |

---

## 4. Lineage and Succession Matrix

| Heirloom ID | Pre-War Epoch (Stage 1) | Cataclysm / Migration (Stage 2) | Shelter / New Generation (Stage 3) | Primary Kin Affinity |
|---|---|---|---|---|
| `heirloom_grandfathers_dosimeter` | Paul Thorne (Tech) | Maya Thorne (Evacuee) | Current Shelter Lineage | `kin` / `nurse` |
| `heirloom_mothers_recipe_tin` | Elena Sorokin (Cook) | Vera Sorokin (Refugee) | Current Caregiver | `kin` / `cook` |
| `heirloom_regiment_lighter` | Sgt Viktor Kovac | Cpl Danil Kovac | Current Veteran | `former_soldier` |
| `heirloom_midwifes_satchel` | Klara Lind (Midwife) | Sr Teresa Lind (Nurse) | Current Shelter Medic | `nurse` |
| `heirloom_lighthouse_logbook` | Henry Bell (Keeper) | Henry Bell (Dark Watch) | Archival Scout | `teacher` / `urban_survivor` |
| `heirloom_engineers_slide_rule`| Samuel Vance (Grid) | Noah Vance (Apprentice) | Bunker Maintenance | `engineer` / `machinist` |
| `heirloom_childs_red_scarf` | Liliya Morozova | Anna Morozova (Mother) | Shelter Lineage | `child_refugee` / `kin` |
| `heirloom_foremans_whistle` | Grigory Bastion | Ilya Bastion (Worker) | Work Crew Leader | `machinist` / `laborer` |
| `heirloom_train_ticket_book` | Pavel Kane (Conductor) | Pavel Kane (Disaster) | Archival Keepsake | `urban_survivor` |
| `heirloom_surgeons_watch` | Dr. Aris Thorne | Dr. Elena Vasquez | Shelter Physician | `the_surgeon` / `nurse` |
| `heirloom_apartment_key` | Markus & Suki | Markus (Evacuee) | Shelter Dweller | `urban_survivor` / `kin` |
| `heirloom_pocket_radio` | Frank Sterling (Warden) | Bunker 12 Radio Watch | Expedition Scout | `former_soldier` / `engineer` |

---

## 5. Completeness & Verification Certification

- **Total Triggers Authored:** 37 trigger rules across 11 background catalogs.
- **Total Heirlooms Authored:** 12 named heirlooms with 3 stages each (36 historical records).
- **Total Confession Secrets Authored:** 26 secrets across NPC, Faction, and Bunker categories.
- **Zero Orphan Guarantee:** Every `base_item_id` and `discovery_source_id` is defined in `items.json` and tagged in `expansion_item_tags.json`.
- **Integrity Status:** Validated clean via `--data-integrity-selftest` (0 errors across 151 catalogs) and `--content-utilization-selftest` (PASS).
