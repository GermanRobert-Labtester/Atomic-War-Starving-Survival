# Plan 183 — Child Development Stages System

## Goal

Create a child development stages system where children grow through distinct developmental phases (infant → toddler → child → adolescent → young adult) with age-appropriate capabilities, needs, learning, and events. Currently `CohortSystem.cs` (174 lines) handles children but `TryMaturation()` is a boolean flag — children instantly become adults with no developmental arc. Plan 154 (education) adds schooling but not developmental stages. Plan 176 (aging) adds aging for adults but not child-specific development. This plan adds generational depth by making childhood a meaningful phase.

## Why

**Repository evidence:** Grep for `ChildDevelopment`, `ChildGrowth`, `Adolescent`, `Teenager`, `ChildAging`, `child_age` in Core returns ZERO matches. `CohortSystem.cs` (174 lines) tracks children with `isMatured` boolean — maturation is instant. No developmental stages, no age-gated capabilities, no childhood events, no adolescent behavior. Plan 150 (romance/family) mentions "parent-child relationship affects child development" but doesn't implement it. Children are either immature or mature — nothing in between.

**What is missing:** No developmental stages. No infant/toddler/child/adolescent phases. No age-gated capabilities. No childhood events. No adolescent behavior. No learning progression. Children are a binary state (immature/mature) with no developmental journey.

**Why existing plans don't solve it:** Plan 154 (education) adds schooling but not developmental stages. Plan 176 (aging) adds aging for adults but not child development. Plan 140 (legacy) covers generational inheritance but not childhood. Plan 150 (romance/family) adds family but not child development. No plan addresses developmental stages.

**Player value:** Creates emotional investment (watch children grow), adds strategic depth (manage child development), generates emergent stories (first steps, first words, adolescent rebellion), and makes generational gameplay meaningful.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/CohortSystem.cs` — current child system
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` — lifecycle
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — needs
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skills
- `Assets/Ashfall.Core/Education/` (Plan 154) — education system
- NEW: `Assets/Ashfall.Core/Cohort/ChildDevelopmentSystem.cs`
- NEW: `Assets/StreamingAssets/Data/development_stages.json`

## Main Task 1 — Foundation / System Contract

1. Create `ChildDevelopmentSystem.cs` in `Assets/Ashfall.Core/Cohort/`
2. Define `DevelopmentStage` DTO: `stageId`, `stageName` (infant/toddler/child/adolescent/young_adult), `ageRange` (min-max), `capabilities` (list of what they can/cannot do), `needs` (special needs for stage), `learningRate` (skill learning multiplier), `socialBehavior` (description), `events` (list of stage-specific events)
3. Define `ChildState` DTO: `childId`, `currentStage`, `age` (days), `parentIds` (list), `caregiverId` (assigned caregiver), `developmentProgress` (0-100 within stage), `learnedSkills` (list), `personality` (emerging traits), `health` (stage-specific health modifiers)
4. Define `DevelopmentEvent` DTO: `eventId`, `childId`, `eventType` (first_steps/first_words/reading/walking_talking/adolescent_rebellion/coming_of_age), `day`, `description`, `effects` (list)
5. Define `ChildDevelopmentState` DTO: list of child states, list of development events, stage transition log, developmental milestones achieved
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define 5 developmental stages:
   - **Infant** (0-1 year): needs constant care, cannot move, learns basic trust
   - **Toddler** (1-3 years): begins walking/talking, needs supervision, learns language
   - **Child** (3-12 years): can help with simple tasks, learns skills, needs education
   - **Adolescent** (12-16 years): capable worker, rebellious phase, identity formation
   - **Young Adult** (16-18 years): full capabilities, preparing for adulthood, transitions to adult system
8. Define stage capabilities:
   - Infant: no capabilities, constant care required
   - Toddler: movement, basic communication, needs supervision
   - Child: simple tasks, skill learning, education, light chores
   - Adolescent: full work capability, skill specialization, leadership potential
   - Young Adult: full adult capabilities, transitions to `SurvivorLifecycle`
9. Define stage-specific needs:
   - Infant: feeding, diapering, comfort, safety
   - Toddler: supervision, nutrition, stimulation, safety
   - Child: education, nutrition, socialization, safety
   - Adolescent: autonomy, guidance, nutrition, purpose
   - Young Adult: adult needs, career direction
10. Define learning progression:
    - Each stage has learning rate multiplier
    - Children learn faster than adults
    - Education quality affects learning (Plan 154 integration)
    - Caregiver quality affects development
    - Nutritional status affects learning
11. Define developmental events:
    - First smile (infant)
    - First steps (toddler)
    - First words (toddler)
    - Learning to read (child)
    - First chore (child)
    - Adolescent rebellion (adolescent)
    - Coming of age ceremony (young adult)
    - Transition to adult (young adult)
