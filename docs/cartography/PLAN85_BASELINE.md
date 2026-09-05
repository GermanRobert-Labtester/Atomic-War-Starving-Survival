# Plan 85 — Baseline Record (2026-09-03)

Repository truth at implementation time, established before any edit.

## Baseline commands

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 0 errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 6798 pass / 2 fail (pre-existing: `WildlifeSeasonalCalendarTests.SeasonWindowForDay_MatchesTheWeatherAuthority`, `Plan19DynamicWorldTests.SeasonModel_DefinesSixPhasesAcrossYear` — unrelated to cartography; green again by final verification) |
| `dotnet build Ashfall.csproj` | transiently broken mid-session by an unrelated in-flight working-tree change (GAP-48A/B weather-gate seams, `forceWeatherGate` / `ExtraBlockReason`); repaired by the concurrent editor during reconnaissance; final state 0 errors / 0 warnings |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 208 catalogs (10754 ids authored) |
| `godot --headless --path . -- --cartography-selftest` | 2 pre-existing failures: expects >= 60 map nodes / >= 200 routes; catalog shipped with 9 nodes / 22 routes since creation. Not caused by Plan 85; not normalized. After Plan 85: 20 nodes / 44 routes (still below the aspirational thresholds — deferred). |

## Baseline damaged-map layer (§3.3)

- `damaged_map_zones.json` contained **6 zones** (not 3 as planned): `industrial_district` (3), `suburban_heights` (2), `military_corridor` (3), `crater_ground_zero` (3), `deep_coast_shelf` (2), `high_scarp_ridgeline` (2). Per execution contract §0.1/§1.12 the delta to 12 is **+6 new zones**; all six original zones preserved verbatim (except two verified broken item refs — see below).
- **No runtime consumer existed.** The catalog was referenced only by `ContentUtilizationScanner` (static attribution strings), `HostCli.Cartography` (file-exists + count check), and `Plan16CartographyTests` (raw JSON structure). No loader, no system, no fragment acquisition, no completion, no reveal.
- Of the 6 hidden installations, only `loc_hidden_relay_bunker` existed as a wasteland-map node. None existed as expedition destinations.
- `revealed_items` was consumed by nothing at runtime (only `CatalogIntegrityValidator` reference checks).
- Pre-existing data bugs found and fixed under Plan 85 §0.13: `generator_parts` and `heirloom_seeds` did not resolve in `items.json` (→ `mechanical_parts`, `family_heirloom_seeds`).
- Pre-existing gap found: `ExpeditionSystem.ScavengingCatalog` was never wired by the host, so Plan 46 tables were not consumed by the live expedition roll path.

## Exit-gate answers (§3.10)

1. **Fragment lifecycle:** fragments are discovery tokens (Model B), not items. Registered once into `WastelandMapState.RegisteredMapFragments`; permanent campaign knowledge.
2. **Completion rule:** all of a zone's catalog fragments registered (distinct ids, set semantics).
3. **`revealed_items` meaning:** now the installation expedition destination's `lootCategories` — guaranteed-eligible signature salvage via the existing expedition loot-roll loop. Never a direct grant.
4. **Reveal path:** `DamagedMapSystem` → `WastelandMapSystem.Discover` + `Unlock` (authoritative fog-of-war/lock authority) + core dispatch gate unlock on the expedition destination.
5. **Save representation:** `WastelandMapState.RegisteredMapFragments` persisted by the existing `wasteland_map` save section (in-place restore; no new section).
6. **Fragments need no item definitions** (Model B ratified; zero fake items added).
7. **Plan 46/76/47:** 49 scavenging tables and 53 expedition destinations existed; both reconciled and extended. No map collectibles exist in `collectibles.json` (Plan 47 doc not required).
8. **One-time loot protection:** v1 authors no unique one-time rewards; all reward items are pre-existing multi-producer items resolved through the seeded expedition loot loop, so duplication by revisit/reload is impossible by construction.
