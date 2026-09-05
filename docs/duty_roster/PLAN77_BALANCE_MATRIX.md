# Duty Season Balance Matrix

> **Balance Audit:** Pacing curve, duration-weighted exposure, and transition delta audit across all 8 seasons.

---

## 1. Pacing Curve & Relative Ordering

- **Encounter Weight Progression:**
  `season_spring_thaw (0.85)` < `season_settling (1.00)` ≈ `season_consolidation (1.10)` < `season_faction_pressure (1.35)` < `season_first_ashfall (1.50)` < `season_second_winter (1.60)` ≈ `season_long_winter (1.65)` < `season_first_siege (1.75)`
- **Steam Trip Boost Progression:**
  `season_first_ashfall (0.02)` ≈ `season_first_siege (0.03)` < `season_settling (0.04)` < `season_faction_pressure (0.05)` ≈ `season_consolidation (0.06)` < `season_second_winter (0.08)` ≈ `season_long_winter (0.09)` < `season_spring_thaw (0.10)`

---

## 2. Duration-Weighted Exposure

| Season ID | Duration (Days) | Encounter Weight | Exposure Metric (`Duration × (Weight - 1.0)`) | Narrative Justification |
|---|---|---|---|---|
| `season_first_ashfall` | 8 | 1.50 | +4.0 | Brief, chaotic opening burst |
| `season_second_winter` | 5 | 1.60 | +3.0 | Acute, short cold shock (preserved baseline) |
| `season_settling` | 18 | 1.00 | 0.0 | Steady baseline operations |
| `season_spring_thaw` | 30 | 0.85 | -4.5 | Operational relief window |
| `season_faction_pressure`| 60 | 1.35 | +21.0 | Extended mid-game external tension |
| `season_first_siege` | 60 | 1.75 | +45.0 | Climax defensive trial |
| `season_consolidation` | 60 | 1.10 | +6.0 | Recovery and rebuilding window |
| `season_long_winter` | 125 | 1.65 | +81.25 | Grinding late-game endgame survival trial |

---

## 3. Transition Delta Shock Audit

Adjacent season encounter weight deltas:
- `first_ashfall` (1.50) → `second_winter` (1.60): `+0.10` (Smooth transition into early cold snap)
- `second_winter` (1.60) → `settling` (1.00): `-0.60` (Relief as initial winter breaks and order emerges)
- `settling` (1.00) → `spring_thaw` (0.85): `-0.15` (Gentle reduction into open season)
- `spring_thaw` (0.85) → `faction_pressure` (1.35): `+0.50` (Noticeable escalation as outsiders arrive)
- `faction_pressure` (1.35) → `first_siege` (1.75): `+0.40` (Tension boils over into lockdown)
- `first_siege` (1.75) → `consolidation` (1.10): `-0.65` (Post-siege relief and reconstruction)
- `consolidation` (1.10) → `long_winter` (1.65): `+0.55` (Gradual onset of brutal second-year winter)

No step change exceeds `0.65`, preventing artificial sawtooth oscillations while maintaining clear, player-perceptible phase shifts.
