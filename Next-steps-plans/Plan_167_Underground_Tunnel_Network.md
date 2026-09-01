# Plan 167 — Underground Tunnel Network System

## Goal

Create an underground tunnel network system where the shelter can discover, explore, map, and utilize a network of subterranean passages connecting to other bunkers, resource deposits, and hidden locations. Currently `SaltMineExtractionSystem.cs` handles subterranean salt mining, and narrative text references "access tunnels" and "subterranean" locations, but there is no tunnel network gameplay system — no inter-bunker travel, no tunnel exploration, no underground mapping, no tunnel maintenance. This plan adds a vertical dimension to the game — the world below the world.

## Why

**Repository evidence:** Grep for `tunnel`, `underground`, `bunker_network`, `subterranean` in Core returns only `SaltMineExtractionSystem.cs` (resource extraction, not tunnel network) and narrative flavor text (questline references to "access tunnel" being sealed, "subterranean" as location descriptions). Data files reference `subterranean_seed_vault` as a location, `tunnel_digger` and `underground_navigator` as survivor traits — but no tunnel network system consumes these. `UndergroundFungiCatalog.cs` is narrative data, not a gameplay system.

**What is missing:** No tunnel network. No inter-bunker underground travel. No tunnel exploration mechanic. No tunnel mapping. No tunnel maintenance (collapse risk, ventilation, structural integrity). No underground resource deposits accessible only via tunnels. No hidden underground locations discoverable through tunnel exploration. The underground is referenced in narrative and trait names but has no gameplay.

**Why existing plans don't solve it:** Plan 133 (expedition consequences) adds surface discovery consequences. Plan 141 (research unlocks) adds research rewards. Plan 153 (espionage) adds faction infiltration. Plan 155 (black market) adds underground economy but not physical underground spaces. Plan 163 (cartography) adds surface map discovery. No plan addresses physical tunnel networks or underground exploration.

**Player value:** Creates exploration depth (the world has a underground layer), adds strategic options (alternative travel routes, hidden resources), generates emergent stories (collapsed tunnels, discovered bunkers, underground encounters), and makes the shelter feel connected to a larger subterranean world.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` — existing subterranean system
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — surface expedition travel
- `Assets/Ashfall.Core/LocationEvolutionSystem.cs` — location state tracking
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill system
- `Assets/StreamingAssets/Data/locations.json` — location definitions
- NEW: `Assets/Ashfall.Core/Underground/TunnelNetworkSystem.cs`
- NEW: `Assets/StreamingAssets/Data/tunnel_network.json`

## Main Task 1 — Foundation / System Contract

1. Create `TunnelNetworkSystem.cs` in `Assets/Ashfall.Core/Underground/`
2. Define `TunnelSegment` DTO: `segmentId`, `segmentName`, `connectsFrom` (location/tunnel ID), `connectsTo` (location/tunnel ID), `length` (ticks to traverse), `difficulty` (1-5), `hazards` (list: collapse/flood/radiation/darkness), `discovered` bool, `explored` (0-100 percentage), `structuralIntegrity` (0-100), `lastInspectedDay`
3. Define `TunnelJunction` DTO: `junctionId`, `junctionName`, `connectedSegments` (list of tunnel segment IDs), `discovered` bool, `features` (list: water_source/radioactive_vein/stable_camp/ancient_bunker)
4. Define `TunnelExpedition` DTO: `expeditionId`, `targetSegmentId`, `assignedSurvivors` (list), `equipment` (list), `status` (planned/active/complete/failed), `startDay`, `findings` (list of discoveries), `casualties` (list)
5. Define `TunnelNetworkState` DTO: list of tunnel segments, list of junctions, list of active/past expeditions, list of accessible locations via tunnel, network map completeness (0-100)
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define tunnel network topology:
   - Shelter connects to 1-3 initial tunnel segments
   - Segments connect to junctions, junctions to more segments
   - Some segments lead to hidden locations (bunkers, resource deposits, seed vaults)
   - Network is partially collapsed (some segments impassable until repaired)
   - Network expands as player explores (new segments discovered)
8. Define tunnel hazards:
   - **Collapse**: structural integrity drops, segment becomes impassable
   - **Flooding**: water ingress, requires pumping, slows traversal
   - **Radiation**: radioactive veins, requires protective equipment
   - **Darkness**: no light sources, requires lanterns/flashlights
   - **Gas**: toxic gas pockets, requires gas masks
9. Define tunnel exploration mechanics:
   - Tunnel expeditions explore segments (like surface expeditions)
   - Exploration reveals tunnel features, hazards, connections
   - Discovered segments added to network map
   - Junctions serve as navigation points
   - Some segments require equipment (ropes, pumps, lights)
10. Define tunnel maintenance:
    - Structural integrity degrades over time
    - Segments can collapse (become impassable)
    - Repair requires materials and survivor labor
    - Regular inspection slows degradation
    - `tunnel_digger` trait bonus: slower degradation
11. Define tunnel travel:
    - Tunnel travel between connected locations (faster than surface?)
    - Tunnel travel has risks (hazards, collapse)
    - Tunnel routes can be safer than surface (no weather, no raiders)
    - Tunnel routes can be more dangerous (collapse, radiation)
12. Add deterministic seeding: tunnel exploration uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupTunnelNetwork`, `TickTunnelNetwork`, `SaveTunnelNetwork`
14. Create `TunnelNetworkCatalogLoader` for initial network definitions
15. Implement tunnel network UI: map panel showing discovered segments, junctions, accessibility

