# C2 — Flagship Integration Plan [40]: Survivor Personal Quests, Character Arcs, Identity-Driven Narrative Progression, and Emergent Life Stories

> **Deliverable:** `C2_planintegration[40].md`
> **Source scope:** Plan 200 — *Survivor Personal Quests & Character Arcs*
> **Primary objective:** create a deterministic survivor-personal narrative system in which meaningful experiences, relationships, traits, skills, memories, possessions, locations, and psychological state can activate authored personal quests and multi-stage character arcs; let those quests branch and resolve through canonical gameplay facts; and make each survivor accumulate an individual narrative trajectory without duplicating the shared quest runtime, relationship state, skill progression, trait state, memory, romance, psychology, location, item, or consequence authorities that already own those facts.
> **Required execution order:** **200A Foundation / Narrative Contract → 200B Quest Generation, Stages, Branching, Arcs & UI → 200C Cross-System Integration, Save/CI, Exploit Control, Balance, and Long-Run Narrative Closure**
> **Hard dependencies:** shared quest runtime from Plan 134 where available; `SurvivorRelationsSystem`; `SkillProgressionSystem`; canonical `TraitSystem`; `EventSystem`; `JournalSystem`; Plan 132 hidden agendas where secret motivation intersects but does not become personal-quest truth; Plan 147 per-NPC memory when available; Plan 150 romance/family for love/family arcs; Plan 179 psychology for psychological consequences; Plan 31 semantic events; Plan 36 ports; Plan 39 save durability; Plan 55 retention; Plan 162 archive/legacy for remembered landmark arcs.
> **Scope discipline:** no second generic quest engine if Plan 134 already owns quest lifecycle; no duplicate relationship meter; no duplicate skill XP; no duplicate trait state; no duplicate memory graph; no duplicate romance or family progression; no duplicate psychology state; no quest stage that fabricates an item/location/event the canonical world has not produced; no polling every survivor/system every frame; no arbitrary “arc progress” percentage detached from actual narrative milestones; no personal quest reward that writes directly into another system’s private state; and no personal quest generation that ignores survivor history merely to fill an active-slot quota.

---

# 0. Executive Intent

ASHFALL already has many ingredients of character identity:

- skills,
- traits,
- relationships,
- hidden agendas,
- survivor memories,
- psychology,
- family/romance,
- events,
- journals,
- expeditions,
- items,
- locations,
- consequences.

What it lacks is a layer that turns those facts into an individual **story trajectory**.

Without that layer, a survivor can:

```text
gain skill
lose a friend
survive a raid
visit a ruin
acquire a keepsake
form a romance
recover from trauma
```

yet still feel narratively flat because the game never connects those moments into:

```text
a question
→ a goal
→ a sequence of meaningful stages
→ a choice
→ a consequence
→ a remembered outcome
```

The target architecture is:

```text
canonical survivor history/state
       │
       ├─ relationships
       ├─ skills
       ├─ traits
       ├─ events
       ├─ memories
       ├─ psychology
       ├─ items
       ├─ locations
       └─ family/romance
       │
       ▼
PersonalQuestSystem
       │
       ├─ trigger eligibility
       ├─ template selection
       ├─ survivor-specific binding
       ├─ stage progression
       ├─ branching
       ├─ quest history
       └─ arc interpretation
       │
       ▼
canonical downstream owners
       │
   ┌───┼─────────┬──────────┬──────────┬──────────┐
   ▼   ▼         ▼          ▼          ▼          ▼
Relations Skills Traits  Psychology  Quests    Journal/Archive
```

The strongest product outcome is:

> **A survivor who survives a specific sequence of events can develop a personal storyline that could not simply be swapped onto another survivor without feeling wrong. Their quest emerges from what happened to them, progresses through real gameplay state, branches through meaningful choices, and resolves into a durable character outcome remembered by the shelter.**

---

# 1. Source Diagnosis

The source establishes:

- no `PersonalQuest`, `CharacterArc`, `SurvivorStory`, `PersonalStoryline`, `IndividualQuest`, or `SurvivorNarrative` system currently exists,
- survivor skills, relations, traits, and events are not composed into personal narrative,
- Plan 132 adds hidden agendas but not open character arcs,
- Plan 147 adds memory but not personal quests,
- Plan 179 adds psychology but not narrative development,
- Plan 185 adds memory decay but not character growth,
- eight quest types are required:
  - redemption,
  - growth,
  - discovery,
  - revenge,
  - protection,
  - legacy,
  - love,
  - mastery,
- six arc types are required:
  - hero,
  - tragic,
  - redemption,
  - wisdom,
  - corruption,
  - growth,
- trigger domains are:
  - relationship milestone,
  - skill achieved,
  - event experienced,
  - item acquired,
  - location visited,
  - trait activated,
  - time passed,
- quests contain 3–7 sequential stages,
- stages may branch,
- quest consequences affect canonical survivor state,
- maximum active quests defaults to 2 per survivor,
- quest generation is deterministic,
- 20+ templates are required,
- old saves begin with no active personal quests,
- headless processing and `--personal-quest-selftest` are required.

The key architectural problem is that the source proposes a whole `PersonalQuest` runtime even though the broader C2 program already contains Plan 134 for a shared quest runtime.

Therefore the preferred architecture is:

```text
SharedQuestRuntime
= generic lifecycle/state machine

PersonalQuestSystem
= survivor-specific trigger/template/arc orchestration
```

If Plan 134 is not yet landed:

- implement a narrow compatibility adapter,
- do not create an incompatible second generic quest runtime that must later be migrated.

---

# 2. Program-Level Success Criteria

C2[40] closes only when all of the following are true.

1. `PersonalQuestSystem` exists.
2. Personal quests use the shared quest runtime when available.
3. Eight source quest types are supported.
4. Six source arc types are supported.
5. Quest generation is deterministic under `ISeededRng`.
6. Personal quests are generated from canonical survivor history/state.
7. A quest cannot activate if required history is absent.
8. Quest stages use typed completion conditions.
9. Stage completion is event-driven where practical.
10. Daily reconciliation is allowed only as bounded fallback/catch-up, not continuous whole-world polling.
11. Stages progress sequentially unless an authored branch explicitly changes path.
12. Branch choice is persisted and idempotent.
13. Completed stages cannot reapply rewards after load.
14. Failed stages cannot re-run failure consequences after load.
15. Quest completion routes rewards through canonical owners.
16. Quest failure routes penalties through canonical owners.
17. Quest abandonment routes consequences through canonical owners.
18. Arc progression derives from meaningful milestone facts, not an independently grindable raw bar.
19. Love quests integrate with Plan 150 romance/family semantics.
20. Revenge quests require a real grievance/target.
21. Redemption quests require a real prior failure/harm/guilt basis.
22. Mastery quests use `SkillProgressionSystem`.
23. Legacy quests create real persistent projects/outputs.
24. Discovery quests use canonical location/information systems.
25. Protection quests target real relationships/people/assets.
26. Trait-triggered quests consume canonical trait activation.
27. Psychology integration uses Plan 179 rather than shadow mental-state fields.
28. Memory integration uses Plan 147 where available.
29. Hidden agendas may influence eligibility but remain Plan 132 truth.
30. Old saves load with no fabricated historical questline.
31. Quest history is retention-aware.
32. UI shows why a quest exists and what the survivor is trying to do.
33. UI does not reveal hidden branch outcomes unless authored as visible.
34. Headless simulation progresses quests without UI.
35. 20+ templates validate against referenced skills, traits, events, items, locations, and relationship conditions.
36. All eight quest types become runtime-observed in targeted fixtures.
37. No survivor exceeds configured active-quest cap unless explicitly exempted.
38. Quest generation does not become a daily spam generator.
39. Same seed + same survivor history + same choices produce the same personal narrative.
40. `--personal-quest-selftest` passes.

---

# 3. Architectural Invariants

## 3.1 Shared quest runtime remains generic quest authority

If Plan 134 exists, it owns:

```text
quest instance ID
quest status
stage status
branch state
completion/failure
save/load
generic quest event sequencing
```

PersonalQuestSystem supplies survivor-specific templates and trigger context.

## 3.2 PersonalQuestSystem owns personal narrative selection

It owns:

- eligibility,
- survivor binding,
- template selection,
- personal narrative provenance,
- arc interpretation,
- active personal-quest cap,
- personal quest history projection.

## 3.3 Relationships remain canonical

A “protect friend” quest references:

```text
relationship pair
```

It does not own affinity.

## 3.4 Skills remain canonical

A mastery quest references:

```text
skill ID / level / milestone
```

It does not own XP.

## 3.5 Traits remain canonical

Quest may require a trait.

It does not own trait activation or mutation unless through canonical trait acquisition/removal API.

## 3.6 Memories remain canonical

Quest trigger can reference memory/event IDs.

PersonalQuestSystem does not create a second per-survivor memory graph.

## 3.7 Arc progress is milestone-based

Prefer:

```text
arc milestone set
```

over:

```text
arcProgress += 7
```

Arbitrary progress numbers are prone to drift.

## 3.8 Personal quests must be survivor-specific

Template binding requires actual relevant context.

No generic quest assignment solely because active slot is empty.

## 3.9 Narrative consequences are idempotent

Every stage/branch/quest consequence has stable application key.

## 3.10 Failure must be state-backed

A quest does not fail merely because a timer rolled randomly unless authored.

---

# 4. Dependency Graph

```text
Survivor canonical state/history
         │
         ├─ relations
         ├─ skills
         ├─ traits
         ├─ events
         ├─ memory
         ├─ psychology
         ├─ items
         ├─ locations
         └─ family/romance
         │
         ▼
 PersonalQuestEligibility
         │
         ▼
 PersonalQuestSystem
         │
         ├─ template selection
         ├─ context binding
         ├─ active-cap arbitration
         ├─ arc selection
         └─ follow-on generation
         │
         ▼
   SharedQuestRuntime
         │
         ├─ stages
         ├─ branch choices
         ├─ completion
         ├─ failure
         └─ save/load
         │
         ▼
 canonical consequence sinks
```

---

# 5. Baseline Capture

Before implementation, inspect:

- shared quest runtime from Plan 134 if landed,
- existing quest DTOs/state machine,
- `SurvivorRelationsSystem`,
- `SkillProgressionSystem`,
- `TraitSystem`,
- `EventSystem`,
- `JournalSystem`,
- Plan 132 hidden agenda state,
- Plan 147 memory interfaces,
- Plan 150 romance/family APIs,
- Plan 179 psychological profile APIs,
- item/inventory ownership,
- location/world-knowledge APIs,
- current quest UI,
- save registry/composition root,
- existing survivor narrative docs/catalogs.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Capture representative survivor fixtures:

```text
veteran with guilt
novice with growth opportunity
curious survivor with discovery opportunity
survivor with grievance/target
survivor with strong protective relationship
high-skill veteran seeking legacy/mastery
romance-eligible survivor pair
psychologically declining survivor
```

---

