# GREENHOUSE ITEM ACQUISITION MATRIX (plan §45)

Every Plan 91 addition needs at least one live acquisition path.
Crafting recipes are live `recipes.json` entries; scavenging is the Plan 46
weighted-table authority (`scavenging_tables.json`); trade = any item with a
`tradeValue` is barterable through the trade economy.

| Item | Craft | Scavenge | Trade | Reachable? |
|---|---|---|---|---|
| `item_greenhouse_trowel` | ✔ `craft_greenhouse_trowel` | – | ✔ (6) | ✔ |
| `item_greenhouse_pruning_shears` | – | – | ✔ (10) | ✔ trade-only |
| `item_greenhouse_watering_can` | ✔ `craft_greenhouse_watering_can` | – | ✔ (7) | ✔ |
| `item_greenhouse_hand_cultivator` | – | – | ✔ (8) | ✔ trade-only |
| `item_greenhouse_compost` | – | – | ✔ (3) | ✔ trade-only (Rot Farmers' Compost Yard lore source) |
| `item_greenhouse_ash_fertilizer` | – | – | ✔ (2) | ✔ trade-only (shelter byproduct lore) |
| `item_greenhouse_fish_emulsion` | – | – | ✔ (9) | ✔ trade-only (trap-fish economy) |
| `item_greenhouse_insecticidal_soap` | – | – | ✔ (6) | ✔ trade-only |
| `item_greenhouse_sticky_traps` | – | – | ✔ (4) | ✔ trade-only |
| `item_greenhouse_pest_mesh` | – | – | ✔ (8) | ✔ trade-only |
| `item_greenhouse_drip_kit` | ✔ `craft_greenhouse_drip_kit` | ✔ `table_loot_greenhouse` (rare) | ✔ (18) | ✔ |
| `item_greenhouse_line_filter` | – | – | ✔ (12) | ✔ trade-only |
| `item_greenhouse_catchment_kit` | ✔ `craft_greenhouse_catchment_kit` | – | ✔ (14) | ✔ |
| `item_greenhouse_glass_pane` | – | ✔ `table_loot_greenhouse` (uncommon) | ✔ (16) | ✔ |
| `item_greenhouse_uv_sheeting` | – | ✔ `table_loot_greenhouse` (uncommon) | ✔ (13) | ✔ |
| `item_greenhouse_shade_cloth` | – | – | ✔ (7) | ✔ trade-only |

Notes:

- **No item is unreachable.** Minimum path for every addition: trade.
- Trade-only items are deliberately the lower-value/more-common supplies;
  the high-utility items (drip kit, catchment kit, trowel, watering can) are
  craftable, and the bulky/scarce repair materials (pane, sheeting) are
  scavengeable from `table_loot_greenhouse` — the "worth carrying back from
  the nursery" loop (plan §79).
- Plan 76 agricultural destinations reach these supplies **through** the
  Plan 46 table authority (no direct destination loot lists were added).
- Recipe inputs are all common scavenge staples (`scrap_metal` 1.2,
  `wood_block` 2, `rubber_hose` 3, `plastic_material` 4,
  `mechanical_parts` 3 — trade values), so no acquisition path depends on
  items rarer than the output (plan §46 arbitrage reviewed:
  `Crafting_GreenhouseOutputsNotPricedBelowInputValue`).
