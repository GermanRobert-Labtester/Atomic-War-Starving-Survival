# Micro-Location Balance Report (F12)

Generated deterministically by `MicroLocationEconomyAuditTests` (`ASHFALL_GEN_MICRO_REPORTS=1` to regenerate). Item value uses live `tradeValue`; morale/guilt/journal/discovery are reported separately — no exchange rate is defined.

## Methodology

- data: live catalogs (micro_locations.json, items.json, expeditions.json, scavenging tables)
- primary baseline: production loot through ExpeditionSystem.TickHours → ScavengingTableCatalog.RollLoot
- micro rewards: greedy max-net-value choice on every surfaced micro-location (upper bound)
- simulation: 100 expeditions, sortie i on destination i%N, seed 4000+i, Stealth stance

## Per-choice reward ledger

| Encounter | Choice | Depletes | Granted | Consumed | Net item | Morale | Guilt | Journal | Location |
|---|---|---|---:|---:|---:|---:|---:|---|---|
| micro_roadside_memorial | leave_memorial | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_roadside_memorial | take_offering | yes | 1.2 | 0 | 1.2 | -1 | +2 | — | — |
| micro_crashed_truck | search_truck_cargo | yes | 24 | 0 | 24 | +1 | +0 | — | — |
| micro_crashed_truck | search_truck_cab | yes | 11 | 0 | 11 | +0 | +1 | — | — |
| micro_crashed_truck | ignore_truck | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_frozen_bus | search_bus_luggage | yes | 20 | 0 | 20 | -2 | +3 | — | — |
| micro_frozen_bus | leave_bus | no | 0 | 0 | 0 | +2 | +0 | — | — |
| micro_frozen_bus | read_bus_tag | no | 0 | 0 | 0 | +0 | +0 | micro_frozen_bus_transit_tag | — |
| micro_improvised_grave | respect_grave | no | 0 | 0 | 0 | +2 | +0 | — | — |
| micro_improvised_grave | inspect_grave_marker | no | 0 | 0 | 0 | +0 | +0 | micro_improvised_grave_marker | — |
| micro_improvised_grave | disturb_grave | yes | 25 | 0 | 25 | -3 | +4 | — | — |
| micro_collapsed_bridge | search_bridge_vehicle | yes | 28 | 0 | 28 | +0 | +0 | — | — |
| micro_collapsed_bridge | inspect_bridge_structure | yes | 3.6 | 0 | 3.6 | +0 | +0 | — | — |
| micro_collapsed_bridge | avoid_bridge | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_drainage_pipe | crawl_pipe | yes | 2.4 | 0 | 2.4 | +0 | +1 | — | — |
| micro_drainage_pipe | read_pipe_warning | no | 0 | 0 | 0 | -1 | +0 | micro_drainage_pipe_warning | — |
| micro_drainage_pipe | ignore_pipe | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_rail_siding | search_rail_car | yes | 9 | 0 | 9 | +0 | +0 | — | — |
| micro_rail_siding | read_rail_ledger | no | 0 | 0 | 0 | +0 | +0 | micro_rail_siding_ledger | — |
| micro_rail_siding | ignore_rail | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_dead_livestock | scavenge_livestock | yes | 2.4 | 0 | 2.4 | -2 | +1 | — | — |
| micro_dead_livestock | inspect_livestock_tags | no | 0 | 0 | 0 | +0 | +0 | micro_dead_livestock_tags | — |
| micro_dead_livestock | avoid_livestock | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_ruined_greenhouse | take_greenhouse_seeds | yes | 12 | 0 | 12 | +1 | +0 | — | — |
| micro_ruined_greenhouse | open_greenhouse_cabinet | yes | 14 | 0 | 14 | +0 | +0 | — | — |
| micro_ruined_greenhouse | leave_greenhouse | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_shell_crater | inspect_crater | yes | 2.4 | 0 | 2.4 | +0 | +0 | — | — |
| micro_shell_crater | salvage_crater_harness | yes | 3 | 0 | 3 | +0 | +0 | — | — |
| micro_shell_crater | avoid_crater | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_field_kitchen | search_kitchen | yes | 12 | 0 | 12 | +1 | +0 | — | — |
| micro_field_kitchen | take_kitchen_tools | yes | 14 | 0 | 14 | +0 | +0 | — | — |
| micro_field_kitchen | read_ration_marks | no | 0 | 0 | 0 | +0 | +0 | micro_field_kitchen_marks | — |
| micro_abandoned_generator | strip_generator | yes | 18 | 0 | 18 | +0 | +0 | — | — |
| micro_abandoned_generator | read_generator_notes | no | 0 | 0 | 0 | +0 | +0 | micro_generator_fuel_notes | — |
| micro_abandoned_generator | mark_generator | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_shrine | leave_shrine | no | 0 | 0 | 0 | +2 | +0 | — | — |
| micro_shrine | take_shrine_offerings | yes | 50 | 0 | 50 | -2 | +3 | — | — |
| micro_shrine | add_shrine_offering | no | 0 | 12 | -12 | +3 | +0 | — | — |
| micro_emergency_cache | open_cache | yes | 10 | 0 | 10 | +2 | +0 | — | — |
| micro_emergency_cache | leave_cache | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_observation_post | search_observation_post | yes | 30 | 0 | 30 | +0 | +0 | — | — |
| micro_observation_post | read_grid_references | no | 0 | 0 | 0 | +0 | +0 | micro_observation_post_grid | rural_gas_station |
| micro_observation_post | ignore_observation_post | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_abandoned_barricade | search_barricade | yes | 10 | 0 | 10 | +0 | +1 | — | — |
| micro_abandoned_barricade | read_barricade_markings | no | 0 | 0 | 0 | +0 | +0 | micro_barricade_markings | — |
| micro_abandoned_barricade | avoid_barricade | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_hunting_blind | search_blind | yes | 10 | 0 | 10 | +0 | +0 | — | — |
| micro_hunting_blind | read_blind_journal | no | 0 | 0 | 0 | +0 | +0 | micro_hunting_blind_journal | — |
| micro_hunting_blind | leave_blind | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_radio_tower | open_radio_cabinet | yes | 10 | 0 | 10 | +0 | +0 | — | — |
| micro_radio_tower | read_radio_log | no | 0 | 0 | 0 | +0 | +0 | micro_radio_tower_log | — |
| micro_radio_tower | ignore_radio | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_destroyed_checkpoint | search_checkpoint | yes | 24 | 0 | 24 | +0 | +1 | — | — |
| micro_destroyed_checkpoint | read_checkpoint_log | no | 0 | 0 | 0 | +0 | +0 | micro_checkpoint_log | — |
| micro_destroyed_checkpoint | avoid_checkpoint | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_abandoned_tent | search_tent | yes | 2.4 | 0 | 2.4 | -1 | +2 | — | — |
| micro_abandoned_tent | take_drawing | yes | 3 | 0 | 3 | -1 | +1 | — | — |
| micro_abandoned_tent | leave_tent | no | 0 | 0 | 0 | +2 | +0 | — | — |
| micro_makeshift_clinic | search_clinic | yes | 30 | 0 | 30 | +1 | +0 | — | — |
| micro_makeshift_clinic | read_triage_list | no | 0 | 0 | 0 | -1 | +0 | micro_clinic_triage | — |
| micro_makeshift_clinic | leave_clinic | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_crashed_drone | open_drone_compartment | yes | 24 | 0 | 24 | +0 | +0 | — | — |
| micro_crashed_drone | read_drone_log | no | 0 | 0 | 0 | +0 | +0 | micro_drone_flight_log | — |
| micro_crashed_drone | avoid_drone | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_fuel_cache | take_fuel_cache | yes | 56 | 0 | 56 | +2 | +0 | — | — |
| micro_fuel_cache | read_route_sketch | no | 0 | 0 | 0 | +0 | +0 | micro_fuel_cache_route | — |
| micro_fuel_cache | leave_fuel_cache | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_water_source | collect_water | yes | 45 | 0 | 45 | +2 | +0 | — | — |
| micro_water_source | test_water | yes | 30 | 0 | 30 | +0 | +0 | — | — |
| micro_water_source | avoid_water | no | 0 | 0 | 0 | +0 | +0 | — | — |
| micro_supply_drop | open_supply_drop | yes | 20 | 0 | 20 | +3 | +0 | — | — |
| micro_supply_drop | read_supply_label | no | 0 | 0 | 0 | +0 | +0 | micro_supply_drop_label | government_bunker |
| micro_supply_drop | leave_supply_drop | no | 0 | 0 | 0 | +2 | +0 | — | — |
| micro_hospital_chapel_ledger | read_the_names | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_hospital_chapel_ledger | take_matches | yes | 6 | 0 | 6 | -1 | +1 | — | — |
| micro_depot_undertow_raft_line | note_the_route | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_depot_undertow_raft_line | cut_one_crate | yes | 2.4 | 0 | 2.4 | -1 | +2 | — | — |
| micro_gamma_levy_board | memorize_the_board | no | 0 | 0 | 0 | +1 | +0 | — | — |
| micro_gamma_levy_board | take_the_chalk | yes | 1.2 | 0 | 1.2 | +0 | +2 | — | — |

