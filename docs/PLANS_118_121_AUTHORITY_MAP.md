# Plans 118–121 Authority Map

Status: implementation baseline, 2026-09-10.

This map records the repository seams that exist today. Several source-plan
names are historical leads rather than current files: the repository has a
live plastic-pyrolysis system, but no `BiogasDigesterSystem`,
`CompositeArmorEngine`, `VehicleTrackGearEngine`, or `PowerSubstationCatalog`
type under those names. New systems therefore use typed projections into the
live authorities below and do not recreate those missing systems.

| Concern | Current owner | Read API | Write API | Save owner | Tick owner | Tests |
|---|---|---|---|---|---|---|
| Industrial feedstock | `PlasticPyrolysisSystem` output/inventory projection | `State.output_buffer`, inventory `CountById` | pyrolysis claim/inventory port | `PlasticPyrolysisSaveStore` | shelter campaign phase | `PlasticPyrolysisEngineTests` |
| Power/heat availability | `PowerGridSystem` and host projections | `PowerGridHostSession`, typed tick inputs | `PowerGridSystem` actions | `PowerGridSaveStore` | `Main.TickPowerGrid` | `PowerGridSystemTests` |
| Lubrication/maintenance | `EquipmentConditionSystem` plus explicit consumer registry added by Plan 118 | registered consumer/service projection | service event; equipment wear remains equipment authority | new engine state only for active synthesis/service ledger | caller-provided shelter day tick | new synthesis tests |
| Portable sensors | item inventory + equipment condition | `IPlayerInventoryPort`, condition APIs | inventory/equipment action ports | owning equipment/save section | expedition/action cadence | direction-finding and condition suites |
| Electrical faults | `PowerGridSystem` state | power-grid snapshot/inspection projection | power-grid maintenance only | `PowerGridSaveStore` | power-grid day tick | `PowerGridSystemTests` |
| Composite quality | new `CarbonCompositeEngine` | batch/cure projection | typed start/inspect/certify actions | engine state | job/day cadence | new composite tests |
| Vehicle weight/capacity | `ExpeditionSystem` + `VehicleGarageSystem` | `ExpeditionVehicleProfile`, estimate path | garage modifications and expedition setup | expedition aggregate/garage stores | expedition tick owner | expedition logistics tests |
| World survey intel | `WastelandMapSystem` | `MapNodeKnowledgeState`, `MapMarkerState` | `DiscoverSurvey`/marker projection | `WastelandMapSaveStore` / world aggregate | world/expedition action cadence | map persistence tests |
| Excavation hazards | `ExcavationSystem` and `ExcavationHazardSystem` | excavation state/projections | excavation preparation/resolution actions | excavation save stores | shelter/expedition phase | excavation tests |
| Research/crafting | `ResearchSystem`, `CraftingSystem`, item/recipe catalogs | shared research and inventory ports | canonical crafting/inventory transactions | corresponding save stores | crafting/day cadence | catalog/crafting suites |
| Deterministic randomness | `ISeededRng` / `SeededRng` | injected RNG | engine-local draws only | stateful engines persist progress, not regenerated outcomes | owning domain tick | determinism guard/tests |

## Resolved boundaries

- Plan 118 consumes an authored feedstock item through `IPlayerInventoryPort`;
  it does not copy pyrolysis inventory or invent a second gas store.
- Plan 119 observes power assets through a callback. It never writes power
  condition or tactical hazard truth.
- Plan 120 owns batch/cure quality and exposes explicit component results;
  vehicle effects are applied only through a caller-provided component
  projection, not a global mass/range multiplier.
- Plan 121 owns survey observations and creates idempotent map knowledge/lead
  projections. `ExcavationSystem` remains the authority for digging, loot, and
  residual hazard resolution.
- All four engines capture/restore only their own state. Existing inventory,
  power, equipment, expedition, map, and excavation save sections remain the
  source of truth for those domains.

## Plan divergence

The repository currently has no live host panels or save-section registrations
for these four new engines. This tranche begins with Core engines, catalog
loaders, focused tests, and headless selftests. Host panel/save wiring remains
an explicit follow-up after the state boundaries are exercised and reviewed.
