# Plan 14 — Economy, Weather & Shelter Loop: Deepening the Daily Texture

> **Predecessor:** Plan 13 (economy goods, trapping, weather-crisis events — complete)
>
> **Theme:** Plan 13 enriched the survival-economy surface. This plan connects the
> systems that are now rich in data but thin in feedback: the weather station
> forecasts go unused, the regional supply profiles have no mechanical teeth,
> the shelter decor system has no trophy pipeline, and the `war_dog_kennel`
> panel sits empty of a Core system. Each task closes one feedback loop.

---

## Evidence Inventory (what already exists)

| System | File | State | Gap |
|---|---|---|---|
| WeatherStationSystem | `Assets/Ashfall.Core/WeatherStationSystem.cs` | **Fully implemented** — Install, Calibrate, GenerateForecast, 3-day horizon, route-safety, 0.7–0.9 accuracy | Forecast is generated but never surfaced to the player in a decision-context |
| WeatherIntelligenceCoordinator | `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs` | **Implemented** — owns Station + Orbital, produces `WeatherIntelligenceReadModel` | ReadModel exists but no UI panel consumes it yet |
| SkyLayerArmorSystem | `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | **Implemented** — 5 tiers, attenuation, kinetic impact | Already threatened by Plan 13C weather events; needs cloud-seeding countermeasure |
| PowerGridSystem | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | **Implemented** — generator, battery, brownout, room allocation | Already threatened by Plan 13C EMP events |
| ShelterDecorSystem | `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | **Implemented** — room/slot placements, morale modifiers, memorial bridge, save section | No trophy items, no trap-to-decor pipeline |
| TravelingCaravanSystem | `Assets/Ashfall.Core/TravelingCaravanSystem.cs` | **Extended in Plan 13A** — regional specialty stock, origin regions | No weather embargo, no route restriction |
| MarketSystem | `Assets/Ashfall.Core/Economy/MarketSystem.cs` | **Implemented** — demand, volatility, barter, ledger | No regional price differentiation; `regionalSupply` in goods JSON unused |
| WildlifeTrappingSystem | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | **Extended in Plan 13B** — bait catalog, quarry species, hides, skill | Hide yield exists but no decor bridge |
| economy_goods.json | `Assets/StreamingAssets/Data/` | 31 goods with `regionalSupply` field | Field is present but unused by any system |
| PanelRegistry | `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` | Panels for `weather_forecast`, `economy_detail`, `trade`, `shelter_decor`, `traveling_caravan`, `war_dog_kennel` | Many panels are registered but have no backing Core system or data flow |
| SaveSectionRegistry | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | Sections for caravan, power_grid, shelter_decor | No embargo, kennel, or cloud-seeding sections |

---

## Task 14A — Commodity embargoes & weather-restricted caravan trade

**Goal:** Make weather matter for trade. When a fallout storm hits, caravan routes
should be blocked, prices should spike, and the player should feel the supply
chain break. This is the mechanical payoff for the weather-crisis events
authored in Plan 13C.

**Files:** `TravelingCaravanSystem.cs`, `MarketSystem.cs`, `HardcoreEconomyTuning.cs`,
`economy_goods.json`, `events.json` (read-only), new `TradeEmbargoSystem.cs` (Core).

### Substeps

1. **Read** the `TravelingCaravanSystem.DailyTick()` and `MarketSystem.TickDay()` to
   identify the injection points for embargo logic.

2. **Author `TradeEmbargoSystem`** in `Assets/Ashfall.Core/Economy/TradeEmbargoSystem.cs`:
   - `EmbargoRule` DTO: `weatherKind`, `affectedRegions[]`, `priceMultiplier`, `caravanBlocked bool`
   - `IsEmbargoActive(WeatherKind current)` → bool
   - `GetPriceModifier(string region, WeatherKind current)` → float
   - `IsRouteBlocked(string region, WeatherKind current)` → bool
   - `CaptureState` / `RestoreState` with serializable DTO
   - `RegisterRule(EmbargoRule)` for data-driven configuration
   - Deterministic via `ISeededRng` for any random elements

3. **Author embargo data** — 8–12 embargo rules in a new JSON authority file
   `Assets/StreamingAssets/Data/trade_embargoes.json`:
   - FalloutStorm → all routes blocked, medical prices +50%
   - BlackRain → flotilla routes blocked, water prices +80%
   - EMPStorm → foundry routes blocked, electronic prices +100%
   - Blizzard → traplines routes blocked, food prices +40%
   - AcidSnow → all routes slowed (not blocked), tools prices +30%
   - BioFog → greenhouse routes blocked, seed prices +60%
   - GlassStorm → settlement routes blocked, materials prices +40%
   - RadHail → all routes blocked for 1 day, all prices +25%

