# Collectible Scavenging Placement — Balance Audit (offline, deterministic)

- Tables: `scavenging_tables.json` (Plan 46 authority) after collectible flagship placement.
- Method: 10,000 weighted rolls per changed table at fixed seed 20260903 (xorshift-equivalent Mersenne Twister draw order documented in script), offline only — no runtime Monte Carlo.
- Selection semantics mirror `ScavengingTableCatalog.RollLoot`: positive-weight entries, pre-roll unique-claimed filtering.

| Table | Bound (live) | Entries | Collectible weight share | Sampled collectible drop rate | Collectible items surfaced |
|---|---|---:|---:|---:|---|
| `table_loot_hospital` | yes | 24 | 1.1% | 1.16% | 1 |
| `table_loot_school` | yes | 18 | 5.5% | 5.46% | 3 |
| `table_loot_military_depot` | no (primary-only) | 22 | 5.1% | 5.07% | 4 |
| `table_loot_apartment_block` | yes | 24 | 8.7% | 8.77% | 5 |
| `table_loot_fire_station` | no (primary-only) | 12 | 5.0% | 4.97% | 2 |
| `table_loot_metro_station` | yes | 16 | 3.1% | 3.02% | 2 |
| `table_loot_police_station` | no (primary-only) | 16 | 3.3% | 3.31% | 2 |
| `table_loot_industrial_district` | yes | 16 | 1.2% | 1.21% | 1 |
| `table_loot_monastery` | no (primary-only) | 8 | 2.7% | 2.95% | 1 |
| `table_loot_clinic` | no (primary-only) | 18 | 1.8% | 1.71% | 1 |
| `table_loot_collapsed_structure` | yes | 11 | 0.6% | 0.61% | 1 |
| `table_loot_checkpoint` | yes | 13 | 7.2% | 7.78% | 3 |
| `table_loot_conscription_office` | yes | 9 | 3.4% | 3.21% | 1 |
| `table_loot_ordnance_shoulder` | yes | 13 | 7.0% | 7.11% | 4 |
| `table_loot_pilgrim_hearth` | yes | 7 | 3.2% | 3.32% | 1 |

- Determinism check: re-running table `table_loot_hospital` at the same seed reproduced identical hit count (116 == 116).
- Suppression check: with all 3 uniques claimed, `table_loot_hospital` never surfaced a claimed unique in 10,000 rolls, and ordinary loot remained reachable.

Verdict: no changed table gives collectibles a dominating share (max 8.7% weight); ordinary survival loot remains the overwhelming majority outcome on every table.
