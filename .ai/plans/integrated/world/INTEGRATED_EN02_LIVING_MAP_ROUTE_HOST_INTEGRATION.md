# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# EN-02 Living Map Route Projection Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EN-02-LIVING-MAP-ROUTE-SURFACE`
**Gate authority:** `Ashfall.Core.World.WastelandMapSystem.ProjectLivingMapRoute` (previously 0 `src/` references)

## Bounded outcome

Give the signed EN-02 read model an operational host surface. It derives route hops, total distance,
and flooded/amphibious hazards and tags from canonical `PlanRoute` without rebuilding the travel
planner or writing state. Read-only projection; no save, no duplicate authority.

## Delivered

- New `src/Host/HostCli.LivingMapRoute.cs` — 8-check probe `LivingMapRouteSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `LivingMapRouteSelfTest` + descriptor
  `--living-map-route-selftest` (alias `--map-route-projection-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

`godot --headless -- --living-map-route-selftest` → 8/8 (empty invalid; single-hop 2 nodes/1 hop/
8.5 km with flooded+amphibious; endpoint preservation; multi-hop 3 nodes/2 hops/25.5 km mixed
hazards; direct-constructor null/negative safety; default validity). Build 0 errors; parity 4/4.

## Non-goals

No change to `WastelandMapSystem.ProjectLivingMapRoute`; no new map state; no UI panel (the map
projection is the read authority, consumed through the CLI surface).
