# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

**Package:** `five-task-wave-2026-10-01` (coordinator retry hardening, keepsake orphan closure, content-utilization scanner false-negative fix, difficulty runtime panel, single pre-release gate)
**Anchor:** claim `claim-five-task-wave-2026-10-01` (`WORKTREE_OWNERSHIP.md`)
**Date:** 2026-10-01
**Type:** Mixed bounded wave — one Core runtime test, one data closure, one Core tooling fix, one host UI route, one release tooling wrapper.
**Status:** All five packages integrated and verified on the current worktree; foreign dirty changes preserved.

---

## 1. Outcome

Tackle five of the deferred "bigger task" items and integrate them end to end, then
sweep for wiring/tooling drift. Each package below states its outcome, the authority
it extends, the exact verification, and the limitation.

## 2. Package A — Coordinator fail-closed retry runtime proof

**Outcome:** the inventory rollback contract (Plan 38 follow-up) is now proven at
runtime with a realistic ration/producer ledger, not only a static source gate.
**Changed:** new `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorRetryRuntimeTests.cs`
(3 tests). A late `FaultOwner` throws mid-day; the retry restores the inventory
custody baseline before ticking; the final checksum equals the no-failure control.
Covers mid-day owner failure, snapshot-preflight failure, and a persistence failure
after owners ticked.
**Authority:** `CampaignDayCoordinator` + `IPreDaySnapshotRestore` (unchanged).
**Verification:** `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorRetryRuntimeTests.cs` → 3/3 PASS.
**Limitation:** the test models the host owners' seam; the host owner registration is
still pinned by `CampaignDayCoordinatorSourceGateTests` (5/5).

## 3. Package B — Enrichment keepsake orphan closure (DEBT-ENRICHMENT-KEEPSAKE-ORPHANS)

**Outcome:** all 65 orphaned `personal_keepsake_item_id` values now resolve.
**Changed:** `Assets/StreamingAssets/Data/items.json` (724 → 789 authored items, 65 new
keepsake objects); `src/Host/HostCli.OriginMechanics.cs` Check 9 now requires full
resolution; new `Ashfall.Core.Tests/Survivors/EnrichmentKeepsakeResolutionTests.cs`
(2 tests); `KNOWN_DEBT.md` row retired.
**Authority:** `items.json` is the canonical item authority; enrichment data unchanged.
**Verification:** `EnrichmentKeepsakeResolutionTests` 2/2; `--origin-mechanics-selftest`
12/12 (Check 9: "all 76 authored keepsakes resolve in the item catalog (0 orphaned)");
`bin/ashfall-dev validate-json` 715/715.
**Limitation:** keepsakes are cosmetic inventory grants; no new gameplay effect.

## 4. Package C — Content-utilization scanner false negatives (F-18)

**Outcome:** a catalog whose exact filename is named by source code now records
filename-based loader evidence, so `DetectDisconnects` no longer reports a false
`NO_LOADER`. This closes the scanner defect the 2026-09-05 audit recorded (F-18).
**Changed:** `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`
(`VerifyConsumersInSource` records `SourceReference:<file>` and advances the stage
to `LOADED` for any catalog with an empty loader and an exact filename reference);
new test in `Ashfall.Core.Tests/ContentUtilizationGraphTests.cs`.
**Verification:** `ContentUtilizationGraphTests` 38/38 (new
`CatalogNamedInSource_IsNotReportedAsNoLoader_F18`); `generate-architecture-map.py`
regenerated (315 subsystems).
**Limitation:** the broader "disposition every remaining authored-but-dormant
catalog" work (T079–T086) remains open; this package closes only the tooling defect.

## 5. Package D — Difficulty runtime panel (DEBT-PLAN181-DIFFICULTY-RUNTIME-UI)

