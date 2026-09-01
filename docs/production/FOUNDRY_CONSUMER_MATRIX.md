# Foundry Consumer Matrix (Downstream Traceability)

Every cast product authored in `foundry_production.json` must map to a real in-game consumer system, repair action, construction requirement, or regional trade flow.

---

## 1. Traceability Table (All 25 Products)

| Product ID | Output Item ID | Primary Consumer | Secondary Consumer | Repeatability | In-Game Consequence |
|---|---|---|---|---|---|
| `foundry_prod_plowshare` | `item_foundry_plowshare` | Greenhouse Allotments | Grain Exchange Trade | Continuous | Increases plot tilling yield by 10%; trades for rare seeds |
| `foundry_prod_t_beam` | `item_foundry_t_beam` | Sky Armor Repair | Block C Load Support | Continuous | Restores 25 points of shelter overhead structural integrity |
| `foundry_prod_ice_anchor` | `item_foundry_ice_anchor` | Cutters Road Treaty | Ice Road Maintenance | Quota / Periodic | Fulfills 30-unit quota for `treaty_road_iron_charter` |
| `foundry_prod_winch_drum` | `item_foundry_winch_drum` | Berth 9 Barge Winch | Heavy Shaft Hoist | Quota / Periodic | Fulfills 3-unit quota; enables heavy vehicle expedition recovery |
| `foundry_prod_brine_pipe` | `item_foundry_brine_pipe` | Saltworks Membrane Hall | Water Treatment Facility | Quota / Periodic | Fulfills 4-unit quota for `treaty_brine_pipe_and_iodine_exchange` |
| `foundry_prod_repair_plate` | `item_foundry_repair_plate` | Bulkhead Patching | Air-Lock Armor | Continuous | Repairs breach incidents and fallout micro-fissures |
| `foundry_prod_fastener_bracket` | `item_foundry_bracket_fastener`| Room Expansion Projects | Plumbing System Mounts | Continuous | Required ingredient for workshop benches and bunk tiers |
| `foundry_prod_valve_body` | `item_foundry_valve_body` | Water Purification Main | Salt Evaporator Line | Continuous | Eliminates water distribution leak penalties |
| `foundry_prod_heavy_tool` | `item_foundry_heavy_tool` | Shelter Maintenance Duty | Workshop Crafting Station| Equipment | Boosts repair action speed by 20% |
| `foundry_prod_alloy_part` | `item_foundry_alloy_part` | Generator Turbine Overhaul | Hydro-Pumps Maintenance | Rare / High-Tier | Prevents catastrophic generator failure during brownouts |
| `foundry_prod_defense_plate` | `item_foundry_defense_plate` | Blast Door Reinforcement| Exterior Bunker Hatch | Fortification | Increases raid defense rating and fallout sealing |
| `foundry_prod_roof_armor_plate` | `item_foundry_roof_armor_plate` | Sky Armor Critical Repair | Ordnance Bunker Roof | Continuous | Critical repair item for Plan 19 acid-rain erosion |
| `foundry_prod_shoring_bracket` | `item_foundry_shoring_bracket` | Salt Mine Expansion | Excavation Shaft Bracing | Continuous | Prevents cave-in events during deep extraction |
| `foundry_prod_blast_fitting` | `item_foundry_blast_fitting` | Heavy Air-Lock Rebuild | Hatch Hinge Restoration | Maintenance | Restores rusted blast door cycle efficiency |
| `foundry_prod_reinforcement_shoe`| `item_foundry_reinforcement_shoe`| Bunker Foundation Anchor | Deep Sump Support | Repair | Halts structural subsidence in flooded lower blocks |
| `foundry_prod_structural_coupling`| `item_foundry_structural_coupling`| Power Conduit Channels | Steam Distribution Grid | Continuous | Connects auxiliary generators to distant wings |
| `foundry_prod_replacement_die` | `item_foundry_replacement_die` | Workshop Tool Restoral | Sheet-Metal Former | Maintenance | Restores workshop machine tool wear to 100% |
| `foundry_prod_drill_blanks` | `item_foundry_drill_blanks` | Salt Mine Drill Re-tip | Rock Core Sampler | Tooling | Replaces blunted drill bits (`drillCondition + 0.35`) |
| `foundry_prod_crucible_spare` | `item_foundry_crucible_spare` | Foundry Cupola Re-lining | Secondary Smelter Hearth | Maintenance | Prevents crucible blowout and furnace damage |
| `foundry_prod_press_fitting` | `item_foundry_press_fitting` | Hydraulic Press Rebuild | Pharma Tablet Machine | Maintenance | Restores automated compression throughput |
| `foundry_prod_bearing_housing` | `item_foundry_bearing_housing` | Ventilation Blower Fan | Geothermal Turbine | Maintenance | Fixes screeching blower fans and air-flow drops |
| `foundry_prod_furnace_grate` | `item_foundry_furnace_grate` | Bunker Central Heating | Coal Boiler Firebox | Maintenance | Improves thermal fuel combustion efficiency by 15% |
| `foundry_prod_weather_canister` | `item_foundry_weather_canister`| Atmospheric Dispersal | Skyfall Fallout Scrub | Fictional / Abstract | Canister body for weather mitigation aerosol firing |
| `foundry_prod_cast_shot` | `item_foundry_cast_shot` | Perimeter Deadfall Traps| Sentry Enclosure Defense | Defense | Fictional defensive pellets for automated perimeter traps |
| `foundry_prod_casing_blanks` | `item_foundry_casing_blanks` | Garrison Trade Delivery| Armory Reserve Barter | Trade / Accord | Fulfills Garrison ammunition-casing barter contracts |
