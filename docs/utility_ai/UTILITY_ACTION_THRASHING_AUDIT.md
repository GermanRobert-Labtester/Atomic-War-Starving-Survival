# Utility Action Thrashing Audit

## Thrashing Definition

Action thrashing occurs when a survivor rapidly switches between actions without completing meaningful work. This can happen when:
1. Multiple actions have near-equal scores
2. Small state changes cause score flipping
3. The reevaluation cadence is too frequent

## Current Architecture

The Core Utility AI is **stateless** — it has no:
- Action commitment duration
- Cooldown between evaluations
- Hysteresis (prefer current action)
- Minimum action duration

These are host-owned concepts. The `UtilityAiSystem` simply selects the highest-scoring action each time it's called.

## Thrashing Prevention (Host-Level)

The host should implement:
1. **Minimum action duration**: Once an action is selected, keep it for at least N ticks
2. **Commitment**: Don't reevaluate until the current action completes a unit of work
3. **Hysteresis**: Give the current action a small score bonus to prevent flipping
4. **Reevaluation cadence**: Only rescore after an action completes or a significant state change

## Plan 72 Data Design

The action data is designed to minimize thrashing risk:
1. **Clear priority gaps**: baseScore differences of 0.05-0.10 provide clear separation
2. **Consistent weights**: All weights are 1.0, so no action has hidden advantages
3. **Deterministic noise**: The seeded noise (0.0001 scale) is too small to flip meaningful score differences
4. **No cooldown data**: No action-level cooldown fields are added (cooldown is host-owned)

## Near-Equal Score Analysis

Actions with identical scores at the same fatigue/skill level:
- `action_audit_inventory` (0.35) vs `action_file_report` (0.35) vs `action_repair_equipment` (0.35) vs `action_resolve_conflict` (0.35) vs `action_stand_watch` (0.35)
  - All have baseScore 0.35 and basePriority 0.1 → score 0.45
  - Different skillBonusFactors create separation at skill > 0
  - Different fatigueGates create separation at different fatigue levels

## Failed Action Retry Prevention

If an action is selected but the executor finds it ineligible (e.g., no patient to treat, no ingredients to cook), the executor should:
1. Report the action as failed
2. The host should temporarily suppress that action from the candidate list
3. Or the host should mark the action as ineligible until conditions change

The Utility AI Core has no built-in retry prevention — it must be host-owned.

## Plan 72 Position

The action catalog is designed to minimize thrashing through clear priority separation. Thrashing prevention mechanisms (commitment, hysteresis, minimum duration) are host responsibilities and are not addressed in the action data.