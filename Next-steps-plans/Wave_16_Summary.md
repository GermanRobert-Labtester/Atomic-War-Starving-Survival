# Wave 16 — Summary (Plans 141–145)

## Wave Overview

Five non-duplicative, implementation-ready plans covering dead-end fixes, missing survival mechanics, cross-system bridges, survivor psychology, and endgame personalization. Each plan addresses a verified gap confirmed by repository inspection and background agent analysis.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 141 — Research → Downstream Unlocks Bridge | Research completions grant items, recipes, upgrades. Fixes dead end where breakthrough items are never granted. | Plans 26/33/34 externalize research data but don't connect to downstream systems. Verified: `ResearchSystem.CompleteResearch` logs breakthrough items but never grants them. | MEDIUM | ResearchSystem, CraftingSystem, ExpeditionSystem, Shelter, Combat, Medical |
| 142 — Clothing & Warmth Gear Progression | Equipped clothing provides warmth bonuses. Fixes gap where `NeedsSystem.ApplyWarmth()` only checks shelter heat source. | Plan 137 connects needs→performance but doesn't fix warmth input. Plan 135 weather cascade doesn't add clothing mitigation. Verified: zero clothing warmth references in NeedsSystem. | LOW | NeedsSystem, Inventory, EquipmentCondition, Expedition, WildlifeTrapping |
| 143 — Medical Afflictions → Quest & Work Bridge | Afflictions gate quests and modify work efficiency. Fixes gap where sick survivors work at full capacity and have same quest options. | Plan 137 needs→performance doesn't address medical. Plan 112 disease catalog adds diseases but not quest/work integration. Verified: zero quest systems query affliction state. | MEDIUM | MedicalPipeline, DutyRoster, Quests, Expedition, Combat, SomaticFlashback |
| 144 — Survivor Autonomy & Initiative | Survivors make independent decisions, help each other, refuse tasks, pursue goals. Fixes gap where survivors are purely reactive instruments. | Plan 132 hidden agendas adds secret motivations but not autonomous behavior. Plan 12 social/shelter life mentions friction but not survivor-initiated actions. Verified: survivors never help/refuse/initiate without player input. | MEDIUM | SurvivorRelations, NeedsSystem, MentalHealth, DutyRoster, SkillProgression |
| 145 — Unified Ending Resolution & Epilogue Personalization | Merges 3 separate ending systems into coherent resolution with personalized prose reflecting specific player choices. | Plans 15/89/96 expand ending content but don't unify systems. Verified: `EpilogueMatrixRuntime` ignores 2 of 8 fields, 12 fixed paragraphs don't reflect faction/moral/survivor choices. | HIGH | HoldfastEndings, EpilogueMatrix, FactionBranch, MoralChoice, SurvivorFate, Verdict |

## Strongest Plan to Implement First

**Plan 141 — Research → Downstream Unlocks Bridge.** It fixes a verified dead end (research breakthrough items are computed but never granted), has the clearest scope (connect existing system to downstream consumers), lowest risk (pure function mapping), and immediate player value (research becomes meaningful). It also creates foundation for Plans 142-143 (research unlocks clothing recipes and medical procedures).

## Dependencies Between the 5 Plans

- **Plan 141 (Research Unlocks) is foundational** — research unlocks clothing recipes (142), medical procedures (143), and shelter upgrades that affect survivor autonomy (144).
- **Plan 142 (Clothing/Warmth) is standalone** but benefits from research unlocks (141) for advanced clothing recipes.
- **Plan 143 (Affliction Bridge) is standalone** but benefits from research unlocks (141) for advanced medical procedures.
- **Plan 144 (Survivor Autonomy) is standalone** but autonomy actions can reference research progress, clothing state, and affliction status.
- **Plan 145 (Unified Ending) depends on all others** — ending personalization references research completed, clothing worn, afflictions suffered, and autonomous actions taken.

## Recommended Implementation Order

1. **Plan 141** — Research → Downstream Unlocks Bridge (dead-end fix, foundational)
2. **Plan 142** — Clothing & Warmth Gear Progression (missing mechanic, builds on 141)
3. **Plan 143** — Medical Afflictions → Quest & Work Bridge (cross-system bridge, builds on 141)
4. **Plan 144** — Survivor Autonomy & Initiative (psychological depth, uses 142/143)
5. **Plan 145** — Unified Ending Resolution & Epilogue Personalization (endgame, uses all others)

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with disconnected systems into one where knowledge has power (research unlocks real capabilities), survival has depth (clothing protects from cold), illness has consequences (afflictions shape what you can do), survivors have agency (they act on their own), and endings have meaning (your specific journey is remembered). Each plan closes a verified gap — dead ends fixed, missing mechanics added, bridges built, autonomy granted, endings personalized — creating a game where every system feeds into every other and every choice echoes to the final screen.
