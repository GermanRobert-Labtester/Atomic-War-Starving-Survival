# C1 — Flagship Integration Plan: Economy, Weather & Shelter Feedback Loops

> **Output:** `docs/plans/C1_planintegration.md`
>
> **Source baseline:** Plan 14 — Economy, Weather & Shelter Loop: Deepening the Daily Texture
> (`Next-steps-plans/shipped_to_chat/Plan_14_Economy_Weather_Shelter_Loop.md`)
>
> **Predecessor:** Plan 13 — economy goods, trapping, and weather-crisis events
>
> **Execution objective:** Convert six partially connected feature groups into production-ready, deterministic gameplay loops with stable save/load contracts, data validation, host/UI surfaces, balance verification, and regression gates.
>
> **Primary implementation sequence:** 14A + 14B → 14C → 14D → 14E → 14F
>
> **Repository rule:** Do not begin broad refactors while implementing this plan. Prefer narrow additive seams, explicit adapters, deterministic Core behavior, and host/UI consumption through stable read models.

---

## 0. Mission and Completion Standard

Plan 14 is not a content-addition pass. It is an **integration closure pass**.

The repository already contains several systems whose data and mechanics exist independently but do not yet form complete player-facing loops:

- weather forecasts exist but do not drive player preparation;
- regional supply metadata exists but does not materially affect prices;
- caravans have regional identity but weather cannot interrupt their routes;
- shelter decor exists but trapping cannot feed it;
- the `war_dog_kennel` panel exists without a backing Core system;
- crisis weather exists but the player has no high-cost strategic countermeasure.

The implementation is complete only when each feature forms a verified loop:

```text
DATA / WORLD STATE
      ↓
CORE SYSTEM DECISION
      ↓
CROSS-SYSTEM EFFECT
      ↓
READ MODEL / HOST BRIDGE
      ↓
PLAYER-FACING UI
      ↓
PLAYER ACTION
      ↓
PERSISTED STATE
      ↓
NEXT-DAY CONSEQUENCE
      ↓
AUTOMATED VERIFICATION
```

A task is **not complete** if only its JSON exists, only its Core class compiles, only the UI displays placeholder information, or only unit tests pass in isolation.

### Global definition of done

Every implemented workstream must satisfy all applicable requirements below:

1. Core logic is deterministic and testable without the UI.
2. Every new data authority loads through the existing catalog/data pipeline.
3. Every new persisted system has a stable DTO and save-section registration.
4. Invalid IDs or malformed values fail through integrity validation rather than silently degrading.
5. Host/UI layers consume read models or explicit events; they must not recalculate business logic.
6. Existing public APIs are preserved where practical; new overloads or adapters are preferred over disruptive rewrites.
7. Every player-visible state has a meaningful empty, blocked, cooldown, unavailable, or failure presentation.
8. Cross-system interactions are covered by integration tests, not only isolated xUnit tests.
9. Balance simulation confirms that new modifiers produce pressure without creating permanent runaway states.
10. The full repository verification gate remains green before the task is marked complete.

---

## 1. Evidence Baseline and Existing Integration Gaps

The implementation plan assumes the following repository baseline.

| Existing system / authority | Current capability | Integration gap to close |
|---|---|---|
| `WeatherStationSystem` | Install, calibrate, generate 3-day forecast, route-safety, 0.7–0.9 accuracy | Forecast lacks decision-grade crisis prediction and player-facing action |
| `WeatherIntelligenceCoordinator` | Owns Station + Orbital and produces `WeatherIntelligenceReadModel` | Read model is not yet the complete weather decision surface |
| `SkyLayerArmorSystem` | Five armor tiers, attenuation, kinetic impact | No cloud-seeding partial mitigation path |
| `PowerGridSystem` | Generator, battery, brownout, room allocation | Already exposed to EMP weather; no new ownership change required |
| `ShelterDecorSystem` | Room/slot assignment, morale modifiers, memorial bridge, save section | No trophy asset pipeline from trapping |
| `TravelingCaravanSystem` | Regional specialty stock and origin regions | No weather embargo or blocked-route state |
| `MarketSystem` | Demand, volatility, barter, ledger | No regional price differentiation and no embargo shock |
| `WildlifeTrappingSystem` | Bait, quarry species, hides, skill | No trophy recipe bridge |
| `economy_goods.json` | 31 goods with `regionalSupply` | Field is mechanically unused |
| `PanelRegistryBootstrap` | Registers `weather_forecast`, `economy_detail`, `trade`, `shelter_decor`, `traveling_caravan`, `war_dog_kennel` | Several panels lack authoritative data flow |
| `SaveSectionRegistry` | Caravan, power grid, shelter decor sections | Missing embargo, cloud-seeding, kennel persistence |

### Integration doctrine

All six tasks must follow four architectural boundaries:

**A. Core owns rules.**
Embargo activation, price multipliers, forecast confidence, cloud-seeding outcomes, trophy morale, kennel training, and dog bonuses belong in Core.

**B. Data owns tunables.**
Weather-to-embargo mappings, regional price coefficients, cloud-seeding recipes, kennel definitions, and catalog IDs should be externalized where the source task explicitly calls for JSON authority.

**C. Host owns orchestration.**
The host may connect systems, translate events into UI notifications, or schedule daily updates, but it should not contain hidden copies of Core formulas.

**D. UI owns presentation only.**
Panels render read models and action availability. UI code must not independently decide whether a route is embargoed, whether a crisis exists, or what a regional price should be.

---

## 1.5 Current-Evidence Corrections — 2026-09-15

Recorded from direct source inspection before any edits, per non-negotiable rule 7
("use current evidence"). Where this document and current source disagree, the
corrections below govern.

### Corrected file paths (Phase 15 map)

Several systems named in this plan live at different paths than the Phase 15 map
assumes. The actual locations, verified 2026-09-15:

| System | Actual path |
|---|---|
| `TravelingCaravanSystem` | `Assets/Ashfall.Core/TravelingCaravanSystem.cs` (Core root, not `Economy/`) |
| `WeatherStationSystem` | `Assets/Ashfall.Core/WeatherStationSystem.cs` (Core root, not `World/`) |
| `WildlifeTrappingSystem` | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` (Core root, not `Economy/`) |
| `SkyLayerArmorSystem` | `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` (`Shelter/`, not `World/`) |
| `WeatherIntelligenceCoordinator` | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` |
| `ExpeditionSystem` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `MarketSystem` | `Assets/Ashfall.Core/Economy/MarketSystem.cs` |
| `ShelterDecorSystem` | `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` |

None of the four planned data files (`trade_embargoes.json`, `regional_prices.json`,
`cloud_seeding.json`, `kennel.json`) exists yet. None of the four planned Core
files (`TradeEmbargoSystem`, `RegionalPriceAtlas`, `CloudSeedingSystem`,
`KennelSystem`) exists yet.

### 14A/14B composition constraint — Plan 212 weather shocks already exist

`Assets/Ashfall.Core/Economy/EconomyWeatherShockRules.cs` (Plan 212, ACCEPTED)
already maps severe weather to market shocks through the canonical
`MarketSystem.ApplyShock` path:

- blizzard-class (severity ≥ 2.0): `food` shortage, 1500bp, 3 days, source `weather_blizzard`;
- storm class (severity ≥ 1.5): `food` shortage, 1000bp, 2 days, source `weather_storm`;
- calmer weather: no shock.

**Consequence for 14A:** embargo price shocks must compose as ONE additional
factor in the canonical price equation (§P0.3) after the Plan 212 shock path.
`TradeEmbargoSystem` must not create a second shock pipeline into `MarketSystem`;
it exposes a price modifier that the single market pricing path applies, and the
embargo/Plan 212 factors must not double-apply to the same category.

### 14F authority constraint — Plan 174 companion animals already exist

`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` (Plan 174,
`PLANS-174-177-FLAGSHIP-SURVIVOR-WORLD`, HANDED_OFF) is the shipped companion
authority. It already owns:

- companion roles (`CompanionRole`: Unassigned/Guard/Pack/Morale — one role at a time);
- training levels (Untrained→Expert), bond, deterministic training/upkeep;
- per-species profiles with daily food and preferred/fallback food items;
- sickness states with medical-host treatment;
- bounded guard benefit routed to `DefenseSystem`/`PerimeterDefenseSystem`,
  bounded pack benefit routed to `ExpeditionSystem` cargo, and reported-only
  morale support left to the morale authorities.

**Consequence for 14F:** `KennelSystem` must be designed as a kennel-focused
extension that routes through the Plan 174 authority split (dog-specific skills,
guard-alert event choice, companion morale in an assigned room) — it must NOT
introduce a second companion/animal state machine, a second food-consumption
path, or a second expedition/guard modifier pipeline. Where `CompanionAnimalSystem`
already provides an equivalent concept (role assignment, daily food, training
progression), 14F extends or consumes it. The WORKTREE_OWNERSHIP.md ledger for
the 174–177 batch additionally names "KennelUI" as a planned presentation wave.

### Panel status — `war_dog_kennel` is a shelved prototype

`Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` registers `war_dog_kennel` as
`PanelMaturity.Prototype`, and `docs/forensics/UI_PANELS_UX_INVENTORY.md`
records it as "Prototype — shelved; player route rejected". Before the 14F
presentation wave invests in this panel, the shelving decision must be revisited
with the foreman/user; the Core-side 14F work is not blocked by it.

### Governance status

- Current batch `DISTRESS-SIGNALS-9-12` is COMPLETE and presented for
  acceptance; no active ownership claim conflicts with the paths this plan
  touches. Claims will be recorded in `WORKTREE_OWNERSHIP.md` /
  `INTEGRATION_PLANS.md` per repository cadence before edits.
- The worktree carries extensive unrelated uncommitted changes; they are
  preserved untouched. Builders touch only claimed paths.

---

## 2. Cross-Cutting Preflight — Phase P0

Complete this phase before implementing 14A.

### P0.1 Repository reconnaissance

Read and map the exact current signatures and ownership boundaries for:

- `TravelingCaravanSystem.DailyTick()`
- `MarketSystem.TickDay()`
- `MarketSystem.GetPrice()`
- `MarketSystem.Barter(...)`
- `GoodsCatalog`
- `WeatherStationSystem.GenerateForecast()`
- `WeatherIntelligenceCoordinator.TickDay(...)`
- `WeatherIntelligenceReadModel`
- `WeatherSystem`
- `SkyLayerArmorSystem`
- `ShelterDecorSystem.Assign(...)`
- `ShelterDecorSystem.GetRoomMoraleDelta(...)`
- `WildlifeTrappingSystem.PreserveHide(...)`
- `ExpeditionSystem` start/dispatch path
- event-resolution path for hatch-breach choices
- `PanelRegistryBootstrap`
- `SaveSectionRegistry`
- existing data loader / validation / selftest registration mechanisms.

### P0.2 Produce an integration map before edits

Record:

- constructor dependencies;
- setter-injection patterns already used in Core;
- event/delegate conventions;
- save DTO naming conventions;
- JSON deserialization conventions;
- catalog ID validation conventions;
- daily-tick ordering;
- host composition root registration sequence;
- UI panel read-model binding sequence;
- deterministic RNG ownership.

The objective is to prevent duplicate service ownership or two systems independently applying the same modifier.

### P0.3 Establish modifier-order policy

Before changing prices, define one canonical price equation.

Recommended order:

```text
base item price
× market demand / volatility
× regional price modifier
× active embargo shock
× transaction-specific modifier
= final quoted price
```

Requirements:

