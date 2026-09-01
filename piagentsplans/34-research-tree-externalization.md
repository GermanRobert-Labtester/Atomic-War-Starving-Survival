# Plan 34 — Research Tree Externalization (15 hardcoded → research_catalog.json 40 nodes)

## Goal (2 lines)
Create `research_catalog.json` for `ResearchSystem` — today 15 knowledge nodes are hardcoded
in C# with no JSON tech tree (verified: file missing, a data-authority invariant violation).
Externalize the 15, then expand to 40 nodes covering medicine, engineering, agriculture,
communications, and shelter technology.

## Why (P1)
- Verified: `ResearchSystem.cs` hardcodes 15 knowledge nodes in C#; `research_catalog.json`
  does not exist — a JSON-authority invariant violation (Invariant 6).
- `ResearchSystem` is fully implemented, save-supported, and tick-registered; the tech tree
  is the only missing layer.
- Research unlocks are referenced by Plan 04 (relic blueprints), Plan 22 (foundry/greenhouse),
  and Plan 141's downstream bridge — but there is nothing to unlock because the tree is empty.

## Files to touch
- `Assets/StreamingAssets/Data/research_catalog.json` (CREATE — new catalog, ~40 entries)
- Read-only: `Assets/Ashfall.Core/Research/ResearchSystem.cs` (confirm node schema:
  id, display name, prerequisites, research cost, unlock fields, category)
- `NEW SYSTEM JUSTIFICATION REQUIRED`: a loader (`ResearchCatalogLoader.cs`) is needed
  only if `ResearchSystem` does not already load from JSON. Check first:
  `grep -rn "research_catalog\|ResearchCatalog\|LoadResearch" Assets/Ashfall.Core/`.
  If hardcoded, the loader is the one Core change — minimal deserialization, no gameplay logic.

## Content grammar (per node)
- snake_case `id` with prefix `knowledge_` (confirmed prefix in CatalogIntegrityValidator).
- category: medical / engineering / agricultural / communications / shelter / military_salvage.
- prerequisites: list of `knowledge_*` ids that must be researched first (forms a DAG — no
  cycles; validate with a topological sort in the integrity validator if supported, else
  manual review).
- research_cost: labor-hours + materials (existing items) + optional `skill_*` prerequisite
  (links to Plan 33).
- unlock: recipe id unlock, item id unlock, or system modifier (e.g. +filter efficiency).
- grounded: chelation synthesis, diesel refinement, hydroponic nutrient formulation, shortwave
  repair, air-filter reconditioning, structural reinforcement, dosimeter calibration, etc.

## Steps
1. Read `ResearchSystem.cs` end-to-end: extract the node schema, the prerequisite-resolution
   logic, the research-cost application, and whether loading is JSON-wired or hardcoded.
2. Inventory all 15 hardcoded nodes; map their prerequisite chains and unlock effects.
3. Create `research_catalog.json` with `schema_version: 1`, porting all 15 existing nodes with
   byte-identical costs and unlocks (no balance changes — pure externalization first).
4. If a loader does not exist: create `ResearchCatalogLoader.cs` using
  `SystemTextJsonSerializer`, following an existing loader pattern. Wire into `GameBootstrap`.
5. Remove the 15 hardcoded definitions only after JSON loading is confirmed.
6. Expand to 40 nodes: add 25 new nodes across 6 categories, forming 3-tier trees per category
   (foundation → applied → advanced). Each advanced node unlocks a recipe or item from an
   existing catalog (cross-reference `items.json`, `recipes.json`, `pharma_recipes.json`).
7. Validate the prerequisite DAG: no cycles, no orphan prerequisites, every `knowledge_*`
   id resolves, every unlock target resolves (TIER-1/TIER-2).
8. xUnit: research catalog loads, prerequisite resolution works, research progress saves and
   round-trips, unlock effects apply to downstream systems.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — same as Plan 33: one mechanical loader + hardcoded-definition removal. The DAG
validation is the new hazard (cycles break the research queue). Mitigated by step 7.

## Definition of Done
- `research_catalog.json` exists with 40 nodes, all `knowledge_*` ids resolving, no
  prerequisite cycles, loader wired, hardcoded definitions removed, research progress saves
  and round-trips, integrity + tests green.

## Follow-on
- Plan 33 (skills) feeds research prerequisites (`skill_*` gates `knowledge_*`).
- Plan 04 (relic blueprints) unlocks via advanced research nodes.
- Library manuals (Plan 80, `library_manuals.json` = 3 broken) unlock research branches.
