# D1 Flagship Integration Plan [8]
## Plan 161 — Survivor Hobby & Leisure System

> **Purpose:** Turn survivor off-hours from an empty interval into a deterministic, data-driven personality and
> shelter-culture layer where survivors pursue hobbies, develop preferences, form social circles, create works,
> teach others, and gain bounded morale benefits without becoming optimization chores.
>
> **Primary source:** Plan 161 — Survivor Hobby & Leisure System.
>
> **Core design problem:** survivors currently possess skills, needs, duties, and relationships, but little
> persistent personal life. They are functional game pieces rather than people with tastes, routines, leisure,
> creative expression, and self-directed culture.
>
> **Implementation posture:** optional, autonomy-compatible, low-micromanagement, deterministic, save-safe,
> data-driven, bounded in mechanical power, and integrated through existing needs, skill, relationship,
> apprenticeship, shelter, and mental-health authorities rather than duplicating them.
>
> **Critical guardrail:** hobbies must not become another mandatory production queue. The player should primarily
> enable time, spaces, and materials; survivors should express personality through what they choose to do.

---

## 1. Source Problem Statement

The source plan identifies a specific survivor-personality gap:

- survivors work, sleep, respond to orders, and react to system events;
- they have skills, needs, and relationships;
- they do not maintain hobbies, interests, creative outlets, leisure routines, or cultural traditions;
- Plan 144 adds autonomy but not structured leisure;
- Plan 150 adds romance/family but not individual interests;
- Plan 154 adds education but not recreation.

That means shelter life is mechanically dense but culturally thin.

Plan 161 should therefore introduce a new run-time layer with this shape:

```text
Survivor identity + traits + skills + current needs
                  ↓
             free-time eligibility
                  ↓
              HobbySystem
       ┌──────────┼──────────┐
       ↓          ↓          ↓
 preference   facility    materials
       └──────────┼──────────┘
                  ↓
            hobby session
                  ↓
      proficiency + morale + social facts
                  ↓
 needs / skills / relations / apprenticeship / culture / journal
```

The hobby system owns leisure participation and proficiency. It does not become the owner of morale, survivor
skills, relationship scores, mental-health state, room construction, or shelter production.

---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `HobbySystem.cs` exists in Core with schema-versioned capture/restore.
2. All hobby definitions live in a canonical data catalog.
3. Survivor hobby preferences are deterministic and save-compatible.
4. Hobby sessions occur only during valid free-time windows.
5. The system integrates with the canonical survivor scheduling/availability authority.
6. Morale changes are routed to the canonical needs/morale system.
7. Skill gains are routed to `SkillProgressionSystem` or its actual canonical equivalent.
8. Shared-hobby relationship effects route to the canonical relations system.
9. Teaching routes to the canonical apprenticeship/mentorship authority where available.
10. Facilities are validated against real shelter rooms/upgrades.
11. Resource costs consume real inventory through canonical transaction APIs.
12. Hobby sessions cannot be spammed by repeatedly opening UI or reloading.
13. Mastery progress persists across save/load.
14. Old saves initialize safely.
15. No hobby definition references nonexistent facilities, materials, skills, or localization keys.
16. Headless simulation runs without any hobby UI scene.
17. `--hobbies-selftest` validates catalog integrity, deterministic selection, sessions, save round-trip, and
    integration effects.
18. A survivor can have personality expression without requiring player micromanagement every day.
19. Hobbies do not provide runaway morale or skill farming.
20. The 30-hobby content target is reached only with valid facilities/resources and distinct gameplay identity.

---

## 3. Repository Reconnaissance Before Editing

Create `docs/hobbies/HOBBY_INTEGRATION_AUDIT.md`.

Inspect:

- `Assets/Ashfall.Core/Survivors/`
- `SkillProgressionSystem.cs`
- `NeedsSystem.cs`
- survivor availability/duty scheduling
- survivor trait/personality data
- survivor autonomy implementation from Plan 144 if present
- survivor relationship system
- apprenticeship/mentorship system
- mental-health crisis system
- shelter rooms/facilities
- inventory/material transaction APIs
- crafting/workshop system
- garden/agriculture system
- library/education systems
- morale event APIs
- save schema
- game clock/day/hour authority
- seeded RNG
- UI survivor panel/presenter architecture
- journal/event log
- localization/catalog loader conventions
- achievement system if Plan 149 is implemented
- epilogue system if Plan 145 is implemented

For each integration point, record:

| Question | Required answer |
|---|---|
| Canonical owner | exact system/type |
| Stable ID | yes/no |
| Persisted | yes/no + schema |
| Event/command API | exact contract |
| Can hobby write directly? | preferably no |
| Deterministic | yes/no |
| Old-save behavior | migration |
| Catalog validator | available/missing |
| UI coupling | risk |

Do not assume example room names such as `library` or `studio` actually exist. Use repository-backed facilities.

---

## 4. Scope Boundaries

### In scope

- hobby catalog;
- survivor hobby preference/interest state;
- participation;
- proficiency;
- mastery bands;
- session scheduling during free time;
- facility/material requirements;
- deterministic session outcomes;
- bounded morale effects;
- shared-hobby social effects;
- teaching hooks;
- hobby group formation;
- notable hobby events;
- UI projection;
- save/load;
- old-save migration;
- selftests and coverage reporting.

### Out of scope for first pass

- full survivor autonomy rewrite;
- new profile/meta progression;
- monetized cultural economy;
- a second crafting system;
- a second education system;
- a second relationship system;
- unrestricted skill farming;
- deep festival calendar;
- procedurally generated art assets;
- live generative storytelling;
- cross-campaign legacy mechanics.

Those can consume hobby facts later.

---

## 5. Core Domain Model

Recommended data types:

```csharp
public sealed record HobbyDefinition
{
    public string HobbyId { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }
    public string CategoryId { get; init; }
    public IReadOnlyList<string> AffinityTags { get; init; }
    public IReadOnlyList<HobbyRequirement> Requirements { get; init; }
    public string? FacilityId { get; init; }
    public IReadOnlyList<ItemCost> SessionCosts { get; init; }
    public int SessionDurationHours { get; init; }
    public int BaseProficiencyGain { get; init; }
    public int BaseMoraleEffect { get; init; }
    public string? RelatedSkillId { get; init; }
    public string? SocialProfileId { get; init; }
}
```

```csharp
public sealed record HobbyProgress
{
    public string SurvivorId { get; init; }
    public string HobbyId { get; init; }
    public int Proficiency { get; init; }
    public int SessionsCompleted { get; init; }
    public int LastSessionDay { get; init; }
    public HobbyMasteryLevel MasteryLevel { get; init; }
    public int InterestStrength { get; init; }
}
```

```csharp
public sealed record HobbyState
{
    public int SchemaVersion { get; init; }
    public IReadOnlyList<HobbyProgress> Progress { get; init; }
    public IReadOnlyDictionary<string, string> PrimaryHobbyBySurvivor { get; init; }
    public IReadOnlyList<HobbyGroupState> Groups { get; init; }
    public IReadOnlySet<string> CelebratedMilestoneIds { get; init; }
}
```

Prefer persistent progress over persisting derived facility availability.

---

## 6. Hobby Categories

Retain the six source categories:

- creative;
- intellectual;
- physical;
- social;
- crafting;
- collecting.

Categories are UI/content taxonomy, not hardcoded effect behavior.

Examples:

### Creative
painting, music, writing, storytelling, sculpture

### Intellectual
reading, puzzles, chess, philosophy, study-for-pleasure

### Physical
exercise, yoga, dancing, sports, recreational martial practice

### Social
games, conversation circles, theater, parties

### Crafting
woodworking, sewing, pottery, cooking-for-pleasure, metal craft

### Collecting
rocks, bottles, artifacts, specimens, keepsakes

Actual definitions must be checked against existing items, facilities, and setting tone.

---

## 7. Hobby Definition Authority

Create:
`Assets/StreamingAssets/Data/hobby_definitions.json`

Recommended root:

```json
{
  "schemaVersion": 1,
  "categories": [],
  "hobbies": [],
  "masteryBands": [],
  "socialProfiles": []
}
```

Do not store runtime survivor progress in content data.

---

## 8. Stable Hobby IDs

IDs are save contracts.

Example:

```text
hobby_painting
hobby_storytelling
hobby_chess
hobby_exercise
hobby_cards
hobby_woodworking
hobby_rock_collecting
```

Never recycle a shipped ID for a different hobby.

Display names stay localized.

---

## 9. Interest vs Proficiency

Separate:

- **interest**: how much a survivor likes/wants to do the hobby;
- **proficiency**: how practiced they are.

A survivor may:
- love music but be poor at it;
- become skilled in woodworking but lose interest;
- casually join a social game without mastery.

This is much richer than one proficiency number standing in for personality.

---

## 10. Deterministic Interest Assignment

Initial interests can derive from:

- survivor traits;
- background tags;
- skills;
- age/life-history tags if canonical;
- authored preferences;
- deterministic seeded fallback.

Priority:

1. explicit authored survivor preference;
2. strong trait affinity;
3. skill/background affinity;
4. deterministic seed selection.

Do not randomize on every load.

---

## 11. Interest Seed

Use canonical `ISeededRng`.

Seed from stable facts such as:
- campaign seed;
- survivor ID;
- hobby preference schema version.

Persist chosen interests so catalog changes do not silently reroll personalities.

Never use wall-clock time.

---

## 12. Personality-Hobby Affinity

Use data tags rather than giant `switch` blocks.

Example:

```text
trait_creative -> creative +20
trait_bookish -> intellectual +25
trait_social -> social +20
skill_crafting_high -> crafting +10
trait_solitary -> social -10
```

Only use real traits.

If repository traits do not support direct affinity mapping, keep v1 simpler.

---

## 13. Hobby Eligibility

A survivor can start a session only if:

- alive;
- awake;
- not on expedition;
- not incapacitated;
- not assigned to higher-priority duty;
- enough free-time block exists;
- facility available if required;
- materials available if required;
- any minimum skill requirement satisfied;
- hobby not on personal cooldown;
- no critical need overrides leisure.

This must use the canonical scheduling/availability authority.

---

## 14. Free-Time Windows

Do not let the hobby system invent a second calendar.

Preferred integration:

```text
scheduler/autonomy system
 -> survivor enters free-time state
 -> HobbySystem chooses eligible leisure action
```

If Plan 144 autonomy is not implemented yet, expose a deterministic `TrySelectLeisureActivity()` API that future
autonomy can call.

Avoid per-frame idle scanning.

---

## 15. Player Control Model

The best UX is "enable and influence," not "queue daily painting."

Allow player actions such as:
- provide facilities;
- provide materials;
- optionally encourage/disable specific hobby;
- designate a hobby-friendly room;
- organize a group event.

Survivors should usually choose sessions themselves during free time.

---

## 16. Hobby Session State Machine

Use:

```text
eligible
 -> planned
 -> active
 -> completed
```

Optional:
- cancelled;
- interrupted.

Persist active sessions only if sessions span saveable time intervals.

Completion should be idempotent.

---

## 17. Session Duration

Source suggests 1–4 hours.

Put duration in data.

Examples:
- reading 1h;
- chess 2h;
- painting 3h;
- group performance 4h.

Do not make every hobby equal.

---

## 18. Session Material Costs

Use canonical inventory transactions.

Examples:
- painting consumes pigment/canvas only if those items actually exist;
- woodworking consumes scrap/wood;
- music may require no consumable cost once instrument exists;
- chess uses no consumable material;
- cooking-for-fun may consume ingredients.

Reserve costs before session begins or commit atomically at completion according to existing transaction patterns.

---

## 19. Facility Requirements

Facility references must resolve to real shelter capability IDs.

Possible conceptual mappings:
- library/reading space;
- workshop;
- garden;
- common room;
- exercise area;
- studio/music space.

If a dedicated room does not exist, use a capability tag on a room rather than inventing a fake shelter type.

---

## 20. Facility Capability Interface

Recommended abstraction:

```csharp
public interface IHobbyFacilitySource
{
    bool HasCapability(string facilityCapabilityId);
    int GetQuality(string facilityCapabilityId);
}
```

HobbySystem reads capability; shelter system remains owner.

---

## 21. Facility Quality

Optional quality scale:
- improvised;
- basic;
- improved;
- excellent.

Quality can affect:
- proficiency gain;
- comfort/morale;
- group capacity.

Keep bonuses small.

---

## 22. Mastery Bands

Retain:

- novice: 0–24;
- apprentice: 25–49;
- journeyman: 50–74;
- master: 75–100.

Use exact non-overlapping thresholds in data.

Mastery is derived from proficiency.

---

## 23. Proficiency Gain

Suggested:

```text
gain =
  base gain
  + interest modifier
  + facility quality modifier
  + teacher modifier
  - fatigue penalty
```

Clamp.

Do not let hobby proficiency rise faster by save/reload.

---

## 24. Diminishing Returns

Avoid infinite farming.

Options:
- lower gain at high proficiency;
- one meaningful session per day;
- fatigue;
- material limits;
- free-time scarcity.

The system should never make "spend every free hour painting" optimal for survival stats.

---

## 25. Morale Effects

HobbySystem emits a morale effect request.

It must not store its own morale.

Suggested components:
- participation bonus;
- favorite-hobby bonus;
- group bonus;
- mastery satisfaction;
- novelty bonus;
- diminishing repeated-session bonus.

Clamp daily contribution.

---

## 26. Morale Daily Cap

Introduce a data-driven daily leisure morale cap per survivor.

Reason:
- prevent spam;
- preserve crisis meaning;
- keep hobbies beneficial but non-dominant.

The cap belongs to hobby effect emission, not NeedsSystem internals.

---

## 27. Mental Health Integration

Hobbies can reduce crisis risk only through a typed integration.

Preferred:
```text
HobbySessionCompleted
 -> MentalHealthCrisisSystem receives protective factor
```

Do not directly set crisis probability from HobbySystem.

A hobby should not instantly cure serious conditions.

---

## 28. Related Skill Gains

Secondary skill improvement should be small.

Examples:
- woodworking -> crafting;
- recreational cooking -> cooking;
- storytelling -> social/leadership only if real;
- exercise -> physical conditioning only if such progression exists.

Cap skill XP from hobbies per day/week.

