# Plan 65 — Final Wishes Expansion (8 → 30 wishes)

## Goal (2 lines)
Expand `final_wishes.json` from 8 verified entries to 30. The `FinalWishSystem` is fully
implemented and save-supported — each wish is a multi-step personal quest a dying survivor
asks for (teach a lesson, deliver a letter, see a place, reconcile, die with dignity).
The system is live but 8 wishes is far too few for 129 survivors.

## Why (P2)
- Verified: `final_wishes.json` has 8 entries (archetype_id, wish_type, wish_title,
  wish_description, steps). `FinalWishSystem.cs` is fully implemented and uses
  `ISeededRng` (Invariant 4 resolved). This plan is the sole final-wish catalog expansion.
- Creates the death-meaning pillar: final wishes are the emotional core of ASHFALL —
  they make each survivor death feel like a loss, not a stat change. 30 wishes covers
  the full range of human motivation in a dying world.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/final_wishes.json` (expand 8 → 30 wishes)
- Read-only: `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` (confirm wish schema:
  archetype_id, wish_type, wish_title, wish_description, steps with stage/objective/
  consequence)

## Content grammar (per wish)
- archetype_id: matches a survivor archetype from `survivors.json` (the_surgeon, the_old_soldier, the_mother, the_engineer, etc.).
- wish_type: teach_lesson / deliver_letter / see_a_place / reconcile / die_with_dignity / last_meal / confess / protect_someone / return_a_relic / name_a_successor.
- wish_title: 2-5 words, evocative.
- wish_description: 1-2 sentences in the survivor's voice (grounded, exhausted, human).
  Skill `ashfall-write`.
- steps: 2-4 steps, each with an objective (deliver item, visit location, talk to NPC,
  wait for a day) and a consequence (morale boost, skill transfer, guilt reduction,
  journal entry, reputation change).
- tone: cold, exhausted, human, restrained (per AGENTS.md). No sentimentality, no
  preaching — show through the wish itself.

## Steps
1. Read `FinalWishSystem.cs` to confirm the wish schema, step resolution, and consequence
   application.
2. Read the 8 existing wishes to understand the structure and avoid duplication.
3. Read `survivors.json` to inventory survivor archetypes (the 8 existing wishes cover 8
   archetypes; identify which archetypes lack wishes).
4. Author 22 new wishes across 10 wish types:
   - 3 teach_lesson (the nurse teaches field surgery, the mechanic teaches engine
     repair, the hunter teaches trapping — skill transfer to another survivor).
   - 3 deliver_letter (to a settlement, to a named NPC, to a grave — feeds Plan 43/52).
   - 2 see_a_place (a survivor wants to see the coast one last time, a survivor wants
     to visit where their family lived — feeds Plan 32 expedition destinations).
   - 2 reconcile (two survivors who hate each other — the dying one wants to make peace;
     feeds existing 12B friction).
   - 3 die_with_dignity (a survivor refuses to be a burden; a survivor asks for a specific
     death ritual; a survivor asks to die alone — feeds existing 30B mourning).
   - 2 last_meal (a survivor asks for a specific meal from their past — feeds Plan 55
     recipes).
   - 2 confess (a survivor admits a crime, a betrayal, a pre-war secret — feeds
     existing 21C confessions).
   - 2 protect_someone (a survivor asks the player to protect their child, their spouse,
     their student — feeds Plan 52 NPC arcs).
   - 2 return_a_relic (a survivor asks the player to return a stolen item to its
     rightful place — feeds Plan 04 relic blueprints).
   - 1 name_a_successor (a leader asks the player to choose who leads next — feeds
     existing 25A faction life).
5. Give each wish: archetype, type, title, description, 2-4 steps with objectives and
   consequences.
6. Cross-reference: every step objective `item_*` / `loc_*` / `npc_*` id resolves; every
   skill-transfer target resolves to Plan 33.
7. Wire 5 wishes to Plan 52 NPC arcs (protect_someone, reconcile, name_a_successor — the
   wish creates a lasting relationship with a named NPC).
8. Wire 3 wishes to Plan 32 expedition destinations (see_a_place — the wish requires an
   expedition to a specific location).
9. Validate: `--data-integrity-selftest`; confirm a wish triggers, progresses through
   steps, and applies consequences in a headless boot.
10. xUnit: wish catalog loads, all references resolve, steps progress in order,
    consequences apply (morale, skill transfer, guilt, journal), save round-trip
    preserves wish state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `final_wishes.json` has 30 wishes (8 existing + 22 new), all references resolving, 5
  wired to NPC arcs, 3 wired to expedition destinations, wishes trigger and progress,
  consequences apply, save round-trip green, integrity + tests green.

## Follow-on
- Plan 52 (NPC arcs) — protect_someone and reconcile wishes create NPC relationships.
- Plan 32 (expedition) — see_a_place wishes require expeditions.
- Plan 55 (recipes) — last_meal wishes require specific crafted food.
- Plan 06 consumes final-wish outcomes only through existing narrative/relationship surfaces.
- Existing 30B (mourning rites) — die_with_dignity wishes feed mourning content.
- Existing 21C (confessions) — confess wishes feed the confession system.
