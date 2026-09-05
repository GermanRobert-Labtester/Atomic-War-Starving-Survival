# Standing Record Faction Territory & Geographic Matrix

## 1. Regional Anchors & Coordinates

All 8 Standing Record factions map directly to the authoritative regional schema established in `WASTELAND_REGION_ATLAS.md` (`Assets/StreamingAssets/Data/wasteland_map_v1.json`):

| Faction ID | Display Name | Home Region | Geographic Anchor / Chokepoint | Node Coordinates | Primary Environmental Threat |
|---|---|---|---|---|---|
| `faction_the_overlay` | The Overlay | `all_regions` | Trans-wasteland cadastral survey posts | Omnipresent | Plate weathering, memory rot |
| `faction_the_scale` | The Scale | `industrial_belt` | Weighbridge (`loc_weighbridge`) & Aqueducts | (690, 180) | Chemical runoff, high pressure |
| `faction_the_compact` | The Compact | `dead_suburbs` | Grange Hall (`loc_grange_hall`) & Registry | (320, 180) | Structural collapse, looters |
| `faction_the_underwrite` | The Underwrite | `industrial_belt` | Tank Farm 4-East (`loc_diesel_tank_farm`) | (740, 220) | Fuel vapors, sabotage, heavy fire |
| `faction_the_cutters` | The Cutters | `the_cut` | Railway Span 44 (`loc_railway_span_44_alpha`) | (640, 150) | Black ice, bridge frost-heave |
| `faction_the_fleet` | The Fleet | `deep_coast` | Coastal Berth 9 / Shallows Market (`loc_the_shallows_market`) | (610, 710) | Saline storm surge, bilge gas |
| `faction_the_rebuilders` | The Rebuilders | `ash_flats` | The Grain Exchange (`loc_grain_silo`) & Allotments | (880, 320) | Ash gales, soil toxicity, blight |
| `faction_the_garrison` | The Garrison | `ash_flats` | Fort Karkov & Checkpoint Gamma (`loc_garrison_checkpoint_gamma`) | (940, 390) | Sniper fire, minefields, wire |

---

## 2. Spatial Contiguity & Territorial Overlap

1. **The Industrial Axis (The Scale + The Underwrite + The Cutters):**
   - Centered along the northern heavy rail line and canal network.
   - The Scale meters cooling and process water.
   - The Underwrite fuels and bonds heavy haulers.
   - The Cutters physically chisel the ice and clear wrecked rolling stock.

2. **The Agrarian-Military Frontier (The Rebuilders + The Garrison):**
   - Centered in Region 5 (Ash Flats & The Verge).
   - The Rebuilders control calorie production and grain stores at the leaning silo.
   - The Garrison enforces highway checkposts and extracts tithes for military maintenance.
   - Friction point: grain requisition quotas vs. communal planting reserves.

3. **The Coastal Fringe (The Fleet):**
   - Controls shallow-water logistics connecting the inland canal basin to the flooded coastal ruins.
   - Operates independent of inland faction squabbles, holding monopoly over marine pitch and salvage launches.