---

## 29. Skill Authority

Emit:

```text
HobbySkillPractice(
  survivorId,
  relatedSkillId,
  practiceAmount,
  sourceHobbyId)
```

`SkillProgressionSystem` decides actual progression.

---

## 30. Shared Hobbies

Two survivors with the same hobby can form a shared activity if:

- both available;
- facility supports group size;
- relationship system does not prohibit;
- schedules overlap.

Shared sessions can emit a small relationship/affinity event.

---

## 31. Relationship Effects

Use typed relationship events.

Examples:
- shared positive leisure;
- teaching session;
- friendly competition;
- conflict/rivalry.

Do not store bond strength in HobbyState.

---

## 32. Hobby Groups

A group is persistent cultural state, not a new faction.

DTO:

```csharp
public sealed record HobbyGroupState
{
    public string GroupId { get; init; }
    public string HobbyId { get; init; }
    public IReadOnlyList<string> MemberSurvivorIds { get; init; }
    public int FormedDay { get; init; }
}
```

Examples:
- book club;
- card circle;
- music ensemble;
- exercise group.

Keep group mechanics lightweight.

---

## 33. Group Formation

Form when:
- minimum members share hobby;
- repeated shared sessions occur;
- suitable facility exists.

Use deterministic threshold rules.

Do not form/dissolve every day due to transient scheduling.

---

## 34. Hobby Teaching

Source says journeymen/masters can teach.

Preferred:
- teacher must meet mastery threshold;
- learner interested/eligible;
- shared free time;
- facility available;
- teaching session replaces ordinary session;
- learner receives extra proficiency;
- teacher receives reduced proficiency but morale/social value.

---

## 35. Apprenticeship Integration

If `ApprenticeshipSystem` exists, route hobby teaching through its mentor relationship or expose compatible
teaching events.

Do not create duplicate mentor graphs.

---

## 36. Friendly Rivalry

Rivalry should be flavor plus bounded social effect.

Possible trigger:
- same hobby;
- similar proficiency;
- repeated group sessions.

Effects:
- competition event;
- small proficiency motivation;
- relationship event.

Avoid permanent hostility from trivial leisure rivalry.

---

## 37. Cultural Output

Some hobbies can produce non-economic cultural artifacts:

- painting;
- song/performance;
- written story;
- sculpture;
- collection display.

Represent as `HobbyCreationFact`, not necessarily inventory item.

This lets journal/epilogue/culture systems reference them.

---

## 38. Physical Hobby Outputs

If a hobby creates a real item:
- use crafting/inventory authority;
- define recipe/material output;
- avoid duplicating crafting.

Most hobby outputs can remain narrative/cultural facts unless they need gameplay use.

---

## 39. Hobby Events

Source events:

- The Masterpiece
- The Performance
- The Exhibition
- The Competition
- The Club
- The Tradition
- The Discovery

Implement as event hooks triggered by data-backed milestones.

Do not make them required for base sessions.

---

## 40. Event Trigger Examples

### Masterpiece
- master proficiency;
- sufficient sessions;
- creative hobby;
- deterministic milestone trigger.

### Performance
- social/creative group;
- enough members;
- suitable space.

### Exhibition
- multiple cultural output facts.

### Competition
- two+ comparable participants.

### Tradition
- repeated event across long period.

### Discovery
- rare insight hook routed to research, never direct free unlock.

---

## 41. Research Insight Integration

If hobbies occasionally produce insights:

```text
HobbyDiscoveryFact
 -> ResearchSystem / ResearchUnlockBridge
```

Research remains authority.

The chance should be rare, deterministic, and bounded.

---

## 42. Hobby Quests

Source hooks:
- The Artist;
- The Performance;
- The Collection;
- The Tournament;
- The Workshop;
- The Legacy.

Expose events/queries so quest runtime can use hobby state.

Do not embed quest logic in HobbySystem.

---

## 43. Collection Hobbies

Collecting needs a different progress model than practice hobbies.

Possible:
- proficiency from curation;
- collection progress from item/tag IDs.

Do not require individual inventory items if catalog support is weak.

A collection may reference tagged discoveries/keepsakes instead.

---

## 44. Collecting and Inventory

If collecting physical objects:
- reserve one instance or record discovery?
- decide whether collection removes usable inventory.

Default recommendation:
use discovery/collection ledger rather than permanently locking survival-critical items.

---

## 45. Crafting Hobbies vs Production Crafting

Critical separation:

- production crafting exists to create needed goods;
- hobby crafting exists for leisure/personality.

A survivor woodworking for relaxation should not silently duplicate production output rates.

If an item is produced, use reduced/explicit recipe output.

---

## 46. Physical Hobbies

Exercise, dance, sport, yoga, etc.

Effects:
- morale;
- social;
- tiny skill/conditioning practice.

Do not create a second health/fitness stat unless already planned.

---

## 47. Intellectual Hobbies

Reading, chess, puzzles, philosophy.

Effects:
- morale;
- social;
- small relevant skill practice;
- rare research insight.

Do not treat leisure reading as full education progression.

---

## 48. Social Hobbies

Games, conversation, parties, theater.

Primary:
- morale;
- relationship events;
- shelter culture.

Avoid making them universal best-in-slot due to affecting many survivors at once; cap group bonuses.

---

## 49. Creative Hobbies

Painting, music, writing, storytelling, sculpture.

Primary:
- personality;
- morale;
- cultural artifacts;
- events.

These are strong candidates for epilogue and shelter-culture facts.

---

## 50. Hobby Selection Algorithm

Recommended deterministic scoring:

```text
score =
  interestStrength
  + traitAffinity
  + recentNeglectBonus
  + socialOpportunityBonus
  + facilityQualityBonus
  - materialScarcityPenalty
  - repeatedSessionPenalty
```

Sort by score, stable hobby ID tie-break.

Optional small seeded variation may be used only if stable and persisted.

---

## 51. Avoiding Repetitive Leisure

Add preference for variation:

- repeated same hobby receives small short-term penalty;
- favorite hobby keeps baseline affinity;
- survivor may rotate secondary hobbies.

This makes people feel less robotic.

---

## 52. Primary and Secondary Hobbies

Recommended:
- one primary hobby;
- zero to two secondary interests.

Avoid tracking mastery in all 30 hobbies for every survivor unless actually used.

---

## 53. Interest Discovery

Survivors may discover a new hobby when:
- new facility becomes available;
- group invites them;
- teacher introduces them;
- event occurs.

Use deterministic rules.

Do not reroll personality wholesale.

---

## 54. Abandoning or Pausing Hobbies

Do not delete progress.

A hobby can become inactive because:
- facility gone;
- materials unavailable;
- survivor busy;
- interest shifts.

Proficiency persists.

---

## 55. Severe Crisis Priority

If survivor has urgent:
- hunger;
- sleep deprivation;
- medical need;
- critical duty;
- immediate danger;

leisure should lose scheduling priority.

Hobbies must never cause a survivor to ignore survival-critical needs.

---

## 56. Optionality

A campaign with no hobby facilities should still function.

Expected:
- no crashes;
- survivors may use zero-cost/facility-light hobbies if design allows;
- no mandatory quest deadlock;
- no morale baseline penalty simply because hobbies are unused, unless intentional shelter-life design says so.

