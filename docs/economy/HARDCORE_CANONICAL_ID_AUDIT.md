# Hardcore Economy Canonical ID Audit

## Faction IDs

The eight tuning IDs are unique and match the Plan 99 economy contract. The
set intentionally retains the two live baseline identities (`central_garrison_remnants`
and `faction_black_flotilla`) while adding six Standing Record identities. It
does not silently substitute the separate `faction_the_fleet` or
`faction_the_garrison` records for those established baseline keys.

| Tuning ID | Authority evidence | Reconciliation |
|---|---|---|
| `central_garrison_remnants` | Legacy `trade_texts.json` preference key | Preserved; aliases to `faction_central_garrison` in Core lookup |
| `faction_black_flotilla` | `holdfast_factions.json`, maritime catalogs | Preserved live baseline |
| `faction_the_scale` | `crossing_factions.json` | Direct |
| `faction_the_compact` | `crossing_factions.json` | Direct |
| `faction_the_underwrite` | `crossing_factions.json` | Direct |
| `faction_the_cutters` | `crossing_factions.json`, Holdfast records | Direct |
| `faction_the_rebuilders` | `standing_record_factions.json` | Direct |
| `faction_the_overlay` | `standing_record_factions.json` | Direct |

## Item IDs and patterns

The expanded tiers and shocks use existing item definitions or supported
patterns. `ammo_*` is a trailing-prefix wildcard, not a new item ID.
Expansion-specific goods such as `item_seed_ash_grain`, `item_hot_dust_drum`,
and `item_foundry_roof_armor_plate` resolve through their existing item
catalogs. The two baseline faction profiles retain their historical trade
tokens unchanged for compatibility with Plan 23 and prior trade text content.

The catalog integrity gate passed with zero errors after the expansion.
