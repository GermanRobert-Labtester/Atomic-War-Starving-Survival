# PERF SPRINT 1 — Runtime baseline suite, CI regression gate, day profile, frame profiler

STATUS: APPROVED BY USER

> **Approval basis:** user's performance program brief (2026-10-02), which names
> this exact five-task first sprint and its definition of done. This plan covers
> the bounded, verifiable slice of that sprint that does not require a new
> architecture decision. Tasks 6/7/8 (dirty-state scheduling, survivor-social,
> Utility AI) are deliberately deferred to a follow-on package because they
> change live Core authorities and need their own premise audit.

## 1. Goal & Outcome

Advance the existing Task-130 performance harness from an aggregate-only,
24-survivor proxy into a **repeatable, machine-readable baseline suite** and
protect it with a **committed regression baseline + CI gate**:

1. **Per-system day profile (Task 4):** the campaign-day harness currently
   discards per-owner timing (`PerfDayOwner.Record` computes and throws away a
   timestamp). Capture inclusive time, call count, and allocated bytes per day
   owner and rank them.
2. **Large-population soak tiers (Task 9):** add deterministic 50/100/250/500
   survivor workload tiers (today the roster caps at 24 authored defs) and run
   them across several in-game years.
3. **Baseline suite (Task 1):** one Core entry point that runs cold start,
   day advancement, save/load/checksum, large-shelter soak, and long campaign
   replay, returning `PerfResult`s plus the day profile. Host writes
   `artifacts/performance/perf-baseline.json`.
4. **CI regression gate (Task 2):** `scripts/ci/perf-baseline-gate.py` compares a
   measured result file against a committed `docs/ci/PERFORMANCE_BASELINE.json`
   with the repo's 30 % tolerance rule and exits non-zero on breach. Registered
   as a non-critical performance-tier gate.
5. **Frame profiler expansion (Task 3):** `tools/performance/frame_profile.gd`
   gains per-system callback timing, frame-time percentiles, memory/object
   snapshots, and CSV + JSON export.

## 2. Non-Goals

- No change to gameplay, save format, determinism, or any Core authority.
- No dirty-state scheduling, survivor-social, or Utility AI rewrite (deferred).
- No Rust conversion; no new language.
- No UI-navigation benchmark (host panel lifecycle is already exercised by
  `player_panels_uitest`; a truthful benchmark needs a separate host package).
- No commit, no ledger status flip; handoff only.

## 3. Claimed Paths & Affected Files

- `Assets/Ashfall.Core/Performance/PerfDayProfile.cs` (new)
- `Assets/Ashfall.Core/Performance/PerformanceBaselineSuite.cs` (new)
- `Assets/Ashfall.Core/Performance/ScaleTier.cs`
- `Assets/Ashfall.Core/Performance/WorkloadProfile.cs`
- `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`
- `src/Host/PerformanceSelfTest.cs`
- `tools/performance/frame_profile.gd`
- `scripts/ci/perf-baseline-gate.py` (new)
- `docs/ci/PERFORMANCE_BASELINE.json` (new)
- `docs/ci/CI_GATE_MANIFEST.json` (additive gate row only)
- `Ashfall.Core.Tests/Performance/PerformanceBaselineSuiteTests.cs` (new)
- `.ai/plans/perf-runtime-baseline-sprint-2026-10-02.md` (this plan)
- `WORKTREE_OWNERSHIP.md` (claim row), `.ai/state.md` (handoff)

**Explicitly untouched:** the concurrently-dirty `scripts/ci/run-gates.py`
path-filter change and the `compiler_warning_baseline` `paths` key already
present in the manifest; all live Core gameplay/save/UI paths.

## 4. Pre-flight Checks

- [x] Existing harness/authority inspected (`PerfSession`, `PerfResult`,
      `PerfStatistics`, `PerformanceCampaignHarness`, `PerformanceSelfTest`).
- [x] No equivalent existing baseline suite or runtime regression gate found.
- [x] Premise verified: `PerfDayOwner.Record` discards its timestamp; roster
      caps at 24; no committed runtime baseline file exists.

## 5. Implementation Steps

