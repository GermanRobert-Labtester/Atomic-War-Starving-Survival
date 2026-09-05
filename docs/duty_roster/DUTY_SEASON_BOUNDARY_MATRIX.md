# Duty Season Boundary Matrix

> **Boundary Truth Table:** Exact integer-day transition mapping proving continuous coverage across all 366 days (0–365) with zero gaps and zero overlaps.

---

## 1. Complete Boundary Truth Table

| Transition Pair | Prior Season | Prior Max Bound | Day Before Trans. | Trans. Day (Next Min) | Next Season | Next Max Bound | Gap/Overlap? |
|---|---|---|---|---|---|---|---|
| **Boot / Origin** | None | N/A | N/A | **Day 0** | `season_first_ashfall` | 7 | Gap-Free |
| **Trans. 1 → 2** | `season_first_ashfall` | 7 | Day 7 | **Day 8** | `season_second_winter` | 12 | Gap-Free (`8 == 7 + 1`) |
| **Trans. 2 → 3** | `season_second_winter` | 12 | Day 12 | **Day 13** | `season_settling` | 30 | Gap-Free (`13 == 12 + 1`) |
| **Trans. 3 → 4** | `season_settling` | 30 | Day 30 | **Day 31** | `season_spring_thaw` | 60 | Gap-Free (`31 == 30 + 1`) |
| **Trans. 4 → 5** | `season_spring_thaw` | 60 | Day 60 | **Day 61** | `season_faction_pressure` | 120 | Gap-Free (`61 == 60 + 1`) |
| **Trans. 5 → 6** | `season_faction_pressure` | 120 | Day 120 | **Day 121** | `season_first_siege` | 180 | Gap-Free (`121 == 120 + 1`) |
| **Trans. 6 → 7** | `season_first_siege` | 180 | Day 180 | **Day 181** | `season_consolidation` | 240 | Gap-Free (`181 == 180 + 1`) |
| **Trans. 7 → 8** | `season_consolidation` | 240 | Day 240 | **Day 241** | `season_long_winter` | 365 | Gap-Free (`241 == 240 + 1`) |
| **Overflow / 365+** | `season_long_winter` | 365 | Day 365 | **Day 366+** | `season_long_winter` (fallback) | Open-ended | Bounded Fallback |

---

## 2. Invariants Proven

1. **Inclusive Endpoints:** Both min and max day bounds are inclusive. Day 8 belongs to `season_second_winter`, day 12 belongs to `season_second_winter`, and day 13 belongs to `season_settling`.
2. **Exactly-One-Match Property:** Every integer `d` in `[0, 365]` matches exactly one season.
3. **Post-365 Overflow:** For campaigns continuing beyond day 365, `GetSeasonForDay(day)` returns the final season (`season_long_winter`), maintaining stable long-term survival conditions without crashing.
