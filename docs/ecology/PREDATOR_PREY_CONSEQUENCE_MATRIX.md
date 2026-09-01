# PREDATOR_PREY_CONSEQUENCE_MATRIX.md — Plan 28 Task 28AA

**Status: partially live; extension designed and capped.**

## Live today (pre-Plan 28, verified)

| Prey signal | Predator/pressure effect | Cap | Reversible |
|---|---|---|---|
| Global population ratio < 0.4 | expedition encounter chance ×1.15 (desperate country) | fixed 1.15 | yes — ratio recovers via birth rule |
| Global ratio > 1.2 | ×0.95 (booming country is quiet) | 0.95 floor | yes |
| Any rabid pack | ×1.05 per composition (single fire per check via `break`) | 1.05 | rabies is terminal per pack |
| Starvation > 0.7 | aggression +0.1/day, population thins | aggression ≤ 1.0 | fed ground decays aggression −0.05 |

These are bounded, existing modifiers — Plan 28 does not touch their math.

## Designed extension (filed, requires predator encounter tables from the encounter pass)

| Trigger | Effect | Cap | Cooldown |
|---|---|---|---|
| HerdGrazer pack collapses to ≤ 25% seed while predators hold the same or adjacent sector | predator encounter weighting +0.1 on that sector's table | +0.2 absolute | resets when ratio recovers > 0.5 or 30 days pass |
| BurrowSwarm bloom (Thaw) | vermin nuisance encounters up near granaries | one eligibility bump per window | seasonal |

Cascade budget (28BC): prey decline → **one** encounter modifier → stop. No second-order
predator starvation loop; predator packs already lose members to starvation in the same tick
that bounds their numbers. Field-guide clue: silent birdsong before bold predator sign
(28AG entry) — unlocked through observation, never an omniscient readout.
