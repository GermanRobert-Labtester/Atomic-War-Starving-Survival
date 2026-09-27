# Clutter Ledger Disposition — Enhancement Task 7 (2026-09-27)

> **STATUS: COMPLETE — no file deleted or moved. Every `POTENTIALCLUTTER.md` finding re-verified at HEAD and dispositioned; rows recorded in `KNOWN_DEBT.md`.**

## Method

For each finding: a working-tree reference search excluding the ledger itself
and `docs/INDEX.md`, a `git status --porcelain` check, and a tracked-ness check.
Dirty or lane-claimed paths were never touched.

## Result

The deletable and archivable set was already executed on 2026-09-12
(`DEBT-WORKTREE-DECLUTTER-2026-09-12`). No clean file still meets the
zero-reference bar, so this task only records decisions.

| Disposition | Findings |
|---|---|
| Archive already executed | A1, A2 (in `docs/archive/forensics/2026-09-12/`, now cited by governance) |
| Deletion already executed | A4, A5, A6, A7, A10, B4, C3, C5, F7, F8 |
| Premise stale: referenced or wired, keep | A3 (15 authority maps; indexed, test-cited), A8 (tracked baselines), B1/B2 (Bio Fermentation panel and host wired), B3 (4 of 5 partials), C7 (22 docs tracked and cited) |
| Resolved, no action | C1, C8, F3, F5, F6, F11; **F10 is a permanent prohibition** (`DEBT-GODOT-PARTIAL-REQUIRED`) |
| Hold: foreman decision, local-only, or lane-owned | A9 (`addons/ziva_agent/`, untracked), B3 `src/Main.Plans126_129.cs` (lane rename in flight), B5/F9 (`.qwen` mirror, untracked), C2 (38 current deletions are lane work), C4 (Unity-era `.uss`/`.uxml`/SDF font cluster), C6 (3 JSON outputs of 5 tracked `tools/` generators), F1 (`.agents`/`.kiro`/`.zcode` ignore policy), F2 (`snapshot-capture/` local duplicate), F4 (`.gitattributes` Unity template) |

`POTENTIALCLUTTER.md` stays at its path. The protocol archives it only once it
is emptied, and nine HOLD findings remain.

## New findings for the foreman

1. The blanket `/addons/` ignore rule also hides the **enabled** `godot_mcp`
   addon (`project.godot` enabled plugins), so a fresh clone lacks a live
   dependency. Recorded as `DEBT-ADDONS-GODOT-MCP-UNTRACKED`.
2. `Twin_ASHFall/`, named as the restore source by
   `DEBT-WORKTREE-DECLUTTER-2026-09-12`, is absent from this worktree. Confirm
   it exists in the twin clone before relying on a restore.
3. About 24 tracked `scripts/tools/` files cite pre-archive forensic paths.
   This is textual only.
4. `scripts/ci/legacy-reference-gate.sh` still allow-lists two deleted root
   scripts. Harmless.
