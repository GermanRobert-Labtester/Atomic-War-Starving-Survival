# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Onboarding/slice truthfulness — 5-loop repair + hardening sweep (2026-10-01)

STATUS: APPROVED BY USER
(User mandate: "please pass a repairing and hardening sweep, 5 loops and then
deep validation and repair anything that may be off!")

## Goal

Run five bounded repair/hardening loops over the T03 onboarding-truthfulness
domain and the `ae6e54387` Day-Goal code, then deep-validate and repair every
real defect found. No new authority, no parallel state.

## Loops and findings

- **Loop 1 — re-audit T03.** `Main.Onboarding.RefreshOnboardingStatusBar`
  carried a stale comment ("Append rather than overwrite") while the code
  overwrites. Comment corrected to match behaviour.
- **Loop 2 — presentation truthfulness.** `OpeningProtocolModal.RefreshGoal`
  and `Main.Campaign.ShowBriefingForDay` joined the slice goal as
  `$"{title} — {body}"`, so a beat with an empty body would render a dangling
  `—` (the same empty-join class as `ae6e54387`); the modal also hid a
  body-only beat. `OnboardingHintPanel.RefreshView` still showed the terminal
  stage's hint after `JOURNEY COMPLETE`. All three repaired.
- **Loop 3 — cross-file contract drift.** Verified `OnboardingCatalog`
  title/objective (both profiles, 14 stages) exactly matches `strings.csv`
  en — 0 mismatches; panel and status bar share one normalization.
- **Loop 4 — hardening.** Added the `SliceGoalText.Join` Core authority and
  `TryGetSliceGoal` empty-beat rejection; added a 5-case `[Theory]` and a
  host-wiring source gate.
- **Loop 5 — deep validation.** Build, focused xUnit, host selftests, l10n
  gate, fabrication gate, and `--ui-layout-selftest` all green.

## Files

- `Assets/Ashfall.Core/Campaign/SliceScenario.cs` — new `SliceGoalText.Join`.
- `src/UI/OpeningProtocolModal.cs` — uses the join; body-only beats show.
- `src/Main.Campaign.cs` — uses the join for the briefing goal.
- `src/Main.SliceScenario.cs` — `TryGetSliceGoal` rejects copy-less beats.
- `src/UI/OnboardingHintPanel.cs` — clear the hint when the journey completes.
- `src/Main.Onboarding.cs` — truthful status-bar comment.
- `Ashfall.Core.Tests/Campaign/Plan54SevenDaySliceIntegrationTests.cs` — join theory.
- `Ashfall.Core.Tests/Campaign/Plan54SevenDaySliceHostIntegrationTests.cs` — host wiring gate.

## Non-goals

- No new save section, mutable state, or parallel goal/objective ledger.
- No change to the slice catalog data or the onboarding journey machine.
- No Unity, no full test suite, no snapshot rebaseline.

## Verification

| Check | Result |
|---|---|
| `dotnet build Ashfall.csproj` | 0 errors (6 pre-existing warnings) |
| `Plan54SevenDaySliceIntegrationTests` | 10/10 |
| `Plan54SevenDaySliceHostIntegrationTests` | 7/7 |
| `OnboardingTruthfulnessGateTests` | 4/4 |
| `OnboardingWiringGateTests` | 4/4 |
| `StringsCsvLocaleGateTests` | 4/4 |
| `ProductionUiNoFabricatedFallbackGateTests` | 4/4 |
| `scripts/ci/l10n_drift_gate.py` | PASS (365 keys, 97 refs) |
| `--day1-selftest` | PASS |
| `--seven-day-slice-selftest` | 25/25 |
| `--onboarding-journey-selftest` | PASS |
| `--ui-layout-selftest` | `blankUnboundPanels=0`, Failures: 0 |
| `git diff --check` | clean |

## Integration record (2026-10-01)

All four defects repaired. `SliceGoalText.Join` is the one join authority for
the slice goal line; `TryGetSliceGoal` reports false for a beat with neither
title nor body so an empty "Today's Goal" can never reach the modal or the
briefing. The panel clears its hint line once the journey completes. The
status-bar comment now matches its overwrite behaviour. A concurrent agent's
`Main.Campaign.cs` WorldIncidents edit was preserved verbatim. No commit; full
suite not run; foreign dirty worktree preserved.
