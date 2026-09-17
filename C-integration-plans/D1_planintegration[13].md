# D1 Flagship Integration Plan [13]
## Plan 183 — Child Development Stages System

> **Purpose:** Replace ASHFALL's binary child maturation flag with one authoritative developmental-stage pipeline
> that tracks children from infancy through young adulthood, gates capabilities by age/stage, integrates care,
> education, needs, skills, relationships, and aging, and hands a fully formed young adult into the canonical
> survivor lifecycle without duplicating state or creating exploitable parallel progression.
>
> **Primary source:** Plan 183 — Child Development Stages System.
>
> **Core repository problem:** `CohortSystem.cs` currently treats maturation as a boolean transition. Children
> are effectively "not mature" until they suddenly become adults. There is no infant/toddler/child/adolescent/
> young-adult progression, no stage-specific capability contract, no development-event authority, and no
> deterministic child-to-adult handoff.
>
> **Implementation posture:** deterministic, age-authoritative, save-safe, data-driven, compatible with old
> saves, integrated through existing cohort, survivor lifecycle, needs, education, skills, aging, relations,
> family, shelter-scheduling, and journal authorities.
>
> **Critical guardrail:** this is a game-development model, not a clinical child-development simulator.
> Developmental stages are coarse gameplay abstractions. The system must avoid pseudo-medical diagnosis,
> punitive "developmental failure" loops, or incentives that make children feel like disposable labor units.

---

## 1. Source Problem Statement

The source plan identifies a clear architectural defect:

- `CohortSystem.cs` tracks children but maturation is binary.
- A child can move from "immature" to "adult" without any intermediate gameplay state.
- Plan 154 handles education but not developmental stages.
- Plan 176 handles aging but not child-specific progression.
- Plan 150 can model family relations but not developmental capability.
- Plan 140 can model generational inheritance but not childhood.

The missing domain is therefore:

```text
Child identity + birth/age fact
           ↓
ChildDevelopmentSystem
           ↓
authoritative stage derivation
  ├─ infant
  ├─ toddler
  ├─ child
  ├─ adolescent
  └─ young_adult
           ↓
stage capability / need profile
           ↓
typed integrations
  ├─ CohortSystem
  ├─ NeedsSystem
  ├─ EducationSystem
  ├─ SkillProgressionSystem
  ├─ SurvivorRelationsSystem
  ├─ Family/Caregiver
  ├─ AgingSystem
  ├─ Scheduler/Autonomy
  └─ SurvivorLifecycle
           ↓
adult transition
```

The important architectural change is that "matured" becomes a derived compatibility projection of stage/age,
not the primary truth.

---

## 2. Flagship Success Criteria

The work is complete only when all of the following are true:

1. `ChildDevelopmentSystem.cs` exists with schema-versioned capture/restore.
2. Child age has one canonical authority.
3. Development stage derives deterministically from age plus explicit migration state.
4. `CohortSystem.TryMaturation()` is replaced, deprecated, or adapted so it no longer performs an independent
   boolean maturation transition.
5. Stage definitions live in `development_stages.json`.
6. Stage ranges are non-overlapping and gap-free for the configured childhood period.
7. Stage capabilities are explicit and data-backed.
8. Stage-specific care/need profiles use canonical needs/care APIs.
9. Education hooks use Plan 154 or the repository's real education authority.
10. Skill progression uses `SkillProgressionSystem`; ChildDevelopmentSystem does not store a second skill tree.
11. Caregiver assignments use stable survivor IDs and canonical availability.
12. Parent relationships remain owned by family/relations systems.
13. Developmental milestone events emit exactly once.
14. Save/load does not duplicate milestone effects or stage transitions.
15. Old saves receive a deterministic estimated age/stage without corrupting existing child state.
16. Adult transition executes exactly once.
17. Adult transition preserves accumulated canonical skills/traits/relationships.
18. The same child cannot remain in both child and adult rosters after transition.
19. Young-adult handoff is transactional and idempotent.
20. Critical survival needs always outrank education, chores, or leisure.
21. Children cannot be assigned to capabilities their stage forbids.
22. Adolescents gain broader responsibilities gradually rather than becoming an unrestricted labor exploit.
23. Development pace cannot be rerolled or accelerated through UI/save-load spam.
24. The system works headlessly.
25. `--child-development-selftest` proves stage progression, care integration, education, migration, milestone
    idempotency, and adult handoff.

---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/child_development/CHILD_DEVELOPMENT_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/CohortSystem.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs`
- canonical survivor aggregate/state
- `NeedsSystem.cs`
- `SkillProgressionSystem.cs`
- education system from Plan 154 if present
- aging system from Plan 176 if present
- romance/family/parent-child relation system from Plan 150
- survivor relations
- scheduler/autonomy/work-assignment authority
- health/disease/injury systems
- nutrition systems
- shelter occupancy/bed/room authority
- memorial/death/fate handling
- campaign clock/calendar
- seeded RNG
- save schema/migrations
- journal/event bus
- quest hooks
- localization
- UI survivor/child panels
- achievement/epilogue systems
- legacy/generational inheritance systems

For each source of child age or maturation, record:

| Source | Field/API | Authority? | Persisted? | Used by | Must deprecate/adapt? |
|---|---|---:|---:|---|---|
| CohortSystem | isMatured | no after Plan 183 | yes/no | ... | yes |
| child birth day | ... | candidate | yes/no | ... | maybe |
| AgingSystem | ... | candidate | yes/no | ... | integrate |

No implementation should proceed until competing age/maturation sources are identified.

---

## 4. Scope Boundary

### In scope

- stage definitions;
- canonical child age/stage state;
- capability gating;
- care requirement profiles;
- caregiver assignment;
- development milestones;
- education/learning multipliers;
- nutrition/care influence;
- social-development hooks;
- personality-emergence hooks;
- transition to young adult/adult;
- migration from binary maturation;
- UI;
- journal/quest/achievement hooks;
- CI/selftests.

### Explicitly out of scope for first pass

- clinical developmental diagnosis;
- detailed pediatric medicine;
- puberty simulation;
- sexual content involving minors;
- full parenting minigame;
- independent child labor economy;
- cross-campaign family legacy storage;
- procedurally generated school curricula;
- bespoke quest chains for every child;
- realistic month-by-month infancy.

The system should create meaningful generational pacing without turning childhood into micromanagement.

---

## 5. Canonical Child Development State

Recommended:

```csharp
public sealed record ChildDevelopmentState
{
    public int SchemaVersion { get; init; }
    public IReadOnlyDictionary<string, ChildDevelopmentRecord> Children { get; init; }
    public IReadOnlySet<string> EmittedMilestoneIds { get; init; }
    public IReadOnlyList<StageTransitionRecord> StageTransitions { get; init; }
}
```

Child-specific state should be keyed by stable child/survivor ID.

Do not duplicate global survivor skills, traits, morale, or health inside this state.

---

## 6. Child Development Record

Recommended:

```csharp
public sealed record ChildDevelopmentRecord
{
    public string ChildId { get; init; }
    public int BirthDay { get; init; }
    public int AgeDays { get; init; }
    public string CurrentStageId { get; init; }

    public int StageProgressBasisPoints { get; init; }
    public string? PrimaryCaregiverId { get; init; }
    public IReadOnlyList<string> SecondaryCaregiverIds { get; init; }

    public IReadOnlySet<string> AchievedMilestoneIds { get; init; }
    public IReadOnlySet<string> EmergedTraitFactIds { get; init; }

