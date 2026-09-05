# Duty Season Regression Matrix

> **Verification Matrix:** Complete test suite mapping for Plan 77.

---

| # | Test Scenario | Subsystem | Expected Outcome | Status |
|---|---|---|---|---|
| 1 | **Catalog Load Count** | `DutyRosterCatalogLoader` | Exactly 8 seasons loaded from `duty_roster_seasons.json` | Verified |
| 2 | **Preserve Second Winter** | `DutyRosterCatalog` | `season_second_winter` matches min 8, max 12, weight 1.6, boost 0.08 | Verified |
| 3 | **Unique Season IDs** | `DutyRosterCatalog` | All 8 seasons have unique IDs prefixed with `season_` | Verified |
| 4 | **Contiguous Coverage** | `DutyRosterCatalog` | Zero gaps between day 0 and day 365 (`next.min == curr.max + 1`) | Verified |
| 5 | **No Overlapping Ranges** | `DutyRosterCatalog` | No day between 0 and 365 matches more than one season | Verified |
| 6 | **Exact Boundary: Day 0** | `DutyRosterCatalog.GetSeasonForDay` | Matches `season_first_ashfall` | Verified |
| 7 | **Exact Boundary: Day 7/8** | `DutyRosterCatalog.GetSeasonForDay` | Day 7 matches `first_ashfall`; Day 8 matches `second_winter` | Verified |
| 8 | **Exact Boundary: Day 12/13**| `DutyRosterCatalog.GetSeasonForDay` | Day 12 matches `second_winter`; Day 13 matches `settling` | Verified |
| 9 | **Exact Boundary: Day 30/31**| `DutyRosterCatalog.GetSeasonForDay` | Day 30 matches `settling`; Day 31 matches `spring_thaw` | Verified |
| 10 | **Exact Boundary: Day 60/61**| `DutyRosterCatalog.GetSeasonForDay` | Day 60 matches `spring_thaw`; Day 61 matches `faction_pressure` | Verified |
| 11 | **Exact Boundary: Day 120/121**| `DutyRosterCatalog.GetSeasonForDay`| Day 120 matches `faction_pressure`; Day 121 matches `first_siege` | Verified |
| 12 | **Exact Boundary: Day 180/181**| `DutyRosterCatalog.GetSeasonForDay`| Day 180 matches `first_siege`; Day 181 matches `consolidation` | Verified |
| 13 | **Exact Boundary: Day 240/241**| `DutyRosterCatalog.GetSeasonForDay`| Day 240 matches `consolidation`; Day 241 matches `long_winter` | Verified |
| 14 | **Exact Boundary: Day 365** | `DutyRosterCatalog.GetSeasonForDay` | Matches `season_long_winter` | Verified |
| 15 | **Post-365 Overflow** | `DutyRosterCatalog.GetSeasonForDay` | Day 366+ returns `season_long_winter` fallback | Verified |
| 16 | **Negative Day Safety** | `DutyRosterCatalog.GetSeasonForDay` | Negative day returns `null` | Verified |
| 17 | **Modifier Bounds** | `DutyRosterSeasonEntry` | `encounterWeight` in [0.5, 2.5], `steamTripChanceBoost` in [0.0, 0.15] | Verified |
| 18 | **Save Round-Trip** | `DutyRosterSaveCodec` | Day advance and state round-trip cleanly with matching checksum | Verified |
| 19 | **Headless Demo Parity** | `DutyRosterHeadlessDemo` | `season_second_winter` checks continue to pass | Verified |
| 20 | **Large Day Jump** | `DutyRosterCatalog.GetSeasonForDay` | Jumping day 6 → 14 → 185 resolves respective seasons cleanly | Verified |
