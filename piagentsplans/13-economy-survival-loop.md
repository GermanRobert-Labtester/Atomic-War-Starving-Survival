# Plan 13 — Economy & Survival Loop: Goods, Trapping & Crisis Weather

> **Theme:** The survival-economy core loop (scavenge → process → consume/trade) is fully
> mechanical but its *texture* is thin: 39 recipes, background-only trapping, and 22 weather
> states with almost no bespoke crisis events. This plan enriches the daily loop.
>
> **Key evidence:** `recipes.json` = 39; `WildlifeTrappingSystem` runs as a background tick
> (registry §20); `events.json` = 77 but few weather-keyed; `economy_goods.json` +
> `MarketSystem`/`LedgerDebtSystem`/`TradeTellEngine` live.

---

## Task 13A — Economy goods & advanced crafting expansion

**Goal:** Broaden the goods/recipe economy so crafting stations and trade have depth, with new
regional specialties that make caravans worth waiting for.

**Files:** `economy_goods.json`, `recipes.json`, `items.json`, `trade_tell_lines.json`
(covered deeper in Plan 05/07), read-only `MarketSystem.cs`, `CraftingSystem.cs`,
`TravelingCaravanSystem.cs`, `HardcoreEconomyTuning.cs`.

**Substeps:**
1. Read `economy_goods.json` + `recipes.json` schemas; map the 39 recipes across the 6 crafting stations to find empty stations.
2. Read `MarketSystem` scarcity tiers + `HardcoreEconomyTuning` so new goods slot into existing price curves, not new ones.
3. Author 15 new trade goods across scarcity tiers (regional specialties: flotilla salt, foundry castings, greenhouse seeds, pre-war pharmaceuticals).
4. Author 12 new recipes filling empty stations (advanced water purification, filter reconditioning, canned ration production, textile repair).
5. Give each new good a regional supply profile so caravan origin matters (route planning pressure).
6. Wire 3 goods into caravan specialty stock (`TravelingCaravanSystem`).
7. Ensure all inputs/outputs resolve to real `item_` ids; no new currencies.
8. Data-integrity selftest; economy selftest.
9. xUnit: recipe validity, station assignment, price-curve placement, caravan stock.
10. Balance sim (`ashfall-balance-sim`): new goods must not break scarcity; cross-tool QA.

**Next steps:** commodity embargoes (registry suggestion); regional price maps surfaced in the economy panel.

---

## Task 13B — Active trapping, hunting & butchery loop

**Goal:** Convert `WildlifeTrappingSystem` from a background tick into an active loop: bait
crafting, lure placement, trap-line management, and butchery decisions.

**Files:** new trapping/bait data (extend `items.json` + a trapping catalog — confirm loader),
`recipes.json` (bait/lure crafting), read-only `WildlifeTrappingSystem.cs`,
`SkillProgressionSystem.cs` (hunting skill), `DiseaseSystem` (rad-taint).

**Substeps:**
1. Read `WildlifeTrappingSystem` (deadfalls, snares, butchery yields, rad-taint) to learn what player inputs it accepts vs. hardcodes.
2. Identify the minimal Core extension to expose trap-line management (if the system is fully passive, this is a small CORE task — flag it, don't hack the host).
3. Author 6 baits/lures (scrap-meat bait, grain lure, mutated-beast pheromone lure) as craftable recipes.
4. Author 6 quarry animals with distinct yields, rad-taint risk, and trap-type affinity.
5. Add trap-line placement/maintenance decisions (where, how many, check frequency) surfacing in a small UI surface.
6. Wire butchery choices (meat vs hide vs both) to yields and to `item_` outputs.
7. Connect rad-taint to dosimeter/food-safety so careless hunting has a cost.
8. Tie hunting-skill level to trap success (existing skill system).
9. Data-integrity selftest; determinism check (`ISeededRng` on yields).
10. xUnit + balance sim: trapping must supplement, not replace, greenhouse/rations; cross-tool QA.

**Next steps:** trophy mounts feed shelter decor (12C); guard-dog training (registry suggestion); mutated-beast hunts as combat encounters (10A).

---

## Task 13C — Weather-specific crisis events (22 states → bespoke events)

**Goal:** Give the 22 `WeatherKind` states bespoke crisis events so weather is an *event
driver*, not just a number modifier — pure data work on the existing weather keys.

**Files:** `events.json` + `year_of_ash_events.json` (weather-keyed entries), read-only
`WeatherSystem.cs`, `WeatherKind.cs`, `WeatherAtmosphereMap.cs`, `SkyLayerArmorSystem` (for
strike events), `PowerGridSystem` (for EMP).

**Substeps:**
1. Read `WeatherKind.cs` to list all 22 states and `events.json` to learn the weather-trigger grammar.
2. Map which states currently have zero bespoke events (likely most).
3. Author 2 crisis events for each major state: fallout storm (breach scare, filter strain), black rain (water contamination, outdoor exposure), EMP storm (electronics failure, radio blackout), acid snow (roof corrosion, gear damage), bio-fog (spore exposure, visibility).
4. Author 1 event for each remaining minor state so no weather is silent.
5. Key each event to the weather system's current state via the existing trigger field — no new scheduler.
6. Make 3 events threaten specific systems (sky armor, power grid, air intake) to pressure those loops.
7. Ensure each event has 2–3 meaningful choices with resource/health/morale trade-offs.
8. Validate ids + weather keys; data-integrity selftest.
9. Event reachability lint (no orphaned weather keys); narrative tone check.
10. xUnit: each weather state can fire its events; choices apply deltas; determinism.

**Next steps:** weather forecasting payoff (weather-station system predicts a crisis event);
cloud-seeding countermeasure (white space #17) cancels a crisis — the strategic capstone.
