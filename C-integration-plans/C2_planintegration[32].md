# C2 — Flagship Integration Plan [32]: Survivor Education, Knowledge Transfer, Apprenticeship Continuity, and Multi-Generation Learning

> **Deliverable:** `C2_planintegration[32].md`
> **Source scope:** Plan 154 — *Survivor Education & Knowledge Transfer*
> **Primary objective:** create a structured, deterministic education layer that lets children and adolescents accumulate real learning over time, connects teacher-led instruction, parent teaching, library study, apprenticeship, skill progression, lineage, and shelter knowledge into one coherent development path, and ensures a child who matures inside the shelter is no longer a blank-slate adult—without creating duplicate skill, age, lineage, library, or relationship authorities.
> **Required execution order:** **154A Foundation/System Contract → 154B Stages, Subjects, Teachers, Graduation & Knowledge Content → 154C Cross-System Integration, Save/CI, Balance, and Generational Closure**
> **Hard dependencies:** `CohortSystem`, `ApprenticeshipSystem`, `LibraryStudySystem`, `GenerationalLineageExtension`, `SkillProgressionSystem`, `SurvivorRelationsSystem`, family/caregiving systems where present, Plan 31 semantic events, Plan 36 ports, Plan 39 save durability, Plan 55 retention, Plan 150 family dynamics where applicable.
> **Scope discipline:** no duplicate age/maturation authority, no duplicate skill XP ledger, no duplicate trait inheritance engine, no duplicate library/research inventory, no “education quality” stat that silently replaces real teacher/facility/resource inputs, no instant skill grants from a single event, no mandatory education path that makes uneducated survivors invalid, no adult/child capability drift between UI and runtime, and no child labor/education mechanic that bypasses canonical duty/age restrictions.

---

# 0. Executive Intent

ASHFALL already contains several systems that imply generational learning:

- children and maturation,
- apprenticeship,
- library study,
- skill progression,
- parent/child lineage,
- caregiver relationships,
- research,
- shelter facilities,
- generational continuity.

But those systems currently do not form a developmental pipeline.

The present conceptual gap is:

```text
child exists
→ time passes
→ TryMaturation()
→ adult survivor
```

with little or no record of:

- what the child learned,
- who taught them,
- which shelter institutions shaped them,
- which parent specialty influenced them,
- which library knowledge survived,
- which apprenticeship they completed.

The intended architecture is:

```text
CohortSystem age / maturation
        │
        ▼
   EducationSystem
        │
        ├─ childhood foundations
        ├─ adolescent specialization
        ├─ young-adult study
        ├─ teacher assignments
        ├─ parent/guardian teaching
        └─ education progression
        │
        ▼
 canonical learning owners
        │
   ┌────┼─────────┬─────────┬─────────┐
   ▼    ▼         ▼         ▼         ▼
skills library apprenticeship lineage relations
        │
        ▼
  graduation / maturation
        │
        ▼
 adult survivor with earned continuity
```

The strongest product outcome is:

> **A child raised in the bunker can become a recognizably different adult because of the teachers, library, family, apprenticeships, facilities, and crises that shaped their education—while every resulting skill, trait, relationship, and lineage fact remains owned by the systems that already govern those facts.**

---

# 1. Source Diagnosis

The source establishes:

- `CohortSystem.TryMaturation()` currently flips a maturity flag without educational development,
- `ApprenticeshipSystem` transfers XP but does not model formal education,
- `LibraryStudySystem` supports study but not structured childhood/adolescent education,
- `GenerationalLineageExtension` has inherited-trait fields but lacks robust population,
- children currently mature as blank slates,
- proposed stages are:
  - childhood,
  - adolescence,
  - young adult,
- proposed subjects include:
  - literacy,
  - numeracy,
  - survival,
  - crafting,
  - medicine,
  - combat,
  - social,
  - science,
- teachers, parents, libraries, facilities, and apprenticeships should all contribute,
- shelter knowledge should persist across generations,
- 10 curriculum subjects are requested in data,
- old saves, deterministic outcomes, headless processing, UI, quests, and CI are all required.

The key architectural correction is:

```text
EducationSystem owns progress toward learning outcomes
```

but:

```text
SkillProgressionSystem owns skills
GenerationalLineageExtension owns inherited traits
CohortSystem owns age/maturation
LibraryStudySystem owns library study
ApprenticeshipSystem owns apprenticeship transfer
SurvivorRelationsSystem owns relationship changes
```

---

# 2. Program-Level Success Criteria

C2[32] closes only when all of the following are true.

1. Children enter education automatically according to canonical age/stage rules.
2. Education stages derive from canonical age, not a duplicate age counter.
3. Education records persist proficiency and study history.
4. Skills are unlocked/applied through `SkillProgressionSystem`.
5. Apprenticeship outcomes route through `ApprenticeshipSystem`.
6. Library study routes through `LibraryStudySystem`.
7. Trait inheritance routes through `GenerationalLineageExtension`.
8. Parent/teacher relationship effects route through `SurvivorRelationsSystem`.
9. Teacher workload uses canonical duty/work capacity.
10. Education can function with teachers, parents, or library self-study depending context.
11. Lack of formal education does not invalidate maturation.
12. Graduation/maturation cannot be rushed by save/load or UI actions.
13. Old saves initialize education records safely.
14. Education progression is deterministic.
15. 10 curriculum subjects validate against real skill/content catalogs.
16. UI displays the exact state used by runtime.
17. Shelter knowledge is represented by a real repository/facility/knowledge authority, not a vague global number unless one is explicitly justified.
18. Education remains optional but strategically valuable.
19. Headless long-run tests produce valid educated and uneducated adults.
20. Multi-generation tests prove knowledge continuity and loss conditions.

---

# 3. Architectural Invariants

## 3.1 CohortSystem owns age and maturation

EducationSystem queries:

```text
age
life stage
maturation readiness
```

It does not increment age separately.

## 3.2 EducationSystem owns educational progress

It may persist:

- subject proficiency,
- enrolled curriculum,
- teachers,
- study history,
- graduation status.

## 3.3 SkillProgressionSystem owns skill XP/levels

