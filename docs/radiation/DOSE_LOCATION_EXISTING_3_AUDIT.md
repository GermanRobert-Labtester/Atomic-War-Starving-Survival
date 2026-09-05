# Dose Location Existing Locations Audit

> **Objective:** Forensic analysis of pre-existing records in `dose_locations.json` to anchor baseline calibration and ensure 100% backward compatibility.

---

## 1. Audit Table

| ID | Display Name | Sector | Risk Level | Radiation (µSv/h) | Historical Origin | Role in Shelter Simulation |
|---|---|---|:---:|:---:|---|---|
| `loc_the_dose_room` | Room Six, the Ledger Table | `bunker` | 0 | 0.02 | Expansion 07 Baseline | Primary administrative ledger station where dosimeter books are kept. Minimal natural background. |
| `loc_the_calibration_bench` | The Calibration Bench | `bunker` | 0 | 0.02 | Expansion 07 Baseline | Precision dosimeter zero-drift calibration station. Low-background lead shield box. |
| `loc_the_childrens_baseline_board` | The Children's Baseline Board | `bunker` | 0 | 0.02 | Expansion 07 Baseline | Pediatric corridor tracking cohort background levels. Clean zone. |
| `loc_the_register_hall` | The Central Register Hall | `bunker` | 0 | 0.02 | Plan 27 Expansion | Central administrative concourse. Clean interior traffic corridor. |
| `loc_the_screening_station` | Corridor Screening Checkpoint | `bunker` | 0 | 0.04 | Plan 27 Expansion | Triage decontam airlock with portal monitors; slight elevation due to returning scavenger clothing dust. |

---

## 2. Key Forensic Takeaways

1. **Bunker Standard Baseline:** 0.02 µSv/h is established as the canonical deep-shelter baseline reading across multiple catalogs (`year_of_ash_locations.json`, `dose_locations.json`).
2. **Transition Elevation:** The screening station establishes that interior points receiving dust or unwashed personnel rise to ~0.04 µSv/h (a 2× factor), still well within safe limits.
3. **Risk Level 0 Anchoring:** All bunker locations are classified at `riskLevel: 0`, meaning risk level 0 represents shielded, human-tolerable permanent living conditions.
4. **Non-Destructive Preservation:** Modifying or deleting any of these 5 entries breaks existing save files, test harnesses (`DoseContentCatalogTests.Locations_AreTheFiveStandingRooms`), and UI expectations. All 5 are strictly preserved.
