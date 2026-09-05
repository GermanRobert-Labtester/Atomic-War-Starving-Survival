# Weather Gate Content Utilization Audit

- **Audit seed:** 424242
- **Campaign horizon:** 360 days
- **Gate count:** 18 (15 route + 3 destination)
- **Source data:** `weather_route_gates.json`, `weather_seasons.json`, `wasteland_trade_caravan_routes.json`, `expeditions.json`, `items.json`

## Summary Verdict

**4 of 18 gates are dead content** (BioFog ×3, EMPStorm ×1) due to zero season weights for their blocked weather states. The remaining 14 gates have meaningful trigger frequencies across the campaign.

## Per-Gate Utilization

| Gate ID | Target | Mode | Weather | Expected Blocked % | Override | Dead/Rare | Restrictive | Redundant | Orphan |
|---|---|---|---|---:|---|---|---|---|---|
| gate_mountain_pass_blizzard | route_12_... | negative | Blizzard | ~18% | — | — | — | — | — |
| gate_highland_supply_blizzard | route_05_... | negative | Blizzard | ~18% | — | — | — | — | — |
| gate_exposed_ridge_blizzard | route_09_... | negative | Blizzard | ~18% | — | — | — | — | — |
| gate_lake_edge_blizzard | route_02_... | negative | Blizzard | ~18% | — | — | — | — | — |
| gate_lowland_marsh_fog | route_07_... | negative | BioFog | **0%** | gas_mask | **DEAD** | — | — | — |
| gate_industrial_valley_fog | route_15_... | negative | BioFog | **0%** | gas_mask | **DEAD** | — | — | — |
| gate_river_basin_fog | route_03_... | negative | BioFog | **0%** | — | **DEAD** | — | — | — |
| gate_underpass_black_rain | route_13_... | negative | BlackRain | ~8% | — | — | — | — | — |
| gate_riverside_black_rain | route_04_... | negative | BlackRain | ~8% | — | — | — | — | — |
| gate_culvert_black_rain | route_17_... | negative | BlackRain | ~8% | — | — | — | — | — |
| gate_open_wasteland_fallout | route_16_... | negative | FalloutStorm | ~10% | hazmat_suit | — | — | — | — |
| gate_exposed_highway_fallout | route_01_... | negative | FalloutStorm | ~10% | hazmat_suit | — | — | — | — |
| gate_frozen_lake_crossing | route_06_... | positive | requires Blizzard | ~18% open | — | — | — | — | — |
| gate_seasonal_ice_road | route_08_... | positive | requires Blizzard | ~18% open | — | — | — | — | — |
| gate_electronics_route_emp | route_11_... | negative | EMPStorm | **0%** | — | **DEAD** | — | — | — |
| gate_dest_silent_observatory | location_silent_observatory | negative | Blizzard, IceStorm | ~18% | — | — | — | — | — |
| gate_dest_flooded_subway_depot | location_flooded_subway_depot | negative | BlackRain | ~8% | — | — | — | — | — |
| gate_dest_shallows_market | loc_the_shallows_market | negative | FalloutStorm | ~10% | — | — | — | — | — |

## Weather Frequency (360-day realization)

| WeatherKind | Rollable? | Expected Weight Range | Notes |
|---|---|---|---|
| Clear | Yes | 0.2–2.4 | Dominant in False Spring |
| Rain | Yes | 0.2–2.2 | Dominant in Spring Storms |
| Overcast | Yes | 0.7–2.0 | Consistent background |
| Ashfall | Yes | 0.6–2.8 | Dominant in Dry Ash, Deep Ash |
| FalloutStorm | Yes | 0.2–2.7 | Peaks in First Fallout |
| Blizzard | Yes | 0.2–2.5 | Peaks in Deep Freeze, Long Winter |
| BlackRain | Yes | 0.1–3.0 | Peaks in Black Rain Season |
| BioFog | **No** | **0 everywhere** | Never rolled |
| EMPStorm | **No** | **0 everywhere** | Never rolled |
| IceStorm | **No** | **0 everywhere** | Never rolled |

## Dead Content Analysis

### BioFog (3 gates affected)
- `gate_lowland_marsh_fog`, `gate_industrial_valley_fog`, `gate_river_basin_fog`
- BioFog has zero weight in all 10 season windows
- These gates never trigger under normal weather generation
- **Recommendation:** Either add BioFog weights to 1-2 seasons (e.g., Spring Storms, Deep Ash) or replace with a rollable weather state (e.g., ParticulateFog if weights are added)

### EMPStorm (1 gate affected)
- `gate_electronics_route_emp`
- EMPStorm has zero weight in all 10 season windows
- This gate never triggers under normal weather generation
- **Recommendation:** Either add EMPStorm weights to 1-2 seasons or document as scripted-only content

### IceStorm (1 gate partially affected)
- `gate_dest_silent_observatory_blizzard` blocks both Blizzard and IceStorm
- Blizzard is rollable (~18% blocked), so the gate still functions
- IceStorm is dead weight in the blocked list — harmless but misleading
- **Recommendation:** Remove IceStorm from the blocked list or add IceStorm weights

## Redundancy Analysis

No redundant gates found. Every target has exactly one gate.

## Orphan Analysis

No orphan gates found. All 15 route targets resolve to caravan routes, all 3 destination targets resolve to expedition destinations.

## Recommended Actions

| Gate | Severity | Action | Rationale |
|---|---|---|---|
| gate_lowland_marsh_fog | P1 | Add BioFog weights or replace weather | Dead content — 3 gates affected |
| gate_industrial_valley_fog | P1 | Add BioFog weights or replace weather | Dead content |
| gate_river_basin_fog | P1 | Add BioFog weights or replace weather | Dead content |
| gate_electronics_route_emp | P1 | Add EMPStorm weights or document as scripted | Dead content |
| gate_dest_silent_observatory_blizzard | P3 | Remove IceStorm from blocked list | Harmless dead weight |