EducationSystem produces:

```text
skill-award / unlock intent
```

not raw duplicated skill state.

## 3.4 ApprenticeshipSystem remains the advanced practical-learning authority

Education integrates with it.

It does not reimplement apprentice XP.

## 3.5 LibraryStudySystem remains library self-study authority

Education schedules/routes into it.

## 3.6 Lineage system owns inherited traits

Education may influence which learned traits/skills become eligible.

It does not copy traits itself.

## 3.7 Relationship system owns bonds

Teacher-student and parent-child educational history becomes relation history through canonical APIs.

## 3.8 Education quality is derived

Teacher skill, facility quality, resources, and attendance produce education quality.

Avoid a free-floating persistent quality meter unless needed as historical summary.

## 3.9 Learning takes time

No single-click graduation/skill unlock.

## 3.10 Children may mature without formal education

Education affects outcome quality, not basic personhood validity.

---

# 4. Dependency Graph

```text
CohortSystem age/maturation
          │
          ▼
    EducationSystem
          │
    ┌─────┼────────┬─────────────┐
    ▼     ▼        ▼             ▼
 teachers parents library   apprenticeship
    │     │        │             │
    └─────┴────────┴─────────────┘
          │
          ▼
 subject proficiency
          │
          ▼
 graduation / maturation outcome
          │
    ┌─────┼────────┬─────────────┐
    ▼     ▼        ▼             ▼
 skills lineage relations     journal/quests
```

---

# 5. Baseline Capture

Before implementation, record:

- CohortSystem age/maturation fields,
- maturation timing semantics,
- SkillProgressionSystem API,
- ApprenticeshipSystem pairing/XP flow,
- LibraryStudySystem study requirements,
- current skill catalog IDs,
- GenerationalLineageExtension fields,
- parent/guardian data,
- survivor teaching-relevant skills/professions,
- room/facility definitions,
- duty/work-capacity model,
- current child-related UI routes,
- save sections.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture:

```text
one child
one adolescent if fixture exists
one mature survivor
one literate teacher
one skilled craft/medical teacher
library present/absent
```

---

# 6. Workstream 154A — Foundation / System Contract

## Goal

Create one deterministic education authority for learning progression while preserving existing age, skill, library, apprenticeship, lineage, relation, and work ownership.

---

# 7. 154A Phase A — Create `EducationSystem`

Path:

```text
Assets/Ashfall.Core/Survivors/EducationSystem.cs
```

Responsibilities:

- determine education stage from canonical age,
- manage enrollment,
- track subject proficiency,
- manage teacher/student assignments,
- calculate daily learning progress,
- expose graduation readiness,
- coordinate downstream learning systems,
- capture/restore education-specific state.

---

# 8. 154A Phase B — Define `EducationStage`

Prefer stable enum/definition:

```text
Childhood
Adolescence
YoungAdult
Graduated
```

Age thresholds come from config/data and must align with CohortSystem.

---

# 9. 154A Phase C — Do Not Duplicate Age

Do not persist:

```text
education_age
```

or a second time-to-stage field.

Stage is derived from canonical age plus graduation state.

---

# 10. 154A Phase D — Define `EducationRecord`

Recommended fields:

```text
survivor_id
enrolled_subject_ids
subject_proficiency
teacher_assignments
attendance_state
education_history_landmarks
graduation_status
graduation_day
```

Do not store derived teacher quality or skill levels.

---

# 11. 154A Phase E — Subject Proficiency

Per subject:

```text
0..100
```

This is educational proficiency, not the same as skill XP.

Document mapping from proficiency to skill-award eligibility.

---

# 12. 154A Phase F — Define `CurriculumDefinition`

Create:

```text
subject_id
name_key
stage availability
prerequisite subject IDs
linked skill IDs
required proficiency
base learning rate
required facility tags
teacher eligibility tags/skills
resource requirements
```

---

# 13. 154A Phase G — Curriculum Data Authority

Create:

```text
Assets/StreamingAssets/Data/education_curriculum.json
```

One canonical source.

No duplicate hardcoded subject list in C#.

---

# 14. 154A Phase H — Resolve 8 vs 10 Subject Source Ambiguity

The source describes 8 core subject families but requires 10 curriculum subjects in data.

Do not invent silently.

Recommended approach:

- preserve the 8 source core subjects,
- add 2 clearly documented specializations only after auditing existing skill catalog,
- or model 10 curriculum rows as specializations under the 8 families.

Example:

```text
Literacy
Numeracy
Survival
Crafting
Medicine
Combat
Social
Science
Agriculture
Engineering
```

Only use final IDs supported by repository content.

---

# 15. 154A Phase I — Learning Capacity

Avoid a standalone immutable “learning capacity” score unless it is truly authored.

Prefer derived factors:

- age stage,
- health,
- fatigue,
- teacher quality,
- facility,
- attendance,
- prior foundational proficiency.

If a bounded learner modifier exists, document source and persistence.

---

# 16. 154A Phase J — Education Quality

Compute from:

```text
teacher skill
teacher availability
student readiness
facility quality
study resources
class size
attendance
```

Do not persist a stale total.

---

# 17. 154A Phase K — Teacher Eligibility

A teacher must satisfy:

- adult/mature,
- sufficient relevant skill,
- fitness/availability,
- not incapacitated,
- valid relationship/context if required.

---

# 18. 154A Phase L — Teacher Capacity

Source proposes up to 5 students per teacher.

Treat as data/config.

Class-size penalty may apply after optimum.

---

# 19. 154A Phase M — Teacher Work Cost

Source proposes:

```text
-25% work capacity
```

Route through canonical duty/work system.

Prefer explicit teaching duty assignment.

---

# 20. 154A Phase N — Teacher Skill Growth

Source proposes `+1 XP/day`.

Route through SkillProgressionSystem.

Tune with anti-farming cap.

---

# 21. 154A Phase O — Parent/Guardian Teaching

Parent/guardian may receive:

- scheduling preference,
- learning-rate bonus,
- relation event.

Source seed:

```text
+20% learning speed
```

Make data-driven.

---

# 22. 154A Phase P — Guardian Support

For orphaned children:

- use canonical guardian/caregiving data,
- allow guardian teaching where eligible,
- never fabricate parentage.

---

# 23. 154A Phase Q — Library Self-Study

Eligibility:

```text
literacy requirement
library access
subject material available
```

Execution routes through LibraryStudySystem.

---

# 24. 154A Phase R — Self-Study Rate

Source proposes 50% of teacher-led pace.

Treat as balance default.

---

# 25. 154A Phase S — Facility Model

Candidate facilities:

```text
schoolroom
library
workshop
```

But do not create duplicate facility state.

Use existing shelter-room/facility authority.

---

# 26. 154A Phase T — Schoolroom

If no schoolroom exists:

- add as canonical facility/buildable only if building system supports it,
- otherwise use generic study space modifier as transitional implementation.

Do not create an EducationSystem-only room flag.

---

# 27. 154A Phase U — Workshop Practical Study

Crafting/engineering instruction may require workshop access.

Use canonical workshop availability/power.

---

# 28. 154A Phase V — Daily Tick

Education progresses at explicit cadence:

```text
once per day / scheduled study period
```

No per-frame learning.

---

# 29. 154A Phase W — Attendance

A child/student can only progress if:

- present,
- not critically ill,
- not assigned to conflicting activity,
- teacher/facility available if required.

---

# 30. 154A Phase X — Deterministic Progress

Base daily progress is deterministic from state.

If “prodigy/struggle” event variation uses RNG:

- use seeded event system,
- persist event identity/outcome.

Core progress itself should not need random noise.

---

# 31. 154A Phase Y — Save State

Persist only:

- records,
- proficiency,
- enrollment,
- attendance/cooldown state,
- graduation status,
- education-history landmarks.

Do not persist:

- derived age/stage,
- teacher skill,
- facility quality,
- skill XP copy.

---

# 32. 154A Phase Z — Old Save Compatibility

Missing section:

```text
valid
```

Migration policy for existing children:

- create record on next education tick,
- derive current stage from age,
- start with documented default proficiency.

Do not retroactively fabricate years of education.

---

# 33. 154A Phase AA — Late-Join Child Policy

If a 15-year-old enters the shelter:

- derive adolescence stage,
- initialize baseline proficiency according to authored newcomer background if available,
- otherwise conservative default.

No assumption they had zero education unless content says so.

---

# 34. 154A Phase AB — Graduation Readiness

Graduation readiness is based on:

- age/stage,
- required foundational proficiency,
- specialization threshold,
- apprenticeship/project completion if configured.

---

# 35. 154A Phase AC — Early Graduation

Source allows earlier graduation with high proficiency.

Gate behind:

- minimum age,
- exceptional proficiency,
- all mandatory prerequisites.

No child-to-adult skip that conflicts with CohortSystem age rules.

If CohortSystem cannot support early maturity:

```text
early academic graduation != adult maturation
```

Keep them separate.

---

# 36. 154A Phase AD — Maturation Boundary

Education graduation and biological/social maturation are not automatically identical.

Define:

```text
education graduation
```

and:

```text
CohortSystem maturation
```

as coordinated but separate states.

---

# 37. 154A Phase AE — Skill Unlock Intent

On graduation/proficiency milestone:

```text
EducationSystem
→ SkillProgressionSystem award/unlock request
```

Skill system decides final skill state.

---

# 38. 154A Phase AF — Lineage Integration Intent

Education may record:

```text
parent specialty taught
mentor influence
```

Lineage system owns inherited traits.

---

# 39. 154A Phase AG — Relationship Event Intent

Teaching may create:

- parent-child bond reason,
- teacher-student bond reason.

Relations system owns actual pair delta/history.

---

# 40. 154A Phase AH — Semantic Events

Candidate event kinds:

```text
education_started
subject_enrolled
teacher_assigned
education_milestone
apprenticeship_started
graduation_completed
education_interrupted
library_knowledge_lost
knowledge_restored
```

Use Plan 31 governance.

---

# 41. 154A Phase AI — Port Contract

Required sinks:

- CohortSystem,
- SkillProgressionSystem,
- ApprenticeshipSystem,
- LibraryStudySystem,
- GenerationalLineageExtension,
- SurvivorRelationsSystem,
- duty/work,
- quest/journal.

Missing required ports fail validation.

---

# 42. 154A Phase AJ — Diagnostics

Expose:

```text
EDUCATION_STUDENTS
EDUCATION_TEACHERS
EDUCATION_SUBJECTS
STUDENTS_WITHOUT_PATH
GRADUATION_READY
REQUIRED_PORTS_MISSING
```

---

# 43. 154A Tests

- stage derivation,
- no duplicate age,
- teacher eligibility,
- class capacity,
- parent bonus,
- library self-study,
- facility requirement,
- deterministic progress,
- old save,
- late-join child,
- graduation readiness,
- early-graduation guard,
- missing-port failure.

---

# 44. 154A Definition of Done

- [ ] EducationSystem,
- [ ] EducationRecord,
- [ ] stage derivation from CohortSystem,
- [ ] curriculum schema/data authority,
- [ ] 8-vs-10 subject decision resolved explicitly,
- [ ] teacher eligibility/capacity,
- [ ] work-cost integration,
- [ ] parent/guardian teaching,
- [ ] library self-study,
- [ ] facility integration,
- [ ] deterministic daily progress,
- [ ] save/old-save,
- [ ] graduation readiness,
- [ ] skill/lineage/relation intents,
- [ ] ports,
- [ ] events,
- [ ] diagnostics.

---

# 45. Workstream 154B — Stages, Subjects, Teachers, Graduation & Knowledge Content

## Goal

Implement a bounded three-stage education loop, 10 data-authored curricula, teacher/parent/library pathways, apprenticeship, graduation outcomes, and a real shelter-knowledge continuity model.

---

# 46. 154B Phase A — Childhood Stage

Source band:

```text
0–12
```

Before hardcoding, align with canonical age/maturity representation.

Core goals:

- literacy,
- numeracy,
- social foundations,
- basic survival/health awareness.

---

# 47. 154B Phase B — Childhood Literacy

Literacy should gate/modify:

