# C2 — Flagship Integration Plan [11]: One Place Authority, Graph-Native Travel, and Geographic Knowledge

> **Deliverable:** `C2_planintegration[11].md`
> **Source scope:** Plan 32 — *One Map, Three Notions of Place*
> **Wave:** Continuity Wave 4 — *The World Beyond the Gate*
> **Primary objective:** collapse ASHFALL’s three disconnected notions of place into one canonical location/map authority, make expeditions and caravans travel along graph edges with real distance/cost/risk, and turn discovery/cartography into a persistent information economy rather than a cosmetic label.
> **Required execution order:** **31A → 32A → 32B → 32C**
> **Hard gate:** do not implement graph-native expedition travel until 32A establishes the place model, the node projection, route integrity, and non-null distances.
> **Dependency inputs:** Plan 30A/30B territory, Plan 20A/20C environmental dose/weather, Plan 21A/21B gear/vehicle condition, Plan 24A survivor fitness, Plan 31 semantic events, Plan 33 intelligence channels.
> **Scope discipline:** no new map framework, no procedural world generation, no hex/tile rewrite, no invented `loc_` IDs outside the existing authority, no duplicate location registry, no omniscient precision, and no panel-side travel math.

---

# 0. Executive Intent

ASHFALL currently has three incompatible concepts of geography:

1. a small `WastelandMap` graph,
2. a much larger `loc_` content universe,
3. expedition travel that targets `locationId` directly and does not consult the graph.

That disconnect makes multiple existing systems weaker than they should be:

- distance is absent from routes,
- fuel is detached from geography,
- radiation exposure is detached from route choice,
- terrain is decorative,
- territory has no spatial substrate,
- discovery does not gate travel,
- caravans do not share the player’s roads,
- map fragments unlock no meaningful route state,
- route closures cannot produce real rerouting,
- map presentation cannot truthfully answer what is known versus merely authored.

The plan does not solve this by building a larger map UI.

It solves it by defining one place authority and forcing travel and knowledge systems to consume it.

The final architecture should be:

```text
location authority (`loc_` universe)
        │
        └─ projection → map nodes
                     │
                     ├─ terrain
                     ├─ danger
                     ├─ territory
                     ├─ reveal/knowledge state
                     ├─ route edges
                     └─ route distances
                          │
                          ▼
                   graph pathfinding
                          │
                ┌─────────┼─────────┐
                ▼         ▼         ▼
              time       fuel      dose
                │         │         │
                └──── risk / wear ──┘
                          │
                          ▼
               expedition / caravan movement
                          │
                          ▼
                    semantic events
                          │
                          ▼
                  player-known geography
```

The flagship player-facing outcome is:

> **The map becomes the same ground the expedition system actually travels: every route has distance, terrain, risk, weather, radiation, control, and knowledge state, and the player sees only the precision the Holdfast has earned.**

---

# 1. Source Evidence and Diagnosis

The source plan establishes the core disconnect:

- the live map graph is much smaller than the location universe,
- graph routes have no usable distances,
- expedition travel never queries the map graph,
- discovery can reveal a location without changing targeting/reachability,
- multiple place-adjacent systems each own partial local truth,
- territorial control is about to need a spatial representation.

The correct architectural reading is:

```text
locations are the authority;
map nodes are a travel projection over that authority.
```

The rejected architecture is:

```text
locations.json
+ separate map-node identity universe
+ expedition destination universe
+ discovery universe
```

because that recreates the current drift.

---

# 2. Program-Level Success Criteria

C2[11] is complete only when all of the following are true.

## 2.1 One canonical place identity

Every map node resolves to an existing canonical location ID.

## 2.2 Map nodes are a projection

A node is not a second location definition.

It is a travel-enabled view of a canonical place.

## 2.3 Every route has a real distance

No null/empty route distance remains.

## 2.4 Graph integrity is enforced

All nodes:

- resolve,
- connect appropriately,
- are reachable from Holdfast unless intentionally isolated,
- use valid route semantics.

## 2.5 Expedition dispatch uses a path

Destination selection resolves through graph reachability.

## 2.6 Cost comes from edges

