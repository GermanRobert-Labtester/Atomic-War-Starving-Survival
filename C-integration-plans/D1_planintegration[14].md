# D1 Flagship Integration Plan [14]
## Plan 187 — Bestiary & Creature Encounter Tracking UI

> **Purpose:** Convert ASHFALL's existing static wasteland-fauna catalog into one deterministic, save-aware
> bestiary knowledge system that records what the player has actually encountered, how reliable that knowledge
> is, where creatures were seen, what was killed or butchered, which behavioral notes have been earned, and
> what remains unknown.
>
> **Primary source:** Plan 187 — Bestiary & Creature Encounter Tracking UI.
>
> **Core design problem:** `WastelandBestiaryCatalog.cs` already defines roughly two dozen irradiated fauna
> entries with gameplay-relevant facts such as habitat, danger, scout notes, and butcher yields, but that data
> is currently static content. No run-local discovery state exists, no encounter provenance exists, combat does
> not feed kill knowledge into a bestiary, expeditions do not create a durable sighting history, and the player
> has no UI that distinguishes unknown, suspected, observed, hunted, butchered, and well-understood creatures.
>
> **Implementation posture:** deterministic, evidence-driven, data-backed, save-safe, low-chore, event-driven,
> journal-integrated, and carefully separated from combat, hunting, expedition, weather, location, inventory,
> and ecology authorities.
>
> **Critical guardrail:** the bestiary records knowledge; it does not become a second creature simulation.
> Creature spawn truth, combat resolution, butcher yields, habitat state, migration, weather, and location
> evolution remain owned by their existing systems. The bestiary observes authoritative events and turns them
> into player-facing knowledge.

---

## 1. Source Problem Statement

The source plan identifies a straightforward but consequential gap:

- `WastelandBestiaryCatalog.cs` is static data;
- no encounter tracking exists;
- no kill count exists;
- no butcher count exists;
- no sighting log exists;
- no discovery progression exists;
- no creature bestiary panel exists;
- journal codex unlocks cover items/locations, but not fauna.

The required architecture is therefore:

```text
Creature catalog + real world encounters
                  ↓
             BestiarySystem
                  ↓
     run-local knowledge ledger
   ┌──────────────┼──────────────────┐
   ↓              ↓                  ↓
discovery      sightings          counters
   ↓              ↓                  ↓
knowledge tier  confidence       encounter/kill/butcher
   └──────────────┼──────────────────┘
                  ↓
          behavior-note unlocks
                  ↓
       panel / journal / map facts
                  ↓
 quest / achievement / epilogue hooks
```

The bestiary must never generate combat kills, habitat truth, or butcher resources on its own.

---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `BestiarySystem.cs` exists with schema-versioned capture/restore.
2. All 24 existing creature IDs can be represented without duplicating the base creature catalog.
3. Creature knowledge starts unknown unless migration or authored scenario state says otherwise.
4. First authoritative encounter creates one discovery record.
5. Encounter counts increase only from authoritative encounter/sighting events.
6. Kill counts increase only from canonical combat/hunting kill events.
7. Butcher counts increase only from canonical butcher/resource-processing completion events.
8. One kill cannot be counted twice because both combat and expedition summary report it.
9. Sighting records use stable IDs and provenance.
10. Recent-sighting retention is bounded.
11. Historical aggregate counts survive even after old sightings are pruned.
12. Knowledge tiers unlock deterministically from evidence thresholds.
13. Behavioral notes are data-backed and validated against creature IDs.
14. Notes never reveal canonical facts before their knowledge requirement is met.
15. Cryptids can remain unconfirmed despite repeated low-confidence sightings.
16. A confirmed creature is not automatically marked hunted or butchered.
17. Sighting confidence is not the same thing as underlying world truth.
18. Expedition sighting generation delegates to canonical creature/ecology/habitat data.
19. Weather can influence sighting opportunity through existing weather authority.
20. Location habitat data remains authoritative.
21. Journal integration creates fauna discovery/codex entries without duplicating the bestiary state.
22. The panel reads projection DTOs only; no panel-owned encounter thresholds exist.
23. Old saves load safely with empty bestiary unless repository evidence supports reconstruction.
24. No retroactive fake kill/butcher counts are invented.
25. Completion percentages are derived, not separately authoritative.
26. Headless tests prove discovery and note progression without UI.
27. `--bestiary-selftest` validates all creature mappings, thresholds, idempotency, save/load, and UI projection.

---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/bestiary/BESTIARY_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Narrative/WastelandBestiaryCatalog.cs`
- creature DTOs and catalog loader
- expedition encounter generation
- encounter resolution DTOs
- combat kill events
- hunting/trapping systems
- wildlife encounter tables
- butchering/resource-harvest pipeline
- location/habitat data
- weather system
- season/calendar system
- location evolution/ecology systems
- journal/codex unlock APIs
- map/location-discovery UI
- save schema
- seeded RNG
- notification/event bus
- quest runtime
- achievement system
- shelter archive/epilogue hooks
- asset registry for creature art/silhouettes
- localization keys
- modding contract from Plan 165 if implemented

Build an authority map:

| Fact | Canonical source | Event/API | Persisted? | Bestiary action |
|---|---|---|---:|---|
| creature exists | bestiary catalog | catalog lookup | base data | read |
| creature encountered | expedition/combat/hunting | event | maybe | count |
| creature killed | combat/hunt | kill event | maybe | count |
| creature butchered | butcher system | completion event | maybe | count |
| habitat | creature/location catalogs | lookup | base data | reveal when known |
| weather | weather system | snapshot | yes/no | sighting input only |
| creature spawn | ecology/encounter | source | no/yes | never own |

Do not wire to UI strings or end-of-run summaries if lower-level authoritative events exist.

---

## 4. Scope Boundary

### In scope

- creature discovery state;
- knowledge tiers;
- encounter counts;
- kill counts;
- butcher counts;
- sighting records;
- witness provenance;
- observation confidence;
- note unlocking;
- category filtering;
- completion tracking;
- journal hooks;
- map/history projections;
- save/load;
- old-save migration;
- deterministic expedition sightings where the expedition/ecology authority expects bestiary-facing observations;
- selftests and validation.

### Out of scope for first pass

- creature AI;
- creature spawn simulation;
- ecosystem population simulation;
- hunting balance;
- combat tactics implementation;
- butcher yield calculation;
- domestication;
- trophies;
- migration simulation;
- procedural creature generation;
- runtime image generation;
- cross-campaign bestiary profile.

The bestiary is run-local knowledge and presentation.

---

## 5. Core Bestiary State

Recommended:

```csharp
public sealed record BestiaryState
{
    public int SchemaVersion { get; init; }
    public IReadOnlyDictionary<string, CreatureDiscoveryState> Creatures { get; init; }
    public IReadOnlyList<CreatureSightingRecord> RecentSightings { get; init; }
    public IReadOnlySet<string> UnlockedNoteIds { get; init; }
    public IReadOnlySet<string> EmittedMilestoneIds { get; init; }
}
```

Do not persist completion percentage or total discovered count as independent truth unless cached with
self-validation.

---

## 6. Creature Discovery State

Recommended:

```csharp
public sealed record CreatureDiscoveryState
{
    public string CreatureId { get; init; }
    public int? DiscoveredDay { get; init; }
    public string KnowledgeTierId { get; init; }

