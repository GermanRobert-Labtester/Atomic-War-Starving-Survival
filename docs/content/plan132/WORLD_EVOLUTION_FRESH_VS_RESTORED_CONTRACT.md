# Plan 132 — World Evolution Fresh vs Restored Campaign Contract

## 1. Architectural Mission & Invariants
`Assets/StreamingAssets/Data/world_evolution_seeds.json` provides the deterministic initial world state for:
- `LocationEvolutionSystem`
- `WildlifeMigrationSystem`
- `LandmarkDegradationSystem`

**Non-Negotiable Invariant:** The seeder must populate fresh campaigns while strictly protecting restored campaign saves. A loaded save is always authoritative; the seed file must never overwrite, revert, or mutate player history in an ongoing campaign.

---

## 2. The Seeder Contract (`EvolvingWorldSeeder`)

### 2.1 Idempotence & State Detection
`EvolvingWorldSeeder.Seed(LocationEvolutionSystem loc, WildlifeMigrationSystem wild, LandmarkDegradationSystem land, EvolvingWorldSeedContainer catalog)` enforces idempotence:

1. **Topology & Graph Binding:**
   - Adjacency graphs and water sector flags are registered in `wild`.
   - Adjacency is structural definition data (like static map connectivity) and does not store transient player state.
2. **Wildlife Packs:**
   - `wild.RegisterPack(...)` checks if a pack ID already exists in `wild.State.packs`.
   - If present, registration preserves current `population`, `currentSectorId`, `starvationLevel`, and `isRabid`.
   - In a fresh campaign, all 24 packs spawn at their authored `sector_id` and `seededPopulation`.
3. **Landmarks:**
   - `land.RegisterLandmark(...)` checks if a landmark ID already exists.
   - If present, the existing `structuralIntegrity`, `ashBurialCm`, and `isCollapsed` state are preserved.
   - In a fresh campaign, 30 landmarks initialize with their baseline integrities (38.0 to 91.0).
4. **Location Seeds:**
   - For each location seed, if `loc.TryGetRecord(locationId)` already exists (visited, cleared, or mutated), it is left untouched.
   - For unvisited locations, a record is created with initial `currentOwner`, baseline `contaminationLevel`, and initial `activeThreats`.

---

## 3. Fresh Campaign vs Restored Save Lifecycle

```
Fresh Campaign Flow:
  GameBootstrap / World Initialization
      │
      ▼
  EvolvingWorldCatalogLoader.Load() -> catalog (24 sectors, 24 packs, 30 landmarks, 40 locations)
      │
      ▼
  EvolvingWorldSeeder.Seed(loc, wild, land, catalog)
      ├─ Sets 24-sector topology + 4 water sectors
      ├─ Spawns 24 packs with seeded baseline populations
      ├─ Registers 30 landmarks with authored structural integrity
      └─ Initializes 40 location seeds (ownership, contamination, threats)
      │
      ▼
  Simulation Loop (TickDay) begins at Day 1
```

```
Restored Save Campaign Flow:
  Campaign Save Envelope Loaded
      │
      ▼
  RestoreState(locState, wildState, landState) -> Restores player-specific records
      │
      ▼
  EvolvingWorldSeeder.Seed(loc, wild, land, catalog)
      ├─ Re-binds 24-sector topology (guarantees migration graph exists)
      ├─ Existing 24 packs detected in state -> NO reset of population/location
      ├─ Existing 30 landmarks detected in state -> NO reset of integrity/burial
      └─ Existing locations preserved -> NO reset of contamination or threats
      │
      ▼
  Simulation Loop continues identically from Day N
```

---

## 4. Verification Evidence & Proof

1. **Idempotence & Re-seed Proof:**
   - Verified by `Ashfall.Core.Tests.EvolvingWorldActivationTests.Seeder_IsIdempotent_AndDeterministic`:
     - Calling `EvolvingWorldSeeder.Seed(...)` twice produces an identical pack and landmark count (`24` packs, `30` landmarks).
     - Re-seeded records keep their existing live values rather than resetting.
2. **Save/Resume Parity Proof:**
   - Verified by `Ashfall.Core.Tests.EvolvingWorldActivationTests.SaveResume_Parity_ContinuesIdentically`:
     - A world ticked for 10 days, captured, and restored into a twin world runs days 11–60 with 100% identical pack positions, populations, rabies status, landmark integrities, collapse days, and location contamination levels.
3. **Zero Save Envelope Drift:**
   - No fields were added or removed from `LocationEvolutionSaveState`, `WildlifeSaveState`, or `LandmarkSaveState`.
   - Existing checksum hashing and save stores remain 100% compatible.
