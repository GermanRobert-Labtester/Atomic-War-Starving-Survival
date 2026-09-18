# BALANCE SIM — Radiation Exposure (C2 / Plan 20A closure)

> Harness: `Ashfall.Core.Tests/Radiation/Plan20ARadiationBalanceSweepTests.cs`
> (9 cases, deterministic hourly ticks, zero RNG — same inputs ⇒ identical trajectory;
> paired-run fingerprint asserted in-test).
> Data authority used: `weather_effects.json` (FalloutStorm +150, Ashfall +45),
> crossing-locations band 18–24 rads/h; resolver defaults 2 / 20 / 40 mSv/h.
> No production data was modified by this report.

## Knobs (owner: data / Core defaults)

| Knob | Where | Value | Notes |
|---|---|---|---|
| Shelter interior base | `ExposureEnvironmentResolver.DefaultShelterInteriorRadRate` | 2.0 mSv/h | Core constant; hardcoded (20B moves to shelter_shielding.json) |
| Ceiling attenuation | `Shelter.GetWeakestCeilingAttenuation()` provider | scenario 1.0 / 0.4 | shelter state authority |
| Wasteland outdoor base | resolver default | 40.0 mSv/h | Core constant |
| Weather rad modifier | `weather_effects.json` | 0 / 45 / 150 / 250 | data authority (G1) |
| Expedition location rate | locations JSON `baseRadsPerHour` | scenario 6 / 24 | data authority |
| Gear protection | worn gear `EffectiveProtection()` | 0 in sweep | equipment authority |
| Acute threshold / scale | `RadiationSystem` | 80 / 100 cap | Core contract |

## 60-day dose/day curves (asserted in-test)

| Scenario | Effective rate | Dose/day | 60-day lifetime | Acute 80 at | Dose cap 100 at |
|---|---:|---:|---:|---:|---:|
| Intact shelter (atten 1.0) | 0 mSv/h | 0 | 0 | — | — |
| Degraded shelter (atten 0.4) | 1.2 mSv/h | 28.8 | 1,728 | ~67 h | ~83 h |
| Expedition low sector (6) | 6 mSv/h | 144 | 8,640 | ~13.3 h | ~16.7 h |
| Expedition authored band (24) | 24 mSv/h | 576 | 34,560 | ~3.3 h | ~4.2 h |
| Outside clear (40) | 40 mSv/h | 960 | 57,600 | 2 h | 2.5 h |
| Outside fallout storm (40+150) | 190 mSv/h | 4,560 | 273,600 | ~25 min | ~32 min |
| Multi-zone expedition (20d@6 + 20d@24 + 20d shelter) | phase | — | 14,400 | day 1 | day 1 |

Ordering principle holds: intact shelter < degraded shelter < expedition sector < clear surface < fallout storm.
Returning indoors stops accumulation but does not heal accumulated acute dose (no healing asserted).

## Findings

**F1 — Surface/expedition dose rates are instantly lethal at expedition timescales (pre-existing).**
Even the *low* 6 mSv/h sector saturates the acute scale within ~17 hours; authored
crossing-locations (18–24 rads/h) saturate in ~4 hours. Any expedition longer than a
few hours therefore kills without near-total protection (gear + shielding), which no
wearable currently supplies (gear protection is subtracted point-for-point).
This is upstream of 20A (resolver defaults + authored catalog), but 20A makes it
strictly more visible because position now genuinely drives dose.
*Proposals (foreman decision, not applied):* either (a) re-scale expedition/outdoor rates
and/or the acute scale, (b) make expedition travel consume a time-averaged outdoor rate
rather than the raw live rate, or (c) accept "no unshielded surface time" as the design
and guarantee gear/vehicle protection ranges to match. Flagged for the 20B balance sweep
which will add filter/vent/airlock modifiers.

**F2 — Ashfall alignment (changed in 20A).** Ashfall outdoor dose moved 0 → +45 mSv/h,
matching the value the forecast projection already displayed. Storm (150) and black rain
(250) are unchanged. Forecast and runtime now share one table (no §57.3 drift).

**F3 — Ceiling attenuation is the dominant shelter lever.** With the interim model,
interior dose is linear in `(1 − attenuation)`; 20B's composite model is where additional
levers (filtration, ventilation, airlock, decon, radon, flooding) must land. A filter or
ventilation effect smaller than a few percent of the 2.0 mSv/h interior base will be
invisible against this curve — size 20B contributions against these numbers.

