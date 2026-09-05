# Verdict Site Radiation Matrix & Dose Semantics

> **Attribute:** `baseRadsPerHour` (float)
> **Verdict Scale:** 20.0 to 60.0 rads/hour.
> **Dose System Reconciliation:** Distinct from Plan 81's `radiationUsv` (µSv/h); Verdict rads represent chronic ionizing exposure inside unshielded pre-war ruins.

---

| # | Location ID | Display Name | Base Rads / Hour | Environmental Radiological Cause |
|---|---|---|:---:|---|
| 1 | `loc_geophone_pit_1` | The First Geophone Pit | 34.0 | Radon seepage from bedrock fracture and legacy fallout washing down wellhead |
| 2 | `loc_twelve_gauge_array` | The Twelve-Gauge Array | 38.0 | Exposed ridgeline fallout deposition adhering to rusted steel posts |
| 3 | `loc_network_fuse_bunker` | The Fuse World | 42.0 | Radioactive dust drawn through ventilation shafts over years of intake |
| 4 | `loc_archive_tape_silo` | The Archive Tape-Silo | 48.0 | Contaminated particulate trapped in enclosed mountain cavern environment |
| 5 | `loc_abandoned_tide_gauge` | Greywater Tide Gauge Station | 28.0 | Seaward wash and marine spray carrying suspended fallout isotopes |
| 6 | `loc_coastal_meteorological_station` | Cape Wrath Meteorological Station | 32.0 | High-altitude windborne ash deposits coating radar dome and screen |
| 7 | `loc_clifftop_observation_bunker` | North Cliff Observation Bunker | 36.0 | Weathered granite outcrop exposed to coastal fallout plumes |
| 8 | `loc_sealed_marine_laboratory` | St. Jude Marine Laboratory | 44.0 | Heavy isotope accumulation in sediment core cylinders and brine tanks |
| 9 | `loc_forestry_survey_post` | Blackwood Forestry Survey Post | 26.0 | Humus and pine-needle fallout retention around timber station |
| 10 | `loc_geological_core_vault` | Highland Core-Sample Repository | 38.0 | Radioactive dust circulating through unsealed shale mine gallery |
| 11 | `loc_river_gauging_station` | Karsk River Gauging Station | 30.0 | Irradiated river silt deposited against masonry weir abutments |
| 12 | `loc_abandoned_agricultural_station` | Valley Experimental Agronomy Station | 35.0 | Residual isotopic contamination in experimental soil trial flats |
| 13 | `loc_decommissioned_signal_relay` | Pass 4 Signal Relay Mast | 36.0 | Alpine ridge fallout scour and snowpack meltwater contamination |
| 14 | `loc_border_checkpoint_ruins` | Gate Seven Border Checkpoint | 40.0 | Churned, irradiated mud and debris in mountain pass vehicle bottleneck |
| 15 | `loc_minefield_observation_tower` | Pylon 19 Observation Post | 46.0 | High elevated concrete observation deck catching regional ash currents |

---

## 2. Radiation Balancing & Safety Contract

- **Minimum:** 26.0 rads/h (`loc_forestry_survey_post`)
- **Maximum:** 48.0 rads/h (`loc_archive_tape_silo`)
- **Mean Rate:** 37.5 rads/h
- **Dwell Planning:** A 2-hour dwell at an average site (75 rads total) is easily manageable with standard anti-rad medication and protective gear, ensuring investigation remains accessible with preparation rather than impossible.
