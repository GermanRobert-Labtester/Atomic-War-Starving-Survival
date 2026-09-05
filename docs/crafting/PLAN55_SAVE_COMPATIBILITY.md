# Plan 55 Save Compatibility

## Catalog growth is additive

- All 73 baseline recipe IDs are unchanged; 8 new IDs appended. No renumbering.
- Recipe knowledge is **not** saved (static catalog membership) — old saves
  automatically see the 8 new recipes with no fabricated unlock state.
- In-progress craft jobs persist as `{RecipeId, HoursRemaining, CrafterId}` and
  restore via `CraftingSystem.SetRecipeLookup` → `FindRecipe`. Unknown IDs are
  silently dropped (pre-existing behavior, unchanged); all legacy IDs resolve.
- `CraftingSystemSave` also carries `WorkshopState` and `PharmaState` — both
  untouched by Plan 55.

## Atomicity guarantees (verified by construction and tests)

- `StartCraft` validates everything (station operational, result gate,
  ingredient bill, output capacity) **before** the single
  `TryExecuteTransaction` mutation — rejected crafts consume nothing.
- Completion produces output exactly once; overflow routes to `OverflowStash`,
  else full ingredient refund + station repair. No duplicate/refund path was
  modified.
- New recipes all have `resultAmount > 0` — none touch the zero-result
  allowlist or the sink-pattern rejection path.

## Item additions (5, all additive)

`item_flatbread`, `item_boiled_roots`, `item_vegetable_soup`, `item_pemmican`,
`item_travel_ration` — plain Food items with standard fields. Old saves load
them as unknown-but-harmless catalog entries; no save migration needed. No
stack sizes, IDs, or fields of existing items were modified.

## Scavenging table additions (additive entries only)

4 entries appended to 2 existing tables (`table_loot_military_depot`,
`table_loot_police_station`). No existing entry weights/quantities changed, so
pre-existing loot distributions are preserved bit-for-bit.

## Host bridge change (one behavior change)

`Main.World.SyncCraftingStationsFromShelter` now also syncs `stove`, `heater`,
`water_purifier` from shelter rooms. For old saves this can only *enable*
previously dead recipes — it removes no state, degrades nothing, and mirrors
the established workbench pattern.
