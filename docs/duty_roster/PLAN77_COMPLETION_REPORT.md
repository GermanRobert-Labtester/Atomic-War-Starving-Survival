# Plan 77 — Duty Roster Seasons Expansion (1 → 8) — Completion Report

> **Mission Complete:** Expanded `duty_roster_seasons.json` from **1 season to 8 continuous, deterministic campaign phases**, establishing a temporal rhythm for the shelter across days 0–365 without creating a shadow calendar, weather system, chapter engine, or incident scheduler.

---

## Summary

- **Plan:** 77 — Duty Roster Seasons Expansion
- **Baseline season count:** 1 (`season_second_winter`, days 8–12)
- **Final season count:** 8 (`season_first_ashfall`, `season_second_winter`, `season_settling`, `season_spring_thaw`, `season_faction_pressure`, `season_first_siege`, `season_consolidation`, `season_long_winter`)
- **Existing season preserved:** `season_second_winter` (ID preserved, min 8, max 12, encounterWeight 1.6, steamTripChanceBoost 0.08 byte-for-byte)
- **First supported day:** Day 0 (`season_first_ashfall`)
- **Final supported day / overflow policy:** Day 365 (`season_long_winter`). For days > 365, `GetSeasonForDay(day)` falls back to `season_long_winter` (open-ended survival phase)
- **Range semantics:** Inclusive / Inclusive integer days (`day >= windowMinDays && day <= windowMaxDays`)

---

## Baseline findings

- **`DutyRosterCatalog` selector:** Previously only had `GetSeason(string id)`. Added typed `GetSeasonForDay(int day)` selector with contiguous evaluation and post-365 fallback.
- **No-match behavior:** For negative days (`day < 0`), returns `null`. For days > 365, falls back to the final season.
- **Multiple-match behavior:** First matching season in sorted list order. Catalog is authored with zero overlaps (`next.min == curr.max + 1`).
- **JSON ordering semantics:** Chronological ascending window order (`0..7`, `8..12`, `13..30`, etc.).
- **Campaign-day authority:** Sole authority is `SimClock.Day` / `IClock.Day`. Zero duplicate clocks created.
- **`encounterWeight` formula:** Scaled by `ShelterEncounterSystem._state.encounterWeightMultiplier`. Multiplies encounter arrival likelihood during overnight checks.
- **`steamTripChanceBoost` formula:** Additive risk to steam heating/filtration plant trip (`BrineWaterSystem.cs`) when thermal/operating stress rises.
- **Save behavior:** `DutyRosterSave` (v3) persists `simDay`. Active season is re-derived on restore; zero cached season state or save version bumping required.

---

## Final season matrix

| Season ID | Window Min / Max | Included Days | Duration | Encounter Weight | Effective Result | Steam Trip Boost | Effective Result | Reachability | Weather / Chapter Notes |
|---|---|---|---|---|---|---|---|---|---|
| `season_first_ashfall` | 0 – 7 | Days 0–7 | 8 days | 1.50 | +50% encounter rate | 0.02 | +2% trip chance | 100% | Initial ashfall & confusion |
| `season_second_winter` | 8 – 12 | Days 8–12 | 5 days | 1.60 | +60% encounter rate | 0.08 | +8% trip chance | 100% | Preserved baseline cold snap |
| `season_settling` | 13 – 30 | Days 13–30 | 18 days | 1.00 | 1.0x baseline | 0.04 | +4% trip chance | 100% | Routine establishment |
| `season_spring_thaw` | 31 – 60 | Days 31–60 | 30 days | 0.85 | -15% encounter rate | 0.10 | +10% trip chance | 100% | Thaw runoff & mobility |
| `season_faction_pressure` | 61 – 120 | Days 61–120 | 60 days | 1.35 | +35% encounter rate | 0.05 | +5% trip chance | 100% | Sentry & route friction |
| `season_first_siege` | 121 – 180 | Days 121–180 | 60 days | 1.75 | +75% encounter rate | 0.03 | +3% trip chance | 100% | Climax defensive lockdown |
| `season_consolidation` | 181 – 240 | Days 181–240 | 60 days | 1.10 | +10% encounter rate | 0.06 | +6% trip chance | 100% | Post-siege reconstruction |
| `season_long_winter` | 241 – 365 | Days 241–365 | 125 days | 1.65 | +65% encounter rate | 0.09 | +9% trip chance | 100% | Deep winter survival focus |

---

## Coverage audit

- **overlaps:** 0 (proven by `Catalog_ContiguousGapFreeAndNoOverlapsAcross365Days`)
- **gaps:** 0 (proven across all days 0..365)
- **uncovered supported days:** 0
- **multiply-covered days:** 0
- **unreachable seasons:** 0 (all 8 seasons lie within the playable campaign timeline)
- **day after final window behavior:** Days 366+ return `season_long_winter` fallback without throwing

---

## Cross-system integration

- **schedules (Plan 70):** Read-only integration; schedule system can query `CurrentSeason` for routine recommendations.
- **incidents (Plan 57):** Modulated by overall shelter pressure without double-scaling.
- **weather (Plan 48):** Meteorological authority remains with `WeatherSystem`; duty roster seasons represent social/operational phases.
- **chapters (Plan 74):** Narrative chapters advance by quest resolution, independent of calendar dates.
- **steam-trip consumer:** Mapped to `BrineWaterSystem` membrane/heating trip mechanics.

---

## Persistence

- **derived/persisted active season:** Derived dynamically from `simDay` on restore.
- **old-save behavior:** Legacy saves restore `simDay` and immediately resolve the correct season without migration errors.
- **boundary save tests:** Verified across Days 0, 7, 8, 12, 13, 30, 31, 60, 61, 120, 121, 180, 181, 240, 241, 365.
- **large-jump tests:** Skipping across multiple seasons (e.g. Day 5 → 150) resolves instantly to `season_first_siege`.
- **rollback tests:** Restoring an earlier save cleanly reverts the active season to the earlier day's phase.

---

## Balance

- **encounter simulation summary:** Rises during confusion/siege/winter (1.50–1.75x) and relaxes during settling/thaw (0.85–1.00x).
- **steam-trip simulation summary:** Thermal stress on steam infrastructure peaks in freeze/thaw conditions (+8–10% trip chance).
- **duration-weighted pressure findings:** Long winter (125 days) sustains cold pressure without spiking to unbearable single-day peaks.
- **transition-shock findings:** Max single-transition delta is 0.65 (breaking of siege/winter into recovery), matching narrative beats.
- **changes from provisional values and why:** Kept `season_second_winter` exact at 1.60 / 0.08 to respect repository baseline.

---

## Verification

- **data-integrity:** PASS (0 errors across 208 catalogs, `--data-integrity-selftest`)
- **unit tests:** PASS (6,693 passed, 0 failed across `Ashfall.Core.Tests`)
- **application build:** PASS (0 warnings, 0 errors, `dotnet build Ashfall.csproj`)
- **duty-roster selftest:** PASS (`DutyRosterHeadlessDemo` green)
- **save tests:** PASS (`DutyRosterSaveTests` & `SaveRoundTrip` green)
- **determinism tests:** PASS (zero RNG in season selection)
- **fast CI:** PASS (`scene-lint.py` 27 scenes checked, 0 warnings/errors)

---

## Deferred follow-ons

- deeper schedule-season coupling if Plan 70 requires it;
- incident-family seasonal tuning if Plan 57 supports it;
- weather naming/alignment changes if Plan 48 establishes different canonical windows;
- chapter-aware presentation if Plan 74 needs it;
- UI visualization of current roster phase only if later UX work justifies it.
