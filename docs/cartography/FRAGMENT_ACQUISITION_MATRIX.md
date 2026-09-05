# Fragment Acquisition Matrix (Plan 85)

Every one of the 32 fragments has at least one reachable producer (§1.9). Producers are Plan 46 location-typed scavenging tables; fragment entries carry weight 2 (rare but reachable), `rarity_tier: rare`, no physical item, no hazard of their own (table hazards apply).

| Fragment | Zone | Producer table(s) | Context | Cadence note |
|---|---|---|---|---|
| damaged_map_industrial_1 | industrial | table_loot_industrial_district | factory district | early (destination danger 4) |
| damaged_map_industrial_2 | industrial | table_loot_industrial_district | sewer/utility paperwork | early |
| damaged_map_industrial_3 | industrial | table_loot_rail_yard | transit paperwork | early |
| damaged_map_suburban_1 | suburban | table_loot_apartment_block | residential blueprint | early |
| damaged_map_suburban_2 | suburban | table_loot_school | school library note | early |
| damaged_map_military_1 | military | table_loot_military_depot | depot survey | mid |
| damaged_map_military_2 | military | table_loot_military_depot | elevator schematic | mid |
| damaged_map_military_3 | military | table_loot_checkpoint | checkpoint manifest | early-mid |
| damaged_map_crater_1 | crater | table_loot_dead_hand_core | thermal survey | late (ground-zero access) |
| damaged_map_crater_2 | crater | table_loot_government_bunker | conduit routing | late |
| damaged_map_crater_3 | crater | table_loot_dead_hand_core | frequency card | late |
| damaged_map_coast_1 | deep_coast | table_loot_tank_farm | bathymetric chart | mid |
| damaged_map_coast_2 | deep_coast | table_loot_relay_mast | tidal lock manual | early-mid |
| damaged_map_scarp_1 | high_scarp | table_loot_relay_mast | elevation map | early-mid |
| damaged_map_scarp_2 | high_scarp | table_loot_hunting_cabin | surveyor's log | early |
| damaged_map_medical_1 | old_medical_quarter | table_loot_hospital | ambulance approach | early-mid |
| damaged_map_medical_2 | old_medical_quarter | table_loot_clinic | utility basement | early-mid |
| damaged_map_medical_3 | old_medical_quarter | table_loot_fire_station | casualty routing | early |
| damaged_map_court_1 | court_district | table_loot_police_station | property index | early-mid |
| damaged_map_court_2 | court_district | table_loot_municipal_archive | service corridor | mid |
| damaged_map_court_3 | court_district | table_loot_printworks | transfer sheet | early-mid |
| damaged_map_pasture_1 | pasture_valley | table_loot_veterinary_surgery | herd ledger | early |
| damaged_map_pasture_2 | pasture_valley | table_loot_farm | irrigation run | early (renewable table) |
| damaged_map_woods_1 | north_woods | table_loot_forestry_compound | firebreak map | early (renewable table) |
| damaged_map_woods_2 | north_woods | table_loot_forestry_compound | spur survey | early (renewable table) |
| damaged_map_woods_3 | north_woods | table_loot_hunting_cabin | cache grid | early |
| damaged_map_university_1 | university_quarter | table_loot_school | teaching wing | early |
| damaged_map_university_2 | university_quarter | table_loot_observatory | ventilation spine | mid |
| damaged_map_university_3 | university_quarter | table_loot_observatory | freight-lift call sheet | mid |
| damaged_map_metro_1 | metro_service_ring | table_loot_metro_station | traction feed | early-mid |
| damaged_map_metro_2 | metro_service_ring | table_loot_metro_station | pump gallery | early-mid |
| damaged_map_metro_3 | metro_service_ring | table_loot_power_substation | junction wiring key | early-mid |

## Distribution properties

- **Zones with 2+ producer contexts:** 9 of 12 (only `industrial_1/_2` and the depot-paired military/crater pairs share a table — depot paperwork is their fiction).
- **Early vs late spread:** fragments for danger-2/3 zones surface from common early destinations (farm, school, clinic, forestry); crater/ground-zero fragments sit in late-game tables (dead_hand_core, government_bunker). The catalog creates campaign-long cadence rather than a first-run flood.
- **Unique/one-time semantics:** fragment entries are repeatable table entries; after registration they resolve to nothing tangible (see lifecycle doc), so duplicates cannot accelerate completion or become spam (§85C.3/85C.4).
- **Quest/trader producers:** none used (§85C.6/85C.7) — no missable-fragment hard-lock exists.
- **Density (§85E.6):** 32 tokens across 23 tables at weight 2 against typical 60–250 table weight: ordinary loot remains dominant; fragment clutter is impossible (no items).
- **Deferred producers:** none. Unreachable fragments: zero.
