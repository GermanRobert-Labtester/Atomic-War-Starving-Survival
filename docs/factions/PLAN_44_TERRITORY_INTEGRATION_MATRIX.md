# PLAN 44 TERRITORY INTEGRATION MATRIX

---

## 1. 19 Factions Classification & Territorial Profiles

| Faction ID | Display Name | Classification | Scale | Control Strength | Trade Tax | Travel Safety | Primary Anchors | Rivals |
|---|---|---|---|---|---|---|---|---|
| `faction_the_office` | The Office | Territorial | Major | 85 | 8% | 85% | `loc_settlement_nine_rails`, `loc_weighbridge` | `faction_the_tally`, `faction_scavenger_guild` |
| `faction_the_cutters` | The Cutters | Territorial | Medium | 70 | 5% | 75% | `loc_settlement_brine_pans`, `loc_the_shallows_market` | `faction_undertow`, `faction_black_flotilla` |
| `faction_black_flotilla` | The Black Flotilla | Mixed | Medium | 80 | 10% | 70% | `loc_settlement_cape_beacon`, `loc_black_flotilla_outpost` | `faction_the_fleet`, `faction_the_cutters` |
| `faction_the_fleet` | The Fleet | Nomadic | Minor | 60 | 5% | 80% | `loc_black_flotilla_outpost`, `loc_lock_gate_four` | `faction_black_flotilla`, `faction_undertow` |
| `faction_deserter_coalition` | The Deserter Coalition | Territorial | Major | 90 | 0% | 90% | `loc_settlement_iron_siding`, `loc_settlement_fort_karkov` | `faction_iron_raiders`, `faction_the_provisioned` |
| `faction_cold_count` | The Cold Count | Territorial | Minor | 65 | 0% | 95% | `loc_settlement_slate_hollow`, `loc_low_background_lab` | `faction_the_tally`, `faction_long_walk` |
| `faction_the_tally` | The Tally | Territorial | Medium | 75 | 12% | 65% | `loc_settlement_lock_seven`, `loc_lock_gate_four` | `faction_undertow`, `faction_the_office` |
| `faction_grain_exchange` | The Grain Exchange | Territorial | Medium | 70 | 2% | 85% | `loc_settlement_silo_burrow`, `loc_grain_silo` | `faction_iron_raiders`, `faction_the_tally` |
| `faction_quiet_house` | The Quiet House | Ideological | Minor | 50 | 0% | 95% | `loc_settlement_st_nicholas`, `loc_shrine_switchback_waystation` | `faction_osteophages`, `faction_iron_raiders` |
| `faction_scavenger_guild` | The Scavenger Guild | Territorial | Major | 75 | 3% | 80% | `loc_settlement_tinkers_notch`, `loc_cut_merchant_caravanserai` | `faction_iron_raiders`, `faction_the_office` |
| `faction_long_walk` | The Long Walk | Nomadic | Minor | 45 | 0% | 90% | `loc_settlement_pilgrim_hearth`, `loc_shrine_switchback_waystation` | `faction_sun_seekers`, `faction_cold_count` |
| `faction_undertow` | The Undertow | Territorial | Medium | 65 | 6% | 70% | `loc_settlement_ferry_crossing`, `loc_water_station` | `faction_the_cutters`, `faction_the_tally` |
| `faction_hydro_barons` | The Coastal Hydro-Barons | Territorial | Major | 80 | 15% | 75% | `loc_water_station`, `loc_terrace_pumphouse` | `faction_undertow`, `faction_grain_exchange` |
| `faction_iron_raiders` | The Iron Raiders | Nomadic | Medium | 60 | 20% | 30% | `loc_cut_abandoned_depot`, `loc_cut_arsenal_ruin` | `faction_scavenger_guild`, `faction_deserter_coalition` |
| `faction_the_provisioned` | The Provisioned | Territorial | Minor | 90 | 0% | 95% | `loc_excavation_command_vault`, `loc_logistics_reserve_cache` | `faction_deserter_coalition`, `faction_scavenger_guild` |
| `faction_archivists` | The Archivists of the Before | Ideological | None | 40 | 0% | 90% | `loc_excavation_archive_bunker`, `loc_hidden_relay_bunker` | `faction_the_office`, `faction_the_tally` |
| `faction_lamplighters` | The Lamplighters | Nomadic | Minor | 50 | 0% | 90% | `loc_cut_merchant_caravanserai`, `loc_cut_abandoned_depot` | `faction_iron_raiders`, `faction_undertow` |
| `faction_sun_seekers` | The Sun-Seekers | Nomadic | None | 40 | 0% | 85% | `loc_cut_radiation_zone_alpha`, `loc_broadcast_bunker_echo` | `faction_long_walk`, `faction_osteophages` |
| `faction_osteophages` | The Osteophages | Territorial | Minor | 55 | 0% | 40% | `loc_cut_radiation_zone_alpha`, `loc_excavation_mine_shaft` | `faction_quiet_house`, `faction_the_cutters` |

---

## 2. Integration Seams & Downstream Roadmap

| System | Field Consumed | Current Status | Integration Target |
|---|---|---|---|
| **Faction Patrols** | `control_strength`, `control_points`, `travel_safety` | Data ready | Plan 45 (Faction Patrol Encounters) |
| **Caravan Economy** | `trade_tax`, `controlled_nodes`, `faction` | Data ready | Plan 16B (Caravan Economy & World Routes) |
| **Faction War** | `contested_zones`, `claimant_factions`, `shift_trigger` | Data ready | Plan 06C (Faction War Chains) |
| **Debt Dispatcher** | `territory_id`, `control_strength` | Data ready | Plan 40 (Debt Consequence Dispatcher) |
| **Settlements** | `control_points`, `faction` | Data ready / Aligned | Plan 43 (Settlements Catalog) |
| **Dynamic World Map**| `controlled_nodes`, `contested_zones` | Data ready | Plan 11 (Living Geography Overlays) |
