# D1 Flagship Integration Plan [12]
## Plan 178 — Art & Culture Creation System

> **Purpose:** Turn survivor creativity from passive hobby flavor into a deterministic, mechanically bounded,
> save-compatible cultural production layer where survivors create art, music, writing, performances, and
> cultural artifacts that can be displayed, performed, traded, archived, remembered, and integrated into
> shelter identity.
>
> **Primary source:** Plan 178 — Art & Culture Creation System.
>
> **Core design problem:** ASHFALL already contains morale systems, survivor skills, decor placement, crafting,
> hobbies/leisure, seasonal events, archive hooks, and faction standing, but no authoritative cultural-production
> system exists. Survivors may consume books and radio or pursue creative hobbies, yet they cannot create durable
> cultural works whose existence affects shelter life.
>
> **Implementation posture:** deterministic, data-driven, low-micromanagement, authored-fragment based,
> save-safe, bounded in economic/morale power, and integrated through existing hobby, skill, decor, crafting,
> relations, faction, archive, seasonal-event, achievement, and epilogue authorities rather than duplicating them.
>
> **Critical guardrail:** art creation must not become a second crafting grind. Hobbies represent personal leisure;
> ArtCreationSystem represents the relatively rare creation of a durable cultural work or performance worth
> remembering. Most hobby sessions should not produce inventory-like art objects.

---

## 1. Source Problem Statement

The source plan identifies a straightforward but important cultural gap:

- no `ArtCreationSystem`;
- no cultural artifact authority;
- no music composition;
- no poetry/literature creation;
- no theater/performance system;
- no art gallery;
- no cultural identity derived from created works.

Existing systems provide the necessary building blocks:

```text
Hobby / free-time participation
        ↓
creative opportunity
        ↓
ArtCreationSystem
        ↓
durable cultural work / performance plan
        ↓
 ┌──────────────┬───────────────┬───────────────┐
 ↓              ↓               ↓               ↓
Decor        Needs/Morale     Factions        Archive
 ↓              ↓               ↓               ↓
display       audience        trade/repute    memory
        └──────────────┬───────────────┘
                       ↓
               Shelter Identity
                       ↓
              Seasonal Events / Epilogue
```

The new system owns **cultural works and cultural-event records**. It does not own survivor skills, morale,
inventory, faction standing, rooms, crafting recipes, hobbies, or final shelter identity.

---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `ArtCreationSystem.cs` exists with schema-versioned capture/restore.
2. `art_templates.json` is the canonical art-definition authority.
3. Six source art forms are represented only where their required facilities/materials and gameplay hooks exist.
4. Created works receive stable IDs and deterministic structural properties.
5. Art creation never depends on UI scene state or wall-clock randomness.
6. Starting or completing a work consumes canonical survivor time and inventory resources exactly once.
7. Artwork quality is deterministic under fixed inputs/seeds.
8. Cultural themes are selected from authored, compatible definitions.
9. Displayable visual works integrate with `ShelterDecorSystem`.
10. Performable works use a cultural-event/performance contract rather than becoming fake decor items.
11. Morale effects route through `NeedsSystem`.
12. Skill progression routes through `SkillProgressionSystem`.
13. Materials route through `CraftingSystem`/inventory authorities.
14. Art trading routes through economy/faction systems.
15. Seasonal festivals can host cultural events without duplicating Plan 170.
16. Hobby/leisure integration uses Plan 161 as the source of creative participation, not a second hobby system.
17. Cultural identity tags are exported to, not owned in parallel with, ShelterIdentity.
18. Old saves initialize with empty art state and no retroactive fabricated masterpieces.
19. A save/reload during creation or between performance planning and execution remains idempotent.
20. Artworks can survive creator death, relocation, and display changes.
21. Trade/export of artworks preserves provenance.
22. The system remains valid with zero art created.
23. Extensive art does not provide uncapped morale.
24. An artwork's economic value cannot be duplicated by repeatedly trading/copying it.
25. `--art-creation-selftest` proves creation, display, event, trade, save/load, and deterministic outcomes.

---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/culture/ART_CULTURE_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`
- `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`
- inventory/item transaction APIs
- shelter room/facility capabilities
- Plan 161 hobby/leisure system if implemented
- Plan 170 seasonal events
- Plan 162 shelter archive
- Plan 166 shelter identity
- faction standing/trade systems
- quest runtime
- journal runtime
- survivor relations
- save schema
- deterministic RNG authority
- localization loader
- asset registry/provenance
- achievements
- unified ending/epilogue
- modding contract if Plan 165 is implemented

For each integration, record:

| Concern | Canonical owner | Read/Write contract | Persistent? | Art system role |
|---|---|---|---:|---|
| creative proficiency | SkillProgression | | | read/request XP |
| morale | Needs | | | effect request |
| decor placement | ShelterDecor | | | display adapter |
| materials | Inventory/Crafting | | | transaction |
| faction standing | Faction system | | | trade consequence |
| archive | ShelterArchive | | | notable-event export |
| identity | ShelterIdentity | | | tag/fact export |

Do not invent an `art skill` if the repository has no such canonical skill. Use real skill/hobby proficiency inputs.

---

## 4. Scope Boundary

### In scope

- cultural work definitions;
- artwork creation projects;
- deterministic quality/theme;
- visual work display;
- performance works;
- cultural events;
- art collection/gallery projection;
- bounded ongoing morale from displayed works;
- art-related skill practice;
- trading/transfer;
- cultural identity facts;
- archive/journal hooks;
- seasonal-event hooks;
- save/load and migration;
- CI/selftests.

### Out of scope for first pass

- procedural image/audio generation at runtime;
- external generative AI calls;
- arbitrary user image imports;
- full auction-house simulation;
- copyright/licensing platform;
- profile-wide art gallery;
- cross-campaign inheritance;
- full performance minigame;
- arbitrary text composition engine;
- a second crafting recipe system.

All cultural content should remain deterministic and offline-safe.

---

## 5. Canonical Art Definition

Recommended:

```csharp
public sealed record ArtTemplateDefinition
{
    public string TemplateId { get; init; }
    public string FormId { get; init; }
    public string TitlePatternKey { get; init; }
    public string DescriptionPatternKey { get; init; }

    public IReadOnlyList<string> AllowedThemeIds { get; init; }
    public IReadOnlyList<ItemCost> MaterialCosts { get; init; }
    public string? RequiredFacilityCapabilityId { get; init; }

    public int CreationTimeHours { get; init; }
    public int BaseDifficulty { get; init; }
    public int BaseCulturalValue { get; init; }

    public string? RelatedSkillId { get; init; }
    public string? HobbyId { get; init; }
}
```

Definitions are authored recipes for cultural production, not finished works.

---

## 6. Artwork Runtime Record

Recommended:

```csharp
public sealed record Artwork
{
    public string ArtworkId { get; init; }
    public string TemplateId { get; init; }
    public string CreatorSurvivorId { get; init; }
    public int CreationDay { get; init; }

