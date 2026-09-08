# Plan 121: Independent Faction Branch Expansion (8 → 15) — Completion Report

## 1. Executive Summary
- **Plan:** Plan 121 — Independent Faction Branch Expansion (8 → 15 branches)
- **Status:** **COMPLETE** (All 15 branches active, all tests passing, CI gates green)
- **Primary Data Catalog:** `Assets/StreamingAssets/Data/independent_faction_branch.json`
- **Build Mirror:** `builds/linux/Assets/StreamingAssets/Data/independent_faction_branch.json`
- **Core Integration:** `Assets/Ashfall.Core/Factions/IndependentBranchIds.cs`
- **Catalog Count:** Expanded from 8 to 15 branches (24 to 45 endings).
- **Baseline Preserved:** `branch_ind_1` through `branch_ind_8` preserved byte-for-byte in positions 0..7.

---

## 2. Seven Authored Branches Summary

1. **`branch_ind_9_hermit` (The Hermit):**
   - PoNR: `flag_branch_ind_9_ponr`
   - Trigger: *"They turn away the final delegation and commit to living beyond every settlement's protection."*
   - Entry: `very_evil` .. `very_positive`
   - Endings:
     - `ending_ind_9a_quiet_holding` (`neutral`..`slightly_positive`, "The Quiet Holding")
     - `ending_ind_9b_unlatched_door` (`positive`..`very_positive`, "The Unlatched Door")
     - `ending_ind_9c_no_smoke` (`very_evil`..`slightly_evil`, "No Smoke on the Ridge")

2. **`branch_ind_10_mediator` (The Mediator):**
   - PoNR: `flag_branch_ind_10_ponr`
   - Trigger: *"They sign their name to a negotiated partition that sacrifices one outpost to preserve the peace of three others."*
   - Entry: `slightly_evil` .. `very_positive`
   - Endings:
     - `ending_ind_10a_open_table` (`positive`..`very_positive`, "The Open Table")
     - `ending_ind_10b_necessary_liar` (`neutral`..`slightly_positive`, "The Necessary Liar")
     - `ending_ind_10c_no_safe_chair` (`very_evil`..`slightly_evil`, "No Safe Chair")

3. **`branch_ind_11_scavenger_king` (The Scavenger King):**
   - PoNR: `flag_branch_ind_11_ponr`
   - Trigger: *"They seal their primary supply cache against communal commandeering, declaring that access requires individual barter contracts."*
   - Entry: `very_evil` .. `slightly_positive`
   - Endings:
     - `ending_ind_11a_quartermaster` (`positive`..`very_positive`, "Quartermaster Without a Flag")
     - `ending_ind_11b_locked_rooms` (`neutral`..`slightly_positive`, "The Locked Rooms")
     - `ending_ind_11c_dead_mans_inventory` (`very_evil`..`slightly_evil`, "Inventory of a Dead Man")

4. **`branch_ind_12_caretaker` (The Caretaker):**
   - PoNR: `flag_branch_ind_12_ponr`
   - Trigger: *"They defy the exclusion order and bring three contaminated orphans into their own quarters under personal bond."*
   - Entry: `neutral` .. `very_positive`
   - Endings:
     - `ending_ind_12a_extra_chairs` (`positive`..`very_positive`, "House With Extra Chairs")
     - `ending_ind_12b_hands_full` (`neutral`..`slightly_positive`, "Hands Full")
     - `ending_ind_12c_broken_promise` (`very_evil`..`slightly_evil`, "The Promise That Broke")

5. **`branch_ind_13_witness` (The Witness):**
   - PoNR: `flag_branch_ind_13_ponr`
   - Trigger: *"They post the unredacted ration ledger on the commons wall, exposing the rationing council's hidden diversion."*
   - Entry: `slightly_evil` .. `very_positive`
   - Endings:
     - `ending_ind_13a_record_stands` (`positive`..`very_positive`, "The Record Stands")
     - `ending_ind_13b_margin_notes` (`neutral`..`slightly_positive`, "Annotations in the Margin")
     - `ending_ind_13c_missing_pages` (`very_evil`..`slightly_evil`, "Missing Pages")

6. **`branch_ind_14_engineer` (The Engineer):**
   - PoNR: `flag_branch_ind_14_ponr`
   - Trigger: *"They cut auxiliary steam to the lower barracks to ensure the main generator core survives the blizzard."*
   - Entry: `very_evil` .. `very_positive`
   - Endings:
     - `ending_ind_14a_load_bearing` (`positive`..`very_positive`, "Load-Bearing")
     - `ending_ind_14b_necessary_failure` (`neutral`..`slightly_positive`, "Necessary Failure")
     - `ending_ind_14c_name_on_breakdown` (`very_evil`..`slightly_evil`, "The Name on the Breakdown")

7. **`branch_ind_15_prophet` (The Prophet):**
   - PoNR: `flag_branch_ind_15_ponr`
   - Trigger: *"They climb the water tower during the black rain and proclaim the storm a judgment that demands public penance."*
   - Entry: `very_evil` .. `very_positive`
   - Endings:
     - `ending_ind_15a_keeper_of_vigils` (`positive`..`very_positive`, "Keeper of Vigils")
     - `ending_ind_15b_voice_in_the_hall` (`neutral`..`slightly_positive`, "The Voice in the Hall")
     - `ending_ind_15c_ash_gospel` (`very_evil`..`slightly_evil`, "Ash Gospel")

---

## 3. Verification Evidence

| Step | Gate / Tool | Result |
|---|---|---|
| 1 | `dotnet build Ashfall.csproj` | **PASS (0 errors, 0 warnings)** |
| 2 | `python3 scripts/ci/scene-lint.py` | **PASS (30 scenes, 0 errors, 0 warnings)** |
| 3 | `godot --headless --path . -- --data-integrity-selftest` | **PASS (0 findings across 298 catalogs; 12,286 authored IDs)** |
| 4 | `godot --headless --path . -- --content-utilization-selftest` | **PASS (CI Content Utilization Gate: PASS)** |
| 5 | `godot --headless --path . -- --scene-binding-selftest` | **PASS (25/25 passed, 0 failed)** |
| 6 | `dotnet test Ashfall.Core.Tests --filter IndependentBranch` | **PASS (45/45 passed)** |
| 7 | `dotnet test Ashfall.Core.Tests --filter FactionBranchCoordinator` | **PASS (17/17 passed)** |
| 8 | `dotnet test Ashfall.Core.Tests` | **PASS (9,949/9,949 passed, 0 failed)** |
| 9 | `python3 scripts/ci/generate-asset-registry.py` | **PASS (0 missing assets)** |
