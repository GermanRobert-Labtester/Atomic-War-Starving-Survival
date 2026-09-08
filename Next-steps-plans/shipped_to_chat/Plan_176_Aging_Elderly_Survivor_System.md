# Plan 176 — Aging & Elderly Survivor System

## Goal

Create an aging and elderly survivor system where survivors age over time, progress through life stages (young adult → middle age → elderly), and experience age-related mechanical effects — wisdom bonuses, physical decline, retirement, mentorship roles, and age-specific events. Currently `SurvivorLifecycle.cs` handles birth/death but survivors don't age — a survivor recruited at age 25 is still 25 on Day 365. `CaregivingSystem.cs` references "elderly care" but no aging mechanic exists. This plan adds temporal depth to survivor management and creates generational gameplay.

## Why

**Repository evidence:** Grep for `AgingSystem`, `Elderly`, `OldAge`, `SeniorCitizen`, `AgeRelated` in Core returns 1 match: `SaveSectionRegistry.cs:97` — "Childcare, elderly care, and comfort" description for caregiving section. No aging system exists. `SurvivorLifecycle.cs` handles birth/death but not aging. `CaregivingSystem.cs` mentions elderly care but has no age-related mechanics. `GenerationalSuccessionEngine.cs` (Plan 140) has aging/retirement skeleton but `AdvanceTime()` is never called. Survivors are ageless — they don't grow older, wiser, or frailer.

**What is missing:** No aging system. No life stages. No age-related bonuses or penalties. No retirement mechanics. No elderly care. No mentorship from elders. No age-specific events. Survivors are functionally immortal (until they die) and ageless.

**Why existing plans don't solve it:** Plan 140 (generational legacy) covers cross-campaign inheritance but not in-campaign aging. Plan 150 (romance/family) covers family dynamics but not aging parents. Plan 154 (education) covers skill transfer but not elderly mentors. No plan addresses survivor aging as a gameplay mechanic.

**Player value:** Creates urgency (survivors age, time matters), adds strategic depth (manage aging workforce), generates emotional stories (elder wisdom, retirement, death from old age), and makes the shelter feel like a living community with generational turnover.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` — lifecycle management
- `Assets/Ashfall.Core/Caregiving/CaregivingSystem.cs` — caregiving system
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill system
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` — generational skeleton
- `Assets/StreamingAssets/Data/survivors.json` — survivor definitions
- NEW: `Assets/Ashfall.Core/Survivors/AgingSystem.cs`

## Main Task 1 — Foundation / System Contract

1. Create `AgingSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `LifeStage` DTO: `stageId`, `stageName` (young_adult/prime/middle_age/elderly), `ageRange` (min-max age), `physicalModifier` (0.5-1.5), `mentalModifier` (0.5-1.5), `wisdomModifier` (0.5-1.5), `description`
3. Define `AgeEvent` DTO: `eventId`, `eventType` (birthday/retirement/age_milestone/elderly_care/mentorship), `survivorId`, `age`, `effects` (list of modifiers), `description`
4. Define `AgingState` DTO: list of survivor ages, list of life stages, list of age events, aging rate (days per year), retirement age
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define life stages:
   - **Young Adult** (18-30): peak physical, learning, +energy, -wisdom
   - **Prime** (31-45): balanced physical/mental, +productivity, -recovery
   - **Middle Age** (46-60): declining physical, +wisdom, +skill, -stamina
   - **Elderly** (61+): significantly declined physical, +wisdom, +mentorship, -work capacity
7. Define aging mechanics:
   - Survivors age 1 year per N campaign days (configurable, default 30)
   - Age affects physical capabilities (work speed, combat, expedition)
   - Age affects mental capabilities (skill learning, problem solving)
   - Age grants wisdom bonuses (leadership, mentoring, decision making)
   - Age-related events at milestones (30th, 40th, 50th, 60th birthday)
8. Define retirement mechanics:
   - At retirement age (default 60), survivor can retire
   - Retired survivors: reduced work capacity, +mentorship, +leisure
   - Retirement is optional (can continue working at reduced capacity)
   - Retired survivors provide mentorship bonuses to younger survivors
   - Retirement affects shelter resource consumption
9. Define elderly care:
   - Elderly survivors may need care (health decline)
   - Caregiving assigns survivor to care for elderly
   - Care improves elderly health and morale
   - Lack of care: health decline, morale penalty
   - Elderly care integrates with `CaregivingSystem`
10. Define mentorship:
    - Elderly survivors can mentor younger survivors
    - Mentorship accelerates skill learning
    - Mentorship transfers knowledge (reduced skill decay)
    - Mentorship strengthens inter-generational bonds
    - Mentor events: "The Mentor" — elder teaches apprentice
11. Define age-related events:
    - Birthday celebrations (annual, morale boost)
    - Age milestone events (30th, 40th, 50th, 60th)
    - Retirement ceremony (when survivor retires)
    - Elder wisdom events (elder provides advice)
    - Age-related death (death from old age, typically 70-80)
12. Add deterministic seeding: aging uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupAging`, `TickAging`, `SaveAging`
14. Implement aging UI: survivor detail shows age, life stage, age-related modifiers
15. Create age event journal: automatic log of aging events