    public bool AdultTransitionCompleted { get; init; }
    public string? MigrationProvenance { get; init; }
}
```

Age can be derived from `currentDay - birthDay` if birth day is authoritative. If persisted for compatibility,
selftest should verify it matches.

---

## 7. Stage Definition Contract

Create:

`Assets/StreamingAssets/Data/development_stages.json`

Recommended DTO:

```csharp
public sealed record DevelopmentStageDefinition
{
    public string StageId { get; init; }
    public string TitleKey { get; init; }
    public int MinAgeDaysInclusive { get; init; }
    public int MaxAgeDaysExclusive { get; init; }

    public IReadOnlyList<string> CapabilityIds { get; init; }
    public IReadOnlyList<string> CareNeedProfileIds { get; init; }

    public int LearningRateBasisPoints { get; init; }
    public string SocialProfileId { get; init; }
    public IReadOnlyList<string> MilestoneProfileIds { get; init; }
}
```

Avoid free-form behavior descriptions as the only gameplay authority.

---

## 8. Stage Range Policy

The source uses:

- infant: 0–1 year;
- toddler: 1–3;
- child: 3–12;
- adolescent: 12–16;
- young adult: 16–18;
- adult transition at 18.

Before hardcoding real-world-year lengths:
- inspect campaign time compression;
- determine whether one campaign day equals one in-world day;
- assess whether a normal campaign can even span 18 years.

If campaigns are much shorter, either:
1. generational modes intentionally accelerate child aging; or
2. child-development content is mostly for extended/legacy modes.

Document the time-scale contract explicitly.

---

## 9. Campaign Time Compression

This is the largest design risk.

If a normal ASHFALL campaign lasts ~120–365 days, literal 18-year maturation is unreachable.

Create:

`docs/child_development/CHILD_TIME_SCALE.md`

It must answer:
- real campaign day → in-world age-day ratio;
- whether children age faster for gameplay;
- whether generational campaigns use a separate scale;
- whether birth/child systems are intended only for multi-year modes.

Do not ship unreachable stages.

---

## 10. Age Authority

Preferred:

```text
AgeDays = ChildAgeClock.GetAgeDays(childId, currentCampaignDay)
```

The age clock may derive from:
- birth day;
- migration baseline age;
- configured time scale.

ChildDevelopmentSystem consumes age.

AgingSystem and CohortSystem must read the same authority.

---

## 11. Single Clock Invariant

Forbidden:

```text
CohortSystem increments childAge
ChildDevelopmentSystem increments childAge
AgingSystem increments childAge
```

Correct:

```text
Campaign clock
 -> Age authority
 -> child stage
 -> adult aging after handoff
```

One source of temporal truth.

---

## 12. Boolean Maturation Compatibility Adapter

During migration, keep:

```csharp
bool IsMatured(childId)
    => stage == adult || AdultTransitionCompleted;
```

or equivalent.

Existing consumers can temporarily call this.

But `TryMaturation()` must stop independently deciding maturation.

---

## 13. Stage Transition State Machine

```text
infant
 -> toddler
 -> child
 -> adolescent
 -> young_adult
 -> adult_transition
```

Transitions occur only forward.

No regression due to:
- save migration;
- clock rewinds;
- data reload.

If debug time travel exists, development state should remain stable unless explicitly reset.

---

## 14. Stage Transition Transaction

Flow:

```text
age threshold reached
 -> validate next stage
 -> persist transition
 -> update capability profile
 -> emit StageChanged
 -> emit stage-entry milestones
 -> notify downstream systems
```

For final transition:

```text
young_adult threshold
 -> prepare adult survivor handoff
 -> validate survivor lifecycle
 -> commit handoff
 -> mark AdultTransitionCompleted
 -> remove/deactivate child-only authority
 -> emit AdultTransitioned
