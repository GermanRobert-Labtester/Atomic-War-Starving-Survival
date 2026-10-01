# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# First-Hour Instrumentation & Tutorial Ordering (P001–P004)

STATUS: APPROVED BY USER — FULLY INTEGRATED (2026-10-01)

User directive (2026-10-01): "start working, coding on these tasks as well as
repair any leaks, bugs, missing tool calls or receiving tool calls, missing
panels, missing UI animations, warnings, errors … run a sweep loop of find
issue fix issue repeat for 5 loops."

Source: `docs/plans/PLAYABILITY_200_SUGGESTIONS_2026-10-01.md` Group 1, rows
P001–P004. Premises verified in source before editing (Rule 7).

## Bounded outcome

Make the first-hour onboarding measurable, ordered, and honestly reported:

- **P001** — an append-delta mode for the first-hour funnel tool so every
  onboarding/playability batch records its delta in
  `docs/telemetry/FIRST_HOUR_FUNNEL.md`.
- **P002** — one metric action per first-hour verb
  (`water.start`, `power.breaker`, `food.consume`, `duty.assign`, `dose.open`,
  `research.start`, `expedition.dispatch`), sourced from the canonical
  `PlayerCommandCode` vocabulary; recorded alongside (never replacing) the
  motivating sigil.
- **P003** — in full-onboarding mode, the terminal Expedition dispatch is gated
  behind the Water→Dose prerequisite stages and fails instructively (naming the
  pending stage) through the existing `CommandResult.ContextBlocked` seam.
- **P004** — `hint_shown` / `hint_dismissed` metric counters per stage emitted
  by `OnboardingHintPanel` through the existing play-metrics recorder.

## Non-goals

- No new save section, no new panel, no parallel telemetry authority.
- No change to the deterministic Core journey math or the sigil vocabulary.
- No rebalance of the survival loop.

## Authority / owners extended

| Concern | Owner extended |
|---|---|
| First-hour stage catalog + ordering | `Ashfall.Core.Onboarding.OnboardingCatalog` / `OnboardingJourney` |
| Player command vocabulary | `Ashfall.Core.PlayerCommand.PlayerCommandCode` |
| Local metric event vocabulary | `Ashfall.Core.Telemetry.PlaySessionRecorder` / `PlaySessionActions` |
| Dispatch gating | `src/Host/ExpeditionHostSession` (existing optional-gate seam) |
| Metric emission | `src/Main.PlayMetrics.cs` + `src/Main.Onboarding.cs` |
| Hint presentation | `src/UI/OnboardingHintPanel.cs` |

## Verification

- New focused Core test file `Ashfall.Core.Tests/Onboarding/OnboardingFirstHourInstrumentationTests.cs`.
- `bin/run-scoped-tests Ashfall.Core.Tests/Onboarding/` (existing + new).
- `bin/run-scoped-tests Ashfall.Core.Tests/UI/OnboardingWiringGateTests.cs`,
  `.../Campaign/Plan46PlayMetricsHostIntegrationTests.cs`,
  `.../Telemetry/PlaySessionRecorderTests.cs`.
- `dotnet build Ashfall.csproj` (0 errors).
- `godot --headless -- --playable-metrics-selftest` and
  `--onboarding-journey-selftest`.
- `python3 scripts/tools/first_hour_funnel.py --selftest`.
- `--ui-layout-selftest` if UI changes regress.

## Sweep

Five find→fix→reprobe loops over the changed seams, then a consolidated report.