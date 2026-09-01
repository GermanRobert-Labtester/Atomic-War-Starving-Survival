# Plan 136 — Wildlife Trapping → Food Pipeline & Cooking System

## Goal

Connect the dead-end `WildlifeTrappingSystem` to the player's inventory and hunger system, then add a cooking/food-safety layer that transforms raw game into safe meals with distinct nutritional profiles. This closes the gap where trapping produces catches that never reach the player, and creates a meaningful food-preparation decision loop.

## Why

**Repository evidence:** `WildlifeTrappingSystem.cs` (471 lines) has full trapping mechanics (bait, quarry species, trap types, butchery, hide preservation) but **zero references to `AddItem`, `InventorySystem`, or `inventory.Add`**. Catches are tracked in `TrapSite` state but never transferred to player inventory. `KitchenNutritionSystem.cs` exists but doesn't read from trapping. `NeedsSystem` handles hunger restoration but has no connection to trapping output. No cooking system exists in Core.

**What is missing:** Players set traps, animals are caught, butchery occurs, but the meat never reaches inventory. The entire trapping gameplay loop is a dead end. Additionally, there is no cooking mechanic — raw irradiated meat exists as items but no preparation system transforms it into safe, nutritious meals.

**Why existing plans don't solve it:** Plan 13 (economy survival loop) mentions "active trapping/hunting" but doesn't address the inventory connection. Plan 36 (wildlife trapping catalog) adds trap/quarry data but not the pipeline. Plan 91 (greenhouse items) adds food items but not cooking. No plan connects trapping output to inventory or adds a cooking system.

**Player value:** Makes trapping a viable food source, creates cooking decisions (raw vs. cooked, irradiated vs. safe), adds food safety mechanics (decontamination through cooking), and provides meaningful choices about resource allocation (time spent cooking vs. other tasks).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` — trapping mechanics (dead end)
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs` — kitchen/food restoration
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — hunger restoration
- `Assets/Ashfall.Core/Inventory/` — inventory system
- `Assets/StreamingAssets/Data/items.json` — food items
- NEW: `Assets/Ashfall.Core/Cooking/CookingSystem.cs`
- NEW: `Assets/StreamingAssets/Data/recipes_cooking.json`

## Main Task 1 — Foundation / System Contract

1. Create `CookingSystem.cs` in `Assets/Ashfall.Core/Cooking/`
2. Define `CookingRecipe` DTO: `id`, `inputItems` (list of item IDs + quantities), `outputItemId`, `cookTimeMinutes`, `requiredEquipment` (e.g., "stove", "fire", "oven"), `nutritionValue`, `radiationRemoval` (0-1 fraction)
3. Define `CookingState` DTO: list of active cooking operations, list of completed recipes, cooking skill level
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define cooking equipment types: improvised stove, basic water boiler, proper oven, industrial cooker
6. Implement radiation removal mechanic: cooking removes fraction of radiation from contaminated food
7. Define food safety levels: raw (radiated, low nutrition), cooked (safe, medium nutrition), well-cooked (safe, high nutrition), burnt (safe, low nutrition, morale penalty)
8. Create `ICookingSource` interface for inventory system to provide raw ingredients
9. Implement cooking skill progression: repeated cooking increases skill, reduces cook time, improves nutrition
10. Wire `WildlifeTrappingSystem` output to inventory: add `TransferCatchToInventory()` method
11. Create trapping inventory integration: butchery produces raw meat items in inventory
12. Add deterministic seeding: cooking outcomes use `ISeededRng`
13. Wire into `GameBootstrap`: `SetupCooking`, `TickCooking`, `SaveCooking`
14. Create `CookingRecipeCatalogLoader` for recipe data

## Main Task 2 — Implementation / Content / Cooking Loop

1. Implement trapping → inventory transfer:
   - `WildlifeTrappingSystem.OnButcheryCompleted` triggers inventory addition
   - Raw meat items added to inventory with radiation contamination flag
   - Hide items added separately (for crafting)
2. Create cooking UI panel:
   - Select raw ingredients from inventory
   - Choose recipe (if known)
   - Select cooking equipment
   - Start cooking (time-gated)
   - Cancel cooking (ingredients returned)
3. Implement cooking mechanics:
   - Cooking consumes fuel (wood, charcoal, gas)
   - Cook time varies by recipe and equipment quality
   - Skill level reduces cook time and improves nutrition
   - Failed cooking (interruption, wrong recipe) produces burnt food
4. Create radiation decontamination through cooking:
   - Raw irradiated meat: high radiation, low nutrition
   - Cooked: 50% radiation removed, medium nutrition
   - Well-cooked: 80% radiation removed, high nutrition
   - Boiled (water-based): 90% radiation removed, medium nutrition
5. Implement 15 cooking recipes:
   - Basic: roasted meat, boiled meat, meat stew, dried jerky
   - Advanced: herb-crusted roast, spiced stew, meat pie
   - Specialized: irradiated meat detox (long cook time), survival rations (long shelf life)
   - Each recipe has distinct nutrition/radiation/morale effects
