# BUG-TEST-WARNINGS repair plan

## Bug, reproduction, root cause

The focused Holdfast regression build on 2026-09-20 emitted five warnings:
xUnit2017 at Plan176AnomalyHazardTests.cs:275 and Expansion4IntegrationTests.cs:47/58;
xUnit2000 at Plan20BShieldingBalanceSweepTests.cs:189; xUnit2029 at
CollectibleSaveMigrationTests.cs:173. Assertions use boolean membership checks,
reversed expected/actual values, or an empty filtered sequence.

## Blast radius and invariants

Only four test files change. The same membership, uniqueness, and numeric
conditions remain tested. No production, data, save, RNG, or host changes.
Earlier worktree edits remain intact. Exact paths are in the ownership claim.

## Options and selected repair

Use the matching Contains/DoesNotContain assertion and put the literal expected
value first. Warning suppression is rejected because the direct APIs also give
better failure diagnostics. No new tests are required for these local rewrites.

## Phases and pre-integration checkpoint

Confirm warning locations still match source and no active claim overlaps;
replace only the five assertions; run each affected test file through
`bash scripts/run_test.sh <file>`; verify a fresh test build has zero warnings.

## Rollback and definition of done

Revert only these assertion hunks if needed. Done means all affected tests pass,
the five warnings disappear, and exact outcomes are recorded in the log.