## Main Task 2 — Implementation / Aging / Stages / Retirement / Mentorship

1. Implement age tracking:
   - Each survivor has current age (starts at recruitment age or birth)
   - Age increments based on aging rate (1 year per N days)
   - Age displayed in survivor detail panel
   - Age affects all age-dependent mechanics
2. Implement life stage transitions:
   - Survivor progresses through life stages based on age
   - Each stage has distinct modifiers (physical, mental, wisdom)
   - Stage transition events logged
   - Stage affects work assignments and capabilities
3. Implement physical decline:
   - Physical modifier decreases with age
   - Elderly survivors: slower work, reduced combat, limited expedition
   - Physical decline affects health and stamina
   - Health care can slow decline
4. Implement wisdom growth:
   - Wisdom modifier increases with age
   - Elderly survivors: better leadership, mentoring, decisions
   - Wisdom affects skill check bonuses
   - Wisdom transfers through mentorship
5. Implement retirement:
   - At retirement age, survivor offered retirement option
   - Retirement: reduced work, +mentorship, +leisure time
   - Retirement ceremony event
   - Retired survivors provide passive mentorship bonuses
   - Retirement affects shelter resource allocation
6. Implement elderly care:
   - Elderly survivors may need care (health check)
   - Caregiver assigned to elderly care
   - Care improves health and morale
   - Lack of care: health decline, possible death
   - Care integrates with `CaregivingSystem`
7. Implement mentorship:
   - Elderly survivors paired with young apprentices
   - Mentorship accelerates apprentice skill learning
   - Mentorship events logged
   - Mentorship strengthens relationships
   - Mentor death: apprentice grief event
8. Implement age-related death:
   - Death chance increases with age (70+ years)
   - Peaceful death from old age (natural causes)
   - Death event logged, memorial created
   - Age death affects shelter morale
   - Legacy: deceased elder's wisdom remembered
9. Create aging events:
   - "The Birthday" — annual birthday celebration
   - "The Milestone" — age milestone reached (30/40/50/60)
   - "The Retirement" — survivor retires
   - "The Mentor" — elder begins mentoring
   - "The Wisdom" — elder provides advice
   - "The Decline" — health declining
   - "The Passing" — death from old age
10. Add aging quest hooks:
    - "The Elder" — have 3 elderly survivors in shelter
    - "The Mentor" — elder mentors 5 apprentices
    - "The Celebration" — hold birthday celebration
    - "The Retirement" — retire 3 survivors
    - "The Caregiver" — provide elderly care for 10 survivors
    - "The Legacy" — elder's wisdom remembered after death
    - "The Generation" — 3 generations in shelter simultaneously
11. Implement aging UI:
    - Survivor detail: age, life stage, age modifiers
    - Age list: all survivors sorted by age
    - Retirement panel: manage retired survivors
    - Elderly care panel: assign caregivers
    - Mentorship panel: pair mentors/apprentices
