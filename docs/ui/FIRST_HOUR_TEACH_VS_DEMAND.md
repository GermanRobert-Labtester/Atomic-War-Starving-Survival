# ASHFALL — First-Hour Teach-vs-Demand Evidence Matrix

**Audit Reference:** Task 14A.1 / `ashfall-tutorial-review`
**System Targets:** NeedsSystem, RadiationSystem, PowerGrid, WaterTreatment, DutyRoster, Inventory/Rations, Weather/Storms, Expeditions, DailyTick.

---

## 1. Teach-vs-Demand Matrix

| System | First Meaningful Demand | First Visible Warning | First Explicit Teaching | Player Action Required | Failure if Missed | Gap Classification & Verdict |
|---|---:|---:|---:|---|---|---|
| **Radiation / Dosimeter** | Day 1 (Mikhail has acute radiation at start: 38 mSv, acute=true) | Day 1 (HUD dose indicator / Medical panel) | Day 1 (Step 5 in TutorialPanel text, but no interactive guidance on rad_away/iodine) | Administer Rad-Away / Iodine in Medical panel; check dosimeter | Survivor Mikhail takes 5 HP/hr health loss; death in ~16 hours without treatment | **P0 GAP (UNTAUGHT_LETHAL)**: Starting survivor Mikhail has acute radiation; novice player is not warned of acute decay rate or guided to treat with Rad-Away. |
| **Ration Policy (Food & Water)** | Day 1–2 (Thirst decay 1.2/hr = 28.8/day; 12 clean water starting) | Day 2 (Water below 50%) | Day 1 (Inventory inspection step, but ration policy mechanics not explained) | Adjust ration policy or run water filter / desalination | Thirst hits 90+ critical on Day 2; health damage 0.6/hr cascade | **P1 GAP (TAUGHT_LATE)**: Opening stores shows inventory counts, but does not explain that ration policy controls daily depletion rate. |
| **Power / Grid & Water Triage** | Day 2–3 (Generator fuel/battery consumption, filter maintenance) | Day 2 (Generator fuel low / battery reserve) | Day 1 (Brief mention in TutorialPanel tips) | Fuel generator or swap batteries; maintain air filter & water membrane | Brownout stops filtration; indoor radiation increases; water production halts | **P1 GAP (OVERLOOKED_TRIAGE)**: Power grid and filtration dependencies are hidden until emergency brownout occurs. |
| **Duty Roster & Work Shifts** | Day 1 (Shift assignments required for production & maintenance) | Day 1 (Roster vacant warning) | Day 1 (Onboarding Stage 4: Assign duty) | Assign survivors to Kitchen/Water/Guard/Scavenge | No shelter maintenance; morale penalty; wasted survivor hours | **RESOLVED**: Stage 4 successfully guides survivor assignment to duty roster. |
| **Weather & Fallout Storms** | Day 2–3 (Atmospheric storm with elevated zone rad levels) | Day 1–2 (Weather forecast tab) | Day 1 (Onboarding Stage 5: Read weather) | Keep survivors sheltered; equip gas mask/hazmat if outdoor | Unprotected survivors receive heavy environmental dose (>50 mSv) | **WELL_TAUGHT**: Weather reading is part of the core onboarding loop. |
| **Daily Tick Resolution** | Day 1 End (Advances all subsystems, evaluates needs, applies decay) | Day 1 (Confirmation modal before tick) | Day 1 (Onboarding Stage 7: End Day 1) | Inspect pending critical needs before confirming day advance | Cascade of unaddressed sickness/starvation overnight | **WELL_TAUGHT**: Briefing modal presents clear day advancement summary. |
| **Expedition Preparation** | Day 3–5 (Scavenging for scrap, fuel, medical supplies) | Day 3 (Resource depletion prompts) | TutorialPanel tip only | Check vehicle/foot speed, loadout, gas mask, fuel, rations | Scavenger dies of radiation/exposure or vehicle breaks down | **P2 GAP (POLISH)**: Expedition readiness indicators needed before first departure. |

---

## 2. Ranked Onboarding Gaps to Close

1. **Gap 1: Radiation & Acute Sickness Intervention (P0)**
   - **Trigger:** Game start (when Mikhail's status is loaded) or first dose spike.
   - **Fix:** Contextual hint pointing directly to Medical Panel with actionable guidance: "Survivor Mikhail suffers from Acute Radiation Sickness (38 mSv). Administer Rad-Away or Iodine to arrest 5 HP/hr decay."
   - **Codex Link:** Field Manual -> Radiation & Medical Triage.

2. **Gap 2: Ration Policy & Water Depletion Causality (P1)**
   - **Trigger:** Opening inventory during Stage 3 / first water usage.
   - **Fix:** Explain consumption math in the inventory/rations summary: "3 survivors consume 3.6 water/day at standard rations. Reserve: 12 units (~3.3 days)."
   - **Codex Link:** Field Manual -> Inventory & Rationing.

3. **Gap 3: Power Grid & Shelter Filtration Anticipation (P1)**
   - **Trigger:** Power reserve dropping below 50% or air filter degradation.
   - **Fix:** Contextual alert: "Power grid operates air/water filtration. Maintain generator fuel to prevent shelter contamination."
   - **Codex Link:** Field Manual -> Power & Infrastructure.
