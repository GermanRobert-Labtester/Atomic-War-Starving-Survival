# Enemy Combatant Matrix — 12 hostile definitions

Authority: `combat_catalog.json → combatants[]` (see
`COMBATANT_AUTHORITY_AUDIT.md`). Threat bands are analytic (§40); each
combatant's AI grammar is loader-validated.

| # | id | kind | faction | HP | armor | cover | lane | stance | special move | acc mod | dmg mod | surr / flee | threat band |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | combatant_burrower_mite | fauna | — | 70 | .10 | .05 | L | Advance | Burrow | .95 | .85 | never / never | low |
| 2 | combatant_spore_hound | fauna | — | 90 | .15 | .10 | R | Advance | Spore | .85 | 1.10 | never / .30 | low-med |
| 3 | combatant_armored_boar | fauna | — | 140 | .45 | .30 | C | HoldPosition | Charge | 1.05 | 1.35 | never / never | **high** |
| 4 | combatant_feral_mutt | fauna | — | 60 | .05 | .10 | L | Advance | Flank | .85 | .75 | never / .55 | low |
| 5 | combatant_pale_crawler | mutant | — | 80 | .20 | .15 | R | Advance | Flank | 1.10 | .95 | never / never | medium |
| 6 | combatant_chrome_loper | mutant | — | 110 | .30 | .05 | C | Advance | Charge | 1.00 | 1.15 | never / .25 | medium-high |
| 7 | combatant_conscript_levy | human | iron_garrison | 85 | .25 | .45 | C | HoldPosition | None | .85 | .95 | .45 / .65 | medium |
| 8 | combatant_warlord_veteran | human | warlords_sector_4 | 110 | .45 | .55 | C | HoldPosition | SuppressiveFire | 1.05 | 1.05 | .20 / .35 | **high** |
| 9 | combatant_flotilla_marine | human | faction_scavengers | 95 | .30 | .50 | C | HoldPosition | SuppressiveFire | 1.00 | 1.00 | .30 / .40 | medium-high |
| 10 | combatant_desperate_scavenger | human | faction_unaligned | 75 | .10 | .30 | R | Retreat | TacticalRetreat | .80 | .85 | .55 / .75 | low |
| 11 | **combatant_salvage_veteran** (Plan 54) | human | faction_unaligned | 90 | .20 | .40 | C | HoldPosition | None | 1.00 | .95 | .35 / .55 | medium |
| 12 | **combatant_hydro_pump_warden** (Plan 54) | human | faction_hydro_barons | 95 | .35 | **.60 (highest)** | C | HoldPosition | None | 1.10 | 1.00 | **never / never** | **high (site defense)** |

## Plan 54 role coverage (DoD #37–42)

| Required role | Covered by |
|---|---|
| 3 scavenger archetypes | desperate_scavenger, **salvage_veteran** (+ flotilla_marine straddles scavenger-faction soldiery) |
| 2 raider archetypes | warlord_veteran (Warlord doctrine stratum = raider command), chrome_loper reads as the non-human raider-adjacent hunter |
| 2 faction soldiers | conscript_levy (iron_garrison), flotilla_marine (faction_scavengers) — plus **hydro_pump_warden** as faction site-garrison |
| 2 feral animals | feral_mutt, spore_hound |
| 2 contaminated fauna | burrower_mite, pale_crawler (chrome_loper, armored_boar exceed minimum) |
| 1 turret / site defense | **hydro_pump_warden** — the DoD #42 *equivalent supported site-defense combatant*: pinned to `HoldPosition`, highest cover in the catalog, `surrender/flee = never` (a literal automated turret would have faked support: `kind` vocabulary is `human|mutant|fauna` and no immobile/machine AI exists — constraint 1.1) |

The baseline landed 10 of the 12; the plan's original role distribution was
authored against a 0-combatant baseline, so Plan 54's two slots were spent on
the two largest gaps: a second scavenger stratum and the wholly missing
site-defense role.

## Design intent of the two additions

- **salvage_veteran** — the anti-`desperate_scavenger`: holds ground instead
  of retreating, aims at par (1.00 vs 0.80), yields later (.35/.55 vs
  .55/.75). Teaches that not every unaligned scavenger breaks first (§29).
- **hydro_pump_warden** — discipline as identity: never resolves, never
  flees, 1.10 accuracy behind the best cover in the game. Dangerous to
  attack, pointless to siege without a plan — exactly the "is this patrol
  worth fighting at all" decision (§80). Faction-true: Hydro Barons defend
  fixed water infrastructure (pump caverns, plant perimeters).
