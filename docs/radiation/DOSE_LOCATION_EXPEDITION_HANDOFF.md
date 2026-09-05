# Dose Location Expedition Handoff

> **Integration:** Cross-system reference and handoff rules connecting `dose_locations.json` to `expeditions.json` and expedition destination tracking.

---

## 1. Expedition Identity Alignment

| Dose Location ID | Sector | Display Name | Corresponding Expedition ID | Expedition File | Context / Rationale |
|---|---|---|---|---|---|
| `loc_irradiated_forest_edge` | `expedition` | Irradiated Forest Edge | `loc_irradiated_forest_edge` | Staged Destination | High-risk natural fallout accumulation perimeter on wasteland approaches. |
| `loc_ruined_hospital_grounds` | `expedition` | Ruined Hospital Grounds | `abandoned_hospital` | `expeditions.json` (line 470) | Represents the exterior contaminated perimeter of the canonical Abandoned Hospital facility. |
| `loc_military_depot_perimeter` | `expedition` | Military Depot Perimeter | `loc_ordnance_shoulder` | `expeditions.json` (line 590) | The heavy-ordnance depot exterior with pulverized military hardstands and craters. |

---

## 2. Expedition Exposure Calculation

1. **Travel vs. Destination Phase:**
   - Travel ticks through external corridors accumulate exposure using the route's external sector baseline (e.g. `loc_frozen_wetland_crossing` or `loc_burned_woodland_ridge`).
   - On arrival at destination, the party enters the destination's dose location context for the duration of the scavenging or reconnaissance phase.
2. **Dwell Duration:**
   - Standard expedition search dwell ranges from 2.0 to 6.0 hours.
   - At `loc_ruined_hospital_grounds` (28.0 µSv/h), a 4-hour search generates:
     $$28.0 \times 4 = 112.0\,\mu\text{Sv} = 0.112\,\text{mSv}$$
   - At `loc_military_depot_perimeter` (45.0 µSv/h), a 4-hour search generates:
     $$45.0 \times 4 = 180.0\,\mu\text{Sv} = 0.180\,\text{mSv}$$
3. **No Double Counting:** The destination dose location is active ONLY during the dwell/search phase; route dose is active ONLY during travel ticks.
