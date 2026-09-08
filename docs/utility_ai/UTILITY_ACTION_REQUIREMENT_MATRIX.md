# Utility Action Requirement Matrix

## How Action Eligibility Works

The current Utility AI Core (`UtilityActionDef`, `UtilityActionScorer`) does **not** have a built-in requirement/eligibility system beyond:
1. `fatigueGate` — rawScore → 0 if fatigue exceeds gate
2. Trait vetoes — hard score → 0 for specific trait×tag pairs
3. `IsAlive` — dead survivors score 0 on all actions

There is no schema field for:
- Room requirements
- Workstation requirements
- Item/resource requirements
- Recipe availability
- Patient existence
- Conflict existence
- Skill trainability
- Research node availability
- Security need

## Plan 72 Design Decision

All eligibility checks beyond fatigue and traits are **executor-owned**. The Utility AI selects an action ID; the host/executor is responsible for:
1. Checking if the action is actually possible (room exists, resources available, etc.)
2. Making the action ineligible if prerequisites aren't met
3. Handling the failure gracefully (not re-selecting the same action endlessly)

This means the action data is purely about scoring priority, not about checking whether the action can actually be performed.

## Action Category × Eligibility Summary

| Action ID | Fatigue Gate | Trait Vetoes | Executor-Owned Checks |
|-----------|-------------|--------------|----------------------|
| `action_weigh_goods` | 85 | Coward → loud_labor | Depot availability |
| `action_read_contract` | 90 | — | Contract availability |
| `action_canvas_support` | 80 | GodComplex → menial_labor | Petition campaign active |
| `action_run_vouch` | 88 | — | Stranger at gate |
| `action_audit_inventory` | 80 | — | Inventory exists |
| `action_file_report` | 80 | — | Report system available |
| `action_repair_equipment` | 80 | Coward → loud_labor | Degraded equipment, workshop, materials |
| `action_inspect_housing` | 85 | — | Shelter structure |
| `action_treat_wounded` | 90 | Hitman → medical_triage; Germaphobe → medical_triage (w/o hazmat) | Injured survivor, medical supplies |
| `action_seek_treatment` | 95 | — | Own injury/illness, medical care available |
| `action_cook_food` | 80 | — | Kitchen, ingredients, recipe |
| `action_preserve_food` | 80 | GodComplex → menial_labor | Perishables, preservation station |
| `action_purify_water` | 80 | Coward → loud_labor | Unsafe water, treatment equipment |
| `action_socialize` | 85 | — | Compatible partner, safe context |
| `action_resolve_conflict` | 85 | ExCon → order | Active conflict, mediator role |
| `action_train_skill` | 70 | — | Trainable skill, energy |
| `action_teach_skill` | 70 | — | Qualified teacher, willing learner |
| `action_stand_watch` | 85 | Pacifist → weapon | Watch post, security need |
| `action_conduct_research` | 75 | — | Lab, active research node |
| `action_rest` | 0 | — | Always available |