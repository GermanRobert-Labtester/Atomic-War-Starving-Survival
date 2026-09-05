# Plan 55 Economy Matrix & Audit

## Value audit (tradeValue, from the merged item catalog)

| Recipe | Input value | Output value | Margin | Verdict |
|---|---|---|---|---|
| `craft_flatbread` | 2×7 + 15 + 14 = 50 | 2×8 = 16 | −34 | value sink; the product is edible staple conversion, not trade goods |
| `craft_boiled_roots` | 3×1 + 15 + 14 = 32 | 3×4 = 12 | −20 | value sink; scarcity survival food |
| `craft_vegetable_soup` | 3×5 + 2×5 + 15 + 14 = 49 | 3×12 = 36 | −13 | near-neutral; morale 4 is the differentiator |
| `craft_pemmican` | 2×18 + 2×7 + 8 = 58 | 3×14 = 42 | −16 | bounded; portability (0.15 weight vs 0.8) is the value |
| `craft_travel_ration` | 18 + 2×10 + 2×8 = 54 | 2×22 = 44 | −10 | bounded; weight 1.1 → 0.8 |
| `craft_splint` | 2 + 2×1.2 + 6 = 10.4 | 9 | −1.4 | ≈neutral vs buying; crafting from scrap is the alternative |
| `reload_556` | 10×1 + 10×3 + 8 = 48 | 10×11 = 110 | **+62** | positive — see ammo audit below |
| `reload_762` | 10×1 + 10×3 + 8 = 48 | 10×12 = 120 | **+72** | positive — see ammo audit below |

**No buy→craft→sell arbitrage loop exists in the Plan-55 food/medicine
additions (all margins negative).** The reloading margins are the intended
craft value and are bounded by scarcity, not by price walls:

- `reloading_primer` and `smokeless_powder` appear in **no trade catalog** —
  acquisition is loot-only (`table_loot_military_depot` uncommon,
  `table_loot_police_station` uncommon/rare — added by Plan 55 to repair the
  pre-existing zero-provenance defect that starved all 5 legacy reload recipes).
- `empty_brass_shell` is loot-only (3 tables); no recipe or system produces it
  (no free brass — conservation pinned by test).
- Legacy reload recipes remain untouched (they are value sinks under the
  existing convention; rebalancing them is legacy-rebalance work, out of
  Plan 55 scope).

## Nutrition conservation (game abstraction: hungerRestore)

| Recipe | Input hunger | Output hunger | Δ |
|---|---|---|---|
| `craft_flatbread` | 2×20 = 40 | 2×22 = 44 | +4 (+fuel/water/labor cost) |
| `craft_boiled_roots` | 3×8 = 24 | 3×14 = 42 | +18 (paid: water + fuel; consistent with cook_meat convention) |
| `craft_vegetable_soup` | 3×8 + 2×12 = 48 | 3×26 = 78 | +30 (paid: water + fuel; morale 4 hot-meal differentiator) |
| `craft_pemmican` | 2×24 + 2×20 = 88 | 3×30 = 90 | +2 (portability is the value) |
| `craft_travel_ration` | 24 + 2×14 + 2×22 = 96 | 2×45 = 90 | −6 (weight 1.1 → 0.8 is the value) |

No recipe multiplies nutrition unboundedly; every conversion pays water+fuel
and/or labor, and preservation recipes trade bulk for shelf-stable weight.

## Water audit

No new water recipes. Existing coverage (8 recipes + pharma overlap) already
spans filtration (craft/consume/recondition), boiling, RO/desalination
component sinks. The unimplemented concepts (rain collector, solar still,
chemical dosing) have **no consumer system**; authoring them would create
orphan outputs (§6.4 rejection reason).

## Recipe-graph analysis

- **No new cycles.** `craft_travel_ration` consumes `item_flatbread` (output
  of `craft_flatbread`) — a depth-2 chain, acyclic, net value-negative at each
  step.
- No recipe's output is a required ingredient of its own ingredient chain
  (`item_flatbread` → `craft_travel_ration` only flows forward).
- No container duplication: tin cans/jars are consumed (existing convention);
  Plan 55 introduces no containers.
- Recursive SCC check over the 81-recipe graph: only the pre-existing
  `craft_textile_repair` (cloth×2+leather+tape → cloth×4) and filter
  reconditioning loops exist; both pre-date Plan 55 and consume non-renewable
  inputs (leather/duct tape, chemicals) that bound the loop. Plan 55 adds no
  new cycle.

## Opportunity-cost notes

Reloading competes for workbench time and expedition capacity; food
preservation competes for stove time and fuel. Advanced pharma (station +
chemist + scarce precursors) remains the strongest late-game crafting route,
keeping scavenging hospitals and trade relevant. No Plan-55 recipe collapses
external demand for loot or trade.