    public int EncounterCount { get; init; }
    public int KillCount { get; init; }
    public int ButcherCount { get; init; }

    public string? FirstEncounterLocationId { get; init; }
    public int? LastEncounterDay { get; init; }

    public IReadOnlyDictionary<string, int> EncounterTypeCounts { get; init; }
    public IReadOnlyDictionary<string, int> LocationEncounterCounts { get; init; }

    public string? DiscoveryContextId { get; init; }
}
```

Avoid storing rendered discovery text.

---

## 7. Creature Sighting Record

Recommended:

```csharp
public sealed record CreatureSightingRecord
{
    public string SightingId { get; init; }
    public string? CreatureId { get; init; }
    public int Day { get; init; }
    public string LocationId { get; init; }

    public IReadOnlyList<string> WitnessSurvivorIds { get; init; }
    public string SightingTypeId { get; init; }
    public string DistanceBandId { get; init; }
    public string BehaviorId { get; init; }

    public int ConfidenceBasisPoints { get; init; }
    public bool ConfirmedIdentity { get; init; }
    public string SourceEventId { get; init; }
}
```

For unconfirmed cryptid-like sightings, `CreatureId` may be a hypothesis rather than a confirmed identity if the
architecture supports explicit claim records.

---

## 8. Behavior Note Definition

Create data-backed note definitions.

```csharp
public sealed record CreatureBehaviorNoteDefinition
{
    public string NoteId { get; init; }
    public string CreatureId { get; init; }
    public string NoteTypeId { get; init; }
    public BestiaryKnowledgeRequirement Requirement { get; init; }
    public string TitleKey { get; init; }
    public string TextKey { get; init; }
}
```

Requirements should support:
- encounter count;
- kill count;
- butcher count;
- confirmed sighting count;
- unique habitats observed;
- specific event type.

Do not put one `unlockThreshold` integer in every note if evidence types differ.

---

## 9. Knowledge Tier Model

The source uses encounter thresholds.

Refine into explicit tiers:

```text
unknown
rumored
identified
observed
understood
mastered
```

Possible mapping:

### Unknown
No evidence.

### Rumored
Unconfirmed track/sighting claim.

### Identified
Confirmed first encounter.

### Observed
Enough encounters to reveal basic habitat/danger data.

### Understood
Behavior/diet patterns unlocked.

### Mastered
Detailed tactical/resource notes unlocked.

Kill/butcher evidence can unlock specialized notes independently of overall tier.

---

## 10. Why Separate Tier and Notes

A creature can be:

- well observed but never killed;
- frequently hunted but never butchered;
- butchered once but poorly understood behaviorally.

Therefore:
- overall knowledge tier summarizes;
- notes track specific evidence.

Do not force all knowledge into one linear progression.

---

## 11. Source Threshold Compatibility

The source suggests:

- 3 encounters: basic stats;
- 5 encounters: behavior;
- 10 encounters: detailed tactics;
- 1 kill: combat notes;
- 5 kills: hunting notes;
- 1 butcher: resource notes.

Use these as initial tuning, not hardcoded constants.

Place thresholds in:
`bestiary_notes.json` or creature knowledge-profile data.

---

## 12. Catalog Authority

Keep `WastelandBestiaryCatalog` as creature-definition truth.

BestiarySystem stores only run-local knowledge state.

Do not copy:
- calories;
- habitat;
- danger;
- scout notes;
- taxonomy;
- butcher yields;

into save state.

The UI resolves base catalog facts only when the current knowledge state permits them.

---

## 13. Catalog Adapter

Recommended:

```csharp
public interface ICreatureKnowledgeCatalog
{
    bool TryGetCreature(string creatureId, out CreatureDefinition definition);
    IReadOnlyList<string> AllCreatureIds { get; }
}
```

BestiarySystem remains independent of concrete narrative loader where possible.

---

## 14. Creature Categories

The source suggests:

- mammals;
- birds;
- reptiles;
- insects;
- aquatic;
- cryptids;
- domestic.

Do not invent category memberships by name.

Audit all 24 creatures and create a validated category field or mapping.

If existing catalog already has taxonomy, reuse it.

---

## 15. Category Data Contract

Prefer base creature catalog field:

```text
bestiaryCategoryId
```

or external mapping:

```json
{
  "creatureId": "creature_rad_deer",
  "categoryId": "mammal"
}
```

No UI-specific switch statements.

---

## 16. Discovery Event Authority

BestiarySystem should receive:

```csharp
CreatureEncounterObserved
{
    eventId,
    creatureId,
    day,
    locationId,
    witnessIds,
    encounterType,
    behavior,
    distance
}
```

The event must come from real gameplay.

BestiarySystem does not decide that an encounter happened.

---

## 17. First Discovery Transaction

On first confirmed encounter:

1. validate creature ID;
2. create discovery state;
3. set `DiscoveredDay`;
4. store first location;
5. store discovery context;
6. increment encounter counter once;
7. evaluate notes/tier;
8. emit `CreatureDiscovered`;
9. update journal projection;
10. mark save dirty.

Use source event ID for idempotency.

---

## 18. Encounter Idempotency

A single event may flow through:
- combat;
- expedition;
- summary;
- journal.

Do not count it multiple times.

Every observation has stable `SourceEventId`.

Persist a bounded or compressed event-id ledger if upstream event bus can replay.

---

## 19. Encounter Type Taxonomy

Source types:

- spotted;
- attacked;
- fleeing;
- dead;
- track_found.

Refine:

```text
visual_spotted
hostile_encounter
creature_fled
carcass_found
tracks_found
nest_den_found
captured
```

Only include types supported by upstream systems.

---

## 20. Encounter Counts vs Sightings

Not every sighting must count equally.

Recommended:

- confirmed visual creature = encounter count +1;
- hostile encounter = encounter +1;
- carcass found = discovery evidence but maybe not live encounter;
- tracks = evidence/confidence, not confirmed encounter;
- rumor = no encounter count.

This prevents easy farming through repeated weak evidence.

---

## 21. Kill Count Authority

Kill count increments from canonical creature death attribution.

Event should include:
- creature ID;
- killer/party;
- source encounter ID;
- location;
- day.

Bestiary can count party kill for the run even if no single survivor personally killed it.

Choose semantics and document:
- player-party kills;
- direct survivor kills;
- all observed creature deaths.

Recommend player-caused/party kills only.

---

## 22. Kill Attribution Edge Cases

Test:

- creature killed by environment;
- creature killed by another faction;
- creature already dead when found;
- trap kill;
- companion kill;
- combat party kill.

Only intended categories increment `KillCount`.

---

## 23. Butcher Count Authority

Butcher count increments only after resource-harvest transaction completes.

Do not increment when:
- player selects butcher option but cancels;
- carcass found;
- kill occurs;
- butcher action fails.

Stable butcher transaction ID required.

---

## 24. Butcher Knowledge

A butcher event can unlock:

- meat quality;
- calorie/yield insight;
- hide/bone/resource notes.

The source catalog already contains butchered-meat calorie data.

Reveal it through a resource note, not immediately on first sighting.

---

## 25. Combat Knowledge

After first real combat/kill, unlock:
- attack pattern;
- vulnerability;
- dangerous behavior;

only if source data exists.

Do not invent combat weaknesses unsupported by combat definitions.

---

## 26. Hunting Knowledge

Five kills can unlock advanced tracking/bait/ambush notes only if actual hunting systems support those mechanics.

If not:
- notes can remain observational;
- do not promise a bait bonus that does not exist.

Bestiary text must not advertise nonexistent mechanics.

---

## 27. Behavior Notes as Evidence-Based Knowledge

Each note should reference real catalog/system data.

Examples:

### Habitat note
Reveals canonical habitat list.

### Diet note
Reveals diet tags if catalog exists.

### Danger note
Reveals danger band.

### Butcher note
Reveals actual yield/calorie data.

### Combat note
Reveals real attack/vulnerability tags.

### Activity note
Reveals day/night/weather activity only if ecological data exists.

---

## 28. Narrative Tone

Behavior notes should read like survivor field observations, but underlying facts remain structured.

Example pattern:

> Seen twice along flooded ditches after dusk. It keeps low until cornered.

Do not encode the only gameplay truth in prose.

The UI can show structured fact + narrative note.

---

## 29. Note Unlock Evaluation

Evaluate when relevant evidence changes.

Do not check all notes daily.

Triggers:
- encounter;
- kill;
- butcher;
- habitat discovery;
- confirmed sighting.

Index notes by creature and requirement type.

---

## 30. Note Unlock Idempotency

Stable note unlock ID:

```text
bestiary_note:<creatureId>:<noteId>
```

Once unlocked:
- persists;
- no duplicate notification;
- no relock if counts change (they should be monotonic anyway).

---

## 31. Sighting Generation Ownership

The source says expeditions have a chance to generate sightings.

The best architecture depends on existing expedition ecology.

Preferred:

```text
Expedition/EncounterSystem decides creature opportunity
 -> emits sighting
 -> Bestiary records it