Travel time, fuel, dose, risk, and wear are derived from the route.

## 2.7 Caravans use the same roads

Their arrival timing and disruptions derive from graph state.

## 2.8 Discovery gates capability

Knowledge state affects:

- targeting,
- preview precision,
- map display,
- route information.

## 2.9 Unknown means unknown

The UI must not reveal exact distance, dose, control, or risk where the fiction has not earned that knowledge.

## 2.10 State persists

Graph status, route closure, reveal state, and mid-route travel survive save/load.

---

# 3. Architectural Invariants

## 3.1 `loc_` is canonical place identity

Do not create parallel:

- `maploc_`,
- graph-only IDs,
- duplicate place catalogs.

## 3.2 Map node is a projection

A map node references a canonical place and adds only graph/travel metadata.

## 3.3 One graph authority

`WastelandMapSystem` answers:

- node existence,
- terrain,
- reachability,
- route status,
- knowledge state,
- route closure,
- control when integrated.

## 3.4 One pathfinder

Expeditions and caravans use the same Core pathfinding contract.

## 3.5 One travel estimate

`ExpeditionSystem.Estimate` must remain the canonical estimate source.

UI consumes the same estimate that runtime uses.

## 3.6 One environmental exposure model

Per-edge dose uses Plan 20A/20C outputs.

No route-specific radiation formula.

## 3.7 One condition ledger

Vehicle wear uses Plan 21’s unified condition authority.

## 3.8 One territory input

Control data comes from Plan 30.

Map graph does not invent faction state.

## 3.9 One knowledge ladder

Map knowledge has one explicit state machine.

## 3.10 Information scarcity

Map rendering reads player-known state, not raw simulation truth.

---

# 4. Dependency Graph

```text
31A — semantic event kinds
 │
 ▼
32A — canonical place authority
 │
 ├─ location projection
 ├─ route distances
 ├─ integrity tier
 ├─ node knowledge state
 └─ control/reachability query
 │
 ▼
32B — graph-native travel
 │
 ├─ time
 ├─ fuel
 ├─ dose
 ├─ risk
 ├─ wear
 ├─ closures
 └─ caravan travel
 │
 ▼
32C — geographic knowledge
    ├─ fragments
    ├─ triangulation
    ├─ survey
    ├─ uncertainty
    ├─ memory
    └─ intel propagation

30A/30B ──► territory/control
20A/20C ──► dose/weather
21A/21B ──► vehicle condition
24A ──────► survivor fitness
33 ───────► known-vs-unknown world state
```

Required order:

```text
31A → 32A → 32B → 32C
```

---

# 5. Prerequisite Verification

Before implementing:

## Plan 31A

Confirm semantic kinds exist for:

- route discovered,
- route closed,
- route reopened,
- location revealed,
- survey completed,
- caravan diverted,
- territory changed.

If names differ, use canonical Plan 31 names.

## Plan 30A/30B

Confirm territory/control output is available at location/region granularity or has a defined projection path.

## Plan 20A/20C

Confirm:

- ambient radiation resolver,
- weather effects,
- exposure breakdown.

## Plan 21A/21B

Confirm vehicle/gear condition authority.

If dependencies are absent, build the seam only; do not duplicate missing mechanics.

---

# 6. Baseline Capture

Record:

- current graph node count,
- route count,
- routes with null distance,
- total canonical `loc_` IDs,
- expedition target validation behavior,
- map reveal behavior,
- current save digest,
- existing world-evolution location events.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest
bash scripts/ci/verify-fast.sh
```

Also capture:

```text
dialog/location reachability lint
map/tilemap QA
balance baseline for existing expedition costs
```

---

# 7. Workstream 32A — One Place Authority

## 7.1 Objective

Define what a place is and make the map a projection over canonical locations.

---

# 8. 32A Phase A — Place ADR

Write:

```text
docs/architecture/PLACE_AUTHORITY.md
```

Decision:

```text
loc_ = canonical named place
map node = graph/travel projection of loc_
```

Explicitly reject:

```text
separate location and node identity universes
```

---

# 9. 32A Phase B — Correct Documentation Claims

Update stale implemented-canon claims.

Publish:

- canonical location count,
- graph node count,
- route count,
- intended graph size.

Do not use “261 nodes” unless actually true.

---

# 10. 32A Phase C — Deliberate Graph Size

Choose a playtable target deliberately.

Recommended source range:

```text
25–40 graph nodes
```

unless testing justifies another number.

Selection criteria:

- meaningful route choices,
- faction control,
- expedition pacing,
- encounter density,
- waystation coverage,
- performance,
- map readability.

Do not maximize node count for marketing.

---

# 11. 32A Phase D — Promote Existing Locations

Select node candidates from existing canonical places.

No new IDs unless already approved in master authority.

For each promoted node:

- `location_id`,
- terrain,
- danger tier,
- faction/region link where appropriate,
- visibility,
- cover,
- initial knowledge state.

---

# 12. 32A Phase E — Route Distances

Populate `distance_km` for every route.

Distance source must be documented.

Prefer:

- authored geography,
- consistent relative scale,
- terrain-aware route design.

Avoid arbitrary per-route ticks.

---

# 13. 32A Phase F — Travel Vector Fields

Add/confirm route/node fields for:

- terrain type,
- travel modifier,
- danger tier,
- visibility,
- cover,
- one-way flag,
- route status.

Use data.

No giant switch in expedition code.

---

# 14. 32A Phase G — Integrity Tier

Extend `CatalogIntegrityValidator`.

Validate:

- node references existing `loc_`,
- route endpoints exist,
- no null distance,
- non-negative distance,
- bidirectional or explicitly one-way,
- no duplicate edge where forbidden,
- graph reachable from Holdfast,
- no orphan promoted node,
- valid danger tier,
- valid terrain,
- valid route status.

---

# 15. 32A Phase H — Connectedness Contract

Run graph traversal from:

```text
loc_holdfast
```

Every normal travel node must be reachable.

Intentionally isolated nodes require explicit metadata/reason.

---

# 16. 32A Phase I — Reveal State as Graph State

Introduce one knowledge enum.

Source ladder may ultimately expand in 32C, but 32A needs minimum states.

Recommended final ladder:

```text
Unknown
Rumoured
Located
Surveyed
Visited
Mapped
```

32A should at least establish persistence and query contract.

---

# 17. 32A Phase J — Triangulation Integration

`SignalTriangulationSystem.OnLocationRevealed(locationId)` should update map knowledge.

No more status-only reveal.

Unknown canonical place that is not graph-enabled may still become known in narrative, but travel eligibility must be explicit.

---

# 18. 32A Phase K — Territory Attachment

Map nodes expose territory/control by querying Plan 30 state.

Do not copy territorial percentages into static map JSON.

Potential projection:

```text
region/sector control
→ node control
```

Document exact rule.

---

# 19. 32A Phase L — One Query Surface

Create or extend methods such as:

```text
GetPlace(locationId)
GetNode(locationId)
GetReachability(locationId)
GetTerrain(locationId)
GetKnowledge(locationId)
GetControl(locationId)
GetRoutesFrom(locationId)
```

Use actual API conventions.

---

# 20. 32A Phase M — Migration Safety

Existing saves may contain:

- old six-node status,
- route state,
- discovered flags.

Define migration:

- preserve known nodes,
- assign defaults for new nodes,
- retain discovered state,
- keep checksum/schema discipline.

Pin SaveWireContract test.

---

# 21. 32A Phase N — Orphaned `damaged_map_zones`

Do not leave this unresolved.

Classify now:

```text
WIRE in 32C
or
REMOVE with documented reason
```

Prefer wiring if content maps cleanly to knowledge state.

---

# 22. 32A Tests

- all nodes resolve,
- all routes have distance,
- connectivity,
- one-way semantics,
- terrain modifier lookup,
- danger tier validation,
- reveal persistence,
- old-save migration,
- control query,
- every travel node reachable from Holdfast,
- no invented location IDs.

---

# 23. 32A Definition of Done

- [ ] place ADR,
- [ ] stale docs corrected,
- [ ] graph size chosen,
- [ ] nodes promoted from existing locations,
- [ ] all route distances populated,
- [ ] terrain/danger/vector fields authored,
- [ ] route integrity tier active,
- [ ] connectivity enforced,
- [ ] reveal state persistent,
- [ ] triangulation updates map,
- [ ] control query integrated,
- [ ] one query surface,
- [ ] migration tests,
- [ ] map-zone content disposition decided.

---

# 24. Workstream 32B — Travel on the Graph

## 24.1 Objective

Expeditions and caravans move along graph edges, and route choice determines time, fuel, dose, risk, and wear.

---

# 25. 32B Phase A — Preserve Estimate Contract

Read current `ExpeditionSystem.Estimate`.

Pin outputs for representative destinations.

Before rewrite, capture:

- ticks,
- fuel,
- capacity/readiness,
- risk,
- vehicle effects.

The graph rewrite must explain intentional changes.

---

# 26. 32B Phase B — Core Pathfinding

Add/reuse one pathfinding method.

Requirements:

- deterministic,
- honors closed edges,
- honors one-way edges,
- supports cost weighting,
- stable tie-breaking.

Default path criteria should be explicit:

- fastest,
- lowest risk,
- or balanced.

UI may offer alternatives later if architecture supports it.

---

# 27. 32B Phase C — Dispatch Takes a Path

Dispatch should resolve:

```text
origin
→ destination
→ path
```

Store selected path or enough deterministic data to reconstruct it.

Do not allow destination to bypass reachability.

---

# 28. 32B Phase D — Per-Edge Time

For each edge:

```text
distance_km
÷ terrain-adjusted speed
× weather modifier
× vehicle speed factor
```

Use Plan 20C weather effects.

---

# 29. 32B Phase E — Per-Edge Fuel

Fuel follows:

- distance,
- vehicle efficiency,
- terrain/weather modifiers where authored.

No destination-flat fuel constant.

---

# 30. 32B Phase F — Per-Edge Dose

This is one of the highest-value links.

For each edge:

```text
ambient zone radiation
× travel hours
× weather exposure
× survivor/vehicle/gear protection
```

using canonical Plan 20/21 APIs.

Do not double-count ambient expedition dose.

---

# 31. 32B Phase G — Per-Edge Risk

Risk source may combine:

- terrain,
- danger tier,
- faction control,
- contested status,
- weather,
- visibility,
- expedition readiness.

Use a documented model.

UI and runtime consume same model.

---

# 32. 32B Phase H — Vehicle Wear

Wear follows:

```text
distance × terrain stress × vehicle profile
```

through Plan 21 condition ledger.

No expedition-local durability counter.

---

# 33. 32B Phase I — Waystations and Camps

Waystations become real graph nodes.

Capabilities may include:

- safe rest,
- limited resupply,
- route checkpoint,
- intel exchange,
- repair if existing authority supports it.

Camp-night events remain existing expedition mechanics.

---

# 34. 32B Phase J — Interception

When travelling through hostile/contested territory:

- interception uses existing encounter bridge,
- faction disposition comes from live political state.

No new interception framework.

---

# 35. 32B Phase K — Edge Closures

Support route state:

- open,
- blocked,
- degraded,
- restricted.

Closure sources may include:

- landmark collapse,
- weather,
- blockade,
- bridge failure.

Emit semantic event.

---

# 36. 32B Phase L — Rerouting

If route closes before dispatch:

- recompute.

If route closes mid-expedition:

- resolve current node/edge,
- offer/recompute legal alternative,
- avoid teleportation.

Persist reroute state.

---

# 37. 32B Phase M — Caravan Travel

Caravans use the same graph/path estimator.

Arrival time derives from route.

Route closures and weather affect them too.

This integrates Plan 30C autonomy.

---

# 38. 32B Phase N — UI Route Legibility

Expedition panel shows:

- route line,
- total distance,
- estimated hours,
- fuel,
- projected dose,
- risk,
- significant edge hazards,
- knowledge confidence.

No panel-side recomputation.

---

# 39. 32B Phase O — Path Alternatives

If multiple routes exist, expose at least:

- recommended route,
- safer/shorter alternative if meaningful.

Do not auto-select without player visibility when tradeoffs are material.

---

# 40. 32B Phase P — Baseline Parity

Where old six-node journeys had equivalent logic, compare.

Intentional differences must be documented.

Use representative routes to ensure migration does not arbitrarily explode cost.

---

# 41. 32B Balance Sweep

Sweep:

```text
route length
× weather frequency
× vehicle condition
× protection
× faction hostility
```

Track:

- hours,
- fuel,
- dose,
- wear,
- encounter risk.

Acceptance:

- farthest routes require preparation,
- long routes are not trivially impossible,
- route choice matters,
- no one edge dominates all costs unless authored.

---

# 42. 32B Tests

- pathfinding,
- stable tie-break,
- closed edge,
- one-way edge,
- time sum,
- fuel sum,
- dose sum,
- risk sum,
- wear,
- waystation stop,
- interception,
- reroute,
- caravan timing,
- mid-route save/load,
- same-seed chosen path.

---

# 43. 32B Definition of Done

- [ ] one pathfinder,
- [ ] dispatch path-based,
- [ ] time edge-based,
- [ ] fuel edge-based,
- [ ] dose edge-based,
- [ ] risk edge-based,
- [ ] wear edge-based,
- [ ] waystations graph-native,
- [ ] interception integrated,
- [ ] closures integrated,
- [ ] rerouting works,
- [ ] caravans share graph,
- [ ] route UI uses canonical estimate,
- [ ] balance sweep passes,
- [ ] mid-route saves stable.

---

# 44. Workstream 32C — Geographic Knowledge

## 44.1 Objective

The map records what the Holdfast knows, how it knows it, and what precision that knowledge deserves.

---

# 45. 32C Phase A — Knowledge Ladder

Final explicit ladder:

```text
Unknown
Rumoured
Located
Surveyed
Visited
Mapped
```

Define exact semantic meaning.

---

# 46. 32C Phase B — Sources Per Rung

Example:

| State | Typical source |
|---|---|
| Unknown | none |
| Rumoured | traveller/radio fragment |
| Located | triangulation/map fragment |
| Surveyed | scout/cartography |
| Visited | expedition arrival |
| Mapped | repeated survey/GIS/cartography |

Use actual game systems.

---

# 47. 32C Phase C — `damaged_map_zones`

Wire the three authored fragments if compatible.

Each fragment should:

- reveal one or more canonical places,
- update knowledge rung,
- emit semantic discovery event,
- record source.

If incompatible, remove with documented rationale.

---

# 48. 32C Phase D — Expedition Targeting by Knowledge

Define minimum knowledge required for:

- seeing node,
- targeting,
- estimating route,
- precise risk display.

Example:

```text
Rumoured: visible but untargetable/approximate
Located: targetable
Surveyed: tighter estimate
Visited/Mapped: exact
```

Use deliberate design.

---

# 49. 32C Phase E — Honest Uncertainty

For unsurveyed locations, display ranges.

Examples:

```text
distance: 18–30 km
dose: low–high
risk: uncertain
```

Do not show hidden exact values behind a “?” icon.

---

# 50. 32C Phase F — Calibration Influence

Geiger/cartography/device calibration may narrow uncertainty.

Important:

```text
instrument reading may be wrong
simulation truth is unchanged
```

Reuse Plan 21/20 calibration mechanics if available.

---

# 51. 32C Phase G — Cartography as Work

Surveying should consume:

- time/duty,
- knowledge/research progression,
- equipment if already modeled.

Use Plan 24 labor.

No free “reveal all”.

---

# 52. 32C Phase H — Location Memory

Surface existing:

- strata inscriptions,
- location memory,
- historical notes,
- standing records

as node annotations.

Map should become cumulative memory.

---

# 53. 32C Phase I — Source Attribution

Each knowledge gain records:

- source event,
- day,
- source type,
- confidence.

Tooltip can answer:

```text
How do we know this?
```

---

# 54. 32C Phase J — Player-Known Territory

Map territory/control view must use known intelligence.

Unknown distant control:

- hidden,
- stale,
- uncertain.

Plan 33 controls propagation.

---

# 55. 32C Phase K — Shared Geography

When information is transmitted outward:

- radio,
- caravan,
- scout/intel

can propagate mapped routes.

But that does not mean the player instantly learns reciprocal information.

Keep directionality explicit.

---

# 56. 32C Phase L — Persist Knowledge

Persist:

- rung,
- source,
- confidence,
- last updated,
- notes.

New campaign starts according to authored initial knowledge.

---

# 57. 32C Phase M — Reachability Lint

Run lint over:

- `loc_`,
- `sector_`,
- `zone_`.

Validate:

- authored target reachable or intentionally non-travel,
- no graph node permanently unvisitable,
- references resolve.

---

# 58. 32C Phase N — UI and Accessibility

Atlas shows:

- knowledge state as text + icon,
- confidence,
- source,
- last update,
- route certainty.

No color-only state.

Create snapshots at:

- early ignorance,
- mid-game partial map,
- late-game mapped world.

---

# 59. 32C Tests

- rung transitions,
- invalid backwards transition rules if any,
- fragment reveal,
- triangulation reveal,
- survey cost,
- uncertainty range,
- calibration effect,
- visit state,
- persistence,
- source attribution,
- unknown node precision blocked,
- unknown territory hidden.

---

# 60. 32C Definition of Done

- [ ] knowledge ladder explicit,
- [ ] sources per rung,
- [ ] damaged-map content resolved,
- [ ] targeting respects knowledge,
- [ ] uncertainty honest,
- [ ] calibration affects confidence only,
- [ ] cartography costs labor,
- [ ] location memory shown,
- [ ] source attribution persisted,
- [ ] territory knowledge gated,
- [ ] knowledge persistence,
- [ ] reachability lint green,
- [ ] map snapshots at three depths,
- [ ] accessibility pass.

---

# 61. Integrated Place/Travel/Knowledge Pipeline

```text
Canonical location (`loc_`)
        │
        ▼
