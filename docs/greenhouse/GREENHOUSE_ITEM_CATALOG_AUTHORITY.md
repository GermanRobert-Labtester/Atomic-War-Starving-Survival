# GREENHOUSE ITEM CATALOG AUTHORITY (Plan 91)

## Decision: Model A — merged global registry

`ItemCatalogLoader.SecondaryItemFiles` (`Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs:67`)
loads these files into **one** global `ItemCatalog`, in order:

1. `items.json` (primary, required)
2. `holdfast_items.json`
3. `black_flotilla_items.json`
4. `verdict_items.json`
5. `greenhouse_items.json` ← 5th
6. `foundry_items.json`, `crossing_items.json`, `dose_items.json`,
   `chemical_dependency_items.json`, `year_of_ash_items.json`

Consequences:

- **All item IDs share one namespace.** A greenhouse ID may not collide with any
  other item file. Collisions are not errors — the later file's entry is
  silently skipped (`if (catalog.Contains(dto.id)) continue;`). First-loaded wins.
- Because `items.json` loads first, **any ID present in both `items.json` and
  `greenhouse_items.json` resolves to the `items.json` definition.**
- Cross-system references (crafting `resultItemId`/`ingredients[].itemId`,
  scavenging `item_id`, trade, quests) resolve greenhouse IDs globally with no
  cross-catalog lookup layer.

## Loader schema (verified end-to-end, Phase 0)

- Root: wrapped object `{ "schema_version": N, "items": [ ... ] }`
  (`CatalogLocator.LoadWrappedList` takes the first array-valued property;
  `schema_version` is skipped by name).
- Entry DTO (`ItemJsonDto`): `id`, `displayName`, `description`, `iconPath`,
  `type`, `stackMax`, `weight`, `radProtection`, `durability`, `isEquipable`,
  `equipSlot`, `contamination`, `hungerRestore`, `thirstRestore`,
  `healthEffect`, `radCleanse`, `moraleEffect`, `decorLocalizedMoraleDelta`,
  `empShielded`, `tradeValue`, `tradeTier`, `disassembleYieldFraction`,
  `scrapValue[]`, `repairRecipe`.
- Unknown `type` values fall back to `Material` (case-insensitive enum parse).
- `stackMax <= 0` coerces to 1; empty `displayName` falls back to the id.

## Accepted `type` values (exact, from `ItemType`, ItemTypes.cs)

`Food, Water, IrradiatedWater, Medical, AntiRad, Iodine, Protective, Tool,
Fuel, Filter, Material, Trade, Comfort, Quest, Device, Weapon, Corpse,
ContaminatedFood, Relic`

There is **no** `Consumable`, `Reagent`, `Fertilizer`, or `Structural` type.
The plan's draft type grammar was normalized to this enum (plan §1.6).

## Greenhouse file's role after Plan 91

`greenhouse_items.json` carries **greenhouse-specific content only**: crop
seeds/crops/production equipment unique to the greenhouse expansion, plus the
Plan 91 supply ecosystem (tools, amendments, pest control, water management,
structural repair). Generic items (filters, plastics, hoses, seed packets)
live in `items.json` and are referenced by ID, not duplicated (plan §1.4, §36).
