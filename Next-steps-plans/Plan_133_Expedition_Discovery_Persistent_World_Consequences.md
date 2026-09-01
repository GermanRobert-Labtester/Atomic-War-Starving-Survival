# Plan 133 — Expedition Discovery → Persistent World Consequences

## Goal

Create a persistent consequence system where expedition discoveries permanently alter the world map, faction relations, economy, and quest availability. Discoveries produce ripple effects: finding a resource deposit triggers faction interest, clearing a threat makes routes safer, uncovering ruins attracts scavengers. The world remembers what the player has found and done.

## Why

**Repository evidence:** `expeditions.json` (759 bytes, 2 entries) defines expedition destinations but has no consequence tracking. `LocationEvolutionSystem.cs` (133 + 160 lines) tracks mutations (owner, contamination, loot depletion) but mutations don't produce visible gameplay changes. `ExpeditionVehicleSystem.cs` (193 lines) handles vehicles but not discovery consequences. `TravelingCaravanSystem.cs` (268 lines) moves goods but doesn't react to discoveries. No system connects expedition outcomes to persistent world state changes.

**What is missing:** Expeditions are one-way: player goes, gets loot, returns. Discoveries don't alter the world. A player who finds a valuable resource deposit doesn't trigger faction competition. Clearing a bandit camp doesn't make the route safer for caravans. The world is static despite player exploration.

**Why existing plans don't solve it:** Plan 32 (expedition destination wiring) connects destinations to the system. Plan 76 (expedition destinations expansion) adds more destinations. Plan 46 (scavenging tables) adds loot variety. Plan 85 (damaged map zones) adds zone data. None address persistent consequences from discoveries.

**Player value:** Makes exploration strategically meaningful. Players decide whether to reveal discoveries (attract faction interest) or keep them secret. Creates competition over resources. Makes the world feel responsive to player actions.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/LocationEvolutionSystem.cs` — mutation tracking foundation
- `Assets/StreamingAssets/Data/expeditions.json` — expedition definitions (2 entries)
- `Assets/StreamingAssets/Data/locations.json` — location data
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction reaction hooks
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs` — caravan routing
- `Assets/Ashfall.Core/Economy/` — economy system
- `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` — expedition logistics
- NEW: `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`
- NEW: `Assets/StreamingAssets/Data/discovery_consequences.json`

## Main Task 1 — Foundation / System Contract

1. Create `DiscoveryConsequenceSystem.cs` in `Assets/Ashfall.Core/Expeditions/`
2. Define `Discovery` DTO: `id`, `locationId`, `discoveryType` (resource/threat/ruins/faction_contact/strategic), `discoverySubtype`, `discoveredDay`, `exploited` bool, `revealed` bool, `consequenceTriggered` bool, `tags`
3. Define `DiscoveryConsequenceState` DTO: list of discoveries, list of triggered consequences, faction awareness map
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define discovery types with distinct consequence chains:
   - **Resource deposit**: triggers faction interest, economic impact, settlement demand
   - **Threat cleared**: improves route safety, attracts settlers, reduces caravan risk
   - **Ruins uncovered**: attracts scavengers, reveals lore, triggers archaeological quests
   - **Faction contact**: establishes diplomatic channel, triggers standing changes
   - **Strategic location**: triggers military interest, fortification options
