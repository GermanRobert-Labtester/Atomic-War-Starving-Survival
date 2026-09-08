# Plan 156 — Shelter Expansion & Physical Renovation

## Goal

Create a shelter expansion and physical renovation system where the bunker grows over time — new rooms are dug, existing areas are renovated, structural improvements are made, and the shelter physically transforms from a cramped emergency shelter into a thriving underground community. Currently shelter rooms are defined in catalogs (Plan 41) but the shelter itself doesn't physically grow or change. This plan adds spatial progression and visible shelter evolution.

## Why

**Repository evidence:** `ShelterScheduleSystem.cs` (243 lines) manages room assignments. `ShelterThermalSystem.cs` (469 lines) handles temperature. `PowerGridSystem` (referenced in Plan 71) manages power rooms. But the shelter is static — rooms exist from the start, nothing is built, nothing is renovated, nothing expands. The shelter-as-character plan (Plan 29) covers room identity and wear but not physical growth. Plan 41 (shelter room catalog) adds room definitions but not construction.

**What is missing:** The shelter doesn't grow. Players can't dig new rooms, renovate existing ones, or physically expand their underground home. The shelter is a fixed set of rooms from day one. There's no sense of progression, no visible transformation, no "we built this" pride.

**Why existing plans don't solve it:** Plan 29 (shelter as character) covers room personality and wear but not construction. Plan 41 (shelter room catalog) defines rooms but not building them. Plan 71 (power grid rooms) adds power infrastructure but not expansion. Plan 138 (shelter defense) adds fortification but not spatial growth. No plan addresses physical shelter expansion or renovation.

**Player value:** Creates progression (shelter grows from cramped to spacious), adds pride of ownership ("we built this"), provides strategic decisions (what to build next?), and makes the shelter feel like a living space that evolves with the community.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Shelter/` — shelter systems
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — room assignments
- `Assets/StreamingAssets/Data/shelter_rooms.json` (VERIFY) — room definitions
- NEW: `Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs`
- NEW: `Assets/StreamingAssets/Data/shelter_construction.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterExpansionSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `ConstructionProject` DTO: `projectId`, `projectType` (new_room/renovation/expansion/upgrade), `targetRoomId` (for renovation/upgrade), `blueprintId`, `resourceCost` (map of item → quantity), `laborRequired` (person-days), `constructionDay` (-1 if not started), `completionDay` (-1 if incomplete), `status` (planned/active/completed)
3. Define `ShelterRoom` DTO: `roomId`, `roomType` (living/storage/medical/work/leisure/infrastructure), `name`, `condition` (0-100), `upgrades` (list), `connectedRooms` (list), `capacity`, `temperature`, `radiation`
4. Define `ShelterExpansionState` DTO: list of rooms, list of construction projects, list of blueprints discovered, shelter level (depth), shelter footprint (area)
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define construction types:
   - **New room**: dig new room (requires blueprint, resources, labor)
   - **Renovation**: repair/improve existing room (restore condition, add upgrades)
   - **Expansion**: connect to adjacent area (tunnel to new section)
   - **Upgrade**: install systems (power, water, ventilation, defense)
7. Define room types with distinct functions:
   - **Living**: quarters, apartments, family housing
   - **Storage**: supplies, equipment, food, water
   - **Medical**: clinic, hospital, pharmacy, quarantine
   - **Work**: workshop, factory, laboratory, office
   - **Leisure**: common room, library, gym, garden
   - **Infrastructure**: power, water, ventilation, defense
8. Define construction mechanics:
   - Blueprints discovered through exploration, research, or trade
   - Resources consumed during construction
   - Labor assigned from survivor duty roster
   - Construction takes time (days to weeks)
   - Construction can be interrupted/resumed
   - Failed construction (resource shortage) wastes partial progress
9. Define shelter growth mechanics:
   - Shelter expands outward (new wings) and downward (deeper levels)
   - Each expansion unlocks new room slots
   - Deeper levels are more stable but harder to dig
   - Expansion requires structural support (engineering)
   - Expansion affects shelter stability (risk of collapse if over-expanded)
10. Add deterministic seeding: construction outcomes use `ISeededRng`
11. Wire into `GameBootstrap`: `SetupShelterExpansion`, `TickConstruction`, `SaveShelterExpansion`
12. Create `ShelterBlueprintCatalogLoader` for blueprint definitions
13. Implement room condition: rooms degrade over time, need maintenance
14. Create UI hook: shelter map showing rooms, construction, expansion
15. Implement construction journal: automatic log of building projects

## Main Task 2 — Implementation / Construction / Renovation / Expansion

1. Implement new room construction:
   - Player selects blueprint and location
   - Assign resources and labor
   - Construction progresses daily
   - Room completed when labor fulfilled
   - Room becomes available for assignment
2. Implement renovation:
   - Existing rooms can be renovated
   - Renovation restores condition to 100%
   - Renovation can add upgrades (heating, insulation, decoration)
   - Renovated rooms provide better morale and efficiency
3. Implement shelter expansion:
   - Expand to adjacent areas (north, south, east, west, down)
   - Each direction has unique challenges (rock, water, radiation)
   - Expansion requires engineering (structural support)
   - Expansion unlocks new room slots in that direction
   - Deeper levels require elevators/ladders (vertical transport)
