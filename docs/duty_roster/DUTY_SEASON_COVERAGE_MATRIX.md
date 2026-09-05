# Duty Season Coverage Matrix

> **Coverage Specification:** Complete narrative and numerical matrix for all 8 duty seasons across the full 365-day arc.

---

| Season ID | Narrative Phase | Min Day | Max Day | Duration (Days) | Encounter Weight | Steam Trip Boost | First Day | Last Day | Prior Season | Next Season |
|---|---|---|---|---|---|---|---|---|---|---|
| `season_first_ashfall` | Immediate Confusion & Emergency Labor | 0 | 7 | 8 | 1.50 | 0.02 | Day 0 | Day 7 | None | `season_second_winter` |
| `season_second_winter` | Early Cold Snap & Acute Friction (Preserved) | 8 | 12 | 5 | 1.60 | 0.08 | Day 8 | Day 12 | `season_first_ashfall` | `season_settling` |
| `season_settling` | Routine Duty & Early Stabilization | 13 | 30 | 18 | 1.00 | 0.04 | Day 13 | Day 30 | `season_second_winter` | `season_spring_thaw` |
| `season_spring_thaw` | Operational Mobility & Lower Friction | 31 | 60 | 30 | 0.85 | 0.10 | Day 31 | Day 60 | `season_settling` | `season_faction_pressure` |
| `season_faction_pressure` | Territory Disputes & Sentry Strain | 61 | 120 | 60 | 1.35 | 0.05 | Day 61 | Day 120 | `season_spring_thaw` | `season_first_siege` |
| `season_first_siege` | High Defensive Strain & Lockdown | 121 | 180 | 60 | 1.75 | 0.03 | Day 121 | Day 180 | `season_faction_pressure` | `season_consolidation` |
| `season_consolidation` | Post-Siege Reconstruction & Industry | 181 | 240 | 60 | 1.10 | 0.06 | Day 181 | Day 240 | `season_first_siege` | `season_long_winter` |
| `season_long_winter` | Deep Winter Survival & Resource Scarcity | 241 | 365 | 125 | 1.65 | 0.09 | Day 241 | Day 365 | `season_consolidation` | Open Fallback |

---

## 2. Total Campaign Duration Accounting

- `8 + 5 + 18 + 30 + 60 + 60 + 60 + 125 = 366 days` (representing day 0 through day 365 inclusive).
- Zero gaps: every campaign day has an unambiguous duty roster phase.
- 100% reachability: every season corresponds to a valid, playable campaign interval.
