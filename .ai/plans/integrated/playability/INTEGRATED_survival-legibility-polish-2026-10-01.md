# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility Polish (10 small tasks) — P009–P016 hardening

STATUS: APPROVED BY USER

User directive (2026-10-01): "Continue with these small tasks, after please again do a
loop of finding issues, repairing issues, hardening the spot where issues found …
repeat for 5 loops and then suggest 15 very small tasks that can be immediately
integrated fully, and cleanly!"

## Bounded outcome

Ten small, single-owner presentation/l10n/verification tasks on top of the integrated
P009–P016 survival-legibility seam. No new authority, no new save section.

1. Localize the threshold **value templates** (`ui.status.threshold.value.*`).
2. Fix `Signed` rendering `-0` for small negatives.
3. Register `StatusPanel`/`GameHudOverlay` in the l10n drift gate's `LOCALIZED_SURFACES`.
4. Expose `NeedDaySpan(int day)` on `SurvivorsHostSession`; use it in `StatusPanel`.
5. Span-annotate `SurvivorsPanel` per-row deltas (thread the day into `Bind`).
6. Add a `--day1-selftest` assertion that the acute-rad lesson is queued.
7. Bounded StatusPanel section/render gate (golden PNG needs a display session).
8. Needs-chip tooltips showing the warn/critical band from `NeedsProfile`.
9. `NeedsDayDeltaTracker` kind-coverage test (every `NeedKind` in `Tracked` reachable).
10. Unify the two cold predicates behind one `IsCold(float)` helper.

## Non-goals

- No gameplay/rebalance change; no Unity; no new save section.

## Owned paths

- `src/UI/StatusPanel.cs`, `src/UI/GameHudOverlay.cs`, `src/UI/SurvivorsPanel.cs`
- `src/Host/SurvivorsHostSession.cs`
- `src/Host/HostCli.Command.RunDay1PlayableSelfTest.cs` (bounded assertion)
- `scripts/ci/l10n_drift_gate.py` (surface registration)
- `assets/l10n/strings.csv`
- `Ashfall.Core.Tests/Survivors/NeedsDayDeltaTests.cs`
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs`
- the plan, `.ai/state.md`

## Acceptance

- Build 0/0; focused gates green; `l10n_drift_gate.py` PASS; `--ui-layout-selftest` and
  `--day1-selftest` PASS; `git diff --check` clean.
## Integration evidence (2026-10-01)

All ten delivered:

1. Threshold value templates localized (`ui.status.threshold.value.*`, 5 en+de rows).
2. `Signed` rounds first, so `-0` can no longer render.
3. `GameHudOverlay` + `StatusPanel` registered in `l10n_drift_gate.py` `LOCALIZED_SURFACES`.
4. `SurvivorsHostSession.NeedDaySpan(int)` added and used by `StatusPanel`.
5. `SurvivorsPanel.Bind(survivors, day)` threads the day; per-row deltas show `/{n}d`.
6. `--day1-selftest` now asserts the day-1 acute-rad trigger condition and at-most-once
   lesson queueing (both PASS).
7. Bounded StatusPanel section gate (`_thresholdData`/`_thermalData`/`_forecastData` +
   `RenderThresholds`/`RenderThermal`) plus `--ui-layout-selftest` fit.
8. HUD need-chip tooltips from the owning bands (`ui.hud.needs.tooltip.high|low`).
9. `TrackedKinds_CoverEveryRenderedNeed_AndRejectTheRest` covers all 9 `NeedKind`s.
10. Shared `NeedsProfile.IsWarmthCritical(float)`; both panels use it (no re-comparison).

**Verification:** host build 0/0; `NeedsDayDeltaTests` 23/23; `StatusPanelThresholdTests`
36/36; `OnboardingWiringGateTests` 5/5; `StringsCsvLocaleGateTests` 4/4;
`LocalizationRatchetTests` 2/2; `ArchitectureTestMapGateTests` 7/7; `l10n_drift_gate.py`
PASS (512 keys, 112 localized-surface references, German parity); `--day1-selftest` PASS;
`--ui-layout-selftest` Failures 0; architecture map regenerated (`--check` OK);
`git diff --check` clean. No commit; full suite not run.

## 5-loop find → repair → harden (2026-10-01)

- **Loop 1 (broad regression):** `Ashfall.Core.Tests/UI` 304/304, `Survivors` 600/600 — clean.
- **Loop 2 (static review):** found the two need-chip tooltip keys collapsing to
  identical copy (losing the high/low direction). Repaired to `Warn ≥` / `Warn ≤`
  (en+de) and **hardened** with a CSV-direction assertion in `Hud_NeedChips_CarryBandTooltips`.
- **Loop 3 (runtime):** `--day1-selftest`, `--ui-layout-selftest`,
  `--onboarding-journey-selftest` all PASS (one rebuild after a concurrent `src/UI`
  edit tripped the staleness guard — correct behaviour).
- **Loop 4 (generated contracts):** `docs/INDEX.md` drift → regenerated (`--check` OK);
  architecture map / catalog registry / core-systems / UI-panel all in sync.
- **Loop 5 (hardening):** `Signed` was concurrently extracted to
  `Ashfall.Core.Survivors.NeedsDayDeltaFormat` (now also guarding NaN/Infinity).
  Aligned the source gate to the Core helper and added a direct
  `Signed_RoundsAndRejectsNonFiniteDeltas` unit test (7 cases) so the `-0`/NaN leak
  cannot return.

**Final:** host build 0/0; `NeedsDayDeltaTests` 26/26; `StatusPanelThresholdTests`
53/53; `l10n_drift_gate.py` PASS (535 keys, 134 localized-surface references, German
parity); `--day1-selftest` PASS; `--ui-layout-selftest` Failures 0; `--onboarding-journey-selftest`
PASS; all generated contracts `--check` OK; `git diff --check` clean.
