# Plan 118 — Baseline Forensic Report: Standing Record Quests Expansion

> **Scope:** Forensic reconnaissance of `standing_record_quests.json`, `StandingRecordCatalog.cs`, `StandingRecordEngine.cs`, `standing_record_memory.json`, `standing_record_layouts.json`, and cross-system dependencies prior to the Plan 118 expansion.

---

## 1. Initial State & Repository Truth

| Vector | Plan Assumption | Repository Reality | Notes / Reconciliation |
|---|---|---|---|
| **Catalog Path** | `Assets/StreamingAssets/Data/standing_record_quests.json` | `Assets/StreamingAssets/Data/standing_record_quests.json` | Exact match. Build mirror at `builds/linux/...` verified synchronized. |
| **Baseline Count** | 10 baseline quests | 22 quests (10 original territorial-record + 12 site forensics) | Quests 1–10 are the original foundational territorial quests (`quest_record_the_plate` through `quest_record_which_gazetteer`). Quests 11–22 are site-forensics quests added in commit `7738facc` (`quest_record_vault_breach_forensics` through `quest_record_the_last_watch_beacon`), pinned by `Plan18ExpansionDeepeningTests` (`srCat.Quests.Count >= 22`). |
| **Expansion Target** | Add 10 new quests (10 → 20 territorial-record quests) | Add 10 new quests (total catalog 22 → 32) | Preserves all 22 existing quests intact; expands the territorial-record layer from 10 to 20 quests. |
| **Catalog DTO** | `StandingRecordCatalog.cs` | `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs` | Confirmed live. Loads wrapped `{"schema_version": 1, "quests": [...]}` into `StandingRecordQuestEntry` objects. |
| **Runtime Engine** | `StandingRecordEngine.cs` | `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` | Coordinates `LocationLayoutSystem`, `LocationMemorySystem`, and `SiteEncounterSystem`. |
| **Headless Demo** | `StandingRecordHeadlessDemo.cs` | `Assets/Ashfall.Core/StandingRecord/StandingRecordHeadlessDemo.cs` | Verifies `complete_mutation` is non-empty and `StageCount >= 3` for every quest in `standing_record_quests.json`. |

---

## 2. Frozen Baseline Roster (Quests 1–22)

### Foundational Territorial-Record Quests (1–10)
1. **`quest_record_the_plate`** ("The Plate on the Last Lamp", Day 75, `loc_cut_kilometre_19`, prereq: `""`)
2. **`quest_record_grease_pencil`** ("Under Glass", Day 75, `loc_transit_authority_hq`, prereq: `quest_record_the_plate`)
3. **`quest_record_wrong_stacks`** ("Grey Brick", Day 75, `loc_municipal_archive`, prereq: `quest_record_grease_pencil`)
4. **`quest_record_the_book`** ("The Visited Column", Day 75, `location_ministry_of_truth_bunker`, prereq: `quest_record_wrong_stacks`)
5. **`quest_record_mass_or_lot`** ("The Needle", Day 75, `loc_weighbridge`, prereq: `quest_record_the_book`)
6. **`quest_record_hands`** ("Plot 114", Day 75, `loc_grange_hall`, prereq: `quest_record_mass_or_lot`)
7. **`quest_record_friendly_obstacle`** ("Listed Charges", Day 75, `loc_bridge_seven`, prereq: `quest_record_hands`)
8. **`quest_record_the_failure`** ("As Far As It Opened", Day 75, `loc_lock_gate_four`, prereq: `quest_record_friendly_obstacle`)
9. **`quest_record_fallback`** ("Fourteen, a Gap, Six", Day 75, `loc_alloc_12b`, prereq: `quest_record_the_failure`)
10. **`quest_record_which_gazetteer`** ("The Second Copy", Day 75, `location_the_memory_vault`, prereq: `quest_record_fallback`)

