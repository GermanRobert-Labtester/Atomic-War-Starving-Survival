# Utility Action Save Contract

## Current Architecture

The Core Utility AI is **stateless**. `UtilityAiSystem` has no state to persist:
- No current action tracking
- No action progress
- No cooldown/commitment state
- No reservations
- No queued return-to-duty state

The `UtilityAiHostSession` also has no save state:
- No `CaptureState()` / `RestoreState()` methods
- Not registered in `SaveStoreHub`
- Not included in the campaign envelope

## Save Behavior

When a save is loaded:
1. The host re-loads the action catalog from `utility_actions.json`
2. The catalog is identical to the saved state (same data file)
3. Any survivor's current action is an **executor concept**, not a Utility AI concept
4. The host reevaluates actions for survivors based on current state

## Plan 72 Impact on Saves

**Zero save-schema change.** The catalog expansion from 6 to 20 actions:
- Does not change the save format
- Does not require migration
- Old saves load with the new catalog
- New actions become eligible naturally
- The first 6 actions remain byte-identical

## Save Compatibility Testing

| Scenario | Expected Behavior |
|----------|------------------|
| Old save with 6-action catalog | Loads with 20 actions; new actions become eligible |
| Old save with survivor in `action_weigh_goods` | Action ID preserved; catalog still contains it |
| Old save with idle survivor | Survivor reevaluates with 20 options |
| New save with survivor in `action_repair_equipment` | Action ID round-trips correctly |
| Save/reload during action | Host-owned action state (not in Core) must handle this |

## Recommendation

If the host later persists current action state (survivor → action ID mapping), store the `action_*` ID string. The catalog expansion is backward-compatible because:
1. Original 6 IDs are preserved
2. New IDs are unique and non-conflicting
3. The catalog loader handles unknown IDs gracefully (they're just strings)