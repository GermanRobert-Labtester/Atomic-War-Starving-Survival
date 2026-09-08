# Duty Season Coverage Matrix

Final eight-season catalog with narrative role, authored windows, modifiers, and balance profile. Every window is reachable: campaigns have no day cap (`SimClock` unbounded; epilogue takes live `FinalDay`), and all seasons are contiguous from day 0. Unreachable-season count: **0**.

| Season | Days (incl.) | Duration | encounterWeight | steamTripBoost | Narrative role | Balance band |
|---|---|---:|---:|---:|---|---|
| `season_first_ashfall` | 0–7 | 8 | 1.45 | 0.01 | confusion, improvised labor, shortages | high encounter / low trip |
| `season_second_winter` | 8–12 | 5 | **1.6** | **0.08** | existing authored pressure pocket (preserved) | high / moderate-high |
| `season_settling` | 13–30 | 18 | 1.0 | 0.06 | routines emerge, deliberate assignments | moderate / rising |
| `season_spring_thaw` | 31–60 | 30 | 0.75 | 0.12 | mobility, external opportunity, lower friction | low / high |
| `season_faction_pressure` | 61–120 | 60 | 1.3 | 0.05 | political competition, security duties | medium-high / moderate |
| `season_first_siege` | 121–180 | 60 | 1.75 | 0.03 | defensive posture, sustained strain | high / low |
| `season_consolidation` | 181–240 | 60 | 1.0 | 0.09 | recovery, organized production, logistics | moderate / rising |
| `season_long_winter` | 241–365 | 125 | 1.5 | 0.02 | late scarcity, maintenance, survival focus | high / low |

## Balance ordering (authored)

- Encounter: `spring_thaw (0.75) < settling = consolidation (1.0) < faction_pressure (1.3) < first_ashfall (1.45) < long_winter (1.5) < second_winter (1.6) < first_siege (1.75)` — matches the plan's target ordering with the preserved season slotting into the high band by its own values.
- Steam-trip: `first_ashfall (0.01) < long_winter (0.02) < first_siege (0.03) < faction_pressure (0.05) < settling (0.06) < consolidation (0.09) < second_winter (0.08→ see note) < spring_thaw (0.12)` — second_winter retains its repository value (0.08), which sits slightly above consolidation; accepted as legacy-character rather than distorted.

## Duration-weighted pressure (encounter Δ × days, relative to neutral 1.0)

| Season | Exposure (|w−1|×days) | Finding |
|---|---:|---|
| first_ashfall | 3.6 | short, sharp opener — fine |
| second_winter | 3.0 | short intense pocket (legacy) — fine |
| settling / consolidation | 0.0 | neutral recovery plateaus |
| spring_thaw | 7.5 | sustained relief — the campaign's "breath" |
| faction_pressure | 18.0 | largest midgame contributor, intended |
| first_siege | 45.0 | largest single contributor — the campaign's crisis spine |
| long_winter | 62.5 | largest cumulative contributor — intentional late-game grind, flagged for telemetry follow-up |

`first_siege` + `long_winter` together contribute ~80% of positive pressure exposure. This is the intended endgame ramp, not an accident, but it is the first thing to revisit if playtests show late-campaign fatigue stacking with weather/incident difficulty (correlation-vs-causation follow-up).

## Adjacent transition deltas (shock audit)

| Transition | Δ encounter | Δ trip | Shock assessment |
|---|---:|---:|---|
| ashfall→second_winter | +0.15 | +0.07 | acceptable — authored early spike |
| second_winter→settling | −0.60 | −0.02 | intended release |
| settling→spring_thaw | −0.25 | +0.06 | smooth |
| thaw→faction_pressure | +0.55 | −0.07 | intended midgame turn |
| faction→first_siege | +0.45 | −0.02 | intended crisis beat |
| siege→consolidation | −0.75 | +0.06 | intended relief |
| consolidation→long_winter | +0.50 | −0.07 | intended late squeeze |

All large deltas coincide with genuine phase boundaries; no unjustified day-201-style cliffs.