- apply each modifier exactly once;
- clamp only at an existing canonical price boundary;
- do not round between intermediate stages;
- ledger records should retain enough information to audit the final price;
- tests must prove order is stable.

### P0.4 Establish day-tick order

Document the current daily scheduler and insert new work without creating feedback-order ambiguity.

Target logical ordering:

```text
1. World/weather advances or scheduled weather becomes authoritative
2. Weather intelligence generates/updates forecast
3. Crisis prediction refreshes
4. Trade embargo state evaluates current weather
5. Caravan movement evaluates embargo route blocking
6. Market day tick applies/decays economic shocks
7. Cloud-seeding cooldown advances
8. Kennel daily state advances
9. Shelter/decor passive effects resolve
10. Daily briefing/read models are rebuilt
```

If repository order differs, preserve the existing scheduler architecture but achieve equivalent causality.

### P0.5 Save-version decision

Determine whether the save framework supports:

- optional new sections with defaults; or
- explicit save schema version/migration.

New systems must load older saves with safe defaults:

```text
TradeEmbargoSystem: no persisted shock / no stale block
CloudSeedingSystem: not installed, cooldown 0
KennelSystem: kennel not built, no dog
RegionalPriceAtlas: reconstructible from data unless runtime state is genuinely required
```

Do not persist derived values that can be safely recomputed from authoritative data and world state unless replay fidelity requires them.

### P0.6 Baseline verification snapshot

Run and record baseline before modifications:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

If any baseline failure exists, classify it before editing so Plan 14 does not absorb unrelated regressions.

**P0 exit gate:** architecture map complete, price order documented, daily tick order documented, save strategy chosen, baseline results recorded.

---

# PHASE 1 — ECONOMY COUPLING
## Task 14A — Commodity Embargoes & Weather-Restricted Caravan Trade

### 14A objective

Make adverse weather alter the survival economy in two distinct but coordinated ways:

1. **logistics disruption:** route progress may be blocked or slowed;
2. **market disruption:** affected goods temporarily become more expensive.

The player should be able to attribute the change to a visible weather event instead of experiencing unexplained price randomness.

### 14A.1 Contract reconnaissance

Inspect:

- caravan route state and how `originRegion` is stored;
- whether a caravan can have no region;
- whether route advancement is integer-node based or time/progress based;
- how `DailyTick()` emits notifications;
- whether `MarketSystem.TickDay()` already has temporary shocks;
- how price history or ledger entries are stored;
- `WeatherKind` enum coverage;
- Plan 13C weather event IDs and `RequireWeather` mappings;
- `HardcoreEconomyTuning` for existing clamps and decay constants.

Do not implement until region IDs and weather kinds have one canonical representation.

### 14A.2 Create `TradeEmbargoSystem`

Create:

`Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs`

Required public concepts:

```csharp
EmbargoRule
- weatherKind
- affectedRegions[]
- priceMultiplier
- caravanBlocked
```

Required behavior:

- `RegisterRule(EmbargoRule rule)`
- `IsEmbargoActive(WeatherKind current)`
- `GetPriceModifier(string region, WeatherKind current)`
- `IsRouteBlocked(string region, WeatherKind current)`
- `CaptureState()`
- `RestoreState(...)`

Recommended additional read-only helpers:

- `GetActiveRules(WeatherKind current)`
- `GetAffectedRegions(WeatherKind current)`
- `GetEmbargoSummary(WeatherKind current)`
- `ValidateRule(EmbargoRule rule)`

Rules:

- null/unknown regions must not crash;
- no active rule returns neutral multiplier `1.0`;
- duplicate rule semantics must be explicit: reject duplicates or combine by a documented rule;
- avoid hidden RNG. If later randomness is added, inject `ISeededRng`.

### 14A.3 Author `trade_embargoes.json`

Create:

`Assets/StreamingAssets/Data/trade_embargoes.json`

Seed with 8–12 rules, including the source-required cases:

| Weather | Route effect | Economic effect |
|---|---|---|
| `FalloutStorm` | all routes blocked | medical +50% |
| `BlackRain` | flotilla blocked | water +80% |
| `EMPStorm` | foundry blocked | electronics +100% |
| `Blizzard` | traplines blocked | food +40% |
| `AcidSnow` | routes slowed, not fully blocked | tools +30% |
| `BioFog` | greenhouse blocked | seeds +60% |
| `GlassStorm` | settlement blocked | materials +40% |
| `RadHail` | all routes blocked for one day | all prices +25% |

Data requirements:

- every `weatherKind` resolves to a real enum/value;
- every region resolves to a canonical region;
- multiplier must be finite and above zero;
- route effect must be explicit;
- any duration field must be non-negative;
- unknown item category references fail integrity validation.

### 14A.4 Decide category-target representation

The source specifies category-specific price increases while the minimal DTO only names `priceMultiplier`.

Resolve this explicitly instead of burying category selection in code.

Preferred schema extension:

```json
{
  "weatherKind": "FalloutStorm",
  "affectedRegions": ["*"],
  "affectedItemTags": ["medical"],
  "priceMultiplier": 1.50,
  "caravanBlocked": true
}
```

If the current goods catalog does not support tags/categories cleanly, use explicit item IDs in data rather than string matching item names.

### 14A.5 Caravan integration

Add `SetEmbargoSystem(TradeEmbargoSystem)` or the repository-equivalent injection seam.

Modify `DailyTick()` so each caravan:

1. determines its origin region;
2. queries current embargo route state;
3. if blocked:
   - does not advance route;
   - sets `embargoBlocked = true`;
   - emits one deduplicated `OnCaravanEmbargoed` transition event;
4. if no longer blocked:
   - clears `embargoBlocked`;
   - resumes normal movement;
   - optionally emits an unblocked/resumed event if existing UI patterns benefit from it.

Do not repeatedly fire "blocked" notifications every tick unless current event semantics intentionally represent daily status.

### 14A.6 Slow-route semantics for non-blocking weather

`AcidSnow` requires slowdown rather than blocking.

Introduce one clear mechanism:

- route progress multiplier; or
- additional travel-day cost.

Avoid fake blocking/unblocking oscillation.

Test 0%, 50%, and neutral progression boundaries.

### 14A.7 Market integration

Add a single embargo modifier path.

Requirements:

- embargo shock is temporary;
- it decays over N days after weather clears;
- decay cannot invert below neutral;
- a new shock during decay updates state deterministically;
- price history remains inspectable;
- regional pricing from 14B can compose without double application.

Recommended state model:

```text
active weather shock
→ captured peak multiplier
→ remaining decay days
→ current decayed multiplier
```

### 14A.8 Economic read model

Add/read model fields sufficient for `economy_detail`:

- active weather name;
- embargo active yes/no;
- affected regions;
- blocked caravan routes;
- affected goods/categories;
- current multipliers;
- days remaining in decaying shock;
- explanatory copy key / reason ID.

The panel must not call raw price math for each label if a suitable economy read model already exists.

### 14A.9 UI behavior

In `economy_detail`:

- active embargo banner;
- affected region list;
- price impact badges;
- blocked-route indicator;
- neutral state when none is active;
- tooltip/explanation linking the price spike to weather;
- no color-only communication.

In `traveling_caravan`:

- blocked caravans show weather reason;
- route progress visibly pauses or slows;
- stale blocked state clears immediately when the authoritative embargo state clears.

### 14A.10 Persistence

Persist only runtime state needed for continuity:

- active/decaying shock state if it cannot be reconstructed exactly;
- route blocked state only if it represents a durable transition rather than a derived current-weather value.

Register any new section in `SaveSectionRegistry`.

Round-trip invariants:

```text
price modifier before save == price modifier after load
decay remaining before save == after load
caravan does not gain a free movement day during reload
old save with no embargo section loads safely
```

### 14A.11 Tests

Minimum unit coverage:

1. neutral weather → no embargo;
2. fallout storm activates all-region route block;
3. region-specific route block applies only to the specified region;
4. correct affected good/category multiplier;
5. unaffected good remains neutral;
6. weather clears → route resumes;
7. shock decay reaches exactly neutral;
8. reactivation during decay is deterministic;
9. save/load round trip;
10. malformed rule validation;
11. deterministic results from same state;
12. slow-route weather changes travel progress without blocking.

### 14A.12 Balance simulation

Use `ashfall-balance-sim` to run repeated weather/economy sequences.

Measure:

- average days per 30-day window with at least one route blocked;
- median and p95 price multiplier by essential category;
- time-to-recovery after weather clears;
- frequency of simultaneous essential-good spikes;
- whether the player can enter a permanent scarcity spiral solely due to embargo mechanics.

Target principle: **weather creates acute scarcity, not irreversible economic collapse.**

### 14A acceptance gate

Task 14A passes only if:

- weather visibly blocks/slows the correct caravans;
- prices respond to the same authoritative rule set;
- shocks recover;
- data integrity catches invalid rules;
- save/load cannot reset or duplicate the effect;
- UI explains the cause;
- full baseline verification remains green.

---

## Task 14B — Regional Price Atlas and Economy Panel Regional Map

### 14B objective

Turn `regionalSupply` from inert metadata into a stable economic identity for each region.

The result should create a planning question:

> Where should I buy this good, and what tradeoff does the route/weather situation impose?

### 14B.1 Contract reconnaissance

Inspect:

- `GoodsCatalog` parsing of `regionalSupply`;
- current item IDs vs human-readable names;
- `MarketSystem.GetPrice()` and rounding;
- `Barter(...)` price source;
- caravan origin/destination semantics;
- existing economy panel table/list architecture.

### 14B.2 Create `RegionalPriceAtlas`

Create:

`Assets/Ashfall.Core/Economy/RegionalPriceAtlas.cs`

Required DTO:

```csharp
RegionalPriceEntry
- region
- itemId
- basePriceModifier
- scarcityProfile
```

Required API:

- `GetRegionalPrice(string itemId, string region)`
- `GetBestRegion(string itemId)`
- `GetRegionalGoods(string region)`
- `CaptureState()` / `RestoreState()` only if runtime state exists
- construction from `regionalSupply` / companion authority data

Recommended API separation:

- `GetModifier(itemId, region)` → multiplier only
- `GetRegionalPrice(basePrice, itemId, region)` → resolved price
- `TryGetRegionalEntry(...)`
- `GetRegionsForItem(itemId)`

### 14B.3 Resolve data-authority strategy

Choose exactly one:

**Option A — extend `economy_goods.json`** if each good naturally owns its regional coefficients.

**Option B — add `regional_prices.json`** if price geography needs independent iteration.

Prefer the option most consistent with current loader conventions.

Do not maintain the same coefficient in two files.

### 14B.4 Required regional profiles

Implement source baseline:

- **flotilla:** brine pipe, desal membrane, RO membrane around `0.7x`; seeds around `1.5x`;
- **foundry:** mechanical parts, electronic scrap, chemicals around `0.8x`; food around `1.3x`;
- **greenhouse:** seeds, herbal tea, canned food around `0.8x`; tools around `1.4x`;
- **traplines:** cooked meat, leather strap, frostbite salve around `0.7x`; electronics around `1.5x`;
- **settlement:** medical kit, anti-rad, solar cell around `0.9x`, with a generally balanced profile.

The original task describes a broad `0.7x–1.3x` range but also includes `1.4x` and `1.5x` examples. Treat the concrete examples as intentional outliers and make validation bounds broad enough to permit them without allowing absurd values.

Recommended integrity bound:

```text
0.50 <= basePriceModifier <= 2.00
```

Balance targets can remain narrower.

