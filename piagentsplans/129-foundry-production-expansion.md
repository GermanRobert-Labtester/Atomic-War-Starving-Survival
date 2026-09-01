# Plan 129 — Foundry Production Expansion (11 → 20 products)

## Goal (2 lines)
Expand `foundry_production.json` from 11 products to 20. The Foundry
production catalog (`SilentFoundrySystem` confirmed live via
`SilentFoundryHeadlessDemo.cs` and `SilentFoundrySystem.Heat.cs`) defines
manufacturable products with ingredients, labor/fuel/water costs, skill and
quality targets, treaty quotas, and tags. 11 products for the industrial
recovery pillar is thin.

## Why (P2)
- Verified: `foundry_production.json` has 11 products in `products` array.
  Each has product_id, display_name, category, result_item_id,
  result_amount, ingredients (array of {item_id, amount}), labor_hours,
  cast_hours, fuel_units, water_litres, skill_target, quality_target,
  treaty_id, quota_amount, sink, notes, tags. `SilentFoundrySystem.Heat.cs`
  consumes it.
- The Foundry is the industrial-recovery pillar — survivors manufacturing
  tools, parts, and goods from salvage. 11 products means the foundry
  feels limited; the agricultural, military, medical, and infrastructure
  categories need more producible goods. The treaty_id and quota_amount
  fields allow foundry production to be tied to faction treaties (Plan
  102).
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/foundry_production.json` (expand `products`
  11 → 20)
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
3. Inventory the 11 existing products: category distribution, ingredient
   overlap. Identify which categories lack products.
4. Author 9 new products:
   - `foundry_prod_water_filter`: infrastructure; result
     `item_water_filter`; ingredients: cloth, charcoal, scrap_metal;
     high labor, moderate fuel; treaty-optional.
   - `foundry_prod_gas_mask_filter`: medical; result
     `item_gas_mask_filter`; ingredients: cloth, charcoal; moderate
     labor; treaty-optional.
   - `foundry_prod_barbed_wire`: military; result `item_barbed_wire`;
     ingredients: scrap_metal; low labor, low fuel; treaty-bound to
     garrison.
   - `foundry_prod_cooking_stove`: infrastructure; result
     `item_cooking_stove`; ingredients: scrap_metal, scrap_wood;
     moderate labor; treaty-optional.
   - `foundry_prod_water_boiler`: infrastructure; result
     `item_water_boiler`; ingredients: scrap_metal, copper; high labor;
     treaty-optional.
   - `foundry_prod_sewing_kit`: tool; result `item_sewing_kit`;
     ingredients: scrap_metal, cloth; low labor; treaty-optional.
   - `foundry_prod_battery_cell`: trade_good; result `item_battery_cell`;
     ingredients: copper, acid; high labor, high fuel; treaty-bound to
     hydro barons.
   - `foundry_prod_windlass_part`: infrastructure; result
     `item_windlass_part`; ingredients: scrap_metal, wood_block;
     moderate labor; treaty-bound to Rebuilders.
   - `foundry_prod_field_surgical_kit`: medical; result
     `item_field_surgical_kit`; ingredients: scrap_metal, cloth,
     antibiotics; very high labor; treaty-optional.
5. Each product: distinct category, balanced costs, result_item_id and
   all ingredient item_ids resolving, treaty_id resolving if non-empty.
6. Cross-reference: every product_id unique; every result_item_id
   resolves; every ingredients[].item_id resolves; every treaty_id
   (if non-empty) resolves.
7. Wire 3 new products to Plan 102 (foundry accords — treaty-bound
  products reference treaties).
8. Wire 2 new products to Plan 116 (deep lore locations — foundry
  products appear in industrial location loot tables).
9. Wire 2 new products to Plan 55 (recipes — foundry products
  complement crafting recipes).
10. Validate: `--data-integrity-selftest` (all item_ids and treaty_ids
    resolve).
11. xUnit: foundry production catalog loads 20 products, all product_ids
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
- `foundry_production.json` has 20 products, all product_ids unique, all
  result_item_ids and ingredient item_ids resolving, all treaty_ids
  resolving, 3 wired to foundry accords, 2 to deep lore locations, 2 to
  recipes, integrity + tests green.

## Follow-on
- Plan 102 (foundry accords) — treaty-bound products reference treaties.
- Plan 116 (deep lore locations) — foundry products in industrial loot.
- Plan 55 (recipes) — foundry products complement crafting recipes.
- Plan 99 (hardcore economy tuning) — foundry products get price tiers.
- Plan 105 (trade specialties) — foundry products match professions.
