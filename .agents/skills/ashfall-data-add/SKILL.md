---
name: ashfall-data-add
description: Generates new item/quest/location JSON with correct schema_version, snake_case ID, and CatalogIntegrityValidator pass. For when the AI already knows the data schema.
---

# ASHFALL Data Authoring Assistant

## ROLE

You eliminate the repetitive validation overhead of adding new data. The AI already knows the schema and ID prefix rules — you just generate the JSON and verify it.

## SCOPE

- **Input**: Data type (`item`, `quest`, `location`, `faction`, etc.), ID (e.g., `item_geiger_counter`), fields
- **Output**: JSON file in `Assets/StreamingAssets/Data/`, `schema_version` added, CatalogIntegrityValidator pass
- **Constraints**: `dotnet` + `godot --headless` only; never Unity

## WORKFLOW

### PHASE 1 — Schema Compliance
- Generate JSON with `schema_version` (next integer in domain)
- snake_case ID with known prefix (e.g., `item_`, `quest_`, `loc_`)
- Validate against `CatalogIntegrityValidator` TIER-1/TIER-2 rules

### PHASE 2 — Loader Registration
- Add to the appropriate `*CatalogLoader.cs` (e.g., `ItemCatalogLoader`)
- Verify loader resolves the new ID

### PHASE 3 — Verify
- `godot --headless --path . -- --data-integrity-selftest` (0 errors)
- `dotnet test Ashfall.Core.Tests` (catalog tests pass)

## CONSTRAINTS
- Never invent new ID prefixes — use the master list from `CatalogIntegrityValidator`
- Never hardcode prose — use `ashfall-write` for tone-compliant text
- Always add `schema_version`

## OUTPUT
`docs/data/DATA_ADD_REPORT_<id>.md` — JSON diff, loader touchpoint, validation results

## QUALITY GATE
- CatalogIntegrityValidator 0 errors
- Loader resolves the new ID
- `schema_version` present
