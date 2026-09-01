# Plan 219 — Survivor Photography & Documentation System

## Goal

Create a survivor photography and documentation system where survivors can capture moments in shelter life through photography, sketching, and written documentation — creating a visual and textual record of the shelter's history, daily life, and significant events. Currently the shelter records data (events, statistics, logs) but there is no system for survivors to actively document life through creative media — no photography, no sketching, no journaling-as-documentation, no photo albums, no visual record. The shelter has no visual historian. This plan adds documentation as a creative and historical layer.

## Why

**Repository evidence:** Grep for `PhotographySystem`, `DocumentationSystem`, `PhotoSystem`, `CameraSystem`, `PhotoAlbum`, `Photograph`, `SketchSystem`, `VisualRecord`, `Documentarian`, `ShelterJournal` in Core returns ZERO system matches (only string references in `PhantomMemoryEngine` and quest narratives). No photography or documentation system exists.

**What is missing:** No photography system. No sketching system. No visual documentation. No photo albums. No documentation mechanics. No camera equipment. No visual record creation. The shelter records data but doesn't create visual/textual documentation.

**Why existing plans don't solve it:** Plan 162 (Shelter Archive) records history automatically but doesn't involve survivor-created documentation. Plan 178 (Art & Culture) covers art creation but not documentation specifically. Plan 218 (Museum) displays artifacts but doesn't create documentation. Plan 212 (Time Capsules) preserves messages but not visual documentation. No plan addresses photography/documentation as a system.

**Player value:** Creates creative expression (survivors document life), adds historical depth (visual record of shelter history), generates emergent stories (photo albums, documentation projects), and makes the shelter feel like a place worth remembering visually.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Journal/JournalSystem.cs` — journal (text documentation)
- `Assets/Ashfall.Core/MemorialSystem.cs` — remembrance (complementary)
- `Assets/Ashfall.Core/Inventory/Inventory.cs` — items (camera equipment)
- NEW: `Assets/Ashfall.Core/Culture/DocumentationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/documentation_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `DocumentationSystem.cs` in `Assets/Ashfall.Core/Culture/`
2. Define `Documentation` DTO: `documentationId`, `documentationType` (photograph/sketch/written_record/audio_recording/video_recording), `authorId` (survivor_id), `subjectType` (person/place/event/object/daily_life/significant_moment), `subjectId` (survivor_id or location_id or event_id or item_id), `title`, `description`, `createdDay`, `quality` (0-100), `sentimentalValue` (0-100), `isPublic` bool, `tags` (list of string tags)
3. Define `Photograph` DTO: `photoId`, `documentationId`, `cameraUsed` (item_id), `subjects` (list of survivor_ids photographed), `location` (location_id where taken), `composition` (0-100, artistic quality), `lighting` (0-100, technical quality), `moment` (candid/posed/documentary/artistic), `filmRemaining` (if film camera, exposures left)
4. Define `PhotoAlbum` DTO: `albumId`, `albumName`, `ownerId` (survivor_id), `photos` (list of photo_ids), `theme` (family/shelter/events/portraits/landscape/daily_life/historical), `createdDay`, `lastUpdatedDay`, `pageCount`, `isShared` bool
5. Define `Sketch` DTO: `sketchId`, `documentationId`, `subject` (what was sketched), `medium` (pencil/charcoal/ink/watercolor/digital), `artisticQuality` (0-100), `accuracy` (0-100, how accurately it represents subject), `timeSpent` (hours)
6. Define `WrittenRecord` DTO: `recordId`, `documentationId`, `recordType` (journal_entry/letter/report/chronicle/poem/essay), `content` (text), `wordCount`, `writingQuality` (0-100), `intendedAudience` (private/family/shelter/public)
7. Define `DocumentationEvent` DTO: `eventId`, `eventType` (photo_taken/sketch_created/record_written/album_created/documentation_shared/documentation_discovered/album_completed), `day`, `authorId`, `documentationId`, `description`, `significance` (minor/moderate/major)
8. Define `DocumentationState` DTO: list of all documentation, list of photo albums, list of documentation events, documentation settings (documentation enabled bool, auto-document bool, quality modifier)
9. Implement `CaptureState/RestoreState` with schema versioning
10. Define documentation types (5 types):
    - **Photograph**: visual capture of moment (requires camera)
    - **Sketch**: artistic drawing of subject (requires art supplies)
    - **Written Record**: text documentation (journal, letter, report)
    - **Audio Recording**: sound capture (requires recording equipment)
    - **Video Recording**: moving image capture (requires video equipment)
11. Define photography mechanics:
    - Survivor with camera can take photographs
    - Photos have subjects, location, quality
    - Film cameras have limited exposures
    - Digital cameras unlimited but require power
    - Photo quality based on: photographer skill, lighting, subject cooperation
    - Photos logged
12. Define album mechanics:
    - Photos organized into albums
    - Albums have themes
    - Albums can be shared or private
    - Albums logged
