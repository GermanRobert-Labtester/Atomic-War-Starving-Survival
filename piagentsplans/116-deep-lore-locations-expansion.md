# Plan 116 — Deep Lore Locations Expansion (10 → 25 locations)

## Goal (2 lines)
Expand `deep_lore_locations.json` from 10 locations to 25. The deep-lore
location catalog (`DeepLoreLocationCatalogLoader.cs` confirmed live) defines
scavenging destinations with radiation levels, danger levels, travel times,
and per-location loot tables (itemId, minQty, maxQty, spawnChance,
degradationChance, degradedItemId). 10 locations for the entire explorable
world is very thin; this is the primary exploration content catalog.

## Why (P1)
- Verified: `deep_lore_locations.json` has 10 locations. Each has id,
  displayName, radiationUSv, dangerLevel, travelHours, lootTable (array of
  loot entries). `DeepLoreLocationCatalogLoader.cs` loads it; the Maritime
  `VariableLootNode.cs` consumes loot tables.
- This is the single thinnest exploration catalog relative to its
  importance. 10 locations means the player exhausts the scavenging map in
  a few expeditions. The world needs urban, rural, industrial, military,
  scientific, subterranean, and wilderness location families (per the
  master roadmap's location expansion directive).
- Pure DATA work — zero new Core code. The loader consumes the array
  directly.

## Files to touch
- `Assets/StreamingAssets/Data/deep_lore_locations.json` (expand `locations`
  10 → 25)
- Read-only: `Assets/Ashfall.Core/Maritime/DeepLoreLocationCatalogLoader.cs`
  (confirm location + loot entry DTO)
- Read-only: `Assets/Ashfall.Core/Maritime/VariableLootNode.cs` (confirm how
  lootTable entries resolve itemId and spawnChance)

## Content grammar (per location)
- `id`: snake_case, prefix `location_` (confirmed convention).
- `displayName`: evocative location name ("The Municipal Library").
- `radiationUSv`: float, ambient radiation in microsieverts (0.0–80.0+).
- `dangerLevel`: integer 1–5 (hostility/hazard severity).
- `travelHours`: float, one-way travel time from shelter (0.5–8.0).
- `lootTable`: array of loot entries:
  - `itemId`: an item id that resolves in the item catalog (REQUIRED).
  - `minQty` / `maxQty`: integer quantity range.
  - `spawnChance`: 0.0–1.0 probability the item spawns per expedition.
  - `degradationChance` (optional): 0.0–1.0 probability the item is
    degraded.
  - `degradedItemId` (optional): the item id it degrades into if
    degradationChance fires (must resolve).

## Steps
1. Read `DeepLoreLocationCatalogLoader.cs` to confirm the location and loot
   entry DTO and all required vs optional fields.
2. Read `VariableLootNode.cs` to confirm how `itemId`, `spawnChance`, and
   `degradedItemId` resolve, and whether `degradationChance` requires
   `degradedItemId` to be present.
3. Inventory the 10 existing locations: which families are covered
   (library, etc.) and which are missing. Confirm the item catalog has the
   item ids you plan to use as loot.
4. Author 15 new locations across all families:
   - Urban: `location_apartment_block` (food remnants, clothing,
     photographs, contaminated water), `location_metro_station` (transit
     passes, tools, sealed documents, fungal hazard), `location_police_station`
     (firearms, ammunition, evidence lockup, body armor).
   - Industrial: `location_chemical_plant` (chemicals, filters, hazmat
     gear, toxic hazard), `location_steelworks` (steel, tools, fuel,
     furnace slag), `location_power_substation` (electrical parts, copper,
     batteries, EMP risk).
   - Military: `location_ammunition_depot` (ammunition, explosives, UXO
     hazard, military documents), `location_radar_site` (electronics,
     communications gear, military maps, radiation).
   - Scientific: `location_weather_station` (instruments, batteries,
     weather logs, iodine), `location_agricultural_research` (seeds,
     fertilizer, research notes, greenhouse supplies).
   - Subterranean: `location_metro_tunnel` (sealed supplies, transit maps,
     fungal hazard, collapse risk), `location_drainage_network` (tools,
     salvage, contaminated water, disease vector).
   - Wilderness: `location_irradiated_forest` (wildlife, contaminated
     forage, firewood, radiation), `location_frozen_wetland` (ice, dead
     livestock, hunting blinds, frostbite risk), `location_burned_woodland`
     (charred wood, salvage, ash, unexploded ordnance).
5. Each location: 4–8 loot entries, distinct loot identity (no two locations
   share the same primary loot), radiation and danger scaled to the
   location type, travel hours scaled to distance.
6. Cross-reference: every `itemId` resolves in the item catalog; every
   `degradedItemId` (if present) resolves; every `id` unique.
7. Wire 4 locations to Plan 112 (disease catalog — specific locations are
   disease vectors: drainage network, metro tunnel, chemical plant,
   irradiated forest).
8. Wire 3 locations to Plan 48 (weather gates — weather modifies access:
   frozen wetland in blizzard, burned woodland in fallout storm, metro in
   flood).
9. Wire 3 locations to Plan 76 (expedition destinations — new locations
   become expedition targets).
10. Wire 2 locations to Plan 113 (Verdict questlines — cases reference
    investigation sites: weather station, radar site).
11. Validate: `--data-integrity-selftest` (all itemIds and degradedItemIds
    resolve).
12. xUnit: deep lore location catalog loads 25 locations, all ids unique,
    all itemIds resolving, all loot tables non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --expedition-selftest   # new expedition targets
```

## Risk
LOW — pure data. The one trap is `degradedItemId` resolution (step 2): if a
loot entry has `degradationChance` but no `degradedItemId`, the integrity
validator may reject it. Confirm the optional-field coupling before
authoring.

## Definition of Done
- `deep_lore_locations.json` has 25 locations, all ids unique, all itemIds
  and degradedItemIds resolving, all loot tables non-empty, 4 wired to
  disease catalog, 3 to weather gates, 3 to expedition destinations, 2 to
  Verdict questlines, integrity + tests green.

## Follow-on
- Plan 112 (disease catalog) — locations are disease vectors.
- Plan 48 (weather gates) — weather modifies location access.
- Plan 76 (expedition destinations) — new locations become expedition targets.
- Plan 113 (Verdict questlines) — cases reference investigation sites.
- Plan 116 itself can extend to micro-locations (roadside memorials, crashed
  trucks) in a follow-on batch.
