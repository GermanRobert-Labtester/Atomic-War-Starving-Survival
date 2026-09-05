# Plan 71 — Power Grid Baseline Reconnaissance

> **Status:** Grounded baseline inspection completed 2026-09-03.
> **Authority:** `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`, `Assets/StreamingAssets/Data/power_grid.json`, `src/Host/PowerGridHostSession.cs`.

---

## 1. Executive Summary

`Assets/StreamingAssets/Data/power_grid.json` established a 6-room footprint:
1. `room_air_filtration` (180 W, critical, `fx_filtration_off`)
2. `room_clinic` (120 W, critical, `fx_clinic_off`)
3. `room_water_pump` (100 W, critical, `fx_water_pressure_drop`)
4. `room_greenhouse` (160 W, standard, `fx_grow_lights_off`)
5. `room_foundry` (220 W, low, `fx_foundry_standstill`)
6. `room_lighting_main` (80 W, low, `fx_lighting_dim`)

Total baseline demand is **860 W**, exceeding the default generator output of **800 W** by 60 W (covered by battery reserves of **4,000 Wh** and fuel reserve of **100 units**).

---

## 2. Core System Architecture

`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` is the single authority for electrical calculations:
- **Generation:** `float GenerationWatts` (default 800 W).
- **Fuel:** `float FuelUnits` (default 100 units, burned proportional to generation: `gen * 24f * 0.001f` per day).
- **Battery:** `float BatteryReserveWh` and `float BatteryCapacityWh` (default 4,000 Wh).
- **Draw calculation:** `ComputeTotalDraw()` sums `DrawWatts` for all rooms with closed breakers, not tripped, and priority != `Disabled`.
- **Net power:** `NetWatts = GenerationWatts - TotalDrawWatts`.
- **Brownout threshold:** `IsBrownout = TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0`.
- **Room powered predicate:** `IsRoomPowered(roomId) = IsBreakerClosed(roomId) && !IsRoomTripped(roomId) && !IsBrownout`.

---

## 3. Priority Enum & Semantics

Defined in `Ashfall.Core.Shelter.PowerGridRoomPriority`:
- `Disabled = 0`: Excluded from draw calculation entirely (`DrawWatts` contributes 0 W).
- `Low = 1`: Non-essential comfort and industrial loads.
- `Standard = 2`: Standard operational facilities (crafting, cooking, science, communications).
- `Critical = 3`: Life-support core (air filtration, water treatment, emergency medical, airlock).

---

## 4. Host Integration & Drift Resolution

`src/Host/PowerGridHostSession.cs` previously hardcoded 4 rooms in `DefaultGrid()` with a comment claiming it matched `power_grid.json`.
Plan 71 updates `LoadGridJson()` to read from `power_grid.json` dynamically via `CatalogPath.ResolveDataDir()`, falling back to `DefaultGrid()`, and aligns `DefaultGrid()` to all 18 rooms.