---

## 57. Baseline Leisure

Consider a small set of facility-light hobbies:
- storytelling;
- conversation;
- simple exercise;
- reading only if books exist;
- simple games if props exist.

This prevents the system from being completely inert early-game.

---

## 58. 30-Hobby Content Strategy

Do not create 30 mechanically identical entries.

Suggested distribution:
- 5 creative;
- 5 intellectual;
- 5 physical;
- 5 social;
- 5 crafting;
- 5 collecting.

Each should differ in at least two of:
- facility;
- materials;
- group size;
- session duration;
- affinity tags;
- social profile;
- skill hook;
- event hooks.

---

## 59. Hobby Catalog Validation

Validate:
- unique ID;
- category exists;
- localization keys exist;
- session duration valid;
- morale effect bounded;
- proficiency gain bounded;
- facility ID/capability resolves;
- material item IDs resolve;
- related skill ID resolves;
- affinity tags resolve if cataloged;
- event hook IDs resolve;
- no negative/invalid cost.

---

## 60. Save Schema

Persist:
- hobby progress;
- interest strength;
- selected primary/secondary hobbies;
- groups;
- active sessions if needed;
- celebrated milestone IDs;
- deterministic session/event seeds if necessary.

Do not persist:
- derived facility availability;
- rendered UI text;
- current morale copied from NeedsSystem.

---

## 61. Old-Save Migration

Source says empty hobby state.

Better migration:
- empty progress;
- no groups;
- no milestone celebrations;
- deterministic interest initialization on first safe load.

Persist interests immediately so next reload is stable.

Do not retroactively award sessions.

---

## 62. Save/Load Mid-Session

Test:
1. session starts;
2. save;
3. reload;
4. finish.

Expected:
- material cost not duplicated;
- proficiency applied once;
- morale applied once;
- session counter increments once.

---

## 63. Idempotent Session Completion

Use stable session IDs:
```text
hobby:<campaign>:<survivor>:<startDay>:<sequence>
```

Persist completed effect IDs if event replay architecture can duplicate completion.

---

## 64. Deterministic RNG

Use `ISeededRng` only for:
- rare hobby events;
- deterministic discovery variation;
- personality fallback selection.

Normal proficiency/morale should be deterministic formulas.

No `System.Random`.

---

## 65. UI Projection

Survivor panel should show:
- primary hobby;
- secondary hobbies;
- proficiency;
- mastery;
- sessions completed;
- facility/material requirements;
- current availability;
- recent notable hobby event.

No UI logic should decide proficiency or morale.

---

## 66. Shelter Hobby Panel

Optional shelter view:
- available facilities;
- groups;
- upcoming cultural events;
- material shortages;
- masters/teachers.

Avoid turning this into another production dashboard.

---

## 67. Progress Display

Use:
- proficiency bar;
- mastery label;
- next mastery threshold;
- sessions or recent activity.

Do not expose hidden RNG.

---

## 68. Tooltips

Show:
- what the hobby is;
- requirements;
- typical session duration;
- morale/social benefits;
- facility;
- material cost.

Avoid exact long-term optimization numbers if that harms tone.

---

## 69. Tutorial / Guidance

First session guidance should explain:
- hobbies happen in free time;
- player can enable spaces/materials;
- hobbies improve morale but do not replace survival needs;
- survivors choose based on personality.

Use canonical guidance UI.

---

## 70. Journal Integration

Log only notable events:
- first mastery;
- masterpiece;
- group formed;
- performance;
- tradition.

Do not log every reading session.

---

## 71. Achievement Integration

Plan 149 can observe:
- first master hobbyist;
- group formation;
- cultural event;
- multi-category shelter culture.

HobbySystem does not own achievement state.

---

## 72. Epilogue Integration

Plan 145 can consume:
- master hobbyists;
- major cultural works;
- shelter traditions;
- enduring hobby groups.

Export stable fact IDs, not prose.

---

## 73. Autonomy Integration

Plan 144 survivor autonomy should be the preferred scheduler.

HobbySystem offers:
- eligible leisure actions;
- score;
- requirements;
- execution API.

Autonomy chooses among leisure actions when higher priorities permit.

---

## 74. Mental Health Guardrail

Hobbies are protective, not curative.

Do not:
- erase trauma;
- instantly remove crises;
- override treatment;
- produce giant mood swings.

Use small, cumulative protective factors.

---

## 75. Shelter Culture

Define optional aggregate culture facts:
- number of active groups;
- number of masters;
- cultural works created;
- traditions established.

Do not invent a new global "culture score" unless another plan needs it.

---

## 76. Performance Budget

Hobbies should be event/schedule-driven.

Requirements:
- catalog parsed once;
- no per-frame scan of all survivor × hobby combinations;
- pre-index hobbies by category/facility;
- score only eligible candidate set;
- group matching at scheduled intervals;
- session progression proportional to active sessions.

Campaigns with no hobby use should have negligible overhead.

---

## 77. Data Integrity Self-Test

Extend standard integrity checks.

Validate:
- all 30 definitions;
- all categories;
- all facilities;
- all items;
- all skills;
- all localization keys;
- all mastery bands;
- all social profiles;
- all event hooks;
- no overlapping mastery ranges;
- no unbounded morale values.

---

## 78. Dedicated `--hobbies-selftest`

It should:

1. load catalog;
2. initialize deterministic survivor interests;
3. verify same seed produces same preferences;
4. run a basic session;
5. verify materials;
6. verify proficiency;
7. verify morale event;
8. verify mastery transition;
9. verify shared hobby session;
10. verify teaching;
11. save/reload mid-session;
12. verify once-only completion;
13. verify no-facility behavior;
14. verify old-save migration;
15. verify UI independence;
16. exit non-zero on mismatch.

---

## 79. Unit Test Matrix

### Catalog
- valid hobby;
- duplicate ID;
- missing category;
- bad facility;
- bad item;
- bad skill;
- bad localization.

### Interest
- authored preference;
- trait affinity;
- deterministic fallback;
- save stability.

### Eligibility
- free survivor;
- sleeping;
- expedition;
- injured;
- missing facility;
- missing material;
- duty conflict.

### Session
- start;
- consume material;
- complete;
- proficiency;
- morale;
- duplicate completion prevention.

### Mastery
- novice;
- apprentice;
- journeyman;
- master;
- threshold boundaries.

### Social
- shared session;
- group formation;
- teaching;
- relationship event.

### Save
- empty;
- active;
- groups;
- progress;
- old migration.

---

## 80. Golden Scenarios

1. Survivor with explicit creative affinity chooses painting.
2. Survivor without authored preference deterministically chooses hobby.
3. No facility available.
4. Material shortage blocks material-heavy hobby.
5. Zero-cost storytelling session succeeds.
6. Two survivors form shared hobby activity.
7. Master teaches novice.
8. Mastery event fires once.
9. Mid-session save/load.
10. Mental-health protective event emitted.
11. Skill practice emitted.
12. Old save initializes interests.
13. Shelter with all facilities but no free time produces no sessions.
14. Critical hunger suppresses leisure.
15. Large roster remains deterministic and performant.

---

## 81. Fuzz / Property Testing

