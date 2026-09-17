# C2 — Flagship Integration Plan [45]: Survivor Photography, Documentation, Albums, Creative Records, and Historical Media

> **Deliverable:** `C2_planintegration[45].md`
> **Source scope:** Plan 219 — *Survivor Photography & Documentation System*
> **Primary objective:** create a deterministic survivor-authored documentation layer that lets survivors intentionally create photographs, sketches, written records, audio recordings, and video recordings of real shelter people, places, objects, events, and daily life; organize those records into albums and collections; share or preserve them; and route their historical, emotional, memorial, museum, journal, belonging, and time-capsule value through existing canonical systems without creating a second archive, journal, museum inventory, item ledger, memorial system, or generated-media truth source.
> **Required execution order:** **219A Foundation / Documentation Provenance Contract → 219B Photography, Sketching, Written/Audio/Video Records, Albums, Sharing & UI → 219C Cross-System Integration, Save/CI, Exploit Control, Long-Horizon Retention, and Historical-Narrative Closure**
> **Hard dependencies:** `JournalSystem`; `MemorialSystem`; canonical `Inventory`; Plan 210 `PersonalBelongingsSystem`; Plan 218 `ShelterMuseumSystem`; Plan 212 `TimeCapsuleSystem`; Plan 162 Shelter Archive; canonical survivor roster/identity; shelter location authority; semantic `EventSystem`; skill/trait/work-time providers; power/equipment state where digital recording requires it; morale/relations/psychology consequence sinks; Plan 31 semantic events; Plan 36 port contracts; Plan 39 save durability; Plan 55 retention; Plan 25 localization; Plan 37/184 accessibility.
> **Critical ownership correction:** `DocumentationSystem` owns **survivor-created documentation metadata, provenance, capture circumstances, quality assessments, sharing state, and album organization**. It does **not** own canonical shelter history, journal truth, memorial truth, museum display truth, item custody, or time-capsule custody.
> **Critical media correction:** a “photograph,” “sketch,” “audio recording,” or “video recording” is first a deterministic documentation record. A runtime-generated visual/audio asset is optional presentation. If no real media-generation pipeline exists, the record remains valid through authored descriptors, captions, transcripts, thumbnails, and asset references.
> **Critical subject correction:** documentation can only depict or describe canonical subjects that actually exist or events that actually occurred, except explicitly fictional/artistic work that is classified as art rather than documentary evidence.
> **Scope discipline:** no screenshot-as-world-truth shortcut, no invented historical event just because auto-document is enabled, no duplicate archive chronicle, no camera item cloned into documentation state, no “digital camera unlimited” without real power/storage constraint if such systems exist, no repeated morale farming from reopening/sharing the same record, no static `filmRemaining` field that duplicates canonical consumable inventory if film is modeled as items, no free quality rerolls via save/load, no album copies of photographs, and no requirement that the game synthesize image/audio/video binaries during headless simulation.

---

# 0. Executive Intent

ASHFALL already records many facts:

```text
events
statistics
journal entries
deaths
memorials
items
locations
relationships
museum exhibits
archive history
```

Those systems answer:

```text
What happened?
```

Plan 219 adds a different layer:

```text
Who deliberately chose to document it?
How did they document it?
What did they focus on?
How good was the record?
Who later saw it?
What did it come to mean?
```

The desired loop is:

```text
real shelter moment / subject
          │
          ▼
survivor chooses to document
          │
   ┌──────┼─────────────┬────────────┐
   ▼      ▼             ▼            ▼
 Photo  Sketch       Written      Audio/Video
   │      │             │            │
   └──────┼─────────────┴────────────┘
          ▼
 DocumentationRecord
          │
          ├─ author
          ├─ subject provenance
          ├─ equipment/materials
          ├─ location/time
          ├─ quality
          ├─ privacy/sharing
          └─ optional media asset reference
          │
          ▼
  album / collection / private record
          │
   ┌──────┼──────────────┬─────────────┐
   ▼      ▼              ▼             ▼
Journal  Memorial      Museum       Time Capsule
   │      │              │             │
   └──────┼──────────────┴─────────────┘
          ▼
      Shelter Archive
```

The strongest product outcome is:

> **A survivor can deliberately preserve a real moment in shelter history, and that documentation remains meaningfully tied to who created it, what actually happened, the equipment and conditions available, and the people who later view, inherit, display, archive, or rediscover it.**

---

# 1. Source Diagnosis

The source establishes:

- no dedicated photography/documentation system exists,
- `JournalSystem` already handles textual journal material,
- `MemorialSystem` already owns remembrance,
- `Inventory` owns camera/art equipment,
- five documentation types are required:
  - Photograph,
  - Sketch,
  - Written Record,
  - Audio Recording,
  - Video Recording,
- photographs require camera equipment,
- film cameras have limited exposures,
- digital cameras require power,
- photo quality depends on photographer skill, lighting, and subject cooperation,
- albums support themes:
  - family,
  - shelter,
  - events,
  - portraits,
  - landscape,
  - daily life,
  - historical,
- sketches have medium, artistic quality, accuracy, and time spent,
- written records support:
  - journal entry,
  - letter,
  - report,
  - chronicle,
  - poem,
  - essay,
- documentation may be private or shared,
- quality affects morale/historical value,
- 15+ documentation templates are required,
- UI requires:
  - documentation panel,
  - photo viewer,
  - album panel,
  - sketch gallery,
  - record library,
  - documentation log,
- integrations are required with:
  - Journal,
  - Memorial,
  - Inventory,
  - Personal Belongings,
  - Shelter Museum,
  - Time Capsules,
- old saves receive no documentation,
- deterministic creation and `--documentation-selftest` are required.

Several source fields need architectural normalization.

### 1.1 One giant `Documentation` DTO would become polymorphic bloat

Use:

```text
DocumentationRecord
+ type-specific detail records
```

rather than stuffing every photo/sketch/audio/video field into one object.

### 1.2 `filmRemaining` should not live on `Photograph`

Film remaining is state of:

- camera,
- film cartridge/roll,
- inventory consumable,

not a photo record.

### 1.3 `quality` and `sentimentalValue` are different concepts

Quality:

```text
technical / documentary / artistic execution
```

Sentimental significance:

```text
meaning to specific people
```

A blurry family photo can have low technical quality and very high personal significance.

### 1.4 `subjectId` needs typed refs

One string cannot safely represent:

- survivor,
- location,
- event,
- item,
- daily-life scene.

Use typed subject refs.

### 1.5 Auto-document cannot invent moments

If enabled, it can:

- propose documentation opportunities,
- autonomously document canonical events,

but must never fabricate an event simply to populate the archive.

---

# 2. Program-Level Success Criteria

C2[45] closes only when all of the following are true.

1. `DocumentationSystem.cs` exists.
2. It has versioned `CaptureState/RestoreState`.
3. Five source documentation types are supported.
4. Each documentation record has a stable ID.
5. Every record has a canonical author or explicit historical/anonymous provenance.
6. Every documentary subject resolves to a real canonical subject/event.
7. Fictional/artistic content is explicitly classified as non-documentary where applicable.
8. Photograph creation requires valid camera equipment.
9. Film photography consumes canonical film/exposure resources if film is modeled.
10. Digital photography checks actual equipment operability and power/storage constraints where modeled.
11. Sketch creation consumes time and art supplies where modeled.
12. Written records consume author time and integrate with Journal rather than duplicating journal truth.
13. Audio/video records require supported recording equipment.
14. Headless simulation can create metadata records without binary media.
15. Optional asset references do not become simulation authority.
16. Quality is deterministic from skill/equipment/conditions/time/subject cooperation.
17. Save/load cannot reroll quality.
18. Sentimental significance is contextual, not simply quality.
19. Albums store record IDs, not copies of photos.
20. Album membership is deterministic and deduplicated.
21. Shared/private visibility is explicit.
22. Sharing a record applies mechanical consequences at most once per defined audience/event.
23. Reopening the viewer does not reapply morale.
24. Documentation can be associated with memorials without duplicating memorial state.
25. Documentation can be displayed in museum without duplicating museum display state.
26. Documentation can enter time capsules through Plan 212 custody/reference rules.
27. Documentation can become personal belongings through Plan 210 without duplicating ownership.
28. Written records can link to Journal entries without copying canonical journal ownership.
29. Significant records can be promoted to Plan 162 Archive through structured refs.
30. Old saves load with no documentation.
31. No historical documentation is retroactively fabricated.
32. Auto-document mode produces records only from eligible real events/opportunities.
33. Auto-document mode respects survivor availability/equipment/material/time.
34. 15+ documentation templates validate.
35. All five documentation types have runtime-observed targeted fixtures.
36. All album themes are supported.
37. Extensive-documentation campaigns remain bounded through retention and UI grouping.
38. Same seed + same author/equipment/subject/conditions produces the same quality/outcome.
39. `--documentation-selftest` passes.
40. Full build/test/data-integrity verification remains green.

---

# 3. Architectural Invariants

## 3.1 DocumentationSystem owns authored records

It owns:

```text
documentation identity
author
capture time
subject refs
creation method
quality assessment
privacy/sharing state
album organization
presentation asset refs
```

## 3.2 Archive owns historical canon

Documentation may become evidence/artifact in archive.

Archive decides long-term historical significance.

## 3.3 Journal owns journal entries

Written documentation can link to journal entries.

Do not create duplicate journal chronology.

## 3.4 Inventory owns equipment and consumables

Camera, film, paper, ink, charcoal, recorder, batteries, storage media remain canonical items/resources.

## 3.5 PersonalBelongings owns personal ownership

A photograph can become a survivor’s personal belonging.

DocumentationSystem does not own that relationship.

## 3.6 Museum owns display

Museum decides whether documentation is currently exhibited.

Documentation stores no second display-slot truth.

## 3.7 TimeCapsule owns delayed custody

Documentation may be placed in capsule by reference/item custody.

Plan 212 owns seal/open timing.

## 3.8 Quality is immutable after creation unless restored/remastered explicitly

Viewing does not improve quality.

## 3.9 Subject truth is immutable provenance

A photograph of survivor X at location Y on day Z remains that provenance even if survivor later dies or location changes.

## 3.10 Generated media is presentation

If an image/thumbnail is generated or authored:

- it can render the record,
- it cannot override simulation metadata.

---

# 4. Documentation Data Architecture

Recommended hierarchy:

```text
DocumentationRecord
    │
    ├─ PhotographDetail
    ├─ SketchDetail
    ├─ WrittenRecordDetail
    ├─ AudioRecordingDetail
    └─ VideoRecordingDetail

PhotoAlbumRecord
DocumentationShareRecord
DocumentationViewRecord
DocumentationDiscoveryRecord
DocumentationHistorySummary
```

---

# 5. `DocumentationRecord`

Recommended fields:

```text
documentation_id
documentation_type
author_id
subject_refs[]
title_key_or_text
description_key_or_text
created_day
created_phase/time optional
location_id optional
quality_assessment
sentimental_context_refs
visibility
tags[]
template_id optional
asset_ref optional
journal_ref optional
archive_ref optional
memorial_ref optional
belonging_ref optional
```

---

# 6. Typed Subject References

Use:

```text
DocumentationSubjectRef
{
    subject_type
    subject_id
    role
}
```

Types:

```text
Survivor
Location
Event
Item
ShelterArea
Faction
DailyLifeContext
SignificantMoment
```

---

# 7. Subject Roles

Examples:

```text
Primary
Secondary
Background
Creator
Memorialized
DocumentedObject
```

Useful for group photos and event records.

---

# 8. Workstream 219A — Foundation / Provenance Contract

## Goal

Create one documentation authority that records survivor-authored media and text while delegating history, equipment, ownership, display, memorial, and delayed storage to existing systems.

---

# 9. 219A Phase A — Create `DocumentationSystem`

Path:

```text
Assets/Ashfall.Core/Culture/DocumentationSystem.cs
```

Responsibilities:

- evaluate documentation opportunities,
- validate authors/equipment/materials,
- create documentation records,
- calculate deterministic quality,
- bind canonical subjects,
- manage privacy/sharing state,
- organize albums/collections,
- track view/share events,
- coordinate integrations,
- capture/restore owned state.

---

# 10. 219A Phase B — Documentation Type Vocabulary

Preserve source:

```text
Photograph
Sketch
WrittenRecord
AudioRecording
VideoRecording
```

Stable serialized IDs.

---

# 11. 219A Phase C — Author Validation

Author must:

- exist,
- be alive,
- be available,
- have required capability/equipment,
- have sufficient time/resource.

Exceptions:

- imported historical records,
- anonymous found records,

must use explicit provenance type.

---

# 12. 219A Phase D — Provenance Types

Recommended:

```text
SurvivorAuthored
AnonymousShelterRecord
ImportedHistoricalRecord
RecoveredExternalRecord
```

Baseline player-created documentation should be `SurvivorAuthored`.

---

# 13. 219A Phase E — No Posthumous Creation

A dead survivor cannot create new documentation.

Records already created remain valid.

---

# 14. 219A Phase F — Creation Opportunity

Define:

```text
DocumentationOpportunity
```

Fields:

```text
opportunity_id
subject_refs
event_ref optional
location_ref
eligible_types
significance
available_day
expires_day optional
```

---

# 15. 219A Phase G — Opportunity Producers

Potential:

- semantic EventSystem,
- memorial event,
- expedition return,
- shelter milestone,
- family/relationship event,
- museum opening,
- leader transition,
- everyday-life scheduler.

---

# 16. 219A Phase H — Daily Life Opportunities

Do not require semantic “major event.”

Can arise from:

- meal,
- work shift,
- recreation,
- child learning,
- repair,
- winter routine.

Must still be a real canonical context.

---

# 17. 219A Phase I — Significant Moment

Significance can derive from:

- semantic event severity,
- rare transition,
- death/memorial,
- major survival milestone,
- personal relationship milestone.

---

# 18. 219A Phase J — Auto-Document Setting

Source asks:

```text
auto-document bool
```

Interpret as:

```text
eligible survivor may automatically create documentation from selected opportunities
```

not:

```text
system fabricates records every day.
```

---

# 19. 219A Phase K — Auto-Document Eligibility

Requires:

- available author,
- required equipment,
- materials,
- time,
- opportunity,
- frequency budget.

---

# 20. 219A Phase L — Documentation Enabled

If false:

- no new automatic documentation,
- existing documentation remains readable.

Manual creation may be disabled depending setting semantics.

Prefer:

```text
documentation_creation_enabled
```

---

# 21. 219A Phase M — Quality Modifier Setting

Source proposes `quality modifier`.

Do not expose a cheat-like global scalar without purpose.

If used for difficulty/accessibility/testing:

- call it tuning/config,
- not player-facing ordinary setting.

---

# 22. 219A Phase N — Deterministic Quality

Quality inputs:

```text
author skill
equipment condition/capability
materials
lighting/environment
subject cooperation
time spent
difficulty
```

No free RNG reroll.

---

# 23. 219A Phase O — Quality Components

Prefer multi-component assessment:

```text
TechnicalQuality
DocumentaryAccuracy
ArtisticQuality
Clarity
```

then derive overall quality per type.

---

# 24. 219A Phase P — Photograph Quality

Suggested:

```text
technical
composition
lighting
subject cooperation
camera capability
photographer skill
```

---

# 25. 219A Phase Q — Sketch Quality

Suggested:

```text
artistic quality
accuracy
medium/tool quality
time spent
author skill
```

---

# 26. 219A Phase R — Written Quality

Suggested:

```text
clarity
writing skill
time spent
record type
author condition
```

Do not algorithmically judge user-entered prose quality.

If player writes free text:

- use authored metadata/author skill only,
- never score semantic writing quality automatically.

---

# 27. 219A Phase S — Audio Quality

Suggested:

```text
recording equipment
power/battery
noise conditions
microphone condition
operator skill
```

---

# 28. 219A Phase T — Video Quality

Suggested:

```text
camera capability
power/storage
lighting
stability
operator skill
duration
```

---

# 29. 219A Phase U — Sentimental Significance

Do not persist one universal `sentimentalValue` as absolute truth.

Prefer:

```text
DocumentationSentimentProfile
```

with contextual tags:

```text
family
memorial
friendship
childhood
survival milestone
romance
mentor
loss
home
```

Reader-specific significance derives later.

---

# 30. 219A Phase V — Historical Significance

Separate from sentiment.

Derived from:

- event significance,
- rarity,
- age,
- subject importance,
- provenance,
- archive/museum context.

Plan 162 may own final archive significance.

---

# 31. 219A Phase W — Privacy / Visibility

Use:

```text
Private
Family
Shelter
Public
Restricted
```

Source only requires public/private, but `intendedAudience` already implies richer scope.

---

# 32. 219A Phase X — Visibility Authority

DocumentationSystem owns who may view the documentation.

Museum/archive may impose additional access, but cannot silently override private status without explicit event/policy.

---

# 33. 219A Phase Y — Share Record

Create:

```text
DocumentationShareRecord
```

Fields:

```text
share_id
documentation_id
shared_by
audience
day
context
consequence_application_ids
```

---

# 34. 219A Phase Z — Share Idempotency

Sharing the same record to same audience repeatedly does not repeatedly grant morale.

New audience/context may be meaningful.

---

# 35. 219A Phase AA — View Tracking

Only persist views if needed for:

- first-view reactions,
- achievements,
- privacy.

Do not store every UI open.

---

# 36. 219A Phase AB — First Meaningful View

Use:

```text
DocumentationViewed
```

only when in-world survivor/audience actually sees it.

Opening UI as player is not automatically an in-world view event.

---

# 37. 219A Phase AC — Equipment Contract

Define:

```text
IDocumentationEquipmentPort
```

Queries:

```text
HasUsableCamera
GetCameraCapabilities
ConsumeFilmExposure
ConsumeArtSupplies
HasRecordingDevice
ConsumeBattery/Power
ReserveStorageMedia
```

---

# 38. 219A Phase AD — Camera Identity

Store canonical item instance/reference:

```text
camera_item_instance_id
```

if item instances matter.

Do not store only definition ID if equipment condition matters.

---

# 39. 219A Phase AE — Film Camera

Film/exposure belongs to canonical inventory/equipment.

Recommended:

```text
camera
+ loaded film item/cartridge
→ ConsumeExposure()
```

No `filmRemaining` on each photograph.

---

# 40. 219A Phase AF — Digital Camera

Source says unlimited but requires power.

Correct:

- no film exposure,
- but device must have:
  - charge/power,
  - storage if modeled,
  - working condition.

“Unlimited” means not film-limited, not resource-free.

---

# 41. 219A Phase AG — Art Supplies

Sketching may consume:

- paper,
- pencil durability,
- charcoal,
- ink,
- watercolor.

Use existing item/crafting/resource model.

---

# 42. 219A Phase AH — Writing Materials

If paper/ink modeled:

- written record consumes them.

If not:

- author time is sufficient baseline.

Do not introduce resource micro-management solely for this plan unless valuable.

---

# 43. 219A Phase AI — Recording Equipment

Audio/video require actual device definition.

If unavailable in catalog:

- templates requiring them fail integrity or remain disabled until assets/items are added.

---

# 44. 219A Phase AJ — Power Requirement

Use canonical device/power system.

Digital camera/recorder can:

- use battery item,
- charge from shelter power,
- have self-contained power.

No local power bar.

---

# 45. 219A Phase AK — Work-Time Contract

Documentation creation consumes:

```text
survivor time
```

through duty/activity system where available.

No invisible free production.

---

# 46. 219A Phase AL — Subject Cooperation

For posed photograph:

- subject consent/availability.

For candid/documentary:

- cooperation may not be required,
- privacy/social consequences may exist.

---

# 47. 219A Phase AM — Consent and Privacy

Do not assume every survivor accepts being photographed.

Potential inputs:

- relationship,
- traits,
- privacy,
- current condition.

Baseline can support:

```text
Cooperative
Neutral
Refuses
```

---

# 48. 219A Phase AN — Candid Photography

If subject does not know:

- documentary record can still exist,
- sharing may cause relation/privacy consequences.

Only if social systems support.

---

# 49. 219A Phase AO — Location Conditions

Use canonical shelter/location state:

- light,
- hazard,
- weather exposure,
- room type.

Do not duplicate environment state.

---

# 50. 219A Phase AP — Media Asset References

Recommended:

```text
DocumentationAssetRef
{
    asset_kind
    asset_id/path
    source
    generation_version optional
}
```

Presentation-only.

---

# 51. 219A Phase AQ — Headless Fallback

Headless record must render with:

- title,
- description,
- subject list,
- quality,
- metadata,
- optional placeholder thumbnail.

No asset generation needed.

---

# 52. 219A Phase AR — Asset Missing

Fallback gracefully.

Do not invalidate historical record because image file missing.

---

# 53. 219A Phase AS — State Persistence

Persist:

- documentation records,
- type-specific details,
- albums,
- sharing state,
- relevant first-view/discovery events,
- asset refs,
- idempotency keys,
- schema version.

Do not persist:

- duplicate items,
- duplicate journal text if linked,
- museum placement,
- archive history copies.

---

# 54. 219A Phase AT — Old Save Compatibility

Source exact requirement:

```text
old saves get no documentation
```

Also:

- no albums,
- no documentation history,
- no retroactive photo generation.

---

# 55. 219A Phase AU — No Historical Backfill

Do not fabricate photos of:

- earlier deaths,
- old raids,
- old shelter milestones.

Future archive import can be separate.

---

# 56. 219A Phase AV — Composition Root

Source asks:

```text
SetupDocumentation
TickDocumentation
SaveDocumentation
```

Follow Plan 28 composition architecture.

---

# 57. 219A Phase AW — Tick Model

Avoid daily scan of all survivors × all subjects.

Use:

- documentation opportunities,
- explicit player actions,
- scheduled project completion,
- auto-document opportunity queue.

---

# 58. 219A Phase AX — RNG Stream

Use:

```text
documentation
```

Stable key:

```text
documentation_id
author_id
subject_digest
creation_day
quality_component
```

---

# 59. 219A Phase AY — Semantic Events

Candidate kinds:

```text
documentation_created
photograph_taken
sketch_created
written_record_created
audio_recording_created
video_recording_created
photo_album_created
documentation_shared
documentation_first_viewed
documentation_archived
documentation_displayed
```

---

# 60. 219A Phase AZ — Port Contract

Mandatory:

- survivor roster,
- location/event subject resolver,
- equipment/inventory,
- save.

Conditional:

- journal,
- memorial,
- belongings,
- museum,
- time capsule,
- archive,
- morale,
- relationships,
- power/activity system.

---

# 61. 219A Phase BA — Diagnostics

Expose:

```text
DOCUMENTATION_TOTAL
DOCUMENTATION_PHOTOS
DOCUMENTATION_SKETCHES
DOCUMENTATION_WRITTEN
DOCUMENTATION_AUDIO
DOCUMENTATION_VIDEO
DOCUMENTATION_ALBUMS
DOCUMENTATION_SHARED
DOCUMENTATION_MISSING_ASSETS
DOCUMENTATION_REQUIRED_PORTS_MISSING
```

---

# 62. 219A Tests

- five types,
- author validation,
- subject validation,
- camera requirement,
- film consumption,
- digital power requirement,
- sketch materials,
- deterministic quality,
- quality vs sentiment separation,
- privacy/share state,
- old-save empty state,
- no historical backfill.

---

# 63. 219A Definition of Done

- [ ] `DocumentationSystem.cs`,
- [ ] five documentation types,
- [ ] typed subject refs,
- [ ] provenance,
- [ ] documentation opportunities,
- [ ] deterministic quality,
- [ ] contextual sentiment,
- [ ] privacy/sharing,
- [ ] equipment/material contract,
- [ ] work-time contract,
- [ ] consent/cooperation,
- [ ] optional asset refs,
- [ ] headless fallback,
- [ ] save/versioning,
- [ ] old-save no-backfill,
- [ ] composition/events/ports/diagnostics.

---

# 64. Workstream 219B — Photography

## Goal

Implement photography as a real survivor action requiring real equipment and canonical subjects.

---

# 65. 219B Phase A — `PhotographDetail`

Fields:

```text
photo_id
documentation_id
camera_item_instance_id
subject_survivor_ids[]
location_id
moment_type
composition_score
lighting_score
technical_score
capture_context
```

---

# 66. 219B Phase B — Moment Types

Preserve:

```text
Candid
Posed
Documentary
Artistic
```

---

# 67. 219B Phase C — Photograph Creation Flow

```text
select author
→ select canonical subject/opportunity
→ validate camera
→ validate film/power
→ validate location/availability
→ determine cooperation
→ reserve activity time
→ consume exposure/power
→ resolve quality
→ create record
→ emit event
```

---

# 68. 219B Phase D — Candid Photo

Benefits:

- lower cooperation requirement,
- stronger spontaneity.

Risks:

- privacy objection,
- lower composition.

---

# 69. 219B Phase E — Posed Photo

Requires:

- subject availability,
- cooperation.

Potential:

- better composition/clarity,
- higher time cost.

---

# 70. 219B Phase F — Documentary Photo

Prioritizes:

- accuracy,
- event/location context.

Historical value can be high even if artistic quality modest.

---

# 71. 219B Phase G — Artistic Photo

Prioritizes:

- composition,
- lighting.

Plan 178 Art & Culture may consume/display.

Documentation still records real subject.

---

# 72. 219B Phase H — Group Photos

Multiple survivor subject refs.

All must exist at capture time.

---

# 73. 219B Phase I — Subject Death After Photo

Record remains.

Can gain memorial/sentimental significance later.

Do not rewrite photo quality.

---

# 74. 219B Phase J — Location Change After Photo

Record preserves original location snapshot/reference.

Archive may note that place later changed/destroyed.

---

# 75. 219B Phase K — Photo Metadata Snapshot

Persist only facts needed to preserve historical capture:

```text
day
location
subject IDs
event ref
camera ref
```

Do not copy entire survivor state.

---

# 76. 219B Phase L — Film Exposure Atomicity

On photo commit:

- exactly one exposure consumed per configured shot.

Save/load cannot restore exposure while retaining photo.

---

# 77. 219B Phase M — Burst/Multiple Photos

Baseline one action = one photo.

Future burst mode may consume multiple exposures.

Avoid content spam.

---

# 78. 219B Phase N — Digital Storage

If storage media system exists:

- consume capacity.

If not:

- omit storage simulation rather than invent pseudo-bytes.

---

# 79. 219B Phase O — Lighting

Use available environment/light provider.

If none:

- location/time/weather profile approximation.

Deterministic.

---

# 80. 219B Phase P — Camera Condition

Equipment condition reduces technical quality if canonical condition exists.

---

# 81. 219B Phase Q — Photographer Skill

Audit existing skills.

If no photography skill:

- use art/perception/documentation-related canonical skill,
- or add capability profile only if justified.

Do not create isolated skill casually.

---

# 82. Workstream 219B — Sketching

## Goal

Support low-tech visual documentation that remains viable during power/film scarcity.

---

# 83. 219B Phase R — `SketchDetail`

Fields:

```text
sketch_id
documentation_id
medium
artistic_quality
accuracy
time_spent
material_refs
```

---

# 84. 219B Phase S — Medium Vocabulary

Preserve:

```text
Pencil
Charcoal
Ink
Watercolor
Digital
```

---

# 85. 219B Phase T — Digital Sketch

Requires supported digital device/power.

If no tablet/computer workflow exists:

- template disabled.

---

# 86. 219B Phase U — Accuracy

Important for documentary sketch.

Inputs:

- observation time,
- subject visibility,
- author skill,
- medium.

---

# 87. 219B Phase V — Artistic Quality

Separate from accuracy.

A stylized sketch can be high art / low documentary accuracy.

---

# 88. 219B Phase W — Time Spent

Source asks hours.

Use canonical activity duration.

More time can improve quality with diminishing returns.

---

# 89. 219B Phase X — Interrupted Sketch

If author becomes unavailable:

- project can remain incomplete,
- or produce lower-quality partial record.

Do not auto-complete invisibly.

---

# 90. 219B Phase Y — Sketch Project State

For long pieces:

```text
Draft
InProgress
Completed
Abandoned
```

Only if work-time system supports multi-step projects.

Baseline can use atomic creation for short sketches.

---

# 91. Workstream 219B — Written Records

## Goal

Create survivor-authored documentation while integrating rather than competing with `JournalSystem`.

---

# 92. 219B Phase Z — Record Types

Preserve:

```text
JournalEntry
Letter
Report
Chronicle
Poem
Essay
```

---

# 93. 219B Phase AA — Journal Entry

Preferred architecture:

```text
JournalSystem creates canonical journal entry
→ DocumentationSystem links it as documented record
```

not a copied text blob.

---

# 94. 219B Phase AB — Letter

If legacy/delayed delivery:

- Plan 212 TimeCapsule/LegacyMessage may own delivery.
- DocumentationSystem owns authored document record.

---

# 95. 219B Phase AC — Report

Can document:

- expedition,
- resource crisis,
- faction encounter,
- scientific observation.

Must bind canonical event/data refs.

---

# 96. 219B Phase AD — Chronicle

Long-form historical synthesis.

Can reference multiple archive/event IDs.

No new world facts.

---

# 97. 219B Phase AE — Poem

Creative expression.

Mark:

```text
documentary_accuracy = not applicable
```

Plan 178 may also consume as art.

---

# 98. 219B Phase AF — Essay

Can be:

- reflection,
- doctrine,
- analysis.

Does not automatically become factual archive evidence.

---

# 99. 219B Phase AG — Content Storage

System-authored:

```text
localization/template key + parameters
```

Player-authored:

```text
raw text
```

if feature permits.

---

# 100. 219B Phase AH — Word Count

Derived from stored text.

Do not persist unless needed for performance.

---

# 101. 219B Phase AI — Writing Quality

If system-authored:

- derive from skill/time/condition.

If player-authored:

- do not judge wording semantically.

Use author capability metadata only.

---

# 102. 219B Phase AJ — Intended Audience

Preserve:

```text
Private
Family
Shelter
Public
```

Can map to visibility.

---

# 103. Workstream 219B — Audio Recording

## Goal

Support documentary audio with transcript-first fallback.

---

# 104. 219B Phase AK — `AudioRecordingDetail`

Fields:

```text
recording_id
documentation_id
device_item_instance_id
duration
audio_asset_ref optional
transcript_ref
noise_quality
clarity
```

---

# 105. 219B Phase AL — Recording Subjects

Can include:

- survivor speech,
- ambient shelter sound,
- event report,
- oral history,
- music/performance.

---

# 106. 219B Phase AM — Transcript

Required for accessibility and headless archival value.

---

# 107. 219B Phase AN — Audio Asset Optional

