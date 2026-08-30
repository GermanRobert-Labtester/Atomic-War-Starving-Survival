# Plan 87 — Relic Recipes Expansion (6 → 15 workshop restoration relics)

## Goal (2 lines)
Expand `relic_recipes.json` from 6 verified relics to 15. The relic restoration
system (`WorkshopReverseEngineeringSystem.cs` confirmed live) defines pre-war
artifacts the player repairs with scavenged components — each relic grants
morale, triggers a narrative event, and sets a world flag. 6 relics is too few
for a workshop-restoration pillar.

## Why (P2)
- Verified: `relic_inks.json` has 6 entries (relic_id, display_name,
  description, required_components, repair_time_hours, morale_bonus,
  dialogue_event_id, restoration_text, world_flag).
  `RelicCatalogLoader.cs` and `WorkshopReverseEngineeringSystem.cs` are
  confirmed live.
- Creates the relic-restoration pillar: relics are the game's morale-recovery
  artifacts — each is a pre-war object the player repairs, triggering a
  narrative moment and a permanent morale boost. 6 relics (gramophone,
  projector, radio, music box, typewriter, camera) is one session of
  discovery; 15 creates a sustained workshop-progression arc.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/relic_recipes.json` (expand 6 → 15 relics)
- Read-only: `Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs`,
  `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` (confirm schema and
  how required_components and world_flag resolve)
- `Assets/StreamingAssets/Data/items.json` (required_components must resolve as
  item ids)

## Content grammar (per relic)
- snake_case `relic_id` (e.g. `gramophone`, `film_projector` — descriptive,
  no prefix).
- description: 1–2 sentences describing the relic's condition and what it was.
- required_components: 2–4 item ids (vacuum_tube, spring_mechanism, etc.) —
  must resolve in items.json. Rarer relics need rarer components.
- repair_time_hours: 2–14 (time cost in the workshop).
- morale_bonus: 2–8 (permanent morale boost on restoration).
- dialogue_event_id: narrative event id triggered on restoration
  (narrative_* prefix).
- restoration_text: 2–4 sentences of prose describing the moment the relic
  works again. Match the existing quality — each restoration is a small,
  human moment.
- world_flag: persistent flag set on restoration (relic_restored_* prefix).
- Diversity: cover entertainment, communication, documentation, domestic life,
  science, and art. No two relics should fill the same emotional niche.

## Steps
1. Read `RelicCatalogLoader.cs` to confirm the schema and how required_components
   and world_flag resolve.
2. Read `items.json` to confirm which component item ids exist; note gaps.
3. Author 9 new relics:
   - Clock (mechanical mantel clock — timekeeping, routine, normalcy).
   - Sewing machine (domestic craft, mending, self-sufficiency).
   - Telescope (stargazing, wonder, perspective on the smallness of human
     suffering).
   - Printing press (small hand-press, pamphlets, the power of the written
     word).
   - Violin (musical instrument, beauty from salvaged wood and wire).
   - Slide projector (educational slides, pre-war science for children).
   - Compass (navigation, orientation, the will to keep going).
   - Kite (child's toy, the sky, a moment of play in a dying world).
   - Coffee grinder (domestic ritual, the smell of something normal).
4. Each relic: distinct required_components, repair time, morale bonus,
   restoration text, and world flag. Match the existing prose quality.
5. Add any missing component item ids to `items.json` (e.g. `clock_spring`,
   `telescope_lens`, `violin_string`, `printing_ink`) — only if a relic's
   required_components reference items that don't exist.
6. Cross-reference: every relic_id unique; every required_component resolves
   in items.json; every world_flag is unique; every dialogue_event_id follows
   existing conventions.
7. Wire 3 relics into Plan 47 collectibles (relics are also collectible items
   found via scavenging).
8. Wire 2 relics into Plan 76 expedition destinations (rare relics found at
   specific expedition sites).
9. Validate: `--data-integrity-selftest` (all ids resolve).
10. xUnit: relic catalog loads 15 relics, all ids unique, all required_components
    resolve, all world_flags unique, morale_bonus and repair_time_hours within
    valid ranges.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is component resolution (step 5): confirm all
required_components exist in items.json before authoring.

## Definition of Done
- `relic_recipes.json` has 15 relics, all ids resolving, 3 wired to collectibles,
  2 wired to expedition destinations, integrity + tests green.

## Follow-on
- Plan 47 (collectibles) — relics are collectible items.
- Plan 76 (expedition destinations) — rare relics found at specific sites.
- Plan 55 (crafting recipes) — relic component crafting.
- Plan 46 (scavenging) — relic components are scavenging loot.
- Existing 04 (relic blueprint expansion) — this plan executes that expansion.
