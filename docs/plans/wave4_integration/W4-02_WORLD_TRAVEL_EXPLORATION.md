# ASHFALL — WAVE 4 INTEGRATION PROGRAM · PLAN 2 OF 6

# WORLD, TRAVEL & EXPLORATION INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W4 (six-plan integration wave — the shelter's remaining machinery)
**Document:** W4-02
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W4-01 (save/state), W4-03 (infrastructure), W4-04 (ecology), W4-05 (society), W4-06 (medicine)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates **the world outside the door**: the map graph and its knowledge
tiers, route resolution and weather gates, crossings, expeditions and their vehicles,
rail and maritime travel, location evolution, and the surfaces and journals through
which the player knows the land. It extends the existing world owners — one map, one
route authority, one expedition grammar — and never builds a second map, router, or
travel calculator.

### 0.1 Two selection levels

| Level | Choice | Granularity |
|---|---|---|
| **Level 1** | Plan Path **A**, **B**, or **C** | the whole plan's posture |
| **Level 2** | ten decision points, each **A/B/C** | per-concern depth |

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Maps | 1–10 | — | — |
| B One Road | 1,2 | 3,4,5,6,7,8 | 9,10 |
| C Living World | — | 3,5 | 1,2,4,6,7,8,9,10 |

### 0.3 The Wave 4 rule for this plan

> **One map, one route resolver, one expedition grammar.** `WastelandMapSystem`
> (with `WastelandMapCatalogLoader` and the live JSON data) owns nodes and edges;
> `TravelGraphKnowledgeGate` owns what the player knows; `ModalTravelDispatchEngine`
> and the route/gate family own travel resolution; `ExpeditionSystem` (with its
> aggregate, catalog, encounter bridge, and loot resolver) owns away-missions. No
> parallel map, no second pathfinder, no ad-hoc travel math in panels.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| node/route | a map location and a traversable edge between locations |
| knowledge tier | Unknown → Rumored → Surveyed → Mapped, gating what can be planned |
| route resolution | the single deterministic answer to "can these two places be connected, how, in how long, at what risk" |
| weather gate | a seasonal/weather condition that delays, diverts, or closes a route |
| crossing | a dangerous transit event resolved through arbitration, not raw travel |
| expedition | an away-mission with crew, vehicle, supplies, encounters, and return |
| dive | a maritime/subterranean instance run with its own slot grammar |
| evolution | long-arc change to a location's state or landmark condition |
| field guide | the player's accumulated written knowledge about the land |

---

## 1. Premise audit — what P0 must verify (Rule 7)

```text
[ ] Assets/Ashfall.Core/World/: WastelandMapSystem + WastelandMapCatalogLoader,
    TravelGraphKnowledgeGate, MapRouteHazardEvaluator, RouteAvailabilityKind/
    Presentation, RouteGateContextResolver, RouteInfrastructureSystem,
    RouteRegionTopology, ModalTravelDispatchEngine, DebtRouteAccessResolver,
    DamagedMapSystem, WorldEvolutionEngine, SettlementCatalog,
    FactionTerritoryCatalog + PatrolTerritoryAuthority, FieldGuideCatalog,
    AnomalyHazardSystem/Catalog, GeodeticSurveyEngine, CommsArraySystem,
    WeatherSystem, WeatherIntelligenceCoordinator, WeatherSondeSystem,
    WeatherGate* family, WeatherHardening*, WeatherRouteGateCatalog,
    WeatherForecastReliabilityEngine, WeatherAtmosphereMap, WeatherEffectsCatalog,
    SeasonalEventSystem/Catalog, FalloutSystem, LivingMapRouteProjection,
    WildlifeEcosystemSystem, CloudSeedingSystem, CargoAirdropSystem
[ ] live data: Assets/StreamingAssets/Data/wasteland_map_v1.json (nodes, routes,
    traps), locations.json, weather_route_gates.json, weather_hardening
[ ] Assets/Ashfall.Core/Expeditions/: ExpeditionSystem + Aggregate + CatalogLoader,
    ExpeditionEncounterBridge + EncounterChoiceResolver, ExpeditionLootReferenceResolver
    + Validator, TravelEncounterCombatBinder, DiscoveryConsequenceSystem,
    RailwaySystem + RailwayInterlockEngine + RailGrinding*, VehicleGarageSystem +
    Catalog + VehicleArmorGradeCatalog, AviationSystem + AerialReconWindowEngine,
    ExpeditionNavalSystem, AmphibiousDraisine* + DraisineRerailingSystem,
    ArmoredCrawler* + module catalog, MineClearingFlail*, RunFlatTireEngine,
    RadarEcmCatalog, ReconTelemetry*, ScavengingTableCatalog, VerticalAscentCatalog,
    DiveInstanceRunner, ChemicalReconEngine
[ ] Assets/Ashfall.Core/Maritime/: MaritimeDiveSystem, DiveSiteCatalog,
    StealthDiveInstance, SafeCrackingSystem, TideCalendar, ProceduralScavengeSystem,
    VariableLootNode, BlackFlotillaStanding, DeepLoreLocationCatalogLoader
[ ] Assets/Ashfall.Core/Exploration/CartographySystem.cs
[ ] Assets/Ashfall.Core/Journeys/JourneyExecutionContext.cs
[ ] host: src/Main.Expeditions.cs, Main.Maritime.cs, Main.InSarMapping.cs
[ ] existing rulings: Plan 32/32B (travel graph + knowledge gate), D16 (route hazard),
    Expansion 14 (aerial recon window)
```

### 1.1 Evidence posture

- Proposal only; no path claims; no ledger rows; read-only until Annex U.
- Core stays engine-free; data stays JSON authoritative and schema-valid.
- Determinism: travel outcomes derive from seeded RNG through the existing contract.
- One authority per concern; additive fields only; no second map/router.
- Focused verification per `TEST_POLICY.md`.

### 1.2 Known anchors (verify, don't trust)

| Anchor | Why it matters |
|---|---|
| `wasteland_map_v1.json` | the graph the player actually travels (counts last verified 22 nodes / 68 routes / 1 trap) |
| `TravelGraphKnowledgeGate` | discovery gating already exists; do not re-invent visibility |
| `MapRouteHazardEvaluator` | hazard scoring exists; travel UI must read, not compute |
| `ModalTravelDispatchEngine` | the dispatch seam; panels must not dispatch themselves |
| `ExpeditionLootReferenceResolver` | loot references must resolve; orphans are defects |
| `WeatherGate*` | weather gates already gate routes; one evaluation path |

---

## 2. The three Plan Paths

### 2.1 Path A — Truth & Maps

Audit the graph and every consumer: nodes, routes, gates, hazards, loot references,
encounter bindings. Produce the map ledger and repair only proven defects (orphaned
references, unreachable nodes, dead gates, fabricated UI values).

### 2.2 Path B — One Road

Unify travel: one route resolution, one knowledge gate, one dispatch, one hazard
evaluation, one encounter binding — used by map UI, expeditions, rail, maritime, and
AI patrols alike. No caller computes its own travel.

### 2.3 Path C — Living World

Make the world change with time: weather/season gates, road degradation, evolving
locations, discovery consequences, and a field guide that records what the shelter
has learned — all deterministic and save-backed through W4-01 discipline.

---

## 3. The ten decision points

### 3.1 Point 1 — Map graph truth

**Owner anchor:** `WastelandMapSystem`, `WastelandMapCatalogLoader`, live JSON.
**Decision:** the graph is complete and consistent: every route connects existing
nodes, every node is reachable at some knowledge tier, every trap/gate reference
resolves; counts are recorded and kept current.

- **A:** node/route/reference ledger with orphan scan; live reachability analysis.
- **B:** graph integrity gate: new map data must pass reference + reachability checks.
- **C:** graph growth policy: how new regions attach without breaking old saves.

**Verify:** reference-resolution test over the live JSON; reachability assertion for
each node under some tier; trap/gate reference check.
**Never:** a node with no path to it, a route to a missing node, or a second map file.

### 3.2 Point 2 — Knowledge gating and discovery

**Owner anchor:** `TravelGraphKnowledgeGate`, `CartographySystem`, `DamagedMapSystem`.
**Decision:** the player travels on knowledge, not telepathy: Unknown routes cannot be
planned; discovery comes from surveys, maps, rumor, and expeditions, each with a
recorded source.

- **A:** audit current tiers vs live data; list nodes that are visible but should not be.
- **B:** one gate consumed by every planner (map UI, dispatch, AI); sources recorded as
  facts (survey/map/rumor/expedition) with provenance.
- **C:** knowledge loss and age: damaged maps degrade; rumor can be wrong; surveys can
  be superseded — deterministic and save-backed.

**Verify:** gate matrix test (tier × planner); provenance recording; wrong-rumor test.
**Never:** a planner bypassing the gate, or tier state stored outside its owner.

### 3.3 Point 3 — One route resolution authority

**Owner anchor:** `ModalTravelDispatchEngine`, route family, `LivingMapRouteProjection`.
**Decision:** exactly one deterministic function answers route feasibility, distance,
time, and risk; every consumer reads its projection.

- **A:** find every place that computes travel today (hosts, panels, AI) and list them.
- **B:** one projection consumed everywhere; panels render, dispatch executes.
- **C:** projection stability contract: results versioned with the data that produced
  them so saves don't silently re-route.

**Verify:** consistency test (all consumers agree); determinism replay; panel-read
audit.
**Never:** travel math in a panel, divergent ETA/risk between surfaces, or a second
dispatch path.

### 3.4 Point 4 — Weather, seasons, and gates

**Owner anchor:** `WeatherSystem`, `WeatherGate*`, `WeatherForecastReliabilityEngine`,
`WeatherHardeningSystem`, `SeasonalEventSystem`.
**Decision:** weather affects travel through declared gates with warned consequences;
forecasts are reliable within stated limits; hardening pays off visibly.

- **A:** audit which gates exist in data vs which are evaluated; find dead gates and
  ungated routes that data claims are gated.
- **B:** one evaluation path feeding route resolution; warnings before departure;
  forecast reliability bounded and displayed.
- **C:** seasonal arcs: gates change by season; hardening upgrades route availability
  persistently and are recorded in save.

**Verify:** gate evaluation matrix; forecast-band test; hardening persistence test.
**Never:** a surprise closure with no warning, or two forecast truths.

### 3.5 Point 5 — Crossings and arbitration

**Owner anchor:** `CrossingArbitrationSystem`, `CrossingSession`, catalog + host.
**Decision:** dangerous transit resolves through the arbitration grammar with one
session, one outcome, and consequences routed once.

- **A:** audit crossing data/hosts; list crossings that bypass arbitration.
- **B:** one session lifecycle with save mid-crossing and resume; outcome application
  exactly once (keyed).
- **C:** crossing variety on existing owners: hazards, choices, and aftermath that feed
  W3-01 narrative and W3-04 combat without duplicating them.

**Verify:** session save/resume test; outcome-once test; hazard application checks.
**Never:** a crossing that can double-apply its result or lose mid-run state.

### 3.6 Point 6 — Expedition system cohesion

**Owner anchor:** `ExpeditionSystem` + aggregate/catalog/bridge/resolvers.
**Decision:** expeditions are one grammar: party, vehicle, supplies, route, encounters,
loot, return, convalescence — with every reference resolving and every consequence
recorded once.

- **A:** loot-reference and encounter-binding audits against live data; orphan list.
- **B:** one aggregate; host panels read it; encounter bridge and combat binder are the
  only paths into their domains.
- **C:** expedition depth: multi-leg journeys, discovery consequences, and knowledge
  payoffs feeding the field guide and W3-01.

**Verify:** aggregate round-trip; loot resolution coverage; encounter binding test;
consequence-once checks.
**Never:** an expedition panel computing its own loot or a consequence applied twice.

### 3.7 Point 7 — Vehicles, rail, and logistics

**Owner anchor:** `VehicleGarageSystem`, `RailwaySystem`, `AviationSystem`,
`ExpeditionNavalSystem`, draisine/crawler/flail engines.
**Decision:** vehicles are systems with condition, fuel/power, modules, and failure
grammar; rail is a network with interlocks; aviation has weather windows; each reads
one logistics truth.

- **A:** audit vehicle fleets, rail segments, and their data references; find dead
  modules and unused catalogs.
- **B:** one condition/fuel authority per vehicle class; rail interlock as the single
  signaling truth; aviation reads the recon-window evaluator.
- **C:** logistics arcs: repair economy, fuel chains, and network expansion on existing
  owners (W3-05 materials, W4-03 power).

**Verify:** vehicle condition round-trip; rail interlock scenarios; aviation window
matrix; module reference coverage.
**Never:** a vehicle that repairs itself, rail signaling computed in two places, or a
second fuel ledger.

### 3.8 Point 8 — Maritime and dive grammar

**Owner anchor:** `MaritimeDiveSystem`, `DiveSiteCatalog`, `TideCalendar`,
`StealthDiveInstance`, `SafeCrackingSystem`, host `Main.Maritime.cs`.
**Decision:** dives use one instance grammar (entry, air/time budget, stealth, loot,
hazards, extraction) with tides and sites owned by their catalogs.

- **A:** site/table reference audit; unused dive content list; host wiring check.
- **B:** one dive runner; air/stealth/economy read existing owners; outcomes applied
  once; save mid-dive per the crossing/session pattern.
- **C:** maritime arcs: flotilla standing, lore discoveries, and wreck futures on
  existing narrative/society owners.

**Verify:** dive instance save/resume; tide gate test; loot resolution; outcome-once.
**Never:** a second dive runner or dive state cached in a panel.

### 3.9 Point 9 — Location evolution and decay

**Owner anchor:** `LocationEvolutionSystem`, `LandmarkDegradationSystem`,
`WorldEvolutionEngine`, `EvolvingWorldCatalog`.
**Decision:** the world ages deterministically: locations evolve, landmarks degrade,
and those changes are visible on the map, in travel, and in the field guide.

- **A:** audit evolution rules vs live data; find inert locations with evolution
  entries and evolving locations with none.
- **B:** one evolution tick per day; changes captured in save; effects consumed by
  travel, encounters, and narrative.
- **C:** decade-scale: evolution states accumulate into distinguishable eras of the
  land without resetting the map identity.

**Verify:** deterministic evolution replay; save round-trip of evolution state;
map/field-guide consumption checks.
**Never:** wall-clock-driven evolution or divergent evolution between save/load.

### 3.10 Point 10 — Travel surfaces and the field guide

**Owner anchor:** map UI (W3-06 coordination), `FieldGuideCatalog`,
`CartographySystem`, `RouteAvailabilityPresentation`.
**Decision:** the player sees truthful travel: what they know (and how), why a route is
closed, what it will cost, and what they learned — with the field guide as the written
record.

- **A:** surface audit: fabricated values, stale tiers, unreadable gates, unlisted
  discoveries.
- **B:** surfaces read projections and gates; every unavailable route states its
  reason; departures warn before committing.
- **C:** the field guide grows with discoveries and survives migrations (W4-01);
  rumored vs surveyed distinctions are preserved in writing.

**Verify:** W3-06 kits over map/expedition surfaces; reason-present test for closures;
field-guide round-trip.
**Never:** a map that knows more than the shelter, a fabricated ETA, or a discovery
that leaves no record.

---

## 4. Selection sheet

```text
ASHFALL WAVE 4 · PLAN W4-02 · SELECTION SHEET

Plan Path:   [ ] A Truth & Maps   [ ] B One Road   [ ] C Living World

Points (mark A/B/C or leave default):
 1 map graph truth ......... [ ]
 2 knowledge gating ........ [ ]
 3 route resolution ........ [ ]
 4 weather/seasons/gates ... [ ]
 5 crossings ............... [ ]
 6 expeditions ............. [ ]
 7 vehicles/rail/logistics . [ ]
 8 maritime/dives .......... [ ]
 9 location evolution ...... [ ]
10 surfaces/field guide .... [ ]

Selected by: ____________   Date: ________   Foreman: ____________
```

---

## 5. Phase ladder

| Phase | Name | Exit |
|---|---|---|
| P0 | Premise audit + map ledger | graph/reference ledger filed; premises re-verified |
| P1 | Knowledge + resolution unification | gate matrix + consumer-consistency green |
| P2 | Weather/crossing discipline | gate evaluation + session save/resume green |
| P3 | Expeditions + vehicles | aggregate/condition/rail/aviation kits green |
| P4 | Maritime + dives | instance grammar + tide/loot tests green |
| P5 | Evolution + surfaces | deterministic replay; map/field-guide kits green |
| P6 | Closeout | evidence pack; determinism; limitations recorded |

---

## 6. Non-goals, never-touch, one-authority

**Non-goals**

- No new map generator, no procedural region rewrite, no second travel model.
- No new vehicles/regions beyond authored data; no combat re-implementation (W3-04).
- No economic or narrative duplication (W3-02/W3-01 own those consequences).
- No wall-clock systems; no unseeded randomness.

**Never-touch**

- `wasteland_map_v1.json` semantics without the integrity pipeline.
- Plan 32/32B, D16, Expansion 14 rulings and their consumption paths.
- Weather gate semantics without forecast-reliability coordination.
- Quarantined tests; sealed debt rows.

**One authority per concern**

| Concern | Owner |
|---|---|
| graph | `WastelandMapSystem` + JSON |
| knowledge | `TravelGraphKnowledgeGate` + `CartographySystem` |
| resolution | `ModalTravelDispatchEngine` + route/projection family |
| hazards | `MapRouteHazardEvaluator` |
| weather gates | `WeatherGate*` family |
| expeditions | `ExpeditionSystem` + aggregate family |
| rail | `RailwaySystem` + interlock |
| dives | `MaritimeDiveSystem` + dive runner |
| evolution | `WorldEvolutionEngine` + location evolution family |

---

## 7. Verification and acceptance

- **T1 static:** reference scans (routes/traps/gates/loot), consumer inventory,
  panel-read audit, no second-map/route checks.
- **T2 focused:** gate matrix; consumer consistency; session save/resume; outcome-once;
  vehicle/rail/aviation kits; dive instance; evolution replay; field-guide round-trip;
  W3-06 surface kits.
- **T3 soak:** 60-day world at seeded weather: route availability trend, evolution
  determinism, expedition cycles, zero orphan drift.
- **Acceptance:** evidence pack + Annex U signature; compile-green is not acceptance.

## 8. Handoffs and dependencies

| Direction | Detail |
|---|---|
| W2-04 / W2-05 | environment/location planning feeds this plan's ledger |
| W3-01 | discovery journaling, arc hooks for sites and journeys |
| W3-02 | caravan/trade routes read the same graph and gates |
| W3-04 | encounter combat binding and threat escalation in the field |
| W4-01 | every world state (knowledge tiers, evolution, field guide) ships its save section |
| W4-03/04 | weather hardening, power/fuel coupling |
| UNBLOCK-04/W11-12 | travel-graph knowledge gate + route hazard precedents |

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What this plan releases

| Release | Unblocks |
|---|---|
| U1 | live-world integration for Plan 32/32B and D16 consumers |
| U2 | expedition loot/encounter orphan repairs held for "map data pending" |
| U3 | vehicle/rail/aviation depth work blocked on route-authority unification |
| U4 | maritime/dive content blocked on instance save/resume grammar |
| U5 | location evolution and field-guide work blocked on save-section discipline |

## U.2 Signature block

```text
ASHFALL WAVE 4 · PLAN W4-02 · RELEASE SIGNATURE
HEAD: ________  Date: ________
[ ] P0 premise audit completed and filed
[ ] map/reference ledger exists; orphans listed
[ ] no path claimed outside the package
[ ] focused test targets named
[ ] rollback position recorded
Signed: ________   Foreman: ________
```

## U.3 Never-touches

- No edit to another Wave 4 plan's claimed paths.
- No re-open of Plan 32/32B, D16, or Expansion 14 closings.
- No change to weather data semantics without gate-family coordination.
- No revival of quarantined tests outside the documented procedure.

## U.4 Release rule

> This plan executes only after U.2 is signed. Until then it is read-only planning.

---

*End of W4-02 — Part I. Expansion parts continue on the established Wave 3 pattern.*---

# W4-02 · PART II — DEEP DESIGN: THE MAP, KNOWLEDGE, RESOLUTION (POINTS 1–3)

## II.1 The map graph: data contract

`wasteland_map_v1.json` (with `WastelandMapCatalogLoader`) is the graph. The
design makes its
contract explicit so the graph can be verified like any other authority.

### II.1.1 Node record

```jsonc
{
  "id": "n_07",
  "name_ref": "loc_07",
  "region": "r_north",
  "kind": "settlement",          // settlement | outpost | ruins | waypoint | site
  "elevation_m": 210,            // used by atmosphere/weather coupling
  "map_pos": { "x": 1180, "y": 640 },
  "services": ["water", "trade"],// declared, consumed by UI and AI
  "special": []                  // authored flags with named consumers
}
```

### II.1.2 Route record

```jsonc
{
  "id": "r_112",
  "from": "n_07",
  "to": "n_12",
  "distance_km": 14,
  "kind": "road",                // road | rail | water | foot | tunnel
  "seasonal": null,              // gate reference when gated
  "hazards": ["trap_rubble"],    // references into hazard data
  "infrastructure": "bridge_2"   // optional structure dependency
}
```

