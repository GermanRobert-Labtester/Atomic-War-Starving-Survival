# D3 — Shutdown Cleanliness — Premise Evidence

**Task:** Wave 8 Part 2, TASK D3 (`Seal-steps/847219_ASHFALL_WAVE8_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md`)
**Status:** CLASSIFIED / DOCUMENTED (benign harness signature)
**Date:** 2026-09-17
**Base commit:** `033df2b7` + prior D1/C2/D2 work
**Process authority read:** `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md` (full)

## 1. Reproduction (Phase 0)

Command: `godot --headless --path . -- --ui-accessibility-selftest` (exit 0 / PASS).

Shutdown tail:
```
WARNING: 2 RIDs of type "CanvasItem" were leaked.
   at: _free_rids (servers/rendering/renderer_canvas_cull.cpp:2735)
ERROR: 1 RID allocations of type '...FontAdvanced...' were leaked at exit.
WARNING: 8 ObjectDB instances were leaked at exit
ERROR: 1 resources still in use at exit
```

`--verbose` object set: 2 parentless `Label`, 2 base `Object`, `StyleBoxFlat`,
`StyleBoxEmpty`, `FontFile`, `Image`.

## 2. Cross-surface comparison (Phase 0.3)

| Selftest | Shutdown leak lines |
|---|---|
| `--ui-accessibility-selftest` | 8 ObjectDB / 2 CanvasItem / 1 font RID / 1 resource |
| `--panel-bind-lifecycle-selftest` | **0** |
| `--player-panels-uitest` | 269 ObjectDB / 103 CanvasItem (separate, larger diagnostic class) |

## 3. Classification (Phase 1)

- **Not a monotonic node leak:** three consecutive a11y runs each leak exactly
  8 objects (flat, fixed).
- **Not a signal/subscription leak:** the production lifecycle path
  (`--panel-bind-lifecycle-selftest`) is clean across bind/unbind/close cycles.
- **Class:** diagnostic-harness specimen lifetime. `UiAccessibilitySelfTest`
  instantiates a fixed panel list + a briefing modal **without attaching them to
  the SceneTree** for static inspection, then frees them. Default-theme
  style/font resources created for those standalone specimens are retained by
  two parentless labels and released only at process teardown. No production
  panel or resource owner leaks.

## 4. Authority boundaries

- UI panel/surface owners: unchanged (no production lifetime defect found).
- Selftest harness owns the specimen lifetime.
- No global forced-free cleanup at process exit (forbidden — masks defects).
- No rendering policy change.
