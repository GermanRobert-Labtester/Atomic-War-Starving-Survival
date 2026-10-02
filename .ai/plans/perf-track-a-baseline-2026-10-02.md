# PERF PROGRAM — Track A: budgeted baseline with measurement context

**Owner:** this session (claim `claim-perf-track-a-baseline-2026-10-02`)
**Order:** A → B → D → C → E (this is Track A, one track per session)

## Outcome

Extend `docs/ci/TIMING_BASELINE.json` from 3 entries (2 of which never matched a
gate id) to **every stage ≥ 30 s**, each carrying measurement context, and teach
`scripts/ci/timing-budget.py` to flag peak-RSS breaches with the same
non-fatal `SUSPICIOUS` posture as wall-time breaches.

## Non-goals

- No Rust conversion, no caching, no CI/branch change, no build-config change.
- No behavior change to any gate.
- Do not overwrite the foreign `hardware` block added 05:37 — incorporate it.

## Concrete changes

1. **Fix a real key-mismatch bug.** `run-gates.py::check_timing_budget` looks up
   `budgets[gate_id]`. The existing keys `docs_index_check` / `plan_register_check`
   match no gate id, so only `build_core_tests` was ever checked. Rename the
   docs-index entry to the real gate id `docs_index_drift`.
2. **Schema 1.1** — every entry records: `reference_seconds`, `reference_kind`,
   `observed_seconds {n,min,median,max}`, `peak_rss_mb`, `memory_sensitive`,
   `load_at_start`, `free_mem_at_start_mb`, `threads`, `recorded`, `note`.
3. **`measurement_context`** block (hardware + measurement rule); the foreign
   `hardware` block is folded in and superseded.
4. **All 17 stages ≥ 30 s** get entries.
5. **Peak-RSS budgets** for the memory-sensitive stages, freshly measured:
   - `build_core_tests` 66.2 s / **1,221 MB**
   - `build_godot_host` 72.9 s / **2,295 MB**
   - `compiler_warning_baseline` 190.0 s / **2,273 MB**
6. **`timing-budget.py`** gains `--rss <MB>`; an RSS over the entry's
   `peak_rss_mb` beyond tolerance is reported `RSS_SUSPICIOUS` (non-fatal).

## Evidence / measurement quality

- All wall-clock references are **worst-case / under-load upper bounds**
  (`load_at_start` recorded); quiet-tree medians are authoritative and pending a
  quiet window. Peak RSS is comparatively load-insensitive and recorded directly.
- `build_core_tests` keeps its authoritative `reference_seconds` (41.82); the
  committed 120.6 s worst case is recorded separately, **not** used to raise the
  reference (rule: >30 % = investigate the stage, never raise the reference).

## Verification

- Baseline covers every ≥ 30 s stage (assert in a check).
- A deliberately slowed stage (synthetic `--seconds` over ref) → `SUSPICIOUS`.
- A synthetic RSS breach (`--rss` over budget) → `RSS_SUSPICIOUS`.
- No behavior change; `run-gates.py --check-only` still valid.

## Status

`STATUS: APPROVED BY USER` (program brief section 4, Track A).