```

Not:

```text
BestiarySystem rolls independent wildlife every expedition
```

Avoid two creature spawn systems.

---

## 32. Deterministic Sighting RNG

If a dedicated observation roll is needed after a creature opportunity exists:

Seed from:
- campaign seed;
- expedition ID;
- location ID;
- day;
- creature/opportunity ID.

Use canonical `ISeededRng`.

No wall-clock.

---

## 33. Habitat Filtering

Sighting candidates must come from real habitat compatibility.

Use:
- creature habitat tags;
- location biome/environment;
- location evolution state;
- season/weather where relevant.

Do not let UI category determine spawn.

---

## 34. Weather Influence

Weather may affect:
- visibility;
- animal activity;
- tracks;
- flight;
- water activity.

But only through configured modifiers.

Do not create a second weather model.

---

## 35. Season Influence

If seasonal creature activity exists:
- read it.

If not:
- do not fabricate migration rates merely for bestiary variety.

Follow-on Plan can add migration.

---

## 36. Rarity

Creature rarity should belong to creature/ecology data.

Bestiary may use rarity to:
- show status after unlocked;
- classify discovery importance.

It should not secretly alter spawn unless it is the designated encounter generator.

---

## 37. Witnesses

Sighting record may contain one or more witnesses.

Witnesses must:
- exist;
- be present on expedition/location;
- be alive at event time.

No random extra witnesses.

---

## 38. Confidence

Confidence is evidence quality, not truth.

Potential inputs:
- distance;
- visibility;
- witness count;
- observer skill if canonical;
- creature obscurity;
- tracks vs visual.

Clamp 0..10000 basis points.

---

## 39. Multiple Witnesses

The source says more witnesses increase confidence.

Use diminishing returns.

Example:
- 1 witness: baseline;
- 2: moderate increase;
- 3+: small further increase.

Do not let a 20-person expedition turn a vague track into certain identity.

---

## 40. Cryptid Model

The source includes cryptids with unconfirmed sightings.

Do not treat them as normal creatures with low spawn chance unless base catalog actually contains them.

Use one of:

### A. Cataloged creature, unconfirmed identity
The world truth exists, player knowledge uncertain.

### B. Rumor entity
No confirmed creature truth until a later event.

Bestiary should explicitly represent confirmation state.

---

## 41. Cryptid Confirmation

Confirmation could require:
- close high-confidence sighting;
- multiple independent sightings;
- physical evidence;
- kill/carcass;
- quest event.

Do not auto-confirm at 3 encounters simply because normal creatures do.

---

## 42. Unknown Creature Presentation

The source says first encounter can show "Unknown Creature" until 3 encounters.

Refine:

- if identification is certain on first close encounter, show actual name;
- if identification confidence low, show unknown/suspected silhouette.

Knowledge system should use evidence, not arbitrary concealment.

---

## 43. Silhouette Unlock

Visual presentation states:

```text
unknown silhouette
recognized silhouette
full illustration
```

Base game must have asset refs.

Do not require new illustration generation to ship tracking system.

Use placeholders where art is absent.

---

## 44. Illustration Unlock Follow-On

Detailed creature art at mastery can be a later content task.

Bestiary architecture should support:

```text
IllustrationTier
```

without making it required for state correctness.

---

## 45. Sighting Retention

The source says recent 50.

Persist:
- most recent N detailed records;
- aggregate encounter/location counts separately.

When pruning:
- do not lose first encounter;
- do not reduce counters;
- do not relock notes.

N belongs in config/data.

---

## 46. Sighting Sort Order

Deterministic:
- day descending;
- stable sequence/event ID tie-break.

No dictionary-order differences.

---

## 47. First Encounter Record

Never prune:

- discovered day;
- first location;
- first context.

This is part of creature discovery provenance.

---

## 48. Last Encounter Record

Can be derived from latest sighting or stored compactly.

If recent sighting pruned, keep:
- last day;
- last location if useful.

Do not depend on list retention.

---

## 49. Location Encounter Counts

Optional per-creature map knowledge:

```text
locationId -> confirmed sightings count
```

Useful for habitat mapping.

Bound number of entries by actual visited locations.

---

## 50. Habitat Mapping

Source follow-on suggests habitat mapping.

V1 panel can show:
- known first/last encounter locations;
- confirmed locations observed.

Do not reveal full canonical habitat range unless habitat note unlocked.

---

## 51. Map Integration

If map supports overlays:

- show confirmed sighting pins;
- filter by creature;
- use recent/high-confidence sightings.

Do not render a full heatmap unless there is actual range data.

---

## 52. Location Evolution Integration

The source says creature habitats affect location evolution.

Reverse dependency needs care.

Preferred:
- LocationEvolution/Ecology changes creature availability;
- bestiary observes resulting sightings.

Bestiary should not mutate location evolution just because sightings were recorded.

---

## 53. If Bestiary Knowledge Affects Decisions

Knowledge may unlock player-facing strategic choices:

- avoid habitat;
- prepare equipment;
- choose hunting action.

Those systems query BestiarySystem knowledge tier.

The bestiary does not itself change world ecology.

---

## 54. Journal Integration

On first discovery:

```text
BestiarySystem emits CreatureDiscovered
 -> JournalSystem unlocks creature codex entry