# 6. Workstream 200A — Foundation / Narrative Contract

## Goal

Create one survivor-personal narrative orchestration layer that selects authored quests from real survivor history and delegates generic quest lifecycle to the canonical quest runtime.

---

# 7. 200A Phase A — Create `PersonalQuestSystem`

Path:

```text
Assets/Ashfall.Core/Narrative/PersonalQuestSystem.cs
```

Responsibilities:

- consume personal-quest triggers,
- evaluate survivor eligibility,
- select deterministic quest templates,
- bind survivor-specific references,
- enforce per-survivor active limits,
- associate personal quests with character arcs,
- emit quest instances into shared runtime,
- interpret quest outcomes into arc milestones,
- expose personal-story read models,
- capture/restore personal-quest-specific metadata only.

---

# 8. 200A Phase B — Shared Quest Runtime Audit

Before defining a new full `PersonalQuest` DTO:

1. inspect Plan 134 contracts,
2. identify fields already owned by shared runtime,
3. reuse generic stage/status/branch persistence,
4. add only personal-narrative metadata.

Avoid duplicate:

```text
status
currentStage
completedDay
stage completion
branch state
```

if generic quest runtime already persists them.

---

# 9. 200A Phase C — Recommended Personal Metadata

If shared runtime exists:

```text
PersonalQuestBinding
{
    quest_instance_id
    survivor_id
    template_id
    personal_quest_type
    arc_id
    trigger_event_id
    bound_survivor_ids
    bound_location_ids
    bound_item_ids
    bound_faction_ids
    narrative_provenance
}
```

---

# 10. 200A Phase D — Compatibility Model If Plan 134 Is Absent

If no shared runtime exists yet:

- create a minimal internal quest adapter with the same future interface,
- isolate it behind:

```text
IPersonalQuestRuntime
```

so Plan 134 can replace implementation without data rewrite.

---

# 11. 200A Phase E — Quest Type Vocabulary

Exactly preserve the source eight:

```text
Redemption
Growth
Discovery
Revenge
Protection
Legacy
Love
Mastery
```

Stable IDs.

---

# 12. 200A Phase F — Arc Type Vocabulary

Exactly preserve source six:

```text
Hero
Tragic
Redemption
Wisdom
Corruption
Growth
```

Stable IDs.

---

# 13. 200A Phase G — Arc Theme Vocabulary

Source themes:

```text
Courage
Sacrifice
Knowledge
Love
Power
Freedom
```

Treat as data-authored theme tags.

Do not infer ideology/identity outside authored data.

---

# 14. 200A Phase H — Character Arc Model

Recommended:

```text
CharacterArcRecord
{
    arc_id
    survivor_id
    arc_type
    theme_id
    milestone_ids
    current_phase
    outcome
    started_day
    resolved_day
}
```

Avoid storing a raw `0–100` as primary truth.

---

# 15. 200A Phase I — Arc Progress Derivation

If UI needs percentage:

```text
completed weighted milestones / total weighted milestones
```

derived.

Do not persist a mutable percentage that can diverge.

---

# 16. 200A Phase J — Arc Outcome Vocabulary

Source:

```text
Positive
Neutral
Negative
Mixed
```

Use stable IDs.

Final outcome derives from authored milestone/outcome rules.

---

# 17. 200A Phase K — Quest Trigger Model

Typed trigger domains:

```text
RelationshipMilestone
SkillAchieved
EventExperienced
ItemAcquired
LocationVisited
TraitActivated
TimePassed
```

Additional future trigger types require explicit schema version.

---

# 18. 200A Phase L — Trigger Definitions vs Runtime Facts

Static:

```text
PersonalQuestTriggerDefinition
```

Runtime:

```text
PersonalQuestTriggerMatch
```

Do not persist every trigger definition per save.

---

# 19. 200A Phase M — Trigger Event Envelope

Recommended:

```text
trigger_event_id
trigger_type
survivor_id
source_id
day
context_refs
```

Used for deterministic quest generation.

---

# 20. 200A Phase N — Relationship Trigger

Inputs from `SurvivorRelationsSystem`:

- threshold/band crossed,
- new family relationship,
- conflict/grudge,
- reconciliation,
- romance interest where Plan 150 exposes it.

Do not poll raw affinity every frame.

---

# 21. 200A Phase O — Skill Trigger

Use skill milestone event:

```text
skill level reached
specialization unlocked
mastery threshold
```

from `SkillProgressionSystem`.

---

# 22. 200A Phase P — Event Trigger

Use semantic event IDs.

Examples:

- survivor wounded,
- survivor saved someone,
- survivor failed rescue,
- survivor witnessed death,
- survivor betrayed,
- survivor promoted,
- survivor survived disaster.

---

# 23. 200A Phase Q — Item Trigger

Use actual item acquisition/equipment/ownership event.

Examples:

- inherited keepsake,
- rare artifact,
- personal tool,
- weapon tied to past event.

Do not create item just to trigger quest.

---

# 24. 200A Phase R — Location Trigger

Use canonical visitation/discovery event.

No hidden location ID scanning.

---

# 25. 200A Phase S — Trait Trigger

Trait system emits activation/context.

Avoid generating a quest simply because survivor has static trait forever.

Prefer meaningful activation event.

---

# 26. 200A Phase T — Time-Passed Trigger

Must anchor to a real source event:

```text
N days since X
```

not arbitrary `day % N`.

---

# 27. 200A Phase U — Trigger Deduplication

A source trigger event should not generate the same template repeatedly.

Stable key:

```text
survivor
template
trigger source
```

---

# 28. 200A Phase V — Quest Template Model

`personal_quest_templates.json` contains:

```text
template_id
quest_type
name_key
description_key
eligibility
trigger_definitions
binding_rules
stage_definitions
branch_definitions
consequence_profiles
arc_profiles
cooldown
repeat_policy
rarity/weight
localization keys
```

---

# 29. 200A Phase W — Stage Definitions

Every stage has:

```text
stage_id
name_key
description_key
completion_condition
failure_condition optional
branch_options optional
consequence_profiles
next_stage_ids
```

---

# 30. 200A Phase X — 3–7 Stage Contract

Source requires:

```text
3–7 stages per quest
```

Validate at data-integrity time.

---

# 31. 200A Phase Y — Completion Conditions

Use typed condition vocabulary.

Examples:

```text
ReachRelationshipBand
ReachSkillLevel
CompleteExpedition
VisitLocation
AcquireItem
CraftItem
SurviveEvent
MakeMoralChoice
ProtectSurvivor
WaitDaysAfterEvent
ResolveConflict
TeachSurvivor
```

Do not use arbitrary script strings.

---

# 32. 200A Phase Z — Failure Conditions

Typed:

```text
TargetDied
RelationshipBroken
DeadlineExpired
RequiredItemDestroyed
LocationLost
OpposingBranchChosen
QuestContextInvalidated
SurvivorDied
```

Not every quest requires failure condition.

---

# 33. 200A Phase AA — Impossible vs Failed

Important distinction:

```text
impossible because target died
```

may become:

- failed,
- transformed,
- redirected,
- closed neutral,

according to template.

Do not automatically classify every invalidated path as failure.

---

# 34. 200A Phase AB — Abandonment

Player may abandon only if template allows.

Abandonment outcome:

- explicit,
- idempotent,
- may affect morale/relationship through canonical sink.

---

# 35. 200A Phase AC — Branch Definitions

A branch contains:

```text
branch_id
choice_key
eligibility
consequence profiles
next_stage_id
arc_milestones
```

---

# 36. 200A Phase AD — Choice Ownership

If branch is player choice:

- shared quest runtime records it.

If survivor-autonomous:

- use deterministic personality/psychology policy,
- persist decision.

Do not reroll.

---

# 37. 200A Phase AE — Personal Quest Generation

On trigger:

```text
collect eligible templates
→ filter by active cap/cooldown/history
→ score against survivor identity
→ deterministic weighted selection
→ bind context
→ instantiate quest
→ attach arc
```

---

# 38. 200A Phase AF — Eligibility Inputs

Use:

- traits,
- skills,
- relationship history,
- recent events,
- memories,
- psychology,
- age/life stage,
- occupation/background,
- family/romance,
- current location/world access,
- inventory ownership.

Only use facts available from canonical systems.

---

# 39. 200A Phase AG — Narrative Specificity Score

A template should score higher when more of its contextual requirements are satisfied.

Example:

```text
Redemption template
+ prior harmful choice
+ guilt
+ affected survivor still alive
→ strong fit
```

Generic fit:

```text
survivor has negative morale
```

should not outrank specific history.

---

# 40. 200A Phase AH — No Slot-Filling Quests

If no meaningful eligible template exists:

```text
generate nothing
```

Two active quests is a cap, not a target.

---

# 41. 200A Phase AI — Active Quest Cap

Source default:

```text
2 per survivor
```

Store in config/user/gameplay tuning.

Global shelter cap may also be useful for UI attention.

---

# 42. 200A Phase AJ — Quest Generation Cooldown

Per survivor:

- avoid back-to-back immediate quests,
- prioritize resolution time.

Source does not specify value.

Tune via soak.

---

# 43. 200A Phase AK — Quest Repeat Policy

Template:

```text
Never
OncePerSurvivor
RepeatAfterCooldown
RepeatWithDifferentBinding
```

Default personal arc quests:

```text
OncePerSurvivor
```

---

# 44. 200A Phase AL — Deterministic RNG

Use dedicated:

```text
personal_quests
```

Seed key:

```text
campaign seed
survivor ID
trigger event ID
generation sequence
```

Sort template candidates.

---

# 45. 200A Phase AM — Quest IDs

Stable runtime ID:

```text
personal:{survivor}:{template}:{trigger_event}
```

or stable deterministic GUID/hash.

---

# 46. 200A Phase AN — Arc IDs

Stable:

```text
arc:{survivor}:{arc_type}:{arc_sequence}
```

---

# 47. 200A Phase AO — Arc Creation Policy

A quest may:

- start an arc,
- advance an existing compatible arc,
- close an arc,
- branch an arc.

Do not create one arc per quest automatically.

---

# 48. 200A Phase AP — One Primary Arc vs Multiple Arcs

Recommended baseline:

- one active primary character arc,
- secondary completed arc history allowed.

This keeps narrative legible.

If multiple active arcs are needed:

- cap tightly.

---

# 49. 200A Phase AQ — Arc Milestones

Examples:

```text
AcceptedResponsibility
ProtectedSomeone
FailedSomeone
ChosePowerOverTrust
ForgaveEnemy
ReachedMastery
PassedKnowledgeOn
SacrificedOpportunity
```

These are narrative facts, not raw stat changes.

---

# 50. 200A Phase AR — Arc Resolution

Arc resolves when:

- terminal milestone,
- authored quest chain completed,
- survivor death,
- campaign completion,
- explicit transformation.

Outcome is derived.

---

# 51. 200A Phase AS — Survivor Death

If survivor dies mid-quest:

- quest closes through `SurvivorDied`,
- arc may resolve tragic/mixed/incomplete according to rules,
- no ghost progression.

---

# 52. 200A Phase AT — Survivor Departure

If roster removal/expulsion exists:

- quest pauses/closes/transfers only if authored.

No quest remains active forever for absent survivor.

---

# 53. 200A Phase AU — Quest Consequence Profile

Never write directly into arbitrary systems.

Define typed consequences:

```text
SkillRewardIntent
TraitChangeIntent
RelationshipEffectIntent
MoraleEffectIntent
PsychologyEffectIntent
InventoryGrantIntent
JournalEntryIntent
ArchiveLandmarkIntent
QuestUnlockIntent
```

---

# 54. 200A Phase AV — Skill Bonus Guard

Source says quest completion can grant skill bonus.

Use SkillProgressionSystem.

Prefer XP/milestone reward.

No direct skill-level assignment unless canonical API supports it.

---

# 55. 200A Phase AW — Skill Penalty Guard

Source says failure can cause skill penalty.

Use cautiously.

A failed personal quest should usually not erase learned competence.

Prefer:

- temporary confidence/work modifier,
- missed bonus,
- psychology consequence.

Only permanent skill loss if skill system/content explicitly supports it.

---

# 56. 200A Phase AX — Trait Improvement/Worsening

Trait change must use TraitSystem.

No ad hoc:

```text
trait = Better
```

Define allowed transformation table.

---

# 57. 200A Phase AY — Relationship Consequence

Use reason-coded relation events.

No local affinity.

---

# 58. 200A Phase AZ — Morale Consequence

Use canonical morale system.

No personal-quest morale field.

---

# 59. 200A Phase BA — Psychology Consequence

Plan 179 owns psychological profile.

Quest can emit:

- growth,
- guilt,
- confidence,
- distrust,
- resilience context

through approved API.

---

# 60. 200A Phase BB — Hidden Agenda Boundary

Plan 132 hidden agenda may:

- increase eligibility,
- create concealed motivation,
- conflict with open personal quest.

PersonalQuestSystem must not reveal hidden agenda truth unless discovered.

---

# 61. 200A Phase BC — Memory Boundary

Plan 147 memory may:

- provide triggering event,
- provide salience,
- be added/updated after quest outcome.

PersonalQuestSystem does not own memory decay/retention.

---

# 62. 200A Phase BD — Love Quest Boundary

Plan 150 owns:

- attraction/eligibility,
- courtship,
- partnership,
- family.

Love quest may structure narrative around those states.

It cannot manufacture mutual attraction.

---

# 63. 200A Phase BE — Revenge Quest Safety Rule

Requires:

- real grievance,
- identifiable target,
- target reachable/existing.

If target dies/disappears:

- transform or resolve according to authored rule.

No synthetic villain.

---

# 64. 200A Phase BF — Redemption Quest Rule

Requires a concrete source:

- moral failure,
- betrayal,
- abandonment,
- failed rescue,
- harmful choice,
- guilt-causing event.

No arbitrary “you feel guilty” generation.

---

# 65. 200A Phase BG — Growth Quest Rule

Requires:

- actual limitation,
- desired skill/behavior,
- challenge/opportunity.

---

# 66. 200A Phase BH — Discovery Quest Rule

Requires:

- real clue,
- secret,
- location,
- historical question,
- unknown relationship truth.

No fabricated world secret.

---

# 67. 200A Phase BI — Protection Quest Rule

Requires:

- real relationship,
- real target,
- actual threat/need.

---

# 68. 200A Phase BJ — Legacy Quest Rule

Requires:

- sufficient experience/skill,
- long-term project/opportunity.

Legacy output must be real:

- mentor,
- artifact,
- archive entry,
- structure,
- doctrine,
- book,
- training line.

---

# 69. 200A Phase BK — Mastery Quest Rule

Requires:

- high current skill,
- valid mastery threshold,
- final challenge.

Use SkillProgressionSystem.

---

# 70. 200A Phase BL — State Persistence

Persist personal metadata:

- personal quest bindings,
- arc records/milestones,
- generation cooldowns/history,
- unresolved personal narrative hooks,
- idempotency keys.

Generic quest runtime persists stages/status if available.

---

# 71. 200A Phase BM — Old Save Compatibility

Source requires:

```text
existing saves get no active quests
```

Implement exactly:

- empty personal active list,
- empty arc list,
- no retroactive quest generation from historical state on first load.

---

# 72. 200A Phase BN — Old Save Grace

Start generating only from new qualifying trigger events after migration.

Optional later backfill must be separate explicit migration feature.

---

# 73. 200A Phase BO — GameBootstrap Integration

Source asks:

```text
SetupPersonalQuests
TickPersonalQuests
SavePersonalQuests
```

Follow Plan 28 composition-root conventions.

Do not reintroduce manual orchestration if descriptors/generator already exist.

---

# 74. 200A Phase BP — Tick Model

Avoid “check every trigger continuously.”

Preferred:

```text
event-driven trigger queue
+ once-per-day bounded reconciliation for time-based conditions
```

---

# 75. 200A Phase BQ — Trigger Queue

Store unresolved trigger events only until:

- processed,
- expired,
- deduplicated.

Avoid infinite event history in live queue.

---

# 76. 200A Phase BR — Semantic Events

Candidate kinds:

```text
personal_quest_triggered
personal_quest_started
personal_quest_stage_completed
personal_quest_branch_selected
personal_quest_completed
personal_quest_failed
personal_quest_abandoned
character_arc_started
character_arc_milestone
character_arc_resolved
```

---

# 77. 200A Phase BS — Port Contract

Mandatory:

- shared quest runtime or compatibility runtime,
- relations,
- skills,
- traits,
- events,
- survivor roster/state.

Conditional:

- memory,
- psychology,
- romance/family,
- archive,
- item/location-specific systems.

---

# 78. 200A Phase BT — Diagnostics

Expose:

```text
PERSONAL_QUEST_TEMPLATES
PERSONAL_QUESTS_ACTIVE
PERSONAL_QUESTS_COMPLETED
PERSONAL_QUESTS_FAILED
PERSONAL_QUESTS_ABANDONED
PERSONAL_QUESTS_GENERATED
PERSONAL_QUEST_TRIGGER_EVENTS
CHARACTER_ARCS_ACTIVE
CHARACTER_ARCS_RESOLVED
PERSONAL_QUEST_REQUIRED_PORTS_MISSING
```

---

# 79. 200A Tests

- quest type vocabulary,
- arc type vocabulary,
- deterministic generation,
- active cap,
- no eligible template,
- trigger deduplication,
- survivor-specific binding,
- old-save no-active behavior,
- no duplicate generic quest runtime,
- consequence port validation,
- survivor death closure,
- branch persistence.

---

# 80. 200A Definition of Done

- [ ] PersonalQuestSystem,
- [ ] Plan 134/shared-runtime integration,
- [ ] 8 quest types,
- [ ] 6 arc types,
- [ ] arc themes,
- [ ] typed triggers,
- [ ] typed stages,
- [ ] 3–7 stage validation,
- [ ] branch model,
- [ ] deterministic quest generation,
- [ ] meaningful eligibility,
- [ ] active cap,
- [ ] cooldown/repeat policy,
- [ ] milestone-derived arcs,
- [ ] canonical consequence intents,
- [ ] Plan 132/147/150/179 boundaries,
- [ ] save/old-save,
- [ ] event-driven trigger queue,
- [ ] composition-root integration,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

---

# 81. Workstream 200B — Quest Generation / Stages / Branching / Arcs / UI

## Goal

Implement the eight quest families and six arc families as authored, survivor-specific narrative structures with real gameplay conditions, meaningful branching, clear progression, and bounded notifications.

---

# 82. 200B Phase A — `personal_quest_templates.json`

Path:

```text
Assets/StreamingAssets/Data/personal_quest_templates.json
```

Source requires:

```text
20+ templates
```

Recommended initial target:

```text
24 templates
```

= 3 per source quest type.

This yields broad coverage without authoring an unmanageable corpus.

---

# 83. 200B Phase B — Template Distribution

Initial target:

```text
Redemption: 3
Growth: 3
Discovery: 3
Revenge: 3
Protection: 3
Legacy: 3
Love: 3
Mastery: 3
```

---

# 84. 200B Phase C — Template Quality Rule

Every template must answer:

```text
Why this survivor?
Why now?
What real fact triggered it?
What meaningful actions can resolve it?
What changes if it succeeds/fails?
```

If these cannot be answered, template is too generic.

---

# 85. 200B Phase D — Redemption Template 1: The One Left Behind

Eligibility:

- survivor experienced failed rescue/abandonment,
- guilt or negative relation consequence,
- a related survivor/community context remains.

Possible stages:

1. acknowledge event,
2. identify way to make amends,
3. perform protective/rescue action,
4. face affected survivor/community,
5. resolve forgiveness/self-forgiveness.

No guaranteed forgiveness.

---

# 86. 200B Phase E — Redemption Template 2: The Broken Promise

Eligibility:

- recorded promise/commitment failed,
- target still meaningful.

Branches:

```text
repair
admit failure
avoid responsibility
```

---

# 87. 200B Phase F — Redemption Template 3: Blood on the Ledger

Eligibility:

- harmful moral choice,
- survivor participated/benefited,
- guilt/ethical conflict.

Outcome can be:

- redemption,
- rationalization,
- corruption.

---

# 88. 200B Phase G — Growth Template 1: Learn to Mend

Eligibility:

- low repair/medical/craft skill,
- repeated relevant failure or desire,
- mentor/resource available.

Stages:

- seek mentor,
- practice,
- perform real task,
- final independent task.

---

# 89. 200B Phase H — Growth Template 2: Speak Up

Eligibility:

- social/leadership limitation,
- event requiring public decision.

Stages:

- prepare,
- support someone,
- address group,
- accept consequence.

---

# 90. 200B Phase I — Growth Template 3: Face the Place

Eligibility:

- fear/negative memory tied to location,
- location still reachable.

Stages:

- discuss/prepare,
- revisit,
- perform meaningful task,
- process outcome.

No magical trauma cure.

---

# 91. 200B Phase J — Discovery Template 1: The Mark on the Map

Eligibility:

- clue/rumor,
- curious/investigative trait,
- undiscovered canonical location.

Stages:

- collect evidence,
- triangulate,
- expedition,
- resolve discovery.

---

# 92. 200B Phase K — Discovery Template 2: Whose Name Was This?

Eligibility:

- artifact/keepsake,
- unresolved identity/history reference.

Can integrate archive/journal.

---

# 93. 200B Phase L — Discovery Template 3: The Hidden Room

Eligibility:

- real shelter/tunnel/location clue.

No creation of nonexistent secret room.

---

# 94. 200B Phase M — Revenge Template 1: A Debt in Blood

Eligibility:

- grievance against real surviving target/faction,
- target known.

Branches:

- revenge,
- confrontation,
- restraint,
- forgiveness.