### 14B.5 Market integration

Add:

```csharp
GetPrice(itemId, region)
```

or repository-equivalent overload.

Requirements:

- old callers without region preserve current behavior;
- region-aware callers apply the atlas exactly once;
- embargo modifier from 14A composes after regional baseline;
- barter uses the region relevant to the transaction;
- ledger can expose region and modifier where useful.

### 14B.6 Caravan integration

Caravan buy/sell price uses `originRegion`.

The UI should be able to display statements such as:

```text
Foundry caravan
Mechanical parts: 20% cheaper than neutral market
Current weather embargo: +30%
Net quote: ...
```

Do not flatten the two concepts. Regional identity is persistent; embargo shock is temporary.

### 14B.7 Regional price heat map

`economy_detail` should provide a 5-region comparison surface.

Minimum cell state:

- good name;
- regional modifier;
- current resolved price if appropriate;
- cheap / neutral / expensive semantic label;
- active weather shock marker where applicable.

Accessibility:

- color may reinforce meaning but cannot be the only encoding;
- selected row/column must be keyboard navigable;
- textual percentage or semantic label must remain visible/accessible.

### 14B.8 Read model optimization

Do not let the panel repeatedly run large catalog scans each frame.

Build a read model on:

- panel open;
- economy day tick;
- relevant weather/embargo transition;
- explicit refresh.

Use immutable/read-only rows if that matches current project patterns.

### 14B.9 Integrity validation

Verify:

- every `regionalSupply` region exists;
- every regional price item ID exists;
- no duplicate item-region entries;
- modifier finite and within schema bound;
- every canonical region can be enumerated;
- every UI heat-map column maps to a real region.

### 14B.10 Tests

Minimum coverage:

1. neutral item/region lookup;
2. flotilla specialty good resolves expected cheap multiplier;
3. expensive imported good resolves expected multiplier;
4. `GetBestRegion()` is deterministic;
5. cross-region barter uses correct side;
6. caravan uses its origin profile;
7. old no-region price call is backward compatible;
8. 14A embargo composes without double applying regional modifier;
9. malformed/unknown region rejected;
10. round-trip only if state is persisted;
11. heat-map read model contains all required regions.

### 14B.11 Balance simulation

Measure:

- arbitrage spread by item;
- potential profit per route;
- impact of travel time;
- impact of weather route blocks;
- whether one region dominates as best source for too many critical goods;
- whether settlement prices become irrelevant.

Target principle: **regional trade should reward planning, not create infinite-money loops.**

### 14B acceptance gate

Pass when:

- the player can identify materially different regional prices;
- caravans quote using their origin;
- the heat map matches Core results;
- embargo and regional modifiers compose correctly;
- no repeatable arbitrage exploit emerges in balance simulation.

---

## Phase 1 Combined Economy Integration Gate

14A and 14B are implemented as a pair because they share `MarketSystem`, caravan state, economy panel data, and price composition.

Required combined scenario:

```text
1. Spawn/resolve a foundry caravan.
2. Quote mechanical parts under neutral weather.
3. Confirm foundry regional discount.
4. Trigger EMPStorm.
5. Confirm foundry route becomes blocked.
6. Confirm electronics/economic shock applies.
7. Confirm UI explains both persistent regional price identity and temporary embargo.
8. Clear weather.
9. Confirm route resumes.
10. Confirm embargo price shock decays.
11. Confirm regional discount remains.
12. Save/load at multiple points and verify identical continuation.
```

Do not start 14C until this scenario is green.

---

# PHASE 2 — WEATHER INTELLIGENCE PAYOFF
## Task 14C — Crisis Prediction from the Weather Station

### 14C objective

Convert the weather station from passive forecast infrastructure into a decision system that warns the player of a future crisis with imperfect confidence.

### 14C.1 Reconnaissance

Read:

- `WeatherStationSystem.GenerateForecast()`;
- forecast DTOs and accuracy model;
- `WeatherIntelligenceReadModel`;
- `WeatherIntelligenceCoordinator.TickDay()`;
- event-condition model used by Plan 13C;
- `RequireWeather` representation;
- daily briefing builder.

Confirm how future scheduled weather is represented. Prediction must use the forecast system, not peek directly at hidden future truth in a way that defeats forecast uncertainty.

### 14C.2 Extend `WeatherIntelligenceReadModel`

Add:

- `predictedCrisisEventId`
- `predictedCrisisDay`
- `predictedCrisisConfidence`
- `crisisPreparationAdvice`
- `predictedWeatherKind`

Recommended robustness additions:

- `hasPredictedCrisis`
- `daysUntilPredictedCrisis`
- `predictionSource`
- `isStationCalibrated`

Prefer nullable/optional semantics consistent with existing C# project conventions rather than magic empty strings.

### 14C.3 Crisis condition index

Avoid scanning arbitrary raw `events.json` on every UI refresh.

Create or reuse a pre-parsed mapping:

```text
WeatherKind → eligible crisis event IDs + metadata
```

This may be constructed when catalogs load.

Validate that all referenced crisis event IDs exist.

### 14C.4 `PredictCrisis(int currentDay)`

Required logic:

1. read generated forecast horizon;
2. inspect each forecast day;
3. map forecast weather to crisis-capable events;
4. calculate confidence from:
   - station accuracy;
   - forecast distance;
   - any existing forecast confidence;
5. choose a deterministic prediction if multiple events match;
6. expose null/no-prediction when confidence or condition does not qualify.

Suggested confidence concept:

```text
predictionConfidence =
    stationAccuracy
  × forecastDayDistanceFactor
  × forecastStateConfidence
```

Do not hard-code a second competing station-accuracy curve if one already exists.

### 14C.5 Calibration behavior

Source intent:

- station accuracy improves from approximately 0.7 to 0.9;
- shorter-horizon predictions are more reliable;
- uncalibrated UI shows unknown/`???`.

Tests must prove calibration changes the information quality and not merely the visual label.

### 14C.6 Preparation advice

Advice should be data-driven or mapped by crisis/weather type.

Examples from source intent:

- stock water;
- brace the hatch;
- protect electronics;
- reinforce ceiling/armor;
- prepare medical supplies.

Avoid free-form logic in UI code.

### 14C.7 Coordinator integration

`WeatherIntelligenceCoordinator.TickDay()`:

1. generates forecast;
2. invokes crisis prediction;
3. rebuilds read model;
4. emits `OnCrisisPredicted` only on meaningful prediction transition.

Deduplicate alerts:

- same crisis + same day should not spam daily;
- confidence changes may update UI silently unless threshold crossing is meaningful;
- changed crisis/day may emit a new alert.

### 14C.8 Weather forecast panel

Display:

- 3-day forecast;
- weather icon;
- confidence;
- route-safety state;
- crisis warning;
- predicted day;
- preparation advice;
- calibrated/uncalibrated state.

The source asks for crisis warnings highlighted in red. Preserve the visual emphasis, but also provide icon/text so warning meaning is not color-dependent.

### 14C.9 Daily briefing

`DailyBriefingReportBuilder` includes the warning when active.

Required information:

- what weather/crisis is predicted;
- when;
- confidence;
- one concise preparation recommendation.

Do not duplicate multi-paragraph UI detail into the briefing.

### 14C.10 False-positive / uncertainty semantics

Tests and UI must allow forecasts to be imperfect.

Do not make the warning a guaranteed oracle.

Define:

- confidence threshold for surfacing;
- what happens if predicted crisis never occurs;
- whether a changed forecast retracts the warning;
- how the daily briefing reports retraction or uncertainty.

### 14C.11 Tests

Minimum unit coverage:

1. calibrated station generates prediction for matching forecast;
2. uncalibrated station exposes unknown/no actionable prediction as designed;
3. nearer forecast day has higher confidence than farther equivalent day;
4. calibration increases confidence;
5. no crisis-capable weather → no prediction;
6. multiple eligible crisis events resolve deterministically;
7. prediction retracts/changes when forecast changes;
8. event fires once per meaningful transition;
9. read model serialization/save continuation if applicable;
10. deterministic results under seeded forecast inputs.

### 14C.12 Integration selftest

Implement/extend:

```bash
godot --headless --weather-forecast-selftest
```

Scenario:

```text
install station
→ calibrate
→ generate forecast
→ predict crisis
→ build read model
→ verify predicted event matches scheduled/eligible event
→ verify UI/host bridge can consume warning
```

### 14C acceptance gate

Pass when the weather station creates a trustworthy-but-imperfect player decision signal and the warning is visible in both the forecast panel and daily briefing.

---

# PHASE 3 — WEATHER STRATEGIC COUNTERPLAY
## Task 14D — Cloud-Seeding Countermeasure

### 14D objective

Provide a costly strategic action that can cancel one predicted crisis, with a meaningful failure state and cooldown.

This is the payoff for 14C: information becomes actionable.

### 14D.1 Core system

Create:

`Assets/Ashfall.Core/World/CloudSeedingSystem.cs`

Required state:

- `IsInstalled`
- `IsOnCooldown`
- `CooldownRemaining`

Required operations:

- `Install(int day)`
- `Deploy(int day, WeatherKind targetWeather)`
- `TickDay(int day)`
- `CaptureState()`
- `RestoreState(...)`

Deployment result must be explicit, not a bare bool.

Recommended result fields:

```text
status
success
failureReason
materialsConsumed
successChance
targetWeather
targetDay
cooldownApplied
partialProtectionApplied
```

### 14D.2 Installation and resource contract

The source deployment cost includes:

- `chemicals × 4`
- `fuel × 3`
- `item_desal_membrane × 1`

Before implementation, decide whether these are:

- installation cost;
- per-deployment cost;
- or both in different quantities.

The source text describes them under `Deploy`, so treat them as per-deployment unless an existing infrastructure build pattern requires a separate install recipe.

Do not silently invent consumption.

### 14D.3 Cloud-seeding item content

Add to `items.json`:

- `item_cloud_seed_dispenser`
- `item_silver_iodide_cartridge`
- `item_weather_sonde`

The source notes `item_weather_sonde` is already referenced in panel registry. Verify whether it already exists before adding it. If present, reuse rather than duplicate.

### 14D.4 Recipes

Add to `recipes.json`:

```text
craft_cloud_seed_dispenser
  workbench
  mechanical_parts ×5
  scrap_metal ×4
  electronic_scrap ×3
  copper_wire_10m_of_10m ×2

craft_silver_iodide_cartridge
  distiller
  chemicals ×3
  sulphur ×2
  fuel ×2

craft_weather_sonde
  workbench
  scrap_metal ×2
  cloth ×1
  battery ×1
```

Validate every ingredient and station ID.

### 14D.5 Deploy eligibility

Deployment must fail with a precise reason if:

- system not installed;
- on cooldown;
- target weather has no active prediction;
- required materials missing;
- target day outside allowed window;
- target weather differs from prediction.

Do not consume materials on precondition failure.

### 14D.6 Success probability

Source rule:

- 70% base;
- +10% if station calibrated;
- −20% if deployed during the crisis itself.

Requirements:

- calculate chance once;
- clamp to `[0,1]`;
- expose chance to UI before action;
- use injected deterministic RNG;
- store no hidden nondeterministic random state.

### 14D.7 Crisis cancellation ownership

Do not let `CloudSeedingSystem` directly rewrite unrelated event catalogs.

Preferred orchestration:

```text
CloudSeedingSystem validates and rolls outcome
→ coordinator / weather authority applies approved weather override
→ crisis scheduler observes changed authoritative weather or explicit cancellation token
```

