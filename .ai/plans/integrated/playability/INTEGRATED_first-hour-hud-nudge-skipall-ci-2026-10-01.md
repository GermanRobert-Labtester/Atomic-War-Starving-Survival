# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# First-Hour HUD, Stage Nudge, Skip-Tutorial & CI Floor (P005–P008)

STATUS: APPROVED BY USER — FULLY INTEGRATED (2026-10-01)

User directive (2026-10-01): "start working, coding on these tasks as well as
repair any leaks, bugs, missing tool calls or receiving tool calls, missing
panels, missing UI animations, warnings, errors … run a sweep loop of find issue
fix issue repeat for 5 loops."

Source: `docs/plans/PLAYABILITY_200_SUGGESTIONS_2026-10-01.md` Group 1, rows
P005–P008. Premises verified in source before editing (Rule 7).

## Bounded outcome

Finish the first-hour funnel legibility set on the **existing** onboarding
authority (`OnboardingJourney` / `OnboardingHintPanel` / `GameHudOverlay`):

- **P005** — a truthful, non-blocking "N days on this stage" nudge. Requires a
  persisted stage-entry day on the onboarding save state (additive field +
  restore path), so the count survives save/load instead of resetting.
- **P006** — first-hour stage progress in the HUD (`1/7 · WATER`), sourced from
  the same catalog/position + `StageLocalizationId` the panel already uses.
- **P007** — a single visible "skip tutorial" affordance wired to the existing
  dead `Main.SkipAllOnboardingStages` seam (no new skip authority).
- **P008** — a registered CI fast gate that runs `--onboarding-journey-selftest`
  and fails below the recorded 7-stage floor (a floor guard inside the selftest
  plus the manifest entry).

## Non-goals

- No new onboarding stage, no reorder of `FirstHourOrder`.
- No parallel progress/telemetry authority; the HUD and nudge read the journey.
- No changes to `Main.GameFlow.cs` HUD update path (the stage label is not
  touched by `UpdateState`, so it stays valid across day ticks).
- No full test suite run (banned unless `RUN FULL TESTS`).

## Evidence (verified this session)

- `OnboardingCatalog.FirstHourOrder` = Water/Power/Food/Duty/Dose/Research/
  Expedition (7). `Main.SkipAllOnboardingStages()` had zero callers
  (`docs/onboarding/TUTORIAL_REVIEW.md`).
- `GameHudOverlay.UpdateState` sets day/value/faction/weather only.
- `--onboarding-journey-selftest` exists and passes; it is **not** registered in
  `docs/ci/CI_GATE_MANIFEST.json` (only `--day1-selftest` is).
- `OnboardingSaveState` is the persisted owner; `Main.Onboarding.cs`,
  `OnboardingHintPanel.cs` are tracked l10n pilot surfaces (drift gate).

## Files

Core: `Assets/Ashfall.Core/Onboarding/OnboardingSaveState.cs`,
`Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`.
Host/UI: `src/UI/GameHudOverlay.cs`, `src/UI/OnboardingHintPanel.cs`,
`src/Main.Onboarding.cs`, `src/Host/HostCli.Onboarding.cs`,
`docs/ci/CI_GATE_MANIFEST.json`, `assets/l10n/strings.csv`.
Tests: `Ashfall.Core.Tests/Onboarding/OnboardingFirstHourInstrumentationTests.cs`.

## Verification

- Host build 0 errors / 0 warnings.
- `--onboarding-journey-selftest` PASS (floor guard green).
- `--ui-accessibility-selftest` PASS, `--ui-layout-selftest` Failures 0.
- Focused: `OnboardingFirstHourInstrumentationTests`, `OnboardingJourneyTests`,
  `OnboardingWiringGateTests`, `OnboardingAssistanceLoopGateTests`,
  `OnboardingTruthfulnessGateTests`, `FirstHourPlaythroughSmokeTests`,
  `LocalizationRatchetTests`, `StringsCsvLocaleGateTests`,
  `CiGateManifestDriftTests`; `l10n_drift_gate.py` PASS.
- 5-loop find/repair sweep; `git diff --check` clean; no commit.