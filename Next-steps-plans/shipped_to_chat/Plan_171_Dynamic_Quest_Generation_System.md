# Plan 171 — Dynamic Quest Generation System

## Goal

Create a dynamic quest generation system where quests are procedurally generated from game state, survivor traits, faction conditions, and world events rather than being entirely static data. Currently all quests are defined in `questline_master.json` and `moral_choice_quests.json` as fixed entries with predetermined objectives, rewards, and outcomes. There is no procedural quest generation, no dynamic quest adaptation, no quests that emerge from the current state of the world. This plan adds infinite replayability through quests that are different every campaign.

## Why

**Repository evidence:** Grep for `DynamicQuest`, `ProceduralQuest`, `QuestGenerat`, `quest_generat` in Core returns ZERO matches. All quests are static data: `questline_master.json` (referenced in ContentUtilizationScanner), `moral_choice_quests.json` (65 quests, ALL with empty `location_id`), and `moral_choice_quest_stubs.json` (10 stub entries). Quest system loads fixed data, presents fixed objectives, grants fixed rewards. No quest adapts to game state, no quest is generated from conditions, no quest varies between campaigns.

**What is missing:** No procedural quest generation. No dynamic quest adaptation. No quests that emerge from current world state. Every campaign plays the same quests in the same order with the same outcomes. There is no replayability in quest content.

**Why existing plans don't solve it:** Plan 133 (expedition consequences) adds discovery consequences but not quest generation. Plan 144 (survivor autonomy) adds autonomous behavior but not quest creation. Plan 148 (friction→events) adds friction events but not full quest generation. No plan addresses procedural or dynamic quest creation.

**Player value:** Creates replayability (different quests each campaign), adds surprise (quests emerge from unexpected situations), generates personal stories (quests reference current survivors, locations, conditions), and extends game lifespan (infinite quest variety).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Quests/` — quest system files
- `Assets/StreamingAssets/Data/questline_master.json` — static quest data
- `Assets/StreamingAssets/Data/moral_choice_quests.json` — moral choice quests
- `Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs` — consequence tracking
- `Assets/Ashfall.Core/Narrative/` — narrative system
- NEW: `Assets/Ashfall.Core/Quests/DynamicQuestGenerator.cs`
- NEW: `Assets/StreamingAssets/Data/quest_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `DynamicQuestGenerator.cs` in `Assets/Ashfall.Core/Quests/`
2. Define `QuestTemplate` DTO: `templateId`, `questType` (fetch/escort/investigate/defend/negotiate/explore/resolve), `triggerConditions` (list of conditions: game state, survivor traits, faction standing, world events), `objectiveTemplate` (parameterized objective with placeholders), `rewardTemplate` (parameterized reward), `difficulty` (1-5), `tags` (list)
3. Define `DynamicQuest` DTO: `questId` (generated), `templateId`, `generatedDay`, `triggeredBy` (what game state triggered generation), `objectives` (resolved from template), `rewards` (resolved from template), `status` (available/active/complete/failed/expired), `assignedSurvivor` (optional), `locationId` (resolved), `factionId` (resolved)
4. Define `QuestCondition` DTO: `conditionType` (morale_below/supply_shortage/faction_standing/survivor_trait/location_discovered/day_range/radiation_level/weather_condition), `parameter` (what to check), `threshold` (value to compare), `comparison` (lt/gt/eq/contains)
5. Define `DynamicQuestState` DTO: list of generated quests, list of completed quests, generation cooldown (days between generations), seed for deterministic generation, generation log
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define quest types:
   - **Fetch**: retrieve item from location (item + location resolved from state)
   - **Escort**: guide NPC/survivor to location (NPC + location resolved)
   - **Investigate**: explore location and report findings (location resolved)
   - **Defend**: protect location from threat (location + threat resolved)
   - **Negotiate**: resolve faction dispute (factions resolved)
   - **Explore**: discover new location or resource (area resolved)
   - **Resolve**: settle survivor conflict or shelter problem (conflict resolved)
