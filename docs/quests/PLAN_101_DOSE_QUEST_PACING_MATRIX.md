# Plan 101 — Dose Quest Pacing Matrix

**Document ID:** `docs/quests/PLAN_101_DOSE_QUEST_PACING_MATRIX.md`
**Campaign Horizon:** 360 Days (Year of Ash default)
**Catalog Authority:** `Assets/StreamingAssets/Data/dose_quests.json`

---

## 1. Campaign-Paced Scheduling

In accordance with Plan 101 guidelines, quest availability windows are verified against active campaign pacing. No quest sits beyond reachable day limits, and every questline provides a wide availability window (`maxDay: 360`) to prevent arbitrary miss states caused by normal gameplay variance.

| Questline ID | Min Day | Max Day | Campaign Phase | Progression Seam |
|:---|:---:|:---:|:---|:---|
| `quest_the_dose_the_first_reading` | 40 | 360 | Early Winter | Establishment of the register and first recorded dose. |
| `quest_the_falsified_reading` | 60 | 360 | Early-Mid Winter | Initial discrepancies between surface scouting and shelter records. |
| `quest_the_stolen_dosimeter` | 80 | 360 | Mid Winter | Tool scarcity during major winter mechanical overhauls. |
| `quest_the_sick_of_room_seven` | 90 | 360 | Deep Winter | Onset of acute symptoms as cumulative exposure accrues. |
| `quest_child_over_the_limit` | 110 | 360 | Late Winter | Youth labor pressure during peak cold survival demands. |
| `quest_the_register_audit` | 130 | 360 | Thaw Approaching | Calibration drift detected across past month of readings. |
| `quest_the_childs_number` | 150 | 360 | Early Spring | New births in the cohort corridor; baseline debates. |
| `quest_black_market_clean_bill` | 160 | 360 | Spring Expansion | Forged clearance chits as surface trade corridors open. |
| `quest_the_broken_calibration_chain` | 180 | 360 | Mid Spring | Reference crystal failure necessitating standard rebuilding. |
| `quest_the_signed_hour` | 200 | 360 | Late Spring | Hazardous reactor corridor maintenance volunteers. |
| `quest_exposure_for_the_essential_worker` | 210 | 360 | Early Summer | Critical generator governor failure threatening shelter grid. |
| `quest_the_missing_page` | 230 | 360 | Mid Summer / Endgame | Historical audit of Day One fallout records in Bay A. |

---

## 2. Window Pacing Design Rules
1. **Reachable Late-Game Horizon:** The latest quest trigger is `minDay: 230`, well within the 360-day campaign threshold.
2. **Generous Expiration Buffer:** Every questline sets `maxDay: 360`, guaranteeing that players who defer resolving a bureaucratic dilemma are never abruptly cut off until the final campaign closure.
3. **Pacing Cadence:** Quests unlock at intervals of 10 to 30 days, pacing the moral dilemmas evenly across the 4 seasons (Fall, Winter, Spring, Summer) of the Year of Ash.
