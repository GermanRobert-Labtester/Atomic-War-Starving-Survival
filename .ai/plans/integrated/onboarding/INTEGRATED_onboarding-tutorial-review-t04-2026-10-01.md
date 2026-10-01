# T04 — ashfall-tutorial-review on the 7-stage onboarding (teach-vs-demand audit)

# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> Follow-up brush sweep (same session): two more truthfulness repairs landed after
> archiving. (1) **Status-bar completion copy** — `RefreshOnboardingStatusBar`
> returned early on `JourneyComplete`, leaving the final "CURRENT: …" objective
> stuck on the status label as a live claim; it now renders the localized
> `onboarding.status.first_hour_complete` copy (TDD gate
> `StatusBar_OnJourneyCompletion_ShowsTheCompleteLineNotAStaleObjective`, red→green).
> (2) **GUIDED no-op tier removed from the cycle** — `OnboardingAssistance.Guided`
> had no distinct behavior (no auto-highlight exists), yet the panel offered it as
> a third press and promised "extra help" in the tooltip/CSV (the silent-mismatch
> class). The cycle is now truthful two-tier MINIMAL⇄STANDARD; legacy Guided saves
> render the STANDARD label (`AssistanceLabel` maps the reserved enum); the enum
> doc and the CSV tooltip row (EN+DE) were corrected. 3 new TDD gates
> (cycle-offer, label-mapping, tooltip) red→green. All re-verified: loop 6/6,
> truth 5/5, wiring 4/4, journey 34/34, strings-csv 4/4, localization-pilot 4/4,
> l10n drift PASS (365 keys), host build 0 errors, `--onboarding-journey-selftest`
> PASS, `--ui-accessibility-selftest` PASS, `git diff --check` clean.
> Full details in the body below._

**Owner:** sweep/integrator agent (this task). **Kind:** audit + bounded repair.

## Outcome

Re-run the `ashfall-tutorial-review` skill against the **live 7-stage first-hour
journey** (`OnboardingProfile.FirstHour` → Water · Power · Food · Duty · Dose ·
Research · Expedition), verify every teach-vs-demand claim against current code and
data (code is truth, planning docs are intent), fix engineering defects found by
the sweep (repair + harden), and publish the refreshed
`docs/onboarding/TUTORIAL_REVIEW.md`.

## Non-goals

- No new gameplay authority, save section, or Core behavior change.
- No content rewrites (prose belongs to `ashfall-write`); copy-drift findings are
  ranked proposals in the review, not silent edits.
- No difficulty/balance tuning (belongs to `balance-sim`).
- No changes to the deferred items owned elsewhere (DailyBriefing `SKIP [Tab]`
  dead-zone → Plan 37 / C2[15]; `water_treatment` dashboard-rail entry → product).

## Findings (evidence-first, swept twice)

1. **DEFECT — unbounded assistance-cycle event recursion.** `OnboardingHintPanel`
   `CycleAssistance()` raises `OnAssistanceChanged`; the host handler
   `Main.SetOnboardingAssistance` calls back into `CycleAssistance`, which re-raises
   the event → synchronous infinite loop → stack overflow on a single press of the
   `ASSISTANCE:` cycle button (OnboardingHintPanel.cs:265-270,
   Main.Onboarding.cs:235-243, wiring at Main.Onboarding.cs:95). No guard exists.
2. **DEAD BOOKKEEPING** — `Main._onboardingFailedActions` /
   `_onboardingLastInteractionSeconds` are written (Main.Onboarding.cs:156,161-162),
   never read; the class comment promises a "contextual-hint heuristic" that does not
   exist. `ObserveFailedAction` already triggers the real behavior
   (`_onboardingHintPanel?.RefreshView()`).
3. **COPY DRIFT (ranked proposals only)** — `TutorialPanel` basics teach
   "Above 50 mSv, Acute Sickness takes −5 HP/hr" and "~3.6 clean units a day";
   code truth is `RadiationSystem.AcuteThreshold = 80f` on a 0–100 acute dose scale
   (health −5/hr at/above 80; no 50 mSv acute threshold exists) and the ration
   day-tick consumes 3 clean water units (Standard) / 2 (Half). The same "50 mSv"
   claim ships in `codex.manual.radiation` (Core + strings.csv + JournalPanel).
4. **TEACH-VS-DEMAND GAP (highest-ranked content finding)** — Gunner Mikhail starts
   Day 1 with `acuteRad: true` and health 72 (starting_survivors.json) — the most
   time-critical Day-1 pressure — but the 7-stage journey and the live contextual
   lessons (expedition.protection / weather.storm_prep / wildlife.*) never teach
   "treat acute radiation in Medical". Only the passive TutorialPanel tip list
   mentions it. Propose an env-contextual Medical lesson or a stage nudge.
5. Weather remains intentionally outside the 7-stage arc (deferred 2026-09-25);
   the `weather.storm_prep` contextual lesson covers the severe-day path. Re-affirmed.
6. MINOR — when the journey completes, the hint panel disables **all** actions
   including REPLAY; a completed player cannot re-read the checklist without
   Settings → RESET TUTORIALS. Flagged, not changed (friction decision).

## Changes (bounded repair)

| File | Change |
|---|---|
| `src/UI/OnboardingHintPanel.cs` | `CycleAssistance` stops re-raising `OnAssistanceChanged`; the button handler remains the single raise site (host-driven update path cannot re-enter the host handler). |
| `src/Main.Onboarding.cs` | Remove never-read `_onboardingFailedActions` / `_onboardingLastInteractionSeconds`; remove dead private `FlushOnboardingIfDirty` wrapper (no callers); correct the stale class comment. |
| `Ashfall.Core.Tests/UI/OnboardingAssistanceLoopGateTests.cs` | NEW source gate: `CycleAssistance` body must not raise `OnAssistanceChanged`; `OnCycleAssistanceClicked` body must raise it (single raise-site contract). TDD-proven pre-fix failure. |
| `docs/onboarding/TUTORIAL_REVIEW.md` | Refreshed teach-vs-demand audit (2026-10-01) incl. defects found/closed and ranked proposals. |
| `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `.ai/state.md` | T04 entries (routine ledger update for this claim). |

## Verification

- TDD: new gate fails before the fix, passes after.
- `scripts/run_test.sh` focused: `OnboardingAssistanceLoopGateTests`,
  `OnboardingWiringGateTests`, `OnboardingTruthfulnessGateTests`,
  `OnboardingJourneyTests`.
- `dotnet build Ashfall.csproj` — 0 new errors.
- Headless `godot --headless --path . -- --onboarding-journey-selftest` (impacted
  runtime path) when the host invocation is available.
- `git diff --check` clean; no commit; full suite not run; foreign dirty worktree
  preserved verbatim.

## Acceptance

- Every Day-1..3 lethal pressure has a teaching verdict with code/data evidence in
  the review (quality gate).
- The assistance-cycle recursion is structurally impossible again (gate).
- No new authority, no save-section change, no Core change.