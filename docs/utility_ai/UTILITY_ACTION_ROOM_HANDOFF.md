# Utility Action Room Handoff

## Architecture

The Utility AI Core has **no room/workstation schema field**. Room prerequisites are executor-owned.

The action ID serves as a contract: the executor maps the action ID to a room/workstation check.

## Action → Room Mapping

| Action ID | Conceptual Room | Room Check | Plan 41 ID |
|-----------|----------------|------------|------------|
| `action_cook_food` | Kitchen | Kitchen exists + has workstation | `room_kitchen_shelter` |
| `action_repair_equipment` | Workshop | Workshop exists + has workstation | `room_workshop_shelter` |
| `action_conduct_research` | Laboratory | Lab exists + has workstation | `room_laboratory_shelter` |
| `action_stand_watch` | Security post | Security post exists | `room_security_post_shelter` |
| `action_purify_water` | Water treatment | Water treatment exists + has equipment | `room_water_treatment_shelter` |

## Room Handoff Contract

1. **Executor checks room**: Before executing the action, the executor verifies the room exists and is operational.
2. **Unavailable room → ineligible**: If the room doesn't exist, is destroyed, or is unpowered, the executor reports the action as ineligible.
3. **Occupied workstation → wait or skip**: If the workstation is occupied, the executor either queues the action or makes it ineligible.
4. **No room IDs in action data**: The room reference lives in the executor, not in `utility_actions.json`.

## Plan 72 Position

All five room-linked actions are added to the catalog with appropriate scoring. The room checks are deferred to the executor implementation. The action data does not contain room references.