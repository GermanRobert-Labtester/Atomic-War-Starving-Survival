# Wasteland Region Atlas

**Authority:** `Assets/StreamingAssets/Data/wasteland_map_v1.json`
**Location Catalog:** `Assets/StreamingAssets/Data/locations.json`
**System Coordinator:** `Assets/Ashfall.Core/World/WastelandMapSystem.cs`

---

## 1. Regional Partition Overview

The ASHFALL wasteland is organized into six geographically and environmentally distinct macro-regions. Each region has authored identities, dominant hazards, environmental weather modifiers, chokepoints, and faction presences.

```
       [ Region 6: Northern Treeline & High Scarp ]
                     ▲
                     │ (Mountain Pass / Switchbacks)
                     ▼
[ Region 2: Dead Suburbs ] ◄───► [ Region 1: Crater Core ] ◄───► [ Region 3: Industrial Belt ]
         ▲                                ▲                                 ▲
         │                                │                                 │
         ▼                                ▼                                 ▼
[ Region 6 (West Pass) ]       [ Region 4: Deep Coast ]        [ Region 5: Ash Flats & The Verge ]
```

---

## 2. Region Specifications

### 2.1 Region 1: Crater Core (Ground Zero)
- **Canvas Bounds:** X: [390, 610], Y: [270, 540]
- **Dominant Environmental Hazard:** Extreme Ionizing Radiation (35–80 rads/hr), Magnetic Pulse Anomalies.
- **Atmospheric Weather Risk:** High (0.30–0.60 multiplier).
- **Core Factions:** None (Uninhabited / Automated Defenses & Remnants).
- **Key Chokepoints:** Shelter Gate (`loc_shelter_gate`), Water Station (`loc_water_station`), Fallout Zone Alpha (`loc_cut_radiation_zone_alpha`).
- **Authored Nodes (10):**
  1. `loc_holdfast` — The Holdfast (Player Home Bunker)
  2. `loc_shelter_gate` — Shelter Perimeter Gate
  3. `loc_water_station` — Deep Aquifer Extraction Station
  4. `loc_cut_radiation_zone_alpha` — Ground Zero Fallout Zone Alpha
  5. `loc_excavation_command_vault` — Collapsed Command Vault
  6. `location_the_dead_hand_core` — The Dead Hand Core
  7. `location_magnetic_anomaly_crater` — Magnetic Anomaly Crater
  8. `location_drone_hive_silo` — Automated Drone Hive Silo
  9. `location_automated_mortar_pit` — Hardened Mortar Defense Pit
  10. `location_deep_core_borehole` — Deep Core Geothermal Borehole

### 2.2 Region 2: Dead Suburbs
- **Canvas Bounds:** X: [180, 460], Y: [140, 290]
- **Dominant Environmental Hazard:** Low Background Radiation (6–18 rads/hr), Structural Collapse, Scavenger Ambush Sites.
- **Atmospheric Weather Risk:** Low (0.08–0.18 multiplier).
- **Core Factions:** The Rebuilders (`faction_rebuilders`), The Scale (`faction_the_scale`).
- **Key Chokepoints:** Merchant Caravanserai (`loc_cut_merchant_caravanserai`), Grange Hall (`loc_grange_hall`), Verity Motel (`loc_motel_verity`).
- **Authored Nodes (10):**
  11. `loc_cut_merchant_caravanserai` — Fortified Barter Caravanserai
  12. `loc_grange_hall` — Rebuilders Grange Assembly Hall
  13. `loc_school_gymnasium` — District School Gymnasium Shelter
  14. `loc_conscription_office` — Pre-War Conscription Office
  15. `loc_the_allotments` — Rebuilders Communal Allotment Gardens
  16. `loc_dentists_row` — Commercial District Dentists' Row
  17. `loc_motel_verity` — The Verity Motel (Waystation Verity)
  18. `loc_logistics_reserve_cache` — Sub-Basement Logistics Reserve
  19. `loc_excavation_utility_tunnels` — Utility Tunnel Network
  20. `suburban_house` — Intact Residential Scavenge House

### 2.3 Region 3: Industrial Belt
- **Canvas Bounds:** X: [580, 830], Y: [120, 270]
- **Dominant Environmental Hazard:** Moderate Radiation (20–35 rads/hr), Chemical Leaks, Heavy Rail Chokepoints, Smelter Slag.
- **Atmospheric Weather Risk:** Moderate (0.15–0.30 multiplier).
- **Core Factions:** Silent Foundry (`faction_silent_foundry`), The Cutters (`faction_the_cutters`), The Scale (`faction_the_scale`).
- **Key Chokepoints:** Abandoned Rail Depot (`loc_cut_abandoned_depot`), Railway Span 44 (`loc_railway_span_44_alpha`), Weighbridge (`loc_weighbridge`).
- **Authored Nodes (10):**
  21. `loc_cut_abandoned_depot` — Rail Freight Depot
  22. `loc_cut_arsenal_ruin` — Munitions Arsenal Ruin
  23. `loc_excavation_metro_interchange` — Buried Metro Interchange
  24. `loc_weighbridge` — The Scale Weighbridge Control Point
  25. `loc_diesel_tank_farm` — Tank Farm 4-East Fuel Depository
  26. `loc_railway_span_44_alpha` — Railway Span 44-Alpha (Waystation Span 44)
  27. `loc_transit_authority_hq` — District Transit Authority HQ
  28. `loc_recovery_yard` — Heavy Equipment Recovery Yard
  29. `location_concrete_batching_plant` — Concrete Batching Plant
  30. `location_substation_omega` — High-Voltage Substation Omega

