# Plan 45 — Bounty/Raid Handoff

## Current State
Patrol encounters produce standing changes through `faction_standing_delta`. Severe standing loss (from combat choices) can push factions toward hostile status (≤-50), which affects future encounter eligibility and faction behavior.

## Bounty Integration
When the existing bounty system is active:
- Hostile standing with a faction increases likelihood of hostile patrol encounters
- Bounty state can be read by patrol eligibility (future extension)
- Patrol combat does not directly create bounty (standing loss is the mechanism)

## Raid Integration
Raid party patrols (`warlord_raid`, `scavenger_raid`) represent outbound faction activity, not enforcement raids against the player's shelter. The existing raid system owns shelter raids independently.

## Future Extension
To connect patrol violations to bounty:
1. Read bounty state during encounter selection
2. Increase hostile patrol weight when player has active bounty
3. Add bounty-specific choices (surrender, negotiate bounty)
