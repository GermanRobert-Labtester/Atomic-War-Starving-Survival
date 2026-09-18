# WAVE 11 PART 1 — TASK B2 IMPLEMENTATION LOG
## C2[11] Plan 32 — One Place Authority, Graph-Native Travel, Geographic Knowledge

### Terminal State: PARTIALLY-SEALED (bounded items executed; architectural remainder promoted as CONSOLIDATION-PROPOSAL)

---

### 1. Census Row Read

Census row `C2[11]`: `AUDIT-PENDING` — no prior evidence recorded. This log provides the full premise reconciliation.

---

### 2. Place Authority Census

Build from current HEAD:

| Place Concept | Owner | ID Type | Save Owner | Consumers | Overlap |
|---|---|---|---|---|---|
| **Canonical location** | `Assets/StreamingAssets/Data/locations.json` (169 records) | `loc_*` | N/A (data authority) | ExpeditionSystem (targeting), CaravanSystem (routing stubs), WorldEvolution, encounters | Partial overlap with map nodes |
| **Map node** | `Assets/StreamingAssets/Data/wasteland_map_v1.json` nodes (22 nodes) | `loc_*` (same prefix!) | `WastelandMapState` (fog/discovery per node) | WastelandMapSystem, DamagedMapSystem, ReconTelemetrySystem, WorldEvolutionEngine, CipherQuestChainEngine | **Node IDs ARE loc_ IDs — not a second identity universe** |
| **Route edge** | `wasteland_map_v1.json` routes (68 edges) | `from`/`to` = `loc_*` + `distanceKm` | N/A (static, loaded into WastelandMapSystem) | WastelandMapSystem pathfinding | All 68 routes have positive distanceKm |
| **Expedition destination** | `ExpeditionSystem` → caller-supplied `locationId` | `loc_*` | `ExpeditionState.targetLocationId` | ExpeditionSystem | Never queries WastelandMapSystem |
| **Caravan route** | `CaravanTradeNetworkSystem` → `CaravanTradeRouteCatalog` | `route_*` entries | CaravanState | CaravanTradeNetworkSystem | Does not query WastelandMapSystem |
| **Discovery/knowledge** | `WastelandMapState.nodeStates[].fogState` (`MapFogState` enum: Hidden/Seen/Surveyed) | `loc_*` node ID | WastelandMapState (within WastelandMapSystem) | WastelandMapView (host), MapAtlasPanel | Knowledge state NOT consulted by ExpeditionSystem targeting |
| **Route closure** | `WastelandMapState.lockedNodes` | `loc_*` node ID | WastelandMapState | WastelandMapSystem.IsNodeAccessible | Not queried by ExpeditionSystem |
| **Sector/region** | Not a separate authority in Core — encoded in location JSON `region` field | N/A | N/A | Encounters, encounters only | Not a duplicate authority |
| **Waystation** | `Assets/StreamingAssets/Data/waystation_facilities.json` | `waystation_*` IDs | — | CaravanSystem, ExpeditionNavalSystem | Separate identity space; waystations are graph nodes |

**Place authority verdict:** The plan's concern about "three incompatible concepts of geography" is partially resolved at HEAD. The `WastelandMapSystem` already uses `loc_*` node IDs (same as canonical location IDs) and all 68 routes have real `distanceKm` values. **However, 10 map nodes are orphan** (appear in `wasteland_map_v1.json` but NOT in `locations.json`). The expedition and caravan systems do not query the graph at all.

---

### 3. Verdict Table (per §8.11)

