# Plan 168 — Water Delivery Authority Map

**Debt:** `DEBT-PLAN168-WATER-DELIVERY`  
**Status:** SEALED 2026-09-12  
**Package:** `DEBT-168-WATER-DELIVERY`

## Authority table

| Concern | Owner |
|---|---|
| Spendable bulk liters + `WaterType` | `WaterTreatmentSystem` |
| In-network transit liters + `FluidQuality` | `FluidLogisticsSystem` |
| Atomic WT → Fluid handoff | `FluidWaterTreatmentBridge.TransferTreatedWater` |
| Plot moisture / soil contamination | `GreenhouseSystem` |
| Infection / water protocol | `DiseaseSystem` |
| Packaged bottles (`clean_water` items) | Inventory (separate packaging authority) |

## No-double-ledger rule

Liters leave WT exactly once into a Fluid reservoir. `Solve` consumes network
volume into sink deliveries. Greenhouse/Disease react to that delivery once.
Do not also spend inventory bottles for the same physical liters.

Manual greenhouse watering via inventory bottles remains a separate player
packaging path (`GreenhouseHostSession.Water`). Fluid irrigation calls
`GreenhouseSystem.Water` directly.

## Default topology (seed when empty)

- `reservoir_main` (Reservoir)
- `sink_greenhouse` (Sink, priority 2)
- `sink_drinking` (Sink, priority 1)
- Edges `pipe_res_gh`, `pipe_res_drink`

## Quality mapping

| From | To |
|---|---|
| `WaterType.Clean` | near-zero `FluidQuality` (Potable) |
| Non-clean WT types | elevated pathogen/radiological axes (Unsafe+) |
| Greenhouse `tainted` | `delivered.Band != Potable` |
| Drinking Potable | `DiseaseSystem.PurifyWater(day)` |
| Drinking Unsafe/Toxic | `DiseaseSystem.TryExpose` waterborne (`disease_cholera`, source `fluid_drinking`) when a living survivor is available |

## Daily cadence

1. Ensure default topology  
2. Transfer bounded clean water WT → `reservoir_main`  
3. `FluidLogisticsSystem.Tick` / `Solve`  
4. Apply sink deliveries to Greenhouse + Disease  

Last-delivery maps are ephemeral; reload relies on persisted WT liters + Fluid
network volumes, then same-day transfer/solve/apply.

## Focused verification

`Ashfall.Core.Tests/Plan168WaterDeliveryTests.cs`
