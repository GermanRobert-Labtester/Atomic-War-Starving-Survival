# ASHFALL — RUNTIME PERFORMANCE BUDGETS (PLAN 26C / TASK 130)

**Authority:** `docs/perf/BUDGETS.md`
**Enforcement:** `godot --headless --path . -- --runtime-scale-selftest` (Gate: `runtime_scale_performance`)
**Artifact Output:** `artifacts/runtime-scale-results.json`
**Date:** 2026-09-17

---

## 1. Runtime Scale Budgets (Headless Simulation)

The simulation budgets below are enforced by `--runtime-scale-selftest` across multi-day horizons.
Measurements are taken across 5 iterations with a 3-day warmup phase.

| Benchmark ID | Horizon | Workload Tier | Median Latency Budget | Per-Day Allocation Budget | Retained Memory Budget | Status |
|---|---|---|---|---|---|---|
| `day_advance_30d` | 30 Days | Normal (24 roster, 30 journal, 1 exp) | **< 2,000 ms** (Target: < 500 ms) | — | — | PASS |
| `day_advance_180d` | 180 Days | Large (48 roster, 180 journal, 3 exp) | **< 12,000 ms** (Target: < 3,000 ms) | — | — | PASS |
| `day_advance_360d` | 360 Days | Stress (96 roster, 360 journal, 6 exp) | **< 30,000 ms** (Target: < 8,000 ms) | — | — | PASS |
| `save_30d` | 30 Days | Normal | **< 500 ms** (Target: < 400 ms) | — | — | PASS |
| `alloc_growth_30d` | 1 Day | Normal (per-day tick) | — | **< 5,000,000 bytes** (Target: < 1,000,000 B) | — | PASS |
| `lifecycle_leak_30d` | 30 Days | Retained Memory | — | — | **< 20,000,000 bytes** (< 20 MB) | PASS |

---

## 2. Telemetry Record

Current headless Linux test run measurements (2026-09-17):
- `day_advance_30d`: ~1.0 ms median (budget: < 2,000 ms)
- `day_advance_180d`: ~7.0 ms median (budget: < 12,000 ms)
- `day_advance_360d`: ~12.7 ms median (budget: < 30,000 ms)
- `save_30d`: ~26.7 ms median (budget: < 500 ms)
- `alloc_growth_30d`: 2,640 bytes median per day (budget: < 5,000,000 bytes)
- `lifecycle_leak_30d`: 0 MB retained memory (budget: < 20 MB)

All simulation workloads operate well inside the allocated budget bounds.