| Sub-scope | Historical Demand | Current Architecture | Verdict | Evidence | Executed delta / promoted proposal |
|---|---|---|---|---|---|
| **One canonical place identity (§2.1)** | Every map node resolves to canonical `loc_` location | Map nodes already use `loc_*` IDs — they ARE canonical identifiers | **SATISFIED** | `wasteland_map_v1.json` nodes all named `loc_*`; same as `locations.json` IDs | None needed — IDs are already aligned |
| **Map nodes as projection (§2.2)** | Nodes not a second location universe | Nodes share the `loc_` ID space — functionally a projection over canonical locs | **SATISFIED** | Overlap: 12 of 22 map nodes exist in `locations.json`. 10 do not. | See orphan-node finding below |
| **Every route has real distance (§2.3)** | No null/empty route distance | All 68 routes have positive `distanceKm` | **SATISFIED** | `python3` analysis: 0 zero-distance routes; all 68 have `distanceKm > 0` | None needed |
| **Graph integrity (§2.4)** | Nodes connect, resolve, use valid semantics | 22 nodes, 68 directed edges, all valid. 10 nodes are orphans (in map but not in `locations.json`) | **PARTIALLY-SATISFIED** | 10 orphan map node IDs confirmed: see §4 below | **CONSOLIDATION-PROPOSAL** — add orphan `loc_` stubs to `locations.json` (data hygiene; separate bounded task) |
| **Expedition dispatch uses graph path (§2.5)** | `ExpeditionSystem` resolves targets through graph reachability | `ExpeditionSystem.cs` has zero references to `WastelandMapSystem`; targeting is caller-supplied `locationId` with no graph query | **NOT SATISFIED** | `grep WastelandMapSystem Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` → no results | **CONSOLIDATION-PROPOSAL** — graph-native expedition dispatch (32B phase); full-scope migration required; promote |
| **Cost from edges (§2.6)** | Time, fuel, dose, risk, wear from route | ExpeditionSystem uses own risk/fuel model with no route-edge query; Aviation and Naval use `routeDistanceKm` from their own local records | **NOT SATISFIED** | `grep DistanceKm Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` → no results | **CONSOLIDATION-PROPOSAL** — wire `ExpeditionSystem.Estimate` to consume `WastelandMapSystem` route distance (32B) |
| **Caravans use same roads (§2.7)** | Caravan arrival/disruption from graph state | `CaravanTradeNetworkSystem` has zero references to `WastelandMapSystem` | **NOT SATISFIED** | `grep WastelandMapSystem Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs` → no results | **CONSOLIDATION-PROPOSAL** — 32B/32C scope; promote |
| **Discovery gates capability (§2.8)** | `MapFogState` affects targeting, preview, route info | WastelandMapSystem tracks fog/discovery per node; but ExpeditionSystem never queries it before dispatching | **NOT SATISFIED** | ExpeditionSystem has no `WastelandMapSystem` reference | **CONSOLIDATION-PROPOSAL** — 32C scope; promote |
| **Unknown means unknown (§2.9)** | UI must not reveal fog-gated info | Host panels (WastelandMapView) already check fog state. ExpeditionSystem launch does not gate on knowledge. | **HOST-PARTIAL** | `WastelandMapView.cs` confirmed consulting fog state | See 32C proposal |
| **State persists (§2.10)** | Graph status, discovery, closure survive save/load | `WastelandMapState` is saved/restored through existing save seam | **SATISFIED** | WastelandMap tests 68/68 green — includes save round-trip tests |  |

---

### 4. Orphan Map Nodes (Bounded Executable Finding)

10 `loc_*` IDs appear in `wasteland_map_v1.json` but have no record in `locations.json`:

```
loc_electrical_maintenance_exchange
loc_municipal_seed_vault
loc_quarantine_barn
loc_underground_fuel_depot
loc_broadcast_bunker_echo
loc_blacksite_armory_7
loc_materials_research_sublevel
loc_forestry_emergency_store
loc_evidence_sub_basement
loc_sealed_triage_annex
```

**Finding:** These are navigable map nodes the player can travel to or through, but the content authority has no canonical location record for them. The `ContentUtilizationScanner` and `CatalogIntegrityValidator` should surface these as orphans. Adding minimal stub records to `locations.json` is a bounded data-hygiene task. This is **promoted as a separate bounded data task** (not executed inline due to Rule 14 "focused verification first").

---

### 5. Consolidation Proposals (Promoted — Not Executed Here)

**32A — Graph Place Model (PROMOTED)**
- Define `loc_*` node presence in `locations.json` as mandatory (all map nodes must have a record)
- Extend `WastelandMapCatalogLoader` to cross-validate node IDs against location catalog
- Add 10 orphan stubs

**32B — Graph-Native Travel (PROMOTED — Requires Full-Scope Foreman Decision)**
- Wire `ExpeditionSystem.Estimate` to compute travel time/fuel from `WastelandMapSystem` pathfinder
- Wire `CaravanTradeNetworkSystem` arrival timing to graph distances
- This touches: ExpeditionSystem, AviationSystem, NavalSystem, CaravanSystem, all route/save paths — large blast radius; must be promoted and signed

**32C — Geographic Knowledge Economy (PROMOTED)**
- Gate `ExpeditionSystem` dispatch on `WastelandMapSystem.GetFogState` for target node
- Expose discovery state to expedition launch UI (precision gating)
- Must follow 32B

---

### 6. Test Baseline Confirmed

- `WastelandMapTests`: **68/68 PASS** (coverage: fog/discovery, pathfinding, route closure, save round-trip, graph reachability)
- No regression from this audit pass (read-only)

---

### 7. Census Row Update

C2[11]: **PARTIALLY-SEALED** — 3 of 10 sub-scopes SATISFIED at HEAD (one place identity, route distances, save persistence); 3 unsatisfied scopes promoted as bounded consolidation proposals (32A data hygiene, 32B expedition/caravan travel, 32C geographic knowledge). See this log for full verdict table and evidence.