- library self-study,
- written information use,
- later advanced subjects.

Use canonical skill/tag if literacy already exists.

---

# 48. 154B Phase C — Childhood Numeracy

Numeracy may gate:

- science,
- engineering/crafting,
- research-related study.

Avoid making it a universal requirement if the game lacks that skill taxonomy.

---

# 49. 154B Phase D — Childhood Social Development

Use education proficiency, not a new personality rewrite.

Potential outcomes:

- relation/communication skill unlock,
- better social skill XP.

---

# 50. 154B Phase E — Physical Development

Source mentions health/coordination.

Do not create a second physical-development system.

At most:

- basic survival/fitness education,
- no skill unlock if canonical health system does not support it.

---

# 51. 154B Phase F — Adolescence Stage

Source band:

```text
12–16
```

Core loop:

- select 2–3 specializations,
- teacher-led study,
- practical sessions,
- milestones.

---

# 52. 154B Phase G — Specialization Selection

Selection may be:

- player-guided,
- survivor preference influenced,
- mixed.

Do not let optimization completely erase survivor identity.

If autonomy exists, expose recommended paths.

---

# 53. 154B Phase H — Subject Capacity

Limit concurrent focus areas.

Source:

```text
2–3
```

Tune by stage.

---

# 54. 154B Phase I — Teacher Matching

Each subject defines teacher qualifications.

Examples:

```text
medicine → medical skill
crafting → repair/crafting skill
science → research skill
combat → combat/tactics skill
```

Use tags/skill IDs.

---

# 55. 154B Phase J — Parent Teaching Bonus

Apply only when:

- actual parent/guardian,
- eligible teacher,
- scheduled together.

No free +20% because a parent exists elsewhere in the shelter.

---

# 56. 154B Phase K — Schoolroom Bonus

Source:

```text
+10%
```

Data-driven.

Requires functioning facility.

If powered/conditioned, use actual facility state.

---

# 57. 154B Phase L — Adolescence Progression

Daily progress formula:

```text
base subject rate
× teacher quality
× facility modifier
× learner readiness
× attendance
× parent/guardian modifier
```

Bounded and explainable.

---

# 58. 154B Phase M — Young Adult Stage

Source band:

```text
16–18
```

Goals:

- advanced specialization,
- apprenticeship,
- graduation project,
- transition into adult survivor role.

---

# 59. 154B Phase N — Apprenticeship Integration

Use `ApprenticeshipSystem`.

EducationSystem supplies:

- subject,
- student,
- eligible mentor,
- education context.

ApprenticeshipSystem owns practical skill XP.

---

# 60. 154B Phase O — Graduation Project

Implement as:

- milestone/event,
- work assignment,
- quest-like task,

depending existing architecture.

Do not create a separate minigame unless needed.

---

# 61. 154B Phase P — Skill Unlocks

Source says:

```text
high proficiency → higher tier
low proficiency → basic or none
```

Translate through SkillProgressionSystem’s actual tier model.

Do not invent tiers if skills use XP only.

---

# 62. 154B Phase Q — Graduation Traits

Source examples:

```text
Educated
Scholar
Specialist
```

Only implement if trait authority supports data-authored acquisition.

Ensure they are not redundant with actual skills.

Prefer descriptive/identity traits with bounded effects.

---

# 63. 154B Phase R — Graduation Ceremony

A morale/social event.

No need for a standalone system.

---

# 64. 154B Phase S — 10 Curriculum Subjects

Final data set should include exactly 10 valid rows, after catalog audit.

Potential specialization mapping:

```text
literacy
numeracy
survival
crafting
medicine
combat
social
science
engineering
agriculture
```

This list is provisional until validated.

---

# 65. 154B Phase T — Subject Prerequisites

Use explicit graph.

Validate:

- no cycles,
- all refs exist,
- stage availability coherent.

---

# 66. 154B Phase U — Curriculum Unlocks

Advanced subjects may require:

- research,
- books,
- facility,
- teacher.

Use canonical research/library/facility state.

---

# 67. 154B Phase V — Teacher System

Teachers are survivors with:

- relevant skill,
- assigned teaching duty,
- capacity.

No separate teacher identity object.

---

# 68. 154B Phase W — Dedicated Teacher Role

“The Teacher” event may formalize a teaching duty/preference.

Do not permanently remove them from other work unless roster assignment does so.

---

# 69. 154B Phase X — Teacher XP Anti-Farming

Teaching can award small XP only if:

- real student attendance,
- actual subject progress,
- daily cap.

No empty-class XP.

---

# 70. 154B Phase Y — Parent-Child Bond

Teaching event can add relation-history reason.

Do not automatically improve affinity every day indefinitely.

Use milestone/significant-event cadence.

---

# 71. 154B Phase Z — Orphan/Guardian Education

Guardian/teacher can fill parent role.

No education dead-end because biological parent died.

---

# 72. 154B Phase AA — Library Self-Study Content

Books/resources should map to subjects.

Use existing library/research catalogs.

No generic “library knowledge = +20 all subjects.”

---

# 73. 154B Phase AB — Shelter Knowledge Authority Audit

The source proposes a shelter knowledge level.

Before implementing:

- inspect whether ResearchSystem/LibraryStudySystem already represents accumulated knowledge.

Preferred:

```text
knowledge repository = books/research/curricula/records
```

rather than one scalar.

---

# 74. 154B Phase AC — If Shelter Knowledge Scalar Is Needed

Only add if a real consumer requires it.

Then define:

- source contributions,
- loss conditions,
- caps,
- meaning,
- persistence.

Do not create a vague global bonus meter.

---

# 75. 154B Phase AD — Knowledge Persistence

Knowledge survives teacher death if recorded in:

- books,
- research,
- curriculum,
- archives.

Unrecorded tacit knowledge may be lost.

This distinction creates meaningful generational pressure.

---

# 76. 154B Phase AE — Knowledge Loss

Library destruction/damage can remove access to:

- books,
- curricula,
- study bonuses.

But do not erase skills already learned by survivors.

---

# 77. 154B Phase AF — Knowledge Recovery

Recovered books, surviving teachers, or research may restore capacity.

