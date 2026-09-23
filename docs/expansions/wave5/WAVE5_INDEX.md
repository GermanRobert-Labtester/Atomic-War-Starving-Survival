# ASHFALL — Expansion Wave 5 Index (Expansions 32–36)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-22
**Purpose:** Index and evidence summary for the five Wave 5 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin or absent around a working seam, and propose
expansions that attach to existing owners. They are design bibles: they do not
claim paths, change code, or authorize implementation. Any implementation must
later pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and
`TEST_POLICY.md`.

---

## The wave theme: the frontier and its cost

Waves 1–4 built the shelter from the inside out: survival, culture, hazards,
infrastructure, and the crafts that make a camp a town. Wave 5 turns outward.
Four plans face the world beyond the wall — the living land, the sky, the road,
and the dark — and one plan faces the cost of surviving all of it.

| Expansion | Live seam | Current content | Gap |
|---|---|---|---|
| 32 · The Wild | `WildlifeEcosystemSystem` (32 KB), `WildlifeMigrationSystem`, `WildlifeTrappingSystem` with seeded streams, `CompanionAnimalSystem` (37 KB), `WildlifeSeasonalCalendar` | `wildlife_ecosystem.json` = **3.8 KB** | hunting, seasons, quotas, pressure visibility, predator conflict, companion work, field study |
| 33 · The Weather | `WeatherSystem` (22 KB) with seven weighted weather kinds and a six-hour tick, `WeatherEffectsCatalog`, `WeatherGate`, `WeatherAtmosphereMap`, `YearOfAshStormCatalog`, `EconomyWeatherShockRules` | forecasts/records absent | forecasting, warnings, storm response, ash seasons, black rain, readiness |
| 34 · The Long Road | `RouteInfrastructureSystem` (16.7 KB) with segment modes, clearance states, and minefields, `WeatherGate`, vehicles and garage, `waystations.json` (10.8 KB) | bridges/ferries/convoys absent | crossings, convoy operations, clearance, maintenance, waystation ops, passage agreements |
| 35 · The Habit | `ChemicalDependencySystem` (25 KB) with managed detox, cold turkey, staffing, stress; `ChemicalDependencyAfflictionHandler` (10 KB); `PharmaLabSystem` | `chemical_dependency_items.json` = **2.8 KB** | treatment programs, tapers, withdrawal care, counseling, recovery, family, policy |
| 36 · The Watch | `PatrolTerritoryAuthority` (9.7 KB), `PatrolEncounterValidator` (16.4 KB), `SoundRangingThreatEngine` (17 KB), `NightWatchCatalog`, patrol radio hooks | `faction_territory.json` (20 KB), narrative logs | posts, patrol routes, perimeter systems, detection content, alarms, gate protocol, incident logs, drills, welfare |

The strongest authority splits in the wave: hunting pressure is recorded through
the live ecosystem ledger and nothing respawns without migration or recovery;
forecasts read the weather engine and never reroll it; road travel resolves
through the live speed, hazard, and traversal calls; dependency progression
stays entirely in the live ledger while new systems only modify tick parameters;
and the watch ends at verification, handing force to combat, rescue to the alarm,
and care to medicine.

---

## The five plans

1. `expansion_32_the_wild_plan.md` — 70,224 chars.
   Species, predator-prey edges, migration, hunting, trapping, quotas, predator
   conflict, companions, breeding, contamination, and field study. Extends the
   live ecosystem, migration, trapping, and companion systems.
2. `expansion_33_the_weather_plan.md` — 70,496 chars.
   Observation posts, instruments, signs, forecast models with honest
   confidence, warnings, storm response, ash seasons, black rain protocol,
   records, and seasonal readiness. Extends `WeatherSystem`, `WeatherGate`, and
   the hardening path.
3. `expansion_34_the_long_road_plan.md` — 70,580 chars.
   Corridors, bridges, fords, ferries, convoys, clearance, maintenance,
   waystations, and passage agreements. Extends `RouteInfrastructureSystem` and
   the live vehicle systems.
4. `expansion_35_the_habit_plan.md` — 70,129 chars.
   Dependency care programs, taper schedules, withdrawal profiles,
   substitution, recovery phases, peer support, family stability, and written
   policy. Extends `ChemicalDependencySystem` and the medical pipeline.
