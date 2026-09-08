# Plan 187 — Bestiary & Creature Encounter Tracking UI

## Goal

Create a bestiary and creature encounter tracking system where players can view discovered creatures, track encounter counts, record kills/sightings, read behavioral notes, and build a comprehensive catalog of wasteland fauna. Currently `WastelandBestiaryCatalog.cs` (105 lines) provides static creature data (24 irradiated wasteland fauna entries with calories, habitat, danger level), but there is no encounter tracking, no kill count, no sighting log, no discovery system, and no bestiary UI panel. Creatures exist as data but players have no way to track their interactions with them. This plan adds a discovery/collector dimension to exploration.

## Why

**Repository evidence:** Grep for `Bestiary`, `CreatureEncounter`, `CreatureSighting`, `KillCount`, `CreatureDiscovery`, `bestiary_panel`, `CreatureCatalog` in Core returns only `WastelandBestiaryCatalog.cs` (105 lines) — a static data catalog with no tracking. No encounter tracking, no kill counts, no sighting logs, no discovery system. The catalog has `butchered_meat_calories`, `harlan_scout_notes`, habitat data, but no player-facing tracking. `JournalSystem` has codex unlocks (`UnlockItemSeen`, `UnlockLocationVisited`) but no creature discovery integration.

**What is missing:** No creature encounter tracking. No kill/sighting counts. No discovery system (creatures start "unknown" until encountered). No bestiary UI panel. No behavioral notes that unlock with encounters. No creature danger assessment. No habitat mapping. No collection completion tracking.

**Why existing plans don't solve it:** Plan 151 (working animals) adds animal companions but not bestiary tracking. Plan 133 (expedition consequences) adds location discovery but not creature discovery. Plan 167 (tunnel network) adds underground exploration but not creature cataloging. No plan addresses bestiary/creature tracking as a system.

