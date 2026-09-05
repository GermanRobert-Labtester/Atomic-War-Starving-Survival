# Power Generator vs. Consumer Boundary

> **Architectural Decision:** Rejection of `room_generator_room` as a 0 W fake consumer.

---

## 1. The Conflict

The initial planning prompt suggested:
- `room_generator_room`
- `draw_watts`: 0 W
- `priority`: critical
- `failure`: no generation

## 2. Invariant & Forensic Analysis

1. **Generation is not consumption:** In `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`, generator state is modeled explicitly via:
   - `float GenerationWatts`
   - `float FuelUnits`
   - `float BatteryReserveWh`
2. **0 W consumer anomaly:** If a 0 W entry were added to `power_grid.json`, `ComputeTotalDraw()` would add 0 W. It would never participate in overload calculation, never draw power, and never be subject to meaningful load shedding.
3. **Generator failure ownership:** Generator loss is an incident- or maintenance-level source event (`incident_generator_failure`), not a downstream room power-loss effect. When the generator fails, generation drops, causing brownouts elsewhere.

## 3. Resolution

- **Decision:** Do NOT author `room_generator_room` as a 0 W consumer.
- **Replacement:** The 18-room target is fulfilled using legitimate electrical consumers. As authorized by Constraint 1.2, the slot is assigned to `room_airlock` (Decontamination Airlock, 110 W, critical) and `room_ward_quarantine` (Isolation Quarantine Bay, 70 W, critical).
- **Generator Representation:** Generator facilities remain authoritative producers in `PowerGridState.GenerationWatts` and `PowerGridState.FuelUnits`, reacting to mechanical incidents and maintenance rosters.
