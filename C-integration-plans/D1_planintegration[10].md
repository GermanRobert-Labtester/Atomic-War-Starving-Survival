# D1 Flagship Integration Plan [10]
## Plan 170 — Seasonal Events & Celebrations System

> **Purpose:** Give ASHFALL's shelter life temporal rhythm by turning calendar progression into recurring,
> data-driven occasions that survivors may celebrate, remember, reinterpret, or deliberately ignore.
>
> **Primary source:** Plan 170 — Seasonal Events & Celebrations System.
>
> **Core design problem:** the campaign calendar advances, morale exists, memorials exist, and feedback strings
> already imply that celebrations should matter, but there is no authoritative event calendar, no celebration
> transaction, no anniversary tracking, no recurring traditions, and no system that marks the passage of time
> with communal meaning.
>
> **Implementation posture:** optional, deterministic, low-micromanagement, data-driven, save-compatible,
> culturally coherent, resource-aware, and integrated through existing calendar, needs, memorial, relationship,
> shelter identity, hobby/leisure, and archive authorities rather than duplicating them.
>
> **Critical guardrail:** celebrations must not become a compulsory morale tax or another production queue. The
> system should create relief, memory, and shelter identity—not punish players every few days for skipping a
> festival while starving.

---

## 1. Source Problem Statement

The source plan identifies a particularly visible architecture gap:

- `CampaignCalendar.cs` tracks time;
- `NeedsSystem.cs` tracks morale;
- `MemorialSystem.cs` or equivalent tracks survivor deaths;
- feedback strings already mention celebrations as morale-relevant;
- yet no celebration system, seasonal event authority, holiday calendar, anniversary registry, festival
  transaction, or shelter tradition state exists.

The result is a survival campaign where time passes mechanically but not culturally.

Plan 170 should introduce this authoritative flow:

```text
CampaignCalendar
      ↓
SeasonalEventSystem
      ↓
eligible seasonal / anniversary / tradition events
      ↓
player response or automatic remembrance
      ↓
celebration transaction
  ├─ scale
  ├─ activities
  ├─ participants
  ├─ resource reservation
  └─ deterministic outcomes
      ↓
typed consequences
  ├─ NeedsSystem
  ├─ SurvivorRelationsSystem
  ├─ MemorialSystem
  ├─ HobbySystem
  ├─ ShelterIdentity
  ├─ ShelterArchive
  └─ Journal / achievements / epilogue hooks
```

The event system owns the temporal occasion and celebration record. It does not own morale, relationships,
inventory, memorial truth, or identity systems.

---

## 2. Flagship Success Criteria

The work is complete only when all of the following are true:

1. `SeasonalEventSystem.cs` exists with schema-versioned capture/restore.
2. Seasonal events are defined in a canonical data catalog.
3. Calendar triggering uses the existing campaign calendar rather than a second clock.
4. Events trigger once per intended recurrence.
5. Anniversary recurrence is deterministic.
6. Founding date and memorial reference dates are persisted correctly.
7. Traditions persist across years within one campaign.
8. Celebration resource costs use canonical inventory transactions.
9. Celebration morale effects route to `NeedsSystem`.
10. Social effects route to the canonical survivor-relations system.
11. Memorial outcomes route through memorial/fate authorities.
12. Hobby/leisure activities can participate without duplicating Plan 161 state.
13. Large/memorable events can be archived without making every routine event an archive record.
14. Player can ignore or scale down events without catastrophic penalties.
15. Celebration costs cannot be duplicated by save/load.
16. Event triggering does not depend on UI scenes.
17. Old saves initialize safely with a deterministic founding reference.
18. Calendar years/seasons are derived from actual project rules.
19. Seasonal event IDs, activity IDs, item IDs, localization keys, and integration hooks validate.
20. `--seasonal-events-selftest` proves recurrence, celebration transactions, anniversaries, traditions,
    save/load, no-event campaigns, and headless operation.
21. Long campaigns do not produce uncontrolled event spam.
22. Celebration morale is bounded and cannot trivialize systemic crises.
23. Skipping an event is a meaningful choice, not a mandatory failure state.
24. No fixed "Day 90 = spring" rule ships until checked against the actual campaign calendar/season authority.

---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/seasonal_events/SEASONAL_EVENTS_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs`
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`
- `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs`
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`
- shelter archive systems;
- shelter identity/culture systems;
- Plan 161 hobby/leisure state if implemented;
- survivor relationship system;
- inventory transaction APIs;
- food/fuel/material catalogs;
- shelter founding/start-day authority;
- season/weather system;
- nuclear winter progression;
- journal/event-log infrastructure;
- quest runtime;
- save schema/migration;
- seeded RNG;
- UI calendar/panel architecture;
- localization catalogs;
- achievement system;
- unified ending/epilogue system.

For each dependency, record:

| Concern | Canonical owner | Persisted? | Event/API | Writable by Plan 170? |
|---|---|---:|---|---:|
| campaign day | | | | no |
| morale | | | | through sink |
| inventory | | | | transaction only |
| survivor death date | | | | read only |
| relationships | | | | event only |
| shelter identity | | | | event/tag only |
| archive | | | | event only |

No new event date system should be written until the actual calendar's notion of year, season, and day is confirmed.

---

## 4. Scope Boundary

### In scope

- data-driven fixed/seasonal events;
- founding anniversary;
- survivor memorial anniversaries;
- achievement/milestone anniversary hooks where supported;
- shelter-created traditions;
- small/medium/large celebration scales;
- celebration activity catalog;
- participant selection;
- resource costs;
- deterministic celebration results;
- bounded morale/social effects;
- memorable celebration archive facts;
- UI calendar and celebration planner;
- save/load;
- old-save migration;
- CI selftest.

### Out of scope for first pass

- cross-campaign traditions;
- profile/meta rewards;
- permanent festival bonuses;
- large bespoke minigames;
- dynamic real-world holiday synchronization;
- online seasonal events;
- monetized seasonal content;
- real-world calendar dependence;
- full cultural identity simulation;
- unrestricted user-authored holiday scripting.

These can layer on later.

---

## 5. Canonical Event Definition

Recommended:

```csharp
public sealed record SeasonalEventDefinition
{
    public string EventId { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }
    public string EventType { get; init; }
    public SeasonalEventTrigger Trigger { get; init; }
    public int DurationDays { get; init; }

    public IReadOnlyList<string> AllowedActivityIds { get; init; }
    public IReadOnlyList<string> DefaultActivityIds { get; init; }