Map-node projection
        │
        ├─ terrain
        ├─ danger
        ├─ control
        ├─ route edges
        └─ knowledge state
                │
                ▼
            pathfinding
                │
        ┌───────┼────────┐
        ▼       ▼        ▼
      time     fuel     risk
        │       │        │
        ├──── dose ──────┤
        └──── wear ──────┘
                │
                ▼
       expedition/caravan runtime
                │
                ▼
        semantic world events
                │
                ▼
       knowledge/intel propagation
                │
                ▼
           map presentation
```

---

# 62. Place Authority Contract

For any `locationId`, the canonical map/world query should answer where applicable:

```text
Does the location exist?
Is it a graph node?
Do we know about it?
Can we reach it?
How far is it?
What terrain?
What danger?
Who controls it?
Is the route open?
What do we know with confidence?
```

No second system should independently answer these.

---

# 63. Graph Data Contract

Each node:

- canonical location ID,
- terrain,
- danger,
- optional region/sector,
- travel metadata.

Each route:

- endpoints,
- distance,
- directionality,
- modifiers,
- status.

No null required fields.

---

# 64. Knowledge vs Truth Contract

Simulation truth may contain:

- exact distance,
- exact dose,
- exact territory,
- exact route status.

Player-known projection may expose only:

- range,
- stale state,
- rumor,
- last known control.

Never bind map directly to raw truth for hidden information.

---

# 65. Expedition Estimate Contract

One `Estimate` result should include:

```text
path
distance
time
fuel
dose
risk
wear
confidence/knowledge
```

UI reads it.
Runtime uses it.
No duplication.

---

# 66. Save Contract

Persist:

- map node statuses,
- knowledge state,
- route closures,
- discovered nodes,
- mid-route expedition path/progress,
- caravan path/progress if owned here.

Old-save migration must preserve original known map state.

---

# 67. Catch-Up and Dynamic World Contract

If Plan 30/world evolution changes routes during catch-up:

- route state updates deterministically,
- active expedition migration must handle impossible stale paths safely,
- no teleporting.

---

# 68. Event Contract

Use Plan 31 for:

- reveal,
- survey,
- visit,
- route closure,
- route reopen,
- reroute,
- caravan diversion,
- territory change.

Events should carry:

- location/route IDs,
- old/new state,
- cause,
- visibility/knowledge implications.

---

# 69. Balance Contract

Graph size and route distances must support:

- early nearby runs,
- mid-game branching,
- late long-haul expeditions,
- meaningful waystations.

Avoid:

- all routes shortest by same corridor,
- one unavoidable lethal edge,
- excessive node density with no decisions.

---

# 70. Performance Contract

Pathfinding and graph queries should be cheap.

With ~25–40 nodes:

- ordinary deterministic pathfinding is trivial,
- no need for complex spatial indexing.

Avoid overengineering.

Cache static graph structure, but not dynamic route status in a way that goes stale.

---

# 71. Failure Modes and Corrective Actions

## 71.1 Expeditions can target unreachable places

Fix:

- dispatch must resolve graph reachability.

## 71.2 Route distance remains null

Fail integrity.

## 71.3 Map has duplicate place identity

Fix:

- node references canonical `loc_`.

## 71.4 UI shows exact unknown radiation

Premise violation.

Fix:

- knowledge projection/ranges.

## 71.5 Route closes mid-expedition and unit teleports

Fix:

- current-edge state + reroute.

## 71.6 Caravan ignores closures

Fix:

- shared pathfinder.

## 71.7 Territory shown where intel absent

Fix:

- Plan 33 player-known control.

## 71.8 Graph expansion breaks old saves

Fix:

- node defaults + schema migration.

## 71.9 Per-edge dose double-counts expedition radiation

Critical.

Fix:

- Plan 20 exactly-once exposure ownership.

---

# 72. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| graph expansion save drift | Medium | High | wire-contract migration |
| travel math changes balance | High | High | parity + sweeps |
| route distance authorship inconsistent | Medium | Medium | documented scale |
| omniscient map leakage | Medium | High | knowledge projection |
| mid-route closure bugs | Medium | High | explicit progress state |
| caravan path divergence | Medium | Medium | shared pathfinder |
| control projection ambiguous | Medium | Medium | ADR |
| too many nodes | Medium | Medium | deliberate playtable target |
| too few nodes | Medium | Medium | route-choice validation |
| dose double-count | Medium | Critical | canonical exposure test |
| performance overengineering | Low | Low | simple graph algorithms |

---

# 73. Commit Strategy

## Commit C2[11].1 — Baseline + place ADR

- real counts,
- stale docs correction.

## Commit C2[11].2 — node projection model

- canonical location references.

## Commit C2[11].3 — route distances/vector fields

## Commit C2[11].4 — graph integrity tier

## Commit C2[11].5 — reveal/control query + persistence

### Gate: 32A complete

## Commit C2[11].6 — pathfinder + estimate path support

## Commit C2[11].7 — edge time/fuel

## Commit C2[11].8 — edge dose/risk/wear

## Commit C2[11].9 — waystations/interception

## Commit C2[11].10 — closures/rerouting

## Commit C2[11].11 — caravan graph travel

## Commit C2[11].12 — route UI + balance/parity

### Gate: 32B complete

## Commit C2[11].13 — knowledge ladder

## Commit C2[11].14 — fragments/triangulation

## Commit C2[11].15 — uncertainty/calibration

## Commit C2[11].16 — cartography/location memory

## Commit C2[11].17 — intel-gated territory + persistence

## Commit C2[11].18 — reachability lint/snapshots/accessibility

### Gate: 32C complete

## Commit C2[11].19 — integrated map/travel closure

---

# 74. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expedition-selftest
bash scripts/ci/verify-fast.sh
```

