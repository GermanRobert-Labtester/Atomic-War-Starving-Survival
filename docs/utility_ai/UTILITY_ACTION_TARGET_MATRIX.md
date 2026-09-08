# Utility Action Target Matrix

## Target Selection Architecture

The Core Utility AI (`UtilityAiSystem.SelectAction`) selects an **action ID**, not a target. Target selection is the responsibility of the action executor in the host layer.

The `AIActionContext` carries:
- `SurvivorId` — the actor making the decision
- No target field

## Target-Dependent Actions

| Action ID | Target Type | Target Selection | Concurrency |
|-----------|------------|------------------|-------------|
| `action_treat_wounded` | Injured survivor | Executor selects most urgent untreated patient | Must avoid duplicate treatment |
| `action_socialize` | Compatible survivor | Executor selects available partner | Avoid same-pair loops |
| `action_resolve_conflict` | Conflicting pair | Executor selects active conflict | One mediator per conflict |
| `action_teach_skill` | Learner survivor | Executor selects valid learner | Avoid duplicate teaching |
| `action_repair_equipment` | Degraded equipment | Executor selects most degraded item | Avoid duplicate repair |

## Non-Target Actions

These actions operate on the survivor or shelter state without a specific target:

| Action ID | Implicit Target |
|-----------|----------------|
| `action_rest` | Self |
| `action_seek_treatment` | Self |
| `action_train_skill` | Self |
| `action_cook_food` | Kitchen/workstation |
| `action_preserve_food` | Preservation station |
| `action_purify_water` | Water treatment |
| `action_stand_watch` | Watch post |
| `action_conduct_research` | Research lab |
| `action_inspect_housing` | Shelter |
| `action_weigh_goods` | Depot |
| `action_read_contract` | Self |
| `action_canvas_support` | Shelter halls |
| `action_run_vouch` | Gate |
| `action_audit_inventory` | Storage |
| `action_file_report` | Self |

## Deterministic Target Selection

When an executor selects a target, it must be deterministic:
- Same survivor state + same shelter state → same target
- Use `ISeededRng` for any randomization
- Prefer urgency-based ordering (e.g., most injured first)
- Document the selection algorithm

## Concurrency Rules

1. **Medical**: One medic per patient. If a patient is already being treated, exclude them from target selection.
2. **Social**: Don't pair the same two survivors repeatedly. Track recent social interactions.
3. **Teaching**: One teacher per learner. Don't assign the same teacher to multiple learners simultaneously.
4. **Repair**: One repairer per equipment item. Claim the item before starting work.