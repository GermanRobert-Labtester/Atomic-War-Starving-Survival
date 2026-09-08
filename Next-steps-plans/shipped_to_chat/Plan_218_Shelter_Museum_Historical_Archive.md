# Plan 218 — Shelter Museum & Historical Archive System

## Goal

Create a shelter museum and historical archive system where significant artifacts, documents, and records from the shelter's history are collected, preserved, and displayed — creating a physical space that tells the story of the shelter and its survivors across time. Currently `MemorialSystem` (262 lines) handles burial remembrance, Plan 162 (Shelter Archive) records history, and Plan 178 (Art & Culture) covers art exhibitions — but there is no dedicated museum/archive system for displaying historical artifacts, no curated collection, no exhibition space, no historical preservation mechanics. The shelter has no museum. This plan adds historical preservation as a cultural layer.

## Why

**Repository evidence:** Grep for `MuseumSystem`, `HistoricalArchive`, `ShelterMuseum`, `ArtifactDisplay`, `ExhibitionSystem`, `HistoricalRecord`, `ArchiveSystem`, `MuseumCollection`, `ArtifactCatalog`, `HeritageDisplay` in Core returns ZERO matches. `MemorialSystem` handles burial remembrance. Plan 162 covers shelter archive (recording history). Plan 178 covers art exhibitions. But no museum/archive system for displaying historical artifacts exists.

**What is missing:** No museum system. No historical archive display. No artifact collection. No curated exhibitions. No historical preservation mechanics. No physical display space for historical items. The shelter records its history but doesn't display it.

**Why existing plans don't solve it:** Plan 162 (Shelter Archive) records history but doesn't display it. Plan 178 (Art & Culture) covers art exhibitions but not historical artifacts. Plan 212 (Time Capsules) covers messages for the future but not historical display. No plan addresses museum/archive as a system.

**Player value:** Creates cultural depth (shelter has a museum), adds historical continuity (past is preserved and displayed), generates emergent stories (artifact discoveries, exhibitions), and makes the shelter feel like a place with history worth remembering.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/MemorialSystem.cs` — remembrance (262 lines, complementary)
- `Assets/Ashfall.Core/Inventory/Inventory.cs` — items (artifact source)
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` — death (artifact origin)
- NEW: `Assets/Ashfall.Core/Culture/ShelterMuseumSystem.cs`
- NEW: `Assets/StreamingAssets/Data/museum_collection_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterMuseumSystem.cs` in `Assets/Ashfall.Core/Culture/`
2. Define `MuseumCollection` DTO: `collectionId`, `collectionName`, `curatorId` (survivor_id), `artifacts` (list of `MuseumArtifact`), `exhibitions` (list of `Exhibition`), `totalValue` (historical significance score), `visitorCount` (total visitors), `establishedDay`, `location` (room_id)
3. Define `MuseumArtifact` DTO: `artifactId`, `itemId` (item_id from inventory), `artifactName`, `artifactType` (document/tool/weapon/clothing/personal_effect/technological/natural/historical), `originStory` (how/when/where acquired), `historicalSignificance` (0-100), `condition` (0-100), `displayedSince` (day), `donorId` (survivor_id who donated), `isOnDisplay` bool
4. Define `Exhibition` DTO: `exhibitionId`, `exhibitionName`, `theme` (founding/medical/military/cultural/technological/personal/memorial), `artifacts` (list of artifact_ids), `startDate`, `endDate` (-1 if permanent), `description`, `visitorCount`, `moraleBoost` (float), `status` (planned/active/completed/cancelled)
5. Define `MuseumEvent` DTO: `eventId`, `eventType` (artifact_donated/exhibition_opened/exhibition_closed/visitor_milestone/collection_milestone/curator_appointed/artifact_discovered), `day`, `description`, `participants` (list of survivor_ids), `significance` (minor/moderate/major)
6. Define `ShelterMuseumState` DTO: museum collection, list of exhibitions, list of museum events, museum settings (museum enabled bool, auto-donate bool, exhibition frequency)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define artifact types (8+ types):
   - **Document**: letters, records, maps, journals
   - **Tool**: significant tools used in shelter history
   - **Weapon**: weapons from notable combats/defenses
   - **Clothing**: significant clothing (leader's coat, medic's uniform)
   - **Personal Effect**: keepsakes from notable survivors
   - **Technological**: pre-war tech, inventions, prototypes
   - **Natural**: unusual natural specimens
   - **Historical**: items from shelter's founding/early days
9. Define museum mechanics:
   - Artifacts donated by survivors or discovered
   - Artifacts assessed for historical significance
   - Artifacts displayed in museum
   - Museum has curator (survivor who manages collection)
   - Museum logged
10. Define exhibition mechanics:
    - Exhibitions curated around themes
    - Exhibitions have start/end dates
    - Exhibitions boost morale
    - Exhibitions attract visitors
    - Exhibitions logged
