# Plan 154 — Survivor Education & Knowledge Transfer

## Goal

Create a survivor education system where children grow up educated, skills pass from generation to generation, and knowledge becomes a persistent shelter resource. Currently `CohortSystem.cs` (174 lines) tracks children with maturation flags but maturation is a single step — children become adult survivors with no education, no skills, and no knowledge of their parents' expertise. `ApprenticeshipSystem.cs` (153 lines) transfers skill XP but is transactional, not educational. `LibraryStudySystem.cs` (206 lines) provides study mechanics but not formal education. This plan adds structured education, knowledge transfer, and generational learning.

## Why

**Repository evidence:** The late-game agent confirmed: "Maturation is a boolean flag, not a developmental system. There is no unique skill tree for children who grew up in the bunker, no 'bunker-born' traits or perspectives, no adolescence phase, no gradual capability growth." `CohortSystem.TryMaturation()` sets `isMatured = true` but doesn't apply education, skills, or traits. `GenerationalLineageExtension` tracks `inheritedTraitIds` but no code populates it from parent traits. Children mature into blank-slate adults with no benefit from their parents' knowledge or the shelter's accumulated wisdom.

**What is missing:** Children don't go to school. They don't learn from their parents. They don't develop skills during adolescence. When they mature, they're identical to any other new survivor — no education, no inherited knowledge, no generational continuity. The shelter's accumulated knowledge dies with each generation.

**Why existing plans don't solve it:** Plan 26 (knowledge/research/skills) externalizes skill data but doesn't address education. Plan 33 (skill catalog) defines skills but not how they're taught. Plan 140 (legacy) adds cross-campaign inheritance but not in-campaign education. Plan 144 (survivor autonomy) adds autonomous behavior but not structured learning. No plan addresses formal education or generational knowledge transfer.

