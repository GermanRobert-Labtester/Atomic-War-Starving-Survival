# Dose Location Rate Contract

> **Definition:** Runtime unit semantics, time-base scaling, and conversion boundaries for `radiationUsv`.

---

## 1. Unit Semantics

- **Authored Field:** `radiationUsv` (float) in `dose_locations.json`.
- **Physical Unit:** Microsieverts per hour (**µSv/h**).
- **Core Ledger Stored Unit:** Millisieverts (**mSv**) in `DoseLedgerSystem` (`nominalMsv`, `bookedMsv`, `cumulativeMsv`).
- **Mathematical Conversion Factor:**
  $$\text{mSv} = \frac{\mu\text{Sv}}{1000}$$

---

## 2. Real-World Scaling & Game Grounding

| Condition | Rate (µSv/h) | Rate (mSv/h) | Time to Amber (100 mSv) | Time to Red (300 mSv) |
|---|---|---|---|---|
| Deep Shelter Living Room | 0.02 | 0.00002 | 5,000,000 h (~570 yrs) | 15,000,000 h |
| Surface Apron (`loc_shelter_exterior_approach`) | 0.85 | 0.00085 | 117,647 h (~13 yrs) | 352,941 h |
| Surface Water Intake (`loc_contaminated_water_access`) | 3.50 | 0.00350 | 28,571 h (~3.2 yrs) | 85,714 h |
| Ridge Traverse (`loc_burned_woodland_ridge`) | 8.40 | 0.00840 | 11,904 h (~1.3 yrs) | 35,714 h |
| Irradiated Forest (`loc_irradiated_forest_edge`) | 18.50 | 0.01850 | 5,405 h (~225 days) | 16,216 h |
| Ruined Hospital (`loc_ruined_hospital_grounds`) | 28.00 | 0.02800 | 3,571 h (~148 days) | 10,714 h |
| Military Depot (`loc_military_depot_perimeter`) | 45.00 | 0.04500 | 2,222 h (~92 days) | 6,666 h |

---

## 3. Acute vs. Chronic Balance

- **Chronic Background:** Ordinary exploration does not immediately hospitalize a survivor with Acute Radiation Syndrome (ARS). Instead, it gradually fills their dosimeter tag history over weeks and months of sorties.
- **Acute Modifiers:** Weather events (fallout dust storms) or high-energy fallout flares apply multiplicative spikes (2× to 10×) on top of this location baseline, making hot zones during a storm acutely lethal.
