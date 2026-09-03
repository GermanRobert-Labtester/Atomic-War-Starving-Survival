# Patrol / Raid / Wildlife / Excavation Bindings (Plan 54 §45–48)

## Status update (Plan 45 — patrol bindings are LIVE)

The expedition ambush hand-off (`src/Main.Expeditions.cs` →
`CombatHostSession.StartCombat`) now passes `enemyCombatantIds` resolved by
`EnemyCompositionSelector` (`Assets/Ashfall.Core/Combat/`): the location's
danger band (≤2 / 3–5 / ≥6, grounded in the locations.json danger
distribution) picks the band pool from the matrix below, the band's anchor
archetype leads, and catalog `base_health` is honored in the fight. The
legacy count/health template remains the fallback for callers that do not
pass ids and for unknown ids (`enemy_catalog_missing`). Pinned by
`Plan45EnemyCompositionTests`.

## Original status at Plan 54 time

- `TacticalCombatSystem.BeginEncounter` accepts `enemyCombatantIds` (the
  sanctioned data → combat bridge), but **no src/ caller currently passes
  enemy ids** — encounter setup (expedition ambushes, `CombatHostSession
  .StartCombat`) still uses the count/health template with the legacy enemy
  block. Per §64, live patrol/raid integration is therefore **prepared, not
  forced**: the binding matrix below is the contract Plan 45/raid work
  consumes; no encounter data was rewritten (pure-data plan).
- Wildlife (Plan 35 / trapping runtime) and excavation (Plan 37) do not
  reference combatant ids in data. The fauna/mutant combatants remain the
  combat-side projection of those ecological systems; `field_guide.json`
  already cross-references 6 combatant ids as the bestiary display layer.

## Binding matrix — patrol encounters (six roles)

| # | Patrol role | Combatant id | Why |
|---|---|---|---|
| 1 | Faction soldier | `combatant_conscript_levy` | iron_garrison discipline |
| 2 | Faction scout/militia | `combatant_flotilla_marine` | mobile, suppression-capable |
| 3 | Raider / warlord checkpoint | `combatant_warlord_veteran` | toll-doctrine heavy |
| 4 | Scavenger threat | `combatant_desperate_scavenger` | low threat, flees |
| 5 | Veteran scavenger threat | `combatant_salvage_veteran` | holds ground (Plan 54) |
| 6 | Border/security site role | `combatant_hydro_pump_warden` | static defense (Plan 54) |

## Binding matrix — raid encounters (four roles)

| # | Raid role | Combatant id |
|---|---|---|
| 1 | Organized raider pressure | `combatant_warlord_veteran` |
| 2 | Heavy/enforcer | `combatant_hydro_pump_warden` (armored, never resolves) |
| 3 | Scavenger attacker | `combatant_salvage_veteran` |
| 4 | Desperate tag-along | `combatant_desperate_scavenger` |

## Binding matrix — wildlife / contaminated fauna

| Combatant | Ecological twin | Note |
|---|---|---|
| feral_mutt / spore_hound | dog/wolf pack strata | pack size controlled by encounter count, not data |
| armored_boar | boar / contaminated fauna | `Charge` special move |
| burrower_mite / pale_crawler / chrome_loper | subterranean/contaminated strata | `Burrow` / `Flank` moves |

No `enemy_wolf` vs `wildlife_wolf` duplication was created: animals exist
only as `combatant_*` rows; bestiary prose lives in `field_guide.json`.

## Turret / excavation (Plan 37)

A literal automated turret is **not** representable (`kind` vocabulary is
`human|mutant|fauna`; no immobile/machine AI). The sanctioned equivalent is
`combatant_hydro_pump_warden` — fixed position (HoldPosition + never
resolve), highest cover, high accuracy — usable as site-defense for
excavation content without new runtime.
