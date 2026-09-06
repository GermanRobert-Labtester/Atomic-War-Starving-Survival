# Plans 158–161 — Repository Reconnaissance (Phase A Exit Gate)

**Stream authority:** [PLANS_158_161_MASTER_PLAN.md](PLANS_158_161_MASTER_PLAN.md)
(ModularVehicleSystem · MacroWeatherSystem · TradeRouteSystem · SuccessionSystem)

**Date:** 2026-09-05 · **Branch:** `feat/asset-pipeline-flagship` · **Method:** six read-only
audits (vehicles, weather, trade/caravans, succession/history, content validation, UI
conventions) + baseline regression run. All findings carry file:line evidence verified
against current source; concurrent streams were active in the worktree (45 M / 92 ?? files)
and were not touched.

Per master plan §3: **no production edits until this authority map is documented.** This
document is that map. Phase B (shared contracts) decisions recorded here are
recommendations for the implementing phase, not edits.

---

## A. Plan 158 — Vehicles: existing ownership

### A.1 Authority inventory

| Concern | Owner | Evidence |
|---|---|---|
| Garage + vehicle instances | `ExpeditionVehicleSystem` (Core root, NOT `Expeditions/`) | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs:12` |
| Vehicle catalog | `vehicles.json` (8 vehicles, 1 track gear; `schema_version:1`) | `Assets/StreamingAssets/Data/vehicles.json:1-111` |
| Sortie travel math | `ExpeditionSystem` (scalar `distanceTicks`, phases) | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:183` |
| Save section | `"expedition"` → `expedition_save.json`, aggregate = sorties + garage | `src/Host/ExpeditionSaveStore.cs:24-31` |
| Host orchestration | `ExpeditionHostSession` (dispatch, refuel, repair, garage RNG) | `src/Host/ExpeditionHostSession.cs:23-26` |
| Vehicle→expedition seam | `ExpeditionVehicleProfile` projection at dispatch | `ExpeditionSystem.cs:87-98`, host `BuildProfile` |

### A.2 Key facts the modular design must respect

1. **Vehicle identity is the catalog id string — one instance per id**
   (`ownedVehicles: Dictionary<string, VehicleInstance>`, `AcquireVehicle` blocks on
   `already_owned` before catalog lookup — `ExpeditionVehicleSystem.cs:139-140`). A modular
   system with per-module durability needs an instance layer: the house pattern is the
   deterministic FNV-1a persisted sequence of
   `ProceduralItemInstance` (`Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs:34-86`).
2. **Condition exists today** (`VehicleInstance.condition` 0–100 + `isBrokenDown` +
   `breakdownCause`; track gear has its own condition with effective multipliers —
   `ExpeditionVehicleSystem.cs:21-60`). Per-module durability should reuse the 0–100 +
   `maxCondition` idiom (`EquipmentConditionSystem.cs:17-35`), not invent a third form.
   Note: `breakdown_threshold` in `vehicles.json` is dead data (no code path reads it).
3. **Fuel:** `fuel_consumption_per_km` × `KmPerTravelTick = 2.5f` (host const) →
   `fuelPerTravelTick`; dispatch gate is exact-distance in the garage
   (`ExpeditionHostSession.cs:633-650`). Cargo: vehicle `cargoCapacityKg` *replaces* the
   40 kg foot cap (`ExpeditionSystem.cs:349-350`); overweight loot stays behind.
4. **Breakdown:** per-travel-tick seeded roll `rng.NextDouble() <
   vehicleBreakdownChancePerTick` (`ExpeditionSystem.cs:605-618`), chance derived from
   condition × 0.15 × gear risk multiplier (`ExpeditionVehicleSystem.cs:295-298`);
   garage prep roll is separate (`:319-325`). `OnVehicleBreakdown` → host `LastEvent`.
5. **All new stats must reach expeditions only through `ExpeditionVehicleProfile`** —
   `Start`/`Estimate` are the single consumption points. Established composition pattern:
   naval (`ExpeditionNavalSystem.cs:256-262`) and crawler
   (`ArmoredCrawlerExpeditionSystem.cs:246-280`) both project alternate profiles.
