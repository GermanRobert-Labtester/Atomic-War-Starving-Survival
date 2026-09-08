# Utility Action Research Handoff

## Architecture

The Utility AI Core has **no research state fields** in `AIActionContext`. There is no research node progress, lab availability, or research skill.

Research is scored purely on static priority:
- `action_conduct_research`: baseScore 0.20 (long-horizon, low priority)

## Research Action Contract

1. **Action selection**: Utility AI selects `action_conduct_research` when it's the highest-scoring eligible action.
2. **Executor checks eligibility**: The executor checks if:
   - An active research node exists
   - The laboratory is available
   - The survivor has appropriate research skill
3. **No active research → ineligible**: If no research project is active, the action is ineligible.
4. **Research authority**: The research system owns node progress, unlocks, and resource requirements.

## Research Safety Rules

1. **No progress during crisis**: The executor should suppress research when survival needs are urgent (this is handled by the scoring priority — research has low baseScore, so survival actions win).
2. **Don't progress without a node**: The executor must verify an active research node exists.
3. **Resource validation**: Verify research materials before starting.

## Plan 72 Position

One research action is added to the catalog. Research authority remains with the research system. The low baseScore (0.20) ensures research only wins during safe idle periods when no survival or maintenance actions are needed.