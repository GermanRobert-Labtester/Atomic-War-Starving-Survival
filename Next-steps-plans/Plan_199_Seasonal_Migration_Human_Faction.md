# Plan 199 — Seasonal Migration (Human/Faction) System

## Goal

Create a seasonal migration system where human populations (refugees, traders, faction members) and caravans follow seasonal movement patterns — migrating to safer/more prosperous areas during harsh seasons and returning when conditions improve. Currently `WildlifeMigrationSystem.cs` (125 lines + `.Live.cs`) tracks wildlife population migration, and `TravelingCaravanSystem.cs` (268 lines) runs NPC caravans on fixed routes, but there is no seasonal human migration — no refugee flows, no seasonal trader movements, no faction relocations, no population shifts based on weather/seasons. The world's human population is static regardless of season. This plan adds demographic realism and creates seasonal gameplay variation.

## Why

**Repository evidence:** Grep for `SeasonalMigration`, `MigrationRoute`, `SeasonalRoute`, `CaravanMigration`, `PopulationMigration`, `SeasonalMovement` in Core returns ZERO matches. `WildlifeMigrationSystem.cs` (125 lines + `.Live.cs`) tracks wildlife migration patterns. `TravelingCaravanSystem.cs` (268 lines) runs NPC caravans on pre-set routes with `SpawnCaravan`, `DailyTick`, `TryBuyItem` — but routes are fixed, not seasonal. Plan 135 (Weather Deep Gameplay Cascade) mentions "Seasonal migration patterns (faction/npc movement)" as a follow-on opportunity but doesn't implement it. Plan 38 (The Year Turns) mentions `WildlifeMigrationSystem` is ticked under `world_evolution` but no human migration exists.

**What is missing:** No seasonal human migration. No refugee flows based on seasons. No seasonal trader movements. No faction relocations. No population shifts based on weather/seasons. Human populations are static regardless of season. The world doesn't feel alive with seasonal demographic changes.

**Why existing plans don't solve it:** Plan 135 (weather cascade) mentions seasonal migration as follow-on but doesn't implement. Plan 164 (nuclear winter) adds seasonal progression but not migration. Plan 160 (expedition colony) adds static colonies but not seasonal movement. Plan 192 (trade routes) adds player trade but not seasonal NPC movement. No plan addresses seasonal human migration as a system.

**Player value:** Creates seasonal variation (world feels different each season), adds strategic depth (plan around migration patterns), generates emergent stories (refugee crises, seasonal trade booms), and makes the world feel alive with moving populations.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` — wildlife migration
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs` — NPC caravans
- `Assets/Ashfall.Core/World/WeatherSystem.cs` — weather/seasons
- `Assets/Ashfall.Core/Clock/ISimClock.cs` — time tracking
- NEW: `Assets/Ashfall.Core/Migration/SeasonalMigrationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/migration_routes.json`

## Main Task 1 — Foundation / System Contract