```

Journal stores its own journal/codex projection if that is existing architecture.

No duplicate note logic.

---

## 55. Journal Entry Structure

Journal can show:
- discovery day;
- first location;
- known notes;
- major encounter.

Detailed sighting list belongs in bestiary panel.

---

## 56. Bestiary UI Projection

Recommended:

```csharp
public sealed record BestiaryPanelModel
{
    public IReadOnlyList<BestiaryCreatureListEntry> Creatures { get; init; }
    public BestiaryCompletionView Completion { get; init; }
    public IReadOnlyList<BestiaryCategoryView> Categories { get; init; }
}
```

Detail:

```csharp
public sealed record BestiaryCreatureDetailView
{
    public string CreatureId { get; init; }
    public string DisplayNameKey { get; init; }
    public string KnowledgeTierId { get; init; }
    public int EncounterCount { get; init; }
    public int KillCount { get; init; }
    public int ButcherCount { get; init; }
    public IReadOnlyList<UnlockedBestiaryNoteView> Notes { get; init; }
    public IReadOnlyList<CreatureSightingView> Sightings { get; init; }
}
```

---

## 57. UI Does Not Own Knowledge Rules

Forbidden in `BestiaryPanel.cs`:

```csharp
if(encounters >= 5) showDiet = true;
```

Correct:

```text
projection contains revealed facts/notes
```

The panel formats only.

---

## 58. Creature List

Show:
- known/unknown state;
- silhouette/illustration;
- category;
- tier;
- encounter count if discovered.

Unknown creatures may appear as:
- hidden slots;
- silhouettes;
- not listed.

Choose design intentionally.

---

## 59. Completion Tracker

Derived:

```text
discoveredCreatures / totalEligibleCreatures
unlockedNotes / totalNotes
```

If modded creatures exist:
- define whether completion uses active catalog;
- Plan 165 fingerprint handles save changes.

No hardcoded `/24` in UI.

---

## 60. Category Completion

Derived per category.

Use active catalog memberships.

No hardcoded creature counts.

---

## 61. Filters

Source filters:

- mammal;
- bird;
- reptile;
- insect;
- aquatic;
- cryptid;
- domestic.

UI reads catalog category IDs.

Search:
- discovered display name only;
- unknown entries should not leak names.

---

## 62. Sorts

Source sorts:

- discovery date;
- encounter count;
- danger level.

Danger sort only for creatures whose danger is unlocked.

Unknown danger should sort separately, not leak hidden numerical value.

---

## 63. Detail View

Show only known information:

### Identified
- name;
- first encounter;
- basic silhouette.

### Observed
- danger;
- habitat;
- behavior notes.

### Understood
- richer behavior/diet.

### Specialized evidence
- combat notes;
- butcher notes.

### Mastered
- all valid notes.

---

## 64. Tooltips

Tooltip may show:
- encounter progress to next generic observation tier;
- kill/butcher evidence progress;
- known note count.

Do not reveal hidden note titles if spoiler-sensitive.

---

## 65. Discovery Notification

First discovery:
- compact;
- localized;
- link/open bestiary.

Rare/cryptid sightings:
- different unconfirmed wording.

No modal spam for routine repeated encounters.

---

## 66. Note Unlock Notification

Show only:
- significant note unlocks;
- or batch several unlocked notes after one event.

One kill should not cause six sequential popups.

---

## 67. Accessibility

Support:
- keyboard/controller navigation;
- text scaling;
- unknown/known state not color-only;
- note lock state readable;
- silhouette alternative label;
- search/filter accessible;
- recent sightings list navigable;
- no hover-only critical info.

---

## 68. Discovery Tutorial

On first confirmed creature discovery:

Explain:
- encounters build knowledge;
- kills/butchering unlock specialized notes;
- bestiary tracks sightings;
- not all observations are certain.

Use canonical guidance system.

---

## 69. Bestiary Events

Source event families:

- The Discovery;
- The Sighting;
- The Encounter;
- The Kill;
- The Butcher;
- The Note;
- The Completion;
- The Master.

Use these as event/quest hooks.

Do not make every kill a journal narrative event.

---

## 70. Quest Hooks

Source:

- The Naturalist;
- The Hunter;
- The Observer;
- The Completion;
- The Cryptid;
- The Tracker;
- The Butcher.

Bestiary exports facts:
- unique discovered count;
- unique killed count;
- unlocked note count;
- confirmed cryptid IDs;
- sighting count;
- butcher count.

QuestSystem owns progress/rewards.

---

## 71. Achievement Integration

Plan 149 may observe same facts.

Avoid duplicate reward state.

Bestiary only exposes run-local read-only metrics/events.

---

## 72. Epilogue Integration

Plan 145 may reference:
- renowned naturalist;
- confirmed rare creature;
- complete bestiary;
- famous dangerous encounter.

Export stable fact IDs.

---

## 73. Archive Integration

ShelterArchive may store:
- first cryptid confirmation;
- complete category;
- full bestiary completion.

Routine sightings stay out of archive.

---

## 74. Hunting Integration

If trapping/hunting systems exist:

- hunt encounter emits observation/kill;
- tracking evidence can unlock sighting;
- butcher completion emits resource knowledge.

Bestiary does not change hunting success directly unless hunting system queries knowledge as a bonus.

---

## 75. Knowledge Bonuses — Optional and Bounded

The source mentions optional knowledge reward.

Avoid generic permanent stat bonus from completion.

If knowledge should matter:
- hunting system may read tactical note unlocked;
- expedition may show warning;
- combat UI may show known danger/vulnerability.

This is more thematic than "+5% morale for 24/24."

---

## 76. Combat UI Knowledge

Potential:
- if tactical note unlocked, combat preview can show known attack traits.

Combat remains authority.

Do not give BestiarySystem power to alter damage calculations unless a separate explicit knowledge-effect contract
exists.

---

## 77. Butcher UI Knowledge

Before first butcher:
- yield unknown/rough.

After:
- actual known yield/calories can display.

Inventory/resource system owns actual yield.

---

## 78. Weather UI Knowledge

If behavior note says creature active after rain:
- display only if backed by actual spawn/activity data.

Never write false strategy hints for flavor.

---

## 79. Data Catalog for Notes

Create:

`Assets/StreamingAssets/Data/bestiary_notes.json`

Suggested root:

```json
{
  "schemaVersion": 1,
  "knowledgeTiers": [],
  "noteTypes": [],
  "notes": []
}
```

Could be merged with creature catalog later.

---

## 80. Six-to-Eight Notes per Creature Target

The source calls for 6–8 notes each.

For 24 creatures, that is 144–192 notes.

Do not blindly author filler.

Recommended minimum note families per supported creature:
1. habitat;
2. danger;
3. diet/resource;
4. behavior;
5. combat;
6. butcher/hunting.

Additional:
- weather/season;
- rare field note.

Only when data supports.

---

## 81. Note Coverage Report

Generate:

`docs/bestiary/BESTIARY_NOTE_COVERAGE.md`

| Creature | Habitat | Danger | Diet | Behavior | Combat | Butcher | Extra | Total |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| | | | | | | | | |

Flag:
- creature with no combat note despite combat presence;
- note referencing missing facts;
- duplicate prose/filler.

---

## 82. Creature Reachability Audit

Critical requirement: all 24 creatures should be discoverable only if their encounter paths actually exist.

Create:

`docs/bestiary/BESTIARY_REACHABILITY.md`

For each creature:
- habitat locations;
- encounter table;
- season/weather restrictions;
- minimum campaign phase;
- expedition path;
- combat/hunting path;
- reachable test fixture.

Do not claim 24/24 completion if some catalog entries are lore-only.

---

## 83. Completion Denominator

Use:
- `eligibleForBestiaryCompletion` field.

Lore-only, disabled, test, DLC/mod, or impossible entries should not silently block completion.

Validator ensures denominator is explicit.

---

## 84. Old-Save Migration

The source says empty bestiary.

Default:

- empty discovery state;
- no sightings;
- no notes;
- no retroactive kill/butcher counts;
- no fake discovery based on inventory meat.

This is safest.

---

## 85. Optional Reconstruction

Only reconstruct if authoritative saved history already exists.

Examples:
- encounter ledger with creature IDs;
- combat kill statistics;
- butcher transaction history.

If not present, do not guess.

Document migration mode.

---

## 86. No Retroactive Notification Flood

If migration reconstructs known creatures:
- mark discovery/note notifications consumed;
- populate state silently;
- show one "Bestiary restored" diagnostic if necessary.

Do not replay years of discoveries.

---

## 87. Save Schema

Persist:
- per-creature counters/tier;
- first/last encounter;
- recent sightings;
- note unlock IDs;
- event idempotency markers;
- migration provenance if useful.

Do not persist base creature stats.

---

## 88. Save/Load Mid-Expedition

If sighting generated and saved before expedition return:
- decide whether event commits immediately or at expedition resolution.

Use same lifecycle as encounter authority.

Do not count it twice on return summary.

---

## 89. Stable Sighting IDs

Recommended:

```text
sighting:<campaignId>:<sourceEventId>:<sequence>
```

If upstream event ID unique, may derive directly.

Avoid timestamp-only IDs.

---

## 90. Stable Discovery IDs

Creature discovery milestone:

```text
creature_discovered:<creatureId>
```

One per campaign.

Note unlock:
```text
bestiary_note:<noteId>
```

---

## 91. Anti-Farming: Repeated Same Encounter

The source warns sightings can't be farmed.

Possible policies:

- same creature group/event counts once;
- repeated observation in same expedition has cooldown;
- one passive sighting per creature per location/day counts toward threshold;
- combat encounters always count independently.

Use source-event identity first; cooldown only if encounter generator can emit meaningless spam.

---

## 92. Anti-Farming: Safe Location Camping

If player can repeatedly enter/exit same location:
- sighting generator should be location/expedition/time constrained;
- no UI action generates sightings;
- no reroll by reopen.

Seed from expedition instance/day.

---

## 93. Anti-Farming: Save Reload

Sightings determined by stable expedition seed.

Reloading does not reroll:
- creature opportunity;
- identification confidence;
- rare sighting.

Persist encounter seed or rely on upstream deterministic expedition.

---

## 94. Anti-Farming: Butcher Loop

One carcass transaction can increment butcher count once.

No repeated process/cancel.

Use carcass/item instance ID or transaction ID.

---

## 95. Anti-Farming: Kill Attribution

One creature death ID counts once.

If combat system reports summary and death event:
- bestiary consumes one canonical source.

Test duplicates explicitly.

---

## 96. Data Integrity Validation

Validate:

- every bestiary creature ID exists;
- every completion-eligible creature has category;
- every note creature ID exists;
- every note requirement valid;
- every note localization key exists;
- every habitat note references actual data;
- every combat/resource note references supported facts;
- category IDs valid;
- knowledge tiers contiguous/ordered;
- no duplicate note IDs.

---

## 97. Behavior Note Fact Validation

If note says:
- "butchered meat provides X calories"

then X should come from structured catalog field, not duplicated number in prose if possible.

Use parameterized localization:
```text
"Yields roughly {calories} calories..."
```

This prevents data/text drift.

---

## 98. Scout Notes Integration

Source catalog has `harlan_scout_notes`.

Decide whether these are:
- initial flavor;
- unlockable field note;
- separate authored observer note.

Do not automatically reveal them all at discovery if they contain advanced knowledge.

---

## 99. Existing Scout Note Migration

Recommended:
- map scout notes to one bestiary note definition per creature;
- choose requirement based on content depth;
- avoid duplication in detail panel.

This converts existing narrative value into progression.

---

## 100. Category Icons

Icons are UI assets only.

If missing:
- text labels suffice.

Do not block feature on bespoke icon generation.

---

## 101. Creature Artwork

Use asset registry.

Visual states may be:
- generic unknown silhouette;
- catalog thumbnail;
- full illustration if available.

Asset absence must not break state.

---

## 102. Search Leakage

When creature undiscovered:
- search index must not reveal its real name.

Search only discovered names or category placeholders.

Critical spoiler/privacy-of-game-knowledge test.

---

## 103. Sort Leakage

When danger locked:
- don't sort using hidden danger in a way that reveals relative danger.

Unknown values group separately.

---

## 104. Completion Leakage

`X/24` reveals total count, which the source wants.

That is acceptable if design intentionally exposes total fauna count.

If cryptids should remain secret:
- denominator may hide secret entries until rumored.

Choose intentionally.

---

## 105. Cryptid Completion Policy

If cryptids are secret:
- full normal completion should not require unknown secret slots unless discovered;
- or UI may show "??".

Document whether "Master" requires confirmed cryptids.

---

## 106. Confidence UI

Show bands:
- uncertain;
- plausible;
- credible;
- confirmed.

Avoid exact percentages unless player-facing observation mechanics support precision.

---

## 107. Witness Death

Sighting remains in historical log even if witness later dies.

Resolve witness name from historical survivor records.

Do not delete sighting.

---

## 108. Missing Witness

If old save/mod removal makes witness unavailable:
- display unknown witness;
- preserve sighting.

No state corruption.

---

## 109. Location Rename

Store stable location ID.

UI resolves current localized name.

No duplicated location strings in save.

---

## 110. Location Removed by Mod

Plan 165 handles compatibility.

Bestiary can show:
- unavailable/missing location placeholder;
- retain historical record.

Do not delete sighting silently.

---

## 111. Modding Integration

If Plan 165 exists, candidate public contracts:
- new creature definitions;
- categories;
- behavior notes;
- assets.

Require:
- namespaced creature IDs;
- completion eligibility;
- reachability declaration;
- validated notes.

No executable sighting logic.

---

## 112. Modded Completion

Active catalog determines current run denominator.

Save fingerprint prevents silent mod-set drift.

If mod creature removed:
- historical discovery remains structurally;
- completion recalculates under active catalog;
- compatibility warning as needed.

---

## 113. Performance Budget

Bestiary is event-driven.

Requirements:
- no per-frame scans;
- note evaluation only for affected creature/evidence kind;
- recent sighting list bounded;
- panel projection cached until state changes;
- completion derived O(number of creatures), tiny at 24.

Negligible runtime overhead.

---

## 114. Save Footprint

Per creature:
- compact counters;
- first/last facts;
- note IDs.

Sightings:
- recent N only.

No full encounter history required.

If player wants historical map later, aggregates can support it.

---

## 115. Structured Diagnostics

Logs:

```text
CreatureDiscovered creature=<id> event=<id> day=<n>
CreatureEncounterRecorded creature=<id> type=<id> count=<n>
CreatureKillRecorded creature=<id> death=<id> count=<n>
CreatureButcherRecorded creature=<id> transaction=<id> count=<n>
BestiaryNoteUnlocked creature=<id> note=<id>
CreatureSightingRecorded sighting=<id> confidence=<band>
```

No normal log spam for every UI open.

---

## 116. Dedicated `--bestiary-selftest`

It should:

1. load all creature and note catalogs;
2. assert all eligible creature IDs resolve;
3. start with empty bestiary;
4. record first confirmed encounter;
5. verify one discovery event;
6. replay same source event and verify no duplicate;
7. reach 3/5/10 encounter thresholds;
8. verify appropriate notes;
9. record one kill;
10. record five kills;
11. record one butcher;
12. verify specialized notes;
13. create low-confidence sighting;
14. create multi-witness sighting;
15. test cryptid unconfirmed/confirmed flow if present;
16. save/reload;
17. prune >50 sightings;
18. verify aggregates remain;
19. verify UI projection hides locked facts;
20. verify old-save empty migration;
21. exit non-zero on mismatch.

---

## 117. Unit Test Matrix

### Discovery
- unknown;
- first encounter;
- duplicate source event;
- first location;
- last day.

### Counts
- encounter;
- kill;
- butcher;
- wrong event source;
- environmental death.

### Notes
- encounter threshold;
- kill threshold;
- butcher threshold;
- multiple notes unlocked in one event;
- no relock.

### Sightings
- one witness;
- multiple witnesses;
- track-only;
- carcass;
- confidence;
- retention.

### Cryptids
- rumor;
- low-confidence;
- repeated independent evidence;
- confirmation.

### UI
- unknown;
- partial knowledge;
- mastered;
- hidden search/sort leakage.

### Persistence
- empty;
- partial;
- full;
- old save.

---

## 118. Golden Creature Fixtures

Use actual creature IDs.

Create fixtures for:

1. common mammal;
2. dangerous predator;
3. bird;
4. reptile;
5. insect;
6. aquatic creature;
7. domestic/feral animal;
8. cryptid if catalog supports.

For each:
- first encounter;
- notes;
- kill/butcher where valid;
- habitat.

---

## 119. Reachability Test

For each completion-eligible creature:

- assert at least one encounter table/location can produce it;
- assert habitat/location IDs exist;
- assert any phase/weather restriction can occur;
- provide deterministic fixture seed that discovers it.

This is mandatory for "all 24 discoverable."

---

## 120. Property / Fuzz Tests

Properties:
- counts never negative;
- note unlock set monotonic;
- same event ID does not change counts twice;
- sightings bounded;
- all sighting creature/location/witness refs valid or safely historical;
- completion in 0..100%;
- same save state => same projection.

---

## 121. Sighting Generator Fuzz

If bestiary owns observation generation:

Randomize valid:
- location;
- weather;
- season;
- expedition party;
- habitat creatures.

Properties:
- no impossible habitat creature;
- deterministic same seed;
- confidence bounded;
- rare weights respected statistically.

---

## 122. Distribution Calibration

Generate:
`docs/bestiary/SIGHTING_DISTRIBUTION.md`

Simulate many expeditions.

Report:
- sightings per expedition;
- species frequency;
- rare creature rate;
- cryptid rumor rate;
- weather influence;
- average completion time.

Avoid one-week full bestiary.

---

## 123. Progression Calibration

Estimate:
- encounters to basic knowledge;
- sessions/expeditions needed;
- kills needed for hunting knowledge;
- butcher access.

Goal:
- common creature knowledge grows naturally;
- rare creatures remain special;
- mastery not pure grind.

---

## 124. Note Authoring Quality Audit

For 144–192 potential notes:

Review:
- duplicates;
- generic filler;
- data contradictions;
- invented mechanics;
- repeated wording;
- omniscient tone.

Prefer 6 excellent notes to 8 weak notes.

The source target is a quality range, not quota fetish.

---

## 125. Completion Rewards Boundary

The source marks rewards optional.

Recommended v1:
- achievement/quest recognition;
- knowledge itself;
- journal/archive fact.

Avoid direct global morale/knowledge bonus owned by BestiarySystem.

If a reward exists, another authority applies it.

---

## 126. Mastery and Player Utility

Knowledge should have tangible utility through information:

- known danger;
- known habitat;
- known butcher yield;
- known tactics;
- sighting history;
- map locations.

This is more valuable than arbitrary numeric reward.

---

## 127. Implementation Phase A — Audit and Contracts

Tasks:
1. audit creature catalog;
2. audit encounter/kill/butcher events;
3. audit journal/map;
4. define state DTOs;
5. define note schema;
6. define knowledge tiers;
7. loader/validator.

Exit:
catalog and empty state load headlessly.

---

## 128. Implementation Phase B — Encounter Ledger

Tasks:
1. observation event adapter;
2. first discovery;
3. encounter counts;
4. encounter type counts;
5. first/last provenance;
6. source-event idempotency;
7. tests.

Exit:
real encounters produce durable bestiary knowledge.

---

## 129. Implementation Phase C — Kill/Butcher Integration

Tasks:
1. canonical kill event;
2. canonical butcher event;
3. dedup;
4. specialized evidence counts;
5. tests across traps/combat/carcasses.

Exit:
combat/resource experience feeds bestiary exactly once.

---

## 130. Implementation Phase D — Knowledge Tiers and Notes

Tasks:
1. requirement evaluator;
2. note indexes;
3. structured fact reveal;
4. note unlock events;
5. scout note migration;
6. narrative localization;
7. tests.

Exit:
knowledge progresses from evidence.

---

## 131. Implementation Phase E — Sightings

Tasks:
1. habitat integration;
2. expedition observation hook;
3. weather modifiers;
4. witness confidence;
5. recent log;
6. retention;
7. cryptid handling;
8. deterministic seed.

Exit:
exploration creates meaningful sightings without a second spawn system.

---

## 132. Implementation Phase F — Journal and Map

Tasks:
1. discovery codex hook;
2. journal view;
3. sighting location projection;
4. first/last encounter facts;
5. optional map pins;
6. no truth leakage.

Exit:
bestiary connects to exploration history.

---

## 133. Implementation Phase G — UI

Tasks:
1. list;
2. category filters;
3. search;
4. sorts;
5. detail;
6. notes;
7. counts;
8. sighting log;
9. completion;
10. accessibility.

Exit:
panel contains no unlock logic.

---

## 134. Implementation Phase H — Content Completion

Tasks:
1. classify 24 creatures;
2. identify completion eligibility;
3. author/convert notes;
4. validate habitats/danger/yields;
5. add localization;
6. coverage report;
7. reachability report.

Exit:
every eligible creature has meaningful progression.

---

## 135. Implementation Phase I — Migration & CI

Tasks:
1. old save empty migration;
2. optional reconstruction if evidence exists;
3. selftest;
4. data integrity;
5. fuzz;
6. distribution;
7. anti-farming;
8. UI leakage tests;
9. full regression.

Exit:
bestiary is deterministic and supportable.

---

## 136. Exact File Plan

Expected new/modified files:

### Core
- `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs`
- `Assets/Ashfall.Core/Bestiary/BestiaryState.cs`
- `Assets/Ashfall.Core/Bestiary/CreatureDiscoveryState.cs`
- `Assets/Ashfall.Core/Bestiary/CreatureSightingRecord.cs`
- `Assets/Ashfall.Core/Bestiary/CreatureBehaviorNoteDefinition.cs`
- `Assets/Ashfall.Core/Bestiary/BestiaryKnowledgeEvaluator.cs`
- `Assets/Ashfall.Core/Bestiary/BestiaryEvents.cs`

### Data
- `Assets/StreamingAssets/Data/bestiary_notes.json`

### UI
- `src/UI/BestiaryPanel.cs`
- presenter/projection files per existing UI architecture

### Tests
- `Ashfall.Core.Tests/Bestiary/BestiarySystemTests.cs`
- `Ashfall.Core.Tests/Bestiary/BestiaryPersistenceTests.cs`
- `Ashfall.Core.Tests/Bestiary/BestiaryNoteTests.cs`
- `Ashfall.Core.Tests/Bestiary/BestiarySightingTests.cs`
- `Ashfall.Core.Tests/Bestiary/BestiaryReachabilityTests.cs`

### Docs
- `docs/bestiary/BESTIARY_INTEGRATION_AUDIT.md`
- `docs/bestiary/BESTIARY_NOTE_COVERAGE.md`
- `docs/bestiary/BESTIARY_REACHABILITY.md`
- `docs/bestiary/SIGHTING_DISTRIBUTION.md`

---

## 137. Bootstrap Order

Recommended:

```text
load creature catalog
load bestiary note catalog
validate cross references
construct BestiarySystem
restore BestiaryState
wire expedition/combat/butcher events
wire Journal adapter
construct UI projection
bind panel
```

Restore state before panel bind.

No discovery animation replay after load.

---

## 138. No Tick Requirement

The source mentions `TickBestiary`.

Prefer event-driven processing.

A daily tick is only needed if:
- note expiry/staleness;
- time-based sighting confidence;
- location aggregation.

V1 likely does not require a generic bestiary tick.

---

## 139. Discovery State Machine

For normal creature:

```text
unknown
 -> identified
 -> observed
 -> understood
 -> mastered
