# ASHFALL Expansion & Integration Program — Wave 8 (2026-09-21)

Fifteen new plans (86–100): **twelve gap-sealing, two major expansion, one
governance**. Plan 1 gained a third expansion round (appendices D–F).

**None is a claim.** Foreman owns `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`.

## Plan 1, expanded again (D–F)

| File | Size | Content |
|---|---:|---|
| `…_APPENDIX-D_SAVE_OWNERSHIP.md` | ~10 KB | capture/restore methods, save-registry knowledge, field counts for all 99 orphans; separates stateful systems (need a save row) from stateless evaluators |
| `…_APPENDIX-E_DETERMINISM_AUDIT.md` | ~2 KB | banned-source audit: only **5 of 99** orphans touch a nondeterministic primitive, each named with counts |
| `…_APPENDIX-F_DEPENDENCY_CLUSTERS.md` | ~4 KB | intra-orphan reference graph: 94 clusters, 89 isolated, suggested referenced-first seal order per cluster |
| **Plan 1 total (A–F)** | ~146 KB | |

## Gap sealing (86–94, 98–99)

| # | Plan | Gap |
|---|---|---|
| 86 | Host CLI Contract | descriptor↔dispatch parity, exit-code contract, generated help, `--cli-manifest`, headless truth probe |
| 87 | Save Migration Corridor | 204 sections, only **5** explicit version ladders: declare the rest, per-section fixtures, forward/backward matrix |
| 89 | Determinism Cross-Host | headless ↔ in-game replay equality proof, divergence bisect, fork-order freeze (complements Plan 13's static gate) |
| 90 | Data Schema Coverage | 703 JSON files, **one** structural schema: schema the consumed catalogs through the existing validator |
| 92 | Mod Content Boundary | precedence, conflict reports, manifest pinning, safe disable, determinism gate over `Mods/` |
| 93 | Inventory Conservation | source/sink ownership, audit deltas, reload duplication probes, transfer edge cases |
| 94 | Deprecated Tree Retirement | `Assets/_Game/` (one file test-only, one uncompiled), empty `src/Bridge/`, marker policy, retirement gate |
| 95 | Spatial Sim Authority | place/territory/route fact ownership, grid semantics, seed-stable placement, read-model guard |
| 96 | Economy Ledger Truth | cross-system reconciliation (orders/barter/caravan/black market); restock ledger explicitly deferred to CF-P5 |
| 98 | Save Integrity Fuzz Operations | coverage matrix over 204 sections, mutation corpus + recipes, recovery paths, bounded rotation |
| 99 | Hotfix Drill | rehearse `scripts/release/hotfix.sh` in a scratch environment: rollback, save-compat matrix, timing |

## Major expansion (88, 91, 97)

| # | Plan | Frontier |
|---|---|---|
| 88 | Text Pack Localization | data-driven content keys, frozen ids, pseudo-locale round, orphan-key gate (Plan 52 extension) |
| 91 | Host Event Archive | bounded per-save fact-event history for post-mortem; replay-independent by guard; consent-gated dump |
| 97 | Audio Mix Authority | bus layout, loudness targets, ducking priority, caption coverage, orphan wiring |

## Governance (100)

| # | Plan | Purpose |
|---|---|---|
| 100 | Programme Closeout | generated programme index (`--check`), promotion ledger, claim cross-check, verification-command audit, sunset rule |

## Programme state (96 plans)

| Wave | Plans | Chars |
|---|---|---|
| 1 | 6 + Appendices A–F | ~168 KB |
| 2 | 10 | ~83 KB |
| 3 | 10 | ~65 KB |
| 4 | 10 | ~62 KB |
| 5 | 10 | ~63 KB |
| 6 | 20 | ~64 KB |
| 7 | 15 | ~72 KB |
| 8 | 15 | this wave |

Table rows are counts, not drift-proof claims — Plan 100 replaces them with a
generated index.

## Ordering

- **87, 94, 90** are one-time cleanups; land early while the tree is quiet.
- **86, 89, 98** strengthen verification; land before the wiring waves grow the surface.
- **92, 95, 96** clarify boundaries; each explicitly defers the seam it does not own.
- **88, 91, 97** are additive surfaces; land with their named dependencies.
- **99** is a rehearsal; schedule before the next real release window.
- **100** executes last, after promotions or retirements are recorded.

## Standing rules

Core engine-free; one authority per concern; JSON authority with live
consumers; seeded RNG with replay equality; typed errors and visible failure;
provenance for content; focused verification only; fictional original content.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
