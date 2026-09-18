# Task A1 Implementation Log: C1[11] Plan 38 — The Year Turns

**Date:** 2026-09-17\
**Wave:** Wave 11 Part 1 (Continuity Wave 5)\
**Package:** A1 — C1[11] Plan 38: "The Year Turns: Seasons, Deadlines, and a Clock With Teeth"\
**Status:** SEALED\

---

## 1. Summary of Changes

Implemented the unified calendar read model, deterministic seasonal context, and generalized commitments/deadline engine:

1. **Calendar Read Model (`CampaignCalendarReadModel`):**
   - Implemented in `Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs`.
   - Immutable pure projection exposing: `Day`, `SeasonId`, `SeasonDisplayName`, `SeasonIndex`, `SeasonProgress` (0.0 to 1.0), `DaysIntoSeason`, `DaysToSeasonEnd`, `Year` (1-based), `Chapter` (1-based), `AmbientTemperatureC`, `SeasonalSeverity`, `DayLengthHours`, `MigrationBias`, `PreservationBias`.
   - Engine-free domain logic.

2. **Authoritative Calendar Extensions (`ICampaignCalendar`, `CampaignCalendar`):**
   - Extended `ICampaignCalendar` in `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` with:
     - `CampaignCalendarReadModel CurrentReadModel { get; }`
     - `CampaignCalendarReadModel ResolveDay(int day)`
     - `void BindProfile(SeasonProfileDef? profile)`
     - `event Action<string, string>? OnSeasonChanged`
   - Multi-year wrap and chapter projection (`365` days per year/chapter matching `GenerationalSuccessionEngine`).
   - Deterministic continuous ambient temperature baseline matching `YearOfAshTimelineSystem` for days 180–360 and smooth annual cycle.
   - Exact boundary change detection on day advancement with `OnSeasonChanged` invocation.

3. **Commitments & Deadlines Engine (`CommitmentSystem`):**
   - Created `Assets/Ashfall.Core/Commitments/`:
     - `CommitmentDefinition.cs`: Authored DTOs and container.
     - `CommitmentReadModel.cs`: Immutable status projection (`Pending`, `Warning`, `Active`, `Met`, `Missed`).
     - `CommitmentCatalogLoader.cs`: Strict loader and validator for `commitments.json`.
     - `CommitmentSystem.cs`: Full lifecycle manager implementing `IDayAdvanceOwner` (`ownerId = "commitments"`, phase 4) and `IPreDaySnapshotRestore` (fail-closed preflight rollback).
     - Exactly-once terminal guarantee: obligations in `met_ids` or `missed_ids` never refire or flip state.
     - Warning threshold emissions at `due_day - warning_lead_days`.
     - Consequence routing via `OnConsequenceRouted` callback.
     - Save capture and restore (`CommitmentSaveState`) with full reload parity.

4. **Data Authority (`commitments.json`):**
   - Created `Assets/StreamingAssets/Data/commitments.json` with 3 schema-valid mechanism samples referencing real catalog entities (`faction_supply_corps`, `faction_grain_exchange`, `faction_the_office`, `item_grain_flour`, `item_water_filter`, `event_eco_blight_decimates_forage`).

5. **Catalog Integrity (`CatalogIntegrityValidator`):**
   - Added `"commitment_"` to `IdPrefixes`.
   - Added `ValidateCommitments` in `CatalogIntegrityValidator.cs` verifying schema version, window order, warning lead time, and non-empty consequence targets.

---

## 2. Verification Evidence

- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CommitmentSystemTests.cs`: 7/7 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`: 10/10 PASS
