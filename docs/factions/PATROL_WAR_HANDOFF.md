# Plan 45 — War Handoff

## Integration
Patrol encounters can contribute to faction war state through standing changes. Severe standing loss (combat, repeated violations) pushes factions toward hostile, which affects war-state evaluation.

## Bounded Escalation
- Single patrol encounter: max -20 standing (Black Ops fight)
- Cannot directly trigger war declaration
- Cannot directly flip territory control
- War pressure accumulates through standing over time

## Territory Influence
Patrol encounters do not directly modify `WarlordTerritoryState`. Territory changes happen through the `WarlordDoctrineSystem` authority, not through encounter data.
