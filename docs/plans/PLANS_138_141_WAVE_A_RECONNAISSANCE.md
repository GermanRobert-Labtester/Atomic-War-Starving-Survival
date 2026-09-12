# Plans 138–141 — Wave A Shared Reconnaissance (Authority Map)

**Status:** COMPLETE (read-only) · **Date:** 2026-09-12 · **Evidence rule:** AGENTS.md rule 7 — every named authority verified in source before implementation.

## Verified live authorities

| Plan | Plan text names | Verified live owner | Verdict |
|---|---|---|---|
| 138 | `RadiationSystem.cs` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | ✅ reuse |
| 138 | `WaterTreatmentSystem.cs` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | ✅ reuse |
| 138 | `KitchenNutritionSystem.cs` | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | ✅ reuse |
| 138 | `PharmaLabPanel.cs` | `src/UI/PharmaLabPanel.cs` | ✅ reuse (lab UI precedent) |
| 138 | `RefractoryCeramicsEngine.cs` | **MISSING** — only `Narrative/CeramicsKilnCatalog.cs` exists | ⚠️ no live ceramics engine; do not cite it as an owner |
| 139 | `WastelandMapSystem.cs` | `Assets/Ashfall.Core/World/WastelandMapSystem.cs` | ✅ reuse (terrain truth) |
| 139 | `TerrainTopologyCatalog.cs` | **MISSING** — topology lives inside `WastelandMapSystem` | ⚠️ extend in place; do not create the catalog file |
| 139 | `DeepExcavationSystem.cs` | **MISSING** — real owners: `ExcavationSystem.cs`, `Excavation/ExcavationHazardSystem.cs`, `Excavation/ExcavationCatalogLoader.cs`, `src/Host/ExcavationHazardSaveStore.cs` | ⚠️ use real names |
| 139 | seismic/geology | `Shelter/SeismicDynamicsSystem.cs` (+ `.Monitoring.cs` partial), `Narrative/GeologicalStrataCatalog.cs`, `Narrative/HydroGeology*` | ✅ consume, not replace |
| 139 | weather/cloud | `Assets/Ashfall.Core/World/WeatherSystem.cs` | ✅ reuse |
| 140 | `FoundryProductionSystem.cs` | **MISSING** — real owner: `Foundry/SilentFoundrySystem.cs` (+ `.Metallurgy`, `.Heat`, `.Glassworks`, `.TreatyLabor` partials), `Foundry/SilentFoundryCatalog.cs`, `Foundry/MetallurgyHeavyCatalog.cs`, `Foundry/PowderMetallurgySystem.cs`, `Foundry/CupolaFoundryEngine.cs`, `Foundry/FoundryActionSurface.cs` | ⚠️ extend `SilentFoundrySystem` |
| 140 | `VacuumInductionMeltingEngine.cs` | **MISSING** — no live equivalent | ⚠️ melting authority is inside the SilentFoundry family |
| 141 | `ExpeditionSystem.cs` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | ✅ reuse |
| 141 | `ExpeditionVehicleLogistics.cs` | **MISSING** — real owners: `ExpeditionVehicleSystem.cs`, `Expeditions/VehicleGarageSystem.cs`, `Expeditions/VehicleGarageCatalog.cs`, `src/Host/VehicleGarageSaveStore.cs` | ⚠️ use real names |
| 141 | `RubberVulcanizationEngine.cs` | **MISSING** — no rubber/vulcanization authority exists | ⚠️ Plan 141 must not premise on it; run-flat profiles come from the new catalog only |
| all | `items.json` | `Assets/StreamingAssets/Data/items.json` | ✅ authoritative |

## Plan-numbering check

No `PLAN_138..141` documents exist anywhere in `docs/` — numbering is free.

## Save ownership

Every touched domain already has an owner in the canonical save aggregate
(radiation, water treatment, excavation hazard, seismic dynamics, vehicle
garage, expeditions). Plans must **extend** those Capture/Restore paths via
`SaveSectionRegistry` — never mirror.

## Consequences for implementation

1. All file/test names in the flagship plan that reference missing systems
   must be rewritten to the verified owners above.
2. Plan 140 extends `SilentFoundrySystem` (a partial-class family) — new
   surface likely belongs in a new `SilentFoundrySystem.Extrusion.cs` partial
   plus catalog, following the existing family pattern.
3. Plan 141 adds a wheel/tire component to `ExpeditionVehicleSystem` /
   `VehicleGarageSystem` state — no rubber-engine premise.
4. Plan 139 adds survey/observation state as a new intelligence save section
   (no existing owner) and consumes `WastelandMapSystem` + `WeatherSystem` +
   `SeismicDynamicsSystem` + excavation authorities read-only.
5. Plan 138 extends `RadiationSystem` assay surface (or its lab host seam)
   with shield/calibration inputs — no second radiation truth.
