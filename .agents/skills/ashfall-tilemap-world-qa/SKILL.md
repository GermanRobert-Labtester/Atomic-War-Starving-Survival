---
name: ashfall-tilemap-world-qa
description: Validates TileSet/TileMapLayer, physics layers, zone/sector/world-history data, and shelter geography (sky-layer armor, hatch defense) against the JSON data authority. Use when touching world, zones, cartography, or TileSet imports.
---

# ASHFALL Tilemap & World QA

## ROLE
World authority lives in `Assets/StreamingAssets/Data/` (`locations.json`, `world_history.json`, `sector_*`, `zone_*`), rendered via Godot `TileSet` + `TileMapLayer`. You ensure every `loc_*`/`sector_*`/`zone_*` resolves, physics layers match collision expectations, and shelter geography invariants hold.

Mirrors `AGENTS.md:ASSET MIGRATION` TileMap→TileSet row and `WORLD`/`SHELTER` domain reference.

## RULES
1. Data authority first — `TileSet` tiles must reference known `loc_*`/`zone_*` IDs validated by `CatalogIntegrityValidator`; never invent IDs.
2. `dotnet` + `godot --headless` only; compare headless tile queries, not editor screenshots.
3. Read-only audit.

## WORKFLOW
### PHASE 1 — Data-to-Tile Census
- Enumerate `*.tscn` with `TileMapLayer`/`TileSet` resources and their `TileSet` atlas sources in `assets/`.
- Cross-ref `locations.json`, `sector_*.json`, `zone_*.json`: every tile's custom-data `loc_id`/`zone_id` must TIER-1 resolve (200+ prefix gate).

### PHASE 2 — Physics & Layers
- TileSet physics layer count vs project `physics/2d` layers; check collision mask for fallout zones, blast doors, hatch defense, greenhouse, sky-layer armor.
- Verify autotiling terrain sets, navigation layers (if any), and Y-sorting for shelter depth.

### PHASE 3 — Shelter Geography Invariants
- Hatch defense tiles adjacent to blast doors; sky-layer armor above shelter bounds; geological strata / hydro-geology cross-ref with `world_history.json` (no real countries — `DataRuleComplianceTests.cs`).
- Fallout storm pathing, visibility, outdoor radiation tiles within seasonal bounds.

### PHASE 4 — Verify
- `godot --headless --path . -- --data-integrity-selftest` 0 errors
- `godot --headless --path . --quit-after 2` loads world scene without `TileSet` import warnings

## OUTPUT
`docs/world/TILEMAP_WORLD_QA.md` — table: tscn | TileSet | tile count | unresolved IDs | physics layer drift | shelter invariant fails.

## QUALITY GATE
- 0 unresolved `loc_*`/`zone_*`/`sector_*`, 0 physics-layer mismatches, shelter geography invariants green.
