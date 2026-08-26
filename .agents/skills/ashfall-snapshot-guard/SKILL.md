---
name: ashfall-snapshot-guard
description: Captures new panel snapshots, diffs against previous, and manages approval workflow. For when the AI already knows the golden images.
---

# ASHFALL UI Snapshot Guard

## ROLE

You eliminate the repetitive snapshot management overhead. The AI already knows the golden images — you just capture, diff, and approve.

## SCOPE

- **Input**: Panel name (e.g., `MedicalTriagePanel`), UiPreview scene path
- **Output**: Golden snapshot in `snapshots/`, diff report, approval workflow
- **Constraints**: `godot --headless` only; never Unity

## WORKFLOW

### PHASE 1 — Capture
- Render the panel headless at 1920×1080 (BarlowCondensed/ShareTechMono)
- Save as `snapshots/<panel>_default.png`

### PHASE 2 — Diff
- Compare against previous golden (pixel diff with tolerance)
- Classify diff: `THEME_DRIFT`, `LAYOUT_SHIFT`, `CONTENT_CHANGE`, `NOISE`, `REAL_REGRESSION`

### PHASE 3 — Approval
- `CONTENT_CHANGE`: validate the data change was intended; update golden only with approval
- `REAL_REGRESSION`: file with repro (panel, state, seed) — hand to `ashfall-repair`

### PHASE 4 — Verify
- `godot --headless --path . -- --data-integrity-selftest` (0 errors)
- `./scripts/ci/godot-asset-gate.sh` (asset registry green)

## CONSTRAINTS
- Never mass-regenerate goldens to "fix" failures
- Always use deterministic state (mocks/fixtures, no RNG)
- Golden updates are deliberate, one commit per approved update

## OUTPUT
`docs/ui/SNAPSHOT_GUARD_REPORT_<panel>.md` — diff classification, diff image path, approval status

## QUALITY GATE
- Every snapshot has a verdict
- Diff classification matches visual evidence
- Golden update approved or rejected with rationale
