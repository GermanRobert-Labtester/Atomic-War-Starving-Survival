# Bureaucratic document inventory

Source: `narrative/bureaucratic_documents_expansion.json`. Transcript length is measured after JSON decoding, with line breaks retained. Tags are the authored tag array.

| ID | Type | Day | Posted by | Location | Material | Tags | Transcript chars |
|---|---|---:|---|---|---|---|---:|
| `bunker_doc_req_fuel_03` | requisition_form | 41 | Ward B Duty Clerk | Quartermaster's inbox, Main Office | Pre-printed form, carbon-copy, fields filled in pencil | requisition, fuel, ward_b, reduction | 892 |
| `bunker_doc_req_medicine_07` | requisition_form | 26 | Clinic Duty Nurse | Quartermaster's inbox, Main Office | Pre-printed form, carbon-copy, fields filled in pen | requisition, medicine, clinic, reserve | 798 |
| `bunker_doc_req_candles_12` | requisition_form | 58 | Sub-Level 3 Steward | Quartermaster's inbox, Main Office | Pre-printed form, carbon-copy, fields filled in pencil, smudged | requisition, candles, sub_level_3, safety | 890 |
| `bunker_doc_denial_extra_04` | ration_denial_notice | 23 | Quartermaster | Ward A notice board, pinned over the meal roster | Pre-printed notice, fields filled in pen, stamped DENIED | ration, denial, loma, appeal, infant | 1004 |
| `bunker_doc_denial_second_09` | ration_denial_notice | 35 | Mess Hall Steward | Mess hall entrance, taped to the serving hatch | Handwritten on the back of a prior notice, dated | ration, denial, mess_hall, flour, children | 784 |
| `bunker_doc_transfer_bram_02` | transfer_order | 1 | Captain of the Gate | Gate house dispatch box | Pre-printed order, sealed with wax, fields filled in pen | transfer, courier, treaty, route | 693 |
| `bunker_doc_transfer_ward_05` | transfer_order | 19 | Doctor (senior) | Ward B duty desk, filed | Handwritten on clinic letterhead | transfer, patient, clinic, ward_b, isolation | 765 |
| `bunker_doc_inspect_generator_06` | inspection_sheet | 21 | Engineer Tomas | Generator room logbook, page 47 | Pre-printed sheet, columns filled in pencil, grease-marked | inspection, generator, bearing, failure, supply | 1224 |
| `bunker_doc_inspect_filters_08` | inspection_sheet | 30 | Maintenance Crew Lead | Maintenance office, filed | Pre-printed sheet, columns filled in pen, water-stained | inspection, filters, air, sub_level_2, reserve | 994 |
| `bunker_doc_assign_loma_01` | shelter_assignment | 14 | Duty Clerk, Admissions | Admissions counter, filed in family folder | Pre-printed slip, carbon-copy, fields filled in pen | assignment, loma, ward_a, family, school | 1252 |
| `bunker_doc_assign_single_03` | shelter_assignment | 37 | Duty Clerk, Admissions | Admissions counter, filed under UNIDENTIFIED | Pre-printed slip, carbon-copy, fields filled in pencil, name field blank | assignment, unidentified, ward_b, single, ring | 1181 |
| `bunker_doc_incident_theft_02` | incident_report | 22 | Quartermaster | Quartermaster's office, filed | Pre-printed form, fields filled in pen, marked PRIORITY | incident, theft, flour, west_locker, internal | 1520 |
| `bunker_doc_incident_injury_05` | incident_report | 56 | Sub-Level 3 Steward | Maintenance office, filed | Pre-printed form, fields filled in pencil | incident, injury, sub_level_3, dark, candles | 1339 |
| `bunker_doc_incident_mortality_07` | incident_report | 33 | Doctor (senior) | Clinic, filed in patient folder | Pre-printed form, fields filled in pen, stamped CLOSED | incident, mortality, ward_b, clinic, river_woman, ring | 1273 |
| `bunker_doc_roster_sentry_w7` | shift_schedule | 44 | Captain of the Gate | Gate house notice board | Typed on recycled paper, pinned with two nails | sentry, roster, week_7, bram | 695 |
| `bunker_doc_meal_notice_flour` | meal_notice | 38 | Mess Hall Steward | Mess hall entrance, over the serving hatch | Handwritten on the back of the Day 35 notice | meal, flour, reduction, notice | 596 |
| `bunker_doc_chit_repair_lamp` | maintenance_chit | 49 | Duty Clerk, Ward C | Maintenance office inbox | Pre-printed chit, fields filled in pencil, grease-marked | maintenance, lamp, ward_c, school, ballast | 891 |
| `bunker_doc_chit_repair_pump` | maintenance_chit | 52 | Duty Clerk, South Corridor | Maintenance office inbox | Pre-printed chit, fields filled in pen | maintenance, pump, water, gasket, boot | 909 |
| `bunker_doc_evacuation_dam` | evacuation_order | 4 | Dam Operator (relayed via Relay Station Kestrel-9) | Gate house, received by radio, transcribed | Handwritten on gate house log paper, radio transcription | evacuation, dam, relay, kestrel_9, high_school | 1191 |
| `bunker_doc_quarantine_ward_b` | quarantine_order | 19 | Doctor (senior) | Ward B door, posted; copy to Captain of the Gate | Pre-printed order, stamped QUARANTINE, fields filled in pen | quarantine, ward_b, respiratory, clinic, sheet | 1226 |
| `bunker_doc_amnesty_weapons` | amnesty_notice | 60 | Captain of the Gate | Gate house notice board, posted at all wards | Pre-printed notice, fields filled in pen, stamped VOLUNTARY | amnesty, weapons, voluntary, gate_house | 1136 |
| `bunker_doc_census_correction` | census_correction | 62 | Quartermaster | Quartermaster's office, filed; copy to Captain of the Gate | Pre-printed form, fields filled in pen, stamped CORRECTED | census, correction, headcount, loma, honesty | 1490 |
| `bunker_doc_roster_kitchen_w9` | shift_schedule | 60 | Mess Hall Steward | Mess hall entrance, by the schedule board | Handwritten on the back of a prior roster | kitchen, roster, week_9, scavenger | 806 |
| `bunker_doc_notice_laundry` | general_notice | 27 | Duty Clerk, Ward B | Ward B notice board, pinned over the wash basin | Handwritten on a scrap of inventory paper | laundry, suspension, ward_b, water, linen | 757 |
| `bunker_doc_chit_door_hatch` | maintenance_chit | 18 | Gate House Sentry | Maintenance office inbox, marked PRIORITY | Pre-printed chit, fields filled in pen, stamped PRIORITY | maintenance, hatch, seal, priority, rope_tar | 904 |
| `bunker_doc_notice_school` | general_notice | 16 | Teacher Suki | Ward C corridor, by the chalk diagram | Handwritten on the back of a meal notice | school, children, ward_c, cycle, chalk | 725 |
| `bunker_doc_req_seed_potato_01` | requisition_form | 8 | Farmer Petr | Quartermaster's inbox, Main Office | Pre-printed form, fields filled in pencil, dirt on the corner | requisition, seed_potato, cold_frame, season, bet | 1238 |

## Controlled vocabulary census

`doc_type` is currently a display category only. The loader preserves the authored string and does not dispatch by type. Counts are: `requisition_form` 4; `incident_report` 3; `maintenance_chit` 3; `ration_denial_notice` 2; `transfer_order` 2; `inspection_sheet` 2; `shelter_assignment` 2; `shift_schedule` 2; `general_notice` 2; and one each of `meal_notice`, `evacuation_order`, `quarantine_order`, `amnesty_notice` and `census_correction`.
