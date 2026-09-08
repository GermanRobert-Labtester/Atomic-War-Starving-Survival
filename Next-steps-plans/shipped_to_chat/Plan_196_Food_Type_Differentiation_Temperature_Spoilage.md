# Plan 196 — Food Type Differentiation & Temperature-Dependent Spoilage

## Goal

Create a food type differentiation and temperature-dependent spoilage system where different food categories (meat, vegetables, dairy, grains, prepared meals) spoil at different rates, temperature affects spoilage speed, and preservation methods are type-specific. Currently `KitchenNutritionSystem.cs` (288 lines) has basic spoilage timers and preservation methods (RootCellar, Refrigeration, Fermentation, Smoking, Canning) but no food type differentiation — all items use the same spoilage formula regardless of type. No temperature-dependent spoilage acceleration. No type-specific preservation. This plan adds culinary realism and strategic depth to food management.

## Why

**Repository evidence:** Grep for `FoodType`, `MeatSpoilage`, `DairySpoilage`, `VegetableSpoilage`, `TemperatureSpoilage`, `SpoilageRate`, `FoodCategory` in Core returns ZERO matches. `KitchenNutritionSystem.cs` (288 lines) has `spoilageTimer`, `maxSpoilageDays`, `PreservationMethod` enum, and `UpdateSpoilage()` that decrements timer daily — but all items use identical spoilage logic. `GetSpoilageDays()` returns 14 days (refrigeration), 5 days (cellar), or 2 days (none) regardless of food type. `cellarTempC` field exists but is not used in spoilage calculations. No food type differentiation, no temperature modeling, no type-specific preservation.

**What is missing:** No food type categories. No differentiated spoilage rates (meat spoils faster than grains). No temperature-dependent spoilage (hotter = faster spoilage). No type-specific preservation methods (dairy needs different handling than meat). No seasonal spoilage variation (summer vs winter). No food safety modeling (improperly preserved food causes illness).

**Why existing plans don't solve it:** Plan 164 (nuclear winter) adds seasonal temperature but not spoilage effects. Plan 135 (weather cascade) affects shelter but not food spoilage. Plan 22 (food authority) covers consumption but not spoilage differentiation. No plan addresses food type differentiation or temperature-dependent spoilage.

**Player value:** Creates strategic depth (choose preservation method by food type), adds realism (meat rots faster than grains), generates emergent stories (food poisoning from improperly preserved dairy), and makes food management more meaningful than just "store everything the same way."

## Files / Systems to Inspect