4. **Wire `TradeEmbargoSystem` into `TravelingCaravanSystem`**:
   - Add `SetEmbargoSystem(TradeEmbargoSystem)` injection method
   - `DailyTick` checks `IsRouteBlocked` for each caravan's `originRegion`
   - Blocked caravans skip their route advancement and emit a `OnCaravanEmbargoed` event
   - Add `embargoBlocked` flag to `CaravanEntry`

5. **Wire `TradeEmbargoSystem` into `MarketSystem`**:
   - Add `ApplyEmbargoModifier(string itemId, float multiplier)` to `TickDay`
   - Embargoed goods get a temporary price multiplier that decays over N days

6. **Surface in the economy panel**: `economy_detail` panel reads embargo state
   and displays active embargoes, affected regions, and price impacts.

7. **Data-integrity selftest**; verify embargo JSON loads cleanly.

8. **xUnit**: embargo activation/deactivation, price modifier correctness,
   caravan route blocking, round-trip save/load, determinism.

9. **Balance sim**: `ashfall-balance-sim` verifies embargoes don't permanently
   tank the economy — prices recover after weather clears.

**Next steps:** regional price maps in the economy panel (14B); caravan
rerouting logic (avoid blocked regions).

---

## Task 14B — Regional price maps surfaced in the economy panel

**Goal:** The `regionalSupply` field added to `economy_goods.json` in Plan 13A
is currently unused. This task makes it the mechanical backbone of the economy
panel: each region gets a price profile, and the player can see where goods
are cheap (and plan caravan routes accordingly).

**Files:** `MarketSystem.cs`, `economy_goods.json`, `HardcoreEconomyTuning.cs`,
`TravelingCaravanSystem.cs`, new `RegionalPriceAtlas.cs` (Core).

### Substeps

1. **Read** `MarketSystem.GetPrice()` and `GoodsCatalog` to understand how
   the current single-price model works.

2. **Author `RegionalPriceAtlas`** in `Assets/Ashfall.Core/Economy/RegionalPriceAtlas.cs`:
   - `RegionalPriceEntry` DTO: `region`, `itemId`, `basePriceModifier`, `scarcityProfile`
   - `GetRegionalPrice(string itemId, string region)` → float
   - `GetBestRegion(string itemId)` → string (cheapest region for a good)
   - `GetRegionalGoods(string region)` → list of goods native to that region
   - `CaptureState` / `RestoreState`
   - Reads `regionalSupply` field from `economy_goods.json` at construction
   - Each region defines a 0.7x–1.3x modifier on the base market price
   - Deterministic: no RNG needed — pure data lookup

3. **Author regional price data** — extend `economy_goods.json` schema or
   add a companion file `regional_prices.json`:
   - **flotilla**: brine pipe, desal membrane, RO membrane at 0.7x; seeds at 1.5x
   - **foundry**: mechanical parts, electronic scrap, chemicals at 0.8x; food at 1.3x
   - **greenhouse**: seeds, herbal tea, canned food at 0.8x; tools at 1.4x
   - **traplines**: cooked meat, leather strap, frostbite salve at 0.7x; electronics at 1.5x
   - **settlement**: medical kit, anti-rad, solar cell at 0.9x (balanced)

4. **Wire `RegionalPriceAtlas` into `MarketSystem`**:
   - Add `SetRegionalAtlas(RegionalPriceAtlas)` injection
   - `GetPrice(itemId, region)` overload that applies regional modifier
   - `Barter` between regions uses the appropriate regional prices

5. **Wire into `TravelingCaravanSystem`**:
   - Caravan buy/sell prices use the caravan's `originRegion` price profile
   - Player can see: "This caravan is from the foundry — mechanical parts are 20% cheaper"

6. **Surface in the economy panel**: `economy_detail` panel shows a regional
   price heat-map: 5 columns (regions) × goods rows, color-coded cheap/expensive.

7. **Data-integrity selftest**; verify all `regionalSupply` values resolve to
   real regions.

8. **xUnit**: regional price lookup, best-region search, cross-region barter
   pricing, round-trip save/load.