    public string FormId { get; init; }
    public string ThemeId { get; init; }
    public int Quality { get; init; }
    public int CulturalValue { get; init; }

    public ArtworkStatus Status { get; init; }
    public string? DisplayLocationId { get; init; }
    public string? CurrentOwnerId { get; init; }

    public IReadOnlyList<string> ProvenanceFactIds { get; init; }
    public string GenerationDigest { get; init; }
}
```

Do not persist only generated English title/description if localization may change. Persist selected pattern/fact IDs.

---

## 7. Cultural Event Runtime Record

```csharp
public sealed record CulturalEventRecord
{
    public string EventInstanceId { get; init; }
    public string EventTypeId { get; init; }
    public int Day { get; init; }

    public IReadOnlyList<string> ArtworkIds { get; init; }
    public IReadOnlyList<string> PerformerIds { get; init; }
    public IReadOnlyList<string> AudienceIds { get; init; }

    public int Quality { get; init; }
    public int CulturalImpact { get; init; }
    public bool Memorable { get; init; }
}
```

The record describes what happened. Needs/relations/archive own downstream effects.

---

## 8. Art Creation State

```csharp
public sealed record ArtCreationState
{
    public int SchemaVersion { get; init; }
    public IReadOnlyList<Artwork> Artworks { get; init; }
    public IReadOnlyList<ArtCreationProject> ActiveProjects { get; init; }
    public IReadOnlyList<CulturalEventRecord> CulturalEvents { get; init; }
    public IReadOnlySet<string> EmittedMilestoneIds { get; init; }
}
```

Do not store a duplicate "total cultural value" if it can be derived safely; if persisted for performance, validate it
against records during selftests.

---

## 9. Art Forms

Source forms:

- painting;
- sculpture;
- music;
- poetry;
- storytelling;
- theater.

Treat these as distinct production/performance profiles.

### Visual durable works
- painting;
- sculpture.

### Written/oral durable works
- poetry;
- storytelling.

### Performative works
- music;
- theater.

A musical composition or play may exist as a durable cultural work even before any performance.

---

## 10. Form Capability Matrix

Generate a data-backed matrix:

| Form | Material cost | Facility | Durable work | Displayable | Performable |
|---|---|---|---:|---:|---:|
| painting | pigments/support | studio-like | yes | yes | no |
| sculpture | material/tools | workshop/studio | yes | yes | no |
| music | instrument/notation optional | performance space | yes | maybe | yes |
| poetry | paper/ink optional | quiet space | yes | yes/post | yes/read |
| storytelling | none | common space | optional record | no | yes |
| theater | props/text optional | performance space | yes | no | yes |

Final requirements must use real shelter capabilities/items.

---

## 11. Art Themes

Source themes:

- hope;
- loss;
- resistance;
- nature;
- love;
- death.

Theme is narrative/cultural metadata, not a direct global buff.

Each theme can influence:
- which description fragment is selected;
- which cultural identity tags are emitted;
- which factions/visitors may value a work;
- which seasonal/memorial events it fits.

Do not hardcode "hope = strongest morale."

---

## 12. Theme Selection

Theme selection inputs may include:

- explicit player commission;
- creator backstory/motivation;
- recent memorial/crisis facts;
- seasonal event context;
- hobby preference;
- deterministic fallback.

Selection must be stable once work creation begins.

---

## 13. Creator Eligibility

A survivor can begin art creation only if:

- alive;
- present;
- available;
- sufficient skill/hobby proficiency;
- required facility available;
- required materials available;
- enough time can be scheduled;
- no critical needs override.

Use canonical scheduling/availability authority.

---

## 14. Player-Directed vs Autonomous Creation

Support two routes:

### Player commission
Player chooses:
- creator;
- form/template;
- optionally theme.

### Autonomous creative project
Plan 161/autonomy may suggest a qualifying survivor begins a work during free time.

Autonomous creation must respect resource-policy limits to avoid silently consuming scarce survival supplies.

---

## 15. Hobby Integration

Critical separation:

```text
Hobby session = personal leisure/practice.
Art project = durable cultural output.
```

A high-proficiency painting hobby may make a survivor eligible to begin a painting project.

Do not create one artwork per hobby session.

---

## 16. Creation Project State Machine

```text
planned
 -> active
 -> completed
```

Optional:
- paused;
- cancelled;
- interrupted.

Persist:
- project ID;
- creator;
- template/theme;
- start day/hour;
- progress;
- resource transaction state;
- deterministic seed.

---

## 17. Creation Time

Source says creation uses ticks.

Prefer game-time hours/days aligned with scheduler.

Data:
```text
creationTimeHours
```

Longer works may span several free-time windows.

Do not block a survivor continuously if project is intended as off-hours work.

---

## 18. Time-Sliced Creative Projects

For multi-session works:

```text
project progress += valid creative hours
```

The survivor can:
- work normal duties;
- return later;
- be interrupted.

This integrates naturally with hobby/autonomy.

---

## 19. Material Reservation

Use canonical inventory transactions.

Decide per template:
- all materials consumed at project start;
- or staged consumption.

Prefer simple start-time reservation for v1.

Cancellation policy must be explicit.

---

## 20. Material Safety

Autonomous projects must not consume:
- quest-critical items;
- emergency medicine;
- equipped weapons;
- critical food/fuel reserves;

unless player policy explicitly allows.

Prefer dedicated art/craft materials or cheap scrap.

---

## 21. Quality Formula

Suggested:

```text
quality =
  base template factor
  + creator skill contribution
  + hobby proficiency contribution
  + facility quality
  + material quality if canonical
  + inspiration/context modifier
  + bounded deterministic variation
  - interruption/shortage penalties
