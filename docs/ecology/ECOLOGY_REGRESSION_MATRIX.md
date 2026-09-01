# ECOLOGY_REGRESSION_MATRIX.md — Plan 28 (state as of this pass)

| # | Scenario (plan §Task 28BJ) | Status | Proof |
|---|---|---|---|
| 1 | season → migration paces | **PASS** | `BoundCalendar_ChangesTrajectory_UnderIdenticalRolls` |
| 2 | migration → corridor traversal | **PASS** (pre-existing + water filter) | `FishRun_NeverStandsOnDryGround`, selftest |
| 3 | migration peak → trapping abundance | **PASS** | seasonal factor composes into `densityMultiplier`; density gate selftest |
| 4 | contaminated corridor → tainted harvest | DEFERRED (RAD_TAINT matrix) | — |
| 5 | fish run → coastal harvest + market | **PARTIAL** — run + market easing live; dedicated coastal-harvest UI = Plan 23 content | selftest 13 |
| 6 | locust swarm → blight | DEFERRED (Plan 22 hook) | — |
| 7 | war closure → disruption | DEFERRED (28N design) | — |
| 8 | collapse → predator pressure | **PARTIAL** — starvation/rabies live; +modifier = 28AA design | — |
| 9–10 | infestation clear/leave | DEFERRED (contract documented) | — |
| 11 | excavation nest disturbance | DEFERRED | — |
| 12 | mold → Plan 09 disease | DEFERRED (no second infection path) | — |
| 13 | pantry pests | DEFERRED (contract in SHELTER_INFESTATION_CONTRACT) | — |
| 14–15 | waystation/caravan ecology | DEFERRED | — |
| 16–17 | field guide / radio projection | radio **PASS** (archetype-flavored, capped); field-guide entries handed to 20A | — |
| 18 | save/load active migration | **PASS** | `SaveRestore_WithBoundCalendar_RoundTripsExactly` |
| 19 | save/load infestation | n/a (deferred) | — |
| 20 | deterministic trace | **PASS** | `SameSeed_ProducesIdenticalSeasonalTrajectory`, `UnboundCalendar_IsExactlyLegacyNeutral` |

## Cross-system authority map (no duplicates)

| Concern | Single authority | Plan 28 role |
|---|---|---|
| seasons | `weather_seasons.json` / `SeasonProfileDef` | reads via pure calendar |
| geography | `world_evolution_seeds.json` sectors | reads; +optional `water` flag |
| population | `WildlifeMigrationSystem` pack ledger | sole owner |
| trapping | `WildlifeTrappingSystem` | consumes density only |
| greenhouse blight | `GreenhouseSystem` | untouched (hook deferred) |
| disease | Plan 09 systems | untouched |
| prices | `MarketSystem` | demand deltas only (existing path) |
| radio/map presentation | Plan 24/16 surfaces | projects, never owns |