**Player value:** Creates collection/compulsion loop (discover all creatures), adds strategic depth (track dangerous creatures, learn habitats), generates emergent stories (first encounter with rare creature, near-death experience), and makes exploration feel rewarding (each new creature is a discovery).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Narrative/WastelandBestiaryCatalog.cs` — static creature data
- `Assets/Ashfall.Core/Journal/` — journal system (codex unlocks)
- `Assets/Ashfall.Core/Expedition/` — expedition system (encounters)
- `Assets/Ashfall.Core/Combat/` — combat system (creature kills)
- NEW: `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs`
- NEW: `src/UI/BestiaryPanel.cs`

## Main Task 1 — Foundation / System Contract

1. Create `BestiarySystem.cs` in `Assets/Ashfall.Core/Bestiary/`
2. Define `CreatureDiscovery` DTO: `creatureId`, `discoveredDay`, `encounterCount` (times seen), `killCount` (times killed), `butcherCount` (times butchered), `firstEncounterLocation` (location_id), `lastEncounterDay`, `discoveryContext` (first encounter description: "spotted during expedition", "attacked shelter", "found dead", etc.)
3. Define `CreatureSighting` DTO: `sightingId`, `creatureId`, `day`, `locationId`, `witnessSurvivorId`, `sightingType` (spotted/attacked/fleeing/dead/track_found), `distance` (close/medium/far), `behavior` (hunting/resting/migrating/feeding/aggressive)
4. Define `CreatureBehaviorNote` DTO: `noteId`, `creatureId`, `noteType` (habitat/diet/behavior/weakness/strength/danger), `unlockThreshold` (encounter count required to unlock), `noteText` (unlocked narrative text)
5. Define `BestiaryState` DTO: list of creature discoveries, list of sightings (recent 50), list of unlocked behavior notes, bestiary completion percentage, total creatures discovered count
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define discovery mechanics:
   - Creatures start "unknown" until first encounter
   - First encounter: creature added to bestiary with basic info (name, silhouette, "unknown creature")
   - 3 encounters: basic stats unlocked (danger level, habitat)
   - 5 encounters: behavior notes unlocked (diet, activity patterns)
   - 10 encounters: detailed notes unlocked (weaknesses, strengths, tactics)
   - 1 kill: combat notes unlocked (fighting style, vulnerabilities)
   - 5 kills: hunting notes unlocked (tracking, bait, ambush tactics)
   - 1 butcher: resource notes unlocked (meat quality, hide value, bone uses)
8. Define creature categories (from existing 24 creatures in catalog):
   - **Mammals**: irradiated deer, mutant wolves, feral dogs, rad-bears
   - **Birds**: mutant ravens, irradiated hawks, scavenger crows
   - **Reptiles**: rad-lizards, mutant snakes, irradiated turtles
   - **Insects**: giant beetles, irradiated ants, mutant wasps
   - **Aquatic**: mutant fish, irradiated crabs, deep-water creatures
   - **Cryptids**: rumored creatures (unconfirmed sightings)
   - **Domestic**: feral livestock, escaped zoo animals
9. Define sighting tracking:
   - Each expedition has chance to generate creature sightings
   - Sightings logged with location, time, witness
   - Multiple witnesses increase confidence
   - Rare creatures have fewer sightings
   - Cryptids only have unconfirmed sightings
10. Define behavior note unlock system:
    - Each creature has 6-8 behavior notes
    - Notes unlock at encounter thresholds
    - Notes provide strategic information
    - Notes written in narrative tone (survivor observations)
    - Notes reference existing creature data (calories, habitat, danger)
11. Define bestiary UI:
    - Creature list: all discovered creatures with silhouette/name
    - Creature detail: stats, encounters, kills, unlocked notes
    - Sighting log: recent sightings with location/day/witness
    - Completion tracker: X/24 creatures discovered, X notes unlocked
    - Filter by category (mammal/bird/reptile/insect/aquatic/cryptid)
    - Sort by discovery date, encounter count, danger level
12. Add deterministic seeding: encounter generation uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupBestiary`, `TickBestiary` (process sightings), `SaveBestiary`
14. Create bestiary panel UI in `src/UI/BestiaryPanel.cs`
15. Integrate with `JournalSystem`: creature discoveries unlock journal entries

## Main Task 2 — Implementation / Discovery / Tracking / Notes / UI

1. Implement discovery system:
   - First encounter: creature added to bestiary
   - Basic info revealed (name from catalog, silhouette image)
   - "Unknown Creature" until 3 encounters
   - Discovery event logged
   - Discovery notification shown
2. Implement encounter tracking:
   - Each creature encounter increments counter
   - Encounters tracked by type (spotted/attacked/fleeing/dead)
   - Encounter location recorded
   - Encounter day recorded
   - Witness survivor recorded
3. Implement kill/butcher tracking:
   - Kill count incremented when creature killed in combat
   - Butcher count incremented when creature butchered
   - Kill/butcher data feeds into note unlocks
   - Kill locations tracked
4. Implement behavior note unlocks:
   - Check encounter thresholds daily
   - When threshold reached: note unlocked
   - Unlock event logged
   - Unlock notification shown
   - Note text revealed (narrative description)
5. Implement sighting generation:
   - Expeditions generate creature sightings based on location
   - Location habitat data determines which creatures can appear
   - Season/weather affect sighting probability
   - Rare creatures have low sighting chance
   - Cryptids have very low sighting chance
   - Multiple witnesses increase confidence
6. Implement sighting log:
   - Recent 50 sightings stored
   - Each sighting: creature, day, location, witness, type, behavior
   - Sighting list viewable in UI
   - Sighting map shows locations
7. Implement bestiary completion:
   - Track X/24 creatures discovered
   - Track X/Y behavior notes unlocked
   - Completion percentage calculated
   - Completion rewards (optional: morale bonus, knowledge bonus)