9. **Balance sim**: `ashfall-balance-sim` verifies regional price spreads are
   wide enough to make route planning matter but not so wide that settlement
   goods are worthless.

**Next steps:** caravan route optimization UI (the player plans a route
to hit cheap regions); dynamic price shocks from weather events (14A+14B cross-wire).

---

## Task 14C — Weather forecasting payoff: the weather-station system predicts a crisis event

**Goal:** `WeatherStationSystem` is fully implemented in Core but its forecasts
never reach the player in a decision-relevant way. This task closes the loop:
a calibrated weather station gives the player advance warning of the crisis
events authored in Plan 13C, enabling proactive preparation instead of
reactive scrambling.

**Files:** `WeatherStationSystem.cs`, `WeatherIntelligenceCoordinator.cs`,
`WeatherSystem.cs` (read-only), `events.json` (read-only), `WeatherIntelligenceReadModel.cs`.

### Substeps

1. **Read** `WeatherStationSystem.GenerateForecast()` and the
   `WeatherIntelligenceReadModel` to understand the current forecast shape.

2. **Extend `WeatherIntelligenceReadModel`** with crisis-prediction fields:
   - `predictedCrisisEventId`: string — the event ID if a crisis is predicted
   - `predictedCrisisDay`: int — the day the crisis is expected
   - `predictedCrisisConfidence`: float — 0.0–1.0
   - `crisisPreparationAdvice`: string — "Stock water," "Brace the hatch," etc.
   - `predictedWeatherKind`: WeatherKind — the weather that triggers the crisis

3. **Extend `WeatherStationSystem`** with crisis prediction:
   - `PredictCrisis(int currentDay)` — scans the forecast horizon for
     weather states that have associated crisis events (from Plan 13C's
     `RequireWeather` conditions)
   - When a forecasted weather state matches a crisis event condition,
     the station flags it with a confidence based on station accuracy ×
     forecast day distance
   - `GetCrisisWarning()` returns the predicted crisis or null
   - Accuracy improves with calibration (0.7 → 0.9) and shorter horizons

4. **Extend `WeatherIntelligenceCoordinator`**:
   - `TickDay` now calls `Station.PredictCrisis()` after `GenerateForecast`
   - `ReadModel` includes the crisis prediction fields
   - Emits `OnCrisisPredicted` event the host can use for UI alerts

5. **Surface in the weather forecast panel**: `weather_forecast` panel shows:
   - 3-day forecast with weather icons and confidence bars
   - Crisis warnings highlighted in red with preparation advice
   - Route-safety indicators for expedition planning
   - "Station not calibrated" state shows ??? for all forecasts

6. **Surface in the daily briefing**: `DailyBriefingReportBuilder` includes
   the crisis warning when one is active.

7. **xUnit**: forecast generation, crisis prediction accuracy at different
   calibration levels, false-positive rate, round-trip save/load, determinism.

8. **Integration test**: `godot --headless --weather-forecast-selftest` —
   smoke-tests the full pipeline: station install → calibrate → generate →
   predict crisis → verify prediction matches scheduled event.

**Next steps:** cloud-seeding countermeasure (14D) cancels a predicted crisis;
weather-station sabotage (hostile faction event).

---

## Task 14D — Cloud-seeding countermeasure: cancels a crisis (strategic capstone)

**Goal:** Give the player a strategic counterplay to weather crises: the
cloud-seeding system. A high-cost, high-reward infrastructure project that
lets the player cancel ONE predicted crisis event at the cost of rare
chemicals and a cooldown period. This is the capstone of the weather
loop — the moment the player fights back against the sky.

**Files:** new `CloudSeedingSystem.cs` (Core), new `cloud_seeding.json` (data),
`WeatherSystem.cs`, `SkyLayerArmorSystem.cs`, `WeatherIntelligenceCoordinator.cs`.

### Substeps

1. **Author `CloudSeedingSystem`** in `Assets/Ashfall.Core/World/CloudSeedingSystem.cs`:
   - `IsInstalled`: bool
   - `IsOnCooldown`: bool — 7-day cooldown after each use
   - `CooldownRemaining`: int — days until next use
   - `Install(int day)` — requires specific items (see below)
   - `Deploy(int day, WeatherKind targetWeather)` → ActionResult:
     - Requires: `chemicals` × 4, `fuel` × 3, `item_desal_membrane` × 1
     - If the weather station predicts a crisis of `targetWeather`, cancels it
     - Sets 7-day cooldown
     - Success chance: 70% base, +10% if station is calibrated, -20% if
       deployed during the crisis itself (reactive, not proactive)
     - Failure: the weather continues but the chemicals are consumed
   - `TickDay(int day)` — decrements cooldown
   - `CaptureState` / `RestoreState`

