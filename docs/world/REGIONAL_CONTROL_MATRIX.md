# Regional Control, Borders & Route Access Matrix

**Authority Catalogs:** `Assets/StreamingAssets/Data/wasteland_map_v1.json`, `Assets/StreamingAssets/Data/foundry_accords.json`
**System Coordinator:** `Assets/Ashfall.Core/World/WastelandMapSystem.cs` / `Assets/Ashfall.Core/FactionWar/FactionWarSystem.cs`

---

## 1. Faction Territorial Hegemony

| Region | Dominant Faction | Controlled Chokepoint Nodes | Transit Toll / Entry Rule |
|---|---|---|---|
| **R1: Crater Core** | None (Automated / Player) | `loc_shelter_gate`, `loc_water_station` | Open to player; dosimeter required for hot zones |
| **R2: Dead Suburbs** | The Scale & Rebuilders | `loc_cut_merchant_caravanserai`, `loc_grange_hall` | 2% Fair Trade barter tariff or trade license |
| **R3: Industrial Belt** | Silent Foundry & The Cutters | `loc_weighbridge`, `loc_railway_span_44_alpha` | Road iron transit token or scrap consignment |
| **R4: Deep Coast** | The Fleet (Black Flotilla) | `loc_black_flotilla_outpost`, `loc_lock_gate_four` | High tide lockage fee or marine salvage permit |
| **R5: Ash Flats & Verge** | Central Garrison | `loc_garrison_checkpoint_gamma`, `loc_eastern_road` | Grain quota passport or military standing >= 0 |
| **R6: High Scarp** | Cult of Ash Sign | `loc_shrine_switchback_waystation`, `loc_snowline_station` | Peace-bonded weapons and paraffin fuel tithe |

---

## 2. Border Closures & Route Bypass Mechanics

1. **Garrison Checkpoint Gamma Blockade:**
   - If the Garrison Grain Tithe Compact is violated, `loc_garrison_checkpoint_gamma` locks. Convoys must divert through `loc_forward_roster_camp` → `loc_apiary_rows` (+12 km detour).
2. **Lock Gate Four Sluice Closure:**
   - If the Saline Corridor Concordat is missed, `loc_lock_gate_four` locks. Coastal convoys must bypass inland via `loc_water_station` (+18 km overland haul).
3. **Switchback Rockfall Seal:**
   - If the Switchback Fuel Accord collapses, `loc_shrine_switchback_waystation` seals. Mountain passage requires scaling through `loc_motel_verity` → `loc_low_background_lab` (+14 km ascent).