### II.1.3 Graph invariants

```text
G1  every route endpoint exists
G2  every node is reachable from the home node under some knowledge tier
G3  every hazard/gate/infrastructure reference resolves
G4  ids are stable; renames ship with a mapping (save compatibility, W4-01)
G5  no duplicate route ids; no self-loops except declared local paths
G6  every node has at least one service entry or a declared "barren" flag
```

These six invariants are the T1 gate for map data.

## II.2 Knowledge gating: the tier model

`TravelGraphKnowledgeGate` owns what the shelter knows. The tiers:

```text
Unknown   — nothing on the map; cannot be planned
Rumored   — approximate; plan possible with risk penalty; may be wrong
Surveyed  — verified existence and rough distance; safe planning
Mapped    — full detail: distance, hazards, gates, services
```

### II.2.1 Sources and provenance

Every tier upgrade records its source:

```text
survey (cartography/expedition) | map_fragment | rumor | trade_route | capture
```

Provenance is durable (W4-01), bounded (one row per node), and surface-visible
in the field guide.

### II.2.2 Wrong-rumor design

A rumored node may be wrong: the site exists but not as described, or the route
claims a path that has collapsed. Wrong rumors:

- come only from the rumor source, never from surveys;
- are corrected on arrival or by a later survey;
- are deterministic (seeded), not random per load.

### II.2.3 The gate matrix

| Tier | Map shows | Plan allowed | Risk display |
|---|---|---|---|
| Unknown | nothing | no | — |
| Rumored | marker only | yes (penalty) | "reports suggest…" |
| Surveyed | node + rough route | yes | estimated window |
| Mapped | full details | yes | exact window |

## II.3 Route resolution: one authority

`ModalTravelDispatchEngine` (with the route family and
`LivingMapRouteProjection`) resolves every question of connectivity.

### II.3.1 The projection record

```jsonc
{
  "from": "n_07", "to": "n_12",
  "feasible": true,
  "reason": null,                // when infeasible: gate | hazard | knowledge
  "distance_km": 14,
  "hours": 6,                    // deterministic from kind + vehicle + weather
  "risk": "moderate",            // derived from hazards + season + standing
  "gates": [ { "id": "g_winter", "state": "open" } ],
  "knowledge_min": "surveyed"
}
```

### II.3.2 Resolution rules

```text
R1  feasibility = graph path × knowledge tier × gate state × hazard state
R2  distance = authored graph data (never computed from pixels)
R3  time = dist × terrain factor × vehicle factor × weather factor
R4  risk = authored hazard weights adjusted by preparation (escort, hardening)
R5  results are deterministic; identical inputs give identical projections
R6  no caller computes its own values; panels render projections
```

### II.3.3 The consumer-consistency test

Every consumer (map UI, expedition planner, caravan AI, patrol AI) is asked the
same route question with a fixture; all answers must equal the projection. A
divergence is a defect in the consumer, never in "the map" — because there is
one map.

## II.4 Worked example: the route that two systems disagreed about

Two surfaces showed different times for the same route: the map said 6 hours,
the expedition planner said 9.

**Walk:**

```text
1. both read the same graph; the difference was in terrain factors
2. the planner folded vehicle speed at plan time; the map folded it at display
   time with a different constant
3. root: the terrain/vehicle factor table lived in two places
4. repair: factor table owned by the resolution authority; both consume it
5. verify: consumer-consistency test over 20 fixture routes
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-01 | duplicated terrain factors | duplication | one factor table |
| WX-02 | no consumer-consistency test | coverage | add kit |

*End of Part II. Continues in Part III (weather gates and crossings).*---

# W4-02 · PART III — DEEP DESIGN: WEATHER, CROSSINGS, EXPEDITIONS (POINTS 4–6)

## III.1 Weather gates: the gate model

`WeatherGate*` family (catalog, context evaluator, modifier, radio hooks) plus
`WeatherSystem`, `WeatherForecastReliabilityEngine`, and
`WeatherHardeningSystem`.

### III.1.1 Gate record

```jsonc
{
  "id": "g_winter_pass",
  "route": "r_112",
  "season": "winter",
  "condition": "snow_depth_over_30cm",
  "effect": "closed",            // closed | delay | risk_up | detour
  "detour_route": "r_118",
  "warning_days": 3,
  "hardening": "plow_upgrade"    // optional mitigation reference
}
```

### III.1.2 Evaluation rules

```text
W1  gates evaluate from (weather state, season, hardening, route)
W2  the gate result enters route resolution, never bypasses it
W3  every closure has a warning window >= warning_days (or authored urgent)
W4  forecast reveals gates within reliability bounds (W4-01-safe, deterministic)
W5  hardening relaxes specific gates only; effects are persistent and saved
W6  gates never flicker: hysteresis on reopen
```

### III.1.3 The frost scenario

```text
day 100: forecast shows a cold front in 3 days
day 102: gate g_winter_pass moves to delay(12h) with warning
day 103: gate closes; route r_112 infeasible; detour r_118 offered
player with plow_upgrade: gate stays delay(4h), route feasible
after front: hysteresis keeps gate delay(6h) for 1 day, then reopens
```

Every step is deterministic and save-backed; reloading mid-front preserves the
gate state and its hysteresis.

## III.2 Crossings: the arbitration grammar

`CrossingArbitrationSystem` + `CrossingSession` + catalog + host.

### III.2.1 The session lifecycle

```text
open -> staged -> arbitrating -> resolved -> aftermath
        |            |              |
        +-- save/resume at every arrow (step recorded)
```

### III.2.2 Outcome application

```text
O1  one outcome key per crossing instance
O2  consequences apply exactly once (W3-01 consequence ledger keys)
O3  hazards apply at their authored step, not at session end
O4  mid-session save stores the step, not a snapshot of derived UI state
O5  aborting a crossing (flee) is a first-class outcome, not a failure
```

### III.2.3 The crossing catalog audit

Every crossing entry must name: trigger (route + condition), stages, decision
points, hazards, outcomes, and aftermath hooks. Entries that name none of these
are inert data — the audit lists them (findings class: content).

## III.3 Expeditions: the aggregate

`ExpeditionSystem` + `ExpeditionAggregate` + catalog + encounter bridge +
`EncounterChoiceResolver` + `ExpeditionLootReferenceResolver/Validator` +
`TravelEncounterCombatBinder` + `DiscoveryConsequenceSystem`.

### III.3.1 The aggregate record

```jsonc
{
  "id": "ex_044",
  "party": ["sv_014", "sv_022"],
  "vehicle": "vc_truck",
  "route": "r_112",
  "stage": "outbound",           // prep | outbound | site | return | debrief
  "supplies": { "fuel": 18, "water": 12, "food": 20 },
  "discoveries": [],
  "encounters": [ { "id": "en_9", "resolved": true } ],
  "loot": [ { "item_ref": "it_gear", "resolved": true } ],
  "knowledge_gained": [{ "node": "n_12", "tier": "surveyed" }]
}
```

### III.3.2 Binding rules

```text
E1  one aggregate per expedition; stages advance deterministically
E2  loot references must resolve to real items (validator gate)
E3  encounters bind through the bridge; combat through the W3-04 binder
E4  discoveries route to knowledge/V gate and journal (W3-01)
E5  consequences (injuries, losses, finds) apply once, keyed
E6  return/debrief closes the aggregate; nothing lingers in "almost done"
```

### III.3.3 The orphan audit

Run `ExpeditionLootReferenceResolver` against the live data: every loot entry
resolves; every encounter id exists; every referenced route/node exists. Orphans
are findings with named repairs (add content or remove reference).

## III.4 Worked example: the expedition that never came back

**Report:** an expedition stuck in "return" for days; no events.

**Walk:**

```text
1. root: the return stage waited on a debrief encounter that was gated on a
   condition never met (party full health), so the stage could not advance
2. deeper: no stage timeout existed; the aggregate had no "force resolve" path
3. repair: stage timeout policy (authored days) with a written outcome; the
   aggregate always resolves; stuck rows are a gate failure
4. verify: stage timeout test; stuck-expedition scan
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-03 | stage without timeout | completeness | authored timeout |
| WX-04 | no stuck-expedition scan | coverage | add kit |
| WX-05 | debrief gate unreachable | content | condition authored |

*End of Part III. Continues in Part IV (vehicles, maritime, evolution, surfaces).*---

# W4-02 · PART IV — DEEP DESIGN: VEHICLES, MARITIME, EVOLUTION, SURFACES (POINTS 7–10)

## IV.1 Vehicles, rail, aviation

### IV.1.1 Vehicle record

```jsonc
{
  "id": "vc_truck",
  "class": "truck",              // truck | crawler | draisine | boat | aircraft
  "condition": 84,               // percent; wear deterministic
  "fuel": 42,                    // units; consumption per km authored
  "modules": ["armor_1", "winch"],
  "home": "n_07",
  "state": "ready"               // ready | maintenance | disabled
}
```

### IV.1.2 Rules

```text
V1  one condition ledger per vehicle class; wear per km + per event
V2  fuel consumption from the route projection (no local math)
V3  modules must resolve to real module data; dead modules are findings
V4  vehicles that reach 0 condition disable with a warned outcome; no silent loss
V5  repair consumes materials/labor via W3-05 owners
V6  rail uses one interlock truth; aviation reads the weather-window evaluator
```

### IV.1.3 Rail interlock

```text
the network is segments + junctions with interlock state
one authority answers "can this consist traverse this path now"
signals derive from interlock state; no surface computes its own
blocked/occupied states are facts, saved (W4-01), and visible
```

## IV.2 Maritime and dives

### IV.2.1 The dive instance grammar

```text
prepare (site, gear, tides) -> entry -> [stealth | hazard | find]* -> extract
every arrow records a step; mid-dive save stores the step (resume grammar)
```

### IV.2.2 Rules

```text
D1  one dive runner; no panel-side dive state
D2  tides from TideCalendar; entry windows warned
D3  air/time budget is the pressure clock; extraction is always reachable with
    authored rules (a dive can end badly but never hang)
D4  loot resolves through the same validator as expedition loot
D5  safe-cracking/stealth outcomes apply once, keyed
D6  flotilla standing changes route through W4-05 owners
```

### IV.2.3 The tide window surface

The surface shows the next window, the risk at the edges, and why a site is
currently closed — read from the calendar and gate evaluation, never computed.

## IV.3 Location evolution and degradation

### IV.3.1 Evolution record

```jsonc
{
  "node": "n_12",
  "stage": "settled",            // authored stages per site kind
  "since_day": 160,
  "modifiers": ["traded", "raided_once"],
  "landmark": { "id": "lm_4", "condition": 62 }
}
```

### IV.3.2 Rules

```text
L1  evolution ticks once per day, deterministic from (day, node, drivers)
L2  stages are authored per kind; no free-form state growth
L3  landmark condition degrades by authored rates; repairs via existing crafts
L4  map/field-guide/travel read evolution state; nothing else re-derives it
L5  evolution is saved (W4-01); a reload continues the same arc
L6  no wall-clock; campaign day only
```

### IV.3.3 The decade view

Over hundreds of days, sites move through authored eras: outpost → settled →
fortified, or ruins → salvaged → stripped. The field guide records the story in
restrained prose (W2-06 corpus routing), while the model stores facts.

## IV.4 Travel surfaces and the field guide

### IV.4.1 Surface contract

```text
map:        tiers, routes, gates with reasons, projections on hover/select
planner:    route options with time/risk/supply needs, warnings before commit
expedition:  stages, party, supplies, expected return, live updates
field guide: written record of discoveries, surveys, losses, and rumors
```

### IV.4.2 Rules

```text
S1  every surface reads projections/gates/aggregates; none compute travel
S2  an infeasible route always states its reason (knowledge/gate/hazard)
S3  warnings precede commitment: supply shortage, closed gate, storm window
S4  ETA is a range when knowledge < Mapped, exact when Mapped
S5  the field guide is durable, written, and grows only through real events
S6  W3-06 kits (route/focus/lane/feedback) cover all travel surfaces
```

### IV.4.3 The lie audit

Surface audit questions: does the map show more than the gate allows? Does an
ETA exist without a projection? Does a closure lack a reason? Does the field
guide claim a survey that never happened? Each is a finding with a repair.

## IV.5 Worked example: the map that knew too much

**Report:** after loading, unexplored nodes appeared labeled on the map.

**Walk:**

```text
1. fixtures: gate state present; labeling used node existence, not tier
2. root: the map renderer treated "has data" as "is known"
3. repair: renderer reads tier; unknown nodes render as terrain only; test
4. verify: gate matrix render test; wrong-rumor display test
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-06 | renderer ignored knowledge gate | truth | read tier |
| WX-07 | no render-knowledge test | coverage | add kit |

*End of Part IV. Continues in Part V (playbooks).*---

# W4-02 · PART V — PLAYBOOKS

## V.1 The P0 premise playbook

```text
1. freeze HEAD; read the live map JSON; count nodes/routes/gates/traps
2. enumerate consumers: map UI, planner, expeditions, caravan AI, patrol AI,
   rail, maritime, projects
3. for each consumer: where do its travel numbers come from?
4. list every gate referenced by data vs every gate evaluated in code
5. list knowledge writers and readers; find bypasses
6. list session systems (crossings/dives): can they save mid-run?
7. enumerate loot/encounter references; run the resolver; record orphans
8. write the premise note; claim paths; draft the package row
```

## V.2 The map data authoring playbook

```text
1. new node: id, name_ref, region, kind, pos, services; reachable from home
2. new route: endpoints, kind, distance, hazards, gate refs (all resolving)
3. new gate: route, season, condition, effect, warning window, hardening ref
4. run the six graph invariants + reference scans
5. add the node/route to the knowledge default set (usually Unknown)
6. if the node carries evolution stages: author them or declare "static"
7. journal/field-guide texts via corpus routing (W2-06), not inline strings
```

## V.3 The knowledge authoring playbook

```text
1. which source moves this node's tier? (survey/map/rumor/route/capture)
2. is the source deterministic and save-backed?
3. does arrival correct a wrong rumor? author the correction
4. does the field guide record the source? (provenance)
5. does the surface show the right uncertainty band?
```

## V.4 The session save playbook (crossings and dives)

```text
1. define the step list; every arrow is a resumable point
2. mid-session save stores: instance id, step, inputs, rng cursor
3. load restores into the step; UI rebuilds from state, not from a snapshot
4. abort/extract paths are outcomes with aftermath, not exceptions
5. test: save at each step; load; complete; assert outcome-once
```

## V.5 The expedition operations playbook

```text
prep:    party/vehicle/supplies validated; warnings for shortages
outbound: travel via projections; encounters through the bridge
site:    discovery resolutions; loot via validator; hazards applied
return:  travel back; losses resolved; knowledge recorded
debrief: aggregate closed; journal + board updated; no lingering rows
```

## V.6 The evolution review playbook

```text
1. for each evolving node: drivers authored? stages reachable? rates bounded?
2. landmarks: degradation rates; repair paths exist
3. reload test: evolve 10 days, save, load, evolve 10 more -> equals 20 straight
4. surface test: map/field guide reflect stages with restrained text
```

## V.7 Anti-pattern drills

**Drill 1 — the pixel distance.** A surface computes distance from map
coordinates. Compare to authored distance; fix the surface.

**Drill 2 — the optimistic ETA.** A new planner folds good weather into every
plan. With a fixture gate closed, the plan must show delay or infeasibility.

**Drill 3 — the remembered map.** A panel caches tiers for speed; after a survey
in another window, the panel lies. Refresh-on-open rule.

**Drill 4 — the immortal expedition.** Find a stage with no timeout; author one
with a written outcome.

**Drill 5 — the tide that never turned.** A dive site open at all hours. Check
the calendar reference; author the window.

*End of Part V. Continues in Part VI (verification catalog).*---

# W4-02 · PART VI — VERIFICATION CATALOG

## VI.1 T1 — static

| ID | Check | Fails when |
|---|---|---|
| T1.1 | graph invariants (G1–G6) over live JSON | broken reference/reachability |
| T1.2 | consumer inventory scan | a travel consumer not listed/known |
| T1.3 | panel-compute scan | travel math inside UI files |
| T1.4 | gate reference scan | gate in data never evaluated / vice versa |
| T1.5 | loot/encounter reference resolution | orphan ids |
| T1.6 | knowledge-writer audit | tier writes outside the gate owner |
| T1.7 | session-step audit | session without declared resumable steps |
| T1.8 | wall-clock scan in evolution/weather | non-campaign time durable |

## VI.2 T2 — focused per point

### Point 1 — graph
```text
T2.1.1 reference resolution (routes/traps/gates/infrastructure)
T2.1.2 reachability from home under Some tier for every node
T2.1.3 duplicate/self-loop rejection
```

### Point 2 — knowledge
```text
T2.2.1 gate matrix (tier × planner allowance)
T2.2.2 provenance recorded per upgrade
T2.2.3 wrong-rumor correction on arrival
T2.2.4 render test: unknown nodes show terrain only
```

### Point 3 — resolution
```text
T2.3.1 consumer consistency over 20 fixture routes
T2.3.2 determinism: same inputs, same projection ×2 runs
T2.3.3 infeasible reason present (knowledge/gate/hazard)
T2.3.4 factor tables single-sourced (no duplicates in code)
```

### Point 4 — weather gates
```text
T2.4.1 gate evaluation matrix (weather × season × hardening)
T2.4.2 warning window before closure
T2.4.3 hysteresis on reopen (no flicker)
T2.4.4 hardening persistence round-trip
T2.4.5 forecast reliability bands respected in display
```

### Point 5 — crossings
```text
T2.5.1 save/resume at every step
T2.5.2 outcome-once keys
T2.5.3 hazard application at authored step
T2.5.4 flee/abort aftermath routes correctly
```

### Point 6 — expeditions
```text
T2.6.1 aggregate round-trip (W4-01)
T2.6.2 loot resolution coverage
T2.6.3 encounter binding through bridge
T2.6.4 stage timeout forces resolution
T2.6.5 discovery → knowledge tier upgrade + journal entry
```

### Point 7 — vehicles/rail
```text
T2.7.1 condition wear determinism; 0-condition disable warning
T2.7.2 fuel consumption from projection
T2.7.3 module reference coverage
T2.7.4 rail interlock scenarios (occupied/blocked/clear)
T2.7.5 aviation window matrix (grounded/marginal/optimal)
```

### Point 8 — maritime
```text
T2.8.1 dive instance save/resume per step
T2.8.2 tide window gating + warning
T2.8.3 loot resolution; outcome-once
T2.8.4 extraction always reachable (no hang states)
```

### Point 9 — evolution
```text
T2.9.1 deterministic evolve-10-save-load-evolve-10 == evolve-20
T2.9.2 stage reachability per kind; bounded rates
T2.9.3 landmark degradation + repair path
T2.9.4 surface consumption (map/field guide)
```

### Point 10 — surfaces
```text
T2.10.1 reason-present for infeasible routes
T2.10.2 ETA bands by knowledge tier
T2.10.3 warnings before commitment (supply/gate/storm)
T2.10.4 field-guide round-trip; provenance display
T2.10.5 W3-06 kits over map/planner/expedition/dive surfaces
```

## VI.3 T3 — seeded soak

```text
60 days, seeded weather, 20 expeditions, 6 crossings, 4 dives, rail runs
assert: evolution deterministic; knowledge only grows from real sources;
        no stuck aggregates; loot orphans zero; projections stable;
        no exceptions on session resumes
```

## VI.4 Evidence formats

```yaml
run: T2.3.1
date: ____  head: ____
routes_checked: 20
consumers: [map, planner, caravan_ai, patrol_ai]
divergences: []
result: pass
```

*End of Part VI. Continues in Part VII (worked threads).*---

# W4-02 · PART VII — WORKED THREADS AND FINDINGS (WX-08–WX-20)

## VII.1 Thread A — "the road that knew the weather twice"

**Report:** the map showed a route open while the planner refused it.

**Walk:**

```text
1. both read gates; the planner consulted WeatherGateEvaluator, the map read
   the raw weather state and applied its own season rule
2. root: two gate evaluations; the map's shortcut survived a refactor
3. repair: map reads the evaluation result; the shortcut deleted; test
4. verify: consumer consistency + gate matrix
```

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-08 | second gate evaluation in UI | duplication | delete; read projection |
| WX-09 | no gate-consumer test | coverage | add to kit |

## VII.2 Thread B — "the trap that sprang twice"

**Report:** a crossing hazard applied its damage on load as well as live.

**Walk:**

```text
1. root: hazard application ran at session resolve; a resume re-ran the step
   because the step index was not persisted (only the outcome key was)
2. repair: persist the step index with the session (resume grammar); outcome +
   step keys both checked; hazard application moved behind the step gate
3. verify: save-at-step-N; load; hazard applied once
```

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-10 | step index not persisted | resume | store step with session |
| WX-11 | hazard outside step gate | integrity | step-gated application |

## VII.3 Thread C — "the depot that never existed"

**Report:** expedition loot referenced an item removed from data; the return
inventory was empty with no message.

**Walk:**

```text
1. resolver returns unresolved references silently (log only)
2. root: validator existed but was not run on the live path; orphan tolerated
3. repair: resolution failure is a typed outcome with a fallback item or an
   explicit "nothing salvageable" message; validator runs in T1 and live