Properties:
- proficiency remains 0..100;
- mastery matches proficiency;
- completed sessions never decrease;
- same persisted state yields same deterministic selection;
- no duplicate session completion;
- no missing survivor references;
- no material cost below zero;
- no survivor in incompatible simultaneous sessions;
- group members exist and share hobby.

---

## 82. Exploit Prevention

Prevent:
- start/cancel material duplication;
- save/reload repeat morale;
- multiple same-hour sessions;
- proficiency farming from UI refresh;
- repeated teaching loops;
- group bonus stacking without cap;
- hobby skill XP exceeding cap;
- zero-duration definitions.

Use persisted session IDs and daily caps.

---

## 83. Balance Framework

Hobbies should provide:

Primary:
- personality;
- morale;
- culture.

Secondary:
- relationships;
- small skill practice;
- occasional events.

Rare:
- research insight;
- notable legacy.

This keeps survival systems central.

---

## 84. Busywork Avoidance Rules

- no mandatory daily assignment;
- no requirement to micromanage every survivor;
- no dozens of facility toggles;
- no resource drain hidden from player;
- no massive penalty for ignoring hobbies;
- no repeated popups for routine sessions;
- no "best hobby" that dominates all others.

---

## 85. UI Accessibility

Support:
- keyboard navigation;
- controller where global support exists;
- text scaling;
- mastery not color-only;
- readable requirements;
- reduced-motion celebrations;
- group membership readable without hover-only dependency.

---

## 86. Structured Diagnostics

Logs:
```text
HobbyInterestInitialized survivor=<id> primary=<id>
HobbySessionStarted session=<id> survivor=<id> hobby=<id>
HobbySessionCompleted session=<id> proficiency=<n>
HobbyMasteryReached survivor=<id> hobby=<id> mastery=<level>
HobbyGroupFormed group=<id> hobby=<id>
HobbyTeachingCompleted teacher=<id> learner=<id>
```

Do not log excessive routine detail in production.

---

## 87. Implementation Phase A — Audit and Contracts

Tasks:
1. audit survivor scheduling, traits, needs, skills, relations, facilities;
2. create integration audit;
3. define hobby catalog schema;
4. define progress/state DTOs;
5. define mastery bands;
6. add loader;
7. add validator;
8. add empty save state.

Exit:
catalog loads headlessly.

---

## 88. Implementation Phase B — Interest and Eligibility

Tasks:
1. explicit preference support;
2. affinity scoring;
3. deterministic fallback;
4. primary/secondary hobbies;
5. availability checks;
6. free-time API;
7. facility/material checks;
8. tests.

Exit:
survivors have stable interests and eligible leisure options.

---

## 89. Implementation Phase C — Sessions and Mastery

Tasks:
1. session lifecycle;
2. material transaction;
3. duration;
4. completion idempotency;
5. proficiency;
6. mastery transitions;
7. daily caps;
8. save/load.

Exit:
single-survivor leisure is fully functional.

---

## 90. Implementation Phase D — Morale and Skills

Tasks:
1. morale effect sink;
2. daily leisure morale cap;
3. related skill practice;
4. skill XP cap;
5. mental-health protective hook;
6. tests.

Exit:
benefits route through canonical systems.

---

## 91. Implementation Phase E — Social Leisure

Tasks:
1. shared session matching;
2. relationship events;
3. groups;
4. teaching;
5. apprenticeship handoff;
6. friendly rivalry;
7. group caps;
8. tests.

Exit:
hobbies create bounded social texture.

---

## 92. Implementation Phase F — Events and Culture

Tasks:
1. masterpiece;
2. performance;
3. exhibition;
4. competition;
5. club;
6. tradition;
7. discovery hook;
8. journal;
9. quest events.

Exit:
high-salience hobby moments exist without routine spam.

---

## 93. Implementation Phase G — UI

Tasks:
1. survivor hobby projection;
2. proficiency/mastery;
3. requirements;
4. facility view;
5. group view;
6. tooltips;
7. guidance;
8. accessibility;
9. snapshots.

Exit:
UI has no hobby business logic.

---

## 94. Implementation Phase H — 30-Hobby Catalog

Tasks:
1. migrate/author 5 creative;
2. 5 intellectual;
3. 5 physical;
4. 5 social;
5. 5 crafting;
6. 5 collecting;
7. validate distinct mechanics;
8. validate facilities/materials;
9. add fixtures;
10. coverage report.

Exit:
30 meaningful definitions, no filler.

---

## 95. Implementation Phase I — CI Hardening

Tasks:
1. `--hobbies-selftest`;
2. data integrity;
3. old-save migration;
4. mid-session save/load;
5. fuzz tests;
6. performance sanity;
7. coverage report;
8. full regression.

Exit:
hobby simulation is deterministic and headless.

---

## 96. Coverage Report

Generate:
`docs/hobbies/HOBBY_COVERAGE.md`

Include:

| Category | Definitions | Facility-backed | Material-backed | Social | Skill hook | Fixtures |
|---|---:|---:|---:|---:|---:|---:|
| creative | | | | | | |
| intellectual | | | | | | |
| physical | | | | | | |
| social | | | | | | |
| crafting | | | | | | |
| collecting | | | | | | |

Also:
- hobbies unreachable due to missing facility;
- unreferenced facilities;
- invalid skill hooks;
- definitions never selected by deterministic fixture survivors;
- duplicate mechanical profiles.

---

## 97. Definition of Done — Flagship

### Core
- [ ] `HobbySystem.cs`
- [ ] catalog
- [ ] loader
- [ ] validator
- [ ] schema-versioned state
- [ ] deterministic interests
- [ ] session lifecycle

### Content
- [ ] 6 categories
- [ ] 30 hobbies
- [ ] meaningful differentiation
- [ ] localization
- [ ] valid facilities/materials/skills

### Runtime
- [ ] free-time integration
- [ ] proficiency
- [ ] mastery
- [ ] morale
- [ ] skill practice
- [ ] mental-health hook
- [ ] save/load
- [ ] anti-spam caps

### Social
- [ ] shared hobbies
- [ ] groups
- [ ] teaching
- [ ] relationship hooks
- [ ] optional rivalry

### Events
- [ ] notable event hooks
- [ ] journal
- [ ] quest hooks
- [ ] culture facts

### UI
- [ ] survivor hobby panel
- [ ] facility/group view
- [ ] accessibility
- [ ] no business logic in renderer

### Validation
- [ ] old saves
- [ ] selftest
- [ ] data-integrity checks
- [ ] fuzz/property tests
- [ ] no missing IDs
- [ ] headless behavior

---

## 98. Follow-On Task 161-A — Shelter Hobby Competition Framework

Goal:
support occasional contests without turning hobbies into competitive production.

Substeps:
1. define eligible hobbies;
2. participant selection;
3. deterministic judging;
4. morale/social effects;
5. journal;
6. achievement hooks;
7. rivalry guardrails;
8. tests.

---

## 99. Follow-On Task 161-B — Exhibitions and Performances

Goal:
allow cultural outputs to become shelter-wide events.

Substeps:
1. collect qualifying cultural facts;
2. schedule event;
3. facility requirement;
4. group participation;
5. bounded morale;
6. journal;
7. epilogue tag;
8. tests.

---