4. Implement room upgrades:
   - **Heating**: reduces cold penalty
   - **Insulation**: reduces temperature fluctuation
   - **Ventilation**: reduces air quality issues
   - **Lighting**: improves morale, reduces accidents
   - **Decoration**: improves morale significantly
   - **Security**: reduces theft, improves safety
5. Implement infrastructure systems:
   - **Power grid**: connect rooms to power (Plan 71 integration)
   - **Water system**: connect rooms to water (plumbing)
   - **Ventilation**: connect rooms to air filtration
   - **Communications**: connect rooms to radio/network (Plan 157 integration)
   - **Defense**: connect rooms to defense system (Plan 138 integration)
6. Implement construction events:
   - "The Blueprint" — discover new construction blueprint
   - "The Groundbreaking" — start major construction project
   - "The Completion" — room/ expansion finished, celebration
   - "The Collapse" — construction accident, structural failure
   - "The Renovation" — renovate old room, discover hidden space
   - "The Expansion" — break through to new area
   - "The Infrastructure" — install major system (power, water)
7. Add construction quest hooks:
   - "The Architect" — design and build new shelter wing
   - "The Engineer" — solve structural challenge
   - "The Discovery" — find hidden room during expansion
   - "The Crisis" — structural failure requires emergency repair
   - "The Masterpiece" — build legendary shelter feature
   - "The Legacy" — shelter expansion becomes shelter landmark
8. Implement construction resource management:
   - Construction consumes materials (concrete, steel, wood)
   - Construction consumes tools (drills, saws, welding equipment)
   - Construction consumes labor (survivor work days)
   - Resource shortage delays construction
   - Tool wear during construction
9. Implement construction skill system:
   - Survivors with construction skill build faster
   - Master builders can undertake complex projects
   - Construction skill improves with practice
   - Apprentices learn from master builders
10. Add UI: shelter map showing rooms, construction progress, expansion options
11. Create construction journal: automatic log of building projects
12. Implement construction tutorial: first construction explains system
13. Add construction tooltips: hover over room shows condition, upgrades
14. Create 20 blueprints and 10 room upgrades in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into shelter systems: new rooms integrate with existing shelter
2. Connect to `DutyRosterSystem`: construction labor assigned via duty
3. Integrate with `ShelterThermalSystem`: new rooms affect temperature
4. Connect to `PowerGridSystem`: new rooms connect to power
5. Wire into `VentilationSystem`: new rooms connect to air
6. Connect to `ShelterDefenseSystem` (Plan 138): new rooms add defense
7. Implement old-save compatibility: existing saves get default shelter state
8. Add deterministic seeding: construction uses `ISeededRng`
9. Create exploit prevention: construction requires real resources/time
10. Add tests: construction, renovation, expansion, save round-trip
11. Verify catalog integrity: all blueprint/room IDs resolve
12. Test edge cases: no construction (static shelter), max expansion (full shelter)
13. Verify headless behavior: construction processes correctly without UI
14. Add data-integrity-selftest: blueprints validate against room/resource catalogs
15. Create `--shelter-expansion-selftest` verb for CI validation

## State / System Interaction Model

```text
Shelter expansion/renovation
├─ Blueprint discovered (exploration, research, trade)
├─ Construction project planned
│  ├─ Select blueprint and location
│  ├─ Calculate resource/labor requirements
│  └─ Assign resources and labor
├─ Construction active
│  ├─ Resources consumed daily
│  ├─ Labor applied daily
│  ├─ Progress tracked
│  └─ Events (accidents, discoveries)
├─ Construction completed
│  ├─ New room available
│  ├─ Renovation finished
│  ├─ Expansion opened
│  └─ Celebration event
├─ Room integration
│  ├─ Connect to power/water/ventilation
│  ├─ Assign to duty roster
│  ├─ Apply upgrades
│  └─ Room provides benefits
└─ Shelter evolution
   ├─ Shelter grows (more rooms, deeper levels)
   ├─ Shelter improves (renovated, upgraded)
   ├─ Shelter becomes community (leisure, family housing)
   └─ Shelter legacy (landmarks, history)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-expansion-selftest
```

## Risk

**MEDIUM** — Shelter expansion complexity can overwhelm players if too many options exist. Risk of expansion making shelter management too complex. Mitigation: start with simple rooms, unlock complex construction gradually, make expansion optional (can play without expanding), and provide clear UI showing construction status.

## Definition of Done

- `ShelterExpansionSystem.cs` exists with full `CaptureState/RestoreState`
- 4 construction types implemented (new room, renovation, expansion, upgrade)
- 6 room types functional (living, storage, medical, work, leisure, infrastructure)
- Blueprint discovery and construction mechanics working
- Room upgrades and infrastructure integration
- Construction events and quest hooks
- Save/load round-trip tested
- Deterministic construction outcomes verified
- Old saves load without error
- 20 blueprints + 10 room upgrades in data authority
- UI map shows shelter layout and construction
- Cross-system integration (duty roster, thermal, power, ventilation, defense)

## Follow-On Opportunities

- Shelter architecture styles (different aesthetic themes)
- Shelter landmarks (famous rooms become shelter history)
- Shelter tours (visitors tour your shelter)
- Shelter legacy (shelter design carries to New Game+)
- Shelter disasters (structural failures, fires, floods)
