# ASHFALL Expansion & Integration Program — Wave 19 (2026-09-21)

Twenty new plans (261–280) — a **file-level** wave. After eighteen waves the
authority-type pool is nearly exhausted (39 left, mostly owned elsewhere), so
the audit moved down a level: **538 Core files are referenced by no plan body**.
This wave covers the twenty largest families of those files as
consumer/dead-file audits.

**None is a claim.** Foreman owns `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`.

## Plan 1, expanded again (AK–AM)

| File | Size | Content |
|---|---:|---|
| `…_APPENDIX-AK_BLOB_INVENTORY.md` | ~5 KB | the **147-file unreachable blob** beyond the 99 authorities — 246 unreachable files total, by directory |
| `…_APPENDIX-AL_COMPILE_SURFACE.md` | ~1 KB | compile verification: **all 246 ship in the game assembly**; no csproj-only orphans |
| `…_APPENDIX-AM_GENERATORS.md` | ~2 KB | the **14 generators are versioned in-repo** (`tools/generators/`) — every appendix is reproducible |
| **Plan 1 total (A–AM)** | ~500 KB | |

## Wave 19 (261–280) — file-family audits

| # | Plan | Family (unreferenced files) |
|---|---|---|
| 261 | Narrative Family Truth | 87 `Narrative/` catalogs, projections, loaders |
| 262 | Core Root Family Truth | 59 root loaders, demos, tooling, tuning |
| 263 | Medical Family Truth | 29 affliction contracts/handlers/bridges |
| 264 | Survivors Family Truth | 29 stores, gates, genealogy bridge |
| 265 | Shelter Family Truth | 26 process catalogs, cascade rules |
| 266 | Radio Family Truth | 24 (distress excluded — CF-P1 owns that seam) |
| 267 | World Family Truth | 24 map/hazard catalogs, knowledge gate |
| 268 | Factions State Family Truth | 23 state/save pairs, routers |
| 269 | Expedition Family Truth | 20 resolvers, validators, aggregates |
| 270 | Economy Data Family Truth | 19 trade catalogs, types, event results |
| 271 | Inventory Family Truth | 17 migrator, transactions, port contracts |
| 272 | Campaign Family Truth | 14 calendar, briefing, provenance |
| 273 | Combat Family Truth | 12 tactical AI, catalogs, partials |
| 274 | Content Acceptance Family Truth | 12 gates for content QA |
| 275 | Muster Family Truth | 11 path evaluator, warfare types |
| 276 | MoralChoice Loader Family Truth | 11 loaders/data with invalid fixtures |
| 277 | UI Contract Family Truth | 10 registry/manifest/contract plumbing |
| 278 | Foundry Family Truth | 9 SilentFoundry partials + catalogs |
| 279 | Performance Harness Family Truth | 9 session/statistics/timing files |
| 280 | Trio Family Truth | 18 crafting + journal + disease support files |

## Programme state (276 plans)

| Waves | Plans | Chars |
|---|---|---|
| 1 | 6 + Appendices A–AM | ~500 KB |
| 2–15 | 10/10/10/10/20/15×9 | ~1,000 KB |
| 16–18 | 15/20/20 | ~230 KB |
| 19 | 20 | this wave |

Counts are indicative; Plan 100 replaces README tables with a generated index.

## Ordering

- **261, 262** first (largest families, both at the root of the data graph).
- **263, 264** (handler/store architecture) before **265–268** (data wiring).
- **271** before any save-touching work: migrator/transaction correctness is a precondition.
- **274** is tooling: land it before trusting content gates in CI.
- **266** must carry its distress exclusion in the claim.
- **273, 279** pair with Plans 62/16 (AI determinism, measurement truth).
- **277** consumes Plan 15's route inventory; **280** is three small packages.

## Standing rules

Core engine-free; one authority per concern; JSON authority with live
consumers; seeded RNG with replay equality; typed errors and visible failure;
provenance for content; focused verification only; fictional original content.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
