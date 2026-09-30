# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# First-Hour Goal + First-Screen Noise — items 3 + 4 (2026-09-30)

STATUS: APPROVED BY USER
(Batch mandate: "start working on these, code them!" — items 3 and 4 of
the 2026-09-30 first-hour audit, 2026-09-30 session.)

## Goal

1. **Item 3 — first-screen noise.** The first-hour onboarding stages
   `Duty` and `Dose` (added 2026-09-25) never received their
   `onboarding.*` localization rows, so the status bar and hint panel
   fall back to hardcoded English and the hint line renders as
   "HINT: —". Add the `onboarding.duty.*`, `onboarding.dose.*` (and the
   matching `onboarding.hint.duty` / `onboarding.hint.dose`) rows to
   `assets/l10n/strings.csv`, wire the two hint switch cases in
   `OnboardingHintPanel.BuildHintLine`, and extend the l10n drift gate's
   dynamic stage-key family to the full first-hour stage set.
2. **Item 3 — 9mm_ammo.** `economy_goods.json` still carries the legacy
   market-projection id `9mm_ammo`; the canonical 9mm item is
   `ammo_9x19` (used by items.json, combat_catalog, recipes, dive sites,
   expeditions). Rename the good id to `ammo_9x19` so the economy good
   resolves to the real item, and update the two tests that pin the old
   id.
3. **Item 4 — give Day 1 a goal.** Drive the authored Plan 54
   Seven-Day Slice beats (`slice_seven_days.json`, days 1–7) through
   the real opening modal and the daily briefing so the player is told
   what to do next: a `TryGetBeatForDay` accessor on the Core
   `SliceScenario` authority, a "TODAY'S GOAL" block on
   `OpeningProtocolModal`, and a leading "Today's Goal" section in
   `ShowBriefingForDay`. No new state, no new save section, no parallel
   goal system — the slice instrument already loads in the campaign
   lifecycle.

## Files

- `assets/l10n/strings.csv` — add duty/dose stage + hint rows (en + de).
- `src/UI/OnboardingHintPanel.cs` — Duty/Dose hint switch cases.
- `scripts/ci/l10n_drift_gate.py` — complete the dynamic stage-key family.
- `Assets/StreamingAssets/Data/economy_goods.json` — `9mm_ammo` → `ammo_9x19`.
- `Ashfall.Core.Tests/Plan56EconomyGoodsTests.cs`, `Plan56FollowUpTests.cs` — repin.
- `Assets/Ashfall.Core/Campaign/SliceScenario.cs` — `TryGetBeatForDay`.
- `src/Main.SliceScenario.cs` — `TryGetSliceGoal(day, …)` host helper.
- `src/UI/OpeningProtocolModal.cs` — goal block + `SetDayGoal`.
- `src/Main.UiPanels.cs`, `src/Main.GameFlow.cs`, `src/Main.PlayerSurfaces.cs` — goal refresh at modal open sites.
- `src/Main.Campaign.cs` — "Today's Goal" briefing section.

## Non-goals

- No new save section, no mutable goal state, no parallel objective ledger.
- No rewrite of the briefing builder or the onboarding journey machine.
- No Unity, no full test suite, no snapshot rebaseline.

## Verification

- `bin/run-scoped-tests` over the changed test files.
- `--seven-day-slice-selftest` (Plan 54 probe) still green.
- l10n drift gate + `StringsCsvLocaleGateTests` green.
- Host build 0 errors.

## Integration record (2026-09-30)

All three items landed and verified:

- strings.csv gained `onboarding.duty.title/objective`, `onboarding.dose.title/objective`,
  `onboarding.hint.duty`, `onboarding.hint.dose` (en + de);
  `OnboardingHintPanel.BuildHintLine` gained the Duty/Dose cases (the hint
  line no longer renders "HINT: —" on those stages); the l10n drift gate's
  dynamic stage-key family now enumerates every OnboardingCatalog stage.
- `economy_goods.json` id `9mm_ammo` renamed to the canonical `ammo_9x19`;
  Plan56 pinning tests repinned and the resolution test now proves the good
  resolves to the real item.
- `SliceScenario.TryGetBeatForDay` (Core, engine-free) + Main
  `TryGetSliceGoal`/`RefreshOpeningProtocolDayGoal` drive the authored beat
  onto `OpeningProtocolModal` ("TODAY'S GOAL" block, hidden when no beat)
  at all three open sites, and `ShowBriefingForDay` leads the briefing with
  a "Today's Goal" section for days 1–7.

Evidence: host build 0 errors; focused xUnit 33/33
(Plan56EconomyGoods, Plan56FollowUp, StringsCsvLocaleGate,
LocalizationPilot, Plan54SevenDaySliceHostIntegration);
`--seven-day-slice-selftest` 25/25; `--day1-selftest` PASS;
`--data-integrity-selftest` PASS (0 errors, 430 catalogs);
l10n drift gate PASS. No snapshot goldens cover these modals; no
rebaseline needed.
