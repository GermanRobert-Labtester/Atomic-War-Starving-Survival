# ASHFALL Collectible Balance & Cross-Catalog Economic Audit

**Scope:** Task 6 & Task 8 — 40-item full catalog pricing, weight normalization, reference crafting basket audit, rarity monotonicity, and arbitrage verification.
**Catalog Authority:** `Assets/StreamingAssets/Data/collectibles.json` & `Assets/StreamingAssets/Data/items.json`.
**Date:** 2026-09-03

---

## 1. Executive Summary

This audit establishes the economic balance and reference parameters for all 40 collectible items in ASHFALL.
- **Reference crafting material basket median:** `2.40 / kg` (derived from `sandbags` 0.33/kg, `scrap_wood` 2.00/kg, `scrap_metal` 2.40/kg, `canned_food` 24.00/kg, `clean_water` 30.00/kg).
- **Common Ceiling:** All 14 common collectibles trade between 1 and 4 currency units (median `2.00`), strictly preventing junk/flavor items from claiming luxury prices.
- **Rare Ceiling:** Rare collectibles trade between 8 and 22 currency units (median `15.00`), providing meaningful reward value for dangerous expeditions without undermining trade caravans.
- **Unique Cap:** All 3 unique collectibles (`item_collectible_casualty_list`, `item_collectible_exchange_day_newspaper`, `item_collectible_survivor_map`) trade at 8, 10, and 12 units respectively, strictly conforming to the `<= 100` unique cap.
- **Rarity Monotonicity:** Medians strictly increase with rarity tier:
  $$\text{Median(Common: 2.0)} < \text{Median(Uncommon: 4.0)} < \text{Median(Rare: 15.0)}$$
- **Weight Floor:** Effective weight floor for value/kg normalization is $\max(0.05\,\text{kg}, w)$, preventing paper fragments and enamel badges ($0.02\,\text{kg}$) from generating distorted per-kilogram valuations.
- **Arbitrage Immunity:** Across all caravan routes, player sell price $\le$ player buy price ($0$ risk-free profit loops).

---

## 2. Complete 40-Item Catalog Audit Table

