# Plan 126 — Completion Report

## Summary

`crossing_items.json` now contains exactly 25 entries: the original 11 plus the fourteen requested IDs. The catalog expansion is data-first and introduces no new item type, inventory subsystem, save section, trade simulator, or per-item Core behavior.

Changed files:

- `Assets/StreamingAssets/Data/crossing_items.json`
- `Assets/Ashfall.Core/CrossingHeadlessDemo.cs` — existing smoke assertion updated from the old 11-item floor to the exact 25-item contract.
- `Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs`
- Plan 126 evidence documents in this directory.

## Final roster

| ID | Type | Primary role |
| --- | --- | --- |
| `item_arbitration_token` | Quest | hearing access |
| `item_charter_stamp` | Quest | document validation |
| `item_weighbridge_chit` | Trade | verified weight record |
| `item_smuggled_medicine` | Medical | off-ledger treatment stock |
| `item_crossing_bread` | Food | local staple |
| `item_lamp_oil_crossing` | Fuel | local utility commodity |
| `item_filtered_water_crossing` | Water | certified drinking water |
| `item_quarantine_bands` | Quest | screening status marker |
| `item_granary_receipt` | Trade | grain claim |
| `item_smugglers_ledger` | Quest | contraband evidence |
| `item_rejection_notice` | Quest | proof of refused claim |
| `item_crossing_map` | Quest | route information |
| `item_black_market_pouch` | Trade | off-ledger trade object |
| `item_charter_draft` | Quest | political leverage |

Exact numeric values and effects are pinned in `CROSSING_ITEM_TYPE_EFFECT_MATRIX.md`.

## Repository-truth deviations

1. The roadmap requested four exact Plan 120 item references, but live `crossing_factions.json` uses macro tags only. No unsupported item-ID fields were added.
2. The roadmap requested three Plan 115 item references, but the live `cost_items` DTO has no discovered resolver/consumer in the Crossing runtime. No new potentially dead costs were added.
3. The roadmap requested two explicit cross-expansion links and Plan 116 loot references. The global item registry accepts all fourteen IDs, but no landed recipient/source was proven safe. Those links are staged.
4. Smuggled Medicine is H0, using the existing global `healthEffect` field. No new health semantics were created.
5. Lamp Oil is L2: the canonical `Fuel` type exists, but no live Crossing lamp consumer was found. The item does not claim a lamp-specific effect.
6. Crossing Map and Off-Ledger Pouch remain descriptive items because no generic map, route, concealment, or container behavior consumes them.

## Verification

- Data integrity: PASS, 0 errors.
- Crossing selftest: PASS, 40/40.
- Plan 126 tests: PASS, 7/7.
- Existing Crossing-filtered tests: PASS, 97/97.
- Core build: PASS, 0 warnings/errors.
- Content utilization: PASS, 0 orphaned catalogs.
- Fast CI: baseline blocked by unrelated whitespace findings; no Plan 126 error was reported.

## Status

The exact 25-item catalog expansion is implemented and validated. Downstream item-level acquisition/consumption wiring remains intentionally staged until the corresponding faction, encounter, loot, or trade authorities expose real item-ID contracts.
