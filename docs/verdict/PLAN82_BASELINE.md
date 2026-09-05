# Plan 82 — Baseline Forensic Report: Verdict Investigation Sites Expansion

> **Scope:** Forensic reconnaissance of `verdict_locations.json`, `VerdictCatalogLoader.cs`, `VerdictNpcSystem.cs`, `verdict_npcs.json`, and `verdict_radio.json` prior to the Plan 82 expansion.

---

## 1. Initial State & Assumptions vs. Repository Truth

| Vector | Plan Assumption | Repository Reality | Notes / Reconciliation |
|---|---|---|---|
| **Location File** | `Assets/StreamingAssets/Data/verdict_locations.json` | `Assets/StreamingAssets/Data/verdict_locations.json` | Verified exact path. Export mirror at `builds/linux/...` also present. |
| **Baseline Count** | 4 investigation sites | 4 investigation sites | Exactly 4 verified sites: `loc_geophone_pit_1`, `loc_twelve_gauge_array`, `loc_network_fuse_bunker`, `loc_archive_tape_silo`. |
| **Loader Class** | `VerdictCatalogLoader.cs` | `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` | Loads wrapped list `{"schema_version": 1, "locations": [...]}` into `VerdictLocationEntry`. |
| **NPC System** | `VerdictNpcSystem.cs` | `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` | Live; loads 6+ NPCs from `verdict_npcs.json`. |
| **Radio Corpus** | `verdict_radio.json` | `Assets/StreamingAssets/Data/verdict_radio.json` | 13 authored telemetry, maintenance, witness, and census broadcasts. |

---

## 2. Baseline Four-Site Inventory (Arc 1: The Tempest Array)

1. **`loc_geophone_pit_1`** (The First Geophone Pit)
   - Danger: 6 | Travel: 5.5h | Rads: 34 rad/h
   - Role: Sunk seismometer array listening to bedrock vibrations; cable runs east into treeline.
2. **`loc_twelve_gauge_array`** (The Twelve-Gauge Array)
   - Danger: 7 | Travel: 6.0h | Rads: 38 rad/h
   - Role: Twelve shot-firing sounding stations on the ridge; charge weights and depths kept legible.
3. **`loc_network_fuse_bunker`** (The Fuse World)
   - Danger: 8 | Travel: 7.5h | Rads: 42 rad/h
   - Role: Shielded service gallery with glass-fronted cabinets of linen-coded shift charters leading to tape-silo door.
4. **`loc_archive_tape_silo`** (The Archive Tape-Silo)
   - Danger: 9 | Travel: 8.5h | Rads: 48 rad/h
   - Role: Chapel-sized vault of tape racks tagged by year; reading lectern with solitary child's handprint.

---

## 3. Expansion Trajectory

Plan 82 expands the catalog to **15 sites** by adding three new investigative trails:
- **Arc 2 — The Coastal Survey (4 sites):** `loc_abandoned_tide_gauge`, `loc_coastal_meteorological_station`, `loc_clifftop_observation_bunker`, `loc_sealed_marine_laboratory`
- **Arc 3 — The Interior Caches (4 sites):** `loc_forestry_survey_post`, `loc_geological_core_vault`, `loc_river_gauging_station`, `loc_abandoned_agricultural_station`
- **Arc 4 — The Border Wire (3 sites):** `loc_decommissioned_signal_relay`, `loc_border_checkpoint_ruins`, `loc_minefield_observation_tower`
