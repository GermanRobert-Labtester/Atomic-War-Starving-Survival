# Plan 118 Closeout Report — Standing Record Quests Expansion

> **Task Title:** Plan 118 — Standing Record Quests Expansion: 10 → 20 Territorial-Record Quests (Total Catalog 22 → 32)
> **Status:** COMPLETE
> **Verification Status:** 100% PASS across unit test suite, data integrity gate, content utilization gate, scene binding, Standing Record headless demo, and Godot host build.

---

## 1. Executive Summary

Plan 118 expands ASHFALL's Standing Record territorial quest layer from 10 foundational quests to **20 territorial-record quests** (and expanding the total catalog from 22 to **32 total quests**, preserving the 12 excavation/site-forensics quests intact).

The expansion enriches the territorial-record loop with ten new cases built around survey nails, sector lamps, brass plates, stencil alterations, oil distribution ledgers, disputed boundaries, winter bluffs, rejected surveys, and unlit frontiers. The central thematic question is upheld: *territory in ASHFALL is maintained rather than merely owned—a boundary exists because someone hammered a nail, lit a lamp, stamped a plate, copied a ledger, returned to check it, and decided whether the old mark still counted.*

All 10 new quests conform to strict directed acyclic graph (DAG) invariants, valid entry and terminal states, non-empty world-change completion mutations, authentic failure mutations, multi-faction standing trade-offs, and day pacing spanning Days 80 through 140.

---

## 2. Deliverables Summary

### 2.1 Catalogs & Authority
- **Primary Source:** `Assets/StreamingAssets/Data/standing_record_quests.json`
- **Linux Build Mirror:** `builds/linux/Assets/StreamingAssets/Data/standing_record_quests.json`
- **Master Quest Registry:** `Assets/StreamingAssets/Data/questline_master.json` (and Linux build mirror)
- **Total Quests:** 32 (10 baseline territorial + 12 baseline site forensics + 10 new territorial)
- **All 22 Previous Quests Preserved Verbatim:**
  1. `quest_record_the_plate`
  2. `quest_record_grease_pencil`
  3. `quest_record_wrong_stacks`
  4. `quest_record_the_book`
  5. `quest_record_mass_or_lot`
  6. `quest_record_hands`
  7. `quest_record_friendly_obstacle`
  8. `quest_record_the_failure`
  9. `quest_record_fallback`
  10. `quest_record_which_gazetteer`
  11. `quest_record_vault_breach_forensics`
  12. `quest_record_metro_derailment_triage`
  13. `quest_record_mine_shaft_adit_collapse`
  14. `quest_record_archive_burn_layer`
  15. `quest_record_sluice_failure_verdict`
  16. `quest_record_seed_bank_purge_trace`
  17. `quest_record_sub_basement_blueprint`
  18. `quest_record_transit_vent_shaft_route`
  19. `quest_record_cold_store_sublevel`
  20. `quest_record_utility_junction_crossover`
  21. `quest_record_the_unmarked_plaque`
  22. `quest_record_the_last_watch_beacon`

### 2.2 Ten New Territorial-Record Quests

| Quest ID | Title | Day | Target Location | Prerequisite | Complete Mutation | Fail Mutation |
|---|---|---:|---|---|---|---|
| `quest_record_the_survey_nail` | The Survey Nail | 80 | `loc_cut_kilometre_19` | `quest_record_the_plate` | `mutation_survey_nail_verified` | `mutation_survey_nail_disputed` |
| `quest_record_the_overlay_pigment` | Overlay Pigment | 85 | `loc_cut_kilometre_19` | `quest_record_the_survey_nail` | `mutation_overlay_pigment_exposed` | `mutation_overlay_pigment_sealed` |
| `quest_record_the_second_count` | The Second Count | 95 | `loc_municipal_archive` | `quest_record_the_overlay_pigment` | `mutation_second_count_reconciled` | `mutation_second_count_discrepancy` |
| `quest_record_the_lamp_keepers_oath` | The Keeper's Oath | 90 | `loc_bridge_seven` | `quest_record_the_plate` | `mutation_lamp_keeper_relit` | `mutation_lamp_keeper_retired` |
| `quest_record_the_boundary_dispute` | The Disputed Traverse | 100 | `loc_highway_checkpoint` | `quest_record_the_survey_nail` | `mutation_boundary_dispute_settled` | `mutation_boundary_dispute_unresolved` |
| `quest_record_the_missing_plate` | The Stripped Post | 105 | `loc_abandoned_tide_gauge` | `quest_record_the_overlay_pigment` | `mutation_missing_plate_recovered` | `mutation_missing_plate_recorded_lost` |
| `quest_record_the_cold_survey` | The Ridge Traverse | 110 | `loc_west_ridge_survey` | `quest_record_the_survey_nail` | `mutation_cold_survey_completed` | `mutation_cold_survey_abandoned` |
| `quest_record_the_lamp_oil_ledger` | The Siphoned Drum | 115 | `loc_weighbridge` | `quest_record_the_lamp_keepers_oath` | `mutation_oil_ledger_audited` | `mutation_oil_ledger_suppressed` |
| `quest_record_the_rejected_survey` | The Red Diagonal | 120 | `loc_coastal_meteorological_station` | `quest_record_the_second_count` | `mutation_rejected_survey_resolved` | `mutation_rejected_survey_shelved` |
| `quest_record_the_last_sector` | The Unlit Frontier | 140 | `loc_birchline_weather_station` | `quest_record_the_cold_survey` | `mutation_last_sector_incorporated` | `mutation_last_sector_declared_void` |

---

## 3. Structural Guarantees & Engine Invariants

