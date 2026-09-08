# Plan 77 — Regression Matrix

## Verification evidence

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 errors, 0 warnings |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — **9461/9461** |
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 errors, 0 warnings, 298 catalogs |
| `godot --headless --path . -- --duty-roster-selftest` | PASS — 40/40 |
| Independent Python selection oracle | PASS — all boundaries, overflow, negatives, exactly-one-match ×366 days, pinned values, balance orderings |

No tests were disabled or weakened. No validator was relaxed.

## DoD checklist

### Catalog
- [x] 8 valid season entries — `Catalog_LoadsExact8SeasonsWithValidSchema`
- [x] `season_second_winter` preserved (id 8–12, 1.6, 0.08) — `Catalog_PreservesSecondWinterIdentityAndValues` + `DutyRosterSystemTests` parity with `DutyRosterIds`
- [x] 7 new unique `season_*` IDs — `Catalog_ContainsAll7NewSeasonsWithCorrectIds`, `Catalog_ZeroDuplicateIds`
- [x] Numeric fields within accepted ranges ([0.5,2.5] / [0.0,0.15]) — schema test per-entry
- [x] No unsupported/dead fields added (schema has exactly 5 fields; prose lives in docs)

### Window semantics
- [x] Inclusive/inclusive documented from tests + oracle — Schema/Boundary docs
- [x] Every transition boundary tested — `Selection_ExactTransitionBoundaries` (16 InlineData cases) + boundary matrix
- [x] No overlaps / no gaps — `Catalog_ContiguousGapFreeAndNoOverlapsAcross365Days`, oracle exactly-one-match on all 366 days
- [x] Every supported day resolves per policy — `Selection_EveryDayFrom0To365ResolvesExactlyOneSeason`
- [x] Day 365/366 explicit — final window max 365; overflow carry-forward tested
- [x] First playable day explicit — day 0 valid; negatives null

### Runtime semantics
- [x] `encounterWeight` consumer + formula documented (replacement multiplier via `ShelterEncounterSystem.SetSecondWinter`)
- [x] `steamTripChanceBoost` documented as range-validated authored data, consumer deferred
- [x] Modifiers apply exactly once (assignment semantics; no second multiplication site)
- [x] Season replacement, not accumulation (setter overwrites; ClearSecondWinter resets)
- [x] No second campaign clock (pure `GetSeasonForDay(day)` derivation from `SimClock` day)

### Reachability & balance
- [x] All 8 phases reachable (contiguous 0–365, unbounded campaign) — unreachable count: 0
- [x] Campaign-duration reconciled (no day cap exists; 365-arc is valid, post-365 defined)
- [x] Encounter/trip profiles differentiated per plan bands; orderings verified in oracle
- [x] Duration-weighted exposure computed; `long_winter` dominance flagged for telemetry follow-up
- [x] Adjacent-delta shock audit clean (all large deltas at genuine phase boundaries)

### Cross-system
- [x] Weather authority untouched (`WeatherSystem`); alignment documented incl. two deliberate label divergences
- [x] Chapters untouched (day-agnostic display rows); correlation documented
- [x] Incidents/schedules: no wiring, documented as follow-ons
- [x] No circular dependencies; no unresolved cross-plan IDs in JSON

### Persistence & determinism
- [x] Save round-trip with re-resolution pinned (day 150 → first_siege)
- [x] Derived season ⇒ rollback/jump/overflow safe by construction; behaviors pinned by tests
- [x] Catalog ordering nonsemantic (selector order-robust; ties first-listed — deterministic)

## Final catalog

| # | id | window | encounterWeight | steamTripChanceBoost |
|---:|---|---|---:|---:|
| 1 | season_first_ashfall | 0–7 | 1.45 | 0.01 |
| 2 | season_second_winter | 8–12 | 1.60 | 0.08 |
| 3 | season_settling | 13–30 | 1.00 | 0.06 |
| 4 | season_spring_thaw | 31–60 | 0.75 | 0.12 |
| 5 | season_faction_pressure | 61–120 | 1.30 | 0.05 |
| 6 | season_first_siege | 121–180 | 1.75 | 0.03 |
| 7 | season_consolidation | 181–240 | 1.00 | 0.09 |
| 8 | season_long_winter | 241–365 | 1.50 | 0.02 |
