# BUG-GRID-LIFECYCLE implementation log

## Phase 1 — reproduction

Pre-integration checkpoint PASS: DataGrid has no active competing claim; earlier
PanelBindLifecycleSelfTest claims are DONE. Existing test edits preserved.
Added Gate 18 and ran the bounded headless panel-bind lifecycle selftest.
Gates 1–17 passed; Gate 18 failed with `emptyRestored=True, leakedPlaceholder=True`.
Evidence: /tmp/ashfall-trade-repair.AqoLAz/grid-before.log.

The holdfast diagnostic independently identified detached `— no entries —`
labels among its 100 orphans. Read native IDs through untyped Godot Variants to
avoid truncating 64-bit object IDs in the SDK's Array<int> return type.

## Phase 2 — ownership repair

Pre-integration checkpoint PASS: permanent parenting plus Visible state requires
no host ownership change, schema, RNG, or gameplay event change. The grid body
retains its reusable label, frees only generated rows, and hides the placeholder
while populated. Regression additionally checks hidden/restored display state.
Host build 0 warnings/errors; original Gate 18 now passes. Final panel lifecycle
21/21 and holdfast runtime PASS with no engine shutdown warnings/errors. The
intermediate holdfast orphan count fell from 100 to 63 after this change alone.
Status: RESOLVED. Exact commands/evidence in the holdfast implementation log.
