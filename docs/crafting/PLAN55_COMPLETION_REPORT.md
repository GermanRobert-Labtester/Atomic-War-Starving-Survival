# Plan 55 Completion Report — Crafting Recipe Expansion

## Summary

- **Implementation date:** Plan 55 execution session (this repository state).
- **Baseline recipe count:** 73 (Case B — the 39-recipe baseline in the source
  plan was stale; other plans had already landed goods/preservation/armory
  content).
- **Final recipe count:** **81** (≥ the 80-recipe breadth target).
- **New recipes added:** 8 (5 food, 1 medicine, 2 ammunition).
- **Duplicate concepts avoided:** 24 source-roster concepts were already
  covered by the 73 baseline recipes and were **not** re-authored.
- **Concepts deferred with evidence:** 9 (no live consumer system; see
  `RECIPE_CATEGORY_MATRIX.md` reconciliation table).
- **Core code changes:** none.
- **Host code changes:** one minimal, justified bridge fix
  (`Main.World.SyncRoomStation`) repairing a proven reachability defect.

## Schema findings

- Actual fields: `id`, `recipeName`, `ingredients[{itemId,amount}]`,
  `resultItemId`, `resultAmount`, `craftingTimeHours`, `requiredStationId`
  (wrapped, `schema_version: 1`).
- Valid prefix: none enforced; Plan 55 used `craft_*` / `reload_*`.
- Valid stations: `workbench`, `stove`, `heater`, `water_purifier` (runtime
  registered), `distiller` (data-only, flagged), `""` (hand craft).
- Skill prerequisite support: **none** (efficiency multipliers only) — deferred.
- Research prerequisite support: **none** (breakthrough-item model) — deferred.
- Tier support: **none** — documentation only.
- Unlock authority: catalog membership; no unlock registry exists.

## Category results (baseline → final)

| Category | Baseline | Final | Delta |
|---|---|---|---|
| Food / preservation | 13 | 18 | +5 |
| Water | 8 | 8 | 0 (already saturated; remaining concepts consumer-less) |
| Medicine | 14 | 15 | +1 (`craft_splint`) |
| Instruments / tools | 4 | 4 | 0 (dosimeter family pre-existing; others consumer-less) |
| Weapons | 4 | 4 | 0 |
| Ammunition reloads | 5 | 7 | +2 (`reload_556`, `reload_762`) |
| Traps | 3 | 3 | 0 |
| Shelter / equipment | 14 | 14 | 0 (already saturated) |
| Vehicle / mechanical | 1 | 1 | 0 (no component consumer; documented substitution) |
| Zero-result sinks (allowlisted) | 6 | 6 | 0 (forbidden to extend) |
| Pharma lab (separate authority) | 26 | 26 | 0 |
| **Total recipes.json** | **73** | **81** | **+8** |

## Item changes (5 new, all with live consumers)

| Item | Why necessary | Consumer |
|---|---|---|
| `item_flatbread` | flatbread output; chain ingredient for travel rations | Food/needs system; `craft_travel_ration` |
| `item_boiled_roots` | boiled-roots output | Food/needs system |
| `item_vegetable_soup` | hot-meal output (morale differentiator) | Food/needs system |
| `item_pemmican` | dense expedition ration output | Food/needs system; expedition logistics |
| `item_travel_ration` | compact bundled ration output | Food/needs system; expedition logistics |

All are `ItemType.Food` with `hungerRestore > 0`, additive, standard fields.

## Progression integration

- Research-linked recipes: 0 authored (mechanism = breakthrough items;
  documented in `CRAFTING_RESEARCH_INTEGRATION.md`); advanced tier already
  occupied by pharma lab + breakthrough items + infrastructure sinks.
- Skill-linked recipes: 0 authored (efficiency-only surface); affinity map
  documented for future wiring.
- Station progression: workbench / stove / heater / water_purifier now all
  reachable through shelter rooms; hand crafts for wire traps and herbal tea.
- Deferred unsupported gates: skill-side (5-target) and research-side
  (10-target) explicitly deferred with no broken references.

## Authority decisions

- **Repair boundary:** components/direct-bills only; no condition bypass
  (`CRAFTING_REPAIR_BOUNDARY.md`). Vehicle recipes omitted — repair consumes
  no items (§55D.12 substitution).
- **Pharma boundary:** one item added (`craft_splint`); all advanced medicine
  left to the pharma lab; iodine/chelation semantics untouched.
- **Ammo boundary:** live calibers only; 1:1 casing conservation pinned by
  test; component provenance repaired via loot tables (`AMMO_RELOADING_CONTRACT.md`).
- **Shelter boundary:** no components authored — shelter systems consume raw
  bills directly; avoided double-charging.

## Economy results

- Recipe graph: acyclic for all new recipes; no new SCC; container duplication: none.
- Worst positive craft margins: `reload_762` +72, `reload_556` +62 — bounded
  by loot-only uncommon primers/powder (no vendor loop possible).
- All food/medicine margins negative (no buy→craft→sell printer).
- Nutrition conservation: bounded gains paying fuel/water/labor; preservation
  recipes trade bulk for portability, not calories.
- Long-run: no simulation harness was added for this pass; the value/nutrition
  audits plus existing economy tests stand in. (Deferred: a dedicated
  crafting-telemetry sweep, §16.)

## Persistence

- Additive catalog + additive items + additive loot entries; old saves load
  unchanged; in-progress craft restore path untouched; no unlock state to
  migrate (none exists).

## Verification

See `PLAN55_REGRESSION_MATRIX.md` — build (0/0), tests 6,523/6,523,
data-integrity 0 findings/208 catalogs, bridge selftest exit 0,
content-utilization gate PASS.

## Remaining risks / deferred work

1. `distiller` station remains ownerless — 4 legacy recipes unreachable
   (bounded by test); needs a distillation room/station-authority decision.
2. `antiseptic` dangling reference in `medical_texts.json` (medical authority).
3. Skill-gated and research-gated recipes deferred pending a general
   recipe-side gate surface.
4. Vehicle service kits deferred until vehicle repair consumes items.
5. Legacy reload recipes are value sinks (10 shells → 1 round convention);
   rebalancing them is legacy-rebalance work, deliberately not mixed into this
   catalog-growth pass.
6. Moonshine gate (`SetMoonshineGate`) has no host wiring (pre-existing).