### Deepening Excavation / Site-Forensics Quests (11–22)
11. **`quest_record_vault_breach_forensics`** ("The Blasted Sump", Day 80, `loc_excavation_command_vault`, prereq: `quest_record_the_plate`)
12. **`quest_record_metro_derailment_triage`** ("Platform 3 Siding", Day 85, `loc_excavation_metro_interchange`, prereq: `quest_record_the_plate`)
13. **`quest_record_mine_shaft_adit_collapse`** ("Adit 4 Shift Rota", Day 90, `loc_excavation_mine_shaft`, prereq: `quest_record_the_plate`)
14. **`quest_record_archive_burn_layer`** ("The Smoldering Index", Day 95, `loc_excavation_archive_bunker`, prereq: `quest_record_the_book`)
15. **`quest_record_sluice_failure_verdict`** ("The Broken Pinion at Gate 4", Day 100, `loc_lock_gate_four`, prereq: `quest_record_the_plate`)
16. **`quest_record_seed_bank_purge_trace`** ("The Emptied Canister", Day 105, `loc_seed_library_annex`, prereq: `quest_record_the_book`)
17. **`quest_record_sub_basement_blueprint`** ("The Unmapped Annex", Day 110, `loc_logistics_reserve_cache`, prereq: `quest_record_grease_pencil`)
18. **`quest_record_transit_vent_shaft_route`** ("Bypass Duct 11-East", Day 115, `loc_transit_authority_hq`, prereq: `quest_record_grease_pencil`)
19. **`quest_record_cold_store_sublevel`** ("Brine Tank Subfloor", Day 120, `loc_cold_store_atlantic`, prereq: `quest_record_the_plate`)
20. **`quest_record_utility_junction_crossover`** ("Conduit Chamber 7", Day 125, `loc_excavation_utility_tunnels`, prereq: `quest_record_the_plate`)
21. **`quest_record_the_unmarked_plaque`** ("A Name on the Abutment", Day 130, `loc_railway_span_44_alpha`, prereq: `quest_record_the_plate`)
22. **`quest_record_the_last_watch_beacon`** ("The Cairn at Summit 9", Day 135, `loc_summit_relay`, prereq: `quest_record_the_plate`)

---

## 3. Ten Planned Territorial-Record Quests to Add

1. `quest_record_the_survey_nail` (Day 80, `loc_cut_kilometre_19`, prereq: `quest_record_the_plate`): Disputed boundary nail vs cadastral record at Kilometre 19 seam.
2. `quest_record_the_overlay_pigment` (Day 85, `loc_cut_kilometre_19`, prereq: `quest_record_the_survey_nail`): Wet lampblack pigment in crate indicating plate placed over older stencil.
3. `quest_record_the_second_count` (Day 95, `loc_municipal_archive`, prereq: `quest_record_the_overlay_pigment`): 26 physical lamps vs 24 registered posts in municipal ledger.
4. `quest_record_the_lamp_keepers_oath` (Day 90, `loc_bridge_seven`, prereq: `quest_record_the_plate`): Unsworn keeper's oath obligating fuel maintenance of dead Post 7.
5. `quest_record_the_boundary_dispute` (Day 100, `loc_highway_checkpoint`, prereq: `quest_record_the_survey_nail`): Compact vs Garrison parcel rights arbitration around road nails (Plan 98 integration).
6. `quest_record_the_missing_plate` (Day 105, `loc_abandoned_tide_gauge`, prereq: `quest_record_the_overlay_pigment`): Pried brass plate on salt wellhead at Greywater Tide Gauge (Plan 82 integration).
7. `quest_record_the_cold_survey` (Day 110, `loc_west_ridge_survey`, prereq: `quest_record_the_survey_nail`): Winter theodolite triangulation cairn inspection under blizzard conditions (Plan 76 integration).
8. `quest_record_the_lamp_oil_ledger` (Day 115, `loc_weighbridge`, prereq: `quest_record_the_lamp_keepers_oath`): 400-litre oil discrepancy between weighbridge dockets and dry Grange drums (Plan 98 integration).
9. `quest_record_the_rejected_survey` (Day 120, `loc_coastal_meteorological_station`, prereq: `quest_record_the_second_count`): 1993 coastal cliff subsidence survey bearing REJECTED stamp (Plan 82 integration).
10. `quest_record_the_last_sector` (Day 140, `loc_birchline_weather_station`, prereq: `quest_record_the_cold_survey`): Unlit Sector 9 frontier incorporation vs permanent dark border (Plan 76 + Plan 98 integration).

---

## 4. Mechanical Guarantees
- **World-Change Bar:** Non-empty `complete_mutation` on every quest.
- **Spatial Bar:** Minimum 4 stages per quest (`StageCount >= 3`).
- **DAG Invariant:** Zero cycles; all prerequisites resolve to earlier valid quests.
- **Pacing:** `min_day` span from Day 80 to Day 140.
- **Zero Engine Code:** 100% data and test validation.
