# Plan 46 — Expedition Destination to Scavenging Table Bindings

## Initial 11 Production Expedition Bindings

The following expedition destinations in `Assets/StreamingAssets/Data/expeditions.json` have been wired directly with `scavenging_table_id`:

| Expedition Destination ID | Display Name | Danger Level | Bound Scavenging Table ID | Location Type |
|---|---|---|---|---|
| `loc_the_allotments` | The Works Allotment Commune | 2 | `table_loot_farm` | Farm |
| `loc_denial_cut_substation` | The Denial Cut Substation | 4 | `table_loot_power_substation` | Power Substation |
| `suburban_house` | Suburban House | 2 | `table_loot_apartment_block` | Apartment Block |
| `rural_gas_station` | Rural Gas Station | 3 | `table_loot_industrial_district` | Industrial District |
| `old_library_cache` | Old Library Cache | 3 | `table_loot_school` | School |
| `ruined_garage` | Ruined Garage | 3 | `table_loot_warehouse` | Warehouse |
| `loc_grange_hall` | The Grange Hall | 3 | `table_loot_farm` | Farm |
| `loc_school_gymnasium` | School Gymnasium | 3 | `table_loot_school` | School |
| `loc_water_station` | Water Station | 3 | `table_loot_chemical_plant` | Chemical Plant |
| `prewar_medical_cache` | Pre-War Medical Cache | 4 | `table_loot_hospital` | Hospital |
| `electrical_substation` | Electrical Substation | 4 | `table_loot_power_substation` | Power Substation |

---

## Fallback & Non-Breaking Compatibility
For any existing expedition destination without an explicit `scavenging_table_id`, `ExpeditionSystem` seamlessly continues to resolve loot from the legacy `lootCategories` array. This guarantees zero breaking changes across all current and future expedition nodes.