## 100. Follow-On Task 161-C — Shelter Traditions

Goal:
turn repeated leisure events into persistent cultural identity.

Substeps:
1. define tradition criteria;
2. deterministic annual/repeating trigger;
3. cultural fact;
4. social/morale effect;
5. journal;
6. epilogue integration;
7. no profile/meta ownership.

---

## 101. Follow-On Task 161-D — Hobby Legacy Integration

Goal:
allow Plan 145 to remember important cultural contributions.

Substeps:
1. define notable master criteria;
2. export stable fact IDs;
3. export masterpiece/tradition IDs;
4. epilogue templates;
5. avoid mentioning routine sessions;
6. golden ending tests.

---

## 102. Follow-On Task 161-E — Hobby Quest Pack

Goal:
author quests only after system hooks are stable.

Candidate arcs:
- The Artist;
- The Performance;
- The Collection;
- The Tournament;
- The Workshop;
- The Legacy.

Each quest must use canonical hobby facts/events rather than duplicate tracking.

---

## 103. Final Guardrails

- No per-frame hobby scan.
- No `System.Random`.
- No wall-clock personality rerolls.
- No hobby business logic in UI.
- No duplicate morale authority.
- No duplicate skill authority.
- No duplicate relationship authority.
- No duplicate apprenticeship authority.
- No fabricated facilities.
- No fabricated material IDs.
- No automatic survival-critical neglect.
- No mandatory daily micromanagement.
- No hobby session save/reload exploit.
- No uncapped morale stacking.
- No uncapped skill farming.
- No 30-hobby filler catalog.
- No New Game+ or profile/meta state.
- No routine-session journal spam.
- No mastery threshold overlap.
- No old-save crash.

When complete, Plan 161 should make the shelter feel inhabited rather than merely operated. Survivors will have
stable personal interests, use genuine free time, choose leisure based on who they are, build social rituals,
teach one another, create memorable cultural moments, and gain modest psychological value from living rather
than only surviving—without forcing the player to micromanage a second work schedule.

---

## Annex A — Representative Hobby Blueprint Matrix

The final 30 entries must match repository-backed skills, rooms, items, and setting. The following matrix
demonstrates the required differentiation.

### Creative
1. Painting — studio-capable space, pigments/material, solo, cultural output.
2. Music — instrument/equipment, solo/group, performance event.
3. Writing — quiet/library capability, low cost, story artifact.
4. Storytelling — no-cost group hobby, relationship emphasis.
5. Sculpture — workshop/studio, material cost, masterpiece hook.

### Intellectual
6. Reading — books/library capability, low cost.
7. Chess — common room, pair/group.
8. Puzzles — common room, low cost.
9. Philosophy circle — social/intellectual group.
10. Nature study — garden/outdoor capability, research-insight hook.

### Physical
11. Exercise — exercise space, solo.
12. Yoga/stretching — low equipment, mental-health protective hook.
13. Dancing — common room, social.
14. Recreational martial practice — training space, bounded skill hook.
15. Team sport — large/common area, group.

### Social
16. Cards — common room, group.
17. Board games — common room, group.
18. Conversation circle — no-cost group.
19. Theater — performance space, group/event.
20. Celebration cooking — kitchen capability, material cost, group event.

### Crafting
21. Woodworking — workshop, wood/scrap.
22. Sewing — textile material.
23. Pottery — workshop/kiln only if real.
24. Decorative metalwork — workshop, metal scrap.
25. Cooking-for-pleasure — kitchen, ingredients, cultural output.

### Collecting
26. Rocks/minerals — expedition/discovery tags.
27. Bottles/labels — scavenged keepsakes.
28. Artifacts — discovery-linked.
29. Botanical specimens — garden/expedition tags.
30. Personal keepsakes — survivor/story-linked.

For every entry, implementation must answer:
- what enables it;
- who tends to prefer it;
- whether it costs material;
- whether it is solo/group;
- what benefit it emits;
- what makes it distinct from another hobby;
- what system owns any secondary effect.

---

## 104. Scheduling Arbitration With Work, Rest, and Survival Needs

Hobbies need a precise place in survivor priority ordering.

Recommended priority model:

```text
life-threatening need
> emergency duty
> required treatment
> sleep/rest debt
> assigned critical work
> normal duty
> scheduled education/apprenticeship
> social obligation
> leisure/hobby
> idle
```

This ordering should not be hardcoded inside `HobbySystem`. The survivor scheduler or autonomy authority should
provide a `CanEnterLeisureWindow` or equivalent decision.

### Required integration cases

- a survivor beginning a hobby must be interruptible by fire, raid, medical emergency, or evacuation;
- interruption must not duplicate material cost;
- partially completed sessions either resume or cancel according to hobby definition;
- leisure cannot begin when the survivor is already reserved by another system;
- a survivor with severe exhaustion should prefer sleep;
- a survivor with starvation-level hunger should not choose chess over food;
- a survivor may still choose leisure while mildly stressed or tired if the autonomy model allows it.

### Test matrix

Create fixtures for:
- free survivor;
- mildly tired;
- critically tired;
- assigned night watch;
- under treatment;
- raid alarm;
- expedition departure imminent;
- scheduled teaching session;
- shelter emergency.

The result must be deterministic and explainable.

---

## 105. Session Reservation and Interruption Semantics

A hobby session consumes three potentially scarce resources:

1. survivor time;
2. facility capacity;
3. optional inventory materials.

Use a reservation phase:

```text
validate
 -> reserve survivor
 -> reserve facility slot
 -> reserve/consume materials
 -> start session
```

On cancellation:
- release survivor;
- release facility;
- refund only resources not actually consumed according to project transaction semantics.

On interruption:
- record progress only if the hobby supports partial credit;
- otherwise resume later or cancel without duplicate effects.

Do not let multiple systems assume the same room slot or survivor simultaneously.

---

## 106. Facility Capacity

A facility is more than a boolean.

Examples:
- small reading corner: capacity 1;
- common room table: capacity 4;
- music/performance space: capacity 6;
- workshop bench: capacity 2.

If the shelter system supports room occupancy/capacity, reuse it.

Otherwise define a minimal capability record:

```csharp
public sealed record HobbyFacilityAvailability
{
    public string CapabilityId { get; init; }
    public int Capacity { get; init; }
    public int Quality { get; init; }
    public int AvailableSlots { get; init; }
}
```

The hobby system does not own room construction.

---

## 107. Material Scarcity and Player Expectations

Material-consuming leisure can become frustrating during famine if survivors silently consume critical supplies.

Rules:

- only hobby-tagged/allowed materials may be auto-consumed;
- survival-critical resources require an explicit policy toggle or prohibition;
- UI shows expected per-session material use;
- shortages block or redirect leisure rather than creating negative inventory;
- survivor autonomy should prefer no-cost hobbies when material scarcity is severe.

Consider player-configurable shelter policy:

```text
Leisure Materials:
[ Conservative ] [ Normal ] [ Generous ]
```

Only implement if the project already has policy infrastructure.

---

## 108. Hobby Preference Stability Across Catalog Revisions

Persist chosen preference IDs.

On catalog revision:

### Hobby still exists
Keep interest/proficiency unchanged.

