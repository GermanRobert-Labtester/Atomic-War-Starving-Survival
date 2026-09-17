# C2 — Flagship Integration Plan [34]: Shelter Archive, Institutional Memory, Memorial Continuity, and Searchable Campaign History

> **Deliverable:** `C2_planintegration[34].md`
> **Source scope:** Plan 162 — *Shelter History & Archive System*
> **Primary objective:** create a deterministic institutional archive that records significant shelter decisions, events, memorials, milestones, discoveries, and achievements as the community’s collective memory; exposes browse/search/timeline views; supports optional player-authored reflections; and feeds legacy/epilogue/history features without duplicating the journal, memorial, consequence, governance, colony, calendar, or completion-record authorities that already own the underlying facts.
> **Required execution order:** **162A Foundation/System Contract → 162B Entries, Memorial References, Timeline, Search & UI → 162C Cross-System Integration, Save/CI, Retention, and Historical Closure**
> **Hard dependencies:** `JournalSystem`, `CampaignConsequenceLedger`, `MemorialSystem`, `CampaignCalendar`, Plan 31 semantic event vocabulary/cause IDs, Plan 34 completion/epilogue record, Plan 55 retention, Plan 59 gate/claims discipline, Plan 158 disaster events, and Plan 159/160 governance/colony systems when those plans exist.
> **Scope discipline:** no duplicate journal store, no duplicate memorial/death truth, no duplicate consequence ledger, no duplicate governance decision state, no duplicate colony history authority, no separately persisted timeline that can drift from entries, no persisted category definitions that duplicate static data, no archive entry generated from UI polling, no silent mutation of historical records after creation, no player deletion of canonical institutional history, and no manual-entry feature that overwrites canonical event history.

---

# 0. Executive Intent

ASHFALL already remembers many facts in isolated systems:

- personal journal entries,
- campaign flags/counters,
- survivor memorials,
- day/calendar state,
- faction and governance consequences,
- disaster outcomes,
- campaign completion facts.

What is missing is an **institutional memory layer**.

Today the game can know:

```text
a survivor died
a policy passed
a raid happened
a discovery occurred
a milestone was reached
```

without producing one coherent, browsable story of the shelter.

The target architecture is:

```text
canonical game events / outcome records
               │
               ▼
      ShelterArchiveSystem
               │
        ┌──────┼─────────────┐
        ▼      ▼             ▼
  archive entry memorial ref manual reflection
        │      │             │
        └──────┼─────────────┘
               ▼
       derived timeline/index
               │
       ┌───────┼────────┐
       ▼       ▼        ▼
     browse   search   filters
       │
       ▼
 journal links / epilogue / legacy / exhibitions
```

The archive is therefore not a second simulation.

It is a **durable interpretation and retrieval layer over facts that already happened**.

The strongest product outcome is:

> **By late campaign, the player can open the archive and reconstruct the shelter’s history—who died, which choices defined the community, which disasters were survived, what was discovered, what milestones mattered, and which people shaped the shelter—without the archive inventing facts or drifting away from the systems that actually own those events.**

---

# 1. Source Diagnosis

The source establishes:

- `JournalSystem` records personal entries,
- `CampaignConsequenceLedger` tracks flags/counters,
- `MemorialSystem` records individual deaths,
- `CampaignCalendar` owns day progression,
- there is no institutional archive or historical timeline,
- the plan proposes six core archive categories:
  - Decisions,
  - Events,
  - Memorials,
  - Milestones,
  - Discoveries,
  - Achievements,
- automatic event recording should be the default,
- manual reflection entries should be optional,
- memorials should integrate with deaths and anniversaries,
- a searchable/filterable chronological timeline is required,
- archive data should feed legacy and epilogue,
- archive events, quests, tutorial, tooltips, and UI are required,
- 10 archive categories are requested in the data authority even though the source names six core categories,
- old saves, deterministic recording, headless behavior, and CI are required.

The key architectural interpretation is:

```text
archive entry = historical projection/reference
```

not:

```text
archive entry = new owner of the underlying event
```

Examples:

```text
death truth → MemorialSystem / SurvivorFate
archive → memorial/history reference

policy truth → GovernanceSystem
archive → decision record

disaster truth → DisasterResponseSystem
archive → event record

campaign flag truth → CampaignConsequenceLedger
archive → contextual history entry
```

---

# 2. Program-Level Success Criteria

C2[34] closes only when all of the following are true.

1. Significant canonical events can automatically create archive entries.
2. Automatic entries are deterministic and idempotent.
3. Archive entry IDs are stable across save/load/replay.
4. Archive does not create or own the underlying gameplay fact.
5. Memorial entries reference canonical memorial/death records.
6. Historical timeline is derived from archive entries, not duplicated.
7. Search indexes are derived/rebuildable.
8. Static archive categories come from one data authority.
9. The source’s six core categories and requested 10-category data file are explicitly reconciled.
10. Manual reflections remain optional and clearly distinguished from canonical institutional entries.
11. Player-written text is preserved exactly as user-authored content, subject only to storage/format constraints.
12. Canonical automatic entries cannot be deleted or silently rewritten by normal player actions.
13. Significant entry text uses localization keys/templates where possible rather than frozen inline English.
14. Archive entries store stable references/cause IDs so later UI can resolve participants and sources.
15. Memorial anniversaries use calendar-derived dates.
16. Archive search supports keyword, tag, participant, category, significance, and date.
17. Large archives remain performant and retention-aware.
18. Old saves initialize cleanly.
19. Archive can reconstruct useful history even if some optional integration systems are not yet present.
20. Epilogue/legacy consume archive through explicit read models rather than parsing UI prose.
21. Headless CI proves automatic recording, search, timeline, save/load, and idempotency.
22. Extensive-archive stress tests remain bounded in size and query time.
23. Empty archive is valid.
24. Archive UI shows source/provenance for canonical entries where useful.
25. No archive action mutates core gameplay state.

---

# 3. Architectural Invariants

## 3.1 Archive owns historical records, not gameplay facts

It may store:

- event identity,
- category,
- significance,
- references,
- localized narrative template/parameters,
- manual notes,
- historical tags.

It does not own:

- survivor alive/dead state,
- policy state,
- faction standing,
- quest state,
- disaster state,
- colony state,
- achievement truth.

## 3.2 Automatic entries are event-driven

No daily polling such as:

```text
if survivor is dead and no entry exists...
```

unless used only for migration repair.

Prefer canonical semantic events.

