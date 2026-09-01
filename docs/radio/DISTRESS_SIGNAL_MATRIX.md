# Distress Signal Matrix

> **Document Status:** Authoritative Distress Signal Catalog & Lifecycle
> **Authority:** Plan 24 (Task 24S–24X)
> **Total Distress Signals:** 26 Authored Signals (5 Baseline + 8 Genuine + 6 Grim + 4 Traps + 3 Mysteries)

---

## 1. Distress Signal Inventory & Lifecycle Specifications

| Signal ID | Frequency (MHz) | Category | Source Name / Voice | Days to Expire | Expedition Target Node | Terminal Outcome Summary |
|---|---|---|---|---|---|---|
| `freq_distress_217_4` | 217.4 | Baseline / Grim | Checkpoint Kilo Automated Beacon | 4 | `loc_checkpoint_kilo` | Armory cache + final memorial log (Corporal Maren). |
| `freq_distress_148_2` | 148.2 | Baseline / Trap | Civilian Bunker 4-East | 3 | `loc_bunker_4_east` | Raider ambush using looped child recording. |
| `freq_distress_108_9` | 108.9 | Baseline / Grim | Sector 9 Electrical Substation | 5 | `loc_sector_9_substation` | High-voltage transformer cache; technician died of radiation. |
| `freq_distress_134_5` | 134.5 | Baseline / Rescue | Relay 44 Bunker SOS | 2 | `loc_relay_44_bunker` | Trapped radio technician `survivor_elena_vasquez` joins shelter. |
| `freq_distress_162_1` | 162.1 | Baseline / Rescue | Marsh Water Caravan Distress | 4 | `loc_marsh_caravan_wreck` | 2 surviving caravaners rescued; water cargo recovered. |
| `freq_distress_77_3` | 77.3 | Genuine Rescue | Meridian Cold Store (Pavel) | 5 | `loc_meridian_cold_store` | Shift supervisor Pavel rescued + seed potato stock secured. |
| `freq_distress_162_8` | 162.8 | Genuine Rescue | Barge *Olenka* Drift Signal | 4 | `loc_river_barge_olenka` | Stranded boatman family rescued; dog + river charts. |
| `freq_distress_93_4` | 93.4 | Genuine Rescue | Collapsed Cellar — Grange 6 | 3 | `loc_grange_6_cellar` | Two trapped teenage farmworkers rescued; grain sack loot. |
| `freq_distress_115_2` | 115.2 | Genuine Rescue | Besieged Waystation Echo | 2 | `loc_waystation_echo` | Scavenger merchant under siege; repels raiders, opens trade discount. |
| `freq_distress_124_7` | 124.7 | Genuine Rescue | Field Medic Post Omicron | 3 | `loc_field_medic_post` | Field medic `survivor_dr_tomas_araujo` rescued with antibiotics. |
| `freq_distress_138_9` | 138.9 | Genuine Rescue | Maintenance Vault Sump Team | 4 | `loc_sump_pump_station` | Two hydraulic mechanics saved from flooding; pump parts recovered. |
| `freq_distress_152_4` | 152.4 | Genuine Rescue | Scavenger Pair — Broken Axle | 3 | `loc_highway_overpass_axle` | Veteran scavenger pair rescued; vehicle repair kit rewarded. |
| `freq_distress_89_6` | 89.6 | Genuine Rescue | Displaced Family — Rail Tunnel | 4 | `loc_rail_tunnel_blind` | Elderly couple and child rescued; joins shelter or sent to settlement. |
| `freq_distress_82_1` | 82.1 | Grim / Late | Old Quarry Excavator Cab | 3 | `loc_granite_quarry_cab` | Too late: operator frozen; diesel canister and family letter recovered. |
| `freq_distress_97_8` | 97.8 | Grim / Late | Grain Elevator Silo 3 | 4 | `loc_grain_silo_3_ruin` | Looping tape: trapped scavengers suffocated; grain dust hazard. |
| `freq_distress_105_6` | 105.6 | Grim / Late | Sub-Basement Pharmacy Clinic | 2 | `loc_ruined_pharmacy_basement`| Overrun by radiation sickness; expired meds + sorrowful log. |
| `freq_distress_119_3` | 119.3 | Grim / Late | Collapsed Highway Culvert | 3 | `loc_highway_culvert_tomb` | Burial site of courier team; sealed dispatch pouch with trade tokens. |
| `freq_distress_144_1` | 144.1 | Grim / Late | Burned Waystation Redoubt | 4 | `loc_waystation_redoubt_ash` | Destroyed by ash fire; steel strongbox requires blowtorch. |
| `freq_distress_156_5` | 156.5 | Grim / Late | Abandoned Transmitter Mast | 5 | `loc_transmitter_mast_ridge`| Solar beacon transmitting over skeleton of lone engineer. |
| `freq_distress_91_8` | 91.8 | False Trap | "Injured Courier" Toll Trap | 2 | `loc_toll_ambush_defile` | Toll Syndicate ambush in narrow pass; combat encounter. |
| `freq_distress_103_4` | 103.4 | False Trap | "Free Antibiotics" Bait Depot | 3 | `loc_decoy_medical_depot` | Raider gang lure; tactical combat; defeat yields ammo and scrap. |
| `freq_distress_127_6` | 127.6 | False Trap | Rigged Military Beacon | 2 | `loc_rigged_military_beacon` | Tripwire-rigged crate; demolitions check to disarm for high loot. |
| `freq_distress_141_2` | 141.2 | False Trap | Decoy S.O.S. Transponder | 4 | `loc_decoy_radio_tower` | Automated pirate decoy drawing patrols into crossfire. |
| `freq_distress_67_8` | 67.8 | Mystery | Deep Geological Beacon Omega | 7 | `loc_deep_fissure_vault` | Pre-war seismic monitoring station transmitting unbroken pulse. |
| `freq_distress_131_0` | 131.0 | Mystery | Coded Morse Distress (Bunker X) | 5 | `loc_bunker_x_sublevel` | Coded SOS matching Plan 11B cipher dictionary; leads to tech blueprint. |
| `freq_distress_174_5` | 174.5 | Mystery | Intermittent Ionospheric Echo | 6 | `loc_radar_dish_crater` | Ghost transmission bouncing off high-altitude ionized ash layer. |

---

## 2. Universal Distress Lifecycle

```text
[ 1. Intercept Signal on Tuner ] (Player tunes within ±0.5 MHz of frequency)
            |
            v
[ 2. Signal Log Entry Created ] (Days remaining countdown starts)
            |
            v
[ 3. Direction-Finding / Triangulation ] (Optional: narrows uncertainty radius)
            |
            v
[ 4. Expedition Dispatched ] (Party travels to location before expiry)
            |
    +-------+-------+
    |               |
[ Within Window ]  [ Expired / Too Late ]
    |               |
    v               v
Terminal Action   Grim Aftermath (Loot / Memorial / Dead Carrier)
(Rescue/Combat)
```