If `WeatherSystem.ForceWeather(WeatherKind.Clear)` is the existing sanctioned API, use it through a narrow integration path.

Ensure cancellation applies to the intended crisis day, not accidentally to today's weather when targeting tomorrow.

### 14D.8 Safe replacement weather

Source permits `Clear` or "the next safest weather."

Define deterministic replacement selection.

Preferred:

1. `Clear` if valid;
2. otherwise canonical safe fallback;
3. never randomly select a replacement.

### 14D.9 Cooldown

Apply exactly 7 days after each actual deployment attempt, whether success or failure, unless repository design explicitly differentiates failed launch.

`TickDay()` must:

- decrement once per game day;
- never go below zero;
- survive save/load;
- not double-decrement on reload.

### 14D.10 Failure partial protection

For failed deployment during `GlassStorm` or `RadHail`:

- apply `-30%` ceiling armor damage for that day.

Integration requirements:

- effect is one-day scoped;
- it modifies damage once;
- it cannot persist after weather clears;
- read model can explain why damage was reduced.

### 14D.11 Weather panel action

When a crisis is predicted, show:

`Deploy Cloud Seeding`

Action presentation includes:

- target crisis/weather;
- target day;
- success chance;
- material cost;
- current inventory sufficiency;
- cooldown;
- unavailable reason;
- confirmation flow if current UI patterns require irreversible-action confirmation.

Disabled state must still expose the reason.

### 14D.12 Events and briefing

Emit:

- `OnCloudSeedingDeployed`

Recommended event payload:

- result;
- target;
- consumed materials;
- cooldown;
- partial mitigation.

Daily briefing next day may summarize success/failure if current report architecture supports previous-day outcomes.

### 14D.13 Persistence

Add cloud-seeding save section if needed.

Persist:

- installation state;
- cooldown remaining / last deployment day;
- any scheduled future weather override or cancellation token that cannot be reconstructed;
- one-day mitigation only if a save during the affected day must preserve it.

### 14D.14 Integrity selftest

Validate:

- items exist;
- recipes resolve;
- crafting stations exist;
- deployment material IDs exist;
- crisis/weather mapping is valid;
- cooldown non-negative;
- success-chance modifiers finite.

### 14D.15 Unit tests

Minimum coverage:

1. install;
2. deploy without install rejected;
3. missing materials rejected without consumption;
4. correct cost consumed on attempt;
5. success at deterministic RNG threshold;
6. failure at deterministic RNG threshold;
7. calibrated bonus;
8. reactive penalty;
9. cooldown applied;
10. cooldown blocks second deployment;
11. cooldown reaches zero correctly;
12. successful deployment cancels/replaces target weather/crisis;
13. failure preserves crisis;
14. failed GlassStorm/RadHail attempt applies 30% armor mitigation;
15. mitigation expires;
16. save/load;
17. old save default;
18. determinism.

### 14D.16 Headless integration selftest

Implement:

```bash
godot --headless --cloud-seeding-selftest
```

Full flow:

```text
install station
→ calibrate station
→ predict crisis
→ provision seeding materials
→ install/enable cloud-seeding
→ deploy
→ deterministic success
→ crisis weather replaced/cancelled
→ cooldown = 7
→ save/load
→ cooldown preserved
```

Also add a deterministic failure-path scenario.

### 14D.17 Balance simulation

Simulate repeated 120–180 day campaigns.

Measure:

- number of crises predicted;
- fraction where player can afford seeding;
- successful cancellations;
- resource opportunity cost;
- cooldown collisions between nearby crises;
- whether the mechanic trivializes severe weather;
- whether costs make the system effectively unusable.

Target principle: **cloud seeding should feel powerful because it is scarce, not powerful because it is free.**

### 14D acceptance gate

Pass when 14C information can drive a real 14D action, success/failure is deterministic under tests, the intended crisis changes, resources/cooldown persist, and partial mitigation is correctly scoped.

---

# PHASE 4 — TRAPPING → CRAFTING → SHELTER DECOR
## Task 14E — Trophy Mount Pipeline

### 14E objective

Turn rare animal catches into persistent shelter identity and localized morale value.

Required loop:

```text
catch rare quarry
→ preserve hide / harvest material
→ unlock/select trophy recipe
→ craft trophy
→ place trophy in eligible decor slot
→ room receives localized morale bonus
→ state persists
```

### 14E.1 Reconnaissance

Inspect:

- `WildlifeTrappingSystem.PreserveHide`;
- species ID catalog;
- hide/pelt output IDs;
- workbench crafting contract;
- `ShelterDecorSystem.Assign`;
- room IDs and slot IDs;
- morale aggregation;
- decor save DTO.

Verify whether source example `leather_strap` is actually the preserved output for the wolf. Do not force the example if current quarry data uses a more specific hide ID.

### 14E.2 Add eight trophy items

Add source-defined items:

1. `item_decor_trophy_wolf_head` — Two-Headed Steppe Wolf — +3 morale — crafting room
2. `item_decor_trophy_deer_antlers` — Wasteland Mule Deer — +2 — common room
3. `item_decor_trophy_boar_tusks` — Razorback Boar — +3 — common room
4. `item_decor_trophy_fox_pelt` — Barren Fox — +2 — sleeping quarters
5. `item_decor_trophy_beetle_carapace` — Titan Slag-Back Beetle — +2 — workshop
6. `item_decor_trophy_molerat_skull` — Tessarat Blind Mole-Rat — +1 — medical
7. `item_decor_trophy_crow_feathers` — Three-Eyed Sentry Crow — +1 — radio room
8. `item_decor_trophy_pheasant_plume` — Ash Pheasant — +1 — greenhouse

Each item:

- `type: "Decor"`
- valid trade value
- explicit morale effect
- `isEquipable: false`

Do not let generic equipment code consume these items.

### 14E.3 Trophy recipe data

Add eight recipes.

Each consumes:

- species-appropriate preserved material;
- `scrap_wood ×2`;
- `chemicals ×1`;

Station:

- `workbench`

Crafting time:

- 4–6 hours.

Requirements:

- each species maps to exactly one standard trophy recipe;
- all outputs exist;
- no recipe consumes the trophy output itself;
- no circular crafting dependency.

### 14E.4 Trap-to-trophy bridge

Add:

```csharp
GetTrophyRecipeForSpecies(string speciesId)
```

and:

```text
OnTrophyReady
```

Semantics:

- event indicates eligibility/opportunity, not automatic crafting;
- no recipe is granted if required species has no trophy mapping;
- mapping should be data-driven if the trapping catalog already supports species metadata.

Avoid hard-coding a switch statement if eight mappings naturally belong in data.

### 14E.5 Shelter decor integration

Existing `Assign(roomId, slotId, itemId)` should remain valid.

Add:

- `GetTrophySlots(string roomId)`
- `GetTrophyMoraleModifier(string itemId)`

Validation before assignment:

- item is decor/trophy;
- slot exists;
- slot accepts trophy class if slot restrictions are modeled;
- item exists in player inventory if assignment consumes/places inventory through existing semantics.

Do not create a second morale path. Trophy morale must flow through the existing room morale aggregation.

### 14E.6 Localized morale rule

Source intent is **localized**, not global.

Tests must prove:

- wolf trophy affects intended room;
- it does not add the same bonus to every room;
- moving/removing trophy updates morale exactly once;
- multiple trophies follow existing decor stacking rules.

If no stacking policy exists, define one before implementation.

### 14E.7 Decor panel

`shelter_decor`:

- trophy-eligible slots have distinct icon/label;
- empty eligible slot: "Place a trophy here";
- placed trophy shows species name;
- morale bonus displayed;
- unavailable item state explains missing crafted trophy;
- keyboard selection follows existing panel conventions.

### 14E.8 Integrity checks

Validate:

- eight item IDs unique;
- eight recipe IDs unique;
- every recipe input exists;
- every output exists;
- species IDs resolve;
- species-to-recipe mapping complete;
- room IDs resolve;
- trophy slot IDs resolve;
- morale values finite and within tuning bound.

### 14E.9 Tests

Minimum coverage:

1. preserve qualifying species material;
2. species returns expected trophy recipe;
3. invalid species returns no recipe;
4. recipe crafting consumes correct materials;
5. output trophy created;
6. trophy placed into valid slot;
7. wrong slot rejected if slot restrictions exist;
8. localized morale applied;
9. trophy removal reverses morale;
10. full trap→craft→place pipeline;
11. save/load preserves placement and morale;
12. data integrity covers all mappings.

### 14E.10 Balance simulation

Measure:

- expected campaign day of first trophy;
- expected number of trophies by day 60/120;
- maximum possible localized morale;
- overlap with other decor bonuses;
- whether common species make "rare trophy" identity too easy.

Target: trophies are meaningful identity rewards, not a primary morale exploit.

### 14E acceptance gate

Pass when a real trapping outcome can generate a real recipe opportunity, craft a valid trophy, place it through existing decor APIs, affect only the correct room, and persist.

---

# PHASE 5 — KENNEL / CANINE COMPANION SYSTEM
## Task 14F — Guard-Dog Training & Kennel System

### 14F objective

Back the existing `war_dog_kennel` panel with a Core system that participates in:

- expedition risk;
- shelter defense events;
- room morale;
- food economy;
- progression;
- persistence.

This is the largest task and is deliberately last.

**Authority constraint (see §1.5):** Plan 174 `CompanionAnimalSystem` is the
shipped companion authority. 14F extends/routes through it; it never creates a
second companion state machine, food path, or expedition/guard modifier pipeline.

### 14F.1 Reconnaissance

Inspect:

- `ExpeditionSystem` encounter-risk calculation;
- expedition assignment model;
- survivor/companion assignment conventions;
- event choice augmentation path;
- hatch-breach event IDs;
- food/inventory consumption API;
- day-based progression conventions;
- health/damage abstractions applicable to non-survivors;
- panel binding conventions;
- save registry;
- `Ecology/CompanionAnimalSystem.cs` (Plan 174) role/training/food/sickness seams.

### 14F.2 Create `KennelSystem`

Create:

`Assets/Ashfall.Core/Shelter/KennelSystem.cs`

Core DTO:

```text
CanineCompanion
- dogId
- name
- breed
- stage: puppy / juvenile / adult / veteran
- trainingLevel: 0–100
- loyalty: 0–100
- health: 0–100
- dayAcquired
```

Core state:

- `IsKennelBuilt`
- current dog or collection according to source scope
- active assignment
- training queue/progress
- active skills
- daily food obligation

The source scope describes one companion workflow. Do not prematurely build breeding/multi-dog simulation; breeding is explicitly deferred.

### 14F.3 Construction

`BuildKennel(int day)`:

- validates prerequisites;
- consumes build resources through canonical inventory APIs;
- sets one persistent built state;
- cannot charge twice if already built;
- emits a transition event if existing shelter construction patterns use events.

The source does not specify kennel build-material quantities. Derive them only from existing game tuning/construction conventions during implementation; mark them as tuning data, not hidden constants.

### 14F.4 Acquisition

`AcquireDog(string dogId, string name, int day)`

Allowed acquisition sources:

- capture event;
- trader event;
- stray event.

Requirements:

- ID unique;
- acquisition cannot silently overwrite an existing active dog unless explicitly supported;
- initial stats come from data/default profile;
- stage is derived consistently from acquisition profile/day;
- invalid name/ID handled according to project conventions.

### 14F.5 Skill system

Required skills:

**`scent_tracking`**
- reduces expedition encounter risk by 5–15%.

**`guard_alert`**
- adds one hatch-breach event choice.

