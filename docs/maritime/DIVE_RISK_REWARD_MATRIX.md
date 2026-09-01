# Dive Risk/Reward Matrix (Plan 23 / Task 23B + 23E)

Representative deterministic-run math for the dive economy (values in rough campaign
units; exact simulation lives in `Plan23DiveMechanicCoverageTests` + Task 23E sweeps).

## Cost model (existing authorities only)

| Cost | Source |
|---|---|
| Travel | deep-coast route hours (10.5–15.5 h) + fuel via expedition economics |
| Gear | real items (cutting tool ~16, rebreather canister ~22, lamp ~34, line ~22 trade value) with condition/wear where equipped |
| Air | site oxygen budget (70–120 ticks); compressor cranks extend within max |
| Noise | room-advance noise adds; compromise at 80, loss at 100+ in deep rooms |
| Decompression | 20 s at hold-approach, 40 s at deep hold; emergency ascent = sickness + dose |
| Radiation | hazard-scaled dose during dive; abort adds +25 emergency |
| Contamination | site-keyed psychological effects on contaminated outcomes (2–5 day work refusals) |
| Safe cost | attempts = noise + tool damage; jam at max attempts |
| Depletion | visit-count decay + day-phase degradation on repeatable salvage |

## Per-site expected value (deterministic seeds, representative)

| Site | Travel+prep | Risk profile | Expected recovery | Rare/unique | Repeatable |
|---|---|---|---|---|---|
| ferry_terminal | low | noise 0.60, hazard 2–5 | low-mid (cloth, canned) | — | scavenge, decaying |
| barge_flotilla | low | noise 0.40 | Flotilla cargo | claim tags (trade) | scavenge |
| flooded_metro | low-mid | noise 0.45 | civilian/utility | — | scavenge |
| submerged_convoy | mid | 0.55 | mixed cargo | — | scavenge |
| submerged_siphon | low-mid | 0.40 | pump parts, water | — | scavenge |
| fuel_depot | mid + cutting tool | 0.65 | bounded fuel ×2 | — | scavenge (capped by decay) |
| offshore_relay | mid-high | 0.70 | electronics, wire | cipher/relay notes | scavenge |
| field_hospital (barge) | mid | 0.50 + contamination | medical | — | scavenge |
| flooded_metro | low | 0.45 | food, paper | — | scavenge |
| picket_craft | mid | 0.65 | bell Relic (rare), scrap | memorial | scavenge |
| sunken_submarine | deep + lamp | noise 0.80, air 70 | technical/military | log hook | high abort cost |
| drowned_fuel_depot | deep + cutting tool | 0.65 | fuel (bounded), barrels | — | depletion applies |
| payroll_strongroom | deep + cutting tool | 0.75 | safes: claim tags, ledgers, ammo, med | one-time safes | scavenge + safes |
| brine_cistern | deep + canister | 0.50 | iodine, resin, bleach | abort-before-max decision | scavenge |

## Boundedness rules verified by tests/sweeps

- Deep sites are the best single recoveries but require travel (10.5–15.5 h), gear
  condition, air budget, noise discipline, and contamination exposure — never a
  passive faucet.
- Repeatable salvage decays per visit and world phase (`ProceduralScavengeSystem`
  phases at days 20/50/80), so no site is an infinite faucet.
- Safe loot is one-time and persisted; reroll-after-load is test-blocked.
- Rare Relic (bell) and quest (log cylinder) nodes are single-source and non-duplicable.
- Full inland↔Flotilla loop audited in Task 23E (no buy/sell discount loop: Flotilla
  buys salvage at premium and refuses luxuries; no Flotilla seller table exists).