---

# 78. 154B Phase AG — Education Events

Source examples:

```text
The First Day
The Teacher
The Graduation
The Prodigy
The Struggle
The Library
The Legacy
```

Expand to bounded event set.

---

# 79. 154B Phase AH — Prodigy Event

Do not create permanent hidden IQ stat.

Possible effect:

- temporary accelerated subject progress,
- one early milestone.

Seeded and bounded.

---

# 80. 154B Phase AI — Struggle Event

State-backed causes:

- poor health,
- absent teacher,
- low attendance,
- prerequisite gap.

Do not randomly punish without context.

---

# 81. 154B Phase AJ — Legacy Event

Parent/mentor passes specialty through actual teaching/apprenticeship.

No direct trait/skill copy.

---

# 82. 154B Phase AK — Quest Hooks

Source:

```text
The Schoolhouse
The Teacher's Pet
The Curriculum
The Library
The Graduation Speech
The Inheritance
```

Use canonical quest runtime.

---

# 83. 154B Phase AL — Education UI

Show:

- students,
- canonical age/stage,
- subjects,
- proficiency,
- teachers,
- attendance,
- facilities,
- graduation readiness.

No hidden future-roll prediction.

---

# 84. 154B Phase AM — Teacher UI

Show:

- teaching load,
- students,
- work-capacity cost,
- subject qualification.

---

# 85. 154B Phase AN — Student Tooltip

Use runtime read model.

Show:

- current stage,
- subjects,
- learning rate factors,
- blockers,
- graduation readiness.

---

# 86. 154B Phase AO — Journal

Significant entries:

- first school day,
- teacher appointed,
- apprenticeship,
- graduation,
- library loss/recovery,
- generational knowledge milestone.

---

# 87. 154B Phase AP — Tutorial

First child/education opportunity explains:

- education is optional,
- teachers cost labor,
- library can substitute partially,
- skills take time,
- maturation still occurs without school.

---

# 88. 154B Phase AQ — Content Integrity

Validate:

- subject IDs,
- prerequisites,
- skill IDs,
- facility tags,
- research/book refs,
- localization keys.

---

# 89. 154B Phase AR — Content Utilization

Run 100/200-day education scenario.

Report:

```text
subjects loaded
subjects enrolled
teachers assigned
library-study sessions
apprenticeships
graduates
skills unlocked
dead curricula
```

---

# 90. 154B Definition of Done

- [ ] childhood education,
- [ ] adolescence specialization,
- [ ] young-adult apprenticeship,
- [ ] 10 validated curricula,
- [ ] teacher matching/capacity,
- [ ] teacher work cost,
- [ ] parent/guardian teaching,
- [ ] library self-study,
- [ ] schoolroom/facility support,
- [ ] knowledge persistence/loss/recovery,
- [ ] graduation,
- [ ] skill unlock routing,
- [ ] graduation traits if justified,
- [ ] education events,
- [ ] quest hooks,
- [ ] UI/tooltips/journal/tutorial,
- [ ] content integrity,
- [ ] utilization report.

---

# 91. Workstream 154C — Cross-System Integration, Save/CI, Balance, and Generational Closure

## Goal

Prove education integrates with maturation, skills, apprenticeship, library, lineage, relations, and long-run generation turnover without save exploits, duplicated state, or mandatory optimization.

---

# 92. 154C Phase A — CohortSystem Integration

On age/stage change:

```text
EducationSystem updates derived stage
```

No separate age tick.

---

# 93. 154C Phase B — Maturation Integration

When CohortSystem matures a survivor:

- education record remains/history closes appropriately,
- earned skill outcomes already applied or applied transactionally,
- adult survivor retains education history.

---

# 94. 154C Phase C — Uneducated Maturation

A child with no formal schooling:

```text
still matures
```

Outcome may lack education-derived advantages.

No invalid survivor.

---

# 95. 154C Phase D — SkillProgression Integration

Graduation/milestones use canonical award APIs.

Tests ensure:

```text
education proficiency != skill XP duplicate
```

---

# 96. 154C Phase E — Apprenticeship Integration

Young adults can have both:

- curriculum progress,
- apprenticeship XP.

Document how they combine.

Avoid double counting same practical learning.

---

# 97. 154C Phase F — LibraryStudy Integration

Self-study sessions contribute education progress through library-owned mechanisms.

Do not add another study timer.

---

# 98. 154C Phase G — Lineage Integration

Source says parents pass traits.

More precise contract:

- inheritable traits are selected through GenerationalLineageExtension,
- educational influence may create learned/mentor provenance separately.

Do not conflate genetic/inherited traits with learned skills.

---

# 99. 154C Phase H — Parent-Child Relations

Teaching milestones can create pair-history reasons.

Use bounded event-based deltas.

---

# 100. 154C Phase I — Teacher-Student Relations

Same approach:

```text
teacher_milestone
student_graduated
teacher_failed_student?
```

Avoid daily affinity grind.

---

# 101. 154C Phase J — FamilySystem Integration

If Plan 150 is live:

- parent/guardian links from family projection aid teacher eligibility,
- education does not own family identity.

---

# 102. 154C Phase K — Duty Roster Integration

Teaching is real work.

Teacher availability and student attendance should respect duty/schedule.

No impossible simultaneous full-time teaching + expedition + critical work.

---

# 103. 154C Phase L — Health/Needs Integration

Severe illness/fatigue/hunger can reduce attendance/learning through canonical readiness modifiers.

Do not duplicate needs.

---

# 104. 154C Phase M — Resource/Facility Integration

If school/library/workshop consumes:

- power,
- materials,
- books,
- tools,

use canonical systems.

---

# 105. 154C Phase N — Save/Load Matrix

Test at:

```text
new student
mid-subject
teacher assigned
library self-study
apprenticeship
pre-graduation
post-graduation
maturation
```

Reload preserves exact progress.

---

# 106. 154C Phase O — Save-Scum Prevention

Reload may not reroll:

- prodigy/struggle event,
- graduation project result,
- teacher event,
- apprenticeship outcome.

Persist event identity/outcome.

---

# 107. 154C Phase P — Old Save Child Migration