### Hobby removed/deprecated
Preserve historical progress in save but mark inactive.

### Hobby renamed
Stable ID means no migration.

### Hobby split into multiple new hobbies
Use explicit migration mapping.

### Hobby facility changes
Existing proficiency remains; future sessions use new requirements.

Never silently reroll a survivor's identity because JSON order changed.

---

## 109. Survivor Recruitment and New Arrivals

When a new survivor enters:

1. resolve explicit authored hobbies if present;
2. derive trait affinities;
3. deterministically choose fallback interests;
4. persist immediately;
5. do not grant retroactive proficiency unless biography data explicitly defines it.

A skilled carpenter might begin with some woodworking proficiency if the survivor content authority supports
background-based starting proficiency. That rule must be data-driven and tested.

---

## 110. Death, Departure, and Retirement

When a survivor leaves the active roster:

### Death
- progress remains available for journal/epilogue history if desired;
- remove from active hobby groups;
- release any facility/session reservation;
- no future sessions.

### Permanent departure
- remove from active groups;
- historical cultural facts remain.

### Retirement
- may remain a passive mentor only if the retirement system supports participation.

HobbySystem should subscribe to canonical fate events, not poll the roster.

---

## 111. Group Membership Maintenance

Group membership changes should be event-driven.

Remove or suspend member when:
- dead;
- departed;
- unavailable long-term;
- loses hobby interest only if design supports drift.

Groups should not disappear just because one member misses a session.

Possible states:

```text
active
dormant
disbanded
```

A dormant group can reactivate when enough members return.

---

## 112. Shelter-Wide Cultural Events

Separate ordinary hobby sessions from shelter-wide events.

A cultural event:
- requires a trigger;
- may reserve common space;
- may affect several survivors;
- may generate a journal entry;
- may create one cultural fact;
- may have a cooldown.

Examples:
- concert;
- exhibition;
- tournament;
- reading night;
- storytelling evening;
- craft fair.

These events should be infrequent enough to remain memorable.

---

## 113. Culture Event Morale Budget

Group events can accidentally multiply morale by roster size.

Define a bounded event budget:

```text
participant bonus <= X
observer bonus <= Y
shelter-wide event bonus <= Z
```

Use data-driven caps.

The event can feel significant narratively without trivializing morale crises.

---

## 114. Traditions as Derived Cultural Facts

A tradition should not be another constantly ticked stat.

Derive a stable fact after repeated qualifying events.

Example criteria:

```text
same cultural event family
+ repeated on 3+ separated dates
+ survived long enough
+ enough participants
```

Emit:
`ShelterTraditionEstablished(traditionId, sourceHobbyId)`

Downstream:
- journal;
- achievements;
- epilogue;
- shelter culture UI.

No permanent gameplay bonus is required.

---

## 115. Hobby Event Determinism

Rare events should use persistent milestone conditions where possible.

Prefer:

```text
masterpiece triggers on first session after:
proficiency >= 90
and sessionsCompleted >= 20
and not already celebrated
```

over a random 1% per session roll.

If RNG is used:
- seed is persisted/stable;
- event fires once;
- save/load cannot reroll.

---

## 116. Hobby Mastery Celebration

When mastery threshold is first reached:

1. update proficiency/mastery;
2. persist milestone marker;
3. emit `HobbyMasteryReached`;
4. optional journal/notification;
5. optional group response;
6. no second firing after load.

Mastery celebration should not directly grant profile rewards.

---

## 117. Proficiency Curve and Tuning

A linear `+N` every session is easy to implement but can feel mechanical.

Recommended data curve:

```text
0–24: 100% base gain
25–49: 80%
50–74: 60%
75–89: 40%
90–100: 20%
```

This makes mastery meaningful.

All rates should be tested against expected campaign duration.

Generate a simple simulation:
- sessions/week;
- expected days to apprentice;
- days to journeyman;
- days to master.

Reject tuning where mastery occurs in a few days or is unreachable in a normal campaign.

---

## 118. Campaign-Length Calibration

If the campaign typically lasts roughly months rather than years, mastery must fit that horizon.

Create a tuning report:

`docs/hobbies/HOBBY_PROGRESSION_CALIBRATION.md`

For each hobby profile:
- average session duration;
- plausible sessions/week;
- days to each mastery;
- resource cost to mastery;
- facility requirement onset.

Use this to prevent content that exists mathematically but never appears in play.

---

## 119. Interest Strength Evolution

Keep v1 stable.

If interest can change later:
- repeated positive sessions may increase interest slowly;
- repeated inability to access hobby should not automatically reduce it;
- trauma/event-based preference changes belong to authored systems.

Do not introduce random daily personality drift.

---

## 120. Boredom and Novelty

Do not create a mandatory boredom meter unless another plan owns it.

A light novelty modifier can prevent repetitive selection:

```text
recently_used_hobby penalty
favorite_hobby floor
secondary_hobby exploration bonus
```

This is sufficient for varied behavior.

---

## 121. Hobby Selection Explainability

For debugging, expose why a survivor selected a hobby.

Developer-only record:

```text
Survivor: survivor_ana
Candidate: hobby_music
Base interest: 72
Trait affinity: +15
Facility: +5
Repeated-use penalty: -10
Material scarcity: 0
Final: 82
```

This is useful for autonomy debugging and deterministic regression tests.

---

## 122. UI Characterization

The panel should make personality visible at a glance.

Recommended survivor card additions:
- primary hobby icon/name;
- mastery band;
- current/last leisure activity;
- optional group membership.

Detailed hobby view:
- all active interests;
- proficiency;
- requirements;
- recent cultural achievements;
- teacher/student links.

Do not overload the main survivor card with all 30 possible hobbies.

---

## 123. No-Hobby and Low-Interest Survivors

Not every survivor needs to be a passionate hobbyist.

Allow:
- low-interest survivor;
- one simple pastime;
- no formal mastery path;
- social participation without personal hobby identity.

This preserves personality variety.

Do not force everyone to have three hobbies for catalog utilization.

---

## 124. Authored Survivor Overrides

Story-critical survivors may have authored hobby preferences.

Data example:

```json
{
  "survivorId": "survivor_x",
  "hobbyPreferences": [
    { "hobbyId": "hobby_storytelling", "interest": 90 }
  ]
}
```

These overrides must resolve through the same catalog validator.

---

## 125. Hobby-to-Relationship Narrative Hooks

Shared leisure can generate narrative tags such as:
- `bond_shared_music`;
- `bond_chess_rivals`;
- `mentor_woodworking`;
- `group_story_circle`.

The relationship system decides actual numeric changes.

This gives writers stable hooks without duplicating social state.

---

## 126. Hobby Conflicts

Facilities and social preferences can produce mild conflicts.

Examples:
- two groups want the common room;
- noisy music disturbs a sleeper;
- competition causes tension.

Do not build a full conflict simulator in v1.

Expose event hooks only when existing shelter/relationship systems can consume them.

---

## 127. Environmental and Weather Influence

Outdoor hobbies may depend on environment.

Possible:
- stargazing requires clear weather and safe exterior access;
- gardening requires garden access;
- outdoor exercise may be unavailable during severe contamination/storm.

Use existing weather/hazard facts.

Do not create parallel environmental state.

---

## 128. Day/Night Restrictions

