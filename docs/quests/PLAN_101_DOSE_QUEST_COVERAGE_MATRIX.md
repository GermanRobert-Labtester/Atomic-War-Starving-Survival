# Plan 101 — Dose Quest Coverage Matrix

**Document ID:** `docs/quests/PLAN_101_DOSE_QUEST_COVERAGE_MATRIX.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/dose_quests.json`

---

## 1. Complete Twelve-Quest Coverage

| Questline ID | Title | Day Window | Core Dilemma | Stages | Choice Pattern | Morale / Guilt Shape | Material Reward |
|:---|:---|:---:|:---|:---:|:---|:---:|:---|
| `quest_the_dose_the_first_reading` | The First Reading | 40–360 | Decide whether shelter begins keeping a dose ledger or closes the book. | 3 | Open ledger vs Close book | (-1, 0) vs (+1, +2) | `item_dose_ledger` x1 |
| `quest_the_falsified_reading` | The Falsified Reading | 60–360 | Discrepancy between field survey and ledger: correct truth vs protect worker clearance. | 3 | Red line correction vs Conceal for mercy | (-2, 0) vs (+1, +2) | `item_forged_clean_bill_chit` x1 |
| `quest_the_stolen_dosimeter` | The Stolen Dosimeter | 80–360 | Calibrated quartz meter stolen before boiler fix: recover meter vs issue drifting spare. | 3 | Confront technician vs Issue spare meter | (-1, 0) vs (0, +1) | `item_calibrated_dosimeter` x1 / `item_dosimeter_tag` x1 |
| `quest_the_sick_of_room_seven` | The Sick of Room Seven | 90–360 | Two Red-band survivors, one morphine tray: triage and bed order. | 4 | Split care vs Hide one vs Draw volunteer shift | (-2, 0) vs (+1, +3) vs (0, +1) | `item_palliative_morphine` x1 |
| `quest_child_over_the_limit` | Child Over the Limit | 110–360 | Adolescent crosses into Amber band: reassign to library vs issue lathe waiver. | 3 | Enforce Amber bar vs Issue workshop waiver | (-1, 0) vs (+1, +2) | `item_shielded_badge_case` x1 |
| `quest_the_register_audit` | The Register Audit | 130–360 | Master bench drifted 15%: retroactively recalculate all pages vs fix forward only. | 3 | Retest & recalculate vs Quarantine drift quietly | (-3, 0) vs (+1, +2) | `item_calibration_key` x1 |
| `quest_the_childs_number` | The Child's Number | 150–360 | Newborn baseline recorded in chalk: book low vs book honest vs refuse to book. | 4 | Book low vs Book honest vs Refuse book | (-1, +1) vs (-2, 0) vs (0, +2) | `item_cohort_first_board` x1 |
| `quest_black_market_clean_bill` | Black-Market Clean Bill | 160–360 | Counterfeit Green-band clearance chits circulating at screening checkpoint. | 3 | Shut down ring vs Confiscate for leverage | (-1, 0) vs (0, +2) | `item_forged_clean_bill_chit` x2 |
| `quest_the_broken_calibration_chain` | The Broken Calibration Chain | 180–360 | Cesium reference source crystal fractured: rebuild chamber vs use drift math. | 3 | Rebuild standard vs Drift approximations | (0, 0) vs (-1, +1) | `item_calibrated_dosimeter` x1 |
| `quest_the_signed_hour` | The Signed Hour | 200–360 | Volunteer signs for reactor corridor: send now and incur dose vs wait for weather. | 3 | Send now vs Wait window | (-1, +2) vs (+1, 0) | None |
| `quest_exposure_for_the_essential_worker` | Exposure for Essential Worker | 210–360 | Chief engineer in Red band hours before turbine failure: override vs rolling cuts. | 3 | Authorize shift vs Rolling blackouts | (-2, +3) vs (-1, 0) | `item_chelation_decorporation_course` x1 |
| `quest_the_missing_page` | The Missing Page | 230–360 | Sheet 04 showing founding families' lethal doses sliced from master ledger. | 3 | Recover & rebind vs Bury secrets | (-2, 0) vs (+1, +2) | `item_dose_ledger` x1 |

---

## 2. Moral Dimension Analysis

Each questline tests an independent institutional tension around the radiation bureaucracy:
1. **Procedural Existence:** Do we measure and categorize, or remain blind? (`The First Reading`)
2. **Individual Compassion vs System Truth:** Should an individual be protected from the harsh classification they physically have? (`The Falsified Reading`)
3. **Tool Scarcity & Accountability:** When instrumentation is scarce, how are meters guarded and accounted for? (`The Stolen Dosimeter`)
4. **Scarcity & Palliative Triage:** Who receives limited symptom relief when supplies cannot cover all sufferers? (`The Sick of Room Seven`)
5. **Vulnerable Population Protection:** Do administrative rules protect youth or deprive the shelter of necessary labor? (`Child Over the Limit`)
6. **Epistemic Honesty:** Does the institution admit past measurement error when it creates panic? (`The Register Audit`)
7. **Identity vs Quantification:** Should a newborn be assigned a number before living? (`The Child's Number`)
8. **Institutional Corruption & Levers:** How does leadership handle illegal underground clearance bypasses? (`Black-Market Clean Bill`)
9. **Physical Standard Maintenance:** How much effort is justified to maintain precise calibration benchmarks? (`The Broken Calibration Chain`)
10. **Informed Consent & Urgent Danger:** Does personal volunteerism justify immediate high exposure? (`The Signed Hour`)
11. **Critical Infrastructure vs Human Sacrifice:** Can an essential specialist be expended to prevent collective failure? (`Exposure for Essential Worker`)
12. **Historical Memory vs Institutional Legitimacy:** Does the shelter confront foundational secrets or allow comfortable gaps? (`The Missing Page`)
