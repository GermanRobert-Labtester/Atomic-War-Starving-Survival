# Recipe Schema Contract (Plan 55)

The authoritative, loader-enforced schema for `recipes.json`. Any field not
listed here is **not consumed by any runtime code** and must not be authored.

```jsonc
{
  "schema_version": 1,
  "recipes": [
    {
      "id": "craft_flatbread",            // string, snake_case, unique; no enforced prefix
                                          // (conventions: craft_*, reload_*, legacy bare verbs)
      "recipeName": "Bake Ash-Grain Flatbread",
      "ingredients": [
        { "itemId": "crop_ash_grain",     // must resolve in the MERGED item catalog
          "amount": 2 }                   // loader clamps <=0 to 1; author positive only
      ],
      "resultItemId": "item_flatbread",   // must resolve in the merged item catalog
      "resultAmount": 2,                  // MUST be > 0 (loader throws on 0 outside the
                                          // 6-recipe LegacyZeroResultAllowlist)
      "craftingTimeHours": 0.8,           // > 0 (loader clamps <=0 to 1)
      "requiredStationId": "stove"        // "" = hand craft; otherwise must be a station
                                          // registered by the host runtime
    }
  ]
}
```

## Loader behavior (`RecipeCatalogLoader`)

- Unknown ingredient IDs are **silently skipped** at load — the recipe still
  loads with fewer ingredients. Plan 55 tests assert explicit resolution.
- Unknown result IDs load as a null result (`catalog.Get(...)!`); output is
  then skipped at completion. Plan 55 tests assert result resolution.
- `resultAmount <= 0` with a non-empty `resultItemId` **throws** unless the id
  is in `LegacyZeroResultAllowlist` (6 legacy non-inventory action recipes).
- Wrap/unwrap goes through `CatalogLocator.LoadWrappedList`.

## Fields deliberately NOT authored (no runtime consumer)

| Proposed field | Status | Repository-native substitute |
|---|---|---|
| `skill_prerequisite` | ❌ no consumer | Skill effects = crafter cost/time multipliers; documented in `CRAFTING_SKILL_INTEGRATION.md` |
| `research_prerequisite` | ❌ no consumer | Research completes → grants `breakthroughItem` (a rare ingredient); advanced gating is economic |
| `tier` | ❌ no consumer | Classification kept in `RECIPE_TIER_MATRIX.md` section of the category matrix |

## Merged item catalog (ingredient/output resolution authority)

`ItemCatalogLoader` merges, in order: `items.json` (primary), then
`holdfast_items.json`, `black_flotilla_items.json`, `verdict_items.json`,
`greenhouse_items.json`, `foundry_items.json`, `crossing_items.json`,
`dose_items.json`, `chemical_dependency_items.json`, `year_of_ash_items.json`.
Crop items (`crop_tuber`, `crop_ash_grain`, …) live in `greenhouse_items.json`.

## Station identity

| Station ID | Runtime registration path | Owning shelter infrastructure |
|---|---|---|
| `workbench` | `CraftingHostSession.SeedStation` + `Main.World.SyncCraftingStationsFromShelter` | `room_workshop` (condition ← machine ToolingHealth) |
| `stove` | `Main.World.SyncRoomStation` (Plan 55 bridge) | `room_kitchen` |
| `heater` | `Main.World.SyncRoomStation` (Plan 55 bridge) | `room_generator` |
| `water_purifier` | `Main.World.SyncRoomStation` (Plan 55 bridge) | `room_filtration` |
| `distiller` | **none** — flagged legacy gap (4 recipes unreachable) | no owning room exists |
| `""` | always available | hand craft |

## Validation checklist (enforced by `Plan55CraftingCatalogTests`)

- ≥ 80 recipes; all Plan-55 IDs present and unique
- every ingredient and result resolves in the merged catalog
- all amounts positive; all craft times positive
- Plan-55 recipes use only runtime-registered stations
- Plan-55 food outputs are `ItemType.Food` with `hungerRestore > 0`
- reload batches never output more rounds than brass consumed
- reloading components have loot-table provenance
- Plan-55 food output value ≤ ingredient value (no arbitrage)
