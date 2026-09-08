# Plan 200 — Survivor Personal Quests & Character Arcs

## Goal

Create a survivor personal quest and character arc system where each survivor can develop individual storylines based on their experiences, relationships, traits, and history — creating unique character development paths that make each survivor feel like a protagonist with their own journey. Currently survivors gain skills, form relationships, and experience events, but there are no personal questlines, no character arcs, no individual story development. Survivors are functional units without narrative depth. This plan adds character-driven storytelling to make each survivor memorable.

## Why

**Repository evidence:** Grep for `PersonalQuest`, `CharacterArc`, `SurvivorStory`, `PersonalStoryline`, `IndividualQuest`, `SurvivorNarrative` in Core returns ZERO matches. Survivors have skills (`SkillProgressionSystem`), relationships (`SurvivorRelationsSystem`), traits (`TraitSystem`), and experience events (`EventSystem`), but these don't combine into personal narratives. Plan 132 (Survivor Hidden Agendas) adds hidden motivations but not positive character arcs. Plan 147 (Per-NPC Memory) adds memory but not personal quests. Plan 179 (Psychology) adds psychological profiles but not character development stories.

**What is missing:** No personal quest system for survivors. No character arcs that develop over time. No individual storylines based on survivor history. No personal goals that evolve. No character development narratives. Survivors don't have "stories" — they have stats and relationships.

**Why existing plans don't solve it:** Plan 132 (hidden agendas) adds secret motivations but not open character arcs. Plan 147 (memory) adds recall but not narrative development. Plan 179 (psychology) adds mental state but not personal stories. Plan 185 (memory decay) adds forgetting but not character growth. No plan addresses personal quests/character arcs as a system.

