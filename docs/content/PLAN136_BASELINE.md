# Plan 136 Baseline: Item Description Catalog Runtime Activation

## 1. Mission Overview
Turn `item_description_texts.json` from a prose warehouse and isolated test fixture into a real, read-only item-inspection layer that enriches inventory, trade, crafting, medical, and expedition presentation without becoming a second item-mechanics authority.

## 2. Pre-Implementation Audit Metrics
- **Data File**: `Assets/StreamingAssets/Data/item_description_texts.json`
- **Schema Version**: `1`
- **Collection ID**: `item_description_texts`
- **Raw Entries Count**: `184`
- **Unique Item IDs**: `183`
- **Duplicate Entries**: `1`
  - `tool_rake` appears at index 164 (line 2777) and verbatim again at index 166 (line 2811).
- **Categories Authoring Breakdown**:
  - `device`: 4
  - `medical`: 4
  - `protective`: 2
  - `tool`: 33
  - `water`: 2
  - `food`: 1
  - `fuel`: 1
  - `material`: 14
  - `weapon`: 9
  - `armor`: 5
  - `clothing`: 12
  - `container`: 3
  - `light`: 3
  - `communication`: 2
  - `defense`: 3
  - `electronics`: 29
  - `quest`: 3
  - `luxury`: 3
  - `comfort`: 4
  - `survival`: 7
  - `cooking`: 4
  - `hygiene`: 4
  - `sports`: 4
  - `vehicle`: 5
  - `furniture`: 6
  - `art`: 10
  - `ammunition`: 7

## 3. Canonical Resolution Baseline
- **Canonical Item Population**: 659 in `items.json`, 917 across all 11 active catalogs.
- **Exact Canonical Matches (`items.json`)**: 27
  `dosimeter`, `geiger_counter`, `iodine_pills`, `anti_rad`, `gas_mask`, `hazmat_suit`, `water_filter`, `air_filter`, `clean_water`, `irradiated_water`, `canned_food`, `bandage`, `medical_kit`, `fuel`, `battery`, `cloth`, `scrap_metal`, `mechanical_parts`, `electronic_scrap`, `chemicals`, `handheld_radio`, `weapon_revolver`, `weapon_smg`, `ammo_556`, `ammo_762`, `ammo_12g`, `ammo_308`.
- **Combat Catalog Matches (`combat_catalog.json`)**: 8
  - Weapons: `weapon_pipe_rifle`, `weapon_scrap_shotgun`, `weapon_bolt_rifle`, `weapon_assault_rifle`.
  - Materials/Armor: `armor_cloth`, `armor_kevlar`, `armor_plate`, `material_wood`.
- **Canonical Aliases**: 14 (including `survival_water_purification_tablets` -> `water_purification_tablets`, `luxury_jewelry` -> `jewelry`, `luxury_book` -> `book`, `material_rope` -> `rope`, `ammo_9mm` -> `ammo_9x19`, `electronics_battery_pack` -> `battery_pack`, `electronics_stethoscope` -> `stethoscope`, etc.).
- **Orphaned / Non-Inventory Flavor Objects**: 134
  Legacy entries describing vehicles, furniture, art objects, sports gear, electronics appliances.
  *Policy*: Preserved as read-only flavor entries; zero phantom items created in gameplay catalogs.
- **Starting Cohort Supplies Coverage**: 100% (15 / 15 starting supply definitions mapped and verified).

## 4. Utilization & Architectural Baseline
- **Content Utilization Classification**: Previously `OPTIONAL` in `artifacts/content-utilization-baseline.json`.
- **Target Classification**: `GAMEPLAY_CONSUMED` / `UI_CONSUMED` via `ItemDescriptionCatalogLoader` -> `ItemDescriptionCatalog` -> `ItemInspectionModel` -> `InventoryDetailPanel`.
- **Core Invariant**: Zero engine dependencies in `Assets/Ashfall.Core/`.
