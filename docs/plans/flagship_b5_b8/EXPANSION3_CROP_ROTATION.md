# §27 Follow-On Expansion 3 — Crop Rotation Ledger (B5–B8 continuation)

> Flagship §27: "richer seasonal greenhouse crop catalog and crop rotation
> without changing the core loop." The rotation half lands as a bounded Core
> mechanic riding the established blight-risk decomposition; the seasonal
> catalog expansion stays with the content stream (new items + recipes +
> nutrition profiles are a catalog-authoring task, not a code task).

## What landed

### Monoculture pressure (§7.7 "crop rotation if represented" — now represented)
- Additive `GreenhousePlotState.sameCropStreak` + `lastCropId` (the soil's
  rotation ledger — **survives harvest and Clear**, because the bed, not the
  tray state, remembers what grew on it; legacy saves restore 0/empty).
- `Plant`: the same crop again on the same bed builds
  `sameCropStreak` (capped at `MaxRotationStreakCount` = 10); planting any
  different crop resets it to zero. `RotationBlightStepPerStreak` = 0.015 —
  ten repeated crops add +0.15 to the daily blight chance, clamped.
- Rides the Phase 4 risk decomposition: `BlightRiskProfile` now carries
  `RotationPressure`/`RotationStreak`, and the TickDay outbreak roll consumes
  the identical number (UI projections stay truthful).
- **GreenhousePanel**: the risk row shows `monoculture ×n` and a rotation
  hint ("Repeated tuber crops exhaust this bed — rotate to another crop").

### Balance shape
- Rotation is a pressure, not a wall: a 10-streak bed at legacy baseline
  contamination still sits in the same risk bands as a moderately
  contaminated fresh bed. Crop choice becomes an ongoing, visible decision
  with zero new items, zero new UI, zero balance arbitrage.
- Drought blight, nutrient reduction, and prevention levers are unchanged.

## Verification

| Gate | Result |
|---|---|
| `GreenhousePhase4LoopClosureTests.cs` | 17/17 PASS (+4 rotation tests) |
| Phase 0 greenhouse fixture | re-captured (additive `sameCropStreak`/`lastCropId`); legacy-restore test pins zero history |
| `--data-integrity-selftest` | PASS |
| `--content-utilization-selftest` | PASS |
| `--panel-bind-lifecycle-selftest` | PASS |
| Full `dotnet test` | **11,006 / 11,010** — same 4 pre-existing failures; zero new |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` (rotation ledger, chance + profile integration)
- `src/UI/GreenhousePanel.cs` (rotation contributor + hint)
- `Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs` (+4 tests)
- `Ashfall.Core.Tests/Fixtures/B5B8_Phase0/greenhouse_phase0.json` (re-captured)
- `docs/plans/flagship_b5_b8/EXPANSION3_CROP_ROTATION.md` (this file)

## Design note

The seasonal *catalog* half of the §27 item (new winter-window crop rows,
per-crop season gates) remains a content-stream task: crops are code-defined
in `GreenhouseExpansionCatalog` with item/recipe/nutrition-profile crossrefs
in three data files — adding rows is catalog authoring with the existing
integrity gates, mechanically supported already by the winter-light model.
