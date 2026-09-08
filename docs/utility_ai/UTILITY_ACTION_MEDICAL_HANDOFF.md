# Utility Action Medical Handoff

## Architecture

The Utility AI Core has **no medical state fields** in `AIActionContext`. There is no health, injury, or disease status.

Medical actions are scored purely on static priority:
- `action_treat_wounded`: baseScore 0.55 (high survival priority)
- `action_seek_treatment`: baseScore 0.45 (moderate priority)

## Medical Action Contract

1. **Action selection**: Utility AI selects `action_treat_wounded` or `action_seek_treatment` based on score.
2. **Executor checks eligibility**: The executor checks if:
   - There is actually an injured/sick survivor to treat
   - Medical supplies are available
   - The survivor has appropriate medical skill
3. **No injury → ineligible**: If no one needs treatment, the executor makes the action ineligible.
4. **Treatment authority**: The medical system (`Assets/Ashfall.Core/Medical/`) owns diagnosis, treatment selection, and outcome.

## Medical Safety Rules

1. **No autonomous medicine selection**: The executor must use the medical system's treatment recommendation, not pick arbitrary drugs.
2. **Triage ordering**: Treat the most severely injured first.
3. **No duplicate treatment**: Two survivors should not treat the same patient simultaneously.
4. **Resource validation**: Verify medical supplies before starting treatment.

## Trait Vetoes

| Action | Trait | Condition |
|--------|-------|-----------|
| `action_treat_wounded` | Hitman | Hard veto (tag: `medical_triage`) |
| `action_treat_wounded` | Germaphobe | Hard veto without hazmat (tag: `medical_triage`) |

## Plan 72 Position

Two medical actions are added to the catalog. Treatment authority remains with the medical system. The action data provides scoring priority; the executor validates eligibility and resource availability.