If ElevenLabs or generated audio pipeline later produces asset:

- attach asset ref.

Simulation validity never depends on it.

---

# 108. 219B Phase AO — Consent

Recorded speech may require subject consent depending context.

---

# 109. Workstream 219B — Video Recording

## Goal

Support rare, resource-expensive moving-image documentation without demanding a runtime video-generation pipeline.

---

# 110. 219B Phase AP — `VideoRecordingDetail`

Fields:

```text
video_id
documentation_id
device_item_instance_id
duration
video_asset_ref optional
transcript_ref
thumbnail_asset_ref optional
stability
lighting
audio_clarity
```

---

# 111. 219B Phase AQ — Video Rarity

Video should likely be:

- later-tech,
- power/storage expensive,
- rare.

This preserves photography/sketching niches.

---

# 112. 219B Phase AR — Video Headless Fallback

Display:

- thumbnail/placeholder,
- textual scene description,
- transcript,
- metadata.

---

# 113. Workstream 219B — Photo Albums

## Goal

Organize records without duplicating them.

---

# 114. 219B Phase AS — `PhotoAlbumRecord`

Fields:

```text
album_id
album_name
owner_id
photo_ids[]
theme
created_day
last_updated_day
visibility
cover_photo_id optional
page_capacity optional
```

---

# 115. 219B Phase AT — Album Themes

Preserve source:

```text
Family
Shelter
Events
Portraits
Landscape
DailyLife
Historical
```

---

# 116. 219B Phase AU — Album Membership

Store IDs.

No copied photograph objects.

---

# 117. 219B Phase AV — Duplicate Photo in Album

Choose policy.

Recommended:

- same photo once per album.

A photo can appear in multiple albums.

---

# 118. 219B Phase AW — Page Count

If physical album item exists:

- page count/capacity belongs to album record/item capability.

If purely digital UI collection:

- page count derived from photos/layout.

Do not invent arbitrary paper pages if not physical.

---

# 119. 219B Phase AX — Physical Album

If album is a physical object:

- Inventory/Belongings owns the album item,
- DocumentationSystem owns membership/order.

---

# 120. 219B Phase AY — Album Owner

Can be:

- survivor,
- shelter archive,
- family/group.

Source only says survivor owner; support shelter ownership if needed for archive albums.

---

# 121. 219B Phase AZ — Shared Album

Visibility state.

Sharing does not clone photos.

---

# 122. 219B Phase BA — Album Ordering

Persist stable photo ID order.

---

# 123. 219B Phase BB — Album Completion

“The Collection” should require authored goal/capacity/theme completion.

Not simply any album with one photo.

---

# 124. Workstream 219B — Documentation Templates

## Goal

Provide 15+ authored opportunities spanning all media and subjects.

---

# 125. 219B Phase BC — Data File

Create:

```text
Assets/StreamingAssets/Data/documentation_templates.json
```

Sections:

```text
documentation_templates
quality_profiles
album_templates
sharing_profiles
reaction_profiles
equipment_profiles
```

---

# 126. 219B Phase BD — Recommended Initial Count

Source requires 15+.

Target:

```text
20 templates
```

for broad coverage.

---

# 127. 219B Phase BE — Photo Template 1: Shelter Portrait

Subject:

- one survivor.

Moment:

- posed.

---

# 128. 219B Phase BF — Photo Template 2: Work Crew

Subjects:

- active duty group.

Moment:

- documentary/posed.

---

# 129. 219B Phase BG — Photo Template 3: After the Raid

Subject:

- real aftermath event/location.

High historical value.

---

# 130. 219B Phase BH — Photo Template 4: Family Photograph

Requires canonical family relation.

---

# 131. 219B Phase BI — Photo Template 5: Ordinary Evening

Daily-life opportunity.

Sentimental potential.

---

# 132. 219B Phase BJ — Sketch Template 1: Shelter Map Study

Documentary sketch of real area.

Not canonical map truth unless accuracy threshold and map system consumes it.

---

# 133. 219B Phase BK — Sketch Template 2: Portrait from Memory

If subject absent/dead:

- artistic/memory-based,
- lower documentary accuracy,
- valid as memorial art.

---

# 134. 219B Phase BL — Sketch Template 3: Machine Diagram

Subject:

- real equipment.

Can integrate technical journal.

---

# 135. 219B Phase BM — Written Template 1: Daily Chronicle

Records current day highlights.

Uses actual semantic events.

---

# 136. 219B Phase BN — Written Template 2: Expedition Report

Binds Expedition result.

---

# 137. 219B Phase BO — Written Template 3: Crisis Record

Binds disaster/rationing/medical crisis.

---

# 138. 219B Phase BP — Written Template 4: Personal Reflection

Private survivor-authored record.

---

# 139. 219B Phase BQ — Written Template 5: Letter Home

Requires canonical recipient/context if delivered.

Otherwise private record.

---

# 140. 219B Phase BR — Audio Template 1: Oral History

Survivor recounts real canonical history refs.

---

# 141. 219B Phase BS — Audio Template 2: Shelter Soundscape

Ambient documentation.

No factual claims.

---

# 142. 219B Phase BT — Audio Template 3: Final Testimony

Prepared while alive.

Can later integrate Plan 212 on death.

---

# 143. 219B Phase BU — Video Template 1: Shelter Tour

Requires valid accessible areas.

---

# 144. 219B Phase BV — Video Template 2: Major Ceremony

Binds real event.

---

# 145. 219B Phase BW — Video Template 3: Training Demonstration

Binds actual skill/mentor context.

---

# 146. 219B Phase BX — Collection Template: Year in the Shelter

Album/collection across time.

Requires multiple real photos.

---

# 147. 219B Phase BY — No Fake Context

Templates become ineligible when required canonical relations/events/locations/equipment are absent.

---

# 148. Workstream 219B — Quality System

## Goal

Make quality meaningful but not overpower sentimental/historical value.

---

# 149. 219B Phase BZ — Quality Bands

Example:

```text
Poor
Rough
Competent
Good
Excellent
Masterwork
```

Thresholds data-driven.

---

# 150. 219B Phase CA — Overall Quality

Derived per media type.

No one formula for everything.

---

# 151. 219B Phase CB — Historical Value

A poor-quality unique photo of major event can still be historically priceless.

Keep distinct.

---

# 152. 219B Phase CC — Sentimental Value

Reader-specific.

A technically perfect landscape may mean less than a blurry photo of a dead friend.

---

# 153. 219B Phase CD — Quality Consequences

Possible:

- stronger museum interest,
- clearer documentary evidence,
- better sharing morale,
- archive significance.

Bounded.

---

# 154. 219B Phase CE — No Quality Grinding

Same opportunity cannot be repeatedly photographed indefinitely for better roll without cost/time/equipment and novelty rules.

---

# 155. Workstream 219B — Sharing

## Goal

Make documentation socially meaningful without turning “share” into a morale button.

---

# 156. 219B Phase CF — Share Audience

```text
SpecificSurvivor
Family
WorkGroup
Shelter
Public
MuseumAudience
```

---

# 157. 219B Phase CG — First Share

Mechanical reaction generally applies on first meaningful exposure per audience.

---

# 158. 219B Phase CH — Repeat Viewing

No repeated morale farm.

Periodic memorial viewing belongs to Memorial/psychology if needed.

---

# 159. 219B Phase CI — Privacy Violation

If private content shared without author consent:

- possible relations/morale conflict.

Only if system supports consent state.

---

# 160. 219B Phase CJ — Shelter Sharing

Shared documentation may produce:

- morale,
- connection,
- historical awareness.

Effect depends on subject/context.

---

# 161. 219B Phase CK — Negative Documentation

A photo/report of disaster can:

- lower morale,
- raise awareness,
- strengthen remembrance.

Do not assume all sharing boosts morale.

---

# 162. 219B Phase CL — Propaganda Boundary

If documentation is deliberately used to persuade factions/shelter politically:

- Plan 168 propaganda owns persuasive effect.

Documentation provides media artifact.

---

# 163. Workstream 219B — Documentation UI

## Goal

Make survivor-created media browseable and meaningful without confusing metadata records with canonical archive state.

---

# 164. 219B Phase CM — Main Documentation Panel

Tabs:

```text
All
Photographs
Sketches
Written
Audio
Video
Albums
Shared
Private
Historical
```

---

# 165. 219B Phase CN — Filters

Filter by:

- author,
- subject,
- date,
- location,
- type,
- quality,
- visibility,
- album,
- memorial/archive/museum association.

---

# 166. 219B Phase CO — Photo Viewer

Show:

- visual asset if present,
- fallback illustration/thumbnail if not,
- title,
- author,
- day,
- location,
- subjects,
- quality,
- historical/sentimental context.

---

# 167. 219B Phase CP — No Fake Pixel Proof

If photo asset is generated stylistically and may not exactly encode scene:

- metadata remains truth,
- caption is authoritative.

Do not let player infer unsupported item counts from generated image.

---

# 168. 219B Phase CQ — Sketch Gallery

Show:

- sketch visual if present,
- subject,
- medium,
- artistic quality,
- accuracy,
- creator.

---

# 169. 219B Phase CR — Record Library

Show:

- type,
- author,
- audience,
- content,
- linked journal/archive refs.

---

# 170. 219B Phase CS — Audio Viewer

Show:

- play control if asset,
- transcript always,
- metadata.

---

# 171. 219B Phase CT — Video Viewer

Show:

- play control if asset,
- transcript/scene summary,
- thumbnail.

---

# 172. 219B Phase CU — Album Panel

Create/edit:

- name,
- theme,
- order,
- cover,
- photos,
- visibility.

---

# 173. 219B Phase CV — Documentation Log

Log meaningful:

- created,
- shared,
- discovered,
- archived,
- displayed,
- album completed.

Not every UI view.

---

# 174. 219B Phase CW — Notification Policy

Notify:

- significant documentation created,
- album completed,
- important old documentation found,
- museum/archive acceptance.

Do not notify every routine photo.

---

# 175. 219B Phase CX — Tutorial

First photograph explains:

- equipment,
- subject,
- quality,
- privacy,
- albums,
- historical value.

---

# 176. 219B Phase CY — Tooltips

Hover + focus/details.

---

# 177. 219B Phase CZ — Large Text

2× font.

Captions/records reflow.

---

# 178. 219B Phase DA — Screen Reader