12. Add deterministic seeding: development uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupChildDevelopment`, `TickChildDevelopment`, `SaveChildDevelopment`
14. Create `DevelopmentStageCatalogLoader` for stage definitions
15. Implement child development UI: child detail panel showing stage, progress, needs

## Main Task 2 — Implementation / Stages / Events / Care / Education

1. Implement stage progression:
   - Children age daily (days increment)
   - At age threshold: stage transition
   - Transition event logged
   - New stage capabilities applied
   - Development continues within stage
2. Implement infant care:
   - Infants require constant caregiver
   - Caregiver feeds, comforts, cares for infant
   - Care quality affects development speed
   - Lack of care: health decline, developmental delay
   - Infant events: first smile, first laugh
3. Implement toddler development:
   - Toddlers begin moving and communicating
   - Supervision required (less than infant)
   - Language development events
   - Motor skill events (first steps, running)
   - Toddler can follow simple instructions
4. Implement child education:
   - Children can learn skills (Plan 154 integration)
   - Education quality affects learning rate
   - Children can do light chores
   - Social development through play
   - Child events: learning to read, first chore
5. Implement adolescent phase:
   - Adolescents can work full-time
   - Rebellious behavior (occasional disobedience)
   - Identity formation (personality traits emerge)
   - Skill specialization begins
   - Adolescent events: rebellion, coming of age
6. Implement young adult transition:
   - At age 18: transition to adult system
   - Young adult becomes full survivor
   - Skills and traits carry over
   - Transition event: coming of age ceremony
   - Young adult enters `SurvivorLifecycle`
7. Implement caregiver assignment:
   - Caregiver assigned to child
   - Caregiver quality affects development
   - Multiple children per caregiver
   - Caregiver can be parent or other survivor
   - Caregiver relationship affects child morale
8. Implement developmental consequences:
   - Good care: accelerated development, better outcomes
   - Poor care: developmental delays, health issues
   - Education quality: affects skill learning
   - Nutrition: affects health and learning
   - Social environment: affects personality
9. Create development events:
   - "The First Smile" — infant smiles for first time
   - "The First Steps" — toddler walks
   - "The First Words" — toddler speaks
   - "The Reading" — child learns to read
   - "The First Chore" — child helps with work
   - "The Rebellion" — adolescent rebels
   - "The Coming of Age" — transition to adulthood
   - "The Adult" — child becomes full survivor
10. Add development quest hooks:
    - "The Parent" — raise child to adulthood
    - "The Teacher" — educate child through all stages
    - "The Caregiver" — care for infant through toddler
    - "The Guide" — guide adolescent through rebellion
    - "The Ceremony" — hold coming of age ceremony
    - "The Generation" — raise 3 children to adulthood
    - "The Legacy" — child becomes skilled adult
11. Implement development UI:
    - Child detail: stage, age, progress, needs
    - Caregiver assignment panel
    - Development log: events and milestones
    - Education panel (Plan 154 integration)
    - Stage transition notification
12. Add development journal: automatic log of development events
13. Implement development tutorial: first child birth explains system
14. Add development tooltips: hover over stage shows capabilities
15. Create 5 stage definitions in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `CohortSystem`: replaces boolean maturation with stage progression
2. Connect to `SurvivorLifecycle`: young adult transitions to adult
3. Integrate with `NeedsSystem`: child-specific needs
4. Connect to `SkillProgressionSystem`: child skill learning
5. Wire into `EducationSystem` (Plan 154): education integration
6. Connect to `AgingSystem` (Plan 176): aging integration
7. Implement old-save compatibility: existing children get estimated ages/stages
8. Add deterministic seeding: development uses `ISeededRng`
9. Create exploit prevention: development is time-based, can't be rushed
10. Add tests: stage progression, care effects, education, transition, save round-trip
11. Verify all stages progress correctly
12. Test edge cases: no care (developmental delay), excellent care (accelerated)
13. Verify headless behavior: development processes correctly without UI
14. Add data-integrity-selftest: stages validate against age/skill catalogs
15. Create `--child-development-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --child-development-selftest
```

## Risk

**LOW** — Child development is straightforward with clear inputs (age, care, education) and outputs (stage progression, events). Risk of children feeling like a chore rather than joy. Mitigation: make events meaningful, show clear progress, allow player investment in outcomes, and ensure children contribute value as they grow.

## Definition of Done

- `ChildDevelopmentSystem.cs` exists with full `CaptureState/RestoreState`
- 5 developmental stages (infant through young adult)
- Stage progression based on age
- Stage-specific capabilities and needs
- Caregiver assignment system
- Development events and milestones
- Education integration (Plan 154)
- Young adult transition to adult system
- Development events and quest hooks
- Save/load round-trip tested
- Deterministic development verified
- Old saves get estimated stages
- 5 stage definitions in data authority
- UI child detail panel
- Cross-system integration (cohorts, lifecycle, needs, skills, education, aging)

## Follow-On Opportunities

- Child specialization (children develop unique talents)
- Child legacy (famous children remembered)
- Child quests (specific development goals)
- Child events (school plays, childhood friendships)
- Child inheritance (children inherit parent traits)
