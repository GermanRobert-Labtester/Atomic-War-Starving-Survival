# Plan 55 Baseline — Crafting Catalog Expansion

**Date of baseline capture:** implementation session (Plan 55).
**Verification state at capture:** build clean (0 warnings/0 errors), 6,513 tests green, `--data-integrity-selftest` 0 findings across 208 catalogs.

## Case determination (Plan 55 §1.3)

**Case B.** The catalog already contained **73 recipes**, not 39 — other plans
(13A goods expansion, Plan 10-era armory additions, preservation batch content)
had already landed content. Per §1.3-B, all 73 recipes are preserved; Plan 55
adds the *missing* Plan-55 coverage without semantic duplicates. Final count:
**81 recipes** (≥ the intended 80-recipe breadth target).

## Baseline answers (Task 55A exit gate)

| Question | Answer (repository truth) |
|---|---|
| Exact recipe schema | `id`, `recipeName`, `ingredients[{itemId, amount}]`, `resultItemId`, `resultAmount`, `craftingTimeHours`, `requiredStationId` — wrapped as `{schema_version: 1, recipes: [...]}` (camelCase) |
| Recipe count at baseline | 73 |
| Valid prefix | No enforced prefix. Dominant conventions: `craft_*`, `reload_*`, plus legacy bare verbs and `recipe_*`. Plan 55 uses `craft_*` / `reload_*`. |
| Valid stations | Data references: `workbench`, `stove`, `distiller`, `heater`, `water_purifier`, `""` (hand craft). Runtime registration authority: `CraftingHostSession` (workbench) + `Main.World.SyncCraftingStationsFromShelter`. |
| Station identity type | String IDs on `CraftingStation` objects synced from shelter rooms/machines at runtime. Not item IDs, not room IDs directly. |
| Skill gates supported? | **No recipe-side gating.** Skills affect crafting only through `SetCrafterCostMultiplier` / `SetCrafterCraftTimeMultiplier` (efficiency). |
| Research gates supported? | **No recipe-side gating.** `ResearchSystem` completion awards the node's `breakthroughItem` into inventory (`CraftingHostSession` wiring). No recipe-unlock payload exists. |
| Who owns recipe discovery/unlock? | No unlock registry exists — every catalog recipe is always known. Visibility = catalog membership. |
| Is `tier` a real field? | No. Kept in documentation only. |
| Ingredient consumption | Atomic: full validation (`CanCraft` → station, result-gate, `ValidateTransaction`, `CanAdd`) then `TryExecuteTransaction` in `StartCraft`. |
| Output production | Once per completed `ActiveCraft` in `CompleteCraft`; overflow → `OverflowStash`, else full ingredient refund + station repair. |
| Failure handling | Failed validation mutates nothing (preview/execute share the same validation path). |
| Craft jobs | Time-based (`craftingTimeHours`), persisted as `{RecipeId, HoursRemaining, CrafterId}`; restored via recipe lookup; unknown IDs silently dropped (old-save safe). |
| Zero-result sinks | Loader **throws** on `resultAmount <= 0` outside a 6-recipe legacy allowlist. Plan 55 adds none. |
| Loader strictness | Unknown ingredient IDs are *silently skipped* at load — Plan 55 tests therefore assert resolution explicitly. |

## Verification baseline commands

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # PASS (0/0)
godot --headless --path . -- --data-integrity-selftest      # PASS (0 findings, 208 catalogs)
godot --headless --path . -- --bridge-selftest              # PASS (exit 0)
godot --headless --path . -- --content-utilization-selftest # PASS
```
