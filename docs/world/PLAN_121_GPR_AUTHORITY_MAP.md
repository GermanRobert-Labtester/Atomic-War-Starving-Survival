# Plan 121 — GPR authority map

Status: Core survey-intelligence proof slice implemented, 2026-09-10.

| Concern | Authority | Boundary |
|---|---|---|
| Survey profiles | `gpr_exploration_catalog.json` / `GroundPenetratingRadarCatalogLoader` | owns modes, terrain attenuation and anomaly signatures |
| Cart power | `IPlayerInventoryPort` | survey reserves battery-pack units atomically |
| Survey state | `GroundPenetratingRadarEngine` | owns calibration, active transect, observations and idempotent leads |
| World map | `WastelandMapSystem` | future adapter consumes lead projection; GPR does not create loot or locations directly |
| Excavation | `ExcavationSystem` | retains digging, hazard and reward truth |
| Expedition logistics | expedition/vehicle authority | future cart equipment integration; no direct range bonus in GPR |

The engine deliberately produces uncertain observations and
`BuriedAnomalyLead` projections. The source-plan excavation/terrain owners are
represented by typed inputs where no matching live class exists.