6. Create `IDiscoverySource` interface for expedition system to report discoveries
7. Implement consequence trigger rules: each discovery type has consequence conditions
8. Create faction awareness system: factions learn about discoveries over time (via rumor network — Plan 131 integration point)
9. Define consequence escalation: ignored discoveries attract more attention
10. Create discovery concealment mechanic: player can hide discoveries (cost: forego benefits, avoid faction interest)
11. Wire into `LocationEvolutionSystem`: discoveries update location mutation records
12. Add deterministic seeding: consequence timing uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupDiscoveryConsequences`, `TickConsequences`, `SaveConsequences`
14. Create `DiscoveryCatalogLoader` for static discovery templates

## Main Task 2 — Implementation / Content / Consequences

1. Implement resource deposit discovery:
   - Player discovers resource (e.g., copper vein, clean water source)
   - Consequence chain:
     - Shelter gains resource extraction option (passive income)
     - Faction awareness increases (if rumor network active)
     - Faction demands share (diplomatic choice)
     - Refusal → faction hostility, possible raid
     - Agreement → resource sharing, faction standing gain
     - Secret keeping → no faction interest but no trade benefits
2. Implement threat cleared discovery:
   - Player clears bandit camp / radiation zone / wildlife nest
   - Consequence chain:
     - Route safety improves (caravan success rate increases)
     - Location becomes attractor (NPC settlers arrive)
     - New trade options unlock at cleared location
     - Faction takes credit if unaware of player action
     - Player can claim credit (faction standing gain)
3. Implement ruins uncovered discovery:
   - Player discovers pre-war ruins with lore value
   - Consequence chain:
     - Archaeological quest unlocks (excavation mini-game)
     - Lore fragments added to journal
     - Scavenger interest increases (random encounters at ruins)
     - Faction offers to buy information
     - Player can excavate (cost: labor, reward: relics/lore)
4. Implement faction contact discovery:
   - Player encounters faction patrol/scout during expedition
   - Consequence chain:
     - Diplomatic channel established
     - Faction standing modified based on encounter outcome
     - Future expeditions in area have faction interaction chance
     - Faction offers quest (test of loyalty)
     - Player can avoid contact (stealth) or engage (diplomacy/combat)
5. Implement strategic location discovery:
   - Player finds defensible position / chokepoint / high ground
   - Consequence chain:
     - Fortification option unlocks (shelter expansion)
     - Military faction offers alliance for access
     - Rebel faction offers to sabotage if player refuses
     - Location can be garrisoned (defensive bonus)
     - Faction competition if multiple factions aware
6. Create 20 discovery templates in `discovery_consequences.json`
7. Implement discovery concealment: player can mark discovery as "hidden" (no consequences but no benefits)
8. Create discovery revelation: player can reveal hidden discoveries later (delayed consequences)
9. Implement consequence escalation timer: unaddressed discoveries attract more faction attention
10. Add UI: "Discovery Log" panel showing discoveries and consequence status
11. Create expedition report: post-expedition summary of discoveries and immediate choices
12. Implement discovery interaction: multiple discoveries at same location compound consequences
13. Add discovery decay: some discoveries become irrelevant over time (resource depleted, threat returned)
14. Create discovery trade: player can sell discovery information to factions for standing

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `LocationEvolutionSystem`: discoveries update location mutation records (owner, threat level, resource status)
2. Connect to faction systems: faction awareness of discoveries affects standing and diplomatic options
3. Integrate with economy: resource discoveries affect local prices and trade availability
4. Connect to quest system: discoveries unlock location-specific quests
5. Wire into caravan system: cleared threats improve caravan success rates on affected routes
6. Implement old-save compatibility: existing saves get empty discovery state, future expeditions generate discoveries
7. Add deterministic seeding: consequence timing uses `ISeededRng`
8. Create exploit prevention: discoveries are one-time events, can't be re-triggered by save/load
9. Add tests: discovery lifecycle (discover → conceal/reveal → consequence), save round-trip, determinism
10. Verify catalog integrity: all discovery location IDs resolve to real locations
11. Test edge cases: no expeditions (no discoveries), all discoveries concealed (no consequences)
12. Verify headless behavior: consequences tick correctly without UI
13. Add data-integrity-selftest: discovery templates validate against location/faction catalogs
14. Create `--discovery-consequences-selftest` verb for CI validation
15. Document consequence architecture for future expansion

## State / System Interaction Model

```text
Expedition discovers something
├─ Discovery recorded (type, location, day)
│  ├─ Player choice: conceal or reveal
│  │  ├─ Conceal: no consequences, no benefits
│  │  │  └─ Can reveal later (delayed consequences)
│  │  └─ Reveal: consequence chain triggers
│  │     ├─ Shelter benefit (resource, safety, lore)
│  │     ├─ Faction awareness increases
│  │     │  ├─ Faction demands share/access
│  │     │  │  ├─ Player agrees: standing +, resource shared
│  │     │  │  └─ Player refuses: standing -, possible raid
│  │     │  └─ Faction competition (multiple factions aware)
│  │     │     ├─ Play factions against each other
│  │     │     └─ Exclusive alliance with one faction
│  │     └─ World state changes
│  │        ├─ Location mutation updated
│  │        ├─ Route safety improved (if threat cleared)
│  │        ├─ Economy adjusted (if resource discovered)
│  │        └─ Quest options unlocked
│  └─ Discovery logged in UI
└─ Consequence escalation (if ignored)
   ├─ Faction takes independent action
   ├─ Scavengers arrive at ruins
   ├─ Resource depleted by others
   └─ Opportunity lost
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --discovery-consequences-selftest
```

## Risk

**MEDIUM** — Consequence chains can become complex with multiple discoveries and faction interactions. Risk of consequence overload if player discovers many things rapidly. Mitigation: cap active consequences, implement escalation timers, allow concealment to defer decisions.

## Definition of Done

- `DiscoveryConsequenceSystem.cs` exists with full `CaptureState/RestoreState`
- 5 discovery types implemented (resource, threat, ruins, faction contact, strategic)
- Each type has distinct consequence chain with player choices
- Discoveries update `LocationEvolutionSystem` mutation records
- Faction awareness system tracks discovery knowledge
- Discovery concealment/revelation mechanic functional
- Save/load round-trip tested
- Deterministic consequence timing verified
- Old saves load without error
- 20 discovery templates in data authority
- UI panel shows discovery log
- Cross-system integration (location evolution, factions, economy, quests, caravans)

## Follow-On Opportunities

- Discovery auctions (factions bid for exclusive access)
- Discovery sabotage (rival factions destroy discoveries)
- Discovery exploitation (player monopolizes resource)
- Discovery trade network (sell information to multiple factions)
- Discovery legacy (discoveries affect epilogue evaluation)
