# GREENHOUSE CRAFTING BINDINGS (plan §37-38)

Four new recipes appended to `Assets/StreamingAssets/Data/recipes.json`
(84 → 88). All use the existing recipe schema and the `workbench` station
(most common station, 55 pre-existing recipes). No recipe IDs collided.

| Recipe ID | Output | Ingredients (trade value each) | Time | Output trade |
|---|---|---|---:|---:|
| `craft_greenhouse_trowel` | `item_greenhouse_trowel` | `scrap_metal`×1 (1.2) + `wood_block`×1 (2) | 0.5h | 6 |
| `craft_greenhouse_watering_can` | `item_greenhouse_watering_can` | `scrap_metal`×2 (2.4) | 1.0h | 7 |
| `craft_greenhouse_drip_kit` | `item_greenhouse_drip_kit` | `rubber_hose`×1 (3) + `plastic_material`×1 (4) + `mechanical_parts`×1 (3) | 2.0h | 18 |
| `craft_greenhouse_catchment_kit` | `item_greenhouse_catchment_kit` | `plastic_material`×2 (8) + `wood_block`×1 (2) + `mechanical_parts`×1 (3) | 2.0h | 14 |

## Design rationale

- **Outputs resolve globally.** The crafting system resolves `resultItemId`
  against the merged item registry (Model A), so greenhouse-catalog outputs
  need no cross-catalog plumbing. Pinned by
  `Crafting_GreenhouseRecipeOutputsResolveInGlobalRegistry`.
- **Ingredients are proven staples** used by pre-existing recipes and found
  in `table_loot_warehouse`/`table_loot_farm`. All resolve
  (`Crafting_GreenhouseRecipeIngredientsResolve`).
- **No arbitrage** (plan §46): every output's trade value covers ≥75% of its
  ingredient cost, and `craftingTimeHours` prices the labor. The drip kit
  (10 input → 18 output) is the best margin — appropriate for the most
  valuable, rarest-to-scavenge output. Pinned by
  `Crafting_GreenhouseOutputsNotPricedBelowInputValue`.
- **Grounded recipes** (plan §38): metal + wood = trowel; folded sheet metal
  = can; hose + sheet + fittings = drip kit; sheet + frame + fittings =
  catchment kit. No chemistry, no hazardous instructions.
- Not every greenhouse tool is craftable: shears, cultivator, and the
  consumable/barrier supplies remain trade/salvage content, keeping
  crafting from becoming a universal vending machine.

## Boundary

Recipes are the *crafting system's* authority. No crafting logic was added;
Plan 91 only appended data rows in the existing schema.
