# Plan 77 — Completion Report

## Summary

- **Plan:** 77 — Duty Roster Seasons Expansion
- **Baseline season count:** 1 (`season_second_winter`, days 8–12, 1.6, 0.08)
- **Final season count:** 8 — contiguous, gap-free, overlap-free, days 0–365
- **Existing season preserved:** yes — ID and all values byte-stable
- **First supported day:** 0 (earlier days, incl. negatives → null)
- **Final supported day / overflow policy:** 365; days ≥ 366 carry `season_long_winter` forward (repository open-ended-window convention, shared with `WeatherSystem.GetSeasonForDay`)
- **Range semantics:** inclusive/inclusive both bounds; contiguous rule `next.min == prev.max + 1`

## Baseline findings

- **Selector:** none existed. Added `DutyRosterCatalog.GetSeasonForDay(int day)` — the minimal, justified Core addition (pure read-only derivation demanded by the pre-existing `DutyRosterSeasonCatalogTests` contract). Matching rule: last season with `windowMinDays <= day`, ties first-listed; order-robust; deterministic; no state.
- **No-match behavior:** `null` before first window; carry-forward after final window. Preserved as the repository-native policy.
- **Multiple-match behavior:** impossible in the authored catalog (exactly-one-match proven for all 366 days); selector resolves deterministically by latest `windowMinDays` if malformed data ever allowed overlap.
- **JSON ordering:** nonsemantic for correctness (selector order-robust); catalog authored chronologically to match test assumptions.
- **Campaign-day authority:** `SimClock` (`DutyRosterHostSession.Clock.Day`). No second clock introduced.
- **`encounterWeight` formula:** relative weight multiplier applied through `ShelterEncounterSystem.SetSecondWinter(multiplier, day)` — **replacement** semantics, non-positive → 1, persisted in encounter state, cleared back to 1. Not a probability.
- **`steamTripChanceBoost` formula:** **no live consumer** (verified). Range-validated authored data [0.0, 0.15]; composition/clamps deferred to the future consumer rather than invented.
- **Save behavior:** active season is derived from the restored day; only encounter multiplier state is persisted. No migration needed.

## Final season matrix

| Season | Window | Days | encounterWeight | steamTripBoost | Reachability | Notes |
|---|---|---:|---:|---:|---|---|
| first_ashfall | 0–7 | 8 | 1.45 | 0.01 | reachable | opener: chaos, low external opportunity |
| second_winter | 8–12 | 5 | 1.60 | 0.08 | reachable | **preserved legacy values**; early pressure pocket |
| settling | 13–30 | 18 | 1.00 | 0.06 | reachable | routines emerge |
| spring_thaw | 31–60 | 30 | 0.75 | 0.12 | reachable | relief peak + external opportunity peak |
| faction_pressure | 61–120 | 60 | 1.30 | 0.05 | reachable | midgame political ramp |
| first_siege | 121–180 | 60 | 1.75 | 0.03 | reachable | values pinned by save-round-trip test |
| consolidation | 181–240 | 60 | 1.00 | 0.09 | reachable | recovery plateau |
| long_winter | 241–365 | 125 | 1.50 | 0.02 | reachable | late-game grind; carry-forward beyond 365 |

## Coverage audit

- **Overlaps:** 0 — exactly-one-match verified for every day 0–365
- **Gaps:** 0 — contiguous 0→365 (`next.min == prev.max + 1` proven)
- **Uncovered supported days:** 0
- **Multiply-covered days:** 0
- **Unreachable seasons:** 0 (campaign has no day cap)
- **Day after final window:** `season_long_winter` (carry-forward, pinned by tests at 366/400/500)

## Cross-system integration

- **Schedules:** not present in this area — no wiring, documented follow-on.
- **Incidents:** no season wiring — encounter pressure consumed only via `ShelterEncounterSystem`; no double scaling (single multiplication site).
- **Weather:** `WeatherSystem` untouched and canonical; alignment table documented, including two deliberate label divergences (`second_winter` roster-phase placement, `spring_thaw` as social phase) per the ID-stability and no-meteorology rules.
- **Chapters:** day-agnostic display rows; thematic correlation documented; seasons never advance chapters.
- **Steam-trip consumer:** verified unrelated (`BrineWaterSystem` membrane threshold); field documented as deferred data.

## Persistence

- **Derived/persisted active season:** derived from campaign day at selection time; encounter multiplier state persisted by the existing codec.
- **Old-save behavior:** loads cleanly; restored day resolves against the new catalog; no retroactive transitions.
- **Boundary save tests:** day-150 round-trip pinned (`season_first_siege`, 1.75/0.03).
- **Large-jump tests:** `Selection_LargeDayJumpsResolveCorrectly` — direct resolution, no transition events needed.
- **Rollback tests:** derived state ⇒ restore-to-day restores era season by construction.

## Balance

- **Encounter simulation:** bands realized — thaw (0.75 relief floor) through siege (1.75 crisis peak); preserved second_winter slots into the high band by its own values.
- **Steam-trip simulation:** deferred (no consumer); authored values follow the plan's relative ordering within [0.0, 0.15].
- **Duration-weighted pressure:** siege (45) and long_winter (62.5) dominate cumulative exposure — intended endgame ramp, flagged for telemetry follow-up.
- **Transition shocks:** all large deltas coincide with authored phase turns; no unjustified cliffs.
- **Changes from provisional values:** provisional bands honored; exact numbers chosen within bands to satisfy the test-pinned first_siege values (1.75/0.03), the preserved second_winter values, and the plan's orderings. Existing season values untouched.

## Verification

| Gate | Result |
|---|---|
| `--data-integrity-selftest` | PASS — 0 errors / 0 warnings / 298 catalogs |
| `dotnet test Ashfall.Core.Tests` | PASS — 9461/9461 |
| `dotnet build Ashfall.csproj` | PASS — 0 errors / 0 warnings |
| Duty-roster selftest | PASS — 40/40 |
| Duty-roster season catalog tests | PASS (within full suite: 10 season-specific facts incl. boundaries, overflow, negatives, contiguity, save round-trip) |
| Determinism (selection oracle) | PASS — same day ⇒ same season, order/locale/restart independent |

## Core change disclosure

One minimal, justified addition: `DutyRosterCatalog.GetSeasonForDay(int)` — a pure selector with full doc comment. No state, no second clock, no behavior change to any existing path. All other deliverables are pure data + documentation.

## Deferred follow-ons

- `steamTripChanceBoost` consumer (composition + clamp definition) when an external-trip mechanic adopts it;
- Plan 70 schedule-season coupling via `GetSeasonForDay`;
- Plan 57 incident-family seasonal weighting (single-application guarantee already documented);
- Plan 48 weather naming reconciliation (social-vs-meteorological labels);
- Late-campaign telemetry pass on duration-weighted pressure (long_winter dominance);
- UI presentation of the active roster phase if later UX work justifies it.
