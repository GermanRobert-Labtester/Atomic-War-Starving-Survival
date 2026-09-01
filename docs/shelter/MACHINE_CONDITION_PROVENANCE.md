# Plan 29 — Machine Condition Provenance (Phase 0, §3.5)

> One row per machine. **No second meter may be introduced for anything in the
> "Modeled" rows** — Plan 29 may only project/narrate these. Verified 2026-09-01.

## 1. Provenance table

| Machine | Runtime owner | Condition field(s) | Failure threshold | Maintenance action | Save owner |
|---|---|---|---|---|---|
| HEPA air filtration stack | `StartingLevelSystem` | `airFilterHealthPercent` (0–100); `airQualityPercent`; `radonLevelBqm3` | warning < 50; hazard `airHazardWarning` when filter <50 or radon >30 | `ServiceAirFilter()` (−1 mechanical scrap, +25) · `ReplaceAirFilter()` (−1 filter spare, →100) | starting-level section |
| Ventilation exhaust filter | `VentilationSystem` | `exhaustFilterSaturation` (0–100) | hazard when CO > 100 ppm or smoke > 60 | `ServiceFilter()` (−25 saturation) · `ReplaceFilter()` (→0) | ventilation section |
| Ventilation ducts | `VentilationSystem` | `ductIntegrity` (100→) | degrades 0.5/day when smoke > 5 | `ClearDuct()` (+20) | ventilation section |
| Water treatment filters | `WaterTreatmentSystem` | `filterIntegrity` (0–100); `filterReplacements` | blocked when RO/decon input can't cover `FilterDegradePerUnit` | `ReplaceFilter()` (→max) | `water_treatment` |
| Generator + battery | `PowerGridSystem` | `GenerationWatts`, `FuelUnits`, `BatteryReserveWh` (capacity clamp) | brownout when draw > gen and battery ≤ 0; breaker trips ≥ 4 h brownout (seeded 0.10/room) | `AddFuel()`; breaker/priority management | `power_grid` |
| Water pump | — (power draw only) | **unmodeled** (pump hardware) | power loss → `fx_water_pressure_drop` (host effect) | none (fuel/breaker only) | `power_grid` (breaker state) |
| Boiler | `ShelterThermalSystem` | `boilerFuelLevel`, `boilerCurrentTempC`, `boilerActive` | fuel ≤ 0 → boiler off | `SetBoilerActive()` (+10 initial fuel) | `shelter_thermal` |
| Heating pipes | `ShelterThermalSystem` | `PipeSegment.condition` (0–100), `hasBurst` | burst: room < 0 °C AND condition < 50 AND seeded roll < 0.05 | `RepairPipe()` (+20; seals at ≥50) — preview/execute command pair exists | `shelter_thermal` |
| Radiators | `ShelterThermalSystem` | `ThermalRoomNode.radiatorValveOpen`, `isFrozen`, `freezeDamage` | frozen < −2 °C (freezeDamage +0.1/day) | `ThawRoom()`; valve control | `shelter_thermal` |
| Silent Foundry facility | `SilentFoundrySystem` | 5 components: `refractoryLining`, `hearthTuyeres`, `sandBeds`, `structuralSupports`, `safetyExhaust` (each 0–100) | component conditions degrade with use; overall = mean of 5 | `StartRepair(component)` — consumes firebrick + labour + time; repair log with before/after | `silent_foundry` |
| Airlock door | `AirlockSecuritySystem` | `AirlockDoorState` cycle, sentry assignment, incident log | `hasActiveIncident` | `CycleDoor()`, incident resolution | `airlock_security` |
| Greenhouse equipment | `GreenhouseSystem` | **unmodeled** (no condition fields) | power loss only (`fx_grow_lights_off`) | none | greenhouse section |
| Survivor gear / weapons | `EquipmentConditionSystem` | per-instance 0–100 condition | 0 = broken | repair via crafting | equipment-condition section (**out of Plan 29 machine scope**; canonical durability authority per AGENTS) |

## 2. Threshold→tell mapping candidates (feed Task 29B, not implemented here)

Existing condition bands that already support truthful diagnostic tells:

- HEPA stack: `airFilterHealthPercent` < 50 → pressure/rattle tell; hazard weather
  doubles degrade → "clogs faster in ashfall" contextual quirk (weather is authoritative
  `WeatherKind`).
- Ventilation: saturation rising + valves closed → "no exhaust path" thump/whistle;
  CO > 100 ppm is already a logged CRITICAL.
- Pipes: cold room + condition < 50 → pre-burst "three-beat knock" tell band.
- Foundry: component < 50 → blower/bearing tells per component (names already canonical:
  Hearth Tuyeres, Safety Exhaust…).
- Power: battery reserve approaching 0 → relay-chatter tell; ≥ 4 h brownout → trips
  (already typed events `PowerGridEventKind.Tripped`).

## 3. Unmodeled machines — decision required (Plan 29 §13.2)

- **Generator hardware wear**: currently the generator never degrades — only fuel does.
  Either (a) add the smallest supported wear field to PowerGridSystem (architecture work,
  Plan 29 §29C.4 gate applies), or (b) keep generator quirks contextual-only
  (cold-start/fuel-starve tells from existing state). Recon recommendation: **(b) for
  Plan 29** — the seeded fuel/brownout/trip state already supports honest tells
  ("coughs on cold start" ← `FuelUnits` low / partial generation ×0.5 state).
- **Water pump, greenhouse equipment**: narrative-only characterization until an owner
  system gains condition state.
