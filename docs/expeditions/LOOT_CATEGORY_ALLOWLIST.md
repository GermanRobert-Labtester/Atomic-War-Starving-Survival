# Loot Category Allowlist

## 1. Resolver Architecture

In `ExpeditionSystem.cs`, `PickLootCategory` directly pulls entries from `def.lootCategories` and uses them as the item IDs passed to `AddLoot(exp, itemId, weight)`. Therefore, every entry in `lootCategories` must be a valid, existing `id` in `items.json`.

## 2. Validated Thematic Loot Allowlist

The following items from `items.json` form the canonical allowlist across all 50 destinations:

### Medical & Chemical
- `bandage`
- `medical_kit`
- `medkit`
- `antibiotics`
- `anti_rad`
- `rad_away`
- `iodine_pills`
- `splint`
- `tweezers`
- `field_surgical_kit`
- `alcohol`
- `chemicals`
- `chemical_solvent`

### Food, Water & Agriculture
- `clean_water`
- `canned_food`
- `canned_soup`
- `dried_rations`
- `military_mre`
- `sugar`
- `seed_packets`
- `item_honey_pot`
- `roots`
- `berries`
- `growing_manual`

### Mechanical, Industrial & Materials
- `scrap_metal`
- `mechanical_parts`
- `metal_pipe`
- `steel_rebar`
- `engine`
- `lubricant_oil`
- `cloth`
- `scrap_wood`
- `wooden_plank`
- `box_of_nails_10`
- `duct_tape`
- `concrete_mix`
- `rope`

### Electrical & Technology
- `electronic_scrap`
- `battery`
- `aa_batteries`
- `vacuum_tube`
- `solar_cell`
- `handheld_radio`
- `military_radio`
- `copper_wire_10m_of_10m` (Plan 76: replaces the invalid `copper_wire` ref)

### Radiation & Detection
- `dosimeter`
- `geiger_counter`
- `gas_mask`
- `air_filter`
- `water_filter`
- `water_purification_tablets`

### Fuel & Power
- `fuel`
- `diesel_fuel`
- `fuel_canister`

### Ammunition & Munitions
- `ammo_9x19`
- `ammo_762`
- `ammo_12g`
- `ammunition_brass`
- `smokeless_powder`

### Knowledge, Culture & Commerce
- `book`
- `childrens_books`
- `blueprint_roll`
- `pocket_notebook`
- `currency`
