# PLAN 125 — Amphibious Draisine Authority Map (Phase 1)

**Status:** ACCEPTED (reconnaissance). Premise-verified against current source.

## Verified owners

| Concern | Owner | Evidence |
|---|---|---|
| Water topology | `World/WastelandMapSystem.cs` routes | `TravelDomain` "land"/"water" (:850), water current strength (:853), contamination (:856) — **water crossings are already topology-owned** |
| Vehicle upgrade/component precedent | `Expeditions/ArmoredCrawlerModuleCatalog.cs` | `CrawlerModuleDefinition`: `slotType`, `mass`, `powerDraw`, `fuelModifier`, `cargoModifier`, `tags`, `Validate` (:11–38) — the module schema Plan 125 mirrors |
| Vehicle state / customization | `Expeditions/VehicleGarageSystem.cs` | `VehicleCustomizationRecord.installedSlots` (:12), chassis/engine/transmission wear (:13–15), immobilization |
| Expedition travel | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` (root), `Expeditions/ExpeditionSystem.cs`, `Expeditions/ExpeditionTravelStretch.cs` | travel/action cadence |
| Naval (distinct — must not replace) | `Expeditions/ExpeditionNavalSystem.cs` | `NavalVesselDef` (capacity, base_speed, cargo, crew, propulsion, fuel_rate :9–21) — open-water remains naval-only |
| Draisine (existing) | `Expeditions/DraisineRerailingSystem.cs` | rail-side draisine owner; amphibious kit extends vehicle capability, not this system |
| Weather/current | `World/WeatherSystem.cs`; route-level `current_risk` authored category if canonical current absent on routes | bounded modifiers |
| Save | new `amphibious_draisine` section via `SaveSectionRegistry` (or additive field on the vehicle garage aggregate if §7.14 integration fits) | kit id, condition, pontoons, crossing state, pump |
| RNG | fork keys `amphibious.pontoon_damage`, `amphibious.ingress_event` | seeded damage parity |

## Answers to workstream 125-A questions

1. Are rivers explicit topology edges? Yes — route `TravelDomain` = "water" with current
   strength and contamination fields on `WastelandMapSystem` routes.
2. Crossings nodes or terrain tags? Route-level domain tags (not separate nodes).
3. Can vehicles declare traversal capabilities? Not yet generically — crawler modules
   define modifier fields + tags; the amphibious kit follows the same schema shape and
   exposes `WaterCrossingRouteCapability` for the route planner.
4. Does cargo mass affect speed/fuel? Vehicle profile-driven (garage/crawler precedent);
   kit cargo modifier flows through existing logistics recalculation.
5. Does water current exist? Yes, per-route current strength (:853).
6. Can route segments carry dynamic risk? Risk derives from route current + weather +
   kit condition at crossing time; route data itself stays map-owned.
7. How are vehicle breakdowns persisted? Garage records (stress/fouling/wear/immobilized)
   — kit condition joins this pattern without duplicating vehicle truth.

## Design decisions

- Kit = vehicle upgrade via the garage/module schema (not a new vehicle authority).
- Buoyancy abstraction: `effective_margin = flotation_capacity - vehicle_mass -
  cargo_penalty`; below minimum → crossing unavailable (plan §7.6).
- Deep water / open-water travel stays with `ExpeditionNavalSystem`; amphibious kit
  unlocks only authored shallow-water route classes (destroyed rail bridge, floodplain,
  swamp rail corridor, submerged causeway) — test invariant 19 (no naval replacement).
- Abort/recovery state machine: LandReady → Deploying → WaterReady → Crossing → Landing →
  Recovering → LandReady, with Aborted/Disabled/EmergencyRecovery branches; save/load
  mid-crossing required.

## Non-goals

No real conversion/inflation/sealing procedures; no universal water-tile traversal; no
direct UI map unlock; no naval-system duplication; skilled operators never guarantee
success (bounded by route risk and seeded hazards).

## New files (planned)

- `Assets/Ashfall.Core/Expeditions/AmphibiousDraisineEngine.cs`
- `Assets/StreamingAssets/Data/amphibious_draisine_catalog.json`
- `src/Host/AmphibiousDraisineHostSession.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`
- `src/Main.AmphibiousDraisine.cs`, `src/UI/AmphibiousDraisinePanel.cs`
- `Ashfall.Core.Tests/Expeditions/AmphibiousDraisineEngineTests.cs`
- Save rows: `amphibious_draisine` (or documented additive field on vehicle garage)
