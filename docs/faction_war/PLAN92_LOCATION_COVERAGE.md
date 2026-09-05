# Plan 92 — Location Coverage Matrix

> **Scope:** Faction War Overheard Dialogue Distribution
> **Rule:** Every locationId must resolve against canonical location catalogs (`locations.json` or `year_of_ash_locations.json`).

---

## 1. Location Coverage Roster

| # | Location ID | Display Name | Source Catalog | Primary Faction / Zone Context | Snippet Count | Snippets Assigned |
|---|---|---|---|---|---|---|
| 1 | `loc_grain_silo` | The Grain Exchange | `locations.json` | Exchange / Neutral Trade Hub | 4 | `dlg_d483_exchange_lean_pool`, `dlg_d526_exchange_roster_kid`, `dlg_d538_checkpoint_awkward_small_talk`, `dlg_d485_exchange_wet_grain_scale` |
| 2 | `loc_garrison_checkpoint_gamma` | Checkpoint Gamma | `locations.json` | Central Garrison Military Cordon | 3 | `dlg_d482_checkpoint_quartermasters`, `dlg_d552_deserter_hunters`, `dlg_d516_garrison_kerosene_stove` |
| 3 | `loc_weighbridge` | The Weighbridge | `locations.json` | Toll Road / Commercial Transit | 4 | `dlg_d493_weighbridge_toll_grumble`, `dlg_d512_weighbridge_reroute`, `dlg_d568_toll_syndicate_cynicism`, `dlg_d508_exchange_axle_grease_delay` |
| 4 | `loc_understory_transmitter` | The Understory Relay | `locations.json` | Understory Radio / Communications | 2 | `dlg_d488_understory_relay_move`, `dlg_d492_understory_porcelain_insulator` |
| 5 | `loc_ash_sign_shrine` | The Ash Sign Shrine | `locations.json` | Cult of the Ash Sign / Pilgrims | 2 | `dlg_d490_switchback_pilgrims`, `dlg_d580_shrine_keepers_doubt` |
| 6 | `loc_railway_span_44_alpha` | Railway Span 44-Alpha | `locations.json` | Contested Railway / Scavengers | 1 | `dlg_d497_scavengers_clean_crater` |
| 7 | `loc_conscription_office` | Conscription Office | `locations.json` | Garrison / Administration | 2 | `dlg_d505_conscription_office_clerks`, `dlg_d542_garrison_sick_list_billet` |
| 8 | `loc_ration_queue_plaza` | Ration Queue Plaza | `locations.json` | Civilian / Food Distribution | 2 | `dlg_d549_children_after_the_plaza`, `dlg_d520_civilian_valve_handle_toy` |
| 9 | `loc_forward_roster_camp` | Forward Roster Camp | `locations.json` | Forward Roster / Independent | 3 | `dlg_d571_forward_roster_checkpoint`, `dlg_d573_forward_roster_identity`, `dlg_d534_independent_tripwire_slack` |
| 10 | `loc_d9_cache_bunker_delta` | D/9 Denial Armory Cache | `year_of_ash_locations.json` | Denial Detachment 9 / Black Ops | 1 | `dlg_d584_d9_cell_debate` |
| 11 | `loc_shrine_switchback_waystation` | Switchback Waystation | `locations.json` | Pilgrim Route / Waystation | 1 | `dlg_d591_switchback_waystation_doubt` |
| 12 | `loc_garrison_motor_pool` | Garrison Motor Pool | `year_of_ash_locations.json` | Garrison Heavy Logistics | 2 | `dlg_d486_garrison_crate_seal`, `dlg_d562_garrison_fuel_drum_tare` |
| 13 | `loc_the_allotments` | The Works Allotment Commune | `year_of_ash_locations.json` | Civilian Agriculture / Greenhouse | 1 | `dlg_d487_civilian_parsnip_stew_scrap` |
| 14 | `loc_water_station` | Water Station | `locations.json` | Civilian / Resource Infrastructure | 1 | `dlg_d489_exchange_drum_bung_dispute` |
| 15 | `loc_snowline_station` | Snowline Patrol Station | `locations.json` | Garrison High Altitude Watch | 1 | `dlg_d494_garrison_boot_leather` |
| 16 | `loc_sector_4_rail_switchyard` | Sector 4 Freight Switchyard | `year_of_ash_locations.json` | Independent Scavengers / Rail | 1 | `dlg_d498_independent_chalk_boundary` |
| 17 | `loc_granite_arsenal_foundry` | Granite Munitions Foundry | `year_of_ash_locations.json` | Heavy Industry / Foundry Guild | 2 | `dlg_d502_foundry_cracked_flask_sand`, `dlg_d528_foundry_crucible_heat_window` |
| 18 | `loc_radio_relay_mast` | Relay Mast 12 | `locations.json` | High Communications Hub | 1 | `dlg_d518_understory_log_overrun` |
| 19 | `loc_supply_corps_highway_redoubt` | Logistics Highway Redoubt | `year_of_ash_locations.json` | Interstate Highway / Toll Gate | 1 | `dlg_d530_exchange_stamped_chits` |
| 20 | `loc_sub_level_maintenance_shaft_9` | Sub-Level Maintenance Shaft 9 | `year_of_ash_locations.json` | Understory Run / Municipal Sump | 1 | `dlg_d546_understory_smudged_pad_entry` |
| 21 | `loc_rebuilder_brickworks_kiln` | The Works Brickworks Kiln | `year_of_ash_locations.json` | Rebuilders / Material Smelting | 1 | `dlg_d556_foundry_slag_billet_reject` |
| 22 | `loc_mountain_tunnel_refuge` | High Alpine Tunnel Refuge | `year_of_ash_locations.json` | Independent Civilian Settlement | 1 | `dlg_d566_independent_blanket_tally` |
| 23 | `loc_second_winter_homestead` | Second Winter Homestead | `year_of_ash_locations.json` | Civilian Hinterland Refuge | 1 | `dlg_d574_civilian_kettle_scouring_mutter` |
| 24 | `loc_continental_radio_beacon` | High Granite Relay Mast | `year_of_ash_locations.json` | High Altitude Communications | 1 | `dlg_d576_understory_copper_splice_tale` |

---

## 2. Spatial Balance Summary
- **Total Locations Utilized:** 24 distinct locations across Sector 4, Sector 8, and the High Ridge.
- **Top Commercial Hubs:** The Grain Exchange (4 snippets), The Weighbridge (4 snippets), Checkpoint Gamma (3 snippets), Forward Roster Camp (3 snippets).
- **Zero Orphan Locations:** Every location is a verified node present in the core location registries.
