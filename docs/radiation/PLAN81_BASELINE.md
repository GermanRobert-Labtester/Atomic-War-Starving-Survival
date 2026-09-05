# Plan 81 — Baseline Forensic Report: Dose-Ledger Locations Expansion

> **Scope:** Audit baseline state of `dose_locations.json`, `DoseContentCatalog.cs`, radiation accumulation semantics, and location identities before Plan 81 expansion.

---

## 1. Initial State & Assumptions vs. Repository Truth

| Vector | Plan Assumption | Repository Reality | Notes / Reconciliation |
|---|---|---|---|
| **File Location** | `Assets/StreamingAssets/Data/dose_locations.json` | `Assets/StreamingAssets/Data/dose_locations.json` | Exact path confirmed |
| **Initial Record Count** | 3 entries | 5 entries | Expansion 07 originally defined 3 (`loc_the_dose_room`, `loc_the_calibration_bench`, `loc_the_childrens_baseline_board`), later expanded with 2 administrative rooms (`loc_the_register_hall`, `loc_the_screening_station`) in Plan 27. |
| **Initial Sectors** | All 3 `bunker` | All 5 `bunker` | Confirmed: 100% of existing locations were bunker-internal. Zero surface, expedition, external, or faction geography existed. |
| **Loader Class** | `DoseContentCatalog.cs` | `Assets/Ashfall.Core/DoseContentCatalog.cs` | Deserializes `DoseLocationsRoot` wrapper (`{"schema_version": 1, "locations": [...]}`). |
| **Host Session** | `DoseLedgerHostSession.cs` | `src/Host/DoseLedgerHostSession.cs` | Holds `DoseLedgerSystem`, `DoseRegistersCatalog`, `DoseContentCatalog`, `QuestlineSystem`. |
| **UI Surface** | `DoseRegisterSurface.cs` | `src/Dose/DoseRegisterSurface.cs` | `RenderContent()` iterates through `_session.Content.locations` and formats display name, id, and description. |

---

## 2. Baseline Location Inventory

The 5 initial bunker records:

1. **`loc_the_dose_room`** (Room Six, the Ledger Table)
   - Sector: `bunker` | Risk: 0 | Dose: 0.02 µSv/h
   - Role: Core record-keeping desk; heavily shielded interior.
2. **`loc_the_calibration_bench`** (The Calibration Bench)
   - Sector: `bunker` | Risk: 0 | Dose: 0.02 µSv/h
   - Role: Instrument maintenance bench; benchmark zero-drift room.
3. **`loc_the_childrens_baseline_board`** (The Children's Baseline Board)
   - Sector: `bunker` | Risk: 0 | Dose: 0.02 µSv/h
   - Role: Pediatric baseline registry in living corridor.
4. **`loc_the_register_hall`** (The Central Register Hall)
   - Sector: `bunker` | Risk: 0 | Dose: 0.02 µSv/h
   - Role: Administrative center for dispute queues and clearances.
5. **`loc_the_screening_station`** (Corridor Screening Checkpoint)
   - Sector: `bunker` | Risk: 0 | Dose: 0.04 µSv/h
   - Role: Decontamination airlock screening portal for returning scavengers.

---

## 3. Plan 81 Target Expansion

To fulfill the radiation-cartography pillar without breaking the 5 existing bunker locations pinned by `DoseContentCatalogTests` and `Plan27BodyMindTests`, Plan 81 preserves all 5 bunker rooms and authors exactly **9 new locations** across the 4 missing sectors:
- **Surface (3):** `loc_shelter_exterior_approach`, `loc_surface_observation_post`, `loc_contaminated_water_access`
- **Expedition (3):** `loc_irradiated_forest_edge`, `loc_ruined_hospital_grounds`, `loc_military_depot_perimeter`
- **External (2):** `loc_frozen_wetland_crossing`, `loc_burned_woodland_ridge`
- **Faction (1):** `loc_garrison_checkpoint_gamma_exterior`

**Total Roster:** 14 locations across 5 distinct sectors.