6. **Strongest prior art — the armored crawler already IS a modular vehicle:**
   `ArmoredCrawlerState { chassisId, installedModuleIds, hullIntegrity, maxSlots:6,
   maxMass:3500 }` with `CrawlerModuleDefinition { slotType ∈ Cabin/Chassis/Utility/
   Defense/Treads, mass, armorModifier, fuelModifier, cargoModifier, … }`, slot+mass
   budget gating, atomic install bills (`ArmoredCrawlerExpeditionSystem.cs:34-226`), and
   aggregate stat math summing module modifiers into a projected profile (`:150-162,
   :246-280`). Data: `armored_crawler_modules.json`. Plan 158 should generalize this
   proven shape onto garage vehicles, not invent a new one.
7. **Save fork risk:** crawler modules live in their own section
   (`src/Host/ArmoredCrawlerSaveStore.cs`) while garage vehicles live inside the
   `"expedition"` aggregate. Master plan §12 recommends a dedicated `modular_vehicles`
   section; the crawler precedent supports that. Decision (Phase B): new section keyed by
   vehicle id; `ExpeditionAggregateCodec.Decode` gains base/default slots for old saves
   (its shape-routing migration ladder is at `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs:88-105`).
8. **Garage proto-slots today:** one `trackGear` slot (install replaces whole state) and a
   stat-free `attachments: List<string>` (`ExpeditionVehicleSystem.cs:29,190-200`). No
   chassis/slot vocabulary exists in the garage.
9. **RNG:** host garage uses hardcoded `SeededRng(7072)` (session `7071`) —
   `ExpeditionHostSession.cs:23-26`. New module streams must instead use keyed
   `CampaignRngStream` ids (see E.2), matching plan §2.4
   (`vehicle.module_install`, `vehicle.damage_degradation`).

---

## B. Plan 159 — Macro weather: existing ownership

### B.1 Authority inventory

| Concern | Owner | Evidence |
|---|---|---|
| Daily weather | `Ashfall.Core.World.WeatherSystem` (single global `WorldWeatherState`) | `Assets/Ashfall.Core/World/WeatherSystem.cs:36-44,53` |
| Seasons | weather *windows* in `weather_seasons.json` (6×60-day), not a calendar | `WeatherSystem.cs:10-32,89-100` |
| Tick path | `WeatherWorldDayOwner.TickDay` → `WorldHostSession.TickHours` → `Weather.Tick` | `src/Main.CampaignOwners.cs:80-84` |
| Save | weather is the `State` field of the `"world"` envelope (`world_save.json`) | `src/Host/WorldSaveStore.cs:131-138` |

### B.2 Key facts

1. **Weather is rolled every 6 in-game hours** (4×/day), weighted by the active season
   window; RNG is reseed-per-roll `seed*397 + rollCount` with `rollCount` persisted
   (`WeatherSystem.cs:143-174`). The seed is the hardcoded `DemoSeed = 1234`, **not
   derived from the campaign seed** (`src/Host/WorldHostSession.cs:16,119`) — a standing
   determinism divergence any macro layer must not inherit. `CampaignStreamIds.Weather =
   "weather"` exists but is never forked in production
   (`Assets/Ashfall.Core/Random/CampaignRngStream.cs:9`).
2. **No per-zone weather state exists.** Zone-ish vocabularies: `loc_*` map locations,
   `zone_id` in `damaged_map_zones.json`, `subnode_*` in `subterranean_zones.json` (which
   carries per-node `base_structural_risk` — prior art for shelter wear). Zone radiation
   exists via `FalloutSystem` clouds/groundwater per zone
   (`Assets/Ashfall.Core/World/FalloutSystem.cs:229-306`).
3. **Composition seam already exists:** `restrictToNonHazardWeather` is a working
   "macro-overrides-local" lever (`WeatherSystem.cs:149-156,277-281`). Macro modifiers
   should compose the same way (global modifier applied at roll/derive time), never
   fork `WeatherSystem`.
4. **Greenhouse has NO temperature input.** `GreenhouseSystem.TickDay(day,
   growLightHours, ashContaminationRate)` is water+light only; the host passes constants
   (`6f`, `0.04f`) — `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs:297-336`,
   `src/Main.CampaignOwners.cs:199`. Crop failure = blight from drought/soil/ash. Macro
   coupling enters via (a) a weather-driven `ashContaminationRate`, (b) a new parameter,
   or (c) the apiculture temperature argument (currently hardcoded 22 °C,
   `src/Host/GreenhouseHostSession.cs:323-331`).
