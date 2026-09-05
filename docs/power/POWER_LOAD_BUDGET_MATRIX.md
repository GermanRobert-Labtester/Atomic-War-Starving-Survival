# Power Load Budget Matrix

> **Budget & Balance:** Analysis of generation, battery endurance, and power draw across progression stages.

---

## 1. Electrical Breakdown by Priority Band

| Priority Band | Rooms in Band | Total Load (W) | Share of Nominal Demand |
|---|---|---|---|
| **Critical** (6 rooms) | `room_air_filtration` (180W), `room_clinic` (120W), `room_water_pump` (100W), `room_water_treatment` (180W), `room_airlock` (110W), `room_ward_quarantine` (70W) | **760 W** | 34.1% |
| **Standard** (8 rooms) | `room_greenhouse` (160W), `room_workshop` (200W), `room_kitchen` (120W), `room_radio_tuner` (100W), `room_laboratory_research` (300W), `room_armory_munitions` (50W), `room_storage_secure` (80W), `room_surveillance` (90W) | **1,100 W** | 49.3% |
| **Low** (4 rooms) | `room_foundry` (220W), `room_lighting_main` (80W), `room_common_mess_hall` (40W), `room_bunks` (30W) | **370 W** | 16.6% |
| **Total Grid** (18 rooms) | All 18 rooms simultaneously active | **2,230 W** | 100.0% |

---

## 2. Supply vs. Demand Dynamics

- **Baseline Dynamo Output:** 800 W.
- **Critical Survival Envelope:** 760 W.
- **Safety Margin on Critical Core:** +40 W surplus under baseline generation.
- **Full Development Deficit:** -1,430 W if every room is powered without generation upgrades or load management.

---

## 3. Battery Endurance Scenarios (4,000 Wh Capacity)

| State | Generation Available | Active Load | Net Balance | Battery Runtime |
|---|---|---|---|---|
| **Full Emergency Blackout** | 0 W | Critical Core only (760 W) | -760 W | **5.26 hours** |
| **Minimal Life Support** | 0 W | Filtration (180W) + Water Pump (100W) + Clinic (120W) = 400 W | -400 W | **10.0 hours** |
| **Standard Day Draw (Mid-Shelter)** | 800 W | Critical Core (760W) + Workshop (200W) + Kitchen (120W) = 1,080 W | -280 W | **14.28 hours** |
| **Fuel-Starved Dynamo (50% Output)** | 400 W | Critical Core (760 W) | -360 W | **11.11 hours** |
| **Full Load Overload** | 800 W | All 18 Rooms (2,230 W) | -1,430 W | **2.80 hours** |

---

## 4. Stage Viability

1. **Early Game (Days 1–20):** Only ~4–6 rooms active (Filtration, Water Pump, Bunks, Clinic, Kitchen). Total draw ~450–600 W. Generator provides comfortable surplus; battery charges to 100%.
2. **Mid Game (Days 21–60):** Workshop, Greenhouse, Radio Tuner, and Secure Storage online. Total draw ~1,100–1,400 W. Player must begin load scheduling (e.g., run workshop by day, turn off non-essential lighting or foundry).
3. **Late Game (Days 61+):** Science Lab, Water Treatment Plant, Airlock, Quarantine, Surveillance, and Foundry all built. Grid requires dedicated generator upgrades, fuel logistics, and active circuit priority management.
