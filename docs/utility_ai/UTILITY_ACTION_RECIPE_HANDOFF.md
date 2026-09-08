# Utility Action Recipe Handoff

## Architecture

The Utility AI Core has **no recipe reference field**. Recipe selection is executor-owned.

The action ID serves as a contract: the executor maps the action ID to a recipe category or specific recipe.

## Action → Recipe Mapping

| Action ID | Recipe Category | Executor Selects |
|-----------|----------------|-----------------|
| `action_cook_food` | Cooking recipes | Eligible recipe from available ingredients |
| `action_preserve_food` | Preservation recipes | Eligible preservation recipe from perishables |
| `action_purify_water` | Water treatment recipes | Filtration/boiling/chemical treatment |

## Recipe Handoff Contract

1. **No recipe IDs in action data**: The action catalog does not reference specific `recipe_*` IDs.
2. **Executor selects recipe**: The executor chooses the best eligible recipe based on available ingredients and survivor skill.
3. **Recipe validation**: The recipe system (`Assets/Ashfall.Core/Crafting/`) validates ingredients, workstation, and skill requirements.
4. **Output handling**: The recipe system produces the output; Utility AI doesn't touch inventory.

## Resource Safety

- **Protected ingredients**: The recipe system should exclude protected/quest ingredients.
- **Rare ingredient protection**: Prefer common recipes over rare ones when multiple options exist.
- **Stock targets**: Stop cooking when prepared-food stock meets the target threshold.

## Plan 72 Position

All three food/water actions are added to the catalog. Recipe selection and resource management are deferred to the executor and recipe system. The action data does not contain recipe references.