# BUG-GRID-LIFECYCLE repair plan

## Bug, reproduction, and root cause

The holdfast runtime test passes but reports 100 orphan CanvasItems at shutdown.
Source inspection finds that AshfallDataGrid.Rebuild detaches its persistent
empty-state label when populated. A managed field does not own the Godot native
node: freeing the grid cannot free the detached label. Other leak sources remain
unproven; do not claim this explains all shutdown diagnostics.

Add Gate 18 to the existing panel-bind lifecycle selftest. Retain the visible
placeholder through empty → populated → empty → populated, then free the grid
and require that its placeholder has also been freed. Run before production edit.

## Blast radius and invariants

All grid consumers share this presentation primitive. Empty state, row order,
selection callbacks, and sizing must remain unchanged. No Core, data, saves,
deterministic RNG, or gameplay event changes. Preserve pre-existing host-test edits.

## Options and selected repair

Keep the placeholder parented to the body and hide it when populated (selected).
Godot containers ignore hidden controls for layout; parent ownership covers both
in-tree and never-attached widgets. Clear only generated rows during rebuild.
Alternatively recreate/free the placeholder on every rebuild: more allocation and
unnecessary churn. Exit-tree cleanup is rejected because re-entry remains valid
and never-attached grids would still leak. No global orphan cleanup.

## File impact and phases

Exact production/test paths: src/UI/AshfallDataGrid.cs and
src/Host/PanelBindLifecycleSelfTest.cs. Documentation and claims listed in ledger.
First reproduce with Gate 18; then recheck source, apply the ownership fix, rebuild
and run the lifecycle gate and isolated holdfast runtime UI test at 15 FPS.

## Rollback and done

Revert only this session's hunks, preserving unrelated dirty worktree changes.
Done when the regression is red before/green after, existing lifecycle gates pass,
host build has no warnings/errors, and the holdfast path is checked for regressions.
Record remaining unrelated diagnostics honestly.
