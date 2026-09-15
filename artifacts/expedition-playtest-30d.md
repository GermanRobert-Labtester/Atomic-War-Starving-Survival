# ASHFALL expedition 30-day playtest

- Seed: `51051`
- Snapshot count: `30`
- Sorties launched/completed: `10/10`

## Balance bands

| Measure | Observed | Guard |
|---|---:|---|
| Estimate tick mismatches | `0` | 0 |
| Returned loot entries | `20` | equals completed loot entries |
| Breakdown events | `0` | bounded, single transition per event |
| Profiles exercised | `9` | at least 2 including foot |

## Persistence and determinism

- Same-seed byte equality: `True`
- Mid-sortie save/load equality: `True`
- Different-seed divergence: `True`

## Sorties

| Sortie | Day | Vehicle | Estimate ticks | Actual ticks | Fuel | Encounters | Breakdowns | Loot value | Result |
|---:|---:|---|---:|---:|---:|---:|---:|---:|---|
| 0 | 1 | foot | 11 | 11 | 0 | 3 | 0 | 40 | completed |
| 1 | 4 | vehicle_ambulance_rig | 9 | 9 | 6.75 | 0 | 0 | 101 | completed |
| 2 | 7 | vehicle_armored_mobile_base | 11 | 11 | 19 | 1 | 0 | 22 | completed |
| 3 | 10 | vehicle_cargo_truck | 7 | 7 | 5 | 1 | 0 | 6.6 | completed |
| 4 | 13 | vehicle_dirt_bike | 11 | 11 | 4 | 0 | 0 | 103 | completed |
| 5 | 16 | vehicle_salvage_dredger | 15 | 15 | 16.5 | 0 | 0 | 15 | completed |
| 6 | 19 | vehicle_scout_motorcycle | 7 | 7 | 1.8 | 0 | 0 | 69 | completed |
| 7 | 22 | vehicle_steam_halftrack | 15 | 15 | 21 | 4 | 0 | 54 | completed |
| 8 | 25 | vehicle_utility_quad | 11 | 11 | 6 | 2 | 0 | 16.2 | completed |
| 9 | 28 | foot | 7 | 7 | 0 | 1 | 0 | 11.4 | completed |

## Tuning decision

No vehicle data was changed by this proof pass. The current catalog is characterized; any rebalance must be a separate vehicles.json-only change justified by this fixed seed set.

## Checks

- PASS: 30-day expedition campaign emits one snapshot per day — count=30
- PASS: scripted cadence launches representative sorties — launched=10
- PASS: at least one sortie returns through the real completion path — completed=10
- PASS: foot and vehicle profiles are exercised — profiles=9
- PASS: estimate and actual completed tick counts agree — mismatches=0
- PASS: fuel and loot ledgers never go negative — negative=0
- PASS: returned loot is recorded exactly once — returned=20, completed=20
- PASS: zero-risk route produces no spurious encounters
- PASS: sortie ledger remains bounded and internally coherent
- PASS: same seed and scripted inputs produce byte-identical daily ledgers
- PASS: different fixed seed produces a divergent daily ledger
- PASS: mid-sortie save/load preserves the post-restore trajectory — save_day=15
- PASS: vehicle breakdown transitions once and resumes on foot