```

For uncertain entity:

```text
unknown
 -> rumored
 -> suspected
 -> confirmed
 -> observed/understood
```

No backwards transitions.

---

## 140. Knowledge Monotonicity

Once the player legitimately learns:
- habitat;
- danger;
- butcher yield;

it stays known during campaign.

A later catalog change/migration should not relock knowledge without explicit version migration.

---

## 141. False Sighting / Misidentification

Optional follow-on.

If implemented:
- sighting claim can be wrong;
- bestiary confidence can later correct it.

Do not use false sightings in v1 unless UI/claim model supports distinction cleanly.

---

## 142. Dead Creature Discovery

Finding carcass:
- can identify creature;
- may unlock anatomy/resource evidence;
- should not increment player kill count.

Butcher can still increment butcher count if carcass harvest allowed.

---

## 143. Track Discovery

Tracks can:
- create rumor/suspected sighting;
- contribute to tracking note;
- not necessarily identify species.

If tracker skill exists, identification confidence may improve.

---

## 144. Witness Skill

If canonical skills include:
- survival;
- tracking;
- hunting;
- naturalist;

they may influence confidence.

Do not invent "naturalist skill" solely for bestiary.

---

## 145. Multiple Creature Encounter

One expedition event can include multiple species.

Create separate observation events per creature or one event with normalized child observation IDs.

Counts stay unambiguous.

---

## 146. Swarm/Pack Encounter

A pack of five wolves is one encounter event, not necessarily five encounter-count increments.

Kill count can count individual kills if combat has individual entities.

Document semantics:
- EncounterCount = discrete encounter episodes.

---

## 147. Creature Count vs Species Count

Bestiary is species/type knowledge.

Do not track individual animal identity unless a named unique creature system exists.

Kills count instances; discovery keyed by species/type ID.

---

## 148. Unique/Named Creatures

If boss/unique fauna exist:
- decide whether separate bestiary entries or special notes under species.

Avoid automatic duplication.

---

## 149. Domestic/Feral Classification

"Domestic" should describe origin/ecology, not imply tameability.

Do not show "domesticate" actions unless Plan 151/other system supports them.

---

## 150. Completion UX

Completion tracker should emphasize:
- discovered species;
- notes learned;
- category progress.

Do not turn every sighting into a checklist burden.

Rare discovery should feel valuable via narrative/provenance.

---

## 151. Rare Creature Presentation

For rare creature:
- distinctive notification;
- first encounter history;
- perhaps archive fact.

No oversized mechanical reward required.

---

## 152. "Master" Completion

All completion-eligible creatures discovered.

Optional:
- separate "Field Mastery" for all notes.

This avoids forcing 5 kills of every peaceful/rare creature for basic bestiary completion.

---

## 153. Ethical Hunting Guardrail

Do not make full bestiary completion require killing every species.

Observation and hunting should be separate mastery paths.

This supports non-lethal/exploration play styles.

---

## 154. Butcher Mastery Guardrail

Similarly, butcher notes can remain specialized.

A naturalist-style player can complete species discovery without butchering every creature.

Quest/achievement definitions should distinguish paths.

---

## 155. Collection Design

Healthy loop:

```text
explore
 -> encounter new fauna
 -> record first evidence
 -> revisit varied habitats
 -> learn behavior
 -> optionally hunt/butcher
 -> use knowledge strategically
 -> discover rare species