2. **Author cloud-seeding items** in `items.json`:
   - `item_cloud_seed_dispenser` — the deployment mechanism (crafted at workbench)
   - `item_silver_iodide_cartridge` — the seeding agent (crafted at distiller)
   - `item_weather_sonde` — already referenced in panel registry; the launch tube

3. **Author cloud-seeding recipes** in `recipes.json`:
   - `craft_cloud_seed_dispenser` → workbench: `mechanical_parts` × 5, `scrap_metal` × 4, `electronic_scrap` × 3, `copper_wire_10m_of_10m` × 2
   - `craft_silver_iodide_cartridge` → distiller: `chemicals` × 3, `sulphur` × 2, `fuel` × 2
   - `craft_weather_sonde` → workbench: `scrap_metal` × 2, `cloth` × 1, `battery` × 1

4. **Wire `CloudSeedingSystem` into `WeatherIntelligenceCoordinator`**:
   - Coordinator owns the CloudSeedingSystem
   - `Deploy` checks the crisis prediction from the weather station
   - On success, `WeatherSystem.ForceWeather(WeatherKind.Clear)` for the
     crisis day (or the next safest weather)
   - Emits `OnCloudSeedingDeployed` event

5. **Wire into `SkyLayerArmorSystem`**:
   - Cloud seeding that fails during a glass storm or rad-hail still
     provides partial protection: -30% damage to ceiling armor for that day
     (the chemicals scatter and provide some attenuation)

6. **Surface in the weather forecast panel**: a "Deploy Cloud Seeding" button
   appears when a crisis is predicted, grayed out if on cooldown or missing
   materials. Shows success chance, cost, and cooldown.

7. **Data-integrity selftest**; verify new items and recipes resolve.

8. **xUnit**: install, deploy success, deploy failure, cooldown enforcement,
   partial protection during failure, round-trip save/load, determinism.

9. **Integration test**: `godot --headless --cloud-seeding-selftest` —
   full pipeline: install → calibrate station → predict crisis → deploy
   cloud seeding → verify crisis cancelled → verify cooldown.

**Next steps:** cloud-seeding overuse consequences (ecological side-effects);
hostile faction sabotage of the seeding installation.

---

## Task 14E — Trophy mounts feed shelter decor

**Goal:** Close the loop between the trapping system (Plan 13B) and the
shelter decor system. When a player butchers a rare animal and preserves
its hide, they can craft a trophy mount that provides a permanent localized
morale bonus in the shelter room where it's placed.

**Files:** `ShelterDecorSystem.cs`, `WildlifeTrappingSystem.cs` (read-only),
`items.json`, `recipes.json`, `economy_goods.json`.

### Substeps

1. **Read** `ShelterDecorSystem` to understand the `Assign(roomId, slotId, itemId)`
   API and the `GetRoomMoraleDelta(roomId)` morale pipeline.

2. **Author 8 trophy decor items** in `items.json`:
   - `item_decor_trophy_wolf_head` — Two-Headed Steppe Wolf mount (+3 morale, crafting room)
   - `item_decor_trophy_deer_antlers` — Wasteland Mule Deer antlers (+2 morale, common room)
   - `item_decor_trophy_boar_tusks` — Razorback Boar tusks (+3 morale, common room)
   - `item_decor_trophy_fox_pelt` — Barren Fox pelt wall hanging (+2 morale, sleeping quarters)
   - `item_decor_trophy_beetle_carapace` — Titan Slag-Back Beetle shell (+2 morale, workshop)
   - `item_decor_trophy_molerat_skull` — Tessarat Blind Mole-Rat skull (+1 morale, medical)
   - `item_decor_trophy_crow_feathers` — Three-Eyed Sentry crow feather display (+1 morale, radio room)
   - `item_decor_trophy_pheasant_plume` — Ash Pheasant tail plume (+1 morale, greenhouse)

   Each item has `type: "Decor"`, `tradeValue`, `moraleEffect`, and `isEquipable: false`.