## Main Task 2 — Implementation / Exploration / Maintenance / Travel / Discoveries

1. Implement tunnel network generation:
   - Initial network: shelter + 1-3 segments + 1 junction
   - Network expands as player explores (new segments discovered)
   - Some segments lead to hidden locations (bunkers, vaults, deposits)
   - Network topology is deterministic (seeded) but discovered progressively
2. Implement tunnel exploration expeditions:
   - Player assigns survivors + equipment to tunnel expedition
   - Expedition traverses segment (takes ticks)
   - Hazard checks during traversal (collapse, flood, radiation, gas)
   - Successful exploration: segment fully mapped, features revealed
   - Failed exploration: survivors injured, equipment lost, segment still partially unknown
3. Implement tunnel maintenance:
   - Structural integrity degrades daily (based on segment age, hazards)
   - Inspection slows degradation (survivor assigned to inspect)
   - Repair requires materials (concrete, steel, timber) and labor
   - Collapsed segments block tunnel travel
   - `tunnel_digger` trait: slower degradation, faster repair
4. Implement tunnel travel:
   - Connected locations can be reached via tunnel (alternative to surface)
   - Tunnel travel time based on segment lengths
   - Tunnel travel safer from surface threats (weather, raiders)
   - Tunnel travel threatened by underground hazards
   - `underground_navigator` trait: faster tunnel travel, better hazard detection
5. Implement tunnel discoveries:
   - Hidden bunkers: abandoned shelters with loot, survivors, or dangers
   - Resource deposits: mineral veins, underground water, geothermal vents
   - Ancient infrastructure: pre-war machinery, power generators, water systems
   - Biological features: underground fungi, mutated creatures, root systems
   - `subterranean_seed_vault` location accessible via tunnel
6. Implement tunnel hazards:
   - Collapse: random events, integrity-based probability
   - Flooding: seasonal (heavy rain), requires pumps
   - Radiation: fixed locations, requires dosimeters + protection
   - Darkness: requires light sources, affects exploration speed
   - Gas: random pockets, requires gas masks
7. Implement tunnel equipment:
   - **Lantern/flashlight**: required for dark segments
   - **Rope**: required for vertical segments
   - **Pump**: required for flooded segments
   - **Gas mask**: required for gas/radiation segments
   - **Inspection kit**: required for structural assessment
   - **Repair kit**: required for structural repair
8. Create tunnel events:
   - "The Discovery" — new tunnel segment found
   - "The Collapse" — tunnel segment collapses
   - "The Flood" — tunnel segment floods
   - "The Bunker" — hidden bunker discovered
   - "The Deposit" — resource deposit found
   - "The Repair" — tunnel segment repaired
   - "The Expedition" — tunnel expedition returns (success/failure)
   - "The Connection" — tunnel connects to surface location
9. Add tunnel quest hooks:
   - "The Explorer" — discover 10 tunnel segments
   - "The Mapper" — map complete tunnel network
   - "The Miner" — discover resource deposit via tunnel
   - "The Bunker" — discover hidden bunker
   - "The Engineer" — repair collapsed tunnel
   - "The Network" — connect 3 surface locations via tunnel
   - "The Vault" — reach subterranean seed vault
10. Implement tunnel UI:
    - Tunnel map panel: shows discovered segments, junctions, accessibility
    - Expedition planning: assign survivors, equipment, target segment
    - Maintenance panel: shows structural integrity, repair needs
    - Discovery log: list of tunnel discoveries
    - Travel planner: compare tunnel vs surface routes