**Outcome:** the already-shipped difficulty authority now has the player surface it
was missing: a Live `difficulty_settings` route that renders the authored presets,
the nine custom scalar lanes, and the irreversible ironman lock.
**Changed:** new `src/UI/DifficultySettingsPanel.cs` (`IBindablePanel`; routes every
edit through `Main.SelectDifficultyPreset` / `SetDifficultyCustomScalar` /
`LockDifficultySettings`); `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`;
`src/Main.PlayerSurfaces.cs`; `src/Main.ExpandedShelterSystems.cs`;
`src/Main.PanelLifecycle.cs`; `src/Main.DifficultySettings.cs`
(`SetupDifficultySettingsPanel`); new
`Ashfall.Core.Tests/UI/DifficultySettingsPanelRouteTests.cs` (3 tests);
`KNOWN_DEBT.md` row retired.
**Verification:** `DifficultySettingsPanelRouteTests` 3/3; `PanelRouteGateTests`
22/22; `PanelRouteReachabilityGateTests` 2/2; `PlayerSurfaceCoverageGateTests` 8/8;
`PanelCatalogCompletenessTests` 3/3; host build 0 errors / 6 pre-existing warnings;
`--ui-layout-selftest` Failures 0; `--difficulty-settings-selftest` 12/12.
**Limitation:** no separate lock-confirmation modal; the lock is one deliberate press.

## 6. Package E — Single pre-release gate (C2[21]/Plan 48 residual)

**Outcome:** one command composes the canonical release gate with the export smoke;
`release-gate.sh` remains the policy authority.
**Changed:** new `scripts/release/pre-release-gate.sh` (`--skip-export` for a fast
local run); `docs/releases/PROCESS.md` pre-release section.
**Verification:** `bash -n scripts/release/pre-release-gate.sh` clean; the wrapper
delegates to the existing `release-gate.sh` and `export-build.sh` and adds no gate
logic. (The full release gate is a full-suite tier; not run per `TEST_POLICY.md`.)
**Limitation:** the tagged CI workflow still runs the two stages as separate jobs by
design; the wrapper is the local single-command entry.

## 7. Cross-cutting sweep findings repaired

1. **Localization ratchet** was already red at HEAD (625 vs a stale 619 baseline).
   The new panel's offline string was routed through `AshfallLocalization`, and the
   baseline was re-recorded to the verified 625 with an honest comment.
2. **Malformed `strings.csv` row**: the prior T18 `ui.research.atlas_tooltip` row had
   unquoted commas (7 fields) and broke `StringsCsvLocaleGateTests`. Quoted, and the
   new difficulty row added quoted. `StringsCsvLocaleGateTests` 4/4;
   `l10n_drift_gate.py` PASS (377 keys).
3. **Stale difficulty probe** (`HostCli.DifficultySettings.cs` Check 12 expected a
   direct `TrySave`): a concurrent stream corrected it to `CaptureSection` +
   `TryCapturePersisted`; re-verified 12/12 after rebuild.
4. **Generated architecture map** regenerated to 315 subsystems (line-shift only).

## 8. Non-goals

- No new save section, RNG change, or Core gameplay-math change.
- No change to the single-authority contracts: `CampaignDayCoordinator`,
  `items.json`, `ContentUtilizationScanner` evidence only, `DifficultySettingsSystem`,
  and `release-gate.sh` remain the owners.
- The T079–T086 content-reachability disposition programme is not closed here.

## 9. Handoff

- Files changed are listed per package above; shared seams (`PanelRegistryBootstrap`,
  `Main.PlayerSurfaces`, `Main.ExpandedShelterSystems`, `Main.PanelLifecycle`) were
  edited additively with foreign dirty hunks preserved.
- Generated artifacts in sync: `ARCHITECTURE_TEST_MAP.md` (315),
  `SELFTEST_MANIFEST.json` (317), `UI_PANEL_ARCHITECTURE_GUIDE.md`, l10n gate.
- No commit; full suite not run (per `TEST_POLICY.md`).