**`companion_morale`**
- +1–3 morale in assigned room.

**`combat_assist`**
- +10% success chance for applicable combat encounters.

Rules:

- training takes 3–7 days per skill level;
- max two active skills;
- skill progression deterministic;
- swapping active skills has explicit rules;
- untrained/unused skill decay is bounded and cannot underflow;
- training cannot progress twice in one day.

### 14F.6 Training state model

Recommended explicit state:

```text
skillId
currentLevel
targetLevel
trainingStartedDay
trainingDurationDays
trainingProgressDays
isActive
```

Avoid one generic `trainingLevel` being forced to represent every skill if independent skills need independent progression. Preserve the source DTO field if useful as aggregate progression, but do not let it make skill state ambiguous.

### 14F.7 Daily tick

`TickDay(int day)` must handle, in deterministic order:

1. food consumption;
2. health consequence if food cannot be supplied, if current design supports it;
3. active training progress;
4. stage/age transition;
5. skill decay;
6. assignment-derived passive state refresh.

Source food cost:

`cooked_meat × 0.5 per day`

If inventory only supports integer quantities, do not introduce fractional inventory silently. Resolve using:

- half-unit capable quantity type if already supported; or
- a two-day consumption cadence / equivalent integer representation.

Document whichever convention is used.

### 14F.8 Kennel data authority

Create:

`Assets/StreamingAssets/Data/kennel.json`

Suggested responsibilities:

- default canine profiles/breeds;
- stage thresholds;
- skill definitions;
- training duration;
- bonus ranges;
- food consumption;
- item references.

Keep runtime dog state in saves, not in static JSON.

### 14F.9 Kennel items

Add:

- `item_kennel_blueprint`
- `item_dog_whistle`
- `item_training_dummy`
- `item_dog_armor_light`
- `item_dog_medkit`

Ensure intended item types prevent accidental use by unrelated systems.

### 14F.10 Kennel recipes

Add source recipes:

```text
craft_dog_whistle
  workbench
  scrap_metal ×1
  leather_strap ×1

craft_training_dummy
  workbench
  cloth ×3
  scrap_wood ×2
  leather_strap ×1

craft_dog_armor_light
  workbench
  leather_strap ×3
  scrap_metal ×2
  duct_tape ×1

craft_dog_medkit
  workbench
  bandage ×2
  anti_rad ×1
  cloth ×1
```

Validate every ID.

### 14F.11 Acquisition events

Add:

**`event_stray_dog_arrival`**
- minDay 10
- weight 1.5
- Clear/Overcast weather

**`event_trader_sells_puppy`**
- minDay 15
- requires caravan at node

**`event_wild_dog_pack`**
- minDay 20
- weight 2.0
- capture or drive-off branch

Each successful acquisition branch must call the same canonical `AcquireDog` path.

No event should directly mutate UI-only dog state.

### 14F.12 Expedition integration

The source requests:

```text
ExpeditionSystem.Start accepts optional kennelBonus float
```

Before adding a raw float, inspect existing modifier architecture.

Preferred hierarchy:

1. use existing expedition modifier/context object if one exists;
2. otherwise add a narrow optional companion/kennel modifier;
3. use raw float only if consistent with existing APIs.

Requirements:

- bonus only applies when dog is assigned to that expedition;
- no bonus while dog assigned to guard/common room;
- scent bonus clamps so encounter risk never becomes negative;
- save/load during expedition preserves assignment if expeditions are persistent.

### 14F.13 Guard-event integration

For hatch-breach events:

- detect active dog assignment to guard;
- verify `guard_alert` active;
- add one fourth choice:
  `"The dog is barking — it knows something. Hold the hatch and listen."`

Requirements:

- do not mutate `events.json` at runtime;
- event choice augmentation is deterministic;
- choice ID stable;
- no duplicate fourth choice on repeated evaluation;
- choice absent without required skill/assignment.

### 14F.14 Companion morale integration

When assigned to a room:

- `GetMoraleBonus()` returns the valid bonus;
- bonus flows through the shelter's existing morale pipeline;
- it is localized to current room;
- moving dog removes previous room effect before adding new one;
- expedition/guard assignment removes room morale if mutually exclusive.

### 14F.15 Combat assist scope

`combat_assist` is in source scope, but `ExpeditionSystem.cs` is described as read-only during initial evidence.

Integrate only through a sanctioned encounter-resolution modifier seam.

Do not rewrite combat architecture for this task.

If no safe seam exists, implement the Core bonus/read model and add an explicit integration blocker/TODO rather than hard-wiring combat logic into the kennel.

### 14F.16 Dog health and equipment

The source defines health and dog armor/medkit items.

Minimum scope:

- health persists;
- medkit can affect dog only through a canonical kennel action if implemented;
- armor item is recognized and can expose intended protection if an equipment seam exists.

Do not invent a complete canine injury subsystem beyond what current combat/events can safely support.

### 14F.17 Kennel panel

`war_dog_kennel` displays:

- dog name;
- portrait/silhouette + breed;
- stage;
- health;
- loyalty;
- active skills;
- training progress;
- food consumption;
- current assignment.

Actions:

- Assign to Expedition
- Assign to Guard
- Assign to Common Room
- Train
- Build Kennel when absent

Empty state:

`No kennel built yet`

Unavailable actions show reason:

- no kennel;
- no dog;
- training already active;
- max active skills;
- insufficient food/equipment if applicable.

Note (§1.5): the panel is currently a shelved prototype; the foreman/user must
reconfirm the presentation wave before panel work begins.

### 14F.18 Save/load

Add `kennel` section.

Persist:

- kennel built;
- dog identity/profile;
- stage;
- stats;
- training state;
- active skills;
- assignment;
- equipment if implemented;
- last processed day or equivalent idempotency field if needed.

Round-trip invariants:

```text
no duplicate food charge
no skipped/duplicated training day
same active skills
same assignment
same expedition bonus
same morale bonus
same health/loyalty
```

### 14F.19 Integrity selftest

Validate:

- kennel JSON loads;
- stages valid;
- skills unique;
- skill ranges valid;
- item IDs exist;
- recipes exist;
- event IDs unique;
- event requirements resolve;
- hatch-breach target events exist;
- bonus percentages finite and bounded;
- food cost valid.

### 14F.20 xUnit coverage

Minimum:

1. build kennel;
2. repeated build rejected/idempotent;
3. acquire dog;
4. duplicate acquisition behavior;
5. stage progression;
6. training start;
7. training duration;
8. max two active skills;
9. skill decay;
10. scent tracking bonus;
11. encounter risk clamp;
12. guard-alert choice addition;
13. guard-alert absent when unassigned;
14. room morale bonus;
15. assignment transfer removes prior bonus;
16. food consumption;
17. no-food behavior;
18. aging;
19. health persistence;
20. save/load round trip;
21. old save default;
22. deterministic daily progression;
23. event acquisition path uses canonical acquisition;
24. panel read model state.

### 14F.21 Balance simulation

Run long-horizon campaigns.

Measure:

- average acquisition day;
- food cost over 30/60/120 days;
- net morale value;
- reduction in expedition encounter frequency;
- effect of combat assist;
- frequency of guard-alert event utility;
- whether dog becomes mandatory optimal play.

Target principle: **the dog is a long-term strategic investment with opportunity cost, not a permanent free buff.**

### 14F acceptance gate

Pass when acquisition, training, assignment, food cost, expedition/guard/morale effects, UI, and persistence all operate through one authoritative kennel state.

---

# PHASE 6 — CROSS-SYSTEM INTEGRATION HARDENING

## 6.1 Economy × Weather

Verify:

- current weather drives embargo activation;
- future forecast does not prematurely alter current market prices;
- cloud seeding targeting future weather does not prematurely clear today's embargo;
- when cloud seeding changes the target crisis day, the embargo follows authoritative resulting weather.

## 6.2 Forecast × Cloud Seeding

Verify:

- no prediction → no deployment;
- prediction change updates target;
- deployment uses the same target the UI displayed;
- success clears/replaces target crisis;
- warning retracts or updates after successful cancellation;
- cooldown remains after warning disappears.

## 6.3 Regional Pricing × Embargo

Verify the quote equation explicitly.

Example:

```text
base price = 100
foundry regional modifier = 0.80
active embargo modifier = 1.30

final pre-transaction quote = 104
```

Tests should assert formula order and prevent accidentally applying 1.30 to an already embargo-adjusted cached price twice.

## 6.4 Trophy × Morale × Kennel

Both trophy and companion can affect room morale.

Verify:

- bonuses use the same canonical morale aggregator;
- relocating either source removes previous contribution;
- source IDs make debugging possible;
- save/load cannot duplicate passive contributions.

## 6.5 Kennel × Economy

Dog food consumption creates ongoing demand for `cooked_meat`.

Verify it does not bypass:

- inventory;
- market scarcity;
- regional pricing;
- existing food accounting.

This connection should be emergent through item consumption, not a special market rule.

## 6.6 Kennel × Events × Expedition

Assignment must be mutually coherent.

Recommended assignment enum:

```text
None
Expedition
Guard
Room
```

Only one active assignment at a time unless the existing game architecture explicitly supports otherwise.

---

# PHASE 7 — DATA AUTHORITY AND VALIDATION MATRIX

## 7.1 New data files

Expected from source scope:

- `trade_embargoes.json`
- `regional_prices.json` **or** extension to `economy_goods.json`
- `cloud_seeding.json`
- `kennel.json`

Do not create redundant companion files if existing authorities cleanly own the fields.

## 7.2 Modified content authorities

Likely:

- `items.json`
- `recipes.json`
- `events.json`
- `economy_goods.json`

## 7.3 Required ID graph validation

Validate all cross-references:

```text
embargo weather → WeatherKind
embargo region → region catalog
embargo item/tag → goods catalog
regional price item → goods/item authority
regional price region → region catalog
cloud-seeding recipe ingredient → item catalog
cloud-seeding station → station catalog
trophy species → trapping species
trophy recipe input → item catalog
trophy output → decor item
trophy room/slot → shelter decor catalog
kennel item → item catalog
kennel recipe → items/station
kennel event → event catalog
guard-alert hook → hatch-breach event
```

## 7.4 Validation severity

Use:

- **error** for broken required reference, duplicate authority, non-finite value, impossible enum;
- **warning** only for balance-like anomalies that are technically valid.

CI/selftest must fail on errors.

---

# PHASE 8 — SAVE/LOAD AND MIGRATION PLAN

## 8.1 New save sections

Expected:

- trade embargo runtime state if required;
- cloud seeding;
- kennel.

Regional price atlas should remain data-derived unless runtime shocks are stored there.

## 8.2 Migration rules

Older save:

- missing new section → default;
- no fabricated dog;
- no phantom cooldown;
- no stale embargo;
- existing caravan resumes according to current authoritative weather;
- existing decor remains untouched.

## 8.3 Idempotency tests

Critical reload points:

1. immediately before day tick;
2. immediately after day tick;
3. caravan blocked;
4. embargo shock decaying;
5. forecast warning active;
6. cloud-seeding target scheduled;
7. cloud-seeding cooldown active;
8. trophy crafted but not placed;
9. trophy placed;
10. kennel training in progress;
11. dog assigned to expedition;
12. dog assigned to room.

At each point, save/load must preserve the same next deterministic result.

---

# PHASE 9 — UI / UX INTEGRATION PLAN

## 9.1 `economy_detail`

Must expose:

