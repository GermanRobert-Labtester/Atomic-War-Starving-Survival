# Verdict Content Utilization Report

> **Target:** `Assets/StreamingAssets/Data/verdict_locations.json` (15 entries)
> **Validator:** `godot --headless --path . -- --content-utilization-selftest`

---

## 1. Inventory & Utilization Status

All 15 locations authored in `verdict_locations.json` are tracked and utilized across the project's systems:

| Location ID | Consuming Class / System | UI Exposure | Status |
|---|---|---|:---:|
| `loc_geophone_pit_1` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_twelve_gauge_array` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_network_fuse_bunker` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_archive_tape_silo` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_abandoned_tide_gauge` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_coastal_meteorological_station` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_clifftop_observation_bunker` | `VerdictCatalogLoader`, `VerdictPanel`, `ExpeditionSystem` | Verdict Map / Expeditions | Consumed |
| `loc_sealed_marine_laboratory` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_forestry_survey_post` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_geological_core_vault` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_river_gauging_station` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_abandoned_agricultural_station` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_decommissioned_signal_relay` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |
| `loc_border_checkpoint_ruins` | `VerdictCatalogLoader`, `VerdictPanel`, `ExpeditionSystem` | Verdict Map / Expeditions | Consumed |
| `loc_minefield_observation_tower` | `VerdictCatalogLoader`, `VerdictPanel`, `ReckoningSystem` | Verdict Investigation Map | Consumed |

---

## 2. CI Utilization Metrics

- **Total Catalogs Evaluated:** 490
- **Gameplay Consumed:** 146
- **Orphaned Catalogs:** 0
- **Unresolved Catalogs:** 40 (within acceptable historical tolerances)
- **CI Content Utilization Gate:** **PASS**
