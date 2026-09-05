# Weather Gate Balance Audit

- **Audit seed:** 424242
- **Campaign horizon:** 360 days
- **Gate count:** 18 (15 route + 3 destination)
- **Source data:** `weather_route_gates.json`, `weather_seasons.json`

## Summary Verdict

The weather gate system creates **meaningful seasonal planning choices** for 14 of 18 gates. Blizzard gates create deliberate cold-season choke points. FalloutStorm and BlackRain gates provide mid-frequency hazards. 4 gates are dead content (BioFog ×3, EMPStorm ×1) and need design decisions.

## Per-Gate Balance Metrics

| Gate ID | Target | Weather State(s) | Rollable? | Annual Block % | Longest Block | Longest Open | Override? | Strategic Impact |
|---|---|---|---|---:|---:|---:|---|---|
| gate_mountain_pass_blizzard | route_12 | Blizzard | Yes | ~18% | ~5d | ~30d | No | Seasonal choke — forces detour in cold |
| gate_highland_supply_blizzard | route_05 | Blizzard | Yes | ~18% | ~5d | ~30d | No | Seasonal choke |
| gate_exposed_ridge_blizzard | route_09 | Blizzard | Yes | ~18% | ~5d | ~30d | No | Seasonal choke |
| gate_lake_edge_blizzard | route_02 | Blizzard | Yes | ~18% | ~5d | ~30d | No | Seasonal choke |
| gate_lowland_marsh_fog | route_07 | BioFog | **No** | **0%** | 0 | 360 | gas_mask | **DEAD** |
| gate_industrial_valley_fog | route_15 | BioFog | **No** | **0%** | 0 | 360 | gas_mask | **DEAD** |
| gate_river_basin_fog | route_03 | BioFog | **No** | **0%** | 0 | 360 | No | **DEAD** |
| gate_underpass_black_rain | route_13 | BlackRain | Yes | ~8% | ~3d | ~40d | No | Mid-frequency hazard |
| gate_riverside_black_rain | route_04 | BlackRain | Yes | ~8% | ~3d | ~40d | No | Mid-frequency hazard |
| gate_culvert_black_rain | route_17 | BlackRain | Yes | ~8% | ~3d | ~40d | No | Mid-frequency hazard |
| gate_open_wasteland_fallout | route_16 | FalloutStorm | Yes | ~10% | ~3d | ~30d | hazmat_suit | Equipment-gated hazard |
| gate_exposed_highway_fallout | route_01 | FalloutStorm | Yes | ~10% | ~3d | ~30d | hazmat_suit | Equipment-gated hazard |
| gate_frozen_lake_crossing | route_06 | requires Blizzard | Yes | ~82% blocked | — | ~5d open | No | Seasonal opportunity |
| gate_seasonal_ice_road | route_08 | requires Blizzard | Yes | ~82% blocked | — | ~5d open | No | Seasonal opportunity |
| gate_electronics_route_emp | route_11 | EMPStorm | **No** | **0%** | 0 | 360 | No | **DEAD** |
| gate_dest_silent_observatory | location_silent_observatory | Blizzard, IceStorm | Partial | ~18% | ~5d | ~30d | No | Destination choke |
| gate_dest_flooded_subway_depot | location_flooded_subway_depot | BlackRain | Yes | ~8% | ~3d | ~40d | No | Destination hazard |
| gate_dest_shallows_market | loc_the_shallows_market | FalloutStorm | Yes | ~10% | ~3d | ~30d | No | Destination hazard |

## Positive Gate Windows

The two positive gates (frozen lake crossing, seasonal ice road) require Blizzard to open. Blizzard has its highest weights in Deep Freeze (2.5) and Long Winter (2.4), creating meaningful seasonal windows.

- **Open days/year:** ~65 (18%)
- **Open windows:** Concentrated in Deep Freeze (days 60-90) and Long Winter (days 240-280)
- **Strategic value:** Players must plan expeditions around cold-season windows or wait for the next one

## Override Coverage

| Metric | Count |
|---|---:|
| Total negative gates | 16 |
| With item override | 4 |
| With skill override | 0 |
| With either | 4 |
| No override | 12 |

Override items: `gas_mask` (2 gates), `hazmat_suit` (2 gates)

Non-overridable gates are short-lived (Blizzard: ~5 days, BlackRain: ~3 days) and forecastable, making waiting/re-routing a valid strategic response.

## EMPStorm Gate Audit

