# ASHFALL — WAVE 2 INTEGRATION PROGRAM · PLAN 5 OF 6

# LOCATION IMPORTANCE & MAP TRUTH INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W2 (six-plan integration wave)
**Document:** W2-05 · part A of C
**Target size:** ~150,000 characters (this plan)
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W2-01 (maintenance), W2-02 (bugs), W2-03 (gameplay), W2-04 (environments), W2-06 (enrichment)
**Plan-unblocking annex:** Annex U at the end — deliberately separated per the Wave 2 rule.

---

## 0. How to read this plan

This plan gives every place a **reason to matter**: a role, a tier, a readable
danger, a route to reach it, a reason to return, and a consequence for its
loss or discovery. It extends the existing map, location catalogs, regional
systems, and evolution owners. It does not add a second map, a second location
authority, or a parallel regional economy.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Truth & Tiers | location/map data truth + importance tiers + readable roles; no new mechanics |
| **B** | Place & Purpose | tiers + services/function read models + discovery/return loops on existing owners |
| **C** | Living Wasteland | location evolution, regional arcs, and long-term place investment |

**Level 2:** ten points, each A/B/C (Appendix A).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Tiers | 1–10 | — | — |
| B Place & Purpose | 1,4,10 | 2,3,5,6,7,8 | 9 |
| C Living Wasteland | — | 2,3,5,10 | 1,4,6,7,8,9 |

### 0.3 The Wave 2 rule for this plan

> **One location authority.** `locations.json` and its regional catalogs are
> the authored authority; `WastelandMapSystem` and its catalog own the graph;
> regional systems own regional meaning; `LocationEvolutionSystem` owns change
> over time. This plan never forks any of them and never duplicates a location
> row.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| location row | an authored place in a location catalog |
| map node | a graph node in `wasteland_map_v1.json` |
| route | an authored edge with distance/travel meaning |
| tier | importance class (hub, anchor, outpost, site, landmark) |
| role | gameplay function (water, trade, salvage, shelter, story) |
| discovery | the state of a place being known/reachable |
| regional context | the region/price/treaty layer a place belongs to |
| evolution | authored change over campaign days |

---

## 1. Executive summary

The repository has a large but unevenly connected place layer:

- **`locations.json`: 179 locations** with `id`, `displayName`,
  `description`, `dangerLevel`, `travelHours` (171/179), `baseRadsPerHour`
  (178/179), and sparse trigger flags (`requiredFlagId` 1, `cleanWaterRewardFlag`
  1, `ambushFlag` 1).
- **`wasteland_map_v1.json`: 22 nodes, 68 routes, 1 trap site** — a proper graph
  with danger/faction/loot fields, discoverable/startingUnlocked flags, and
  positions.
- Additional location catalogs: `crossing_locations.json` (13),
  `holdfast_locations.json` (38), `year_of_ash_locations.json` (66),
  `micro_locations.json` (28 encounters), `deep_lore_locations.json`,
  `dive_sites.json`, `excavation_sites.json`, `damaged_map_zones.json`.
- Regional systems: `RegionalPriceAtlas`, `RegionalSupplyRouter`,
  `RegionalTreatySystem` (+ catalog/feed), `CaravanTradeNetworkSystem`,
  `BlackMarketSettlementService`.
- Evolution: `LocationEvolutionSystem` (+ `.Live`), `world_evolution_events.json`,
  `world_evolution_seeds.json`, `world_history.json`.
- A sealed gate already exists: `AllMapNodes_ExistInLocationsCatalog`
  (from the Plan 32 debt row) plus `WastelandMapRouteValidationTests`.

The **gap** is not existence; it is **importance and consequence**:

1. 179 locations carry no tier/role/services; a player cannot tell a
   story-critical place from filler (Point 1/2).
2. 22 map nodes vs. 179 locations means most places have no graph identity;
   the relationship between the two authorities is unclear (Point 3/10).
3. `dangerLevel` is authored with **mixed numeric forms** (integers and floats)
   and travel/rads have gaps (Point 4; W2-01 fixes the forms).
4. No canonical statement of what each place **provides** (Point 5).
5. Discovery flags are sparse; unlock progression is thin (Point 6).
6. Regional meaning (prices/treaties/supply) is not attached to place identity
   in a readable way (Point 7).
7. Micro-location encounters, deep lore, dive/excavation sites are rich but
   disconnected from the primary location rows (Point 8).
8. `LocationEvolutionSystem` exists; what changes and why is not surfaced
   (Point 9).
9. The map gate covers node coverage but not role/route/service consistency
   (Point 10).

This plan turns "place" into a first-class, machine-checked concept without
disturbing any existing owner.

---

## 2. Verified current state (location evidence)

### 2.1 `locations.json`

| Field | Coverage |
|---|---|
| `id`, `displayName`, `description` | 179/179 |
| `dangerLevel` | 179/179 — mixed integer/float literal forms |
| `baseRadsPerHour` | 178/179 |
| `travelHours` | 171/179 |
| `requiredFlagId` | 1/179 |
| `cleanWaterRewardFlag` | 1/179 |
| `ambushFlag` | 1/179 |

Danger distribution (verified): `3 (×29), 5 (×22), 4 (×21), 6 (×18), 2 (×17),
7 (×14), 8.0 (×10), 9.0 (×8), 7.0 (×8), 1 (×8), 8 (×7), 6.0 (×7), 10.0 (×4),
5.0 (×3), 4.0 (×2), 0 (×1)`.

Loader: `ExpeditionCatalogLoader` reads `locations.json` among its location
files (verified lines 35–37) and registers `ExpeditionDefinition`s.

### 2.2 `wasteland_map_v1.json`

| Element | Count / example |
|---|---|
| nodes | 22 (e.g., `loc_holdfast`, `loc_cut_abandoned_depot`, `loc_cut_radiation_zone_alpha`, `loc_black_flotilla_outpost`, `loc_cut_merchant_caravanserai`) |
| routes | 68 |
| trap sites | 1 (`snare_perimeter_north` anchored to `loc_holdfast`) |
| node fields | `id`, `displayName`, `danger`, `faction`, `lootTable`, `positionX/Y`, `discoverable`, `startingUnlocked` |

Gate: `AllMapNodes_ExistInLocationsCatalog` (sealed) ensures every map node
exists in the locations catalog — the reverse (every location has a node) is
**not** required and is part of Point 3/10's design work.

### 2.3 Regional systems

| System | Role |
|---|---|
| `RegionalPriceAtlas` + `regional_prices.json` | regional price modifiers |
| `RegionalSupplyRouter` | supply routing between regions |
| `RegionalTreatySystem` + `regional_treaties.json` + feed | treaties between factions/regions |
| `CaravanTradeNetworkSystem` + `caravan_trade_routes.json` | caravan network |
| `BlackMarketSettlementService` | black-market settlement |
| `RegionalTreatyCatalogLoader` | treaty data |

### 2.4 Evolution

| Component | Role |
|---|---|
| `LocationEvolutionSystem` (+ `.Live`) | location change over time |
| `world_evolution_events.json` | authored evolution events |
| `world_evolution_seeds.json` | evolution seeds |
| `world_history.json` | world history data |

### 2.5 Micro/deep surfaces

