# Foundry Accord Resource Allocation Matrix

**Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`

---

## 1. Resource Allocations Across All 12 Accords

| Treaty ID | Water (lpm) | Power (kW) | Operational Meaning & Physical Justification |
|---|---:|---:|---|
| `treaty_brine_pipe_and_iodine_exchange` | 40.0 | 12.0 | Industrial steam loop and cooling brine flow for lead-pipe casting runs. |
| `treaty_cluster_labour_schedule` | 25.0 | 8.0 | Stoker potable water ration (0.5L boiled/shift) and auxiliary ventilation power. |
| `treaty_road_iron_charter` | 15.0 | 6.0 | Ice-road waypoint hydration and winch motor charging. |
| `treaty_the_cluster_charter` | 0.0 | 0.0 | Constitution signature only; utility flows governed by underlying operational accords. |
| `treaty_garrison_grain_tithe_compact` | 50.0 | 15.0 | Verge irrigation ditch diversion and Checkpoint Gamma perimeter floodlights. |
| `treaty_flotilla_saline_corridor_concordat` | 20.0 | 10.0 | Lock Gate Four sluice hydraulic actuators and wash basin replenishment. |
| `treaty_switchback_fuel_and_passage_accord` | 10.0 | 5.0 | Snowline patrol station potable supply and signal lantern charging batteries. |
| `treaty_scale_suburban_fair_trade_convention` | 30.0 | 8.0 | Caravanserai livestock trough supply and market hall illumination. |
| `treaty_scrap_salvage_demarcation` | 15.0 | 18.0 | Cutting torch oxygen compressors and heavy crane winch power at the Recovery Yard. |
| `treaty_roster_border_demilitarization_pact` | 0.0 | 0.0 | Demilitarized buffer pact; utilities explicitly barred to prevent fortification. |
| `treaty_deep_coast_aquifer_protection_treaty` | 60.0 | 14.0 | Desalination intake volume and pump station sediment centrifuge power. |
| `treaty_high_scarp_observatory_sanctuary` | 5.0 | 20.0 | High-altitude cistern reserve and transmission antenna amplifier array. |

---

## 2. Invariants Observed

- **Zero-Value Truthfulness:** Treaties that represent political or constitutional recognition (`the_cluster_charter`, `border_demilitarization`) carry explicit `0.0` allocations rather than fabricated transfers.
- **Physical Plausibility:** Industrial and municipal allocations remain within realistic small-scale post-collapse ranges ($5.0$ to $60.0$ lpm; $5.0$ to $20.0$ kW).
- **Zero Inversion:** All values are non-negative ($x \ge 0.0$).
