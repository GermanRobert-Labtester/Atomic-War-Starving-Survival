# Plan 55 — Crafting Recipe Expansion (39 → 80 recipes)

## Goal (2 lines)
Expand `recipes.json` from 39 verified recipes to 80, covering food preparation, water
purification, medicine synthesis, tool crafting, shelter equipment, ammunition reloading,
and vehicle repair. The `CraftingSystem` is fully implemented but the recipe catalog is
too thin to make crafting a meaningful progression path.

## Why (P2)
- Verified: `recipes.json` has 39 recipes (`craft_bandage`, `purify_water`, etc.);
  `CraftingSystem.cs` is fully implemented. The recipe catalog covers basics but lacks
  mid- and late-game recipes that make skills (Plan 33) and research (Plan 34) meaningful.
- Creates the crafting-progression pillar: early recipes use common materials (cloth →
  bandage); mid recipes require skills (Plan 33) and research unlocks (Plan 34); late
  recipes require rare materials from excavation (Plan 37) and scavenging tables (Plan 46).
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/recipes.json` (expand 39 → 80 recipes)
- `Assets/StreamingAssets/Data/items.json` (add missing `item_*` outputs/ingredients)
- Read-only: `Assets/Ashfall.Core/CraftingSystem.cs` (confirm recipe schema: id,
  recipeName, ingredients, resultItemId, resultAmount, craftingTimeHours,
  requiredStationId, optional skill/research prerequisite fields)

## Content grammar (per recipe)
- snake_case `id` with prefix `craft_` or `recipe_` (confirm accepted prefix — existing
  recipes use `craft_`).
- ingredients: list of { itemId, amount } — every `item_*` id must resolve.
- resultItemId: `item_*` id — the crafted output.
- requiredStationId: existing station id (workbench, kitchen, lab, foundry, workshop —
  confirm which station ids the system accepts).
- optional skill_prerequisite: `skill_*` id (Plan 33) — some recipes require a skill.
- optional research_prerequisite: `knowledge_*` id (Plan 34) — some recipes require a
  research unlock.
- tier: basic / intermediate / advanced — escalating material cost and station requirements.

## Steps
1. Read `CraftingSystem.cs` to confirm the recipe schema, the station-id set, and whether
   skill/research prerequisites are supported fields.
2. Read all 39 existing recipes to understand the current coverage and avoid duplication.
3. Read `items.json` to inventory available ingredients and outputs; identify gaps where
   new `item_*` entries are needed for recipe outputs.
4. Author 41 new recipes across 7 categories:
   - Food (10): dried meat, smoked fish, preserved vegetables, flatbread, bone broth,
     fermented cabbage, pemmican, boiled roots, canned stew, ration bars.
   - Water (5): charcoal filter, rainwater collector, solar still, iodine treatment,
     boiling station.
   - Medicine (8): herbal poultice, iodine solution, chelation agent, painkillers,
     antibiotics (requires research), antiseptic, splint, field suture kit.
   - Tools (6): repaired wrench, sharpened knife, hand axe, sewing kit, lockpick set,
     radiation detector (requires research).
   - Shelter equipment (5): air filter cartridge, repaired generator, insulation panel,
     reinforced door, water pipe section.
   - Ammunition (4): reloaded 9mm, reloaded 556, reloaded 12gauge, improvised slug
     (requires skill + station).
   - Vehicle repair (3): engine gasket, tire patch, fuel filter
     (requires skill + Plan 60 vehicles).
5. Assign each recipe: ingredients, output, station, crafting time, tier, and optional
   skill/research prerequisites.
6. Add missing `item_*` entries to `items.json` for recipe outputs that don't exist yet.
7. Cross-reference: every ingredient `item_*` id resolves; every output `item_*` id
   exists; every `skill_*` prerequisite resolves to Plan 33; every `knowledge_*`
   prerequisite resolves to Plan 34; every `requiredStationId` is accepted by the system.
8. Wire 10 advanced recipes to research unlocks (Plan 34) — advanced recipes require a
   `knowledge_*` node to be researched first.
9. Wire 5 intermediate recipes to skill prerequisites (Plan 33) — e.g. ammunition
   reloading requires `skill_reloading`.
10. Validate: `--data-integrity-selftest`; confirm a craft → consume materials → produce
    output loop works in a headless boot for one recipe per category.
11. xUnit: recipe catalog loads, all references resolve, crafting consumes ingredients,
    produces output, skill/research gates block unqualified crafting, save round-trip
    preserves unlocked recipes.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is the skill/research prerequisite fields (step 1): if the
schema doesn't support them, those gates are data-first and the wiring is a follow-on.

## Definition of Done
- `recipes.json` has 80 recipes (39 existing + 41 new), all ids resolving, 10 wired to
  research unlocks, 5 wired to skill prerequisites, crafting loop works end-to-end, save
  round-trip green, integrity + tests green.

## Follow-on
- Plan 33 (skills) — skill-gated recipes make skills meaningful.
- Plan 34 (research) — research-gated recipes make the tech tree meaningful.
- Plan 37 (excavation) — rare materials from deep digs feed advanced recipes.
- Plan 46 (scavenging) — location-specific ingredients feed recipe categories.
- Existing 13A (goods + recipes expansion) — this plan executes that expansion.