Every visual record needs text alternative:

- subject description,
- author,
- date,
- location,
- key content.

---

# 179. 219B Phase DB — Audio Accessibility

Audio records have transcript.

---

# 180. 219B Phase DC — Video Accessibility

Video records have captions/transcript/audio description as appropriate.

---

# 181. 219B Phase DD — Cognitive Load Mode

Compact card:

```text
Title
Type
Author
Day
Subject
Why it matters
```

---

# 182. Workstream 219B — Source Events

Preserve:

```text
The Photo
The Sketch
The Record
The Album
The Sharing
The Discovery
The Collection
The Masterpiece
```

---

# 183. 219B Phase DE — The Photo

Meaningful photograph created.

---

# 184. 219B Phase DF — The Sketch

Sketch completed.

---

# 185. 219B Phase DG — The Record

Written record completed.

---

# 186. 219B Phase DH — The Album

Album created.

---

# 187. 219B Phase DI — The Sharing

Meaningful share.

---

# 188. 219B Phase DJ — The Discovery

Old/recovered documentation discovered.

Do not emit on ordinary UI filter.

---

# 189. 219B Phase DK — The Collection

Album/collection completed according to explicit criteria.

---

# 190. 219B Phase DL — The Masterpiece

High-quality documentation created.

Threshold per media type.

---

# 191. Workstream 219B — Quest / Achievement Hooks

## Goal

Preserve source hooks without incentivizing spam.

---

# 192. 219B Phase DM — Ownership

Plan 149/shared quest runtime owns actual hook state.

Documentation emits semantic facts.

---

# 193. 219B Phase DN — The Photographer

Source:

```text
50 photographs
```

Count valid committed photographs.

Consider novelty requirements to discourage 50 identical shots of same subject.

---

# 194. 219B Phase DO — The Artist

30 completed sketches.

---

# 195. 219B Phase DP — The Writer

20 completed written records.

---

# 196. 219B Phase DQ — The Archivist

10 distinct meaningful albums.

Avoid empty album farming.

---

# 197. 219B Phase DR — The Documentarian

Document all shelter areas.

Use canonical area IDs.

One qualifying record per area.

---

# 198. 219B Phase DS — The Historian

Document 50 significant events.

Requires event significance and event-document binding.

---

# 199. 219B Phase DT — The Sharer

Share 20 distinct documentation pieces to meaningful audiences.

No repeated same-record share farming.

---

# 200. 219B Definition of Done

- [ ] photography,
- [ ] film/digital equipment rules,
- [ ] group/posed/candid/documentary/artistic modes,
- [ ] sketching,
- [ ] five sketch media,
- [ ] written records,
- [ ] six written types,
- [ ] audio,
- [ ] video,
- [ ] album themes,
- [ ] quality bands,
- [ ] contextual sentiment,
- [ ] sharing/privacy,
- [ ] 15+ templates,
- [ ] recommended 20 templates,
- [ ] UI viewers/galleries/library/albums/log,
- [ ] source events/hooks,
- [ ] tutorial/tooltips,
- [ ] accessibility.

---

# 201. Workstream 219C — JournalSystem Integration

## Goal

Make written documentation and journal records cross-reference each other without duplicate text/history authority.

---

# 202. 219C Phase A — Journal Link

Define:

```text
IDocumentationJournalPort
```

Operations:

```text
CreateLinkedJournalEntry(...)
GetJournalEntry(...)
AttachDocumentationRef(...)
```

---

# 203. 219C Phase B — Journal Entry Documentation

If author creates journal entry as documentation:

- Journal owns entry,
- Documentation stores `journal_entry_id`.

---

# 204. 219C Phase C — Chronicle / Report

Can reference multiple journal/event IDs.

---

# 205. 219C Phase D — No Copy Drift

Avoid:

```text
journal text
+
documentation copied text
```

unless immutable snapshot is intentionally required.

---

# 206. Workstream 219C — MemorialSystem Integration

## Goal

Use documentation as remembrance material without creating a second memorial engine.

---

# 207. 219C Phase E — Memorial Documentation

Possible:

- portrait of deceased,
- memorial sketch,
- oral history,
- final letter,
- album.

---

# 208. 219C Phase F — Association

Store:

```text
memorial_ref
```

Documentation remains record.

Memorial remains remembrance owner.

---

# 209. 219C Phase G — Post-Death Significance

When documented survivor dies:

- sentimental/historical read model can update,
- record is not recreated.

---

# 210. 219C Phase H — Memorial Display

If MemorialSystem supports displayed objects:

- use ref.

No duplicate display state.

---

# 211. Workstream 219C — Inventory Integration

## Goal

Use real equipment/materials and prevent free media creation.

---

# 212. 219C Phase I — Camera

Canonical item instance.

---

# 213. 219C Phase J — Film

Canonical consumable if catalog supports.

---

# 214. 219C Phase K — Art Supplies

Canonical item/charge/durability.

---

# 215. 219C Phase L — Recording Device

Canonical item.

---

# 216. 219C Phase M — Battery / Power

Canonical energy source.

---

# 217. 219C Phase N — Item Condition

Equipment condition affects capture quality/availability.

---

# 218. 219C Phase O — Equipment Loss

Existing documentation survives after camera destroyed.

Future creation blocked.

---

# 219. Workstream 219C — PersonalBelongings Integration

## Goal

Let documentation become meaningful possessions without duplicating ownership.

---

# 220. 219C Phase P — Plan 210 Binding

A photo/album/sketch/document can become:

```text
personal belonging
```

through canonical belonging record.

---

# 221. 219C Phase Q — Owner vs Author

Important distinction:

```text
author ≠ current owner
```

A child can inherit a parent’s photo.

Documentation keeps author.

Belongings keeps owner.

---

# 222. 219C Phase R — Provenance

Ownership transfers do not rewrite creator/subject history.

---

# 223. 219C Phase S — Physical vs Digital

If documentation is physical item:

- Inventory/Belongings owns instance.

If digital-only record:

- DocumentationSystem owns access metadata,
- no fake physical item.

---

# 224. Workstream 219C — Shelter Museum Integration

## Goal

Display documentation as exhibits while preserving one museum display authority.

---

# 225. 219C Phase T — Plan 218 Display Eligibility

Museum may query:

- type,
- quality,
- historical significance,
- subject,
- privacy,
- owner permission.

---

# 226. 219C Phase U — Exhibit Ref

Museum stores:

```text
documentation_id
```

as exhibit source.

Documentation does not store active display slot as authority.

---

# 227. 219C Phase V — Museum Sharing Effect

Museum audience effect belongs to Museum/Culture.

Documentation only provides artifact and quality metadata.

---

# 228. 219C Phase W — Private Record

Cannot be displayed without authorization.

---

# 229. Workstream 219C — TimeCapsule Integration

## Goal

Allow survivor-created records to become future media without duplicating capsule custody.

---

# 230. 219C Phase X — Plan 212 Content Type

Time Capsule may include:

- photo,
- drawing/sketch,
- recording,
- written record.

Use documentation ID/reference.

---

# 231. 219C Phase Y — Physical Media

If physical:

- Plan 212/Inventory custody moves item.

DocumentationSystem keeps metadata.

---

# 232. 219C Phase Z — Digital Record in Capsule

If capsule supports logical media:

- store immutable documentation ref/snapshot policy.

No duplicate created record.

---

# 233. 219C Phase AA — Opening

TimeCapsuleSystem opens/delivers.

DocumentationSystem records first view if applicable.

---

# 234. Workstream 219C — Shelter Archive Integration

## Goal

Turn intentional documentation into archive evidence while preserving one historical canon.

---

# 235. 219C Phase AB — Archive Candidate

Significant documentation can emit:

```text
ArchiveArtifactCandidate
```

---

# 236. 219C Phase AC — Archive Decision

Plan 162 decides:

- archive,
- prominence,
- retention.

---

# 237. 219C Phase AD — Documentary Evidence

Archive may use:

- subject refs,
- date,
- author,
- quality/accuracy,
- provenance.

---

# 238. 219C Phase AE — Artistic Record

Artistic piece can be archived as cultural artifact without claiming documentary accuracy.

---

# 239. Workstream 219C — EventSystem Integration

## Goal

Tie significant documentation to real events and prevent fabricated history.

---

# 240. 219C Phase AF — Event Opportunity

Semantic event emits:

```text
DocumentationOpportunity
```

where appropriate.

---

# 241. 219C Phase AG — Event Subject

Documentation stores:

```text
event_ref
```

---

# 242. 219C Phase AH — Event Deletion/Retention

If EventSystem rolls up detailed event:

- archive/documentation must preserve stable event identity/summary ref according to Plan 55.

---

# 243. Workstream 219C — Art & Culture Integration

## Goal

Respect Plan 178’s art authority while allowing documentary sketches/photography to participate.

---

# 244. 219C Phase AI — Documentary vs Art

A sketch/photo may have:

```text
documentary purpose
artistic purpose
both
```

---

# 245. 219C Phase AJ — Plan 178

If Plan 178 owns cultural/artistic value:

- Documentation provides created artifact,
- Plan 178 consumes it.

No duplicate culture score.

---

# 246. Workstream 219C — Skills / Work-Time Integration

## Goal

Use real survivor capability and opportunity cost.

---

# 247. 219C Phase AK — Skill Audit

Find existing:

- art,
- writing,
- perception,
- technical,
- social,
- media-related skill.

Prefer reuse.

---

# 248. 219C Phase AL — No Isolated Photography Skill by Default

If photography specialization follow-on later:

- add through SkillProgressionSystem.

Baseline can derive from existing skills/traits.

---

# 249. 219C Phase AM — Time Cost

Every creation action consumes:

- time,
- or work slot.

Auto-document also consumes time.

---

# 250. 219C Phase AN — Fatigue / Injury

Author condition may:

- reduce ability,
- quality,
- prevent action.

Canonical state.

---

# 251. Workstream 219C — Morale / Relations / Psychology

## Goal

Make documentation emotionally useful without turning every photo into a morale consumable.

---

# 252. 219C Phase AO — First Share Reaction

Use:

- subject,
- audience,
- author relation,
- quality,
- sentiment context,
- event tone.

---

# 253. 219C Phase AP — Positive Sharing

Examples:

- family photo,
- successful rebuild,
- celebration.

May produce bounded morale/connection.

---

# 254. 219C Phase AQ — Negative / Difficult Sharing