```

Clamp 0..100.

Only use inputs that actually exist.

---

## 22. Deterministic Quality RNG

If variation is used:

Seed:
```text
campaignSeed
+ artworkId
+ templateId
+ creatorId
```

Persist seed/project ID.

Reload cannot reroll quality.

---

## 23. Quality Bands

Possible:

- rough: 0–24;
- competent: 25–49;
- notable: 50–74;
- exceptional: 75–89;
- masterwork: 90–100.

Use data-driven thresholds.

"Masterwork" milestone fires once.

---

## 24. Cultural Value

Cultural value should not simply equal quality.

Possible inputs:
- quality;
- rarity/form;
- theme relevance;
- event history;
- creator significance;
- memorial provenance;
- age/history.

Keep deterministic.

Cultural value drives identity/archive/trade context; it should not be another morale bar.

---

## 25. Provenance Facts

An artwork can preserve context:

- created after major raid;
- dedicated to deceased survivor;
- created for Founding Day;
- produced by master hobbyist;
- first work of a form;
- performed during winter festival.

Persist stable fact IDs.

This is what makes cultural artifacts narratively valuable.

---

## 26. Title and Description Composition

Use authored patterns.

Example structural selection:

```text
titlePatternId
descriptionPatternId
themeId
creatorId
dedicationFactId
```

Render localized text from these.

No runtime LLM.

---

## 27. Artwork Naming

If player can rename:
- stable artwork ID remains unchanged;
- custom display name stored safely;
- length/rich-text sanitized.

Generated default name remains available.

---

## 28. Visual Art Display

Integrate with `ShelterDecorSystem`.

ArtCreationSystem provides:

```text
ArtworkDecorDescriptor
```

Decor system owns:
- room slot;
- placement;
- movement;
- visibility.

Artwork state stores current display location/reference only through adapter results.

---

## 29. Display Morale Effects

The source proposes ongoing morale.

Use very small bounded effects.

Prefer:
- room-local or periodic cultural-environment effect;
- diminishing returns;
- quality/cultural variety.

Avoid stacking 100 paintings into enormous passive morale.

---

## 30. Display Diminishing Returns

Potential:

```text
first displayed cultural work: full effect
second/third distinct forms: reduced
additional same-form works: mostly identity/decor value
```

This encourages variety over spam.

---

## 31. Display Damage / Loss

If decor system supports damage/destruction:
- artwork can be damaged/lost;
- archive provenance remains;
- cultural identity updates;
- journal may record destruction if notable.

Do not create independent room-damage simulation.

---

## 32. Creator Death

Artwork persists after creator death.

This is important.

Effects:
- memorial relevance may increase;
- archive/epilogue can reference creator;
- creator ID remains valid historical reference even if inactive/deceased.

Do not delete art with survivor.

---

## 33. Music Composition

A musical work can have:

- composition record;
- creator;
- theme;
- quality;
- performance history.

Instrument/equipment is required for performance if canonical, not necessarily for composition.

---

## 34. Poetry / Literature

A written work can:
- exist as cultural record;
- be posted/displayed;
- be read at event;
- be archived.

Use paper/ink only if such item costs are meaningful and not artificially added.

---

## 35. Storytelling

Storytelling may create:
- a named oral tale;
- performance record;
- archive fact.

Because no material cost exists, time/skill/rarity must prevent infinite creation spam.

Require a meaningful creative threshold/cooldown.

---

## 36. Theater

Theater is inherently multi-survivor.

Separate:
- authored play/work;
- scheduled performance.

Creation can have playwright/director.
Performance has cast/audience.

---

## 37. Performance Scheduling

Use canonical scheduler.

Inputs:
- work;
- performers;
- space;
- audience eligibility;
- duration;
- optional seasonal-event context.

No performance during emergency unless explicitly allowed.

---

## 38. Cultural Event Types

Source:

- exhibition;
- concert;
- reading;
- performance;
- workshop.

All are data-driven event profiles.

---

## 39. Exhibition

Inputs:
- multiple visual/written artworks;
- display space;
- audience.

Outputs:
- bounded morale;
- cultural impact;
- archive fact if memorable;
- identity tags.

---

## 40. Concert

Inputs:
- one or more musical works;
- performers;
- instruments/space.

Outputs:
- performance event;
- morale/social effects;
- performance history.

---

## 41. Reading

Poetry/story reading.

Low-cost event.

Useful during scarcity.

Integrates with Plan 170 festivals.

---

## 42. Theater Performance

Requires:
- work;
- multiple participants;
- space;
- schedule.

Use relations/hobby skill facts where relevant.

No separate acting skill unless canonical.

---

## 43. Workshop

Workshop is teaching.

Route to:
- Plan 161 hobby teaching;
- apprenticeship/skill system.

ArtCreationSystem hosts event record but does not own skill progression.

---

## 44. Audience Participation

Determine eligible audience:
- shelter present;
- awake/available;
- not critically ill/on duty.

Attendance may use Plan 170 participation logic or shared utility.

Do not duplicate two separate attendance algorithms if celebrations already have one.

---

## 45. Shared Cultural Event Utility

If Plan 170 exists, extract reusable:

```text
ParticipantEligibility
EventAttendanceResolver
EventResourceTransaction
```

Art events and seasonal celebrations can share infrastructure.

Avoid circular dependencies.

---

## 46. Morale Effects From Events

ArtCreationSystem emits typed event:

```text
CulturalEventMoraleEffect
```

NeedsSystem applies.

Cap by:
- event type;
- participation rate;
- quality.

No double effect if event also occurs inside seasonal celebration.

---

## 47. Seasonal Event Integration

Plan 170 can host:

- concert at midsummer;
- reading at remembrance;
- exhibition at founding day;
- theater at tradition festival.

Use one event instance linked to both systems.

Prevent double morale/resource application.

---

## 48. Cultural Identity

Do not store one authoritative `totalCultureScore` if Plan 166 owns identity.

Export facts:

```text
culture.artistic
culture.musical
culture.literary
culture.performing
culture.memorial
culture.resistance
```

Identity system aggregates.

---

## 49. Identity Thresholds

Examples:
- artistic: N displayed visual works;
- musical: repeated performances;
- literary: written works/readings;
- theatrical: multiple performances.

Thresholds should be data-driven and bounded.

ArtCreationSystem may calculate evidence counts, Identity system owns final tag if that is its architecture.

---

## 50. Visitor Perception Integration

Source says cultural identity affects visitors.

Do not alter visitor logic directly.

Export:
- culture tags;
- cultural reputation band;
- notable work facts.

Visitor/faction systems decide reaction.

---

## 51. Faction Trading

Artwork can be transferred through economy/trade.

Create:

```csharp
ArtworkTradeDescriptor
{
    artworkId,
    owner,
    baseValue,
    culturalTags,
    qualityBand
}
```

Economy owns price and transaction.

---

## 52. Art Trade Value

Value may depend on:
- quality;
- cultural value;
- rarity;
- faction preferences;
- provenance.

Do not let cultural value map 1:1 to currency.

Trade balance needs caps.

---

## 53. Faction Preferences

Faction art preferences should live in faction/trade data.

Examples:
- one faction values resistance art;
- another values technical sculpture;
- another is indifferent.

ArtCreationSystem does not store faction preference tables unless no better authority exists.

---

## 54. Standing Effects From Trade

Trading a meaningful artwork may emit:
- normal economic trade;
- optional cultural diplomacy effect.

Faction system owns standing.

Prevent repeated standing gain if same artwork is traded back/forth.

---

## 55. Ownership and Transfer

Artwork has one current owner/location.

Transfer is transactional.

States:
- shelter-owned;
- faction-owned;
- destroyed/lost;
- loaned only if later supported.

Do not clone artwork on trade.

---

## 56. Anti-Duplication

Stable artwork ID plus ownership prevents:
- selling same painting twice;
- displaying after sold;
- archive incorrectly treating copies as originals.

If reproduction/prints later exist, they need separate IDs/derivative contract.

---

## 57. Crafting Integration

Art materials come from canonical item/crafting systems.

ArtCreationSystem consumes items; it does not define how pigments, paper, canvas, instruments, clay, etc. are crafted.

If an art material does not exist, either:
- choose existing material;
- or create it in a separate item/crafting content task.

---

## 58. Skill Progression

Creation can emit practice XP.

Performance can emit practice XP.

Workshops can emit teaching XP.

Skill system owns final level.

Add daily/weekly caps to avoid art XP farming.

---

## 59. Hobby Proficiency vs Skill

If Plan 161 already tracks hobby proficiency:
- use that as creative-practice input;
- do not invent a parallel art proficiency.

If a canonical art-related skill exists, both may contribute with diminishing weights.

---

## 60. Inspiration

The source mentions inspiration.

Do not add an inspiration meter in v1.

Use contextual modifier facts:
- recent major event;
- memorial;
- seasonal celebration;
- backstory relevance;
- relationship milestone.

These modify theme/quality slightly.

---

## 61. Masterwork

Masterwork criteria:
- quality >= configured threshold;
- first-time milestone per artwork;
- optionally high cultural value.

Emit:
`MasterworkCreated(artworkId, creatorId)` once.

Achievement/quest/archive can consume.

---

## 62. Cultural Significance Over Time

Cultural value may increase when:
- displayed through major events;
- used in memorial;
- survives many years;
- linked to founding/tradition;
- creator becomes historically significant.

Avoid ticking every artwork every day.

Update only on relevant events/year boundaries.

---

## 63. Archive Integration

Archive notable:
- first artwork;
- first masterwork;
- major exhibition;
- famous concert;
- creator death + preserved work;
- traded cultural icon;
- oldest surviving artwork.

No need to archive every rough sketch.

---

## 64. Journal Integration

Journal:
- project completion;
- masterwork;
- major performance;
- significant trade;
- destroyed famous work.

Routine creative practice stays out of journal.

---

## 65. Quest Hooks

Source hooks:
- The Artist;
- The Master;
- The Exhibition;
- The Concert;
- The Culture;
- The Teacher;
- The Legacy.

Export typed facts.

QuestSystem owns quest state/rewards.

---

## 66. Achievement Integration

Plan 149 can observe:
- first created work;
- masterwork;
- exhibition;
- cultural identity tag;
- number of unique forms;
- teaching milestone.

ArtCreationSystem does not own rewards/meta progression.

---

## 67. Epilogue Integration

Plan 145 may consume:
- notable masterwork;
- creator legacy;
- cultural identity;
- famous performance;
- memorial artwork;
- surviving art collection.

Export stable fact IDs/provenance.

---

## 68. Backstory Integration

Plan 174 may provide:
- artist occupation;
- prior musician/writer experience;
- motivation/defining moment.

These can influence eligibility/theme but do not rewrite backstory.

---

## 69. Cultural Work Provenance

Recommended facts:
- creator;
- creation day;
- theme;
- source event;
- dedication;
- display history count;
- performance count;
- ownership transfers.

Persist only bounded important history, not every room movement.

---

## 70. Old-Save Migration

Source says empty art state.

Migration:
- initialize schema;
- no artworks;
- no active projects;
- no cultural events;
- no retroactive art generated from hobbies;
- no retroactive morale;
- identity system receives zero art facts until new work exists.

Safe and simple.

---

## 71. Save/Load During Creation

Test:

1. project started;
2. materials committed;
3. partial progress;
4. save;
5. reload;
6. finish.

Expected:
- no duplicate cost;
- same quality/theme;
- one artwork ID;
- one completion event.

---

## 72. Save/Load Before Performance

Persist:
- event plan;
- performers;
- work IDs;
- scheduled time;
- seed;
- resource reservation if any.

Reload must not regenerate audience/result.

---

## 73. Stable IDs

Artwork:
```text
artwork:<campaignId>:<sequence>
```

Project:
```text
art_project:<campaignId>:<sequence>
```

Cultural event:
```text
culture_event:<campaignId>:<sequence>
```

Do not derive identity from generated title.

---

## 74. Idempotent Completion

Stable effect IDs:

```text
artwork_created:<artworkId>
cultural_event_completed:<eventId>
art_trade:<artworkId>:<transactionId>
```

Use to prevent duplicate downstream effects.

---

## 75. UI — Gallery

Gallery shows:
- artwork title;
- form;
- creator;
- quality band;
- theme;
- current location/owner;
- provenance highlights;
- display status.

Support filtering by form/creator/theme.

---

## 76. UI — Creation Panel

Show:
- eligible creators;
- art forms/templates;
- required time;
- materials;
- facility;
- expected quality band;
- optional theme choice.

No exact RNG roll.

---

## 77. UI — Performance Panel

Show:
- work;
- performers;
- venue;
- expected audience;
- schedule;
- event type.

If Plan 170 owns event calendar scheduling, use shared calendar integration.

---

## 78. UI — Cultural Identity

Show exported culture tags and evidence:

```text
Musical
- 4 compositions
- 3 concerts
- 1 masterwork
```

Avoid opaque culture score if identity system can explain tags.

---

## 79. Accessibility

Support:
- keyboard/controller navigation;
- text scaling;
- quality not color-only;
- clear ownership/display state;
- no hover-only provenance;
- reduced-motion event animations;
- readable gallery with large collections.

---

## 80. Art Template Catalog

Create:

`Assets/StreamingAssets/Data/art_templates.json`

Suggested root:

```json
{
  "schemaVersion": 1,
  "forms": [],
  "themes": [],
  "templates": [],
  "eventProfiles": [],
  "qualityBands": []
}
```

---

## 81. 15-Template Strategy

Do not create 15 mechanically identical recipes.

Suggested:
- 3 painting;
- 2 sculpture;
- 3 music;
- 2 poetry;
- 2 storytelling;
- 3 theater/performance work types.

Actual balance should follow supported materials/facilities.

---

## 82. Template Differentiation

Each template should differ by at least two:

- form;
- material profile;
- time;
- facility;
- difficulty;
- theme restrictions;
- performer count;
- event compatibility;
- trade/cultural profile.

Reject filler.

---

## 83. Catalog Validation

Validate:
- unique form/theme/template IDs;
- valid localization keys;
- item references;
- facility capabilities;
- related skill/hobby IDs;
- event profiles;
- quality bands;
- legal time/costs;
- allowed themes;
- no impossible performer counts.

---

## 84. Theme Validation

Every template must have at least one reachable theme.

If theme requires context:
- fallback theme exists.

No project should get stuck because no theme matches.

---

## 85. Data Integrity Self-Test

Extend standard integrity:

- template IDs;
- item IDs;
- facility IDs;
- skill/hobby refs;
- theme IDs;
- cultural event profiles;
- trade descriptor compatibility;
- decor adapter compatibility.

Fail early before campaign load.

---

## 86. Dedicated `--art-creation-selftest`

It should:

1. load catalog;
2. create eligible survivor fixture;
3. start visual art project;
4. commit materials;
5. save/reload mid-project;
6. complete;
7. verify deterministic quality/theme/digest;
8. display through decor adapter;
9. schedule performance work;
10. execute cultural event;
11. verify morale event once;
12. trade artwork;
13. verify ownership changes;
14. verify old-save empty migration;
15. verify no-art campaign;
16. verify masterwork milestone once;
17. verify headless operation;
18. exit non-zero on mismatch.

---

## 87. Unit Test Matrix

### Catalog
- valid;
- invalid item;
- invalid theme;
- invalid facility;
- duplicate ID.

### Creation
- eligible;
- insufficient skill;
- missing facility;
- missing material;
- partial progress;
- interruption;
- completion.

### Quality
- deterministic;
- bounds;
- masterwork threshold.

### Display
- place;
- move;
- remove;
- sold work cannot display.

### Performance
- valid cast;
- absent performer;
- audience;
- event completion.

### Trade
- value descriptor;
- transfer;
- cannot duplicate.

### Persistence
- empty;
- project;
- artwork;
- event;
- old save.

---

## 88. Golden Scenarios

1. First rough painting.
2. Skilled painter produces notable work.
3. Master hobbyist creates masterwork.
4. Sculpture displayed in shelter.
5. Music composed then performed.
6. Poetry reading during scarcity.
7. Storytelling performance without materials.
8. Theater with several performers.
9. Exhibition during Plan 170 festival.
10. Artwork dedicated to deceased survivor.
11. Artwork traded to faction.
12. Creator dies but artwork remains.
13. Old save with no art.
14. Large art collection with diminishing display morale.
15. Save/load during long project.

---

## 89. Property / Fuzz Testing

Properties:
- quality always 0..100;
- cultural value bounded;
- stable project inputs => stable result;
- each artwork has one owner;
- sold artwork not shelter-displayed;
- project completion at most once;
- event completion at most once;
- no unresolved template/theme IDs;
- no duplicate stable artwork IDs.

---

## 90. Art Economy Exploit Tests

Test:
- create -> trade -> reacquire -> retrade;
- no repeated standing reward without policy;
- no duplication on save/load;
- no negative material costs;
- no zero-time profitable art loop;
- no art value exceeding sane balance threshold without exceptional provenance.

---

## 91. Morale Exploit Tests

Test:
- display 100 artworks;
- repeat same concert daily;
- stack exhibition + seasonal celebration;
- multiple identical story events.

Ensure caps/diminishing returns.

---

## 92. Performance Budget

Art creation is low-frequency.

Requirements:
- catalog parsed once;
- active project ticking O(active projects);
- no per-frame quality calculation;
- gallery indexing by ID/form/creator;
- display morale recalculated on display changes, not every frame;
- cultural identity facts updated on relevant events.

Large collections remain cheap.

---

## 93. Save Footprint

Persist:
- compact artwork structural record;
- active projects;
- notable cultural events;
- ownership/display refs.

Do not persist:
- rendered localized prose;
- redundant image/audio bytes;
- every audience member for routine events unless needed.

---

## 94. Artwork History Retention

For notable works, retain:
- creator;
- creation day;
- key performance count;
- major exhibition/trade facts.

Do not store every display move.

Keep history bounded.

---

## 95. Cultural Event Retention

Archive:
- memorable events;
- aggregate counts for routine ones.

This keeps save growth manageable.

---

## 96. Modding Integration

If Plan 165 exists, art templates can become a public data catalog after stabilization.

Mods may add:
- themes;
- templates;
- event profiles;
- localization;
- registered assets.

No executable quality formulas or arbitrary scripts.

---

## 97. Modded Artwork Save Safety

If a save contains artwork from removed mod:
- preserve structural artwork ID/provenance if possible;
- Plan 165 reports missing content;
- do not reroll/replace with base artwork.

If required template is missing, artwork may become inert historical placeholder under mod compatibility policy.

---

## 98. Narrative Quality Review

Export 50 generated structural works and rendered descriptions.

Review:
- repetitive names;
- theme mismatch;
- "masterwork" inflation;
- implausible materials;
- tone mismatch;
- repeated grief imagery;
- melodrama;
- culturally empty generic wording.

Fix authored templates, not via uncontrolled random prose.

---

## 99. Cultural Identity Calibration

Generate:
`docs/culture/CULTURE_IDENTITY_CALIBRATION.md`

Simulate:
- no art;
- 5 works;
- 20 works;
- mixed forms;
- only one form;
- multiple performances.

Ensure tags are reachable but not instant.

---

## 100. Art Power Budget

Generate:
`docs/culture/ART_BALANCE.md`

For each template:
- time cost;
- material cost;
- expected quality;
- expected morale contribution;
- trade value range;
- cultural-value potential.

Reject obvious profit/morale loops.

---

## 101. Art Coverage Report

Generate:
`docs/culture/ART_COVERAGE.md`

| Form | Templates | Facility-backed | Material-backed | Performance | Decor | Trade | Fixtures |
|---|---:|---:|---:|---:|---:|---:|---:|
| painting | | | | | | | |
| sculpture | | | | | | | |
| music | | | | | | | |
| poetry | | | | | | | |
| storytelling | | | | | | | |
| theater | | | | | | | |

Flag unreachable templates.

---

## 102. Structured Diagnostics

Logs:

```text
ArtProjectStarted project=<id> template=<id> creator=<id>
ArtworkCreated artwork=<id> form=<id> quality=<n>
ArtworkDisplayed artwork=<id> location=<id>
CulturalEventCompleted event=<id> type=<id>
ArtworkTransferred artwork=<id> owner=<id>
MasterworkCreated artwork=<id>
```

No verbose per-tick logs.

---

## 103. Implementation Phase A — Audit and Contracts

Tasks:
1. audit hobby/skill/decor/crafting/needs/faction/event architecture;
2. define art catalog;
3. define artwork/project/event state;
4. loader;
5. validator;
6. save schema;
7. baseline tests.

Exit:
catalog loads and state round-trips.

---

## 104. Implementation Phase B — Creation Projects

Tasks:
1. eligibility;
2. player/autonomous project start;
3. time progression;
4. material transaction;
5. theme;
6. quality;
7. stable IDs/digest;
8. completion idempotency.

Exit:
survivor can create one durable cultural work headlessly.

---

## 105. Implementation Phase C — Hobby/Skill Integration

Tasks:
1. hobby proficiency input;
2. skill input;
3. practice XP output;
4. autonomy opportunity;
5. daily caps;
6. tests.

Exit:
creative hobbies can naturally mature into cultural works.

---

## 106. Implementation Phase D — Display

Tasks:
1. decor descriptor;
2. placement;
3. move/remove;
4. passive bounded morale;
5. creator death;
6. lost/destroyed state;
7. tests.

Exit:
visual works become real shelter objects.

---

## 107. Implementation Phase E — Performances

Tasks:
1. composition/performance distinction;
2. performer eligibility;
3. shared attendance resolver;
4. concert;
5. reading;
6. theater;
7. workshop;
8. morale/social event sinks.

Exit:
performative art works end to end.

---

## 108. Implementation Phase F — Culture/Archive/Seasonal

Tasks:
1. identity fact exports;
2. archive;
3. journal;
4. Plan 170 integration;
5. masterwork milestone;
6. quest/achievement/epilogue hooks.

Exit:
art participates in shelter culture.

---

## 109. Implementation Phase G — Trading

Tasks:
1. trade descriptor;
2. ownership transfer;
3. faction preferences;
4. standing consequence sink;
5. provenance;
6. anti-loop tests.

Exit:
art can move through real economy without duplication.

---

## 110. Implementation Phase H — UI

Tasks:
1. gallery;
2. creation panel;
3. performance scheduler;
4. artwork detail;
5. culture tags;
6. provenance;
7. accessibility;
8. snapshots.

Exit:
UI contains no creation/business logic.

---

## 111. Implementation Phase I — 15 Templates

Tasks:
1. author templates by supported forms;
2. validate materials/facilities;
3. author themes;
4. event compatibility;
5. localization;
6. fixtures;
7. coverage report.

Exit:
15 meaningful templates, no filler.

---

## 112. Implementation Phase J — CI Hardening

Tasks:
1. selftest;
2. data integrity;
3. old-save migration;
4. save/load mid-project;
5. trade duplication;
6. morale stacking;
7. fuzz;
8. performance;
9. documentation;
10. full regression.

Exit:
cultural production is deterministic and bounded.

---

## 113. Definition of Done — Flagship

### Core
- [ ] `ArtCreationSystem.cs`
- [ ] art forms/themes/templates
- [ ] project state
- [ ] artwork state
- [ ] cultural event state
- [ ] schema-versioned save

### Creation
- [ ] eligibility
- [ ] time
- [ ] materials
- [ ] deterministic quality
- [ ] stable theme
- [ ] provenance
- [ ] masterwork

### Integration
- [ ] hobby
- [ ] skill
- [ ] decor
- [ ] needs
- [ ] crafting/inventory
- [ ] relations
- [ ] faction/economy
- [ ] seasonal events
- [ ] archive
- [ ] identity
- [ ] achievement
- [ ] epilogue

### Content
- [ ] six forms where supported
- [ ] six themes
- [ ] 15 meaningful templates
- [ ] localization
- [ ] no unresolved item/facility IDs

### UI
- [ ] gallery
- [ ] creation
- [ ] performance
- [ ] culture display
- [ ] artwork detail
- [ ] accessibility

### Validation
- [ ] old save
- [ ] project save/load
- [ ] deterministic quality
- [ ] no duplicate trade
- [ ] no morale stacking exploit
- [ ] selftest
- [ ] data integrity
- [ ] headless

---

## 114. Follow-On Task 178-A — Cultural Artifact Provenance & Heritage

Goal:
allow famous works to accumulate history.

Substeps:
1. provenance milestones;
2. age/history facts;
3. creator death link;
4. major display/performance history;
5. archive integration;
6. epilogue facts;
7. bounded save record.

---

## 115. Follow-On Task 178-B — Cultural Competitions

Goal:
use Plan 161 competitions and Plan 170 festivals for art contests.

Requirements:
- deterministic judging;
- no large reward loops;
- quality/provenance inputs;
- journal/achievement hooks.

---

## 116. Follow-On Task 178-C — Cultural Diplomacy

Goal:
allow artworks/performances to influence faction diplomacy more deeply.

Requires:
- faction cultural preference data;
- diplomatic event sink;
- anti-repeat logic;
- no duplicate standing authority.

---

## 117. Follow-On Task 178-D — Art Education Track

Goal:
formalize cultural instruction using canonical education/apprenticeship systems.

Do not create separate school progression inside ArtCreationSystem.

---

## 118. Follow-On Task 178-E — Cultural Legacy / Epilogue

Goal:
allow Plan 145 to remember:
- famous masterwork;
- shelter's artistic identity;
- performance tradition;
- memorial artwork.

Export structural facts only.

---

## 119. Follow-On Task 178-F — Cultural Trading Post

Goal:
support a specialized art exchange only after basic economy integration proves safe.

Requires:
- demand;
- valuation;
- provenance;
- no duplication;
- faction preferences.

Not required for v1.

---

## 120. Final Guardrails

- No artwork per routine hobby session.
- No second crafting system.
- No second skill system.
- No second morale authority.
- No second faction-standing authority.
- No runtime generative AI.
- No wall-clock RNG.
- No save/load quality reroll.
- No duplicate material consumption.
- No duplicate artwork ID.
- No selling the same artwork twice.
- No displaying artwork after ownership transfer.
- No uncapped passive morale from wall spam.
- No uncapped event morale.
- No infinite art-to-money loop.
- No direct culture identity ownership if Plan 166 owns it.
- No direct seasonal-event duplication if Plan 170 owns scheduling.
- No unsupported item/facility IDs created merely to fill templates.
- No deletion of artworks when creator dies.
- No retroactive old-save masterpieces.
- No 15-template filler catalog.

When complete, Plan 178 should make culture materially visible in the shelter. Survivors will occasionally turn
their hobbies, histories, grief, hope, relationships, and skills into durable works and performances. Those
works can hang on walls, be read or performed, become part of seasonal celebrations, travel through trade,
outlive their creators, enter the shelter archive, and shape how the shelter remembers itself—without turning
art into another mandatory production treadmill.

---

## Annex A — Representative Art Template Blueprints

These are structural authoring examples. Final item/facility/skill IDs must come from repository validation.

### A1. Charcoal Wall Study
Form: painting
Materials:
- low-cost pigment/charcoal;
- paper/canvas substitute.

Time:
- short.

Purpose:
- accessible early-game visual work.

Themes:
- loss;
- hope;
- nature.

### A2. Salvaged-Pigment Mural
Form: painting
Facility:
- display wall / suitable common area.

Time:
- long.

Purpose:
- high cultural value;
- shelter identity.

### A3. Scrap-Metal Figure
Form: sculpture
Materials:
- metal scrap;
- tools.

Facility:
- workshop/studio capability.

Trade:
- moderate.

### A4. Memorial Figure
Form: sculpture
Theme:
- death/loss.

Requires:
- memorial fact/dedication.

Strong archive/epilogue hook.

### A5. Shelter Song
Form: music
Creation:
- composition project.

Performance:
- common/performance space.

Theme:
- hope/resistance.

### A6. Lament
Form: music
Theme:
- loss/death.

Memorial/seasonal compatibility.

### A7. March / Defiance Song
Form: music
Theme:
- resistance.

Faction reactions only through preference data.

### A8. Short Poem
Form: poetry
Low materials/time.

Can be read or posted.

### A9. Chronicle Verse
Form: poetry
Higher cultural value when linked to archive event.

### A10. Fireside Story
Form: storytelling
No material cost.
Requires cooldown/time to avoid spam.

### A11. Survivor Tale
Form: storytelling
Can draw from creator backstory/recent event fact.

### A12. Shelter Play
Form: theater
Creation:
- written work.

Performance:
- multi-survivor.

### A13. Memorial Play
Form: theater
Theme:
- loss/death.

### A14. Festival Comedy
Form: theater
Theme:
- hope/love.

Strong Plan 170 integration.

### A15. Founding Pageant
Form: theater/performance.
Theme:
- hope/resistance.
Requires:
- Founding Day or tradition context.

---

## Annex B — Cultural Power Budget Worksheet

For each template calculate:

| Template | Creation hrs | Material cost | Typical quality | Passive morale | Event morale | Trade value | Cultural value |
|---|---:|---:|---:|---:|---:|---:|---:|
| | | | | | | | |

Rules:
1. passive morale stays very small;
2. cultural value can be high without direct stat power;
3. large event morale is capped;
4. high trade value requires meaningful time/material/provenance;
5. no free storytelling loop produces infinite trade or standing;
6. masterworks remain rare because of skill/time/quality requirements, not pure RNG.

---

## Annex C — Cross-System Acceptance Scenario

A survivor with a creative hobby and artist backstory begins a long mural project during valid free-time windows.
The project consumes validated materials once, progresses across several days, and survives save/reload. On
completion, deterministic quality places it in the exceptional band and emits one creation event.

The player displays the mural in a common room through ShelterDecorSystem. NeedsSystem receives only the bounded
display environment effect. Later, Plan 170 schedules a Founding Day exhibition containing the mural plus two
other works. The attendance resolver runs once, the celebration and cultural-event systems share one event
identity, and morale is not applied twice.

Years later, the creator has died. The mural remains. ShelterArchive records it as a notable cultural artifact,
ShelterIdentity retains an artistic tag, and Plan 145 can reference the work structurally in the ending.

This scenario is the flagship proof that the system creates persistent culture rather than transient morale
buttons.

---

## 121. Cultural Work Creation Arbitration

Art projects must coexist with:

- normal work duties;
- hobby sessions;
- education;
- medical treatment;
- expeditions;
- seasonal events;
- emergency response.

Recommended scheduling priority:

```text
critical survival/emergency
> mandatory treatment
> required duty
> scheduled expedition
> education/apprenticeship
> commissioned cultural project
> autonomous cultural project
> routine hobby/leisure
> idle
```

This ordering should live in the canonical scheduler/autonomy layer, not inside `ArtCreationSystem`.

The art system exposes:
- project opportunity;
- required duration;
- resource/facility requirements;
- interruption behavior.

The scheduler decides when the survivor works on it.

---

## 122. Project Interruption Semantics

Art projects may be interrupted by:

- raid;
- fire;
- medical crisis;
- mandatory expedition;
- facility loss;
- material loss if staged;
- creator incapacitation.

Project states:

```text
active
paused
cancelled
completed
```

Paused projects keep:
- project ID;
- progress;
- deterministic result seed;
- consumed/reserved material state.

Resuming cannot reroll theme or quality.

---

## 123. Creator Incapacitation

If creator becomes temporarily unavailable:
- project pauses;
- no progress occurs;
- no quality penalty unless design explicitly says prolonged interruption matters.

If creator dies before completion:
- project becomes `orphaned`.

Possible v1 outcomes:
- project cancelled and historical unfinished-work fact retained;
- or another qualified survivor may complete it only through an explicit handoff mechanic.

Do not silently reassign creator.

---

## 124. Unfinished Works

Persist unfinished notable works only where needed.

A cancelled rough sketch does not need permanent archive state.

Potential rule:
- if project >= 50% complete or culturally linked to a major event, create an `UnfinishedCulturalWorkFact`;
- otherwise discard project after cancellation.

This provides narrative texture without save bloat.

---

## 125. Collaborative Art

Some works may support multiple creators.

Examples:
- mural;
- musical composition;
- theater script;
- monument.

Do not force collaboration into v1 if ownership/provenance becomes complex.

If enabled:

```csharp
CreatorIds: IReadOnlyList<string>
PrimaryCreatorId: string
```

Skill contribution uses deterministic aggregate formula.

---

## 126. Collaboration Quality Formula

Potential:

```text
effectiveCreativeInput =
  primarySkill
  + boundedSupportingContribution
  + relationshipCompatibility
  - coordinationPenalty