This can feed corruption/redemption/hero arcs.

---

# 95. 200B Phase N — Revenge Template 2: The Raider's Name

Eligibility:

- raid caused personal loss,
- identified faction/leader or authored target.

If no individual target exists:

- target faction action instead.

---

# 96. 200B Phase O — Revenge Template 3: Justice or Vengeance

Eligibility:

- moral/legal conflict,
- governance or faction justice path available.

Strong branching.

---

# 97. 200B Phase P — Protection Template 1: Keep Them Safe

Eligibility:

- strong relation,
- real threat to target.

Stages:

- identify threat,
- prepare,
- intervene,
- resolve.

---

# 98. 200B Phase Q — Protection Template 2: The Child's Future

Eligibility:

- parent/guardian relation,
- child/young survivor,
- Plan 150/154 family/education context.

Outcome may include:

- education,
- shelter safety,
- legacy.

---

# 99. 200B Phase R — Protection Template 3: Hold the Door

Eligibility:

- defense/emergency context.

May branch:

- self-sacrifice,
- tactical retreat,
- ask for help.

Do not force death.

---

# 100. 200B Phase S — Legacy Template 1: Teach What I Know

Eligibility:

- high skill,
- apprentice/learner candidate.

Uses Apprenticeship/Education systems.

---

# 101. 200B Phase T — Legacy Template 2: Leave a Record

Eligibility:

- experienced survivor,
- archive/journal capability.

Output:

- memoir/archive project,
- doctrine,
- tutorial record.

---

# 102. 200B Phase U — Legacy Template 3: Build Something That Lasts

Eligibility:

- high craft/engineering/leadership,
- real build/project available.

---

# 103. 200B Phase V — Love Template 1: Say What Matters

Eligibility:

- Plan 150 mutual romantic eligibility/interest.

Stages may include:

- private conversation,
- supportive action,
- relationship choice.

No one-sided forced romance.

---

# 104. 200B Phase W — Love Template 2: Through the Winter

Eligibility:

- established partnership,
- shared hardship.

Arc focuses on commitment/protection, not dating meter.

---

# 105. 200B Phase X — Love Template 3: Let Go or Hold On

Eligibility:

- partnership strain/separation/conflict.

Possible outcomes:

- repair,
- respectful separation,
- unresolved.

---

# 106. 200B Phase Y — Mastery Template 1: The Final Lesson

Eligibility:

- near skill mastery,
- final challenge.

Uses canonical skill milestone.

---

# 107. 200B Phase Z — Mastery Template 2: Better Than the Teacher

Eligibility:

- mentor relationship,
- high skill.

Can branch into:

- gratitude,
- rivalry,
- mentorship succession.

---

# 108. 200B Phase AA — Mastery Template 3: Prove It Under Pressure

Eligibility:

- high skill,
- real high-risk task.

Completion requires actual gameplay use.

---

# 109. 200B Phase AB — Stage Progression

Prefer event subscription.

Example:

```text
stage condition = VisitLocation(loc_x)

LocationVisited event
→ condition evaluator
→ stage complete
```

No daily full scan.

---

# 110. 200B Phase AC — Daily Reconciliation

Once/day fallback checks:

- time-based stages,
- missed events after load,
- impossible-state detection.

Bounded to active personal quests.

---

# 111. 200B Phase AD — Condition Evaluator Registry

Create:

```text
IPersonalQuestConditionEvaluator
```

Implement by typed condition.

No giant string switch spread across systems.

---

# 112. 200B Phase AE — Branch Choice UI

At branch:

- show survivor framing,
- available options,
- known costs/consequences,
- unavailable options with reason if appropriate.

Do not spoil hidden final outcome.

---

# 113. 200B Phase AF — Survivor-Autonomous Choices

Some branches can be survivor-driven.

Use:

- traits,
- psychology,
- relationships,
- hidden agenda if ethically/narratively valid.

Persist selected branch.

---

# 114. 200B Phase AG — Player Agency vs Survivor Agency

Document each template branch:

```text
PlayerChoice
SurvivorChoice
JointChoice
WorldOutcome
```

Prevents inconsistent agency.

---

# 115. 200B Phase AH — Quest Completion

When terminal success state reached:

1. shared runtime commits complete,
2. PersonalQuestSystem records history,
3. consequence profiles apply once,
4. arc milestones resolve,
5. journal/archive events emit,
6. active slot frees.

---

# 116. 200B Phase AI — Quest Failure

When authored failure condition reached:

1. shared runtime commits failed,
2. consequences apply once,
3. arc milestone resolves,
4. history records reason,
5. follow-on quest eligibility may change.

---

# 117. 200B Phase AJ — Quest Abandonment

Explicit action:

- confirmation,
- consequence preview,
- idempotent closure.

No accidental one-click abandonment.

---

# 118. 200B Phase AK — Failure Transformations

Some quests should transform instead of fail.

Example:

```text
revenge target dies
→ "What Now?" redemption/growth branch
```

Use authored continuation.

---

# 119. 200B Phase AL — Quest Chaining

A completion can emit:

```text
personal_quest_follow_up_eligible
```

but should not auto-create immediately if active cap/cooldown blocks it.

---

# 120. 200B Phase AM — Arc Family: Hero

Milestones:

- accepts responsibility,
- protects others,
- succeeds under pressure,
- leads at cost.

Avoid “hero = always positive stats.”

---

# 121. 200B Phase AN — Arc Family: Tragic

Milestones:

- flaw reinforced,
- warning ignored,
- relationship lost,
- final downfall/partial realization.

Do not predetermine failure at arc start.

---

# 122. 200B Phase AO — Arc Family: Redemption

Milestones:

- acknowledges harm,
- makes restitution,
- chooses restraint,
- receives or does not receive forgiveness,
- changes behavior.

Positive outcome should not require victim forgiveness.

---

# 123. 200B Phase AP — Arc Family: Wisdom

Milestones:

- learns from failure,
- mentors another,
- chooses long-term good,
- records knowledge.

---

# 124. 200B Phase AQ — Arc Family: Corruption

Milestones:

- chooses expedience/power,
- exploits relationship,
- justifies harm,
- achieves goal at ethical cost.

No automatic villain transformation from success.

---

# 125. 200B Phase AR — Arc Family: Growth

Milestones:

- confronts limitation,
- practices,
- changes behavior,
- demonstrates competence.

---

# 126. 200B Phase AS — Arc Outcome Calculation

Use weighted milestone profile.

Example:

```text
positive milestones
negative milestones
unresolved milestones
terminal branch
```

→ final:

```text
Positive / Neutral / Negative / Mixed
```

---

# 127. 200B Phase AT — No Generic Progress Farming

Completing random routine quests cannot push arc progress.

Only quest/branch milestones explicitly mapped to arc.

---

# 128. 200B Phase AU — Arc Stage Names

Arc phases might be:

```text
Inciting
Struggle
TurningPoint
Resolution
Aftermath
```

Use data definitions.

Not every arc needs same count.

---

# 129. 200B Phase AV — Personal Quest UI: Survivor Detail

Show:

- active personal quests,
- current arc,
- recent narrative milestone.

Keep concise.

---

# 130. 200B Phase AW — Quest Panel

Show:

- quest title,
- survivor,
- why it began,
- stage list,
- current stage,
- known completion condition,
- relevant people/items/locations,
- branch state.

---

# 131. 200B Phase AX — “Why This Quest?” Explainability

Add:

```text
Triggered by:
- survived the refinery fire
- relationship with Mara became Close
- guilt from abandoning team
```

Only player-known facts.

This is essential for avoiding arbitrariness.

---

# 132. 200B Phase AY — Arc Panel

Show:

- arc type/theme,
- current narrative phase,
- completed milestones,
- unresolved turning point.

Do not expose exact hidden negative outcomes.

---

# 133. 200B Phase AZ — Quest History

Filter:

- survivor,
- quest type,
- result,
- arc,
- day.

Plan 55 retention applies.

---

# 134. 200B Phase BA — Notifications

Notify:

- quest starts,
- meaningful stage completion,
- branch decision available,
- completion/failure.

Do not notify every internal condition update.

---

# 135. 200B Phase BB — Tutorial

First personal quest explains:

- why it appeared,
- stages,
- branching,
- consequences,
- survivor-specific nature.

---

# 136. 200B Phase BC — Tooltips

Source asks hover tooltips.

Plan 37 accessibility requires keyboard/focus access too.

Tooltip/detail view reachable by:

- hover,
- focus,
- explicit details action.

---

# 137. 200B Phase BD — Journal Integration

Journal logs:

- quest start,
- major stage,
- branch,
- completion/failure.

Do not duplicate full quest panel prose.

---

# 138. 200B Phase BE — Archive Integration

Plan 162 may record only landmark personal stories:

- famous hero arc,
- redemption,
- legacy project,
- tragic downfall.

No automatic archive entry for every stage.

---

# 139. 200B Phase BF — Quest Events

Source events:

```text
The Quest Begins
The Stage
The Choice
The Completion
The Failure
The Arc
The Outcome
The Legacy
```

Treat as semantic/narrative events, not separate quest systems.

---

# 140. 200B Phase BG — Quest Hooks / Meta Challenges

Source hooks:

```text
The Hero
The Mentor
The Storyteller
The Legacy
The Romance
The Mastery
The Redemption
```

Important ownership rule:

If Plan 149 owns achievements:

- these are achievement criteria there,
- PersonalQuestSystem only emits facts.

Do not create a second achievement engine.

---

# 141. 200B Phase BH — “The Hero”

Source:

```text
complete 5 hero arc quests
```

Define exactly:

- 5 personal quests associated with Hero arc milestones,
- not five full separate hero arcs unless intended.

---

# 142. 200B Phase BI — “The Mentor”

Guide 3 survivors through growth arcs.

Requires real mentor relation/teaching context.

---

# 143. 200B Phase BJ — “The Storyteller”

Witness 10 quest completions.

If player-facing meta achievement exists, Plan 149 owns it.

---

# 144. 200B Phase BK — “The Legacy”

Complete one legacy quest.

---

# 145. 200B Phase BL — “The Romance”

Source says:

```text
complete love quest with 3 survivors
```

Interpret as:

- three survivor-specific love quests successfully completed over campaign,
- not necessarily simultaneous relationships.

Plan 150 consent/eligibility remains authoritative.

---

# 146. 200B Phase BM — “The Mastery”

Complete mastery quest in 5 skills.

Use canonical skill IDs.

---

# 147. 200B Phase BN — “The Redemption”

Complete redemption quest for 3 survivors.

---

# 148. 200B Phase BO — Localization

All system-authored quest text uses keys and parameterized context.

Survivor names/items/locations resolve dynamically.

---

# 149. 200B Phase BP — No Fake Biography

Templates may not invent:

- spouse,
- child,
- hometown,
- past crime,
- military service,
- faith,
- profession

unless canonical survivor data supports it.

---

# 150. 200B Phase BQ — Missing Context Fallback

