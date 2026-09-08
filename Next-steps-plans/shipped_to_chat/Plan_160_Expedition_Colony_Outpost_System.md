# Plan 160 — Expedition Colony & Outpost System

## Goal

Create an expedition colony and outpost system where players can establish permanent settlements at expedition destinations, maintain supply lines, defend outposts, and expand their influence across the wasteland. Currently expeditions are in-and-out affairs — players travel to a location, scavenge, and return. There is no option to stay, build, or maintain a presence. This plan adds persistent expedition presence and territorial expansion.

## Why

**Repository evidence:** `ExpeditionSystem.cs` handles travel, scavenging, encounters, and return. `ExpeditionVehicleSystem.cs` (193 lines) manages vehicle logistics. `LocationEvolutionSystem.cs` (133 + 160 lines) tracks location mutations. But expeditions are temporary — players visit, loot, leave. No system supports establishing colonies, building outposts, maintaining permanent presence, or territorial expansion. Plan 133 (expedition consequences) connects discoveries to world state but doesn't add permanent presence.

**What is missing:** Players cannot stay at expedition destinations. They cannot build outposts, establish colonies, or maintain a permanent presence away from the shelter. All expedition value is extracted through loot and returned to the shelter. There is no territorial expansion, no forward operating base, no wasteland colonization.

**Why existing plans don't solve it:** Plan 32 (expedition destination wiring) connects destinations but not permanent presence. Plan 133 (discovery consequences) adds world state changes but not colonies. Plan 152 (vehicle customization) adds mobile bases but not fixed outposts. Plan 156 (shelter expansion) expands the shelter but not outward presence. No plan addresses expedition colonies or outposts.

**Player value:** Creates strategic expansion (establish forward bases), adds territorial control (claim and hold locations), provides new gameplay (colony management, supply lines, outpost defense), and makes the world feel conquerable (players can spread their influence across the wasteland).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition mechanics
- `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` — vehicle logistics
- `Assets/Ashfall.Core/LocationEvolutionSystem.cs` — location state
- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — trade/economy
- `Assets/StreamingAssets/Data/expeditions.json` — expedition destinations
- NEW: `Assets/Ashfall.Core/Expeditions/ColonySystem.cs`
- NEW: `Assets/StreamingAssets/Data/colony_blueprints.json`

## Main Task 1 — Foundation / System Contract

1. Create `ColonySystem.cs` in `Assets/Ashfall.Core/Expeditions/`
2. Define `Colony` DTO: `colonyId`, `locationId`, `colonyType` (outpost/settlement/fortress/trading_post/farming_commune), `name`, `population` (list of survivor IDs), `buildings` (list of building IDs), `resources` (map of resource → quantity), `defense` (0-100), `morale` (0-100), `establishedDay`
3. Define `ColonyBuilding` DTO: `buildingId`, `buildingType` (housing/storage/medical/work/defense/infrastructure), `condition` (0-100), `capacity`, `effects` (list of modifiers), `constructionDay`
4. Define `SupplyLine` DTO: `lineId`, `originColonyId`, `destinationColonyId` (or shelter), `route` (list of location IDs), `status` (active/disrupted/destroyed), `cargoCapacity`, `lastSupplyDay`
5. Define `ColonyState` DTO: list of colonies, list of supply lines, colony blueprints discovered, territorial influence map
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define colony types:
   - **Outpost**: small military presence, scouting, early warning
   - **Settlement**: civilian population, resource production, community
   - **Fortress**: heavily defended, military stronghold, territorial control
   - **Trading post**: commerce hub, trade with factions and travelers
   - **Farming commune**: food production, agriculture, self-sustaining
8. Define colony establishment mechanics:
   - Player selects expedition destination for colony
   - Colony requires initial investment (resources, survivors, time)
   - Colony construction takes days to weeks
   - Colony becomes operational when minimum buildings complete
   - Colony can expand over time (more buildings, population)
