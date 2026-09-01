# WILDLIFE_MIGRATION_SCHEMA.md — Plan 28 catalog & runtime contract

**Status: authoritative (post-reconciliation).** The retired `wildlife_migration.json`
proposal is documented in `RETIRED_ECOLOGY_ISLAND.md`; this is the one contract.

## 1. The catalog contract (Task 28A verdict)

The migration catalog already existed — **`world_evolution_seeds.json`**
(`EvolvingWorldCatalogLoader`, `Assets/Ashfall.Core/EvolvingWorldCatalog.cs`):

```jsonc
{
  "schema_version": 1,
  "collection_id": "world_evolution_seeds",
  "shelter_sector_id": "sector_4_hinterlands",
  "scarcity_goods": ["canned_food"],
  "sectors": [ { "sector_id": "...", "neighbors": ["..."], "water": false } ],  // water: Plan 28, optional
  "packs":   [ { "pack_id": "...", "species_id": "...", "sector_id": "...", "population": 5 } ],
  "landmarks": [...], "location_seeds": [...]
}
```

- `EvolvingWorldSeeder.Seed` is idempotent and save-safe (restore-then-seed; live-state-first).
- A second migration catalog would fork authority (§1.1, Invariant 6) — **none is authored.**
- Seasonal rhythm is **computed, not stored**: `WildlifeSeasonalCalendar` (pure functions)
  reads the Plan 19 `weather_seasons.json` windows — no wildlife-specific seasons (§1.3).

## 2. Runtime contract (`WildlifeMigrationSystem` + `.Live.cs`)

- Packs: `packId, speciesId, currentSectorId, population, seededPopulation,
  aggressionScore, starvationLevel, isRabid, lastThreatFiredDay` — save schema v1.
- Movement: hunger-driven adjacency walk (`starvation > 0.5`, 25%/day, per-day RNG fork
  `CampaignRngStream.DeriveSeed(master, "world_evolution", 1, day, action)`).
- Population bounds: −1/day above starvation 0.7 (+3%/day rabies); births toward a 2× seed
  ceiling below starvation 0.3 with 3-day breathing room. Collapse/recovery are bounded
  terminal states.
- Water-bound runners (`species_mirror_carp`, `species_gray_heron`) may only move along
  `water: true` sectors; a stranded runner walks toward water (no teleport).
- Season binding: `WorldHostSession.Create` → `Wildlife.BindSeasonProfile(profile)`.
  No binding → factor 1.0 = byte-identical legacy trajectory (pinned by test).

## 3. Species → archetype table (12 species, pinned by test)

| species_id | archetype |
|---|---|
| species_rad_dog / species_wolf / species_dust_lynx | Resident |
| species_feral_goat | HerdGrazer |
| species_blight_rat / species_cotton_hare | BurrowSwarm |
| species_ash_boar | Sounder |
| species_iron_crow / species_ash_gull | PassageFlock |
| species_gray_heron / species_mirror_carp | CoastalRunner |
| species_ghost_moth | SwarmBlight |

Unknown species ids read as `Resident` (never throw, never invent).

## 4. Save behavior

- Calendar holds **no state**; the pack ledger round-trips byte-identically
  (`SaveRestore_WithBoundCalendar_RoundTripsExactly`).
- Season profile is re-bound from `weather_seasons.json` at boot (`WeatherProfileLoader`);
  water flags re-derived from the seeds catalog at seed time.
- Old saves: load unchanged; a fresh boot seeds 13 packs, a restored save keeps exactly
  its own packs (idempotent seeder).
