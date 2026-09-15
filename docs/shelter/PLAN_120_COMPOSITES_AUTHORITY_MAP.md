# Plan 120 — Carbon composite authority map

Status: Core batch/cure proof slice implemented, 2026-09-10.

| Concern | Authority | Boundary |
|---|---|---|
| Material/cure/component definitions | `carbon_composite_catalog.json` / `CarbonCompositeCatalogLoader` | catalog owns freshness, cure targets, quality and component factors |
| Input inventory | `IPlayerInventoryPort` | jobs consume material atomically; engine does not own stock |
| Cure job | `CarbonCompositeEngine` | owns active job, defect roll, cure progress, autoclave condition and output buffer |
| Cold storage | caller-provided bounded input | engine does not duplicate refrigeration state |
| Vehicle/sensor/cart effects | explicit `CompositeComponentProjection` | consumers opt in; no universal mass/range bonus |
| Equipment condition | existing equipment authority | supplied through job conditions; no second condition store |

The named `CompositeArmorEngine` and `VehicleTrackGearEngine` are absent in
this checkout. The engine exposes explicit component output for later
integration rather than fabricating those owners.
