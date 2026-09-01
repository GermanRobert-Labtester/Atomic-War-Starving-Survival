# Orbital Harrow Event Matrix

> **Authority:** `Assets/StreamingAssets/Data/orbital_harrow_events.json`, `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`

---

## 1. Kinetic Strike Event Templates

| Event ID | Event Name | Severity | Energy (MJ) | Warning (Days) | Cell Spread | Salvage Item Yield | Revealed Site |
|---|---|---|---|---|---|---|---|
| `event_orbital_small_debris_shower` | Decaying Shrapnel Scatter | Minor | 8.0 | 3 | 3 | `scrap_mechanical` (4x) | None |
| `event_orbital_heavy_kinetic_impact` | Tungsten Penetrator Plunge | Severe | 35.0 | 2 | 1 | `scrap_electronic` (6x) | `loc_excavation_command_vault` |
| `event_orbital_clustered_impact` | Telemetry Station Cluster Strike | Moderate | 22.0 | 4 | 4 | `copper_wire` (5x) | None |
| `event_orbital_near_miss_shockwave` | Sub-Orbital Airburst Shockwave | Minor | 12.0 | 3 | 2 | `fuel` (3x) | None |
| `event_orbital_low_warning_strike` | Rapid-Decay High-Density Core | Severe | 40.0 | 1 | 2 | `heavy_industrial_motor` (1x) | `loc_excavation_mine_shaft` |

---

## 2. Event Lifecycle

```text
Orbital Decay Detected (Telemetry Window)
       ↓
Impact Warning Dispatched (Lead Days: 1-4)
       ↓
Player Bracing & Reinforcement Window (Ceiling Plating / Power Shunting)
       ↓
Strike Resolution on Impact Day
       ↓
Sky Armor Absorption vs Breach Calculation
       ↓
Cascading Shelter Effects (Power Grid Disruption / Structural Wear)
       ↓
Aftermath: Salvage Opportunity Created & Optional Map Site Revealed
```
