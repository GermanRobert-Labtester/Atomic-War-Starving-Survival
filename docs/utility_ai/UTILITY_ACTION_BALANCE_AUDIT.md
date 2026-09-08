# Utility Action Balance Audit

## Priority Band Design

The 20 actions are organized into priority bands based on `baseScore`:

### Emergency Band (baseScore ≥ 0.50)
| Action | baseScore | Purpose |
|--------|-----------|---------|
| `action_treat_wounded` | 0.55 | Medical response |
| `action_rest` | 0.50 | Fatigue recovery |

### High Survival Band (baseScore 0.40–0.49)
| Action | baseScore | Purpose |
|--------|-----------|---------|
| `action_canvas_support` | 0.45 | Petition support |
| `action_purify_water` | 0.45 | Clean water |
| `action_seek_treatment` | 0.45 | Self-care |
| `action_weigh_goods` | 0.40 | Cargo verification |
| `action_cook_food` | 0.40 | Food preparation |

### Routine Survival Band (baseScore 0.30–0.39)
| Action | baseScore | Purpose |
|--------|-----------|---------|
| `action_read_contract` | 0.35 | Contract review |
| `action_audit_inventory` | 0.35 | Stock verification |
| `action_file_report` | 0.35 | Report filing |
| `action_repair_equipment` | 0.35 | Equipment repair |
| `action_resolve_conflict` | 0.35 | Conflict mediation |
| `action_stand_watch` | 0.35 | Security watch |
| `action_run_vouch` | 0.30 | Stranger vouch |
| `action_preserve_food` | 0.30 | Food preservation |

### Discretionary Band (baseScore 0.20–0.29)
| Action | baseScore | Purpose |
|--------|-----------|---------|
| `action_inspect_housing` | 0.25 | Preventive inspection |
| `action_socialize` | 0.20 | Social contact |
| `action_conduct_research` | 0.20 | Research |

### Long-Horizon Band (baseScore < 0.20)
| Action | baseScore | Purpose |
|--------|-----------|---------|
| `action_train_skill` | 0.15 | Self-training |
| `action_teach_skill` | 0.15 | Teaching |

## Category Distribution

| Category | Count | Actions |
|----------|-------|---------|
| Companion flavor | 4 | weigh_goods, read_contract, canvas_support, run_vouch |
| Shelter labor | 2 | audit_inventory, file_report |
| Maintenance | 2 | repair_equipment, inspect_housing |
| Medical | 2 | treat_wounded, seek_treatment |
| Food | 2 | cook_food, preserve_food |
| Water | 1 | purify_water |
| Social | 2 | socialize, resolve_conflict |
| Training | 2 | train_skill, teach_skill |
| Security | 1 | stand_watch |
| Research | 1 | conduct_research |
| Rest | 1 | rest |

## Weight Design

All 20 actions use `weight: 1.0`. This is intentional:
- Weight is a multiplier, not a priority
- Priority differences are expressed through baseScore
- Consistent weight makes the scoring hierarchy transparent
- Override actions (none in this catalog) would use weight to exceed the clamp

## Survival Preemption

With baseScore hierarchy:
- `treat_wounded` (0.55) > `cook_food` (0.40) > `socialize` (0.20) > `train_skill` (0.15)
- Medical response wins over food preparation
- Food preparation wins over socializing
- Socializing wins over training
- Rest (0.50) is always available and competes with productive work

## Skill Bonus Effects

At max `CraftingSkill` (1.0):
- `treat_wounded`: 0.55 + 0.30 = 0.85 (still highest)
- `conduct_research`: 0.20 + 0.30 = 0.50 (competitive with routine labor)
- `repair_equipment`: 0.35 + 0.25 = 0.60 (competitive with survival)
- `teach_skill`: 0.15 + 0.25 = 0.40 (competitive with routine labor)

## Fatigue Gate Design

| Fatigue Gate | Actions | Meaning |
|-------------|---------|---------|
| 0 | rest | Always available |
| 70 | train_skill, teach_skill | Need moderate energy |
| 75 | conduct_research | Need moderate energy |
| 80 | cook_food, preserve_food, purify_water, repair_equipment, audit_inventory, file_report, canvas_support | Standard work gates |
| 85 | socialize, resolve_conflict, stand_watch, inspect_housing, weigh_goods | Moderate fatigue tolerance |
| 88 | run_vouch | High fatigue tolerance |
| 90 | treat_wounded, read_contract | Can push through fatigue |
| 95 | seek_treatment | Almost always available |

## No Dominant Action

No single action dominates all states:
- At high fatigue: `rest` and `seek_treatment` remain available
- At low fatigue with skill: `treat_wounded` wins (base 0.55 + skill)
- At low fatigue without skill: multiple actions compete in the 0.35-0.45 range
- The companion flavor actions (0.30-0.45) provide texture without dominating