# Plan 45 — Caravan Handoff

## Integration
Caravan escort and supply run patrols are reachable through the travel encounter system when the player is traveling along caravan routes.

## Eligible Patrols
- `enc_patrol_railway_convoy` — Railway Guild escort
- `enc_patrol_hydro_escort` — Hydro Baron water convoy
- `enc_patrol_foundry_supply` — Ordnance Foundry supply
- `enc_patrol_supply_corps_convoy` — Supply Corps relief

## Future Integration
To wire patrols into the caravan event system specifically:
1. Add caravan-route region tags to patrol entries
2. Use caravan encounter hooks if they support generic encounter injection
3. Keep patrols as encounter content, not caravan simulation