## 3.3 Timeline is derived

No second `historicalTimeline` list persisted separately from entries.

## 3.4 Categories are static data

Do not persist the category catalog in every save.

## 3.5 Archive history is immutable in meaning

Canonical entries can gain presentation metadata if necessary, but must not be rewritten to contradict prior history.

## 3.6 Manual reflections are a separate entry provenance

Player-authored content never masquerades as system-generated canonical fact.

## 3.7 Entry IDs are deterministic

Same canonical source event produces the same archive entry ID.

## 3.8 Search index is rebuildable

Do not persist an index unless proven necessary.

## 3.9 Memorial ownership remains canonical

Archive references MemorialSystem state.

## 3.10 Retention preserves institutional meaning

Plan 55 may compact low-value detail, but landmark history remains durable.

---

# 4. Dependency Graph

```text
Semantic events / cause IDs
        │
        ├────────► JournalSystem
        ├────────► MemorialSystem
        ├────────► Governance
        ├────────► DisasterResponse
        ├────────► ColonySystem
        ├────────► CampaignConsequenceLedger
        └────────► achievement/discovery systems
                         │
                         ▼
                 ShelterArchiveSystem
                         │
            ┌────────────┼─────────────┐
            ▼            ▼             ▼
      automatic entry  memorial ref  manual note
            │            │             │
            └────────────┼─────────────┘
                         ▼
                 derived timeline/index
                         │
          ┌──────────────┼─────────────┐
          ▼              ▼             ▼
       Archive UI      epilogue      legacy/export
```

---

# 5. Baseline Capture

Before implementation, inspect and record:

- `JournalSystem` entry schema/API,
- `CampaignConsequenceLedger` flags/counters,
- `MemorialSystem` memorial/death representation,
- `CampaignCalendar` day/date conversion,
- semantic event envelope/kinds from Plan 31,
- completion/epilogue record APIs,
- governance decision APIs if present,
- disaster event APIs from Plan 158 if present,
- colony-history APIs if present,
- achievement/discovery milestone producers,
- current save registry,
- current UI routing/search widgets.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture one representative campaign fixture containing:

- at least one survivor death,
- one major decision,
- one discovery,
- one milestone,
- one battle/disaster/event if available.

---

# 6. Workstream 162A — Foundation / System Contract

## Goal

Create one deterministic institutional archive authority with provenance-aware entries, immutable canonical records, category data, save/version support, and event-driven recording.

---

# 7. 162A Phase A — Create `ShelterArchiveSystem`

Path:

```text
Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs
```

Responsibilities:

- receive canonical historical events,
- decide whether an event qualifies for institutional recording,
- create deterministic archive entries,
- maintain manual reflection entries,
- expose filtered/searchable read models,
- maintain memorial references,
- capture/restore archive-specific state,
- expose export/epilogue projections.

It does not own source-system gameplay facts.

---

# 8. 162A Phase B — Split Entry Types

Prefer:

```text
ArchiveEntry
ArchiveEntrySource
ArchiveEntryContent
ArchiveManualReflection
ArchiveMemorialReference
ArchiveSearchQuery
ArchiveSearchResult
ArchiveTimelineItem
```

Avoid one DTO containing mutually exclusive fields.

---

# 9. 162A Phase C — Archive Entry Fields

Recommended:

```text
entry_id
entry_type
category_id
significance
day
source_event_id
cause_id
participant_ids
location_ids
faction_ids
tags
content_template_key
content_parameters
provenance
created_by
```

For manual reflection:

```text
manual_title
manual_body
custom_tags
player_assigned_significance
```

---

# 10. 162A Phase D — Entry Type Vocabulary

Source types:

```text
Decision
Event
Memorial
Milestone
Discovery
Achievement
```

Use typed enum/stable IDs.

---

# 11. 162A Phase E — Significance Vocabulary

```text
Minor
Notable
Major
Historic
```

Automatic significance comes from event rules.

Manual entries may let the player assign significance within safe bounds.

---

# 12. 162A Phase F — Entry Provenance

Add explicit provenance:

```text
SystemGenerated
PlayerReflection
MigrationImported
LegacyImported
```

This prevents ambiguity.

---

# 13. 162A Phase G — Deterministic Entry ID

Prefer:

```text
archive:{source_event_id}:{projection_kind}
```

or stable hash.

Requirements:

- same source event → same ID,
- duplicate delivery → idempotent no-op,
- stable after save/load.

---

# 14. 162A Phase H — `archive_categories.json`

Create:

```text
Assets/StreamingAssets/Data/archive_categories.json
```

Fields:

```text
category_id
name_key
description_key
entry_types
display_order
default_significance_filter
retention_class
icon_key if UI uses icons
```

---

# 15. 162A Phase I — Reconcile 6 Core vs 10 Data Categories

The source names six required core categories but asks for 10 catalog rows.

Do not silently invent four categories.

Recommended strategy:

Keep the six core categories exactly:

```text
decisions
events
memorials
milestones
discoveries
achievements
```

Then add four **substantively justified institutional categories** only after auditing event producers.

Possible candidates:

```text
diplomacy
disasters
community
knowledge
```

Alternatively keep 10 rows as category/subcategory entries if the loader supports hierarchy.

Final rule:

```text
all 10 must have real producers or documented future-bound status
```

No dead decorative category.

---

# 16. 162A Phase J — Category Definitions Are Static

Do not store category DTOs inside `ShelterArchiveState`.

Save stores only category IDs used by entries.

---

# 17. 162A Phase K — Founding Day

Source proposes `shelter founding day`.

Prefer:

- campaign start/founding day from CampaignCalendar/campaign metadata.

Persist only if the canonical campaign start is not otherwise available.

---

# 18. 162A Phase L — No Persisted Historical Timeline

The timeline is:

```text
entries sorted by day + stable tie-break
```

Do not store a second timeline array.

---

# 19. 162A Phase M — Stable Timeline Ordering

Order by:

```text
day
event timestamp/sequence if available
significance
entry_id
```

Use one canonical order.

---

# 20. 162A Phase N — Automatic Recording Interface

Create:

```text
IArchiveEventRecorder
```

or semantic-event subscription.

Input:

```text
SemanticEvent / HistoricalEventEnvelope
```

Output:

```text
zero or more ArchiveEntry projections
```

---

# 21. 162A Phase O — Event Projection Rules

Each supported event kind maps to:

- entry type,
- category,
- significance,
- participant extraction,
- title/body localization template,
- tags.