9. Define colony management mechanics:
   - Population management: assign roles (worker, soldier, leader)
   - Resource management: production, consumption, storage
   - Defense management: fortifications, garrison, alerts
   - Morale management: housing quality, food, safety, community
   - Supply management: request supplies from shelter, send supplies back
10. Define supply line mechanics:
    - Supply lines connect colonies to shelter and each other
    - Supply lines require vehicles/caravans (Plan 152 integration)
    - Supply lines can be disrupted (weather, raids, distance)
    - Supply lines deliver resources, personnel, equipment
    - Supply lines can be defended (military escort)
11. Define territorial influence:
    - Colonies project influence over surrounding area
    - Influence radius depends on colony size and type
    - Influence provides benefits (resource access, safety, trade)
    - Influence contested by factions and other colonies
    - Influence can be expanded through more colonies
12. Add deterministic seeding: colony outcomes use `ISeededRng`
13. Wire into `GameBootstrap`: `SetupColony`, `TickColony`, `SaveColony`
14. Create `ColonyBlueprintCatalogLoader` for building definitions
15. Create UI hook: colony map showing colonies, supply lines, influence

## Main Task 2 — Implementation / Colonies / Buildings / Supply / Defense

1. Implement colony establishment:
   - Player selects location and colony type
   - Assign initial survivors and resources
   - Construction begins (buildings erected over time)
   - Colony becomes operational when core buildings complete
   - Colony can accept new settlers from shelter
2. Implement colony buildings:
   - **Housing**: population capacity, morale bonus
   - **Storage**: resource capacity, preservation
   - **Medical**: healthcare, injury treatment
   - **Work**: production facilities (workshop, farm, mine)
   - **Defense**: fortifications, garrison, watchtowers
   - **Infrastructure**: power, water, communications
   - Buildings can be upgraded for better effects
3. Implement colony resource management:
   - Colonies produce resources (food, materials, goods)
   - Colonies consume resources (food, supplies, equipment)
   - Surplus can be sent to shelter or other colonies
   - Shortage requires supply line or local production
   - Resource balance affects colony growth and morale
4. Implement colony defense:
   - Colonies can be attacked (raiders, factions, wildlife)
   - Defense rating based on fortifications and garrison
   - Colony defense uses `ShelterDefenseSystem` (Plan 138) mechanics
   - Successful defense: colony safe, morale boost
   - Failed defense: colony damaged, resources lost, population casualties
5. Implement supply line management:
   - Establish supply line between colonies/shelter
   - Assign vehicles/caravans to supply line
   - Supply line delivers resources on schedule
   - Supply line can be disrupted (weather, raids)
   - Disrupted supply line: colony isolated, must self-sustain
   - Supply line can be defended (military escort)
6. Implement territorial influence:
   - Colony projects influence over surrounding area
   - Influence radius: 5-20 km depending on colony size
   - Influence provides: resource access, safety, trade bonuses
   - Influence contested by: faction presence, other colonies, hazards
   - Influence can be expanded: more colonies, stronger presence
7. Implement colony population:
   - Survivors assigned to colony (voluntary or drafted)
   - Colony population grows (births, refugees, recruitment)
   - Population roles: workers, soldiers, leaders, specialists
   - Population morale affected by: conditions, safety, leadership
   - Population can revolt if morale too low
8. Create colony events:
   - "The Founding" — establish new colony
   - "The Growth" — colony expands, new buildings
   - "The Siege" — colony under attack, defend or fall
   - "The Supply Crisis" — supply line cut, colony isolated
   - "The Boom" — colony prospers, population grows
   - "The Revolt" — colony population revolts against leadership
   - "The Connection" — establish supply line to new colony
9. Add colony quest hooks:
   - "The Pioneer" — establish first colony in dangerous territory
   - "The Lifeline" — maintain supply line through hostile area
   - "The Siege" — defend colony against overwhelming odds
   - "The Expansion" — grow small outpost into thriving settlement
   - "The Rebellion" — put down colony revolt
   - "The Alliance" — form alliance with faction near colony
   - "The Legacy" — colony becomes permanent wasteland landmark
