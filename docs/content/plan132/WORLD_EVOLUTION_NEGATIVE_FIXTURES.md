# Plan 132 — World Evolution Negative Fixtures & Integrity Guards

## 1. Overview
This document specifies the validation rules, negative fixtures, and defensive failure boundaries governing `Assets/StreamingAssets/Data/world_evolution_seeds.json` and its consumers (`EvolvingWorldSeeder`, `WildlifeMigrationSystem`, `LandmarkDegradationSystem`, and `LocationEvolutionSystem`).

Every constraint documented here is enforced either at schema/data-integrity load time, via core seeding guards, or within unit tests in `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs` and `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`.

---

## 2. Structural & Topological Integrity Matrix

| Failure Mode | Fixture / Condition | Expected Engine Behavior | Test Coverage Gate |
|---|---|---|---|
| **Null Catalog** | `catalog == null` passed to `EvolvingWorldSeeder.Seed(...)` | Graceful early return, 0 exceptions, no state modified. Helper methods return empty (`ScarcityGoods` -> empty, `ShelterSectorId` -> `""`). | `ScarcityGoods_ShieldedFromEmptyCatalog` |
| **Empty Catalogs** | `catalog.sectors = []`, `catalog.packs = []` | Seeder logs warning, systems remain initialized with empty collections, simulation proceeds safely. | `Seeder_IsIdempotent_AndDeterministic` |
| **Unknown Sector Link** | Sector $A$ specifies neighbor $X$ where $X \notin \text{sectors}$ | Load/test assertion fails: every neighbor must resolve to a valid sector definition. Prevents wandering into undefined sectors. | `SeedCatalog_LoadsGraphPacksLandmarksAndLocations` |
| **Asymmetric Adjacency** | Edge $A \to B$ exists, but $B \to A$ is absent | Test assertion fails. Undirected migration requires bilateral adjacency so packs are not trapped in one-way sinks. | `SeedCatalog_LoadsGraphPacksLandmarksAndLocations` |
| **Graph Partition (Islands)** | Graph split into 2+ disjoint subgraphs | BFS reachability test fails. All 24 sectors must be reachable from `shelter_sector_id` (`sector_4_hinterlands`). | `SeedCatalog_LoadsGraphPacksLandmarksAndLocations` |
| **Self-Loop Adjacency** | Sector $A$ contains $A$ in its `neighbors` array | Test assertion fails. Packs must not migrate to their currently occupied sector during migration step. | `SeedCatalog_LoadsGraphPacksLandmarksAndLocations` |
| **Pack in Unknown Sector** | Pack $P$ has `sector_id` not in `sectors` | Test assertion fails. Wildlife packs must spawn on valid topology nodes. | `SeedCatalog_LoadsGraphPacksLandmarksAndLocations` |
| **Non-Canonical Species** | Pack specifies `species_unknown_mutant` | Test assertion fails against `wildlife_ecosystem.json`. Species must resolve to known ecosystem entries. | `SeedCatalog_LoadsGraphPacksLandmarksAndLocations` |
| **Non-Canonical Location** | Landmark or location seed points to `loc_invalid_slug` | Test assertion fails against `locations.json`. All 30 landmarks and 40 locations must exist in master data. | `SeedCatalog_LoadsGraphPacksLandmarksAndLocations` |
| **Invalid Threat Token** | Location seed lists `"threat_alien_invasion"` | Engine ignores or flags invalid threat strings; only `threat_rad_squatters` and `threat_wild_beasts` are simulated. | `Threats_SproutOnDormantGround_AndDecay` |
| **Duplicate Sector/Pack IDs** | Two records share `sector_id` or `pack_id` | Dictionary key collisions or catalog integrity validator fails. Unique keys strictly required. | `CatalogIntegrityValidator` / `DataIntegrityTests` |

---

## 3. Negative Scenarios & Verification

### 3.1 Waterway Corridor & Aquatic Migration Guard
- **Risk:** If a `CoastalRunner` species (`species_mirror_carp`, `species_gray_heron`) migrates into a water sector without adjacent water neighbors, the pack becomes permanently trapped on dry ground or stranded.
- **Negative Scenario:** A water sector disconnected from the regional aquatic chain or bordered only by land sectors.
- **Enforced Topology:** The 4-sector waterway chain (`sector_4_river` $\leftrightarrow$ `sector_8_estuary` $\leftrightarrow$ `sector_8_reed_flats` $\leftrightarrow$ `sector_8_deep_shelf`) forms a contiguous water channel. In `FishRun_NeverStandsOnDryGround`, a 360-day simulation proves aquatic species never migrate onto non-water sectors.

### 3.2 Collapse & Starvation Invariants
- **Extinction Resistance:** Under extreme multi-year degradation (360 days of consecutive black rain and heavy harvest pressure), `wild.GetGlobalPopulationRatio()` must remain $> 0.05$. The remnant pair harvest floor ($2$ units) guarantees species survival.
- **Integrity Floor:** Landmark structural integrity is clamped within $[0.0, 100.0]$. Multiple collapses cannot refire once `isCollapsed == true` and `collapseDay` is set.
- **Contamination Saturation:** Location contamination clamps within $[0.0, 1.0]$. Crossing $1.0$ sets `isRuined = true`, which is sticky and irrevocable.

---

## 4. Maintenance Guidelines
When adding future sectors or packs:
1. Always add reverse adjacency edges simultaneously.
2. Verify with `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs`.
3. Check `wildlife_ecosystem.json` before introducing any new species ID.
4. Check `locations.json` before referencing any new location ID.
