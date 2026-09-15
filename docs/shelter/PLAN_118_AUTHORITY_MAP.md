# Plan 118 — Synthetic lubricant authority map

Status: Core proof slice implemented, 2026-09-10.

| Concern | Authority | Boundary |
|---|---|---|
| Feedstock | existing inventory projection (`synthetic_fuel_canister`) | reactor consumes through `IPlayerInventoryPort`; it does not own a warehouse |
| Process profile | `fischer_tropsch_catalog.json` / `FischerTropschCatalogLoader` | catalog owns authored bands, split, catalyst and product IDs |
| Reactor state | `FischerTropschSynthesisEngine` | owns active batch, catalyst condition, progress and output buffer |
| Equipment wear | `EquipmentConditionSystem` | lubricant service is an explicit registered-consumer seam; wear remains equipment-owned |
| Power/heat | existing shelter/power authorities | supplied as bounded tick inputs; no duplicate thermal/power state |
| Persistence | engine `CaptureState`/`RestoreState` contract | dedicated host save registration is deferred until a live production owner exists |

The repository has no live `BiogasDigesterSystem` or named Fischer–Tropsch
host panel. This implementation uses existing inventory and industrial seams
instead of creating a parallel upstream resource authority.
