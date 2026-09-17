# A1 Briefing Deferred — Claim-Safe Consumer Handoff Record

**Task:** A1 — C1.4 Crisis Prediction
**Status:** RESOLVED (Core predictor complete & verified; DailyBriefing consumer integrated 2026-09-17)
**Date:** 2026-09-17

## Resolution (2026-09-17)

The Plan 24 claim's tasks are all recorded DONE, so the deferred consumer was
integrated as an additive claim handoff (no Plan 24 seam modified):

- `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs`: new
  `AppendCrisisWarnings(report, predictions, maxEntries)` renderer + bounded
  "Crisis Warnings" section. Additive only — existing methods untouched.
- `src/Main.BriefingCrisis.cs` (new): assembles read-only
  `CrisisPredictionInputs` from canonical owners (roster, inventory, power
  grid, sanitation spill, weather intelligence) and calls
  `CrisisPredictor.Evaluate`. No RNG, no mutation, no persistence.
- `src/Main.Campaign.cs`: one call to `AppendCrisisWarnings(...)` in
  `ShowBriefingForDay` before the empty check (both event and fallback paths).
- `Ashfall.Core.Tests/Campaign/DailyBriefingCrisisTests.cs` (new, 8 cases):
  empty/null no-op, section rendering, horizon wording, bounded overflow,
  real-predictor consumption, and healthy-input no-op.

Inputs without a clear canonical source (daily burn rates, distress ambush,
working-adult demographics) are intentionally left at the predictor's neutral
defaults rather than invented; they produce no false prediction. Documented in
the `Main.BriefingCrisis` XML doc.

## Completed Authority / Mechanism
- `Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs`: Pure read model, immutable projection record (`CrisisPredictionRecord`), and deterministic evaluator (`CrisisPredictor.Evaluate`).
- Evaluates food, water, power, radiation, pathogen/sanitation, distress ambush, demographic collapse, and severe weather forecasts.
- 0 RNG, 0 mutation, 0 persistence, bounded confidence, monotonic horizon approach.
- `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs`: Read model extended with typed crisis prediction fields (`predictedCrisisEventId`, `predictedCrisisDay`, `predictedCrisisConfidence`, `crisisPreparationAdvice`, `predictedWeatherKind`, `hasPredictedCrisis`, `daysUntilPredictedCrisis`).
- `src/UI/WeatherForecastPanel.cs`: Crisis alert banner rendered with warning icon, projected day, confidence, and advice.
- Tests: `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs` (16/16 passed).

## Deferred Consumer
- Presentation of crisis warnings inside `DailyBriefingReportBuilder.cs`.

## Claim Owner & Blocked Paths
- **Active Claim:** `claim-c1-plan24-survivor-ledger-2026-09-16` (`C1-PLAN24-SURVIVOR-LEDGER`)
- **Claim Owner:** Integrator (user-authorized)
- **Blocked Paths:**
  - `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs`
  - `src/Main.CampaignOwners.cs`
  - `Ashfall.Core.Tests/Campaign/DailyBriefingReportBuilderTests.cs`

## Why No Workaround Was Created
Per AGENTS.md rule 5 ("One authority per concern") and Section 2.10 of the Wave 9 Master Plan, creating a shadow or parallel briefing system would pollute authority and cause merge conflicts with the active Plan 24 package.

## Required Integration Action After Handoff
Once the Plan 24 integrator releases the claim:
1. In `DailyBriefingReportBuilder.cs`, add a section or warning entry consuming `CrisisPredictor.Evaluate(...)` when active crisis predictions exist.
2. In `DailyBriefingReportBuilderTests.cs`, add assertions verifying that predicted crises surface in briefing output.

## Tests Already Passing
- `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`: 16/16 PASS
- `Ashfall.Core.Tests/WeatherIntelligenceCoordinatorTests.cs`: 12/12 PASS
- `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`: 18/18 PASS
