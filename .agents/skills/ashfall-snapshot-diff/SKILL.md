---
name: ashfall-snapshot-diff
description: Re-renders ASHFALL's 69 golden UI snapshot panels and diffs them against snapshots/ to catch visual regressions before QA, using only headless Godot rendering and image comparison tooling.
---

# ASHFALL UI Snapshot Regression Checker

## ROLE

`snapshots/` contains 34 golden captures (`*_default.png`: caravan_barter, combat_hud, dose_ledger, duty_roster, …) with zero automation around them. You turn that folder into a real regression net: re-render each panel, diff pixel-by-pixel, and explain every difference.

## WORKFLOW

### PHASE 1 — Map Snapshot↔Panel
- Inventory `snapshots/*.png`; match each to its panel class in `src/UI/` (207 files) and the capture path used (selftest verb, UiPreview scene in `Builds/`, or Main.UiPanels wiring).
- List orphans: snapshots with no live panel (migration debris) and panels with no snapshot.

### PHASE 2 — Re-render
- Use the existing capture path (headless scene run or UiPreview) to regenerate each panel at 1920×1080, identical theme/fonts (`BarlowCondensed`/`ShareTechMono`).
- Deterministic state required: drive panels from mocks/fixtures, never live RNG or wall-clock data, or diffs are noise.

### PHASE 3 — Diff
- Compare regenerated vs golden: exact hash first; on mismatch run pixel diff (ImageMagick `compare` or PIL) and produce a diff image + per-region delta percentage.
- Classify each diff: `THEME_DRIFT` (font/theme change), `LAYOUT_SHIFT`, `CONTENT_CHANGE` (data-driven text), `NOISE` (anti-aliasing/rendering jitter), `REAL_REGRESSION`.

### PHASE 4 — Verdict
- `CONTENT_CHANGE`: validate the data change was intended; update golden only with approval.
- `REAL_REGRESSION`: file with repro (panel, state, seed) — hand to ashfall-repair, do not fix UI yourself.

## RULES
- Headless only; no editor.
- Golden updates are deliberate, recorded in the report, one commit per approved update.
- Never mass-regenerate goldens to "fix" failures.

## OUTPUT
`docs/ui/SNAPSHOT_DIFF_REPORT.md` — panel matrix, diff classifications, diff images path, orphan lists.

## QUALITY GATE
- Every snapshot has a verdict; zero unexplained diffs remain.
- Re-run is repeatable: same inputs ⇒ same diff result.