5. **No canonical shelter structural-health system.** Fragments:
   `WeatherHardeningSystem` (per-zone insulation wear, pipe freeze/burst, frost heave —
   already consumes `WeatherSystem.Current` + temperature penalty,
   `Assets/Ashfall.Core/World/WeatherHardeningSystem.cs:193-292`) — **but its `TickDay` is
   never called in production** (`src/Main.ShelterInfrastructure.cs:351-366` constructs +
   saves only); `ShelterThermalSystem` (explicit external hooks
   `SetExternalInsulationModifier`/`RegisterExternalBurst`, `:621-660`); `SkyLayerArmorSystem`
   (roof-cell durability); `ShelterFireHazardSystem` (fire-scoped structural damage).
   Recommendation: extend `WeatherHardeningSystem` as the prolonged-stress consumer and
   separately wire its missing production tick (pre-existing gap, independent fix).
6. **Forecast machinery largely exists:** `WeatherSystem.PeekForecast(daysAhead)`
   (deterministic, non-mutating — `WeatherSystem.cs:286-351`),
   `WeatherStationSystem.GenerateForecast` (tiered horizon, confidence decay,
   preparation payoffs), `WeatherSondeSystem`, `WeatherIntelligenceCoordinator`
   (`bestTravelDay`, `routeSafeDays`). Early-warning radar (plan §6.11) extends the
   station/sonde confidence plumbing rather than duplicating it.
7. **Unwired travel-gate surface:** `WeatherRouteGateCatalog.EvaluateBlock` +
   `ExpeditionHostSession.ExtraGateBlock`/`OnWeatherGateForced` are fully built but
   `ExtraGateBlock` is assigned nowhere in production — the natural consumer hook for
   macro-disaster travel disruption (`src/Host/ExpeditionHostSession.cs:77-158`).
8. **`WeatherKind` defines 23 kinds; only 7 are ever rolled.** The extras (EMPStorm,
   AcidSnow, BlackSnow, RadHail, IceStorm, …) already have audio cue mappings
   (`src/Audio/AudioEventBridge.cs:447-464`), ambience routing
   (`src/Audio/SurfaceAmbienceController.cs:70-95`) and station preparation payoffs —
   presentation is pre-wired for macro disaster kinds; only the roll/composition layer is
   missing.
9. No existing macro/disaster phase machine anywhere (verified: `nuclear_winter` is lore
   text only; `monsoon`/`disaster` hits are test/narrative-only).

---

## C. Plan 160 — Trade routes: existing ownership

### C.1 Authority inventory — caravans DO exist (three overlapping systems)

| System | Shape | Evidence |
|---|---|---|
| `CaravanTradeNetworkSystem` | Seasonal faction caravans *coming to the shelter*; status machine, manifests, guard-strength numbers, midpoint permille hazard roll, atomic barter vs shared `Inventory` | `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs:68` (tick `:321-381`, hazard `:383-425`, barter `:227-319`); host `src/Main.AdvancedShelterSystems.cs:41-79`; data `caravan_trade_routes.json` (10 routes) |
| `TravelingCaravanSystem` | NPC caravans hopping **map node waypoints** (`routeNodeIds`, `currentNodeId`, stay duration); barter does NOT touch shared inventory | `Assets/Ashfall.Core/TravelingCaravanSystem.cs:45,169-204`; data `caravans.json` (4 caravans over `loc_*` nodes) |
| `TradeCaravanCatalog` | Narrative static routes (18 entries, unwired) | `Assets/Ashfall.Core/Narrative/TradeCaravanCatalog.cs` |

No class named `CaravanSystem`. Plan 160 must extend/compose `CaravanTradeNetworkSystem`
(player-dispatched caravans are a new capability, but barter/pricing/manifest idioms and
the `caravan_trade_routes.json` consumer registration are reusable) rather than add a
fourth caravan system.

### C.2 Key facts

1. **The only real graph is `WastelandMapSystem`:** `MapNode` (danger, faction, PositionX/Y)
   + `MapRoute` edges (DistanceKm, WeatherHazard, TravelDomain, strength, contamination),
   deterministic BFS `PlanRoute(fromId,toId)` weighted by DistanceKm over discovered nodes;
   data `wasteland_map_v1.json` (11 nodes, 26 edges) —
   `Assets/Ashfall.Core/World/WastelandMapSystem.cs:17,197-238,277-356`.
