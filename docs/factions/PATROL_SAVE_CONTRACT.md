# Plan 45 — Save Contract

## Persistence
Patrol encounters use the existing `TravelEncounterState` save envelope:
- `ChainStages` — chain progression
- `EncounterAvailableDay` — cooldown timestamps

## No New Save Section Required
All patrol state is captured by the existing travel encounter save contract.

## Round-Trip Behavior
- Active cooldowns survive save/reload
- Chain stages survive save/reload
- No duplicate effects on reload

## Old-Save Compatibility
- Pre-Plan-45 saves load safely
- New patrol encounters are available immediately
- No migration needed
