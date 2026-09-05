# Plan 80 — Library Manuals Expansion (3 → 15) — Closeout Report

> **Mission Complete:** Expanded `library_manuals.json` into a complete fifteen-manual study curriculum spanning all six knowledge domains in ASHFALL (technical, medical, military, survival, scientific, social), backed by an acyclic prerequisite DAG, 100% skill discipline resolution, canonical research knowledge unlocks, and comprehensive test coverage.

---

## 1. Executive Summary

- **Plan:** 80 — Library Manuals Expansion
- **Original Baseline:** 3 manuals (`manual_water_filtration`, `manual_rad_first_aid`, `manual_improvised_weapons`)
- **Intermediate Baseline:** 12 manuals (Plan 27 expanded solar, hydroponics, surgery, radio, canning, handloading, cartography, relic electronics, quarantine)
- **Final Roster:** 15 manuals (added sub-zero cold weather survival, radiation dosimetry monitoring, and group conflict mediation)
- **Invariants Maintained:** Pure DATA expansion. Zero modifications to Core systems or save schema. `LibraryStudySystem` and `LibraryStudyHostSession` remain authoritative.

---

## 2. Complete Fifteen-Manual Roster

| # | Manual ID | Display Name | Category | Tier | Hours | Fatigue/hr | Morale | Power | Prereqs | Research / Knowledge Unlock | Skill Grant |
|---|---|---|---|:---:|:---:|:---:|:---:|:---:|---|---|---|
| 1 | `manual_water_filtration` | Field Water Filtration | technical | 1 | 10 | 0.30 | -0.5 | Yes | None | `knowledge_water_basics` | `survival`: 25 |
| 2 | `manual_rad_first_aid` | Radiation First Aid | medical | 1 | 12 | 0.35 | -0.4 | No | None | `knowledge_radiation_basics` | `medical`: 30 |
| 3 | `manual_improvised_weapons` | Improvised Weapons Fabrication | military | 2 | 14 | 0.40 | -0.6 | No | `manual_water_filtration` | `knowledge_combat_training` | `combat`: 35 |
| 4 | `manual_solar_maintenance` | Photovoltaic Maintenance & Inverter Rewiring | technical | 2 | 14 | 0.30 | -0.3 | Yes | `manual_water_filtration` | `knowledge_solar_basics` | `crafting`: 30 |
| 5 | `manual_bunker_hydroponics` | Subterranean Hydroponics & Soil Nutrients | survival | 2 | 12 | 0.25 | +0.2 | Yes | `manual_water_filtration` | `knowledge_hydroponics` | `survival`: 30 |
| 6 | `manual_field_trauma_surgery` | Emergency Trauma & Field Surgery Protocols | medical | 2 | 18 | 0.45 | -0.7 | Yes | `manual_rad_first_aid` | `knowledge_field_trauma_surgery` | `medical`: 40 |
| 7 | `manual_radio_signal_direction` | Radio Direction Finding & Morse Signal Analysis | technical | 1 | 12 | 0.30 | -0.2 | No | None | `knowledge_radio_basics` | `science`: 30 |
| 8 | `manual_vacuum_preservation` | Pressure Canning & Food Preservation | survival | 1 | 10 | 0.25 | +0.1 | No | None | `knowledge_food_preservation` | `survival`: 25 |
| 9 | `manual_ballistic_handloading` | Precision Match Handloaded Ammunition | military | 3 | 15 | 0.35 | -0.4 | Yes | `manual_improvised_weapons` | `knowledge_precision_ballistics` | `combat`: 35 |
| 10 | `manual_subterranean_cartography` | Subterranean Fault & Vault Cartography | technical | 2 | 14 | 0.35 | -0.3 | No | `manual_radio_signal_direction` | `knowledge_seismic_fault_mapping` | `scavenging`: 35 |
| 11 | `manual_relic_reverse_engineering` | Pre-War Solid-State Electronics Repair | technical | 3 | 16 | 0.40 | -0.4 | Yes | `manual_solar_maintenance` | `knowledge_signal_amplifier_blueprint` | `crafting`: 25, `science`: 25 |
| 12 | `manual_quarantine_epidemiology` | Pathogen Containment & Quarantine Protocols | medical | 2 | 16 | 0.40 | -0.6 | Yes | `manual_rad_first_aid` | `knowledge_pathogen_containment` | `medical`: 35 |
| 13 | `manual_cold_weather_survival` | Sub-Zero Exposure & Thermal Insulation | survival | 1 | 10 | 0.25 | -0.2 | No | None | `knowledge_shelter_insulation` | `survival`: 30 |
| 14 | `manual_radiation_monitoring` | Dosimetry & Environmental Radiation Monitoring | scientific | 2 | 12 | 0.30 | -0.3 | Yes | `manual_rad_first_aid` | `knowledge_micro_dosimeter_blueprint` | `science`: 35 |
| 15 | `manual_conflict_mediation` | De-Escalation & Group Conflict Mediation | social | 1 | 10 | 0.20 | +0.2 | No | None | `knowledge_scavenge_efficiency` | `survival`: 25 |

---

## 3. Prerequisite Graph Topography

- **Foundation Tier (Depth 0):** 6 manuals (`manual_water_filtration`, `manual_rad_first_aid`, `manual_radio_signal_direction`, `manual_vacuum_preservation`, `manual_cold_weather_survival`, `manual_conflict_mediation`).
- **Intermediate Tier (Depth 1):** 7 manuals (`manual_improvised_weapons`, `manual_solar_maintenance`, `manual_bunker_hydroponics`, `manual_field_trauma_surgery`, `manual_subterranean_cartography`, `manual_quarantine_epidemiology`, `manual_radiation_monitoring`).
- **Advanced Tier (Depth 2):** 2 manuals (`manual_ballistic_handloading`, `manual_relic_reverse_engineering`).
- **Topological Acyclicity:** 100% verified. Zero cycles detected.
- **Reachability:** All 15 manuals are fully reachable starting from zero-prerequisite foundations.

---

## 4. Verification Matrix Results

| Verification Gate | Command | Result | Notes |
|---|---|---|---|
| **C# Build** | `dotnet build Ashfall.csproj` | **PASS** | 0 warnings, 0 errors |
| **xUnit Unit Tests** | `dotnet test Ashfall.Core.Tests` | **PASS** | **6,768 passed, 0 failed, 0 skipped** (incl. 12 new tests in `Plan80LibraryManualsExpansionTests`) |
| **Data Integrity** | `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 0 errors across 208 catalogs (10,706 IDs validated) |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS** | CI gate PASS |
| **Scene Binding** | `godot --headless --path . -- --scene-binding-selftest` | **PASS** | 22/22 production panels passed |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS** | 27 production scenes checked, 0 errors |
