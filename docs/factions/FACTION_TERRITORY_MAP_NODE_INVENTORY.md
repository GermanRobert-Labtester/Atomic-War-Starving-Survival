# FACTION TERRITORY MAP NODE & CONTROL POINT INVENTORY (Plan 44)

---

## 1. Active Map Nodes (`wasteland_map_v1.json`)

All node references in `faction_territory.json` strictly resolve against active map nodes. Zero uncommitted future IDs are referenced in production data.

| Map Node ID | Display Name | Region | Associated Controlling Territories |
|---|---|---|---|
| `loc_holdfast` | The Holdfast Shelter | Valley Basin | `territory_hydro_barons` |
| `loc_cut_abandoned_depot` | Abandoned Depot | The Cut | `territory_deserter_coalition`, `territory_undertow`, `territory_iron_raiders` |
| `loc_cut_radiation_zone_alpha` | Radiation Zone Alpha | The Cut / Flats | `territory_the_cutters`, `territory_the_tally`, `territory_sun_seekers`, `territory_osteophages` |
| `loc_black_flotilla_outpost` | Black Flotilla Outpost | Coastal Shelf | `territory_black_flotilla`, `territory_the_fleet`, `territory_quiet_house` |
| `loc_cut_merchant_caravanserai` | Merchant Caravanserai | Crossroads | `territory_grain_exchange`, `territory_scavenger_guild`, `territory_long_walk`, `territory_lamplighters` |
| `loc_cut_arsenal_ruin` | Arsenal Ruin | Industrial Spur | `territory_the_office`, `territory_cold_count`, `territory_iron_raiders` |
| `loc_hidden_relay_bunker` | Hidden Relay Bunker | High Scarp | `territory_the_provisioned` |
| `loc_logistics_reserve_cache` | Logistics Reserve Cache | Foothills | `territory_archivists` |
| `loc_broadcast_bunker_echo` | Broadcast Bunker Echo | Ridge Pass | *Nomadic / Range Overlay* |

---

## 2. Active Physical Control Points (`locations.json`)

