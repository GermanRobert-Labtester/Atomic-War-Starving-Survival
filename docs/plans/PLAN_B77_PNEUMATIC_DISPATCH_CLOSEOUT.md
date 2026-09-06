# Plan B77 — Pneumatic dispatch closeout

Status: implemented in the current Godot host.

## Delivered

- `PneumaticDispatchSystem` owns station/link catalogs, route selection, pressure, capsule queues, cargo conservation, seals, jams, blackout behavior and voice-pipe availability.
- `pneumatic_network_catalog.json` is the authoritative network catalog.
- `PneumaticDispatchHostSession` registers room endpoints against the canonical inventory authority.
- `pneumatic_dispatch` is persisted in the campaign envelope and participates in the daily coordinator.
- `PneumaticDispatchPanel` exposes dispatch, jam clearing, link maintenance and blackout state.

## Verification

- `Plans74To77SystemsTests.PneumaticDispatch_PreservesCargoAndBlocksDuringBlackout`
- Core and Godot host builds pass.
- Data-integrity and content-utilization gates pass.

Known limitation: the first host projection maps room terminals onto the existing shared inventory because no per-room inventory authority exists. Cargo is still removed and delivered through the Core endpoint contract without a duplicate warehouse.
