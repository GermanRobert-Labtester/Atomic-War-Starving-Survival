# Plan B74 — Geothermal ORC closeout

Status: implemented in the current Godot host.

## Delivered

- `GeothermalOrcSystem` owns deterministic ORC loop state, fouling, reserve depletion and recovery, leakage isolation, maintenance, waste heat and persistence.
- `geothermal_strata_catalog.json` is the authoritative strata catalog.
- `GeothermalOrcHostSession` publishes electrical output to `PowerGridSystem` through the keyed external-generation seam.
- `geothermal_orc` is registered in the campaign envelope and reset lifecycle.
- `GeothermalOrcPanel` exposes loop commissioning, flow, descaling and repair through typed host actions.

## Verification

- `Plans74To77SystemsTests.GeothermalOrc_OperatesAndRestoresDeterministically`
- Core and Godot host builds pass.
- Data-integrity and content-utilization gates pass.

Known limitation: waste heat is exposed as a projection for the existing thermal/water authorities; this slice does not create a second heating or water authority.