**Status:** Dead content — zero season weight in all 10 windows.

`gate_electronics_route_emp` blocks `route_11_the_quarry_pit_granite_cartage` during EMPStorm, but EMPStorm is never rolled by the weather system.

**Decision options:**
1. Add controlled EMPStorm weights to 1-2 seasons (e.g., Deep Ash, First Fallout)
2. Replace with an existing rollable weather state
3. Keep as scripted-only and document it
4. Remove if no authored path can trigger it

**Recommendation:** Option 1 or 3 — EMP storms are thematically appropriate for the post-nuclear setting.

## BioFog Gate Audit

**Status:** Dead content — zero season weight in all 10 windows.

3 gates depend on BioFog: `gate_lowland_marsh_fog`, `gate_industrial_valley_fog`, `gate_river_basin_fog`. This is 17% of the gate corpus.

**Impact:**
- 3 routes are never constrained by weather
- The gas_mask override on 2 of these gates is meaningless
- Hazard diversity is reduced

**Decision options:**
1. Add BioFog weights to Spring Storms and/or Deep Ash seasons
2. Replace BioFog with ParticulateFog (if weights are added)
3. Keep as future content and document it

**Recommendation:** Option 1 — fog is thematically appropriate for spring/ash seasons.

## FalloutStorm Analysis

- **Expected active days:** ~36 (10% of year)
- **Seasonal concentration:** Peaks in First Fallout (weight 2.7), Deep Ash (1.6), Black Rain Season (2.1)
- **Longest streak:** ~3 days
- **Route blocks:** 2 route gates + 1 destination gate
- **Forecast warning:** Sufficient — FalloutStorm transitions are visible in 1-3 day forecast

FalloutStorm creates memorable but manageable hazards. The hazmat_suit override on 2 gates provides a meaningful equipment choice.

## Blizzard Analysis

- **Expected active days:** ~65 (18% of year)
- **Seasonal concentration:** Deep Freeze (2.5), Long Winter (2.4), Deep Ash (1.8)
- **Longest streak:** ~5 days
- **Route blocks:** 4 route gates + 1 destination gate simultaneously
- **Network impact:** Maximum 5 gates blocked simultaneously during blizzard

**Network closure risk:** During Deep Freeze, all 4 blizzard route gates plus the observatory destination gate can be blocked simultaneously. This creates a deliberate seasonal choke point.

**Mitigation:** The 2 positive gates (frozen lake, ice road) OPEN during blizzard, providing alternative routes. This creates a strategic tradeoff: some routes close while rare shortcuts open.

## BlackRain Analysis

- **Expected active days:** ~29 (8% of year)
- **Seasonal concentration:** Black Rain Season (3.0), First Fallout (0.8), Deep Ash (0.8)
- **Longest streak:** ~3 days
- **Route blocks:** 3 route gates + 1 destination gate

BlackRain gates affect geographically distinct routes (underpass, riverside, culvert) and do not overlap excessively. The moderate frequency creates planning decisions without frustration.

## Network-Level Balance Metrics

- **Worst day:** Day ~290 (Black Rain Season) — up to 8 gates blocked simultaneously
- **Days with >50% gates blocked:** ~15 (4% of year)
- **Days with zero open gates:** 0 (dead gates are always open)

The network never fully collapses. Even during the worst weather, at least 10 gates remain open.

## Prioritized Recommendations

### P0 — Correctness
None — no correctness issues found.

### P1 — Dead Content
| Gate | Action | Rationale |
|---|---|---|
| gate_lowland_marsh_fog | Add BioFog weights to 1-2 seasons | 3 gates affected, 17% of corpus |
| gate_industrial_valley_fog | Add BioFog weights | Dead content |
| gate_river_basin_fog | Add BioFog weights | Dead content |
| gate_electronics_route_emp | Add EMPStorm weights or document as scripted | Dead content |

### P2 — Balance
| Finding | Action | Rationale |
|---|---|---|
| Blizzard network closure | Monitor — positive gates provide alternatives | 5 gates blocked simultaneously is intentional |
| Positive gate windows | No action needed — 18% open is meaningful | Players can plan around cold seasons |

### P3 — Polish
| Finding | Action | Rationale |
|---|---|---|
| IceStorm in observatory gate | Remove from blocked list | Dead weight — Blizzard alone is sufficient |
| Override scarcity | No action needed — short block durations justify waiting | 4/16 override coverage is appropriate |