If a template cannot bind all required context:

```text
template ineligible
```

Do not fill missing person/location with generic placeholder.

---

# 151. 200B Phase BR — Content Validation Matrix

For every template:

| Template | Type | Stages | Trigger domains | Required refs | Branches | Arc mapping | Runtime observed |
|---|---|---:|---|---|---:|---|---:|

---

# 152. 200B Phase BS — Arc Coverage Matrix

| Arc Type | Templates contributing | Positive milestones | Negative milestones | Terminal outcomes | Runtime observed |
|---|---:|---|---|---|---:|

---

# 153. 200B Phase BT — Trigger Coverage Matrix

| Trigger Type | Producers | Templates | Runtime observed | Dead producers |
|---|---|---:|---:|---:|

All seven source trigger types must be exercised.

---

# 154. 200B Phase BU — Content Utilization

Run targeted fixtures and long soak.

Report:

```text
templates loaded
templates eligible
quests generated
quests completed
quests failed
quests abandoned
branches chosen
arc milestones
arc resolutions
never eligible templates
```

---

# 155. 200B Phase BV — Dead Template Policy

Never-observed template:

- fix trigger,
- mark intentionally rare,
- remove,
- exempt with reason.

---

# 156. 200B Phase BW — Narrative Similarity Audit

Check templates for:

- same trigger,
- same stages,
- same consequences,
- only different prose.

Merge or differentiate.

---

# 157. 200B Phase BX — Quest Spam Budget

Measure:

```text
new personal quests / survivor / 100 days
```

Target should feel rare enough to matter.

Do not set numerical target until soak evidence.

---

# 158. 200B Phase BY — Global Attention Budget

Even if many survivors qualify, cap simultaneous notifications.

Quest generation can queue nonurgent announcements.

---

# 159. 200B Definition of Done

- [ ] 20+ templates,
- [ ] recommended 24-template balanced set,
- [ ] all 8 quest types,
- [ ] all 7 trigger types,
- [ ] 3–7 stages each,
- [ ] branching,
- [ ] completion/failure/abandonment,
- [ ] quest chaining,
- [ ] 6 arc families,
- [ ] milestone-derived outcomes,
- [ ] survivor detail integration,
- [ ] quest panel,
- [ ] arc panel,
- [ ] history,
- [ ] explainability,
- [ ] notifications,
- [ ] tutorial/tooltips,
- [ ] journal/archive,
- [ ] source quest events/hooks,
- [ ] localization,
- [ ] no fake biography,
- [ ] content/arc/trigger coverage matrices,
- [ ] utilization report,
- [ ] spam budget.

---

# 160. Workstream 200C — Relations Integration

## Goal

Make relationship-driven personal quests consume real relationship history and apply consequences through the relation authority.

---

# 161. 200C Phase A — Relationship Milestone Events

Use:

- band crossed,
- conflict milestone,
- reconciliation,
- family change,
- mentor relation,
- romance interest/partnership.

No raw per-frame affinity scans.

---

# 162. 200C Phase B — Protection Quests

Target survivor must:

- exist,
- be related strongly enough,
- face real threat/need.

---

# 163. 200C Phase C — Love Quests

Plan 150 owns:

- mutual eligibility,
- courtship,
- partnership,
- separation.

Personal quest stages reference those state transitions.

---

# 164. 200C Phase D — Revenge / Grudge

If relation system records:

- betrayal,
- harm,
- grudge,

use cause IDs.

No invented grievance.

---

# 165. 200C Phase E — Relationship Rewards

Use reason-coded canonical API.

Cap repeated boosts.

---

# 166. 200C Phase F — Relationship Damage

Failure/abandonment can affect relation only where the other survivor is actually involved.

---

# 167. 200C Phase G — Target Death

If related survivor dies:

- update quest path,
- maybe transform protection → grief/redemption/legacy,
- do not leave impossible stage.

---

# 168. Workstream 200C — Skill Integration

## Goal

Make growth/mastery/legacy quests use real skills and real teaching without duplicate XP.

---

# 169. 200C Phase H — Skill Trigger

Listen to canonical skill milestone.

---

# 170. 200C Phase I — Skill Condition

Stage evaluator:

```text
skill ID
threshold/tier
```

---

# 171. 200C Phase J — Skill Reward

Award XP/skill unlock via SkillProgressionSystem.

---

# 172. 200C Phase K — No Permanent Skill Penalty by Default

Failure should usually:

- lose opportunity,
- reduce confidence,
- delay growth.

Permanent competence regression requires explicit skill-system support.

---

# 173. 200C Phase L — Teaching / Mentor

Legacy/wisdom quests can use apprenticeship/education systems.

---

# 174. Workstream 200C — Trait Integration

## Goal

Use traits as narrative identity and trigger context without turning quests into a second trait engine.

---

# 175. 200C Phase M — Trait Activation Event

Prefer real activation context.

Example:

```text
cowardly trait activated during raid
→ potential courage/growth quest
```

---

# 176. 200C Phase N — Trait Transformation

If quest outcome changes trait:

- use approved transformation table,
- canonical TraitSystem applies.

---

# 177. 200C Phase O — Trait Contradiction

A quest can challenge a trait.

Do not remove trait just because quest succeeded once unless threshold/milestone criteria support it.

---

# 178. Workstream 200C — Event Integration

## Goal

Use significant lived events as the primary source of narrative causality.

---

# 179. 200C Phase P — Event Salience

Only significant events should trigger personal quest generation.

Use:

- semantic kind,
- survivor participation,
- severity,
- relation impact,
- moral consequence.

---

# 180. 200C Phase Q — Event-to-Quest Mapping

Examples:

```text
survivor_saved_other
→ Hero / Protection / Growth

survivor_abandoned_other
→ Redemption / Corruption

survivor_betrayed
→ Revenge / Growth

major_discovery
→ Discovery / Legacy

survived_disaster
→ Hero / Wisdom
```

---

# 181. 200C Phase R — No Global Event Spam

A shelter-wide event may affect 20 survivors.

Do not automatically generate 20 personal quests.

Score:

- direct participation,
- emotional salience,
- identity fit.

Cap generation per event.

---

# 182. Workstream 200C — Item Integration

## Goal

Use meaningful possessions as quest anchors without inventing or duplicating inventory.

---

# 183. 200C Phase S — Item Trigger

Real acquisition event.

---

# 184. 200C Phase T — Item Binding

Store stable item definition ID plus unique instance ID if item instances matter.

---

# 185. 200C Phase U — Item Loss

If required item is lost:

- fail,
- branch,
- allow replacement

according to template.

---

# 186. 200C Phase V — Legacy Artifact

Quest may craft/create an artifact through canonical crafting/item system.

No personal-quest-owned fake item.

---

# 187. Workstream 200C — Location Integration

## Goal

Make discovery/revisit/return quests use real world knowledge and travel.

---

# 188. 200C Phase W — Location Visited Trigger

Use canonical visit event.

---

# 189. 200C Phase X — Location Access

Quest should not reveal exact hidden destination unless character has canonical knowledge/clue.

---

# 190. 200C Phase Y — Location Lost/Destroyed

LocationEvolution may invalidate objective.

Quest transforms/fails according to template.

---

# 191. 200C Phase Z — Return Home Quest

“Return home” requires a canonically authored origin/home reference.

Do not invent hometown.

---

# 192. Workstream 200C — Memory Integration

## Goal

Use Plan 147 memory as a narrative substrate without becoming its owner.

---

# 193. 200C Phase AA — Memory Trigger

Memory system can expose:

- high-salience unresolved memory,
- repeated remembered event,
- relationship-linked memory.

---

# 194. 200C Phase AB — Memory Decay / Plan 185

If a triggering memory decays:

- quest remains if already instantiated because its historical source occurred,
- future triggers may be affected.

Do not erase an active quest because memory record became less salient unless template explicitly models forgetting.

---

# 195. 200C Phase AC — Quest Outcome Memory

Quest completion can emit:

```text
significant personal event
```

for memory system.

Memory owner decides retention/salience.

---

# 196. Workstream 200C — Psychology Integration

## Goal

Let personal quests react to and influence psychology without becoming a second psychological model.

---

# 197. 200C Phase AD — Psychology Eligibility

Plan 179 can influence:

- arc fit,
- autonomous branch weighting,
- quest readiness,
- response to success/failure.

---

# 198. 200C Phase AE — No Diagnosis Gate Unless Canonical

Do not require invented:

```text
depression > 50
```

Use actual psychological profile bands/traits.

---

# 199. 200C Phase AF — Arc → Psychology

Source says character arc affects psychology.

Translate to explicit milestone consequence.

Examples:

- successful redemption may reduce guilt context,
- tragic betrayal may increase distrust,
- wisdom arc may increase confidence/advisor identity.

Canonical psychology applies.

---

# 200. 200C Phase AG — Psychology → Arc

Psychology can influence branch tendency.

But player choice remains respected where branch is player-owned.

---

# 201. Workstream 200C — Hidden Agenda Integration

## Goal

Allow secret motivations to influence personal narratives without accidentally revealing them.

---

# 202. 200C Phase AH — Agenda Eligibility

A hidden agenda may silently increase template weight.

Example:

```text
protection agenda
→ stronger protection/legacy eligibility
```

---

# 203. 200C Phase AI — No Secret Leakage

Quest text cannot reveal:

```text
"Because this survivor is secretly a spy..."
```

unless the agenda is discovered.

---

# 204. 200C Phase AJ — Conflict

Open personal quest may conflict with hidden agenda.

Use this for authored branch pressure, not random sabotage.

---

# 205. Workstream 200C — Shared Quest Runtime Integration

## Goal

Ensure there is one generic quest lifecycle in the repository.

---

# 206. 200C Phase AK — Quest Instance Creation

PersonalQuestSystem sends:

```text
QuestDefinition/Template
+ bound parameters
+ survivor owner metadata
```

to shared runtime.

---

# 207. 200C Phase AL — Stage Events

Shared runtime emits:

```text
stage complete
branch selected
quest complete
quest fail
```

PersonalQuestSystem consumes to update arc.

---

# 208. 200C Phase AM — Save Ownership

Generic runtime saves:

- stage,
- status,
- branch.

PersonalQuestSystem saves:

- personal binding,
- arc metadata,
- generation history.

---

# 209. 200C Phase AN — No Duplicate Runtime Test

Static scan:

- no second generic quest scheduler,
- no duplicate stage-tick engine,
- no duplicate generic quest persistence.

---

# 210. Workstream 200C — Consequence Delivery

## Goal

Use typed consequence ports and idempotency for every personal-quest outcome.

---

# 211. 200C Phase AO — Consequence Key

Key:

```text
quest_instance
stage/terminal
consequence_profile
target
```

Apply once.

---

# 212. 200C Phase AP — Completion Consequences

Possible:

- XP,
- trait milestone,
- relationship reason,
- morale effect,
- psychology effect,
- item/project reward,
- new quest eligibility.

