# Orbital Harrow Telemetry Runtime Contract

## 1. System Overview

- **Core File**: `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`
- **Catalog File**: `Assets/StreamingAssets/Data/orbital_harrow_events.json`
- **Catalog Loader**: `Assets/Ashfall.Core/Shelter/OrbitalHarrowCatalog.cs` (`OrbitalHarrowCatalogLoader`)

## 2. Event Grammar & Fields

| Field | Type | Description |
|---|---|---|
| `id` | `string` | Unique snake_case event ID (`event_orbital_*`) |
| `name` | `string` | Technical / display title for warning logs |
| `description` | `string` | Atmospheric narrative description of the event |
| `severity` | `string` | "Minor", "Moderate", "Severe", "Catastrophic" |
| `signal_type` | `string` | `radar_anomaly`, `thermal_signature`, `seismic_precursor`, `radio_interference`, `dead_hand_ping` |
| `is_false_positive` | `bool` | If true, resolves at 0 MJ without physical strike |
| `impact_energy_mj` | `float` | Strike energy in MegaJoules (0.0 for false alarms) |
| `lead_time_days` | `int` | Countdown warning duration in days (1–5) |
| `affected_cell_spread` | `int` | Number of contiguous roof grid cells impacted |
| `penetration_power_mj` | `float` | Concentrated kinetic penetrator rating |
| `salvage_yield_item_id` | `string` | Post-impact salvage item ID |
| `salvage_yield_quantity` | `int` | Quantity of salvage spawned |
| `revealed_site_id` | `string` | Location ID of revealed deep-strata excavation site |
| `radio_hook_text` | `string` | Pre-warning or concurrent radio intercept transmission |

## 3. Telemetry State & Lifecycle

1. **Activation**: `ActivateTelemetry(day)` enables tracking.
2. **Scheduling**: `ScheduleEventDef(def, day, gridX)` records target, energy, spread, and raises `OnImpactWarning`.
3. **Player Counter-Play**: `Brace(materialId, amount)` locks in a 50% kinetic mitigation modifier for the pending strike.
4. **Daily Tick**: `TickDay(day)` decrements days and triggers `ResolveImpact()` on `nextImpactDay`.
5. **Resolution**: Delegates impact to `SkyLayerArmorSystem.EvaluateKineticImpact()`, computes cascading electrical disruption, spawns 7-day salvage opportunity, unlocks excavation sites, and fires `OnImpactDetailed(report)`.
6. **Salvage Claim**: `ClaimSalvage(eventId)` allows survivors to collect materials from the strike crater.
7. **Save / Restore**: `CaptureState()` and `RestoreState()` fully persist pending impacts, active warnings, claimed salvage, and revealed sites.