```

Unhealthy loop:

```text
farm same spawn 10 times
 -> fill bar
```

Tune accordingly.

---

## 156. Bestiary Completion Calibration

Simulate representative campaign paths:

- exploration-heavy;
- combat-heavy;
- non-lethal;
- short campaign;
- long campaign.

Measure:
- species discovered;
- notes;
- rare sightings.

Ensure meaningful progression without mandatory grind.

---

## 157. Data Provenance

For debug detail:
- note source requirement;
- source event IDs;
- first encounter location.

Useful for bug reports.

Do not expose internal IDs in normal UI.

---

## 158. Support Diagnostics

Support report can include:

- creature catalog digest;
- note catalog digest;
- discovered IDs/counts;
- recent event IDs;
- mod set.

No need to include all narrative text.

---

## 159. Release Gate

Release fails if:

- any eligible creature unreachable;
- note references nonexistent fact;
- kill/butcher duplicate tests fail;
- hidden data leaks through panel;
- recent sighting cap breaks;
- old-save migration crashes;
- selftest fails;
- zero-mod/catalog integrity fails.

This plan is UI-visible, but the release gate is primarily data/integration correctness.

---

## 160. Definition of Done — Flagship

### Core
- [ ] `BestiarySystem.cs`
- [ ] schema-versioned state
- [ ] creature discovery records
- [ ] sightings
- [ ] note definitions/evaluator
- [ ] stable event IDs
- [ ] no duplicate creature catalog

### Tracking
- [ ] encounter count
- [ ] kill count
- [ ] butcher count
- [ ] first/last encounter
- [ ] type/location aggregates
- [ ] bounded sighting log

### Knowledge
- [ ] unknown/identified/observed/understood/mastered
- [ ] cryptid uncertainty if supported
- [ ] 3/5/10-style configurable thresholds
- [ ] kill notes
- [ ] butcher notes
- [ ] no relock
- [ ] no unsupported mechanics in text

### Integrations
- [ ] creature catalog
- [ ] expedition
- [ ] combat
- [ ] hunting/trapping where applicable
- [ ] butcher/resource pipeline
- [ ] journal
- [ ] weather/habitat
- [ ] map projection
- [ ] quests/achievements
- [ ] epilogue/archive facts

### UI
- [ ] creature list
- [ ] detail
- [ ] filters
- [ ] sort
- [ ] search
- [ ] completion
- [ ] sightings
- [ ] accessibility
- [ ] no business rules

### Validation
- [ ] all eligible creatures reachable
- [ ] all note refs valid
- [ ] old save
- [ ] deterministic sightings
- [ ] anti-farming
- [ ] selftest
- [ ] data integrity
- [ ] headless

---

## 161. Follow-On Task 187-A — Creature Illustration Progression

Goal:
unlock richer visual art as knowledge increases.

Stages:
- silhouette;
- field sketch;
- full illustration.

Requires asset pipeline/registry.

No state redesign needed.

---

## 162. Follow-On Task 187-B — Trophy & Specimen System

Goal:
allow select hunted creatures to create physical trophies/specimens.

Requires:
- item/decor;
- hunting ethics/balance;
- no requirement for base bestiary completion.

Bestiary supplies source species knowledge.

---

## 163. Follow-On Task 187-C — Habitat Mapping

Goal:
turn repeated confirmed sightings into map range knowledge.

Requires:
- location graph;
- sighting aggregates;
- no revelation of unseen canonical spawn tables.

Bestiary can export observed-range data.

---

## 164. Follow-On Task 187-D — Seasonal Migration Tracking

Goal:
show changing creature distributions over time.

Requires:
- real ecology/migration authority;
- seasonal/weather integration;
- historical sightings.

Do not implement as bestiary-only fake movement.

---

## 165. Follow-On Task 187-E — Naturalist Role/Skill Integration

Goal:
allow a canonical survivor skill/profession to improve observation confidence.

Only if existing skills/backstory systems support it.

No standalone bestiary XP system.

---

## 166. Follow-On Task 187-F — Creature Domestication Bridge

Goal:
connect confirmed domestic/feral species to Plan 151 working animals.

Requires:
- actual tameable species;
- explicit domestication system;
- bestiary remains knowledge source only.

---

## 167. Follow-On Task 187-G — Field Research Expeditions

Goal:
support observation-focused expeditions.

Possible objectives:
- photograph/sketch;
- track;
- collect non-lethal evidence;
- confirm habitat.

This gives non-lethal bestiary progression.

---

## 168. Final Guardrails

- No second creature spawn system.
- No second combat authority.
- No second butcher-yield authority.
- No per-frame bestiary polling.
- No `System.Random`.
- No wall-clock sightings.
- No duplicate encounter event count.
- No duplicate kill count.
- No duplicate butcher count.
- No hardcoded `/24` completion in UI.
- No unverified cryptid treated as confirmed.
- No search/sort leakage of hidden creature facts.
- No note text promising mechanics that do not exist.
- No full completion requiring killing every species.
- No note unlock relock.
- No sighting log growing without bound.
- No retroactive fake old-save kill history.
- No UI-owned thresholds.
- No location evolution mutated merely because bestiary observed fauna.
- No habitat revealed before knowledge unlock unless intentionally public.
- No 6–8-note filler quota at the cost of quality.
- No "rare = impossible" completion blocker without reachability proof.

When complete, Plan 187 should make wildlife encounters accumulate into useful knowledge rather than disappear
when an expedition ends. The player will remember where a creature was first seen, distinguish rumor from
confirmation, understand which species have actually been hunted or butchered, unlock grounded field notes from
experience, and build a bestiary that reflects the campaign they actually lived.

The flagship proof is that the bestiary remains a knowledge layer: authoritative creature, combat, expedition,
weather, habitat, and resource systems continue to own world truth, while BestiarySystem turns their events
into deterministic, persistent, strategically useful player knowledge.
