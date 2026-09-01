# Plan 163 — Wasteland Cartography & Mapping

## Goal

Create a wasteland cartography and mapping system where survivors can map the world, discover locations, create detailed maps, and trade cartographic knowledge. Currently the map shows expedition destinations but there is no cartography skill, no map discovery mechanic, no detailed mapping, no map trading, and no sense of exploring an unknown world. This plan adds exploration depth and makes the wasteland feel like a place to be discovered rather than a list of destinations.

## Why

**Repository evidence:** `ExpeditionSystem.cs` handles travel to predefined destinations. `LocationEvolutionSystem.cs` tracks location state. The map displays expedition destinations but there is no cartography system, no fog of war, no map discovery, no mapping skill, no map trading. The world is known from the start — no exploration, no discovery, no sense of the unknown.

**What is missing:** Players cannot map the world. There is no fog of war, no undiscovered locations, no cartography skill, no map creation, no map trading. The world is a fixed list of destinations, not an unknown territory to be explored and charted.

**Why existing plans don't solve it:** Plan 11 (world exploration) mentions living map geography but not cartography mechanics. Plan 32 (expedition destination wiring) connects destinations but not map discovery. Plan 49 (micro-location discovery) adds travel discoveries but not mapping. Plan 85 (damaged map zones) adds zone data but not cartography. No plan addresses mapping, cartography skill, or map discovery.

**Player value:** Creates exploration motivation (discover the unknown), adds strategic depth (maps reveal opportunities), provides progression (map fills in over time), and makes the world feel like a place to be explored rather than a menu to select from.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition travel
- `Assets/Ashfall.Core/LocationEvolutionSystem.cs` — location state
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill system
- `Assets/StreamingAssets/Data/locations.json` — location definitions
- NEW: `Assets/Ashfall.Core/Exploration/CartographySystem.cs`
- NEW: `Assets/StreamingAssets/Data/map_regions.json`

## Main Task 1 — Foundation / System Contract

1. Create `CartographySystem.cs` in `Assets/Ashfall.Core/Exploration/`
2. Define `MapRegion` DTO: `regionId`, `regionName`, `discovered` bool, `explored` (0-100 percentage), `hazards` (list), `pointsOfInterest` (list of location IDs), `terrain` (urban/rural/industrial/wasteland/water)
3. Define `MapDiscovery` DTO: `discoveryId`, `locationId`, `discoveredDay`, `discoveredBy` (survivor ID), `discoveryType` (location/hazard/resource/landmark/secret), `mapQuality` (0-100)
4. Define `CartographySkill` DTO: `survivorId`, `proficiency` (0-100), `specialization` (terrain/urban/industrial/wasteland), `tools` (list of mapping equipment)
5. Define `CartographyState` DTO: list of map regions, list of discoveries, list of cartographers, map completeness (0-100)
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define map discovery mechanics:
   - World starts with fog of war (most regions undiscovered)
   - Expeditions reveal regions along travel route
   - Cartography skill improves discovery quality and range
   - Mapping equipment (compass, sextant, survey tools) improves accuracy
   - Discovered locations added to map
   - Map quality affects detail (basic → detailed → precise)
8. Define cartography skill progression:
   - Skill increases with mapping activity
   - Higher skill: better discovery range, accuracy, detail
   - Specializations: terrain types where skill is bonus
   - Master cartographers can create highly accurate maps
9. Define map trading mechanics:
   - Maps can be traded with factions and settlements
   - Map value depends on quality and completeness
   - Trading maps builds faction standing
   - Buying maps reveals regions without exploration
   - Map rarity: some regions only discoverable through trade
10. Define map quality levels:
    - **Rough** (0-33): basic location names, major hazards
    - **Standard** (33-66): location details, resource indicators, terrain types
    - **Detailed** (66-100): precise locations, hidden features, optimal routes
    - Quality affects expedition planning and success
11. Define mapping equipment:
    - **Compass**: basic navigation, +10% discovery range
    - **Sextant**: position calculation, +20% accuracy
    - **Survey tools**: detailed mapping, +30% quality
    - **Cartography kit**: full equipment, +50% all bonuses
12. Add deterministic seeding: discovery outcomes use `ISeededRng`
13. Wire into `GameBootstrap`: `SetupCartography`, `TickCartography`, `SaveCartography`
14. Create `MapRegionCatalogLoader` for region definitions
15. Create UI hook: map panel showing discovered regions, fog of war, discoveries

## Main Task 2 — Implementation / Discovery / Skill / Trading / UI

1. Implement fog of war:
   - World map starts mostly hidden
   - Shelter location and immediate area revealed
   - Expedition routes reveal regions along path
   - Cartography expeditions specifically map regions
   - Undiscovered regions show as "?" or fog
2. Implement region discovery:
   - Expedition travels through region
   - Region marked as discovered
   - Locations within region revealed
   - Hazards and resources noted
   - Region exploration percentage increases
3. Implement cartography skill:
   - Survivors develop cartography skill through mapping
   - Skill improves discovery range and quality
   - Specializations for terrain types
   - Master cartographers create best maps
   - Skill can be taught (apprenticeship)
4. implement mapping equipment:
   - Equipment improves mapping quality
   - Equipment can be crafted or traded
   - Equipment degrades with use
   - Better equipment = better maps
