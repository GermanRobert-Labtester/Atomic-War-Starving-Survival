# Plan 28 — Wildlife, Migration & the Living Wasteland Ecology

> **Theme:** The non-human world as a *system*. `WildlifeMigrationSystem` (+
> `WildlifeMigrationSystem.Live`) exists alongside `WildlifeTrappingSystem`, but there's no
> migration content, no ecological web, no seasonal herds. This plan makes the wasteland's
> ecology move.
>
> **Key evidence (verified):** `WildlifeMigrationSystem.cs` + `.Live.cs` live; **no**
> `wildlife_migration.json` exists; `WildlifeTrappingSystem` (13B covers trapping); 20A covers
> the field guide. This plan is the *movement and ecology* layer those two don't touch.

---

## Task 28A — Wildlife migration & seasonal herds

**Goal:** Author migration content so herds/pest swarms move across the map with the seasons,
creating hunting windows, hazards, and famine/abundance cycles.

**Files:** new `wildlife_migration.json` (catalog), `locations.json` (migration corridors),
read-only `WildlifeMigrationSystem.cs`, `WildlifeMigrationSystem.Live.cs`, `WeatherSystem`
(seasons), `WildlifeTrappingSystem` (13B quarry).

**Substeps:**
1. Read both `WildlifeMigrationSystem` files to learn the model (species, routes, season triggers, population) and confirm the catalog file it expects (it's missing).
2. Author the migration catalog: 6 species (a caribou-analog herd, a molerat swarm, a mutated boar sounder, a bird migration, a fish run for the coast, a locust-analog blight).
3. Give each a seasonal route across real `loc_*` corridors (16A map) keyed to 19C seasons.
4. Author abundance/scarcity effects: a passing herd = hunting window (13B); a locust swarm = crop blight risk (22B); a fish run = coastal bounty (23C).
5. Author 6 migration events (the herd arrives, the swarm descends, the run begins) surfaced via radio/forecast (24A/19A).
6. Wire migration to rad-taint: herds passing through fallout zones become tainted (food-safety cost, 13B).
7. Author 4 ecological disruptions (a route blocked by war 06C → herd starves → predators turn desperate).
8. Validate ids; data-integrity selftest.
9. xUnit: migration by season, corridor traversal, abundance event, rad-taint application, determinism.
10. Balance sim: migration must create rhythm without a guaranteed food exploit; cross-tool QA.

**Next steps:** a legendary beast that migrated off-script (a hunt quest); migration collapse as
a famine driver (22B); herd movement visible on the map (16A).

---

## Task 28B — Ecological hazard blooms & infestations

**Goal:** Author dynamic infestations — mold, vermin, hive, blight — that bloom in locations
and the shelter, creating localized crises with ecological logic.

**Files:** `events.json` + a hazard-bloom data source, `disease_catalog.json` (spore link, 09A),
read-only `GreenhouseSystem` (blight), `VentilationSystem`, `ExcavationSystem` (11A disturbs nests).

**Substeps:**
1. Read how blight/infestation is modeled in `GreenhouseSystem` and whether a general infestation mechanic exists (if not, this is data + events on existing systems, not a new system).
2. Author 6 location infestations (a molerat nest, a hornet-analog hive, a mold bloom, a roach colony, a fungus carpet, a rat king) that render a site hazardous until cleared.
3. Author the clear methods per infestation (smoke, fire, traps, seal it) using existing items/skills.
4. Author 4 shelter infestations (vent mold — `VentilationSystem`, pantry weevils — food loss, a nest in the walls) as crises.
5. Wire excavation (11A) to a chance of disturbing a nest — the risk of digging.
6. Key mold/spore blooms to the 09A spore disease and to damp seasons (19C).
7. Author the "leave it" option where an infestation is a resource (a hive = honey 22B, roaches = protein) — ecological ambivalence.
8. Validate ids; data-integrity selftest.
9. xUnit: infestation bloom, clear-method resolution, shelter food loss, harvest-if-left option.
10. Narrative-continuity + `DataRuleComplianceTests` (grounded, not fantasy monsters).

**Next steps:** a "pest controller" trade specialty (26B); an infestation that spreads between
waystations (16B); a beneficial-symbiote discovery (a mold that eats radiation — research 26A).

---

## Task 28C — The ecological web: predator-prey & resource chains

**Goal:** Connect fauna, flora, and resources into a legible ecological web the player can
*read and exploit* — the deepest, most systemic ecology content.

**Files:** `field_guide.json` (20A) + ecology data, `economy_goods.json` (13A), read-only
`WildlifeMigrationSystem`, `WildlifeTrappingSystem`, `GreenhouseSystem`, `MarketSystem`.

**Substeps:**
1. Design the web on paper first: 3 chains (e.g., grain → rodent → predator; carrion → scavenger → disease; kelp → fish → flotilla catch).
2. Read how trapping/migration/greenhouse/market could share a population or abundance signal — if none share one, keep the web *content-level* (authored consequences) not a new simulation.
3. Author consequence rules as content: overhunt the herd (13B) → predators starve → desperate predator encounters (20C) near the shelter.
4. Author the inverse: a blight (28B) kills the grain → rodent crash → a lean season (13A scarcity).
5. Author 6 "reading the land" field-guide entries (20A) that *teach* the web (tracks, scat, birdsong going quiet) so players can anticipate.
6. Author 4 exploitation opportunities (follow the scavengers to a kill = loot; the fish run = a fishing event).
7. Wire web state to market prices (13A) — a herd collapse raises meat prices at waystations (16B).
8. Validate ids; data-integrity selftest.
9. xUnit: a web consequence fires on its trigger (overhunt → predator encounters); price effect applies.
10. Balance sim + cross-tool QA: the web must create legible consequences, not chaotic cascades.

**Next steps:** a steward-vs-exploiter moral axis (26B trait); rewilding a zone as an endgame
achievement (15A epilogue line); a collapsed-ecology dead zone as a cautionary location (11A).
