# Seasonal Event Cadence & Anti-Spam Budget

> **Authority:** `Assets/StreamingAssets/Data/seasonal_events.json`, `Assets/Ashfall.Core/World/SeasonalEventSystem.cs`

---

## 1. Event Distribution by Phase

| Season Phase | Event ID | Event Name | Severity | Trigger Chance | Cooldown | Mitigation |
|---|---|---|---|---|---|---|
| **Ash Fall** | `event_season_ash_filter_clog` | Coarse Ash Intake Clog | Medium | 0.40 | 12d | 1x `water_filter` |
| | `event_season_ash_roof_load` | Volumetric Ash Surcharge | Medium | 0.30 | 15d | 2x `scrap_mechanical` |
| | `event_season_static_radio_blackout` | Particulate Ion Blackout | Low | 0.35 | 10d | 1x `copper_wire` |
| **Deep Freeze** | `event_season_freeze_pipe_burst` | Main Hydraulic Conduit Freeze | High | 0.45 | 14d | 3x `scrap_mechanical` |
| | `event_season_freeze_salter_hypothermia` | Desalination Shift Frostbite | Medium | 0.35 | 12d | 4x `coal` |
| | `event_season_freeze_ice_road_solid` | Bedrock Ice Stabilization | Low | 0.50 | 20d | None (Positive) |
| **The Thaw** | `event_season_thaw_sump_flood` | Permafrost Slush Sump Inundation | High | 0.40 | 14d | 2x `fuel` |
| | `event_season_thaw_ice_road_fracture` | Estuary Slush Crack | High | 0.50 | 15d | 2x `item_foundry_ice_anchor` |
| | `event_season_thaw_spoilage_acceleration` | Thermal Humidity Bloom | Medium | 0.35 | 10d | 3x `item_crossing_traded_salt` |
| **Black Bloom** | `event_season_bloom_greenhouse_spores` | Black-Rust Fungal Infiltration | High | 0.40 | 12d | 2x `medicine` |
| | `event_season_bloom_algal_water_taint` | Warm-Water Algal Colony | Medium | 0.35 | 14d | 2x `water_filter` |
| | `event_season_bloom_surface_rad_burst` | Rad-Absorbing Pollen Wave | Medium | 0.30 | 10d | 1x `medicine` |
| **High Cold** | `event_season_highcold_generator_stall` | Diesel Fuel Waxing Stall | High | 0.45 | 15d | 3x `fuel` |
| | `event_season_highcold_heater_overload` | Heating Element Coil Burnout | Medium | 0.35 | 12d | 2x `copper_wire` |
| | `event_season_highcold_frozen_caravan` | Wasteland Caravan Sled Jam | Medium | 0.30 | 18d | 2x `fuel` |
| **The Turning** | `event_season_turning_clear_sky_window` | High-Pressure Clear Sky Opening | Low | 0.50 | 20d | None (Positive) |
| | `event_season_turning_soil_reclamation` | Spring Silt Siltation | Low | 0.40 | 15d | 1x `item_crossing_traded_salt` |
| | `event_season_turning_migratory_surge` | Wasteland Game Herd Migration | Low | 0.45 | 18d | None (Positive) |

---

## 2. Anti-Spam Scheduling Budget

1. **Daily Event Budget:** At most **1** new seasonal event may trigger per calendar day across all categories.
2. **Per-Event Cooldown:** Triggered events enter a 10–20 day cooldown window preventing immediate re-triggering.
3. **Active Lifetime:** Active events persist for 4 days or until mitigated by the player.