    public CelebrationPolicy CelebrationPolicy { get; init; }
    public string? ArchiveTag { get; init; }
    public string? IdentityTag { get; init; }
}
```

Do not persist event definitions in campaign saves; persist occurrence state.

---

## 6. Event Types

Retain the source taxonomy:

- holiday;
- anniversary;
- festival;
- tradition;
- memorial.

Treat these as semantic categories, not separate runtime systems.

---

## 7. Trigger Model

A trigger should be declarative.

Examples:

```json
{
  "kind": "day_of_year",
  "day": 90
}
```

```json
{
  "kind": "season_boundary",
  "seasonId": "spring"
}
```

```json
{
  "kind": "anniversary",
  "referenceKind": "shelter_founding"
}
```

```json
{
  "kind": "phase_entered",
  "phaseId": "nuclear_winter"
}
```

Do not encode triggers as arbitrary executable expressions.

---

## 8. Calendar Authority

Before using the source's example days 1/90/180/270/360, determine:

- length of campaign year;
- whether seasons are 90-day quarters;
- whether calendar uses day-of-year;
- whether nuclear winter disrupts season semantics;
- whether campaign can exceed one year;
- whether day 1 is already shelter founding.

Use calendar helpers such as:

```text
GetYear(day)
GetDayOfYear(day)
GetSeason(day)
```

from canonical authority or create read-only adapter if missing.

---

## 9. Source Event Set

Initial source set:

- New Year;
- Spring Festival;
- Midsummer Feast;
- Harvest Festival;
- Winter Solstice;
- Shelter Founding;
- Remembrance.

However, these names/dates should be setting-coherent.

Avoid importing modern holiday assumptions unless ASHFALL's fiction supports them.

Prefer diegetic names where appropriate:
- First Light;
- Thaw;
- Long Day;
- Stores Reckoning;
- Long Night;
- Founding Day;
- Remembrance.

---

## 10. Annual Recurrence Identity

Each recurrence needs a stable occurrence ID:

```text
<eventId>:year:<yearIndex>
```

Example:
```text
festival_midwinter:year:2
```

This prevents duplicate triggers after save/load.

---

## 11. Anniversary Identity

Example:

```text
anniversary:shelter_founding:year:3
memorial:survivor_anna:year:2
```

Store occurrence IDs, not just last day.

---

## 12. Seasonal Event State

Recommended:

```csharp
public sealed record SeasonalEventState
{
    public int SchemaVersion { get; init; }
    public int FoundingDay { get; init; }

    public IReadOnlySet<string> TriggeredOccurrenceIds { get; init; }
    public IReadOnlyList<CelebrationRecord> Celebrations { get; init; }
    public IReadOnlyList<TraditionState> Traditions { get; init; }
    public IReadOnlySet<string> ArchivedMilestoneIds { get; init; }

    public int? LastCelebrationDay { get; init; }
}
```

Do not persist duplicated catalog definitions.

---

## 13. Celebration Record

```csharp
public sealed record CelebrationRecord
{
    public string CelebrationId { get; init; }
    public string EventOccurrenceId { get; init; }
    public int Day { get; init; }
    public CelebrationScale Scale { get; init; }
    public IReadOnlyList<string> ParticipantIds { get; init; }
    public IReadOnlyList<string> ActivityIds { get; init; }
    public IReadOnlyList<ItemCost> ResourcesSpent { get; init; }
    public int MoraleEffectMagnitude { get; init; }
    public bool Memorable { get; init; }
}
```

Persist structural outcome, not only rendered text.

---

## 14. Celebration Scale

Retain:

- small;
- medium;
- large.

Do not hardcode source costs like "5 food + 2 fuel per participant" globally.

Instead define:
- scale multipliers;
- per-activity costs;
- optional participant scaling.

This is easier to balance and works across different activities.

---

## 15. Celebration Scale Philosophy

Small:
- cheap;
- intimate;
- reliable;
- low management overhead.

Medium:
- meaningful community gathering;
- moderate resources;
- better participation.

Large:
- rare;
- high cost;
- major cultural significance;
- potentially memorable/archive-worthy.

Large should not always be optimal.

---

## 16. The Existing Hint: Small vs Large Celebrations

The source specifically notes a feedback hint stating small celebrations can boost morale more than large ones.

Treat this as a design clue that "scale" should not map linearly to efficiency.

Possible rule:
- small celebrations have high morale-per-resource;
- large celebrations have higher total communal/cultural impact;
- large events are memorable/tradition-building.

This preserves strategic choice.

---

## 17. Activity Catalog

Create a separate activity data authority inside or alongside `seasonal_events.json`.

Source activities:

- feast;
- music;
- stories;
- games;
- memorial;
- dance;
- gifts.

Each activity defines:
- allowed event types;
- cost profile;
- participant bounds;
- effect tags;
- facility requirements if any;
- hobby integration tag;
- relationship effect profile.

---

## 18. Feast Activity

Inputs:
- food resource transaction;
- food quality/category if canonical;
- participants.

Effects:
- morale event;
- optional relationship/social event;
- archive tag for exceptional feast.

Do not invent food-quality rules if no canonical quality exists.

---

## 19. Music Activity

Prefer Plan 161 integration:

```text
music hobbyists / instruments / performance group
 -> celebration music activity quality
```

Do not consume fuel simply because the source example says so unless actual lighting/equipment requires power/fuel.

Use real shelter capabilities.

---

## 20. Stories Activity

Low-cost / no-resource activity.

Effects:
- modest morale;
- relationship event;
- optional archive/hobby storytelling fact.

Useful during scarcity so celebration remains possible without food waste.

---

## 21. Games Activity

Can use:
- Plan 161 social hobbies;
- small morale;
- friendly competition event;
- tiny skill practice only if canonical skill hooks exist.

No separate skill system.

---

## 22. Memorial Activity

Memorial is not just a "negative morale" activity.

Potential mixed effects:
- short-term sadness;
- comfort/belonging;
- relationship reflection;
- memorial archive fact.

Route through memorial/needs systems with explicit effect profile.

---

## 23. Dance Activity

Effects:
- morale;
- social;
- fatigue cost or movement activity.

The source says fatigue reduction, but dancing may logically increase fatigue. Audit tone/system semantics before
implementing that literal.

Do not blindly encode contradictory effects.

---

## 24. Gifts Activity

Uses canonical item transactions.

Must define:
- allowed gift item categories;
- giver/receiver selection;
- relation event.

Do not allow transfer of quest-critical or equipped items unintentionally.

---

## 25. Celebration Planning Transaction

Flow:

```text
select event occurrence
 -> choose scale
 -> choose activities
 -> determine eligible participants
 -> preview required resources
 -> validate facilities
 -> reserve/commit resources
 -> create celebration
 -> apply deterministic participation
 -> compute outcome
 -> persist record
 -> emit downstream events
```

UI never directly mutates inventory or morale.

---

## 26. Resource Reservation

Celebration should use canonical transaction semantics.

Prevent:
- duplicate cost after reload;
- negative inventory;
- partial cost consumption when planning fails.

Use one transactional request if inventory supports it.

---

## 27. Celebration Cancellation

Before commit:
- no cost;
- no record.

After resources committed:
- cancellation policy depends on event start state.

Do not let player repeatedly commit/cancel to reroll participation.

---

## 28. Deterministic Seed

Use `ISeededRng` only for bounded variation.

Seed:

```text
campaignSeed
+ eventOccurrenceId
+ celebrationId
```

Persist celebration ID.

Do not use wall-clock time.

---

## 29. Participation Eligibility

A survivor may participate only if:

- alive;
- present at shelter;
- not incapacitated;
- not on expedition;
- not assigned to emergency duty;
- celebration timing does not conflict with critical needs;
- activity requirements permit them.

Use scheduler/availability authority.

---

## 30. Participation Preference

Source suggests morale and relationships influence attendance.

Recommended deterministic scoring:

```text
attendanceScore =
  eventInterest
  + relationship/group affinity
  + tradition familiarity
  + favorite activity affinity
  - severe fatigue
  - severe morale withdrawal
  - feud avoidance
