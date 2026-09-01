# Plan 80 — Library Manuals Expansion (3 → 15 study manuals)

## Goal (2 lines)
Expand `library_manuals.json` from 3 verified manuals to 15. The library study
system (`LibraryStudyHostSession.cs` confirmed live) lets survivors study manuals
to gain skill XP, unlock research and knowledge, at the cost of fatigue and
morale. 3 manuals is too few for a knowledge-progression system that should
cover survival, medical, combat, engineering, and science.

## Why (P2)
- Verified: `library_manuals.json` has 3 entries (manual_id, display_name,
  category, study_hours_required, fatigue_per_hour, morale_effect,
  skill_xp_grants, research_unlocks, knowledge_unlocks, prerequisites,
  requires_power). `LibraryManualCatalogLoader.cs` and
  `LibraryStudyHostSession.cs` are confirmed live.
- Creates the knowledge-progression pillar: manuals are how survivors learn
  new skills without expeditions. Studying costs fatigue and morale (sitting
  still in a bunker reading is not free), and the prerequisite chain creates a
  learning tree. 3 manuals cover water filtration, radiation first aid, and
  improvised weapons — the other skill domains are invisible.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/library_manuals.json` (expand 3 → 15 manuals)
- Read-only: `Assets/Ashfall.Core/LibraryManualCatalogLoader.cs` (confirm schema
  and how skill_xp_grants / research_unlocks / knowledge_unlocks resolve)
- `Assets/StreamingAssets/Data/items.json` (if manuals are items, confirm)

## Content grammar (per manual)
- snake_case `id` with prefix `manual_` (confirmed prefix).
- Category: technical / medical / military / scientific / survival / social.
- study_hours_required: 6–20 (time cost to complete the manual).
- fatigue_per_hour: 0.2–0.5 (fatigue accumulated per study hour).
- morale_effect: -0.8 to -0.2 (studying is demoralizing — sitting in a bunker
  reading while the world dies outside).
- skill_xp_grants: 1–2 skill ids (skill_* prefix) — XP granted on completion.
- research_unlocks: 1–2 research ids (research_* prefix).
- knowledge_unlocks: 1–2 knowledge ids (knowledge_* prefix).
- prerequisites: 0–2 manual_ids that must be studied first (creates a learning
  tree — advanced manuals require foundational ones).
- requires_power: true/false (some manuals need powered library/study room).
- Difficulty curve: foundational manuals (no prereqs, low hours) → intermediate
  (1 prereq, moderate hours) → advanced (2 prereqs, high hours, high reward).

## Steps
1. Read `LibraryManualCatalogLoader.cs` to confirm the schema and how
   skill_xp_grants, research_unlocks, and knowledge_unlocks resolve.
2. Read the existing 3 manuals to confirm the prerequisite chain pattern
   (manual_improvised_weapons requires manual_water_filtration).
3. Confirm which skill_*, research_*, and knowledge_* ids exist by grepping the
   relevant catalogs (Plan 33 skills, Plan 34 research).
4. Author 12 new manuals across 6 categories:
   - Technical (2): shelter repair manual, electrical systems manual.
   - Medical (2): field surgery manual, epidemic response manual.
   - Military (2): squad tactics manual, fortification manual.
   - Scientific (2): radiation monitoring manual, soil analysis manual.
   - Survival (3): advanced foraging manual, water purification manual,
     cold-weather survival manual.
   - Social (1): conflict mediation manual (morale management).
5. Each manual: distinct category, study hours, fatigue, morale cost, skill XP,
   research/knowledge unlocks, and prerequisite chain. Build a 3-level learning
   tree: foundational → intermediate → advanced.
6. Cross-reference: every manual_id unique; every skill_xp_grants resolves in
   the skill catalog; every research_unlocks resolves in the research catalog;
   every knowledge_unlocks resolves in the knowledge catalog; every
   prerequisite manual_id exists in this file.
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: library manual catalog loads 15 manuals, all ids unique, all skill/
   research/knowledge/prerequisite ids resolve, no circular prerequisites,
   study_hours and fatigue within valid ranges.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is prerequisite cycles (step 6): confirm no
manual's prerequisite chain forms a cycle (A requires B requires A).

## Definition of Done
- `library_manuals.json` has 15 manuals, all ids resolving, no circular
  prerequisites, integrity + tests green.

## Follow-on
- Plan 33 (skill catalog) — manuals grant skill XP.
- Plan 34 (research tree) — manuals unlock research nodes.
- Plan 71 (power grid rooms) — some manuals require powered study rooms.
- Plan 72 (utility AI actions) — study actions reference manuals.
- Existing 26 (knowledge/research/skills) — this plan provides the manual data.