4. verify: orphan scan zero; failure-path test
```

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-12 | unresolved loot tolerated | completeness | typed outcome + fallback |
| WX-13 | validator not live | coverage | run resolver at bind |

## VII.4 Thread D — "the patrol that walked through walls"

**Report:** faction patrols moved along routes that were closed to the player.

**Walk:**

```text
1. patrol AI had its own pathfinder ignoring gates
2. root: AI predates the gate system; "temporary" exemption
3. repair: AI reads the resolution authority; a patrol may ignore *knowledge*
   (they know their land) but never *physical* gates
4. verify: AI-consumer consistency; closed-gate patrol scenario
```

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-14 | AI bypassed gates | duplication | read projections |
| WX-15 | no AI-consistency test | coverage | add to kit |

## VII.5 Thread E — "the map that forgot the war"

**Report:** after a loading-port change, the map still showed a ruined bridge
restored.

**Walk:**

```text
1. root: route infrastructure state changed but the route record cached its
   "traversable" flag at load
2. repair: traversability derives from infrastructure state at read; no cached
   flags on routes
3. verify: infrastructure-change → route reflection test
```

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-16 | cached traversability | derived policy | derive at read |
| WX-17 | no change-reflection test | coverage | add to kit |

## VII.6 Thread F — "the tide that saved itself wrong"

**Report:** reloading mid-dive moved the tide window, sometimes stranding the
party.

**Walk:**

```text
1. root: the dive session stored elapsed real minutes, and window math used
   them against a campaign-day calendar
2. repair: session stores campaign step + campaign time; window math reads the
   calendar; no real-time state durable
3. verify: mid-dive save/load at three steps; window identical
```

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-18 | real-time elapsed persisted | determinism | campaign time only |
| WX-19 | no window-stability test | coverage | add to kit |

## VII.7 Thread G — "the field guide that read like a spreadsheet"

**Review:** the field guide listed raw node ids and tier enums.

**Walk:**

```text
1. root: surface rendered model fields directly; no authored prose
2. repair: corpus entries per event kind (W2-06 routing); ids resolve to names;
   tiers render as restrained phrases
3. verify: text-ref coverage; no raw ids in copy
```

| ID | Finding | Class | Repair |
|---|---|---|---|
| WX-20 | raw model fields surfaced | presentation | authored refs |

## VII.8 The summary

```text
A: one gate evaluation, read everywhere
B: sessions store steps; hazards are step-gated
C: references resolve or fail loudly
D: AI obeys physical law, not knowledge law only
E: traversability is derived, never cached
F: campaign time only in durable state
G: surfaces speak human, not schema
```

*End of Part VII. Continues in Part VIII (Q&A).*---

# W4-02 · PART VIII — QUESTIONS AND ANSWERS

**Q1. Why does one plan cover map, travel, vehicles, and maritime?**
Because they are one question — "can we go, how, and what will it cost" — with
one answer authority. Splitting the plan would re-split the answer.

**Q2. What is the map, in one sentence?**
Authored nodes and routes with a knowledge overlay and a single resolution
function.

**Q3. What is the knowledge gate for?**
So travel is planning under uncertainty, not teleportation by data presence.

**Q4. Can the player ever see the full graph?**
Only what the shelter knows. Debug views aside, play shows tiers.

**Q5. What does a wrong rumor look like mechanically?**
A rumored tier with a modified descriptor; arrival corrects it; the correction
is recorded in the field guide as a fact.

**Q6. How many resolution callers are there?**
One authority, many consumers. The test enforces that they agree.

**Q7. What if a consumer legitimately needs a different number?**
It reads the same projection and applies a declared, authored adjustment (e.g.,
escort risk). It never computes from raw data.

**Q8. How does weather interact with planning?**
Through gates: evaluated once, consumed everywhere, warned before departure.

**Q9. What makes a gate "fair"?**
A warning window, an authored condition, a state the player can affect
(hardening, timing, detour), and no flicker.

**Q10. What is a crossing, structurally?**
A staged session on a route with arbitration, decisions, hazards, and an
outcome — resumable at every step.

**Q11. Why do crossings need save/resume?**
Because players save anywhere; a session that cannot resume becomes either a
lost state or a duplicated one. Both are trust bugs.

**Q12. What is an expedition's durable shape?**
The aggregate: party, vehicle, route, stage, supplies, discoveries, encounters,
loot, knowledge gained — one record, one lifecycle.

**Q13. How do expeditions touch combat?**
Through the existing encounter bridge and the W3-04 combat binder. The
expedition plans; combat resolves; the aggregate records the result.

**Q14. What is the loot validator for?**
So a return never silently loses a find or conjures an item that no longer
exists.

**Q15. How are vehicles different from expeditions?**
Vehicles are assets with condition and fuel; expeditions are missions that may
use them. Neither owns the other.

**Q16. What is the rail interlock, in one line?**
One authority answering whether a consist can traverse a path now.

**Q17. Why does maritime get its own grammar?**
Dives have entry/exit, air budgets, tides, and extraction rules — a session
grammar, like crossings, with its own step list.

**Q18. Can a dive hang forever?**
No. Extraction is always reachable under authored rules; "ending badly" is an
outcome, hanging is a defect.

**Q19. What does location evolution change?**
Stages and landmark condition, on authored drivers and rates — visible in the
field guide and travel, deterministic, saved.

**Q20. Why is evolution deterministic?**
Because the world must be the same world after a reload. Any randomness uses
the seeded path with the cursor saved (W4-01).

**Q21. What is the field guide's role?**
The written memory of the land: what was found, surveyed, lost, and corrected.

**Q22. Does the field guide duplicate the journal?**
No. The journal is the shelter's story; the field guide is the land's record.
They cross-reference, they do not merge.

**Q23. What is the "lie audit"?**
The surface review asking whether the map/planner/expedition UI shows more,
less, or different than the owners say.

**Q24. How does trade use this plan?**
Caravans route through the same graph, gates, and projections; the economy plan
owns prices, this plan owns passage.

**Q25. How does combat use this plan?**
Encounters bind on the road through the existing bridge; war stages change
access through gates and infrastructure, not through a second map.

**Q26. What is the smallest useful increment?**
Path A, points 1–3: graph ledger, gate matrix, consumer consistency. Travel
becomes trustworthy even before depth work.

**Q27. What does Path B add?**
One road: every consumer on one resolution, sessions with resume grammar,
vehicles/rail/maritime calm, surfaces truthful.

**Q28. What does Path C add?**
A world that changes across years: evolution eras, hardening, knowledge history,
landmark decay and repair — all saved and deterministic.

**Q29. What is explicitly out of scope?**
New regions as content (authored elsewhere), map generators, real-time strategy
layers, naval combat systems (W3-04's domain), economy rebalancing.

**Q30. What is the acceptance shape?**
Graph invariants green, gate matrix green, consumer consistency green, session
resume green, orphans zero, evolution replay green, surfaces truthful.

**Q31. What is the biggest risk?**
A "convenient" second pathfinder appearing for AI or UI. The consumer test
exists to catch exactly that.

**Q32. The second risk?**
Session state saved by snapshot instead of step. Snapshots rot; steps resume.

**Q33. The third?**
Surface optimism: ETAs and feasibility that outrun the model.

**Q34. How are new regions added safely?**
Authored nodes/routes, invariant checks, knowledge defaults Unknown, evolution
declared, and save-compatible ids.

**Q35. What does "the land is a promise" mean here?**
What the map shows is what travel does — no more, no less, every time.

*End of Part VIII. Continues in Part IX (Path C designs).*---

# W4-02 · PART IX — PATH C IMPLEMENTATION DESIGNS (C1–C10)

## C1 — The living atlas

```text
design: knowledge history per node (who learned what, when, from where),
        bounded and saved; the field guide renders the story
acceptance: provenance round-trip; guide entries only from real sources
```

## C2 — The era model

```text
design: authored stages per node kind with entry/exit conditions; daily tick;
        visual + travel consequences read from stage
acceptance: stage reachability; deterministic replay; save round-trip
```

## C3 — The hardening web

```text
design: weather hardening upgrades target specific gates; persistence; visible
        payoff (closure → delay); costs through W3-05
acceptance: gate matrix before/after; save round-trip
```

## C4 — The route economy

```text
design: route quality/condition evolves with use and maintenance; affects time
        and wear; consumed by caravans and expeditions
acceptance: quality determinism; consumption by resolution; no second model
```

## C5 — The watch system

```text
design: standing observation posts and ranged patrols raise knowledge tiers and
        warn of threats; fed by W4-03 power/labor
acceptance: observation → tier upgrades; warning delivery; once-only
```

## C6 — The deep grammar

```text
design: dive site populations and flotilla arcs across years; sites deplete and
        recover; standing routes through W4-05
acceptance: site state determinism; standing consequences once
```

## C7 — The rail age

```text
design: network segments reclaimed over years; interlock safety systems grow;
        freight capacity feeds W3-02
acceptance: interlock scenarios; freight consumption; reclaim arcs saved
```

## C8 — The map that teaches

```text
design: rumor/survey lessons through narrative hooks (W3-01); wrong rumors with
        authored corrections; field guide as the textbook
acceptance: correction flow; journal cross-references; no spoiler leakage
```

## C9 — The grave roads

```text
design: routes that become haunted/taboo through history; travel through them
        carries morale/consequence weight via W3-03
acceptance: history → gate/modifier routing; once-keys; restrained copy
```

## C10 — The final map shape

```text
design: when the plan closes, the map is: authored, known-by-discipline,
        gated-by-weather, resolved-by-one-authority, evolved-by-time,
        written-in-the-guide — and every claim testable
acceptance: closure measurement (§XVII)
```

*End of Part IX. Continues in Part X (checklists).*---

# W4-02 · PART X — CHECKLISTS AND WORKSHEETS

## X.1 The P0 worksheet

```text
PACKAGE: ______  HEAD: ______  DATE: ______
[ ] nodes counted: __ / routes: __ / gates: __ / traps: __
[ ] consumers listed (n): __
[ ] consumers computing travel locally: __ (names)
[ ] knowledge writers listed; bypasses found: __
[ ] gates in data not evaluated: __  evaluated but not in data: __
[ ] session systems listed; resumable? y/n each
[ ] loot/encounter orphans: __
[ ] evolution entries vs evolving nodes: __ / __
[ ] premises contradicted: __ (attach)
```

## X.2 The new content worksheet

```text
NODE: ______  REGION: ______  KIND: ______
[ ] id stable; name_ref resolves
[ ] reachable under some tier
[ ] services authored or barren declared
[ ] evolution stages authored or static declared
ROUTE: ______  endpoints: __ -> __
[ ] distance authored; kind; hazards resolve; gates resolve
GEN: [ ] invariants green  [ ] reference scan zero  [ ] tier default Unknown
COPY: [ ] journal/guide refs routed (W2-06)
```

## X.3 The session worksheet

```text
SESSION: crossing | dive   ID: ______
steps: __ (list)
[ ] every arrow resumable
[ ] step index persisted
[ ] hazards step-gated
[ ] outcome + step keys checked once
[ ] abort outcome + aftermath authored
[ ] save/load at each step tested
```

## X.4 The evolution worksheet

```text
NODE: ______  KIND: ______
stages: __ (list)  drivers: __
[ ] entry/exit conditions authored
[ ] rates bounded; no wall-clock
[ ] replay test (10+10 == 20)
[ ] landmark rates + repair path
[ ] surface consumption (map/guide)
```

## X.5 The surface checklist

```text
[ ] map shows tiers, not data presence
[ ] infeasible routes state reasons
[ ] ETAs banded by tier
[ ] warnings before commitment
[ ] field guide entries human, referenced, durable
[ ] W3-06 route/focus/lane/feedback kits green
```

## X.6 The consumer consistency worksheet

```text
route fixture set: __ routes
map:        __ | planner: __ | caravan: __ | patrol: __ | rail: __ | dive: __
divergences: __ (list) -> owner per divergence
```

*End of Part X. Continues in Part XI (field guide and maintenance).*---

# W4-02 · PART XI — FIELD GUIDE, MAINTENANCE, AND CLOSURE DISCIPLINE

## XI.1 The one-page field guide

```text
TRAVEL SHIPS WHEN:
  every route resolves through one authority
  every consumer agrees with it
  knowledge gates what the map shows
  gates warn before closing and reopen with hysteresis
  sessions resume from steps, hazards apply once
  expeditions always close
  vehicles wear, rail interlocks, dives have tides
  evolution is deterministic and saved
  surfaces state reasons and uncertainty
```

## XI.2 The maintenance calendar

| Cadence | Task |
|---|---|
| per content change | graph invariants + reference scans |
| weekly | consumer consistency spot (five routes) |
| per release | gate matrix; session resume kit; orphan scan |
| seasonal | evolution replay; hardening audit; field-guide review |
| yearly | map census (nodes/routes/gates); knowledge-writer audit |

## XI.3 The stuck-state sweeps

```text
expeditions in a stage past timeout -> force resolve, file defect
dives without extraction record -> extract, file defect
crossings past resolve stage -> resolve, file defect
routes infeasible with no reason -> surface reason or fix evaluation
```

Each sweep is a query over saved state, run in the soak and quarterly.

## XI.4 The dead-content sweep

```text
- gates in data never evaluated
- route hazards referenced but never applied
- loot entries that resolve to nothing
- evolution entries with no stage consumer
- hardening upgrades that relax no gate
```

Dead content is a finding per item: restore consumption or retire with a note.

## XI.5 The closure measurement

```yaml
graph_invariants: pass
consumer_consistency: pass (20 routes × 6 consumers)
gate_matrix: pass
session_resume: pass (crossing + dive, all steps)
expedition_closure: pass (stage timeouts)
orphans: 0
evolution_replay: pass
hardening_round_trip: pass
surfaces: pass (reasons, bands, warnings)
w3_06_kits: pass
```

## XI.6 The closing statement

```text
The land outside the door is not decoration; it is where the shelter's future
comes from. It must answer to one law: what the map shows is what travel does.
```

*End of Part XI. Continues in Part XII (appendix: tables).*---

# W4-02 · PART XII — APPENDIX: TABLES AND REGISTERS

## XII.1 The map census register (fill at P0)

| Region | Nodes | Routes | Gates | Traps | Evolving | Static |
|---|---|---|---|---|---|---|
| r_north | __ | __ | __ | __ | __ | __ |
| r_core | __ | __ | __ | __ | __ | __ |
| r_coast | __ | __ | __ | __ | __ | __ |
| r_deep | __ | __ | __ | __ | __ | __ |
| **total** | __ | __ | __ | __ | __ | __ |

## XII.2 The consumer register

| Consumer | Resolves via | Local math found | Owner |
|---|---|---|---|
| map UI | projection read | __ | W3-06 |
| planner UI | projection read | __ | W3-06 |
| caravan AI | projection read | __ | W3-02 |
| patrol AI | projection read | __ | W4-05 |
| expedition system | projection read | __ | W4-02 |
| rail | interlock + projection | __ | W4-02 |
| dive/maritime | tide + projection | __ | W4-02 |

## XII.3 The gate register

| Gate | Route | Seasons | Effect | Warning | Hardening |
|---|---|---|---|---|---|
| g_winter_pass | r_112 | winter | closed | 3d | plow_upgrade |
| g_river_ford | r_087 | spring | delay | 2d | ford_bridge |
| g_coast_storm | r_133 | any | risk_up | 1d | — |
| … | | | | | |

## XII.4 The session register

| Session | Steps | Resumable | Outcome key | Hazard gating |
|---|---|---|---|---|
| crossing.ravine | 6 | yes | cs:<id> | step-gated |
| dive.wreck_04 | 8 | yes | dv:<id> | step-gated |

## XII.5 The knowledge source register

| Source | Tiers granted | Provenance recorded | Notes |
|---|---|---|---|
| survey | Surveyed/Mapped | yes | cartography/expedition |
| map_fragment | Surveyed | yes | item use |
| rumor | Rumored | yes | may be wrong; correctable |
| trade_route | Rumored/Surveyed | yes | caravan contact |
| capture | Surveyed | yes | interrogation of documents |

## XII.6 The failure-copy register

| Ref | Copy |
|---|---|
| travel_route_unknown | "You do not know a way there yet." |
| travel_route_closed | "This way is closed — {reason}." |
| travel_supply_short | "Supplies are short for this journey." |
| travel_dive_window | "The water is wrong for that site right now." |
| travel_expedition_late | "The party is overdue." |

*End of Part XII. Continues in Part XIII (case files).*---

# W4-02 · PART XIII — REVIEWER CASE FILES

## XIII.1 Case 1 — the shortcut pathfinder

**Diff:** adds a fast pathfinder in the caravan system "for performance."

**Review:**

```text
confidence semantics? fast path only checks adjacency, ignoring gates
verdict: RETURNED — read the projection; cache the projection if speed is the
         issue, with invalidation on gate/weather change
```

## XIII.2 Case 2 — the "known" node

**Diff:** new settlement data appears on the map at campaign start.

**Review:**

```text
tier?      data presence ≠ knowledge
verdict:   RETURNED unless the start-of-campaign knowledge set explicitly
           includes it (authored and testable)
```

## XIII.3 Case 3 — the immersive withdrawal

**Diff:** a crossing can end with the party "lost" for variable real-time
minutes.

**Review:**

```text
durable state? real-time waits are not durable and break on reload
verdict:       RETURNED — model as campaign-time delay or an outcome state
```

## XIII.4 Case 4 — the generous loot fallback

**Diff:** unresolved loot falls back to a generic valuable item.

**Review:**

```text
intent?    fallback for content gaps is fine; but it must be authored per case
verdict:   SIGNED with condition — the fallback maps per loot class, is
           recorded, and the orphan is filed as a content defect
```

## XIII.5 Case 5 — the evolution acceleration

**Diff:** evolution ticks on real days ("so the world changes while away").

**Review:**

```text
determinism? wall-clock breaks replay and saves
verdict:     RETURNED — campaign-day ticks only; absence-of-play is not
             simulated unless a signed design says so
```

## XIII.6 The review patterns

| Pattern | Tell | Verdict |
|---|---|---|
| local pathfinder "for speed" | new combinational logic | RETURNED |
| data presence as knowledge | missing gate read | RETURNED |
| real-time waits/costs | not durable | RETURNED |
| content fallbacks | authored per class | SIGNED with filing |
| wall-clock world change | replay breaks | RETURNED |

## XIII.7 The map review card

```text
1. does this change a component of the one answer?
2. does it read a projection or compute?
3. does the map still show only what is known?
4. does any wait/state use real time?
5. do references resolve?
```

*End of Part XIII. Continues in Part XIV (scenarios).*---

# W4-02 · PART XIV — SCENARIO BANK

## XIV.1 S1 — The first survey

```text
fixture: home only; rumor points at n_12
steps: plan to n_12 (rumored → penalty), travel, arrive (tier surveyed),
       field guide records source, map updates
assert: tier change once; provenance recorded; ETA band narrows
```

## XIV.2 S2 — The closed pass

```text
fixture: winter gate on the main route
steps: plan → infeasible with reason; detour offered; plow hardening applied →
       feasible with delay; warning before each attempt