Data-driven where practical.

---

# 22. 162A Phase P — No Polling as Normal Path

Archive should not scan every system daily to infer what happened.

Use event delivery.

Migration/backfill may be separate.

---

# 23. 162A Phase Q — Canonical Entry Immutability

After creation, automatic entries may not be deleted by player.

Edits are limited to:

- display annotations,
- user-added commentary,
- corrected migration metadata where explicitly versioned.

No rewriting the historical source event.

---

# 24. 162A Phase R — Manual Reflection Model

Manual reflection is optional.

Fields:

```text
entry_id
day
title
body
custom_tags
category_id
significance
```

Clearly labeled as player-authored.

---

# 25. 162A Phase S — Manual Text Safety

Preserve player text.

Do not localize/translate it automatically unless user explicitly requests.

Limit only:

- maximum length,
- invalid control characters,
- storage constraints.

---

# 26. 162A Phase T — Manual Reflection Deletion Policy

Source says archive entries permanent/cannot be deleted.

Interpret carefully:

- canonical institutional entries: immutable/non-deletable,
- manual reflections: choose product policy explicitly.

Recommended:

```text
manual notes editable/deletable
```

because they are user-authored notes, not historical truth.

If source requires absolute permanence for manual entries too, document that as a deliberate UX decision.

---

# 27. 162A Phase U — Memorial Reference Model

Do not duplicate full memorial state.

Store:

```text
memorial_id
survivor_id
source_event_id
archive_entry_id
```

Resolve inscription/type/location from MemorialSystem when displaying, unless historical snapshotting is necessary.

---

# 28. 162A Phase V — Historical Snapshot vs Live Reference

For canonical facts that may later change presentation:

Store both:

```text
stable source reference
historical snapshot fields required for truth-at-time
```

Example:

- participant display name may change,
- faction name/localization may change.

Prefer stable IDs plus localized current display.

---

# 29. 162A Phase W — Localization

System-generated titles/descriptions use:

```text
template key + parameters
```

not hardcoded English strings in save.

Manual entries remain raw user text.

---

# 30. 162A Phase X — Tag Vocabulary

System tags should be stable IDs:

```text
governance
disaster
death
expedition
research
faction
community
survival
```

Manual custom tags stored separately.

---

# 31. 162A Phase Y — Archive Search Query

Define:

```text
text
category IDs
entry types
significance
participant IDs
tag IDs
day range
sort
```

---

# 32. 162A Phase Z — Search Index

Initial implementation can search normalized entry fields.

If corpus grows large, build ephemeral index.

Do not persist the index unless measured.

---

# 33. 162A Phase AA — Search Normalization

Support:

- case-insensitive matching,
- localization-aware text where practical,
- stable ID matching for participant/tag filters.

Do not use OCR-like fuzzy magic unnecessarily.

---

# 34. 162A Phase AB — Archive Save State

Persist:

```text
automatic entries
manual reflections
entry annotations if any
memorial references if not derivable
schema version
retention summaries
```

Do not persist:

```text
categories catalog
timeline
search index
resolved display names
```

---

# 35. 162A Phase AC — Old Save Compatibility

Missing archive state:

```text
valid
→ initialize empty institutional archive
```

Do not fabricate a full historical archive from unknown past events.

---

# 36. 162A Phase AD — Optional Backfill

For facts that are safely reconstructible from durable canonical history:

- existing memorial deaths,
- current major completion flags,
- founding date.

Backfill must:

- be deterministic,
- label entries `MigrationImported`,
- avoid pretending unknown event timing/details.

---

# 37. 162A Phase AE — Save Schema Versioning

Register archive state with explicit version.

Migration tests required.

---

# 38. 162A Phase AF — Retention Classes

Use Plan 55:

```text
LandmarkPermanent
RecentDetail
ManualReflection
DerivedSummary
```

Historic/major entries remain durable.

Minor automatic events may be summarized only if semantic meaning survives.

---

# 39. 162A Phase AG — Archive Size Budget

Define:

- entries/year,
- bytes/year,
- search latency,
- largest expected multi-generation archive.

No unbounded prose duplication.

---

# 40. 162A Phase AH — Semantic Events for Archive Itself

Possible:

```text
archive_entry_added
manual_reflection_added
memorial_reference_added
archive_milestone_reached
```

Avoid recursive recording:

```text
archive_entry_added
→ archive records archive_entry_added
```

Explicitly exclude archive-internal events from recording rules.

---

# 41. 162A Phase AI — Port Contract

Required source/consumer seams:

- semantic events,
- journal,
- memorial,
- consequence ledger,
- completion/epilogue,
- optional governance/disaster/colony integrations.

Required ports must distinguish:

```text
mandatory now
optional pending plan
```

No boot failure for genuinely unavailable future systems.

---

# 42. 162A Phase AJ — Diagnostics

Expose:

```text
ARCHIVE_ENTRIES_TOTAL
ARCHIVE_ENTRIES_AUTOMATIC
ARCHIVE_ENTRIES_MANUAL
ARCHIVE_LANDMARKS
ARCHIVE_DUPLICATE_EVENTS_IGNORED
ARCHIVE_UNRESOLVED_REFERENCES
ARCHIVE_SEARCH_INDEX_SIZE
```

---

# 43. 162A Tests

- deterministic entry ID,
- duplicate event idempotency,
- category load,
- six-core/ten-total category validation,
- manual reflection provenance,
- automatic entry immutability,
- timeline derivation,
- search normalization,
- old save,
- migration backfill,
- recursive-event prevention.

---

# 44. 162A Definition of Done

- [ ] ShelterArchiveSystem,
- [ ] typed entries/provenance,
- [ ] deterministic IDs,
- [ ] archive_categories.json,
- [ ] six core categories preserved,
- [ ] ten-category data requirement explicitly resolved,
- [ ] categories not persisted,
- [ ] event-driven recording,
- [ ] no normal polling,
- [ ] automatic entry immutability,
- [ ] optional manual reflections,
- [ ] memorial references,
- [ ] localized automatic text,
- [ ] tag/search schema,
- [ ] derived timeline,
- [ ] save/versioning,
- [ ] old-save/backfill policy,
- [ ] retention classes,
- [ ] size budget,
- [ ] ports,
- [ ] diagnostics.

---

# 45. Workstream 162B — Entries, Memorials, Timeline, Search & UI

## Goal

Implement meaningful automatic recording, memorial integration, timeline/search/browse surfaces, optional reflections, archive events/quests, and institutional-memory presentation without busywork.

