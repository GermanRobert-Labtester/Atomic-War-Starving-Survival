# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility Predicate Hardening — 5-loop find → repair → harden

STATUS: APPROVED BY USER

User directive (2026-10-01): "Continue with these small tasks, after please again do a
loop of finding issues, repairing issues, hardening the spot where issues found …
repeat for 5 loops and then suggest 15 very small tasks."

## Outcome

The 15 suggested tasks were already integrated by the concurrent session (its fifth
wave). This pass ran the requested 5-loop find → repair → harden over the merged state.

**Loop 2 found a real defect:** `NeedsProfile` exposes `IsHungerCritical` /
`IsThirstCritical` / `IsFatigueCritical` / `IsMoraleCritical` / `IsWarmthCritical`, but
`SurvivorsPanel` still compared the raw fields (`s.Hunger >= profile.hungerCritical`,
`s.Thirst >= profile.thirstCritical`) in the strain classification and the row status.
That is exactly the per-panel threshold drift the predicates exist to prevent; the
existing gate only covered warmth.

## Repair + hardening

- `src/UI/SurvivorsPanel.cs` — both sites now call `profile.IsHungerCritical(...)` /
  `profile.IsThirstCritical(...)` (warmth already used the predicate).
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs` — updated the now-stale
  `SurvivorsPanel_StrainThresholds_ReadTheProfile` to the predicate form, and added
  `Panels_UseProfileCriticalPredicates_NotRawComparisons`, which fails if any UI file
  reintroduces `>= profile.hungerCritical` / `>= profile.thirstCritical`.

## Verification

- Host build 0 warnings / 0 errors.
- `StatusPanelThresholdTests` 71/71; `UI` directory 338/338; `Survivors` 604/604.
- `Localization` 25/25; `Tooling` 150/150.
- `--day1-selftest` PASS; `--ui-layout-selftest` Failures 0.
- All generated contracts `--check` OK; `l10n_drift_gate.py` PASS (574 keys, German
  parity); `git diff --check` clean.
- No residual raw critical comparison remains anywhere under `src/`.

No commit; full suite not run; concurrent-session edits preserved.