3. **Author 8 trophy crafting recipes** in `recipes.json`:
   - Each recipe consumes: the hide/pelt from the corresponding quarry species
     (from `WildlifeTrappingSystem.PreserveHide`), plus `scrap_wood` × 2 and
     `chemicals` × 1 (for taxidermy preservation)
   - Example: `craft_trophy_wolf_head` → `leather_strap` × 2 + `scrap_wood` × 2 + `chemicals` × 1 → `item_decor_trophy_wolf_head` × 1
   - All recipes use `workbench` station, 4–6 hours crafting time

4. **Wire the trap-to-trophy bridge** in `WildlifeTrappingSystem`:
   - `PreserveHide` already returns `hideItemId` and `hideQuantity`
   - Add `GetTrophyRecipeForSpecies(string speciesId)` → returns the recipe ID
     for the trophy that species yields
   - Add `OnTrophyReady` event that the host can use to prompt the player

5. **Wire into `ShelterDecorSystem`**:
   - `Assign("common_room", "north_wall", "item_decor_trophy_wolf_head")` works
     with the existing API — no changes needed
   - Add `GetTrophySlots(string roomId)` → returns available trophy-specific
     slot IDs for that room
   - Add `GetTrophyMoraleModifier(string itemId)` → returns the morale bonus

6. **Surface in the shelter decor panel**: `shelter_decor` panel shows
   trophy-eligible slots with a distinct icon. Empty slots show "Place a trophy
   here" tooltip. Placed trophies show the species name and morale bonus.

7. **Data-integrity selftest**; verify all decor items and recipes resolve.

8. **xUnit**: trophy crafting, decor placement, morale bonus application,
   trap-to-trophy full pipeline, round-trip save/load.

9. **Balance sim**: `ashfall-balance-sim` verifies trophy morale bonuses don't
   make morale trivial — trophies are rare (require rare species catches) and
   provide modest localized bonuses, not global ones.

**Next steps:** trophy degradation (trophies fade over time, requiring
maintenance); legendary trophy variants (one-in-a-hundred catches yield
a unique decor item with a story).

---

## Task 14F — Guard-dog training & kennel system

**Goal:** The `war_dog_kennel` panel exists in the panel registry but has no
backing Core system. This task creates the kennel system: train a wasteland
canine companion that provides expedition bonuses (scent tracking reduces
encounter risk), shelter defense (alerts during hatch breach events), and
morale benefits.

**Files:** new `KennelSystem.cs` (Core), new `kennel.json` (data),
`items.json`, `recipes.json`, `WildlifeTrappingSystem.cs` (read-only),
`ExpeditionSystem.cs` (read-only), `events.json` (read-only).

### Substeps

1. **Read** `ExpeditionSystem` to identify the encounter-risk hook point, and
   `events.json` to identify hatch-breach event IDs that the dog can interact with.

2. **Author `KennelSystem`** in `Assets/Ashfall.Core/Shelter/KennelSystem.cs`:
   - `CanineCompanion` DTO: `dogId`, `name`, `breed`, `stage` (puppy/juvenile/adult/veteran),
     `trainingLevel` (0–100), `loyalty` (0–100), `health` (0–100), `dayAcquired`
   - `IsKennelBuilt`: bool
   - `BuildKennel(int day)` — consumes items
   - `AcquireDog(string dogId, string name, int day)` — from a capture event
     or trader
   - `Train(int day, string skill)` → ActionResult:
     - Skills: `scent_tracking` (reduces expedition encounter risk by 5–15%),
       `guard_alert` (hatch breach events give +1 choice option),
       `companion_morale` (passive +1–3 morale in the room the dog is in),
       `combat_assist` (combat encounters get +10% success chance)
     - Training takes 3–7 days per skill level
     - Max 2 active skills at a time
   - `TickDay(int day)` — ages the dog, degrades untrained skills, consumes
     `cooked_meat` × 0.5 per day (food cost)
   - `GetExpeditionBonus()` → float (encounter risk reduction)
   - `GetHatchBreachBonus()` → string (extra choice option ID)
   - `GetMoraleBonus()` → float
   - `CaptureState` / `RestoreState`

3. **Author kennel items** in `items.json`:
   - `item_kennel_blueprint` — the build plan (found in salvage or traded)
   - `item_dog_whistle` — recall tool (crafted)
   - `item_training_dummy` — training equipment (crafted)
   - `item_dog_armor_light` — protective vest for combat (crafted)
   - `item_dog_medkit` — canine first aid (crafted)

