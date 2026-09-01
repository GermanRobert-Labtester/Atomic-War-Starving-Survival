# Plan 39 Harrow Telemetry QA Matrix

## 1. Authored 12-Event QA Matrix

| Event ID | Signal | Window | Energy | False Pos | Radio Precursor | Salvage Yield | Revealed Excavation Site |
|---|---|---:|---:|:---:|:---:|---|---|
| `event_orbital_kinetic_early_track` | `radar_anomaly` | 5d | 22 MJ | No | No | `scrap_mechanical` (4) | `loc_excavation_utility_tunnels` |
| `event_orbital_kinetic_thermal_descent` | `thermal_signature` | 2d | 45 MJ | No | Yes | `scrap_electronic` (6) | `loc_excavation_command_vault` |
| `event_orbital_kinetic_seismic_precursor` | `seismic_precursor` | 1d | 38 MJ | No | No | `heavy_industrial_motor` (1) | `loc_excavation_mine_shaft` |
| `event_orbital_kinetic_fragmented_track` | `radar_anomaly` | 3d | 28 MJ | No | No | `scrap_mechanical` (5) | `loc_excavation_storage_chamber` |
| `event_orbital_cluster_multiple_returns` | `radar_anomaly` | 4d | 24 MJ | No | No | `copper_wire_10m_of_10m` (3) | `loc_excavation_metro_interchange` |
| `event_orbital_cluster_split_track` | `thermal_signature` | 3d | 36 MJ | No | No | `mechanical_parts` (5) | `loc_excavation_drainage_network` |
| `event_orbital_emp_radio_blackout` | `radio_interference` | 3d | 14 MJ | No | Yes | `battery` (4) | `loc_excavation_civilian_shelter` |
| `event_orbital_emp_signature_mismatch` | `thermal_signature` | 2d | 28 MJ | No | No | `fuel` (4) | `loc_excavation_archive_bunker` |
| `event_orbital_dead_hand_repeating_ping` | `dead_hand_ping` | 4d | 20 MJ | No | Yes | `scrap_electronic` (5) | `loc_excavation_command_vault` |
| `event_orbital_dead_hand_broken_checksum` | `dead_hand_ping` | 3d | 32 MJ | No | Yes | `scrap_electronic` (6) | `loc_excavation_archive_bunker` |
| `event_orbital_radar_ducting_false_alarm` | `radar_anomaly` | 3d | 0 MJ | Yes | No | `scrap_mechanical` (1) | None |
| `event_orbital_debris_misclassification` | `radar_anomaly` | 2d | 0 MJ | Yes | No | `scrap_metal` (1) | None |

## 2. Consequence Mapping (8 Concepts)

| # | Consequence Concept | Trigger Condition | Downstream System Impact |
|---|---|---|---|
| 1 | **Armor Hold** | `impactEnergy <= absorptionThreshold` | Zero roof damage, slight cell durability loss, 0% power disruption |
| 2 | **Armor Breach** | `impactEnergy > absorptionThreshold` | Penetration damage to roof, -50 cell durability, power grid disruption |
| 3 | **Pre-Impact Bracing** | `Brace(material, qty)` | Halves incoming strike energy (0.5x multiplier) |
| 4 | **No Protection** | Unarmored grid cell | `damage = impactEnergy * 10f`, guaranteed breach |
| 5 | **Power Grid Disruption** | Breached kinetic strike | `powerDisruption = min(100, damage * 2.5f)` |
| 6 | **Salvage Generation** | Impact resolution | 7-day timed salvage opportunity at target grid coordinate |
| 7 | **Site Discovery** | Impact resolution with site ID | Adds location to `RevealedSites` and unlocks excavation tier |
| 8 | **Harmless False Alarm** | `is_false_positive == true` (0 MJ) | 0 roof damage, 0 cell degradation, 0 power disruption, small cosmetic scrap |