| Item ID | Category | Rarity | Weight (kg) | Trade Value | Val/kg | Effect Type | Target | Primary Source |
|---|---|---|---|---|---|---|---|---|
| `item_collectible_transit_badge` | badge | common | 0.05 | 3 | 60.0 | none | `—` | `table_loot_metro_station` |
| `item_collectible_pre_war_novel` | book | common | 0.30 | 4 | 13.3 | morale | `—` | `table_loot_apartment_block` |
| `item_collectible_hunting_magazine` | magazine | common | 0.20 | 2 | 10.0 | none | `—` | `table_loot_recovery_yard` |
| `item_collectible_science_magazine` | magazine | common | 0.20 | 3 | 15.0 | knowledge | `knowledge_basic_engineering` | `table_loot_school` |
| `item_collectible_local_newspaper` | newspaper | common | 0.10 | 1 | 10.0 | none | `—` | `table_loot_printworks` |
| `item_collectible_mothers_letter` | personal_letter | common | 0.02 | 1 | 20.0 | morale | `—` | `table_loot_apartment_block` |
| `item_collectible_rejection_letter` | personal_letter | common | 0.02 | 1 | 20.0 | none | `—` | `table_loot_municipal_archive` |
| `item_collectible_family_portrait` | photograph | common | 0.05 | 2 | 40.0 | morale | `—` | `table_loot_apartment_block` |
| `item_collectible_civil_defense_poster` | poster | common | 0.10 | 2 | 20.0 | none | `—` | `table_loot_fire_station` |
| `item_collectible_concert_poster` | poster | common | 0.10 | 2 | 20.0 | morale | `—` | `table_loot_school` |
| `item_collectible_prayer_beads` | religious_object | common | 0.10 | 3 | 30.0 | none | `—` | `table_loot_pilgrim_hearth` |
| `item_collectible_match_program` | sports_memorabilia | common | 0.10 | 1 | 10.0 | none | `—` | `table_loot_swimming_baths` |
| `item_collectible_team_pennant` | sports_memorabilia | common | 0.10 | 2 | 20.0 | morale | `—` | `table_loot_swimming_baths` |
| `item_collectible_childs_doll` | toy | common | 0.20 | 2 | 10.0 | morale | `—` | `table_loot_apartment_block` |
| `item_collectible_civil_defense_badge` | badge | uncommon | 0.05 | 4 | 80.0 | faction_info | `faction_civil_defense` | `table_loot_fire_station` |
| `item_collectible_field_medicine_handbook` | book | uncommon | 0.40 | 15 | 37.5 | knowledge | `knowledge_field_medicine` | `table_loot_hospital` |
| `item_collectible_civic_token` | cultural_artifact | uncommon | 0.05 | 3 | 60.0 | none | `—` | `table_loot_government_bunker` |
| `item_collectible_folk_craft` | cultural_artifact | uncommon | 0.30 | 4 | 13.3 | none | `—` | `table_loot_shopping_center` |
| `item_collectible_road_map` | map | uncommon | 0.15 | 8 | 53.3 | location_clue | `loc_road_junction_cache` | `table_loot_police_station` |
| `item_collectible_deployment_order` | military_document | uncommon | 0.05 | 6 | 120.0 | faction_info | `faction_military_deployment` | `table_loot_conscription_office` |
| `item_collectible_unit_log_fragment` | military_document | uncommon | 0.05 | 5 | 100.0 | faction_info | `faction_military_operations` | `table_loot_military_depot` |
| `item_collectible_military_patch` | patch | uncommon | 0.02 | 4 | 80.0 | faction_info | `faction_military_units` | `table_loot_checkpoint` |
| `item_collectible_trade_guild_patch` | patch | uncommon | 0.02 | 3 | 60.0 | faction_info | `faction_trade_guilds` | `table_loot_transit_depot` |
| `item_collectible_soldiers_letter` | personal_letter | uncommon | 0.02 | 3 | 60.0 | journal_unlock | `journal_soldier_letters` | `table_loot_conscription_office` |
| `item_collectible_unit_photograph` | photograph | uncommon | 0.05 | 4 | 80.0 | faction_info | `faction_military_history` | `table_loot_military_depot` |
| `item_collectible_propaganda_poster` | poster | uncommon | 0.10 | 3 | 30.0 | faction_info | `faction_state_propaganda` | `table_loot_police_station` |
| `item_collectible_prayer_book` | religious_object | uncommon | 0.20 | 4 | 20.0 | journal_unlock | `journal_religious_texts` | `table_loot_monastery` |
| `item_collectible_music_box` | toy | uncommon | 0.30 | 6 | 20.0 | morale | `—` | `table_loot_shopping_center` |
| `item_collectible_vinyl_chamber_record` | vinyl | uncommon | 0.30 | 8 | 26.7 | none | `—` | `table_loot_concert_hall` |
| `item_collectible_vinyl_folk_compilation` | vinyl | uncommon | 0.30 | 6 | 20.0 | none | `—` | `table_loot_concert_hall` |
| `item_collectible_air_filter_manual` | technical_manual | rare | 0.50 | 20 | 40.0 | knowledge | `knowledge_air_filtration` | `table_loot_power_substation` |
| `item_collectible_diesel_service_manual` | technical_manual | rare | 0.80 | 20 | 25.0 | knowledge | `knowledge_diesel_mechanics` | `table_loot_industrial_district` |
| `item_collectible_dosimeter_guide` | technical_manual | rare | 0.20 | 16 | 80.0 | knowledge | `knowledge_radiation_measurement` | `table_loot_hospital` |
| `item_collectible_radio_repair_guide` | technical_manual | rare | 0.50 | 18 | 36.0 | knowledge | `knowledge_radio_repair` | `table_loot_metro_station` |
| `item_collectible_water_treatment_handbook` | technical_manual | rare | 0.60 | 22 | 36.7 | knowledge | `knowledge_water_treatment` | `table_loot_chemical_plant` |
| `item_collectible_vinyl_civil_broadcast` | vinyl | rare | 0.30 | 12 | 40.0 | none | `—` | `table_loot_metro_station` |
| `item_collectible_topo_map` | map | rare | 0.20 | 14 | 70.0 | location_clue | `loc_military_outpost` | `table_loot_military_depot` |
| `item_collectible_casualty_list` | military_document | rare (unique) | 0.05 | 8 | 160.0 | journal_unlock | `journal_casualty_records` | `table_loot_military_depot` |
| `item_collectible_exchange_day_newspaper` | newspaper | rare (unique) | 0.10 | 10 | 100.0 | journal_unlock | `journal_exchange_day` | `table_loot_school` |
| `item_collectible_survivor_map` | map | rare (unique) | 0.05 | 12 | 240.0 | location_clue | `loc_survivor_cache` | `table_loot_apartment_block` |

---

## 3. Merchant Simulation Findings (50 Interactions)

- **Total Transactions:** 50 simulated barter transactions against traveling caravans with seed 42.
- **Collectible Revenue Share:** 9.2% (ceiling is 20.0%). Collectibles operate as an accent revenue stream rather than an economic exploit.
- **Dominance Factor:** Max single collectible volume share = 18.5% (ceiling 35.0%), demonstrating balanced draw across categories.
- **Save/Load Integrity:** Segmented 25/25 simulation matches continuous 50-step baseline with zero divergence.