### 2.4 Region 4: Deep Coast
- **Canvas Bounds:** X: [480, 750], Y: [620, 820]
- **Dominant Environmental Hazard:** Saline Inundation, Sump Gases, Submerged Wrecks, Toxic Tidal Sludge (25–55 rads/hr).
- **Atmospheric Weather Risk:** High Coastal Exposure (0.20–0.60 multiplier).
- **Core Factions:** The Black Flotilla / The Fleet (`faction_the_fleet`), The Cutters (`faction_the_cutters`).
- **Key Chokepoints:** Black Flotilla Outpost (`loc_black_flotilla_outpost`), Lock Gate Four (`loc_lock_gate_four`), The Shallows Market (`loc_the_shallows_market`).
- **Authored Nodes (10):**
  31. `loc_black_flotilla_outpost` — Black Flotilla Garrison Pier
  32. `loc_deaddrop_command_shelter` — Maritime Dead-Drop Command Shelter
  33. `loc_cold_store_atlantic` — Atlantic Cold Store Facility
  34. `loc_bathymetric_boat` — Survey Launch Kittiwake
  35. `loc_the_shallows_market` — The Shallows Flotilla Exchange
  36. `loc_drowned_cinema` — The Odeon Drowned Cinema
  37. `loc_lock_gate_four` — Lock Gate Four (Waystation Coast Lock)
  38. `loc_pump_station_nine` — Sump Pump Station Nine
  39. `location_submerged_arcology` — Submerged Luxury Arcology
  40. `location_ash_whale_carcass` — Stranded Ash-Whale Carcass

### 2.5 Region 5: Ash Flats & The Verge
- **Canvas Bounds:** X: [860, 1050], Y: [280, 520]
- **Dominant Environmental Hazard:** Airborne Ash Gale Exposure, Open Sentry Sightlines, Heavy Tolls (15–30 rads/hr).
- **Atmospheric Weather Risk:** Moderate (0.15–0.30 multiplier).
- **Core Factions:** Central Garrison (`faction_central_garrison`), Rebuilders (`faction_rebuilders`), Forward Roster (`faction_forward_roster`).
- **Key Chokepoints:** The Grain Exchange (`loc_grain_silo`), Checkpoint Gamma (`loc_garrison_checkpoint_gamma`), Forward Roster Camp (`loc_forward_roster_camp`).
- **Authored Nodes (10):**
  41. `loc_grain_silo` — The Grain Exchange (Waystation Verge)
  42. `loc_garrison_checkpoint_gamma` — Garrison Fortified Checkpoint Gamma
  43. `loc_forward_roster_camp` — Forward Roster Outpost Camp
  44. `loc_apiary_rows` — Cultivated Apiary Rows
  45. `loc_seed_library_annex` — Seed Library Agricultural Annex
  46. `loc_cider_press` — The Community Cider Press
  47. `loc_terrace_pumphouse` — Agricultural Terrace Pumphouse
  48. `loc_ration_queue_plaza` — District Ration Queue Plaza
  49. `loc_eastern_road` — Eastern Arterial Highway
  50. `loc_neutral_ground` — Neutral Trade Ground

### 2.6 Region 6: Northern Treeline & High Scarp
- **Canvas Bounds:** X: [160, 390], Y: [600, 830]
- **Dominant Environmental Hazard:** Sub-Zero Blizzard Exposure, Scree Avalanches, High Mountain Altitude (20–65 rads/hr).
- **Atmospheric Weather Risk:** Severe Blizzard Risk (0.25–0.65 multiplier).
- **Core Factions:** Cult of Ash Sign (`faction_ash_sign`), Forward Roster (`faction_forward_roster`).
- **Key Chokepoints:** The Switchback Waystation (`loc_shrine_switchback_waystation`), Snowline Station (`loc_snowline_station`), Avalanche Gallery (`loc_avalanche_gallery`).
- **Authored Nodes (10):**
  51. `loc_shrine_switchback_waystation` — The Switchback Waystation (Waystation Switchback)
  52. `loc_snowline_station` — Snowline Patrol Station
  53. `loc_pilgrim_switchbacks` — The Pilgrim Switchbacks
  54. `loc_avalanche_gallery` — Avalanche Protection Gallery
  55. `loc_summit_relay` — Summit Communications Relay Spire
  56. `loc_low_background_lab` — Low-Background Physics Laboratory
  57. `loc_ice_core_store` — Glaciological Ice Core Archive
  58. `loc_the_vessels_cell` — Cult of Ash Sign: The Vessel's Cell
  59. `loc_excavation_mine_shaft` — Industrial Mine Shaft Adit 4
  60. `loc_excavation_archive_bunker` — Pre-War Technical Archive Bunker
