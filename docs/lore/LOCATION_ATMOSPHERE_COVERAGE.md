# Location → Environmental Text Coverage Matrix

**Date:** 2026-09-01
**Status:** Coverage audit complete — identifies gaps for Plan 17 integration.

---

## Purpose

Map every visitable location to its atmosphere text sources, identify coverage gaps, and prioritize locations for environmental storytelling integration.

---

## Data Sources

| Source | Entries | Key format | Status |
|--------|---------|------------|--------|
| `environmental_atmosphere_expansion.json` | 152 | Conceptual names (e.g., `geothermal_plant_ruins`) | **Orphaned** — no runtime consumer |
| `environmental_texts_expansion_05.json` | 36 | Conceptual names (e.g., `bunker_perimeter`) | **Orphaned** — no runtime consumer |
| `world_history.json` | 79 | `loc_*` IDs (59), `location_*` (7), bare (5) | Active via `EvolvingWorldCatalog` |
| `deep_lore_locations.json` | 10 | `loc_*` IDs | Active via `DeepLoreLocationCatalogLoader` |

---

## Coverage Matrix

### Key Format Mismatch

**Critical issue:** Atmosphere texts use **conceptual location names** (e.g., `flooded_subway_depot`), while the canonical location system uses **`loc_*` prefixed IDs** (e.g., `loc_flooded_subway_depot`). This prevents direct runtime binding.

| Atmosphere source | Key format | Count | Canonical equivalent |
|-------------------|------------|-------|----------------------|
| `environmental_atmosphere_expansion.json` | Conceptual | 152 | Requires mapping table |
| `environmental_texts_expansion_05.json` | Conceptual | 36 | Requires mapping table |
| `world_history.json` (discovery_location_id) | `loc_*` / `location_*` / bare | 79 | Already canonical |

### Coverage Tiers

| Tier | Definition | Count | Examples |
|------|------------|-------|----------|
| **Direct coverage** | Location has atmosphere text keyed to its conceptual name | ~40 | `geothermal_plant_ruins`, `flooded_subway_depot`, `municipal_archive` |
| **Generic coverage** | Location can use generic atmosphere texts (weather, time-of-day, sensory cues) | ~80 | Any outdoor location can use `atm_weather_*`, `atm_daynight_*` |
| **No coverage** | Location has no atmosphere text and no generic fallback applies | ~30 | Minor quest locations, newly added `loc_*` IDs |

### Atmosphere Text Categories (152 entries)

| Category | ID prefix | Count | Applicability |
|----------|-----------|-------|---------------|
| Location descriptions | `atm_loc_` | 5 | Direct (specific locations) |
| Sensory deprivation | `atm_sens_deprivation_` | 4 | Underground/shelter locations |
| Environmental load | `atm_env_load_` | 4 | Hazard zones |
| Threat detection | `atm_threat_detection_` | 4 | Expedition encounters |
| Stealth awareness | `atm_stealth_awareness_` | 4 | Faction territory |
| Loot weight | `atm_loot_weight_` | 3 | Scavenging sites |
| Encumbrance strain | `atm_encumbrance_` | 1 | Universal (carrying state) |
| Extraction vignettes | `atm_extraction_` | 4 | Expedition escape |
| Entryway descriptions | `atm_entryway_` | 2 | Location entry transitions |
| Chemical warning | `atm_chemical_warning_` | 3 | Contaminated zones |
| Biological warning | `atm_biological_warning_` | 4 | Mutated flora/fauna zones |
| Temperature warning | `atm_temperature_warning_` | 4 | Extreme cold/heat |
| Weather cues | `atm_weather_` | 5 | Universal (outdoor) |
| Day/night cues | `atm_daynight_` | 4 | Universal (time-based) |
| Sound cues | `atm_sound_cue_` | 4 | Universal |
| Smell cues | `atm_smell_cue_` | 4 | Universal |
| Touch cues | `atm_touch_cue_` | 4 | Universal |
| Taste cues | `atm_taste_cue_` | 4 | Universal (radiation/metallic) |
| Visual cues | `atm_visual_cue_` | 4 | Universal |
| Hidden details | `atm_hidden_detail_` | 4 | Discovery mechanics |
| Environmental storytelling | `atm_env_storytelling_` | 4 | Narrative vignettes |
| Atmospheric mood (14 subtypes) | `atm_atmos_*` | 36 | Contextual (mood-based) |

---

## Location Exposure Ranking

### Tier 1 — High exposure (faction hubs, quest hubs)

These locations see the most player traffic and benefit most from atmosphere text.

| Location | Canonical ID | Atmosphere coverage | Priority |
|----------|--------------|---------------------|----------|
| Holdfast (player shelter) | `player_shelter` | Generic only | **HIGH** — player spends 60%+ of time here |
| Water Treatment Plant | `loc_water_treatment_plant` | Direct (`atm_loc_*`) | Medium |
| Civil Defense Bunker | `loc_civil_defense_bunker` | Direct | Medium |
| Municipal Archive | `loc_municipal_archive` | Direct (`municipal_archive`) | Medium |
| Grain Silo | `loc_grain_silo` | Direct | Medium |
| Bridge Seven | `loc_bridge_seven` | Direct | Medium |

### Tier 2 — Medium exposure (quest hubs, deep-lore sites)