- regional price matrix;
- selected region;
- cheap/neutral/expensive labels;
- embargo status;
- affected goods;
- route impact;
- final current quote where meaningful.

## 9.2 `traveling_caravan`

Must expose:

- origin region;
- regional specialty;
- blocked/slowed state;
- weather cause;
- price context.

## 9.3 `weather_forecast`

Must expose:

- 3-day weather;
- confidence;
- route safety;
- crisis prediction;
- preparation advice;
- cloud-seeding action;
- success chance;
- resource cost;
- cooldown.

## 9.4 `shelter_decor`

Must expose:

- trophy-compatible slots;
- available trophy;
- species;
- morale effect;
- placement state.

## 9.5 `war_dog_kennel`

Must expose:

- build/acquisition empty state;
- dog stats;
- training;
- food;
- active skills;
- assignment;
- health/loyalty;
- valid actions and disabled reasons.

## 9.6 Presentation rule

No panel may silently recompute Core outcomes.

Every displayed numeric bonus or availability state should trace to an authoritative Core/read-model value.

---

# PHASE 10 — AUTOMATED TEST STRATEGY

## 10.1 Unit-test target

Source estimate is 50–63 xUnit tests. Treat that as a floor-level planning estimate rather than a hard cap.

A flagship implementation should aim for enough coverage to prove:

- pure rule correctness;
- invalid-input behavior;
- transition behavior;
- save/load;
- determinism;
- cross-system composition.

## 10.2 Integration-test scenarios

Create named scenarios:

### Scenario I — Weather Trade Shock
```text
clear weather
→ foundry caravan progresses
→ EMP storm
→ foundry caravan blocked
→ electronic price shock
→ panel read model reflects cause
→ weather clears
→ route resumes
→ price decays
```

### Scenario II — Regional Arbitrage
```text
same item
→ compare five regions
→ identify cheapest
→ caravan from that region quotes correct value
→ weather shock changes temporary quote
→ best-region baseline remains correct
```

### Scenario III — Forecast to Countermeasure
```text
station install
→ calibration
→ crisis prediction
→ forecast panel warning
→ cloud-seeding deployment
→ deterministic success
→ crisis cancelled
→ warning updates
→ cooldown persists
```

### Scenario IV — Failed Countermeasure Mitigation
```text
GlassStorm/RadHail predicted
→ deploy
→ deterministic failure
→ crisis remains
→ ceiling damage reduced by 30% for one day
→ mitigation expires
```

### Scenario V — Trophy Lifecycle
```text
rare species caught
→ hide preserved
→ trophy recipe identified
→ trophy crafted
→ placed in valid room
→ localized morale increases
→ save/load
→ same morale
```

### Scenario VI — Dog Lifecycle
```text
build kennel
→ acquire dog
→ train scent_tracking + guard_alert
→ assign expedition
→ encounter risk reduced
→ reassign guard
→ hatch-breach extra choice
→ reassign room
→ morale bonus
→ daily food cost
→ save/load
```

### Scenario VII — Combined Campaign Day
Run one deterministic day with:

- active embargo;
- regional caravan;
- active forecast warning;
- dog food consumption;
- placed trophy;
- kennel training;
- daily briefing generation.

Verify no subsystem double-ticks or overwrites another.

---

# PHASE 11 — BALANCE AND EXPLOIT TESTING

## 11.1 Economy exploit probes

Probe:

- buy cheap region → sell expensive region loop;
- same-day caravan resale;
- embargo onset/clear price timing exploit;
- save/reload price reset;
- repeated day tick;
- blocked caravan stock duplication.

## 11.2 Weather exploit probes

Probe:

- deploy cloud seeding twice same day;
- deploy with insufficient materials;
- save/reload to reroll success;
- target one weather but cancel another;
- reactive penalty bypass;
- cooldown reset on old save.

## 11.3 Decor exploit probes

Probe:

- place same trophy in multiple slots;
- duplicate morale on reload;
- move trophy without removing previous bonus;
- craft recipe without consuming preserved hide.

## 11.4 Kennel exploit probes

Probe:

- assign dog to expedition and room simultaneously;
- food charged twice or not at all;
- training progresses via reload;
- active skill cap bypass;
- guard choice duplicated;
- scent tracking makes risk negative;
- dog acquisition duplicated.

---

# PHASE 12 — PERFORMANCE AND ALLOCATION CHECKS

These systems are not expected to be computationally heavy, but UI read models and catalog scans can create avoidable churn.

Check:

- no `events.json` full scan every frame;
- no regional price full-catalog rebuild every cell draw;
- no JSON parsing inside day tick;
- no repeated allocations for unchanged read models if current architecture supports caching;
- no event subscription leaks when panels open/close;
- no duplicate system instances from composition root.

If a benchmark harness exists, add lightweight measurements for:

- building regional price matrix;
- crisis prediction;
- kennel daily tick;
- embargo rule lookup.

Performance optimization must not compromise correctness or determinism.

---

# PHASE 13 — OBSERVABILITY AND DEBUGGING

Add diagnostic information through existing logging conventions.

Useful structured diagnostics:

```text
[Embargo] weather=EMPStorm region=foundry blocked=true modifier=...
[Market] item=... region=... base=... regional=... embargo=... final=...
[Forecast] day=... weather=... crisis=... confidence=...
[CloudSeeding] target=... chance=... roll=... result=...
[Trophy] species=... recipe=... item=... room=... morale=...
[Kennel] dog=... assignment=... skill=... food=... bonus=...
```

Do not log every frame.

Diagnostics should make a failing headless test explainable.

---

# PHASE 14 — IMPLEMENTATION ORDER AND CHECKPOINTS

## Checkpoint C1.1 — Preflight
- P0 complete.
- Baseline green.
- No code changes beyond notes/test scaffolding.

## Checkpoint C1.2 — Economy Core
- `TradeEmbargoSystem`.
- `RegionalPriceAtlas`.
- data authorities.
- unit tests.

## Checkpoint C1.3 — Economy Wiring/UI
- caravan integration.
- market integration.
- economy panel.
- combined economy scenario green.

## Checkpoint C1.4 — Forecast
- crisis prediction.
- coordinator/read model.
- forecast panel.
- daily briefing.
- weather selftest green.

## Checkpoint C1.5 — Cloud Seeding
- Core.
- content recipes/items.
- weather cancellation.
- armor mitigation.
- UI action.
- selftest green.

## Checkpoint C1.6 — Trophy Pipeline
- items/recipes.
- species mapping.
- decor slots.
- morale.
- full pipeline test.

## Checkpoint C1.7 — Kennel Core
- data.
- acquisition.
- training.
- daily tick.
- save/load.

## Checkpoint C1.8 — Kennel Cross-Wiring
- expedition.
- events.
- morale.
- panel.
- balance sim.

## Checkpoint C1.9 — Final Hardening
- cross-system scenarios.
- exploit probes.
- old-save tests.
- full build/test/selftests.
- final diff audit.

## Checkpoint C1.10 — Content Expansion Tranche (+30%)
- Phase 22 content authored per the §22 tables.
- integrity gate extended for every new row.
- balance sim re-run with expanded content.
- ship gate re-verified end-to-end.

Do not merge a later checkpoint to compensate for an earlier red gate.

---

# PHASE 15 — FILE-LEVEL IMPLEMENTATION MAP

## New Core files

Expected:

```text
Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs
Assets/Ashfall.Core/Economy/RegionalPriceAtlas.cs
Assets/Ashfall.Core/World/CloudSeedingSystem.cs
Assets/Ashfall.Core/Shelter/KennelSystem.cs
```

## New data authorities

Expected:

```text
Assets/StreamingAssets/Data/trade_embargoes.json
Assets/StreamingAssets/Data/regional_prices.json   # only if not extending economy_goods.json
Assets/StreamingAssets/Data/cloud_seeding.json
Assets/StreamingAssets/Data/kennel.json
```

## Modified Core / host areas

Likely:

```text
Assets/Ashfall.Core/TravelingCaravanSystem.cs        (Core root — see §1.5)
Assets/Ashfall.Core/Economy/MarketSystem.cs
Assets/Ashfall.Core/WeatherStationSystem.cs          (Core root — see §1.5)
Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs
Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs   (Shelter/ — see §1.5)
Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs
Assets/Ashfall.Core/WildlifeTrappingSystem.cs        (Core root — see §1.5)
Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs or its host integration seam
DailyBriefingReportBuilder
PanelRegistry / panel backing code
SaveSectionRegistry
composition root / service wiring
data-integrity selftest registration
bridge/headless selftests
```

## Modified data files

Expected:

```text
items.json
recipes.json
events.json
economy_goods.json
```

Before editing any file, confirm the real repository path and current ownership; this map is a target, not permission to create duplicates.

---

# PHASE 16 — FINAL VERIFICATION MATRIX

## 16.1 Per-task verification

Run after each task:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Additional targeted commands when implemented:

```bash
godot --headless --path . -- --weather-forecast-selftest
godot --headless --path . -- --cloud-seeding-selftest
```

## 16.2 Final regression requirements

Final implementation must show:

- Core tests: all pass;
- Godot host build: 0 errors;
- warnings: no newly introduced actionable warnings;
- data-integrity selftest: 0 errors;
- bridge selftest: exits 0;
- weather forecast selftest: exits 0;
- cloud-seeding selftest: exits 0;
- save migration tests: pass;
- deterministic replay checks: pass;
- balance sim: no runaway economy or trivialized crisis system.

---

# PHASE 17 — ACCEPTANCE CHECKLIST BY PLAYER-FACING LOOP

## Economy loop
- [ ] Regional origin affects caravan prices.
- [ ] Regional prices are visible before trade.
- [ ] Weather can block/slow appropriate routes.
- [ ] Weather shock affects appropriate prices.
- [ ] Price shock decays.
- [ ] UI explains why price changed.
- [ ] Save/load preserves continuity.
- [ ] No infinite arbitrage exploit.

## Forecast loop
- [ ] Station generates forecast.
- [ ] Calibration changes confidence.
- [ ] Crisis can be predicted.
- [ ] Warning is visible.
- [ ] Daily briefing includes warning.
- [ ] False/changed predictions are handled.
- [ ] Prediction is not a hidden perfect oracle.

## Cloud-seeding loop
- [ ] Action requires valid prediction.
- [ ] Materials are checked.
- [ ] Chance shown before action.
- [ ] Deterministic success/failure in tests.
- [ ] Success changes intended crisis weather.
- [ ] Failure consumes materials.
- [ ] GlassStorm/RadHail failure can mitigate armor damage.
- [ ] Seven-day cooldown enforced.
- [ ] Save/load cannot reroll outcome or reset cooldown.

## Trophy loop
- [ ] Qualifying species maps to trophy recipe.
- [ ] Recipe consumes correct preserved material.
- [ ] Eight trophy outputs resolve.
- [ ] Trophy can be placed in valid slot.
- [ ] Morale is localized.
- [ ] Move/remove updates morale correctly.
- [ ] Save/load preserves decor state.

## Kennel loop
- [ ] Kennel can be built.
- [ ] Dog can be acquired through canonical events.
- [ ] Training progresses.
- [ ] Max two active skills enforced.
- [ ] Daily food cost applied once.
- [ ] Expedition assignment modifies encounter risk.
- [ ] Guard assignment adds event choice when eligible.
- [ ] Room assignment adds localized morale.
- [ ] Assignment states do not overlap illegally.
- [ ] Health/loyalty/training persist.
- [ ] UI reflects authoritative state.

---

# PHASE 18 — RISK REGISTER

