# Plan 134 — Dynamic Faction Territory & Supply Line Control

## Goal

Create a dynamic territory-control system where factions expand, contract, and compete for geographic control based on military strength, economic pressure, player actions, and world events. Add supply-line logistics connecting faction holdings, creating vulnerable corridors that can be raided, negotiated, or severed. This transforms factions from static data into living geopolitical actors.

## Why

**Repository evidence:** `FactionWarSystem` (referenced in `PrpfStandingSystem.cs` and `Factions/` directory) handles faction standing and war states. `RegionalTreatySystem.cs` (176 lines) manages treaties between factions with compliance tracking. `FactionBranchCoordinator.cs` coordinates military/rebel/independent branches. `TravelingCaravanSystem.cs` (268 lines) moves goods between locations. `WaystationSystem.cs` (200 lines) provides rest stops. But no system connects these into dynamic territory control or supply-line logistics.

**What is missing:** Factions don't expand or contract territory based on player actions, economic pressure, or military outcomes. No supply lines connect faction holdings. No "faction X now controls this region" gameplay. No sieges, no territorial negotiation, no supply raids. Factions are static data, not dynamic actors.

**Why existing plans don't solve it:** Plan 44 (faction territory map) adds static territory data. Plan 45 (faction patrol encounters) adds patrol combat. Plan 92 (faction war dialogue) adds war dialogue content. Plan 124 (faction war location overrides) adds location-specific war content. None address dynamic territory change or supply-line logistics.

