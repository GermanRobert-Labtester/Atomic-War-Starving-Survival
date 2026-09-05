# Power Incident Integration

> **Integration Contract:** Wiring between Plan 57 incidents (`incidents.json`) and the shelter electrical power grid.

---

## 1. Verified Incident Seams (6 Incident Integrations)

| # | Incident ID | Target Power System / Room | Mechanism | Activation Condition | Resolution Path |
|---|---|---|---|---|---|
| 1 | `incident_generator_failure` | Primary Diesel Dynamo | **Generation Reduction** | MinDay 22; dynamo drops under load; generation halved or zeroed | Mechanics repair dynamo; generation restored |
| 2 | `incident_air_filter_breakdown` | `room_air_filtration` | **Circuit Overload / Trip** | MinDay 40; filter vibration triggers breaker trip | Replace clogged filter element; reset breaker |
| 3 | `incident_water_pipe_burst` | `room_water_pump` | **Service Disablement** | MinDay 5; pipe joint burst floods storage / shorts pump motor | Repair joint; pump motor restarted |
| 4 | `incident_radiation_spike` | `room_air_filtration` | **Demand / Stress Spike** | MinDay 20; radioactive ash cloud forces filtration to maximum draw | Ash front clears; blower load returns to normal |
| 5 | `incident_water_contamination` | `room_water_treatment` | **Purification Overload** | MinDay 15; toxic silt breaches intake, requiring continuous chemical treatment | Purifier filters flushed and chemical reagents replenished |
| 6 | `incident_radio_interference` | `room_radio_tuner` | **Signal Jamming / Preamp Short** | MinDay 8; atmospheric EM pulse bursts across receiver | Antenna lead grounded and receiver tubes realigned |

---

## 2. Structural Authority Invariants

1. **Incidents Own Narrative & Triggers:** The incident system decides when an incident fires, tracks repair tasks, and logs incident records.
2. **Power Grid Owns Electrical Response:** When an incident modifies generation capacity or forces a breaker open, `PowerGridSystem` calculates the resulting load balance and brownout state.
3. **No Duplicate State:** Incident status flags are never stored inside `power_grid.json` or `PowerGridState`.