5. `expansion_36_the_watch_plan.md` — 70,584 chars.
   Watch posts, patrol routes, perimeter systems, detection profiles, alarm
   protocols, gate rules, territory operations, incident logs, and readiness
   drills. Extends the territory, patrol, and acoustic-detection systems.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core
  logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with
  integer `schema_version`, validated by `CatalogIntegrityValidator` and
  registered with `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement
  and an integration-seam table. No second ecology, weather, road, dependency,
  security, or save system is introduced.
- **Deterministic.** Live seeded paths are used where they already exist
  (trapping's deployment, encounter, and incident streams; weather generation;
  patrol validation). New outcomes are deterministic functions of authored
  inputs. Paired replay hashes must match.
- **Persistence.** New state is additive inside existing owners (wildlife,
  weather/gate, route infrastructure, dependency ledger, patrol/radio). Legacy
  saves load neutral; the Triad drift gate must pass.
- **Tone.** Restrained, human, fictional. No real brands, agencies, institutions,
  or events copied.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A
  compile-green result is not acceptance.

---

## Ethical and content contracts specific to Wave 5

| Plan | Hard contract |
|---|---|
| 32 Wild | Subsistence, never trophy; predators are animals, not monsters; extinction is real but recoverable via migration; companions eat, tire, and die; contaminated game drives the radiation pipeline |
| 33 Weather | Forecasts display honest confidence and can miss; no storm is an unrecoverable wipe; ash and fallout are procedural and factual, never lurid; no weather control |
| 34 Road | Convoy losses default to partial and survivable; mine clearance is procedural, never gory; tolls are negotiated agreements, never extortion; banditry has social causes, never caricature |
| 35 Habit | Dependency is a medical condition, never shame; no glamorized use; no punishment, torture, or public shaming; relapse re-enters care; children are family stability only, never exposed; a required sensitivity review gates content |
| 36 Watch | Detection ends at verification; force belongs to combat, rescue to the alarm, care to medicine; gates are predictable and dignified with an appeal path; territory is map-and-agreement, never ethnic framing; fatigue is visible and fixable |

---

## Cross-wave hooks (summary)

- **Wave 1 × Wave 5:** children learning the land and the watch; rites for taken
  animals and storm vigils; flight weather and air watches; game pressure and
  field crops; prosthetics for hunters, drivers, and sentries.
- **Wave 2 × Wave 4 × Wave 5:** radio weather warnings and patrol calls; black
  rain in the quarantine and decon pipeline; the grid keeping the watch lit; the
  cleaning, printing, glazing, and kiln systems supplying every new room.
- **Wave 5 × Wave 5:** the Wild's animals feed the Watch's alarm lines; the
  Weather's storms close the Road and drive the Habit's pain and cold; the Road
  carries the Wild's meat, the Weather's warnings, and the Habit's medicine; the
  Watch guards all of it and logs what happens.

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 5 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an
   API or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and
   focused verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then
   content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe
pre-signature work is Phase 1 (schemas and validators), which is additive and
reversible.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; the full wave is
  roughly 300,000–340,000 words of new prose if all five are authored).
- Save placement (additive sub-objects versus sibling sections) per domain. Each
  plan recommends additive sub-objects.
- Whether any Wave 5 expansion adds a new headless selftest verb or extends an
  existing one. Each plan recommends extending existing verbs.
- Priority order. Recommended: 33 (Weather) first, because forecasts make every
  other outdoor system safer; 32 (Wild) second, because food pressure is the
  tightest loop; 36 (Watch) third, because readiness hardens the perimeter for
  everything else; 34 (Road) fourth, because it depends on weather and vehicles;
  35 (Habit) fifth and always, because it touches the medical system every other
  plan feeds.

---

## Evidence anchors (file references used across the plans)

- `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` — `FaunaSpeciesDef`,
  `PredatorPreyEdgeDef`, `PressureKeyState`, `ApexActivityState`,
  `RecordHuntingPressure`, `KnowledgeLevel`, `CanTame`.
- `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` — trap sites, durability,
  seeded deployment/encounter/incident streams.
- `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` — assign, feed, results.
- `Assets/Ashfall.Core/World/WeatherSystem.cs` — `SeasonWindowDef` with
  clear/rain/overcast/ashfall/fallout storm/blizzard/black rain,
  `SeasonProfileDef` six-hour interval, `WorldWeatherState`.
- `Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs` —
  `RouteSegmentInfrastructureRecord`, `MinefieldSegmentState`,
  `RailSegmentCondition`, `GetSpeedLimit`, `GetHazardModifier`,
  `GetTravelModifier`, `CanTraverse`.
- `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` —
  `OnSubstanceConsumed`, `ReportStress`, `BeginManagedDetox`,
  `BeginColdTurkey`, `TickHours`, `HasActiveWithdrawal`, `DependencyLevel`.
- `Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs` and
  `Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs`.
- Data: `wildlife_ecosystem.json` (3.8 KB), `wildlife_trapping_catalog.json`
  (23.2 KB), `weather_seasons.json` (3.2 KB), `weather_effects.json` (7.6 KB),
  `seasonal_events.json` (12.3 KB), `waystations.json` (10.8 KB),
  `faction_territory.json` (20 KB), `chemical_dependency_items.json` (2.8 KB).
- `docs/expansions/wave1/WAVE1_INDEX.md`, `wave2/WAVE2_INDEX.md`,
  `wave3/WAVE3_INDEX.md`, `wave4/WAVE4_INDEX.md`.
- `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`,
  `INTEGRATION_PLANS.md`.