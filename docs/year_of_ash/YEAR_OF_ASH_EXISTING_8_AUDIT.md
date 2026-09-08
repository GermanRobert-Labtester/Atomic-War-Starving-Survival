# Existing Eight Questline Audit

The original definitions remain unchanged in the JSON authority:

| Questline | Faction | Window | Stages |
|---|---|---:|---:|
| `quest_garrison_blood_debt` | `faction_central_garrison` | 185–260 | 6 |
| `quest_ash_sign_revelation` | `faction_ash_sign` | 220–310 | 6 |
| `quest_rebuilder_seed_vault` | `faction_rebuilders` | 200–280 | 6 |
| `quest_hydro_baron_aqueduct` | `faction_hydro_barons` | 250–330 | 5 |
| `quest_black_ops_null_order` | `faction_black_ops` | 270–355 | 5 |
| `quest_survivor_mutiny` | *(blank in legacy data)* | 240–320 | 6 |
| `quest_the_last_broadcast` | *(blank in legacy data)* | 320–360 | 4 |
| `quest_winter_harvest` | *(blank in legacy data)* | 195–240 | 5 |

These IDs and their stage chains are the parity oracle. The built-in eight-definition catalog remains
a fallback/test fixture; it is not allowed to overwrite the expanded JSON authority.

The three blank legacy tags were preserved. Plan 114 does not silently backfill them because that
would alter existing authored data and faction-selection semantics.
