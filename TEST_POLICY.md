# ASHFALL Test Policy

Tests protect current behavior; they are not a production-count target. This
policy supersedes conflicting active test allowances elsewhere.

## Selection

- Builders run the modified test file and directly affected regional tests,
  normally below 100 cases.
- The integrator deduplicates affected targets after packages combine.
- Sweep agents prefer static evidence and run tests only to reproduce one
  suspected defect.
- Full-suite, soak, or release checks require an explicit user or foreman
  reason and a dedicated execution window. They are never a default response.
- Use `scripts/run_test.sh <file-or-focused-directory>` for xUnit targets; it
  applies the 180-second limit and rejects excluded targets.

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
