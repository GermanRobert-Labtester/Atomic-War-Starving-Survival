# E1B register implementation log

Date: 2026-09-19

E1B adds the canonical, report-only plan register. The shared parser in
`scripts/ci/plan_corpus_lib.py` handles the small front-matter contract,
legacy files, enum normalization, supersedence-cycle detection, and
repo-relative reference verdicts without a third-party YAML dependency.

Delivered:

- `scripts/ci/generate-plan-register.py` with `--write`, `--check`, `--json`,
  `--explain`, `--self-test`, and baseline modes.
- `PLAN_REGISTER.json` as the machine-readable substrate and
  `PLAN_REGISTER.md` as its generated presentation.
- Six governance fixtures covering valid metadata, legacy metadata, stale
  references, supersedence cycles, invalid categories, and unknown rails.
- Three focused xUnit contract tests in
  `Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs`.

Current register measurement: 609 plans across `next_steps` (230),
`piagents` (134), and `integration` (245). Twenty-nine files have front
matter; legacy rows retain stable synthetic `LEGACY-*` identities and
`METADATA_MISSING` status. Invalid authored enum values remain visible in
`status_raw` and diagnostics and are never promoted to an executable state.

Verification:

- `python3 -m py_compile scripts/ci/plan_corpus_lib.py scripts/ci/generate-plan-register.py` — PASS.
- `python3 scripts/ci/generate-plan-register.py --self-test` — PASS.
- `python3 scripts/ci/generate-plan-register.py --write` then `--check` — PASS.
- `python3 scripts/ci/generate-plan-register.py --baseline --check` — PASS.
- `python3 scripts/ci/generate-docs-index.py --check` — PASS (2,543 docs).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` — 3/3 PASS.
- `git diff --check` — PASS.

No Core, host, data, save, scene, or UI implementation path was changed by
E1B. E1C metadata migration remains intentionally unstarted.