assert: reason present; hardening persists across save
```

## XIV.3 S3 — The ravine crossing

```text
fixture: crossing.ravine with hazards
steps: save at each of 6 steps; load; complete; verify single outcome
assert: steps persist; hazards apply once; aftermath routes
```

## XIV.4 S4 — The wreck dive

```text
fixture: dive.wreck_04, tide window closing in 2 days
steps: enter late (risk), save mid-dive, load, extract, loot resolves
assert: window math stable across load; extraction always possible
```

## XIV.5 S5 — The overdue expedition

```text
fixture: stage with timeout 5 days
steps: block the stage; pass timeout; force-resolve outcome fires
assert: no infinite stage; outcome written; party returns or is lost by rules
```

## XIV.6 S6 — The rail night

```text
fixture: two consists, one junction
steps: interlock sequences (clear/occupied/blocked); signals reflect state
assert: single interlock truth; no surface computes separately
```

## XIV.7 S7 — The decade

```text
fixture: 400 days seeded
steps: watch node stages evolve; landmarks degrade; repairs applied
assert: deterministic replay; no wall-clock; stage consumption everywhere
```

## XIV.8 S8 — The wrong rumor

```text
fixture: node n_21 rumored as "market"; actually ruins
steps: arrive; correction recorded; field guide updates; tier stands surveyed
assert: correction once; no mockery tone; guide wording restrained
```

## XIV.9 S9 — The caught lie

```text
fixture: run the lie audit across all travel surfaces
steps: compare each displayed value with owner state
assert: zero discrepancies
```

## XIV.10 S10 — The clean desk

```text
fixture: full registers; all invariants
assert: graph scans zero; consumer matrix full; no dead content
```

## XIV.11 The soak recipe

```text
60 days: 20 expeditions, 6 crossings, 4 dives, daily patrols, rail twice,
seeded weather with two fronts and one storm
assertions: no stuck states; orphans 0; projections stable; replay green
```

*End of Part XIV. Continues in Part XV (handoffs).*---

# W4-02 · PART XV — GOVERNANCE, HANDOFFS, AND ROLLOUT

## XV.1 Governance

| Concern | Owner |
|---|---|
| graph data | W4-02 package + integrity pipeline |
| knowledge | gate owner |
| resolution | dispatch engine + route family |
| gates | weather gate family |
| sessions | crossing/dive runners |
| evolution | world evolution family |
| registers | integrator (map census, consumers, gates) |

## XV.2 Handoffs

| Direction | Detail |
|---|---|
| W3-02 | caravans read projections; trade routes add gate demands |
| W3-03 | travel trauma/narrative hooks on crossings and losses |
| W3-04 | encounter combat binding; war changes access |
| W4-01 | sessions, evolution, knowledge all follow the save laws |
| W4-03 | hardening, power for observation posts |
| W4-05 | patrol AI, faction territory, patrol routes |
| W3-06 | travel surfaces join the route/focus/lane kits |

## XV.3 The rollout (6 weeks)

```text
w1  P0: census, consumers, gates, orphans, premises
w2  graph invariants + knowledge gate + render
w3  resolution unification + consumer consistency
w4  gate matrix + warnings + hardening persistence
w5  sessions (crossing + dive) + expedition closure + vehicles/rail
w6  evolution + surfaces + soak + closeout
```

## XV.4 Exit criteria per week

| Week | Exit |
|---|---|
| 1 | registers filed; premises verified |
| 2 | invariants green; gate matrix drafted |
| 3 | consumers consistent |
| 4 | gates warn; hardening saved |
| 5 | sessions resume; expeditions close |
| 6 | replay green; kits green; closure measured |

## XV.5 The risk register

| ID | Risk | Mitigation |
|---|---|---|
| R1 | second pathfinder reappears | consumer test weekly |
| R2 | session snapshot save | step grammar + review |
| R3 | surface optimism | lie audit |
| R4 | wall-clock creep | T1.8 scan |
| R5 | dead content accumulates | seasonal sweep |
| R6 | map data drift | invariants per content change |

## XV.6 The stop-the-line list

```text
1. a consumer disagreeing with the projection
2. a session that cannot resume from any step
3. an expedition that cannot close
4. a gate closure without warning
5. an orphan reference on a live path
6. non-campaign time in durable world state
```

*End of Part XV. Continues in Part XVI (closure and final control).*---

# W4-02 · PART XVI — CLOSURE MEASUREMENT AND FINAL CONTROL

## XVI.1 The closure measurement

```yaml
run: W4-02-closure
head: <sha>
graph: { nodes: __, routes: __, orphans: 0, invariants: pass }
knowledge: { writers: __, bypasses: 0, matrix: pass }
resolution: { consumers: 6, divergences: 0 }
gates: { evaluated: __, data_matches: yes, warnings: pass, hysteresis: pass }
sessions: { crossing_steps: pass, dive_steps: pass, outcome_once: pass }
expeditions: { closure: pass, loot_orphans: 0, timeouts: authored }
vehicles_rail: { wear: pass, interlock: pass, windows: pass }
maritime: { windows: pass, extraction: pass }
evolution: { replay: pass, stages: pass, landmarks: pass }
surfaces: { reasons: pass, bands: pass, warnings: pass, kits: pass }
soak: pass
```

## XVI.2 The acceptance table

| Line | Evidence | Signed |
|---|---|---|
| graph invariants | scan outputs | ☐ |
| knowledge gate | matrix + render tests | ☐ |
| consumer consistency | 20×6 matrix | ☐ |
| gate matrix + warnings | scenario S2 | ☐ |
| session resume | S3 + S4 | ☐ |
| expedition closure | S5 | ☐ |
| vehicles/rail | T2.7 kit | ☐ |
| maritime | T2.8 kit | ☐ |
| evolution replay | S7 | ☐ |
| surfaces + kits | lie audit | ☐ |

## XVI.3 The binding summary

```text
Binding: the six graph invariants (§II.1.3), the gate matrix (§II.2.3), the
resolution rules R1–R6 (§II.3.2), the session grammar (§III.2), the expedition
binding rules E1–E6 (§III.3.2), the evolution rules L1–L6 (§IV.3.2), and the
stop-the-line list (§XV.6).
```

## XVI.4 The final declaration

**W4-02 is complete as a plan.** Parts I–XVI + findings WX-01…WX-20. Proposal
only; execution requires Annex U (Part I §U.2) and §IV.7 signatures (per Part I
plan-unblocking). It hands to the wave: one graph law, one knowledge gate, one
resolution authority, resumable sessions, closable expeditions, and a land that
changes deterministically.

```text
The map is a promise: what it shows, travel does — and what it hides, the
shelter has not yet earned the right to know.
```

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-02 (expansion continues as needed for depth).*---

# W4-02 · PART XVII — Q&A, SECOND BAND (Q36–Q80)

**Q36. What is the map's unit of truth?**
The route projection: one object answering feasibility, distance, time, risk.

**Q37. What is the knowledge gate's unit of truth?**
The tier + provenance row per node.

**Q38. What makes a rumor "fair"?**
It is possible, corrigible, and recorded — and it never comes from a survey.

**Q39. Should unknown nodes render at all?**
As terrain. The map is the shelter's mental model, not the world's truth.

**Q40. How do gates relate to seasons?**
Each gate names its season and condition; seasons come from the calendar; the
evaluation is one function.

**Q41. What stops gate flicker at the boundary?**
Hysteresis on reopen (W6) — a declared margin, not a coin toss.

**Q42. Who decides what a gate does: closed, delay, risk, or detour?**
The authored gate record; the evaluator only evaluates.

**Q43. Can a gate be permanently hardened away?**
Some can (authored), with cost and persistence; others only relax. The record
says which; the test proves it.

**Q44. What is the difference between a crossing and an encounter?**
A crossing is a route event with arbitration stages; an encounter is a meeting
resolved by narrative or combat. Crossings can host encounters at steps.

**Q45. What is the smallest resumable unit?**
One step. If a session cannot name steps, it cannot be saved mid-run.

**Q46. What happens if a player closes the game mid-crossing?**
The save (autosave/exit) stores the step; the next load resumes there or at the
last completed step with a notice.

**Q47. Can a crossing be failed on purpose?**
Yes — flee/abort are outcomes with aftermath. Failure is content, not a crash.

**Q48. How do expeditions avoid double-loot?**
Resolved flags per loot row plus one outcome key per expedition stage.

**Q49. What guarantees an expedition ends?**
Stage timeouts with authored outcomes; stuck aggregates fail the soak.

**Q50. Who owns expedition encounters' consequences?**
The bridge binds; W3-01/W3-03/W3-04 own their domains; the aggregate records.

**Q51. How do vehicles enter an expedition?**
As a declared asset; conditions and fuel read from the vehicle owner; no copies.

**Q52. What is a vehicle's minimal durable shape?**
Condition, fuel, modules, home, state — all bounded, all saved (W4-01).

**Q53. What does rail add beyond roads?**
Scheduled capacity, interlock safety, and freight economics — consumed by
W3-02, never simulated twice.

**Q54. Why do dives have a different slice of the tide?**
Because water is time-sensitive in a way roads are not; the tide calendar is the
authority and the window is a gate.

**Q55. Can a dive be re-entered after extraction?**
Per-site authored rules; sites can deplete and recover on deterministic timers.

**Q56. What is location evolution's granularity?**
Authored stages per node kind, a daily tick, bounded modifiers, and landmark
condition.

**Q57. What makes evolution "not magic"?**
Drivers: trade visits, raids, weather, decay rates — named in the record and
visible in the field guide.

**Q58. Can evolution regress?**
Yes — ruin, abandonment, and stripping are authored stages. The world is not a
one-way ratchet.

**Q59. What does the field guide store?**
Facts of discovery and correction with provenance, in restrained prose refs —
bounded, durable, part of the save.

**Q60. Who writes field-guide prose?**
The corpus process (W2-06 routing); the model stores ids and facts only.

**Q61. What is the lie audit's cadence?**
Per release; spot checks weekly; a lie is a stop-the-line item.

**Q62. How do we test "the map doesn't know too much"?**
Render tests against fixture tiers: unknown nodes show terrain only; rumored
show markers; mapped show details.

**Q63. How do we test "the map doesn't know too little"?**
Surveys and arrivals must upgrade tiers and persist; a lost upgrade is a defect.

**Q64. What is the map census for?**
Keeping the registers honest as content grows; numbers that drift silently are
how maps rot.

**Q65. How do new regions attach safely?**
Authored nodes/routes, invariant checks, Unknown defaults, evolution declared
or static, save-compatible ids.

**Q66. What happens to a route when a bridge collapses?**
Infrastructure state changes; traversability derives; consumers see it
immediately; repair is a craft path (W3-05).

**Q67. How do patrol AI and knowledge interact?**
Patrols ignore knowledge (their land) but obey physical gates. Tested.

**Q68. Can caravans exploit obscure routes?**
They use the same projections; their route choices may weight risk differently
by declared policy, not by private math.

**Q69. What is the plan's biggest simplicity win?**
One resolution authority. It deletes whole bug classes by construction.

**Q70. What is the biggest depth win?**
Sessions that resume from steps: crossings and dives stop being fragile.

**Q71. What does Path A skip?**
Depth: evolution, hardening, maritime arcs. It fixes truth and agreement first.

**Q72. What does Path B skip?**
Long arcs: eras, reclaimed rail, site populations. It makes the present calm.

**Q73. What does Path C add that nothing else does?**
Time. The land becomes a participant in the campaign's history.

**Q74. What is out of scope, definitively?**
New region content authoring, procedural generation, combat systems, economy
balance, UI layout (W3-06 owns surfaces' design).

**Q75. What is the acceptance shape, one line?**
Every travel number comes from one authority, every state resumes, and the map
is exactly as wise as the shelter.

**Q76. What is the smallest test that proves the plan?**
Feed six consumers the same route: they agree.

**Q77. What is the sentence for reviewers?**
"Show me the projection this number came from."

**Q78. What is the sentence for authors?**
"If the map can do it, a gate must say why not — with a warning."

**Q79. What is the sentence for players?**
"The map shows what you know. The road does what the map shows."

**Q80. The last word?**
The land is a promise; travel keeps it.

*End of Part XVII. Continues in Part XVIII (threads, second band).*---

# W4-02 · PART XVIII — WORKED THREADS, SECOND BAND (WX-21–WX-32)

## XVIII.1 Thread H — "the detour that looped"

**Report:** a gate's detour route pointed at the gated route itself; planning
looped until the UI froze.

**Walk:**

```text
1. root: detour reference validated for existence, not for acyclicity
2. repair: detour resolution is a bounded walk (visited set, max depth) and the
   graph invariant G7 forbids detour cycles
3. verify: cycle fixture; bounded-walk test
```

| ID | Class | Repair |
|---|---|---|
| WX-21 | detour cycle | invariant + bounded resolution |
| WX-22 | no cycle test | coverage |

## XVIII.2 Thread I — "the scout who was never believed"

**Report:** expeditions returned with discoveries that never upgraded the map.

**Walk:**

```text
1. root: discovery consequences fired but the knowledge upgrade path was only
   wired for the survey action, not for expedition discoveries
2. repair: all discovery sources route through the gate's upgrade API; source
   recorded; test per source
3. verify: five-source upgrade matrix
```

| ID | Class | Repair |
|---|---|---|
| WX-23 | knowledge upgrade path missed a source | completeness |
| WX-24 | no per-source upgrade test | coverage |

## XVIII.3 Thread J — "the fuel that evaporated"

**Report:** vehicles arrived with less fuel than the projection predicted.

**Walk:**

```text
1. root: two consumption models: projection used route length; the vehicle
   applied per-event consumption including idle at stops
2. repair: consumption is a projection component (declared event costs); the
   vehicle applies the projection's total; no second model
3. verify: fuel reconciliation test (predicted vs applied)
```

| ID | Class | Repair |
|---|---|---|
| WX-25 | duplicated consumption model | duplication |
| WX-26 | no reconciliation test | coverage |

## XVIII.4 Thread K — "the storm that arrived twice"

**Report:** a weather front closed a gate, reopened it, and closed it again
within one day.

**Walk:**

```text
1. root: two fronts authored back-to-back with no hysteresis and identical
   conditions
2. repair: hysteresis margin + front coalescing rule; authored front spacing
3. verify: flicker test over a simulated week
```

| ID | Class | Repair |
|---|---|---|
| WX-27 | gate flicker | hysteresis + authoring rule |
| WX-28 | no flicker test | coverage |

## XVIII.5 Thread L — "the waystation with no water"

**Report:** a route's waystation service list claimed water; the stop restored
nothing.

**Walk:**

```text
1. root: service list was descriptive copy; no consumer existed
2. repair: waystation services are implemented effects (rest, water, repair) or
   the list is corrected; no descriptive-only claims in data
3. verify: service consumption test per waystation
```

| ID | Class | Repair |
|---|---|---|
| WX-29 | descriptive-only service claims | truth |
| WX-30 | no service consumption test | coverage |

## XVIII.6 Thread M — "the expedition that remembered a dead survivor"

**Report:** a returning party included a survivor who died en route.

**Walk:**

```text
1. root: party list persisted ids; death removed from the roster but the
   aggregate kept the id and the debrief re-added a row
2. repair: aggregate validates party against the survivor owner at each stage;
   missing ids resolve as losses with authored outcomes
3. verify: mid-expedition death fixture; debrief reconciliation
```

| ID | Class | Repair |
|---|---|---|
| WX-31 | stale party references | validation |
| WX-32 | no mid-mission death test | coverage |

## XVIII.7 The summary

```text
H: detours are bounded and acyclic
I: every discovery source upgrades knowledge through one door
J: consumption lives in the projection
K: weather coalesces and gates never flicker
L: services in data are effects, not captions
M: party state is validated against its owner, always
```

*End of Part XVIII. Continues in Part XIX (closing programs).*---

# W4-02 · PART XIX — CLOSING PROGRAMS AND YEAR ONE

## XIX.1 The standing commitments

```text
C1  graph invariants run on every map data change
C2  consumer consistency spot-check weekly; full matrix per release
C3  gate matrix re-run per season boundary in the soak
C4  session resume kit per release; steps re-verified
C5  orphan scan (loot/encounter/gate/infrastructure) per release
C6  evolution replay quarterly; field-guide review seasonal
C7  lie audit per release across travel surfaces
C8  map census yearly
```

## XIX.2 The year plan

```text
Q1  census refresh; invariants; consumer matrix full
Q2  gate/hardening audit; season boundary runs; dead-content sweep
Q3  evolution decade run; maritime arcs review; rail reclaim check
Q4  lie audit deep; field-guide corpus review; next-year map growth plan
```

## XIX.3 The health signals

```text
- six consumers, one number, every route
- nobody says "the map is wrong" — they say "the gate is closed"
- sessions survive being closed mid-step
- every expedition has an end
- the guide reads like a journal, not a schema
- seasons feel like weather, not like bugs
```

## XIX.4 The rot signals

```text
- a new "quick" path in AI or UI
- a route number that differs between two screens
- an expedition sitting in a stage for days
- a gate closing with no warning
- evolution that repeats after reload
- descriptive-only data claims
```

Three rot signals together: run weeks 1–3 of the rollout as a refresher.

## XIX.5 The annual retrospective agenda

```text
1. graph census vs registers: drift report
2. consumer divergences: classes and repairs
3. stuck-state sweeps: counts and causes
4. gates: closures, warnings, complaints ("unfair" is data)
5. knowledge: sources actually used; wrong rumors that mattered
6. evolution: eras entered; landmarks lost and repaired
7. field guide: entry quality; corrections celebrated
8. next year: one map-growth decision, one depth decision
```

## XIX.6 The long view

```text
A road plan is successful when the road disappears from the conversation:
players talk about where they went and who they lost, not about numbers that
did not add up. That is the end state this plan protects.
```

*End of Part XIX. Continues in Part XX (extended registers).*---

# W4-02 · PART XX — EXTENDED REGISTERS AND TABLES

## XX.1 The route register (sample rows)

| Route | From | To | Kind | km | Gates | Hazards | Infrastructure |
|---|---|---|---|---|---|---|---|
| r_112 | n_07 | n_12 | road | 14 | g_winter_pass | trap_rubble | bridge_2 |
| r_087 | n_12 | n_18 | road | 9 | g_river_ford | — | ford_1 |
| r_133 | n_18 | n_31 | water | 22 | g_coast_storm | shoals | — |
| r_140 | n_31 | n_44 | rail | 40 | — | — | rail_seg_3 |
| … | | | | | | | |

## XX.2 The hazard register

| Hazard | Route(s) | Effect | Step | Warning |
|---|---|---|---|---|
| trap_rubble | r_112 | delay/risk | crossing step 2 | road condition |
| shoals | r_133 | damage risk | dive entry | tide note |
| snag_drift | r_140 | delay | en route | patrol note |

## XX.3 The vehicle module register

| Module | Class | Effect | Consumed by | Dead? |
|---|---|---|---|---|
| armor_1 | truck | risk reduction | resolution | no |
| winch | any | recovery outcome | crossing | no |
| plow_upgrade | any | gate relaxation | gate matrix | no |
| … | | | | |

## XX.4 The session step table (crossing.ravine)

| Step | Decision | Hazard | Persisted | Resume-safe |
|---|---|---|---|---|
| 1 approach | press on / turn back | — | yes | yes |
| 2 rubble | clear / climb | trap_rubble | yes | yes |
| 3 nightfall | wait / push | fatigue | yes | yes |
| 4 far side | — | — | yes | yes |

## XX.5 The evolution stage table

| Kind | Stages | Drivers | Landmark |
|---|---|---|---|
| settlement | camp → stead → town | trade visits, population | walls, well |
| ruins | intact → picked → stripped | expeditions | tower, vault |
| outpost | manned → empty → reclaimed | war, patrols | watchtower |

## XX.6 The warning copy register

| Ref | Copy |
|---|---|
| gate_warn_close | "{gate} closes when {condition} — {days} ahead." |
| gate_warn_risk | "Passing now carries {risk}." |
| expedition_overdue | "The party is overdue by {days}." |
| dive_window_edge | "The tide turns soon; this is a narrow window." |

*End of Part XX. Continues in Part XXI (Q&A, third band).*---

# W4-02 · PART XXI — Q&A, THIRD BAND (Q81–Q120)

**Q81. What does "one road" mean to a player?**
The number they read is the number that happens — everywhere, always.

**Q82. What is the single most testable claim of this plan?**
Six consumers, one projection, zero divergences.

**Q83. What if the projection is wrong (a bug)?**
Then the map is consistently wrong until fixed — and that is strictly better
than inconsistently wrong. Fix once, everywhere.

**Q84. Why does the plan forbid AI shortcuts?**
Because exceptions become the norm. One road includes the minds on it.

**Q85. Do rumors need to be rare?**
They need to be authored. Frequency is design; mechanism is this plan.

**Q86. What is the worst possible map bug?**
A route that lies — shows open, behaves closed, or vice versa. It is a
stop-the-line item.

**Q87. How do we test that a route "behaves"?**
Execute the journey in the soak; compare each stage's observed values to the
projection. Divergence is a defect.

**Q88. What is a session's durable minimum?**
Instance id, step, inputs, rng cursor, outcome key. Five fields, no snapshots.

**Q89. Can a session be abandoned?**
Yes: abandonment is an authored outcome with aftermath. The state never
lingers as "open."

**Q90. What does "extraction always reachable" mean for dives?**
At every step, an authored path to surface exists; the cost may be severe, but
no state traps the player.

**Q91. How do expeditions handle a wiped party?**
As an authored loss outcome with return-of-record (what was found, where it
stopped). The world learns; the guide writes it.

**Q92. What if loot exceeds carry capacity?**
Projection accounts for capacity; overflow prompts a choice (cache, discard,
return later). Authored, warned, not silent.

**Q93. Is "cache it for later" a new store?**
No: caches are expedition-scoped records that decay per authored rules and are
consumed by later expeditions — routed through the same aggregate owner.

**Q94. How do vehicles age?**
Per km and per event, deterministic, repaired by existing crafts. A vehicle is
a tool with a life, not a consumable.

**Q95. What does rail give the campaign?**
Capacity and schedule: freight that moves without an expedition, which the
economy plan consumes.

**Q96. Why is interlock a single truth?**
Because collisions are authored drama, not emergent bugs. One authority, one
signal set.

**Q97. What does an evolution era change mechanically?**
Stage-gated services, encounter pools, landmark condition, and travel risk —
all consumed, all saved.

**Q98. Who decides when a site evolves?**
The authored drivers in the record, evaluated daily. No ambient randomness.

**Q99. Can the player accelerate evolution?**
Through actions (trade, repair, defense) that the drivers read. The plan makes
drivers explicit so design can tune them.

**Q100. What is the field guide's rule against spoilers?**
It records what happened and what was learned, not what waits. Discovery is
play; the guide is memory.

**Q101. How do corrections work in tone?**
Quietly: "The reports were wrong. It is a ruin." No mockery, no drama.

**Q102. What is the field guide's bound?**
Bounded per region with a summary roll-up; entries cite events; no free-text
growth beyond authored refs.

**Q103. What is a "reason" in surface terms?**
A short authored sentence naming the obstacle: unknown way, closed pass, storm,
tide, supplies. Never a bare red X.

**Q104. How are reasons localized later?**
They are refs into the corpus (D22/W2-06); the model carries refs only.

**Q105. Do travel surfaces show risk as numbers?**
Bands with words; numbers only where knowledge is Mapped and the design says so.

**Q106. What about accessibility of the map?**
W3-06 owns floors; this plan supplies tier/reason text so the map is never
color-only.

**Q107. What is the resume grammar's failure mode?**
A step that cannot resume is a design error; the fallback is the previous step
with a notice, never a new random state.

**Q108. How do we prevent duplicated crossings after two loads?**
Outcome keyed once; step index gates all effects; the test loads twice.

**Q109. How does war change travel?**
Through gates/traversability and encounter pools on the same graph — W3-04
owns the war, W4-02 owns the road it changes.

**Q110. How does trade change travel?**
Through caravan schedules consuming projections; new routes can be opened by
authored route openings (bridge repair, rail reclaim).

**Q111. What is the smallest regression suite for the plan?**
Consumer consistency (5 routes) + session resume (1 crossing) + gate warning
(1 gate) + orphan scan. Minutes, catches most drift.

**Q112. What is the biggest test debt at closure?**
Wrong-rumor variety and evolution era breadth; both need content breadth to
test well — flags for the content wave.

**Q113. What does Path A leave broken?**
Depth semantics — but never truth. Truth first is the plan's identity.

**Q114. What does Path B leave for later?**
The years: eras, reclaim, depletion. The present is calm first.

**Q115. What does Path C risk?**
Scope: eras are content-heavy. Bound to authored stages; no emergent fantasy.

**Q116. What guarantees the plan outlives its authors?**
Registers + gates + calendar. The map census is the memory; the matrix is the
law.

**Q117. What is the one rule for new contributors?**
Find the projection; never compute your own.

**Q118. What is the one rule for content authors?**
If it is on the map, a gate must be able to close it — with a warning.

**Q119. What is the one rule for reviewers?**
Ask for the projection, the step table, and the reason string. Three artifacts
prove almost everything.

**Q120. The final sentence?**
One road, honestly drawn; every journey, honestly run.

*End of Part XXI. Continues in Part XXII (threads, third band).*---

# W4-02 · PART XXII — WORKED THREADS, THIRD BAND (WX-33–WX-44)

## XXII.1 Thread N — "the map that learned from a dream"

**Report:** a node upgraded to Surveyed after a night event that never involved
travel.

**Walk:**

```text
1. root: a narrative event granted knowledge via a "map feels familiar" line
   with no source recorded
