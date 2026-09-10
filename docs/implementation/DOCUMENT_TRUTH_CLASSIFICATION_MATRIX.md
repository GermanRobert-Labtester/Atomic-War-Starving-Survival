# Document truth classification matrix

The runtime map is the reviewable authority for this classification. Every static number, date, person and assertion remains inside the transcript. The reader labels context; it does not promote the claim into simulation state.

| Truth class | Count | Documents | Presentation rule |
|---|---:|---|---|
| Historical canonical record | 10 | `bunker_doc_transfer_bram_02`, `bunker_doc_transfer_ward_05`, `bunker_doc_assign_loma_01`, `bunker_doc_assign_single_03`, `bunker_doc_incident_theft_02`, `bunker_doc_incident_injury_05`, `bunker_doc_incident_mortality_07`, `bunker_doc_evacuation_dam`, `bunker_doc_quarantine_ward_b`, `bunker_doc_census_correction` | `ARCHIVED RECORD`; authored chronology and consequences are fixed history |
| Contemporaneous authored record | 6 | `bunker_doc_denial_extra_04`, `bunker_doc_denial_second_09`, `bunker_doc_meal_notice_flour`, `bunker_doc_amnesty_weapons`, `bunker_doc_notice_laundry`, `bunker_doc_notice_school` | `AUTHORED NOTICE`; posted day is a chronology label plus producer gate, never a live status feed |
| Template-compatible record | 11 | `bunker_doc_req_fuel_03`, `bunker_doc_req_medicine_07`, `bunker_doc_req_candles_12`, `bunker_doc_inspect_generator_06`, `bunker_doc_inspect_filters_08`, `bunker_doc_roster_sentry_w7`, `bunker_doc_chit_repair_lamp`, `bunker_doc_chit_repair_pump`, `bunker_doc_roster_kitchen_w9`, `bunker_doc_chit_door_hatch`, `bunker_doc_req_seed_potato_01` | `ARCHIVED FORM · TEMPLATE-COMPATIBLE`; static values are snapshots, and no live template is emitted in Plan 149 |
| Flavor-only artifact | 0 | — | No record met this class after the provenance and continuity review |
| Unsafe/unresolved | 0 shipped; negative fixtures covered | — | Withheld from discovery and marked `WITHHELD · UNRESOLVED` if a map is missing or invalid |

## Numerical and state-like claim audit