Examples:

- devastation,
- dead friend,
- famine record.

Can produce grief, anger, resolve, historical awareness.

---

# 255. 219C Phase AR — No Repeated Viewer Farm

Mechanical response keyed by:

```text
documentation + audience + share context
```

---

# 256. 219C Phase AS — Subject Reaction

Survivor may react to being documented.

Relations owner applies.

---

# 257. Workstream 219C — Save / Load / Idempotency

## Goal

Prove equipment costs, quality, creation, album membership, and sharing survive arbitrary save points.

---

# 258. 219C Phase AT — Save Matrix

Test:

```text
opportunity available
creation planned
equipment reserved
film exposure commit
documentation created
asset missing
private record
shared record
album draft
album updated
first-view pending
museum association
time-capsule association
```

---

# 259. 219C Phase AU — Creation Idempotency

Stable documentation ID.

No duplicate photo after load around exposure consumption.

---

# 260. 219C Phase AV — Film Consumption Idempotency

One committed exposure.

---

# 261. 219C Phase AW — Quality Idempotency

Persist resolved assessment.

No reroll.

---

# 262. 219C Phase AX — Album Idempotency

Same photo ID not added twice accidentally.

---

# 263. 219C Phase AY — Share Idempotency

Same audience/context effect once.

---

# 264. 219C Phase AZ — First-View Idempotency

One in-world first view.

---

# 265. 219C Phase BA — Old Save

No documentation.

No retroactive opportunities replayed.

---

# 266. Workstream 219C — Exploit Prevention

## Goal

Prevent photography/writing/material/morale/achievement farms.

---

# 267. 219C Phase BB — Photo Spam

Repeated identical subject/opportunity photos:

- consume real resources/time,
- diminishing novelty for morale/history,
- achievements may apply per unique context.

---

# 268. 219C Phase BC — Film Duplication

Save/load around photo creation cannot retain both:

- old exposure count,
- new photo.

---

# 269. 219C Phase BD — Digital Free-Spam

Even without film:

- time cost,
- power/device availability,
- attention/novelty budget.

---

# 270. 219C Phase BE — Sketch Material Spam

Materials consumed once.

---

# 271. 219C Phase BF — Writing Spam

Records require time.

Achievements can require minimum meaningful length/template/context.

---

# 272. 219C Phase BG — Empty Album Farming

Empty/one-photo albums do not count as completed collections unless criteria explicitly permit.

---

# 273. 219C Phase BH — Share Morale Farming

Repeated share no repeated effect.

---

# 274. 219C Phase BI — Masterpiece Save Scumming

Quality fixed at commit.

---

# 275. 219C Phase BJ — Subject Coverage Farming

“The Documentarian” checks distinct canonical shelter area IDs.

---

# 276. 219C Phase BK — Event Documentation Farming

“The Historian” checks distinct significant event IDs.

---

# 277. Workstream 219C — Edge Cases

## Goal

Make the system robust to no equipment, missing subjects, destroyed locations, deceased authors, and huge archives.

---

# 278. 219C Phase BL — No Documentation

Baseline campaign works.

---

# 279. 219C Phase BM — No Camera

Photography unavailable.

Sketch/writing can still work.

---

# 280. 219C Phase BN — No Art Supplies

Sketch blocked or fallback medium if valid.

---

# 281. 219C Phase BO — No Power

Digital photo/audio/video blocked if equipment requires power.

Film/sketch/writing remain.

---

# 282. 219C Phase BP — Subject Dies Before Planned Photo

Opportunity invalidates or becomes memorial-from-memory sketch/writing option.

Do not take posthumous “live photo.”

---

# 283. 219C Phase BQ — Author Dies During Multi-Step Project

Project:

- incomplete,
- inherited draft,
- abandoned,

according to type/policy.

---

# 284. 219C Phase BR — Location Destroyed Before Capture

Opportunity invalidates.

Past photo remains valid.

---

# 285. 219C Phase BS — Camera Destroyed After Photo

Past record unaffected.

---

# 286. 219C Phase BT — Missing Asset File

Fallback presentation.

No data loss.

---

# 287. 219C Phase BU — Private Author Dies

Privacy policy:

- may transfer via Plan 206/210,
- may remain private,
- may become archive candidate only if ownership/legal policy permits.

No automatic public release.

---

# 288. 219C Phase BV — Extensive Documentation

Stress:

```text
5,000 records
500 albums
```

Use indexing/retention.

---

# 289. 219C Phase BW — Duplicate Subject IDs

Multiple subjects valid.

Stable ordered/role list.

---

# 290. 219C Phase BX — Anonymous Historical Record

If discovered external record support exists:

- provenance explicit,
- no fake author survivor.

---

# 291. Workstream 219C — Determinism

## Goal

Guarantee repeatable documentation outcomes.

---

# 292. 219C Phase BY — Stable Subject Ordering

Sort subject refs by:

```text
role + type + ID
```

before digest/RNG.

---

# 293. 219C Phase BZ — Same-Creation Digest

Build:

```text
documentation_creation_digest
```

from:

- author,
- type,
- subjects,
- equipment,
- conditions,
- time,
- quality.

---

# 294. 219C Phase CA — Same Inputs

Same:

```text
seed
author state
equipment
subject context
location
time spent
```

→ same quality record.

---

# 295. 219C Phase CB — Asset Ref Non-Determinism Boundary

Generated media asset may vary if produced externally.

This does not affect simulation digest.

Digest uses metadata, not pixels/audio waveform.

---

# 296. Workstream 219C — Data Integrity

## Goal

Reject broken templates, media types, equipment refs, album refs, and impossible subjects.

---

# 297. 219C Phase CC — Template Validation

Validate:

- unique ID,
- valid documentation type,
- valid subject types,
- valid equipment profile,
- valid material refs,
- valid quality profile,
- localization keys.

---

# 298. 219C Phase CD — Equipment Validation

Photo template requiring camera must resolve to valid equipment tag.

---

# 299. 219C Phase CE — Media Validation

Audio/video templates require supported recorder/camera capabilities or are explicitly disabled.

---

# 300. 219C Phase CF — Album Validation

Photo IDs exist.

No duplicate membership.

Cover photo belongs to album.

---

# 301. 219C Phase CG — Subject Validation

Canonical subject refs resolve.

Historical/event rollups use valid retained IDs.

---

# 302. 219C Phase CH — Visibility Validation

Private/shared states valid.

---

# 303. 219C Phase CI — No Duplicate Authority Scan

Flag DocumentationSystem if it adds:

```text
inventory quantities
museum slot state
memorial state
archive chronicle state
journal master text store
time-capsule open state
```

---

# 304. Workstream 219C — `--documentation-selftest`

Required scenarios:

1. photograph with film camera,
2. film exposure consumed,
3. digital photo with power,
4. digital photo without power rejected,
5. candid photo,
6. posed photo,
7. documentary photo,
8. artistic photo,
9. group photo,
10. sketch pencil,
11. sketch charcoal,
12. sketch ink,
13. sketch watercolor,
14. written journal link,
15. written letter,
16. written report,
17. chronicle,
18. poem,
19. essay,
20. audio record,
21. audio transcript,
22. video record,
23. video fallback metadata,
24. album create,
25. album add/remove/reorder,
26. private documentation,
27. shelter sharing,
28. repeated share no duplicate effect,
29. memorial association,
30. belonging association,
31. museum display adapter,
32. time-capsule reference,
33. archive candidate,
34. deterministic quality,
35. author death,
36. subject death after creation,
37. missing asset fallback,
38. save/load,
39. old save,
40. extensive-documentation index test.

---

# 305. Workstream 219C — Deliberate Failure Proof

Break:

- missing author,
- dead author creation,
- nonexistent subject,
- missing camera,
- insufficient film,
- no digital power,
- invalid sketch medium,
- duplicate album photo,
- invalid visibility,
- invalid asset ref.

Assert correct gate fails.

---

# 306. Workstream 219C — Long-Horizon Documentation Soak

## Goal

Prove documentation enriches history without becoming repetitive busywork or save bloat.

---

# 307. 219C Phase CJ — 200-Day Profiles

Run:

```text
no_documentation
casual_documentarian
photography_heavy
sketch_heavy
writer_heavy
auto_document
museum_archive_heavy
family_memory_heavy
```

---

# 308. 219C Phase CK — Metrics

Record:

```text
opportunities
records created
records/type
unique subjects
unique events
materials consumed
time spent
albums
shares
first-view reactions
museum exhibits
archive promotions
capsule inclusions
```

---

# 309. 219C Phase CL — Activity Cost

Measure survivor-hours spent documenting.

Documentation must compete with work/survival.

---

# 310. 219C Phase CM — Opportunity Frequency

Too many:

- busywork/spam.

Too few:

- system invisible.

Tune by event significance + daily-life cadence.

---

# 311. 219C Phase CN — Media Mix

Ensure one type does not dominate solely because it is cheapest.

Photography vs sketch vs written vs audio/video each needs niche.

---

# 312. 219C Phase CO — Photo Value

Fast, vivid, equipment-dependent.

---

# 313. 219C Phase CP — Sketch Value

Low-tech, slower, artistic/documentary flexibility.

---

# 314. 219C Phase CQ — Written Value

Deep context, cheap equipment, time-heavy.

---

# 315. 219C Phase CR — Audio Value

Voice/oral history, equipment/power-dependent.

---

# 316. 219C Phase CS — Video Value

Richest presentation, highest resource cost/rarity.

---

# 317. 219C Phase CT — Auto-Document Balance

Auto mode should not:

- consume all film,
- overbook survivors,
- document trivial events continuously.

Config:

```text
daily/weekly budget
minimum significance
preferred documentarian
reserve materials threshold
```

---

# 318. 219C Phase CU — Sentimental vs Quality Review

Ensure:

- low-quality family photo can matter,
- high-quality empty landscape is not always best.

---

# 319. 219C Phase CV — Sharing Balance

Sharing should produce bounded, context-sensitive effects.

No universal +morale per record.

---

# 320. 219C Phase CW — Documentation Discovery

Old documentation discovered later can be meaningful via:

- storage,
- inheritance,
- museum/archive recovery,
- time capsule.

Do not randomly “rediscover” already-known records.

---

# 321. Workstream 219C — Performance

## Goal

Keep documentation browsing and simulation cheap with thousands of records.

---