---

# 46. 162B Phase A — Automatic Decision Recording

Record only significant decisions.

Examples:

- major policy enactment,
- governance vote outcome,
- diplomatic pact,
- refugee admission crisis,
- faction alliance/embargo,
- execution/expulsion/major moral decision.

Use cause IDs.

---

# 47. 162B Phase B — Governance Integration

If Plan 159 is present:

```text
governance_decision_committed
→ archive decision entry
```

If absent, keep adapter optional.

Do not infer policy history by scanning current policy state.

---

# 48. 162B Phase C — Diplomatic Decisions

Faction/diplomacy outcomes can create archive decisions/events when significance threshold met.

---

# 49. 162B Phase D — Disaster Recording

Plan 158 events can map:

```text
disaster warning?
contained?
resolved?
```

Avoid three nearly duplicate historical entries unless each is meaningful.

Recommended:

- one major resolved event entry,
- optional notable rescue/casualty entries.

---

# 50. 162B Phase E — Battle/Raid Recording

Shelter defense/faction conflict:

- significant attack,
- siege,
- major victory/defeat.

Use canonical combat/defense event.

---

# 51. 162B Phase F — Celebration Recording

Only shelter-wide celebration/milestone events.

Do not record every small morale tick.

---

# 52. 162B Phase G — Memorial Recording

On canonical survivor death:

```text
MemorialSystem creates/owns memorial
ShelterArchiveSystem records memorial reference
```

Do not create a second memorial record with separate inscription truth.

---

# 53. 162B Phase H — Memorial Type

Source types:

```text
wall_entry
grave
ceremony
legacy_project
```

If MemorialSystem already owns types, reuse its vocabulary.

If it lacks them, extend MemorialSystem—not ArchiveSystem.

---

# 54. 162B Phase I — Memorial Inscription

Player/auto inscription belongs to MemorialSystem.

Archive displays via reference.

---

# 55. 162B Phase J — Memorial Location

Location authority belongs to memorial/world/shelter system.

Archive stores reference only.

---

# 56. 162B Phase K — Memorial Anniversary

Calculate from:

```text
death day / memorial day
+ CampaignCalendar
```

Do not increment a separate anniversary counter.

---

# 57. 162B Phase L — Anniversary Event

Anniversary can:

- notify,
- offer ceremony,
- add journal/archive event if ceremony occurs.

Avoid automatic yearly morale penalty with no player context.

---

# 58. 162B Phase M — Memorial Morale Effect

Use canonical morale/relations/grief systems.

Archive never directly changes morale because a memorial exists.

---

# 59. 162B Phase N — Milestone Recording

Examples:

- shelter expansion,
- population threshold,
- first child/graduation,
- first major research,
- survival-year milestone,
- first alliance.

Use semantic producers.

---

# 60. 162B Phase O — Discovery Recording

Examples:

- expedition discovery,
- new location,
- research breakthrough,
- historical artifact.

Do not archive every routine loot pickup.

---

# 61. 162B Phase P — Achievement Recording

If Plan 149/achievement system is active:

```text
achievement unlocked
→ archive achievement entry
```

Archive does not evaluate achievements.

---

# 62. 162B Phase Q — Manual Reflections

UI flow:

```text
Add reflection
→ title
→ body
→ optional category
→ custom tags
→ significance
→ save
```

Keep optional.

---

# 63. 162B Phase R — Reflection Association

Allow linking a manual reflection to:

- existing archive entry,
- survivor,
- day,
- location.

This adds personal interpretation without rewriting canonical entry.

---

# 64. 162B Phase S — Archive Browse View

Core browse modes:

- latest,
- significant,
- category,
- participant,
- memorials.

---

# 65. 162B Phase T — Timeline View

Chronological display with:

- day/date,
- significance,
- category,
- participants,
- concise preview.

Key moments highlighted.

---

# 66. 162B Phase U — Timeline Density

For long campaigns:

- collapse minor events,
- group by month/year,
- allow zoom/filter.

Do not render thousands of rows at once.

---

# 67. 162B Phase V — Search

Search by:

- keyword,
- tag,
- participant,
- category,
- significance,
- date range.

---

# 68. 162B Phase W — Sort

Support:

```text
date ascending/descending
significance
category
```

Stable secondary order.

---

# 69. 162B Phase X — Search Result Preview

Show:

- title,
- day,
- category,
- participants,
- snippet.

No duplicate full-entry rendering.

---

# 70. 162B Phase Y — Archive Room

Source proposes a physical archive room.

Treat as integration target, not mandatory duplicate UI access.

If Plan 156 shelter expansion/building supports rooms:

```text
archive room can unlock/boost archive interactions
```

If not:

```text
archive panel remains accessible
```

Do not make history inaccessible because a physical room plan is absent.

---

# 71. 162B Phase Z — Archive Room Effects

Possible concrete benefits:

- better search access,
- exhibition,
- preservation/knowledge bonuses,
- memorial display.

Avoid opaque “archive quality +20”.

---

# 72. 162B Phase AA — Archive Events

Source examples:

```text
The Chronicle
The Memorial
The Anniversary
The Discovery
The Legacy
The Exhibition
The History
```

Use canonical event/quest systems.

---

# 73. 162B Phase AB — “The Chronicle”

Trigger when a major/historic event is recorded.

Do not record another redundant archive entry unless the event itself is distinct.

---

# 74. 162B Phase AC — “The Memorial”

Triggered by canonical memorial creation.

---

# 75. 162B Phase AD — “The Anniversary”

Annual remembrance opportunity.

---

# 76. 162B Phase AE — “The Discovery”

Historical artifact can create both:

- discovery gameplay event,
- archive entry.

Archive does not own artifact inventory.

---

# 77. 162B Phase AF — “The Legacy”

Milestone around cumulative shelter history.

Could unlock epilogue/legacy content.

---

# 78. 162B Phase AG — “The Exhibition”

If visitors/colony diplomacy systems exist:

- display archive,
- affect visitor/faction reaction.

No archive-specific diplomacy score.

---

# 79. 162B Phase AH — “The History”

Compilation milestone:

- creates export/book/history artifact if content system supports it.

---

# 80. 162B Phase AI — Quest Hooks

Source:

```text
The Historian
The Memorial
The Anniversary
The Archive
The Legacy
The Exhibition
The Discovery
```

Use canonical quest runtime.

---

# 81. 162B Phase AJ — “The Historian”

Potential goal:

- classify/import archive,
- compile history,
- identify missing artifact.

No manual busywork requirement for normal archive functionality.

---

# 82. 162B Phase AK — “The Archive”

Physical archive-room quest only if building system supports it.

---

# 83. 162B Phase AL — Journal Integration

Personal journal and institutional archive remain distinct.

Possible links:

```text
journal entry references archive event
archive entry links related journal excerpts
```

Do not copy all journal prose into archive automatically.

---

# 84. 162B Phase AM — Archive Journal / Notifications

Source asks for automatic log of archive additions.

Prefer:

- lightweight briefing/toast,
- journal cross-link only for notable/historic entries.

Do not create recursive duplicated logs for every archive insertion.

---

# 85. 162B Phase AN — Epilogue Export Model

Create structured:

```text
ArchiveLegacyProjection
```

Fields:

- major decisions,
- landmark disasters,
- memorial highlights,
- discoveries,
- achievements,
- major milestones.

Epilogue consumes data, not rendered prose.

---

# 86. 162B Phase AO — Legacy Export

Cross-campaign legacy receives only approved summary/history references.

Do not copy entire archive into meta profile unless explicitly designed.

---

# 87. 162B Phase AP — Colony History

If Plan 160 exists:

- colony founded,
- colony lost,
- colony alliance,
- colony milestone

can become archive events.

Archive does not own colony state.

---

# 88. 162B Phase AQ — Governance History

Decision record uses governance event IDs.

No scanning current policy list and pretending enactment day.

---

# 89. 162B Phase AR — Archive Tutorial

First automatic notable entry:

Explain:

- the archive records important shelter history automatically,
- manual reflections are optional,
- search/timeline exist.

Keep brief.

---

# 90. 162B Phase AS — Tooltips

Entry tooltip/details show:

- significance,
- category,
- day,
- participants,
- source context,
- linked memorial/decision/discovery.

---

# 91. 162B Phase AT — Accessibility

Archive panel:

- keyboard navigation,
- controller if supported,
- text labels,
- scalable text,
- search focus order,
- non-color significance indicators.

---

# 92. 162B Phase AU — Localization

Automatic entries/categories/events/quests use localization keys.

Manual reflections stay user-authored.

---

# 93. 162B Phase AV — 10-Category Coverage Matrix

Generate:

| Category | Entry types | Producers | UI filter | Runtime observed | Retention class |
|---|---|---|---|---:|---|

All 10 rows need disposition.

---

# 94. 162B Phase AW — Automatic Entry Coverage Matrix

Map each producer:

```text
event kind
→ archive rule
→ category
→ significance
→ template
```

---

# 95. 162B Phase AX — Content Utilization

Run representative campaign.

Report:

```text
archive rules eligible
entries created
categories used
historic entries
memorial refs
manual entries
unused categories
```

---

# 96. 162B Phase AY — Busywork Guard

Measure:

- manual actions required to keep archive useful,
- notifications/day,
- archive-related modal count.

Target:

```text
automatic usefulness
manual expression optional
```

---

# 97. 162B Definition of Done

- [ ] automatic decision recording,
- [ ] disaster/battle/event recording,
- [ ] memorial references,
- [ ] anniversaries,
- [ ] milestone/discovery/achievement recording,
- [ ] manual reflections,
- [ ] browse view,
- [ ] timeline,
- [ ] search/filter/sort,
- [ ] archive room optional integration,
- [ ] 7 archive events,
- [ ] 7 quest hooks,
- [ ] journal cross-links,
- [ ] epilogue/legacy projection,
- [ ] governance/disaster/colony adapters,
- [ ] tutorial/tooltips,
- [ ] accessibility,
- [ ] localization,
- [ ] 10-category coverage,
- [ ] utilization report,
- [ ] busywork guard.

---

# 98. Workstream 162C — Cross-System Integration, Save/CI, Retention, and Historical Closure

## Goal

Prove the archive records canonical history exactly once, survives long campaigns, remains searchable, never mutates underlying gameplay, and feeds completion/legacy systems through stable structured projections.

---

# 99. 162C Phase A — JournalSystem Integration

Journal and archive relationship:

```text
JournalSystem
→ personal perspective

ShelterArchiveSystem
→ institutional history
```

Provide cross-links, not duplication.

---

# 100. 162C Phase B — MemorialSystem Integration

On memorial/death:

- archive adds reference entry,
- MemorialSystem remains inscription/type/location authority.

---

# 101. 162C Phase C — CampaignConsequenceLedger Integration

Use ledger events/flags as evidence.

Do not mirror the entire ledger.

Only major outcomes become entries.

---

# 102. 162C Phase D — GovernanceSystem Integration

If Plan 159 exists:

- committed decisions generate archive entries.

If absent:

- adapter compiles but remains optional/inactive.

---

# 103. 162C Phase E — DisasterResponseSystem Integration

Plan 158:

- disaster resolution generates archive event,
- landmark rescue/casualty may generate additional entry.

Avoid logging every effect wave.

---

# 104. 162C Phase F — ColonySystem Integration

Plan 160:

- colony milestones become archive entries.

No duplicate colony ledger.

---

# 105. 162C Phase G — Achievement Integration

Archive listens to actual achievement unlock.

No re-evaluation.

---

# 106. 162C Phase H — Discovery/Research Integration

Use real discovery/research event.

No polling catalogs.

---

# 107. 162C Phase I — Calendar Integration

All dates come from `CampaignCalendar`.

Anniversary calculation uses canonical calendar.

---

# 108. 162C Phase J — Completion/Epilogue Integration

Structured projection feeds:

- chronicle,
- ending summary,
- long-form shelter story.

Do not parse localized strings for logic.

---

# 109. 162C Phase K — Save/Load Round Trip

Test:

- automatic entries,
- manual reflections,
- memorial refs,
- retention summaries.

Reload preserves:

- IDs,
- order,
- provenance,
- tags,
- links.

---

# 110. 162C Phase L — Duplicate Event Delivery

Deliver same canonical event twice.

Expected:

```text
one archive entry
```

with duplicate metric increment.

---

# 111. 162C Phase M — Event Replay

Rebuild/replay semantic events if architecture supports it.

Archive remains idempotent.

---

# 112. 162C Phase N — Old Save Empty Archive

Old save:

```text
archive empty
```

is valid.

No failure because historical data is absent.

---

# 113. 162C Phase O — Safe Migration Backfill

If configured:

