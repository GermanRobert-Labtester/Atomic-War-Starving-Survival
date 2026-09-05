# Plan 71 Balance Report

> **Simulation & Balance:** Mathematical analysis of electrical load budgets across early, mid, and late shelter progression.

---

## 1. Electrical Capacity Baseline

- **Primary Diesel Dynamo:** 800 W default output.
- **Daily Fuel Consumption:** `800 W * 24 h * 0.001 = 19.2 units/day`.
- **Default Fuel Supply:** 100 units = **5.2 days of continuous unthrottled generation**.
- **Accumulator Battery Bank:** 4,000 Wh capacity (can buffer 400 W deficit for 10 hours).

---

## 2. Progression Stage Simulations

### Stage 1: Initial Holdfast Shelter (Days 1–20)
- **Active Rooms (5 rooms):**
  - Air Filtration: 180 W (Critical)
  - Water Pump: 100 W (Critical)
  - Clinic: 120 W (Critical)
  - Standard Dormitory: 30 W (Low)
  - Galley Kitchen: 120 W (Standard)
- **Total Draw:** **550 W**.
- **Net Power:** +250 W surplus.
- **Battery State:** Maintained at 4,000 Wh (100%).
- **Result:** Early shelter operates smoothly with a positive power buffer, giving the player room to scavenge fuel without immediate blackout panic.

### Stage 2: Expanding Holdfast Shelter (Days 21–60)
- **Active Rooms (10 rooms):**
  - Stage 1 rooms (550 W)
  - General Workshop: 200 W (Standard)
  - Greenhouse: 160 W (Standard)
  - Radio Communications Bay: 100 W (Standard)
  - Reinforced Storage Vault: 80 W (Standard)
  - Main Lighting: 80 W (Low)
- **Total Draw:** **1,170 W**.
- **Net Power:** -370 W deficit if all run continuously.
- **Battery Discharge Rate:** 370 W deficit = **10.8 hours before brownout**.
- **Player Tactical Choice:** The player cannot leave all circuits powered 24/7 on a single 800 W dynamo without upgrades or management. The player must either:
  - Open the breaker for the Workshop (200 W) and Main Lighting (80 W) during the night, bringing draw down to 890 W (easily buffered by battery).
  - Secure fuel shipments and scavenge auxiliary dynamos.
- **Result:** Shelter expansion creates meaningful operational tradeoffs.

### Stage 3: Full Late-Game Shelter (Days 61+)
- **Active Rooms (All 18 rooms):**
  - Critical Core (6 rooms): 760 W
  - Standard Operations (8 rooms): 1,100 W
  - Low Priority (4 rooms): 370 W
- **Total Draw:** **2,230 W**.
- **Net Power on Baseline Dynamo:** -1,430 W deficit.
- **Result:** Requires generator tier upgrades, auxiliary dynamos, or selective emergency load shedding. The critical life-support core (760 W) can always be sustained on a standard 800 W dynamo, proving survival viability while making full luxury/industrial operation an earned late-game achievement.

---

## 3. Dominant-Priority Avoidance

- No single priority layout is universally dominant:
  - Setting everything to `Critical` creates an unmanageable 760 W core that starves production.
  - Setting too many rooms to `Low` causes rapid cascading outages and work halts.
  - Standard priority allows flexible player toggling between industrial output (Workshop, Foundry) and scientific discovery (Laboratory, Radio).
