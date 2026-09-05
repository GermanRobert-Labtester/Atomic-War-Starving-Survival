# PLAN91 BASELINE — RECONNAISSANCE RECORD (Phase 0)

Date: recorded at Plan 91 execution start.

## The plan's stale audit, corrected

Plan 91 was authored against an audit stating "14 entries". Verified reality:

| Metric | Value |
|---|---|
| Entries on disk in `greenhouse_items.json` at start | 30 |
| Entries that actually register in the global catalog | **14** |
| Dead entries (skipped by loader) | 16 |

The 16 dead entries are the seed/crop pairs from plans 36/47/50
(`item_seed_hardy_tuber`, `crop_hardy_tuber`, `item_seed_ash_grain`,
`crop_ash_grain`, `item_seed_biolum_mushroom`, `crop_biolum_mushroom`,
`item_seed_nutrient_algae`, `crop_nutrient_algae`,
`item_seed_medicinal_herb`, `crop_medicinal_herb`, `item_seed_leafy_green`,
`crop_leafy_green`, `item_seed_oilseed`, `crop_oilseed`,
`item_seed_cold_legume`, `crop_cold_legume`). A later parity pass
(commit `8bb494b4`) added **improved versions of all 16 to `items.json`**
(richer descriptions, `empShielded: true`). Because `items.json` loads first
and the loader drops duplicates, the greenhouse copies have been inert since
that commit. The live authority for those 16 IDs is `items.json`.

This is why the plan's "verified baseline of 14" was correct: exactly 14
entries from this file are live.

## Resolution (plan §1.1, §32, §36)

- The 14 live entries are preserved **byte-identical** (parity matrix below).
- The 16 dead stale copies are removed from `greenhouse_items.json`. This
  changes **zero** registered values — their live definitions remain in
  `items.json` (they already won every lookup before this change).
- 16 new supply items are added → **exactly 30 live entries**, matching the
  plan's target with no content loss anywhere in the global registry.

## ItemCatalogLoader / AssetRegistry audit

- See `GREENHOUSE_ITEM_CATALOG_AUTHORITY.md` (Model A merged registry,
  full DTO schema, exact type enum).
- `AssetRegistry.cs:1056` and `AssetCoverageScanner.cs:25` walk
  `greenhouse_items.json` ids for asset coverage (report-only, non-gating
  full sweep; gating selftest samples referenced ids). No registration
  capacity or list edit is required to add items — the loader is data-driven.

## Live consumer audit (plan §3.6, §42)

| Consumer | Evidence | Items consumed |
|---|---|---|
| `GreenhouseSystem.Plant` | `GreenhouseSystem.cs:154-170` via `GreenhouseExpansionCatalog.CropCatalog` | 12 seed IDs |
| `GreenhouseSystem.TreatBlight` | `GreenhouseSystem.cs:270-272` | `item_blight_treatment` (LIVE) |
| `GreenhouseExpansionCatalog.Items` constants | registry only | planter box, grow lamp, lead-glass pane, grow medium — constants exist, no runtime consumption found (CRAFTING/TRADE content) |
| `ContentUtilizationScanner` | maps file → `GreenhouseSystem`/`ApicultureSystem`/`GreenhousePanel` | coverage reporting |

No greenhouse runtime consumes fertilizer, pest supplies, irrigation, or
structural repair items today. All Plan 91 supply additions are therefore
inventory/crafting/scavenging/trade content with documented future hooks —
no description claims a live greenhouse effect (plan §1.11).

## Crafting authority audit (plan §3.7)

`recipes.json`: `{schema_version, recipes:[{id, recipeName,
ingredients:[{itemId,amount}], resultItemId, resultAmount,
craftingTimeHours, requiredStationId}]}` — 84 recipes, none greenhouse-bound.
Stations in use: `workbench` (55), `stove`, `water_purifier`, `distiller`,
`heater`. Ingredient IDs verified present in `items.json`:
`scrap_metal`, `wood_block`, `rubber_hose`, `plastic_material`,
`mechanical_parts`, `cloth`, `chemicals`, `battery`.

## Scavenging authority audit (plan §3.8)

`scavenging_tables.json` (Plan 46 authority): 49 weighted tables.
Agricultural tables present: `table_loot_greenhouse` (10 entries),
`table_loot_farm` (11), `table_loot_warehouse` (10).
Entry schema: `{item_id, weight, min_quantity, max_quantity, rarity_tier}`
plus optional `codex_unlock_id` / `map_fragment_id`.
Note: entries with empty `item_id` + `map_fragment_id` are the deliberate
map-fragment reward pattern (28 occurrences across tables) — not defects.

## Baseline gate results (exact)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS, 0 warnings/errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 6892/6895 pass; 3 FAIL — `RebelBranchCatalogTests`, `IndependentBranchCatalogTests`, `CatalogIntegrityValidatorTests.AllCatalogIdsCrossReferenceCleanly` — all one pre-existing root cause: 13 unresolved `ending_*` ids in `muster_epilogues.json` (Muster expansion debt, unrelated to items) |
| `dotnet build Ashfall.csproj` | PASS, 0 warnings/errors |
| `godot --headless -- --data-integrity-selftest` | FAIL(13) — same 13 muster `ending_*` findings; 208 other catalogs clean, **zero item/greenhouse findings** |
| `godot --headless -- --greenhouse-selftest` | PASS 24/24 |
| `godot --headless -- --content-utilization-selftest` | CI gate PASS |

Plan 91's regression gate: **no new findings beyond the 13 pre-existing
muster-ending findings**, and all item/greenhouse checks stay clean.