2. **`IWorldRoutePlanner`/`ITravelCostCalculator` do not exist** (zero grep matches) —
   plan §7.8's "preferred shared service" must be introduced as a thin facade over
   `WastelandMapSystem.PlanRoute`. **`ExpeditionSystem` deliberately does NOT use the
   graph** — it travels a scalar `distanceTicks` per destination and is explicitly
   decoupled from the map graph (`ExpeditionSystem.cs:157-213`). See divergence F.2.
3. **Prices have four coexisting owners** (no single authority): `MarketSystem`
   (demand-multiplier model, daily seeded volatility walk, clamp [0.25×,4×], scarcity
   signal `IsSuppliesShort` — `Assets/Ashfall.Core/Economy/MarketSystem.cs:67,115-201`);
   `HoldfastTradeSession` (stance multipliers); `CaravanTradeNetworkSystem` (canonical
   value switch × export/import/crisis/favored/treaty — `:175-225,427-469`); settlement
   authored modifiers (`SettlementCatalog.cs:21-25`). Plan §7.12 says extend the existing
   economy owner → `MarketSystem` is the closest demand/scarcity model (Phase B decision).
4. **Settlements:** 12 in `settlements.json` with authored economy (stock ids, export/
   import, price modifiers), `route_node` links, threat_level, keeper/trader NPCs,
   sideway quests (`Assets/Ashfall.Core/World/SettlementCatalog.cs:10-134`). No live
   settlement inventory simulation; waystations DO have live stock + 7-day resupply +
   shortage lapse (`WaystationNetworkSystem.cs:17-60`) — prior art for shortage state.
5. **Threats: four unmerged vocabularies** — `MapNodeDanger` (None..Locked), expedition
   `dangerLevel`+`encounterChancePerTick`, settlement `threat_level` (1-3), caravan
   `base_risk_permille` + guard mitigation. Plan §7.9 requires canonical zone threat →
   derive from `MapNode`/`MapRoute` (the graph is the world topology).
6. **Faction relations: two parallel authorities** — `FactionWarSystem` (int standing
   −100..+100, territorial control, `OnFactionStandingChanged`) and `FactionStanceEngine`
   (float trust dict, stance derivation, raid overrides). Trade relation effects must
   pick the economy-side authority (`FactionStanceEngine`, already consumed by trade
   stances) and document the choice.
7. **Raids/escorts: no quest/encounter integration today** — escorts are numeric guard
   strength; `TradeStance.HostileRaid` is display-only; the `trade_route_disrupted`
   feedback message is an orphaned hook awaiting exactly this system
   (`FeedbackMessageCatalogLoader.cs:289`).
8. Region model is tag strings (`the_toll`, `industrial_belt`, …) plus
   `faction_territory.json` (19 territories + 5 contested, `controlled_nodes`);
   `PositionX/Y` are canvas-layout only — no travel math uses coordinates.

---

## D. Plan 161 — Succession/legacy: existing ownership

### D.1 Authority inventory

| Concern | Owner | Evidence |
|---|---|---|
| Death resolution (single authority) | `SurvivorFateSystem` — idempotent cascade, first-report-wins | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:71-88,182-205` |
| Leadership (exists!) | `LeadershipSystem` — `current_leader_id`, `DesignateLeader`, `StepDown`, leader stress; **vacates on leader death; its doc comment flags succession as the missing piece** | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs:19-48,103,145-180` |
| Memorial | `MemorialSystem.Memorialize` — idempotent by SurvivorId, grief cascade | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs:158-225` |
| Skills | `SkillProgressionState` (parallel arrays), `SkillAtrophySystem`, `ApprenticeshipSystem` (mentor ≥30 XP) | `Assets/Ashfall.Core/Survivors/SkillProgressionState.cs:75-91` |
| Governance neighbor | `PoliticsSystem` (elections/coups, uncapped `electionHistory`) | `Assets/Ashfall.Core/Narrative/PoliticsSystem.cs:52-66` |
| Generational prior art | `GenerationalSuccessionEngine` (Expansion 12: retirement succession, generation records, inherited traits); `GenerationalLineageExtension.PerformSuccession` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs:16-105` |

### D.2 Key facts

