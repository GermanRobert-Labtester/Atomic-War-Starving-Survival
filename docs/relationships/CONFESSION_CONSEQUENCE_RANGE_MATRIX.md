# Confession Consequence Range Matrix

> **Scope:** All 20 Personal Survivor Confession Secrets
> **Systems:** `SurvivorRelationsSystem`, `NeedsSystem`, `GuiltInsomniaSystem`, `MoralBranchingSystem`, `FlagLedger`

---

## 1. Consequence Profiles Table

| Archetype | Secret ID | Forgive Affinity | Forgive Morale | Grudge Affinity | Grudge Morale | Expose Faction & Delta | Expose Guilt | Blackmail Resource | Blackmail Hardening | Keep Trust |
|---|---|---|---|---|---|---|---|---|---|---|
| `the_surgeon` | `secret_surgeon_lost_patient` | +20 | +15 | -30 | -15 | `faction_independent` (+10) | +5 | `medicine` | +0.15 | +25 |
| `the_soldier` | `secret_soldier_civilian_order` | +15 | +10 | -40 | -20 | `faction_rebel` (+15) | +10 | `assault_rifle_parts` | +0.20 | +30 |
| `the_pharmacist` | `secret_pharmacist_stolen_morphine` | +18 | +12 | -35 | -18 | `faction_independent` (+10) | +8 | `pharmaceuticals` | +0.20 | +25 |
| `the_mother` | `secret_mother_child_left` | +25 | +20 | -20 | -10 | `faction_independent` (-15) | +20 | `comfort_items` | +0.10 | +35 |
| `the_mechanic` | `secret_mechanic_sabotaged_generator` | +14 | +8 | -35 | -20 | `faction_iron_clique` (-10) | +12 | `generator_coils` | +0.20 | +25 |
| `the_teacher` | `secret_teacher_burned_books` | +16 | +10 | -25 | -12 | `faction_independent` (+5) | +15 | `firewood_reserve` | +0.10 | +20 |
| `the_refugee` | `secret_refugee_stolen_identity` | +18 | +12 | -30 | -15 | `faction_military` (-20) | +10 | `evacuation_papers` | +0.15 | +25 |
| `the_electrician` | `secret_electrician_blackout` | +15 | +10 | -45 | -25 | `faction_military` (+15) | +20 | `copper_wiring` | +0.20 | +30 |
| `the_cook` | `secret_cook_ration_cache` | +16 | +10 | -30 | -15 | `faction_independent` (+10) | +12 | `rations` | +0.15 | +25 |
| `the_engineer` | `secret_engineer_unreinforced_span` | +12 | +8 | -45 | -25 | `faction_rebel` (+20) | +25 | `building_materials` | +0.20 | +30 |
| `the_farmer` | `secret_farmer_scorched_seeds` | +14 | +10 | -30 | -15 | `faction_independent` (-10) | +15 | `seed_packet` | +0.15 | +25 |
| `the_priest` | `secret_priest_silent_prayers` | +22 | +18 | -25 | -15 | `faction_independent` (-15) | +10 | `influence` | +0.20 | +30 |
| `the_journalist` | `secret_journalist_killed_story` | +12 | +8 | -40 | -20 | `faction_rebel` (+15) | +20 | `intelligence` | +0.15 | +25 |
| `the_pilot` | `secret_pilot_refused_sortie` | +18 | +12 | -35 | -15 | `faction_military` (-25) | +18 | `aviation_parts` | +0.15 | +30 |
| `the_scientist` | `secret_scientist_altered_assays` | +10 | +8 | -40 | -20 | `faction_independent` (-15) | +22 | `water_filters` | +0.20 | +25 |
| `the_hunter` | `secret_hunter_treeline_shot` | +8 | +5 | -50 | -30 | `faction_independent` (-20) | +30 | `food_and_ammo` | +0.25 | +20 |
| `the_nurse` | `secret_nurse_missed_medication` | +15 | +10 | -35 | -18 | `faction_independent` (-10) | +18 | `medical_supplies` | +0.15 | +25 |
| `the_carpenter` | `secret_carpenter_faulty_shoring` | +14 | +8 | -32 | -16 | `faction_independent` (-12) | +15 | `building_materials` | +0.20 | +25 |
| `the_child` | `secret_child_left_friend` | +24 | +16 | -20 | -14 | `faction_independent` (-5) | +25 | `scavenged_trinkets` | +0.25 | +30 |
| `the_old_man` | `secret_old_man_quiet_compliance` | +12 | +6 | -40 | -22 | `faction_independent` (-15) | +16 | `ration_stamps` | +0.20 | +25 |

---

## 2. Parameter Distribution Analysis

- **Forgiveness Affinity:** Mean $\approx +16.0$, Range: $[+8, +25]$
- **Forgiveness Morale:** Mean $\approx +10.9$, Range: $[+5, +20]$
- **Grudge Affinity:** Mean $\approx -34.8$, Range: $[-50, -20]$
- **Grudge Morale:** Mean $\approx -17.9$, Range: $[-30, -10]$
- **Keep Trust:** Mean $\approx +26.5$, Range: $[+20, +35]$
- **Blackmail Hardening:** Range: $[0.10, 0.25]$
- **Expose Guilt:** Range: $[+5, +30]$

Every secret presents an authentic moral crisis where neither forgiveness nor grudge is an easy, objective choice.
