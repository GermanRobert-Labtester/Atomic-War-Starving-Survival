# Timing Budget — Reference Durations & the Suspicion Threshold

**Authority:** `docs/ci/TIMING_BASELINE.json` (references) + `scripts/ci/timing-budget.py` (checker).

ASHFALL builds and gates should run in a known duration range. A run that is
**more than 30 % longer than its reference** is the signal that something is
wrong — an undetected compile error, a stale Godot build, a deadlocked
subprocess, or heavy concurrent load — and must be investigated and optimized,
not left to "keep building for literally no reason".

**The rule:** a stage **≤ 30 % over** its reference is tolerable; **> 30 % means
stop, investigate, and optimize the test** — do **not** simply raise the budget.
The default tolerance is **1.30×**, and the boundary itself (exactly 1.30×) is
still `OK`; only strictly more than 1.30× is flagged `SUSPICIOUS`.

## Usage

```bash
# Compare a full gate run (run-gates.py already emits durations)
python3 scripts/ci/run-gates.py --report-json build/reports/gates.json
python3 scripts/ci/timing-budget.py check --measured build/reports/gates.json

# Compare a single ad-hoc stage, e.g. a build
python3 scripts/ci/timing-budget.py check --key build_core_tests --seconds 84.2

# Tighten/loosen once
python3 scripts/ci/timing-budget.py check --measured … --tolerance-ratio 1.30
```

Exit code is `0` when every measured stage is within budget and `1` when at
least one is `SUSPICIOUS`. Stages with no baseline entry are reported
`NO-REFERENCE` and never fail the check — that is how missing references are
discovered.

## Recording references

References are **authored, committed values**, not a live ledger — a single
noisy sample must not silently rebase them (the same discipline as
`MONITORING_POLICY.json`'s growth rule). Record only from a quiet-system run:

```bash
python3 scripts/ci/timing-budget.py update --measured build/reports/gates.json
python3 scripts/ci/timing-budget.py update --key build_core_tests --seconds 56.0
```

## Accepted inputs

`--measured` accepts any of:

- a `run-gates.py --report-json` file (top-level `results[]`, each with
  `gate_id` and `duration`);
- a bare list of such records;
- a flat `{"stage": seconds}` map.

## Why this exists

Measured 2026-10-02: `docs_index_drift` scanned 5955 markdown files (1472 under
`docs/plans/`) in 215 s, on every commit. Excluding the plan corpora (which have
their own register) cut the file count to 4159 and the runtime to ~126 s. The
remaining cost is the multi-GB non-plan corpus; a future change can cache
per-file scan results to push the check under ~30 s. Until then the reference
above makes an abnormal run visible instead of silent.