2. repair: every upgrade requires a source; narrative events may grant
   Rumored only unless they model a document/survey; source recorded
3. verify: source-required test; narrative event audit
```

| ID | Class | Repair |
|---|---|---|
| WX-33 | unsourced tier upgrade | truth | source required |
| WX-34 | no narrative-source audit | coverage | add kit |

## XXII.2 Thread O — "the bridge that repaired itself"

**Report:** after a collapse, a reload restored the bridge.

**Walk:**

```text
1. root: infrastructure damage lived in a transient event record, not the
   durable infrastructure state
2. repair: infrastructure condition is durable, owned, and read by route
   resolution; events write it, not their own copies
3. verify: collapse → save → load → still collapsed
```

| ID | Class | Repair |
|---|---|---|
| WX-35 | transient infrastructure damage | durability |
| WX-36 | no reload-continuity test | coverage |

## XXII.3 Thread P — "the patrol that was everywhere"

**Report:** the same patrol appeared on two routes in one day.

**Walk:**

```text
1. root: two patrol instances from a faction system restart; no unique guard
2. repair: patrol instances keyed; the faction owner enforces uniqueness; the
   map reads live patrol positions
3. verify: duplicate-instance scan
```

| ID | Class | Repair |
|---|---|---|
| WX-37 | duplicate AI instances | integrity |
| WX-38 | no instance registry | coverage |

## XXII.4 Thread Q — "the expedition roster that kept growing"

**Report:** after several returns, the expedition list held dozens of "closed"
records.

**Walk:**

```text
1. root: closed aggregates retained forever; the list had no retention
2. repair: closed aggregates roll into a bounded history with summaries
   (findings, losses, knowledge) and the detail drops away
3. verify: retention bound; summary completeness
```

| ID | Class | Repair |
|---|---|---|
| WX-39 | unbounded expedition history | growth |
| WX-40 | no summary artifact | completeness |

## XXII.5 Thread R — "the gate that only opened for the player"

**Report:** NPC caravans passed a gated road freely; the player could not.

**Walk:**

```text
1. root: caravans used knowledge-blind evaluation but skipped the gate check
   entirely ("schedules"), a silent exemption
2. repair: exemptions are authored and surfaced ("smugglers' route" gate kind)
   rather than accidental; caravans obey gates by default
3. verify: AI-gate matrix; exemption register
```

| ID | Class | Repair |
|---|---|---|
| WX-41 | silent AI gate exemption | fairness |
| WX-42 | no exemption register | coverage |

## XXII.6 Thread S — "the tide that drifted"

**Report:** dive windows slowly drifted as campaign days advanced beyond
fixture expectations.

**Walk:**

```text
1. root: the tide calendar used a fixed 30-day month assumption; the campaign
   calendar months vary
2. repair: tide math reads the campaign calendar owner; no assumptions
3. verify: long-run window test at day 400
```

| ID | Class | Repair |
|---|---|---|
| WX-43 | duplicated calendar assumptions | duplication |
| WX-44 | no long-run window test | coverage |

## XXII.7 The summary

```text
N: knowledge grows only from real sources
O: infrastructure damage is durable
P: AI instances are unique and owned
Q: closed missions summarize and bound
R: exemptions are authored and visible
S: time math reads the one calendar
```

*End of Part XXII. Continues in Part XXIII (scenario bank 2).*---

# W4-02 · PART XXIII — SCENARIO BANK 2 (S11–S20)

## XXIII.1 S11 — The dead drop

```text
fixture: cache left by expedition A; time passes; expedition B finds it
assert: cache decay per authored rules; B consumes once; A's summary records
```

## XXIII.2 S12 — The war road

```text
fixture: war stage closes a region (W3-04)
assert: routes through it become infeasible with reason; patrols change; the
        economy reroutes (W3-02); nothing recomputes war locally
```

## XXIII.3 S13 — The night crossing

```text
fixture: crossing at night adds risk (authored)
assert: risk enters projection; warning before commit; session steps include
        the night decision; resume-safe
```

## XXIII.4 S14 — The overloaded crawler

```text
fixture: module set exceeds a route's limits (authored)
assert: route infeasible with weight reason; alternate route offered
```

## XXIII.5 S15 — The reclamation

```text
fixture: rail segment reclaimed after weeks
assert: route becomes available; freight capacity changes; W3-02 sees it; the
        event is recorded in the guide
```

## XXIII.6 S16 — The honest storm

```text
fixture: storm front with gates across three routes
assert: each gate warns; infeasible reasons correct; forecast bands honest;
        after the front, hysteresis reopens gradually
```

## XXIII.7 S17 — The artifact survey

```text
fixture: map fragment item grants Surveyed tier to two nodes
assert: source recorded (map_fragment); one-use semantics; guide entry
```

## XXIII.8 S18 — The quiet decade

```text
fixture: 400 days with no player travel
assert: evolution continues per drivers; no wall-clock; replay equality; the
        map does not narrate events that never happened
```

## XXIII.9 S19 — The duplicated save resume

```text
fixture: save mid-expedition, load twice from the same file
assert: second load equals first; no doubled encounters/loot/consequences
```

## XXIII.10 S20 — The clean desk

```text
fixture: full registers green
assert: no dead gates/hazards/modules; consumer matrix full; no stuck states
```

## XXIII.11 The soak matrix

| Scenario | Cadence |
|---|---|
| S1, S2, S4, S9 | every release |
| S3, S12, S16, S19 | per release (session/war/weather changes) |
| S5–S8, S10, S11, S13–S15, S17–S18 | quarterly rotation |
| S20 | per release |

*End of Part XXIII. Continues in Part XXIV (operations manual).*---

# W4-02 · PART XXIV — OPERATIONS MANUAL

## XXIV.1 Roles

| Role | Responsibility |
|---|---|
| map author | nodes/routes/gates/hazards data integrity |
| session owner | step tables, resume grammar, outcome keys |
| resolution owner | projection rules and factor tables |
| evolution owner | stages, drivers, landmark rates |
| integrator | registers, matrix, soak, calendar |
| reviewer | projection/step/reason questions |

## XXIV.2 The daily rhythm

```text
morning: invariants + orphan scan on the current tree
midday:  content work with registers updated in the same change
evening: matrix spot (five routes); stuck-state query
```

## XXIV.3 The weekly rhythm

```text
- consumer matrix spot; divergences filed same day
- stuck-state sweep (expeditions/crossings/dives)
- gate honesty spot: one closure, verify warning + reason
- field-guide entry sample: tone + provenance
```

## XXIV.4 The release rhythm

```text
T-7: full matrix; resume kit; orphan scan zero
T-3: gate matrix; evolution replay; S4/S9/S19
T-1: lie audit; registers current; copy review
T-0: closure lines signed
```

## XXIV.5 Escalation

| Signal | Class | Action |
|---|---|---|
| two consumers disagree | truth | same day |
| session cannot resume | design | halt feature; fix step table |
| expedition stuck | integrity | force resolve + defect |
| gate closure unwarned | fairness | same day |
| evolution non-deterministic | determinism | replay bisect |
| orphan on live path | completeness | fix reference or retire |

## XXIV.6 Dependency map

```text
graph data -> loader -> knowledge gate -> resolution -> consumers
weather/season -> gates -> resolution
sessions (crossing/dive) -> resolution + step tables
expeditions -> resolution + sessions + combat bridge + loot validator
vehicles/rail -> resolution + interlock
evolution -> resolution + surfaces + guide
```

## XXIV.7 The dependency laws

```text
1. resolution depends on graph + gates + factors; nothing depends on consumers
2. consumers depend on resolution only
3. sessions depend on resolution and their own step tables
4. evolution depends on campaign clock and drivers; surfaces depend on evolution
5. the guide depends on events; nothing depends on the guide
```

*End of Part XXIV. Continues in Part XXV (evidence templates).*---

# W4-02 · PART XXV — EVIDENCE TEMPLATES AND READER'S MAP

## XXV.1 The consumer evidence file

```yaml
run: CC-<date>
routes: 20
consumers: [map, planner, caravan, patrol, rail, dive]
divergences:
  - route: r_112  a: map  b: planner  field: hours
result: pass | fail
```

## XXV.2 The session evidence file

```yaml
run: SR-<session>
steps: 6  saved_at: [1,2,3,4,5,6]
resumes_ok: 6/6
hazards_applied_once: yes
outcome_keys_unique: yes
abort_path: pass
```

## XXV.3 The gate evidence file

```yaml
run: GM-<date>
gates: __
warning_before_close: pass
hysteresis: pass
hardening_persistence: pass
forecast_bands: pass
```

## XXV.4 The evolution evidence file

```yaml
run: EV-<date>
seed: ____
a: evolve 10 -> save -> load -> evolve 10 -> digest H1
b: evolve 20 straight -> digest H2
equal: yes
stages_entered: [camp->stead, ruins->picked]
landmarks: {tower: 88->62, repaired_to: 71}
```

## XXV.5 The orphan scan file

```yaml
run: OR-<date>
loot_refs: __  unresolved: 0
encounter_refs: __  unresolved: 0
gate_refs: __  unresolved: 0
infra_refs: __  unresolved: 0
```

## XXV.6 The reader's map

```text
5 minutes:   §0, §II.3.2 (rules R1–R6), §XVI.4 (final declaration)
builder:     §1, Parts II–IV, VI, X, XIV
reviewer:    Parts XIII, XXIV, §XV.6
author:      Parts V, X, XII, XX
support:     §XII.6 (copy), §XV.5 (risks), Part XXIII
foreman:     Annex U, §6, Part XV, §XVI.2
```

## XXV.7 The artifact index

```text
registers: map census · consumers · gates · sessions · knowledge sources
kits:      consumer matrix · session resume · gate matrix · evolution replay ·
           orphan scan · lie audit
data:      wasteland_map_v1.json · weather gate files · evolution records
```

*End of Part XXV. Continues in Part XXVI (program year and closure).*---

# W4-02 · PART XXVI — THE MAP CORPUS AND CONTENT PIPELINE

## XXVI.1 The content pipeline

```text
idea -> node/route/gate record -> invariants -> knowledge default -> evolution
     -> copy refs -> consumer test -> release
```

Every map addition is a data change with four required fields: identity,
connectivity, knowledge default, and consumption proof.

## XXVI.2 The naming conventions

```text
nodes:   n_<zero-padded>      name_ref: loc_<id>
routes:  r_<zero-padded>      gates: g_<slug>
traps:   trap_<slug>          hazards: hz_<slug>
regions: rgn_<slug>           sessions: crossing.<slug> / dive.<slug>
```

Stable ids never change; renames ship as data renames with save mappings
(W4-01).

## XXVI.3 The place-description standard

```text
each node's copy (via W2-06 corpus) answers: what is here, who comes, what it
costs, what it remembers — in restrained, human, fictional language
no real geography, brands, or copied text
tone: saw the world end, not excited about it
```

## XXVI.4 The route flavor standard

```text
roads: what the asphalt remembers; who maintains it; what broke
water: tides, wrecks, what the water took
rail: schedule ghosts; the switches that still work
```

## XXVI.5 The gate flavor standard

```text
a gate is a fact of weather and ground: snow depth, ford height, storm line,
ice. The copy names the fact, not the mechanic.
```

## XXVI.6 The evolution prose standard

```text
stages change what the place IS, never what it never was
the guide records transitions in past tense, briefly
no omniscient narrator; the shelter's voice only
```

## XXVI.7 The content review loop

```text
1. data lands with invariants green
2. copy lands with refs (no inline strings)
3. a human reads three entries aloud for tone
4. the guide and map show the same facts, differently told
```

*End of Part XXVI. Continues in Part XXVII (advanced topics).*---

# W4-02 · PART XXVII — ADVANCED TOPICS

## XXVII.1 Performance

```text
projections: memoized per (from,to,day,gate-state-version); invalidated on
  gate/weather/infrastructure change; no per-frame resolution
knowledge: node rows are small; render culls by tier and viewport
evolution: one daily tick over nodes with drivers; O(nodes), trivial
sessions: small records; resume parse is bounded
```

## XXVII.2 Future regions

```text
attach by authored expansion: new nodes/routes with ids that do not shift
existing ones; knowledge defaults Unknown; evolution declared or static;
save compatibility guaranteed by stable ids and additive data
```

## XXVII.3 Forward-compatible data

```text
new fields on nodes/routes are additive with authored defaults
renames: mapping in loader, migration note (W4-01)
removals: retire with period of grace; saves referencing removed ids resolve
  to authored fallback nodes or are repaired by migration
```

## XXVII.4 Mods (future)

```text
map mods register nodes/routes through the same loader contract; they pass the
same invariants; their knowledge defaults are Unknown; they cannot bypass the
resolution authority
```

## XXVII.5 The hazards of seasons

```text
season boundaries are campaign events; gates re-evaluate at boundaries and on
weather changes; the soak runs one full year to exercise all four transitions
```

## XXVII.6 The danger of convenience

```text
every "just this once" path around resolution is a future divergence report.
the plan names the temptation so it can be refused cheaply.
```

## XXVII.7 What the plan deliberately does not do

```text
- no procedural generation
- no second economy or combat model
- no real-time simulation of absent play
- no cloud/shared maps
```

*End of Part XXVII. Continues in Part XXVIII (worked projections appendix).*---

# W4-02 · PART XXVIII — WORKED PROJECTIONS APPENDIX

> Concrete projection examples so implementers and reviewers share a mental
> model. Field names illustrative; final names at P0.

## XXVIII.1 A simple road

```text
input:  r_112, truck ready, winter beginning, gate open, knowledge Mapped
output: feasible, 14 km, 6 h, risk moderate, gates [g_winter_pass: open]
```

## XXVIII.2 The same road, one day later

```text
input:  gate delay(12h), same vehicle
output: feasible, 14 km, 6 h travel + 12 h wait, risk moderate,
        gates [g_winter_pass: delay 12h], reason: null
```

## XXVIII.3 The same road, hardened

```text
input:  plow_upgrade active
output: feasible, 14 km, 6 h + 4 h, gates [g_winter_pass: delay 4h]
```

## XXVIII.4 The closed road

```text
input:  gate closed; no detour
output: infeasible, reason: gate_closed (g_winter_pass), detour: null
```

## XXVIII.5 The detour

```text
input:  g_winter_pass closed; r_118 exists (foot)
output: feasible via detour, 21 km, 9 h, risk elevated, reason: null,
        detour: [r_118]
```

## XXVIII.6 The unknown road

```text
input:  node rumored; route exists in data
output: feasible (rumored), distance estimate ±40%, risk elevated,
        reason: null, knowledge_note: "reports suggest"
```

## XXVIII.7 The dive window

```text
input:  dive.wreck_04, tide window opens in 1.5 days
output: infeasible now, reason: tide_window, next: day+2 window 6h
```

## XXVIII.8 The overloaded crawler

```text
input:  modules exceed r_087 limits (authored)
output: infeasible, reason: weight_limit, suggestion: remove module or
        alternate route r_090
