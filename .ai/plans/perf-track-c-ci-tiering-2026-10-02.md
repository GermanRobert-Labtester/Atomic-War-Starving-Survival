# Perf Track C — Remote CI Tiering (2026-10-02)

> **STATUS: APPROVED BY USER** — authorized by the Master Performance Prompt v3
> (Track C) handed to this session, plus the user's explicit "You can work on
> this!", "Proceed with all the other agent has finished!" and "continue!"
> (2026-10-02).

## Goal

Stop the PR CI path from paying the full core xUnit suite on top of the fast
tier. The PR path keeps the fast tier; the full core suite moves to merge
(`push` to `main`/`master`) plus a new nightly workflow.

## Premise (verified before change)

- `.github/workflows/ci.yml` ran **both** `run-gates.py --tier fast` **and** an
  always-on `run-gates.py --gate test_core_suite` on every `push` and
  `pull_request` to `main`/`master`.
- Committed `docs/ci/CI_GATE_MANIFEST.json` (read at `HEAD`, not the dirty
  working copy): `test_core_suite` is `classification: full`, `critical: true`,
  `release_required: true`, command `dotnet test … --nologo --no-build`,
  `depends_on: ["build_core_tests"]`. `build_core_tests` is `classification: fast`.
  So the fast tier does **not** contain the suite — the extra step was the only
  place it ran, i.e. genuine per-PR duplication of cost, not of coverage.
- `scripts/ci/run-gates.py` expands `--gate <id>` to include transitive
  `depends_on` prerequisites (line ~410), so a standalone nightly needs no
  Godot setup: `test_core_suite` pulls in `build_core_tests` (dotnet only).
- No nightly workflow existed (`build.yml`, `ci-autogen-cleanup.yml`, `ci.yml`,
  `docs-regen.yml`, `hotfix.yml`, `release.yml`,
  `selftest-manifest-regen.yml`, `workflow-lint.yml`).

## Scope (exact paths)

- `.github/workflows/ci.yml` — add `if: github.event_name == 'push'` to the
  existing "Execute Full Core xUnit Regression Gate" step, with a comment.
- `.github/workflows/nightly-core-suite.yml` — new: scheduled + dispatch full
  core suite, annotations, step summary, artifact upload.

## Non-goals

- No change to `docs/ci/CI_GATE_MANIFEST.json` or `docs/ci/GATE_INVENTORY.md`
  (both dirty from a concurrent session; only the committed version was read).
- No change to `scripts/ci/**`, `run-gates.py`, timings, or the baseline.
- No change to `build.yml` (Track D) or to any local gate.
- **Not** enabling `--changed-base` on the PR path (see Proposal).

## Proposal (not applied)

`run-gates.py` gained an opt-in `--changed-base` path filter (`743e1ac96`).
Enabling it on the PR fast tier would cut PR time further but changes gate
coverage semantics per-diff, so it needs owner confirmation and its own
measurement. Left as a proposal.

## Verification

- PyYAML parse of every file in `.github/workflows/` (the only local check
  available — `actionlint` is not installed; `.github/workflows/workflow-lint.yml`
  runs actionlint on a runner for `.github/workflows/**`).
- `ast.parse` of the nightly's embedded Python annotation block.
- Logic dry-run of the annotation block against a synthetic failure report.

## Known limitation

PR-run wall time before/after cannot be recorded locally — GitHub Actions does
not run off-runner. The workflow edit is PyYAML-validated only until a runner
executes it.

## Recovery

Both files are tracked; `git revert` or `git checkout HEAD~1 -- .github/workflows/ci.yml`
restores the previous behavior. No history is rewritten.