# 322. 219C Phase CX — Creation Event-Driven

No per-frame work.

---

# 323. 219C Phase CY — Indexes

Derived indexes:

```text
by type
by author
by subject
by date
by location
by album
by visibility
by association
```

Rebuildable after load.

---

# 324. 219C Phase CZ — Album Storage

Store photo IDs only.

---

# 325. 219C Phase DA — Asset Lazy Loading

Do not load all full-resolution media assets for documentation panel.

Use thumbnails/lazy load.

---

# 326. 219C Phase DB — Headless Save

No media binary serialization.

---

# 327. 219C Phase DC — Stress Fixture

Test:

```text
5,000 documentation records
500 albums
100 memorial links
100 museum links
100 capsule links
```

Measure:

- save/load,
- filter,
- album open,
- subject search,
- daily tick.

---

# 328. Workstream 219C — Retention

## Goal

Preserve meaningful survivor-created history without deleting user-authored content unexpectedly.

---

# 329. 219C Phase DD — Plan 55 Retention

Keep:

- all user-created/player-directed records unless explicit deletion,
- landmark/high-significance records,
- active album members,
- museum/memorial/capsule-linked records.

Roll up only:

- low-value auto-generated metadata history,
- repeated share logs,
- routine view logs.

---

# 330. 219C Phase DE — User Authored Text

Never silently discard.

---

# 331. 219C Phase DF — Asset Retention

If optional generated media assets are large:

- allow thumbnail cache rebuild,
- keep canonical metadata,
- do not remove irreplaceable user-facing asset without policy.

---

# 332. 219C Phase DG — Historical Summary

Archive can retain summary even if low-level share/view logs roll up.

---

# 333. Workstream 219C — Accessibility

## Goal

Ensure a visual-documentation feature remains fully usable by players who cannot see/hear the underlying media.

---

# 334. 219C Phase DH — Photo Alt Description

Every photo record has deterministic textual description:

```text
subjects
location
moment
event context
```

---

# 335. 219C Phase DI — Sketch Alt Description

Describe:

- subject,
- medium,
- style/context,
- documentary accuracy.

---

# 336. 219C Phase DJ — Audio Transcript

Required.

---

# 337. 219C Phase DK — Video Transcript / Audio Description

Required for meaningful content.

---

# 338. 219C Phase DL — No Quality Color-Only

Quality band uses text/icon.

---

# 339. 219C Phase DM — Keyboard / Controller

Album organization and viewers fully navigable.

---

# 340. 219C Phase DN — Reduced Motion

Video playback respects reduced motion where applicable.

Static summary available.

---

# 341. Workstream 219C — Human Narrative Review

## Goal

Ensure documentation feels like intentional memory-making rather than collectible generation.

---

# 342. 219C Phase DO — Review Questions

```text
Why did this survivor choose to document this?
Was the subject real and meaningful?
Did equipment/time create an opportunity cost?
Does quality reflect conditions?
Would this record matter even without a morale reward?
Does it reveal something about the author or shelter?
Does the record still make sense years later?
```

---

# 343. 219C Phase DP — Swap Test

Swap author.

If record feels identical despite different survivor:

- add author traits/skills/context.

---

# 344. 219C Phase DQ — Subject Test

Replace real event with generic placeholder.

If nothing changes:

- template lacks historical specificity.

---

# 345. 219C Phase DR — Busywork Test

Ask player to document 10 routine events.

If process feels repetitive:

- strengthen opportunities/presets/auto mode,
- reduce cadence.

---

# 346. 219C Phase DS — Historical Value Test

Open a 100-day-old album.

Player should understand:

- who made it,
- who appears,
- what changed since then.

---

# 347. Documentation

Create:

```text
docs/systems/SURVIVOR_DOCUMENTATION.md
```

Include:

- authority boundaries,
- provenance,
- subject refs,
- creation opportunities,
- equipment/materials,
- quality,
- privacy/sharing,
- albums,
- media asset refs,
- Journal/Memorial/Museum/TimeCapsule/Archive integration,
- save/idempotency,
- retention.

---

# 348. Content Authoring Guide

Create:

```text
docs/content/DOCUMENTATION_TEMPLATE_AUTHORING.md
```

Checklist:

```text
1. choose documentation type
2. identify canonical subject producer
3. identify equipment/material requirements
4. define creation time
5. define quality profile
6. define privacy/default audience
7. define historical/sentimental tags
8. define integration hooks
9. add localization/accessibility description
10. add targeted selftest
```

---

# 349. Integrated Documentation Pipeline

```text
canonical event / person / place / object
                 │
                 ▼
      documentation opportunity
                 │
                 ▼
        survivor creation action
                 │
         ┌───────┼────────┬────────┐
         ▼       ▼        ▼        ▼
       Photo   Sketch   Written   Audio/Video
         │       │        │        │
         └───────┼────────┴────────┘
                 ▼
        DocumentationRecord
                 │
          ┌──────┼───────────────┐
          ▼      ▼               ▼
        Album   Sharing        Belonging
          │      │               │
          └──────┼───────────────┘
                 ▼
      Journal / Memorial / Museum
                 │
                 ▼
        Time Capsule / Archive
```

---

# 350. Documentation Authority Contract

DocumentationSystem owns:

- created record metadata,
- authorship,
- subject provenance,
- creation quality,
- privacy/sharing,
- albums,
- presentation asset refs.

---

# 351. Journal Authority Contract

JournalSystem owns canonical journal entries.

Documentation links.

---

# 352. Inventory Authority Contract

Inventory owns:

- cameras,
- film,
- art supplies,
- recorders,
- batteries/storage items.

---

# 353. Belongings Authority Contract

Plan 210 owns personal ownership/provenance of physical documentation objects.

---

# 354. Memorial Authority Contract

MemorialSystem owns remembrance state.

Documentation can be memorial material.

---

# 355. Museum Authority Contract

Plan 218 owns exhibit placement/display state.

Documentation supplies exhibit candidate.

---

# 356. Time Capsule Authority Contract

Plan 212 owns sealed custody/open timing.

Documentation supplies record/content reference.

---

# 357. Archive Authority Contract

Plan 162 owns shelter historical archive.

Documentation supplies survivor-created evidence/artifact.

---

# 358. Art Authority Contract

Plan 178 owns culture/art effects where applicable.

Documentation owns documentary provenance.

---

# 359. Event Authority Contract

EventSystem owns event truth.

Documentation may depict it.

---

# 360. Quality Contract

Quality is determined once at creation from canonical factors.

---

# 361. Sentiment Contract

Sentimental meaning is reader/context-dependent.

Not identical to technical quality.

---

# 362. Asset Contract

Pixels/audio/video are presentation.

Metadata/provenance is simulation truth.

---

# 363. Sharing Contract

Sharing affects a defined audience/context once.

UI viewing is not equivalent to in-world sharing.

---

# 364. Save Contract

Persist documentation/albums/share state/refs.

Do not duplicate inventory, journal, memorial, museum, archive, or capsule state.

---

# 365. Old-Save Contract

No documentation and no retroactive backfill.

---

# 366. Determinism Contract

Same:

```text
author
subject
equipment
conditions
time
seed
```

→ same documentation metadata/quality.

---

# 367. Accessibility Contract

Every visual/audio record has a text-accessible representation.

---

# 368. Retention Contract

User-created documentation is durable.

Routine generated logs may compact.

---

# 369. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| documentation duplicates archive | High | High | archive candidate/ref only |
| written records duplicate Journal | High | High | linked journal IDs |
| film state duplicated in photo DTO | High | High | inventory exposure transaction |
| generated image treated as world truth | Medium | High | metadata authority |
| auto-document fabricates events | Medium | High | opportunity-driven only |
| photography becomes free spam | High | Medium | equipment/time/novelty |
| share becomes morale farm | High | High | audience-context idempotency |
| albums duplicate photo objects | Medium | Medium | ID membership only |
| sentiment conflated with quality | High | Medium | separate models |
| audio/video impossible without assets | Medium | Medium | transcript/metadata fallback |
| privacy ignored | Medium | Medium | visibility/consent |
| old saves retroactively generate archive | Medium | High | no backfill |
| thousands of records bloat save/UI | Medium | Medium | indexes + retention |
| missing assets destroy records | Medium | High | asset-optional fallback |

---

# 370. Commit Strategy

## 219A — Foundation

### C2[45].1 — baseline + documentation ownership ADR

### C2[45].2 — documentation/type-specific DTOs

### C2[45].3 — typed subject/provenance model

### C2[45].4 — opportunity system / auto-document rules

### C2[45].5 — equipment/material/work-time adapters

### C2[45].6 — deterministic quality model

### C2[45].7 — privacy/share/view model

### C2[45].8 — media asset refs/headless fallback

### C2[45].9 — save/versioning / old-save no-backfill

### C2[45].10 — composition/events/ports/diagnostics

### Gate: 219A complete

---

## 219B — Media / Albums / UI

### C2[45].11 — film/digital photography

### C2[45].12 — candid/posed/documentary/artistic photo modes

### C2[45].13 — sketching / media/time

### C2[45].14 — written records / Journal bridge

### C2[45].15 — audio recordings / transcript

### C2[45].16 — video recordings / fallback

### C2[45].17 — albums / themes / ordering

### C2[45].18 — 20 documentation templates

### C2[45].19 — quality/sentiment/historical value

### C2[45].20 — sharing/reactions

### C2[45].21 — documentation/photo/sketch/record UI

### C2[45].22 — audio/video/album UI

### C2[45].23 — events/hooks/tutorial/accessibility

### Gate: 219B complete

---

## 219C — Integration / Validation

### C2[45].24 — Journal integration

### C2[45].25 — Memorial integration

### C2[45].26 — Inventory / PersonalBelongings

### C2[45].27 — Museum / Art & Culture

### C2[45].28 — Time Capsule / Archive

### C2[45].29 — event/skills/work-time integration

### C2[45].30 — morale/relations/psychology

### C2[45].31 — save-load/idempotency matrix

### C2[45].32 — exploit prevention

### C2[45].33 — edge cases

### C2[45].34 — data-integrity / ownership gates

### C2[45].35 — `--documentation-selftest`

### C2[45].36 — deliberate failure proof

### C2[45].37 — 200-day documentation soak

### C2[45].38 — 5,000-record performance/retention stress

### C2[45].39 — accessibility/narrative playtest/docs

