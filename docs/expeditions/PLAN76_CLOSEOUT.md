# Plan 76 — Regression Matrix & Closeout

## Summary

Plan 76 shipped as a **gap-closure expansion (55 → 63 destinations + 5 new scavenging tables)** after baseline reconnaissance proved the plan's "2 → 15" premise stale: 55 destinations already existed with Plan 46 table bindings live. Five of the plan's thirteen proposed concepts were already covered (hospital ×4, metro, shopping center, substations ×2, checkpoints ×2); the remaining eight genuine gaps were closed with distinct, grounded destinations. **Zero new Core code**; test-count pins updated (55 → 63; tier distribution 16/20/13/6 → 16/23/17/7) to reflect the expanded authored catalog.

## Destination roster (new)

| ID | Name | Family | D/Dgr/p/Stam | Table (reuse/new) |
|---|---|---|---|---|
| `loc_vulcan_works_chemical` | Vulcan Works Chemical Plant | Industrial | 7/7/0.20/3.0 | chemical_plant (reuse) |
| `loc_north_freight_yard` | North Freight Yard | Industrial | 9/5/0.20/2.8 | rail_yard (**was unused**) |
| `loc_blackridge_ammunition_depot` | Blackridge Ammunition Depot | Military | 11/8/0.24/3.2 | military_depot (**was unused**), discovery-gated |
| `loc_birchline_weather_station` | Birchline Weather Station | Scientific | 13/5/0.10/3.8 | weather_station (new) |
| `loc_west_ridge_survey` | West Ridge Survey Camp | Scientific | 12/6/0.18/3.4 | geological_survey (new) |
| `loc_mirrim_forest_edge` | Mirrim Forest Edge | Wilderness | 9/7/0.20/3.2 | forest_edge (new) |
| `loc_marsh_hollow` | Marsh Hollow | Wilderness | 11/6/0.16/3.6 | frozen_wetland (new) |
| `loc_charcoat_burns` | The Charcoat Burns | Wilderness | 6/5/0.14/2.6 | burned_woodland (new) |

## DoD highlights

- Existing destinations: all 55 preserved; the plan's "original two" (`loc_the_allotments`, `loc_denial_cut_substation`) pinned by parity test.
- 63 unique IDs; 0 collisions with `locations.json`/expansion catalogs; grounded names throughout (no `area_01` placeholders).
- All numeric ranges valid (Plan 32 bounds tests); distance spread 2–18; danger spread 1–10 with meaningful distribution (16/23/17/7 across tiers).
- Loot: 63/63 table bindings resolve; 0 unresolved loot tokens; 0 new items; distinct loot signature per destination.
- No destination strictly dominates; no dead destination (each new entry has a unique family/role/hazard identity).
- Determinism: `BalanceSweep_Deterministic_WithSanityBounds` byte-for-byte two-pass proof green.
- Old saves: additive expansion; discovery-gated entries follow the existing `requiresDiscovery` policy (3 total); no expedition-state migration.

## Verification

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 errors |
| `dotnet test Ashfall.Core.Tests` (expedition/scavenging filter) | PASS — **327/327** |
| `dotnet test Ashfall.Core.Tests` (full) | 9460/9461 — 1 failure in `PowerGridCatalogTests` from **pre-existing concurrent power-grid work** (uncommitted `power_grid.json` on branch), unrelated to expeditions |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `--data-integrity-selftest` | PASS — 0 errors, 0 warnings, 298 catalogs |
| `--expedition-selftest` | PASS |
| `--content-utilization-selftest` | PASS — CI gate PASS |
| Programmatic audits | 63/63 table refs resolve · 0 unresolved loot tokens · 0 duplicate IDs · cumulative-encounter review per new destination |

## Deferred (explicit)

- **Plan 49 micro-locations:** no destination-ID approach seam exists; stable IDs delivered (`abandoned_hospital`, `location_flooded_subway_depot`, `checkpoint_kilo_armory`, `loc_garrison_checkpoint_gamma`, `loc_north_freight_yard`).
- **Plan 48 weather gates:** no destination-eligibility field; stable IDs delivered (`loc_marsh_hollow`, `loc_mirrim_forest_edge`).
- **Plan 58/50/59:** narrative encounter pools, distress-signal sources, quest targets — stable IDs are the hook.
- **Plan 46 enrichment:** new tables authored to current schema; weighted tuning can deepen with playtest telemetry.
- Unused Plan 46 tables remaining: `clinic`, `fire_station`, `greenhouse`, `hunting_cabin`, `monastery`, `police_station` — future binding candidates, no dangling refs.
