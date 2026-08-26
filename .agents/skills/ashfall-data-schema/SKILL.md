---
name: ashfall-data-schema
description: Sweeps ASHFALL's ~280 data-authority JSON files to add missing schema_version, normalize camelCase to snake_case with migration notes, and gate regression through CatalogIntegrityValidator. Data hygiene without forking authority.
---

# ASHFALL Data Schema Normalizer

## ROLE

`Assets/StreamingAssets/Data/` is the single data authority, but only ~35 of ~280 JSON files carry `schema_version`, and property casing mixes camelCase/snake_case. You bring the corpus to the documented standard — mechanically, safely, reviewably.

## RULES
1. `Assets/StreamingAssets/Data/` is authoritative — never fork data per engine; ScriptableObjects are generated conveniences.
2. IDs are snake_case with known prefixes (`item_`, `loc_`, `faction_`, `trait_`, `quest_`, `recipe_`, `event_`, `npc_`, `affliction_`, `expansion_`, `encounter_`, `radio_`, `echo_`, `flag_`, `skill_`, `knowledge_`, `ending_`, `article_`, `sector_`, `zone_`, …). Never invent new prefixes.
3. Every file must end with a green `godot --headless --path . -- --data-integrity-selftest` (0 errors).
4. Loaders (10 `*CatalogLoader.cs` files remain in Core) read these files — property renames require matching loader changes in the SAME task or you break loading.

## WORKFLOW

### PHASE 1 — Census
- Enumerate all data JSON; record: has `schema_version`?, casing style, loader that consumes it, `CatalogIntegrityValidator` status.

### PHASE 2 — schema_version Sweep
- Add `schema_version` to every file missing it, choosing the next integer consistent with sibling files in the same domain; add a `migrations` note only where a real migration exists.
- Batch by domain (crossing_*, dose_*, deep_lore_*, etc.), one commit-sized batch per domain.

### PHASE 3 — Casing Normalization
- Per file: list camelCase keys → snake_case targets; verify every key against loader code and cross-file reference keys (TIER-2 keys like `resultItemId`, `requiredItemId`).
- Apply rename + loader update atomically. Reference-key renames get extra scrutiny: `CatalogIntegrityValidator` TIER-1/TIER-2 must still resolve.

### PHASE 4 — Verify
- `godot --headless --path . -- --data-integrity-selftest` → 0 errors.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → all green (catalog tests cover many of these files).

## OUTPUT
`docs/data/DATA_SCHEMA_REPORT.md` — census table, per-file changes, loader touchpoints, before/after validator results.

## QUALITY GATE
- 100% of data JSON have `schema_version` at task end (or explicit owner-approved exceptions).
- Data-integrity selftest 0 errors; full test suite green; no reference key left dangling.