```

Do not simply sum all survivor skill values.

Large groups should not automatically produce perfect quality.

---

## 127. Relationship Effects of Collaboration

Successful collaboration can emit:
- shared accomplishment;
- mentorship;
- rivalry.

Failed/interrupted collaboration may emit mild tension.

SurvivorRelationsSystem owns actual bond changes.

---

## 128. Art Project Commission Policy

Player commissions should be explicit about:

- resource cost;
- expected time;
- creator opportunity cost;
- facility use;
- potential quality band.

No hidden mandatory cost.

Commissioning a specific theme can reduce autonomous expression; if design wants creator agency, some survivors
may prefer or refuse themes through autonomy/personality hooks.

Keep refusal rare and explainable.

---

## 129. Autonomous Creation Resource Policy

Autonomous creators should not drain scarce materials unexpectedly.

Possible global policy:

```text
Creative Projects:
[ Free materials only ]
[ Common materials allowed ]
[ Any non-reserved materials ]
```

Only implement if shelter policy infrastructure exists.

Otherwise autonomous creation should default to:
- no-cost storytelling;
- writing with negligible resources;
- projects using explicitly leisure-tagged supplies.

---

## 130. Cultural Material Tags

If inventory supports tags, add or reuse:

```text
art_material
writing_material
instrument
display_material
luxury_material
```

Do not add a separate art inventory.

Catalog validation checks real item tags.

---

## 131. Instrument Ownership

Music should distinguish:

- composition;
- practice;
- performance.

An instrument is:
- inventory item;
- facility capability;
- or shelter asset.

Do not duplicate instrument ownership inside art state.

A composition can survive loss of instrument; performance eligibility changes.

---

## 132. Theater Props and Costumes

Theater may optionally use:
- scrap cloth;
- decorative items;
- common props.

Do not require a full costume-production subsystem.

Use generic performance-material profile if needed.

---

## 133. Cultural Event Scheduler Collision

A concert may collide with:
- seasonal celebration;
- memorial;
- expedition departure;
- raid.

Use the same event calendar/scheduling utility where possible.

If event is linked to Plan 170:
- one event time;
- one participant reservation;
- one resource commit;
- two systems consume shared structural result.

---

## 134. Cross-System Effect Deduplication

The most dangerous integration bug is double morale.

Example:

```text
Founding Day celebration
+ concert hosted inside celebration
```

Wrong:
- Plan 170 applies full celebration morale;
- Plan 178 applies full concert morale independently.

Correct:
- event composer creates one combined effect budget;
- cultural performance contributes quality/activity modifiers;
- NeedsSystem receives one normalized effect transaction.

Use stable shared event ID.

---

## 135. Shared Cultural Event ID

Recommended:

```text
cultural_event:<campaignId>:<sequence>
```

Seasonal celebration stores reference:
```text
hostedCulturalEventIds
```

Art event stores:
```text
hostSeasonalOccurrenceId
```

No duplicate creation.

---

## 136. Audience Eligibility Reuse

Extract or reuse:

```csharp
ICommunalEventParticipationResolver
```

Inputs:
- event type;
- time;
- survivors;
- availability;
- relationship/hobby affinity.

Used by:
- Plan 170 celebration;
- concert;
- reading;
- exhibition;
- theater.

This prevents divergent attendance behavior.

---

## 137. Audience Scaling

For large shelters, individual attendance lists can be expensive and unnecessary.

Policy:

### Small shelter
Persist exact attendees.

### Large shelter
Persist:
- total eligible;
- attendee count;
- notable attendee IDs;
- performer IDs.

If achievements require exact all-survivor participation, resolver can provide boolean proof.

---

## 138. Cultural Event Quality

Event quality may derive from:

- artwork quality;
- performer skill;
- venue/facility;
- rehearsal/preparation;
- audience participation;
- contextual relevance.

Clamp 0..100.

Do not let an exceptional artwork guarantee an exceptional performance if performers are unprepared.

---

## 139. Rehearsal

Do not add a rehearsal subsystem in v1.

A theater/concert creation project can include preparation hours in its event profile.

This is enough for deterministic quality.

---

## 140. Cultural Event Memorable Criteria

Possible:

- quality >= threshold;
- attendance >= threshold;
- first event of type;
- tied to major shelter milestone;
- masterwork featured;
- creator/performer significance.

Persist `Memorable`.

Archive decides final retention.

---

## 141. Passive Art Morale Budget

Visual art must have a shelter-wide passive cap.

Example model:

```text
roomEffect =
  highestQualityWork * fullWeight
  + nextDistinctTheme * reducedWeight
  + diversityBonus
