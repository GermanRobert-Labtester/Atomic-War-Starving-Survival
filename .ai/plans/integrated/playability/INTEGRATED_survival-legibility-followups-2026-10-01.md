# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility Follow-ups (10 small tasks) — P009–P012 hardening

STATUS: APPROVED BY USER

User directive (2026-10-01): "Allright lets go with those 10 tasks and then suggest
10 small tasks more!" — i.e. implement the ten follow-ups listed at the bottom of
`.ai/plans/integrated/playability/INTEGRATED_survival-legibility-p009-p012-2026-10-01.md`.

## Bounded outcome

Ten small, single-owner presentation/verification tasks that complete the P009–P012
survival-legibility seam. No new authority, no new save section, no game-rule change.

1. HUD **WARM** chip — `HoldfastRuntimeSession` gains a `Warmth` projection; the HUD
   glance row gains a warmth chip reading `NeedsProfile.warmthWarn/warmthCritical`.
2. Localize `StatusPanel` **LETHAL THRESHOLDS** labels (`ui.status.threshold.*`).
3. Localize the **drift rows** + `/day` suffix (`ui.status.drift.*`).
4. `--survival-legibility-selftest`-equivalent bounded gate: assert the acute-rad
   lesson is requested exactly once and the day-1 acute-rad path is reachable through
   the Core journey (`OnboardingJourney`), replacing the source-only gate.
5. `NeedsProfile` **warn<critical invariant** test over every authored pair.
6. Bounded **layout/source gate** for the new LETHAL THRESHOLDS + drift sections
   (row-count + section-built), instead of a renderer-dependent PNG golden.
7. **WARM delta** in `SurvivorsPanel` (MOR was added; warmth remains unshown).
8. **Truthful no-baseline state**: show "trend available after first day" rather than
   silently omitting drift rows on a fresh load.
9. **Clamp multi-day deltas** in the delta display so a restore/offline gap cannot
   render an implausible `/d` value.
10. **Transient-only invariant** test for `NeedsDayDeltaTracker` (never persisted).

## Non-goals

- No new gameplay thresholds, no rebalance, no Unity.
- No new save section; the tracker stays transient.
- No change to the event-driven HUD contract.

## Owned paths

- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` (warmthWarn field)
- `Assets/Ashfall.Core/Survivors/NeedsDayDeltaTracker.cs` (clamp/annotation helper)
- `src/Host/HoldfastRuntimeSession.cs` (Warmth projection)
- `src/UI/GameHudOverlay.cs` (WARM chip)
- `src/UI/StatusPanel.cs` (localized threshold + drift rows, no-baseline note)
- `src/UI/SurvivorsPanel.cs` (WARM delta)
- `assets/l10n/strings.csv` (new `ui.status.*` rows, en+de)
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs` (invariant + section gates)
- `Ashfall.Core.Tests/Survivors/NeedsDayDeltaTests.cs` (transient-only + clamp)
- the plan, `.ai/state.md`

## Acceptance

- Build 0 errors / 0 warnings; focused gates green; `git diff --check` clean.
- Every new user-facing label resolves through `AshfallLocalization.Tr` with an
  authored en+de row.
- The HUD/panel thresholds all read `NeedsProfile`/`RadiationSystem`, never re-typed.
- `NeedsDayDeltaTracker` remains transient (no save-state reference).
## Integration evidence (2026-10-01)

All ten items delivered (items 4 and 6 as bounded, CI-safe equivalents):

1. HUD **WARM** chip; `HoldfastRuntimeSession.Warmth` projection (+ fallback drift);
   `NeedsProfile.warmthWarn = 40`.
2. LETHAL THRESHOLDS labels localized (`ui.status.threshold.*`).
3. Drift labels + `/day` / `/{n}d` suffix localized (`ui.status.drift.*`).
4. Acute-rad lesson bounded gate: Core dedup test (`RequestContextualTutorial` once)
   + existing production source gate (replaces a new HostCli action).
5. `NeedsProfile` warn<critical invariant over every authored pair.
6. Bounded section/localization gate + `--ui-layout-selftest` fit PASS (no PNG golden).
7. WARM day-over-day delta in `SurvivorsPanel`.
8. Truthful no-baseline note ("trend available after the first day advance").
9. Delta clamped to 0..100 and multi-day span annotated.
10. Transient-only invariant test: no `*Save*` Core file references the tracker.

**Verification:** host build 0/0; `NeedsDayDeltaTests` 12/12; `StatusPanelThresholdTests`
15/15; `OnboardingWiringGateTests` 5/5; `LocalizationRatchetTests` 2/2;
`StringsCsvLocaleGateTests` 4/4; `ArchitectureTestMapGateTests` 7/7;
`l10n_drift_gate.py` PASS (488 keys, German parity); `--ui-layout-selftest` Failures 0;
`--day1-selftest` PASS; architecture map regenerated (`--check` OK); `git diff --check` clean.
No commit; full suite not run.
