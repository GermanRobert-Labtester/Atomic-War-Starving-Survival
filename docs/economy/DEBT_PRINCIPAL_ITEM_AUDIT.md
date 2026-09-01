# Plan 40 — Principal Item Audit

## All Principal Items Resolve

| Template | Item ID | Type | Trade Value | Stack Max |
|---|---|---|---|---|
| supply_corps_rations | canned_food | Food | 12 | 10 |
| supply_corps_fuel | fuel | Fuel | 14 | 20 |
| supply_corps_medical | medical_kit | Medical | 10 | 10 |
| hydro_barons_water | clean_water | Water | 15 | 10 |
| hydro_barons_filter | water_filter | Filter | 20 | 10 |
| hydro_barons_purification | water_purification_tablets_40_of_40 | Medical | 18 | 10 |
| railway_guild_fuel | diesel_fuel | Fuel | 10 | 20 |
| railway_guild_parts | mechanical_parts | Material | 3 | 50 |
| railway_guild_transport | engine | Tool | 80 | 1 |
| ordnance_foundry_ammo | ammo_762 | Ammo | 12 | 100 |
| ordnance_foundry_tools | soldering_kit | Tool | 14 | 10 |
| ordnance_foundry_armor | gas_mask | Protective | 40 | 10 |
| scavengers_food | dried_rations | Food | 5 | 20 |
| scavengers_medicine | antibiotics | Medical | 10 | 10 |
| scavengers_equipment | dosimeter | Device | 30 | 10 |

## Validation
- All 15 item IDs exist in items.json
- All quantities are representable within stackMax
- All items have tradeValue > 0
- No quest-critical items used as principals
