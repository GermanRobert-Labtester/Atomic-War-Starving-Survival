# BUG-SLURRY-CLEANUP repair plan

## Bug and proof

SetupSumpFlooding removes the existing SlurryDewateringSumpPanel from Main,
overwrites the field with a new instance, and never frees the old subtree.
The initial instance carries the OnClose wiring; the replacement does not.
The isolated holdfast UI bootstrap reports the entire unparented slurry subtree.

## Authority, scope, and options

User explicitly authorized takeover of only this replacement/cleanup block from
Plan 24. Main.UiPanels and the adjacent sump panel block remain untouched.
Reuse the existing valid slurry panel and bind it to the canonical session;
create/attach/wire close only when absent. This preserves node ownership and
existing close wiring. Alternatively queue-free and recreate the old panel, but
that introduces avoidable churn and requires rebuilding identical UI state.

## Invariants and test

No new host/session authority, save fields, RNG calls, data, or gameplay behavior.
Retain hide-on-setup semantics. Add a runtime assertion that bootstrap leaves a
bound, parented slurry panel and no orphan SlurryDewateringSumpPanel. Prove failure
before and success after. Recheck holdfast runtime at bounded 15 FPS and build.
Rollback only owned hunks. Acceptance includes preservation of existing OnClose.