| Catalog | Count | Role |
|---|---|---|
| `micro_locations.json` | 28 encounters | micro-location encounter surfaces |
| `deep_lore_locations.json` | (verify) | lore sites (Black Projects note says no new locations authored) |
| `dive_sites.json` | (verify) | dive locations |
| `excavation_sites.json` | (verify) | dig sites |
| `crossing_locations.json` | 13 | crossing points |
| `year_of_ash_locations.json` | 66 | storm-era locations |
| `damaged_map_zones.json` | (verify) | zone damage |

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Location tier/role/function authoring and read models.
- Map graph truth and relationship to the location authority.
- Danger/travel/rads truth (in coordination with W2-01's numeric fix).
- Discovery/unlock progression on existing flags and state.
- Regional meaning attached to places (read-side).
- Micro/deep surface integration into primary place identity.
- Evolution surfacing (what changes, why, since when).
- Integrity gates for tier/role/graph consistency.

### 3.2 Non-goals

- New map system, new graph, new regional economy.
- Route/trade DTO contracts (UNBLOCK-02 F13-E/F).
- Environment fields themselves (W2-04 supplies them; this plan consumes).
- Prose/descriptions (W2-06 writes; this plan may name required surfaces).
- Gameplay pacing (W2-03).
- Save schema without a signed line.

### 3.3 Rules

1. `locations.json` remains the canonical place list; tiers/roles are additive
   fields there (or in a companion catalog keyed by location id — decided in
   Point 1).
2. `WastelandMapSystem` remains the graph; no second route model.
3. Regional systems remain read sources; this plan attaches them to places.
4. Discovery uses existing flags/state; a new persisted field needs a signed
   line.
5. Every new consumer names its owner (the repo's standard).
6. Integrity gates extend the existing map gate family.

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Location identity and taxonomy truth | B |
| 2 | Importance tiers and roles | B |
| 3 | Map graph truth and node coverage doctrine | B |
| 4 | Danger, travel, and rads truth | A |
| 5 | Services and functions read model | B |
| 6 | Discovery and unlock progression | B |
| 7 | Regional meaning attachment | B |
| 8 | Micro/deep/lore surface integration | B |
| 9 | Location evolution surfacing | C |
| 10 | Map/spatial integrity gates | B |

### 4.2 Selection sheet

```text
PLAN 5 — LOCATION IMPORTANCE
Plan Path: [ ] A Truth & Tiers  [ ] B Place & Purpose (default)  [ ] C Living Wasteland

01 identity/taxonomy ... [A] [B] [C]   default B
02 tiers/roles ......... [A] [B] [C]   default B
03 map graph ........... [A] [B] [C]   default B
04 danger/travel/rads .. [A] [B] [C]   default A
05 services/functions .. [A] [B] [C]   default B
06 discovery .......... [A] [B] [C]   default B
07 regional meaning .... [A] [B] [C]   default B
08 micro/deep surfaces . [A] [B] [C]   default B
09 evolution .......... [A] [B] [C]   default C
10 integrity gates .... [A] [B] [C]   default B
```

---

## 5. Decision Point 1 — Location identity and taxonomy truth (default B)

### 5.1 The design question

179 rows with rich prose but no machine-checkable **kind**. The taxonomy should
let the game (and gates) answer: is this a hub, a ruin, a resource site, a
story site, a crossing, a water source?

### 5.2 Path A — Prefix/id audit only

- Classify by naming convention (e.g., `loc_cut_*`, `loc_*`) and description
  keywords; report the classification without writing it to data.
- No new fields.

### 5.3 Path B — Authored taxonomy field

- Add an additive `kind` field (closed vocabulary) to location rows (or a
  companion catalog keyed by id — choose by diff size; recommended: additive
  field, default `"site"`, so old rows load unchanged).
- Author `kind` for all rows through a review pass driven by the report.
- Consumers: map filters, tier validation, service checks, UI grouping.
- This is an additive data field; no schema version bump if loaders default
  absent values (verify each loader). If a loader rejects unknown fields, add
  the DTO property (additive).

### 5.4 Path C — Full taxonomy with provenance

Path B, plus authored sub-kinds and provenance (who built it, era) consumed by
W2-06 prose and by region/evolution checks. Data-heavy; not required for
importance.

### 5.5 Acceptance

- Every row has a kind from the closed vocabulary.
- No loader break; absent field defaults.
- A gate validates kind values against the vocabulary.

---

## 6. Decision Point 2 — Importance tiers and roles (default B)

### 6.1 The design question

Tier answers "how important", role answers "what for". Both must be authored
and consumed, or they are decoration.

### 6.2 Tier model (proposal)

| Tier | Meaning | Expected count |
|---|---|---|
| `hub` | major settlement/trade node; the map anchors | 3–6 |
| `anchor` | regional center, story-critical | 8–14 |
| `outpost` | functional place (water, salvage, watch) | 20–40 |
| `site` | standard expedition target | ~80–120 |
| `landmark` | lore/sight, minimal function | ~10–20 |

Tiers are authored as an additive `tier` field; tier consistency rules:
`hub` implies a map node; `anchor` implies a map node or a graph route to one;
`outpost` implies at least one function; `site`/`landmark` free.

### 6.3 Role model (proposal)

Closed roles: `water`, `trade`, `salvage`, `shelter`, `medical`, `power`,
`research`, `story`, `hazard`, `crossing`, `defense`, `ritual`.
A place may have several; a consumer (expedition/loot/service read) uses roles
to answer "what can I get here".

### 6.4 Path A — Report tiers/roles from description

- Derive provisional tiers/roles heuristically; report only.

### 6.5 Path B — Author + consume

- Author tier and roles; wire consumers:
  - expedition selection weights by tier;
  - loot tables by role (existing loot resolution);
  - map display grouping by tier;
  - regional systems read role sets.
- Tests: tier/role consistency rules; consumers read authored fields.

### 6.6 Path C — Dynamic importance

Path B, plus campaign-time importance shifts (a place becomes a hub after an
evolution event) through `LocationEvolutionSystem` — requires a signed
persisted tier override. C only.

### 6.7 Acceptance

- All rows tiered and roled.
- Consistency rules enforced.
- Consumers demonstrably read the fields.
- No second importance field.

---

## 7. Decision Point 3 — Map graph truth and node coverage doctrine (default B)

### 7.1 The design question

22 nodes for 179 places. Which places need graph identity and how do the rest
relate? The sealed gate covers node→catalog only.

### 7.2 Path A — Doctrine document

- State the doctrine: which tier/role combos require a node; everything else is
  reachable through a node (a hub/outpost "serves" nearby sites).
- Report the current alignment; no data change.

### 7.3 Path B — Coverage + service mapping

- Add authored `serves`/`nodeId` association (additive) linking places to graph
  nodes where they are not themselves nodes.
- Extend the gate family: every `hub` is a node; every `anchor` is a node or
  served by one; every place has a route path (direct or via its serving node).
- Route truth: every route references valid nodes with sane distances (extends
  `WastelandMapRouteValidationTests`).

### 7.4 Path C — Graph growth

Path B, plus authored new nodes/routes for the highest-value unserved anchors
(through the map catalog, reviewed), turning the 22-node graph into a regional
network. This is data-heavy and needs W2-04 visibility inputs and W2-06 names;
C only.

### 7.5 Acceptance

- Doctrine documented.
- Every place node-or-served.
- Route validation green.
- No second route model.

---

## 8. Decision Point 4 — Danger, travel, and rads truth (default A)

### 8.1 The design question

`dangerLevel` is mixed-form (W2-01 fixes forms); travel/rads have gaps. This
point makes the numbers meaningful and consistent with tier/role.

### 8.2 Path A — Truth pass

- Consume W2-01's normalization; fill missing `travelHours`/`baseRadsPerHour`
  per the W2-01 dispositions.
- Author consistency expectations: hub/anchor ≤ mid danger; hazard role ≥ high
  danger; crossing role low-mid.
- Report violations; fix data where clear.

### 8.3 Path B — Danger doctrine + gate

- Document what each danger band means (encounter weight, rads band, travel
  risk) and gate authored rows to the doctrine (warn-only first, then enforce).
- UI: a readable danger label derived from the band (Point 5 surface).

### 8.4 Path C — Dynamic danger

Danger varies by campaign state (war front, evolution) through the evolution
owner — signed persistence; C only.

### 8.5 Acceptance

- No missing fields; forms normalized.
- Doctrine documented; band-labelled UI.
- No danger value consumed by two different meanings.

---

## 9. Decision Point 5 — Services and functions read model (default B)

### 9.1 The design question

A place should answer: what can I do here? The read model composes existing
owners (loot resolution, trade network, water sources, medical/rest sites) into
a per-place function list.

### 9.2 Path A — Function inventory

- For each place with a function (from roles), find the existing consumer that
  provides it; list places whose role has no consumer (dead role).

### 9.3 Path B — Place read model

- `PlaceProfile` read model: tier, roles, services with current availability
  (e.g., "trade: available, caravan present day 34"), danger band, rad band,
  travel estimate, discovery state, region.
- Consumed by: map/expedition panels, briefing, W2-03 choice planning.
- All values owner-sourced; no UI math.

### 9.4 Path C — Service investment

Path B, plus player investment in a place (a shelter cache, a repaired bridge,
a radio relay) through existing facility/route owners — signed persistence for
the investment state; C only, and W2-05 coordinates with W2-04's route memory.

### 9.5 Acceptance

- No dead role (every authored role has a consumer or is removed).
- Profile values all read from owners.
- No new service simulation.

---

## 10. Decision Point 6 — Discovery and unlock progression (default B)

### 10.1 The design question

Sparse flags exist. Discovery should be a real progression: rumors → location
known → route known → visitable, with reasons.

### 10.2 Path A — Discovery audit

- Inventory existing discovery/flag state (`discoverable`,
  `startingUnlocked`, `requiredFlagId`, `damaged_map_zones`, world-history
  reveals).
- Report dead flags and unreachable discoveries.

### 10.3 Path B — Discovery progression

- A read model over existing state: discovery stage per place (unknown /
  rumored / located / route-known / visited) derived from flags, visited
  records, and authored requirements.
- Acquisition routes: existing radio/rumor/quest/travel systems grant the
  stages (no new grant authority).
- Map rendering consumes the stage (fog semantics already exist for the graph).
- Tests: stage transitions via existing grants; no place permanently
  undiscoverable if authored discoverable.

### 10.4 Path C — Dynamic discovery arcs

Authored multi-step discoveries (a chain of places revealing each other)
through existing quest/radio owners. Content-shaped; W2-06 writes the prose.

### 10.5 Acceptance

- Every `discoverable` place has at least one acquisition route.
- Stage transitions tested.
- Fog rendering reads the stage.

---

## 11. Decision Point 7 — Regional meaning attachment (default B)

### 11.1 The design question

Regions have prices, supply routes, treaties. Attach them to places readably so
a player understands why the merchant here pays less.

### 11.2 Path A — Region map truth

- Verify each place resolves to a region (or `unassigned`) through
  `RegionalPriceAtlas` semantics; report unassigned places.

### 11.3 Path B — Regional read composition

- `PlaceProfile` includes region id, price posture (from the atlas), treaty
  status (from the treaty system), and supply route presence (from the router).
- Consumers: trade UI, briefing, W2-03 economy pressure tiers (read).
- Tests: every place resolves a region or is explicitly unassigned; prices
  match the atlas for a sampled place.

### 11.4 Path C — Regional arcs

Authored regional change (a treaty collapse changes prices) through existing
world-evolution/treaty owners. W2-06 content; C only.

### 11.5 Acceptance

- Region resolution complete or explicitly unassigned.
- No second price model.
- Regional values owner-sourced in the profile.

---

## 12. Decision Point 8 — Micro/deep/lore surface integration (default B)

### 12.1 The design question

`micro_locations.json` (28 encounters), deep lore sites, dive sites, dig sites,
crossings, and Year-of-Ash locations are rich but live beside the primary rows.
Integrate them by reference, not by copy.

### 12.2 Path A — Surface inventory

- Map each micro/deep/dive/excavation/crossing entry to a primary location id
  (or mark it standalone).
- Report entries with no primary anchor.

### 12.3 Path B — Anchored surfaces

- For each special surface, author an anchor field (location id) where missing
  (additive) or document the standalone relationship.
- The `PlaceProfile` exposes which special surfaces a place hosts, read from
  their owners.
- Gates: anchored ids exist; standalone entries are listed in the doctrine.

### 12.4 Path C — Unified encounter graph

A read-side graph connecting places → micro encounters → consequences for
planning (W2-03 choice density). Still a read; no new narrative authority.

### 12.5 Acceptance

- Every special surface anchored or explicitly standalone.
- Place profile lists hosted surfaces.
- No copied location rows.

---

## 13. Decision Point 9 — Location evolution surfacing (default C)

### 13.1 The design question

`LocationEvolutionSystem` exists; what changes, why, and can the player see it?

### 13.2 Path A — Evolution audit

- Inventory authored evolution events/seeds and their consumers.
- Report changes with no player-visible signal.

### 13.3 Path B — Evolution read model

- The `PlaceProfile` includes: last change day, change kind, and a world-history
  reference (from the existing history data).
- Consumers: map tooltip, journal/chronicle via existing owners.

### 13.4 Path C — Evolution arcs

Path B, plus authored evolution arcs (a place grows, decays, or changes hands)
with tier/role updates through the evolution owner — signed persistence for the
override; W2-06 prose.

### 13.5 Acceptance

- Every evolution event has a signal.
- No second evolution system.
- Tier overrides (C) persisted only per signed line.

---

## 14. Decision Point 10 — Map/spatial integrity gates (default B)

### 14.1 The design question

The sealed gate is node→catalog. Importance adds new invariants: tier/role,
node/service, route validity, special-surface anchors, region resolution.

### 14.2 Path A — Extend the report

- A single script reports all invariants; no enforcement.

### 14.3 Path B — Gate family

Extend `WastelandMapCatalogLoaderTests`/route tests with:

| Invariant | Rule |
|---|---|
| tier-node | hub/anchor are nodes or served |
| role-consumer | every role has a consumer |
| route-valid | every route references valid nodes, positive distance |
| anchor-exists | special surfaces anchor to real ids |
| region-resolution | every place has a region or is allowlisted unassigned |
| danger-band | danger values are within doctrine bands (warn-only first) |

### 14.4 Path C — Spatial consistency report

Path B, plus a generated spatial report (adjacency sanity, coordinate ranges,
route crossings when positions exist) with a `--check` mode.

### 14.5 Acceptance

- All invariants green or allowlisted with reasons.
- Generated report current.
- No hand-edited generated artifacts.

---

## 15. Execution phases

### L0 — Location premise freeze (1 day)

- Verify every count/field/catalog in §2 at HEAD.
- Run the map/integrity tests; produce `P0_LOCATION_PREMISE.md`.

### L1 — Identity and tiers (Points 1 + 2)

- Taxonomy field + tier/role authoring + consistency rules + consumer binds.

### L2 — Graph and coverage (Point 3)

- Doctrine + serves/nodeId + gates.

### L3 — Truth pass (Point 4)

- Consume W2-01 fixes; fill gaps; doctrine + band labels.

### L4 — Services and discovery (Points 5 + 6)

- PlaceProfile read model + discovery stages + acquisition routes.

### L5 — Regional and special surfaces (Points 7 + 8)

- Region composition + anchors + profile exposure.

### L6 — Evolution and gates (Points 9 + 10)

- Evolution surfacing + full invariant gate family.

### L7 — Closeout

- Evidence; ledger proposals; Annex U.

### 15.1 Ordering constraints

- L0 first; L3 consumes W2-01's data fix (coordinate if W2-01 is mid-flight).
- L1 before L2 (tiers define node doctrine).
- L4 before L5 (profile is the composition surface).
- L6 C-items last (persistence lines).

---

## 16. Verification plan

| Point | Evidence |
|---|---|
| 1 | taxonomy present/valid gate; loader compatibility |
| 2 | tier/role authoring complete; rules green; consumers tested |
| 3 | doctrine doc; coverage gate; route validation |
| 4 | no missing fields; doctrine bands; labels |
| 5 | profile tests; no dead roles |
| 6 | stage transitions; acquisition routes; fog reads |
| 7 | region resolution; sampled price match |
| 8 | anchors exist; profile lists surfaces |
| 9 | evolution signals; (C) override persistence |
| 10 | invariant family green |

Commands:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandMapCatalogLoaderTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandMapRouteValidationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
```

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | 179-row authoring becomes a grind | H | M | taxonomy/tier pass is script-assisted; review batches |
| 2 | loader breaks on new fields | M | H | additive defaults; loader tests first |
| 3 | tier/role drift after authoring | M | M | gate family |
| 4 | graph growth overreaches | M | M | doctrine + node gate; growth is C only |
| 5 | route meaning collides with UNBLOCK-02 F13 | M | H | no route DTO work here |
| 6 | evolution overrides need persistence | M | M | signed C line only |
| 7 | duplicate place truth across catalogs | M | H | anchors by reference; no row copies |
| 8 | region resolution gaps | M | L | allowlist unassigned with reasons |
| 9 | W2-01 data fix not landed | M | M | coordinate; do not duplicate the fix |
| 10 | profile exposes fabricated data | L | H | owner-sourced values; purity gates |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| L0 | `W2-05-L0-PREMISE` | premise doc |
| L1 | `W2-05-L1-TAXONOMY-TIERS` | locations data + loader DTOs (additive) + tests |
| L2 | `W2-05-L2-GRAPH` | map catalog doctrine/serves + gates |
| L3 | `W2-05-L3-TRUTH` | locations data (travel/rads gaps) |
| L4 | `W2-05-L4-PROFILE-DISCOVERY` | read model + consumers + tests |
| L5 | `W2-05-L5-REGION-SURFACES` | anchors + profile extension + tests |
| L6 | `W2-05-L6-EVOLUTION-GATES` | evolution surfaces + gate family |
| L7 | `W2-05-L7-CLOSEOUT` | evidence + ledger proposals |

Coordination: W2-01 owns the numeric-form fix; W2-04 supplies environment
fields; W2-06 owns prose; W2-02 owns runtime lookup defects.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | remove taxonomy field | places unclassified |
| 2 | remove tiers/roles | importance flat |
| 3 | keep doctrine, drop serves | node doctrine unenforced |
| 4 | revert data fixes | gaps return |
| 5 | remove profile | services opaque |
| 6 | remove stages | discovery flat |
| 7 | remove composition | regions opaque |
| 8 | remove anchors | surfaces disconnected |
| 9 | keep audit | evolution invisible |
| 10 | keep report | invariants unenforced |

---

## 20. DoD and handoff

**Path A:** taxonomy/tier/role reports and applied truth fixes; doctrine docs;
no consumers.

**Path B:** all of A, plus authored tiers/roles, node/serves coverage, profile
read model, discovery stages, region composition, anchored surfaces, and the
gate family — each tested.

**Path C:** all of B, plus signed dynamic items (importance shifts, graph
growth, dynamic danger, service investment, evolution arcs).

**Handoff:** outcome, files, contract (fields + invariants + profile), commands,
limitations, untouched shared paths, ledger proposals, Annex U.

### 20.1 First safe step

> L0 only: verify the counts/fields and run the map tests. No authoring before
> the premise.

---

*(Part A ends. Part B continues with worked location designs, sample rows, gate
implementations, scenario walks, and Annex U.)*---

# PART B — WORKED LOCATION DESIGNS, GATES, SCENARIOS, ANNEX U

---

## B.1 Taxonomy and tier authoring (Points 1–2, worked shape)

### B.1.1 Closed vocabulary

```json
// companion vocabulary (or documented constants)
{
  "kinds": ["settlement","ruin","industrial","medical","military","research",
            "transport","natural","religious","residential","hazard","crossing"],
  "tiers": ["hub","anchor","outpost","site","landmark"],
  "roles": ["water","trade","salvage","shelter","medical","power","research",
            "story","hazard","crossing","defense","ritual"]
}
```

### B.1.2 Sample authored rows (additive fields only)

```json
{
  "id": "abandoned_hospital",
  "displayName": "Abandoned Hospital",
  "kind": "medical",
  "tier": "anchor",
  "roles": ["medical","salvage","hazard","story"],
  "serves": null,
  "nodeId": null,
  "region": "north",
  "...": "existing fields unchanged"
}
```

```json
{
  "id": "loc_holdfast",
  "displayName": "Holdfast",
  "kind": "settlement",
  "tier": "hub",
  "roles": ["shelter","trade","defense","water"],
  "serves": ["abandoned_hospital","…"],
  "nodeId": "loc_holdfast",
  "region": "holdfast",
  "...": "existing fields unchanged"
}
```

### B.1.3 Authoring workflow (efficient at 179 rows)

1. Script the provisional classification from ids/descriptions (review aid
   only; never committed as data).
2. Review in batches of ~20 rows with the description text visible.
3. Write `kind`/`tier`/`roles` for the batch.
4. Run the consistency report; fix violations in the same batch.
5. Commit per batch with the report attached.

This keeps the authoring diff reviewable and the vocabulary stable.

### B.1.4 Consistency rules (gate-ready)

```text
hub:      nodeId required OR serves is non-empty
anchor:   nodeId required OR served by a node
outpost:  roles ∩ {water,trade,shelter,medical,power,defense,research} ≠ ∅
hazard:   dangerLevel >= 6 OR baseRadsPerHour >= authored threshold
crossing: dangerLevel in [2,6]
trade:    region required
```

---

## B.2 The PlaceProfile read model (Point 5/7/9 composition)

```csharp
public sealed record PlaceProfile(
    string LocationId,
    string DisplayName,
    string Kind,
    string Tier,
    IReadOnlyList<string> Roles,
    string? RegionId,
    DangerBand Danger,
    float BaseRadsPerHour,
    float TravelHours,
    DiscoveryStage Discovery,
    IReadOnlyList<PlaceService> Services,
    EvolutionStamp? LastChange);

public sealed record PlaceService(
    string Role, bool AvailableNow, string? Basis);

public sealed record EvolutionStamp(
    int Day, string Kind, string? HistoryRef);
```

Rules:

- Every field resolves from an owner: catalog row, map node, region atlas,
  discovery read, service owners, evolution owner.
- `AvailableNow` answers from the service owner (e.g., caravan present via the
  trade network's current state), never guessed.
- The profile is recomputed on demand and cached per day; it is not persisted.

### B.2.1 Where the profile is consumed

| Consumer | Uses |
|---|---|
| map tooltip | tier, roles, discovery, danger band |
| expedition board | tier, roles, travel, danger, services |
| trade UI | region, trade posture, treaty status |
| briefing | nearest threats/opportunities by place |
| W2-03 choice planning | tier/role density (read) |
| W2-06 presentation | which surfaces have prose (by role/tier) |

### B.2.2 Purity

No profile value is computed by the UI. If a service's owner is unavailable,
`AvailableNow` is false with `Basis = "unknown until read"` — never a
fabricated default that looks live.

---

## B.3 Map graph doctrine and coverage (Point 3)

### B.3.1 Doctrine text (paste-ready)

> **Node coverage doctrine.** A `hub` is always a graph node. An `anchor` is a
> graph node or is served by exactly one node via the `serves` association.
> Starting from any graph node, every place is reachable by: direct node
> identity, or node identity + one overland leg from its serving node. No
> authored place may be reachable only by a route that does not exist.

### B.3.2 Coverage check implementation sketch

```python
# scripts/ci/location_integrity.py (read-only report + --check)
nodes = load_map()['nodes']; locations = load_locations()
served = {s for loc in locations for s in (loc.get('serves') or [])}
problems = []
for loc in locations:
    tier = loc.get('tier')
    if tier == 'hub' and loc['id'] not in node_ids:
        problems.append((loc['id'], 'hub without node'))
    if tier == 'anchor':
        if loc['id'] not in node_ids and not (loc['id'] in served or loc.get('nodeId') in node_ids):
            problems.append((loc['id'], 'anchor without node or server'))
# route validation
for route in load_map()['routes']:
    if route['from'] not in node_ids or route['to'] not in node_ids:
        problems.append((route.get('id'), 'route references unknown node'))
```

The script also emits the reverse coverage: places per node, nodes per region,
and unserved places by tier.

### B.3.3 Route truth extension

`WastelandMapRouteValidationTests` already checks routes; the extension adds:

- positive distance/time values (no zero-length route unless authored),
- no duplicate route pairs,
- every node reachable from `loc_holdfast` (graph connectivity),
- trap sites anchored to existing nodes.

---

## B.4 Discovery progression (Point 6)

### B.4.1 Stage model

| Stage | Meaning | Typical grants |
|---|---|---|
| unknown | not in the player's knowledge | — |
| rumored | name/area hinted | radio, rumor, briefings |
| located | position known, route not | travel/quest/discovery |
| route-known | reachable on the map | exploration, caravan, scout |
| visited | been there | actual visit record |

Stages derive from existing state: `discoverable`, `startingUnlocked`,
`requiredFlagId`, visited records (the plan verifies the existing visited
mechanism, e.g., `PlayerVisitedTrigger` references in the war-clock debt row),
and radio/quest grants.

### B.4.2 Stage computation sketch

```csharp
public static DiscoveryStage StageFor(
    LocationRow row, bool visited, IReadOnlySet<string> flags,
    IReadOnlySet<string> nodeVisible)
{
    if (visited) return DiscoveryStage.Visited;
    if (row.nodeId is not null && nodeVisible.Contains(row.nodeId))
        return DiscoveryStage.RouteKnown;
    if (row.requiredFlagId is not null && flags.Contains(row.requiredFlagId))
        return DiscoveryStage.Located;
    if (row.startingUnlocked) return DiscoveryStage.RouteKnown;
    return row.discoverable ? DiscoveryStage.Rumored : DiscoveryStage.Unknown;
}
```

This is a read over existing state; new persisted discovery fields would need a
signed line (not expected).

### B.4.3 Acquisition audit

A gate asserts: every `discoverable` place has at least one acquisition route
(a flag grant, a rumored source, or a node connection) — otherwise it is
permanently invisible.

---

## B.5 Regional composition (Point 7)

### B.5.1 Region resolution

```csharp
string RegionFor(string locationId)
    => _regionMap.TryGetValue(locationId, out var r) ? r : RegionIds.Unassigned;
```

The map is authored (`region` field) or derived from node/regional price atlas
semantics after verification. Unassigned places are allowlisted with reasons.

### B.5.2 Trade posture

```csharp
TradePosture PostureFor(string locationId)
{
    var region = RegionFor(locationId);
    var price = _priceAtlas.ModifierFor(region);     // existing owner
    var treaty = _treaties.StatusFor(region);        // existing owner
    var supplied = _supplyRouter.HasRoute(region);   // existing owner
    return new TradePosture(region, price, treaty, supplied);
}
```

### B.5.3 Tests

- Sampled places' prices equal the atlas.
- No place resolves two regions.
- Unassigned list matches the allowlist.

---

## B.6 Special surface anchoring (Point 8)

### B.6.1 Anchor matrix (target)

| Surface | File | Anchor field |
|---|---|---|
| micro encounter | `micro_locations.json` | `location_id` (add if missing) |
| deep lore | `deep_lore_locations.json` | existing site id → location |
| dive site | `dive_sites.json` | `location_id` |
| dig site | `excavation_sites.json` | `location_id` |
| crossing | `crossing_locations.json` | crossing id ↔ node |
| Year-of-Ash | `year_of_ash_locations.json` | location id |

Where a surface is intentionally standalone, it is listed in the doctrine with
a reason (e.g., an off-map wreck).

### B.6.2 Profile exposure

`PlaceProfile.Services` may include special surfaces as service entries with
their owner's availability, so a player at a place can see "dive site: tidal
window open".

---

## B.7 Evolution surfacing (Point 9)

### B.7.1 Stamp read

```csharp
public EvolutionStamp? LastChangeFor(string locationId)
{
    var evt = _evolution.LastEventFor(locationId);   // existing owner
    return evt is null ? null
        : new EvolutionStamp(evt.Day, evt.Kind, evt.HistoryRef);
}
```

### B.7.2 UI rule

If a place changed, the tooltip says what and when, and links (by reference) to
the world history entry. No new history store.

### B.7.3 Dynamic tier (C)

If signed: `LocationEvolutionSystem` may raise/lower tier via an override field
in its own state; the profile prefers the override when present. One signed
line; default off.

---

## B.8 Integrity gate family (Point 10)

### B.8.1 Test file plan

| Test | Assertion |
|---|---|
| `LocationTaxonomyGateTests` | every `kind` ∈ vocabulary |
| `LocationTierGateTests` | consistency rules (§B.1.4) |
| `LocationRoleConsumerGateTests` | every role has a consumer |
| `MapServesCoverageTests` | doctrine coverage |
| `MapRouteExtrasTests` | positive distance, no dupes, connectivity |
| `SpecialSurfaceAnchorTests` | anchors exist or allowlisted |
| `RegionResolutionTests` | region or allowlisted unassigned |
| `DangerBandGateTests` | bands within doctrine (warn-only first) |

### B.8.2 Negative demonstrations

For each new gate: introduce one deliberate violation, observe failure, revert.
The gate family's value is exactly this bite.

### B.8.3 Generated report

`docs/locations/LOCATION_INTEGRITY_REPORT.md` generated by the script with
`--check`; contains counts, violations, and the unassigned/allowlist tables.

---

## B.9 Scenario walks

### B.9.1 Scenario — Path A, one week

1. L0 premise + integrity report (2 days).
2. L1-A taxonomy report + apply `kind` only where unambiguous (1 day).
3. L3-A truth pass consuming W2-01's fix (1 day).
4. L7 closeout (0.5 day).
Outcome: places classified, numbers true, gaps reported.

### B.9.2 Scenario — Path B, one month

1. L0 (1 day).
2. L1: author kind/tier/roles in ~9 batches + consistency gates (6 days).
3. L2: doctrine + serves/nodeId + coverage gates (3 days).
4. L3: truth pass + band labels (2 days).
5. L4: PlaceProfile + discovery stages + consumers (5 days).
6. L5: region composition + anchors (4 days).
7. L6: evolution surfacing + gate family completion (3 days).
8. L7 (1 day).
Outcome: every place has a reason to matter, a way to be reached, a readable
profile, and machine-checked invariants.

### B.9.3 Scenario — Path C, a season

Path B, plus the signed dynamic items: tier shifts, graph growth, dynamic
danger, service investment, regional/evolution arcs. Each with its own claim,
persistence line (if any), and soak/report.

### B.9.4 Scenario — the authoring pass finds a contradiction

Example: a `hub` with danger 9 and no water. Resolution order: (1) check whether
the tier is wrong (likely), (2) check whether the role set is wrong, (3) only
then consider whether the doctrine band needs an authored exception with a
reason. Data fixes are preferred over doctrine changes.

### B.9.5 Scenario — a special surface cannot be anchored

An off-map wreck with no primary row: the doctrine lists it as standalone with
a reason; the anchor gate allowlists it. Never create a fake location row to
silence the gate (that would duplicate authority).

---

## B.10 Foreman Q&A

**Q1. Do tiers change gameplay immediately?**
Yes, through consumers: expedition weights, map grouping, loot by role, trade
posture. Each consumer is a bound existing system.

**Q2. Will 179 authored rows be maintainable?**
The vocabulary and gates keep them consistent; batches keep reviews small; the
report makes drift visible.

**Q3. Why not make every location a map node?**
The graph is a travel network; 179 nodes would be noise. Node doctrine gives
every place a path without bloating the graph.

**Q4. Does the plan create route DTOs?**
No. Route meaning and trade routes stay with UNBLOCK-02 F13 and the existing
map. This plan validates existing routes.

**Q5. Where do descriptions come from?**
W2-06 owns prose. This plan consumes existing descriptions and names surfaces
needing prose.

**Q6. Is discovery a new system?**
No: stages derive from existing flags/visited/node visibility. Only a signed
line could add persisted discovery state (not expected).

**Q7. What if regional data has gaps?**
Unassigned is allowlisted with reasons; the profile says "unassigned", which is
honest.

**Q8. How does this help W2-03?**
The profile supplies choice/opportunity density inputs ("four reachable trade
places, two water sources") without gameplay authority.

**Q9. Smallest approval?**
L0 + the integrity report: a complete picture of what places are and where the
gaps are.

**Q10. Biggest?**
Path C with graph growth and evolution arcs — content-heavy and signed
separately.

**Q11. Does this conflict with the sealed Plan 32 map-orphan debt?**
No: that gate stays; this plan extends the family around it and never removes
it.

**Q12. What proves importance is real?**
The consumers test table: each tier/role is read by at least one live path, and
the gate family keeps the data honest.

---

## B.11 Annex U — Plan-unblocking (separately)

> **Wave 2 rule:** release implications are kept apart from the design body.

### U.1 What W2-05 releases

| Blocked item | Release mechanism | Gate |
|---|---|---|
| Expansion 12 (Second Generation) | Place roles include settlement/ritual anchors for rites and schools | L1 |
| Expansion 13 (Faithful) | `ritual` role + landmarks give pilgrimage targets | L1/L5 |
| Expansion 14 (Above the Ash) | Map/regional truth for sky-adjacent sites | L2/L5 |
| Expansion 15 (Deep Root) | `water`/`natural` roles identify ground sites | L1 |
| Expansion 18 (Underneath) | Graph + danger doctrine for underground sites | L2/L3 |
| Expansion 20 (Quiet Hand) | Discovery stages support informant/dead-drop placement | L4 |
| Expansion 24/25/26 | Region/settlement roles support care/rail/food arcs | L5 |
| W2-03 choice density | Place profile supplies opportunities | L4 |
| W2-04 route meaning | Visibility/gate inputs bind to places | L2/L5 |
| W2-06 prose | Role/tier surfaces name where prose is needed | L1/L4 |
| Plan 32 residual debt | Gate family extends the sealed map gate | L6 |

### U.2 Signatures needed

```text
[ ] I authorize L0 premise + integrity report.
[ ] I authorize Point 1 taxonomy field (additive) + authoring batches.
[ ] I authorize Point 2 tier/role authoring + consistency gates.
[ ] I authorize Point 3 node doctrine + serves/nodeId fields + coverage gates.
[ ] I authorize Point 4 truth pass consuming W2-01's numeric fix.
[ ] I authorize Point 5 PlaceProfile read model + consumers.
[ ] I authorize Point 6 discovery stages over existing state.
[ ] I authorize Point 7 region composition.
[ ] I authorize Point 8 special-surface anchors (additive fields).
[ ] I authorize Point 9 evolution surfacing (+ dynamic tier: [ ] no [ ] signed).
[ ] I authorize Point 10 integrity gate family.
[ ] I authorize Path C growth/arcs separately (list each).
```

### U.3 What W2-05 never touches for unblocking

- Route/trade DTOs and migration (UNBLOCK-02 F13-E/F).
- Environment mechanics (W2-04).
- Needs/economy tuning (W2-03).
- Prose (W2-06).
- Save schema without a signed line.
- The sealed Plan 32 gate (extended, never removed).

### U.4 The importance-release rule

A place "matters" only when a live consumer reads its tier/role and a player can
observe the consequence. Authored fields without consumers do not release an
expansion — the read model and its surfaces are the gate.

---

## B.12 Appendices

### B.12.1 Selection sheet

```text
ASHFALL WAVE 2 · PLAN 5 (LOCATION IMPORTANCE) · SELECTION
Date: ______  Foreman: ______  HEAD: ______

PLAN PATH: [ ] A Truth & Tiers  [ ] B Place & Purpose (default)  [ ] C Living Wasteland

01 identity/taxonomy ... [A] [B] [C]   default B
02 tiers/roles ......... [A] [B] [C]   default B
03 map graph ........... [A] [B] [C]   default B
04 danger/travel/rads .. [A] [B] [C]   default A
05 services/functions .. [A] [B] [C]   default B
06 discovery .......... [A] [B] [C]   default B
07 regional meaning .... [A] [B] [C]   default B
08 micro/deep surfaces . [A] [B] [C]   default B
09 evolution .......... [A] [B] [C]   default C
10 integrity gates .... [A] [B] [C]   default B

Signature: ________________
```

### B.12.2 Vocabulary reference (closed sets)

```text
kind:  settlement | ruin | industrial | medical | military | research |
       transport | natural | religious | residential | hazard | crossing
tier:  hub | anchor | outpost | site | landmark
role:  water | trade | salvage | shelter | medical | power | research |
       story | hazard | crossing | defense | ritual
stage: unknown | rumored | located | route-known | visited
```

### B.12.3 Authoring record template

```markdown
### Batch <n> (ids <first>..<last>)
- kind assigned: <counts>
- tier assigned: <counts>
- roles assigned: <counts>
- consistency violations found/fixed: <n>
- report: <path>
```

### B.12.4 What "done" looks like (default Path B)

| Point | Done when |
|---|---|
| 1 | every row has a valid kind |
| 2 | every row tiered/roled; rules green; consumers tested |
| 3 | doctrine live; every place node-or-served; routes valid |
| 4 | fields complete; bands labelled; doctrine documented |
| 5 | profile live with owner-sourced values |
| 6 | stages derived; acquisition audit green |
| 7 | region resolution complete or allowlisted |
| 8 | anchors resolve; standalone list documented |
| 9 | evolution changes surfaced |
| 10 | gate family green with negative demonstrations |

### B.12.5 Glossary

| Term | Meaning |
|---|---|
| tier | importance class |
| role | gameplay function |
| serves | places a node represents |
| discovery stage | player knowledge level of a place |
| profile | read model of a place |
| anchor | association from a special surface to a place |
| doctrine | authored consistency rules |
| allowlist | explicit exception list with reasons |

---

## B.13 Final statement for W2-05

ASHFALL already has the places; this plan gives them **meaning and
consequence**. Tiers and roles make importance machine-checkable; the node
doctrine gives every place a path without bloating the graph; the profile
answers "what is here, is it safe, can I reach it, why does it matter"; the
gate family keeps the data honest forever.

Recommended: **Plan Path B** with Point 4 at Path A. Start with L0 — the
premise and the integrity report — because the authoring pass needs the map of
what already exists.

---

**End of W2-05.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

*Document control: W2-05 · Wave 2 · HEAD 5be1a30a · companion to W2-01/02/03/04/06.*---

# PART C — AUTHORING PLAYBOOK AND PER-POINT CHECKLISTS

This part is the execution companion to W2-05: batch procedures, checklists,
gate commands, and worked examples for each of the ten points.

---

## C.1 The location authoring playbook

### C.1.1 Batch procedure (repeatable for all 179 rows)

```text
Batch N (ids A..B):
1. Export the batch: id, displayName, description (first 200 chars), existing
   fields, map-node membership.
2. Provisional classify (script): kind/tier/roles guesses from keywords.
3. Human review: read every description; correct the guesses.
4. Write the additive fields: kind, tier, roles, serves (if applicable),
   nodeId (if applicable), region.
5. Run the integrity report; fix violations in this batch.
6. Commit the batch with the report attached.
```

Batch size 15–20 rows keeps review under an hour and diffs reviewable.

### C.1.2 Classification heuristics (script aid only)

| Signal | Suggested kind | Suggested tier |
|---|---|---|
| "settlement", "hub", "trading post" | settlement | hub/anchor |
| "hospital", "clinic", "ward" | medical | anchor/outpost |
| "plant", "factory", "refinery", "kiln" | industrial | outpost/site |
| "depot", "station", "yard" | transport | outpost/site |
| "bunker", "armory", "checkpoint" | military | site/landmark |
| "lab", "research", "archive" | research | anchor/site |
| "forest", "quarry", "spring" | natural | site |
| "church", "shrine", "cenotaph" | religious | landmark |
| "bridge", "crossing", "ford" | crossing | outpost |
| radiation/hazard language | hazard | site/landmark |

The script never writes these guesses to data; they exist to speed review.

### C.1.3 The three-question test per row

1. **What is this place for?** (roles) — if the answer is "nothing", the tier
   is `landmark` and the role set is `story` or `hazard`.
2. **Who can reach it?** (node/serves/route) — if no path, the doctrine needs
   a serving node.
3. **Why return?** (service/evolving reason) — if never, `landmark` with a lore
   reason; otherwise a service role must exist.

Rows failing question 2 block the batch until a serving node or an
allowlist reason is authored.

---

## C.2 Per-point checklists

### C.2.1 Point 1 — taxonomy

```text
[ ] Vocabulary frozen (kinds listed in one place)
[ ] Additive field added to DTO/loader with default
[ ] Loader test proves absent field loads
[ ] Batches authored; report per batch
[ ] Gate: every kind in vocabulary
[ ] Negative test: invalid kind fails
```

### C.2.2 Point 2 — tiers/roles

```text
[ ] Tier vocabulary frozen; rules documented
[ ] Roles frozen; every role has a consumer
[ ] Batches authored (tiers + roles)
[ ] Consistency gate green
[ ] Consumers tested: expedition weight, map grouping, loot-by-role
[ ] Negative test: hub without node fails; role without consumer fails
```

### C.2.3 Point 3 — graph

```text
[ ] Doctrine published
[ ] serves/nodeId fields authored where required
[ ] Coverage report: every place node-or-served
[ ] Route extras: positive distance, no dupes, connectivity from holdfast
[ ] Trap sites anchored to nodes
[ ] Negative tests for each route invariant
```

### C.2.4 Point 4 — truth

```text
[ ] W2-01 numeric fix consumed (forms canonical)
[ ] travelHours/baseRadsPerHour gaps closed with dispositions
[ ] Danger doctrine bands documented
[ ] Band labels derived (no UI math)
[ ] Consistency expectations checked (hub low danger, hazard high)
[ ] Warn-only gate staged
```

### C.2.5 Point 5 — profile

```text
[ ] PlaceProfile record defined (owner-sourced fields only)
[ ] Services resolve from owners (availability + basis)
[ ] Profile cached per day (not persisted)
[ ] Consumers wired: map tooltip, expedition board, trade UI, briefing
[ ] Purity gates green
[ ] No dead role
```

### C.2.6 Point 6 — discovery

```text
[ ] Stage computation from existing state
[ ] Acquisition routes audited (every discoverable has one)
[ ] Map/fog reads the stage
[ ] Transition tests via existing grants
[ ] No persisted stage field without a signed line
```

### C.2.7 Point 7 — regions

```text
[ ] Region resolution map authored/verified
[ ] Unassigned allowlist with reasons
[ ] Trade posture composed from atlas/treaties/router
[ ] Sampled price equality test
[ ] No second price model
```

### C.2.8 Point 8 — surfaces

```text
[ ] Anchor fields added where missing (additive)
[ ] Every special surface anchored or allowlisted standalone
[ ] Profile lists hosted surfaces with availability
[ ] No duplicated location rows
[ ] Negative test: broken anchor fails
```

### C.2.9 Point 9 — evolution

```text
[ ] Evolution inventory (events/seeds/consumers)
[ ] Stamp read surfaced on the profile/tooltip
[ ] History reference by id, not copy
[ ] (C) dynamic tier override: signed? field added to evolution state only
[ ] Signal test for each event kind
```

### C.2.10 Point 10 — gates

```text
[ ] Integrity script report + --check
[ ] Test family added (taxonomy/tier/role/coverage/route/anchor/region/band)
[ ] Negative demonstration per gate
[ ] Generated report current
[ ] No hand edits to generated docs
```

---

## C.3 Worked gate implementation (Python report + C# tests)

### C.3.1 The report script (detailed)

```python
#!/usr/bin/env python3
"""scripts/ci/location_integrity.py — report + --check for location invariants."""
import json, sys, collections

DATA = 'Assets/StreamingAssets/Data'
MAP = json.load(open(f'{DATA}/wasteland_map_v1.json'))
LOCS = json.load(open(f'{DATA}/locations.json'))['locations']

NODE_IDS = {n['id'] for n in MAP['nodes']}
SERVED = set()
for loc in LOCS:
    SERVED.update(loc.get('serves') or [])

problems = collections.defaultdict(list)

ALLOWLIST = json.load(open('docs/locations/integrity_allowlist.json')) \
    if __import__('os').path.exists('docs/locations/integrity_allowlist.json') \
    else {'unassigned_regions': [], 'standalone_surfaces': []}

for loc in LOCS:
    lid, tier = loc['id'], loc.get('tier')
    if tier == 'hub' and lid not in NODE_IDS:
        problems['hub_without_node'].append(lid)
    if tier == 'anchor' and lid not in NODE_IDS and lid not in SERVED \
       and loc.get('nodeId') not in NODE_IDS:
        problems['anchor_without_server'].append(lid)
    if loc.get('kind') is None:
        problems['missing_kind'].append(lid)
    if not loc.get('region') and lid not in ALLOWLIST['unassigned_regions']:
        problems['region_unresolved'].append(lid)

for route in MAP.get('routes', []):
    if route.get('from') not in NODE_IDS or route.get('to') not in NODE_IDS:
        problems['route_unknown_node'].append(route.get('id') or f"{route.get('from')}->{route.get('to')}")
    if (route.get('distance') or 0) <= 0:
        problems['route_nonpositive_distance'].append(route.get('id'))

total = sum(len(v) for v in problems.values())
print(f'locations={len(LOCS)} nodes={len(NODE_IDS)} problems={total}')
for k, v in sorted(problems.items()):
    print(f'  {k}: {len(v)}')
    for item in v[:10]:
        print(f'    - {item}')

if '--check' in sys.argv and total:
    sys.exit(1)
```

### C.3.2 The C# test family

```csharp
public sealed class LocationTaxonomyGateTests
{
    [Fact]
    public void EveryLocation_HasKnownKind()
    {
        var kinds = LocationVocabulary.Kinds;
        foreach (var loc in LocationCatalog.LoadAll())
            Assert.Contains(loc.Kind, kinds);
    }
}

public sealed class MapServesCoverageTests
{
    [Fact]
    public void EveryHub_IsANode() { /* doctrine rule */ }

    [Fact]
    public void EveryAnchor_IsNodeOrServed() { /* doctrine rule */ }

    [Fact]
    public void EveryNode_ReachableFromHoldfast()
    {
        var reach = MapGraph.ReachableFrom("loc_holdfast");
        foreach (var node in MapGraph.Nodes)
            Assert.Contains(node.Id, reach);
    }
}
```

The C# tests and the Python report must agree; the report is for humans and CI,
the tests for the suite.

---

## C.4 Extended scenarios

### C.4.1 Scenario — a hub is expected but no node exists

If a `hub` place has no node, the resolution is one of:

1. Promote an existing nearby node to represent it (rename/retitle; data edit),
2. Add a node (Path C growth),
3. Downgrade the place to `anchor` with a server.

Never leave the hub-node rule broken; the gate exists to force the choice.

### C.4.2 Scenario — a description contradicts the chosen tier

The description is lore (W2-06); the tier is gameplay. If they disagree, the
gameplay classification wins and the description gets an enrichment note for
W2-06 — never rewrite lore to fit a gate.

### C.4.3 Scenario — two regions claim a place

Region resolution must be single-valued. If two systems disagree, the authored
`region` field is the tiebreak; the other system is corrected or its input
documented. No place has two regions in the profile.

### C.4.4 Scenario — a batch reveals a missing consumer

If a role's consumer does not exist (e.g., `ritual` has no surface), the role
is either removed from the vocabulary or a consumer is wired through an
existing owner in the same tranche. A role with no consumer is the exact "dead
data" class W2-06 fights.

---

## C.5 Extended per-point option guidance

### C.5.1 When Path A is right

- Data truth is unknown or disputed.
- The campaign is mid-flight and large diffs are unsafe.
- You need evidence to justify B/C.

### C.5.2 When Path B is right

- The authority is stable and the consumers exist.
- You can afford the gate family and the profile read model.
- You want importance to be machine-checked forever.

### C.5.3 When Path C is right

- The game is stable enough for data-heavy graph growth.
- Persistence lines can be signed for dynamic fields.
- Content capacity (W2-06) exists for the prose those places need.

### C.5.4 Mixing guidance

The safest powerful mix: Plan Path B, Point 4 at A (truth only), Point 9 at A
(evolution audit), everything else B. The riskiest mix: Point 3 at C with
Point 10 at A — graph growth without gates.

---

## C.6 The location knowledge base

### C.6.1 Doctrine statements (canonical, paste-ready)

1. A place exists once, in one catalog.
2. A place's importance is authored (`tier`), not inferred at runtime.
3. A place's function is authored (`roles`) and consumed.
4. A place is reachable by node identity or a serving node.
5. Discovery is a stage over existing state, not a new store.
6. A region is one value per place.
7. Special surfaces reference places; they never duplicate them.
8. Integrity is checked by a gate family, not by review.

### C.6.2 Anti-patterns

| Anti-pattern | Why it fails |
|---|---|
| inferring tier from danger at runtime | two places with same danger, different importance |
| a parallel "points of interest" list | duplicates authority |
| copying location rows into regional catalogs | drift |
| UI computing travel/danger | fabricated values |
| adding nodes for every place | graph noise |
| persisting discovery in a new store | schema creep |
| deleting a place to satisfy a gate | history loss |

### C.6.3 The one-page summary

- **What:** place importance made real: taxonomy, tiers, roles, graph doctrine,
  truth, profile, discovery, regions, surfaces, evolution, gates.
- **Choose:** Plan Path (default B) + ten point paths.
- **Recommended first:** L0 premise + integrity report.
- **Smallest:** L0 + taxonomy report.
- **Largest:** Path C graph growth + evolution arcs (signed separately).
- **Never:** a second map/location/region authority, route DTO work, prose.
- **Unblocking:** Annex U, signed, separate.

---

## C.7 Final checklist (tear-out)

```text
[ ] L0 premise + report attached
[ ] Vocabulary frozen (kind/tier/role/stage)
[ ] Batches authored with reports
[ ] Doctrine published
[ ] Profile live, owner-sourced
[ ] Discovery stages derived
[ ] Region resolution complete
[ ] Anchors resolve
[ ] Evolution surfaced
[ ] Gate family green + negative demonstrations
[ ] Annex U signatures recorded
```

**End of W2-05 authoring playbook.**

*Document control: W2-05 · Wave 2 · HEAD 5be1a30a · proposal only.*---

# PART D — LOCATION VALUE DESIGN AND REGIONAL STRUCTURE

This part turns the importance concepts into authored value: what each tier and
role *means* for play, how regions are structured, and how places earn return
visits.

---

## D.1 Tier value design

### D.1.1 hub — the anchors

| Property | Value |
|---|---|
| count target | 3–6 |
| graph | always a node |
| services | at least shelter + trade + water + defense |
| danger | low–mid (≤5) |
| discovery | known early or via briefings |
| function | the player's mental map anchors; safe-ish return points |
| gameplay weight | high: expedition basing, trade, story convergence |

A hub that cannot provide shelter/trade/water is mis-tiered (gate rule).

### D.1.2 anchor — regional centers

| Property | Value |
|---|---|
| count target | 8–14 |
| graph | node or served |
| services | at least one major (medical, power, research, story) |
| danger | mid (4–7) |
| function | chapter objectives, faction contact, unique services |
| gameplay weight | high per-chapter destinations |

### D.1.3 outpost — functional places

| Property | Value |
|---|---|
| count target | 20–40 |
| graph | served by a node |
| services | one function (water, salvage, watch, cache) |
| danger | mid |
| function | the working geography: where you go for a specific need |
| gameplay weight | medium: repeatable short trips |

### D.1.4 site — standard targets

| Property | Value |
|---|---|
| count target | ~80–120 |
| graph | served |
| services | salvage/loot or minor story |
| danger | authored 1–10 |
| function | expedition texture; discovery rewards |
| gameplay weight | low–medium each, high in aggregate |

### D.1.5 landmark — lore/sight

| Property | Value |
|---|---|
| count target | 10–20 |
| graph | served |
| services | none (or a single authored ritual/story moment) |
| danger | variable |
| function | world identity, echo/atmosphere hosts |
| gameplay weight | low mechanically, high atmospherically |

---

## D.2 Role value design

Each role answers "why come here" and "what do I get":

| Role | Player value | Consumer |
|---|---|---|
| water | resupply | water/exposure owners |
| trade | goods, prices | market/regional systems |
| salvage | materials | loot resolution |
| shelter | rest, warmth, safety | shelter/camp owners |
| medical | healing, supplies | medical owners |
| power | fuel/energy | grid owners |
| research | knowledge, unlocks | research/archive owners |
| story | narrative progression | narrative/quest owners |
| hazard | danger/reward tradeoff | expedition risk |
| crossing | route access | map graph |
| defense | military value | sky/war owners |
| ritual | meaning, morale | spiritual/memorial owners |

A role's value must be observable: if the player cannot tell what a `research`
place offers, the profile surface has failed.

---

## D.3 Regional structure design

### D.3.1 Region concept

Regions group places for economy (prices), politics (treaties), and travel
(route networks). The plan does not invent new region semantics; it attaches
existing ones to places.

### D.3.2 Proposed region resolution table (shape)

```json
{
  "schema_version": 1,
  "regions": [
    { "id": "holdfast",  "display_name": "The Holdfast Reach",
      "node_ids": ["loc_holdfast"],
      "price_modifiers": { "food": 1.0, "medicine": 1.1, "parts": 0.95 },
      "treaty_status": "player" },
    { "id": "north",     "display_name": "The Northern Cut",
      "node_ids": ["loc_cut_abandoned_depot","loc_cut_radiation_zone_alpha"],
      "price_modifiers": { "food": 1.2, "medicine": 1.4, "parts": 1.0 },
      "treaty_status": "neutral" }
  ]
}
```

This is a **read-side composition** over the existing atlas; if the atlas
already encodes this, the plan reuses it rather than duplicating (Rule 5).

### D.3.3 Region value to the player

- Prices differ visibly (trade arbitrage within existing economy).
- Treaties affect passage/safety.
- Supply routes affect availability.
- Regional danger has a character (rad north, war east).

### D.3.4 Regional danger character (authored table)

| Region | Danger band | Dominant hazard | Player expectation |
|---|---|---|---|
| holdfast | low | none | safe base |
| north cut | mid–high | radiation | dosimeters, short trips |
| east | high | war/faction | avoid or time windows |
| south | mid | environmental | weather planning |
| west | variable | subterranean | preparation |

---

## D.4 Return-visit design

A place earns a return when at least one of these is true:

1. **Recurring need**: water/salvage/trade (outposts).
2. **Evolution**: the place changes (Point 9) — new danger or opportunity.
3. **Progression**: a service unlocks after a flag/quest (discovery stages).
4. **Story**: a chapter returns the player there.
5. **Economy**: prices shift by region/season.

The authoring pass records, per place, which reason applies — the "why return"
column. Landmarks may answer "none mechanically" (their value is identity),
which is fine when authored deliberately.

---

## D.5 Worked examples

### D.5.1 Example — a hub row

```json
{
  "id": "loc_holdfast",
  "kind": "settlement",
  "tier": "hub",
  "roles": ["shelter","trade","defense","water"],
  "nodeId": "loc_holdfast",
  "region": "holdfast",
  "dangerLevel": 1,
  "travelHours": 0,
  "baseRadsPerHour": 0.1,
  "whyReturn": ["recurring:base","story:convergence"]
}
```

### D.5.2 Example — an outpost row

```json
{
  "id": "loc_cut_abandoned_depot",
  "kind": "transport",
  "tier": "outpost",
  "roles": ["salvage","crossing"],
  "nodeId": "loc_cut_abandoned_depot",
  "region": "north",
  "dangerLevel": 2,
  "travelHours": 6,
  "baseRadsPerHour": 1.5,
  "whyReturn": ["recurring:salvage","economy:parts"]
}
```

### D.5.3 Example — a landmark row

```json
{
  "id": "abandoned_hospital",
  "kind": "medical",
  "tier": "anchor",
  "roles": ["medical","salvage","hazard","story"],
  "nodeId": null,
  "serves": [],
  "region": "north",
  "dangerLevel": 7,
  "travelHours": 10,
  "baseRadsPerHour": 4.0,
  "whyReturn": ["story:pharmacy","progression:unlock"]
}
```

(An anchor without a node must be served — the example would need a serving
node authored or the tier adjusted; this is exactly what the gate catches.)

---

## D.6 Regional economy interplay

| Interaction | Rule |
|---|---|
| price atlas | regions read existing atlas modifiers; no new price model |
| supply router | routes read the router's current availability |
| treaties | treaty status from the treaty system |
| caravans | caravan presence from the trade network owner |
| black market | black-market settlement reads region/heat |

The location layer composes; it never trades.

---

## D.7 Content hooks for W2-06

| Surface | Value |
|---|---|
| map tooltip | name, tier, roles, danger band, discovery, region |
| expedition board | target roles, travel, danger, last change |
| location arrival | approach atmosphere text (W2-04/W2-06) |
| journal | first visit/return entries per place |
| radio | regional news referencing places |
| archive | place histories for discovered anchors/landmarks |

Each hook names the value it reads; W2-06 authors the words.

---

## D.8 Anti-goals restated

- No place rating number for the player ("importance 7/10") — tier is an
  authoring concept, surfaced indirectly through services and presentation.
- No second danger model.
- No place ownership simulation.
- No grind loops invented to justify a place.
- No graph growth without gates.

---

## D.9 The location value one-pager

- **Tiers:** hub/anchor/outpost/site/landmark with hard rules.
- **Roles:** twelve functions, each with a consumer.
- **Regions:** composed from existing atlas/treaty/router.
- **Return:** one authored reason per place.
- **Surface:** the profile is the player's answer to "what is here, why
  matter".
- **Gate:** the integrity family keeps it honest.

**End of the location value design.**

*Document control: W2-05 · Wave 2 · HEAD 5be1a30a · proposal only.*