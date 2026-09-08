# Plan 161 — Survivor Hobby & Leisure System

## Goal

Create a survivor hobby and leisure system where survivors pursue personal interests, pastimes, and creative activities during off-hours. Currently survivors work, sleep, and exist — but have no personality beyond stats and relationships. No hobbies, no pastimes, no creative outlets, no leisure activities. This plan adds depth to survivor personality, creates morale opportunities, and generates emergent stories about who these people are beyond their utility.

## Why

**Repository evidence:** The survivor social agent confirmed: "Survivors are reactive instruments. They respond to player orders and system-generated stimuli, but never initiate actions, express preferences, or make independent decisions." Plan 144 (survivor autonomy) adds autonomous behavior but not structured leisure. Survivors have skills, needs, and relationships — but no personality, no interests, no hobbies, no creative outlets. They are workers, not people.

**What is missing:** Survivors don't paint, play music, read, garden, craft, tell stories, stargaze, meditate, exercise, cook for fun, or pursue any interest beyond survival. There is no leisure system, no hobby tracking, no personality depth beyond numerical stats. The shelter is a workplace, not a home.

**Why existing plans don't solve it:** Plan 12 (social/shelter life) mentions shelter decor but not hobbies. Plan 144 (survivor autonomy) adds autonomous behavior but not structured leisure. Plan 150 (romance/family) adds relationships but not personal interests. Plan 154 (education) adds learning but not leisure. No plan addresses hobbies, pastimes, or creative activities.

