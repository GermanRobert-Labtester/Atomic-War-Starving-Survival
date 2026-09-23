# ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)

Ten more proposed plans (numbered 11–20), continuing Wave 1 in
`../EXPANSION_PROGRAM_2026-09-21/`. Produced from a read-only audit of HEAD
`5be1a30a`. **None is a claim.** The foreman owns `INTEGRATION_PLANS.md` and
`WORKTREE_OWNERSHIP.md`.

Wave 1 sealed the gaps (orphans, registry, unblocking) and designed the two
gameplay verticals and the launch surface. Wave 2 makes the supporting
disciplines strong enough to survive that scale: governance, saves,
determinism, data, UI, performance, tests, narrative, assets, and release ops.

## The ten plans

| # | Plan | Purpose | Key evidence |
|---|---|---|---|
| 11 | [`PLAN-CORE-ONLY-REGISTRY-11.md`](PLAN-CORE-ONLY-REGISTRY-11.md) | Registry + retirement protocol for Core-only/dead authorities | 99 orphans, 5 dead, no registry exists |
| 12 | [`PLAN-SAVE-GOVERNANCE-12.md`](PLAN-SAVE-GOVERNANCE-12.md) | Section budget, single parity source, migration window, per-family fuzz | 204 sections, 181 stores, matrix red, no compat policy |
| 13 | [`PLAN-DETERMINISM-REPLAY-13.md`](PLAN-DETERMINISM-REPLAY-13.md) | Stream governance, per-vertical replay, golden saves, cross-host equality | ~35 streams, Plans174-177 precedent, 99 new tick sites |
| 14 | [`PLAN-DATA-AUTHORITY-14.md`](PLAN-DATA-AUTHORITY-14.md) | Catalog lifecycle, consumption proof, schema normalization, large-file budgets | 703 JSON, `UNRESOLVED` classes, 21 files >100 KB |
| 15 | [`PLAN-UI-SURFACE-15.md`](PLAN-UI-SURFACE-15.md) | Surface inventory, consolidation map, liveness proofs, accessibility | 192 routes, 69 snapshots, 0 focus usage |
| 16 | [`PLAN-RUNTIME-PERF-16.md`](PLAN-RUNTIME-PERF-16.md) | Composition profile, per-owner tick budgets, measured debloat, soak | 30d median 1.29 s/p95 2.72 s, 226 setups, 18 manifest entries |
| 17 | [`PLAN-TEST-WELFARE-17.md`](PLAN-TEST-WELFARE-17.md) | Suite map, fixture/static-state hygiene, coverage where policy says, warning cleanup | 1,378 test files; a Unity-era file still compiled via `Compile Include` |
| 18 | [`PLAN-NARRATIVE-GRAPH-18.md`](PLAN-NARRATIVE-GRAPH-18.md) | Flag reachability (832 set / 8 read), continuity gate, quest DAG, prose hygiene | `artifacts/narrative-continuity.md` |
| 19 | [`PLAN-ASSET-PIPELINE-19.md`](PLAN-ASSET-PIPELINE-19.md) | Strict asset truth, fallback elimination, audio cue parity, LFS hygiene | 355 assets, strict=false, 12 missing icons, 3,858 LFS objects |
| 20 | [`PLAN-RELEASE-OPS-20.md`](PLAN-RELEASE-OPS-20.md) | Gate reliability census, workflow hygiene, repo health, dependency ops | 57 gates, 6 workflows, 1.8 GB `.git` |

## How Wave 2 fits the programme

```
Wave 1:  KIT(02) → ORPHAN(01) → CULTURE(04)/BODY-IND(05) → FACE(06)
                 ↘ UNBLOCK(03)
Wave 2:  11 governance · 12 saves · 13 determinism · 14 data · 15 UI ·
         16 perf · 17 tests · 18 narrative · 19 assets · 20 release ops
```

Wave 2 plans are mostly **enablers and guardrails**: they do not add gameplay,
they make the gameplay safe to add. Recommended order:

1. **13 + 12** before orphan wiring (determinism and save contracts must exist
   first).
2. **11 + 17 + 14** while wiring runs (registry, test hygiene, data lifecycle).
3. **15 + 16 + 18** alongside the verticals (surfaces, budgets, narrative
   readers).
4. **19 + 20** before any external build (asset truth, release ops).

## Shared rules

- Core engine-free; host adapters own presentation/persistence.
- One authority per concern; extend the owner named in the plan's seam map.
- JSON is authority; generated files come from their generators.
- Seeded RNG only; no `System.Random`/`GetHashCode` in simulation.
- Focused verification (`scripts/run_test.sh`, single CLI verb); no full-suite
  defaults.
- Reports before deletions in ops/hygiene; deletions need evidence and approval.
- No secrets, no real-world wars/countries/people, no copied art or text.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
