# Pipeline Regression Fix (Phase 26 close)
During Phase 26 close, an on-disk byte-level integrity check revealed a regression
that affected every Phase 13–26 snapshot since the SubViewport pipeline was first
introduced at Phase 14. The orchestrator logged `[PASS]` for each capture, but
14 captures were in fact identical 4062B transparent PNGs (all-zero framebuffers).

## Symptom

12 Phase 13+ dashboards were returning the same MD5
`66562626834bd0ac0c6bf8fd74342ba9` and the same 4062B file. Pixel-byte inspection
showed 800×1280 RGBA buffers with `00 00 00 00` across every row.

13 Phase 12 + earlier snapshots rendered correctly (49KB–90KB) because
their layouts did fewer process-ticks to settle.

## Root cause

`SnapshotOrchestrator` was firing the SubViewport `Read()` at tick=2 after
`Mounted`. The new HYBRID shell panels (`AshfallDashboardShell` +
`AshfallSidebar` + `AshfallStatusRail` + 2–4 `AshfallDataGrid`s) nest deep
VBox / HBox layouts that need extra process ticks before their fixture grids
become visible. Reading the framebuffer before those ticks completed returned
an empty surface.

## Fix

`src/UI/SnapshotOrchestrator.cs`:

1. Bumped `Mounted → FramesWait` ticks from 4 → 8
2. Added full child traversal in `TraverseVisible()` (walks visible flag through every descendant)
3. Bumped `FramesWait → Reshow` ticks from 8 → 12
4. Bumped `Reshow → Read` ticks from 2 → 6
5. In `Reshow`: switched `RenderTargetUpdateMode` Always → Once + added explicit `RenderingServer.ForceDraw(false)`

## Verification

| | Before | After |
|---|---|---|
| Distinct MD5 fingerprints | 14 (12 dup) | 27 distinct |
| Duplicate MD5 groups | 1 (14 files) | 0 |
| Blank (zero non-zero pixels) snapshots | 14 | 0 |
| `dotnet build` | 0/0 | 0/0 |
| `dotnet test` | 1999/1999 | 1999/1999 |
| `--bridge-selftest` | PASS | PASS |
| `--data-integrity-selftest` | PASS | PASS |
| `--asset-registry-selftest` | 48/48 | 48/48 |
| `--ui-snapshot-uitest` | 27/27 (but 14 visually blank) | 27/27 (all visually distinct) |

All 27 snapshots are now byte-distinct and visually non-blank.