**Player value:** Creates strategic depth (ally with expanding faction, raid rival supply lines), makes faction conflicts visible and impactful, provides new gameplay options (supply line raiding, territorial defense, siege warfare).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction branch coordination
- `Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs` — military faction logic
- `Assets/Ashfall.Core/Factions/RebelBranchSystem.cs` — rebel faction logic
- `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs` — independent faction logic
- `Assets/Ashfall.Core/RegionalTreatySystem.cs` — treaty framework
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs` — caravan movement
- `Assets/Ashfall.Core/WaystationSystem.cs` — waystation network
- `Assets/StreamingAssets/Data/faction_lore.json` — faction definitions
- `Assets/StreamingAssets/Data/locations.json` — location data
- NEW: `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`
- NEW: `Assets/Ashfall.Core/Factions/SupplyLineSystem.cs`
- NEW: `Assets/StreamingAssets/Data/faction_territory.json`
- NEW: `Assets/StreamingAssets/Data/supply_lines.json`

## Main Task 1 — Foundation / System Contract

1. Create `TerritoryControlSystem.cs` in `Assets/Ashfall.Core/Factions/`
2. Define `TerritoryState` DTO: `locationId`, `controllingFactionId`, `controlStrength` (0-100), `contested` bool, `lastContestDay`, `fortificationLevel` (0-3), `garrisonSize`
3. Define `TerritoryControlState` DTO: map of location → territory state, list of contested locations, historical control log
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define territory change rules:
   - Factions expand via military pressure (garrison size + control strength)
   - Economic pressure: faction with trade presence gains influence
   - Player actions: completing faction quests increases control
   - World events: weather, radiation, resource depletion affect control
6. Create contest mechanic: multiple factions in same location triggers contest
7. Define contest resolution: control strength comparison + seeded RNG for outcomes
8. Create `SupplyLineSystem.cs` in `Assets/Ashfall.Core/Factions/`
9. Define `SupplyLine` DTO: `id`, `owningFactionId`, `originLocationId`, `destinationLocationId`, `route` (list of waypoint location IDs), `status` (active/disrupted/destroyed), `lastSupplyDay`, `cargoValue`
10. Define `SupplyLineState` DTO: list of active supply lines, list of disrupted lines
11. Implement supply line mechanics:
    - Lines connect faction holdings, deliver resources periodically
    - Lines can be raided (expedition encounter), disrupted (weather, threats), or destroyed (military action)
    - Disrupted lines reduce faction control strength at destination
    - Destroyed lines require rebuilding (cost: resources, time)
12. Create `ISupplyLineSource` interface for faction systems to request supply lines
13. Wire territory changes into `LocationEvolutionSystem`: territory updates location mutation records
14. Add deterministic seeding: territory changes and supply line raids use `ISeededRng`
15. Wire into `GameBootstrap`: `SetupTerritoryControl`, `TickTerritory`, `SaveTerritory`

## Main Task 2 — Implementation / Content / Territory Dynamics

1. Implement faction expansion AI:
   - Military faction: expands via garrison placement, fortifies holdings
   - Rebel faction: expands via insurgency, undermines enemy control
   - Independent faction: expands via economic influence, trade presence
   - PRPF: expands via hidden recruitment, positive alignment (if Plan 131 rumor network active)
2. Create territory contest events:
   - Faction A attempts to take location from Faction B
   - Player can intervene (support defender, support attacker, stay neutral)
   - Outcome affects faction standing and territorial control
3. Implement supply line establishment:
   - Faction requests player assistance to establish supply line
   - Player must clear route (expedition), escort caravan, or negotiate with intermediate factions
   - Successful establishment provides faction standing + ongoing resource flow
4. Create supply line raiding:
   - Player can raid rival faction supply lines (expedition encounter)
   - Success: loot cargo, disrupt line, rival faction standing -
   - Failure: combat loss, faction standing --
   - Moral choice: raiding civilian supplies vs. military supplies
5. Implement siege mechanic:
   - Faction besieges rival holding (cuts supply lines, increases pressure)
   - Player can break siege (military expedition), negotiate, or profit from scarcity
   - Siege outcome affects territorial control and faction standing
6. Create territorial negotiation:
   - Player mediates territorial disputes between factions
   - Success: faction standing ++, regional stability
   - Failure: factions become hostile to player
7. Implement fortification system:
   - Player can invest resources to fortify faction holdings
   - Fortified locations resist contest attempts
   - Fortification levels: 0 (none), 1 (basic), 2 (reinforced), 3 (fortress)
8. Create garrison management:
   - Factions request player assistance to garrison contested locations
   - Player assigns survivors or resources to garrison duty
   - Garrison size affects control strength
9. Implement territorial economy:
   - Locations under faction control produce resources for faction
   - Player can tax trade in faction-controlled locations (standing gain)
   - Faction economic strength affects military expansion capability
10. Create territorial events:
    - Rebellion: low-control location revolts against controlling faction
    - Defection: garrison switches allegiance (if morale low)
    - Natural disaster: radiation storm / weather event reduces control
    - Resource boom: new resource discovery increases location value
11. Add UI: "Territory Map" panel showing faction control colors, contested zones, supply lines
12. Create "Supply Line Status" panel showing active/disrupted/destroyed lines
13. Implement territorial intelligence: player can scout faction movements (expedition)
14. Add territorial diplomacy: player can propose territorial agreements between factions
15. Create 10 territorial dispute scenarios in `faction_territory.json`

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `LocationEvolutionSystem`: territory changes update location mutation records (owner, fortification, threats)
2. Connect to faction standing systems: territorial outcomes modify faction standing
3. Integrate with economy: territorial control affects resource availability and prices
4. Connect to quest system: territorial disputes unlock faction quests
5. Wire into caravan system: supply lines use caravan infrastructure, disrupted lines affect caravan routing
6. Connect to expedition system: supply line raids are expedition encounters
7. Implement old-save compatibility: existing saves get default territory state (factions start with lore-defined holdings)
8. Add deterministic seeding: territory changes use `ISeededRng`
9. Create exploit prevention: territory changes have cooldowns, can't be farmed
10. Add tests: territory contest resolution, supply line lifecycle, save round-trip, determinism
11. Verify catalog integrity: all territory location IDs and faction IDs resolve
12. Test edge cases: single faction dominates (no contests), all supply lines destroyed (faction collapse)
13. Verify headless behavior: territory ticks correctly without UI
14. Add data-integrity-selftest: territory templates validate against location/faction catalogs
15. Create `--territory-control-selftest` verb for CI validation

## State / System Interaction Model

```text
Faction expansion pressure
├─ Military: garrison placement, fortification
│  ├─ Contest triggered if location already controlled
│  │  ├─ Player intervenes (support attacker/defender)
│  │  │  ├─ Success: standing +, territorial outcome
│  │  │  └─ Failure: standing -, no change
│  │  └─ Player stays neutral
│  │     ├─ Attacker wins: new control, player missed opportunity
│  │     └─ Defender holds: status quo, attacker frustrated
│  └─ No contest: faction expands into uncontrolled territory
├─ Economic: trade presence, resource control
│  ├─ Supply lines established (player-assisted or autonomous)
│  │  ├─ Active: resources flow, faction strength increases
│  │  ├─ Disrupted: resources reduced, faction weakens
│  │  └─ Destroyed: faction loses control of destination
│  └─ Supply line raiding (player action)
│     ├─ Success: loot, disruption, rival weakens
│     └─ Failure: combat loss, rival alerted
├─ Player actions: quests, diplomacy, investment
│  ├─ Quest completion: faction control strength +
│  ├─ Diplomatic mediation: contested location resolved
│  ├─ Fortification investment: location resists contest
│  └─ Garrison support: control strength increased
└─ World events: weather, radiation, resource depletion
   ├─ Natural disaster: control strength reduced
   ├─ Resource boom: location value increased, contest likely
   └─ Rebellion: low-control location revolts
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --territory-control-selftest
```

## Risk

**HIGH** — Territory control complexity can spiral with multiple factions, contested locations, and supply lines. Risk of performance issues if too many territory updates per tick. Risk of player confusion if territory changes too rapidly. Mitigation: cap contested locations, implement territory change cooldowns, provide clear UI feedback.

## Definition of Done

- `TerritoryControlSystem.cs` exists with full `CaptureState/RestoreState`
- `SupplyLineSystem.cs` exists with full `CaptureState/RestoreState`
- Factions expand/contract territory based on military/economic/player pressure
- Territory contest mechanic with player intervention options
- Supply lines connect faction holdings, can be raided/disrupted/destroyed
- Siege and fortification mechanics functional
- Territorial negotiation and diplomacy options
- Save/load round-trip tested
- Deterministic territory changes verified
- Old saves load without error (default territory state)
- UI panels show territory map and supply line status
- Cross-system integration (location evolution, factions, economy, quests, caravans, expeditions)

## Follow-On Opportunities

- Naval supply lines (maritime expansion)
- Underground supply routes (tunnel networks)
- Mercenary factions (hire military support for territorial disputes)
- Territorial sanctions (economic warfare)
- Independence movements (locations secede from faction control)
