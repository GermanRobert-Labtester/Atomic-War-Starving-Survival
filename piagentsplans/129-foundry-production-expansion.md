# Plan 129 — Foundry Production Expansion (26 → 35 products)

## Goal (2 lines)
Expand the live `foundry_production.json` catalog from 26 products to 35.
The original 11-product count was stale when implementation began. The
accepted baseline is the current 26-product roster, including concurrent
recovery-melt and tooling additions; this plan is additive only.

## Why (P2)
- Reconciled: `foundry_production.json` has 26 products in the live
  `products` array before this plan's additions.
  Each has product_id, display_name, category, result_item_id,
  result_amount, ingredients (array of {item_id, amount}), labor_hours,
  cast_hours, fuel_units, water_litres, skill_target, quality_target,
  treaty_id, quota_amount, sink, notes, tags. `SilentFoundrySystem.Heat.cs`
  consumes it.
- The Foundry is the industrial-recovery pillar — survivors manufacturing
  tools, parts, and goods from salvage. The additive expansion broadens
  industrial, survey, power, railway, containment, and medical-manufacturing
  coverage without changing the runtime contract. The treaty_id and
  quota_amount fields allow foundry production to be tied to faction
  treaties (Plan 102).
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/foundry_production.json` (append 9 products,
  26 → 35)
- `Assets/StreamingAssets/Data/deep_lore_locations.json` (add two industrial
  loot references without removing concurrent location content)
- `Ashfall.Core.Tests/Foundry/FoundryPlan129IntegrationTests.cs`
- `Ashfall.Core.Tests/Foundry/FoundryExpansionProductTests.cs`
- `docs/production/FOUNDRY_PRODUCT_MATRIX.md`
- `docs/production/FOUNDRY_CONSUMER_MATRIX.md`
- `docs/plans/PLAN_129_FOUNDRY_PRODUCTION_CLOSEOUT.md`
- Read-only: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Heat.cs`
  (confirm product DTO and how result_item_id/ingredients resolve)
- Read-only: `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs`
  (confirm runtime consumption)

## Content grammar (per product)
- `product_id`: snake_case, prefix `foundry_prod_` (confirmed convention).
- `display_name`: evocative product name.
- `category`: product category (agricultural_tool, military, medical,
  infrastructure, tool, trade_good — confirm valid set in step 1).
- `result_item_id`: the item id produced (must resolve in the item
  catalog).
- `result_amount`: integer quantity produced per run.
- `ingredients`: array of {item_id (must resolve), amount}.
- `labor_hours` / `cast_hours` / `fuel_units` / `water_litres`: resource
  costs.
- `skill_target`: 0.0–1.0 skill requirement.
- `quality_target`: integer quality threshold.
- `treaty_id`: a treaty id if this product is treaty-quota-bound, or ""
  (must resolve if non-empty).
- `quota_amount`: integer quota if treaty-bound, or 0.
- `sink`: string (excess production sink — confirm in step 1).
- `notes`: string of design notes.
- `tags`: array of string tags.

## Steps
1. Read `SilentFoundrySystem.Heat.cs` to confirm the product DTO, how
   `result_item_id` and `ingredients[].item_id` resolve, and the valid
   `category` and `sink` values.
2. Read `SilentFoundryHeadlessDemo.cs` to confirm runtime consumption and
   that new product ids are additive (save-safe).
3. Inventory the reconciled 26-product baseline: category distribution,
   ingredient overlap, and downstream ownership.
4. Author these nine validated industrial products:
   - `foundry_prod_bronze_datum_plate` → `item_datum_plate_bronze`
   - `foundry_prod_flywheel_rotor_shaft` → `item_forged_rotor_shaft`
   - `foundry_prod_flywheel_containment_ring` → `item_containment_ring_steel`
   - `foundry_prod_culvert_brace` → `item_high_tensile_steel_culvert_brace`
   - `foundry_prod_sealed_lead_pig` → `item_sealed_lead_pig`
   - `foundry_prod_ground_anchor_spikes` → `item_hardened_ground_anchor_spikes`
   - `foundry_prod_turbine_blade_blank` → `item_superalloy_turbine_blade_blank`
   - `foundry_prod_rail_grinding_head` → `item_rail_grinding_head`
   - `foundry_prod_press_tooling_set` → `item_press_tooling_set`
5. Each product: distinct category, balanced costs, result_item_id and
   all ingredient item_ids resolving, treaty_id resolving if non-empty.
6. Cross-reference: every product_id unique; every result_item_id
   resolves; every ingredients[].item_id resolves; every treaty_id
   (if non-empty) resolves.
7. Preserve the existing treaty-bound product lanes. Do not assign new
  treaty IDs: the current runtime compliance rows only track the four
  authored obligations, and a new quota would be documentary rather than
  mechanically assessed.
8. Wire two new products to Plan 116 through the existing Riverside
  Steelworks and Eastern Power Substation loot tables.
9. Audit Plan 55 recipes. Do not duplicate the existing water-filter,
  gas-mask, battery, or surgical crafting paths; no recipe mutation is made
  where no new component has a truthful consumer contract.
10. Validate: `--data-integrity-selftest` (all item_ids and treaty_ids
    resolve).
11. xUnit: foundry production catalog loads exactly 35 products, all product_ids
    unique, all result_item_ids and ingredient item_ids resolving, all
    treaty_ids resolving.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The traps are `result_item_id` and `ingredients[].item_id`
resolution (step 6): every item id must resolve in the item catalog.
Confirm the item catalog has the target item ids before authoring, or
add the missing items first.

## Definition of Done
- `foundry_production.json` has exactly 35 products, all product_ids unique,
  all result_item_ids and ingredient item_ids resolving, all treaty_ids
  resolving, existing treaty products mechanically wired, 2 new outputs
  present in industrial loot, the Plan 55 duplication audit documented, and
  integrity + tests green.

## Follow-on
- Plan 102 (foundry accords) — treaty-bound products reference treaties.
- Plan 116 (deep lore locations) — foundry products in industrial loot.
- Plan 55 (recipes) — foundry products complement crafting recipes.
- Plan 99 (hardcore economy tuning) — foundry products get price tiers.
- Plan 105 (trade specialties) — foundry products match professions.
