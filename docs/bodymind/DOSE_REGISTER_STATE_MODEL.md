# Dose Register State Model & Institutional Architecture

## 1. Overview & Institutional Identity

The Dose Register is not a hospital and not a radiation physics simulator. It is the administrative institution that keeps the count after catastrophe.

It operates four distinct ledgers:
1. **The Dose Ledger (`register_ledger`):** Maintained by Dr. Irina Vel. Books cumulative readings against assigned dosimeter tags.
2. **The Sick List (`register_sick`):** Maintained by Sister Wyn Omah. Tracks palliative care plans, comfort rounds, and bed allocations.
3. **The Cohort Board (`register_cohort`):** Maintained by Midwife Saria Voss. Tracks children's baselines in erasable chalk.
4. **The Voluntary Register (`register_voluntary`):** Manages signatures for hazardous, high-exposure shifts and emergency repairs.

---

## 2. Band Definitions & Semantics

| Band ID | Label | Threshold (mSv) | Administrative Disposition | Institutional Effect |
| :--- | :--- | :--- | :--- | :--- |
| `band_green` | Green | 0 | No measurable burden. Walk the corridor. | Unrestricted access; eligible for all shifts and surface expeditions. |
| `band_amber` | Amber | 100 | The ledger shows a number worth watching. | Advisory caution; recommended shift rotation; eligible for regular duties. |
| `band_red` | Red | 300 | Named on the sick list. Care is a choice, not a cure. | Restricted from high-radiation reactor/crater shifts; priority for clean-room beds and comfort rounds. |
| `band_black` | Black | 600 | The band the registrar will not soften. Still on the roster. | Palliative focus; restricted from further exposure shifts unless authorized by emergency leadership override. |

---

## 3. Physical Dose vs. Administrative Record

```
[ Physical Radiation Hazard ] ---> [ RadiationSystem: SurvivorRadState ] (Biological Truth)
                                               |
                                     (Measurement / Guess)
                                               v
[ Dosimeter / Geiger / Tag ] ----> [ DoseLedgerSystem: DoseEntry ]       (Administrative Record)
                                               ^
                                               | (Forgery / Chit / Error)
                                   [ Administrative Classification ]
```

- **Nominal vs Booked:** When a high-energy event occurs, the dial shows nominal mSv, but shielding and anti-rad reduce what reaches the body and what is booked.
- **Calibration Drift:** Piet Abar tracks drift. Overdue calibration introduces systematic uncertainty until calibrated at the bench.
- **Administrative Override / Clean Bill:** If a survivor holds a forged clean-bill chit, the Dose Register accepts their Green-band classification for entry checks, while their physical `RadiationSystem` state continues to accumulate acute/lifetime burden.