---

# 213. 200C Phase AQ — Failure Consequences

Possible:

- morale,
- relationship damage,
- psychology,
- missed opportunity,
- arc negative milestone.

Avoid gratuitous stat punishment.

---

# 214. 200C Phase AR — Abandonment Consequences

Only if authored and explained.

---

# 215. 200C Phase AS — Branch Consequences

Apply immediately only if branch action itself has consequence.

Terminal consequences wait for resolution.

---

# 216. Workstream 200C — Save / Load / Migration

## Goal

Prove personal stories survive exact lifecycle points without reroll or duplicate consequences.

---

# 217. 200C Phase AT — Save Matrix

Test:

```text
trigger queued
quest generated
stage 1 active
stage complete pending advance
branch choice pending
branch selected
terminal success pending consequence
completed
failed
abandoned
arc mid-progress
arc resolved
```

---

# 218. 200C Phase AU — Trigger Reroll Prevention

Once a trigger has selected a quest template:

- persist selection,
- reload cannot choose another template.

---

# 219. 200C Phase AV — Branch Reroll Prevention

Persist branch.

No reload reroll for survivor-autonomous choices.

---

# 220. 200C Phase AW — Consequence Duplication Prevention

Reload at terminal transition:

- no duplicate XP,
- no duplicate trait change,
- no duplicate relationship effect,
- no duplicate item.

---

# 221. 200C Phase AX — Old Save

Source exact policy:

```text
no active personal quests
```

Pass.

---

# 222. 200C Phase AY — No Retroactive Flood

After migration, old historical events do not generate hundreds of quests.

Only new trigger events count.

---

# 223. Workstream 200C — Exploit Prevention

## Goal

Prevent personal quests from becoming farmable deterministic reward loops.

---

# 224. 200C Phase AZ — Trigger Farming

Repeatedly crossing a relationship threshold:

```text
Friendly ↔ Neutral ↔ Friendly
```

must not create repeated same quest.

Use milestone event history.

---

# 225. 200C Phase BA — Item Farming

Acquire/drop/reacquire same item:

- no repeated once-per-survivor quest.

---

# 226. 200C Phase BB — Location Farming

Repeated visits:

- no repeated discovery quest unless repeat policy allows.

---

# 227. 200C Phase BC — Skill Farming

Level threshold emits milestone once.

No level down/up loop.

---

# 228. 200C Phase BD — Time Trigger Farming

Save/load cannot reset elapsed-day anchor.

---

# 229. 200C Phase BE — Abandon/Regenerate Exploit

Player cannot:

```text
abandon unwanted quest
→ reroll instantly
```

Use cooldown/template history.

---

# 230. 200C Phase BF — Reward Farming

Repeatable templates have diminishing/no repeat reward as designed.

---

# 231. 200C Phase BG — Love Quest Farming

Cannot farm relation reward by cycling partnership states.

Plan 150 relationship history prevents.

---

# 232. 200C Phase BH — Mastery Reward Farming

Completing mastery quest once per skill/survivor unless explicitly repeatable.

---

# 233. Workstream 200C — Edge Cases

## Goal

Ensure narrative system remains valid in sparse, dense, tragic, and mechanically unusual campaigns.

---

# 234. 200C Phase BI — No Quests

Current behavior remains valid.

A survivor may never receive a personal quest.

No errors.

---

# 235. 200C Phase BJ — Many Quests

Stress fixture:

- many survivors,
- many triggers.

Enforce:

- per-survivor active cap,
- global notification budget,
- bounded evaluation cost.

---

# 236. 200C Phase BK — Survivor Dies During Quest

Close deterministically.

Arc outcome may become:

- tragic,
- mixed,
- unresolved,

according to data.

---

# 237. 200C Phase BL — Target Survivor Dies

Branch/transform/fail.

Never leave impossible active stage forever.

---

# 238. 200C Phase BM — Item Destroyed

Branch/fail/replace.

---

# 239. 200C Phase BN — Location Becomes Unreachable

Pause/transform/fail.

Use canonical route state.

---

# 240. 200C Phase BO — Skill Requirement Regresses

If canonical system allows temporary skill impairment:

- distinguish base mastery from temporary performance.

Do not fail permanent mastery quest due to fatigue.

---

# 241. 200C Phase BP — Relationship Changes Mid-Quest

Stage may:

- adapt,
- branch,
- fail.

Use template rules.

---

# 242. 200C Phase BQ — Psychology Crisis Mid-Quest

Quest can pause/branch if survivor incapacitated.

No invisible progress.

---

# 243. 200C Phase BR — Hidden Agenda Revealed Mid-Quest

May unlock new branch/interpretation.

Do not rewrite prior history.

---

# 244. Workstream 200C — Determinism

## Goal

Guarantee reproducible personal narratives when source state and choices match.

---

# 245. 200C Phase BS — RNG Stream

Dedicated:

```text
personal_quests
```

No `Random.Shared`, `DateTime.UtcNow`, hash-order dependence.

---

# 246. 200C Phase BT — Candidate Ordering

Sort by stable IDs:

- templates,
- candidate survivors,
- target survivors,
- items,
- locations.

---

# 247. 200C Phase BU — Same-History Digest

Build canonical digest of relevant trigger history.

Same history + seed gives same generated quest.

---

# 248. 200C Phase BV — Autonomous Branch Determinism

Survivor-autonomous branch selection uses stable inputs and persisted decision.

---

# 249. Workstream 200C — Data Integrity

## Goal

Fail CI when personal quest content references nonexistent or impossible game facts.

---

# 250. 200C Phase BW — Template Schema Validation

Validate:

- unique IDs,
- 3–7 stages,
- valid quest type,
- valid arc mappings,
- valid condition types,
- valid branches,
- no cycles unless explicitly allowed.

---

# 251. 200C Phase BX — Reference Validation

Validate:

- survivor archetype refs if used,
- skill IDs,
- trait IDs,
- relationship bands,
- semantic event kinds,
- item IDs/tags,
- location IDs,
- psychology profile/band IDs,
- consequence profiles,
- localization keys.

---

# 252. 200C Phase BY — Branch Reachability

Every branch target stage exists.

No dead-end unless terminal.

---

# 253. 200C Phase BZ — Stage Reachability

Every nonterminal stage reachable from start.

---

# 254. 200C Phase CA — Contradictory Condition Detection

Examples:

```text
requires target alive
+
failure if target alive
```

should fail validation.

---

# 255. 200C Phase CB — Self-Reference

Quest cannot require its own completion as trigger unless explicit loop-safe follow-up system.

---

# 256. 200C Phase CC — Arc Mapping Integrity

Every quest arc milestone references valid arc type/theme.

---

# 257. 200C Phase CD — Data Utilization

Targeted test fixture should exercise all eight quest types.

Long soak identifies unreachable templates.

---

# 258. Workstream 200C — `--personal-quest-selftest`

Required scenarios:

1. deterministic redemption quest,
2. growth quest,
3. discovery quest,
4. revenge quest,
5. protection quest,
6. legacy quest,
7. love quest,
8. mastery quest,
9. relationship trigger,
10. skill trigger,
11. event trigger,
12. item trigger,
13. location trigger,
14. trait trigger,
15. time-passed trigger,
16. stage progression,
17. branch choice,
18. completion,
19. failure,
20. abandonment,
21. arc milestone,
22. arc resolution,
23. save/load mid-stage,
24. save/load at branch,
25. terminal idempotency,
26. survivor death,
27. target death,
28. old save,
29. no-quest case,
30. active-cap enforcement.

---

# 259. Workstream 200C — Deliberate Failure Proof

Break:

- missing skill ID,
- invalid location,
- invalid relationship band,
- branch target stage missing,
- duplicate template ID,
- 2-stage quest,
- 8-stage quest,
- invalid consequence sink,
- duplicate terminal reward key.

Assert relevant gate fails.

---

# 260. Workstream 200C — Long-Run Balance

## Goal

Prove personal quests remain meaningful and sparse rather than turning the game into an endless checklist.

---

# 261. 200C Phase CE — 200-Day Narrative Soak

Use 10–20 survivors.

Record:

```text
trigger events
quests generated
quests/survivor
completions
failures
abandonments
branch choices
arc starts
arc resolutions
notification count
```

---

# 262. 200C Phase CF — 400-Day / Multi-Generation Soak

If children/long campaign present:

- young survivor growth arc,
- adult mastery/legacy arc,
- family/love arcs,
- old survivor legacy/wisdom.

Measure retention.

---

# 263. 200C Phase CG — Narrative Density Budget

Goal:

- each personal quest feels significant,
- not every survivor has 2 active quests continuously.

Use:

```text
meaningful trigger threshold
+ cooldown
+ active cap
+ global attention budget
```

---

# 264. 200C Phase CH — Quest Type Distribution

Track percentage by type.

If one type dominates due to common trigger:

- tune eligibility/weight,
- not random flattening.

---

# 265. 200C Phase CI — Arc Distribution

Ensure:

- Hero/Growth do not dominate all survivors,
- Tragic/Corruption remain possible but not arbitrary,
- Redemption appears only after real mistakes.

---

# 266. 200C Phase CJ — Completion Rate

Too high:

- quests trivial.

Too low:

- goals unrealistic.

Balance by type.

---

# 267. 200C Phase CK — Failure Fairness

For each failed quest:

```text
was failure condition visible?
could player/survivor plausibly respond?
was target invalidated by canonical world state?
```

No hidden arbitrary failure.

---

# 268. 200C Phase CL — Reward Balance

Compare:

- XP,
- traits,
- relation,
- morale,
- project/item rewards.

Personal quests should not outcompete ordinary systems.

---

# 269. 200C Phase CM — Failure Penalty Balance

Avoid making failure so punitive players reject personal quests.

Narrative consequence > raw stat punishment.

---

# 270. 200C Phase CN — Abandonment Balance

Abandonment should matter but remain valid.

No irreversible catastrophe for declining a personal storyline unless explicitly authored.

---

# 271. 200C Phase CO — Active Cap Balance

Default 2.

Test:

```text
1
2
3
```

with UI and narrative density.

---

# 272. 200C Phase CP — Auto-Generate Toggle

Source proposes:

```text
auto-generate quests bool
```

Interpret carefully.

If disabled:

- player may manually accept eligible personal opportunities,
- system should still detect eligibility.

Do not require player to invent arbitrary quests.

---

# 273. 200C Phase CQ — Manual Assignment

Source allows manual assignment.

Recommended meaning:

```text
player chooses among currently eligible personal-quest opportunities
```

not:

```text
assign revenge quest to anyone
```

Preserve narrative validity.

---

# 274. Workstream 200C — UI / Accessibility

## Goal

Ensure personal narratives remain understandable across large text, controller navigation, and cognitive-load modes.

---

# 275. 200C Phase CR — Large Text

Quest/arc panels must survive 2× font under Plan 184.

---

# 276. 200C Phase CS — Keyboard / Controller