11. Define historical significance:
    - Items have historical significance score (0-100)
    - Significance based on: age, origin story, donor importance, rarity
    - High significance items: featured in exhibitions
    - Significance logged
12. Define visitor mechanics:
    - Survivors visit museum
    - Visitors gain morale boost
    - Visitor count tracked
    - Visitors logged
13. Add deterministic seeding: museum events use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupMuseum`, `TickMuseum`, `SaveMuseum`

## Main Task 2 — Implementation / Collection / Exhibitions / Artifacts / Visitors / UI

1. Implement museum collection:
   - Artifacts collected/donated
   - Collection assessed for significance
   - Collection displayed
   - Collection logged
2. Implement exhibitions:
   - Exhibitions curated around themes
   - Exhibitions have duration
   - Exhibitions boost morale
   - Exhibitions logged
3. Implement artifacts:
   - Artifacts donated/discovered
   - Artifacts assessed for significance
   - Artifacts displayed
   - Artifacts logged
4. Implement curator:
   - Curator manages collection
   - Curator organizes exhibitions
   - Curator logged
5. Implement visitors:
   - Survivors visit museum
   - Visitors gain morale
   - Visitor count tracked
   - Visitors logged
6. Implement museum UI:
   - Museum panel: collection, exhibitions, curator
   - Artifact detail: significance, origin, display status
   - Exhibition panel: active/planned exhibitions
   - Visitor log: visitor count, morale effects
   - Museum event log: history of events
7. Create museum events:
    - "The Donation" — artifact donated
    - "The Exhibition" — exhibition opened
    - "The Discovery" — significant artifact found
    - "The Visitor" — visitor milestone reached
    - "The Curator" — curator appointed
    - "The Collection" — collection milestone
    - "The Memorial" — memorial exhibition
    - "The Legacy" — historical preservation achieved
8. Add museum quest hooks:
    - "The Curator" — curate 10 exhibitions
    - "The Collector" — collect 50 artifacts
    - "The Historian" — document 20 artifact origin stories
    - "The Donor" — donate 10 personal items
    - "The Visitor" — attract 100 museum visitors
    - "The Preservation" — preserve items from each shelter era
    - "The Legacy" — establish museum lasting 200 days
9. Implement museum tutorial: first donation explains system
10. Add museum tooltips: hover over artifact shows details
11. Create museum collection templates in data file (20+ artifact types)
12. Implement museum persistence: collection/exhibitions saved
13. Integrate with `MemorialSystem`: memorial artifacts in museum

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MemorialSystem`: memorial artifacts displayed
2. Connect to `Inventory`: artifacts sourced from inventory
3. Integrate with `SurvivorFateSystem`: deceased survivor items donated
4. Connect to `PersonalBelongingsSystem` (Plan 210): sentimental items donated
5. Wire into `TimeCapsuleSystem` (Plan 212): capsules displayed after opening
6. Connect to `GenealogySystem` (Plan 217): family artifacts displayed
7. Implement old-save compatibility: existing saves get no museum
8. Add deterministic seeding: museum events use `ISeededRng`
9. Create exploit prevention: artifacts are finite, can't be gamed
10. Add tests: collection, exhibitions, artifacts, visitors, curator, save round-trip
11. Verify all artifact types work correctly
12. Test edge cases: no museum (current behavior), extensive collection (major museum)
13. Verify headless behavior: museum processes correctly without UI
14. Add data-integrity-selftest: museum validates against item/survivor catalogs
15. Create `--shelter-museum-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-museum-selftest
```

## Risk

**LOW** — Museum is straightforward with clear inputs (artifacts) and outputs (exhibitions, morale). Risk of museum feeling like decoration. Mitigation: make artifacts meaningful (origin stories, historical significance), show clear morale effects, and ensure museum feels like cultural preservation not just display.

## Definition of Done

- `ShelterMuseumSystem.cs` exists with full `CaptureState/RestoreState`
- 8+ artifact types (document, tool, weapon, clothing, personal effect, technological, natural, historical)
- Museum collection (artifacts, significance, display)
- Exhibitions (themed, timed, morale-boosting)
- Curator system (survivor manages collection)
- Visitor mechanics (morale boost, count tracking)
- Historical significance scoring
- Museum events and quest hooks
- Save/load round-trip tested
- Deterministic museum events verified
- Old saves load with no museum
- Museum collection templates in data authority (20+ artifact types)
- UI museum panel, artifact detail, exhibition panel, visitor log, event log
- Cross-system integration (memorial, inventory, fate, belongings, time capsules, genealogy)

## Follow-On Opportunities

- Museum specialization (survivors become expert curators/historians)
- Museum legacy (famous museums remembered across campaigns)
- Museum quests (specific collection goals)
- Museum events (grand opening, traveling exhibition)
- Museum trading (trade artifacts with other settlements)