12. Add aging journal: automatic log of aging events
13. Implement aging tutorial: first birthday explains system
14. Add age tooltips: hover over age shows life stage and modifiers
15. Create aging data: life stage definitions, retirement age, aging rate

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SurvivorLifecycle`: age tracked from birth/recruitment
2. Connect to `CaregivingSystem`: elderly care integration
3. Integrate with `SkillProgressionSystem`: wisdom bonuses, mentorship
4. Connect to `GenerationalSuccessionEngine`: aging advances time
5. Wire into `ShelterArchiveSystem` (Plan 162): age events recorded
6. Connect to `SeasonalEventSystem` (Plan 170): birthday celebrations
7. Implement old-save compatibility: existing survivors get estimated ages
8. Add deterministic seeding: aging uses `ISeededRng`
9. Create exploit prevention: aging is time-based, can't be rushed
10. Add tests: age progression, life stages, retirement, mentorship, death, save round-trip
11. Verify catalog integrity: all life stage IDs resolve
12. Test edge cases: no elderly (young shelter), many elderly (aging shelter)
13. Verify headless behavior: aging processes correctly without UI
14. Add data-integrity-selftest: life stages validate against survivor catalogs
15. Create `--aging-selftest` verb for CI validation

## State / System Interaction Model

```text
Aging & elderly system
├─ Age tracking
│  ├─ Each survivor has age
│  ├─ Age increments (1 year per N days)
│  ├─ Age displayed in UI
│  └─ Age affects mechanics
├─ Life stages
│  ├─ Young Adult (18-30): peak physical
│  ├─ Prime (31-45): balanced
│  ├─ Middle Age (46-60): declining physical, +wisdom
│  └─ Elderly (61+): declined physical, +wisdom, +mentorship
├─ Physical decline
│  ├─ Physical modifier decreases
│  ├─ Slower work, reduced combat
│  ├─ Health and stamina affected
│  └─ Health care slows decline
├─ Wisdom growth
│  ├─ Wisdom modifier increases
│  ├─ Better leadership, mentoring
│  ├─ Skill check bonuses
│  └─ Transfers through mentorship
├─ Retirement
│  ├─ Optional at retirement age
│  ├─ Reduced work, +mentorship
│  ├─ Retirement ceremony
│  └─ Passive mentorship bonuses
├─ Elderly care
│  ├─ May need care (health check)
│  ├─ Caregiver assigned
│  ├─ Care improves health/morale
│  └─ Lack of care: decline/death
└─ Integration
   ├─ Lifecycle (age from birth)
   ├─ Caregiving (elderly care)
   ├─ Skills (wisdom, mentorship)
   ├─ Succession (aging advances time)
   ├─ Archive (age events recorded)
   └─ Seasons (birthday celebrations)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --aging-selftest
```

## Risk

**LOW** — Aging is straightforward with clear inputs (time) and outputs (life stages, modifiers). Risk of aging feeling punitive rather than meaningful. Mitigation: balance physical decline with wisdom growth, make retirement rewarding, make mentorship impactful, and ensure elderly survivors contribute value.

## Definition of Done

- `AgingSystem.cs` exists with full `CaptureState/RestoreState`
- 4 life stages implemented (young adult through elderly)
- Age tracking and progression functional
- Physical decline and wisdom growth mechanics
- Retirement system with ceremony
- Elderly care integration with CaregivingSystem
- Mentorship system (elder → apprentice)
- Age-related death mechanics
- Aging events and quest hooks
- Save/load round-trip tested
- Deterministic aging verified
- Old saves get estimated ages
- Life stage definitions in data authority
- UI showing age, life stage, modifiers
- Cross-system integration (lifecycle, caregiving, skills, succession, archive, seasons)

## Follow-On Opportunities

- Age-specific traits (elderly wisdom, youth energy)
- Age legacy (famous elders remembered)
- Age quests (specific age milestones)
- Age ceremonies (coming of age, retirement parties)
- Age demographics (shelter age distribution management)
