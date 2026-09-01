# Map Evolution, Discovery & Mutation Contract

**Authority Catalog:** `Assets/StreamingAssets/Data/world_evolution_events.json`
**Damaged Zones Catalog:** `Assets/StreamingAssets/Data/damaged_map_zones.json`
**Coordinator Engine:** `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs` / `Assets/Ashfall.Core/World/WastelandMapSystem.cs`

---

## 1. Map Discovery Principles

1. **Discovery Hierarchy:**
   - `StartingUnlocked`: Nodes visible and accessible at Day 1 (`loc_holdfast`, `loc_shelter_gate`, `loc_water_station`, `loc_cut_merchant_caravanserai`).
   - `Discoverable`: Nodes hidden under cartographic fog until revealed by physical sortie exploration, scout observation, damaged map fragment synthesis, or cipher decode.
2. **Damaged Map Fragments:**
   - Assembling all fragments of a regional damaged zone in `damaged_map_zones.json` executes an automatic `map.Discover(targetInstallationId)` call and grants corresponding revealed items.
3. **Deterministic Persistence:**
   - Discovered nodes, locked nodes, and route traversability are captured into `WastelandMapState` and serialized via `SaveStore<T>`.

---

## 2. Route Blockades & Map Mutation

1. **Non-Destructive Closures:**
   - When a living geography evolution event triggers (e.g. `event_evolution_checkpoint_kilo`), the target node is marked `IsLocked = true`.
   - `WastelandMapSystem.PlanRoute(from, to)` dynamically avoids locked intermediate nodes, calculating the next best deterministic BFS detour.
2. **Connectivity Safety Guardrail:**
   - An evolution event or route blockade is disallowed from creating disconnected sub-graphs that isolate critical story progression or prevent returning to `loc_holdfast`.
3. **Route Repair Pathways:**
   - Cleared blockades (via player quest action or faction treaty resolution) invoke `map.Unlock(nodeId)`, immediately restoring standard transit corridors.