```

Do not sum every wall item.

This encourages curation.

---

## 142. Room-Specific Cultural Environment

If NeedsSystem supports room/environment effects:

- medical ward memorial art may comfort;
- common room mural improves communal atmosphere;
- sleeping quarters art has lower effect.

If no such spatial morale authority exists, keep one shelter-wide bounded effect.

Do not create room psychology inside ArtCreationSystem.

---

## 143. Gallery Capacity

Decor placement should enforce physical slots/capacity.

Art gallery UI may list stored works separately from displayed works.

A stored painting contributes:
- collection/cultural history;
- no passive display morale.

This makes placement meaningful.

---

## 144. Artwork Storage

Artwork not displayed/traded may be:
- stored;
- archived;
- inventory-linked.

Use canonical storage/inventory if physical objects require it.

Avoid duplicate "art storage slots" unless shelter systems need them.

---

## 145. Artwork Destruction

Possible causes:
- raid damage;
- fire;
- abandonment;
- deliberate disposal.

Use event:
`ArtworkDestroyed(artworkId, causeId)`

Then:
- remove display;
- update owner/status;
- preserve historical provenance for notable works;
- archive may record loss.

---

## 146. Artwork Restoration — Follow-On Hook

Damaged works may later be restored.

Do not implement restoration in v1 unless damage state exists.

Prepare:
```text
ArtworkCondition
```
only if decor/item condition systems already support it.

---

## 147. Art Trade Provenance After Transfer

When sold:
- artwork record remains in campaign cultural history;
- current owner becomes faction/settlement;
- shelter display removed;
- trade transaction recorded.

The shelter may still gain historical identity from having created it, but no passive display effect.

---

## 148. Reacquisition

If later reacquired:
- same artwork ID;
- ownership returns;
- display becomes possible again;
- no second "first creation" milestone.

This is important anti-duplication behavior.

---

## 149. Trade Standing Idempotency

Cultural diplomacy effect should fire once per unique transfer direction or transaction.

Prevent:
```text
sell -> buy back -> sell -> infinite standing
```

Possible:
- first external transfer gets cultural diplomacy bonus;
- later trades use ordinary economic value only.

Faction system decides exact rule.

---

## 150. Faction Cultural Preference Contract

If supported, faction data can expose:

```text
preferredArtForms
preferredThemes
dislikedThemes
culturalTradeMultiplier
```

These are faction/economy inputs.

ArtCreationSystem only tags artwork.

---

## 151. Resistance Art Consequences

The source says resistance art may affect faction standing.

This must be contextual.

A work with theme `resistance` should not globally increase standing.

It may:
- appeal to rebels;
- anger authoritarian faction;
- matter only when displayed/traded publicly.

Use faction preference/consequence rules.

---

## 152. Love Theme

Love-themed work may reference:
- romantic relation;
- family;
- friendship.

Only if canonical relationship fact exists.

Do not randomly assign a romantic dedication.

---

## 153. Loss / Death Themes

A memorial work should reference a real deceased survivor/event when possible.

If no memorial fact:
- use general loss theme;
- do not fabricate a named death.

This keeps narrative claims grounded.

---

## 154. Nature Theme

Nature art may be inspired by:
- discovered location;
- garden;
- memory/backstory.

Use stable fact IDs if available.

No need for gameplay bonus beyond normal cultural effects.

---

## 155. Hope Theme

Hope is not mechanically superior.

It can:
- fit founding/recovery events;
- influence description;
- contribute identity.

Keep morale differences between themes small or zero.

---

## 156. Cultural Value Accumulation

Instead of ticking cultural value daily, update on milestone events:

- created;
- first display;
- major exhibition;
- famous performance;
- historic survival;
- trade abroad;
- creator death;
- anniversary.

This is efficient and narratively meaningful.

---

## 157. Cultural Value Cap

Keep bounded, e.g. 0..1000 or project-appropriate.

Quality remains 0..100.

Do not let one ancient artwork overflow value through repeated annual increments.

---

## 158. Cultural Significance Bands

Possible:

- local;
- notable;
- cherished;
- iconic;
- historic.

These are presentation/identity bands.

Use data thresholds.

---

## 159. Art Collection Statistics

Derived diagnostics:

- works by form;
- masterworks;
- displayed works;
- traded works;
- destroyed works;
- notable creators;
- performances.

Do not make this a separate saved authority unless needed for performance.

---

## 160. Creator Legacy

A survivor's cultural legacy can derive from:

- number of notable works;
- masterworks;
- major performances;
- teaching.

ArtCreationSystem can export creator cultural facts.

Plan 145 owns final epilogue synthesis.

---

## 161. Art Education Integration

If Plan 154 education exists:

```text
Cultural Workshop
 -> education/apprenticeship practice
```

ArtCreationSystem records cultural event and provides subject.

Education system owns learning rate.

---

## 162. Master Artist Definition

Do not define "master artist" solely from number of works.

Use:
- Plan 161 hobby mastery;
- canonical skill threshold;
- or repeated high-quality works.

If no formal art skill exists, hobby mastery + quality history is enough.

---

## 163. Masterwork Frequency Calibration

Generate simulation for:

- novice creator;
- apprentice;
- journeyman;
- master.

Estimate masterwork probability/rate.

Goal:
- novice masterwork extremely rare;
- master not guaranteed every time;
- masterwork remains meaningful.

Use deterministic seed corpus.

---

## 164. Art Creation Distribution Audit

Generate 1,000 synthetic projects across templates.

Report:
- mean quality;
- masterwork rate;
- form frequency;
- material spend;
- time;
- trade value;
- cultural value.

Publish:
`docs/culture/ART_CREATION_DISTRIBUTION.md`
