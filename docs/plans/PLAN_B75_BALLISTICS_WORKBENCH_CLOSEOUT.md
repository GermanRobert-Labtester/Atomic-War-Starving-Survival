# Plan B75 — Ballistics workbench closeout

Status: implemented in the current Godot host.

## Delivered

- `BallisticsWorkbenchSystem` owns calibration profiles, ammunition-batch quality, wear, headspace states, refurbishment and bounded combat modifiers.
- `CombatTypes` and tactical firing consume the projection without creating a second equipment-condition authority.
- `ballistics_workbench_catalog.json` and refurbishment recipes are authoritative JSON.
- `BallisticsWorkbenchHostSession` and `ballistics_workbench` campaign save section are wired.
- `BallisticsWorkbenchPanel` exposes profile registration, inspection, calibration and refurbishment.

## Verification

- `Plans74To77SystemsTests.BallisticsWorkbench_UsesEventIdForIdempotentWear`
- Core and Godot host builds pass.
- Save registry, triad, architecture-map and filename gates pass.

Known limitation: the panel uses the existing equipment inventory and condition authorities; it does not invent a separate weapon inventory.