```

No half-transition.

---

## 15. Development Progress

The source asks for `developmentProgress 0–100 within stage`.

Prefer it as a derived UI value:

```text
(ageDays - stageMin) / stageDuration
```

Do not make care/education arbitrarily shift chronological stage thresholds unless design explicitly supports
developmental delay/acceleration.

This separates age from developmental quality.

---

## 16. Development Quality vs Chronological Stage

Use two dimensions:

### Chronological stage
Age-derived and deterministic.

### Development support quality
Reflects care, nutrition, education, social support.

Poor support may affect:
- learning rate;
- milestone timing within a stage;
- morale/health hooks.

It should not casually make a 14-year-old mechanically remain a toddler.

---

## 17. "Developmental Delay" Guardrail

The source suggests poor care can cause developmental delay.

Implement carefully.

Prefer:
- slower milestone achievement;
- reduced learning;
- stress/health consequences;
- support-needed flags.

Avoid pseudo-clinical labels or permanent stigmatizing debuffs unless a dedicated health system explicitly owns
such conditions.

This is a game abstraction.

---

## 18. Infant Stage

Gameplay profile:

- no work assignments;
- no independent movement tasks;
- very high care requirement;
- feeding/comfort/safety;
- sleep needs;
- caregiver availability.

Milestones:
- first smile;
- first laugh;
- bonding events.

Do not represent diapering as a repetitive click loop. Model as abstract care load/capacity.

---

## 19. Infant Care Load

Represent care as scheduled capacity.

Example:

```csharp
CareRequirement
{
    supervisionHoursPerDay,
    feedingSupport,
    comfortSupport,
    sleepSupport
}
```

Scheduler allocates caregiver time.

Player should assign caregiver/policy, not issue hourly care commands.

---

## 20. Infant Care Quality

Possible inputs:
- caregiver availability;
- relationship/parent bond;
- caregiver exhaustion;
- nutrition availability;
- shelter safety.

Output:
- support-quality band.

Use bounded band:
- insufficient;
- adequate;
- strong.

Avoid exact "parenting score."

---

## 21. Toddler Stage

Gameplay profile:

- supervised movement;
- basic communication;
- high safety requirement;
- play/stimulation;
- no production work;
- simple participation in family/social events.

Milestones:
- first steps;
- first words;
- running/basic communication.

These should be threshold/event driven, not random rerolls.

---

## 22. Toddler Safety

Do not create per-frame accident RNG.

Use environment/care abstractions:
- unsafe shelter event increases risk;
- absent supervision during hazard can trigger one event check;
- normal supervised state is safe.

Keep deterministic under fixed seed.

---

## 23. Child Stage

Gameplay profile:

- formal/basic education;
- play/social development;
- simple age-appropriate chores;
- limited task capacity;
- hobby exposure;
- emerging interests/personality.

No heavy combat, dangerous industrial work, or unrestricted expedition duty.

---

## 24. Child Chore Contract

Define capability IDs:

```text
chore_tidy
chore_carry_light
chore_sort_supplies
chore_garden_assist
chore_kitchen_assist
```

Only map to real work systems.

Chores should:
- contribute small value;
- train responsibility/skills modestly;
- never substitute for adult labor.

Avoid "child worker efficiency" optimization.

---

## 25. Education Integration

Plan 154 or canonical education authority owns:

- lessons;
- curricula;
- teachers;
- skill XP;
- educational facilities.

ChildDevelopmentSystem provides:

```text
stage
learningRateMultiplier
education eligibility
attention/support modifiers
```

No duplicate education progress.

---

## 26. Learning Rate

The source says children learn faster than adults.

Do not use one huge universal multiplier.

Use stage-specific bounded multipliers.

Example:
- toddler: language/basic-development only;
- child: high general learning;
- adolescent: specialization;
- young adult: near-adult.

Final values require balance tests.

---

## 27. Learning Domains

Stage capability should limit which skills are learnable.

Example:

### Child
- literacy;
- basic survival;
- gardening;
- simple crafting;
- social;
- foundational academics.

### Adolescent
- advanced technical;
- leadership;
- combat training only if design allows;
- specialized production.

No arbitrary adult skill access merely because learning rate is high.

---

## 28. Nutrition Integration

Needs/Nutrition system remains authority.

ChildDevelopmentSystem can consume:
- adequately fed;
- malnourished risk;
- severe shortage.

Effects:
- learning modifier;
- care quality;
- health consequence hooks.

Do not create duplicate hunger.

---

## 29. Health Integration

Child-specific vulnerability may be represented through health-system modifiers.

Examples:
- infant disease vulnerability;
- injury sensitivity;
- treatment needs.

Do not store a second `health` number in ChildState if health system already owns it.

The source DTO's `health` field should become a reference/profile/modifier, not duplicate health truth.

---

## 30. Needs Profiles

Stage-specific needs should be declarative.

Example:

```json
{
  "stageId": "infant",
  "needProfileId": "child_needs_infant"
}
```

NeedsSystem resolves:
- nutrition;
- sleep;
- comfort;
- social;
- safety.

Avoid hardcoding need math in ChildDevelopmentSystem.

---

## 31. Caregiver Assignment

Recommended state:

```csharp
public sealed record CaregiverAssignment
{
    public string ChildId { get; init; }
    public string PrimaryCaregiverId { get; init; }
    public IReadOnlyList<string> BackupCaregiverIds { get; init; }
}
```

Assignment is child-development state only if no general duty authority already owns it.

Prefer scheduler assignment if available.

---

## 32. Caregiver Eligibility

Caregiver must be:

- alive;
- present;
- capable;
- not incapacitated;
- allowed by scheduler;
- not overloaded beyond configured care capacity.

Parent may be preferred but not required.

---

## 33. Multiple Children per Caregiver

Care load should be capacity-based.

Example:
- infant consumes high care capacity;
- toddler moderate;
- child low;
- adolescent minimal.

Do not use one hard "maximum 3 children" unless balanced from time simulation.

---

## 34. Caregiver Overload

If assigned load exceeds capacity:

- effective care quality decreases;
- scheduler warning;
- player can assign backup;
- no instant catastrophic penalty.

This avoids all-or-nothing micromanagement.

---

## 35. Parent vs Caregiver

Parent relationship is canonical family truth.

Caregiver is a current role.

A non-parent can be caregiver.

Do not rewrite family relationships when care assignment changes.

---

## 36. Relationship Integration

Care can emit:

- bonding;
- trust;
- resentment if neglectful;
- mentor attachment.

Relations system applies actual changes.

ChildDevelopmentSystem only emits context.

---

## 37. Parent-Child Relationship Effects

If Plan 150 has parent-child bonds:

- strong bond can improve comfort/trust;
- conflict can influence adolescent autonomy;
- separation can produce morale hooks.

Do not let relationship score directly change age/stage.

---

## 38. Social Development

Use structured social-development facts:

- regular peer play;
- caregiver attachment;
- education group participation;
- isolation;
- conflict.

These may contribute to emerging personality through trait authority.

Do not create a second "social development score" unless clearly necessary.

---

## 39. Personality Emergence

The source says personality traits emerge in adolescence.

Recommended:

- early childhood accumulates behavioral evidence;
- adolescence evaluates a small set of candidate traits;
- trait system owns final trait assignment.

Inputs:
- family relations;
- social events;
- education;
- hardship;
- hobby interests;
- backstory/family inheritance if applicable.

Use deterministic rules.

---

## 40. Trait Emergence Idempotency

Stable event:

```text
child_trait_emerged:<childId>:<traitId>
```

Once emitted:
- save persists;
- no reroll after reload.

If trait system rejects duplicate, even better.

---

## 41. Adolescent Stage

Gameplay profile:

- increased autonomy;
- broader work capability;
- education specialization;
- identity formation;
- greater responsibility;
- still stage-gated from some dangerous/leadership roles depending design.

The source says "full work capability." Refine this to "broad work capability" unless the game's ethical/design
framework explicitly treats adolescents as full adult workers.

---

## 42. Adolescent Work Policy

Create capability categories:

- safe production;
- agriculture;
- maintenance assistant;
- education/apprenticeship;
- logistics;
- leadership trainee.

High-risk:
- frontline combat;
- hazardous expeditions;
- radiation cleanup;
- dangerous machinery;

should require explicit age/policy checks or remain adult-only.

This prevents min-max exploitation.

---

## 43. Adolescent Autonomy

If Plan 144 exists:

- adolescent has growing autonomy weight;
- can refuse low-priority assignments under conflict conditions;
- preferences begin to matter;
- hobby/education specialization influences choices.

Do not implement random disobedience as a coin flip.

---

## 44. "Rebellion" Event Reframing

The source proposes adolescent rebellion.

Treat as a contextual autonomy/identity event, not a stereotype.

Triggers may include:
- high control + low trust;
- blocked preferred path;
- family conflict;
- high stress;
- social influence.

Possible outcomes:
- argument;
- skipped duty;
- request for independence;
- identity choice.

Deterministic from facts/seed.

---

## 45. Rebellion Consequences

Use relations/autonomy/moral systems.

Avoid:
- permanent "rebellious" debuff;
- random severe punishment.

Player response may affect:
- trust;
- autonomy;
- future role preference.

This should generate character, not annoyance.

---

## 46. Young Adult Stage

Gameplay profile:

- near-adult capabilities;
- career/specialization choice;
- coming-of-age event;
- adult survivor transition preparation.

At threshold:
- handoff to canonical survivor lifecycle.

---

## 47. Coming-of-Age Ceremony

The source proposes a ceremony.

If Plan 170 seasonal/celebration system exists:
- create an optional coming-of-age celebration hook.

If not:
- simple milestone/journal event.

Adult transition must not depend on holding the ceremony.

---

## 48. Adult Transition Contract

Recommended command:

```csharp
public sealed record PromoteChildToAdultRequest
{
    public string ChildId { get; init; }
    public string TransitionId { get; init; }
    public int Day { get; init; }
}
```

SurvivorLifecycle validates and commits.

ChildDevelopmentSystem then marks transition complete.

---

## 49. Adult Transition Invariants

After success:

- stable survivor ID remains same if architecture supports;
- skills remain;
- traits remain;
- relationships remain;
- parent links remain;
- inventory/possessions remain;
- education history remains;
- child-only caregiver requirement removed;
- adult needs profile becomes active;
- no duplicate survivor created.

Prefer one aggregate changing lifecycle status over creating a second identity.

---

## 50. Adult Transition Idempotency

Stable transition ID:

```text
adult_transition:<childId>
```

If repeated:
- no duplicate adult;
- no duplicate event;
- no duplicate inventory/skills.

Persist `AdultTransitionCompleted`.

---

## 51. CohortSystem Refactor

Target end state:

`CohortSystem` owns:
- cohort membership/birth tracking if still appropriate.

`ChildDevelopmentSystem` owns:
- child stage/development record.

`SurvivorLifecycle` owns:
- lifecycle state.

`AgingSystem` owns:
- general age progression after adult transition, or shares one age authority.

Remove duplicated maturation mutation.

---

## 52. `TryMaturation()` Migration

Phase 1:
- wrap old API;
- return true only when ChildDevelopmentSystem says transition eligible.

Phase 2:
- route callers to new transition API.

Phase 3:
- remove/deprecate boolean mutation.

Add source search regression for direct `isMatured = true`.

---

## 53. Milestone Event Contract

Recommended:

```csharp
public sealed record DevelopmentMilestoneEvent
{
    public string MilestoneInstanceId { get; init; }
    public string ChildId { get; init; }
    public string MilestoneDefinitionId { get; init; }
    public int Day { get; init; }
}
```

Effects handled downstream.

---

## 54. Milestone Categories

Source milestones:

- first smile;
- first laugh;
- first steps;
- first words;
- learning to read;
- first chore;
- adolescent identity/autonomy event;
- coming of age;
- adult transition.

Not every milestone requires RNG.

Prefer deterministic eligibility windows and one-time thresholds.

---

## 55. Milestone Timing

Use:

```text
eligible age window
+ support-quality modifier
+ deterministic seed
```

This gives variation without reroll.

Persist selected trigger day once scheduled or derive deterministically from child seed.

---

## 56. First Steps / First Words

Do not implement as medical developmental tests.

Treat as narrative milestones in a broad age window.

Poor support may modestly shift timing but should not produce medicalized outcomes.

---

## 57. Reading Milestone

Should depend on:

- child stage;
- education exposure;
- literacy curriculum;
- support quality.

EducationSystem provides learning fact.

ChildDevelopmentSystem emits milestone when threshold reached.

---

## 58. First Chore

Trigger when:
- stage capability unlocked;
- child completes first valid chore.

Work system emits task-completed event.

No fake chore completion from UI.

---

## 59. Development Event Log

Persist only meaningful milestones and stage transitions.

Do not store every daily care evaluation.

Journal may render:
- first steps;
- first words;
- reading;
- coming of age.

This keeps save state compact.

---

## 60. Needs Priority

Children's critical needs outrank:

- education;
- chores;
- hobbies;
- celebrations;
- cultural events.

Scheduler must respect.

No child should continue a chore while starving/critically ill.

---

## 61. Care Policy UI

Player should manage care at a policy/assignment level.

Show:
- child;
- stage;
- care requirement;
- primary/backup caregiver;
- coverage status.

Avoid hourly diaper/feeding buttons.

---

## 62. Education UI Integration

Child detail may show:

- education status;
- teacher;
- learning focus;
- skill progress;
- stage learning modifier.

But EducationSystem remains source.

Do not duplicate school UI logic.

---

## 63. Child Detail UI

Display:

- stage;
- age;
- stage progress;
- stage capabilities;
- current care coverage;
- needs summary;
- milestone history;
- education link;
- emerging interests/traits where revealed.

Keep stage explanations localized.

---

## 64. Stage Transition Notification

Use compact notification:

```text
<name> entered adolescence.
New capabilities: ...
New needs: ...
```

Do not force a modal unless transition has important player decisions.

---

## 65. Coming-of-Age UI

At young-adult transition, show:

- major learned skills;
- emerged traits;
- education path;
- relationships/caregiver history;
- chosen specialization if relevant.

This creates payoff.

---

## 66. Accessibility

Support:

- keyboard/controller navigation;
- text scaling;
- stage status not color-only;
- care coverage expressed in text;
- progress bars with numbers/labels;
- no timed responses;
- reduced-motion milestone animation.

---

## 67. Tutorial

First child:
- explain stages;
- caregiver assignment;
- education;
- milestone progression;
- adult transition.

Do not imply the player must optimize every developmental variable.

---

## 68. Journal Integration

Journal notable entries:

- birth/arrival;
- first steps/words;
- reading;
- first chore;
- major adolescent event;
- coming of age.

Do not journal every stage progress tick.

---

## 69. Quest Hooks

Source hooks:

- The Parent;
- The Teacher;
- The Caregiver;
- The Guide;
- The Ceremony;
- The Generation;
- The Legacy.

ChildDevelopmentSystem exports facts.

QuestSystem owns quest state/rewards.

---

## 70. Achievement Integration

Plan 149 can observe:

- first child reaches adulthood;
- multiple children reach adulthood;
- education milestones;
- coming-of-age ceremony.

ChildDevelopmentSystem does not own achievements/rewards.

---

## 71. Epilogue Integration

Plan 145 may consume:

- child raised to adulthood;
- mentor/caregiver bond;
- coming-of-age fact;
- education specialization;
- family continuity.

Export stable facts only.

---

## 72. Legacy Integration

Plan 140/175 or later generational systems may consume:

- adult-transition fact;
- parent IDs;
- emerged traits;
- learned skills.

ChildDevelopmentSystem owns no cross-campaign profile state.

---

## 73. Backstory Integration

Plan 174 can apply when child becomes adult.

Do **not** generate a pre-war occupation backstory for a shelter-born child.

Instead:
- use developmental history as their origin;
- occupation/profession can derive from education/apprenticeship later.

This prevents narrative contradictions.

---

## 74. Hobby Integration

Plan 161 can provide:
- play/leisure;
- emerging interests;
- teaching/group exposure.

ChildDevelopmentSystem only gates age-appropriate hobby capability.

No duplicate hobby progress.

---

## 75. Art/Culture Integration

Plan 178 may allow:
- child participation in readings/play;
- adolescent creative projects;
- coming-of-age cultural event.

Age-gated and optional.

---

## 76. Seasonal Event Integration

Plan 170 may host:
- coming-of-age ceremony;
- school presentation;
- family milestone celebration.

Adult transition remains independent.

---

## 77. Health Events

If child health deteriorates:
- canonical health system handles diagnosis/treatment;
- development consumes health-status modifiers.

No "health" field duplicated in development state.

---

## 78. Death Handling

If a child dies:

- fate/memorial system owns death;
- child development record becomes historical/inactive;
- caregiver assignment released;
- education/work assignments cleared;
- milestone progression stops;
- journal/memorial hooks may fire.

Do not delete historical record if save/epilogue needs it.

---

## 79. Orphaned Child

If parents die:
- family relation remains historical;
- caregiver can be reassigned;
- morale/relationship effects route externally.

No special orphan stat required.

---

## 80. Caregiver Death

On caregiver death/departure:

- assignment invalidated;
- backup promoted if available;
- UI warning;
- no immediate catastrophic developmental penalty.

Grace period allows reassignment.

---

## 81. Caregiver Burnout

If caregiver needs system shows severe exhaustion/stress:
- care quality can decline;
- scheduler suggests backup.

Do not add a second caregiver fatigue meter.

---

## 82. Shelter Safety

Child care quality may read:
- shelter hazard;
- contamination;
- temperature;
- raid status.

Do not create duplicate environment simulation.

Stage can change vulnerability or priority.

---

## 83. Education Quality

EducationSystem should provide normalized quality:

```text
none / basic / adequate / strong
```

or numeric band.

ChildDevelopmentSystem uses it for:
- learning multiplier;
- reading milestone eligibility;
- specialization readiness.

Do not calculate teacher quality independently.

---

## 84. Adolescent Specialization

At adolescent stage, allow focus:

- technical;
- medical;
- agriculture;
- social;
- leadership;
- logistics;
- creative;
- other real skill domains.

Education/apprenticeship system owns training.

ChildDevelopmentSystem records selected developmental focus only if needed.

---

## 85. Young Adult Career Direction

Source says career direction.

This should be a transition preference, not a locked class.

Potential inputs:
- strongest skills;
- interests;
- apprenticeship;
- caregiver/mentor influence;
- shelter needs.

Autonomy/work assignment remains dynamic after adulthood.

---

## 86. Deterministic RNG

Use `ISeededRng` only for bounded milestone/event variation.

Seed from:
- campaign seed;
- child ID;
- milestone ID;
- stage.

Never:
- `System.Random`;
- wall-clock time;
- UI order.

---

## 87. Development Determinism

Chronological stage should not require RNG.

RNG may affect:
- exact milestone day within safe window;
- contextual adolescent event selection;
- personality candidate tie-break.

This keeps core maturation predictable.

---

## 88. Save Schema

Persist:

- birth/migration age anchor;
- current stage if necessary;
- caregiver assignment;
- achieved milestones;
- emerged trait fact IDs;
- adult transition completed;
- any scheduled deterministic milestone day;
- migration provenance.

Do not persist duplicate skill/need/health values.

---

## 89. Old-Save Migration

The source says existing children get estimated ages/stages.

Use evidence hierarchy:

1. persisted birth day;
2. cohort age/progress field;
3. creation/recruitment day;
4. `isMatured` state;
5. deterministic fallback mapping.

Do not invent precise ages when evidence is absent.

Store migration confidence/provenance for diagnostics.

---

## 90. Migrating Already-Matured Legacy Children

If old save says `isMatured=true`:

- ensure survivor already exists in adult lifecycle;
- mark adult transition completed;
- do not re-add;
- preserve identity.

If mature flag exists but adult survivor handoff did not occur due old bug:
- repair through migration with explicit test.

---

## 91. Migrating Immature Legacy Children

If only boolean `false` exists:

- use age hints if any;
- otherwise choose deterministic estimated stage using cohort-specific migration rules;
- do not randomly assign on each load.

Prefer conservative stage mapping.

---

## 92. Migration Milestones

Do not retroactively emit every missed first smile/steps/words.

Mark earlier milestones as historically passed/suppressed if stage implies them.

Only future milestones should notify.

This avoids notification flood.

---

## 93. Migration Education

Do not grant retroactive skills merely because estimated age implies schooling.

Preserve existing canonical skills.

Developmental UI may show education history as unknown.

---

## 94. Migration Adult Handoff

If estimated age crosses adult threshold:

- detect whether canonical survivor already exists;
- if not, perform one repair handoff;
- use idempotent transition ID;
- record migration provenance.

Critical CI fixture.

---

## 95. Stage Definition Validation

Validate:

- unique stage IDs;
- ranges sorted;
- no overlap;
- no gaps;
- final young-adult threshold matches adult transition;
- capability IDs valid;
- need profiles valid;
- learning multipliers bounded;
- milestone profiles valid;
- localization keys exist.

---

## 96. Capability Registry

Create explicit capability IDs.

Examples:

```text
can_move_independently
can_communicate_basic
can_attend_school
can_perform_light_chore
can_apprentice
can_work_general
can_lead_training
can_transition_adult
```

Consumers check capability, not string stage names.

---

## 97. Why Capability IDs Matter

Avoid:

```csharp
if(stage == "adolescent")
```

throughout the codebase.

Use:
```csharp
childDevelopment.HasCapability(childId, Capability.WorkGeneral)
```

This makes stage tuning safer.

---

## 98. Dangerous Capability Registry

Some tasks should require:
- adult only;
- or explicit adolescent policy.

Examples:
- frontline combat;
- hazardous expedition;
- radiation cleanup;
- heavy industrial machinery;
- armed night guard.

Centralize these gates.

---

## 99. Work Assignment Validation

Every work assignment API should validate:

- survivor exists;
- stage capability;
- health;
- schedule;
- equipment/skill.

Do not rely on UI hiding invalid work.

---

## 100. Headless Validation

The same invalid task must be rejected headlessly.

This is essential because UI-only gating is not an authority.

---

## 101. Care Requirement Profiles

Create data:

```json
{
  "careProfiles": [
    {
      "id": "infant_high_care",
      "careHoursPerDay": 8,
      "supervisionWeight": 100
    }
  ]
}
```

Actual values require balance.

Avoid literal real-world caregiving simulation.

---

## 102. Care Evaluation Interval

Evaluate on:
- day tick;
- schedule checkpoint;
- care assignment change.

No per-frame care checks.

---

## 103. Care Coverage Metric

Derive:

```text
careCoverage = providedCareCapacity / requiredCareCapacity
```

Clamp.

Use bands:
- insufficient;
- adequate;
- strong.

Do not show pseudo-scientific precision to player unless useful.

---

## 104. Care Consequences

Insufficient care may produce:

- reduced learning;
- morale/stress hook;
- health-risk event under sustained severe deficit.

Use accumulated shortage threshold, not one missed tick causing disaster.

---

## 105. Excellent Care

Excellent care should not "accelerate age."

Benefits:
- better learning;
- milestone support;
- stronger relationship;
- reduced stress.

Chronological stage remains age-driven.

---

## 106. Development Support Score — Avoid Persistent Single Number

Do not persist one vague "development score" unless necessary.

Prefer derived inputs:
- care coverage;
- nutrition;
- education;
- social support.

This keeps cause/effect understandable.

---

## 107. Milestone Prerequisite Registry

Examples:

### First steps
- toddler stage;
- minimum age window;
- adequate health.

### First words
- toddler;
- communication exposure/support.

### Reading
- child;
- education/literacy fact.

### First chore
- child capability;
- completed task.

### Coming of age
- young adult threshold.

All prerequisites typed.

---

## 108. Milestone Event Effects

Most milestones should primarily:
- journal;
- relationship;
- morale;
- archive.

Keep mechanical rewards small.

Do not turn first words into permanent stat bonuses.

---

## 109. Parent/Caregiver Morale

Milestones may affect caregivers/parents.

Relations/Needs systems receive:
- pride;
- grief;
- concern.

ChildDevelopmentSystem identifies participants only.

---

## 110. Coming-of-Age Effect

Can emit:
- shelter morale event;
- relationship milestone;
- archive fact;
- achievement/quest hook.

Adult transition itself is not a reward.

---

## 111. Social Peer Groups

Children may have peer interactions via relations/hobby/education.

Do not create separate child-friendship matrix.

RelationsSystem remains owner.

---

## 112. Childhood Friend Milestones

Follow-on possible:
- first close friend;
- school group;
- adolescent peer conflict.

Not required for stage foundation.

---

## 113. Childhood Events Content Strategy

Source includes 8 development event families.

Start with deterministic milestone events.

Do not immediately author dozens of random child crises.

Foundation must be stage progression first.

---

## 114. Child Event Tone

Events should avoid:
- infantilizing older children;
- treating normal adolescent autonomy as pathology;
- excessive tragedy;
- repetitive "cute" popups.

Use concise, grounded shelter-life details.

---

## 115. UI Progress Semantics

Stage progress should show:

```text
Age: 8 years, 4 months equivalent
Stage: Child
Next stage: Adolescent
```

or campaign-appropriate abstraction.

Do not imply educational/development quality equals age progress.

---

## 116. Time-Scale Presentation

If aging is accelerated:
- UI must communicate in-world age consistently.

Do not show raw simulation-day multiplier to player unless debug.

The game should still say coherent ages.

---

## 117. Calendar Integration

If one campaign day advances multiple in-world age days:
- calendar/year display and child age may differ.

Document clearly in architecture.

Prefer a shared world-age time scale rather than child-only acceleration if generational gameplay is core.

---

## 118. AgingSystem Integration

At adult transition:
- AgingSystem takes over adult age effects;
- same age anchor continues.

Do not reset age to 18 or zero.

One continuous age history.

---

## 119. Education-to-Adult Handoff

Young adult's canonical skills already reflect education.

Adult transition should not reapply "graduation bonuses."

Preserve existing skills directly.

---

## 120. Inventory Handoff

If child has personal inventory/keepsakes:
- preserve owner ID;
- no duplicate transfer needed if same survivor aggregate.

If separate child inventory exists, migrate transactionally.

---

## 121. Room/Bed Handoff

Child-specific sleeping arrangements may change.

Shelter occupancy system receives lifecycle transition event.

Do not hardcode room changes in ChildDevelopmentSystem.

---

## 122. Family Authority

Parent IDs should come from canonical family system if Plan 150 exists.

If current cohort state stores parent IDs:
- adapt, do not duplicate.

ChildDevelopmentState may cache only stable references.

---

## 123. Parent Unknown

A child may have:
- one parent;
- two parents;
- no known parent in shelter.

System must still work with caregiver assignment.

No assumption of family structure.

---

## 124. Caregiver Choice

Player can assign:
- parent;
- relative;
- trusted survivor;
- community caregiver.

No special moral penalty for non-parent caregiving.

---

## 125. Education Access Failure

If no teacher/school exists:
- child remains chronologically developing;
- education progress slower/absent;
- no stage deadlock.

Adult transition still occurs.

---

## 126. Extreme Scarcity

During famine/war:
- education/chores may pause;
- care/survival prioritized.

Do not freeze chronological development.

Long-term effects remain bounded and routed through needs/skills.

---

## 127. No-Caregiver Edge Case

If no eligible caregiver exists:
- warning;
- use communal/basic care fallback if shelter population exists;
- severe sustained shortage can trigger support consequences.

Do not instantly kill child due to one missing assignment.

---

## 128. One-Adult Shelter Edge Case

A single adult with multiple children:
- scheduler must trade off duties/care;
- system remains playable;
- care can be insufficient but not bugged.

Test explicitly.

---

## 129. Large Cohort Performance

Test:
- 100+ children;
- multiple stages;
- caregivers;
- education groups.

Daily tick should be O(children) or close.

Avoid pairwise relationship scans.

---

## 130. Stage Indexing

Pre-index:
- children by stage;
- milestones by stage;
- due transitions by age threshold.

This supports large generational simulations.

---

## 131. Save Footprint

Per child persist only:
- age anchor;
- caregiver refs;
- milestones;
- transition facts;
- minimal stage metadata.

Do not snapshot every daily support calculation.

---

## 132. Development Digest

Normalize:

```text
childId
birth/age anchor
stage
caregiver IDs
milestone IDs
emerged trait facts
adult transition flag
```

Hash for selftest.

Same save/load must yield same digest.

---

## 133. Data Integrity Self-Test Extension

Validate:
- stage definitions;
- capability IDs;
- care profile IDs;
- needs profiles;
- education hooks;
- milestone IDs;
- adult transition threshold;
- localization keys;
- no range overlap/gap;
- no stage with impossible required capability.

---

## 134. Dedicated `--child-development-selftest`

Selftest should:

1. load stage catalog;
2. create infant fixture;
3. advance to toddler threshold;
4. verify stage transition once;
5. trigger first steps/words deterministically;
6. validate caregiver assignment;
7. verify care coverage influence;
8. advance into child stage;
9. integrate education fact;
10. trigger reading/first chore;
11. advance to adolescent;
12. verify capability expansion;
13. trigger contextual autonomy event;
14. advance young adult;
15. perform adult transition;
16. verify no duplicate survivor;
17. save/reload at multiple points;
18. test old boolean-state migration;
19. verify headless task gating;
20. exit non-zero on mismatch.

---

## 135. Unit Test Matrix

### Stage catalog
- valid;
- overlap;
- gap;
- bad capability;
- invalid range;
- duplicate ID.

### Aging
- below threshold;
- exact threshold;
- multi-day advance crossing stage;
- year/time-scale conversion.

### Care
- primary caregiver;
- backup;
- overload;
- missing;
- caregiver death.

### Education
- no teacher;
- adequate school;
- learning multiplier;
- reading milestone.

### Capabilities
- infant task denied;
- child chore allowed;
- dangerous child task denied;
- adolescent allowed tasks;
- adult transition.

### Persistence
- infant;
- adolescent;
- milestones;
- caregiver;
- transition;
- migration.

---

## 136. Golden Development Fixtures

Create:

1. newborn/infant with strong care.
2. infant with temporarily insufficient care.
3. toddler milestone progression.
4. child with education.
5. child without education.
6. child completes first chore.
7. adolescent with strong family trust.
8. adolescent under high conflict.
9. young adult prepared for transition.
10. transition at exact threshold.
11. save/reload immediately before transition.
12. legacy `isMatured=false`.
13. legacy `isMatured=true` already adult.
14. legacy mature flag missing adult handoff.
15. large cohort.

---

## 137. Property / Fuzz Tests

Properties:

- stage always valid for age;
- stage never regresses;
- milestone emits at most once;
- adult transition at most once;
- caregiver IDs resolve or are null;
- no invalid stage capability assignment;
- child never exists simultaneously in contradictory lifecycle states;
- same seed/state deterministic.

---

## 138. Time Compression Fuzz

Randomly test age-scale configurations.

Properties:
- transitions occur in order;
- no skipped adult handoff;
- one large time advance crossing multiple stages emits ordered transitions safely;
- milestone suppression policy handles skipped windows.

This matters for debug/fast-forward.

---

## 139. Fast-Forward Semantics

If simulation advances 100 days at once:

- derive all crossed stages;
- emit transitions in order;
- do not require one tick per day;
- milestone events may be summarized/suppressed according to policy.

Avoid missing adult transition.

---

## 140. Offline/Paused Simulation

If the game does not simulate while closed:
- no aging while closed.

If it does:
- age authority handles elapsed simulation time;
- ChildDevelopmentSystem receives resulting progression.

No wall-clock-specific child logic.

---

## 141. Chore Exploit Prevention

Prevent:
- assigning a child to multiple chores simultaneously;
- using light chores as full production replacement;
- skill XP farming from zero-cost rapid tasks;
- bypassing stage checks through headless APIs.

Use task reservation and XP caps.

---

## 142. Learning Exploit Prevention

Prevent:
- save/load reroll learning multiplier;
- duplicated lesson completion;
- simultaneous multiple curricula if education forbids;
- infinite tutoring loops.

Education authority owns idempotency; development supplies stage multiplier.

---

## 143. Care Exploit Prevention

Prevent:
- assigning same caregiver beyond capacity without penalty;
- UI-only care assignment with no scheduler reservation;
- duplicate care credit from parent + caregiver record pointing to same person twice.

Normalize caregiver IDs.

---

## 144. Adult Transition Exploit Prevention

Prevent:
- transition event granting duplicate items;
- duplicate adult roster entries;
- rerunning ceremony for rewards;
- resetting age or skills.

Transition ID idempotency is mandatory.

---

## 145. Balance Framework

Childhood gameplay should create:

Primary:
- emotional/generational continuity;
- care/resource planning;
- education choices;
- future survivor development.

Secondary:
- light chores;
- relationships;
- milestones;
- culture.

Not:
- optimal labor pipeline;
- punishment sink;
- stat-maxing spreadsheet.

---

## 146. Chore Contribution Budget

Generate:

`docs/child_development/CHILD_TASK_BALANCE.md`

For each stage:
- allowed task types;
- maximum productive contribution;
- skill XP cap;
- supervision requirement.

Make child contribution helpful but non-essential.

---

## 147. Learning Calibration

Generate:

`docs/child_development/LEARNING_CALIBRATION.md`

Simulate:
- no education;
- basic;
- strong;
- excellent support.

Measure:
- skill levels at adolescence;
- skill levels at adulthood;
- variance.

Reject settings that create super-skilled adults automatically.

---

## 148. Care Calibration

Simulate:
- one infant;
- multiple infants;
- mixed stages;
- single caregiver;
- multiple caregivers.

Measure:
- caregiver hours;
- duty impact;
- support quality.

Ensure care is meaningful but not overwhelming.

---

## 149. Development Coverage Report

Generate:

`docs/child_development/DEVELOPMENT_COVERAGE.md`

| Stage | Capabilities | Need profile | Milestones | Education | Work gates | Fixtures |
|---|---:|---:|---:|---:|---:|---:|
| infant | | | | | | |
| toddler | | | | | | |
| child | | | | | | |
| adolescent | | | | | | |
| young_adult | | | | | | |

Flag:
- unreachable stages;
- unused capabilities;
- missing UI;
- missing integration.

---

## 150. UI Snapshot Cases

Capture:

- infant detail;
- toddler milestone;
- child education panel;
- adolescent capability view;
- young-adult transition;
- missing caregiver warning;
- high text scale;
- long milestone history.

No critical information only in icons.

---

## 151. Structured Diagnostics

Logs:

```text
ChildStageChanged child=<id> from=<id> to=<id> day=<n>
ChildMilestoneReached child=<id> milestone=<id>
CaregiverAssigned child=<id> caregiver=<id>
ChildAdultTransitioned child=<id> transition=<id>
ChildDevelopmentMigrated child=<id> provenance=<kind>
```

Do not log daily care spam.

---

## 152. Implementation Phase A — Audit and Age Authority

Tasks:
1. audit cohort maturation;
2. audit all age fields;
3. choose canonical age clock;
4. document time scale;
5. add compatibility adapter;
6. baseline tests.

Exit:
one authoritative child-age calculation.

---

## 153. Implementation Phase B — Stage Contract

Tasks:
1. `DevelopmentStage` DTO;
2. capability registry;
3. care profiles;
4. stage catalog;
5. loader;
6. validator;
7. stage derivation;
8. transition records.

Exit:
children deterministically occupy valid stages.

---

## 154. Implementation Phase C — Caregiver and Needs

Tasks:
1. caregiver assignment;
2. eligibility;
3. care capacity;
4. backup caregivers;
5. coverage bands;
6. NeedsSystem integration;
7. caregiver death handling;
8. tests.

Exit:
infant/toddler care works without micro-clicks.

---

## 155. Implementation Phase D — Education and Learning

Tasks:
1. education eligibility;
2. stage learning multipliers;
3. literacy/read milestone;
4. skill practice integration;
5. nutrition/support modifiers;
6. caps;
7. tests.

Exit:
learning is stage-aware but owned by canonical systems.

---

## 156. Implementation Phase E — Capabilities and Chores

Tasks:
1. stage capability API;
2. work assignment gates;
3. light chore profiles;
4. dangerous-task denial;
5. first chore event;
6. headless validation;
7. balance tests.

Exit:
children can contribute in bounded age-appropriate ways.

---

## 157. Implementation Phase F — Adolescence and Personality

Tasks:
1. autonomy expansion;
2. identity/trait evidence;
3. contextual rebellion event;
4. specialization eligibility;
5. relation hooks;
6. tests.

Exit:
adolescence has character without random disobedience spam.

---

## 158. Implementation Phase G — Young Adult Handoff

Tasks:
1. transition eligibility;
2. SurvivorLifecycle command;
3. idempotent transition;
4. skills/traits preservation;
5. assignment cleanup;
6. adult needs/aging handoff;
7. coming-of-age hook;
8. tests.

Exit:
one child becomes one adult survivor cleanly.

---

## 159. Implementation Phase H — Milestones

Tasks:
1. milestone definitions;
2. deterministic timing;
3. first smile/laugh;
4. first steps/words;
5. reading;
6. first chore;
7. coming of age;
8. journal;
9. quest/achievement hooks.

Exit:
development has meaningful narrative beats.

---

## 160. Implementation Phase I — Old-Save Migration

Tasks:
1. evidence hierarchy;
2. age estimation;
3. mature false mapping;
4. mature true mapping;
5. adult handoff repair;
6. milestone suppression;
7. no retroactive education bonuses;
8. fixtures.

Exit:
old cohorts migrate without duplication or notification flood.

---

## 161. Implementation Phase J — UI

Tasks:
1. child detail;
2. caregiver panel;
3. stage capabilities;
4. progress;
5. needs;
6. milestones;
7. education link;
8. transition view;
9. accessibility.

Exit:
UI is a projection of Core state.

---

## 162. Implementation Phase K — CI Hardening

Tasks:
1. `--child-development-selftest`;
2. data integrity;
3. fuzz;
4. time compression;
5. migration;
6. large cohort performance;
7. chore/learning/care exploit tests;
8. UI snapshots;
9. docs;
10. full regression.

Exit:
development is deterministic and supportable.

---

## 163. Exact File Plan

Expected new/modified files, adjusted to repository reality:

### Core
- `Assets/Ashfall.Core/Cohort/ChildDevelopmentSystem.cs`
- `Assets/Ashfall.Core/Cohort/DevelopmentStageDefinition.cs`
- `Assets/Ashfall.Core/Cohort/ChildDevelopmentRecord.cs`
- `Assets/Ashfall.Core/Cohort/ChildDevelopmentState.cs`
- `Assets/Ashfall.Core/Cohort/ChildDevelopmentEvents.cs`
- `Assets/Ashfall.Core/Cohort/ChildCapabilityRegistry.cs`
- `Assets/Ashfall.Core/Cohort/ChildAgeAuthority.cs` or adapter

### Data
- `Assets/StreamingAssets/Data/development_stages.json`

### Tests
- `Ashfall.Core.Tests/Cohort/ChildDevelopmentSystemTests.cs`
- `Ashfall.Core.Tests/Cohort/ChildDevelopmentMigrationTests.cs`
- `Ashfall.Core.Tests/Cohort/ChildAdultTransitionTests.cs`
- `Ashfall.Core.Tests/Cohort/ChildCapabilityGateTests.cs`
- `Ashfall.Core.Tests/Cohort/ChildDevelopmentFuzzTests.cs`

### Docs
- `docs/child_development/CHILD_DEVELOPMENT_INTEGRATION_AUDIT.md`
- `docs/child_development/CHILD_TIME_SCALE.md`
- `docs/child_development/DEVELOPMENT_COVERAGE.md`
- `docs/child_development/LEARNING_CALIBRATION.md`
- `docs/child_development/CHILD_TASK_BALANCE.md`
- `docs/child_development/CHILD_DEVELOPMENT_MIGRATION.md`

---

## 164. Bootstrap / Composition Root

Recommended order:

```text
load development stage catalog
construct age authority
construct child development
restore cohort/family/survivor state
restore child development state
wire needs/education/relations/scheduler adapters
wire day tick
wire adult transition sink
bind UI projection
```

Restore order must prevent a child from being temporarily treated as adult before development state is loaded.

---

## 165. Event Contracts

Recommended:

```csharp
ChildStageChanged(childId, fromStageId, toStageId, day)
ChildDevelopmentMilestoneReached(childId, milestoneId, day)
ChildCareCoverageChanged(childId, band)
ChildAdultTransitionReady(childId)
ChildAdultTransitioned(childId, day)
```

Consumers subscribe.

Do not let UI infer events by comparing labels.

---

## 166. No Per-Frame Polling

Development runs on:

- day/age tick;
- caregiver assignment change;
- education completion event;
- task completion;
- relationship milestone;
- transition threshold.

Never evaluate every child every rendered frame.

---

## 167. One Large Time Advance

If debug/fast-forward crosses several stage thresholds:

```text
infant -> toddler -> child
```

Process transitions in order.

Do not emit impossible intermediate milestone popups unless policy allows.

State must end at correct stage.

---

## 168. Stage Transition Side Effects

Changing stage may notify:

- scheduler;
- needs;
- education;
- UI;
- family;
- achievement.

But transition should not directly:
- grant large skills;
- create items;
- change relationships.

Those systems decide.

---

## 169. Capability Change Notification

When stage changes, downstream work scheduler invalidates now-invalid assignments and enables newly legal ones.

Example:
- child → adolescent unlocks apprenticeship/work profiles.

No stale assignment remains.

---

## 170. Needs Profile Change Notification

NeedsSystem receives stage-profile change.

Do not reset current hunger/sleep/morale.

Only change rates/requirements through canonical transition logic.

---

## 171. Education Profile Change

EducationSystem receives:
- new stage;
- learning-rate profile;
- eligible curricula.

Do not reset learning progress.

---

## 172. Relationship Continuity

Stage transitions do not reset relationships.

Parent/caregiver/friend bonds remain.

Adult transition preserves all stable IDs.

---

## 173. Naming and Localization

Stage names and milestone text use localization keys.

Avoid hardcoding "teenager" if project prefers "adolescent" or lore-specific labels.

Content language should remain neutral and respectful.

---

## 174. Risk Register

### Risk: literal 18-year stages unreachable
Mitigation:
- explicit time-scale audit and reachability simulation.

### Risk: duplicated age authorities
Mitigation:
- single clock invariant and compatibility adapter.

### Risk: children become optimization labor
Mitigation:
- capability gates, chore budget, dangerous-task restrictions.

### Risk: care becomes tedious
Mitigation:
- capacity/policy model, not hourly commands.

### Risk: poor care becomes punitive pseudo-medical system
Mitigation:
- bounded support effects and no clinical diagnosis.

### Risk: adult transition duplicates survivor
Mitigation:
- stable identity and idempotent handoff.

### Risk: old saves get notification/stat floods
Mitigation:
- migration suppression and no retroactive education bonuses.

---

## 175. Definition of Done — Flagship

### Architecture
- [ ] one child age authority
- [ ] `ChildDevelopmentSystem.cs`
- [ ] stage catalog
- [ ] capability registry
- [ ] schema-versioned state
- [ ] boolean maturation adapted/deprecated

### Stages
- [ ] infant
- [ ] toddler
- [ ] child
- [ ] adolescent
- [ ] young adult
- [ ] valid contiguous ranges
- [ ] reachable under configured time scale

### Care
- [ ] caregiver assignment
- [ ] backup caregiver
- [ ] capacity/coverage
- [ ] needs integration
- [ ] no per-hour micromanagement

### Learning
- [ ] education eligibility
- [ ] learning rate profile
- [ ] skill integration
- [ ] nutrition/support modifiers
- [ ] no duplicate skill authority

### Capabilities
- [ ] stage gates
- [ ] light chores
- [ ] dangerous task restrictions
- [ ] headless validation
- [ ] adolescent progression

### Milestones
- [ ] first smile/laugh
- [ ] first steps/words
- [ ] reading
- [ ] first chore
- [ ] adolescence event
- [ ] coming of age
- [ ] once-only emission

### Adult handoff
- [ ] transactional
- [ ] same identity
- [ ] preserves skills/traits/relations
- [ ] no duplicate survivor
- [ ] adult aging/needs active

### Migration
- [ ] old immature children
- [ ] old matured children
- [ ] broken handoff repair
- [ ] no retroactive milestone spam
- [ ] no retroactive skill grants

### Validation
- [ ] selftest
- [ ] data integrity
- [ ] fuzz
- [ ] time compression
- [ ] large cohort
- [ ] save round-trip
- [ ] headless

---

## 176. Follow-On Task 183-A — Child Talent & Interest Emergence

Goal:
allow interests/talents to emerge from education, hobbies, family, and personality evidence.

Rules:
- no predetermined genetic destiny;
- deterministic evidence-based selection;
- skill systems remain owner.

Outputs:
- preference tags;
- education recommendations;
- hobby affinity.

---

## 177. Follow-On Task 183-B — Childhood Friendship & Peer Groups

Goal:
add age-appropriate peer relations.

Requires:
- SurvivorRelationsSystem;
- school/hobby groups;
- no separate friendship authority.

Potential events:
- first friend;
- group conflict;
- shared project.

---

## 178. Follow-On Task 183-C — School & Childhood Cultural Events

Goal:
connect Plan 154 education, Plan 161 hobbies, Plan 170 celebrations, and Plan 178 culture.

Examples:
- reading presentation;
- school play;
- small competition;
- coming-of-age performance.

No duplicate calendar.

---

## 179. Follow-On Task 183-D — Generational Inheritance Integration

Goal:
connect parent traits/history to later adult identity.

Requires:
- explicit Plan 140/150 contract;
- bounded inheritance;
- no deterministic "parent stat cloning."

ChildDevelopmentSystem exports developmental history only.

---

## 180. Follow-On Task 183-E — Child-Specific Narrative Event Pack

Goal:
add grounded events after foundation is stable.

Categories:
- learning;
- family;
- friendship;
- shelter responsibility;
- fear/recovery;
- aspirations.

Avoid repetitive tragedy and stereotype.

---

## 181. Follow-On Task 183-F — Adult Origin Synthesis

Goal:
when shelter-born young adult transitions, synthesize a non-prewar origin record for Plan 174/backstory consumers.

Inputs:
- caregivers;
- education;
- milestones;
- hobbies;
- traits;
- major childhood events.

Output:
- stable developmental-origin facts.

No false pre-war occupation.

---

## 182. Final Guardrails

- No second age clock.
- No `System.Random`.
- No wall-clock aging.
- No per-frame child development scan.
- No boolean maturation authority after cutover.
- No stage regression.
- No overlapping/gapped age ranges.
- No literal 18-year progression without time-scale proof.
- No adult transition twice.
- No duplicate adult survivor.
- No skill reset at adulthood.
- No relationship reset at adulthood.
- No retroactive old-save skill grants.
- No retroactive milestone notification flood.
- No heavy/dangerous child work through UI or headless bypass.
- No "poor care = toddler forever" pseudo-clinical mechanic.
- No random adolescent disobedience coin flip.
- No caregiver micromanagement every hour.
- No duplicate health/needs/education authorities.
- No coming-of-age ceremony requirement for maturation.
- No family-structure assumptions.
- No child-state deletion on adult transition before historical facts are safely handed off.

When complete, Plan 183 should turn childhood from a boolean waiting room into a coherent generational lifecycle.
Children will move through distinct stages, need different kinds of support, learn at age-appropriate rates,
reach memorable milestones, gradually gain capabilities and autonomy, and eventually enter the adult survivor
system as the same persistent person—with their skills, relationships, education, family history, and developmental
record intact.

The flagship proof is architectural as much as narrative: one clock, one stage authority, one idempotent adult
handoff, explicit capability gates, safe migration, bounded care mechanics, and deterministic CI coverage.
