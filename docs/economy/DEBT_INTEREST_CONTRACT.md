# Plan 40 — Interest Model Contract

## Formula
`totalOwed = principal × (1 + rate)`

Flat rate, never compounded. Settled debt returns 0.

## Template Interest Burden

| Template | Principal (TV) | Rate | Interest (TV) | Due (TV) | Term |
|---|---|---|---|---|---|
| supply_corps_medical | 30 | 10% | 3.0 | 33.0 | 15d |
| ordnance_foundry_tools | 28 | 15% | 4.2 | 32.2 | 25d |
| railway_guild_parts | 45 | 15% | 6.8 | 51.7 | 35d |
| hydro_barons_purification | 36 | 20% | 7.2 | 43.2 | 22d |
| hydro_barons_filter | 40 | 30% | 12.0 | 52.0 | 30d |
| supply_corps_rations | 96 | 15% | 14.4 | 110.4 | 20d |
| scavengers_equipment | 60 | 25% | 15.0 | 75.0 | 20d |
| supply_corps_fuel | 84 | 20% | 16.8 | 100.8 | 25d |
| scavengers_medicine | 50 | 35% | 17.5 | 67.5 | 10d |
| railway_guild_fuel | 100 | 18% | 18.0 | 118.0 | 28d |
| scavengers_food | 75 | 30% | 22.5 | 97.5 | 12d |
| railway_guild_transport | 80 | 35% | 28.0 | 108.0 | 45d |
| ordnance_foundry_armor | 120 | 25% | 30.0 | 150.0 | 30d |
| hydro_barons_water | 180 | 25% | 45.0 | 225.0 | 18d |
| ordnance_foundry_ammo | 480 | 20% | 96.0 | 576.0 | 14d |

## Balance Notes
- Low-tier (3-7 TV interest): medical, tools, parts — mild survival credit
- Mid-tier (14-28 TV interest): rations, fuel, food, equipment — meaningful pressure
- High-tier (30-96 TV interest): water, armor, ammo — severe obligation
- No template exceeds 100 TV interest except ammo (96 TV for 480 TV principal)
- Rates range from 10% (Supply Corps medical) to 35% (Scavengers medicine, Railway engine)
