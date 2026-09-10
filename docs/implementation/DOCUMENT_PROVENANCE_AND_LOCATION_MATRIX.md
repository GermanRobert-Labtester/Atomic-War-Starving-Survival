# Document provenance and location matrix

`location` is an authored provenance string. It is shown verbatim in the document header. The audit found no exact, stable room ID shared by the shelter room catalog and all 27 strings, so every shipped `location_id` is intentionally empty. A future mapping must be added here and in the separate runtime map only after exact resolution.

| Provenance family | Documents | Runtime location decision |
|---|---|---|
| Quartermaster's inbox / office | `bunker_doc_req_fuel_03`, `bunker_doc_req_medicine_07`, `bunker_doc_req_candles_12`, `bunker_doc_incident_theft_02`, `bunker_doc_census_correction`, `bunker_doc_req_seed_potato_01` | Preserve human-readable inbox/office text; no `room_id` inferred |
| Ward A/B boards and desks | `bunker_doc_denial_extra_04`, `bunker_doc_transfer_ward_05`, `bunker_doc_quarantine_ward_b`, `bunker_doc_notice_laundry` | Preserve ward provenance; no generic ward entity created |
| Mess hall surfaces | `bunker_doc_denial_second_09`, `bunker_doc_meal_notice_flour`, `bunker_doc_roster_kitchen_w9` | Preserve surface text; no current ration or kitchen state read |
| Gate house / perimeter records | `bunker_doc_transfer_bram_02`, `bunker_doc_roster_sentry_w7`, `bunker_doc_evacuation_dam`, `bunker_doc_amnesty_weapons` | Preserve gate-house and radio provenance; no route or dispatch mutation |
| Clinic records | `bunker_doc_transfer_ward_05`, `bunker_doc_incident_mortality_07` | Preserve clinic/patient-folder provenance; no medical mutation |
| Maintenance / generator records | `bunker_doc_inspect_generator_06`, `bunker_doc_inspect_filters_08`, `bunker_doc_incident_injury_05`, `bunker_doc_chit_repair_lamp`, `bunker_doc_chit_repair_pump`, `bunker_doc_chit_door_hatch` | Preserve office/logbook/inbox text; no infrastructure mutation |
| Admissions / family files | `bunker_doc_assign_loma_01`, `bunker_doc_assign_single_03` | Preserve filing provenance; no roster or population mutation |
| Ward C / school corridor | `bunker_doc_notice_school`, `bunker_doc_chit_repair_lamp` | Preserve corridor provenance; no school/room object created |

## Explicit related-document edges

The edges below are the only navigation relationships shipped. They are ID-to-ID links from the runtime map, validated at load. Missing targets are filtered with a warning; keyword similarity is never used by the reader.

| Record | Related records |
|---|---|
| `bunker_doc_req_medicine_07` | `bunker_doc_transfer_ward_05` |
| `bunker_doc_req_candles_12` | `bunker_doc_incident_injury_05` |
| `bunker_doc_denial_extra_04` | `bunker_doc_assign_loma_01`, `bunker_doc_census_correction` |
| `bunker_doc_denial_second_09` | `bunker_doc_meal_notice_flour` |
| `bunker_doc_transfer_bram_02` | `bunker_doc_roster_sentry_w7` |
| `bunker_doc_transfer_ward_05` | `bunker_doc_req_medicine_07`, `bunker_doc_incident_mortality_07`, `bunker_doc_quarantine_ward_b` |
| `bunker_doc_assign_loma_01` | `bunker_doc_denial_extra_04`, `bunker_doc_census_correction`, `bunker_doc_roster_kitchen_w9` |
| `bunker_doc_incident_injury_05` | `bunker_doc_req_candles_12` |
| `bunker_doc_incident_mortality_07` | `bunker_doc_transfer_ward_05`, `bunker_doc_quarantine_ward_b` |
| `bunker_doc_meal_notice_flour` | `bunker_doc_denial_second_09` |
| `bunker_doc_chit_repair_lamp` | `bunker_doc_notice_school` |
| `bunker_doc_quarantine_ward_b` | `bunker_doc_transfer_ward_05`, `bunker_doc_incident_mortality_07` |
| `bunker_doc_census_correction` | `bunker_doc_assign_loma_01`, `bunker_doc_denial_extra_04` |
| `bunker_doc_roster_kitchen_w9` | `bunker_doc_assign_loma_01` |
| `bunker_doc_notice_school` | `bunker_doc_assign_loma_01`, `bunker_doc_chit_repair_lamp` |

The graph is intentionally directional in data even where the narrative relationship is reciprocal. The reader can offer the forward links without inventing an inverse edge.