- memorial deaths can backfill,
- founding milestone can backfill,
- durable major flags can backfill.

Never fabricate unknown participants/text/timing.

---

# 114. 162C Phase P — No Archive Edge Case

Zero entries:

- UI empty state works,
- search returns none,
- epilogue falls back to other completion data.

---

# 115. 162C Phase Q — Extensive Archive Edge Case

Create thousands of entries.

Assert:

- search latency bounded,
- timeline virtualization/paging,
- save/load budget,
- no UI freeze.

---

# 116. 162C Phase R — Search Correctness

Test:

- keyword,
- participant,
- tag,
- category,
- significance,
- day range,
- combined filters,
- stable sort.

---

# 117. 162C Phase S — Search Localization

Automatic localized strings may differ by locale.

Participant/tag/category search should still work by IDs.

Text search searches current localized rendering where applicable.

---

# 118. 162C Phase T — Manual Reflection Integrity

Save/load:

- exact text,
- tags,
- category,
- day,
- provenance.

No automatic mutation.

---

# 119. 162C Phase U — Canonical Entry Immutability Test

Attempt normal delete/update.

Expected:

```text
blocked
```

except permitted annotation layer.

---

# 120. 162C Phase V — Source Deletion/Retirement

If a source content item is removed in a future version:

- archive entry remains readable where possible,
- stable historical snapshot/template fallback exists.

Do not crash on missing current content.

---

# 121. 162C Phase W — Participant Death/Rename

Archive references survivor ID.

Display:

- historical/current known name as appropriate.

No broken entry after death.

---

# 122. 162C Phase X — Retention Policy

Plan 55.

Recommended:

```text
Historic/Major automatic entries → permanent
Memorial landmarks → permanent
Manual reflections → permanent unless user deletion policy permits
Notable → long-lived / summary after very long horizon
Minor → bounded detail / roll-up
```

Never silently delete source-defined permanent institutional landmarks.

---

# 123. 162C Phase Y — Summary Roll-Up

If minor events are summarized:

```text
month/year/category summary
```

must preserve counts/key references without contradicting history.

---

# 124. 162C Phase Z — Archive Size Ceiling

Measure:

```text
10-year
100-year
400-year
```

archive bytes and entry counts.

Align with Plan 55.

---

# 125. 162C Phase AA — Search Performance Budget

Define p50/p95 query budget for largest fixture.

Use realistic query set.

---

# 126. 162C Phase AB — Timeline Rendering Budget

Virtualize/paginate.

Do not instantiate every row.

---

# 127. 162C Phase AC — `--shelter-archive-selftest`

Required scenarios:

1. automatic major decision entry,
2. disaster entry,
3. memorial reference,
4. milestone entry,
5. manual reflection,
6. duplicate-event idempotency,
7. search/filter,
8. timeline ordering,
9. old save,
10. backfill,
11. extensive archive,
12. completion projection.

---

# 128. 162C Phase AD — Catalog Integrity

Validate:

- 10 categories,
- unique IDs,
- entry type refs,
- display order,
- localization keys,
- retention classes.

---

# 129. 162C Phase AE — Reference Integrity

Validate current refs where expected:

- survivor IDs,
- memorial IDs,
- faction IDs,
- location IDs,
- source event IDs.

Historical missing-reference fallback should be explicit.

---

# 130. 162C Phase AF — Deliberate Failure Proof

Break:

- duplicate category ID,
- missing localization key,
- duplicate source event,
- invalid memorial reference,
- stale generated category docs.

Assert relevant gate/selftest fails.

---

# 131. 162C Phase AG — Same-Seed Recording

Because recording is event-pure:

```text
same canonical event stream
→ same archive digest
```

---

# 132. 162C Phase AH — No Gameplay Mutation Proof

Run archive disabled vs enabled with same event stream.

Gameplay state digest must match except archive-owned state/UI notifications.

This is a critical non-interference test.

---

# 133. 162C Phase AI — Archive Event Recursion Guard

Ensure archive-generated notifications/events do not recursively create new archive entries.

---

# 134. 162C Phase AJ — Annual Memorial Soak

Run multi-year campaign with deaths.

Verify:

- anniversary reminders occur once/year,
- ceremonies optional,
- no duplicate morale/history application.

---

# 135. 162C Phase AK — Long Campaign History Test

Run or load mature campaign.

Check whether timeline tells a coherent story:

- founding,
- first crisis,
- major decision,
- deaths,
- discoveries,
- milestones.

---

# 136. 162C Phase AL — Narrative Quality Review

Human review:

```text
Are important events captured?
Is trivial noise excluded?
Do titles/descriptions feel institutional rather than personal-journal duplicates?
Does the archive help reconstruct causality?
```

Not a deterministic CI judgement.

---

# 137. 162C Phase AM — Archive/Epilogue Parity

Select major entries.

Verify completion/epilogue projection references the same canonical archive facts.

No contradictory dates/participants.

---

# 138. 162C Phase AN — Legacy Export Boundaries

Cross-campaign export only:

- approved summaries,
- landmark references,
- shelter identity facts.

No huge raw archive dump.

---

# 139. 162C Phase AO — Accessibility

Search/timeline/browse:

- keyboard,
- controller if supported,
- semantic labels,
- scalable text,
- no color-only significance,
- search-results focus restoration.

---

# 140. 162C Phase AP — Headless Behavior

Automatic recording and anniversaries work without UI.

UI does not own tick.

---

# 141. 162C Phase AQ — Documentation

Create:

```text
docs/systems/SHELTER_ARCHIVE.md
```

Include:

- authority boundaries,
- entry/provenance model,
- categories,
- event projection rules,
- memorial references,
- timeline/search,
- retention,
- save/migration behavior,
- adding new event producers.

---

# 142. 162C Definition of Done

- [ ] Journal integration,
- [ ] Memorial integration,
- [ ] ConsequenceLedger integration,
- [ ] optional Governance integration,
- [ ] DisasterResponse integration,
- [ ] optional Colony integration,
- [ ] Achievement/discovery integration,
- [ ] CampaignCalendar integration,
- [ ] completion/epilogue projection,
- [ ] save/load,
- [ ] duplicate-event idempotency,
- [ ] old-save empty archive,
- [ ] safe backfill,
- [ ] empty-archive UI,
- [ ] extensive-archive stress,
- [ ] search correctness,
- [ ] manual reflection integrity,
- [ ] canonical entry immutability,
- [ ] missing-source fallback,
- [ ] Plan 55 retention,
- [ ] archive size ceiling,
- [ ] search/render budget,
- [ ] selftest,
- [ ] reference integrity,
- [ ] deliberate failure proof,
- [ ] same-event-stream archive digest,
- [ ] gameplay non-interference,
- [ ] recursion guard,
- [ ] anniversary soak,
- [ ] mature-history review,
- [ ] epilogue parity,
- [ ] accessibility,
- [ ] headless,
- [ ] docs.

