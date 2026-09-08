# Starting Profile Item Eligibility

Profile supplies use only existing item definitions from the authoritative item
catalog. No new item IDs, recipes, research nodes, rooms, quests, faction
tokens, weapons, or ammunition are introduced.

| Item ID | Role | Catalog value | Weight | Progression / eligibility decision |
|---|---|---:|---:|---|
| `clean_water` | Immediate survival | 15 | 0.5 | Eligible; quantity is the main water-pressure knob. |
| `canned_food` | Immediate survival | 12 | 0.5 | Eligible; quantity is the main food-pressure knob. |
| `irradiated_water` | Risky survival reserve | 2 | 0.5 | Eligible; retained in every profile as a meaningful tradeoff. |
| `item_air_filter_hepa` | Shelter air maintenance | 25 | 1.5 | Eligible; existing early shelter material, not a room unlock. |
| `item_desal_membrane` | Water treatment material | 30 | 0.8 | Eligible; used only to shift treatment pressure. |
| `iodine_pills` | Radiation prevention | 6 | 0.1 | Eligible; ordinary consumable, no standing or research effect. |
| `bandage` | Basic medical recovery | 10 | 0.1 | Eligible; ordinary consumable. |
| `rad_away` | Radiation treatment | 20 | 0.2 | Eligible in restrained quantities; no advanced medication. |
| `item_dosimeter_pen` | Dose measurement | 15 | 0.1 | Eligible; diagnostic device only. |
| `item_geiger_m3` | Radiation measurement | 35 | 0.9 | Eligible; diagnostic device only. |
| `gas_mask` | Face protection | 40 | 1.5 | Eligible; already part of the legacy baseline. |
| `hazmat_suit` | Body protection | 40 | 5.0 | Eligible; already part of the legacy baseline. |
| `battery` | Device/material support | 5 | 0.2 | Eligible; quantity supports device-oriented starts without unlocking systems. |
| `scrap_mechanical` | Repair/material leverage | 2 | 0.2 | Eligible; common material, no crafted product granted. |
| `scrap_electronic` | Repair/material leverage | 3 | 0.1 | Eligible; common material, no research granted. |
| `filter_pack` | Consumable filtration | 10 | 0.3 | Eligible in restrained quantities; does not build or unlock a room. |
| `calibration_kit` | Device maintenance | 18 | 0.4 | Eligible in restrained quantities; the recipe remains unchanged. |
| `item_seed_mushroom` | Early greenhouse input | 4 | 0.1 | Eligible; ordinary crop input. |
| `item_seed_tuber` | Early greenhouse input | 6 | 0.3 | Eligible; ordinary crop input. |
| `item_seed_grain` | Early greenhouse input | 5 | 0.05 | Eligible; ordinary crop input. |
| `item_planter_box` | Greenhouse material | 25 | 8.0 | Eligible in a small quantity; does not prebuild a room. |
| `item_grow_medium` | Greenhouse material | 8 | 2.0 | Eligible in a small quantity; does not grant crop research. |
| `item_blight_treatment` | Greenhouse maintenance | 15 | 0.2 | Eligible in a small quantity; no disease immunity is granted. |

## Exclusions

- `item_seed_wheat` is excluded because it is a quest-tier item with a high
  trade value and can bypass intended rare-seed progression.
- No quest/evidence/unique collectible item is eligible.
- No faction access or standing item is eligible.
- No weapon or ammunition is added. The checkpoint profile is a survey and
  protection start, not a combat class.
- No item is treated as a permanent trait or background marker after seeding.
