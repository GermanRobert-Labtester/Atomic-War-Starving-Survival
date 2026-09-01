# Route Risk, Distance & Terrain Balance Analysis

**Authority:** `Assets/StreamingAssets/Data/wasteland_map_v1.json`
**Simulation System:** `Assets/Ashfall.Core/World/WastelandMapSystem.cs` / `Assets/Ashfall.Core/Expedition/ExpeditionSystem.cs`

---

## 1. Route Cost & Parameter Distribution

Across the 202 directed routes (101 bidirectional links):

| Metric | Minimum | Mean | Median | Maximum | Outlier Rule |
|---|---|---|---|---|---|
| **Distance (km)** | 3.0 km | 9.8 km | 9.0 km | 18.0 km | Distance must be > 0 and <= 25.0 km |
| **Weather Hazard Multiplier** | 0.05 | 0.28 | 0.25 | 0.65 | Hazard factor must be between 0.05 and 0.80 |
| **Foot Travel Hours (est.)** | 0.5 hrs | 2.2 hrs | 2.0 hrs | 4.5 hrs | Capped by daily expedition stamina budgets |

---

## 2. Strategic Route Tradeoff Analysis

The 60-node topology was engineered to ensure that no single dominant path trivializes travel between key regions.

### Tradeoff Scenario 1: Holdfast to The Grain Exchange (`loc_grain_silo`)
- **Direct Industrial Path:** `loc_holdfast` → `loc_cut_abandoned_depot` → `loc_cut_arsenal_ruin` → `loc_grain_silo`
  - *Distance:* 37 km
  - *Risk:* Moderate danger (0.30 weather hazard, arsenal ammunition traps).
- **Southern Verge Bypass:** `loc_holdfast` → `loc_excavation_command_vault` → `loc_forward_roster_camp` → `loc_grain_silo`
  - *Distance:* 38 km
  - *Risk:* Lower explosive risk, but requires transit through active Forward Roster patrol territory.
- **Suburban Commercial Detour:** `loc_holdfast` → `loc_cut_merchant_caravanserai` → `loc_grange_hall` → `loc_transit_authority_hq` → `loc_weighbridge` → `loc_grain_silo`
  - *Distance:* 49 km
  - *Risk:* Lowest radiation/weather hazard (0.12–0.20), multiple waystation rest stops, higher food consumption due to length.

### Tradeoff Scenario 2: Holdfast to Black Flotilla Outpost (`loc_black_flotilla_outpost`)
- **High-Risk Crater Cut:** `loc_holdfast` → `loc_cut_radiation_zone_alpha` → `loc_black_flotilla_outpost`
  - *Distance:* 29 km
  - *Risk:* Severe ionizing radiation (75 rads/hr, 0.50 weather hazard).
- **Safe Coastal Lock Bypass:** `loc_holdfast` → `loc_water_station` → `loc_lock_gate_four` → `loc_the_shallows_market` → `loc_black_flotilla_outpost`
  - *Distance:* 39 km
  - *Risk:* Minimal radiation (15 rads/hr), but requires passing through Cutters lock gate toll.

### Tradeoff Scenario 3: Holdfast to High Scarp Summit Relay (`loc_summit_relay`)
- **Pilgrim Switchback Climb:** `loc_holdfast` → `loc_water_station` → `loc_shrine_switchback_waystation` → `loc_snowline_station` → `loc_pilgrim_switchbacks` → `loc_summit_relay`
  - *Distance:* 40 km
  - *Risk:* Steep vertical ascent, blizzard risk (0.40–0.55 weather hazard).
- **Physics Laboratory Approach:** `loc_holdfast` → `loc_cut_merchant_caravanserai` → `loc_motel_verity` → `loc_low_background_lab` → `loc_summit_relay`
  - *Distance:* 47 km
  - *Risk:* Gentler grade, stable sheltered waystations, lower avalanche risk.