---

# 143. Integrated Archive Pipeline

```text
canonical event occurs
        │
        ▼
semantic event / durable outcome
        │
        ▼
Archive projection rule
        │
        ├─ qualifies? no → ignore
        │
        └─ yes
            │
            ▼
    deterministic entry ID
            │
            ▼
       archive entry
            │
     ┌──────┼─────────┐
     ▼      ▼         ▼
 timeline  search   memorial/source links
     │
     ▼
 player reflection / epilogue / legacy
```

---

# 144. Archive Authority Contract

Archive owns:

- institutional historical records,
- manual reflections,
- archive-specific tags/provenance,
- historical presentation metadata.

It does not own source gameplay facts.

---

# 145. Journal Contract

Journal:

```text
personal perspective
```

Archive:

```text
institutional history
```

Cross-link, do not duplicate wholesale.

---

# 146. Memorial Contract

MemorialSystem owns:

- death memorial identity,
- inscription,
- memorial type/location.

Archive owns historical reference.

---

# 147. Consequence Contract

CampaignConsequenceLedger owns flags/counters.

Archive records only meaningful outcomes.

---

# 148. Governance Contract

Governance owns policy/decision state.

Archive records committed decisions.

---

# 149. Disaster Contract

DisasterResponse owns disaster lifecycle.

Archive records landmark outcome.

---

# 150. Colony Contract

ColonySystem owns colony state.

Archive records colony milestones.

---

# 151. Calendar Contract

CampaignCalendar owns dates.

Archive stores event day/reference.

---

# 152. Timeline Contract

Timeline is a query over entries.

Never a separate mutable store.

---

# 153. Search Contract

Search index is derived/rebuildable.

No index is authoritative.

---

# 154. Category Contract

Category catalog is static data.

Save stores category IDs only.

---

# 155. Provenance Contract

Every entry clearly identifies:

```text
system-generated
player-authored
migration-imported
legacy-imported
```

---

# 156. Immutability Contract

System-generated institutional entries cannot be player-deleted or rewritten.

Manual reflection edit/delete policy is explicit and separate.

---

# 157. Localization Contract

System text uses template keys/parameters.

Player text remains authored text.

---

# 158. Significance Contract

Significance controls:

- default visibility,
- highlighting,
- retention policy,
- export priority.

It does not alter gameplay by itself.

---

# 159. Retention Contract

Historical landmarks remain durable.

Low-value detail may roll up only through explicit Plan 55 policy.

---

# 160. Completion/Epilogue Contract

Epilogue consumes structured archive facts.

No logic parses localized narrative text.

---

# 161. Non-Interference Contract

Archive enabled/disabled must not change campaign simulation outcomes.

---

# 162. Content Acceptance Contract

Archive categories/projection rules progress through:

```text
AUTHORED
→ LOADS
→ EVENT_ELIGIBLE
→ ENTRY_PRODUCED
→ SEARCHABLE
→ PLAYER_VISIBLE
→ LEGACY/EPILOGUE_CONSUMED
```

---

# 163. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| archive duplicates journal | Medium | High | personal vs institutional contract |
| archive duplicates memorial state | Medium | High | references only |
| timeline drifts from entries | Medium | High | derived timeline |
| 6-vs-10 category mismatch creates dead data | High | Medium | explicit reconciliation/coverage |
| automatic recording becomes noisy | High | Medium | significance thresholds |
| manual entries become required busywork | Medium | Medium | automatic-first design |
| localized prose in save becomes stale | Medium | Medium | template keys + parameters |
| duplicate event delivery creates duplicate history | Medium | High | deterministic entry IDs |
| archive grows unbounded | Medium | High | retention + size budget |
| historical source ref disappears | Medium | Medium | stable snapshots/fallbacks |
| archive changes gameplay via morale directly | Medium | High | non-interference/owner routing |
| recursive archive notifications create infinite entries | Low–Med | High | explicit event exclusion |

---

# 164. Commit Strategy

## 162A — Foundation

### C2[34].1 — baseline + archive ADR

### C2[34].2 — entry/provenance/search DTOs

### C2[34].3 — archive_categories.json + 6/10 reconciliation

### C2[34].4 — deterministic event projection/idempotency

### C2[34].5 — manual reflection model

### C2[34].6 — memorial reference model

### C2[34].7 — save/old-save/backfill/retention

### C2[34].8 — ports/events/diagnostics

### Gate: 162A complete

---

## 162B — History / Memorials / UI

### C2[34].9 — decision/event producers

### C2[34].10 — memorial/anniversary integration

### C2[34].11 — milestone/discovery/achievement producers

### C2[34].12 — timeline/search/filter/sort

### C2[34].13 — browse UI/manual reflection

### C2[34].14 — archive events/quests

### C2[34].15 — journal/legacy/epilogue projections

### C2[34].16 — tutorial/tooltips/accessibility/localization

### C2[34].17 — content-utilization/busywork report

### Gate: 162B complete

---

## 162C — Closure

### C2[34].18 — Journal/Memorial/Consequence integrations

### C2[34].19 — Governance/Disaster/Colony adapters

### C2[34].20 — save-load/idempotency matrix

### C2[34].21 — empty/extensive archive edge cases

### C2[34].22 — retention/performance budgets

### C2[34].23 — selftest + deliberate failure proof

### C2[34].24 — same-event-stream/non-interference tests

### C2[34].25 — memorial-anniversary/mature-history soak

### C2[34].26 — epilogue/legacy parity

### C2[34].27 — docs/playtest/release closure

### Gate: 162C complete

---

