# Collectible Scavenging Provenance — location_type → table mapping

Flagship integration (collectibles): documented mapping from
`collectibles.json` `location_type` to live `scavenging_tables.json` tables
(Plan 46 authority). Every placement carries **primary** provenance (the
required/authored home) and, where the primary table is not bound to a live
expedition destination, a **documented secondary** placement on a bound table
so the collectible is actually reachable in play.

Live reachability rule: an expedition destination rolls exactly the table named
by its `scavenging_table_id` (`expeditions.json`). Tables not referenced by any
destination never roll; secondary provenance exists to keep placements live
without weakening provenance.

## Mapping

| location_type | Preferred (primary) table | Live binding | Secondary table (when primary unbound) |
|---|---|---|---|
| residential | `table_loot_apartment_block` | bound | — |
| military | `table_loot_military_depot` | unbound | `table_loot_ordnance_shoulder` |
| civic (civil defense) | `table_loot_fire_station` | unbound | `table_loot_checkpoint` |
| civic (police/government) | `table_loot_police_station` | unbound | `table_loot_conscription_office` |
| school / cultural | `table_loot_school` | bound | — |
| clinic / medical | `table_loot_clinic` | unbound | `table_loot_hospital` |
| industrial | `table_loot_industrial_district` | bound | — |
| radio_station / transit | `table_loot_metro_station` | bound | — |
| religious | `table_loot_monastery` | unbound | `table_loot_pilgrim_hearth` |
| residential (survivor cache) | `table_loot_apartment_block` | bound | `table_loot_collapsed_structure` |

`table_loot_school` is the documented primary provenance for the exchange-day
newspaper (civic) per the flagship placement specification — school display
cases kept the paper's kept copy.

## Placement register

Primary (required, plan-exact weight/rarity):

| Collectible | Table | Weight | Rarity | Live |
|---|---|---:|---|---|
| `item_collectible_family_portrait` | apartment_block | 8 | common | yes |
| `item_collectible_unit_photograph` | military_depot | 5 | uncommon | via secondary |
| `item_collectible_civil_defense_poster` | fire_station | 6 | common | via secondary |
| `item_collectible_propaganda_poster` | police_station | 4 | uncommon | via secondary |
| `item_collectible_concert_poster` | school | 5 | common | yes |
| `item_collectible_field_medicine_handbook` | clinic | 4 | uncommon | via secondary |
| `item_collectible_pre_war_novel` | apartment_block | 6 | common | yes |
| `item_collectible_diesel_service_manual` | industrial_district | 3 | rare | yes |
| `item_collectible_radio_repair_guide` | metro_station | 3 | rare | yes |
| `item_collectible_road_map` | police_station | 3 | uncommon | via secondary |
| `item_collectible_exchange_day_newspaper` | school | 2 | rare | yes (UNIQUE) |

Additional real-catalog placements (provenance-appropriate):

| Collectible | Table | Weight | Rarity | Live |
|---|---|---:|---|---|
| `item_collectible_mothers_letter` | apartment_block | 4 | common | yes |
| `item_collectible_childs_doll` | apartment_block | 3 | common | yes |
| `item_collectible_science_magazine` | school | 4 | common | yes |
| `item_collectible_transit_badge` | metro_station | 4 | common | yes |
| `item_collectible_unit_log_fragment` | military_depot | 3 | uncommon | via secondary |
| `item_collectible_topo_map` | military_depot | 2 | rare | via secondary (UNIQUE) |
| `item_collectible_casualty_list` | military_depot | 2 | rare | via secondary (UNIQUE) |
| `item_collectible_survivor_map` | apartment_block | 1 | rare | yes (UNIQUE) |
| `item_collectible_prayer_book` | monastery | 4 | uncommon | via secondary |
| `item_collectible_civil_defense_badge` | fire_station | 4 | uncommon | via secondary |

Secondary (documented reachability placements on bound tables):
`ordnance_shoulder` (unit photograph, unit log fragment, topo map, casualty
list), `checkpoint` (civil defense poster, road map, civil defense badge),
`conscription_office` (propaganda poster), `hospital` (field medicine
handbook), `pilgrim_hearth` (prayer book), `collapsed_structure` (survivor
map).

Totals: 32 placements · 22 live · 21 distinct collectibles · all 3
globally-unique collectibles placed and claim-suppressed at generation.

Uniqueness is **definition-level only** (`collectibles.json` `unique` flag →
`UniqueItemClaimRegistry`); table entries carry no contradictory unique
metadata. Balance audit artifact:
`artifacts/collectible-scavenging-balance-report.md` (10,000 offline rolls per
changed table, fixed seed; max collectible weight share 8.7%).