Existing child/adolescent:

- record created,
- stage derived,
- no fabricated historical proficiency.

---

# 108. 154C Phase Q — Old Save Mature Survivor

Do not retroactively add education record unless needed for lineage/history.

No fake graduation.

---

# 109. 154C Phase R — No Teachers Edge Case

Source says “no education.”

More nuanced:

- teacher-led education unavailable,
- parent/library self-study may remain if valid,
- otherwise progress stalls,
- maturation still occurs.

---

# 110. 154C Phase S — No Library Edge Case

Teacher-led education remains possible.

Library-only subjects may be blocked.

---

# 111. 154C Phase T — No Facilities Edge Case

Use degraded/limited learning where subject permits.

Do not soft-lock all childhood fundamentals unless deliberately designed.

---

# 112. 154C Phase U — All Subjects Stress Case

A student may not max every subject trivially.

Use:

- time budget,
- specialization limits,
- teacher capacity,
- stage duration.

---

# 113. 154C Phase V — Teacher Death Edge Case

If teacher dies:

- assignment clears,
- education record remains,
- another teacher/library can continue,
- unrecorded specialized knowledge may be lost if no repository exists.

---

# 114. 154C Phase W — Library Destruction Edge Case

Loss affects access/bonuses.

Existing student proficiency remains.

---

# 115. 154C Phase X — Graduation Idempotency

Graduation rewards apply once.

No duplicate:

- skill unlock,
- trait,
- morale event,
- journal entry.

---

# 116. 154C Phase Y — Skill Unlock Integrity

Every education-linked skill:

- exists,
- can be awarded through skill system,
- has valid tier/XP semantics.

---

# 117. 154C Phase Z — `--education-selftest`

Required scenarios:

1. child enters education,
2. teacher-led progress,
3. parent/guardian bonus,
4. library self-study,
5. no-teacher stall/fallback,
6. adolescence specialization,
7. apprenticeship,
8. graduation,
9. skill unlock,
10. old save,
11. save/load mid-study,
12. teacher death,
13. library loss.

---

# 118. 154C Phase AA — Data Integrity

Validate:

- 10 curricula,
- prerequisites,
- stage ranges,
- skill refs,
- book/research refs,
- facility refs,
- localization.

---

# 119. 154C Phase AB — Deliberate Failure Proof

Break:

- circular prerequisite,
- missing skill ID,
- duplicate graduation reward,
- invalid stage.

Assert gate fails.

---

# 120. 154C Phase AC — 200-Day Education Soak

Record:

```text
students
teacher-days
subject progress
library study
apprenticeships
graduates
uneducated maturations
skills earned
```

---

# 121. 154C Phase AD — Multi-Generation Soak

Run multiple cohorts.

Measure:

- inherited/learned specialization continuity,
- teacher replacement,
- library knowledge continuity,
- skill distribution by generation.

---

# 122. 154C Phase AE — Knowledge Loss Scenario

Destroy/remove library and key teacher.

Observe:

- current skills remain,
- future education capacity falls,
- recovery possible through surviving knowledge.

---

# 123. 154C Phase AF — Education Strategy Profiles

Run:

```text
formal_school
parent_led
library_heavy
apprenticeship_heavy
minimal_education
```

Compare:

- skill outcomes,
- labor cost,
- research/work output,
- survivor readiness.

No path should dominate every axis.

---

# 124. 154C Phase AG — Labor Opportunity Cost

Education must cost:

- teacher time,
- facility time,
- student time.

Ensure high education has a meaningful short-term tradeoff.

---

# 125. 154C Phase AH — Optionality Balance

Uneducated survivors:

- remain viable,
- may require later training,
- should not be permanently useless.

---

# 126. 154C Phase AI — UI Runtime Parity

Displayed:

- progress,
- learning rate,
- blockers,
- teacher quality,
- graduation readiness

must match runtime calculations.

---

# 127. 154C Phase AJ — Accessibility

Education panel:

- keyboard navigation,
- controller if supported,
- text labels,
- non-color proficiency indicators,
- scalable layout,
- localized.

---

# 128. 154C Phase AK — Retention

Use Plan 55:

- recent education events,
- landmark graduations,
- teacher legacy,
- knowledge-loss events.

Do not retain every daily study tick.

---

# 129. 154C Phase AL — Epilogue / Legacy

Potential completion inputs:

- first graduate,
- famous teacher,
- preserved library,
- lost archive,
- multi-generation specialty.

Use canonical completion record.

---

# 130. 154C Phase AM — Human Playtest

Assess:

```text
Does education feel like investment instead of chores?
Are teacher costs legible?
Do subject choices matter?
Does a bunker-raised adult feel meaningfully shaped?
```

This is human review, not CI.

---

# 131. 154C Phase AN — Documentation

Create:

```text
docs/systems/EDUCATION_AND_KNOWLEDGE.md
```

Include:

- authority boundaries,
- stage model,
- curriculum schema,
- teacher/parent/library routes,
- graduation,
- save behavior,
- knowledge continuity/loss,
- adding new subjects.

---

# 132. 154C Definition of Done

- [ ] Cohort stage integration,
- [ ] maturation coordination,
- [ ] uneducated maturation,
- [ ] skill progression integration,
- [ ] apprenticeship integration,
- [ ] library study integration,
- [ ] lineage integration,
- [ ] parent/teacher relation integration,
- [ ] family integration,
- [ ] duty/work integration,
- [ ] health/needs integration,
- [ ] save/load matrix,
- [ ] save-scum prevention,
- [ ] old-save child/adult handling,
- [ ] no-teacher/no-library/no-facility edge cases,
- [ ] all-subject stress case,
- [ ] teacher death,
- [ ] library destruction,
- [ ] graduation idempotency,
- [ ] selftest,
- [ ] failure proof,
- [ ] 200-day soak,
- [ ] multi-generation soak,
- [ ] knowledge-loss scenario,
- [ ] strategy profiles,
- [ ] labor-cost balance,
- [ ] optionality,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] retention,
- [ ] epilogue/legacy,
- [ ] playtest,
- [ ] docs.

---

# 133. Integrated Education Pipeline

