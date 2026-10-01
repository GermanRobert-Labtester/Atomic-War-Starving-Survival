# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Status Glance, Game-Over Ledger & Supply Forecast (P013–P016)

STATUS: APPROVED BY USER — FULLY INTEGRATED (2026-10-01)

User directive (2026-10-01): "Proceed and work on these next tasks! | P013 … P014
… P015 … P016" plus the standing "repair leaks, bugs, missing tool calls,
missing panels, missing UI animations, warnings, errors … sweep loop of find
issue fix issue repeat for 5 loops."

Source: `docs/plans/PLAYABILITY_200_SUGGESTIONS_2026-10-01.md` Group 1, rows
P013–P016. Premises verified in source before editing (Rule 7).

## Bounded outcome

- **P013** — flag the day-1 acute-radiation start so the tutorial survivor is
  never lost to an *untaught* mechanic, and make the acute status recoverable.
- **P014** — a per-survivor cause-of-death ledger on the Game Over screen.
- **P015** — a warmth/temperature readout (roster warmth + clothing cold-loss
  reduction + shelter thermal) on `StatusPanel`.
- **P016** — a read-only "next 3 days: needs vs supply" forecast strip computed
  from the existing ration + inventory owners.

## Premise corrections (verified this session)

- The lethal premise is **false**: `starting_survivors.json` Mikhail seeds
  `acuteRad:true` with `RadiationDose = 15`, and `RadiationSystem` applies
  `HealthLossPerHourAtAcute` only at `dose >= AcuteThreshold (80)`. He takes no
  automatic health loss from the day-1 state.
- The real residual defects: (a) `HasAcuteRadiationSickness` is set and **never
  cleared** anywhere in Core, so the seeded acute sickness is un-resolvable even
  after anti-rad treatment; (b) it is invisible on the status glance (the
  `dose >= 50` objective misses a dose of 15).
- The P012 one-time `medical.acute_radiation` lesson already exists and teaches
  the mechanic (`Main.Medical.MaybeRequestAcuteRadiationLesson`, triggered from
  `Main.GameFlow`), so P013 is the *flag + recoverability* residual, not a new lesson.
- `StatusPanel` already renders `Warmth Drift` (P010) but has no absolute
  warmth/temperature readout and no forward-supply view.
- No `starting_survivors.json` change is required, so the data-owner hazard in
  the row is avoided.

## Non-goals

- No new onboarding lesson; no rebalance of the authored start data.
- No new save section; the forecast/thermal readouts are transient projections.
- No new gameplay authority — every number is read from the owning system.

## Files

Core: `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` (acute-status
resolution), `Assets/Ashfall.Core/Campaign/SupplyForecast.cs` (new pure
calculator).
Host/UI: `src/UI/StatusPanel.cs`, `src/UI/GameOverPanel.cs`,
`src/Main.SurvivorFate.cs`, `src/Main.GameFlow.cs`, `src/Main.PlayerSurfaces.cs`,
`src/Main.SupplyForecast.cs` (new partial).
Tests: `Ashfall.Core.Tests/UI/StatusGlanceAndForecastGateTests.cs` (new),
`Ashfall.Core.Tests/Radiation/AcuteSicknessResolutionTests.cs` (new).

## Verification

- Host build 0/0 (warning baseline 0).
- Focused: new gate tests, `SurvivalLegibilityGateTests`, `RootSweepSanity`,
  radiation/needs suites, `MainTriadDriftGateTests`.
- `--ui-layout-selftest` Failures 0, `--ui-accessibility-selftest` PASS,
  `--survivors-selftest`, `--real-campaign-journey-selftest`.
- 5-loop find/repair sweep; `git diff --check` clean; no commit.