| Risk | Severity | Mitigation |
|---|---:|---|
| Regional and embargo modifiers applied twice | Critical | Canonical price equation + component-level tests + ledger diagnostics |
| Forecast reads hidden future truth and becomes perfect oracle | High | Use forecast representation/confidence, not direct future event certainty |
| Cloud-seeding changes wrong day/weather | Critical | Explicit target day/weather in action result and integration tests |
| Save/reload rerolls cloud-seeding | Critical | Resolve roll once; persist resulting authoritative state |
| New save sections break old saves | Critical | Optional/default migration tests |
| Caravan route block traps caravan permanently | High | explicit clear/resume transition tests |
| Trophy morale duplicates after relocation/reload | High | canonical morale source aggregation |
| Kennel daily tick double charges food | High | last-processed-day/idempotency or scheduler guarantees |
| Dog assigned to multiple roles | High | explicit assignment state machine |
| Fractional `0.5 cooked_meat` incompatible with inventory | High | resolve representation before implementation |
| Event choice injection duplicates option | Medium | stable choice ID + set-based validation |
| UI frame loop scans full catalogs | Medium | cached read models/refresh on state change |
| Data authority duplicated across JSON files | Medium | choose one owner per coefficient/mapping |
| Balance turns weather into permanent death spiral | High | long-run simulation and decay clamps |
| Cloud seeding trivializes all severe weather | High | scarcity, failure chance, cooldown, campaign simulation |
| Kennel duplicates Plan 174 companion authority | Critical | 14F extends/routes through `CompanionAnimalSystem` (see §1.5) |
| Embargo shock doubles the Plan 212 weather shock | Critical | single market pricing path; embargo composes as one factor (see §1.5) |

---

# PHASE 19 — DEFERRED FOLLOW-UPS

These ideas belong after C1 and must not expand the current implementation unless required by a discovered blocker:

- caravan rerouting around blocked regions;
- player-authored route optimization UI;
- ecological side effects from repeated cloud seeding;
- hostile faction sabotage of weather infrastructure;
- trophy degradation/maintenance;
- legendary trophy variants;
- dog breeding;
- full combat-dog simulation;
- dog death memorial/plaque;
- richer dog injury treatment subsystem;
- dynamic region production simulation beyond the current price atlas.

Treat them as successors, not hidden scope.

---

# PHASE 20 — EXECUTION HANDOFF FOR AN IMPLEMENTATION AGENT

Use the following operating rules when handing this plan to Antigravity, Gemini, Claude, Codex, or another repository agent:

1. Read the actual repository before editing any target file.
2. Never assume a file/API from this plan exists unchanged.
3. Reuse existing architectural patterns before inventing new ones.
4. Make one checkpoint coherent before moving to the next.
5. Add or update tests in the same checkpoint as behavior.
6. Keep Core deterministic.
7. Avoid UI-owned business logic.
8. Avoid broad refactors unrelated to the current checkpoint.
9. Never silence a failing integrity test by weakening validation.
10. Do not replace canonical IDs with fuzzy/name-based matching.
11. Preserve backward compatibility where feasible.
12. Run targeted tests after each edit cluster.
13. Run the full verification matrix before declaring a checkpoint complete.
14. Report every deviation from the plan caused by actual repository evidence.
15. If the repository already implements a requested behavior, verify and reuse it rather than duplicating it.

### Recommended agent cadence

For each checkpoint:

```text
RECON
→ EVIDENCE REPORT
→ IMPLEMENT
→ TARGETED TESTS
→ INTEGRATION TEST
→ DATA SELFTEST
→ FULL REGRESSION
→ DIFF REVIEW
→ CHECKPOINT REPORT
```

### Checkpoint report template

```markdown
## Checkpoint C1.x Report

### Files changed
- ...

### Behavior implemented
- ...

### Data added/modified
- ...

### Tests added
- ...

### Commands run
- ...

### Results
- build:
- unit tests:
- integrity:
- headless selftests:
- balance:

### Deviations from plan
- ...

### Remaining blockers
- ...

### Next checkpoint
- ...
```

---

# PHASE 21 — FINAL SHIP GATE

C1 is complete only when all of the following are true:

- [ ] 14A complete and verified.
- [ ] 14B complete and combined economy scenario green.
- [ ] 14C complete and forecast selftest green.
- [ ] 14D complete and cloud-seeding selftest green.
- [ ] 14E complete and full trapping→trophy→decor pipeline green.
- [ ] 14F complete and kennel lifecycle/cross-wiring green.
- [ ] New/modified JSON authorities pass integrity validation.
- [ ] Old saves load safely.
- [ ] New saves round-trip all runtime state.
- [ ] No deterministic test can be changed by save/reload rerolling.
- [ ] No duplicate passive morale source exists.
- [ ] No duplicate price modifier path exists.
- [ ] No UI layer owns gameplay formulas.
- [ ] All baseline build/test commands pass.
- [ ] Balance sim shows weather pressure is recoverable.
- [ ] Balance sim shows regional trade is useful but not exploitable.
- [ ] Balance sim shows kennel value has meaningful food/opportunity cost.
- [ ] Final diff audit confirms no dead duplicate systems or orphaned data.
- [ ] Implementation report documents any deviations from source assumptions.
- [ ] C1.10 content tranche (+30%, §22) authored, validated, balance-simmed, and re-gated.

---

# PHASE 22 — CONTENT EXPANSION TRANCHE (+30%)

**Authorization:** user-directed expansion of C1 scope. **Sizing rule:** every
workstream's authored content volume grows by ≈30% (per-workstream arithmetic
in the tables below; total ≈ +21 content rows over the ~70-row C1 baseline).
**Non-goals unchanged:** §19 deferred follow-ups stay deferred — no legendary
trophy variants, no dog breeding, no caravan rerouting. Every new row passes
the same integrity pipeline as the base content; no placeholder ids ever.

**Evidence gate for every row:** item ids resolve in `economy_goods.json` /
`items.json`, species ids resolve in the wildlife/trapping catalogs, weather
kinds resolve in `WeatherKind`, regions resolve in the
`RegionalSupplyRouter` vocabulary. A row whose premise fails verification is
replaced, never hand-waved.

## 22.1 Embargo rules — 10 → 14 (+40% of rules; +4 rows)

Four more real `WeatherKind` values currently unmapped (all verified to exist):
`Ashfall`, `BloodRain`, `ThermalInversion`, `ParticulateFog`.

| rule_id | weather_kind | regions | effect | route |
|---|---|---|---|---|
| `embargo_ashfall_materials` | `Ashfall` | `*` | materials +20% | slow 800 |
| `embargo_blood_rain_water` | `BloodRain` | `settlement` | water +50% | blocked |
| `embargo_thermal_inversion_food` | `ThermalInversion` | `greenhouse` | food +25% | slow 700 |
| `embargo_particulate_fog_medical` | `ParticulateFog` | `traplines` | medical +30% | slow 800 |

Deliberately left unmapped: `Silence`, `FalseSpring`, `AlgaeBloom`,
`AshLightning` (calm/odd-ball weathers must stay trade-neutral — scarcity
belongs to danger, not to variety).

## 22.2 Regional atlas entries — 18 → 24 (+6 rows)

Deepens the two thinnest profiles and adds the `coastal` supply tag as a
first-class atlas region (it is already in the accepted vocabulary):

| region | target | modifier | profile |
|---|---|---|---|
| `coastal` | `trap_fish` | 700 | local_surplus |
| `coastal` | `clean_water` | 850 | local_surplus |
| `coastal` | category `tools` | 1200 | imported_scarce |
| `flotilla` | `item_taper_kit_opioid` | 1100 | balanced |
| `settlement` | `tobacco_pouch` | 1100 | balanced |
| `traplines` | `trap_box` | 850 | local_surplus |

## 22.3 Crisis intelligence copy — +8 rows

14C's crisis condition index gains authored, data-driven preparation advice
and briefing lines (no free-form UI logic):

- +6 per-crisis preparation-advice entries covering every crisis-capable
  weather kind in the embargo table (stock water / brace hatch / protect
  electronics / reinforce ceiling / prepare medical / recall expeditions),
  each with a matching daily-briefing one-liner.
- +1 retraction line ("Stand down: the predicted {weather} did not develop.")
  and +1 uncertainty line ("Confidence low — monitor the mast.") for the
  briefing's false-positive path (§14C.10).

## 22.4 Cloud-seeding content — 3 items + 3 recipes → +1 recipe (+1 row)

- `craft_silver_iodide_cartridge_bulk` — distiller — cartridges ×3 batch
  (chemicals ×8, sulphur ×5, fuel ×5): a genuine bulk discount that keeps the
  scarce-use economy while reducing crafting tedium in long campaigns.
- No new deployable items: the countermeasure must stay scarce (§14D.17).

## 22.5 Trophy pipeline — 8 → 11 trophies (+3 items +3 recipes = +6 rows)

Three additional species-to-trophy mappings. **Premise evidence:**
`species_ash_hound` exists (Plan 174 added it to `wildlife_ecosystem.json`);
the other two rows MUST be verified against the live trapping species catalog
during authoring and re-targeted to actual rare species ids if the names
differ:

| item id | species (verify at authoring) | morale | room |
|---|---|---|---|
| `item_decor_trophy_ash_hound_pelt` | `species_ash_hound` | +2 | common room |
| `item_decor_trophy_gulden_wolf` | rare wolf variant (verify) | +3 | sleeping quarters |
| `item_decor_trophy_kestrel_wings` | small raptor species (verify) | +1 | radio room |

Same recipe shape as the base eight (preserved material + scrap_wood ×2 +
chemicals ×1, workbench, 4–6h). Trophy morale stays inside the existing
decor-stacking policy.

## 22.6 Kennel content — +4 rows

- +2 breeds in `kennel.json` (four total): a small alert breed (high
  trainability, low guard) and a heavy guard breed (low food, high guard,
  slow training) — stage thresholds and food costs are authored tuning data.
- +2 acquisition events: `event_drowning_pup_rescue` (minDay 12, Rain/Storm
  gate, rescue-or-drive-off branch) and `event_expedition_stray_follows_home`
  (minDay 25, requires a completed expedition that week). Both route through
  the canonical `AcquireDog` path like the base three.
- No fifth skill, no breeding, no multi-dog: those stay deferred (§19).

## 22.7 Radio/briefing flavor — +4 rows

- 2 market-rumor lines for embargo start/end (Core-projected from
  `EmbargoSummary`, mirroring the Plan 212 shock-rumor pattern).
- 2 kennel milestone band items (first training completion, first guard save).

## 22.8 Expansion integrity and balance requirements

- Every §22 row enters `CatalogIntegrityValidator` coverage automatically via
  the existing 14A/14B hooks; new hooks only where a NEW file appears (none
  planned — all rows extend existing authorities).
- Balance sim re-runs §14A.12/§14B.11/§14E.10/§14F.21 with expanded content:
  embargo days-blocked per 30 days must not exceed the base-content result by
  more than 25%; trophy-by-day-120 expectation must stay ≤ 5; kennel food
  pressure must stay within the authored affordability band.
- The Phase 21 ship gate re-runs in full after C1.10.

---

## Final Intended Player Experience

When C1 is complete, the day-to-day survival loop should have visible causality.

A forecast warns that a dangerous weather pattern may arrive. The player checks the economy and sees that the same weather can cut off a region and distort supply. They decide whether to stockpile, alter trade plans, or spend rare resources on cloud seeding. Meanwhile, trapping can produce rare trophies that permanently change shelter rooms, while a trained dog competes for food but contributes to expedition safety, shelter defense, or morale depending on assignment.

