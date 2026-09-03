# Combat Balance Audit (Plan 54 §51–58, analytic + deterministic runtime checks)

## Method

Nominal values below are computed from the catalog (damage × ammo.damage_mod
× burst; jam chance from `jam_base`; condition life ≈ `1 / (degrade_per_shot ×
burst)` pulls to ruin). Runtime behavior (hit/jam/degrade/damage/AI choice)
is verified deterministic and content-driven by
`Plan54CombatCatalogTests` (fixed-seed encounter replays with Plan 54
weapons and combatants).

## Weapon simulation matrix (nominal, unarmored, no stance mods)

| Weapon | DpP (dmg×ammo×burst) | Est. hits to drop 100 HP | Pulls to ruin condition | Jam profile |
|---|---|---|---|---|
| rebar_spear | 26.4 | 4 | ~46 | .018 + jury |
| coach_shotgun | **36.4** | 3 | ~19 | .038 |
| trail_carbine | 21.9 | 5 | ~38 | .030 |
| battle_rifle | **37.4** (2-rd pair) | 3 | ~36 | .026 |
| quiet_carbine | 17.0 (2-rd pair) | 6 | ~42 | .030 |
| revolver | 15.0 | 7 | ~83 | **.015 (best)** |
| marksman_rifle (ref) | 26.4 | 4 | ~83 | .018 |
| lmg (ref) | 60.5 (5-rd burst) | 2 | ~19 | .040 |

Key balances:

- **coach_shotgun** out-damages everything per pull at 0.70 range but is
  fed by craft-gated `12g_buck` handloads and .46 accuracy — ambush weapon,
  not a general upgrade (§52 satisfied: range 0.70 is unique).
- **battle_rifle** matches `lmg` damage-per-pull with half the ammo burn and
  better accuracy, but burst 2 vs 5 gives the LMG its suppression identity;
  battle rifle pays with repair cost 6 (highest of the five additions).
- **quiet_carbine** is deliberately the weakest pull (17.0) — subsonic
  economy: spends scarce handloads for the precision/suppression niche the
  SMG fills with volume (SMG DpP 31.4 but .62 accuracy at burst 3).
- **revolver** is the reliability floor: .015 jam, .008 degrade, repair 2 —
  a gun that always works but never shines.

## Enemy threat bands (vs 100-HP player, no armor)

Damage per enemy hit = (6 or 10 lane-matched) × dmg mod; accuracy =
0.50 × acc mod (× stance defense).

| Band | Combatant | Threat math |
|---|---|---|
| low | desperate_scavenger | 5.1 dmg/hit @ ~40% → nuisance; flees by .55/.75 |
| low | salvage_veteran | 5.7 dmg/hit @ ~50%, holds position — dangerous to ignore |
| medium | conscript_levy / flotilla_marine | 5.7–9.5 dmg/hit, resolve paths open |
| high | warlord_veteran | 10.5 dmg/hit @ ~52%, armor .45 |
| high | **hydro_pump_warden** | 9.5–10 dmg/hit @ **55%**, cover .60, never resolves |
| high fauna | armored_boar | 13.5 dmg/hit, 140 HP, never flees |

- No low-tier enemy is accidentally lethal (§63): worst low-band case is
  ~5 HP/hit at coin-flip accuracy.
- High-tier enemies are beatable but costly (§64): the warden's 0.60 cover +
  never-resolve semantics make siege arithmetic visible before the first shot.

## Combat economy (§54)

Victory loot is runtime-fixed (`scrap_metal ×3 + ammo_556 ×6`) — combat
**cannot** out-earn scavenging/trade, so the "fighting as farming" risk is
structurally capped. Plan 54 additions do not change the loot grant; new
weapon items carry `tradeValue` 25–60 (mid-tier, below marksman_rifle) so
trade value tracks scarcity without creating a dominant sale loop.

## Ammunition scarcity (§55)

All five additions reuse existing calibers; three orphaned calibers gained
consumers. No new caliber was introduced — the economy fragments less than
at baseline.

## Repair economy (§56)

Repair costs of the additions: 2 (revolver), 3 (coach, trail), 5–6 (military
pair). No addition exceeds `marksman_rifle`/`lmg` (5/6). Cheapest addition
(revolver, 2 scrap) is intentionally the most reliable — a working gun poor
survivors can actually maintain.

## Mutant fauna canon gate (§58)

No new fauna was added in Plan 54; existing mutants (size, disease, altered
tissue) remain within the grounded-mutation policy. No energy/magic attacks
exist in the data.
