# Plan 77 — Baseline

Verified 2026-09 from active source.

## Catalog & DTO

`Assets/StreamingAssets/Data/duty_roster_seasons.json` — wrapped list (`schema_version` + `items`), loaded by `DutyRosterCatalogLoader` into `DutyRosterCatalog.Seasons` (file order preserved, no sorting).

`DutyRosterSeasonEntry` fields (exactly five, all live schema):

| Field | Type | Baseline value (`season_second_winter`) |
|---|---|---|
| `id` | string | `season_second_winter` |
| `windowMinDays` | int | 8 |
| `windowMaxDays` | int | 12 |
| `encounterWeight` | float | 1.6 |
| `steamTripChanceBoost` | float | 0.08 |

Baseline catalog count: **1 season**.

## Selection semantics (proven)

- **No day selector existed.** `DutyRosterCatalog` exposed only `GetSeason(string id)`. The pre-existing test contract (`Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs`) requires `GetSeasonForDay(int)`.
- **Bounds are inclusive/inclusive** (test: day 7 → first_ashfall, day 8 → second_winter, day 12 → second_winter, day 13 → settling, day 365 → long_winter). Contiguity rule: `next.min == current.max + 1`.
- **First supported day: 0** (test: first season min must be 0; negative days → null).
- **Post-final-window policy: carry-forward.** Days > 365 resolve to the last season (repository convention shared with `WeatherSystem.GetSeasonForDay` / `WildlifeSeasonalCalendar`: last window whose start ≤ day).
- **Exactly-one-match property** asserted for every day 0–365.
- JSON order is nonsemantic for correctness; tests assume chronological authoring (`Seasons[0].min == 0`). Selector is order-robust (max `windowMinDays ≤ day`, first-listed wins ties).

## Campaign-day authority

`SimClock` (day counter) is the authoritative campaign clock; `DutyRosterHostSession.Clock.Day` feeds all roster logic. Season selection is a pure function of that day — no second clock, no persisted season state.

## Modifier consumers

### `encounterWeight` — LIVE consumer
Chain: catalog season → `DutyRosterHostSession.ActivateSecondWinter()` → `ShelterEncounterSystem.SetSecondWinter(multiplier, day)` → `encounterWeightMultiplier` state.
- **Replacement semantics** (assignment, not accumulation): `multiplier <= 0 → 1`; `ClearSecondWinter()` resets to 1.
- Persisted in `ShelterEncounterSystemState` (save round-trip covered by existing codec tests).
- It is a relative weight multiplier on shelter-encounter pressure — **not a probability**.

### `steamTripChanceBoost` — authored data, no live consumer
No runtime consumer exists outside catalog/demo/tests. The only "steam trip" mechanic in Core is `BrineWaterSystem`'s membrane-integrity threshold event, which is unrelated and does not read this field. Validated range [0.0, 0.15] only. **Documented as deferred; no consumer was invented.**

## Save behavior

Active season is **derived, not persisted**. `DutyRosterSaveCodec` persists sim day + encounter multiplier state; on restore, the season re-resolves from the restored day (pinned by `SaveRoundTrip_RestoresSimDayAndReResolvesActiveSeasonCleanly`, day 150 → `season_first_siege`).

## Cross-plan availability

| System | Status |
|---|---|
| Weather seasons (`weather_seasons.json`, `WeatherSystem.GetSeasonForDay`) | live — alignment only |
| Plan 74 chapters | display-only metadata, day-agnostic |
| Plan 57 incidents | no season wiring |
| Plan 70 schedules | not present in this area |

## Tests referencing the existing season

- `DutyRosterSeasonCatalogTests.Catalog_PreservesSecondWinterIdentityAndValues` (pins id + all four values)
- `DutyRosterSystemTests` (value parity with `DutyRosterIds` constants)
- `DutyRosterHeadlessDemo` (profile presence, window/weight parity)

## Baseline exit-gate answers

Both bounds inclusive · day 0 valid (campaigns start day 1, day 0 is the ashfall arrival day) · no max playable day (SimClock unbounded) · post-365 carries forward · overlaps forbidden (exactly-one) · gaps forbidden (contiguous 0–365) · JSON order nonsemantic · `encounterWeight` = replacement multiplier on shelter-encounter pressure · `steamTripChanceBoost` = range-validated authored data, consumer deferred · neither cached as season identity in saves · `season_second_winter` referenced by tests/`DutyRosterIds` only (stable ID preserved) · weather seasons ≠ duty seasons (separate authorities) · chapters are day-agnostic display rows.