### Gate: 219C complete

---

# 371. Verification Checklist

Run source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --documentation-selftest
```

Also run repository-canonical equivalents of:

```text
documentation authority scan
camera/film custody idempotency test
subject provenance integrity test
Journal duplicate-truth audit
Museum/Archive/TimeCapsule ownership audit
same-input documentation quality digest replay
old-save no-backfill fixture
share/view consequence idempotency test
5,000-record indexing/save stress
documentation accessibility coverage test
```

---

# 372. `--documentation-selftest` Acceptance Matrix

| Scenario | Expected |
|---|---|
| Film photo | camera + exposure required |
| Digital photo | power/device required |
| Candid | real subject/context |
| Posed | cooperation checked |
| Documentary | accuracy/provenance emphasized |
| Artistic | artistic quality distinct |
| Group photo | multiple canonical subjects |
| Pencil sketch | valid |
| Charcoal sketch | valid |
| Ink sketch | valid |
| Watercolor sketch | valid |
| Journal record | Journal link |
| Letter | written documentation |
| Report | event/data refs |
| Chronicle | multiple historical refs |
| Poem | creative/non-documentary |
| Essay | authored record |
| Audio | device + transcript |
| Video | device + transcript/fallback |
| Album | ID membership |
| Private | visibility enforced |
| Shared | audience effect once |
| Memorial | association only |
| Belonging | ownership external |
| Museum | display external |
| Time capsule | custody/open external |
| Archive | candidate/ref |
| Quality | deterministic |
| Missing asset | record survives |
| Save/load | exact state |
| Old save | empty |
| Extensive | bounded/indexed |

---

# 373. Flagship Definition of Done — Foundation

- [ ] `DocumentationSystem.cs`,
- [ ] five documentation types,
- [ ] typed subject refs,
- [ ] authorship/provenance,
- [ ] opportunity generation,
- [ ] auto-document policy,
- [ ] equipment/material requirements,
- [ ] work-time costs,
- [ ] deterministic quality,
- [ ] sentiment/historical separation,
- [ ] privacy/sharing,
- [ ] media asset refs,
- [ ] headless fallback,
- [ ] save/versioning,
- [ ] old-save behavior,
- [ ] semantic events,
- [ ] ports/diagnostics.

---

# 374. Flagship Definition of Done — Runtime / UI

- [ ] film photography,
- [ ] digital photography,
- [ ] photo subjects/location/quality,
- [ ] candid/posed/documentary/artistic moments,
- [ ] sketching,
- [ ] five sketch media,
- [ ] written records,
- [ ] six record types,
- [ ] audio recording,
- [ ] video recording,
- [ ] transcripts,
- [ ] albums,
- [ ] seven album themes,
- [ ] sharing,
- [ ] 15+ templates,
- [ ] recommended 20 templates,
- [ ] documentation panel,
- [ ] photo viewer,
- [ ] sketch gallery,
- [ ] record library,
- [ ] audio/video viewers,
- [ ] album panel,
- [ ] documentation log,
- [ ] source events/hooks,
- [ ] tutorial/tooltips,
- [ ] accessibility.

---

# 375. Flagship Definition of Done — Integration / Validation

- [ ] JournalSystem,
- [ ] MemorialSystem,
- [ ] Inventory,
- [ ] PersonalBelongingsSystem,
- [ ] ShelterMuseumSystem,
- [ ] TimeCapsuleSystem,
- [ ] Shelter Archive,
- [ ] Art & Culture,
- [ ] semantic events,
- [ ] skills/work-time,
- [ ] morale/relations/psychology,
- [ ] no duplicate journal/archive/museum/capsule truth,
- [ ] creation/equipment/share idempotency,
- [ ] anti-spam/anti-quality-reroll,
- [ ] no-equipment/no-power edges,
- [ ] author/subject death edges,
- [ ] missing asset fallback,
- [ ] deterministic replay,
- [ ] data integrity,
- [ ] deliberate failure fixtures,
- [ ] selftest,
- [ ] 200-day soak,
- [ ] 5,000-record stress,
- [ ] retention,
- [ ] accessibility,
- [ ] narrative playtest,
- [ ] docs.

---

# 376. Global Definition of Done

- [ ] no duplicate shelter archive,
- [ ] no duplicate journal chronology,
- [ ] no duplicate memorial system,
- [ ] no duplicate museum display state,
- [ ] no duplicate inventory/film state,
- [ ] no duplicate time-capsule custody,
- [ ] no generated-pixel world truth,
- [ ] no retroactive fabricated documentation,
- [ ] no free digital-photo spam,
- [ ] no repeated share/view morale farming,
- [ ] no quality save-scumming,
- [ ] no missing-media data loss,
- [ ] full verification green.

---

# 377. Closure Report Template

```markdown
## C2[45] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Journal authority:
- Memorial authority:
- Inventory/equipment:
- Personal belongings:
- Museum:
- Time capsules:
- Archive:
- Art/culture:
- Existing photography/documentation matches:

### 219A — Foundation
- DocumentationSystem:
- Documentation types:
- Subject refs:
- Provenance:
- Opportunities:
- Auto-document:
- Equipment port:
- Work-time:
- Quality:
- Sentiment:
- Privacy/sharing:
- Asset fallback:
- Save schema:
- Old-save behavior:
- Missing ports:
- Result:

### 219B — Runtime / UI
- Photos:
- Film photos:
- Digital photos:
- Sketches:
- Written records:
- Audio:
- Video:
- Albums:
- Templates:
- Shares:
- First-view effects:
- UI:
- Missing assets:
- Result:

### 219C — Integration
- Journal:
- Memorial:
- Inventory:
- Belongings:
- Museum:
- Time Capsule:
- Archive:
- Art & Culture:
- Events:
- Skills/work-time:
- Morale/relations:
- Duplicate truth findings:
- Result:

### Long-Run / Balance
- 200-day records:
- Photos:
- Sketches:
- Written:
- Audio:
- Video:
- Albums:
- Survivor-hours:
- Film consumed:
- Power/storage used:
- Shares:
- Museum exhibits:
- Archive promotions:
- Capsule inclusions:
- Auto-document frequency:
- Busywork findings:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Documentation selftest:
- Authority scan:
- Film idempotency:
- Provenance integrity:
- Old-save fixture:
- Same-input quality digest:
- 5,000-record stress:
- Accessibility:
- Result:

### Final Metrics
- DOCUMENTATION_TOTAL:
- DOCUMENTATION_PHOTOS:
- DOCUMENTATION_SKETCHES:
- DOCUMENTATION_WRITTEN:
- DOCUMENTATION_AUDIO:
- DOCUMENTATION_VIDEO:
- DOCUMENTATION_ALBUMS:
- DOCUMENTATION_SHARED:
- DOCUMENTATION_ARCHIVED:
- DOCUMENTATION_MUSEUM_DISPLAYED:
- DOCUMENTATION_CAPSULE_LINKED:
- FILM_DUPLICATION_ERRORS:
- QUALITY_REROLL_ERRORS:
- SHARE_DUPLICATE_EFFECT_ERRORS:
- INVALID_SUBJECT_REFS:
- MISSING_MEDIA_ASSET_REFS:
- DUPLICATE_AUTHORITY_VIOLATIONS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Media asset generation:
- Photography specialization:
- Oral history:
- Documentation trading:
- Museum exhibitions:
- UI:
```

---

# 378. Final Execution Directive

Execute Plan 219 as a **survivor-authored documentation and provenance layer over the existing event, journal, memorial, item, belonging, museum, time-capsule, and archive systems**.

The critical sequence is:

```text
audit existing historical/media-adjacent owners
→ establish DocumentationSystem ownership boundaries
→ create typed subject/provenance records
→ create real documentation opportunities from canonical events/daily life
→ validate author, equipment, materials, power, and time
→ deterministically resolve type-specific quality
→ commit one immutable documentation record
→ optionally attach presentation media asset
→ organize records by ID into albums
→ share to real audiences with one-time contextual consequences
→ route Journal/Memorial/Museum/TimeCapsule/Archive interactions through their canonical owners
→ preserve accessible textual representation for every media record
→ validate save/load, anti-spam, long-horizon retention, and historical specificity
```

Do not put film remaining on photographs.

Do not copy photographs into albums.

Do not copy journal/archive/museum/time-capsule state into DocumentationSystem.

Do not fabricate an event simply because auto-document wants content.

Do not use generated pixels/audio/video as authoritative proof of simulation facts.

Do not score player-written prose semantically as “good” or “bad” writing.

Do not make every shared photograph a repeatable morale bonus.

The strongest authority rule is:

> **DocumentationSystem owns the fact that a survivor intentionally created a particular record of a particular canonical subject at a particular time and place; all equipment, inventory, journal history, memorial state, exhibit placement, item ownership, delayed capsule custody, and shelter historical canon remain owned by their existing systems.**

The strongest provenance rule is:

> **A documentary record may only claim what its typed canonical subject references support. Presentation art may embellish visually, but metadata and source refs remain the historical truth.**

The strongest emotional rule is:

> **Technical quality, historical importance, and sentimental significance are separate. A blurry photograph of a dead friend can matter more emotionally than a technically perfect landscape, while a rough documentary sketch of a destroyed shelter room can still be historically invaluable.**

The flagship acceptance scenario is:

> **Create a seeded shelter event where three named survivors repair a damaged water room after a crisis. Assign one available survivor with a real film camera and loaded film to take a documentary group photograph. Verify the canonical camera/film inventory supplies exactly one exposure, the photo binds the three survivor IDs, location ID, event ID, day, camera instance, and capture conditions, and quality resolves deterministically from photographer capability, camera condition, lighting, and cooperation. Save/load immediately around creation and verify one photo and one consumed exposure. Add the photo by ID to both an `Events` album and a survivor-owned `DailyLife` album without copying the photo record. Share it with the shelter once and route one contextual morale/relationship response; reopening or sharing to the same audience again must not duplicate the effect. Later kill one photographed survivor: the existing photo gains memorial/sentimental context without changing its original quality or provenance. Display it through Plan 218 Museum, link it to MemorialSystem, then place the same documentation reference into a Plan 212 time capsule while all display/custody owners remain external. Remove the optional image asset and verify the photo still renders from its accessible metadata description. Finally replay with the same seed/state and verify identical documentation ID inputs, quality assessment, film transaction, subject provenance, album membership, and simulation digest.**