8. Implement creature categories:
   - Categorize creatures by type (mammal/bird/reptile/etc.)
   - Category filters in UI
   - Category completion tracking
   - Category icons in list
9. Implement creature detail view:
   - Creature name and image (silhouette until fully discovered)
   - Basic stats (danger level, habitat, diet)
   - Encounter/kill/butcher counts
   - Unlocked behavior notes
   - First encounter details
   - Last encounter details
   - Sighting history
10. Create discovery events:
    - "The Discovery" — new creature found
    - "The Sighting" — rare creature spotted
    - "The Encounter" — close encounter with creature
    - "The Kill" — creature killed
    - "The Butcher" — creature butchered for resources
    - "The Note" — behavior note unlocked
    - "The Completion" — all creatures in category discovered
    - "The Master" — all 24 creatures discovered
11. Add discovery quest hooks:
    - "The Naturalist" — discover 10 creatures
    - "The Hunter" — kill 5 different creature types
    - "The Observer" — unlock 20 behavior notes
    - "The Completion" — discover all 24 creatures
    - "The Cryptid" — confirm a cryptid sighting
    - "The Tracker" — log 50 creature sightings
    - "The Butcher" — butcher 10 creatures
12. Implement bestiary UI panel:
    - Creature list with silhouettes/names
    - Category filters
    - Sort options
    - Completion tracker
    - Search/filter by name
13. Add bestiary journal: automatic log of discovery events
14. Implement discovery tutorial: first creature encounter explains system
15. Add discovery tooltips: hover over creature shows encounter count, note progress

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `WastelandBestiaryCatalog`: creature data loaded
2. Connect to `ExpeditionSystem`: sightings generated during expeditions
3. Integrate with `CombatSystem`: kills tracked
4. Connect to `JournalSystem`: discoveries unlock journal entries
5. Wire into `LocationEvolutionSystem`: creature habitats affect location evolution
6. Connect to `WeatherSystem`: weather affects sighting probability
7. Implement old-save compatibility: existing saves get empty bestiary (all creatures unknown)
8. Add deterministic seeding: sightings use `ISeededRng`
9. Create exploit prevention: sightings are time/location-based, can't be farmed
10. Add tests: discovery, encounter tracking, note unlocks, sightings, save round-trip
11. Verify all 24 creatures can be discovered
12. Test edge cases: no encounters (empty bestiary), many encounters (all notes unlocked)
13. Verify headless behavior: bestiary processes correctly without UI
14. Add data-integrity-selftest: bestiary validates against creature catalog
15. Create `--bestiary-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bestiary-selftest
```

## Risk

**LOW** — Bestiary tracking is straightforward with clear inputs (encounters, kills) and outputs (discovery, note unlocks). Risk of bestiary feeling like a chore rather than rewarding discovery. Mitigation: make discoveries feel meaningful (narrative text, notifications), show clear progress, and ensure rare creatures feel special.

## Definition of Done

- `BestiarySystem.cs` exists with full `CaptureState/RestoreState`
- 24 creatures from `WastelandBestiaryCatalog` trackable
- Discovery system (unknown → basic → detailed → complete)
- Encounter/kill/butcher counting
- Behavior note unlock system (6-8 notes per creature)
- Sighting generation during expeditions
- Sighting log (recent 50)
- Bestiary completion tracking (X/24 creatures, X/Y notes)
- Creature categories (mammal/bird/reptile/insect/aquatic/cryptid/domestic)
- Discovery events and quest hooks
- Save/load round-trip tested
- Deterministic sightings verified
- Old saves load with empty bestiary
- UI bestiary panel with creature list, detail view, filters
- Cross-system integration (catalog, expedition, combat, journal, location, weather)

## Follow-On Opportunities

- Creature illustration unlocks (detailed art at full discovery)
- Creature trophy system (display killed creatures)
- Creature habitat mapping (show creature ranges on map)
- Creature migration tracking (seasonal movement patterns)
- Creature domestication (tame creatures for use)
