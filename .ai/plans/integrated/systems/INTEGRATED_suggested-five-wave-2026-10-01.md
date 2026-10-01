# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

**Package:** `suggested-five-wave-2026-10-01` (content-reachability dispositions, host coordinator retry probe, difficulty lock/persistence, release-craft rehearsal, localization ratchet burn-down)
**Anchor:** claim `claim-suggested-five-wave-2026-10-01` (`WORKTREE_OWNERSHIP.md`)
**Date:** 2026-10-01
**Status:** All five integrated and verified; foreign dirty worktree preserved.

---

## 1. Outcome

Complete the two limitations left by the first five-task wave and execute the five
suggested follow-on tasks, each with a named authority, focused verification, and an
explicit remaining-work note.

## 2. T079–T086 — Content-reachability dispositions

**Outcome:** every `UNRESOLVED` catalog now carries a reviewed, owned disposition;
an undispositioned unresolved catalog fails the runtime gate.
**Changed:** `docs/ci/content_reachability_dispositions.json` (188 entries: 51 WIRED,
27 CODEX_ONLY, 110 DORMANT, each with owner/classification/rationale/ticket/expiry);
`src/Host/ContentUtilizationSelfTest.cs` Phase 9 loads the policy and fails with
`UNDISPOSITIONED:` for any uncovered unresolved catalog; new
`Ashfall.Core.Tests/Content/ContentReachabilityDispositionTests.cs` (4 tests).
**Verification:** `--content-utilization-selftest` → "Disposition policy: 188
entries", "Unresolved without a disposition: 0", invalid 0, stale 0; disposition
tests 4/4; `ContentUtilizationGraphTests` 38/38.
**Remaining:** the 51 `WIRED` entries are scanner-evidence gaps, not new wiring;
each should be promoted to GAMEPLAY_CONSUMED once a typed loader trace is added.

## 3. Host-level coordinator retry-injection probe

**Outcome:** the real production `inventory_custody` + `starting_level_rations`
owners are proven to roll back on a fail-closed retry.
**Changed:** new `src/Main.CoordinatorRetryProbe.cs`
(`RunCoordinatorRetryProductionProbe`: fresh campaign, seeded food surplus, injected
late fault, same-day retry, exactly-one-delta assertions);
`src/Host/HostCli.WorldPlaytest.cs` calls it as a world-playtest check.
**Verification:** `--world-playtest-selftest` → "coordinator retry: rations consumed
once (before=16, after_fail=13, after_retry=13, delta=3)" + "production day-owner
retry does not double-consume rations" PASS.
**Remaining:** the probe isolates the two inventory owners; a full 60-owner retry
with a persisted-envelope checksum comparison is a stronger follow-on.

## 4. Difficulty lock confirmation + persistence surfacing

**Outcome:** the irreversible ironman lock requires a second deliberate press, and
the panel now states whether the checksummed `difficulty_settings` section is saved
and exposes an explicit save.
**Changed:** `src/UI/DifficultySettingsPanel.cs` (`_lockConfirmPending`,
`isDirty`/`save` delegates, `SAVE SETTINGS` + `UNSAVED CHANGES` row);
`src/Main.DifficultySettings.cs` passes `() => _difficultySettingsDirty` and
`SaveDifficultySettings`; `DifficultySettingsPanelRouteTests` pins all three.
**Verification:** route tests 3/3; build 0 errors; `--difficulty-settings-selftest`
12/12; `--ui-layout-selftest` Failures 0.
**Remaining:** no modal backdrop for the confirmation; the inline two-press is the
deliberate affordance.

## 5. Release-craft end-to-end rehearsal

**Outcome:** the hotfix iron rule is now rehearsed (positive + negative) and the
live curated codecs are pinned to the immutable v1.1.0 snapshot.
**Changed:** `scripts/ci/version-gate.py --self-test` gains the non-mutating
"Hotfix iron-rule rehearsal" (synthetic `SchemaVersion` bump must be flagged; a
docs-only change must be clean); new
`Ashfall.Core.Tests/Save/HotfixRehearsalGateTests.cs` (snapshot ↔ live codec
equality + rehearsal source pin); `docs/releases/HOTFIX.md` "Dry-Run Rehearsal".
**Verification:** `version-gate.py --self-test` PASS; `HotfixRehearsalGateTests` 2/2.
**Remaining:** a real throwaway-branch hotfix on a fixture tree (a `git worktree`
rehearsal) is still not automated; the current rehearsal is in-memory.

## 6. Localization ratchet burn-down

**Outcome:** the top-offending panel is localized and the ratchet is lowered.
**Changed:** `src/UI/WorkshopPanel.cs` (17 literals routed through a `T(key,fallback)`
shim); `assets/l10n/strings.csv` (+14 workshop keys, EN+DE);
`LocalizationRatchetTests` baseline 625 → 608.
**Verification:** `LocalizationRatchetTests` 2/2; `StringsCsvLocaleGateTests` 4/4;
`LocalizationPilotTests` 4/4; `l10n_drift_gate` PASS (391 keys); `--workshop-relic-uitest`
PASS; build 0 errors.
**Remaining:** `VisitorIntegrationPanel`, `ShelterHudPanel`, `DutyRosterPanel`
(13 each) and `SilentFoundryPanel` (12) are the next burn-down targets.

## 7. Non-goals

No save-section, RNG, or Core gameplay-math change. No new authority: the
disposition policy extends `ContentExemption`; the coordinator remains the day
authority; difficulty remains `DifficultySettingsSystem`; the release gate remains
`release-gate.sh`.

## 8. Verification summary

Host build 0 errors / 6 pre-existing warnings; all new focused tests pass;
`--world-playtest-selftest` PASS; `--content-utilization-selftest` disposition gate
0 undispositioned; `--difficulty-settings-selftest` 12/12; `--workshop-relic-uitest`
PASS; architecture map regenerated (315) and `--check` clean; l10n gates green.
No commit; full suite not run (per `TEST_POLICY.md`).
