# GREENHOUSE ITEM PARITY MATRIX — the preserved 14

The 14 live baseline entries, preserved byte-identical through Plan 91.
(Plan §1.2 — IDs, names, descriptions, types, stacks, weights, trade values
all unchanged. `tainted_food` carries the live `contamination`/`hungerRestore`
fields it shipped with.)

| # | ID | Name | Type | Stack | Weight | Trade | Functional role |
|---|---|---|---|---:|---:|---:|---|
| 1 | `item_seed_mushroom` | Spore Capsule | Material | 20 | 0.10 | 4 | Seed — mushroom crop (LIVE: planted) |
| 2 | `item_seed_tuber` | Tuber Cutting | Material | 10 | 0.30 | 6 | Seed — tuber crop (LIVE: planted) |
| 3 | `item_seed_grain` | Mutated Grain Spike | Material | 30 | 0.05 | 5 | Seed — grain crop (LIVE: planted) |
| 4 | `item_seed_wheat` | Pre-War Wheat Seed | Quest | 5 | 0.05 | 80 | Seed — wheat crop, unlock-gated (LIVE: planted) |
| 5 | `item_planter_box` | Planter Box | Material | 6 | 8.0 | 25 | Growing infrastructure (no live consumer — trade/supply) |
| 6 | `item_grow_lamp` | Grow Lamp | Device | 2 | 3.0 | 40 | Light infrastructure (no live consumer — trade/supply) |
| 7 | `item_lead_glass_pane` | Lead-Glass Pane | Material | 4 | 5.0 | 30 | Shielded glazing (no live consumer — trade/supply) |
| 8 | `item_blight_treatment` | Blight Treatment | Medical | 6 | 0.20 | 15 | Crop-disease wash (LIVE: consumed by `TreatBlight`) |
| 9 | `item_grow_medium` | Sterile Grow Medium | Material | 8 | 2.0 | 8 | Planting substrate (no live consumer — trade/supply) |
| 10 | `crop_mushroom` | Greenhouse Mushroom | Food | 20 | 0.10 | 3 | Clean yield of mushroom crop (LIVE: harvested) |
| 11 | `crop_tuber` | Greenhouse Tuber | Food | 12 | 0.30 | 5 | Clean yield of tuber crop (LIVE: harvested) |
| 12 | `crop_grain` | Clean Grain | Food | 30 | 0.10 | 6 | Clean yield of grain crop (LIVE: harvested) |
| 13 | `crop_wheat` | Pre-War Wheat | Food | 20 | 0.20 | 30 | Clean yield of wheat crop (LIVE: harvested) |
| 14 | `tainted_food` | Tainted Rations | ContaminatedFood | 20 | 0.20 | 1 | Tainted yield of any blighted crop (LIVE: harvested) |

## Removed dead copies (16)

The following existed in `greenhouse_items.json` at Plan 91 start but were
**inert** — each has a differing, improved parity copy in `items.json` which
is the live global definition (first-loaded wins). They were removed from the
greenhouse file with zero runtime change. Their IDs and live values are
untouched in `items.json` and remain consumed by `GreenhouseExpansionCatalog
.CropCatalog`.

`item_seed_hardy_tuber`, `crop_hardy_tuber`, `item_seed_ash_grain`,
`crop_ash_grain`, `item_seed_biolum_mushroom`, `crop_biolum_mushroom`,
`item_seed_nutrient_algae`, `crop_nutrient_algae`,
`item_seed_medicinal_herb`, `crop_medicinal_herb`, `item_seed_leafy_green`,
`crop_leafy_green`, `item_seed_oilseed`, `crop_oilseed`,
`item_seed_cold_legume`, `crop_cold_legume`

Net effect: greenhouse file 30 entries on disk / 14 live → **30 entries, 30 live**.
