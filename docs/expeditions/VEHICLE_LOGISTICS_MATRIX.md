# Vehicle Expedition Logistics Matrix

**Document:** `docs/expeditions/VEHICLE_LOGISTICS_MATRIX.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/vehicles.json`
**Runtime System:** [`Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`](../../Assets/Ashfall.Core/ExpeditionVehicleSystem.cs)

---

## 1. Route Simulation & Fuel Economics

Representative route consumption modeled over short (20 km), medium (60 km), and long (150 km) expedition sorties:

| Vehicle ID | 20 km Short Route (Fuel) | 60 km Medium Route (Fuel) | 150 km Long Route (Fuel) | Max Range (Full Tank) | Net Usable Cargo | Primary Mission Profile |
|---|---|---|---|---|---|---|
| `vehicle_utility_quad` | 6.0 L | 18.0 L | 45.0 L (Requires refueling) | 133 km | 90 kg | Short-range supply runs & local salvaging. |
| `vehicle_dirt_bike` | 4.0 L | 12.0 L | 30.0 L (Requires refueling) | 125 km | 30 kg | Rapid reconnaissance and radio relay inspection. |
| `vehicle_cargo_truck` | 10.0 L | 30.0 L | 75.0 L | 160 km | 250 kg | Major hub logistics and heavy trade convoys. |
| `vehicle_steam_halftrack` | 14.0 L | 42.0 L | 105.0 L | 171 km | 180 kg | Rough terrain crossing and swamp expeditions. |
| `vehicle_armored_mobile_base` | 19.0 L | 57.0 L | 142.5 L | 210 km | 380 kg | Fortified long-haul relocations & deep sector operations. |
| `vehicle_salvage_dredger` | 11.0 L | 33.0 L | 82.5 L | 172 km | 260 kg | Coastal wharf salvage and deep-coast diving support. |
| `vehicle_scout_motorcycle` | 3.6 L | 10.8 L | 27.0 L (Requires refueling) | 100 km | 18 kg | Emergency medical courier and message delivery. |
| `vehicle_ambulance_rig` | 9.0 L | 27.0 L | 67.5 L | 133 km | 140 kg | Casualty rescue and contagious quarantine transport. |