```

Then deterministic threshold/seed.

Do not randomly exclude survivors for flavor every load.

---

## 31. Opt-Out

Some survivors may skip.

This is personality texture, not punishment.

Record attendance only when useful to:
- relationship events;
- tradition history;
- journal notable cases.

Avoid huge save arrays for every trivial event if unnecessary.

---

## 32. Participation Bonuses

High participation can improve communal impact.

Cap:
- no exponential scaling;
- large roster should not generate huge morale.

Use normalized participation rate:
```text
participants / eligibleParticipants
```

---

## 33. Morale Authority

SeasonalEventSystem emits:

```text
CelebrationMoraleEffect
```

NeedsSystem applies actual morale change.

Store only the effect magnitude/result for historical record if needed.

Do not duplicate survivor morale in seasonal state.

---

## 34. Morale Cap

Celebrations should not trivialize depression, starvation, trauma, or severe crisis.

Add:
- per-event cap;
- per-day celebration cap;
- diminishing returns if events occur too close together.

Make large celebrations emotionally meaningful but mechanically bounded.

---

## 35. Ignoring Events

Source suggests slight morale penalty.

Refine:

Not every ignored event should punish.

Suggested:
- fixed seasonal festivals: usually no direct penalty;
- established tradition with strong streak: small disappointment;
- founding anniversary: mild effect only if shelter culturally values it;
- memorial: no generic penalty; memorial dynamics may be personal.

This prevents "calendar chore tax."

---

## 36. Celebration Cooldown

Prevent spam outside calendar events.

If ad-hoc celebrations are allowed:
- minimum interval;
- morale diminishing returns;
- resource cost;
- optional morale threshold requirement.

Seasonal events already provide natural cadence.

---

## 37. Anniversary Model

Anniversary definition references:
- type;
- original day;
- reference ID;
- recurrence policy.

Types:
- founding;
- memorial;
- achievement/milestone if real.

Do not store duplicate survivor names; resolve localized/display names from canonical records.

---

## 38. Founding Anniversary

Determine founding day from campaign/shelter creation authority.

Old saves:
- use campaign start day or migration rule;
- document ambiguity;
- persist once.

Do not assume day 1 if campaigns can begin later.

---

## 39. Memorial Anniversaries

Use survivor death date from MemorialSystem/fate authority.

For each death:
- register reference fact;
- first anniversary occurs after one full calendar year, not after arbitrary 365 if year length differs;
- handle multiple deaths same day by grouping if appropriate.

---

## 40. Memorial Grouping

A long campaign may have many deceased survivors.

Do not generate 30 separate popups on one day.

Group:
- "Remembrance Day" with multiple names;
- one memorial event occurrence containing several references.

Preserve individual identity inside memorial UI/journal.

---

## 41. Major Anniversary Multipliers

Source suggests 1st, 5th, 10th, 25th.

Campaign length may never reach 25 years.

Validate expected campaign horizon.

Use configurable milestone years:
```json
[1, 3, 5, 10]
```
if more realistic.

---

## 42. Tradition Contract

```csharp
public sealed record TraditionState
{
    public string TraditionId { get; init; }
    public string Name { get; init; }
    public int ReferenceDayOfYear { get; init; }
    public IReadOnlyList<string> ActivityIds { get; init; }
    public int EstablishedYear { get; init; }
    public int CurrentStreak { get; init; }
    public int TimesCelebrated { get; init; }
    public bool Active { get; init; }
}
```

If names are user-entered, sanitize and persist safely.

---

## 43. Establishing Traditions

Do not allow arbitrary unlimited traditions.

Require:
- a memorable celebration;
- or repeated similar event;
- or player explicit establishment after qualifying celebration.

Cap active traditions.

This keeps the calendar readable.

---

## 44. Tradition Streak

A streak increments when:
- tradition becomes due;
- qualifying celebration occurs within allowed window.

Breaks when:
- due window closes without celebration;
- tradition explicitly retired.

Do not reset because the player celebrated one day early/late if allowed window exists.

---

## 45. Tradition Window

Allow:
- exact day;
- ±1 or ±2 days depending calendar design.

This reduces arbitrary failure when emergencies occur on the exact date.

---

## 46. Breaking Tradition Consequences

Use mild disappointment, not severe morale collapse.

The cultural meaning can be recorded:
- streak ended;
- journal entry;
- identity tag removed/changed.

This is more interesting than raw punishment.

---

## 47. Retiring Traditions

Player can intentionally retire a tradition.

Different from forgetting it.

Possible:
- no disappointment;
- archive note;
- identity update.

Useful when campaign circumstances change.

---

## 48. Shelter Identity Integration

Plan 166 or actual identity system receives:
- active tradition IDs;
- oldest tradition;
- major celebrations;
- memorial customs.

SeasonalEventSystem does not own aggregate shelter identity score.

---

## 49. Shelter Archive Integration

Plan 162 or archive system can record:

- first celebration;
- first founding anniversary;
- first memorial;
- largest celebration;
- longest tradition streak;
- notable festival under crisis conditions.

Do not archive every routine annual event.

---

## 50. Hobby System Integration

Plan 161 can provide activity quality/participation:

- music hobbyists;
- storytellers;
- game groups;
- cooks;
- dancers;
- artists.

SeasonalEventSystem asks for capability facts, not hobby progress mutation.

HobbySystem remains owner.

---

## 51. Culture Feedback Loop

Healthy integration:

```text
hobbies create performers/groups
 -> celebrations provide venue
 -> memorable celebration creates tradition
 -> tradition becomes shelter identity
 -> epilogue remembers tradition