1. Add `PerfDayProfile` (per-owner inclusive time / calls / allocated bytes).
2. Extend `ScaleTier`/`WorkloadProfile` with 50/100/250/500 tiers; make
   `SeedRoster` synthesize deterministic defs beyond the authored 24.
3. Record per-owner timing in `PerformanceCampaignHarness`; expose the profile.
4. Add `PerformanceBaselineSuite` (cold start, day advance, save/load/checksum,
   large-shelter soak, long replay) returning results + profile.
5. Wire `PerformanceSelfTest` to write `artifacts/performance/perf-baseline.json`.
6. Add `perf-baseline-gate.py` + `docs/ci/PERFORMANCE_BASELINE.json` + manifest row.
7. Expand `frame_profile.gd`.
8. Add focused `PerformanceBaselineSuiteTests`.

## 6. Verification

- [x] `scripts/run_test.sh Ashfall.Core.Tests/Performance/PerformanceBaselineSuiteTests.cs` — **9/9 PASS** (451 ms)
- [x] `scripts/run_test.sh Ashfall.Core.Tests/Performance/` — **61/61 PASS** (9 s)
- [x] `dotnet build Ashfall.csproj --no-restore` — **0 warnings / 0 errors** (86.8 s)
- [x] `python3 scripts/ci/perf-baseline-gate.py --check-only` — **PASS**
- [x] synthetic breach → `PERF_BASELINE_GATE FAIL` exit 1 (ratio 3.92×, delta +3.723 ms)
- [x] missing artifact → `PERF_BASELINE_GATE PASS (no measured artifact)` exit 0
- [x] real host artifact → **PASS (12 checked, 0 no-reference)**
- [x] `python3 -m py_compile scripts/ci/perf-baseline-gate.py` — clean
- [x] `godot --headless --check-only --script tools/performance/frame_profile.gd` — exit 0
- [x] `run-gates.py --check-only` — valid (75 gates, 70 fast); `--check-inventory` PASS
- [x] host `--runtime-scale-selftest` end-to-end — **RUNTIME_SCALE_SELFTEST PASS** + `artifacts/performance/perf-baseline.json` (12 results)
- [x] no save/determinism change (no Core authority touched; large-roster determinism test added)

### Measured output (host, 2026-10-02)

| Benchmark | median ms | p95 ms | median alloc B |
|---|---:|---:|---:|
| cold_start | 0.039 | 0.138 | 23,480 |
| day_advance_30d | 1.165 | 1.300 | 84,160 |
| day_advance_180d | 7.225 | 7.369 | 499,920 |
| day_advance_360d | 16.343 | 18.374 | 1,002,888 |
| save_30d | 0.086 | 0.165 | 27,664 |
| load_30d | 0.026 | 0.096 | 1,288 |
| checksum_30d | 0.011 | 0.061 | 11,608 |
| large_shelter_50 | 0.063 | 0.106 | 2,784 |
| large_shelter_100 | 0.117 | 0.123 | 2,784 |
| large_shelter_250 | 0.510 | 0.521 | 2,784 |
| large_shelter_500 | 1.716 | 1.769 | 2,784 |
| long_replay_720d | 31.895 | 36.105 | 2,008,080 |

Campaign-day owner ranking (360d run, 2,163 owner ticks): `perf_survivors` 22.45 ms,
`perf_weather` 14.84 ms, `perf_journal` 13.65 ms, `perf_world` 11.84 ms,
`perf_expeditions` 11.30 ms.

## 7. Known limitation / deferred

- The Core harness is a **structural proxy**: five synthetic owners; its survivor
  loop is a liveness scan, not the full needs/social/Utility-AI stack. The
  large-shelter numbers measure coordinator/owner overhead scaling only. A
  truthful 50–500 survivor soak against the real systems needs a host package
  that builds a real campaign roster and registers the real day owners.
- Sprint items 6/7/8 (dirty-state scheduling, survivor-social optimization,
  Utility AI optimization) are **not** in this package; they change live Core
  authorities and need their own premise audit and claims.
- Save/load profiling here measures the Core envelope path; the host save
  orchestrator's compression/disk-write phases and an incremental-save path are
  a follow-on package.
- UI navigation timing is not benchmarked here; `player_panels_uitest` already
  covers lifecycle correctness.