5. Implement map trading:
   - Maps traded with factions and settlements
   - Map value based on quality and rarity
   - Trading builds faction standing
   - Buying maps reveals without exploration
   - Some regions only available through trade
6. Implement map quality:
   - Quality affects map detail and accuracy
   - Higher quality: better expedition planning
   - Quality improved by skill and equipment
   - Quality degrades if region changes (events, disasters)
7. Implement cartography expeditions:
   - Special expeditions focused on mapping
   - Cartography expeditions reveal regions faster
   - Expeditions can target specific regions
   - Expedition success depends on skill and equipment
8. Create cartography events:
   - "The Discovery" — new region discovered
   - "The Map" — high-quality map created
   - "The Trade" — map traded with faction
   - "The Expedition" — cartography expedition launched
   - "The Masterwork" — master cartographer creates perfect map
   - "The Secret" — hidden location discovered
   - "The Update" — map updated after region changes
9. Add cartography quest hooks:
   - "The Explorer" — discover all regions
   - "The Cartographer" — master cartography skill
   - "The Mapmaker" — create detailed map of region
   - "The Trade" — trade maps with all factions
   - "The Secret" — discover hidden locations
   - "The Legacy" — create definitive wasteland atlas
   - "The Expedition" — map dangerous region
10. Implement map UI:
    - World map with fog of war
    - Discovered regions shown
    - Locations marked
    - Hazards and resources indicated
    - Map quality indicator
    - Filter by discovery type
11. Add map journal: automatic log of discoveries
12. Implement cartography tutorial: first discovery explains system
13. Add map tooltips: hover over region shows discovery status
14. Create 20 map regions in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ExpeditionSystem`: expeditions reveal regions
2. Connect to `LocationEvolutionSystem`: discovered locations tracked
3. Integrate with `SkillProgressionSystem`: cartography skill progression
4. Connect to `FactionBranchCoordinator`: map trading affects standing
5. Wire into `MarketSystem`: maps traded as goods
6. Connect to `ColonySystem` (Plan 160): colonies reveal surrounding regions
7. Implement old-save compatibility: existing saves get default map state (shelter region revealed)
8. Add deterministic seeding: discovery uses `ISeededRng`
9. Create exploit prevention: discovery requires exploration, can't be rushed
10. Add tests: region discovery, skill progression, map trading, save round-trip
11. Verify catalog integrity: all region/location IDs resolve
12. Test edge cases: no discoveries (fog of war), all discovered (complete map)
13. Verify headless behavior: cartography processes correctly without UI
14. Add data-integrity-selftest: map regions validate against location catalogs
15. Create `--cartography-selftest` verb for CI validation

## State / System Interaction Model

```text
Wasteland cartography
├─ Fog of war
│  ├─ World starts mostly hidden
│  ├─ Shelter area revealed
│  ├─ Expeditions reveal regions
│  └─ Cartography expeditions map specifically
├─ Region discovery
│  ├─ Expedition travels through region
│  ├─ Region marked discovered
│  ├─ Locations revealed
│  ├─ Hazards/resources noted
│  └─ Exploration percentage increases
├─ Cartography skill
│  ├─ Skill increases with mapping
│  ├─ Better range, accuracy, detail
│  ├─ Specializations for terrain
│  ├─ Master cartographers best
│  └─ Skill teachable
├─ Mapping equipment
│  ├─ Equipment improves quality
│  ├─ Crafted or traded
│  ├─ Degrades with use
│  └─ Better = better maps
├─ Map trading
│  ├─ Trade with factions/settlements
│  ├─ Value based on quality/rarity
│  ├─ Builds faction standing
│  ├─ Buy maps to reveal
│  └─ Some regions trade-only
├─ Map quality
│  ├─ Rough → Standard → Detailed
│  ├─ Affects expedition planning
│  ├─ Improved by skill/equipment
│  └─ Degrades if region changes
└─ Integration
   ├─ Expeditions reveal regions
   ├─ Locations tracked
   ├─ Skill progression
   ├─ Faction standing
   ├─ Market trading
   └─ Colonies reveal area
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --cartography-selftest
```

## Risk

**LOW** — Cartography system is straightforward with clear inputs (exploration, skill, equipment) and outputs (discovered regions, map quality). Risk of fog of war feeling arbitrary rather than meaningful. Mitigation: make discovery tied to exploration, show clear progress, provide multiple discovery paths (expedition, trade, cartography), and integrate with expedition planning.

## Definition of Done

- `CartographySystem.cs` exists with full `CaptureState/RestoreState`
- Fog of war system functional
- Region discovery mechanics working
- Cartography skill progression implemented
- Mapping equipment system
- Map trading with factions
- Map quality levels (rough, standard, detailed)
- Cartography events and quest hooks
- Save/load round-trip tested
- Deterministic discovery verified
- Old saves load without error
- 20 map regions in data authority
- UI map with fog of war and discovery status
- Cross-system integration (expedition, location, skills, factions, market, colonies)

## Follow-On Opportunities

- Cartography specialization (terrain-specific mapping)
- Map artifacts (ancient maps with secrets)
- Cartography legacy (famous maps remembered)
- Cartography quests (map specific regions, discover secrets)
- Cartography competitions (who can map fastest)
