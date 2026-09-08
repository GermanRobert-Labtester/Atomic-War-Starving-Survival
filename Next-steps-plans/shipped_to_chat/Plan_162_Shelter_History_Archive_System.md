# Plan 162 — Shelter History & Archive System

## Goal

Create a shelter history and archive system where significant events, decisions, and milestones are automatically recorded, preserved, and accessible as the shelter's collective memory. Currently the journal system records personal entries but there is no institutional archive — no shelter history, no record of major decisions, no memorial of the fallen, no institutional knowledge that persists across generations. This plan adds a historical layer that makes the shelter's journey meaningful and remembered.

## Why

**Repository evidence:** `JournalSystem` (referenced in AGENTS.md H11) records personal journal entries. `CampaignConsequenceLedger` tracks flags and counters. `MemorialSystem` records individual deaths. But there is no institutional archive — no shelter history book, no record of major decisions, no memorial wall, no historical timeline. The shelter's collective experience is lost. Plan 140 (legacy) adds cross-campaign inheritance but not in-campaign historical record.

**What is missing:** The shelter has no memory of its own journey. Major decisions (admitting refugees, fighting raids, establishing alliances) are not recorded. Survivor deaths are memorialized individually but not as part of shelter history. There is no "shelter story" — no narrative of what this community has been through together.

**Why existing plans don't solve it:** Plan 17 (environmental storytelling) adds lore documents but not shelter-specific history. Plan 51 (environmental storytelling documents) adds narrative content but not institutional archive. Plan 140 (legacy) adds cross-campaign persistence but not in-campaign historical record. No plan addresses shelter history or archival systems.