Every stage/branch action focusable under Plan 37.

---

# 277. 200C Phase CT — Cognitive Load Reduction

Reduced-density mode shows:

- quest purpose,
- current stage,
- next action,
- key consequence.

Advanced history/details collapsible.

---

# 278. 200C Phase CU — No Color-Only Arc Outcome

Use labels/icons.

---

# 279. 200C Phase CV — Screen Reader Semantics

Quest stage list announces:

```text
stage name
complete/incomplete
current
branch choice
```

No hidden branch spoilers.

---

# 280. 200C Phase CW — Notification Accessibility

No audio-only personal-quest alert.

---

# 281. Workstream 200C — Retention / Archive / Legacy

## Goal

Keep personal stories remembered without allowing histories to grow unbounded.

---

# 282. 200C Phase CX — Plan 55 Retention

Keep:

- active quest state,
- completed quest summary,
- arc milestones,
- landmark branch choices,
- terminal outcome.

Roll up:

- low-value stage logs,
- repetitive trigger diagnostics.

---

# 283. 200C Phase CY — Journal Retention

Journal holds selected narrative entries.

Quest runtime/history remains source.

---

# 284. 200C Phase CZ — Archive Integration

Plan 162 records only:

- major hero arc,
- tragic downfall,
- redemption,
- legacy,
- famous mastery,
- significant love/family story.

---

# 285. 200C Phase DA — Survivor Biography Projection

Follow-on opportunity from source can be supported later by:

```text
PersonalStoryProjection
```

built from:

- quest history,
- arc milestones,
- canonical memories,
- archive.

Do not create a separate biography truth now.

---

# 286. 200C Phase DB — Campaign Completion

Plan 34 completion record can include:

- landmark personal arcs,
- famous survivor outcomes.

Structured IDs, not parsed prose.

---

# 287. Workstream 200C — Performance

## Goal

Avoid whole-world daily trigger scans.

---

# 288. 200C Phase DC — Event-Driven Evaluation

Quest eligibility evaluates on:

- relationship milestone,
- skill milestone,
- significant event,
- item acquisition,
- location visit,
- trait activation,
- daily time-anchor check.

---

# 289. 200C Phase DD — Active Quest Cost

Daily progression only checks:

```text
active quests
```

not all templates for all survivors.

---

# 290. 200C Phase DE — Template Indexing

Index templates by:

- trigger type,
- quest type,
- required trait/skill if useful.

---

# 291. 200C Phase DF — No Per-Frame Evaluation

Hard gate.

---

# 292. 200C Phase DG — Allocation Budget

Avoid rebuilding dictionaries/lists every stage tick.

Cache static parsed conditions.

---

# 293. Workstream 200C — Human Narrative Review

## Goal

Verify personal quests feel like survivor stories rather than achievements with names.

---

# 294. 200C Phase DH — Review Questions

For every template:

```text
Could this quest fit almost any survivor unchanged?
Does the trigger visibly relate to their history?
Does the branch reveal character?
Does failure create story rather than just penalty?
Does completion alter how we understand the survivor?
```

---

# 295. 200C Phase DI — Swap Test

Take quest generated for Survivor A.

Try to swap Survivor B.

If it still fits perfectly despite different history:

- template may be too generic.

---

# 296. 200C Phase DJ — Causality Review

Player should be able to answer:

```text
Why did this story begin now?
```

---

# 297. 200C Phase DK — Consequence Review

Player should understand:

```text
What changed because this story resolved?
```

---

# 298. 200C Phase DL — Agency Review

Branches should reflect:

- player agency,
- survivor agency,
- world constraint

clearly.

---

# 299. 200C Phase DM — Tone Review

Avoid:

- melodrama on trivial triggers,
- modern therapy language if setting tone conflicts,
- deterministic moral labeling,
- forced romance,
- arbitrary redemption.

---

# 300. Documentation

Create:

```text
docs/systems/PERSONAL_QUESTS_AND_CHARACTER_ARCS.md
```

Include:

- ownership boundaries,
- Plan 134 shared runtime contract,
- trigger model,
- template schema,
- stage/branch semantics,
- arc milestone model,
- consequence delivery,
- save/idempotency,
- retention,
- adding new templates.

---

# 301. Content Authoring Guide

Create:

```text
docs/content/PERSONAL_QUEST_AUTHORING.md
```

Template checklist:

```text
1. identify survivor-specific trigger
2. identify canonical source IDs
3. define 3–7 stages
4. define player/survivor/world agency
5. define branches
6. define canonical consequences
7. define arc milestones
8. define invalidation/failure behavior
9. define localization
10. add targeted test fixture
```

---

# 302. Integrated Personal Narrative Pipeline

```text
canonical survivor event/state
          │
          ▼
 personal trigger event
          │
          ▼
eligibility + narrative-fit scoring
          │
          ▼
deterministic template selection
          │
          ▼
personal binding
          │
          ▼
shared quest runtime
          │
    ┌─────┼──────────┐
    ▼     ▼          ▼
 stage  branch    failure/complete
    │     │          │
    └─────┼──────────┘
          ▼
personal arc milestones
          │
          ▼
canonical consequence sinks
          │
          ▼
journal / archive / biography projection
```

---

# 303. Quest Runtime Authority Contract

Generic lifecycle belongs to shared quest runtime.

PersonalQuestSystem owns personal narrative orchestration only.

---

# 304. Relationship Authority Contract

Personal quests consume relationship facts.

They do not own affinity/family/romance.

---

# 305. Skill Authority Contract

Mastery/growth quests consume skill milestones.

Skills remain `SkillProgressionSystem` truth.

---

# 306. Trait Authority Contract

Traits remain `TraitSystem` truth.

Quest outcomes request trait changes through explicit APIs.

---

# 307. Event Authority Contract

Events remain `EventSystem`/semantic-event truth.

Personal quests react.

---

# 308. Item Authority Contract

Items remain inventory/crafting truth.

Personal quests reference/consume/grant through canonical transactions.

---

# 309. Location Authority Contract

Locations remain world/topology/knowledge truth.

Personal quests cannot create hidden places by string ID alone.

---

# 310. Memory Authority Contract

Plan 147/185 owns memory and decay.

Personal quests reference memory/event facts.

---

# 311. Psychology Authority Contract

Plan 179 owns psychological state.

Arc milestones emit psychology intents.

---

# 312. Hidden Agenda Authority Contract

Plan 132 owns secret motivations.

Personal quests do not reveal them unless discovered.

---

# 313. Romance Authority Contract

Plan 150 owns romantic eligibility/progression.

Love quests narrativize, not manufacture.

---

# 314. Arc Authority Contract

PersonalQuestSystem owns:

- arc type,
- theme,
- narrative milestones,
- arc outcome.

It does not own underlying mechanical stats.

---

# 315. Trigger Authority Contract

Trigger events originate from canonical systems.

PersonalQuestSystem does not poll private fields everywhere.

---

# 316. Stage Contract

Stage progression is driven by typed gameplay facts.

No arbitrary `progress += 1` unless the condition itself is a counted action.

---

# 317. Branch Contract

Branch choice is explicit, typed, and persisted.

No reroll.

---

# 318. Consequence Contract

Every consequence goes through owning system.

No direct private-field mutation.

---

# 319. Save Contract

Persist:

- personal bindings,
- arc milestones,
- trigger-generation history,
- cooldowns,
- personal metadata,
- consequence idempotency.

Shared runtime persists generic quest state.

---

# 320. Old-Save Contract

Old saves:

```text
no active personal quests
no active arcs
no historical backfill
```

until new trigger events occur.

---

# 321. Determinism Contract

Same:

```text
seed
survivor history
canonical state
trigger sequence
player choices
```

→ same personal story.

---

# 322. Attention Contract

No quest generation solely to fill slots.

Personal quests should be sparse and meaningful.

---

# 323. Narrative Truth Contract

Personal quest text may only assert facts supported by bound canonical context.

---

# 324. Failure Contract

Failure follows real world/state invalidation or explicit stage condition.

No arbitrary hidden failure.

---

# 325. Arc Progress Contract

Arc progress is milestone-derived.

No independently farmable percentage.

---

# 326. Retention Contract

Keep terminal personal-story meaning.

Roll up low-level stage history.

---

# 327. Content Acceptance Contract

Every template progresses through:

```text
AUTHORED
→ LOADS
→ TRIGGERABLE
→ SURVIVOR_ELIGIBLE
→ GENERATED
→ STAGE_CONSUMED
→ BRANCH/OUTCOME PRODUCED
→ CANONICAL CONSEQUENCE APPLIED
→ PLAYER_VISIBLE
→ RETAINED/ARCHIVED
```

---

# 328. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| personal quest system duplicates shared quest runtime | High | Critical | Plan 134 adapter-first architecture |
| quests feel arbitrary | High | High | survivor-specific trigger + explainability |
| arc progress becomes generic XP bar | Medium | High | milestone-derived progress |
| quest generation spam | High | High | meaningful triggers + caps/cooldowns |
| rewards bypass canonical systems | Medium | Critical | typed consequence ports |
| love quests force romance | Medium | Critical | Plan 150 mutual eligibility |
| revenge quests invent villains | Medium | High | real grievance/target requirement |
| redemption quest fabricates guilt | Medium | High | concrete prior event |
| hidden agenda leaks through quest text | Medium | High | knowledge-aware rendering |
| save/load duplicates rewards | Medium | Critical | consequence idempotency |
| old saves flood with retroactive quests | High | High | no historical backfill |
| generic templates fit everyone | High | Medium | swap-test/narrative review |
| branch outcomes reroll | Medium | High | persisted branch |
| failure becomes punitive checklist | Medium | Medium | narrative consequence > raw penalty |

---

# 329. Commit Strategy

## 200A — Foundation

### C2[40].1 — baseline + personal-narrative ownership ADR

### C2[40].2 — Plan 134/shared-runtime compatibility adapter

### C2[40].3 — personal binding / arc DTOs

### C2[40].4 — trigger vocabulary / event adapters

### C2[40].5 — stage/condition/branch schema

### C2[40].6 — deterministic generation / active cap / cooldown

### C2[40].7 — arc milestone/outcome model

### C2[40].8 — canonical consequence ports

### C2[40].9 — save/old-save/idempotency

### C2[40].10 — composition/events/diagnostics

### Gate: 200A complete

---

## 200B — Templates / Arcs / UI

### C2[40].11 — redemption templates

### C2[40].12 — growth templates

### C2[40].13 — discovery templates

### C2[40].14 — revenge templates

### C2[40].15 — protection templates

### C2[40].16 — legacy templates

### C2[40].17 — love templates

### C2[40].18 — mastery templates

### C2[40].19 — stage evaluators / branching

### C2[40].20 — six arc families

### C2[40].21 — quest/arc/history UI

### C2[40].22 — tutorial/tooltips/localization

### C2[40].23 — journal/archive/meta hooks

### C2[40].24 — utilization / similarity / spam reports

### Gate: 200B complete

---

