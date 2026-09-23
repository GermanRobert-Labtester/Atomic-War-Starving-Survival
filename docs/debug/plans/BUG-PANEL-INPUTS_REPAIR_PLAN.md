# BUG-PANEL-INPUTS repair plan

## Findings, root causes, and scope

PneumaticDispatchPanel creates capsule/link inputs used by existing commands but
never parents them. Players cannot supply a capsule to clear a jam or change the
maintenance link; both LineEdits leak. InventoryPanel discards SetSidebar's return
value, leaving _sidebar null and never subscribing its existing filter handler.

Both are missing presentation wiring, not missing Core commands. Claim just these
panel regions and lifecycle regressions. Parent both inputs through existing
AddRow and assign _sidebar. Deleting the fields or bypassing the existing filter
would hide symptoms or duplicate authority; do neither.

## Invariants, tests, and sequence

Preserve existing commands, filter definitions, data, saves, and RNG. Add focused
gates proving maintenance inputs are in the UI and emit chosen IDs, plus inventory
selection from all → material → consumable → all using authored catalog entries.
Run red before editing; implement the minimum wiring; build and rerun lifecycle
and holdfast checks at 15 FPS. Rollback only this session's hunks. Done requires
both behavioral regressions green with no adjacent runtime regression.
