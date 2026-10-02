# ASHFALL — Performance Diagnostic (2026-10-02)

**Diagnostic phase only.** No production code was modified and nothing was
converted to Rust. This report is evidence-based; every number below is either a
fresh measurement taken this session or an existing recorded artifact, and each
is labelled with its source and its measurement-quality caveats.

## 1. Executive summary

- **There is no confirmed frame-time bottleneck on real hardware.** The recorded
  budget (`docs/health/PERF_BUDGET_2026-09-25.md`) measured idle pacing at
  **59.9 FPS (vsync 60)** with an order of magnitude of headroom on every
  simulation budget, and its verdict was explicitly *"No proven bottleneck; no
  fix applied."*
- **The one saturated-looking frame artifact is not a runtime bottleneck:** the
  2026-09-27 capture behind `artifacts/performance/three-plan-2026-09-27/` ran on
  **llvmpipe (software rasterizer)** at a 15 FPS cap and includes engine startup
  (`first_frame_engine_ms = 11.57 s`). Its p95/p99 spikes are environmental.
- **The largest measured, high-frequency costs are in the build/test/tooling
  loop (objective #11), not the frame loop.** Fresh measurements this session:
  host build **74.0 s**, Core-tests build **41.8 s**, docs-index check
  **67–215 s**, plan-register check **31.8 s**. These are paid on every sweep,
  integration, and commit — i.e. exactly the "agents taking ages" complaint.
- **Runtime candidates worth watching (not critical):** day-advance allocates
  **0.5–1.0 MB per advance**, and the menu `UiBackgroundCarousel._Process` runs
  **every frame** (2749/2749). Neither is in the hot per-frame gameplay path.

## 2. Configuration

| Item | Value |
|---|---|
| Engine | Godot 4.7 (.NET) |
| Build targets | host `Ashfall.csproj` (net8.0) · Core `Assets/Ashfall.Core` · tests `Ashfall.Core.Tests` (net9.0) |
| OS / arch | Linux x86_64 |
| Build mode | Debug (dev), `ASHFALL_BUILD_FAST=1`, `--no-restore` |
| Renderer (frame artifact) | `llvmpipe` (software) — NOT representative |
| Concurrency | Multiple agent sessions active → timings noisy (see §6) |
| Target frame rate | 15 FPS in tests (project rule); 60 Hz vsync in play |

## 3. Baseline measurements

### 3a. Build / test / tooling (fresh, this session)

| Stage | Command | Time |
|---|---|---|
| Host build | `dotnet build Ashfall.csproj --no-restore` | **74.0 s** |
| Core-tests build | `dotnet build Ashfall.Core.Tests/...csproj --no-restore` | **41.8 s** |
| Docs index check | `generate-docs-index.py --check` | **67–215 s** (load-dependent; 215 s before plan/ledger exclusion, 67–126 s after) |
| Plan register check | `generate-plan-register.py --check` | **31.8 s** |
| Docs corpus size | 4159 indexed files, **~4.17 GB** of markdown |

### 3b. Runtime simulation (`artifacts/runtime-scale-results.json`, seed 9001)

| Benchmark | Iters | Median | P95 | Max | Median alloc |
|---|---|---|---|---|---|
| `day_advance_30d` | 5 | 1.28 ms | 2.61 ms | 2.94 ms | 84 KB |
| `day_advance_180d` | 5 | 9.09 ms | 19.68 ms | 22.30 ms | 507 KB |
| `day_advance_360d` | 5 | 16.28 ms | 16.72 ms | 16.76 ms | **1.00 MB** |
| `save_30d` | **2** | 32.61 ms | 61.44 ms | 64.64 ms | 160 KB |
| `alloc_growth_30d` | 5 | 0.046 ms | 0.101 ms | 0.114 ms | 2.8 KB |

### 3c. Presentation

| Measurement | Result | Source / caveat |
|---|---|---|
| Idle pacing (real display) | avg 59.9 FPS (min 58, max 61) | PERF_BUDGET 2026-09-25 |
| Boot wall (headless) | 3.51 s | PERF_BUDGET 2026-09-25 |
| Retained memory | 352 MB headless · 768 MB display | PERF_BUDGET 2026-09-25 |
| PCK | 128.4 MB (lossy art) | re-baseline 2026-09-26 |
| Frame ms (llvmpipe, incl. startup) | mean 109.1 · p50 66.8 · p95 286.1 · p99 385.4 · max 539.7 | three-plan 2026-09-27 — **environmental** |
| Startup engine→1st process | 11,588 ms | three-plan 2026-09-27 — **llvmpipe** |
| Draw calls | mean 41.9 | three-plan 2026-09-27 |

## 4. Bottlenecks ranked (gain × frequency × confidence ÷ cost)

1. **B-01 — Host build 74 s** · *Critical (dev loop)* · confidence High.
   Paid on every build in every sweep; dominates agent wall-clock.
2. **B-02 — Docs-index check 67–215 s** · *High* · confidence High.
   Long-running, deterministic, plain-data; ~4 GB corpus scan.
3. **B-03 — Plan-register check 32 s** · *High-Medium* · confidence High.
4. **B-04 — Core-tests build 42 s** · *High for test loop* · confidence High.
5. **B-05 — Day-advance allocation 0.5–1.0 MB/advance** · *Medium* · confidence
   High (measured); frequency once/day → GC spike, not frame time.
6. **B-06 — `save_30d` variability (0.57 vs 64.6 ms, n=2)** · *Unconfirmed* ·
   measurement-quality problem, not yet a bottleneck.
7. **B-07 — Frame p95/p99 spikes** · *Unconfirmed / likely environmental* ·
   needs a clean GPU run; the recorded run is llvmpipe + startup.
8. **B-08 — `UiBackgroundCarousel._Process` every frame** · *Low* · menu only.

### Bottleneck detail (highest two)

```text
BOTTLENECK ID: B-01
SYSTEM: build tooling
FILE: Ashfall.csproj (+ 82 GameBootstrap/Main partials)
LANGUAGE: C#
WORKLOAD: dotnet build --no-restore (ASHFALL_BUILD_FAST=1)
BASELINE: 74.0 s (measured this session)
SCALING: grows with source surface (god-file/partial count)
CONFIDENCE: High
ROOT CAUSE: C# compile of a very large single-project surface; per-sweep cost
PROPOSED FIX: reduce recompilation surface / assembly split; keep analyzers-off
RUST CANDIDATE: no (compiler-bound)
EXPECTED IMPROVEMENT: unknown until incremental-vs-clean split is measured
TEST PLAN: time clean vs incremental builds, 3 runs
ROLLBACK: n/a (measurement only)
```

```text
BOTTLENECK ID: B-02
SYSTEM: CI docs index
FILE: scripts/ci/generate-docs-index.py
LANGUAGE: Python
WORKLOAD: --check over ~4159 files / 4.17 GB
BASELINE: 67–215 s (215 s pre-fix; 67–126 s after plan+ledger exclusion)
P95/P99: not captured separately; wide load variance observed
ALLOCATIONS: ~4.17 GB read per run
CONFIDENCE: High
ROOT CAUSE: full-corpus read + per-document scan every run; no cache
PROPOSED FIX: move to Rust (long-running check → Rust per policy); Rust can
  read/hash the corpus in seconds; optional content cache
RUST CANDIDATE: YES (deterministic, plain data, no engine coupling, batch)
EXPECTED IMPROVEMENT: order-of-magnitude (est. <10 s)
TEST PLAN: byte-identical docs/INDEX.md vs Python output
ROLLBACK: keep Python generator until parity is proven
```

## 5. Recommended C# optimizations (no language change)

1. **Reduce day-advance allocation (B-05).** 0.5–1.0 MB/advance; reuse buffers /
   avoid per-advance list+string churn. Measure before/after with
   `--runtime-scale-selftest`.
2. **Fix the save benchmark measurement (B-06)** before optimizing it: add warmup
   and ≥5 iterations (currently 2, no warmup), then re-measure.
3. **Throttle `UiBackgroundCarousel._Process` (B-08)** to a fixed cadence (it is a
   menu background; it does not need every frame).
4. **Audit LINQ in the day-tick path** (475 LINQ call sites exist repo-wide; most
   are self-test/UI — confirm which, if any, are on the per-day path before
   touching anything).

## 6. Measurement-quality fixes required first (Phase 1)

- Frame capture must run on a **GPU display**, exclude startup frames, and record
  the renderer string (the 2026-09-27 run fails all three).
- `save_30d` needs warmup + ≥5 iters.
- Label and separate **cold vs warm** startup.
- Record that **concurrent agent sessions** load the machine: the same
  docs-index command measured 67 s, 126 s, 184 s, and 215 s across runs today.

## 7. Rust conversion candidates (evidence-gated, NOT started)

| Candidate | Why | Fit |
|---|---|---|
| docs-index checker (B-02) | long-running, deterministic, plain data, batch | **strong** |
| plan-register generator (B-03) | same profile | **strong** |
| Go dev toolchain | user-directed port already underway (Stages 1–2 done/in progress) | **approved** |
| save-file validation / large deterministic sims | heavy, deterministic, no engine objects | medium |

**Keep in C#:** all gameplay/domain/UI, and the day-advance simulation itself
(once/day, ~16 ms, engine/catalog-coupled) — reduce its allocations instead.
**Keep in Python:** CI orchestration, timing/report analysis (short-lived) — but
long-running ones become Rust per policy. **Keep in GDScript:** nothing today;
reserved for small Godot glue.

## 8. Regression protection (already landed)

`docs/ci/TIMING_BASELINE.json` + `scripts/ci/timing-budget.py` + the non-fatal
`[TIMING BUDGET]` warning in `run-gates.py`, with the rule **≤30 % over tolerable,
>30 % investigate and optimize**. This is the automated regression guard for the
budgets in §3.

## 9. Implementation order

1. Phase 1 — measurement quality: fix frame capture (GPU), save-benchmark
   warmup/iters, record hardware + concurrency.
2. Phase 2 — C# wins: day-advance allocations, carousel cadence.
3. Phase 3 — tooling: Rust docs-index + plan-register (parity-tested).
4. Phase 4 — Rust conversions only for confirmed heavy workloads.
5. Phase 5 — extend `TIMING_BASELINE.json` to every budgeted stage.

## 10. Commands used and durations (this session)

| Command | Duration |
|---|---|
| `dotnet build Ashfall.csproj --no-restore` | 74.0 s |
| `dotnet build Ashfall.Core.Tests/... --no-restore` | 41.8 s |
| `generate-docs-index.py --check` | 67–215 s (varies) |
| `generate-plan-register.py --check` | 31.8 s |
| `run-gates.py --check-only` | < 3 s |
| `python3 -m py_compile` ×4 scripts | < 1 s |

## 11. Decision

**No Rust conversion is approved by this report.** The only artifact-level red
(B-01…B-04) is build/test tooling, and the runtime reds (B-06/B-07) are
unconfirmed due to measurement-quality gaps. The next step is Phase 1
(measurement quality) and the Phase 2 C# allocation/cadence wins; the Rust port of
the docs-index/plan-register checkers is the strongest language-level candidate
but requires the parity plan already used for the Go toolchain port.

---

## PERF-001 result (implemented 2026-10-02) — host build time

**Measured root cause:** `CoreCompile`/`Csc` = **112.6 s of a 116.9 s incremental
host build (96 %)**; every other target/task < 400 ms. The host compiles
**1383 `src/` files (271,530 LOC) + `Assets/Ashfall.Core` (1232 files, 344,648
LOC) as one ~616 k-LOC assembly**, so any single-file edit recompiles the whole
assembly (a no-op build is 3.3 s; touching one file costs 98.7–131.2 s).

**Rust conversion: ABORTED — infeasible.** Rust cannot compile C#; the diagnostic
never approved Rust for B-01 (`RUST CANDIDATE: no (compiler-bound)`), so the
per-runbook guard is satisfied by aborting.

**Implemented lever (opt-in, behavior-preserving):** `ASHFALL_BUILD_LEAN=1`
excludes the same self-test/harness sources the shipping `ExportRelease` build
already excludes (281 files / 54,226 LOC).

| | Time (incremental, one file touched) |
|---|---|
| default (`ASHFALL_BUILD_FAST=1`) | 98.7 s · 116.9 s · 131.2 s (Csc 112.6 s) |
| lean (`ASHFALL_BUILD_LEAN=1`) | **81.5 s · 70.4 s** |
| reduction | **~24–46 %** (deterministic — fewer files compiled) |

Default build unchanged; lean is for gameplay-iteration compile checks, not the
test loop (which needs the self-tests). **Structural fix (needs separate
approval):** compile `Assets/Ashfall.Core` as its own assembly instead of into
the host, so a host edit no longer recompiles ~345 k Core LOC.

## Phase 1 result (implemented 2026-10-02) — measurement quality

`save_30d` (`src/Host/PerformanceSelfTest.cs`) took **n=2, no warmup**, and mixed
save-latency with payload capture. Fixed to **1 warmup + 5 measured save-latency
samples**.

| | Result |
|---|---|
| before | n=2, no warmup: 0.57 ms vs 64.6 ms → median 32.6 ms (all first-run cost) |
| after | warmup=1, iters=5: **median 0.057 ms, p95 0.161, max 0.184** — true steady state |
| selftest | `RUNTIME_SCALE_SELFTEST PASS` (6/6) |

**Still open (environmental, not code):** frame capture must run on a **GPU
display**, exclude startup frames, and record the renderer string — the only
remaining Phase 1 gap, and it cannot be closed from inside the code.