```

This is richer than raw morale arithmetic.

---

## 52. Relationship Integration

Activities may emit:
- shared positive event;
- gift exchange;
- memorial comfort;
- friendly competition;
- attendance conflict.

Relations system decides score changes.

---

## 53. Memorial System Integration

Seasonal events read:
- deceased survivor IDs;
- death dates;
- memorial records.

They do not duplicate death state.

---

## 54. Weather / Season Integration

Seasonal events can require:
- season;
- weather-safe shelter condition;
- phase.

Example:
Midsummer outdoors only if weather/hazard permits.

If not, alternate indoor form.

Do not cancel major communal rhythm because weather event logic is missing; support fallback activity sets.

---

## 55. Nuclear Winter Integration

A nuclear-winter campaign may make old agricultural holidays tonally different.

Options:
- rename/transform events;
- use phase-specific variants;
- reduce outdoor activities;
- create Long Night/First Thaw variants.

Do not treat seasonal data as purely cosmetic if world season has radically changed.

---

## 56. Event Variant System

Definition can declare variants:

```json
{
  "variants": [
    {
      "when": { "phase": "nuclear_winter" },
      "titleKey": "...",
      "activityOverrides": []
    }
  ]
}
```

Keep conditions finite and validated.

---

## 57. Event Notification

When event becomes active:
- show notification;
- add to calendar;
- do not force modal during emergency;
- player can open planner when safe.

Use canonical notification/guidance framework.

---

## 58. Celebration Planning UI

Display:
- event;
- due window;
- scale;
- activities;
- eligible participants;
- projected resource cost;
- expected benefit band;
- cultural/tradition effects;
- shortages.

No hidden cost after confirmation.

---

## 59. Calendar UI

Show:
- upcoming seasonal events;
- founding anniversary;
- grouped memorial dates;
- traditions;
- due/active windows;
- previous memorable events.

Avoid showing every old routine event forever.

---

## 60. Tradition UI

Allow:
- inspect;
- name;
- activity pattern;
- next recurrence;
- streak;
- retire.

Do not allow date collisions without warning.

---

## 61. Accessibility

Support:
- keyboard navigation;
- controller if globally supported;
- text scale;
- scale/benefit not color-only;
- activity requirements readable;
- no timed modal response;
- reduced-motion celebration animations;
- calendar readable without hover-only details.

---

## 62. Journal Integration

Journal notable entries:
- memorable celebration;
- first founding anniversary;
- grouped remembrance;
- tradition established;
- tradition retired;
- major streak milestone.

Routine annual celebration may use compact history rather than narrative entry.

---

## 63. Achievement Integration

Plan 149 may observe:
- first celebration;
- first tradition;
- all-eligible attendance;
- multi-year streak;
- major memorial;
- grand festival.

SeasonalEventSystem does not own achievements/rewards.

---

## 64. Epilogue Integration

Plan 145 may consume:
- oldest tradition;
- notable celebration;
- longest streak;
- remembrance custom;
- cultural identity tags.

Export stable fact IDs, not prose.

---

## 65. Quest Hooks

Source hooks:
- First Celebration;
- Tradition;
- Streak;
- Grand Festival;
- Remembrance;
- Community;
- Legacy.

Expose events:
- event due;
- celebration held;
- tradition established;
- tradition streak reached;
- memorial held.

Quest runtime owns quest state.

---

## 66. Save Schema

Persist:
- founding day;
- triggered occurrence IDs;
- celebration records;
- traditions;
- milestone/archive emission IDs;
- active event windows if needed.

Do not persist:
- duplicated event definitions;
- current morale;
- inventory balances;
- survivor relationship scores.

---

## 67. Old-Save Migration

Source says default state/no traditions/founding day set.

Migration:
1. detect missing seasonal block;
2. derive founding day from canonical campaign start;
3. triggered occurrences empty;
4. no celebrations/traditions;
5. register memorial references from existing deaths only if recurrence can be derived safely;
6. do not retroactively trigger past holidays.

This avoids notification floods.

---

## 68. Mid-Celebration Save/Load

If celebrations are instantaneous transactions:
- no mid-state needed.

If multi-hour/day event:
- persist active celebration instance;
- resource commit state;
- participants;
- seed;
- completion event ID.

Test reload exactly once.

---

## 69. Idempotency

Stable IDs:

```text
event occurrence: seasonal:<eventId>:<year>
celebration: celebration:<campaign>:<occurrence>:<sequence>
tradition recurrence: tradition:<traditionId>:<year>
```

Use to prevent duplicate trigger/effect.

---

## 70. Resource Exploit Prevention

Prevent:
- repeated confirm/cancel refunds creating resources;
- save/load duplicate cost;
- gifts using same item twice;
- feast spending negative stock;
- celebration preview mutating inventory.

Use transaction IDs.

---

## 71. Morale Exploit Prevention

Prevent:
- multiple celebrations same day;
- triggering same occurrence twice;
- establishing traditions from one repeated UI action;
- huge roster multiplication;
- stacking all activities beyond cap.

Use per-celebration effect budget.

---

## 72. Event Spam Prevention

Long campaign with many memorials/traditions can overwhelm player.

Mitigations:
- group memorials;
- cap active traditions;
- event windows;
- priority categories;
- calendar digest/summary;
- auto-resolve low-priority reminders if player chooses.

No dozens of popups at midnight.

---

## 73. Optional Auto-Celebration Policy

Potential quality-of-life setting:

```text
Traditions:
[ Ask me ] [ Auto small ] [ Never auto ]
```

If implemented:
- deterministic;
- never overspend below configured reserve;
- uses only allowed activities.

May be follow-on if policy system absent.

---

## 74. Resource Reserve Safety

Auto/AI celebration must respect survival reserves.

Define optional minimum reserves:
- food;
- fuel;
- medicine not spendable;
- critical quest items forbidden.

Manual celebration can warn before crossing reserves.

---

## 75. Celebration Quality

Avoid one opaque quality number unless useful.

Can derive from:
- scale;
- participation;
- activity variety;
- facility/hobby support;
- shortages.

Quality affects:
- morale efficiency;
- memorable flag;
- tradition eligibility.

Keep bounded.

---

## 76. Memorable Celebration Criteria

Example:

```text
large scale
OR high participation + multiple activities
OR first major festival after severe crisis
OR significant tradition milestone
```

Do not mark every large event automatically if resources alone buy "memory."

---

## 77. Archive Threshold

Persist `Memorable=true` structurally.

Archive system decides storage/presentation.

This preserves separation.

---

## 78. Event Catalog

Create:

`Assets/StreamingAssets/Data/seasonal_events.json`

Suggested root:

```json
{
  "schemaVersion": 1,
  "events": [],
  "activities": [],
  "scales": [],
  "anniversaryMilestones": []
}
```

---

## 79. Catalog Validation

Validate:
- unique IDs;
- valid event type;
- trigger kind;
- legal day/season;
- localization;
- activity IDs;
- item cost IDs;
- effect profile IDs;
- no negative duration;
- no duplicate recurrence trigger;
- anniversary milestone ordering;
- no impossible activity requirement.

---

## 80. Event Reachability

Generate report:
`docs/seasonal_events/SEASONAL_EVENT_COVERAGE.md`

For each event:
- trigger reachable in normal campaign?
- year required?
- season/phase?
- required resources?
- valid activities?
- fixture?

Flag fifth/tenth-year content if campaign cannot reach it.

---

## 81. Progression Calibration

Simulate:
- normal campaign duration;
- events/year;
- expected celebration count;
- expected resource spend;
- morale contribution.

Reject tuning where:
- festivals consume an excessive fraction of food;
- player receives major morale every few days;
- only large celebrations are worthwhile.

---

## 82. No-Celebration Campaign

Must remain playable.

Expected:
- event reminders may occur;
- no mandatory deadlock;
- only mild cultural consequences where justified;
- no systematic morale collapse.

This is a key anti-chore test.

---

## 83. Scarcity Campaign

Scenario:
- severe food/fuel shortage.

System should offer:
- stories;
- memorial;
- simple games;
- other low-cost activities.

Celebration should adapt rather than say "you need 50 food or everyone is sad."

---

## 84. High-Abundance Campaign

Large feasts/complex events become possible.

Still:
- bounded morale;
- memorable/cultural value becomes main distinction;
- no infinite resource-to-morale conversion.

---

## 85. Emergency Collision

If raid/fire occurs on festival day:
- event window remains open;
- no forced immediate modal;
- celebration can be delayed within window;
- missing exact moment does not automatically break tradition.

This creates humane scheduling.

---

## 86. Death on Celebration Day

Memorial and festival may collide.

Use category ordering:
- immediate death/fate event has priority;
- event UI can acknowledge mixed tone;
- do not create contradictory "everyone rejoiced" prose.

Structural event system should permit mixed outcomes.

---

## 87. Event Composition Rules

Multiple due events can combine if compatible:
- Founding + seasonal festival;
- several memorials grouped.

Do not merge incompatible:
- solemn memorial + exuberant party unless player intentionally chooses mixed remembrance celebration.

Use explicit compatibility tags.

---

## 88. Deterministic Participation Test

Given same:
- eligible survivors;
- relationships;
- morale;
- hobby affinities;
- event seed;

attendance must be identical after reload.

---

## 89. Performance Budget

Seasonal event checks happen once per day tick.

Requirements:
- no per-frame scanning;
- fixed event definitions indexed by trigger;
- anniversaries indexed by day-of-year;
- traditions indexed by due window;
- participant calculation only when event planned.

Long campaigns remain cheap.

---

## 90. Save Footprint

Avoid storing every ignored event forever if occurrence ID set becomes huge.

Possible:
- compact per-event last-triggered year for fixed annual events;
- explicit records for celebrations/traditions;
- memorial recurrence cursor.

Choose schema that scales.

---

## 91. Structured Diagnostics

Logs:

```text
SeasonalEventTriggered occurrence=<id> day=<n>
CelebrationPlanned id=<id> scale=<scale>
CelebrationCommitted id=<id> participants=<n>
TraditionEstablished id=<id>
TraditionStreakAdvanced id=<id> streak=<n>
AnniversaryTriggered id=<id>
```

No spam for calendar checks with no event.

---

## 92. Data Integrity Self-Test

Extend standard data integrity:

- event IDs;
- activity IDs;
- item references;
- localization;
- trigger validity;
- milestone values;
- effect profiles;
- facility/hobby integration references;
- archive/identity tags if registry exists.

---

## 93. Dedicated `--seasonal-events-selftest`

Selftest should:

1. load catalog;
2. create fixed calendar;
3. trigger first seasonal event;
4. verify once-only occurrence;
5. plan small celebration;
6. validate resource transaction;
7. validate morale event;
8. save/reload;
9. trigger founding anniversary;
10. register memorial anniversary;
11. establish tradition;
12. advance recurrence year;
13. verify streak;
14. skip recurrence and verify bounded break behavior;
15. verify no-event campaign path;
16. verify scarcity low-cost activity path;
17. verify headless operation;
18. exit non-zero on mismatch.

---

## 94. Unit Test Matrix

### Catalog
- valid event;
- duplicate event;
- invalid trigger;
- invalid item;
- invalid activity;
- invalid milestone.

### Calendar
- due;
- not due;
- recurrence;
- year boundary;
- season boundary.

### Celebration
- small;
- medium;
- large;
- insufficient resources;
- mixed activities;
- participation.

### Anniversary
- founding;
- memorial;
- multiple memorials;
- major milestone.

### Tradition
- establish;
- recur;
- streak;
- grace window;
- skip;
- retire.

### Persistence
- empty;
- triggered occurrence;
- celebration;
- tradition;
- old save.

---

## 95. Golden Scenarios

1. First New Year/seasonal event.
2. Small scarcity celebration using stories.
3. Medium feast with moderate roster.
4. Large memorable festival.
5. Founding anniversary.
6. Single survivor memorial anniversary.
7. Multiple deaths grouped into remembrance.
8. Tradition established and repeated.
9. Tradition missed during emergency but still within grace window.
10. Tradition truly broken.
11. Nuclear-winter event variant.
12. No celebration for entire campaign.
13. Old save migration.
14. Save/load after planning but before completion if multi-stage.
15. Large roster participation scaling.

---

## 96. Fuzz / Property Testing

Properties:
- each occurrence triggers at most once;
- tradition streak never negative;
- resource spend never exceeds committed inventory;
- participants all exist/eligible at commit;
- morale effect bounded;
- event recurrence ordering deterministic;
- no invalid activity IDs;
- same saved state produces same attendance/result.

---

## 97. Implementation Phase A — Audit and Calendar Contract

Tasks:
1. inspect calendar/year/season;
2. inspect feedback hints;
3. inspect founding/death dates;
4. create audit;
5. define event/trigger DTOs;
6. create catalog;
7. loader/validator.

Exit:
fixed trigger evaluation works headlessly.

---

## 98. Implementation Phase B — Event Occurrence Engine

Tasks:
1. annual recurrence;
2. occurrence IDs;
3. active windows;
4. once-only state;
5. season/phase conditions;
6. notifications;
7. tests.

Exit:
calendar produces deterministic events.

---

## 99. Implementation Phase C — Celebration Transaction

Tasks:
1. scale profiles;
2. activity catalog;
3. planning DTO;
4. participant eligibility;
5. resource preview;
6. transaction;
7. outcome;
8. morale/relationship sinks;
9. idempotency.

Exit:
player can hold a real small/medium/large celebration.

---

## 100. Implementation Phase D — Anniversaries

Tasks:
1. founding reference;
2. death reference;
3. recurrence;
4. grouping;
5. milestone years;
6. journal;
7. mixed morale profile;
8. tests.

Exit:
campaign history is remembered over time.

---

## 101. Implementation Phase E — Traditions

Tasks:
1. qualifying celebration;
2. establish;
3. due date/window;
4. streak;
5. break/retire;
6. identity/archive facts;
7. tests.

Exit:
shelter-specific recurring culture exists.

---

## 102. Implementation Phase F — Hobby/Culture Integration

Tasks:
1. music/story/game activity quality;
2. Plan 161 hobby participants;
3. relation events;
4. archive;
5. identity;
6. quest hooks;
7. epilogue/achievement fact exports.

Exit:
celebrations feel connected to shelter life rather than isolated morale buttons.

---

## 103. Implementation Phase G — UI

Tasks:
1. calendar panel;
2. due-event notifications;
3. celebration planner;
4. participant view;
5. cost preview;
6. tradition management;
7. anniversary display;
8. accessibility.

Exit:
UI contains no duplicate event logic.

---

## 104. Implementation Phase H — Content Authoring

Tasks:
1. validate setting-appropriate seasonal names;
2. author 5 core seasonal/festival events;
3. founding template;
4. memorial template;
5. activities;
6. phase variants;
7. localization;
8. coverage fixtures.

Exit:
7 source event families are data-backed and reachable.

---

## 105. Implementation Phase I — CI Hardening

Tasks:
1. old save;
2. recurrence;
3. save/load;
4. no-celebration path;
5. scarcity path;
6. fuzz;
7. performance;
8. selftest;
9. coverage report;
10. full regression.

Exit:
seasonal rhythm is deterministic and non-invasive.

---

## 106. Definition of Done — Flagship

### Core
- [ ] `SeasonalEventSystem.cs`
- [ ] event definitions
- [ ] activity definitions
- [ ] occurrence tracking
- [ ] schema-versioned state
- [ ] deterministic trigger evaluation

### Calendar
- [ ] actual year/season rules used
- [ ] annual recurrence
- [ ] founding anniversary
- [ ] memorial anniversary
- [ ] event windows
- [ ] no duplicate triggers

### Celebrations
- [ ] small/medium/large
- [ ] resource transactions
- [ ] participation
- [ ] activity combinations
- [ ] bounded morale
- [ ] memorable events
- [ ] anti-spam

### Traditions
- [ ] establish
- [ ] recur
- [ ] streak
- [ ] grace window
- [ ] break/retire
- [ ] identity/archive export

### Integrations
- [ ] Needs
- [ ] Calendar
- [ ] Memorial
- [ ] Relations
- [ ] Hobby
- [ ] Archive
- [ ] Identity
- [ ] Quest
- [ ] Achievement/Epilogue facts

### UI
- [ ] calendar
- [ ] planner
- [ ] tradition panel
- [ ] anniversary display
- [ ] accessibility
- [ ] no business logic duplication

### Validation
- [ ] old saves
- [ ] data integrity
- [ ] selftest
- [ ] fuzz
- [ ] scarcity
- [ ] no-celebration path
- [ ] headless

---

## 107. Follow-On Task 170-A — Celebration Event Composer

Goal:
allow multiple compatible due occasions to share one celebration.

Substeps:
1. compatibility tags;
2. event merge rules;
3. shared resource plan;
4. mixed tone;
5. archive attribution;
6. deterministic ordering;
7. tests.

---

## 108. Follow-On Task 170-B — Seasonal Competition Framework

Goal:
add occasional contests without turning festivals into stat farming.

Examples:
- cooking;
- games;
- hobby competitions.

Use Plan 161 proficiency and deterministic judging.

---

## 109. Follow-On Task 170-C — Cultural Goods

Goal:
support festival-specific decorative/commemorative items.

Requirements:
- canonical crafting/economy;
- no profile rewards;
- no temporary event-only orphan items;
- save-safe IDs;
- mod compatibility.

---

## 110. Follow-On Task 170-D — Trading and Inter-Settlement Festivals

Goal:
connect allied settlements through event invitations/trade.

Requires:
- communications/outpost/faction systems;
- travel;
- trade scheduling;
- relationship consequences.

Do not add until those integrations exist.

---

## 111. Follow-On Task 170-E — Cross-Campaign Cultural Legacy

Goal:
allow Plan 175 or later meta system to remember famous traditions.

Plan 170 exports stable tradition facts only.

It must not own profile persistence.

---

## 112. Final Guardrails

- No second campaign clock.
- No real-world holiday dependence.
- No per-frame event polling.
- No duplicate event occurrence.
- No duplicate resource cost.
- No UI-owned celebration rules.
- No direct morale authority.
- No direct relationship authority.
- No direct memorial truth duplication.
- No harsh punishment for every skipped event.
- No celebration spam.
- No unlimited tradition count.
- No impossible fifth/tenth-year content without reachability evidence.
- No blindly hardcoded day 90/180/270/360 schedule before calendar audit.
- No mandatory expensive feast when shelter is starving.
- No unbounded morale stacking.
- No retroactive holiday notification flood on old saves.
- No cross-campaign legacy ownership.
- No routine archive/journal spam.

When complete, Plan 170 should make time feel inhabited. The campaign calendar will no longer be only a survival
counter: survivors will recognize seasons, remember the shelter's founding, mourn people they lost, create
traditions, adapt celebrations to scarcity, and occasionally produce moments worth archiving. The system will
support communal life while respecting the realities of survival and the authority of existing morale,
inventory, memorial, relationship, hobby, and identity systems.

---

## Annex A — Detailed Seasonal Event Blueprint

The following blueprints are starting points; final names and dates must be validated against actual ASHFALL
calendar and lore.

### A1. New Year / First Light
Trigger:
- first day of a new campaign year.

Tone:
- hope, reset, survival count.

Allowed activities:
- stories;
- small gifts;
- music;
- modest feast.

Strategic identity:
- low-cost, hopeful, high-efficiency celebration.

### A2. Spring / Thaw Festival
Trigger:
- season transition to spring or equivalent thaw state.

Tone:
- renewal;
- planting;
- repair.

Allowed activities:
- food;
- gardening-linked gathering;
- stories;
- games.

If nuclear winter invalidates spring:
- use phase-specific variant rather than pretending normal weather returned.

### A3. Midsummer / Long Day
Trigger:
- summer midpoint/season event.

Tone:
- community and relative abundance.

Strong fit:
- outdoor activities if safe;
- music;
- dance;
- games.

Fallback:
- indoor celebration during contamination/storm.

### A4. Harvest / Stores Reckoning
Trigger:
- autumn/harvest timing.

Tone:
- gratitude plus preparation.

Can reference:
- agriculture output;
- storage;
- winter readiness.

Do not assume abundance; scarcity variant may become a solemn ration-sharing event.

### A5. Winter Solstice / Long Night
Trigger:
- winter transition or shortest-day equivalent.

Tone:
- endurance;
- memory;
- shelter closeness.

Strong fit:
- storytelling;
- memorial;
- music;
- lights if power/fuel permits.

### A6. Founding Day
Trigger:
- anniversary of canonical shelter founding date.

Recurrence:
- annual.

Effects:
- identity;
- communal pride;
- archive milestones.

Major anniversaries:
- configure based on reachable campaign length.

### A7. Remembrance
Trigger:
- memorial recurrence window.

Content:
- one or multiple deceased survivors.

Effects:
- mixed morale;
- comfort;
- relationship/journal;
- epilogue fact.

Never reduce deceased survivors to a generic count when names are available.

---

## Annex B — Celebration Balance Worksheet

For each event/scale combination, calculate:

| Event | Scale | Participants | Food | Fuel | Other | Expected morale | Morale/resource | Memorable chance/criteria |
|---|---|---:|---:|---:|---:|---:|---:|---|
| First Light | small | | | | | | | |
| First Light | medium | | | | | | | |
| First Light | large | | | | | | | |

Balance rules:
1. Small has strong efficiency.
2. Medium provides best normal communal value.
3. Large provides strongest cultural/memorable outcome but worse resource efficiency.
4. Scarcity variants offer meaningful low-cost options.
5. No scale should invalidate survival-resource planning.

---

## 113. Calendar Boundary and Leap/Irregular-Year Handling

Do not assume the campaign calendar is a fixed 360-day arithmetic loop unless that is the actual authority.

The seasonal event adapter must answer:

- how many days are in a campaign year;
- whether years can differ in length;
- whether the campaign begins at year/day 1;
- whether seasons use fixed days or state transitions;
- whether special world phases alter the calendar;
- whether campaigns can end before a full year.

Use canonical helper methods rather than local modulo arithmetic.

Regression tests:
- final day of year;
- first day of next year;
- event due on final day;
- save/reload across year boundary;
- tradition window crossing year boundary;
- memorial anniversary at year-end.

---

## 114. Event Trigger Precedence

On one day, multiple conditions may become true.

Recommended processing order:

1. world/campaign phase transition;
2. memorial/fate-critical recurrence;
3. founding anniversary;
4. seasonal/festival event;
5. shelter tradition;
6. ad-hoc optional celebration.

This is not narrative priority; it is deterministic processing order.

The player-facing calendar may combine compatible events after trigger evaluation.

---

## 115. Event Window State Machine

Use explicit states:

```text
upcoming
 -> available
 -> celebrated
 -> expired
