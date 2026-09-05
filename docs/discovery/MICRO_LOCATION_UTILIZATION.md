# Micro-Location Utilization Report (F11)

Generated deterministically by `MicroLocationUtilizationAuditTests` (set `ASHFALL_GEN_MICRO_REPORTS=1` to regenerate). Values come from the live catalogs.

## Audit configuration

- catalog: micro_locations.json (28 entries)
- simulation: 1000 encounter opportunities, one persistent campaign system (depletion accumulates)
- seeds: opportunity i uses SeededRng(9000+i); destinations cycle the authored expedition catalog
- stance: Stealth (encounter chance ×0.5, parity with ExpeditionSystem.RollEncounter)

## Eligibility-context matrix

| Entry | Eligible contexts | First context |
|---|---:|---|

## 1000-opportunity utilization

| Entry | Category | Weight | Min danger | Required location | Eligible opportunities | Selected | Overall rate | Eligible rate | Status |
|---|---|---:|---:|---|---:|---:|---:|---:|---|
| micro_roadside_memorial | Discovery | 0.8 | 0 | — | 1000 | 8 | 0.8% | 0.8% | OK |
| micro_crashed_truck | Discovery | 0.6 | 1 | — | 1000 | 3 | 0.3% | 0.3% | OK |
| micro_frozen_bus | Discovery | 0.5 | 0 | — | 1000 | 4 | 0.4% | 0.4% | OK |
| micro_improvised_grave | Discovery | 0.7 | 0 | — | 1000 | 2 | 0.2% | 0.2% | OK |
| micro_collapsed_bridge | Hazard | 0.4 | 2 | — | 559 | 1 | 0.1% | 0.2% | OK |
| micro_drainage_pipe | Discovery | 0.7 | 0 | — | 1000 | 4 | 0.4% | 0.4% | OK |
| micro_rail_siding | Discovery | 0.5 | 1 | — | 1000 | 3 | 0.3% | 0.3% | OK |
| micro_dead_livestock | Hazard | 0.6 | 1 | — | 1000 | 5 | 0.5% | 0.5% | OK |
| micro_ruined_greenhouse | Discovery | 0.5 | 0 | — | 1000 | 4 | 0.4% | 0.4% | OK |
| micro_shell_crater | Hazard | 0.4 | 2 | — | 559 | 2 | 0.2% | 0.4% | OK |
| micro_field_kitchen | Discovery | 0.6 | 0 | — | 1000 | 2 | 0.2% | 0.2% | OK |
| micro_abandoned_generator | Discovery | 0.4 | 1 | — | 1000 | 2 | 0.2% | 0.2% | OK |
| micro_shrine | Social | 0.7 | 0 | — | 1000 | 5 | 0.5% | 0.5% | OK |
| micro_emergency_cache | Discovery | 0.2 | 1 | — | 1000 | 1 | 0.1% | 0.1% | OK |
| micro_observation_post | Discovery | 0.3 | 2 | — | 559 | 2 | 0.2% | 0.4% | OK |
| micro_abandoned_barricade | Discovery | 0.7 | 1 | — | 1000 | 7 | 0.7% | 0.7% | OK |
| micro_hunting_blind | Discovery | 0.5 | 0 | — | 1000 | 3 | 0.3% | 0.3% | OK |
| micro_radio_tower | Discovery | 0.3 | 1 | — | 1000 | 2 | 0.2% | 0.2% | OK |
| micro_destroyed_checkpoint | Discovery | 0.5 | 1 | — | 1000 | 3 | 0.3% | 0.3% | OK |
| micro_abandoned_tent | Social | 0.7 | 0 | — | 1000 | 6 | 0.6% | 0.6% | OK |
| micro_makeshift_clinic | Discovery | 0.4 | 1 | — | 1000 | 3 | 0.3% | 0.3% | OK |
| micro_crashed_drone | Discovery | 0.2 | 2 | — | 559 | 0 | 0.0% | 0.0% | ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE |
| micro_fuel_cache | Discovery | 0.2 | 1 | — | 1000 | 2 | 0.2% | 0.2% | OK |
| micro_water_source | Discovery | 0.5 | 0 | — | 1000 | 3 | 0.3% | 0.3% | OK |
| micro_supply_drop | Discovery | 0.1 | 2 | — | 559 | 1 | 0.1% | 0.2% | OK |
| micro_hospital_chapel_ledger | Discovery | 0.9 | 0 | abandoned_hospital | 8 | 0 | 0.0% | 0.0% | ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE |
| micro_depot_undertow_raft_line | Discovery | 0.7 | 0 | location_flooded_subway_depot | 8 | 0 | 0.0% | 0.0% | ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE |
| micro_gamma_levy_board | Discovery | 0.8 | 0 | loc_garrison_checkpoint_gamma | 8 | 0 | 0.0% | 0.0% | ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE |

## Findings

- micro_crashed_drone: ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE
- micro_hospital_chapel_ledger: ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE
- micro_depot_undertow_raft_line: ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE
- micro_gamma_levy_board: ELIGIBLE_BUT_NOT_SELECTED_IN_SAMPLE

## Redundancy review pairs

- none above threshold

## Simulation canonical trace

```
opps=1000;triggered=78;none=0;micro_abandoned_barricade:e=1000,s=7;micro_abandoned_generator:e=1000,s=2;micro_abandoned_tent:e=1000,s=6;micro_collapsed_bridge:e=559,s=1;micro_crashed_drone:e=559,s=0;micro_crashed_truck:e=1000,s=3;micro_dead_livestock:e=1000,s=5;micro_depot_undertow_raft_line:e=8,s=0;micro_destroyed_checkpoint:e=1000,s=3;micro_drainage_pipe:e=1000,s=4;micro_emergency_cache:e=1000,s=1;micro_field_kitchen:e=1000,s=2;micro_frozen_bus:e=1000,s=4;micro_fuel_cache:e=1000,s=2;micro_gamma_levy_board:e=8,s=0;micro_hospital_chapel_ledger:e=8,s=0;micro_hunting_blind:e=1000,s=3;micro_improvised_grave:e=1000,s=2;micro_makeshift_clinic:e=1000,s=3;micro_observation_post:e=559,s=2;micro_radio_tower:e=1000,s=2;micro_rail_siding:e=1000,s=3;micro_roadside_memorial:e=1000,s=8;micro_ruined_greenhouse:e=1000,s=4;micro_shell_crater:e=559,s=2;micro_shrine:e=1000,s=5;micro_supply_drop:e=559,s=1;micro_water_source:e=1000,s=3
```
