# MIGRATION_CORRIDOR_MATRIX.md — Plan 28 / Task 28C

**Authority:** `world_evolution_seeds.json` `sectors[]` — the same graph Plan 16 geography and
`LocationEvolutionSystem` read. No invisible wildlife-only geography exists or is added.

## The corridor graph (11 sectors, 2 water)

| Sector | Region | Neighbors | Water |
|---|---|---|---|
| sector_4_hinterlands | home (shelter) | hills, floodplain, highway_junction | — |
| sector_4_hills | home | hinterlands, canyon, railway_cut | — |
| sector_4_floodplain | home | hinterlands, river, 8_lowlands | — |
| sector_4_canyon | home | hills, railway_cut, 8_quarries | — |
| sector_4_railway_cut | home | hills, canyon, highway_junction | — |
| **sector_4_river** | waterway | floodplain, 8_estuary | ✅ |
| sector_4_highway_junction | home | hinterlands, railway_cut, 8_bluffs | — |
| sector_8_bluffs | deep | highway_junction, 8_lowlands, 8_quarries | — |
| sector_8_lowlands | deep (agricultural) | bluffs, 4_floodplain, 8_estuary | — |
| **sector_8_estuary** | deep coast | 8_lowlands, 4_river | ✅ |
| sector_8_quarries | deep | bluffs, 4_canyon | — |

## Corridor reading per archetype

| Pattern | Corridor behavior | Bound |
|---|---|---|
| Herd (goat) | hills ⇄ canyon ⇔ quarries high country; drops to hinterlands/floodplain when starved | graph walk |
| Burrower (rat, hare) | lowlands ⇄ floodplain ⇄ hinterlands — grain-following drift toward the shelter | graph walk |
| Sounder (boar) | floodplain ⇄ lowlands ⇄ river edge; mast pull to bluffs in The Turning | graph walk |
| Flock (crow, gull) | railway_cut ⇄ estuary ⇄ lowlands — passage lines | graph walk |
| Fish run (carp, heron) | **river ⇄ estuary only** (`water: true` pair) | `FilterNeighbors` |
| Moth bloom | drifts lowlands → floodplain → hinterland edge (crop country) | graph walk |
| Resident predators | any sector | graph walk |

## Validation

- Every neighbor link → known sector; every pack → known sector
  (`--evolving-world-selftest` steps 2–3; `SeedCatalog_LoadsGraphPacksLandmarksAndLocations`).
- Water-bound runners pinned to the waterway pair over a 360-day run
  (`WildlifeSeasonalCalendarTests.FishRun_NeverStandsOnDryGround`).
- No orphan nodes: every seeded pack stands in a linked sector (assert in both gates).
- Aquatic routing never crosses dry ground: neighbors are filtered at move time, and a
  stranded runner (legacy save) walks toward the nearest water rather than teleporting.
