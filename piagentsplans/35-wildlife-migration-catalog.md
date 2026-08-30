# Plan 35 — Wildlife Migration Catalog (system exists, no data)

## Goal (2 lines)
Create `wildlife_migration.json` for `WildlifeMigrationSystem` — the system is fully
implemented, save-supported, and tick-registered, but has **zero data** (verified: file
missing). Add 12 seasonal migration patterns that shift fauna across the map, creating
time-sensitive hunting, trapping, and hazard windows.

## Why (P2)
- Verified: `WildlifeMigrationSystem.cs` exists in Core; `wildlife_migration.json` does not
  exist. The system is wired into `GameBootstrap` and saves state, but has nothing to migrate.
- Creates a renewable, time-based content loop: animals move → hunting windows open/close →
  trapping yield shifts → predator encounters follow herds → food economy swings seasonally.
- Pure DATA work — zero new Core code, zero save changes.

## Files to touch
- `Assets/StreamingAssets/Data/wildlife_migration.json` (CREATE — new catalog, 12 entries)
- Read-only: `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` (confirm migration schema:
  species id, seasonal window, source zone, destination zone, population size, speed,
  predator-follow flag), `Assets/StreamingAssets/Data/locations.json` (zone/region references)
- Check: does `WildlifeMigrationSystem` already have a loader, or does it need one?
  `grep -rn "wildlife_migration\|WildlifeMigration" Assets/Ashfall.Core/` first.

## Content grammar (per migration)
- snake_case `id` with prefix `wildlife_` or `migration_` (confirm accepted prefix in
  CatalogIntegrityValidator before authoring — do not invent a new prefix).
- species: deer_herd, wolf_pack, wild_boar, rabbit_warren, migratory_fowl, irradiated_fauna,
  etc. (grounded; no fantasy creatures — irradiated variants are canon per AGENTS.md).
- seasonal_window: minDay / maxDay (must be ordered — RANGES validation).
- route: source region → destination region (use existing location/zone references).
- population: integer; affects hunting yield and predator-encounter probability.
- predator_follow: boolean — if true, wolf packs trail deer herds, creating combat encounters
  along the migration route.
- hazard: some migrations carry disease (ticks, contaminated water contact) or radiation
  spread (irradiated fauna crossing clean zones).

## Steps
1. Read `WildlifeMigrationSystem.cs` end-to-end: confirm the migration schema, the seasonal
   tick logic, the zone-resolution mechanism, and the save DTO shape.
2. Confirm whether a loader exists; if not, check whether the system accepts JSON at all or
   needs a loader added (same pattern as Plan 33/34 — `NEW SYSTEM JUSTIFICATION REQUIRED`
   only if a loader is needed).
3. Inventory existing location/zone references in `locations.json` to use as migration
   endpoints (do not invent new zones — reuse existing region references).
4. Author 12 migration patterns across 4 seasons (3 per season): spring spawning runs, summer
   highland grazing, autumn deer rut, winter valley descent. Include 2 irradiated-fauna
   migrations that spread contamination along their route.
5. Link 4 migrations to predator-follow (wolf packs, feral dogs) — these generate combat
   encounters on the route during the migration window (feeds Plan 36 trapping + existing
   28A wildlife ecology).
6. Link 2 migrations to disease vectors (tick-borne, water-contact) — feeds existing 09A
   disease system.
7. Validate: `--data-integrity-selftest` (all ids resolve, minDay/maxDay ordered); confirm
   migrations activate on the correct day in a headless boot.
8. xUnit: migration schedule fires on correct day, population moves between zones, save
   round-trip preserves migration state, predator-follow generates encounters.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — data-only if a loader exists; MEDIUM if a loader must be created (but the loader is
mechanical deserialization, no gameplay logic).

## Definition of Done
- `wildlife_migration.json` exists with 12 entries, all ids resolving, seasonal windows
  ordered, migrations fire on schedule in headless boot, predator-follow generates
  encounters, save round-trip green, integrity + tests green.

## Follow-on
- Plan 36 (trapping catalog) consumes migration data for trap-yield modifiers.
- Existing 28A (wildlife ecology) and 13B (hunting loop) use migration windows.
- Irradiated-fauna migrations create contamination events along trade routes (feeds 43).
