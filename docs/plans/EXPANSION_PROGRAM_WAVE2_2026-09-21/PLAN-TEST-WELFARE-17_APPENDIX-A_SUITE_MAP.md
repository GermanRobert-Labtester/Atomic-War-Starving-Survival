# PLAN-TEST-WELFARE-17 — Appendix A: Test Suite Map

**Generated:** 2026-09-21 from `Ashfall.Core.Tests/**` (1378 files,
109 directory families, 11766 `[Fact]`, 98 `[Theory]`).
**Columns:** directory · files · `[Fact]` · `[Theory]` · total cases ·
TEST-AGGREGATION metadata rows · duration class (fast <5 s / medium <30 s /
slow <180 s — to be measured per family by TW-17B).
**Use:** TW-17B/17C — the map drives duration budgets, split candidates (the
largest families), and fixture consolidation targets. The 15 largest families
by case count are the split-candidate shortlist.

## Suite map (sorted by total cases)

| Directory | Files | Facts | Theories | Total | Agg-meta |
|---|---:|---:|---:|---:|---:|
| `Ashfall.Core.Tests` | 560 | 5705 | 26 | 5731 | 3 |
| `Ashfall.Core.Tests/Shelter` | 87 | 750 | 4 | 754 | 3 |
| `Ashfall.Core.Tests/World` | 65 | 536 | 3 | 539 | 0 |
| `Ashfall.Core.Tests/Survivors` | 57 | 447 | 6 | 453 | 0 |
| `Ashfall.Core.Tests/Medical` | 49 | 428 | 7 | 435 | 0 |
| `Ashfall.Core.Tests/Radio` | 47 | 351 | 3 | 354 | 3 |
| `Ashfall.Core.Tests/Economy` | 41 | 328 | 1 | 329 | 1 |
| `Ashfall.Core.Tests/Expeditions` | 41 | 320 | 2 | 322 | 1 |
| `Ashfall.Core.Tests/Narrative` | 26 | 291 | 2 | 293 | 0 |
| `Ashfall.Core.Tests/Campaign` | 32 | 184 | 3 | 187 | 0 |
| `Ashfall.Core.Tests/Tooling` | 35 | 116 | 1 | 117 | 0 |
| `Ashfall.Core.Tests/Inventory` | 15 | 114 | 0 | 114 | 0 |
| `Ashfall.Core.Tests/Save` | 15 | 102 | 8 | 110 | 1 |
| `Ashfall.Core.Tests/UI` | 18 | 103 | 0 | 103 | 0 |
| `Ashfall.Core.Tests/Endgame` | 11 | 99 | 2 | 101 | 0 |
| `Ashfall.Core.Tests/Combat` | 10 | 84 | 0 | 84 | 1 |
| `Ashfall.Core.Tests/Progression` | 11 | 83 | 0 | 83 | 0 |
| `Ashfall.Core.Tests/Radiation` | 10 | 77 | 1 | 78 | 1 |
| `Ashfall.Core.Tests/Integration` | 16 | 74 | 0 | 74 | 0 |
| `Ashfall.Core.Tests/Foundry` | 8 | 73 | 0 | 73 | 0 |
| `Ashfall.Core.Tests/Factions` | 10 | 72 | 0 | 72 | 0 |
| `Ashfall.Core.Tests/Collectibles` | 11 | 72 | 0 | 72 | 0 |
| `Ashfall.Core.Tests/WildlifeTrapping` | 5 | 68 | 2 | 70 | 0 |
| `Ashfall.Core.Tests/Memorial` | 6 | 64 | 3 | 67 | 0 |
| `Ashfall.Core.Tests/Flagship11` | 7 | 63 | 0 | 63 | 0 |
| `Ashfall.Core.Tests/Verdict` | 7 | 50 | 0 | 50 | 0 |
| `Ashfall.Core.Tests/DutyRoster` | 5 | 49 | 0 | 49 | 0 |
| `Ashfall.Core.Tests/Performance` | 10 | 44 | 3 | 47 | 0 |
| `Ashfall.Core.Tests/Culture` | 7 | 40 | 0 | 40 | 0 |
| `Ashfall.Core.Tests/Farming` | 4 | 38 | 0 | 38 | 0 |
| `Ashfall.Core.Tests/Water` | 5 | 37 | 0 | 37 | 0 |
| `Ashfall.Core.Tests/Defense` | 4 | 36 | 0 | 36 | 1 |
| `Ashfall.Core.Tests/Mods` | 3 | 32 | 2 | 34 | 0 |
| `Ashfall.Core.Tests/MoralChoice` | 4 | 33 | 0 | 33 | 0 |
| `Ashfall.Core.Tests/Phantoms` | 1 | 29 | 4 | 33 | 0 |
| `Ashfall.Core.Tests/Codex` | 2 | 29 | 0 | 29 | 0 |
| `Ashfall.Core.Tests/Audio` | 5 | 27 | 1 | 28 | 0 |
| `Ashfall.Core.Tests/Governance` | 5 | 27 | 0 | 27 | 0 |
| `Ashfall.Core.Tests/Quests` | 4 | 25 | 0 | 25 | 0 |
| `Ashfall.Core.Tests/Research` | 4 | 25 | 0 | 25 | 0 |
| `Ashfall.Core.Tests/Difficulty` | 5 | 24 | 0 | 24 | 0 |
| `Ashfall.Core.Tests/Settings` | 2 | 19 | 3 | 22 | 0 |
| `Ashfall.Core.Tests/Content` | 6 | 22 | 0 | 22 | 1 |
| `Ashfall.Core.Tests/Localization` | 4 | 21 | 0 | 21 | 0 |
| `Ashfall.Core.Tests/Needs` | 4 | 21 | 0 | 21 | 0 |
| `Ashfall.Core.Tests/Data` | 3 | 19 | 1 | 20 | 0 |
| `Ashfall.Core.Tests/NarrativeConsequence` | 1 | 20 | 0 | 20 | 0 |
| `Ashfall.Core.Tests/InformationFlow` | 3 | 19 | 0 | 19 | 0 |
| `Ashfall.Core.Tests/Host` | 3 | 17 | 0 | 17 | 0 |
| `Ashfall.Core.Tests/Greenhouse` | 1 | 17 | 0 | 17 | 0 |
| `Ashfall.Core.Tests/Communication` | 3 | 17 | 0 | 17 | 0 |
| `Ashfall.Core.Tests/Journeys` | 5 | 16 | 0 | 16 | 0 |
| `Ashfall.Core.Tests/Orchestration` | 3 | 16 | 0 | 16 | 0 |
| `Ashfall.Core.Tests/Cognition` | 2 | 15 | 0 | 15 | 0 |
| `Ashfall.Core.Tests/Warlords` | 2 | 14 | 0 | 14 | 0 |
| `Ashfall.Core.Tests/Exploration` | 2 | 14 | 0 | 14 | 0 |
| `Ashfall.Core.Tests/Voice` | 3 | 14 | 0 | 14 | 0 |
| `Ashfall.Core.Tests/Holdfast` | 1 | 13 | 0 | 13 | 0 |
| `Ashfall.Core.Tests/Propaganda` | 2 | 13 | 0 | 13 | 0 |
| `Ashfall.Core.Tests/Underground` | 2 | 13 | 0 | 13 | 0 |
| `Ashfall.Core.Tests/Spiritual` | 2 | 12 | 0 | 12 | 0 |
| `Ashfall.Core.Tests/Production` | 3 | 12 | 0 | 12 | 0 |
| `Ashfall.Core.Tests/Crafting` | 1 | 11 | 0 | 11 | 0 |
| `Ashfall.Core.Tests/Telemetry` | 2 | 11 | 0 | 11 | 0 |
| `Ashfall.Core.Tests/Education` | 2 | 11 | 0 | 11 | 0 |
| `Ashfall.Core.Tests/Weather` | 2 | 11 | 0 | 11 | 0 |
| `Ashfall.Core.Tests/Core` | 1 | 3 | 7 | 10 | 0 |
| `Ashfall.Core.Tests/Remediation` | 1 | 10 | 0 | 10 | 0 |
| `Ashfall.Core.Tests/Kitchen` | 2 | 10 | 0 | 10 | 0 |
| `Ashfall.Core.Tests/Release` | 1 | 6 | 3 | 9 | 0 |
| `Ashfall.Core.Tests/Library` | 1 | 8 | 0 | 8 | 0 |
| `Ashfall.Core.Tests/Reputation` | 1 | 8 | 0 | 8 | 0 |
| `Ashfall.Core.Tests/Visitors` | 1 | 8 | 0 | 8 | 0 |
| `Ashfall.Core.Tests/Assets` | 1 | 7 | 0 | 7 | 0 |
| `Ashfall.Core.Tests/BodyMind` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Expansions` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Visual` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Achievements` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Vehicles` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Communications` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Events` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Accessibility` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Bestiary` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Emergency` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Diplomacy` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Maritime` | 1 | 6 | 0 | 6 | 0 |
| `Ashfall.Core.Tests/Lifecycle` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Recreation` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Balance` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Generations` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Nutrition` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Excavation` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Rail` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Textiles` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Optics` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Print` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Presentation` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Records` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Launch` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Settlements` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Cooking` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Legacy` | 1 | 5 | 0 | 5 | 0 |
| `Ashfall.Core.Tests/Archaeology` | 1 | 4 | 0 | 4 | 0 |
| `Ashfall.Core.Tests/Equipment` | 1 | 4 | 0 | 4 | 0 |
| `Ashfall.Core.Tests/Espionage` | 1 | 4 | 0 | 4 | 0 |
| `Ashfall.Core.Tests/Hygiene` | 1 | 4 | 0 | 4 | 0 |
| `Ashfall.Core.Tests/PlayerCommand` | 1 | 1 | 0 | 1 | 0 |
| `Ashfall.Core.Tests/Fixtures` | 2 | 0 | 0 | 0 | 0 |
| `Ashfall.Core.Tests/obj/Debug/net9.0` | 2 | 0 | 0 | 0 | 0 |

## Top 15 families by case count (split candidates)

1. `Ashfall.Core.Tests` — 5731 cases
2. `Ashfall.Core.Tests/Shelter` — 754 cases
3. `Ashfall.Core.Tests/World` — 539 cases
4. `Ashfall.Core.Tests/Survivors` — 453 cases
5. `Ashfall.Core.Tests/Medical` — 435 cases
6. `Ashfall.Core.Tests/Radio` — 354 cases
7. `Ashfall.Core.Tests/Economy` — 329 cases
8. `Ashfall.Core.Tests/Expeditions` — 322 cases
9. `Ashfall.Core.Tests/Narrative` — 293 cases
10. `Ashfall.Core.Tests/Campaign` — 187 cases
11. `Ashfall.Core.Tests/Tooling` — 117 cases
12. `Ashfall.Core.Tests/Inventory` — 114 cases
13. `Ashfall.Core.Tests/Save` — 110 cases
14. `Ashfall.Core.Tests/UI` — 103 cases
15. `Ashfall.Core.Tests/Endgame` — 101 cases