13. Define documentation quality:
    - Quality based on: author skill, equipment, conditions, time spent
    - High quality: morale boost when viewed, historical value
    - Low quality: still has sentimental value
    - Quality logged
14. Define documentation sharing:
    - Documentation can be shared with shelter
    - Shared documentation: morale boost for shelter
    - Private documentation: personal value only
    - Sharing logged
15. Add deterministic seeding: documentation events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupDocumentation`, `TickDocumentation`, `SaveDocumentation`

## Main Task 2 — Implementation / Photos / Sketches / Records / Albums / UI

1. Implement photography:
    - Survivor with camera takes photo
    - Photo has subjects, location, quality
    - Film/digital camera mechanics
    - Photo logged
2. Implement sketching:
    - Survivor with art supplies creates sketch
    - Sketch has subject, medium, quality
    - Sketch logged
3. Implement written records:
    - Survivor writes documentation
    - Record has type, content, quality
    - Record logged
4. Implement albums:
    - Photos organized into albums
    - Albums have themes
    - Albums can be shared
    - Albums logged
5. Implement documentation quality:
    - Quality based on skill/equipment/conditions
    - Quality affects morale/historical value
    - Quality logged
6. Implement documentation sharing:
    - Documentation shared with shelter
    - Shared documentation: morale boost
    - Sharing logged
7. Implement documentation UI:
    - Documentation panel: all documentation, filter by type
    - Photo viewer: view photographs
    - Album panel: view/create albums
    - Sketch gallery: view sketches
    - Record library: view written records
    - Documentation log: history of documentation
8. Create documentation events:
    - "The Photo" — photograph taken
    - "The Sketch" — sketch created
    - "The Record" — written record created
    - "The Album" — photo album created
    - "The Sharing" — documentation shared
    - "The Discovery" — old documentation found
    - "The Collection" — album completed
    - "The Masterpiece" — high-quality documentation created
9. Add documentation quest hooks:
    - "The Photographer" — take 50 photographs
    - "The Artist" — create 30 sketches
    - "The Writer" — write 20 records
    - "The Archivist" — organize 10 albums
    - "The Documentarian" — document all shelter areas
    - "The Historian" — document 50 significant events
    - "The Sharer" — share 20 documentation pieces
10. Implement documentation tutorial: first photo explains system
11. Add documentation tooltips: hover shows details
12. Create documentation templates in data file (15+ templates)
13. Implement documentation persistence: documentation/albums saved
14. Integrate with `JournalSystem`: written records link to journal

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `JournalSystem`: written records integrate
2. Connect to `MemorialSystem`: memorial documentation
3. Integrate with `Inventory`: camera/art supply items
4. Connect to `PersonalBelongingsSystem` (Plan 210): documentation as belongings
5. Wire into `ShelterMuseumSystem` (Plan 218): documentation displayed in museum
6. Connect to `TimeCapsuleSystem` (Plan 212): documentation in capsules
7. Implement old-save compatibility: existing saves get no documentation
8. Add deterministic seeding: documentation events use `ISeededRng`
9. Create exploit prevention: documentation requires equipment/time, can't be gamed
10. Add tests: photos, sketches, records, albums, quality, sharing, save round-trip
11. Verify all documentation types work correctly
12. Test edge cases: no documentation (current behavior), extensive documentation (rich archive)
13. Verify headless behavior: documentation processes correctly without UI
14. Add data-integrity-selftest: documentation validates against survivor/location/item catalogs
15. Create `--documentation-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --documentation-selftest
```

## Risk

**LOW** — Documentation is straightforward with clear inputs (documentation creation) and outputs (albums, morale). Risk of documentation feeling like busywork. Mitigation: make documentation meaningful (morale effects, historical value), show clear quality differences, and ensure documentation feels like creative expression not just data entry.

## Definition of Done

- `DocumentationSystem.cs` exists with full `CaptureState/RestoreState`
- 5 documentation types (photograph, sketch, written record, audio recording, video recording)
- Photography mechanics (camera, subjects, location, quality, film/digital)
- Sketching mechanics (art supplies, subject, medium, quality)
- Written records (journal, letter, report, chronicle, poem, essay)
- Photo albums (themes, organization, sharing)
- Documentation quality (skill/equipment/conditions based)
- Documentation sharing (morale effects)
- Documentation events and quest hooks
- Save/load round-trip tested
- Deterministic documentation events verified
- Old saves load with no documentation
- Documentation templates in data authority (15+ templates)
- UI documentation panel, photo viewer, album panel, sketch gallery, record library, log
- Cross-system integration (journal, memorial, inventory, belongings, museum, time capsules)

## Follow-On Opportunities

- Documentation specialization (survivors become expert photographers/artists/writers)
- Documentation legacy (famous documentation remembered across campaigns)
- Documentation quests (specific documentation goals)
- Documentation events (documentation exhibition, historical discovery)
- Documentation trading (trade documentation services with other settlements)
