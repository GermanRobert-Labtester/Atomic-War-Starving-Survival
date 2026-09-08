# Utility Action Resource Policy

## Architecture

The Utility AI Core does **not** consume resources. It selects an action ID. The executor in the host layer is responsible for:
1. Checking resource availability
2. Reserving/consuming resources
3. Producing outputs

The action data (`baseScore`, `fatigueGate`, `skillBonusFactor`) influences how likely an action is to be selected, but does not control resource consumption.

## Resource-Consuming Actions

| Action ID | Resources Consumed | Resources Produced | Guardrail |
|-----------|-------------------|-------------------|-----------|
| `action_cook_food` | Ingredients, fuel | Prepared food | Recipe system validates |
| `action_preserve_food` | Perishables, salt/containers | Preserved food | Preservation system validates |
| `action_purify_water` | Unsafe water, fuel/filters | Clean water | Water system validates |
| `action_treat_wounded` | Medical supplies | Treated patient | Medical system validates |
| `action_seek_treatment` | Medical supplies | Self-treatment | Medical system validates |
| `action_repair_equipment` | Repair materials | Repaired equipment | Repair system validates |

## Resource Safety Rules

1. **Never consume protected/quest items**: The executor must check item flags before consuming.
2. **Don't consume from insufficient stock**: The executor validates availability before starting.
3. **Don't overproduce**: Stop cooking/purifying when stock targets are met.
4. **Don't waste rare resources**: Medical actions should prefer common supplies over rare ones.
5. **Failed actions don't consume**: If the action can't complete, resources should not be consumed.

## Bounded Autonomy

The Utility AI should not be able to:
- Cook through all ingredients leaving nothing for emergencies
- Use rare medicine for trivial symptoms
- Purify water when clean water is already sufficient
- Repair equipment that isn't degraded

These bounds are enforced by the executor, not the action data. The action data only makes the action more or less likely to be selected.