## 100-expedition results

- mean primary loot value / expedition: 7.69
- mean micro item value / expedition: 1.5
- micro/primary contribution ratio: 19.5% — target band 10–30%
- median micro value: 0; p95: 14
- median primary value: 0; p95: 51
- expeditions completed / failed: 24 / 0
- non-item rewards across the run: 0 journal unlocks, 0 location discoveries, morale -3, guilt +6
- micro encounters surfaced: 8

## Named outlier reviews

- **micro_supply_drop** — 2 × medical_kit (face 20), depleting, minDanger 2, military route affinity: expected value is P(selected) × 20; one-shot, so face value is not per-expedition income. Qualitative utility of medical kits exceeds tradeValue — monitor, no change.
- **micro_roadside_memorial** — take_offering grants 1 cloth (1.2), depleting; leave_memorial +1 morale: the encounter's purpose is narrative texture; cloth is incidental. No change.
- **micro_improvised_grave** — disturb_grave: +25 wedding_ring for −3 morale, +4 guilt, one-shot: guilt is persistent (morale-coupled) and the site is single-use; the tradeoff reads as intended. No change.
- **micro_shrine** — add_shrine_offering: repeatable, costs 1 canned_food for +3 morale: morale farming is capped by canned-food scarcity (deliberate resource sink). Monitor.

## Farming resistance

- every positive-granting choice depletes its encounter (static gate test)
- depleting encounters are never re-selected after resolution (production selector, 64 seeds each)
- depletion persists across save/reload (F9 suite); reload cannot refill a searched site
- non-depleting choices are offerings (net item cost) or non-item rewards — no unbounded item loop

## Recommendation

- ratio measured at 19.5% — inside the 10–30% design band: no change.