```text
CohortSystem age
    │
    ▼
Education stage
    │
    ├──────────────► teacher-led
    ├──────────────► parent/guardian-led
    ├──────────────► library self-study
    └──────────────► apprenticeship
                     │
                     ▼
              subject proficiency
                     │
                     ▼
              graduation readiness
                     │
      ┌──────────────┼──────────────┐
      ▼              ▼              ▼
SkillProgression   Lineage       Relations
      │              │              │
      └──────────────┼──────────────┘
                     ▼
             mature educated survivor
```

---

# 134. Age Authority Contract

Age/life-stage derives from CohortSystem.

Education never owns a second age counter.

---

# 135. Education Progress Contract

Education owns only:

- enrollment,
- proficiency,
- educational milestones,
- graduation status.

---

# 136. Skill Contract

Education may unlock/award through SkillProgressionSystem.

It never writes raw duplicated skill levels.

---

# 137. Apprenticeship Contract

Apprenticeship remains practical transfer authority.

Education orchestrates entry/requirements.

---

# 138. Library Contract

LibraryStudySystem remains study authority.

Education interprets study toward curriculum progress.

---

# 139. Lineage Contract

Inherited traits remain owned by GenerationalLineageExtension.

Education can add learned-history provenance, not overwrite lineage semantics.

---

# 140. Teacher Contract

Teacher quality is derived from real skill, availability, capacity, and context.

---

# 141. Parent/Guardian Contract

Family relation provides:

```text
eligibility + contextual bonus
```

not guaranteed automatic progress.

---

# 142. Facility Contract

Facilities must exist in canonical shelter state.

No education-only fake building flags.

---

# 143. Knowledge Repository Contract

Knowledge should preferably be represented by:

- books,
- curricula,
- research unlocks,
- recorded techniques.

Avoid a single opaque shelter-knowledge number unless explicitly justified.

---

# 144. Graduation Contract

Graduation is an education milestone.

Adult maturation remains CohortSystem authority.

---

# 145. Optionality Contract

Formal education improves capability but is not required for a valid adult survivor.

---

# 146. Save Contract

Persist:

- subject proficiency,
- enrollment,
- teacher assignments,
- graduation milestones,
- event/cooldown state.

Do not persist:

- age copy,
- skill copy,
- lineage copy,
- library-state copy,
- derived quality.

---

# 147. Old-Save Contract

Old children get present-day records, not fabricated past education.

---

# 148. Determinism Contract

Core learning progression is deterministic.

Event variation uses seeded RNG and persisted outcomes.

---

# 149. Retention Contract

Preserve:

- graduation,
- famous teachers,
- lineage-defining education,
- library-loss landmarks.

Roll up daily study detail.

---

# 150. UI Contract

UI displays the same learning-rate/progress model used by runtime.

---

# 151. Content Acceptance Contract

Curricula progress through:

```text
AUTHORED
→ LOADS
→ ELIGIBLE
→ STUDIED
→ PROFICIENCY_PRODUCED
→ SKILL/OUTCOME_PRODUCED
→ PLAYER_VISIBLE
```

---

# 152. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| duplicate age/maturation state | Medium | High | Cohort-derived stages |
| duplicate skill XP | Medium | High | SkillProgression owns skills |
| “education quality” becomes opaque stat | Medium | Medium | derive from inputs |
| subject system becomes too broad | High | Medium | 10-subject cap |
| teacher role dominates labor economy | Medium | High | opportunity-cost balance |
| children become useless without schooling | Medium | High | optionality guard |
| library destruction erases learned skills | Medium | High | knowledge vs acquired skill separation |
| parent teaching auto-copies traits | Medium | High | lineage authority only |
| apprenticeship double-counts study | Medium | High | explicit integration formula |
| save/load rerolls graduation outcomes | Medium | High | persisted event IDs/outcomes |
| curricula become unreachable | Medium | Medium | utilization report |
| UI overwhelms player | Medium | Medium | staged/summary presentation |

---

# 153. Commit Strategy

## 154A — Foundation

### C2[32].1 — baseline + education ADR

### C2[32].2 — stage/record/curriculum DTOs

### C2[32].3 — curriculum authority + subject reconciliation

### C2[32].4 — teacher/parent/guardian model

### C2[32].5 — library/facility integration

### C2[32].6 — deterministic daily progress

### C2[32].7 — save/old-save/graduation contract

### C2[32].8 — ports/events/diagnostics

### Gate: 154A complete

---

## 154B — Content / Progression

### C2[32].9 — childhood curricula

### C2[32].10 — adolescence specialization

### C2[32].11 — young-adult/apprenticeship stage

### C2[32].12 — teacher workload/XP/parent bonus

### C2[32].13 — library/knowledge persistence

### C2[32].14 — graduation/skill/traits

### C2[32].15 — education events/quests

### C2[32].16 — UI/journal/tutorial/localization

### C2[32].17 — utilization report

### Gate: 154B complete

---

## 154C — Closure

### C2[32].18 — Cohort/maturation integration

### C2[32].19 — Skill/Apprenticeship/Library integration

### C2[32].20 — Lineage/Relations/Family/Duty integration

### C2[32].21 — save-load/exploit matrix

### C2[32].22 — edge-case suite

### C2[32].23 — selftest + deliberate failure proof

### C2[32].24 — 200-day education soak

### C2[32].25 — multi-generation/knowledge-loss soak

### C2[32].26 — strategy/labor/optionality balance

### C2[32].27 — UI/accessibility/playtest/docs

### Gate: 154C complete

---

