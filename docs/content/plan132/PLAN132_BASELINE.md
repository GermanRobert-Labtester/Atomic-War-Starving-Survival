# Plan 132 — Evolving World Seed Expansion: Forensic Baseline

## 1. Executive Summary
Plan 132 expands `Assets/StreamingAssets/Data/world_evolution_seeds.json` from its initial compact baseline into a comprehensive starting-world substrate supporting regional ecology, wildlife migration, landmark degradation, and location evolution.

This document establishes the empirical baseline, system answers, and catalog inventories prior to expanding the seed file.

---

## 2. Ten Implementation Pre-Flight Determinations

### 2.1 Validation Rules for Identifiers
- **Sector IDs:** Must follow `sector_<zone>_<slug>` format. Referenced by `shelter_sector_id`, sector `neighbors`, pack `sector_id`, and location `sector_id`.
- **Pack IDs:** Must follow `pack_<slug>` format and be unique across the `packs` array.
- **Species IDs:** Must resolve to canonical species defined in `wildlife_ecosystem.json` and recognized in `WildlifeSeasonalCalendar.ArchetypeOf(speciesId)`.
- **Landmark IDs:** Must follow `landmark_<slug>` format and be unique across the `landmarks` array.
- **Location IDs:** Must resolve to valid `id` entries in `Assets/StreamingAssets/Data/locations.json`.
- **Owner Faction IDs:** Must be `"none"` or resolve to canonical factions in `faction_lore.json` / `holdfast_factions.json` / `crossing_factions.json`.
- **Threat IDs:** Must resolve to the engine's recognized active threat constants (`LocationEvolutionSystem.ThreatSquatters` = `"threat_rad_squatters"`, `LocationEvolutionSystem.ThreatWildBeasts` = `"threat_wild_beasts"`).

### 2.2 Adjacency Symmetry
- **Requirement:** **Strictly Symmetric (Undirected).**
- **Rationale:** `WildlifeMigrationSystem.TickDay` reads `_sectorNeighbors[currentSectorId]` and chooses a neighbor. If an edge $A \to B$ exists without $B \to A$, packs would permanently trap themselves in $B$ without the ability to return.

### 2.3 Connected Components
- **Requirement:** The entire graph must form a **single connected component**.
- **Rationale:** The shelter sector (`sector_4_hinterlands`) must be reachable from and connected to all regional sectors. No isolated islands of unreachable wildlife.

### 2.4 Wildlife Migration Routing Mechanics
- **Mechanism:** Migration operates purely across **direct adjacent neighbors** ($1$-hop step per migration event).
- **Waterway Filtering:** For species with `MigrationArchetype.CoastalRunner` (`species_gray_heron`, `species_mirror_carp`), `WildlifeSeasonalCalendar.FilterNeighbors` restricts valid candidate targets to sectors marked `"water": true`.

### 2.5 Scarcity Goods Semantics
- **Consumer Behavior:** In `src/Main.CampaignOwners.cs` (lines 903–926), `EvolvingWorldSeeder.ScarcityGoods(seeds)` is iterated over via `foreach (var g in goods)`, adjusting market demand for each listed good via `RegionalSupplyRouter.WorldShortageDemandScale`.
- **Decision:** The consumer is genuinely plural. Plan 132 expands the list from 1 to 4 grounded survival commodities (`canned_food`, `cooked_meat`, `item_smoked_meat`, `clean_water`).

### 2.6 Landmark Integrity Bounds
- **Representation:** Float in range `0.0` to `100.0`.
- **Decay:** Weather and time apply passive daily weathering (`-0.2/day`) plus ash burial. When integrity reaches $0.0$, the landmark collapses (`isCollapsed = true`).
- **Seeding Rule:** Baselines are distributed across 38.0 to 91.0, avoiding arbitrary 0 or 100 values.

### 2.7 Location Contamination Scale & Semantics
- **Representation:** Normalized float `0.0` to `1.0`.
- **Dynamics:** Increases under hazard weather (`+0.02 * rad_modifier/150`), decays in clear weather (`-0.01/day`).
- **Threshold:** At or above `1.0`, the location becomes permanently ruined (`isRuined = true`).
- **Seeding Rule:** Graded values between `0.02` and `0.55`.

### 2.8 Active Threat Interpretation & Registry
- **Active Tokens:** `"threat_rad_squatters"` and `"threat_wild_beasts"`.
- **Dynamics:** Dormant uncleared locations have a $2\%$ daily chance to sprout a threat; active threats have a $5\%$ daily decay chance.

### 2.9 Ownership Influence Boundaries
- **Scope:** `LocationEvolutionSystem` tracks `currentOwner` and fires `OnLocationOwnerChanged`.
- **Non-duplication:** Seed ownership populates initial unvisited state. Live campaign changes and territory systems remain authoritative.

### 2.10 Existing Test Counts
- **Impact Identified:** `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs` (lines 54–57, 90) previously asserted exact counts:
  - 11 sectors
  - 13 packs
  - 10 landmarks
  - 12 location seeds
- **Resolution:** These assertions will be updated to match the new targets (24 sectors, 24 packs, 30 landmarks, 40 location seeds).

---

## 3. Catalog Inventories

### 3.1 Species Authority (`wildlife_ecosystem.json`)
All 12 canonical species:
1. `species_cotton_hare` (Herbivore / Prey, BurrowSwarm)
2. `species_feral_goat` (Herbivore / Prey, HerdGrazer)
3. `species_blight_rat` (Scavenger / Swarm, BurrowSwarm)
4. `species_ash_boar` (Herbivore / Sounder, Sounder)
5. `species_mirror_carp` (Herbivore / Aquatic, CoastalRunner)
6. `species_ghost_moth` (Herbivore / Nocturnal, SwarmBlight)
7. `species_rad_dog` (Carnivore / Pack Canine, Resident)
8. `species_wolf` (Carnivore / Apex Canine, Resident)
9. `species_dust_lynx` (Carnivore / Apex Feline, Resident)
10. `species_iron_crow` (Scavenger / Avian, PassageFlock)
11. `species_ash_gull` (Scavenger / Coastal Avian, PassageFlock)
12. `species_gray_heron` (Carnivore / Coastal Wader, CoastalRunner)

### 3.2 Canonical Factions for Location Ownership
- `"none"` (Uncontrolled / Wild)
- `"faction_the_scale"` (Trade arbitration / weighbridge logistics)
- `"faction_the_compact"` (Civil administration / agricultural coordination)
- `"faction_the_underwrite"` (Financial risk contracts / salvage claims)
- `"faction_the_office"` (Bureaucracy / archives / records)
- `"faction_the_cutters"` (Coastal salt works / brine harvesting)
- `"faction_the_fleet"` (Deep water salvage / estuary passage)
- `"iron_garrison"` (Martial law / military fortifications)
- `"cult_of_ash_sign"` (Rad-purification asceticism / crater shrines)
