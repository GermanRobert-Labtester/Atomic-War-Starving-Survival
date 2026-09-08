# Utility Action Skill Handoff

## Architecture

The Utility AI Core uses a single skill field: `CraftingSkill` (float 0-1). This is the only skill dimension in `AIActionContext`.

The `skillBonusFactor` field on each action determines how much `CraftingSkill` contributes to `EvaluateRaw`:

```
rawScore = baseScore + CraftingSkill * skillBonusFactor
```

## Actions with Skill Integration

| Action ID | skillBonusFactor | Skill Effect |
|-----------|-----------------|--------------|
| `action_repair_equipment` | 0.25 | Skilled crafters repair more readily |
| `action_treat_wounded` | 0.30 | Skilled medics treat more readily |
| `action_conduct_research` | 0.30 | Skilled researchers research more readily |
| `action_teach_skill` | 0.25 | Skilled teachers teach more readily |
| `action_weigh_goods` | 0.25 | (existing) |
| `action_read_contract` | 0.20 | (existing) |
| `action_canvas_support` | 0.15 | (existing) |
| `action_run_vouch` | 0.10 | (existing) |
| `action_cook_food` | 0.20 | Skilled cooks cook more readily |
| `action_preserve_food` | 0.15 | Skilled preserve more readily |
| `action_purify_water` | 0.15 | Skilled purify more readily |
| `action_train_skill` | 0.20 | Skilled train more readily |
| `action_resolve_conflict` | 0.10 | Skilled mediate more readily |
| `action_stand_watch` | 0.15 | Skilled guards watch more readily |
| `action_inspect_housing` | 0.10 | Skilled inspect more readily |

## Skill Specialization

The current `AIActionContext` has only `CraftingSkill` — a single generic skill. There is no medical skill, repair skill, cooking skill, etc.

The `skillBonusFactor` uses the same `CraftingSkill` value for all actions. This means:
- A survivor with high `CraftingSkill` scores higher on ALL skill-bonus actions
- There is no domain-specific skill matching

This is a simplification that works for the companion-bias Utility AI. A more sophisticated skill system would require expanding `AIActionContext` with domain-specific skill fields.

## Plan 72 Position

Three actions have explicit skill integration via `skillBonusFactor > 0`:
1. `action_repair_equipment` (0.25)
2. `action_treat_wounded` (0.30)
3. `action_conduct_research` (0.30)

These use the existing `CraftingSkill` field. Domain-specific skill matching (e.g., checking if the survivor has `skill_medicine` before treating) is deferred to the executor.