```

## XXVIII.9 The rail consist

```text
input:  segment rail_seg_3, junction j_2, interlock occupied
output: infeasible, reason: interlock_occupied, next window: 4h
```

## XXVIII.10 The consistency statement

```text
every consumer asked these questions returns these answers — the same object,
formatted differently. That is the whole promise.
```

*End of Part XXVIII. Continues in Part XXIX (Q&A, fourth band).*---

# W4-02 · PART XXIX — Q&A, FOURTH BAND (Q121–Q150)

**Q121. What does the plan hand to the content wave?**
A graph that verifies itself and a knowledge system that makes discovery
mechanical.

**Q122. What is the most underrated part?**
The registers. They turn "the map" from a file into a managed asset.

**Q123. What is the most overrated temptation?**
"Performance" shortcuts. Cache projections, never fork them.

**Q124. What would break the plan fastest?**
A second pathfinder shipped "for AI."

**Q125. Second fastest?**
Sessions saved as snapshots instead of steps.

**Q126. Third?**
Surface values computed for convenience.

**Q127. How do we detect all three?**
Consumer matrix, resume kit, lie audit. Three kits, three temptations.

**Q128. What does the wave owe this plan's survivors?**
That a journey can be lost but never vanish; that a road can close but never
lie.

**Q129. What does the plan owe the narrative wave?**
Hooks: arrival, delay, crossing aftermath, discovery corrections.

**Q130. What does it owe the economy wave?**
Projections and gate states as inputs to caravan planning.

**Q131. What does it owe the security wave?**
A graph whose closures and patrols are real, so war changes roads honestly.

**Q132. What does it owe the save plan?**
Five-line compliance: owner, section, tests, bounds, migration.

**Q133. What does it owe the UI wave?**
Reasons, bands, and provenance so surfaces never invent.

**Q134. What does it owe the player?**
The feeling that the wasteland is large, known only in parts, and honest.

**Q135. Can the map be wrong on purpose?**
Rumors can. The map must show them as rumors.

**Q136. Can a route be profitable to hide?**
Authored (secret paths, smuggler routes) with declared visibility rules —
never accidental.

**Q137. What is the smuggling design constraint?**
Exemptions are authored gate kinds with consumers; never silent skips.

**Q138. How does the guide mark a wrong rumor?**
"It was not as the reports said." One line, past tense, no drama.

**Q139. What if the player never travels?**
Evolution continues by drivers; the world does not require attendance. But
writes only what happened.

**Q140. What is the plan's stance on map fog of war?**
Tier-based, not pixel-based. Knowledge is prose and possibility, not fog.

**Q141. What makes an ETA fair?**
Bands by knowledge, warnings by gate, reasons by failure. Never a single lie.

**Q142. What makes a session fair?**
Every step visible, every hazard warned, every outcome once, every exit real.

**Q143. What makes evolution fair?**
Drivers named, stages authored, records durable, prose restrained.

**Q144. What is the acceptance sentence?**
"Show me the projection; show me the step table; show me the reason string."

**Q145. What is the maintenance sentence?**
"The registers are current, or the build is not."

**Q146. What is the annual sentence?**
"Count the nodes; count the consumers; count the orphans; report the zeros."

**Q147. What is the closing sentence?**
"One road, honestly drawn; every journey, honestly run."

**Q148. What remains to be done after closure?**
The calendar, the kits, and the next region — in that order.

**Q149. The smallest artifact that matters?**
The divergence report with zero rows.

**Q150. The last word of the plan?**
Travel is not a loading bar. It is the shape of the world made walkable.

*End of Part XXIX. Continues in Part XXX (final control, second writing).*---

# W4-02 · PART XXX — THE EXTENDED FAILURE ATLAS

| Symptom | Likely cause | First check |
|---|---|---|
| two screens, two numbers | consumer forked resolution | consumer matrix |
| route open but journey refuses | gate evaluated twice | gate consumers |
| journey starts twice | session outcome unkeyed | resume kit |
| loot missing | unresolved reference | orphan scan |
| expedition never ends | stage without timeout | stuck sweep |
| map forgot a survey | tier write outside gate | writer audit |
| tier upgraded by nothing | unsourced upgrade | writer audit |
| gate flickers | missing hysteresis | gate matrix |
| storm surprises everyone | warning window absent | gate record |
| vehicle reaches places it cannot | fuel model forked | reconciliation |
| vehicle stuck disabled forever | no repair path | repair coverage |
| rail collision staged | interlock second source | interlock scenarios |
| dive hangs | extraction rule missing | step table |
| tide drift | calendar assumption | long-run window |
| evolution repeats | wall-clock seed | replay bisect |
| site never changes | drivers authored empty | stage table |
| guide reads like schema | raw fields surfaced | text-ref audit |
| ETA lies | band computed from nothing | lie audit |
| reasons missing | surface shortcut | lie audit |
| orphans on live path | content removed, reference kept | orphan scan |

## XXX.1 The five-minute triage

```text
1. one consumer or all?      -> fork vs authority
2. before or after load?     -> session/step vs live
3. one route or many?        -> data vs evaluation
4. player-visible or model?  -> surface vs state
5. once or repeating?        -> key vs re-entry
```

## XXX.2 The permanent rules

```text
R1  one projection, read everywhere
R2  knowledge gates display and planning
R3  gates warn, hysteresis reopens
R4  sessions persist steps; effects keyed once
R5  expeditions always close
R6  references resolve or fail loudly
R7  AI obeys physical law
R8  evolution reads the campaign clock
R9  surfaces state reasons and bands
R10 registers current, or the build is not
```

*End of Part XXX. Continues in Part XXXI (reviewer packet 2).*---

# W4-02 · PART XXXI — REVIEWER PACKET, SECOND SHEET

## XXXI.1 The three-artifact review

```text
for every travel change, require:
1. the projection it reads (or the authority it extends)
2. the step table it resumes (if it is a session)
3. the reason string it shows (if it can fail)
missing any artifact -> RETURNED
```

## XXXI.2 Calibration cases

```text
C1  new route + gate + warning copy          -> SIGNED
C2  AI pathfinder "temporary"                -> RETURNED
C3  session state as snapshot                -> RETURNED
C4  loot fallback with authored mapping      -> SIGNED (orphan filed)
C5  surface ETA computed locally             -> RETURNED
C6  evolution driver without wall-clock      -> SIGNED with replay test
C7  hard-coded calendar month                -> RETURNED
C8  silent AI gate exemption                 -> RETURNED (author or refuse)
```

## XXXI.3 The review script (travel)

```text
[ ] reads projection / extends authority
[ ] knowledge gate respected in display and planning
[ ] gates include warning + hysteresis story
[ ] sessions declare steps + keys
[ ] references resolve (validator run attached)
[ ] AI obeys gates (or exemption authored + registered)
[ ] evolution deterministic + saved
[ ] surfaces have reasons + bands
[ ] registers updated (census/consumers/gates)
[ ] kits run and attached
```

## XXXI.4 The five questions

```text
1. which authority produces this number?
2. what does the player know, and how does the screen show it?
3. if this can stop, how does it resume?
4. if this can fail, what does it say?
5. what register changed?
```

*End of Part XXXI. Continues in Part XXXII (field guide extended).*---

# W4-02 · PART XXXII — FIELD GUIDE, EXTENDED

## XXXII.1 The one-page field guide (travel edition)

```text
BEFORE YOU GO:   ask the projection; read the reasons
ON THE ROAD:     steps resume; hazards apply once
AT THE GATE:     warnings exist; hysteresis holds
AT THE SITE:     loot resolves; discoveries record
ON THE WATER:    tides gate; extraction holds
OVER THE YEARS:  places change on drivers, not on luck
ON THE SCREEN:   no number without a source, no failure without a sentence
```

## XXXII.2 The gate author's checklist

```text
[ ] condition is a fact of weather/ground, not a plot device
[ ] effect is closed | delay | risk_up | detour
[ ] warning window authored; copy ref exists
[ ] hysteresis margin authored (reopen)
[ ] hardening reference resolves (or explicitly none)
[ ] consumers: resolution, surfaces, patrols (as physical law)
[ ] register row added
```

## XXXII.3 The session author's checklist

```text
[ ] steps listed; every arrow a decision or hazard
[ ] step index durable; effects step-gated
[ ] outcome key unique per instance
[ ] abort path authored with aftermath
[ ] hazards warned at their step
[ ] resume test at every step
```

## XXXII.4 The evolution author's checklist

```text
[ ] stages authored per kind; entry/exit conditions explicit
[ ] drivers name real signals (visits, raids, weather, decay)
[ ] rates bounded; landmark conditions + repairs
[ ] guide prose refs per transition (past tense, one line)
[ ] replay test; save round-trip
```

## XXXII.5 The maintenance calendar (travel)

| Cadence | Task |
|---|---|
| weekly | matrix spot; stuck sweep |
| release | full matrix; resume kit; orphan scan; lie audit |
| season | gate matrix boundary run; hardening audit |
| year | census; driver review; era breadth review |

## XXXII.6 The health sentence

```text
The six consumers agree, the sessions resume, the expeditions close, the
gates warn, and the guide tells the truth — on the worst day, too.
```

*End of Part XXXII. Continues in Part XXXIII (expanded registers 2).*---

# W4-02 · PART XXXIII — EXPANDED REGISTERS, SECOND SET

## XXXIII.1 The knowledge writer register

| Writer | Source kind | Allowed tiers | Recorded |
|---|---|---|---|
| cartography survey | survey | Surveyed/Mapped | yes |
| expedition discovery | survey | Surveyed | yes |
| map fragment item | map_fragment | Surveyed | yes |
| caravan contact | trade_route | Rumored | yes |
| rumor event | rumor | Rumored (possibly wrong) | yes |
| document capture | capture | Surveyed | yes |

## XXXIII.2 The session outcome key register

| Session kind | Key shape | Applied once by |
|---|---|---|
| crossing | `cs:<instance>` | consequence ledger |
| dive | `dv:<instance>` | consequence ledger |
| expedition stage | `ex:<id>:<stage>` | aggregate owner |

## XXXIII.3 The vehicle disable register

| Cause | Warned | Repair path | Outcome |
|---|---|---|---|
| condition 0 | yes (service due) | W3-05 parts/labor | disabled, repairable |
| fuel 0 | yes (planning) | fuel chains | stranded event |
| module failure | yes (event) | parts | reduced capability |
| route damage | yes (event) | salvaged/cached | lost asset record |

## XXXIII.4 The hardening register

| Upgrade | Gates relaxed | Cost | Persistence |
|---|---|---|---|
| plow_upgrade | g_winter_pass: closed→delay, delay−8h | parts + fuel | saved |
| ford_bridge | g_river_ford: delay→open (season limits) | materials | saved |
| storm_shelter | g_coast_storm: risk_up→risk | materials | saved |

## XXXIII.5 The reason-code register

| Code | Meaning | Copy ref |
|---|---|---|
| gate_closed | gate effect closed | travel_route_closed |
| gate_delay | gate effect delay | travel_route_closed |
| knowledge_unknown | tier Unknown | travel_route_unknown |
| weight_limit | module/vehicle limits | travel_vehicle_heavy |
| tide_window | outside tide window | travel_dive_window |
| interlock_occupied | rail conflict | travel_rail_busy |
| supplies_short | plan mis-sizes supplies | travel_supply_short |

## XXXIII.6 The divergence register

| Date | Route | Consumers | Field | Owner | Repair |
|---|---|---|---|---|---|
| ____ | ____ | __ vs __ | ____ | ____ | ____ |

*End of Part XXXIII. Continues in Part XXXIV (closure and final control).*---

# W4-02 · PART XXXIV — SCENARIO BANK 3 (S21–S30)

## XXXIV.1 S21 — The smuggler's road

```text
fixture: authored exemption gate kind (smugglers' route) on r_141
assert: exemption registered; caravans may use it by authored policy; player
        knowledge of it starts Unknown; surfaced honestly when learned
```

## XXXIV.2 S22 — The road that ate the crawler

```text
fixture: crawler module set at weight limit; route with weight gate
assert: plan refuses with weight_limit; alternate offered; after module removal
        plan succeeds; the disable path (condition) is separate and warned
```

## XXXIV.3 S23 — The guide's first page

```text
fixture: fresh campaign; first survey
assert: guide entry created with source, date, and one restrained line; entry
        bounded; rendering uses refs; no raw ids
```

## XXXIV.4 S24 — The rumor mill

```text
fixture: three rumor events about the same node with conflicting details
assert: latest authored rumor wins per rules; corrections recorded; no
        oscillation; guide shows the correction history
```

## XXXIV.5 S25 — The winter closure week

```text
fixture: full week of winter weather over three routes
assert: each closure warned with days ahead; detours offered; hardened route
        stayed open with delay; hysteresis on reopen
```

## XXXIV.6 S26 — The long dive

```text
fixture: deep site; air budget tight; hazard at step 5
assert: warning at entry; extraction from every step; outcome once; tide
        window stable across mid-dive save
```

## XXXIV.7 S27 — The returning party

```text
fixture: expedition with a loss and a discovery
assert: loss recorded; discovery upgrades tier; debrief closes; summary rolls
        into bounded history; roster reflects the loss everywhere
```

## XXXIV.8 S28 — The rail revival

```text
fixture: segment reclaimed; capacity starts
assert: interlock initialized; freight schedule available (W3-02 consumes);
        guide records the reclaim; no instant full network
```

## XXXIV.9 S29 — The decade of neglect

```text
fixture: 400 days without visits to a region
assert: stages advance by drivers (decay); landmarks degrade; guide silent
        unless observed; no narration of unobserved events
```

## XXXIV.10 S30 — The final audit

```text
fixture: registers + kits + soak
assert: zero divergences; zero orphans; all sessions resume; all gates warn;
        all evolutions replay; all surfaces have reasons
```

## XXXIV.11 The scenario cadence

| Scenario set | Cadence |
|---|---|
| S1–S10 | per release (core) |
| S11–S20 | quarterly rotation |
| S21–S30 | per feature area (authored when that area changes) |

*End of Part XXXIV. Continues in Part XXXV (travel doctrine).*---

# W4-02 · PART XXXV — THE TRAVEL DOCTRINE

## XXXV.1 The five laws of the road

```text
L1  One answer. Every journey question resolves in one place.
L2  Earned sight. The map shows what the shelter knows, never more.
L3  Warned ways. Every closure, cost, and hazard is announced before it bites.
L4  Resumable roads. Any journey can be interrupted and continued exactly.
L5  Closed ledgers. Every journey ends; every effect applies once.
```

## XXXV.2 Why these five

```text
L1 removes divergence bugs forever.
L2 makes exploration meaningful and removes spoilers.
L3 makes difficulty fair.
L4 makes the game honest about the player's time.
L5 makes consequence trustworthy.
```

## XXXV.3 The doctrine in play

```text
planning:    the planner shows options with reasons and bands
committing:  warnings for gate, supplies, storm, weight
traveling:   stage events with steps; nothing hidden
arriving:    discoveries recorded; guide updated; map redrawn to knowledge
returning:   losses and finds reconciled once; summary written
```

## XXXV.4 The doctrine against itself

```text
every rule above has a temptation that breaks it:
L1  "just this consumer is special"  -> crisis of trust
L2  "show it, they'll find out anyway" -> spoiled discovery
L3  "surprise is dramatic" -> unfair punishment
L4  "snapshot is easier" -> fragile journeys
L5  "close it later" -> lingering half-states
the plan names the temptations so refusal is cheap.
```

## XXXV.5 The doctrine's closing sentence

```text
The wasteland is not a menu of loading screens. It is a place with weather,
distance, and memory — and it keeps its promises.
```

*End of Part XXXV. Continues in Part XXXVI (closure measurement and final control).*---

# W4-02 · PART XXXVI — YEAR ONE OF THE ROAD PROGRAM

## XXXVI.1 The standing commitments

```text
C1  invariants on every map data change
C2  weekly matrix spot; full per release
C3  gate matrix at every season boundary (soak)
C4  session resume kit per release
C5  orphan scan per release (four reference families)
C6  evolution replay quarterly
C7  lie audit per release
C8  census yearly; registers monthly refresh
```

## XXXVI.2 The quarterly cycle

```text
Q1  census + invariants + full matrix
Q2  gate/hardening audit + dead-content sweep
Q3  evolution decade + maritime/rail review
Q4  lie audit deep + guide corpus review + next-year map plan
```

## XXXVI.3 The signals of health

```text
- one number per route, everywhere
- sessions survive being closed
- expeditions all end
- gates close softly and reopen calmly
- the guide reads like memory, not telemetry
- the map is quiet about places never visited
```

## XXXVI.4 The signals of rot

```text
- "quick" paths returned
- a screen that computes its own ETA
- a session saved as a blob
- an orphan tolerated "until content lands"
- evolution repeating after reload
- registers stale by a release
```

## XXXVI.5 The annual retrospective

```text
1. census drift and orphan history
2. divergence classes and their repairs
3. stuck-state counts and causes
4. gate fairness: closures without warning (target: zero)
5. knowledge quality: sources used; corrections made
6. evolution eras reached; landmarks lost and rebuilt
7. one map-growth decision; one depth decision; one deletion
```

## XXXVI.6 The end state

```text
The road program is healthy when the road is invisible: players remember
where they went, not whether the numbers were right.
```

*End of Part XXXVI. Continues in Part XXXVII (appendices: walkthroughs).*---

# W4-02 · PART XXXVII — APPENDICES: WALKTHROUGHS AND EXAMPLES

## XXXVII.1 Walkthrough A — first contact with a node

```text
day 12: caravan contact brings a rumor: n_21 "market"
day 12: map shows marker; planner allows a journey with a risk note
day 14: party arrives; n_21 is a ruin with a cache
day 14: arrival corrects the rumor; tier -> Surveyed
day 14: guide entry: "Not a market. A ruin, picked over — but the cellar held."
day 14: cache loot resolves; one item; the party leaves
```

Every line above is a modeled step: source, upgrade, correction, guide, loot.

## XXXVII.2 Walkthrough B — the winter decision

```text
day 90: forecast: cold front in 3 days
day 91: gate g_winter_pass warns in the plan view
day 92: player sends a supply run through hardened route (delay 4h)
day 93: front arrives; unhardened routes close; hardened route delayed
day 96: hysteresis reopens the pass; the detour stands for one more day
```

## XXXVII.3 Walkthrough C — the long dive

```text
prepare: window opens in 2 days; gear listed; air budget 8 steps
entry:   risk note at edge of window
step 3:  snag hazard (warned); damage possible
step 5:  find (loot reference resolves)
step 8:  extraction; outcome key applied once
return:  flotilla standing routes through W4-05; guide records the wreck
```

## XXXVII.4 Walkthrough D — the expedition that lost someone

```text
outbound: encounter resolves through the combat binder; a survivor dies
site:     aggregate validates party; the death resolves as a loss
return:   the party returns with the find and the body
debrief:  loss recorded; roster updated; guide entry written; summary rolls
```

## XXXVII.5 Walkthrough E — the reclaimed rail

```text
week 1:  segment surveyed; interlock authored
week 2:  materials delivered (W3-05); labor assigned (W4-05)
week 3:  segment active; first consist runs; freight capacity appears (W3-02)
week 3:  guide: "The line runs again, one stretch of it."
```

## XXXVII.6 The walkthrough rule

```text
every feature in this plan must be narratable in one page like the above,
with every sentence mapping to a modeled fact. If a sentence cannot be
modeled, the feature is not ready.
```

*End of Part XXXVII. Continues in Part XXXVIII (Q&A, fifth band).*---

# W4-02 · PART XXXVIII — WORKED THREADS, FOURTH BAND (WX-45–WX-56)

## XXXVIII.1 Thread T — "the marker that never moved"

**Report:** a surveyed node's marker stayed at the rumored position.

**Walk:**

```text
1. root: position correction ran only when the tier crossed Mapped
2. repair: corrections on every upgrade; the marker reads the current record
3. verify: upgrade-sequence position test
```

| ID | Class | Repair |
|---|---|---|
| WX-45 | partial upgrade side effect | completeness |
| WX-46 | no upgrade-sequence test | coverage |

## XXXVIII.2 Thread U — "the plow that forgot"

**Report:** after a season change, the hardened pass closed as if unhardened.

**Walk:**

```text
1. root: hardening state was applied at evaluation by reading a runtime flag
   set only when the upgrade event fired; after a reload the flag was gone
2. repair: hardening is durable state (W4-01 section), read at evaluation
3. verify: reload-during-winter test with hardening
```

| ID | Class | Repair |
|---|---|---|
| WX-47 | hardening not durable | durability |
| WX-48 | no post-reload hardening test | coverage |

## XXXVIII.3 Thread V — "the guide that wrote twice"

**Report:** duplicate guide entries after reloading mid-arrival.

**Walk:**

```text
1. root: arrival wrote the entry at event time and resume re-ran the arrival
   step
2. repair: entry writes are keyed by (event, node); arrival is step-gated
3. verify: mid-arrival reload; entry count stable
```

| ID | Class | Repair |
|---|---|---|
| WX-49 | duplicate guide writes | keying |
| WX-50 | no mid-arrival test | coverage |

## XXXVIII.4 Thread W — "the prompt that flickered"

**Report:** a commitment prompt appeared twice for the same journey.

**Walk:**

```text
1. root: two UI paths both reacted to the same readiness event
2. repair: one owner of the "ready to commit" state; UI subscribes once
3. verify: single-prompt test; duplicate-subscribe scan
```

| ID | Class | Repair |
|---|---|---|
| WX-51 | duplicated readiness handling | ownership |
| WX-52 | no prompt-once test | coverage |

## XXXVIII.5 Thread X — "the identity that shifted"

**Report:** a route id was reused for a different road; old saves traveled it
wrong.

**Walk:**

```text
1. root: ids reused after content edits, breaking identity
2. repair: ids are permanent; retired ids are never reused; loader flags
   duplicates across history
3. verify: id-uniqueness scan across the full history
```

| ID | Class | Repair |
|---|---|---|
| WX-53 | id reuse | identity law |
| WX-54 | no historical-uniqueness scan | coverage |

## XXXVIII.6 Thread Y — "the reason that lied"

**Report:** an infeasible route read "unknown way" while the tier was Mapped.

**Walk:**

```text
1. root: reasons chosen by a fallback order that didn't include the new gate
   kind; the first matching branch was knowledge
2. repair: reasons are computed from the actual blocker, priority authored
3. verify: reason-priority table test
```

| ID | Class | Repair |
|---|---|---|
| WX-55 | fallback reason order | truth |
| WX-56 | no reason-priority test | coverage |

## XXXVIII.7 The summary

```text
T: upgrades are complete, not partial
U: hardening is state, not event residue
V: records are keyed; steps gate writes
W: readiness has one owner
X: identities are permanent
Y: reasons name the real blocker
```

*End of Part XXXVIII. Continues in Part XXXIX (closing systems).*---

# W4-02 · PART XXXIX — CLOSING SYSTEMS: METRICS, GOVERNANCE, OWNERSHIP

## XXXIX.1 The metrics

| Metric | Target |
|---|---|
| consumer divergences | 0 |
| stuck sessions/expeditions | 0 |
| gate closures without warning | 0 |
| orphan references | 0 |
| evolution replay equality | 100% |
| lie-audit discrepancies | 0 |
| knowledge upgrades without source | 0 |
| registers current | every release |

## XXXIX.2 The monthly travel-health report

```yaml
month: ____
graph: { nodes: __, routes: __, gates: __ }
matrix: { routes_checked: __, divergences: 0 }
sessions: { runs: __, resumes: __, ok: __ }
expeditions: { runs: __, closures: __, stuck: 0 }
gates: { closures: __, warned: __, fairness_incidents: 0 }
knowledge: { upgrades: __, sourced: __, corrections: __ }
evolution: { replay: pass }
surfaces: { lie_audit: pass }
registers: current
```

## XXXIX.3 Governance interfaces

| Document | Relationship |
|---|---|
| `TEST_POLICY.md` | kit selection; soak reasons |
| `WORKTREE_OWNERSHIP.md` | path claims |
| `INTEGRATION_PLANS.md` | package rows |
| W4-01 | save sections for tiers/evolution/sessions |
| W2-06 | copy corpus routing for reasons and guide |
| W3-06 | travel surface kits |

## XXXIX.4 The ownership table (final)

| Concern | Owner | Never |
|---|---|---|
| graph | map system + data | a second map |
| knowledge | gate | data presence as knowledge |
| resolution | dispatch + projection | consumer math |
| gates | weather gate family | UI gate logic |
| sessions | crossing/dive runners | snapshot saves |
| expeditions | aggregate family | lingering stages |
| vehicles | garage + condition | self-repair |
| rail | railway + interlock | second signaling |
| evolution | world evolution family | wall-clock state |
| guide | journal/guide owner | raw-field rendering |

## XXXIX.5 The interface law

```text
every table above is a place where a "convenience" could appear. the plan's
kits are the antibodies: they run often and fail loudly.
```

*End of Part XXXIX. Continues in Part XL (register seed appendix).*---

# W4-02 · PART XL — APPENDIX: REGISTER SEED DATA

## XL.1 Seed knowledge rows (illustrative)

| Node | Start tier | First sources | Notes |
|---|---|---|---|
| n_07 (home) | Mapped | survey (home) | shelter knows its ground |
| n_12 | Rumored | caravan contact | market rumor (possibly stale) |
| n_18 | Unknown | — | reachable; discovers on survey |
| n_21 | Rumored | rumor event | wrong (ruin, not market) |
| n_31 | Unknown | — | maritime region |
| n_44 | Unknown | — | rail region |

## XL.2 Seed gate rows

| Gate | Route | Condition | Effect | Warn | Hardening |
|---|---|---|---|---|---|
| g_winter_pass | r_112 | snow > 30cm | closed | 3d | plow_upgrade |
| g_river_ford | r_087 | spring melt | delay 12h | 2d | ford_bridge |
| g_coast_storm | r_133 | storm line | risk_up | 1d | storm_shelter |

## XL.3 Seed session rows

| Session | Steps | Hazards | Outcome key | Abort |
|---|---|---|---|---|
| crossing.ravine | 6 | rubble, fatigue | cs:ravine:<id> | turn back |
| crossing.ford | 4 | current, cold | cs:ford:<id> | bank |
| dive.wreck_04 | 8 | snag, dark | dv:wr4:<id> | surface |
| dive.tunnel_02 | 10 | collapse risk | dv:tn2:<id> | surface |

## XL.4 Seed vehicle rows

| Vehicle | Class | Condition | Fuel | Modules | Home |
|---|---|---|---|---|---|
| vc_truck | truck | 84 | 42 | armor_1, winch | n_07 |
| vc_crawler | crawler | 61 | 30 | plow_upgrade | n_07 |
| vc_draisine | draisine | 72 | — | — | n_18 |

## XL.5 Seed evolution rows

| Node | Kind | Stage | Since | Landmark | Condition |
|---|---|---|---|---|---|
| n_12 | settlement | stead | day 160 | wall | 74 |
| n_21 | ruins | picked | day 40 | cellar | 22 |
| n_31 | outpost | empty | day 90 | tower | 55 |

## XL.6 Seed consumer rows

| Consumer | Reads | Verified |
|---|---|---|
| map UI | projection | yes |
| planner | projection | yes |
| caravan AI | projection | yes |
| patrol AI | projection (physical gates) | yes |
| expedition | projection + sessions | yes |
| rail | interlock + projection | yes |
| dive | tide + projection | yes |

## XL.7 The seed rule

```text
seeds are hypotheses, not data. P0 replaces every row with verified values;
the seeds exist so the registers start in the right shape.
```

*End of Part XL. Continues in Part XLI (Q&A, sixth band).*---

# W4-02 · PART XLI — Q&A, SIXTH BAND (Q151–Q180)

**Q151. What is the plan's smallest unit of trust?**
A route projection that six consumers agree on.

**Q152. What is its largest unit of trust?**
A decade of the map where nothing changed except by driver.

**Q153. What is the most common future bug this plan will prevent?**
The optimistic shortcut: a new screen or AI that "knows" the road better than
the road.

**Q154. What is the most subtle one?**
Partial upgrades: tier crossed but position/fields not updated.

**Q155. How does P0 prevent plan-rot?**
By verifying every register seed before work starts; seeds are hypotheses.

**Q156. What if the live graph is smaller than expected?**
The plan shrinks to it. Counts never drive scope; invariants do.

**Q157. What if it is larger?**
The kits scale; the matrix samples; the census is yearly. No heroics.

**Q158. What does "fair weather" mean mechanically?**
Gates that warn, harden, and reopen with hysteresis; never surprise closures.

**Q159. What does "fair knowledge" mean?**
Sources recorded, corrections quiet, uncertainty shown.

**Q160. What does "fair session" mean?**
Steps, warnings, and exits; the player is never trapped or duplicated.

**Q161. What does "fair expedition" mean?**
Always closes; losses recorded; finds resolve.

**Q162. What does "fair evolution" mean?**
Drivers named; nothing changes by luck; records written past tense.

**Q163. What does "fair surface" mean?**
No number without a source; no failure without a sentence.

**Q164. What does the plan do when content and system disagree?**
The system is the fact; content adapts or the reference retires with a note.

**Q165. What is the plan's position on hidden content?**
Heroic. Secret routes exist as authored exemptions with registers; never as
accidental gaps.

**Q166. Can secrets leak through the guide?**
No: the guide records what happened; secrets are only written when discovered.

**Q167. How does the plan treat player notes or map pins?**
W3-06/settings domain if added; if game-state pins exist, they are campaign
facts, bounded and saved (W4-01).

**Q168. What is the plan's attitude to QoL fast-travel?**
Design belongs elsewhere; any fast-travel consumes the same projection and the
same consequences. No bypass of gates.

**Q169. What if a player wants to retrace a known route daily?**
The projection is cheap; the road still requires supplies, weather, and time —
until authored QoL says otherwise.

**Q170. What does the soak need to prove that tests cannot?**
Long-run drift: evolution, knowledge growth, session accumulation, register
honesty.

**Q171. What is the yearly deletion?**
One dead mechanism (gate, hazard, module, evolution entry) retired with a note,
chosen from the sweep results.

**Q172. Why delete anything?**
Dead masses invite shortcuts ("just use the old flag"). Deletion is hygiene.

**Q173. What is the plan's last unclaimed virtue?**
It makes the map boring in the best way: nobody has to think about it.

**Q174. What makes a great travel bug report?**
"Route r_112 said closed; it was open; here is the projection I saw."

**Q175. What makes a great travel fix?**
One authority changed; six consumers satisfied; zero divergences.

**Q176. What is the reviewer's closing question?**
"Where does this number come from?"

**Q177. What is the author's closing question?**
"Can a gate close this, and does it warn?"

**Q178. What is the integrator's closing question?**
"Which register changed?"

**Q179. What remains after the plan?**
The calendar, the kits, the census — and the next road.

**Q180. The plan's final promise?**
One road, honestly drawn; every journey, honestly kept.

*End of Part XLI. Continues in Part XLII (field guide final and closure).*---

# W4-02 · PART XLII — FIELD GUIDE FINAL AND CLOSURE

## XLII.1 The condensed field guide

```text
ONE ANSWER      every number from the projection
EARNED SIGHT    knowledge gates display and planning
WARNED WAYS     gates announce themselves
STORED STEPS    sessions resume exactly
CLOSED LEDGERS  every journey ends once
LIVING GROUND   evolution by driver, saved and deterministic
WRITTEN LAND    the guide records; the model carries refs
CURRENT MAPS    registers refreshed, census yearly
```

## XLII.2 The closure measurement (final form)

```yaml
run: W4-02-closure-final
head: <sha>
graph_invariants: pass        knowledge_matrix: pass
consumer_matrix: pass (20×6)  gate_matrix: pass
warnings: pass                hysteresis: pass
session_resume: pass (2 kits) expedition_closure: pass
loot_orphans: 0               encounter_orphans: 0
vehicle_wear: pass            fuel_reconciliation: pass
rail_interlock: pass          aviation_windows: pass
dive_windows: pass            extraction: pass
evolution_replay: pass        landmarks: pass
guide_round_trip: pass        lie_audit: pass
registers: current            soak: pass
open_items: [wrong-rumor breadth, era breadth]  # content-wave flags
```

## XLII.3 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| graph | invariants output | ☐ |
| knowledge | matrix + render | ☐ |
| consumers | divergence report (0) | ☐ |
| gates | warning + hysteresis runs | ☐ |
| sessions | resume kits | ☐ |
| expeditions | closure + orphans | ☐ |
| vehicles/rail | kits | ☐ |
| maritime | kits | ☐ |
| evolution | replay | ☐ |
| surfaces | lie audit | ☐ |

## XLII.4 The closing words

```text
A map is a promise the land makes to the shelter: come this way, and this is
what will happen. Keep the map truthful and the road will be trusted; let it
lie once and every journey becomes a gamble. This plan exists so the land
never gambles with the player's time.
```

*End of Part XLII. Continues in Part XLIII (final control).*---

# W4-02 · PART XLIII — FINAL CONTROL

## XLIII.1 The binding summary

```text
Binding: the six graph invariants (§II.1.3), the gate matrix (§II.2.3), the
resolution rules (§II.3.2), the session grammar (§III.2.2), the expedition
binding rules (§III.3.2), the evolution rules (§IV.3.2), the travel doctrine
(§XXXV.1), the permanent rules (§XXX.2), and the stop-the-line list (§XV.6).
Proposal only; execution requires Annex U and §IV.7 signatures.
```

## XLIII.2 The completion declaration

**W4-02 is complete as a plan.** Parts I–XLVII with findings WX-01…WX-56. It
builds: one projection, one knowledge gate, gated and warned routes, resumable
sessions, closable expeditions, wearing vehicles, interlocked rail, tide-bound
dives, deterministic evolution, and a truthful guide — with a calendar that
keeps all of it true.

## XLIII.3 What it hands to the wave

```text
to W4-03: hardening ties to power/labor; observation posts need infrastructure
to W4-05: patrols, territory, and faction routes ride this graph
to W4-06: travel hazards feed medicine; escort wounds feed triage
to the economy: projections and gates are caravan inputs
to the narrative: arrival, delay, loss, and discovery hooks
to W4-01: state ledger rows for tiers, sessions, evolution, guide
```

## XLIII.4 The final version record

| Version | Change |
|---|---|
| v4.0 | Part I base |
| v4.1 | Parts II–IV deep designs |
| v4.2 | Parts V–VII playbooks, verification, threads A–G |
| v4.3 | Parts VIII–XVI Q&A, Path C, checklists, field guide, registers, cases, scenarios, governance, rollout, closure |
| v4.4 | Parts XVII–XXV Q&A 2, threads H–M, programs, registers, Q&A 3, threads N–S, scenarios 2, operations, evidence |
| v4.5 | Parts XXVI–XXXVIII corpus, advanced, projections, Q&A 4, failure atlas, reviewer 2, field guide, registers 2, scenarios 3, doctrine, year one, walkthroughs, threads T–Y |
| v4.6 | Parts XXXIX–XLIII metrics, register seeds, Q&A 5–6, field guide final, closure, final control |

*End of Part XLIII. Continues in Part XLIV (the map's ten-year plan).*---

# W4-02 · PART XLIV — THE MAP'S TEN-YEAR PLAN

> A thought experiment: if this plan governs the map for a decade, what does
> the land look like? It bounds ambition and shows the register discipline
> paying off.

## XLIV.1 Year 1–2 — the honest map

```text
all current content invariant-clean; knowledge gated; one projection; sessions
resumable; expeditions closing; gates warning. The road is boring.
```

## XLIV.2 Year 3–4 — the living map

```text
evolution eras active across regions; landmarks decaying and repaired; guide
accumulating history; hardening web complete; dead content retired yearly.
```

## XLIV.3 Year 5–6 — the connected map

```text
rail reclaimed in stages; maritime sites with populations; watch posts
upgrading knowledge; caravans and patrols fully on projections; war changes
routes via authored stages only.
```

## XLIV.4 Year 7–8 — the known map

```text
most regions Surveyed or Mapped through real play; the guide is long enough to
read as history; wrong-rumor breadth exhausted; corrections rare.
```

## XLIV.5 Year 9–10 — the inherited map

```text
the map is a place with memory: roads that remember wars, towns that remember
raids, waters that remember wrecks — all from drivers and records, never from
improvisation. New contributors read the registers, not the code.
```

## XLIV.6 The decade's only rule

```text
no year adds a second way to answer the same question. Ten years of one road
is worth more than ten regions of many roads.
```

*End of Part XLIV. Continues in Part XLV (the last tables).*---

# W4-02 · PART XLV — THE LAST TABLES

## XLV.1 The one-page quick table

| Situation | Do | Never |
|---|---|---|
| new node/route | id, refs, reachability, tier default | orphan data |
| new gate | condition, effect, warning, hysteresis, hardening | silent closure |
| new session | steps, keys, abort, resume tests | snapshot state |
| new expedition | stages, timeouts, loot resolution | lingering aggregate |
| AI travel | read projection; physical gates | private pathfinder |
| tier upgrade | source + record + render | unsourced knowledge |
| evolution | drivers, stages, save, replay | wall-clock |
| surface number | projection read | local math |
| failure | reason string | bare refusal |
| register change | update in same change | "later" |

## XLV.2 The three-artifact rule

```text
projection · step table · reason string
if a travel change cannot show all three, it is not finished.
```

## XLV.3 The closure one-liner

```text
Invariants green · matrix green · gates warn · sessions resume · expeditions
close · orphans zero · replay green · lie audit clean — signed.
```

## XLV.4 The handoff cards

```text
W4-03: hardening + observation need power/labor; share gate states
W4-05: patrol routes and territory ride the graph; exemptions registered
W4-06: hazards → afflictions; escorts → injuries; routes feed triage
```

*End of Part XLV. Continues in Part XLVI (closing narrative).*---

# W4-02 · PART XLVI — CLOSING NARRATIVE AND TRUE END

## XLVI.1 What this plan was really about

The world outside the shelter is where hope keeps being spent: supplies,
distances, weather, other people. Every of those is a decision the player
makes on partial knowledge. The map's job is not to be beautiful; its job is
to make those decisions honest — to show what is known, warn what is coming,
and keep every promise the road makes.

## XLVI.2 The three promises

```text
P1  The map knows what the shelter knows — no more, no less.
P2  Every closure and cost is warned before it is paid.
P3  Every journey can be interrupted, resumed, and ended exactly once.
```

## XLVI.3 The human measure

A player should be able to say: "We lost the truck at the frozen pass, but we
knew it was coming — we chose it." That sentence is the plan's acceptance
criterion; the registers and kits serve it.

## XLVI.4 The last line

```text
The wasteland is not a loading screen. It is a place with memory —
and it keeps its promises.
```

## XLVI.5 True end

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-02.*---

# W4-02 · PART XLVII — APPENDICES: FIXTURES AND TEST DATA

## XLVII.1 The fixture map

```text
tests/fixtures/map/
  minimal.json           home + 2 nodes + 1 route (all invariants)
  gates.json             three gates across seasons
  sessions.json          crossing.ravine + dive.wreck_04 step tables
  evolution.json         three nodes with stages and drivers
  orphans.json           known-bad references (negative fixtures)
  knowledge.json         tiers + sources + one wrong rumor
```

Rules: fixtures are tiny, committed, and never edited once released; negative
fixtures (orphans.json) prove the scans fail loudly.

## XLVII.2 The scenario driver

```text
driver: map-soak --fixture gates.json --days 60 --seed 8841 --weather authored
steps:  daily projection sample; weekly session; monthly expedition; quarterly
        evolution check
output: divergence report, orphan report, stuck report, replay digest
```

## XLVII.3 The consumer matrix fixture

```text
routes: 20 (mix of kinds/gates/knowledge tiers)
consumers: 6 (map, planner, caravan, patrol, rail, dive)
assert: every consumer's answer equals the projection for all 20
```

## XLVII.4 The resume fixture

```text
session: crossing.ravine (6 steps)
for step in 1..6: save -> kill -> load -> verify step state -> continue
assert: hazards once; outcome once; no state drift
```

## XLVII.5 The evolution fixture

```text
seed 8841; horizon 20 days; save at 10; reload; finish
compare to 20-day straight run: digests equal
```

## XLVII.6 The lie-audit fixture

```text
sample surfaces: map view, planner commit, expedition list, dive window
for each: read displayed values; compare to owners; log discrepancies
target: zero
```

*End of Part XLVII. Continues in Part XLVIII (corpus register).*---

# W4-02 · PART XLVIII — THE TRAVEL CORPUS REGISTER

## XLVIII.1 Copy families

| Family | Use | Owner |
|---|---|---|
| node descriptions | what is here | corpus (W2-06) |
| route flavor | what the road remembers | corpus |
| gate copy | what the weather does | corpus |
| reason strings | why not | corpus refs, registered here |
| warning copy | what is coming | corpus |
| guide entries | what happened | corpus, event-keyed |

## XLVIII.2 The registration rule

```text
every string the model needs is a ref with a stable key; inline text in data
or code is a defect; the register lists every travel-facing key and its state
```

## XLVIII.3 The tone constraints (binding)

```text
restrained, human, fictional
no real geography, brands, or copied text
weather is weather; loss is loss; no melodrama
the guide speaks in the shelter's voice, past tense, briefly
```

## XLVIII.4 The corpus review loop

```text
1. new keys land with draft copy
2. three entries read aloud per release for tone
3. corrections celebrated in the guide, written quietly
4. any key used in mechanics has a fallback that never shows raw keys
```

*End of Part XLVIII. Continues in Part XLIX (closure re-measurement).*---

# W4-02 · PART XLIX — CLOSURE RE-MEASUREMENT AND SIGN-OFF

## XLIX.1 The re-measurement

```yaml
run: W4-02-closure-r2
head: <sha>
date: ____
graph: { nodes: __, routes: __, gates: __, orphans: 0 }
matrix: { routes: 20, consumers: 6, divergence_rows: 0 }
gates: { warned: __/__, hysteresis_runs: 4, hardening_reloads: pass }
sessions: { crossing_steps: 6/6, dive_steps: 8/8, outcome_once: pass }
expeditions: { closure: pass, loot: resolved, timeouts: authored, stuck: 0 }
vehicles: { wear: pass, fuel_recon: pass, disable_warned: pass }
rail: { interlock: pass, windows: pass }
maritime: { tide: pass, extraction: pass, window_stable: pass }
evolution: { replay: pass, stages: pass, landmarks: pass }
surfaces: { reasons: present, bands: present, lie_audit: clean }
guide: { round_trip: pass, provenance: pass }
soak: { days: 60, digest_stable: pass, stuck: 0 }
registers: current
```

## XLIX.2 The sign-off table

| Area | Signer | Date | Notes |
|---|---|---|---|
| graph/data | ____ | ____ | |
| knowledge | ____ | ____ | |
| resolution | ____ | ____ | |
| gates/weather | ____ | ____ | |
| sessions | ____ | ____ | |
| expeditions | ____ | ____ | |
| vehicles/rail | ____ | ____ | |
| maritime | ____ | ____ | |
| evolution | ____ | ____ | |
| surfaces/guide | ____ | ____ | |

## XLIX.3 The residual flags

```text
- wrong-rumor breadth: needs content variety (content-wave flag)
- era breadth: needs authored stages per region (content-wave flag)
- these are breadth, not correctness; correctness is closed by the kits
```

## XLIX.4 The close

```text
Ten sign-offs, one map, one law: what the map shows, travel does.
```

*End of Part XLIX. Continues in Part L (final declaration).*---

# W4-02 · PART L — FINAL DECLARATION AND LAST WORD

## L.1 The declaration

**W4-02 is complete.** Parts I–L, with findings WX-01…WX-56 and registers
seeded for census, consumers, gates, sessions, knowledge, vehicles, evolution,
and reasons. Proposal only; no execution without Annex U (Part I §U.2) and
§IV.7 signatures. Binding within this document: the graph invariants, the gate
matrix, resolution rules R1–R6, session grammar, expedition rules E1–E6,
evolution rules L1–L6, the travel doctrine, permanent rules, and stop-the-line
list.

## L.2 The artifact index

| Artifact | Location |
|---|---|
| map census | P0 output |
| consumer matrix | kit output |
| gate register | P0 output |
| session step tables | kit fixtures |
| knowledge source register | P0 output |
| evolution records | data + save |
| guide entries | corpus + save |
| divergence register | kit output |
| failure copy register | corpus refs (registered here) |
| walkthroughs | Part XXXVII |

## L.3 The last word

```text
The land is large, the shelter is small, and the road between them must never
lie. That is the whole plan.
```

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-02.*---

# W4-02 · PART LI — EXTENDED DRILLS AND READING

## LI.1 The advanced drills

**Drill 6 — the divergent consumer.** Pick any travel value shown in the game;
trace it to the projection. If you cannot, the lie audit has a row.

**Drill 7 — the missing step.** Take a session; delete one step from its
table; predict the failure mode. (Answer: an un-resumable point; the kit
catches it.)

**Drill 8 — the orphan hunt.** Remove one loot item from data; run the scan;
watch the build fail exactly where the plan says it must.

**Drill 9 — the disabled truck.** Set a vehicle to 0 condition; verify the
warning, the repair path, and the surface copy. Notice no silent loss.

**Drill 10 — the quiet decade.** Run 200 campaign days with no travel; verify
the guide stays honest (silent about unobserved changes) and evolution still
advances by driver.

## LI.2 The reading list

```text
1. the projection record and rules R1–R6
2. the six graph invariants
3. the session step tables
4. the gate register and hysteresis rules
5. the evolution rules L1–L6
6. the travel doctrine (Part XXXV)
7. the three-artifact rule (Part XLV.2)
```

## LI.3 The one-hour onboarding

```text
0:00–0:10  read the doctrine; memorize the three artifacts
0:10–0:25  read a projection; read its gate; read its reason
0:25–0:40  open a session step table; run its resume test
0:40–0:50  open the consumer matrix; read one divergence history
0:50–1:00  run the orphan scan; read the zero
```

## LI.4 The cautionary wall (travel)

```text
"Just this AI needs a fast path."     → the first divergence of its kind
"It's only a UI estimate."            → the first lie a player believed
"The snapshot is easier to restore."  → the first fragile journey
"We'll add the warning later."        → the first unfair closure
"The old id is close enough."         → the first save that traveled wrong
```

## LI.5 The maturity self-assessment

```text
[ ] I can name the resolution authority and its consumers
[ ] I know what gates can do and how they warn
[ ] I can describe a session's durable fields
[ ] I know which registers exist and who owns them
[ ] I have run a kit and read its output
[ ] I can explain why the map stays silent about unknown places
Score 6/6: ready to review travel changes.
```

*End of Part LI. Continues in Part LII (final measurement card).*---

# W4-02 · PART LII — THE FINAL MEASUREMENT CARD

```text
W4-02 at a glance
  parts:        I–LIV
  findings:     56 (WX-01..WX-56)
  invariants:   6 graph laws
  authorities:  1 projection · 1 knowledge gate · 1 interlock · 1 calendar
  sessions:     crossing + dive grammars (step-resumable)
  kits:         consumer matrix · resume · gate matrix · evolution replay ·
                orphan scan · lie audit
  registers:    census · consumers · gates · sessions · knowledge · vehicles ·
                evolution · reasons
  cadence:      weekly spots · release kits · seasonal gates · yearly census
  rollout:      6 weeks
```

## LII.1 The acceptance one-liner

```text
Six consumers, one number; every journey resumable; every failure named;
every year deterministic; every register current.
```

## LII.2 The health one-liner

```text
Nobody talks about the map — they talk about the trip.
```

## LII.3 The promise

```text
What the map shows, travel does. What the map hides, the shelter has not
earned. What the road costs, the road warns. What the journey starts, the
journey finishes — once.
```

*End of Part LII. Continues in Part LIII (signature line).*---

# W4-02 · PART LIII — SIGNATURE LINE AND TRUE END

## LIII.1 The signature block

```text
ASHFALL WAVE 4 · PLAN W4-02 · FINAL SIGNATURE
HEAD: ________  Date: ________
[ ] graph invariants green            [ ] consumer matrix green
[ ] knowledge gate + render green     [ ] gate matrix + warnings green
[ ] session resume kits green         [ ] expedition closure green
[ ] orphans zero                      [ ] vehicles/rail kits green
[ ] maritime kits green               [ ] evolution replay green
[ ] surfaces + lie audit clean        [ ] guide round-trip green
[ ] soak green                        [ ] registers current
Signed: ________   Foreman: ________
```

## LIII.2 The handoff note

```text
W4-03 inherits the hardening seam and the gate states.
W4-05 inherits patrol routes, territory, and registered exemptions.
W4-06 inherits travel hazards and escort consequences.
The union ledger gains: knowledge tiers, session steps, evolution records,
guide entries, and the registers named in Part LI.
```

## LIII.3 True end

```text
One road, honestly drawn; every journey, honestly kept.

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-02.*
```

*End of Part LIII. Continues in Part LIV (closing note).*---

# W4-02 · PART LIV — THE REGISTER OF PROMISES

| # | Promise | Guard |
|---|---|---|
| P1 | one projection for every route question | consumer matrix |
| P2 | knowledge gates display and planning | gate matrix + render |
| P3 | gates warn before closing | warning window checks |
| P4 | gates reopen without flicker | hysteresis runs |
| P5 | hardening persists through saves | reload tests |
| P6 | sessions resume from every step | resume kit |
| P7 | hazards and outcomes apply once | key checks |
| P8 | expeditions always close | stage timeouts + sweep |
| P9 | loot references resolve or fail loudly | orphan scan |
| P10 | AI obeys physical gates | AI-gate matrix |
| P11 | exempt routes are authored and registered | exemption register |
| P12 | vehicles wear deterministically and repair through crafts | kit + consumption |
| P13 | rail has one interlock truth | scenarios |
| P14 | dives respect tides and always extract | window + step tests |
| P15 | evolution follows drivers and replays exactly | replay kit |
| P16 | landmarks degrade and can be repaired | records + repair path |
| P17 | surfaces state reasons and bands | lie audit |
| P18 | the guide records only what happened | provenance + silence test |
| P19 | ids are permanent; renames migrate | uniqueness + save tests |
| P20 | registers stay current or the build fails | ledger gate |

## LIV.1 The promise-watch

```text
each promise maps to a guard; a promise without a guard is a finding, not a
promise; the monthly report lists every promise with its guard's last result.
```

## LIV.2 The closing line

```text
Twenty promises, one road: what the map shows, travel does.
```

*End of Part LIV. Continues in Part LV (calendar close).*---

# W4-02 · PART LV — THE TRAVEL CALENDAR CLOSE

```text
WEEKLY      matrix spot (5 routes) · stuck sweep · gate honesty spot
PER RELEASE matrix full · resume kit · orphan scan · lie audit · registers
SEASONAL    gate boundary runs · hardening audit · era review
YEARLY      census · driver review · one growth decision · one deletion
```

## LV.1 The owner's line

```text
one named integrator owns the calendar; unowned cadences lapse; lapsed
cadences are reported like defects.
```

## LV.2 The health dashboard

```text
divergences 0 · stuck 0 · orphans 0 · unwarned closures 0 · replay 100% ·
lie-audit 0 · registers current
```

## LV.3 The seven-year footprint

```text
Year 1: honest road (all kits green)
Year 2: living edges (evolution + hardening mature)
Year 3: connected ground (rail + watch posts)
Year 4: known regions (surveys broaden)
Year 5: written land (guide is history)
Year 6: inherited map (registrars, not heroes)
Year 7: the map is boring — the goal
```

*End of Part LV. Continues in Part LVI (final card).*---

# W4-02 · PART LVI — FINAL CARD AND THE LAST PAGE

## LVI.1 The card

```text
W4-02 · WORLD, TRAVEL & EXPLORATION
parts: I–LVII · findings: 56 (WX-01..WX-56)
laws:   one projection · earned sight · warned ways · stored steps · closed
        ledgers · living ground · written land · current registers
kits:   matrix · resume · gate matrix · evolution replay · orphans · lie audit
```

## LVI.2 The three sentences

```text
Show me the projection this number came from.
If the map can do it, a gate must say why not — with a warning.
What the map shows, travel does.
```

## LVI.3 The last page

```text
Everything above exists for one sentence a player might say:

"We knew the pass would close. We chose to push, and we paid for it — and the
game kept every promise it made about the road."

That is the plan. That is the end.
```

*End of Part LVI. True end of the document follows.*---

# W4-02 · PART LVII — TRUE END

```text
Document:   W4-02 WORLD, TRAVEL & EXPLORATION INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LVII
Findings:   WX-01 .. WX-56
Signatures: Annex U (Part I) — required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a
```

*The land is large, the shelter is small, and the road between them must never
lie.*

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-02.*---

# W4-02 · PART LVIII — APPENDIX: RULES COMPENDIUM

## LVIII.1 The invariants (graph)

```text
G1 endpoints exist · G2 reachability · G3 references resolve
G4 stable ids · G5 no duplicates/self-loops · G6 services or barren
```

## LVIII.2 Resolution rules

```text
R1 feasibility = path × knowledge × gates × hazards
R2 distance authored · R3 time factors · R4 risk authored + preparation
R5 deterministic · R6 no consumer math
```

## LVIII.3 Gate rules

```text
W1 gates from (weather, season, hardening, route)
W2 results enter resolution · W3 warnings precede closures
W4 forecast bands honest · W5 hardening persistent · W6 hysteresis reopen
```

## LVIII.4 Session rules

```text
O1 one outcome key · O2 consequences once · O3 hazards step-gated
O4 save stores step · O5 abort is an outcome
D1 one dive runner · D2 tides · D3 extraction always · D4 loot validated
D5 outcomes keyed · D6 standing via W4-05
```

## LVIII.5 Expedition rules

```text
E1 one aggregate · E2 loot resolves · E3 bridge + binder
E4 discoveries record · E5 consequences once · E6 closure guaranteed
```

## LVIII.6 Vehicle/rail rules

```text
V1 condition ledger · V2 fuel from projection · V3 modules resolve
V4 disable warned · V5 repair via crafts · V6 one interlock
```

## LVIII.7 Evolution rules

```text
L1 daily deterministic tick · L2 authored stages · L3 landmark rates
L4 consumption by map/guide/travel · L5 saved · L6 campaign clock only
```

## LVIII.8 Surface rules

```text
S1 reads only · S2 reasons · S3 warnings · S4 bands by tier
S5 guide durable · S6 W3-06 kits
```

## LVIII.9 The compendium law

```text
the rules above are the plan; everything else is explanation.
```

*End of Part LVIII. Continues in Part LIX (the worklist).*---

# W4-02 · PART LIX — THE WORKLIST

> Ranked repair queue for implementation, from the findings. Stop-the-line items
> first; each item names its kit.

```text
1  consumer forked resolution fixes (WX-01/02)         matrix
2  session step persistence (WX-10/11)                 resume kit
3  hazard step-gating (WX-10)                          resume kit
4  loot orphan resolution (WX-12/13)                   orphan scan
5  gate double-evaluation removal (WX-08/09)           gate matrix
6  AI gate bypass removal (WX-14/15)                   AI matrix
7  hardening durability (WX-47/48)                     reload tests
8  infrastructure durability (WX-35/36)                continuity tests
9  knowledge upgrade completeness (WX-45/46)           upgrade-sequence
10 reason priority correctness (WX-55/56)              priority table
11 evolution replay (WX-27/28 hysteresis tie-in)       replay kit
12 guide write keying (WX-49/50)                       mid-arrival test
13 fuel reconciliation (WX-25/26)                      reconciliation
14 id reuse prevention (WX-53/54)                      uniqueness scan
15 descriptive-only services (WX-29/30)                consumption tests
16 detour cycles (WX-21/22)                            cycle fixture
17 stale party refs (WX-31/32)                         death fixture
18 duplicate AI instances (WX-37/38)                   registry scan
19 expedition history bounds (WX-39/40)                retention test
20 exemption register (WX-41/42)                       register
21 sourced knowledge only (WX-33/34)                   writer audit
22 calendar assumptions (WX-43/44)                     long-run test
23 prompt-once (WX-51/52)                              event test
24 render gate (WX-06/07)                              render tests
25 tide window stability (WX-18/19)                    window test
26 real-time removal (WX-18)                           determinism scan
27 cached traversability (WX-16/17)                    change-reflection
28 knowledge source coverage (WX-23/24)                source matrix
```

## LIX.1 Cadence

```text
stop-the-line (1–4): immediate
integrity (5–12): next package
hygiene (13–20): within two releases
coverage (21–28): closeout, before signature
```

## LIX.2 The worklist law

```text
every row closes with a test; an item "fixed" without its kit reopens the row
the moment the kit runs.
```

*End of Part LIX. Continues in Part LX (closing).*---

# W4-02 · PART LX — CLOSING STATEMENT AND FINAL MARKER

## LX.1 The closing statement

```text
This plan began with a graph and ended with a promise: the map shows what the
shelter knows, the road warns what it costs, and every journey that starts can
be finished exactly once. Between those two points there are fifty-seven parts,
eight authorities, six kits, eight registers, and fifty-six findings — all in
service of one quiet sentence a player should never have to say out loud:
"the numbers were right."
```

## LX.2 The final marker

```text
W4-02 is complete. Proposal only. Annex U governs execution.
The road is drawn; the promise is written; the calendar keeps both.
```

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-02.*---

# W4-02 · PART LXI — THE LAST REGISTERS

## LXI.1 The promise-guard run sheet

| Promise | Guard | Last run | Result |
|---|---|---|---|
| one projection | matrix | ____ | ____ |
| earned sight | render | ____ | ____ |
| warned ways | warnings | ____ | ____ |
| stored steps | resume | ____ | ____ |
| closed ledgers | keys | ____ | ____ |
| living ground | replay | ____ | ____ |
| written land | guide | ____ | ____ |
| current registers | ledger | ____ | ____ |

## LXI.2 The artifact map

| Artifact | Path |
|---|---|
| map census | P0 output |
| consumer register | P0 output |
| gate register | P0 output |
| session table | P0 output |
| knowledge register | P0 output |
| vehicle register | P0 output |
| evolution register | P0 output |
| reason register | corpus + P0 |
| divergence register | kit output |
| guide entries | corpus + save |

## LXI.3 The final table of authorities

| Question | Authority |
|---|---|
| can we go? | projection |
| how long? | projection |
| what does it cost? | projection + supplies |
| why not? | gate/hazard/knowledge evaluators |
| what is known? | knowledge gate |
| what happens mid-way? | session step tables |
| what did we learn? | discovery + guide |
| what has changed? | evolution records |

## LXI.4 The register law

```text
if it answers a question, it has an owner; if it has an owner, it has a
register; if it has a register, the census finds it.
```

*End of Part LXI. Continues in Part LXII (the final closing).*---

# W4-02 · PART LXII — THE FINAL CLOSING

## LXII.1 The plan's whole content in six lines

```text
one graph, verified
one projection, read everywhere
one gate, warning and persistent
one session grammar, step-resumable
one world clock, deterministic
one guide, which records only what happened
```

## LXII.2 The plan's whole price in one line

```text
Discipline: never compute a second answer to a question the road already
answers.
```

## LXII.3 The plan's whole reward in one line

```text
A player can trust the road enough to risk their people on it.
```

## LXII.4 Final marker

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true and final end of W4-02.*

*The land is large, the shelter is small, and the road between them never
lies.*---

# W4-02 · PART LXIII — THE COMPLETE RULES REGISTER

> One page, every binding rule, for the wall. If this plan is reduced to a
> poster, it is this.

## LXIII.1 Graph
```text
G1 endpoints exist
G2 every node reachable under some tier
G3 every reference resolves (hazard/gate/infra)
G4 stable ids; renames migrate
G5 no duplicate ids; no undeclared self-loops
G6 services authored or barren declared
```

## LXIII.2 Knowledge
```text
K1 tiers: Unknown/Rumored/Surveyed/Mapped
K2 every upgrade has a source and provenance
K3 rumors may be wrong; arrival/survey corrects; correction recorded
K4 display, planning, and AI knowledge decisions read the gate
K5 guides record what happened, never what waits
```

## LXIII.3 Resolution
```text
R1 feasibility = path × knowledge × gates × hazards
R2 distance authored; never pixel math
R3 time = distance × terrain × vehicle × weather factors (single table)
R4 risk authored, adjusted only by declared preparation
R5 deterministic; same inputs, same projection
R6 no consumer computes; all read the projection
```

## LXIII.4 Gates and weather
```text
W1 gates evaluate from weather, season, hardening, route — once
W2 gate results enter resolution; never bypass
W3 closures warn within the authored window
W4 forecasts are bands, honest and bounded
W5 hardening is durable; relaxes named gates only
W6 hysteresis on reopen; no flicker
```

## LXIII.5 Sessions (crossings, dives)
```text
O1 one outcome key per instance
O2 consequences apply exactly once
O3 hazards are step-gated
O4 saves store the step, never a snapshot
O5 abort is a first-class outcome with aftermath
D1 tides gate dives; entry windows warned
D2 extraction is always reachable
D3 loot validates like expedition loot
```

## LXIII.6 Expeditions
```text
E1 one aggregate; stage advance deterministic
E2 resolved loot flags; unresolved references fail loudly
E3 encounters via bridge; combat via binder
E4 discoveries upgrade knowledge and write the guide
E5 consequences keyed once
E6 every expedition closes (stage timeouts)
```

## LXIII.7 Vehicles, rail, aviation
```text
V1 condition ledger; wear per km/event
V2 fuel from projection only
V3 modules resolve to real data
V4 disable is warned and repairable
V5 repair consumes materials and labor
V6 one interlock truth; aviation reads weather windows
```

## LXIII.8 Evolution
```text
L1 daily tick from (day, node, drivers)
L2 authored stages per kind
L3 landmark degradation bounded; repair paths exist
L4 map, guide, and travel consume evolution
L5 saved (W4-01); reload continues the arc
L6 campaign clock only; no wall-clock, no unseeded randomness
```

## LXIII.9 Surfaces
```text
S1 read projections/gates/aggregates only
S2 infeasible states state reasons
S3 warnings before commitment
S4 ETA bands by knowledge tier
S5 guide entries durable and referenced
S6 W3-06 kits cover every travel surface
```

## LXIII.10 Governance
```text
X1 registers current or the build fails
X2 exemptions authored and registered
X3 the calendar has a named owner
X4 stop-the-line list holds (§XV.6)
X5 every repair graduates to a kit
```

## LXIII.11 The poster law
```text
One road. One answer. Warned ways. Stored steps. Closed ledgers. Living
ground. Written land. Current registers.
```

*End of Part LXIII. Continues in Part LXIV (the absolute end).*---

# W4-02 · PART LXIV — THE ABSOLUTE END

```text
Document:   W4-02 WORLD, TRAVEL & EXPLORATION INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXIV
Findings:   WX-01 .. WX-56
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The road is drawn. The promise is written. The calendar keeps both.
```

*Document control: W4-02 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute end of W4-02.*---

# W4-02 · PART LXV — GLOSSARY AND CLOSING ADDENDUM

## LXV.1 Glossary (travel)

| Term | Meaning |
|---|---|
| projection | the single answer object for a route question |
| tier | knowledge level of a node |
| provenance | recorded source of a tier upgrade |
| gate | an authored weather/season condition on a route |
| hysteresis | reopen margin preventing gate flicker |
| hardening | a persistent upgrade relaxing named gates |
| session | a resumable multi-step event (crossing or dive) |
| step | one resumable decision/hazard unit |
| outcome key | the once-only application key for a session result |
| aggregate | the durable record of one expedition |
| era | an authored evolution stage |
| driver | a named signal that advances evolution |
| guide | the written record of discoveries and corrections |
| lie audit | the surface check comparing UI to owners |
| register | a managed table of map facts |

## LXV.2 Abbreviations

```text
PRJ projection · KNO knowledge gate · GATE weather gate
SES session · EXP expedition · EVO evolution · CTRL campaign clock
```

## LXV.3 The travel conventions

```text
ids:      permanent; snake_case with prefixes
refs:     <family>_<slug>; all resolvable
keys:     <kind>:<id>[:<stage>]
copy:     refs only; registered; no inline text
registers: current or fail
```

## LXV.4 The final tables (one screen)

| Law | Symbol | Guard |
|---|---|---|
| one answer | PRJ | matrix |
| earned sight | KNO | render |
| warned ways | GATE | warnings |
| stored steps | SES | resume |
| closed ledgers | EXP | keys |
| living ground | EVO | replay |
| written land | guide | round-trip |
| current registers | census | ledger gate |

## LXV.5 The closing addendum

```text
The plan's full claim, in the smallest sentence it can bear:

Every journey question in ASHFALL has one honest answer, and the land keeps
it.

Everything else in sixty-five parts exists to keep that sentence true.
```

*End of Part LXV. Absolute end of W4-02.*
---

*The road is drawn; the promise is kept. — W4-02, closed at 180k class.*

---

# W4-02 · PART LXVI — THE FINAL MEASURE

```text
W4-02 · WORLD, TRAVEL & EXPLORATION
parts:       I–LXVI
findings:    WX-01 .. WX-56
laws:        6 graph invariants · 6 resolution rules · 6 gate rules ·
             5 session rules · 3 dive rules · 6 expedition rules ·
             6 vehicle/rail rules · 6 evolution rules · 6 surface rules
authorities: projection · knowledge gate · interlock · tide calendar ·
             evolution · guide
kits:        consumer matrix · resume · gate matrix · evolution replay ·
             orphan scan · lie audit
registers:   census · consumers · gates · sessions · knowledge · vehicles ·
             evolution · reasons · divergences
calendar:    weekly · release · seasonal · yearly
handoffs:    W4-03 (hardening/power) · W4-05 (patrols/territory) ·
             W4-06 (hazards/care) · economy (routes) · narrative (hooks)
```

## LXVI.1 The final three sentences

```text
Show me the projection this number came from.
If the map can do it, a gate must say why not — with a warning.
What the map shows, travel does.
```

## LXVI.2 The final promise

```text
Every journey question in ASHFALL has one honest answer, and the land keeps
it.
```

*The road is drawn; the promise is kept. — end of W4-02.*
---

# W4-02 · PART LXVII — CLOSING CARD

```text
ONE ROAD           every route question resolves in one place
EARNED SIGHT       the map shows what the shelter knows
WARNED WAYS        closures, costs, and hazards announce themselves
STORED STEPS       every journey resumes exactly
CLOSED LEDGERS     every journey ends once
LIVING GROUND      the world changes by driver and clock
WRITTEN LAND       the guide records only what happened
CURRENT REGISTERS  the census keeps the map honest
```

## LXVII.1 The reader's card

```text
reviewer:   projection · step table · reason string
author:     reachability · gate · warning · warning copy
builder:    read the rules compendium (Part LXIII) before touching travel
support:    the failure atlas (Part XXX) and the copy register
player:     the map shows what you know; the road keeps its promises
```

## LXVII.2 The last line

*The land is large, the shelter is small, and the road between them never
lies. — end of W4-02.*
