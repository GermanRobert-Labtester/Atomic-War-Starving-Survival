# Plan 32 — One Map, Three Notions of Place

> **Wave:** Continuity Wave 4 — *The World Beyond the Gate*
> **Depends on:** 31 (movement and discovery must be reportable), 30A/B (territory is drawn on it).
>
> **Theme:** the game has three separate ideas of "where": a **6-node travel graph** with no
> distances, **218 `loc_` ids** across the authority (115 in `locations.json`), and **expedition
> travel that uses `locationId` without consulting the graph at all**. The registry advertises
> "Wasteland Map (261 Nodes, 6 Tiers) … graph-based node network across 261 locations with
> terrain-modified travel vectors". The graph has six nodes, seven routes, and every
> `distance_km` is empty. The map is not wrong — it is *disconnected* from the thing that travels.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The travel graph is tiny | `Assets/StreamingAssets/Data/wasteland_map_v1.json` → `nodes: 6` (`loc_holdfast`, `loc_cut_abandoned_depot`, `loc_cut_radiation_zone_alpha`, `loc_black_flotilla_outpost`, `loc_cut_merchant_caravanserai`, `loc_cut_arsenal_ruin`), `routes: 7` |
| 2 | **Every route lacks its distance** | `MapRouteDef` documents *"Distance between nodes in kilometers"* (`Assets/Ashfall.Core/World/WastelandMapCatalogLoader.cs:62`) but all 7 routes resolve to `distance_km = None` — travel cost cannot come from the map |
| 3 | The claim in the docs doesn't match | `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md:623` `Map["WastelandMapSystem (261 Nodes, Routes, 6 Danger Tiers)"]`, `:656` *"graph-based map node network across 261 locations with terrain-modified travel vectors"*, `:1771` matrix row. Reality: 6 nodes / 7 routes; 218 unique `loc_` ids exist across all data (115 in `locations.json`) — so "261" describes the location universe, not the graph |
| 4 | Expeditions don't use the graph | no `WastelandMap`/`MapNode` reference in `Assets/Ashfall.Core/Expeditions/*`; travel is by `locationId` (`src/Main.Expeditions.cs:177–180` starts combat at `state.locationId`) with `ExpeditionHostSession` validating targets as *known locations* (`:225`, `:376` — `"unknown_target"`), not as graph-reachable places |
| 5 | The map system is live but only for status text | `src/Host/WorldHostSession.cs:51,66,99` constructs/restores `WastelandMap`; `src/UI/MapPanel.cs:198` calls `ResolveNodeStatus(item.id)`; persisted via `src/Main.Expeditions.cs:211` `SaveWastelandMap()` (registered in `SaveSectionRegistry`) |
| 6 | Terrain/vector modelling exists on the node side | `MapNodeDef` (`WastelandMapCatalogLoader.cs:22`) + route validation (`MapRouteValidationError`, `:86–99`) — the shape for terrain-modified travel is authored; there is simply no populated graph to run it on |
| 7 | Map-shaped content is orphaned | `damaged_map_zones.json` (3 defs — *"map fragments revealing hidden installations"*) has **no consumer** outside `ContentUtilizationScanner.cs`; `wave: ashfall-dialog-graph-lint` reachability over `loc_`/`sector_`/`zone_` is unrun |
| 8 | Discovery exists but unlocks nothing | `SignalTriangulationSystem.cs:83,188` raises `OnLocationRevealed(locationId)`; consumers are `RadioHostSession.cs:60` (status text), `TriangulationPanel.cs:53`, `Main.Narrative.cs:206` — expedition targeting does not read reveal state, so "discovered" is a label |
| 9 | Adjacent place-systems each hold their own truth | `LocationEvolutionSystem`, `LandmarkDegradationSystem`, `WildlifeMigrationSystem` (all ticked under `world_evolution`, which emits 7 event sites incl. landmark collapse at a `locationId`), `WaystationNetwork` route, `TravelingCaravanSystem`, `District8DeepCoastSystem` |
| 10 | Territory is about to need the graph | Wave 4's 30B renders `territorialControlPercent` on `FactionWarMapWidget` — control percentages currently have no spatial representation to attach to |