**Player value:** Creates long-term investment (educate children for future benefits), adds depth to family dynamics (parents teach children), makes shelter knowledge persistent (library, traditions), and generates emergent stories (a child follows in parent's footsteps, a teacher shapes the next generation).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/CohortSystem.cs` — children/maturation
- `Assets/Ashfall.Core/ApprenticeshipSystem.cs` — skill transfer
- `Assets/Ashfall.Core/LibraryStudySystem.cs` — study mechanics
- `Assets/Ashfall.Core/GenerationalLineageExtension.cs` — lineage tracking
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill progression
- NEW: `Assets/Ashfall.Core/Survivors/EducationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/education_curriculum.json`

## Main Task 1 — Foundation / System Contract

1. Create `EducationSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `EducationStage` DTO: `stageId`, `stageName` (childhood/adolescence/young_adult), `ageRange` (min-max years), `learningCapacity` (0-100), `availableSubjects` (list)
3. Define `EducationRecord` DTO: `survivorId`, `currentStage`, `subjectsStudied` (map of subject → proficiency 0-100), `teachers` (list of survivor IDs), `educationQuality` (0-100), `graduationDay` (-1 if not graduated)
4. Define `Curriculum` DTO: `subjectId`, `subjectName`, `prerequisites` (list), `skillUnlocked` (skill ID), `proficiencyRequired` (0-100), `teachingSpeed` (modifier)
5. Define `EducationState` DTO: list of education records, list of available curricula, shelter knowledge level
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define education stages:
   - **Childhood** (0-12 years): basic literacy, numeracy, social skills
   - **Adolescence** (12-16 years): subject specialization, skill foundations
   - **Young Adult** (16-18 years): advanced skills, apprenticeship, graduation
8. Define education subjects:
   - **Literacy**: reading, writing, communication (unlocks library study)
   - **Numeracy**: mathematics, logic, calculation (unlocks research)
   - **Survival**: scavenging, navigation, first aid (unlocks survival skills)
   - **Crafting**: building, repair, manufacturing (unlocks crafting skills)
   - **Medicine**: biology, treatment, care (unlocks medical skills)
   - **Combat**: weapons, tactics, defense (unlocks combat skills)
   - **Social**: leadership, diplomacy, persuasion (unlocks social skills)
   - **Science**: research, analysis, experimentation (unlocks research skills)
9. Define education mechanics:
   - Children progress through stages based on age
   - Each stage has available subjects
   - Subjects require teachers (survivors with skill in subject)
   - Education quality depends on teacher skill, facilities, resources
   - Proficiency gained over time (daily study)
   - Graduation unlocks skills based on subjects studied
10. Define knowledge transfer:
    - Parents can teach children directly (parent-child bonus)
    - Teachers transfer knowledge to students
    - Library provides self-study option (if literate)
    - Shelter knowledge accumulates (traditions, techniques)
    - Knowledge persists across generations (if recorded)
11. Add deterministic seeding: education outcomes use `ISeededRng`
12. Wire into `GameBootstrap`: `SetupEducation`, `TickEducation`, `SaveEducation`
13. Create `CurriculumCatalogLoader` for subject definitions
14. Implement education facilities: schoolroom, library, workshop
15. Create UI hook: education panel showing students, teachers, progress

## Main Task 2 — Implementation / Stages / Subjects / Teachers / Graduation

1. Implement childhood education (0-12 years):
   - Basic literacy: reading, writing (prerequisite for all subjects)
   - Basic numeracy: math, logic (prerequisite for science/crafting)
   - Social skills: cooperation, communication
   - Physical development: health, coordination
   - Taught by any literate survivor or parent
   - Education quality affects future learning capacity
2. Implement adolescence education (12-16 years):
   - Subject specialization: choose 2-3 focus areas
   - Each subject builds proficiency (0-100)
   - Teachers required for each subject (survivor with skill)
   - Parent teaching: +20% learning speed for own children
   - Schoolroom facility: +10% learning speed
   - Education quality affects graduation outcomes
3. Implement young adult education (16-18 years):
   - Advanced subjects: deeper specialization
   - Apprenticeship: paired with expert survivor (ApprenticeshipSystem integration)
   - Graduation project: demonstrate proficiency
   - Graduation unlocks skills based on subjects
   - High proficiency: skill unlocked at higher tier
   - Low proficiency: skill unlocked at basic tier or not at all
4. Implement teacher system:
   - Survivors with high skill can teach
   - Teaching reduces teacher's work capacity (-25%)
   - Teaching improves teacher's skill slightly (+1 XP/day)
   - Multiple students per teacher (up to 5)
   - Teacher skill affects education quality
5. Implement parent-child teaching:
   - Parents can teach own children
   - Parent teaching: +20% learning speed
   - Parent passes traits to child (GenerationalLineageExtension integration)
   - Parent-child bond increases through teaching
   - Orphaned children assigned to guardians
6. Implement library self-study:
   - Literate survivors can study in library
   - Library books provide subject bonuses
   - Self-study slower than teacher-led (50% speed)
   - Library expands with new books (research integration)
   - Library serves as knowledge repository
7. Implement shelter knowledge:
   - Shelter accumulates knowledge over time
   - Knowledge stored in library (books, records)
   - Knowledge persists across generations
   - Knowledge can be lost (library destroyed, teachers die)
   - Knowledge provides shelter-wide bonuses
8. Implement graduation:
   - Graduation at age 18 (or earlier with high proficiency)
   - Graduation ceremony (shelter morale event)
   - Skills unlocked based on subjects studied
   - Graduation traits: "Educated", "Scholar", "Specialist"
   - Graduates become full survivors with education benefits
9. Create education events:
   - "The First Day" — child begins education
   - "The Teacher" — survivor becomes dedicated teacher
   - "The Graduation" — child graduates, skills unlocked
   - "The Prodigy" — child excels, accelerated learning
   - "The Struggle" — child struggles, needs extra help
   - "The Library" — new books added to library
   - "The Legacy" — parent teaches child own specialty
10. Add education quest hooks:
    - "The Schoolhouse" — build dedicated schoolroom
    - "The Teacher's Pet" — help struggling student
    - "The Curriculum" — develop new subject curriculum
    - "The Library" — expand shelter library
    - "The Graduation Speech" — inspire graduating class
    - "The Inheritance" — ensure knowledge passes to next generation
11. Add UI: education panel showing students, teachers, subjects, progress
12. Create education journal: automatic log of education events
13. Implement education tutorial: first child explains system
14. Add education tooltips: hover over student shows progress
15. Create 10 curriculum subjects in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `CohortSystem`: children enter education system
2. Connect to `ApprenticeshipSystem`: young adults apprentice
3. Integrate with `SkillProgressionSystem`: graduation unlocks skills
4. Connect to `LibraryStudySystem`: library provides self-study
5. Wire into `GenerationalLineageExtension`: traits pass to children
6. Connect to `SurvivorRelationsSystem`: parent-child/teacher-student bonds
7. Implement old-save compatibility: existing saves get empty education state
8. Add deterministic seeding: education outcomes use `ISeededRng`
9. Create exploit prevention: education requires time, can't be rushed
10. Add tests: education progression, skill unlocking, graduation, save round-trip
11. Verify catalog integrity: all subject/skill IDs resolve
12. Test edge cases: no teachers (no education), all subjects (max education)
13. Verify headless behavior: education processes correctly without UI
14. Add data-integrity-selftest: curriculum validates against skill catalogs
15. Create `--education-selftest` verb for CI validation

## State / System Interaction Model

```text
Child born/arrives at shelter
├─ Childhood education (0-12 years)
│  ├─ Basic literacy, numeracy, social skills
│  ├─ Taught by parents or any literate survivor
│  └─ Education quality affects future learning
├─ Adolescence education (12-16 years)
│  ├─ Subject specialization (2-3 focus areas)
│  ├─ Teachers required for each subject
│  ├─ Parent teaching bonus (+20% speed)
│  └─ Proficiency gained over time
├─ Young adult education (16-18 years)
│  ├─ Advanced subjects, deeper specialization
│  ├─ Apprenticeship with expert survivor
│  ├─ Graduation project
│  └─ Skills unlocked based on subjects
├─ Graduation (age 18 or earlier)
│  ├─ Ceremony (morale event)
│  ├─ Skills unlocked (based on proficiency)
│  ├─ Graduation traits gained
│  └─ Becomes full survivor with education
├─ Knowledge persistence
│  ├─ Library stores knowledge
│  ├─ Shelter knowledge accumulates
│  ├─ Knowledge passes to next generation
│  └─ Knowledge can be lost (library destroyed)
└─ Generational continuity
   ├─ Parents teach children
   ├─ Traits pass through lineage
   ├─ Family specialties develop
   └─ Shelter traditions form
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --education-selftest
```

## Risk

**MEDIUM** — Education system complexity can overwhelm players if too many subjects and stages exist. Risk of education feeling like a chore rather than meaningful investment. Mitigation: keep subjects focused (8 core subjects), make education optional (children can mature without formal education), and show clear benefits (educated survivors have skills and traits).

## Definition of Done

- `EducationSystem.cs` exists with full `CaptureState/RestoreState`
- 3 education stages implemented (childhood, adolescence, young adult)
- 8 curriculum subjects defined
- Teacher system functional (skill transfer, work capacity cost)
- Parent-child teaching mechanics working
- Library self-study integrated
- Graduation system unlocks skills
- Shelter knowledge persistence
- Education events and quest hooks
- Save/load round-trip tested
- Deterministic education outcomes verified
- Old saves load without error
- 10 curriculum subjects in data authority
- UI panel shows education progress
- Cross-system integration (cohort, apprenticeship, skills, library, lineage, relations)

## Follow-On Opportunities

- University system (advanced education for adults)
- Education specialization (survivors become dedicated teachers)
- Education legacy (famous schools remembered in epilogue)
- Education quests (develop new curricula, train specialists)
- Education traditions (shelter-specific teaching methods)
