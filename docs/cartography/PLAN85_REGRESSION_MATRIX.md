# Plan 85 — Regression Matrix

## Gates run (final verification, 2026-09-03)

| # | Gate | Result |
|---|---|---|
| 1 | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 errors |
| 2 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — **6866/6866** (0 failed, 0 skipped) |
| 3 | `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| 4 | `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings across 208 catalogs (10810 ids authored); `map_fragment_id` references enforced |
| 5 | `godot --headless --path . -- --bridge-selftest` | PASS (stable CI verb, shim removal notice) |
| 6 | `godot --headless --path . -- --content-utilization-selftest` | PASS — CI content-runtime gate green; orphaned 0 |
| 7 | `godot --headless --path . -- --cartography-selftest` | 2 pre-existing failures only (aspirational thresholds: ≥60 nodes / ≥200 routes; catalog now 20/44 vs 9/22 baseline). All damaged-map assertions pass (12 zones) |

## Test coverage added/updated

**New — `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs` (13 tests):** catalog structure (12 zones, unique ids, count consistency, 2–4 fragment band); reward resolution against `items.json`; fragment-producer coverage; installation node+route existence; lifecycle (0→1, N-1→N, duplicate no-op, edge-triggered once); reveal persistence through capture/restore; destination gating (locked until reveal, never gates other locations); old-save fixture under expanded catalog; catalog-order independence; expedition fragment-roll plumbing (deterministic single-entry table); live-catalog seeded soak (tokens surface and resolve); `Start` refusal while locked; negative fixtures (duplicate zone id, duplicate fragment across zones, count mismatch, empty/duplicate rewards, missing name).

**Updated pins (same strictness):** `Plan16CartographyTests` (6→12 zones); `Plan76DestinationLootReferenceTests` (53→65 destinations; table-binding pin 65); `Plan76BalanceSimulationTests` (53→65); `Plan32ExpeditionDestinationWiringTests` (65; tier distribution 21/23/15/6); `ScavengingTableCatalogTests` (integrity gate extended: empty `item_id` requires a resolvable `map_fragment_id` — net stricter).

**No gate weakened.** The only gate modifications extend coverage to the new entry shape or update exact-count pins to the new authored reality.

## Regression risks checked

- Original 3(+3) zones: byte-identical zone records except the two broken `revealed_items` ids (data bug fix; not save state).
- Expedition loot for all 65 destinations: full suite + Plan 76/32 pins green.
- Concurrent working-tree changes (GAP-48A/B weather gates by another session): preserved; `ExtraGateBlock` composition untouched; both gate layers compose in `GetBlockReason`.
- Pre-existing failures recorded at baseline (2 season-calendar tests, cartography node-count thresholds) — re-run after changes; season tests green in the final run; cartography thresholds remain the only red items and predate Plan 85.