| Location | Canonical ID | Atmosphere coverage | Priority |
|----------|--------------|---------------------|----------|
| Substation Yard | `loc_substation_yard` | Generic | Medium |
| Agricultural Coop | `loc_agricultural_coop` | Generic | Medium |
| Evacuation Bus Depot | `loc_evacuation_bus_depot` | Generic | Low |
| Police Precinct | `loc_police_precinct` | Generic | Low |
| Dentist's Row | `loc_dentists_row` | Generic | Low |
| Transit Authority HQ | `loc_transit_authority_hq` | Generic | Low |
| Comm Array | `loc_comm_array` | Generic | Low |
| Missile Silo | `loc_missile_silo` | Generic | Low |
| Suburban District | `loc_suburban_district` | Generic | Low |
| Regional Hospital | `loc_regional_hospital` | Generic | Low |

### Tier 3 — Low exposure (minor locations, expansion sites)

| Location | Canonical ID | Atmosphere coverage | Priority |
|----------|--------------|---------------------|----------|
| Conscription Office | `loc_conscription_office` | None | Low |
| Grange Hall | `loc_grange_hall` | None | Low |
| Bus Reversal Loop | `loc_bus_reversal_loop` | Direct (`bus_reversal_loop`) | Low |
| Highway Checkpoint | `loc_highway_checkpoint` | None | Low |
| Ash Woodland | `loc_ash_woodland` | None | Low |
| Metro Tunnel | `loc_metro_tunnel` | None | Low |
| Basement Vault | `loc_basement_vault` | None | Low |
| The Vessel's Cell | `loc_the_vessels_cell` | None | Low |
| Fuel Depot | `loc_fuel_depot` | None | Low |
| Toll House | `loc_toll_house` | None | Low |
| Apiary Rows | `loc_apiary_rows` | None | Low |
| Ash Sign Shrine | `loc_ash_sign_shrine` | None | Low |
| Low Background Lab | `loc_low_background_lab` | None | Low |
| Ice Road Gate | `loc_ice_road_gate` | None | Low |
| Cluster Office | `loc_cluster_office` | None | Low |
| Cluster Block C | `loc_cluster_block_c` | None | Low |
| Shelf Hearth 4 | `loc_shelf_hearth4` | None | Low |
| Allotment Glasshouse Complex | `loc_allotment_glasshouse_complex` | None | Low |
| Hydro Baron Aqueduct Manifold | `loc_hydro_baron_aqueduct_manifold` | None | Low |
| Garrison Checkpoint Gamma | `loc_garrison_checkpoint_gamma` | None | Low |
| Denial Cut Substation | `loc_denial_cut_substation` | None | Low |
| Second Winter Homestead | `loc_second_winter_homestead` | None | Low |
| Maritime Icebreaker Dock | `loc_maritime_icebreaker_dock` | None | Low |
| D9 Cache Bunker Delta | `loc_d9_cache_bunker_delta` | None | Low |
| Crossing Records Room | `loc_crossing_records_room` | None | Low |
| Crossing Weighbridge | `loc_crossing_weighbridge` | None | Low |

### `location_*` prefix locations (7 entries — non-canonical prefix)

| Location | Non-canonical ID | Canonical equivalent | Atmosphere coverage |
|----------|------------------|----------------------|---------------------|
| Ministry of Truth Bunker | `location_ministry_of_truth_bunker` | `loc_ministry_of_truth_bunker` | None |
| Abandoned Convoy Yard | `location_abandoned_convoy_yard` | `loc_abandoned_convoy_yard` | None |
| Flooded Subway Depot | `location_flooded_subway_depot` | `loc_flooded_subway_depot` | Direct (`flooded_subway_depot`) |
| The Memory Vault | `location_the_memory_vault` | `loc_the_memory_vault` | None |
| Sub-Level 4 Transit | `location_sub_level_4_transit` | `loc_sub_level_4_transit` | None |
| Subterranean Seed Vault | `location_subterranean_seed_vault` | `loc_subterranean_seed_vault` | Direct (`subterranean_seed_vault`) |

---

## Coverage Gaps

### Critical gaps (high-exposure locations with no direct coverage)

1. **`player_shelter` (Holdfast)** — Player's primary location; only generic weather/time cues apply. **Priority: HIGH**
2. **Expansion locations (15+)** — Crossing, Hydro Baron, cluster sites have no atmosphere text. **Priority: MEDIUM**

### Systemic gaps

1. **Key format mismatch** — 152 atmosphere texts use conceptual names; canonical system uses `loc_*` IDs. Requires a mapping table or ID normalization.
2. **No runtime consumer** — Both atmosphere text files are orphaned. Plan 17 must wire a loader and presentation system.
3. **Generic fallback insufficient** — Weather/time cues apply universally, but location-specific atmosphere requires direct coverage.

---

## Plan 17 Integration Priorities

1. **Build mapping table** — Map conceptual atmosphere keys to canonical `loc_*` IDs (estimated ~40 direct mappings).
2. **Wire atmosphere loader** — Create `AtmosphereTextCatalogLoader` in Core; inject into `LocationMemorySystem` or new `AtmospherePresentationSystem`.
3. **Prioritize high-exposure locations** — Start with `player_shelter`, faction hubs, quest hubs.
4. **Generic fallback system** — Weather/time/sensory cues should auto-apply based on location tags (outdoor/underground/hazard).
5. **Location-specific authoring** — Author new atmosphere texts for the 30+ locations with no coverage.
