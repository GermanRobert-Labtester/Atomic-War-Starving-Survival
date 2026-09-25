# ASHFALL Test Policy

Tests protect current behavior; they are not a production-count target. This
policy supersedes conflicting active test allowances elsewhere.

## Core Rule: Do Not Run Every Test Every Edit

Run only tests directly related to files changed during the current task. Run the full suite only after a completed feature, before a commit, or in CI. Never create duplicate tests that assert the same behavior through slightly different wording. If an agent creates thousands of tests and executes all of them after each small edit, that is inefficient regardless of language.

### Test Execution Hierarchy

| Tier | Duration | Scope & Cadence |
|---|---|---|
| **Fast** | < 30 seconds | Changed-module tests only; run on each edit |
| **Medium** | < 5 minutes | Subsystem tests before feature commit |
| **Full** | 10–60+ minutes | All tests before merge / release / nightly CI |
| **Stress** | Long-running | Fuzzing, simulations, soak tests overnight |

### 10 Non-Negotiable Testing Policy Rules

1. **High-Signal Coverage:** Prefer 3–10 high-signal tests per behavior over dozens of near-duplicate tests.
2. **Parametrization:** Reuse parameterized tests instead of generating one test file per input.
3. **No Trivial/Internal Tests:** Do not test third-party library internals, trivial getters/setters, or framework behavior.
4. **Test Categorization:** Mark tests as `fast`, `integration`, `slow`, or `e2e` (in Python/pytest) or appropriate focused traits (in xUnit).
5. **No Full Suites on Edit:** Do not run the full test suite after every edit.
6. **Targeted Execution:** Run only tests affected by changed files, then report the exact command and result.
7. **Determinism:** Use deterministic seeds; never use arbitrary `sleep()` calls.
8. **Isolation in Fast Tests:** Mock network, disk-heavy assets, time, and external processes in fast tests.
9. **Deduplication Check:** Check for an existing equivalent test before adding a new test.
10. **Hygiene:** Delete or merge redundant generated tests when found.

## Selection

- Builders run the modified test file and directly affected regional tests,
  normally below 100 cases (< 30s).
- The integrator deduplicates affected targets after packages combine.
- Sweep agents prefer static evidence and run tests only to reproduce one
  suspected defect.
- Full-suite, soak, or release checks require an explicit user or foreman
  reason and a dedicated execution window. They are never a default response.
- Use `scripts/run_test.sh <file-or-focused-directory>` for xUnit targets; it
  applies the 180-second limit and rejects excluded targets.
- For Python/pytest targets:
  - Fast default after edit: `pytest -n 4 -m fast tests/...` (or `PYTEST_DISABLE_PLUGIN_AUTOLOAD=1 pytest -p pytest_xdist.plugin -n 4 -m fast ...`)
  - Subsystem before commit: `pytest -n 4 -m "not slow and not e2e"`
  - Full suite: `pytest -n 4`
  - Stress suite: `pytest -n 2 -m "slow or e2e"`
  - Profiling slow tests: `pytest --durations=10`, `pytest --collect-only`, `python -X importtime -m pytest --collect-only 2> import-time.log`

## When to create a test

Create one only when it covers an uncovered confirmed defect, a new public
contract, save/load, determinism, lifecycle, mutation, state transition, or a
cross-system consequence. Reuse and extend an existing test when that retains
clear diagnostics.

Do not create a test solely because a plan names a feature, a catalog has more
rows, a compile error exists, or a deprecated API would make the old test pass.

## Aggregation

Aggregate homogeneous static mappings, labels, thresholds, and catalog checks
when each failed row is reported clearly. Keep save/load, determinism,
lifecycle, state-transition, mutation, fuzzing, and cross-system workflows
independently reported.

Use `// TEST-AGGREGATION: source_rows=N aggregate_cases=M saved_cases=K`
metadata when a consolidation deliberately reduces homogeneous rows. The
targeted runner reports this reduction.

## Quarantine and re-enable

A source may be quarantined only with a reason, current API/content evidence,
owner role, restoration condition, and preserved path/hash when moved out of
the active project. Re-enable only after the current contract compiles and its
focused target passes. A green compile alone is not re-enable evidence.

## Reporting

Report command, result, selected tests, and known limitation. Summarize failures
and point to stored logs; do not paste large raw test output into handoffs.

## Tooling Authority for Testing and Development

- AI agents must execute and create tools for the 8 core development tasks strictly in Go (`.go` via `bin/ashfall-dev` / `tools/gotools`):
  1. Repository file indexer (`ashfall-dev index`)
  2. Changed-file / changed-test selector (`ashfall-dev select-tests`)
  3. Test-result parser (`ashfall-dev parse-results`)
  4. Fast JSON/YAML validator (`ashfall-dev validate-json`)
  5. Save-file scanner (`ashfall-dev scan-saves`)
  6. Asset manifest builder (`ashfall-dev build-manifest`)
  7. Parallel subprocess/task runner (`ashfall-dev run-tasks`)
  8. LLM API proxy/router (`ashfall-dev llm-proxy`)
- Do not launch Python processes for these 8 concerns when Go tooling is authoritative.
