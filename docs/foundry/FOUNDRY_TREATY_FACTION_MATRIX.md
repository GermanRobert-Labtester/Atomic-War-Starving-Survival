# Foundry Treaty Faction Matrix

**Catalog Authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
**Accords Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`

---

## 1. Faction Representation Across Policies

Every consequence policy in `foundry_treaty_consequences.json` is anchored to a verified signatory faction of that treaty.

| Faction ID | Display Name | Policies Owned | Associated Treaties |
|---|---|---|---|
| `faction_silent_foundry` | The Silent Foundry | 6 | `treaty_brine_pipe_and_iodine_exchange`, `treaty_cluster_labour_schedule`, `treaty_road_iron_charter` |
| `faction_the_fleet` | The Fleet / Black Flotilla | 4 | `treaty_flotilla_saline_corridor_concordat`, `treaty_deep_coast_aquifer_protection_treaty` |
| `faction_ash_sign` | The Ash Sign | 2 | `treaty_switchback_fuel_and_passage_accord` |
| `faction_central_garrison` | The Central Garrison | 2 | `treaty_garrison_grain_tithe_compact` |
| `faction_the_scale` | The Scale | 1 | `treaty_scale_suburban_fair_trade_convention` |

---

## 2. Policy-to-Signatory Verification

| Treaty ID | Policy Faction ID | Is Official Signatory in `foundry_accords.json`? | Other Co-Signatories |
|---|---|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | `faction_silent_foundry` | **Yes** | `faction_the_office` |
| `treaty_cluster_labour_schedule` | `faction_silent_foundry` | **Yes** | `faction_the_office`, `faction_the_cutters` |
| `treaty_road_iron_charter` | `faction_silent_foundry` | **Yes** | `faction_the_cutters`, `faction_the_fleet` |
| `treaty_flotilla_saline_corridor_concordat` | `faction_the_fleet` | **Yes** | `faction_the_cutters` |
| `treaty_switchback_fuel_and_passage_accord` | `faction_ash_sign` | **Yes** | `faction_forward_roster` |
| `treaty_deep_coast_aquifer_protection_treaty` | `faction_the_fleet` | **Yes** | `faction_rebuilders` |
| `treaty_garrison_grain_tithe_compact` | `faction_central_garrison` | **Yes** | `faction_rebuilders` |
| `treaty_scale_suburban_fair_trade_convention` | `faction_the_scale` | **Yes** | `faction_rebuilders` |

All 15 policies map 100% to valid signatories. No orphaned or cross-faction leakage exists.
