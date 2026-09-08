# Plan 126 — Baseline

Date: 2026-09-08

## Repository facts

- `crossing_items.json` was 11 entries before this change.
- `CrossingCatalogLoader` reads the local `items` array into `CrossingItemEntry`.
- `ItemCatalogLoader` also merges `crossing_items.json` into the global item registry after `items.json` and the other expansion item catalogs.
- Global duplicate IDs are retained on the earlier definition with a warning; Plan 126 IDs were absent from every item catalog before authoring.
- The local Crossing item DTO exposes `id`, `displayName`, `description`, `type`, `stackMax`, `weight`, `tradeValue`, `thirstRestore`, `hungerRestore`, and `moraleEffect`.
- The global item DTO additionally supports `healthEffect`; this is used only for `item_smuggled_medicine` and is ignored by the local Crossing projection.

## Supported types

`Food`, `Water`, `IrradiatedWater`, `Medical`, `AntiRad`, `Iodine`, `Protective`, `Tool`, `Fuel`, `Filter`, `Material`, `Trade`, `Comfort`, `Quest`, `Device`, `Weapon`, `Corpse`, `ContaminatedFood`, and `Relic` are valid case-insensitive `ItemType` values.

## Baseline verification

| Gate | Result |
| --- | --- |
| `godot --headless --path . -- --data-integrity-selftest` | PASS; 0 findings |
| `dotnet test ... --filter FullyQualifiedName~Crossing --no-restore` | PASS; 97 tests |
| `dotnet build Ashfall.csproj --no-restore` | PASS; 0 warnings/errors |
| `godot --headless --path . -- --content-utilization-selftest` | PASS; 0 orphaned catalogs |
| `python3 scripts/ci/run-gates.py --tier fast` | Baseline blocked by pre-existing whitespace in `artifacts/asset_registry.md` and `docs/narrative/PLAN_89_MUSTER_EPILOGUES_EXPANSION_CLOSEOUT.md` |

The working tree contained unrelated changes before Plan 126; they were preserved.