10. Implement colony integration:
    - Colonies integrate with expedition system (base for operations)
    - Colonies integrate with faction system (territorial claims)
    - Colonies integrate with economy (trade hubs, production)
    - Colonies integrate with communications (Plan 157, network nodes)
    - Colonies integrate with governance (Plan 159, local administration)
11. Add UI: colony map showing colonies, supply lines, influence zones
12. Create colony journal: automatic log of colony events
13. Implement colony tutorial: first colony explains system
14. Add colony tooltips: hover over colony shows status, population
15. Create 15 colony buildings and 10 colony blueprints in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ExpeditionSystem`: colonies serve as expedition bases
2. Connect to `LocationEvolutionSystem`: colonies affect location state
3. Integrate with `FactionBranchCoordinator`: colonies claim territory
4. Connect to `MarketSystem`: colonies participate in economy
5. Wire into `CommunicationsSystem` (Plan 157): colonies join network
6. Connect to `GovernanceSystem` (Plan 159): colonies governed
7. Implement old-save compatibility: existing saves get empty colony state
8. Add deterministic seeding: colony outcomes use `ISeededRng`
9. Create exploit prevention: colonies require real investment, can't be infinite
10. Add tests: colony establishment, supply lines, defense, save round-trip
11. Verify catalog integrity: all colony/location/survivor IDs resolve
12. Test edge cases: no colonies (shelter only), many colonies (vast territory)
13. Verify headless behavior: colonies process correctly without UI
14. Add data-integrity-selftest: colony blueprints validate against catalogs
15. Create `--colony-selftest` verb for CI validation

## State / System Interaction Model

```text
Colony establishment and management
├─ Colony founded at expedition destination
│  ├─ Select location and type
│  ├─ Assign survivors and resources
│  ├─ Construction over days/weeks
│  └─ Colony becomes operational
├─ Colony management
│  ├─ Population: assign roles, manage morale
│  ├─ Resources: production, consumption, storage
│  ├─ Defense: fortifications, garrison, alerts
│  └─ Buildings: construct, upgrade, maintain
├─ Supply lines
│  ├─ Connect colonies to shelter and each other
│  ├─ Deliver resources on schedule
│  ├─ Can be disrupted (weather, raids)
│  └─ Can be defended (military escort)
├─ Territorial influence
│  ├─ Colony projects influence over area
│  ├─ Influence provides benefits
│  ├─ Influence contested by factions/others
│  └─ Influence expanded through more colonies
├─ Colony events
│  ├─ Growth, siege, supply crisis, boom, revolt
│  ├─ Connection to other colonies
│  └─ Legacy as permanent landmark
└─ Integration
   ├─ Expedition base (launch operations)
   ├─ Territorial claim (faction system)
   ├─ Trade hub (economy)
   ├─ Network node (communications)
   └─ Local administration (governance)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --colony-selftest
```

## Risk

**HIGH** — Colony system complexity can overwhelm players if too many colonies, buildings, and supply lines exist. Risk of colonies becoming a second shelter management layer (too much micromanagement). Mitigation: start with one outpost, unlock colony types gradually, automate colony management where possible, and make colonies optional (can play without them).

## Definition of Done

- `ColonySystem.cs` exists with full `CaptureState/RestoreState`
- 5 colony types implemented (outpost, settlement, fortress, trading post, farming commune)
- Colony establishment and management functional
- 6 building types with construction and upgrades
- Supply line system working (establish, deliver, disrupt, defend)
- Territorial influence mechanics
- Colony defense integrated with shelter defense
- Colony events and quest hooks
- Save/load round-trip tested
- Deterministic colony outcomes verified
- Old saves load without error
- 15 colony buildings + 10 colony blueprints in data authority
- UI map shows colonies, supply lines, influence
- Cross-system integration (expedition, location, factions, economy, communications, governance)

## Follow-On Opportunities

- Colony specialization (colonies become unique through specializations)
- Colony federation (colonies form alliance, shared governance)
- Colony legacy (famous colonies remembered in epilogue)
- Colony quests (establish specific colony types, overcome challenges)
- Colony warfare (offensive operations from colonies)
