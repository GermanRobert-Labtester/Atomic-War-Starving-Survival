# Duty Season Boundary Matrix

Bounds are **inclusive/inclusive**. Contiguous rule: `next.min == current.max + 1`. Selector: last season with `min <= day` (deterministic, order-robust). All rows verified by the independent Python oracle and pinned by `DutyRosterSeasonCatalogTests.Selection_ExactTransitionBoundaries` / `Selection_Post365OverflowFallback` / `Selection_NegativeDayReturnsNull`.

| Transition | Day before | First day of next | Final day of prior | Exact max bound | Exact min bound | Day after |
|---|---|---|---|---|---|---|
| first_ashfall → second_winter | 6 → first_ashfall | 8 → second_winter | 7 → first_ashfall | 7 → first_ashfall | 8 → second_winter | 9 → second_winter |
| second_winter → settling | 12 → second_winter | 13 → settling | 12 → second_winter | 12 → second_winter | 13 → settling | 14 → settling |
| settling → spring_thaw | 30 → settling | 31 → spring_thaw | 30 → settling | 30 → settling | 31 → spring_thaw | 32 → spring_thaw |
| spring_thaw → faction_pressure | 60 → spring_thaw | 61 → faction_pressure | 60 → spring_thaw | 60 → spring_thaw | 61 → faction_pressure | 62 → faction_pressure |
| faction_pressure → first_siege | 120 → faction_pressure | 121 → first_siege | 120 → faction_pressure | 120 → faction_pressure | 121 → first_siege | 122 → first_siege |
| first_siege → consolidation | 180 → first_siege | 181 → consolidation | 180 → first_siege | 180 → first_siege | 181 → consolidation | 182 → consolidation |
| consolidation → long_winter | 240 → consolidation | 241 → long_winter | 240 → consolidation | 240 → consolidation | 241 → long_winter | 242 → long_winter |

## Catalog edges

| Case | Day | Result |
|---|---|---|
| First playable boundary | 0 | `season_first_ashfall` |
| Catalog start | −1, −50 | `null` |
| Final inclusive bound | 365 | `season_long_winter` |
| Overflow (carry-forward policy) | 366, 400, 500 | `season_long_winter` |

## Short-window exhaustive check

`season_second_winter` (5 days) — every individual day verified: 8, 9, 10, 11, 12 all → `season_second_winter`; 7 and 13 resolve to neighbors.

## Exactly-one-match property

For every day 0–365 (366 days), the match count across all eight windows is exactly 1 — verified programmatically, pinned by `Selection_EveryDayFrom0To365ResolvesExactlyOneSeason`.

## Time-jump behavior

Selection derives from the current day only (no transition events exist): day 5 → 25 → 75 → 150 → 210 → 300 each resolve directly to the containing season; no intermediate season must "fire" (pinned by `Selection_LargeDayJumpsResolveCorrectly`). Rollback follows restored day by construction (derived state, nothing cached).