| Document(s) | Authored claims audited | Runtime treatment |
|---|---|---|
| `bunker_doc_req_fuel_03` | Day 41; 40 litres requested; 25 L reduced allocation; 12C threshold; three consecutive nights; Days 42–48 | Historical form text. Does not read or mutate fuel, temperature, shelter rooms or patient placement. |
| `bunker_doc_req_medicine_07` | Day 26; three antibiotic courses requested; two approved; one held in reserve | Historical form text. Does not consume or create medicine. |
| `bunker_doc_req_candles_12` | Day 58; 12 candles requested; 11 days without wired light; two injuries; six approved; four-hour 22:00–02:00 window; Days 59–65 | Historical form text. Does not create candles, lighting failure or injury state. |
| `bunker_doc_denial_extra_04` | Day 23; Loma family seven persons; infant four months; headcount 147 versus provisioning basis 140; appeal Day 27 | Authored notice. Does not change population, rations or appeal state. |
| `bunker_doc_denial_second_09`, `bunker_doc_meal_notice_flour` | Day 35 suspension; Day 38 notice; one slice rather than two; serving and flour reductions | Authored notices. Do not alter food inventory or meal portions. |
| `bunker_doc_transfer_bram_02` | Order Day 1, effective Day 2; two days outbound and return; four days total; five days before overdue; four days rations plus one each of water, lamp and blade | Fixed record. Does not dispatch a courier or remove supplies. |
| `bunker_doc_transfer_ward_05`, `bunker_doc_quarantine_ward_b` | Ward B cases 1 and 2; patient admitted Day 12; transfer Day 19; immediate quarantine; clinic isolation | Fixed medical history. Does not alter disease, room capacity, health or isolation. |
| `bunker_doc_inspect_generator_06` | Day 21; 168 hours; 96 litres consumed; 184 L tank; 40 L reserve; cylinder 3 knock; cleaning Day 24; oil change Day 32 | Archived inspection. Does not overwrite generator, fuel or maintenance authority. |
| `bunker_doc_inspect_filters_08` | 62%, 78%, 45%, 51% and 90% occlusion; replace within seven days; three bank-1 and one bank-2 reserves; scheduled Day 37; reserve-after counts 2 and 1 | Archived inspection. Does not mutate ventilation or filters. |
| `bunker_doc_assign_loma_01` | Day 14 arrival; seven persons; four adult and three child cots; full ration from Day 15; pump hours 06:00–20:00; assessment Day 16 | Fixed assignment. Does not add survivors, beds, rations or staffing. |
| `bunker_doc_assign_single_03` | Day 37 arrival; identifier `UNIDENTIFIED-37A`; Ward B B-7; full ration Day 38; back-ration Day 37; clinic intake Day 38 | Fixed assignment. The unidentified person is not a phantom survivor. |
| `bunker_doc_incident_theft_02` | Incident between Days 20–22; report Day 22; two 25 kg flour sacks; three key-holders | Fixed incident. Does not remove flour or assign blame. |
| `bunker_doc_incident_injury_05` | Day 56 at about 23:00; report Day 57; prior injury Day 52; 22:00–02:00 shift; aisle 4 closure | Fixed incident. Does not apply injury, lighting or candle state. |
| `bunker_doc_incident_mortality_07` | Day 33 at 04:00; transfer from Day 19; admission Day 12; two broad-spectrum and one narrow course; death 04:00; cold room at 06:00; burial Day 34 | Fixed medical history. Does not kill, heal, move or create a survivor. |
| `bunker_doc_roster_sentry_w7` | 24-hour and 12-hour rotations; six-hour blocks; dosimeter every two hours; Bram returned Day 5 | Archived roster. Does not schedule or staff the live duty roster. |
| `bunker_doc_chit_repair_lamp` | Day 49; lamp 3; six-second clicking; Ward C school corner | Archived maintenance context. Does not create a fault or repair job. |
| `bunker_doc_chit_repair_pump` | Day 52; queue position 4; worn gasket; no replacement | Archived maintenance context. Does not change water or maintenance queue. |
| `bunker_doc_evacuation_dam` | Day 4 03:30 receipt; 03:45 broadcast; 04:00 notification; 04:15 evacuation; Day 5 04:00 gate closure | Historical radio transcription. Does not open routes or move population. |
| `bunker_doc_amnesty_weapons` | Day 60; voluntary surrender and notice scope | Authored notice. Does not alter inventory or law state. |
| `bunker_doc_census_correction` | Day 62; Day 30 headcount/basis 140; corrected headcount 147; Loma arrival Day 14 plus seven; reductions since Day 23 | Historical correction. Does not rewrite live population or provisioning basis. |
| `bunker_doc_roster_kitchen_w9` | Week 9; named role slots; prior Day 35 chalk reference | Archived roster. Does not schedule kitchen labor. |
| `bunker_doc_notice_laundry` | Day 27 suspension and water/linen references | Authored notice. Does not change water, laundry or linen. |
| `bunker_doc_notice_school` | Daily 10:00–11:00 school period and child access | Authored notice. Does not create a school or staffing assignment. |
| `bunker_doc_req_seed_potato_01` | Day 8; four kg seed potatoes; cold frame; seasonal bet | Archived requisition. Does not add seed or alter greenhouse state. |

The exact claim text remains available in the source transcript. This matrix records why each claim is safe to show as paper and unsafe to use as a live write.