1. **Canonical death cascade order** (`RunCascade`, `SurvivorFateSystem.cs:258-357`):
   roster.Die → Needs.ForceDeath → assignments cleared/recalled → **leadership notified**
   → shelter-wide grief → final wishes → memorialize → journal (dedup key
   `survivor_death_{id}`) → consequence ledger → buffered day-event. SuccessionSystem
   must subscribe to `OnSurvivorFate` (or an injected cascade lane — all nullable,
   `:107-121`) AFTER memorialization semantics, never intercept — exactly plan §8.11.
   The house rule is written down at `src/Host/SurvivorsHostSession.cs:79`.
2. **`LeadershipSystem` already exists and owns leader identity** — SuccessionSystem
   composes with it (heir designation, era records) rather than re-owning the leader
   field. `SurvivorSocialCoordinator` is the wiring path
   (`SurvivorSocialCoordinator.cs:60,308-313`); persisted under `"survivor_social"`.
3. **Traits are catalog-static** on `SurvivorDefinition`; roster entries carry only
   id/definitionId/joinedDay/isAlive/deathReason — so "legacy traits" as institutional
   effects (plan §8.4) need their own activation state, not survivor-trait mutation.
4. **Durable history today:** survivor-fate ledger (day-sorted), memorial entries,
   consequence-ledger counters/flags, election history (uncapped!), warlord doctrine
   history, cultural-archive chronicles. **The journal is a 64-entry rolling window** —
   not history. `ProceduralEulogyEngine` exists but is unwired in `src/` — free
   integration point for funeral rites / final-words content
   (`Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs:29-36`).
5. **Campaign day is `int` everywhere** (`ICampaignCalendar.CurrentDay`, all persisted
   day fields), day arithmetic is unchecked, `ISimClock.CurrentTick` is `long`
   (1440/day). **No overflow tests exist.** Plan §8.17's multi-century audit is a real
   gap, not a formality: a 500-year campaign ≈ 182,500 days (int-safe), but tick-derived
   and duration counters need the audit + soak.
6. **Save versions are 32-bit ints**; `SaveSectionRegistry.SchemaVersions` versions only
   5 sections today. Envelope `manifestVersion = 2`.
7. **New save section recipe (verified):** (1) `SaveSectionMetadata` in
   `SaveSectionRegistry.All` (`Save/SaveSectionRegistry.cs:53-197`); (2) filename in
   `SectionFileNames` (`:216-359`) — this map IS the envelope whitelist, unknown keys
   throw in `CampaignEnvelopeBuilder.Build` (`:61-70`); (3) optional `SchemaVersions`
   entry; (4) host `SetupX`/`SaveX`/`FlushXIfDirty` triad (pattern:
   `src/Main.SurvivorFate.cs:22-137`) — the triad is CI-validated (MainTriadDriftGate).
8. No `heir`/player-era concept exists anywhere; `era` is archaeology metadata only;
   `legacy` is a namespace name only. NPC factions already model generation counters
   (`FactionSuccessionResult{PreviousLeader,NewLeader,Generation}`) — display precedent.

---

## E. Shared infrastructure (registration recipes verified)

### E.1 Catalog registration recipe (for all four new JSON catalogs)

Verified from the latest flagship additions (kinetic flywheel + toxic chemical, commit
`1e328684`):

1. **Placement:** directly in `Assets/StreamingAssets/Data/` root — the integrity
   selftest enumerates **TopDirectoryOnly**
   (`CatalogIntegrityValidator.cs:529-530`); snake_case filename; object root;
   `"schema_version": 1` (presence enforced for object roots, `:720-725`).
2. **Definition/reference keys:** ids under `DefinitionKeys` (`id`, `route_id`,
   `trait_id`, …) auto-register (`:179-220`); outbound refs under `ReferenceKeys` are
   strict (`:226-263`). Beware `VocabularyKeys` (e.g. `tags`) — skipped wholesale.
3. **New id prefixes:** `module_`, `disaster_`, plain `route_`, `legacy_` are NOT in
   `IdPrefixes` today (`:76-170`; `trade_`, `trait_`, `weather_gate_`, `scav_route_`
   are). Each new prefix must be appended with a plan comment (the array's own
   instruction, `:75`).
4. **A `routes` top-level array auto-activates the route validator** (self-route,
   duplicate from→to — `:777-817`); precedent `caravan_trade_routes.json` uses
   `route_id`/`faction_id`.