Also run:

```text
ashfall-dialog-graph-lint
ashfall-tilemap-world-qa
ashfall-balance-sim
```

using repository-canonical commands.

---

# 75. Flagship Definition of Done

## Prerequisites

- [ ] Plan 31 semantic map/travel events,
- [ ] Plan 30 control data,
- [ ] Plan 20 exposure/weather,
- [ ] Plan 21 condition authority.

## 32A

- [ ] canonical place ADR,
- [ ] graph node projection,
- [ ] deliberate graph size,
- [ ] distances complete,
- [ ] terrain/danger vectors,
- [ ] integrity tier,
- [ ] connectivity,
- [ ] reveal state,
- [ ] territory query,
- [ ] one place query surface,
- [ ] old-save migration,
- [ ] damaged-map disposition.

## 32B

- [ ] pathfinding,
- [ ] dispatch path-based,
- [ ] edge time,
- [ ] edge fuel,
- [ ] edge dose,
- [ ] edge risk,
- [ ] vehicle wear,
- [ ] waystations,
- [ ] interception,
- [ ] route closures,
- [ ] reroute,
- [ ] caravans share roads,
- [ ] route UI canonical,
- [ ] balance/parity.

## 32C

- [ ] knowledge ladder,
- [ ] fragments/triangulation,
- [ ] targeting gates,
- [ ] uncertainty ranges,
- [ ] calibration effect,
- [ ] survey/cartography cost,
- [ ] location memory,
- [ ] source attribution,
- [ ] player-known territory,
- [ ] persistence,
- [ ] lint,
- [ ] snapshots/accessibility.

