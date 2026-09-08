# Independent Branch Ending Truth Table (New Branches 9–15)

| Branch | Band 0 (`very_evil`) | Band 1 (`evil`) | Band 2 (`slightly_evil`) | Band 3 (`neutral`) | Band 4 (`slightly_positive`) | Band 5 (`positive`) | Band 6 (`very_positive`) |
|---|---|---|---|---|---|---|---|
| `branch_ind_9_hermit` | `ending_ind_9c_no_smoke` | `ending_ind_9c_no_smoke` | `ending_ind_9c_no_smoke` | `ending_ind_9a_quiet_holding` | `ending_ind_9a_quiet_holding` | `ending_ind_9b_unlatched_door` | `ending_ind_9b_unlatched_door` |
| `branch_ind_10_mediator` | `ending_ind_10c_no_safe_chair` | `ending_ind_10c_no_safe_chair` | `ending_ind_10c_no_safe_chair` | `ending_ind_10b_necessary_liar` | `ending_ind_10b_necessary_liar` | `ending_ind_10a_open_table` | `ending_ind_10a_open_table` |
| `branch_ind_11_scavenger_king` | `ending_ind_11c_dead_mans_inventory` | `ending_ind_11c_dead_mans_inventory` | `ending_ind_11c_dead_mans_inventory` | `ending_ind_11b_locked_rooms` | `ending_ind_11b_locked_rooms` | `ending_ind_11a_quartermaster` | `ending_ind_11a_quartermaster` |
| `branch_ind_12_caretaker` | `ending_ind_12c_broken_promise` | `ending_ind_12c_broken_promise` | `ending_ind_12c_broken_promise` | `ending_ind_12b_hands_full` | `ending_ind_12b_hands_full` | `ending_ind_12a_extra_chairs` | `ending_ind_12a_extra_chairs` |
| `branch_ind_13_witness` | `ending_ind_13c_missing_pages` | `ending_ind_13c_missing_pages` | `ending_ind_13c_missing_pages` | `ending_ind_13b_margin_notes` | `ending_ind_13b_margin_notes` | `ending_ind_13a_record_stands` | `ending_ind_13a_record_stands` |
| `branch_ind_14_engineer` | `ending_ind_14c_name_on_breakdown` | `ending_ind_14c_name_on_breakdown` | `ending_ind_14c_name_on_breakdown` | `ending_ind_14b_necessary_failure` | `ending_ind_14b_necessary_failure` | `ending_ind_14a_load_bearing` | `ending_ind_14a_load_bearing` |
| `branch_ind_15_prophet` | `ending_ind_15c_ash_gospel` | `ending_ind_15c_ash_gospel` | `ending_ind_15c_ash_gospel` | `ending_ind_15b_voice_in_the_hall` | `ending_ind_15b_voice_in_the_hall` | `ending_ind_15a_keeper_of_vigils` | `ending_ind_15a_keeper_of_vigils` |

## Resolution Properties
1. **Total Coverage:** 100% of all valid `MoralPathBand` values resolve deterministically.
2. **Zero Gaps:** No band falls through to fallback logic.
3. **Zero Overlaps:** Range partitions are mutually exclusive.
4. **Verified by Test:** `Ashfall.Core.Tests.IndependentBranchExpansionTests.NewSevenBranches_HaveCompleteDeterministicSevenBandPartition` and `ExhaustiveResolution_AllFifteenBranchesResolveValidEndingAtEveryBand`.
