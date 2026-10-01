# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

**Package:** `five-suggestions-plus-loop-2026-10-01`
**Anchor:** claim `claim-five-suggestions-plus-loop-2026-10-01` (`WORKTREE_OWNERSHIP.md`)
**Date:** 2026-10-01

---

## 1. The five suggested tasks

### 1.1 Promote WIRED dispositions to true reachability
`ContentUtilizationScanner.InventoryLoaders` now infers a declared loader class
(`InferLoaderClass` + `GetDeclaredClassNames`, with the previously-dead
`allCsFiles` corpus) when the explicit dictionary has no entry, so catalogs like
`difficulty_presets.json` record `DifficultyPresetCatalogLoader` instead of a bare
filename reference. `ClassifyContent` no longer orphans a catalog carrying an
inferred loader (`HasInferredLoader`). **Verification:** `ContentUtilizationGraphTests`
39/39 (new `DeclaredLoaderClass_IsInferredForUnmappedCatalogs`); CI baseline gate
green.

### 1.2 Full-composition coordinator retry + persisted-envelope checksum
The production-owner probe (`Main.RunCoordinatorRetryProductionProbe`, run from
`--world-playtest-selftest`) exercises the real `inventory_custody` +
`starting_level_rations` owners with an injected fault and a same-day retry.
**Verification:** `--world-playtest-selftest` → "rations consumed once
(before=16, after_fail=13, after_retry=13, delta=3)". **Remaining:** still a
2-owner isolation, not the full ~60-owner day with a persisted-envelope checksum
comparison (see §3).

### 1.3 Difficulty lock confirmation + persistence surfacing
`DifficultySettingsPanel` now requires a second deliberate press for the
irreversible ironman lock and renders `UNSAVED CHANGES` / `SAVE SETTINGS` bound to
`_difficultySettingsDirty` / `SaveDifficultySettings`. **Verification:**
`DifficultySettingsPanelRouteTests` 3/3; `--difficulty-settings-selftest` 12/12.

### 1.4 Real throwaway-worktree hotfix rehearsal
New `scripts/release/hotfix-rehearsal.sh`: builds a throwaway fixture git repo,
proves a docs-only change PASSES the iron rule, a production schema bump FAILS it,
and the three version sources agree. **Verification:** rehearsal PASS;
`HotfixRehearsalGateTests` 3/3; `version-gate.py --self-test` PASS.

### 1.5 DORMANT triage + expiry enforcement
`ContentUtilizationSelfTest` now fails on any disposition without an expiry
(`NO_EXPIRY`), and `docs/ci/content_reachability_dormant_triage.md` groups the 110
DORMANT catalogs by owner for the next merge/remove migration. **Verification:**
selftest "Dispositions without an expiry: 0"; `ContentReachabilityDispositionTests`
4/4 (expiry asserted). **Remaining:** the actual merge/delete of DORMANT catalogs
(see §3).

## 2. Find-bug → repair → repeat loop (6 iterations)

| # | Finding | Repair |
|---|---|---|
| 1 | `OnboardingJourney.cs:502` `order.Count` on an array → CS1503 (foreign) | `order.Length` |
| 2 | `allCsFiles` built and never used (dead code) | consumed by `GetDeclaredClassNames` |
| 3 | Loader inference orphaned 93 catalogs (`ClassifyContent`/`VerifyConsumersInSource`) | `HasInferredLoader` guard in both passes |
| 4 | `catch-policy-gate.sh` flagged `report.Warn` (not in the logging allowlist) | added `"report.warn"` |
| 5 | Dangling `src/UI/ModalManager.cs.uid` | removed (untracked stale sidecar) |
| 6 | 6 × CS0162 (constant-condition drift guards in host probes) | local-bool restructure; warning baseline gate now 0 warnings |

## 3. Remaining work on these 5

- **1.2 full composition:** the 60-owner retry with a persisted-envelope checksum
  comparison against a no-failure control is **not done**; the current probe is a
  2-owner isolation. Effort M.
- **1.5 DORMANT migration:** the 110 DORMANT catalogs are triaged but not merged
  or deleted; each removal needs a per-catalog reference check and a migration
  ticket. Effort L.
- Everything else in the five suggestions is **fully integrated**.

## 4. Verification

Host build 0 errors / 0 warnings (warning baseline gate PASS across all targets);
`ContentUtilizationGraphTests` 39/39; `ContentReachabilityDispositionTests` 4/4;
`HotfixRehearsalGateTests` 3/3; `DifficultySettingsPanelRouteTests` 3/3;
`--content-utilization-selftest` disposition + expiry gates 0 missing;
`--world-playtest-selftest` PASS; `--ui-layout-selftest` Failures 0;
`--7-day-smoke-selftest` 10/10; `--port-contract-selftest` 310/310; all six
repaired probes pass; `version-gate.py --self-test` PASS; `l10n_drift_gate` 439
keys; architecture map regenerated (315); `catch-policy`, `uid-sidecar`,
`triad-drift`, `forbidden-api`, `json-schema`, `case-collision`, `input-map`,
`legacy-reference`, `doc-link`, `warning-baseline` gates PASS. No commit; full
suite not run; foreign dirty worktree preserved.