1. Create `SeasonalMigrationSystem.cs` in `Assets/Ashfall.Core/Migration/`
2. Define `MigrationRoute` DTO: `routeId`, `routeName`, `migrationType` (refugee_flow/trader_circuit/faction_relocation/nomadic_circuit), `seasonalPattern` (spring_summer/fall_winter/year_round/opposite_season), `originRegion` (location_id or region), `destinationRegion` (location_id or region), `populationSize` (number of migrants), `departureMonth` (1-12), `arrivalMonth` (1-12), `returnMonth` (1-12, -1 if one-way)
3. Define `MigrantGroup` DTO: `groupId`, `routeId`, `groupSize` (current number of migrants), `composition` (families/traders/fighters/mixed), `leaderId` (survivor_id or faction_id), `departureDay`, `currentLocation` (location_id), `destination` (location_id), `status` (en_route/arrived/returning/settled), `morale` (0-100), `supplies` (0-100)
4. Define `SeasonalPattern` DTO: `patternId`, `patternName`, `triggerSeason` (spring/summer/fall/winter), `triggerCondition` (temperature_threshold/radiation_level/resource_scarcity/faction_pressure), `migrationDirection` (north/south/east/west/to_shelter/from_shelter), `populationAffected` (refugees/traders/factions/nomads)
5. Define `MigrationEvent` DTO: `eventId`, `eventType` (departure/arrival/return/settlement/conflict/scarcity), `groupId`, `day`, `locationId`, `description`, `effects` (list of consequences)
6. Define `SeasonalMigrationState` DTO: list of migration routes, list of active migrant groups, list of seasonal patterns, list of migration events, migration settings (migration intensity modifier, refugee acceptance bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define migration types (4+ types):
   - **Refugee Flow**: populations flee harsh conditions (radiation storms, nuclear winter, faction conflicts), move toward shelter/safer areas, temporary settlement
   - **Trader Circuit**: traders follow seasonal routes, visit settlements on schedule, bring goods/news, return to home base
   - **Faction Relocation**: factions move operations seasonally (summer camps, winter bunkers), strategic repositioning
   - **Nomadic Circuit**: nomadic groups follow resource availability, circular migration patterns, visit settlements periodically
9. Define seasonal patterns:
   - **Spring**: migration northward, refugees return home, traders resume circuits
   - **Summer**: peak trade season, nomadic groups active, faction operations expand
   - **Fall**: migration southward, refugees seek shelter, traders stockpile
   - **Winter**: migration to shelter, refugees crowd bunkers, trade slows, factions hunker down
   - **Nuclear Winter**: extended winter patterns, constant migration toward shelter, severe resource scarcity
10. Define migration triggers:
    - **Temperature**: cold temperatures trigger southward migration
    - **Radiation**: high radiation triggers refugee flows
    - **Resource Scarcity**: low resources trigger nomadic movement
    - **Faction Pressure**: faction conflicts trigger refugee flows
    - **Seasonal**: calendar-based migration patterns
11. Define migrant group mechanics:
    - Groups have size, composition, leader, morale, supplies
    - Groups move along routes (daily progress)
    - Groups consume supplies during travel
    - Low supplies: morale drops, group may scatter
    - Groups arrive at destination, settle temporarily
    - Groups return on schedule (if round-trip route)
12. Define migration consequences:
    - Refugee arrivals: population increase, resource demand, potential conflict
    - Trader arrivals: trade opportunities, news, goods
    - Faction relocations: strategic shifts, territory changes
    - Nomadic visits: trade, information, cultural exchange
    - Migration conflicts: resource competition, cultural friction
13. Define player interaction:
    - Player can accept/reject refugee groups
    - Player can trade with trader circuits
    - Player can negotiate with relocating factions
    - Player can hire nomadic guides
    - Player can establish migration routes (Plan 192 integration)
14. Add deterministic seeding: migration uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupSeasonalMigration`, `TickSeasonalMigration`, `SaveSeasonalMigration`

## Main Task 2 — Implementation / Routes / Groups / Patterns / Events / UI

1. Implement migration routes:
   - Define routes with origin, destination, seasonal pattern
   - Routes activate based on season/conditions
   - Migrant groups follow routes
   - Route progress tracked daily
   - Routes logged
2. Implement migrant groups:
   - Groups spawn on active routes
   - Groups have size, composition, morale, supplies
   - Groups move daily toward destination
   - Groups consume supplies
   - Groups arrive at destination
   - Groups settle or return
3. Implement seasonal patterns:
   - Patterns trigger based on season/conditions
   - Patterns activate migration routes
   - Patterns affect migration intensity
   - Patterns logged
4. Implement migration events:
   - Departure: group leaves origin
   - Arrival: group reaches destination
   - Return: group returns to origin
   - Settlement: group settles permanently
   - Conflict: groups compete for resources
   - Scarcity: group runs low on supplies
   - Events logged
5. Implement refugee flows:
   - Refugees flee harsh conditions
   - Refugees move toward shelter/safer areas
   - Refugees arrive at settlements
   - Player can accept/reject refugees
   - Accepted refugees: population increase, resource demand
   - Rejected refugees: move on, may return later
6. Implement trader circuits:
   - Traders follow seasonal routes
   - Traders visit settlements on schedule
   - Traders bring goods/news
   - Player can trade with traders
   - Traders return to home base
   - Trader circuits create predictable trade opportunities
7. Implement faction relocations:
   - Factions move operations seasonally
   - Factions establish summer/winter bases
   - Faction relocations affect territory control
   - Player can negotiate with relocating factions
   - Relocations create strategic opportunities
8. Implement nomadic circuits:
   - Nomadic groups follow resource availability
   - Nomads visit settlements periodically
   - Nomads bring trade/information
   - Player can hire nomadic guides
   - Nomadic circuits create cultural exchange
9. Implement migration UI:
   - Migration map: show active routes and groups
   - Group detail: size, composition, morale, supplies, destination
   - Route panel: all routes with seasonal patterns
   - Event log: migration events
   - Refugee panel: incoming refugee groups, accept/reject
   - Trader panel: incoming trader groups, trade options
10. Create migration events:
    - "The Exodus" — large refugee flow
    - "The Arrival" — migrant group arrives
    - "The Return" — group returns home
    - "The Settlement" — group settles permanently
    - "The Conflict" — migration conflict
    - "The Scarcity" — group runs low on supplies
    - "The Trade" — trader circuit arrives
    - "The Relocation" — faction relocates
11. Add migration quest hooks:
    - "The Host" — accept 10 refugee groups
    - "The Trader" — trade with 20 trader circuits
    - "The Guide" — hire 5 nomadic guides
    - "The Diplomat" — negotiate with 3 relocating factions
    - "The Route" — establish 5 migration routes
    - "The Settlement" — help 3 refugee groups settle
    - "The Network" — maintain trade with 10 trader circuits
12. Implement migration tutorial: first refugee arrival explains system
13. Add migration tooltips: hover over group shows details
14. Create migration route definitions in data file (10+ routes)
15. Implement migration persistence: routes/groups saved with game state

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `WildlifeMigrationSystem`: coordinate human/wildlife migration
2. Connect to `TravelingCaravanSystem`: integrate trader circuits
3. Integrate with `WeatherSystem`: weather triggers migration
4. Connect to `FactionBranchCoordinator`: faction relocations affect branches
5. Wire into `ExpeditionSystem`: migration affects expedition safety
6. Connect to `HoldfastTradeSession`: trader arrivals affect trade
7. Implement old-save compatibility: existing saves get no active migration
8. Add deterministic seeding: migration uses `ISeededRng`
9. Create exploit prevention: migration is season/condition-based, can't be gamed
10. Add tests: migration routes, groups, patterns, events, refugee flows, trader circuits, save round-trip
11. Verify all migration types work correctly
12. Test edge cases: no migration (stable season), heavy migration (crisis)
13. Verify headless behavior: migration processes correctly without UI
14. Add data-integrity-selftest: migration validates against location/faction catalogs
15. Create `--seasonal-migration-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --seasonal-migration-selftest
```

## Risk

**LOW** — Seasonal migration is straightforward with clear inputs (seasons, conditions) and outputs (group movement, events). Risk of migration feeling like background noise rather than meaningful gameplay. Mitigation: make player interaction meaningful (accept/reject refugees, trade with traders), show clear consequences, and ensure migration creates opportunities not just problems.

## Definition of Done

- `SeasonalMigrationSystem.cs` exists with full `CaptureState/RestoreState`
- 4+ migration types (refugee flow, trader circuit, faction relocation, nomadic circuit)
- Seasonal patterns (spring/summer/fall/winter/nuclear winter)
- Migration triggers (temperature, radiation, resources, faction pressure, seasonal)
- Migrant group mechanics (size, composition, morale, supplies, movement)
- Migration consequences (population changes, trade, conflicts, settlements)
- Player interaction (accept/reject refugees, trade with traders, negotiate with factions)
- Migration events and quest hooks
- Save/load round-trip tested
- Deterministic migration verified
- Old saves load with no active migration
- Migration route definitions in data authority
- UI migration map, group detail, route panel, event log, refugee/trader panels
- Cross-system integration (wildlife migration, caravans, weather, factions, expedition, trade)

## Follow-On Opportunities

- Migration specialization (survivors become expert guides/diplomats)
- Migration legacy (famous migrations remembered)
- Migration quests (specific migration goals)
- Migration events (mass exodus, legendary trader circuit)
- Migration trading (trade migration routes between settlements)
