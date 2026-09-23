# BUG-PANEL-ORPHANS repair plan

## Bug, proof, and cause

The holdfast shutdown diagnostic identifies five orphan headings: DIVE DETAIL,
MUSTER DETAIL, QUEST DETAIL, SITE DETAIL, and EVENT LOG. Each respective panel's
_Ready allocates an unparented _detailTitle immediately before RefreshView, which
calls RefreshDetail and overwrites the field with a fresh parented heading.

## Scope, invariants, and minimal repair

Remove only the five throwaway allocations. Existing initial and refreshed child
headings remain intact. No layout, labels, callbacks, host, data, saves, or RNG
changes. Parenting then immediately freeing these allocations would add needless
work; global orphan cleanup would mask production ownership defects.

Claim exact five panels and the existing panel lifecycle selftest. Add Gate 19
with independent create/Ready/Free probes for each panel and an orphan-ID baseline.
Require zero newly orphaned nodes (debug-only native API). Run red first, remove
allocations, rerun the focused selftest and isolated holdfast runtime at 15 FPS.

## Rollback and acceptance

Revert only this session's hunks. Done means each panel's probe fails before and
passes after, earlier lifecycle gates pass, the host builds cleanly, and holdfast
behavior remains passing. Other orphan roots are tracked independently.

## Follow-up: two throwaway containers

Per-panel Ready diagnostics identify MedicalPanel's temporary empty content
MarginContainer (immediately replaced) and EconomyDetailPanel's initial unparented
_heatMapDetail (recreated/parented only when actually rendered). Extend Gate 19
using the real EconomyDetailPanel scene. Remove these two unused allocations,
preserving actual medical content and heat-map detail rendering. Exact follow-up
claims entered; no DashboardShell ownership-contract change is necessary.
