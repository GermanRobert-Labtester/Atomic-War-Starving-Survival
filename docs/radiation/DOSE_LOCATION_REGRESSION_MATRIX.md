# Dose Location Regression Matrix

> **Matrix:** 20-point comprehensive verification covering parsing, location identity, accumulation, expedition handoff, weather modifiers, ledger provenance, save state, and UI.

---

## 1. Twenty-Scenario Verification Matrix

| # | Scenario | Test / Inspection Target | Result | Evidence / Notes |
|:---:|---|---|:---:|---|
| 1 | Existing Bunker Room 1 | `loc_the_dose_room` loads with sector `bunker`, risk 0, dose 0.02 | **PASS** | Pinned in `DoseContentCatalogTests` and `Plan81DoseLocationsExpansionTests` |
| 2 | Existing Bunker Room 2 | `loc_the_calibration_bench` loads with sector `bunker`, risk 0, dose 0.02 | **PASS** | Preserved verbatim |
| 3 | Existing Bunker Room 3 | `loc_the_childrens_baseline_board` loads with sector `bunker`, risk 0, dose 0.02 | **PASS** | Preserved verbatim |
| 4 | Existing Bunker Room 4 | `loc_the_register_hall` loads with sector `bunker`, risk 0, dose 0.02 | **PASS** | Preserved verbatim |
| 5 | Existing Bunker Room 5 | `loc_the_screening_station` loads with sector `bunker`, risk 0, dose 0.04 | **PASS** | Preserved verbatim |
| 6 | Surface Location 1 | `loc_shelter_exterior_approach` (0.85 µSv/h, risk 1) | **PASS** | Immediate transition apron outside airlock |
| 7 | Surface Location 2 | `loc_surface_observation_post` (1.75 µSv/h, risk 2) | **PASS** | Armored observation cupola |
| 8 | Surface Location 3 | `loc_contaminated_water_access` (3.50 µSv/h, risk 3) | **PASS** | Intake weir with radioactive sediment |
| 9 | Expedition Location 1 | `loc_irradiated_forest_edge` (18.5 µSv/h, risk 5) | **PASS** | Dead red pine needle fallout sink |
| 10 | Expedition Location 2 | `loc_ruined_hospital_grounds` (28.0 µSv/h, risk 6) | **PASS** | Broken radiotherapy debris perimeter |
| 11 | Expedition Location 3 | `loc_military_depot_perimeter` (45.0 µSv/h, risk 7) | **PASS** | Pulverized ordnance hardstand |
| 12 | External Location 1 | `loc_frozen_wetland_crossing` (6.20 µSv/h, risk 4) | **PASS** | Black ice causeway crossing |
| 13 | External Location 2 | `loc_burned_woodland_ridge` (8.40 µSv/h, risk 4) | **PASS** | Exposed granite ridgeline |
| 14 | Faction Location | `loc_garrison_checkpoint_gamma_exterior` (4.10 µSv/h, risk 3) | **PASS** | Warlord checkpoint perimeter |
| 15 | Expedition ID Lookup | Cross-reference destination IDs | **PASS** | Shared canonical identities documented |
| 16 | Surface Transition | Leaving bunker changes dose accumulation | **PASS** | Proven in ledger booking harness |
| 17 | Weather Modifier | Baseline static, dynamic weather multiplies | **PASS** | Verified zero mutation of catalog |
| 18 | High-Dose Dwell | 45.0 µSv/h accumulated across dwell | **PASS** | Numerically stable, non-saturating |
| 19 | Mid-Exposure Save | Capturing state mid-campaign round-trips | **PASS** | Verified in `DoseLedgerStateCaptureAndRestoreRoundTrip` |
| 20 | Deterministic Dose Trace | Replay with fixed seed produces identical booked readings | **PASS** | Verified using `SeededRng` |