**Player value:** Creates sense of legacy (the shelter's story matters), provides reflection (look back on the journey), generates emotional depth (memorialize the fallen), and makes the shelter feel like a community with shared history.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Journal/` — journal system
- `Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs` — consequence tracking
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` — death memorial
- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` — day tracking
- NEW: `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs`
- NEW: `Assets/StreamingAssets/Data/archive_categories.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterArchiveSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `ArchiveEntry` DTO: `entryId`, `entryType` (decision/event/memorial/milestone/discovery/achievement), `title`, `description`, `day`, `participants` (list of survivor IDs), `tags` (list), `significance` (minor/notable/major/historic)
3. Define `ArchiveCategory` DTO: `categoryId`, `categoryName`, `description`, `entryTypes` (list), `displayOrder`
4. Define `MemorialEntry` DTO: `memorialId`, `survivorId`, `memorialType` (wall_entry/grave/ceremony/legacy), `inscription`, `day`, `location`
5. Define `ShelterArchiveState` DTO: list of archive entries, list of memorial entries, list of categories, shelter founding day, historical timeline
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define archive categories:
   - **Decisions**: major governance choices, policy enactments, diplomatic actions
   - **Events**: disasters, battles, celebrations, crises, breakthroughs
   - **Memorials**: survivor deaths, memorial services, legacy tributes
   - **Milestones**: shelter expansion, population milestones, achievement unlocks
   - **Discoveries**: expedition findings, research breakthroughs, new locations
   - **Achievements**: survivor accomplishments, shelter awards, community honors
8. Define archive mechanics:
   - Significant events automatically recorded
   - Player can manually add entries (personal reflections)
   - Entries tagged and categorized for retrieval
   - Entries have significance levels (minor to historic)
   - Archive searchable and browsable
9. Define memorial mechanics:
   - Survivor deaths recorded in memorial
   - Memorial types: wall entry, grave marker, ceremony, legacy project
   - Memorial inscriptions (player-written or auto-generated)
   - Memorial anniversaries (annual remembrance)
   - Memorial affects shelter morale (remembering the fallen)
10. Define historical timeline:
    - Chronological record of all significant events
    - Timeline view shows shelter journey
    - Key moments highlighted
    - Timeline can be exported (for legacy, epilogue)
11. Add deterministic recording: archive entries are pure functions of game events
12. Wire into `GameBootstrap`: `SetupShelterArchive`, `TickArchive`, `SaveShelterArchive`
13. Create `ArchiveCategoryCatalogLoader` for category definitions
14. Implement archive UI: browse, search, filter entries
15. Create archive journal: automatic log of archive additions

## Main Task 2 — Implementation / Entries / Memorials / Timeline / UI

1. Implement automatic event recording:
   - Major decisions recorded (governance, diplomacy, policy)
   - Significant events recorded (disasters, battles, celebrations)
   - Survivor deaths recorded (memorial entries)
   - Milestones recorded (expansion, population, achievements)
   - Discoveries recorded (expeditions, research, locations)
   - Achievements recorded (survivor accomplishments)
2. Implement manual entry creation:
   - Player can add personal reflections
   - Entries tagged with custom tags
   - Entries categorized manually
   - Significance assigned by player
3. Implement memorial system:
   - Survivor death triggers memorial creation
   - Player chooses memorial type (wall, grave, ceremony, legacy)
   - Memorial inscription written (player or auto-generated)
   - Memorial location assigned (shelter room, graveyard, monument)
   - Memorial anniversary recorded (annual remembrance)
4. Implement memorial anniversaries:
   - Annual reminder of deaths
   - Memorial ceremony option (shelter event)
   - Memorial affects morale (remembering fallen)
   - Memorial legacy (survivor's contributions remembered)
5. Implement historical timeline:
   - Chronological view of all entries
   - Filter by category, significance, date range
   - Key moments highlighted
   - Timeline exportable (for epilogue, legacy)
6. Implement archive search:
   - Search by keyword, tag, participant, date
   - Filter by category, significance
   - Sort by date, significance, category
   - Search results show entry previews
7. Implement archive display:
   - Archive room in shelter (physical location)
   - Archive accessible via UI panel
   - Entries displayed with title, description, tags
   - Memorials displayed with inscription, date, location
8. Create archive events:
   - "The Chronicle" — major event recorded in archive
   - "The Memorial" — survivor memorialized
   - "The Anniversary" — annual remembrance
   - "The Discovery" — historical artifact found
   - "The Legacy" — shelter milestone reached
   - "The Exhibition" — archive displayed for visitors
   - "The History" — shelter history compiled
9. Add archive quest hooks:
   - "The Historian" — compile shelter history
   - "The Memorial" — create memorial for fallen survivor
   - "The Anniversary" — organize remembrance ceremony
   - "The Archive" — build dedicated archive room
   - "The Legacy" — ensure shelter story preserved
   - "The Exhibition" — display archive for visitors
   - "The Discovery" — find historical artifact
10. Implement archive integration:
    - Archive integrates with journal (personal + institutional)
    - Archive integrates with memorial (individual + collective)
    - Archive integrates with legacy (cross-campaign history)
    - Archive integrates with epilogue (shelter story in ending)
    - Archive integrates with governance (decision record)
11. Add UI: archive panel with browse, search, timeline views
12. Create archive journal: automatic log of archive additions
13. Implement archive tutorial: first archive entry explains system
14. Add archive tooltips: hover over entry shows details
15. Create 10 archive categories in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `JournalSystem`: archive complements personal journal
2. Connect to `MemorialSystem`: memorial entries integrate
3. Integrate with `CampaignConsequenceLedger`: consequences recorded
4. Connect to `GovernanceSystem` (Plan 159): decisions archived
5. Wire into `DisasterResponseSystem` (Plan 158): disasters recorded
6. Connect to `ColonySystem` (Plan 160): colony history archived
7. Implement old-save compatibility: existing saves get empty archive state
8. Add deterministic recording: archive entries are pure functions of events
9. Create exploit prevention: archive entries are permanent, can't be deleted
10. Add tests: entry creation, memorial system, timeline, search, save round-trip
11. Verify catalog integrity: all category/survivor IDs resolve
12. Test edge cases: no archive (empty history), extensive archive (rich history)
13. Verify headless behavior: archive processes correctly without UI
14. Add data-integrity-selftest: archive categories validate against catalogs
15. Create `--shelter-archive-selftest` verb for CI validation

## State / System Interaction Model

```text
Shelter history and archive
├─ Automatic event recording
│  ├─ Decisions (governance, diplomacy, policy)
│  ├─ Events (disasters, battles, celebrations)
│  ├─ Memorials (survivor deaths)
│  ├─ Milestones (expansion, population, achievements)
│  ├─ Discoveries (expeditions, research, locations)
│  └─ Achievements (survivor accomplishments)
├─ Manual entry creation
│  ├─ Player adds personal reflections
│  ├─ Entries tagged and categorized
│  └─ Significance assigned
├─ Memorial system
│  ├─ Survivor death triggers memorial
│  ├─ Memorial type chosen (wall, grave, ceremony, legacy)
│  ├─ Inscription written
│  ├─ Anniversary recorded
│  └─ Memorial affects morale
├─ Historical timeline
│  ├─ Chronological view of entries
│  ├─ Filter by category, significance, date
│  ├─ Key moments highlighted
│  └─ Timeline exportable
├─ Archive search and display
│  ├─ Search by keyword, tag, participant
│  ├─ Filter and sort
│  ├─ Archive room in shelter
│  └─ UI panel access
└─ Integration
   ├─ Journal (personal + institutional)
   ├─ Memorial (individual + collective)
   ├─ Legacy (cross-campaign history)
   ├─ Epilogue (shelter story in ending)
   └─ Governance (decision record)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-archive-selftest
```

## Risk

**LOW** — Archive system is straightforward with clear inputs (game events) and outputs (recorded entries). Risk of archive feeling like busywork rather than meaningful history. Mitigation: make automatic recording default, manual entries optional, show archive significance, and integrate with epilogue/legacy.

## Definition of Done

- `ShelterArchiveSystem.cs` exists with full `CaptureState/RestoreState`
- 6 archive categories implemented (decisions, events, memorials, milestones, discoveries, achievements)
- Automatic event recording functional
- Manual entry creation working
- Memorial system with 4 memorial types
- Memorial anniversary system
- Historical timeline with filtering
- Archive search and display
- Archive events and quest hooks
- Save/load round-trip tested
- Deterministic recording verified
- Old saves load without error
- 10 archive categories in data authority
- UI panel with browse, search, timeline views
- Cross-system integration (journal, memorial, consequences, governance, disasters, colonies)

## Follow-On Opportunities

- Archive exhibitions (display for visitors)
- Archive publications (compile shelter history book)
- Archive traditions (annual history review)
- Archive legacy (shelter history carries to New Game+)
- Archive quests (compile history, find artifacts)