**Player value:** Creates emotional investment (each survivor has a story), adds narrative depth (survivors develop over time), generates emergent stories (unique character arcs), and makes survivors feel like individuals with journeys rather than interchangeable units.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skills
- `Assets/Ashfall.Core/TraitSystem.cs` — traits
- `Assets/Ashfall.Core/EventSystem.cs` — events
- `Assets/Ashfall.Core/Journal/JournalSystem.cs` — journal
- NEW: `Assets/Ashfall.Core/Narrative/PersonalQuestSystem.cs`
- NEW: `Assets/StreamingAssets/Data/personal_quest_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `PersonalQuestSystem.cs` in `Assets/Ashfall.Core/Narrative/`
2. Define `PersonalQuest` DTO: `questId`, `survivorId`, `questType` (redemption/growth/discovery/revenge/protection/legacy/love/mastery), `questName`, `description`, `stages` (list of quest stages), `currentStage` (index), `triggeredDay`, `completedDay` (-1 if incomplete), `status` (active/completed/failed/abandoned), `relatedSurvivorIds` (list), `relatedLocationIds` (list), `relatedItemIds` (list)
3. Define `QuestStage` DTO: `stageId`, `stageName`, `description`, `completionCondition` (type + parameters), `isCompleted` bool, `completedDay` (-1 if incomplete), `outcome` (success/failure/skipped)
4. Define `CharacterArc` DTO: `arcId`, `survivorId`, `arcType` (hero/tragic/redemption/wisdom/corruption/growth), `arcTheme` (courage/sacrifice/knowledge/love/power/freedom), `arcStages` (list of arc stages), `currentStage` (index), `arcProgress` (0-100), `arcOutcome` (positive/neutral/negative/mixed)
5. Define `QuestTrigger` DTO: `triggerId`, `triggerType` (relationship_milestone/skill_achieved/event_experienced/item_acquired/location_visited/trait_activated/time_passed), `triggerParameters` (dict of parameter → value), `questTemplateId` (which quest this triggers)
6. Define `PersonalQuestState` DTO: list of active personal quests per survivor, list of character arcs per survivor, list of quest triggers, list of completed quests (history), personal quest settings (max active quests per survivor, auto-generate quests bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define quest types (8+ types):
   - **Redemption**: survivor seeks to atone for past mistake (requires: negative event, trait flaw)
   - **Growth**: survivor develops new capability or overcomes limitation (requires: skill gap, personal challenge)
   - **Discovery**: survivor uncovers hidden truth or secret (requires: curiosity trait, investigation opportunity)
   - **Revenge**: survivor seeks justice against someone who wronged them (requires: grudge, target identified)
   - **Protection**: survivor commits to protecting someone/something (requires: strong relationship, threat detected)
   - **Legacy**: survivor works to leave lasting impact (requires: high skill, long-term commitment)
   - **Love**: survivor pursues romantic relationship (requires: relationship interest, mutual attraction)
   - **Mastery**: survivor achieves ultimate skill in domain (requires: high skill level, dedication)
9. Define character arc types (6+ types):
   - **Hero**: survivor rises to challenge, becomes leader/protector (positive outcome)
   - **Tragic**: survivor's flaws lead to downfall (negative outcome)
   - **Redemption**: survivor overcomes past mistakes, finds forgiveness (positive outcome)
   - **Wisdom**: survivor gains understanding, becomes advisor/mentor (positive outcome)
   - **Corruption**: survivor's power/success corrupts them (negative outcome)
   - **Growth**: survivor matures, develops character (positive outcome)
10. Define quest triggers:
    - **Relationship milestone**: friendship/romance/family reaches threshold
    - **Skill achieved**: survivor reaches skill level
    - **Event experienced**: survivor experiences significant event
    - **Item acquired**: survivor obtains specific item
    - **Location visited**: survivor visits specific location
    - **Trait activated**: survivor's trait triggers in situation
    - **Time passed**: certain number of days since event
11. Define quest stages:
    - Each quest has 3-7 stages
    - Stages have completion conditions
    - Stages progress sequentially
    - Stages can have branching outcomes
    - Quest completion requires all stages complete
12. Define quest generation:
    - Quests generated based on survivor history, traits, relationships, skills
    - Quest templates define structure, stages, conditions
    - Quest generation uses deterministic seeding
    - Max active quests per survivor (default: 2)
    - Quests can be manually assigned or auto-generated
13. Define quest consequences:
    - Quest completion: survivor gains benefits (skill bonus, trait improvement, relationship boost)
    - Quest failure: survivor suffers consequences (skill penalty, trait worsening, relationship damage)
    - Quest abandonment: survivor loses opportunity, may affect morale
    - Quest outcomes affect character arc progression
14. Add deterministic seeding: quest generation uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupPersonalQuests`, `TickPersonalQuests`, `SavePersonalQuests`

## Main Task 2 — Implementation / Quests / Arcs / Triggers / Stages / UI

1. Implement quest generation:
   - Evaluate survivor for quest eligibility
   - Check trigger conditions (relationships, skills, events, etc.)
   - Select quest template based on survivor profile
   - Generate quest with stages and conditions
   - Add quest to survivor's active quests
   - Quest generation logged
2. Implement quest progression:
   - Check quest stage completion conditions daily
   - Advance quest to next stage when condition met
   - Track quest progress
   - Quest progression logged
3. Implement quest completion:
   - When all stages complete: quest completed
   - Apply quest completion benefits
   - Update character arc progress
   - Quest moved to history
   - Quest completion logged
4. Implement quest failure:
   - When stage condition impossible: quest failed
   - Apply quest failure consequences
   - Update character arc progress
   - Quest moved to history
   - Quest failure logged
5. Implement character arcs:
   - Track arc type and theme per survivor
   - Arc progress based on quest outcomes
   - Arc stages unlock at progress thresholds
   - Arc outcomes determined by final progress
   - Arc progression logged
6. Implement quest UI:
   - Survivor detail: active quests, character arc
   - Quest panel: quest description, stages, progress
   - Arc panel: arc type, theme, progress, outcome
   - Quest log: history of completed/failed quests
   - Quest notifications: stage completed, quest completed/failed