5. **Loader:** in `Assets/Ashfall.Core`, hardcoding the literal filename (required by
   `ContentUtilizationScanner.VerifyConsumersInSource` — GAMEPLAY_CONSUMED is downgraded
   to OPTIONAL unless the filename literally appears in `.cs` source,
   `ContentUtilizationScanner.cs:1295-1353`); throws on missing/duplicate ids.
6. **Utilization registration:** `loaderPatterns` (`:242-389`) + `consumerMap`
   (`:588-786`) entries in `ContentUtilizationScanner.cs` — consumer entries are what
   grant GAMEPLAY_CONSUMED (`:1264-1267`); new ORPHANs absent from
   `artifacts/content-utilization-baseline.json` fail the gate
   (`ContentUtilizationGate.cs:100-115`) — regenerate the baseline when classifications
   change intentionally.
7. **Diagnostics are order-stable** (ordinal file sort + insertion-order dedup —
   `:551,574-577`); scanner output stabilized via `ContentUtilizationGraph.Stabilize()`.

### E.2 RNG strategy (plan §2.4 → house mechanism)

House facility: `CampaignRngStream` — dot-namespaced stream ids
(`CampaignStreamIds.Expedition = "expedition"`, `Weather = "weather"` exists unused),
seed derivation `masterSeed*31337 + StableHash.Of(streamId)*1009 + day*37 +
actionIndex`, `Fork(day, actionIndex)`, `CapturePositions`/`RestorePositions` for
save-replay (`Assets/Ashfall.Core/Random/CampaignRngStream.cs`). The plan's preferred
streams (`vehicle.module_install`, `weather.macro_pattern`, `trade.ambush`,
`succession.inheritance`, …) map 1:1 onto this — add the constants, never new magic
seed constants. Existing offenders to not imitate: expedition host `7071/7072`,
weather `DemoSeed 1234` (not campaign-derived — see F.4).

### E.3 UI conventions (closure rules the four panels must satisfy)

1. **Lifecycle:** `IBindablePanel` (IsBound/Unbind); reference shape =
   `SumpFloodingPanel`/`SlurryDewateringSumpPanel` (unsubscribe-before-resubscribe,
   `_ExitTree`→Unbind, Escape consumes + closes; `PanelBindLifecycleSelfTest` codifies
   15 gates). `Bind(object?)` alone proves nothing (UI-22).
2. **Wiring contract is six-part:** descriptor in `PanelRegistryBootstrap.RegisterAll`
   (group + maturity) → `ConfigureActions` in `Main.PlayerSurfaces` (bind/open/close) →
   dispatch works through `OpenPlayerPanel` → dashboard `AddNavButton` entry point →
   Expanded-group routing where applicable → Escape/overlay lifecycle. Registering a
   subset manufactures UI-09 dead routes; register all-or-shelve (`PanelMaturity.Prototype`).
3. **Command flow:** panel → host session command (typed args, never hardcoded payload
   constants — UI-12) → `LastEvent`/result rendered as the only feedback (no fixture
   success, UI-19/21); state → blocker → cost → consequence on every action, disabled
   controls carry explanatory tooltips (`AshfallUiHelpers.MakeDisabledButton`).
4. **Focus/accessibility (UI-23):** `AshfallDataGrid` rows are mouse-only today — new
   grids must add focusable keyboard selection; use `AshfallFocusPolicy` helpers; color
   ONLY via `Theme` tuples (Hex constants drift); `Dim` fails contrast — use
   `Muted`/`Pale`/`Warm` for body text.
5. **No canvas map component exists** (no `_Draw`/plot in MapPanel/MapAtlasPanel) —
   TradeRouteUI's map view is either a custom-draw Control (new) or grid-based; avoid
   the MapAtlas quadrant selection bug (UI-04: carry stable ids, not row indices).
6. GarageUI/WeatherUI extend existing surfaces — the UI audit's open findings for
   WeatherPanel (fixture branches) and dashboards apply; do not grow the 19-shell
   hardcoded-console family (UI-05).

### E.4 Save architecture

Four new sections per plan §12: `modular_vehicles`, `macro_weather`, `trade_routes`,
`succession_legacy` — each via the E.1-of-D.2 recipe (registry metadata + filename
whitelist + optional schema version + Setup/Save/Flush triad), each with
empty/default old-save migration (plan §14). Envelope order follows
`SaveSectionRegistry.All` order; restore order per plan §13 adapted to actual
composition (succession after memorial/fate; trade after world/weather; modular
vehicles after expedition).