4. **Author kennel recipes** in `recipes.json`:
   - `craft_dog_whistle` → workbench: `scrap_metal` × 1, `leather_strap` × 1
   - `craft_training_dummy` → workbench: `cloth` × 3, `scrap_wood` × 2, `leather_strap` × 1
   - `craft_dog_armor_light` → workbench: `leather_strap` × 3, `scrap_metal` × 2, `duct_tape` × 1
   - `craft_dog_medkit` → workbench: `bandage` × 2, `anti_rad` × 1, `cloth` × 1

5. **Author dog acquisition events** in `events.json`:
   - `event_stray_dog_arrival` — a stray wasteland dog appears at the hatch
     (minDay 10, weight 1.5, Clear/Overcast weather)
   - `event_trader_sells_puppy` — a caravan trader offers a puppy for trade
     (minDay 15, requires caravan at node)
   - `event_wild_dog_pack` — a pack of feral dogs threatens the perimeter;
     player can capture one or drive them off (minDay 20, weight 2.0)

6. **Wire `KennelSystem` into `ExpeditionSystem`**:
   - `ExpeditionSystem.Start` accepts an optional `kennelBonus` float
   - If the dog is assigned to the expedition, reduce encounter risk

7. **Wire into the event system**:
   - Hatch breach events from Plan 13C check for an active guard dog
   - If the dog has `guard_alert` skill, the event offers a 4th choice:
     "The dog is barking — it knows something. Hold the hatch and listen."

8. **Surface in the kennel panel**: `war_dog_kennel` panel shows:
   - Dog name, portrait (silhouette + breed), stage, health bar, loyalty bar
   - Active skills with training progress bars
   - Food consumption rate
   - "Assign to Expedition" / "Assign to Guard" / "Assign to Common Room" buttons
   - Empty state: "No kennel built yet" with build button

9. **Data-integrity selftest**; verify all kennel items, recipes, and events.

10. **xUnit**: kennel build, dog acquisition, training progression, expedition
    bonus calculation, morale bonus, food consumption, aging, save/load round-trip,
    determinism.

11. **Balance sim**: `ashfall-balance-sim` verifies the food cost of a dog
    (0.5 cooked_meat/day) is meaningful but not crippling — a dog is a
    long-term investment, not a free bonus.

**Next steps:** dog breeding (two dogs produce puppies); combat dog
deployment (the dog fights alongside the player in combat encounters);
dog death & memorial (the dog gets a plaque in the shelter decor system).

---

## Cross-Task Dependencies

```
14A (embargoes) ──┐
                  ├── 14B (regional price maps) ── economy panel payoff
                  │
14C (forecast)  ──┼── 14D (cloud-seeding) ── weather-strategic capstone
                  │
13B (trapping)  ──┼── 14E (trophy mounts) ── shelter decor payoff
                  │
14E (trophies)  ──┼── 14F (guard-dog) ── expedition/shelter payoff
```

**Recommended execution order:**
1. **14A + 14B together** — they share the economy panel and the regional/trade
   data structures. Implementing them as a pair avoids rework.
2. **14C standalone** — the weather station is already implemented; this is
   mostly data-flow and UI surface work.
3. **14D after 14C** — cloud-seeding needs the crisis prediction from 14C.
4. **14E standalone** — depends on 13B (already done) but otherwise isolated.
5. **14F after 14E** — the kennel system is the largest new system; do it
   last when the shelter decor and economy loops are stable.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass
3. dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
5. godot --headless --path . -- --bridge-selftest               # Exits 0
```

---

## Estimated Effort

| Task | Systems | Data files | New items | New events | New recipes | xUnit tests | Difficulty |
|---|---|---|---|---|---|---|---|
| 14A | 1 new + 2 modified | 1 new JSON | 0 | 0 | 0 | 8–10 | Medium |
| 14B | 1 new + 2 modified | 1 new JSON | 0 | 0 | 0 | 6–8 | Medium |
| 14C | 2 modified | 0 | 0 | 0 | 0 | 6–8 | Low |
| 14D | 1 new + 2 modified | 1 new JSON | 3 | 0 | 3 | 10–12 | High |
| 14E | 1 modified | 0 | 8 | 0 | 8 | 8–10 | Medium |
| 14F | 1 new + 3 modified | 1 new JSON | 5 | 3 | 4 | 12–15 | High |
| **Total** | **4 new + 12 modified** | **4 new JSON** | **16** | **3** | **15** | **50–63** | — |