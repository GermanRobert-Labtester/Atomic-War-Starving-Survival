# Utility Action Recipe Handoff

> **Recipe Seams:** Interfacing food and water utility actions with Plan 55 crafting/cooking recipes (`recipes.json`).

---

## 1. Recipe Selection Rules

Utility AI does **not** hardcode recipe IDs into action data. Instead:

- Utility AI scores and selects the high-level intent: `action_cook_food`, `action_preserve_food`, or `action_purify_water`.
- The corresponding subsystem executor (`KitchenSystem`, `WaterSystem`, `CraftingSystem`) evaluates the live inventory and selects the highest-priority eligible recipe:
  - For `action_cook_food`: Evaluates cooked meal recipes (e.g. `recipe_stew_hearty`, `recipe_mash_protein`) against available root vegetables, fungal meat, and clean water.
  - For `action_preserve_food`: Evaluates drying, salting, or smoking recipes against perishable harvest batches.
  - For `action_purify_water`: Evaluates `recipe_water_boil` or chemical filter tablets against raw contaminated water stocks.

---

## 2. Resource Protection

- Autonomous cooking and crafting never consume items flagged with quest or unique lore tags.
- Batch sizes are clamped to the shelter's immediate consumption needs, preventing depletion of raw ingredient stockpiles.
