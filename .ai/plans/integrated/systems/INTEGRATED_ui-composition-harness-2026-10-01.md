# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

**Package:** `ui-composition-harness-2026-10-01`
**Date:** 2026-10-01

## 1. Goal & Outcome
- **Goal:** Fully integrate suggestion #1 — a headless Godot host that builds the
  real UI/composition root and drives the production `CampaignDayCoordinator`
  through a deterministic multi-day run, a mid-run fault injection + same-day
  retry, a persisted-envelope checksum comparison, and repeated soak replays.
- **Non-Goals:** No new gameplay authority, no second day coordinator, no new
  save store, no production behavior change.

## 2. Delivered
- **New:** `src/Main.UiTests.CompositionHarness.cs` —
  `RunUiCompositionHarnessAndQuit()` + `UiHarnessFaultOwner` + artifact writer.
- **Wiring:** `UiCompositionHarnessSelfTest` added to both `HostCliAction` enums,
  `HostCli.Parse`, `PrintHelp`, `Main.Application` dispatch, and the Core
  `HostCliRegistry` descriptor (`--ui-composition-harness-selftest`).
- **Observed result:** 120 owners composed; 92 restorable; 3/3 faults failed
  closed; 3/3 retries succeeded; canned-food `before=20 → after_fail=17 →
  after_retry=17` (exactly-once); envelope aggregate + 278 section checksums
  verified; disk == memory.
- **Follow-up logged:** `DEBT-COORDINATOR-RETRY-NONRESTORABLE-OWNERS` captures the
  28 non-restorable owners the harness measured.

## 3. Repairs made during the sweep
1. Renamed the harness partial to `Main.UiTests*.cs` so ExportRelease excludes it.
2. Removed a double `EmitSummary`/`QuitUiTestAfterFrame` on the early-return path.
3. Added a stable roster fixture (needs/health/radiation/stock) so an unattended
   full-composition run cannot starve and seal the slot as a terminal loss.
4. Surfaced `SaveLoadResult.Status`/`Details` when an envelope cannot be loaded.
5. Fixed a manifest timeout defect: the harness runs ~40–58s but the registry
   declared 30s. Added a per-descriptor `TimeoutSecondsFor` authority (120s for
   the composition harness) and a 120s generator budget; the manifest
   `--run` path now PASSes at 58.48s.

## 4. Verification
Host build 0/0; harness 23/23 PASS; `HostCliActionParityGateTests` 5/5;
`SelfTestManifestGateTests` 4/4; `ArchitectureTestMapGateTests` 6/6;
`generate-selftest-manifest.py --check` OK (318/316); CLI catalog `--check` OK
(359 entries / 593 tokens). No commit; full suite not run; foreign dirty worktree
preserved.