Some hobbies may have time tags:
- daytime;
- evening;
- anytime.

Examples:
- stargazing: night;
- gardening: day;
- loud music: restricted during sleep hours if shelter rules support it.

Keep this data-driven and optional.

---

## 129. Lighting and Power Dependencies

Some hobbies may require:
- light;
- powered equipment;
- music player;
- workshop tools.

Only reference power if shelter power authority exposes a stable capability.

A blackout should naturally disable affected hobbies without deleting progress.

---

## 130. Facility Damage and Outages

If a room is damaged:
- active session interrupts;
- future sessions blocked;
- group becomes dormant;
- no progress loss.

On repair:
- eligibility returns automatically.

This should be covered by facility-capability events.

---

## 131. Hobby Items and Keepsakes

If survivors create or collect keepsakes:
- decide whether item enters inventory;
- whether it is marked non-consumable;
- whether it can be lost;
- whether epilogue references it.

Prefer a cultural-fact ledger for non-mechanical artifacts.

---

## 132. Economic Non-Goals

Do not let hobbies become a hidden profit engine.

If a crafted hobby object can be traded:
- route through economy;
- apply explicit value;
- avoid infinite material/value loops;
- cap or rarity-tag outputs.

The primary value remains personal/cultural.

---

## 133. Testing With Large Rosters

Create a stress fixture:
- 100+ survivors;
- 30 hobbies;
- multiple facilities;
- several groups.

Measure:
- daily selection time;
- allocations;
- deterministic ordering.

The system must avoid O(survivors × hobbies × facilities × groups) where simple indexing can reduce cost.

---

## 134. Candidate Indexing

Pre-index definitions by:
- category;
- facility;
- affinity tag;
- material requirement.

For a survivor, first build a small candidate set.

This keeps selection cheap.

---

## 135. Save Footprint

Persist only meaningful progress.

Avoid storing a zero-value `HobbyProgress` row for every survivor × hobby pair.

Create rows when:
- interest selected;
- session completed;
- proficiency nonzero;
- group relationship requires it.

This prevents save bloat.

---

## 136. Catalog Schema Version Migration

If hobby schema evolves:

v1 → v2 migration must:
- preserve hobby IDs;
- preserve proficiency;
- map mastery from proficiency;
- default new fields safely;
- not reroll interests.

Test serialized v1 fixtures.

---

## 137. Headless Simulation Scenario

Create a 30-day deterministic leisure simulation.

Inputs:
- fixed survivor roster;
- fixed schedules;
- fixed facilities;
- fixed resources;
- fixed seed.

Outputs:
- sessions per survivor;
- selected hobbies;
- mastery changes;
- morale events;
- groups formed.

Run twice and assert identical digest.

---

## 138. Digest for Regression

Generate a hobby simulation digest from normalized state:
- sorted progress;
- sorted groups;
- milestone IDs.

Use it in `--hobbies-selftest`.

This catches subtle ordering regressions.

---

## 139. Cross-System Acceptance Scenario — Crisis Recovery

Scenario:
- survivor experiences morale decline;
- work pressure eases;
- free-time window opens;
- survivor chooses favorite hobby;
- session completes;
- NeedsSystem receives bounded morale event;
- MentalHealth system receives protective fact.

Expected:
- no direct morale mutation in HobbyState;
- one session event;
- deterministic result;
- no cure/instant reset.

---

## 140. Cross-System Acceptance Scenario — Shared Music Group

Scenario:
- three survivors share music interest;
- suitable room and instrument capability exist;
- schedules align;
- repeated sessions occur.

Expected:
- group forms once;
- relationship events route externally;
- performance can become eligible;
- group persists through save/load;
- one member death removes that member without corrupting group.

---

## 141. Cross-System Acceptance Scenario — Workshop Scarcity

Scenario:
- woodworking hobby competes with production use of workshop;
- facility capacity is full during duty time.

Expected:
- hobby waits for valid free slot;
- no duplicate workshop capacity;
- survivor does not abandon assigned production duty;
- hobby resumes later.

---

## 142. Cross-System Acceptance Scenario — Old Save

Scenario:
- campaign save predates HobbyState;
- roster has authored traits.

Expected:
- empty progress;
- deterministic preferences generated;
- no retroactive sessions/mastery;
- no notification flood;
- save immediately stabilizes interest IDs.

---

## 143. Cross-System Acceptance Scenario — Power Outage

Scenario:
- powered hobby facility becomes unavailable during blackout.

Expected:
- active powered session interrupts/cancels safely;
- no duplicate material cost;
- unpowered hobbies remain available;
- progress persists.

---

## 144. Cross-System Acceptance Scenario — Critical Need

Scenario:
- survivor is scheduled for leisure but hunger becomes critical.

Expected:
- scheduler cancels leisure priority;
- survivor attends need;
- no morale/proficiency completion effect;
- session can be retried later.

---

## 145. Cross-System Acceptance Scenario — Master Teaching

Scenario:
- master and apprentice share hobby;
- facility/time available.

Expected:
- teaching session selected;
- learner gets bounded accelerated proficiency;
- teacher receives social/morale effect;
- apprenticeship/relationship events route correctly;
- no duplicate progression on load.

---

## 146. Release Gate

Release should fail if:

- hobby catalog invalid;
- a required facility ID no longer resolves;
- an item ID disappears;
- a mastery range overlaps;
- `--hobbies-selftest` fails;
- old-save migration fails;
- UI depends directly on `NeedsSystem`/skills for hobby rule calculation;
- deterministic 30-day digest changes without approved baseline update.

---

## 147. Documentation Set

Create:

- `docs/hobbies/HOBBY_INTEGRATION_AUDIT.md`
- `docs/hobbies/HOBBY_SCHEMA.md`
- `docs/hobbies/HOBBY_COVERAGE.md`
- `docs/hobbies/HOBBY_PROGRESSION_CALIBRATION.md`
- `docs/hobbies/HOBBY_MIGRATION.md`
- `docs/hobbies/HOBBY_AUTHORING_GUIDE.md`

The authoring guide should state:
- how to add a hobby;
- valid categories;
- valid facilities;
- material policy;
- affinity tags;
- skill hook rules;
- balance caps;
- validation commands.

---

## 148. Final Execution Order

Recommended implementation sequence:

```text
audit
 -> contracts/catalog
 -> interest persistence
 -> scheduling eligibility
 -> session lifecycle
 -> proficiency/mastery
 -> morale/skill integrations
 -> facilities/materials
 -> social/group/teaching
 -> events/culture
 -> UI
 -> 30-hobby catalog expansion
 -> migration
 -> selftests/fuzz/perf
 -> documentation
```

Do not author all 30 hobbies before the session and facility contracts are proven.

---

## 149. Final Acceptance Statement

Plan 161 is successful when the player can observe survivors using free time in ways that feel consistent with
their personalities, those choices persist across the campaign, the activities create modest mechanical and
social benefits, the shelter accumulates memorable cultural texture, and none of this requires the player to
operate another daily work queue.

The engineering proof is equally important: interests are deterministic, sessions are idempotent, facilities
and materials are real, downstream state remains owned by existing systems, old saves remain valid, and
headless CI can replay the same leisure simulation to the same digest.
