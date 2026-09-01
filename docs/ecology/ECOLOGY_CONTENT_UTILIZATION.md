# ECOLOGY_CONTENT_UTILIZATION.md — Plan 28 Task 28BE

**Status: gate contract + current state. The automated scan reuses
`ContentUtilizationScanner` conventions (catalogs must be opened/read and definitions
consumed) — the scanner's catalog list needs the Plan 28 additions registered when the
content passes land.**

## Reachability contract (flag = orphan)

| Content | Reachable when | Checked by |
|---|---|---|
| species (12) | seeded into a live pack | seed-count gate (13 packs) + archetype table test |
| corridor sectors (11) | every link + pack position resolves | selftest steps 2–3, xUnit graph assertions |
| waterway pair (2) | water flags load; runner constrained | `FishRun_NeverStandsOnDryGround` |
| seasonal windows (6) | every window is reachable in a 360-day campaign | weather season tests (existing) + `SeasonWindowForDay` parity |
| migration notices | archetype has a notice string and at least one pack moves in a year | selftest step "starving pack migrated"; 360-day scenarios |
| radio intercepts | projected by the day owner (live) | existing briefing/radio pipeline |
| trapping link | density multiplier consumed by `CheckTraps` | density gate selftest step |
| market effect | scarcity_goods non-empty and market clamps | selftest step 13 |

## Explicit allowlist (rare-by-design content, reviewed not orphan)

- Rabid-turn warnings (rare RNG, day-stamped — by design).
- Landmark collapse warnings (each landmark collapses at most once).
- Deep-cold fish-run absence (Deep Freeze factor 0.2 is intentional scarcity).

## Deferred-content gate

Species with no route, routes with invalid nodes, events that can never fire, and
infestations with no resolution **fail the gates above** rather than being allowlisted.
The Plan 28 additions ship with zero known orphans: 13 packs ↔ 11 sectors, 12 species ↔
12 archetype assignments, 2 water-flagged sectors, 6 event projections, all validated.