11. Add tunnel journal: automatic log of tunnel events
12. Implement tunnel tutorial: first tunnel discovery explains system
13. Add tunnel tooltips: hover over segment shows hazards, integrity
14. Create 30 tunnel segments + 10 junctions in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ExpeditionSystem`: tunnel expeditions use expedition framework
2. Connect to `LocationEvolutionSystem`: tunnel discoveries affect locations
3. Integrate with `SkillProgressionSystem`: tunnel skills (exploration, repair)
4. Connect to `SaltMineExtractionSystem`: tunnel deposits complement mining
5. Wire into `WeatherSystem`: flooding affected by surface weather
6. Connect to `ShelterThermalSystem`: tunnels affect shelter temperature
7. Implement old-save compatibility: existing saves get default tunnel state (shelter entrance only)
8. Add deterministic seeding: tunnel topology and exploration use `ISeededRng`
9. Create exploit prevention: tunnel collapse is permanent without repair, hazards require equipment
10. Add tests: tunnel exploration, maintenance, travel, hazards, save round-trip
11. Verify catalog integrity: all segment/junction/location IDs resolve
12. Test edge cases: no tunnels (shelter only), full network (all discovered)
13. Verify headless behavior: tunnel network processes correctly without UI
14. Add data-integrity-selftest: tunnel segments validate against location catalogs
15. Create `--tunnel-network-selftest` verb for CI validation

## State / System Interaction Model

```text
Underground tunnel network
├─ Tunnel topology
│  ├─ Segments connect shelter to junctions and locations
│  ├─ Junctions serve as navigation points
│  ├─ Network expands as player explores
│  ├─ Some segments lead to hidden locations
│  └─ Topology is seeded but discovered progressively
├─ Tunnel exploration
│  ├─ Expeditions traverse segments
│  ├─ Hazard checks during traversal
│  ├─ Successful: segment mapped, features revealed
│  ├─ Failed: injuries, equipment lost
│  └─ Discoveries: bunkers, deposits, infrastructure
├─ Tunnel maintenance
│  ├─ Structural integrity degrades daily
│  ├─ Inspection slows degradation
│  ├─ Repair requires materials + labor
│  ├─ Collapsed segments block travel
│  └─ tunnel_digger trait: slower degradation
├─ Tunnel travel
│  ├─ Alternative to surface travel
│  ├─ Safer from surface threats
│  ├─ Threatened by underground hazards
│  ├─ underground_navigator trait: faster travel
│  └─ Connects surface locations underground
├─ Tunnel hazards
│  ├─ Collapse: integrity-based probability
│  ├─ Flooding: seasonal, requires pumps
│  ├─ Radiation: fixed locations, requires protection
│  ├─ Darkness: requires light sources
│  └─ Gas: random pockets, requires masks
└─ Integration
   ├─ Expeditions (tunnel expedition framework)
   ├─ Locations (tunnel discoveries)
   ├─ Skills (exploration, repair)
   ├─ Mining (tunnel deposits)
   ├─ Weather (flooding)
   └─ Thermal (tunnel temperature)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --tunnel-network-selftest
```

## Risk

**MEDIUM** — Tunnel network complexity can overwhelm if too many segments and hazards exist. Risk of tunnels feeling like a second expedition system rather than a distinct gameplay layer. Mitigation: keep tunnel topology manageable (30 segments max), make tunnels distinct from surface (underground hazards, hidden locations), and integrate with existing systems rather than duplicating expedition mechanics.

## Definition of Done

- `TunnelNetworkSystem.cs` exists with full `CaptureState/RestoreState`
- Tunnel network topology functional (segments, junctions, connections)
- Tunnel exploration expeditions working
- Tunnel maintenance system (integrity, degradation, repair)
- Tunnel travel as alternative to surface
- 5 hazard types implemented (collapse, flood, radiation, darkness, gas)
- Tunnel equipment system (lanterns, ropes, pumps, masks, kits)
- Tunnel discoveries (bunkers, deposits, infrastructure, biological)
- Tunnel events and quest hooks
- Save/load round-trip tested
- Deterministic tunnel topology verified
- Old saves load without error
- 30 tunnel segments + 10 junctions in data authority
- UI map panel showing tunnel network
- Cross-system integration (expeditions, locations, skills, mining, weather, thermal)

## Follow-On Opportunities

- Tunnel colonization (establish underground outposts)
- Tunnel trading (underground trade routes)
- Tunnel warfare (defend tunnels from intruders)
- Tunnel legacy (famous tunnel networks remembered)
- Tunnel quests (explore specific segments, reach hidden locations)
