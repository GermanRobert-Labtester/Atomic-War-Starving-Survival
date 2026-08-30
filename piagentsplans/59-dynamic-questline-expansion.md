# Plan 59 — Dynamic Questline Expansion (4 → 15 multi-stage questlines)

## Goal (2 lines)
Expand `dynamic_questlines.json` from 4 verified entries to 15 multi-stage questlines.
Each questline has stages with objectives, target locations, and item requirements — the
`QuestlineSystem` is fully implemented but has almost no questlines to run.

## Why (P2)
- Verified: `dynamic_questlines.json` has 4 entries (`quest_dying_signal` and 3 others,
  each with quest_id, title, target_location_id, stages with objectives and
  objective_items). The `QuestlineSystem.cs` is fully implemented in `YearOfAsh/`.
- Creates the quest-content pillar: multi-stage quests give the player directed goals
  beyond survival — investigate, travel, retrieve, decide, and see consequences.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/dynamic_questlines.json` (expand 4 → 15 questlines)
- Read-only: `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` (confirm questline schema:
  quest_id, title, target_location_id, stages with stage number, name, description,
  objective_items, optional flag progression), `Assets/StreamingAssets/Data/questline_master.json`
  (362 quests — confirm the relationship between dynamic questlines and the master quest
  catalog; do not duplicate)

## Content grammar (per questline)
- snake_case `id` with prefix `quest_` (confirmed prefix from existing entries).
- 3-5 stages per questline, each with: stage number, name, description, objective_items
  (items to find/deliver), optional target_location_id, optional flag prerequisite.
- quest_type: investigation / rescue / faction / resource / mystery / engineering / moral.
- target_location_id: `loc_*` id (Plan 32) — where the questline takes place.
- flag_progression: `flag_*` ids that track questline state (produced and consumed —
  validate with dialog-graph lint).
- reward: `item_*` ids, reputation delta, or `knowledge_*` unlock (Plan 34) on completion.
- consequence: world-state change on completion (territory shift, settlement allegiance
  change, resource availability — feeds Plan 44/43).

## Steps
1. Read `QuestlineSystem.cs` to confirm the questline schema, stage resolution, flag
   progression, and reward mechanism.
2. Read the 4 existing questlines to understand the stage structure and avoid duplication.
3. Read `questline_master.json` to avoid duplicating existing quest ids.
4. Author 11 new questlines across 7 types:
   - Investigation (2): a signal traced to a dead station (feeds Plan 50); a missing
     caravan traced to a faction ambush (feeds Plan 45).
   - Rescue (2): a trapped engineer at a power substation (feeds Plan 52 NPC arc); a
     child separated from refugees (feeds Plan 52 child arc).
   - Faction (2): a faction requesting a resource delivery to a contested settlement
     (feeds Plan 44); a faction offering a bounty on a rival patrol leader (feeds
     Plan 40/45).
   - Resource (1): a settlement's water supply is contaminated — find a filter or a clean
     source (feeds Plan 43/56).
   - Mystery (1): a number-station cipher leads to a hidden bunker (feeds existing 11B).
   - Engineering (2): repair a waystation generator (feeds existing 16B); restore a rail
     section for caravan travel (feeds existing 16A).
   - Moral (1): a cache of medicine is found — a nearby settlement also needs it (feeds
     Plan 43 moral dilemma).
5. Give each questline: 3-5 stages, target location, flag progression, reward, consequence.
6. Cross-reference: every `target_location_id` resolves to Plan 32; every `objective_item`
   resolves to `items.json`; every `flag_*` id is produced and consumed (dialog-graph
   lint); every `knowledge_*` reward resolves to Plan 34.
7. Wire 5 questlines to Plan 50 distress signals (the quest begins with a radio signal).
8. Wire 3 questlines to Plan 52 NPC arcs (the questline is an NPC's personal objective).
9. Validate: `--data-integrity-selftest`; run dialog-graph lint for flag reachability;
   confirm a questline progresses through stages in a headless boot.
10. xUnit: questline catalog loads, all references resolve, stages progress in order,
    flag progression works, rewards apply on completion, consequences fire, save
    round-trip preserves questline state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The flag-reachability check (step 9) is the one trap: orphan flags
break quest progression.

## Definition of Done
- `dynamic_questlines.json` has 15 questlines (4 existing + 11 new), all references
  resolving, flag progression valid (no orphans), 5 triggered by distress signals, 3
  linked to NPC arcs, stages progress in order, rewards apply, consequences fire, save
  round-trip green, integrity + tests green.

## Follow-on
- Plan 50 (distress signals) — 5 questlines begin with a radio signal.
- Plan 52 (NPC arcs) — 3 questlines are NPC personal objectives.
- Plan 44 (faction territory) — faction questlines shift territory control.
- Plan 43 (settlements) — resource/moral questlines involve settlement needs.
- Existing 11B (cipher hunts) — mystery questlines feed the cipher-decode loop.
