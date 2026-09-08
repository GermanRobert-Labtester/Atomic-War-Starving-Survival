# Plan 61 Regression Matrix

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

Commands run against the final Plan 61 state (2026-02). Working tree also carries
**uncommitted concurrent work from another stream** (items.json, WildlifeTrapping*,
CatalogIntegrityValidator, shelter/medical files); none of it was touched, stashed
permanently, or reverted by Plan 61.

| # | Gate | Command | Result |
|---|---|---|---|
| 1 | Core tests build | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** — 0 errors |
| 2 | Full test suite | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS — 9426/9426** (3 consecutive runs; see note below) |
| 3 | Trade-scenario subset | `dotnet test ... --filter FullyQualifiedName~TradeScreen` | **PASS — 49/49** (30 pre-existing + 19 new Plan 61 gates) |
| 4 | Godot host build | `dotnet build Ashfall.csproj` | **PASS** — 0 errors, 0 warnings |
| 5 | Catalog integrity | `godot --headless --path . -- --data-integrity-selftest` | **PASS** — 298 catalogs, 0 errors, 0 warnings |
| 6 | Bridge verb | `godot --headless --path . -- --bridge-selftest` | **PASS** (exit 0, shim-removal verb) |
| 7 | Economy | `godot --headless --path . -- --economy-selftest` | **PASS** |
| 8 | Content utilization | `godot --headless --path . -- --content-utilization-selftest` | **PASS** (exit 0; no trade_screen_scenarios entries unresolved) |
| 9 | Save checksum sweep | `godot --headless --path . -- --save-store-checksum-selftest` | **PASS — 21/21 gates** |
| 10 | 7-day smoke (incl. trade + save/reload round-trip) | `godot --headless --path . -- --7-day-smoke-selftest` | **PASS** |
| 11 | Real campaign journey | `godot --headless --path . -- --real-campaign-journey-selftest` | **PASS** |

## Suite-stability note (honest record)

During verification, two intermediate full-suite runs showed 6 then 37 failures in
catalog-sweep classes (`CatalogIntegrityValidatorTests`, `RuntimeJsonBootstrapParityTests`,
`WildlifeTrapping*`, branch-catalog cross-reference tests) **with unchanged binaries**,
followed by three consecutive fully-green 9426/9426 runs on the identical final state.
An A/B run with the entire Plan 61 change stashed also passed (9407/9407). The failing
classes exclusively sweep catalogs modified by the **concurrent uncommitted stream**
and emit provenance-collision warnings (e.g. duplicate ids in `black_flotilla_items.json`,
a committed file). Diagnosis: pre-existing parallel-ordering flakiness in those sweeps
under a heavily modified working tree; not attributable to Plan 61 (which touches one
JSON catalog + two test files). Flagged for the owning stream.

## Files changed by Plan 61

- `Assets/StreamingAssets/Data/trade_screen_scenarios.json` — 3 → 15 scenarios
- `Ashfall.Core.Tests/TradeScreenSeamTests.cs` — one characterization count assertion updated (3 → ≥15; original IDs still pinned)
- `Ashfall.Core.Tests/Economy/TradeScreenScenarioCatalogTests.cs` — new (19 gates)
- `docs/economy/PLAN61_*.md`, `docs/economy/TRADE_*.md` — new (11 documents)

No Core/runtime source files were modified. No scenes, no themes, no other catalogs.