---

## Task 32A — Decide what "a place" is, and make one place authority

**Goal:** one canonical node/location model, so travel, discovery, territory, encounters, evolution,
and radio all speak about the same ground.

**Files:** `Assets/Ashfall.Core/World/WastelandMapSystem.cs`, `WastelandMapCatalogLoader.cs`,
`wasteland_map_v1.json`, `locations.json` + expansion location catalogs,
`Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (+ `CatalogIntegrityRules.cs`),
`docs/data/CATALOG_REGISTRY.md`, `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`.

### Substeps

1. **Write the ADR before touching data**: `loc_` = anything nameable in the world (218 today);
   `map node` = a place reachable on the travel graph (6 today); decide whether nodes are a
   *projection* of locations (recommended — one authority, a view over it) or a second list to keep
   in sync (rejected; it is the current bug).
2. **Fix the documentation claim first** so nobody plans against 261 nodes: registry rows
   `:623/:656/:1771` restated with the real counts and an evidence pointer (Wave 3's 29B pattern).
3. **Choose the graph size deliberately**: 25–40 nodes is a playtable overdrive for a
   node-graph 2D management game; more than that needs procedural routing. Record the number and
   the reason — "the registry said 261" is not a reason.
4. **Populate `distance_km` for every route** from the terrain model (`MapNodeDef` terrain +
   `MapRouteDef`), because travel time, fuel (21's vehicle logistics), and dose-per-hour (20A) all
   want a distance, not a hand-waved tick count.
5. **Add a route integrity tier** to `CatalogIntegrityValidator`: nodes resolve to existing
   `loc_` ids, routes are bidirectional or explicitly one-way, graph is connected from
   `loc_holdfast`, no orphan node, no route with null distance. Today `:86–99` already has
   validation plumbing to extend.
6. **Model the missing vector fields** the claim promised: terrain type, travel modifier, danger
   tier (6 tiers claimed — author them in data), visibility/cover for expeditions.
7. **Expand the node set from existing authored locations** — the 115 `locations.json` entries and
   expansion catalogs already name places; promote a subset to nodes rather than inventing new ids
   (the `AGENTS.md` id rule: never invent an id outside the master list).
8. **Reveal state becomes graph state**: mark nodes `Unknown / Reported / surveyed / Visited` from
   triangulation (`OnLocationRevealed`), damaged-map fragments (`damaged_map_zones.json` — 32C), and
   first arrival; persist in the map section that already exists.
9. **One query surface**: systems ask `WastelandMapSystem` about a place (terrain, reachability,
   control, status) rather than each keeping a private location notion — the same
   single-authority principle as Wave 2's plans 21B/23A/24A.
10. **Deprecate `damaged_map_zones.json`'s orphan status** by wiring it here (32C) or removing it
    with a record (Wave 1's 18B step 2 classification).
11. **Migration safety**: adding nodes changes saves that key on node ids — prove old saves load
    (the map section's restore path) and pin a wire-contract test like the existing
    `SaveWireContract` suite does.
12. **Tests**: integrity tier tests, graph reachability, reveal persistence, terrain→travel
    modifier, and a data test asserting every node is reachable from the Holdfast.
13. **Run the checklist** + `--data-integrity-selftest` (must stay at 0 errors with the new tier).

**DoD:** one authority answers "where is this, how far, how dangerous, who holds it, do I know
about it" — and the docs agree with the data.

---

## Task 32B — Travel on the graph: routes, time, cost, and risk

**Goal:** expeditions and caravans move **along edges**, so distance, terrain, weather, and control
become the same journey the player can see on the map.

**Files:** `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` (+
`ExpeditionSystem.Estimate`), `src/Host/ExpeditionHostSession.cs`, `ExpeditionVehicleSystem.cs`,
`TravelingCaravanSystem.cs`, `Waystation*`, `src/UI/ExpeditionPanel.cs`, `MapPanel.cs` /
`MapAtlasPanel.cs`, `src/Host/ExpeditionEncounterBridge.cs`.

### Substeps

1. **Read `ExpeditionSystem.Estimate`'s current tick math** (ticks, fuel, capacity, breakdown +
   readiness-adjusted risk from Initiative #101) and express it as *sum over route edges* instead of
   a per-destination constant — same outputs, honest inputs.
2. **Dispatch takes a path**: origin → destination resolved on the graph (a single `FindPath` in
   Core); the UI shows the route on the map, so the player sees the ground they are buying.
3. **Per-edge cost**: distance ÷ terrain-modified speed × weather modifier (20C's
   `WeatherEffects`) × vehicle multiplier (existing `ExpeditionVehicleProfile` speed factor).
4. **Per-edge dose**: ambient zone radiation from the node (20A's resolver) integrated over travel
   hours — this is the single most valuable link in the plan: *route choice becomes a dose choice*.
5. **Per-edge risk**: encounter probability from terrain + controlling faction (30B) — replace the
   current flat risk with the sum along the path, and surface the breakdown.
6. **Fuel and wear follow the road**: vehicle fuel per tick and wear per km reuse 21's condition
   ledger, so a long route is a real expenditure, not a longer wait.
7. **Waystations and camps** become graph nodes with resupply semantics (the camp-night machinery
   already exists in `ExpeditionSystem`'s camp events) — a waystation is a safe edge endpoint, not a
   description.
8. **Interception**: with control percentages live (30), a route through contested territory can be
   stopped — resolve through the existing encounter bridge, not a new event kind.
9. **Blockage and rerouting**: a collapsed landmark (`world_evolution` already emits
   `hazard_warning` with `locationId`), a washed bridge, or a blockade closes an edge; the player
   sees it on the map and reroutes — the payoff for the evolution systems that currently only
   journal.
10. **Caravans travel the same edges** — `TravelingCaravanSystem` arrival timing derives from the
    graph so the trader at your gate is subject to the same roads (and 30C's autonomy).
11. **Legibility**: the expedition panel shows distance, hours, fuel, projected dose, and per-edge
    risk with the route drawn; keyed text via Wave 3's 25A (no new inline English).
12. **Balance**: sweep route length × weather frequency × vehicle condition with
    `ashfall-balance-sim`; the farthest node must be survivable with preparation and fatal without.
13. **Tests**: pathfinding, cost equivalence with the old estimate (behaviour-preserving baseline),
    dose integration, edge closure, caravan timing, save round-trip of a mid-route expedition,
    determinism of a chosen path.
14. **Run the checklist** + `--expedition-selftest`.

**DoD:** the map is the interface for deciding how to spend fuel, dose, and luck.

---

## Task 32C — What the player knows about the world: fragments, reports, and memory

**Goal:** make knowledge of places a resource with sources, and let the map record what the
holdfast has learned — the information economy the atlas already describes, applied to geography.

**Files:** `damaged_map_zones.json` (+ new loader or removal), `SignalTriangulationSystem.cs`,
`Main.Narrative.cs:206`, `MapPanel.cs`, `MapAtlasPanel.cs`, `CartographyGis*` (see Wave 1's 16A
verdict), `LocationMemorySystem` / strata inscriptions, `StandingRecordEngine`, journal.

### Substeps

1. **Wire or retire `damaged_map_zones.json`** — 3 authored map fragments revealing hidden
   installations, currently consumed by nothing: a half-day of work that converts dead content into
   discoverable geography (Wave 1's 18B classification applies).
2. **Define the knowledge ladder** (Unknown → Rumoured → Located → Surveyed → Visited → Mapped)
   with the source for each rung (radio triangulation, fragment, traveller tale, own survey) — and
   make expedition targeting respect it (32A step 8, 32B's path preview degrades with knowledge).
3. **Uncertainty must be honest**: an unsurveyed node reports a *range* of distance/dose, not a
   false precise number, and calibration/`geiger_calibration` quality widens or narrows that range.
4. **Cartography as a craft**: survey work that converts Rumoured→Surveyed using existing
   knowledge/research progression and a duty shift (24's labour), so map quality is an investment.
5. **Location memory on the map**: strata inscriptions and `LocationMemorySystem` outputs surface as
   place notes when a node is inspected (the atlas calls this "Hidden State (Weak Feedback)" — this
   is the fix), and feed `StandingRecordEngine` as already intended.
6. **Persist discovery** in the map section (already saved) so a reloaded campaign keeps its
   knowledge, and a new campaign starts ignorant.
7. **Share knowledge with the world**: revealed nodes should also be *revealed to others* — radio
   broadcasts and caravans carry geography (33), so a mapped route becomes common knowledge and
   therefore contested (30B).
8. **UI**: the atlas shows knowledge state per node with words and numbers, not colour-only
   (`ashfall-ui-access`), plus a "how do I know this" tooltip citing the source event.
9. **No omniscience**: the player must not be able to see territory control they have no channel
   for; the map renders what the holdfast knows. This is the plan's central rule.
10. **Reachability lint**: run `ashfall-dialog-graph-lint` over `loc_`/`sector_`/`zone_` ids so no
    authored location is unreachable and no node is unvisitable.
11. **Snapshots** for the map at three knowledge depths; the atlas is the game's cover art — it must
    look like progress.
12. **Tests**: ladder transitions, fragment reveal, survey cost, uncertainty ranges, persistence,
    and a test that unknown nodes expose no precise values.
13. **Run the checklist.**

**DoD:** the map is a record of what the holdfast learned, how it learned it, and what it paid.

---

## Cross-Task Dependencies

```
31A (event kinds) ──► 32B steps 9 & 32C step 7 (report closures, reveals, surveys)
30A/30B (territory) ──► 32A step 9 (control per node) ──► 32B step 8 (interception)
20A/20C (zone dose, weather) ──► 32B step 4 (per-edge dose)
21A/21B (gear/vehicle condition) ──► 32B step 6 (fuel + wear on the road)
24A (fitness) ──► 32B step 11 (who can survive the long route)
   32A (one place authority) ──► 32B (travel) ──► 32C (knowledge)
```

**Execution order:** 31A → 32A → 32B → 32C. Wave-4 internal order: **32A is the gate** — do not add
travel features on a 6-node graph with null distances, and do not expand nodes before the ADR says
what a node *is*.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors + new graph tier
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --expedition-selftest            # route cost parity
7. ashfall-dialog-graph-lint (loc_/sector_/zone_ reachability)
8. ashfall-map/tilemap QA: ashfall-tilemap-world-qa for zone/sector authority
9. ashfall-balance-sim (route length × weather × condition sweep)
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 32A | 2 | 1 | **large** (nodes/routes) | 0 | 10–14 | Medium | MEDIUM (save keys on node ids) |
| 32B | 2–3 | 2 | 1 | 2 | 12–16 | **High** | MEDIUM–HIGH (travel math rewrite; mitigate with step 12 parity test) |
| 32C | 1–2 | 2 | 1 | 2 | 8–12 | Medium | LOW |

**Guardrails:** no new map framework, no procedural planet generation, no hex/tile rework (the
Godot `TileMapLayer` QA is a separate concern), no invented `loc_` ids outside the master list, and
no player-visible precision the fiction hasn't earned. If the node set stays small on purpose, say
so in the registry and delete the 261 claim rather than quietly ignoring it.
