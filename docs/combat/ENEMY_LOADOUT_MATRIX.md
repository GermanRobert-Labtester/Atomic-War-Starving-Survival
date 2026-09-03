# Enemy Loadout & Faction Equipment Matrix (Plan 54 §14, §42)

## The contract that already holds (constraint 1.14)

**Enemies do not carry weapon stats.** `CombatantDefinition` has no weapon
field; the runtime resolves enemy damage as
`(6 + lane-match bonus 4) × AiDamageMod` and accuracy as
`0.50 × AiAccuracyMod × (1 − stance defense)`. There is no second,
contradicting weapon-damage table to keep in sync — the failure mode §14
warns about is structurally impossible in this schema. No per-enemy
loadout field was invented.

## What "loadout" means per faction (lore-derived, from `faction_lore.json`)

| Faction | Doctrine (from lore) | Combatant expression |
|---|---|---|
| iron_garrison | Military continuity, martial law, rationed everything | `conscript_levy` — trembling conscript, HoldPosition, high surrender (.45) |
| warlords_sector_4 | Checkpoint warlords, tolls, brutal chokepoint control | `warlord_veteran` — heavy salvage armor (.45), SuppressiveFire, low surrender (.20) |
| faction_scavengers | Black Flotilla boarding discipline, hold like an anchor | `flotilla_marine` — balanced 1.00/1.00, SuppressiveFire, no retreat culture (.30/.40) |
| faction_unaligned | Unorganised survivors; weakness and defence in one | `desperate_scavenger` (breaks first), **`salvage_veteran` (holds when the arithmetic is settled)** |
| faction_hydro_barons | Water infrastructure as power; wardens on fixed works | **`hydro_pump_warden`** — best cover (.60), never resolves, 1.10 aim |

Weapon-tier association remains implicit by design: the faction equipment
"table" is the doctrine-doctored AI trait set, not a JSON weapon list. A
future faction-equipment pass (§79.7) can select from the now-stable
20-weapon catalog without schema change.

## Human resolve paths (non-combat resolution)

The four baseline humans all keep at least one threshold open (pinned by
`Plan10CatalogCoverageTests.CombatCatalog_HumanArchetypes_HaveNonNegativeResolveThresholds`).

| Combatant | surrender | flee | Reads as |
|---|---|---|---|
| desperate_scavenger | .55 | .75 | breaks earliest |
| **salvage_veteran** | .35 | .55 | yields late, only when beaten |
| conscript_levy | .45 | .65 | morale of a conscript |
| warlord_veteran | .20 | .35 | fanatical chokepoint holder |
| flotilla_marine | .30 | .40 | boarding-party discipline |
| **hydro_pump_warden** | −1 | −1 | standing orders: the water holds |

The warden is the only human with both paths closed — intentional site-defense
semantics, same grammar the `armored_boar` uses for fauna.