# 154. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --education-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
education curriculum content-utilization report
old-save fixture load
same-state education replay
200-day education soak
multi-generation knowledge continuity test
education-panel snapshot/accessibility gate
```

---

# 155. Flagship Definition of Done

## 154A — Foundation

- [ ] EducationSystem,
- [ ] stage derivation from CohortSystem,
- [ ] no duplicate age state,
- [ ] EducationRecord,
- [ ] curriculum data authority,
- [ ] 8-vs-10 subject ambiguity resolved explicitly,
- [ ] teacher eligibility/capacity,
- [ ] teacher work cost,
- [ ] parent/guardian teaching,
- [ ] library self-study,
- [ ] facility integration,
- [ ] deterministic daily progression,
- [ ] save/old-save,
- [ ] graduation readiness,
- [ ] skill/lineage/relation ports,
- [ ] semantic events,
- [ ] diagnostics.

## 154B — Content

- [ ] childhood stage,
- [ ] adolescence stage,
- [ ] young-adult stage,
- [ ] 10 curriculum subjects,
- [ ] specialization limits,
- [ ] teacher matching,
- [ ] parent bonus,
- [ ] schoolroom/library/workshop support,
- [ ] apprenticeship,
- [ ] graduation project,
- [ ] skill unlock routing,
- [ ] knowledge persistence/loss/recovery,
- [ ] education events,
- [ ] quest hooks,
- [ ] UI/tooltips/journal/tutorial,
- [ ] localization,
- [ ] utilization.

## 154C — Integration

- [ ] CohortSystem integration,
- [ ] maturation coordination,
- [ ] uneducated maturation valid,
- [ ] SkillProgression integration,
- [ ] Apprenticeship integration,
- [ ] LibraryStudy integration,
- [ ] GenerationalLineage integration,
- [ ] Relations integration,
- [ ] Family/guardian integration,
- [ ] Duty/work integration,
- [ ] health/needs integration,
- [ ] save/load matrix,
- [ ] save-scum prevention,
- [ ] old-save child/adult handling,
- [ ] no-teacher/no-library/no-facility edge cases,
- [ ] all-subject stress case,
- [ ] teacher death,
- [ ] library destruction,
- [ ] graduation idempotency,
- [ ] selftest,
- [ ] failure proof,
- [ ] 200-day soak,
- [ ] multi-generation soak,
- [ ] knowledge-loss test,
- [ ] strategy profiles,
- [ ] labor opportunity-cost balance,
- [ ] optionality,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] retention,
- [ ] epilogue/legacy,
- [ ] playtest,
- [ ] docs.

## Global

- [ ] no duplicate age authority,
- [ ] no duplicate skill authority,
- [ ] no duplicate lineage authority,
- [ ] no fake global knowledge meter without consumer,
- [ ] no instant education exploit,
- [ ] no mandatory education meta,
- [ ] full verification green.

---

# 156. Closure Report Template

```markdown
## C2[32] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Cohort age/maturation fields:
- Current child stages:
- Skill system:
- Apprenticeship system:
- Library system:
- Lineage system:
- Existing education refs:

### 154A — Foundation
- Education stages:
- Curricula:
- Subject-count resolution:
- Teacher model:
- Parent/guardian model:
- Library/facilities:
- Save schema:
- Old save:
- Missing ports:
- Result:

### 154B — Content
- Childhood:
- Adolescence:
- Young Adult:
- Subjects:
- Specializations:
- Teachers:
- Parent bonus:
- Library:
- Apprenticeship:
- Graduation:
- Knowledge persistence:
- Knowledge loss:
- Events:
- Quests:
- UI:
- Unused curricula:
- Result:

### 154C — Integration
- Cohort:
- Maturation:
- Skill progression:
- Apprenticeship:
- Library:
- Lineage:
- Relations:
- Family/guardians:
- Duty/work:
- Health/needs:
- Old save:
- Save-load rerolls:
- Graduation duplicates:
- 200-day soak:
- Multi-generation soak:
- Knowledge-loss scenario:
- Strategy balance:
- Playtest:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Education selftest:
- Port contract:
- Content utilization:
- Old-save fixtures:
- Same-state replay:
- 200-day soak:
- Multi-generation soak:
- Verify fast:

### Final Metrics
- EDUCATION_SUBJECTS:
- ACTIVE_STUDENTS:
- ACTIVE_TEACHERS:
- SUBJECT_ENROLLMENTS:
- LIBRARY_STUDY_SESSIONS:
- APPRENTICESHIPS:
- GRADUATES:
- UNEDUCATED_MATURATIONS:
- SKILLS_UNLOCKED:
- DEAD_CURRICULA:
- GRADUATION_DUPLICATES:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Curriculum:
- Facilities:
- Library:
- Teacher economy:
- Family education:
- UI:
```

---

# 157. Final Execution Directive

Execute Plan 154 as a **structured learning and generational-knowledge layer over the existing age, skills, apprenticeship, library, lineage, relationship, and duty authorities**.

The critical sequence is:

```text
derive education stage from CohortSystem
→ define a bounded curriculum
→ assign real teachers/parents/guardians
→ make learning consume real time and labor
→ route self-study through LibraryStudySystem
→ route practical learning through ApprenticeshipSystem
→ route graduation outcomes through SkillProgressionSystem
→ route inherited traits through GenerationalLineageExtension
→ preserve save/load determinism
→ prove knowledge continuity and loss across generations
```

Do not create a second age counter.

Do not create a second skill XP system.

Do not copy parent traits directly inside EducationSystem.

Do not invent a vague global “knowledge” score unless it has concrete ownership and consumers.

Do not make formal schooling mandatory for maturation.

The strongest authority rule is:

> **Education owns how a survivor learns; the systems that already own age, skill, library knowledge, apprenticeship, lineage, and relationships continue to own the resulting facts.**

The strongest generational rule is:

> **Knowledge survives because people teach, books persist, curricula exist, and apprenticeships happen—not because the shelter has an unexplained permanent bonus.**

The strongest player-value rule is:

> **Education should create a long-term tradeoff between present labor and future capability, while leaving uneducated survivors viable and making teachers, parents, libraries, and apprenticeships meaningfully different learning paths.**

The flagship acceptance scenario is:

> **Take one child through a seeded multi-year development path with a skilled parent, a library, and later an expert apprenticeship. Save/load during adolescence, remove the original teacher, destroy the library in a controlled test, replace teaching through a surviving mentor, and graduate the survivor through the canonical skill system. The resulting adult must retain earned skills and education history, the lineage system must own inherited traits, relationship history must show meaningful teaching milestones, knowledge loss must reduce future learning capacity without erasing already learned skills, and the same seed/state/actions must reproduce the same progression in headless CI.**