## 200C — Integration / Validation

### C2[40].25 — relations / romance integration

### C2[40].26 — skills / traits integration

### C2[40].27 — events / items / locations integration

### C2[40].28 — memory / psychology / hidden-agenda integration

### C2[40].29 — shared runtime / consequence integration

### C2[40].30 — save-load/idempotency matrix

### C2[40].31 — exploit prevention suite

### C2[40].32 — edge-case suite

### C2[40].33 — data-integrity / branch reachability

### C2[40].34 — `--personal-quest-selftest`

### C2[40].35 — deliberate failure proof

### C2[40].36 — 200-day narrative soak

### C2[40].37 — 400-day/multi-generation soak

### C2[40].38 — reward/failure/attention balance

### C2[40].39 — accessibility/performance/retention

### C2[40].40 — narrative playtest/docs/release closure

### Gate: 200C complete

---

# 330. Verification Checklist

Run the source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --personal-quest-selftest
```

Also run repository-canonical equivalents of:

```text
personal-quest template integrity
3–7 stage count gate
branch/stage reachability gate
canonical reference validation
shared-quest-runtime ownership audit
consequence-port contract validation
old-save no-backfill fixture
same-history deterministic quest digest
200-day narrative soak
personal-quest accessibility snapshot
```

---

# 331. `--personal-quest-selftest` Acceptance Matrix

| Scenario | Required Result |
|---|---|
| Redemption trigger | real negative history produces eligible quest |
| Growth trigger | real skill/limitation produces eligible quest |
| Discovery trigger | real clue/location knowledge produces quest |
| Revenge trigger | real grievance + target required |
| Protection trigger | real relationship + threat required |
| Legacy trigger | real high skill/commitment required |
| Love trigger | Plan 150 mutual eligibility required |
| Mastery trigger | real mastery threshold required |
| Relationship milestone | exactly one eligible trigger |
| Skill milestone | exactly one eligible trigger |
| Event experienced | direct survivor participation preferred |
| Item acquired | real inventory event |
| Location visited | canonical visit event |
| Trait activated | contextual trait event |
| Time passed | anchored to real source event |
| Branch | branch persisted |
| Completion | consequences once |
| Failure | failure consequences once |
| Abandonment | explicit + once |
| Arc resolution | milestone-derived |
| Save/load | exact state retained |
| Old save | no active quests |
| No quest | valid |
| Many quests | cap enforced |

---

# 332. Flagship Definition of Done — Foundation

- [ ] `PersonalQuestSystem.cs` exists,
- [ ] shared quest runtime reused,
- [ ] 8 quest types,
- [ ] 6 arc types,
- [ ] arc themes,
- [ ] trigger system,
- [ ] typed stage conditions,
- [ ] branching,
- [ ] deterministic selection,
- [ ] active-cap policy,
- [ ] auto/manual eligible generation,
- [ ] arc milestone model,
- [ ] canonical consequence ports,
- [ ] versioned save state,
- [ ] old-save no-active policy,
- [ ] composition-root registration,
- [ ] semantic events,
- [ ] diagnostics.

---

# 333. Flagship Definition of Done — Content / UI

- [ ] 20+ templates,
- [ ] recommended 24-template set,
- [ ] redemption coverage,
- [ ] growth coverage,
- [ ] discovery coverage,
- [ ] revenge coverage,
- [ ] protection coverage,
- [ ] legacy coverage,
- [ ] love coverage,
- [ ] mastery coverage,
- [ ] 3–7 stages/template,
- [ ] stage progression,
- [ ] failure/invalidation,
- [ ] branching,
- [ ] six arc families,
- [ ] quest panel,
- [ ] survivor detail,
- [ ] arc panel,
- [ ] history,
- [ ] notifications,
- [ ] tutorial,
- [ ] accessible tooltips/details,
- [ ] journal/archive hooks,
- [ ] quest events/hooks,
- [ ] localization,
- [ ] no fake biography,
- [ ] content-utilization report.

---

# 334. Flagship Definition of Done — Integration / Validation

- [ ] SurvivorRelationsSystem,
- [ ] SkillProgressionSystem,
- [ ] TraitSystem,
- [ ] EventSystem,
- [ ] JournalSystem,
- [ ] Plan 132 hidden agendas,
- [ ] Plan 147 memory where available,
- [ ] Plan 150 romance/family,
- [ ] Plan 179 psychology,
- [ ] item/inventory,
- [ ] location/world knowledge,
- [ ] shared quest runtime,
- [ ] save/load lifecycle,
- [ ] trigger/branch/consequence idempotency,
- [ ] exploit prevention,
- [ ] no-quest/many-quest edge cases,
- [ ] survivor/target death,
- [ ] item/location invalidation,
- [ ] deterministic replay,
- [ ] data/reference integrity,
- [ ] deliberate failure fixtures,
- [ ] `--personal-quest-selftest`,
- [ ] 200-day soak,
- [ ] multi-generation soak,
- [ ] reward/failure balance,
- [ ] attention budget,
- [ ] accessibility,
- [ ] performance,
- [ ] retention,
- [ ] narrative playtest,
- [ ] docs.

---

# 335. Global Definition of Done

- [ ] no second generic quest engine,
- [ ] no duplicate relationship state,
- [ ] no duplicate skill state,
- [ ] no duplicate trait state,
- [ ] no duplicate memory graph,
- [ ] no duplicate romance/family state,
- [ ] no duplicate psychology state,
- [ ] no invented world secrets,
- [ ] no forced romance,
- [ ] no fabricated grievance/guilt,
- [ ] no slot-filling quest spam,
- [ ] no save/load reward duplication,
- [ ] no old-save retroactive quest flood,
- [ ] no hidden-agenda leakage,
- [ ] no per-frame trigger scanning,
- [ ] full verification green.

---

# 336. Closure Report Template

```markdown
## C2[40] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Shared quest runtime:
- Relations owner:
- Skill owner:
- Trait owner:
- Event owner:
- Memory availability:
- Psychology availability:
- Romance/family availability:
- Existing personal narrative content:

### 200A — Foundation
- PersonalQuestSystem:
- Shared-runtime adapter:
- Quest types:
- Arc types:
- Trigger types:
- Stage condition types:
- Branch model:
- Active cap:
- RNG stream:
- Arc milestone model:
- Consequence ports:
- Save schema:
- Old-save behavior:
- Missing ports:
- Result:

### 200B — Templates / UI
- Template count:
- Redemption:
- Growth:
- Discovery:
- Revenge:
- Protection:
- Legacy:
- Love:
- Mastery:
- Min stages:
- Max stages:
- Branching templates:
- Arc families:
- Quest UI:
- Arc UI:
- History:
- Journal/archive:
- Unused templates:
- Narrative similarity findings:
- Result:

### 200C — Integration
- Relations:
- Skills:
- Traits:
- Events:
- Items:
- Locations:
- Memory:
- Psychology:
- Hidden agendas:
- Romance/family:
- Shared quest runtime:
- Save-load duplicates:
- Trigger exploits:
- Reward exploits:
- Branch rerolls:
- Old-save retroactive quests:
- Result:

### Balance / Soak
- 200-day quests generated:
- quests/survivor:
- completion rate:
- failure rate:
- abandonment rate:
- arc resolutions:
- dominant quest type:
- dominant arc type:
- notification count:
- active-cap violations:
- 400-day/multigeneration findings:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Personal quest selftest:
- Port contract:
- Shared-runtime ownership audit:
- Template reference integrity:
- Branch reachability:
- Old-save fixture:
- Same-history deterministic replay:
- Narrative soak:
- Accessibility:
- Result:

### Final Metrics
- PERSONAL_QUEST_TEMPLATES:
- PERSONAL_QUESTS_GENERATED:
- PERSONAL_QUESTS_ACTIVE:
- PERSONAL_QUESTS_COMPLETED:
- PERSONAL_QUESTS_FAILED:
- PERSONAL_QUESTS_ABANDONED:
- CHARACTER_ARCS_ACTIVE:
- CHARACTER_ARCS_RESOLVED:
- QUEST_TRIGGER_DUPLICATES:
- QUEST_REWARD_DUPLICATES:
- ACTIVE_CAP_VIOLATIONS:
- UNREACHABLE_STAGES:
- INVALID_REFERENCE_COUNT:
- SHARED_RUNTIME_DUPLICATION_VIOLATIONS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Personal quest content:
- Memory integration:
- Psychology integration:
- Romance/family integration:
- Legacy/biography:
- UI:
```

---

# 337. Final Execution Directive

Execute Plan 200 as a **survivor-specific narrative orchestration layer over the canonical quest runtime and existing survivor-state systems**.

The critical sequence is:

```text
audit/reuse the shared quest runtime
→ define personal quest bindings and arc milestones
→ listen to canonical relationship/skill/event/item/location/trait/time triggers
→ score only truly relevant templates against survivor identity/history
→ deterministically generate a quest when a meaningful match exists
→ progress stages through typed canonical conditions
→ persist branch decisions
→ route rewards/failures through canonical owners
→ convert quest outcomes into character-arc milestones
→ preserve terminal personal-story history
→ validate rarity, relevance, determinism, and long-run narrative density
```

Do not create a second generic quest engine.

Do not create a duplicate relationship, skill, trait, memory, romance, or psychology state.

Do not invent a grudge, romance, secret location, trauma, or biography merely to make a template fit.

Do not generate quests simply because a survivor has an empty personal-quest slot.

Do not treat a character arc as an XP bar detached from meaningful milestones.

The strongest authority rule is:

> **PersonalQuestSystem owns why a survivor-specific story begins and how its narrative milestones are interpreted; the shared quest runtime owns generic quest lifecycle, while every relationship, skill, trait, item, location, memory, romance, and psychological fact remains owned by its canonical system.**

The strongest narrative rule is:

> **A personal quest must be explainable from the survivor’s actual history: the player should be able to understand why this person, why this story, and why now.**

The strongest arc rule is:

> **Character arcs emerge from authored milestone patterns across real quest choices and outcomes; they are not generic progress bars that advance because the player completed arbitrary tasks.**

The flagship acceptance scenario is:

> **Create a seeded survivor who failed a rescue, carries a guilt-producing event reference, has a damaged relationship with the person they left behind, and possesses a relevant keepsake. Emit the canonical trigger and generate a Redemption personal quest from the matching template. The quest must bind those exact survivor/event/item/relationship references, progress through 3–7 authored stages in the shared quest runtime, and present one branch between restitution, avoidance, and self-justification. Save/load before the branch, choose restitution, complete a real rescue/protective action, and resolve the quest. Relationship, morale, trait, skill, psychology, journal, and archive consequences must be applied only through their canonical owners and exactly once. The resulting Redemption arc must derive from completed milestones rather than a manually incremented bar. Re-run with the same seed/history/choices and verify the same quest ID, template, branch options, stage sequence, consequence IDs, and arc outcome. Then swap in a survivor without the failed-rescue history: the quest must not generate.**