# 165. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-archive-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
archive-category/projection content-utilization report
old-save fixture load
same-event-stream archive digest
archive gameplay non-interference test
large-archive search/timeline performance test
archive-panel snapshot/accessibility gate
```

---

# 166. Flagship Definition of Done

## 162A — Foundation

- [ ] ShelterArchiveSystem,
- [ ] typed archive entries,
- [ ] provenance,
- [ ] deterministic IDs,
- [ ] six source core categories,
- [ ] ten-category data requirement reconciled,
- [ ] static category authority,
- [ ] event-driven recording,
- [ ] automatic entry immutability,
- [ ] optional manual reflections,
- [ ] memorial references,
- [ ] localized automatic text,
- [ ] search query model,
- [ ] derived timeline,
- [ ] save/versioning,
- [ ] old-save/backfill policy,
- [ ] retention/size budget,
- [ ] ports,
- [ ] diagnostics.

## 162B — Entries / Memorials / UI

- [ ] decisions recorded,
- [ ] significant events recorded,
- [ ] memorials referenced,
- [ ] anniversaries,
- [ ] milestones,
- [ ] discoveries,
- [ ] achievements,
- [ ] manual reflections,
- [ ] browse,
- [ ] timeline,
- [ ] search/filter/sort,
- [ ] optional archive-room integration,
- [ ] archive events,
- [ ] quest hooks,
- [ ] Journal cross-links,
- [ ] epilogue projection,
- [ ] legacy projection,
- [ ] tutorial/tooltips,
- [ ] accessibility,
- [ ] localization,
- [ ] category coverage,
- [ ] busywork guard.

## 162C — Integration / Closure

- [ ] JournalSystem,
- [ ] MemorialSystem,
- [ ] CampaignConsequenceLedger,
- [ ] Governance adapter,
- [ ] DisasterResponse adapter,
- [ ] Colony adapter,
- [ ] Calendar,
- [ ] completion/epilogue,
- [ ] save/load,
- [ ] duplicate event idempotency,
- [ ] old-save empty archive,
- [ ] safe backfill,
- [ ] no-archive edge case,
- [ ] extensive-archive stress,
- [ ] search correctness,
- [ ] manual reflection integrity,
- [ ] canonical entry immutability,
- [ ] historical missing-ref fallback,
- [ ] retention,
- [ ] archive size ceiling,
- [ ] search/render budgets,
- [ ] selftest,
- [ ] reference integrity,
- [ ] failure proof,
- [ ] same-stream digest,
- [ ] non-interference,
- [ ] recursion guard,
- [ ] anniversary soak,
- [ ] mature-history review,
- [ ] epilogue parity,
- [ ] headless,
- [ ] docs.

## Global

- [ ] no duplicate journal authority,
- [ ] no duplicate memorial authority,
- [ ] no duplicate consequence/governance/colony truth,
- [ ] no separately persisted timeline,
- [ ] no persisted category catalog,
- [ ] no archive-driven gameplay mutation,
- [ ] no required manual busywork,
- [ ] no unbounded archive growth,
- [ ] full verification green.

---

# 167. Closure Report Template

```markdown
## C2[34] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Journal authority:
- Memorial authority:
- Consequence ledger:
- Calendar:
- Existing historical producers:
- Existing archive references:

### 162A — Foundation
- Archive entry schema:
- Provenance:
- Core categories:
- Total categories:
- 6/10 reconciliation:
- Automatic projection rules:
- Manual reflection:
- Memorial references:
- Save schema:
- Old-save policy:
- Backfilled entries:
- Retention classes:
- Missing ports:
- Result:

### 162B — Entries / UI
- Decision entries:
- Event entries:
- Memorial entries:
- Milestones:
- Discoveries:
- Achievements:
- Manual reflections:
- Timeline:
- Search:
- Archive events:
- Quests:
- Journal links:
- Epilogue projection:
- Legacy projection:
- Unused categories:
- Busywork metrics:
- Result:

### 162C — Closure
- Journal:
- Memorial:
- Consequence ledger:
- Governance:
- Disaster:
- Colony:
- Calendar:
- Save/load:
- Duplicate event failures:
- Empty archive:
- Large archive:
- Search p95:
- Timeline render:
- Archive bytes:
- Non-interference:
- Anniversary soak:
- Mature-history review:
- Epilogue parity:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Shelter archive selftest:
- Port contract:
- Content utilization:
- Old-save fixtures:
- Same-stream digest:
- Non-interference:
- Large-archive performance:
- Verify fast:

### Final Metrics
- ARCHIVE_CATEGORIES:
- ARCHIVE_ENTRIES_TOTAL:
- ARCHIVE_AUTOMATIC:
- ARCHIVE_MANUAL:
- ARCHIVE_HISTORIC:
- ARCHIVE_MEMORIAL_REFS:
- DUPLICATE_EVENTS_IGNORED:
- UNRESOLVED_REFERENCES:
- ARCHIVE_BYTES:
- SEARCH_P95_MS:
- TIMELINE_RENDER_P95_MS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Archive content:
- Governance producer:
- Colony producer:
- Legacy export:
- Physical archive room:
- UI:
```

---

# 168. Final Execution Directive

Execute Plan 162 as an **institutional-memory projection over the game’s existing event, journal, memorial, consequence, calendar, governance, disaster, colony, and completion authorities**.

The critical sequence is:

```text
define stable archive categories and provenance
→ subscribe to canonical semantic events
→ project only significant events into deterministic immutable archive entries
→ reference memorials rather than duplicating them
→ allow optional player reflections as clearly separate authored notes
→ derive timeline/search/indexes
→ feed structured history into epilogue/legacy
→ apply retention without erasing landmark institutional memory
→ prove archive non-interference with gameplay
```

Do not create a second personal journal.

Do not copy MemorialSystem into the archive.

Do not persist a second timeline.

Do not store the category catalog in every save.

Do not let automatic history depend on UI being open.

Do not turn manual note-taking into required maintenance.

The strongest authority rule is:

> **The archive records what the canonical game systems say happened; it never becomes the system that decides what happened.**

The strongest historical rule is:

> **Automatic institutional history is immutable and source-attributed, while optional player reflections remain visibly separate personal interpretation.**

The strongest long-campaign rule is:

> **A 400-year archive must preserve landmark shelter memory without requiring every minor event to remain as full raw detail forever.**

The flagship acceptance scenario is:

> **Run a seeded campaign sequence containing a major governance decision, one disaster, a survivor death, an expedition discovery, and a shelter milestone. Each canonical source event must generate exactly one deterministic archive entry with the correct day, category, significance, participants, and source reference. Create one manual reflection linked to the disaster. Save/load, replay one duplicate source event, and verify no duplicate canonical entry appears. Search by survivor, category, tag, and date; open the memorial through its canonical MemorialSystem reference; then feed the structured major-history projection into the epilogue. Finally compare gameplay state with the archive system disabled and enabled: every non-archive simulation digest must remain identical.**
