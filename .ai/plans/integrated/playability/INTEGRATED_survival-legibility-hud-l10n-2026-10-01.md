# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility HUD Localization (10 small tasks) — P009–P016

STATUS: APPROVED BY USER

User directive (2026-10-01): "Continue with these small tasks … repeat for 5 loops …
suggest 15 very small tasks."

## Bounded outcome

Close the remaining localization + hardening gaps on the survival-legibility surfaces.

1. Localize `GameHudOverlay` day text (`ui.hud.day`).
2. Localize HUD health text (`ui.hud.health`).
3. Localize HUD radiation text (`ui.hud.radiation`).
4. Localize HUD value counter (`ui.hud.value`).
5. Localize static HUD labels (`ui.hud.label.hp|rad|value`).
6. Add a `ui.hud.*` key-presence gate.
7. Register `GameHudOverlay` in the l10n pilot literal-sweep.
8. Extend the shared-predicate gate to `StatusPanel`/`SurvivorDetailPanel`.
9. Add a `NeedsProfile` all-predicate boundary test.
10. Make `NeedsDayDeltaFormat.Signed` overflow-safe + test.
11. Register `scripts/ci/ui-layout-check.sh` in `CI_GATE_MANIFEST.json`.
12. Add a `NeedsDayDeltaTracker.Capture` idempotence test.

## Owned paths

`src/UI/GameHudOverlay.cs`, `Assets/Ashfall.Core/Survivors/NeedsDayDeltaFormat.cs`,
`scripts/ci/l10n_drift_gate.py`, `docs/ci/CI_GATE_MANIFEST.json`, `assets/l10n/strings.csv`,
`Ashfall.Core.Tests/Survivors/NeedsDayDeltaTests.cs`,
`Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs`, the plan, `.ai/state.md`.

## Acceptance

Build 0/0; focused gates green; `l10n_drift_gate.py` PASS; `--ui-layout-selftest` PASS;
`git diff --check` clean.

## Integration evidence (2026-10-01)

All twelve items delivered: HUD day/health/radiation/value formats + `HP`/`RAD` labels
localized (`ui.hud.*`, 6 en+de rows); `ui.hud.*` key-presence gate; `GameHudOverlay`
registered in the pilot literal sweep; predicate gate extended to `StatusPanel` +
`SurvivorDetailPanel`; `NeedsProfile` all-predicate boundary test; `NeedsDayDeltaFormat.Signed`
clamped overflow-safe (+ test); `ui_layout_selftest` registered in `CI_GATE_MANIFEST.json`
(66 gates); `Capture` idempotence test.

## 5-loop find → repair → harden

- **Loop 1:** UI 357/357, Survivors 606/606, Localization 25/25 (one transient
  duplicate-key read during a concurrent CSV write re-ran clean).
- **Loop 2 (found two real defects):** `NeedsDayDeltaFormat.Signed`'s doc claimed it
  rendered `+0` while the code returns `0`; and `ui.hud.needs.tooltip` was a dead key
  superseded by `.high`/`.low`. **Repaired** both; **hardened** by updating the pinned
  survival-legibility key gate to the live `.high`/`.low` form and adding the six new
  HUD keys to it.
- **Loop 3:** `--ui-layout-selftest`, `--day1-selftest`, `--onboarding-journey-selftest`
  all PASS.
- **Loop 4:** `docs/INDEX.md` drift → regenerated (`--check` OK); other contracts in sync.
- **Loop 5:** UI 357/357, Survivors 606/606, Localization 25/25; `git diff --check` clean.

**Final:** build 0/0; `StatusPanelThresholdTests` 90/90; `NeedsDayDeltaTests` 29/29;
`CiGateManifestDriftTests` 7/7; `StringsCsvLocaleGateTests` 4/4; `LocalizationRatchetTests`
2/2; `l10n_drift_gate` PASS; manifest 66 gates; `git diff --check` clean. No commit; full
suite not run; concurrent-session edits preserved.
