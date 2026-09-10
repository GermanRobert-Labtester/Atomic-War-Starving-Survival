# ENGINEERING SUBSYSTEM AUTHORITY MAP

## 1. System Authority vs. Narrative Projection
In ASHFALL, the shelter simulation is strictly authoritative. Subterranean infrastructure and machinery states are computed by dedicated Core systems running in the game loop. The maintenance glitch catalog and engineering logs are diagnostic and atmospheric narrative projections that observe or enrich this state, never driving or mutating it.

```
Authoritative Core Simulation (State Owners)
┌─────────────────────────┐   ┌─────────────────────────┐   ┌─────────────────────────┐
│   PowerGridSystem       │   │  StartingLevelSystem    │   │  ShelterThermalSystem   │
│   (Generator / Battery) │   │  (HEPA filter, radon)   │   │  (Boilers / Steam)      │
└────────────┬────────────┘   └────────────┬────────────┘   └────────────┬────────────┘
             │                             │                             │
             └──────────────────────┐      │      ┌──────────────────────┘
                                    ▼      ▼      ▼
                              ┌─────────────────────────┐
                              │  BunkerMaintenance      │
                              │  Projection Service     │
                              └────────────┬────────────┘
                                           ▼
                              ┌─────────────────────────┐
                              │  Diagnostic Glitch /    │
                              │  Maintenance Tell       │
                              │  (Read-Only Display)    │
                              └─────────────────────────┘
```

---

## 2. Authoritative Domain Mapping

| Subsystem Category | Authoritative Core System | Authority Source | Machine Condition Keys | Canonical Room Target |
|---|---|---|---|---|
| **Heating / Steam / Boiler** | `ShelterThermalSystem` / `StartingLevelSystem` | Core simulation loop | `MachineConditionKeys.ThermalBoilerFuel` | `room_filtration`, `room_bunks`, `room_bunker_corridor` |
| **Water / Drainage / Sump** | `SumpFloodingSystem` / `StartingLevelSystem` | Hydrology simulation | `MachineConditionKeys.WaterFilterIntegrity` | `room_water_pump` |
| **Power / Generator / Electrical** | `PowerGridSystem` / `PowerDistributionSubgridSystem` | Electrical simulation | `MachineConditionKeys.PowerFuelUnits`, `MachineConditionKeys.PowerBatteryReserve` | `room_workshop`, `room_filtration` |
| **Ventilation / Scrubber / Filter** | `VentilationSystem` / `StartingLevelSystem` | Atmospheric simulation | `MachineConditionKeys.HepaFilterHealth`, `MachineConditionKeys.VentilationFilterSaturation`, `MachineConditionKeys.VentilationDuctIntegrity` | `room_filtration`, `room_airlock` |
| **Structural / Airlock / Blast Door** | `StartingLevelSystem` / `AirlockIncidentSystem` | Security/hazard state | `MachineConditionKeys.AirlockIncidentActive` | `room_airlock`, `room_main` |
| **Life Support / Biosphere / Green** | `StartingLevelSystem` / `GreenhouseSystem` | Biosphere monitoring | `MachineConditionKeys.HepaRadon` | `room_greenhouse`, `room_filtration` |
| **Communications / Precision** | `RadioSystem` / `StartingLevelSystem` | Comms network state | N/A (Stationary apparatus) | `room_radio_tuner`, `room_bunker_corridor` |

---

## 3. One-Way Projection Rules
1. **Zero Simulation Mutation**: Calling any method on `BunkerMaintenanceCatalog` or `BunkerMaintenanceProjection` must never modify power levels, water reserves, temperature readings, battery charge, or equipment health.
2. **Snapshot at Query Time**: When the UI or journal queries a machine tell or glitch log, it passes the current system readings as an input snapshot. The projection selects or decorates the matching narrative profile.
3. **No Automatic Breakdown Injection**: Authored narrative glitches (e.g. `glitch_18_substation_battery_rack_thermal_runaway`) do NOT cause a real battery fire in `PowerGridSystem`. Real system failures are driven by fuel depletion, durability loss, or hazard events, which then look up the appropriate narrative log.
4. **Idempotent Inspection**: Inspecting a room or viewing a maintenance report multiple times produces identical diagnostic projections given identical machine state.
