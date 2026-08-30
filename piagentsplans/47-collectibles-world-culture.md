# Plan 47 — Collectibles & World Culture Catalog (40 collectibles)

## Goal (2 lines)
Create `collectibles.json` — 40 collectible items (vinyl records, photographs, posters,
books, magazines, technical manuals, military documents, personal letters, badges,
patches, toys, religious objects, sports memorabilia) that provide morale, knowledge,
recipes, location clues, and faction information when found. These are the pre-war culture
that survived the exchange — the world's lost texture.

## Why (P2)
- Verified: `items.json` has 159 items but only 1 vinyl (`item_vinyl_collection`); no
  dedicated collectibles catalog exists. The world has no cultural artifacts — no evidence
  that a society existed before the collapse.
- Collectibles create the exploration-motivation loop: each location type has unique
  collectibles (schools have textbooks, military depots have documents, apartments have
  photographs) that reward thorough scavenging beyond raw materials.
- Pure DATA work — collectibles are items with special unlock effects.

## Files to touch
- `Assets/StreamingAssets/Data/collectibles.json` (CREATE — 40 collectibles)
- `Assets/StreamingAssets/Data/items.json` (add `item_collectible_*` entries for each
  collectible — they must exist as items to be lootable; the collectibles catalog defines
  their special effects)
- Read-only: `Assets/StreamingAssets/Data/scavenging_tables.json` (Plan 46 — collectibles
  appear as rare/unique entries in location-specific tables), `CatalogIntegrityValidator`
  (confirm `collectible_` or `item_collectible_` prefix is accepted)

## Content grammar (per collectible)
- snake_case `id` with prefix `item_collectible_` (reuse `item_` prefix so it's lootable;
  the collectibles catalog adds the effect layer).
- category: vinyl / photograph / poster / book / magazine / technical_manual /
  military_document / personal_letter / badge / patch / toy / religious_object /
  sports_memorabilia / cultural_artifact / newspaper / map.
- effect_type: morale / knowledge / recipe / location_clue / faction_info / journal_unlock /
  none (purely collectible — no mechanical effect, just world texture).
- effect_value: morale delta, `knowledge_*` id unlocked (Plan 34), `recipe_*` id unlocked,
  `loc_*` id revealed (location clue), `faction_*` info unlocked, `journal_*` entry added.
- rarity: common / uncommon / rare / unique — affects scavenging-table weight (Plan 46).
- description: 1-2 sentences of grounded, human texture (what the object is, who owned it,
  what it meant). No exposition dumps. Skill `ashfall-write`.
- location_type: which scavenging table (Plan 46) this collectible appears in — schools
  have textbooks, military depots have documents, apartments have photographs.

## Steps
1. Read `items.json` to confirm the item schema; collectibles are items with an extra
   effect layer defined in `collectibles.json`.
2. Read `scavenging_tables.json` (Plan 46) to understand how collectibles slot into
   location-specific loot tables as rare/unique entries.
3. Confirm `CatalogIntegrityValidator` accepts the `item_collectible_` prefix (it should —
   `item_` is a known prefix; the `collectible_` sub-namespace is a naming convention).
4. Author 40 collectibles across 16 categories (2-3 per category):
   - Vinyl: a scratched classical album, a propaganda broadcast record, a blues compilation.
   - Photographs: a family portrait, a wedding photo, a military unit photo.
   - Posters: a civil defense poster, a propaganda poster, a concert poster.
   - Books: a field medicine manual, a pre-war novel, a survival handbook.
   - Magazines: a science journal, a hunting magazine, a news weekly.
   - Technical manuals: a diesel engine manual, a radio repair guide, a water treatment
     handbook (these unlock `knowledge_*` research nodes — Plan 34).
   - Military documents: a unit log, a deployment order, a casualty list (faction info).
   - Personal letters: a mother's letter, a soldier's last letter, a rejection letter.
   - Badges/patches: a civil defense badge, a unit patch, a trade guild patch.
   - Toys: a child's doll, a wooden soldier, a music box (pure morale + grief hooks).
   - Religious objects: a prayer book, a rosary, a shrine icon (feeds existing 30A folklore).
   - Sports memorabilia: a team pennant, a match program, a medal.
   - Newspapers: an exchange-day edition, a pre-war local paper, an evacuation notice.
   - Maps: a pre-war road map, a military topo map, a hand-drawn survivor map (location
     clues — reveals `loc_*` entries).
5. Assign each collectible: category, effect type/value, rarity, description, location type.
6. Add 40 `item_collectible_*` entries to `items.json` (weight, stack, value per existing
   conventions) so they're lootable.
7. Wire 15 collectibles into `scavenging_tables.json` (Plan 46) as rare/unique entries in
   the matching location-type tables.
8. Wire 5 technical manuals to unlock `knowledge_*` research nodes (Plan 34).
9. Wire 3 maps to reveal hidden `loc_*` expedition destinations (Plan 32).
10. Validate: `--data-integrity-selftest`; confirm a collectible found via scavenging
    applies its effect (morale, knowledge unlock, location reveal) in a headless boot.
11. xUnit: collectible catalog loads, all `item_*` ids resolve, all `knowledge_*` /
    `recipe_*` / `loc_*` / `faction_*` effect targets resolve, effects apply on pickup,
    save round-trip preserves collected state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is effect-target ids that don't resolve (step 11 prevents
this). Technical manuals unlocking research nodes depends on Plan 34 being done first.

## Definition of Done
- `collectibles.json` exists with 40 collectibles, all `item_*` ids in `items.json`, 15
  wired into scavenging tables, 5 unlocking research, 3 revealing locations, effects
  apply on pickup, save round-trip green, integrity + tests green.

## Follow-on
- Plan 46 (scavenging tables) — collectibles are rare/unique loot entries.
- Plan 34 (research) — technical manuals unlock research nodes.
- Plan 32 (expedition wiring) — maps reveal hidden destinations.
- Existing 17C (codex) — collectibles unlock codex/journal entries.
- Existing 30A (folklore) — religious objects and toys feed the meaning pillar.
- Plan 05 (vinyl catalog, existing) — vinyl collectibles feed the VinylMoraleSystem.