8. Define trigger conditions:
   - Morale below threshold → morale quest generated
   - Supply shortage → resource acquisition quest
   - Faction standing change → diplomatic quest
   - Survivor with specific trait → personal quest
   - Location discovered → exploration follow-up
   - Day range → seasonal/timed quest
   - Radiation event → radiation response quest
   - Weather event → weather response quest
9. Define parameterized objectives:
   - "Retrieve {item} from {location}" — item and location resolved from state
   - "Escort {survivor} to {location}" — survivor and location resolved
   - "Investigate {location} for {anomaly}" — location and anomaly resolved
   - "Defend {location} from {threat}" — location and threat resolved
   - "Negotiate between {faction_a} and {faction_b}" — factions resolved
10. Define parameterized rewards:
    - Item rewards scaled to difficulty
    - Faction standing gains scaled to faction involved
    - Morale rewards scaled to quest type
    - Skill XP scaled to quest complexity
    - Unique rewards for rare quest combinations
11. Define generation rules:
    - Max 3 active dynamic quests at once
    - Generation cooldown: 5 days between new quests
    - Quests expire after 30 days if not accepted
    - Generated quests cannot duplicate active quests
    - Generated quests reference current game state (survivors, locations, factions)
12. Add deterministic seeding: quest generation uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupDynamicQuests`, `TickDynamicQuests`, `SaveDynamicQuests`
14. Create `QuestTemplateCatalogLoader` for template definitions
15. Implement dynamic quest UI: quest board showing available dynamic quests

## Main Task 2 — Implementation / Generation / Resolution / Integration

1. Implement quest generation engine:
   - Each tick, check trigger conditions against game state
   - Matching conditions → generate quest from template
   - Resolve placeholders (item, location, survivor, faction)
   - Generated quest added to available quests
   - Quest shown on quest board
2. Implement parameter resolution:
   - `{item}` resolved from available items (weighted by rarity)
   - `{location}` resolved from discovered locations (weighted by distance)
   - `{survivor}` resolved from shelter survivors (weighted by traits)
   - `{faction}` resolved from active factions (weighted by standing)
   - `{threat}` resolved from current threats (raiders, weather, radiation)
   - `{anomaly}` resolved from location evolution state
3. Implement quest acceptance:
   - Player reviews generated quest (objectives, rewards, difficulty)
   - Player assigns survivor (optional, affects success chance)
   - Quest status changes to active
   - Quest objectives tracked
4. Implement quest completion:
   - Objectives checked against game state
   - Completed objectives grant rewards
   - Quest status changes to complete
   - Completion recorded in quest log
   - Completion may trigger new quests
5. Implement quest failure:
   - Failed objectives (timeout, survivor death, location lost)
   - Failed quests have consequences (faction standing loss, morale penalty)
   - Failure recorded in quest log
   - Failure may trigger follow-up quests
6. Implement quest chaining:
   - Completed quests can trigger follow-up quests
   - Quest chains develop story arcs
   - Chain decisions affect later quests
   - Chains resolved through game state
7. Implement quest variety:
   - Templates have variations (different objective structures)
   - Generated quests have unique flavor text
   - Quest combinations create unique situations
   - Rare quest templates for special conditions
8. Create dynamic quest events:
   - "The Opportunity" — new dynamic quest available
   - "The Assignment" — survivor assigned to quest
   - "The Completion" — quest successfully completed
   - "The Failure" — quest failed
   - "The Chain" — quest chain continues
   - "The Rare" — rare dynamic quest generated
   - "The Expiry" — quest expired without acceptance
9. Add dynamic quest hooks:
   - "The Quester" — complete 10 dynamic quests
   - "The Chain" — complete a quest chain
   - "The Rare" — complete a rare dynamic quest
   - "The Variety" — complete all quest types
   - "The Generator" — have 3 dynamic quests active simultaneously
   - "The Resolver" — resolve 5 faction disputes via quests
   - "The Explorer" — discover locations via exploration quests
10. Implement dynamic quest UI:
    - Quest board: shows available dynamic quests
    - Quest detail: objectives, rewards, difficulty, assigned survivor
    - Quest log: history of completed/failed quests
    - Quest filter: by type, status, difficulty
    - Quest notification: new quest available
11. Add quest journal: automatic log of quest events
12. Implement quest tutorial: first dynamic quest explains system
13. Add quest tooltips: hover over quest shows details
14. Create 20 quest templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into existing quest system: dynamic quests complement static quests
2. Connect to `CampaignConsequenceLedger`: quest triggers reference consequences
3. Integrate with `FactionBranchCoordinator`: faction quests affect standing
4. Connect to `LocationEvolutionSystem`: location quests reference evolution
5. Wire into `SurvivorRelationsSystem`: survivor quests affect relationships
6. Connect to `ExpeditionSystem`: exploration quests use expedition framework
7. Implement old-save compatibility: existing saves get empty dynamic quest state
8. Add deterministic seeding: generation uses `ISeededRng`
9. Create exploit prevention: generation cooldown, max active quests, expiry
10. Add tests: quest generation, parameter resolution, completion, failure, chaining, save round-trip
11. Verify catalog integrity: all template/item/location/faction IDs resolve
12. Test edge cases: no triggers (no quests), many triggers (quest overflow)
13. Verify headless behavior: quest generation processes correctly without UI
14. Add data-integrity-selftest: quest templates validate against catalogs
15. Create `--dynamic-quest-selftest` verb for CI validation

## State / System Interaction Model

```text
Dynamic quest generation
├─ Quest templates (20+ templates)
│  ├─ Quest types: fetch/escort/investigate/defend/negotiate/explore/resolve
│  ├─ Trigger conditions: game state thresholds
│  ├─ Parameterized objectives: {item}, {location}, {survivor}, {faction}
│  ├─ Parameterized rewards: scaled to difficulty
│  └─ Tags for filtering
├─ Generation engine
│  ├─ Check triggers each tick
│  ├─ Match conditions → generate quest
│  ├─ Resolve placeholders from state
│  ├─ Max 3 active, 5-day cooldown
│  └─ Quests expire after 30 days
├─ Parameter resolution
│  ├─ Items from available inventory
│  ├─ Locations from discovered map
│  ├─ Survivors from shelter roster
│  ├─ Factions from active factions
│  └─ Threats from current dangers
├─ Quest lifecycle
│  ├─ Available → accepted → active → complete/failed
│  ├─ Completion grants rewards
│  ├─ Failure has consequences
│  ├─ Chains trigger follow-ups
│  └─ All recorded in quest log
└─ Integration
   ├─ Quest system (complements static quests)
   ├─ Consequences (trigger references)
   ├─ Factions (standing effects)
   ├─ Locations (evolution references)
   ├─ Relations (survivor effects)
   └─ Expeditions (exploration framework)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --dynamic-quest-selftest
```

## Risk

**MEDIUM** — Dynamic quest generation can produce nonsensical combinations if templates and resolution are not carefully constrained. Risk of generated quests feeling formulaic rather than organic. Mitigation: extensive template variety, careful parameter resolution rules, quality filtering, and playtesting generated quest combinations.

## Definition of Done

- `DynamicQuestGenerator.cs` exists with full `CaptureState/RestoreState`
- 7 quest types implemented (fetch, escort, investigate, defend, negotiate, explore, resolve)
- Quest trigger condition system functional
- Parameterized objective resolution working
- Quest lifecycle (available → active → complete/failed)
- Quest chaining for story arcs
- Dynamic quest events and quest hooks
- Save/load round-trip tested
- Deterministic generation verified
- Old saves load without error
- 20 quest templates in data authority
- Quest board UI panel
- Cross-system integration (quests, consequences, factions, locations, relations, expeditions)

## Follow-On Opportunities

- Quest difficulty scaling (quests grow harder over campaign)
- Quest rarity system (common/uncommon/rare/legendary quests)
- Quest customization (player influence quest parameters)
- Quest legacy (famous dynamic quests remembered)
- Quest trading (share dynamic quests with other settlements)