6. Add cooking skill progression:
   - Skill increases with successful cooks
   - Higher skill unlocks advanced recipes
   - Skill reduces cook time by up to 30%
   - Skill improves nutrition value by up to 20%
7. Create cooking equipment upgrades:
   - Improvised stove: basic cooking, slow, high fuel cost
   - Basic water boiler: boiling recipes, medium speed
   - Proper oven: baking recipes, fast, efficient
   - Industrial cooker: bulk cooking, fastest, lowest fuel cost
8. Implement food spoilage:
   - Cooked food spoils after 3-7 days (depending on recipe)
   - Spoiled food causes affliction if eaten
   - Dried/smoked recipes have longer shelf life (30 days)
9. Add cooking events:
   - "Excellent meal" — critical success, morale bonus
   - "Kitchen accident" — minor injury chance
   - "Recipe discovery" — unlock new recipe through experimentation
10. Create cooking quest hooks:
    - "The Last Chef" — survivor with cooking skill shares recipes before death
    - "Feast or Famine" — prepare a large meal for shelter morale event
    - "Toxic Harvest" — cook heavily irradiated meat to make it safe
11. Add UI: cooking panel with recipe book, ingredient selection, cooking queue
12. Implement cooking journal: automatic log of recipes discovered and cooking skill progression
13. Create cooking interaction with other systems:
    - `KitchenNutritionSystem`: cooked food provides better nutrition
    - `NeedsSystem`: hunger restoration varies by food quality
    - `MentalHealthCrisisSystem`: good meals improve morale
14. Add 15 cooking recipes to `recipes_cooking.json`

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `WildlifeTrappingSystem`: butchery completion triggers inventory transfer
2. Connect to `InventorySystem`: raw meat and cooked food stored in inventory
3. Integrate with `NeedsSystem`: cooked food restores hunger with nutrition value
4. Connect to `RadiationSystem`: cooking reduces food radiation contamination
5. Wire into `KitchenNutritionSystem`: cooked food provides better nutrition than raw
6. Connect to `MentalHealthCrisisSystem`: good meals provide morale bonus
7. Implement old-save compatibility: existing saves get empty cooking state, trapping catches retroactively transferred
8. Add deterministic seeding: cooking outcomes use `ISeededRng`
9. Create exploit prevention: cooking has fuel costs, can't be spammed
10. Add tests: trapping→inventory transfer, cooking lifecycle, save round-trip, determinism
11. Verify catalog integrity: all recipe item IDs resolve to real items
12. Test edge cases: no fuel (cooking blocked), no ingredients (cooking blocked), all food irradiated (cooking required)
13. Verify headless behavior: cooking ticks correctly without UI
14. Add data-integrity-selftest: cooking recipes validate against item catalog
15. Create `--cooking-system-selftest` verb for CI validation

## State / System Interaction Model

```text
Wildlife trapping produces catch
├─ Butchery completed
│  ├─ Raw meat added to inventory (irradiated, low nutrition)
│  ├─ Hide added to inventory (for crafting)
│  └─ Toxic meat flagged (requires cooking or detox)
├─ Player chooses to cook
│  ├─ Select recipe + ingredients + equipment
│  ├─ Cooking consumes fuel and time
│  ├─ Cooking skill affects outcome
│  │  ├─ Success: cooked food (safe, nutritious)
│  │  ├─ Critical success: excellent meal (morale bonus)
│  │  └─ Failure: burnt food (low nutrition, morale penalty)
│  └─ Radiation removed based on cook quality
│     ├─ Raw: 0% removed
│     ├─ Cooked: 50% removed
│     ├─ Well-cooked: 80% removed
│     └─ Boiled: 90% removed
├─ Cooked food stored in inventory
│  ├─ Spoils after 3-7 days
│  ├─ Eaten to restore hunger
│  └─ Nutrition value affects needs restoration
└─ Cooking skill increases with practice
   ├─ Unlocks advanced recipes
   ├─ Reduces cook time
   └─ Improves nutrition value
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --cooking-system-selftest
```

## Risk

**MEDIUM** — Cooking system complexity can overwhelm players if too many recipes and equipment tiers exist. Risk of cooking feeling like a chore rather than a meaningful choice. Mitigation: start with 5 basic recipes, unlock advanced recipes gradually, make cooking optional (raw food still works but less efficient).

## Definition of Done

- `CookingSystem.cs` exists with full `CaptureState/RestoreState`
- `WildlifeTrappingSystem` output connected to inventory (dead end fixed)
- Cooking mechanics functional (recipes, equipment, fuel, skill)
- Radiation decontamination through cooking implemented
- 15 cooking recipes in data authority
- Cooking skill progression working
- Food spoilage mechanic functional
- Save/load round-trip tested
- Deterministic cooking outcomes verified
- Old saves load without error
- UI panel shows cooking interface
- Cross-system integration (trapping, inventory, needs, radiation, kitchen, mental health)

## Follow-On Opportunities

- Fermentation/preservation system (long-term food storage)
- Cooking competitions (shelter morale events)
- Recipe trading (exchange recipes with other settlements)
- Cooking specialization (survivor skill tree)
- Food poisoning mechanic (bad cooking causes affliction)