- `Assets/Ashfall.Core/KitchenNutritionSystem.cs` — current spoilage system
- `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs` — item instances
- `Assets/Ashfall.Core/World/WeatherSystem.cs` — temperature data
- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs` — shelter temperature
- NEW: `Assets/Ashfall.Core/Kitchen/FoodTypeSystem.cs`
- NEW: `Assets/StreamingAssets/Data/food_types.json`

## Main Task 1 — Foundation / System Contract

1. Create `FoodTypeSystem.cs` in `Assets/Ashfall.Core/Kitchen/`
2. Define `FoodType` DTO: `typeId`, `typeName` (meat/vegetable/dairy/grain/fruit/prepared_meal/preserved), `category` (perishable/semi_perishable/shelf_stable), `baseSpoilageRate` (days at 20°C without preservation), `temperatureSensitivity` (0-1, how much temperature affects spoilage), `optimalPreservation` (list of best preservation methods for this type), `preservationEffectiveness` (dict of preservation method → effectiveness multiplier)
3. Define `FoodItem` DTO: `itemId`, `foodTypeId`, `productionDay`, `currentCondition` (0-100, 100=fresh), `spoilageThreshold` (condition where food becomes unsafe), `currentTemperature` (°C, last known), `preservationMethod` (applied preservation), `preservationDay` (when preservation applied), `isSpoiled` bool
4. Define `TemperatureSpoilageModifier` DTO: `temperatureRange` (min-max °C), `spoilageMultiplier` (how much faster/slower spoilage at this temperature), `safetyThreshold` (temperature where food becomes unsafe immediately)
5. Define `PreservationEffectiveness` DTO: `preservationMethod`, `foodTypeId`, `effectivenessMultiplier` (0-1, how well this method works for this food type), `durationMultiplier` (how much preservation extends shelf life), `qualityRetention` (0-1, how well preservation maintains food quality)
6. Define `FoodSpoilageState` DTO: list of food items with conditions, list of temperature readings, spoilage settings (global temperature modifier, spoilage rate modifier)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define food type categories (7+ types):
   - **Meat**: highly perishable, 2 days at 20°C, high temperature sensitivity, best preserved by smoking/canning/freezing
   - **Vegetables**: moderately perishable, 5 days at 20°C, medium temperature sensitivity, best preserved by root cellar/fermentation/canning
   - **Dairy**: highly perishable, 1 day at 20°C, very high temperature sensitivity, best preserved by refrigeration/fermentation (cheese/yogurt)
   - **Grains**: shelf-stable, 30+ days at 20°C, low temperature sensitivity, best preserved by drying/sealing
   - **Fruits**: moderately perishable, 4 days at 20°C, medium temperature sensitivity, best preserved by drying/canning/fermentation
   - **Prepared Meals**: highly perishable, 1 day at 20°C, very high temperature sensitivity, best preserved by refrigeration
   - **Preserved Foods**: shelf-stable (already preserved), 60+ days, low temperature sensitivity
9. Define temperature spoilage modifiers:
   - **-10°C to 0°C**: 0.2x spoilage rate (freezing slows dramatically)
   - **0°C to 5°C**: 0.4x spoilage rate (refrigeration)
   - **5°C to 15°C**: 0.7x spoilage rate (cool cellar)
   - **15°C to 25°C**: 1.0x spoilage rate (room temperature, baseline)
   - **25°C to 35°C**: 1.5x spoilage rate (warm, accelerates spoilage)
   - **35°C+**: 3.0x spoilage rate (hot, rapid spoilage)
   - **50°C+**: instant spoilage (unsafe)
10. Define preservation effectiveness by food type:
    - **Meat + Smoking**: 0.8x effectiveness, 10x duration, 0.7 quality retention
    - **Meat + Canning**: 0.9x effectiveness, 20x duration, 0.8 quality retention
    - **Meat + Refrigeration**: 0.6x effectiveness, 3x duration, 0.9 quality retention
    - **Dairy + Refrigeration**: 0.9x effectiveness, 5x duration, 0.95 quality retention
    - **Dairy + Fermentation**: 0.85x effectiveness, 15x duration, 0.8 quality retention (cheese/yogurt)
    - **Vegetables + Root Cellar**: 0.7x effectiveness, 4x duration, 0.85 quality retention
    - **Vegetables + Fermentation**: 0.8x effectiveness, 12x duration, 0.75 quality retention (pickles)
    - **Vegetables + Canning**: 0.9x effectiveness, 18x duration, 0.8 quality retention
    - **Grains + Drying**: 0.95x effectiveness, 25x duration, 0.9 quality retention
    - **Fruits + Drying**: 0.85x effectiveness, 10x duration, 0.7 quality retention
    - **Fruits + Canning**: 0.9x effectiveness, 15x duration, 0.85 quality retention
11. Define spoilage mechanics:
    - Each food item has condition (0-100, 100=fresh)
    - Condition decreases daily based on: food type base rate × temperature modifier × preservation effectiveness
    - Below 50% condition: food is "aging" (still safe but lower quality)
    - Below 25% condition: food is "spoiling" (risk of illness if consumed)
    - Below 10% condition: food is "spoiled" (unsafe, causes illness)
    - At 0% condition: food is "rotten" (completely inedible)
12. Define food safety mechanics:
    - Consuming spoiled food (below 25%) causes illness
    - Illness severity based on how far below threshold
    - Food poisoning: nausea, vomiting, reduced work capacity for 1-3 days
    - Severe food poisoning: requires medical treatment
    - Properly preserved food never causes illness
    - Visual inspection reveals condition (can detect spoilage)
13. Define temperature tracking:
    - Food items track current temperature
    - Temperature updates based on storage location (shelter temperature, outside temperature, refrigerator)
    - Temperature affects spoilage rate calculation
    - Temperature history logged
14. Add deterministic seeding: spoilage uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupFoodTypes`, `TickFoodTypes`, `SaveFoodTypes`

## Main Task 2 — Implementation / Types / Temperature / Preservation / Safety

1. Implement food type assignment:
   - Each food item gets food type from item definition
   - Food type determines base spoilage rate
   - Food type determines optimal preservation
   - Food type displayed in item detail
2. Implement temperature-dependent spoilage:
   - Each day, calculate spoilage based on current temperature
   - Apply temperature modifier to base spoilage rate
   - Update food condition
   - Temperature changes (seasonal, shelter) affect spoilage
   - Spoilage logged
3. Implement preservation effectiveness:
   - When preservation applied, check food type + method
   - Calculate effectiveness multiplier
   - Apply preservation to extend shelf life
   - Preservation quality affects final food quality
   - Preservation logged
4. Implement food condition tracking:
   - Each food item has 0-100 condition
   - 100-50: fresh (green)
   - 49-25: aging (yellow) — still safe
   - 24-10: spoiling (orange) — risk of illness
   - 9-0: spoiled/rotten (red) — unsafe
   - Condition displayed in UI
5. Implement food safety checks:
   - When food consumed, check condition
   - Below 25%: roll for food poisoning
   - Severity based on how far below threshold
   - Food poisoning applied to survivor
   - Safety check logged
