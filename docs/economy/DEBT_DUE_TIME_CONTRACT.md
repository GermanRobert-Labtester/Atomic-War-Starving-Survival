# Plan 40 — Due-Time Contract

## Semantics
- `termDays`: duration in campaign days after signing
- `signedDay`: campaign day when `SignContract()` was called
- `daysRemaining`: decremented by `TickDaily(day)` each campaign day
- Default: `daysRemaining <= 0` → `forfeited = true`

## Template Terms

| Template | Term | Category |
|---|---|---|
| scavengers_medicine | 10d | Short — emergency |
| scavengers_food | 12d | Short — emergency |
| ordnance_foundry_ammo | 14d | Short — military urgency |
| supply_corps_medical | 15d | Short — medical urgency |
| hydro_barons_water | 18d | Short-moderate |
| scavengers_equipment | 20d | Short-moderate |
| supply_corps_rations | 20d | Short-moderate |
| hydro_barons_purification | 22d | Moderate |
| supply_corps_fuel | 25d | Moderate |
| ordnance_foundry_tools | 25d | Moderate |
| railway_guild_fuel | 28d | Moderate |
| ordnance_foundry_armor | 30d | Moderate-long |
| hydro_barons_filter | 30d | Moderate-long |
| railway_guild_parts | 35d | Long |
| railway_guild_transport | 45d | Long — capital equipment |

## Boundary Behavior
- Default fires at `daysRemaining <= 0` (exact boundary, not after grace period)
- No pause behavior — term runs continuously
- `PayContract()` allowed even after forfeit ("the honoured path")
