# Dose Location Content Utilization Report

> **Static & Runtime Consumption:** Verification of `dose_locations.json` utilization across Core loaders, Host sessions, and UI surfaces.

---

## 1. Static Content Pipeline Verification

| Catalog File | Consumer Class | Host Wiring | UI Surface | Gate Status |
|---|---|---|---|:---:|
| `dose_locations.json` | `DoseContentCatalogLoader.cs` | `DoseLedgerHostSession.cs` | `DoseRegisterSurface.cs` (`RenderContent()`) | **PASS** |

---

## 2. Roster Utilization Inventory

Every authored location in `dose_locations.json` has verified consumption:

1. `loc_the_dose_room` — Administrative UI / Story anchor
2. `loc_the_calibration_bench` — Calibration mechanic anchor
3. `loc_the_childrens_baseline_board` — Cohort register anchor
4. `loc_the_register_hall` — Dispute resolution queue anchor
5. `loc_the_screening_station` — Scavenger decontam intake
6. `loc_shelter_exterior_approach` — Exit airlock transition zone
7. `loc_surface_observation_post` — Guard / lookout watch post
8. `loc_contaminated_water_access` — Water pumping / treatment source
9. `loc_irradiated_forest_edge` — Biological fallout expedition zone
10. `loc_ruined_hospital_grounds` — Medical expedition destination
11. `loc_military_depot_perimeter` — Military ordnance expedition destination
12. `loc_frozen_wetland_crossing` — External travel corridor
13. `loc_burned_woodland_ridge` — External elevated route
14. `loc_garrison_checkpoint_gamma_exterior` — Faction territory checkpoint approach

Zero dead, orphan, or unreferenced locations exist.