```

For auto-anniversaries:
```text
upcoming
 -> observed
```

Persist state only where needed.

A celebration opportunity should not retrigger every day in its window.

---

## 116. Grace Windows

A strict one-day requirement is hostile to emergency-heavy survival gameplay.

Example:
- event opens day 90;
- valid through day 92.

If raid/fire occurs on day 90, player can still observe it later.

Tradition streak should count if celebrated within the valid window.

---

## 117. Event Expiry

When window closes:

- mark occurrence expired;
- do not reopen;
- evaluate any bounded disappointment consequence;
- update tradition streak if relevant;
- emit one event.

Expiry must be idempotent.

---

## 118. Celebration Preview Accuracy

The planner must distinguish:

### Guaranteed
- resource cost;
- eligible participant count;
- activity requirements.

### Estimated
- attendance;
- morale effect;
- relationship outcomes;
- memorable status.

Never present estimated participation as guaranteed.

---

## 119. Participant Reservation

If celebration occurs at a scheduled future time, reserve or revalidate participants at start.

A survivor can become:
- injured;
- dispatched;
- dead;
- assigned emergency duty.

Final participant list must be canonical at celebration start.

Do not consume per-participant cost for people who are no longer participating unless policy says resources were
already prepared.

---

## 120. Food Cost Semantics

The source examples scale food per participant.

Audit inventory units first.

"1 food" may not equal one meal or serving.

Define celebration food costs in real item/resource units.

If food items have nutrition/quality classes, celebration may accept tagged categories rather than exact IDs.

---

## 121. Gift Safety Rules

Gift activities require a filtered candidate set.

Exclude by default:
- equipped weapons;
- quest-critical items;
- medicine reserved for treatment;
- unique keys/codes;
- critical survival tools;
- mod-missing placeholders.

Allow:
- keepsakes;
- luxury food;
- decorative items;
- designated gift-category items.

Player may explicitly override only if inventory UI supports safe transfer.

---

## 122. Feast Quality Without New Subsystem

If food has no quality stat, do not invent one.

Use:
- recipe/item tags;
- variety count;
- scarcity ratio;
- special/luxury tag.

Or keep feast effect scale-based.

Avoid adding a food-quality system solely for celebrations.

---

## 123. Shelter Facility Integration

Activities may need:
- common room;
- kitchen;
- performance space;
- memorial space;
- outdoor area.

Reference facility capabilities, not invented room names.

If no facility exists, fallback activity set remains available.

---

## 124. Power and Lighting

Music, dance, or night events may require lighting/equipment.

Integrate with shelter power only where real capabilities exist.

On brownout:
- downgrade event;
- switch to stories/memorial;
- do not silently cancel after resources were committed.

---

## 125. Weather Disruption

Weather can alter activity eligibility.

Example:
- outdoor feast blocked by severe storm;
- indoor celebration still available.

Do not reroll the entire event.

Use activity-level fallback.

---

## 126. Nuclear Winter Tone Variants

Events during nuclear winter should not use contradictory abundance language.

Add variant keys such as:

```text
event_harvest.normal
event_harvest.scarcity
event_harvest.nuclear_winter
```

Variant choice is deterministic from world phase and resource context.

---

## 127. Morale Context Sensitivity

Celebration benefit may depend modestly on current morale.

Avoid:
- low morale = huge exploit bonus;
- high morale = zero benefit.

Suggested:
- low morale gets slightly higher relief;
- high morale gets stronger social/cultural value.

NeedsSystem remains final authority.

---

## 128. Grief Sensitivity

A large party immediately after a death may be tonally wrong.

If a recent death occurred:
- memorial activity gains relevance;
- some survivors may opt out of exuberant activities;
- celebration can become mixed.

Use canonical recent-death facts.

No hardcoded survivor-specific emotional model inside seasonal state.

---

## 129. Survivor Preference Integration

If Plan 161 exists, activity affinity can influence attendance.

Examples:
- musician prefers music;
- storyteller joins stories;
- game group joins tournament;
- cook helps feast.

This produces personality without requiring a second preference table.

---

## 130. Relationship Conflict Integration

Feuding survivors may avoid each other in a celebration.

Do not manually edit attendance based on raw relationship numbers if a relationship API already exposes affinity/conflict.

Shared attendance can emit reconciliation opportunities but should not automatically erase feuds.

---

## 131. Attendance Grouping

For large rosters, do not persist every attendee in every routine celebration unless needed.

Possible policy:
- memorable event: persist full participant IDs;
- routine event: persist participant count + notable participants.

If achievements/epilogue require exact attendance, retain required IDs.

---

## 132. Memorable Participant Selection

For archive/epilogue, choose notable participants deterministically:

- event organizer;
- performer;
- tradition founder;
- key memorial relation;
- highest hobby contributor.

Do not randomly name survivors.

---

## 133. Tradition Naming

If user-entered names are supported:

- length limit;
- Unicode-safe;
- sanitize rich text;
- no markup execution;
- preserve in save.

If custom names are not needed, choose localized generated names from activities/event type.

---

## 134. Tradition Identity Stability

Tradition ID must not derive solely from user name.

Use generated stable ID:

```text
tradition:<campaignId>:<sequence>
```

Name can change without breaking save references.

---

## 135. Tradition Editing

After establishment, allow limited editing:
- display name;
- preferred activities;
- optional date within constraints.

Changing date should not manufacture a second recurrence in same year.

Track `lastObservedYear`.

---

## 136. Tradition Collision Detection

If multiple traditions share the same window:
- warn;
- allow compatible merge;
- or stagger.

Do not silently force several separate resource-heavy celebrations on one day.

---

## 137. Tradition Retirement Archive

Retired tradition remains historical.

Persist:
- established year;
- retired year;
- longest streak;
- times celebrated.

Archive/epilogue can still reference it.

---

## 138. Memorial Tradition Conversion

Repeated remembrance can become a shelter memorial tradition.

This should be an optional cultural evolution:
- same memorial observed over multiple years;
- enough participation;
- player establishes or system suggests tradition.

Do not duplicate MemorialSystem.

---

## 139. Founding Day Identity Growth

Founding Day can accumulate metadata:

- number of years;
- largest celebration;
- crises survived since founding;
- tradition activities.

SeasonalEventSystem may expose facts; ShelterIdentity/Archive owns interpretation.

---

## 140. Celebration Under Siege

If shelter is under active siege:
- planner may block unsafe festivities;
- low-cost remembrance/story activity may still be possible;
- tradition grace window applies.

This creates meaningful contrast without absurd behavior.

---

## 141. Celebration During Famine

Scarcity variant:
- no forced feast;
- storytelling;
- games;
- memorial;
- symbolic gift/keepsake;
- tiny ration-sharing if player chooses.

Do not punish player for refusing expensive food spend.

---

## 142. Celebration During Abundance

Abundance can unlock:
- feast variety;
- gifts;
- larger participation;
- decoration if supported.

Main gain:
- memorable/cultural status.

Morale remains capped.

---

## 143. Long-Campaign Event Compression

Campaigns spanning years can generate repetitive annual events.

After several repetitions:
- combine routine event notification into calendar summary;
- keep major streak/milestone years prominent;
- allow auto-small tradition policy later.

The event still resolves deterministically.

---

## 144. Event History Pruning

Save history can grow.

Keep:
- memorable celebrations;
- latest routine occurrences;
- aggregate counts;
- tradition records.

Do not need every routine participant list forever.

Define retention policy before long-campaign testing.

---

## 145. Seasonal Statistics

Optional diagnostics:
- celebrations held;
- skipped;
- resources spent;
- average participation;
- traditions active;
- longest streak.

These are run-local analytics, not profile progression.

---

## 146. Celebration Value Simulation

Create a balance simulator:

Inputs:
- roster size;
- food stock;
- morale;
- scale;
- activities.

Outputs:
- cost;
- expected attendance;
- morale effect;
- efficiency;
- memorable eligibility.

Run across:
- 5 survivors;
- 20 survivors;
- 50 survivors;
- 100 survivors.

Prevent roster-size exploits.

---

## 147. Resource Scaling Functions

Avoid purely linear cost if huge rosters make celebrations impossible.

Possible:
- base cost + per-participant cost;
- capped feast servings;
- group activity fixed cost.

Example:

```text
foodCost = base + ceil(participants * perParticipantRate)
```

Tune using actual resource economy.

---

## 148. Morale Scaling Functions

Likewise:

```text
totalEffect = baseEffect
            + participationRateBonus
            + activityVarietyBonus
            + traditionBonus