6. Implement temperature tracking:
   - Food items track current temperature
   - Temperature updates from storage location
   - Shelter temperature from `ShelterThermalSystem`
   - Outside temperature from `WeatherSystem`
   - Refrigerator temperature (if available)
   - Temperature displayed in UI
7. Implement preservation UI:
   - Food storage panel: all food items with condition/temperature
   - Preservation panel: select food, choose method, apply preservation
   - Spoilage warning: food nearing spoilage highlighted
   - Temperature display: current storage temperature
   - Food safety indicator: safe/aging/spoiling/spoiled
8. Implement seasonal spoilage:
   - Summer: higher temperatures, faster spoilage
   - Winter: lower temperatures, slower spoilage
   - Nuclear winter: cold temperatures, very slow spoilage
   - Seasonal spoilage variation creates strategic planning
9. Implement food storage optimization:
   - Refrigerator: best for dairy, meat, prepared meals
   - Root cellar: best for vegetables, fruits
   - Smoking station: best for meat, fish
   - Canning station: best for vegetables, fruits, meat
   - Fermentation vat: best for vegetables, dairy
   - Drying rack: best for grains, fruits, herbs
10. Create spoilage events:
    - "The Spoilage" — food spoiled
    - "The Poisoning" — survivor ate spoiled food
    - "The Preservation" — food successfully preserved
    - "The Temperature" — temperature change affected spoilage
    - "The Storage" — food stored optimally
    - "The Waste" — food wasted (spoiled)
    - "The Harvest" — fresh food acquired
    - "The Stockpile" — large food reserve maintained
11. Add spoilage quest hooks:
    - "The Chef" — maintain 20 fresh food items
    - "The Preserver" — preserve 50 food items
    - "The Stockpile" — maintain 100 food items in storage
    - "The Temperature" — keep all food at optimal temperature for 30 days
    - "The Variety" — store all 7 food types
    - "The Safety" — no food poisoning incidents for 60 days
    - "The Efficiency" — preserve food with 90%+ effectiveness
12. Implement spoilage tutorial: first spoiled food explains system
13. Add spoilage tooltips: hover over food shows condition, temperature, spoilage rate
14. Create food type definitions in data file (7+ types)
15. Implement spoilage persistence: food conditions saved with game state

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `KitchenNutritionSystem`: food type spoilage integrated
2. Connect to `ShelterThermalSystem`: shelter temperature affects spoilage
3. Integrate with `WeatherSystem`: outside temperature affects spoilage
4. Connect to `DiseaseSystem`: food poisoning integrated
5. Wire into `NeedsSystem`: spoiled food affects hunger/health
6. Connect to `InventorySystem`: food items tracked
7. Implement old-save compatibility: existing saves get default food type (grain)
8. Add deterministic seeding: spoilage uses `ISeededRng`
9. Create exploit prevention: spoilage is time/temperature-based, can't be gamed
10. Add tests: food types, temperature effects, preservation effectiveness, food safety, save round-trip
11. Verify all food types spoil correctly
12. Test edge cases: no spoilage (frozen), rapid spoilage (hot + unpreserved)
13. Verify headless behavior: spoilage processes correctly without UI
14. Add data-integrity-selftest: food types validate against item catalogs
15. Create `--food-type-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --food-type-selftest
```

## Risk

**LOW** — Food type differentiation is straightforward with clear inputs (food type, temperature, preservation) and outputs (spoilage rate, condition changes). Risk of food management becoming tedious. Mitigation: make preservation easy, show clear spoilage warnings, allow bulk operations, and ensure food safety is predictable not random.

## Definition of Done

- `FoodTypeSystem.cs` exists with full `CaptureState/RestoreState`
- 7+ food types (meat, vegetable, dairy, grain, fruit, prepared meal, preserved)
- Temperature-dependent spoilage (6 temperature ranges with modifiers)
- Preservation effectiveness by food type (11+ combinations)
- Food condition tracking (0-100, color-coded)
- Food safety mechanics (food poisoning from spoiled food)
- Temperature tracking per food item
- Seasonal spoilage variation
- Food storage optimization (refrigerator, cellar, smoking, canning, fermentation, drying)
- Spoilage events and quest hooks
- Save/load round-trip tested
- Deterministic spoilage verified
- Old saves load with default food type
- Food type definitions in data authority
- UI food storage panel, preservation panel, spoilage warnings
- Cross-system integration (kitchen nutrition, shelter thermal, weather, disease, needs, inventory)

## Follow-On Opportunities

- Food type specialization (survivors become expert preservers)
- Food type legacy (famous recipes remembered)
- Food type quests (specific preservation goals)
- Food type events (food festivals, harvest celebrations)
- Food type trading (trade preserved foods between settlements)
