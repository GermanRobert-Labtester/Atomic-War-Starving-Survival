# PLAN 123 — Sound-Ranging Characterization (Phase 11)

**Source:** `--plans-122-125-balance-soak` fixed-seed event matrix (8 cells × 6 days, plan §11.2)
**Status:** ACCEPTED — defensive-only intelligence, no precision-strike output anywhere.

## Observed matrix (array Mk I: base error 8°, region 3 cells, confidence cap 9000)

| Cell | Bearing error ° | Region cells | Confidence | Refused |
|---|---|---|---|---|
| clear / full array / stationary | 8.0 → 8.1 | 4 → 1 | 9000 | no |
| clear / full / repeated salvos | 8.0 → 8.1 | 4 → 1 | 9000 | no |
| wind / full / stationary | 11.2 → 11.3 | 5 → 1 | 7500 | no |
| storm / full | — | — | — | **yes (weather too noisy)** |
| clear / half array damaged | 12.0 → 12.1 | 5 → 1 | 7000 | no |
| wind / half array damaged | 16.8 → 17.0 | 5 → 1 | 5000 | no |
| clear / moving source | 8.0 → 8.1 | 4 → 2 | 6000 | no |
| storm / damaged array | — | — | — | **refused** |

## Reading

- **Weather matters exactly as designed:** wind widens error 8°→11.2° and cuts
  confidence 9000→7500; half-array damage compounds (17° under wind, 5000 conf).
  Storms are **refused outright** (`weather_too_noisy`) — the array refuses to
  present uncertainty as certainty (plan must-not §24).
- **Salvo correlation converges:** repeated confirming observations narrow the
  region from 4 to the 1-cell design floor within the matrix window. The floor
  is hard (`RegionRadiusFloorCells = 1`) — never weapon-grade.
- **Moving source resets the picture:** the moving-source cell retains a wider
  region (2 cells) and lower confidence (6000) — redeployment breaks the
  correlation loop as authored.
- **No precision strike path exists:** bearing error never below the authored
  base (8°) in any produced estimate; the estimate DTO carries no targeting
  fields (reflection-gated in unit tests); refusal cells produce nothing.

## Gates

`acoustic_matrix_bounds` (produced estimates only), `acoustic_matrix_no_weapon_precision` — ALL PASS.

## Follow-ups

- Ally-warning delivery and expedition route-avoidance consumption are typed
  consumers wired in Phase 7 (`OnThreatEstimate`); their balance impact is
  scenario-level (75-day harness) rather than matrix-level.
