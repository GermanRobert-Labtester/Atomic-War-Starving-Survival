# Plan 16 — Cartography & Infrastructure: The Map, Waystations & Treaty Web

> **Theme:** The *physical* world structure. The live wasteland map is nearly empty of authored
> geography, the waystation/caravan network is abstract, and regional treaties are minimal.
> This plan builds the map players actually navigate.
>
> **Key evidence (verified):** `wasteland_map_v1.json` = only **6 nodes / 7 routes** against
> 115 locations and 261 claimed nodes elsewhere; `damaged_map_zones.json` = **3 zones**;
> `currents.json` = 17; `foundry_accords.json` = **4 treaties**; `RegionalTreatySystem.cs`,
> `WaystationSystem.cs`, `TravelingCaravanSystem.cs`, `WastelandMapSystem.cs` all live.

---

## Task 16A — Wasteland map node & route densification (6 → 60 nodes)

**Goal:** Replace the skeletal 6-node map with a real navigable graph whose nodes resolve to
actual locations and whose routes carry risk, distance, and terrain.

**Files:** `wasteland_map_v1.json` (primary), `locations.json` (node↔location refs),
read-only `WastelandMapSystem.cs`, `ExpeditionSystem.cs` (travel/tick math).

**Substeps:**
1. Read `WastelandMapSystem` to learn the node/route schema (coords, danger, travelHours, terrain, tier) and how nodes bind to `loc_*` ids.
2. Read `ExpeditionSystem` to confirm which node fields drive travel time, rads, and encounter rolls — only author fields the engine reads.
3. Plot a coherent geography: cluster the 115 `locations.json` entries into 6 regions (crater core, dead suburbs, industrial belt, deep coast, ash flats, northern treeline).
4. Author 54 new nodes across the 6 regions, each resolving to a real `loc_*` or a pure transit waypoint.
5. Author routes with travelHours, dangerLevel, baseRadsPerHour matching destination tiers (danger tiers 1–10 already exist in `locations.json`).
6. Ensure route topology creates real decisions: a fast-dangerous corridor vs. a slow-safe loop between the same two points.
7. Pin region anchors: faction seats (10A warlords), the deep-coast entry (10C), excavation sites (11A).
8. Validate every `loc_*` ref resolves; no orphan nodes (unreachable) and no orphan locations (unreachable from the shelter).
9. Run data-integrity selftest + map/expedition selftests.
10. xUnit: graph connectivity (shelter reaches all nodes), route cost math, danger-tier consistency.

**Next steps:** feeds 16B (waystations sit on routes) and 11C (map evolution mutates this graph);
the map panel (08A art) renders this densified graph.

---

## Task 16B — Waystation & caravan route network

**Goal:** Make the abstract waystation/caravan systems concrete: authored waystation sites on
real routes and scheduled caravans that turn the map into a living logistics web.

**Files:** waystation + caravan data (extend `expeditions.json`/economy data — confirm loaders),
`locations.json` (waystation `loc_*`), read-only `WaystationSystem.cs`, `TravelingCaravanSystem.cs`,
`CaravanAtomicTrader.cs`, `MarketSystem.cs`.

**Substeps:**
1. Read `WaystationSystem` + `TravelingCaravanSystem` to learn waystation state (stock, defense, staffing) and caravan scheduling.
2. Place 6 waystations on the 16A routes at natural chokepoints (pass, bridge, river ford, rail junction).
3. Author each waystation's identity: specialty stock (ties to 13A regional goods), defense level, a named keeper, one problem.
4. Author 4 caravan circuits, each visiting 3–4 waystations on a schedule the player can learn and intercept.
5. Give each caravan a specialty (13A goods) and a vulnerability (a route leg through high danger).
6. Wire caravan arrival to a local price/stock effect via `MarketSystem` (buy low before it leaves).
7. Author 6 waystation quests (defend, resupply, cure a sickness, find a lost drover).
8. Cross-check `loc_*`, `item_`, `faction_` refs; data-integrity selftest.
9. xUnit: caravan schedule determinism, waystation stock refresh, arrival price effect.
10. Balance sim: caravan goods must not undercut scavenging; cross-tool QA.

**Next steps:** waystation fortification quests (registry suggestion); player-founded waystation
(late-game); caravan raids (10A enemies).

---

## Task 16C — Regional treaty & diplomacy web (4 → 12 accords)

**Goal:** Expand `foundry_accords.json` + `RegionalTreatySystem` into a real diplomatic layer
where treaties between factions create obligations, embargoes, and flashpoints the player navigates.

**Files:** `foundry_accords.json`, `foundry_treaty_consequences.json`, `faction_lore.json`,
read-only `RegionalTreatySystem.cs`, `FactionWarSystem.cs` (Year of Ash), `MarketSystem.cs` (embargoes).

**Substeps:**
1. Read `RegionalTreatySystem` + `foundry_accords.json` to learn the treaty schema (parties, terms, consequences, breach rules).
2. Read `foundry_treaty_consequences.json` to see how breaches are resolved today.
3. Author 8 new accords: trade pacts, non-aggression, resource-sharing, border demarcation, mutual defense — across the real factions.
4. Give each accord a consequence chain (what happens on breach) wired into `foundry_treaty_consequences.json`.
5. Author 2 commodity embargoes (a faction refuses to sell medicine/fuel) that pressure `MarketSystem` prices.
6. Create 3 flashpoint events where two treaties conflict and the player must choose a side (feeds moral branching + faction standing).
7. Tie 2 treaties to the faction-war arc (06C) so diplomacy visibly fails into war.
8. Ensure treaties produce/consume real `flag_` and `faction_` ids; no orphans (dialog-graph lint).
9. Data-integrity selftest + narrative-continuity check.
10. xUnit: treaty load, breach consequence trigger, embargo price effect, faction-standing delta.

**Next steps:** player-brokered treaties (endgame diplomacy); treaty violations as Verdict evidence (15B).
