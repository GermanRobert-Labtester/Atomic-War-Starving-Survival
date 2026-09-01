# Production Content Utilization & Scanner Registry

This document lists all newly registered catalogs, definitions, and consumer links for Plan 22 to satisfy `CatalogIntegrityValidator` and `ContentUtilizationScanner`.

---

## 1. Newly Authored Product & Item IDs

### Foundry Items (14 new items)
- `item_foundry_roof_armor_plate`
- `item_foundry_shoring_bracket`
- `item_foundry_blast_fitting`
- `item_foundry_reinforcement_shoe`
- `item_foundry_structural_coupling`
- `item_foundry_replacement_die`
- `item_foundry_drill_blanks`
- `item_foundry_crucible_spare`
- `item_foundry_press_fitting`
- `item_foundry_bearing_housing`
- `item_foundry_furnace_grate`
- `item_foundry_weather_canister`
- `item_foundry_cast_shot`
- `item_foundry_casing_blanks`

### Greenhouse Seeds & Crops (16 new items)
- `item_seed_hardy_tuber`, `crop_hardy_tuber`
- `item_seed_ash_grain`, `crop_ash_grain`
- `item_seed_biolum_mushroom`, `crop_biolum_mushroom`
- `item_seed_nutrient_algae`, `crop_nutrient_algae`
- `item_seed_medicinal_herb`, `crop_medicinal_herb`
- `item_seed_leafy_green`, `crop_leafy_green`
- `item_seed_oilseed`, `crop_oilseed`
- `item_seed_cold_legume`, `crop_cold_legume`

### Apiculture & Salt Items (7 new items)
- `item_honey_pot`
- `item_beeswax_block`
- `item_raw_propolis`
- `item_mead_must_base`
- `item_preservation_salt`
- `item_trade_salt_sack`
- `item_medical_saline_salt`

### Preserved Foods (10 new items)
- `food_pickled_tubers`
- `food_dried_mushrooms`
- `food_smoked_meat`
- `food_canned_grain_stew`
- `food_salted_meat`
- `food_fat_confit`
- `food_fermented_sauerkraut`
- `food_honey_preserved_pulp`
- `food_dried_herb_packets`
- `food_brined_legume_mash`

---

## 2. Consumer Systems

All new items feed directly into:
- `SilentFoundrySystem` & `FoundryActionSurface`
- `GreenhouseSystem` & `KitchenNutritionSystem`
- `ApicultureSystem` & `SaltMineExtractionSystem`
- `DutyRosterSystem` & `NeedsSystem`
- `BunkerMaintenanceCatalog` & `EconomyMarketPanel`