The important outcome is not six isolated features. It is a coherent network of decisions:

```text
weather
→ information
→ preparation
→ logistics
→ prices
→ resource pressure
→ counterplay

trapping
→ crafting
→ shelter identity
→ morale

kennel
→ food cost
→ training
→ assignment
→ expedition / defense / morale payoff
```

That network is the acceptance target for `docs/plans/C1_planintegration.md`.

---

# CHECKPOINT REPORTS

## Checkpoint C1.1 Report — Preflight (P0)

### Files changed
- `docs/plans/C1_planintegration.md` (this document, with §1.5 corrections); `WORKTREE_OWNERSHIP.md` (claim row). No production code.

### Evidence recorded (P0 exit gate)

**Integration map.**
- DI conventions: Core systems take authorities via constructor (`WeatherSystem`, `ISeededRng`, `ILog`) and optional collaborators via property injection (`TravelingCaravanSystem.Catalog`, `.TravelEncounters`; `WeatherStationSystem.GateCatalog`; `MarketSystem.BindCatalog`/`BindCommodityCatalog`).
- Events: `event Action<T>` typed payloads raised after state change; `RaiseChanged()` idiom; host marks sections dirty / refreshes panels.
- Save DTOs: `[Serializable]` classes; versioned state (`Version` const, restore migrates older, throws on newer); SystemTextJsonSerializer with `IncludeFields=true`.
- JSON data DTOs: snake_case field names matched literally; strict nullable Raw DTOs inside loaders; schema envelope (`schema_version`, `collection_id`, `description`); errors collected, never thrown.
- Price equation (canonical, current, verified in `MarketSystem.ExplainPrice`): `base × demand → × category index → × per-shock factors → floor/ceiling clamp [0.25×, 4×] base`, exposed as typed `PriceFactorRecord` rows (`PriceFactorKind`). **C1 extension order: Regional factor after Category, Embargo factor after Shock, both before the existing clamps — no new clamps.**
- Day tick: host-side day owners (economy owner precedes index update + `market_shocks_active` event; weather bridge precedent `Main.TickEconomyWeatherBridge` is a thin adapter calling `MarketSystem.ApplyShock` from `EconomyWeatherShockRules`).
- RNG: `ISeededRng` injected per tick; embargo/price-geography logic needs none.
- Regions: TWO vocabularies (specialty tags flotilla/foundry/greenhouse/traplines/settlement/coastal/general + route origins deep_coast/industrial_belt/ash_flats/settlement), normalized by `RegionalSupplyRouter.TagsForOrigin`; `IsAcceptedSupplyTag` is the acceptance gate.

**Save strategy decision (P0.5).** Versioned-section pattern with old-save neutral
defaults (the MarketState/TradeEmbargoState convention above). No derived value
is persisted that data + world state can reconstruct. Embargo decay runtime
state is the only genuinely unreconstructible state in 14A/14B and is captured
in `TradeEmbargoState` (wired into the economy save path in C1.3).

**Baseline snapshot (P0.6).** Tests build 0 err/0 warn; **full suite
11,172/11,172 PASS**; host build 0 err (4 pre-existing CS8602 warnings from the
user's uncommitted `SilentFoundryPanel.cs`, outside any claim). No pre-existing
failure to classify. Log: `artifacts/baseline_c1_p0.log`.

### Deviations from plan
- None beyond the §1.5 corrections recorded before implementation.

## Checkpoint C1.2 Report — Economy Core (14A + 14B)

### Files changed
- `Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs` (new): `EmbargoRule`, `TradeEmbargoCatalog` + strict loader (`trade_embargoes.json`), `TradeEmbargoSystem` (pure route queries `IsRouteBlocked`/`GetRouteProgressMultiplier`, pure `GetWeatherPriceMultiplierPermille`, decay-aware `GetCurrentPriceMultiplierPermille`, `NotifyWeather` day-idempotent state machine with exact-neutral decay, `GetEmbargoSummary`, `IsRuleValid`, versioned `CaptureState`/`RestoreState` — old saves neutral, newer throws; zero RNG).
- `Assets/Ashfall.Core/Economy/RegionalPriceAtlas.cs` (new): `RegionalPriceEntry`, `RegionalPriceCatalog` + strict loader (`regional_prices.json`), `RegionalPriceAtlas` (stateless — no save state; item-over-category resolution, `GetRegionalPrice`, `GetBestRegion` deterministic with ordinal tie-break, `GetRegionalGoods`, `GetRegionsForItem`).
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (additive only): `ValidateTradeEmbargoRules` + `ValidateRegionalPriceAtlas` hooks in the permanent gate (loader errors reported; cross-file goods-id resolution against `economy_goods.json`).
- `Assets/StreamingAssets/Data/trade_embargoes.json` (new): 10 rules — all 8 source-required cases (FalloutStorm/BlackRain/EMPStorm/Blizzard/AcidSnow/BioFog/GlassStorm/RadHail) + BlackSnow/IceStorm; only real `WeatherKind` values, accepted region tags, known goods categories, and existing goods ids; AcidSnow/IceStorm use slowdown (500/700‰) instead of blocking.
- `Assets/StreamingAssets/Data/regional_prices.json` (new): 18 entries covering all 5 canonical regions with the source profiles (flotilla water-chain 0.7x / seeds 1.5x; foundry production 0.8x / food 1.3x; greenhouse seeds+canned 0.8x / tools 1.4x; traplines meat+salve 0.7x / electronics 1.5x; settlement medical 0.9x).
- `docs/data/CATALOG_REGISTRY.md` regenerated via the owning script (611 catalogs, in-sync `--check` PASS).
- Tests: `Ashfall.Core.Tests/Economy/TradeEmbargoSystemTests.cs` (20 cases), `Ashfall.Core.Tests/Economy/RegionalPriceAtlasTests.cs` (16 cases).

### Data-authority decision (14B.3)
**Option B — separate `regional_prices.json`**, consistent with the
`commodity_baselines.json` companion-file precedent; per-item/per-category
multipliers only, base prices never duplicated.

### Commands run / results
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeEmbargoSystemTests.cs` → 20/20 (after running alone first)
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/RegionalPriceAtlasTests.cs` → 16/16
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/` → 154/154 (Plan 212 + Plan 56 neighbors green)
- `godot --headless --path . -- --data-integrity-selftest` → PASS, 0 errors across 327 catalogs (both new files walk clean through the permanent gate)
- `dotnet build Ashfall.csproj` → 0 errors; no new warnings (4 pre-existing, outside claim)

### Deviations from plan
- Source embargo categories `electronics`/`seeds` are not canonical goods categories; expressed as explicit validated item ids (`electronic_scrap`, `solar_cell`, `seed_packets`) per §14A.4 fallback.
- `herbal_tea` (source greenhouse profile) does not exist in `economy_goods.json`; `canned_food` covers the greenhouse cheap row instead.
- `leather_strap` (source traplines profile) is not a goods-catalog item; omitted from the atlas (relevant again for 14E/14F recipes — items.json ids there).
- Wildcard `"*"` embargo coverage intentionally matches only real supply-tag regions (unknown region names never match and never crash).

### Remaining blockers
- None. C1.3 (caravan `DailyTick` embargo blocking, single embargo/regional factor path in `MarketSystem` composing after the Plan 212 shock, economy/caravan panels, combined economy scenario) is the next checkpoint.

### Next checkpoint
- **C1.3 — Economy Wiring/UI** (Phase 1 combined gate scenario §"Phase 1 Combined Economy Integration Gate").

## Checkpoint C1.3 Report — Economy Wiring (Core + host bridge)

### Files changed
- **Core:** `Assets/Ashfall.Core/Economy/MarketSystem.cs` (additive v2→v3: `BindRegionalPriceAtlas(atlas, marketRegion)` + `BindEmbargoSystem(...)`; canonical factor order now demand → category → shocks → **Regional** → **Embargo** → existing clamps; `PriceFactorKind.Regional=5/Embargo=6`; region-aware `GetPrice(itemId, region)` / `ExplainPrice(..., region)` / `Buy/Sell/Barter(..., region)` — null region falls back to the bound market region; `MarketState.Version=3` with nested `tradeEmbargo` decay state, v1/v2 restores embargo-neutral, newer throws); `Assets/Ashfall.Core/TravelingCaravanSystem.cs` (additive `Embargoes` property + `CaravanEntry.embargoBlocked` durable flag + `DailyTick(..., weather)` — blocked caravans lose the movement day and fire `OnCaravanEmbargoed` ONCE per transition, `OnCaravanResumed` on clear; slowdown = additional travel-day cost, 0.5 progress doubles stay days; capture/restore carries `embargoBlocked`).
- **Host:** `src/Host/EconomyHostSession.cs` (loads `trade_embargoes.json` + `regional_prices.json`; missing/invalid → neutral path, never hard-fail; exposes the campaign's ONE `EmbargoSystem` shared with caravans); `src/Host/TravelingCaravanHostSession.cs` (`TickRoute(weather)`); `src/Main.Economy.cs` (weather bridge feeds `NotifyWeather` from the authoritative weather BEFORE the Plan 212 band — embargo and severity shocks stay separate factors); `src/Main.CampaignOwners.cs` (caravan day owner passes the day's authoritative weather; causality: weather_world (phase 1) → embargo evaluate + market tick (phase 2) → caravan movement (phase 4)).
- **Tests:** `Ashfall.Core.Tests/Economy/Plan14AEconomyIntegrationTests.cs` (new, 7 cases — §6.3 quote equation, §10.2 Scenario I + II Core harnesses, mid-block/mid-decay save parity, v2 old-save neutrality, seeded-tick determinism); `Ashfall.Core.Tests/Economy/Plan212DynamicEconomyTests.cs` (two stale version pins retargeted: capture pin 2→3, newer-throws now `Version + 1`).
- **Generated:** `docs/data/CATALOG_REGISTRY.md`, `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` (regenerated via owning scripts).

### Commands run / results
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/` → **161/161** (all Plan 14A/14B + Plan 212 + Plan 56 suites)
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` → **1154/1154**
- `godot --headless --path . -- --data-integrity-selftest` → PASS (327 catalogs, 0 errors)
- `godot --headless --path . -- --bridge-selftest` → PASS; `--caravan-selftest` → PASS
- `dotnet build Ashfall.csproj` → 0 errors; architecture map `--check` OK (192 subsystems); save-store matrix `--check` OK (193 stores)

### Deviations / scope notes
- **Trade embargo persistence rides the economy section** (`MarketState` v3 nested `tradeEmbargo`) instead of a new `SaveSectionRegistry` row — the embargo state is market-price state and the economy section already owns market runtime state; a separate section would add a section-count gate bump for no independence benefit. Old saves restore embargo-neutral; newer throws.
- **Panel presentation deferred:** `economy_detail` embargo banner + regional heat map and `traveling_caravan` blocked/slowed reason display are the next presentation wave (the repo's established Core→UI wave cadence). The combined Phase 1 gate's UI-verification bullet moves to that wave; the Core harnesses above verify every quote/block/decay fact the panels will render.
- Caravan stock quotes remain ration-denominated (existing `priceRations` model); regional composition of caravan trade-screen pricing lands with the trade-panel wave.
- A second agent is concurrently active on the Campaign/DailyBriefing area (14C-adjacent files); its untracked/modified files were left untouched and no shared-root conflict occurred.

### Remaining blockers
- None for the Core phase. Next: **presentation wave for economy/caravan panels**, then C1.4 (crisis prediction).
