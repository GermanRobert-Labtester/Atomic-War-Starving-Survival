# Plan 80 — Library Manuals Progression & Balance Audit

> **Balance Authority:** `Assets/StreamingAssets/Data/library_manuals.json` and `Assets/Ashfall.Core/LibraryStudySystem.cs`.

---

## 1. Catalog Metric Summary

| Manual ID | Category | Tier | Hours | Days (8h/d) | Fatigue/hr | Total Fatigue | Morale | Power | Skill XP Granted | Research / Knowledge Unlock |
|---|---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|---|---|
| `manual_water_filtration` | technical | 1 | 10 | 2 | 0.30 | 3.0 | -0.5 | Yes | Survival: 25 | `knowledge_water_basics` |
| `manual_rad_first_aid` | medical | 1 | 12 | 2 | 0.35 | 4.2 | -0.4 | No | Medical: 30 | `knowledge_radiation_basics` |
| `manual_improvised_weapons` | military | 2 | 14 | 2 | 0.40 | 5.6 | -0.6 | No | Combat: 35 | `knowledge_combat_training` |
| `manual_solar_maintenance` | technical | 2 | 14 | 2 | 0.30 | 4.2 | -0.3 | Yes | Crafting: 30 | `knowledge_solar_basics` |
| `manual_bunker_hydroponics` | survival | 2 | 12 | 2 | 0.25 | 3.0 | +0.2 | Yes | Survival: 30 | `knowledge_hydroponics` |
| `manual_field_trauma_surgery` | medical | 2 | 18 | 3 | 0.45 | 8.1 | -0.7 | Yes | Medical: 40 | `knowledge_field_trauma_surgery` |
| `manual_radio_signal_direction` | technical | 1 | 12 | 2 | 0.30 | 3.6 | -0.2 | No | Science: 30 | `knowledge_radio_basics` |
| `manual_vacuum_preservation` | survival | 1 | 10 | 2 | 0.25 | 2.5 | +0.1 | No | Survival: 25 | `knowledge_food_preservation` |
| `manual_ballistic_handloading` | military | 3 | 15 | 2 | 0.35 | 5.25 | -0.4 | Yes | Combat: 35 | `knowledge_precision_ballistics` |
| `manual_subterranean_cartography` | technical | 2 | 14 | 2 | 0.35 | 4.9 | -0.3 | No | Scavenging: 35 | `knowledge_seismic_fault_mapping` |
| `manual_relic_reverse_engineering` | technical | 3 | 16 | 2 | 0.40 | 6.4 | -0.4 | Yes | Crafting: 25, Science: 25 | `knowledge_signal_amplifier_blueprint` |
| `manual_quarantine_epidemiology` | medical | 2 | 16 | 2 | 0.40 | 6.4 | -0.6 | Yes | Medical: 35 | `knowledge_pathogen_containment` |
| `manual_cold_weather_survival` | survival | 1 | 10 | 2 | 0.25 | 2.5 | -0.2 | No | Survival: 30 | `knowledge_shelter_insulation` |
| `manual_radiation_monitoring` | scientific | 2 | 12 | 2 | 0.30 | 3.6 | -0.3 | Yes | Science: 35 | `knowledge_micro_dosimeter_blueprint` |
| `manual_conflict_mediation` | social | 1 | 10 | 2 | 0.20 | 2.0 | +0.2 | No | Survival: 25 | `knowledge_scavenge_efficiency` |

---

## 2. Economic & Progress Totals

- **Total Study Hours across Catalog:** 197 hours (~25 full-day survivor study assignments).
- **Power Distribution:** 8 manuals require power (40W room draw required), 7 manuals can be read without power (candlelight/daylight study).
- **Discipline Distribution:**
  - `survival`: 5 grants (140 XP total)
  - `medical`: 3 grants (105 XP total)
  - `science`: 3 grants (90 XP total)
  - `combat`: 2 grants (70 XP total)
  - `crafting`: 2 grants (55 XP total)
  - `scavenging`: 1 grant (35 XP total)
- **Fatigue Expenditure:** Ranges from 2.0 to 8.1 total fatigue units across a manual's duration, matching survivor stamina budgets without triggering exhaustion collapse during normal resting intervals.
