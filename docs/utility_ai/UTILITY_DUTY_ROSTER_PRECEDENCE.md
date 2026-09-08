# Utility Duty Roster Precedence

## Current Architecture

The Duty Roster system (`Assets/Ashfall.Core/DutyRoster/`) and the Utility AI system (`Assets/Ashfall.Core/UtilityAI/`) are **independent systems** with no direct integration.

The `AIActionContext` has no field for:
- Current duty assignment
- Duty roster status
- Explicit player orders

## Precedence Design

Since the Utility AI is stateless and host-driven, the precedence between explicit assignments and autonomous behavior is **host-owned**:

1. The host checks if the survivor has an active duty/player assignment
2. If assigned: the host either skips Utility AI evaluation or uses it only to decide what to do during idle gaps
3. If unassigned/idle: the host runs full Utility AI selection

## Plan 72 Position

The Utility AI catalog expansion does **not** change the duty roster or player order behavior. The action data is scoring-only; the host decides when to run the scorer and what to do with the result.

## Recommended Host Behavior

```text
for each survivor:
  if survivor has active player order:
    execute player order
  else if survivor has active duty roster assignment:
    execute duty assignment
  else if survivor is idle:
    run Utility AI selection
    execute selected action
  else:
    continue current action
```