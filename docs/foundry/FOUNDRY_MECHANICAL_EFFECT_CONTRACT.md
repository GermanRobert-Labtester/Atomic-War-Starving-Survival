# Foundry Mechanical Effect Contract

The roadmap field name `mechanical_effect` is not part of the live policy
schema. The executable data surface is:

1. `standing_delta`: accumulated and clamped by
   `SilentFoundrySystem`/`SilentFoundryConsequenceState`, then mirrored to the
   existing `FactionStanceEngine`.
2. `market_modifiers[]`: copied into the applied record and sent to
   `MarketSystem.AdjustDemand(good_id, demand_delta)` by the host.

There are no composite effect objects, access tokens, production tokens,
contamination tokens, or global-faction shorthand values in this catalog.

## Supported goods used by the 15 rows

All declared IDs resolve in `economy_goods.json`:

`item_foundry_brine_pipe`, `item_foundry_ice_anchor`, `coal`, `fuel`,
`clean_water`, `water_filter`, and `scrap_metal`.

## Validation and failure behavior

- Duplicate `(treaty_id, outcome)` rows are collected as catalog errors.
- Unknown outcomes are collected as catalog errors.
- Missing treaty/faction/good references are rejected by the Plan 103 test
  matrix before content is considered complete.
- A missing policy is a neutral lookup miss; it does not invent a consequence.
- The applied record is written once per treaty assessment day and restored
  through the existing save envelope.

No new Core effect dispatcher was introduced.
