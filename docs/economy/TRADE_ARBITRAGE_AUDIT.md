# Trade Arbitrage Audit — Plan 61

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

## Structural finding: scenario tables cannot create arbitrage

The plan's arbitrage risk model (buy at merchant A → sell at merchant B → infinite
loop) assumes **directional prices** and **refreshing stock**. This seam has neither:

1. **One worth per item.** A table line's `unit_price` is the item's worth, identical
   on both edges of every table. There is no buy price vs sell price, so no spread
   to exploit. Test gate: `Catalog_UnitPricesAreGloballyConsistentPerItem` +
   `Catalog_CrossScenarioPriceSpreadsStayWithinAntiArbitrageBand` (spread must be
   < 0.01 across all 15 scenarios).
2. **Static tables.** No stock refresh, regeneration, or depletion exists; nothing
   can be repeated for compounding gain.
3. **No currency loop.** Tables are barter comparisons (two edges vs the
   arbitrator's scale); confirming routes through `ITradeIntentSink`/`ITradeExecutionSink`
   and does not mint value.

## Per-pair review of the plan's priority loops

| Loop | Verdict | Evidence |
|---|---|---|
| bulk dealer ↔ caravan (`crate_lot` ↔ `long_road_caravan`) | **SAFE** | shared staples (`item_canned_grain_stew`, `item_grain_flour`) carry identical worths (15/14) in both tables |
| quartermaster ↔ black market (`depot_window` ↔ `back_room_exchange`) | **SAFE** | shared good `filter_pack` worth 28 in both; no directional spread |
| smuggler ↔ settlement trader (`border_runner` ↔ any) | **SAFE** | `fuel` worth 40 everywhere it appears (4 scenarios) |
| desperate survivor ↔ bulk dealer (`last_vials` ↔ `crate_lot`) | **SAFE** | no shared goods; shared-class goods (`clean_water` 22 / `clean_water_jug` 30) are distinct items with stable worths |
| two settlements with different defaults | **N/A (deferred)** | no settlement defaults exist |
| black market ↔ black market (`back_room_exchange` ↔ `ledgerless_broker`) | **SAFE** | no shared goods; both premium tables price from the same canonical worths |

## High-worth consistency spot checks (canonical worths now locked by test)

`canned_food` 18 · `clean_water` 22 · `fuel` 40 · `filter_pack` 28 ·
`item_grain_flour` 14 · `item_canned_grain_stew` 15 · `item_salted_meat` 16 ·
`clean_water_jug` 30 · `item_honey_pot` 18 · `item_trade_salt_sack` 10

## Exploit vectors checked and closed

- **Scenario cycling reroll:** impossible — static content, no selection RNG.
- **Free goods:** impossible — no stock generation.
- **Negotiation stacking to zero/negative price:** not applicable — no scenario
  negotiation math exists; tell selection is presentation-only. Bio values derive
  from `TradePricing` and cannot be authored down.
- **Modifier confusion:** the plan's feared `price_modifier` direction bug cannot
  occur — the field does not exist; worths are authored and test-locked.