```

not:
```text
morale += participants * hugeValue
```

NeedsSystem receives individual or group effects in canonical format.

---

## 149. Tradition Streak Scaling

Streak bonus should plateau.

Example:
- year 1: none;
- year 2: small;
- year 3+: capped cultural bonus.

Avoid exponential accumulation over long campaigns.

---

## 150. Celebration Failure Modes

A celebration can be:

- planned but impossible due to shortage;
- interrupted by emergency;
- low attendance;
- modest;
- successful;
- memorable.

Do not reduce all failure to "morale penalty."

Narrative result can be nuanced.

---

## 151. Interrupted Celebration

If raid interrupts:
- resources already consumed remain spent according to transaction stage;
- event marked interrupted;
- morale effect reduced;
- archive may record interruption if memorable.

Save/reload must not restart it as untouched.

---

## 152. Rescheduled Celebration

If not started:
- reschedule within window;
- no cost until commit;
- same event occurrence ID.

Prevents exploit of generating multiple event instances.

---

## 153. Event Result DTO

```csharp
public sealed record CelebrationResult
{
    public string CelebrationId { get; init; }
    public CelebrationOutcome Outcome { get; init; }
    public int ParticipantCount { get; init; }
    public int ParticipationRateBasisPoints { get; init; }
    public int MoraleEffect { get; init; }
    public bool Memorable { get; init; }
    public IReadOnlyList<string> CulturalFactIds { get; init; }
}
```

Keep downstream-facing output structural.

---

## 154. Event Effect Registry

Activity effects should be typed:

- morale;
- relationship;
- hobby performance;
- memorial;
- archive;
- identity;
- skill-practice if supported.

Validator requires known effect kind.

No arbitrary reflection calls from JSON.

---

## 155. UI Preview vs Final Result

Preview uses deterministic known inputs plus uncertainty bands.

Final result may differ due to:
- attendance;
- interruption;
- current state changes.

UI should explain why.

---

## 156. No Save-Scum Reroll

Celebration seed assigned when plan commits.

Reloading before result yields same:
- attendance variation;
- memorable roll if any;
- minor outcome variation.

Prefer threshold-based memorable status where possible.

---

## 157. Modding Contract Integration

If Plan 165 exists, seasonal event definitions are a candidate public data catalog.

Only expose after:
- trigger schema stable;
- activity effect registry stable;
- save-safe IDs stable.

Mods may add events, but should not inject executable trigger logic.

---

## 158. Modded Event Save Safety

A save may reference:
- modded tradition source event;
- modded celebration activity.

If mod is removed:
- historical celebration record should remain readable structurally;
- future modded recurring event may disappear;
- tradition referencing missing activity needs compatibility handling.

Plan 165 owns missing-mod compatibility reporting.

---

## 159. Localization Requirements

Every:
- event title;
- description;
- activity;
- result string;
- tradition default name;
- anniversary label;

uses localization keys.

User-entered tradition names remain literal.

---

## 160. Narrative Tone Review

Seasonal content must fit ASHFALL.

Review:
- no cheerful theme-park tone during catastrophe;
- no modern holiday brand references without lore reason;
- no melodrama for every memorial;
- no identical "hope returned" phrasing annually.

Use physical shelter detail and survivor-specific context where available.

---

## 161. Accessibility Snapshot Cases

Capture UI states:
- event due;
- cost shortage;
- high text scale;
- many activities;
- many anniversaries;
- grouped memorial;
- tradition streak;
- controller focus.

No critical status only in color.

---

## 162. Headless Five-Year Simulation

If campaign supports long enough:

Simulate 5 campaign years with:
- fixed roster;
- some deaths;
- founding anniversary;
- one tradition;
- periodic skipped events.

Assert:
- recurrence counts;
- streak;
- grouping;
- no duplicate occurrence;
- bounded save growth;
- deterministic digest.

If five years is unreachable, use compressed test calendar fixture.

---

## 163. Seasonal State Digest

Create deterministic digest from:
- founding day;
- occurrence cursor/state;
- celebration IDs/results;
- traditions;
- milestone emissions.

Use in selftest.

---

## 164. Failure Recovery

If catalog fails:
- fail data integrity before campaign load.

If one event references invalid item:
- development/CI failure;
- production recovery according to catalog policy.

Do not silently drop costs and create free morale.

---

## 165. Release Gate

Release fails when:

- `--seasonal-events-selftest` fails;
- event catalog invalid;
- calendar trigger no longer resolves;
- old-save migration fails;
- celebration resource transaction test fails;
- tradition recurrence changes unexpectedly;
- no-celebration path begins causing severe penalty;
- deterministic digest changes without approved baseline.

---

## 166. Documentation Set

Create:

- `docs/seasonal_events/SEASONAL_EVENTS_INTEGRATION_AUDIT.md`
- `docs/seasonal_events/SEASONAL_EVENT_SCHEMA.md`
- `docs/seasonal_events/SEASONAL_EVENT_COVERAGE.md`
- `docs/seasonal_events/CELEBRATION_BALANCE.md`
- `docs/seasonal_events/TRADITION_CONTRACT.md`
- `docs/seasonal_events/SEASONAL_EVENT_MIGRATION.md`
- `docs/seasonal_events/SEASONAL_EVENT_AUTHORING.md`

---

## 167. Implementation Agent Final Pass

After coding:

1. search for direct morale mutation in seasonal code;
2. search for direct inventory mutation outside transaction API;
3. search for duplicate calendar arithmetic;
4. verify no UI rule branches;
5. verify no per-frame polling;
6. verify event IDs stable;
7. run old-save fixture;
8. run scarcity fixture;
9. run long-campaign recurrence fixture;
10. run full test suite.

---

## 168. Final Acceptance Scenario — A Shelter That Remembers

A representative campaign should be able to produce this structural sequence:

```text
Year 1:
- first seasonal celebration
- survivor dies
- founding day observed

Year 2:
- survivor remembrance grouped into memorial
- small tradition established
- scarcity forces low-cost celebration

Year 3:
- tradition streak reaches 2
- hobby musicians perform
- one festival becomes memorable

Endgame:
- archive contains only notable cultural events
- shelter identity references tradition
- epilogue can reference remembered celebration
```

Every part of this sequence must be supported by canonical facts and persisted IDs, not improvised prose or
duplicate state.

This is the final proof that the system creates temporal rhythm rather than just morale buttons.
