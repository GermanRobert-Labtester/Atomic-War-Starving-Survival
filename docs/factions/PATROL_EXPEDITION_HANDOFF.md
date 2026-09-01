# Plan 45 — Expedition Handoff

## Integration Point
The `ExpeditionEncounterBridge` uses `NarrativeEncounterSystem`, not `TravelEncounterSystem`. Patrol encounters live in `travel_encounters.json` and are selected by `TravelEncounterSystem.SelectEncounter()`.

## Current State
Expedition encounters and travel encounters are separate systems. Patrol encounters are reachable through the travel encounter path.

## Future Integration
To wire patrols into expeditions:
1. Add patrol entries to `narrative_encounters.json` (or its expansion)
2. Use `EncounterDefinition.GetEffectiveWeight(stance, dangerLevel, locationId)` for territory-aware selection
3. The ExpeditionEncounterBridge will surface them automatically

## Current Reachability
Patrol encounters are reachable through:
- Travel encounter selection (region + danger + stance + season)
- Territory danger modifier (increases encounter chance in controlled/contested zones)