**Player value:** Makes survivors feel like real people (they have interests), creates morale opportunities (leisure activities boost morale), generates emergent stories (a survivor's hobby becomes shelter tradition), and adds personality depth beyond stats.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/` — survivor systems
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill system
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — morale tracking
- `Assets/Ashfall.Core/Shelter/` — shelter systems
- NEW: `Assets/Ashfall.Core/Survivors/HobbySystem.cs`
- NEW: `Assets/StreamingAssets/Data/hobby_definitions.json`

## Main Task 1 — Foundation / System Contract

1. Create `HobbySystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `Hobby` DTO: `hobbyId`, `hobbyName`, `hobbyCategory` (creative/intellectual/physical/social/crafting/collecting), `skillRequired` (minimum skill to pursue), `moraleBonus` (per session), `resourceCost` (optional materials), `spaceRequired` (room type)
3. Define `HobbyProgress` DTO: `survivorId`, `hobbyId`, `proficiency` (0-100), `sessionsCompleted`, `lastSessionDay`, `masteryLevel` (novice/apprentice/journeyman/master)
4. Define `HobbyState` DTO: list of survivor hobby progress, list of available hobbies, shelter hobby facilities
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define hobby categories with distinct hobbies:
   - **Creative**: painting, music, writing, storytelling, sculpture
   - **Intellectual**: reading,研究, puzzles, chess, philosophy
   - **Physical**: exercise, yoga, martial arts, dancing, sports
   - **Social**: games, parties, conversation circles, theater
   - **Crafting**: woodworking, sewing, metalwork, pottery, cooking
   - **Collecting**: stamps, rocks, bottles, artifacts, specimens
7. Define hobby mechanics:
   - Survivors choose hobby based on personality, skills, interests
   - Hobby pursued during off-hours (free time between duties)
   - Each session increases proficiency and provides morale bonus
   - Higher proficiency = better morale bonus, mastery rewards
   - Some hobbies require materials (paint, wood, instruments)
   - Some hobbies require space (workshop, library, garden)
8. Define hobby facility requirements:
   - **Library**: reading,研究, writing
   - **Workshop**: woodworking, metalwork, pottery
   - **Garden**: gardening, botany, nature study
   - **Common room**: games, parties, conversation
   - **Gym**: exercise, martial arts, sports
   - **Studio**: painting, sculpture, music
9. Define hobby mastery levels:
   - **Novice** (0-25): basic participation, small morale bonus
   - **Apprentice** (25-50): improving skill, moderate morale bonus
   - **Journeyman** (50-75): competent, good morale bonus, can teach
   - **Master** (75-100): expert, large morale bonus, shelter reputation
10. Define hobby social effects:
    - Shared hobbies create bonds between survivors
    - Hobby masters can teach apprentices
    - Hobby groups form (book club, art circle, sports team)
    - Hobby achievements celebrated (shelter events)
11. Add deterministic seeding: hobby outcomes use `ISeededRng`
12. Wire into `GameBootstrap`: `SetupHobbies`, `TickHobbies`, `SaveHobbies`
13. Create `HobbyCatalogLoader` for hobby definitions
14. Implement hobby facility system: rooms can be designated for hobbies
15. Create UI hook: survivor panel showing hobbies, proficiency, sessions

## Main Task 2 — Implementation / Hobbies / Facilities / Mastery / Events

1. Implement hobby selection:
   - Survivor chooses hobby based on personality traits
   - Personality-hobby affinity (creative survivors prefer art)
   - Skill requirements (need basic skill to start)
   - Facility availability (need library for reading)
   - Material availability (need paint for painting)
2. Implement hobby sessions:
   - Survivor pursues hobby during free time
   - Session takes 1-4 hours (game time)
   - Proficiency increases with each session
   - Morale bonus applied after session
   - Materials consumed (if required)
3. Implement hobby mastery:
   - Proficiency increases with practice
   - Mastery levels unlock benefits
   - Masters can teach apprentices
   - Masters gain shelter reputation
   - Mastery achievements celebrated
4. Implement hobby facilities:
   - Rooms designated for hobby use
   - Facilities provide bonuses (better tools, space)
   - Multiple hobbies can share facilities
   - Facilities can be upgraded (better equipment)
5. Implement hobby social effects:
   - Shared hobbies create affinity bonds
   - Hobby groups form naturally
   - Group activities provide extra morale
   - Hobby rivalries can develop (friendly competition)
6. Implement hobby teaching:
   - Masters can teach apprentices
   - Teaching accelerates apprentice proficiency
   - Teaching provides morale to both
   - Teaching strengthens mentor-apprentice bond
7. Create hobby events:
   - "The Masterpiece" — survivor creates exceptional work
   - "The Performance" — survivor performs for shelter
   - "The Exhibition" — hobby works displayed
   - "The Competition" — hobby contest between survivors
   - "The Club" — hobby group forms
   - "The Tradition" — hobby becomes shelter tradition
   - "The Discovery" — hobby leads to unexpected insight
8. Add hobby quest hooks:
   - "The Artist" — help survivor complete masterpiece
   - "The Performance" — organize shelter talent show
   - "The Collection" — help survivor complete collection
   - "The Tournament" — host hobby competition
   - "The Workshop" — build dedicated hobby facility
   - "The Legacy" — hobby becomes shelter cultural heritage
9. Implement hobby integration:
   - Hobbies affect morale (primary benefit)
   - Hobbies can improve related skills (painting→crafting)
   - Hobbies create social bonds (shared interests)
   - Hobbies provide shelter culture (art, music, stories)
   - Hobbies can lead to discoveries (research insights)
10. Add UI: hobby panel showing survivor hobbies, proficiency, facilities
11. Create hobby journal: automatic log of hobby events and achievements
12. Implement hobby tutorial: first hobby session explains system
13. Add hobby tooltips: hover over hobby shows benefits and requirements
14. Create 30 hobbies across 6 categories in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `NeedsSystem`: hobby sessions provide morale bonus
2. Connect to `SkillProgressionSystem`: hobbies can improve related skills
3. Integrate with `SurvivorRelationsSystem`: shared hobbies create bonds
4. Connect to `ApprenticeshipSystem`: hobby teaching integrates
5. Wire into `ShelterExpansionSystem` (Plan 156): hobby facilities in rooms
6. Connect to `MentalHealthCrisisSystem`: hobbies reduce crisis risk
7. Implement old-save compatibility: existing saves get empty hobby state
8. Add deterministic seeding: hobby outcomes use `ISeededRng`
9. Create exploit prevention: hobbies require time, can't be spammed
10. Add tests: hobby sessions, proficiency gain, morale effects, save round-trip
11. Verify catalog integrity: all hobby/facility IDs resolve
12. Test edge cases: no hobbies (no leisure), all hobbies (max morale)
13. Verify headless behavior: hobbies process correctly without UI
14. Add data-integrity-selftest: hobby definitions validate against catalogs
15. Create `--hobbies-selftest` verb for CI validation

## State / System Interaction Model

```text
Survivor hobby system
├─ Hobby selection
│  ├─ Based on personality, skills, interests
│  ├─ Requires facility and materials
│  └─ Survivor pursues during free time
├─ Hobby sessions
│  ├─ 1-4 hours per session
│  ├─ Proficiency increases
│  ├─ Morale bonus applied
│  └─ Materials consumed
├─ Hobby mastery
│  ├─ Novice → Apprentice → Journeyman → Master
│  ├─ Higher mastery = better benefits
│  ├─ Masters can teach
│  └─ Mastery celebrated
├─ Hobby facilities
│  ├─ Rooms designated for hobbies
│  ├─ Facilities provide bonuses
│  ├─ Can be upgraded
│  └─ Multiple hobbies share
├─ Hobby social effects
│  ├─ Shared hobbies create bonds
│  ├─ Hobby groups form
│  ├─ Group activities boost morale
│  └─ Friendly rivalries develop
└─ Hobby integration
   ├─ Morale bonus (primary)
   ├─ Skill improvement (secondary)
   ├─ Social bonds (tertiary)
   ├─ Shelter culture (quaternary)
   └─ Research insights (rare)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --hobbies-selftest
```

## Risk

**LOW** — Hobby system is straightforward with clear inputs (time, materials, facilities) and outputs (morale, proficiency, bonds). Risk of hobbies feeling like busywork rather than meaningful personality. Mitigation: keep hobby sessions optional, make benefits clear, show hobby achievements, and integrate with social systems.

## Definition of Done

- `HobbySystem.cs` exists with full `CaptureState/RestoreState`
- 6 hobby categories with 30 total hobbies
- Hobby session mechanics functional
- Mastery progression working (novice through master)
- Hobby facility system integrated
- Hobby social effects (bonds, groups, teaching)
- Hobby events and quest hooks
- Save/load round-trip tested
- Deterministic hobby outcomes verified
- Old saves load without error
- 30 hobbies in data authority
- UI panel shows survivor hobbies
- Cross-system integration (needs, skills, relations, apprenticeship, shelter, mental health)

## Follow-On Opportunities

- Hobby competitions (shelter-wide contests)
- Hobby exhibitions (display works for morale)
- Hobby traditions (annual events)
- Hobby legacy (famous hobbyists remembered)
- Hobby quests (complete masterpiece, win competition)
