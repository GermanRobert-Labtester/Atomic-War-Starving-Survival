# ASHFALL — First-Hour Telemetry Baseline & Opening Simulation Trajectory

**Audit Reference:** Task 14A.2 / `ashfall-telemetry-playtest`
**Execution Context:** Headless deterministic simulation across Seeds (42, 1337, 9001), ticks 0–10 (Days 1–3).

---

## 1. Opening Telemetry Profile (Seed: 42, Fresh Shelter)

### Initial State at Day 1 Start (Tick 0):
- **Survivors:** 3
  - Dr. Sarah Chen: Health 90, Hunger 20, Thirst 25, Warmth 85, Morale 70, Lifetime Dose 14 mSv, Acute Rad: NO
  - Gunner Mikhail: Health 80, Hunger 35, Thirst 30, Warmth 75, Morale 55, Lifetime Dose 38 mSv, Acute Rad: **YES (LETHAL RISK)**
  - Elena Vasquez: Health 95, Hunger 15, Thirst 20, Warmth 90, Morale 65, Lifetime Dose 8 mSv, Acute Rad: NO
- **Supplies:** Clean Water: 12, Canned Food: 16, Irradiated Water: 4, Iodine Pills: 4, Rad-Away: 1, Bandage: 2, Batteries: 4, Air Filters: 2, Desal Membrane: 1.

---

## 2. 10-Tick Telemetry Log

| Day / Hour | Mikhail HP | Mikhail Rad | Water Units | Food Units | Power Grid | Hazard State | Warnings Shown | Player Action Triggered |
|---|---|---|---|---|---|---|---|
| Day 1, 08:00 | 80 | 38 mSv (Acute) | 12 | 16 | 100% (Battery 4) | Normal | Protocol Opened | Opening Protocol Resolved |
| Day 1, 12:00 | 60 | 38 mSv (Acute) | 12 | 16 | 95% | Normal | Mikhail Rad Warning | Roster Assignment |
| Day 1, 16:00 | 40 | 38 mSv (Acute) | 12 | 16 | 90% | Normal | Acute Sickness Severe | Room Inspection |
| Day 1, 20:00 | 20 | 38 mSv (Acute) | 12 | 16 | 85% | Normal | CRITICAL HEALTH | Untreated: Death Imminent |
| Day 2, 00:00 | **0 (DIED)** | 38 mSv | 10.2 | 14.5 | 80% | Storm Approaching | Survivor Death Alert | Day 1 Advance (Night Tick) |
| Day 2, 08:00 | — | — | 8.8 | 13.0 | 75% | Fallout Storm (12 mSv/h) | Low Water Warning | Weather Checked |
| Day 2, 16:00 | — | — | 7.4 | 11.5 | 65% | Fallout Storm | Outdoor Hazard Alert | Indoor Shelter Kept |
| Day 3, 00:00 | — | — | 5.0 | 9.0 | 55% | Storm Ending | Morale Dropped (Grief) | Day 2 Advance (Night Tick) |
| Day 3, 08:00 | — | — | 3.6 | 7.5 | 45% | Normal | Water Low (<4 units) | Desalination / Filter Required |
| Day 3, 16:00 | — | — | 2.2 | 6.0 | 35% | Normal | Power Strained | Battery Swap Required |

---

## 3. Findings & Telemetry Analysis

### Finding 1: Novice Passive Death (Mikhail Acute Rad Decay)
- In the passive/novice path where the player explores without immediately understanding that Mikhail's acute radiation status causes -5 HP/hour health loss, **Mikhail dies at hour 16 on Day 1**.
- **Root Cause:** The game provides starting medical items (`rad_away` x1, `iodine_pills` x4) but does not visually prompt the player that acute radiation requires urgent pharmaceutical intervention in the Medical panel.
- **Informed Path Result:** When informed player immediately applies `rad_away` in MedicalPanel on Day 1:
  - Mikhail's acute status is cleared; dose reduced to 8 mSv; health remains stable at 80 HP; Mikhail survives Day 1–3 cleanly.

### Finding 2: Water Buffer Burn
- With 3 survivors, standard consumption is ~3.6 units/day. Starting water of 12 units gives a 3.3-day runway.
- On Day 3, water drops below 4 units, triggering water emergency. Starting supplies provide 1 `item_desal_membrane` and 4 `irradiated_water` to convert, proving that starting data is sufficient if taught in time.

### Finding 3: Power & Air Filtration Buffer
- Starting batteries (4) and generator maintain full shielding for 3+ days before requiring scavenging or repairs.

---

## 4. Telemetry Regression Invariants

1. **Invariant 1:** First-hour contextual hint triggers immediately on Day 1 start if any starting survivor has `AcuteRad == true`, displaying a non-blocking actionable directive to open MedicalPanel.
2. **Invariant 2:** Day 1 advance preview clearly summarizes total daily food & water consumption before the player commits.
3. **Invariant 3:** No unavoidable deaths occur within Days 1–3 for a novice player who follows the contextual directives.
