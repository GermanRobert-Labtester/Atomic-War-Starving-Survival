# Plan 55 Regression Matrix

## Commands run (canonical verification pipeline)

| # | Command | Result |
|---|---|---|
| 1 | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** — 0 errors, 0 warnings |
| 2 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** — 6,523 / 6,523 (includes 10 new Plan-55 tests) |
| 3 | `dotnet build Ashfall.csproj` | **PASS** — 0 errors, 0 warnings |
| 4 | `godot --headless --path . -- --data-integrity-selftest` | **PASS** — 0 findings (10,056 ids authored; 208 catalogs, 0 errors/warnings) |
| 5 | `godot --headless --path . -- --bridge-selftest` | **PASS** — exit 0 |
| 6 | `godot --headless --path . -- --content-utilization-selftest` | **PASS** — CI gate PASS |

## New test coverage (`Ashfall.Core.Tests/Plan55CraftingCatalogTests.cs`, 10 tests)

1. `Catalog_reaches_the_80_recipe_breadth_target` — 81 ≥ 80
2. `All_plan55_recipe_ids_are_present_and_unique` — 8 IDs, no duplicates
3. `Every_recipe_ingredient_and_result_resolves_and_is_positive` — all 81 recipes
4. `Plan55_recipes_only_use_registered_station_ids` — strict for Plan-55 set
5. `Legacy_distiller_gap_is_bounded_to_known_recipes` — 4 flagged recipes pinned
6. `Plan55_food_outputs_resolve_as_food_items` — 5 items, Food type, hunger > 0
7. `Plan55_ammo_reload_recipes_consume_casings_one_to_one` — no free casings/primers/powder
8. `Reloading_components_have_live_acquisition_paths` — provenance pinned
9. `Splint_recipe_output_matches_medical_treatment_requirement` — canonical `splint` id
10. `Plan55_food_output_values_do_not_exceed_ingredient_values` — no arbitrage

## Atomicity / gate behavior (covered by existing suite, untouched by Plan 55)

`CraftingSystemTests` and host session tests continue to pin: failed
validation consumes nothing, preview/execute share validation, output produced
exactly once, overflow → stash-or-refund, save round-trip of active crafts.
Plan 55 modified **none** of that code.

## Not weakened

No validator, test, selftest, or exclusion was relaxed. The loader's
sink-pattern rejection, strict-mode integrity validator, and all pre-existing
tests pass unchanged.
