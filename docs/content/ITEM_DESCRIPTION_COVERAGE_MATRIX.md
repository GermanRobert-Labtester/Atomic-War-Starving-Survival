# Item Description Coverage Matrix

## 1. Tier 1: Core Starting Supplies & Lifeline Equipment
All starting supplies from `starting_supplies.json` resolve deterministically to authored inspection entries:

| Item ID | Display Name | Category | Resolution Source | Inspection Status |
|---------|--------------|----------|-------------------|-------------------|
| `clean_water` | Clean Water | Water | Exact Match | FULL (14 fields) |
| `canned_food` | Canned Food | Food | Exact Match | FULL (14 fields) |
| `irradiated_water` | Irradiated Water | Water | Exact Match | FULL (14 fields) |
| `item_air_filter_hepa` | HEPA Air Filter | Filter | Alias: `air_filter` | FULL (14 fields) |
| `item_desal_membrane` | Desalination Membrane | Filter | Alias: `water_filter` | FULL (14 fields) |
| `iodine_pills` | Potassium Iodide | Medical | Exact Match | FULL (14 fields) |
| `bandage` | Sterile Bandage | Medical | Exact Match | FULL (14 fields) |
| `rad_away` | Rad-Away Decorporation | Medical | Alias: `anti_rad` | FULL (14 fields) |
| `item_dosimeter_pen` | Pocket Dosimeter Pen | Device | Alias: `dosimeter` | FULL (14 fields) |
| `item_geiger_m3` | M3 Military Survey Meter | Device | Alias: `geiger_counter`| FULL (14 fields) |
| `gas_mask` | Full-Face Gas Mask | Protective | Exact Match | FULL (14 fields) |
| `hazmat_suit` | NBC Hazmat Suit | Protective | Exact Match | FULL (14 fields) |
| `battery` | Standard Battery Cell | Material | Exact Match | FULL (14 fields) |
| `scrap_mechanical` | Mechanical Scrap | Material | Alias: `scrap_metal` | FULL (14 fields) |
| `scrap_electronic` | Electronic Scrap | Material | Alias: `electronic_scrap`| FULL (14 fields) |

**Starting Supply Coverage**: 15/15 (100%).

## 2. Tier 2: Weapons, Materials, Medical & Common Scavenged Goods
- **Weapons**: `weapon_revolver` (Exact), `weapon_smg` (Exact), `weapon_pipe_rifle` (Combat), `weapon_scrap_shotgun` (Combat), `weapon_bolt_rifle` (Combat), `weapon_assault_rifle` (Combat), `pistol_cz75_9x19` (Alias: `weapon_pistol`), `weapon_pipe_shotgun` (Alias: `weapon_shotgun`), `weapon_marksman_rifle` (Alias: `weapon_sniper_rifle`).
- **Ammunition**: `ammo_556` (Exact), `ammo_762` (Exact), `ammo_12g` (Exact), `ammo_308` (Exact), `ammo_9x19` (Alias: `ammo_9mm`).
- **Materials**: `cloth` (Exact), `chemicals` (Exact), `mechanical_parts` (Exact), `fuel` (Exact), `rope` (Alias: `material_rope`).
- **Devices**: `handheld_radio` (Exact), `battery_pack` (Alias: `electronics_battery_pack`), `stethoscope` (Alias: `electronics_stethoscope`).
- **Trade & Valuables**: `jewelry` (Alias: `luxury_jewelry`), `book` (Alias: `luxury_book`).

## 3. Fallback Graceful Degradation
Any item in `items.json` without an explicit entry in `item_description_texts.json` (such as specialized expansion crafting components) renders with its baseline `ItemDefinition.description`, displaying standard stats with zero presentation errors or crashes.
