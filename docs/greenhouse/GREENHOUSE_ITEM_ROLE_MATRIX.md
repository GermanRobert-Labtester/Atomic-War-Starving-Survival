# GREENHOUSE ITEM ROLE MATRIX — all 30 live entries

Combines the live-consumer status matrix (plan §43) with physicality/value
review (plan §47). Inventory = registered in the global item catalog.

## Preserved baseline (14)

| Item | Inventory | Crafting | Scavenging | Greenhouse live effect | Status |
|---|---|---|---|---|---|
| `item_seed_mushroom` | ✔ | – | – (via `seed_packets` abstraction) | **LIVE_CONSUMED** — planted via `CropCatalog` | live |
| `item_seed_tuber` | ✔ | – | – | **LIVE_CONSUMED** — planted | live |
| `item_seed_grain` | ✔ | – | – | **LIVE_CONSUMED** — planted | live |
| `item_seed_wheat` | ✔ | – | – | **LIVE_CONSUMED** — planted (unlock-gated) | live |
| `item_planter_box` | ✔ | – | – | none (constant only) | SCAVENGE_TRADE_ONLY |
| `item_grow_lamp` | ✔ | – | – | none (constant only) | SCAVENGE_TRADE_ONLY |
| `item_lead_glass_pane` | ✔ | – | – | none (constant only) | SCAVENGE_TRADE_ONLY |
| `item_blight_treatment` | ✔ | – | – | **LIVE_CONSUMED** — `TreatBlight` | live |
| `item_grow_medium` | ✔ | – | – | none (constant only) | SCAVENGE_TRADE_ONLY |
| `crop_mushroom` | ✔ | – | – | **LIVE_YIELD** — clean harvest | live |
| `crop_tuber` | ✔ | – | – | **LIVE_YIELD** — clean harvest | live |
| `crop_grain` | ✔ | – | – | **LIVE_YIELD** — clean harvest | live |
| `crop_wheat` | ✔ | – | – | **LIVE_YIELD** — clean harvest | live |
| `tainted_food` | ✔ | – | – | **LIVE_YIELD** — blighted harvest | live |

## Plan 91 additions (16)

| Item | Type | Stack | Weight | Trade | Crafting | Scavenging | Greenhouse live effect | Status |
|---|---|---:|---:|---:|---|---|---|---|
| `item_greenhouse_trowel` | Tool | 1 | 0.40 | 6 | **craft_greenhouse_trowel** | – | none | CRAFTING_ONLY / trade |
| `item_greenhouse_pruning_shears` | Tool | 1 | 0.35 | 10 | – | – | none | SCAVENGE_TRADE_ONLY |
| `item_greenhouse_watering_can` | Tool | 1 | 0.90 | 7 | **craft_greenhouse_watering_can** | – | none | CRAFTING_ONLY / trade |
| `item_greenhouse_hand_cultivator` | Tool | 1 | 0.50 | 8 | – | – | none | SCAVENGE_TRADE_ONLY |
| `item_greenhouse_compost` | Material | 10 | 1.50 | 3 | – | – | none | SCAVENGE_TRADE_ONLY (FUTURE_GREENHOUSE_HOOK: fertility) |
| `item_greenhouse_ash_fertilizer` | Material | 12 | 0.80 | 2 | – | – | none | SCAVENGE_TRADE_ONLY (FUTURE_GREENHOUSE_HOOK: fertility) |
| `item_greenhouse_fish_emulsion` | Material | 8 | 0.40 | 9 | – | – | none | SCAVENGE_TRADE_ONLY (FUTURE_GREENHOUSE_HOOK: fertility) |
| `item_greenhouse_insecticidal_soap` | Material | 8 | 0.30 | 6 | – | – | none | SCAVENGE_TRADE_ONLY (FUTURE_GREENHOUSE_HOOK: pests) |
| `item_greenhouse_sticky_traps` | Material | 10 | 0.15 | 4 | – | – | none | SCAVENGE_TRADE_ONLY (FUTURE_GREENHOUSE_HOOK: pests) |
| `item_greenhouse_pest_mesh` | Material | 4 | 0.60 | 8 | – | – | none | SCAVENGE_TRADE_ONLY (FUTURE_GREENHOUSE_HOOK: pests) |
| `item_greenhouse_drip_kit` | Material | 2 | 1.60 | 18 | **craft_greenhouse_drip_kit** | `table_loot_greenhouse` (rare) | none | CRAFTING + SCAVENGE + trade |
| `item_greenhouse_line_filter` | Filter | 6 | 0.20 | 12 | – | – | none | SCAVENGE_TRADE_ONLY (pairs with drip kit) |
| `item_greenhouse_catchment_kit` | Material | 2 | 1.80 | 14 | **craft_greenhouse_catchment_kit** | – | none | CRAFTING_ONLY / trade |
| `item_greenhouse_glass_pane` | Material | 3 | 2.60 | 16 | – | `table_loot_greenhouse` (uncommon) | none | SCAVENGE_TRADE_ONLY (repair) |
| `item_greenhouse_uv_sheeting` | Material | 5 | 1.20 | 13 | – | `table_loot_greenhouse` (uncommon) | none | SCAVENGE_TRADE_ONLY (repair) |
| `item_greenhouse_shade_cloth` | Material | 6 | 0.80 | 7 | – | – | none | SCAVENGE_TRADE_ONLY (FUTURE_GREENHOUSE_HOOK: climate) |

## Physicality / value review (plan §47)

- No heavy item rides a high stack (heaviest: pane 2.6 kg at stack 3;
  planter box baseline 8.0 kg at stack 6 precedes Plan 91).
- No hand tool exceeds 1.0 kg; all hand tools stack at 1.
- Trade spread across additions: 2–18 (no uniform 8–12 band). Rarest/highest
  utility (drip kit 18) > common amendments (ash 2, compost 3).
- Baseline comparison intact: seeds 4–12, crops 3–30, blight treatment 15.
- No same-function clones: the three pest items act by three distinct modes
  (contact wash / capture / barrier); trowel vs cultivator differ in
  function (planting lift vs crusted-row soil work), weight, value, and
  description; drip-line filter is distinct from `item_air_filter_hepa`
  (air), `item_ro_membrane` (potable RO), and `holdfast item_water_filter`
  (ceramic potable).
