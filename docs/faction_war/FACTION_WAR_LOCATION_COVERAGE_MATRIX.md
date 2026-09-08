# Faction War Location Coverage Matrix

> **Scope:** Geographic and Sector analysis of all 19 unique locations affected by overrides in Plan 124.

---

## 1. Geographic Sector Distribution

The 20 overrides span 19 distinct wasteland locations situated across core sectors, frontier crossings, deep exploration wilderness, and industrial zones:

| Geographic Sector | Unique Locations | Override IDs | Strategic Narrative Function |
|---|---|---|---|
| **Central Municipal & Civic Hub** | 3 | `loc_override_almshouse_pre_strike`<br>`loc_override_almshouse_post_strike`<br>`loc_override_plaza_cleared`<br>`loc_override_conscription_burned` | Civil order breakdown, soup kitchens, anti-draft riots, municipal collapse. |
| **Industrial & Freight Corridors** | 5 | `loc_override_silo_fortified`<br>`loc_override_weighbridge_barricaded`<br>`loc_override_rail_yard_fortified`<br>`loc_override_factory_occupied`<br>`loc_override_station_reclaimed` | Logistics hubs, bulk grain defense, freight rail yards, chemical manufacturing, metro transit. |
| **High Ridge & Mountain Passes** | 2 | `loc_override_waystation_refugee`<br>`loc_override_understory_transmitter_ambient` | Refugee evacuation route, high-elevation signal relay, radio reconnaissance. |
| **Frontier Roads & Checkpoints** | 4 | `loc_override_checkpoint_occupied`<br>`loc_override_bridge_destroyed`<br>`loc_override_roadblock_liberated`<br>`loc_override_village_abandoned` | River crossings, military road control, militia deadfalls, depopulated rail stops. |
| **Crossing Frontier** | 2 | `loc_override_granary_burned`<br>`loc_override_camp_overrun` | Border refugee petitions, grain store arson, border destabilization. |
| **Subterranean & Deep Wilderness** | 3 | `loc_override_cache_looted`<br>`loc_override_well_contaminated`<br>`loc_override_field_scorched` | Deep fallout zones, contaminated reservoirs, stripped military cache bunkers, scorched deadfall. |

---

## 2. Sector Matrix Table

| Location ID | Display Title | Sector / Region | Overrides Applied | Primary Threat / Transition |
|---|---|---|---|---|
| `loc_almshouse` | St. Jude's Almshouse | Municipal Core | 2 (Pre/Post Strike) | Artillery bombardment |
| `loc_ration_queue_plaza` | Ration Distribution Plaza | Municipal Core | 1 (Pre Strike) | Early civil order |
| `loc_conscription_office` | District Conscription Office | Municipal Core | 1 (Post Strike) | Anti-conscription arson |
| `loc_grain_silo` | Leaning Grain Silo | Sector 4 Industrial | 1 (Post Strike) | Faction fortification |
| `loc_weighbridge` | North Weighbridge | Sector 4 Transit | 1 (Post Strike) | Barricade and toll extort |
| `loc_sector_4_rail_switchyard` | Rail Switchyard | Sector 4 Industrial | 1 (Fortified) | Rail switch redoubt |
| `loc_shrine_switchback_waystation` | Shrine Switchback | High Ridge Pass | 1 (Ambient) | Refugee influx |
| `loc_understory_transmitter` | Understory Mast | High Ridge Pass | 1 (Ambient) | Signals espionage |
| `loc_garrison_checkpoint_gamma` | Garrison Checkpoint Gamma | Border Transit | 1 (Occupied) | Armed military control |
| `loc_settlement_iron_siding` | Iron Siding Settlement | Sector 8 Frontier | 1 (Abandoned) | Civilian flight / freezing |
| `loc_bridge_seven` | Bridge Seven Crossing | River Valley | 1 (Post Strike) | Tactical bridge demolition |
| `loc_ash_militia_deadfall_barrier` | Deadfall Barrier | Sector 4 Scrub | 1 (Liberated) | Militia repulsion |
| `loc_crossing_granary_pledge` | Crossing Granary Post | The Crossing | 1 (Post Strike) | Punitive granary burning |
| `loc_crossing_petition_tent` | Petition Assembly Tent | The Crossing | 1 (Abandoned) | Food riot & camp collapse |
| `location_municipal_water_reservoir` | Municipal Water Reservoir | Deep Lore Perimeter | 1 (Contaminated) | Petrochemical pollution |
| `location_chemical_plant` | Chemical Synthesis Plant | Deep Lore Industrial | 1 (Occupied) | Militia solvent seizure |
| `location_metro_station` | Flooded Metro Station | Deep Lore Urban | 1 (Reclaimed) | Survivor salvage & pumping |
| `location_burned_woodland` | Burned Woodland Basin | Deep Lore Wilderness | 1 (Post Strike) | Phosphorus defoliation |
| `loc_d9_cache_bunker_delta` | D/9 Cache Bunker Delta | Subterranean | 1 (Post Strike) | Vault breach & stripping |