1. **World-Change Bar:** Every quest defines a non-empty `complete_mutation`. Required and asserted by `StandingRecordHeadlessDemo.cs` and `StandingRecordSystemTests.cs`.
2. **Spatial Bar:** Every quest defines 4 distinct narrative stages (`StageCount >= 3`).
3. **DAG Invariant:** Every prerequisite resolves to an earlier valid quest with `min_day <= child.min_day`. Prerequisite graph contains 0 cycles and 0 self-dependencies.
4. **Choice Density & Invariants:** Every quest provides 2–3 authored choices setting persistent flags with unique choice IDs.
5. **No Engine Modification:** Pure DATA addition in `standing_record_quests.json` and `questline_master.json`; zero changes to C# engine code.

---

## 4. Cross-Domain Integrations

### 4.1 Plan 98 Standing Record Factions Integration (3 Quests)
- **`quest_record_the_boundary_dispute`:** Compact (`faction_the_compact`) vs Garrison (`faction_the_garrison`) arbitration with neutral buffer alternative.
- **`quest_record_the_lamp_oil_ledger`:** Scale (`faction_the_scale`) audit vs Rebuilders (`faction_the_rebuilders`) and Underwrite (`faction_the_underwrite`) emergency heating ratification.
- **`quest_record_the_last_sector`:** Frontier incorporation supported by Compact, Overlay (`faction_the_overlay`), and Underwrite vs dark margin supported by Garrison, Scale, and Cutters (`faction_the_cutters`).

### 4.2 Plan 76 Expedition Destinations Integration (2 Quests)
- **`quest_record_the_cold_survey`:** Targets `loc_west_ridge_survey` (West Ridge Survey Camp), requiring winter theodolite baseline navigation.
- **`quest_record_the_last_sector`:** Targets `loc_birchline_weather_station` (Birchline Weather Station), reaching the northern frontier.

### 4.3 Plan 82 Verdict Locations Integration (2 Quests)
- **`quest_record_the_missing_plate`:** Targets `loc_abandoned_tide_gauge` (Greywater Tide Gauge Station), investigating pried brass plates while respecting Verdict evidence separation.
- **`quest_record_the_rejected_survey`:** Targets `loc_coastal_meteorological_station` (Cape Wrath Meteorological Station), analyzing suppressed coastal subsidence surveys.

---

## 5. Automated Testing Suite

Authored dedicated xUnit regression suite: `Ashfall.Core.Tests/StandingRecordQuestExpansionTests.cs` (10 tests, all passing):

| Test Method | Purpose | Status |
|---|---|---|
| `Catalog_Loads_All_32_Quests` | Verifies full catalog deserialization and minimum count of 32 quests | PASS |
| `All_Quest_Ids_Are_Unique_And_Prefixed` | Asserts all IDs start with `quest_record_` and have zero collisions | PASS |
| `Baseline_10_Territorial_Quests_Preserved_Verbatim` | Asserts original 10 territorial quests are intact | PASS |
| `Deepening_12_Site_Forensics_Quests_Preserved` | Asserts 12 excavation quests are intact | PASS |
| `All_10_New_Plan118_Quests_Present` | Asserts presence and valid field structure for all 10 new quests | PASS |
| `Prerequisite_Graph_Has_No_Cycles_And_All_Prereqs_Resolve` | Cycle detection via DFS recursion stack + resolution verification | PASS |
| `Prerequisite_Day_Ordering_Is_Coherent` | Asserts min_day of dependent quest >= prerequisite min_day | PASS |
| `Every_Quest_Satisfies_World_Change_Bar_And_Spatial_Bar` | Verifies non-empty complete_mutation and StageCount >= 3 on all 32 quests | PASS |
| `Cross_Plan_Integrations_Verified` | Verifies Plan 76, Plan 82, and Plan 98 bindings | PASS |
| `Engine_ApplySiteMutation_Idempotence` | Validates mutation application, state persistence, and idempotence | PASS |

---

## 6. Verification Matrix Evidence

| Verification Step | Command | Exit Code | Result |
|---|---|---|---|
| **Standing Record Expansion Tests** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~StandingRecordQuestExpansionTests` | 0 | 10 passed, 0 failed (114 ms) |
| **All Standing Record Unit Tests** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~StandingRecord` | 0 | 47 passed, 0 failed (239 ms) |
| **Plan 18 Deepening Suite** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~Plan18ExpansionDeepeningTests` | 0 | 6 passed, 0 failed (176 ms) |
| **Standing Record Host Self-Test** | `godot --headless --path . -- --standing-record-selftest` | 0 | PASS (29/29 passed, quests=32) |
| **Data Integrity Gate** | `godot --headless --path . -- --data-integrity-selftest` | 0 | PASS (0 findings, 12,094 IDs across 298 catalogs) |
| **Content Utilization Gate** | `godot --headless --path . -- --content-utilization-selftest` | 0 | CI gate PASS (316 events, 581 catalogs) |
| **Scene Binding Gate** | `godot --headless --path . -- --scene-binding-selftest` | 0 | PASS (25/25 scenes passed) |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | 0 | PASS (30 scenes checked, 0 errors, 0 warnings) |
| **Host Compilation** | `dotnet build Ashfall.csproj` | 0 | PASS (0 errors, 0 warnings) |
| **Full Unit Test Suite** | `dotnet test Ashfall.Core.Tests` | 0 | PASS (9,891 passed, 0 failed) |

---

## 7. Conclusion

Plan 118 is fully implemented, verified, and integrated into the repository baseline. The Standing Record questline catalog now provides a 32-quest territorial investigation arc with grounded material voice, authoritative world-state mutations, and full backward/forward compatibility.
