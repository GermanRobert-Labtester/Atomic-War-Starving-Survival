# Plan 76 — New Destination Roster & Balance Matrix

8 new destinations (55 → 63), one per audited concept gap. Distinct profiles; no two share a family + role + risk shape.

| ID | Name | Family | D | Dgr | p/tick | Stam | Cum. P(enc) | Scavenging table | Role & loot identity |
|---|---|---|---:|---:|---:|---:|---:|---|---|
| `loc_vulcan_works_chemical` | Vulcan Works Chemical Plant | Industrial | 7 | 7 | 0.20 | 3.0 | 0.79 | `table_loot_chemical_plant` (reuse) | Chemicals/solvents/PPE; extreme environmental hazard, modest hostile pressure |
| `loc_north_freight_yard` | North Freight Yard | Industrial | 9 | 5 | 0.20 | 2.8 | 0.87 | `table_loot_rail_yard` (was unused) | Mechanical/route salvage; caravan-investigation hook |
| `loc_blackridge_ammunition_depot` | Blackridge Ammunition Depot | Military | 11 | 8 | 0.24 | 3.2 | 0.95 | `table_loot_military_depot` (was unused) | Ammo/powder/radios at depot scale; discovery-gated; not a guaranteed ammo tap |
| `loc_birchline_weather_station` | Birchline Weather Station | Scientific | 13 | 5 | 0.10 | 3.8 | 0.75 | `table_loot_weather_station` (new) | Instruments/records; the cost is distance + stamina, not combat (lowest p/tick added) |
| `loc_west_ridge_survey` | West Ridge Survey Camp | Scientific | 12 | 6 | 0.18 | 3.4 | 0.91 | `table_loot_geological_survey` (new) | Survey gear/maps/drill parts; distinct from weather station (structural hazard, mid combat) |
| `loc_mirrim_forest_edge` | Mirrim Forest Edge | Wilderness | 9 | 7 | 0.20 | 3.2 | 0.87 | `table_loot_forest_edge` (new) | Timber/forage under contamination; radiation hazard identity |
| `loc_marsh_hollow` | Marsh Hollow | Wilderness | 11 | 6 | 0.16 | 3.6 | 0.85 | `table_loot_frozen_wetland` (new) | Water/fish/plant resources; cold+travel identity, quiet but draining |
| `loc_charcoat_burns` | The Charcoat Burns | Wilderness | 6 | 5 | 0.14 | 2.6 | 0.60 | `table_loot_burned_woodland` (new) | Charcoal/salvage; the safe-ish wilderness run — lowest cumulative pressure of the new set |

## New scavenging tables (5)

`table_loot_weather_station`, `table_loot_geological_survey`, `table_loot_forest_edge`, `table_loot_frozen_wetland`, `table_loot_burned_woodland` — each with distinct `location_type`, hazard profile (`radiation` / `structural` / `cold`), and ≥ 6 weighted entries using existing item IDs only. Reused 3 existing-but-unused tables rather than authoring duplicates.

## Family balance after expansion (63 total)

- Urban/industrial/settlement dense (as before) — now with true chemical-plant and rail-yard identities.
- Scientific: observatory, ministry bunker, **weather station, survey camp** — a genuine mid-distance scientific tier now exists.
- Wilderness: forestry compound + **forest edge, wetland, burns** — three distinct hazard/resource/travel identities (contamination vs. cold vs. fire-scavenging).

## Distinctiveness / no-dominance check (new set)

- No new entry dominates: highest-value targets (depot) carry the highest danger (8) + discovery gate; cheapest trip (Charcoat, cum 0.60) has the narrowest loot identity.
- Weather station vs. survey camp: separated by hazard type (radiation vs. structural), combat pressure (0.10 vs. 0.18), and loot identity (instruments/electronics vs. drill/maps).
- Forest edge vs. burns: contamination vs. fire-scavenging; danger 7 vs. 5; distance 9 vs. 6.
- No monotonic distance=danger=loot ladder: Birchline is far but safe; Vulcan is mid-distance but dangerous.

## Cumulative-encounter sanity

All new entries sit inside the existing data envelope (existing catalog already reaches 0.996 cumulative); none creates new pathological pressure, and the lowest-combat long haul (Birchline, 0.75 over 13 ticks) fills the plan's "expensive through distance, not combat" role.
