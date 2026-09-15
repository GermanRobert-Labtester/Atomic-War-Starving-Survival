# Plan 119 — UV corona authority map

Status: Core observation proof slice implemented, 2026-09-10.

| Concern | Authority | Boundary |
|---|---|---|
| Detector profile | `uv_corona_detector_catalog.json` / `UvCoronaDetectionCatalogLoader` | owns range, noise, battery and drift values |
| Electrical truth | power-grid/fault caller | engine observes supplied faults and never repairs or mutates them |
| Battery | `IPlayerInventoryPort` | scan consumes the catalog battery item atomically |
| Environment | caller-provided environment ID/catalog profile | visibility, ash and fault-activity modifiers are bounded |
| Observation state | `UvCoronaDetectionEngine` | owns calibration, condition, scan count and observations |
| Tactical/map consumers | host/world integration seams | observations are projections; no duplicate hazard truth is created |

The repository does not contain the source-plan `PowerSubstationCatalog` or a
live UV panel, so the engine accepts typed fault inputs and is ready for a
production projection adapter without taking ownership of power state.