## Cross-system

- [ ] one location identity,
- [ ] one graph authority,
- [ ] one pathfinder,
- [ ] one travel estimate,
- [ ] one knowledge projection,
- [ ] no omniscient precision,
- [ ] no invented IDs,
- [ ] full verification green.

---

# 76. Closure Report Template

```markdown
## C2[11] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Baseline
- Canonical loc IDs:
- Graph nodes:
- Routes:
- Null distances:
- Expedition targets:
- Reveal-only targets:
- Save schema:
- Balance baseline:

### 32A — Place Authority
- ADR:
- Graph size:
- Promoted locations:
- Route distances:
- Terrain fields:
- Danger tiers:
- Integrity results:
- Connectivity:
- Reveal persistence:
- Territory query:
- Save migration:
- Result:

### 32B — Travel
- Pathfinder:
- Path estimate:
- Distance:
- Hours:
- Fuel:
- Dose:
- Risk:
- Wear:
- Waystations:
- Closures:
- Rerouting:
- Caravan timing:
- Mid-route save:
- Balance:
- Result:

### 32C — Knowledge
- Knowledge ladder:
- Damaged fragments:
- Triangulation:
- Targeting gate:
- Uncertainty:
- Calibration:
- Survey:
- Location memory:
- Territory intel:
- Persistence:
- Reachability lint:
- Snapshots:
- Accessibility:
- Result:

### Full Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Expedition:
- Dialog/location lint:
- World QA:
- Balance:
- Verify fast:

### Final Metrics
- Graph nodes:
- Routes:
- Null distances:
- Unreachable nodes:
- Unknown exact-value leaks:
- Avg route distance:
- Max route distance:
- Caravan graph usage:
- Mid-route save failures:

### Remaining Debt
- Map:
- Travel:
- Knowledge:
- Territory:
- Intel:
- Cartography:
```

---

# 77. Final Execution Directive

Execute Plan 32 as a geography-continuity repair.

The critical sequence is:

```text
decide what a place is
→ make map nodes a projection of canonical locations
→ populate real route distances
→ enforce graph integrity
→ make expeditions choose graph paths
→ derive time/fuel/dose/risk/wear from edges
→ make caravans use the same roads
→ make discovery and cartography change what the player can know and do
```

Do not expand the map because a stale registry claimed a large number.

Do not build travel mechanics on a six-node graph with null distances.

Do not show exact values for places the Holdfast has only heard rumours about.

The strongest identity rule is:

> **`loc_` is the canonical place identity; a map node is a travel projection of that place, not a second authority.**

The strongest travel rule is:

> **The expedition runtime and the expedition UI must consume the same graph path and the same edge-derived cost model.**

The strongest information rule is:

> **The map represents what the Holdfast knows, not everything the simulation knows.**

The flagship acceptance scenario is:

> **Select two destinations with different routes. The map must show different distance, weather exposure, fuel, projected dose, risk, and control; the expedition must actually travel those edges; a route closure must force a visible reroute; and an unknown destination must not expose precision the player has not earned.**
