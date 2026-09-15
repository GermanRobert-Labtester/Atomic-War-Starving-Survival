# Expedition balance baseline

Plan 51 characterization for the current Core expedition authority.

## Authorities and formula map

| Concern | Authority | Evidence |
|---|---|---|
| Travel and encounters | `Ashfall.Core.Expeditions.ExpeditionSystem` | `ExpeditionSystem.Tick` |
| Estimate | `ExpeditionSystem.Estimate` | `PreviewStart` uses the same vehicle profile inputs |
| Vehicle values | `Assets/StreamingAssets/Data/vehicles.json` | 8 authored vehicle profiles + foot fallback |
| Vehicle state | `ExpeditionVehicleSystem` / `ExpeditionAggregateState` | garage and active sortie save state |
| Weapon condition | `WeaponEquipmentBridge` → `WeaponConditionSystem` | no expedition-local condition authority |
| Day ownership | campaign coordinator | host day-owner path |

Travel execution is discrete: each travel tick uses the vehicle speed profile,
rounds the resulting progress with the Core midpoint rule, and charges the
profile fuel rate. The estimate path now uses that same discrete step function;
the prior continuous estimate could disagree on short routes. This was a
behavior-neutral correctness fix to preview parity, not a vehicle rebalance.

The 30-day proof uses the real data catalog, fixed seed `51051`, a three-day
sortie cadence, foot fallback, all eight authored vehicle profiles, loot
return, and the garage state path. No vehicle-data tuning was applied.

## Current balance knobs

- `speed_multiplier`
- `cargo_capacity`
- `fuel_consumption_per_km`
- `breakdown_threshold`
- garage condition/fuel state and repair/refuel costs
- route distance, danger, readiness, and world influence

Balance changes remain data-only in `vehicles.json` or an existing profile
constant. Weapon ballistics and condition rules are outside this authority.