7. Implement quest triggers:
   - Monitor trigger conditions continuously
   - When trigger condition met: generate quest
   - Trigger conditions based on survivor state
   - Triggers logged
8. Implement quest branching:
   - Some stages have multiple outcome paths
   - Player/survivor choices affect branch
   - Branches lead to different quest outcomes
   - Branching logged
9. Implement quest integration:
   - Quests integrate with relationships (protect friend, romance interest)
   - Quests integrate with skills (master skill, teach apprentice)
   - Quests integrate with events (avenge wrong, discover secret)
   - Quests integrate with items (find artifact, craft legacy)
   - Quests integrate with locations (explore ruins, return home)
10. Create quest events:
    - "The Quest Begins" — personal quest started
    - "The Stage" — quest stage completed
    - "The Choice" — quest branching point
    - "The Completion" — quest successfully completed
    - "The Failure" — quest failed
    - "The Arc" — character arc stage reached
    - "The Outcome" — character arc completed
    - "The Legacy" — survivor's story remembered
11. Add quest hooks:
    - "The Hero" — complete 5 hero arc quests
    - "The Mentor" — guide 3 survivors through growth arcs
    - "The Storyteller" — witness 10 quest completions
    - "The Legacy" — have survivor complete legacy quest
    - "The Romance" — complete love quest with 3 survivors
    - "The Mastery" — complete mastery quest in 5 skills
    - "The Redemption" — complete redemption quest for 3 survivors
12. Implement quest tutorial: first personal quest explains system
13. Add quest tooltips: hover over quest shows stages, progress
14. Create quest templates in data file (20+ templates)
15. Implement quest persistence: quests/arcs saved with survivor state

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SurvivorRelationsSystem`: relationship quests integrate
2. Connect to `SkillProgressionSystem`: skill quests integrate
3. Integrate with `TraitSystem`: trait-triggered quests
4. Connect to `EventSystem`: event-triggered quests
5. Wire into `JournalSystem`: quest events logged in journal
6. Connect to `PsychologicalProfileSystem` (Plan 179): arc affects psychology
7. Implement old-save compatibility: existing saves get no active quests
8. Add deterministic seeding: quest generation uses `ISeededRng`
9. Create exploit prevention: quests are history-based, can't be gamed
10. Add tests: quest generation, progression, completion, failure, arcs, triggers, save round-trip
11. Verify all quest types work correctly
12. Test edge cases: no quests (current behavior), many quests (complex narratives)
13. Verify headless behavior: quests process correctly without UI
14. Add data-integrity-selftest: quests validate against survivor/relationship/skill catalogs
15. Create `--personal-quest-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --personal-quest-selftest
```

## Risk

**LOW** — Personal quests are straightforward with clear inputs (triggers, stages) and outputs (quest progression, arc development). Risk of quests feeling arbitrary or disconnected from survivor identity. Mitigation: base quests on survivor history/traits/relationships, show clear narrative connections, ensure quests feel meaningful not mechanical.

## Definition of Done

- `PersonalQuestSystem.cs` exists with full `CaptureState/RestoreState`
- 8+ quest types (redemption, growth, discovery, revenge, protection, legacy, love, mastery)
- 6+ character arc types (hero, tragic, redemption, wisdom, corruption, growth)
- Quest triggers (relationship, skill, event, item, location, trait, time)
- Quest stages with completion conditions
- Quest branching (multiple outcome paths)
- Quest consequences (completion benefits, failure penalties)
- Character arc progression based on quest outcomes
- Quest UI (quest panel, arc panel, quest log, notifications)
- Quest events and hooks
- Save/load round-trip tested
- Deterministic quest generation verified
- Old saves load with no active quests
- Quest templates in data authority (20+ templates)
- Cross-system integration (relations, skills, traits, events, journal, psychology)

## Follow-On Opportunities

- Quest specialization (survivors develop quest preferences)
- Quest legacy (famous quests remembered across campaigns)
- Quest trading (survivors share quest opportunities)
- Quest events (quest festivals, story competitions)
- Quest documentation (survivor memoirs, biographies)
