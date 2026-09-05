# Faction Equipment Matrix (Plan 54 §3.7 / §42)

Faction identity is expressed through combatant AI traits and threat
composition — verified against `faction_lore.json` (the loader's cross-ref
authority) rather than invented loadout tables.

| Faction (canonical id) | Lore stratum | Equipment tier implied | Combatant(s) | Behavior signature |
|---|---|---|---|---|
| `iron_garrison` | ex-military command remnant, rationed conscripts | mixed salvage + service rifles | conscript_levy | defensive (HoldPosition), breaks under pressure |
| `warlords_sector_4` | checkpoint warlords, toll doctrine | heavy tactical salvage, suppression platforms | warlord_veteran | SuppressiveFire specialist, near-fanatical |
| `faction_scavengers` | Black Flotilla boarding culture | marine pattern, balanced kit | flotilla_marine | anchored defense, low surrender |
| `faction_unaligned` | unorganized survivor mass | whatever they found | desperate_scavenger, salvage_veteran | polarized: flight vs veteran caution |
| `faction_hydro_barons` | water-infrastructure barons | site-defense kit, dug-in emplacements | hydro_pump_warden | immovable site garrison |

## Rules respected

- No invented factions ("garrison"/"rebel" placeholders were not used) —
  every `faction_id` resolves in `faction_lore.json`, enforced by the loader.
- No two factions share the same AI signature (checked across the 12-row
  matrix: stance/move/thresholds combinations are distinct per faction).
- Doctrine depth (8 warlord doctrines, `WarlordDoctrineCatalog`) is untouched
  Plan 10 territory; Plan 54 only feeds it stable combatant ids to consume.
