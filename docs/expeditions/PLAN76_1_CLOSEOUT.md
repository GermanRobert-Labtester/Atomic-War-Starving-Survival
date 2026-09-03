# Plan 76.1 — Closeout: Full Scavenging-Table Migration

Final task of the 42-destination `lootCategories`→Plan 46 migration.
**Every authored destination in `expeditions.json` now carries a
`scavenging_table_id` — 53 / 53 bound.**

## What this closeout task shipped (final 12)

| Destination | Binding | Note |
|---|---|---|
| `loc_apiary_rows` | **`table_loot_apiary_rows`** (new, renewable) | Forage/honey signature the farm table lacks. |
| `convoy_echo7_cache` | **`table_loot_convoy_cache`** (new, `one_time`) | A hidden cache is the schema's one-time find, not a ruin's stock. |
| `loc_seed_library_annex` | `table_loot_farm` (existing) | Seed/agronomy ecology — 3 of 4 authored categories live in the farm table. |
| `loc_cider_press` | `table_loot_farm` (existing) | Orchard-processing farm ecology. |
| `loc_municipal_archive` | **`table_loot_municipal_archive`** (new) | Records vault: books, blueprints, wills. |
| `loc_printworks` | **`table_loot_printworks`** (new) | Ink/solvent floor, chemical item hazards, civil-defense posters. |
| `location_ministry_of_truth_bunker` | **`table_loot_ministry_bunker`** (new) | Deep records bunker, radiation 0.10, vandalized propaganda. |
| `government_bunker` | **`table_loot_government_bunker`** (new) | Continuity stores: MREs, service radios, rad tablets, deployment map. |
| `location_the_dead_hand_core` | **`table_loot_dead_hand_core`** (new) | Hottest table in the catalog (radiation 0.20): rad tablets, detection gear, hazmat, lead cask. |
| `loc_the_shallows_market` | **`table_loot_shallows_market`** (new, renewable) | Living boat market — trade-stock precedent per §42. |
| `loc_settlement_pilgrim_hearth` | **`table_loot_pilgrim_hearth`** (new, renewable) | Living priory's care stock (medical/water) — `table_loot_monastery` considered but it is finite-flavoured ruin stock; the priory is alive. |
| `loc_settlement_brine_pans` | **`table_loot_brine_pans`** (new, renewable) | Working salt camp. |

No new item ids invented (Plan 76 §1.10). Codex refs reuse existing ids only.

## Migration totals

| Metric | Before Plan 76.1 | After |
|---|---:|---:|
| Bound destinations | 11 / 53 | **53 / 53** |
| Plan 46 tables | 20 | **49** |
| New tables authored | — | 29 across 7 family tasks |
| Reuse bindings (no new table) | — | 13 destinations across existing tables |
| Renewable tables (living sites) | 2 (farm, greenhouse) | **8** |
| One-time cache tables | 0 | 1 (convoy echo-7) |

Family task records: `PLAN76_1_MEDICAL_TABLE_BINDINGS.md`,
`PLAN76_1_MECHANICAL_FUEL_BINDINGS.md`,
`PLAN76_1_HOUSEHOLD_COMMERCIAL_BINDINGS.md`,
`PLAN76_1_MILITARY_BINDINGS.md`, `PLAN76_1_ELECTRICAL_BINDINGS.md`,
`PLAN76_1_WATER_CHEMICAL_BINDINGS.md`, and this closeout.

## Regression gate (final state)

`Ashfall.Core.Tests/Expeditions/Plan76DestinationLootReferenceTests.cs` (5 tests):
authored catalog 53 + original-two parity; all `lootCategories` resolve against
merged item catalogs; all `scavenging_table_id` resolve against Plan 46 tables
with full-coverage assertion (`Assert.Equal(53, bound)`); dedicated
no-unbound-destination test; repaired-ref anti-regression.

## Legacy `lootCategories` fields

All 53 destinations keep their `lootCategories` arrays. The runtime reads them
only when no table resolves; with every destination table-bound they are now
inert provenance records of the authored intent. Leave them in place — removal
would be a noisy diff with zero runtime effect.

## Verification (all PASS)

- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
- Scoped tests: 66 / 66 PASS
- `godot --headless --path . -- --data-integrity-selftest` — 0 findings
- `godot --headless --path . -- --expedition-selftest` — 19/19
- `godot --headless --path . -- --content-utilization-selftest` — CI gate PASS

## Known follow-ups (out of scope)

1. Full-suite green depends on the concurrent radio workstream's 2 unrelated
   test failures (see `PLAN76_CLOSEOUT.md` concurrency note).
2. `suburban_house`-era tables (`table_loot_apartment_block`) contain a
   duplicate `battery` entry (weights 20 + 15) — pre-existing Plan 46 data;
   harmless but worth a tidy-up pass.
3. Balance pass optional: per-table expected-yield vs danger-tier simulation
   (`ashfall-balance-sim`) over the 49-table catalog.
