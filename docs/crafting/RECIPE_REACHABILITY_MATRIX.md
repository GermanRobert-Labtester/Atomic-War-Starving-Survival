# Plan 55 Reachability Matrix

Every Plan-55 recipe traced through:
**definition → catalog knowledge → station → ingredient acquisition → craft transaction → output → downstream use.**

Recipe knowledge is static catalog membership (see `RECIPE_UNLOCK_AUTHORITY.md`),
so the reachability questions are station, ingredients, and consumer.

| Recipe | Station path | Ingredient provenance | Consumer | Reachable |
|---|---|---|---|---|
| `craft_flatbread` | `stove` ← `room_kitchen` (Plan 55 bridge) | `crop_ash_grain` greenhouse harvest; `clean_water`/`fuel` starter economy + recipes | Food/needs system | ✅ |
| `craft_boiled_roots` | `stove` ← `room_kitchen` | `roots` surface foraging; water/fuel | Food/needs system | ✅ |
| `craft_vegetable_soup` | `stove` ← `room_kitchen` | greenhouse harvest; water/fuel | Food/needs system | ✅ |
| `craft_pemmican` | `workbench` ← `room_workshop` | `item_smoked_meat` (craft_smoked_meat_rations), grain, `item_preservation_salt` (loot) | Food/needs system | ✅ |
| `craft_travel_ration` | `workbench` ← `room_workshop` | `item_smoked_meat`, `item_pickled_tubers` (craft_pickled_tubers), `item_flatbread` (Plan 55) | Food/needs system | ✅ |
| `craft_splint` | `workbench` ← `room_workshop` | `wooden_plank`, `cloth`, `duct_tape` (loot/starting economy) | `medical_fracture` treatment | ✅ |
| `reload_556` | `workbench` ← `room_workshop` | `empty_brass_shell` (3 loot tables), `reloading_primer` + `smokeless_powder` (depot/police tables — provenance added by Plan 55) | `combat_catalog.json` 5.56mm weapons | ✅ |
| `reload_762` | `workbench` ← `room_workshop` | same as above | `combat_catalog.json` 7.62mm weapon | ✅ |

## Pre-existing reachability defects (found during baseline; status)

| Defect | Evidence | Status |
|---|---|---|
| `stove`/`heater`/`water_purifier` stations never registered → ~30 recipes dead at runtime | Only `workbench` was synced/seeded (`CraftingHostSession`, `Main.World.cs`) | **Fixed** — Plan 55 bridge `SyncRoomStation` (room_kitchen→stove, room_generator→heater, room_filtration→water_purifier) |
| `distiller` station has no owner | No registration path anywhere; no shelter room for distillation | **Flagged** — 4 legacy recipes (`brew_lethe_substitute`, `craft_distilled_spirits`, `craft_fuel_gel`, `craft_antiseptic_solution`) unreachable; bounded by test |
| `reloading_primer`/`smokeless_powder` zero provenance | Present only in items.json/recipes.json; absent from all loot/trade data | **Fixed** — added to military depot & police station scavenging tables |
| `antiseptic` dangling item reference | `medical_texts.json` requires it; item does not exist (`antiseptic_1l_of_1l` does) | **Flagged** — medical authority scope, not crafting |
| `moonshine` gate never wired | `SetMoonshineGate` has no caller in src/ | **Flagged** — pre-existing; no Plan-55 recipe is moonshine |
