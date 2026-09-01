# Plan 72 — Utility AI Actions Expansion (6 → 20 actions)

## Goal (2 lines)
Expand `utility_actions.json` from 6 verified entries to 20. The utility AI system
governs survivor autonomous behavior — each action has a priority, weight, tags, and
curve points that determine when a survivor chooses it. 6 actions is too few for
survivors to feel like they make their own decisions.

## Why (P2)
- Verified: `utility_actions.json` has 6 entries (id, displayName, description,
  basePriority, weight, isOverrideAction, tags, curvePoints). The utility AI system
  is in `src/UtilityAI/` (Godot host) and `Assets/Ashfall.Core/UtilityAI/` (Core).
- Creates the survivor-autonomy pillar: survivors should do things on their own —
  repair equipment, cook food, treat wounds, clean, socialize, train — not just wait
  for the player's orders. More actions make the shelter feel inhabited.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/utility_actions.json` (expand 6 → 20 actions)
- Read-only: `Assets/Ashfall.Core/UtilityAI/` (confirm action schema: id, displayName,
  description, basePriority, weight, isOverrideAction, tags, curvePoints; confirm how
  curve points drive priority based on need/state)

## Content grammar (per action)
- snake_case `id` with prefix `action_` (confirmed prefix from existing 6).
- basePriority: 0.0–1.0 — base likelihood of the action being chosen.
- weight: multiplier on the final priority score.
- isOverrideAction: true if this action overrides all others (e.g. fleeing from danger).
- tags: behavioral categories (loud_labor, quiet_labor, medical, social, maintenance,
  training, rest, hygiene, food, water, security, research, crafting).
- curvePoints: how priority changes based on a need/state (hunger → eat; fatigue → rest;
  injury → treat; low morale → socialize; broken equipment → repair).

## Steps
1. Read the utility AI system to confirm the action schema and how curve points drive
   priority.
2. Read the 6 existing actions to understand the structure and avoid duplication.
3. Author 14 new actions across 8 categories:
   - Maintenance (2): repair_equipment (fixes degraded items), clean_shelter (morale +
     hygiene bonus).
   - Medical (2): treat_wounded (applies first aid to injured survivors), self_medicate
     (takes medicine when sick — feeds Plan 112 disease content and Plan 09A response).
   - Food (2): cook_food (uses Plan 55 recipes), preserve_food (extends food shelf life).
   - Water (1): purify_water (uses Plan 55 water recipes).
   - Social (2): socialize (morale bonus for both survivors), resolve_conflict (reduces
     friction between two survivors — feeds existing 12B).
   - Training (2): train_skill (practices a skill — feeds Plan 33), teach_skill (teaches
     a skill to another survivor — feeds Plan 65 final wishes).
   - Security (1): stand_watch (perimeter guard — feeds Plan 57 security incidents).
   - Research (1): conduct_research (advances a research node — feeds Plan 34).
   - Rest (1): rest (reduces fatigue — the most basic survival action).
4. Give each action: basePriority, weight, tags, curve points, description.
5. Cross-reference: every action that uses a recipe references Plan 55 recipe ids; every
   skill action references Plan 33 skill ids; every research action references Plan 34
   knowledge ids.
6. Wire 5 actions to Plan 41 shelter rooms (cook_food requires kitchen; repair_equipment
   requires workshop; conduct_research requires laboratory; stand_watch requires
   armory/surveillance; purify_water requires water treatment room).
7. Wire 3 actions to Plan 33 skills (train_skill and teach_skill reference skill ids).
8. Validate: `--data-integrity-selftest`; confirm survivors autonomously choose actions
   based on curve points in a headless boot.
9. xUnit: action catalog loads, all references resolve, curve points drive priority,
   override actions fire on emergency, save round-trip preserves action state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `utility_actions.json` has 20 actions (6 existing + 14 new), all references resolving,
  5 wired to shelter rooms, 3 wired to skills, curve points drive priority, override
  actions fire on emergency, save round-trip green, integrity + tests green.

## Follow-on
- Plan 33 (skills) — train_skill and teach_skill reference the skill catalog.
- Plan 34 (research) — conduct_research advances research nodes.
- Plan 41 (shelter rooms) — actions require specific rooms.
- Plan 55 (recipes) — cook_food and purify_water use recipes.
- Existing 12B (duty roster) — utility AI actions complement duty-roster assignments.
- Plan 65 (final wishes) — teach_skill feeds the teach_lesson wish type.