| Location ID | Name | Controlling Territory | Strategic Purpose |
|---|---|---|---|
| `loc_settlement_nine_rails` | Nine Rails Depot | `territory_the_office` | Primary rail yard administration |
| `loc_weighbridge` | The Weighbridge | `territory_the_office` | Freight axle weighing & tariff enforcement |
| `loc_settlement_brine_pans` | Brine Pans | `territory_the_cutters` | Tidal salt harvesting settlement |
| `loc_the_shallows_market` | The Shallows Market | `territory_the_cutters` | Fish & salt barter quay |
| `loc_settlement_cape_beacon` | Cape Beacon | `territory_black_flotilla` | Coastal cliffside lighthouse stronghold |
| `loc_black_flotilla_outpost` | Coastal Outpost | `territory_black_flotilla` / `territory_the_fleet` | Marine salvage berths & dry dock |
| `loc_lock_gate_four` | Lock Gate Four | `territory_the_fleet` / `territory_the_tally` | Hydraulic canal bypass gate |
| `loc_settlement_iron_siding` | Iron Siding | `territory_deserter_coalition` | Armored railcar redoubt |
| `loc_settlement_fort_karkov` | Fort Karkov | `territory_deserter_coalition` | Heavy concrete battery fortification |
| `loc_settlement_slate_hollow` | Slate Hollow | `territory_cold_count` | Subterranean quarry mining settlement |
| `loc_low_background_lab` | Low-Background Lab | `territory_cold_count` | Deep-strata radiation physics lab |
| `loc_settlement_lock_seven` | Lock Seven | `territory_the_tally` | Sluice control & debt collection tollgate |
| `loc_settlement_silo_burrow` | Silo Burrow | `territory_grain_exchange` | Converted grain elevator community |
| `loc_grain_silo` | Central Grain Silo | `territory_grain_exchange` | Protected bulk seed & calorie reserve |
| `loc_settlement_st_nicholas` | St. Nicholas Sanctuary | `territory_quiet_house` | Artesian hospice priory |
| `loc_shrine_switchback_waystation` | Switchback Waystation | `territory_quiet_house` / `territory_long_walk` | Mountain pilgrim shelter |
| `loc_settlement_tinkers_notch` | Tinker's Notch | `territory_scavenger_guild` | Scrap fabricators' container town |
| `loc_cut_merchant_caravanserai` | Caravanserai Concourse | `territory_scavenger_guild` / `territory_lamplighters` | Free market trading ring |
| `loc_settlement_pilgrim_hearth` | Pilgrim Hearth | `territory_long_walk` | High-altitude circuit hospice |
| `loc_settlement_ferry_crossing` | Ferry Crossing | `territory_undertow` | River cable ferry barge community |
| `loc_water_station` | Water Pumping Station | `territory_undertow` / `territory_hydro_barons` | River silt intake & treatment plant |
| `loc_terrace_pumphouse` | Terrace Pumphouse | `territory_hydro_barons` | Pressurized aqueduct distribution hub |
| `loc_cut_abandoned_depot` | Sunken Freight Depot | `territory_iron_raiders` | Raider ambush redoubt |
| `loc_cut_arsenal_ruin` | Arsenal Ruin Trench | `territory_iron_raiders` | Military cache pillbox |
| `loc_excavation_command_vault` | Command Vault Alpha | `territory_the_provisioned` | Automated fallout bunker |
| `loc_logistics_reserve_cache` | Logistics Depot B-12 | `territory_the_provisioned` / `territory_archivists` | Pre-war sealed warehouse |
| `loc_excavation_archive_bunker` | Archive Crypt | `territory_archivists` | Subterranean magnetic media vault |
| `loc_hidden_relay_bunker` | Relay Bunker 09 | `territory_archivists` | Secure telecommunications node |
| `loc_cut_radiation_zone_alpha` | Hot Zone Alpha | `territory_sun_seekers` / `territory_osteophages` | High-rad ash flat extraction field |
| `loc_broadcast_bunker_echo` | Broadcast Bunker Echo | `territory_sun_seekers` | Radio mast & solar array |
| `loc_excavation_mine_shaft` | Deep Sump Drift | `territory_osteophages` | Irradiated lead leaching sumps |

---

## 3. Five Contested Geographic Flashpoints

1. **`zone_contested_water_rights` (Estuary Water Manifold & Silt Reclaim)**
   - Focal Node: `loc_holdfast`
   - Focal Location: `loc_water_station`
   - Claimants: `faction_hydro_barons`, `faction_undertow`, `faction_the_cutters`
2. **`zone_contested_cut_salvage` (The Cut Heavy Machinery Salvage Corridor)**
   - Focal Node: `loc_cut_abandoned_depot`
   - Focal Location: `loc_cut_abandoned_depot`
   - Claimants: `faction_scavenger_guild`, `faction_iron_raiders`, `faction_deserter_coalition`
3. **`zone_contested_merchant_crossroads` (Caravanserai Free Barter Concourse)**
   - Focal Node: `loc_cut_merchant_caravanserai`
   - Focal Location: `loc_cut_merchant_caravanserai`
   - Claimants: `faction_the_office`, `faction_the_tally`, `faction_scavenger_guild`
4. **`zone_contested_scarp_pass` (High Scarp Geothermal Chokepoint)**
   - Focal Node: `loc_hidden_relay_bunker`
   - Focal Location: `loc_shrine_switchback_waystation`
   - Claimants: `faction_long_walk`, `faction_cold_count`, `faction_quiet_house`
5. **`zone_contested_coastal_bluff` (Coastal Shelf Deep Saline Demarcation)**
   - Focal Node: `loc_black_flotilla_outpost`
   - Focal Location: `loc_black_flotilla_outpost`
   - Claimants: `faction_black_flotilla`, `faction_the_fleet`, `faction_the_cutters`
