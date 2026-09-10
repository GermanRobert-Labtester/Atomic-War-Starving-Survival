# ASHFALL — Unity→Godot Asset Migration Ledger (Plans 48+)

**Direction:** Unity-era `Assets/` tree → Godot-native `assets/` tree, always.
Copy-first: originals are retained (moved to `_migrated/` or held in place);
deletion belongs to the later repo-hygiene wave only.

**Batch manifests:** `scripts/tools/asset-migration-batch-*.json`
**Tool:** `scripts/tools/migrate-assets-batch.py` (dry-run, case-collision
detection, never deletes).

## Legacy backlog position

| Legacy tree | Files at Plan 48 start | Status |
|---|---|---|
| `Assets/art` | 0 (`.gdignore` only) | fully migrated in earlier waves |
| `Assets/ui`   | 0 (`.gdignore` only) | fully migrated in earlier waves |
| `Assets/sprites` | 0 (`.gdignore` only) | fully migrated in earlier waves |
| `Assets/audio` | 0 (`.gdignore` only) | fully migrated in earlier waves |
| Repo-root strays | 3 | batch 01 (below) |

Zero live `res://Assets/art|ui|sprites|audio` references remain in
`src/`, `scenes/` or `project.godot` (verified by sweep at Plan 48).

## Batch 01 — Plan 48

| Source | Destination | Consumers | Import status | LFS | Visual QA | Legacy hold |
|---|---|---|---|---|---|---|
| `UI_StyleReference_01.jpg` | `assets/ui/reference/ui_style_reference_01.jpg` | none (design reference material, not runtime-loaded) | default Godot texture import (reference photo; no runtime filter requirements) | tracked (`git lfs ls-files` shows pointer at new path) | n/a — not rendered by any scene | `_migrated/UI_StyleReference_01.jpg` (retained, git-tracked) |
| `codex_alt_01_rgb.png.import` | — | none | orphan `.import` stub; target PNG absent from repo | n/a | n/a | ORPHAN_HOLD in place; deletion deferred to hygiene wave |
| `codex_alt_02_rgb.png.import` | — | none | orphan `.import` stub; target PNG absent from repo | n/a | n/a | ORPHAN_HOLD in place; deletion deferred to hygiene wave |

Machine-readable results: `artifacts/asset-migration-batch-01-report.json`.

## Verification at batch close

- manifest dry-run PASS; rerun idempotent (second run reports COPIED entry as
  ALREADY_MIGRATED / hold states unchanged);
- `scripts/ci/case-collision-gate.sh` PASS — 15119 tracked paths, no
  case-folded collisions;
- `core.ignorecase=false` pinned (repo setup contract);
- LFS: migrated binary is a clean LFS pointer; no >5 MB binary escapes LFS;
- `./scripts/ci/godot-asset-gate.sh` — see Plan 48 closeout report for the
  recorded run of this batch.