---

## F. Documented divergences from the plan text (all material — decide in Phase B)

1. **Plan-number collision.** `Next-steps-plans/` already numbers 158 = Disaster
   Emergency Response, 159 = Shelter Governance, 160 = Expedition Colony, and the
   trade-route design on file is Plan 192. This stream follows the master plan doc's
   numbering (158 vehicles / 159 macro weather / 160 trade / 161 succession); the
   collision is documentation-only but handoffs must cite the master plan doc, not bare
   plan numbers.
2. **"Reuse the same route/pathing graph used by `ExpeditionSystem`" (§2.2) is not
   literally satisfiable** — expeditions deliberately use scalar `distanceTicks`, not
   the map graph (`ExpeditionSystem.cs:207-213`). Canonical world topology =
   `WastelandMapSystem` (BFS over discovered nodes). Resolution: TradeRouteSystem paths
   over `WastelandMapSystem` via a new thin `IWorldRoutePlanner` facade; expedition
   travel math stays untouched; route cost ↔ tick conversion uses the host
   `KmPerTravelTick` constant. This honors the plan's *intent* (one topology) without
   rewriting expedition travel.
3. **Vehicle identity.** Plan assumes stable per-vehicle modular state; reality is
   one-instance-per-catalog-id with no instance layer. Resolution: keyed module state
   under the existing vehicle id (dictionary `vehicleId → slots`), with module
   *instances* identified by `ProceduralItemInstance`-pattern deterministic sequence
   ids where physical detach/re-attach identity matters.
4. **Weather seed is not campaign-derived** (`DemoSeed = 1234`). Enabling Plan 159 must
   not shift existing weather rolls (plan §2.4's "enabling trade must not shift
   weather" applies symmetrically). Resolution: MacroWeatherSystem uses a NEW
   `weather.macro_*` keyed stream; `WeatherSystem`'s own seeding is left untouched this
   stream (fixing it to the campaign seed is a separate, save-compat-sensitive task).
5. **"Create `ShelterIntegritySystem` only if none exists" (§6.9)** — none exists as a
   single authority, but `WeatherHardeningSystem` is the de-facto structural-stress
   owner AND is never ticked in production. Resolution: extend
   `WeatherHardeningSystem` with macro-stress inputs; wiring its production tick is a
   small independent fix to land with Plan 159 host wiring.
6. **Pricing owner (§7.12)** — four price models coexist. Recommendation:
   `MarketSystem` extension (demand/scarcity/clamps already match the plan's formula
   shape); caravan-local modifiers (route reliability, faction standing) enter as
   explicit multipliers on `MarketSystem.GetPrice`, not a fifth price engine.
7. **Leader authority (§8.2)** — `LeadershipSystem` already owns the leader field and
   vacates on death. SuccessionSystem observes `SurvivorFateSystem.OnSurvivorFate` +
   composes with `LeadershipSystem.DesignateLeader`-style commands; it does not become
   the leader registry.
8. **Greenhouse coupling (§6.10)** enters through `TickDay`'s existing
   `ashContaminationRate` + a new optional stress parameter, not a parallel crop model.

---

## G. Baseline gates

Recorded 2026-09-05/06 on `feat/asset-pipeline-flagship` with concurrent streams active
(45 modified / 92 untracked foreign files — the earlier UI-audit "8 test errors" state
has since been repaired by those streams):

| Gate | Result | Notes |
|---|---|---|
| `dotnet build Ashfall.Core.Tests` | **PASS** | 0 errors, 5 analyzer warnings (xUnit style, pre-existing across streams) |
| `dotnet test Ashfall.Core.Tests` | *(recorded below at run completion)* | |
| `dotnet build Ashfall.csproj` (host) | *(recorded below)* | |
| `--data-integrity-selftest` | *(recorded below)* | |
| `--content-utilization-selftest` | *(recorded below)* | |
| `--scene-binding-selftest` | *(recorded below)* | |

*(Baseline completion results appended below after the background run finished — see
G.1.)*

### G.1 Final baseline results

(filled at commit time)

---

**Phase A exit:** authority map complete; integration seams documented (E.1–E.4);
divergences F.1–F.8 recorded with resolutions for Phase B. Production edits may begin
with Phase B (shared contracts: RNG stream constants, save-section skeleton, planner
facade, modifier/event interfaces) per master plan §49.
