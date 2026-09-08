# Utility Action Content Utilization

## Action Inventory — 20 Actions

| # | ID | Category | baseScore | fatigueGate | skillBonus | Tags |
|---|----|----------|-----------|-------------|------------|------|
| 1 | `action_weigh_goods` | Companion | 0.40 | 85 | 0.25 | loud_labor |
| 2 | `action_read_contract` | Companion | 0.35 | 90 | 0.20 | — |
| 3 | `action_canvas_support` | Companion | 0.45 | 80 | 0.15 | menial_labor |
| 4 | `action_run_vouch` | Companion | 0.30 | 88 | 0.10 | — |
| 5 | `action_audit_inventory` | Shelter | 0.35 | 80 | 0.00 | quiet_labor |
| 6 | `action_file_report` | Shelter | 0.35 | 80 | 0.00 | quiet_labor |
| 7 | `action_repair_equipment` | Maintenance | 0.35 | 80 | 0.25 | loud_labor |
| 8 | `action_inspect_housing` | Maintenance | 0.25 | 85 | 0.10 | quiet_labor |
| 9 | `action_treat_wounded` | Medical | 0.55 | 90 | 0.30 | medical_triage |
| 10 | `action_seek_treatment` | Medical | 0.45 | 95 | 0.00 | medical |
| 11 | `action_cook_food` | Food | 0.40 | 80 | 0.20 | quiet_labor |
| 12 | `action_preserve_food` | Food | 0.30 | 80 | 0.15 | menial_labor |
| 13 | `action_purify_water` | Water | 0.45 | 80 | 0.15 | loud_labor |
| 14 | `action_socialize` | Social | 0.20 | 85 | 0.00 | — |
| 15 | `action_resolve_conflict` | Social | 0.35 | 85 | 0.10 | order |
| 16 | `action_train_skill` | Training | 0.15 | 70 | 0.20 | quiet_labor |
| 17 | `action_teach_skill` | Training | 0.15 | 70 | 0.25 | quiet_labor |
| 18 | `action_stand_watch` | Security | 0.35 | 85 | 0.15 | weapon |
| 19 | `action_conduct_research` | Research | 0.20 | 75 | 0.30 | quiet_labor |
| 20 | `action_rest` | Rest | 0.50 | 0 | 0.00 | — |

## Utilization Audit

### Dead Actions (never eligible)
**None.** All 20 actions have valid scoring that can produce positive scores under some conditions:
- Dead survivor → all score 0 (correct)
- Fatigue > gate → action scores 0 (correct)
- Trait veto → action scores 0 (correct)
- All other conditions → positive score

### Missing Executors
The action catalog defines scoring. The executor is host-owned. The following actions need executor mapping:
- All 14 new actions require host-level executor implementation
- The 6 existing actions already have host-level executor mapping

### Zero-Score Actions
No action has baseScore ≤ 0. All actions can produce positive scores when the survivor is alive and fatigue is below the gate.

### Unreachable Due to Precedence
Since all actions have different baseScore values, the priority hierarchy is:
1. `treat_wounded` (0.55) — wins at low/moderate fatigue
2. `rest` (0.50) — wins at high fatigue (no gate)
3. `canvas_support` / `purify_water` / `seek_treatment` (0.45) — compete in mid-range
4. `weigh_goods` / `cook_food` (0.40) — compete in mid-range
5. `repair_equipment` / `audit_inventory` / `file_report` / `resolve_conflict` / `stand_watch` (0.35) — compete in routine band
6. `run_vouch` / `preserve_food` (0.30) — lower routine
7. `inspect_housing` (0.25) — discretionary
8. `socialize` / `conduct_research` (0.20) — discretionary
9. `train_skill` / `teach_skill` (0.15) — long-horizon

With skill bonuses, the order can shift. For example, at max skill (1.0):
- `treat_wounded` = 0.85 (still #1)
- `conduct_research` = 0.50 (jumps to #3)
- `repair_equipment` = 0.60 (jumps to #2)

### Duplicate Action Roles
No duplicate roles. Each action has a distinct purpose:
- `action_rest` is unique (no other sleep/rest action)
- `action_treat_wounded` vs `action_seek_treatment` — different targets (other vs self)
- `action_train_skill` vs `action_teach_skill` — different targets (self vs other)
- `action_cook_food` vs `action_preserve_food` — different food operations

## Dead Reference Audit

**No references to external IDs in action data.** The action catalog does not reference:
- Room IDs
- Recipe IDs
- Skill IDs
- Research node IDs
- Item/resource IDs
- Security post IDs

All external references are executor-owned.

## Tag Inventory

| Tag | Used By | Consumer |
|-----|---------|----------|
| `loud_labor` | weigh_goods, repair_equipment, purify_water | Coward veto |
| `menial_labor` | canvas_support, preserve_food | GodComplex veto |
| `quiet_labor` | audit_inventory, file_report, inspect_housing, cook_food, train_skill, teach_skill, conduct_research | (informational) |
| `medical_triage` | treat_wounded | Hitman/Germaphobe veto |
| `medical` | seek_treatment | (informational) |
| `order` | resolve_conflict | ExCon veto |
| `weapon` | stand_watch | Pacifist veto |
| `dirty_labor` | — | Politician bias |
| `farming` | — | Hitman veto |
| `gun` | — | Blind veto |

Tags `dirty_labor`, `farming`, and `gun` are defined in `UtilityTags` but not used by any action in the 20-action catalog. They remain available for future expansion.