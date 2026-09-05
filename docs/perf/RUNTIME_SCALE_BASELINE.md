# ASHFALL — RUNTIME SCALE & PERFORMANCE BASELINE (PLAN 82 / TASK B23)

**Date:** 2026-09-03
**Classification:** Performance & Scaling Baseline
**Environment:** Linux x86_64, .NET 8 Host / .NET 9 Tests, Godot 4.7.1 Headless
**Data Artifact:** `artifacts/runtime-scale-results.json`

---

## 1. Executive Summary

As part of the Flagship Hardening wave, the runtime scale performance suite (`--runtime-scale-selftest`) measures campaign simulation over 30, 180, and 360-day horizons. The baseline captures day-advance latency, heap allocation per daily tick, campaign envelope serialization latency, and post-lifecycle retained memory leaks.

---

## 2. Baseline Benchmark Telemetry

| Benchmark ID | Campaign Horizon | Scale Tier | Median Latency | Median Allocations | Initial Gate Threshold |
|---|---|---|---|---|---|
| `day_advance_30d` | 30 Days | Normal (24 roster, 30 journal, 1 exp) | 0.85 ms | 72,160 bytes | < 2,000 ms |
| `day_advance_180d` | 180 Days | Large (48 roster, 180 journal, 3 exp) | 4.02 ms | 435,128 bytes | < 12,000 ms |
| `day_advance_360d` | 360 Days | Stress (96 roster, 360 journal, 6 exp) | 8.92 ms | 858,720 bytes | < 30,000 ms |
| `save_30d` | 30 Days | Normal | 38.04 ms | 168,648 bytes | < 500 ms |
| `alloc_growth_30d` | 1 Day | Normal (per-day tick) | 0.025 ms | 2,400 bytes | < 5,000,000 bytes |
| `lifecycle_leak_30d`| 30 Days | Retained Memory | — | 0 MB retained | < 20 MB retained |

---

## 3. Hot-Path Analysis & Allocation Bottlenecks

1. **CampaignDayCoordinator View Allocations**:
   - `CampaignDayCoordinator.Owners` allocated a new `List<IDayAdvanceOwner>` on every property invocation, causing heap churn during multi-phase tick inspections.
   - `DayOwnerReport` retained newly instantiated empty `List<DayStateChangeEvent>` objects for quiet systems that reported zero daily events.
2. **Predicate Delegate Allocations in Simulation Loops**:
   - `LocationEvolutionSystem.GetOrCreateRecord` and `TryGetRecord` invoked `_state.mutations.Find(m => string.Equals(...))` allocating predicate closures on every location access.
   - `WildlifeMigrationSystem.MigratePack` and `TryGetPack` allocated predicate closures on every pack lookup.
   - `SurvivorRosterSystem.RegisterDefinition` used `_catalog.Exists(d => d.id == def.id)`, allocating a predicate instance per definition.
   - `SurvivorRosterSystem.CaptureState` allocated a new lambda delegate on every call to `ordered.Sort((a, b) => ...)`.
3. **Threshold Slack**:
   - The initial thresholds (2s for 30d, 12s for 180d, 30s for 360d, 5MB per tick) were overly permissive and would fail to catch regressions in production PRs.