**F4 — Intact shelter = zero dose exactly.** Ceiling-attributed shielding equals the
interior base at full attenuation. When 20B adds internal sources (radon), interior dose
should become non-zero indoors so shelter maintenance has a real floor — currently any
warning language about "interior radiation" has no numeric backing at full integrity.

## Seeds

No RNG is consumed by this sweep; reproducibility is structural (fixed hourly ticks).
The explicit paired-run fingerprint test fails if trajectory determinism regresses.

## Verification at recording

- `Plan20ARadiationBalanceSweepTests` 9/9; `ExposureBreakdownTests` 15/15;
  Radiation directory 72/72; host build 0 errors; data-integrity selftest PASS.
---

## Addendum — Plan 20B shielding sweep (2026-09-15)

Harness: `Ashfall.Core.Tests/Shelter/Plan20BShieldingBalanceSweepTests.cs`
(12 cases incl. breakdown semantics + paired-run fingerprint; deterministic hourly ticks, no RNG).
Interior baseline 2.0 mSv/h; shelter radon floor uses the shelter-live 12 Bq/m³ default.

| Scenario (60 days, 24 h/day) | Interior rate | Dose/day | 60-day lifetime |
|---|---:|---:|---:|
| Intact ceiling (att 1.0) + baseline radon | 0.06 mSv/h | 1.44 | 86.4 |
| Degraded ceiling (att 0.5) + radon | 1.06 mSv/h | 25.4 | 1,526 |
| Degraded ceiling + fully clogged filter (×1.6) | 1.66 mSv/h | 39.8 | 2,390 |
| Degraded ceiling + open airlock + fallout storm (×3.0) | 3.06 mSv/h | 73.4 | 4,406 |
| Degraded ceiling + flooding 0.25 (+1.0) | 2.06 mSv/h | 49.4 | 2,966 |
| Above with active decon (internal halved) | 1.53 mSv/h | 36.7 | 2,203 |

Findings:
- **F5 — weather cannot enter an intact structure** (no-defect + storm = same curve as clear).
  Weather matters exactly through named defects (open/breached airlock, duct breach,
  recirculation, clogged filter) — matching plan §25 and giving the player a controllable
  causal chain: seal the hatch → storm stops mattering.
- **F6 — maintenance ordering**: clogged filter (+57% over degraded baseline) < open airlock
  in a storm (+189%) < flooding with contamination. Filter maintenance is meaningful but not
  the only lever; doors and drainage dominate in storms.
- **F7 — decon is bounded**: halves internal sources only; a contaminated, flooded shelter
  still shows 1.53 mSv/h while decon runs (no magic reset; plan §24.2).
- **F8 — intact shelter floor**: 1.44 mSv/day from radon alone — a real but survivable
  maintenance pressure vs the 80 acute / 400 chronic thresholds.

No production data was modified.

---

## Foreman Decision & Closure (Wave 9 Part 2 — 2026-09-17)

All findings (F1–F8) are formally closed per signed foreman authorization (`docs/plans/wave9_part2/C3_DECISION.md`):

1. **F1 (Surface/expedition lethality): DECLINED RE-SCALE.** The proposal to scale down outdoor rates is rejected. Ashfall is designed as a hardcore survival simulator where the radioactive wasteland surface is inherently lethal without appropriate environmental protection. Long surface excursions require Plan 21 gear condition, Plan 50 vehicle shielding, Plan 20B shelter infrastructure, and Plan 22C anti-rad countermeasures.
2. **F2 (Ashfall alignment): RESOLVED.** Closed as verified in 20A (+45 mSv/h alignment in `weather_effects.json`).
3. **F3 (Ceiling attenuation): RESOLVED.** Composite multi-component model fully realized in Plan 20B.
4. **F4 (Intact shelter floor): RESOLVED.** 12 Bq/m³ radon baseline provides the intended 1.44 mSv/day indoor floor in Plan 20B.
5. **F5–F8 (Shielding invariants): CONFIRMED.** Intact structure weather immunity, maintenance ordering, bounded decontamination, and indoor radon floor are confirmed canonical rules.
6. **Data Modifications: 0.** Zero unauthorized production data changes; all test pins in `Plan20ARadiationBalanceSweepTests` (9/9) and `Plan20BShieldingBalanceSweepTests` (12/12) are preserved byte-for-byte.
